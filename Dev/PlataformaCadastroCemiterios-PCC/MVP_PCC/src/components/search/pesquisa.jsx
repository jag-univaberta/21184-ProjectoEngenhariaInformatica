import React, { Component, PureComponent } from "react";
import PropTypes from "prop-types";
import { Toolbar as ToolbarDHX, TreeCollection } from "dhx-suite";
 


class PesquisaToolbar extends Component {
	constructor(props) {
		super(props);
		this.state = {
			event: "",
			id: "",
		};
	}

	componentDidMount() {
		 
		this.toolbar = new ToolbarDHX(this.el, {
			data:  [ {
				"id": "search",
				"type": "input",
				"placeholder": "Search",
				"icon": "dxi dxi-magnify"
				}
			],
			navigationType: "pointer",
		});
		this.toolbar.events.on("click", id => this.handleClick(id, "click"));
		this.toolbar.events.on("inputCreated", id => this.handleClick(id, "inputCreated"));
		this.toolbar.events.on("openMenu", id => this.handleClick(id, "openmenu"));
		this.toolbar.events.on("inputFocus", id => this.handleClick(id, "inputFocus"));
		this.toolbar.events.on("inputBlur", id => this.handleClick(id, "inputBlur"));
		this.toolbar.events.on("afterHide", id => this.handleClick(id, "afterHide"));
		this.toolbar.events.on("beforeHide", id => this.handleClick(id, "beforeHide"));
	}
	handleClick(id, event) {
		this.setState({
			event: event,
			id: id,
		});
	}
	componentWillUnmount() {
		this.toolbar.destructor();
	}
	render() {
		return <div style={{ width: "100%" }} ref={el => (this.el = el)}></div>;
	}
}
 

PesquisaToolbar.propTypes = {
	css: PropTypes.string,
	data: PropTypes.instanceOf([PropTypes.array, PropTypes.instanceOf(TreeCollection)]),
	navigationType: PropTypes.string,
};

export default PesquisaToolbar;