using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WalmartManagementApplication.Models;

namespace WalmartManagementApplication.Controllers
{
    public class storesController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: stores
        public ActionResult Index()
        {
           

            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }
            return View(db.stores.ToList());
        }
        public ActionResult PromotionalMessage()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");
            }
            return View();
        }

        [HttpPost]
        public ActionResult PromotionalMessage(string Message)
        {
            int storeid = Convert.ToInt32(Session["storeid"]);

            string smstext = string.Concat(Message, " " + Session["storename"].ToString()).Replace(" ", "%20");
            var userslist = db.membership_cards.Where(x => x.membership.storeid == storeid).ToList();
            foreach (var usr in userslist)
            {
                try
                {
                string mobileno = usr.Mobileno;
                string sms = "http://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + mobileno + "&msg=" + smstext + "&msg_type=TEXT&userid=2000036319&auth_scheme=plain&password=vsssmsserver&v=1.1&format=text";
                WebClient client = new WebClient();
                Stream strm = client.OpenRead(sms);
                StreamReader rdr = new StreamReader(strm);
                rdr.ReadToEnd();
                 return RedirectToAction("successpage", "stores");
                }
                catch(Exception e)
                {
                  
                }
            }
            return View();
        }
        public ActionResult successpage()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");
            }
            return View();
        }
        // GET: stores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            store store = db.stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // GET: stores/Create
        public ActionResult Create()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }

            if (Session["utype"].ToString() == "Limited")
            {
                return RedirectToAction("notauthorised", "Administrators");
            }
            return View();
        }


        public JsonResult checkstorename(string storename)
        {
            var result = db.stores.SingleOrDefault(x => x.storename.ToLower() == storename.ToLower());
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
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("storelogin");

        }
        // POST: stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "storeid,storename,address,managername,manageremail,managermobile,storephoneno,photo,password,confirmpassword")] store store, HttpPostedFileBase Photo)
        {
            bool duplicate = checkstore(store.storename);
            if (duplicate)
            {
                ModelState.AddModelError("storename", "Store Already Exists");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    string photoname = Photo.FileName;
                    Photo.SaveAs(Server.MapPath("~/uploads/" + photoname));
                    store.photo = "~/uploads/" + photoname;
                }
                catch (Exception e)
                {

                }
                db.stores.Add(store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(store);
        }

        public bool checkstore(string storename)
        {
            var result = db.stores.SingleOrDefault(x => x.storename == storename);
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        // GET: stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }

            if (Session["utype"].ToString() == "Limited")
            {
                return RedirectToAction("notauthorised", "Administrators");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            store store = db.stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }


        // POST: stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "storeid,storename,address,managername,manageremail,managermobile,storephoneno,photo,password,confirmpassword")] store store, HttpPostedFileBase EPhoto)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }

            if (Session["utype"].ToString() == "Limited")
            {
                return RedirectToAction("notauthorised", "Administrators");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string photoname = EPhoto.FileName;
                    EPhoto.SaveAs(Server.MapPath("~/uploads/" + photoname));
                    store.photo = "~/uploads/" + photoname;
                }
                catch (Exception e)
                {

                }
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(store);
        }
        public ActionResult storelogin()
        {

            return View();
        }
        [HttpPost]
        public ActionResult storelogin(int storeid, string password)
        {
            var result = db.stores.SingleOrDefault(x => x.storeid == storeid && x.password == password);
            if (result == null)
            {
                ModelState.AddModelError("storeid", "Storeid/Password Doesnt Match");
            }
            else
            {
                Session["storeid"] = storeid;
                Session["storename"] = result.storename;
                return RedirectToAction("Home");
            }
            return View();
        }
        public ActionResult Home()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");
            }
            return View();
        }
        public string checkdelete(string storeids)
        { 
         
            store store = db.stores.Find(Convert.ToInt32(storeids));
            try
            {
                db.stores.Remove(store);
                db.SaveChanges();
                return "true";
            }
            catch (Exception e)
            {
                return "false";
            }
            return "false";
        }
        // POST: stores/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin", "Administrators");
            }
            if (Session["utype"].ToString() == "Limited")
            {
                return RedirectToAction("notauthorised", "Administrators");
            }
            store store = db.stores.Find(Convert.ToInt32(id));
            try
            {
                db.stores.Remove(store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Session["error"] = "Cannot Delete This Store";
                return RedirectToAction("Index");
            }
            
            return View();

        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult changepassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changepassword(ChangePasswordStore chview)
        {

            var user = db.stores.SingleOrDefault(x => x.storeid == chview.storeid && x.password == chview.password);
            if (user != null)
            {
                user.password = chview.newpassword;
                user.confirmpassword = chview.confirmpassword;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Success");
            }
            else
            {
                ModelState.AddModelError("password", "Invalid Storeid or Password");
                return View();
            }

        }
        public ActionResult monthlysale()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin");
            }
            return View();
        }
        public ActionResult categorysale()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin");
            }
            ViewBag.categoryname = new SelectList(db.categories, "id", "categoryname");
            return View();
        }
        public ActionResult catsales(int categoryid, string startdate, string enddate)
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");
            }

            var r = db.products.Include("subcategory").Where(x => x.subcategory.categoryid == categoryid).ToList().
                Select(x => new
                {
                    pid = x.pid
                });
            int storeid = Convert.ToInt32(Session["storeid"]);
            var catlist = db.bills.Where(x => x.storeid == storeid).ToList();
            DateTime sd = Convert.ToDateTime(startdate);
            DateTime ed = Convert.ToDateTime(enddate);
            List<salesVM> salelist = new List<salesVM>();
            DateTime checkdate;
            int cnt = catlist.Count;
            var subcatlist = db.subcategories.Where(x => x.categoryid == categoryid);
            foreach (var subcat in subcatlist)
            {
                var productlist = db.products.Where(x => x.subcatid == subcat.id);
                foreach (var products in productlist)
                {
                    var billid = db.billdetails.Where(x => x.productid == products.pid);
                    foreach (var bills in billid)
                    {

                        var getdata = db.bills.Where(x => x.billid == bills.billid);
                        foreach (var bill in getdata)
                        {
                            checkdate = DateTime.ParseExact(Convert.ToDateTime(bill.dateofbill).ToShortDateString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            if (checkdate >= sd && checkdate <= ed)
                            {
                                bool duplicate = false;
                                int count = 0;
                                foreach (var sale in salelist)
                                {
                                    count++;
                                    if (sale.dateofsale == checkdate.ToShortDateString())
                                    {
                                        duplicate = true;
                                        break;
                                    }
                                }
                                if (duplicate == false)
                                {
                                    salelist.Add(new Models.salesVM()
                                    {
                                        dateofsale = Convert.ToDateTime(bill.dateofbill).ToShortDateString(),
                                        saleamount = Convert.ToDouble(bills.totalamount)

                                    });
                                }
                                else
                                {
                                    salelist[count - 1].saleamount += Convert.ToDouble(bills.totalamount);
                                }
                            }


                        }
                    }
                }
            }

            return PartialView("_msalepartialview", salelist);
        }
        public ActionResult checksales(string startdate, string enddate)
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");
            }
            int storeid = Convert.ToInt32(Session["storeid"]);
            // var billist = db.bills.SqlQuery("s).ToList();
            var billist = db.bills.Where(x => x.storeid == storeid).ToList();
            DateTime sd = Convert.ToDateTime(startdate);
            DateTime ed = Convert.ToDateTime(enddate);
            List<salesVM> salelist = new List<salesVM>();
            DateTime checkdate;
            int cnt = billist.Count;
            foreach (var bill in billist)
            {
                checkdate = DateTime.ParseExact(Convert.ToDateTime(bill.dateofbill).ToShortDateString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (checkdate >= sd && checkdate <= ed)
                {
                    bool duplicate = false;
                    int count = 0;
                    foreach (var sale in salelist)
                    {
                        count++;
                        if (sale.dateofsale == checkdate.ToShortDateString())
                        {
                            duplicate = true;
                            break;
                        }
                    }
                    if (duplicate == false)
                    {
                        salelist.Add(new Models.salesVM()
                        {
                            dateofsale = Convert.ToDateTime(bill.dateofbill).ToShortDateString(),
                            saleamount = Convert.ToDouble(bill.billamount)

                        });
                    }
                    else
                    {
                        salelist[count - 1].saleamount += Convert.ToDouble(bill.billamount);
                    }
                }
            }
            return PartialView("_msalepartialview", salelist);
        }
        public ActionResult salebyproduct()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin");
            }
            return View();
        }
        public JsonResult saledata()
        {

            int storeid = Convert.ToInt32(Session["storeid"]);
            List<ChartVM> chartlist = new List<ChartVM>();

            var query1 = db.billdetails.Include("bill").Where(x => x.bill.storeid == storeid);

            foreach (var item in query1)
            {
                bool flag = false;
                foreach (var ch in chartlist)
                {
                    if (ch.name == item.product.productname)
                    {
                        flag = true;
                        ch.y += Convert.ToDouble(item.totalamount);
                        break;
                    }
                }
                if (flag == false)
                {
                    chartlist.Add(new ChartVM()
                    {
                        name = item.product.productname,
                        y = Convert.ToDouble(item.totalamount)
                    });
                }
            }
            double total = chartlist.Sum(x => x.y);
            List<ChartVM> chartlist2 = new List<ChartVM>();
            foreach (var item in chartlist)
            {
                var per = Math.Round((item.y / total) * 100, 2);
                chartlist2.Add(new ChartVM()
                {
                    name = item.name,
                    y = per
                });
            }


            return Json(chartlist2, JsonRequestBehavior.AllowGet);
        }
        public ActionResult salebymonth()
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin");
            }
            return View();
        }
        public JsonResult monthlysaledata(int year)
        {

            int storeid = Convert.ToInt32(Session["storeid"]);
            List<ChartVM> chartlist = new List<ChartVM>();

            var query1 = db.bills.Where(x => x.storeid == storeid);
            for (int i = 1; i <= 12; i++)
            {
                string month = new DateTime(year, i, 1).ToString("MMM");
                double sale = 0;
                foreach (var item in query1)
                {
                    int billyear = Convert.ToDateTime(item.dateofbill).Year;
                    int mnth = Convert.ToDateTime(item.dateofbill).Month;
                    if (billyear == year && mnth == i)
                    {
                        sale = sale + Convert.ToDouble(item.billamount);
                    }
                }
                chartlist.Add(new ChartVM()
                {
                    name = month,
                    y = sale
                });
            }
            return Json(chartlist, JsonRequestBehavior.AllowGet);
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
