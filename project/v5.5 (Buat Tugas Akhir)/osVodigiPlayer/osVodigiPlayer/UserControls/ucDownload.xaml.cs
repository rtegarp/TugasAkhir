﻿using System;
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
using System.IO;

namespace TugasAkhirClient.UserControls
{
    public partial class ucDownload : UserControl
    {
        DispatcherTimer timerdownload;
        private List<Download> downloads = new List<Download>();
        int currentfileindex = 0;
        int totalfilecount = 0;

        Storyboard sbFadeIn;
        Storyboard sbFadeOut;

        public bool IsApproved { get; set; }

        public static readonly RoutedEvent DownloadClosedEvent = EventManager.RegisterRoutedEvent(
            "DownloadClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucDownload));

        public event RoutedEventHandler DownloadClosed
        {
            add { AddHandler(DownloadClosedEvent, value); }
            remove { RemoveHandler(DownloadClosedEvent, value); }
        }

        public ucDownload()
        {
            try
            {
                InitializeComponent();

                sbFadeIn = (Storyboard)FindResource("sbFadeIn");
                sbFadeOut = (Storyboard)FindResource("sbFadeOut");
                sbFadeOut.Completed += sbFadeOut_Completed;

                btnApprove.MouseLeftButtonUp += btnApprove_MouseLeftButtonUp;
                btnApprove.TouchUp += btnApprove_TouchUp;

                timerdownload = new DispatcherTimer();
                timerdownload.Interval = TimeSpan.FromSeconds(1);
                timerdownload.Tick += timerdownload_Tick;
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                if (IsApproved)
                {
                    GetDownloadsList();

                    progressBar.Minimum = 0;
                    progressBar.Maximum = downloads.Count - 1;
                    progressBar.Value = 0;

                    currentfileindex = 0;
                    if (downloads != null && downloads.Count > 0)
                    {
                        lblDownloadFile.Text = downloads[currentfileindex].FileType + ": " + downloads[currentfileindex].Name;
                        lblDownloadStatus.Text = "File " + (currentfileindex + 1).ToString() + " of " + totalfilecount.ToString();
                    }
                    else
                    {
                        lblDownloadFile.Text = "Please wait. Downloading media.";
                        lblDownloadStatus.Text = String.Empty;
                    }

                    lblApproveInstructions.Visibility = Visibility.Collapsed;
                    btnApprove.Visibility = Visibility.Collapsed;

                    lblDownloadFile.Visibility = Visibility.Visible;
                    progressBar.Visibility = Visibility.Visible;
                    lblDownloadStatus.Visibility = Visibility.Visible;

                    timerdownload.Start();
                }
                else
                {
                    lblApproveInstructions.Visibility = Visibility.Visible;
                    btnApprove.Visibility = Visibility.Visible;

                    lblDownloadFile.Visibility = Visibility.Collapsed;
                    progressBar.Visibility = Visibility.Collapsed;
                    lblDownloadStatus.Visibility = Visibility.Collapsed;
                }
            }
            catch { }
        }

        void btnApprove_TouchUp(object sender, TouchEventArgs e)
        {
            ApproveClicked();
        }

        void btnApprove_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ApproveClicked();
        }

        private void ApproveClicked()
        {
            try
            {
                // Save the Approved setting in the Player Configuration File
                PlayerConfiguration.configIsDownloadApproved = true;
                PlayerConfiguration.SavePlayerConfiguration();

                IsApproved = true;
                ResetControl();
            }
            catch { }
        }

        void sbFadeOut_Completed(object sender, EventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                RaiseEvent(new RoutedEventArgs(DownloadClosedEvent));
            }
            catch { }
        }

        void timerdownload_Tick(object sender, EventArgs e)
        {
            try
            {
                timerdownload.Stop();

                if (currentfileindex != totalfilecount)
                {
                    DownloadNextFile();
                    timerdownload.Start();
                }
                else
                {
                    lblDownloadFile.Text = "Downloads Complete";
                    lblDownloadStatus.Text = "File " + currentfileindex.ToString() + " of " + totalfilecount.ToString();

                    FadeOut();
                }
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
                timerdownload.Stop();
                sbFadeOut.Begin();
            }
            catch { }
        }

        public void GetDownloadsList()
        {
            try
            {
                downloads = new List<Download>();

                foreach (Image image in CurrentSchedule.Images)
                {
                    Download download = new Download();
                    download.FileType = "Image";
                    download.StoredFilename = image.StoredFilename;
                    download.Name = image.ImageName;
                    downloads.Add(download);
                }

                foreach (Music music in CurrentSchedule.Musics)
                {
                    Download download = new Download();
                    download.FileType = "Music";
                    download.StoredFilename = music.StoredFilename;
                    download.Name = music.MusicName;
                    downloads.Add(download);
                }

                foreach (Video video in CurrentSchedule.Videos)
                {
                    Download download = new Download();
                    download.FileType = "Video";
                    download.StoredFilename = video.StoredFilename;
                    download.Name = video.VideoName;
                    downloads.Add(download);
                }

                totalfilecount = downloads.Count;

                progressBar.Minimum = 0;
                progressBar.Maximum = downloads.Count;
                progressBar.Value = 0;
            }
            catch { }
        }

        public void DownloadNextFile()
        {
            try
            {
                // Build the source and target urls
                // Save to C:\osVodigi\Images\GUID.ext or C:\osVodigi\Videos\GUID.ext by default
                string target = String.Empty;
                string source = String.Empty;

                if (downloads[currentfileindex].FileType.ToLower() == "video")
                {
                    source = DownloadManager.MediaSourceUrl + PlayerConfiguration.configAccountID.ToString() + "/Videos/" + downloads[currentfileindex].StoredFilename;
                    target = DownloadManager.DownloadFolder + @"Videos\" + downloads[currentfileindex].StoredFilename;
                }
                else if (downloads[currentfileindex].FileType.ToLower() == "music")
                {
                    source = DownloadManager.MediaSourceUrl + PlayerConfiguration.configAccountID.ToString() + "/Music/" + downloads[currentfileindex].StoredFilename;
                    target = DownloadManager.DownloadFolder + @"Music\" + downloads[currentfileindex].StoredFilename;
                }
                else
                {
                    source = DownloadManager.MediaSourceUrl + PlayerConfiguration.configAccountID.ToString() + "/Images/" + downloads[currentfileindex].StoredFilename;
                    target = DownloadManager.DownloadFolder + @"Images\" + downloads[currentfileindex].StoredFilename;
                }

                try
                {
                    lblDownloadFile.Text = downloads[currentfileindex + 1].FileType + ": " + downloads[currentfileindex + 1].Name;
                    lblDownloadStatus.Text = "File " + (currentfileindex + 2).ToString() + " of " + totalfilecount.ToString();
                    progressBar.Value = progressBar.Value + 1;
                    System.Windows.Forms.Application.DoEvents();
                }
                catch { }

                if (!File.Exists(target))
                {
                    DownloadManager.DownloadAndSaveFile(source, target);
                }

                currentfileindex = currentfileindex + 1;
            }
            catch { }
        }

    }
}
