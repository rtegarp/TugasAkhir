
namespace osVodigiWeb.Controllers
{
    public class ReportLoginLogPageState
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}