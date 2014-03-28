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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace TugasAkhirClient.UserControls
{
    public partial class ucSplash : UserControl
    {
        DispatcherTimer timersplash = new DispatcherTimer();

        Storyboard sbFadeIn;
        Storyboard sbFadeOut;

        public static readonly RoutedEvent SplashClosedEvent = EventManager.RegisterRoutedEvent(
            "SplashClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSplash));

        public event RoutedEventHandler SplashClosed
        {
            add { AddHandler(SplashClosedEvent, value); }
            remove { RemoveHandler(SplashClosedEvent, value); }
        }

        public static readonly RoutedEvent SplashFadeInCompleteEvent = EventManager.RegisterRoutedEvent(
            "SplashFadeInComplete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSplash));

        public event RoutedEventHandler SplashFadeInComplete
        {
            add { AddHandler(SplashFadeInCompleteEvent, value); }
            remove { RemoveHandler(SplashFadeInCompleteEvent, value); }
        }

        public ucSplash()
        {
            try
            {
                InitializeComponent();

                sbFadeIn = (Storyboard)FindResource("sbFadeIn");
                sbFadeOut = (Storyboard)FindResource("sbFadeOut");
                sbFadeOut.Completed += sbFadeOut_Completed;
                sbFadeIn.Completed += sbFadeIn_Completed;

                //timersplash = new DispatcherTimer();
                //timersplash.Interval = TimeSpan.FromSeconds(20);
                //timersplash.Tick += timersplash_Tick;
            }
            catch { }
        }

        void sbFadeIn_Completed(object sender, EventArgs e)
        {
            try
            {
                RaiseEvent(new RoutedEventArgs(SplashFadeInCompleteEvent));
            }
            catch { }
        }

        void sbFadeOut_Completed(object sender, EventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                RaiseEvent(new RoutedEventArgs(SplashClosedEvent));
            }
            catch { }
        }

//        void timersplash_Tick(object sender, EventArgs e)
//        {
//            try
//            {
//                FadeOut();
//            }
//            catch { }
//        }

        public void FadeIn()
        {
            try
            {
                gridMain.Opacity = 0;
                this.Visibility = Visibility.Visible;
                sbFadeIn.Begin();
                timersplash.Start();
            }
            catch { }
        }

        public void FadeOut()
        {
            try
            {
                timersplash.Stop();
                sbFadeOut.Begin();
            }
            catch { }
        }
    }
}
