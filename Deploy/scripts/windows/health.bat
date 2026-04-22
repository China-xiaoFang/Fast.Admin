@echo off
REM Fast Deploy - Health Check Script (Windows)
REM Usage: health.bat [HealthCheckUrl]
REM Exit code 0 = healthy, 1 = unhealthy

set HEALTH_URL=%~1

if "%HEALTH_URL%"=="" (
    echo HEALTHY
    exit /b 0
)

curl -sf "%HEALTH_URL%" >nul 2>&1
if %errorlevel% == 0 (
    echo HEALTHY
    exit /b 0
) else (
    echo UNHEALTHY
    exit /b 1
)
