using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DailyStatusReporter.Controllers
{
    public class EmployeesController : ApiController
    {
        [Authorize]
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities employeeDbEntities = new EmployeeDBEntities())
            {
                DailyMailService mailScheduler = new DailyMailService();
                return employeeDbEntities.Employees?.ToList();
            }
        }
    }
}
