﻿
namespace osVodigiWeb.Controllers
{
    public class UserPageState
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public bool IncludeInactive { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}