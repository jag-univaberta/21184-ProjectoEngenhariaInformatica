import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';

const Wpcc_Form_TalhaoOrdenar= ({obj_talhaoordenar_show}) => {
  const windowRef = useRef(null);  
 
  const [dataCadastro, setDataCadastro] = useState(null);
  const apiEndpointCadastro = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointCadastro);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid);

  const layoutRefgeometrias = useRef(null); 
  const treeCadastro = useRef(null);
  const FormDadosEnt = useRef(null);

  const handleClose = () => { 
    console.log('Close Ordenar Impressões');
    
    var aux={ type: Actions.HIDE_PRETENSAO_ORDENA, payload: "" };
    viewer.dispatch(aux); 
    
  };

  useEffect(() => {
      
    const fetchDataTreeCadastro = async () => {//ok
       
       
      const url = apiEndpointCadastro + 'ArvConstrucoes/' + userid;//ok
      const jwtToken = authtoken; 

		  async function fetchDataCadastro() {
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
       
            setCheckboxFalse(jsonData);
            
            treeCadastro.current.data.removeAll();
            treeCadastro.current.data.parse(jsonData);
            treeCadastro.current.expandAll();
            
          })
          .catch(error => {
            console.error('There was a problem with your fetch operation:', error);
          }); 
        } catch (e) {
            console.log(e.message || 'Unexpected error');
        }

      }
 
      fetchDataCadastro();
  
    }
    fetchDataTreeCadastro();
  
    function setCheckboxFalse(items) {
      items.forEach(item => {
        item.checkbox = false;
        if (item.items && item.items.length > 0) {
          setCheckboxFalse(item.items);
        }
      });
    }
}, [obj_talhaoordenar_show]);

useEffect(() => {
 
  if ((treeCadastro.current !=null )&&(dataCadastro !=null )){

    treeCadastro.current.data.removeAll();
    treeCadastro.current.data.parse(jsonData);
    treeCadastro.current.expandAll();
    
    setDataCadastro(dataCadastro);
  }


}, [dataCadastro]);

  useEffect(() => {
    if (obj_talhaoordenar_show !== false) {
       
      const windowHtmlContainer = `<div id="formContainer"></div>`
      windowRef.current = new WindowDHX({
        width: 500,
        height: 700,
        title: 'Ordenar Cadastro', 
        closable: true,
        movable: true,
        resizable: true,
        modal: false,
        header: true,
        footer: true,
        css: "pcc_window",
      });
      globalWindowReference = windowRef.current;  
      treeCadastro.current = new TreeDHX(null,{
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
        const contSaveOrdenacaoTalhoes= (dados)=> {   
          const saveOrdenacaoTalhoes= async (dados) => { 
                              
            const url = apiEndpointCadastro + 'CadastroOrdena';//ok 
            const jwtToken = authtoken;        
            const response = await fetch(url, {
              method: 'PUT',
              body: JSON.stringify({
                data: dados 
              }),
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
                var aux={ type: Actions.TREECEM_RELOADTREE,  payload: {
                  reloadKey: Date.now().toString()
                }};			
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
          saveOrdenacaoTalhoes (dados);    
        }
        if (FormDadosEnt.current) {          
          let informacao=[];
          let i=0;
          treeCadastro.current.data.forEach(function(element, index, array) { 
            console.log(element);
            let id = element.id;
            if (id!='parent'){
              let recid = element.recid;
              let parent = element.parent; 
              let parentid ='';
              if (parent!='parent'){
                parentid = treeCadastro.current.data.getItem(parent)?.recid;
                informacao.push(recid + "|" + parentid);
              } else {
                informacao.push(recid + "|");
              }
              i++; 
            }                     
          }); 
          let dados = informacao.join("#");
          console.log(dados);          
          contSaveOrdenacaoTalhoes (dados);
      }      
      }
      const ReloadData = async () => { 
        treeCadastro.current.data.removeAll();
        const url = apiEndpointCadastro + 'ArvConstrucoes/' + userid;//ok
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
          treeCadastro.current.data.parse(jsonData);
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
   
      layoutRefgeometrias.current.getCell("treegeo").attach(treeCadastro.current);
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
      treeCadastro.current.events.on("itemRightClick", function(id, e){
        console.log("The item with the id "+ id +" was right-clicked.");       
      });
      // Evento chamado após soltar o item
      treeCadastro.current.events.on("beforeDrop", function(data) {
        const draggedItemId = data.source; // ID do item que foi arrastado
        const targetParentId = data.target; // ID do novo pai para onde o item foi solto
        // Obtém o item arrastado
        const draggedItem = treeCadastro.current.data.getItem(draggedItemId); 
        const targetItem = treeCadastro.current.data.getItem(targetParentId); // Obtém o novo pai
        // Verifica se o item arrastado é do tipo ITEM
        if (draggedItem && draggedItem.tipo === "ITEM") {
            // Verifica se o novo pai é válido
            if (!targetParentId || targetParentId === "" || (targetItem && targetItem.tipo === "ITEM")) {
              dhx.alert({
								header: "Erro",
								text: "Não é possível mover o ITEM para fora de um grupo ou para dentro de outro ITEM.",
								buttonsAlignment: "center",
								buttons: ["ok"],
							});
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
      windowRef.current = null;
    }
    return () => {       
      windowRef.current = null;
    };
  }, [obj_talhaoordenar_show]);
  return (
    <div ref={windowRef}></div>    
  );  
}

Wpcc_Form_TalhaoOrdenar.propTypes = {
  obj_talhaoordenar_show: PropTypes.boolean,
};
export default Wpcc_Form_TalhaoOrdenar;
