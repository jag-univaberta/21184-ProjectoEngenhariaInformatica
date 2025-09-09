import React, { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog"; 
import PCC_Form_Cemiterio from "../window/Wpcc_Form_Cemiterio";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
import { useState, useEffect  } from "react";
 
export const PCC_ModalCemiterio = () => {  
 
    const form_cemiterioedit_recId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.form_cemiterioedit_recId); 
    
    return <div>
    {(() => {          
         if (form_cemiterioedit_recId !="") {        
            return  <PCC_Form_Cemiterio  form_cemiterioedit_recId={form_cemiterioedit_recId}/> 
         }
    })()}
    </div>
}
 
 

 