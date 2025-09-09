import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';
import { processData } from '../../utils/utils';

const Wpcc_Form_CartografiaOrdenar= ({obj_cartografiaordenar_show}) => {
  const windowRef = useRef(null);  
 
  const [dataCartografia, setDataCartografia] = useState(null);
  const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint);
  const apiEndpointSIG = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointSIG);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid);

  const layoutRefgeometrias = useRef(null); 
  const treeCartografia = useRef(null);
  const FormDadosEnt = useRef(null);

  const handleClose = () => { 
    console.log('Close Ordenar Impressões');
    
    var aux={ type: Actions.HIDE_CARTOGRAFIA_ORDENA, payload: "" };
    viewer.dispatch(aux); 
    
  };

  useEffect(() => {
      
    const fetchDataTreeImpressoes = async () => {//ok
       
       
      const url = apiEndpointSIG + 'ArvoreCartografia/' + userid;//ok
      const jwtToken = authtoken; 

		  async function fetchDataImpressoes() {
        try {
 
          console.log('jwtToken: ' + jwtToken);

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
            //setDataImpressoes(jsonData); 
            //console.log(jsonData);
            let newdados=processData(jsonData); 
            treeCartografia.current.data.removeAll();
            treeCartografia.current.data.parse(newdados);
            treeCartografia.current.expandAll();
            
          })
          .catch(error => {
            console.error('There was a problem with your fetch operation:', error);
          }); 
        } catch (e) {
            console.log(e.message || 'Unexpected error');
        }

      }
 
      fetchDataImpressoes();
  
    }
    fetchDataTreeImpressoes();
  
  
}, [obj_cartografiaordenar_show]);

useEffect(() => {
 
  if ((treeCartografia.current !=null )&&(dataCartografia !=null )){

    treeCartografia.current.data.removeAll();
    treeCartografia.current.data.parse(jsonData);
    treeCartografia.current.expandAll();
    
    setDataCartografia(dataCartografia);
  }


}, [dataCartografia]);

  useEffect(() => {
    if (obj_cartografiaordenar_show !== false) {
       
      const windowHtmlContainer = `<div id="formContainer"></div>`
      windowRef.current = new WindowDHX({
        width: 500,
        height: 700,
        title: 'Ordenar Cartografia', 
        closable: true,
        movable: true,
        resizable: true,
        modal: false,
        header: true,
        footer: true,
        css: "pcc_window",
      });
      globalWindowReference = windowRef.current;
  
      // criar a árvore das geometrias
      treeCartografia.current = new TreeDHX(null,{
        checkbox: false, 
        dragMode:"both", 
        dropBehaviour:"complex",
        isFolder: function (item) {
          return item.tipo === "GRUPO";
        }
      });

        

      function closeWindow() {
        if (windowRef.current) {
          windowRef.current.close();
        }
      
      }

      FormDadosEnt.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_layout_interv",
        padding: 17,
        cols:  
            [ 
              { type: "spacer", height: 15 }, // Espaço antes das rows  
              { css: "pcc_input", id: "buttonsave", type: "button", value: "Gravar", tooltip: "Gravar", title: "Gravar" },
            ]  
      }); 
      
      FormDadosEnt.current.getItem("buttonsave").events.on("click", function(events) {
        console.log("click", events);

        handleSave();
      });

      function handleSave() {
        const contSaveImpressoes = (dados)=> {   
          const saveImpressoes  = async (dados) => { 
                              
            const url = apiEndpointSIG + 'Cartografia/Ordena';//ok 
            const jwtToken = authtoken;        
            const response = await fetch(url, {
              method: 'PUT',
              body: JSON.stringify(dados),
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${jwtToken}`
                },
            });              
            if (response.ok) {
              dhx.alert({
                header:"Gravação efectuada",
                text:"A Gravação foi efetuada com sucesso!",
                buttonsAlignment:"center",
                buttons:["ok"],
              }).then(function(){

                var aux = { 
                  type: Actions.TREEGUCARTOGRAFIA_RELOADTREE, 
                  payload: {
                    reloadKey: Date.now().toString()
                  }
                };
                viewer.dispatch(aux);

                ReloadData();
              });
            } else {
              dhx.alert({
								header: "Erro",
								text: "Não é possível mover o ITEM para fora de um grupo ou para dentro de outro ITEM.",
								buttonsAlignment: "center",
								buttons: ["ok"],
							});
              ReloadData();
            }
          };
          saveImpressoes (dados);     
        }
        if (FormDadosEnt.current) {
          // Obter os dados da árvore
          let treeDataString = treeCartografia.current.data; // Supondo que isso seja uma string JSON
      
          // Log para verificar o tipo de dados
          //console.log("Tipo de dados:", typeof treeDataString);
          //console.log("Dados da árvore:", treeDataString);
      
          let treeData;
          try {
              // Verifica se é uma string antes de tentar analisar
              if (typeof treeDataString === 'string') {
                  treeData = JSON.parse(treeDataString); // Parseia a string JSON
              } else {
                  treeData = treeDataString; // Se já for um objeto, use diretamente
              }
          } catch (error) {
              console.error("Erro ao analisar a árvore:", error);
              return; // Interrompe se houver erro
          }
      
          // Função para processar a árvore e coletar rec_id, parentid, tipo e value
          function processTree(items, result = []) {
              items.forEach(item => {
                if (item.tipo=='cartografia'){
                  result.push({
                    rec_id: item.rec_id,
                    parentid: treeCartografia.current.data?.getItem(item.parent)?.rec_id,
                    tipo: item.tipo,
                    value: item.value
                  });   
                } 
                if (item.items && item.items.length > 0) {
                    processTree(item.items, result);
                }
              });
              return result;
          }
      
          // Processar a árvore para obter todos os itens
          let allItems = processTree(treeData);
      
          // Criar a string para salvar
          let dadostree = allItems.map(function(element) {
              return element.rec_id + '|' + element.parentid + '|' + element.tipo + '|' + element.value;
          });
      
          let dados = dadostree.join("#");
          console.log(dados);
          
          // Chamar a função para salvar as impressões
          contSaveImpressoes(dados);
      }
 
      
      }

      const ReloadData = async () => { 
        treeCartografia.current.data.removeAll();
        const url = apiEndpointSIG + 'ArvoreCartografia/' + userid;//ok
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
          treeCartografia.current.data.parse(jsonData);
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        });
        
  
      }

      layoutRefgeometrias.current = new LayoutDHX(null,{
        type: "line",
        rows: [
            {
              id: "treegeo",
              html: "treegeo",
              css: "pcc_layout",
              height: 500,
              progressDefault: true,
          }, 
          {
            id: "dadosent",
            html: "dadosent",
            css: "pcc_layou_tipointervsolo",
            progressDefault: true,
        }
      ]});
   
      layoutRefgeometrias.current.getCell("treegeo").attach(treeCartografia.current);

      layoutRefgeometrias.current.getCell("dadosent").attach(FormDadosEnt.current);

       
      windowRef.current?.attach(layoutRefgeometrias.current);
 
      windowRef.current?.events.on("move", function(position, oldPosition, side) {
        console.log("The window is moved to " + position.left, position.top)
      });
      
      windowRef.current?.events.on("afterHide", function(position, events){
          console.log("A window is hidden", events);
          handleClose();
          
      });
       
      let objgeomid;

      treeCartografia.current.events.on("itemRightClick", function(id, e){
        console.log("The item with the id "+ id +" was right-clicked.");
       
      });

      // Evento chamado após soltar o item
      treeCartografia.current.events.on("beforeDrop", function(data) {
        const draggedItemId = data.source; // ID do item que foi arrastado
        const targetParentId = data.target; // ID do novo pai para onde o item foi solto

        // Obtém o item arrastado
        const draggedItem = treeCartografia.current.data.getItem(draggedItemId); 
        const targetItem = treeCartografia.current.data.getItem(targetParentId); // Obtém o novo pai

        // Verifica se o item arrastado é do tipo ITEM
        if (draggedItem && draggedItem.tipo === "cartografia") {
            // Verifica se o novo pai é válido
            if (!targetParentId || targetParentId === "" || (targetItem && targetItem.tipo !== "cartografia")) {

              /*dhx.alert({
								header: "Erro",
								text: "Não é possível mover.",
								buttonsAlignment: "center",
								buttons: ["ok"],
							});*/

              return false; // Cancela a ação de mover
            }
        }

        console.log("Item movido com sucesso:", draggedItemId, "para o parent:", targetParentId);
        return true; // Permite a ação de mover
      });

      if (windowRef.current!=undefined){
        windowRef.current.show();
      }else {
        
        windowRef.current = null; 
      } 
    } else {
      // pretensaoId está vazio logo destroy the window e tudo o resto
 
       
      windowRef.current = null;
    }

    return () => {
       
      windowRef.current = null;
    };
  }, [obj_cartografiaordenar_show]);

   

  return (
    <div ref={windowRef}></div>
    
  );
  
}

Wpcc_Form_CartografiaOrdenar.propTypes = {
  obj_cartografiaordenar_show: PropTypes.boolean,
};
export default Wpcc_Form_CartografiaOrdenar;
