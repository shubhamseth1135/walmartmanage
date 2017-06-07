using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WalmartManagementApplication.Models;

namespace WalmartManagementApplication.Controllers
{
    public class subcategoriesController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: subcategories
        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }

            var subcategories = db.subcategories.Include(s => s.category);
            ViewBag.categories = new SelectList(db.categories, "id", "categoryname");
            return View(subcategories.ToList());

        }
        

        [HttpPost]
        public ActionResult Index(string tbsearch)
        {


            var subcategories = db.subcategories.Include(s => s.category);
            ViewBag.categories = new SelectList(db.categories, "id", "categoryname");
            return View(subcategories.ToList().Where( s => s.categoryid.ToString() == tbsearch));

        }




        // GET: subcategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategory subcategory = db.subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        // GET: subcategories/Create
        public ActionResult Create()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }
            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname");
            return View();
        }

        // POST: subcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,categoryid,subcategoryname,description")] subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.subcategories.Add(subcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname", subcategory.category);
            return View(subcategory);
        }

        // GET: subcategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategory subcategory = db.subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.categories = new SelectList(db.categories, "id", "categoryname", subcategory.category);
            return View(subcategory);
        }
        public JsonResult checksubcategory(string subcategoryname,string categoryid)
        {
         //   var findcat = db.subcategories.SingleOrDefault(x => x.subcategoryname == subcategoryname);

          //  var result = db.subcategories.SingleOrDefault(x => x.subcategoryname == subcategoryname && x.category.categoryname==findcat.category.categoryname);

            var r = db.subcategories.SingleOrDefault(x => x.categoryid.ToString() == categoryid && x.subcategoryname == subcategoryname);

            bool flag;
            if (r == null)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        // POST: subcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,categoryid,subcategoryname,description")] subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            ViewBag.categories = new SelectList(db.categories, "id", "categoryname", subcategory.category);
            return View(subcategory);
        }

        // GET: subcategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategory subcategory = db.subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        // POST: subcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                subcategory subcategory = db.subcategories.Find(id);
                db.subcategories.Remove(subcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                Session["error"] = "Cannot Delete This Category";
                return RedirectToAction("Index");
            }

        }

        ////public ActionResult Search(string tbsearch)
        ////{
        ////    var result = db.subcategories.SingleOrDefault(x => x.category.categoryname == tbsearch);

        ////    return View();
        ////}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
