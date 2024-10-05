using SunShop.Helper;
using SunShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SunShop.Auth
{
    public class HasRole : AuthorizeAttribute
    {
        private readonly string[] _role;
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();
        public HasRole(params string[] role) 
        {
            _role = role;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                string email = httpContext.User.Identity.Name;
                string roleName = context.Users.Where(u => u.Email == email).FirstOrDefault().Role.RoleName;

                foreach (var role in _role)
                {
                    return role == roleName;
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if(filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Error/Forbidden");
            }
            else
            {
                filterContext.Result = new RedirectResult("/Auth/Login");
            }
        }
    }
}