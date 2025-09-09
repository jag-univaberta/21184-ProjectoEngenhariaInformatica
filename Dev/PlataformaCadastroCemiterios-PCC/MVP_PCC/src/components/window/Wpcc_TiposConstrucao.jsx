import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';
 
const Wpcc_TiposConstrucao= ({obj_tiposconstrucao_show}) => {
  const windowRef = useRef(null);  
  const menuRef = useRef(null);
  const tabRef = useRef(null);

  const [dataTiposConstrucao, setDataTiposConstrucao] = useState(null); 
  const apiEndpointCadastro = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointCadastro);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  
  const layoutReferencia = useRef(null);
  const menutreeReferencia = useRef(null); 
  const menutreeGrupo = useRef(null);
  const treeReferencia = useRef(null);
  const FormDadosEnt = useRef(null);

  const handleClose = () => { 
    console.log('Close Tipos de Construção');    
    var aux={ type: Actions.HIDE_GESTAOTIPOSCONSTRUCAO, payload: "" };
    viewer.dispatch(aux);     
  };

  useEffect(() => {     
    const fetchdataTiposConstrucao = async () => {//ok
      const url3 = apiEndpointCadastro + 'TipoConstrucao';//ok 
      console.log(url3);
      const jwtToken = authtoken; 
      fetch(url3, {
        headers: {
          'Authorization': `Bearer ${jwtToken}`
        }
      })
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        FormDadosEnt.current.getItem("buttonsave").disable();
       
        return response.json();
      })
      .then(jsonData => {
        setDataTiposConstrucao(jsonData); 
      })
      .catch(error => {
        console.error('There was a problem with your fetch operation:', error);
      });  
    } 
    fetchdataTiposConstrucao();   
  }, [obj_tiposconstrucao_show]);

  useEffect(() => {
    const transformResponse = (originalResponses) => {
    //function transformResponse(originalResponses) {
      // Initialize an empty array to store the transformed data
      const transformedData = [];
      // Initialize a counter for appending to descriptions
      let counter = 1;
      if (Array.isArray(originalResponses)){
        if (originalResponses.length==0){
          transformedData[0] = transformedData[0] || {
            value: "Tipos de Construção",
            id: "update",
            opened: true,
            items: [],
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
                obs: observacao,
                movimentos: movimentosn,
                icon: {
                  folder: "fas fa-book",
                  openFolder: "fas fa-book-open",
                  file: "fas fa-file",
                },
              };

              // Push the transformed record into the items array
              transformedData[0] = transformedData[0] || {
                value: "Tipos de Construção",
                id: "update",
                opened: true,
                items: [],
              };
              transformedData[0].items.push(transformedRecord);
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
          obs: observacao,
          movimentos: movimentosn,
          icon: {
            folder: "fas fa-book",
            openFolder: "fas fa-book-open",
            file: "fas fa-file",
          },
        };
    
        // Push the transformed record into the items array
        transformedData[0] = transformedData[0] || {
          value: "Tipos de Construção",
          id: "update",
          opened: true,
          items: [],
        };
        transformedData[0].items.push(transformedRecord);
      }   
      return transformedData;
    }      
    if ((treeReferencia.current !=null )&&(dataTiposConstrucao !=null )){
      const transformedResult = transformResponse(dataTiposConstrucao); 
      treeReferencia.current.data.parse(transformedResult);
      setDataTiposConstrucao(dataTiposConstrucao);
    }
  }, [dataTiposConstrucao]);

  useEffect(() => {
    if (obj_tiposconstrucao_show !== false) {
       
      const windowHtmlContainer = `<div id="formContainer"></div>`
      windowRef.current = new WindowDHX({
        width: 600,
        height: 700,
        title: 'Tipos de Construção', 
        closable: true,
        movable: true,
        resizable: true,
        modal: false,
        header: true,
        footer: true,
        css: "pcc_window",
      });
      globalWindowReference = windowRef.current; 
      tabRef.current = new TabbarDHX(null,{
        css: "pcc_tab",
        tabs:[]
      });
      tabRef.current.addTab({ id: "geral", tab: "Geral"}, 1);
      treeReferencia.current = new TreeDHX(null,{
        checkbox: false 
      });
      menutreeGrupo.current = new ContextMenuDHX(null, {
        css: "dhx_widget--bg_gray",
        data: [
          { id: "new", value: "Novo"}
        ]
      });
       menutreeReferencia.current = new ContextMenuDHX(null, {
        css: "dhx_widget--bg_gray",
        data: [
          { id: "update", value: "Editar" }, 
          { type: "separator" } ,
          { id: "remover",value: "Eliminar" } ,
        ]
      });

      menutreeReferencia.current.events.on("click", function(id,e){
        console.log(id);
        let obj_nome;
        let obj_obs;
        let obj_movimentos;
        switch(id){
          case 'update':
            console.log('update');            
            obj_nome= treeReferencia.current.data.getItem(objgeomid)?.value;
            obj_obs= treeReferencia.current.data.getItem(objgeomid)?.obs;
            obj_movimentos= treeReferencia.current.data.getItem(objgeomid)?.movimentos;
 
            FormDadosEnt.current.setValue({ "nome": obj_nome });
            FormDadosEnt.current.setValue({ "observacao": obj_obs }); 
            FormDadosEnt.current.getItem("chkMovimentos").setValue(obj_movimentos); 
            FormDadosEnt.current.getItem("buttonsave").enable();
           
            break;
          case 'remover':
            console.log('remover');            
            ondeleteWindow(objgeomid); 
            break;
          default:
            break;
        }
      });

      menutreeGrupo.current.events.on("click", function(id,e){
        console.log(id);
        switch(id){
          case 'new':
            console.log('new'); 
            FormDadosEnt.current.setValue({ "nome": "" });
            FormDadosEnt.current.setValue({ "observacao": "" });
            FormDadosEnt.current.setValue({ "chkMovimentos": "" });
            FormDadosEnt.current.getItem("buttonsave").enable();           
            break;
          default:
            break;
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
        rows: [
          {
            css: "pcc_input", type: "fieldset", label: "Tipo de Construção: ", name: "g1", disabled: false, hidden: false, height: "content",
            rows: 
            [ 
              { type: "spacer", height: 15 }, // Espaço antes das rows
              { css: "pcc_input", id: "objformname", type: "input", name: "nome", label: "Descrição:", placeholder: "Descrição", required: true },
              { css: "pcc_input", id: "objformobs", type: "textarea", name: "observacao", label: "Observação:", placeholder: "Observação", value: "", height: 100, maxlength: 950},
              { type: "checkbox", label: "Permite movimentos (s/n):", labelPosition: "left", name: "chkMovimentos", id: "chkMovimentos"}, 
              { css: "pcc_input", id: "buttonsave", type: "button",  circle: true, value: "Gravar", tooltip: "Gravar", title: "Gravar" },
            ] 
          }
        ] 
      }); 
      
      FormDadosEnt.current.getItem("buttonsave").events.on("click", function(events) {
        console.log("click", events);
        handleSave();
      });

      function handleSave() {
        if (FormDadosEnt.current) {
          let objformnome = document.getElementById("objformname").value;
          let objformobs = document.getElementById("objformobs").value;
          let objformparent = treeReferencia.current.data.getItem(objgeomid)?.parent;
          let objmovimentos = FormDadosEnt.current.getItem("chkMovimentos").getValue();

          if(objformparent == "update"){
            let objformid= treeReferencia.current.data.getItem(objgeomid)?.id;
            onupdateWindow(objformid, objformnome, objformobs, objmovimentos); 
          }else{
            onsaveWindow(objformnome, objformobs, objmovimentos);
          }

        }
      
      }

      layoutReferencia.current = new LayoutDHX(null,{
        type: "line",
        rows: [
            {
              id: "treegeo",
              html: "treegeo",
              css: "pcc_layout",
              height: 157,
              progressDefault: true,
          }, 
          {
            id: "dadosent",
            html: "dadosent",
            css: "pcc_layou_tipointervsolo",
            progressDefault: true,
        }
      ]});
   
      layoutReferencia.current.getCell("treegeo").attach(treeReferencia.current);

      layoutReferencia.current.getCell("dadosent").attach(FormDadosEnt.current);

      tabRef.current.getCell("geral").attach(layoutReferencia.current); 
     
      windowRef.current?.attach(tabRef.current);
 
      windowRef.current?.events.on("move", function(position, oldPosition, side) {
        console.log("The window is moved to " + position.left, position.top)
      });
      
      windowRef.current?.events.on("afterHide", function(position, events){
          console.log("A window is hidden", events);
          handleClose();
          
      });
       
      let objgeomid;

      treeReferencia.current.events.on("itemRightClick", function(id, e){
        console.log("The item with the id "+ id +" was right-clicked.");
        objgeomid=id;
        if(objgeomid != "update"){
          e.preventDefault();
          menutreeReferencia.current.showAt(e);
        }else{
          e.preventDefault();
          menutreeGrupo.current.showAt(e);
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
  }, [obj_tiposconstrucao_show]);

  const ondeleteWindow = (objgeomid) => {
    const DeleteObjGeometry = (objgeomid)=> {
      try {				
        const savePretensaoObj = async () => {                   
          const url = apiEndpointCadastro + 'TipoConstrucao';//ok   
          const jwtToken = authtoken;         
          const response = await fetch(url, {
            method: 'DELETE', 
            body: JSON.stringify({
              recid: objgeomid,
              designacao: ''
            }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
              // mapa refresh
              // tree refresh      
              FormDadosEnt.current.setValue({ "nome": "" });
              FormDadosEnt.current.setValue({ "obseervacao": "" });
              refreshDataTiposConstrucao();               
            
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        savePretensaoObj();
      } catch (error) {
        console.error('error: ' + error);                 
      } 
    }
     

    dhx.confirm({
      header:"Remover tipo de construção",
      text:"Tem a certeza que pretende remover o tipo de Construção?",
      buttons:["sim", "nao"],
      buttonsAlignment:"center"
    }).then(function(resposta){
      console.log('resposta ', resposta);

      switch(resposta){
        case false:
          DeleteObjGeometry(objgeomid); 
          break;
        default:
          break;
      }
    });

    const refreshDataTiposConstrucao = async () => {//ok
      const url3 = apiEndpointCadastro + 'TipoConstrucao';//ok 
      const jwtToken = authtoken; 
      fetch(url3, {
        headers: {
          'Authorization': `Bearer ${jwtToken}`
        }
      })
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        FormDadosEnt.current.getItem("buttonsave").disable(); 
        return response.json();
      })
      .then(jsonData => {
        setDataTiposConstrucao(jsonData); 
      })
      .catch(error => {
        console.error('There was a problem with your fetch operation:', error);
      });  
    }

    
  }

  const onsaveWindow = (objformnome, objformobs, objmovimentos) => {   
    const contSave = (objformnome, objformobs, objmovimentos)=> {   
        const saveTipoConstrucao = async () => {  
          const url = apiEndpointCadastro + 'TipoConstrucao/';//ok     
          const jwtToken = authtoken;    
          const response = await fetch(url, {
            method: 'POST',
            body: JSON.stringify({ 
                recid: 0,
                designacao: objformnome,
                observacao: objformobs,
                movimentosn: objmovimentos
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {   
            FormDadosEnt.current.setValue({ "nome": "" });
            FormDadosEnt.current.setValue({ "observacao": "" });    
            FormDadosEnt.current.setValue({ "chkMovimentos": "" });
            refreshDataTiposConstrucao();    
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        saveTipoConstrucao();       
    }  
    contSave(objformnome, objformobs, objmovimentos); 

    const refreshDataTiposConstrucao = async () => {//ok
      const url3 = apiEndpointCadastro + 'TipoConstrucao';//ok 
      const jwtToken = authtoken; 
      fetch(url3, {
        headers: {
          'Authorization': `Bearer ${jwtToken}`
        }
      })
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        FormDadosEnt.current.getItem("buttonsave").disable();
        return response.json();
      })
      .then(jsonData => {
        setDataTiposConstrucao(jsonData); 
      })
      .catch(error => {
        console.error('There was a problem with your fetch operation:', error);
      });  
    }
  }

  const onupdateWindow = (objformid, objformnome, objformobs, objmovimentos ) => {   
    const contSave = (objformid, objformnome, objformobs, objmovimentos)=> {   
        const savePretensaoObj = async () => {                             
          const url = apiEndpointCadastro + 'TipoConstrucao/';
          const jwtToken = authtoken;        
          const response = await fetch(url, {
            method: 'PUT',
            body: JSON.stringify({
                recid: objformid,
                designacao: objformnome,
                observacao: objformobs,
                movimentosn: objmovimentos
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
              FormDadosEnt.current.setValue({ "nome": "" });   
              FormDadosEnt.current.setValue({ "observacao": "" });      
              FormDadosEnt.current.setValue({ "chkMovimentos": "" });
              refreshDataTiposConstrucao(); 
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        savePretensaoObj();       
    } 
    
    contSave(objformid, objformnome, objformobs, objmovimentos);
     
    const refreshDataTiposConstrucao = async () => {//ok
      const url3 = apiEndpointCadastro + 'TipoConstrucao';//ok 
      const jwtToken = authtoken; 
      fetch(url3, {
        headers: {
          'Authorization': `Bearer ${jwtToken}`
        }
      })
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        FormDadosEnt.current.getItem("buttonsave").disable();       
        return response.json();
      })
      .then(jsonData => {
        setDataTiposConstrucao(jsonData); 
      })
      .catch(error => {
        console.error('There was a problem with your fetch operation:', error);
      });  
    }   
  }

  return (
    <div ref={windowRef}></div>    
  );  
}

Wpcc_TiposConstrucao.propTypes = {
  obj_tiposconstrucao_show: PropTypes.boolean,
};
export default Wpcc_TiposConstrucao;
