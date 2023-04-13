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
        QLDienThoaiEntities db = new QLDienThoaiEntities();
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
            if (sanpham != null)
            {
                gioHang.RemoveAll(id => id.MaSP == MaSP);
                return RedirectToAction("HienThiGioHang"); //Route về trang giỏ hàng
            }
            if (gioHang.Count == 0)
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
        [HttpGet]
        public ActionResult ThongTinMuaHang()
        {
            List<MatHangMua> giohang = LayGioHang();
            if (giohang == null || giohang.Count == 0) return RedirectToAction("Index", "PhoneStore");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            ViewBag.TongCong = TinhTongTien() + 25000;
            return View(giohang);
        }
        [HttpPost]
        public ActionResult ThongTinMuaHang(DonHang dh)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(dh.TenNguoiNhan))
                    ModelState.AddModelError(String.Empty, "Họ tên không được để trống ");
                if (String.IsNullOrEmpty(dh.SDTnhan))
                    ModelState.AddModelError(String.Empty, "SDT không được để trống ");
                if (String.IsNullOrEmpty(dh.DiaChiNhan))
                    ModelState.AddModelError(String.Empty, "Địa chỉ không được để trống ");
                if (String.IsNullOrEmpty(dh.HTThanhToan))
                    ModelState.AddModelError(String.Empty, "Chọn httt không được để trống ");
                if (String.IsNullOrEmpty(dh.HTGiaohang))
                    ModelState.AddModelError(String.Empty, "chon htgh không được để trống ");
                if (ModelState.IsValid)
                {
                    dh.TriGia = ((int)TinhTongTien());
                    //giao hang tan nha lam mac dinh se thu them 25k
                    dh.TongTien = ((int)TinhTongTien()) + 25000;
                    dh.TinhTrang = "Chờ Duyệt";
                    dh.NgayDH = DateTime.Now;
                    dh.MaDH = TaoMa("mdh");

                }
                else
                    return View();
            }
            //tao khach hang moi bang thong tin giao hang

            DonHang dh1 = LayTTDon(); //lay thong tin don trong sesion
            dh1 = dh;//gan dh1=dh
            Session["TTdonHang"] = dh1; //luu vao sesion
            var kh = Session["KhachHang"] as KhachHang;
            //ktr dang nhap neu co thif luu thong tin chi tiet don hang
            if (kh != null)
            {//dang dang nhap
                dh.MaKH = kh.MaKH;
            }
            db.DonHang.Add(dh);//add don hang
            db.SaveChanges();
            //them chi tiet don hang vao database
            ChiTietDH ctdh = new ChiTietDH();
            List<MatHangMua> giohang = LayGioHang();
            foreach (var item in giohang)
            {
                ctdh.MaCTDH = TaoMa("ctdh");
                ctdh.SoLuong = item.SoLuong;
                ctdh.Thanhtien = ((int)item.ThanhTien());
                ctdh.MaCTSP = item.MaCTSP;
                ctdh.MaDH = dh.MaDH;
                db.ChiTietDH.Add(ctdh);
            }
            db.SaveChanges();
            if(kh==null)  return RedirectToAction("CauHoiDKTK");
              
            return RedirectToAction("DatHangThanhCong");
        }
        private string TaoMa(string bien)
        {
            int ma = 10;
            if (bien == "mdh")
            {

                string madh = "DH" + ma.ToString();
                //kiem tra ton tai ma don hang
                while (db.DonHang.FirstOrDefault(s => s.MaDH == madh) != null)
                {
                    ma++;
                    madh = "DH" + ma.ToString();
                }
                return madh;
            }
            if (bien == "ctdh")
            {
                string madh = "CTDH" + ma.ToString();
                //kiem tra ton tai ma don hang
                while (db.ChiTietDH.FirstOrDefault(s => s.MaCTDH == madh) != null)
                {
                    ma++;
                    madh = "CTDH" + ma.ToString();
                   
                }
                return madh;
            }
            else return null;

        }
        public ActionResult CauHoiDKTK()
        {

            if (Session["TaiKhoan"] == null)// chua dang nhap
            {
                return View();
            }
            return RedirectToAction("DatHangThanhCong");
        }
        public ActionResult DatHangThanhCong()
        {
            return View();
        }
        public DonHang LayTTDon()
        {
            DonHang dh = Session["TTDonHang"] as DonHang;
            //neu gio hang chua ton tai thi  tao moi dua vao session
            if (dh == null)
            {
                dh = new DonHang();
                Session["TTDonHang"] = dh;
            }
            return dh;
        }
        public ActionResult ThongTinDKTK()
        {

            var dh = LayTTDon();
            if (dh == null) return RedirectToAction("HienThiGioHang");
            if (db.KhachHang.FirstOrDefault(s => s.SDT == dh.SDTnhan) != null)
            {
                ViewBag.thongbao = "SDT đã được dùng không thể vạo tài khoản mới.";
                return View();
            }
            else
            {
                var tk = new KhachHang();
                tk.TenKH = dh.TenNguoiNhan;
                tk.SDT = dh.SDTnhan;
                tk.DiaChi = dh.DiaChiNhan;
                tk.MK = dh.SDTnhan;
                tk.MaLoaiKH = "LKH01";
                db.KhachHang.Add(tk);
                db.SaveChanges();
                ViewBag.TenDN = tk.SDT;
                ViewBag.MK = tk.MK;
                ViewBag.TENKH = tk.TenKH;
                ViewBag.DIACHI = tk.DiaChi;
                ViewBag.thongbao = "Đơn hàng đã được lưu vào tài khoản";
                //gan ma khachhang vao don hang vua mua
                var a = db.KhachHang.FirstOrDefault(s => s.SDT == tk.SDT);
                var donhang = db.DonHang.FirstOrDefault(s => s.MaDH == dh.MaDH);
                donhang.MaKH = a.MaKH;
                db.SaveChanges();
                return View();
            }

        }

    }
}