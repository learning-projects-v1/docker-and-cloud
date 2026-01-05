import { Injectable } from "@angular/core";
import { environment } from "../environment/environment";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom, map } from "rxjs";


@Injectable({providedIn: 'root'})
export class ApiConfigService{
    private config!: {apiBaseUrl: string};
    constructor(private http: HttpClient){

    }
    
    async load(){
        // this.config = await fetch('/assets/config.json').then(res => res.json());
        const apiUrl = environment.apiBaseUrl;
        const apiTestUrl = `${apiUrl}/test`;
        const obs = this.http.get(apiTestUrl);
        return firstValueFrom(obs).then(res => {
            console.log("GOT RES: " + res);
            this.config = {
                apiBaseUrl: apiUrl
            }
        })
    }

    getApiBaseUrl(){
        return this.config.apiBaseUrl;
    }
}