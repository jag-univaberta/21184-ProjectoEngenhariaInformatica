 
import React = require("react");
import Menu_InfoUser from "../pcc_comp/Menu_InfoUser"; 
import { useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 

export const ModalMenu_InfoUser = () => {
    const username = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.username);
    console.log(username);
    //const treecartografia_itenschecked = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.treecartografia.treecartografia_itenschecked);
    return <div>
    {(() => {                  
            return  <Menu_InfoUser /> 

    })()}
    </div>
}
 
 

