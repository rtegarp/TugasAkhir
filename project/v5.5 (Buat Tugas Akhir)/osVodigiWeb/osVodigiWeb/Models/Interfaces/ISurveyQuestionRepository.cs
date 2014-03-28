using System.Collections.Generic;

namespace osVodigiWeb.Models
{
    public interface ISurveyQuestionRepository
    {
        void CreateSurveyQuestion(SurveyQuestion surveyquestion);
        void DeleteSurveyQuestion(SurveyQuestion surveyquestion);
        void UpdateSurveyQuestion(SurveyQuestion surveyquestion);
        void MoveSurveyQuestion(SurveyQuestion surveyquestion, bool ismoveup);
        SurveyQuestion GetSurveyQuestion(int surveyquestionid);
        IEnumerable<SurveyQuestion> GetSurveyQuestions(int surveyid);
        int SaveChanges();
    }
}