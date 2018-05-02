#region Usings
//.Net Libs
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

//IQCare Libs
using Application.Presentation;
using Interface.Security;
using Interface.Reports;
using Interface.Clinical;

//Third party Libs
using Telerik.Web.UI;
using BusinessProcess.Administration;
using System.Configuration;
#endregion

namespace Touch
{
    //Backup before user control migration
    public partial class frmTouchFacilityHome : TouchPageBase
    {
        #region Local Variables
        DataSet theDS;
        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetSQLAuditData();
                Init_Form();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetResultsGridDataSource();
            rgResults.DataBind();
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

        }
        protected void rgResults_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GetResultsGridDataSource();
        }
        protected void rgResults_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (TouchGlobals.ModuleName == "KNH")
            {
                switch (e.Column.HeaderText)
                {
                    case "Ptn_Pk":
                        e.Column.Display = false;
                        break;
                    case "locationID":
                        e.Column.Display = false;
                        break;
                    case "Services":
                        e.Column.ItemStyle.Width = Unit.Pixel(60);
                        break;

                    default:
                        //leave visible
                        break;
                }
            }
            else
            {
                switch (e.Column.HeaderText)
                {
                    case "Ptn_Pk":
                        e.Column.Display = false;
                        break;
                    case "location ID":
                        e.Column.Display = false;
                        break;
                    default:
                        //leave visible
                        break;
                }
            }
        }
        protected void rgResults_SelectedCellChanged(object sender, EventArgs e)
        {
            //Check to see if Patient is in another facility - if so then transfer
            string PatientID = (rgResults.SelectedItems[0] as GridDataItem).GetDataKeyValue("Ptn_Pk").ToString();
            string oldLocationId = string.Empty;
            if (TouchGlobals.ModuleName == "KNH")
                oldLocationId = ((GridDataItem)rgResults.SelectedItems[0]).Cells[10].Text;
            else
                oldLocationId = ((GridDataItem)rgResults.SelectedItems[0]).Cells[9].Text;

            if (oldLocationId != Session["AppLocationId"].ToString())
            {
                Transfer.PatientToFacility(PatientID, oldLocationId, Session["AppLocationId"].ToString(), Convert.ToInt32(Session["AppUserId"]));
            }


            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "redirect", "GoToPatientHome('" + PatientID + "');", true);

            //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "redirect", "document.getElementById('hdGoToPatient').click();", true);
        }
        #endregion

        #region Helper Functions

        private void Init_Form()
        {
            hidmodule.Value = TouchGlobals.ModuleName;
            if (TouchGlobals.ModuleName == "KNH")
            {
                lnkfacilityhome.Attributes.Add("href", "Styles/KNH.css?reload");
                imgfindadd.ImageUrl = "images/SearchIconKNH01.jpg";
                rwLostToFollow.Skin = "MetroTouch";
                rwDueCareEnded.Skin = "MetroTouch";
                rwFindAdd.Skin = "MetroTouch";
                lblname.Text = "Sex";
                txtFolderNo.Visible = false;
                lbladdfolder.Visible = false;
                txtNewFolderNo.Visible = false;
                divfacility.Visible = true;
            }
            else
            {
                lnkfacilityhome.Attributes.Add("href", "Styles/PASDP.css?reload");
                imgfindadd.ImageUrl = "images/findadd.png";
                rwLostToFollow.Skin = "BlackMetroTouch";
                rwDueCareEnded.Skin = "BlackMetroTouch";
                rwFindAdd.Skin = "BlackMetroTouch";
                lblname.Text = "Folder No:";
                cmbsex.Visible = false;
                divfacility.Visible = false;
            }
            AppTitle.Text = TouchGlobals.RootTitle + " [" + Session["AppLocation"].ToString() + "]";
            if (Session["AppUserName"] != null)
                lblUserName.Text = Session["AppUserName"].ToString();
            if (Session["AppLocation"] != null)
                lblFacilityName.Text = Session["AppLocation"].ToString();

            //if D9Search is false then hide the Add window
            if (TouchGlobals.D9Search)
            {
                divAdd.Visible = false;
                divAddHdr.Visible = false;
            }

            //set max date for add user to todays date
            //dtpDOB.se = DateTime.Today;

            BindCombo();
            LoadStats();
            BindServiceDropdown();
            GetAllDropDowns();

            //Check the Mode
            if (Request.QueryString["Mode"] != null)
            {
                if (Request.QueryString["Mode"] == "Home")
                    rwFindAdd.VisibleOnPageLoad = false;
            }


        }
        protected void GetAllDropDowns()
        {
            IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");
            BindFunctions theBind = new BindFunctions();

            string GetSex = "select ID, Name from mst_decode where codeid = 4 and deleteflag = 0";
            string GetFacility = "select FacilityID,FacilityName from mst_facility";

            theBind.BindCombo(rcbSex, ptnMgr.ReturnDatatableQuery(GetSex), "Name", "ID");
            theBind.BindCombo(cmbsex, ptnMgr.ReturnDatatableQuery(GetSex), "Name", "ID");
            theBind.BindCombo(cmbfacility, ptnMgr.ReturnDatatableQuery(GetFacility), "FacilityName", "FacilityID");

        }
        protected void BindCombo()
        {
            IUser theLocationManager;
            theLocationManager = (IUser)ObjectFactory.CreateInstance("BusinessProcess.Security.BUser, BusinessProcess.Security");
            DataTable theDT = theLocationManager.GetFacilityList();
            ViewState["Facility"] = theDT;
            DataView theDV = new DataView(theDT);
            theDV.Sort = "FacilityID";
            IQCareUtils theUtils = new IQCareUtils();
            DataTable theDT1 = (DataTable)theUtils.CreateTableFromDataView(theDV);
            rcbFacility.DataSource = theDT1;
            rcbFacility.DataTextField = "FacilityName";
            rcbFacility.DataValueField = "FacilityId";
            rcbFacility.DataBind();

            rcbFacility.SelectedValue = Session["AppLocationId"].ToString();
        }
        protected void LoadStats()
        {
            IFacility FacilityManager;
            FacilityManager = (IFacility)ObjectFactory.CreateInstance("BusinessProcess.Security.BFacility, BusinessProcess.Security");
            theDS = FacilityManager.GetTouchFacilityStats(Convert.ToInt32(rcbFacility.SelectedValue));
            FacilityManager = null;

            if (theDS.Tables.Count > 0)
            {
                lblTot.Text = theDS.Tables[0].Rows[0].ItemArray[0].ToString();
                lblTotAct.Text = theDS.Tables[1].Rows[0].ItemArray[0].ToString();
            }

            GetLostToFollow();
            GetDueForCareEnded();
        }
        protected void BindServiceDropdown()
        {
            BindFunctions BindManager = new BindFunctions();
            IPatientRegistration ptnMgr = (IPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientRegistration, BusinessProcess.Clinical");
            DataSet DSModules = ptnMgr.GetModuleNames(Convert.ToInt32(Session["AppLocationId"]));

            DataTable theDT = new DataTable();
            theDT = DSModules.Tables[0];

            if (theDT.Rows.Count > 0)
            {
                BindManager.BindCombo(rcbService, theDT, "ModuleName", "ModuleID");
            }

            ptnMgr = null;
        }
        private void GetResultsGridDataSource()
        {
            DataTable dt = Search.All(rcbService, txtLName.Text, txtFName.Text, txtIdNo.Text, dtpDOBs.SelectedDate.ToString(), rcbFacility.SelectedValue, txtFolderNo.Text);
            if (TouchGlobals.ModuleName == "KNH")
            {
                string[] ColNames = new string[] { "PatientID", "PatientClinicID", "middlename", "Precision", "dobPatient" };
                string[] ChangeColNames = new string[] { "firstname", "lastname", "PatientIDType", "Name", "dob", "status" };
                string[] NewNames = new string[] { "First Name", "Last Name", "Services", "Sex", "DOB", "Status" };
                foreach (var name in ColNames)
                    dt.Columns.Remove(name);
                for (int i = 0; i < NewNames.Length; i++)
                {
                    dt.Columns[ChangeColNames.GetValue(i).ToString()].ColumnName = NewNames[i];
                }
                rgResults.DataSource = dt;
                //rgResults.MasterTableView.DataKeyNames = new string[] { "Ptn_Pk" };
                rgResults.Visible = true;
                //rgResults.Columns[3].ItemStyle.Width = Unit.Pixel(50);
            }
            else
            {
                string[] ColNames = new string[] { "PatientID", "PatientIDType", "PatientClinicID", "Precision", "dobPatient", "middlename" };
                string[] ChangeColNames = new string[] { "firstname", "lastname", "Name", "dob", "status" };
                string[] NewNames = new string[] { "First Name", "Last Name", "Sex", "DOB", "Status" };
                foreach (var name in ColNames)
                    dt.Columns.Remove(name);
                for (int i = 0; i < NewNames.Length; i++)
                {
                    dt.Columns[ChangeColNames.GetValue(i).ToString()].ColumnName = NewNames[i];
                }
                rgResults.DataSource = dt;
                //rgResults.MasterTableView.DataKeyNames = new string[] { "Ptn_Pk" };
                rgResults.Visible = true;
            }

        }


        #region Grid Stat Functions
        private void GetLostToFollow()
        {
            IReports ReportDetails = (IReports)ObjectFactory.CreateInstance("BusinessProcess.Reports.BReports, BusinessProcess.Reports");
            DataTable dtLost = (DataTable)ReportDetails.GetLosttoFollowupPatientReport(Convert.ToInt32(rcbFacility.SelectedValue)).Tables[0];
            dtLost.DefaultView.Sort = dtLost.Columns[1].ColumnName + " DESC";
            dtLost = dtLost.DefaultView.ToTable();
            rgdLostTF.DataSource = dtLost;
            rgdLostTF.DataBind();
        }
        private void GetDueForCareEnded()
        {
            IReports ReportDetails = (IReports)ObjectFactory.CreateInstance("BusinessProcess.Reports.BReports, BusinessProcess.Reports");
            
            string dateInString = Application["AppCurrentDate"].ToString();
            DateTime convertdate = DateTime.Parse(dateInString);
            DateTime startDate = convertdate.AddDays(-90);

            System.Data.DataSet dsARTUnknown = ReportDetails.GetPtnotvisitedrecentlyUnknown(startDate.ToString("dd MMM yyyy"), Application["AppCurrentDate"].ToString(), Convert.ToInt32(rcbFacility.SelectedValue));
            DataTable dtDueFCE = dsARTUnknown.Tables[0];
            dtDueFCE.DefaultView.Sort = dtDueFCE.Columns[1].ColumnName + " DESC";
            dtDueFCE = dtDueFCE.DefaultView.ToTable();
            dtDueFCE.Columns.Remove("PatientFile#");
            rgdDueForCare.DataSource = dtDueFCE;
            rgdDueForCare.DataBind();
        }
        #endregion

        #endregion

        #region Old Code


        protected void rcbFacility_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            LoadStats();
        }
        private static bool IsError = false; private static bool FolderNoExists = false;
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");

                if ((bool)ptnMgr.CheckPatientFolderNo(txtNewFolderNo.Text))
                {
                    FolderNoExists = true;
                }
                else
                {
                    FolderNoExists = false;
                    int theRes = (int)ptnMgr.SavePatientRecord(Session["AppLocationId"].ToString(), txtAFName.Text, txtALName.Text, rcbSex.SelectedValue,
                        dtpDOB.SelectedDate.ToString(), txtMidName.Text, txtNewFolderNo.Text);

                    txtAFName.Text = string.Empty;
                    txtALName.Text = string.Empty;
                    rcbSex.SelectedIndex = 0;
                    dtpDOB.Clear();
                    txtMidName.Text = string.Empty;
                    txtNewFolderNo.Text = string.Empty;

                    IsError = false;

                    updtWindow.Update();
                    Session["JustAddedPatient"] = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Patient record saved successfully');window.location = 'frmTouchPatientHome.aspx?PatientID=" + theRes + "';", true);
                }

            }
            catch (Exception ex)
            {
                IsError = true;
            }
            finally
            {
                if (IsError)
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('An errror occured please contact your Administrator')", true);
                if (FolderNoExists)
                {
                    var FolderExistsMessage = "'A patient with this folder number already exists. " + " \\n" + "Please use the Find Patient window to locate this patient '";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert(" + FolderExistsMessage + ")", true);
                }
            }
        }
        #endregion

        private void AuditTrailNotification()
        {
            Telerik.Web.UI.RadNotification RNot = new Telerik.Web.UI.RadNotification();
            RNot.ID = "RadNotification";
            RNot.VisibleOnPageLoad = true;
            RNot.Position = Telerik.Web.UI.NotificationPosition.Center;
            RNot.Width = 350;
            RNot.Height = 100;
            RNot.Animation = Telerik.Web.UI.NotificationAnimation.Fade;
            RNot.EnableRoundedCorners = true;
            RNot.EnableShadow = true;
            RNot.VisibleTitlebar = false;
            RNot.Text = "Audit Trail information is backedup";
            rdpStats.Controls.Add(RNot);
        }


        protected void btnGoToPat_Click(object sender, EventArgs e)
        {
            //Check to see if Patient is in another facility - if so then transfer
            string PatientID = Ptn_PkVal.Value;
            string LocationID = locationIDVal.Value;
            string oldLocationId = LocationID;

            if (oldLocationId != Session["AppLocationId"].ToString())
            {
                Transfer.PatientToFacility(PatientID, oldLocationId, Session["AppLocationId"].ToString(), Convert.ToInt32(Session["AppUserId"]));
            }


            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "redirect", "GoToPatientHome('" + PatientID + "');", true);
        }

        protected void btnSetPattern_Click(object sender, EventArgs e)
        {
            BUser ptrnLockUSer;
            ptrnLockUSer = (BUser)ObjectFactory.CreateInstance("BusinessProcess.Administration.BUser, BusinessProcess.Administration");

            ptrnLockUSer.SaveUserLock(Convert.ToInt32(Session["AppUserId"].ToString()), Convert.ToInt32(Session["AppLocationId"].ToString()), patLock.Value, Request.RawUrl.ToString());

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "ClosePattern", "closePatternWindow();", true);

        }

        //protected void GetSQLAuditData()
        //{
        //    IUser LoginManager = (IUser)ObjectFactory.CreateInstance("BusinessProcess.Security.BUser, BusinessProcess.Security");
        //    System.Data.DataSet theDS = LoginManager.GetAuditData();
        //    if (Convert.ToInt32(theDS.Tables[0].Rows[0]["Days"].ToString()) == 2)
        //    {
        //        string xmlFilesPath = string.Empty;
        //        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["XMLAuditTrail"].ToString()))
        //        {
        //            xmlFilesPath = ConfigurationManager.AppSettings["XMLAuditTrail"].ToString();
        //            string TodayDatetime = DateTime.Today.ToShortDateString().Replace("/", "-");
        //            string AuditTrail = xmlFilesPath + "_" + TodayDatetime + ".XML";
        //            System.IO.FileInfo theFileInfo1 = new System.IO.FileInfo(AuditTrail);
        //            System.Data.DataSet WriteXMLDS = new System.Data.DataSet();
        //            WriteXMLDS.Tables.Add(theDS.Tables[1].Copy());
        //            WriteXMLDS.WriteXml(AuditTrail, XmlWriteMode.WriteSchema);
        //            AuditTrailNotification();
        //        }
        //    }
        //}
    }
}
