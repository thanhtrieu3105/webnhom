using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_dienthoai.Models
{
    public class ProductView
    {
        public SanPham SanPham { get; set; }

        public HinhSP HinhSP { get; set; } 

        public ChiTietSP ChiTietSP { get; set;}
    }
}