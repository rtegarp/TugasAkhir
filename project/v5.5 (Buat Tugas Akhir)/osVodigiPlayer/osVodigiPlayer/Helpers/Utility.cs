using System;
using System.Text;
using System.IO;
using System.Configuration;

namespace TugasAkhirClient
{
    class Utility
    {
        public static string EncodeXMLString(string xmlin)
        {
            try
            {
                return xmlin.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
            }
            catch
            {
                return xmlin;
            }
        }

        public static string DecodeXMLString(string xmlin)
        {
            try
            {
                return xmlin.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
            }
            catch
            {
                return xmlin;
            }
        }

        public static bool SaveWebserviceURL(string url)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string filepath = GetAppConfigFilePath();

                bool found = false;
                using (StreamReader reader = new StreamReader(filepath))
                {
                    String line;
                    while (!found && (line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("<endpoint") && line.Contains("address="))
                        {
                            found = true;
                            line = "<endpoint address=\"" + url + "\"";
                        }
                        sb.AppendLine(line);
                    }
                }

                BackupAndSaveAppConfig(filepath, sb.ToString());

                ConfigurationManager.RefreshSection("client");

                return true;
            }
            catch { return false; }
        }

        public static string GetWebserviceURL()
        {
            try
            {
                string url = String.Empty;
                string filepath = GetAppConfigFilePath();
                bool found = false;
                using (StreamReader reader = new StreamReader(filepath))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null && !found)
                    {
                        if (line.Contains("<endpoint") && line.Contains("address="))
                        {
                            int startindex = line.IndexOf("address=\"");
                            int endindex = line.LastIndexOf("\"");
                            url = line.Substring(startindex, endindex - startindex);
                            url = url.Replace("address=\"", "").Replace("\"", "");
                            found = true;
                        }
                    }
                }

                return url.Trim();
            }
            catch { return String.Empty; }
        }

        public static string GetAppSetting(string keyname)
        {
            try
            {
                string keyvalue = String.Empty;
                string filepath = GetAppConfigFilePath();

                using (StreamReader reader = new StreamReader(filepath))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("<add") && line.Contains("key=\"" + keyname + "\""))
                        {
                            int startindex = line.IndexOf("value=\"");
                            int endindex = line.LastIndexOf("\"");
                            keyvalue = line.Substring(startindex, endindex - startindex);
                            keyvalue = keyvalue.Replace("value=\"", "").Replace("<", "").Replace("/>", "").Replace("\"", "");
                        }
                    }
                }

                return keyvalue.Trim();
            }
            catch { return String.Empty; }
        }

        public static bool SaveAppSetting(string keyname, string newkeyvalue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string filepath = GetAppConfigFilePath();

                // Read each line and update the appropriate value
                using (StreamReader reader = new StreamReader(filepath))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("<add") && line.Contains("key=\"" + keyname + "\""))
                        {
                            string oldkeyvalue = String.Empty;
                            int startindex = line.IndexOf("value=\"");
                            int endindex = line.LastIndexOf("\"");
                            oldkeyvalue = line.Substring(startindex, endindex - startindex);
                            oldkeyvalue = oldkeyvalue.Replace("value=\"", "").Replace("<", "").Replace("/>", "").Replace("\"", "");

                            line = line.Replace("value=\"" + oldkeyvalue + "\"", "value=\"" + newkeyvalue + "\"");
                        }

                        sb.AppendLine(line);
                    }
                }

                BackupAndSaveAppConfig(filepath, sb.ToString());

                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            catch { return false; }
        }

        public static string GetAppConfigFilePath()
        {
            try
            {
                string filepath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");
                if (!filepath.EndsWith(@"\"))
                    filepath += @"\";
                filepath += "osVodigiPlayer.exe.config";

                return filepath;
            }
            catch { return String.Empty; }
        }

        public static bool BackupAndSaveAppConfig(string filepath, string filecontents)
        {
            try
            {
                // Creates a backup of the App.Config and updates App.Config
                File.Copy(filepath, filepath + "." +
                                String.Format("{0:00}", DateTime.UtcNow.Month) + "-" +
                                String.Format("{0:00}", DateTime.UtcNow.Day) + "-" +
                                String.Format("{0:0000}", DateTime.UtcNow.Year) + "-" +
                                String.Format("{0:00}", DateTime.UtcNow.Hour) + "-" +
                                String.Format("{0:00}", DateTime.UtcNow.Minute) + "-" +
                                String.Format("{0:00}", DateTime.UtcNow.Second) + "-" +
                                String.Format("{0:0000}", DateTime.UtcNow.Millisecond));
                File.Delete(filepath);
                File.WriteAllText(filepath, filecontents);

                return true;
            }
            catch { return false; }

        }
    }
}
