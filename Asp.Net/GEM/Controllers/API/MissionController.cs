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
        JourneyServices objJourney = new JourneyServices();

        [HttpGet, Route("api/missionfluency")]
        public IHttpActionResult GetFluency(int teamjourneyid)
        {
            try
            {
                var practice = objMission.Getpractice(teamjourneyid);
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
                        ShortName = practice[0].fluency_level.ShortName,
                        Number = practice[0].fluency_level.Number,
                        FluencyLevelId = practice[0].fluency_level.FluencyLevelId,
                        Name = practice[0].fluency_level.Name,
                        practice = list
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

                var journeyMember = objJourney.GetTeamJourneyMember(teamjourneyid);
                if (journeyMember == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));

                var teamJourneymember = new List<Models.team_journey_member>();

                foreach (var team_journey_member in journeyMember.OrderBy(x => x.TeamJourneyMemberRoleId).ToList())
                {
                    teamJourneymember.Add(new Models.team_journey_member
                    {
                        TeamJourneyId = team_journey_member.TeamJourneyId,
                        TeamJourneyMemberId = team_journey_member.TeamJourneyMemberId,
                        TeamJourneyMemberRoleId = team_journey_member.TeamJourneyMemberRoleId,
                        MemberId = team_journey_member.MemberId,
                        member = new Models.member
                        {
                            MemberId = team_journey_member.member.MemberId,
                            EmailAddress = team_journey_member.member.EmailAddress
                        }
                    });
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", new Models.Team_Journey
                {
                    TeamJourneyId = team_Journey.TeamJourneyId,
                    TeamId = team_Journey.TeamId,
                    JourneyId = team_Journey.JourneyId,
                    team = new Models.team
                    {
                        TeamId = team_Journey.team.TeamId,
                        Name = team_Journey.team.Name,
                    },
                    team_journey_member = teamJourneymember
                }));

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
                        var missionDetail = objMission.GetMission(mission);
                        for (int i = 0; i < practiceid.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(practiceid[i].ToString()))
                            {
                                mission_practice mission_practice = new mission_practice();
                                mission_practice.MissionId = missionDetail.MissionId;
                                mission_practice.PracticeId = Convert.ToInt32(practiceid[i]);
                                result = objMission.AddorUpdatemissionpractice(mission_practice);
                            }
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
