using GEM.BusinessLogics;
using GEM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using static GEM.Utilities.HelperEnum;

namespace GEM.Controllers
{
    public class JourneyController : Controller
    {
        // GET: Journey
        public ActionResult Index()
        {
            return Create();
        }

        // GET: Journey
        public ActionResult Create()
        {
            if (ValidateSession("LoginMemberID"))
            {
                int memberId = Convert.ToInt16(Session["LoginMemberID"]);
                int journeyId = 1;
                var objJourney = new JourneyServices();
                var model = new TeamJourney();

                var journey = new List<JourneyInformation>();

                foreach (var item in Enum.GetValues(typeof(Journey_Information)))
                {
                    int id = (int)item;
                    journey.Add(new JourneyInformation
                    {
                        JourneyId = id,
                        Name = ((Journey_Information)id).ToString()
                    });
                }

                model.JourneyList = journey;
                model.Teams = objJourney.GetTeams(journeyId, memberId);

                var objTeam = new API.TeamController();
                var teamInstance = objTeam.GetTeamInstanceCount(journeyId, memberId);

                string ResponseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)teamInstance).Content.Data);
                var teamDetails = JObject.Parse(ResponseData);
                model.InstanceTeamName = Convert.ToString("Team" + teamDetails["Count"] + ' ' + "[Name me soon]");

                return View("SelectJourney", model);
            }
            else
            {
                return View("../Users/Index");
            }
        }

        public ActionResult RemoveMember(int tjmemberid)
        {
            var objTeam = new API.TeamController();
            objTeam.DeleteTeamMemberId(tjmemberid);
            return Create();
        }

        private bool ValidateSession(string sessionId)
        {
            if (CheckSessionID(sessionId))
            {
                int memberId = Convert.ToInt16(Session[sessionId]);
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

        private bool CheckSessionID(string sessionId)
        {
            if (Session == null || Session.Count == 0) return false;
            else if ( Session[sessionId] != null) return true;
            else return false;
        }
    }
}