
 /*

var FMenus = 1500

var FMenuCart = 1505;
var FMenuIGT = 3505;
var FMenuPOI = 1600;
var FMenuGU = 3550;
var FMenuAtendimento = 3700;
var FMenusGU = 3500;
var FMenuMapa = 1525;

var FMenusEM = 15025;
var FMenuEM = 15100;
*/
// permissionsUtils.ts
type Permission = {
    cod: string;
    subCod: string;
    CodPai: string;
    SubCodPai: string;
    Tipo: string;
    perm: string;
};

export function findPermissionByCodAndSubCod(permissions: Permission[], targetCod: string, targetSubCod: string): boolean {
    for (const permission of permissions) {
        if (permission.cod == targetCod && permission.subCod == targetSubCod) {
            return permission.perm=='1';
        }
    }
    return false; // Return null if permission not found
}

export const  F_ExportarDXF = 1951;
export const  F_ExportarDWF = 1952;

export const  F_FicheirosAssociados_Consultar = 1954;
export const  F_FicheirosAssociados_Editar = 1953;

export const  F_GestaoModelos = 1933;
export const  F_GestaoImpressoes = 2100;
export const  F_GestaoRegulamentos = 1935;
export const  F_GestaoSeparadores = 1560;
export const  F_GestaoUsersAdministrativa = 3111;
export const  F_GestaoUsersAdministrativa_Admin = 3112;


export const  F_GestaoEntidadesExternas = 3105;
export const  F_GestaoServicos = 3115;
export const  F_GestaoTiposIntervencaoSolo = 3107;
export const  F_GestaoTiposPretensao = 3109;

export const  F_GestaoTiposAtendimento = 3120;
export const  F_GestaoTiposRequerenteAtendimento = 3125;
export const  F_GestaoEstadosProcesso = 3130;

export const  F_ProcessarRegistoImpressoes = 1253;
export const  F_RegistoImpressoes = 1934;
export const  F_LayersConfrontacao = 1256;

export const  F_Impressao = 1800;
export const  F_Impressao_Editar = 2101;
export const listaDeCodigosGestaoTabelas = [1800, 2101, 1256, 1951, 1952, 1953, 1954, 1253, 1933, 1934, 1935, 1560, 2100, 3105, 3111, 3112, 3115, 3107, 3109, 3120, 3125, 3130];


export const listaDeCodigosImpressao = [1800, 2101];

export const  F_Cartas = 1750;
export const  F_Cartas_Editar = 1751;

export const F_ProcessarCartas = 1250;
export const listaDeCodigosCartas = [1750, 1751, 1250];
 
export const  FCart_NovoItem = 1510;   
export const  FCart_EditarItem = 1520;
export const  FCart_EditarOrdem = 1530;
 

export const listaDeCodigosCartografia = [1510, 1520, 1530];


export const  FIGT_NovoGrupo = 3510;
export const  FIGT_NovoItem = 3520;
export const  FIGT_NovoGrupoPrin = 3530;
export const  FIGT_EditarGrupo = 3540;
export const  FIGT_EditarItem = 3545;  
export const  FIGT_EditarOrdem = 3555;

export const  FIGT_NovoRegulamento = 3556;  
export const  FIGT_EditarRegulamento = 3557; 
export const  FIGT_EditarOrdemRegulamento = 3558; 

export const  FIGT_ConsultarRegulamentoAssociado = 3559; 

export const listaDeCodigosInstrumentos = [3510, 3520, 3530, 3540, 3545, 3555, 3556, 3557, 3558, 3559 ];

export const FGU_NovoGrupoPOI = 1620;
export const FGU_NovoItemPOI= 1610; 
export const FGU_EditarGrupoPOI = 1640;
export const FGU_EditarItemPOI = 1630;
export const FGU_EditarOrdemPOI = 1635; 

export const listaDeCodigosPOIs = [1610, 1620, 1630, 1635, 1640 ];

export const FGU_NovoGrupoPretensao = 3560;
export const FGU_NovoItemPretensao = 3570;
export const FGU_ConsultarItemPretensao = 3575;
export const FGU_EditarGrupoPretensao = 3580;
export const FGU_EditarItemPretensao = 3590;
export const FGU_EditarOrdemPretensao = 3595;
export const FGU_PesquisaPretensao = 3600;
export const FGU_ObterSHPPretensao = 3605;
export const FGU_ImportarPretensao = 3610;

export const listaDeCodigosConstrucoes = [3015, 3560, 3570, 3575, 3580, 3590, 3595, 3600, 3605, 3610 ];

export const FGU_AcessoEscritaArvoreConstrucoes = 3015;//, 'Acesso escrita à Árvore de Construções

export const FGU_NovoGrupoAtendimento = 3710;
export const FGU_NovoItemAtendimento = 3715;
export const FGU_ConsultarItemAtendimento = 3713;
export const FGU_EditarGrupoAtendimento = 3720;
export const FGU_EditarItemAtendimento = 3725;
export const FGU_EditarOrdemAtendimento = 3730;
export const FGU_PesquisaAtendimento = 3735;
export const FGU_ObterSHPAtendimento = 3740;
export const FGU_ImportarAtendimento = 3745;
export const FGU_AdminAtendimento = 3746;

export const listaDeCodigosAtendimento = [3710, 3715, 3713, 3720, 3725, 3730, 3735, 3740, 3745, 3746];

export const listaDeCodigosSeparadores = [1100];

export const FGU_AcessoEscritaArvoreSeparador = 1100;//, 'Acesso escrita à Árvore de Separador

export const aplicationtoken_gu='84405d19-b8fd-2d98-cb89-20264413b213'; 
export const aplicationtoken_epl='00793205-9d1d-4c4c-ae75-a83b9fb9d61d';  

