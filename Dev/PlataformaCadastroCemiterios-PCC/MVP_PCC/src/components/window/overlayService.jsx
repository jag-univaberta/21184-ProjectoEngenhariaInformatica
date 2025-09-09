import React from 'react';
import ReactDOM from 'react-dom';
import LoadingOverlay from './LoadingOverlay';

let overlayContainer = null;
let quantidadeImpressoes = 0;

export const incrementarImpressoes = () => {
  quantidadeImpressoes++;
  updateOverlayDisplay();
  return quantidadeImpressoes;
};

export const decrementarImpressoes = () => {
  if (quantidadeImpressoes > 0) {
    quantidadeImpressoes--;
  }
  updateOverlayDisplay();
  return quantidadeImpressoes;
};

export const getQuantidadeImpressoes = () => quantidadeImpressoes;

const updateOverlayDisplay = () => {
  if (overlayContainer) {
    ReactDOM.render(
      <LoadingOverlay 
        quantidadeImpressoes={quantidadeImpressoes} 
        message={`A imprimir ${quantidadeImpressoes} ${quantidadeImpressoes > 1 ? 'impressões' : 'impressão'}`} 
      />,
      overlayContainer
    );
  }
};

export const showOverlay = () => {
  if (!overlayContainer) {
    overlayContainer = document.createElement('div');
    document.body.appendChild(overlayContainer);
  }
  updateOverlayDisplay();
  overlayContainer.style.display = 'block';
};

export const hideOverlay = () => {
  if (overlayContainer) {
    overlayContainer.style.display = 'none';
  }
};

export const updateOverlay = (message) => {
  if (overlayContainer) {
    ReactDOM.render(
      <LoadingOverlay quantidadeImpressoes={quantidadeImpressoes} message={message} />,
      overlayContainer
    );
  }
};