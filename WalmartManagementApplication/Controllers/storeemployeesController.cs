using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WalmartManagementApplication.Models;

namespace WalmartManagementApplication.Controllers
{
    public class storeemployeesController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: storeemployees
        public ActionResult Index()
        {

            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");

            }
            var storeemployees = db.storeemployees.Include(s => s.store);

            return View(storeemployees.ToList().Where(s => s.storeid == Convert.ToInt32(Session["storeid"])));
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("EmployeeLogin");
        }

        public ActionResult changepassword()
        {
            if (Session["employeeid"] == null)
            {
                return RedirectToAction("EmployeeLogin");
            }
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changepassword(empPasswordVM chview)
        {
            var user = db.storeemployees.SingleOrDefault(x => x.id == chview.username && x.password == chview.password);
            if (user != null)
            {


                user.password = chview.newpassword;
                
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Success");



            }
            else
            {
                ModelState.AddModelError("password", "Invalid Username or Password");
                return View();
            }

        }
        // GET: storeemployees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            storeemployee storeemployee = db.storeemployees.Find(id);
            if (storeemployee == null)
            {
                return HttpNotFound();
            }
            return View(storeemployee);
        }

        // GET: storeemployees/Create
        public ActionResult Create()
        {
            if (Session["storeid"] == null || Session["storeid"].ToString().Equals(""))
            {
                return RedirectToAction("storelogin", "stores");
            }
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename");
            return View();
        }
        // POST: storeemployees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,gender,emailid,address,mobileno,qualification,dob,photo,storeid,dateofjoining")] storeemployee storeemployee, HttpPostedFileBase Photo)
        {

            string rand = new Random().Next(1111, 9999).ToString();
            storeemployee.password = rand;
            storeemployee.status = "Active";
            storeemployee.dateofleave = "";
            if (ModelState.IsValid)
            {

                string photoname = Photo.FileName;
                Photo.SaveAs(Server.MapPath("~/uploads/" + photoname));
                storeemployee.photo = "~/uploads/" + photoname;
                db.storeemployees.Add(storeemployee);
                db.SaveChanges();

                var query = (from emp in db.storeemployees
                             orderby emp.id descending
                             select emp).First();

                MailMessage mailmsg = new MailMessage();
                mailmsg.To.Add(storeemployee.emailid);
                mailmsg.From = new MailAddress("walmartmanage619@gmail.com");
                mailmsg.Subject = "Employee Login Details For " + Session["storename"];
                mailmsg.Body = "<div style='text-align:center'><img src='https://www.walmartbrandcenter.com/uploadedImages/BrandCenter/Content/downloads/Logos/specifications/specifications-logo2.jpg?n=4490' /> </br></div>" + "Login ID is : " + query.id + Environment.NewLine + "Password : " + storeemployee.password;
                mailmsg.IsBodyHtml = true;
                mailmsg.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("walmartmanage619@gmail.com", "Female123");
                try
                {
                    client.Send(mailmsg);
                    ViewBag.Message = "Mail Sent Successfully!!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }


                return RedirectToAction("Index");
            }
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", storeemployee.storeid);
            return View(storeemployee);
        }

        // GET: storeemployees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["storeid"] == null)
            {
                return RedirectToAction("storelogin", "stores");

            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            storeemployee storeemployee = db.storeemployees.Find(id);
            if (storeemployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", storeemployee.storeid);
            return View(storeemployee);
        }

        // POST: storeemployees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,gender,emailid,address,mobileno,qualification,dob,photo,storeid,password,dateofjoining,dateofleave,status")] storeemployee storeemployee, HttpPostedFileBase Photo)
        {
            if (storeemployee.dateofleave == null)
            {
                storeemployee.dateofleave = "";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string photoname = Photo.FileName;
                    Photo.SaveAs(Server.MapPath("~/uploads/" + photoname));
                    storeemployee.photo = "~/uploads/" + photoname;
                }
                catch (Exception e)
                {

                }
                db.Entry(storeemployee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.storeid = new SelectList(db.stores, "storeid", "storename", storeemployee.storeid);
            return View(storeemployee);
        }

        // GET: storeemployees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            storeemployee storeemployee = db.storeemployees.Find(id);
            if (storeemployee == null)
            {
                return HttpNotFound();
            }
            return View(storeemployee);
        }

        // POST: storeemployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            storeemployee storeemployee = db.storeemployees.Find(id);
            db.storeemployees.Remove(storeemployee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EmployeeLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmployeeLogin(string id, string password)
        {
            var result = db.storeemployees.SingleOrDefault(x => x.id.ToString() == id && x.password == password);
            if (result == null)
            {
                ModelState.AddModelError("id", "id/Password Doesnt Match");
            }
            else
            {
                Session["employeeid"] = id;
                Session["empsessionid"] = result.storeid;
                Session["empstorename"] = result.store.storename;
                return RedirectToAction("Home","storeemployees");
            }
            return View();
        }
        public ActionResult Home()
        {
            if(Session["employeeid"]==null)
            {
                return RedirectToAction("EmployeeLogin", "storeemployees");

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
