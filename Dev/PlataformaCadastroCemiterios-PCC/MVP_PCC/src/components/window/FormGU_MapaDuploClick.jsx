 
 
 
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

const FormGU_MapaDuploClick= ({mapa_zoomcoordenadas, obj_info_WKTs}) => {
  
  const windowObjectosMapa = useRef(null);   
  const gridObjectosMapa = useRef(null);  


  const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);

  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid);
  const usersession = useSelector((state)=> state.aplicacaopcc.geral.usersession); 
 
  const obj_infoobj_info = useSelector((state)=> state.aplicacaopcc.windows.mapa_duploclick.obj_info);
  const obj_info_tipo = useSelector((state)=> state.aplicacaopcc.windows.mapa_duploclick.obj_info_tipo);
  const obj_info_ids = useSelector((state)=> state.aplicacaopcc.windows.mapa_duploclick.obj_info_ids);
  //const obj_info_WKTs = useSelector((state)=> state.aplicacaopcc.windows.mapa_duploclick.obj_info_WKTs);
  const grupos_construcoes_ligados = useSelector((state)=> state.aplicacaopcc.treeconstrucoes.treeconstrucoes_itenschecked);
  const grupos_atendimentos_ligados = useSelector((state)=> state.aplicacaopcc.treeatendimento.treeatendimento_itenschecked);

  const handleClose = () => { 
    windowObjectosMapa.current?.destructor();  
    windowObjectosMapa.current = null;
    var aux={ type: Actions.HIDE_MAPA_DUPLOCLICK, payload: false };
    viewer.dispatch(aux); 
    
  };
  

  useEffect(() => {
     
    if (mapa_zoomcoordenadas  == true  ) {
      
      const localizaPretensao = (centroid, bboxWKT, aux_pretensaoid, aux_pretensaoobjid) => {
    
        if(centroid!=null){
           const coordsc = centroid.substring(7, centroid.length - 1).split(' ');
           const centro_x = parseFloat(coordsc[0]);
           const centro_y = parseFloat(coordsc[1]);
      
         // Extract the coordinates from the WKT string
           const coords = bboxWKT.substring(10, bboxWKT.length - 2).split(',');
     
           // Parse the coordinates as numbers
           const points = coords.map((coord) => {
            coord=coord.trim();
             const [px, py] = coord.split(' ');
             return [parseFloat(px), parseFloat(py)];
           });
     
           // Calculate the width and height of the bounding box
           const [minx, miny] = points[0];
           let maxx = minx;
           let maxy = miny;
           points.forEach(([px, py]) => {
             if (px < minx) minx = px;
             if (px > maxx) maxx = px;
             if (py < miny) miny = py;
             if (py > maxy) maxy = py;
           });
           const width = Math.abs(maxx - minx);
           const height = Math.abs(maxy - miny);
           let estado = viewer.getState();
           let activeMapName = estado.config.activeMapName;
    
           var view = estado.mapState[activeMapName].currentView; 
         
           let escala_mapa=parseInt(view.scale); 
           let centro_x_mapa=view.x; 
           let centro_y_mapa=view.y; 

           let resolution = Math.max(width / 800, height / 600);
           let escala = parseInt( resolution / 0.00028) + 50;
           if (escala_mapa!=escala){
            if (centro_x!=centro_x_mapa && centro_y!= centro_y_mapa){
              if (centro_x!=null && centro_y!= null && escala !=null){ 
                var state = viewer.getState(); 
                var NomeMapa = state.config.activeMapName; 
                var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }}; 
                //viewer.dispatch(aux);
              } 
            }
           }
           
          
           let mapState = estado.mapState[activeMapName];
           let currentMap = mapState.mapguide.runtimeMap; 
           let sessionId  = currentMap.SessionId; 
           let mapaDef  = currentMap.MapDefinition;
             
           let ObjectoGeografico_id= aux_pretensaoobjid;
           setConstrucoesSelection(apiEndpoint, authtoken, activeMapName, mapaDef, sessionId, aux_pretensaoid, centro_x, centro_y, escala); 
         }
      } 
       
      const localizaAtendimento = (centroid, bboxWKT, aux_atendimentoid, aux_atendimentoobjid) => {
    
        if(centroid!=null){
          const coordsc = centroid.substring(7, centroid.length - 1).split(' ');
          const centro_x = parseFloat(coordsc[0]);
          const centro_y = parseFloat(coordsc[1]);
      
        // Extract the coordinates from the WKT string
          const coords = bboxWKT.substring(10, bboxWKT.length - 2).split(',');
    
          // Parse the coordinates as numbers
          const points = coords.map((coord) => {
            coord=coord.trim();
            const [px, py] = coord.split(' ');
            return [parseFloat(px), parseFloat(py)];
          });
    
          // Calculate the width and height of the bounding box
          const [minx, miny] = points[0];
          let maxx = minx;
          let maxy = miny;
          points.forEach(([px, py]) => {
            if (px < minx) minx = px;
            if (px > maxx) maxx = px;
            if (py < miny) miny = py;
            if (py > maxy) maxy = py;
          });
          const width = Math.abs(maxx - minx);
          const height = Math.abs(maxy - miny);
          let estado = viewer.getState();
          let activeMapName = estado.config.activeMapName;
    
          //var view = estado.mapState[activeMapName].currentView; 
          //var escala = view.scale; 
          let resolution = Math.max(width / 800, height / 600);
          let escala = parseInt( resolution / 0.00028) + 50;
          if (centro_x!=null && centro_y!= null && escala !=null){ 
            var state = viewer.getState(); 
            var NomeMapa = state.config.activeMapName; 
            var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }}; 
            viewer.dispatch(aux);
          } 
        
          let mapState = estado.mapState[activeMapName];
          let currentMap = mapState.mapguide.runtimeMap; 
          let sessionId  = currentMap.SessionId; 
          let mapaDef  = currentMap.MapDefinition;
            
          let ObjectoGeografico_id= aux_atendimentoobjid;
          const setAtendimentosSelection = async () => {					 
      
            const jwtToken = authtoken;
            const url = apiEndpoint + 'MapaAtendimentosSetSelection';//ok
    
            const response = await fetch(url, {
              method: 'POST',
              body: JSON.stringify({
                  mapa: activeMapName,
                  mapadef: mapaDef,
                  sessionid: sessionId,             
                  viewer: 'false',
                  atendimentoid: aux_atendimentoid,
                  atendimentoobjid: ObjectoGeografico_id,
                  cx: centro_x.toString(),
                  cy: centro_y.toString(),
                  escala: escala.toString(),
                }),
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${jwtToken}`
                },
            });
        
            if (response.ok) { 
              const xml = await response.text();  
              if (viewer_interface ==null){
                viewer_interface = GetViewerInterface(); 
              } 
              viewer_interface.setSelectionXml(xml);  
            } else {
              const errorMessage = await response.text();
              console.error('error: ' + errorMessage);
              this.setState({ errorMessage: errorMessage });
            } 
          };  
          setAtendimentosSelection(); 
        }
      } 
      console.log('mapa_duploclick obj_infoobj_info: ' + obj_infoobj_info); 
      console.log('mapa_duploclick obj_info_tipo: ' + obj_info_tipo);
      console.log('mapa_duploclick obj_info_ids: ' + obj_info_ids);
      console.log('mapa_duploclick grupos_construcoes_ligados: ' + grupos_construcoes_ligados.join('|'));  
      console.log('mapa_duploclick grupos_atendimentos_ligados: ' + grupos_atendimentos_ligados.join('|'));

      try {
					
        const getObjetosMapaDuploClick = async () => {			
          const onzoomToWindow = (centroid, bboxWKT) => {
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
             
            } 
            
          }
          function showDadosObjectos(dadosent){
            console.log(windowObjectosMapa.current);
            if (windowObjectosMapa.current==null){
              // vamos criar uma janela de confirmação do delete
              windowObjectosMapa.current = new WindowDHX({
                width: 800,
                height: 300,
                title: 'Lista de Objectos', 
                closable: true,
                movable: true,
                resizable: false,
                modal: false,
                header: true,
                footer: true,
                css: "pcc_window",
              }); 
                
              windowObjectosMapa.current?.events.on("afterHide", function(position, events){

                gridObjectosMapa.current && gridObjectosMapa.current.destructor();
                windowObjectosMapa.current && windowObjectosMapa.current.destructor();
                gridObjectosMapa.current = null;
                windowObjectosMapa.current = null;
                console.log("A window is hidden", events);
                handleClose();          
              });
              function rowDataTemplate(value, row, col) {
                let identificador_coluna=col.id;
                switch (identificador_coluna){
                  case 'locpin':
                    return 'Localizar';
                  case 'open':
                    return 'Abrir';
                  case 'loctree':
                    return 'Localizar na árvore';
                  default:
                    return value;
                }
               /* if ((col.id === "locpin")){
                  return 'Localizar'; // prevent a tooltip from being shown
                } else{
                  if ((col.id === "open")){
                    return 'Abrir'; 
                  } else{
                      if ((col.id === "locpin") || (col.id === "open")|| (col.id === "loctree")){
                        return 'Localizar na árvore'; // prevent a tooltip from being shown
                      } else {
                        return value;
                      } 
                    }  
                  }


                if ((col.id === "locpin") || (col.id === "open")|| (col.id === "loctree")){
                  return false; // prevent a tooltip from being shown
                } else{
                  return value;
                }*/
              
              };
              gridObjectosMapa.current = new GridDHX(null,{ 
                css: "pcc_grid",
                columns: [
                  { width: 0, autoWidth: true, id: "rec_id", hidden: true, header: [{ text: "#" }] , tooltipTemplate: rowDataTemplate},
                  { width: 0, autoWidth: true, id: "parent_id", hidden: true, header: [{ text: "#" }] , tooltipTemplate: rowDataTemplate},
                  { width: 0, autoWidth: false, id: "centroid", hidden: true, header: [{ text: "centroid" }] , tooltipTemplate: rowDataTemplate} ,
                  { width: 0, autoWidth: false, id: "bbox", hidden: true, header: [{ text: "bbox" }] , tooltipTemplate: rowDataTemplate} ,
                  { width: 50, autoWidth: false, id: "locpin", header: [{ text: "" }]  , tooltipTemplate: rowDataTemplate},  
                  { width: 50, autoWidth: false, id: "open", header: [{ text: "" }]  , tooltipTemplate: rowDataTemplate}, 
                  { width: 50, autoWidth: false, id: "loctree", header: [{ text: "" }]  , tooltipTemplate: rowDataTemplate},                     
                  { width: 50, autoWidth: false, id: "tipo", header: [{ text: "Tipo" }] , tooltipTemplate: rowDataTemplate },   
                  { width: 575, autoWidth: true,id: "descricao", header: [{ text: "Descrição"}] , tooltipTemplate: rowDataTemplate},
                ],
                htmlEnable: true,
                selection: "row",
                headerRowHeight: 50 
              });
              gridObjectosMapa.current?.events.on("cellDblClick",  async (row, column, e) => {
                // abrir a Pretensao -  dispatch pretensaoid 
                // fechar a Janela de Pesquisa
              
                if (column.id=="open"){ 
                  var aux_tipo=row.tipo;
                  if (aux_tipo=='PCC'){
                    var pretensaoid=row.rec_id.split('|')[0];
                    var aux2={ type: Actions.HIDE_MAPA_DUPLOCLICK, payload: pretensaoid }; 
                    viewer.dispatch(aux2); 
                    var aux3={ type: Actions.CONSTRUCAO_EDITA, payload: pretensaoid }; 
                    viewer.dispatch(aux3);
                  }   
                 
                }
              });
              gridObjectosMapa.current?.events.on("cellClick", function(row,column,e){
                // your logic here
                if (column.id=="locpin"){ 
                  var aux_tipo=row.tipo;
                  if (aux_tipo=='PCC'){
                    var aux_pretensaoid=row.rec_id.split('|')[0];
                    var aux_pretensaoobjid=row.rec_id.split('|')[1]; 
                    var centroid=row.centroid;
                    var mbr=row.bbox;  
                    if (centroid!=""){
                      onzoomToWindow(centroid, mbr);
                      localizaPretensao(centroid, mbr, aux_pretensaoid, aux_pretensaoobjid) 
                    }  
                  }        
                }
                if (column.id=="loctree"){ 
                  var aux_tipo=row.tipo;
                  if (aux_tipo=='PCC'){
                    var aux_pretensaoid=row.rec_id.split('|')[0];
                    var aux_pretensaoobjid=row.rec_id.split('|')[1];
                    var aux_pretensaoparent_id=row.parent_id;
                    var aux_payload = {
                      sigla_separador: "CEM",
                      parent_id: aux_pretensaoparent_id,
                      rec_id: aux_pretensaoid 
                    }
                    var aux={ type: Actions.SIDEBARLOCALIZA_START, payload: aux_payload };
                    viewer.dispatch(aux); 
                  }                   
                }
                if (column.id=="open"){ 
                  var aux_tipo=row.tipo;
                  if (aux_tipo=='PCC'){
                    var construcaoid=row.rec_id.split('|')[0];
                    var aux2={ type: Actions.HIDE_MAPA_DUPLOCLICK, payload: construcaoid }; 
                    viewer.dispatch(aux2); 
                    var aux3={ type: Actions.CONSTRUCAO_EDITA, payload: construcaoid }; 
                    viewer.dispatch(aux3);
                  }  
                 
                }


              });
              gridObjectosMapa.current?.events.on("cellRightClick", (row, column, event) => {
                event.preventDefault();
              })
              windowObjectosMapa.current?.attach(gridObjectosMapa.current);
              windowObjectosMapa.current?.show(); 
            }
            gridObjectosMapa.current.data.parse(dadosent);
            
    
          }
          var estado = viewer.getState(); 
          var activeMapName = estado.config.activeMapName;  
          //Get active runtime map
          var mapState = estado.mapState[activeMapName];
          var currentMap = mapState.mapguide.runtimeMap; 
          var sessionId  = currentMap.SessionId;
    
          var mapaDef  = currentMap.MapDefinition;
          const jwtToken = authtoken;
          const url = apiEndpoint + 'MapaObjetosDuploClick';//ok

          const response = await fetch(url, {
            method: 'POST',
            body: JSON.stringify({
                mapa: activeMapName,
                mapadef: mapaDef,
                sessionid: sessionId,
                userid: userid,
                usersession: usersession,
                viewer: 'false',
                mapa_objetos: obj_info_ids,
                mapa_tipoobjetos: obj_info_tipo,
                mapa_wktobjetos: obj_info_WKTs,
                grupos_construcoes: grupos_construcoes_ligados.join('|'),
                grupos_atendimento: grupos_atendimentos_ligados.join('|'),
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });
      
          if (response.ok) {
            const jsonData = await response.text();
  
            if (jsonData){


              Promise.resolve(showDadosObjectos(jsonData))
              .then((json) => {
                
              })
              .catch((error) => {
                 
                console.error("Error processing data:", error);
              }); 
               
            } 
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
            //this.setState({ errorMessage: errorMessage });
          }
        };
        getObjetosMapaDuploClick();
      } catch (error) {
        console.error('error: ' + error);
        this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Pretensões.' });
      }
 
      if (windowObjectosMapa.current!=undefined){
        windowObjectosMapa.current.show();
      
      }else {        
        windowObjectosMapa.current = null; 
      } 
    } else {
      // obj_mapazoomcoordenadas está vazio logo destroy the window e tudo o resto  
      windowObjectosMapa.current = null;   
    }

    return () => { 
      windowObjectosMapa.current?.destructor();  
      windowObjectosMapa.current = null;
    };
  }, [mapa_zoomcoordenadas, obj_info_WKTs ]);

 
  return (
    <div ref={windowObjectosMapa}></div>
    
  );
  
}
 
FormGU_MapaDuploClick.propTypes = {
  mapa_zoomcoordenadas: PropTypes.boolean, 
};
export default FormGU_MapaDuploClick;
