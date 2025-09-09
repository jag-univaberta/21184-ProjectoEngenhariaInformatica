//
import React, { Component, PureComponent } from "react";
import PropTypes from "prop-types";
import { Grid as GridDHX, DataCollection } from "dhx-suite";

import GridCEM_G from "./GridCEM_G"; 
/*
function rowDataTemplate(value, row, col) {
    return `Nome: ${row.nome}</br>
            Número: ${row.numero}</br>
            Tipo: ${row.tipo}`;
}*/
function rowDataTemplate(value, row, col) {
    return `<div> 
                Designação: ${row.nome}</br>
                Número: ${row.numero}</br>
                Tipo construção: ${row.tipo}
            </div>`;
}
/*
function rowDataTemplate(value, row, col) {
    return `<div style={{z-index; "99999999" }}> Nome: ${row.nome}</br>
            Número: ${row.numero}</br>
            Tipo: ${row.tipo}
			</div>`;
}
*/
/*
 (value, row, col) => `<div class="custom-tooltip">
                <img src="https://snippet.dhtmlx.com/codebase/data/common/img/02/${row.avatar}.jpg" />
                <span>Last edit time:<br>${row.editing.toUTCString()}</span>
            </div>`
*/

export class GridCEM extends PureComponent {
	constructor(props) {
		super(props);
		const { grupo_pretId, apiEndpointCadastro, authtoken } = props;
		this.state = {
			firstItem: null,
			data: null,
		};
		this.data = new DataCollection();

		this.data.events.on("load", () => {
			this.setState({
				firstItem: this.data.getId(0) ? this.data.getItem(this.data.getId(0)).nome : "",
			});
		});
 
        const url = apiEndpointCadastro + "Construcao/PorTalhao/?id=" + grupo_pretId;    
	    const jwtToken = this.props.authtoken; // Certifique-se de que tem acesso a esta variável

		if (grupo_pretId!=''){
			/*this.data.load(url).then(() => {
				this.data.events.on("change", () => {
					this.setState({
						firstItem: this.data.getId(0) ? this.data.getItem(this.data.getId(0)).nome : "",
						data: this.data,
					});
				});
			});*/
			fetch(url, {
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
				'Authorization': `Bearer ${jwtToken}`, // Adiciona o cabeçalho de autenticação
			},
			})
			.then(response => {
			if (!response.ok) {
				throw new Error('Falha na autenticação ou na requisição.');
			}
			return response.json(); // Analisa a resposta JSON
			})
			.then(responseData => {
			// Carrega os dados na coleção DHTMLX após a requisição ser bem-sucedida
			this.data.parse(responseData);

			// A partir daqui, a sua lógica original para gerir a mudança de dados
			this.data.events.on("change", () => {
				this.setState({
				firstItem: this.data.getId(0) ? this.data.getItem(this.data.getId(0)).nome : "",
				data: this.data,
				});
			});
			})
			.catch(error => {
			console.error('Ocorreu um erro ao carregar os dados:', error);
			// Trate o erro de forma adequada, como mostrar uma mensagem ao utilizador.
			});


		}
		
 
	}
	componentWillUnmount() {
		this.data.events.detach("load");
	}
	handleClick() {
		if (this.state.firstItem) {
			this.data.remove(this.data.getId(0));
		} else {
			this.data.load(`${process.env.PUBLIC_URL}/static/grid.json`);
		}
	}
	componentDidUpdate(prevProps, prevState) { 
		if (prevProps.grupo_pretId !== this.props.grupo_pretId) {
			if (this.props.grupo_pretId !== '' && prevProps.grupo_pretId !== this.props.grupo_pretId) { 
				const apiEndpointCadastro = this.props.apiEndpointCadastro;
				const url = apiEndpointCadastro + "Construcao/PorTalhao/" + this.props.grupo_pretId;
				const jwtToken = this.props.authtoken; 
				this.data.removeAll(); 
				fetch(url, {
				method: 'GET',
				headers: {
					'Content-Type': 'application/json',
					'Authorization': `Bearer ${jwtToken}`, // Adiciona o cabeçalho de autenticação
				},
				})
				.then(response => {
				if (!response.ok) {
					throw new Error('Falha na autenticação ou na requisição.');
				}
				return response.json(); // Analisa a resposta JSON
				})
				.then(responseData => {
				// Carrega os dados na coleção DHTMLX após a requisição ser bem-sucedida
				this.data.parse(responseData);

				// A partir daqui, a sua lógica original para gerir a mudança de dados
				this.data.events.on("change", () => {
					this.setState({
					firstItem: this.data.getId(0) ? this.data.getItem(this.data.getId(0)).nome : "",
					data: this.data,
					});
				});
				})
				.catch(error => {
				console.error('Ocorreu um erro ao carregar os dados:', error);
				// Trate o erro de forma adequada, como mostrar uma mensagem ao utilizador.
				});
			} else {
				this.data.removeAll();
			}		
		}
	}
	render() {
		const columns = [
			{ hidden:true, id: "rec_id",   header: [{ text: "Rec_id" }] , tooltipTemplate: rowDataTemplate},
			{ width: "100%", id: "descricao", header: [{ text: "Construções" }] , tooltipTemplate: rowDataTemplate},
			{ hidden:true, id: "nome", header: [{ text: "Construções" }] , tooltipTemplate: rowDataTemplate},
			{ hidden:true, id: "numero",   header: [{ text: "Número" }] , tooltipTemplate: rowDataTemplate},  
			{ hidden:true,  id: "tipo",   header: [{ text: "Tipo" }] , tooltipTemplate: rowDataTemplate},
		];
		return (
			<div style={{ width: "100%", height: "100%" }}>
				<GridCEM_G
					rowHeight={30}
					adjust={false}
					autoWidth={false}
					columns={columns}
					data={this.data}
					editable={false} 
					multiselection={true}
					htmlEnable={true}
					selection={"complex"}
				/>				 
			</div>
		);
	}
}
/*tooltipTemplate: function (value, row, col) { 
                if (row.country === "Bangladesh") {
                    return false; // prevent a tooltip from being shown
                }
                return `<div className="custom-tooltip"> 
                    <img src="../data/common/img/02/${row.avatar}.jpg" /> 
                    <span>Last edit time:<br/>${row.editing.toUTCString()}</span> 
                </div>`; 
            } */
			
GridCEM_G.propTypes = {
	columns: PropTypes.array,
	spans: PropTypes.array,
	data: PropTypes.oneOfType([PropTypes.array, PropTypes.instanceOf(DataCollection)]),
	headerRowHeight: PropTypes.number,
	footerRowHeight: PropTypes.number,
	rowHeight: PropTypes.number,
	width: PropTypes.number,
	height: PropTypes.number,
	sortable: PropTypes.bool,
	rowCss: PropTypes.func,
	splitAt: PropTypes.number,
	selection: PropTypes.bool,
	sortable: PropTypes.bool,
	autoWidth: PropTypes.bool,
	css: PropTypes.string,
	selection: PropTypes.oneOf(["complex", "row", "cell"]),
	resizeble: PropTypes.bool,
	multiselection: PropTypes.bool,
	keyNavigation: PropTypes.bool,
	htmlEnable: PropTypes.bool,
	editable: PropTypes.bool,
	dragMode: PropTypes.oneOf(["target", "source", "both"]),
	dragCopy: PropTypes.bool,
	adjust: PropTypes.bool,
	autoEmptyRow: PropTypes.bool,
};

 
 
export default GridCEM;