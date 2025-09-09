import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, List as ListDHX,Toolbar as ToolbarDHX,
  ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react'; 
import * as Perm from "../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 

const FormGU_FicheirosAssociados= ({obj_ficheiro_show}) => {
  const windowRef = useRef(null);  
  const menuRef = useRef(null);
  const refreshFicheiros = useSelector((state)=> state.aplicacaopcc.windows.associa_ficheiro.obj_listaficheiros_refresh);
  const [datageom, setDataTipoAtendimento] = useState(null);
  const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  const obj_info_ids = useSelector((state)=> state.aplicacaopcc.windows.detalhes_objectos.obj_info_ids);
  const obj_info = useSelector((state)=> state.aplicacaopcc.windows.detalhes_objectos);
  const obj_info_tipo = useSelector((state)=> state.aplicacaopcc.windows.detalhes_objectos.obj_info_tipo);
  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid);

  const layoutRefgeometrias = useRef(null);
   const menutreeRefgeometrias = useRef(null); 
  const menutreeRefFicheiros= useRef(null); 
  const menutreeGrupo = useRef(null);
  const treeRefgeometrias = useRef(null);
  const FormDadosEnt = useRef(null);
  const tabRef = useRef(null);
  const ListaFicheirosAssociados = useRef(null);

  const listaDeCodigosPermissoes = Perm.listaDeCodigosGestaoTabelas;
  const funcionalidade = useSelector((state)=> state.aplicacaopcc.funcionalidades); 
  // Filter permissions that have cod in listaDeCodigosPermissoes
  const funcionalidadesRelevantes = funcionalidade.filter(permission => {
    // Check if cod is in listaDeCodigosInteressantes
    return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
  });
  const icon_save = <Icon iconName="Save" />;
  const icon_delete = <Icon iconName="Delete" />;  
  const icon_saveHtml = ReactDOMServer.renderToStaticMarkup(icon_save);
  const icon_deleteHtml = ReactDOMServer.renderToStaticMarkup(icon_delete); 

  const permissaoFicheirosAssociados_Consultar = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_FicheirosAssociados_Consultar, '');
  const permissaoFicheirosAssociados_Editar = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_FicheirosAssociados_Editar, '');
 
  const handleClose = () => { 
    console.log('Close Tipos');
    
    //var aux={ type: Actions.HIDE_FICHEIROSASSOCIADOS, payload: "" };
    //viewer.dispatch(aux); 
    
  };

  useEffect(() => {
    if (obj_ficheiro_show !== false) {
       
      const windowHtmlContainer = `<div id="formContainer"></div>`
      windowRef.current = new WindowDHX({
        width: 625,
        height: 689,
        title: 'Ficheiros Associados', 
        closable: true,
        movable: true,
        resizable: true,
        modal: false,
        header: true,
        footer: true,
        css: "pcc_window",
      });
      globalWindowReference = windowRef.current;

      //windowRef.current.header.disable('close');
      //windowRef.current.header.hide('imprimir');
   
      // criar as tabulações
      tabRef.current = new TabbarDHX(null,{
        css: "pcc_tab",
        tabs:[ 
            { id: "tab1", tab: "tab1" },
            { id: "tab2", tab: "south" },
            { id: "tab3", tab: "east" },
            { id: "tab4", tab: "west" },
        ]
      });
      tabRef.current.addTab({ id: "geral", tab: "Geral"}, 1);
  
      let datalist= []; 
      let datalist2 = []; 
      ListaFicheirosAssociados.current= new ListDHX(null,{
        css: "dhx_widget--bg_white dhx_widget--bordered formlist",
        //template: template, //takes function "template"
        //itemHeight: itemHeight,
        data: datalist, 
      });
      
      const FormFicheirosAssociados = new FormDHX(null, { 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        padding: 10,
        
        rows: [
          {
            type: "radioGroup",
            name: "opcao_arquivo",
            label: "Opção de armazenamento",
            value: "copiar", // Define o valor inicial
            options: {
              cols: [
                {
                  type: "radioButton",
                  text: "Copiar o ficheiro para base de dados",
                  value: "copiar",
                  checked: true // Marca este radioButton como selecionado
                },
                {
                  type: "radioButton",
                  text: "Gravar a localização do ficheiro",
                  value: "gravar"
                }
              ]
            },
            css: "pcc_input"
          },
          { 
            css: "pcc_input",
            type: "input", 
            name: "descricao", 
            label: "Descrição", 
            placeholder: "Nome", 
            disabled: true,
            labelPosition: "left"  
          },
          {
            cols: [           
              {
                css: "pcc_input",
                type: "input", 
                name: "arquivdir", 
                label: "Ficheiro/Directório", 
                placeholder: "Ficheiro/Directório", 
                hidden: true,
                labelPosition: "left" 
              },
              { 
                css: "pcc_input_ir", 
                id: "buttonAbrirFicheiro", 
                type: "button", 
                name: "abrir_ficheiro", 
                value: "Abrir/Copiar", 
                tooltip: "Abrir/Copiar", 
                title: "Abrir/Copiar",
                hidden: true, 
                circle: true,
                size: "small", 
                view: "flat", 
                color: "primary",
              }],
          }, 
          { 
            css: "pcc_input",
            type: "datepicker", 
            name: "dataarquivo",  
            label: "Data",  
            dateFormat: "20%y/%m/%d",
            disabled: true ,
            labelPosition: "left"            
          },
          { 
            css: "pcc_input",
            type: "input", 
            name: "observficheiro", 
            label: "Observações:", 
            placeholder: "Observações",
            disabled: true,
            labelPosition: "left"  
          },
          {
            cols: [           
              { 
                css: "pcc_input_addficheiro", 
                id: "buttonAdicionarFicheiro", 
                type: "button", 
                name:"upload_ficheiro", 
                value: "Adicionar", 
                tooltip: "Adicionar", 
                title: "Adicionar", 
                circle: true,
                size: "small", 
                view: "flat", 
                color: "primary"
              },
              { type: "spacer", name: "spacer" },
              { 
                css: "pcc_input_newfile", 
                id: "downloadItem", 
                type: "button", 
                name:"downloaditem", 
                value: "Descarregar", 
                tooltip: "Descarregar", 
                title: "Descarregar", 
                circle: true,
                size: "small", 
                view: "flat", 
                color: "primary",
                hidden: true,
              },
            ], 
          },
          {
            cols: [           
              { 
                css: "pcc_input_addficheiro", 
                id: "buttonGravarFicheiro", 
                type: "button", 
                name:"save_ficheiro", 
                value: "Adicionar", 
                tooltip: "Adicionar", 
                title: "Adicionar", 
                circle: true,
                size: "small", 
                view: "flat", 
                color: "primary",
                hidden: true,
              }
            ], 
          }  
        ]
    });
    
      const layoutFicheirosAssociados = new LayoutDHX(null,{
        type: "line",
        rows: 
        [   
            {
              id: "dadosent1",
              html: "dadosent1",
              header:"Lista dos ficheiros:",  
              progressDefault: true,
              padding: 10,
              height: 200,
            },
            {
              id: "dadosent2",
              html: "dadosent2",
              header:"Dados do ficheiro associado:",  
              progressDefault: true,
              padding: 10,
            } 
        ]
      });
      layoutRefgeometrias.current = new LayoutDHX(null,{
        type: "line",
        rows: [
          {
            id: "dadosent",
            html: "dadosent",
            css: "pcc_layout2",
            progressDefault: true,
          }
      ]});
   
      // criar o menu na tree das geometrias
      menutreeRefFicheiros.current = new ContextMenuDHX(null, {
        css: "dhx_widget--bg_gray",
        data: [ 
          { id:"newfile", value: "Novo" }, 
          { type: "separator" } , 
          { id:"eliminar", value: "Remover", }, 
          { type: "separator" }, 
          //{ type: "separator" },
          //{ id:"associar", value: "Associar Confrontação", }, 
        ]
      });

      menutreeRefFicheiros.current.events.on("click", function(id,e){
       
        console.log(id);     
      

        switch(id){
          case 'newfile':  
          FormFicheirosAssociados.getItem("opcao_arquivo").enable();
      
          function formatarDataAtual() {
            const data = new Date();
            const ano = data.getFullYear();
            const mes = String(data.getMonth() + 1).padStart(2, '0'); // Mês começa em 0, então adicionamos 1
            const dia = String(data.getDate()).padStart(2, '0');
            return `${ano}/${mes}/${dia}`;
          }
          // Limpar os campos do formulário
          FormFicheirosAssociados.setValue({ 
              "descricao": '',
              "dataarquivo": formatarDataAtual(),
              "observficheiro": '',
              "arquivdir": ''
          });
      
          FormFicheirosAssociados.getItem("descricao").enable();
          FormFicheirosAssociados.getItem("dataarquivo").enable();
          FormFicheirosAssociados.getItem("observficheiro").enable();
          FormFicheirosAssociados.getItem("arquivdir").enable();
  
          FormFicheirosAssociados.getItem("downloaditem").hide();
          // Desselecionar qualquer item na ListaFicheirosAssociados
          if (ListaFicheirosAssociados.current.selection) {
              ListaFicheirosAssociados.current.selection.remove();
          }
      
          FormFicheirosAssociados.getItem("abrir_ficheiro").hide();
      
          // Opcional: Se você quiser garantir que o radio button "Copiar o arquivo para base de dados" esteja selecionado
          FormFicheirosAssociados.getItem("opcao_arquivo").setValue("copiar");
  
          FormFicheirosAssociados.getItem("save_ficheiro").hide();
          FormFicheirosAssociados.getItem("upload_ficheiro").hide(); 
          toggleElementsVisibility();
            break;
          case 'eliminar': 
          const deleteitemficheiro = async () => { 
            console.log(reciditemficheiro);
            let idItem=obj_info_ids.split('|');
            const url = apiEndpoint + 'FicheirosAssociadosGeral';      
            const jwtToken = authtoken;   
            const response = await fetch(url, {
            method: 'DELETE',
            body: JSON.stringify({ 
              Id : reciditemficheiro,
              Registo_id : idItem[0]
              }),
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`,
              },
            });              
            if (response.ok) { 
              if (viewer_interface ==null){
                viewer_interface = GetViewerInterface(); 
              } 
              //viewer_interface.clearSelection();
              FormFicheirosAssociados.setValue({ "descricao": '' });
              FormFicheirosAssociados.setValue({ "dataarquivo": '' });
              FormFicheirosAssociados.setValue({ "observficheiro": '' });
              FormFicheirosAssociados.getItem("downloaditem").hide();

              FormFicheirosAssociados.getItem("arquivdir").enable();
              
              FormFicheirosAssociados.getItem("opcao_arquivo").enable();
              FormFicheirosAssociados.getItem("opcao_arquivo").setValue("copiar");
              FormFicheirosAssociados.getItem("abrir_ficheiro").hide();

              fetchDataListaDeFicheiros();               
              toggleElementsVisibility();
  
            } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
            }
  
            
          };
  
          dhx.confirm({
            header:"Remover Ficheiro Associado",
            text:"Tem a certeza que pretende remover o ficheiro?",
            buttons:["sim", "nao"],
            buttonsAlignment:"center"
          }).then(function(resposta){
            console.log('resposta ', resposta);
        
            switch(resposta){
            case false:
              deleteitemficheiro(); 
              break;
            default:
              break;
            }
          }); 
      
            break;
         
          default:
            
            break;
        }
      });

      tabRef.current.getCell("geral").attach(layoutRefgeometrias.current);
      layoutRefgeometrias.current.getCell("dadosent").attach(layoutFicheirosAssociados);
     
      layoutFicheirosAssociados.getCell("dadosent1").attach(ListaFicheirosAssociados.current);
      layoutFicheirosAssociados.getCell("dadosent2").attach(FormFicheirosAssociados); 
     
     

      async function enviarDadosParaServidor(descricao, arquivdir, dataarquivo, observficheiro, obj_associaficheiro_id,opcaoarq) {
        try {
          const url = apiEndpoint + 'FicheirosAssociadosGeral/';
          const jwtToken = authtoken;  

          const formData = new FormData();
          const emptyFile = new File([""], "empty.txt", { type: "text/plain" });

          formData.append("File", emptyFile); 

          formData.append('Descricao', descricao);
          formData.append('ArquivDir', arquivdir);
          formData.append('Datafile', dataarquivo);
          formData.append('Observacao', observficheiro);
          formData.append("Userid", userid); 
          formData.append("Registoid", obj_associaficheiro_id);
          formData.append("OpcaoArq", opcaoarq);
          
          const response = await fetch(url, {
            method: "POST",
            body: formData,
            headers: {
              'Authorization': `Bearer ${jwtToken}`,
            },
          });
      
          if (response.ok) {
            // Seu código existente para lidar com uma resposta bem-sucedida
            const refreshFiles = () => dispatch({
              type: Actions.REFRESH_FILES_START,
              payload: ""
            });
            
            
            const alertBox = dhx.alert({
              header: "Carregamento efectuado",
              text: "O ficheiro foi associado com sucesso!",
              buttonsAlignment: "center",
              buttons: ["ok"]
            }).then(function (i) {
              //clearTimeout(autoClose);
              fetchDataListaDeFicheiros();
            });
            
            /*const autoClose = setTimeout(() => {
              if (alertBox) {
                alertBox.destructor();
                fetchDataListaDeFicheiros();
              }
            }, 3000);*/
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        } catch (error) {
          console.error(error);
        }
      }
      
      
      windowRef.current?.attach(layoutRefgeometrias.current);
 
      windowRef.current?.events.on("move", function(position, oldPosition, side) {
        console.log("The window is moved to " + position.left, position.top)
      });
      
      windowRef.current?.events.on("afterHide", function(position, events){
          console.log("A window is hidden", events);
          handleClose();
          
      });

      const fetchDataListaDeFicheiros= async () => {
        let datalist= []; 
        let datalist2 = []; 
        console.log(obj_info_ids);
        console.log(obj_info);
        console.log(obj_info_tipo);

        let idItem=obj_info_ids.split('|');
        const url3 = apiEndpoint + 'FicheirosAssociadosGeral/' + idItem[0];
        // const response = await fetch(url3);
        const jwtToken = authtoken;
        //const json = await response.json();
        fetch(url3, {
          headers: {
            'Authorization': `Bearer ${jwtToken}`
          }
        })
        .then(response => {
          if (!response.ok) {
            throw new Error('Resposta da rede não foi ok');
          }
          return response.text(); // Altere isso de response.json() para response.text()
        })
        .then(text => {
          console.log('Resposta bruta:', text); // Registre a resposta bruta
          return JSON.parse(text); // Em seguida, analise-a como JSON
        })
        .then(jsonData => {
          datalist = jsonData;

          for (let index = 0; index < datalist.length; index++) {
            const element = datalist[index];
            let nomeitemficheiro= element.descricao;
                datalist2.push(element); 
          }
          ListaFicheirosAssociados.current.data.removeAll();
          ListaFicheirosAssociados.current.data.parse(datalist2); 

          if (permissaoFicheirosAssociados_Editar){
            FormFicheirosAssociados.getItem("upload_ficheiro").enable();
            //FormFicheirosAssociados.getItem("remove_ficheiro").enable();
          } else {
            FormFicheirosAssociados.getItem("upload_ficheiro").disable();
            //FormFicheirosAssociados.getItem("remove_ficheiro").enable();
          }
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        }); 
        
      }	

      fetchDataListaDeFicheiros();

      
      // Função para controlar a visibilidade do botão Adicionar e do campo Arquivo/Dir
      function toggleElementsVisibility() {
        const opcaoSelecionada = FormFicheirosAssociados.getItem("opcao_arquivo").getValue();
        const botaoAdicionar = FormFicheirosAssociados.getItem("upload_ficheiro");
        const campoArquivDir = FormFicheirosAssociados.getItem("arquivdir");
        //const BotaoArquivDir = FormFicheirosAssociados.getItem("selecionar_arquivo");


        const campoDescricao = FormFicheirosAssociados.getItem("descricao");
        const campoData = FormFicheirosAssociados.getItem("dataarquivo");  
        const campoObserv = FormFicheirosAssociados.getItem("observficheiro");

        // Desselecionar qualquer item na ListaFicheirosAssociados
        if (ListaFicheirosAssociados.current.selection) {
          ListaFicheirosAssociados.current.selection.remove();
        }
      
        
       
        //FormFicheirosAssociados.getItem("remove_ficheiro").hide();
        function formatarDataAtual() {
          const data = new Date();
          const ano = data.getFullYear();
          const mes = String(data.getMonth() + 1).padStart(2, '0'); // Mês começa em 0, então adicionamos 1
          const dia = String(data.getDate()).padStart(2, '0');
          return `${ano}/${mes}/${dia}`;
        }

        if (opcaoSelecionada === "copiar") {
            botaoAdicionar.show();
            campoArquivDir.hide();
          

            campoDescricao.disable();
            campoData.disable();
            campoObserv.disable();
            FormFicheirosAssociados.getItem("save_ficheiro").hide();
            FormFicheirosAssociados.setValue({ "descricao": '' });
            FormFicheirosAssociados.setValue({ "dataarquivo": formatarDataAtual() });
            FormFicheirosAssociados.setValue({ "observficheiro": '' }); 
        } else if (opcaoSelecionada === "gravar") {
            botaoAdicionar.hide();
            campoArquivDir.show();
            //BotaoArquivDir.show();
            FormFicheirosAssociados.getItem("save_ficheiro").show();
            campoDescricao.enable();
            campoData.enable();
            campoObserv.enable();

            FormFicheirosAssociados.setValue({ "descricao": '' });
            FormFicheirosAssociados.setValue({ "dataarquivo": formatarDataAtual() });
            FormFicheirosAssociados.setValue({ "observficheiro": '' });
            FormFicheirosAssociados.setValue({ "arquivdir": '' });
        }
      }

      FormFicheirosAssociados.getItem("downloaditem").hide();
      
      // Adicionar evento de mudança ao radioGroup
      FormFicheirosAssociados.getItem("opcao_arquivo").events.on("change", toggleElementsVisibility);
      
      // Chamar a função inicialmente para definir o estado correto dos elementos
      toggleElementsVisibility();

      let reciditemficheiro;
      let itemficheironame;
      let abriroucopiar;
     ListaFicheirosAssociados.current.events.on("click", function(id,event) {
              var item = ListaFicheirosAssociados.current.data.getItem(id);
              var itemdesc = item.descricao;
              var itemdatagrav = item.dataemissao;
              var observ = item.observacoes;
              var modogravacao = item.modogravacao;
              reciditemficheiro = item.recid;
              itemficheironame = item.ficheirolink;

              
              
              
              let formattedDate = formatDate(itemdatagrav);
              
              // Selecionar a opção correta baseada no modogravacao
              if (modogravacao === "LINK") {
                  FormFicheirosAssociados.getItem("opcao_arquivo").setValue("gravar");
                  FormFicheirosAssociados.getItem("opcao_arquivo").disable();
                   
                  FormFicheirosAssociados.getItem("arquivdir").show(); // Mostrar o campo Arquivo/Dir
                  //FormFicheirosAssociados.getItem("selecionar_arquivo").hide(); // Mostrar o botão Selecionar
                  function isHttpLink(url) {
                    return url.startsWith("http://") || url.startsWith("https://");
                  }
                
                  // Exemplo de uso
                  if (isHttpLink(itemficheironame)) {
                    FormFicheirosAssociados.getItem("abrir_ficheiro").show();
                    abriroucopiar = "abrir";
                  } else {
                    FormFicheirosAssociados.getItem("abrir_ficheiro").show();
                    abriroucopiar = "copiar";
                  }
                
                  
                  // Preencher os campos
                  FormFicheirosAssociados.setValue({
                    "descricao": itemdesc,
                    "dataarquivo": formattedDate,
                    "observficheiro": observ,
                    "arquivdir": itemficheironame // Adicionado para preencher o campo Arquivo/Dir
                  });

                  FormFicheirosAssociados.getItem("downloaditem").hide();
                  FormFicheirosAssociados.getItem("save_ficheiro").hide();

                  FormFicheirosAssociados.getItem("descricao").disable();
                  FormFicheirosAssociados.getItem("dataarquivo").disable();
                  FormFicheirosAssociados.getItem("observficheiro").disable();
                  FormFicheirosAssociados.getItem("arquivdir").disable();
      
              } else {
                  FormFicheirosAssociados.getItem("opcao_arquivo").setValue("copiar");
                  FormFicheirosAssociados.getItem("opcao_arquivo").disable();
                  FormFicheirosAssociados.getItem("arquivdir").hide();
                  //FormFicheirosAssociados.getItem("selecionar_arquivo").hide(); 
                  FormFicheirosAssociados.getItem("abrir_ficheiro").hide();
                  FormFicheirosAssociados.getItem("upload_ficheiro").hide();
                  // Preencher os campos
                  FormFicheirosAssociados.setValue({
                    "descricao": itemdesc,
                    "dataarquivo": formattedDate,
                    "observficheiro": observ
                  });
      
                  FormFicheirosAssociados.getItem("downloaditem").show();
                  
              }
          
      });
     ListaFicheirosAssociados.current.events.on("itemRightClick", function(id,e) {
        var item = ListaFicheirosAssociados.current.data.getItem(id);
        reciditemficheiro = item.recid;

        e.preventDefault();
        
        menutreeRefFicheiros.current.showAt(e);
        
        
    
     });
      

      function formatDate(dateString) {
        // Supondo que dateString esteja no formato AAAA/MM/DD
        
        const parts = dateString.split('/');
        const parts1 = dateString.split('-');
        if (parts1.length === 3) {
            const year = parts1[0];
            const month = parts1[1];
            const day = parts1[2];
            return `${year}/${month}/${day}`; // Formato AAAA-MM-DD
        }
        
        if (parts.length === 3) {
            const year = parts[0];
            const month = parts[1];
            const day = parts[2];
            return `${year}/${month}/${day}`; // Formato AAAA-MM-DD
        }
        return null; // Retorna null se a data não estiver no formato esperado
      }
      FormFicheirosAssociados.getItem("abrir_ficheiro").events.on("click", function(value) {
        let campoArquivDir = FormFicheirosAssociados.getItem("arquivdir")._value;
        
        if(abriroucopiar == "abrir") {
            dhx.confirm({
                header: "Abrir Link?",
                text: "Tem a certeza que pretende abrir o link?" + "("+ campoArquivDir +")",
                buttons: ["sim", "nao"],
                buttonsAlignment: "center"
            }).then(function(resposta) {
                if (resposta === false) {
                    window.open(campoArquivDir, '_blank');
                }
            });
        } else {
            // Função segura para copiar com fallback
            const copyToClipboard = (text) => {
                // Tentativa com Clipboard API moderno
                if (navigator.clipboard) {
                    navigator.clipboard.writeText(text)
                        .then(() => showSuccess())
                        .catch(err => handleError(err));
                } 
                // Fallback para browsers antigos
                else {
                    const textArea = document.createElement('textarea');
                    textArea.value = text;
                    textArea.style.position = 'fixed';
                    document.body.appendChild(textArea);
                    textArea.select();
                    
                    try {
                        const success = document.execCommand('copy');
                        if (success) {
                            showSuccess();
                        } else {
                            throw new Error('Fallback copy failed');
                        }
                    } catch (err) {
                        handleError(err);
                    } finally {
                        document.body.removeChild(textArea);
                    }
                }
            };
    
            // Feedback visual
            const showSuccess = () => {
              dhx.alert({
                header: "Sucesso",
                text: "Item copiado para a área de transferência!",
                buttonsAlignment: "center",
                buttons: ["ok"]
              });
            };
    
            const handleError = (err) => {
                console.error("Falha na cópia:", err);
                dhx.alert({
                  header: "Erro",
                  text: "Não foi possível copiar!",
                  buttonsAlignment: "center",
                  buttons: ["ok"]
                });
            };
    
            copyToClipboard(campoArquivDir);
        }
      });
    
    

      FormFicheirosAssociados.getItem("downloaditem").events.on("click", function(value) {
      
        const opcaoSelecionada = FormFicheirosAssociados.getItem("opcao_arquivo").getValue();
        var modogravacao = '';
        if (opcaoSelecionada=='copiar'){
          modogravacao = 'BINARIO';
        }
        if (opcaoSelecionada=='gravar'){
          modogravacao = 'LINK';
        } 
        console.log(modogravacao);
        if (modogravacao === "BINARIO") {
          const getitemficheiro = async () => {
            const jwtToken = authtoken;
            const url3 = apiEndpoint + 'FicheirosAssociados/' + reciditemficheiro;
            const response = await  fetch(url3, {
              headers: {
                'Authorization': `Bearer ${jwtToken}`
              }
            });
      
  
            if (response.ok) {
        
              // Read the response as a blob
              const fileBlob = await response.blob();
              
              const fileUrl = URL.createObjectURL(fileBlob, { type: 'image/png' });
              //window.open(fileUrl);
              
              const downloadLink = document.createElement('a');
              downloadLink.href = fileUrl;
              downloadLink.download = itemficheironame;
              downloadLink.click();
              
            };
          };
          getitemficheiro();
        }
       
      });

      FormFicheirosAssociados.getItem("upload_ficheiro").events.on("click", function(value) {
        //console.log('SHOW_ASSOCIAFILE_REGISTOATENDIMENTO'); 
        //chama Janela Associar Ficheiro 
        let idItem=obj_info_ids.split('|');
        var aux={ type: Actions.SHOW_ASSOCIAFILE, payload: idItem[0]};
        viewer.dispatch(aux); 

      });

      FormFicheirosAssociados.getItem("save_ficheiro").events.on("click", function(value) {
        const formValues = FormFicheirosAssociados.getValue();
        let idItem=obj_info_ids.split('|');
        const obj_associaficheiro_id = idItem[0];
        const descricao = formValues.descricao;
        const arquivdir = formValues.arquivdir;
        const dataarquivo = formValues.dataarquivo;
        const observficheiro = formValues.observficheiro;
        const opcaoarq = formValues.opcao_arquivo;
        // Agora você tem os valores dos campos
        console.log("Descrição:", descricao);
        console.log("Ficheiro/Directório:", arquivdir);
        console.log("Data:", dataarquivo);
        console.log("Observações:", observficheiro);
      
        if(arquivdir == "" || descricao ==""){
          dhx.alert({
            header: "Falta item Descrição ou Ficheiro/Directório",
            text: "Os items Descrição e Ficheiro/Directório são necessarios!",
            buttonsAlignment: "center",
            buttons: ["ok"]
          });
        }else{
          enviarDadosParaServidor(descricao, arquivdir, dataarquivo, observficheiro, obj_associaficheiro_id,opcaoarq);
        }
      }); 
    

      if (windowRef.current!=undefined){
        windowRef.current.show();
      }else {
        menuRef.current?.destructor();
        tabRef.current?.destructor();
        windowRef.current = null;
        menuRef.current = null;
        tabRef.current = null;
        windowRef.current = null;
      } 
    } else {
      // pretensaoId está vazio logo destroy the window e tudo o resto
 
      menuRef.current?.destructor();
      tabRef.current?.destructor();
      windowRef.current = null;
      menuRef.current = null;
      tabRef.current = null;
      windowRef.current = null;
    }

    return () => {
      menuRef.current?.destructor();
      tabRef.current?.destructor();
      windowRef.current?.destructor();
      menuRef.current = null;
      tabRef.current = null; 
      windowRef.current = null;
    };
  }, [obj_ficheiro_show]);

  useEffect(() => {
    if (refreshFicheiros){
      const fetchDataListaDeFicheiros= async () => {
        let datalist= []; 
        let datalist2 = [];  
        let idItem=obj_info_ids.split('|');
        let url3 = apiEndpoint + 'FicheirosAssociadosGeral/' + idItem[0];
        // const response = await fetch(url3);
        let jwtToken = authtoken;
        //const json = await response.json();
        fetch(url3, {
          headers: {
            'Authorization': `Bearer ${jwtToken}`
          }
        })
        .then(response => {
          if (!response.ok) {
            throw new Error('Resposta da rede não foi ok');
          }
          return response.text(); // Altere isso de response.json() para response.text()
        })
        .then(text => {
          console.log('Resposta bruta:', text); // Registre a resposta bruta
          return JSON.parse(text); // Em seguida, analise-a como JSON
        })
        .then(jsonData => {
          datalist = jsonData;

          for (let index = 0; index < datalist.length; index++) {
            const element = datalist[index];
            let nomeitemficheiro= element.descricao;
                datalist2.push(element); 
          }
          ListaFicheirosAssociados.current.data.removeAll();
          ListaFicheirosAssociados.current.data.parse(datalist2); 
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        }); 
      }
      fetchDataListaDeFicheiros();
      //dispatch({type: Actions.REFRESH_FILES_REGISTOATENDIMENTO_END, payload: ""}); 
    }
  }, [refreshFicheiros]); 
  return (
    <div ref={windowRef}></div>
    
  );
  
}

FormGU_FicheirosAssociados.propTypes = {
  obj_ficheiro_show: PropTypes.boolean,
};
export default FormGU_FicheirosAssociados;
