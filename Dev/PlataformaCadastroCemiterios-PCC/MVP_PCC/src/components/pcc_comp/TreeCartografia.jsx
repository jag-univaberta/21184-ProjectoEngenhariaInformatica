import React, { Component, PureComponent } from "react";
import PropTypes from "prop-types";
import { Tree as TreeDHX, TreeCollection } from "dhx-suite";
import "dhx-suite/codebase/suite.min.css";

import TreeCartografia_G from "./TreeCartografia_G";
import { useDispatch, useSelector } from 'react-redux';
import { processData } from '../../utils/utils';

export class TreeCartografia extends PureComponent {
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

		const datajson  = props.cartografiadata;
		//console.log(datajson);
		
		let newdados=processData(datajson);
		this.data.parse(newdados);
	}
	
	handleClick() {
		this.data.map(item => this.data.update(item.id, { opened: false }));
	}
	render() {
		 
		return (
			<div style={{height:"100%", background: "#b3d3ef21" }}>
				<TreeCartografia_G keyNavigation={true} checkbox={true} data={this.data} apiEndpoint={this.props.apiEndpoint} authtoken={this.props.authtoken}/>				
			</div>
		);
	}
}

TreeCartografia.propTypes = {
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

export default TreeCartografia;