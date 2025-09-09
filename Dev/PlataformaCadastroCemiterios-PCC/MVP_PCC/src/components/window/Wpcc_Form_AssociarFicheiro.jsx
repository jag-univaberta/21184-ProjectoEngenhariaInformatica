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


 const Wpcc_Form_AssociarFicheiro= ({obj_associaficheiro_show, obj_associaficheiro_id, obj_associaficheiro_tipoassociacao}) => {
   
  const inputRef = useRef();
  const [files, setFiles] = useState(null);
  const [descricao, setDescricao] = useState('');
  const windowRef = useRef(null);
  const windowRefFicheiro = useRef(null);    
  const menuRef = useRef(null);
  const tabRef = useRef(null);
  const apiEndpointDocumentos = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointDocumentos);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid);
  const dispatch = useReduxDispatch();

 

  const handleDragOver = (event) => {
    event.preventDefault();
  };
  const handleDescricaoChange = (e) => {
    setDescricao(e.target.value); // Atualiza o estado conforme o usuário digita
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

        const lastModifiedDate = files[i].lastModifiedDate;

        if (lastModifiedDate) {
          // Converter lastModifiedDate para o formato yyyy-MM-dd
          const formattedDate = lastModifiedDate.toISOString().split('T')[0]; // yyyy-MM-dd

          // Atualizar o campo de input "datafile"
          const inputDateField = document.getElementById('datafile');
          if (inputDateField) {
            inputDateField.value = formattedDate;
          }
        }
      }
    }
     
    console.log('upload de ficheiro fim -------');

  }, [files]);

  // send files to the server // learn from my other video
     
    const handleUpload = async () => {     
      const campoobrigatorio = document.getElementById('descricaoobj').value || '';
      if (campoobrigatorio!=''){
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
        const descricaoValue = document.getElementById('descricaoobj').value || '';
        const observacao = document.getElementById('observacao').value || '';
        const datafile = document.getElementById('datafile').value || '';
        formData.append("Descricao", descricaoValue);
        formData.append("Dataficheiro", datafile);
        formData.append("Observacao", observacao);
        formData.append("Userid", userid);   
        formData.append("Tipoassociacao", obj_associaficheiro_tipoassociacao);
        formData.append("Codigoassociacao", obj_associaficheiro_id);        
        try {
          const url = apiEndpointDocumentos + 'FicheiroAssociado/';     
          const response = await fetch(url, {
            method: "POST",
            body:  formData,
            headers: {
              'Authorization': `Bearer ${jwtToken}`,
            },
            });  
   
          if (response.ok) {
            const hideWindow= () => dispatch({
              type: Actions.HIDE_ASSOCIAFILE,
              payload: ""});
            // Função de refresh
            const refreshFiles = () => dispatch({
              type: Actions.REFRESH_FILES_START,
              payload: ""
            });
            
            hideWindow();  
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          } 
        } catch (error) {
          console.error(error);
        }
      } else {
        dhx.alert({
          header:"Campo obrigatório",
          text:"Descrição do ficheiro obrigatória",
          buttonsAlignment:"center",
          buttons:["ok"],
        })
      }      
    };

    

    if (files) return (
      <div className="uploadsdesenho" >
          <div className="ficheirosdesenho">
            <ul>
                {Array.from(files).map((file, idx) =>{
                  console.log(file);
                  return <li key={idx}>{file.name} </li>
                })}
            </ul>  
            <table class="pccwindow_tabela  pccwindow_titulo">
              <tr><td>Dados do Ficheiro:</td></tr> 
            </table>       
            <table class="pccwindow_tabela pccwindow_detalhe">
            { <tr><td class="pccwindow_detalhe_c1" >Descrição:</td><td>
              <input type="text" id="descricaoobj" class="pcc_edit_input_ficheiro" onChange={handleDescricaoChange} />
              </td></tr>}
            {<tr><td class="pccwindow_detalhe_c1" >Data ficheiro:</td><td>
            <input type="date" id="datafile" class="pcc_edit_input_ficheiro" />
            </td></tr> }
            {<tr><td class="pccwindow_detalhe_c1" >Observações:</td><td>
              <input type="text" id="observacao" class="pcc_edit_input_ficheiro" />
              </td></tr> }
            </table>
          </div><br></br>
          
          <div className="actionsdesenho">
              <button id="buttoncancelfiledesenho" className="buttoncancelfiledesenho" onClick={() => setFiles(null)}>Cancelar</button>
              <button id="buttonuploadfiledesenho" className="buttonuploadfiledesenho" disabled={descricao.trim() === ''}  onClick={handleUpload}>Carregar</button>
          </div>
      </div>
      
    )
  return (
      <div ref={windowRef} className="dropzone dhx_popup dhx_popup--window_modal" onDragOver={handleDragOver} onDrop={handleDrop}>
            <h2>Arraste o ficheiro para carregar</h2>
            <h2>ou</h2>
            <input 
              type="file"
              onChange={(event) => setFiles(event.target.files)}
              hidden
              accept=".*"//,.dwg"
              ref={inputRef}
            />
            <button className="buttonficheiro" onClick={() => inputRef.current.click()}>Selecione o Ficheiro</button>
            <br></br>
            <br></br>
            <div></div>
      </div>
  );
    
 
}

Wpcc_Form_AssociarFicheiro.propTypes = {  
  obj_associaficheiro_show: PropTypes.boolean,
  obj_associaficheiro_id: PropTypes.string
};
export default Wpcc_Form_AssociarFicheiro;
