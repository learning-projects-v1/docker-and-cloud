import { environment } from "../../environments/environment.development";
import { ApiPaths } from "./api-paths";


export class ApiEndPoints{
    private static readonly BASE = environment.api.baseUrl;
    private static readonly ApiVersoin = `Api/${environment.api.version}`;

    static readonly DirectMessagees = {
        Direct: `${ApiEndPoints.BASE}/${ApiEndPoints.ApiVersoin}/${ApiPaths.Publish.Root}/${ApiPaths.Publish.Direct}`
    };
    static GetMessageEndpoint = (type: string) => `${ApiEndPoints.BASE}/${ApiEndPoints.ApiVersoin}/${ApiPaths.Publish.Root}/${type}`
    
    static readonly RabbitmqResponses = {
        Get: `${ApiEndPoints.BASE}/${ApiEndPoints.ApiVersoin}/${ApiPaths.RabbitMqResponses.Root}/${ApiPaths.RabbitMqResponses.Get}`
    };

    static readonly NotificationHubEndpoint = `${ApiEndPoints.BASE}/${ApiPaths.signalR}`;
}

