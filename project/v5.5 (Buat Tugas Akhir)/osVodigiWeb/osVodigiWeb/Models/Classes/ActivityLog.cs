﻿using System;

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
    public class ActivityLog
    {
        public int ActivityLogID { get; set; }
        public int AccountID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string EntityType { get; set; }
        public string EntityAction { get; set; }
        public DateTime ActivityDateTime { get; set; }
        public string ActivityDetails { get; set; }
    }
}