using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gem.BusinessEntities;
using GEM.BusinessLogics;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GEM.Models;

namespace GEM.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("Index");
        }

        public ActionResult Logon(string email)
        {
            var objLogin = new API.UserController();
            try
            {
                var userDetails = objLogin.Get(email);
                string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)userDetails).Content.Data);
                var user = JObject.Parse(responseData);

                string userId = user["User"]["UserId"].ToString();
                string emailAddress = user["User"]["EmailAddress"].ToString();

                CreateSession("LoginMemberID", userId);
                CreateSession("LoginEmail", emailAddress);

                return Content("success", "OK");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("Index");
        }

        [WebMethod]
        public void SetSession(string sessionval)
        {
            CreateSession("LoginMemberID", sessionval);
        }

        private void CreateSession(string sessionId, string value)
        {
            if (Session != null && Session.Count > 0 && Session[sessionId] != null) Session.Clear();
            Session[sessionId] = value;
        }
    }
}