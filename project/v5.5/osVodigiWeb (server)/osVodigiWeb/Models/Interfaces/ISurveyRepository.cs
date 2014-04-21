using System.Collections.Generic;

namespace osVodigiWeb.Models
{
    public interface ISurveyRepository
    {
        void CreateSurvey(Survey survey);
        void UpdateSurvey(Survey survey);
        Survey GetSurvey(int surveyid);
        IEnumerable<Survey> GetAllSurveys(int accountid);
        IEnumerable<Survey> GetActiveSurveys(int accountid);
        IEnumerable<Survey> GetApprovedSurveys(int accountid);
        IEnumerable<Survey> GetSurveyPage(int accountid, string surveyname, bool onlyapproved, bool includeinactive, string sortby, bool isdescending, int pagenumber, int pagecount);
        int GetSurveyRecordCount(int accountid, string surveyname, bool onlyapproved, bool includeinactive);
        int SaveChanges();
    }
}