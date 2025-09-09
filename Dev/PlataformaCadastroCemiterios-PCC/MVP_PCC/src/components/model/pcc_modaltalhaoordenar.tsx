 
import React = require("react"); 
import Wpcc_Form_TalhaoOrdenar from "../window/Wpcc_Form_TalhaoOrdenar"; 
import { useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
 

export const PCC_Modal_TalhaoOrdenar = () => {
        
    const obj_talhaoordenar_show= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.ordenaconstrucao.obj_talhaoordenar_show);
    return <div>
        {(() => {
                            
            if (obj_talhaoordenar_show) { 
                return  <Wpcc_Form_TalhaoOrdenar obj_talhaoordenar_show={obj_talhaoordenar_show}/>  
            }
        })()}
    </div>
}
 
 

