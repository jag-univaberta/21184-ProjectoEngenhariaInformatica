import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';
 
const Wpcc_GestaoConcessionarios= ({obj_concessionarios_show}) => {
  const windowRef = useRef(null);  
  const menuRef = useRef(null);
  const tabRef = useRef(null);

  const [dadosConcessionario, setDadosConcessionario] = useState(null);
 
  const [dadoslistaConcessionarios, setDadosListaConcessionarios] = useState(null);

  const [dataDistritos, setDataDistritos] = useState(null);
  const [dataConcelhos, setDataConcelhos] = useState(null);
  const [dataFreguesias, setDataFreguesias] = useState(null);

  const [dataDicofre, setDataDicofre] = useState(null); 

  const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint);
  const apiEndpointCadastro = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointCadastro);

  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  
  const layoutRefgeometrias = useRef(null); 
  const treeRefgeometrias = useRef(null);
  const FormDadosConcessionarios = useRef(null);
  const MenuRef = useRef(null);
  const GridDadosConcessionarios = useRef(null);
  
  let datacombodistritos=[];
  let datacomboconcelhos=[];
  let datacombofreguesias=[];

  const handleClose = () => { 
    console.log('Close Gestão Concessionários');
    
    var aux={ type: Actions.HIDE_GESTAOCONCESSIONARIOS, payload: "" };
    viewer.dispatch(aux); 
    
  };
  const fetchDataGridConcessionarios = async () => {//ok
    const url3 = apiEndpointCadastro + 'Concessionarios';//ok 
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
       
      return response.json();
    })
    .then(jsonData => {
      
      const elementosConcessionario = (item) => ({
        recid: String(item.recId),
        nif: String(item.nif || ""),
        nome: String(item.nome || ""),
        morada: String(item.morada || ""),
        contacto: String(item.contacto || ""),
        dicofre: String(item.dicofre || "").trim() 
      });
      /*  { width: 0, autoWidth: true, id: "recid", hidden: true, header: [{ text: "#" }], tooltipTemplate: rowDataTemplate},  
        { width: 150, autoWidth: true, id: "nif", header: [{ text: "NIF"}], tooltipTemplate: rowDataTemplate },  
        { width: 150, autoWidth: true, id: "nome", header: [{ text: "Nome"}], tooltipTemplate: rowDataTemplate },  
        { width: 200, autoWidth: true, id: "morada", header: [{ text: "Morada"}] , tooltipTemplate: rowDataTemplate},  
        { width: 100, autoWidth: true, align: "dicofre", id: "escala", header: [{ text: "Dicofre"}] , tooltipTemplate: rowDataTemplate},  
        { width: 100, autoWidth: true, id: "contacto", header: [{ text: "Contacto"}] , tooltipTemplate: rowDataTemplate}           */
      const dados = jsonData.map(elementosConcessionario);
      setDadosListaConcessionarios(dados); 

    })
    .catch(error => {
      console.error('There was a problem with your fetch operation:', error);
    });  
  }
  const fetchDataDistritos = async () => {//ok
    const url3 = apiEndpointCadastro + 'Distrito';//ok 
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
       
      return response.json();
    })
    .then(jsonData => {
      
      const elementosDis = (item) => ({
        id: String(item.recId),
        value: String(item.nome || ""),
        di: String(item.di || "").trim() 
      });
      const dados = jsonData.map(elementosDis);
      setDataDistritos(dados); 
      
    })
    .catch(error => {
      console.error('There was a problem with your fetch operation:', error);
    });  
  }
 
  const fetchDataConcelhos = async (codigo_distrito) => {
    var comboconcelhos= FormDadosConcessionarios.current.getItem('concelho').getWidget();
    comboconcelhos.clear();
    var combofreguesias= FormDadosConcessionarios.current.getItem('freguesia').getWidget();
    combofreguesias.clear(); 
    const url = apiEndpointCadastro + 'Concelho/' + codigo_distrito;
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
          
          const elementosCon = (item) => ({
            id: String(item.recId),
            value: String(item.nome || ""),
            di: String(item.di || "").trim(),
            co: String(item.co || "").trim() 
          });
          const dados = jsonData.map(elementosCon); 
          setDataConcelhos(dados); 
 
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        }); 
  };
   const fetchDataFreguesias = async (codigo_concelho) => { 
    var combofreguesias= FormDadosConcessionarios.current.getItem('freguesia').getWidget();
    combofreguesias.clear(); 
    const url = apiEndpointCadastro + 'Freguesia/' + codigo_concelho;
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

      
      const elementosCon = (item) => ({
        id: String(item.recId),
        value: String(item.nome || ""),
        di: String(item.di || "").trim(),
        co: String(item.co || "").trim(), 
        fre: String(item.fre || "").trim(),
      });
      const dados = jsonData.map(elementosCon);
      setDataFreguesias(dados); 
      
    })
    .catch(error => {
      console.error('There was a problem with your fetch operation:', error);
    }); 
  };
  useEffect(() => {
      
    fetchDataDistritos();
   
    fetchDataGridConcessionarios();
  }, [obj_concessionarios_show]);
  useEffect(() => {
      
     if (GridDadosConcessionarios.current!=null) {    
           
      GridDadosConcessionarios.current.data.removeAll();
      GridDadosConcessionarios.current.data.parse(dadoslistaConcessionarios); 

     }
  }, [dadoslistaConcessionarios]);

  useEffect(() => {
    // Update the form when data.nif changes
    if (FormDadosConcessionarios.current!=null) {   
      var myComboBox= FormDadosConcessionarios.current.getItem('distrito').getWidget();
      myComboBox.data.parse(dataDistritos);
      if ((dataDicofre!=null)&&(dataDicofre!='')){ 
        console.log(dataDicofre);
        let distrito_id = dataDicofre.substr(0,2);
        let concelho_id = dataDicofre.substr(2,2);
        let freguesia_id = dataDicofre.substr(4,3); 
        let valor_distritoid = (distrito_id || '').trim();
        let item = dataDistritos.find(item => item.di === valor_distritoid);
        valor_distritoid=item.id;
        valor_distritoid!=''? myComboBox.setValue(valor_distritoid):myComboBox.setValue('-1');
        if (myComboBox.data.getIndex(valor_distritoid)==-1){
          myComboBox.setValue('-1');
        }
        const valor_concelhoid = (concelho_id || '').trim();
        const valor_freguesiaid = (freguesia_id || '').trim();

        var comboconcelhos= FormDadosConcessionarios.current.getItem('concelho').getWidget();
        if (valor_concelhoid==''){comboconcelhos.clear();}
        var combofreguesias= FormDadosConcessionarios.current.getItem('freguesia').getWidget();
        if (valor_freguesiaid==''){combofreguesias.clear();}
      }else {
         myComboBox.clear();
      }
    }
  }, [dataDistritos]);

  useEffect(() => {
    // Update the form when data.nif changes
    if (FormDadosConcessionarios.current!=null) {   
        
      var myComboBox= FormDadosConcessionarios.current.getItem('concelho').getWidget();
      myComboBox.data.parse(dataConcelhos);  
      if ((dataDicofre!=null)&&(dataDicofre!='')){ 
        console.log(dataDicofre);
        let distrito_id = dataDicofre.substr(0,2);
        let concelho_id = dataDicofre.substr(0,4);
        let freguesia_id = dataDicofre; 

        let valor = (concelho_id || '').trim();
        let item = dataConcelhos.find(item => item.co === valor);
        valor = item.id;
        valor!=''? myComboBox.setValue(valor):myComboBox.setValue('-1');
        if (myComboBox.data.getIndex(valor)==-1){
          myComboBox.setValue('-1');
        }
        const valor_freguesiaid = (freguesia_id || '').trim();
        if (valor_freguesiaid==''){
          var combofreguesias= FormDadosConcessionarios.current.getItem('freguesia').getWidget();
          combofreguesias.data.removeAll(); 
        } 
      }else {
         myComboBox.setValue('-1');
      }
    }
  }, [dataConcelhos]);
  useEffect(() => {
    // Update the form when data.nif changes
    if (FormDadosConcessionarios.current!=null) {   
        
      var myComboBox= FormDadosConcessionarios.current.getItem('freguesia').getWidget();
      myComboBox.data.parse(dataFreguesias);  
      if ((dataDicofre!=null)&&(dataDicofre!='')){ 
        console.log(dataDicofre);
        let distrito_id = dataDicofre.substr(0,2);
        let concelho_id = dataDicofre.substr(0,4);
        let freguesia_id = dataDicofre; 

        let valor = (freguesia_id || '').trim();
        let item = dataFreguesias.find(item => item.fre === valor);
        valor = item.id;
        valor!=''? myComboBox.setValue(valor):myComboBox.setValue('-1');
        if (myComboBox.data.getIndex(valor)==-1){
          myComboBox.setValue('-1');
        }
        setDataDicofre('');
      }else {
         myComboBox.setValue('-1');
      }
    }
  }, [dataFreguesias]);

useEffect(() => {
  if (obj_concessionarios_show !== false) {
       
    windowRef.current = new WindowDHX({
      width: 800,
      height: 750,
      title: 'Concessionários', 
      closable: true,
      movable: true,
      resizable: true,
      modal: false,
      header: true,
      footer: true,
      css: "pcc_window",
    });
    globalWindowReference = windowRef.current;

    layoutRefgeometrias.current  = new LayoutDHX(null,{
      type: "line",
      rows: [ 
        {
          id: "form",
          css: "pcc_layout", 
        },
        {
          id: "grid",
          css: "pcc_layout", 
        }
      ]
    });  
    function closeWindow() {
      if (windowRef.current) {
        windowRef.current.close();
      }    
    }

    FormDadosConcessionarios.current = new FormDHX(null,{ 
      css: "dhx_widget--bg_white dhx_widget--bordered pcc_layout_interv",
      padding: 17,
      rows: [ 
          { type: "spacer", height: 15 }, // Espaço antes das rows
          {cols:[
            { type: "spacer", name: "spacer", width: 30 },
            { css: "pcc_input", id: "nif", type: "input", labelWidth: 75, required: true, validation: "integer", name: "nif", label: "NIF:", placeholder: "NIF", labelPosition: "left", width:200, }, 
            { type: "spacer", name: "spacer" , width: 10 },
            { css: "pcc_input", id: "nome", type: "input",  labelWidth: 50, required: true, name: "nome", label: "Nome:", maxlength: 100, placeholder: "Nome" , labelPosition: "left", width:350,},
            { type: "spacer", name: "spacer" },
          ],},
          {cols:[
            { type: "spacer", name: "spacer", width: 30 },
            { css: "pcc_input", id: "morada", type: "input",  labelWidth: 75, name: "morada", label: "Morada:", maxlength: 100, placeholder: "Morada" , labelPosition: "left", width:500 },
            { type: "spacer", name: "spacer" },
          ],},
          {cols:[
            { type: "spacer", name: "spacer", width: 30 },
            { css: "gismat_input", type: "combo",  labelWidth: 75,  name: "distrito", labelPosition: "left", label: "Distrito:", readOnly: true, width:250, data: dataDistritos }, 
            { type: "spacer", name: "spacer", width: 10 }, 
            { css: "gismat_input", type: "combo", name: "concelho", labelPosition: "left", label: "Concelho:", readOnly: true,  width:350, data: dataConcelhos }, 
          ],},
          {cols:[
            { type: "spacer", name: "spacer", width: 30 },
            { css: "gismat_input", type: "combo", labelWidth: 75, name: "freguesia", labelPosition: "left", label: "Freguesia:", readOnly: true, width:350, data: dataFreguesias },
            { type: "spacer", name: "spacer" },
          ],}, 
          {cols:[
             { type: "spacer", name: "spacer", width: 30 },
             { css: "pcc_input", id: "contacto", labelWidth: 75,  type: "input", validation: "integer", name: "contacto", label: "Contacto:", placeholder: "Contacto" , labelPosition: "left", width:250,},
             { type: "spacer", name: "spacer", width: 10 },
             { type: "input", name: "dicofre", label: "DICOFRE:", labelPosition: "left", readOnly: true , labelPosition: "left",  width:200 },
             { type: "input", name: "rec_id", label: "Recid:", hidden:true, labelPosition: "left", readOnly: true , labelPosition: "left",  width:200 },
          ],}, { type: "spacer", name: "spacer", height: 15 },
          {cols:[
              { type: "spacer", name: "spacer", width: 200 },
              { css: "pcc_input", id: "buttonnew", type: "button",  width: 100, circle: true, value: "Novo", tooltip: "Novo", title: "Novo" },
              { type: "spacer", name: "spacer", width: 75 },
              { css: "pcc_input", id: "buttonsave", type: "button",  width: 100, circle: true, value: "Gravar", tooltip: "Gravar", title: "Gravar" },
              { type: "spacer", name: "spacer", width: 75 },            
              { css: "pcc_input", id: "buttondelete", type: "button",  width: 100, circle: true, value: "Eliminar", tooltip: "Eliminar", title: "Eliminar" },
            
          ],},
        
      
      ] 
    }); 
    function rowDataTemplate(value, row, col) {
        if ((col.id === "locpin") || (col.id === "pdf")){
          return false; // prevent a tooltip from being shown
        } else{
          return value;
        }      
    };
    FormDadosConcessionarios.current.getItem("contacto").events.on("keydown", function(event) {
      // Permite teclas de controle como Backspace, Tab, Delete, setas, etc.
      if (
        event.key === 'Backspace' ||
        event.key === 'Tab' ||
        event.key === 'Delete' ||
        event.key.startsWith('Arrow') ||
        event.metaKey || // Para atalhos de teclado como Command+V no Mac
        event.ctrlKey    // Para atalhos de teclado como Ctrl+V no Windows
      ) {
        return true; // Deixa o evento prosseguir
      }

      // Se o caractere digitado não for um dígito, cancela o evento
      if (!/\d/.test(event.key)) {
        event.preventDefault();
        return false;
      }
      
      // Limita o número de caracteres a 15
      if (event.target.value.length >= 15) {
        event.preventDefault();
      }
    });

    FormDadosConcessionarios.current.getItem("nif").events.on("keydown", function(event) {
      // Permite teclas de controle como Backspace, Tab, Delete, setas, etc.
      if (
        event.key === 'Backspace' ||
        event.key === 'Tab' ||
        event.key === 'Delete' ||
        event.key.startsWith('Arrow') ||
        event.metaKey || // Para atalhos de teclado como Command+V no Mac
        event.ctrlKey    // Para atalhos de teclado como Ctrl+V no Windows
      ) {
        return true; // Deixa o evento prosseguir
      }

      // Se o caractere digitado não for um dígito, cancela o evento
      if (!/\d/.test(event.key)) {
        event.preventDefault();
        return false;
      }
      
      // Limita o número de caracteres a 15
      if (event.target.value.length >= 15) {
        event.preventDefault();
      }
    });
    FormDadosConcessionarios.current.getItem("nome").events.on("keydown", function(event) {
      // Permite teclas de controle como Backspace, Tab, Delete, setas, etc.
      if (
        event.key === 'Backspace' ||
        event.key === 'Tab' ||
        event.key === 'Delete' ||
        event.key.startsWith('Arrow') ||
        event.metaKey || // Para atalhos de teclado como Command+V no Mac
        event.ctrlKey    // Para atalhos de teclado como Ctrl+V no Windows
      ) {
        return true; // Deixa o evento prosseguir
      }

       
      // Valida se está com informação ou não
      if (event.target.value.length >= 0) {
        FormDadosConcessionarios.current.getItem("buttonsave").enable();
      } else {
        FormDadosConcessionarios.current.getItem("buttonsave").disable();
      }
    });
   
    FormDadosConcessionarios.current.getItem("buttonnew").events.on("click", function(events) {
      try{
        FormDadosConcessionarios.current.setValue({ "nif": ''});
        FormDadosConcessionarios.current.setValue({ "nome": ''});
        FormDadosConcessionarios.current.setValue({ "morada":  ''});
        FormDadosConcessionarios.current.setValue({ "dicofre":  ''});
        FormDadosConcessionarios.current.setValue({ "contacto":  ''});
        FormDadosConcessionarios.current.setValue({ "rec_id":  ''});
        var comboconcelhos= FormDadosConcessionarios.current.getItem('concelho').getWidget();
        comboconcelhos.data.removeAll(); 
        comboconcelhos.clear();
        var combofreguesias= FormDadosConcessionarios.current.getItem('freguesia').getWidget();
        combofreguesias.data.removeAll(); 
        combofreguesias.clear();
        setDataDicofre('');
        fetchDataDistritos();
      }catch(e){

      }
    });

    GridDadosConcessionarios.current = new GridDHX(null,{ 
      css: "gismat_grid",
      columns: [
        { width: 0, autoWidth: true, id: "recid", hidden: true, header: [{ text: "#" }], tooltipTemplate: rowDataTemplate},  
        { width: 150, autoWidth: true, align: "left", id: "nif", header: [{ text: "NIF"}], tooltipTemplate: rowDataTemplate },  
        { width: 150, autoWidth: true, align: "left", id: "nome", header: [{ text: "Nome"}], tooltipTemplate: rowDataTemplate },  
        { width: 200, autoWidth: true, align: "left", id: "morada", header: [{ text: "Morada"}] , tooltipTemplate: rowDataTemplate},  
        { width: 100, autoWidth: true, align: "center", id: "dicofre", header: [{ text: "Dicofre"}] , tooltipTemplate: rowDataTemplate},  
        { width: 100, autoWidth: true, align: "left",  id: "contacto", header: [{ text: "Contacto"}] , tooltipTemplate: rowDataTemplate}             
      ],
      htmlEnable: true,
      selection: "row",
      headerRowHeight: 50 
    });
    GridDadosConcessionarios.current.events.on("cellClick", (row, column, event) => {
      try{
        FormDadosConcessionarios.current.setValue({ "nif": row?.nif });
        FormDadosConcessionarios.current.setValue({ "nome": row?.nome });
        FormDadosConcessionarios.current.setValue({ "morada": row?.morada });
        FormDadosConcessionarios.current.setValue({ "dicofre": row?.dicofre }); 
        FormDadosConcessionarios.current.setValue({ "contacto": row?.contacto });
        FormDadosConcessionarios.current.setValue({ "rec_id": row?.recid }); 

        let distrito = row?.dicofre.substr(0,2);
        let concelho = row?.dicofre.substr(2,2);
        let freguesia = row?.dicofre.substr(4,3);
        let dico=row?.dicofre || '' ;
       
        setDataDicofre(dico);
        fetchDataDistritos();
      }catch(e){

      }
      
    });
    FormDadosConcessionarios.current.getItem("buttonsave").events.on("click", function(events) {
       
      handleSave();
    });

    FormDadosConcessionarios.current.getItem("distrito").events.on("change", (id) => {
      const selectedDistrito = FormDadosConcessionarios.current.getItem('distrito').getWidget();
      const iditem = FormDadosConcessionarios.current.getItem("distrito").getValue();
      let di = selectedDistrito.data.getItem(iditem).di;  
      FormDadosConcessionarios.current.setValue({ "dicofre": "" });
      FormDadosConcessionarios.current.setValue({ "dicofre": di });
         
      fetchDataConcelhos(di);
    });
    FormDadosConcessionarios.current.getItem("concelho").events.on("change", (id) => {
      const selectedConcelho = FormDadosConcessionarios.current.getItem('concelho').getWidget();
      const iditem = FormDadosConcessionarios.current.getItem("concelho").getValue();
      let co = selectedConcelho.data.getItem(iditem).co;   
      FormDadosConcessionarios.current.setValue({ "dicofre": "" });
      FormDadosConcessionarios.current.setValue({ "dicofre": co }); 
         
      fetchDataFreguesias(co);
    });
    FormDadosConcessionarios.current.getItem("freguesia").events.on("change", (id) => {
    
      const selectedFreguesia = FormDadosConcessionarios.current.getItem('freguesia').getWidget();
      const iditem = FormDadosConcessionarios.current.getItem("freguesia").getValue();
      let dicofre = selectedFreguesia.data.getItem(iditem).fre;         
    
      FormDadosConcessionarios.current.setValue({ "dicofre": "" });
      FormDadosConcessionarios.current.setValue({ "dicofre": dicofre });

    });
     
    function handleSave() {
      if (FormDadosConcessionarios.current) {
        let objform_nif = FormDadosConcessionarios.current.getItem("nif").getValue()|| '';
        let objform_nome = FormDadosConcessionarios.current.getItem("nome").getValue()|| '';
        let objform_morada = FormDadosConcessionarios.current.getItem("morada").getValue()|| '';
        let objform_dicofre = FormDadosConcessionarios.current.getItem("dicofre").getValue()|| '';
        let objform_contacto = FormDadosConcessionarios.current.getItem("contacto").getValue()|| '';
        let objform_recid = FormDadosConcessionarios.current.getItem("rec_id").getValue()|| '0';
        console.log(objform_nome);
        if(objform_nome.trim() == ""){
          dhx.alert({
            header:"Erro ao gravar",
            text:"É necessário que o campo (Nome*) esteja preenchido",
            buttonsAlignment:"center",
            buttons:["ok"],
          });
        }else{         
          onsaveWindow(objform_recid, objform_nif, objform_nome, objform_morada, objform_dicofre, objform_contacto);  
        } 

      }
    
    }
 
    layoutRefgeometrias.current.getCell("grid").attach(GridDadosConcessionarios.current); 
    layoutRefgeometrias.current.getCell("form").attach(FormDadosConcessionarios.current);
 
    windowRef.current?.attach(layoutRefgeometrias.current);

    windowRef.current?.events.on("move", function(position, oldPosition, side) {
      console.log("The window is moved to " + position.left, position.top)
    });
    
    windowRef.current?.events.on("afterHide", function(position, events){
        console.log("A window is hidden", events);
        handleClose();
        
    });
      
    let objgeomid;
 

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
}, [obj_concessionarios_show]);

  const ondeleteWindow = (objgeomid) => {
    const DeleteObjGeometry = (objgeomid)=> {
      try {				
        const savePretensaoObj = async () => {                   
          const url = apiEndpoint + 'TipoIntervencaoSolo';//ok   
          const jwtToken = authtoken;         
          const response = await fetch(url, {
            method: 'DELETE',
            body: JSON.stringify({ 
                id : objgeomid
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
              // mapa refresh
              // tree refresh      
              FormDadosConcessionarios.current.setValue({ "nome": "" });
              refreshDataTipoIntervencao();               
            
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
    
    // vamos criar uma janela de confirmação do delete

    dhx.confirm({
      header:"Remover tipo intervenção solo",
      text:"Tem a certeza que pretende remover o tipo de intervenção solo?",
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

    const refreshDataTipoIntervencao = async () => {//ok
      const url3 = apiEndpoint + 'TipoIntervencaoSolo';//ok 
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
        FormDadosConcessionarios.current.getItem("buttonsave").disable();
       
        return response.json();
      })
      .then(jsonData => {
        setDataTipoIntervencao(jsonData); 
      })
      .catch(error => {
        console.error('There was a problem with your fetch operation:', error);
      });  
    }

    
  }

  const onsaveWindow = (objform_recid, objform_nif, objform_nome, objform_morada, objform_dicofre, objform_contacto) => {   
    const contSave = (objform_recid, objform_nif, objform_nome, objform_morada, objform_dicofre, objform_contacto)=> {   
        const saveConcessionario = async () => { 
          const jwtToken = authtoken;           
          const url = apiEndpointCadastro + 'Concessionarios/';//ok 
          let metodo='POST';
          if (objform_recid!='0'){
            metodo='PUT';
          }
          
          const response = await fetch(url, {
            method: metodo,
            body: JSON.stringify({ 
                recId: objform_recid,
                nif: objform_nif,
                nome: objform_nome,
                morada: objform_morada, 
                dicofre: objform_dicofre,
                contacto: objform_contacto
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
                  
              //refreshDataGeometrias();   
               
            
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        saveConcessionario();       
    } 
    
    contSave(objform_recid, objform_nif, objform_nome, objform_morada, objform_dicofre, objform_contacto);
    
  }

  const onupdateWindow = (objformid,objformnome) => {   
    const contSave = (objformid,objformnome)=> {   
        const savePretensaoObj = async () => { 
                            
          const url = apiEndpoint + 'TipoIntervencaoSolo/';//ok 
          const jwtToken = authtoken;        
          const response = await fetch(url, {
            method: 'PUT',
            body: JSON.stringify({
                id: objformid,
                nome: objformnome
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
              // mapa refresh
              // tree refresh 
              FormDadosConcessionarios.current.setValue({ "nome": "" });     
              refreshDataTipoIntervencao();               
            
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        savePretensaoObj();       
    } 
    
    contSave(objformid,objformnome);
      
    
    const refreshDataTipoIntervencao = async () => {//ok
      const url3 = apiEndpoint + 'TipoIntervencaoSolo';//ok 
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
        FormDadosConcessionarios.current.getItem("buttonsave").disable();
       
        return response.json();
      })
      .then(jsonData => {
        setDataTipoIntervencao(jsonData); 
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

Wpcc_GestaoConcessionarios.propTypes = {
  obj_concessionarios_show: PropTypes.boolean,
};
export default Wpcc_GestaoConcessionarios;
