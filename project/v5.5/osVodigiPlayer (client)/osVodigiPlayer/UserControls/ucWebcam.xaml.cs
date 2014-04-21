using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using System.Runtime.InteropServices;

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
    public partial class ucWebcam : UserControl
    {
        // Public properties
        public string dsCameraSource { get; set; }

        // Local variables
        LiveJob videojob;
        EncoderDevice currentcam;
        System.Windows.Forms.Panel pnlVideoPreview;
        LiveDeviceSource videosource;

        public ucWebcam()
        {
            try
            {
                InitializeComponent();

                this.Unloaded += new RoutedEventHandler(ucWebcam_Unloaded);
            }
            catch { }
        }

        void ucWebcam_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StopCam();
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                // Maximize the main grid
                gridMain.Width = this.Width;
                gridMain.Height = this.Height;

                formsHost.Width = this.Width;
                formsHost.Height = this.Height - 100;

                // Add a win form panel to the grid
                pnlVideoPreview = new System.Windows.Forms.Panel();
                pnlVideoPreview.Width = Convert.ToInt32(this.Width);
                pnlVideoPreview.Height = Convert.ToInt32(this.Height) - 100;
                pnlVideoPreview.Left = 0;
                pnlVideoPreview.Top = 0;

                // Add the panel
                formsHost.Child = pnlVideoPreview;

                // Get the webcam matching the specified name
                foreach (EncoderDevice cam in EncoderDevices.FindDevices(EncoderDeviceType.Video))
                {
                    if (cam.Name == dsCameraSource)
                    {
                        currentcam = cam;
                    }
                }
            }
            catch { }
        }

        public void StartCam()
        {
            try
            {
                // Start the video stream
                videojob = new LiveJob();

                // Create a new device source. We use the first audio and video devices on the system
                videosource = videojob.AddDeviceSource(currentcam, null);

                // Sets preview window to winform panel
                videosource.PreviewWindow = new PreviewWindow(new HandleRef(pnlVideoPreview, pnlVideoPreview.Handle));

                // Activate the source and start
                videojob.ActivateSource(videosource);
            }
            catch { }
        }

        public void StopCam()
        {
            try
            {
                videojob.StopEncoding();

                if (videosource != null)
                {
                    videojob.RemoveDeviceSource(videosource);
                    videosource = null;
                }

                videojob.Dispose();
            }
            catch { }
        }
    }
}
