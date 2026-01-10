import { ApiEndPoints } from "./api-endpoints";
import { ApiPaths } from "./api-paths";

export interface RabbitmqRoutingKeyConfig {
    routePattern: string,
    actualRoute?: string[],
}

export interface RabbitmqExchangeConfig {
    name: string;
    routingKeys: RabbitmqRoutingKeyConfig[];
}

export interface RabbitmqTypeConfig {
    name: string;
    description: string;
    exchanges: RabbitmqExchangeConfig[];
}

export class RabbitmqConfig {
    private static config: RabbitmqTypeConfig[] = [
        {
            name: ApiPaths.Publish.Direct,
            description: "Publish directly to the target queue",
            exchanges: [
                {
                    name: "direct",
                    routingKeys: [{
                        routePattern: "email",
                    },
                    {
                        routePattern: "payment",
                    }
                    ]
                }
            ]
        },
        {
            name: ApiPaths.Publish.Topic,
            description: "Publish to topic exchange with varying routing keys",
            exchanges: [
                {
                    name: "topic",
                    routingKeys: [
                        {
                            routePattern: "email.*"
                        },
                        {
                            routePattern: "*.email.*"
                        },
                        {
                            routePattern: "payment.#"
                        },
                        {
                            routePattern: "*.*.payment"
                        }
                    ]
                },
            ]
        },
        {
            name: ApiPaths.Publish.Topic,
            description: "Publish to a service that is mostly unavailable. It then retries up to max limit. If it doesn't success within the retry limits, it sends the request to a dead letter queue",
            exchanges: [
                {
                    name: 'project4.unavailable.exchange',
                    routingKeys: [
                        {
                            routePattern: "unavailable"
                        }
                    ]
                }

            ]
        },
        {
            name: ApiPaths.Publish.Fanout,
            description: "Sends a fanout request meaning it sends the request to all consumers that is currently bound to that exchange.",
            exchanges: [
                {
                    name: 'project4.all.exchange',
                    routingKeys: [
                        {
                            routePattern: "doesn't matter"
                        }
                    ]
                }

            ]
        }
    ]

    public static getConfig() {
        return this.config;
    }
}