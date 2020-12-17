using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CryptoCurrency;
using System.Net.Mail;
using System.Net;


namespace HomeAutomationService
{
    public partial class HomeAutomationService : ServiceBase
    {
        private int eventId = 1;
        CryptoCurrency.Helpers.Logger _logger;
        CryptoCurrency.Helpers.MySettings _mySettings;


        public HomeAutomationService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("HomeAutomationSource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "HomeAutomationSource", "HomeAutomationNewLog");
            }
            eventLog1.Source = "HomeAutomationSource";
            eventLog1.Log = "HomeAutomationNewLog";

            _logger = new CryptoCurrency.Helpers.Logger();
            _mySettings = new CryptoCurrency.Helpers.MySettings();

        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart.");
            // Set up a timer that triggers every minute.
            Timer timer = new Timer();
            timer.Interval = 300000; // 
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop.");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
            CryptoInfoService cryptoServ = new CryptoInfoService();
            bool alert;
            var myPairs = cryptoServ.GetMyPairsInfo(out alert);
            if (alert)//send message
            {
                var emailTo = _mySettings.ReadSetting("emailTo");
                if (!string.IsNullOrEmpty(emailTo))
                {
                    _logger.LogFile("Info: Send email to: " + emailTo);
                    var msg = cryptoServ.GetInfoJson(myPairs);
                    SendEmailAsync(emailTo, msg);
                }
                else
                {
                    _logger.LogFile("Error: emailTo is Empty.");
                }
            }
        }

        public void SendEmailAsync(string email, string msg, string subject = "")
        {
            Task t = Task.Run(async () =>
            {
                try
                {
                    var myEmailFrom = _mySettings.ReadSetting("emailFrom");
                    var myEmailPassw = _mySettings.ReadSetting("emailPassw");

                    // Initialization.  
                    var body = msg;
                    var message = new MailMessage();

                    // Settings.  
                    message.To.Add(new MailAddress(email));
                    message.From = new MailAddress(myEmailFrom);
                    message.Subject = !string.IsNullOrEmpty(subject) ? subject : "CRYPTO ALERT";
                    message.Subject = message.Subject + "___" + DateTime.Now.ToString();
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        // Settings.  
                        var credential = new NetworkCredential
                        {
                            UserName = myEmailFrom,
                            Password = myEmailPassw
                        };

                        // Settings.  
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com"; // "smtp.yandex.ru"; for tut.by
                        smtp.Port = Convert.ToInt32(587);//465
                        smtp.EnableSsl = true;
                        smtp.Timeout = 20000; //20s

                        // Sending  
                        await smtp.SendMailAsync(message);
                        //smtp.SendMailAsync(message);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogFile("Error: Send email: " + ex.Message);
                }

            });
            //t.Wait(); // Wait until the above task is complete, email is sent

        }
    }
}
