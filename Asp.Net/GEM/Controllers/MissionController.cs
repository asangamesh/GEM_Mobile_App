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
        public ActionResult Index()
        {
            var model = new Mission_info();
            var teamJourneyId = 11;
            var loadPractice = new List<Practice>();

            var objmission = new API.MissionController();
            var response = objmission.GetTeam_Journey(teamJourneyId);
            string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)response).Content.Data);

            var userdetail = JObject.Parse(responseData);
            model.JourneyId = Convert.ToInt32(userdetail["Team_Journey"]["JourneyId"]);

            response = objmission.GetFluencyName(Convert.ToInt32(userdetail["Team_Journey"]["teamId"]), model.JourneyId);
            responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)response).Content.Data);
            userdetail = JObject.Parse(responseData);
            model.fluencyName = userdetail["Fluency"]["ShortName"].ToString();

            response = objmission.GetPractice();
            responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)response).Content.Data);
            var practice = JObject.Parse(responseData);

            foreach (var item in practice["practice"])
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

            model.PracticeList = loadPractice;

            return View("Index", model);
        }
    }
}