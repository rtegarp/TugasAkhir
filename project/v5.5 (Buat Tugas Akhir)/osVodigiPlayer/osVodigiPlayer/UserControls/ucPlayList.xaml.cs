using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TugasAkhirClient.UserControls
{
    public partial class ucPlayList : UserControl
    {
        // Complete Event
        public static readonly RoutedEvent PlayListCompleteEvent = EventManager.RegisterRoutedEvent(
            "PlayListComplete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucPlayList));

        public event RoutedEventHandler PlayListComplete
        {
            add { AddHandler(PlayListCompleteEvent, value); }
            remove { RemoveHandler(PlayListCompleteEvent, value); }
        }

        // Public properties
        public List<string> dsVideoURLs { get; set; }
        public bool dsFireCompleteEvent { get; set; }

        // Local Variables
        int iVideoIndex = -1; // Zero-based index

        public ucPlayList()
        {
            try
            {
                InitializeComponent();

                mediaPlayer.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(mediaPlayer_MediaFailed);
                mediaPlayer.MediaEnded += new RoutedEventHandler(mediaPlayer_MediaEnded);
                this.Unloaded += ucPlayList_Unloaded;
            }
            catch { }
        }

        public void Pause()
        {
            try
            {
                mediaPlayer.Pause();
            }
            catch { }
        }

        public void Resume()
        {
            try
            {
                mediaPlayer.Play();
            }
            catch { }
        }

        public void StopAll()
        {
            try
            {
                mediaPlayer.Stop();
                mediaPlayer.Source = null;
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                LayoutRoot.Width = this.Width;
                LayoutRoot.Height = this.Height;

                gridMain.Width = this.Width;
                gridMain.Height = this.Height;

                iVideoIndex = -1;
                SetNextMedia();
            }
            catch { }
        }

        void ucPlayList_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                mediaPlayer.Stop();
                mediaPlayer.Source = null;
            }
            catch { }
        }

        void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetNextMedia();
            }
            catch { }
        }

        void mediaPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            try
            {
                SetNextMedia();
            }
            catch { }
        }

        private void SetNextMedia()
        {
            try
            {
                if (iVideoIndex + 1 < dsVideoURLs.Count)
                    iVideoIndex = iVideoIndex + 1;
                else
                {
                    if (dsFireCompleteEvent)
                        RaiseEvent(new RoutedEventArgs(PlayListCompleteEvent));

                    iVideoIndex = 0;
                }

                mediaPlayer.Source = new Uri(dsVideoURLs[iVideoIndex]);
                mediaPlayer.Play();
            }
            catch { }
        }
    }
}
