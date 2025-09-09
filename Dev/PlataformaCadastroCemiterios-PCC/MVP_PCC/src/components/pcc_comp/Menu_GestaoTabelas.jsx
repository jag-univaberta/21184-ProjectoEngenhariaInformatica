import React, { Component, PureComponent } from "react";
import PropTypes from 'prop-types';
import { Menu as MenuDHX, TreeCollection } from "dhx-suite";
import * as Actions from "../../constants/actions";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';  
import { v4 as uuidv4 } from 'uuid'; 
import { connect } from "react-redux";
import * as Perm from "../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 
  
class Menu_GT extends Component {
	componentDidMount() {
		let { css, data, aplicacao_sigla, apiEndpoint, authtoken, 
			permissaoGestaoConcessionarios, permissaoGestaoTiposMovimentos, permissaoGestaoTiposConstrucao    
		 } = this.props;
		 

		this.menu = new MenuDHX(this.el, {
			css: css,
			data: [{id:"tabelas", value: "Gestão de tabelas",
					items:[ 
						{id: "concessionarios", value:"Gestão dos concessionários", disabled: !permissaoGestaoConcessionarios},
						{ type: "separator" } ,
						{id: "movimentos", value:"Tipos de movimentos", disabled: !permissaoGestaoTiposMovimentos},
						{ type: "separator" } ,
						{id: "tiposconstrucao", value:"Tipos de construção", disabled: !permissaoGestaoTiposConstrucao},
					]
					}],
		});		 
	
		/*if (aplicacao_sigla === 'PCC') {
			console.log(permissaoProcessarLayersConfrontacao);
			console.log(permissaoGestaoUsersAdministrativa);
			this.menu = new MenuDHX(this.el, {
				css: css,
				data: [{id:"tabelas", value: "Gestão de tabelas",
						items:[ 
							{id: "separadores", value:"Gestão dos separadores", disabled: !permissaoGestaoSeparadores},
							{ type: "separator" } ,
							{id: "regulamentos", value:"Gestão de Regulamentos", disabled: !permissaoGestaoRegulamentos},
							{type:"separator"},
							{id: "utilizadores_administrativa", value:"Utilizadores Ap. Admnistrativa" , disabled: !permissaoGestaoUsersAdministrativa},
							{type:"separator"},
							{id: "entidades_externas", value:"Entidades Externas" , disabled: !permissaoGestaoEntidadesExternas},
							{id: "servicos", value:"Serviços", disabled: !permissaoGestaoServicos},
							{id: "tipos_intervencao_solo", value:"Tipos Intervenção Solo", disabled: !permissaoGestaoTiposIntervencaoSolo},
							{id: "tipos_pretensao", value:"Tipos de Pretensão", disabled: !permissaoGestaoTiposPretensao},
							{id: "tipos_atendimento", value:"Tipos do Atendimento", disabled: !permissaoGestaoTiposAtendimento},
							{id: "tipos_requerente_atendimento", value:"Tipo de Requerentes do Atendimento", disabled: !permissaoGestaoTiposRequerenteAtendimento},
							{id: "estados_processo", value:"Estados do Processo", disabled: !permissaoGestaoEstadosProcesso},
							{type:"separator"},
							{id: "layers_confrontacao", value:"Layers Confrontação", disabled: !permissaoProcessarLayersConfrontacao},
						]
						}],
			});
			
		}*/
		this.menu.events.on("click", function(id, e){
			console.log(id); 
			 
			switch (id){ 
				case 'concessionarios': 
					var aux={ type: Actions.SHOW_GESTAOCONCESSIONARIOS, payload: "" };
					viewer.dispatch(aux); 
					break;
				case 'movimentos': 
					var aux={ type: Actions.SHOW_GESTAOTIPOSMOVIMENTO, payload: "" };
					viewer.dispatch(aux); 
					break;
				case 'tiposconstrucao': 
					var aux={ type: Actions.SHOW_GESTAOTIPOSCONSTRUCAO, payload: "" };
					viewer.dispatch(aux); 
					break;
				 
				default:
					break; 
			}  
		});
	}
	componentWillUnmount() {
		this.menu && this.menu.destructor();
	}
	render() {
		return <div style={{ width: "100%", maxWidth: 1200 }} ref={el => (this.el = el)}></div>;
	}
}

class Menu_GestaoTabelas extends PureComponent {
	constructor(props) {
		super(props);
		this.state = {
			isDisabled: "",
		};
		this.data = new TreeCollection();
		this.data.events.on("load", () => {
		});
	}
	componentDidMount() {
		 
		 
	}
	componentWillUnmount() {
		this.data.events.detach("load");
	}
	 
	render() {
		return (
			<div style={{ width: "100%", maxWidth: 1200 }}>
				<Menu_GT css="dhx_widget--bordered dhx_widget--bg_white" data={this.data}   authtoken={this.props.authtoken} userid={this.props.userid}  
				apiEndpoint={this.props.apiEndpoint}  aplicacao_sigla={this.props.aplicacao_sigla} 
				permissaoGestaoConcessionarios ={this.props.permissaoGestaoConcessionarios} 
				permissaoGestaoTiposMovimentos = {this.props.permissaoGestaoTiposMovimentos}
				permissaoGestaoTiposConstrucao = {this.props.permissaoGestaoTiposConstrucao} 
				/>
				 
			</div>
		);
	}
}
 
function mapStateToProps(state){

	const listaDeCodigosPermissoes = Perm.listaDeCodigosGestaoTabelas;
	const funcionalidade = state.aplicacaopcc.funcionalidades;
			
	// Filter permissions that have cod in listaDeCodigosPermissoes
	const funcionalidadesRelevantes = funcionalidade.filter(permission => {
		// Check if cod is in listaDeCodigosInteressantes
		return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
	});
	const permissaoGestaoConcessionarios = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_GestaoSeparadores, '') || true;
	const permissaoGestaoTiposMovimentos = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_GestaoRegulamentos, '') || true;
    const permissaoGestaoTiposConstrucao = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.F_GestaoUsersAdministrativa, '') || true;
    
	
	return {
		  
		permissaoGestaoConcessionarios: permissaoGestaoConcessionarios,
		permissaoGestaoTiposMovimentos: permissaoGestaoTiposMovimentos,
		permissaoGestaoTiposConstrucao: permissaoGestaoTiposConstrucao 
	};
}
//Envia nova propriedades para o estado
function mapDispatchToProps(dispatch){

	return {
		 
	};
}
 
export default connect(mapStateToProps,mapDispatchToProps)(Menu_GestaoTabelas);