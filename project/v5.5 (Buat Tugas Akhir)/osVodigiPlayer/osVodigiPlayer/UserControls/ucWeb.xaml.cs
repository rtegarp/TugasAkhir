using System.Windows.Controls;

namespace TugasAkhirClient.UserControls
{
    public partial class ucWeb : UserControl
    {
        // Public properties
        public string dsWebsiteUrl { get; set; }

        public ucWeb()
        {
            InitializeComponent();
        }

        public void ResetControl()
        {
            try
            {
                webBrowser.Width = this.Width;
                webBrowser.Height = this.Height - 100;

                webBrowser.Navigate(dsWebsiteUrl);
            }
            catch { }
        }
    }
}
