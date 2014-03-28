using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using System.Runtime.InteropServices;

namespace TugasAkhirClient.UserControls
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
