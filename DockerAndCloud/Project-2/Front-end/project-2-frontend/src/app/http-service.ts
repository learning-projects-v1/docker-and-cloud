import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ApiEndpointsService } from "./api.endpoints.service";

@Injectable({providedIn: 'root'})
export class HttpService{
    constructor(private httpClient: HttpClient, private apiEndpointsService: ApiEndpointsService){
    }

    public getFileReadResult(){
        const endpoint = this.apiEndpointsService.api.fileRead;
        return this.httpClient.get<string>(endpoint);
    }

    public getSystemInfo(){
        const endpoint = this.apiEndpointsService.api.systemInfo;
        return this.httpClient.get<string>(endpoint);
    }

    public getRootResult(){
        const endpoint = this.apiEndpointsService.api.root;
        return this.httpClient.get<string>(endpoint);
    }

    public getEnvironmentInfo(){
        const endpoint = this.apiEndpointsService.api.environment;
        return this.httpClient.get<string>(endpoint);
    }

    public getLogs(){
        const endpoint = this.apiEndpointsService.api.log;
        return this.httpClient.get<string>(endpoint);
    }

    public postLogs(req: any){
        const endpoint = this.apiEndpointsService.api.log;
        const headers = new HttpHeaders().set('Content-Type', 'application/json');
        return this.httpClient.post<any>(endpoint, JSON.stringify(req), {headers: headers});
    }

    public deleteLogs(){
        const endpoint = this.apiEndpointsService.api.log;
        return this.httpClient.delete(endpoint);
    }

    
}