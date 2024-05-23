using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrieuTruongGiang.Models;

namespace TrieuTruongGiang
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            QlbanHangContext dbql = new QlbanHangContext();
            var query = from sp in dbql.SanPhams
                        join p in dbql.LoaiSanPhams
                        on sp.MaLoai equals p.MaLoai
                        where p.MaLoai == "l1"
                        select new 
                        {
                            sp.MaSp,
                            sp.TenSp,
                            p.TenLoai,
                            sp.DonGia,
                            sp.SoLuong,
                            ThanhTien = sp.SoLuong*sp.DonGia
                        };
            dgvThongke.ItemsSource = query.ToList();
        }
    }
}
