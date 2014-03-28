﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.IO;

namespace TugasAkhirClient.UserControls
{
    public partial class ucSlideShowZoomIn : UserControl
    {
        // Complete Event
        public static readonly RoutedEvent SlideShowCompleteEvent = EventManager.RegisterRoutedEvent(
            "SlideShowComplete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSlideShowZoomIn));

        public event RoutedEventHandler SlideShowComplete
        {
            add { AddHandler(SlideShowCompleteEvent, value); }
            remove { RemoveHandler(SlideShowCompleteEvent, value); }
        }

        // Public properties
        public Color dsBackgroundColor { get; set; }
        public int dsSlideDurationInSeconds { get; set; }
        public List<string> dsImageURLs { get; set; }
        public List<string> dsMusicURLs { get; set; }
        public bool dsFireCompleteEvent { get; set; }
        public string dsImageFillMode { get; set; }

        // Local variables
        DispatcherTimer timer;
        int imageIndex = -1; // Zero-based index
        int imageToDisplay = 1; // 1 or 2 to indicate which Image control is currently visible
        int musicIndex = -1; // Zero-based index

        // Storyboard variables
        Storyboard sbImageOneScale;
        Storyboard sbImageTwoScale;

        public ucSlideShowZoomIn()
        {
            try
            {
                InitializeComponent();

                sbImageOneScale = (Storyboard)FindResource("sbImageOneScale");
                sbImageTwoScale = (Storyboard)FindResource("sbImageTwoScale");
            }
            catch { }
        }

        public void Pause()
        {
            try
            {
                timer.Stop();
                mediaPlayer.Pause();
            }
            catch { }
        }

        public void Resume()
        {
            try
            {
                timer.Start();
                mediaPlayer.Play();
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                // Resizes and positions the control contents according to the specified properties

                // Set the clipping region
                rectClip.Rect = new Rect(0, 0, this.Width, this.Height);

                // Set the width/height of the image controls
                imgOne.Width = this.Width;
                imgOne.Height = this.Height;

                imgTwo.Width = this.Width;
                imgTwo.Height = this.Height;

                if (dsImageFillMode.ToLower().StartsWith("f"))
                {
                    imgOne.Stretch = Stretch.Fill;
                    imgOne.Stretch = Stretch.Fill;
                }
                else
                {
                    imgTwo.Stretch = Stretch.UniformToFill;
                    imgTwo.Stretch = Stretch.UniformToFill;
                }

                // Set the Background color - applied to gridMain
                gridMain.Background = new SolidColorBrush(dsBackgroundColor);

                // Validate the slide duration - no less than 5 seconds
                if (dsSlideDurationInSeconds < 5) dsSlideDurationInSeconds = 5;

                // Set the centers
                stImageSlideshow1.CenterX = Convert.ToDouble(this.Width / 2);
                stImageSlideshow1.CenterY = Convert.ToDouble(this.Height / 2);
                stImageSlideshow2.CenterX = Convert.ToDouble(this.Width / 2);
                stImageSlideshow2.CenterY = Convert.ToDouble(this.Height / 2);

                mediaPlayer.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(mediaPlayer_MediaFailed);
                mediaPlayer.MediaEnded += new RoutedEventHandler(mediaPlayer_MediaEnded);

                this.Unloaded += ucSlideShowZoomIn_Unloaded;

                // Create the timer for the transition
                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = TimeSpan.FromSeconds(dsSlideDurationInSeconds);

                musicIndex = -1;
                SetNextMedia();

                ShowNextImage();
                timer.Start();
            }
            catch { }
        }

        private BitmapImage GetBitmap(string sFile)
        {
            try
            {
                if (File.Exists(sFile))
                {
                    BitmapImage bmpimg = new BitmapImage();
                    bmpimg.BeginInit();
                    bmpimg.UriSource = new Uri(sFile, UriKind.Absolute);
                    bmpimg.EndInit();
                    return bmpimg;
                }
                else
                    return null;
            }
            catch { return null; }
        }

        private void ShowNextImage()
        {
            try
            {
                if (dsImageURLs != null && dsImageURLs.Count > 0)
                {
                    if (imageIndex + 1 < dsImageURLs.Count)
                        imageIndex = imageIndex + 1;
                    else
                    {
                        if (dsFireCompleteEvent)
                        {
                            RaiseEvent(new RoutedEventArgs(SlideShowCompleteEvent));
                            mediaPlayer.Stop();
                        }

                        imageIndex = 0;
                    }

                    if (imageToDisplay == 1)
                    {
                        imgOne.Source = GetBitmap(dsImageURLs[imageIndex]);
                        sbImageOneScale.Begin();
                        canvasImageOne.SetValue(Canvas.ZIndexProperty, 100);
                        canvasImageTwo.SetValue(Canvas.ZIndexProperty, 99);
                        imageToDisplay = 2;
                    }
                    else
                    {
                        imgTwo.Source = GetBitmap(dsImageURLs[imageIndex]);
                        sbImageTwoScale.Begin();
                        canvasImageTwo.SetValue(Canvas.ZIndexProperty, 100);
                        canvasImageOne.SetValue(Canvas.ZIndexProperty, 99);
                        imageToDisplay = 1;
                    }
                }
            }
            catch { }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ShowNextImage();
        }

        public void StopTimer()
        {
            try
            {
                timer.Stop();
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
                if (dsMusicURLs.Count == 0)
                    return;

                if (musicIndex + 1 < dsMusicURLs.Count)
                    musicIndex = musicIndex + 1;
                else
                {
                    musicIndex = 0;
                }

                mediaPlayer.Source = new Uri(dsMusicURLs[musicIndex]);
                mediaPlayer.Play();
            }
            catch { }
        }

        void ucSlideShowZoomIn_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                mediaPlayer.Stop();
                mediaPlayer.Source = null;
                timer.Stop();
            }
            catch { }
        }

    }
}
