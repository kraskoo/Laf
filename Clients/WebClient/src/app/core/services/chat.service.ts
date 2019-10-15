import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hubConnection: signalR.HubConnection;
  private chatUrl = `chat`;

  startConnection() {
    const builder = new signalR.HubConnectionBuilder().withUrl(`${environment.url}/${this.chatUrl}`);
    this.hubConnection = builder.build();
    this.hubConnection.start().then(() => console.log('Connection started!')).catch(console.error);
  }

  messageTo(user: string, message: string) {
    this.hubConnection.invoke('SendMessage', user, message);
  }

  initRevieceMessage(onReceiveCb) {
    // tslint:disable-next-line: only-arrow-functions
    this.hubConnection.on('ReceiveMessage', onReceiveCb);
  }
}
