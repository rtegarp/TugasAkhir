﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.XPath;
using System.Net;

namespace TugasAkhirClient.UserControls
{
    public partial class ucWeather : UserControl
    {
        // Public properties
        // public string dsZipCode { get; set; }
        public decimal dsLongitude { get; set; }
        public decimal dsLatitude { get; set; }

        // Local Variables
        DispatcherTimer timer = new DispatcherTimer();

        public ucWeather()
        {
            try
            {
                InitializeComponent();
            }
            catch { }
        }

        public void ResetControl()
        {
            try
            {
                rectClip.Rect = new Rect(0, 0, 920, 600);
                if (this.Width < 1000)
                {
                    gridMain.Width = 615;
                    rectClip.Rect = new Rect(0, 0, 615, 600);
                }

                GetWeather();

                timer.Interval = TimeSpan.FromHours(2);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
            catch { }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Get the latest weather
                GetWeather();
            }
            catch { }
        }

        void ucWeather_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the latest weather
                GetWeather();
            }
            catch { }
        }

        private void GetWeather()
        {
            try
            {
                string sTempHighDay1 = "";
                string sTempHighDay2 = "";
                string sTempHighDay3 = "";
                string sTempLowDay1 = "";
                string sTempLowDay2 = "";
                string sTempLowDay3 = "";
                string sWeather1 = "";
                string sWeather2 = "";
                string sWeather3 = "";
                string sIconURL1 = "";
                string sIconURL2 = "";
                string sIconURL3 = "";


                // Get three days worth of weather - coded to read to 4 levels of node hierarchy

                string sCurrentNode = "";
                string noaarest = "http://graphical.weather.gov/xml/sample_products/browser_interface/ndfdBrowserClientByDay.php?lat=" + dsLatitude.ToString() + "&lon=" + dsLongitude.ToString() + "&format=24+hourly&numDays=3";

                WebClient webclient = new WebClient();
                string sWeatherXML = webclient.DownloadString(noaarest);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sWeatherXML);
                XPathNavigator nav1 = doc.CreateNavigator();
                nav1.MoveToRoot();
                nav1.MoveToFirstChild(); // dwml
                // Level 1 - find data node ---------------------------
                if (nav1.HasChildren)
                {
                    nav1.MoveToFirstChild();
                    do
                    {
                        if (nav1.NodeType == XPathNodeType.Element)
                        {
                            if (nav1.Name.ToLower() == "data")
                            {
                                // Level 2 - find the parameters node -----------------------------
                                if (nav1.HasChildren)
                                {
                                    XPathNavigator nav2 = nav1.Clone();
                                    nav2.MoveToFirstChild();
                                    do
                                    {
                                        if (nav2.NodeType == XPathNodeType.Element)
                                        {
                                            if (nav2.Name.ToLower() == "parameters")
                                            {
                                                // Level 3 - find the temperature, weather and conditions-icon nodes
                                                if (nav2.HasChildren)
                                                {
                                                    XPathNavigator nav3 = nav2.Clone();
                                                    nav3.MoveToFirstChild();
                                                    do
                                                    {
                                                        if (nav3.NodeType == XPathNodeType.Element)
                                                        {
                                                            if (nav3.Name.ToLower() == "temperature" || nav3.Name.ToLower() == "weather" || nav3.Name.ToLower() == "conditions-icon")
                                                            {
                                                                sCurrentNode = nav3.Name.ToLower();

                                                                // Level 4 - read the values
                                                                if (sCurrentNode == "temperature")
                                                                {
                                                                    if (nav3.HasAttributes)
                                                                    {
                                                                        XPathNavigator nava = nav3.Clone();
                                                                        nava.MoveToAttribute("type", "");
                                                                        if (nava.Value.ToLower() == "maximum")
                                                                        {
                                                                            XPathNavigator nav4a = nav3.Clone();
                                                                            nav4a.MoveToFirstChild();
                                                                            do
                                                                            {
                                                                                if (nav4a.NodeType == XPathNodeType.Element)
                                                                                {
                                                                                    if (nav4a.Name.ToLower() == "value")
                                                                                    {
                                                                                        if (String.IsNullOrEmpty(sTempHighDay1))
                                                                                            sTempHighDay1 = nav4a.Value;
                                                                                        else if (String.IsNullOrEmpty(sTempHighDay2))
                                                                                            sTempHighDay2 = nav4a.Value;
                                                                                        else if (String.IsNullOrEmpty(sTempHighDay3))
                                                                                            sTempHighDay3 = nav4a.Value;
                                                                                    }
                                                                                }
                                                                            } while (nav4a.MoveToNext());

                                                                        }
                                                                        else if (nava.Value.ToLower() == "minimum")
                                                                        {
                                                                            XPathNavigator nav4b = nav3.Clone();
                                                                            nav4b.MoveToFirstChild();
                                                                            do
                                                                            {
                                                                                if (nav4b.NodeType == XPathNodeType.Element)
                                                                                {
                                                                                    if (nav4b.Name.ToLower() == "value")
                                                                                    {
                                                                                        if (String.IsNullOrEmpty(sTempLowDay1))
                                                                                            sTempLowDay1 = nav4b.Value;
                                                                                        else if (String.IsNullOrEmpty(sTempLowDay2))
                                                                                            sTempLowDay2 = nav4b.Value;
                                                                                        else if (String.IsNullOrEmpty(sTempLowDay3))
                                                                                            sTempLowDay3 = nav4b.Value;
                                                                                    }
                                                                                }
                                                                            } while (nav4b.MoveToNext());
                                                                        }
                                                                    }
                                                                }
                                                                else if (sCurrentNode == "weather")
                                                                {
                                                                    XPathNavigator nav4c = nav3.Clone();
                                                                    nav4c.MoveToFirstChild();
                                                                    do
                                                                    {
                                                                        if (nav4c.NodeType == XPathNodeType.Element)
                                                                        {
                                                                            if (nav4c.Name.ToLower() == "weather-conditions")
                                                                            {
                                                                                XPathNavigator nav4ca = nav4c.Clone();
                                                                                nav4ca.MoveToAttribute("weather-summary", "");
                                                                                if (String.IsNullOrEmpty(sWeather1))
                                                                                    sWeather1 = nav4ca.Value;
                                                                                else if (String.IsNullOrEmpty(sWeather2))
                                                                                    sWeather2 = nav4ca.Value;
                                                                                else if (String.IsNullOrEmpty(sWeather3))
                                                                                    sWeather3 = nav4ca.Value;
                                                                            }
                                                                        }
                                                                    } while (nav4c.MoveToNext());
                                                                }
                                                                else if (sCurrentNode == "conditions-icon")
                                                                {
                                                                    XPathNavigator nav4d = nav3.Clone();
                                                                    nav4d.MoveToFirstChild();
                                                                    do
                                                                    {
                                                                        if (nav4d.NodeType == XPathNodeType.Element)
                                                                        {
                                                                            if (nav4d.Name.ToLower() == "icon-link")
                                                                            {
                                                                                if (String.IsNullOrEmpty(sIconURL1))
                                                                                    sIconURL1 = nav4d.Value;
                                                                                else if (String.IsNullOrEmpty(sIconURL2))
                                                                                    sIconURL2 = nav4d.Value;
                                                                                else if (String.IsNullOrEmpty(sIconURL3))
                                                                                    sIconURL3 = nav4d.Value;
                                                                            }
                                                                        }
                                                                    } while (nav4d.MoveToNext());
                                                                }
                                                            }
                                                        }
                                                    } while (nav3.MoveToNext());
                                                }
                                            }
                                        }
                                    } while (nav2.MoveToNext());
                                }
                            }
                        }
                    } while (nav1.MoveToNext());
                }

                imgWeatherDay1.Source = new BitmapImage(new Uri(GetImagePath(sWeather1), UriKind.Relative));
                txtDayOfWeek1.Text = GetDay(Convert.ToInt32(DateTime.Today.DayOfWeek));
                txtWeather1.Text = sWeather1;
                txtHigh1.Text = sTempHighDay1;
                txtLow1.Text = sTempLowDay1;

                imgWeatherDay2.Source = new BitmapImage(new Uri(GetImagePath(sWeather2), UriKind.Relative));
                txtDayOfWeek2.Text = GetDay(Convert.ToInt32(DateTime.Today.AddDays(1).DayOfWeek));
                txtWeather2.Text = sWeather2;
                txtHigh2.Text = sTempHighDay2;
                txtLow2.Text = sTempLowDay2;

                imgWeatherDay3.Source = new BitmapImage(new Uri(GetImagePath(sWeather3), UriKind.Relative));
                txtDayOfWeek3.Text = GetDay(Convert.ToInt32(DateTime.Today.AddDays(2).DayOfWeek));
                txtWeather3.Text = sWeather3;
                txtHigh3.Text = sTempHighDay3;
                txtLow3.Text = sTempLowDay3;
            }
            catch
            {
                imgWeatherDay1.Source = new BitmapImage(new Uri("/Images/partlycloudy.png"));
                txtDayOfWeek1.Text = "N/A";
                txtWeather1.Text = "N/A";
                txtHigh1.Text = "N/A";
                txtLow1.Text = "N/A";

                imgWeatherDay2.Source = new BitmapImage(new Uri("/Images/partlycloudy.png"));
                txtDayOfWeek2.Text = "N/A";
                txtWeather2.Text = "N/A";
                txtHigh2.Text = "N/A";
                txtLow2.Text = "N/A";

                imgWeatherDay3.Source = new BitmapImage(new Uri("/Images/partlycloudy.png"));
                txtDayOfWeek3.Text = "N/A";
                txtWeather3.Text = "N/A";
                txtHigh3.Text = "N/A";
                txtLow3.Text = "N/A";
            }
        }

        private string GetDay(int dayOfWeek)
        {
            if (dayOfWeek == 0)
                return "Sunday";
            else if (dayOfWeek == 1)
                return "Monday";
            else if (dayOfWeek == 2)
                return "Tuesday";
            else if (dayOfWeek == 3)
                return "Wednesday";
            else if (dayOfWeek == 4)
                return "Thursday";
            else if (dayOfWeek == 5)
                return "Friday";
            else
                return "Saturday";
        }

        private string GetImagePath(string weatherDescription)
        {
            try
            {
                weatherDescription = weatherDescription.ToLower();

                string imagePath = String.Empty;

                if (weatherDescription == "sunny")
                    imagePath = "/Images/sunny.png";
                else if (weatherDescription == "mostly sunny")
                    imagePath = "/Images/sunny.png";
                else if (weatherDescription == "partly sunny")
                    imagePath = "/Images/partlycloudy.png";
                else if (weatherDescription == "mostly cloudy")
                    imagePath = "/Images/partlycloudy.png";
                else if (weatherDescription == "cloudy")
                    imagePath = "/Images/cloudy.png";
                else if (weatherDescription.Contains("clear"))
                    imagePath = "/Images/sunny.png";
                else if (weatherDescription == "partly cloudy")
                    imagePath = "/Images/partlycloudy.png";
                else if (weatherDescription.Contains("clouds"))
                    imagePath = "/Images/partlycloudy.png";
                else if (weatherDescription.Contains("cloudy"))
                    imagePath = "/Images/partlycloudy.png";
                else if (weatherDescription.Contains("clearing"))
                    imagePath = "/Images/sunny.png";
                else if (weatherDescription.Contains("sunny"))
                    imagePath = "/Images/sunny.png";
                else if (weatherDescription.Contains("fog"))
                    imagePath = "/Images/partlycloudy.png";
                else if (weatherDescription.Contains("snow"))
                    imagePath = "/Images/snow.png";
                else if (weatherDescription.Contains("flurries"))
                    imagePath = "/Images/snow.png";
                else if (weatherDescription.Contains("blizzard"))
                    imagePath = "/Images/snow.png";
                else if (weatherDescription.Contains("ice"))
                    imagePath = "/Images/wintermix.png";
                else if (weatherDescription.Contains("freezing"))
                    imagePath = "/Images/wintermix.png";
                else if (weatherDescription.Contains("sleet"))
                    imagePath = "/Images/wintermix.png";
                else if (weatherDescription.Contains("chance rain showers"))
                    imagePath = "/Images/rainsun.png";
                else if (weatherDescription.Contains("chance rain"))
                    imagePath = "/Images/rainsun.png";
                else if (weatherDescription.Contains("rain showers"))
                    imagePath = "/Images/rain.png";
                else if (weatherDescription.Contains("rain"))
                    imagePath = "/Images/rain.png";
                else if (weatherDescription.Contains("drizzle"))
                    imagePath = "/Images/rainsun.png";
                else if (weatherDescription.Contains("rain/snow"))
                    imagePath = "/Images/wintermix.png";
                else if (weatherDescription.Contains("freezing rain"))
                    imagePath = "/Images/wintermix.png";
                else if (weatherDescription.Contains("wintry mix"))
                    imagePath = "/Images/wintermix.png";
                else if (weatherDescription.Contains("thunderstorms"))
                    imagePath = "/Images/thunderstorm.png";
                else if (weatherDescription.Contains("tstms"))
                    imagePath = "/Images/thunderstorm.png";
                else
                    imagePath = "/Images/partlycloudy.png";

                return imagePath;
            }
            catch { return "/Images/partlycloudy.png"; }
        }
    }
}
