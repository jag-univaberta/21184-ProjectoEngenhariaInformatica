import { Component, PureComponent } from "react";
import React from "react";  
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog";
import Wpcc_Form_ConstrucaoImportFicheiro from "../window/Wpcc_Form_ConstrucaoImportFicheiro";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
import { useState, useEffect  } from "react";
import '../../../css/pcc.css';
import { useSelector } from 'react-redux';
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 

export interface IPCCLayoutTemplateState { 
    obj_importaficheiro_show: boolean;
}

 

export function modalconstrucaoimportwindowTemplateReducer(origState: IPCCLayoutTemplateState, state: any, action = { type: '', payload: "" }) :ITemplateReducerState
{
    
    switch (action.type) {
        case Actions.HIDE_IMPORTAFILE_CONSTRUCAO:
            //return { ...state, windows: { detalhes_objectos: { obj_info_show: false }}}; 
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
    }
    return state;
}
export const PCC_ModalConstrucaoImportarFicheiro = () => {
    /*const {
        dispatch
    } = useCommonTemplateState();*/
    const dispatch = useReduxDispatch();
    const [dialogPosition, setDialogPosition] = useState({ x: (window.innerWidth / 2) - 400, y: 10 });
    const hideWindow= () => dispatch({
        type: Actions.HIDE_IMPORTAFILE_CONSTRUCAO,
        payload: ""});
    const onHideWindow = () => hideWindow();
 
    const obj_importaficheiroconstrucao_show = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.importa_construcao.obj_importaficheiroconstrucao_show);
    const obj_importaficheiroconstrucao_id = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.importa_construcao.obj_importaficheiroconstrucao_id);
 
    
    return <div className="ficheirosGeral">
    {(() => {
        if (obj_importaficheiroconstrucao_show == true) {
            return <RndModalDialogG20 
            locale={"pt"} 
            isOpen={true}
            title={"Importar Ficheiro"}
            onClose={onHideWindow} 
            x={dialogPosition.x}
            y={dialogPosition.y}
            width={400}
            height={380}
            disableYOverflow={false}
            enableInteractionMask={true}> 
            {([, h]) => <Wpcc_Form_ConstrucaoImportFicheiro obj_importaficheiroconstrucao_show={obj_importaficheiroconstrucao_show} obj_importaficheiroconstrucao_id={obj_importaficheiroconstrucao_id}/>}
        </RndModalDialogG20>

        }                  
            

    })()}
    </div>
}

 
 

