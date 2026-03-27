@echo off
:: Garante que o terminal execute na pasta StreamingAssets
cd /d "%~dp0"

:: Inicia o ambiente Bash do BOSS usando o BusyBox
busybox.exe sh script_boss.sh

:: Repassa o código de saída para a Unity (Plano B)
exit /b %errorlevel%
