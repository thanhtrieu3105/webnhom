using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCbuoi4.Models;
using System.Net;

namespace MVCbuoi4.Controllers
{
    public class CategoriesController : Controller
    {
        DBSportStoreEntities db = new DBSportStoreEntities();
        // GET: Categories
        public ActionResult Index()
        {
            var catagori = db.Category.ToList();
            return View(catagori);
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Category ct)
        {
            try
            {
                db.Category.Add(ct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Lỗi tạo mới category");
            }
        }
        public ActionResult Details(int id)
        {
            var category = db.Category.Where(s => s.Id == id).FirstOrDefault();
            return View(category);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = db.Category.Where(s => s.Id == id).FirstOrDefault();
            return View(category);
        }
        public ActionResult Edit(int id, Category category)
        {
            db.Entry(category).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = db.Category.Where(s => s.Id == id).FirstOrDefault();
            if(category==null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var category = db.Category.Where(s => s.Id == id).FirstOrDefault();
                db.Category.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác ");
            }
        }
    }
}