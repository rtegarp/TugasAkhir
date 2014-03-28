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
using System.Net;
using System.IO;
using System.Windows.Threading;

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

namespace osVodigiUploader.UserControls
{
    public partial class ucUpload : UserControl
    {
        private DispatcherTimer tmrupload = new DispatcherTimer();
        private WebClient webclient;
        private FileInfo fileuploading;
        private List<FileInfo> filesuploading;
        public List<FileInfo> filesuploaded;
        private int fileindex = 0;

        // UploadComplete Event
        public static readonly RoutedEvent UploadCompleteEvent = EventManager.RegisterRoutedEvent(
            "UploadComplete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucUpload));

        public event RoutedEventHandler UploadComplete
        {
            add { AddHandler(UploadCompleteEvent, value); }
            remove { RemoveHandler(UploadCompleteEvent, value); }
        }

        public ucUpload()
        {
            InitializeComponent();
            tmrupload.Interval = new TimeSpan(0, 0, 0, 0, 500);
            tmrupload.Tick += new EventHandler(tmrupload_Tick);
        }

        public void UploadFiles(List<FileInfo> filestoupload)
        {
            try
            {
                fileindex = 0;
                filesuploaded = new List<FileInfo>();
                filesuploading = filestoupload;
                if (filesuploading == null || filesuploading.Count == 0)
                {
                    this.Visibility = Visibility.Hidden;
                    return;
                }

                tmrupload.Start();
            }
            catch { }
        }

        public void StartUpload(int accountid,
            string ftpserver,
            string ftpusername,
            string ftppassword,
            FileInfo filetoupload,
            string mediatype)
        {
            try
            {
                using (webclient = new WebClient())
                {
                    ResetUpload();

                    CreateFTPFolders(ftpserver, ftpusername, ftppassword, accountid);

                    webclient = new WebClient();
                    webclient.UploadFileCompleted += new UploadFileCompletedEventHandler(webclient_UploadFileCompleted);
                    webclient.UploadProgressChanged += new UploadProgressChangedEventHandler(webclient_UploadProgressChanged);

                    fileuploading = filetoupload;

                    webclient.Credentials = new NetworkCredential(ftpusername, ftppassword);

                    Uri uploadfile = new Uri(ftpserver + "/" + accountid.ToString() + "/" + mediatype + "/" + filetoupload.Name);
                    webclient.UploadFileAsync(uploadfile, "STOR", filetoupload.FullName);
                }
            }
            catch { }
        }

        void webclient_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            try
            {
                progressBar.Value = 100;
                lblProgress.Content = fileuploading.Name;
                lblTransfer.Content = "Upload complete.";

                fileindex += 1;

                if (e.Error == null && !e.Cancelled)
                    filesuploaded.Add(fileuploading);

                tmrupload.Start();
            }
            catch { }
        }

        void webclient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            try
            {
                if (e.TotalBytesToSend > 0)
                    progressBar.Value = Convert.ToInt32(Convert.ToDouble(e.BytesSent) / Convert.ToDouble(e.TotalBytesToSend) * 100);

                lblProgress.Content = String.Format("Uploading {0}", fileuploading.Name);

                if (e.TotalBytesToSend < 1024)
                {
                    lblTransfer.Content = String.Format("Uploaded {0} of {1}", e.BytesSent.ToString(), e.TotalBytesToSend.ToString() + " Bytes");
                }
                else if (e.TotalBytesToSend >= 1024 && e.TotalBytesToSend < 1048576)
                {
                    lblTransfer.Content = String.Format("Uploaded {0} of {1}",
                        String.Format("{0:0.00}", (Convert.ToDouble(e.BytesSent) / 1024)), String.Format("{0:0.00}", Convert.ToDouble(e.TotalBytesToSend) / 1024) + " KB");
                }
                else if (e.TotalBytesToSend >= 1048576 && e.TotalBytesToSend < 1073741824)
                {
                    lblTransfer.Content = String.Format("Uploaded {0} of {1}",
                        String.Format("{0:0.00}", (Convert.ToDouble(e.BytesSent) / 1048576)), String.Format("{0:0.00}", Convert.ToDouble(e.TotalBytesToSend) / 1048576) + " MB");
                }
                else
                {
                    lblTransfer.Content = String.Format("Uploaded {0} of {1}",
                        String.Format("{0:0.00}", (Convert.ToDouble(e.BytesSent) / 1073741824)), String.Format("{0:0.00}", Convert.ToDouble(e.TotalBytesToSend) / 1073741824) + " GB");
                }
            }
            catch { }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Hidden;
                tmrupload.Stop();
                webclient.CancelAsync();

                // Attempt to delete the current file
                string ftpfiletodelete = Global.CurrentUserAccount.FTPServer + "/" + Global.CurrentUserAccount.AccountID.ToString() + "/";
                if (fileuploading.Name.ToLower().EndsWith(".mp4") || fileuploading.Name.ToLower().EndsWith(".wmv"))
                    ftpfiletodelete += "Videos/" + fileuploading.Name;
                else if (fileuploading.Name.ToLower().EndsWith(".mp3") || fileuploading.Name.ToLower().EndsWith(".wma"))
                    ftpfiletodelete += "Music/" + fileuploading.Name;
                else
                    ftpfiletodelete += "Images/" + fileuploading.Name;

                WebRequest req = WebRequest.Create(ftpfiletodelete);
                req.Credentials = new NetworkCredential(Global.CurrentUserAccount.FTPUsername, Global.CurrentUserAccount.FTPPassword);
                req.Method = WebRequestMethods.Ftp.DeleteFile;
                var resp = (FtpWebResponse)req.GetResponse();

                RaiseEvent(new RoutedEventArgs(UploadCompleteEvent));
            }
            catch { }
        }

        void tmrupload_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrupload.Stop();

                if (fileindex >= filesuploading.Count)
                {
                    this.Visibility = Visibility.Hidden;
                    RaiseEvent(new RoutedEventArgs(UploadCompleteEvent));
                }

                ResetUpload();

                fileuploading = filesuploading[fileindex];

                string mediatype = "Images";
                if (fileuploading.Name.ToLower().EndsWith(".mp4") || fileuploading.Name.ToLower().EndsWith(".wmv"))
                    mediatype = "Videos";
                else if (fileuploading.Name.ToLower().EndsWith(".mp3") || fileuploading.Name.ToLower().EndsWith(".wma"))
                    mediatype = "Music";

                StartUpload(Global.CurrentUserAccount.AccountID,
                            Global.CurrentUserAccount.FTPServer,
                            Global.CurrentUserAccount.FTPUsername,
                            Global.CurrentUserAccount.FTPPassword,
                            fileuploading,
                            mediatype);
            }
            catch { }
        }

        public void ResetUpload()
        {
            try
            {
                lblProgress.Content = "Preparing Upload...";
                progressBar.Value = 0;
                lblTransfer.Content = "Please wait...";
            }
            catch { }
        }

        private void CreateFTPFolders(string ftpserver, string ftpusername, string ftppassword, int accountid)
        {
            try
            {
                WebRequest req1 = WebRequest.Create(ftpserver + "/" + accountid.ToString());
                req1.Credentials = new NetworkCredential(ftpusername, ftppassword);
                req1.Method = WebRequestMethods.Ftp.MakeDirectory;
                var resp1 = (FtpWebResponse)req1.GetResponse();

                WebRequest req2 = WebRequest.Create(ftpserver + "/" + accountid.ToString() + "/Images");
                req2.Credentials = new NetworkCredential(ftpusername, ftppassword);
                req2.Method = WebRequestMethods.Ftp.MakeDirectory;
                var resp2 = (FtpWebResponse)req2.GetResponse();

                WebRequest req3 = WebRequest.Create(ftpserver + "/" + accountid.ToString() + "/Videos");
                req3.Credentials = new NetworkCredential(ftpusername, ftppassword);
                req3.Method = WebRequestMethods.Ftp.MakeDirectory;
                var resp3 = (FtpWebResponse)req3.GetResponse();

                WebRequest req4 = WebRequest.Create(ftpserver + "/" + accountid.ToString() + "/Music");
                req4.Credentials = new NetworkCredential(ftpusername, ftppassword);
                req4.Method = WebRequestMethods.Ftp.MakeDirectory;
                var resp4 = (FtpWebResponse)req4.GetResponse();
            }
            catch { }
        }
    }
}
