//
import React, { useState, useEffect, useRef ,Component, PureComponent } from "react";
import * as Perm from "../../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 

import PropTypes from "prop-types";
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import "dhx-suite/codebase/suite.min.css";
import "dhx-suite/codebase/suite.min.js";
import * as Actions from "../../../constants/actions";
import '../../../../css/pcc.css';
import { connect } from "react-redux";

import { Client } from "mapguide-react-layout/lib/api/client";
import { refresh } from 'mapguide-react-layout/lib/actions/legend';
import { RuntimeMapFeatureFlags } from "mapguide-react-layout/lib/api/request-builder";
import { ActionType } from 'mapguide-react-layout/lib/constants/actions';
import { v4 as uuidv4 } from 'uuid';
import { processData } from '../../../utils/utils';

class TreeCEM_G extends Component {
	constructor(props) {
		super(props);
		this.state = {
			checkedIds: [], // isto tem os rec_ids
			selectedId:'',
			checkedElem: [], // isto tem os ids da arvore
		};
		this.tree = React.createRef();
		this.janela = React.createRef();
		this.tabRef = React.createRef();
		this.FormDadosEnt = React.createRef();
		this.Combobox = React.createRef();
		
	}

	componentDidMount() {


		const { css, data, keyNavigation, checkbox, apiEndpoint, apiEndpointSIG, authtoken, userid, usersession, 
			permissaoNovoItem, 
			localiza_arvore, rec_id_arvore, sigla_separador,
			permissaoEditarItem, permissaoEditarOrdem, permissaoObterSHPP} = this.props;
		this.tree = new TreeDHX(this.el, {
			css: css,
			keyNavigation: keyNavigation,
			checkbox: true,
			data: data,
		});
		
		
		this.tree.events.on("itemClick", id => {			 

			const recid = this.tree.data.getItem(id)?.recid;
			const tipo = this.tree.data.getItem(id)?.tipo;
			this.setState(prevState => ({
				selectedId:  recid 				 
			}));
			let permissaoEscrita = Perm.findPermissionByCodAndSubCod(this.props.funcionalidadesRelevantes, Perm.FGU_AcessoEscritaArvoreConstrucoes, 'G:'+recid);
			console.log(permissaoEscrita);
			if (tipo=='talhao'){
				this.props.carregaGridCEM({grupo_recid: recid}); 
			}
			
		});
		let objtreeid;
		let objid;
		let objidparent;
		let objname;
		let objtipo;
		this.tree.events.on("itemRightClick", (id,e) => {
			objtreeid = id;
			console.log("The item with the id "+ id +" was right-clicked.");
			console.log(this.tree.data?.getItem(id));
			objid = this.tree.data?.getItem(id)?.recid;
			objname = this.tree.data?.getItem(id)?.value;
			objidparent = this.tree.data?.getItem(id)?.parentid;
			objtipo = this.tree.data?.getItem(id)?.tipo;
			if ((objtipo=='pai')){
				e.preventDefault();
				cmenuparent.showAt(e,id);

			} else {
				if ((objtipo=='cemiterio')){
					e.preventDefault();
					cmenucemiterio.showAt(e,id);
				} else {
					if ((objtipo=='talhao')){
						e.preventDefault();
						cmenutalhao.showAt(e,id);
					} else {

					}
				}
			}
			
			/*
			objid= recid1;
			objname= recid2;
			objidparent= recid3;
			let permissaoEscrita = Perm.findPermissionByCodAndSubCod(this.props.funcionalidadesRelevantes, Perm.FGU_AcessoEscritaArvoreConstrucoes, 'G:'+objid);
			console.log(permissaoEscrita);
			if (permissaoNovoGrupo && permissaoEscrita) { 
				cmenu.enable("new_grupo");
				cmenu.enable("ordenacao"); 
				//console.log(" cmenu.enable new_grupo");
			} else {
				cmenu.disable("new_grupo");
				cmenu.disable("ordenacao"); 
				//console.log(" cmenu.disable new_grupo");
			}
			
			
			if (permissaoObterSHPP) {
				cmenu.enable("exportar");
				//console.log(" cmenu.enable edit_grupo ");
				//console.log(" cmenu.enable remover ");
			 }else {
				cmenu.disable("exportar");
				//console.log(" cmenu.disable edit_grupo ");
				//console.log(" cmenu.disable remover ");
			}

			if (permissaoEditarGrupo && permissaoEscrita) {
				cmenu.enable("edit_grupo");
				cmenu.enable("remover"); 
				//console.log(" cmenu.enable edit_grupo ");
				//console.log(" cmenu.enable remover ");
			 }else {
				cmenu.disable("edit_grupo");
				cmenu.disable("remover"); 
				//console.log(" cmenu.disable edit_grupo ");
				//console.log(" cmenu.disable remover ");
			}
		 
			if (permissaoNovoItem  && permissaoEscrita) { 
				cmenu.enable("new_item"); 
				//console.log(" cmenu.enable new_item ");
			}else {
				cmenu.disable("new_item"); 
				//console.log(" cmenu.disable new_item ");
			}
			
			if (permissaoEditarGrupo && permissaoEscrita) {
				cmenu.enable("remover"); 
				//console.log(" cmenu.enable remover ");
			} else {
				cmenu.disable("remover");
				//console.log(" cmenu.disable remover "); 
			}
			if (permissaoEditarGrupo && permissaoEscrita) { 
				cmenu.enable("exportar"); 
				//console.log(" cmenu.enable exportar ");
			} else {
				cmenu.disable("exportar");
				//console.log(" cmenu.disable exportar "); 
			}  
			if (id=='parent'){
				cmenu.disable("edit_grupo");
				cmenu.disable("new_item");
				cmenu.disable("remover");
			}
			console.log(objid);*/

			
			//return false;
		});
		function ProcessaCheck(id, contexto){
			/*let isChecked = isChecks.includes(id);
			
			 
			if (isChecked) {
				contexto.tree.unCheckItem(id);
				isChecks.pop(id);
			} else {
				contexto.tree.checkItem(id);
				isChecks.push(id);
			}*/
			
			SetLayersMap(id);
		}
		
		const cmenuparent = new dhx.ContextMenu(null, {
			css: "dhx_widget--bg_gray",
			data: [ 
			  { id: "new_cemiterio", value: "Novo Cemitério" },  	
			  { type: "separator" } ,
			  { id: "ligarlayers",value: "Ligar Layers" } ,
			  { id: "desligarlayers",value: "Desligar Layers" } ,
			  { type: "separator" } ,
			  { id: "expandir",value: "Expandir" } ,
			  { id: "contrair",value: "Contrair" } ,
			]
		});
 		const cmenucemiterio = new dhx.ContextMenu(null, {
			css: "dhx_widget--bg_gray",
			data: [  
			  { id: "edit_cemiterio", value: "Editar Cemitério" },	  
			  { type: "separator" },
			  { id: "remover_cemiterio", value: "Remover Cemitério" },	  
			  { type: "separator" },
			  { id: "new_talhao", value: "Novo Talhão" }, 	  	
			  { type: "separator" } ,
			  { id: "ligarlayers",value: "Ligar Layers" } ,
			  { id: "desligarlayers",value: "Desligar Layers" } ,
			  { type: "separator" } ,
			  { id: "expandir",value: "Expandir" } ,
			  { id: "contrair",value: "Contrair" } ,
			]
		});
		const cmenutalhao = new dhx.ContextMenu(null, {
			css: "dhx_widget--bg_gray",
			data: [  
			  { id: "edit_talhao", value: "Editar Talhão" },	  
			  { type: "separator" },
			   { id: "remover_talhao", value: "Remover Talhão" },	  
			  { type: "separator" },
			  { id: "new_construcao", value: "Nova Construção" }, 	  	
			  { type: "separator" } ,
			  { id: "ligarlayers",value: "Ligar Layers" } ,
			  { id: "desligarlayers",value: "Desligar Layers" } ,
			  { type: "separator" } ,
			  { id: "expandir",value: "Expandir" } ,
			  { id: "contrair",value: "Contrair" } ,
			]
		});
		const cmenu = new dhx.ContextMenu(null, {
			css: "dhx_widget--bg_gray",
			data: [ 
			  { id: "new_cemiterio", value: "Novo Cemitério" }, 
			  { type: "separator" } ,
			  { id: "edit_cemiterio", value: "Editar Cemitério" }, 
			  { type: "separator" } ,
			  { id: "new_item", value: "Eliminar Cemitério" }, 
			  { type: "separator" } ,
			  
			  //{ id: "reload", value: "Reload" }, 
			  //{ type: "separator" } ,
			  { id: "remover",value: "Eliminar Grupo" } ,
			  { type: "separator" } ,
			  { id: "ordenacao",value: "Editar Ordenação" } ,
			  { type: "separator" } ,
			  { id: "exportar", value: "Exportar Shapefile" }, 
			  { type: "separator" } ,
			  { id: "ligarlayers",value: "Ligar Layers" } ,
			  { id: "desligarlayers",value: "Desligar Layers" } ,
			  { type: "separator" } ,
			  { id: "expandir",value: "Expandir" } ,
			  { id: "contrair",value: "Contrair" } ,
			]
		});
		cmenuparent.events.on("click", (id, e) => {
			switch(id){ 
				case 'ligarlayers':
					{
						this.tree.checkItem(objtreeid);
						ProcessaCheck(objtreeid, this); 
						break;
					}
				case 'desligarlayers':
					{ 
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
				case 'new_cemiterio':   
					var aux={ type:Actions.CEMITERIO_NOVO, payload: 'principal'}; 
					viewer.dispatch(aux); 
					break; 
				default:
					break;
			} 
			 
		});
		cmenucemiterio.events.on("click", (id, e) => {
			console.log(objid);
			switch(id){ 
				case 'ligarlayers':
					{
						this.tree.checkItem(objtreeid);
						ProcessaCheck(objtreeid, this); 
						break;
					}
				case 'desligarlayers':
					{ 
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
				case 'edit_cemiterio': 
					{
						var aux={ type:Actions.CEMITERIO_EDITA, payload: objid}; 
						viewer.dispatch(aux);
						break; 
					}
				case 'new_talhao':   
					{
						var aux={ type:Actions.TALHAO_NOVO, payload: objid}; 
						viewer.dispatch(aux); 
						break; 
					}
				case 'remover_cemiterio':    
					if (objid!=''){ 
						ondeleteCemiterio(objid);  
					}
					break;
				default:
					break;
			} 
			 
		});
		cmenutalhao.events.on("click", (id, e) => {
			switch(id){ 
				case 'ligarlayers':
					{
						this.tree.checkItem(objtreeid);
						ProcessaCheck(objtreeid, this); 
						break;
					}
				case 'desligarlayers':
					{ 
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
				case 'edit_talhao': 
					{
						var aux={ type:Actions.TALHAO_EDITA, payload: objid}; 
						viewer.dispatch(aux);
						break; 
					}
			    case 'remover_talhao':    
					if (objid!=''){ 
						ondeleteTalhao(objid);  
					}
					break;
				case 'new_construcao':   
					{
						var aux={ type:Actions.CONSTRUCAO_NOVO, payload: objid}; 
						viewer.dispatch(aux); 
						break; 
					}
				default:
					break;
			} 
			 
		});
		cmenu.events.on("click", (id, e) => {
			 

			const onGetShapeFile = async (tipo, parent_id) => {
		 
				const url = apiEndpoint + 'ConstrucaoShapefile?tipo=' + tipo + '&id=' + parent_id;

				const jwtToken = authtoken;     
				const response = await fetch(url, {
				method: 'GET',  
				headers: {
					'Content-Type': 'application/json',
					'Authorization': `Bearer ${jwtToken}`
				},
				}); 
				if (response.ok) {
					
				// Read the response as a blob
				const fileBlob = await response.blob();
	
				// Create a URL for the blob data
				const fileUrl = URL.createObjectURL(fileBlob, { type: 'application/zip' });
	
				// Open the file in a new window or tab
				//window.open(fileUrl);
		
				// Create an anchor element
				const downloadLink = document.createElement('a');
	
				// Generate the file name with date and time
				const currentDate = new Date();
				const year = currentDate.getFullYear();
				const month = (currentDate.getMonth() + 1).toString().padStart(2, '0'); // Month is 0-indexed
				const day = currentDate.getDate().toString().padStart(2, '0');
				const hour = currentDate.getHours().toString().padStart(2, '0');
				const minute = currentDate.getMinutes().toString().padStart(2, '0');
				const second = currentDate.getSeconds().toString().padStart(2, '0');
				
				const formattedDateTime = `${year}${month}${day}${hour}${minute}${second}`; 
		
				const fileName = `Construcoes_${formattedDateTime}.zip`;
	
				
				// Set the download link properties
				downloadLink.href = fileUrl;
				downloadLink.download = fileName;
				downloadLink.target = '_blank'; // Open the file in a new window or tab
	
				// Trigger the download
				downloadLink.click();
				} else {
				console.error('Error:', response.status, response.statusText); 
				}
				
				 
			}
			console.log(id);
			console.log(objid);
			switch(id){
				case 'reload':  
					ReloadData(); 
					break; 
				case 'ligarlayers':
					{
						this.tree.checkItem(objtreeid);
						ProcessaCheck(objtreeid, this);
							
						break;
					}
				case 'desligarlayers':
					{
						
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
				case 'new_grupo':   
					if (objid==''){
						var aux={ type:Actions.CEMITERIO_NOVO, payload: 'principal'}; 
						viewer.dispatch(aux);
					} else {
						var aux={ type:Actions.CEMITERIO_NOVO, payload: objid}; 
						viewer.dispatch(aux);
					} 
					break;
				case 'new_item':   
					if (objid!=''){
						var aux={ type:Actions.PRETENSAO_NOVO, payload: objid}; 
						viewer.dispatch(aux);
					}
					break;
				case 'edit_grupo':     
					if (objid!=''){

						var aux={ type:Actions.CEMITERIO_EDITA, payload: objid}; 
						viewer.dispatch(aux);
					}
					break;
				case 'remover':    
					if (objid!=''){ 
						ondeleteWindow(objid);  
					}
					break;
				case 'ordenacao':     
					  
					// Edita o ITEM de Cartografia
					// Chama Actions.CARTOGRAFIAITEM_EDITA

					var aux={ type:Actions.SHOW_PRETENSAO_ORDENA, payload: ''};
				
					viewer.dispatch(aux);

				break;
				case 'exportar': 

					if (objid==''){
						onGetShapeFile('grupopretensao','parent'); 
					} else {
						
						onGetShapeFile('grupopretensao',objid);
					}
					break;
				default:
					break;
			} 
			 
		});
		const arvore = this.tree;
		this.tree.data.forEach(function(element, index, array) { 
			let aux_id = element.id; 
			arvore.uncheckItem(aux_id);
		});
		var isChecks=[];
		this.tree.events.on("afterCheck",  (index, id, value) =>{ 
			/*let isChecks= this.state.checkedElem;
			
			let isChecked = isChecks.includes(id);
	 
			if (isChecked) {
				this.tree.unCheckItem(id);
				isChecks.pop(id);
			} else {
				this.tree.checkItem(id);
				isChecks.push(id);
			}*/
			
			SetLayersMap(id);
		});

		const ReloadData = () => {
		 
			this.tree.data.removeAll();
			const url = apiEndpoint + 'ArvConstrucoes/' + userid;//ok
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
		const ondeleteCemiterio = (objid) => {
			const DeleteCemiterio= (objid)=> {
				try {				
				const DeleteCemiterioAsync = async () => {  
					const jwtToken = authtoken;                 
					const url = this.props.apiEndpointCadastro + 'Cemiterio/'+objid;//ok        
					const response = await fetch(url, {
					method: 'DELETE', 
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`
						},
					});              
					if (response.ok) {
						// mapa refresh
						// tree refresh      
						//ReloadData();
						let aux_rec_id=objid; 
						let aux_id='';
						this.tree.data.forEach(function(element, index, array) { 
							if (element.recid==aux_rec_id){ 
								console.log("This is an item of treeCollection: ", element); 
								aux_id = element.id; 
							}
						});
						if (aux_id!=''){ 
							this.tree.data.remove(aux_id); 
							this.tree.focusItem("parent");
						}               
					
					} else {
					const errorMessage = await response.text();
					console.error('error: ' + errorMessage);
					}
				};
				DeleteCemiterioAsync();
				} catch (error) {
				console.error('error: ' + error);                 
				} 
			}
		
			// vamos criar uma janela de confirmação do delete
		
			dhx.confirm({
				header:"Remover Cemitério",
				text:"Tem a certeza que pretende remover o cemitério?",
				buttons:["sim", "nao"],
				buttonsAlignment:"center"
			}).then(function(resposta){
				console.log('resposta ', resposta);
		
				switch(resposta){
					case false:
						DeleteCemiterio(objid); 
						break;
					default:
						break;
				}
			});
	
		}
		const ondeleteTalhao = (objid) => {
			const DeleteTalhao = (objid)=> {
				try {				
				const DeleteTalhaoAsync = async () => {  
					const jwtToken = authtoken;                 
					const url = this.props.apiEndpointCadastro + 'Talhao/'+objid;//ok        
					const response = await fetch(url, {
					method: 'DELETE', 
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`
						},
					});              
					if (response.ok) {
						// mapa refresh
						// tree refresh      
						//ReloadData();
						let aux_rec_id=objid; 
						let aux_id='';
						this.tree.data.forEach(function(element, index, array) { 
							if (element.recid==aux_rec_id){ 
								console.log("This is an item of treeCollection: ", element); 
								aux_id = element.id; 
							}
						});
						if (aux_id!=''){ 
							this.tree.data.remove(aux_id); 
							this.tree.focusItem("parent");
						}               
					
					} else {
					const errorMessage = await response.text();
					console.error('error: ' + errorMessage);
					}
				};
				DeleteTalhaoAsync();
				} catch (error) {
				console.error('error: ' + error);                 
				} 
			}
		
			// vamos criar uma janela de confirmação do delete
		
			dhx.confirm({
				header:"Remover Talhão",
				text:"Tem a certeza que pretende remover o talhão?",
				buttons:["sim", "nao"],
				buttonsAlignment:"center"
			}).then(function(resposta){
				console.log('resposta ', resposta);
		
				switch(resposta){
					case false:
						DeleteTalhao(objid); 
						break;
					default:
						break;
				}
			});
	
		}

		const SetLayersMap = (id) => {
			var checkedElem = this.tree.getChecked();
			var layers = this.tree.data.map(function(element) {
				if (checkedElem.includes(element.id)) {
				  return element.recid;
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
			var pai=layers.join("|");
 
			var estado = viewer.getState();
			//Get active map name
			var activeMapName = estado.config.activeMapName;

			var args = estado.config;
			//args.agentUri && args.agentKind 

			//Get active runtime map
			var mapState = estado.mapState[activeMapName];
			var currentMap = mapState.mapguide.runtimeMap;
			var currentView = estado.mapState[activeMapName].currentView;
			var sessionId  = currentMap.SessionId;

			var mapaDef  = currentMap.MapDefinition;
  
			this.props.guardaGruposAtivos({payload: { dados: layers}});

			this.setState(prevState =>  {
				return {
				checkedIds:  [...pai.split('|')],
				checkedElem: [...checkedElem],					 
			};
			}, () => { 
				const construcoesString = this.state.checkedIds.join('|');
				//const construcoesString = pai;
				try {
					
					const setConstrucoes = async () => {					 
	
						const jwtToken = authtoken;
						const url = this.props.apiEndpointSIG + 'MapaConstrucoes';//ok

						const response = await fetch(url, {
							method: 'POST',
							body: JSON.stringify({
									mapa: activeMapName,
									mapadef: mapaDef,
									sessionid: sessionId,
									userid: userid,
									usersession: usersession,
									viewer: 'false',
									construcoes: construcoesString,
								}),
							headers: {
									'Content-Type': 'application/json',
									'Authorization': `Bearer ${jwtToken}`
								},
						});
				
						if (response.ok) {
							
							var state = viewer.getState();
							const args = state.config;
							var NomeMapa = state.config.activeMapName;
							const uid = uuidv4();
							var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};
						
							viewer.dispatch(aux);
	

						} else {
							const errorMessage = await response.text();
							console.error('error: ' + errorMessage);
							this.setState({ errorMessage: errorMessage });
						}
					};
					setConstrucoes();
				} catch (error) {
					console.error('error: ' + error);
					this.setState({ errorMessage: 'Ocorreu um erro ao atualizar Pretensões.' });
				}
			})

			
			
		}
	 
	}
	componentDidUpdate() { 
		//console.log('aui');
		//console.log(this.props);
		if (this.props.update_tipo!="") {

			 
			if (this.props.update_tipo=="insert") {
				let aux_parent_recid=this.props.update_parent_recid;
				let aux_rec_id=this.props.update_rec_id;
				let aux_nome=this.props.update_nome;
				let aux_tipo=this.props.update_objtipo;
				let aux_id='';
				let tipo_insert='talhao';
				if (aux_parent_recid==undefined){
					aux_parent_recid='principal';
				}
				if (aux_tipo=='pai'){
					tipo_insert='cemiterio';
				}  
				if (aux_parent_recid!=''){
					this.tree.data.forEach(function(element, index, array) { 
						if ((element.recid==aux_parent_recid)&&(element.tipo==aux_tipo)){
							//console.log("Encontrei");
							//console.log("This is an item of treeCollection: ", element.recid);
							//console.log("This is an item of treeCollection: ", element.parentid);
							//console.log("This is an item of treeCollection: ", element.value);
							//console.log("This is an index of the element: ", index);
							//console.log("This is an array of the elements: ", array);
							aux_id = element.id; 
						}
					});
					if (aux_id!=''){
						
						this.tree.data.add({ recid: aux_rec_id, parentid: aux_parent_recid, value:aux_nome, tipo: tipo_insert },-1, aux_id);
						this.props.fezAtualizacao();
					}  
				} else {
						
					this.tree.data.add({ recid: aux_rec_id, parentid: aux_parent_recid, value:aux_nome, tipo: tipo_insert}, -1, 'parent');
					this.props.fezAtualizacao();
				}
			}
			if (this.props.update_tipo=="update") {

				let aux_rec_id=this.props.update_rec_id;
				let aux_nome=this.props.update_nome;
				let aux_id='';
				this.tree.data.forEach(function(element, index, array) { 
					if (element.recid==aux_rec_id){
						//console.log("Encontrei");
						console.log("This is an item of treeCollection: ", element);
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
					this.tree.data.update(aux_id,{ value:aux_nome });
					this.props.fezAtualizacao();
				}
			}
			if (this.props.update_tipo=="delete") {

				let aux_rec_id=this.props.update_rec_id;
				let aux_id='';
				this.tree.data.forEach(function(element, index, array) { 
					if (element.recid==aux_rec_id){
						//console.log("Encontrei"); 
						aux_id = element.id; 
					}
				});
				if (aux_id!=''){
					this.tree.data.remove(aux_id);
					this.props.fezAtualizacao();
				}
			}
			if (this.props.update_tipo=="reloadtree") {
				const TreePret = this.tree;
				TreePret.data.removeAll();
				const url = `${this.props.apiEndpoint}ArvConstrucoes/${this.props.userid}`;
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
					TreePret.data.parse(jsonData);
				})
				.catch(error => {
					console.error('There was a problem with your fetch operation:', error);
				});

				this.props.fezAtualizacao();
			} 
		}

		if (this.props.localiza_arvore) {
			if (this.props.sigla_separador=="CEM") {
				const elemento = this.tree.data.find(element => element.recid === this.props.rec_id_arvore);
				if (elemento) {
					const recid = this.props.rec_id_arvore;
					this.tree.focusItem(elemento.id);
					this.tree.selection.remove();
					this.tree.selection.add(elemento.id);
					this.setState(prevState => ({
						selectedId:  recid 				 
					}));
					let permissaoEscrita = Perm.findPermissionByCodAndSubCod(this.props.funcionalidadesRelevantes, Perm.FGU_AcessoEscritaArvoreConstrucoes, 'G:'+recid);
					
					this.props.carregaGridCEM({grupo_recid: recid});
					this.props.fezLocalizacaoArvore();
				}  
			}
		}

	}
	componentWillUnmount() {
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

// tem que retornar um objeto

//Mapear o estado para as propriedades do objeto
function mapStateToProps(state){
	/*export const FGU_NovoGrupoPretensao = 3560;
export const FGU_NovoItemPretensao = 3570;
export const FGU_ConsultarItemPretensao = 3575;
export const FGU_EditarGrupoPretensao = 3580;
export const FGU_EditarItemPretensao = 3590;
export const FGU_EditarOrdemPretensao = 3595;
export const FGU_PesquisaPretensao = 3600;
export const FGU_ObterSHPPretensao = 3605;
export const FGU_ImportarPretensao = 3610;*/


	const listaDeCodigosPermissoes = Perm.listaDeCodigosConstrucoes;
	const funcionalidade = state.aplicacaopcc.funcionalidades;
			
	// Filter permissions that have cod in listaDeCodigosPermissoes
	const funcionalidadesRelevantes = funcionalidade.filter(permission => {
		// Check if cod is in listaDeCodigosInteressantes
		return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
	});
	const permissaoNovoGrupo = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_NovoGrupoPretensao, '');
	const permissaoNovoItem = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_NovoItemPretensao, '');
	const permissaoEditarGrupo = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_EditarGrupoPretensao, '');
	const permissaoEditarItem = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_EditarItemPretensao, '');
	const permissaoEditarOrdem = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_EditarOrdemPretensao, '');
	
	const permissaoObterSHPP = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_ObterSHPPretensao, '');
 
	return {
		arv : state.message,  
		update_tipo: state.aplicacaopcc.treeconstrucoes.treeconstrucoes_tipo,
		update_rec_id: state.aplicacaopcc.treeconstrucoes.treeconstrucoes_rec_id,
		update_nome:  state.aplicacaopcc.treeconstrucoes.treeconstrucoes_nome,
		update_objtipo:  state.aplicacaopcc.treeconstrucoes.treeconstrucoes_objtipo,
		update_parent_recid:  state.aplicacaopcc.treeconstrucoes.treeconstrucoes_parent_recid,
		reloadKey: state.aplicacaopcc.treeatendimento.reloadKey,

		apiEndpoint: state.aplicacaopcc.config.configapiEndpoint,
		apiEndpointCadastro: state.aplicacaopcc.config.configapiEndpointCadastro,
		apiEndpointSIG: state.aplicacaopcc.config.configapiEndpointSIG,
		authtoken: state.aplicacaopcc.geral.authtoken, 
		userid: state.aplicacaopcc.geral.userid,
		usersession: state.aplicacaopcc.geral.usersession,	

		sigla_separador: state.aplicacaopcc.windows.localiza_na_arvore.sigla_separador, 
		localiza_arvore: state.aplicacaopcc.windows.localiza_na_arvore.localiza_arvore, 
		rec_id_arvore: state.aplicacaopcc.windows.localiza_na_arvore.rec_id_arvore,


		permissaoNovoGrupo: permissaoNovoGrupo,
		permissaoNovoItem: permissaoNovoItem,
		permissaoEditarGrupo: permissaoEditarGrupo,
		permissaoEditarItem: permissaoEditarItem,
		permissaoEditarOrdem: permissaoEditarOrdem,
		permissaoObterSHPP: permissaoObterSHPP,	
		funcionalidadesRelevantes: funcionalidadesRelevantes	
	};
}
//Envia nova propriedades para o estado
function mapDispatchToProps(dispatch){

	return {
		carregaGridCEM: (item) => dispatch({
			type: Actions.TREECEM_SELECIONA, payload: item.grupo_recid
		} ) ,
		fezAtualizacao: () => dispatch({
			type: Actions.TREECEM_UPDATEFIM, payload: ''
		}),
		guardaGruposAtivos:(item) => dispatch({
			type: Actions.TREECEM_GUARDA_GRUPOSLIGADOS, payload: item.payload
		}),
		fezLocalizacaoArvore:(item) => dispatch({
			type: Actions.TREELOCALIZA_END, payload: ''
		}),
		 
	};
}
 
export default connect(mapStateToProps,mapDispatchToProps)(TreeCEM_G);






