using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WalmartManagementApplication.Models;
using Newtonsoft.Json;

namespace WalmartManagementApplication.Controllers
{
    public class membershipsController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: memberships
        public ActionResult Index()
        {
            if (Session["employeeid"] == null)
            {
                return RedirectToAction("EmployeeLogin", "storeemployees");

            }
            var memberships = db.memberships.Include(m => m.store);
            return View(memberships.ToList());
        }

        // GET: memberships/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            membership membership = db.memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }
        public ActionResult GenerateBill()
        {
            if (Session["employeeid"] == null)
            {
                return RedirectToAction("EmployeeLogin", "storeemployees");

            }
            Session["cartlist"] = null;
            return View();
        }

        public string checkcartdetails(int cardnumber)
        {
            string expirydate = DateTime.Today.ToString();
            var findcard = db.membership_cards.SingleOrDefault(x => x.cardnumber == cardnumber);
            if (findcard != null)
            {
                expirydate = findcard.membership.dateofexpiry;
            }
            if (Convert.ToDateTime(expirydate) < DateTime.Today)
            {
                return "error1";
            }
            if (findcard == null)
            {
                return "error2";
            }
            return "done";
        }
        public ActionResult generateb(int cardnumber)
        {


            if (Session["cartlist"] == null)
            {
                cartlist = new List<CartVM>();

            }
            else
            {
                cartlist = (List<CartVM>)Session["cartlist"];

            }
            bill b = new bill();
            billdetail bd = new billdetail();
            int tamount = 0;
            int tdiscount = 0;
            int tnetprice = 0;
            b.cardnumber = cardnumber;

            foreach (var cart in cartlist)
            {

                tamount = tamount + Convert.ToInt32(cart.price * cart.qty);
                tdiscount = tdiscount + Convert.ToInt32(cart.discount * cart.qty);
                b.storeid = Convert.ToInt32(Session["empsessionid"]);
                b.empid = Convert.ToInt32(Session["employeeid"]);
                b.dateofbill = DateTime.Now.ToString();

            }
            b.totalamount = tamount;
            b.billamount = tamount - tdiscount;
            b.discount = tdiscount;
            db.bills.Add(b);
            db.SaveChanges();
            bd.billid = b.billid;
            foreach (var cart in cartlist)
            {
                bd.productid = cart.productid;
                bd.quantity = cart.qty;
                bd.price = Convert.ToInt32(cart.price);
                bd.netprice = Convert.ToInt32(cart.netprice);
                bd.discount = Convert.ToInt32(cart.discount);
                bd.totalamount = Convert.ToInt32(cart.totalamount);
                db.billdetails.Add(bd);
                db.SaveChanges();

            }
            return View();
        }
        public string checkstockqty(int pid, int Quantity)
        {

            int pqty = 0;
            int getstore = Convert.ToInt32(Session["empsessionid"]);
            try
            {
                var billdet = (db.billdetails.Include("bill").Where(x => x.productid == pid && x.bill.storeid == getstore)).Sum(x => x.quantity);
                pqty = Convert.ToInt32(billdet);
            }
            catch (Exception e)
            {
                pqty = 0;
            }
            if (Session["cartlist"] == null)
            {
                cartlist = new List<CartVM>();



            }
            else
            {
                cartlist = (List<CartVM>)Session["cartlist"];

            }

            Session["cartlist"] = cartlist;
            try {
                var result = (from stock in db.stocks where stock.productid == pid && stock.storeid == getstore select stock.quantity).Sum();
                var getcartqty = cartlist.SingleOrDefault(x => x.productid == pid);
                int getqty = 0;
                if (getcartqty != null)
                {
                    getqty = (cartlist.SingleOrDefault(x => x.productid == pid)).qty;
                }


                int totalqty = result - (getqty + Quantity + pqty);
                if (totalqty < 0)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }
            }
            catch (Exception e)
            {
                return "Error";
            }


        }
        public string checkstockcart(int pid, int Quantity)
        {

            int pqty = 0;
            int getstore = Convert.ToInt32(Session["empsessionid"]);
            try
            {
                var billdet = (db.billdetails.Include("bill").Where(x => x.productid == pid && x.bill.storeid == getstore)).Sum(x => x.quantity);
                pqty = Convert.ToInt32(billdet);
            }
            catch (Exception e)
            {
                pqty = 0;
            }
            if (Session["cartlist"] == null)
            {
                cartlist = new List<CartVM>();



            }
            else
            {
                cartlist = (List<CartVM>)Session["cartlist"];

            }

            Session["cartlist"] = cartlist;
            try
            {
                var result = (from stock in db.stocks where stock.productid == pid && stock.storeid == getstore select stock.quantity).Sum();



                int totalqty = result - (Quantity + pqty);
                if (totalqty < 0)
                {
                    return "false";
                }
                else
                {
                    return "true";
                }
            }
            catch (Exception e)
            {
                return "Error";
            }


        }

        public ActionResult EditCartItem(int sno, int Quantity)
        {
            int index = sno - 1;
            List<CartVM> cartlist = (List<CartVM>)Session["cartlist"];
            int pid = cartlist[index].productid;
            cartlist[index].qty = Quantity;
            cartlist[index].totalamount = Quantity * cartlist[index].netprice;
            Session["cartlist"] = cartlist;
            return PartialView("_CartPartialView", cartlist);



        }
        public ActionResult printbill()
        {
            if (Session["employeeid"] == null)
            {
                return RedirectToAction("EmployeeLogin", "storeemployees");

            }
            List<CartVM> cartlist = (List<CartVM>)Session["cartlist"];
            return View(cartlist);
        }
        List<CartVM> cartlist = null;
        public ActionResult AddToCart(int pid, string productname, int Quantity, double price, double discount, double netprice)
        {
            if (Session["cartlist"] == null)
            {
                cartlist = new List<CartVM>();
            }
            else
            {
                cartlist = (List<CartVM>)Session["cartlist"];

            }
            bool isavaialble = false;
            foreach (var item in cartlist)
            {
                if (item.productid == pid)
                {
                    int qttt = item.qty;
                    item.qty = item.qty + Quantity;
                    item.totalamount = item.qty * item.netprice;
                    isavaialble = true;
                    break;
                }
            }
            if (isavaialble == false)
            {
                cartlist.Add(new CartVM()
                {
                    productid = pid,
                    productname = productname,
                    qty = Quantity,
                    price = price,
                    discount = discount,
                    netprice = netprice,
                    totalamount = Quantity * netprice
                });
            }
            Session["cartlist"] = cartlist;


            return PartialView("_CartPartialView", cartlist);
        }

        public string getdata(string sku)
        {
            int getstore = Convert.ToInt32(Session["empsessionid"]);

            var data = db.products.Where(x => x.skunumber == sku && x.storeid == getstore).Select(x => new
            {
                id = x.pid,
                name = x.productname,
                price = x.price,
                //  disc = x.discount,
                dis = (x.price * x.discount) / 100,
                netprice = x.price - ((x.price * x.discount) / 100)
            });
            if (data.Count() == 0)
            {
                return "false";
            }
            else
            {
                var r = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return r;
            }
        }
        // GET: memberships/Create
        public ActionResult Create()
        {
            if (Session["employeeid"] == null || Session["employeeid"].ToString().Equals(""))
            {
                return RedirectToAction("employeelogin", "storeemployees");

            }
            
            return View();
        }

        // POST: memberships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "memberid,merchantname,emailid,mobileno,address,vatno,servicetaxno,storeid,identifyproof")] membership membership, HttpPostedFileBase identifyproof)
        {

            if (ModelState.IsValid)
            {
                membership.status = "Active";
                int year = Convert.ToInt32(DateTime.Now.Year.ToString()) + 5;
                membership.dateofexpiry = DateTime.Today.AddYears(5).ToString();
                string photoname = identifyproof.FileName;
                identifyproof.SaveAs(Server.MapPath("~/uploads/" + photoname));
                membership.identifyproof = "~/uploads/" + photoname;
                db.memberships.Add(membership);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", membership.storeid);
            return View(membership);
        }

        // GET: memberships/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            membership membership = db.memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", membership.storeid);
            return View(membership);
        }

        // POST: memberships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "memberid,merchantname,emailid,mobileno,address,identifyproof,vatno,servicetaxno,dateofexpiry,status,storeid")] membership membership, HttpPostedFileBase identifyproof)
        {

            if (ModelState.IsValid)
            {
                if (identifyproof != null)
                {
                    string photoname = identifyproof.FileName;
                    identifyproof.SaveAs(Server.MapPath("~/uploads/" + photoname));
                    membership.identifyproof = "~/uploads/" + photoname;
                }
                db.Entry(membership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", membership.storeid);
            return View(membership);
        }

        // GET: memberships/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            membership membership = db.memberships.Find(id);
            if (membership == null)
            {
                return HttpNotFound();
            }
            return View(membership);
        }

        // POST: memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            membership membership = db.memberships.Find(id);
            db.memberships.Remove(membership);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
 
        public ActionResult deletecart(string sno)
        {
            int index = Convert.ToInt32(sno) - 1;
            List<CartVM> cartlist = (List<CartVM>)Session["cartlist"];
            cartlist.RemoveAt(index);
            Session["cartlist"] = cartlist;
            return PartialView("_CartPartialView", cartlist);

        }
        public ActionResult editcart(string sno)
        {

            return PartialView("_CartPartialView", cartlist);

        }
        public ActionResult viewbills()
        {
            var res = db.bills.Where(x => x.billid == -1);
            return View(res.ToList());

        }
        [HttpPost]
        public ActionResult viewbills(int cardnumber)
        {
            var result = db.bills.Where(x => x.cardnumber == cardnumber);
            if (result == null)
            {
             
            }
            else
            {
                return View(result.ToList());
            }
            return View();

        }
        
        public ActionResult BillDetails(int billid)
        {
            var result = db.billdetails.Where(x => x.billid == billid);
            List<billdetail> det = new List<billdetail>();
            if (result==null)
            {

            }
            else
            {
                foreach(var bill in result)
                {
                    det.Add(new Models.billdetail()
                    {
                        productid = bill.productid,
                        quantity = bill.quantity,
                        price = bill.price,
                        discount = bill.discount,
                        netprice = bill.netprice,
                        totalamount = bill.totalamount,    
                    });
                }
                return PartialView("_billdetailspartialview", det);
            }
            return View();
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
