import React, { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog"; 
import PCC_Form_Talhao from "../window/Wpcc_Form_Talhao";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
import { useState, useEffect  } from "react";
 
export const PCC_ModalTalhao = () => {  
 
     

    const form_talhaoedit_recId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.form_talhaoedit_recId);
    const form_talhaonew_paiId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.form_talhaonew_paiId);
       
    return <div>
    {(() => {          
          if (((form_talhaoedit_recId !="")&&(form_talhaonew_paiId==""))||((form_talhaoedit_recId =="")&&(form_talhaonew_paiId !=""))) {
       
            return  <PCC_Form_Talhao form_talhaoedit_recId={form_talhaoedit_recId} form_talhaonew_paiId={form_talhaonew_paiId} /> 
 
         }
    })()}
    </div>
}
 
 

 