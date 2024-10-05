using SunShop.Helper;
using SunShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SunShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();
        public ActionResult Index()
        {
            List<Product> products = context.Products.Where(p => p.QuantityStock != 0).ToList();
            return View(products);
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

        public ActionResult Search()
        {
            var search = Request.QueryString["key_search"];
            if (search != null)
            {
                var product = context.Products.Include(s => s.Category)
                    .Where(x => x.Name.ToUpper().Contains(search.ToUpper())).ToList();
                return View(product);
            }
            return View();
        }
    }
}