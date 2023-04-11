using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;

namespace Web_dienthoai.Controllers
{
    public class NguoiDungController : Controller
    {
        QLDienThoaiEntities db = new QLDienThoaiEntities();

        // GET: NguoiDung
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            if(ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.TenKH))
                    ModelState.AddModelError(string.Empty, "Họ tên không được để trống");
                if (string.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(kh.MK))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.SDT))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                //Kiểm tra đã có ai đk tài khoản này hay chưa
                var khachhang = db.KhachHangs.FirstOrDefault(id => id.TenDN == kh.TenDN);
                if(khachhang != null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập đã được sử dụng, hãy chọn tên khác");

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.KhachHangs.Add(kh);
                        db.SaveChanges();
                    }
                    catch
                    {
                        return Content("LỖI TẠO MỚI TÀI KHOẢN!");
                    }
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhap");
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangNhap(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.TenDN))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền tên đăng nhập");
                if (string.IsNullOrEmpty(kh.MK))
                    ModelState.AddModelError(string.Empty, "Vui lòng điền mật khẩu");

                if (ModelState.IsValid)
                {
                    //Tìm KH hợp lệ có trong CSDL
                    var khachhang = db.KhachHangs.FirstOrDefault(id => id.TenDN == kh.TenDN && id.MK == kh.MK);
                    if (khachhang != null)
                    {
                        ViewBag.ThongBao = "Đăng nhập thành công!";
                        //Lưu > session
                        Session["KhachHang"] = khachhang;
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không hợp lệ!";
                }
            }
            return View();
        }
    }
}