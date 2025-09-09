import React = require("react"); 
import Wpcc_Form_CartografiaOrdenar from "../window/Wpcc_Form_CartografiaOrdenar"; 
import { useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer"  

export const PCC_Modal_CartografiaOrdenar = () => {
    const obj_cartografiaordenar_show= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.ordenacartografia.obj_cartografiaordenar_show);
     return <div>
        {(() => {
                            
            if (obj_cartografiaordenar_show) { 
                return  <Wpcc_Form_CartografiaOrdenar obj_cartografiaordenar_show={obj_cartografiaordenar_show}/>  
            }
        })()}
    </div>
}

