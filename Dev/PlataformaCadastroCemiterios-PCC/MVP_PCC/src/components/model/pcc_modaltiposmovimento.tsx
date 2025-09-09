import  React,{ Component, PureComponent } from "react"; 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog"; 
import Wpcc_TiposMovimento from "../window/Wpcc_TiposMovimento";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
import { useState, useEffect  } from "react";
 
export interface IJanelaTiposMovimentoTemplateState { 
    obj_tiposmovimento_show: boolean;
}

export function modaltiposmovimentoTemplateReducer(origState: IJanelaTiposMovimentoTemplateState, state: any, action = { type: '', payload: "" }) :ITemplateReducerState
{
    
    switch (action.type) {
        case Actions.HIDE_GESTAOTIPOSMOVIMENTO: 
            return {
                ...state,
                windows: {
                  ...state.windows,
                  gestao_movimentos: {
                    ...state.windows.gestao_movimentos,
                    obj_tiposmovimento_show: false
                  }
                }
              };
    }
    return state;
}
export const PCC_ModalTiposMovimento = () => { 
    const dispatch = useReduxDispatch();

    const hideWindow= () => dispatch({
        type: Actions.HIDE_GESTAOTIPOSMOVIMENTO,
        payload: ""});
    const onHideWindow = () => hideWindow();

    const obj_tiposmovimento_show = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.gestao_movimentos.obj_tiposmovimento_show); 
    return <div>
    {(() => {       
        if (obj_tiposmovimento_show){
            return  <Wpcc_TiposMovimento obj_tiposmovimento_show={obj_tiposmovimento_show}/> 
        }    
    })()}
    </div>
}
 
 

