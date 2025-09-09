import { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog";
import React = require("react");
import FormGU_MapaZoomCoordenadas from "../window/FormGU_MapaZoomCoordenadas"; 
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer"  
  
export const ModalFormGU_MapaZoomCoordenadas = () => {

    
    const mapa_zoomcoordenadas = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.mapa_zoomcoordenadas.show_janela);
 
    return <div>
    {(() => {                  
            return  <FormGU_MapaZoomCoordenadas mapa_zoomcoordenadas={mapa_zoomcoordenadas}/> 

    })()}
    </div>
}
 
 

