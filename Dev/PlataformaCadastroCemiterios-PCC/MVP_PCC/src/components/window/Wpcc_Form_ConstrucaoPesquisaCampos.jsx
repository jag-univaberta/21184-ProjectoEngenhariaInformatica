import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';
import { setConstrucoesSelection } from '../../utils/utils'; 
import IconWithLetterWhite from '../icon/IconWithLetterWhite';

const Wpcc_Form_ConstrucaoPesquisaCampos= ({obj_pesquisaconstrucao}) => {
 
  const windowRef = useRef(null);  

  const layoutRef = useRef(null);
  const FormRefgeral = useRef(null);
  const FormRefgeralBotao = useRef(null);
  const windowConstrucaoResultadoRef = useRef(null);   
  const gridConstrucaoResultadoRef = useRef(null);  
  const icon_print = <Icon iconName="Print" />;
  const icon_printHtml = ReactDOMServer.renderToStaticMarkup(icon_print);

  const icon_csv = <div title="Obter CSV"> 
                    <a><IconWithLetterWhite icon="application" letter="CSV" /></a>
                 </div>;

  const icon_csvHtml = ReactDOMServer.renderToStaticMarkup(icon_csv);

  const apiEndpointCadastro= useSelector((state)=> state.aplicacaopcc.config.configapiEndpointCadastro);
  const apiEndpointSIG = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointSIG);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  
  const [datatipoconstrucao, setDataTipoConstrucao] = useState(null); 

  const handleClose = () => { 
    console.log('Close Pesquisa');
    
    var aux={ type: Actions.HIDE_PESQUISA_CONSTRUCAO, payload: "" };
    viewer.dispatch(aux); 
    
  };
  const novoProcessoAPI = () => { 
    console.log('novo Processo');
    
    var aux={ type: Actions.PROCESSOS_NOVOPROCESSO, payload: "" };
    viewer.dispatch(aux);  
  }; 
  const terminouProcessoAPI = () => { 
    console.log('terminou Processo');
    
    var aux={ type: Actions.PROCESSOS_TERMINAPROCESSO, payload: "" };
    viewer.dispatch(aux);  
  };
  const transformResponse = (originalResponses) => {
    
    const transformedData = []; 
    let counter = 1;
    if (Array.isArray(originalResponses)){
      if (originalResponses.length==0){
        transformedData[0] = transformedData[0] || {
          value: "Tipos de Construção",
          id: "update",
        };
      } else {
        // Loop through each original response record
        originalResponses.forEach((originalResponse) => {
          // Extract relevant fields from the original response
          const { recId, designacao, observacao, movimentosn} = originalResponse;
          // Check if "descricao" is null or an empty string
          let transformedDescricao = designacao;
          
          if (designacao === null || designacao === '') {
            transformedDescricao = `Tipo Construção ${counter}`;
            counter++; // Increment the counter
          }
          if (recId !== 0 ) {
            // Create the desired structure for each record
            const transformedRecord = {
              value: transformedDescricao, // Use the "descricao" field as the "value"
              id:  recId, // Use the "recId" field as the "id"
              observacao:observacao, 
              movimentosn: movimentosn                
            };

            transformedData.push(transformedRecord);
          }
          

        });
      }
    } else {
      // Extract relevant fields from the original response
      const { recId, designacao, observacao, movimentosn} = originalResponses;
      // Check if "descricao" is null or an empty string
      let transformedDescricao = designacao; 

      if (designacao === null || designacao === '') {
        transformedDescricao = `Tipo Construção ${counter}`;
        counter++; // Increment the counter
      }
      // Create the desired structure for each record
      const transformedRecord = {
        value: transformedDescricao, // Use the "descricao" field as the "value"
        id:   recId, // Use the "recId" field as the "id"   
        observacao:observacao, 
        movimentosn: movimentosn          
      };
      
      transformedData.push(transformedRecord);
    }   
    return transformedData;
  } 
  // useEffect para atualizar a combo do tipo de intervenção com os dados lidos
  useEffect(() => {
    // Update the form when data.nif changes
    if (FormRefgeral.current!=null) {

      const transformedResult = transformResponse(datatipoconstrucao); 
          
      const myComboBox= FormRefgeral.current.getItem('tipoconstrucao').getWidget();
      myComboBox.data.parse(transformedResult);
      myComboBox.setValue('-1');
       
    }
  }, [datatipoconstrucao]); 

  useEffect(() => {
    if (obj_pesquisaconstrucao  !== false) {
 
      const onPesquisaConstrucaoId = () => {    
        const contPesquisaConstrucaoId = async () => { 
          const onzoomToWindow = (aux_construcaoid, centroid, bboxWKT) => {
            //var centroid = data?.centroid;
            if(centroid!=null){
              const coordsc = centroid.substring(7, centroid.length - 1).split(' ');
              const centro_x = parseFloat(coordsc[0]);
              const centro_y = parseFloat(coordsc[1]);
        
              //var bboxWKT = data?.bbox;
        
                // Extract the coordinates from the WKT string
              const coords = bboxWKT.substring(10, bboxWKT.length - 2).split(',');
        
              // Parse the coordinates as numbers
              const points = coords.map((coord) => {
                const [x, y] = coord.split(' ');
                return [parseFloat(x), parseFloat(y)];
              });
        
              // Calculate the width and height of the bounding box
              const [minx, miny] = points[0];
              let maxx = minx;
              let maxy = miny;
              points.forEach(([x, y]) => {
                if (x < minx) minx = x;
                if (x > maxx) maxx = x;
                if (y < miny) miny = y;
                if (y > maxy) maxy = y;
              });
              const width = Math.abs(maxx - minx);
              const height = Math.abs(maxy - miny);
        
              // Calculate the initial map scale
              const resolution = Math.max(width / 800, height / 600);
              const escala = parseInt( resolution / 0.00028) + 50;
              if (centro_x!=null && centro_y!= null && escala !=null){
        
                var state = viewer.getState();
                const args = state.config;
                var NomeMapa = state.config.activeMapName;
                var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }};
                //zoomToView(aux);
                viewer.dispatch(aux);
              }
              let estado = viewer.getState();
              let activeMapName = estado.config.activeMapName;
         
              let mapState = estado.mapState[activeMapName];
              let currentMap = mapState.mapguide.runtimeMap; 
              let sessionId  = currentMap.SessionId; 
              let mapaDef  = currentMap.MapDefinition;
                
              setConstrucoesSelection(apiEndpointSIG, authtoken, activeMapName, mapaDef, sessionId, aux_construcaoid, centro_x, centro_y, escala);
             
            } 
            
          }
          function showDataConstrucao(dadosent, caminhoFicheiro){
            // vamos criar uma janela de confirmação do delete
            windowConstrucaoResultadoRef.current = new WindowDHX({
              width: 800,
              height: 700,
              title: 'Lista Construções', 
              closable: true,
              movable: true,
              resizable: false,
              modal: false,
              header: true,
              footer: true,
              css: "pcc_window",
            }); 
          

            windowConstrucaoResultadoRef.current.header.data.add({
              type: "button",
              view: "link",
              size: "medium",
              color: "primary", 
              disabled: false,
              value: "CSV",
              tooltip: "Obter CSV",
              color: "primary", 
              id: "imprimir",
              css: "pcc_button_delete dhx_button--circle",
              html: icon_csvHtml, 
            }, 2);
            const handleDownload = async (ficheiro) => {
              try {
                novoProcessoAPI();
                const url = apiEndpointCadastro + 'ConstrucaoByCampos?filename=' + ficheiro;  
                const jwtToken = authtoken;      
                const response = await fetch(url, {
                  method: 'GET',  
                  headers: {
                      'Content-Type': 'application/json',
                      'Authorization': `Bearer ${jwtToken}`,
                    },
                });   

                if (response.ok) { 
                  // Read the response as a blob
                  const fileBlob = await response.blob(); 
                  // Create a URL for the blob data
                  const fileUrl = URL.createObjectURL(fileBlob, { type: 'application/csv' });  
                  // Create an anchor element
                  const downloadLink = document.createElement('a'); 
                  // Generate the file name with date and time
                  const currentDate = new Date();
                  const year = currentDate.getFullYear();
                  const month = (currentDate.getMonth() + 1).toString().padStart(2, '0'); // Month is 0-indexed
                  const day = currentDate.getDate().toString().padStart(2, '0');
                  const hour = currentDate.getHours().toString().padStart(2, '0');
                  const minute = currentDate.getMinutes().toString().padStart(2, '0');
                  const second = currentDate.getSeconds().toString().padStart(2, '0');
                  
                  const formattedDateTime = `${year}${month}${day}${hour}${minute}${second}`;  
                  const fileName = `pesquisaconstrucoes_${formattedDateTime}.csv`; 
                  // Set the download link properties
                  downloadLink.href = fileUrl;
                  downloadLink.download = fileName;
                  downloadLink.target = '_blank'; // Open the file in a new window or tab 
                  // Trigger the download
                  downloadLink.click();
                } else {
                  console.error('Error:', response.status, response.statusText); 
                }
              } catch (error) {
                console.error('Erro ao baixar o ficheiro:', error);
              }
            };
            windowConstrucaoResultadoRef.current.header.events.on("click", function(id,e){
              switch(id){
                case 'imprimir':
                  handleDownload(caminhoFicheiro);
                   
                  break;
                default:
                  break;
              }

            });
            gridConstrucaoResultadoRef.current = new GridDHX(null,{ 
              css: "pcc_grid",
              columns: [
                { width: 0, autoWidth: true, id: "id", hidden: true, header: [{ text: "#" }] },
                { width: 0, autoWidth: false, id: "construcao_centroid", hidden: true, header: [{ text: "construcao_centroid" }] } ,
                { width: 0, autoWidth: false, id: "construcao_bbox", hidden: true, header: [{ text: "construcao_bbox" }] } ,
                { width: 50, autoWidth: false, id: "locpin", header: [{ text: "" }]  },   
                { width: 50, autoWidth: false, id: "open", header: [{ text: "" }]  },  
                { width: 120, autoWidth: true,id: "designacao", header: [{ text: "Designação"}] },  
                { width: 120, autoWidth: true,id: "nif", header: [{ text: "NIF"}] },  
                { width: 275, autoWidth: true,id: "nome", header: [{ text: "Nome"}] },    
                { width: 150, autoWidth: true,id: "tipoconstrucao", header: [{ text: "Tipo Construção"}] },  
                { width: 0, hidden: true, id: "analise", header: [{ text: "Análise", rowspan: 2 }] },  
                
              ],
              htmlEnable: true,
              selection: "row",
              headerRowHeight: 50 
            });
            gridConstrucaoResultadoRef.current?.events.on("cellDblClick",  async (row, column, e) => {
             
              if (column.id=="open"){ 
                var construcaoid=row.id;
                var aux2={ type: Actions.HIDE_PESQUISA_CONSTRUCAO, payload: construcaoid }; 
                viewer.dispatch(aux2); 
                var aux3={ type: Actions.CONSTRUCAO_EDITA, payload: construcaoid }; 
                viewer.dispatch(aux3);  
               
                

              }
            });
            gridConstrucaoResultadoRef.current?.events.on("cellClick", function(row,column,e){
              // your logic here
              if (column.id=="locpin"){ 
                var aux_construcaoid=row.id;
                var centroid=row.construcao_centroid;
                var mbr=row.construcao_bbox;  
                if (centroid!=""){
                  onzoomToWindow(aux_construcaoid, centroid, mbr);
                }  

              }

            });
            layoutRef.current.getCell("loading").hide();
            windowConstrucaoResultadoRef.current?.attach(gridConstrucaoResultadoRef.current);
            windowConstrucaoResultadoRef.current?.show(); 

            terminouProcessoAPI();
            gridConstrucaoResultadoRef.current.data.parse(dadosent);
    
    
          }
          const objformdesignacao = FormRefgeral.current.getItem("designacao").getValue();
          const objformnome = FormRefgeral.current.getItem("nome").getValue();
          const objformnif = FormRefgeral.current.getItem("nif").getValue();
          const ComboBoxTipoConstrucao = FormRefgeral.current.getItem('tipoconstrucao').getWidget();
          
          let objformtipoconstrucao = (ComboBoxTipoConstrucao.getValue()=="-1"?"":ComboBoxTipoConstrucao.getValue());
            
          novoProcessoAPI()
          const url = apiEndpointCadastro + 'ConstrucaoByCampos/';  
          const jwtToken = authtoken;       
          const response = await fetch(url, {
            method: 'POST', 
            body: JSON.stringify({ 
              designacao: objformdesignacao,
              nome: objformnome,
              nif: objformnif,
              tipoconstrucao: objformtipoconstrucao 
              }),
            headers: {
                'Content-Type': 'application/json', 
                'Authorization': `Bearer ${jwtToken}`,
              },
          });       
  
          if (response.ok) {
            const resposta = await response.json();

            // Acessar o caminho do PDF
            const caminhoFicheiro = resposta.caminhoFicheiro;
            const jsonData = resposta.dados;
            console.log(caminhoFicheiro);
            if (jsonData){


              Promise.resolve(showDataConstrucao(jsonData, caminhoFicheiro))
              .then((json) => {
                // Execute commands after showDataProcessos completes
                //alert("Data parsed successfully!");
               
          
                // Your additional commands here...
              })
              .catch((error) => {
                // Handle errors if necessary
                console.error("Error processing data:", error);
              });

            }
          } else {
            console.log(response);
            FormRefgeralBotao.current.getItem("textmessage").show();
            layoutRef.current.getCell("loading").hide();
            // Define um temporizador para ocultar o item após 5 segundos (5000 ms)
            setTimeout(() => {
              FormRefgeralBotao.current.getItem("textmessage").hide();
            }, 2000);
          }
            
        };
        layoutRef.current.getCell("loading").show();
        contPesquisaConstrucaoId();       
   
      }
      const fetchDataTipoConstrucao = async () => {//ok
        const url2 = apiEndpointCadastro + 'TipoConstrucao';//ok 
        const jwtToken = authtoken; 
        fetch(url2, {
					headers: {
						'Authorization': `Bearer ${jwtToken}`
					}
				})
				.then(response => {
					if (!response.ok) {
						throw new Error('Network response was not ok');
					}
					return response.json();
				})
				.then(jsonData => {
				  setDataTipoConstrucao(jsonData); 
				})
				.catch(error => {
					console.error('There was a problem with your fetch operation:', error);
				});  
      }
    
      windowRef.current = new WindowDHX({
        width: 600,
        height: 600,
        title: 'Pesquisa Construções', 
        closable: true,
        movable: true,
        resizable: true,
        modal: false,
        header: true,
        footer: true,
        css: "pcc_window",
      });
      globalWindowReference = windowRef.current;
 
      function closeWindow() {
        if (windowRef.current) {
          windowRef.current.close();
        }      
      }
       
      FormRefgeral.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        //padding: 17,
        rows: [
          {
            css: "pcc_input",type: "fieldset", label: "Informação geral: ", name: "g3", disabled: false, hidden: false ,
            rows:[  { type: "spacer", name: "spacer" ,  height:"10px"}, 
          { css: "pcc_input",type: "input", id: "designacao", name: "designacao", label: "Designação", placeholder: "Designação", required: false },  
          { css: "pcc_input",type: "combo", name: "tipoconstrucao", label: "Tipo", labelPosition: "left" ,  readOnly:true, },    
            ]},
          {
            css: "pcc_input", type: "fieldset", label: "Concessionário: ", name: "g1", disabled: false, hidden: false, value:'',
            rows: 
            [ 
              { css: "pcc_input",type: "input", id: "nif", name: "nif", label: "NIF", placeholder: "NIF", required: false },    
              { css: "pcc_input",type: "input", id: "nome", name: "nome", label: "Nome", placeholder: "Nome", required: false },  
            ]}
          ] 
        
      }); 
      FormRefgeralBotao.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form", 
        cols: [ 
            { css: "pcc_input", id: "buttonpesquisar", type: "button",  circle: true, value: "Pesquisar", tooltip: "Pesquisar", title: "Pesquisar" }, 
            { type: "spacer" } ,
            { css: "pcc_input",
              id: "textmessage",
              hidden: true,
              hiddenLabel: true,
              name: "textmessage",
              type: "text",
              label: "Text",  
              value: "Registo não existe!",
          }, 
          ]  
      }); 
      
      FormRefgeralBotao.current.getItem("buttonpesquisar").events.on("click", function(events) {
        console.log("click", events);
        handlePesquisaConstrucao();
      });
     

      function handlePesquisaConstrucao() {
        
          const objformdesignacao = FormRefgeral.current.getItem("designacao").getValue();
          const objformnome = FormRefgeral.current.getItem("nome").getValue();
          const objformnif = FormRefgeral.current.getItem("nif").getValue();
          const ComboBoxTipoConstrucao = FormRefgeral.current.getItem('tipoconstrucao').getWidget(); 

          const objformtipoconstrucao = ComboBoxTipoConstrucao.getValue(); 
       
          if  ((objformdesignacao == "")&&(objformnome == "")&&(objformnif == "")&&
              (objformtipoconstrucao == "-1")){
            dhx.alert({
              header:"Erro Campos de Pesquisa vazios",
              text:"É necessário que pelo menos um dos campos esteja preenchido",
              buttonsAlignment:"center",
              buttons:["ok"],
            });
          }else{
            onPesquisaConstrucaoId(); 
        }
      }

      layoutRef.current = new LayoutDHX(null,{
        type: "line",
        rows: [
          {
            id: "dadosent",
            height: "400px",
            html: "dadosent",
            css: "pcc_layout2",
            progressDefault: true,
        },
        
        {cols:[
          {
            id: "botaopesquisa",
            html: "botaopesquisa",
            css: "pcc_layout2",
            progressDefault: true,
        },
        {
          id: "loading", 
          css: "pcc_layout2",
          progressDefault: true,}
        ]
      }
      ]});
      layoutRef.current.getCell("loading").hide();
      layoutRef.current.getCell("dadosent").attach(FormRefgeral.current);
      layoutRef.current.getCell("botaopesquisa").attach(FormRefgeralBotao.current); 

      windowRef.current?.attach(layoutRef.current);
      windowRef.current?.events.on("afterHide", function(position, events){ 
          handleClose(); 
      });
      if (windowRef.current!=undefined){
        fetchDataTipoConstrucao(); 
        windowRef.current.show();
      }else {        
        windowRef.current = null; 
      } 
    } else { 
      windowRef.current = null;   
    }

    return () => { 
      windowRef.current?.destructor();  
      windowRef.current = null;
    };
  }, [obj_pesquisaconstrucao ]);

 
  return (
    <div ref={windowRef}></div> 
  );
  
}
 
Wpcc_Form_ConstrucaoPesquisaCampos.propTypes = {
  obj_pesquisaconstrucao: PropTypes.boolean, 
};
export default Wpcc_Form_ConstrucaoPesquisaCampos;
