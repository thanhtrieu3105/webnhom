using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class PhoneStoreController : Controller
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();
       
        // GET: PhoneStore
        public ActionResult Index()
        {
            var Pview = new List<ProductView>();
            var sanpham = db.SanPham.ToList();

            foreach (var item in sanpham)
            {
                var hinhSP = db.HinhSP.FirstOrDefault(id => id.MaSP == item.MaSP);
                var MyView = new ProductView()
                {
                    SanPham = item,
                    HinhSP = hinhSP
                };
                Pview.Add(MyView);
            }

            return View(Pview);
        }


        public ActionResult LayHangDT()
        {
            var dsDT = db.ThuongHieu.ToList();
            return PartialView(dsDT);
        }


        public ActionResult SPTheoHang(string MaTH)
        {
            var listsp = new List<ProductView>();
            var sanpham = db.SanPham.ToList();
            var brand = db.ThuongHieu.FirstOrDefault(id => id.MaTH == MaTH);

            foreach (var item in sanpham)
            {
                var hinhSP = db.HinhSP.FirstOrDefault(id => id.MaSP == item.MaSP);
                
                var myview = new ProductView()
                {
                    SanPham = item,
                    HinhSP = hinhSP,
                    ThuongHieu = brand
                };

                listsp.Add(myview);
            }

            ViewBag.HangDT = brand.TenTH;

            var filter = listsp.Where(id => id.SanPham.MaTH == MaTH).ToList();

            return View(filter);
        }

        
        
        public ActionResult Details(string id)
        {
            var list = new List<DetailsView>();

            var phone = db.SanPham.FirstOrDefault(s => s.MaSP== id);
            var detail = db.ChiTietSP.FirstOrDefault(s => s.MaSP == id);
            var tskt = db.TSKTSP.FirstOrDefault(s => s.MaSP == id);

            var pics = (from h in db.HinhSP
                        join s in db.SanPham on h.MaSP equals s.MaSP
                        where s.MaSP == id
                        select h).ToList();

            var myview = new DetailsView()
            {
                SanPham = phone,
                ChiTietSP = detail,
                TSKTSP = tskt,
                HinhSP = pics
            };


            list.Add(myview);
            
            return View(myview);
        }
        [HttpGet]
        public ActionResult Comments(string MaSP,string Kh)
        {
            MatHangMua mh = new MatHangMua(MaSP);
            return View(mh);
        }
        [HttpPost]
        //public ActionResult Comments(BinhLuan bl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (string.IsNullOrEmpty(bl.BinhLuan1))
        //            ModelState.AddModelError(string.Empty, "Bình luận trống");
               
        //        if (ModelState.IsValid)
        //        {
        //            //Tìm KH hợp lệ có trong CSDL
        //            var khachhang = db.KhachHang.FirstOrDefault(id => id.SDT == kh.SDT && id.MK == kh.MK);
        //            if (khachhang != null)
        //            {
        //                ViewBag.ThongBao = "Đăng nhập thành công!";
        //                //Lưu > session
        //                Session["KhachHang"] = khachhang;
        //                return RedirectToAction("Index", "PhoneStore");
        //            }
        //            else
        //                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không hợp lệ!";
        //        }
        //        return View(mh);
        //}
    }
}