import React from 'react';
import { IconName } from '@blueprintjs/icons';
import { Icon } from '@blueprintjs/core';

//https://blueprintjs.com/docs/#icons/icons-list

interface IconWithLetterProps {
  icon: IconName;
  letter: string;
}

const IconWithLetter: React.FC<IconWithLetterProps> = ({ icon, letter }) => {
  // Style for the container div (adjust as needed)
  const containerStyle: React.CSSProperties = {
    display: 'inline-block',
    position: 'relative',
    width: '100%',
    height: '100%'
  };

  // Style for the letter element
  const letterStyle: React.CSSProperties = {
    position: 'absolute',
    //bottom: '5px', // Adjust the positioning as needed
    //right: '5px', // Adjust the positioning as needed
    fontSize: '10px', // Adjust the font size as needed
    fontWeight: 'bold',
    color: '#000', // Adjust the color as needed
    width: '100%',
    height: '100%',
    top: '13px',
    left: '0px'
  };

  return (
    <div style={containerStyle}>
      <Icon icon={icon} />
      <span style={letterStyle}>{letter}</span>
    </div>
  );
};

export default IconWithLetter;
