using SunShop.Helper;
using SunShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SunShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();
        // GET: Product
        [Authorize]
        public ActionResult Detail(int id)
        {
            Product product = context.Products.Where(p => p.Id == id).FirstOrDefault();
            return View(product);
        }

    }
}