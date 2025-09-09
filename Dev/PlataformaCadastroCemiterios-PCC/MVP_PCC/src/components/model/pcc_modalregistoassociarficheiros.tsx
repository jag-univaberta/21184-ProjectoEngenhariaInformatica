import { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG999  from "./modal-dialog999";
import React = require("react");
import Wpcc_Form_AssociarFicheiro from "../window/Wpcc_Form_AssociarFicheiro";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
import { useState, useEffect  } from "react";
import '../../../css/pcc.css';
import { useSelector } from 'react-redux';
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 

export interface IPCCLayoutTemplateState { 
    obj_importaficheiro_show: boolean;
}

 
export function PCC_modalassociarficheiroswindowTemplateReducer(origState: IPCCLayoutTemplateState, state: any, action = { type: '', payload: "" }) :ITemplateReducerState
{
    
    switch (action.type) {
        case Actions.HIDE_ASSOCIAFILE:
            //return { ...state, windows: { detalhes_objectos: { obj_info_show: false }}}; 
            return {
                ...state,
                windows: {
                  ...state.windows,
                  importa_atendimento: {
                    ...state.windows.importa_atendimento,
                    obj_associaficheiro_show: false,
                    obj_associaficheiro_id: "",
                    obj_listaficheiros_refresh: false,

                  }
                }
              };
    }
    return state;
}

export const PCC_ModalRegistoAssociarFicheiros = () => {
    /*const {
        dispatch
    } = useCommonTemplateState();*/
    const dispatch = useReduxDispatch();
    const [dialogPosition, setDialogPosition] = useState({ x: 850, y: 10 });
    const hideWindow= () => dispatch({
        type: Actions.HIDE_ASSOCIAFILE,
        payload: ""});
    const onHideWindow = () => hideWindow();
 
    const obj_associaficheiro_show = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.associa_ficheiro.obj_associaficheiro_show);
    const obj_associaficheiro_id = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.associa_ficheiro.obj_associaficheiro_id);
    const obj_associaficheiro_tipoassociacao = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.associa_ficheiro.obj_associaficheiro_tipoassociacao);
    return (
        <div className="ficheirosGeral">
          {(() => {
            if (obj_associaficheiro_show === true) {
              return (
                <>
                  {/* Overlay cinza que bloqueia a tela */}
                  <div
                    style={{
                      position: 'fixed',
                      top: 0,
                      left: 0,
                      width: '100%',
                      height: '100%',
                      backgroundColor: 'rgba(128, 128, 128, 0.7)', // Cor cinza com transparência
                      zIndex: 1000, // Certifique-se de que esteja acima de outros elementos
                    }}
                  />
      
                  {/* Modal que será renderizado no topo */}
                  <RndModalDialogG999
                    locale={"pt"}
                    isOpen={true}
                    title={"Associar Ficheiro"}
                    onClose={onHideWindow}
                    x={dialogPosition.x}
                    y={dialogPosition.y}
                    width={365}
                    height={430}
                    disableYOverflow={false}
                    enableInteractionMask={true} 
                  >
                    {([, h]) => (
                      <Wpcc_Form_AssociarFicheiro
                        obj_associaficheiro_show={obj_associaficheiro_show}
                        obj_associaficheiro_id={obj_associaficheiro_id}  
                        obj_associaficheiro_tipoassociacao={obj_associaficheiro_tipoassociacao}

                      />
                    )}
                  </RndModalDialogG999>
                </>
              );
            }
          })()}
        </div>
      );

    
}

 
 

