import React from 'react'; 
import { useState, useEffect  } from "react";
import { useSelector } from 'react-redux';
import * as Actions from "../../constants/actions";


const Pedidos_Display = ({ quantidadePedidos, message, customStyle }) => {


  const [previousquantidadePedidos, setPreviousquantidadePedidos] = useState(null);
  useEffect(() => {
    if ((quantidadePedidos !== previousquantidadePedidos)&&(quantidadePedidos >0) ) {

      setPreviousquantidadePedidos(quantidadePedidos);
    }

  }, [quantidadePedidos, previousquantidadePedidos]);

  const defaultOverlayStyle = {
    position: 'fixed',
    bottom: '0px',
    left: '465px',
    //right: '1100px',
    backgroundColor: 'rgb(15 108 189)',
    color: 'white',
    paddingLeft: '5px',
    //padding: '10px',
    //borderRadius: '5px',
    zIndex: '9999',
    display: 'flex',
    alignItems: 'center',
    height: '19px',
  };

  const overlayStyle = { ...defaultOverlayStyle, ...customStyle };

  const messageStyle = {
    fontSize: '10pt',
  };

  const spinnerStyle = {
    width: '15px',
    height: '15px',
    border: '3px solid #f3f3f3',
    borderTop: '3px solid #3498db',
    borderRadius: '50%',
    animation: 'spin 1s linear infinite',
    marginLeft: '10px',
    marginRight: '5px',
  };

  const keyframes = `
    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }
  `;

  const defaultMessage = `A processar ${quantidadePedidos} ${quantidadePedidos > 1 ? 'pedidos' : 'pedido'}`;

  return (
    <div style={overlayStyle}>
      <style>{keyframes}</style>
      <div style={messageStyle}>
        {message || defaultMessage}
      </div>
      <div style={spinnerStyle}></div>
    </div>
  );
};

Pedidos_Display.defaultProps = {
  quantidadePedidos: 0,
  message: '',
  customStyle: {},
};

export default Pedidos_Display;