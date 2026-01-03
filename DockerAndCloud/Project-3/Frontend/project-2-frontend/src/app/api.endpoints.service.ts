import { Injectable } from "@angular/core";
import { ApiConfigService } from "./api.config.service";

export interface ApiEndpoints {
  fileRead: string;
  systemInfo: string;
  root: string;
  environment: string;
  log: string;
}


export function createApiEndpoints(apiBaseUrl: string): ApiEndpoints {
  return {
    fileRead: `${apiBaseUrl}/test/file-read`,
    systemInfo: `${apiBaseUrl}/test/system`,
    root: `${apiBaseUrl}/test`,
    environment: `${apiBaseUrl}/test/environment`,
    log: `${apiBaseUrl}/test/log`,
  };
}

@Injectable({providedIn: 'root'})
export class ApiEndpointsService{
    private endpoints!: ApiEndpoints;

    constructor(private config: ApiConfigService) {
        
    }

    init(){
        this.endpoints = createApiEndpoints(this.config.getApiBaseUrl());
    }

    get api(){
        return this.endpoints;
    }
}

