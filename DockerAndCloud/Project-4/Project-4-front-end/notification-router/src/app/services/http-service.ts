import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { RequestModel, ResponseModel } from "../models/dtos";
import { ApiEndPoints } from "../configs/api-endpoints";
import { ApiPaths } from "../configs/api-paths";


@Injectable({
    providedIn: "root"
})
export class HttpService{

    constructor(private httpClient : HttpClient) {
        
    }

    postDirectRequest(request: RequestModel){
        const requestUrl = ApiEndPoints.DirectMessagees.Direct;
        return this.httpClient.post(requestUrl, request);
    }

    postRequest(request: RequestModel){
        const requestUrl = ApiEndPoints.GetMessageEndpoint(request.exchangeType);
        return this.httpClient.post(requestUrl, request);
    }

    getResponse(){
        const requestUrl = ApiEndPoints.RabbitmqResponses.Get;
        return this.httpClient.get<ResponseModel[]>(requestUrl);
    }
}