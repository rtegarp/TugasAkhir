using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;


namespace TugasAkhirClient
{
    class CurrentSchedule
    {
        public static List<PlayerGroupSchedule> PlayerGroupSchedules; // Should only be one
        public static List<Screen> Screens;
        public static List<PlayList> PlayLists;
        public static List<PlayListVideoXref> PlayListVideoXrefs;
        public static List<SlideShow> SlideShows;
        public static List<SlideShowImageXref> SlideShowImageXrefs;
        public static List<SlideShowMusicXref> SlideShowMusicXrefs;
        public static List<ScreenScreenContentXref> ScreenScreenContentXrefs;
        public static List<ScreenContent> ScreenContents;
        public static List<Image> Images;
        public static List<Video> Videos;
        public static List<Music> Musics;
        public static List<Survey> Surveys;
        public static List<SurveyQuestion> SurveyQuestions;
        public static List<SurveyQuestionOption> SurveyQuestionOptions;
        public static string LastScheduleXML;

        public static void ParseScheduleXml(string xml)
        {
            try
            {
                // Get the PlayerGroupSchedule(s)
                try
                {
                    List<PlayerGroupSchedule> pgs = new List<PlayerGroupSchedule>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    pgs = (from PlayerGroupSchedule in xmldoc.Descendants("PlayerGroupSchedule")
                           select new PlayerGroupSchedule
                           {
                               PlayerGroupScheduleID = Convert.ToInt32(PlayerGroupSchedule.Attribute("PlayerGroupScheduleID").Value),
                               PlayerGroupID = Convert.ToInt32(PlayerGroupSchedule.Attribute("PlayerGroupID").Value),
                               ScreenID = Convert.ToInt32(PlayerGroupSchedule.Attribute("ScreenID").Value),
                               Day = Convert.ToInt32(PlayerGroupSchedule.Attribute("Day").Value),
                               Hour = Convert.ToInt32(PlayerGroupSchedule.Attribute("Hour").Value),
                               Minute = Convert.ToInt32(PlayerGroupSchedule.Attribute("Minute").Value),
                           }
                    ).ToList();

                    PlayerGroupSchedules = pgs;
                }
                catch { }

                // Parse out the Screens
                try
                {
                    List<Screen> ss = new List<Screen>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    ss = (from Screen in xmldoc.Descendants("Screen")
                          select new Screen
                          {
                              ScreenID = Convert.ToInt32(Screen.Attribute("ScreenID").Value),
                              AccountID = Convert.ToInt32(Screen.Attribute("AccountID").Value),
                              ScreenName = Utility.DecodeXMLString(Convert.ToString(Screen.Attribute("ScreenName").Value)),
                              PlayListID = Convert.ToInt32(Screen.Attribute("PlayListID").Value),
                              SlideShowID = Convert.ToInt32(Screen.Attribute("SlideShowID").Value),
                              ButtonImageID = Convert.ToInt32(Screen.Attribute("ButtonImageID").Value),
                              IsInteractive = Convert.ToBoolean(Screen.Attribute("IsInteractive").Value),
                          }
                    ).ToList();

                    Screens = ss;
                }
                catch { }

                // Parse out the ScreenScreenContentXrefs
                try
                {
                    List<ScreenScreenContentXref> sscxrefs = new List<ScreenScreenContentXref>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    sscxrefs = (from ScreenScreenContentXref in xmldoc.Descendants("ScreenScreenContentXref")
                                select new ScreenScreenContentXref
                                {
                                    ScreenScreenContentXrefID = Convert.ToInt32(ScreenScreenContentXref.Attribute("ScreenScreenContentXrefID").Value),
                                    ScreenID = Convert.ToInt32(ScreenScreenContentXref.Attribute("ScreenID").Value),
                                    ScreenContentID = Convert.ToInt32(ScreenScreenContentXref.Attribute("ScreenContentID").Value),
                                    DisplayOrder = Convert.ToInt32(ScreenScreenContentXref.Attribute("ScreenID").Value),
                                }
                    ).ToList();

                    ScreenScreenContentXrefs = sscxrefs;
                }
                catch { }

                // Parse out the ScreenContents
                try
                {
                    List<ScreenContent> scs = new List<ScreenContent>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    scs = (from ScreenContent in xmldoc.Descendants("ScreenContent")
                           select new ScreenContent
                           {
                               ScreenContentID = Convert.ToInt32(ScreenContent.Attribute("ScreenContentID").Value),
                               ScreenContentTypeID = Convert.ToInt32(ScreenContent.Attribute("ScreenContentTypeID").Value),
                               ScreenContentTypeName = Utility.DecodeXMLString(Convert.ToString(ScreenContent.Attribute("ScreenContentTypeName").Value)),
                               ScreenContentName = Utility.DecodeXMLString(Convert.ToString(ScreenContent.Attribute("ScreenContentName").Value)),
                               ScreenContentTitle = Convert.ToString(ScreenContent.Attribute("ScreenContentTitle").Value),
                               ThumbnailImageID = Convert.ToInt32(ScreenContent.Attribute("ThumbnailImageID").Value),
                               CustomField1 = Utility.DecodeXMLString(Convert.ToString(ScreenContent.Attribute("CustomField1").Value)),
                               CustomField2 = Utility.DecodeXMLString(Convert.ToString(ScreenContent.Attribute("CustomField2").Value)),
                               CustomField3 = Utility.DecodeXMLString(Convert.ToString(ScreenContent.Attribute("CustomField3").Value)),
                               CustomField4 = Utility.DecodeXMLString(Convert.ToString(ScreenContent.Attribute("CustomField4").Value)),
                           }
                    ).ToList();

                    ScreenContents = scs;
                }
                catch { }

                // Parse out the SlideShows
                try
                {
                    List<SlideShow> sss = new List<SlideShow>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    sss = (from SlideShow in xmldoc.Descendants("SlideShow")
                           select new SlideShow
                           {
                               SlideShowID = Convert.ToInt32(SlideShow.Attribute("SlideShowID").Value),
                               IntervalInSecs = Convert.ToInt32(SlideShow.Attribute("IntervalInSecs").Value),
                               TransitionType = Utility.DecodeXMLString(Convert.ToString(SlideShow.Attribute("TransitionType").Value)),
                           }
                    ).ToList();

                    SlideShows = sss;
                }
                catch { }

                // Parse out the SlideShowImageXrefs
                try
                {
                    List<SlideShowImageXref> ssis = new List<SlideShowImageXref>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    ssis = (from SlideShowImageXref in xmldoc.Descendants("SlideShowImageXref")
                            select new SlideShowImageXref
                            {
                                SlideShowImageXrefID = Convert.ToInt32(SlideShowImageXref.Attribute("SlideShowImageXrefID").Value),
                                SlideShowID = Convert.ToInt32(SlideShowImageXref.Attribute("SlideShowID").Value),
                                ImageID = Convert.ToInt32(SlideShowImageXref.Attribute("ImageID").Value),
                                PlayOrder = Convert.ToInt32(SlideShowImageXref.Attribute("PlayOrder").Value),
                            }
                    ).ToList();

                    SlideShowImageXrefs = ssis;
                }
                catch { }

                // Parse out the SlideShowMusicXrefs
                try
                {
                    List<SlideShowMusicXref> ssms = new List<SlideShowMusicXref>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    ssms = (from SlideShowMusicXref in xmldoc.Descendants("SlideShowMusicXref")
                            select new SlideShowMusicXref
                            {
                                SlideShowMusicXrefID = Convert.ToInt32(SlideShowMusicXref.Attribute("SlideShowMusicXrefID").Value),
                                SlideShowID = Convert.ToInt32(SlideShowMusicXref.Attribute("SlideShowID").Value),
                                MusicID = Convert.ToInt32(SlideShowMusicXref.Attribute("MusicID").Value),
                                PlayOrder = Convert.ToInt32(SlideShowMusicXref.Attribute("PlayOrder").Value),
                            }
                    ).ToList();

                    SlideShowMusicXrefs = ssms;
                }
                catch { }

                // Parse out the Images
                try
                {
                    List<Image> images = new List<Image>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    images = (from Image in xmldoc.Descendants("Image")
                              select new Image
                              {
                                  ImageID = Convert.ToInt32(Image.Attribute("ImageID").Value),
                                  StoredFilename = Convert.ToString(Image.Attribute("StoredFilename").Value),
                                  ImageName = Utility.DecodeXMLString(Image.Attribute("ImageName").Value),
                              }
                    ).ToList();

                    Images = images;
                }
                catch { }

                // Parse out the Musics
                try
                {
                    List<Music> musics = new List<Music>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    musics = (from Music in xmldoc.Descendants("Music")
                              select new Music
                              {
                                  MusicID = Convert.ToInt32(Music.Attribute("MusicID").Value),
                                  StoredFilename = Convert.ToString(Music.Attribute("StoredFilename").Value),
                                  MusicName = Utility.DecodeXMLString(Music.Attribute("MusicName").Value),
                              }
                    ).ToList();

                    Musics = musics;
                }
                catch { }

                // Parse out the PlayLists
                try
                {
                    List<PlayList> pls = new List<PlayList>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    pls = (from PlayList in xmldoc.Descendants("PlayList")
                           select new PlayList
                           {
                               PlayListID = Convert.ToInt32(PlayList.Attribute("PlayListID").Value),
                           }
                    ).ToList();

                    PlayLists = pls;
                }
                catch { }

                // Parse out the PlayListVideoXrefs
                try
                {
                    List<PlayListVideoXref> plvs = new List<PlayListVideoXref>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    plvs = (from PlayListVideoXref in xmldoc.Descendants("PlayListVideoXref")
                            select new PlayListVideoXref
                            {
                                PlayListVideoXrefID = Convert.ToInt32(PlayListVideoXref.Attribute("PlayListVideoXrefID").Value),
                                PlayListID = Convert.ToInt32(PlayListVideoXref.Attribute("PlayListID").Value),
                                VideoID = Convert.ToInt32(PlayListVideoXref.Attribute("VideoID").Value),
                                PlayOrder = Convert.ToInt32(PlayListVideoXref.Attribute("PlayOrder").Value),
                            }
                    ).ToList();

                    PlayListVideoXrefs = plvs;
                }
                catch { }

                // Parse out the Videos
                try
                {
                    List<Video> videos = new List<Video>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    videos = (from Video in xmldoc.Descendants("Video")
                              select new Video
                              {
                                  VideoID = Convert.ToInt32(Video.Attribute("VideoID").Value),
                                  StoredFilename = Convert.ToString(Video.Attribute("StoredFilename").Value),
                                  VideoName = Utility.DecodeXMLString(Convert.ToString(Video.Attribute("VideoName").Value)),
                              }
                    ).ToList();

                    Videos = videos;
                }
                catch { }

                // Parse out the Surveys
                try
                {
                    List<Survey> surveys = new List<Survey>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    surveys = (from Survey in xmldoc.Descendants("Survey")
                               select new Survey
                               {
                                   SurveyID = Convert.ToInt32(Survey.Attribute("SurveyID").Value),
                                   SurveyName = Utility.DecodeXMLString(Convert.ToString(Survey.Attribute("SurveyName").Value)),
                                   SurveyImageID = Convert.ToInt32(Survey.Attribute("SurveyImageID").Value),
                               }
                    ).ToList();

                    Surveys = surveys;
                }
                catch { }

                // Parse out the SurveyQuestions
                try
                {
                    List<SurveyQuestion> questions = new List<SurveyQuestion>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    questions = (from SurveyQuestion in xmldoc.Descendants("SurveyQuestion")
                                 select new SurveyQuestion
                                 {
                                     SurveyQuestionID = Convert.ToInt32(SurveyQuestion.Attribute("SurveyQuestionID").Value),
                                     SurveyID = Convert.ToInt32(SurveyQuestion.Attribute("SurveyID").Value),
                                     SurveyQuestionText = Utility.DecodeXMLString(Convert.ToString(SurveyQuestion.Attribute("SurveyQuestionText").Value)),
                                     AllowMultiselect = Convert.ToBoolean(SurveyQuestion.Attribute("AllowMultiselect").Value),
                                     SortOrder = Convert.ToInt32(SurveyQuestion.Attribute("SortOrder").Value),
                                 }
                    ).ToList();

                    SurveyQuestions = questions;
                }
                catch { }

                // Parse out the SurveyQuestionOptions
                try
                {
                    List<SurveyQuestionOption> options = new List<SurveyQuestionOption>();
                    XDocument xmldoc = XDocument.Parse(xml);
                    options = (from SurveyQuestionOption in xmldoc.Descendants("SurveyQuestionOption")
                               select new SurveyQuestionOption
                               {

                                   SurveyQuestionOptionID = Convert.ToInt32(SurveyQuestionOption.Attribute("SurveyQuestionOptionID").Value),
                                   SurveyQuestionID = Convert.ToInt32(SurveyQuestionOption.Attribute("SurveyQuestionID").Value),
                                   SurveyQuestionOptionText = Utility.DecodeXMLString(Convert.ToString(SurveyQuestionOption.Attribute("SurveyQuestionOptionText").Value)),
                                   SortOrder = Convert.ToInt32(SurveyQuestionOption.Attribute("SortOrder").Value),
                               }
                    ).ToList();

                    SurveyQuestionOptions = options;
                }
                catch { }
            }

            catch { }
        }

        public static void ClearSchedule()
        {
            try
            {
                // Clear the global data used to store schedule data
                PlayerGroupSchedules = new List<PlayerGroupSchedule>();
                Screens = new List<Screen>();
                PlayListVideoXrefs = new List<PlayListVideoXref>();
                SlideShowImageXrefs = new List<SlideShowImageXref>();
                ScreenScreenContentXrefs = new List<ScreenScreenContentXref>();
                ScreenContents = new List<ScreenContent>();
                Images = new List<Image>();
                Videos = new List<Video>();
                Surveys = new List<Survey>();
                SurveyQuestions = new List<SurveyQuestion>();
                SurveyQuestionOptions = new List<SurveyQuestionOption>();
            }
            catch { }
        }
    }
}
