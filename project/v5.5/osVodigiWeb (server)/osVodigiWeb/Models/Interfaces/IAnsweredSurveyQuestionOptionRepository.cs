using System.Collections.Generic;

/* ----------------------------------------------------------------------------------------
    Vodigi - Open Source Interactive Digital Signage
    Copyright (C) 2005-2012  JMC Publications, LLC

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
---------------------------------------------------------------------------------------- */

namespace osVodigiWeb.Models
{
    public interface IAnsweredSurveyQuestionOptionRepository
    {
        void CreateAnsweredSurveyQuestionOption(AnsweredSurveyQuestionOption answeredsurveyquestionoption);
        void UpdateAnsweredSurveyQuestionOption(AnsweredSurveyQuestionOption answeredsurveyquestionoption);
        AnsweredSurveyQuestionOption GetAnsweredSurveyQuestionOption(int id);
        IEnumerable<AnsweredSurveyQuestionOption> GetBySurveyQuestionOptionId(int id);
        int SaveChanges();
    }
}