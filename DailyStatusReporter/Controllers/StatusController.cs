using DailyStatusReporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScrumMaster.Controllers
{
    [Authorize]
    public class StatusReportController : ApiController
    {
        public void Post([FromBodyAttribute] StatusReport Status)
        {
            using (var employeeDBEntities = new EmployeeDBEntities())
            {
                employeeDBEntities.StatusReports.Add(Status);
                employeeDBEntities.SaveChanges();
            }
        }

        public void Put(int id, [FromBody] StatusReport Status)
        {
            using (var employeeDBEntities = new EmployeeDBEntities())
            {
                var entity = employeeDBEntities.StatusReports.FirstOrDefault(e => e.ID == id);
                entity.ID = Status.ID;
                entity.EmpId = Status.EmpId;
                entity.Description = Status.Description;
                entity.NeedHelp = Status.NeedHelp;
                entity.UserStory = Status.UserStory;
                entity.Task = Status.Task;
                entity.CompletionStage = Status.CompletionStage;
                employeeDBEntities.SaveChanges();
            }
        }



        public IEnumerable<StatusReport> GetStatusReports()
        {
            using (var EmployeeDBEntities = new EmployeeDBEntities())
            {

                return EmployeeDBEntities.StatusReports.ToList();
            }

        }

        public IEnumerable<StatusReport> GetStatusReport(int id)
        {
            using (var EmployeeDBEntities = new EmployeeDBEntities())
            {

                return EmployeeDBEntities.StatusReports.Where(x => x.ID == id).ToList();
            }

        }

        public void DeleteStatusReport(int id)
        {
            using (var employeeDBEntities = new EmployeeDBEntities())
            {

                var entity = employeeDBEntities.StatusReports.Where(x => x.ID == id).FirstOrDefault();
                employeeDBEntities.StatusReports.Remove(entity);
                employeeDBEntities.SaveChanges();
            }
        }

        public void GetStatusReporttest()
        {
            DailyMailService mailScheduler = new DailyMailService();
            mailScheduler.SendMail("sachinporeyana@yahoo.in", "steppershotty@gmail.com", "Test email", "Hello");


        }

    }
}
