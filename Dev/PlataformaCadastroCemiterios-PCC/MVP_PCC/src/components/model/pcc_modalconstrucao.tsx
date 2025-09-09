 
import React from "react";  
import PCC_Form_Construcao from "../window/Wpcc_Form_Construcao";
import { useSelector } from 'react-redux';
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
 
export const PCC_ModalConstrucao = () => {    
    const form_construcaoedit_recId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.form_construcaoedit_recId);
    const form_construcaonew_paiId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.form_construcaonew_paiId);
    
    return <div>
    {(() => {
                        
        if (((form_construcaoedit_recId !="")&&(form_construcaonew_paiId==""))||((form_construcaoedit_recId =="")&&(form_construcaonew_paiId !=""))) {
            return  <PCC_Form_Construcao form_construcaoedit_recId={form_construcaoedit_recId} form_construcaonew_paiId={form_construcaonew_paiId} />  
        }

    })()}
    </div>
}

 
 

