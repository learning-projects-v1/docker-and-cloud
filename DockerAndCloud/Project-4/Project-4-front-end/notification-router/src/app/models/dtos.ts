

export interface RequestModel{
    exchangeType: string,
    exchangeName: string,
    message: string,
    routingKey: string,
    correlationId: string;
}

export interface ResponseModel{
    correlationId: string,
    payload: string
}

