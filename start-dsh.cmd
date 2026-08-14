@echo off
setlocal EnableExtensions
title DeepSeek Harness Launcher
cd /d "C:\Users\Pluto\AppData\Local\npm-cache\_npx\1e7f6d9597241db0"

set "NODE=D:\node.js\node.exe"
set "BIN=C:\Users\Pluto\AppData\Local\npm-cache\_npx\1e7f6d9597241db0\node_modules\@deepseek-ai\dsh\lib\bin.js"
set "URL=http://127.0.0.1:3080"

REM --- if the server is already running, just open the UI
netstat -ano | findstr ":3080" | findstr "LISTENING" >nul 2>&1
if not errorlevel 1 goto open

REM --- start the server in its own console window (close that window to stop it)
echo Starting DeepSeek Harness server...
start "DeepSeek Harness Server - close this window to stop" "%NODE%" "%BIN%" web

REM --- wait for port 3080 (up to 30 seconds)
for /l %%i in (1,1,30) do (
    netstat -ano | findstr ":3080" | findstr "LISTENING" >nul 2>&1 && goto open
    ping -n 2 127.0.0.1 >nul
)
echo Warning: port 3080 did not answer in time, opening the browser anyway.

:open
if "%1"=="test" goto :eof
start "" "%URL%"
endlocal