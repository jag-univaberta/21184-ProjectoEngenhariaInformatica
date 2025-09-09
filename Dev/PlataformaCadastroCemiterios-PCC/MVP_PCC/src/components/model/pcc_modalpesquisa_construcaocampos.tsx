import { Component, PureComponent } from "react";
 
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common';  
import React = require("react");
import Wpcc_Form_ConstrucaoPesquisaCampos from "../window/Wpcc_Form_ConstrucaoPesquisaCampos";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
 
import { useDispatch, useSelector } from "react-redux";
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 
import { useState, useEffect  } from "react";
 
export const ModalPesquisa_ConstrucaoCampos = () => {

    
   
    const obj_pesquisaconstrucao = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.pesquisa_construcaocampos.obj_pesquisaconstrucao);

    return <div>
    {(() => {        
        if (obj_pesquisaconstrucao) {          
            return  <Wpcc_Form_ConstrucaoPesquisaCampos obj_pesquisaconstrucao ={obj_pesquisaconstrucao}/> 
        }
    })()}
    </div>
}
 
 

