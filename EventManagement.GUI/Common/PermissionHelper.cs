using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EventManagement.Core.Session;

namespace EventManagement.GUI.Common
{
    /// <summary>
    /// Tiện ích phân quyền giao diện động (AD-4 Lớp 1 / Task T1-17 - Ngày 8).
    /// Tự động ẩn/hiện hoặc vô hiệu hóa các nút bấm, menu, thanh tác vụ theo quyền hạn của CurrentSession.
    /// </summary>
    public static class PermissionHelper
    {
        public const string TagPrefixAuth = "AUTH:";
        public const string TagPrefixRole = "ROLE:";

        /// <summary>
        /// Kiểm tra xem một điều khiển có thỏa mãn điều kiện thẩm quyền hay không (không phụ thuộc vào phân cấp cha con).
        /// </summary>
        public static bool IsControlAuthorized(Control? control)
        {
            if (control == null || control.Tag == null) return true;
            return IsPermissionSatisfied(control.Tag.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Quét đệ quy toàn bộ cây điều khiển trong form/container và áp dụng phân quyền động.
        /// </summary>
        /// <param name="container">Form hoặc ContainerControl cần rà soát</param>
        /// <param name="hideInsteadOfDisable">True: Ẩn hoàn toàn (Visible = false). False: Làm mờ (Enabled = false)</param>
        public static void ApplyAuthorization(Control container, bool hideInsteadOfDisable = true)
        {
            if (container == null) return;

            // Rà soát chính container nếu có Tag phân quyền
            EvaluateControlPermission(container, hideInsteadOfDisable);

            // Rà soát các MenuStrip / ToolStrip đặc biệt
            if (container is ToolStrip toolStrip)
            {
                ApplyToolStripAuthorization(toolStrip, hideInsteadOfDisable);
            }

            // Duyệt các điều khiển con
            foreach (Control child in container.Controls)
            {
                if (child is ToolStrip childToolStrip)
                {
                    ApplyToolStripAuthorization(childToolStrip, hideInsteadOfDisable);
                }
                else
                {
                    EvaluateControlPermission(child, hideInsteadOfDisable);
                    if (child.HasChildren)
                    {
                        ApplyAuthorization(child, hideInsteadOfDisable);
                    }
                }
            }
        }

        /// <summary>
        /// Đánh giá quyền hạn cho một điều khiển đơn lẻ dựa trên thuộc tính Tag.
        /// </summary>
        public static bool EvaluateControlPermission(Control control, bool hideInsteadOfDisable = true)
        {
            if (control == null || control.Tag == null) return true;

            string tagStr = control.Tag.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(tagStr)) return true;

            bool isAllowed = IsPermissionSatisfied(tagStr);

            if (hideInsteadOfDisable)
            {
                control.Visible = isAllowed;
            }
            else
            {
                control.Enabled = isAllowed;
            }

            return isAllowed;
        }

        /// <summary>
        /// Rà soát phân quyền cho ToolStrip / MenuStrip / ContextMenuStrip.
        /// </summary>
        public static void ApplyToolStripAuthorization(ToolStrip toolStrip, bool hideInsteadOfDisable = true)
        {
            if (toolStrip == null) return;

            foreach (ToolStripItem item in toolStrip.Items)
            {
                EvaluateToolStripItemPermission(item, hideInsteadOfDisable);
            }
        }

        private static void EvaluateToolStripItemPermission(ToolStripItem item, bool hideInsteadOfDisable)
        {
            if (item == null) return;

            if (item.Tag != null)
            {
                string tagStr = item.Tag.ToString()?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(tagStr))
                {
                    bool isAllowed = IsPermissionSatisfied(tagStr);
                    if (hideInsteadOfDisable)
                        item.Visible = isAllowed;
                    else
                        item.Enabled = isAllowed;
                }
            }

            // Nếu là DropDown / Menu phân cấp
            if (item is ToolStripDropDownItem dropDownItem && dropDownItem.HasDropDownItems)
            {
                foreach (ToolStripItem subItem in dropDownItem.DropDownItems)
                {
                    EvaluateToolStripItemPermission(subItem, hideInsteadOfDisable);
                }
            }
        }

        /// <summary>
        /// Phân tích cú pháp chuỗi Tag phân quyền và kiểm tra tính hợp lệ với CurrentSession.
        /// Cú pháp hỗ trợ:
        /// - "UC03" hoặc "F03"
        /// - "AUTH:UC03"
        /// - "ROLE:ADMIN"
        /// - "UC04,UC08" (Toán tử OR: Thỏa mãn 1 trong các quyền)
        /// - "UC04+UC08" (Toán tử AND: Thỏa mãn tất cả)
        /// </summary>
        public static bool IsPermissionSatisfied(string tagRule)
        {
            if (string.IsNullOrWhiteSpace(tagRule)) return true;

            // Admin luôn có toàn quyền
            if (CurrentSession.IsAdmin) return true;

            // Kiểm tra theo Role đặc thù (ví dụ: ROLE:ADMIN)
            if (tagRule.StartsWith(TagPrefixRole, StringComparison.OrdinalIgnoreCase))
            {
                string requiredRole = tagRule.Substring(TagPrefixRole.Length).Trim();
                return string.Equals(CurrentSession.MaNhomQuyen, requiredRole, StringComparison.OrdinalIgnoreCase);
            }

            // Bỏ tiền tố AUTH: nếu có
            if (tagRule.StartsWith(TagPrefixAuth, StringComparison.OrdinalIgnoreCase))
            {
                tagRule = tagRule.Substring(TagPrefixAuth.Length).Trim();
            }

            // Toán tử AND (kết nối bởi dấu +)
            if (tagRule.Contains('+'))
            {
                var andTokens = tagRule.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                return andTokens.All(CurrentSession.CoQuyen);
            }

            // Toán tử OR (kết nối bởi dấu phẩy hoặc dấu gạch đứng |)
            var orTokens = tagRule.Split(new[] { ',', '|' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return orTokens.Any(CurrentSession.CoQuyen);
        }

        /// <summary>
        /// Kiểm tra quyền trước khi mở màn hình, nếu từ chối thì hiển thị MessageBox thông báo rõ ràng cho người dùng.
        /// </summary>
        public static bool CheckAccessAndNotify(IWin32Window? owner, string maChucNang, string? tenChucNang = null)
        {
            if (CurrentSession.CoQuyen(maChucNang))
            {
                return true;
            }

            string user = CurrentSession.TenDangNhap ?? "Khách";
            string role = CurrentSession.MaNhomQuyen ?? "Chưa phân quyền";
            string feature = tenChucNang ?? maChucNang;

            MessageBox.Show(
                owner,
                $"Bạn không có quyền truy cập chức năng \"{feature}\"!\n\n" +
                $"• Tài khoản: {user}\n" +
                $"• Nhóm quyền: {role}\n" +
                $"• Mã quyền yêu cầu: {maChucNang}\n\n" +
                "Vui lòng liên hệ Quản trị viên hệ thống (Admin) để được cấp quyền tương ứng.",
                "Từ Chối Truy Cập (Quyền Hạn Bị Giới Hạn)",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        /// <summary>
        /// Khởi chạy một Form an toàn với 2 lớp phòng vệ: kiểm tra quyền truy cập trước, nếu đạt mới mở Form.
        /// </summary>
        public static DialogResult SafeLaunchDialog<TForm>(
            IWin32Window? owner,
            string maChucNang,
            Func<TForm> formFactory,
            string? tenChucNang = null) where TForm : Form
        {
            if (!CheckAccessAndNotify(owner, maChucNang, tenChucNang))
            {
                return DialogResult.Cancel;
            }

            try
            {
                using var form = formFactory();
                return form.ShowDialog(owner);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    owner,
                    $"Đã xảy ra lỗi khi mở chức năng \"{tenChucNang ?? maChucNang}\":\n{ex.Message}",
                    "Lỗi Khởi Chạy",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return DialogResult.Abort;
            }
        }
    }
}
