﻿using System.Collections.Generic;

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
    public interface IPlayerRepository
    {
        void CreatePlayer(Player player);
        void UpdatePlayer(Player player);
        Player GetPlayer(int playerid);
        IEnumerable<Player> GetAllPlayers(int accountid);
        IEnumerable<Player> GetPlayerByName(int accountid, string playername);
        IEnumerable<Player> GetPlayersByPlayerGroup(int playergroupid);
        IEnumerable<Player> GetPlayerPage(int accountid, int playergroupid, string playername, bool includeinactive, string sortby, bool isdescending, int pagenumber, int pagecount);
        int GetPlayerRecordCount(int accountid, int playergroupid, string playername, bool includeinactive);
        int SaveChanges();
    }
}