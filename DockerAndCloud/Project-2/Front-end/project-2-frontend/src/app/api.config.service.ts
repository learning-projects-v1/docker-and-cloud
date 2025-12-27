import { Injectable } from "@angular/core";


@Injectable({providedIn: 'root'})
export class ApiConfigService{
    private config!: {apiBaseUrl: string};

    async load(){
        this.config = await fetch('/assets/config.json').then(res => res.json());
    }

    getApiBaseUrl(){
        return this.config.apiBaseUrl;
    }
}