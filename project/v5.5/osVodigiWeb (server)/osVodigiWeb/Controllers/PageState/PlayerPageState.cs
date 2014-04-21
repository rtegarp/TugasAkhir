
namespace osVodigiWeb.Controllers
{
    public class PlayerPageState
    {
        public int AccountID { get; set; }
        public int PlayerGroupID { get; set; }
        public string PlayerName { get; set; }
        public bool IncludeInactive { get; set; }
        public string SortBy { get; set; }
        public string AscDesc { get; set; }
        public int PageNumber { get; set; }
    }
}