import { IApplicationState, ReduxDispatch, PlaceholderComponent, DefaultComponentNames, ToolbarContainer, TOOLBAR_BACKGROUND_COLOR, DEFAULT_TOOLBAR_SIZE, ViewerApiShim, ModalLauncher, FlyoutRegionContainer, WEBLAYOUT_TOOLBAR } from "mapguide-react-layout";
import * as React from "react";
import { useEffect } from 'react';
import { useSelector } from 'react-redux'; 
import { connect } from "react-redux"; 
import { Spinner, Intent, Icon } from '@blueprintjs/core';
import {SidebarTab, PCCSideBar}  from "../components/PCCSideBar"; 
import FormLogin from "../components/login/FormLogin";
import FormDummy from "../components/login/FormDummy";
import {IAppPccReducerState1} from "../reducers/app_pcc_reducer";   
import AppsIcon from '@mui/icons-material/Apps';
import * as Actions from "../constants/actions";
import {PCCMapLoadIndicator} from "../components/pcc_comp/PCCMapLoadIndicator"; 
import {Modal_FicheirosAssociadosGeral} from "../components/model/modalficheirosassociadosgeral";
import {ModalFormGU_MapaZoomCoordenadas} from "../components/model/modalmapazoomcoordenadas";
import {ModalFormGU_MapaDuploClick} from "../components/model/modalmapaduploclick";  
import {PCC_Modal_CartografiaOrdenar} from "../components/model/pcc_modalcartografiaordenar"; 
import {PCC_Modal_TalhaoOrdenar} from "../components/model/pcc_modaltalhaoordenar"; 
import {Modal_Coordenadas} from "../components/model/modalcoordenadas"; 
import {PCC_ModalMoverConstrucao} from "../components/model/pcc_modalmoverconstrucao"; 
import {Container_Pedidos} from "../components/pedidosapi/container_pedidos"; 
// ************************* PCC
import {PCC_ModalTiposMovimento} from "../components/model/pcc_modaltiposmovimento"; //pcc
import {PCC_ModalTiposConstrucao} from "../components/model/pcc_modaltiposconstrucao"; //PCC
import {PCC_ModalGestaoConcessionarios} from "../components/model/pcc_modalgestaoconcessionarios"; //PCC
import {PCC_ModalCemiterio} from "../components/model/pcc_modalcemiterio"; //PCC
import {PCC_ModalTalhao} from "../components/model/pcc_modaltalhao"; //PCC
import {PCC_ModalConstrucao} from "../components/model/pcc_modalconstrucao"; //PCC
import {PCC_ModalConstrucaoImportarFicheiro} from  "../components/model/pcc_modalconstrucaoimportarficheiros"; //PCC
import {PCC_ModalCartografiaItem} from "../components/model/pcc_modalcartografiaitem"; //PCC
import {PCC_ModalRegistoAssociarFicheiros} from "../components/model/pcc_modalregistoassociarficheiros"; //PCC
import {ModalPesquisa_ConstrucaoCampos} from "../components/model/pcc_modalpesquisa_construcaocampos"; // PCC
// ************************* PCC FIM   
import {Modal_CoordinateTracker} from "../components/coordinatetracker/modal_coordinatetracker";
import {ToolbarGISContainer} from "../components/toolbar_gis/toolbar_gis";  
import PCCToolbarComponent from "../components/toolbar_gis/pcctoolbar";
import '@fortawesome/fontawesome-svg-core/styles.css'; 
const SIDEBAR_WIDTH = 250;
const TASKPANE_HEIGHT = 500;
export const DEFAULT_CABECALHO_SIZE = 48;
import { initializeIcons } from '@fluentui/font-icons-mdl2';
initializeIcons();
/**
 * This interface defines any props that can be passed into the component
 *
 * @export
 * @interface IPCCLayoutTemplateProps
 */
export interface IPCCLayoutTemplateProps { }
/**
 * This interface defines observable redux state that is automatically updated when new state is
 * dispatched to the redux store. This is how the component can automatically render/update based
 * on change of state in the redux store.
 *
 * The function mapStateToProps is used to project the redux state tree to the properties defined
 * in this interface
 * @export
 * @interface IPCCLayoutTemplateState
 */
export interface IPCCLayoutTemplateState { 
    isBusy: boolean;
    locale: string;
    mapastate: any;
    toolbar: any;
    config: any;
    aplicacao_titulo: string;
    aplicacao_sigla: string;
    aplicacao_start: boolean;
    userid: string;
    username: string;
    usersession: string; 
    separadoresnomes: string[];
    separadoressiglas: string[];
    separadoresids: string[];
    separadoresdata: string[];
    form_pretensaonew_paiId: string;
    form_pretensaoedit_recId: string;
    form_atendimentoedit_recId: string;  
    form_atendimentonew_paiId: string; 
}
/**
 * This interfaces defines actions that can push new state to the redux store
 *
 * The function mapDispatchToProps is used to map the actions to invoke the redux
 * dispatcher that is passed
 *
 * @export
 * @interface IPCCLayoutTemplateDispatch
 */
export interface IPCCLayoutTemplateDispatch { }
/**
 * The react component consists of props and state. As the above interface are all part of the component's props, this type alias
 * is a convenient descriptor of the props as a whole, an intersection type of the above interfaces. The Partial<> type wrapper
 * means that all members of this intersection type are optional.
 */
export type PCCLayoutTemplateProps = Partial<IPCCLayoutTemplateProps & IPCCLayoutTemplateState & IPCCLayoutTemplateDispatch>;

/**
 * This function projects parts of the redux state tree into the target properties of our component state interface
 *
 * @param {IApplicationState} state The redux state tree
 * @returns {Partial<IPCCLayoutTemplateState>}
 */
function mapStateToProps(state: IAppPccReducerState1) : Partial<IPCCLayoutTemplateState> {
    return { 
        isBusy: state.viewer.busyCount > 0,
        locale: state.config.locale,
        toolbar: state.toolbar,
        mapastate: state.mapState,
        config: state.aplicacaopcc.config,
        aplicacao_titulo: state.aplicacaopcc.aplicacao_titulo,
        aplicacao_sigla: state.aplicacaopcc.aplicacao_sigla,
        aplicacao_start: state.aplicacaopcc.aplicacao_start,
        userid: state.aplicacaopcc.geral.userid,
        username: state.aplicacaopcc.geral.username,
        usersession: state.aplicacaopcc.geral.usersession, 
        separadoresnomes: state.aplicacaopcc.geral.separadoresnomes,
        separadoressiglas: state.aplicacaopcc.geral.separadoressiglas,
        separadoresids: state.aplicacaopcc.geral.separadoresids,
        separadoresdata: state.aplicacaopcc.geral.separadoresdata,         
        form_pretensaonew_paiId: state.aplicacaopcc.form_pretensaonew_paiId,
        form_pretensaoedit_recId: state.aplicacaopcc.form_pretensaoedit_recId, 
        form_atendimentoedit_recId: state.aplicacaopcc.form_atendimentoedit_recId,  
        form_atendimentonew_paiId: state.aplicacaopcc.form_atendimentonew_paiId, 
    };
}
type OurReduxDispatch = (action: { type: string, payload?: any }) => void;
/**
 * This function maps actions of our component dispatch interface to calls to the provided
 * redux action dispatcher function.
 *
 * As this component does not push any state, this interface has no actions and thus this
 * function returns an empty object
 *
 * @param {ReduxDispatch} dispatch
 * @returns {IPCCLayoutTemplateDispatch}
 */ 
function mapDispatchToProps(dispatch: OurReduxDispatch): IPCCLayoutTemplateDispatch {
    return {	};
}
/**
 * This react component represents our example viewer template. It subscribes to state in
 * the redux store via the connect() function below
 *
 * @export
 * @class PCCLayoutTemplate
 * @extends {React.Component<PCCLayoutTemplateProps, any>}
 */
export class PCCLayoutTemplate extends React.Component<PCCLayoutTemplateProps, any> {
    //private fnhidePretensao: () => void;
    constructor(props: PCCLayoutTemplateProps) {
        super(props); 
    }
    render(): JSX.Element | null {
        const { locale, isBusy, aplicacao_sigla, userid, usersession, aplicacao_titulo, aplicacao_start, form_pretensaonew_paiId, form_pretensaoedit_recId, form_atendimentoedit_recId, form_atendimentonew_paiId, config } = this.props;  
        const MyComponent = () => { 
            const { 
                separadoresids, 
                separadoressiglas, 
                separadoresnomes, 
                separadorestooltips, 
                separadoresdata,
            } = useSelector((state: IAppPccReducerState1) => ({
                separadoresids: state.aplicacaopcc.geral.separadoresids,
                separadoressiglas: state.aplicacaopcc.geral.separadoressiglas,
                separadoresnomes: state.aplicacaopcc.geral.separadoresnomes,
                separadorestooltips: state.aplicacaopcc.geral.separadorestooltips, 
                separadoresdata: state.aplicacaopcc.geral.separadoresdata,
            }));        
            React.useEffect(() => {
                // Este useEffect será executado quando as dependências mudarem                
            }, [separadoresids, separadoressiglas, separadoresnomes, separadorestooltips, separadoresdata]);
        
            return ( 
                // ... use as props mapeadas aqui ... 
                <div>
                </div>
            );
        };
        if (aplicacao_titulo=="login") {
            return <div className="loginpage" > 
                <div className="logocontainer"> 
                    <img src="static/toplogo.png" alt="Image" className="top-right"></img> 
                </div>
                <div className="logincontainer"> 
                    <FormLogin />
                    <FormDummy />
                </div>
            </div>;
        } else {
        return <div style={{ width: "100%", height: "100%" }}>
                {
                <ToolbarGISContainer id={WEBLAYOUT_TOOLBAR} containerStyle={{ display: "flex", position: "absolute", top: 50, right: 10, zIndex: 9999, backgroundColor: TOOLBAR_BACKGROUND_COLOR }} />
                }  
            <div id='cab' className="cabtitle"  style={{ display: "flex", alignItems: "center", position: "relative", left: 0, top: 0, right: 0, height: DEFAULT_CABECALHO_SIZE  }}>  
                <div className="logocab" > 
                    <img src="static/gismatlogo.png" alt="Image" className="top-left"></img> 
                </div>  
                <div className={`apps apps_${aplicacao_sigla?.toLowerCase()}`} > 
                    <a role="tab">
                        <AppsIcon  style={{ verticalAlign : "middle" }} />
                    </a>
                </div> 
                <div className={`regiaonome ${aplicacao_sigla?.toLowerCase()}`}>
                    <div className="app_name"> 
                    <a className="nametop-left" title={aplicacao_titulo}>{aplicacao_titulo}</a>
                    </div>
                </div>                
            </div>            
            <div className="app_toolbar"> 
                <div className="regiaopesquisa">
                    <div className="app_pesquisa"> 
                        {/* Aqui pode existir um componente de pesquida global*/} 
                    </div>
                </div>                
                <PCCToolbarComponent  />
            </div>            
            <div style={{ position: "absolute", left: 0, top: DEFAULT_CABECALHO_SIZE, bottom: 0, right: 0 }}>
                {/* The map component */}
                <PlaceholderComponent id={DefaultComponentNames.Map} locale={locale} />
                {/* The mouse coordinates display component */}
                <PlaceholderComponent id={DefaultComponentNames.MouseCoordinates} locale={locale} />
                {/* The scale display component */}
                <PlaceholderComponent id={DefaultComponentNames.ScaleDisplay} locale={locale} />
                {/* The base layer switcher component */}
                <PlaceholderComponent id="BaseMapSwitcher" locale="pt" componentProps={{ inlineBaseLayerSwitcher: true }} />  
                {aplicacao_start && <PCCSideBar />}
                <PCCMapLoadIndicator isBusy={isBusy || false}  />
            </div> 
            <Modal_CoordinateTracker/> 
            <PCC_ModalCartografiaItem/> {/*Janela de  Cartografia - Edição e Novas*/}
            <PCC_ModalRegistoAssociarFicheiros/>  
            <PCC_Modal_CartografiaOrdenar/>             
            <Modal_Coordenadas/> {/*Janela mostra coordenadas nos diversos sistemas */}
            <Container_Pedidos/> {/*Janela de Pedidos API - Em Cursos  */}            
            <ModalPesquisa_ConstrucaoCampos/> {/*Janela de Pesquisa de Construções */}  
            <PCC_ModalCemiterio/> {/*Janela de Cemitério - Edição e Novas*/}
            <PCC_ModalTalhao/> {/*Janela de Talhão - Edição e Novas*/}
            <PCC_ModalConstrucao/> {/*Janela de Construção - Edição e Novas*/}
            <PCC_ModalConstrucaoImportarFicheiro/> {/*Janela de Importação de objectos geográficos para Construção*/}
            <PCC_ModalMoverConstrucao/> {/*Janela para Mover Construções */}
            <PCC_ModalGestaoConcessionarios/> {/*Janela de Gestão de Concessionarios - Edição e Novas*/}
            <PCC_ModalTiposMovimento/> {/*Janela de Tipos de Movimentos - Edição e Novas*/}
            <PCC_ModalTiposConstrucao/>  {/*Janela de Tipos de Construção - Edição e Novas*/}
            <Modal_FicheirosAssociadosGeral/> {/*Janela de Cemitério - Edição e Novas*/}                  
            <ModalFormGU_MapaDuploClick/> 
            <ModalFormGU_MapaZoomCoordenadas/>         
            {/*This component automatically installs polyfill APIs for the AJAX viewer and partial support for the Fusion API, such APIs
            are accessible from Task Pane content.*/}
            <ViewerApiShim />
            {/*This component services modal content display requests and displays such content in floating modal dialogs when requested*/}
            <ModalLauncher />
            {/*This component services request for displaying flyout menus*/}
            <FlyoutRegionContainer />
            <MyComponent/>
        </div>
    }
    }
}
/**
 * This connects to the redux store and returns the decorated viewer template component
 */
export default connect(mapStateToProps, mapDispatchToProps)(PCCLayoutTemplate);