using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace QuanLyBanQuanAo.Models
{
    public class GioHang
    {
        QuanLyBanQuanAoEntities db = new QuanLyBanQuanAoEntities();

        public string MaSanPhamGioHang { get; set; }
        public string AnhSanPhamGioHang { get; set; }
        public string TenSanPhamGioHang { get; set; }
        public int DonGiaGioHang { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int SoLuongGioHang { get; set; }

        public int ThanhTien()
        {
            return DonGiaGioHang * SoLuongGioHang;
        }

        // Hàm tạo cho giỏ hàng.
        public GioHang(string MaSanPham)
        {
            MaSanPhamGioHang = MaSanPham;
            SanPham sanPham = db.SanPhams.Single(sp => sp.MaSanPham == MaSanPhamGioHang);
            AnhSanPhamGioHang = sanPham.AnhSP;
            TenSanPhamGioHang = sanPham.TenSanPham;
            DonGiaGioHang = sanPham.DonGia;
            SoLuongGioHang = 1;
        }
    }
}