import config from '../../config.json';
import * as Actions from "../constants/actions";
import { IApplicationState } from "mapguide-react-layout/lib/api/common";
import { FeatureSet, LayerMetadata, QueryMapFeaturesResponse, SelectedFeature } from "mapguide-react-layout/lib/api/contracts/query";
import TreeCartografia_G from "../components/pcc_comp/TreeCartografia_G";
import TreeCEM_G from "../components/pcc_comp/CEM/TreeCEM_G"; 
import { IDatePickerStrings, anchorProperties } from '@fluentui/react';
import { string } from 'prop-types';
 
export interface IAppPccReducerState1 extends IApplicationState {
    aplicacaopcc: IAppPccReducerState; 
}

// Define the type of permission entries
type Permission = {
    cod: string;
    subCod: string;
    CodPai: string;
    SubCodPai: string;
    Tipo: string;
    perm: string;
};
 

export interface IAppPccReducerState { 
    grupo_poi_recid: string;
    grupo_pret_recid: string;
    grupo_ate_recid: string;
    grupo_fis_recid: string;
    grupo_edu_recid: string;
    grupo_pat_recid: string;
    grupo_cpr_recid: string;
    grupo_cpr_tipo: string;
    //grid_cpr_tipo: string;
    grupo_rvmu_recid: string;
    grupo_rvnos_recid: string;
    grupo_rvnp_recid: string;
    grupo_rvoa_recid: string;
    grupo_rvf_recid: string;
    grupo_rvr_recid: string;
    grupo_rvs_recid: string;
    grupo_rvs_tipo: string;
    //apiEndpoint: string;
    aplicacao_titulo: string;
    aplicacao_sigla: string;
    aplicacao_start: boolean;
    geral: ISeparadoresReducerState; 
    pedidos_api: IPedidosAPIReducerState; // Janela com a quantidade de pedidos à APi - Impressoes e Ficheiros (pedidos + demorados)

    dadosimpressoes: IDadosImpressoesReducerState;

    treepontosinteresse: ITreePontosInteresseReducerState;
    treeconstrucoes: ITreePretensoesReducerState;
    treecartografia: ITreeCartografiaReducerState;
    treeinstrumentos: ITreeInstrumentosReducerState; 
    treeatendimento: ITreeAtendimentoReducerState;
    treeseparador: ITreeSeparadorReducerState;    
    windows: IJanelasReducerState;

    reload_menui: IMenuImpReloadReducerState;  
    exportar_dwf: boolean;
    exportar_dxf: boolean;
    exportar_dados: string; 
    resetMenuIcons: boolean; // Desliga os icons no menu das cartas quando está a true
    layers_totais: string[]; // Array com todos os layers da cartografia e instrumentos
    layers_ligados: string[]; // Array com os layers ligados
    reset_layers: string[]; // Array com os layers a desligar

    referencia_treeconstrucoes: React.RefObject<typeof TreeCEM_G> | null; 

    referencia_treecartografia: React.RefObject<typeof TreeCartografia_G> | null;
    // Estas variaveis se tiverem dados definem que os respetivos formulários são apresentados 
 

    //Form do ITEM de CARTOGRAFIA
    form_cartografiaitemedit_recId: string; //não nulo edita grupo cartografia - rec_id do grupo de cartografia
    form_cartografiaitemnew_paiId: string; //não nulo novo grupo cartografia - rec_id do pai do grupo de cartografia 

    //Form do Grupo de INSTRUMENTOS
    form_instrumentosgrupoedit_recId: string; //não nulo edita grupo instrumentos - rec_id do grupo de instrumentos
    form_instrumentosgruponew_paiId: string; //não nulo novo grupo instrumentos - rec_id do pai do grupo de instrumentos  
    
    //Form do ITEM de INSTRUMENTOS
    form_instrumentositemedit_recId: string; //não nulo edita grupo instrumentos - rec_id do item de instrumentos
    form_instrumentositemnew_paiId: string; //não nulo novo grupo instrumentos - rec_id do pai do item de instrumentos  
     
    //Form de consulta do REGULAMENTO e ENTIDADES do ITEM de INSTRUMENTOS
    form_instrumentositemregulamento_recId: string; //não nulo apresenta o form de consulta- rec_id do item de instrumentos

    //Form do Grupo de POIs
    form_pontointeressegrupoedit_recId: string; //não nulo edita grupo pontointeresse - rec_id do grupo de pontointeresse
    form_pontointeressegruponew_paiId: string; //não nulo novo grupo pontointeresse - rec_id do pai do grupo de pontointeresse 

    //Form do POI
    form_pontointeresseedit_recId: string; //não nulo edita pontointeresse - rec_id da pontointeresse
    form_pontointeressenew_paiId: string; //não nulo nova pontointeresse - rec_id do pai da pontointeresse 
 

    //PCC - Form de Gestão Cemiterios
    form_cemiterioedit_recId: string; //não nulo edita cemiterio - rec_id do cemiterio

    //PCC - Form de Gestão Talhão
    form_talhaoedit_recId: string; //não nulo edita cemiterio - rec_id do talhao
    form_talhaonew_paiId: string; //não nulo nova pretensão - rec_id do pai da talhao (cemiterio) 

    //PCC - Form de Gestão Construção
    form_construcaoedit_recId: string; //não nulo edita construção - rec_id da construção
    form_construcaonew_paiId: string; //não nulo nova construção - rec_id do pai da construção (talhão) 
 

    //Form da Pretensao
    form_pretensaoedit_recId: string; //não nulo edita pretensão - rec_id da pretensão
    form_pretensaonew_paiId: string; //não nulo nova pretensão - rec_id do pai da pretensão 
 
    //Form do Grupo de ATENDIMENTO
    form_atendimentogrupoedit_recId: string; //não nulo edita grupo cartografia - rec_id do grupo de cartografia
    form_atendimentogruponew_paiId: string; //não nulo novo grupo cartografia - rec_id do pai do grupo de cartografia 

     //Form do ATENDIMENTO
    form_atendimentoedit_recId: string; //não nulo edita grupo atendimento - rec_id do atendimento
    form_atendimentonew_paiId: string; //não nulo novo grupo atendimento - rec_id do pai do atendimento 


    //Form das impressões - apresenta os campos a serem preenchidos pelos utilizadores
    form_impressoes_Id: string; // não nulo apresenta no form as variaveis - rec_id das impressoes (conjunto de cartas/definições de modelos)
    form_impressoesnew: string; // não nulo apresenta no form as variaveis - rec_id das impressoes (conjunto de cartas/definições de modelos)
 
    form_cartanew: string;
    form_cartaidparent: string;
    form_cartagruponew: string;
    //Form do Grupo de Separador
    form_separadorgrupoedit_recId: string; //não nulo edita grupo pretensão - rec_id do grupo de pretensão
    form_separadorgruponew_paiId: string; //não nulo novo grupo pretensão - rec_id do pai do grupo de pretensão 

    //Form do Item de Separador
    form_separadoritemedit_recId: string; //não nulo edita item separador - rec_id do item separador
    form_separadoritemnew_paiId: string; //não nulo novo item separador - rec_id do pai do item separador

    form_separadorid: string; //id tree Separador
 
    config: IConfigReducerState; 
    ligacoes_gu: ILigacoesGUReducerState; 
}
 
  
 
export interface ILigacoesGUReducerState {
    ligado_nopaper: boolean;
    ligado_medidata: boolean;
    ligado_airc: boolean;
    ligado_ano: boolean; 
}

export interface IPedidosAPIReducerState { // Ramo da Store de pedidos de impressão/ficheiros às APIs
    numero_pedidos: number; // numero de pedidos em processamento
    ficheiros: string[] ; // url de ficheiros para download
}
export interface ISeparadoresReducerState { // Ramo da Store de dados lidos no login
    authtoken: string;
    aplicacao_titulo: string;
    aplicacao_sigla: string; 
    userid: string;
    username: string;
    usersession: string;
    permissoes: string;
    funcionalidades: string[] ;

    sep_app : string[] ;
    sep_write: string[] ;
    sep_read : string[] ; 

    separadoresnomes: string[] ;
    separadorestooltips: string[] ;
    separadoresids: string[] ;
    separadoressiglas: string[] ;
    separadoresdata: string[] ;

    treeseparadores_itenschecked: string[]; // informações itens checked arvore

    mapa_layers_on: string[] ; 

    mapa_initialview_x: string ; 
    mapa_initialview_y: string ; 
    mapa_initialview_escala: string ; 

     // se estes array tiverem rec_id os mesmos sao colocados checked nas arvores  
    // utilizado nas definicoes iniciais  
    treecartografia_data: string[] ; // data da treecartografia - carregado no login
    treecartografia_init: string[] ; // elementos da treecartografia - ligados ao inicio
   
    treeinstrumentos_data: string[] ; // data da treeinstrumentos - carregado no login
    treeinstrumentos_init: string[] ; // elementos da treeinstrumentos - ligados ao inicio

    layers_iniciais: string[];
    
    treeseparador_data: string[] ; // data da treeinstrumentos - carregado no login
    //treeseparador_init_checked: string[] ; // elementos da treeseparador - ligados ao inicio
    treepois_data: string[]; // data da treepoi - carregado  no login
    treeconstrucoes_data: string[] ; // data da treeconstrucoes  - carregado no login
    treeatendimento_data: string[] ; // data da treeatendimento - carregado no login
    treecadastro_data: string[] ;
}
export interface ITreeCartografiaReducerState {  // Ramo da Store para atualizacao da arvore de Cartografia
    treecartografia_tipo: string ; // tipo de atualizacao: delete / update / insert
    treecartografia_rec_id: string ; // rec_id do grupo - diferente do id do elem na arvore 
    treecartografia_nome: string ; // nome do grupo - value do elem na arvore
    treecartografia_parent_recid: string ; // rec_id do pai do grupo, essencial no insert
    treecartografia_layers: string ; 
    treecartografia_legenda: string ; 
    treecartografia_objtipo: string ; // GRUPO OU ITEM
    reloadKey: string ; 
    // Este array tem os rec_id dos elementos checked na arvore 
    // utilizado para gravar as definicoes iniciais e nas cartas
    treecartografia_itenschecked: string[]; // informações itens checked arvore - rec_id - diferente do id do elem na arvore
    treecartografia_setcheck: string[] ; 
 
}

export interface IDadosImpressoesReducerState {  
    // Este array tem os rec_id dos elementos checked na arvore  
    aux_campos: string[]; // informações campos
    aux_value: string[]; // informações value campos
}

export interface ITreePontosInteresseReducerState {  // Ramo da Store para atualizacao da arvore de Pontos de Interesse
    treepontosinteresse_tipo: string ; // tipo de atualizacao: delete / update / insert
    treepontosinteresse_rec_id: string ; // rec_id do grupo - diferente do id do elem na arvore
    treepontosinteresse_nome: string ; // nome do grupo - value do elem na arvore
    treepontosinteresse_parent_recid: string ; // rec_id do pai do grupo, essencial no insert
    reloadKey: string ; 
    // Este array tem os rec_id dos elementos checked na arvore  
    treepontosinteresse_itenschecked: string[]; // informações itens checked arvore - rec_id - diferente do id do elem na arvore
}
export interface ITreePretensoesReducerState {  // Ramo da Store para atualizacao da arvore de Pretensoes
    treeconstrucoes_tipo: string ; // tipo de atualizacao: delete / update / insert
    treeconstrucoes_objtipo: string ; // tipo de objecto pai: pai / cemiterio
    treeconstrucoes_rec_id: string ; // rec_id do grupo - diferente do id do elem na arvore
    treeconstrucoes_nome: string ; // nome do grupo - value do elem na arvore
    treeconstrucoes_parent_recid: string ; // rec_id do pai do grupo, essencial no insert
    reloadKey: string ; 
    // Este array tem os rec_id dos elementos checked na arvore  
    treeconstrucoes_itenschecked: string[]; // informações itens checked arvore - rec_id - diferente do id do elem na arvore
}
export interface IMenuImpReloadReducerState { 
    reloadtipo: string ; // tipo de atualizacao: delete / update / insert 
 
}
export interface ITreeInstrumentosReducerState { 
    treeinstrumentos_tipo: string ; // tipo de atualizacao: delete / update / insert 
    treeinstrumentos_rec_id: string ;
    treeinstrumentos_nome: string ;
    treeinstrumentos_parent_recid: string ;
    treeinstrumentos_layers: string ; 
    treeinstrumentos_legenda: string ; 
    treeinstrumentos_objtipo: string ; // GRUPO OU ITEM
    reloadKey: string ; 
     // Este array tem os rec_id dos elementos checked na arvore 
    // utilizado para gravar as definicoes iniciais e nas cartas
    treeinstrumentos_itenschecked: string[]; // informações itens checked arvore - rec_id - diferente do id do elem na arvore
    treeinstrumentos_setcheck: string[] ; 
 
}
export interface ITreeAtendimentoReducerState { 
    treeatendimento_tipo: string ; // tipo de atualizacao: delete / update / insert 
    treeatendimento_rec_id: string ;
    treeatendimento_nome: string ;
    treeatendimento_parent_recid: string ;
    reloadKey: string ; 
     // Este array tem os rec_id dos elementos checked na arvore  
     treeatendimento_itenschecked: string[]; // informações itens checked arvore - rec_id - diferente do id do elem na arvore
}
export interface ITreeSeparadorReducerState { 
    treeseparador_tipo: string ; // tipo de atualizacao: delete / update / insert 
    treeseparador_rec_id: string ;
    treeseparador_nome: string ;
    treeseparador_parent_recid: string ;

    treeseparador_layers: string ; 
    treeseparador_legenda: string ; 
    treeseparadorobjtipo: string ; // GRUPO OU ITEM

     // Este array tem os rec_id dos elementos checked na arvore 
    // utilizado para gravar as definicoes iniciais e nas cartas
    treeseparador_itenschecked: string[]; // informações itens checked arvore - rec_id - diferente do id do elem na arvore
    treeseparador_idarvore: string[];
    treeseparador_itemlayers: string[];
    treeseparador_setcheck: string[] ;
}
export interface IConfigReducerState {
    aplicationTokenId: string;
    startSessionId: string;
    configapiEndpoint: string;
    configapiEndpointSIG: string;
    configapiEndpointCadastro: string;
    configapiEndpointDocumentos: string;
    configmapagenturl: string;
    configmapa:  string;
}
export interface IJanelasReducerState {
    definir_cor: IJanelaDefinirCorReducerState;
    criar_buffer: IJanelaCriarBufferReducerState; 
    mapa_zoomcoordenadas: IJanelaMapaZoomCoordenadasReducerState;
    detalhes_objectos: IJanelaDetalhesObjetoReducerState;
    mapa_duploclick: IJanelaMapaDuploClickReducerState;  
    ficheiros_associados: IJanelaDetalhesFicheirosAssociadosReducerState;     
    pesquisa_pretensaoid: IJanelaPesquisaPretensaoIdReducerState;
    pesquisa_construcaocampos: IJanelaPesquisaPretensaoCamposReducerState;
    importa_construcao: IJanelaImportaConstrucaoReducerState;

    ordenaconstrucao : IJanelaOrdenaConstrucaoReducerState;
    moverconstrucao : IJanelaMoverConstrucaoReducerState;

    associa_ficheiro:  IJanelaAssociaFicheiroReducerState;
    detalhes_ficheiros:IJanelaFicheirosAssociadosReducerState;
    importa_desenho: IJanelaImportaDesenhoReducerState;
    gestao_concessionarios: IJanelaConcessionariosReducerState; 
    gestao_construcoes: IJanelaTiposConstrucaoReducerState;
    gestao_movimentos: IJanelaTiposMovimentoReducerState; 
    ordenacartografia : IJanelaOrdenaCartografiaReducerState;  

    coordenadas : IJanelaCoordenadasReducerState;
    trackcoordenadas : IJanelaTrackCoordenadasReducerState; 
    localiza_na_arvore : IJanelaLocalizaNaArvoreReducerState;
}
export interface  IJanelaLocalizaNaArvoreReducerState{ 
    localiza_separador_cem: boolean;
    localiza_arvore: boolean;
    localiza_grelha: boolean; 
    sigla_separador: string;
    rec_id_arvore: string;
    rec_id_grelha: string;
}
 
export interface IJanelaTrackCoordenadasReducerState { 
    obj_trackcoordenadas_show: boolean; 
} 
export interface IJanelaCoordenadasReducerState { 
    obj_coordenadas_show: boolean;
    coordenadas: [number, number];
    srid: string;
}
 
 
export interface IJanelaDetalhesFicheirosAssociadosReducerState { 
    obj_ficheiro_show: boolean;
}

 
export interface IJanelaOrdenaConstrucaoReducerState { 
    obj_talhaoordenar_show: boolean;
}
export interface IJanelaMoverConstrucaoReducerState { 
    obj_moverconstrucao_show: boolean;
    parentconstrucaoid: string;
    construcaoid: string;
}
 
export interface IJanelaOrdenaCartografiaReducerState { 
    obj_cartografiaordenar_show: boolean;
}
export interface IJanelaFicheirosAssociadosReducerState {
    obj_ficheiro: string;
    obj_ficheiro_show: boolean;
}
export interface IJanelaCriarBufferReducerState{ 
    form_buffer_show: boolean;
} 
export interface IJanelaDefinirCorReducerState{ 
    form_desenhocor_show: boolean;
    mostradefinicoes: boolean;
    valor_cor: string;
    contorno: boolean;
} 
export interface IJanelaMapaZoomCoordenadasReducerState { 
    show_janela: boolean;
}
export interface IJanelaPesquisaPretensaoCamposReducerState { 
    obj_pesquisaconstrucao: boolean;
}
export interface IJanelaImportaConstrucaoReducerState { 
    obj_importaficheiroconstrucao_show: boolean;
    obj_importaficheiroconstrucao_id: string;
    obj_listageografica_refresh: boolean;
}
export interface IJanelaImportaAtendimentoReducerState { 
    obj_importaficheiroatendimento_show: boolean;
    obj_importaficheiroatendimento_id: string;
    obj_listageografica_refresh: boolean;
} 
export interface IJanelaAssociaFicheiroReducerState { 
    obj_associaficheiro_show: boolean;
    obj_associaficheiro_id: string;
    obj_associaficheiro_tipoassociacao: string;
    obj_listaficheiros_refresh: boolean;
}
export interface IJanelaAssociaFicheiroArtigoReducerState { 
    obj_associaficheiroartigo_show: boolean;
    obj_associaficheiroartigo_id: string;
    obj_listaficheirosartigo_refresh: boolean;
}
export interface IJanelaAssociaRegistoAtendimentoReducerState { 
    obj_associaficheiroregistoatendimento_show: boolean;
    obj_associaficheiroregistoatendimento_id: string;
    obj_listaficheirosregistoatendimento_refresh: boolean;
}
export interface IJanelaImportaDesenhoReducerState { 
    obj_importaficheiro_show: boolean;
}  
export interface IJanelaConcessionariosReducerState { 
    obj_concessionarios_show: boolean;
}
export interface IJanelaTiposConstrucaoReducerState { 
    obj_tiposconstrucao_show: boolean;
} 
export interface IJanelaTiposMovimentoReducerState { 
    obj_tiposmovimento_show: boolean;
}
export interface IJanelaGestaoRegulamentosReducerState { 
    obj_gestaoregulamentos_show: boolean;
} 

export interface IJanelaPesquisaPretensaoIdReducerState { 
    obj_pesquisapretid: boolean;
    obj_id: string;
}
export interface IJanelaPesquisaAtendimentoCamposReducerState { 
    obj_pesquisaatendimento: boolean;
}
export interface IJanelaPesquisaAtendimentoIdReducerState { 
    obj_pesquisaatendimentoid: boolean;
}
export interface IJanelaDetalhesGestaoTabelaReducerState {
    obj_info_gt: string;
    obj_info_show_gt: boolean;
}

 
 
export interface IJanelaDetalhesTipoPretensaoReducerState {
    obj_tipopretensao: string;
    obj_tipopretensao_show: boolean;
}

export interface IJanelaDetalhesTipoAtendimentoReducerState {
    obj_tipoatendimento: string;
    obj_tipoatendimento_show: boolean;
}

export interface IJanelaDetalhesTipoReqAtendimentoReducerState {
    obj_tiporeqatendimento: string;
    obj_tiporeqatendimento_show: boolean;
}

export interface IJanelaDetalhesEstadoProcessoReducerState {
    obj_estadoprocesso: string;
    obj_estadoprocesso_show: boolean;
}

export interface IJanelaDetalhesServicosReducerState {
    obj_serv: string;
    obj_serv_show: boolean;
}

export interface IJanelaDetalhesEntidadeExternaReducerState {
    obj_entex: string;
    obj_entex_show: boolean;
}

export interface IJanelaDetalhesObjetoReducerState {
    obj_info: string;
    obj_info_tipo: string;
    obj_info_ids: string;
    obj_info_WKTs: string;
    currentTime: number;
    obj_info_show: boolean;
    posicao_x: number;
    posicao_y: number; 
    pretensaoId: string;
    patrimonioId: string;
}

export interface IJanelaMapaDuploClickReducerState {
    show_janela: boolean;
    obj_info: string;
    obj_info_tipo: string;
    obj_info_ids: string;
    obj_info_WKTs: string;
    currentTime: number;
    
}  

export interface IJanelaConfrontaAtendimentoReducerState{ 
    atendimento_recid: string;
    atendimento_registo_recid: string;
    atendimento_geometria_recid: string;
    atendimento_geometria_descricao: string;
    atendimento_confrontacao_json: string; 
    atendimento_confrontacao_identificador: string; 
}



/*
export interface IGUApplicationState extends IApplicationState {
    grupo_recid: IAppPccReducerState;
}
*/


export const MESSAGE_INITIAL_STATE: IAppPccReducerState = { 
    grupo_poi_recid: "",
    grupo_pret_recid: "",
    grupo_ate_recid: "",
    grupo_fis_recid: "",
    grupo_edu_recid: "",
    grupo_pat_recid: "",
    grupo_cpr_recid: "",    
    grupo_cpr_tipo: "",
    //grid_cpr_tipo: "",
    grupo_rvmu_recid: "",
    grupo_rvnos_recid: "",
    grupo_rvnp_recid: "",
    grupo_rvoa_recid: "",
    grupo_rvf_recid: "",
    grupo_rvr_recid: "",
    grupo_rvs_recid: "",
    grupo_rvs_tipo: "",
    //apiEndpoint:  "" ,
    aplicacao_titulo: "login",
    aplicacao_sigla: "",
    aplicacao_start: false, 
    config : {
        aplicationTokenId: "",
        startSessionId: "",
        configapiEndpoint: "",
        configapiEndpointSIG: "",
        configapiEndpointCadastro: "",
        configapiEndpointDocumentos: "",
        configmapagenturl: "",
        configmapa: ""
    },
    ligacoes_gu : {
        ligado_nopaper: false,
        ligado_medidata: false,
        ligado_airc: false,
        ligado_ano: false, 
    }, 
    pedidos_api: {
        numero_pedidos: 0,
        ficheiros: []
    },
    geral : {
        authtoken: "",
        aplicacao_titulo: "",
        aplicacao_sigla: "",
        userid: "",
        username: "",
        usersession: "",
        permissoes: "",
        funcionalidades: [],

        sep_app : [],
        sep_write: [],
        sep_read : [],

        separadoresnomes: [],
        separadorestooltips: [],
        separadoresids: [],
        separadoressiglas: [],
        separadoresdata: [],

        treeseparadores_itenschecked: [],
        mapa_layers_on: [],

        mapa_initialview_x: "",
        mapa_initialview_y: "", 
        mapa_initialview_escala: "",

        treecartografia_data: [] ,
        treeinstrumentos_data: [] ,
        treeseparador_data: [] ,
        treecartografia_init: [],
        treeinstrumentos_init: [],
        layers_iniciais: [],
       // treecartografia_init: ['65bcaaad-eb15-47b2-99e6-e041e9addbe5', '17cdc612-f317-4c05-ba6c-aba5f47041be', '2302754a-c9fd-4cbe-b1fe-b73dfc5a7ed9'],
        //treeinstrumentos_init:  [] , // elementos da treeinstrumentos - ligados ao inicio
       // treeseparador_init: [] , // elementos da treeseparador - ligados ao inicio
        treepois_data: [] ,
        treeconstrucoes_data: [] , 
        treeatendimento_data: [] ,
        treecadastro_data: [] ,
    },
    dadosimpressoes:{ 
        aux_campos: [],
        aux_value: [],
    },
    reload_menui:{ 
        reloadtipo: "",
    },
    treecartografia:{ 
        treecartografia_tipo: "" ,
        treecartografia_parent_recid: "" ,
        treecartografia_nome: "" ,
        treecartografia_rec_id: "" ,
        treecartografia_layers: "" ,
        treecartografia_legenda: "" ,
        treecartografia_objtipo: "" ,
        treecartografia_setcheck: [] ,
        treecartografia_itenschecked: [],
        reloadKey: "",
   
    },
    treeinstrumentos:{ 
        treeinstrumentos_tipo: "" ,
        treeinstrumentos_parent_recid: "" ,
        treeinstrumentos_nome: "" ,
        treeinstrumentos_rec_id: "" ,
        treeinstrumentos_layers: "" ,
        treeinstrumentos_legenda: "" ,
        treeinstrumentos_objtipo: "" ,
        treeinstrumentos_setcheck: [] ,
        treeinstrumentos_itenschecked: [],
        reloadKey: "",
    },
    treeseparador:{ 
        treeseparador_tipo: "" ,
        treeseparador_parent_recid: "" ,
        treeseparador_nome: "" ,
        treeseparador_rec_id: "" ,
        treeseparador_layers: "" ,
        treeseparador_legenda: "" ,
        treeseparadorobjtipo: "" , 
        treeseparador_setcheck: [] ,
        treeseparador_itenschecked: [],
        treeseparador_idarvore: [],
        treeseparador_itemlayers: [],
    },
    treepontosinteresse:{ 
        treepontosinteresse_tipo: "" ,
        treepontosinteresse_parent_recid: "" ,
        treepontosinteresse_nome: "" ,
        treepontosinteresse_rec_id: "" ,
        treepontosinteresse_itenschecked: [],
        reloadKey: "",
    },
    treeconstrucoes:{ 
        treeconstrucoes_tipo: "" ,
        treeconstrucoes_objtipo: "",
        treeconstrucoes_parent_recid: "" ,
        treeconstrucoes_nome: "" ,
        treeconstrucoes_rec_id: "" ,
        treeconstrucoes_itenschecked: [],
        reloadKey: "",
    },
    treeatendimento:{ 
        treeatendimento_tipo: "" ,
        treeatendimento_parent_recid: "" ,
        treeatendimento_nome: "" ,
        treeatendimento_rec_id: "" ,
        treeatendimento_itenschecked: [],
        reloadKey: "",
    },
    windows :  {
        definir_cor: { 
            form_desenhocor_show: false,
            mostradefinicoes: false,
            valor_cor: "#FF0000",
            contorno: true           
        },
        criar_buffer: {
            form_buffer_show: false
        },
        detalhes_objectos: {
            obj_info: "",
            obj_info_tipo: "",
            obj_info_ids: "",
            obj_info_WKTs: "",
            currentTime: 0,
            obj_info_show: false,
            posicao_x: 650,
            posicao_y: 10,
            pretensaoId: "",
            patrimonioId: ""
        },
        mapa_duploclick: {
            show_janela: false,
            obj_info: "",
            obj_info_tipo: "",
            obj_info_ids: "",
            obj_info_WKTs: "",
            currentTime: 0 
        },
        mapa_zoomcoordenadas:{
            show_janela: false
        },        
         
        gestao_concessionarios: {
            obj_concessionarios_show: false  
        },
        gestao_construcoes: { 
            obj_tiposconstrucao_show: false
        },
        gestao_movimentos: { 
            obj_tiposmovimento_show: false
        },
  
        detalhes_ficheiros: {
            obj_ficheiro: "",
            obj_ficheiro_show: false
        },
            
        pesquisa_pretensaoid:{ 
            obj_pesquisapretid: false,
            obj_id: ""
        }, 
        pesquisa_construcaocampos:{ 
            obj_pesquisaconstrucao: false
        },  
        ordenacartografia:{
            obj_cartografiaordenar_show: false
        },
        ordenaconstrucao:{
            obj_talhaoordenar_show: false
        },
        moverconstrucao:{
            obj_moverconstrucao_show: false,
            parentconstrucaoid: "",
            construcaoid: "",
        },
        ficheiros_associados:{
            obj_ficheiro_show: false,
        },
        trackcoordenadas:{
            obj_trackcoordenadas_show: false,
        },
        coordenadas:{
            obj_coordenadas_show: false,
            coordenadas: [0, 0],
            srid: "",
        },  
        importa_construcao: { 
            obj_importaficheiroconstrucao_show: false,
            obj_importaficheiroconstrucao_id: "",
            obj_listageografica_refresh: false
        },
       
        associa_ficheiro: { 
            obj_associaficheiro_show: false,
            obj_associaficheiro_id: "",
            obj_associaficheiro_tipoassociacao: "",
            obj_listaficheiros_refresh: false
        },
       
        importa_desenho: { 
            obj_importaficheiro_show: false
        },
        localiza_na_arvore: {  
            localiza_separador_cem: false,
            localiza_arvore: false, 
            localiza_grelha: false, 
            sigla_separador: "",
            rec_id_arvore: "",
            rec_id_grelha: "",
        }
    },  
    exportar_dwf: false,
    exportar_dxf: false,
    exportar_dados: "", 

    resetMenuIcons: false,

    layers_totais: [],
    layers_ligados: [], 
    reset_layers: [],
    referencia_treeconstrucoes: null, 
    referencia_treecartografia: null,

    // PCC

    form_cemiterioedit_recId: "", //não nulo edita cemiterio - rec_id do cemiterio

    form_talhaoedit_recId: "", //não nulo edita talhao - rec_id do talhao
    form_talhaonew_paiId:"", //não nulo novo talhao - rec_id do pai do talhao (cemiterio) 

    form_construcaoedit_recId: "", //não nulo edita construção - rec_id da construção
    form_construcaonew_paiId: "", //não nulo nova construção - rec_id do pai da construção (talhão) 
 
    // FIM PCC

    form_separadorgrupoedit_recId: "", //não nulo edita grupo separador - rec_id do grupo de separador
    form_separadorgruponew_paiId:"", //não nulo novo grupo separador - rec_id do pai do grupo de separador 

    form_separadoritemedit_recId: "", //não nulo edita item separador - rec_id do item de separador
    form_separadoritemnew_paiId:"", //não nulo novo item separador - rec_id do pai do item de separador 

    form_separadorid:"",
    
    form_cartografiaitemedit_recId: "", //não nulo edita item cartografia - rec_id do item de cartografia
    form_cartografiaitemnew_paiId: "", //não nulo novo item cartografia - rec_id do pai do item de cartografia 

    form_instrumentosgrupoedit_recId: "",//não nulo edita grupo instrumentos - rec_id do grupo de instrumentos
    form_instrumentosgruponew_paiId: "", //não nulo novo grupo instrumentos - rec_id do pai do grupo de instrumentos   
    form_instrumentositemedit_recId: "",//não nulo edita item instrumentos - rec_id do item de instrumentos
    form_instrumentositemnew_paiId: "", //não nulo novo item instrumentos - rec_id do pai do item de instrumentos   
    
    form_instrumentositemregulamento_recId:"", //não nulo apresenta o form de consulta- rec_id do item de instrumentos
 
    form_pontointeressegrupoedit_recId: "", //não nulo edita grupo pontointeresse - rec_id do grupo de pontointeresse
    form_pontointeressegruponew_paiId: "", //não nulo novo grupo pontointeresse - rec_id do pai do grupo de pontointeresse 

    form_pontointeresseedit_recId: "", //não nulo edita pontointeresse - rec_id da pontointeresse
    form_pontointeressenew_paiId: "", //não nulo nova pontointeresse - rec_id do pai da pontointeresse 
  
    form_pretensaoedit_recId: "", //não nulo edita pretensão - rec_id da pretensão
    form_pretensaonew_paiId:"", //não nulo nova pretensão - rec_id do pai da pretensão 
     
    form_atendimentogrupoedit_recId: "", //não nulo edita grupo atendimento - rec_id do grupo de atendimento
    form_atendimentogruponew_paiId: "", //não nulo novo grupo atendimento - rec_id do pai do grupo de atendimento 
    form_atendimentoedit_recId: "", //não nulo edita atendimento - rec_id do atendimento
    form_atendimentonew_paiId: "", //não nulo novo atendimento - rec_id do pai do atendimento 
 
    form_impressoes_Id: "", // não nulo apresenta no form as variaveis - rec_id das impressoes (conjunto de cartas/definições de modelos)
    form_impressoesnew: "",
    form_cartanew:"",
    form_cartaidparent:"",
    form_cartagruponew: "",

     
   
}

//This reducer function handles our custom redux actions made to our custom state branch

export function AppPCCReducer(state = MESSAGE_INITIAL_STATE, action: { type: string; payload: { [key: string]: any } } = { type: '', payload: {} }
) {
    switch (action.type) {
 
        case Actions.APPLICATION_START:
            return {
                ...state,
                aplicacao_start: true
            };
       
  
        // JANELA DE CRIAR BUFFER
        case Actions.HIDE_MAPA_CRIARBUFFER:
            return {
                ...state,
                windows: {
                ...state.windows,
                criar_buffer: {
                    ...state.windows.criar_buffer,
                    form_buffer_show: false
                    }
                }
            };
        case Actions.SHOW_MAPA_CRIARBUFFER: 
            return {
                ...state,
                windows: {
                ...state.windows,
                criar_buffer: {
                    ...state.windows.criar_buffer,
                    form_buffer_show: true
                }
                }
            }; 
        // JANELA DE COORDENADAS PARA ZOOM
        case Actions.HIDE_MAPA_ZOOMCOORDENADAS:
            return {
                ...state,
                windows: {
                ...state.windows,
                mapa_zoomcoordenadas: {
                    ...state.windows.mapa_zoomcoordenadas,
                    show_janela: false
                    }
                }
            };
        case Actions.SHOW_MAPA_ZOOMCOORDENADAS: 
            return {
                ...state,
                windows: {
                ...state.windows,
                mapa_zoomcoordenadas: {
                    ...state.windows.mapa_zoomcoordenadas,
                    show_janela: true
                }
                }
            };
        
        
   
			// pcc
        case Actions.SHOW_GESTAOTIPOSCONSTRUCAO:
           
            return {
                ...state,
                windows: {
                ...state.windows,
                gestao_construcoes: {
                    ...state,
                    obj_tiposconstrucao_show: true
                }
                }
            };
            //pcc
        case Actions.HIDE_GESTAOTIPOSCONSTRUCAO:
  
            return {
                ...state,
                windows: {
                ...state.windows,
                gestao_construcoes: {
                    ...state,
                    obj_tiposconstrucao_show: false
                }
                }
            };	
	
			// pcc
        case Actions.SHOW_GESTAOTIPOSMOVIMENTO:
           
            return {
                ...state,
                windows: {
                ...state.windows,
                gestao_movimentos: {
                    ...state,
                    obj_tiposmovimento_show: true
                }
                }
            };
            //pcc
        case Actions.HIDE_GESTAOTIPOSMOVIMENTO :
  
            return {
                ...state,
                windows: {
                ...state.windows,
                gestao_movimentos: {
                    ...state,
                    obj_tiposmovimento_show: false
                }
                }
            };	
	

           			// pcc
        case Actions.SHOW_GESTAOCONCESSIONARIOS :
           
            return {
                ...state,
                windows: {
                ...state.windows,
                gestao_concessionarios: {
                    ...state,
                    obj_concessionarios_show: true
                }
                }
            };
            //pcc
        case Actions.HIDE_GESTAOCONCESSIONARIOS :
  
            return {
                ...state,
                windows: {
                ...state.windows,
                gestao_concessionarios: {
                    ...state,
                    obj_concessionarios_show: false
                }
                }
            };	
	
  
	   
        case Actions.SHOW_FICHEIROS:
           
            return {
                ...state,
                windows: {
                ...state.windows,
                detalhes_ficheiros: {
                    ...state,
                    obj_ficheiro_show: true
                }
                }
            };

        case Actions.HIDE_FICHEIROS:
  
            return {
                ...state,
                windows: {
                ...state.windows,
                detalhes_ficheiros: {
                    ...state,
                    obj_ficheiro_show: false
                }
                }
            };
 

        
        case Actions.TREECEM_RELOADTREE:
            
            return {
                ...state,
                treeconstrucoes: {
                ...state.treeconstrucoes,
                treeconstrucoes_tipo: 'reloadtree',
                reloadKey: Date.now().toString(), // Chave única para forçar atualização
                }
            };

 
         
 
        case 'Map/CLEAR_CLIENT_SELECTION':
            return {
                ...state,
                windows: {
                ...state.windows,
                mapa_duploclick: {
                        ...state.windows.mapa_duploclick,  
                        currentTime: 0 
                    }
                }
            }
         case 'Map/SET_VIEW':
            console.log('Map/SET_VIEW');
         
            let aux_escala = action.payload.view.scale;
            if (!isNaN(aux_escala)){
                let pode_exportar = false;
                if (aux_escala<=5000){
                    pode_exportar = true;
                }

                return {
                    ...state, 
                    exportar_dwf_dxf: pode_exportar, 
                }  
            }
                      
           
         case 'Map/SET_SELECTION':


            //console.log(state.viewer.tool);

            let lastTime = state.windows.detalhes_objectos.currentTime;
            console.log('Map/SET_SELECTION time: ' + lastTime);
            let janela_duploclick_aberta = state.windows.mapa_duploclick.show_janela; 
            console.log('Map/SET_SELECTION janela_duploclick_aberta: ' + janela_duploclick_aberta);


            let currentTime = new Date().getTime();
            let elapsedTime = currentTime;
            if (typeof lastTime === 'number' && !isNaN(lastTime)) {
                elapsedTime = currentTime - lastTime;
            } 
            let duplo_click: boolean = false;
            if (elapsedTime<500) {
                console.log('DUPLO CLICK');
                //console.log(elapsedTime);
                //console.log(currentTime);
                //console.log('DUPLO CLICK');
                //info_show=true;
                duplo_click = true;


            }
            if (janela_duploclick_aberta){
                duplo_click = false;
            }
            let info_show=false;
            
            //console.log('Map/SET_SELECTION gismat: ' + currentTime);
            let resObjectos: string[] = [];
            let restipoObjectos: string[] = [];
            let residsObjectos: string[] = [];
            let resWKTsObjectos: string[] = [];
            let abre_detalhe: boolean = true;
            let nomelayer: string = '';
            let id_objeto: string = '';
            let WKT_objeto = '';
            let nr_objetos = 0;
            let resShow: boolean = false;
            if (typeof action.payload === 'object' && 'selection' in action.payload) { 
                const selectiondata = action.payload as { selection: QueryMapFeaturesResponse };
                const selectedLayer = selectiondata.selection.SelectedFeatures?.SelectedLayer;
                var objetos = selectiondata.selection.SelectedFeatures;
                //var polyFeature = objetos?.item[0]; // we suppose your feature is the first one
                // from there, do what you want with your feature, i.e. get its geometry:
                //var geometry = polyFeature.getGeometry();
                 
                //console.log(objetos);
                if (selectedLayer) {
                    for (let i = 0; i < selectedLayer.length; i++) {
                        
                        const feature = selectedLayer[i].Feature;
                        if (feature) {
                            nr_objetos +=1;
                            for (let j = 0; j < feature.length; j++) {
                                // Layer Name
                                //console.log(selectedLayer[i]["@name"]);

                                //if ((selectedLayer.length==1)&&(feature.length==1)){
                                    nomelayer = selectedLayer[i]["@name"];
                                    switch (nomelayer.toUpperCase())
                                    {
                                        case "EPL_ElementsLayer".toUpperCase():
                                            abre_detalhe = false;
                                            restipoObjectos.push("EPL");
                                             break;
                                        case "GU_Pretensao".toUpperCase():
                                            abre_detalhe = false;
                                            restipoObjectos.push("GU");
                                             break;
                                        case "GU_Atendimento".toUpperCase():
                                            abre_detalhe = false;
                                            restipoObjectos.push("ATE");
                                            break;
                                        case "PM_Predios".toUpperCase():
                                            abre_detalhe = false;
                                            restipoObjectos.push("PM");
                                            break;
                                        default:
                                            restipoObjectos.push("");
                                            break;
                                    } 
                                //}

                                resObjectos.push("<table>");
                                resObjectos.push(`<tr><td class='detalheobj_columnA'>Nome: </td><td class='detalheobj_columnB'>${selectedLayer[i]["@name"]}</td></tr>`);
                                resShow = true;
                                id_objeto = '';
                                WKT_objeto = '';
                                const properties = feature[j].Property;
                                if (properties) {
                                    for (let k = 0; k < properties.length; k++) {
                                        // Properties
                                        //console.log(properties[k].Name);
                                        //console.log(properties[k].Value);
                                        //if (abre_detalhe==false){
                                            if (properties[k].Name.toUpperCase()=='ID'){
                                                if (properties[k].Value !== null){
                                                    id_objeto=properties[k].Value ?? '';
                                                }                                                
                                            }
                                            if (properties[k].Name.toUpperCase()=='WKT'){
                                                if (properties[k].Value !== null){
                                                    WKT_objeto=properties[k].Value ?? '';
                                                }                                                
                                            }
                                       // }
                                        resObjectos.push(`<tr><td class='detalheobj_columnA'>${properties[k].Name}: </td><td class='detalheobj_columnB'>${properties[k].Value}</td></tr>`);
                                    }
                                }
                                resObjectos.push("</table>");
                                resObjectos.push("|");
                                residsObjectos.push(id_objeto);
                                residsObjectos.push("|");
                                resWKTsObjectos.push(WKT_objeto);
                                resWKTsObjectos.push("|");
                                restipoObjectos.push("|");
                            }
                        }
                    }
                }
            }
            if (abre_detalhe){

                resObjectos.pop();
                restipoObjectos.pop();
                residsObjectos.pop();
                resWKTsObjectos.pop();
                // Join the array elements to form a single string
                const resString: string = resObjectos.join("");

                const restipoString: string = restipoObjectos.join("");
                const residsString: string = residsObjectos.join("");
                const resWKTsString: string = resWKTsObjectos.join("");
                //const updatedState1 = { ...state.windows.detalhes_objectos, obj_info: resString };
                //console.log(updatedState1);
                //return { ...state, obj_info: resString, obj_info_show: resShow };
                //return { ...state, obj_info: resString };
                //return { ...state, windows: { detalhes_objectos: { obj_info: resString }}};

                return {
                    ...state,
                    windows: {
                    ...state.windows,
                    detalhes_objectos: {
                            ...state.windows.detalhes_objectos, 
                            obj_info: resString,
                            obj_info_tipo: restipoString,
                            obj_info_ids: residsString,
                            obj_info_WKTs: resWKTsString,
                            currentTime: currentTime,
                            pretensaoId: '',
                            patrimonioId: ''
                        }
                    }
                };
            } else {
                resObjectos.pop();
                restipoObjectos.pop();
                residsObjectos.pop();
                resWKTsObjectos.pop();
                const resString: string = resObjectos.join("");
                const restipoString: string = restipoObjectos.join("");
                const residsString: string = residsObjectos.join("");
                const resWKTsString: string = resWKTsObjectos.join("");
                if (duplo_click){ ///////////////////////DUPLO CLICK

                    // NÃO VAMOS FAZER ISTO
                    if (nr_objetos===-10){
                        // duplo click e temos um objeto
                        // abre pretensao      
                        switch (nomelayer)
                        {
                            case "GU_Pretensao":    
                                return { ...state, form_pretensaoedit_recId: id_objeto, form_pretensaonew_paiId : "" };
                        }
                    }
                    // VAMOS VERIFICAR QUAIS AS PRETENSÕES NESSA ÁREA


                    return {
                        ...state,
                        windows: {
                        ...state.windows,
                        mapa_duploclick: {
                                ...state.windows.mapa_duploclick, 
                                show_janela: true,
                                obj_info: resString,
                                obj_info_tipo: restipoString,
                                obj_info_ids: residsString,
                                obj_info_WKTs: resWKTsString,
                                currentTime: currentTime 
                            }
                        }
                    }

                } else {
                    /*
                    resObjectos.pop();
                    restipoObjectos.pop();
                    residsObjectos.pop();
                    resWKTsObjectos.pop();
                    const resString: string = resObjectos.join("");

                    const restipoString: string = restipoObjectos.join("");
                    const residsString: string = residsObjectos.join("");
                    const resWKTsString: string = resWKTsObjectos.join("");
                    //const updatedState1 = { ...state.windows.detalhes_objectos, obj_info: resString };
                    //console.log(updatedState1);
                    //return { ...state, obj_info: resString, obj_info_show: resShow };
                    //return { ...state, obj_info: resString };
                    //return { ...state, windows: { detalhes_objectos: { obj_info: resString }}};
    */
                    return {
                        ...state,
                        windows: {
                        ...state.windows,
                        detalhes_objectos: {
                                ...state.windows.detalhes_objectos, 
                                obj_info: resString,
                                obj_info_tipo: restipoString,
                                obj_info_ids: residsString,
                                obj_info_WKTs: resWKTsString,
                                currentTime: currentTime,
                                pretensaoId: '',
                                patrimonioId: ''
                            }
                        }
                    };  
                }
                 
            }
            
        // JANELA QUE TRATA O DUPLO CLICK - APRESENTA REGISTOS OU ABRE FICHA 
        case Actions.HIDE_MAPA_DUPLOCLICK:
            return {
                ...state,
                windows: {
                ...state.windows,
                mapa_duploclick: {
                        ...state.windows.mapa_duploclick, 
                        show_janela: false,
                        currentTime: 0 
                    }
                }
            }; 
        case Actions.SHOW_MAPA_DUPLOCLICK: 
            return {
                ...state,
                windows: {
                ...state.windows,
                mapa_duploclick: {
                        ...state.windows.mapa_duploclick, 
                        show_janela: true,
                        currentTime: 0 
                    }
                }
            };
        
 

        // CARTOGRAFIA - ITEM DE CARTOGRAFIA
        case Actions.CARTOGRAFIAITEM_NOVO:   
            const aux_idcartografiaitempai = action.payload;          
            return { ...state, form_cartografiaitemedit_recId:"", form_cartografiaitemnew_paiId: aux_idcartografiaitempai };
   
        case Actions.CARTOGRAFIAITEM_EDITA:   
            
            const aux_idcartografiasel = action.payload;          
            return { ...state, form_cartografiaitemedit_recId: aux_idcartografiasel, form_cartografiaitemnew_paiId:"" };
        case Actions.CARTOGRAFIAITEM_FECHA:            
            return { ...state, form_cartografiaitemedit_recId:"", form_cartografiaitemnew_paiId: "" };
         
        case Actions.SHOW_CARTOGRAFIA_ORDENA: 
            return {
                ...state,
                windows: {
                ...state.windows,
                ordenacartografia: {
                    ...state.windows.ordenacartografia,
                    obj_cartografiaordenar_show: true 
                }
                }
            };
        case Actions.HIDE_CARTOGRAFIA_ORDENA:    
            return {
                ...state,
                windows: {
                ...state.windows,
                ordenacartografia: {
                    ...state.windows.ordenacartografia,
                    obj_cartografiaordenar_show: false 
                }
                }
            }; 

        // PROCESSOS - ADICIONAR OU REMOVER INICACAO DE PEDIDO APIs
        case Actions.PROCESSOS_NOVOPROCESSO:
        
            var numero = state.pedidos_api.numero_pedidos;
            numero ++;
            return { ...state, 
                pedidos_api: {
                    ...state.pedidos_api,
                    numero_pedidos: numero}
                };

        case Actions.PROCESSOS_TERMINAPROCESSO:
            
            var numero = state.pedidos_api.numero_pedidos;
            numero --;
            return  { ...state, 
                pedidos_api: {
                    ...state.pedidos_api,
                    numero_pedidos: numero}
                }; 
        case Actions.SIDEBARLOCALIZA_DADOS:   
            const aux_separador1 = action.payload; 
            let tree_recid1=aux_separador1.parent_id;
            let grelha_recid1=aux_separador1.rec_id;
            let sigla1=aux_separador1.sigla_separador;
            
            return {
                ...state,
                windows: {
                    ...state.windows,
                    localiza_na_arvore: {
                        ...state.windows.localiza_na_arvore,
                        localiza_separador_cem: false, 
                        localiza_arvore: false, 
                        localiza_grelha: false, 
                        sigla_separador: sigla1,
                        rec_id_arvore: tree_recid1,
                        rec_id_grelha: grelha_recid1, 
                    }
                } 
            }; 
        case Actions.SIDEBARLOCALIZA_DADOS_SEND:   
            const aux_sigla = state.windows.localiza_na_arvore.sigla_separador; 
            if (aux_sigla=='CEM'){
                return {
                    ...state,
                    windows: {
                        ...state.windows,
                        localiza_na_arvore: {
                            ...state.windows.localiza_na_arvore,
                            localiza_separador_cem: true                             
                        }
                    } 
                }; 
            }
            
        case Actions.SIDEBARLOCALIZA_START:   
            const aux_separador = action.payload; 
            let tree_recid=aux_separador.parent_id;
            let grelha_recid=aux_separador.rec_id;
            let sigla=aux_separador.sigla_separador;
            if (sigla=='CEM'){
                return {
                    ...state,
                    windows: {
                        ...state.windows,
                        localiza_na_arvore: {
                            ...state.windows.localiza_na_arvore,
                            localiza_separador_cem: true, 
                            localiza_arvore: false, 
                            localiza_grelha: false, 
                            sigla_separador: sigla,
                            rec_id_arvore: tree_recid,
                            rec_id_grelha: grelha_recid, 
                        }
                    } 
                }; 
            }
            
           
        case Actions.SIDEBARLOCALIZA_END:    
            return {
                ...state,
                windows: {
                    ...state.windows,
                    localiza_na_arvore: {
                        ...state.windows.localiza_na_arvore, 
                        localiza_separador_cem: false, 
                        localiza_arvore: true,  
                    }
                }               
            }; 
        case Actions.TREELOCALIZA_END:    
            return {
                ...state,
                windows: {
                    ...state.windows,
                    localiza_na_arvore: {
                        ...state.windows.localiza_na_arvore,
                        localiza_separador_cem: false, 
                        localiza_arvore: false,  
                        localiza_grelha: true, 
                    }
                }               
            }; 
        case Actions.GRELHALOCALIZA_END:    
            return {
                ...state,
                windows: {
                    ...state.windows,
                    localiza_na_arvore: {
                        ...state.windows.localiza_na_arvore,
                        localiza_separador_cem: false, 
                        localiza_arvore: false,  
                        localiza_grelha: false,  
                        sigla_separador: "",
                        rec_id_arvore: "",
                        rec_id_grelha: "", 
                    }
                }               
            }; 
        case Actions.DWF_MAPA_START:   
            const aux_dwfdados = action.payload;
            return {
                ...state,
                exportar_dwf: true, 
                exportar_dados: aux_dwfdados                
            }; 
        case Actions.DWF_MAPA_END:    
            return {
                ...state,
                exportar_dwf: false, 
                exportar_dados: ""                
            }; 
        case Actions.DXF_MAPA_START:   
            const aux_dxfdados = action.payload;
            return {
                ...state,
                exportar_dxf: true, 
                exportar_dados: aux_dxfdados                
            }; 
        case Actions.DXF_MAPA_END:     
            return {
                ...state,
                exportar_dxf: false, 
                exportar_dados: ""                
            }; 
 
 
        
 
        case Actions.SHOW_TRACKCOORDENADAS:  
            return {
                ...state,
                windows: {
                ...state.windows,
                trackcoordenadas: {
                    ...state.windows.trackcoordenadas,
                    obj_trackcoordenadas_show: true,  
                }
                }
            };
        case Actions.HIDE_TRACKCOORDENADAS:    
            return {
                ...state,
                windows: {
                ...state.windows,
                trackcoordenadas: {
                    ...state.windows.trackcoordenadas,
                    obj_trackcoordenadas_show: false, 
                }
                }
            };

        case Actions.SHOW_COORDENADAS: 
            const aux_coorddata = action.payload;
            let aux_coordenadas=aux_coorddata.coordenadas;
            let aux_srid=aux_coorddata.srid; 
            return {
                ...state,
                windows: {
                ...state.windows,
                coordenadas: {
                    ...state.windows.coordenadas,
                    obj_coordenadas_show: true,
                    coordenadas: aux_coordenadas,
                    srid: aux_srid,
                }
                }
            };
        case Actions.HIDE_COORDENADAS:    
            return {
                ...state,
                windows: {
                ...state.windows,
                coordenadas: {
                    ...state.windows.coordenadas,
                    obj_coordenadas_show: false,
                    coordenadas: [0, 0],
                    srid: "",
                }
                }
            }; 
   
    

        // PCC - Gestão Cemiterio
        case Actions.CEMITERIO_NOVO:   
            const aux_idcemiteriogrupopai = action.payload;          
            return { ...state, form_cemiterioedit_recId: "principal"};
        case Actions.CEMITERIO_EDITA:   
            const aux_idcemiteriogrupo_id = action.payload;          
            return { ...state, form_cemiterioedit_recId: aux_idcemiteriogrupo_id};
        case Actions.CEMITERIO_FECHA:            
            return { ...state, form_cemiterioedit_recId: ""};
        // PCC - Gestão Talhões
        case Actions.TALHAO_NOVO:   
            const aux_idtalhaogrupopai = action.payload;          
            return { ...state, form_talhaoedit_recId: "", form_talhaonew_paiId: aux_idtalhaogrupopai};
        case Actions.TALHAO_EDITA:   
            const aux_idtalhaogrupo_id = action.payload;          
            return { ...state, form_talhaoedit_recId: aux_idtalhaogrupo_id, form_talhaonew_paiId: ""};
        case Actions.TALHAO_FECHA:            
            return { ...state, form_talhaoedit_recId: "", form_talhaonew_paiId: ""};
        // PCC - Gestão Construções    
        case Actions.CONSTRUCAO_EDITA:   
            const aux_idconstsel = action.payload;          
            return { ...state, form_construcaoedit_recId: aux_idconstsel, form_construcaonew_paiId:"" };
        case Actions.CONSTRUCAO_FECHA:
            return { ...state, form_construcaoedit_recId: "" , form_construcaonew_paiId:"" }; 
        case Actions.CONSTRUCAO_NOVO:   
            const aux_idconstpaisel = action.payload;          
            return { ...state, form_construcaoedit_recId:"", form_construcaonew_paiId: aux_idconstpaisel };


 
      
        // GESTÃO URBANISTICA - PRETENSOES

        case Actions.TREECEM_UPDATE:
            const aux_treegudata = action.payload;
            let u_recid=aux_treegudata.rec_id;
            let u_nome=aux_treegudata.nome;
            let u_objtipo=aux_treegudata.objtipo;
            
            return {
                ...state,
                treeconstrucoes: {
                ...state.treeconstrucoes,
                treeconstrucoes_tipo: 'update',
                treeconstrucoes_objtipo: u_objtipo,
                treeconstrucoes_rec_id: u_recid,
                treeconstrucoes_nome: u_nome,
                treeconstrucoes_parent_recid: '',
                }
            };
        case Actions.TREECEM_INSERT:
            const aux_treegudatai = action.payload;
            let obj_recid=aux_treegudatai.rec_id;
            let obj_nome=aux_treegudatai.nome;
            let par_recid=aux_treegudatai.parent_recid; 
            let obj_tipo=aux_treegudatai.objtipo;
          
            return {
                ...state,
                treeconstrucoes: {
                ...state.treeconstrucoes,
                treeconstrucoes_tipo: 'insert',
                treeconstrucoes_objtipo: obj_tipo,
                treeconstrucoes_rec_id: obj_recid,
                treeconstrucoes_nome: obj_nome,
                treeconstrucoes_parent_recid: par_recid,
                } 
            };
        case Actions.TREECEM_UPDATEFIM:
                
            return {
                ...state,
                treeconstrucoes: {
                ...state.treeconstrucoes,
                treeconstrucoes_tipo: '',
                treeconstrucoes_rec_id: '',
                treeconstrucoes_nome:'',
                treeconstrucoes_parent_recid: '',
                }
            };
        case Actions.TREECEM_GUARDA_GRUPOSLIGADOS:
            const aux_treeconstrucoesdatacheck = action.payload.dados; 
            return {
                ...state,
                treeconstrucoes: {
                ...state.treeconstrucoes,
                treeconstrucoes_itenschecked: aux_treeconstrucoesdatacheck 
                }
            };
        
  
 
        case Actions.MENUI_UPDATEFIM:
                
            return {
                ...state,
                reload_menui: {
                ...state.reload_menui,
                reloadtipo: '',
                }
            };

        case Actions.STORE_LAYERS_RESET:
            // OS Layers que estão ligados são passados para a TreeCartografia para serem desligados
            // Layers da Cartografia e dos Instrumentos
 
            let LayersLigados: string[] = [...state.layers_ligados];

            return {
                ...state,
                reset_layers: LayersLigados,
                layers_ligados: [],
                treecartografia: {
                    ...state.treecartografia,
                    treecartografia_tipo: 'resetcheck' 
                    },
                treeinstrumentos:{ 
                    ...state.treeinstrumentos,
                    treeinstrumentos_tipo: 'resetcheck' 
                    },
            };
        case Actions.STORE_LAYERS_RESET_FIM:
           
            return {
                ...state,
                reset_layers: [],
                layers_ligados: [],
                treecartografia: {
                    ...state.treecartografia,
                    treecartografia_tipo: '' 
                    },
                treeinstrumentos:{ 
                    ...state.treeinstrumentos,
                    treeinstrumentos_tipo: '' 
                    },
            };
        case Actions.STORE_LAYERS:
                
                // Convert addedlayers and removedlayers strings to arrays
                const  LayersArray = action.payload.dados;
                 
                return {
                    ...state, 
                    layers_totais: [
                        ...state.layers_totais,         // Pega os dados antigos
                        ...action.payload.dados        // Adiciona os novos dados
                      ]
                };
             
        case Actions.STORE_LAYERS_LIGADOS:
            const { addedlayers, removedlayers } = action.payload;
            // Convert addedlayers and removedlayers strings to arrays
            const addedLayersArray = [].concat(addedlayers || []) ;
            const removedLayersArray = removedlayers || [];
            console.log('STORE_LAYERS_LIGADOS addedLayersArray ' + addedLayersArray);
           

            // Copiar o array atual de `layers_ligados`
            let updatedLayersLigados: string[] = [...state.layers_ligados];
            //console.log('STORE_LAYERS_LIGADOS updatedLayersLigados1 ' + updatedLayersLigados);

            // Remover os elementos de `removedlayers`
            updatedLayersLigados =(updatedLayersLigados || []).filter(layer => !removedLayersArray.includes(layer));
            //console.log('STORE_LAYERS_LIGADOS updatedLayersLigados2 ' + updatedLayersLigados);
            // Adicionar os elementos de `addedlayers`, garantindo que não haja duplicatas
            //updatedLayersLigados = [...updatedLayersLigados, ...addedLayersArray.filter((layer: string)  => !updatedLayersLigados.includes(layer))];
            updatedLayersLigados = [
                ...updatedLayersLigados, 
                ...(addedLayersArray || []).filter((layer: string) => !updatedLayersLigados.includes(layer))
            ];
            
            //console.log('STORE_LAYERS_LIGADOS updatedLayersLigados3 ' + updatedLayersLigados);

            
            // Update layers_ligados array
            /*const updatedLayersLigados = [
                ...state.layers_ligados,
                ...addedLayersArray.filter((layer: string) => !state.layers_ligados.includes(layer)),
            ].filter((layer) => !removedLayersArray.includes(layer));*/

            return {
                ...state,
                layers_ligados: updatedLayersLigados,
            };

         
        //CARTOGRAFIA
        case Actions.TREEGUCARTOGRAFIA_STORECHECKED:
            console.log('TREEGUCARTOGRAFIA_STORECHECKED '+ action.payload);
            const aux_treegudatacarcheck1 = action.payload.dados; 
            
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: '',
                treecartografia_itenschecked: aux_treegudatacarcheck1,
                }
            };
        
        case Actions.TREEGUCARTOGRAFIA_SETCHECKED:
            console.log('TREEGUCARTOGRAFIA_SETCHECKED '+ action.payload);
            const aux_treegudatacarcheck = action.payload.dados;  
            
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: 'updatecheck',
                treecartografia_setcheck: aux_treegudatacarcheck,
                }
            };
        
        case Actions.TREEGUCARTOGRAFIA_UPDATECHECKFIM:
            console.log('TREEGUCARTOGRAFIA_UPDATECHECKFIM ');
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: '',
                treecartografia_setcheck: [],
                }
            };  
       
       
        case Actions.TREEGUCARTOGRAFIA_RELOADTREE:
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: 'reloadtree',
                reloadKey: Date.now().toString(),
                }
            };

              
        case Actions.TREEGUCARTOGRAFIA_UPDATE:
            const aux_treegudatacar = action.payload;
            let u_recidcar=aux_treegudatacar.rec_id;
            let u_nomecar=aux_treegudatacar.nome;
            let u_layers=aux_treegudatacar.layers; 
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: 'update',
                treecartografia_rec_id: u_recidcar,
                treecartografia_nome: u_nomecar,
                treecartografia_parent_recid: '',
                treecartografia_layers: u_layers,
                }
            };
        case Actions.TREEGUCARTOGRAFIA_INSERT:
            const aux_treegudatacart = action.payload;
            let obj_recidcart=aux_treegudatacart.rec_id;
            let obj_nomecart=aux_treegudatacart.nome;
            let par_recidcart=aux_treegudatacart.parent_recid;
            let objlayers=aux_treegudatacart.layers; 
            let objlegenda=aux_treegudatacart.legenda; 
            let objtipo=aux_treegudatacart.objtipo;  
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: 'insert',
                treecartografia_rec_id: obj_recidcart,
                treecartografia_nome: obj_nomecart,
                treecartografia_parent_recid: par_recidcart,
                treecartografia_layers: objlayers,
                treecartografia_legenda: objlegenda,
                treecartografia_objtipo: objtipo,
                }
            };
        case Actions.TREEGUCARTOGRAFIA_DELETE:
            const aux_treegudatacartdel = action.payload;
            let objdel_recidcart=aux_treegudatacartdel.rec_id;
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: 'delete',
                treecartografia_rec_id: objdel_recidcart,
                }
            };
        case Actions.TREEGUCARTOGRAFIA_UPDATEFIM:
                
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: '',
                treecartografia_rec_id: '',
                treecartografia_nome:'',
                treecartografia_parent_recid: '',
                }
            };    
        case Actions.TREEGUCARTOGRAFIA_RELOADMAP:
            
            return {
                ...state,
                treecartografia: {
                ...state.treecartografia,
                treecartografia_tipo: 'reloadmap',
                }
            };
         

  
         
      
        case Actions.TREECEM_SELECIONA:   
            const aux_idpret = action.payload;     
             
            const aux_url = config.apiEndpointCadastro + "Construcao/PorTalhao/?id=" + aux_idpret;    
            return { 
                ...state, 
                    grupo_pret_recid: aux_idpret , 
                    apiEndpoint: aux_url
                };
        case Actions.TREECEM_LIMPA:
            return { ...state, grupo_pret_recid: "" , apiEndpoint: "" };
 
        case Actions.PRETENSAO_NOVO:   
            const aux_idpretpaisel = action.payload;          
            return { ...state, form_pretensaoedit_recId:"", form_pretensaonew_paiId: aux_idpretpaisel };
        case Actions.SHOW_PRETENSAO_ORDENA: 
            return {
                ...state,
                windows: {
                ...state.windows,
                ordenaconstrucao: {
                    ...state.windows.ordenaconstrucao,
                    obj_talhaoordenar_show: true 
                }
                }
            };
        case Actions.HIDE_PRETENSAO_ORDENA:    
            return {
                ...state,
                windows: {
                ...state.windows,
                ordenaconstrucao: {
                    ...state.windows.ordenaconstrucao,
                    obj_talhaoordenar_show: false 
                }
                }
            };
        case Actions.SHOW_CONSTRUCAO_MOVER: 
            const const_id = action.payload.item; 
            const aux_parentconstrucaoid = const_id.parentconstrucaoid;
            const aux_construcaoid = const_id.construcaoid; 
            return {
                ...state,
                windows: {
                ...state.windows,
                moverconstrucao: {
                    ...state.windows.moverconstrucao,
                    obj_moverconstrucao_show: true,
                    parentconstrucaoid: aux_parentconstrucaoid,
                    construcaoid: aux_construcaoid,
                }
                }
            };
        case Actions.HIDE_CONSTRUCAO_MOVER:    
            return {
                ...state,
                windows: {
                ...state.windows,
                moverconstrucao: {
                    ...state.windows.moverconstrucao,
                    obj_moverconstrucao_show: false,
                    parentconstrucaoid: "",
                    construcaoid: "", 
                }
                }
            };      
       
            case Actions.HIDE_PESQUISA_CONSTRUCAO_ID:
  
            return {
                ...state,
                windows: {
                ...state.windows,
                pesquisa_construcaoid: {
                    ...state,
                    obj_pesquisaconstrucaoid: false
                }
                }
            };
 

        case Actions.SHOW_PESQUISA_CONSTRUCAO_ID:
           
            return {
                ...state,
                windows: {
                ...state.windows,
                pesquisa_construcaoid: {
                    ...state,
                    obj_pesquisaconstrucaoid: true
                }
                }
            };
     
        case Actions.HIDE_PESQUISA_CONSTRUCAO:
  
            return {
                ...state,
                windows: {
                ...state.windows,
                pesquisa_construcaocampos: {
                    ...state,
                    obj_pesquisaconstrucao: false
                }
                }
            };

        case Actions.SHOW_PESQUISA_CONSTRUCAO:
           
            return {
                ...state,
                windows: {
                ...state.windows,
                pesquisa_construcaocampos: {
                    ...state,
                    obj_pesquisaconstrucao: true
                }
                }
            };
            
        case Actions.SHOW_IMPORTAFILE_CONSTRUCAO:
            const pret_id = action.payload; 
            return {
                ...state,
                windows: {
                ...state.windows,
                importa_construcao: {
                    ...state.windows.importa_construcao,
                    obj_importaficheiroconstrucao_show: true,
                    obj_importaficheiroconstrucao_id: pret_id,
                    obj_listageografica_refresh: false
                }
                }
            };
        case Actions.HIDE_IMPORTAFILE_CONSTRUCAO:
           
            return {
                ...state,
                windows: {
                ...state.windows,
                importa_construcao: {
                    ...state.windows.importa_construcao,
                    obj_importaficheiroconstrucao_show: false,
                    obj_importaficheiroconstrucao_id: "" 
                }
                }
            };
        case Actions.HIDE_AND_REFRESH_IMPORTAFILE_CONSTRUCAO:
           
            return {
                ...state,
                    windows: {
                    ...state.windows,
                    importa_construcao: {
                        ...state.windows.importa_construcao, 
                        obj_importaficheiroconstrucao_show: false,
                        obj_importaficheiroconstrucao_id: "",
                        obj_listageografica_refresh: true
                    }
                }
            };
        case Actions.REFRESH_END_IMPORTAFILE_CONSTRUCAO:
           
            return {
                ...state,
                    windows: {
                    ...state.windows,
                    importa_construcao: {
                        ...state.windows.importa_construcao,  
                        obj_listageografica_refresh: false
                    }
                }
            };
        // GESTÃO URBANISTICA - ATENDIMENTO
/*        case Actions.TREEATE_SELECIONA:   
            const aux_idate = action.payload;       
            const aux_urla = config.apiEndpoint + "gridAtendimentos?id=" + aux_idate;      
            return { ...state, grupo_ate_recid: aux_idate , apiEndpoint: aux_urla };
        case Actions.TREEATE_LIMPA:
            return { ...state, grupo_ate_recid: "" , apiEndpoint: "" };
        
        case Actions.SHOW_IMPORTAFILE_ATENDIMENTO:
            const atendimento_id = action.payload; 
            return {
                ...state,
                windows: {
                ...state.windows,
                importa_atendimento: {
                    ...state,
                    obj_importaficheiroatendimento_show: true,
                    obj_importaficheiroatendimento_id: atendimento_id,
                    obj_listageografica_refresh: false
                }
                }
            };
        case Actions.HIDE_IMPORTAFILE_ATENDIMENTO:
           
            return {
                ...state,
                windows: {
                ...state.windows,
                importa_atendimento: {
                    ...state,
                    obj_importaficheiroatendimento_show: false,
                    obj_importaficheiroatendimento_id: "",
                    obj_listageografica_refresh: true
                }
                }
            };
        case Actions.SHOW_ASSOCIAFILE_REGISTOATENDIMENTO:
            const atendimentoregisto_id = action.payload; 
            return {
                ...state,
                windows: {
                ...state.windows,
                associa_registoatendimento: {
                    ...state,
                    obj_associaficheiroregistoatendimento_show: true,
                    obj_associaficheiroregistoatendimento_id: atendimentoregisto_id,
                    obj_listaficheirosregistoatendimento_refresh: false,
                }
                }
            };
        case Actions.HIDE_ASSOCIAFILE_REGISTOATENDIMENTO:
            
            return {
                ...state,
                windows: {
                ...state.windows,
                associa_registoatendimento: {
                    ...state,
                    obj_associaficheiroregistoatendimento_show: false,
                    obj_associaficheiroregistoatendimento_id: "",
                    obj_listaficheirosregistoatendimento_refresh: false,
                }
                }
            };
        case Actions.REFRESH_FILES_REGISTOATENDIMENTO_START: 
            return {
                ...state,
                windows: {
                ...state.windows,
                associa_registoatendimento: {
                    ...state,
                    obj_associaficheiroregistoatendimento_show: false,
                    obj_associaficheiroregistoatendimento_id: "",
                    obj_listaficheirosregistoatendimento_refresh: true,
                }
                }
            };
        case Actions.REFRESH_FILES_REGISTOATENDIMENTO_END: 
            return {
                ...state,
                windows: {
                ...state.windows,
                associa_registoatendimento: {
                    ...state,
                    obj_associaficheiroregistoatendimento_show: false,
                    obj_associaficheiroregistoatendimento_id: "",
                    obj_listaficheirosregistoatendimento_refresh: false,
                }
                }
            };
*/

            case Actions.SHOW_ASSOCIAFILE:
                const registo_id = action.payload.registo_id; 
                const tipoassociacao = action.payload.tipoassociacao; 
                return {
                    ...state,
                    windows: {
                    ...state.windows,
                    associa_ficheiro: {
                        ...state,
                        obj_associaficheiro_show: true,
                        obj_associaficheiro_id: registo_id,
                        obj_associaficheiro_tipoassociacao: tipoassociacao,
                        obj_listaficheiros_refresh: false,
                    }
                    }
                };
            case Actions.HIDE_ASSOCIAFILE:
                
                return {
                    ...state,
                    windows: {
                    ...state.windows,
                    associa_ficheiro: {
                        ...state,
                        obj_associaficheiro_show: false,
                        obj_associaficheiro_id: "",
                        obj_associaficheiro_tipoassociacao: "",
                        obj_listaficheiros_refresh: false,
                    }
                    }
                };
            case Actions.REFRESH_FILES_START: 
                return {
                    ...state,
                    windows: {
                    ...state.windows,
                    associa_ficheiro: {
                        ...state,
                        obj_associaficheiro_show: false,
                        obj_associaficheiro_id: "",
                        obj_associaficheiro_tipoassociacao: "",
                        obj_listaficheiros_refresh: true,
                    }
                    }
                };
            case Actions.REFRESH_FILES_END: 
                return {
                    ...state,
                    windows: {
                    ...state.windows,
                    associa_ficheiro: {
                        ...state,
                        obj_associaficheiro_show: false,
                        obj_associaficheiro_id: "",
                        obj_associaficheiro_tipoassociacao: "",
                        obj_listaficheiros_refresh: false,
                    }
                    }
                };
    
 
          
        
  
        // LOGIN
        case Actions.LOGIN:
            const logindata = action.payload;                
            const auxpermissoes =  action.payload.permissoes;            
            const entries = auxpermissoes.split(';').filter(Boolean);
            const permissions: Permission[] = [];
            entries.forEach((permissao : string) => {
                const aux = permissao.split('|'); // Split entry into parts
                const cod = aux[1];
                if (cod != '') {
                    const subCod = aux[2];
                    const CodPai = aux[3];
                    const SubCodPai = aux[4];
                    const Tipo = aux[5];
                    const perm = aux[7];

                    // If Perm is 1 or 256, store the permission
                    if (perm === '1' || perm === '256') {
                        let valor = '1';
                        if (perm === '256') { valor = '0'; }
                        permissions.push({ cod, subCod, CodPai, SubCodPai, Tipo, perm: valor });
                    }
                }
            });            
            // sep_app: aux_separadores_app, sep_write: aux_separadores_escrita, sep_read: aux_separadores_leitura,
            const auxsep_app =  action.payload.sep_app;
            const auxsep_write =  action.payload.sep_write;
            const auxsep_read =  action.payload.sep_read;
            console.log(Actions.LOGIN);
            return { ...state, 
                 aplicacao_titulo: action.payload.aplicacao_titulo , 
                 aplicacao_sigla: action.payload.aplicacao_sigla , 
                 geral: action.payload , 
                 funcionalidades: permissions };
             
        //CONFIG
        case Actions.CONFIG:
            const configdata = action.payload;   

            //const auxseparadores =  action.payload.separadores.split("|");
            //const auxseparadoresids =  action.payload.separadoresids.split("|");
            //const auxseparadoressiglas =  action.payload.separadoressiglas.split("|");       
            console.log(Actions.CONFIG);
            return { ...state, config: action.payload  };

 
       
        default:
            return state;

    }
    return state;
    
}