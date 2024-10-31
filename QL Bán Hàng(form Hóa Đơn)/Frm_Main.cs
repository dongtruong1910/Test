using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_Bán_Hàng_form_Hóa_Đơn_
{
    public partial class Frm_Main : Form
    {
        private readonly QLBanHangEntities db;
        public Frm_Main()
        {
            this.db = new QLBanHangEntities();
            InitializeComponent();
            txtMatKhau.PasswordChar = '*';
            this.StartPosition = FormStartPosition.CenterScreen;
            txtTaiKhoan.KeyDown += new KeyEventHandler(txtTaiKhoan_KeyDown);
            txtMatKhau.KeyDown += new KeyEventHandler(txtMatKhau_KeyDown);
        }

        

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            var TaikhoanNV = db.tblNhanViens.ToList();
            bool DNthanhcong = false;

            foreach(var tk in TaikhoanNV) {
                if (tk.MaNhanVien == txtTaiKhoan.Text.ToString() && tk.MatKhau == txtMatKhau.Text.ToString())
                {
                    MessageBox.Show("Đăng nhập thành công");
                    DNthanhcong = true;

                    // Mở FormHoaDon trước
                    FormHoaDon form1 = new FormHoaDon();

                    form1.MaNhanVien = tk.MaNhanVien;
                    // Lấy tên nhân viên từ cơ sở dữ liệu dựa trên mã nhân viên
                    form1.TenNhanVien = db.tblNhanViens
                                          .Where(c => c.MaNhanVien == tk.MaNhanVien)
                                          .Select(c => c.TenNhanVien)
                                          .FirstOrDefault();

                    form1.LoadEmployeeInfo();

                    this.Hide();
                    form1.ShowDialog();

                    // Đóng form đăng nhập sau khi đóng FormHoaDon
                    this.Close();

                    break; // Thoát khỏi vòng lặp sau khi tìm thấy tài khoản hợp lệ

                }
            }
            if (DNthanhcong == false)
            {
                txtTaiKhoan.Text = "";
                txtMatKhau.Text = "";
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng, vui lòng nhập lại");
            }
            
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close(); // Đóng form hiện tại
            }
            // Nếu người dùng chọn No thì không thực hiện gì thêm, form sẽ tiếp tục mở
        }

        private void txtTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Kiểm tra nếu phím nhấn là Enter
            {
                btnDangNhap_Click(sender, e); // Gọi sự kiện đăng nhập
            }
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Kiểm tra nếu phím nhấn là Enter
            {
                btnDangNhap_Click(sender, e); // Gọi sự kiện đăng nhập
            }
        }



    }
}
