//
import React, { Component } from "react";
 

import * as Actions from "../../constants/actions"; 
//import { connect } from "react-redux";
class AtendimentoRegistos extends Component {
  constructor(props) {
    super(props);
    const  atendimentoId  = props.atendimentoId;
    const apiEndpoint = props.apiEndpoint;
    this.state = {
      data: null,
      previousAtendimentoId: null,
      atendimentoId: atendimentoId,
    };

   // this.dispatch = useReduxDispatch();
    //this.atendimentoId = useSelector((state)=> state.aplicacaopcc.atendimentoId);
    //fetchData();

  }


  async fetchData() {
    //const url = 'http://112.115.117.10/pcc_webapi/api/Atendimento/' + this.props.atendimentoId;
    
    const url = apiEndpoint + 'Atendimento/' + this.props.atendimentoId;
    const response = await fetch(url);
    const json = await response.json();
    this.setState({ data: json });
  }

  componentDidMount() {
    if (this.state.atendimentoId !== this.state.previousAtendimentoId) {
      this.fetchData();
      this.setState({ previousAtendimentoId: this.state.atendimentoId });
    }
  }

  componentDidUpdate() {
    if (this.state.atendimentoId !== this.state.previousAtendimentoId) {
      this.fetchData();
      this.setState({ previousAtendimentoId: this.state.atendimentoId });
    }
  }

  render() {
    return (
      <form className="pccwindow_form">
        <br></br>
        <table className="pccwindow_tabela  pccwindow_titulo">
          <tr><td>Registos</td></tr>
        </table>
        
        <table className="pccwindow_tabela pccwindow_detalhe">
          {this.state.data && (<tr><td className="pccwindow_detalhe_c1">Descrição:</td><td>{this.state.data.descricao}</td></tr>)}
        </table> 
        <table class="pccwindow_tabela  pccwindow_titulo">
        <tr><td>Confrontantes</td></tr>
        </table>
        <table class="pccwindow_tabela pccwindow_detalhe">
        {this.state.data && ( <tr><td class="pccwindow_detalhe_c1" >Norte:</td><td>{this.state.data.confrontante_norte}</td></tr> )}
        {this.state.data && ( <tr><td class="pccwindow_detalhe_c1" >Sul:</td><td>{this.state.data.confrontante_sul}</td></tr> )}
        {this.state.data && ( <tr><td class="pccwindow_detalhe_c1" >Nascente:</td><td>{this.state.data.confrontante_nascente}</td></tr> )}
        {this.state.data && ( <tr><td class="pccwindow_detalhe_c1" >Poente:</td><td>{this.state.data.confrontante_poente}</td></tr> )}
        </table> 
      </form>
    );
  }
}

 
export default AtendimentoRegistos;