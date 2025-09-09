import React, { ReactElement } from 'react';

type Props = {
  title: string;
  children: ReactElement | ReactElement[];
};

const TabPane = ({ children }: Props): JSX.Element => <div className={`tabpane`}>{children}</div>;

export default TabPane;