# Notification Router System

Microservice-based notification system demonstrating **Angular frontend**, **.NET Web API**, and **asynchronous consumers** with RabbitMQ. Fully containerized with Docker for easy deployment.

---

## Architecture
```
Angular Dashboard
      │ HTTP
      ▼
ASP.NET Core API
      │ RabbitMQ (Direct / Topic)
      ▼
Backend Consumer Services (email, payment and retry)
      │ RabbitMQ (Result Events)
      ▼
ASP.NET Core Hosted Service
      │ SignalR (WebSocket)
      ▼
Angular Dashboard (Real-time Updates)
```

## Tech Stack
- **Frontend:** Angular + Nginx
- **Backend:** .NET 8 Web API
- **Messaging:** RabbitMQ
- **Containerization:** Docker, Docker Compose
- **Cloud:** AWS Free Tier EC2

# Screenshots

<img width="1508" height="834" alt="Screenshot 2026-01-12 at 9 46 41 PM" src="https://github.com/user-attachments/assets/a9d616c9-c2fc-42fd-be72-92fbcb8088ec" />
<img width="1507" height="716" alt="Screenshot 2026-01-12 at 9 50 59 PM" src="https://github.com/user-attachments/assets/8d225bfb-6bd4-42d4-b981-f6f3963b86c6" />
<img width="1264" height="170" alt="Screenshot 2026-01-12 at 10 04 34 PM" src="https://github.com/user-attachments/assets/d30abe8c-3764-4be3-bf7e-992a2da4bdb9" />

<p float="left">
  <img src="https://github.com/user-attachments/assets/02d75388-2e04-40cc-b19f-0a78e1846348" width="300" />
  <img src="https://github.com/user-attachments/assets/1fefb026-0b39-43f4-8a8e-4fdf052e8580" width="300" />
  <img src="https://github.com/user-attachments/assets/8ae77322-6358-4c1b-95f2-d9dbb7892c2e" width="300" />
</p>
*Angular Dashboard | RabbitMQ UI* | Ec2 instance | docker-hub | docker ps | consumer logs

## Future Improvements
- Horizontal scaling: add more consumers
- Persistent DB: PostgreSQL or Redis
- HTTPS via Nginx + Let's Encrypt
- CI/CD with GitHub Actions
- Monitoring with CloudWatch / Prometheus

## Quick Deployment
```bash
docker-compose pull
docker-compose up -d
```
## Live Demo (Temporary)

A temporary live instance is available for demo purposes:

- **Frontend (Angular Dashboard):** [http://<EC2_PUBLIC_IP>](http://98.84.98.8/)

> Note: This instance is **temporary** and may be stopped to avoid charges. RabbitMQ and internal services remain private and are not publicly exposed.

