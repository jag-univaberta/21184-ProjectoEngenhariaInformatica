import { Component, PureComponent } from "react";
import React, { useState } from "react";
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "./modal-dialog";  

import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";


import { useSelector } from 'react-redux';
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer"  
import Wpcc_Form_CartografiaItem from "../window/Wpcc_Form_CartografiaItem";
 
export const PCC_ModalCartografiaItem = () => {
 
     
    const form_cartografiaitemedit_recId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.form_cartografiaitemedit_recId);
    const form_cartografiaitemnew_paiId = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.form_cartografiaitemnew_paiId);


    return <div>
    {(() => {
                        
        if (((form_cartografiaitemedit_recId !="")&&(form_cartografiaitemnew_paiId==""))||((form_cartografiaitemedit_recId =="")&&(form_cartografiaitemnew_paiId !=""))) {

            return  <Wpcc_Form_CartografiaItem form_cartografiaitemedit_recId={form_cartografiaitemedit_recId} form_cartografiaitemnew_paiId={form_cartografiaitemnew_paiId}/> 

  
        }

    })()}
    </div>
}

 
 

