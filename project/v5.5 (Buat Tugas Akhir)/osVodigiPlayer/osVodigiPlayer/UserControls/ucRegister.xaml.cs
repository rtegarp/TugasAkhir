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

namespace TugasAkhirClient.UserControls
{
    public partial class ucRegister : UserControl
    {
        Storyboard sbFadeIn;
        Storyboard sbFadeOut;

        public static readonly RoutedEvent RegisterClosedEvent = EventManager.RegisterRoutedEvent(
            "RegisterClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucRegister));

        public event RoutedEventHandler RegisterClosed
        {
            add { AddHandler(RegisterClosedEvent, value); }
            remove { RemoveHandler(RegisterClosedEvent, value); }
        }

        public ucRegister()
        {
            try
            {
                InitializeComponent();

                sbFadeIn = (Storyboard)FindResource("sbFadeIn");
                sbFadeOut = (Storyboard)FindResource("sbFadeOut");
                sbFadeOut.Completed += sbFadeOut_Completed;

                btnRegister.MouseLeftButtonUp += btnRegister_MouseLeftButtonUp;
                btnRegister.TouchUp += btnRegister_TouchUp;
            }
            catch { }
        }

        void sbFadeOut_Completed(object sender, EventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                RaiseEvent(new RoutedEventArgs(RegisterClosedEvent));
            }
            catch { }
        }

        void btnRegister_TouchUp(object sender, TouchEventArgs e)
        {
            RegisterClicked();
        }

        void btnRegister_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RegisterClicked();
        }

        private void RegisterClicked()
        {
            try
            {
                lblError.Text = String.Empty;

                if (String.IsNullOrEmpty(txtAccountName.Text.Trim()) || String.IsNullOrEmpty(txtPlayerName.Text.Trim()))
                {
                    lblError.Text = "Please enter Account and Player Names.";
                    return;
                }

                TugasAkhirClient.osVodigiWS.osVodigiServiceSoapClient ws = new TugasAkhirClient.osVodigiWS.osVodigiServiceSoapClient();
                ws.Endpoint.Address = new System.ServiceModel.EndpointAddress(new Uri(Utility.GetWebserviceURL()));

                // Validate the account
                osVodigiWS.Account account = ws.Account_GetByName(txtAccountName.Text.Trim());
                if (account == null)
                {
                    lblError.Text = "Invalid Account Name. Please retry.";
                    return;
                }

                PlayerConfiguration.configAccountID = account.AccountID;
                PlayerConfiguration.configAccountName = account.AccountName;

                // Validate the player
                osVodigiWS.Player player = ws.Player_GetByName(account.AccountID, txtPlayerName.Text.Trim());
                if (player == null)
                {
                    lblError.Text = "Invalid Player Name. Please retry.";
                    return;
                }

                PlayerConfiguration.configPlayerID = player.PlayerID;
                PlayerConfiguration.configPlayerName = player.PlayerName;

                // Set the remaining properties on PlayerConfiguration and save the configuration
                PlayerConfiguration.configIsPlayerInitialized = true;
                PlayerConfiguration.configIsDownloadApproved = false;

                PlayerConfiguration.SavePlayerConfiguration();

                // Since registration can cause accountid/playerid changes, delete the local schedule file
                ScheduleFile.DeleteScheduleFile();

                // Register the player at vodigi.com
                try
                {
                    VodigiWS.VodigiWSSoapClient vws = new VodigiWS.VodigiWSSoapClient();
                    vws.PlayerRegistered("PlayerRegistration");
                }
                catch { }

                FadeOut();
            }
            catch { lblError.Text = "Cannot connect to Vodigi Server. Please retry."; }
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
                sbFadeOut.Begin();
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                txtAccountName.Text = String.Empty;
                txtPlayerName.Text = String.Empty;
                lblError.Text = String.Empty;
                lblWebserviceUrl.Text = Utility.GetWebserviceURL();
            }
            catch { }
        }
    }
}
