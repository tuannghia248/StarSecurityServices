using StarSecurityService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarSecurityService.Dao
{
    public class UserDao
    {
        StarSecurityDataDataContext db = null;

        public UserDao()
        {
            db = new StarSecurityDataDataContext();
        }

        public Account GetById(string userName)
        {
            return db.Accounts.SingleOrDefault(x => x.username == userName);
        }

        public int Login(string userName, string passWord)
        {
            var result = db.Accounts.SingleOrDefault(x => x.username == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.password == passWord)
                    return 1;
                else
                    return -1;
            }
        }
    }
}