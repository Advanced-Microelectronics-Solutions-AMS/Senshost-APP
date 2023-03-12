﻿using System.Net;

namespace Senshost.Models.Common
{
    public class ErrorModel
    {
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
