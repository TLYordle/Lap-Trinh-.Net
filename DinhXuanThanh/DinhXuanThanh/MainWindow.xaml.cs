using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrieuTruongGiang.Models;

namespace TrieuTruongGiang
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        QlbanHangContext db = new QlbanHangContext();

        private void HienThiDuLieu()
        {
            var query = from sp in db.SanPhams
                        orderby sp.DonGia
                        select new
                        {
                            sp.MaSp,
                            sp.TenSp,
                            sp.MaLoai,
                            sp.SoLuong,
                            sp.DonGia,
                            ThanhTien = sp.DonGia * sp.SoLuong
                        };
            dgvSanPham.ItemsSource = query.ToList();

        }
        private void HienThiCB()
        {
            var query = from sp in db.LoaiSanPhams
                        select sp;
            cboBox.ItemsSource = query.ToList();
            cboBox.DisplayMemberPath = "TenLoai";
            cboBox.SelectedValuePath = "MaLoai";
            cboBox.SelectedIndex = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDuLieu();
            HienThiCB();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(t => string.Equals(t.MaSp, txtMaSP) == true);
            if (query != null)
            {
                MessageBox.Show("Ma san pham nay da ton tai!", "Thong Bao");
                HienThiDuLieu();
            }
            else
            {
                try
                {
                    if (CheckDL())
                    {
                        SanPham spMoi = new SanPham();
                        spMoi.MaSp = txtMaSP.Text;
                        spMoi.TenSp = txtTenSP.Text;
                        spMoi.DonGia = double.Parse(txtDonGia.Text);
                        spMoi.SoLuong = int.Parse(txtSoLuong.Text);
                        db.Add(spMoi);
                        db.SaveChanges();
                        MessageBox.Show("Them thanh cong!", "Thong Bao");
                        HienThiDuLieu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Co loi khi them: " + ex.Message, "Thong Bao");
                }
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var spSua = db.SanPhams.SingleOrDefault(t => string.Equals(t.MaSp, txtMaSP) == true);
                if (spSua != null)
                {
                    if (CheckDL())
                    {
                        spSua.TenSp = txtTenSP.Text;
                        LoaiSanPham lsp = (LoaiSanPham)cboBox.SelectedItem;
                        spSua.MaLoai = lsp.MaLoai;
                        spSua.DonGia = double.Parse(txtDonGia.Text);
                        spSua.SoLuong = int.Parse(txtSoLuong.Text);
                        db.SaveChanges();
                        MessageBox.Show("Suathanh cong!", "Thong Bao");
                        HienThiDuLieu();
                    }
                }
                else
                {
                    MessageBox.Show("Khong tim thay san pham can sua!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Co loi khi sua: " + ex.Message, "Thong Bao");
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var spXoa = db.SanPhams.SingleOrDefault(t => string.Equals(t.MaSp, txtMaSP) == true);
            if (spXoa != null)
            {
                MessageBoxResult rs = MessageBox.Show("Ban co chac chan muon xoa khong?", "Thong Bao", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    db.SanPhams.Remove(spXoa);
                    db.SaveChanges();
                    HienThiDuLieu();
                }
            }
            else
            {
                MessageBox.Show("Khong co san pham nay de xoa!", "Thong Bao");
            }
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnThongKe_Click(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            w2.Show();
        }

        public bool CheckDL()
        {
            string tb = "";
            if (string.Equals(txtMaSP.Text, "") == true || string.Equals(txtTenSP.Text, "") == true || string.Equals(txtSoLuong.Text, "") == true || string.Equals(txtDonGia.Text, "") == true)
            {
                tb += "\n Ban can nhap day du du lieu";
            }
            if (!Regex.IsMatch(txtDonGia.Text, @"\d+"))
            {
                tb += "\n Don gia nhap vao phai la so";
            }
            else
            {
                double dg = double.Parse(txtDonGia.Text);
                if (dg < 0)
                {
                    tb += "\n Don gia nhap vao phai a so duong";
                }
            }
            if (!Regex.IsMatch(txtSoLuong.Text, @"\d+"))
            {
                tb += "\n So luong nhap vao phai la so";
            }
            else
            {
                int sl = int.Parse(txtDonGia.Text);
                if (sl < 0)
                {
                    tb += "\n Don gia nhap vao phai a so duong";
                }
            }
            if (tb != "")
            {
                MessageBox.Show(tb, "Thong Bao", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void cboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvSanPham.SelectedItem != null)
            {
                try
                {
                    Type t = dgvSanPham.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtMaSP.Text = p[0].GetValue(dgvSanPham.SelectedValue).ToString();
                    txtTenSP.Text = p[1].GetValue(dgvSanPham.SelectedValue).ToString();
                    cboBox.SelectedValue = p[2].GetValue(dgvSanPham.SelectedValue).ToString();
                    txtDonGia.Text = p[3].GetValue(dgvSanPham.SelectedValue).ToString();
                    txtSoLuong.Text = p[4].GetValue(dgvSanPham.SelectedValue).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Co loi khi chon hang " + ex.Message, "Thong Bao");
                }
            }
        }
    }
}
