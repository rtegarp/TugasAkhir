using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.IO;

namespace TugasAkhirClient.UserControls
{
    public partial class ucSelection : UserControl
    {
        // Selection Event
        public static readonly RoutedEvent SelectionCompleteEvent = EventManager.RegisterRoutedEvent(
            "SelectionComplete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSelectionBar));

        public event RoutedEventHandler SelectionComplete
        {
            add { AddHandler(SelectionCompleteEvent, value); }
            remove { RemoveHandler(SelectionCompleteEvent, value); }
        }

        // SelectionBack Event
        public static readonly RoutedEvent SelectionBackEvent = EventManager.RegisterRoutedEvent(
            "SelectionBack", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSelectionBar));

        public event RoutedEventHandler SelectionBack
        {
            add { AddHandler(SelectionBackEvent, value); }
            remove { RemoveHandler(SelectionBackEvent, value); }
        }

        // SelectionNext Event
        public static readonly RoutedEvent SelectionNextEvent = EventManager.RegisterRoutedEvent(
            "SelectionNext", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSelectionBar));

        public event RoutedEventHandler SelectionNext
        {
            add { AddHandler(SelectionNextEvent, value); }
            remove { RemoveHandler(SelectionNextEvent, value); }
        }

        // SelectionMouseEnter Event
        public static readonly RoutedEvent SelectionMouseEnterEvent = EventManager.RegisterRoutedEvent(
            "SelectionMouseEnter", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSelectionBar));

        public event RoutedEventHandler SelectionMouseEnter
        {
            add { AddHandler(SelectionMouseEnterEvent, value); }
            remove { RemoveHandler(SelectionMouseEnterEvent, value); }
        }

        // SelectionMouseLeave Event
        public static readonly RoutedEvent SelectionMouseLeaveEvent = EventManager.RegisterRoutedEvent(
            "SelectionMouseLeave", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucSelectionBar));

        public event RoutedEventHandler SelectionMouseLeave
        {
            add { AddHandler(SelectionMouseLeaveEvent, value); }
            remove { RemoveHandler(SelectionMouseLeaveEvent, value); }
        }

        // Public properties
        public string dsImageURL { get; set; }
        public int dsSelectionIndex { get; set; }
        public bool dsUseKinect { get; set; }

        // Storyboard variables
        Storyboard sbImageZoom;
        Storyboard sbImageNormal;

        // Drag / Drop
        Point mousestart = new Point(0, 0);

        // Touch
        TouchPoint touchstart;

        public ucSelection()
        {
            try
            {
                InitializeComponent();

                this.MouseEnter += new MouseEventHandler(ucSelection_MouseEnter);
                this.MouseLeave += new MouseEventHandler(ucSelection_MouseLeave);

                sbImageZoom = (Storyboard)FindResource("sbImageZoom");
                sbImageNormal = (Storyboard)FindResource("sbImageNormal");

            }
            catch { }
        }

        void ucSelection_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (dsUseKinect)
                {
                    RaiseEvent(new RoutedEventArgs(SelectionMouseLeaveEvent));
                }
            }
            catch { }
        }

        void ucSelection_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (dsUseKinect)
                {
                    RaiseEvent(new RoutedEventArgs(SelectionMouseEnterEvent));
                }
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                // Resizes and positions the control contents according to the specified properties

                // Set the clipping region
                rgGridClip.Rect = new Rect(0, 0, this.Width, this.Height);

                // Set the width/height of the image control
                img.Width = this.Width;
                img.Height = this.Height;

                // Set the width/height of the border
                rectBorder.Width = this.Width;
                rectBorder.Height = this.Height;

                // Set the centers
                stImage.CenterX = Convert.ToDouble(this.Width / 2);
                stImage.CenterY = Convert.ToDouble(this.Height / 2);

                // Set the image
                img.Source = GetBitmap(dsImageURL);

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

        public void ZoomIn()
        {
            try
            {
                sbImageZoom.Begin();
            }
            catch { }
        }

        public void ZoomOut()
        {
            try
            {
                sbImageNormal.Begin();
            }
            catch { }
        }

        private void rectBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                rectBorder.CaptureMouse();
                mousestart = e.GetPosition(gridMain);
            }
            catch { }
        }

        private void rectBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                rectBorder.ReleaseMouseCapture();

                double diffx = mousestart.X - e.GetPosition(gridMain).X;
                if (diffx > 25)
                    RaiseEvent(new RoutedEventArgs(SelectionNextEvent));
                else if (diffx < -25)
                    RaiseEvent(new RoutedEventArgs(SelectionBackEvent));
                else
                    RaiseEvent(new RoutedEventArgs(SelectionCompleteEvent));

            }
            catch { }
        }

        private void rectBorder_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                rectBorder.CaptureTouch(e.TouchDevice);
                touchstart = e.GetTouchPoint(rectBorder);
            }
            catch { }
        }

        private void rectBorder_TouchUp(object sender, TouchEventArgs e)
        {
            try
            {
                rectBorder.ReleaseTouchCapture(e.TouchDevice);

                double diffx = touchstart.Position.X - e.GetTouchPoint(rectBorder).Position.X;
                if (diffx > 25)
                    RaiseEvent(new RoutedEventArgs(SelectionNextEvent));
                else if (diffx < -25)
                    RaiseEvent(new RoutedEventArgs(SelectionBackEvent));
                else
                    RaiseEvent(new RoutedEventArgs(SelectionCompleteEvent));
            }
            catch { }
        }

        public void ClickThis()
        {
            try
            {
                RaiseEvent(new RoutedEventArgs(SelectionCompleteEvent));
            }
            catch { }
        }
    }
}
