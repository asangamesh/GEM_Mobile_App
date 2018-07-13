using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gem.BusinessEntities;
using GEM.BusinessLogics;
using static GEM.Utilities.HelperEnum;
using GEM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace GEM.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult Measures(int journeyId = 0)
        {
            if (ValidateSessionID("LoginMemberID"))
            {
                if (journeyId == 0) return Index();

                int memberId = Convert.ToInt16(Session["LoginMemberID"]);

                var objTeam = new API.TeamController();
                var model = new Models.mission();

                var json = objTeam.GetMission(journeyId);
                string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)json).Content);
                var teamDetails = JObject.Parse(responseData);

                model = JsonConvert.DeserializeObject<Models.mission>(JsonConvert.SerializeObject(teamDetails["Data"]));
                return View("TeamMeasure", model);
            }
            else
            {
                return View("../Users/Index");
            }
        }

        // GET: Team
        public ActionResult Index()
        {
            if (ValidateSessionID("LoginMemberID"))
            {
                int memberId = Convert.ToInt16(Session["LoginMemberID"]);

                var objTeam = new API.TeamController();
                var model = new List<Models.mission>();

                var json = objTeam.GetMissionbyMember(memberId);
                string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)json).Content);
                var teamDetails = JObject.Parse(responseData);

                model = JsonConvert.DeserializeObject<List<Models.mission>>(JsonConvert.SerializeObject(teamDetails["Data"]));
                return View("Measures", model);
            }
            else
            {
                return View("../Users/Index");
            }
        }

        private bool ValidateSessionID(string sessionId)
        {
            if (Session == null || Session.Count == 0) return false;
            else if (Session[sessionId] != null) return true;
            else return false;
        }
    }
}