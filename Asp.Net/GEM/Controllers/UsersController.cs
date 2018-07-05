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

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            if (ValidateSession("LoginMemberID"))
            {
                return View("HelloWorld");
            }
            else return Login();
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

                return Content("test", "test1");
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

        private bool CheckSessionID(string sessionId)
        {
            if (Session == null || Session.Count == 0) return false;
            else if (Session[sessionId] != null) return true;
            else return false;
        }

        private bool ValidateSession(string sessionId)
        {
            if (CheckSessionID(sessionId))
            {
                int memberId = Convert.ToInt16(sessionId);
                var objUser = new API.UserController();
                var userResponse = objUser.GetUser(memberId);
                string ResponseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)userResponse).Content.Data);
                var userdetail = JObject.Parse(ResponseData);
                Session["LoginEmail"] = userdetail["User"]["EmailAddress"].ToString();
                ViewBag.User = userdetail["User"]["EmailAddress"].ToString();
                return true;
            }
            return false;
        }
    }
}