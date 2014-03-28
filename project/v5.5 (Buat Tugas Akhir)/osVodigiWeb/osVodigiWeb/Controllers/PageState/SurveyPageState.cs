﻿
namespace osVodigiWeb.Controllers
{
    public class SurveyPageState
    {
        public int AccountID { get; set; }
        public string SurveyName { get; set; }
        public bool OnlyApproved { get; set; }
        public bool IncludeInactive { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}