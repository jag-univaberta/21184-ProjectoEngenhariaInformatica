import React from 'react';
import { useSelector } from 'react-redux';
import Pedidos_Display from './pedidos_display'; 
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context"; 
import {IAppPccReducerState1} from "../../reducers/app_pcc_reducer" 

export const Container_Pedidos = () => { 
  const dispatch = useReduxDispatch();  
  const quantidadePedidos = useSelector((state: Readonly<IAppPccReducerState1>)=> state.aplicacaopcc.pedidos_api.numero_pedidos);
 

  return <div>
  {(() => {
      if (quantidadePedidos > 0) {
          return <Pedidos_Display quantidadePedidos={quantidadePedidos} /> 
      }      
  })()}
  </div>
}