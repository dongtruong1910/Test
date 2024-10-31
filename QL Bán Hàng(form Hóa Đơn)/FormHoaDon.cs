using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_Bán_Hàng_form_Hóa_Đơn_
{
    public partial class FormHoaDon : Form
    {
        private readonly QLBanHangEntities db;
        List<tblChiTietHDBan> tblChiTietHDBans;



        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public FormHoaDon()
        {

            this.db = new QLBanHangEntities();
            InitializeComponent();
            LoadEmployeeInfo();
            txtDienThoai.KeyDown += new KeyEventHandler(txtDienThoai_KeyDown);
            this.Size = new Size(1112, Screen.PrimaryScreen.WorkingArea.Height - 100);
            txtSoLuong.KeyDown += new KeyEventHandler(_KeyDown);
            txtGiamGia.KeyDown += new KeyEventHandler(_KeyDown);
            dTPNgayBan.Value = DateTime.Now;
            tblChiTietHDBans = new List<tblChiTietHDBan>();

        }

        public void LoadEmployeeInfo()
        {
            cbMaNV.Text = MaNhanVien; // Giả sử lblMaNhanVien là Label để hiển thị mã nhân viên
            txtTenNV.Text = TenNhanVien; // Giả sử lblTenNhanVien là Label để hiển thị tên nhân viên
        }

        private void FormHoaDon_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void cbMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            var TenNhanVien = db.tblNhanViens.Where(c => c.MaNhanVien == cbMaNV.Text.ToString()).Select(c => c.TenNhanVien).FirstOrDefault();
            txtTenNV.Text = TenNhanVien.ToString();
        }

        private void FormHoaDon_Load_1(object sender, EventArgs e)
        {
            var MaNhanViens = db.tblNhanViens.Select(c => c.MaNhanVien).ToList();
            foreach (var ma in MaNhanViens)
            {
                cbMaNV.Items.Add(ma);
            }

            var MaKHs = db.tblKhaches.Select(c => c.MaKhach).ToList();
            foreach (var ma in MaKHs)
            {
                cbMaKH.Items.Add(ma);
            }

            var MaHangs = db.tblHangs.Select(c => c.MaHang).ToList();
            foreach (var ma in MaHangs)
            {
                cbMaHang.Items.Add(ma);
            }

            // Calculate the date 7 days ago
            DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);

            // Query for recent invoices
            var maHoaDons = db.tblHDBans
                .Where(c => c.NgayBan >= sevenDaysAgo) // Use the calculated date
                .Select(c => c.MaHDBan) // Select the invoice codes
                .ToList(); // Return the list of invoice codes

            foreach (var ma in maHoaDons)
            {
                cbMaHoaDon.Items.Add(ma);
            }

            

        }

        private void txtDienThoai_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtDienThoai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Kiểm tra nếu phím nhấn là Enter
            {
                string soDienThoai = txtDienThoai.Text.Trim();

                if (string.IsNullOrEmpty(soDienThoai))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại.");
                    return;
                }

                // Tìm khách hàng trong CSDL
                var khachHang = db.tblKhaches.FirstOrDefault(kh => kh.SoDienThoai == soDienThoai);

                if (khachHang != null)
                {
                    // Khách hàng đã tồn tại, hiển thị thông tin
                    txtDiaChi.Text = khachHang.DiaChi;
                    txtTenKH.Text = khachHang.TenKhach;
                    cbMaKH.Text = khachHang.MaKhach;

                }
                else
                {
                    int id = GetNextCustomerID();
                    var khachHangMoi = new tblKhach
                    {
                        TenKhach = txtTenKH.Text.ToString(),
                        SoDienThoai = txtDienThoai.Text.ToString(), // Số điện thoại nhập vào
                        DiaChi = txtDiaChi.Text.ToString(),
                        MaKhach = "MK" + id.ToString(),
                    };
                    cbMaKH.Items.Add(khachHangMoi.MaKhach);

                    db.tblKhaches.Add(khachHangMoi);
                    db.SaveChanges();
                    MessageBox.Show("Thêm Khách Hàng thành công");
                }
            }

        }

        private int GetNextCustomerID()
        {
            // Lấy mã khách hàng cao nhất hiện có
            var maxID = db.tblKhaches.Count();


            // Nếu không có khách hàng nào, bắt đầu từ 1
            if (maxID == 0)
            {
                return 1;
            }

            // Trả về mã khách hàng mới
            return maxID + 1;
        }


        private void txtTenKH_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            var khachHang = db.tblKhaches.FirstOrDefault(kh => kh.MaKhach == cbMaKH.Text.ToString());

            if (khachHang != null)
            {
                // Khách hàng đã tồn tại, hiển thị thông tin
                txtDiaChi.Text = khachHang.DiaChi;
                txtTenKH.Text = khachHang.TenKhach;
                txtDienThoai.Text = khachHang.SoDienThoai;

            }
        }

        private void dTPNgayBan_ValueChanged(object sender, EventArgs e)
        {
            txtNgayBan.Text = dTPNgayBan.Value.ToString("dd/MM/yyyy");
        }

        private void cbMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbMaHang.SelectedIndex != -1)
            {
                var Hang = db.tblHangs.Where(c => c.MaHang == cbMaHang.Text.ToString()).FirstOrDefault();
                txtTenHang.Text = Hang.TenHang;
                txtDonGia.Text = Hang.DonGiaBan;

                txtSoLuong.Text = "";
                txtGiamGia.Text = "";
            }    
            


        }

        private void btnThemHoaDon_Click(object sender, EventArgs e)
        {
            txtTenKH.Text = "";
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            cbMaKH.SelectedIndex = -1;

            txtMaHoaDon.Text = "HDB_" + dTPNgayBan.Value.ToString("ddMMyyyy") + (db.tblHDBans.Count() + 1).ToString();
            txtNgayBan.Text = "";

            cbMaHang.SelectedIndex = -1;
            //txtTenHang.Text = "";
            //txtDonGia.Text = "";
            txtGiamGia.Text = "";
            txtThanhTien.Text = "";
            txtSoLuong.Text = "";
            //dgvChiTietHoaDon.Rows.Clear();
            if (tblChiTietHDBans != null)
            {
                tblChiTietHDBans.Clear();
            }
            LoadDataToDataGridView();

            btnLuu.Enabled = true;

        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            // Check if txtSoLuong.Text is a valid integer
            if (!int.TryParse(txtSoLuong.Text, out int soLuongNhap) || string.IsNullOrEmpty(txtSoLuong.Text))
            {
                // Ignore if empty or invalid input (could clear the field here if desired)
                return;
            }

            // Retrieve the remaining stock for the selected item
            int soluongconlai = db.tblHangs
                .Where(x => x.MaHang == cbMaHang.Text)
                .Select(x => (int?)x.SoLuong) // Convert to nullable int to avoid null reference issues
                .FirstOrDefault() ?? 0;       // Default to 0 if no stock found

            // Check if the entered quantity exceeds available stock
            if (soLuongNhap > soluongconlai)
            {
                MessageBox.Show("Đơn hàng trong kho chỉ còn lại " + soluongconlai);
                txtSoLuong.Text = ""; // Clear the field if input exceeds stock
            }
            else
            {
                CalculateTotal();
            }
        }


        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            // Check if txtGiamGia.Text is a valid integer or empty
            if (!int.TryParse(txtGiamGia.Text, out int giamGia))
            {
                // Ignore invalid or empty input
                return;
            }

            CalculateTotal();
        }

        private void CalculateTotal()
        {
            // Biến để lưu trữ giá trị số lượng và đơn giá
            decimal soLuong = 0;
            decimal donGia = 0;
            decimal GiamGia = 0;

            // Kiểm tra và chuyển đổi giá trị từ TextBox
            if (decimal.TryParse(txtSoLuong.Text, out soLuong) &&
                decimal.TryParse(txtDonGia.Text, out donGia) && decimal.TryParse(txtGiamGia.Text, out GiamGia))
            {
                // Tính thành tiền
                decimal thanhTien = (soLuong * donGia) - soLuong * donGia * GiamGia / 100;
                txtThanhTien.Text = thanhTien.ToString("N2"); // Định dạng thành tiền với 2 chữ số thập phân
            }
            else
            {
                // Nếu không thể chuyển đổi, đặt thành tiền về rỗng
                txtThanhTien.Text = string.Empty;
            }
        }


        public string ConvertNumberToWords(decimal number)
        {
            if (number == 0)
                return "Không";

            string words = "";
            string[] units = { "", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín" };
            string[] tens = { "", "Mười", "Hai mươi", "Ba mươi", "Bốn mươi", "Năm mươi", "Sáu mươi", "Bảy mươi", "Tám mươi", "Chín mươi" };
            string[] hundreds = { "", "Một trăm", "Hai trăm", "Ba trăm", "Bốn trăm", "Năm trăm", "Sáu trăm", "Bảy trăm", "Tám trăm", "Chín trăm" };

            // Xử lý hàng trăm
            if (number >= 100)
            {
                int hundredsIndex = (int)(number / 100);
                if (hundredsIndex < hundreds.Length) // Kiểm tra xem chỉ số có hợp lệ không
                {
                    words += hundreds[hundredsIndex];
                }
                number %= 100;
                if (number > 0)
                    words += " ";
            }

            // Xử lý hàng chục
            if (number >= 10)
            {
                words += tens[(int)(number / 10)];
                number %= 10;
                if (number > 0)
                    words += " ";
            }

            // Xử lý hàng đơn vị
            if (number > 0)
            {
                words += units[(int)number];
            }

            return words.Trim();
        }




        private void _KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && cbMaHang.Text.ToString() != "" && txtThanhTien.Text.ToString() != "")
            {

                tblChiTietHDBan chitietHDB = new tblChiTietHDBan();
                chitietHDB.MaHDBan = txtMaHoaDon.Text.ToString();
                chitietHDB.MaHang = cbMaHang.Text.ToString();
                chitietHDB.SoLuong = int.Parse(txtSoLuong.Text.ToString());
                chitietHDB.GiamGia = int.Parse(txtGiamGia.Text.ToString());
                chitietHDB.ThanhTien = txtThanhTien.Text.ToString();



                tblChiTietHDBans.Add(chitietHDB);
                LoadDataToDataGridView();

                decimal tong = decimal.TryParse(txtTongTien.Text, out decimal result) ? result : 0;
                tong += decimal.Parse(txtThanhTien.Text.ToString());
                txtTongTien.Text = tong.ToString();



                //lblTongTien.Text = ConvertNumberToWords(tong) + " đồng";


                var hang = db.tblHangs.FirstOrDefault(c => c.MaHang == cbMaHang.Text);


                // Trừ số lượng trong CSDL bằng chính nó trừ đi số lượng trong txtSoLuong
                int soLuongGiam = int.Parse(txtSoLuong.Text);
                hang.SoLuong -= soLuongGiam;

                // Lưu thay đổi vào CSDL
                db.SaveChanges();



            }
            if (e.KeyCode == Keys.Enter && (cbMaHang.Text.ToString() == "" || txtThanhTien.Text.ToString() == ""))
            {
                MessageBox.Show("Yêu cầu nhập đủ Mã Hàng, Số Lượng và Giảm Giá");
            }
        }
        private void LoadDataToDataGridView()
        {
            
                
            

            if (tblChiTietHDBans != null)
            {
                dgvChiTietHoaDon.DataSource = null; // Clear previous binding
                dgvChiTietHoaDon.DataSource = tblChiTietHDBans; // Re-bind with updated list
                dgvChiTietHoaDon.Refresh(); // Ensure display refresh


                // Gán danh sách dữ liệu vào DataGridView


                // Tùy chỉnh tên cột nếu cần
                dgvChiTietHoaDon.Columns["MaHDBan"].HeaderText = "Mã Hóa Đơn";
                dgvChiTietHoaDon.Columns["MaHang"].HeaderText = "Mã Hàng";
                dgvChiTietHoaDon.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvChiTietHoaDon.Columns["GiamGia"].HeaderText = "Giảm Giá";
                dgvChiTietHoaDon.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                if (dgvChiTietHoaDon.Columns["tblHang"] != null)
                {
                    dgvChiTietHoaDon.Columns["tblHang"].Visible = false;
                }
                if(dgvChiTietHoaDon.Columns["tblHDB"] != null)
                {
                    dgvChiTietHoaDon.Columns["tblHDB"].Visible = false;
                }    
                
                

            }
            else
            {
                dgvChiTietHoaDon.DataSource = null;
            }
            

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cbMaKH.Text.ToString() != "" && txtNgayBan.Text.ToString() != "" && txtTongTien.Text.ToString() != "")
            {
                tblHDBan HoaDon = new tblHDBan();
                HoaDon.MaHDBan = txtMaHoaDon.Text.ToString();
                HoaDon.MaNhanVien = cbMaNV.Text.ToString();
                HoaDon.MaKhach = cbMaKH.Text.ToString();
                HoaDon.NgayBan = dTPNgayBan.Value;
                HoaDon.TongTien = txtTongTien.Text.ToString();

                db.tblHDBans.Add(HoaDon);
                db.SaveChanges();
                cbMaHoaDon.Items.Add(HoaDon.MaHDBan);

                foreach (var cthdb in tblChiTietHDBans)
                {
                    db.tblChiTietHDBans.Add(cthdb);
                }

                db.SaveChanges();
                MessageBox.Show("Lưu Hóa Đơn thành công");

                

            }
        }

        private void dgvChiTietHoaDon_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có click vào một dòng hợp lệ
            if (e.RowIndex >= 0)
            {
                // Lấy mã sản phẩm từ hàng được chọn
                var maSanPham = dgvChiTietHoaDon.Rows[e.RowIndex].Cells["MaHang"].Value?.ToString();

                // Xác nhận xóa
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?",
                                                    "Xác nhận xóa",
                                                    MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Xóa sản phẩm khỏi DataGridView
                    dgvChiTietHoaDon.Rows.RemoveAt(e.RowIndex);

                    tblChiTietHDBans.RemoveAt(e.RowIndex);
                }
            }
        }

        private void cbMaHoaDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void btnHuyHoaDon_Click(object sender, EventArgs e)
        {
            // Tìm hóa đơn theo mã hóa đơn
            var hoaDon = db.tblHDBans.FirstOrDefault(c => c.MaHDBan == txtMaHoaDon.Text.ToString());

            if (hoaDon != null)
            {
                // Xóa hóa đơn khỏi cơ sở dữ liệu
                db.tblHDBans.Remove(hoaDon);

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                MessageBox.Show("Xóa Hóa Đơn thành công");
            }
            else
            {
                MessageBox.Show("Không tìm thấy hóa đơn với mã này.");
            }

            var ChiTietHoaDons = db.tblChiTietHDBans.Where(c => c.MaHDBan == txtMaHoaDon.Text.ToString()).ToList();
            foreach (var cthd in ChiTietHoaDons)
            {
                db.tblChiTietHDBans.Remove(cthd);
                db.SaveChanges();
            }

        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            // Lấy danh sách hóa đơn từ cơ sở dữ liệu (giả sử bạn đã có mô hình hóa đơn)


            // Đường dẫn lưu file Excel
            string filePath = "C:\\Users\\pc\\source\\repos\\QL Bán Hàng(form Hóa Đơn)\\"; // Đổi đường dẫn theo ý muốn

            // Tạo file Excel
            using (ExcelPackage package = new ExcelPackage())
            {
                // Tạo một worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Hóa Đơn");

                // Thiết lập tiêu đề cho các cột
                worksheet.Cells[1, 1].Value = "Mã Hóa Đơn";
                worksheet.Cells[1, 2].Value = "Tên Khách Hàng";
                worksheet.Cells[1, 3].Value = "Ngày Bán";
                worksheet.Cells[1, 4].Value = "Tổng Tiền";
                worksheet.Cells[1, 5].Value = "Mã NV";

                // Thêm dữ liệu vào worksheet

                worksheet.Cells[2, 1].Value = txtMaHoaDon.Text;         // Mã hóa đơn
                worksheet.Cells[2, 2].Value = txtTenKH.Text;   // Tên khách hàng
                worksheet.Cells[2, 3].Value = txtNgayBan.Text; // Ngày bán
                worksheet.Cells[2, 4].Value = txtTongTien.Text;        // Tổng tiền
                worksheet.Cells[2, 5].Value = cbMaNV.Text;


                // Lưu file Excel
                FileInfo fi = new FileInfo(filePath);
                package.SaveAs(fi);
            }

            // Thông báo khi in thành công
            MessageBox.Show("Đã in hóa đơn ra file Excel thành công!");
        }

        private void btnĐóng_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            
            if (result == DialogResult.Yes)
            {
                this.Close(); 
            }
        }


        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            var HoaDon = db.tblHDBans.Where(c => c.MaHDBan == cbMaHoaDon.Text.ToString()).FirstOrDefault();
            var ChiTietHoaDons = db.tblChiTietHDBans.Where(c => c.MaHDBan == cbMaHoaDon.Text.ToString()).ToList();

            tblChiTietHDBans.Clear();
            tblChiTietHDBans = ChiTietHoaDons;
            LoadDataToDataGridView();

            txtMaHoaDon.Text = HoaDon.MaHDBan.ToString();
            cbMaNV.Text = HoaDon.MaNhanVien.ToString();
            cbMaKH.Text = HoaDon.MaKhach.ToString();
            txtNgayBan.Text = HoaDon.NgayBan.ToString();
            txtTongTien.Text = HoaDon.TongTien.ToString();
            
           






            cbMaHang.SelectedIndex = -1;
            //txtTenHang.Text = "";
            //txtDonGia.Text = "";
            txtGiamGia.Text = "";
            txtThanhTien.Text = "";
            txtSoLuong.Text = "";



            btnHuyHoaDon.Enabled = true;
            btnInHoaDon.Enabled = true;
        }
    }
}
