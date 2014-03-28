﻿
namespace osVodigiWeb.Controllers
{
    public class VideoPageState
    {
        public int AccountID { get; set; }
        public string VideoName { get; set; }
        public string Tag { get; set; }
        public bool IncludeInactive { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}