using Microsoft.Ajax.Utilities;
using SunShop.Enums;
using SunShop.Helper;
using SunShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SunShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly SunShopDataContext context = DatabaseHelper.GetDataContext();
        // GET: Order
        public ActionResult Index()
        {
            Session["ProductId"] = Request.QueryString["product-id"];
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            User user = context.Users.SingleOrDefault(u => u.Email == HttpContext.User.Identity.Name);

            Shipping shipping = new Shipping() { 
                Name = formCollection["name"],
                PhoneNumber = formCollection["phone-number"],
                Address = formCollection["address"],
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            Order order = null;

            if (Session["ProductId"] != null)
            {
                int productId = int.Parse(Session["ProductId"] as string);
                Product product = context.Products.Where(p => p.Id == productId).FirstOrDefault();

                OrderDetail orderDetail = new OrderDetail() { 
                    Price = product.Price,
                    ProductId = productId,
                    Quantity = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                orderDetails.Add(orderDetail);

                order = new Order()
                {
                    User = user,
                    TotalAmount = product.Price,
                    OrderStatus = OrderStatus.PENDING.ToString(),
                    PaymentStatus = PaymentStatus.PENDING.ToString(),
                    PaymentMethod = PaymentMethod.CASHONDELIVERY.ToString(),
                    Shipping = shipping,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                product.QuantityStock = product.QuantityStock -1;
            }
            else
            {
                decimal totalAmount = 0;
                user.Carts.ForEach(c =>
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        Price = c.Product.Price,
                        ProductId = c.Product.Id,
                        Quantity = c.Quantity,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    orderDetails.Add(orderDetail);

                    totalAmount += c.Product.Price.HasValue ? c.Product.Price.Value * c.Quantity.Value : 0;
                    c.Product.QuantityStock = c.Product.QuantityStock - c.Quantity;
                });

                order = new Order()
                {
                    User = user,
                    TotalAmount = totalAmount,
                    OrderStatus = OrderStatus.PENDING.ToString(),
                    PaymentStatus = PaymentStatus.PENDING.ToString(),
                    PaymentMethod = PaymentMethod.CASHONDELIVERY.ToString(),
                    Shipping = shipping,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                context.Carts.DeleteAllOnSubmit(user.Carts);
                
            }

            orderDetails.ForEach(o => o.Order = order);
            context.OrderDetails.InsertAllOnSubmit(orderDetails);
            context.SubmitChanges();

            return RedirectToAction("CreateOrderSuccess", "Order");
        }

        public ActionResult CreateOrderSuccess()
        {
            return View();
        }
    }
}