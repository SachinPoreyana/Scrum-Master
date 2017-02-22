using System;
using System.Threading;
using System.Configuration;
using System.ServiceProcess;
using System.IO;

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
            this.WriteToFile("DailyMailSendder stopped");
            this.SheduleService();
        }

        private Timer scheduler;
        private void SheduleService()
        {
            try
            {
                scheduler = new Timer(new TimerCallback(ShedularCallBack));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                this.WriteToFile("DailyMailSenderLog Mode: " + mode + "   " + "{0}");
                //setting default value.
                DateTime scheduledTime = DateTime.MinValue;

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
                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                this.WriteToFile("DailyMailSenderService scheduled to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
                scheduler.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                WriteToFile("DailyMailSenderService Error on: {0} " + ex.Message + ex.StackTrace);
                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }

        private void ShedularCallBack(object e)
        {
            this.WriteToFile("DailyMailSenderLog: {0}");
            this.SheduleService();
        }

        private void WriteToFile(string message)
        {
            var path = "C://ServiceLog.text";
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