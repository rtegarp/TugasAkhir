﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.IO;

/* ----------------------------------------------------------------------------------------
    Vodigi - Open Source Interactive Digital Signage
    Copyright (C) 2005-2012  JMC Publications, LLC

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
---------------------------------------------------------------------------------------- */

namespace osVodigiPlayer.UserControls
{
    public partial class ucSlideShowFader : UserControl
    {
        // Complete Event
        public static readonly RoutedEvent SlideShowCompleteEvent = EventManager.RegisterRoutedEvent(
            "SlideShowComplete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSlideShowFader));

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
        Storyboard sbFadeOutImageOne;
        Storyboard sbFadeInImageOne;
        Storyboard sbFadeOutImageTwo;
        Storyboard sbFadeInImageTwo;

        public ucSlideShowFader()
        {
            try
            {
                InitializeComponent();

                sbFadeOutImageOne = (Storyboard)FindResource("sbFadeOutImageOne");
                sbFadeInImageOne = (Storyboard)FindResource("sbFadeInImageOne");
                sbFadeOutImageTwo = (Storyboard)FindResource("sbFadeOutImageTwo");
                sbFadeInImageTwo = (Storyboard)FindResource("sbFadeInImageTwo");
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
                imgSlideshow1.Width = this.Width;
                imgSlideshow1.Height = this.Height;

                imgSlideshow2.Width = this.Width;
                imgSlideshow2.Height = this.Height;

                if (dsImageFillMode.ToLower().StartsWith("f"))
                {
                    imgSlideshow1.Stretch = Stretch.Fill;
                    imgSlideshow1.Stretch = Stretch.Fill;
                }
                else
                {
                    imgSlideshow2.Stretch = Stretch.UniformToFill;
                    imgSlideshow2.Stretch = Stretch.UniformToFill;
                }

                // Set the Background color - applied to gridMain
                gridMain.Background = new SolidColorBrush(dsBackgroundColor);

                // Validate the slide duration - no less than 5 seconds
                if (dsSlideDurationInSeconds < 5) dsSlideDurationInSeconds = 5;

                this.Unloaded += ucSlideShowFader_Unloaded;

                mediaPlayer.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(mediaPlayer_MediaFailed);
                mediaPlayer.MediaEnded += new RoutedEventHandler(mediaPlayer_MediaEnded);

                musicIndex = -1;
                SetNextMedia();

                // Create the timer for the transition
                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = TimeSpan.FromSeconds(dsSlideDurationInSeconds);

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
                        imgSlideshow1.Source = GetBitmap(dsImageURLs[imageIndex]);
                        sbFadeInImageOne.Begin();
                        sbFadeOutImageTwo.Begin();
                        imageToDisplay = 2;
                    }
                    else
                    {
                        imgSlideshow2.Source = GetBitmap(dsImageURLs[imageIndex]);
                        sbFadeInImageTwo.Begin();
                        sbFadeOutImageOne.Begin();
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

        void ucSlideShowFader_Unloaded(object sender, RoutedEventArgs e)
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
