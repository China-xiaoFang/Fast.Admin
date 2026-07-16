# DeploymentCenter

An isolated .NET 10, Vue 3, Server + Agent release control plane for Windows, Linux, and macOS.

## Layout

| Directory | Responsibility |
| --- | --- |
| `Server` | HTTP API, SignalR hubs, orchestration, storage adapters |
| `Agent` | Cross-platform SignalR agent and task executor |
| `Shared` | Contracts, models, storage and deployment strategy interfaces |
| `Web` | Vue 3/Vite live operations console |
| `Database` | PostgreSQL schema |
| `Docs` | Architecture, protocol, security, and operations guidance |
| `examples` | Portable package manifest and script examples |

## Quick start

```sh
dotnet run --project Server
dotnet run --project Agent
cd Web && pnpm install && pnpm dev
```

The agent identity must be configured through environment variables or a secret manager:

```sh
Agent__ServerUrl=http://server:8080 Agent__NodeId=<node-guid> dotnet run --project Agent
```

Never store access keys, agent secrets, or JWTs in `appsettings.json`.

## Package format

A release archive contains `app/`, `scripts/`, and `manifest.json`. The agent verifies the uploaded archive SHA-256 before extracting it. Scripts support `start`, `stop`, `restart`, `health`, and `rollback`; provide a `.ps1` variant for Windows and a `.sh` variant for Linux/macOS.

## Validation

```sh
dotnet build DeploymentCenter.slnx
dotnet test DeploymentCenter.slnx
cd Web && pnpm build
docker compose up --build
```
