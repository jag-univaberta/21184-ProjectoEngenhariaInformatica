
import config from '../../config.json';
import * as React from "react";
import { connect } from "react-redux";
import * as MapActions from "mapguide-react-layout/lib/actions/map";
import * as MessageActions from "../actions/message";
import { ReduxDispatch, IApplicationState, ActiveMapTool } from "mapguide-react-layout/lib/api/common";
import { Popover, PopoverInteractionKind, Position } from "@blueprintjs/core";


export interface IDemoComponentOwnProps {

}

export interface IDemoComponentProps {
    activeMapName: string;
    scale: number;
    selectedFeatureCount: number;
    selectedLayerCount: number;
    activeTool: ActiveMapTool;
    tooltipsEnabled: boolean;
}

export interface IDemoComponentDispatch {
    setActiveTool: (tool: ActiveMapTool) => void;
    setTooltipsEnabled: (enabled: boolean) => void;
    addMessage: (message: string) => void;
    clearMessages: () => void;
}

export type DemoComponentProps = Partial<IDemoComponentOwnProps> & Partial<IDemoComponentProps> & Partial<IDemoComponentDispatch>;

function mapStateToProps(state: Readonly<IApplicationState>, ownProps: Readonly<IDemoComponentOwnProps>): Partial<IDemoComponentProps> {
    const activeMapName = state.config.activeMapName;
    let scale;
    let selectedFeatureCount;
    let selectedLayerCount;
    let activeTool;
    if (activeMapName) {
        const mapState = state.mapState[activeMapName];
        if (mapState.currentView) {
            scale = mapState.currentView.scale;
        }
        if (mapState.mapguide?.selectionSet && mapState.mapguide.selectionSet.FeatureSet) {
            selectedFeatureCount = mapState.mapguide.selectionSet.FeatureSet.Layer.map(layer => layer.Class.ID.length).reduce((a, b) => a + b, 0);
            selectedLayerCount = mapState.mapguide.selectionSet.FeatureSet.Layer.length;
        }
    }
    return {
        activeMapName: activeMapName,
        scale: scale,
        selectedFeatureCount: selectedFeatureCount,
        selectedLayerCount: selectedLayerCount,
        activeTool: state.viewer.tool,
        tooltipsEnabled: state.viewer.featureTooltipsEnabled
    };
}

// Have to type our own dispatcher here as the default dispatcher is strongly-typed against a fixed set of
// viewer built-in actions
type OurReduxDispatch = (action: { type: string, payload?: any }) => void;

function mapDispatchToProps(dispatch: OurReduxDispatch): Partial<IDemoComponentDispatch> {
    return {
        setActiveTool: (tool) => dispatch(MapActions.setActiveTool(tool)),
        setTooltipsEnabled: (enabled) => dispatch(MapActions.setFeatureTooltipsEnabled(enabled)),
        addMessage: (message) => dispatch(MessageActions.addMessage(message)),
        clearMessages: () => dispatch(MessageActions.clearMessages())
    };
}

export class DemoComponent extends React.Component<DemoComponentProps, any> {
    private fnSetSelect: () => void;
    private fnSetPan: () => void;
    private fnSetZoom: () => void;
    private fnToggleTooltips: () => void;
    private fnMessageChanged: (e: any) => void;
    private fnreadfromURL: (e: any) => void;
    private fnAddMessage: () => void;
    private fnClearMessages: () => void;
    constructor(props: DemoComponentProps) {
        super(props);
        this.fnSetSelect = this.onSetSelect.bind(this);
        this.fnSetPan = this.onSetPan.bind(this);
        this.fnSetZoom = this.onSetZoom.bind(this);
        this.fnToggleTooltips = this.onToggleTooltips.bind(this);
        this.fnMessageChanged = this.onMessageChanged.bind(this);
        this.fnreadfromURL = this.readfromURL.bind(this); 
        this.fnAddMessage = this.onAddMessage.bind(this);
        this.fnClearMessages = this.onClearMessages.bind(this);
        this.state = {
            message: ""
        };
    }
    private onSetSelect() {
        if (this.props.setActiveTool) {
            this.props.setActiveTool(ActiveMapTool.Select);
        }
    }
    private onSetPan() {
        if (this.props.setActiveTool) {
            this.props.setActiveTool(ActiveMapTool.Pan);
        }
    }
    private onSetZoom() {
        if (this.props.setActiveTool) {
            this.props.setActiveTool(ActiveMapTool.Zoom);
        }
    }
    private onToggleTooltips() {
        if (this.props.setTooltipsEnabled) {
            this.props.setTooltipsEnabled(!!!this.props.tooltipsEnabled);
        }
    }
    private onMessageChanged(e: any) {
        this.setState({ message: e.target.value });
    }
    private async readfromURL(e: any) {
        //let a=await data();
        //this.setState({ message: a });
        //this.onAddMessage();
        const apiEndpoint = config.apiEndpoint;

        //http("http://112.115.117.10/pcc_webapi/api/Cartografiaitems").then(text => {
            //text; // => 'Page not found'
        http(apiEndpoint + "Cartografiaitems").then(text => {
            this.setState({ message: text });
            this.onAddMessage();
          }).catch(error => {
            this.setState({ message: error.message}); // 'An error has occurred: 404'
            this.onAddMessage();
          });

    }
    private onAddMessage() {

       
        if (this.props.addMessage) {
            this.props.addMessage(this.state.message);
            this.setState({ message: "" });
        }
    }
    private onClearMessages() {
        if (this.props.clearMessages) {
            this.props.clearMessages();
        }
    }
    render(): JSX.Element {
        const {
            activeMapName,
            scale,
            selectedLayerCount,
            selectedFeatureCount,
            activeTool,
            tooltipsEnabled
        } = this.props;
        const popoverContent = (
            <div style={{ maxHeight: 350, overflowY: "scroll" }}>
                <h5>About</h5>
                <p>If you are seeing this message. Congratulations! You have just rendered this component through the use of existing InvokeURL commands.</p>
                <p>This component was rendered from clicking an InvokeURL command with a URI of <strong>component://DemoComponent</strong></p>
                <p>This component is connected the redux application state tree. Watch this component automatically update in response to various change in application state</p>
                <p>The toolbar buttons in this component dispatch redux actions to push new application state, causing components connected to the redux application state tree (including this one) to automatically update</p>
                <p>This sample application also demonstrates custom application reducers. This component and the messages component use the same messages state. This component pushes messages to it (and clears messages). The messages component displays messages pushed to it.</p>
                <button className="pt-button pt-popover-dismiss">Dismiss</button>
            </div>
        );
        return <div>
            <div className="pt-card">
                <Popover
                    content={popoverContent}
                    interactionKind={PopoverInteractionKind.CLICK}
                    popoverClassName="pt-popover-content-sizing"
                    position="auto">
                    <button className="pt-button pt-intent-primary">About</button>
                </Popover>
            </div>
            <div className="pt-card">
                <p>Map Name: {activeMapName ? activeMapName : "<not set>"}</p>
                <p>Current Scale: {scale ? `${scale}` : "<not set>"}</p>
                <p>Selected: {selectedLayerCount && selectedFeatureCount ? `${selectedFeatureCount} features in ${selectedLayerCount} layers` : "none"}</p>
                <p>Active Tool: {activeTool ? ActiveMapTool[activeTool] : "none"}</p>
            </div>
            <div className="pt-card" style={{ height: "50px" }}>
               {/* <div className="pt-button-group pt-large">
                    <a onClick={this.fnSetSelect} title="Select" className={`pt-button pt-icon-select ${activeTool == ActiveMapTool.Select ? "pt-active" : ""}`} role="button"></a>
                    <a onClick={this.fnSetZoom} title="Zoom" className={`pt-button pt-icon-zoom-in pt-active ${activeTool == ActiveMapTool.Zoom ? "pt-active" : ""}`} role="button"></a>
                    <a onClick={this.fnSetPan} title="Pan" className={`pt-button pt-icon-hand ${activeTool == ActiveMapTool.Pan ? "pt-active" : ""}`} role="button"></a>
                    <a onClick={this.fnToggleTooltips} title="Feature Tooltips Enabled" className={`pt-button pt-icon-comment ${!!tooltipsEnabled ? "pt-active" : ""}`} role="button"></a>
        </div>*/}
                <div className="pt-button-group pt-large">
                    <button type="button" onClick={this.fnSetSelect} className={`pt-button pt-icon-select pt-active`}>Select</button>
                    <button type="button" onClick={this.fnSetZoom} className={`pt-button pt-icon-zoom-in pt-active`}>Zoom</button>
                    <button type="button" onClick={this.fnSetPan} className={`pt-button pt-icon-hand pt-active`}>Pan</button>
                    <button type="button" onClick={this.fnToggleTooltips} className={`pt-button pt-icon-comment pt-active`}>Feature Tooltips Enabled</button>
                </div>
            </div>
            <div className="pt-card">
                <p>Open the messages component (from the main toolbar) to see messages added from here</p>
                <textarea value={this.state.message} style={{ width: "100%" }} onChange={this.fnMessageChanged} />
                <div className="pt-button-group pt-large">
                    <button type="button" className="pt-button pt-intent-primary" onClick={this.fnAddMessage}>Add</button>
                    <button type="button" className="pt-button pt-intent-primary" onClick={this.fnClearMessages}>Clear</button>
                    <button type="button" className="pt-button pt-intent-primary" onClick={this.fnreadfromURL}>URL</button>
                </div>
            </div>
        </div>;
    }
}



export async function http(
    request: RequestInfo
  ): Promise<any> {
    const response = await fetch(request);
    const body = await response.text();
    return body;
  }

  // example consuming code
const data1 = async () => { 

    setTimeout(function () {
      return "teste";},5000);
    
    //await http(
    //"https://jsonplaceholder.typicode.com/todos"
   // "http://192.168.99.1:5567/todos.html"
 // );

  
};

function data (){
    return new Promise(
        resolve => { resolve (http("http://112.115.117.10/pcc_webapi/WeatherForecast"));
            //setTimeout(() => { resolve ("teste msg depois de 5 segundos");},3000);
    });

    http("https://jsonplaceholder.typicode.com/todos").then(text => {
        text; // => 'Page not found'
      }).catch(error => {
        error.message; // 'An error has occurred: 404'
      });
}


export default connect(mapStateToProps, mapDispatchToProps)(DemoComponent);