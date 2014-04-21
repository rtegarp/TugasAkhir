
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
    public class ScreenContentView
    {
        public int ScreenContentID { get; set; }
        public int AccountID { get; set; }
        public int ScreenContentTypeID { get; set; }
        public string ScreenContentTypeName { get; set; }
        public string ScreenContentName { get; set; }
        public string ScreenContentTitle { get; set; }
        public int ThumbnailImageID { get; set; }
        public string StoredFilename { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public bool IsActive { get; set; }
    }
}