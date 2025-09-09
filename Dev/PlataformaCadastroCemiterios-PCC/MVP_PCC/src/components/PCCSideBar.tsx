 
import * as React from "react";
import * as Actions from "../constants/actions";

//import Api from "../api/api";
import isMobile from "ismobilejs";

import { Spinner, Intent, Icon } from '@blueprintjs/core';

import { IElementState, ViewerAction } from 'mapguide-react-layout/lib/actions/defs';
import { GenericEvent, ITemplateReducerState } from "mapguide-react-layout/lib/api/common";
import { setElementStates } from 'mapguide-react-layout/lib/actions/template';
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ActionType } from 'mapguide-react-layout/lib/constants/actions';
import { isElementState } from 'mapguide-react-layout/lib/reducers/template';
import { PlaceholderComponent, DefaultComponentNames } from "mapguide-react-layout/lib/api/registry/component";

import IconWithLetter from './icon/IconWithLetter';

import {TreeCartografia} from "./pcc_comp/TreeCartografia"
//import {TreePersonalizada} from "./pcc_comp/TreePersonalizada"
//import {TreeInstrumentos} from "./pcc_comp/TreeInstrumentos"

//import {TreePOI} from "./pcc_comp/POIs/TreePOI"
//import {GridPOI} from "./pcc_comp/POIs/GridPOI" 

import {TreeCEM} from "./pcc_comp/CEM/TreeCEM"
import {GridCEM} from "./pcc_comp/CEM/GridCEM"
import {MenuCEM} from "./pcc_comp/CEM/MenuCEM"
import {GridCEMContextMenu} from "./pcc_comp/CEM/GridCEMContextMenu";  

//import {TreeATE} from "./pcc_comp/ATE/TreeATE"
//import {GridATE} from "./pcc_comp/ATE/GridATE" 

export type SidebarTab = "EXT" | "CAR" |  "CEM" ; 
import { useSelector } from 'react-redux';
import {IAppPccReducerState1} from "../reducers/app_pcc_reducer"  

export interface IPCCSideBarProps {
    /*data : string[] | undefined;
    loading : boolean;
    error : string; 
    listTab: string[] | undefined;
    activeTab: string;
    collapsed: boolean; 
    busy: boolean;
    position: "left" | "right";
    onExpand: () => void;
    onCollapse: () => void; */
    onActivateTab: (tab: string | SidebarTab, collapsed?: boolean) => void;
} 
function sidebarTemplateReducer(origState: ITemplateReducerState, state: ITemplateReducerState, action: ViewerAction): ITemplateReducerState {
    
    switch (action.type) {
        case ActionType.MAP_SET_SELECTION:
            {
                //This is the only template that does not conform to the selection/legend/taskpane is a mutually
                //exclusive visible set. We take advantage of the custom template reducer function to apply the
                //correct visibility state against the *original state* effectively discarding whatever the root
                //template reducer has done against this action.
                const { selection } = action.payload;
                if (selection && selection.SelectedFeatures) {
                    let autoExpandSelectionPanel = origState.autoDisplaySelectionPanelOnSelection;
                    const ism = isMobile(navigator.userAgent);
                    if (ism.phone) {
                        return origState; //Take no action on mobile
                    }
                    if (selection.SelectedFeatures.SelectedLayer.length && autoExpandSelectionPanel) {
                        return {
                            ...origState,
                            ...{ selectionPanelVisible: true, taskPaneVisible: false, legendVisible: false }
                        }
                    }
                }
                return state; //No action taken: Return "current" state
            }
        case ActionType.MAP_ADD_CLIENT_SELECTED_FEATURE:
            {
                //This is the only template that does not conform to the selection/legend/taskpane is a mutually
                //exclusive visible set. We take advantage of the custom template reducer function to apply the
                //correct visibility state against the *original state* effectively discarding whatever the root
                //template reducer has done against this action.
                const { feature } = action.payload;
                if (feature?.properties) {
                    let autoExpandSelectionPanel = origState.autoDisplaySelectionPanelOnSelection;
                    const ism = isMobile(navigator.userAgent);
                    if (ism.phone) {
                        return origState; //Take no action on mobile
                    }
                    if (autoExpandSelectionPanel) {
                        return {
                            ...origState,
                            ...{ selectionPanelVisible: true, taskPaneVisible: false, legendVisible: false }
                        }
                    }
                }
            }
        case ActionType.FUSION_SET_LEGEND_VISIBILITY:
            {
                const data = action.payload;
                if (typeof (data) == "boolean") {
                    let state1: Partial<ITemplateReducerState>;
                    if (data === true) {
                        state1 = { legendVisible: true, taskPaneVisible: false, selectionPanelVisible: false };
                    } else {
                        state1 = { legendVisible: data };
                    }
                    return { ...state, ...state1 };
                }
            }
        case ActionType.FUSION_SET_SELECTION_PANEL_VISIBILITY:
            {
                const data = action.payload;
                if (typeof (data) == "boolean") {
                    let state1: Partial<ITemplateReducerState>;
                    if (data === true) {
                        state1 = { legendVisible: false, taskPaneVisible: false, selectionPanelVisible: true };
                    } else {
                        state1 = { selectionPanelVisible: data };
                    }
                    return { ...state, ...state1 };
                }
            }
        case ActionType.TASK_INVOKE_URL:
            {
                let state1: Partial<ITemplateReducerState> = { taskPaneVisible: true, selectionPanelVisible: false, legendVisible: false };
                return { ...state, ...state1 };
            }
        case ActionType.FUSION_SET_TASK_PANE_VISIBILITY:
            {
                const data = action.payload;
                if (typeof (data) == "boolean") {
                    let state1: Partial<ITemplateReducerState>;
                    if (data === true) {
                        state1 = { legendVisible: false, taskPaneVisible: true, selectionPanelVisible: false };
                    } else {
                        state1 = { taskPaneVisible: data };
                    }
                    return { ...state, ...state1 };
                }
            }
        case ActionType.FUSION_SET_ELEMENT_STATE:
            {
                const data = action.payload;
                if (isElementState(data)) {
                    return { ...state, ...data };
                }
            }
    }
    return state;
}
const SidebarHeader = (props: any) => {
    const sbHeaderStyle: React.CSSProperties = {
        position: "absolute",
        top: 0,
        right: 0,
        height: 40,
        left: 0,
        margin: 0
    };
    sbHeaderStyle.paddingLeft = 10;
    return <h1 style={sbHeaderStyle} className="sidebar-header">
        {props.text}
        <span className="sidebar-close" onClick={props.onCloseClick}><i className="fas fa-angle-double-left"></i></span>
    </h1>;
};
const SidebarPREHeader = (props: any) => {
    const sbHeaderStyle: React.CSSProperties = {
        position: "absolute",
        top: 0,
        right: 0,
        height: 40,
        left: 0,
        margin: 0
    };
    sbHeaderStyle.paddingLeft = 10;
    return <h1 style={sbHeaderStyle} className="sidebar-header">
        {props.text}
        <span className="sidebar-pre-search" title="Pesquisas" onClick={props.onSearchClick}><i className="fa fa-search"></i></span>        
        <span className="sidebar-pre-close" title="Colapsar" onClick={props.onCloseClick}><i className="fas fa-angle-double-left"></i></span>
    </h1>;
};
const styles = {
    container: {
        top: 50,
        left: 50,
        width: 300,
        height: 300, 
        backgroundColor: "orange",
        display: "block",
    },
  } as const;

 
export const PCCSideBar = () => {

    const {
        dispatch,
        locale,
        capabilities,
        showSelection,
        showLegend,
        showTaskPane,
    } = useCommonTemplateState(sidebarTemplateReducer);
 
    const sep_app = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.sep_app);
    const sep_write = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.sep_write);
    const sep_read = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.sep_read);
   
    const separadoresnomes = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.separadoresnomes);
    const separadoresids = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.separadoresids);
    const separadoressiglas = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.separadoressiglas);
    const separadoresdata = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.separadoresdata);
    const separadorestooltips = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.separadorestooltips);

    const localiza_separador_cem = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.localiza_na_arvore.localiza_separador_cem); 
    const open_separador = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.localiza_na_arvore.sigla_separador);

    const [data, setData] = React.useState([]);
    const [loading, setLoading] = React.useState(false);
    const [error, setError] = React.useState('');
    const [activeTab, setActiveTab] = React.useState('CAR');
    //const [listTab, setTabs] = React.useState(initialTabs);
    const [collapsed, setcollapsed] = React.useState(false);
    const [position, setposition] = React.useState('left');
    const [listTab, setTabs] = React.useState( useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.separadoressiglas));

    const setElementStatesAction = (states: IElementState) => dispatch(setElementStates(states));
    const onActivateLayersExternos= (e: GenericEvent) => {
     
        e.preventDefault();
        setActiveTab("EXT");

        onActivateTab("EXT");
        return false;
    };
    const onActivateCartografia= (e: GenericEvent) => {
     
        e.preventDefault();
        setActiveTab("CAR");

        onActivateTab("CAR");
        return false;
    };

    
    const onActivateSeparador =  (index: string) => (e: GenericEvent) => {
     
        e.preventDefault();
        setActiveTab(index);
 
        onActivateTab(index);
        return false;
    };
    
    const onActivateConstrucoes= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("CEM");
        onActivateTab("CEM");
        return false;
    };
    const onActivateAtendimento= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("ATE");
        onActivateTab("ATE");
        return false;
    };
    const onActivateFiscalizacao= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("FIS");
        onActivateTab("FIS");
        return false;
    };
    const onActivateCPropriedade= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("CPR");
        onActivateTab("CPR");
        return false;
    };
    const onActivatePatrimonio= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("PAT");
        onActivateTab("PAT");
        return false;
    };
    const onActivateEducacao= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("EDU");
        onActivateTab("EDU");
        return false;
    };
    const onActivateRVMU= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("RVMU");
        onActivateTab("RVMU");
        return false;
    };
    const onActivateRVNOS= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("RVNOS");
        onActivateTab("RVNOS");
        return false;
    };
    const onActivateRVNP= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("RVNP");
        onActivateTab("RVNP");
        return false;
    };
    const onActivateRVOA= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("RVOA");
        onActivateTab("RVOA");
        return false;
    };
    const onActivateRVF= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("RVF");
        onActivateTab("RVF");
        return false;
    };
    const onActivateRVR= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("RVR");
        onActivateTab("RVR");
        return false;
    };
    const onActivateRVS= (e: GenericEvent) => {
      
        e.preventDefault();
       
        setActiveTab("RVS");
        onActivateTab("RVS");
        return false;
    };  
    
    
    const onOpenPontoInteresseIdClick = (e: GenericEvent) => {         
        e.preventDefault();
        //onOpenPontoInteresseId();
        return false;
    };
    const onOpenPontoInteresseId = () => { 
        //var aux={ type: Actions., payload: "" };
        //dispatch(aux);               
    };
    const onSearchPontoInteresseClick = (e: GenericEvent) => {         
        e.preventDefault();
        onSearchPontoInteresse();
        return false;
    };
    const onSearchPontoInteresse  = () => {
        //var aux={ type: Actions.SHOW_PESQUISA_PONTOINTERESSE , payload: "" };
       //dispatch(aux); 
    };
    const onOpenConstrucaoIdClick = (e: GenericEvent) => {         
        e.preventDefault();
        onOpenConstrucaoId();
        return false;
    };

    const onOpenConstrucaoId = () => { 
        var aux={ type: Actions.SHOW_PESQUISA_CONSTRUCAO_ID, payload: "" };
        dispatch(aux);               
    };
     
    const onSearchConstrucaoClick = (e: GenericEvent) => {         
        e.preventDefault();
        onSearchConstrucao();
        return false;
    };
    const onSearchConstrucao  = () => {
        var aux={ type: Actions.SHOW_PESQUISA_CONSTRUCAO , payload: "" };
        dispatch(aux); 
    };  
    const onClickCollapse = (e: GenericEvent) => {         
        e.preventDefault();
        onCollapse();
        return false;
    };
    const onClickExpand = (e: GenericEvent) => {
         
        e.preventDefault();
        onExpand();
        return false;
    };
    const onCollapse = () => {     
        setcollapsed(!collapsed);
    };
    const onExpand = () => {         
        setcollapsed(!collapsed);
    }     
    if (loading) {
        return <div>Loading</div>
    }     
    if (error) {
        return <div style={{color: 'red'}}>ERROR: {error}</div>
    } 
    const onActivateTab = (tab: string | SidebarTab) => {
        const est: IElementState = {
            legendVisible: false,
            selectionPanelVisible: false,
            taskPaneVisible: false
        };
        switch (tab) {
            case "CAR":
                est.legendVisible = true;
                break;            
        }
        if (est.legendVisible || est.selectionPanelVisible || est.taskPaneVisible) {
            setElementStatesAction(est);
            setActiveTab(tab);
        }
    }     
    React.useEffect(() => {
        async function fetchData() {
            try {
                const urla = apiEndpoint + 'Aplicacao';   
                const response = await fetch(urla);                 
                //const response = await Api.getAplicacoes();
                const json = await response.json(); 
             setData(json);
            } catch (e) {
                setError(e.message || 'Unexpected error');
            }
 
            setLoading(false);
        }
 
        //fetchData();
    }, []);

    React.useEffect(() => {
        if (localiza_separador_cem){
            console.log("CEM" );
            setActiveTab("CEM" );
            onActivateTab("CEM");          
            // Dispatch depois de 1 segundo
            setTimeout(() => {
                const aux = { type: Actions.SIDEBARLOCALIZA_END, payload: "" };
                dispatch(aux);
            }, 1000);

        }        

    }, [localiza_separador_cem]); 
   // const {position, busy, collapsed, activeTab } = props;
   React.useEffect(() => {

        setTabs(separadoressiglas);
   }, [separadoresids, separadoressiglas, separadoresnomes, separadorestooltips, separadoresdata]);
    
    const grupo_pretId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.grupo_pret_recid);
      
    const apiEndpoint = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpoint);
    const apiEndpointCadastro = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpointCadastro);
    const apiEndpointSIG = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpointSIG);
    const apiEndpointDocumentos = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpointDocumentos); 
    const authtoken = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.authtoken);
    const userid = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.userid);
    const usersession = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.usersession); 

    const treecartografia_data= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.treecartografia_data);
    const treeinstrumentos_data= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.treeinstrumentos_data);
    const treeseparador_data= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.treeseparador_data);

    const treepois_data= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.treepois_data);
     

    const treeconstrucoes_data= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.treeconstrucoes_data);
    const treeatendimento_data= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.treeatendimento_data);

    const indices: number[] = listTab.reduce((acc:number[], str, index) => {
        if (str.startsWith('SEP')) {
          acc.push(index);
        }
        return acc;
      }, []);

      const componentInstances  = [];
      for (let i = 0; i < indices.length; i++) {

          const auxsigla = "SEP" + indices[i];
          const auxtitle1 = separadorestooltips[indices[i]];

          const auxtitle = auxtitle1.split("@")[0];
          const auxtema = auxtitle1.split("@")[1];
          const ss=indices[i];
          const auxid = separadoresids[ss];
          //const apiEndpoint = config.apiEndpoint;
          
          //get separador data 
          var auxdata="";
          var auxstate="";
          for (let j = 0; j < separadoresdata.length; j++) {
            var aa= separadoresdata[j].split("{|}");
            if (aa[0]==auxid){ 
                auxdata=aa[1];
                if (aa[2]!=undefined){auxstate=aa[2];}
                j = separadoresdata.length;
            }
 
          }
        }

    return (
            <div className={`sidebar ${collapsed ? "collapsed" : ""} sidebar-${position}`}> 
                <div  className="sidebar-tabs">
                <ul role="tablist">
                    <li>
                        {(() => {                        
                            if (collapsed) {
                                return <a onClick={onClickExpand}><Icon icon="menu-open" /></a>;
                            } else {
                                return <a onClick={onClickCollapse}><Icon icon="menu-closed" /></a>;
                            }                    
                        })()}
                    </li>
                    {(() => {
                        if (listTab?.includes('CAR')  && sep_app?.includes('CAR')) {
                            return <li className={collapsed == false && activeTab == "CAR" ? "active" : ""}>
                              {/* <a onClick={onActivateCartografia} title={separadorestooltips[separadoressiglas.indexOf("CAR")]} role="tab">{separadoresnomes[separadoressiglas.indexOf("CAR")]}</a>*/}
                            <a onClick={onActivateCartografia} title={separadorestooltips[separadoressiglas.indexOf("CAR")]} role="tab">
                                <IconWithLetter icon="layers" letter="CAR" />
                                
                            </a>                                
                            </li>;
                        }
                    })()}                               
                    {(() => {
                        if (listTab?.includes('CEM')  && sep_app?.includes('CEM')) {
                            return <li className={collapsed == false && activeTab == "CEM" ? "active" : ""}> 
                                <a onClick={onActivateConstrucoes} title={separadorestooltips[separadoressiglas.indexOf("CEM")]} role="tab"><IconWithLetter icon="layers" letter="CEM" /></a>
                            </li>;
                        }
                    })()}   
                    {(() => {
                        if (listTab?.includes('EXT')&& sep_app?.includes('EXT'))   {
                            return <li className={collapsed == false && activeTab == "EXT" ? "active" : ""}>
                             <a onClick={onActivateLayersExternos} title="Layers Externos" role="tab">
                                <IconWithLetter icon="layers" letter="EXT" />
                                
                            </a>                                
                            </li>;
                        }
                    })()}
                </ul>
                </div> 
                <div className="sidebar-content">
                    {(() => {
                        if (listTab?.includes('EXT')) {
                            return <div className={`sidebar-pane ${activeTab == "EXT" ? "active" : ""}`}>
                                <SidebarHeader text="Layers Externos" onCloseClick={onClickCollapse} />
                                <div style={{ position: "absolute", top: 40, bottom: 0, right: 0, left: 0, overflow: "auto" }}> 
                                     <PlaceholderComponent id="AddManageLayers" locale="pt" componentProps={{  }} />  
                                </div>
                            </div>;
                        }
                    })()}  
                    {(() => {
                        if (listTab?.includes('CAR')) {
                            return <div className={`sidebar-pane ${activeTab == "CAR" ? "active" : ""}`}>
                                <SidebarHeader text={separadorestooltips[separadoressiglas.indexOf("CAR")]} onCloseClick={onClickCollapse} />
                                <div style={{ position: "absolute", top: 40, bottom: 0, right: 0, left: 0, overflow: "auto" }}>
                                    <TreeCartografia authtoken={authtoken} apiEndpoint={apiEndpoint} cartografiadata={treecartografia_data}/> 
                                </div>
                            </div>;
                        }
                    })()}  
                    {(() => {
                        if (listTab?.includes('CEM')) {
                            const alturadiv = (window.innerHeight - 100) / 2;
                            const topdiv = 40 + alturadiv;
                            return <div className={`sidebar-pane ${activeTab == "CEM" ? "active" : ""}`}>
                                <SidebarPREHeader text={separadorestooltips[separadoressiglas.indexOf("CEM")]} onOpenIdClick={onOpenConstrucaoIdClick} onSearchClick={onSearchConstrucaoClick} onCloseClick={onClickCollapse} /> 
                                <div style={{ position: "absolute", top: 40, height: alturadiv, right: 0, left: 0, overflow: "auto" }}>
                                    <TreeCEM authtoken={authtoken} userid={userid} usersession={usersession} apiEndpointCadastro={apiEndpointCadastro} apiEndpointSIG={apiEndpointSIG} construcoesdata={treeconstrucoes_data}/> 
                                </div>
                                <div style={{ position: "absolute", height: alturadiv - 5 , top: topdiv, right: 0, left: 0, overflow: "auto" }}>
                                    {/* <MenuGU/>  */}
                                    <GridCEM authtoken={authtoken} userid={userid} usersession={usersession} apiEndpointCadastro={apiEndpointCadastro} grupo_pretId={grupo_pretId}/> 
                                </div>
                            </div>;
                        }
                    })()}  
                </div>  
            </div>            
        );                      
} 
 

 