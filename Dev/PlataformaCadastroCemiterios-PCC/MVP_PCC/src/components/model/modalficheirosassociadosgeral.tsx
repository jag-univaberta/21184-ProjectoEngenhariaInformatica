 
import React = require("react"); 
import FormGU_FicheirosAssociados from "../window/FormGU_FicheirosAssociados"; 
import { useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
 

export const Modal_FicheirosAssociadosGeral = () => {

    const apiEndpoint = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.config.configapiEndpoint);

        
    const obj_ficheiro_show= useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.ficheiros_associados.obj_ficheiro_show);

    //const form_impressoesgerir_id = "novaimpressao";
    

    return <div>
        {(() => {
                            
            if (obj_ficheiro_show) {
 
                return  <FormGU_FicheirosAssociados obj_ficheiro_show={obj_ficheiro_show}/> 
 
            }

        })()}
    </div>

}
 
 

