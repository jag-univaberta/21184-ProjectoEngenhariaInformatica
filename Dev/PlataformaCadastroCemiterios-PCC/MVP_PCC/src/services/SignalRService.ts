
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

export  class SignalRService{

    private signalRConnection?: HubConnection;

    createUserRoomConnection(){
        this.signalRConnection= new HubConnectionBuilder()
        .withUrl(`https://localhost:7282/connectionuser`, {//?connectionId=${connectionId}`, { //adiciona o connectionId como parametro de consulta.
          withCredentials: true,
        })
        .withAutomaticReconnect()
        .build();

        this.signalRConnection.on('UserConnected',()=>{
            console.log ('Server called here')
        });
    }
}