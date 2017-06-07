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
    public class AdministratorsController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: Administrators
        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View(db.Administrators.ToList());
        }


        // GET: Administrators/Create
        public ActionResult Create()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            if(Session["utype"].ToString()=="Limited")
            {
                return RedirectToAction("notauthorised", "Administrators");
            }

            return View();
        }

        public ActionResult notauthorised()
        {
            return View();
        }
        // POST: Administrators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,password,ConfirmPassword,fullname,gender,emailid,usertype")] Administrator administrator)
        {
            administrator.ConfirmPassword = administrator.password;
            if (ModelState.IsValid)
            {
                var result = db.Administrators.Find(administrator.username);
                if (result != null)
                {
                    ModelState.AddModelError("username", "Username Already Exists!");
                }
                else
                { 
                db.Administrators.Add(administrator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            }

            return View(administrator);
        }
        public ActionResult Success()
        {
            return View();
        }
        public JsonResult checkusername(String username)
        {
            var result = db.Administrators.Find(username);
            bool flag;
            if(result==null)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        // GET: Administrators/Edit/5
        public ActionResult Edit(string id)
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
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,password,Confirmpassword,fullname,gender,emailid,usertype")] Administrator administrator)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            if (Session["utype"].ToString() == "Limited")
            {
                return RedirectToAction("notauthorised", "Administrators");
            }

            if (ModelState.IsValid)
            {

                db.Entry(administrator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Where(x => x.Value.Errors.Count>0).Select(x=>new { x.Key, x.Value.Errors }).ToArray();
            }
            return View(administrator);
        }
        public ActionResult changepassword()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult changepassword(ChangePasswordVM chview)
        {
            var user = db.Administrators.SingleOrDefault(x=>x.username==chview.username && x.password==chview.password);
            if (user != null)
            {
              

                    user.password = chview.newpassword;
                    user.ConfirmPassword = chview.confirmpassword;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                  
                    return RedirectToAction("Success");
                 
               

            }
            else
            {
                ModelState.AddModelError("password", "Invalid Usename or Password");
                return View();
            }
            
        }
        public ActionResult Home()
        {
            if(Session["uid"]==null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult AdminLogin(string username,string password)
        {
            var result = db.Administrators.SingleOrDefault(x => x.username == username && x.password == password);
            if(result==null)
            {
                ModelState.AddModelError("username", "Username/Password Doesnt Match");
            }
            else
            {
                Session["uid"] = username;
                Session["utype"] = result.usertype;
                return RedirectToAction("Home");
            }
            return View();
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
      //  [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            if (Session["utype"].ToString() == "Limited")
            {
                return RedirectToAction("notauthorised", "Administrators");
            }
            Administrator administrator = db.Administrators.Find(id);
            try { 
            db.Administrators.Remove(administrator);
            db.SaveChanges();
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Index");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("AdminLogin");
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
