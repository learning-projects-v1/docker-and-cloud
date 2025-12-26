import { environment } from "../environment/environment";


export const ApiEndpoints = {
    fileRead : `${environment.apiBaseUrl}/test/file-read`,
    systemInfo: `${environment.apiBaseUrl}/test/system`,
    root: `${environment.apiBaseUrl}/test`,
    environment: `${environment.apiBaseUrl}/test/environment`,
    log: `${environment.apiBaseUrl}/test/log`,
}