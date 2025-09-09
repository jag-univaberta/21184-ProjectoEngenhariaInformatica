import React from 'react';
import { Icon } from '@fluentui/react';
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { Earth24Regular, LocationTargetSquare24Regular, arrowDownload24Regular, ClipboardImage24Regular, EyeTracking24Regular, GlobeLocation24Regular }  from '@fluentui/react-icons';
import { invokeCommand } from 'mapguide-react-layout/lib/actions/map';
import { v4 as uuidv4 } from 'uuid';  
import IconWithLetterWhite from '../icon/IconWithLetterWhite';

import IconWithLetterWhiteMdiArrowCollapseDown from '../icon/IconWithLetterWhiteMdiArrowCollapseDown'; 
import IconWithLetterWhiteMdiMapLegend from '../icon/IconWithLetterWhiteMdiMapLegend'; 
import  {FluentEarth24Regular, MaterialSymbolsLightGlobeLocationPin  } from '../icon/Icons_Gismat';


import {ModalMenu_GestaoTabelas} from "../model/modalmenugestaotabelas";
import {ModalMenu_InfoUser} from "../model/modalinfouser";
import { PersonCircle24Regular } from '@fluentui/react-icons';
//import { invokeCommand, setActiveTool, setFeatureTooltipsEnabled } from '../actions/map';

import { getCommand, DefaultCommands } from 'mapguide-react-layout/lib/api/registry/command';
import { CompositeSelection } from 'mapguide-react-layout/lib/api/composite-selection';
import { VapingRooms } from '@mui/icons-material';
import * as Perm from "../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 
 

class PCCToolbarComponent extends React.Component {

  constructor(props) {
    super(props);
    
    var obj_info = props.detalhes_objectos.obj_info;
     
    let apiEndpoint = props.config.configapiEndpoint;

    let permissaoExportarDXF = props.permissaoExportarDXF;
    let permissaoExportarDWF = props.permissaoExportarDWF;
    let permissaoFicheirosAssociados_Consultar= props.permissaoFicheirosAssociados_Consultar;
    let permissaoFicheirosAssociados_Editar= props.permissaoFicheirosAssociados_Editar;

    //let export_dwf = props.export_dwf;
  }
 
  novoProcessoAPI = () => { 
    console.log('novo Processo');
    
    var aux={ type: Actions.PROCESSOS_NOVOPROCESSO, payload: "" };
    viewer.dispatch(aux);  
  }; 
  terminouProcessoAPI = () => { 
    console.log('terminou Processo');
    
    var aux={ type: Actions.PROCESSOS_TERMINAPROCESSO, payload: "" };
    viewer.dispatch(aux);  ;
  };
 
  handleCopyMap=() =>{ 
   
 
    if (viewer_interface ==null){
      viewer_interface = GetViewerInterface(); 
    } 
    const cmd = getCommand(DefaultCommands.Print);
    //const cmd = getCommand(DefaultCommands.QuickPlot);
    //const cmd = getCommand("PreviewPlot");
    if (cmd) {
        viewer.dispatch(invokeCommand(cmd));
    } 

  }
 
  handleTrackCoordenadas=() =>{ 
    var aux={ type:'MAPA/SHOW_TRACKCOORDENADAS', payload: ''}; 
    viewer.dispatch(aux);  


    
    //var aux={ type:'Modal/SHOW_URL', payload: { modal: {title: 'Add Manage Layers', backdrop: false, size: [350, 500], overflowYScroll: true}, name:  'Add Manage Layers', url: 'component://AddManageLayers' } };
    //viewer.dispatch(aux); 
  } 

   
  handleObterCoordenadas=() =>{
    const OnTerminouDesenhoPT = (objetodesenhado)=> { 
      let coordenadas = objetodesenhado.flatCoordinates;
      
      const centro_x = parseFloat(coordenadas[0]);
      const centro_y = parseFloat(coordenadas[1]);
     
      const width = 1000;
      const height = 1000; 
      // Calculate the initial map scale
      const resolution = Math.max(width / 800, height / 600);
      const escala = parseInt( resolution / 0.00028) + 50;
   
      var state = viewer.getState();
      const args = state.config;
      var NomeMapa = state.config.activeMapName;
      var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }};
      //zoomToView(aux);
      viewer.dispatch(aux);

      //const coords: [number, number] = [-9.1393, 38.7223]; // Exemplo de coordenadas (Lisboa)
      const coords = [centro_x, centro_y];  
      const srid = "3763"; // Sistema de referência original (ETRS89)
      var aux={ type:'MAPA/SHOW_COORDENADAS', payload: { coordenadas: coords,  srid: srid }};
      //zoomToView(aux);
      viewer.dispatch(aux);
  

    }
    viewer_interface = GetViewerInterface(); 
    factory = viewer_interface.getOLFactory();
 
    viewer_interface.digitizePoint(OnTerminouDesenhoPT);
   
  }
   
  handleCentrarCoordenadas=() =>{
    var aux={ type: Actions.SHOW_MAPA_ZOOMCOORDENADAS, payload: true };
    viewer.dispatch(aux); 
    
  }
  handleSeleccione=() =>{
    dhx.alert({
      header:"Seleccione um objecto",
      text:"Necessita de ter um objecto seleccionado!",
      buttonsAlignment:"center",
      buttons:["ok"],
    });  
  }

  handleMeasure = () => {
    // Perform Measure action
    console.log('Measure');

    var aux={ type:'Modal/SHOW_URL', payload: { modal: {title: 'Medir', backdrop: false, size: [350, 500], overflowYScroll: true}, name:  'Measure', url: 'component://Measure' } };
    viewer.dispatch(aux); 
  }; 

  handleZoomInicial = () => {
    // Perform handleZoomInicial action
    console.log('handleZoomInicial');

    var viewerstate = viewer.getState();
    //Get active map name
    var activeMapName = viewerstate.config.activeMapName;

    var mapState = viewerstate.mapState[activeMapName];
    //var currentMap = mapState.mapguide.runtimeMap;

    var view = viewerstate.mapState[activeMapName].history[0]; 
    var escala = view.scale; 
 
    var aux={ type:'Map/SET_VIEW', payload: { view: {resolution: view.resolution, scale: escala, x: view.x , y: view.y }, mapName: activeMapName } };
    viewer.dispatch(aux); 
  };

  
   handleZoomAnterior = () => {
    // Perform handleZoomInicial action
    console.log('handleZoomInicial');

    var viewerstate = viewer.getState();
    //Get active map name
    var activeMapName = viewerstate.config.activeMapName; 
  
    var aux={ type:'Map/PREVIOUS_VIEW', payload: { mapName: activeMapName } };
    viewer.dispatch(aux); 
  };


  handleZoomSeguinte = () => {
    // Perform handleZoomInicial action
    console.log('handleZoomInicial');
    var viewerstate = viewer.getState(); 
    
    //Get active map name
    var activeMapName = viewerstate.config.activeMapName; 
    var aux={ type:'Map/NEXT_VIEW', payload: { mapName: activeMapName } };
    viewer.dispatch(aux); 
  };


  handleZoomMais = () => {
    // Perform handleZoomMais action
    console.log('handleZoomMais');

    if (viewer_interface ==null){
      viewer_interface = GetViewerInterface(); 
    }  
    const cmd = getCommand(DefaultCommands.ZoomIn);
    if (cmd) {
      try{
        viewer.dispatch(invokeCommand(cmd));
      }
      catch(e){
        console.log(e);
      }
    }
 
  };

  handleZoomMenos = () => {
    // Perform handleZoomMenos action
    console.log('handleZoomMenos');

    if (viewer_interface ==null){
      viewer_interface = GetViewerInterface(); 
    }  
    const cmd = getCommand(DefaultCommands.ZoomOut);
    if (cmd) {
      try{
        viewer.dispatch(invokeCommand(cmd));
      }
      catch(e){
        console.log(e);
      }
    }
 
  };

  handleZoomWindow = () => {
    // Perform handleSelectMode action
    console.log('handleZoomWindow');

    var aux={ type:'Map/SET_ACTIVE_TOOL', payload: 0 };
    viewer.dispatch(aux); 
  };

  handleSelectMode = () => {
    // Perform handleSelectMode action
    console.log('handleSelectMode');

    var aux={ type:'Map/SET_ACTIVE_TOOL', payload: 1 };
    viewer.dispatch(aux); 
  };
  handlePanMode = () => {
    // Perform handleZoomMenos action
    console.log('handlePanMode');

    
    var aux={ type:'Map/SET_ACTIVE_TOOL', payload: 2 };
    viewer.dispatch(aux); 
  };

  handleBuffer = () => {
    // Perform Buffer action
    console.log('Buffer');
    var aux={ type: Actions.SHOW_MAPA_CRIARBUFFER, payload: true };
    viewer.dispatch(aux); 

    try{
      var viewer_interface = GetViewerInterface();
      var selection = viewer_interface.getSelection();
     
      if (selection && selection.SelectedFeatures) {
        const st = viewer.getState();
        //const selection = getSelectionSet(st);
        let cs;
        if (st.config.activeMapName) {
            cs = st.mapState[st.config.activeMapName].clientSelection;
        }
        var compSel = new CompositeSelection(selection?.SelectedFeatures, cs);
        const bounds = compSel.getBounds();
        console.log(bounds);
      }
    } catch(e){
      console.log(e);
    }
     
  };
 
 
  componentDidMount() {
    // colocar a seta como definida - o modo de selecção faz pan com o botão do meio
    //var aux = { type: 'Map/SET_ACTIVE_TOOL', payload: 1 };
    // Pedido para o pan ser colocado por defeito no arranque das aplicações 
    var aux = { type: 'Map/SET_ACTIVE_TOOL', payload: 2 };
    viewer.dispatch(aux);

    // aqui já se pode dizer ao mapa para adicionar o layer do elementos desenhados
    // mais importante é remover elementos antigos da visualizacao
    var estado = viewer.getState(); 
    var activeMapName = estado.config.activeMapName;
    var mapState = estado.mapState[activeMapName];
    var currentMap = mapState.mapguide.runtimeMap;
    var sessionId  = currentMap.SessionId;
    var mapaDef  = currentMap.MapDefinition;
  
			try {
        // isto  esta a ser executado no arranque
				const setLayerObjetosDesenhados = async () => {
          console.log(' SET EPL LAYER ');
          let url = this.props.config.configapiEndpoint + 'MapaLayerDesenho';
          let jwtToken = this.props.authtoken;
          let userid = this.props.userid;
          let usersession = this.props.usersession;
          let appsigla = this.props.aplicacao_sigla;
          let contorno = this.props.contorno;
          let valor_cor = this.props.valor_cor;
        
   
          const response = await fetch(url, {
						method: 'POST',
						body: JSON.stringify({
								mapa: activeMapName,
								mapadef: mapaDef,
								sessionid: sessionId,
								userid: userid,
								usersession: usersession,
								viewer: 'false', 
                valor_cor: valor_cor,
                contorno: contorno,
                Sigla: appsigla,
							}),
						headers: {
								'Content-Type': 'application/json',
								'Authorization': `Bearer ${jwtToken}`
							},
					});
			
					if (response.ok) {
            console.log(' SET EPL LAYER FIM');
						var state = viewer.getState();
						const args = state.config;
						var NomeMapa = state.config.activeMapName;
						const uid = uuidv4();
						var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }}; 
						viewer.dispatch(aux);
            // depois do refresh do mapa
            // colocar no store que pode avancar
						var aux={ type:'APP/START', payload: ''}; 
						viewer.dispatch(aux);
					} else {
						const errorMessage = await response.text();
						console.error('error: ' + errorMessage);
						this.setState({ errorMessage: errorMessage });
					}
           
				};
				//setLayerObjetosDesenhados();
        var state = viewer.getState();
        const args = state.config;
        var NomeMapa = state.config.activeMapName;
        const uid = uuidv4();
        var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }}; 
        viewer.dispatch(aux);
        // depois do refresh do mapa
        // colocar no store que pode avancar
        var aux={ type:'APP/START', payload: ''}; 
        viewer.dispatch(aux);
			} catch (error) {
				console.error('error: ' + error); 
			} 
 
      try {				
        const saveGeometriaClean = async () => {                   
          const url = this.props.config.configapiEndpoint  + 'MapaLayerDesenhoClean/';//ok   
          const jwtToken = this.props.authtoken;     
          const aux_usersession = this.props.usersession;  
          const aux_userid = this.props.userid;
          const aux_aplicacao_sigla = this.props.aplicacao_sigla;
          var estado = viewer.getState();			
          var activeMapName = estado.config.activeMapName;
          var mapState = estado.mapState[activeMapName];
          var currentMap = mapState.mapguide.runtimeMap; 
          var sessionId  = currentMap.SessionId; 
          var mapaDef  = currentMap.MapDefinition;
           
          const response = await fetch(url, {
            method: 'POST',
            body: JSON.stringify({ 
              aplicacao_sigla : aux_aplicacao_sigla,
              mapa: activeMapName,
              mapadef: mapaDef,
              sessionid: sessionId, 
              userid: aux_userid,
              usersession : aux_usersession,   
              viewer: 'false', 
            }),
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`,
            },
          });              
          if (response.ok) {
            // mapa refresh
                
            var state = viewer.getState();
            const args = state.config;
            var NomeMapa = state.config.activeMapName;
            const uid = uuidv4();
              var aux1={ type:'Map/CLEAR_CLIENT_SELECTION', payload: { mapName: NomeMapa }};     
            viewer.dispatch(aux1); 
            var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};                     
            viewer.dispatch(aux);   
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        //saveGeometriaClean();
      } catch (error) {
        console.error('error: ' + error);                 
      } 
     

  }
  render() {
    console.log('props: ' + this.props.obj_info_tipo);
    let sigla_separador="";
    let id_objeto = ''; 
    let id_parent = '';
    var viewerstate = viewer.getState();
    //Get active map name
    var activeMapName = viewerstate.config.activeMapName; 
    let hasSelection = false;
    let hasSelectionLocalizaArvore = false;
    let hasSelectionFicheirosAssociados = false;
    const selection = viewerstate.mapState[viewerstate.config.activeMapName].mapguide.selectionSet;
    hasSelection = (selection != null && selection.SelectedFeatures != null);

    if (selection && selection.SelectedFeatures) {
      try {
        const selectedLayer = selection.SelectedFeatures.SelectedLayer;
        console.log('selectedLayer: ' + JSON.stringify(selectedLayer)); 

        const primeiroElemento = selectedLayer[0];
        let layerName = primeiroElemento['@name'];
        console.log('selectedLayer: ' + layerName); 
        const feature = selectedLayer[0].Feature.length;
        console.log('feature length: ' + feature);   
        hasSelectionFicheirosAssociados = (selection != null && selection.SelectedFeatures != null && feature == 1 );
        if (((this.props.obj_info_tipo==='PCC')&& (feature == 1))||((this.props.obj_info_tipo==='ATE')&& (feature == 1))){
           
          hasSelectionLocalizaArvore = true; 
          switch (layerName.toUpperCase())
          {
              case "PCC_Construcao".toUpperCase():
                sigla_separador="CEM"; 
                break; 
          } 
          const obj = selectedLayer[0].Feature;
          const properties = obj[0].Property;
          if (properties) {
              for (let k = 0; k < properties.length; k++) {
                  if (sigla_separador===''){
                    if (properties[k].Name.toUpperCase()=='ID'){
                        if (properties[k].Value !== null){
                          id_objeto=properties[k].Value ?? '';
                        }                                                
                    } 
                  }
                  if (sigla_separador==='CEM'){
                    if (properties[k].Name.toUpperCase()=='ID'){
                      if (properties[k].Value !== null){
                        id_objeto=properties[k].Value ?? '';
                      }                                                
                    }  
                  }
                  if (sigla_separador==='ATE'){ 
                    if (properties[k].Name.toUpperCase()=='ID_ATENDIMENTO'){
                      if (properties[k].Value !== null){
                        id_objeto=properties[k].Value ?? '';
                      }                                                
                    }
                  }
                  if (properties[k].Name.toUpperCase()=='PARENT_ID'){
                    if (properties[k].Value !== null){
                      id_parent=properties[k].Value ?? '';
                    }                                                
                }  
               }
          }
          if (this.props.aplicacao_sigla!='EPL'){
            var aux_payload = {
              sigla_separador: sigla_separador,
              parent_id: id_parent,
              rec_id: id_objeto 
            }
            var aux={ type: Actions.SIDEBARLOCALIZA_DADOS, payload: aux_payload };
            viewer.dispatch(aux); 
          } else {
            hasSelectionLocalizaArvore = false; 
          }

        
        } else {
          
          var aux={ type: Actions.GRELHALOCALIZA_END, payload: '' };
          viewer.dispatch(aux); 
        }
        
        if (this.props.obj_info_tipo=='EPL'){
          console.log('EPL: '); 
          hasSelectionFicheirosAssociados = false;
        }
      
       

      }catch (e){
        console.log('error: ' + e); 
      }
    } 
    let Export_dwf=false;
    try{
      var viewa = viewerstate.mapState[viewerstate.config.activeMapName].currentView; 
      var escala = viewa.scale; 
      if (escala<=5000){
        Export_dwf=true;
      }
    }catch(e){
      console.log('error: ' + e); 
    }
    

    //const obj_info = useSelector((state)=> state.aplicacaopcc.windows.detalhes_objectos.obj_info);
    //let export_dwf= this.props.export_dwf;
    let permissaoExportarDXF= this.props.permissaoExportarDXF;
    let permissaoExportarDWF= this.props.permissaoExportarDWF;
    let permissaoFicheirosAssociados = this.props.permissaoFicheirosAssociados_Consultar || this.props.permissaoFicheirosAssociados_Editar;
    let permissaoImpressaoEditar= this.props.permissaoImpressaoEditar;
    let apiEndpoint = this.props.config.configapiEndpoint; 
    let valor_cor =this.props.config.valor_cor;
    let contorno = this.props.config.contorno; 
    let showSelectionLocalizaArvore =  (this.props.aplicacao_sigla!='EPL'); 
    

    return (
      <div className="pcctoolbar">
          <div className="pcctoolbarseparator">&nbsp;</div>
          <div className="pcctoolbarseparator">&nbsp;</div>

          <div className="pcctoolbarseparator">&nbsp;</div>
          <div className="pcctoolbarseparator">&nbsp;</div>          
          <div class="pcctoolbarmenu">
          <div className="pcctoolbaritemmenutabelas" title=""> 
            {<ModalMenu_GestaoTabelas permissaoGestaoRegulamentos/>}
          </div> 
          <div className="pcctoolbarseparator">&nbsp;</div>
          <div className="pcctoolbarseparator">&nbsp;</div>  
          <div className="pcctoolbarseparator">&nbsp;</div>

          <div className="pcctoolbaritem" onClick={this.handleCopyMap} title="Copiar imagem do mapa"> 
            <ClipboardImage24Regular /> 
          </div> 
          <div className="pcctoolbaritem" onClick={this.handleTrackCoordenadas} title="Monitor de Coordenadas"> 
             <GlobeLocation24Regular /> 
              
          </div> 
          <div className="pcctoolbarseparator">&nbsp;</div> 
          <div className="pcctoolbarseparator">&nbsp;</div>
          <div className="pcctoolbaritem" onClick={this.handleMeasure} title="Medir"> 
            <img className="pcctoolbaricon" style={{ fontSize: '20px', width: '20px', height:'20px' }} src="./stdicons/medicao.svg" >
            </img>
          </div> 
 
          <div className="pcctoolbarseparator">&nbsp;</div>
          <div className="pcctoolbaritem"  onClick={this.handleZoomInicial} title="Zoom inicial"> 

            <Icon iconName="ZoomToFit" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
          </div>
          <div className="pcctoolbaritem" onClick={this.handleZoomAnterior} title="Zoom anterior"> 
            <img className="pcctoolbaricon"   style={{ fontSize: '20px', width: '20px', height:'20px' }}  src="./stdicons/zoom_anterior_white2.svg" >
            </img>
          </div>


          <div className="pcctoolbarseparator">&nbsp;</div>
          <div className="pcctoolbaritem" onClick={this.handleZoomWindow} title="Zoom janela"> 

            <Icon iconName="SearchAndApps" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
            
          </div>
          <div className="pcctoolbaritem" onClick={this.handleZoomMais} title="Zoom mais"> 
      
            <Icon iconName="ZoomIn" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
          </div>
          <div className="pcctoolbaritem" onClick={this.handleZoomMenos} title="Zoom menos"> 
        
            <Icon iconName="ZoomOut" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
          </div>
          <div className="pcctoolbarseparator">&nbsp;</div>
          <div className="pcctoolbaritem" onClick={this.handleSelectMode} title="Seleção"> 
            <img className="pcctoolbaricon" style={{ fontSize: '20px', width: '20px', height:'20px' }} src="./stdicons/select.svg" >
            </img>
          </div>
          <div className="pcctoolbaritem" onClick={this.handlePanMode} title="Pan"> 
        
            <Icon iconName="HandsFree" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
          </div>
          </div> 
          <div className="pcctoolbaritemuser" title={this.props.username ? this.props.username : "Utilizador"}>
              {<ModalMenu_InfoUser/>}
          </div> 
        <table class="table-icons"> 
          <tr>
            <div className="pcctoolbaritem" onClick={this.handleCopyMap} title="Copiar imagem do mapa"> 
             <ClipboardImage24Regular /> 
            </div> 
          </tr>
          <tr>
           <div className="pcctoolbaritem" onClick={this.handleTrackCoordenadas} title="Monitor de Coordenadas"> 
             <MaterialSymbolsLightGlobeLocationPin /> 
               
           </div>  
          </tr>  
          <tr>
              <div className="pcctoolbaritem" onClick={this.handleMeasure} title="Medir"> 
                <img className="pcctoolbaricon" style={{ fontSize: '20px', width: '20px', height:'20px' }} src="./stdicons/medicao.svg" >
                </img>
              </div>
          </tr>
          <tr>
              <div className="pcctoolbaritem"  onClick={this.handleZoomInicial} title="Zoom inicial"> 
    
              <Icon iconName="ZoomToFit" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
              </div>
          </tr>
          <tr>
              <div className="pcctoolbaritem" onClick={this.handleZoomAnterior} title="Zoom anterior"> 
                <img className="pcctoolbaricon"   style={{ fontSize: '20px', width: '20px', height:'20px' }}  src="./stdicons/zoom_anterior_white2.svg" >
                </img>
              </div>
          </tr>
          <tr>
              <div className="pcctoolbaritem" onClick={this.handleZoomWindow} title="Zoom janela"> 
       
              <Icon iconName="SearchAndApps" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
              
              </div>
          </tr>
          <tr>
            <div className="pcctoolbaritem" onClick={this.handleZoomMais} title="Zoom mais"> 
   
            <Icon iconName="ZoomIn" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
            </div>
          </tr>
          <tr>
            <div className="pcctoolbaritem" onClick={this.handleZoomMenos} title="Zoom menos"> 
 
              <Icon iconName="ZoomOut" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
            </div>
          </tr>
          <tr>
            <div className="pcctoolbaritem" onClick={this.handleSelectMode} title="Seleção"> 
              <img className="pcctoolbaricon" style={{ fontSize: '20px', width: '20px', height:'20px' }} src="./stdicons/select.svg" >
              </img>
            </div>
          </tr>
          <tr>
            <div className="pcctoolbaritem" onClick={this.handlePanMode} title="Pan"> 
  
              <Icon iconName="HandsFree" style={{ fontSize: '20px', width: '20px', height:'20px' }} />
            </div>
          </tr> 
        </table>
      </div>
       
     
    );
  }
};
 
function mapStateToProps(state){

  const obj_info_tipo = state.aplicacaopcc.windows.detalhes_objectos.obj_info_tipo;
  const listaDeCodigosPermissoes = Perm.listaDeCodigosGestaoTabelas;
  const funcionalidade = state.aplicacaopcc.funcionalidades;
        
  // Filter permissions that have cod in listaDeCodigosPermissoes
  const funcionalidadesRelevantes = funcionalidade.filter(permission => {
      // Check if cod is in listaDeCodigosInteressantes
    return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
  });
  const permissaoExportarDXF = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_ExportarDXF, '');
  const permissaoExportarDWF = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_ExportarDWF, '');
  const permissaoFicheirosAssociados_Consultar = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_FicheirosAssociados_Consultar, '');
  const permissaoFicheirosAssociados_Editar = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_FicheirosAssociados_Editar, '');
  const permissaoImpressaoEditar = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_Impressao_Editar, '');
  
	return {
		config: state.aplicacaopcc.config,
    authtoken: state.aplicacaopcc.geral.authtoken,
    userid: state.aplicacaopcc.geral.userid,
    aplicacao_sigla: state.aplicacaopcc.aplicacao_sigla,
    username: state.aplicacaopcc.geral.username,
    usersession: state.aplicacaopcc.geral.usersession, 
    detalhes_objectos: state.aplicacaopcc.windows.detalhes_objectos,
    exportar_dwf_dxf: state.aplicacaopcc.exportar_dwf_dxf, 
    valor_cor: state.aplicacaopcc.windows.definir_cor.valor_cor,
    contorno: state.aplicacaopcc.windows.definir_cor.contorno, 
    permissaoExportarDXF: permissaoExportarDXF,
    permissaoExportarDWF: permissaoExportarDWF,
    permissaoImpressaoEditar: permissaoImpressaoEditar,
    permissaoFicheirosAssociados_Consultar: permissaoFicheirosAssociados_Consultar,
    permissaoFicheirosAssociados_Editar: permissaoFicheirosAssociados_Editar,
    obj_info_tipo: obj_info_tipo
	};
}
//Envia nova propriedades para o estado
function mapDispatchToProps(dispatch){

	return {
		 
	};
}
 
export default connect(mapStateToProps,mapDispatchToProps)(PCCToolbarComponent);