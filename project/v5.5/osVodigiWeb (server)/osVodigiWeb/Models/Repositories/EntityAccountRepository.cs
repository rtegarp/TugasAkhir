using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

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

namespace osVodigiWeb.Models
{
    public class EntityAccountRepository : IAccountRepository
    {
        private VodigiContext db = new VodigiContext();

        public Account GetAccount(int id)
        {
            Account account = db.Accounts.Find(id);

            return account;
        }

        public IEnumerable<Account> GetActiveAccounts()
        {
            // Build the query
            var query = from account in db.Accounts
                        select account;

            // Apply the filters first
            query = query.Where(accts => accts.IsActive == true);
            query = query.OrderBy("AccountName", false);

            List<Account> accounts = query.ToList();

            return accounts;
        }

        public IEnumerable<Account> GetAccountByName(string accountname)
        {
            // Build the query
            var query = from account in db.Accounts
                        select account;

            query = query.Where(accts => accts.AccountName == accountname);
            query = query.OrderBy("IsActive", true);

            List<Account> accounts = query.ToList();

            return accounts;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            // Build the query
            var query = from account in db.Accounts
                        select account;

            query = query.OrderBy("AccountName", false);

            List<Account> accounts = query.ToList();

            return accounts;
        }

        public IEnumerable<Account> GetAccountPage(string accountname, string description, bool includeinactive, string sortby, bool isdescending, int pagenumber, int pagecount)
        {
            // Build the query
            var query = from account in db.Accounts
                        select account;

            // Apply the filters first
            if (!String.IsNullOrEmpty(accountname))
                query = query.Where(accts => accts.AccountName.StartsWith(accountname));
            if (!String.IsNullOrEmpty(description))
                query = query.Where(accts => accts.AccountDescription.Contains(description));
            if (!includeinactive)
                query = query.Where(accts => accts.IsActive == true);

            // Apply the ordering
            if (!String.IsNullOrEmpty(sortby))
                query = query.OrderBy(sortby, isdescending);

            // Get a single page from the filtered records
            int iSkip = (pagenumber * Constants.PageSize) - Constants.PageSize;

            List<Account> accounts = query.Skip(iSkip).Take(Constants.PageSize).ToList();

            return accounts;
        }

        public int GetAccountRecordCount(string accountname, string description, bool includeinactive)
        {
            // Build the query
            var query = from account in db.Accounts
                        select account;

            // Apply the filters first
            if (!String.IsNullOrEmpty(accountname))
                query = query.Where(accts => accts.AccountName.StartsWith(accountname));
            if (!String.IsNullOrEmpty(description))
                query = query.Where(accts => accts.AccountDescription.Contains(description));
            if (!includeinactive)
                query = query.Where(accts => accts.IsActive == true);

            // Get a Count of all filtered records
            return query.Count();
        }

        public void CreateAccount(Account account)
        {
            db.Accounts.Add(account);
            db.SaveChanges();
        }

        public void UpdateAccount(Account account)
        {
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public void CreateSampleData(int accountid)
        {
            // Initialize the sample data for a new account so there is data available
            try
            {
                // Copy the five images for the sample
                CopySampleImage(accountid, "6f5e187f-52a2-4799-bdac-2e9199580b98.png");
                CopySampleImage(accountid, "60255096-6a72-409e-b905-4d98ee717bb0.jpg");
                CopySampleImage(accountid, "612bb76c-e16e-4fe8-87f2-bddc7eb59300.jpg");
                CopySampleImage(accountid, "69f99c47-d1b0-4123-b62b-8f18bdc5702f.jpg");
                CopySampleImage(accountid, "626c6a35-4523-46aa-9d0a-c2687b581e27.jpg");

                // Create the image records
                IImageRepository imagerep = new EntityImageRepository();

                Image img1 = new Image();
                img1.AccountID = accountid;
                img1.ImageName = "Visit Las Vegas Button";
                img1.OriginalFilename = "vegasbutton.png";
                img1.StoredFilename = "6f5e187f-52a2-4799-bdac-2e9199580b98.png";
                img1.Tags = "Las Vegas";
                img1.IsActive = true;
                imagerep.CreateImage(img1);

                Image img2 = new Image();
                img2.AccountID = accountid;
                img2.ImageName = "Vegas 01";
                img2.OriginalFilename = "vegas01.jpg";
                img2.StoredFilename = "60255096-6a72-409e-b905-4d98ee717bb0.jpg";
                img2.Tags = "Las Vegas";
                img2.IsActive = true;
                imagerep.CreateImage(img2);

                Image img3 = new Image();
                img3.AccountID = accountid;
                img3.ImageName = "Vegas 02";
                img3.OriginalFilename = "vegas02.jpg";
                img3.StoredFilename = "612bb76c-e16e-4fe8-87f2-bddc7eb59300.jpg";
                img3.Tags = "Las Vegas";
                img3.IsActive = true;
                imagerep.CreateImage(img3);

                Image img4 = new Image();
                img4.AccountID = accountid;
                img4.ImageName = "Vegas 03";
                img4.OriginalFilename = "vegas03.jpg";
                img4.StoredFilename = "69f99c47-d1b0-4123-b62b-8f18bdc5702f.jpg";
                img4.Tags = "Las Vegas";
                img4.IsActive = true;
                imagerep.CreateImage(img4);

                Image img5 = new Image();
                img5.AccountID = accountid;
                img5.ImageName = "Vegas 04";
                img5.OriginalFilename = "vegas04.jpg";
                img5.StoredFilename = "626c6a35-4523-46aa-9d0a-c2687b581e27.jpg";
                img5.Tags = "Las Vegas";
                img5.IsActive = true;
                imagerep.CreateImage(img5);

                // Create a slideshow with the four images
                ISlideShowRepository slideshowrep = new EntitySlideShowRepository();
                ISlideShowImageXrefRepository ssixrefrep = new EntitySlideShowImageXrefRepository();

                SlideShow slideshow = new SlideShow();
                slideshow.AccountID = accountid;
                slideshow.SlideShowName = "Visit Vegas Slideshow";
                slideshow.Tags = "Las Vegas";
                slideshow.TransitionType = "Fade";
                slideshow.IntervalInSecs = 10;
                slideshow.IsActive = true;
                slideshowrep.CreateSlideShow(slideshow);

                SlideShowImageXref xref1 = new SlideShowImageXref();
                xref1.PlayOrder = 1;
                xref1.SlideShowID = slideshow.SlideShowID;
                xref1.ImageID = img2.ImageID;
                ssixrefrep.CreateSlideShowImageXref(xref1);

                SlideShowImageXref xref2 = new SlideShowImageXref();
                xref2.PlayOrder = 2;
                xref2.SlideShowID = slideshow.SlideShowID;
                xref2.ImageID = img3.ImageID;
                ssixrefrep.CreateSlideShowImageXref(xref2);

                SlideShowImageXref xref3 = new SlideShowImageXref();
                xref3.PlayOrder = 3;
                xref3.SlideShowID = slideshow.SlideShowID;
                xref3.ImageID = img4.ImageID;
                ssixrefrep.CreateSlideShowImageXref(xref3);

                SlideShowImageXref xref4 = new SlideShowImageXref();
                xref4.PlayOrder = 4;
                xref4.SlideShowID = slideshow.SlideShowID;
                xref4.ImageID = img5.ImageID;
                ssixrefrep.CreateSlideShowImageXref(xref4);

                // Create one screencontent item for each image
                IScreenContentRepository screencontentrep = new EntityScreenContentRepository();

                ScreenContent content1 = new ScreenContent();
                content1.AccountID = accountid;
                content1.ScreenContentName = "Vegas 01 Image";
                content1.ScreenContentTitle = "Las Vegas Is Fun!";
                content1.ScreenContentTypeID = 1000000;
                content1.ThumbnailImageID = img2.ImageID;
                content1.CustomField1 = img2.ImageID.ToString();
                content1.CustomField2 = String.Empty;
                content1.CustomField3 = String.Empty;
                content1.CustomField4 = String.Empty;
                content1.IsActive = true;
                screencontentrep.CreateScreenContent(content1);

                ScreenContent content2 = new ScreenContent();
                content2.AccountID = accountid;
                content2.ScreenContentName = "Vegas 02 Image";
                content2.ScreenContentTitle = "Visit Las Vegas!";
                content2.ScreenContentTypeID = 1000000;
                content2.ThumbnailImageID = img3.ImageID;
                content2.CustomField1 = img3.ImageID.ToString();
                content2.CustomField2 = String.Empty;
                content2.CustomField3 = String.Empty;
                content2.CustomField4 = String.Empty;
                content2.IsActive = true;
                screencontentrep.CreateScreenContent(content2);

                ScreenContent content3 = new ScreenContent();
                content3.AccountID = accountid;
                content3.ScreenContentName = "Vegas 03 Image";
                content3.ScreenContentTitle = "There's so much to do in Vegas!";
                content3.ScreenContentTypeID = 1000000;
                content3.ThumbnailImageID = img4.ImageID;
                content3.CustomField1 = img4.ImageID.ToString();
                content3.CustomField2 = String.Empty;
                content3.CustomField3 = String.Empty;
                content3.CustomField4 = String.Empty;
                content3.IsActive = true;
                screencontentrep.CreateScreenContent(content3);

                ScreenContent content4 = new ScreenContent();
                content4.AccountID = accountid;
                content4.ScreenContentName = "Vegas 04 Image";
                content4.ScreenContentTitle = "Good times, day or night!";
                content4.ScreenContentTypeID = 1000000;
                content4.ThumbnailImageID = img5.ImageID;
                content4.CustomField1 = img5.ImageID.ToString();
                content4.CustomField2 = String.Empty;
                content4.CustomField3 = String.Empty;
                content4.CustomField4 = String.Empty;
                content4.IsActive = true;
                screencontentrep.CreateScreenContent(content4);

                // Create the screen with slideshow and four screen content items
                IScreenRepository screenrep = new EntityScreenRepository();
                IScreenScreenContentXrefRepository sscxrefrep = new EntityScreenScreenContentXrefRepository();

                Screen screen = new Screen();
                screen.ButtonImageID = img1.ImageID;
                screen.AccountID = accountid;
                screen.IsInteractive = true;
                screen.PlayListID = 0;
                screen.SlideShowID = slideshow.SlideShowID;
                screen.ScreenName = "Visit Las Vegas";
                screen.ScreenDescription = String.Empty;
                screen.IsActive = true;
                screenrep.CreateScreen(screen);

                ScreenScreenContentXref sscxref1 = new ScreenScreenContentXref();
                sscxref1.ScreenID = screen.ScreenID;
                sscxref1.ScreenContentID = content1.ScreenContentID;
                sscxref1.DisplayOrder = 1;
                sscxrefrep.CreateScreenScreenContentXref(sscxref1);

                ScreenScreenContentXref sscxref2 = new ScreenScreenContentXref();
                sscxref2.ScreenID = screen.ScreenID;
                sscxref2.ScreenContentID = content2.ScreenContentID;
                sscxref2.DisplayOrder = 2;
                sscxrefrep.CreateScreenScreenContentXref(sscxref2);

                ScreenScreenContentXref sscxref3 = new ScreenScreenContentXref();
                sscxref3.ScreenID = screen.ScreenID;
                sscxref3.ScreenContentID = content3.ScreenContentID;
                sscxref3.DisplayOrder = 3;
                sscxrefrep.CreateScreenScreenContentXref(sscxref3);

                ScreenScreenContentXref sscxref4 = new ScreenScreenContentXref();
                sscxref4.ScreenID = screen.ScreenID;
                sscxref4.ScreenContentID = content4.ScreenContentID;
                sscxref4.DisplayOrder = 4;
                sscxrefrep.CreateScreenScreenContentXref(sscxref4);

                // Create a PlayerGroup - My Players
                IPlayerGroupRepository playergrouprep = new EntityPlayerGroupRepository();
                PlayerGroup playergroup = new PlayerGroup();
                playergroup.AccountID = accountid;
                playergroup.PlayerGroupName = "My Players";
                playergroup.PlayerGroupDescription = String.Empty;
                playergroup.IsActive = true;
                playergrouprep.CreatePlayerGroup(playergroup);

                // Create a Player - My Player
                IPlayerRepository playerrep = new EntityPlayerRepository();
                Player player = new Player();
                player.AccountID = accountid;
                player.PlayerGroupID = playergroup.PlayerGroupID;
                player.PlayerName = "My Player";
                player.PlayerLocation = String.Empty;
                player.PlayerDescription = String.Empty;
                player.IsActive = true;
                playerrep.CreatePlayer(player);

                // Create the schedule for My Players player group
                IPlayerGroupScheduleRepository schedulerep = new EntityPlayerGroupScheduleRepository();

                PlayerGroupSchedule schedule1 = new PlayerGroupSchedule();
                schedule1.PlayerGroupID = playergroup.PlayerGroupID;
                schedule1.ScreenID = screen.ScreenID;
                schedule1.Day = 0;
                schedule1.Hour = 0;
                schedule1.Minute = 0;
                schedulerep.CreatePlayerGroupSchedule(schedule1);

                PlayerGroupSchedule schedule2 = new PlayerGroupSchedule();
                schedule2.PlayerGroupID = playergroup.PlayerGroupID;
                schedule2.ScreenID = screen.ScreenID;
                schedule2.Day = 1;
                schedule2.Hour = 0;
                schedule2.Minute = 0;
                schedulerep.CreatePlayerGroupSchedule(schedule2);

                PlayerGroupSchedule schedule3 = new PlayerGroupSchedule();
                schedule3.PlayerGroupID = playergroup.PlayerGroupID;
                schedule3.ScreenID = screen.ScreenID;
                schedule3.Day = 2;
                schedule3.Hour = 0;
                schedule3.Minute = 0;
                schedulerep.CreatePlayerGroupSchedule(schedule3);

                PlayerGroupSchedule schedule4 = new PlayerGroupSchedule();
                schedule4.PlayerGroupID = playergroup.PlayerGroupID;
                schedule4.ScreenID = screen.ScreenID;
                schedule4.Day = 3;
                schedule4.Hour = 0;
                schedule4.Minute = 0;
                schedulerep.CreatePlayerGroupSchedule(schedule4);

                PlayerGroupSchedule schedule5 = new PlayerGroupSchedule();
                schedule5.PlayerGroupID = playergroup.PlayerGroupID;
                schedule5.ScreenID = screen.ScreenID;
                schedule5.Day = 4;
                schedule5.Hour = 0;
                schedule5.Minute = 0;
                schedulerep.CreatePlayerGroupSchedule(schedule5);

                PlayerGroupSchedule schedule6 = new PlayerGroupSchedule();
                schedule6.PlayerGroupID = playergroup.PlayerGroupID;
                schedule6.ScreenID = screen.ScreenID;
                schedule6.Day = 5;
                schedule6.Hour = 0;
                schedule6.Minute = 0;
                schedulerep.CreatePlayerGroupSchedule(schedule6);

                PlayerGroupSchedule schedule7 = new PlayerGroupSchedule();
                schedule7.PlayerGroupID = playergroup.PlayerGroupID;
                schedule7.ScreenID = screen.ScreenID;
                schedule7.Day = 6;
                schedule7.Hour = 0;
                schedule7.Minute = 0;
                schedulerep.CreatePlayerGroupSchedule(schedule7);
            }
            catch { }
        }

        private bool CopySampleImage(int accountid, string filename)
        {
            try
            {
                string sourceimage = HttpContext.Current.Server.MapPath(@"~/SampleImages/" + filename);
                string newimage = HttpContext.Current.Server.MapPath(@"~/Media");
                if (!newimage.EndsWith(@"\"))
                    newimage += @"\";
                System.IO.Directory.CreateDirectory(newimage + Convert.ToString(accountid) + @"\Images\");
                newimage += Convert.ToString(accountid) + @"\Images\" + filename;
                System.IO.File.Copy(sourceimage, newimage);

                return true;
            }
            catch { return false; }
        }
    }
}