using Gem.BusinessEntities;
using GEM.BusinessLogics;
using GEM.Helpers;
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
        public IHttpActionResult GetFluency()
        {
            try
            {
                var Fluency = objMission.GetFluency();
                if (Fluency == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content", Json(new { Message = "error", Status = false }).Content));
                else
                {
                    var practice = objMission.Getpractice(Fluency.FluencyLevelId);

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
                        FluencyPractice = new
                        {
                            ShortName = Fluency.ShortName,
                            Number = Fluency.Number,
                            FluencyLevelId = Fluency.FluencyLevelId,
                            Name = Fluency.Name,
                            practice = list
                        },
                    }).Content));
                }
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

        [HttpPost, Route("api/Mission")]
        public IHttpActionResult Post([FromBody]dynamic data)
        {
            try
            {
                var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));

                string startdate = Convert.ToString(json["startdate"]);
                string enddate = Convert.ToString(json["enddate"]);
                int teamjourneyid = Convert.ToInt32(json["teamjourneyid"]);
                JArray practiceid = JArray.Parse(json["practiceid"].ToString());

                if (string.IsNullOrEmpty(startdate)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing StartDate field", Status = false }).Content));
                else if (string.IsNullOrEmpty(enddate)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing enddate field", Status = false }).Content));

                if (Convert.ToDateTime(startdate) <= Convert.ToDateTime(enddate))
                {
                    var missionname = objMission.GetMissionName(teamjourneyid);
                    var jsonMission = JObject.Parse(JsonConvert.SerializeObject(missionname));
                    var MissionNamedet = jsonMission["Name"] + " " + jsonMission["TeamName"];

                    mission mission = new mission();
                    mission.TeamJourneyId = teamjourneyid;
                    mission.Name = MissionNamedet;
                    mission.StartDate = Convert.ToDateTime(startdate);
                    mission.EndDate = Convert.ToDateTime(enddate);

                    var result = objMission.AddorUpdateMission(mission);
                    if (result == 1)
                    {

                        var missionDetail = objMission.GetMission(teamjourneyid);

                        for (int i = 0; i < practiceid.Count(); i++)
                        {
                            mission_practice mission_practice = new mission_practice();
                            mission_practice.MissionId = missionDetail.MissionId;
                            mission_practice.PracticeId = Convert.ToInt32(practiceid[i]);
                            result = objMission.AddorUpdatemissionpractice(mission_practice);
                        }

                        return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { missionId = missionDetail.MissionId, Message = "Mission has been created successfully..!", Status = true }).Content));
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
