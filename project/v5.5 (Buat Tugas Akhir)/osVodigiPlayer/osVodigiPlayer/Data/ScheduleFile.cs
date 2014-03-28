using System;
using System.IO;
using System.Configuration;


namespace TugasAkhirClient
{
    class ScheduleFile
    {
        public static string ReadScheduleFile()
        {
            try
            {
                // Make sure the file exists
                string downloadfolder = ConfigurationManager.AppSettings["DownloadFolder"];
                if (!downloadfolder.EndsWith(@"\")) downloadfolder += @"\";
                string sFilePath = downloadfolder + "ScheduleFile.xml";
                if (File.Exists(sFilePath))
                {
                    string xml = String.Empty;

                    // Read the schedule file
                    using (StreamReader reader = new StreamReader(File.Open(sFilePath, FileMode.Open, FileAccess.Read)))
                    {
                        xml = reader.ReadToEnd();
                    }

                    return xml;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch { return String.Empty; }
        }

        public static void SaveScheduleFile(string xml)
        {
            try
            {
                // Delete the file if it exists
                string downloadfolder = ConfigurationManager.AppSettings["DownloadFolder"];
                if (!downloadfolder.EndsWith(@"\")) downloadfolder += @"\";
                string sFilePath = downloadfolder + "ScheduleFile.xml";
                if (File.Exists(sFilePath))
                {
                    File.Delete(sFilePath);
                }

                // Save the file
                File.WriteAllText(sFilePath, xml);
            }
            catch { }
        }

        public static void DeleteScheduleFile()
        {
            try
            {
                string downloadfolder = ConfigurationManager.AppSettings["DownloadFolder"];
                if (!downloadfolder.EndsWith(@"\")) downloadfolder += @"\";
                string sFilePath = downloadfolder + "ScheduleFile.xml";
                if (File.Exists(sFilePath))
                {
                    File.Delete(sFilePath);
                }
            }
            catch {  }            
        }
    }

}
