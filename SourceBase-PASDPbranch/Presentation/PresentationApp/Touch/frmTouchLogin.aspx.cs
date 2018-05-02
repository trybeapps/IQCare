#region Usings
//.Net Libs
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;

//IQCare Libs
using Interface.Clinical;
using Application.Common;
using Application.Presentation;
using Interface.Security;
using BusinessProcess.Administration;
#endregion

namespace Touch
{
    public partial class frmTouchLogin : System.Web.UI.Page
    {
        #region Page Vars
        protected bool isPattern { get; set; }
        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            AppTitle.InnerText = TouchGlobals.RootTitle;
            try
            {
                //if (Request.Cookies["ptrnLkd"] != null)
                //{
                //    HttpCookie newCookie = new HttpCookie("ptrnLkd");
                //    newCookie.Expires = DateTime.Now.Subtract(new TimeSpan(0, 20, 0));
                //    Response.Cookies.Add(newCookie);
                //}

                //Ajax.Utility.RegisterTypeForAjax(typeof(frmTouchLogin));
                if (Page.IsPostBack != true)
                {
                    //Thread theThread = new Thread(GenerateCache);
                    //theThread.Start();
                    /*
                     * Calling generate cache from common location
                     * Update By: Gaurav 
                     * Update Date: 8 July 2014
                     */
                    Thread theThread = new Thread(new ParameterizedThreadStart(IQCareUtils.GenerateCache));
                    theThread.Start(false);
                    Init_Form(false);
                }
                else
                {
                    if (Request.Cookies["ptrnLkd"] != null)    // user's session has timed out and is within the pattern lock time
                    {
                        BUser ptrnLockUser = (BUser)ObjectFactory.CreateInstance("BusinessProcess.Administration.BUser, BusinessProcess.Administration");
                        DataSet ptrnLockRecord = ptrnLockUser.GetUserLock(Convert.ToInt32(Request.Cookies["ptrnLkd"].Value.Split('_')[1]));

                        if (patLock.Value.Equals(ptrnLockRecord.Tables[0].Rows[0].Field<string>("ptrnLock_code")))
                        {
                            patternLogin(ptrnLockRecord);
                        }
                        else
                        {
                            contPtrnLock.Style["display"] = "block";
                            updtLogin.Visible = false;
                            isPattern = true;
                            lblCurUser.InnerText = ptrnLockRecord.Tables[0].Rows[0].Field<string>("UserFirstName") + " " + ptrnLockRecord.Tables[0].Rows[0].Field<string>("UserLastName");
                            IQCareMsgBox.Show("InvalidLogin", this);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }
        }
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            if (ValidateLogin() == false)
            {
                Init_Form(true);
                return;
            }

            IUser LoginManager;
            try
            {
                string ObjFactoryParameter = "BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical";
                IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);
                string sqlQuery1 = string.Empty;
                sqlQuery1 = string.Format("select SystemId from mst_facility where FacilityID=" + Convert.ToInt32(ddFacility.SelectedValue) + "");
                DataTable DT = ptnMgr.ReturnDatatableQuery(sqlQuery1);
                if (Convert.ToInt32(DT.Rows[0]["SystemId"]) != Convert.ToInt32(Session["SystemId"]))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AFail", "alert('Access Denied for this Facility-Contact Administrator')", true);
                }
                else
                {
                    LoginManager = (IUser)ObjectFactory.CreateInstance("BusinessProcess.Security.BUser, BusinessProcess.Security");
                    DataSet theDS = LoginManager.GetUserCredentials(txtUname.Text.Trim(), Convert.ToInt32(ddFacility.SelectedValue), Convert.ToInt32(Session["SystemId"]));
                    if (theDS.Tables.Count > 0)
                    {
                        int FacilityExist = 1;
                        if (theDS.Tables[5].Rows.Count > 0)
                        {
                            DataView theDV = new DataView();
                            FacilityExist = 0;
                            foreach (DataRow theDR in theDS.Tables[5].Rows)
                            {
                                if (Convert.ToInt32(theDR["GroupId"]) > 1)
                                {
                                    theDV = new DataView(theDS.Tables[1]);
                                    theDV.RowFilter = "FacilityID= " + ddFacility.SelectedValue + "";
                                    if (theDV.ToTable().Rows.Count > 0)
                                    {
                                        FacilityExist = 1;
                                    }
                                }
                                else if (Convert.ToInt32(theDR["GroupId"]) == 1)
                                {
                                    FacilityExist = 1;
                                }
                            }
                        }
                        if (FacilityExist == 0)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AFail", "alert('Access Denied for this Facility-Contact Administrator')", true);
                            //IQCareMsgBox.Show("AccessDenied", this);
                            return;
                        }
                        Utility theUtil = new Utility();
                        if (theDS.Tables[0].Rows.Count > 0)
                        {
                            if (theUtil.Decrypt(Convert.ToString(theDS.Tables[0].Rows[0]["Password"])) != txtPass.Text.Trim())
                            {
                                if ((Request.Browser.Cookies))
                                {
                                    HttpCookie theCookie = Request.Cookies[txtUname.Text];
                                    if (theCookie == null)
                                    {
                                        HttpCookie theNCookie = new HttpCookie(txtUname.Text);
                                        theNCookie.Value = txtUname.Text + ",1";
                                        DateTime theNewDTTime = Convert.ToDateTime(ViewState["theCurrentDate"]).AddMinutes(5);
                                        theNCookie.Expires = theNewDTTime;
                                        Response.Cookies.Add(theNCookie);
                                    }

                                    else
                                    {
                                        string[] theVal = (theCookie.Value.ToString()).Split(',');
                                        if (Convert.ToInt32(theVal[1]) >= 3 && theCookie.Name == txtUname.Text)
                                        {
                                            MsgBuilder theBuilder = new MsgBuilder();
                                            theBuilder.DataElements["MessageText"] = "User Account Locked. Try again after 5 Mins.";
                                            IQCareMsgBox.Show("#C1", theBuilder, this);
                                            return;
                                        }
                                        else
                                        {
                                            theVal[1] = (Convert.ToInt32(theVal[1]) + 1).ToString();
                                            theCookie.Value = txtUname.Text + "," + theVal[1];
                                            DateTime theAddNewDTTime = Convert.ToDateTime(ViewState["theCurrentDate"]).AddMinutes(5);
                                            theCookie.Expires = theAddNewDTTime;
                                            Response.Cookies.Add(theCookie);
                                        }
                                    }
                                }
                                IQCareMsgBox.Show("PasswordNotMatch", this);
                                Init_Form(true);
                                return;
                            }
                            else
                            {
                                HttpCookie theCookie = Request.Cookies[txtUname.Text];
                                if (theCookie != null)
                                {
                                    string[] theVal = (theCookie.Value.ToString()).Split(',');
                                    if (Convert.ToInt32(theVal[1]) >= 3)
                                    {
                                        MsgBuilder theBuilder = new MsgBuilder();
                                        theBuilder.DataElements["MessageText"] = "User Account Locked. Try again after 5 Mins.";
                                        IQCareMsgBox.Show("#C1", theBuilder, this);
                                        return;
                                    }
                                }

                            }
                        }

                        else
                        {
                            IQCareMsgBox.Show("InvalidLogin", this);
                            Init_Form(true);
                            return;
                        }

                        Session["AppUserId"] = Convert.ToString(theDS.Tables[0].Rows[0]["UserId"]);
                        Session["AppUserName"] = Convert.ToString(theDS.Tables[0].Rows[0]["UserFirstName"]) + " " + Convert.ToString(theDS.Tables[0].Rows[0]["UserLastName"]);
                        Session["EnrollFlag"] = theDS.Tables[1].Rows[0]["EnrollmentFlag"].ToString();
                        Session["CareEndFlag"] = theDS.Tables[1].Rows[0]["CareEndFlag"].ToString();
                        Session["IdentifierFlag"] = theDS.Tables[1].Rows[0]["IdentifierFlag"].ToString();
                        Session["UserRight"] = theDS.Tables[1];
                        DataTable theDT = theDS.Tables[2];
                        Session["AppLocationId"] = theDT.Rows[0]["FacilityID"].ToString();
                        Session["AppLocation"] = theDT.Rows[0]["FacilityName"].ToString();
                        Session["AppCountryId"] = theDT.Rows[0]["CountryID"].ToString();
                        Session["AppPosID"] = theDT.Rows[0]["PosID"].ToString();
                        Session["AppSatelliteId"] = theDT.Rows[0]["SatelliteID"].ToString();
                        Session["GracePeriod"] = theDT.Rows[0]["AppGracePeriod"].ToString();
                        Session["AppDateFormat"] = theDT.Rows[0]["DateFormat"].ToString();
                        Session["BackupDrive"] = theDT.Rows[0]["BackupDrive"].ToString();
                        Session["SystemId"] = theDT.Rows[0]["SystemId"].ToString();
                        Session["AppCurrency"] = theDT.Rows[0]["Currency"].ToString();
                        Session["AppUserEmployeeId"] = theDS.Tables[0].Rows[0]["EmployeeId"].ToString();

                        //Session["AppSystemId"] = theDT.Rows[0]["SystemId"].ToString();

                        #region "ModuleId"
                        Session["AppModule"] = theDS.Tables[3];
                        DataView theSCMDV = new DataView(theDS.Tables[3]);
                        theSCMDV.RowFilter = "ModuleId=201";
                        if (theSCMDV.Count > 0)
                            Session["SCMModule"] = theSCMDV[0]["ModuleName"];
                        #endregion
                        IQWebUtils theIQUtils = new IQWebUtils();
                        //theIQUtils.CreateSessionObject(Session.SessionID); 
                        Session["Paperless"] = theDT.Rows[0]["Paperless"].ToString();
                        Session["Program"] = "";
                        LoginManager = null;
                        /////////////// Appointment Updates//////////////////
                        //UpdateAppointment();
                        /////////////////////////////////////////////////////
                        if (theDS.Tables[3].Rows[0]["ExpPwdFlag"] != null)
                        {
                            if (Convert.ToInt32(theDS.Tables[0].Rows[0]["UserId"]) != 1)
                            {
                                if (Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdFlag"]) == 1)
                                {
                                    //DateTime lastcontDate = Convert.ToDateTime(theDS.Tables[0].Rows[0]["PwdDate"]).AddDays(Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdDays"]));
                                    //lastcontDate.AddDays(90);
                                    DateTime lastcontDate = Convert.ToDateTime(theDS.Tables[0].Rows[0]["PwdDate"]);
                                    TimeSpan t = Convert.ToDateTime(theDS.Tables[4].Rows[0]["CurrentDate"]) - lastcontDate;
                                    double NrOfDaysdiffernce = t.TotalDays;
                                    //int result = Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdDays"]) - Convert.ToInt32(NrOfDays);
                                    string msgString;
                                    string theUrl = string.Format("{0}", "./AdminForms/frmAdmin_ChangePassword.aspx");
                                    if (NrOfDaysdiffernce > Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdDays"]))
                                    {
                                        msgString = "Your Password has expired. Please Change it now.\\n";
                                        string script = "<script language = 'javascript' defer ='defer' id = 'changePwdfunction2'>\n";
                                        script += "alert('" + msgString + "');\n";
                                        string url = Request.RawUrl.ToString();
                                        Application["PrvFrm"] = url;
                                        Session["MandatoryChange"] = "1";
                                        script += "window.location.href='" + theUrl + "'\n";
                                        script += "</script>\n";
                                        ClientScript.RegisterStartupScript(Page.GetType(), "changePwdfunction2", script);

                                    }

                                    else
                                    {
                                        Response.Redirect("frmTouchFacilityHome.aspx");
                                    }
                                }
                                else
                                {
                                    Response.Redirect("frmTouchFacilityHome.aspx");
                                }
                            }
                            else
                            {
                                LoginManager = null;
                                Response.Redirect("frmTouchFacilityHome.aspx");
                            }

                        }

                        //Response.Redirect("frmFacilityHomenew.aspx");
                    }
                    else
                    {
                        IQCareMsgBox.Show("InvalidLogin", this);
                        return;
                    }
                }
            }
            catch (Exception err)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseLoadingasdf", "CloseLoading();", true);
                LoginManager = null;
            }
        }

        protected void patternLogin(DataSet ptrnLockRecord)
        {
            string userName = ptrnLockRecord.Tables[0].Rows[0].Field<string>("UserName");
            int locationID = ptrnLockRecord.Tables[0].Rows[0].Field<int>("ptrnLock_locationID");
            string lastURL = ptrnLockRecord.Tables[0].Rows[0].Field<string>("ptrnLock_lastURL");

            IUser LoginManager;
            try
            {
                string ObjFactoryParameter = "BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical";
                IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);

                LoginManager = (IUser)ObjectFactory.CreateInstance("BusinessProcess.Security.BUser, BusinessProcess.Security");
                DataSet theDS = LoginManager.GetUserCredentials(userName, locationID, Convert.ToInt32(Session["SystemId"].ToString()));
                if (theDS.Tables.Count > 0)
                {
                    int FacilityExist = 1;
                    if (theDS.Tables[5].Rows.Count > 0)
                    {
                        DataView theDV = new DataView();
                        FacilityExist = 0;
                        foreach (DataRow theDR in theDS.Tables[5].Rows)
                        {
                            if (Convert.ToInt32(theDR["GroupId"]) > 1)
                            {
                                theDV = new DataView(theDS.Tables[1]);
                                theDV.RowFilter = "FacilityID= " + locationID + "";
                                if (theDV.ToTable().Rows.Count > 0)
                                {
                                    FacilityExist = 1;
                                }
                            }
                            else if (Convert.ToInt32(theDR["GroupId"]) == 1)
                            {
                                FacilityExist = 1;
                            }
                        }
                    }
                    if (FacilityExist == 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AFail", "alert('Access Denied for this Facility-Contact Administrator')", true);
                        //IQCareMsgBox.Show("AccessDenied", this);
                        return;
                    }
                    Utility theUtil = new Utility();
                    if (theDS.Tables[0].Rows.Count > 0)
                    {
                        HttpCookie theCookie = Request.Cookies[userName];
                        if (theCookie != null)
                        {
                            string[] theVal = (theCookie.Value.ToString()).Split(',');
                            if (Convert.ToInt32(theVal[1]) >= 3)
                            {
                                MsgBuilder theBuilder = new MsgBuilder();
                                theBuilder.DataElements["MessageText"] = "User Account Locked. Try again after 5 Mins.";
                                IQCareMsgBox.Show("#C1", theBuilder, this);
                                return;
                            }
                        }
                    }

                    else
                    {
                        IQCareMsgBox.Show("InvalidLogin", this);
                        Init_Form(true);
                        return;
                    }

                    Session["AppUserId"] = Convert.ToString(theDS.Tables[0].Rows[0]["UserId"]);
                    Session["AppUserName"] = Convert.ToString(theDS.Tables[0].Rows[0]["UserFirstName"]) + " " + Convert.ToString(theDS.Tables[0].Rows[0]["UserLastName"]);
                    Session["EnrollFlag"] = theDS.Tables[1].Rows[0]["EnrollmentFlag"].ToString();
                    Session["CareEndFlag"] = theDS.Tables[1].Rows[0]["CareEndFlag"].ToString();
                    Session["IdentifierFlag"] = theDS.Tables[1].Rows[0]["IdentifierFlag"].ToString();
                    Session["UserRight"] = theDS.Tables[1];
                    DataTable theDT = theDS.Tables[2];
                    Session["AppLocationId"] = theDT.Rows[0]["FacilityID"].ToString();
                    Session["AppLocation"] = theDT.Rows[0]["FacilityName"].ToString();
                    Session["AppCountryId"] = theDT.Rows[0]["CountryID"].ToString();
                    Session["AppPosID"] = theDT.Rows[0]["PosID"].ToString();
                    Session["AppSatelliteId"] = theDT.Rows[0]["SatelliteID"].ToString();
                    Session["GracePeriod"] = theDT.Rows[0]["AppGracePeriod"].ToString();
                    Session["AppDateFormat"] = theDT.Rows[0]["DateFormat"].ToString();
                    Session["BackupDrive"] = theDT.Rows[0]["BackupDrive"].ToString();
                    Session["SystemId"] = theDT.Rows[0]["SystemId"].ToString();
                    Session["AppCurrency"] = theDT.Rows[0]["Currency"].ToString();
                    Session["AppUserEmployeeId"] = theDS.Tables[0].Rows[0]["EmployeeId"].ToString();

                    //Session["AppSystemId"] = theDT.Rows[0]["SystemId"].ToString();

                    #region "ModuleId"
                    Session["AppModule"] = theDS.Tables[3];
                    DataView theSCMDV = new DataView(theDS.Tables[3]);
                    theSCMDV.RowFilter = "ModuleId=201";
                    if (theSCMDV.Count > 0)
                        Session["SCMModule"] = theSCMDV[0]["ModuleName"];
                    #endregion
                    IQWebUtils theIQUtils = new IQWebUtils();
                    //theIQUtils.CreateSessionObject(Session.SessionID); 
                    Session["Paperless"] = theDT.Rows[0]["Paperless"].ToString();
                    Session["Program"] = "";
                    LoginManager = null;
                    /////////////// Appointment Updates//////////////////
                    //UpdateAppointment();
                    /////////////////////////////////////////////////////
                    if (theDS.Tables[3].Rows[0]["ExpPwdFlag"] != null)
                    {
                        if (Convert.ToInt32(theDS.Tables[0].Rows[0]["UserId"]) != 1)
                        {
                            if (Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdFlag"]) == 1)
                            {
                                //DateTime lastcontDate = Convert.ToDateTime(theDS.Tables[0].Rows[0]["PwdDate"]).AddDays(Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdDays"]));
                                //lastcontDate.AddDays(90);
                                DateTime lastcontDate = Convert.ToDateTime(theDS.Tables[0].Rows[0]["PwdDate"]);
                                TimeSpan t = Convert.ToDateTime(theDS.Tables[4].Rows[0]["CurrentDate"]) - lastcontDate;
                                double NrOfDaysdiffernce = t.TotalDays;
                                //int result = Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdDays"]) - Convert.ToInt32(NrOfDays);
                                string msgString;
                                string theUrl = string.Format("{0}", "./AdminForms/frmAdmin_ChangePassword.aspx");
                                if (NrOfDaysdiffernce > Convert.ToInt32(theDS.Tables[3].Rows[0]["ExpPwdDays"]))
                                {
                                    msgString = "Your Password has expired. Please Change it now.\\n";
                                    string script = "<script language = 'javascript' defer ='defer' id = 'changePwdfunction2'>\n";
                                    script += "alert('" + msgString + "');\n";
                                    string url = Request.RawUrl.ToString();
                                    Application["PrvFrm"] = url;
                                    Session["MandatoryChange"] = "1";
                                    script += "window.location.href='" + theUrl + "'\n";
                                    script += "</script>\n";
                                    ClientScript.RegisterStartupScript(Page.GetType(), "changePwdfunction2", script);

                                }

                                else
                                {
                                    Response.Redirect(lastURL);
                                }
                            }
                            else
                            {
                                Response.Redirect(lastURL);
                            }
                        }
                        else
                        {
                            LoginManager = null;
                            Response.Redirect(lastURL);
                        }

                    }

                    //Response.Redirect("frmFacilityHomenew.aspx");
                }
                else
                {
                    IQCareMsgBox.Show("InvalidLogin", this);
                    return;
                }

            }
            catch (Exception err)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseLoadingasdf", "CloseLoading();", true);
                LoginManager = null;
            }
        }

        protected void btnNotMe_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["ptrnLkd"] != null)
            {
                // Effectively log the user, whose lock was initiated, out
                HttpCookie newCookie = new HttpCookie("ptrnLkd");
                newCookie.Expires = DateTime.Now.Subtract(new TimeSpan(0, 20, 0));
                Response.Cookies.Add(newCookie);

                Response.Redirect("~/frmLogOff.aspx?Touch=true");
            }
        }

        #endregion

        #region Helper Methods

        private void BindCombo()
        {
            IUser UserManager = (IUser)ObjectFactory.CreateInstance("BusinessProcess.Security.BUser,BusinessProcess.Security");
            DataTable theDT = UserManager.GetFacilityList();
            BindFunctions theBind = new BindFunctions();

            if (theDT.Rows.Count == 1)
            {
                ddFacility.DataSource = theDT;
                ddFacility.DataTextField = "FacilityName";
                ddFacility.DataValueField = "FacilityId";
                ddFacility.DataBind();

            }
            else
            {
                theBind.BindCombo(ddFacility, theDT, "FacilityName", "FacilityID");
                if (ViewState["pwd"] != null)
                    txtPass.Text = ViewState["pwd"].ToString();
            }
            ViewState["pwd"] = null;
        }
        private bool ValidateLogin()
        {
            if (txtUname.Text.Trim() == "")
            {
                return false;
            }
            if (txtPass.Text.Trim() == "")
            {
                return false;
            }
            if (Convert.ToInt32(ddFacility.SelectedValue) < 1)
            {
                return false;
            }
            return true;
        }
        private void Init_Form(bool LoginFail)
        {
            if (Request.Cookies["ptrnLkd"] != null)    // user's session has timed out and is within the pattern lock time
            {
                contPtrnLock.Style["display"] = "block";
                updtLogin.Visible = false;
                isPattern = true;
                BUser ptrnLockUser = (BUser)ObjectFactory.CreateInstance("BusinessProcess.Administration.BUser, BusinessProcess.Administration");
                DataSet ptrnLockRecord = ptrnLockUser.GetUserLock(Convert.ToInt32(Request.Cookies["ptrnLkd"].Value.Split('_')[1]));
                lblCurUser.InnerText = ptrnLockRecord.Tables[0].Rows[0].Field<string>("UserFirstName") + " " + ptrnLockRecord.Tables[0].Rows[0].Field<string>("UserLastName");
            }
            //// Setting Application/Session Parameters /////

            Session.Timeout = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("appSettings"))["SessionTimeOut"]);
            Session.Add("AppUserId", "");
            Session.Add("AppUserName", "");
            Session.Add("EnrollFlag", "");
            Session.Add("IdentifierFlag", "");
            Session.Add("AppLocationId", "");
            Session.Add("AppLocation", "");
            Session.Add("AppCountryId", "");
            Session.Add("AppPosID", "");
            Session.Add("AppSatelliteId", "");
            Session.Add("GracePeriod", "");
            Session.Add("AppDateFormat", "");
            Session.Add("UserRight", "");
            Session.Add("BackupDrive", "");
            Session.Add("SystemId", "");
            Session.Add("ModuleId", "");
            Application.Add("AppCurrentDate", "");
            Session.Add("Program", "");
            Session.Add("AppCurrency", "");
            Session.Add("AppUserEmployeeId", "");
            Session.Add("CustomfrmDrug", "");
            Session.Add("CustomfrmLab", "");
            ////////////////////////////////////////

            GetApplicationParameters();
            BindCombo();


            lblversion.Text = AuthenticationManager.AppVersion;
            lblrelDate.Text = AuthenticationManager.ReleaseDate;

            if (LoginFail)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "loginInvalid", "CloseLoading();pInvalid();", true);
            }

        }
        private void GetApplicationParameters()
        {
            IUser ApplicationManager;
            ApplicationManager = (IUser)ObjectFactory.CreateInstance("BusinessProcess.Security.BUser, BusinessProcess.Security");
            DataSet theDS = ApplicationManager.GetFacilitySettings(3);
            DataTable theDT = theDS.Tables[0];

            if (theDT.Rows.Count < 1)
            {
                string theUrl = string.Format("{0}", "./AdminForms/frmAdmin_FacilityList.aspx");
                Response.Redirect(theUrl);
            }
            if (!string.IsNullOrEmpty(theDT.Rows[0]["Image"].ToString()))
            {
                //implement on live only
                //imgLogin.ImageUrl = string.Format("~/Touch/images/{0}", theDT.Rows[0]["Image"].ToString());

                //imgLogin.ImageUrl = "~/Touch/images/menuButtons/ELHospLogolg.png";
                if (TouchGlobals.ModuleName == "KNH")
                {
                    imgLogin.ImageUrl = "~/Touch/images/menuButtons/KNHlogo.png";
                    imgLogin.AlternateText = "Kenyatta National Hospital";
                    lnklogin.Attributes.Add("href", "styles/KNH.css");
                    imgdownlogo.ImageUrl = "images/menubuttons/IQCare-PADMTLogoKNH.png";

                    Image ContributorImage = (Image)rwContributors.ContentContainer.FindControl("ContribLogo");
                    ContributorImage.ImageUrl = "~/Touch/images/menuButtons/KNHlogo.png";
                }
                else
                {
                    imgLogin.ImageUrl = "~/Touch/images/menuButtons/ELHospLogolg.png";
                    imgLogin.AlternateText = "East London Hospital Complex";
                    lnklogin.Attributes.Add("href", "styles/PASDP.css");
                    imgdownlogo.ImageUrl = "images/menubuttons/IQCare-PADMTLogo275.png";

                    Image ContributorImage = (Image)rwContributors.ContentContainer.FindControl("ContribLogo");
                    ContributorImage.ImageUrl = "~/Touch/images/menuButtons/ELHospLogolg.png";
                }
            }
            else
            {
                //imgLogin.ImageUrl = "~/Touch/images/menuButtons/ELHospLogolg.png";
                if (TouchGlobals.ModuleName == "KNH")
                {
                    imgLogin.ImageUrl = "~/Touch/images/menuButtons/KNHlogo.png";
                }
                else
                {
                    imgLogin.ImageUrl = "~/Touch/images/menuButtons/ELHospLogolg.png";
                }
            }
            Session["SystemId"] = Convert.ToInt32(theDT.Rows[0]["SystemId"]);
            #region "Version Control"

            if (theDS.Tables[1].Rows[0]["AppVer"].ToString() != AuthenticationManager.AppVersion || ((DateTime)theDS.Tables[1].Rows[0]["RelDate"]).ToString("dd-MMM-yyyy") != AuthenticationManager.ReleaseDate)
            {
                string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
                script += "var ans=true;\n";
                script += "alert('You are using a Wrong Version of Application. Please contact Support staff.');\n";
                script += "if (ans==true)\n";
                script += "{\n";
                script += "window.close() - y;\n";
                script += "}\n";
                script += "</script>\n";
                ClientScript.RegisterStartupScript(Page.GetType(), "confirm", script);
                btnlogin.Enabled = false;
            }
            #endregion

            ApplicationManager = null;
            IIQCareSystem DateManager;
            DateManager = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            DateTime theDTime = DateManager.SystemDate();

            if (theDTime.ToString("dd-mm-yyyy") == "01-01-0001")
            {
                theDTime = DateTime.Now;
            }

            ViewState["theCurrentDate"] = theDTime;
            Application["AppCurrentDate"] = theDTime.ToString("dd-MMM-yyyy");
            Session["AppCurrentDateClass"] = theDTime.ToString("dd-MMM-yyyy");
        }
        private void GenerateCache()
        {
            IIQCareSystem DateManager;
            DateManager = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            DateTime theDTime = DateManager.SystemDate();

            System.IO.FileInfo theFileInfo1 = new System.IO.FileInfo(Server.MapPath(@"~\XMLFiles\AllMasters.con").ToString());
            System.IO.FileInfo theFileInfo2 = new System.IO.FileInfo(Server.MapPath(@"~\XMLFiles\DrugMasters.con").ToString());
            System.IO.FileInfo theFileInfo3 = new System.IO.FileInfo(Server.MapPath(@"~\XMLFiles\LabMasters.con").ToString());

            if (theFileInfo1.LastWriteTime.Date != theDTime.Date || theFileInfo2.LastWriteTime.Date != theDTime.Date || theFileInfo3.LastWriteTime.Date != theDTime.Date)
            {
                theFileInfo1.Delete();
                theFileInfo2.Delete();
                theFileInfo3.Delete();
                IIQCareSystem theCacheManager = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem,BusinessProcess.Security");
                DataSet theMainDS = theCacheManager.GetSystemCache();
                DataSet WriteXMLDS = new DataSet();

                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Provider"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Ward"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Division"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_District"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Reason"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Education"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Designation"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Employee"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Occupation"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Province"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Village"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Code"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_HIVAIDSCareTypes"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_ARTSponsor"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_HivDisease"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Assessment"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Symptom"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Decode"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Feature"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Function"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_HivDisclosure"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_LPTF"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_StoppedReason"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["mst_facility"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_HIVCareStatus"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_RelationshipType"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_TBStatus"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_ARVStatus"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_LostFollowreason"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Regimen"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_CouncellingType"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_CouncellingTopic"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_ReferredFrom"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_pmtctDeCode"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Module"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_ModDecode"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_ARVSideEffects"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_ModCode"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Country"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Town"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["VWDiseaseSymptom"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["VW_ICDList"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["mst_RegimenLine"].Copy());
                WriteXMLDS.WriteXml(Server.MapPath(@"~\XMLFiles\AllMasters.con").ToString(), XmlWriteMode.WriteSchema);

                WriteXMLDS.Tables.Clear();
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Strength"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_FrequencyUnits"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Drug"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Generic"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_DrugType"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_Frequency"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_DrugSchedule"].Copy());
                WriteXMLDS.WriteXml(Server.MapPath(@"~\XMLFiles\DrugMasters.con").ToString(), XmlWriteMode.WriteSchema);

                WriteXMLDS.Tables.Clear();
                WriteXMLDS.Tables.Add(theMainDS.Tables["Mst_LabTest"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Lnk_TestParameter"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Lnk_LabValue"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["Lnk_ParameterResult"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["LabTestOrder"].Copy());
                WriteXMLDS.Tables.Add(theMainDS.Tables["mst_PatientLabPeriod"].Copy());
                WriteXMLDS.WriteXml(Server.MapPath(@"~\XMLFiles\LabMasters.con").ToString(), XmlWriteMode.WriteSchema);
            }
        }
        private void UpdateAppointment()
        {
            //*******Update appointment status priviously missed, missed, careended and met from pending*******//                
            IUser LoginManager;
            LoginManager = (IUser)ObjectFactory.CreateInstance("BusinessProcess.Security.BUser, BusinessProcess.Security");
            int theAffectedRows = LoginManager.UpdateAppointmentStatus(Convert.ToString(Application["AppCurrentDate"]), Convert.ToInt16(Session["AppLocationId"]));
        }

        #endregion
    }
}
