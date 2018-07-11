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
        public ActionResult Create(int journeyId = 1)
        {
            if (ValidateSessionID("LoginMemberID"))
            {
                int memberId = Convert.ToInt16(Session["LoginMemberID"]);

                var objTeam = new API.TeamController();
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
                var teams = objTeam.GetTeams(journeyId, memberId);

                string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)teams).Content.Data);
                var teamDetails = JObject.Parse(responseData);

                model.Teams = JsonConvert.DeserializeObject<List<Team_Journey>>(JsonConvert.SerializeObject(teamDetails["TeamJourney"]));
                model.InstanceTeamName = Convert.ToString("Team" + (teamDetails["Instance"].ToString().Length > 1 ? "" : "0") + teamDetails["Instance"].ToString() + ' ' + "[Name me soon]");

                return View("SelectJourney", model);
            }
            else
            {
                return View("../Users/Index");
            }
        }

        public ActionResult RemoveMember(int tjMemberId)
        {
            var objTeam = new API.TeamController();
            objTeam.DeleteTeamMemberId(tjMemberId);
            return Create();
        }

        public ActionResult MakeLeader(int tjMemberId)
        {
            var objTeam = new API.TeamController();
            objTeam.UpdateMemberRole(tjMemberId);
            return Create();
        }

        public JsonResult CreateMission(int teamJourneyId)
        {
            var objJourney = new API.JourneyController();
            var json = objJourney.GetTeamJourney(teamJourneyId);

            string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)json).Content);
            var teamDetails = JObject.Parse(responseData);

            if (teamDetails["Data"]["Status"].ToString() == "True")
            {
                var count = Convert.ToInt16(teamDetails.Count);
                return Json(new {success = count > 1 }, JsonRequestBehavior.AllowGet);
            }
           else
            {
                var count = Convert.ToInt16(teamDetails["Count"]);
                return Json(new { Message = teamDetails["Data"]["Message"].ToString(), success = count > 1 }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult View()
        {
            if (ValidateSessionID("LoginMemberID"))
            {
                int memberId = Convert.ToInt16(Session["LoginMemberID"]);

                var objTeam = new API.JourneyController();
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
                var teams = objTeam.GetMission(memberId);

                string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)teams).Content);
                var teamDetails = JObject.Parse(responseData);

                model.Teams = JsonConvert.DeserializeObject<List<Team_Journey>>(JsonConvert.SerializeObject(teamDetails["Data"]));
                model.InstanceTeamName = Convert.ToString("Team" + teamDetails["Count"].ToString() + ' ' + "[Name me soon]");

                return View("TeamMission", model);
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