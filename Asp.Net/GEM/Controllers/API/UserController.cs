using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gem.BusinessEntities;
using GEM.BusinessLogics;
using System.Web.Http.Results;
using GEM.Helpers;
using Newtonsoft.Json;

namespace GEM.Controllers.API
{
    public class UserController : ApiController
    {
        UserServices objLogin = new UserServices();

        [HttpGet, Route("api/user")]
        public IHttpActionResult Get()
        {
            try
            {
                var Users = objLogin.GetUsers();

                var list = new List<object>();

                for (int i = 0; i < Users.Count(); i++)
                {
                    list.Add(new
                    {
                        UserId = Users[i].MemberId,
                        EmailAddress = Users[i].EmailAddress,
                        CreatedDate = Users[i].CreatedDate
                    });
                }
   
                return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                {
                    User =list,
                    Status = true
                }).Content));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/user")]
        public IHttpActionResult Get(string email)
        {
            try
            {
                var member = new member();
                member.EmailAddress = email;

                var loginUser = objLogin.GetLoginUser(member);
                if (loginUser == null)
                {
                    member.CreatedDate = DateTime.Now;
                    var result = objLogin.AddorUpdateUser(member);

                    loginUser = objLogin.GetLoginUser(member);
                }

                if (loginUser == null) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "There isn't an account for this email address", Status = false }).Content));
                else
                {
                    var teamMemberRole = new List<object>();
                    foreach (var teamJourneyMemberRole in loginUser.team_journey_member.ToList()) teamMemberRole.Add(new
                    {
                        TeamJourneyId = teamJourneyMemberRole.team_journey.TeamJourneyId,
                        JourneyId = teamJourneyMemberRole.team_journey.JourneyId,
                        TeamId = teamJourneyMemberRole.team_journey.TeamId,
                        TeamName = teamJourneyMemberRole.team_journey.team.Name,
                        MemberRoleId = teamJourneyMemberRole.team_journey_member_role.TeamJourneyMemberRoleId,
                        MemberRole = teamJourneyMemberRole.team_journey_member_role.Name
                    });

                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                    {
                        User = new
                        {
                            UserId = loginUser.MemberId,
                            EmailAddress = loginUser.EmailAddress,
                            CreatedDate = loginUser.CreatedDate,
                            TeamMemberRole = teamMemberRole
                        },
                        Status = true
                    }).Content, null));
                }
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpGet, Route("api/user/{memberId}")]
        public IHttpActionResult GetUser(int memberId)
        {
            try
            {
                var loginUser = objLogin.GetUserDetails(new member { MemberId = memberId });
                if (loginUser == null) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "you entered member id is not found", Status = false }).Content));
                else
                {
                    var teamMemberRole = new List<object>();
                    foreach (var teamJourneyMemberRole in loginUser.team_journey_member.ToList()) teamMemberRole.Add(new
                    {
                        TeamId = teamJourneyMemberRole.team_journey.team.TeamId,
                        JourneyId = teamJourneyMemberRole.team_journey.JourneyId,
                        TeamName = teamJourneyMemberRole.team_journey.team.Name,
                        MemberRoleId = teamJourneyMemberRole.team_journey_member_role.TeamJourneyMemberRoleId,
                        MemberRole = teamJourneyMemberRole.team_journey_member_role.Name
                    });

                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new
                    {
                        User = new
                        {
                            UserId = loginUser.MemberId,
                            EmailAddress = loginUser.EmailAddress,
                            CreatedDate = loginUser.CreatedDate,
                            TeamMemberRole = teamMemberRole
                        },
                        Status = true
                    }).Content, null));
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, CommonHelper.ResponseData(ex.Message, 500, "Internal Server Error"));
            }
        }

        [HttpPost, Route("api/user")]
        public IHttpActionResult Post([FromBody]dynamic data)
        {
            try
            {
                if(data == null) return Content(HttpStatusCode.NoContent, CommonHelper.ResponseData("", 204, "No Content"));
                member user = JsonConvert.DeserializeObject<member>(JsonConvert.SerializeObject(data));
                user.CreatedDate = DateTime.Now;

                if (string.IsNullOrEmpty(user.EmailAddress)) return Content(HttpStatusCode.BadRequest, CommonHelper.ResponseData("", 400, "Bad Request", Json(new { Message = "Missing EmailAddress field", Status = false }).Content));

                var loginUser = objLogin.GetLoginUser(user);
                if (loginUser != null) return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { Message = "you entered email address is already exists!", Status = false }).Content));

                user.CreatedDate = DateTime.Now;
                var result = objLogin.AddorUpdateUser(user);

                if (result == 1)
                {
                    var _user = objLogin.GetLoginUser(new member { EmailAddress = user.EmailAddress });
                    return Content(HttpStatusCode.OK, CommonHelper.ResponseData("", 200, "OK", Json(new { UserID = _user.MemberId, Message = "your request is saved", Status = true }).Content));
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
