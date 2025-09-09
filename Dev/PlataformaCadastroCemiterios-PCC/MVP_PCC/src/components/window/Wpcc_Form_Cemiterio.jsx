import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';


const PCC_Form_Cemiterio= ({form_cemiterioedit_recId}) => {
  const windowRef = useRef(null);  
  const menuRef = useRef(null);
  const tabRef = useRef(null); 

  const [previousCemiterioId, setPreviousCemiterioId] = useState(null);
  const apiEndpointCadastro = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointCadastro);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  
  const [data, setData] = useState(null);

  const layoutRefgeometrias = useRef(null);
  const FormDadosEnt = useRef(null);

  const handleClose = () => { 
     
    var aux={ type: Actions.CEMITERIO_FECHA, payload: "" };
    viewer.dispatch(aux); 
    
  };

 

  useEffect(() => {

    if ((form_cemiterioedit_recId !== previousCemiterioId)) {
 
      const fetchData = async () => {//ok
        const url = apiEndpointCadastro + 'Cemiterio/' + form_cemiterioedit_recId;//ok
        const jwtToken = authtoken; 
        const response = await fetch(url, {
					method: 'GET', 
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`
          },
        });              
					 
        const json = await response.json();
        console.log('resposta fetchData : ' + JSON.stringify(json));
        // Update the state with the fetched data
        setData(json);
        
      }; 
      if (form_cemiterioedit_recId!='principal'){
        fetchData();
      }      
    } 
  }, [form_cemiterioedit_recId,previousCemiterioId]);

  useEffect(() => {
    // Update the form when data.nif changes
    if (FormDadosEnt.current !== null) {
     
        FormDadosEnt.current.setValue({ "nome": data?.nome }); 
        FormDadosEnt.current.setValue({ "morada": data?.morada });
        FormDadosEnt.current.setValue({ "dicofre": data?.dicofre });
        FormDadosEnt.current.setValue({ "rec_id": data?.recId });
    }
  }, [data?.recId]);


  useEffect(() => { 
   let novo_cemiterio = false;
   if (form_cemiterioedit_recId =='principal') {
      novo_cemiterio = true;
   }

   if (((form_cemiterioedit_recId !=""))) {

       
      const windowHtmlContainer = `<div id="formContainer"></div>`
      windowRef.current = new WindowDHX({
        width: 425,
        height: 475,
        title: 'Ficha de Cemitério', 
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
      FormDadosEnt.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        padding: 17,
        rows: [
          { css: "pcc_input",type: "input", name: "nome", label: "Nome", placeholder: "Nome", required: true }, 
          { css: "pcc_input",type: "input", name: "morada", label: "Morada", placeholder: "Morada", required: true }, 
          { css: "pcc_input",type: "input", name: "dicofre", label: "Localidade", placeholder: "Localidade", required: true }, 
          {
            css: "pcc_input",type: "fieldset", label: "Dados auxiliares:", name: "g3", disabled: true, hidden: true, 
            rows:[  
              { css: "pcc_input",type: "input", name: "rec_id", label: "rec_id", placeholder: "rec_id", required: true }, 
            ]},
          { css: "pcc_input", id: "buttonsave", type: "button",  circle: true, value: "Gravar", tooltip: "Gravar", title: "Gravar" }
        ] 
      }); 
      
      FormDadosEnt.current.getItem("buttonsave").events.on("click", function(events) {
        console.log("click", events);
        handleSave();
      });

      function handleSave() {
        
        let objform_nome = FormDadosEnt.current.getItem("nome").getValue()|| '';
        let objform_morada = FormDadosEnt.current.getItem("morada").getValue()|| '';
        let objform_dicofre = FormDadosEnt.current.getItem("dicofre").getValue()|| '';
        let objform_recid = FormDadosEnt.current.getItem("rec_id").getValue()|| '0';
        console.log(objform_nome);
        if(objform_nome.trim() == ""){
          dhx.alert({
            header:"Erro ao gravar",
            text:"É necessário que o campo (Nome*) esteja preenchido",
            buttonsAlignment:"center",
            buttons:["ok"],
          });
        }else{         
          onsaveWindow(objform_recid, objform_nome, objform_morada, objform_dicofre);  
        }
      }
 
      windowRef.current?.attach(FormDadosEnt.current);
 
      windowRef.current?.events.on("move", function(position, oldPosition, side) {
        console.log("The window is moved to " + position.left, position.top)
      });
      
      windowRef.current?.events.on("afterHide", function(position, events){
          console.log("A window is hidden", events);
          handleClose();
          
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
  }, [form_cemiterioedit_recId]);

  const onsaveWindow = (objform_recid, objform_nome, objform_morada, objform_dicofre) => {   
    const contSave = (objform_recid, objform_nome, objform_morada, objform_dicofre)=> {   
        const saveCemiterio = async () => { 
          const jwtToken = authtoken;           
          const url = apiEndpointCadastro + 'Cemiterio/';//ok 
          let metodo='POST';
          if (objform_recid!='0'){
            metodo='PUT';
          }
          const recIdAsInt = parseInt(objform_recid, 10) || 0;          
          const response = await fetch(url, {
            method: metodo,
            body: JSON.stringify({ 
                recId: recIdAsInt,
                nome: objform_nome,
                morada: objform_morada, 
                dicofre: objform_dicofre
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
              // mapa refresh
              // tree refresh      
              //refreshDataGeometrias();   
              const data = await response.json();
              if (data!=undefined){
                  dhx.alert({
                    header:"Gravação efectuada",
                    text:"A Gravação foi efetuada com sucesso!",
                    buttonsAlignment:"center",
                    buttons:["ok"],
                  }).then(function(){
                    windowRef.current.hide();  
                    var item = data.recId + data.nome; 
                    if (metodo=='PUT'){ 
                      viewer.dispatch({type: Actions.TREECEM_UPDATE, payload: { rec_id: data.recId, nome: data.nome, objtipo: 'pai'  } }) 
                        
                    }
                    if (metodo=='POST'){ 
                      viewer.dispatch({type: Actions.TREECEM_INSERT, payload: {  rec_id: data.recId, nome: data.nome , objtipo: 'pai' } }) 
                      
                    } 
                  }); 
              } else {
                // resposta == "0"

              }
              
              
            
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        saveCemiterio();       
    } 
    
    contSave(objform_recid, objform_nome, objform_morada, objform_dicofre);
    
  }
  return (
    <div ref={windowRef}></div>
    
  );
  
}

PCC_Form_Cemiterio.propTypes = { 
  form_cemiterioedit_recId: PropTypes.string.isRequired,
};
export default PCC_Form_Cemiterio;
