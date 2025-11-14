using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace Ban_Sach_Online.Views.Admin
{
    public partial class AdminWindow : Window
    {
        public ObservableCollection<DoanhThuNgay> DoanhThuThangHienTai { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            DataContext = this;

            // 👉 Dữ liệu mẫu
            DoanhThuThangHienTai = new ObservableCollection<DoanhThuNgay>
            {
                new DoanhThuNgay { Ngay = "1", GiaTri = 100, SoDonHang = 5 },
                new DoanhThuNgay { Ngay = "2", GiaTri = 180, SoDonHang = 8 },
                new DoanhThuNgay { Ngay = "3", GiaTri = 150, SoDonHang = 6 },
                new DoanhThuNgay { Ngay = "4", GiaTri = 250, SoDonHang = 12 },
                new DoanhThuNgay { Ngay = "5", GiaTri = 200, SoDonHang = 10 },
                new DoanhThuNgay { Ngay = "6", GiaTri = 300, SoDonHang = 15 },
                new DoanhThuNgay { Ngay = "7", GiaTri = 270, SoDonHang = 14 },
                new DoanhThuNgay { Ngay = "8", GiaTri = 320, SoDonHang = 16 },
                new DoanhThuNgay { Ngay = "9", GiaTri = 180, SoDonHang = 7 },
                new DoanhThuNgay { Ngay = "10", GiaTri = 240, SoDonHang = 11 }
            };

            LoadChart();
        }

        private void LoadChart()
        {
            var doanhThuValues = new ChartValues<double>(DoanhThuThangHienTai.Select(d => d.GiaTri));
            var soDonHangValues = new ChartValues<double>(DoanhThuThangHienTai.Select(d => d.SoDonHang));
            var ngayLabels = DoanhThuThangHienTai.Select(d => d.Ngay).ToArray();

            chartDoanhThu.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Doanh Thu (VNĐ)",
                    Values = doanhThuValues,
                    Fill = System.Windows.Media.Brushes.Green
                },
                new LineSeries
                {
                    Title = "Số Đơn Hàng",
                    Values = soDonHangValues,
                    Stroke = System.Windows.Media.Brushes.OrangeRed,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    PointGeometrySize = 6,
                    ScalesYAt = 1 // Dùng trục Y thứ 2
                }
            };

            // Trục X: Ngày
            chartDoanhThu.AxisX.Clear();
            chartDoanhThu.AxisX.Add(new Axis
            {
                Title = "Ngày",
                Labels = ngayLabels
            });

            // Trục Y chính: Doanh thu
            chartDoanhThu.AxisY.Clear();
            chartDoanhThu.AxisY.Add(new Axis
            {
                Title = "Doanh Thu (VNĐ)",
                LabelFormatter = value => value.ToString("N0")
            });

            // Trục Y phụ: Số đơn hàng
            chartDoanhThu.AxisY.Add(new Axis
            {
                Title = "Số Đơn Hàng",
                Position = AxisPosition.RightTop,
                LabelFormatter = value => value.ToString("N0")
            });
        }

        // --- Các nút điều hướng ---
        private void BtnQLSach_Click_1(object sender, RoutedEventArgs e)
        {
            QlSach qlSachWindow = new QlSach { Owner = this };
            qlSachWindow.ShowDialog();
        }

        private void BtnQLUser_Click(object sender, RoutedEventArgs e)
        {
            QlNguoiDung qlUserWindow = new QlNguoiDung { Owner = this };
            qlUserWindow.ShowDialog();
        }

        private void BtnQLDonHang_Click(object sender, RoutedEventArgs e)
        {
            QlDonHang qlDonHangWindow = new QlDonHang { Owner = this };
            qlDonHangWindow.ShowDialog();
        }

        private void BtnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new DangNhap().Show();
                this.Close();
            }
        }

        private void BtnQlDanhMuc_Click(object sender, RoutedEventArgs e)
        {
            QlDanhMuc qlDanhMuc = new QlDanhMuc { Owner = this };
            qlDanhMuc.ShowDialog();
        }
    }

    // 🔹 Lớp đại diện dữ liệu cho từng ngày
    public class DoanhThuNgay
    {
        public string Ngay { get; set; }
        public double GiaTri { get; set; }     // Doanh thu (cột)
        public double SoDonHang { get; set; }  // Số đơn hàng (đường)
    }
}
