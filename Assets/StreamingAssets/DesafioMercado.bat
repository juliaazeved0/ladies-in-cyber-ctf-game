@echo off
cd /d "%~dp0"
:: Chama o busybox para rodar o script do Mercado Escondido
busybox.exe sh script_mercado.sh
exit /b %errorlevel%