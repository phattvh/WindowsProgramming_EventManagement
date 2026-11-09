@echo off
chcp 65001 >nul
net session >nul 2>&1
if %errorLevel% == 0 (
    echo ====================================================
    echo [1/2] Cau hinh tai khoan dich vu SQL Server ve LocalSystem...
    echo ====================================================
    sc.exe config MSSQLSERVER obj= "LocalSystem"
    echo.
    echo ====================================================
    echo [2/2] Khoi dong Microsoft SQL Server (MSSQLSERVER)...
    echo ====================================================
    net start MSSQLSERVER
    echo.
    echo ====================================================
    echo SQL Server da khoi dong thanh cong! Ban co the chay ung dung.
    echo ====================================================
    pause
) else (
    echo Dang yeu cau quyen Administrator...
    powershell -Command "Start-Process '%~dpnx0' -Verb RunAs"
)
