using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gem.BusinessEntities;
using GEM.BusinessLogics;
using static GEM.Utilities.HelperEnum;
using GEM.Models;

namespace GEM.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        TeamServices objTeam = new TeamServices();
        public ActionResult Create()
        {
            return View("CreateTeam");
        }

        public ActionResult Team_list()
        {
            
            var Team = objTeam.GetTeam();
            var model = new List<ModelTeam>();
            foreach (var item in Team)
            {
                model.Add(new ModelTeam
                {
                    teamId = item.TeamId,
                    teamName = item.Name,
                    Desc= item.Description,
                    createdby= Convert .ToInt32(item.CreatedBy),
                    createddate= Convert.ToDateTime(item.CreatedDate)

                });
            }

            return View(model);
        }

    }
}