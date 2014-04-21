using System;
using System.Collections.Generic;

namespace osVodigiWeb.Models
{
    public interface ISystemMessageRepository
    {
        void CreateSystemMessage(SystemMessage systemmessage);
        void UpdateSystemMessage(SystemMessage systemmessage);
        void DeleteSystemMessage(SystemMessage systemmessage);
        SystemMessage GetSystemMessage(int systemmessageid);
        IEnumerable<SystemMessage> GetAllSystemMessages();
        IEnumerable<SystemMessage> GetSystemMessagesByDate(DateTime date);
        IEnumerable<SystemMessage> GetSystemMessagePage(string systemmessagetitle, string systemmessagebody, string sortby, bool isdescending, int pagenumber, int pagecount);
        int GetSystemMessageRecordCount(string systemmessagetitle, string systemmessagebody);
        int SaveChanges();
    }
}