using SunShop.Helper;
using SunShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebPages;

namespace SunShop.Controllers
{
    public class AuthController : Controller
    {
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();

        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            string email = formCollection["email"];
            string password = formCollection["password"];
            ViewBag.Email = email;
            ViewBag.Password = password;

            if (string.IsNullOrEmpty(password))
            {
                ViewBag.ErrMessage = "Mật khẩu không được để trống";
                return View("Login");
            }

            User userLogin = context.Users.Where(
                    u => u.Email == email && u.Password == password
                ).FirstOrDefault();

            if (userLogin == null)
            {
                ViewBag.ErrMessage = "Email hoặc mật khẩu không đúng";
                return View("Login");
            }

            FormsAuthentication.SetAuthCookie(userLogin.Email, true);

            HttpCookie fullNameCookie = new HttpCookie("FullNameCookie")
            {
                Value = HttpUtility.UrlEncode(userLogin.Fullname),
                Expires = DateTime.Now.AddDays(30)
            };
            Response.Cookies.Add(fullNameCookie);
           
            string prevUrl = formCollection["ReturnUrl"];

            if(userLogin.Role.RoleName == "ADMIN")
            {
                return RedirectToAction("Index", "Admin");
            }

            if(!string.IsNullOrEmpty(prevUrl))
            {
                return Redirect(prevUrl);
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection formCollection) 
        {
            string fullName = formCollection["fullName"];
            string email = formCollection["email"];
            string password = formCollection["password"];
            string confirmPassword = formCollection["confirm-password"];
            ViewBag.FullName = fullName;
            ViewBag.Email = email;
            ViewBag.Password = password;
            ViewBag.ConfirmPassword = confirmPassword;

            if(string.IsNullOrEmpty(password))
            {
                ViewBag.ErrMessage = "Mật khẩu không được để trống";
                return View("Register");
            }
            if(string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.ErrMessage = "Mật khẩu nhập lại không được để trống";
                return View("Register");

            }
            if(!password.Equals(confirmPassword))
            {
                ViewBag.ErrMessage = "Mật khẩu nhập lại không đúng";
                return View("Register");
            }

           
            if(context.Users.Any(u => u.Email.Equals(email)))
            {
                ViewBag.ErrMessage = "Email đã được sử dụng";
                return View("Register");
            }

            Role roleCustomer = context.Roles.Where(
                    r => r.RoleName == "CUSTOMER"
                ).FirstOrDefault(); 

            User newUser = new User();
            newUser.Fullname = fullName;
            newUser.Email = email;
            newUser.Password = password;
            newUser.Role = roleCustomer;
            newUser.CreatedAt = DateTime.Now;
            newUser.UpdatedAt = DateTime.Now;
            context.Users.InsertOnSubmit(newUser);
            context.SubmitChanges();

            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            HttpCookie fullNameCookie = new HttpCookie("FullNameCookie")
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Add(fullNameCookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}