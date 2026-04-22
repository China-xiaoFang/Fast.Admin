#!/bin/bash
# Fast Deploy - Start Script (Linux)
# Usage: ./start.sh [AppType] [AppPath] [Port]
#   AppType: DotNet or Vue
#   AppPath: absolute path to the deployed application
#   Port:    port to listen on

APP_TYPE="${1:-DotNet}"
APP_PATH="${2:-.}"
PORT="${3:-8080}"

cd "$APP_PATH" || exit 1

if [ "$APP_TYPE" = "Vue" ]; then
    echo "[FastDeploy] Serving Vue static files via nginx..."
    nohup nginx -p "$APP_PATH" -c nginx.conf > app.log 2>&1 &
    echo $! > app.pid
    echo "[FastDeploy] nginx started (PID: $!)"
    exit 0
fi

# DotNet
DLL_FILE=$(ls *.dll 2>/dev/null | head -1)
if [ -z "$DLL_FILE" ]; then
    echo "[FastDeploy] ERROR: No .dll file found in $APP_PATH"
    exit 1
fi

echo "[FastDeploy] Starting .NET application: $DLL_FILE on port $PORT..."
nohup dotnet "$DLL_FILE" --urls "http://0.0.0.0:$PORT" > app.log 2>&1 &
echo $! > app.pid
echo "[FastDeploy] Application started (PID: $!)"
