using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunShop.DTO.Response
{
    public class ApiResponse
    {
        public string status { get; set; }
        public int code { get; set; }
        public string message { get; set; }

        public Object data { get; set; }

        public ApiResponse(string status, int code, string message)
        {
            this.status = status;
            this.code = code;
            this.message = message;
        }

        public ApiResponse(string status, int code, string message, object data) : this(status, code, message)
        {
            this.data = data;
        }
    }
}