using System;
using System.IO;
using System.Net;

namespace TugasAkhirClient
{
    class DownloadManager
    {
        public static string DownloadFolder = @"C:\osVodigi\";
        public static string MediaSourceUrl = String.Empty;

        public static void CreateDownloadFolders()
        {
            try
            {
                if (!DownloadFolder.EndsWith(@"\"))
                {
                    DownloadFolder += @"\";
                }
                if (!Directory.Exists(DownloadFolder))
                {
                    Directory.CreateDirectory(DownloadFolder);
                }
                if (!Directory.Exists(DownloadFolder + @"Images"))
                {
                    Directory.CreateDirectory(DownloadFolder + @"Images");
                }
                if (!Directory.Exists(DownloadFolder + @"Music"))
                {
                    Directory.CreateDirectory(DownloadFolder + @"Music");
                }
                if (!Directory.Exists(DownloadFolder + @"Videos"))
                {
                    Directory.CreateDirectory(DownloadFolder + @"Videos");
                }
            }
            catch { }
        }

        public static bool DownloadAndSaveFile(string sourceURL, string destinationUri)
        {
            try
            {
                // Don't process if the destination exists
                if (File.Exists(destinationUri))
                    return true;

                // Download the file
                WebClient wcDownload = new WebClient();
                wcDownload.DownloadFile(sourceURL, destinationUri);

                return true;
            }
            catch { return false; }
        }

    }
}
