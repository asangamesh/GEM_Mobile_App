using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gem.BusinessEntities;
using GEM.BusinessLogics;
using GEM.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GEM.Controllers.API
{
    public class TeamController : ApiController
    {
        TeamServices objTeam = new TeamServices();

        [HttpGet, Route("api/team")]
        public IHttpActionResult Get()
        {
            try
            {
                var Team = objTeam.GetTeam();

                var list = new List<object>();

                for (int i = 0; i < Team.Count(); i++)
                {
                    list.Add(new
                    {
                        TeamId = Team[i].TeamId,
                        Name = Team[i].Name,
                        Description = Team[i].Description,
                        CreatedBy = Team[i].CreatedBy,
                        CreatedDate = Team[i].CreatedDate
                    });
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {
                    User = list,
                    Status = true
                }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/team")]
        public IHttpActionResult Post([FromBody]dynamic name)
        {
            try
            {
                var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(name));

                team team = new team();
                team.Name = json["Name"].ToString();
                team.Description = "";
                team.CreatedBy = Convert.ToInt16(json["MemberId"]);
                team.CreatedDate = DateTime.Now;

                if (string.IsNullOrEmpty(team.Name)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing TeamName field", Status = false }).Content));

                var result = objTeam.AddorUpdateTeam(team);

                if (result == 1)
                {
                    var _Team = objTeam.GetTeamMembers(team.TeamId);
                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { UserID = team.TeamId, Message = "your request is saved", Status = true }).Content));
                }

                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, try again later!", Status = false }).Content));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }
    }
}
