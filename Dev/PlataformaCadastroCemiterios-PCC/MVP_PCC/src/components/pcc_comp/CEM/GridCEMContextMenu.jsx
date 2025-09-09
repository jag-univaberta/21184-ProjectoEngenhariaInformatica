import React, { Component } from "react";
import PropTypes from "prop-types";
import { Grid as GridDHX, DataCollection, ContextMenu } from "dhx-suite"; 
import { Menu as MenuDHX, TreeCollection } from "dhx-suite";

import awaitRedraw from "dhx-suite"; 

export class GridCEMContextMenu extends Component {
	componentDidMount() {
		  
		this.contextMenu = new ContextMenu(null, { css: 'grid-context-menu' });
		this.contextMenu.data.parse([
			{
				type: 'menuItem',
				id: 'remove',
				value: 'Remove Row',
			},
			{
				type: 'menuItem',
				id: 'cutrow',
				value: 'Cut Row',
			},
			{
				type: 'menuItem',
				id: 'pasterow',
				value: 'Paste Row',
			}
		]);

		 
	}
	componentWillUnmount() {
		this.menu && this.menu.destructor();
	}
	render() {
		return <div style={{ width: "100%", maxWidth: 1200 }} ref={el => (this.el = el)}></div>;
	}
}

  
export default GridCEMContextMenu;
 
