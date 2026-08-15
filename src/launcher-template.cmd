@echo off
setlocal EnableExtensions
title DeepSeek Harness Launcher

REM ============================================================
REM  DeepSeek Harness Launcher
REM  Auto-detects Node.js and DeepSeek Harness, starts the web
REM  server (if not already running) and opens the browser UI.
REM ============================================================

set "URL=http://127.0.0.1:3080"

REM ---------- locate node.exe ----------
set "NODE="
where node >nul 2>&1
if not errorlevel 1 (
    for /f "delims=" %%P in ('where node') do if not defined NODE set "NODE=%%P"
)
if not defined NODE if exist "%ProgramFiles%\nodejs\node.exe" set "NODE=%ProgramFiles%\nodejs\node.exe"
if not defined NODE if exist "%ProgramFiles(x86)%\nodejs\node.exe" set "NODE=%ProgramFiles(x86)%\nodejs\node.exe"
if not defined NODE (
    echo [ERROR] Node.js was not found.
    echo Please install Node.js from https://nodejs.org and try again.
    pause
    exit /b 1
)

REM ---------- locate DeepSeek Harness (npx cache, newest first) ----------
set "BIN="
for /f "delims=" %%D in ('dir /b /ad /o-d "%LOCALAPPDATA%\npm-cache\_npx" 2^>nul') do (
    if not defined BIN if exist "%LOCALAPPDATA%\npm-cache\_npx\%%D\node_modules\@deepseek-ai\dsh\lib\bin.js" set "BIN=%LOCALAPPDATA%\npm-cache\_npx\%%D\node_modules\@deepseek-ai\dsh\lib\bin.js"
)
if not defined BIN if exist "%APPDATA%\npm\node_modules\@deepseek-ai\dsh\lib\bin.js" set "BIN=%APPDATA%\npm\node_modules\@deepseek-ai\dsh\lib\bin.js"
if not defined BIN (
    echo [ERROR] DeepSeek Harness was not found.
    echo Install it once with:  npx @deepseek-ai/dsh web
    echo then run this launcher again.
    pause
    exit /b 1
)

REM ---------- already running? just open the UI ----------
netstat -ano | findstr ":3080" | findstr "LISTENING" >nul 2>&1
if not errorlevel 1 goto open

REM ---------- start the server ----------
echo Starting DeepSeek Harness server...
start "DeepSeek Harness Server - close this window to stop" "%NODE%" "%BIN%" web

REM ---------- wait for the port (up to 30 seconds) ----------
for /l %%i in (1,1,30) do (
    netstat -ano | findstr ":3080" | findstr "LISTENING" >nul 2>&1 && goto open
    ping -n 2 127.0.0.1 >nul
)
echo Warning: port 3080 did not answer in time, opening the browser anyway.

:open
start "" "%URL%"
endlocal
