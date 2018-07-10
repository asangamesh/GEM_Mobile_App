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

namespace GEM.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public ActionResult Measures(int journeyId)
        {
            var objTeam = new API.TeamController();
            var model = new Models.mission();

            var json = objTeam.GetMission(journeyId);
            string responseData = JsonConvert.SerializeObject(((System.Web.Http.Results.NegotiatedContentResult<GEM.Models.ResponseData<object>>)json).Content);
            var teamDetails = JObject.Parse(responseData);

            model = JsonConvert.DeserializeObject<Models.mission>(JsonConvert.SerializeObject(teamDetails["Data"]));
            return View("TeamMeasure", model);
        }

    }
}