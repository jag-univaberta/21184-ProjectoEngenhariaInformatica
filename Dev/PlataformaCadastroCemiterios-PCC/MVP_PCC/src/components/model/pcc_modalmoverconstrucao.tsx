 
import React = require("react");  
import { useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer";
import Wpcc_Form_MoverConstrucao from "../window/Wpcc_Form_MoverConstrucao";
 

export const PCC_ModalMoverConstrucao = () => {

    const apiEndpoint = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpoint);

        
    const obj_moverconstrucao_show= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.moverconstrucao.obj_moverconstrucao_show);
    const parentconstrucaoid= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.moverconstrucao.parentconstrucaoid);
    const construcaoid= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.moverconstrucao.construcaoid);

    //const form_impressoesgerir_id = "novaimpressao";
    

    return <div>
        {(() => {
                            
            if (obj_moverconstrucao_show) {
 
                return  <Wpcc_Form_MoverConstrucao obj_moverconstrucao_show={obj_moverconstrucao_show} parentconstrucaoid= {parentconstrucaoid} construcaoid={construcaoid}/> 
 
            }

        })()}
    </div>

}
 
 

