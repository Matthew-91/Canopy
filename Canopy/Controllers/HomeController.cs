using Canopy.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Canopy.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private CanopyEntities db = new CanopyEntities();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var accounts = db.Customers.Single(c => c.AspNetUserId == userId).BankAccounts;

            return View(accounts.ToList());
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
    }
}