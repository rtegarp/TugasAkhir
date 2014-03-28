﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace osVodigiWeb.Models
{
    public class EntitySystemMessageRepository : ISystemMessageRepository
    {
        private VodigiContext db = new VodigiContext();

        public SystemMessage GetSystemMessage(int id)
        {
            SystemMessage systemmessage = db.SystemMessages.Find(id);

            return systemmessage;
        }

        public IEnumerable<SystemMessage> GetAllSystemMessages()
        {
            var query = from systemmessage in db.SystemMessages
                        select systemmessage;
            query = query.OrderBy("SystemMessageTitle", false);

            List<SystemMessage> systemmessages = query.ToList();

            return systemmessages;
        }

        public IEnumerable<SystemMessage> GetSystemMessagesByDate(DateTime date)
        {
            var query = from systemmessage in db.SystemMessages
                        select systemmessage;
            query = query.Where(sms => sms.DisplayDateStart <= date);
            query = query.Where(sms => sms.DisplayDateEnd >= date);
            query = query.OrderBy("SystemMessageTitle", false);

            List<SystemMessage> systemmessages = query.ToList();

            return systemmessages;
        }

        public IEnumerable<SystemMessage> GetSystemMessagePage(string systemmessagetitle, string systemmessagebody, string sortby, bool isdescending, int pagenumber, int pagecount)
        {
            var query = from systemmessage in db.SystemMessages
                        select systemmessage;

            if (!String.IsNullOrEmpty(systemmessagetitle))
                query = query.Where(sms => sms.SystemMessageTitle.StartsWith(systemmessagetitle));
            if (!String.IsNullOrEmpty(systemmessagebody))
                query = query.Where(sms => sms.SystemMessageBody.StartsWith(systemmessagebody));

            if (!String.IsNullOrEmpty(sortby))
                query = query.OrderBy(sortby, isdescending);

            // Get a single page from the filtered records
            int iSkip = (pagenumber * Constants.PageSize) - Constants.PageSize;

            List<SystemMessage> systemmessages = query.Skip(iSkip).Take(Constants.PageSize).ToList();

            return systemmessages;
        }

        public int GetSystemMessageRecordCount(string systemmessagetitle, string systemmessagebody)
        {
            var query = from systemmessage in db.SystemMessages
                        select systemmessage;

            if (!String.IsNullOrEmpty(systemmessagetitle))
                query = query.Where(sms => sms.SystemMessageTitle.StartsWith(systemmessagetitle));
            if (!String.IsNullOrEmpty(systemmessagebody))
                query = query.Where(sms => sms.SystemMessageBody.StartsWith(systemmessagebody));

            return query.Count();
        }

        public void CreateSystemMessage(SystemMessage systemmessage)
        {
            db.SystemMessages.Add(systemmessage);
            db.SaveChanges();
        }

        public void UpdateSystemMessage(SystemMessage systemmessage)
        {
            db.Entry(systemmessage).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteSystemMessage(SystemMessage systemmessage)
        {
            db.SystemMessages.Remove(systemmessage);
            db.SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}