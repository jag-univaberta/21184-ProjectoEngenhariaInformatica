import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';

const FormGU_MapaZoomCoordenadas= ({mapa_zoomcoordenadas}) => {
 
  const windowRef = useRef(null);  
 
  const FormRefgeral = useRef(null);
  const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
   
  const handleClose = () => { 
    
    var aux={ type: Actions.HIDE_MAPA_ZOOMCOORDENADAS, payload: false };
    viewer.dispatch(aux); 
    
  };
  

  useEffect(() => { 
    if (mapa_zoomcoordenadas  == true  ) {
      
      const onZoomCoordenadas = () => {   
        const onzoomToWindow = async  (longitude, latitude) => {
          //var centroid = data?.centroid;
          var viewerstate = viewer.getState();
          //Get active map name
          var activeMapName = viewerstate.config.activeMapName;

          var mapState = viewerstate.mapState[activeMapName];
          var currentMap = mapState.mapguide.runtimeMap;
          
          var coordsys = currentMap.CoordinateSystem;
          var coordsys_destino = coordsys.EpsgCode;
          var map_extents = currentMap.Extents; 
          var Lower = map_extents.LowerLeftCoordinate;
          var Upper = map_extents.UpperRightCoordinate;

          var combosys = FormRefgeral.current.getItem('sistemacoordenadas').getWidget();
          var coordsys_origem = combosys.getValue();

          if ((coordsys_origem != null)&&(coordsys_destino != null)){
            if (parseInt(coordsys_origem) == parseInt(coordsys_destino)){
        
              const centro_x = parseFloat(longitude);
              const centro_y = parseFloat(latitude);
              const lower_x = parseFloat(Lower.X);
              const lower_y = parseFloat(Lower.Y);
              const upper_x = parseFloat(Upper.X);
              const upper_y = parseFloat(Upper.Y);
              const width = 1000;
              const height = 1000;
      
              // Calculate the initial map scale
              const resolution = Math.max(width / 800, height / 600);
              const escala = parseInt( resolution / 0.00028) + 50;
              if ((centro_x!=null) && (centro_y!= null) && (escala !=null)){
                if ((centro_x > lower_x)&&(centro_x < upper_x)&&(centro_y > lower_y)&&(centro_y < upper_y)){
                  var state = viewer.getState();
                  const args = state.config;
                  var NomeMapa = state.config.activeMapName;
                  var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }};
                  //zoomToView(aux);
                  viewer.dispatch(aux);
                }
              } 
            } else { 
              const url = apiEndpoint + 'mapacoordenadas';
        
              const response = await fetch(url, {
                method: 'POST',
                body: JSON.stringify({
                  longitude_X: longitude,
                  latitude_Y: latitude,
                  epsG_Origem: coordsys_origem,
                  epsG_Destino: coordsys_destino, 
                  }),
                headers: {
                    'Content-Type': 'application/json',
                  },
              });
           
              const json = await response.json();
              console.log('resposta : ' + JSON.stringify(json));
              // Accessing values from the response
              const centro_x = parseFloat(json.longitude_X); 
              const centro_y  = parseFloat(json.latitude_Y); 
              var state = viewer.getState();
              const args = state.config;
              var NomeMapa = state.config.activeMapName;
              const width = 1000;
              const height = 1000;
      
              // Calculate the initial map scale
              const resolution = Math.max(width / 800, height / 600);
              const escala = parseInt( resolution / 0.00028) + 50;
              var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }};
              //zoomToView(aux);
              viewer.dispatch(aux); 

              
            }
          } else {
            FormRefgeral.current.getItem("textmessage").show(); 
          } 
        }
        const objform_latitude = FormRefgeral.current.getItem("latitude").getValue();
        const objform_longitude= FormRefgeral.current.getItem("longitude").getValue(); 
        onzoomToWindow(objform_longitude, objform_latitude);       
   
      }
       
      windowRef.current = new WindowDHX({
        width: 500,
        height: 475,
        title: 'Localizar coordenadas', 
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
        padding: 17,
        rows: [
           
              { css: "pcc_input",type: "combo", id:"sistemacoordenadas", name: "sistemacoordenadas", label: "Sistema de Coordenadas", labelPosition: "top" ,  readOnly:true,
                data: [
                  { id: "3763", value:"PT-TM0/ETRS89 (EPSG:3763)"},
                  { id: "3857", value: "Pseudo-Mercator/WGS84 (EPSG:3857)"}, 
                  { id: "4326", value: "WGS84 (EPSG:4326)"},
                  { id: "27493", value: "Datum 73/Hayford-Gauss (EPSG:27493)"}] 
              },
              {
                label: "Unidades:",id: "tipounidades",type: "fieldset",css: "dhx_layout-cell--bordered dhx_layout-cell--no-border",hidden: true,
                rows: [
                    {
                        type: "radioGroup",
                        name: "tipounidadesrd",
                        options: {
                            align: "center",
                            cols: [
                                {type: "radioButton",text: "Graus decimais",value: "decimais",checked: true,autoWidth: true,},
                                {type: "radioButton",text: "Graus/Minutos/Segundos",value: "graus",autoWidth: true,},
                            ],
                        },
                    },
                ],
              },
              {
                label: "Coordenadas:",id: "coor",type: "fieldset",css: "dhx_layout-cell--bordered dhx_layout-cell--no-border", hidden: false,
                rows: [
                 { css: "pcc_input",type: "input", id: "longitude", name: "longitude", label: "Longitude", placeholder: "Longitude", required: true  },   
                 { css: "pcc_input",type: "input", id: "latitude", name: "latitude", label: "Latitude", placeholder: "Latitude", required: true  },  
              ]},
               
              { css: "pcc_input",id: "textmessage",hidden: true,hiddenLabel: true,name: "textmessage",type: "text",label: "Text",value: "Tem que inserir as coordenadas pretendidas!",
              },
              { css: "pcc_input", id: "buttonzoomcoordenadas", type: "button",  circle: true, value: "Localizar", tooltip: "Localizar", title: "Localizar" },
                
          ]  
        
      }); 
      FormRefgeral.current.getItem('tipounidadesrd').events.on("change", function(ids) {
        const value = FormRefgeral.current.getItem("tipounidadesrd").getValue();
        switch(value){
          case 'decimais':  
            FormRefgeral.current.getItem("longitude").setProperties({ css: "pcc_input",type: "input", id: "longitude", name: "longitude", label: "X", placeholder: "X", required: true });
            FormRefgeral.current.getItem("latitude").setProperties({ css: "pcc_input",type: "input", id: "latitude", name: "latitude", label: "Y", placeholder: "Y", required: true });
            break;
        }
      });
      FormRefgeral.current.getItem('sistemacoordenadas').events.on("change", function(ids) {
       
        //FormRefgeral.current.setValue({ "tipointervencao_id": ids });
        switch(ids){
          case '3763': 
            FormRefgeral.current.getItem("tipounidades").hide();
            FormRefgeral.current.getItem("longitude").setProperties({ css: "pcc_input",type: "input", id: "longitude", name: "longitude", label: "X", placeholder: "X", required: true });
            FormRefgeral.current.getItem("latitude").setProperties({ css: "pcc_input",type: "input", id: "latitude", name: "latitude", label: "Y", placeholder: "Y", required: true });
            break;
          case '3857': 
            FormRefgeral.current.getItem("tipounidades").hide();
            FormRefgeral.current.getItem("longitude").setProperties({ css: "pcc_input",type: "input", id: "longitude", name: "longitude", label: "X", placeholder: "X", required: true  });
            FormRefgeral.current.getItem("latitude").setProperties({ css: "pcc_input",type: "input", id: "latitude", name: "latitude", label: "Y", placeholder: "Y", required: true  });
            break;
          case '4326': //WGS84 (EPSG:4326) 
            FormRefgeral.current.getItem("tipounidades").hide();
            FormRefgeral.current.getItem("longitude").setProperties({ css: "pcc_input",type: "input", id: "longitude", name: "longitude", label: "Longitude", placeholder: "Longitude graus decimais", required: true   });
            FormRefgeral.current.getItem("latitude").setProperties({ css: "pcc_input",type: "input", id: "latitude", name: "latitude", label: "Latitude", placeholder: "Latitude graus decimais", required: true  });
            break;
          case '27493':
            FormRefgeral.current.getItem("tipounidades").hide(); 
            FormRefgeral.current.getItem("longitude").setProperties({ css: "pcc_input",type: "input", id: "longitude", name: "longitude", label: "X", placeholder: "X", required: true });
            FormRefgeral.current.getItem("latitude").setProperties({ css: "pcc_input",type: "input", id: "latitude", name: "latitude", label: "Y", placeholder: "Y", required: true  });
            break;
          default:  break;

        }
      });
      FormRefgeral.current.getItem("buttonzoomcoordenadas").events.on("click", function(events) {
        console.log("click", events);
        handleZoomCoordenadas();
      });
      FormRefgeral.current.getItem("latitude").events.on("keydown", function(event) {
        var a=FormRefgeral.current.getItem("textmessage").isVisible(); 
        if (a){
          FormRefgeral.current.getItem("textmessage").hide();
        } 
      });   
      FormRefgeral.current.getItem("longitude").events.on("keydown", function(event) {
        var a=FormRefgeral.current.getItem("textmessage").isVisible(); 
        if (a){
          FormRefgeral.current.getItem("textmessage").hide();
        } 
      });   
      function handleZoomCoordenadas() {
        
          const objformnr_latitude = FormRefgeral.current.getItem("latitude").getValue();
          const objform_longitude= FormRefgeral.current.getItem("longitude").getValue();
           
       
          if  ((objformnr_latitude == "")&&(objform_longitude == "")){
            dhx.alert({
              header:"Erro Campos de Coordenadas vazios",
              text:"É necessário colocar as Coordenadas e definir o Sistema de Coordenadas",
              buttonsAlignment:"center",
              buttons:["ok"],
            });
          }else{
            onZoomCoordenadas(); 
        }
      }
 
 
      windowRef.current?.events.on("move", function(position, oldPosition, side) {
        console.log("The window is moved to " + position.left, position.top)
      });
      
      windowRef.current?.events.on("afterHide", function(position, events){
          console.log("A window is hidden", events);
          handleClose();          
      });
      windowRef.current?.attach(FormRefgeral.current);
      if (windowRef.current!=undefined){
         
        var viewerstate = viewer.getState(); 
        var activeMapName = viewerstate.config.activeMapName; 
        var mapState = viewerstate.mapState[activeMapName];
        var currentMap = mapState.mapguide.runtimeMap; 
        var coordsys = currentMap.CoordinateSystem; 
        var myComboBox= FormRefgeral.current.getItem('sistemacoordenadas').getWidget(); 
        var coordsys_destino = coordsys.EpsgCode;
        coordsys_destino!=''? myComboBox.setValue(coordsys_destino):myComboBox.setValue('-1');
        if (myComboBox.data.getIndex(coordsys_destino)==-1){
          myComboBox.setValue('-1');
        }


        windowRef.current.show();
      }else {        
        windowRef.current = null; 
      } 
    } else {
      // obj_mapazoomcoordenadas está vazio logo destroy the window e tudo o resto  
      windowRef.current = null;   
    }

    return () => { 
      windowRef.current?.destructor();  
      windowRef.current = null;
    };
  }, [mapa_zoomcoordenadas ]);

 
  return (
    <div ref={windowRef}></div>
    
  );
  
}
 
FormGU_MapaZoomCoordenadas.propTypes = {
  mapa_zoomcoordenadas: PropTypes.boolean, 
};
export default FormGU_MapaZoomCoordenadas;
