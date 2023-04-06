using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_dienthoai.Models
{
    public class MatHangMua
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();

        public string MaSP { get; set; }

        public string TenSP { get; set; }

        public string HinhSP { get; set; }

        public double DonGia { get; set; }

        public int SoLuong { get; set; }


        public double ThanhTien()
        {
            return SoLuong * DonGia;
        }

        public MatHangMua(string MaSP)
        {
            this.MaSP = MaSP;
            var phone = db.SanPhams.Single(s => s.MaSP == this.MaSP);
            var pic = db.HinhSPs.FirstOrDefault(s => s.MaSP == this.MaSP);
            var detail = db.ChiTietSPs.FirstOrDefault(s => s.MaSP == this.MaSP);
            this.TenSP = phone.TenSP;
            this.HinhSP = pic.MaHinh;
            this.DonGia = double.Parse(detail.Gia.ToString());
            this.SoLuong = 1;
        }
    }
}