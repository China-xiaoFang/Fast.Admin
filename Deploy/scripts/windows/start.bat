@echo off
REM Fast Deploy - Start Script (Windows)
REM Usage: start.bat [AppType] [AppPath] [Port]
REM   AppType: DotNet or Vue
REM   AppPath: absolute path to the deployed application
REM   Port:    port to listen on

set APP_TYPE=%~1
set APP_PATH=%~2
set PORT=%~3

if "%PORT%"=="" set PORT=8080

cd /d "%APP_PATH%"

if /i "%APP_TYPE%"=="Vue" (
    echo [FastDeploy] Serving Vue static files via nginx...
    start "" nginx -p "%APP_PATH%" -c nginx.conf
    goto :done
)

REM Default: DotNet
echo [FastDeploy] Starting .NET application on port %PORT%...
for %%f in (*.dll) do (
    start "" dotnet "%%f" --urls "http://0.0.0.0:%PORT%"
    goto :done
)

echo [FastDeploy] ERROR: No .dll file found in %APP_PATH%
exit /b 1

:done
echo [FastDeploy] Application started.
