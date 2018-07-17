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
using System.Web;

namespace GEM.Controllers.API
{
    public class TeamController : ApiController
    {
        TeamServices objTeam = new TeamServices();
        JourneyServices objJourney = new JourneyServices();
        UserServices objUser = new UserServices();

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

        [HttpGet, Route("api/team/{teamId}")]
        public IHttpActionResult Get(int teamId)
        {
            try
            {
                var team = objTeam.GetTeamById(teamId);
                if (team == null) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "No team was found", Status = false }).Content));

                var teamMember = new List<object>();
                if (team.team_journey != null)
                {
                    foreach (var member in team.team_journey.FirstOrDefault().team_journey_member)
                    {
                        teamMember.Add(new
                        {
                            MemberId = member.member.MemberId,
                            EmailAddress = member.member.EmailAddress,
                            TeamRole = member.team_journey_member_role.Name,
                        });
                    }
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {
                    Team = new
                    {
                        TeamId = team.TeamId,
                        Name = team.Name,
                        Description = team.Description,
                        CreatedBy = team.CreatedBy,
                        CreatedDate = team.CreatedDate,
                        TeamMember = teamMember
                    },
                    Status = true
                }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/team")]
        public IHttpActionResult GetTeams(int journeyId, int memberId)
        {
            try
            {
                var teams = objJourney.GetTeams(journeyId, memberId);
                var teamName = objTeam.GetTeambyName(journeyId, memberId);

                var teamJourney = new List<Models.Team_Journey>();
                var teamJourneymember = new List<Models.team_journey_member>();
                var missions = new List<Models.mission>();

                foreach (var team in teams)
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

                    missions = new List<Models.mission>();
                    foreach (var mission in team.missions.ToList())
                    {
                        if (mission.EndDate > DateTime.Now)
                        {
                            missions.Add(new Models.mission
                            {
                                MissionId = mission.MissionId,
                                TeamJourneyId = mission.TeamJourneyId,
                                Name = mission.Name,
                                StartDate = mission.StartDate,
                                EndDate = mission.EndDate
                            });
                        }
                    }

                    teamJourney.Add(new Models.Team_Journey
                    {
                        TeamJourneyId = team.TeamJourneyId,
                        JourneyId = team.JourneyId,
                        TeamId = team.TeamId,
                        missions = missions,
                        team = new Models.team
                        {
                            TeamId = team.team.TeamId,
                            Name = team.team.Name
                        },
                        team_journey_member = teamJourneymember
                    });
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Instance = teamName.Count + 1, TeamJourney = teamJourney }).Content, teamJourney.Count));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/teamMission/{teamJourneyId}")]
        public IHttpActionResult GetMission(int teamJourneyId)
        {
            try
            {
                var mission = objJourney.GetTeamPractice(teamJourneyId);

                if (mission == null) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "No mission was found", Status = false }).Content));

                var teamMission = new Models.mission();
                var missionPractices = new List<Models.mission_practice>();

                foreach (var practice in mission.mission_practice)
                {
                    var measures = new List<Models.measure>();
                    foreach (var measure in practice.practice.measures.ToList())
                    {
                        measures.Add(new Models.measure
                        {
                            MeasureId = measure.MeasureId,
                            PracticeId = measure.PracticeId.Value,
                            Measure1 = measure.Measure1,
                            Description = measure.Description
                        });
                    }

                    missionPractices.Add(new Models.mission_practice
                    {
                        MissionPracticeId = practice.MissionPracticeId,
                        MissionId = practice.MissionId,
                        PracticeId = practice.PracticeId,
                        practice = new Models.Practice
                        {
                            PracticeId = practice.practice.PracticeId,
                            Name = practice.practice.Name,
                            FluencyLevelId = practice.practice.FluencyLevelId.Value,
                            SequenceNum = practice.practice.SequenceNum.Value,
                            PrerequisiteNum = practice.practice.PrerequisiteNum.Value,
                            fluency_level = new Models.fluency_level
                            {
                                FluencyLevelId = practice.practice.fluency_level.FluencyLevelId,
                                Name = practice.practice.fluency_level.Name,
                                Number = practice.practice.fluency_level.Number,
                                ShortName = practice.practice.fluency_level.ShortName
                            },
                            measures = measures
                        }
                    });
                };

                teamMission = new Models.mission
                {
                    MissionId = mission.MissionId,
                    TeamJourneyId = mission.TeamJourneyId,
                    Name = mission.Name,
                    StartDate = mission.StartDate,
                    EndDate = mission.EndDate,
                    MissionPractice = missionPractices,
                    Team = new Models.team
                    {
                        TeamId = mission.team_journey.team.TeamId,
                        Name = mission.team_journey.team.Name,
                    }
                };

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", teamMission, null));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/memberMission/{memberid}")]
        public IHttpActionResult GetMissionbyMember(int memberid)
        {
            try
            {
                var missions = objJourney.GetMemberPractice(memberid);

                var gMissions = missions.GroupBy(x => x.MissionId).ToList();

                if (missions == null) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "No mission was found", Status = false }).Content));

                var teamMission = new List<Models.mission>();
                var missionPractices = new List<Models.mission_practice>();

                foreach (var gMission in gMissions)
                {
                    var mission = gMission.ToList().FirstOrDefault();

                    foreach (var practice in mission.mission_practice)
                    {
                        var measures = new List<Models.measure>();
                        foreach (var measure in practice.practice.measures.ToList())
                        {
                            var memberAssesment = new List<Models.mission_member_measure_assesment>();
                            foreach (var assesment in measure.mission_member_measure_assesment.ToList())
                            {
                                if (memberid == assesment.MemberId.Value)
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

                        missionPractices.Add(new Models.mission_practice
                        {
                            MissionPracticeId = practice.MissionPracticeId,
                            MissionId = practice.MissionId,
                            PracticeId = practice.PracticeId,
                            practice = new Models.Practice
                            {
                                PracticeId = practice.practice.PracticeId,
                                Name = practice.practice.Name,
                                FluencyLevelId = practice.practice.FluencyLevelId.Value,
                                SequenceNum = practice.practice.SequenceNum.Value,
                                PrerequisiteNum = practice.practice.PrerequisiteNum.Value,
                                fluency_level = new Models.fluency_level
                                {
                                    FluencyLevelId = practice.practice.fluency_level.FluencyLevelId,
                                    Name = practice.practice.fluency_level.Name,
                                    Number = practice.practice.fluency_level.Number,
                                    ShortName = practice.practice.fluency_level.ShortName
                                },
                                measures = measures
                            }
                        });
                    };

                    teamMission.Add(new Models.mission
                    {
                        MissionId = mission.MissionId,
                        TeamJourneyId = mission.TeamJourneyId,
                        Name = mission.Name,
                        StartDate = mission.StartDate,
                        EndDate = mission.EndDate,
                        MissionPractice = missionPractices,
                        Journey = new Models.JourneyInformation
                        {
                            JourneyId = mission.team_journey.JourneyId.Value,
                            Name = Utilities.HelperEnum.JourneyInformation(true)[mission.team_journey.JourneyId.Value - 1]
                        },
                        Team = new Models.team
                        {
                            TeamId = mission.team_journey.team.TeamId,
                            Name = mission.team_journey.team.Name,
                        }
                    });
                }

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", teamMission, teamMission.Count));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/team")]
        public IHttpActionResult PostTeam([FromBody]dynamic data)
        {
            try
            {
                var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));

                string Id = Convert.ToString(json["TeamId"]);
                string name = Convert.ToString(json["Name"]);
                string journeyId = Convert.ToString(json["JourneyId"]);
                string createdBy = Convert.ToString(json["MemberId"]);

                if (string.IsNullOrEmpty(name)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing team name field", Status = false }).Content));
                else if (string.IsNullOrEmpty(journeyId)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing journey id field", Status = false }).Content));
                else if (string.IsNullOrEmpty(createdBy)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing member id field", Status = false }).Content));

                team team = new team();
                if (!string.IsNullOrEmpty(Id))
                {
                    team.TeamId = Convert.ToInt16(Id);
                    team = objTeam.GetTeamById(team.TeamId);
                    team.UpdatedBy = Convert.ToInt16(createdBy);
                    team.UpdatedDate = DateTime.Now;
                }
                else
                {
                    team.CreatedBy = Convert.ToInt16(createdBy);
                    team.CreatedDate = DateTime.Now;
                }
                team.Name = name;

                var result = objTeam.AddorUpdateTeam(team);
                if (result == 1)
                {
                    var Team = objTeam.GetTeam(team);

                    team_journey teamJourney = new team_journey();
                    teamJourney.TeamId = Team[0].TeamId;
                    teamJourney.JourneyId = Convert.ToInt32(journeyId);
                    if (string.IsNullOrEmpty(Id)) result = objJourney.AddorUpdatetTeamJourney(teamJourney);

                    var team_journey = objJourney.GetTeamJourney(teamJourney.TeamId.Value, teamJourney.JourneyId.Value);

                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { TeamJourneyId = team_journey.TeamJourneyId, TeamId = teamJourney.TeamId, Message = string.IsNullOrEmpty(Id) ? "New team is created successfully!..." : "Team details are updated!...", Status = true }).Content));
                }

                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, try again later!", Status = false }).Content));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/teamMember")]
        public IHttpActionResult PostMember([FromBody]dynamic data)
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

                var isMemberAlreadyExist = false;

                var teamJourneyMember = objJourney.GetTeamJourneyMember(Convert.ToInt32(teamJourneyId));
                if (teamJourneyMember != null && teamJourneyMember.Count > 0)
                {
                    isMemberAlreadyExist = teamJourneyMember.Where(m => m.MemberId.Equals(UserDetails.MemberId)).Count() > 0;
                }

                team_journey_member team_journey_member = new team_journey_member();
                team_journey_member.TeamJourneyId = Convert.ToInt32(teamJourneyId);
                team_journey_member.MemberId = UserDetails.MemberId;
                team_journey_member.TeamJourneyMemberRoleId = Convert.ToInt16(role);  // 2 - Team_Member
                if (!isMemberAlreadyExist) result = objJourney.AddorUpdatetteamjourneymember(team_journey_member);
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = emailAddress + " member is already added", Status = false }).Content));

                if (result == 1)
                {
                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "New team member is added.", Status = true }).Content));
                }
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved.", Status = false }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/teamAvatar")]
        public IHttpActionResult PostAvatar([FromBody]dynamic data)
        {
            try
            {
                //var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));
                byte[] str = System.Text.Encoding.Unicode.GetBytes(data);
                var buffer = Convert.FromBase64String(Convert.ToBase64String(str));
                var file = HttpContext.Current.Server.MapPath("~/Content/images/err.png");
                System.IO.File.WriteAllBytes(file, buffer);

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, try again later!", Status = false }).Content));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/MissionAssesment")]
        public IHttpActionResult PostAssesment([FromBody]dynamic data)
        {
            try
            {
                var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));
                JArray measures = JArray.Parse(json["Assesments"].ToString());
                if (measures.Count > 0)
                {
                    foreach (var measure in measures)
                    {
                        var memberMeasure = new mission_member_measure_assesment();
                        memberMeasure.MissionAssesmentId = Convert.ToInt32(measure["missionAssesmentId"]);
                        memberMeasure.MissionId = Convert.ToInt32(measure["missionId"]);
                        memberMeasure.MemberId = Convert.ToInt32(measure["memberId"]);
                        memberMeasure.MeasureId = Convert.ToInt32(measure["measureId"]);
                        memberMeasure.Assesment = Convert.ToInt32(measure["assesment"]);
                        var result = objTeam.AddorUpdateTeamMemberMeasure(memberMeasure);
                    }

                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your observation records have been saved!...", Status = true }).Content));
                }
                else
                {
                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "You have missing observation details.", Status = false }).Content));
                }

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpDelete, Route("api/teamMember/{tjmemberId}")]
        public IHttpActionResult DeleteTeamMemberId(int tjmemberId)
        {
            try
            {
                var objTeam = new TeamServices();
                team_journey_member objtjmember = objTeam.GetTeamJourneyMember(tjmemberId);
                if (objtjmember == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));

                var journey = objTeam.DeleteTeamMember(objtjmember);

                if (journey == 1) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { MemberID = objtjmember.member.EmailAddress, Message = "Member has been removed from team", Status = true }).Content));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { MemberID = objtjmember.member.EmailAddress, Message = "The request process does not completed please try again!", Status = true }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPut, Route("api/teamMember/{tjmemberId}")]
        public IHttpActionResult UpdateMemberRole(int tjmemberId)
        {
            try
            {
                team_journey_member objtjmember = objTeam.GetTeamJourneyMember(tjmemberId);
                if (objtjmember == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));

                var leaders = objJourney.GetTeamJourneyMember(objtjmember.TeamJourneyId.Value).Where(x => x.TeamJourneyMemberRoleId == 1).ToList();
                foreach (var leader in leaders)
                {
                    leader.TeamJourneyMemberRoleId = 2;
                    objJourney.AddorUpdatetteamjourneymember(leader);
                }

                objtjmember.TeamJourneyMemberRoleId = 1;
                var journey = objJourney.AddorUpdatetteamjourneymember(objtjmember);

                if (journey == 1) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { MemberID = objtjmember.member.EmailAddress, Message = "Member has been removed from team", Status = true }).Content));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { MemberID = objtjmember.member.EmailAddress, Message = "The request process does not completed please try again!", Status = true }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }
    }
}
