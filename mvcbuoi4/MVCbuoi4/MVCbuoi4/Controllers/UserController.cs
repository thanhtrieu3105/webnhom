using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCbuoi4.Models;

namespace MVCbuoi4.Controllers
{
    public class UserController : Controller
    {
        private DBSportStoreEntities db = new DBSportStoreEntities();
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer cust)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cust.NameCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(cust.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(cust.EmailCus))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(cust.PhoneCus))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var khachhang = db.Customer.FirstOrDefault(k => k.NameCus == cust.NameCus);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");
                if (ModelState.IsValid)
                {
                    db.Customer.Add(cust);
                    db.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer cus)
        {
            if(ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(cus.NameCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (String.IsNullOrEmpty(cus.PassCus))
                    ModelState.AddModelError(string.Empty, ("Mật khẩu không được để trống"));
                if(ModelState.IsValid)
                {
                    var khachhang = db.Customer.FirstOrDefault(s => s.NameCus == cus.NameCus && s.PassCus == cus.PassCus);
                    if (khachhang != null)
                    {
                        ViewBag.thongbao = "Đăng nhập thành công";
                        Session["TaiKhoan"] = khachhang;
                    }
                    else
                         ViewBag.thongbao = "Tài khoản hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}