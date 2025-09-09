 
import React = require("react");
import Menu_GestaoTabelas from "../pcc_comp/Menu_GestaoTabelas"; 
import { useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 

export const ModalMenu_GestaoTabelas = () => {
    const apiEndpoint = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpoint);
    const authtoken = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.authtoken);
    const userid = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.geral.userid);
    const aplicacao_sigla = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.aplicacao_sigla);
    return <div>
    {(() => {                  
            return  <Menu_GestaoTabelas authtoken={authtoken} userid={userid}  apiEndpoint={apiEndpoint}  aplicacao_sigla={aplicacao_sigla}/> 

    })()}
    </div>
}
 
 

