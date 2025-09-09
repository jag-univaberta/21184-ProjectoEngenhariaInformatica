 
import React = require("react"); 
import Form_Coordenadas from "../window/Form_Coordenadas"; 
import { useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
 

export const Modal_Coordenadas = () => {

    const apiEndpoint = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpoint);

        
    const obj_coordenadas_show= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.coordenadas.obj_coordenadas_show);

    const coordenadas= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.coordenadas.coordenadas);
    const srid= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.coordenadas.srid);


    return <div id="containercoordenada"> 
    <div id="coordenadalista" className="listacoordenada">

        {(() => {
                            
            if (obj_coordenadas_show) {
               
               
                return  <Form_Coordenadas  coordenadas={coordenadas} srid={srid} /> 

               
            }

        })()}
    </div>
 </div>
}
 
 

