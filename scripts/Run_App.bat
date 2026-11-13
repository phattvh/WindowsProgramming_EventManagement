@echo off
chcp 65001 >nul
cd /d "%~dp0.."
title KHOI CHAY HE THONG QUAN LY SU KIEN - EVENT MANAGEMENT
color 1F

echo ===============================================================================
echo                KHOI CHAY UNG DUNG EVENT MANAGEMENT (1-CLICK RUN)
echo ===============================================================================
echo.

set "EXE_PATH=EventManagement.GUI\bin\Debug\net8.0-windows\EventManagement.GUI.exe"

if exist "%EXE_PATH%" (
    echo Dang mo ung dung truc tiep tu ban build san (khoi dong tuc thi duoi 0.5s)...
    start "" "%EXE_PATH%"
    exit /b 0
)

echo Chua tim thay ban build san, dang tien hanh build va khoi chay qua .NET CLI...
dotnet run --project EventManagement.GUI
