CREATE TABLE dc_applications (
    id UUID PRIMARY KEY, tenant_id VARCHAR(64) NOT NULL, name VARCHAR(128) NOT NULL,
    application_type VARCHAR(16) NOT NULL, retained_versions INTEGER NOT NULL DEFAULT 20,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (tenant_id, name)
);
CREATE TABLE dc_environments (
    id UUID PRIMARY KEY, tenant_id VARCHAR(64) NOT NULL, name VARCHAR(64) NOT NULL,
    is_production BOOLEAN NOT NULL DEFAULT FALSE, UNIQUE (tenant_id, name)
);
CREATE TABLE dc_nodes (
    id UUID PRIMARY KEY, tenant_id VARCHAR(64) NOT NULL, name VARCHAR(128) NOT NULL,
    platform VARCHAR(16) NOT NULL, access_key_hash VARCHAR(256) NOT NULL,
    secret_ciphertext TEXT NOT NULL, last_heartbeat TIMESTAMP WITH TIME ZONE
);
CREATE TABLE dc_releases (
    id UUID PRIMARY KEY, application_id UUID NOT NULL REFERENCES dc_applications(id),
    version VARCHAR(128) NOT NULL, package_key TEXT NOT NULL, sha256 CHAR(64) NOT NULL,
    is_lts BOOLEAN NOT NULL DEFAULT FALSE, is_prerelease BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (application_id, version)
);
CREATE TABLE dc_deployments (
    id UUID PRIMARY KEY, application_id UUID NOT NULL REFERENCES dc_applications(id),
    environment_id UUID NOT NULL REFERENCES dc_environments(id), release_id UUID NOT NULL REFERENCES dc_releases(id),
    strategy VARCHAR(16) NOT NULL, status VARCHAR(16) NOT NULL, created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
CREATE TABLE dc_audit_logs (
    id UUID PRIMARY KEY, tenant_id VARCHAR(64) NOT NULL, actor_id VARCHAR(128) NOT NULL,
    action VARCHAR(128) NOT NULL, resource_type VARCHAR(64) NOT NULL, resource_id VARCHAR(128), payload JSONB,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
