# ====================================================================
# SCRIPT CHẠY KIỂM THỬ TỰ ĐỘNG CSDL NGÀY 1 (THÀNH VIÊN 1)
# ====================================================================
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

$server = "."
$db = "EventManagementDB"
$testFilePath = Join-Path $PSScriptRoot "03_Test_Database.sql"

Write-Host "======================================================================" -ForegroundColor Cyan
Write-Host "     BẮT ĐẦU CHẠY KIỂM THỬ CSDL: $db" -ForegroundColor Yellow
Write-Host "======================================================================" -ForegroundColor Cyan

if (-not (Test-Path $testFilePath)) {
    Write-Host "[LỖI] Không tìm thấy file test: $testFilePath" -ForegroundColor Red
    exit 1
}

$connStr = "Server=$server;Database=$db;Integrated Security=True;TrustServerCertificate=True"
try {
    $conn = [System.Data.SqlClient.SqlConnection]::new($connStr)
    $conn.Open()
} catch {
    Write-Host "[LỖI] Không thể kết nối đến SQL Server ($server): $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

$testContent = Get-Content $testFilePath -Raw -Encoding utf8
$batches = $testContent -split "(?m)^\s*GO\s*$"

$passCount = 0
$failCount = 0
$warnCount = 0

foreach ($batch in $batches) {
    $sql = $batch.Trim()
    if ($sql.Length -gt 0) {
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = $sql
        try {
            $reader = $cmd.ExecuteReader()
            while ($reader.Read()) {
                if ($reader.GetName(0) -eq "Mã Test") {
                    $id = $reader.GetValue(0)
                    $group = $reader.GetValue(1)
                    $name = $reader.GetValue(2)
                    $result = $reader.GetValue(3)
                    $detail = $reader.GetValue(4)

                    if ($result -eq "PASS") {
                        $passCount++
                        Write-Host "  [$result] $id - $name" -ForegroundColor Green
                    } elseif ($result -eq "FAIL") {
                        $failCount++
                        Write-Host "  [$result] $id - $name ($detail)" -ForegroundColor Red
                    } else {
                        $warnCount++
                        Write-Host "  [$result] $id - $name ($detail)" -ForegroundColor Yellow
                    }
                }
            }
            $reader.Close()
        } catch {
            Write-Host "  [LỖI THỰC THI SQL]: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}
$conn.Close()

$total = $passCount + $failCount + $warnCount
$summaryColor = if ($failCount -eq 0) { "Green" } else { "Red" }

Write-Host "----------------------------------------------------------------------" -ForegroundColor Cyan
Write-Host "TỔNG KẾT: $passCount PASS / $total TESTS (Thất bại: $failCount, Cảnh báo: $warnCount)" -ForegroundColor $summaryColor

if ($failCount -eq 0 -and $total -gt 0) {
    Write-Host ">> KẾT LUẬN: ĐẠT 100% TIÊU CHUẨN NGHIỆM THU NGÀY 1 CỦA THÀNH VIÊN 1!" -ForegroundColor Green
} else {
    Write-Host ">> KẾT LUẬN: CHƯA ĐẠT! Vui lòng kiểm tra lại các ca kiểm thử thất bại." -ForegroundColor Red
}
Write-Host "======================================================================" -ForegroundColor Cyan


