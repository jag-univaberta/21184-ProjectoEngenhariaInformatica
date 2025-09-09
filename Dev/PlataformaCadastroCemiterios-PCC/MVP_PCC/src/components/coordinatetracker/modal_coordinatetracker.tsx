import { Component, PureComponent } from "react";
import React, { useState } from "react";
import * as Actions from "../../constants/actions";
import { connect } from "react-redux";
import { useCommonTemplateState } from 'mapguide-react-layout/lib/layouts/hooks';
import { ITemplateReducerState } from 'mapguide-react-layout/lib/api/common'; 
import  RndModalDialogG20  from "../model/modal-dialog";

import { PlaceholderComponent, DefaultComponentNames } from "mapguide-react-layout/lib/api/registry/component";
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";


import { useSelector } from 'react-redux';
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer"   
 
export const Modal_CoordinateTracker = () => { 
/*
     
    'EPSG:3763'); // ETRS89
    'EPSG:27493'); // Datum 73
    'EPSG:20790'); // Datum LX
    'EPSG:4326'); // WGS84
    'EPSG:3857'); // Web Mercator
    
    */
   const obj_trackcoordenadas_show = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.windows.trackcoordenadas.obj_trackcoordenadas_show);

    const dispatch = useReduxDispatch();
    const [dialogPosition, setDialogPosition] = useState({ x: 1450, y: 10 });
    const hideWindow= () => dispatch({
        type: Actions.HIDE_TRACKCOORDENADAS,
        payload: ""});
    const onHideWindow = () => hideWindow();
   
return <div className="CoordinateTracker">
{(() => {
     if (obj_trackcoordenadas_show == true) {
        return <RndModalDialogG20 
        locale={"pt"} 
        isOpen={true}
        title={"Monitor de Coordenadas"}
        onClose={onHideWindow} 
        x={dialogPosition.x}
        y={dialogPosition.y}
        width={350}
        height={380}
        disableYOverflow={false}
        enableInteractionMask={true}> 
        {([, h]) => <PlaceholderComponent id="CoordinateTracker" componentProps={{ projections:["EPSG:4326", "EPSG:3857", "EPSG:27493", "EPSG:20790", "EPSG:3763"] }} /> }
    </RndModalDialogG20>

     }           
        

})()}
</div>
}

 
 

