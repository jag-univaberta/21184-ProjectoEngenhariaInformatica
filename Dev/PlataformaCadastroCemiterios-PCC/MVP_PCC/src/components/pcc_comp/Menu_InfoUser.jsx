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
import Menu_InfoUser_C from './Menu_InfoUser_C';
const UserIcon = () => {
	return (
	  <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', width: '24px', height: '24px' }}>
		<PersonCircle24Regular />
	  </div>
	);
};


 

class Menu_InfoUser extends PureComponent {
	constructor(props) {
		super(props);
		this.state = {
			isDisabled: "",
		};
		
		
		this.data = new TreeCollection();
		this.data.events.on("load", () => {
			/*this.setState({
				isDisabled: this.data.getItem("edit").disabled,
			});*/
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
				<Menu_InfoUser_C css="dhx_widget--bordered dhx_widget--bg_white" data={this.data}   />
				 
			</div>
		);
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
	};
}
Menu_InfoUser.propTypes = {
	navigationType: PropTypes.oneOf(["pointer", "click"]),
};

export default connect(mapStateToProps,mapDispatchToProps)(Menu_InfoUser);