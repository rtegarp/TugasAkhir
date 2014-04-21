
namespace osVodigiWeb.Controllers
{
    public class PlayListPageState
    {
        public int AccountID { get; set; }
        public string PlayListName { get; set; }
        public string Tag { get; set; }
        public bool IncludeInactive { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}