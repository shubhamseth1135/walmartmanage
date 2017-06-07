using Newtonsoft.Json;
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
    public class productsController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: products
        public ActionResult Index()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");
            }
            var products = db.products.Include(p => p.store).Include(p => p.subcategory);
            ViewBag.categories = new SelectList(db.categories, "id", "categoryname");
            return View(products.ToList().Where(x=>x.storeid==Convert.ToInt32(Session["storeid"])));
        }
        public ActionResult checking()
        {
            return View();
        }
        public ActionResult PublicHome(string subcatname)
        {
            var productlist = db.products.Where(x => x.discount > 0).ToList();
            if (!string.IsNullOrEmpty(subcatname))
            {
                productlist = db.products.Where(x => x.subcategory.subcategoryname == subcatname).ToList();
            }
            return View(productlist);
        }
        public ActionResult ProductSearch(string productname)
        {
            var productlist = db.products.Where(x => x.discount >= 0).ToList();
            if (!string.IsNullOrEmpty(productname))
            {
                productlist = db.products.Where(x => x.productname.Contains(productname)).ToList();
            }
            return View(productlist);
        }
        [HttpPost]
        public ActionResult Index(string subcat,string categories)
        {
            var products = db.products.Include(p => p.store).Include(p => p.subcategory);
            ViewBag.categories = new SelectList(db.categories, "id", "categoryname");
            


            return View(products.ToList().Where(s => s.subcatid.ToString() == subcat && s.subcategory.categoryid.ToString() == categories && s.storeid==Convert.ToInt32(Session["storeid"])));
        }



        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public string GetSubcategories(string catid)
        {
            var list = db.subcategories.Where(s => s.categoryid.ToString() == catid);

            var subcatlist = list.Select(x => new
            {
                id = x.id,
                subcategoryname = x.subcategoryname
            });

            var jsondata = JsonConvert.SerializeObject(subcatlist, Formatting.Indented,
                 new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
                );
           
            return jsondata;
        }
        public JsonResult checkskunumber(String skunumber)
        {
            var result = db.products.SingleOrDefault(x=>x.skunumber==skunumber);
            bool flag;
            if (result == null)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        // GET: products/Create
        public ActionResult Create()
        {
            
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");

            }
           

            //ViewBag.storeid = new SelectList(db.stores, "storeid", "storename");
            
            ViewBag.categoryname = new SelectList(db.categories, "id", "categoryname");
            List<SelectListItem> discount = new List<SelectListItem> { };
            for (int i = 0; i <= 16; i++)
            {
                discount.Add(new SelectListItem() { Text = "" + (i * 5), Value = "" + (i * 5) });
            }
            ViewBag.discount = discount;
            return View();
        }

        // POST: products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pid,productname,description,subcatid,price,discount,photo,storeid,skunumber")] product product, HttpPostedFileBase Photo)
        {
            product.storeid = Convert.ToInt32(Session["Storeid"]);
            var result = db.products.SingleOrDefault(x => x.skunumber == product.skunumber);
            if(result!=null)
            {
                ModelState.AddModelError("skunumber","Sku Number Already Exists");
            }
            if (ModelState.IsValid)
            {
                string photoname = Photo.FileName;

                Photo.SaveAs(Server.MapPath("~/uploads/" + photoname));
                product.photo = "~/uploads/" + photoname;
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<SelectListItem> discount = new List<SelectListItem> { };
            for (int i = 0; i <= 16; i++)
            {
                discount.Add(new SelectListItem() { Text = "" + (i * 5), Value = "" + (i * 5) });

            }
            ViewBag.discount = discount;

            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", product.storeid);
            //ViewBag.subcatid = new SelectList(db.subcategories, "id", "subcategoryname", product.subcatid);
            ViewBag.categoryname = new SelectList(db.categories, "id", "categoryname");
            return View(product);
        }

        // GET: products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> discount = new List<SelectListItem> { };
            for (int i = 0; i <= 16; i++)
            {
                discount.Add(new  SelectListItem() { Text = "" + (i * 5), Value = "" + (i * 5) });

            }
            ViewBag.discount = new SelectList(discount,"Value","Text",product.discount);

            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", product.storeid);
            ViewBag.subcatid = new SelectList(db.subcategories, "id", "subcategoryname", product.subcatid);
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pid,productname,description,subcatid,price,discount,photo,storeid,skunumber")] product product,HttpPostedFileBase EPhoto)
        {
            product.storeid = Convert.ToInt32(Session["Storeid"]);
            if (ModelState.IsValid)
            {

                if(EPhoto != null)
                { 
                    string photoname = EPhoto.FileName;
                    EPhoto.SaveAs(Server.MapPath("~/uploads/" + photoname));
                    product.photo = "~/uploads/" + photoname;
                }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");   
            }
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", product.storeid);
            ViewBag.subcatid = new SelectList(db.subcategories, "id", "subcategoryname", product.subcatid); 
            return View(product);
        }

        // GET: products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try { 
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                Session["error"] = "Cannot Delete This Item";
                return RedirectToAction("Index");
            }
        }
        
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
