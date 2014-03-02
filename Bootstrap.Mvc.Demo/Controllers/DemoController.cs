using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bootstrap.Mvc.Demo.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            var db = new NorthwindDbContext();
            var customers = db.Customers.ToList();

            return View(customers);
        }

    }
}
