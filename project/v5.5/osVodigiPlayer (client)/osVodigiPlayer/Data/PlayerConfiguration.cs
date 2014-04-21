using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Configuration;

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

namespace osVodigiPlayer
{
    class PlayerConfiguration
    {
        public static int configPlayerID { get; set; }
        public static string configPlayerName { get; set; }
        public static int configAccountID { get; set; }
        public static string configAccountName { get; set; }
        public static bool configIsPlayerInitialized { get; set; }
        public static bool configIsDownloadApproved { get; set; }

        public static void LoadPlayerConfiguration()
        {
            try
            {
                // Make sure the file exists
                string downloadfolder = ConfigurationManager.AppSettings["DownloadFolder"];
                if (!downloadfolder.EndsWith(@"\")) downloadfolder += @"\";
                string sFilePath = downloadfolder + "PlayerConfiguration.xml";
                if (File.Exists(sFilePath))
                {
                    string xml = String.Empty;

                    // Read the registration file
                    using (StreamReader reader = new StreamReader(File.Open(sFilePath, FileMode.Open, FileAccess.Read)))
                    {
                        xml = reader.ReadToEnd();
                    }

                    // Parse the XML

                    // PlayerID
                    try
                    {
                        XDocument xmldoc = XDocument.Parse(xml);
                        configPlayerID = (from PlayerID in xmldoc.Descendants("PlayerID")
                                          select new PlayerID
                                          {
                                              ID = Convert.ToInt32(PlayerID.Value),
                                          }
                        ).First().ID;
                    }
                    catch { configPlayerID = 0; }

                    // Player Name
                    try
                    {
                        XDocument xmldoc = XDocument.Parse(xml);
                        configPlayerName = (from PlayerName in xmldoc.Descendants("PlayerName")
                                            select new PlayerName
                                            {
                                                Name = Convert.ToString(PlayerName.Value),
                                            }
                        ).First().Name;
                    }
                    catch { configPlayerName = "N/A"; }

                    // AccountID
                    try
                    {
                        XDocument xmldoc = XDocument.Parse(xml);
                        configAccountID = (from AccountID in xmldoc.Descendants("AccountID")
                                           select new AccountID
                                          {
                                              ID = Convert.ToInt32(AccountID.Value),
                                          }
                        ).First().ID;
                    }
                    catch { configAccountID = 0; }

                    // Account Name
                    try
                    {
                        XDocument xmldoc = XDocument.Parse(xml);
                        configAccountName = (from AccountName in xmldoc.Descendants("AccountName")
                                             select new AccountName
                                            {
                                                Name = Convert.ToString(AccountName.Value),
                                            }
                        ).First().Name;
                    }
                    catch { configAccountName = "N/A"; }

                    // IsPlayerInitialized
                    try
                    {
                        XDocument xmldoc = XDocument.Parse(xml);
                        configIsPlayerInitialized = (from IsPlayerInitialized in xmldoc.Descendants("IsPlayerInitialized")
                                                     select new IsPlayerInitialized
                                                      {
                                                          PlayerInitialized = Convert.ToBoolean(IsPlayerInitialized.Value),
                                                      }
                        ).First().PlayerInitialized;
                    }
                    catch { configIsPlayerInitialized = false; }

                    // IsDownloadApproved
                    try
                    {
                        XDocument xmldoc = XDocument.Parse(xml);
                        configIsDownloadApproved = (from IsDownloadApproved in xmldoc.Descendants("IsDownloadApproved")
                                                     select new IsDownloadApproved
                                                     {
                                                         DownloadApproved = Convert.ToBoolean(IsDownloadApproved.Value),
                                                     }
                        ).First().DownloadApproved;
                    }
                    catch { configIsDownloadApproved = false; }

                }
                else
                {
                    configPlayerID = 0;
                    configPlayerName = "N/A";
                    configAccountID = 0;
                    configAccountName = "N/A";
                    configIsPlayerInitialized = false;
                    configIsDownloadApproved = false;

                    SavePlayerConfiguration();
                }
            }
            catch { }
        }

        public static void SavePlayerConfiguration()
        {
            try
            {
                // Create the XML
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><PlayerConfiguration>";
                xml += "<PlayerID>" + configPlayerID.ToString() + "</PlayerID>";
                xml += "<PlayerName>" + configPlayerName + "</PlayerName>";
                xml += "<AccountID>" + configAccountID.ToString() + "</AccountID>";
                xml += "<AccountName>" + configAccountName + "</AccountName>";
                if (configIsPlayerInitialized)
                    xml += "<IsPlayerInitialized>true</IsPlayerInitialized>";
                else
                    xml += "<IsPlayerInitialized>false</IsPlayerInitialized>";
                if (configIsDownloadApproved)
                    xml += "<IsDownloadApproved>true</IsDownloadApproved>";
                else
                    xml += "<IsDownloadApproved>false</IsDownloadApproved>";

                xml += "</PlayerConfiguration>";

                // Delete the file if it exists
                string downloadfolder = ConfigurationManager.AppSettings["DownloadFolder"];
                if (!downloadfolder.EndsWith(@"\")) downloadfolder += @"\";
                string sFilePath = downloadfolder + "PlayerConfiguration.xml";
                if (File.Exists(sFilePath))
                {
                    File.Delete(sFilePath);
                }

                // Save the file
                File.WriteAllText(sFilePath, xml);

            }
            catch { }
        }
    }

    class PlayerID
    {
        public int ID;
    }

    class PlayerName
    {
        public string Name;
    }

    class AccountID
    {
        public int ID;
    }

    class AccountName
    {
        public string Name;
    }

    class IsPlayerInitialized
    {
        public bool PlayerInitialized;
    }

    class IsDownloadApproved
    {
        public bool DownloadApproved;
    }

}
