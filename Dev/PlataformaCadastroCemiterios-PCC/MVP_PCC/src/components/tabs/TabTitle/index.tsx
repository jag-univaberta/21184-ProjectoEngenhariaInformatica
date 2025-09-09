import React, { useCallback } from "react";
//import styles from "./tabTitle.module.css";

export type Props = {
  title: string;
  index: number;
  setSelectedTab: (index: number) => void;
  isActive?: boolean;
};

const TabTitle = (props: Props): JSX.Element => {
  const { title, setSelectedTab, index, isActive } = props;

  const handleOnClick = useCallback(() => {
    setSelectedTab(index);
  }, [setSelectedTab, index]);

  return ( 
    <li className={`tabtitle ${isActive ? "active" : ""}`}>
      <button onClick={handleOnClick}>{title}</button>
    </li>
  );
};

export { TabTitle as default };
