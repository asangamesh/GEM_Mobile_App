using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Models
{
    public class ResponseData<T>
    {
        public int Count { get; set; }
        public T Data { get; set; }
        public ResponseErrors[] Errors { get; set; }
        public string Status { get; set; }
        public int StatusCode { get; set; }
    }

    public class ResponseErrors
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}