@echo off
setlocal EnableExtensions
cd /d "%~dp0"

set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
if not exist "%CSC%" (
    echo [ERROR] .NET Framework compiler ^(csc.exe^) not found.
    exit /b 1
)

"%CSC%" /nologo /target:exe /optimize+ /win32icon:..\DeepSeekHarness-WhaleGirl.ico /resource:launcher-template.cmd,LauncherCmd /resource:..\DeepSeekHarness-WhaleGirl.ico,WhaleIcon /out:..\DeepSeekHarness-Launcher-Setup.exe /r:System.dll /r:System.Core.dll /r:Microsoft.CSharp.dll Setup.cs

if errorlevel 1 (
    echo [ERROR] BUILD FAILED
    exit /b 1
)
echo [OK] Built: ..\DeepSeekHarness-Launcher-Setup.exe
endlocal