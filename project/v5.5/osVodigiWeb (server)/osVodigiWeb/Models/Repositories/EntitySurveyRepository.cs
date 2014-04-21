using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace osVodigiWeb.Models
{
    public class EntitySurveyRepository : ISurveyRepository
    {
        private VodigiContext db = new VodigiContext();

        public Survey GetSurvey(int id)
        {
            Survey survey = db.Surveys.Find(id);

            return survey;
        }

        public IEnumerable<Survey> GetAllSurveys(int accountid)
        {
            var query = from survey in db.Surveys
                        select survey;
            query = query.Where(svs => svs.AccountID.Equals(accountid));
            query = query.OrderBy("SurveyName", false);

            List<Survey> surveys = query.ToList();

            return surveys;
        }

        public IEnumerable<Survey> GetActiveSurveys(int accountid)
        {
            var query = from survey in db.Surveys
                        select survey;
            query = query.Where(svs => svs.AccountID.Equals(accountid));
            query = query.Where(svs => svs.IsActive == true);
            query = query.OrderBy("SurveyName", false);

            List<Survey> surveys = query.ToList();

            return surveys;
        }

        public IEnumerable<Survey> GetApprovedSurveys(int accountid)
        {
            var query = from survey in db.Surveys
                        select survey;
            query = query.Where(svs => svs.AccountID.Equals(accountid));
            query = query.Where(svs => svs.IsApproved == true);
            query = query.OrderBy("SurveyName", false);

            List<Survey> surveys = query.ToList();

            return surveys;
        }

        public IEnumerable<Survey> GetSurveyPage(int accountid, string surveyname, bool onlyapproved, bool includeinactive, string sortby, bool isdescending, int pagenumber, int pagecount)
        {
            var query = from survey in db.Surveys
                        select survey;

            query = query.Where(svs => svs.AccountID.Equals(accountid));
            if (!String.IsNullOrEmpty(surveyname))
                query = query.Where(svs => svs.SurveyName.StartsWith(surveyname));
            if (onlyapproved)
                query = query.Where(svs => svs.IsApproved == true);
            if (!includeinactive)
                query = query.Where(svs => svs.IsActive == true);

            if (!String.IsNullOrEmpty(sortby))
                query = query.OrderBy(sortby, isdescending);

            // Get a single page from the filtered records
            int iSkip = (pagenumber * Constants.PageSize) - Constants.PageSize;

            List<Survey> surveys = query.Skip(iSkip).Take(Constants.PageSize).ToList();

            return surveys;
        }

        public int GetSurveyRecordCount(int accountid, string surveyname, bool onlyapproved, bool includeinactive)
        {
            var query = from survey in db.Surveys
                        select survey;

            query = query.Where(svs => svs.AccountID.Equals(accountid));
            if (!String.IsNullOrEmpty(surveyname))
                query = query.Where(svs => svs.SurveyName.StartsWith(surveyname));
            if (onlyapproved)
                query = query.Where(svs => svs.IsApproved == true);
            if (!includeinactive)
                query = query.Where(svs => svs.IsActive == true);

            return query.Count();
        }

        public void CreateSurvey(Survey survey)
        {
            db.Surveys.Add(survey);
            db.SaveChanges();
        }

        public void UpdateSurvey(Survey survey)
        {
            db.Entry(survey).State = EntityState.Modified;
            db.SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}