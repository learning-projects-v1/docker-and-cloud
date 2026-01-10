
import * as signalR from "@microsoft/signalr"
import { ApiEndPoints } from "../configs/api-endpoints";
import { ResponseModel } from "../models/dtos";
import { concatWith, Subject } from "rxjs";
import { Injectable } from "@angular/core";


@Injectable({providedIn: 'root'})
export class SignalrService {
    private hubConnection!: signalR.HubConnection;
    private responseSubjectSource = new Subject<ResponseModel>();
    public responseSubject$ = this.responseSubjectSource.asObservable();

    public start() {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(ApiEndPoints.NotificationHubEndpoint)
            .withAutomaticReconnect()
            .build();

        this.hubConnection.start().then(res => {
            console.log("SignalR started");
        })
        
        this.hubConnection.on('onReceiveResponseSignalr', (response: ResponseModel) => {
            console.log("websocket method 'onReceiveResponseSignalr' invoked!")
            this.responseSubjectSource.next(response);

        });
    }

   private onResultReceived(){
        this.hubConnection.on('onReceiveResponseSignalr', (response: ResponseModel) => {
            this.responseSubjectSource.next(response);
        });
    }
}