using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

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
    public class EntityMusicRepository : IMusicRepository
    {
        private VodigiContext db = new VodigiContext();

        public Music GetMusic(int id)
        {
            Music music = db.Musics.Find(id);

            return music;
        }

        public Music GetMusicByGuid(string guid)
        {
            // Build the query
            var query = from music in db.Musics
                        select music;

            // Apply the filters first
            query = query.Where(ms => ms.StoredFilename.Contains(guid));

            List<Music> musics = query.ToList();

            if (musics.Count > 0)
                return musics[0];
            else
                return null;
        }

        public IEnumerable<Music> GetActiveMusics(int accountid)
        {
            // Build the query
            var query = from music in db.Musics
                        select music;

            // Apply the filters first
            query = query.Where(ms => ms.AccountID.Equals(accountid));
            query = query.Where(ms => ms.IsActive == true);
            query = query.OrderBy("MusicName", false);

            List<Music> musics = query.ToList();

            return musics;
        }

        public IEnumerable<Music> GetAllMusics(int accountid)
        {
            var query = from music in db.Musics
                        select music;

            query = query.Where(ms => ms.AccountID.Equals(accountid));
            query = query.OrderBy("MusicName", false);

            List<Music> musics = query.ToList();

            return musics;
        }

        public IEnumerable<Music> GetMusicPage(int accountid, string musicname, string tag, bool includeinactive, string sortby, bool isdescending, int pagenumber, int pagecount)
        {
            // Build the query
            var query = from music in db.Musics
                        select music;

            // Apply the filters first
            query = query.Where(ms => ms.AccountID.Equals(accountid));
            if (!String.IsNullOrEmpty(musicname))
                query = query.Where(ms => ms.MusicName.StartsWith(musicname));
            if (!String.IsNullOrEmpty(tag))
                query = query.Where(ms => ms.Tags.Contains(tag));
            if (!includeinactive)
                query = query.Where(ms => ms.IsActive == true);

            // Apply the ordering
            if (!String.IsNullOrEmpty(sortby))
                query = query.OrderBy(sortby, isdescending);

            // Get a single page from the filtered records
            int iSkip = (pagenumber * Constants.PageSize) - Constants.PageSize;

            List<Music> musics = query.Skip(iSkip).Take(Constants.PageSize).ToList();

            return musics;
        }

        public int GetMusicRecordCount(int accountid, string musicname, string tag, bool includeinactive)
        {
            // Build the query
            var query = from music in db.Musics
                        select music;

            // Apply the filters first
            query = query.Where(ms => ms.AccountID.Equals(accountid));
            if (!String.IsNullOrEmpty(musicname))
                query = query.Where(ms => ms.MusicName.StartsWith(musicname));
            if (!String.IsNullOrEmpty(tag))
                query = query.Where(ms => ms.Tags.Contains(tag));
            if (!includeinactive)
                query = query.Where(ms => ms.IsActive == true);

            // Get a Count of all filtered records
            return query.Count();
        }

        public void CreateMusic(Music music)
        {
            db.Musics.Add(music);
            db.SaveChanges();
        }

        public void UpdateMusic(Music music)
        {
            db.Entry(music).State = EntityState.Modified;
            db.SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}