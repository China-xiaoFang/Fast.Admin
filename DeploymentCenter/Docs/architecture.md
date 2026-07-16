# Architecture and delivery design

The Server is the authoritative control plane. It persists applications, environments, nodes, releases, deployments, RBAC grants, and immutable audit records. It exposes HTTP APIs for management and SignalR hubs for agent commands and real-time logs/status.

`Shared` owns all domain-neutral contracts. `IDeploymentStrategy` selects the deployment flow, `IStorageProvider` isolates local/MinIO/S3 storage, and `IAgentControlClient` isolates command transport. The orchestrator only coordinates these interfaces; it contains no platform-specific deployment logic.

| Strategy | Flow | Failure behavior |
| --- | --- | --- |
| Single | dispatch one node | request rollback |
| Rolling | dispatch percentage batches | stop at first failed batch and roll back completed nodes |
| Blue-green | stage inactive directory, check, switch Nginx upstream, drain old directory | retain old upstream and restore it on failure |

Agent task executors download, checksum, unpack, back up, invoke platform scripts, health-check, retain the configured version count, and report structured events. Platform execution is intentionally an Agent extension point so container/Kubernetes executors can be added without changing Server services.

Deployment lifecycle: validate authorization → save release metadata → create plan → emit live event → dispatch batch → collect health/status → traffic switch when required → audit success. A failed task stops remaining batches and emits rollback commands.

The Web console subscribes to `DeploymentHub` to show the same event stream as an operator `logs -f` session. The API path is `/api/deployments`, the deployment hub is `/hubs/deployment`, and the agent hub is `/hubs/agent`.
