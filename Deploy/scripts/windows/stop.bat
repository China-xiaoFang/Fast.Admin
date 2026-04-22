@echo off
REM Fast Deploy - Stop Script (Windows)
REM Usage: stop.bat [Port]

set PORT=%~1

if "%PORT%"=="" (
    echo [FastDeploy] Usage: stop.bat [Port]
    exit /b 1
)

echo [FastDeploy] Stopping application on port %PORT%...

for /f "tokens=5" %%a in ('netstat -aon ^| findstr ":%PORT% " ^| findstr "LISTENING"') do (
    echo [FastDeploy] Killing process %%a
    taskkill /f /pid %%a
    goto :done
)

echo [FastDeploy] No process found listening on port %PORT%.
:done
echo [FastDeploy] Stop complete.
