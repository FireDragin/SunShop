using Microsoft.Ajax.Utilities;
using SunShop.DTO.Response;
using SunShop.DTO.Resquest;
using SunShop.Helper;
using SunShop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace SunShop.Api
{
    [RoutePrefix("api/cart")]
    public class CartApiController : ApiController
    {
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();

        [HttpPost]
        [Route("")]
        public HttpResponseMessage AddToCart(AddToCartRequestDTO cartDTO)
        {
            User user = context.Users
                .Include(u => u.Carts)
                .FirstOrDefault(u => u.Email == HttpContext.Current.User.Identity.Name);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new ApiResponse("error", 404, "User not found"));
            }

            var existingCart = user.Carts.FirstOrDefault(c => c.ProductId == cartDTO.productId);
            if (existingCart != null)
            {
                existingCart.Quantity += cartDTO.quantity;
            }
            else
            {
                Cart newCart = new Cart
                {
                    ProductId = cartDTO.productId,
                    UsersId = user.Id,
                    Quantity = cartDTO.quantity,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                context.Carts.InsertOnSubmit(newCart);
                user.Carts.Add(newCart);
            }

            context.SubmitChanges();

            ApiResponse apiResponse = new ApiResponse("success", 201, "Add to cart success");
            return Request.CreateResponse(HttpStatusCode.Created, apiResponse);
        }


        [HttpGet]
        [Route("quantity")]
        public HttpResponseMessage GetQuantityCartByUser()
        {
            User user = context.Users.Where(u => u.Email == HttpContext.Current.User.Identity.Name).FirstOrDefault();
            int quantity = 0;
            if(user != null)
            {
                quantity = user.Carts.Count;
            }

            ApiResponse apiResponse = new ApiResponse("success", 200, "Get quantity cart success", quantity);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, apiResponse);
            return response;
        }
    }
}
