import React, { Component } from "react";
import PropTypes from "prop-types";
import { Menu as MenuDHX, TreeCollection } from "dhx-suite";

export class MenuCEM extends Component {
	componentDidMount() {
		this.menu = new MenuDHX(this.el, {
			css: "dhx_widget--bordered dhx_widget--bg_white",
		});
		this.menu.data.load(`http://192.168.99.1:5567/static/menu.json`);
	}
	componentWillUnmount() {
		this.menu && this.menu.destructor();
	}
	render() {
		return <div style={{ width: "100%", maxWidth: 1200 }} ref={el => (this.el = el)}></div>;
	}
}

MenuCEM.propTypes = {
	css: PropTypes.string,
	data: PropTypes.instanceOf([PropTypes.array, PropTypes.instanceOf(TreeCollection)]),
	navigationType: PropTypes.oneOf(["pointer", "click"]),
};

export default MenuCEM;