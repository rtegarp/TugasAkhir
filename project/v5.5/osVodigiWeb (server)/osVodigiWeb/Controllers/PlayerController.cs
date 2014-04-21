using System;
using System.Collections.Generic;
using System.Web.Mvc;
using osVodigiWeb.Models;

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

namespace osVodigiWeb.Controllers
{ 
    public class PlayerController : Controller
    {
        IPlayerRepository repository;

        public PlayerController() : this(new EntityPlayerRepository()) 
        { }

        public PlayerController(IPlayerRepository paramrepository) 
        {
            repository = paramrepository;
        }

        //
        // GET: /Player/

        public ActionResult Index()
        {
            try
            {
                if (Session["UserAccountID"] == null)
                    return RedirectToAction("Validate", "Login");
                User user = (User)Session["User"];
                ViewData["LoginInfo"] = Utility.BuildUserAccountString(user.Username, Convert.ToString(Session["UserAccountName"]));
                if (user.IsAdmin)
                    ViewData["txtIsAdmin"] = "true";
                else
                    ViewData["txtIsAdmin"] = "false";

                // Initialize or get the page state using session
                PlayerPageState pagestate = GetPageState();

                // Get the account id
                int accountid = 0;
                if (Session["UserAccountID"] != null)
                    accountid = Convert.ToInt32(Session["UserAccountID"]);

                // Set and save the page state to the submitted form values if any values are passed
                if (Request.Form["lstAscDesc"] != null)
                {
                    pagestate.AccountID = accountid;
                    pagestate.PlayerGroupID = Convert.ToInt32(Request.Form["lstPlayerGroup"].ToString().Trim());
                    pagestate.PlayerName = Request.Form["txtPlayerName"].ToString().Trim();
                    if (Request.Form["chkIncludeInactive"].ToLower().StartsWith("true"))
                        pagestate.IncludeInactive = true;
                    else
                        pagestate.IncludeInactive = false;
                    pagestate.SortBy = Request.Form["lstSortBy"].ToString().Trim();
                    pagestate.AscDesc = Request.Form["lstAscDesc"].ToString().Trim();
                    pagestate.PageNumber = Convert.ToInt32(Request.Form["txtPageNumber"].ToString().Trim());
                    SavePageState(pagestate);
                }

                // Add the session values to the view data so they can be populated in the form
                ViewData["AccountID"] = pagestate.AccountID;
                ViewData["PlayerGroupID"] = pagestate.PlayerGroupID;
                ViewData["PlayerName"] = pagestate.PlayerName;
                ViewData["IncludeInactive"] = pagestate.IncludeInactive;
                ViewData["SortBy"] = pagestate.SortBy;
                ViewData["PlayerGroupList"] = new SelectList(BuildPlayerGroupList(true), "Value", "Text", pagestate.PlayerGroupID);
                ViewData["SortByList"] = new SelectList(BuildSortByList(), "Value", "Text", pagestate.SortBy);
                ViewData["AscDescList"] = new SelectList(BuildAscDescList(), "Value", "Text", pagestate.AscDesc);

                // Determine asc/desc
                bool isdescending = false;
                if (pagestate.AscDesc.ToLower().StartsWith("d"))
                    isdescending = true;

                // Get a Count of all filtered records
                int recordcount = repository.GetPlayerRecordCount(pagestate.AccountID, pagestate.PlayerGroupID, pagestate.PlayerName, pagestate.IncludeInactive);

                // Determine the page count
                int pagecount = 1;
                if (recordcount > 0)
                {
                    pagecount = recordcount / Constants.PageSize;
                    if (recordcount % Constants.PageSize != 0) // Add a page if there are more records
                    {
                        pagecount = pagecount + 1;
                    }
                }

                // Make sure the current page is not greater than the page count
                if (pagestate.PageNumber > pagecount)
                {
                    pagestate.PageNumber = pagecount;
                    SavePageState(pagestate);
                }

                // Set the page number and account in viewdata
                ViewData["PageNumber"] = Convert.ToString(pagestate.PageNumber);
                ViewData["PageCount"] = Convert.ToString(pagecount);
                ViewData["RecordCount"] = Convert.ToString(recordcount);

                // We need to add the player group name
                IEnumerable<Player> players = repository.GetPlayerPage(pagestate.AccountID, pagestate.PlayerGroupID, pagestate.PlayerName, pagestate.IncludeInactive, pagestate.SortBy, isdescending, pagestate.PageNumber, pagecount);
                List<PlayerView> playerviews = new List<PlayerView>();
                IPlayerGroupRepository pgrep = new EntityPlayerGroupRepository();
                foreach (Player player in players)
                {
                    PlayerView playerview = new PlayerView();
                    playerview.PlayerID = player.PlayerID;
                    playerview.AccountID = player.AccountID;
                    playerview.PlayerGroupID = player.PlayerGroupID;
                    playerview.PlayerName = player.PlayerName;
                    playerview.PlayerLocation = player.PlayerLocation;
                    playerview.PlayerDescription = player.PlayerDescription;
                    playerview.IsActive = player.IsActive;
                    PlayerGroup pg = pgrep.GetPlayerGroup(player.PlayerGroupID);
                    playerview.PlayerGroupName = pg.PlayerGroupName;

                    playerviews.Add(playerview);
                }

                ViewResult result = View(playerviews);
                result.ViewName = "Index";
                return result;
            }
            catch (Exception ex)
            {
                Helpers.SetupApplicationError("Player", "Index", ex.Message);
                return RedirectToAction("Index", "ApplicationError");
            }
        }

        //
        // GET: /Player/Create

        public ActionResult Create()
        {
            try
            {
                if (Session["UserAccountID"] == null)
                    return RedirectToAction("Validate", "Login");
                User user = (User)Session["User"];
                ViewData["LoginInfo"] = Utility.BuildUserAccountString(user.Username, Convert.ToString(Session["UserAccountName"]));
                if (user.IsAdmin)
                    ViewData["txtIsAdmin"] = "true";
                else
                    ViewData["txtIsAdmin"] = "false";

                ViewData["PlayerGroupList"] = new SelectList(BuildPlayerGroupList(false), "Value", "Text");
                ViewData["ValidationMessage"] = String.Empty;

                return View(CreateNewPlayer());
            }
            catch (Exception ex)
            {
                Helpers.SetupApplicationError("Player", "Create", ex.Message);
                return RedirectToAction("Index", "ApplicationError");
            }
        } 

        //
        // POST: /Player/Create

        [HttpPost]
        public ActionResult Create(Player player)
        {
            try
            {
                if (Session["UserAccountID"] == null)
                    return RedirectToAction("Validate", "Login");
                User user = (User)Session["User"];
                ViewData["LoginInfo"] = Utility.BuildUserAccountString(user.Username, Convert.ToString(Session["UserAccountName"]));
                if (user.IsAdmin)
                    ViewData["txtIsAdmin"] = "true";
                else
                    ViewData["txtIsAdmin"] = "false";

                if (ModelState.IsValid)
                {
                    // Set NULLs to Empty Strings
                    player = FillNulls(player);
                    player.AccountID = Convert.ToInt32(Session["UserAccountID"]);
                    player.PlayerGroupID = Convert.ToInt32(Request.Form["lstPlayerGroup"]);

                    string validation = ValidateInput(player);
                    if (!String.IsNullOrEmpty(validation))
                    {
                        ViewData["ValidationMessage"] = validation;
                        ViewData["PlayerGroupList"] = new SelectList(BuildPlayerGroupList(false), "Value", "Text", player.PlayerGroupID);
                        return View(player);
                    }

                    repository.CreatePlayer(player);

                    CommonMethods.CreateActivityLog((User)Session["User"], "Player", "Add",
                            "Added player '" + player.PlayerName + "' - ID: " + player.PlayerID.ToString());

                    return RedirectToAction("Index");
                }

                return View(player);
            }
            catch (Exception ex)
            {
                Helpers.SetupApplicationError("Player", "Create POST", ex.Message);
                return RedirectToAction("Index", "ApplicationError");
            }
        }
        
        //
        // GET: /Player/Edit/5
 
        public ActionResult Edit(int id)
        {
            try
            {
                if (Session["UserAccountID"] == null)
                    return RedirectToAction("Validate", "Login");
                User user = (User)Session["User"];
                ViewData["LoginInfo"] = Utility.BuildUserAccountString(user.Username, Convert.ToString(Session["UserAccountName"]));
                if (user.IsAdmin)
                    ViewData["txtIsAdmin"] = "true";
                else
                    ViewData["txtIsAdmin"] = "false";

                Player player = repository.GetPlayer(id);

                ViewData["PlayerGroupList"] = new SelectList(BuildPlayerGroupList(false), "Value", "Text", player.PlayerGroupID);
                ViewData["ValidationMessage"] = String.Empty;

                return View(player);
            }
            catch (Exception ex)
            {
                Helpers.SetupApplicationError("Player", "Edit", ex.Message);
                return RedirectToAction("Index", "ApplicationError");
            }
        }

        //
        // POST: /Player/Edit/5

        [HttpPost]
        public ActionResult Edit(Player player)
        {
            try
            {
                if (Session["UserAccountID"] == null)
                    return RedirectToAction("Validate", "Login");
                User user = (User)Session["User"];
                ViewData["LoginInfo"] = Utility.BuildUserAccountString(user.Username, Convert.ToString(Session["UserAccountName"]));
                if (user.IsAdmin)
                    ViewData["txtIsAdmin"] = "true";
                else
                    ViewData["txtIsAdmin"] = "false";

                if (ModelState.IsValid)
                {
                    // Set NULLs to Empty Strings
                    player = FillNulls(player);

                    player.PlayerGroupID = Convert.ToInt32(Request.Form["lstPlayerGroup"]);

                    string validation = ValidateInput(player);
                    if (!String.IsNullOrEmpty(validation))
                    {
                        ViewData["PlayerGroupList"] = new SelectList(BuildPlayerGroupList(false), "Value", "Text", player.PlayerGroupID);
                        ViewData["ValidationMessage"] = validation;
                        return View(player);
                    }

                    repository.UpdatePlayer(player);

                    CommonMethods.CreateActivityLog((User)Session["User"], "Player", "Edit",
                            "Edited player '" + player.PlayerName + "' - ID: " + player.PlayerID.ToString());

                    return RedirectToAction("Index");
                }

                return View(player);
            }
            catch (Exception ex)
            {
                Helpers.SetupApplicationError("Player", "Edit POST", ex.Message);
                return RedirectToAction("Index", "ApplicationError");
            }
        }

        //
        // Support Methods

        private List<SelectListItem> BuildSortByList()
        {
            // Build the sort by list
            List<SelectListItem> sortitems = new List<SelectListItem>();

            SelectListItem sortitem1 = new SelectListItem();
            sortitem1.Text = "Player Name";
            sortitem1.Value = "PlayerName";

            SelectListItem sortitem3 = new SelectListItem();
            sortitem3.Text = "Location";
            sortitem3.Value = "PlayerLocation";

            SelectListItem sortitem4 = new SelectListItem();
            sortitem4.Text = "Description";
            sortitem4.Value = "PlayerDescription";

            SelectListItem sortitem5 = new SelectListItem();
            sortitem5.Text = "Is Active";
            sortitem5.Value = "IsActive";

            sortitems.Add(sortitem1);
            sortitems.Add(sortitem3);
            sortitems.Add(sortitem4);
            sortitems.Add(sortitem5);

            return sortitems;
        }

        private List<SelectListItem> BuildAscDescList()
        {
            // Build the asc desc list
            List<SelectListItem> ascdescitems = new List<SelectListItem>();

            SelectListItem ascdescitem1 = new SelectListItem();
            ascdescitem1.Text = "Asc";
            ascdescitem1.Value = "Asc";

            SelectListItem ascdescitem2 = new SelectListItem();
            ascdescitem2.Text = "Desc";
            ascdescitem2.Value = "Desc";

            ascdescitems.Add(ascdescitem1);
            ascdescitems.Add(ascdescitem2);

            return ascdescitems;
        }

        private List<SelectListItem> BuildPlayerGroupList(bool addAllItem)
        {
            // Build the player group list
            List<SelectListItem> pgitems = new List<SelectListItem>();

            // Add an 'All' item at the top
            if (addAllItem)
            {
                SelectListItem all = new SelectListItem();
                all.Text = "All Player Groups";
                all.Value = "0";
                pgitems.Add(all);
            }

            IPlayerGroupRepository pgrep = new EntityPlayerGroupRepository();
            IEnumerable<PlayerGroup> pgs = pgrep.GetAllPlayerGroups(Convert.ToInt32(Session["UserAccountID"]));
            foreach (PlayerGroup pg in pgs)
            {
                SelectListItem item = new SelectListItem();
                item.Text = pg.PlayerGroupName;
                item.Value = pg.PlayerGroupID.ToString();

                pgitems.Add(item);
            }

            return pgitems;
        }

        private PlayerPageState GetPageState()
        {
            try
            {
                PlayerPageState pagestate = new PlayerPageState();


                // Initialize the session values if they don't exist - need to do this the first time controller is hit
                if (Session["PlayerPageState"] == null)
                {
                    int accountid = 0;
                    if (Session["UserAccountID"] != null)
                        accountid = Convert.ToInt32(Session["UserAccountID"]);

                    pagestate.AccountID = accountid;
                    pagestate.PlayerGroupID = 0;
                    pagestate.PlayerName = String.Empty;
                    pagestate.IncludeInactive = false;
                    pagestate.SortBy = "PlayerName";
                    pagestate.AscDesc = "Ascending";
                    pagestate.PageNumber = 1;
                    Session["PlayerPageState"] = pagestate;
                }
                else
                {
                    pagestate = (PlayerPageState)Session["PlayerPageState"];
                }
                return pagestate;
            }
            catch { return new PlayerPageState(); }
        }

        private void SavePageState(PlayerPageState pagestate)
        {
            Session["PlayerPageState"] = pagestate;
        }

        private Player FillNulls(Player player)
        {
            if (player.PlayerLocation == null) player.PlayerLocation = String.Empty;
            if (player.PlayerDescription == null) player.PlayerDescription = String.Empty;

            return player;
        }

        private Player CreateNewPlayer()
        {
            Player player = new Player();
            player.PlayerID = 0;
            player.AccountID = 0;
            player.PlayerGroupID = 0;
            player.PlayerName = String.Empty;
            player.PlayerLocation = String.Empty;
            player.PlayerDescription = String.Empty;
            player.IsActive = true;

            return player;
        }

        private string ValidateInput(Player player)
        {
            if (player.AccountID == 0)
                return "Account ID is not valid.";

            if (player.PlayerGroupID == 0)
                return "Player Group is required.";

            if (String.IsNullOrEmpty(player.PlayerName))
                return "Player Name is required.";

            return String.Empty;
        }

    }
}