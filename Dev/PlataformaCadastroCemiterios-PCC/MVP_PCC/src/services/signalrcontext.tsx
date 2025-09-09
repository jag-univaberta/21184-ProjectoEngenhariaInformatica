// signalRContext.tsx
import React, { createContext, useState, useEffect, useContext } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

interface SignalRContextProps {
  connection: HubConnection | null;
  startConnection: (connectionId: string) => Promise<void>;
  stopConnection: () => Promise<void>;
  sendMessage: (method: string, ...args: any[]) => Promise<void>;
  registerHandler: (method: string, handler: (...args: any[]) => void) => void;
  unregisterHandler: (method: string) => void;
}

const SignalRContext = createContext<SignalRContextProps | undefined>(
  undefined
);

export const SignalRProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [handlers, setHandlers] = useState<{
    [method: string]: (...args: any[]) => void;
  }>({});
//const apiEndpoint = useSelector((state)=> state.aplicacaopcc.config.configapiEndpoint); 
  const startConnection = async (connectionId: string) => {
    try {
      const newConnection = new HubConnectionBuilder()
        .withUrl(`https://localhost:7282/ChatHub?connectionId=${connectionId}`, { //adiciona o connectionId como parametro de consulta.
          withCredentials: true,
        })
        .withAutomaticReconnect()
        .build();

      setConnection(newConnection);

      await newConnection.start();
      console.log('SignalR Connected');

      for (const method in handlers) {
        newConnection.on(method, handlers[method]);
      }
    } catch (error) {
      console.error('SignalR Connection Error: ', error);
    }
  };

  const stopConnection = async () => {
    if (connection) {
      await connection.stop();
      console.log('SignalR Disconnected');
      setConnection(null);
    }
  };

  const sendMessage = async (method: string, ...args: any[]) => {
    if (connection) {
      try {
        await connection.invoke(method, ...args);
      } catch (error) {
        console.error('Error sending message: ', error);
      }
    }
  };

  const registerHandler = (
    method: string,
    handler: (...args: any[]) => void
  ) => {
    setHandlers((prevHandlers) => ({ ...prevHandlers, [method]: handler }));
    if (connection) {
      connection.on(method, handler);
    }
  };

  const unregisterHandler = (method: string) => {
    setHandlers((prevHandlers) => {
      const newHandlers = { ...prevHandlers };
      delete newHandlers[method];
      return newHandlers;
    });
    if (connection) {
      connection.off(method);
    }
  };

  useEffect(() => {
    return () => {
      if (connection) {
        connection.off('ReceiveMessage');
      }
    };
  }, [connection]);

  const value: SignalRContextProps = {
    connection,
    startConnection,
    stopConnection,
    sendMessage,
    registerHandler,
    unregisterHandler,
  };

  return (
    <SignalRContext.Provider value={value}>
      {children}
    </SignalRContext.Provider>
  );
};

export const useSignalR = () => {
  const context = useContext(SignalRContext);
  if (!context) {
    throw new Error('useSignalR must be used within a SignalRProvider');
  }
  return context;
};