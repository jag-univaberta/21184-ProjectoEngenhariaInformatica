//
import React, { Component, PureComponent } from "react";
import PropTypes from "prop-types";
import { Tree as TreeDHX, TreeCollection } from "dhx-suite";
import "dhx-suite/codebase/suite.min.css";
 
import { useDispatch, useSelector } from 'react-redux';
import TreeCEM_G from "./TreeCEM_G";

export class TreeCEM extends PureComponent {
	constructor(props) {
		super(props); 
		this.data = new TreeCollection();
		this.data.events.on("load", () => {
			let i = this.data.map(item => (item.opened ? 1 : 0)).reduce((a, b) => a + b, 0);
			 
			this.data.events.on("onReady", () => {
				this.data.events.on("onItemCheck", (id, state) => {
				  console.log(`Item ${id} checked: ${state}`);
				});
			  });
		}); 
 
	
		const datajson  = props.construcoesdata;
		//console.log(datajson);
		this.data.parse(datajson);
	  
		 
	}
	handleClick() {
		this.data.map(item => this.data.update(item.id, { opened: false }));
		 
	}
	render() {
		 
		return (
			<div style={{height:"100%", background: "#b3d3ef21" }}>
				<TreeCEM_G authtoken={this.props.authtoken} userid={this.props.userid} apiEndpoint={this.props.apiEndpoint} usersession={this.props.usersession} keyNavigation={true} checkbox={true} data={this.data} />				
			</div>
		);
	}
}

TreeCEM.propTypes = {
	data: PropTypes.instanceOf([PropTypes.array, PropTypes.instanceOf(TreeCollection)]),
	icon: PropTypes.shape({
		folder: PropTypes.string,
		openFolder: PropTypes.string,
		file: PropTypes.string,
	}),
	css: PropTypes.string,
	keyNavigation: PropTypes.bool,
	dragCopy: PropTypes.bool,
	dragMode: PropTypes.string,
	dropBehaviour: PropTypes.string,
	editable: PropTypes.bool,
	autoload: PropTypes.bool,
	checkbox: PropTypes.bool,
};
 
export default  TreeCEM;
