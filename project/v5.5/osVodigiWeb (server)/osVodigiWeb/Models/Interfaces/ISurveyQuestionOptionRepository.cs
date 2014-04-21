using System.Collections.Generic;

namespace osVodigiWeb.Models
{
    public interface ISurveyQuestionOptionRepository
    {
        void CreateSurveyQuestionOption(SurveyQuestionOption surveyquestionoption);
        void DeleteSurveyQuestionOption(SurveyQuestionOption surveyquestionoption);
        void UpdateSurveyQuestionOption(SurveyQuestionOption surveyquestionoption);
        void MoveSurveyQuestionOption(SurveyQuestionOption surveyquestionoption, bool ismoveup);
        SurveyQuestionOption GetSurveyQuestionOption(int surveyquestionoptionid);
        IEnumerable<SurveyQuestionOption> GetSurveyQuestionOptions(int surveyquestionid);
        int SaveChanges();
    }
}