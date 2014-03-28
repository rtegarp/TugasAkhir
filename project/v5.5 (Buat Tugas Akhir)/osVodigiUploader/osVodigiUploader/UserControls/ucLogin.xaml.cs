using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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


namespace osVodigiUploader.UserControls
{
    public partial class ucLogin : UserControl
    {
        public ucLogin()
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(ucLogin_Loaded);
            }
            catch { }
        }

        void ucLogin_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUsername.Focus();
            }
            catch { }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate the login
                lblError.Text = String.Empty;

                if (String.IsNullOrEmpty(txtUsername.Text.Trim()) || String.IsNullOrEmpty(txtPassword.Password.Trim()))
                {
                    lblError.Text = "Please enter your username and password.";
                    return;
                }

                osVodigiService.osVodigiServiceSoapClient ws = new osVodigiService.osVodigiServiceSoapClient();
                osVodigiService.UserAccount useraccount = ws.User_Validate(txtUsername.Text.Trim(), txtPassword.Password.Trim());
                if (useraccount != null)
                {
                    Global.CurrentUserAccount = useraccount;
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    lblError.Text = "Invalid login. Please try again.";
                }
            }
            catch { lblError.Text = "System Error. Login failed."; }
        }
    }
}
