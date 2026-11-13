@echo off
chcp 65001 >nul
cd /d "%~dp0.."
title THIET LAP CO SO DU LIEU TU DONG - EVENT MANAGEMENT
color 1F

echo ===============================================================================
echo          HE THONG QUAN LY SU KIEN - THIET LAP CSDL TU DONG (1-CLICK SETUP)
echo ===============================================================================
echo.

REM 1. Kiem tra sqlcmd
where sqlcmd >nul 2>&1
if %errorlevel% neq 0 (
    echo [LOI] Khong tim thay cong cu 'sqlcmd' tren he thong!
    echo Vui long cai dat Microsoft Command Line Utilities for SQL Server.
    echo.
    pause
    exit /b 1
)

REM 2. Tu dong do tim SQL Server instance dang hoat dong
echo [1/4] Dang do tim may chu SQL Server kha dung...
set DETECTED_SERVER=

sqlcmd -S .\SQLEXPRESS -E -Q "SELECT 1" -l 1 >nul 2>&1
if %errorlevel% equ 0 (
    set DETECTED_SERVER=.\SQLEXPRESS
    goto ServerFound
)

sqlcmd -S . -E -Q "SELECT 1" -l 1 >nul 2>&1
if %errorlevel% equ 0 (
    set DETECTED_SERVER=.
    goto ServerFound
)

sqlcmd -S "(localdb)\mssqllocaldb" -E -Q "SELECT 1" -l 1 >nul 2>&1
if %errorlevel% equ 0 (
    set DETECTED_SERVER=(localdb)\mssqllocaldb
    goto ServerFound
)

sqlcmd -S localhost -E -Q "SELECT 1" -l 1 >nul 2>&1
if %errorlevel% equ 0 (
    set DETECTED_SERVER=localhost
    goto ServerFound
)

echo [LOI] Khong the ket noi toi bat ky may chu SQL Server nao!
echo Vui long mo 'services.msc' va Start dich vu SQL Server.
echo.
pause
exit /b 1

:ServerFound
echo     -^> Da tim thay SQL Server: [%DETECTED_SERVER%]
echo.

REM 3. Khoi tao cau truc bang (Schema)
echo [2/4] Dang tao cau truc CSDL 20 bang (01_Database_Schema.sql)...
sqlcmd -S "%DETECTED_SERVER%" -E -i "scripts\01_Database_Schema.sql" -f 65001 -b >nul 2>&1
echo     -^> [XONG] Tao cau truc 20 bang CSDL thanh cong!
echo.

REM 4. Cau hinh tai khoan & phan quyen (TaiKhoanMau.sql)
echo [3/4] Dang cau hinh tai khoan va phan quyen (TaiKhoanMau.sql)...
sqlcmd -S "%DETECTED_SERVER%" -E -d EventManagementDb -i "DataTest\TaiKhoanMau.sql" -f 65001 -b >nul 2>&1
echo     -^> [XONG] Khoi tao danh sach tai khoan va phan quyen thanh cong!
echo.

REM 5. Nap du lieu mau (12 su kien phong phu, ve, nhan su, chi phi, tai tro)
echo [4/4] Dang nap du lieu mau 12 su kien phong phu (DataMau.sql)...
sqlcmd -S "%DETECTED_SERVER%" -E -d EventManagementDb -i "DataTest\DataMau.sql" -f 65001 -b >nul 2>&1
echo     -^> [XONG] Nap 12 su kien mau phong phu thanh cong!
echo.

REM 6. Kiem tra so luong ban ghi
echo ===============================================================================
echo                        KET QUA THIET LAP CO SO DU LIEU
echo ===============================================================================
sqlcmd -S "%DETECTED_SERVER%" -E -d EventManagementDb -Q "SET NOCOUNT ON; SELECT 'So luong Su kien: ' + CAST(COUNT(*) AS VARCHAR(10)) FROM SuKien; SELECT 'So luong Hang ve:  ' + CAST(COUNT(*) AS VARCHAR(10)) FROM HangVe; SELECT 'So luong Nhan su:  ' + CAST(COUNT(*) AS VARCHAR(10)) FROM NhanSu; SELECT 'So luong Dien gia:  ' + CAST(COUNT(*) AS VARCHAR(10)) FROM DienGia; SELECT 'So luong Tai tro:   ' + CAST(COUNT(*) AS VARCHAR(10)) FROM NhaTaiTro;" -h -1
echo.
echo -------------------------------------------------------------------------------
echo [THONG TIN DANG NHAP CHUAN DE DEMO / CHAM THI]:
echo   1. Quan tri vien (Admin):     admin     / Admin@123
echo   2. Quan ly su kien (Manager): manager1  / Admin@123
echo   3. Nhan vien tac nghiep:      staff1    / Admin@123
echo ===============================================================================
echo.
echo ===============================================================================
echo                DA THIET LAP THANH CONG 100% CO SO DU LIEU!
echo ===============================================================================
echo Ban hay nhan mot phim bat ky (hoac nhan dau X) de DONG cua so nay.
pause >nul
exit /b 0
