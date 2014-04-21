using System;

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
    public class PlayerScreenContentLog
    {
        public int PlayerScreenContentLogID { get; set; }
        public int AccountID { get; set; }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public int ScreenID { get; set; }
        public string ScreenName { get; set; }
        public int ScreenContentID { get; set; }
        public string ScreenContentName { get; set; }
        public int ScreenContentTypeID { get; set; }
        public string ScreenContentTypeName { get; set; }
        public DateTime DisplayDateTime { get; set; }
        public DateTime CloseDateTime { get; set; }
        public string ContentDetails { get; set; }
    }
}