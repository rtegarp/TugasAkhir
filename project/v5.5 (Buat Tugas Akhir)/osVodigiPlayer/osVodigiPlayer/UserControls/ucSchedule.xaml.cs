using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.Linq;
using System.IO;
using System.Configuration;

namespace TugasAkhirClient.UserControls
{
    public partial class ucSchedule : UserControl
    {
        Storyboard sbFadeIn;
        Storyboard sbFadeOut;

        string countdownlabel = "Retrying schedule download in 30 seconds";
        string secondslabel = "Waiting: x seconds";

        DispatcherTimer timerschedule;
        DispatcherTimer timercountdown;
        int seconds = 0;
        int countdown = 30;

        public static readonly RoutedEvent ScheduleClosedEvent = EventManager.RegisterRoutedEvent(
            "ScheduleClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSchedule));

        public event RoutedEventHandler ScheduleClosed
        {
            add { AddHandler(ScheduleClosedEvent, value); }
            remove { RemoveHandler(ScheduleClosedEvent, value); }
        }

        public ucSchedule()
        {
            try
            {
                InitializeComponent();

                sbFadeIn = (Storyboard)FindResource("sbFadeIn");
                sbFadeOut = (Storyboard)FindResource("sbFadeOut");
                sbFadeOut.Completed += sbFadeOut_Completed;
                sbFadeIn.Completed += sbFadeIn_Completed;

                btnRetry.MouseLeftButtonUp += btnRetry_MouseLeftButtonUp;
                btnRetry.TouchUp += btnRetry_TouchUp;

                btnUseLast.MouseLeftButtonUp += btnUseLast_MouseLeftButtonUp;
                btnUseLast.TouchUp += btnUseLast_TouchUp;

                timerschedule = new DispatcherTimer();
                timerschedule.Interval = TimeSpan.FromSeconds(1);
                timerschedule.Tick += timerschedule_Tick;

                timercountdown = new DispatcherTimer();
                timercountdown.Interval = TimeSpan.FromSeconds(1);
                timercountdown.Tick += timercountdown_Tick;
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                timercountdown.Stop();
                timerschedule.Stop();

                // Display the big counter - hide everything else
                lblCounter.Text = secondslabel.Replace("x", "0");
                lblCounter.Visibility = Visibility.Visible;

                lblErrorLabel.Visibility = Visibility.Collapsed;
                lblError.Visibility = Visibility.Collapsed;

                btnRetry.Visibility = Visibility.Collapsed;
                btnUseLast.Visibility = Visibility.Collapsed;

                lblRetryTime.Visibility = Visibility.Collapsed;
                lblRetryTime.Text = countdownlabel;

                // Start the schedule timer
                seconds = 0;
                timerschedule.Start();
            }
            catch { }
        }

        private void DisplayErrorCondition()
        {
            try
            {
                timerschedule.Stop();

                // Display the error message and buttons
                lblCounter.Text = secondslabel.Replace("x", "0");
                lblCounter.Visibility = Visibility.Collapsed;

                lblErrorLabel.Visibility = Visibility.Visible;
                lblError.Visibility = Visibility.Visible;
                btnRetry.Visibility = Visibility.Visible;
                btnUseLast.Visibility = Visibility.Visible;

                lblRetryTime.Visibility = Visibility.Visible;

                // Start the countdown timer
                countdown = 30;
                timercountdown.Start();
            }
            catch { }
        }

        void timerschedule_Tick(object sender, EventArgs e)
        {
            try
            {
                seconds += 1;
                lblCounter.Text = secondslabel.Replace("x", seconds.ToString());
            }
            catch { }
        }

        void timercountdown_Tick(object sender, EventArgs e)
        {
            try
            {
                countdown -= 1;
                lblRetryTime.Text = countdownlabel.Replace("30", countdown.ToString());

                // At zero seconds, retry the schedule
                if (countdown == 0)
                    RetryClicked();
            }
            catch { }
        }

        void sbFadeOut_Completed(object sender, EventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                RaiseEvent(new RoutedEventArgs(ScheduleClosedEvent));
            }
            catch { }
        }

        void sbFadeIn_Completed(object sender, EventArgs e)
        {
            try
            {
                GetSchedule();
            } 
            catch { }
        }

        void btnRetry_TouchUp(object sender, TouchEventArgs e)
        {
            RetryClicked();
        }

        void btnRetry_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RetryClicked();
        }

        void btnUseLast_TouchUp(object sender, TouchEventArgs e)
        {
            UseLastClicked();
        }

        void btnUseLast_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UseLastClicked();
        }

        private void RetryClicked()
        {
            try
            {
                ResetControl();
                GetSchedule();
            }
            catch { }
        }

        private void UseLastClicked()
        {
            try
            {
                string xml = ScheduleFile.ReadScheduleFile();
                if (String.IsNullOrEmpty(xml))
                {
                    MessageBox.Show("Unable to load previous schedule. Please use the 'Retry' button.", "No Previous Schedule", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CurrentSchedule.ParseScheduleXml(xml);
                FadeOut();
            }
            catch { }
        }

        public void FadeIn()
        {
            try
            {
                ResetControl();
                gridMain.Opacity = 0;
                this.Visibility = Visibility.Visible;
                sbFadeIn.Begin();
            }
            catch { }
        }

        public void FadeOut()
        {
            try
            {
                timerschedule.Stop();
                timercountdown.Stop();

                sbFadeOut.Begin();
            }
            catch { }
        }

        async void GetSchedule()
        {
            try
            {
                // Get the schedule asychronously
                TugasAkhirClient.osVodigiWS.osVodigiServiceSoapClient ws = new TugasAkhirClient.osVodigiWS.osVodigiServiceSoapClient();
                ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(Utility.GetWebserviceURL()));

                TugasAkhirClient.osVodigiWS.Player_GetCurrentScheduleResponse scheduleResponse = await ws.Player_GetCurrentScheduleAsync(PlayerConfiguration.configPlayerID);
                string xml = scheduleResponse.Body.Player_GetCurrentScheduleResult;

                if (xml.StartsWith("<xml><Error>"))
                    throw new Exception("Error");

                ScheduleFile.SaveScheduleFile(xml);

                CurrentSchedule.ClearSchedule();
                CurrentSchedule.LastScheduleXML = xml;
                CurrentSchedule.ParseScheduleXml(xml);

                // At this point, the schedule has been retrieved, so go to the Download control
                FadeOut();
            }
            catch
            {
                DisplayErrorCondition();
            }
        }
    }
}
