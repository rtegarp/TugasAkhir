using System.Collections.Generic;
using System.Linq;

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
    public class EntityPlayerGroupScheduleRepository : IPlayerGroupScheduleRepository
    {
        private VodigiContext db = new VodigiContext();

        public PlayerGroupSchedule GetPlayerGroupSchedule(int id)
        {
            PlayerGroupSchedule playergroupschedule = db.PlayerGroupSchedules.Find(id);

            return playergroupschedule;
        }

        public IEnumerable<PlayerGroupSchedule> GetPlayerGroupSchedulesByPlayerGroup(int playergroupid)
        {
            // Build the query
            var query = from playergroupschedule in db.PlayerGroupSchedules
                        where playergroupschedule.PlayerGroupID == playergroupid
                        orderby playergroupschedule.Day, playergroupschedule.Hour, playergroupschedule.Minute
                        select playergroupschedule;

            List<PlayerGroupSchedule> playergroupschedules = query.ToList();

            return query;
        }

        public void DeletePlayerGroupSchedule(int playergroupscheduleid)
        {
            // Build the query
            var query = from playergroupschedule in db.PlayerGroupSchedules
                        select playergroupschedule;

            // Apply the filters first
            query = query.Where(pgss => pgss.PlayerGroupScheduleID.Equals(playergroupscheduleid));

            List<PlayerGroupSchedule> schedules = query.ToList();

            foreach (PlayerGroupSchedule schedule in schedules)
            {
                db.PlayerGroupSchedules.Remove(schedule);
            }

            db.SaveChanges();
        }

        public void DeletePlayerGroupSchedulesByPlayerGroup(int playergroupid)
        {
            // Build the query
            var query = from playergroupschedule in db.PlayerGroupSchedules
                        select playergroupschedule;

            // Apply the filters first
            query = query.Where(pgss => pgss.PlayerGroupID.Equals(playergroupid));

            List<PlayerGroupSchedule> schedules = query.ToList();

            foreach (PlayerGroupSchedule schedule in schedules)
            {
                db.PlayerGroupSchedules.Remove(schedule);
            }

            db.SaveChanges();
        }

        public void DeletePlayerGroupSchedulesByDayTime(int playergroupid, int day, int hour, int minute)
        {
            // Build the query
            var query = from playergroupschedule in db.PlayerGroupSchedules
                        select playergroupschedule;

            // Apply the filters first
            query = query.Where(pgss => pgss.PlayerGroupID.Equals(playergroupid));
            query = query.Where(pgss => pgss.Day.Equals(day));
            query = query.Where(pgss => pgss.Hour.Equals(hour));
            query = query.Where(pgss => pgss.Minute.Equals(minute));

            List<PlayerGroupSchedule> schedules = query.ToList();

            foreach (PlayerGroupSchedule schedule in schedules)
            {
                db.PlayerGroupSchedules.Remove(schedule);
            }

            db.SaveChanges();
        }

        public void CreatePlayerGroupSchedule(PlayerGroupSchedule playergroupschedule)
        {
            db.PlayerGroupSchedules.Add(playergroupschedule);
            db.SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}