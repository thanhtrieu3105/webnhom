using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_dienthoai.Models
{
    public class MatHangMua
    {
        QLDienThoaiEntities3 db = new QLDienThoaiEntities3();

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
            var phone = db.SanPham.Single(s => s.MaSP == this.MaSP);
            var pic = db.SanPham.FirstOrDefault(s => s.MaSP == this.MaSP);
            var detail = db.ChiTietSP.FirstOrDefault(s => s.MaSP == this.MaSP);
            this.TenSP = phone.TenSP;
            this.HinhSP = db.HinhSP.FirstOrDefault(s => s.MaSP == this.MaSP).ToString();
            this.DonGia = double.Parse(detail.Gia.ToString());
            this.SoLuong = 1;
        }
    }
}