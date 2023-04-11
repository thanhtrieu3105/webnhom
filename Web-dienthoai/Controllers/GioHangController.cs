using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        public List<MatHangMua> LayGioHang()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;

            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }

        public ActionResult ThemSP(string MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();


            MatHangMua phone = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (phone == null)
            {
                phone = new MatHangMua(MaSP);
                gioHang.Add(phone);
            }
            else
            {
                phone.SoLuong++;
            }
            return RedirectToAction("Details", "PhoneStore", new { id = MaSP });
        }


        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
            {
                tongSL = gioHang.Sum(id => id.SoLuong);
            }
            return tongSL;
        }

        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
            {
                TongTien = gioHang.Sum(sp => sp.ThanhTien());
            }
            return TongTien;
        }


        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = LayGioHang();

            if (gioHang == null || gioHang.Count == 0)
            {
                return RedirectToAction("Index", "PhoneStore");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }


        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }


        public ActionResult XoaMatHang(string MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();

            //Lấy sp trong giỏ
            var sanpham = gioHang.FirstOrDefault(id => id.MaSP == MaSP);
            if(sanpham != null)
            {
                gioHang.RemoveAll(id => id.MaSP == MaSP);
                return RedirectToAction("HienThiGioHang"); //Route về trang giỏ hàng
            }
            if(gioHang.Count == 0)
            {
                return RedirectToAction("Index", "PhoneStore"); //Route về trang chủ nếu không có gì
            }
            return RedirectToAction("HienThiGioHang");
        }


        public ActionResult CapNhatMatHang(string MaSP, int Soluong)
        {
            //Lấy giỏ hàng
            List<MatHangMua> gioHang = LayGioHang();

            //Lấy SP trong giỏ hàng
            var sanpham = gioHang.FirstOrDefault(id => id.MaSP == MaSP);
            if (sanpham != null)
            {
                //Cập nhật số lượng tương ứng
                //Số lượng luôn >= 1
                sanpham.SoLuong = Soluong;
            }
            return RedirectToAction("HienThiGioHang");
        }
    }
}