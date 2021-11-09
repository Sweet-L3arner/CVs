using QuanLyBanQuanAo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanQuanAo.ViewModels
{
    public class TrangChuViewModel
    {
        public IEnumerable<LoaiSanPham> LoaiSanPhams { get; set; }
        public IEnumerable<NhanHieu> NhanHieus { get; set; }
        public IEnumerable<SanPham> SanPhams { get; set; }
    }
}