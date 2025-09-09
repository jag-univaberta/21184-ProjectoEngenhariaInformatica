import React from 'react';

const LoadingOverlay = ({ quantidadeImpressoes, message, customStyle }) => {
  const defaultOverlayStyle = {
    position: 'fixed',
    bottom: '4px',
    right: '1100px',
    backgroundColor: 'rgb(15 108 189)',
    color: 'white',
    padding: '10px',
    borderRadius: '5px',
    zIndex: '9999',
    display: 'flex',
    alignItems: 'center',
  };

  const overlayStyle = { ...defaultOverlayStyle, ...customStyle };

  const messageStyle = {
    fontSize: '16px',
  };

  const spinnerStyle = {
    width: '20px',
    height: '20px',
    border: '3px solid #f3f3f3',
    borderTop: '3px solid #3498db',
    borderRadius: '50%',
    animation: 'spin 1s linear infinite',
    marginLeft: '10px',
  };

  const keyframes = `
    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }
  `;

  const defaultMessage = `A imprimir ${quantidadeImpressoes} ${quantidadeImpressoes > 1 ? 'impressões' : 'impressão'}`;

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

LoadingOverlay.defaultProps = {
  quantidadeImpressoes: 0,
  message: '',
  customStyle: {},
};

export default LoadingOverlay;