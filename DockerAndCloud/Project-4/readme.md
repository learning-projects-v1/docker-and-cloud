# Notification Router System

Microservice-based notification system demonstrating **Angular frontend**, **.NET Web API**, and **asynchronous consumers** with RabbitMQ. Fully containerized with Docker for easy deployment.

---

## Architecture

```
flowchart LR
    Angular[Angular Dashboard (Nginx)] --> API[.NET Web API]
    API --> RabbitMQ[RabbitMQ]
    RabbitMQ --> EC[Email Consumer]
    RabbitMQ --> PC[Payment Consumer]
    RabbitMQ --> RC[Retry Consumer]
```

