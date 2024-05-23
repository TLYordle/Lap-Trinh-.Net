using System;
using System.Collections.Generic;
using System.Linq;
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
using TranVanLam_2021601486.Models;

namespace TranVanLam_2021601486
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
            dssanpham.ItemsSource = query.ToList();

        }
        private void HienThiCB()
        {
            var query = from sp in db.LoaiSanPhams
                        select sp;
            cbloaisp.ItemsSource = query.ToList();
            cbloaisp.DisplayMemberPath = "TenLoai";
            cbloaisp.SelectedValuePath = "MaLoai";
            cbloaisp.SelectedIndex = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDuLieu();
            HienThiCB();
        }
        public bool CheckDL()
        {
            string tb = "";
            if (string.Equals(txtmasp.Text, "") == true || string.Equals(txttensp.Text, "") == true || string.Equals(txtsoluong.Text, "") == true || string.Equals(txtdongia.Text, "") == true)
            {
                tb += "\n Ban can nhap day du du lieu";
            }
            if (!Regex.IsMatch(txtdongia.Text, @"\d+"))
            {
                tb += "\n Don gia nhap vao phai la so";
            }
            else
            {
                double dg = double.Parse(txtdongia.Text);
                if (dg < 0)
                {
                    tb += "\n Don gia nhap vao phai a so duong";
                }
            }
            if (!Regex.IsMatch(txtsoluong.Text, @"\d+"))
            {
                tb += "\n So luong nhap vao phai la so";
            }
            else
            {
                int sl = int.Parse(txtdongia.Text);
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
        private void btthem_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(t => string.Equals(t.MaSp, txtmasp) == true);
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
                        spMoi.MaSp = txtmasp.Text;
                        spMoi.TenSp = txttensp.Text;
                        spMoi.DonGia = double.Parse(txtdongia.Text);
                        spMoi.SoLuong = int.Parse(txtsoluong.Text);
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

        private void btsua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var spSua = db.SanPhams.SingleOrDefault(t => string.Equals(t.MaSp, txtmasp) == true);
                if (spSua != null)
                {
                    if (CheckDL())
                    {
                        spSua.TenSp = txttensp.Text;
                        LoaiSanPham lsp = (LoaiSanPham)cbloaisp.SelectedItem;
                        spSua.MaLoai = lsp.MaLoai;
                        spSua.DonGia = double.Parse(txtdongia.Text);
                        spSua.SoLuong = int.Parse(txtsoluong.Text);
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

        private void btxoa_Click(object sender, RoutedEventArgs e)
        {
            var spXoa = db.SanPhams.SingleOrDefault(t => string.Equals(t.MaSp, txtmasp) == true);
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

        private void bttim_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bttk_Click(object sender, RoutedEventArgs e)
        {
            Window1 w2 = new Window1();
            w2.Show();
        }
    }
}
