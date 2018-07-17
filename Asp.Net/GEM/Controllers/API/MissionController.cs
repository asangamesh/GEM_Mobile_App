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
        public IHttpActionResult GetFluency(int teamJourneyId)
        {
            try
            {
                var practice = objMission.GetPractice(teamJourneyId).ToList();
                var list = new List<object>();

                for (int i = 0; i < practice.Count(); i++)
                {
                    list.Add(new
                    {
                        PracticeId = practice[i].PracticeId,
                        FluencyLevelId = Convert.ToInt32(practice[i].FluencyLevelId),
                        Name = practice[i].Name,
                        SequenceNum = Convert.ToInt32(practice[i].SequenceNum),
                        PrerequisiteNum = Convert.ToInt32(practice[i].PrerequisiteNum),
                        fluency = new Models.fluency_level
                        {
                            FluencyLevelId = practice[i].fluency_level.FluencyLevelId,
                            Number = practice[i].fluency_level.Number,
                            Name = practice[i].fluency_level.Name,
                            ShortName = practice[i].fluency_level.ShortName
                        }

                    });
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {

                    Data = list,
                    Status = true

                }).Content));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/Mission")]
        public IHttpActionResult GetTeam_Journey(int teamJourneyId)
        {
            try
            {
                var team_Journey = objMission.GetTeam_Journey(teamJourneyId);
                if (team_Journey == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content", Json(new { Message = "error", Status = false }).Content));

                var journeyMember = objJourney.GetTeamJourneyMember(teamJourneyId);
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
                            EmailAddress = team_journey_member.member.EmailAddress,
                            CreatedBy = team_journey_member.member.CreatedBy,
                            CreatedDate = team_journey_member.member.CreatedDate
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
                        Name = team_Journey.team.Name
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

                mission mission = new mission();
                string startdate = Convert.ToString(json["startDate"]);
                string enddate = Convert.ToString(json["endDate"]);
                int teamJourneyId = Convert.ToInt32(json["teamJourneyId"]);
                mission.TeamJourneyId = teamJourneyId;
                JArray practiceid = JArray.Parse(json["practiceId"].ToString());


                var availablemission = objMission.GetMission(mission);
                if (availablemission != null)
                {
                    var missionavailable = objMission.GetMissioAvailable(teamJourneyId);
                    if (missionavailable != null)
                    {
                        return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Existing mission is not complete", Status = false }).Content));
                    }
                }

                if (string.IsNullOrEmpty(startdate)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing StartDate field", Status = false }).Content));
                else if (string.IsNullOrEmpty(enddate)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing EndDate field", Status = false }).Content));

                if (Convert.ToDateTime(startdate) <= Convert.ToDateTime(enddate))
                {
                    var missionname = objMission.GetMissionName(teamJourneyId);
                    var jsonMission = JObject.Parse(JsonConvert.SerializeObject(missionname));
                    var MissionNamedet = jsonMission["Name"] + " " + jsonMission["TeamName"];

                    mission = new mission();
                    mission.TeamJourneyId = teamJourneyId;
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
                                result = objMission.AddorUpdateMissionPractice(mission_practice);
                            }
                        }

                        return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { missionId = missionDetail.MissionId, Message = "Mission has been created successfully..!", Status = true }).Content));
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, please try again later!", Status = false }).Content));
                    }
                }
                else
                {
                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "End date greater then to Start date.. ", Status = false }).Content));
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

    }
}
