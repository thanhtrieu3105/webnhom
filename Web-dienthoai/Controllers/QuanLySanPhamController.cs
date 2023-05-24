using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_dienthoai.Models;
using System.Net;
using System.IO;
using System.Data.Entity;
namespace Web_dienthoai.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        QLDienThoaiEntities database = new QLDienThoaiEntities();
        // GET: Product
        public ActionResult Index()
        {
            var list = new List<ProductView>();

            var sanpham = database.SanPham.ToList();
            foreach (var item in sanpham)
            {
                var chitietSP = database.ChiTietSP.FirstOrDefault(id => id.MaSP == item.MaSP);
                var hinhSP = database.HinhSP.FirstOrDefault(id => id.MaSP == item.MaSP);
                var thuongHieu = database.ThuongHieu.Find(item.MaTH);
                var MyView = new ProductView()
                {
                    SanPham = item,
                    HinhSP = hinhSP,
                    ChiTietSP = chitietSP,
                    ThuongHieu = thuongHieu
                };
                list.Add(MyView);
            }
            return View(list);
        }

        public ActionResult Details(string id)
        {
            var tskt = database.TSKTSP.FirstOrDefault(s => s.MaSP == id);
            return View(tskt);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var th = database.ThuongHieu.ToList();

            return View(th);
        }

        [HttpPost]
        public ActionResult Create(SanPham sp, ChiTietSP ctsp, TSKTSP tskt, HttpPostedFileBase Anh, string MaTH)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(sp.TenSP))
                    ModelState.AddModelError(string.Empty, " tên san pham không được để trống");
                if (ModelState.IsValid)
                {
                    ////san pham
                    sp.MaSP = TaoMa("sp");
                    sp.MaTH = MaTH;
                    database.SanPham.Add(sp);
                    ///thong so sp
                    tskt.MaSP = sp.MaSP;
                    database.TSKTSP.Add(tskt);
                    ///chi tiet sp
                    ctsp.MaCTSP = TaoMa("ctsp");
                    ctsp.MaSP = sp.MaSP;
                    ctsp.MaDL = "DL01";
                    ctsp.MaMau = "M01";
                    database.ChiTietSP.Add(ctsp);
                    //hinh san pham
                    var hinhsp = new HinhSP();
                    if (Anh != null)
                    {
                        //Lấy tên file của hình được up lên

                        var fileName = Path.GetFileName(Anh.FileName);

                        //Tạo đường dẫn tới file

                        var path = Path.Combine(Server.MapPath("~/Images/Phones"), fileName);
                        //Lưu tên

                        hinhsp.MaHinh = fileName;
                        hinhsp.MaSP = sp.MaSP;
                        //Save vào Images Folder
                        Anh.SaveAs(path);

                    }
                    database.HinhSP.Add(hinhsp);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            var th = database.ThuongHieu.ToList();
            return View(th);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            
            var pv = new DetailsView();
            pv.SanPham = database.SanPham.FirstOrDefault(s=>s.MaSP==id);
            pv.ChiTietSP = database.ChiTietSP.FirstOrDefault(s => s.MaSP == id);
            pv.TSKTSP = database.TSKTSP.FirstOrDefault(s => s.MaSP == id);
            ViewBag.litsThuongHieu = database.ThuongHieu.ToList();
            var masp = pv.SanPham.MaSP;
            ViewBag.MaTH = new SelectList(database.ThuongHieu, "MaTH", "TenTH", pv.SanPham.MaTH);
            return View(pv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP")] SanPham sp/*, [Bind(Include = "SoLuongKho,Gia")] ChiTietSP ctsp, [Bind(Include = "CongNgheManHinh,DoPhanGiai,ManHinhrong,MatKinhCamUng,DoPhanGiaiCamS,QuayPhim,Flash,TinhNangCamS,DoPhanGiaiCamT,TinhNangCamT,HeDieuHanh,CPU,TocDoCPU,GPU,RAM,DungLuong,DungLuongCon,DanhBa,Mang,Sim,Wifi,GPS,Bluetooth,Sac,Jack,KetNoiKhac,DungLuongPin,LoaiPin,SacToiDa,CongNghePin,BaoMatNC,TinhNangDB,KhangNuocBui,XemPhim,NgheNhac,ThietKe,ChatLieu,KichThuoc,ThoiDiemRaMat")] TSKTSP tsktsp*/)
        {
            if (ModelState.IsValid)
            {
                var sp1 = database.SanPham.FirstOrDefault(s=>s.MaSP==sp.MaSP);
                var ctsp1 = database.ChiTietSP.FirstOrDefault(s => s.MaSP == sp.MaSP);
                var tskt1 = database.TSKTSP.FirstOrDefault(s => s.MaSP == sp.MaSP);
                sp1 = sp;
                //ctsp1 = ctsp;
                //tskt1 = tsktsp;
                
                var masp = sp.MaSP;
                if(masp==null)   return HttpNotFound();
                database.SaveChanges();
            
                return RedirectToAction("Index");
            }
            else return View();
            
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sanpham = database.SanPham.Where(c => c.MaSP == id).FirstOrDefault();
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            return View(sanpham);
        }
        public ActionResult XoaSP(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sanpham = database.SanPham.Where(c => c.MaSP == id).FirstOrDefault();
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            else
            {
                database.SanPham.Remove(sanpham);
                database.SaveChanges();

            }
            return RedirectToAction("Index");
        }


        public string TaoMa(string bien)
        {
            int ma = 1000;

            if (bien == "sp")
            {
                string masp = ma.ToString();
                //kiem tra ton tai ma don hang
                while (database.SanPham.FirstOrDefault(s => s.MaSP == masp) != null)
                {
                    ma++;
                    masp = ma.ToString();
                }
                return masp;
            }
            if (bien == "ctsp")
            {
                ma = 1000000;
                string mactsp = ma.ToString();
                //kiem tra ton tai ma don hang
                while (database.ChiTietSP.FirstOrDefault(s => s.MaCTSP == mactsp) != null)
                {
                    ma++;
                    mactsp = ma.ToString();
                }
                return mactsp;
            }

            return null;
        }


    }
}
