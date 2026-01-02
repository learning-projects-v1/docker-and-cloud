# Project-2 — Dockerized Full-Stack Chat Application

## Overview

**Project-2** is a full-stack chat application built using **Angular**, **ASP.NET Core**, with a strong emphasis on real-world architecture, Docker-based deployment, and environment-specific configuration.  
This project is intended for learning, system design practice, and portfolio demonstration.

---

## Tech Stack

### Frontend
- Angular
- Nginx (serving production build)

### Backend
- ASP.NET Core Web API

### Infrastructure
- Docker & Docker Compose
- Nginx (reverse proxy / static hosting)

---

## Configuration & Environments

The backend follows standard ASP.NET Core configuration conventions:

- `appsettings.json` — base configuration
- `appsettings.Development.json` — local development
- `appsettings.Production.json` — mounted via Docker volume

## Running the Application
docker compose up --build

## Design & Learning Goals

Docker multi-stage builds

Docker Compose service orchestration

Backend–frontend communication inside Docker networks

Environment-specific configuration management

Foundations for scalable architecture

