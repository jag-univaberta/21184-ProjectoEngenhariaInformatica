import { Component, PureComponent } from "react";
import React, { useState } from "react";
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "../model/modal-dialog";

import { PlaceholderComponent, DefaultComponentNames } from "mapguide-react-layout/lib/api/registry/component";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
import { LegendContainer } from 'mapguide-react-layout/lib/containers/legend';

import { useSelector } from 'react-redux';
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer"   
 
export const Modal_Legenda = () => { 
 
    const obj_legenda_show = true;//useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.trackcoordenadas.obj_trackcoordenadas_show);

    const dispatch = useReduxDispatch();
    const [dialogPosition, setDialogPosition] = useState({ x: 1450, y: 10 });
    const hideWindow= () => dispatch({
        type: Actions.HIDE_TRACKCOORDENADAS,
        payload: ""});
    const onHideWindow = () => hideWindow();
   
return <div className="Legenda">
{(() => {
     if (obj_legenda_show == true) {
        return <RndModalDialogG20 
        locale={"pt"} 
        isOpen={true}
        title={"Legenda"}
        onClose={onHideWindow} 
        x={dialogPosition.x}
        y={dialogPosition.y}
        width={350}
        height={380}
        disableYOverflow={false}
        enableInteractionMask={true}> 
        {([, h]) =>  <LegendContainer />}
    </RndModalDialogG20>

     }           
        

})()}
</div>
}

 
 

