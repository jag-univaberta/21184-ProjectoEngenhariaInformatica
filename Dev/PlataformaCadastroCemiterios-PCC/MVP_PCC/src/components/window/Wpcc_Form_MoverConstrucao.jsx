import React, { useState, useEffect, useRef   } from "react";
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';

const Wpcc_Form_MoverConstrucao= ({obj_moverconstrucao_show, parentconstrucaoid, construcaoid}) => {
  const windowRef = useRef(null);  
 
  const [dataPretensao, setDataPretensao] = useState(null);
  const [dataGridPretensao, setDataGridPretensao] = useState(null);
  const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint);
  const authtoken = useSelector((state)=> state.aplicacaopcc.geral.authtoken);
  const userid = useSelector((state)=> state.aplicacaopcc.geral.userid);

  const layoutRefgeometrias = useRef(null); 
  const treePretensao = useRef(null);
  const gridPretensao = useRef(null);
  const FormDadosEnt = useRef(null);

  const handleClose = () => { 
    console.log('Close Ordenar Impressões');
    
    var aux={ type: Actions.HIDE_CONSTRUCAO_MOVER, payload: "" };
    viewer.dispatch(aux); 
    
  };

  useEffect(() => {
      
    const fetchDataTreeImpressoes = async () => {//ok
       
       
      const url = apiEndpoint + 'PretensaoMoverTree/' + userid;//ok
      const jwtToken = authtoken; 

		  async function fetchDataImpressoes() {
        try {
 
          console.log('jwtToken: ' + jwtToken);

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
            //setDataImpressoes(jsonData); 
            //console.log(jsonData);
            treePretensao.current.data.removeAll();
            treePretensao.current.data.parse(jsonData);
            treePretensao.current.expandAll();
            
            fetchDatagridPretensao();
          })
          .catch(error => {
            console.error('There was a problem with your fetch operation:', error);
          }); 
        } catch (e) {
            console.log(e.message || 'Unexpected error');
        }

      }
 
      fetchDataImpressoes();
  
    }
    fetchDataTreeImpressoes();
  
    const fetchDatagridPretensao = async () => {
      const url = apiEndpoint + 'gridPretensoes?id=' + parentconstrucaoid;
      const jwtToken = authtoken;
    
      async function fetchDatagrid() {
        try {
          console.log('jwtToken: ' + jwtToken);
    
          const response = await fetch(url, {
            headers: {
              'Authorization': `Bearer ${jwtToken}`
            }
          });
    
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
    
          const jsonData = await response.json();
          console.log('Dados originais:', jsonData);
    
          // Encontrar o item pai na árvore de pretensão usando o rec_id
          const parentItem = treePretensao.current.data?.find(item => item.recid === parentconstrucaoid);
          if (!parentItem) {
            throw new Error('Item pai não encontrado na árvore de pretensão');
          }
    
          // Criar o item raiz para a pretensão atual
          const rootItem = {
            id: parentconstrucaoid,
            value: parentItem.value,
            items: [] // Aqui vamos armazenar os itens filhos
          };
    
          // Adicionar os itens do JSON como filhos diretos do item raiz
          jsonData.forEach(item => {
            rootItem.items.push({
              id: item.rec_id,
              value: item.nome
            });
          });
    
          console.log('Dados transformados:', rootItem);
    
          // Limpar dados existentes
          gridPretensao.current.data.removeAll();
    
          // Carregar novos dados
          gridPretensao.current.data.parse([rootItem]);
    
          // Expandir todos os nós
          gridPretensao.current.expandAll();

          // Fazer o check do item escolhido
          gridPretensao.current.checkItem(pretensaoid); 
          
        } catch (error) {
          console.error('Erro ao buscar ou processar dados:', error);
        }
      }
    
      await fetchDatagrid();
    };
    
    
  
}, [obj_moverconstrucao_show]);


useEffect(() => {
 
  if ((treePretensao.current !=null )&&(dataPretensao !=null )){

    treePretensao.current.data.removeAll();
    treePretensao.current.data.parse(jsonData);
    treePretensao.current.expandAll();
    
    setDataPretensao(dataPretensao);
  }


}, [dataPretensao]);

useEffect(() => {
 
  if ((gridPretensao.current !=null )&&(dataGridPretensao !=null )){

    gridPretensao.current.data.removeAll();
    gridPretensao.current.data.parse(jsonData);
    gridPretensao.current.expandAll();
    
    setDataGridPretensao(dataGridPretensao);
  }


}, [dataGridPretensao]);

  useEffect(() => {
    if (obj_moverconstrucao_show !== false) {
       
      const windowHtmlContainer = `<div id="formContainer"></div>`
      windowRef.current = new WindowDHX({
        width: 900,
        height: 540,
        title: 'Mover Construção', 
        closable: true,
        movable: true,
        resizable: true,
        modal: false,
        header: true,
        footer: true,
        css: "pcc_window",
      });
      globalWindowReference = windowRef.current;
  
      // criar a árvore das geometrias
      treePretensao.current = new TreeDHX(null,{
        checkbox: true, 
        dragMode:"both", 
        dropBehaviour:"complex",
        isFolder: function (item) {
          return item.tipo === "GRUPO";
        }
      });

        // criar a árvore das geometrias
      gridPretensao.current = new TreeDHX(null,{
        checkbox: true, 
        dragMode:"both", 
        dropBehaviour:"complex",
      }); 

      function closeWindow() {
        if (windowRef.current) {
          windowRef.current.close();
        }
      
      }

      FormDadosEnt.current = new FormDHX(null,{ 
        css: "dhx_widget--bg_white dhx_widget--bordered pcc_layout_interv",
        padding: 17,
        cols:  
            [ 
              { type: "spacer", height: 15 }, // Espaço antes das rows  
              { css: "pcc_input", id: "buttonsave", type: "button", value: "Mover", tooltip: "Mover", title: "Mover" },
            ]  
      }); 
      
      FormDadosEnt.current.getItem("buttonsave").events.on("click", function(events) {
        console.log("click", events);

        handleSave();
      });

      function handleSave() {
        const contSaveImpressoes = (ParentId, Items)=> {   
          const saveImpressoes  = async (ParentId, Items) => { 
                              
            const url = apiEndpoint + 'PretensaoMover';//ok 
            const jwtToken = authtoken;        
            const response = await fetch(url, {
              method: 'PUT',
              body: JSON.stringify({
                ParentId: ParentId,
                Items: Items 
              }),
              headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${jwtToken}`
                },
            });              
            if (response.ok) {
              dhx.alert({
                header:"Gravação efectuada",
                text:"A Gravação foi efetuada com sucesso!",
                buttonsAlignment:"center",
                buttons:["ok"],
              }).then(function(){
                var aux={ type: Actions.TREECEM_RELOADTREE,  payload: {
                  reloadKey: Date.now().toString()
                }};
			
                viewer.dispatch(aux);
                viewer.dispatch({type: Actions.TREECEM_LIMPA, payload: "" })
                viewer.dispatch({type: Actions.TREECEM_SELECIONA, payload: parentconstrucaoid }) 

                ReloadData();

                windowRef.current.hide();
              });
            } else {
              dhx.alert({
								header: "Erro",
								text: "Não é possível mover o ITEM para fora de um grupo ou para dentro de outro ITEM.",
								buttonsAlignment: "center",
								buttons: ["ok"],
							});
              ReloadData();
            }
          };
          saveImpressoes (ParentId, Items);     
        }
       if (FormDadosEnt.current) {
        // Obter os dados da árvore
        let GrupoId = treePretensao.current.getChecked();
        let ParentRecId = treePretensao.current.data?.getItem(GrupoId[0])?.recid;
        let ItemId = gridPretensao.current.getChecked();

        // Função para processar a árvore e coletar apenas os itens filhos
        function processTree(items, parentId) {
          let result = [];
          items.forEach(item => {
            // Verifica se o item não é o pai
            if (item !== parentId) {
              result.push(item);
            }
          });
          return result;
        }

        // Processa os itens, excluindo o pai
        let childItems = processTree(ItemId, ParentRecId);

        // Criar a string para salvar
        let dados = childItems.join("#");

        // Log para verificar o tipo de dados
        console.log("GRUPO:", ParentRecId);
        console.log("ITENS FILHOS:", dados);

        // Chama a função de salvamento apenas se houver itens filhos
        if (childItems.length > 0) {
          contSaveImpressoes(ParentRecId, dados);
        } else {
          console.log("Nenhum item filho selecionado.");
          dhx.alert({
            header: "Erro",
            text: "Nenhuma Pretensão Seleccionada",
            buttonsAlignment: "center",
            buttons: ["ok"],
          });
        }
      }
 
      
      }

      const ReloadData = async () => { 
        treePretensao.current.data.removeAll();
        const url = apiEndpoint + 'ArvPretensoes/' + userid;//ok
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
          treePretensao.current.data.parse(jsonData);
        })
        .catch(error => {
          console.error('There was a problem with your fetch operation:', error);
        });
        
  
      }

      layoutRefgeometrias.current = new LayoutDHX(null,{
        type: "line",
        rows: [
          {
          height: 350,
          cols: [
            {
              id: "treegeo1",
              html: "treegeo1",
              header:"Lista de Pretensões:", 
              width:"50%",
              css: "pcc_layout",
              progressDefault: true,
            },
            {
              id: "treegeo2",
              html: "treegeo2",
              header:"Grupo destino:", 
              width:"50%",
              css: "pcc_layout",
              progressDefault: true,
            }
          ]}, 
          {
            id: "dadosent",
            height: 68,
            html: "dadosent",
            css: "pcc_layou_tipointervsolo",
            progressDefault: true,
          }
        ]
      });
   

      layoutRefgeometrias.current.getCell("treegeo1").attach(gridPretensao.current);
      layoutRefgeometrias.current.getCell("treegeo2").attach(treePretensao.current);

      layoutRefgeometrias.current.getCell("dadosent").attach(FormDadosEnt.current);

       
      windowRef.current?.attach(layoutRefgeometrias.current);
 
      windowRef.current?.events.on("move", function(position, oldPosition, side) {
        console.log("The window is moved to " + position.left, position.top)
      });
      
      windowRef.current?.events.on("afterHide", function(position, events){
          console.log("A window is hidden", events);
          handleClose();
          
      });
       
      let objgeomid;

      treePretensao.current.events.on("itemRightClick", function(id, e){
        console.log("The item with the id "+ id +" was right-clicked.");
       
      });

      treePretensao.current.events.on("beforeCheck", function (index, id) {
        console.log("beforeCheck", index, id);
        // return false;
        if(id=='parent'){
          return false;
        }
        const checkedItems = treePretensao.current.getChecked(); 
        checkedItems.forEach(function(itemId) {
          // Se o ID do item marcado não é o ID do item atual, desmarque-o
          if (itemId !== id) {
            treePretensao.current.uncheckItem(itemId);
          }
        });
      });

      // Evento chamado após soltar o item
      treePretensao.current.events.on("beforeDrop", function(data) {
        const draggedItemId = data.source; // ID do item que foi arrastado
        const targetParentId = data.target; // ID do novo pai para onde o item foi solto

        // Obtém o item arrastado
        const draggedItem = treePretensao.current.data.getItem(draggedItemId); 
        const targetItem = treePretensao.current.data.getItem(targetParentId); // Obtém o novo pai

        // Verifica se o item arrastado é do tipo ITEM
        if (draggedItem && draggedItem.tipo === "ITEM") {
            // Verifica se o novo pai é válido
            if (!targetParentId || targetParentId === "" || (targetItem && targetItem.tipo === "ITEM")) {

              dhx.alert({
								header: "Erro",
								text: "Não é possível mover o ITEM para fora de um grupo ou para dentro de outro ITEM.",
								buttonsAlignment: "center",
								buttons: ["ok"],
							});

                return false; // Cancela a ação de mover
            }
        }

        console.log("Item movido com sucesso:", draggedItemId, "para o parent:", targetParentId);
        return true; // Permite a ação de mover
      });

      if (windowRef.current!=undefined){
        windowRef.current.show();
      }else {
        
        windowRef.current = null; 
      } 
    } else {
      // pretensaoId está vazio logo destroy the window e tudo o resto
 
       
      windowRef.current = null;
    }

    return () => {
       
      windowRef.current = null;
    };
  }, [obj_moverconstrucao_show]);

   

  return (
    <div ref={windowRef}></div>
    
  );
  
}

Wpcc_Form_MoverConstrucao.propTypes = {
  obj_moverconstrucao_show: PropTypes.boolean,
};
export default Wpcc_Form_MoverConstrucao;
