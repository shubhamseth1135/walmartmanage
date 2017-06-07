using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WalmartManagementApplication.Models;

namespace WalmartManagementApplication.Controllers
{
    public class stocksController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: stocks
        public ActionResult Index()
        {
            if (Session["employeeid"] == null)
            {
                return RedirectToAction("EmployeeLogin", "storeemployees");

            }
            var stocks = db.stocks.Include(s => s.product).Include(s => s.storeemployee).Include(s => s.store);
            return View(stocks.ToList().Where(x=>x.storeid==Convert.ToInt32(Session["empsessionid"])));
        }
        [HttpPost]
        public ActionResult Index(string tbstock)
        {

            if (Session["empsessionid"] == null)
            {
                return RedirectToAction("EmployeeLogin", "storeemployees");
            }   
            return View(db.stocks.ToList().Where(x => x.product.skunumber == tbstock && x.storeid==Convert.ToInt32(Session["empsessionid"])));

        }
        // GET: stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: stocks/Create
        public ActionResult Create()
        {
          
            if(Session["employeeid"]==null)
            {
                return RedirectToAction("EmployeeLogin", "storeemployees");
            }
            ViewBag.productid = new SelectList(db.products, "pid", "productname");
            ViewBag.employeeid = new SelectList(db.storeemployees, "id", "name");
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename");
            return View();
        }
        
        // POST: stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "stockid,productid,storeid,quantity,dateofstock,employeeid")] stock stock)
        {
            stock.storeid = Convert.ToInt32(Session["empsessionid"]);
            if (ModelState.IsValid)
            {
                db.stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productid = new SelectList(db.products, "pid", "productname", stock.productid);
            ViewBag.employeeid = new SelectList(db.storeemployees, "id", "name", stock.employeeid);
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", stock.storeid);
            return View(stock);
        }
        public string getdata(string sku)
        {
            int getstore = Convert.ToInt32(Session["empsessionid"]);
            var data = db.products.Where(x => x.skunumber == sku && x.storeid== getstore).Select(x => new

            {
                id = x.pid,
                name = x.productname,
             
            
           });
            if(data.Count()==0)
            {
                return "false";
            }
            else
            {
                var r = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore

                });
                return r;
            }
            
        }
        // GET: stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.products, "pid", "productname", stock.productid);
            ViewBag.employeeid = new SelectList(db.storeemployees, "id", "name", stock.employeeid);
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", stock.storeid);
            return View(stock);
        }

        // POST: stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "stockid,productid,storeid,quantity,dateofstock,employeeid")] stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.products, "pid", "productname", stock.productid);
            ViewBag.employeeid = new SelectList(db.storeemployees, "id", "name", stock.employeeid);
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", stock.storeid);
            return View(stock);
        }

        // GET: stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stock stock = db.stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            stock stock = db.stocks.Find(id);
            db.stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Index");
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
