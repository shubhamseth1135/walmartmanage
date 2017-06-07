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
    public class membership_cardsController : Controller
    {
        private WalmartContext db = new WalmartContext();

        // GET: membership_cards
        public ActionResult Index()
        {
            var membership_cards = db.membership_cards.Include(m => m.membership);
            return View(membership_cards.ToList());
        }

        // GET: membership_cards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            membership_cards membership_cards = db.membership_cards.Find(id);
            if (membership_cards == null)
            {
                return HttpNotFound();
            }
            return View(membership_cards);
        }

        // GET: membership_cards/Create
        public ActionResult Create(int ?id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  var query = db.membership_cards.SqlQuery("");


            //     Response.Write(query);
            Session["memberid"] = id;
            int query = (from cards in db.membership_cards where cards.memberid == id select cards.memberid).Count();
            if(query>=3)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();

        }

        // POST: membership_cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "cardnumber,cardholdernname,photo,Mobileno,address,memberid")] membership_cards membership_cards,HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                string photoname = photo.FileName;
                photo.SaveAs(Server.MapPath("~/uploads/" + photoname));
                membership_cards.photo = "~/uploads/" + photoname;
                db.membership_cards.Add(membership_cards);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.memberid = new SelectList(db.memberships, "memberid", "merchantname", membership_cards.memberid);
            return View(membership_cards);
        }

        // GET: membership_cards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            membership_cards membership_cards = db.membership_cards.Find(id);
            if (membership_cards == null)
            {
                return HttpNotFound();
            }
            ViewBag.memberid = new SelectList(db.memberships, "memberid", "merchantname", membership_cards.memberid);
            return View(membership_cards);
        }

        // POST: membership_cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cardnumber,cardholdernname,photo,Mobileno,address,memberid")] membership_cards membership_cards,HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    string photoname = photo.FileName;
                    photo.SaveAs(Server.MapPath("~/uploads/" + photoname));
                    membership_cards.photo = "~/uploads/" + photoname;
                }
                db.Entry(membership_cards).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.memberid = new SelectList(db.memberships, "memberid", "merchantname", membership_cards.memberid);
            return View(membership_cards);
        }

        // GET: membership_cards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            membership_cards membership_cards = db.membership_cards.Find(id);
            if (membership_cards == null)
            {
                return HttpNotFound();
            }
            return View(membership_cards);
        }

        // POST: membership_cards/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                membership_cards membership_cards = db.membership_cards.Find(id);
                db.membership_cards.Remove(membership_cards);
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
