import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiEndpoints } from "./api-endpoints";

@Injectable({providedIn: 'root'})
export class HttpService{
    constructor(private httpClient: HttpClient){

    }

    public getFileReadResult(){
        const endpoint = ApiEndpoints.fileRead;
        return this.httpClient.get<string>(endpoint);
    }

    public getSystemInfo(){
        const endpoint = ApiEndpoints.systemInfo;
        return this.httpClient.get<string>(endpoint);
    }

    public getRootResult(){
        const endpoint = ApiEndpoints.root;
        return this.httpClient.get<string>(endpoint);
    }

    public getEnvironmentInfo(){
        const endpoint = ApiEndpoints.environment;
        return this.httpClient.get<string>(endpoint);
    }

    public getLogs(){
        const endpoint = ApiEndpoints.log;
        return this.httpClient.get<string>(endpoint);
    }

    public postLogs(req: any){
        const endpoint = ApiEndpoints.log;
        const payload = {
            msg: req
        };
        // const header = new HttpHeaders();
        // header.set("Content-Type", "application/json")
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        // return this.httpClient.post<any>(endpoint, payload);
        return this.httpClient.post<any>(endpoint, JSON.stringify(req), {headers: headers});
    }

    
}