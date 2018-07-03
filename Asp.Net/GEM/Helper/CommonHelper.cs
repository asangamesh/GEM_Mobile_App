using GEM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace GEM.Helpers
{
    public class CommonHelper
    {
        public static ResponseData<dynamic> ResponseData(string error, int statuscode, string status, dynamic datamsg = null, int? count = null, string field = null)
        {
            var responseResult = new ResponseData<dynamic>();
            if (datamsg != null) responseResult.Data = datamsg;
            if (!string.IsNullOrEmpty(error)) responseResult.Errors = new ResponseErrors[] { new ResponseErrors { Code = field, Message = error } };
            else responseResult.Errors = new ResponseErrors[] { };
            responseResult.Status = status;
            responseResult.StatusCode = statuscode;

            return responseResult;
        }
    }
}