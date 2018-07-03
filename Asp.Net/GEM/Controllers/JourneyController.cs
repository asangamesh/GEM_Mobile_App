using GEM.BusinessLogics;
using GEM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GEM.Utilities.HelperEnum;

namespace GEM.Controllers
{
    public class JourneyController : Controller
    {
        // GET: Journey
        public ActionResult Index()
        {
            var model = new List<JourneyInformation>();
            foreach (var item in Enum.GetValues(typeof(Journey_Information)))
            {
                int id = (int)item;
                model.Add(new JourneyInformation
                {
                    JourneyId = id,
                    Name = ((Journey_Information)id).ToString()
                });
            }

            return View(model);
        }

        // GET: Journey
        public ActionResult Create()
        {
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
            model.Journey = objJourney.GetJourney(1);

            return View("SelectJourney", model);
        }
    }
}