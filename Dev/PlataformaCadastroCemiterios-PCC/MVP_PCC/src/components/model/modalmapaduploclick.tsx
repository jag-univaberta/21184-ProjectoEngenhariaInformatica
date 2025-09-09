import { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog";
import React = require("react");
import FormGU_MapaDuploClick from "../window/FormGU_MapaDuploClick"; 
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer"  
  
export const ModalFormGU_MapaDuploClick = () => {

    
    const mapa_duploclick = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.mapa_duploclick.show_janela);
    const obj_info_WKTs = useSelector((state: Readonly<IAppPccReducerState1>)=>  state.aplicacaopcc.windows.mapa_duploclick.obj_info_WKTs);
 
    return <div>
          
    {(() => {                  
            //return  <FormGU_MapaDuploClick mapa_duploclick={mapa_duploclick}/> 
            return  <FormGU_MapaDuploClick mapa_zoomcoordenadas={mapa_duploclick} obj_info_WKTs={obj_info_WKTs} /> 

    })()}
    </div>
    
    
}
 
 

