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
        public bool Login(string userName, string passWord)
        {
            var result = db.Accounts.Count(x => x.username == userName && x.password == passWord);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}