#!/bin/bash
# Fast Deploy - Stop Script (Linux)
# Usage: ./stop.sh [AppPath]

APP_PATH="${1:-.}"

if [ -f "$APP_PATH/app.pid" ]; then
    PID=$(cat "$APP_PATH/app.pid")
    if kill -0 "$PID" 2>/dev/null; then
        kill "$PID"
        echo "[FastDeploy] Stopped process $PID"
        rm -f "$APP_PATH/app.pid"
    else
        echo "[FastDeploy] Process $PID not running, cleaning up pid file"
        rm -f "$APP_PATH/app.pid"
    fi
else
    echo "[FastDeploy] No app.pid found at $APP_PATH, nothing to stop"
fi
