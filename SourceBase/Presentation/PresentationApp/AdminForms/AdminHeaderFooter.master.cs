using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Interface.Security;
using Application.Presentation;
using Application.Common;
using Interface.Clinical;


public partial class AdminHeaderFooter : System.Web.UI.MasterPage
{
    private void InitMenu()
    {
        string theURL;
        if(Session["SystemId"].ToString() == "1")
        {
            theURL = string.Format("{0}", "../AdminForms/frmAdmin_PMTCT_CustomItems.aspx");
            mnuAdminCustom.HRef = theURL;
        }
        else
        {
            theURL = string.Format("{0}", "../AdminForms/frmAdmin_PMTCT_CustomItems.aspx");
            mnuAdminCustom.HRef = theURL;
        }
        

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        InitMenu();

        lblTitle.InnerText = "International Quality Care Patient Management and Monitoring System [" + Session["AppLocation"].ToString() + "]";
        string url = Request.RawUrl.ToString();
        Application["PrvFrm"] = url;
        if (Session["AppUserName"] != null)
        {
            lblUserName.Text = Session["AppUserName"].ToString();
        }
        if (Session["AppLocation"] != null)
        {
            lblLocation.Text = Session["AppLocation"].ToString();
        }
        IIQCareSystem AdminManager;
        AdminManager = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");

        if (Session["AppDateFormat"] != "")
        {
            lblDate.Text = AdminManager.SystemDate().ToString(Session["AppDateFormat"].ToString());
        }
        else
        {
            lblDate.Text = AdminManager.SystemDate().ToString("dd-MMM-yyyy");
        }
        if (Session.Count == 0)
        {
            IQCareMsgBox.Show("SessionExpired", this);
            Response.Redirect("~/frmlogin.aspx",true);
        }
        if (Session["AppUserID"].ToString() == "")
        {
            IQCareMsgBox.Show("SessionExpired", this);
            Response.Redirect("~/frmlogin.aspx",true);
        }

        //####### Delete Patient #############
        string theUrl;
        theUrl = string.Format("{0}?mnuClicked={1}", "../frmFindAddPatient.aspx", "DeletePatient");
        mnuAdminDeletePatient.HRef = theUrl;

        lblversion.Text = AuthenticationManager.AppVersion;
        lblrelDate.Text = AuthenticationManager.ReleaseDate;
        if (Session["UserRight"] != "")
            AuthenticationRights();
    }

    //RTyagi..19Feb 07..
    private void AuthenticationRights()
    {
        DataView theDV = new DataView((DataTable)Session["UserRight"]);
        DataTable theDTModule = (DataTable)Session["AppModule"];
        string ModuleId = "";
        foreach (DataRow theDR in theDTModule.Rows)
        {
            if (ModuleId == "")
                ModuleId = theDR["ModuleId"].ToString();
            else
                ModuleId = ModuleId + "," + theDR["ModuleId"].ToString();
        }       
        theDV.RowFilter = "ModuleId in (0," + ModuleId + ")";
        DataTable theDT = new DataTable();
        theDT = theDV.ToTable();

        AuthenticationManager Authentication = new AuthenticationManager();
    
        /******** Admin menus *********/
        if (Authentication.HasFeatureRight(ApplicationAccess.UserAdministration,   theDT) == false)
        {
            mnuAdminUser.Visible = false;
        }
        if (Authentication.HasFeatureRight(ApplicationAccess.UserGroupAdministration,   theDT) == false)
        {
            mnuAdminUserGroup.Visible = false;
        }
        if (Authentication.HasFeatureRight(ApplicationAccess.DeletePatient,   theDT) == false)
        {
            mnuAdminDeletePatient.Visible = false;
        }
        //if (Authentication.HasFeatureRight(ApplicationAccess.CustomiseDropDown, (DataTable)Session["UserRight"]) == false)
        //{
        //    mnuAdminCustom.Visible = false;
        //}
        if (Authentication.HasFeatureRight(ApplicationAccess.FacilitySetup,   theDT) == false)
        {
            mnuAdminFacility.Visible = false;
        }
        if (Authentication.HasFeatureRight(ApplicationAccess.DonorReports,   theDT) == false)
        {
            mnuAdminDonorReport.Visible = false;
        }
        if (Authentication.HasFeatureRight(ApplicationAccess.CustomReports,   theDT) == false)
        {
            mnuAdminCustomReport.Visible = false;
        }
        if (Authentication.HasFeatureRight(ApplicationAccess.FacilityReports,   theDT) == false)
        {
            mnuAdminFacilityReport.Visible = false;
        }
        if (Authentication.HasFeatureRight(ApplicationAccess.ConfigureCustomFields,   theDT) == false)
        {
            mnuAdminCustomConfig.Visible = false;
        }
        if (Authentication.HasFeatureRight(ApplicationAccess.Schedular,   theDT) == false)
        {
            mnuAdminScheduler.Visible = false;
        }
    }
    
}
