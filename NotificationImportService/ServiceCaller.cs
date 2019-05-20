using NotificationImportService.DataObject;
using NotificationImportService.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NotificationImportService
{
    public partial class ServiceCaller : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Timer timer = new Timer();
        int numberOfDaysToProcess = 3;
        public ServiceCaller()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();

            timer.Enabled = false;
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
        }
        protected override void OnStart(string[] args)
        {
            int interval = 60000; //every minute

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["POLLING_INTERVAL_SECONDS"]))
            {
                try
                {
                    interval = Convert.ToInt32(ConfigurationManager.AppSettings["POLLING_INTERVAL_HOURS"]) * 3600000;
                    timer.Interval = interval;
                }
                catch { }
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MESSAGES_INTERVAL_DAYS"]))
            {
                try
                {
                    numberOfDaysToProcess = Convert.ToInt32(ConfigurationManager.AppSettings["MESSAGES_INTERVAL_DAYS"]);
                }
                catch { }
            }

            log.Debug(String.Format("Checking for outbound push notifications messages created in the last {0} days; every {1} ms", numberOfDaysToProcess, interval));

            Start();
        }

        private void Start()
        {
            try
            {
                ProcessMessages();
                timer.Start();

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        protected override void OnStop()
        {
            try
            {
                log.Info("stopped");
                timer.Stop();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected override void OnContinue()
        {
            try
            {
                log.Info("continuing - checking for outbound push notifications");
                ProcessMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        protected override void OnPause()
        {
            try
            {
                log.Info("pausing");
                timer.Stop();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            ProcessMessages();
        }

        private void ProcessMessages()
        {
            try
            {
                var RestResponse = RestService.RestClientCall<NotificationResponse>(ConfigurationManager.AppSettings["KYCUrl"], "/AppTransactionsNotifications", RestSharp.Method.GET, null, true);
                if(RestResponse?.Value != null)
                {
                    if(RestResponse?.Value.Count > 0)
                    {
                        var index = 0;
                        foreach(var i in RestResponse.Value)
                        {
                            if (!string.IsNullOrEmpty(i.PhoneNo))
                            {
                                index++;
                                DataService.RecordNotification(i);
                            }
                        }
                        log.Info(index + " Notification(s) imported");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                timer.Start();
            }

        }
    }
}
