import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import * as Actions from "../../constants/actions";
import PropTypes from 'prop-types'; 
import { message as messageDHX, alert as alertDHX, confirm as confirmDHX, 
  Window as WindowDHX,List as ListDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, Toolbar as ToolbarDHX,
  ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import { v4 as uuidv4 } from 'uuid';
import ReactDOMServer from 'react-dom/server';
import { Icon, FontIcon, ImageIcon, values } from '@fluentui/react';
import {Teste} from '../../external';
import { addClientSelectedFeature, clearClientSelection, nextView, previousView, setBaseLayer, setCurrentView, setScale, setSelection } from "mapguide-react-layout/lib/actions/map";
import { mapStateReducer } from "mapguide-react-layout/lib/reducers/map-state";
import  WKT  from 'ol/format/WKT'; 
import { Point, LineString, Polygon } from 'ol/geom'; 
import * as Perm from "../../constants/permissoes"; // Importe a função do arquivo permissoes.ts 
 
import { MapViewer } from 'mapguide-react-layout/lib/containers/neo-map-viewer';
import { useMapProviderContext, useReduxDispatch } from 'mapguide-react-layout/lib/components/map-providers/context';
import { MapGuideMapProviderContext, setSelectionXml } from 'mapguide-react-layout/lib/components/map-providers/mapguide';

import { deArrayify, isQueryMapFeaturesResponse } from 'mapguide-react-layout/lib/api/builders/deArrayify';
import { setConstrucoesSelection, convertDateEdicao2Storage, convertDateStorage2Edicao } from '../../utils/utils';

const  PCC_Form_Construcao  = ({ form_construcaoedit_recId, form_construcaonew_paiId  }) => {

  // Definição Referencias para objetos DHTMLX
  const messageRef = useRef(null);
  const windowRef = useRef(null);
  const menuRef = useRef(null);
  const tabRef = useRef(null);

  const formRefgeral = useRef(null); 
  const formRefconcessionarios = useRef(null);

  const layoutRefmaingeometrias = useRef(null);
  const layoutRefgeometrias = useRef(null);
  const menuRefgeometrias = useRef(null);
  const menutreeRefgeometrias = useRef(null); 
  const treeRefgeometrias = useRef(null); 

  const layoutRegMovimentos= useRef(null); 
	const	gridRegMovimentos= useRef(null); 
	const	formRegButtoes= useRef(null); 
 
  const formstatusobjecto = useRef(null);  
  const ListaFicheirosAssociados = useRef(null);

  const ListaFicheirosAssociadosMovimento = useRef(null); 
  const FormFicheirosAssociadosMovimento = useRef(null);

  const menutreeRefFicheiros= useRef(null); 
  const layoutFicheiroGeral = useRef(null);
  const windowRegistoMovimento = useRef(null);

  const layoutFicheirosAssociadosConstrucao = useRef(null);
  const layoutFicheirosAssociadosMovimento = useRef(null);

  const formRegistoMovimentoDados = useRef(null);
  const tabRegistoMovimento = useRef(null);

  const dispatch = useReduxDispatch(); 
  // Fim Definição Referencias para objetos DHTMLX
  
  const icon_print = <FontIcon iconName="Print" />;
  const icon_delete = <Icon iconName="Delete" />;
  const icon_save = <Icon iconName="Save" />; 
  const icon_location = <Icon iconName="MapPin" />;
  const icon_desenhar = <Icon iconName="edit" />;
  const icon_poligono = <Icon iconName="Print" />;
  const icon_importar = <Icon iconName="import" />;
  const icon_minus = <FontIcon iconName="ChromeMinimize" />;
  var viewerstate = viewer.getState();
  //Get active map name
  var activeMapName = viewerstate.config.activeMapName;
  
  const icon_printHtml = ReactDOMServer.renderToStaticMarkup(icon_print);
  const icon_deleteHtml = ReactDOMServer.renderToStaticMarkup(icon_delete);
  const icon_saveHtml = ReactDOMServer.renderToStaticMarkup(icon_save);
  const icon_locationHtml = ReactDOMServer.renderToStaticMarkup(icon_location);
  const icon_minusHtml = ReactDOMServer.renderToStaticMarkup(icon_minus);

  const icon_desenharHtml = ReactDOMServer.renderToStaticMarkup(icon_desenhar);
  const icon_poligonoHtml = ReactDOMServer.renderToStaticMarkup(icon_poligono);
  const icon_importarHtml = ReactDOMServer.renderToStaticMarkup(icon_importar);

  
  const [previousConstrucaoId, setPreviousConstrucaoId] = useState(null);

  const refreshObjectos = useSelector((state)=> state.aplicacaopcc.windows.importa_construcao.obj_listageografica_refresh); 
  const refreshFicheiros = useSelector((state)=> state.aplicacaopcc.windows.associa_ficheiro.obj_listaficheiros_refresh);
   

  const apiEndpointDocumentos = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointDocumentos);
  const apiEndpointCadastro = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointCadastro);
  const apiEndpointSIG = useSelector((state)=> state.aplicacaopcc.config.configapiEndpointSIG); 
  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid); 
  const username = useSelector((state)=> state.aplicacaopcc.geral.username);
  const aplicacao_sigla = useSelector((state)=> state.aplicacaopcc.aplicacao_sigla);
  const usersession = useSelector((state)=> state.aplicacaopcc.geral.usersession);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  const layers_ligados = useSelector((state)=> state.aplicacaopcc.layers_ligados);
  const listaDeCodigosPermissoes = Perm.listaDeCodigosConstrucoes;
  const funcionalidade = useSelector((state)=> state.aplicacaopcc.funcionalidades);
  const obj_info_WKTs = useSelector((state)=> state.aplicacaopcc.windows.detalhes_objectos.obj_info_WKTs);

  const ligado_nopaper  = useSelector((state)=> state.aplicacaopcc.ligacoes_gu.ligado_nopaper);
  const ligado_airc  = useSelector((state)=> state.aplicacaopcc.ligacoes_gu.ligado_airc);
  const ligado_ano  = useSelector((state)=> state.aplicacaopcc.ligacoes_gu.ligado_ano);
  const ligado_medidata  = useSelector((state)=> state.aplicacaopcc.ligacoes_gu.ligado_medidata );
       
  // Filter permissions that have cod in listaDeCodigosPermissoes
  const funcionalidadesRelevantes = funcionalidade.filter(permission => {
    // Check if cod is in listaDeCodigosInteressantes
    return listaDeCodigosPermissoes.includes(parseInt(permission.cod));
  });
  /*
  export const FGU_AcessoEscritaArvoreConstrucoes = 3015;//, 'Acesso escrita à Árvore de Pretensões
  export const FGU_NovoGrupoAtendimento = 3710;
  export const FGU_NovoItemAtendimento = 3715;
  export const FGU_ConsultarItemAtendimento = 3713;
  export const FGU_EditarGrupoAtendimento = 3720;
  export const FGU_EditarItemAtendimento = 3725;
  export const FGU_EditarOrdemAtendimento = 3730;
  export const FGU_PesquisaAtendimento = 3735;
  export const FGU_ObterSHPAtendimento = 3740;
  export const FGU_ImportarAtendimento = 3745;
  export const FGU_AdminAtendimento = 3746;*/

  const permissaoNovoItem = true;// Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_NovoItemConstrucao, '');

  const permissaoConsultarPre = true;// Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_ConsultarItemConstrucao, '');
  const permissaoEditarItem = true;// Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_EditarItemConstrucao, '');
  const permissaoNovoMovimento= true;

  const permissaoEditarRegistoMovimento = true;
  const permissaoNovoRegistoMovimento= true;

  //
  
  // Definição objetos guardar informação
  const [dadosConstrucao, setDadosConstrucao] = useState(null);
  const [dadosTipoConstrucao, setDataTipoConstrucao] = useState(null);
  const [dadosTipoMovimento, setDadosTipoMovimento] = useState(null);
  
  const [datageom, setDataGeom] = useState(null); 
  const [dadosMovimentos, setDadosMovimentos] = useState(null); 
   
  /*
  var Objecto_id = ''; 
  var ObjectoGeografico_id=''; 
  var ObjectoGeografico_centroid=''; 
  var ObjectoGeografico_mbr='';  */
  let objectoid='';
  let movimentoid='';
  let permitemovimentos=false;
  let centroid='';
  const [Objecto_id, setObjecto_id] = useState(null); 

   
  // Definição objetos guardar informação dos layers
    //const addedlayers = this.state.addedlayers.join('|');
  // const removedlayers = this.state.removedlayers.join('|');
  const [addedlayers, setaddedlayers] = useState(null);
  const [removedlayers, setremovedlayers] = useState(null);

  const [prevState_checkedIds, setprevState_checkedIds] = useState(null);
      
  const novoProcessoAPI = () => { 
    console.log('novo Processo');
    
    var aux={ type: Actions.PROCESSOS_NOVOPROCESSO, payload: "" };
    viewer.dispatch(aux);  
  }; 
  const terminouProcessoAPI = () => { 
    console.log('terminou Processo');
    
    var aux={ type: Actions.PROCESSOS_TERMINAPROCESSO, payload: "" };
    viewer.dispatch(aux); 
    
  };

  const fetchConcessionario = async (nif) => {  
  
    const url = apiEndpointCadastro + 'Concessionarios/nif/' + nif;
    const jwtToken = authtoken;
    fetch(url, {
      headers: {
        'Authorization': `Bearer ${jwtToken}`
      }
    })
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then(jsonData => {
      formRefgeral.current.setValue({ "nome": '' });
      formRefgeral.current.setValue({ "nome": jsonData[0].nome });
    })
    .catch(error => {
      console.error('There was a problem with your fetch operation:', error);
    }); 
  };
  const fetchDataMovimentos = async () => {
    const url3 = apiEndpointCadastro + 'Movimento/PorConstrucao/' + form_construcaoedit_recId;  
    const jwtToken = authtoken; 
    const response = await fetch(url3, {
      method: 'GET', 
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${jwtToken}`
      },
    });  
    const json = await response.json(); 
    if (json?.status==404){ 
    } else{
      setDadosMovimentos(json);
    }       
  }
  // useEffect quando muda o form_construcaoedit_recId - Reler dados
  useEffect(() => {
    if ((form_construcaoedit_recId !== previousConstrucaoId)&&((form_construcaoedit_recId !="")&&(form_construcaonew_paiId==""))) {

      const fetchData = async () => {
        const url = apiEndpointCadastro + 'Construcao/' + form_construcaoedit_recId;
        const jwtToken = authtoken; 
        const response = await fetch(url, {
					method: 'GET', 
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`
          },
        });              
					 
        const jsonData = await response.json();
        centroid = jsonData.centroid;
        //console.log('resposta fetchData : ' + JSON.stringify(jsonData)); 
        setDadosConstrucao(jsonData);   
        setDataGeom(jsonData);
      };
      const fetchDataTipoConstrucao = async () => {//ok
        const url3 = apiEndpointCadastro + 'TipoConstrucao';//ok 
        const jwtToken = authtoken; 
        const response = await fetch(url3, {
					method: 'GET', 
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`
          },
        });              
					 
        const json = await response.json();  
        setDataTipoConstrucao(json); 
         
      } 
      const fetchDataListaDeFicheiros= async () => {
        let datalist= []; 
        let datalist2 = [];  
        let url3 = apiEndpointDocumentos + 'FicheiroAssociado/construcao/' + form_construcaoedit_recId;
        // const response = await fetch(url3);
        let jwtToken = authtoken;
        //const json = await response.json();
        fetch(url3, {
          headers: {
            'Authorization': `Bearer ${jwtToken}`
          }
        })
        .then(response => {
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
          return response.json();
        })
        .then(jsonData => {
          datalist = jsonData;
          let Ficheiros =[]; 
          
          Ficheiros = datalist.map((item, index) => ({
            ...item,
            id: item.RecId, 
            value: item.descricaoDocumento + '(' + item.nomeDocumento +')',       // usa o campo "nome" do JSON como o valor exibido
          }));
          ListaFicheirosAssociados.current.data.removeAll();
          ListaFicheirosAssociados.current.data.parse(Ficheiros); 
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        }); 
      }
      fetchData(); 
      fetchDataTipoConstrucao();
      fetchDataListaDeFicheiros();
      fetchDataMovimentos();
      setPreviousConstrucaoId(form_construcaoedit_recId);      
    }
    if (((form_construcaoedit_recId =="")&&(form_construcaonew_paiId!=""))) {
      const fetchDataTipoConstrucao = async () => {//ok
        const url3 = apiEndpointCadastro + 'TipoConstrucao';//ok 
        const jwtToken = authtoken; 
        fetch(url3, {
          headers: {
            'Authorization': `Bearer ${jwtToken}`
          }
        })
        .then(response => {
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
          return response.json();
        })
        .then(jsonData => {
          setDataTipoConstrucao(jsonData); 
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        });  
      } 
      fetchDataTipoConstrucao() 
    }
  }, [form_construcaoedit_recId, previousConstrucaoId]);
  
  useEffect(() => {
    if (refreshObjectos){
      const fetchData = async () => {
        const url = apiEndpointCadastro + 'Construcao/' + form_construcaoedit_recId;
        const jwtToken = authtoken; 
        const response = await fetch(url, {
					method: 'GET', 
					headers: {
						'Content-Type': 'application/json',
						'Authorization': `Bearer ${jwtToken}`
          },
        });              
					 
        const jsonData = await response.json();
        centroid = jsonData.centroid;
        //console.log('resposta fetchData : ' + JSON.stringify(jsonData)); 
        setDadosConstrucao(jsonData);   
        setDataGeom(jsonData);
      };
      if (form_construcaoedit_recId!=''){
        dispatch({type: Actions.REFRESH_END_IMPORTAFILE_CONSTRUCAO, payload: ""});
        fetchData();
      } 
    }
  }, [refreshObjectos]);


 

  // useEffect quando muda os dados lidos
  useEffect(() => {
    // Update the form when dadosConstrucao.nif changes
    if (formRefgeral.current!=null) {
      //formRefgeral.current.setValue({ "ext_id": dadosConstrucao?.ext_id });
      windowRef.current.header.data.update("title", { value: 'Construção (' + dadosConstrucao?.designacao + ')' } ) 
      //var htmlx=windowRef.current.getContainer();
      formRefconcessionarios.current.setValue({ "nif": dadosConstrucao?.nif });
      formRefconcessionarios.current.setValue({ "nome": dadosConstrucao?.nome });
      formRefgeral.current.setValue({ "designacao": dadosConstrucao?.designacao });      
      formRefgeral.current.setValue({ "observacoes": dadosConstrucao?.descricao });       
      formRefgeral.current.setValue({ "rec_id": dadosConstrucao?.recId });
      formRefgeral.current.setValue({ "parent_id": dadosConstrucao?.talhaoId }); 
      formRefgeral.current.setValue({ "tipoconstrucao_id": dadosConstrucao?.tipoconstrucaoId }); 
      formRefgeral.current.setValue({ "construcao_bbox": dadosConstrucao?.bbox });
      formRefgeral.current.setValue({ "construcao_centroid": dadosConstrucao?.centroid });   
      const state = formRefgeral.current.getValue();     
      let permissaoEscritaaux = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_AcessoEscritaArvoreConstrucoes, 'G:'+dadosConstrucao?.parent_id);      
      console.log(permissaoNovoItem);
      console.log(permissaoEditarItem);
      console.log(permissaoEscritaaux);
      
      let permissaoEditarItemaux = true;//permissaoEditarItem && permissaoEscritaaux;
      if(permissaoNovoItem){
        console.log("Perm ok");
        if(permissaoConsultarPre && !permissaoEditarItemaux){ 
          formRefgeral.current.getItem("designacao").disable();
          formRefconcessionarios.current.getItem("nif").disable();
          formRefconcessionarios.current.getItem("nome").disable(); 
          formRefgeral.current.getItem("observacoes").disable(); 
          formRefgeral.current.getItem("tipoconstrucao").disable(); 
          windowRef.current.header.disable('gravar');
          windowRef.current.header.disable('eliminar');
          menuRefgeometrias.current.disable(["desenhar"]);
          menuRefgeometrias.current.disable(["importar"]);
        }
      }else{
        if(permissaoConsultarPre && !permissaoEditarItemaux){
          formRefgeral.current.getItem("designacao").disable();
          formRefconcessionarios.current.getItem("nif").disable();
          formRefconcessionarios.current.getItem("nome").disable();
         
          formRefgeral.current.getItem("observacoes").disable();
           
          formRefgeral.current.getItem("tipoconstrucao").disable();  

          windowRef.current.header.disable('gravar');
          windowRef.current.header.disable('eliminar'); 
          menuRefgeometrias.current.disable(["desenhar"]);
          menuRefgeometrias.current.disable(["importar"]);
        }
      }
    }
  }, [dadosConstrucao?.recId]);
 
  // useEffect para atualizar a combo do tipo de intervenção com os dados lidos
  useEffect(() => {

    const transformResponse = (originalResponses) => {
    
      const transformedData = []; 
      let counter = 1;
      if (Array.isArray(originalResponses)){
        if (originalResponses.length==0){
          transformedData[0] = transformedData[0] || {
            value: "Tipos de Construção",
            id: "update",
          };
        } else {
          // Loop through each original response record
          originalResponses.forEach((originalResponse) => {
            // Extract relevant fields from the original response
            const { recId, designacao, observacao, movimentosn} = originalResponse;
            // Check if "descricao" is null or an empty string
            let transformedDescricao = designacao;
            
            if (designacao === null || designacao === '') {
              transformedDescricao = `Tipo Construção ${counter}`;
              counter++; // Increment the counter
            }
            if (recId !== 0 ) {
              // Create the desired structure for each record
              const transformedRecord = {
                value: transformedDescricao, // Use the "descricao" field as the "value"
                id:  recId, // Use the "recId" field as the "id"
                observacao:observacao, 
                movimentosn: movimentosn                
              };

              transformedData.push(transformedRecord);
            }
            

          });
        }
      } else {
        // Extract relevant fields from the original response
        const { recId, designacao, observacao, movimentosn} = originalResponses;
        // Check if "descricao" is null or an empty string
        let transformedDescricao = designacao; 

        if (designacao === null || designacao === '') {
          transformedDescricao = `Tipo Construção ${counter}`;
          counter++; // Increment the counter
        }
        // Create the desired structure for each record
        const transformedRecord = {
          value: transformedDescricao, // Use the "descricao" field as the "value"
          id:   recId, // Use the "recId" field as the "id"   
          observacao:observacao, 
          movimentosn: movimentosn          
        };
       
        transformedData.push(transformedRecord);
      }   
      return transformedData;
    }     
    // Update the form when data.nif changes
    if (formRefgeral.current!=null) {
      
      const transformedResult = transformResponse(dadosTipoConstrucao); 
        
      var myComboBox= formRefgeral.current.getItem('tipoconstrucao').getWidget();
      myComboBox.data.parse(transformedResult);
      const valor =  parseInt(dadosConstrucao?.tipoconstrucaoId , 10) || 0;
      if (valor!=''){
        myComboBox.setValue(valor);
        myComboBox.disable();
      }
      else {
        myComboBox.setValue('-1');
      }
      if (myComboBox.data.getIndex(valor)==-1){
        myComboBox.setValue('-1');
      }else { 
        let item=myComboBox.data.getItem(valor);
        console.log(item);
        permitemovimentos=item.movimentosn;
        if (!permitemovimentos){
          tabRef.current.removeTab("movimentos");
        }else {
          tabRef.current.enableTab("movimentos");
        }
     }
       
    }
  }, [dadosTipoConstrucao && dadosConstrucao?.tipoconstrucaoId]);
 
  useEffect(() => {
    if (refreshFicheiros){
      const fetchDataListaDeFicheiros= async () => {
        let datalist= []; 
        let datalist2 = [];  
        let url3 = apiEndpointDocumentos + 'FicheiroAssociado/construcao/' + form_construcaoedit_recId;
        // const response = await fetch(url3);
        let jwtToken = authtoken;
        //const json = await response.json();
        fetch(url3, {
          headers: {
            'Authorization': `Bearer ${jwtToken}`
          }
        })
        .then(response => {
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
          return response.json();
        })
        .then(jsonData => {
          datalist = jsonData;   
          let Ficheiros =[]; 
        
          Ficheiros = datalist.map((item, index) => ({
            ...item,
            id: item.RecId, 
            value: item.descricaoDocumento + '(' + item.nomeDocumento +')',       // usa o campo "nome" do JSON como o valor exibido
          }));
          ListaFicheirosAssociados.current.data.removeAll();
          ListaFicheirosAssociados.current.data.parse(Ficheiros); 
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        }); 
      }
      fetchDataListaDeFicheiros();
    }
  }, [refreshFicheiros]); 

  useEffect(() => {
    const transformResponse = (originalResponses) => {
      const transformedData = [];
      if (Array.isArray(originalResponses)){
        originalResponses.forEach((originalResponse) => {
          const { recId, dataMovimento, construcaodestinoId, tipomovimentoId, tipomovimentoNome, residenteId, residenteNome,
            residente_Datanascimento, residente_Datafalecimento, residente_Datainumacao } = originalResponse; 
          const transformedRecord = {
            recId: recId,  
            dataMovimento: convertDateStorage2Edicao(dataMovimento),
            construcaodestinoId: construcaodestinoId,
            tipomovimentoId: tipomovimentoId,  
            tipomovimentoNome: tipomovimentoNome,
            residenteId: residenteId,
            residenteNome: residenteNome,
            residente_Datanascimento: convertDateStorage2Edicao(residente_Datanascimento),
            residente_Datafalecimento: convertDateStorage2Edicao(residente_Datafalecimento),
            residente_Datainumacao: convertDateStorage2Edicao(residente_Datainumacao)
          };
          transformedData.push(transformedRecord); 
        });
      } else {
          const { recId, dataMovimento, construcaodestinoId, tipomovimentoId, tipomovimentoNome, residenteId, residenteNome,
            residente_Datanascimento, residente_Datafalecimento, residente_Datainumacao } = originalResponses; 
          const transformedRecord = {
            recId: recId,  
            dataMovimento: convertDateStorage2Edicao(dataMovimento),
            construcaodestinoId: construcaodestinoId,
            tipomovimentoId: tipomovimentoId,  
            tipomovimentoNome: tipomovimentoNome,
            residenteId: residenteId,
            residenteNome: residenteNome,
            residente_Datanascimento: convertDateStorage2Edicao(residente_Datanascimento),
            residente_Datafalecimento: convertDateStorage2Edicao(residente_Datafalecimento),
            residente_Datainumacao: convertDateStorage2Edicao(residente_Datainumacao)
          };
          transformedData.push(transformedRecord); 
      }    
      return transformedData;
    }
    if (formRefgeral.current!=null) {
      
      if (gridRegMovimentos.current !=null ){
        const dadostransformados = transformResponse(dadosMovimentos); 
    
        gridRegMovimentos.current.data.parse(dadostransformados);
      } 
    }
  }, [dadosMovimentos]); 

  // useEffect para criar o formulario
  useEffect(() => {
    if (((form_construcaoedit_recId !="")&&(form_construcaonew_paiId==""))||((form_construcaoedit_recId =="")&&(form_construcaonew_paiId !=""))) {
      //verificamos se é uma nova construcao
      let nova_construcao=false;
      if (((form_construcaoedit_recId =="")&&(form_construcaonew_paiId !=""))) {
        nova_construcao=true;
      }      

      const windowHtmlContainer = `<div id="formContainer"></div>`;
      windowRef.current = new WindowDHX({
        width: 750,
        height: 600,
        title: 'Ficha de Construção', 
        closable: true,
        movable: true,
        resizable: false,
        modal: false,
        header: true,
        footer: true,
        css: "pcc_window",
      });
      globalWindowReference = windowRef.current;
 
      windowRef.current.header.data.add({
        type: "button",
        view: "link",
        size: "medium",
        color: "primary", 
        disabled: (nova_construcao || !permissaoEditarItem),  
        value: "Gravar",
        tooltip: "Gravar",
        title: "Gravar",
        id: "gravar",
        css: "pcc_button_save dhx_button--circle", 
        html: icon_saveHtml
      }, 2);
      windowRef.current.header.data.add({
          type: "button",
          view: "link",
          size: "medium",
          color: "primary", 
          disabled: (nova_construcao || !permissaoEditarItem), 
          value: "Eliminar",
          tooltip: "Eliminar",
          title: "Eliminar",
          id: "eliminar",
          css: "pcc_button_delete dhx_button--circle",
          html: icon_deleteHtml
      }, 3);
      windowRef.current.header.data.add({
        type: "button",
        view: "link",
        size: "medium",
        color: "primary", 
        disabled: nova_construcao,
        value: "Imprimir",
        tooltip: "Imprimir",
        color: "primary", 
        id: "imprimir",
        css: "pcc_button_delete dhx_button--circle",
        html: icon_printHtml
      }, 4);
      windowRef.current.header.data.add({
        type: "button",
        view: "link",
        size: "medium",
        color: "primary", 
        disabled: nova_construcao,
        tooltip: "Localizar",
        value: "Localizar",
        title: "Localizar",
        id: "localizar",
        css: "pcc_button_location dhx_button--circle",
        html: icon_locationHtml
      }, 5);
      windowRef.current.header.data.add({
        type: "button",
        view: "link",
        size: "medium",
        color: "primary", 
        disabled: false,
        tooltip: "Minimizar",
        value: "Minimizar",
        title: "Minimizar",
        id: "minimizar",
        css: "pcc_button_minimizar dhx_button--circle",
        html: icon_minusHtml
      }, 6);
      //windowRef.current.header.disable('close');
      //windowRef.current.header.hide('imprimir');
   
      // criar as tabulações
      tabRef.current = new TabbarDHX(null,{
        css: "pcc_tab",
        tabs:[]
      });
      tabRef.current.addTab({ id: "geral", tab: "Geral"}, 4); 
      tabRef.current.addTab({ id: "movimentos", tab: "Movimentos mortuários"}, 6);
      tabRef.current.addTab({ id: "geograficos", tab: "Dados Geográficos"}, 7);
      tabRef.current.addTab({ id: "ficheiros", tab: "Ficheiros"}, 7);
      // criar o formulário geral

      
      formRefgeral.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        padding: 17,
        rows: [
          {
            css: "pcc_input",type: "fieldset", label: "Informação geral: ", name: "g3", disabled: false, hidden: false ,
            rows:[  { type: "spacer", name: "spacer" ,  height:"10px"}, 
              {
                css: "pcc_input",type: "combo", name: "tipoconstrucao", label: "Tipo de construção", labelPosition: "left" ,  readOnly:true,
                data: dadosTipoConstrucao 
              },
              { css: "pcc_input", type: "input", name: "designacao", label: "Designação", labelPosition: "left" ,placeholder: "Designação", readOnly: false,  value:'', required: true},
              { css: "pcc_input",  type: "textarea", name:"observacoes", label: "Observações",
                  placeholder: "Observações", value: "", height: 100, maxlength: 950}
            ] 
          },  
           {
            css: "pcc_input", type: "fieldset", label: "Concessionário: ", name: "g1", disabled: false, hidden: false, value:'',
            rows: 
            [ 
            { rows: 
            [ 
              { css: "pcc_input", type: "input", name: "nif", label: "NIF", labelPosition: "left" , placeholder: "Nº Identificação Fiscal", readOnly: false,  validation: "integer", width: "200px",  value:'',},
              { type: "spacer", name: "spacer" , width: "20px" }, 
              { css: "pcc_input",type: "input", name: "nome", label: "Nome", labelPosition: "left" , placeholder: "Nome", readOnly: true, width: "300px",  value:'',},   
              { type: "spacer", name: "spacer" , width: "30px" },  
              ] 
            }, 
          ]}, 
             
          {
            css: "pcc_input",type: "fieldset", label: "Dados auxiliares:", name: "g3", disabled: true, hidden: true, 
            rows:[  
                { css: "pcc_input",type: "input", name: "ext_id", label: "ext_id", placeholder: "ext_id",  value:'', },
                { css: "pcc_input",type: "input", name: "rec_id", label: "rec_id", placeholder: "rec_id", value:"0"},
                { css: "pcc_input",type: "input", name: "parent_id", label: "parent_id", placeholder: "parent_id",  value:'', },
                { css: "pcc_input",type: "input", name: "documento_id", label: "documento_id", placeholder: "documento_id",  value:'', },
                { css: "pcc_input",type: "input", name: "entidade_id", label: "entidade_id", placeholder: "entidade_id" ,  value:'',},
                { css: "pcc_input",type: "input", name: "tipoconstrucao_id", label: "tipoconstrucao_id", placeholder: "tipoconstrucao_id",  value:'', },
                { css: "pcc_input",type: "input", name: "estadoprocesso_id", label: "estadoprocesso_id", placeholder: "estadoprocesso_id",  value:'', },
                { css: "pcc_input",type: "input", name: "construcao_bbox", label: "construcao_bbox", placeholder: "construcao_bbox",  value:'', },
                { css: "pcc_input",type: "input", name: "construcao_centroid", label: "construcao_centroid", placeholder: "construcao_centroid",  value:'', },
                { css: "pcc_input",type: "input", name: "objectogeografico_recid", label: "objectogeografico_recid", placeholder: "objectogeografico_recid",  value:'', },
                { css: "pcc_input",type: "input", name: "objectogeografico_nome", label: "objectogeografico_nome", placeholder: "objectogeografico_nome",  value:'', },
                { css: "pcc_input",type: "input", name: "objectogeografico_mbr", label: "objectogeografico_mbr", placeholder: "objectogeografico_mbr",  value:'', },
                { css: "pcc_input",type: "input", name: "objectogeografico_centroid", label: "objectogeografico_centroid", placeholder: "objectogeografico_centroid",  value:'', },
                { css: "pcc_input",type: "input", name: "objectogeografico_tema_id", label: "objectogeografico_tema_id", placeholder: "objectogeografico_tema_id",  value:'', },
                { css: "pcc_input",type: "input", name: "objectogeografico_tema_nome", label: "objectogeografico_tema_nome", placeholder: "objectogeografico_tema_nome",  value:'', },
            ] 
          }
        ] 
      }); 
      formRefgeral.current.getItem("designacao").events.on("change", function(value) {
        let aux_designacao =formRefgeral.current.getItem("designacao")._value;
        if (aux_designacao!=''){
          windowRef.current.header.enable('gravar');
        } else {
          windowRef.current.header.disable('gravar');
        }
      });
      formRefgeral.current.getItem("nif").events.on("change", function(value) {
        let aux_nif=formRefgeral.current.getItem("nif")._value;
        fetchConcessionario(aux_nif);
        
      });
 
      // criar o formulário concessionario
      formRefconcessionarios.current = new dhx.Form(null, {
        css: "dhx_widget--bg_white dhx_widget--bordered",
        padding: 40,
        rows: [ 
            {
            css: "pcc_input", type: "fieldset", label: "Concessionário: ", name: "g1", disabled: false, hidden: false, value:'',
            rows: 
            [ 
            { rows: 
            [ 
              { css: "pcc_input", type: "input", name: "nif1", label: "NIF", placeholder: "Número Identificação Fiscal", readOnly: true,  validation: "integer", width: "200px",  value:'',},
              { type: "spacer", name: "spacer" , width: "20px" }, 
              { css: "pcc_input",type: "input", name: "nome1", label: "Nome", placeholder: "Nome", readOnly: true, width: "300px",  value:'',},   
              { type: "spacer", name: "spacer" , width: "30px" }, 
              { rows: 
                [    
              { type: "spacer", name: "spacer" , height:"28px"} ,   
              { type: "button", name: "criar_entidade1", circle: true, hidden: false, text: "Gerir" , size: "small", view: "flat", color: "primary"  } ,//entidade 
              ] 
              }, 
              ] 
            }, 
          ]}, 
        ]
      });
      // criar a árvore das geometrias
      treeRefgeometrias.current = new TreeDHX(null,{
        checkbox: false 
      });      
      // criar o menu das geometrias
      menuRefgeometrias.current = new ToolbarDHX(null, {
        css: "dhx_widget--bordered dhx_widget--bg_gray",
        data: [
          { id:"desenhar", value: "Desenhar", icon: "bp3-icon bp3-icon-Print",
          items: [
            { id:"pg", value: "Polígono", },// icon: icon_poligonoHtml, }, 
            { id:"pl", value: "Linha", },// icon: "bp3-icon bp3-icon-delete", }, 
            { id:"pt", value: "Ponto", },// icon: "ms-Icon ms-Icon--Search", },
            { id:"rt", value: "Rectângulo", },// icon: "dxi dxi-delete", },
            { id:"ci", value: "Circulo", },// icon: "dxi dxi-LocationCircle", },     
          ]}, 
          { type: "separator" } ,
          { id:"importarficheiro",value: "Importar ficheiro", //, icon: "ms-Icon ms-Icon--import",
         /* items: [
            { id:"importarficheiro", value: "Importar ficheiro", },//, icon: "dxi dxi-undo",},
            { type: "separator" } ,           
            { id:"seleccionar", value: "Importar objecto seleccionado",] },*/
          },
        ]
      });
       
   
      let definicoesItems =[];
      if (dadosTipoConstrucao!=undefined){
          // Construa os itens do menu "Definição" com base nos dados da variável JSON
          definicoesItems = dadosTipoConstrucao.map((item, index) => ({
          id: `def${index + 1}`, // gera ids dinâmicos, ex: def1, def2, ...
          value: item.nome,       // usa o campo "nome" do JSON como o valor exibido
        }));

      }
    
      // Adicione o item "Sem definição" manualmente
      definicoesItems.push(
        { type: "separator" }, 
        { id: "semdefinicao", value: "Sem definição" }
      );
      // criar o menu na tree das geometrias
      menutreeRefgeometrias.current = new ContextMenuDHX(null, {
        css: "dhx_widget--bg_gray",
        data: [ 
          { id:"localizar", value: "Localizar" }, 
          { type: "separator" } , 
          { id:"exportar", value: "Exportar", }, 
          { type: "separator" }, 
          { id:"remover", value: "Remover", }, 
        ]
      });
       
      layoutRefmaingeometrias.current = new LayoutDHX(null,{
        type: "line", 
        cols: [
            {
                id: "geometrias",
                css: "pcc_layout",
                progressDefault: true,
                width: "45%" 
            } , {
            header: "Informações do objecto", collapsable:false ,
            id: "statusobjecto",
            html: "",
            css: "pcc_layout", 
            progressDefault: true, 
            height:"100%"
          } 
           ]},  
      );
         
      layoutRefgeometrias.current = new LayoutDHX(null,{
        type: "line",
        rows: [
            {
                id: "menugeo",
                css: "pcc_layout",
                height: "content",
                progressDefault: true,
               //html: "menugeo"
            } , {
              id: "treegeo",
              html: "treegeo",
              css: "pcc_layout",
              progressDefault: true,
          }  
           ]
      });
 
      let datalist= []; 
      let datalist2 = []; 
      ListaFicheirosAssociados.current= new ListDHX(null,{
        css: "dhx_widget--bg_white dhx_widget--bordered formlist",
        //template: template, //takes function "template"
        //itemHeight: itemHeight,
        data: datalist, 
      });
  
      layoutFicheirosAssociadosConstrucao.current = new LayoutDHX(null,{
        type: "line",
        rows: 
        [   
            {
              id: "dadosent1",
              html: "dadosent1",
              header:"Lista dos ficheiros:",  
              progressDefault: true,
              padding: 10,
              height: 175,
            },
            {
              id: "dadosent2",
              html: "dadosent2",
              header:"Dados do ficheiro associado:",  
              progressDefault: true,
              padding: 10,
            } 
        ]
      });
      layoutFicheiroGeral.current = new LayoutDHX(null,{
        type: "line",
        rows: [
          {
            id: "dadosent",
            html: "dadosent",
            css: "pcc_layout2",
            progressDefault: true,
          }
      ]});

      const FormFicheirosAssociados = new FormDHX(null, { 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        padding: 10,
        
        rows: [
          
          { 
            css: "pcc_input",
            type: "input", 
            name: "descricao", 
            label: "Descrição", 
            placeholder: "Nome", 
            disabled: true,
            labelPosition: "left"  
          },
          {
            cols: [   
              { 
                css: "pcc_input",
                type: "datepicker", 
                name: "dataarquivo",  
                label: "Data",  
                dateFormat: "20%y/%m/%d",
                disabled: true ,
                labelPosition: "left"            
              },        
              {
                css: "pcc_input",
                type: "input", 
                name: "arquivdir", 
                label: "Ficheiro", 
                placeholder: "Ficheiro", 
                hidden: true,
                labelPosition: "left" 
              },
              { 
                css: "pcc_input_ir", 
                id: "download_ficheiro", 
                type: "button", 
                name: "download_ficheiro", 
                value: "Descarregar",
                tooltip: "Descarregar",
                title: "Descarregar",
                hidden: true, 
                circle: true,
                size: "small", 
                view: "flat", 
                color: "primary",
              }],
          },  
          { 
            css: "pcc_input",
            type: "input", 
            name: "observficheiro", 
            label: "Observações:", 
            placeholder: "Observações",
            disabled: true,
            labelPosition: "left"  
          },
          {
            cols: [           
              { 
                css: "pcc_input_addficheiro", 
                id: "buttonAdicionarFicheiro", 
                type: "button", 
                name:"upload_ficheiro", 
                value: "Adicionar", 
                tooltip: "Adicionar", 
                title: "Adicionar", 
                circle: true,
                size: "small", 
                view: "flat", 
                color: "primary"
              }, 
            ], 
          },
          {
            cols: [           
              { 
                css: "pcc_input_addficheiro", 
                id: "buttonGravarFicheiro", 
                type: "button", 
                name:"save_ficheiro", 
                value: "Adicionar", 
                tooltip: "Adicionar", 
                title: "Adicionar", 
                circle: true,
                size: "small", 
                view: "flat", 
                color: "primary",
                hidden: true,
              }
            ], 
          }  
        ]
      });
      
    
      FormFicheirosAssociados.getItem("download_ficheiro").events.on("click", function(value) {
      
        
        
        const getitemficheiro = async () => {
          const jwtToken = authtoken;
          const url3 = apiEndpointDocumentos  + 'FicheiroAssociado/' + reciditemficheiro;
          const response = await  fetch(url3, {
            headers: {
              'Authorization': `Bearer ${jwtToken}`
            }
          });
    

          if (response.ok) {
      
            // Read the response as a blob
            const fileBlob = await response.blob();
             // Obtém o tipo de conteúdo do cabeçalho da resposta
            const contentType = response.headers.get('Content-Type');

            // Cria um URL para o blob, usando o tipo de conteúdo original
            const fileUrl = URL.createObjectURL(fileBlob, { type: contentType });
 
            const downloadLink = document.createElement('a');
            downloadLink.href = fileUrl;
            downloadLink.download = itemficheironame;
            downloadLink.click();
            // Limpa o URL do objeto para liberar memória
            URL.revokeObjectURL(fileUrl);

            dhx.alert({
              header: "Download",
              text: "Foi efectuado o download do ficheiro.",
              buttonsAlignment: "center",
              buttons: ["ok"]
            });
          };
        };
        getitemficheiro();
         
      
      });
   
      FormFicheirosAssociados.getItem("upload_ficheiro").events.on("click", function(value) {
  
        var aux={ type: Actions.SHOW_ASSOCIAFILE, payload: { registo_id: form_construcaoedit_recId , tipoassociacao: 'construcao' }};
        viewer.dispatch(aux); 

      });
   
      FormFicheirosAssociados.getItem("save_ficheiro").events.on("click", function(value) {
        const formValues = FormFicheirosAssociados.getValue(); 
        const obj_associaficheiro_id = form_construcaoedit_recId;
        const descricao = formValues.descricao; 
        const dataarquivo = formValues.dataarquivo;
        const observficheiro = formValues.observficheiro; 
        // Agora você tem os valores dos campos
        console.log("Descrição:", descricao); 
        console.log("Data:", dataarquivo);
        console.log("Observações:", observficheiro)
      
        if(descricao ==""){
          dhx.alert({
            header: "Falta Descrição",
            text: "O item Descrição é necessario!",
            buttonsAlignment: "center",
            buttons: ["ok"]
          });
        }else{
          //enviarDadosParaServidor(descricao, dataarquivo, observficheiro, obj_associaficheiro_id);
        }
      }); 
      formstatusobjecto.current = new dhx.Form(null, {
        css: "dhx_widget--bg_white dhx_widget--bordered", 
        rows: [ 
            {
                css: "pcc_input",
                type: "textarea",
                name:"statusobjecto", 
                value: "", 
                readOnly: true,
                height:"100%",
                width:"100%"
            }]
      });
      // layout - acima sao os botoes abaixo a grelha com os registos
      layoutRegMovimentos.current = new LayoutDHX(null,{
        type: "line",
        rows: [
        {
          id: "grid2",
          html: "grid2",
        },
        {
            id: "grid1",
            css: "pcc_layout",
            //html: "grid1"
        }
        ]
      }); 
      // criar area botoes para adicionar novos registos de movimentos
      formRegButtoes.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
        rows: [
        {
          cols: [
            { type: "spacer", name: "spacer" },
            {css: "pcc_input_reg", id: "novo_registomovimentos", type: "button", value: "Novo Movimento", tooltip: "Novo Movimento", title: "Novo Registo", circle: true,size: "small", view: "flat", color: "primary" },
            {css: "pcc_input_reg", id: "edit_registomovimentos", type: "button", value: "Editar Movimento", tooltip: "Editar Movimento", title: "Editar Registo" , circle: true,size: "small", view: "flat", color: "primary"},
            {css: "pcc_input_reg", id: "delete_registomovimentos", type: "button", value: "Eliminar Movimento", tooltip: "Eliminar Movimento", title: "Eliminar Registo", circle: true,size: "small", view: "flat", color: "primary" },
            {css: "pcc_input_reg", id: "print_registomovimentos", type: "button", value: "Imprimir Movimento", tooltip: "Imprimir Movimento", title: "Imprimir Registo", circle: true,size: "small", view: "flat", color: "primary" },  
          ]
        }
        ] 
      });
      // grelha com os registos de movimentos
      gridRegMovimentos.current = new GridDHX(null,{ 
        css: "pcc_grid",
        columns: [
          { width: 75, id: "recId", header: [{ text: "Id" }], hidden: true },
          { width: 100, id: "dataMovimento", header: [{ text: "Data" , tooltip: "Data movimento"}] },
           { width: 75, id: "residenteId", header: [{ text: "Residente_id" }], hidden: true },
          { width: 155, id: "residenteNome", header: [{ text: "Nome" }] },
          { width: 100, id: "residente_Datanascimento", header: [{ text: "Nascimento" }] },
          { width: 100, id: "residente_Datafalecimento", header: [{ text: "Falecimento" }] },
          { width: 100, id: "residente_Datainumacao", header: [{ text: "Inumação" }] },
          { width: 100, id: "tipomovimentoId", header: [{ text: "TipoMov_id." }] , hidden: true },
          { width: 100, id: "tipomovimentoNome", header: [{ text: "TipoMov." }] },
          { width: 100, id: "construcaodestinoId", header: [{ text: "ConstrucaoDestino" }], hidden: true }

        ],
        selection: "row",
        headerRowHeight: 50 
      });

      layoutRefmaingeometrias.current.getCell("geometrias").attach(layoutRefgeometrias.current);
      layoutRefmaingeometrias.current.getCell("statusobjecto").attach(formstatusobjecto.current); 
 
      
      layoutRegMovimentos.current.getCell("grid1").attach(gridRegMovimentos.current);

      layoutRegMovimentos.current.getCell("grid2").attach(formRegButtoes.current);

      tabRef.current.getCell("geral").attach(formRefgeral.current); 

      tabRef.current.getCell("movimentos").attach(layoutRegMovimentos.current);

      layoutRefgeometrias.current.getCell("menugeo").attach(menuRefgeometrias.current);
      layoutRefgeometrias.current.getCell("treegeo").attach(treeRefgeometrias.current); 
       
      formRefgeral.current.setValue({ "observacoes": ''});

      tabRef.current.getCell("geograficos").attach(layoutRefmaingeometrias.current);  
      tabRef.current.getCell("ficheiros").attach(layoutFicheiroGeral.current);
      layoutFicheiroGeral.current.getCell("dadosent").attach(layoutFicheirosAssociadosConstrucao.current); 
      layoutFicheirosAssociadosConstrucao.current.getCell("dadosent1").attach(ListaFicheirosAssociados.current);
      layoutFicheirosAssociadosConstrucao.current.getCell("dadosent2").attach(FormFicheirosAssociados); 

      formRefgeral.current.getItem('tipoconstrucao').events.on("change", function(ids) {
 
      formRefgeral.current.setValue({ "tipoconstrucao_id": ids });
      });
      gridRegMovimentos.current.events.on("cellClick", function(row, column,e){
        console.log(row);
        movimentoid=row.recId;
        /*recId: recId,  
            dataMovimento: convertDateStorage2Edicao(dataMovimento),
            construcaodestinoId: construcaodestinoId,
            tipomovimentoId: tipomovimentoId,  
            tipomovimentoNome: tipomovimentoNome,
            residenteId: residenteId,
            residenteNome: residenteNome,
            residente_Datanascimento: convertDateStorage2Edicao(residente_Datanascimento),
            residente_Datafalecimento: convertDateStorage2Edicao(residente_Datafalecimento),
            residente_Datainumacao: convertDateStorage2Edicao(residente_Datainumacao)*/
      });
      gridRegMovimentos.current.events.on("cellDblClick", function(row, column,e){
        setMovimentoid(row.recId);
        if (permissaoEditarRegistoMovimento) { 
          EditarMovimento(row.recId);
        }else{
          dhx.alert({
            header:"Permissão Necessária",
            text:"É necessário permissão para editar movimento!",
            buttonsAlignment:"center",
            buttons:["ok"],
          });
        } 
      });
      formRegButtoes.current.getItem("novo_registomovimentos").events.on("click", function(events) {
       
        if (permissaoNovoMovimento) { 
          EditarMovimento(form_construcaoedit_recId,'');
        }else{
          dhx.alert({
            header:"Permissão Necessária",
            text:"É necessário permissão para criar movimento!",
            buttonsAlignment:"center",
            buttons:["ok"],
          });
        } 
        
      });
      formRegButtoes.current.getItem("edit_registomovimentos").events.on("click", function(events) {
       
        if (permissaoEditarRegistoMovimento) { 
          EditarMovimento(form_construcaoedit_recId, movimentoid);
        }else{
          dhx.alert({
            header:"Permissão Necessária",
            text:"É necessário permissão para editar movimento!",
            buttonsAlignment:"center",
            buttons:["ok"],
          });
        } 
        
      });
      windowRef.current.header.events.on("click", function(id,e){
        const SaveConstrucao = (rec_id, parent_id)=> {
          try { 
          const saveConstrucaocall = async () => {      
            var tipoconstrucao_id=formRefgeral.current.getItem("tipoconstrucao_id")._value || '';
            var designacao=formRefgeral.current.getItem("designacao")._value || '';
            var observacoes=formRefgeral.current.getItem("observacoes")._value || '';
            let metodo='POST';
            if (rec_id!='0'){
              metodo='PUT';
            }
            const recIdAsInt = parseInt(rec_id, 10) || 0;   
            const parentIdAsInt = parseInt(parent_id, 10) || 0;   
            const tipoconstrucao_IdAsInt = parseInt(tipoconstrucao_id, 10) || 0;              
            const jwtToken = authtoken;
            const url = apiEndpointCadastro + 'Construcao';// + rec_id;        
            const response = await fetch(url, {
              method: metodo,
              body: JSON.stringify({ 
                  recId : recIdAsInt,
                  talhaoId: parentIdAsInt,
                  designacao: designacao,
                  tipoconstrucaoId: tipoconstrucao_IdAsInt,                    
                  observacoes: observacoes,
                }),
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${jwtToken}`
                },
            });              
            if (response.ok) {
              try{ 
                const json = await response.text(); 
                if (json=="0"){
                  dhx.alert({ 
                    header: 'Gravar Construção',
                    text: "Erro ao gravar informação da Construção.",
                    buttonsAlignment: "center",
                    buttons: ["ok"],
                  })
                } else {
                  dhx.alert({
                    header:"Gravação efectuada",
                    text:"A Gravação foi efetuada com sucesso!",
                    buttonsAlignment:"center",
                    buttons:["ok"],
                  }).then(function(){  
                    const data = JSON.parse(json);
                    dispatch({type: Actions.TREECEM_LIMPA, payload: "" }) 
                    dispatch({type: Actions.TREECEM_SELECIONA, payload: parent_id }) 
                    dispatch({type: Actions.CONSTRUCAO_EDITA, payload: data.recId});
                  });                  
                }              
              }catch (e){
                console.log(e.message); 
              } 
            } else {
              const errorMessage = await response.text();
              console.error('error: ' + errorMessage);
            } 
          };
          saveConstrucaocall();
          } catch (error) {
          console.error('error: ' + error);                 
          } 
        }
        const DeleteConstrucao = (objid,parent_id)=> {
          try { 
          const deleteConstrucaocall = async () => {                   
            const url = apiEndpointCadastro + 'Construcao';//ok  
            const jwtToken = authtoken;       
            const response = await fetch(url, {
            method: 'DELETE',    
            body: JSON.stringify({ 
              id : objid
              }),         
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`
              },
            });              
            if (response.ok) {    
              try{
                if (viewer_interface ==null){
                  viewer_interface = GetViewerInterface(); 
                } 
                viewer_interface.clearSelection();

                             
                var state = viewer.getState();
                const args = state.config;
                var NomeMapa = state.config.activeMapName;
                const uid = uuidv4();
                var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};                     
                viewer.dispatch(aux);

                menuRef.current?.destructor();
                tabRef.current?.destructor();
                formRefgeral.current?.destructor();
                windowRef.current?.destructor();
                menuRef.current = null;
                tabRef.current = null;
                formRefgeral.current = null;
                windowRef.current = null;
              }catch(e){}   
              
              dispatch({type: Actions.CONSTRUCAO_FECHA, payload: ""});  
              dispatch({type: Actions.HIDE_IMPORTAFILE_CONSTRUCAO, payload: ""});
              dispatch({type: Actions.TREECEM_LIMPA, payload: "" }) 
              dispatch({type: Actions.TREECEM_SELECIONA, payload: parent_id }) 
            } else {
              const errorMessage = await response.text();
              console.error('error: ' + errorMessage);
            }
          };
          deleteConstrucaocall();
          } catch (error) {
          console.error('error: ' + error);                 
          } 
        }
        console.log(id);
        // Aqui dispara os eventos do click nos botões do header
        switch(id){
          case 'gravar':
            var rec_id=formRefgeral.current.getItem("rec_id").getValue();
            var parent_id=formRefgeral.current.getItem("parent_id").getValue();
            let permissaoEscritaaux = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_AcessoEscritaArvoreConstrucoes, 'G:'+parent_id);
            if (permissaoNovoItem || permissaoEscritaaux) { 
              SaveConstrucao(rec_id, parent_id); 
            }else{
              dhx.alert({
                header:"Permissão Necessária",
                text:"A Gravação não foi efetuada é necessário permissão!",
                buttonsAlignment:"center",
                buttons:["ok"],
              });
            }            
          break;
          case 'eliminar':
            var objid=formRefgeral.current.getItem("rec_id").getValue();
            var parent_id=formRefgeral.current.getItem("parent_id").getValue(); 
            // vamos criar uma janela de confirmação do delete
            if (permissaoNovoItem) { 
              dhx.confirm({
                header:"Remover Construção",
                text:"Tem a certeza que pretende remover a construção?",
                buttons:["sim", "não"],
                buttonsAlignment:"center"
              }).then(function(resposta){
                console.log('resposta ', resposta);            
                switch(resposta){
                case false:
                  DeleteConstrucao(objid, parent_id); 
                  break;
                default:
                  break;
                }
              });
            }else{
              dhx.alert({
                header:"Permissão Necessária",
                text:"A Eliminação não foi efetuada é necessário permissão!",
                buttonsAlignment:"center",
                buttons:["ok"],
              });
            }           
            break;
          case 'imprimir':
             dhx.alert({ 
                header: 'Funcionalidade em implementação',
                text: "Esta funcionalidade ainda não está disponivel.",
                buttonsAlignment: "center",
                buttons: ["ok"],
              })

            break;
          case 'localizar':
            console.log('localizar');  
            var centroid=formRefgeral.current.getItem("construcao_centroid").getValue();
            var mbr=formRefgeral.current.getItem("construcao_bbox").getValue();  
 
            onzoomToWindow(centroid, mbr, 1);
            break;
          case 'minimizar':
            let size = windowRef.current.getSize(); 
            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral"); 
              tabRef.current.disableTab("observacoes");
              tabRef.current.disableTab("geograficos");
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral"); 
              tabRef.current.enableTab("observacoes");
              tabRef.current.enableTab("geograficos");
            } 
            break;
          case 'close':
            closeWindow();
            break;
          default:
            break;
        }
      });
      menuRefgeometrias.current.events.on("click", function(id,e){
       
     
        const PostWKT = (wktString)=> {
          try {				
            const saveConstrucaoObj = async (wktString) => {                   
              const url = apiEndpointCadastro + 'Construcao/' +form_construcaoedit_recId +'/geometria/' ;   
              const jwtToken = authtoken;      
              const response = await fetch(url, {
                method: 'PUT',
                body: JSON.stringify({  
                    wkt: wktString 
                  }),
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${jwtToken}`
                  },
              });              
              if (response.ok) {
                  // mapa refresh
                  // tree refresh      
                  refreshDataGeometrias();               
                  var state = viewer.getState();
                  const args = state.config;
                  var NomeMapa = state.config.activeMapName;
                  const uid = uuidv4();
                  var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};                     
                  viewer.dispatch(aux);   
                  dhx.alert({ 
                    header: 'Novos objectos',
                    text: "Criados novos objectos.",
                    buttonsAlignment: "center",
                    buttons: ["ok"],
                  })
              } else {
                const errorMessage = await response.text();
                console.error('error: ' + errorMessage);
              }
            };
            saveConstrucaoObj(wktString);
          } catch (error) {
            console.error('error: ' + error);                 
          } 
        }
        const OnSeleccao = ()=> {
          try {				
            const saveGeometriaSeleccao = async () => {                   
              const url = apiEndpointCadastro + 'MapaLayerDesenhoSeleccaoWKT/';//ok
              const jwtToken = authtoken;     
              const aux_usersession = usersession;  
              var estado = viewer.getState();			
              var activeMapName = estado.config.activeMapName;
              var mapState = estado.mapState[activeMapName];
              var currentMap = mapState.mapguide.runtimeMap; 
              var sessionId  = currentMap.SessionId; 
              var mapaDef  = currentMap.MapDefinition;
              console.log(userid);
              console.log(aux_usersession); 
              console.log(activeMapName);
              console.log(mapaDef);
              console.log(sessionId); 
    
            
              const response = await fetch(url, {
                method: 'POST',
                body: JSON.stringify({ 
                  mapa: activeMapName,
                  mapadef: mapaDef,
                  sessionid: sessionId, 
                  userid: userid,
                  usersession : aux_usersession,   
                  viewer: 'false', 
                }),
                headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${jwtToken}`,
                },
              });              
              if (response.ok) {
  
                const wkt = await response.text();  
                if (wkt!=''){
                  PostWKT(wkt); 
                } else {
                  dhx.alert({ 
                    header: 'Importação de objecto seleccionado',
                    text: "Tem que selecionar um objecto no mapa.",
                    buttonsAlignment: "center",
                    buttons: ["ok"],
                  })
                }
                 
              } else {
                const errorMessage = await response.text();
                console.error('error: ' + errorMessage);
              }
            };
            saveGeometriaSeleccao();
          } catch (error) {
            console.error('error: ' + error);                 
          } 
        };
        const OnTerminouDesenhoPG = (objetodesenhado)=> {
          let coordenadas = objetodesenhado.flatCoordinates;
          const poligono = [];
          for (let i = 0; i < coordenadas.length; i++) {
            const coordinate1 = coordenadas[i];
            const coordinate2 = coordenadas[i+1];
            const point = [];
            point.push(coordinate1);
            point.push(coordinate2);
            i++;
            poligono.push(point);
          }           
          // Create a  geometry
          const geometry = new Polygon([poligono]);
          // Create an instance of the WKT format
          const format = new WKT();   
          // Convert the geometry to WKT
          const wktString = format.writeGeometry(geometry, {
            dataProjection: 'EPSG:3857',
            featureProjection: 'EPSG:3857',
          }); 
          console.log(wktString); 
          //POST do WKT    
          PostWKT(wktString);
          
          let size = windowRef.current.getSize(); 
            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 
        };
        const OnTerminouDesenhoCircle = (objetodesenhado)=> {
              let coordenadas = objetodesenhado.flatCoordinates;
              const centro = [coordenadas[0], coordenadas[1]];
              const exterior = [coordenadas[2], coordenadas[3]];
              // Calcula o raio do círculo
              const raio = Math.sqrt(
                Math.pow(exterior[0] - centro[0], 2) + Math.pow(exterior[1] - centro[1], 2)
              );

              // Defina um número mínimo de pontos para representar o círculo
              const numPontosMinimo = 20;

              // Calcule o número de pontos com base no raio
              const numPontos = Math.max(numPontosMinimo, Math.ceil(raio / 10));

              // Crie os pontos do círculo
              const pontosCirculo = [];
              for (let i = 0; i < numPontos; i++) {
                const theta = (2 * Math.PI * i) / numPontos;
                const x = centro[0] + raio * Math.cos(theta);
                const y = centro[1] + raio * Math.sin(theta);
                pontosCirculo.push([x, y]);
              }
              const theta = (2 * Math.PI * 0) / numPontos;
              const x = centro[0] + raio * Math.cos(theta);
              const y = centro[1] + raio * Math.sin(theta);
              pontosCirculo.push([x, y]);
              // Crie o polígono representando o círculo
              const geometry = new Polygon([pontosCirculo]); 
               
              // Create an instance of the WKT format
              const format = new WKT();   
              // Convert the geometry to WKT
              const wktString = format.writeGeometry(geometry, {
                dataProjection: 'EPSG:3857',
                featureProjection: 'EPSG:3857',
              }); 
              console.log(wktString); 
              //POST do WKT    
              PostWKT(wktString);
              
              let size = windowRef.current.getSize(); 
              if (size.height!=120){
                windowRef.current.setSize(800, 120); 
                tabRef.current.disableTab("geral");
                tabRef.current.disableTab("geograficos");
                tabRef.current.disableTab("ficheiros");
                tabRef.current.disableTab("movimentos"); 
              } else {
                windowRef.current.setSize(800, 580);
                tabRef.current.enableTab("geral");
                tabRef.current.enableTab("geograficos");
                tabRef.current.enableTab("ficheiros");
                tabRef.current.enableTab("movimentos");
              } 
        };
        const OnTerminouDesenhoPL = (objetodesenhado)=> { 
          let coordenadas = objetodesenhado.flatCoordinates;
          // Define an array of LinearRings
          const polilinha = [];
          for (let i = 0; i < coordenadas.length; i++) {
            const coordinate1 = coordenadas[i];
            const coordinate2 = coordenadas[i+1];
            // Perform some operation on 'coordinate' here
            const point = [coordinate1, coordinate2]; 
            i++;
            polilinha.push(point);
          }
          // Create a geometry
          const geometry = new LineString(polilinha);

          // Create an instance of the WKT format
          const format = new WKT();

          // Convert the geometry to WKT
          const wktString = format.writeGeometry(geometry, {
            dataProjection: 'EPSG:3857',
            featureProjection: 'EPSG:3857',
          });
   
          console.log(wktString); 

          //POST do WKT    
          PostWKT(wktString); 
          let size = windowRef.current.getSize(); 
            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 
        };
        const OnTerminouDesenhoPT = (objetodesenhado)=> { 
          let coordenadas = objetodesenhado.flatCoordinates;
          const point = [coordenadas[0], coordenadas[1]]; 
          // Create a geometry
          const geometry = new Point(point);
          // Create an instance of the WKT format
          const format = new WKT();
          // Convert the geometry to WKT
          const wktString = format.writeGeometry(geometry, {
            dataProjection: 'EPSG:3857',
            featureProjection: 'EPSG:3857',
          });
          console.log(wktString); 

          //POST do WKT    
          PostWKT(wktString);
          let size = windowRef.current.getSize(); 
            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 

        };
        // Aqui dispara os eventos do click no menu do separador das geometrias     
        if (viewer_interface ==null){
          viewer_interface = GetViewerInterface(); 
        } 
        let size = windowRef.current.getSize(); 
        switch(id){
          case 'pg': 
            viewer_interface.digitizePolygon(OnTerminouDesenhoPG);
            factory = viewer_interface.getOLFactory();
            console.log('poligono');
            
            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 
            break;
          case 'pl':
            viewer_interface.digitizeLineString(OnTerminouDesenhoPL);
            factory = viewer_interface.getOLFactory();
            console.log('linha');

            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 
            break;
          case 'pt':
            console.log('ponto');
            viewer_interface.digitizePoint(OnTerminouDesenhoPT);
            factory = viewer_interface.getOLFactory();

            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 
            break;
          case 'rt':
            console.log('retangulo');
            viewer_interface.digitizeRectangle(OnTerminouDesenhoPG);
            factory = viewer_interface.getOLFactory();

            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 
            break;
          case 'ci':
            console.log('circulo');
            viewer_interface.digitizeCircle(OnTerminouDesenhoCircle);
            factory = viewer_interface.getOLFactory();

            if (size.height!=120){
              windowRef.current.setSize(800, 120); 
              tabRef.current.disableTab("geral");
              tabRef.current.disableTab("geograficos");
              tabRef.current.disableTab("ficheiros");
              tabRef.current.disableTab("movimentos"); 
            } else {
              windowRef.current.setSize(800, 580);
              tabRef.current.enableTab("geral");
              tabRef.current.enableTab("geograficos");
              tabRef.current.enableTab("ficheiros");
              tabRef.current.enableTab("movimentos");
            } 
            break;
         
          case 'importarficheiro': 
            console.log('importarficheiro');
            var aux={ type: Actions.SHOW_IMPORTAFILE_CONSTRUCAO, payload: form_construcaoedit_recId };
            viewer.dispatch(aux); 
            break; 
          /*case 'seleccionar':
            console.log('seleccionar'); 
            OnSeleccao();
            break;*/
          
          default:
            break;
        }
      });
      // Foi premido o botão para criar novo/editar movimento
      const EditarMovimento = (form_construcaoedit_recId, registomovimento_id) => {
            
        const icon_save = <Icon iconName="Save" />;
        const icon_delete = <Icon iconName="Delete" />;
        const icon_saveHtml = ReactDOMServer.renderToStaticMarkup(icon_save);
        const icon_deleteHtml = ReactDOMServer.renderToStaticMarkup(icon_delete);
              
        windowRegistoMovimento.current = new WindowDHX({
          width: 800,
          height: 600,
          title: 'Registo Movimento', 
          closable: true,
          movable: true,
          resizable: false,
          modal: true,
          header: true,
          footer: true,
          css: "pcc_window",
        });
        windowRegistoMovimento.current.show();
        windowRegistoMovimento.current.header.data.add({
          type: "button",
          view: "link",
          size: "medium",
          color: "primary",   
          value: "Gravar",
          tooltip: "Gravar",
          title: "Gravar",
          id: "gravar",
          css: "pcc_button_save dhx_button--circle", 
          html: icon_saveHtml
        }, 2);
        windowRegistoMovimento.current.header.data.add({
          type: "button",
          view: "link",
          size: "medium",
          color: "primary", 
          disabled: false,
          tooltip: "Minimizar",
          value: "Minimizar",
          title: "Minimizar",
          id: "minimizar",
          css: "pcc_button_minimizar dhx_button--circle",
          html: icon_minusHtml
        }, 3);
        windowRegistoMovimento.current.header.events.on("click", function(id,e){
          console.log(id);
          // Aqui dispara os eventos do click nos botões do header
          switch(id){
            case 'gravar':
                console.log(userid);
                handleSave(userid);
                break;
            case 'minimizar':
              let size = windowRegistoMovimento.current.getSize(); 
              if (size.height!=120){
                windowRegistoMovimento.current.setSize(860, 120); 
                tabRegistoMovimento.current.disableTab("reggeral"); 
                tabRegistoMovimento.current.disableTab("atveco");  
              } else {
                windowRegistoMovimento.current.setSize(860, 831); 
                tabRegistoMovimento.current.enableTab("reggeral"); 
                tabRegistoMovimento.current.enableTab("atveco");  
              } 

              break;
            default:
              break;
            }
          }
        );

        if (permissaoEditarRegistoMovimento || permissaoNovoRegistoMovimento){
          windowRegistoMovimento.current.header.enable('gravar');
        } else {
          windowRegistoMovimento.current.header.disable('gravar');
        }
              

        tabRegistoMovimento.current = new TabbarDHX(null,{
          css: "pcc_tab",
          tabs:[ 
            { id: "reggeral", tab: "tabreg" }, 
            { id: "regficheiros", tab: "tabfx" }, 
        ]});

        tabRegistoMovimento.current.addTab({ id: "reggeral", tab: "Geral"}, 1); 
        tabRegistoMovimento.current.addTab({ id: "regficheiros", tab: "Ficheiros"}, 2); 

        let datalistatipomovimento;
        let datacombo2; 
        formRegistoMovimentoDados.current = new FormDHX(null,{ 
          css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
          padding: 17,
          rows: [ 
            { type: "spacer", name: "spacer" ,  height:"25px"}, 
            { css: "pcc_input",type: "input", name: "registomovimento_recid", label: "rec_id",  value: '', hidden: true   }, 
            { css: "pcc_input",type: "input", name: "registoconstrucao_recid", label: "rec_id",  value: form_construcaoedit_recId, hidden: true   }, 
            { css: "pcc_input",type: "input", name: "registotipomovimento_recid", label: "rec_id",  value: '', hidden: true   },  
            { css: "pcc_input",type: "input", name: "registoresidente_id", label: "id",  value: '', hidden: true   },  
            { css: "pcc_input",type: "datepicker", name: "data_movimento",  dateFormat: "%d/%m/%Y", label: "Data movimento:", labelPosition: "left", placeholder: "", width: 250, labelWidth: 125 },  
            {
            css: "pcc_input",type: "combo", name: "tipomovimento", labelPosition: "left", label: "Tipo de Movimento", readOnly:true, width: 300,
            data: datalistatipomovimento,
            }, 
            { type: "spacer", name: "spacer" ,  height:"25px"}, 
            { css: "pcc_input",type: "input", name: "nome", label: "Nome", labelPosition: "left", placeholder: "Nome" }, 
            { type: "spacer", name: "spacer" ,  height:"10px"}, 
            { css: "pcc_input",type: "datepicker", name: "data_nascimento", dateFormat: "%d/%m/%Y", label: "Data nascimento:", labelPosition: "left", placeholder: "", width: 250, labelWidth: 125}, 
            { css: "pcc_input",type: "datepicker", name: "data_falecimento", dateFormat: "%d/%m/%Y", label: "Data falecimento:", labelPosition: "left", placeholder: "", width: 250, labelWidth: 125}, 
            { type: "spacer", name: "spacer" ,  height:"10px"}, 
            { css: "pcc_input",type: "datepicker", name: "data_inumacao", dateFormat: "%d/%m/%Y", label: "Data inumação:", labelPosition: "left", placeholder: "", width: 250, labelWidth: 125 }, 
            {
            type: "button",
            name: "associarconfrontacao",
            text: "Associar Confrontação",
            size: "medium",
            view: "flat",
            color: "primary", hidden: true 
          }   
          ]  
        }); 
        layoutFicheirosAssociadosMovimento.current = new LayoutDHX(null,{
          type: "line",
          rows: 
          [   
              {
                id: "dadosent1",
                html: "dadosent1",
                header:"Lista dos ficheiros:",  
                progressDefault: true,
                padding: 10,
                height: 175,
              },
              {
                id: "dadosent2",
                html: "dadosent2",
                header:"Dados do ficheiro associado:",  
                progressDefault: true,
                padding: 10,
              } 
          ]
        });
        let datalistficheirosmovimento= []; 
       
        ListaFicheirosAssociadosMovimento.current= new ListDHX(null,{
          css: "dhx_widget--bg_white dhx_widget--bordered formlist", 
          data: datalistficheirosmovimento, 
        });
      
        FormFicheirosAssociadosMovimento.current = new FormDHX(null, { 
          css: "dhx_widget--bg_white dhx_widget--bordered pcc_form",
          padding: 10,
          
          rows: [
            
            { 
              css: "pcc_input",
              type: "input", 
              name: "descricao", 
              label: "Descrição", 
              placeholder: "Nome", 
              disabled: true,
              labelPosition: "left"  
            },
            {
              cols: [   
                { 
                  css: "pcc_input",
                  type: "datepicker", 
                  name: "dataarquivo",  
                  label: "Data",  
                  dateFormat: "20%y/%m/%d",
                  disabled: true ,
                  labelPosition: "left"            
                },        
                {
                  css: "pcc_input",
                  type: "input", 
                  name: "arquivdir", 
                  label: "Ficheiro", 
                  placeholder: "Ficheiro", 
                  hidden: true,
                  labelPosition: "left" 
                },
                { 
                  css: "pcc_input_ir", 
                  id: "download_ficheiro", 
                  type: "button", 
                  name: "download_ficheiro", 
                  value: "Descarregar",
                  tooltip: "Descarregar",
                  title: "Descarregar",
                  hidden: true, 
                  circle: true,
                  size: "small", 
                  view: "flat", 
                  color: "primary",
                }],
            },  
            { 
              css: "pcc_input",
              type: "input", 
              name: "observficheiro", 
              label: "Observações:", 
              placeholder: "Observações",
              disabled: true,
              labelPosition: "left"  
            },
            {
              cols: [           
                { 
                  css: "pcc_input_addficheiro", 
                  id: "buttonAdicionarFicheiro", 
                  type: "button", 
                  name:"upload_ficheiro", 
                  value: "Adicionar", 
                  tooltip: "Adicionar", 
                  title: "Adicionar", 
                  circle: true,
                  size: "small", 
                  view: "flat", 
                  color: "primary"
                }, 
              ], 
            },
            {
              cols: [           
                { 
                  css: "pcc_input_addficheiro", 
                  id: "buttonGravarFicheiro", 
                  type: "button", 
                  name:"save_ficheiro", 
                  value: "Adicionar", 
                  tooltip: "Adicionar", 
                  title: "Adicionar", 
                  circle: true,
                  size: "small", 
                  view: "flat", 
                  color: "primary",
                  hidden: true,
                }
              ], 
            }  
          ]
        });
         
        FormFicheirosAssociadosMovimento.current.getItem("download_ficheiro").events.on("click", function(value) {
         
          const getitemficheiro = async () => {
            const jwtToken = authtoken;
            const url3 = apiEndpointDocumentos  + 'FicheiroAssociado/' + reciditemficheiro;
            const response = await  fetch(url3, {
              headers: {
                'Authorization': `Bearer ${jwtToken}`
              }
            });
      

            if (response.ok) {
        
              // Read the response as a blob
              const fileBlob = await response.blob();
              // Obtém o tipo de conteúdo do cabeçalho da resposta
              const contentType = response.headers.get('Content-Type');

              // Cria um URL para o blob, usando o tipo de conteúdo original
              const fileUrl = URL.createObjectURL(fileBlob, { type: contentType });
  
              const downloadLink = document.createElement('a');
              downloadLink.href = fileUrl;
              downloadLink.download = itemficheironame;
              downloadLink.click();
              // Limpa o URL do objeto para liberar memória
              URL.revokeObjectURL(fileUrl);

              dhx.alert({
                header: "Download",
                text: "Foi efectuado o download do ficheiro.",
                buttonsAlignment: "center",
                buttons: ["ok"]
              });
            };
          };
          getitemficheiro();
           
        });
    
        FormFicheirosAssociadosMovimento.current.getItem("upload_ficheiro").events.on("click", function(value) {
     
          var aux={ type: Actions.SHOW_ASSOCIAFILE, payload: { registo_id: form_construcaoedit_recId , tipoassociacao: 'movimento' }};
       
          viewer.dispatch(aux); 

        });
    
        FormFicheirosAssociadosMovimento.current.getItem("save_ficheiro").events.on("click", function(value) {
          const formValues = FormFicheirosAssociados.getValue(); 
          const obj_associaficheiro_id = form_construcaoedit_recId;
          const descricao = formValues.descricao; 
          const dataarquivo = formValues.dataarquivo;
          const observficheiro = formValues.observficheiro; 
          // Agora você tem os valores dos campos
          console.log("Descrição:", descricao); 
          console.log("Data:", dataarquivo);
          console.log("Observações:", observficheiro);
        
          if(descricao ==""){
            dhx.alert({
              header: "Falta Descrição",
              text: "O item Descrição é necessario!",
              buttonsAlignment: "center",
              buttons: ["ok"]
            });
          }else{
            //enviarDadosParaServidor(descricao, dataarquivo, observficheiro, obj_associaficheiro_id);
          }
        }); 
        layoutFicheirosAssociadosMovimento.current.getCell("dadosent1").attach(ListaFicheirosAssociadosMovimento.current);
        layoutFicheirosAssociadosMovimento.current.getCell("dadosent2").attach(FormFicheirosAssociadosMovimento.current); 

        tabRegistoMovimento.current.getCell("reggeral").attach(formRegistoMovimentoDados.current);
        tabRegistoMovimento.current.getCell("regficheiros").attach(layoutFicheirosAssociadosMovimento.current);
        windowRegistoMovimento.current?.attach(tabRegistoMovimento.current);
        //tabRegistoMovimento.current.disableTab("regficheiros");
      
      
        

        function handleSave(userid) { 
          let objregistoconstrucao_recid= formRegistoMovimentoDados.current.getItem("registoconstrucao_recid")._value; 
          let tipomovimento = formRegistoMovimentoDados.current.getItem("tipomovimento").getValue(); 
          let objregistodata_movimento = formRegistoMovimentoDados.current.getItem("registodata_movimento").getValue(); 
          let objnome = formRegistoMovimentoDados.current.getItem("nome")._value;
          let objdata_nascimento = formRegistoMovimentoDados.current.getItem("data_nascimento").getValue();
          let objdata_falecimento= formRegistoMovimentoDados.current.getItem("data_falecimento").getValue();
          let objdata_inumacao = formRegistoMovimentoDados.current.getItem("data_inumacao").getValue();


          let objregistomovimento_recid= formRegistoMovimentoDados.current.getItem("registomovimento_recid").getValue();

          console.log(objregistoconstrucao_recid); 
          console.log(tipomovimento);
          console.log(objregistodata_movimento);
          console.log(objnome);
          console.log(objdata_nascimento);
          console.log(objdata_falecimento); 
          console.log(objdata_inumacao);
          console.log(objregistomovimento_recid);
          objregistodata_movimento= convertDateEdicao2Storage(objregistodata_movimento); 
          objdata_nascimento= convertDateEdicao2Storage(objdata_nascimento); 
          objdata_falecimento= convertDateEdicao2Storage(objdata_falecimento); 
          objdata_inumacao= convertDateEdicao2Storage(objdata_inumacao); 

          if (objregistomovimento_recid==""){
            insereMovimento(objregistoconstrucao_recid, tipomovimento, objregistodata_movimento, objnome, objdata_nascimento, objdata_falecimento, objdata_inumacao); 					
          }
        }
      
        const insereMovimento = (registoconstrucao_recid, tipomovimento_id, registodata_movimento, objnome, objdata_nascimento, objdata_falecimento, objdata_inumacao) => {   
          const SaveMovimento = async(registoconstrucao_recid, tipomovimento_id, registodata_movimento, objnome, objdata_nascimento, objdata_falecimento, objdata_inumacao) => {   
            const url = apiEndpointCadastro + 'Movimento/';        
            const response = await fetch(url, {
              method: 'POST',
              body: JSON.stringify({ 
                  construcao_id: registoconstrucao_recid.toString(),
                  tipomovimento_id: tipomovimento_id.toString(),
                  data_movimento: registodata_movimento.toString(),
                  nome: objnome.toString(),
                  data_nascimento: objdata_nascimento.toString(),
                  data_falecimento: objdata_falecimento.toString(),
                  data_inumacao: objdata_inumacao.toString(), 
              }),
              headers: {
                  'Content-Type': 'application/json',
              },
            }); 	             
            if (response.ok) {
              const responseText = await response.text();
              const [status, id] = responseText.split('|');
              
              if (status === "OK") {
                  console.log(`Gravação efetuada com sucesso! ID: ${id}`);
                  console.log(id);
                  // Atualizações na interface do usuário
                  //windowRegistoMovimento.current.hide();      
                  //fetchDatalistRegistos();          
                  saveitemRegistoAtividades(id);
              } else {
                  console.error('Erro ao salvar o registro.');
              }
            } else {
              const errorMessage = await response.text();
              console.error('Error: ' + errorMessage);
            } 
          };
          SaveMovimento(registoconstrucao_recid, tipomovimento_id, registodata_movimento, objnome, objdata_nascimento, objdata_falecimento, objdata_inumacao);
        };
        const fetchListaTipoMovimento = async () => {
          const url2 = apiEndpointCadastro + 'TipoMovimento';
          const jwtToken = authtoken; 
          const response = await fetch(url2, {
            method: 'GET', 
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`
            },
          });              
          const json = await response.json();
          const transformResponse = (originalResponses) => { 
              const transformedData = [];  
              let counter2 = 1;
              if (Array.isArray(originalResponses)){
                originalResponses.forEach((originalResponse) => {
                  const { recId, designacao } = originalResponse;
                  let transformedDescricao = designacao; 
                  const transformedRecord = {
                    id: recId,  
                    value: transformedDescricao 
                  };
                  transformedData.push(transformedRecord);
                  counter2++;
                });
              } else {
                  const { recId, designacao  } = originalResponses;
                  let transformedDescricao = designacao;
                  const transformedRecord = {
                    id: recId,   
                    value: transformedDescricao 
                  };
                  transformedData.push(transformedRecord); 
              }    
              return transformedData;
            }
           const transformedResult = transformResponse(json); 
          setDadosTipoMovimento(transformedResult); 
         
          datalistatipomovimento = json; 
          var myComboBox= formRegistoMovimentoDados.current.getItem('tipomovimento').getWidget();
          myComboBox.data.parse(transformedResult);
          myComboBox.setValue('-1');  
          
        }
        fetchListaTipoMovimento();

            
        const fetchDataRegisto = async () => {

          const url3 = apiEndpointCadastro + 'Movimento/' + registomovimento_id;
            
          const jwtToken = authtoken; 
          const response = await fetch(url3, {
            method: 'GET', 
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${jwtToken}`
            },
          });   
          const json = await response.json();
          console.log('resposta Registos : ' + JSON.stringify(json));
            
          formRegistoMovimentoDados.current.setValue({ "registomovimento_recid": json.recId });
          formRegistoMovimentoDados.current.setValue({ "registoconstrucao_recid": json.construcaodestinoId }); 
          formRegistoMovimentoDados.current.setValue({ "registotipomovimento_recid":  json.tipomovimentoId});
          formRegistoMovimentoDados.current.setValue({ "registoresidente_id": json.residenteId });
          
          formRegistoMovimentoDados.current.setValue({ "data_movimento":convertDateStorage2Edicao(json.dataMovimento)});
          formRegistoMovimentoDados.current.setValue({ "tipomovimento": json.tipomovimentoNome });
          formRegistoMovimentoDados.current.setValue({ "nome": json.residenteNome }); 
          formRegistoMovimentoDados.current.setValue({ "data_nascimento":  convertDateStorage2Edicao(json.residente_Datanascimento)});
          formRegistoMovimentoDados.current.setValue({ "data_falecimento": convertDateStorage2Edicao(json.residente_Datafalecimento) });
          formRegistoMovimentoDados.current.setValue({ "data_inumacao": convertDateStorage2Edicao(json.residente_Datainumacao) });
 
          var myComboBox= formRegistoMovimentoDados.current.getItem('tipomovimento').getWidget();
           
          const valor =  parseInt(json?.tipomovimentoId , 10) || 0;
          if (valor!=''){
            myComboBox.setValue(valor);
            myComboBox.disable();
          }
          else {
            myComboBox.setValue('-1');
          }
          

        }	
        if(registomovimento_id !=''){
           console.log('get registo movimento '+ registomovimento_id); 
           fetchDataRegisto(registomovimento_id);
        }
      }
       
      let obj_nome="";
      let clickTimeout = null;
      treeRefgeometrias.current.events.on("itemClick", id => {
        if (clickTimeout) {
            clearTimeout(clickTimeout); // Cancela o timeout anterior
        }

        clickTimeout = setTimeout(() => {
            // Código existente para selecionar o objeto
            formstatusobjecto.current.setValue({ "statusobjecto": '' });
            console.log(id);

            if (id !== 'objectos') {
                const obj_nome = treeRefgeometrias.current.data.getItem(id)?.value;
                
                objectoid = id;
                setObjecto_id(id);

                const centroid = treeRefgeometrias.current.data.getItem(id)?.centroid;
                const mbr = treeRefgeometrias.current.data.getItem(id)?.mbr;
                const area = treeRefgeometrias.current.data.getItem(id)?.area;
                const perimetro = treeRefgeometrias.current.data.getItem(id)?.perimetro;
                const geometria = treeRefgeometrias.current.data.getItem(id)?.geometria;
                // 1. Converte a área para número. Se o valor for inválido ou não existir, o resultado será NaN.
                const areaNumerica = parseFloat(area);
                // 2. Formata o número apenas se a conversão foi bem-sucedida. Caso contrário, define um valor por defeito.
                const areaFormatada = !isNaN(areaNumerica) ? areaNumerica.toFixed(2) : 'N/D';

                // Repete o mesmo processo para o perímetro
                const perimetroNumerico = parseFloat(perimetro);
                const perimetroFormatado = !isNaN(perimetroNumerico) ? perimetroNumerico.toFixed(2) : 'N/D'; 

                const texto = 'Área: ' + areaFormatada + 'm2\nPerímetro: ' + perimetroFormatado + ' m\n\n' +
                              'WKT: ' + geometria;
                formstatusobjecto.current.setValue({ "statusobjecto": texto });

                if (viewer_interface == null) {
                    viewer_interface = GetViewerInterface();
                }

                const bounds = viewer_interface.getCurrentExtent();
                const view = viewer_interface.getViewForExtent(bounds);
                const escala = parseInt(view.scale);
                const centro_x = view.x;
                const centro_y = view.y;
                const estado = viewer.getState();
                const activeMapName = estado.config.activeMapName;
                const mapState = estado.mapState[activeMapName];
                const currentMap = mapState.mapguide.runtimeMap;
                const sessionId = currentMap.SessionId;
                const mapaDef = currentMap.MapDefinition;
                const aux_construcaoid = form_construcaoedit_recId;
                const ObjectoGeografico_id = id;

                if (ObjectoGeografico_id !== undefined) {
                    setConstrucoesSelection(apiEndpointSIG, authtoken, activeMapName, mapaDef, sessionId, aux_construcaoid, centro_x, centro_y, escala);
                }
            }  
            clickTimeout = null; // Reseta o timeout
        }, 300); // 300ms é o tempo de espera para ignorar cliques subsequentes
      });
      var isChecks=[];

      treeRefgeometrias.current.events.on("afterEditEnd", function(value, id) {
        // atualizar o nome do objeto
        try {				
          const saveConstrucaoObj = async (id, value) => {  
            
            let ObjectoGeografico_tema_id=formRefgeral.current.getItem("objectogeografico_tema_id")._value;                 
            const url = apiEndpointCadastro + 'ConstrucaoObjGeom';  
            const jwtToken = authtoken;        
            const response = await fetch(url, {
              method: 'PUT',
              body: JSON.stringify({ 
                  id : form_construcaoedit_recId,
                  construcaoobj_id: id ,
                  construcaoobj_nome: value,
                  construcaoobj_tipoid: "0"
                }),
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${jwtToken}`
                },
            });              
            if (response.ok) {
                // mapa refresh
                // tree refresh      
                refreshDataGeometrias();               
                var state = viewer.getState();
                const args = state.config;
                var NomeMapa = state.config.activeMapName;
                const uid = uuidv4();
                var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};                     
                viewer.dispatch(aux);   
            } else {
              const errorMessage = await response.text();
              console.error('error: ' + errorMessage);
            }
          };
          saveConstrucaoObj(id, value);
        } catch (error) {
          console.error('error: ' + error);                 
        } 
      });
 
      windowRef.current?.attach(tabRef.current);
 
      windowRef.current?.events.on("move", function(position, oldPosition, side) {
        console.log("The window is moved to " + position.left, position.top)
      });
      windowRef.current?.events.on("afterHide", function(position, events){
         
          formRefgeral.current && formRefgeral.current.destructor();
          windowRef.current && windowRef.current.destructor();
          formRefgeral.current = null;
          windowRef.current = null;
      }); 
  
      let objgeomid=''; 
      let objgeomnome='';
      let centroid=''; 
      let mbr=''; 
      
      treeRefgeometrias.current.events.on("itemRightClick", function(id, e){
        console.log("The item with the id "+ id +" was right-clicked.");
        objgeomid=id;
        //ObjectoGeografico_id=id;
        objgeomnome  = treeRefgeometrias.current.data.getItem(id)?.value;
        centroid = treeRefgeometrias.current.data.getItem(id)?.centroid;
        mbr = treeRefgeometrias.current.data.getItem(id)?.mbr;
       

        formRefgeral.current.setValue({ "objectogeografico_recid": objgeomid });
        formRefgeral.current.setValue({ "objectogeografico_nome": objgeomnome }); 
        formRefgeral.current.setValue({ "objectogeografico_mbr":  mbr});
        formRefgeral.current.setValue({ "objectogeografico_centroid": centroid });

        let parent_id=formRefgeral.current.getItem("parent_id").getValue();  
        let permissaoEscritaaux = Perm.findPermissionByCodAndSubCod(funcionalidadesRelevantes, Perm.FGU_AcessoEscritaArvoreConstrucoes, 'G:'+parent_id);
        let permissaoEditarItemaux = permissaoEditarItem && permissaoEscritaaux;
 
        if (id=='objectos'){
          menutreeRefgeometrias.current.disable(["localizar"]);
          menutreeRefgeometrias.current.disable(["exportar"]);
          menutreeRefgeometrias.current.disable(["remover"]);
 
        } else {
          menutreeRefgeometrias.current.enable(["localizar"]);
          if (permissaoEditarItemaux){
            menutreeRefgeometrias.current.enable(["exportar"]);
            menutreeRefgeometrias.current.enable(["remover"]);
          } else {
            menutreeRefgeometrias.current.disable(["exportar"]);
            menutreeRefgeometrias.current.disable(["remover"]);
          }  
        }  
             
        e.preventDefault();
        if(permissaoNovoItem){
          menutreeRefgeometrias.current.showAt(e);
        }
      });
      function formatDate(dateString) {
        // Supondo que dateString esteja no formato AAAA/MM/DD
        
        const parts = dateString.split('/');
        const parts1 = dateString.split('-');
        if (parts1.length === 3) {
            const year = parts1[0];
            const month = parts1[1];
            const day = parts1[2];
            return `${year}/${month}/${day}`; // Formato AAAA-MM-DD
        }
        
        if (parts.length === 3) {
            const year = parts[0];
            const month = parts[1];
            const day = parts[2];
            return `${year}/${month}/${day}`; // Formato AAAA-MM-DD
        }
        return null; // Retorna null se a data não estiver no formato esperado
      }
      let reciditemficheiro;
      let itemficheironame;
      let abriroucopiar;
      ListaFicheirosAssociados.current.events.on("click", function(id,event) {
        var item = ListaFicheirosAssociados.current.data.getItem(id);
        var itemdesc = item.descricaoDocumento;
        var itemdatagrav = item.datahoraupload;
        var observ = item.observacaoDocumento;
          
        reciditemficheiro = item.recId;
        itemficheironame = item.nomeDocumento; 
        
        let formattedDate = formatDate(itemdatagrav);
  
          
        FormFicheirosAssociados.getItem("arquivdir").show(); // Mostrar o campo Arquivo/Dir
        //FormFicheirosAssociados.getItem("selecionar_arquivo").hide(); // Mostrar o botão Selecionar
        function isHttpLink(url) {
          return url.startsWith("http://") || url.startsWith("https://");
        }
              
        // Preencher os campos
        FormFicheirosAssociados.setValue({
          "descricao": itemdesc,
          "dataarquivo": formattedDate,
          "observficheiro": observ,
          "arquivdir": itemficheironame // Adicionado para preencher o campo Arquivo/Dir
        });

        FormFicheirosAssociados.getItem("download_ficheiro").show();
        FormFicheirosAssociados.getItem("save_ficheiro").hide();

        FormFicheirosAssociados.getItem("descricao").disable();
        FormFicheirosAssociados.getItem("dataarquivo").disable();
        FormFicheirosAssociados.getItem("observficheiro").disable();
        FormFicheirosAssociados.getItem("arquivdir").disable();
          
      });
      ListaFicheirosAssociados.current.events.on("itemRightClick", function(id,e) {
          var item = ListaFicheirosAssociados.current.data.getItem(id);
          reciditemficheiro = item.recid;

          e.preventDefault();
          
          menutreeRefFicheiros.current.showAt(e);
      
      });

      // Se for uma nova construcao
      // Coloca disable Tabs
      if (nova_construcao){ 
        tabRef.current.disableTab("movimentos");
        tabRef.current.disableTab("geograficos"); 
        tabRef.current.disableTab("ficheiros"); 
        formRefgeral.current.setValue({ "parent_id": form_construcaonew_paiId });
      } 
      if (!permitemovimentos){
        tabRef.current.disableTab("movimentos");
      }
      if (!permissaoEditarItem){
        windowRef.current.header.disable('gravar'); 
		    menuRefgeometrias.current.disable(["desenhar"]);
        menuRefgeometrias.current.disable(["importar"]);
      }
  
      if (windowRef.current!=undefined){
        windowRef.current.show();
      }else {
        menuRef.current?.destructor();
        tabRef.current?.destructor();
        formRefgeral.current?.destructor();
        windowRef.current?.destructor();
        menuRef.current = null;
        tabRef.current = null;
        formRefgeral.current = null;
        windowRef.current = null;
      } 
    } else {
      // form_construcaoedit_recId está vazio logo destroy the window e tudo o resto 
      menuRef.current?.destructor();
      tabRef.current?.destructor();
      formRefgeral.current?.destructor();
      windowRef.current?.destructor();
      menuRef.current = null;
      tabRef.current = null;
      formRefgeral.current = null;
      windowRef.current = null;
    }

    return () => {
      menuRef.current?.destructor();
      tabRef.current?.destructor();
      formRefgeral.current?.destructor();
      windowRef.current?.destructor();
      menuRef.current = null;
      tabRef.current = null;
      formRefgeral.current = null;
      windowRef.current = null;
    };
  }, [form_construcaoedit_recId,form_construcaonew_paiId]);
 
 
  useEffect(() => {

    const transformResponseObjetos = (originalResponses) => {

    
      // Initialize an empty array to store the transformed data
      const transformedData = [];
      // Initialize a counter for appending to descriptions
      let counter = 1;
      if (Array.isArray(originalResponses)){
         // Loop through each original response record
        originalResponses.forEach((originalResponse) => {
          // Extract relevant fields from the original response
          const { recId, tipoconstrucaoId, designacao,  talhaoId, geometriaWKT, centroid, area, perimetro, mbr } = originalResponse;
          if (geometriaWKT!=''){
            // Check if "descricao" is null or an empty string
            let transformedDescricao = designacao + '_geom';
            
            if (designacao === null || designacao === '') {
              transformedDescricao = `Geom_${counter}`;
            counter++; // Increment the counter
            }
            // Create the desired structure for each record
            const transformedRecord = {
              value: transformedDescricao, // Use the "descricao" field as the "value"
              id:  recId, // Use the "recId" field as the "id" 
              geometria: geometriaWKT,
              mbr: mbr,
              centroid: centroid,
              area: area, 
              perimetro: perimetro,
              icon: {
                folder: "fas fa-book",
                openFolder: "fas fa-book-open",
                file: "fas fa-file",
              },
            };
        
            // Push the transformed record into the items array
            transformedData[0] = transformedData[0] || {
              value: "Objecto",
              id: "objectos",
              opened: true,
              items: [],
            };
            transformedData[0].items.push(transformedRecord);
          }
        });
      } else {
         // Extract relevant fields from the original response
         const { recId, tipoconstrucaoId, designacao,  talhaoId, geometriaWKT, centroid, area, perimetro, mbr  } = originalResponses;
         if (geometriaWKT!=''){
          // Check if "descricao" is null or an empty string
          let transformedDescricao = designacao + '_geom';

          if (designacao === null || designacao === '') {
            transformedDescricao = `Geom_${counter}`;
            counter++; // Increment the counter
          }
          // Create the desired structure for each record
            const transformedRecord = {
              value: transformedDescricao, // Use the "descricao" field as the "value"
              id:  recId, // Use the "recId" field as the "id" 
              geometria: geometriaWKT,
              mbr: mbr,
              centroid: centroid,
              area: area, 
              perimetro: perimetro,
              icon: {
                folder: "fas fa-book",
                openFolder: "fas fa-book-open",
                file: "fas fa-file",
              },
            };
        
      
          // Push the transformed record into the items array
          transformedData[0] = transformedData[0] || {
            value: "Objecto",
            id: "objectos",
            opened: true,
            items: [],
          };
          transformedData[0].items.push(transformedRecord);
         }else {
           transformedData[0] = transformedData[0] || {
            value: "Objecto",
            id: "objectos",
            opened: true,
            items: [],
          };
         }
         
      }   
      if (transformedData.length==0){
        transformedData[0] = transformedData[0] || {
          value: "Objecto",
          id: "objectos",
          opened: true,
          items: [],
        };
      }
      return transformedData;
    }    
   
    if ((treeRefgeometrias.current !=null )&&(datageom !=null )){
      const transformedResult = transformResponseObjetos(datageom); 
      treeRefgeometrias.current.data.parse(transformedResult);
      setDataGeom(datageom);
    } else {
      if ((treeRefgeometrias.current !=null )&&(datageom ==null )){
        let dados = {
          value: "Objectos",
          id: "objectos",
          opened: true,
          items: [],
        };
        treeRefgeometrias.current.data.parse(dados);
      }
    }
    
  }, [datageom]);
  
  function closeWindow() {    
    
    dispatch({type: Actions.CONSTRUCAO_FECHA,payload: ""});  
    dispatch({type: Actions.HIDE_IMPORTAFILE_CONSTRUCAO, payload: ""});
    dispatch({type: Actions.REFRESH_END_IMPORTAFILE_CONSTRUCAO, payload: ""});
 
  }
  
  const refreshDataGeometrias = async () => {
    const url3 = apiEndpointCadastro + 'ConstrucaoGeom/' + form_construcaoedit_recId;
    //console.log('chamada fetchDataGeometrias : ' + url3);
    const response = await fetch(url3);
    const json = await response.json();
    //console.log('resposta fetchDataGeometrias : ' + JSON.stringify(json));
    if (json?.status==404){
      
    } else{
      setDataGeom(json);
    }
    
    /*formRefconcessionarios.current.getItem('tipoconstrucao').data.load(url2).then(() => {          
    });*/
  }

  const onzoomToWindow = (centroid, bboxWKT, muda_escala) => {
    //var centroid = dadosConstrucao?.centroid;
    if(centroid!=null){
      const coordsc = centroid.substring(7, centroid.length - 1).split(' ');
      const centro_x = parseFloat(coordsc[0]);
      const centro_y = parseFloat(coordsc[1]);

      //var bboxWKT = dadosConstrucao?.bbox;

        // Extract the coordinates from the WKT string
      const coords = bboxWKT.substring(10, bboxWKT.length - 2).split(',');

      // Parse the coordinates as numbers
      const points = coords.map((coord) => {
        coord=coord.trim();
        const [px, py] = coord.split(' ');
        return [parseFloat(px), parseFloat(py)];
      });

      // Calculate the width and height of the bounding box
      const [minx, miny] = points[0];
      let maxx = minx;
      let maxy = miny;
      points.forEach(([px, py]) => {
        if (px < minx) minx = px;
        if (px > maxx) maxx = px;
        if (py < miny) miny = py;
        if (py > maxy) maxy = py;
      });
      const width = Math.abs(maxx - minx);
      const height = Math.abs(maxy - miny);

      let estado = viewer.getState();			
											
			let activeMapName = estado.config.activeMapName;

			let mapState = estado.mapState[activeMapName];
			//var currentMap = mapState.mapguide.runtimeMap;
		
			let viewa = estado.mapState[activeMapName].currentView; 
			let escala = viewa.scale;

      // Calculate the initial map scale
      const resolution = Math.max(width / 800, height / 600);
      if (muda_escala==1){
        escala = parseInt( resolution / 0.00028) + 50;
      }
      if (centro_x!=null && centro_y!= null && escala !=null){
        console.log(centro_x);
        console.log(centro_y);
        console.log(escala); 
        var state = viewer.getState(); 
        var NomeMapa = state.config.activeMapName;
        
        var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }};
        //zoomToView(aux);
        viewer.dispatch(aux);
      }
     
      // estado = viewer.getState();			
      //let activeMapName = estado.config.activeMapName;
      //let mapState = estado.mapState[activeMapName];
      let currentMap = mapState.mapguide.runtimeMap; 
      let sessionId  = currentMap.SessionId; 
      let mapaDef  = currentMap.MapDefinition;
       
      let aux_construcaoid = form_construcaoedit_recId;
      let ObjectoGeografico_id=formRefgeral.current.getItem("objectogeografico_recid")._value;
      
      if (ObjectoGeografico_id === undefined) {
        ObjectoGeografico_id = '';
      }
      
      setConstrucoesSelection(apiEndpointSIG, authtoken, activeMapName, mapaDef, sessionId, aux_construcaoid, centro_x, centro_y, escala);

     
    } 
    
  }


  const ondeleteWindow = (objgeomid) => {
    const DeleteObjGeometry = (objgeomid)=> {
      try {				
        const deleteConstrucaoObj = async () => {                   
          const url = apiEndpointCadastro + 'ConstrucaoObjGeom';//ok     
          const jwtToken = authtoken;    
          const response = await fetch(url, {
            method: 'DELETE',
            body: JSON.stringify({ 
                id : form_construcaoedit_recId,
                construcaoobj_id: objgeomid
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
              // mapa refresh
              // tree refresh      
              if (viewer_interface ==null){
                viewer_interface = GetViewerInterface(); 
              } 
              viewer_interface.clearSelection();
              refreshDataGeometrias();               
              var state = viewer.getState();
              const args = state.config;
              var NomeMapa = state.config.activeMapName;
              const uid = uuidv4();
              var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};                     
              viewer.dispatch(aux);   
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        deleteConstrucaoObj();
      } catch (error) {
        console.error('error: ' + error);                 
      } 
    }
    
    // vamos criar uma janela de confirmação do delete

    dhx.confirm({
      header:"Remover objecto",
      text:"Tem a certeza que pretende remover o objecto?",
      buttons:["não", "sim"],
      buttonsAlignment:"center"
    }).then(function(resposta){
      console.log('resposta ', resposta);
      if (resposta){DeleteObjGeometry(objgeomid);}
    });
  }
  const localizaConstrucao = (centroid, bboxWKT, aux_construcaoid) => {
    
    if(centroid!=null){
       const coordsc = centroid.substring(7, centroid.length - 1).split(' ');
       const centro_x = parseFloat(coordsc[0]);
       const centro_y = parseFloat(coordsc[1]);
  
     // Extract the coordinates from the WKT string
       const coords = bboxWKT.substring(10, bboxWKT.length - 2).split(',');
 
       // Parse the coordinates as numbers
       const points = coords.map((coord) => {
        coord=coord.trim();
         const [px, py] = coord.split(' ');
         return [parseFloat(px), parseFloat(py)];
       });
 
       // Calculate the width and height of the bounding box
       const [minx, miny] = points[0];
       let maxx = minx;
       let maxy = miny;
       points.forEach(([px, py]) => {
         if (px < minx) minx = px;
         if (px > maxx) maxx = px;
         if (py < miny) miny = py;
         if (py > maxy) maxy = py;
       });
       const width = Math.abs(maxx - minx);
       const height = Math.abs(maxy - miny);
       let estado = viewer.getState();
       let activeMapName = estado.config.activeMapName;

       //var view = estado.mapState[activeMapName].currentView; 
       //var escala = view.scale;
       
       let resolution = Math.max(width / 800, height / 600);
       let escala = parseInt( resolution / 0.00028) + 50;
       if (centro_x!=null && centro_y!= null && escala !=null){ 
         var state = viewer.getState(); 
         var NomeMapa = state.config.activeMapName; 
         var aux={ type:'Map/SET_VIEW', payload: { mapName: NomeMapa,  view: { x: centro_x, y: centro_y, scale: escala } }}; 
         viewer.dispatch(aux);
       } 
      
       let mapState = estado.mapState[activeMapName];
       let currentMap = mapState.mapguide.runtimeMap; 
       let sessionId  = currentMap.SessionId; 
       let mapaDef  = currentMap.MapDefinition;
         
       let ObjectoGeografico_id= '';
       setConstrucoesSelection(apiEndpointSIG, authtoken, activeMapName, mapaDef, sessionId, aux_construcaoid, centro_x, centro_y, escala);
     }
 } 
  
  
   
  const onDefinicao = (construcaorec_id, ObjectoGeografico_id, ObjectoGeografico_nome, rec_id_definicao) => {
    const onDefinicaoObjGeometry = (construcaorec_id, ObjectoGeografico_id, ObjectoGeografico_nome, rec_id_definicao)=> {
      try {				
        const setDefinicaoConstrucaoObj = async () => {                   
          const url = apiEndpointCadastro + 'ConstrucaoObjGeom';//ok     
          const jwtToken = authtoken;    
          const response = await fetch(url, {
            method: 'PUT',
            body: JSON.stringify({ 
                id : construcaorec_id,
                construcaoobj_id: ObjectoGeografico_id,  
                construcaoobj_nome: ObjectoGeografico_nome,
                construcaoobj_tipoid: rec_id_definicao
              }),
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwtToken}`
              },
          });              
          if (response.ok) {
              refreshDataGeometrias();    
              var state = viewer.getState();
              const args = state.config;
              var NomeMapa = state.config.activeMapName;
              const uid = uuidv4();
              var aux={ type:'Legend/SET_LAYER_VISIBILITY', payload: { id: uid, value: true, mapName: NomeMapa }};                     
              viewer.dispatch(aux);   
          } else {
            const errorMessage = await response.text();
            console.error('error: ' + errorMessage);
          }
        };
        setDefinicaoConstrucaoObj();
      } catch (error) {
        console.error('error: ' + error);                 
      } 
    } 
    onDefinicaoObjGeometry(construcaorec_id, ObjectoGeografico_id, ObjectoGeografico_nome, rec_id_definicao);
  }
     
  return (
    <div ref={windowRef}></div>
  );
};
PCC_Form_Construcao.propTypes = {
  form_construcaoedit_recId: PropTypes.string.isRequired,
};
export default  PCC_Form_Construcao ;
