# Security and operations

Authenticate people with JWT/OIDC and agents with a rotated AccessKey/SecretKey proof or short-lived JWT. Store only salted key hashes and encrypted secret material. Keep encryption keys in a platform secret store, not the repository. Enforce TLS, origin allow-lists, rate limits, tenant scoping, and RBAC policies for application, version, deployment, rollback, node, and settings actions.

Roles are Administrator, Operator, Developer, and Reader. Every state-changing request must record actor, tenant, action, resource, request metadata, and result in `dc_audit_logs`.

Use signed or SHA-256-verified archives. Reject path traversal during extraction, run scripts with a restricted service account, allowlist script names, set process timeouts, and redact secrets from streamed logs. Object storage adapters must use server-side encryption and least-privilege credentials.

## Host deployment

- **Windows:** run the Agent as a Windows service; package PowerShell scripts.
- **Linux:** run it under a dedicated `systemd` user; package executable POSIX shell scripts.
- **macOS:** use a dedicated `launchd` service user and POSIX shell scripts.
- **Docker:** run `docker compose up --build`; use an external database and object store in production.

For Nginx blue-green releases, maintain `blue` and `green` roots, start and health-check the inactive root, atomically update the upstream include, run `nginx -s reload`, then gracefully drain the old process. Do not switch traffic before health succeeds.
