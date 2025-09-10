import React, { Component, PureComponent } from "react"; 
import { Grid as GridDHX, DataCollection, ContextMenu } from "dhx-suite"; 
import { connect } from "react-redux";
import * as Actions from "../../../constants/actions"; 
import * as Perm from "../../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 
import { setConstrucoesSelection } from '../../../utils/utils';
 class GridCEM_G extends Component {
	 
	componentDidMount() {
		const {
			rowHeight,
			adjust,
			autoWidth,
			columns,
			data,
			editable, 
			htmlEnable,
			selection,
			permissaoConsultar,
			permissaoEditarItem,
			permissaoObterSHPP, 
			funcionalidadesRelevantes,
			localiza_grelha, rec_id_grelha, sigla_separador
		} = this.props;

		this.grid = new GridDHX(this.el, {
			rowHeight,
			adjust,
			autoWidth,
			columns,
			data,
			editable, 
			htmlEnable,
			selection,			
		});

		const cmenu = new dhx.ContextMenu(null, {
			css: "dhx_widget--bg_gray",
			data: [
			  { id: "localizar_construcao", value: "Localizar" }, 
			  { type: "separator" } ,
			  { id: "editar_construcao", value: "Editar" }, 
			  { type: "separator" } ,
			  { id: "eliminar_construcao",value: "Eliminar" } ,
			  { type: "separator" } ,
			  //{ id: "mover_construcao", value: "Mover Construção" }, 
			  //{ type: "separator" } ,
			  { id: "exportar_construcao",value: "Exportar Shapefile" } ,
			]
		});

		let objid;
		let viewerstate;
		let parent_id;
		let apiEndpointCadastro;
		let apiEndpointSIG;
		let authtoken;
		this.grid.events.on('CellRightClick', function (row, column, e) {
			//this.grid.selection.setCell(row.id, column);
			objid= row.rec_id;
			viewerstate = viewer.getState(); 
			parent_id=viewerstate.aplicacaopcc.grupo_pret_recid;
			apiEndpointCadastro=viewerstate.aplicacaopcc.config.configapiEndpointCadastro;
			authtoken=viewerstate.aplicacaopcc.geral.authtoken;
			const event = e;
			//this.setState({
				//showContextMenu: true,
			//});
			//awaitRedraw().then(function () {
				//this.contextMenu.showAt(event);
			//});
			//this.contextMenu.showAt(event);
			console.log(objid);
			let permissaoEscrita = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_AcessoEscritaArvoreConstrucoes, 'G:'+parent_id);
			
			if(permissaoConsultar){

				cmenu.enable("editar_construcao");
				cmenu.disable("eliminar_construcao"); 
				cmenu.disable("mover_construcao"); 
				cmenu.enable("exportar_construcao"); 
			}

			if(permissaoEditarItem && permissaoEscrita) { 

				cmenu.enable("editar_construcao");
				cmenu.enable("eliminar_construcao"); 
				cmenu.enable("mover_construcao"); 
				cmenu.enable("exportar_construcao"); 
			}
			if(permissaoEditarItem && !permissaoEscrita) { 

				cmenu.enable("editar_construcao");
				cmenu.disable("eliminar_construcao"); 
				cmenu.disable("mover_construcao"); 
				cmenu.enable("exportar_construcao"); 
			}

			if (permissaoObterSHPP) {
				cmenu.enable("exportar_construcao"); 
				//console.log(" cmenu.enable edit_grupo ");
				//console.log(" cmenu.enable remover ");
			 }else {
				cmenu.disable("exportar_construcao"); 
				//console.log(" cmenu.disable edit_grupo ");
				//console.log(" cmenu.disable remover ");
			}

			event.preventDefault();
			cmenu.showAt(event);
			return false;
		});

	
		this.grid.events.on("cellDblClick",  (row, column, e) => {
			
			//row.nome
			//row.numero
			//row.tipo
			if(permissaoConsultar){
				this.props.loadConstrucao({pretensaoId: row.rec_id});
			}

		});

		cmenu.events.on("click", (e, id) => {

			const onGetShapeFile = async (tipo, parent_id) => {
		 
				const url = apiEndpointCadastro + 'ConstrucaoShapefile?tipo=' + tipo + '&id=' + parent_id;

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
		
				const fileName = `Construcao_${formattedDateTime}.zip`;
	
				
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
			if(e == "localizar_construcao"){
				console.log('localizar_construcao');
				onlocalizaWindow(objid, parent_id); 
			}
			if(e == "editar_construcao"){
				console.log('editar_construcao');  
				this.props.loadConstrucao({pretensaoId: objid});
			}
			if(e == "mover_construcao"){
				console.log('mover_construcao');  
				this.props.loadMoverConstrucao({parentconstrucaoid: parent_id, construcaoid: objid});
			}
			
			if(e == "eliminar_construcao"){
				console.log('eliminar_construcao');
				ondeleteWindow(objid, parent_id); 
			}
			if(e == "exportar_construcao"){
				console.log('exportar_construcao');
				onGetShapeFile('construcao',objid); 
			}			
		});
		const tempStorage = {};

		const onlocalizaWindow = (objid, parent_id) => { 
			try { 
				const localizaConstrucaocall = async () => {      
					
					viewerstate = viewer.getState();  
					apiEndpointCadastro=viewerstate.aplicacaopcc.config.configapiEndpointCadastro;
					
					apiEndpointSIG=viewerstate.aplicacaopcc.config.configapiEndpointSIG;

					let urlloc= apiEndpointCadastro + 'Construcao/' + objid;//ok   
					let jwtToken = authtoken;     
					let response = await fetch(urlloc, {
					method: 'GET',  
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`
					},
					}); 
					try{
						let jsonData = await response.json();
						let centroid = jsonData.centroid;
						let bbox = jsonData.mbr;
						onzoomToWindow(centroid, bbox, objid); 
						
				 	}catch(e){  
						console.error('error: ' + e);
					}
				};
				localizaConstrucaocall();
			} catch (error) {
			console.error('error: ' + error);                 
			} 
			  	 
		}

		const ondeleteWindow = (objid, parent_id) => {
			const DeleteConstrucao = (objid, parent_id)=> {
				try { 
				const deleteConstrucaocall = async () => {                   
				  const url = apiEndpointCadastro + 'Construcao';//ok   
				  const jwtToken = authtoken;     
				  const response = await fetch(url, {
				  method: 'DELETE',   
				  body: JSON.stringify({ 
					id : objid
					}),        
				  headers: {
					'Content-Type': 'application/json',
					'Authorization': `Bearer ${jwtToken}`
					},
				  });              
				  if (response.ok) {    
					try{
					  menuRef.current?.destructor();
					  tabRef.current?.destructor();
					  formRefgeral.current?.destructor();
					  windowRef.current?.destructor();
					  menuRef.current = null;
					  tabRef.current = null;
					  formRefgeral.current = null;
					  windowRef.current = null;
					}catch(e){}   
					
					viewer.dispatch({type: Actions.CONSTRUCAO_FECHA,payload: ""});  
					viewer.dispatch({type: Actions.HIDE_IMPORTAFILE_CONSTRUCAO, payload: ""});
					viewer.dispatch({type: Actions.TREECEM_LIMPA, payload: "" }) 
					viewer.dispatch({type: Actions.TREECEM_SELECIONA, payload: parent_id }) 
				  } else {
					const errorMessage = await response.text();
					console.error('error: ' + errorMessage);
				  }
				};
				deleteConstrucaocall();
				} catch (error) {
				console.error('error: ' + error);                 
				} 
			  }
			
			// vamos criar uma janela de confirmação do delete
		
			dhx.confirm({
			  header:"Remover construção",
			  text:"Tem a certeza que pretende remover a construção?",
			  buttons:["sim", "nao"],
			  buttonsAlignment:"center"
			}).then(function(resposta){
			  console.log('resposta ', resposta);
		
			  switch(resposta){
				case false:
					DeleteConstrucao(objid, parent_id); 
				  	break;
				default:
				  	break;
			  }
			});
			

		}
		const onzoomToWindow = (centroid, bboxWKT, construcaoid) => {
			//var centroid = dadosConstrucao?.centroid;
			if(centroid!=null){
			  const coordsc = centroid.substring(7, centroid.length - 1).split(' ');
			  const centro_x = parseFloat(coordsc[0]);
			  const centro_y = parseFloat(coordsc[1]);
		
			  //var bboxWKT = dadosConstrucao?.bbox;
		
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
		
			  // Calculate the initial map scale
			  const resolution = Math.max(width / 800, height / 600);
			  const escala = parseInt( resolution / 0.00028) + 50;
			  if (centro_x!=null && centro_y!= null && escala !=null){
				console.log(centro_x);
				console.log(centro_y);
				console.log(escala); 
				var state = viewer.getState(); 
				var NomeMapa = state.config.activeMapName;
				
				var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }};
				//zoomToView(aux);
				viewer.dispatch(aux);
			  }
			 
			  let estado = viewer.getState();			
			  let activeMapName = estado.config.activeMapName;
			  let mapState = estado.mapState[activeMapName];
			  let currentMap = mapState.mapguide.runtimeMap; 
			  let sessionId  = currentMap.SessionId; 
			  let mapaDef  = currentMap.MapDefinition;
			    
			  let ObjectoGeografico_id='';
			   
			  apiEndpointSIG=estado.aplicacaopcc.config.configapiEndpointSIG;
			  
			  setConstrucoesSelection(apiEndpointSIG, authtoken, activeMapName, mapaDef, sessionId, construcaoid, centro_x, centro_y, escala);
 			 
			} 
			
	    }  

	}
	componentDidUpdate() { 
		if (this.props.localiza_grelha) {
			if (this.props.sigla_separador=="CEM") {
				const elemento = this.grid.data.find(element => element.rec_id === this.props.rec_id_grelha);
				if (elemento) {
					const recid = this.props.rec_id_grelha;
					//this.grid.focusItem(elemento.id);
					this.grid.selection.enable();
					this.grid.selection.removeCell();
					
					let row = this.grid.data.getItem(elemento.id);
					let column = this.grid.getColumn("descricao");
					this.grid.scrollTo(elemento.id,'descricao');
					setTimeout(() => {
						this.grid.selection.setCell(row, column, true, true);

						this.grid.scrollTo(row.id, column.id);

					},250);
					
					this.props.fezLocalizacaoGrelha();
				}  
			}
		}
	}
	componentWillUnmount() {
		this.grid && this.grid.destructor();
	}

	
	render() {
		return (<div style={{ width: "100%", height: "100%" }} ref={el => (this.el = el)}></div>);
	}


	 
}

 
 
function mapStateToProps(state){
	const listaDeCodigosPermissoes = Perm.listaDeCodigosConstrucoes;
		const funcionalidade = state.aplicacaopcc.funcionalidades;
		// Filter permissions that have cod in listaDeCodigosPermissoes
		const funcionalidadesRelevantes = funcionalidade.filter(permission => {
			// Check if cod is in listaDeCodigosInteressantes
			return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
		});
		const permissaoConsultar = true;//Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_ConsultarItemConstrucao, '');
	 	const permissaoEditarItem = true;//Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_EditarItemConstrucao, '');
		const permissaoObterSHPP = true;//Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_ObterSHPConstrucao, '');
	return {
		arv : state.message, 
		pretensaoId: state.aplicacaopcc.pretensaoId,
		permissaoConsultar: permissaoConsultar,
		permissaoEditarItem: permissaoEditarItem,
		permissaoObterSHPP: permissaoObterSHPP,
		funcionalidadesRelevantes: funcionalidadesRelevantes,
		sigla_separador: state.aplicacaopcc.windows.localiza_na_arvore.sigla_separador, 
		localiza_grelha: state.aplicacaopcc.windows.localiza_na_arvore.localiza_grelha, 
		rec_id_grelha: state.aplicacaopcc.windows.localiza_na_arvore.rec_id_grelha,
		
	};
}
//Envia nova propriedades para o estado
function mapDispatchToProps(dispatch){

	return {
		loadConstrucao: (item) => dispatch({
			type: Actions.CONSTRUCAO_EDITA, payload: item.pretensaoId
		}),
		loadMoverConstrucao: (item) => dispatch({
			type: Actions.SHOW_CONSTRUCAO_MOVER, payload: {item}
		}), 
		fezLocalizacaoGrelha: () => dispatch({
			type: Actions.GRELHALOCALIZA_END, payload:' '
		})
	};
}
 
export default connect(mapStateToProps,mapDispatchToProps)(GridCEM_G);
