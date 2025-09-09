import React from 'react';
import { IconName } from '@blueprintjs/icons';
import { Icon } from '@blueprintjs/core';


//https://blueprintjs.com/docs/#icons/icons-list

interface IconWithLetterWhiteProps {
  icon: IconName;
  letter: string;
}

const IconWithLetterWhite: React.FC<IconWithLetterWhiteProps> = ({ icon, letter }) => {
  // Style for the container div (adjust as needed)
  const containerStyle: React.CSSProperties = {
    display: 'inline-block',
    position: 'relative',
    color:'#fff',
    width: '100%',
    height: '100%'
  };

  // Style for the letter element
  const letterStyle: React.CSSProperties = {
    position: 'absolute',
    //bottom: '5px', // Adjust the positioning as needed
    //right: '5px', // Adjust the positioning as needed
    fontSize: '10px', // Adjust the font size as needed
    fontWeight: 'normal',
    color: '#fff', // Adjust the color as needed
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

export default IconWithLetterWhite;
