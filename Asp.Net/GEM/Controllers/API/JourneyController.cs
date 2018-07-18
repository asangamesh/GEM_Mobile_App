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
    public class JourneyController : ApiController
    {
        JourneyServices objJourney = new JourneyServices();
        UserServices objUser = new UserServices();

        [HttpGet, Route("api/Journey")]
        public IHttpActionResult GetJourney()
        {
            try
            {
                var list = new List<object>();

                foreach (var item in Enum.GetValues(typeof(Journey_Information)))
                {
                    int id = (int)item;
                    list.Add(new
                    {
                        JourneyId = id,
                        Name = ((Journey_Information)id).ToString()
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

        [HttpGet, Route("api/Journey/{journeyId}")]
        public IHttpActionResult GetJourneyId(int journeyId)
        {
            try
            {
                var journey = objJourney.GetJourney(journeyId);
                if (journey == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));

                var teamJourney = new List<Models.Team_Journey>();
                var teamJourneymember = new List<Models.team_journey_member>();

                foreach (var team in journey.team_journey.ToList())
                {
                    teamJourneymember = new List<Models.team_journey_member>();
                    foreach (var team_journey_member in team.team_journey_member.OrderBy(x => x.TeamJourneyMemberRoleId).ToList())
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

                    teamJourney.Add(new Models.Team_Journey
                    {
                        TeamJourneyId = team.TeamJourneyId,
                        JourneyId = team.JourneyId,
                        TeamId = team.TeamId,
                        team = new Models.team
                        {
                            TeamId = team.team.TeamId,
                            Name = team.team.Name
                        },
                        team_journey_member = teamJourneymember
                    });
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", teamJourney, teamJourney.Count));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/TeamMember/{teamJourneyId}")]
        public IHttpActionResult GetTeamJourney(int teamJourneyId)
        {
            try
            {
                var objmission = new MissionServices();
                mission mission = new mission();
                mission.TeamJourneyId = teamJourneyId;
                var journeyMember = objJourney.GetTeamJourneyMember(teamJourneyId);
                var availablemission = objmission.GetMission(mission);
                if (availablemission != null)
                {
                    var missionavailable = objmission.GetMissioAvailable(teamJourneyId);
                    if (missionavailable != null)
                    {
                        return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Existing mission is not complete", Status = false }).Content));
                    }
                }
                if (journeyMember == null || journeyMember.Count == 1)
                {
                    return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Add member to team", Status = false }).Content));
                }
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

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { teamJourneymember, Status = true }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/Journey")]
        public IHttpActionResult Post([FromBody]dynamic data)
        {
            try
            {
                var result = 0;
                var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));

                string emailAddress = Convert.ToString(json["EmailAddress"]);
                string teamJourneyId = Convert.ToString(json["TeamJourneyId"]);
                string createdBy = Convert.ToString(json["MemberId"]);
                string role = Convert.ToString(json["Role"]);

                if (role == "1")
                {
                    var memberDetails = objUser.GetUserDetails(new member { MemberId = Convert.ToInt16(createdBy) });
                    if (memberDetails != null) emailAddress = memberDetails.EmailAddress;
                }

                if (string.IsNullOrEmpty(emailAddress)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing email address field", Status = false }).Content));
                else if (string.IsNullOrEmpty(emailAddress)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing team journey id field", Status = false }).Content));
                else if (string.IsNullOrEmpty(createdBy)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing member id field", Status = false }).Content));
                else if (string.IsNullOrEmpty(role)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing Role field", Status = false }).Content));

                member member = new member();
                member.EmailAddress = emailAddress;
                member.CreatedBy = Convert.ToInt16(createdBy);
                member.CreatedDate = DateTime.Now;

                var UserDetails = objUser.GetLoginUser(member);
                if (UserDetails == null)
                {
                    result = objUser.AddorUpdateUser(member);
                    UserDetails = objUser.GetLoginUser(member);
                }

                team_journey_member team_journey_member = new team_journey_member();
                team_journey_member.TeamJourneyId = Convert.ToInt32(teamJourneyId);
                team_journey_member.MemberId = UserDetails.MemberId;
                team_journey_member.TeamJourneyMemberRoleId = Convert.ToInt16(role);  // 2 - Team_Member
                result = objJourney.AddorUpdatetteamjourneymember(team_journey_member);

                if (result == 1)
                {
                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "your request is saved", Status = true }).Content));
                }
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved", Status = false }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }

        }

        [HttpGet, Route("api/MissionJourney")]
        public IHttpActionResult GetMission(int memberId)
        {
            try
            {
                var teams = objJourney.GetTeamMission(memberId).ToList();

                var tjgroup = objJourney.GetTeamMission(memberId).GroupBy(x => x.TeamJourneyId).ToList();
                var teamJourney = new List<Models.Team_Journey>();

                foreach (var tj in tjgroup)
                {
                    var team = tj.ToList().FirstOrDefault();

                    var teamJourneymember = new List<Models.team_journey_member>();
                    foreach (var team_journey_member in team.team_journey_member.OrderBy(x => x.TeamJourneyMemberRoleId).ToList())
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

                    var teamMission = new List<Models.mission>();
                    foreach (var team_mission in team.missions.OrderByDescending(x => x.MissionId).ToList())
                    {
                        if(team_mission.EndDate < DateTime.Now) break;

                        var missionPractice = new List<Models.mission_practice>();
                        foreach (var mission_practice in team_mission.  mission_practice.ToList())
                        {
                            if (mission_practice.member_mission_practice.Count == 0 || mission_practice.member_mission_practice.Where(m => m.MemberId == memberId).Count() == 0)
                            {
                                var measures = new List<Models.measure>();
                                foreach (var measure in mission_practice.practice.measures.ToList())
                                {
                                    var memberAssesment = new List<Models.mission_member_measure_assesment>();
                                    foreach (var assesment in measure.mission_member_measure_assesment.ToList())
                                    {
                                        if (memberId == assesment.MemberId.Value && team_mission.MissionId == assesment.MissionId)
                                        {
                                            memberAssesment.Add(new Models.mission_member_measure_assesment
                                            {
                                                MissionAssesmentId = assesment.MissionAssesmentId,
                                                MissionId = assesment.MissionId.Value,
                                                MeasureId = assesment.MeasureId.Value,
                                                MemberId = assesment.MemberId.Value,
                                                Assesment = assesment.Assesment.Value
                                            });
                                        }
                                    }

                                    measures.Add(new Models.measure
                                    {
                                        MeasureId = measure.MeasureId,
                                        PracticeId = measure.PracticeId.Value,
                                        Measure1 = measure.Measure1,
                                        Description = measure.Description,
                                        mission_member_measure_assesment = memberAssesment
                                    });
                                }

                                missionPractice.Add(new Models.mission_practice
                                {
                                    MissionPracticeId = mission_practice.MissionPracticeId,
                                    MissionId = mission_practice.MissionId,
                                    PracticeId = mission_practice.PracticeId,
                                    practice = new Models.Practice
                                    {
                                        PracticeId = mission_practice.practice.PracticeId,
                                        Name = mission_practice.practice.Name,
                                        FluencyLevelId = mission_practice.practice.FluencyLevelId.Value,
                                        fluency_level = new Models.fluency_level
                                        {
                                            FluencyLevelId = mission_practice.practice.fluency_level.FluencyLevelId,
                                            Number = mission_practice.practice.fluency_level.Number,
                                            Name = mission_practice.practice.fluency_level.Name,
                                            ShortName = mission_practice.practice.fluency_level.ShortName
                                        },
                                        measures = measures
                                    }
                                });
                            }
                        }

                        teamMission.Add(new Models.mission
                        {
                            TeamJourneyId = team_mission.TeamJourneyId,
                            MissionId = team_mission.MissionId,
                            Name = team_mission.Name,
                            StartDate = team_mission.StartDate,
                            EndDate = team_mission.EndDate,
                            MissionPractice = missionPractice
                        });
                        break;
                    }

                    if (teamMission.Count > 0)
                    {
                        teamJourney.Add(new Models.Team_Journey
                        {
                            TeamJourneyId = team.TeamJourneyId,
                            JourneyId = team.JourneyId,
                            TeamId = team.TeamId,
                            team = new Models.team
                            {
                                TeamId = team.team.TeamId,
                                Name = team.team.Name
                            },
                            team_journey_member = teamJourneymember,
                            missions = teamMission
                        });
                    }
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", teamJourney, teamJourney.Count));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }
    }
}
