import React, { Component, PureComponent } from "react";
import PropTypes from 'prop-types';
import { useSelector } from "react-redux";
import { Menu as MenuDHX, TreeCollection } from "dhx-suite";
import { PersonCircle24Regular } from '@fluentui/react-icons';
import ReactDOM from 'react-dom';
import { connect } from "react-redux";
import * as Actions from "../../constants/actions";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';  
import { v4 as uuidv4 } from 'uuid'; 

const UserIcon = () => {
	return (
	  <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', width: '24px', height: '24px' }}>
		<PersonCircle24Regular />
	  </div>
	);
};
const UserDisplay = ({ username }) => {
    return (
        <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <PersonCircle24Regular />
            <span>{username}</span>
        </div>
    );
};

class Menu_InfoUser_C extends Component {
	constructor(props) {
		super(props);
		this.state = {
			 // initialize empty array for checked IDs
		};
	}
	componentDidMount() {
		let { css, data, apiEndpointCadastro, apiEndpointSIG ,username } = this.props;
		
		this.menu = new MenuDHX(this.el, {
			css: css,
			data: [
				{
					id: "user",
					value: "Utilizador",
					html: "<div id='user-display-container'></div>",
					items: [
					  {/*id:"nameuser", value: username*/},
					  { id: "profile", value: "Perfil", disabled: true},
					  { id: "settings", value: "Configurações",
						items: [
							{ id: "statemap", value: "Guardar Estado Layers" },
							{ id: "reloadmap", value: "Voltar Definição Inicial" }
						  ]
					   },
					  { id: "logout", value: "Sair", disabled: true }
					]
				},
			],
		});

		// Renderize o ícone personalizado no elemento criado
		//ReactDOM.render(<UserIcon />, document.getElementById('user-icon'));
 
        ReactDOM.render(<UserDisplay username={username} />, document.getElementById('user-display-container'));

		this.menu.events.on("click", (id, e) => {
			const menu = this.menu;  // Captura o this.menu
			
			switch (id){
				case 'statemap': 
					 
					let itenslayers_cart = this.props.treecartografia_itenschecked; 
					try{
						itenslayers_cart = itenslayers_cart.replace(/\|/g, ';');
					}catch(e){
						itenslayers_cart ='';
					}
					
					let itenslayers_gu = this.props.treeinstrumentos_itenschecked; 
					
					try{
						itenslayers_gu = itenslayers_gu.replace(/\|/g, ';');
					}catch(e){
						itenslayers_gu ='';
					}
					//console.log(itenslayers_cart);
					//console.log(itenslayers_gu);
					//console.log('state');

 
					const userid = this.props.userid; 
					const authtoken = this.props.authtoken;
					const jwtToken = authtoken;
					const tkid = this.props.aplicationTokenId;
					var estado = viewer.getState();
					//Get active map name
					var activeMapName = estado.config.activeMapName;
				
					
					var mapState = estado.mapState[activeMapName];
					var currentMap = mapState.mapguide.runtimeMap;
					var currentView = estado.mapState[activeMapName].currentView;
					var sessionId  = currentMap.SessionId;
					var scale = 0;
					var x = 0;
					var y = 0;
					if (mapState.currentView) {
						scale = mapState.currentView.scale;
						x = mapState.currentView.x;
						y = mapState.currentView.y;
					}
					/*   JSON.stringify({
										userId: userid,
										escala: scale.toString(),
										cx: x.toString(), cy: y.toString(),
										layerscart: itenslayers_cart,
										layersinstr: itenslayers_gu,
									}),
									
									string LayersCart = dto.LayersCart;
  string LayersIgt = dto.LayersInstr;

  string LayersList = LayersCart + "|" + LayersIgt;
  appCfg.Definitions = Cx + ";" + Cy + ";" + Cx + ";" + Cy + ";" + escala + "|" + LayersList;*/

					let dados = JSON.stringify({
										utilizador_id: userid,
										definicoes:  x.toString() +';' + y.toString() +';' + x.toString() +';' 
										+ y.toString() +';' +scale.toString() + "|" + itenslayers_cart+ "|" 
									});

					try {
						const setMapaLayersGuardar = async () => {
							
							const url = apiEndpointCadastro + 'CadastroEstadoApp';//ok

							const response = await fetch(url, {
								method: 'POST',
								body: dados,
								headers: {
									'Content-Type': 'application/json',
									'Authorization': `Bearer ${jwtToken}`,
								},
							});
					
							if (response.ok) {
								//console.log('ITEM GUARDADO');

								dhx.alert({
									header:"Gravação efectuada",
									text:"A Gravação foi efetuada com sucesso!",
									buttonsAlignment:"center",
									buttons:["ok"],
								  }).then(function(){
									console.log('ITEM GUARDADO');

								  });
							} else {
								const errorMessage = await response.text();
								console.error('error: ' + errorMessage);
								this.setState({ errorMessage: errorMessage });
							}
						};
						setMapaLayersGuardar();
					} catch (error) {
						console.error('error: ' + error);
						//this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Instrumentos.' });
					}
					
					break;
				case 'reloadmap': 

					let addedlayers_array=this.props.layers_iniciais;      
					let itenslayers = this.props.layers_totais;  
					let layersdesligados = itenslayers.filter(item => !addedlayers_array.includes(item));						
					let removedlayers = layersdesligados.join('|');
					var addedlayers = addedlayers_array.join('|');
					console.log('reload');


					const SetLayersInicioMap = () => { 
						
						var estado = viewer.getState();			
						var activeMapName = estado.config.activeMapName;
						var mapState = estado.mapState[activeMapName];
						var currentMap = mapState.mapguide.runtimeMap; 
						var sessionId  = currentMap.SessionId; 
						var mapaDef  = currentMap.MapDefinition; 
						var jwtToken = this.props.authtoken; 
						//this.props.guardaLayerAtivos({payload: { addedlayers: addedlayers, removedlayers: removedlayers}});
						//let aux_layers_iniciais=this.props.layers_iniciais.join('|');  
						//addedlayers = addedlayers +'|'+ aux_layers_iniciais;
						try { 
							const setCartografiaInicial = async () => {
								
								const url = this.props.apiEndpointSIG + 'mapalayers';//ok
									
								const response = await fetch(url, {
									method: 'POST',
									body: JSON.stringify({
											mapa: activeMapName,
											mapadef: mapaDef,
											sessionid: sessionId,
											viewer: 'false',
											addedlayers: addedlayers,
											removedlayers: removedlayers,
										}),
									headers: {
											'Content-Type': 'application/json',
											'Authorization': `Bearer ${jwtToken}`,
										},
								});
						
								if (response.ok) {
													
									var state = viewer.getState();
									const args = state.config;
									var NomeMapa = state.config.activeMapName;
									const uid = uuidv4();
									var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};
								
									viewer.dispatch(aux);
									viewer.dispatch({type: Actions.TREEGUCARTOGRAFIA_RELOADMAP, payload: '' })
								 
									setTimeout(() => {
										const escala1 = parseInt(this.props.mapa_initialview_escala);
										const centro_x1 = parseFloat(this.props.mapa_initialview_x);
										const centro_y1 = parseFloat(this.props.mapa_initialview_y);
									
										var estado1 = viewer.getState();
										var activeMapName1 = estado1.config.activeMapName;
									
										viewer.dispatch({
											type: 'Map/SET_VIEW',
											payload: { mapName: activeMapName1, view: { x: centro_x1, y: centro_y1, scale: escala1 } }
										});
									}, 500);  // 2000 milissegundos = 2 segundos
				
									//toggleItemCheck(this.tree, id);
		
								} else {
									const errorMessage = await response.text();
									console.error('error: ' + errorMessage);
									this.setState({ errorMessage: errorMessage });
								}
							};
							setCartografiaInicial();
						} catch (error) {
							console.error('error: ' + error);
							this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Instrumentos.' });
						}
					 
					}
					SetLayersInicioMap();

					break;
				default:
					break; 
			} 
		});
	}
	componentWillUnmount() {
		this.menu && this.menu.destructor();
		ReactDOM.unmountComponentAtNode(document.getElementById('user-icon'));
	}
	render() {
		return <div>
			<div style={{ width: "100%", maxWidth: 1200 }} ref={el => (this.el = el)}></div>
		</div>;
	}
}
 // Mapeia o estado Redux para as props do componente
 function mapStateToProps(state){
/*const apiEndpoint= this.props.apiEndpoint;s
					const userid= this.props.userid;
					const sessionid= this.props.usersession;
					const authtoken= this.props.authtoken;
					const jwtToken = authtoken;
					const tkid=this.props.aplicationTokenId;*/
 
	return { 
		treecartografia_itenschecked: state.aplicacaopcc.treecartografia.treecartografia_itenschecked,
		treeinstrumentos_itenschecked: state.aplicacaopcc.treeinstrumentos.treeinstrumentos_itenschecked,
		treecartografia_init: state.aplicacaopcc.geral.treecartografia_init,
		treeinstrumentos_init: state.aplicacaopcc.geral.treeinstrumentos_init,
		layers_iniciais: state.aplicacaopcc.geral.layers_iniciais,
 
		mapa_initialview_x: state.aplicacaopcc.geral.mapa_initialview_x ,
		mapa_initialview_y: state.aplicacaopcc.geral.mapa_initialview_y , 
		mapa_initialview_escala: state.aplicacaopcc.geral.mapa_initialview_escala ,
		
		apiEndpointAutenticacao: state.aplicacaopcc.config.configapiEndpoint, 
		apiEndpointCadastro: state.aplicacaopcc.config.configapiEndpointCadastro,  
		apiEndpointSIG: state.aplicacaopcc.config.configapiEndpointSIG,  
		sessionid: state.aplicacaopcc.geral.usersession,
		userid: state.aplicacaopcc.geral.userid,
		username: state.aplicacaopcc.geral.username,
		authtoken:   state.aplicacaopcc.geral.authtoken, 
		aplicationTokenId: state.aplicacaopcc.config.aplicationTokenId, 
		layers_totais: state.aplicacaopcc.layers_totais,
	}
}
function mapDispatchToProps(dispatch){

	return {
		  
	};
}
// Você pode adicionar mapDispatchToProps se precisar despachar ações do Redux, mas aqui não parece necessário.

export default connect(mapStateToProps,mapDispatchToProps)(Menu_InfoUser_C);