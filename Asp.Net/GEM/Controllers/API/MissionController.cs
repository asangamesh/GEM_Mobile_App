using Gem.BusinessEntities;
using GEM.BusinessLogics;
using GEM.Helpers;
using GEM.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static GEM.Utilities.HelperEnum;

namespace GEM.Controllers.API
{
    public class MissionController : ApiController
    {
        MissionServices objMission = new MissionServices();

        [HttpGet, Route("api/Mission")]
        public IHttpActionResult GetFluencyName(int TeamId, int JourneyId)
        {
            try
            {
                var Fluency = objMission.GetFluency(TeamId, JourneyId);
                if (Fluency == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content", Json(new { Message = "error", Status = false }).Content));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {
                    Fluency = new
                    {
                        ShortName = Fluency.ShortName
                    },
                }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/Mission")]
        public IHttpActionResult GetTeam_Journey(int teamjourneyid)
        {
            try
            {
                var team_Journey = objMission.GetTeam_journey(teamjourneyid);

                if (team_Journey == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content", Json(new { Message = "error", Status = false }).Content));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {
                    Team_Journey = new
                    {
                        teamId = team_Journey.TeamId,
                        JourneyId = team_Journey.JourneyId
                    },
                }).Content));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/Mission")]
        public IHttpActionResult GetPractice()
        {
            try
            {
                var practice = objMission.Getpractice();

                var list = new List<object>();

                for (int i = 0; i < practice.Count(); i++)
                {
                    list.Add(new
                    {
                        PracticeId = practice[i].PracticeId,
                        FluencyLevelId = Convert.ToInt32(practice[i].FluencyLevelId),
                        Name = practice[i].Name,
                        SequenceNum = Convert.ToInt32(practice[i].SequenceNum),
                        PrerequisiteNum = Convert.ToInt32(practice[i].PrerequisiteNum)
                    });
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {
                    practice = list,
                    Status = true
                }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/Mission")]
        public IHttpActionResult Post([FromBody]dynamic data)
        {
            try
            {
                var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));

                string startdate = Convert.ToString(json["startdate"]);
                string enddate = Convert.ToString(json["enddate"]);
                int teamjourneyid = Convert.ToInt32(json["teamjourneyid"]);
                int practiceid = Convert.ToInt32(json["practiceid"]);

                if (string.IsNullOrEmpty(startdate)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing StartDate field", Status = false }).Content));
                else if (string.IsNullOrEmpty(enddate)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing enddate field", Status = false }).Content));

                if (Convert.ToDateTime(startdate) <= Convert.ToDateTime(enddate))
                {
                    mission mission = new mission();
                    mission.TeamJourneyId = teamjourneyid;
                    mission.Name = "";
                    mission.StartDate = Convert.ToDateTime(startdate);
                    mission.EndDate = Convert.ToDateTime(enddate);

                    var result = objMission.AddorUpdateMission(mission);
                    if (result == 1)
                    {

                        team_journey_practice team_journey_practice = new team_journey_practice();
                        team_journey_practice.TeamJourneyId = teamjourneyid;
                        team_journey_practice.PracticeId = practiceid;
                        result = objMission.AddorUpdateteamjourneypractice(team_journey_practice);

                        if (result == 1)
                        {
                            var missionDetail = objMission.GetMission(teamjourneyid);

                            mission_practice mission_practice = new mission_practice();
                            mission_practice.MissionId = missionDetail.MissionId;
                            mission_practice.PracticeId = practiceid;
                            result = objMission.AddorUpdatemissionpractice(mission_practice);

                            return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { missionId = missionDetail.MissionId, Message = "Mission has been created successfully..!", Status = true }).Content));
                        }
                        else
                        {
                            return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, try again later!", Status = false }).Content));
                        }

                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, try again later!", Status = false }).Content));
                    }

                }
                else
                {
                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Enddate greater then to Startdate.. ", Status = false }).Content));
                }

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

    }
}
