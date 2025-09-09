import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
import ReactDOMServer from 'react-dom/server';
import { Checkbox, Icon } from '@fluentui/react';
import { v4 as uuidv4 } from 'uuid';
import { setConstrucoesSelection } from '../../utils/utils';

const Wpcc_Form_ConstrucaoImportFicheiro= ({obj_importaficheiroconstrucao_show, obj_importaficheiroconstrucao_id}) => {
  const inputRef = useRef();
  const [files, setFiles] = useState(null);
  const windowRef = useRef(null);
  const windowRefFicheiro = useRef(null);    
  const menuRef = useRef(null);
  const tabRef = useRef(null);
  const apiEndpointCadastro = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointCadastro);
  const apiEndpointSIG = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointSIG);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid);
  const dispatch = useReduxDispatch();

  useEffect(() => {
    if (obj_importaficheiroconstrucao_show !== false) {
      
    } else {
      //windowRef.current.hide();
    }

  }, [obj_importaficheiroconstrucao_show]);

  const handleDragOver = (event) => {
    event.preventDefault();
  };

  const handleDrop = (event) => {
    event.preventDefault();
    setFiles(event.dataTransfer.files)
    //handleUpload()
  };
  useEffect(() => {
    console.log('upload de ficheiro -------');
    if (files!=undefined){
      for (let i=0; i < files.length; i++){
        console.log(files[i]);
      }
    }
     
    console.log('upload de ficheiro fim -------');

  }, [files]);

  // send files to the server // learn from my other video
     
    const handleUpload = async () => {    
      const jwtToken = authtoken;      
      var estado = viewer.getState();			
      var activeMapName = estado.config.activeMapName;
      var mapState = estado.mapState[activeMapName];
      var currentMap = mapState.mapguide.runtimeMap; 
      var sessionId  = currentMap.SessionId; 
      var mapaDef  = currentMap.MapDefinition;
      const file = files[0];
      const formData = new FormData();
      if (files && files.length > 0) {
        for (let i=0; i < files.length; i++){
          
          formData.append("File", files[i]); 
        }
      }
      console.log(obj_importaficheiroconstrucao_id);
      formData.append("Userid", userid);  
      formData.append("construcaoid", obj_importaficheiroconstrucao_id);   
      try {
        const url = apiEndpointCadastro + 'ImportarFicheiroConstrucao/';//ok   
        const response = await fetch(url, {
          method: "POST",
          body:  formData,
						  headers: {
							  'Authorization': `Bearer ${jwtToken}`,
						  },
          });   
        if (response.ok) {
          const data = await response.text(); // Lê a resposta como texto
          const [centroid, bboxWKT] = data.split('|'); // Divide a string em duas partes        
          // Agora você tem as duas strings separadas:
          console.log("WKT_centroide:", centroid);
          console.log("WKT_BBox:", bboxWKT);
          dhx.alert({
            header:"Carregamento efectuado",
            text:"O item foi enviado com sucesso!",
            buttonsAlignment:"center",
            buttons:["ok"],
            });
            const hideWindow= () => dispatch({
              type: Actions.HIDE_AND_REFRESH_IMPORTAFILE_CONSTRUCAO,
              payload: ""
            });
            hideWindow();
            // mapa refresh
						          
						var state = viewer.getState();
						const args = state.config;
						var NomeMapa = state.config.activeMapName;
						const uid = uuidv4();
						var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};                     
						viewer.dispatch(aux);  
            localizaconstrucao (centroid, bboxWKT, obj_importaficheiroconstrucao_id); 
        } else {
          const errorMessage = await response.text();
          console.error('error: ' + errorMessage);
        }
        //console.log(data);
      } catch (error) {
        console.error(error);
      }
    };

   const localizaconstrucao = (centroid, bboxWKT, aux_construcaoid) => {
    
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
         let [minx, miny] = points[0];
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
           
         let ObjectoGeografico_id= '';
         setConstrucoesSelection(apiEndpointSIG, authtoken, activeMapName, mapaDef, sessionId, aux_construcaoid, centro_x, centro_y, escala); 
       }
   } 

    if (files) return (
      <div className="uploadsdesenho">
          <div className="ficheirosdesenho">
            <ul>
                {Array.from(files).map((file, idx) =>{
                  console.log(file);
                  return <li key={idx}>{file.name} </li>
                })}
            </ul> 
          </div>
          <div className="actionsdesenho">
              <button className="buttoncancelfiledesenho" onClick={() => setFiles(null)}>Cancelar</button>
              <button className="buttonuploadfiledesenho" onClick={handleUpload}>Carregar</button>
          </div>
      </div>
      
    )
  return (
      <div ref={windowRef} className="dropzone" onDragOver={handleDragOver} onDrop={handleDrop}>
            <h2>Arraste o ficheiro para carregar</h2>
            <h2>ou</h2>
            <input 
              type="file"
              onChange={(event) => setFiles(event.target.files)}
              hidden
              accept=".zip"//,.dwg"
              ref={inputRef}
            />
            <button className="buttonficheiro" onClick={() => inputRef.current.click()}>Selecione o Ficheiro</button>
            <br></br>
            <br></br>
            <div>Permitido ficheiro zip com shapefile</div>
      </div>
  );
    
 
}

Wpcc_Form_ConstrucaoImportFicheiro.propTypes = {
  obj_importaficheiroconstrucao_show: PropTypes.boolean,
  obj_importaficheiroconstrucao_id: PropTypes.string
};
export default Wpcc_Form_ConstrucaoImportFicheiro;
