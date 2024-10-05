using SunShop.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SunShop.Helper
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Server=LAPTOP-G162F227\\SQLEXPRESS;Database=SunShop;Trusted_Connection=True;MultipleActiveResultSets=true;Pooling=true;";
        private static SunShopDataContext context = new SunShopDataContext(connectionString);

        public static SunShopDataContext GetDataContext()
        {
            return context;
        }
    }
}