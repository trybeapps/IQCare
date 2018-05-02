#region Usings
//.Net libs
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Runtime.Remoting;

// Third party libs
using Telerik.Web.UI;

//IQCare libs
using Touch.Custom_Forms;
using Touch.FormObjects;
using Interface.Clinical;
using Application.Presentation;

#endregion

namespace Touch
{
    public partial class tester : TouchPageBase
    {
        private static int patientID;
        protected void Page_Load(object sender, EventArgs e)
        {
            

            patientID = Convert.ToInt32(Request.QueryString["PatientID"]);
            Session["PatientId"] = patientID;
            if (!IsPostBack)
            {
                Init_Form();
            }
            else
            {
                if (Session["CurrentForm"] != null)
                {
                    //phForms.EnableViewState = true;
                    UserControl fr = (UserControl)Page.LoadControl("" + Session["CurrentForm"].ToString() + ".ascx");
                    fr.ID = "uc";
                    //phForms.Controls.Add(fr);
                    //updtForms.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
                }
            }
        }

        private void Init_Form()
        {
            AppTitle.Text = TouchGlobals.RootTitle + " [" + Session["AppLocation"].ToString() + "]";
            if (Session["AppUserName"] != null)
                lblUserName.Text = Session["AppUserName"].ToString();
            if (Session["AppLocation"] != null)
                lblFacilityName.Text = Session["AppLocation"].ToString();
            
            //DateTime theTmpDate;
            //string strPatientEnrollmentId = string.Empty;
            //IPatientHome PatientManager;

            string ObjFactoryParameter = "BusinessProcess.Clinical.BPatientRegistration, BusinessProcess.Clinical";
            
            //PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");

            ////System.Data.DataSet theDS = PatientManager.GetPatientDetails(Convert.ToInt32(Session["PatientId"]), Convert.ToInt32(Session["SystemId"]));
            //System.Data.DataSet theDS = PatientManager.GetPatientDetails(Convert.ToInt32(Session["PatientId"]), Convert.ToInt32(Session["SystemId"]), Convert.ToInt32(Session["TechnicalAreaId"]));
            //PatientManager = null;

            IPatientRegistration ptnMgr = (IPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);

            DataSet theDs = ptnMgr.GetPatientRegistration(patientID, 12);


            //#endregion
            DataTable theDTMod = (DataTable)Session["AppModule"];
            DataView theDVMod = new DataView(theDTMod);
            theDVMod.RowFilter = "ModuleId=" + Convert.ToInt32(Session["TechnicalAreaId"]);

            setUserDetailsInTab(theDs.Tables[0]);
            BindServiceDropdown();
        }

        private void setUserDetailsInTab(DataTable theDt)
        {
            DataRow dr = theDt.Rows[0]; string _middleName = string.Empty;

            if (dr["MiddleName"].ToString().Length > 1)
                _middleName += " ." + dr["MiddleName"].ToString().Substring(0, 1);

            string patientName = dr["FirstName"].ToString() + _middleName +
                " " + dr["LastName"].ToString();

            //set the vals
            lblAge.Text = dr["Age"].ToString();
            lblDistrict.Text = "Unknown";
            lblDOB.Text = dr["DOB"].ToString();
            lblFolderNo.Text = "No D9 Set";
            lblName.Text = patientName;
            lblPhone.Text = dr["Phone"].ToString();
            lblSex.Text = dr["Sex"].ToString();
            lblStatus.Text = dr["Status"].ToString() == "0" ? "Active" : "InActive";


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
        protected void rgResults_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GetResultsGridDataSource();
        }
        protected void rgResults_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
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
        protected void rgResults_SelectedCellChanged(object sender, EventArgs e)
        {
            //Check to see if Patient is in another facility - if so then transfer
            string PatientID = (rgResults.SelectedItems[0] as GridDataItem).GetDataKeyValue("Ptn_Pk").ToString();
            string oldLocationId = ((GridDataItem)rgResults.SelectedItems[0]).Cells[10].Text;
            if (oldLocationId != Session["AppLocationId"].ToString())
            {
                Transfer.PatientToFacility(PatientID, oldLocationId, Session["AppLocationId"].ToString(), Convert.ToInt32(Session["AppUserId"]));
            }


            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "redirect", "GoToPatientHome('" + PatientID + "');", true);

            //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "redirect", "document.getElementById('hdGoToPatient').click();", true);
        }
        private void GetResultsGridDataSource()
        {
            DataTable dt = Search.All(rcbService, txtLName.Text, txtFName.Text, txtIdNo.Text, dtpDOBs.SelectedDate.ToString(), txtNewFolderNo.Text);
            string[] ColNames = new string[] { "PatientID", "PatientIDType", "PatientClinicID", "Precision", "dobPatient" };
            string[] ChangeColNames = new string[] { "firstname", "middlename", "lastname", "Name", "dob", "status" };
            string[] NewNames = new string[] { "First Name", "MiddleName", "Last Name", "Sex", "DOB", "Status" };
            foreach (var name in ColNames)
                dt.Columns.Remove(name);
            for (int i = 0; i < NewNames.Length; i++)
            {
                dt.Columns[ChangeColNames.GetValue(i).ToString()].ColumnName = NewNames[i];
            }
            rgResults.DataSource = dt;
            rgResults.MasterTableView.DataKeyNames = new string[] { "Ptn_Pk" };
            rgResults.Visible = true;
        }

        #region OldCode
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetResultsGridDataSource();
            rgResults.DataBind();
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
        }


        public class Person
        {
            public Person(string LastName, string FirstName, string FolderNo, string Sex, string DOB, string Status, string Location)
            {
                _lastName = LastName;
                _firstName = FirstName;
                _folderNo = FolderNo;
                _sex = Sex; _dob = DOB; _status = Status;
                _location = Location;

            }
            private string _lastName;
            public string LastName
            {
                get { return _lastName; }
                set { _lastName = value; }
            }
            private string _firstName;
            public string FirstName
            {
                get { return _firstName; }
                set { _firstName = value; }
            }
            private string _folderNo;
            public string FolderNo
            {
                get { return _folderNo; }
                set { _folderNo = value; }
            }
            private string _sex;
            public string Sex
            {
                get { return _sex; }
                set { _sex = value; }
            }
            private string _dob;
            public string DOB
            {
                get { return _dob; }
                set { _dob = value; }
            }
            private string _status;
            public string Status
            {
                get { return _status; }
                set { _status = value; }
            }
            private string _location;
            public string Location
            {
                get { return _location; }
                set { _location = value; }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "AddUser", "document.getElementById('hdAddPatient').click();", true);
        }

        protected void btnClearViewState_Click(object sender, EventArgs e)
        {
            
            //phForms.EnableViewState = false;
            //ViewState.Clear();
            //phForms.Controls.Clear();
            //updtForms.Update();
        }
        protected void btnGoToVisit_Click(object sender, EventArgs e)
        {
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmVisitTouch.aspx";
            updtForms.Update();

        }
        protected void btnGoToRegistration_Click(object sender, EventArgs e)
        {
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmRegistrationTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString();
            updtForms.Update();
        }
        protected void btnGotoLab_Click(object sender, EventArgs e)
        {
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmLaboratoryTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString();
            updtForms.Update();
        }
        protected void btnGoToHist_Click(object sender, EventArgs e)
        {
            //phForms.Controls.Clear();
            //phForms.EnableViewState = false;
            //Touch.Custom_Forms.frmHistoryTouch fr = (frmHistoryTouch)Page.LoadControl("~/Touch/Custom Forms/frmHistoryTouch.ascx");
            //phForms.Controls.Add(fr);
            //updtForms.Update();
            //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
        }
        protected void btnGoToImmun_Click(object sender, EventArgs e)
        {
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmImmunisationTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString();
            updtForms.Update();
        }
        protected void btnGoToPharm_Click(object sender, EventArgs e)
        {
            Session["IsFirstLoad"] = "true";
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmPharmacyOrderManagementTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString();
            updtForms.Update();
        } 
        protected void btnGoToNon_Click(object sender, EventArgs e)
        {
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmClinicalNotesTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString();
            updtForms.Update();
        }
        protected void btnGoToRep_Click(object sender, EventArgs e)
        {
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmReportsTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString();
            updtForms.Update();
        }
        protected void btnGoToCare_Click(object sender, EventArgs e)
        {
            theFrame.Attributes["onload"] = "CloseLoading()";
            theFrame.Attributes["src"] = "Custom Forms/frmCareEndedTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString();
            updtForms.Update();
        }
        #endregion
    }
}