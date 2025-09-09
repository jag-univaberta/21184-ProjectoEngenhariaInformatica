//
import * as React from 'react';

 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";

 

class FormDummy extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      errorMessage: '',
    }
  }

  componentDidMount() {
    fetch('config.json')
    .then(response => response.json())
    .then(data => {
      let apiEndpoint = data.apiEndpoint;
      let apiEndpointSIG = data.apiEndpointSIG;
      let apiEndpointCadastro = data.apiEndpointCadastro;
      let apiEndpointDocumentos = data.apiEndpointDocumentos; 
      let mapagenturl = data.mapagenturl;
      let mapa = data.mapa; 
      let aplicationtoken = data.aplicationtoken;  

      this.props.dispatchConfig({payload: {aplicationTokenId: aplicationtoken, startSessionId: window.sessionId, configapiEndpoint: apiEndpoint, configapiEndpointSIG: apiEndpointSIG, configapiEndpointCadastro: apiEndpointCadastro, configapiEndpointDocumentos: apiEndpointDocumentos, configmapagenturl: mapagenturl, configmapa: mapa } });
      
    })
    .catch(error => {
      console.error('Error fetching JSON:', error);
    });
  }
  render() {
     
    return  (<div></div>)
  }
}

//Mapear o estado para as propriedades do objeto
function mapStateToProps(state){
	return {
		arv : state.message, 
	};
}
//Envia nova propriedades para o estado
function mapDispatchToProps(dispatch){

	return {
		dispatchConfig: (item) => dispatch({
			type: Actions.CONFIG, payload: item.payload
		})
	};
}
 
export default connect(mapStateToProps,mapDispatchToProps)(FormDummy);

 
