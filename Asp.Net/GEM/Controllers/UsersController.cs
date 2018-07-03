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
        UserServices objLogin = new UserServices();

        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            if (ValidateSession("LoginMemberID"))
            {
                int memberId = Convert.ToInt16(Session["LoginMemberID"]);
                var objUser = new API.UserController();
                var userResponse = objUser.GetUser(memberId);
                string ResponseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)userResponse).Content.Data);
                var userdetail = JObject.Parse(ResponseData);
                Session["LoginEmail"] = userdetail["User"]["EmailAddress"].ToString();
                ViewBag.User = userdetail["User"]["EmailAddress"].ToString();
                return View("HelloWorld");
            }
            else return Login();
        }

        [WebMethod]
        public void SetSession(string sessionval)
        {
            CreateSession("LoginMemberID", sessionval);
        }

        public void CreateSession(string sessionId, string value)
        {
            if (Session != null && Session.Count > 0 && Session[sessionId] != null) Session.Clear();
            Session[sessionId] = value;
        }

        public bool ValidateSession(string sessionId)
        {
            if (Session == null || Session.Count == 0) return false;
            else if (Session[sessionId] != null) return true;
            else return false;
        }
    }
}