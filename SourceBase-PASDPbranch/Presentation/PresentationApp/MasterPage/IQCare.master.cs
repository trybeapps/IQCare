using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Interface.Security;
using Application.Presentation;
using Application.Common;
using Interface.Clinical;
using System.Configuration;

public partial class MasterPage_IQCare : System.Web.UI.MasterPage
{
    String strPathAndQuery;
    String strUrl;
    public int SessionLengthMinutes
    {
        get { return 60; }
    }
    public string SessionExpireDestinationUrl 
    {        
        get { return strUrl +"IQCare/frmLogin.aspx"; }
    }
    protected override void OnPreRender(EventArgs e)
    {
        
        base.OnPreRender(e);
        //string path = HttpContext.Current.Request.Url.AbsolutePath;
        strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
        strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
        this.pageHead.Controls.Add(new LiteralControl(
            String.Format("<meta http-equiv='refresh' content='{0};url={1}'>",
            SessionLengthMinutes * 60, SessionExpireDestinationUrl)));
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Page.Header.DataBind(); 
        if (Session["AppLocation"] == null)
        {
            IQCareMsgBox.Show("SessionExpired", this);
            Response.Redirect(strUrl +"/IQCare/frmLogOff.aspx");
        }
        
        
        lblTitle.Text = "International Quality Care Patient Management and Monitoring System [" + Session["AppLocation"].ToString() + "]";
        string url = Request.RawUrl.ToString();
        Application["PrvFrm"] = url;
        //string pageName = this.Page.ToString();
        System.IO.FileInfo fileinfo = new System.IO.FileInfo(Request.Url.AbsolutePath);
        string pageName = fileinfo.Name;

        if (Session["PatientID"] != null)
        {
            if (int.Parse(Session["PatientID"].ToString()) > 0)
            {
                if (pageName.Equals("frmFamilyInformation.aspx"))
                {
                    (levelTwoNavigationUserControl1.FindControl("PanelPatiInfo") as Panel).Visible = false;
                }
                
                if (pageName.Equals("frmPatient_Home.aspx"))
                {
                    facilityBanner.Style.Add("display", "none");
                    patientBanner.Style.Add("display", "inline");
                    username1.Attributes["class"] = "usernameLevel1"; //Style.Add("display", "inline");
                    currentdate1.Attributes["class"] = "currentdateLevel1"; //Style.Add("display", "inline");
                    facilityName.Attributes["class"] = "facilityLevel1"; //Style.Add("display", "inline");
                    //userNameLevel2.Style.Add("display", "none");
                    //currentDateLevel2.Style.Add("display", "none");
                    imageFlipLevel2.Style.Add("display", "none");
                    //facilityLevel2.Style.Add("display", "none");
                    level2Navigation.Style.Add("display", "inline");
                }
                else if (pageName.Equals("frmAddTechnicalArea.aspx"))
                {
                    facilityBanner.Style.Add("display", "inline");
                    patientBanner.Style.Add("display", "none");
                    username1.Attributes["class"] = "usernameLevel1"; //Style.Add("display", "inline");
                    currentdate1.Attributes["class"] = "currentdateLevel1"; //Style.Add("display", "inline");
                    facilityName.Attributes["class"] = "facilityLevel1"; //Style.Add("display", "inline");
                    //userNameLevel2.Style.Add("display", "none");
                    //currentDateLevel2.Style.Add("display", "none");
                    imageFlipLevel2.Style.Add("display", "none");
                    //facilityLevel2.Style.Add("display", "none");
                    level2Navigation.Style.Add("display", "none");
                }
                else if (pageName.Equals("frmAdmin_DeletePatient.aspx"))
                {
                    facilityBanner.Style.Add("display", "inline");
                    patientBanner.Style.Add("display", "none");
                    username1.Attributes["class"] = "usernameLevel1"; //Style.Add("display", "inline");
                    currentdate1.Attributes["class"] = "currentdateLevel1"; //Style.Add("display", "inline");
                    facilityName.Attributes["class"] = "facilityLevel1"; //Style.Add("display", "inline");
                    //userNameLevel2.Style.Add("display", "none");
                    //currentDateLevel2.Style.Add("display", "none");
                    imageFlipLevel2.Style.Add("display", "none");
                    //facilityLevel2.Style.Add("display", "none");
                    level2Navigation.Style.Add("display", "none");
                }
                else if (pageName.Equals("frmPatientCustomRegistration.aspx"))
                {
                    facilityBanner.Style.Add("display", "none");
                    patientBanner.Style.Add("display", "inline");
                    username1.Attributes["class"] = "usernameLevel1"; //Style.Add("display", "inline");
                    currentdate1.Attributes["class"] = "currentdateLevel1"; //Style.Add("display", "inline");
                    facilityName.Attributes["class"] = "facilityLevel1"; //Style.Add("display", "inline");
                    //userNameLevel2.Style.Add("display", "none");
                    //currentDateLevel2.Style.Add("display", "none");
                    imageFlipLevel2.Style.Add("display", "none");
                    //facilityLevel2.Style.Add("display", "none");
                    level2Navigation.Style.Add("display", "none");
                }
                else
                {
                    facilityBanner.Style.Add("display", "none");
                    patientBanner.Style.Add("display", "none");
                    //usernameLevel1.Style.Add("display", "none");
                    //currentdateLevel1.Style.Add("display", "none");
                    //facilityLevel1.Style.Add("display", "none");
                    username1.Attributes["class"] = "userNameLevel2"; //userNameLevel2.Style.Add("display", "inline");
                    currentdate1.Attributes["class"] = "currentDateLevel2"; //currentDateLevel2.Style.Add("display", "inline");
                    imageFlipLevel2.Style.Add("display", "inline");
                    facilityName.Attributes["class"] = "facilityLevel2"; //facilityLevel2.Style.Add("display", "inline");
                    level2Navigation.Style.Add("display", "inline");
                    level2Navigation.Attributes["class"] = "";

                }
            }
            else
            {
                facilityBanner.Style.Add("display", "inline");
                patientBanner.Style.Add("display", "none");
                username1.Attributes["class"] = "usernameLevel1"; //Style.Add("display", "inline");
                currentdate1.Attributes["class"] = "currentdateLevel1"; //Style.Add("display", "inline");
                facilityName.Attributes["class"] = "facilityLevel1"; //Style.Add("display", "inline");
                //userNameLevel2.Style.Add("display", "none");
                //currentDateLevel2.Style.Add("display", "none");
                imageFlipLevel2.Style.Add("display", "none");
                //facilityLevel2.Style.Add("display", "none");
                level2Navigation.Style.Add("display", "none");
            }
        }
        else
        {
            facilityBanner.Style.Add("display", "inline");
            patientBanner.Style.Add("display", "none");
            username1.Attributes["class"] = "usernameLevel1"; //Style.Add("display", "inline");
            currentdate1.Attributes["class"] = "currentdateLevel1"; //Style.Add("display", "inline");
            facilityName.Attributes["class"] = "facilityLevel1"; //Style.Add("display", "inline");
            //userNameLevel2.Style.Add("display", "none");
            //currentDateLevel2.Style.Add("display", "none");
            imageFlipLevel2.Style.Add("display", "none");
            //facilityLevel2.Style.Add("display", "none");
            level2Navigation.Style.Add("display", "none");
        }

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
            Response.Redirect("frmLogOff.aspx");
        }
        if (Session["AppUserID"].ToString() == "")
        {
            IQCareMsgBox.Show("SessionExpired", this);
            Response.Redirect("../frmLogOff.aspx");
        }

        lblversion.Text = AuthenticationManager.AppVersion;
        lblrelDate.Text = AuthenticationManager.ReleaseDate;

 
        
    }

    //public void childMenu1_MenuItemDataBound(object sender, MenuEventArgs e)
    //{
    //    e.Item.Text = "<div style='width:100px; color:Yellow; background-color:Orange' >" + e.Item.Text + "</div>";

    //}
   
    protected void lnkPassword_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/AdminForms/frmAdmin_ChangePassword.aspx");
    }
    protected void lnkLogOut_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmLogOff.aspx");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        Menu childMenu1 = (Menu)Page.Master.FindControl("levelOneNavigationUserControl1").FindControl("mainMenu");
        string a = childMenu1.ID;

        foreach (MenuItem mnr in childMenu1.Items)
        {
            for(int i=0;i <mnr.ChildItems.Count ; i++)
            {
            //    mnr.ChildItems[i].Text = "<div style='width:175px;'>" + "&nbsp&nbsp " + mnr.ChildItems[i].Text + "</div>";             
            }            
        }
       // childMenu1.MenuItemDataBound += new MenuEventHandler(childMenu1_MenuItemDataBound);
    }

    protected void lnkReportDefect_Click(object sender, EventArgs e)
    {
        string path = ConfigurationSettings.AppSettings["reportdefecturl"].ToString();
        System.Web.UI.ScriptManager.RegisterStartupScript(Page, typeof(Page),"", "" + "pageurl('" + path + "');", true);
    }
}
