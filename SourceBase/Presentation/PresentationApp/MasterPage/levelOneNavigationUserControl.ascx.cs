using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Application.Presentation;
using Interface.Security;
using Application.Common;

public partial class MasterPage_levelOneNavigationUserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["SystemId"] = null;
        //if (Session["SystemId"] == null)
        //    Response.Redirect("~/frmlogoff.aspx");

        if (Session["SystemId"] !=null && Convert.ToString(Session["SystemId"]) == "3")
        {
            AuthenticatePASDP();
        }
        
        AuthenticateRights();
    }

    #region "Hide menu item by value"
    public void RemoveMenuItemByValue(MenuItemCollection items, String value)
    {
        List<MenuItem> rmvMenuItem = new List<MenuItem>();

        //Breadth first, look in the collection
        foreach (MenuItem item in items)
        {
            if (item.Value == value)
            {
                rmvMenuItem.Add(item);
            }
        }

        if (rmvMenuItem.ToArray().Length != 0)
        {
            for (int j = 0; j < rmvMenuItem.ToArray().Length; j++)
            {
                items.Remove(rmvMenuItem[j]);
            }
        }

        //Search children
        foreach (MenuItem item in items)
        {
            RemoveMenuItemByValue(item.ChildItems, value);
        }
    }
    #endregion

    #region "Assign URL by value"
    public void AssignUrl(MenuItemCollection items, String value, String url)
    {
        foreach (MenuItem item in items)
        {
            if (item.Value == value)
            {
                item.NavigateUrl = url;
            }
        }

        foreach (MenuItem item in items)
        {
            AssignUrl(item.ChildItems, value, url);
        }
    }
    #endregion
    #region "Hiding Links for PASDP"

    private void AuthenticatePASDP()
    {
        foreach (MenuItem item in mainMenu.Items)
        {
            if (item.Value == "Find/Add Patient")
            {
                item.Enabled = false;
            }
            else if (item.Value == "Reports")
            {
                item.Enabled = false;
            }
            else if (item.Value == "Scheduler")
            {
                item.Enabled = false;
            }
            else if (item.Value == "Facility Home")
            {
                item.Enabled = false;
            }

            else if (item.Value == "Administration")
            {
                foreach (MenuItem items in item.ChildItems)
                {
                    if (items.Value == "Delete Patient")
                    {
                        items.Enabled = false;
                    }
                    else if (items.Value == "Configure Custom Fields")
                    {
                        items.Enabled = false;
                    }
                    else if (items.Value == "Export")
                    {
                        items.Enabled = false;
                    }
                }
            }
        }
    }


    #endregion
    #region "User Functions ReportHeader footer master"
    private void AuthenticateRights()
    {
        
         
        AuthenticationManager Authentication = new AuthenticationManager();
        if (Session["UserRight"].ToString() != "")
        {
            if (Authentication.HasFeatureRight(ApplicationAccess.FacilitySetup, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminFacility.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "Facility Setup");
            }
            //if (Authentication.HasFeatureRight(ApplicationAccess.CustomiseDropDown, (DataTable)Session["UserRight"]) == false)
            //{
            //    mnuAdminCustom.Visible = false;
            //}
            if (Authentication.HasFeatureRight(ApplicationAccess.UserAdministration, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminUser.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "User Administration");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.UserGroupAdministration, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminUserGroup.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "User Group Administration");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.DeletePatient, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminDeletePatient.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "Delete Patient");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.CustomReports, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminCustomReport.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "Custom Reports");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.FacilityReports, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminFacilityReport.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "Facility Reports");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.DonorReports, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminDonorReport.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "Donor Reports");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.QueryBuilderReports, (DataTable)Session["UserRight"]) == false)
            {
                RemoveMenuItemByValue(mainMenu.Items, "Query Builder Reports");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.Schedular, (DataTable)Session["UserRight"]) == false)
            {
                //mnuSchedular.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "Scheduler");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.ConfigureCustomFields, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminCustomConfig.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "Configure Custom Fields");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.IQToolsReports, (DataTable)Session["UserRight"]) == false)
            {
                //mnuAdminCustomConfig.Visible = false;
                RemoveMenuItemByValue(mainMenu.Items, "IQToolsReportsmain");
            }

            if (Authentication.HasFeatureRight(ApplicationAccess.AuditTrail, (DataTable)Session["UserRight"]) == false)
            {
                RemoveMenuItemByValue(mainMenu.Items, "AuditTrail");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.Export, (DataTable)Session["UserRight"]) == false)
            {
                RemoveMenuItemByValue(mainMenu.Items, "Export");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.Backupsetup, (DataTable)Session["UserRight"]) == false)
            {
                RemoveMenuItemByValue(mainMenu.Items, "Backup/Restore setup");
            }
            if (Authentication.HasFeatureRight(ApplicationAccess.Backuprestore, (DataTable)Session["UserRight"]) == false)
            {
                RemoveMenuItemByValue(mainMenu.Items, "Backup/Restore Database");
            }


        }
    }
    #endregion
}