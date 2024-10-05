using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunShop.DTO.Resquest
{
    public class AddToCartRequestDTO
    {
        public int productId { get; set; }
        public int quantity { get; set; }

        public AddToCartRequestDTO(int productId, int quantity)
        {
            this.productId = productId;
            this.quantity = quantity;
        }
    }
}