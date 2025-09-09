//
import React, { Component, PureComponent } from "react";
import * as Perm from "../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 

import PropTypes, { array } from "prop-types";
import {List as ListDHX, DataCollection ,Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, TreeCollection, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import "dhx-suite/codebase/suite.min.css";
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useSelector } from 'react-redux';

import { Client } from "mapguide-react-layout/lib/api/client";
import { refresh } from 'mapguide-react-layout/lib/actions/legend';
import { RuntimeMapFeatureFlags } from "mapguide-react-layout/lib/api/request-builder";
import { ActionType } from 'mapguide-react-layout/lib/constants/actions';
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';
import { v4 as uuidv4 } from 'uuid';
import { processData } from '../../utils/utils';

  class TreeC extends Component {

	constructor(props) {
		super(props);
		this.state = {
			checkedLayers: [], 
			addedlayers: [], 
			removedlayers: [], // initialize empty array for checked IDs
			checkedElem: []
		};
		this.tree = React.createRef();
		this.janela = React.createRef();
		this.tabRef = React.createRef();
		this.FormDadosEnt = React.createRef();
		this.ListDados = React.createRef();
	}
	
	componentDidMount() {
		//console.log('componentDidMount - treeCartografia_G');
		const { css, data, keyNavigation, height, template, itemHeight, virtual, checkbox,
			 align, apiEndpoint, apiEndpointSIG, authtoken, userid,aplicationTokenId, usersession, 
			 permissaoNovoItem, 
			 permissaoEditarItem, permissaoEditarOrdem } = this.props;
		this.tree = new TreeDHX(this.el, {
			css: css,
			keyNavigation: keyNavigation,
			checkbox: true,
			data: data,
		}); 
		const TreeCart = this.tree;
 
		//var isChecks=[];
		this.tree.events.on("afterCheck",  (index, id, value) =>{ 
			//console.log("afterCheck: " + value);
			/*let isChecks= this.state.checkedElem;
			
			let isChecked = isChecks.includes(id);
			
			if (isChecked) {
				this.tree.unCheckItem(id);
			} else {
				this.tree.checkItem(id);
			}*/


			const SetLayersMap = () => {
				var checkedElem = TreeCart.getChecked();
				let layers = this.tree.data.map(function(element) {
					if (checkedElem.includes(element.id)) {
						return element.layers;
					}
				});
				// Remover valores undefined   
			    layers = layers.filter(function(id) {
					return id !== undefined;
				});
				// Remover valores vazios   
				layers = layers.filter(function(id) {
					return id !== '';
				});		
				let auxdados= this.tree.data; 
				console.log(auxdados);
				let rec_ids = this.tree.data.map(function(element) {
					if (checkedElem.includes(element.id)) {
						//if (element.tipo=='ITEM'){
							return element.rec_id;
						//} 
					}
				});
				layers = layers.flatMap(str => str.split(','));

				//console.log(layers);
				// Remover valores undefined   
				rec_ids = rec_ids.filter(function(id) {
					return id !== undefined;
				});
				// Remover valores vazios   
				rec_ids = rec_ids.filter(function(id) {
					return id !== '';
				});
				let estado_arvore=rec_ids.join('|');
				//console.log('Cart guardaEstadoArvore ' + estado_arvore);	
				this.props.guardaEstadoArvore({payload: { dados: estado_arvore}});

				this.setState(prevState => { 
					//console.log('SetLayersMap set State');			  
					var addedlayers = layers.filter(function(id) {
						return !prevState.checkedLayers.includes(id);
					});
					var removedlayers  = prevState.checkedLayers.filter(function(id) {
						return !layers.includes(id);
					}); 
					var new_checkedElem=[...checkedElem];
					var new_checkedLayers=[...layers];
					console.log(' ------ AFTER CHECK ------ ');
					console.log('Cart addedlayers ' + addedlayers);	
					console.log('Cart removedlayers ' + removedlayers);
					//console.log('Cart checkedElem ' + new_checkedElem);
					console.log('Cart checkedLayers ' + new_checkedLayers);		
					console.log(' ------ AFTER CHECK ------ ');  
					return { 
						addedlayers: addedlayers,
						removedlayers: removedlayers, 
						checkedElem: [...checkedElem],
						checkedLayers:  [...layers]
					};
				}, () => { 
					//console.log('SetLayersMap fim set State');	
					var estado = viewer.getState();			
					var activeMapName = estado.config.activeMapName;
					var mapState = estado.mapState[activeMapName];
					var currentMap = mapState.mapguide.runtimeMap; 
					var sessionId  = currentMap.SessionId; 
					var mapaDef  = currentMap.MapDefinition;
					var addedlayers_array = this.state.checkedLayers;
					var removedlayers_array = this.state.removedlayers;

					var addedlayers = this.state.addedlayers.join('|');
					var removedlayers = this.state.removedlayers.join('|');
					var jwtToken = authtoken;
					//console.log('Cart addedlayers2 ' + addedlayers);	
					//console.log('Cart removedlayers2 ' + removedlayers);
					 
					try { 
						const setCartografiaItemCheck = async (addedlayers, removedlayers) => {
							//console.log('Cart addedlayers3 ' + addedlayers);	
							//console.log('Cart removedlayers3 ' + removedlayers);
							const url = apiEndpointSIG + 'mapalayers';//ok
	
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
								//console.log('Cart addedlayers4 ' + addedlayers);	
								//console.log('Cart removedlayers4 ' + removedlayers);
								this.props.guardaLayerAtivos({payload: { addedlayers: addedlayers_array, removedlayers: removedlayers_array}});
								
								var state = viewer.getState();
								const args = state.config;
								var NomeMapa = state.config.activeMapName;
								const uid = uuidv4();
								var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};
							
								viewer.dispatch(aux);
								//toggleItemCheck(this.tree, id);
	
							} else {
								const errorMessage = await response.text();
								console.error('error: ' + errorMessage);
								this.setState({ errorMessage: errorMessage });
							}
						};
						setCartografiaItemCheck(addedlayers, removedlayers);
					} catch (error) {
						console.error('error: ' + error);
						this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Instrumentos.' });
					}
				});
			}
			SetLayersMap();
		});
		let objtreeid;
		let objid;
		let objtipo;
		let objidparent;
		let objname;
		 
		let objidtipo;
		this.tree.events.on("itemRightClick", (id,e) => { 
			objtreeid = id;  
			objid = this.tree.data?.getItem(id)?.rec_id;
			objtipo  = this.tree.data?.getItem(id)?.tipo ;
			objname = this.tree.data?.getItem(id)?.value;
			objidparent= this.tree.data?.getItem(id)?.parentid; 
			
			//Tratar as permissões
			if (objtipo =='parent'){
 
			  	if (permissaoNovoItem) { 
					cmenu.enable("new_cartografia");  
				}else {
					cmenu.disable("new_cartografia");  
				}
				cmenu.disable("edit_cartografia");
				if (permissaoEditarOrdem) { 
					cmenu.enable("new_cartografia");  
				}else {
					cmenu.disable("new_cartografia");  
				}   
			} else {
				if (permissaoNovoItem) { 
					cmenu.enable("new_cartografia");  
				}else {
					cmenu.disable("new_cartografia");  
				}
				if (permissaoEditarItem) { 
					cmenu.enable("edit_cartografia");  
				}else {
					cmenu.disable("edit_cartografia");  
				} 
				if (permissaoEditarOrdem) { 
					cmenu.enable("new_cartografia");  
				}else {
					cmenu.disable("new_cartografia");  
				}  ;
			}  
			e.preventDefault();
			cmenu.showAt(e,id); 
		});
		 
		function ProcessaCheck(id, contexto){ 
			 
			/*let isChecks = contexto.state.checkedElem;
			
			let isChecked = isChecks.includes(id);
			
			if (isChecked) {
				contexto.tree.unCheckItem(id);
			} else {
				contexto.tree.checkItem(id);
			}*/
			const SetLayersMap = () => {
				var checkedElem = TreeCart.getChecked();
				let layers = contexto.tree.data.map(function(element) {
					if (checkedElem.includes(element.id)) {
						return element.layers.replace(/(\|{4})/g, '|');
					}
				});
				// Remover valores undefined   
			   layers = layers.filter(function(id) {
					return id !== undefined;
				});
				// Remover valores vazios   
				layers = layers.filter(function(id) {
					return id !== '';
				});				 
				let rec_ids = contexto.tree.data.map(function(element) {
					if (checkedElem.includes(element.id)) {
						if (element.tipo=='cartografia'){
							return element.rec_id;
						} 
					}
				});
				layers = layers.flatMap(str => str.split('|'));

				//console.log(layers);
				// Remover valores undefined   
				rec_ids = rec_ids.filter(function(id) {
					return id !== undefined;
				});
				// Remover valores vazios   
				rec_ids = rec_ids.filter(function(id) {
					return id !== '';
				});
				let estado_arvore=rec_ids.join('|');
				//console.log('Cart guardaEstadoArvore ' + estado_arvore);	
				contexto.props.guardaEstadoArvore({payload: { dados: estado_arvore}});

				contexto.setState(prevState => { 
					//console.log('SetLayersMap set State');			  
					var addedlayers = layers.filter(function(id) {
						return !prevState.checkedLayers.includes(id);
					});
					var removedlayers  = prevState.checkedLayers.filter(function(id) {
						return !layers.includes(id);
					}); 
					var new_checkedElem=[...checkedElem];
					var new_checkedLayers=[...layers];
					console.log(' ------ AFTER CHECK ------ ');
					console.log('Cart addedlayers ' + addedlayers);	
					console.log('Cart removedlayers ' + removedlayers);
					//console.log('Cart checkedElem ' + new_checkedElem);
					console.log('Cart checkedLayers ' + new_checkedLayers);		
					console.log(' ------ AFTER CHECK ------ ');  
					return { 
						addedlayers: addedlayers,
						removedlayers: removedlayers, 
						checkedElem: [...checkedElem],
						checkedLayers:  [...layers]
					};
				}, () => { 
					//console.log('SetLayersMap fim set State');	
					var estado = viewer.getState();			
					var activeMapName = estado.config.activeMapName;
					var mapState = estado.mapState[activeMapName];
					var currentMap = mapState.mapguide.runtimeMap; 
					var sessionId  = currentMap.SessionId; 
					var mapaDef  = currentMap.MapDefinition;
					var addedlayers_array = contexto.state.checkedLayers;
					var removedlayers_array = contexto.state.removedlayers;

					var addedlayers = contexto.state.addedlayers.join('|');
					var removedlayers = contexto.state.removedlayers.join('|');
					var jwtToken = authtoken;
					//console.log('Cart addedlayers2 ' + addedlayers);	
					//console.log('Cart removedlayers2 ' + removedlayers);
					 
					try { 
						const setCartografiaItemCheck = async (addedlayers, removedlayers) => {
							//console.log('Cart addedlayers3 ' + addedlayers);	
							//console.log('Cart removedlayers3 ' + removedlayers);
							const url = apiEndpointSIG + 'mapalayers';//ok
	
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
								//console.log('Cart addedlayers4 ' + addedlayers);	
								//console.log('Cart removedlayers4 ' + removedlayers);
								contexto.props.guardaLayerAtivos({payload: { addedlayers: addedlayers_array, removedlayers: removedlayers_array}});
								c
								var state = viewer.getState();
								const args = state.config;
								var NomeMapa = state.config.activeMapName;
								const uid = uuidv4();
								var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};
							
								viewer.dispatch(aux);
								//toggleItemCheck(this.tree, id);
	
							} else {
								const errorMessage = await response.text();
								console.error('error: ' + errorMessage);
								contexto.setState({ errorMessage: errorMessage });
							}
						};
						setCartografiaItemCheck(addedlayers, removedlayers);
					} catch (error) {
						console.error('error: ' + error);
						contexto.setState({ errorMessage: 'Ocorreu um erro ao atualizar Instrumentos.' });
					}
				});
			}
			SetLayersMap();
		};
		const cmenu = new dhx.ContextMenu(null, {
			css: "dhx_widget--bg_gray",
			data: [			  
			  { id: "new_cartografia", value: "Nova Cartografia" }, 
			  { type: "separator" } ,
			  { id: "edit_cartografia", value: "Editar Cartografia" }, 
			  { type: "separator" } ,
			  { id: "ordenacao",value: "Editar Ordenação" } ,
			  { type: "separator" } ,
			  { id: "ligarlayers",value: "Ligar Layers" } ,
			  { id: "desligarlayers",value: "Desligar Layers" } ,
			  { type: "separator" } ,
			  { id: "expandir",value: "Expandir" } ,
			  { id: "contrair",value: "Contrair" } ,

			]
		});
		cmenu.events.on("click", (id, e) => {
		
			 
			//console.log(id);
			switch(id){
			case 'reload':  
				//console.log('reload');
				ReloadData(); 
				break;
			case 'ligarlayers':
                {
					 
					console.log(objidtipo);
					/*if (objidtipo=='GRUPO'){
						this.tree.uncheckItem(objtreeid); 
					} */
					this.tree.checkItem(objtreeid);
					 
					ProcessaCheck(objtreeid, this);
                    break;
                }
            case 'desligarlayers':
                {
                    
					console.log(objidtipo);
					/*if (objidtipo=='GRUPO'){
						this.tree.checkItem(objtreeid); 
					}  */
					this.tree.uncheckItem(objtreeid); 
					ProcessaCheck(objtreeid, this); 
                    break;
                }
            case 'expandir':
                {
                    this.tree.expandAll();
                    break;
                }
            case 'contrair':
                {
                    this.tree.collapseAll();
                    break;
                }
			 
			case 'new_cartografia':  
				 
				// Criar um novo ITEM de Cartografia
				// Chama Actions.CARTOGRAFIAITEM_NEW
				var aux={ type:Actions.CARTOGRAFIAITEM_NOVO, payload: objid}; 
				viewer.dispatch(aux);
	 
				break;
			case 'edit_cartografia':     
					  
				// Edita o ITEM de Cartografia
				// Chama Actions.CARTOGRAFIAITEM_EDITA

				var aux={ type:Actions.CARTOGRAFIAITEM_EDITA, payload: objid}; 
				viewer.dispatch(aux);

				break;
	
			case 'remover':  
			// Função principal
				const removeItem = (id_tree) => {
					console.log('remover');
					console.log('ID do item:', id_tree);

					if (!id_tree) {
						console.log("ID inválido ou não encontrado");
						return;
					}

					const item = this.tree.data?.getItem(id_tree);
					console.log('Item:', item);

					if (!item) {
						console.log("Item não encontrado");
						return;
					}

					// Função recursiva para verificar todos os itens e seus descendentes
					const checkItemsRecursively = (items) => {
						if (!items || !Array.isArray(items)) return false;

						return items.some(childId => {
							if (!childId || !childId.id) return false;

							const childItem = this.tree.data.getItem(childId.id);
							if (!childItem) return false;

							if (childItem.tipo === "cartografia") return true;

							return childItem.items && childItem.items.length > 0 && checkItemsRecursively(childItem.items);
						});
					};

					// Verifica se o item tem filhos
					const hasChildren = item.items && Array.isArray(item.items) && item.items.length > 0;

					if (hasChildren) {
						const hasItemTypeDescendant = checkItemsRecursively(item.items);

						if (hasItemTypeDescendant) {
							dhx.alert({
								header: "Erro",
								text: "Não foi possível excluir o grupo cartografia. Possui filhos!",
								buttonsAlignment: "center",
								buttons: ["ok"],
							});
						} else {
							console.log("Nenhum item do tipo ITEM encontrado nos descendentes.");
							// Aqui você pode adicionar lógica para remover o item
							ondeleteWindow(objid);
						}
					} else {
						console.log("O item não tem filhos.");
						// Lógica para remover um item sem filhos
						ondeleteWindow(objid);
					}
				};

				// Chamada da função
				removeItem(objtreeid);

				break;
			case 'ordenacao':     
					  
				// Edita o ITEM de Cartografia
				// Chama Actions.CARTOGRAFIAITEM_EDITA

				var aux={ type:Actions.SHOW_CARTOGRAFIA_ORDENA, payload: ''};
			
				viewer.dispatch(aux);

				break;
			default:
				break;
			} 
			
			
		});
		console.log(' check all');
		const arvore = this.tree;
		this.tree.data.forEach(function(element, index, array) { 
			let aux_id = element.id; 
			arvore.uncheckItem(aux_id);
		});
		
		const SetLayersInicioMap = () => {
			console.log(' SET SetLayersInicioMap');
			var checkedElem = TreeCart.getChecked();
			let layers = this.tree.data.map(function(element) {
				if (checkedElem.includes(element.id)) {
					return element.layers.replace(/(\|{4})/g, '|');
				}
			  });
			// Remover valores undefined   
			layers = layers.filter(function(id) {
				return id !== undefined;
			});
			// Remover valores vazios   
			layers = layers.filter(function(id) {
				return id !== '';
			});
			
			let rec_ids = this.tree.data.map(function(element) {
				if (checkedElem.includes(element.id)) {
					if (element.tipo=='cartografia'){
						return element.rec_id;
					} 
				}
			});
			layers = layers.flatMap(str => str.split('|'));
			// Remover valores undefined   
			rec_ids = rec_ids.filter(function(id) {
				return id !== undefined;
			});
			// Remover valores vazios   
			rec_ids = rec_ids.filter(function(id) {
				return id !== '';
			});
			let estado_arvore=rec_ids.join('|');


			//this.props.dispatchLogin({payload: {authtoken: token, aplicacao_titulo:applicationname,  aplicacao_sigla:applicationsigla,  userid: aux_userid, username: aux_username, usersession: aux_usersession,  permissoes: aux_permissoes, sep_app: aux_separadores_app, sep_write: aux_separadores_escrita, sep_read: aux_separadores_leitura, separadoresnomes: aux_sep_nomes, separadorestooltips: aux_sep_tooltips,separadoresids: aux_sep_ids, separadoressiglas: aux_sep_sigla, separadoresdata: aux_final, treeconstrucoes_data: aux_treeconstrucoes_data,treeinstrumentos_data: aux_treeinstrumentos_data, treeatendimento_data: aux_treeatendimento_data, treeseparador_data: aux_treeseparador_data, treecartografia_data: aux_treecartografia_data} });
			//console.log('SetLayersInicioMap guardaEstadoArvore ' + estado_arvore);
			this.props.guardaEstadoArvore({payload: { dados: estado_arvore}});

			this.setState(prevState => { 
				//console.log('SetLayersInicioMap set State');	
				
				//console.log('Cart checkedLayers ' + prevState.checkedLayers);	
				var removedlayers = [];			 
				var addedlayers =[];
				try{ 

					var addedlayers = layers.filter(function(id) {
						return !prevState.checkedLayers.includes(id);
					});
				}catch(e){}
				try{ 
					var removedlayers = (prevState.checkedLayers || []).filter(function(id) {
						return !layers.includes(id);
					});
				}catch(e){}
				var new_checkedElem=[...checkedElem];
				var new_checkedLayers=[...layers];
				//console.log('Cart addedlayers ' + addedlayers);	
				//console.log('Cart removedlayers ' + removedlayers);
				//console.log('Cart checkedElem ' + new_checkedElem);
				//console.log('Cart checkedLayers ' + new_checkedLayers);				 

				return {
					addedlayers: addedlayers,
					removedlayers: removedlayers,
					checkedElem: [...checkedElem],
					checkedLayers:  [...layers]	
				};
			}, () => { 
				//console.log('SetLayersInicioMap fim State');
				
				var estado = viewer.getState();			
				var activeMapName = estado.config.activeMapName;
				var mapState = estado.mapState[activeMapName];
				var currentMap = mapState.mapguide.runtimeMap; 
				var sessionId  = currentMap.SessionId; 
				var mapaDef  = currentMap.MapDefinition;

			
				var addedlayers = this.state.addedlayers.join('|');
				var removedlayers = this.state.removedlayers.join('|');
				var jwtToken = authtoken;
				//console.log('Cart addedlayers ' + addedlayers);	
				//console.log('Cart removedlayers ' + removedlayers);
				this.props.guardaLayerAtivos({payload: { addedlayers: addedlayers, removedlayers: removedlayers}});
				let aux_layers_iniciais=this.props.layers_iniciais.join('|');  
				addedlayers = addedlayers +'|'+ aux_layers_iniciais;
				try { 
					const setCartografiaInicial = async () => {
						
						const url = apiEndpointSIG + 'mapalayers';//ok

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
							console.log(' SET SetLayersInicioMap FIM');
							var state = viewer.getState();
							const args = state.config;
							var NomeMapa = state.config.activeMapName;
							const uid = uuidv4();
							var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};
						
							viewer.dispatch(aux);
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
			});
		}

		const ReloadData = () => {
		 
			this.tree.data.removeAll();
			const url = apiEndpointSIG + 'ArvoreCartografia/' + userid;//ok
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
				let newdados=processData(jsonData);  
				this.tree.data.parse(newdados);	 
			})
			.catch(error => {
				console.error('There was a problem with your fetch operation:', error);
			});
			
		}
		const ondeleteWindow = (objid) => {
			const DeleteObjGeometry = (objid)=> {
				try {				
				const savePretensaoObj = async () => {                   
					const url = apiEndpoint + 'CartografiaGrupo';//ok     
					const jwtToken = authtoken;   
					const response = await fetch(url, {
					method: 'DELETE',
					body: JSON.stringify({ 
						id : objid
						}),
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`,
						},
					});              
					if (response.ok) {
						// mapa refresh
						// tree refresh      
						//ReloadData();               
						let aux_rec_id=objid; 
						let aux_id='';

						this.tree.data.forEach(function(element, index, array) { 
							if (element.rec_id==aux_rec_id){ 
								console.log("This is an item of treeCollection: ", element); 
								aux_id = element.id; 
							}
						});
						if (aux_id!=''){ 
							this.tree.data.remove(aux_id);  
						}

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
				header:"Remover Grupo Cartografia",
				text:"Tem a certeza que pretende remover o grupo cartografia?",
				buttons:["sim", "nao"],
				buttonsAlignment:"center"
			}).then(function(resposta){
				console.log('resposta ', resposta);
		
				switch(resposta){
				case false:
					DeleteObjGeometry(objid); 
					break;
				default:
					break;
				}
			});
		
		}
		// Colocar o estado inicial guardado
		if ((this.props.treecartografia_init_checked!=[])&&(this.props.treecartografia_init_checked!='')){ 
			try {
				console.log(' SET treecartografia_init_checked');
				const escala = parseInt(this.props.mapa_initialview_escala);
				const centro_x = parseFloat(this.props.mapa_initialview_x);
				const centro_y = parseFloat(this.props.mapa_initialview_y);
				var estado = viewer.getState();			
				var activeMapName = estado.config.activeMapName;
				
			}catch(e){}
			 
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
			}, 1000);  // 2000 milissegundos = 2 segundos
			try {
				let fez_um_check = false;
				let aux_elementosligados=this.props.treecartografia_init_checked;  
				let aux_layers_iniciais=this.props.layers_iniciais;  

				
				const arvore = this.tree;
				this.tree.data.forEach(function(element, index, array) { 
					//console.log(' element.rec_id :' + element.rec_id);
					for(let i = 0; i < aux_elementosligados.length; i++) {
						let itemid = aux_elementosligados[i];
						//console.log('   itemid :' + itemid);
						
						if (element.rec_id==itemid){
							let aux_id = element.id;  
							//console.log('   aux_id :' + aux_id);
							//isChecks.push(aux_id);
							arvore.checkItem(aux_id); 
							fez_um_check = true;
						} 
					}
				}); 
				
				if (fez_um_check){
					SetLayersInicioMap();
				} 
			}catch(e){}
		}

		//Guardar todos os layers da árvore
		let layersarvore = this.tree.data.map(function(element) { 
			return element.layers.replace(/(\|{4})/g, '|'); 
		});
		// Remover valores undefined   
		layersarvore = layersarvore.filter(function(id) {
			return id !== undefined;
		});
		// Remover valores vazios   
		layersarvore = layersarvore.filter(function(id) {
			return id !== '';
		}); 
		layersarvore = layersarvore.flatMap(str => str.split('|'));

		this.props.guardaLayersArvore({payload: { dados: layersarvore}});

		
	}
	componentDidUpdate(prevProps, prevState) { 
		
		if (this.props.update_tipo!="") { 
			if (this.props.update_tipo=="insert") {
				let aux_parent_recid=this.props.update_parent_recid;
				let aux_rec_id=this.props.update_rec_id;
				let aux_nome=this.props.update_nome;
				let aux_layers=this.props.update_layers;
				let aux_legenda=this.props.update_legenda;
				let aux_objtipo=this.props.update_objtipo;
				let aux_id='';
				//if (aux_parent_recid!=''){
					this.tree.data.forEach(function(element, index, array) { 
						if (element.rec_id==aux_parent_recid){
							//console.log("Encontrei");
							//console.log("This is an item of treeCollection: ", element.rec_id);
							//console.log("This is an item of treeCollection: ", element.parentid);
							//console.log("This is an item of treeCollection: ", element.value);
							//console.log("This is an index of the element: ", index);
							//console.log("This is an array of the elements: ", array);
							aux_id = element.id; 
						}
					});
					if (aux_id!=''){
						//console.log('aui');
						//console.log(aux_id);
						//console.log(aux_nome);
						this.tree.data.add({ rec_id: aux_rec_id, parentid: aux_parent_recid, value:aux_nome, layers:aux_layers, tipo:aux_objtipo },-1, aux_id);
						this.props.fezAtualizacao();
					}
				//} else {
					 
				//	this.tree.data.add({ rec_id: aux_rec_id, parentid: aux_parent_recid, value:aux_nome, layers:aux_layers, legenda:aux_legenda, tipo:aux_objtipo}, 0);
				//	this.props.fezAtualizacao();
				//}
				
			}
			if (this.props.update_tipo=='updatecheck') {
				const TreeCartografia = this.tree;
				//colocar tudo desligado
				this.tree.data.forEach(function(element, index, array) { 					 
					TreeCartografia.unCheckItem(element.id);					 
				}); 
				//vamos ligar
				let iditensarvore=this.props.treecartografia_setcheck;
				//console.log(iditensarvore);
				//const iditensarvore=aux_rec_id_checkitem.split("{||}");
				let aux_id='';
				for(let i = 0; i < iditensarvore.length; i++) {
					let itemid = iditensarvore[i];
					  
					this.tree.data.forEach(function(element, index, array) { 
						if (element.rec_id==itemid){
							aux_id = element.id; 
							TreeCartografia.checkItem(aux_id);
						}
					}); 
				}
				
				var checkedElem = TreeCartografia.getChecked();
				this.setState(prevState => ({
					checkedElem:  [...checkedElem]					 
				}));
				this.props.fezDefinicoesIniciais();
				const SetLayersMapUpdateCheck = () => {
					this.props.fezDefinicoesIniciais();
					var TreeCartografia = this.tree;
					var checkedElem = TreeCartografia.getChecked();
					console.log(checkedElem);
					let layers = this.tree.data.map(function(element) {
						if (checkedElem.includes(element.id)) {
							return element.layers.replace(/(\|{4})/g, '|');
						}
					  });
					// Remover valores undefined   
					layers = layers.filter(function(id) {
						return id !== undefined;
					});
					// Remover valores vazios   
					layers = layers.filter(function(id) {
						return id !== '';
					});
					
					let rec_ids = this.tree.data.map(function(element) {
						if (checkedElem.includes(element.id)) {
							if (element.tipo=='cartografia'){
								return element.rec_id;
							} 
						}
					});
					layers = layers.flatMap(str => str.split('|'));
					// Remover valores undefined   
					rec_ids = rec_ids.filter(function(id) {
						return id !== undefined;
					});
					// Remover valores vazios   
					rec_ids = rec_ids.filter(function(id) {
						return id !== '';
					});
					let estado_arvore=rec_ids.join('|');
					//console.log('Cart guardaEstadoArvore ' + estado_arvore);
					this.props.guardaEstadoArvore({payload: { dados: estado_arvore}});
	
		
					this.setState(prevState => { 	
						//console.log('SetLayersMapUpdateCheck set State');			  
						var addedlayers = layers.filter(function(id) {
							return !prevState.checkedLayers.includes(id);
						});
						var removedlayers  = prevState.checkedLayers.filter(function(id) {
							return !layers.includes(id);
						}); 
						var new_checkedElem=[...checkedElem];
						var new_checkedLayers=[...layers];
						console.log(' ------ UPDATE CHECK ------ ');
						console.log('Cart addedlayers ' + addedlayers);	
						console.log('Cart removedlayers ' + removedlayers);
						//console.log('Cart checkedElem ' + new_checkedElem);
						console.log('Cart checkedLayers ' + new_checkedLayers);		
						console.log(' ------ UPDATE CHECK ------ '); 					  
						return {
							addedlayers: addedlayers,
							removedlayers: removedlayers, 
							checkedElem: [...checkedElem],
							checkedLayers:  [...layers]
						};
					}, () => { 
						//console.log('SetLayersMapUpdateCheck fim set State');
						var addedlayers_array = this.state.checkedLayers || [];
					 	var removedlayers_array = this.state.removedlayers || [];
						this.props.guardaLayerAtivos({payload: { addedlayers: addedlayers_array, removedlayers: removedlayers_array}});

						/*var estado = viewer.getState();			
						var activeMapName = estado.config.activeMapName;
						var mapState = estado.mapState[activeMapName];
						var currentMap = mapState.mapguide.runtimeMap; 
						var sessionId  = currentMap.SessionId; 
						var mapaDef  = currentMap.MapDefinition;
		
						var addedlayers = this.state.addedlayers.join('|');
						var removedlayers = this.state.removedlayers.join('|');
						var jwtToken = this.props.authtoken;
		
						try { 
							const setCartografiaUpdateCheck = async () => {
								
								const url = this.props.apiEndpoint + 'mapalayers';//ok
		
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
									//toggleItemCheck(this.tree, id);
									
									this.props.fezDefinicoesIniciais();
		
								} else {
									const errorMessage = await response.text();
									this.setState({ errorMessage: errorMessage });
								}
							};
							setCartografiaUpdateCheck();
						} catch (error) {
							console.error('error: ' + error);
							this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Instrumentos.' });
						}*/
					});
				}
				SetLayersMapUpdateCheck();
			}
			if (this.props.update_tipo=="resetcheck") {

				
				const TreeCartografia = this.tree;
				//colocar tudo desligado
				this.tree.data.forEach(function(element, index, array) { 					 
					TreeCartografia.unCheckItem(element.id);					 
				}); 
				 
				let layers_to_turn_off=this.props.reset_layers;
  				this.props.fezresetCartografia();
		
		
				this.setState(prevState => { 	
					//console.log('SetLayersMapResetCheck set State');			  
					var addedlayers = '';
					var removedlayers  = layers_to_turn_off; 
									
					return {
						addedlayers: addedlayers,
						removedlayers: removedlayers, 
						checkedElem: [],
						checkedLayers:  []
					};
				}, () => { 
					//console.log('SetLayersMapResetCheck fim set State');
					var estado = viewer.getState();			
					var activeMapName = estado.config.activeMapName;
					var mapState = estado.mapState[activeMapName];
					var currentMap = mapState.mapguide.runtimeMap; 
					var sessionId  = currentMap.SessionId; 
					var mapaDef  = currentMap.MapDefinition;
		
					var addedlayers = '';
					var removedlayers = this.state.removedlayers.join('|');
					var jwtToken = this.props.authtoken;
		
					try { 
						const setMapaResetCheck = async () => {
							
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
								//toggleItemCheck(this.tree, id);
								
								this.props.fezresetCartografia();
		
							} else {
								const errorMessage = await response.text();
								console.error('errorMessage: ' + errorMessage);
								this.setState({ errorMessage: errorMessage });
							}
						};
						setMapaResetCheck();
					} catch (error) {
						console.error('error: ' + error);
						this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Instrumentos.' });
					}
				});
				 
			}
			
			if (this.props.update_tipo=="update") {
				let aux_parent_recid=this.props.update_parent_recid;
				let aux_rec_id=this.props.update_rec_id;
				let aux_nome=this.props.update_nome;
				let aux_layers=this.props.update_layers;
				let aux_legenda=this.props.update_legenda;
				let aux_objtipo=this.props.update_objtipo;
				let aux_id='';
				this.tree.data.forEach(function(element, index, array) { 
					if (element.rec_id==aux_rec_id){
						//console.log("Encontrei");
						//console.log("This is an item of treeCollection: ", element);
						//console.log("This is an item of treeCollection: ", element.parentid);
						//console.log("This is an item of treeCollection: ", element.value);
						//console.log("This is an index of the element: ", index);
						//console.log("This is an array of the elements: ", array);
						aux_id = element.id; 
					}
				});
				if (aux_id!=''){
					//console.log('aui');
					//console.log(aux_id);
					//console.log(aux_nome);
					this.tree.data.update(aux_id, { rec_id: aux_rec_id, parentid: aux_parent_recid, value:aux_nome, layers:aux_layers, legenda:aux_legenda, tipo:aux_objtipo } );
					
					//this.tree.data.update(aux_id,{ value:aux_nome });
					this.props.fezAtualizacao();
				}
			}
			if (this.props.update_tipo=="delete") {

				let aux_rec_id=this.props.update_rec_id;
				let aux_id='';
				this.tree.data.forEach(function(element, index, array) { 
					if (element.rec_id==aux_rec_id){
						//console.log("Encontrei"); 
						aux_id = element.id; 
					}
				});
				if (aux_id!=''){
					this.tree.data.remove(aux_id);
					this.props.fezAtualizacao();
				}
			}

			if (this.props.update_tipo=="reloadmap") {
				if ((this.props.treecartografia_init_checked!=[])&&(this.props.treecartografia_init_checked!='')){ 
					const arvore = this.tree;
					let fez_um_check = false;
						 
					let aux_elementosligados=this.props.treecartografia_init_checked;
					this.tree.data.forEach((element) => { 
						if (!aux_elementosligados.includes(element.rec_id)) {
							let aux_id = element.id; 
							if(element.tipo=='cartografia'){ 
								arvore.unCheckItem(aux_id); 
							} 
						} else {
							let aux_id = element.id;   
							if(element.tipo=='cartografia'){ 
								arvore.checkItem(aux_id);
								fez_um_check = true;  // Aqui o valor de fez_um_check será atualizado corretamente
							}
						}
						 
					}); 
					const SetLayersInicioMap = () => {
						const TreeCart = this.tree;
						var checkedElem = TreeCart.getChecked();
						let layers = this.tree.data.map(function(element) {
							if (checkedElem.includes(element.id)) {
								return element.layers.replace(/(\|{4})/g, '|');
							}
						  });
						// Remover valores undefined   
						layers = layers.filter(function(id) {
							return id !== undefined;
						});
						// Remover valores vazios   
						layers = layers.filter(function(id) {
							return id !== '';
						});
						
						let rec_ids = this.tree.data.map(function(element) {
							if (checkedElem.includes(element.id)) {
								if (element.tipo=='cartografia'){
									return element.rec_id;
								} 
							}
						});
						layers = layers.flatMap(str => str.split('|'));
						// Remover valores undefined   
						rec_ids = rec_ids.filter(function(id) {
							return id !== undefined;
						});
						// Remover valores vazios   
						rec_ids = rec_ids.filter(function(id) {
							return id !== '';
						});
						let estado_arvore=rec_ids.join('|');
			
			
						//this.props.dispatchLogin({payload: {authtoken: token, aplicacao_titulo:applicationname,  aplicacao_sigla:applicationsigla,  userid: aux_userid, username: aux_username, usersession: aux_usersession,  permissoes: aux_permissoes, sep_app: aux_separadores_app, sep_write: aux_separadores_escrita, sep_read: aux_separadores_leitura, separadoresnomes: aux_sep_nomes, separadorestooltips: aux_sep_tooltips,separadoresids: aux_sep_ids, separadoressiglas: aux_sep_sigla, separadoresdata: aux_final, treeconstrucoes_data: aux_treeconstrucoes_data,treeinstrumentos_data: aux_treeinstrumentos_data, treeatendimento_data: aux_treeatendimento_data, treeseparador_data: aux_treeseparador_data, treecartografia_data: aux_treecartografia_data} });
						//console.log('SetLayersInicioMap guardaEstadoArvore ' + estado_arvore);
						this.props.guardaEstadoArvore({payload: { dados: estado_arvore}});
			
						this.setState(prevState => { 
							//console.log('SetLayersInicioMap set State');	
							
							console.log('Cart checkedLayers ' + prevState.checkedLayers);	
							var removedlayers = [];			 
							var addedlayers =[];
							try{ 
			
								var addedlayers = layers.filter(function(id) {
									return !prevState.checkedLayers.includes(id);
								});
							}catch(e){}
							try{ 
								var removedlayers = (prevState.checkedLayers || []).filter(function(id) {
									return !layers.includes(id);
								});
							}catch(e){}
							var new_checkedElem=[...checkedElem];
							var new_checkedLayers=[...layers];
							console.log('Cart addedlayers ' + addedlayers);	
							console.log('Cart removedlayers ' + removedlayers);
							console.log('Cart checkedElem ' + new_checkedElem);
							console.log('Cart checkedLayers ' + new_checkedLayers);				 
			
							return {
								addedlayers: addedlayers,
								removedlayers: removedlayers,
								checkedElem: [...checkedElem],
								checkedLayers:  [...layers]	
							};
						}, () => { 
							//console.log('SetLayersInicioMap fim State');
							
							var estado = viewer.getState();			
							var activeMapName = estado.config.activeMapName;
							var mapState = estado.mapState[activeMapName];
							var currentMap = mapState.mapguide.runtimeMap; 
							var sessionId  = currentMap.SessionId; 
							var mapaDef  = currentMap.MapDefinition;
			
						
							var addedlayers = this.state.addedlayers.join('|');
							var removedlayers = this.state.removedlayers.join('|');
							var jwtToken = this.props.authtoken;
							//console.log('Cart addedlayers ' + addedlayers);	
							//console.log('Cart removedlayers ' + removedlayers);
							this.props.guardaLayerAtivos({payload: { addedlayers: addedlayers, removedlayers: removedlayers}});
							let aux_layers_iniciais=this.props.layers_iniciais.join('|');  
							addedlayers = addedlayers +'|'+ aux_layers_iniciais;
							try { 
								/*const setCartografiaInicial = async () => {
									
									const url = this.props.apiEndpoint + 'mapalayers';//ok
									 
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
										//toggleItemCheck(this.tree, id);
			
									} else {
										const errorMessage = await response.text();
										console.error('error: ' + errorMessage);
										this.setState({ errorMessage: errorMessage });
									}
								};
								setCartografiaInicial();*/
							} catch (error) {
								console.error('error: ' + error);
								this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Instrumentos.' });
							}
						});
					}
					SetLayersInicioMap();
					 
				}else {
					const arvore = this.tree;
					this.tree.data.forEach(function(element, index, array) {
						let aux_id = element.id;   
						if(element.tipo=='cartografia'){
							arvore.unCheckItem(aux_id); 
						} 
					});
				}
				 
				this.props.fezAtualizacao();
				
			}
			
			if(this.props.update_tipo=="reloadtree"){
					console.log('Recarregando árvore...');
					this.reloadTree();

					this.props.fezAtualizacao();
			}
		} 
		
	}
	reloadTree() {
		const TreeCart = this.tree;
		TreeCart.data.removeAll();
		const url = `${this.props.apiEndpointSIG}ArvoreCartografia/${this.props.userid}`;
		const jwtToken = this.props.authtoken;
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
		  TreeCart.data.parse(jsonData);
		})
		.catch(error => {
		  console.error('There was a problem with your fetch operation:', error);
		});
	  }
	componentWillUnmount() {
		console.log('componentWillUnmount - treeCartografia_G');
		this.tree && this.tree.destructor();
	}
	render() {
		return (
			<div
				style={{ padding: 10,  overflow: "auto" }}
				ref={el => (this.el = el)}
			></div>
		);
	}
}
/*

function getCheckedItems ()  {
	const checkedElem = TreeC.getChecked();
	if (checkedElem != '') {
	  return checkedElem;
	}
	return [];
};
*/

//Mapear o estado para as propriedades do objeto
function mapStateToProps(state){
	/*
		 
		var FCart_NovoItem = 1510; 
		var FCart_EditarItem = 1520;
		var FCart_EditarOrdem = 1530;*/
		const listaDeCodigosPermissoes = Perm.listaDeCodigosCartografia;
		const funcionalidade = state.aplicacaopcc.funcionalidades;
				
		// Filter permissions that have cod in listaDeCodigosPermissoes
		const funcionalidadesRelevantes = funcionalidade.filter(permission => {
			// Check if cod is in listaDeCodigosInteressantes
			return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
		});
		 
		const permissaoNovoItem = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FCart_NovoItem, ''); 
		const permissaoEditarItem = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FCart_EditarItem, '');
		const permissaoEditarOrdem = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FCart_EditarOrdem, '');
	 

	return {
		arv : state.message,  
		update_tipo: state.aplicacaopcc.treecartografia.treecartografia_tipo, 
		update_rec_id: state.aplicacaopcc.treecartografia.treecartografia_rec_id,
		update_nome:  state.aplicacaopcc.treecartografia.treecartografia_nome,
		update_parent_recid:  state.aplicacaopcc.treecartografia.treecartografia_parent_recid,
		update_layers: state.aplicacaopcc.treecartografia.treecartografia_layers,
		update_legenda:  state.aplicacaopcc.treecartografia.treecartografia_legenda,
		update_objtipo:  state.aplicacaopcc.treecartografia.treecartografia_objtipo,
		reloadKey: state.aplicacaopcc.treeatendimento.reloadKey,

		treeinstrumentos_itenschecked: state.aplicacaopcc.treeinstrumentos.treeinstrumentos_itenschecked,
		treecartografia_init_checked: state.aplicacaopcc.geral.treecartografia_init,

		layers_iniciais: state.aplicacaopcc.geral.layers_iniciais,

		mapa_initialview_x: state.aplicacaopcc.geral.mapa_initialview_x ,
		mapa_initialview_y: state.aplicacaopcc.geral.mapa_initialview_y , 
		mapa_initialview_escala: state.aplicacaopcc.geral.mapa_initialview_escala ,

		treecartografia_setcheck: state.aplicacaopcc.treecartografia.treecartografia_setcheck,
		treecartografia_itenschecked: state.aplicacaopcc.treecartografia.treecartografia_itenschecked,

		//para o desligar dos layers 
		reset_layers: state.aplicacaopcc.reset_layers,
		
		aplicationTokenId: state.aplicacaopcc.config.aplicationTokenId,
		apiEndpoint: state.aplicacaopcc.config.configapiEndpoint,
		apiEndpointSIG: state.aplicacaopcc.config.configapiEndpointSIG,
		authtoken: state.aplicacaopcc.geral.authtoken, 
		userid: state.aplicacaopcc.geral.userid,
		usersession: state.aplicacaopcc.geral.usersession,	 

		//permissaoNovoGrupoPrincipal: permissaoNovoGrupoPrincipal,
		//permissaoNovoGrupo: permissaoNovoGrupo,
		permissaoNovoItem: permissaoNovoItem,
		//permissaoEditarGrupo: permissaoEditarGrupo,
		permissaoEditarItem: permissaoEditarItem,
		permissaoEditarOrdem: permissaoEditarOrdem		 
	};
}
//Envia nova propriedades para o estado
function mapDispatchToProps(dispatch){

	return {
		updateTree: (item) => dispatch({
			type: Actions.TREEGUCARTOGRAFIA_UPDATE, payload: item.datatree
		}),
		fezAtualizacao: () => dispatch({
			type: Actions.TREEGUCARTOGRAFIA_UPDATEFIM, payload: ''
		}),
		fezDefinicoesIniciais: () => dispatch({
			type: Actions.TREEGUCARTOGRAFIA_UPDATECHECKFIM, payload: ''
		}),
		guardaEstadoArvore: (item) => dispatch({
			type: Actions.TREEGUCARTOGRAFIA_STORECHECKED, payload: item.payload
		}),
		guardaLayersArvore: (item) => dispatch({
			type: Actions.STORE_LAYERS, payload: item.payload
		}),
		guardaLayerAtivos:(item) => dispatch({
			type: Actions.STORE_LAYERS_LIGADOS, payload: item.payload
		}),
		resetCartografia: () => dispatch({
			type: Actions.STORE_LAYERS_RESET, payload: ''
		}),
		fezresetCartografia: () => dispatch({
			type: Actions.STORE_LAYERS_RESET_FIM, payload: ''
		}), 
		
	};
}


export default connect(mapStateToProps,mapDispatchToProps)(TreeC);
//export default TreeInstrumentos;
