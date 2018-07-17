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
using static GEM.Utilities.HelperEnum;

namespace GEM.Controllers
{
    public class MissionController : Controller
    {
        // GET: Mission
        public ActionResult Index(int teamJourneyId = 0)
        {
            if (ValidateSessionID("LoginMemberID"))
            {
                if(teamJourneyId == 0) return Observe();
                var model = new Mission_Info();
                var loadPractice = new List<Practice>();

                var objmission = new API.MissionController();
                var response = objmission.GetTeam_Journey(teamJourneyId);
                string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)response).Content.Data);

                var userdetail = JObject.Parse(responseData);
                model.journeyid = Convert.ToInt32(userdetail["JourneyId"]);

                model.teams = JsonConvert.DeserializeObject<Team_Journey>(JsonConvert.SerializeObject(userdetail));

                response = objmission.GetFluency(teamJourneyId);

                responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)response).Content.Data);
                var fluencydetail = JObject.Parse(responseData);

                model.fluencyname = fluencydetail["Data"][0]["fluency"]["ShortName"].ToString();

                foreach (var item in fluencydetail["Data"])
                {
                    loadPractice.Add(new Practice
                    {
                        PracticeId = Convert.ToInt32(item["PracticeId"]),
                        FluencyLevelId = Convert.ToInt32(item["FluencyLevelId"]),
                        Name = Convert.ToString(item["Name"]),
                        SequenceNum = Convert.ToInt32(item["SequenceNum"]),
                        PrerequisiteNum = Convert.ToInt32(item["PrerequisiteNum"])
                    });
                }

                model.practiceList = loadPractice;
                model.teamjourneyid = teamJourneyId;
                return View("Index", model);
            }
            else
            {
                return View("../Users/Index");
            }
        }

        // GET: Observe
        public ActionResult teamObserve(int teamJourneyId = 0)
        {
            if (ValidateSessionID("LoginMemberID"))
            {
                if (teamJourneyId == 0) return Observe();

                int memberId = Convert.ToInt16(Session["LoginMemberID"]);

                var objTeam = new API.TeamController();
                var model = new Models.mission();

                var json = objTeam.GetMission(teamJourneyId);
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

        // GET: Observe
        public ActionResult Observe()
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