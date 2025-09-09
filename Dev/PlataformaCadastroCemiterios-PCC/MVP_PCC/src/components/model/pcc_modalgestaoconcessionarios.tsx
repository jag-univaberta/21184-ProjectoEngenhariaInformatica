import { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog";
import React = require("react");
import Wpcc_GestaoConcessionarios from "../window/Wpcc_GestaoConcessionarios";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
import { useState, useEffect  } from "react";
 
export interface IJanelaDetalhesGestaoConcessionariosTemplateState {
    obj_tipoatendimento: string;
    obj_tipoatendimento_show: boolean;
}

export function modaltipointervsoloTemplateReducer(origState: IJanelaDetalhesGestaoConcessionariosTemplateState, state: any, action = { type: '', payload: "" }) :ITemplateReducerState
{
    
    switch (action.type) {
        case Actions.HIDE_GESTAOCONCESSIONARIOS: 
            return {
                ...state,
                windows: {
                  ...state.windows,
                  gestao_concessionarios: {
                    ...state.windows.gestao_concessionarios,
                    obj_concessionarios_show: false
                  }
                }
              };
    }
    return state;
}
export const PCC_ModalGestaoConcessionarios = () => { 
    const dispatch = useReduxDispatch();

    const hideWindow= () => dispatch({
        type: Actions.HIDE_GESTAOCONCESSIONARIOS,
        payload: ""});
    const onHideWindow = () => hideWindow();
 
    const obj_concessionarios_show = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.gestao_concessionarios.obj_concessionarios_show);


    
    return <div>
    {(() => {      
        if (obj_concessionarios_show){
            return  <Wpcc_GestaoConcessionarios obj_concessionarios_show={obj_concessionarios_show}/> 
        }     

    })()}
    </div>
}
 
 

