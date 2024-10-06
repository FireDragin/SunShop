using Microsoft.Ajax.Utilities;
using SunShop.Helper;
using SunShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SunShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();
        // GET: Cart
        public ActionResult Index()
        {
            User user = context.Users.Where(u => u.Email  == HttpContext.User.Identity.Name).FirstOrDefault();
            List<Cart> carts = context.Carts.Where(c => c.UsersId == user.Id).ToList();
            return View(carts);
        }
        
        public ActionResult Delete(FormCollection formCollection)
        {
            int idCart = int.Parse(formCollection["delete-cart"]);
            Cart cart = context.Carts.First(c => c.Id == idCart);
            context.Carts.DeleteOnSubmit(cart);
            context.SubmitChanges();

            return RedirectToAction("Index", "Cart");
        }

        public ActionResult DeleteAll()
        {
            string email = HttpContext.User.Identity.Name;
            User user = context.Users.FirstOrDefault(u => u.Email == email);
            context.Carts.DeleteAllOnSubmit(user.Carts);
            
            context.SubmitChanges();

            return RedirectToAction("Index", "Cart");
        }

    }
}