﻿
namespace osVodigiWeb.Controllers
{
    public class ScreenPageState
    {
        public int AccountID { get; set; }
        public string ScreenName { get; set; }
        public string Description { get; set; }
        public bool IncludeInactive { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}