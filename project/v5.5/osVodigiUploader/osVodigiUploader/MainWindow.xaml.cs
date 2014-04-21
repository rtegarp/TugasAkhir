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
using Microsoft.Win32;
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

namespace osVodigiUploader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ucUpload.UploadComplete += new RoutedEventHandler(ucUpload_UploadComplete);
        }

        private void btnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog opendlg = new OpenFileDialog();
                opendlg.Filter = "Image Files|*.png;*.jpg;*.jpeg|Video Files|*.wmv;*.mp4|Music Files|*.wma;*.mp3";
                opendlg.Multiselect = true;
                bool dlgresult = Convert.ToBoolean(opendlg.ShowDialog());

                if (dlgresult)
                {
                    List<FileUpload> fileuploads;
                    if (gridFiles.ItemsSource != null)
                        fileuploads = gridFiles.ItemsSource.Cast<FileUpload>().ToList();
                    else
                        fileuploads = new List<FileUpload>();

                    List<string> files = opendlg.FileNames.ToList();
                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);

                        FileUpload upload = new FileUpload();
                        upload.FileName = fi.Name;
                        upload.FilePath = fi.FullName;
                        double filesize = Convert.ToDouble(fi.Length) / Convert.ToDouble(1048576);
                        upload.FileSize = String.Format("{0:0.00}", filesize) + " MB";
                        if (fi.Extension.ToLower() == ".mp4" || fi.Extension.ToLower() == ".wmv")
                            upload.FileType = "Video";
                        else if (fi.Extension.ToLower() == ".mp3" || fi.Extension.ToLower() == ".wma")
                            upload.FileType = "Music";
                        else
                            upload.FileType = "Image";
                        upload.FileStatus = "Pending";

                        fileuploads.Add(upload);
                    }

                    gridFiles.ItemsSource = fileuploads;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUploadFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Make sure there are files that need to be uploaded
                bool upload = false;
                foreach (FileUpload file in gridFiles.ItemsSource)
                {
                    if (file.FileStatus != "Uploaded")
                        upload = true;
                }

                if (!upload)
                {
                    MessageBox.Show("There are no files to upload.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Upload the files
                ucUpload.ResetUpload();
                List<FileInfo> filestoupload = new List<FileInfo>();
                foreach (FileUpload file in gridFiles.ItemsSource)
                {
                    if (file.FileStatus != "Uploaded")
                        filestoupload.Add(new FileInfo(file.FilePath));
                }
                if (filestoupload.Count == 0)
                {
                    MessageBox.Show("There are no files to upload.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ucUpload.UploadFiles(filestoupload);
                ucUpload.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("There are no files to upload.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRemoveFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridFiles.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select one or more items to remove.", "Select an Item", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                List<FileUpload> fileuploads;
                if (gridFiles.ItemsSource != null)
                    fileuploads = gridFiles.ItemsSource.Cast<FileUpload>().ToList();
                else
                    fileuploads = new List<FileUpload>();

                foreach (FileUpload file in gridFiles.SelectedItems)
                {
                    fileuploads.Remove(file);
                }

                gridFiles.ItemsSource = fileuploads;
            }
            catch { }
        }

        void ucUpload_UploadComplete(object sender, RoutedEventArgs e)
        {
            try
            {
                List<FileUpload> fileuploads;
                if (gridFiles.ItemsSource != null)
                    fileuploads = gridFiles.ItemsSource.Cast<FileUpload>().ToList();
                else
                    fileuploads = new List<FileUpload>();

                // Update the grid
                foreach (FileInfo fileinfo in ucUpload.filesuploaded)
                {
                    // Update the status of the appropriate file
                    foreach (FileUpload file in fileuploads)
                    {
                        // Set to uploaded if upload was complete
                        if (file.FileName == fileinfo.Name && file.FilePath == fileinfo.FullName)
                            file.FileStatus = "Uploaded";
                    }
                }

                gridFiles.ItemsSource = fileuploads;
            }
            catch { }
        }
    }
}
