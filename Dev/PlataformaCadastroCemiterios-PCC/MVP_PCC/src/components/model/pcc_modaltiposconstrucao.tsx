import React, { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog"; 
import Wpcc_TiposConstrucao from "../window/Wpcc_TiposConstrucao";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
import { useState, useEffect  } from "react";
 
export interface IJanelaDetalhesTiposConstrucaoTemplateState { 
    obj_tiposconstrucao_show: boolean;
}

export function modalGestaoTiposConstrucaoTemplateReducer(origState: IJanelaDetalhesTiposConstrucaoTemplateState, state: any, action = { type: '', payload: "" }) :ITemplateReducerState
{
    
    switch (action.type) {
        case Actions.HIDE_GESTAOTIPOSCONSTRUCAO: 
            return {
                ...state,
                windows: {
                  ...state.windows,
                  gestao_construcoes: {
                    ...state.windows.gestao_construcoes,
                    obj_tiposconstrucao_show: false
                  }
                }
              };
    }
    return state;
}
export const PCC_ModalTiposConstrucao = () => {
    /*const {
        dispatch
    } = useCommonTemplateState();*/
    const dispatch = useReduxDispatch();

    const hideWindow= () => dispatch({
        type: Actions.HIDE_GESTAOTIPOSCONSTRUCAO,
        payload: ""});
    const onHideWindow = () => hideWindow();
 
    const obj_tiposconstrucao_show = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.gestao_construcoes.obj_tiposconstrucao_show);


    
    return <div>
    {(() => {                  
        if (obj_tiposconstrucao_show){
            return  <Wpcc_TiposConstrucao obj_tiposconstrucao_show={obj_tiposconstrucao_show}/> 
        }
          

    })()}
    </div>
}
 
 

