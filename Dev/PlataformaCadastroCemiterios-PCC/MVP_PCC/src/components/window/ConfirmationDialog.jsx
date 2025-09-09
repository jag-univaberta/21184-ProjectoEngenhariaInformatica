import React from 'react';

function ConfirmationDialog({ isOpen, titulo, mensagem, onConfirm, onCancel }) {
  return (
    isOpen && (
        <div className="pcc_edit_confirmation_background">

       
        <div className="pcc_edit_confirmation_dialog">
            <div className="pcc_edit_confirmation_dialog_titulo">
                <p>{titulo}</p>
            </div>
            <div className="pcc_edit_confirmation_dialog_content">
            <p>{mensagem}</p>
            <button className="pcc_edit_confirmation_dialog_button" onClick={onConfirm}>Sim</button>
            <button className="pcc_edit_confirmation_dialog_button" onClick={onCancel}>Não</button>
            </div>
        </div>
      </div>
    )
  );
}

export default ConfirmationDialog;
