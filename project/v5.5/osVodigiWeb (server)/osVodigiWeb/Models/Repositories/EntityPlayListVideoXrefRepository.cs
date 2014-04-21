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
    public class EntityPlayListVideoXrefRepository : IPlayListVideoXrefRepository
    {
        private VodigiContext db = new VodigiContext();

        public IEnumerable<PlayListVideoXref> GetPlayListVideoXrefs(int playlistid)
        {
            // Build the query
            var query = from playlistvideoxref in db.PlayListVideoXrefs
                        select playlistvideoxref;

            // Apply the filters first
            query = query.Where(xrefs => xrefs.PlayListID.Equals(playlistid));

            // Apply the ordering
            query = query.OrderBy("PlayOrder", false);

            List<PlayListVideoXref> plvxs = query.ToList();

            return plvxs;
        }

        public void DeletePlayListVideoXrefs(int playlistid)
        {
            // Build the query
            var query = from playlistvideoxref in db.PlayListVideoXrefs
                        select playlistvideoxref;

            // Apply the filters first
            query = query.Where(xrefs => xrefs.PlayListID.Equals(playlistid));

            List<PlayListVideoXref> plvxs = query.ToList();

            foreach (PlayListVideoXref plvx in plvxs)
            {
                db.PlayListVideoXrefs.Remove(plvx);
            }

            db.SaveChanges();
        }

        public void CreatePlayListVideoXref(PlayListVideoXref xref)
        {
            db.PlayListVideoXrefs.Add(xref);
            db.SaveChanges();
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }
    }
}