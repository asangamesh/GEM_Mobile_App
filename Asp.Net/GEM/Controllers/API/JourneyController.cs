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

        // GET: api/Journey/{teamId}
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
                var json = (JToken)JObject.Parse(JsonConvert.SerializeObject(data));
                var count = 0;
                
                var JourneyDet = objJourney.GetJourney(Convert.ToInt32(json["JourneyId"]));

                member memer = new member();
                memer.EmailAddress = Convert.ToString(json["Name"]);
                memer.CreatedBy = Convert.ToInt16(JourneyDet.CreatedBy);
                memer.CreatedDate = DateTime.Now;
                count = objUser.AddorUpdateUser(memer);

                var UserDetails= objUser.GetLoginUser(memer);

                team_journey team_journey = new team_journey();
                team_journey.TeamJourneyId = 25;
                team_journey.TeamId = Convert.ToInt32(json["TeamId"]);
                team_journey.JourneyId = Convert.ToInt32(json["JourneyId"]);
                count += objJourney.AddorUpdatetTeamJourney(team_journey);

                team_journey_member team_journey_member = new team_journey_member();
                team_journey_member.TeamJourneyId = 25;
                team_journey_member.MemberId = UserDetails.MemberId;
                team_journey_member.TeamJourneyMemberRoleId = 2;  // 2 - Team_Member
                count += objJourney.AddorUpdatetteamjourneymember(team_journey_member);

                var journey = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(data));
                journey.CreatedDate = DateTime.Now;

                if (string.IsNullOrEmpty(journey.MemberId)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing MemberId field", Status = false }).Content));

                var result = objJourney.AddorUpdateJourney(journey);

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
