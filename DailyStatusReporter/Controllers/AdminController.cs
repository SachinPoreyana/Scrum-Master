using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace DailyStatusReporter.Controllers
{
    public class AdminController : ApiController
    {
        //public HttpResponseMessage GetTeams()
        //{

        //}
        public MembershipUserCollection GetAllUsers()
        {
            int users;
            return Membership.GetAllUsers(100, 100, out users);
        }
    }
}
