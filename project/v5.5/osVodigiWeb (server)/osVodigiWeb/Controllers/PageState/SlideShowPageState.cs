﻿
namespace osVodigiWeb.Controllers
{
    public class SlideShowPageState
    {
        public int AccountID { get; set; }
        public string SlideShowName { get; set; }
        public string Tag { get; set; }
        public bool IncludeInactive { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}