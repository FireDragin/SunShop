using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunShop.Auth;
using SunShop.Helper;
using SunShop.Models;

namespace SunShop.Controllers
{
    [HasRole("ADMIN")]
    public class AdminController : Controller
    {
        // GET: Admin
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();
        public ActionResult Index()
        {
            ViewBag.DashboardSelected = true;
            return View();
        }

        public ActionResult ManageProducts()
        {
            ViewBag.ProductSelected = true;
            List<Product> products = context.Products.ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ViewBag.ProductSelected = true;

            if (Request.Form.Count > 0)
            {

                Product p = new Product();

                p.Name = Request.Form["name"];
                p.Description = Request.Form["description"];
                p.Price = int.Parse(Request.Form["Price"]);
                p.QuantityStock = int.Parse(Request.Form["QuantityStock"]);
                p.CategoriesId = int.Parse(Request.Form["CategoriesID"]);
                p.CreatedAt = DateTime.Now;
                HttpPostedFileBase file = Request.Files["Picture"];
                if (file != null)
                {
                    string severPath = HttpContext.Server.MapPath("~/Images");
                    string filePath = severPath + "/" + file.FileName;
                    file.SaveAs(filePath);
                    p.ImgUrl = file.FileName;
                }
                context.Products.InsertOnSubmit(p);
                context.SubmitChanges();
                return RedirectToAction("ManageProducts", "Admin");
            }

            List<Category> categories = context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();

        }
        public ActionResult Edit(int id)
        {
            ViewBag.ProductSelected = true;

            Product p = context.Products.FirstOrDefault(x => x.Id == id);
            if (Request.Form.Count == 0)
            {
                List<Category> categories = context.Categories.ToList();
                ViewBag.Categories = categories;
                return View(p);
            }

            p.Name = Request.Form["name"];
            p.Description = Request.Form["description"];
            p.Price = int.Parse(Request.Form["Price"]);
            p.QuantityStock = int.Parse(Request.Form["QuantityStock"]);

            Category category = context.Categories.SingleOrDefault(c => c.Id == int.Parse(Request.Form["CategoriesID"]));
            p.Category = category;
            p.UpdatedAt = DateTime.Now;
            HttpPostedFileBase file = Request.Files["Picture"];
            if (file != null && file.FileName != "")
            {
                string severPath = HttpContext.Server.MapPath("~/Images");
                string filePath = severPath + "/" + file.FileName;
                file.SaveAs(filePath);
                p.ImgUrl = file.FileName;
            }
            
            context.SubmitChanges();
            return RedirectToAction("ManageProducts", "Admin");

        }
        public ActionResult Delete(int id)
        {
            ViewBag.ProductSelected = true;

            Product p = context.Products.FirstOrDefault(x => x.Id == id);
            if (p != null)
            {
                context.Products.DeleteOnSubmit(p);
                context.SubmitChanges();

            }
            return RedirectToAction("ManageProducts", "Admin");

        }
        public ActionResult ManageCategories()
        {
            ViewBag.CategorySelected = true;
            List<Category> category = context.Categories.ToList();
            return View(category);
        }
        public ActionResult CreateCategory()
        {
            ViewBag.CategorySelected = true;

            if (Request.Form.Count > 0)
            {

                Category c = new Category();

                c.Name = Request.Form["name"];
                c.CreatedAt = DateTime.Now;
                c.UpdatedAt = DateTime.Now;
                context.Categories.InsertOnSubmit(c);
                context.SubmitChanges();
                return RedirectToAction("ManageCategories", "Admin");
            }
            return View();

        }
        public ActionResult EditCategory(int id)
        {
            ViewBag.CategorySelected = true;

            Category c = context.Categories.FirstOrDefault(x => x.Id == id);
            if (Request.Form.Count == 0)
            {

                return View(c);
            }
            c.Name = Request.Form["name"];
            c.CreatedAt = DateTime.Now;
            c.UpdatedAt = DateTime.Now;
            context.SubmitChanges();
            return RedirectToAction("ManageCategories", "Admin");
        }
        public ActionResult DeleteCategory(int id)
        {
            ViewBag.CategorySelected = true;

            Category c = context.Categories.FirstOrDefault(x => x.Id == id);
            if (c != null)
            {
                context.Categories.DeleteOnSubmit(c);
                context.SubmitChanges();

            }
            return RedirectToAction("ManageCategories", "Admin");

        }
    }
}