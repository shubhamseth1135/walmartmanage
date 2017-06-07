using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WalmartManagementApplication.Models;
namespace WalmartManagementApplication.Controllers
{

    public class HomeController : Controller
    {
        private WalmartContext db = new WalmartContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult feedback()
        {
            return View();
        }
        public string searchfunc(string itemis)
        {

            return itemis;
        }

        public string getproducts(string term)
        {
            var productslist = db.products.Where(x => x.productname.StartsWith(term)).Select(x => x.productname);
            var result = JsonConvert.SerializeObject(productslist, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return result;
        }
        [HttpPost]
        public ActionResult feedback([Bind(Include = "ftext,cardnumber")] feedback feed)
        {
            var result = db.membership_cards.SingleOrDefault(x => x.cardnumber == feed.cardnumber);
            if (result != null)
            {
                if (ModelState.IsValid)
                {
                    db.feedbacks.Add(feed);
                    db.SaveChanges();
                    return RedirectToAction("PublicHome", "products");
                }
            }
            else
            {
                ModelState.AddModelError("cardnumber", "Inavalid Card Number");
            }
            return View();
        }


    }
}