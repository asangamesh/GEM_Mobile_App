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
    public class JourneyController : ApiController
    {
        JourneyServices objJourney = new JourneyServices();
        UserServices objUser = new UserServices();

        // GET: api/Journey
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

        // GET: api/Journey/{journeyId}
        public IHttpActionResult GetJourneyId(int journeyId)
        {
            try
            {
                var journey = objJourney.GetJourney(journeyId);
                if (journey == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", journey));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        // GET: api/Journey/{teamId}
        public IHttpActionResult GetJourneyByTeamId(int teamId)
        {
            try
            {
                var journeys = objJourney.GetJourneybyTeamId(teamId);
                if (journeys == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", journeys));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        // GET: api/Journey/{memberId}
        public IHttpActionResult GetJourneyByMemberId(int MemberId)
        {
            try
            {
                var journeys = objJourney.GetJourneybyUserId(MemberId);
                if (journeys == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", journeys));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        // GET: api/Journey/{memberId}
        public IHttpActionResult GetTeamJourney(int journeyId, int memberId)
        {
            try
            {
                var journeys = objJourney.GetTeams(journeyId, memberId);
                if (journeys == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", journeys));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        // GET: api/Journey/{memberId}
        public IHttpActionResult GetTeamJourney(int teamJourneyId)
        {
            try
            {
                var journeys = objJourney.GetTeamJourneyMember(teamJourneyId);
                if (journeys == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));
                else return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Count = journeys.Count, TeamJourneyMember = journeys })));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        // PUT: api/Journey/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Content(HttpStatusCode.NotFound, CommonHelper.ResponseData("", 404, "NotFound"));
        }

        // POST: api/Journey
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
                    var memberDetails = objUser.GetUserDetails(new member { MemberId = Convert.ToInt16(createdBy)});
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
    }
}
