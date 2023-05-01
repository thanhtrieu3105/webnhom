using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_dienthoai.Models
{
    public class CTSPView
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();
        public string MaCTSP { get; set; }
        public string MaSP { get; set; }

        public string TenSP { get; set; }

        public string HinhSP { get; set; }

        public int? DonGia { get; set; }

        public int? SoLuong { get; set; }

        public int? ThanhTien { get; set; }
        public CTSPView(string MaCTSP, int? sl)
        {
            this.MaCTSP = MaCTSP;

            var CTSP = db.ChiTietSP.FirstOrDefault(s => s.MaCTSP == MaCTSP);
            var SP = db.SanPham.FirstOrDefault(s => s.MaSP == CTSP.MaSP);
            this.MaSP = SP.MaSP;
            this.TenSP = SP.TenSP;
            this.HinhSP = db.HinhSP.FirstOrDefault(s => s.MaSP == SP.MaSP).MaHinh;
            this.DonGia = CTSP.Gia;
            this.SoLuong = sl ;
            this.ThanhTien = this.SoLuong * this.DonGia;
        }
    }
}