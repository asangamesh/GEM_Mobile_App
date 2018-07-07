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

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {
                    Team = new
                    {
                        TeamId = team.TeamId,
                        Name = team.Name,
                        Description = team.Description,
                        CreatedBy = team.CreatedBy,
                        CreatedDate = team.CreatedDate
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

                foreach (var team in teams)
                {
                    teamJourneymember = new List<Models.team_journey_member>();
                    foreach (var team_journey_member in team.team_journey_member.OrderBy(x=>x.TeamJourneyMemberRoleId).ToList())
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

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", teamJourney, teamName.Count));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/team")]
        public IHttpActionResult Post([FromBody]dynamic data)
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

                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { TeamJourneyId = team_journey.TeamJourneyId, Message = "your request is saved", Status = true }).Content));
                }

                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, try again later!", Status = false }).Content));

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/teamAvatar")]
        public IHttpActionResult PostAvatar(HttpPostedFileBase file)
        {
            try
            {
                //var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));

                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "Your request is not saved, try again later!", Status = false }).Content));

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
              
                if(journey == 1) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { MemberID = objtjmember.member.EmailAddress, Message = "Member has been removed from team", Status = true }).Content));
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

                var leaders = objJourney.GetTeamJourneyMember(objtjmember.TeamJourneyId.Value).Where(x=>x.TeamJourneyMemberRoleId == 1).ToList();
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
