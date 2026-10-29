# Tài liệu Đặc tả Chức năng - Hệ thống Quản lý Sự kiện

## 1. Tổng quan chức năng

Phần mềm Quản lý sự kiện hỗ trợ ban quản trị và nhân viên tối ưu hóa toàn bộ vòng đời tổ chức sự kiện: từ khởi tạo, chuẩn bị địa điểm, lịch trình, diễn giả, khách mời, vé, nhân sự, tài trợ, chi phí cho đến check-in, thu thập phản hồi và lập báo cáo thống kê. Hệ thống bao gồm 20 Form chức năng được phân loại thành 6 nhóm nghiệp vụ:

| Nhóm nghiệp vụ | Danh sách Form |
| :--- | :--- |
| **Quản lý hệ thống** | F01 (Đăng nhập), F02 (Dashboard/Trang chủ), F03 (Quản lý người dùng), F20 (Cấu hình hệ thống) |
| **Quản lý danh mục** | F06 (Loại sự kiện), F07 (Địa điểm), F09 (Diễn giả/Khách mời), F14 (Nhân sự), F16 (Nhà tài trợ) |
| **Quản lý sự kiện** | F04 (Quản lý sự kiện), F05 (Chi tiết sự kiện), F08 (Lịch/Chương trình) |
| **Quản lý người tham gia** | F10 (Người tham gia), F11 (Đăng ký tham gia), F12 (Quản lý vé), F13 (Check-in), F18 (Phản hồi) |
| **Nhân sự & Tài chính** | F15 (Phân công nhân sự), F17 (Ngân sách & Chi phí) |
| **Báo cáo & Thống kê** | F19 (Báo cáo & Thống kê) |

---

## 2. Chi tiết chức năng

### F01 – Đăng nhập hệ thống
- **Mục đích:** Xác thực danh tính và phân quyền truy cập hệ thống theo tài khoản nội bộ.
- **Quy trình & Nghiệp vụ:** Người dùng nhập Tên đăng nhập và Mật khẩu → Hệ thống kiểm tra tài khoản (tồn tại, đúng mật khẩu, trạng thái đang hoạt động và không bị khóa) → Điều hướng vào F02 (Trang chủ) theo vai trò (Admin: toàn quyền; Manager: quản lý sự kiện và tài nguyên; Staff: thực hiện nghiệp vụ được chỉ định). Nếu thất bại, hiển thị cảnh báo lỗi chi tiết.
- **Kiểm tra dữ liệu:** Bắt buộc nhập đầy đủ tài khoản và mật khẩu; chặn đăng nhập nếu tài khoản bị khóa; ngăn chặn nhân viên truy cập trái phép các Form ngoài thẩm quyền.

### F02 – Trang chủ / Dashboard
- **Mục đích:** Tổng quan hóa trạng thái vận hành của toàn bộ hệ thống bằng các chỉ số trực quan ngay sau khi đăng nhập.
- **Chức năng chính:**
  - **Số liệu tổng quan:** Tổng số sự kiện (phân loại: đang chuẩn bị, sắp diễn ra, đã hoàn thành), tổng số người tham gia, số vé đã bán, tổng doanh thu vé, tổng chi phí thực tế, số nhà tài trợ.
  - **Sự kiện sắp diễn ra:** Bảng danh sách gồm Tên sự kiện, ngày bắt đầu/kết thúc, địa điểm, số người đăng ký, số người đã check-in, trạng thái.
  - **Biểu đồ trực quan:** Số lượng sự kiện/người tham gia theo tháng, doanh thu vs chi phí theo sự kiện, tỷ lệ check-in thực tế.
  - **Lối tắt (Quick Access):** Các nút bấm mở nhanh: Tạo sự kiện (F04), Đăng ký (F11), Check-in (F13), Vé (F12), Báo cáo (F19).

### F03 – Quản lý người dùng (Bảng Users)
- **Mục đích:** Quản trị danh sách tài khoản được cấp phép sử dụng phần mềm.
- **Thông tin quản lý:** Mã người dùng, Tên đăng nhập, Mật khẩu (mã hóa), Họ tên, Email, Số điện thoại, Vai trò (Admin/Manager/Staff), Trạng thái (Hoạt động/Khóa).
- **Nghiệp vụ:** Thêm/sửa thông tin tài khoản; khóa/mở khóa tài khoản (ngăn chặn đăng nhập tức thì); tìm kiếm nhanh theo tài khoản, tên, email, vai trò. Riêng thao tác xóa: chỉ Admin mới có quyền và hệ thống tuyệt đối không cho phép xóa tài khoản Admin cuối cùng.

### F04 – Quản lý sự kiện
- **Mục đích:** Quản lý thông tin tổng thể của các sự kiện trong hệ thống.
- **Thông tin quản lý:** Mã sự kiện, Tên sự kiện, Loại sự kiện, Mô tả, Ngày bắt đầu, Ngày kết thúc, Địa điểm, Sức chứa, Ngân sách dự kiến, Người phụ trách, Trạng thái (Chuẩn bị, Sắp diễn ra, Đang diễn ra, Đã hoàn thành, Đã hủy).
- **Nghiệp vụ:** Thêm, sửa, tìm kiếm đa điều kiện, lọc theo trạng thái, xem chi tiết ở F05.
- **Ràng buộc:** Chỉ cho phép xóa sự kiện khi chưa phát sinh bất kỳ dữ liệu liên kết nào (chưa có người đăng ký, vé bán, check-in, chi phí, phản hồi). Nếu đã phát sinh dữ liệu, hệ thống chỉ cho phép chuyển trạng thái sang Đã hủy.

### F05 – Chi tiết sự kiện
- **Mục đích:** Trung tâm điều phối dữ liệu tập trung cho từng sự kiện cụ thể.
- **Bố cục hiển thị:** Phần đầu hiển thị thông tin chung và khối KPI thống kê (Tổng vé bán, tỷ lệ check-in, tổng doanh thu, tổng chi, số diễn giả/nhân sự/nhà tài trợ).
- **Hệ thống 9 Tab chức năng:**
  - **Tab 1 (Thông tin):** Xem và cập nhật thông tin sự kiện cơ bản.
  - **Tab 2 (Lịch chương trình):** Danh sách lịch trình hoạt động chi tiết.
  - **Tab 3 (Diễn giả):** Danh sách diễn giả và khách mời tham gia.
  - **Tab 4 (Người tham gia):** Danh sách hồ sơ đăng ký tham dự.
  - **Tab 5 (Vé):** Quản lý các hạng vé và tình trạng phát hành.
  - **Tab 6 (Nhân sự):** Danh sách nhân sự được phân công tổ chức.
  - **Tab 7 (Nhà tài trợ):** Các đơn vị tài trợ và gói tài trợ.
  - **Tab 8 (Chi phí):** Chi tiết các khoản chi và hóa đơn phát sinh.
  - **Tab 9 (Phản hồi):** Tổng hợp đánh giá và ý kiến đóng góp của người tham gia.

### F06 – Quản lý loại sự kiện
- **Mục đích:** Chuẩn hóa các danh mục hình thức sự kiện (Hội thảo, Workshop, Hội nghị, Thể thao, Giải trí, Hoạt động sinh viên...).
- **Thông tin & Nghiệp vụ:** Quản lý Mã loại, Tên loại, Mô tả. Cho phép Thêm, Sửa, Tìm kiếm.
- **Ràng buộc:** Không cho phép xóa loại sự kiện nếu đã có sự kiện đang áp dụng loại này.

### F07 – Quản lý địa điểm
- **Mục đích:** Quản lý thông tin cơ sở hạ tầng tổ chức sự kiện.
- **Thông tin quản lý:** Mã địa điểm, Tên địa điểm, Địa chỉ, Sức chứa tối đa, Người quản lý cơ sở, Số điện thoại liên hệ, Trạng thái.
- **Kiểm tra trùng lịch:** Khi gán địa điểm cho sự kiện, hệ thống tự động kiểm tra giao thoa thời gian (Start_Time đến End_Time). Nếu địa điểm đã được xếp lịch cho sự kiện khác trong cùng khoảng thời gian, hệ thống sẽ chặn thao tác và đưa ra cảnh báo xung đột lịch.

### F08 – Quản lý lịch / chương trình
- **Mục đích:** Thiết lập timeline chi tiết các hoạt động diễn ra xuyên suốt sự kiện.
- **Thông tin quản lý:** Mã lịch, Mã sự kiện, Tên chương trình/tiết mục, Thời gian bắt đầu, Thời gian kết thúc, Mô tả, Diễn giả phụ trách, Địa điểm/phòng chức năng.
- **Ràng buộc:** Thời gian kết thúc phải lớn hơn thời gian bắt đầu; khung giờ chương trình con phải nằm hoàn toàn trong thời gian diễn ra sự kiện; không xếp 2 chương trình trùng giờ tại cùng 1 phòng; không phân công 1 diễn giả cho 2 hoạt động diễn ra cùng thời điểm.

### F09 – Quản lý diễn giả / khách mời
- **Mục đích:** Lưu trữ hồ sơ các nhân vật chủ chốt tham gia chia sẻ hoặc giao lưu.
- **Thông tin & Chức năng:** Mã diễn giả, Họ tên, Chức vụ, Đơn vị/Tổ chức, Email, Số điện thoại, Tiểu sử tóm tắt, Hình ảnh chân dung, Trạng thái. Hỗ trợ Thêm, Sửa, Xóa, Tra cứu và liên kết nhanh vào lịch trình sự kiện (F08).

### F10 – Quản lý người tham gia
- **Mục đích:** Quản lý cơ sở dữ liệu hồ sơ cá nhân của người tham dự sự kiện.
- **Thông tin quản lý:** Mã người tham gia, Họ tên, Giới tính, Ngày sinh, Số điện thoại, Email, Đơn vị/Trường học/Doanh nghiệp, Địa chỉ, Trạng thái.
- **Chức năng:** CRUD, tra cứu đa tiêu chí và xem Lịch sử tham gia (chi tiết các sự kiện từng đăng ký, loại vé, ngày mua, tình trạng thanh toán và lịch sử check-in).

### F11 – Đăng ký tham gia sự kiện
- **Mục đích:** Thực hiện nghiệp vụ ghi danh và bán vé cho người tham dự.
- **Luồng xử lý:** Chọn sự kiện → Chọn người tham gia → Chọn loại vé → Nhập số lượng → Thẩm định điều kiện → Tính tổng tiền (Đơn giá × Số lượng) → Lưu đăng ký → Cập nhật giảm số lượng vé khả dụng và cấp mã vé/QR check-in.
- **Ràng buộc:** Sự kiện phải còn hiệu lực (chưa kết thúc/chưa hủy); người tham gia phải có thông tin hợp lệ; số lượng vé mua không vượt quá số vé còn lại; chặn đăng ký trùng lặp nếu hệ thống cấu hình mỗi cá nhân chỉ được đăng ký một vé duy nhất cho mỗi sự kiện.

### F12 – Quản lý vé
- **Mục đích:** Quản trị các hạng vé (Standard, VIP,...) và số lượng phát hành theo sự kiện.
- **Thông tin & Công thức:** Mã vé, Sự kiện, Tên loại vé, Đơn giá, Số lượng phát hành, Số lượng đã bán, Số lượng còn lại = Phát hành - Đã bán, Thời gian mở bán/đóng bán, Trạng thái.
- **Ràng buộc:** Hệ thống tự động khóa bán khi hết vé hoặc quá hạn; không cho phép sửa giảm số lượng phát hành nhỏ hơn số vé đã bán.

### F13 – Check-in sự kiện
- **Mục đích:** Điểm danh và kiểm soát lối vào sự kiện theo thời gian thực.
- **Phương thức:** Nhập mã vé, mã đăng ký hoặc quét camera mã QR.
- **Quy trình thẩm định:** Nhập/quét mã → Tìm thông tin vé → Kiểm tra tính hợp lệ (đúng sự kiện, trạng thái đã thanh toán, chưa bị hủy) → Kiểm tra trùng lặp (chặn nếu mã vé đã được check-in trước đó) → Ghi nhận Thời gian check-in thành công và cập nhật thống kê tức thì (Tổng đăng ký, Đã đến, Chưa đến, Tỷ lệ %).

### F14 – Quản lý nhân sự (Bảng EventStaffs)
- **Mục đích:** Quản lý đội ngũ nhân viên và tình nguyện viên phục vụ công tác tổ chức.
- **Thông tin & Nghiệp vụ:** Mã nhân sự, Họ tên, Số điện thoại, Email, Chức danh, Phòng ban/Bộ phận, Trạng thái làm việc. Cung cấp đầy đủ các thao tác Thêm, Sửa, Xóa, Tra cứu và Lọc theo bộ phận.

### F15 – Phân công nhân sự
- **Mục đích:** Điều phối nhân lực vào từng sự kiện cụ thể theo vai trò và ca trực.
- **Nghiệp vụ:** Chọn Sự kiện → Chọn Nhân sự → Gán Vai trò nhiệm vụ (Lễ tân, Kỹ thuật, Hậu cần...) → Thiết lập Ca làm việc (Giờ bắt đầu - Giờ kết thúc) → Ghi chú.
- **Ràng buộc:** Ngăn chặn phân công 1 nhân sự vào 2 ca làm việc có thời gian chồng chéo nhau; không phân công nhân sự đang ở trạng thái ngưng hoạt động; không phân công vào sự kiện đã kết thúc; Lấy dữ liệu từ F14 - Quản lý nhân sự.

### F16 – Quản lý nhà tài trợ
- **Mục đích:** Quản trị hồ sơ các doanh nghiệp, tổ chức và cá nhân tài trợ kinh phí/hiện vật.
- **Thông tin quản lý:** Mã nhà tài trợ, Tên tổ chức, Người đại diện, Email, Số điện thoại, Địa chỉ, Ghi chú, Lịch sử tài trợ.
- **Gói tài trợ:** Hỗ trợ cấu hình các mức hạng tài trợ (Bronze, Silver, Gold, Platinum...) tương ứng với giá trị đóng góp và quyền lợi truyền thông kèm theo.

### F17 – Quản lý ngân sách / chi phí
- **Mục đích:** Kiểm soát dòng tiền, kế hoạch ngân sách và các khoản chi thực tế của sự kiện.
- **Thông tin quản lý:** Mã chi phí, Mã sự kiện, Khoản mục chi (Địa điểm, Âm thanh/Ánh sáng, Ăn uống, Marketing, Thiết bị, Nhân công...), Diễn giải nội dung, Số tiền, Ngày chi, Người lập phiếu, Trạng thái, Chứng từ đính kèm.
- **Tính toán & Cảnh báo:**
  - **Công thức tính toán:**
    - Ngân sách còn lại = Ngân sách dự kiến - Tổng chi phí thực tế
    - Tỷ lệ sử dụng = (Tổng chi phí thực tế / Ngân sách dự kiến) × 100%
  - Hệ thống kích hoạt cảnh báo đổi màu giao diện khi tỷ lệ sử dụng vượt ngưỡng an toàn (ví dụ: cảnh báo khi chi quá 80%); không cho phép nhập số tiền chi nhỏ hơn hoặc bằng 0.

### F18 – Quản lý phản hồi
- **Mục đích:** Tiếp nhận và phân tích đánh giá từ người tham gia sau khi sự kiện kết thúc.
- **Nội dung & Tiêu chí:** Lưu thông tin Người tham gia, Sự kiện, Nội dung nhận xét, Thời gian gửi. Đánh giá theo thang điểm từ 1 đến 5 sao trên các phương diện: Nội dung chương trình, Chất lượng diễn giả, Cơ sở vật chất/Địa điểm, Công tác tổ chức và Dịch vụ hỗ trợ.
- **Ràng buộc & Thống kê:** Chỉ người đã check-in tham gia mới được gửi phản hồi; giới hạn 1 phản hồi/người/sự kiện; hệ thống tự động tính điểm đánh giá trung bình và phân bổ tỷ lệ các mức sao (1★ - 5★).

### F19 – Báo cáo & Thống kê
- **Mục đích:** Tổng hợp, phân tích dữ liệu và cung cấp các báo cáo in ấn phục vụ công tác quản trị, đánh giá hiệu quả sự kiện và lưu trữ hồ sơ.
- **Nội dung và tiêu chí:**
  - **Báo cáo tài chính:** Thống kê Doanh thu bán vé, Tổng chi phí thực tế và Lợi nhuận theo từng sự kiện hoặc theo khoảng thời gian.
  - **Báo cáo tham dự & vé:** Thống kê tổng số vé phát hành, số vé đã bán, số người đã check-in, tỷ lệ tham dự thực tế.
  - **Báo cáo đánh giá:** Tổng hợp điểm trung bình (1-5 sao) và phân bổ tỷ lệ các mức đánh giá cho từng sự kiện.
  - **Trực quan hóa:** Hiển thị biểu đồ Cột/Đường/Tròn thể hiện xu hướng doanh thu theo tháng, cơ cấu doanh thu theo hạng vé.
- **Ràng buộc và Tiện ích in ấn:** Cho phép lọc đa điều kiện như Thời gian, Loại sự kiện, Địa điểm, Trạng thái; Tích hợp công nghệ RDLC Report để thiết kế mẫu in ấn chuẩn form A4; Hỗ trợ xuất báo cáo ra định dạng PDF, Excel (.xlsx) hoặc in trực tiếp qua máy in.

### F20 – Quản lý nhóm quyền & Cấu hình hệ thống
- **Mục đích:** Quản trị danh mục vai trò và thiết lập phân quyền chức năng động cho từng nhóm người dùng.
- **Nội dung và tiêu chí:** Quản lý danh mục nhóm quyền (Admin, Manager, Staff...) và bảng chi tiết chức năng tương ứng trên từng Form. Cho phép Admin tạo nhóm quyền mới, gán/hủy quyền truy cập trên từng Form/nút bấm, lưu ma trận phân quyền vào bảng PhanQuyen trong CSDL.
- **Ràng buộc và thống kê:** Nhóm quyền Admin mặc định luôn có toàn quyền và không thể bị thu hồi quyền cốt lõi; mọi thay đổi phân quyền có hiệu lực ngay trong phiên làm việc tiếp theo của người dùng.

---

## 3. Quy chuẩn chức năng dùng chung & Kiểm tra dữ liệu

### Chức năng Tìm kiếm, Lọc và Tiện ích dùng chung
- **Tìm kiếm:** Tìm kiếm nhanh đa trường không phân biệt hoa thường (Mã, Tên, Số điện thoại, Email) ngay trên thanh công cụ.
- **Bộ lọc:** Lọc kết hợp theo Trạng thái, Khoảng thời gian, Loại danh mục, Địa điểm, Phụ trách.
- **Làm mới:** Xóa trắng các tiêu chí lọc, tải lại dữ liệu mới nhất từ CSDL về DataGridView.

### Quy chuẩn kiểm tra dữ liệu (Validation Rules)
- **Bắt buộc nhập:** Không để trống các trường khóa chính, tên, số điện thoại, ngày giờ, số lượng.
- **Kiểm tra kiểu số & giá trị:** Đơn giá vé, số lượng phát hành, sức chứa, ngân sách, số tiền chi tiêu phải là số nguyên/số thực dương (≥ 0).
- **Kiểm tra logic thời gian:** Ngày kết thúc ≥ Ngày bắt đầu; thời gian diễn ra chương trình con phải nằm trong khoảng thời gian diễn ra sự kiện; không xếp lịch chồng chéo cho cùng một địa điểm hoặc một nhân sự/diễn giả.
- **Kiểm tra định dạng:** Email phải đúng định dạng chuẩn (`name@domain.com`); Số điện thoại đúng chuẩn từ 10 chữ số.
- **Tính duy nhất:** Tuyệt đối không trùng lặp Tên đăng nhập, Mã sự kiện, Mã vé, Mã đăng ký.

---

## 4. Ma trận phân quyền chi tiết (RBAC)

| Nhóm nghiệp vụ | Tên Form / Chức năng | Admin | Manager | Staff |
| :--- | :--- | :---: | :---: | :---: |
| **Quản lý hệ thống** | F01 – Đăng nhập hệ thống | ✓ | ✓ | ✓ |
| | F02 – Trang chủ / Dashboard | ✓ | ✓ | ✓ |
| | F03 – Quản lý người dùng | ✓ | ✓ (Users) | ✗ |
| | F20 – Quản lý nhóm quyền | ✓ | ✓ (Tùy) | ✗ |
| **Quản lý danh mục** | F06 – Quản lý loại sự kiện | ✓ | ✓ | Chỉ xem |
| | F07 – Quản lý địa điểm | ✓ | ✓ | Chỉ xem |
| | F09 – Quản lý diễn giả / khách mời | ✓ | ✓ | Chỉ xem |
| | F14 – Quản lý nhân sự | ✓ | ✓ | Chỉ xem |
| | F16 – Quản lý nhà tài trợ | ✓ | ✓ | Chỉ xem |
| **Quản lý sự kiện** | F04 – Quản lý sự kiện | ✓ | ✓ | Chỉ xem |
| | F05 – Chi tiết sự kiện | ✓ | ✓ | Chỉ xem |
| | F08 – Quản lý lịch / chương trình | ✓ | ✓ | Chỉ xem |
| **Quản lý người tham gia** | F10 – Quản lý người tham gia | ✓ | ✓ | ✓ |
| | F11 – Đăng ký tham gia | ✓ | ✓ | ✓ |
| | F12 – Quản lý vé | ✓ | ✓ | Chỉ xem |
| | F13 – Check-in sự kiện | ✓ | ✓ | ✓ |
| | F18 – Quản lý phản hồi | ✓ | ✓ | ✓ |
| **Nhân sự & Tài chính** | F15 – Phân công nhân sự | ✓ | ✓ | ✗ |
| | F17 – Quản lý ngân sách / chi phí | ✓ | ✓ | ✗ |
| **BC & TK** | F19 – Báo cáo & Thống kê | ✓ | ✓ | Chỉ xem |

---

## 5. Các chức năng tiện ích mở rộng tích hợp trên Form

- **Xử lý dữ liệu nâng cao:** Phân trang tự động trên DataGridView cho danh sách lớn; sắp xếp linh hoạt theo tiêu đề cột; lọc kết hợp nhiều tiêu chí cùng lúc.
- **Tự động hóa & Nhận diện:** Tạo tự động mã QR Code khi đăng ký vé thành công; tích hợp webcam quét QR Code trực tiếp trên Form Check-in (F13).
- **Cảnh báo nghiệp vụ thời gian thực:** Cảnh báo bằng thông báo nổi hoặc đổi màu khi sắp hết vé, khi địa điểm bị trùng lịch, khi chương trình con vượt khung thời gian hoặc khi ngân sách chi tiêu vượt quá 80%.
- **Báo cáo & Tương tác ngoại vi:**
  - Xuất dữ liệu trực tiếp từ lưới DataGridView ra định dạng Excel (.xlsx).
  - Kết xuất phiếu check-in hoặc báo cáo tài chính ra định dạng PDF.
  - Hỗ trợ gửi email xác nhận đăng ký thành công cho người tham dự.
- **Công nghệ & Thư viện đề xuất:**
  - **Tạo & Quét mã QR (F11, F13):** Dùng `ZXing.Net` hoặc `QRCoder`.
  - **Vẽ biểu đồ dashboard (F02):** Dùng `LiveCharts2` / `WinForms.DataVisualization.Charting`.
  - **Xuất Excel (F10, F12...):** Dùng `ClosedXML` hoặc `EPPlus`.
  - **Xuất PDF (Vé, Phiếu thu):** Dùng `iText7` hoặc export trực tiếp từ `RDLC Report`.
  - **Gửi Email tự động (F11):** Dùng `System.Net.Mail` (cấu hình SMTP Gmail / SendGrid).
