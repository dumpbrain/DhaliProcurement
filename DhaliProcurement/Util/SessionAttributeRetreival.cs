using AuditReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AuditReport.Util
{
    public class SessionAttributeRetreival
    {
        private EPARSDbContext db = new EPARSDbContext();

        public UsersRole getStoredUserPermission()
        {
            string username = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            User  appuser = db.User.SingleOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

            try
            {
                UsersRole userpermission = db.UsersRole.SingleOrDefault(u => u.UserId == appuser.Id);
                return userpermission;
            }
            catch (NullReferenceException exp)
            {
                return null;
            }

        }

    }
}