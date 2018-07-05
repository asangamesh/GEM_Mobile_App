using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.BusinessEntities;
using System.Data.Entity.Migrations;

namespace GEM.BusinessLogics
{
    public class UserServices
    {
        gemEntities1 gemdb = new gemEntities1();

        public List<member> GetUsers()
        {
            gemdb = new gemEntities1();
            return (from u in gemdb.members select u).ToList();
        }

        public member GetLoginUser(member user)
        {
            gemdb = new gemEntities1();
            user.MemberId = 0;

            if (!string.IsNullOrEmpty(user.EmailAddress)) return GetUserDetails(user);
            else return null;
        }

        public member GetUserDetails(member user)
        {
            gemdb = new gemEntities1();
            var _userDetail = new member();

            if (user.MemberId > 0) _userDetail = (from u in gemdb.members.Where(u => u.MemberId == user.MemberId) select u).FirstOrDefault();
            else if (!string.IsNullOrEmpty(user.EmailAddress)) _userDetail = (from u in gemdb.members.Where(u => u.EmailAddress == user.EmailAddress) select u).FirstOrDefault();

            return _userDetail;
        }

        public int AddorUpdateUser(member user)
        {
            gemdb = new gemEntities1();
            gemdb.members.AddOrUpdate(user);
            return gemdb.SaveChanges();
        }
    }
}
    