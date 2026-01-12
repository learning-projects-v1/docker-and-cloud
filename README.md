# Project-4: Deploy on Aws

- **GitHub Repository & Deployment:** Original project deployed on AWS - [Notification Router System](https://github.com/learning-projects-v1/Rabbitmq/tree/main/Project-4)


# Project-2: Dockerized Angular-Dotnet Dashboard
Description: A full-stack dashboard featuring an Angular frontend and a .NET Web API. The entire stack is containerized, utilizing NGINX as both a web server for static assets and a reverse proxy for API routing.

Key Technical Highlights:

Reverse Proxy: NGINX handles traffic, mapping /api requests to the .NET container.
Dynamic Config: Uses Docker environment variables to inject configurations at runtime.
Networking: Orchestrated via Docker Compose with a private internal network for inter-service communication.

Stack: Angular | .NET Core | NGINX | Docker Compose

## Run
docker-compose up --build

### alternately the front-end and backend can be run individually by running the corresponding start commands.
* Frontend:

```ng serve```
* Backend: 
```
dotnet build
dotnet run
```
