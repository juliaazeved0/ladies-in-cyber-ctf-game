@echo off
cd /d "%~dp0"
busybox.exe sh script_h2.sh
exit /b %errorlevel%