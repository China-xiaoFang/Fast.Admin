#!/bin/bash
# Fast Deploy - Health Check Script (Linux)
# Usage: ./health.sh [HealthCheckUrl]
# Exit code 0 = healthy, 1 = unhealthy

HEALTH_URL="${1}"

if [ -z "$HEALTH_URL" ]; then
    echo "HEALTHY"
    exit 0
fi

if curl -sf "$HEALTH_URL" > /dev/null 2>&1; then
    echo "HEALTHY"
    exit 0
else
    echo "UNHEALTHY"
    exit 1
fi
