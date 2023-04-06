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
            var sanpham = db.SanPhams.ToList();

            foreach (var item in sanpham)
            {
                var hinhSP = db.HinhSPs.FirstOrDefault(id => id.MaSP == item.MaSP);
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
            var dsDT = db.ThuongHieux.ToList();
            return PartialView(dsDT);
        }


        public ActionResult SPTheoHang(string MaTH)
        {
            var listsp = new List<ProductView>();
            var sanpham = db.SanPhams.ToList();

            foreach (var item in sanpham)
            {
                var hinhSP = db.HinhSPs.FirstOrDefault(id => id.MaSP == item.MaSP);
                
                var myview = new ProductView()
                {
                    SanPham = item,
                    HinhSP = hinhSP
                };

                listsp.Add(myview);
            }

            var filter = listsp.Where(id => id.SanPham.MaTH == MaTH).ToList();

            return View(filter);
        }

        
        
        public ActionResult Details(string id)
        {
            var list = new List<ProductView>();

            var phone = db.SanPhams.FirstOrDefault(s => s.MaSP== id);
            var detail = db.ChiTietSPs.FirstOrDefault(s => s.MaSP == id);
            var pic = db.HinhSPs.FirstOrDefault(s => s.MaSP == id);

            var myview = new ProductView()
            {
                SanPham = phone,
                ChiTietSP = detail,
                HinhSP = pic
            };


            return View(myview);
        }
    }
}