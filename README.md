# Event Management System (Hệ Thống Quản Lý Sự Kiện)

> **Đồ án môn học:** Lập trình Windows (.NET / C# WinForms)  
> **Nền tảng:** .NET 8.0 Windows Forms | C# 12 | Microsoft SQL Server  
> **Kiến trúc:** Phân tầng 3-Tier Layered Architecture (Core - DAL - BLL - GUI - Tests)

---

## 📖 Giới thiệu tổng quan (Overview)

**Event Management System** là phần mềm quản lý trọn gói toàn bộ vòng đời tổ chức sự kiện chuyên nghiệp: từ khâu hoạch định, thiết lập danh mục cơ sở vật chất, phân công nhân sự, quản lý bán vé, soát vé check-in thời gian thực bằng camera mã QR, quản lý thu chi - ngân sách, cho đến thống kê và kết xuất báo cáo quyết toán tài chính định dạng PDF/Excel.

Phần mềm được thiết kế theo tiêu chuẩn công nghiệp với giao diện Windows Forms hiện đại, tối ưu hóa trải nghiệm người dùng (UX), phân quyền bảo mật 3 cấp độ (Admin - Manager - Staff) và tuân thủ các quy tắc nghiệp vụ nghiêm ngặt.

---

## 🚀 Tính năng nổi bật (Key Features)

### 1. Quản trị hệ thống & Bảo mật (F01, F02, F03, UC21, AD-6)
- **Đăng nhập an toàn (F01):** Băm mật khẩu một chiều chuẩn BCrypt kèm muối sinh ngẫu nhiên (salt). Chống dò quét danh sách tài khoản (Username Enumeration).
- **Phòng thủ Brute-force tự động (BR-20, AD-7):** Tự động khóa tài khoản tạm thời 15 phút khi nhập sai mật khẩu quá 5 lần liên tiếp, hiển thị đồng hồ đếm ngược thời gian thực.
- **Trang chủ Dashboard thông minh (F02):** Khối 4 thẻ KPI động truy vấn trực tiếp từ CSDL, biểu đồ tròn LiveCharts thể hiện tỷ lệ Check-in thực tế, bảng sự kiện sắp xếp linh hoạt theo tiêu đề cột. Tự động ẩn thẻ tài chính đối với tài khoản Nhân viên (ST-11). Tự động đồng bộ số liệu realtime qua `DataSyncNotifier`.
- **Quản lý người dùng & Phân quyền RBAC (F03):** Cấp phát tài khoản theo 3 vai trò (`ADMIN`, `MANAGER`, `STAFF`). Cơ chế bảo vệ Admin cuối cùng (Last Admin Safeguard). Nút mở khóa tức thì cho tài khoản bị khóa do Brute-force.
- **Đổi mật khẩu (UC21):** Kiểm tra mật khẩu cũ, yêu cầu mật khẩu mới tối thiểu 6 ký tự và tự động mở khóa tài khoản.
- **Cấu hình CSDL động (AD-6):** Hộp thoại cấu hình chuỗi kết nối Microsoft SQL Server linh hoạt, kiểm tra kết nối an toàn (timeout 3 giây), chống văng ứng dụng (AD-42) và hỗ trợ khởi chạy độc lập qua tham số `--server-config`.

### 2. Quản lý sự kiện & Danh mục Master-Detail (F04 - F09, F14, F16)
- **Danh sách sự kiện (F04):** Tìm kiếm và lọc đa điều kiện theo trạng thái (Chuẩn bị, Sắp diễn ra, Đang diễn ra, Đã hoàn thành, Đã hủy). Chỉ cho phép xóa khi chưa có dữ liệu liên kết (BR-01).
- **Chi tiết sự kiện Master-Detail (F05):** Quản lý tập trung 5 phân hệ con trên cùng giao diện thông qua hệ thống Tab: Lịch trình, Hạng vé, Nhân sự, Chi phí và Nhà tài trợ.
- **Quản lý danh mục chuyên sâu:**
  - *Loại sự kiện (F06):* Chuẩn hóa loại hình tổ chức, kiểm tra khóa ngoại trước khi xóa (BR-02).
  - *Địa điểm (F07):* Quản lý sức chứa tối đa, thuật toán kiểm tra chống trùng lịch địa điểm (BR-03, BR-04).
  - *Lịch trình tiết mục (F08):* Kiểm soát khung giờ tiết mục nằm trọn trong sự kiện cha, chống trùng lặp giờ giữa các tiết mục (BR-05, BR-06).
  - *Diễn giả (F09):* Quản lý hồ sơ chuyên môn, ảnh đại diện, chống phân công trùng lịch diễn giả (BR-06, BR-07).
  - *Nhân sự (F14):* Quản lý hồ sơ cộng tác viên, kỹ thuật viên kèm chuyên môn và thông tin liên hệ chuẩn (BR-12).
  - *Nhà tài trợ (F16):* Quản lý các gói tài trợ (Kim cương, Vàng, Bạc, Đồng) và hạch toán dòng tiền (BR-16).

### 3. Vận hành Bán vé, Soát vé & Tài chính (F10 - F13, F15, F17, F19)
- **Quản lý người tham dự (F10):** Theo dõi hồ sơ khách mời và lịch sử tham gia các sự kiện (BR-08).
- **Bán vé & Ghi danh (F11):** Đặt mua từ 1 đến 10 vé/lần để chống đầu cơ giữ chỗ ảo (BR-10). Tự động sinh mã đơn hàng, tạo vé điện tử và cấp mã QR độc lập dạng GUID duy nhất cho từng vé.
- **Quản lý hạng vé (F12):** Thiết lập giá vé và số lượng phát hành. Đảm bảo tổng số vé không vượt quá sức chứa địa điểm tổ chức (BR-04) và không hạ phát hành thấp hơn số vé đã bán (BR-08).
- **Soát vé Check-in bằng Camera QR (F13):** Điểm danh đại biểu tốc độ cao qua camera (AForge / ZXing). Cơ chế soát vé 1 chiều chống gian lận (BR-11, BR-12): chặn đứng và báo động đỏ nếu mã vé bị quét lại lần thứ hai.
- **Phân công nhân sự (F15):** Điều phối ca trực, tích hợp thuật toán chống trùng ca trực giữa các sự kiện diễn ra cùng khung giờ (BR-13).
- **Ngân sách & Chi phí (F17):** Lập phiếu chi, tính tỷ lệ giải ngân lũy kế so với ngân sách. Hệ thống cảnh báo bội chi đa tầng (vùng an toàn < 80%, cảnh báo vàng 80% - 100%, báo động đỏ bội chi > 100%) (BR-14, BR-15).
- **Báo cáo & Thống kê (F19):** Tổng kết tài chính, vé bán và điểm danh. Hỗ trợ xuất dữ liệu ra bảng tính Excel (chuẩn mã UTF-8 BOM) và kết xuất tệp tài liệu PDF dạng vector chuẩn A4 sắc nét.

---

## 🛠️ Công nghệ sử dụng (Tech Stack)

| Thành phần | Công nghệ / Thư viện | Vai trò |
| :--- | :--- | :--- |
| **Framework** | .NET 8.0 (`net8.0-windows`) | Nền tảng thực thi ứng dụng Windows hiện đại |
| **Ngôn ngữ** | C# 12 | Lập trình hướng đối tượng, LINQ, Pattern Matching |
| **Giao diện (UI)** | Windows Forms (WinForms) | Xây dựng giao diện máy trạm đồ họa native |
| **Biểu đồ** | LiveChartsCore (SkiaSharpView) | Vẽ biểu đồ tỷ lệ Check-in trực quan, hiệu năng cao |
| **Thị giác máy tính** | AForge.Video, AForge.Video.DirectShow | Kết nối và thu hình ảnh trực tiếp từ webcam/camera |
| **Mã vạch / QR** | ZXing.Net | Tạo mã và giải mã QR Code tốc độ cao |
| **Bảo mật** | BCrypt.Net-Next | Thuật toán băm mật khẩu một chiều có muối bảo mật |
| **Cơ sở dữ liệu** | Microsoft SQL Server | Hệ quản trị cơ sở dữ liệu quan hệ (20 bảng chuẩn hóa) |
| **Kiểm thử** | MSTest, FluentAssertions | Kiểm thử đơn vị (Unit Test) và kiểm thử tích hợp |

---

## 🏗️ Cấu trúc Solution (Architecture)

Dự án được tổ chức theo mô hình phân lớp rõ ràng nhằm tối ưu hóa tính độc lập, khả năng mở rộng và bảo trì:

```
WindowsProgramming_EventManagement/
│
├── EventManagement.Core/          # Lớp lõi: Chứa Entities, DTOs, Enums, Interfaces chung
├── EventManagement.DAL/           # Tầng dữ liệu: AppDbContext, Repository, Connection Helper
├── EventManagement.BLL/           # Tầng nghiệp vụ: Xử lý logic, thẩm định Business Rules, ServiceResult
├── EventManagement.GUI/           # Tầng giao diện: Các Windows Form (F01 - F19), User Controls, RDLC
├── EventManagement.Tests/         # Tầng kiểm thử: Unit Tests cho Services, Forms, CSDL
│
├── DataTest/                      # Chứa kịch bản dữ liệu mẫu chuẩn (DataMau.sql)
├── Speakers/                      # Chứa ảnh đại diện của diễn giả
├── scripts/                       # Các kịch bản tự động hóa khởi tạo CSDL và khởi chạy nhanh
└── .release/                      # Thư mục chứa gói phát hành độc lập (.exe và .zip)
```

---

## 🔑 Tài khoản mặc định hệ thống (Default Accounts)

Hệ thống đã nạp sẵn 3 tài khoản chuẩn để thuận tiện cho việc kiểm thử và chấm điểm:

| Tên đăng nhập | Mật khẩu | Vai trò | Quyền hạn chính |
| :---: | :---: | :---: | :--- |
| **`admin`** | `Admin@123` | **Quản trị viên (Admin)** | Toàn quyền kiểm soát hệ thống, quản lý tài khoản, cấu hình CSDL, toàn quyền sự kiện và tài chính. |
| **`manager1`** | `Admin@123` | **Quản lý (Manager)** | Quản lý toàn diện sự kiện, lịch trình, bán vé, nhân sự, chi phí và báo cáo (không quản trị user). |
| **`staff1`** | `Admin@123` | **Nhân viên (Staff)** | Tra cứu người tham dự, bán vé, quét mã QR soát vé Check-in (ẩn thẻ tài chính và ngân sách). |

---

## 💻 Hướng dẫn cài đặt & Khởi chạy (Getting Started)

### 1. Yêu cầu môi trường
- Hệ điều hành: Windows 10 / Windows 11 (64-bit).
- Cơ sở dữ liệu: Microsoft SQL Server 2019 / 2022 hoặc SQL Server Express.
- Môi trường phát triển (nếu chạy từ mã nguồn): Visual Studio 2022 (với gói *.NET desktop development*) hoặc .NET 8 SDK.

### 2. Thiết lập cơ sở dữ liệu
1. Mở **SQL Server Management Studio (SSMS)** và kết nối tới instance SQL Server trên máy của bạn.
2. Mở và thực thi tuần tự các tệp kịch bản trong thư mục dự án:
   - `01_Database_Schema.sql` (Tạo cơ sở dữ liệu `QuanLySuKienDB` và 20 bảng quan hệ).
   - `02_SeedData.sql` (Tạo nhóm quyền, tài khoản quản trị mặc định và danh mục cơ sở).
   - `DataTest/DataMau.sql` (Nạp bộ dữ liệu mẫu phong phú gồm 12 sự kiện, vé, nhân sự, chi phí).
   *(Hoặc nhấp đúp vào script tự động `scripts/Setup_Database.bat` để hệ thống tự thiết lập).*

### 3. Khởi chạy ứng dụng

#### Cách 1: Chạy từ Visual Studio hoặc dòng lệnh (Dành cho nhà phát triển)
```powershell
# Chạy ứng dụng giao diện WinForms
dotnet run --project EventManagement.GUI/EventManagement.GUI.csproj

# Chạy kiểm thử tự động
dotnet test EventManagement.Tests/EventManagement.Tests.csproj
```

#### Cách 2: Chạy trực tiếp từ gói phát hành độc lập (.exe)
- Mở thư mục `.release/` và nhấp đúp vào file **`EventManagement.exe`**.
- File thực thi đã được đóng gói dạng **Self-contained Single-File** (~82 MB), có thể chạy độc lập ngay lập tức mà không cần cài đặt thêm .NET 8 Runtime trên máy.
- *Lưu ý cấu hình CSDL:* Nếu máy của bạn dùng tên SQL Server khác mặc định, trên màn hình đăng nhập hãy bấm vào biểu tượng Cấu hình (hoặc chạy lệnh `EventManagement.exe --server-config`) để nhập tên máy chủ và nhấn **[Lưu & Khởi động lại]**.

---

## 👥 Thành viên nhóm & Phân công nhiệm vụ (Team Members)

| Họ và tên | Mã số SV | Trách nhiệm chính trong đồ án |
| :--- | :---: | :--- |
| **Trần Vũ Hoà Phát** | 51.01.104.072 | Thiết kế kiến trúc giải pháp 3-Tier, thiết kế CSDL 20 bảng, tầng DAL (Generic Repository & Unit of Work), bảo mật BCrypt, kiểm soát Brute-force, Dashboard KPI & LiveCharts (F01 – F03, UC21, AD-6). |
| **Trần Trọng Hưng** | 51.01.104.132 | Chuẩn hóa giao diện người dùng UI/UX responsive, quản lý danh sách và chi tiết sự kiện Master-Detail, quản lý các danh mục địa điểm, loại sự kiện, lịch trình, diễn giả, nhà tài trợ (F04 – F09, F14, F16). |
| **Huỳnh Minh Trí** | 51.01.104.105 | Xây dựng thuật toán nghiệp vụ BLL, quy trình đặt mua vé, giải thuật giao dịch an toàn (ACID Transaction), soát vé Check-in bằng camera QR Code, quản lý ngân sách - chi phí và báo cáo thống kê PDF/Excel (F10 – F13, F15, F17, F19). |

---

## 📄 Bản quyền (License)

Dự án được phát triển phục vụ mục đích học tập và nghiên cứu trong khuôn khổ môn học Lập trình Windows. Bản quyền thuộc về Nhóm WINDOWN.
