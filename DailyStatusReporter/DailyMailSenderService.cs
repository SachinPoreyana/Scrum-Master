using System;
using System.Threading;
using System.Configuration;
using System.ServiceProcess;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using System.Globalization;

namespace DailyStatusReporter
{
    public partial class DailyMailSenderService: ServiceBase
    {
        public DailyMailSenderService ()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DailyMailService mailScheduler = new DailyMailService();
            WriteToFile("DailyMailSender started");
            SheduleService();
        }

        protected override void OnStop()
        {
            WriteToFile("DailyMailService stopped {0}");
            scheduler.Dispose();
        }

        private static Timer scheduler;
        public static void SheduleService()
        {
            try
            {
                DailyMailService mailScheduler = new DailyMailService();
                
                scheduler = new Timer(new TimerCallback(ShedularCallBack));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                WriteToFile("DailyMailSenderLog Mode: " + mode + "   " + "{0}");
                //setting default value.
                DateTime scheduledTime = DateTime.MinValue;

                var statusReportExcel = CreateStatusSpreadSheet(GetStatusReports());
                mailScheduler.SendMail("sachinporedsfsdffssfdyana@yahoo.in", "steppershotty@gmail.com", "Daily Status report", "PFA the status report for the day", statusReportExcel);
                if (mode == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }
                if (mode.ToUpper() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }
                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                WriteToFile("DailyMailSenderService scheduled to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
                scheduler.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                WriteToFile("DailyMailSenderService Error on: {0} " + ex.Message + ex.StackTrace);
                //Stop the Windows Service.
            }
        }

        public static IEnumerable<StatusReport> GetStatusReports()
        {
            IEnumerable<StatusReport> satusReports = new List<StatusReport>();
            IEnumerable<StatusReport> satusReports1 = new List<StatusReport>();
            using (var employeeDBEntities = new EmployeeDBEntities())
            {
                var today = DateTime.Now;
                
                satusReports1 = employeeDBEntities.StatusReports.Where(x => x.Date != null)?.ToList();
                satusReports = employeeDBEntities.StatusReports.ToList().Where(x => x.Date.Value.ToString("yyyy/MM/dd") == today.ToString("yyyy/MM/dd"))?.ToList();
            }
            return satusReports;
        }

        public static ExcelWorksheet CreateStatusSpreadSheet(IEnumerable<StatusReport> statusReports)
        {
            string statusExcelSheetPath = Properties.Settings.Default.FolderPath + "\\statusExcelSheetPath.xlsx";
            File.Delete(statusExcelSheetPath);
            FileInfo statusExcelSheetInfo = new FileInfo(statusExcelSheetPath);
            ExcelPackage exp = new ExcelPackage(statusExcelSheetInfo);
            var statusWorksheet = exp.Workbook.Worksheets.Add("dailyStatusWorksheet");
            statusWorksheet.Cells["A1"].Value = "Employee name";
            statusWorksheet.Cells["B1"].Value = "UserStory";
            statusWorksheet.Cells["C1"].Value = "Task";
            statusWorksheet.Cells["D1"].Value = "Description";
            statusWorksheet.Cells["E1"].Value = "Completion stage";
            statusWorksheet.Cells["A1 : E1"].Style.Font.Bold = true;

            var currentRow = 2;

            foreach(var statusReport in statusReports)
            {
                statusWorksheet.Cells["A"+ currentRow.ToString()].Value = statusReport.NeedHelp;
                statusWorksheet.Cells["B" + currentRow.ToString()].Value = statusReport.UserStory;
                statusWorksheet.Cells["C" + currentRow.ToString()].Value = statusReport.Task;
                statusWorksheet.Cells["D" + currentRow.ToString()].Value = statusReport.Description;
                statusWorksheet.Cells["E" + currentRow.ToString()].Value = statusReport.CompletionStage;

                currentRow++;
            }
            exp.Save();
            return statusWorksheet;
        }

        

        private static void ShedularCallBack(object e)
        {
            WriteToFile("DailyMailSenderLog: {0}");
            SheduleService();
        }

        private static void WriteToFile(string message)
        {
            var path = "C:/code/ServiceLog.text";
            using(StreamWriter streamWriter = new StreamWriter(path, true))
            {
                streamWriter.WriteLine(string.Format(message, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                streamWriter.Close();
            }
        }

        private void InitializeComponent()
        {
            // 
            // DailyMailSenderService
            // 
            this.ServiceName = "DailyMailSenderService";

        }
    }
}