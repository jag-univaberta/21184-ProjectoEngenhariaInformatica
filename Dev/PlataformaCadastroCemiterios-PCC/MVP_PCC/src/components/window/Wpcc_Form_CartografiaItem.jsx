import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, List as ListDHX,
   ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";

   
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
import * as Perm from "../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 

const Wpcc_Form_CartografiaItem= ({form_cartografiaitemedit_recId, form_cartografiaitemnew_paiId}) => {
  
  const icon_delete = <Icon iconName="Delete" />;
  const icon_save = <Icon iconName="Save" />;  
  
  const icon_deleteHtml = ReactDOMServer.renderToStaticMarkup(icon_delete);
  const icon_saveHtml = ReactDOMServer.renderToStaticMarkup(icon_save); 

  const dispatch = useReduxDispatch();

  const [previousCartografiaItemId, setPreviousCartografiaItemId] = useState(null);
  const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint);
  const apiEndpointSIG = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointSIG);
  const treeGU_Cartografia = useSelector((state)=> state.aplicacaopcc.referencia_treecartografia);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
     
  const [data, setData] = useState(null);

  const windowRef = useRef(null);  
  const FormLayout= useRef(null);
 
  const FormDadosCabecalho = useRef(null);
  const FormDadosBotoes= useRef(null);
  const ListaLayersDisponiveis= useRef(null);
  const ListaLayersUsados= useRef(null);
  const FormDadosFiltro= useRef(null);

  const listaDeCodigosPermissoes = Perm.listaDeCodigosCartografia;
  const funcionalidade = useSelector((state)=> state.aplicacaopcc.funcionalidades);
      
  // Filter permissions that have cod in listaDeCodigosPermissoes
  const funcionalidadesRelevantes = funcionalidade.filter(permission => {
    // Check if cod is in listaDeCodigosInteressantes
    return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
  });

  const permissaoNovoItem = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FCart_NovoItem, '');
  const handleClose = () => { 
    
    var aux={ type: Actions.CARTOGRAFIAITEM_FECHA, payload: "" };
    viewer.dispatch(aux); 
    
  };


  useEffect(() => {

    if ((form_cartografiaitemedit_recId !== previousCartografiaItemId)&&((form_cartografiaitemedit_recId !="")&&(form_cartografiaitemnew_paiId==""))) {
  
      const fetchData = async () => {//ok
        const url = apiEndpointSIG + 'Cartografia/' + form_cartografiaitemedit_recId;//ok
        console.log(url);
        const jwtToken = authtoken; 
        fetch(url, {
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
          setData(jsonData);
				})
				.catch(error => {
					console.error('There was a problem with your fetch operation:', error);
				}); 
      }; 
       
      fetchData();
    } 
  }, [form_cartografiaitemedit_recId,previousCartografiaItemId]);

  useEffect(() => {
    // Update the form when data.nif changes
    if (FormDadosCabecalho.current !== null) {
      console.log(data);
      FormDadosCabecalho.current?.setValue({ "nome": data?.nome }); 
      FormDadosCabecalho.current?.setValue({ "parent_id": data?.parent });
      FormDadosCabecalho.current?.setValue({ "rec_id": data?.recId });
      let objetoslayers = data?.layers;
      let datalayers=[];
      objetoslayers.forEach(el => {
        datalayers.push(el.layer);
      });
      FormDadosCabecalho.current?.setValue({ "layers": datalayers.join(',')});
      console.log(datalayers.join(','));
      //const datalayers=data?.layer.split("||||");
      const items = ListaLayersDisponiveis.current?.data;
      const remover=[];
      datalayers.forEach(layer => { 
        items.forEach(element => {
            if (element.value==layer){
              ListaLayersUsados.current?.data.add(element);
              remover.push(element.id);
            } 
        });
      });
      remover.forEach(id => {
        ListaLayersDisponiveis.current?.data.remove(id);
      }); 

    }
    if(permissaoNovoItem){
      console.log("Perm ok");
    }else{
        FormDadosCabecalho.current?.getItem("nome").disable();
        FormDadosBotoes.current?.getItem("buttonadicionar").disable();
        FormDadosBotoes.current?.getItem("buttonremover").disable();
        FormDadosFiltro.current?.getItem("buttonpesquisar").disable();

    }
  }, [data?.recId]);


  useEffect(() => {
  

    if (((form_cartografiaitemedit_recId !="")&&(form_cartografiaitemnew_paiId==""))||((form_cartografiaitemedit_recId =="")&&(form_cartografiaitemnew_paiId !=""))) {
     
       //verificamos se é um  novo grupo de cartografia
      let novo_cartografiaitem=false;
      if ((form_cartografiaitemedit_recId =="")&&(form_cartografiaitemnew_paiId !="")) {
        novo_cartografiaitem=true;
      }
  
      const fetchLayersDisponiveis = async () => {
        var estado = viewer.getState();			
        var activeMapName = estado.config.activeMapName;
        var mapState = estado.mapState[activeMapName];
        var currentMap = mapState.mapguide.runtimeMap; 
        var sessionId  = currentMap.SessionId; 
        console.log(apiEndpointSIG);
        var mapaDef  = currentMap.MapDefinition;
        const jwtToken = authtoken;
        const url3 = apiEndpointSIG + 'MapaLayers/' + sessionId
        console.log(url3);
        fetch(url3, {
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
					    
          ListaLayersDisponiveis.current?.data.parse(jsonData);          
          refreshLayersUsados();  
				})
				.catch(error => {
					console.error('There was a problem with your fetch operation:', error);
				});
      }	 
      const refreshLayersUsados = async () => {
        // Update the form when data.nif changes
        if (FormDadosCabecalho.current !== null) {
        
          const layers=FormDadosCabecalho.current?.getItem("layers").getValue();

          const datalayers=layers.split(",");
          const items = ListaLayersDisponiveis.current?.data;
          const remover=[];
          datalayers.forEach(layer => {
           
            items.forEach(element => {
                if (element.value==layer){
                  ListaLayersUsados.current?.data.add(element);
                  remover.push(element.id);
                } 
            });
          });
          remover.forEach(id => {
            ListaLayersDisponiveis.current?.data.remove(id);
          });    
        }
      }  
      
      windowRef.current = new WindowDHX({
        width: 900,
					height: 613,
					title: 'Elemento de Cartografia', 
					closable: true,
					movable: true,
					resizable: false,
					modal: false,
					header: true,
					footer: true,
					css: "pcc_window",
      });

      FormLayout.current = new LayoutDHX(null,{
        type: "line",
        rows: 
        [   
        {
          id: "cabecalho",
          html: "cabecalho",
          height: 110,
          css: "pcc_layout2",
          progressDefault: true,
        },
        {
          //align: align,
          //height: 300,
          cols: [
            {
              id: "layersdisponiveis",
              html: "layersdisponiveis",
              header:"Layers Disponiveis:", 
              height: 350,
              width: 350,
              padding: 10,
              progressDefault: true,
            },
            {
              id: "areabotoes",
              html: "areabotoes",
              height: 350,
              width: 100,
              padding: 0,
              progressDefault: true,
            },
            {
              id: "layersusados",
              html: "layersusados",
              header:"Layers Usados:", 
              height: 350,
              width: 350,
              padding: 10,
              progressDefault: true,
            }],
        },
        {
          id: "filtro",
          html: "filtro",
          height: 88,
          padding: 10,
          progressDefault: true,
        },
        ]
      });

      windowRef.current?.header.data.add({
        type: "button",
        view: "link",
        size: "medium",
        color: "primary",  
        disabled: false,   
        value: "Gravar",
        tooltip: "Gravar",
        title: "Gravar",
        id: "gravar",
        css: "pcc_button_save dhx_button--circle", 
        html: icon_saveHtml
      }, 2);
      windowRef.current?.header.data.add({
        type: "button",
        view: "link",
        size: "medium",
        color: "primary", 
        disabled: novo_cartografiaitem,  
        value: "Eliminar",
        tooltip: "Eliminar",
        title: "Eliminar",
        id: "eliminar",
        css: "pcc_button_delete dhx_button--circle",
        html: icon_deleteHtml
      }, 3);
      // Trata os botões no cabeçalho - Save e Delete
      windowRef.current?.header.events.on("click", function(id,e){
        const SaveCartografiaItem= (rec_id, parent_id)=> {
          try { 
          const saveCartografiaItemcall = async () => {      
           
            const nome=FormDadosCabecalho.current?.getItem("nome")._value;  
            const items = ListaLayersUsados.current?.data;
            const listalayersarray=[];
            items.forEach(element => {
              listalayersarray.push(element.value);
            }); 
            let listalayers=listalayersarray.join(','); 
            //let tipo_atualizacao = 'update';
            let auxmethod ='PUT';
            let url = apiEndpointSIG + 'Cartografia/' + parseInt(rec_id).toString();//ok  
            if (rec_id=="nova_cartografia"){
              rec_id=0;
              auxmethod ='POST';
              //tipo_atualizacao = 'insert';
              url = apiEndpointSIG + 'Cartografia';//ok  
            } 
            const processedLayers = listalayersarray.map(layerValue => {
                return {
                    layer: layerValue  
                };
            });
            // Construir o objeto principal para enviar
            const cartografiaPayload = {
              recid : parseInt(rec_id),
              parent: parseInt(parent_id),                  
              nome: nome, 
              layers: processedLayers 
            };           
            const jwtToken = authtoken;              
            const response = await fetch(url, {
              method: auxmethod,
              body: JSON.stringify(cartografiaPayload),
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${jwtToken}`
                },
            });     
            if (!response.ok) {
                // Se a resposta não for OK (ex: 400, 401, 500), tenta ler a resposta como JSON para mais detalhes.
                let errorData;
                try {
                    errorData = await response.json();
                } catch (e) {
                    // Se a resposta de erro não for JSON, usa o texto do status.
                    errorData = { message: response.statusText };
                }
                console.error('Falha ao enviar cartografia:', response.status, errorData);
                // Lança um erro para ser capturado pelo bloco catch externo.
                throw new Error(`Erro HTTP ${response.status}: ${errorData.title || errorData.message || response.statusText}`);
            }
            if (response.ok) {    
              const data = await response.json();
              let tipo_atualizacao='insert';
              if (data.ordem==-1){
                tipo_atualizacao='update';
              }
              if (data.recId!=0){
                  dhx.alert({
                    header:"Gravação efectuada",
                    text:"A Gravação foi efetuada com sucesso!",
                    buttonsAlignment:"center",
                    buttons:["ok"],
                  }).then(function(){
                    windowRef.current.hide();
                    
                    //var item = data.rec_id + data.nome + data.pai_recid; 
                    if (tipo_atualizacao=='update'){
                      ///treeGU_Pretensoes.current.reloadTreeData();
                      viewer.dispatch({type: Actions.TREEGUCARTOGRAFIA_UPDATE, 
                        payload: { rec_id: data.recId, nome: data.nome , layers: listalayers } });                      
                    }
                    if (tipo_atualizacao=='insert'){
                      console.log(listalayers);
                      
                      viewer.dispatch({type: Actions.TREEGUCARTOGRAFIA_INSERT, 
                        payload: {  parent_recid: data.parent, 
                          rec_id: data.recId, nome: data.nome, 
                          layers: listalayers, objtipo: 'ITEM' } })                      
                    }
                    
                  }); 
              }
            
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
          };
          saveCartografiaItemcall();
          } catch (error) {
          console.error('error: ' + error);                 
          } 
        }
        const DeleteCartografiaItem = (objid,parent_id)=> {
          try { 
          const deleteCartografiaItemcall = async () => {                   
            const url = apiEndpointSIG + 'Cartografia/'+ parseInt(objid).toString();//ok    
            const jwtToken = authtoken;     
            const response = await fetch(url, {
            method: 'DELETE',  
            body: JSON.stringify({ 
              id : objid
              }),          
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`
              },
            });              
            if (response.ok) {    
              dhx.alert({
                header:"Remover Item Separador",
                text:"O Item foi removido com sucesso!",
                buttonsAlignment:"center",
                buttons:["ok"],
              }).then(function(){
                try{     
                  viewer.dispatch({type: Actions.TREEGUCARTOGRAFIA_DELETE, payload: { rec_id: objid} })  
                    
                  FormDadosFiltro.current?.destructor();
                  ListaLayersUsados.current?.destructor();
                  ListaLayersDisponiveis.current?.destructor();
                  FormDadosBotoes.current?.destructor();
                  FormDadosCabecalho.current?.destructor();
                  FormLayout.current?.destructor(); 
                  windowRef.current?.destructor();
            
                  FormDadosFiltro.current = null;
                  ListaLayersUsados.current = null; 
                  ListaLayersDisponiveis.current = null;
                  FormDadosBotoes.current = null; 
                  FormDadosCabecalho.current = null;
                  FormLayout.current = null; 
                  windowRef.current = null; 
                }catch(e){}   
                handleClose();
              });  
            } else {
              const errorMessage = await response.text();
              console.error('error: ' + errorMessage);
            }
          };
          deleteCartografiaItemcall();
          } catch (error) {
          console.error('error: ' + error);                 
          } 
        }
        console.log(id);
        // Aqui dispara os eventos do click nos botões do header
        switch(id){
          case 'gravar': 
            var rec_id=FormDadosCabecalho.current?.getItem("rec_id").getValue();
            var parent_id=FormDadosCabecalho.current?.getItem("parent_id").getValue();
            var nome =FormDadosCabecalho.current?.getItem("nome")._value;
            if (permissaoNovoItem) {
            if(nome == ""){
              dhx.alert({
                header:"Erro ao gravar",
                text:"É necessário que o campo (Nome*) esteja preenchido",
                buttonsAlignment:"center",
                buttons:["ok"],
              });
            } else { 
              SaveCartografiaItem(rec_id, parent_id);  
            }
          }else{
            dhx.alert({
              header:"Permissão Necessária",
              text:"A Gravação não foi efetuada é necessário permissão!",
              buttonsAlignment:"center",
              buttons:["ok"],
            });
          }

            
            break;
          case 'eliminar':
            var objid=FormDadosCabecalho.current?.getItem("rec_id").getValue();
            var parent_id=FormDadosCabecalho.current?.getItem("parent_id").getValue(); 
            // vamos criar uma janela de confirmação do delete
            if (permissaoNovoItem) { 
                dhx.confirm({
                  header:"Remover Cartografia",
                  text:"Tem a certeza que pretende remover a cartografia?",
                  buttons:["sim", "não"],
                  buttonsAlignment:"center"
                }).then(function(resposta){
                  console.log('resposta ', resposta);
              
                  switch(resposta){
                  case false:
                    DeleteCartografiaItem(objid, parent_id); 
                    break;
                  default:
                    break;
                  }
                });
            }else{
              dhx.alert({
                header:"Permissão Necessária",
                text:"A Eliminação não foi efetuada é necessário permissão!",
                buttonsAlignment:"center",
                buttons:["ok"],
              });
            }
            break;
          case 'close':
            closeWindow();
            break;
          default:
            break;
        }
      
      });

      globalWindowReference = windowRef.current;
 
      let datacombo;
      let datacombo2;	
    
      function closeWindow() {
        if (windowRef.current) {
          windowRef.current?.close();
        }
      
      }

      FormDadosCabecalho.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        padding: 10,
        rows: [
          { css: "pcc_input",type: "input", name: "nome", label: "Nome", placeholder: "Nome" },
          {
            css: "pcc_input",type: "fieldset", label: "Dados auxiliares:", name: "g3", disabled: true, hidden: true, 
            rows:[  
                { css: "pcc_input",type: "input", name: "rec_id", label: "rec_id", placeholder: "rec_id", value:"nova_cartografia"},
                { css: "pcc_input",type: "input", name: "parent_id", label: "parent_id", placeholder: "parent_id" },
                { css: "pcc_input",type: "input", name: "layers", label: "layers", placeholder: "layers" },
              ] 
          }
          
        ] 
      });

      ListaLayersDisponiveis.current = new ListDHX(null,{
        css: "dhx_widget--bg_white dhx_widget--bordered formlist",
        //template: template, //takes function "template"
        //itemHeight: itemHeight,
        data: datacombo, 
      });

      ListaLayersUsados.current = new ListDHX(null,{
        css: "dhx_widget--bg_white dhx_widget--bordered formlist",
        //template: template,
        //itemHeight: itemHeight,
        data: datacombo2, 
      });

      FormDadosFiltro.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        padding: 10,
        rows: [
          {
          cols: [
            {css: "pcc_input_filter",type: "input", name: "filter", placeholder: "Filtro" },
            {css: "pcc_input", id: "buttonpesquisar", circle: true,  type: "button", value: "Filtrar", tooltip: "Filtrar", title: "Filtrar" }
          ], 
          }
        ] 
      });
      
      FormDadosBotoes.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        padding: 10,
        rows: [
          {css: "pcc_input_buttonadicionar", id: "buttonadicionar", type: "button",  circle: true, value: ">>", tooltip: ">>", title: ">>" },
          {css: "pcc_input", id: "buttonremover", circle:true,  type: "button", circle: true,  value: "<<", tooltip: "<<", title: "<<" }, 
        ] 
      }); 
      

      ListaLayersDisponiveis.current?.events.on("click", function(id,event) {
        if(permissaoNovoItem){
        var itemdt = ListaLayersDisponiveis.current?.data.getItem(id); 
        ListaLayersUsados.current?.data.add(itemdt);
        ListaLayersDisponiveis.current?.data.remove(id); 
        }
      });

      ListaLayersUsados.current?.events.on("click", function(id,event) {
        if(permissaoNovoItem){
        var itemdt = ListaLayersUsados.current?.data.getItem(id); 
        ListaLayersDisponiveis.current?.data.add(itemdt);
        ListaLayersUsados.current?.data.remove(id);  
        }
      });

  
      FormDadosBotoes.current?.getItem("buttonadicionar").events.on("click", function(events) {
        if(permissaoNovoItem){
        const items = ListaLayersDisponiveis.current?.data;
				items.forEach(element => {
          ListaLayersUsados.current?.data.add(element);
        });
        
        ListaLayersDisponiveis.current?.data.removeAll();
      }
        
      });
      FormDadosBotoes.current?.getItem("buttonremover").events.on("click", function(events) {
        if(permissaoNovoItem){
        const items = ListaLayersUsados.current?.data;
				items.forEach(element => {
          ListaLayersDisponiveis.current?.data.add(element);
        });
        
        ListaLayersUsados.current?.data.removeAll();
      }
      });

      
      FormDadosFiltro.current?.getItem("buttonpesquisar").events.on("click", function(events) {
          if (permissaoNovoItem) {
              var texto_pesquisa = FormDadosFiltro.current?.getItem("filter").getValue().toLowerCase(); // Converte o texto de pesquisa para minúsculas
      
              ListaLayersDisponiveis.current?.data.filter({
                  by: "value",
                  match: texto_pesquisa,
                  compare: (value, match, item) => {
                      return value.toLowerCase().includes(match); // Converte o valor a ser comparado para minúsculas
                  }
              });
          }
      });
    
      FormLayout.current?.getCell("cabecalho").attach(FormDadosCabecalho.current);
      FormLayout.current?.getCell("layersdisponiveis").attach(ListaLayersDisponiveis.current);
      FormLayout.current?.getCell("layersusados").attach(ListaLayersUsados.current);
      FormLayout.current?.getCell("filtro").attach(FormDadosFiltro.current);
      FormLayout.current?.getCell("areabotoes").attach(FormDadosBotoes.current);
      
      windowRef.current?.attach(FormLayout.current);

      function handleSave() {
        
        let objform_nome = FormDadosCabecalho.current?.getItem("nome").getValue()|| '';
        let objform_recid = FormDadosCabecalho.current?.getItem("rec_id").getValue()|| '';
        let objform_parentid = FormDadosCabecalho.current?.getItem("parent_id").getValue()|| '';
        console.log(objform_nome);
        if(objform_nome.trim() == ""){
          dhx.alert({
            header:"Erro ao gravar",
            text:"É necessário que o campo (Nome*) esteja preenchido",
            buttonsAlignment:"center",
            buttons:["ok"],
          });
        }else{
          onsaveWindow(objform_recid,objform_parentid, objform_nome);  
        }
      }
  
      windowRef.current?.events.on("afterHide", function(position, events){
          console.log("A window is hidden", events);
          handleClose();
          
      }); 

      FormDadosCabecalho.current?.setValue({ "parent_id": form_cartografiaitemnew_paiId });


      if (windowRef.current!=undefined){
         windowRef.current?.show();

        fetchLayersDisponiveis();
        
      }else {
        FormDadosFiltro.current?.destructor();
        ListaLayersUsados.current?.destructor();
        ListaLayersDisponiveis.current?.destructor();
        FormDadosBotoes.current?.destructor();
        FormDadosCabecalho.current?.destructor();
        FormLayout.current?.destructor(); 
        windowRef.current?.destructor();
  
        FormDadosFiltro.current = null;
        ListaLayersUsados.current = null; 
        ListaLayersDisponiveis.current = null;
        FormDadosBotoes.current = null; 
        FormDadosCabecalho.current = null;
        FormLayout.current = null; 
        windowRef.current = null; 
      } 
    } else {
      // cartografiaId está vazio logo destroy the window e tudo o resto
 
      FormDadosFiltro.current?.destructor();
      ListaLayersUsados.current?.destructor();
      ListaLayersDisponiveis.current?.destructor();
      FormDadosBotoes.current?.destructor();
      FormDadosCabecalho.current?.destructor();
      FormLayout.current?.destructor(); 
      windowRef.current?.destructor();

      FormDadosFiltro.current = null;
      ListaLayersUsados.current = null; 
      ListaLayersDisponiveis.current = null;
      FormDadosBotoes.current = null; 
      FormDadosCabecalho.current = null;
      FormLayout.current = null; 
      windowRef.current = null; 
 
    }

    return () => {
      FormDadosFiltro.current?.destructor();
      ListaLayersUsados.current?.destructor();
      ListaLayersDisponiveis.current?.destructor();
      FormDadosBotoes.current?.destructor();
      FormDadosCabecalho.current?.destructor();
      FormLayout.current?.destructor(); 
      windowRef.current?.destructor();

      FormDadosFiltro.current = null;
      ListaLayersUsados.current = null; 
      ListaLayersDisponiveis.current = null;
      FormDadosBotoes.current = null; 
      FormDadosCabecalho.current = null;
      FormLayout.current = null; 
      windowRef.current = null; 
 

    };
  }, [form_cartografiaitemedit_recId,form_cartografiaitemnew_paiId]);
 


  return (
    <div ref={windowRef}></div>
    
  );
  
}
Wpcc_Form_CartografiaItem.propTypes = {
  form_cartografiaitemedit_recId: PropTypes.string.isRequired,
  form_cartografiaitemnew_paiId: PropTypes.string.isRequired,
};
export default Wpcc_Form_CartografiaItem;
