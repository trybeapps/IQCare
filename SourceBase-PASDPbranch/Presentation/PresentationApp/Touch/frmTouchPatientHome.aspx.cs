#region Usings
//.Net libs
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;
using System.Globalization;

// Third party libs
using Telerik.Web.UI;

//IQCare libs
using Touch.Custom_Forms;
using Touch.FormObjects;
using Interface.Clinical;
using Application.Presentation;
using ChartDirector;
using Interface.Administration;
using Application.Common;

#endregion

namespace Touch
{
    public partial class frmTouchPatientHome : TouchPageBase
    {
        private int _CheckInt(string TheInt)
        {
            int theVal = new int();
            if (int.TryParse(TheInt, out theVal))
                return theVal;
            else
                return 0;
        }
        private decimal _CheckDecimal(string TheDec)
        {
            decimal theVal = new int();
            if (decimal.TryParse(TheDec, out theVal))
                return theVal;
            else
                return 0;
        }
        private DateTime _CheckDate(string TheDate)
        {
            DateTime theVal = new DateTime();
            if (DateTime.TryParse(TheDate, out theVal))
                return theVal;
            else
                return theVal;
        }
        private bool _CheckBool(string TheBool)
        {
            switch (TheBool)
            {
                case "1":
                    TheBool = "true";
                    break;
                default:
                    TheBool = "false";
                    break;
            }
            bool theVal = new bool();
            if (bool.TryParse(TheBool, out theVal))
                return theVal;
            else
                return false;
        }
        private static int patientID; private static int FormCount = 1; private static bool IsError = false;
       
        protected void Page_Load(object sender, EventArgs e)
        {

            
            patientID = Convert.ToInt32(Request.QueryString["PatientID"]);
            Session["PatientId"] = patientID;
            if (!IsPostBack)
            {
                Init_Form();
                if (Session["JustAddedPatient"] != null)
                {
                    if ((bool)Session["JustAddedPatient"])
                    {
                        Session["JustAddedPatient"] = false;
                        hidJustAddPat.Value = "true";
                    }
                }
            }
            else
            {
                //if (Session["FormIsLoaded"] != null)
                if (FormMode.Value == "Loaded")
                {
                    if (Session["CurrentFormName"] != null)
                    {
                        UserControl fr = (UserControl)Page.LoadControl("Custom Forms/" + Session["CurrentFormName"].ToString() + ".ascx");
                        fr.ID = "ID" + Session["CurrentFormName"].ToString();
                        phForms.Controls.Add(fr);
                        //updtForms.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
                    }
                }
            }
            if (lblStatus.Text == "Inactive")
            {
                btnStatusActivate.Visible = true;
            }

            CheckUserRights();

            if (refreshViewExisting.Value == "true")
            {
                Session["GoingBack"] = "true";
                refreshViewExisting.Value = "false";
                updtHdButtons.Update();
            }
            updtAllModals.Update();
        }

        private void Init_Form()
        {
            hidmodule.Value = TouchGlobals.ModuleName;
            if (TouchGlobals.ModuleName == "KNH")
            {

                imgreg.ImageUrl = "images/menuButtons/RegistrationKNH01.png";
                imgreg.Width = Unit.Pixel(150);
                imgreg.Height = Unit.Pixel(150);

                imgvisit.ImageUrl = "images/menuButtons/ExpressVisitKNH01.png";
                imgvisit.Width = Unit.Pixel(150);
                imgvisit.Height = Unit.Pixel(150);

                imgpharmacy.ImageUrl = "images/menuButtons/PharmacyKNH01.png";
                imgpharmacy.Width = Unit.Pixel(150);
                imgpharmacy.Height = Unit.Pixel(150);

                imglab.ImageUrl = "images/menuButtons/LaboratoryKNH01.png";
                imglab.Width = Unit.Pixel(150);
                imglab.Height = Unit.Pixel(150);

                imgAdultFuKNH.ImageUrl = "images/menuButtons/AdultFUKNH01.png";
                imgAdultFuKNH.Width = Unit.Pixel(150);
                imgAdultFuKNH.Height = Unit.Pixel(150);

                imgAdultIEKNH.ImageUrl = "images/menuButtons/AdultIEKNH01.png";
                imgAdultIEKNH.Width = Unit.Pixel(150);
                imgAdultIEKNH.Height = Unit.Pixel(150);

                imgExpressKNH.ImageUrl = "images/menuButtons/ExpressKNHMetro.png";
                imgExpressKNH.Width = Unit.Pixel(150);
                imgExpressKNH.Height = Unit.Pixel(150);

                imgPedFUKNH.ImageUrl = "images/menuButtons/PedFUKNH01.png";
                imgPedFUKNH.Width = Unit.Pixel(150);
                imgPedFUKNH.Height = Unit.Pixel(150);

                imgPedIEKNH.ImageUrl = "images/menuButtons/PedIEKNH01.png";
                imgPedIEKNH.Width = Unit.Pixel(150);
                imgPedIEKNH.Height = Unit.Pixel(150);

                imgPsychotherapyFormknh.ImageUrl = "images/menuButtons/PsychotherapyFormknhMetro.png";
                imgPsychotherapyFormknh.Width = Unit.Pixel(150);
                imgPsychotherapyFormknh.Height = Unit.Pixel(150);

                imgleftlogo.ImageUrl = "images/menuButtons/KNHlogoshort.png";

                imghome.ImageUrl = "images/HomeIconKNH01.jpg";
                imgfindadd.ImageUrl = "images/SearchIconKNH01.jpg";
                imgsave.ImageUrl = "images/SaveIconKNH01.jpg";
                imgback.ImageUrl = "images/Prev1KNHNewMetro.png";
                imgmore.ImageUrl = "images/MoreIconKNH01.jpg";

                //hide image for pasdp
                //imgreg.Visible = false;
                imgimu.Visible = false;
                imglogo.Visible = false;
                imgnov.Visible = false;
                imgreport.Visible = false;
                imgcare.Visible = false;


                lnkhomepage.Attributes.Add("href", "Styles/KNH.css?reload");
                lnkscroll.Attributes.Add("href", "Styles/jquery.jscrollpane.lozengeKNH.css?reload");
                rwFindAdd.CssClass = "availability";
                rwFindAdd.Skin = "MetroTouch";

                lblChangeName.Text = "Sex";
                txtFolderNo.Visible = false;
                lblChangeName.Visible = true;

                divfacility.Visible = true;
            }
            else
            {
                //hide KNH
                imgAdultFuKNH.Visible = false;
                imgAdultIEKNH.Visible = false;
                imgExpressKNH.Visible = false;
                imgPedFUKNH.Visible = false;
                imgPedIEKNH.Visible = false;
                imgPsychotherapyFormknh.Visible = false;
                //
                imghome.ImageUrl = "images/home.png";
                imgfindadd.ImageUrl = "images/findadd.png";
                imgsave.ImageUrl = "images/btnSave.jpg";
                imgback.ImageUrl = "images/back.jpg";
                imgmore.ImageUrl = "images/more.jpg";

                imgvisit.ImageUrl = "images/menuButtons/VisitSA.png";
                imgreg.ImageUrl = "images/menuButtons/RegistrationSA.png";
                imgpharmacy.ImageUrl = "images/menuButtons/PharmacySA.png";
                imgimu.ImageUrl = "images/menuButtons/ImmunisationSA.png";
                imglogo.ImageUrl = "images/menuButtons/ECapeCOA.png";
                imglab.ImageUrl = "images/menuButtons/LaboratorySA.png";
                imgnov.ImageUrl = "images/menuButtons/NonSA.png";
                imgreport.ImageUrl = "images/menuButtons/ReportsSA.png";
                imgcare.ImageUrl = "images/menuButtons/CareEndedSA.png";
                imgleftlogo.ImageUrl = "images/menuButtons/ELHospLogo.png";
                lnkhomepage.Attributes.Add("href", "Styles/PASDP.css?reload");
                lnkscroll.Attributes.Add("href", "Styles/jquery.jscrollpane.lozenge.css?reload");
                rwFindAdd.Skin = "BlackMetroTouch";

                lblChangeName.Text = "Folder No:";
                cmbsex.Visible = false;
                lblChangeName.Visible = true;

                divfacility.Visible = false;
            }

            AppTitle.Text = TouchGlobals.RootTitle + " [" + Session["AppLocation"].ToString() + "]";
            if (Session["AppUserName"] != null)
                lblUserName.Text = Session["AppUserName"].ToString();
            if (Session["AppLocation"] != null)
                lblFacilityName.Text = Session["AppLocation"].ToString();

            string ObjFactoryParameter = "BusinessProcess.Clinical.BPatientRegistration, BusinessProcess.Clinical";

            IPatientRegistration ptnMgr = (IPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);

            System.Data.DataSet theDs = ptnMgr.GetPatientRegistration(patientID, 12);


            DataTable theDTMod = (DataTable)Session["AppModule"];
            DataView theDVMod = new DataView(theDTMod);
            theDVMod.RowFilter = "ModuleId=" + Convert.ToInt32(Session["TechnicalAreaId"]);

            setUserDetailsInTab(theDs.Tables[0]);
            BindServiceDropdown();
            BindGraph();
        }

        private void CheckUserRights()
        {
            /***************** Check For User Rights on Visit Form ****************/
            AuthenticationManager Authentication = new AuthenticationManager();

            if (Convert.ToInt32(ViewState["visitPk"]) == 0)
            {
                if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPInitialandFollowup, FunctionAccess.Add, (DataTable)Session["UserRight"]))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep1v", "$('#imgvisit').attr('onclick','').unbind('click');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep2v", "$(function(){ $('#imgvisit').click(function() {  alert( 'You do not have permission to add a Visit.' ); }); });", true);
                }

                if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPRegistrationForm, FunctionAccess.Add, (DataTable)Session["UserRight"]))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep1r", "$('#imgreg').attr('onclick','').unbind('click');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep2r", "$(function(){ $('#imgreg').click(function() {  alert( 'You do not have permission to Register a patient.' ); }); });", true);
                }

                if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPLabrotary, FunctionAccess.Add, (DataTable)Session["UserRight"]))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep1l", "$('#imglab').attr('onclick','').unbind('click');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep2l", "$(function(){ $('#imglab').click(function() {  alert( 'You do not have permission to add a Laboratory Order' ); }); });", true);
                }

                if(!Authentication.HasFunctionRight(ApplicationAccess.PASDPPharmacyform, FunctionAccess.Add, (DataTable)Session["UserRight"]))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep1p", "$('#imgpharmacy').attr('onclick','').unbind('click');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep2p", "$(function(){ $('#imgpharmacy').click(function() {  alert( 'You do not have permission to add a Pharmacy Order' ); }); });", true);
                }

                if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPNonVisitClinicalNote, FunctionAccess.Add, (DataTable)Session["UserRight"]))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep1n", "$('#imgnov').attr('onclick','').unbind('click');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep2n", "$(function(){ $('#imgnov').click(function() {  alert( 'You do not have permission to add a Non Visit Clinical Note' ); }); });", true);
                }
                if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPImmunisation, FunctionAccess.Add, (DataTable)Session["UserRight"]))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep1i", "$('#imgimu').attr('onclick','').unbind('click');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "noAddstep2i", "$(function(){ $('#imgimu').click(function() {  alert( 'You do not have permission to add an Immunisation Record' ); }); });", true);
                }
            }
        }

        private void setUserDetailsInTab(DataTable theDt)
        {
            DataRow dr = theDt.Rows[0]; string _middleName = string.Empty;

            if (dr["MiddleName"].ToString().Length > 1)
                _middleName += dr["MiddleName"].ToString().Substring(0, 1) + ".";

            string patientName = dr["FirstName"].ToString() + " " + _middleName +
                " " + dr["LastName"].ToString();

            //set the vals
            lblAge.Text = dr["Age"].ToString();
            lblDistrict.Text = "Unknown";
            lblDOB.Text = dr["DOB"].ToString();
            if (dr["D9FolderNo"].ToString() == "") lblFolderNo.Text = "No D9 Set";
            else lblFolderNo.Text = dr["D9FolderNo"].ToString();
            lblName.Text = patientName;
            Session["patientname"] = patientName;
            lblPhone.Text = dr["Phone"].ToString();
            lblSex.Text = dr["Sex"].ToString();
            string strPat = "0";
            if (dr["Status"].ToString() != null)
            {
                if (dr["Status"].ToString() == "1")
                {
                    strPat = "1";
                }
                else
                    strPat = "0";
            }
            lblStatus.Text = strPat.ToString() == "0" ? "Active" : "Inactive";

            IIQTouchPatientRegistration DistrictMng = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");
            if (dr["DistrictName"].ToString() != "")
            {
                string GetDistricts = "select Name FROM mst_district WHERE ID = " + dr["DistrictName"].ToString() + " AND DeleteFlag = 0 and systemId = 3";
                DataTable DistrictDT = DistrictMng.ReturnDatatableQuery(GetDistricts);
                if (DistrictDT.Rows.Count > 0)
                    lblDistrict.Text = DistrictDT.Rows[0][0].ToString();
            }
        }

        protected void BindServiceDropdown()
        {
            BindFunctions BindManager = new BindFunctions();
            IIQTouchPatientRegistration ptnMgrTouch = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");
            IPatientRegistration ptnMgr = (IPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientRegistration, BusinessProcess.Clinical");
            System.Data.DataSet DSModules = ptnMgr.GetModuleNames(Convert.ToInt32(Session["AppLocationId"]));

            DataTable theDT = new DataTable();
            theDT = DSModules.Tables[0];

            if (theDT.Rows.Count > 0)
            {
                BindManager.BindCombo(rcbService, theDT, "ModuleName", "ModuleID");
            }
            string GetSex = "select ID, Name from mst_decode where codeid = 4 and deleteflag = 0";

            BindManager.BindCombo(cmbsex, ptnMgrTouch.ReturnDatatableQuery(GetSex), "Name", "ID");

            ptnMgr = null;
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
            string oldLocationId = ((GridDataItem)rgResults.SelectedItems[0]).Cells[10].Text;
            string dob = Ptn_DOB.Value;
            if (oldLocationId != Session["AppLocationId"].ToString())
            {
                Transfer.PatientToFacility(PatientID, oldLocationId, Session["AppLocationId"].ToString(), Convert.ToInt32(Session["AppUserId"]));
            }
            if (dob != null)
            {
                int PatientAge = GetPatientAge(Convert.ToDateTime(dob), Convert.ToDateTime(Application["AppCurrentDate"]));
                Session["PatientAge"] = PatientAge;
            }

            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "redirect", "GoToPatientHome('" + PatientID + "');", true);

        }
        private void GetResultsGridDataSource()
        {
            DataTable dt = Search.All(rcbService, txtLName.Text, txtFName.Text, txtIdNo.Text, dtpDOBs.SelectedDate.ToString(), txtFolderNo.Text);
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
        protected void btnGoToPat_Click(object sender, EventArgs e)
        {
            //Check to see if Patient is in another facility - if so then transfer
            string PatientID = Ptn_PkVal.Value;
            string LocationID = locationIDVal.Value;
            string oldLocationId = LocationID;
            string dob = Ptn_DOB.Value;
            if (oldLocationId != Session["AppLocationId"].ToString())
            {
                Transfer.PatientToFacility(PatientID, oldLocationId, Session["AppLocationId"].ToString(), Convert.ToInt32(Session["AppUserId"]));
            }
            if (!String.IsNullOrEmpty(dob))
            {
                int PatientAge = GetPatientAge(Convert.ToDateTime(dob), Convert.ToDateTime(Application["AppCurrentDate"]));
                Session["PatientAge"] = PatientAge;
            }

            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "redirect", "GoToPatientHome('" + PatientID + "');", true);
        }
        #region Graphing
        public void BindGraph()
        {
            DateTime theTmpDate;
            IPatientHome PatientManager;
            PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
            System.Data.DataSet theDS = PatientManager.IQTouchGetPatientDetails(Convert.ToInt32(Request.QueryString["PatientID"]));

            /*CD4 and Viral Load Graph */
            double[] CD4 = new Double[theDS.Tables[0].Rows.Count];
            for (Int32 a = 0, l = CD4.Length; a < l; a++)
            {
                if (theDS.Tables[0].Rows[a]["TestResult"] != System.DBNull.Value)
                {
                    CD4.SetValue(Convert.ToDouble(theDS.Tables[0].Rows[a]["TestResult"]), a);
                }
            }

            double[] ViralLoad = new Double[theDS.Tables[1].Rows.Count];
            for (Int32 a = 0, l = ViralLoad.Length; a < l; a++)
            {
                if (theDS.Tables[1].Rows[a]["TestResult"] != System.DBNull.Value)
                {
                    lblviralload.Text = Convert.ToDateTime(theDS.Tables[1].Rows[a]["DATE"].ToString()).ToString("dd-MMM-yyyy") + " " + theDS.Tables[1].Rows[a]["TestResult"].ToString();
                    ViralLoad.SetValue(Convert.ToDouble(theDS.Tables[1].Rows[a]["TestResult"]), a);
                }
            }

            DateTime[] YearCD4 = new DateTime[theDS.Tables[0].Rows.Count];
            for (Int32 a = 0, l = YearCD4.Length; a < l; a++)
            {
                YearCD4.SetValue((DateTime)theDS.Tables[0].Rows[a]["DATE"], a);
            }

            DateTime[] YearVL = new DateTime[theDS.Tables[1].Rows.Count];
            for (Int32 a = 0, l = YearVL.Length; a < l; a++)
            {
                YearVL.SetValue(theDS.Tables[1].Rows[a]["DATE"], a);
            }

            DateTime[] Year = new DateTime[theDS.Tables[2].Rows.Count];
            for (Int32 a = 0, l = Year.Length; a < l; a++)
            {
                Year.SetValue(theDS.Tables[2].Rows[a]["DATE"], a);
            }
            //18thAug2009 createChartCD4(CD4, ViralLoad, YearCD4, YearVL, Year);
            Chart.setLicenseCode("DEVP-2AC2-336W-54FM-EAB2-F8E2");
            createChartCD4(WebChartViewerCD4VL, CD4, ViralLoad, YearCD4, YearVL, Year);

            // BMI
            double[] Height = new Double[theDS.Tables[3].Rows.Count];
            for (Int32 a = 0, l = Height.Length; a < l; a++)
            {
                if (theDS.Tables[3].Rows[a]["Height"] != System.DBNull.Value)
                {
                    Height.SetValue(Convert.ToDouble(theDS.Tables[3].Rows[a]["Height"]), a);
                }
            }
            double[] Weight = new Double[theDS.Tables[3].Rows.Count];
            for (Int32 a = 0, l = Weight.Length; a < l; a++)
            {
                if (theDS.Tables[3].Rows[a]["Weight"] != System.DBNull.Value)
                {
                    Weight.SetValue(Convert.ToDouble(theDS.Tables[3].Rows[a]["Weight"]), a);
                }
            }

            double[] BMI = new Double[theDS.Tables[3].Rows.Count];
            for (Int32 a = 0, l = Weight.Length; a < l; a++)
            {
                if (theDS.Tables[3].Rows[a]["BMI"] != System.DBNull.Value)
                { BMI.SetValue(Convert.ToDouble(theDS.Tables[3].Rows[a]["BMI"]), a); }
            }

            DateTime[] YearWeightBMI = new DateTime[theDS.Tables[3].Rows.Count];
            for (Int32 a = 0, l = YearWeightBMI.Length; a < l; a++)
            {
                if (theDS.Tables[3].Rows[a]["Visit_OrderbyDate"] != System.DBNull.Value)
                {
                    YearWeightBMI.SetValue(theDS.Tables[3].Rows[a]["Visit_OrderbyDate"], a);
                }
            }
            // 18thAug2009 createChartWeight( Weight, BMI, YearWeightBMI);
            createChartWeight(WebChartViewerWeight, Weight, BMI, YearWeightBMI);


            if (theDS.Tables[10].Rows.Count > 0)
            {
                theTmpDate = Convert.ToDateTime(theDS.Tables[10].Rows[0]["VisitDate"]);
                lbllstvisit.Text = theTmpDate.ToString(Session["AppDateFormat"].ToString());

            }
            else
            {
                lbllstvisit.Text = "";
            }
            if (theDS.Tables[11].Rows.Count > 0)
            {
                if (theDS.Tables[11].Rows[0]["AppDate"] != System.DBNull.Value)
                {
                    theTmpDate = Convert.ToDateTime(theDS.Tables[11].Rows[0]["AppDate"]);
                    lblnextapp.Text = theTmpDate.ToString(Session["AppDateFormat"].ToString());
                }
                else
                {
                    lblnextapp.Text = "";
                }
            }
            else
            {
                lblnextapp.Text = "";
            }
            //current ARV Regimen
            if (theDS.Tables[4].Rows.Count > 0)
            {
                if (theDS.Tables[4].Rows[0]["Current ARV Regimen"].ToString() != "0")
                {
                    lbllastregimen.Text = theDS.Tables[4].Rows[0]["Current ARV Regimen"].ToString();
                    lblarvregimen.Text = theDS.Tables[4].Rows[0]["Current ARV Regimen"].ToString();
                }
                else
                {
                    lbllastregimen.Text = "";
                    lblarvregimen.Text = "";
                }

            }
            else
            {
                lbllastregimen.Text = "";

            }

            //Last Visit Date Info
            if (theDS.Tables[12].Rows.Count > 0)
            {
                lblsidelastvisit.Text = theDS.Tables[12].Rows[0]["VisDate"].ToString() + " " + theDS.Tables[12].Rows[0]["DaysAgo"].ToString() + " days ago";
            }

            if (theDS.Tables[4].Rows.Count > 0)
            {
                if (theDS.Tables[4].Rows[0]["Current ARV StartDate"] != System.DBNull.Value)
                {
                    lblarvstartdate.Text = Convert.ToDateTime(theDS.Tables[4].Rows[0]["Current ARV StartDate"]).ToString(Session["AppDateFormat"].ToString());

                }
                else
                {
                    lblarvstartdate.Text = "";
                }

                if (theDS.Tables[4].Rows[0]["AidsRelief ARV StartDate"] != System.DBNull.Value)
                {
                    lblaidsrstartdate.Text = Convert.ToDateTime(theDS.Tables[4].Rows[0]["AidsRelief ARV StartDate"]).ToString(Session["AppDateFormat"].ToString());
                }
                else
                {
                    lblaidsrstartdate.Text = "";
                }
                if ((theDS.Tables[4].Rows[0]["Hist ARV StartDate"] != System.DBNull.Value) && (Session["SystemId"].ToString() == "1"))
                {
                    lblhistoricalsdate.Text = Convert.ToDateTime(theDS.Tables[4].Rows[0]["Hist ARV StartDate"]).ToString(Session["AppDateFormat"].ToString());
                }

                else if ((theDS.Tables[4].Rows[0]["Hist ARV StartDateCTC"] != System.DBNull.Value) && (Session["SystemId"].ToString() == "2"))
                {
                    lblhistoricalsdate.Text = Convert.ToDateTime(theDS.Tables[4].Rows[0]["Hist ARV StartDateCTC"]).ToString(Session["AppDateFormat"].ToString());
                }
                else
                {
                    lblhistoricalsdate.Text = "";
                }
            }

            //-----Most Recent CD4 - AidsRelief-----------------------------------------------------------------------
            if ((Convert.ToInt32(theDS.Tables[5].Rows[0]["RecentCD4Flag"].ToString()) != 0))
            {
                if (theDS.Tables[6].Rows.Count > 0)
                {
                    DataView theDV = new DataView(theDS.Tables[6]);
                    if (theDV.Count > 0)
                    {
                        string theRecentCD4 = "";
                        theRecentCD4 = theDV[0]["TestResults"].ToString();
                        lblmostresentcd4.Text = theRecentCD4;
                        if (theDV[0].Row.IsNull("OrderedByDate") == false)
                            theRecentCD4 = theRecentCD4 + "(" + ((DateTime)theDV[0]["OrderedByDate"]).ToString(Session["AppDateFormat"].ToString()) + ")";
                        lblnextcd4date.Text = theRecentCD4;
                    }
                }
            }
            if (Convert.ToInt32(theDS.Tables[7].Rows[0]["RecentCD4Flag"].ToString()) != 0)
            {
                if (theDS.Tables[8].Rows[0][0].ToString() != "")
                {
                    lblnextcd4date.Text = ((DateTime)theDS.Tables[8].Rows[0][0]).ToString(Session["AppDateFormat"].ToString());
                }
                else
                {
                    lblnextcd4date.Text = "";
                }
            }
            //lab history
            DateTime dtnow = System.DateTime.Now;
            string dt = dtnow.ToString("D");
            lbllabname.Text = "Latest Laboratory Results for: " + Session["patientname"].ToString() + "";
            lbldate.Text = "Report Date: " + dt + "";
            int patientid = Convert.ToInt32(Request.QueryString["PatientID"]);
            PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
            System.Data.DataSet theDSLab = PatientManager.GetPatientLabHistory(patientid);
            rgvlabhistory.DataSource = theDSLab.Tables[0];
            if (theDSLab.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < theDSLab.Tables[1].Rows.Count; i++)
                {
                    Label lbl = new Label();
                    lbl.ID = "lbl" + i;
                    lbl.ForeColor = System.Drawing.Color.Red;
                    lbl.Font.Bold = false;
                    lbl.Text = theDSLab.Tables[1].Rows[i][0].ToString() + "  ordered on " + theDSLab.Tables[1].Rows[i][1].ToString() + " has results pending.";
                    PnlConFields.Controls.Add(lbl);

                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                }
            }

            if (theDSLab.Tables[2].Rows.Count > 0)
            {
                var theLabDateRecent = new DateTime();
                var theLabDateLast = new DateTime();

                int MostRecentCD4 = Convert.ToInt32(theDSLab.Tables[2].Rows[0][0].ToString());
                if (MostRecentCD4 < 200)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl1 = new Label();
                    lbl1.ForeColor = System.Drawing.Color.Red;
                    lbl1.Font.Bold = false;

                    theLabDateRecent = DateTime.Parse(theDSLab.Tables[2].Rows[0][1].ToString());

                    lbl1.Text = "Most recent CD4 count (" + theDSLab.Tables[2].Rows[0][0].ToString() + "; " + theLabDateRecent.ToString("dd-MMM-yyyy") + ") < 200 ";
                    PnlConFields.Controls.Add(lbl1);
                }
                int preMostRecentCD4 = 0;
                if (theDSLab.Tables[2].Rows.Count > 1)
                {
                    if (theDSLab.Tables[2].Rows[1][0] != DBNull.Value)
                    {
                        preMostRecentCD4 = Convert.ToInt32(theDSLab.Tables[2].Rows[1][0].ToString());
                    }


                    if (MostRecentCD4 < preMostRecentCD4)
                    {
                        theLabDateRecent = DateTime.Parse(theDSLab.Tables[2].Rows[0][1].ToString());
                        theLabDateLast = DateTime.Parse(theDSLab.Tables[2].Rows[1][1].ToString());

                        PnlConFields.Controls.Add(new LiteralControl("<br>"));
                        Label lbl2 = new Label();
                        lbl2.Font.Bold = false;
                        lbl2.ForeColor = System.Drawing.Color.Red;
                        lbl2.Text = "Most recent CD4 count " +
                        "(" + theDSLab.Tables[2].Rows[0][0].ToString() + "; " + theLabDateRecent.ToString("dd-MMM-yyyy") + ") " +
                        "< prior CD4 count " +
                        "(" + theDSLab.Tables[2].Rows[1][0].ToString() + "; " + theLabDateLast.ToString("dd-MMM-yyyy") + ") ";
                        PnlConFields.Controls.Add(lbl2);
                    }
                }
            }
            if (theDSLab.Tables[3].Rows.Count > 0)
            {
                int MostRecentCD4PER = Convert.ToInt32(theDSLab.Tables[3].Rows[0][0].ToString());
                int preMostRecentCD4PER = 0;

                if (theDSLab.Tables[3].Rows.Count > 1)
                {
                    if (theDSLab.Tables[3].Rows[1][0] != DBNull.Value)
                    {
                        preMostRecentCD4PER = Convert.ToInt32(theDSLab.Tables[3].Rows[1][0].ToString());
                    }
                    if (MostRecentCD4PER < preMostRecentCD4PER)
                    {
                        PnlConFields.Controls.Add(new LiteralControl("<br>"));
                        Label lbl3 = new Label();
                        lbl3.Font.Bold = false;
                        lbl3.ForeColor = System.Drawing.Color.Red;
                        lbl3.Text = "Most recent CD4 percent < prior CD4 percent";
                        PnlConFields.Controls.Add(lbl3);
                    }
                }
            }
            int TotalCholesterol = 0;
            if (theDSLab.Tables[4].Rows.Count > 0)
            {
                TotalCholesterol = Convert.ToInt32(theDSLab.Tables[4].Rows[0][0].ToString());
                if (TotalCholesterol < 6)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl4 = new Label();
                    lbl4.Font.Bold = false;
                    lbl4.ForeColor = System.Drawing.Color.Red;
                    lbl4.Text = "Most recent Total Cholesterol <6 mmol/l";
                    PnlConFields.Controls.Add(lbl4);
                }
            }
            
            int Hb = 0;
            if (theDSLab.Tables[5].Rows.Count > 0)
            {
                Hb = Convert.ToInt32(theDSLab.Tables[5].Rows[0][0].ToString());
                DateTime hb_orderbydate = Convert.ToDateTime(theDSLab.Tables[5].Rows[0][1].ToString());
                if (Hb < 10)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl5 = new Label();
                    lbl5.Font.Bold = false;
                    lbl5.ForeColor = System.Drawing.Color.Red;
                    lbl5.Text = "Most recent Hb < 10  g/dl";
                    PnlConFields.Controls.Add(lbl5);
                }

            }
           
            //----------------------ALT/SGPT
            int altsgpt = 0;
            if (theDSLab.Tables[6].Rows.Count > 0)
            {
                altsgpt = Convert.ToInt32(theDSLab.Tables[6].Rows[0][0].ToString());
            }
            if (altsgpt > 100)
            {
                PnlConFields.Controls.Add(new LiteralControl("<br>"));
                Label lbl6 = new Label();
                lbl6.Font.Bold = false;
                lbl6.ForeColor = System.Drawing.Color.Red;
                lbl6.Text = "Most recent ALT > 100 U/L";
                PnlConFields.Controls.Add(lbl6);
            }
            //Case if (TodaysDate-Max(HbDate)>= 365
            int datediff = 0;
            if (theDSLab.Tables[7].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(theDSLab.Tables[7].Rows[0][0].ToString()))
                    datediff = Convert.ToInt32(theDSLab.Tables[7].Rows[0][0].ToString());
            }
            if (datediff >= 365)
            {
                PnlConFields.Controls.Add(new LiteralControl("<br>"));
                Label lbl7 = new Label();
                lbl7.Font.Bold = false;
                lbl7.ForeColor = System.Drawing.Color.Red;
                lbl7.Text = "Hb test due  " + theDSLab.Tables[7].Rows[0][1].ToString();
                PnlConFields.Controls.Add(lbl7);
            }
            int datediffCD4 = 0;
            if (theDSLab.Tables[8].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(theDSLab.Tables[8].Rows[0][0].ToString()))
                    datediffCD4 = Convert.ToInt32(theDSLab.Tables[8].Rows[0][0].ToString());
            }
            if (datediffCD4 >= 365)
            {
                PnlConFields.Controls.Add(new LiteralControl("<br>"));
                Label lbl8 = new Label();
                lbl8.Font.Bold = false;
                lbl8.ForeColor = System.Drawing.Color.Red;
                lbl8.Text = "CD4 Count due  " + theDSLab.Tables[8].Rows[0][1].ToString();
                PnlConFields.Controls.Add(lbl8);
            }
            int datediffCD4per = 0;
            if (theDSLab.Tables[9].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(theDSLab.Tables[9].Rows[0][0].ToString()))
                    datediffCD4per = Convert.ToInt32(theDSLab.Tables[9].Rows[0][0].ToString());
            }
            if (datediffCD4per >= 365)
            {
                PnlConFields.Controls.Add(new LiteralControl("<br>"));
                Label lbl9 = new Label();
                lbl9.Font.Bold = false;
                lbl9.ForeColor = System.Drawing.Color.Red;
                lbl9.Text = "CD4 Percent due  " + theDSLab.Tables[9].Rows[0][1].ToString();
                PnlConFields.Controls.Add(lbl9);
            }
            //-------Viral load >1000
            int viralloadfro90day = 0;
            if (theDSLab.Tables[10].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(theDSLab.Tables[10].Rows[0][0].ToString()))
                    viralloadfro90day = Convert.ToInt32(theDSLab.Tables[10].Rows[0][0].ToString());
            }
            if (viralloadfro90day >= 1000)
            {
                int daydiff = 0;
                if (!String.IsNullOrEmpty(theDSLab.Tables[10].Rows[0][0].ToString()))
                    daydiff = Convert.ToInt32(theDSLab.Tables[10].Rows[0][1].ToString());
                if (daydiff >= 90)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl10 = new Label();
                    lbl10.Font.Bold = false;
                    lbl10.ForeColor = System.Drawing.Color.Red;
                    lbl10.Text = "Viral load due  " + theDSLab.Tables[10].Rows[0][2].ToString();
                    PnlConFields.Controls.Add(lbl10);
                }
            }
            //-------Viral load >400
            int viralloadfro180day = 0;
            if (theDSLab.Tables[11].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(theDSLab.Tables[11].Rows[0][0].ToString()))
                    viralloadfro180day = Convert.ToInt32(theDSLab.Tables[11].Rows[0][0].ToString());
            }
            if (viralloadfro180day >= 400)
            {
                int daydiff = 0;
                if (!String.IsNullOrEmpty(theDSLab.Tables[11].Rows[0][0].ToString()))
                    daydiff = Convert.ToInt32(theDSLab.Tables[11].Rows[0][1].ToString());
                if (daydiff >= 180)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl11 = new Label();
                    lbl11.Font.Bold = false;
                    lbl11.ForeColor = System.Drawing.Color.Red;
                    lbl11.Text = "Viral load due  " + theDSLab.Tables[11].Rows[0][2].ToString();
                    PnlConFields.Controls.Add(lbl11);
                }
            }

            //-------Viral load 365

            int daydiff365 = 0;
            if (theDSLab.Tables[12].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(theDSLab.Tables[12].Rows[0][0].ToString()))
                    daydiff365 = Convert.ToInt32(theDSLab.Tables[12].Rows[0][1].ToString());
                if (daydiff365 >= 365)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl11 = new Label();
                    lbl11.Font.Bold = false;
                    lbl11.ForeColor = System.Drawing.Color.Red;
                    lbl11.Text = "Viral load due  " + theDSLab.Tables[12].Rows[0][2].ToString();
                    PnlConFields.Controls.Add(lbl11);
                }
            }
            //
            if (Convert.ToInt32(theDSLab.Tables[13].Rows[0][0].ToString()) == 1)
            {
                if (theDSLab.Tables[11].Rows.Count > 0)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl13 = new Label();
                    lbl13.Font.Bold = false;
                    lbl13.ForeColor = System.Drawing.Color.Red;
                    lbl13.Text = "Viral load due  " + theDSLab.Tables[11].Rows[0][2].ToString();
                    PnlConFields.Controls.Add(lbl13);
                }
            }

            if (theDSLab.Tables[14].Rows.Count > 0)
            {
                int find = 0;
                if (!String.IsNullOrEmpty(theDSLab.Tables[14].Rows[0][0].ToString()))
                    find = Convert.ToInt32(theDSLab.Tables[14].Rows[0][0].ToString());
                if (find == 1)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl14 = new Label();
                    lbl14.Font.Bold = false;
                    lbl14.ForeColor = System.Drawing.Color.Red;
                    lbl14.Text = "CD4 Count/Percent due  " + theDSLab.Tables[14].Rows[0][1].ToString();
                    PnlConFields.Controls.Add(lbl14);
                }
            }
            if (theDSLab.Tables[15].Rows.Count > 0)
            {
                int find = 0;
                if (!String.IsNullOrEmpty(theDSLab.Tables[15].Rows[0][0].ToString()))
                    find = Convert.ToInt32(theDSLab.Tables[15].Rows[0][0].ToString());
                if (find == 1)
                {
                    PnlConFields.Controls.Add(new LiteralControl("<br>"));
                    Label lbl14 = new Label();
                    lbl14.Font.Bold = false;
                    lbl14.ForeColor = System.Drawing.Color.Red;
                    lbl14.Text = "Hb test due  " + theDSLab.Tables[15].Rows[0][1].ToString();
                    PnlConFields.Controls.Add(lbl14);
                }
            }
        }
        //



        private void createChartCD4(WebChartViewer viewer, Double[] CD4, Double[] ViralLoad, DateTime[] YearCD4, DateTime[] YearVL, DateTime[] Year)
        {
            XYChart c = new XYChart(620, 400, 0xddddff, 0x000000, 1);
            c.addLegend(90, 10, false, "Arial Bold", 7).setBackground(0xcccccc);
            c.setPlotArea(60, 60, 470, 250, 0xffffff).setGridColor(0xcccccc, 0xccccccc);
            c.xAxis().setTitle("Year");
            c.xAxis().setLabelStyle("Arial", 8, 1).setFontAngle(90);
            c.yAxis().setTitle("CD4 Count");
            c.yAxis2().setTitle("Viral Load");
            c.yAxis().setLinearScale(50, 4500, 450, 0);
            c.yAxis2().setLogScale(1, 10000000, 10);

            LineLayer layer = c.addLineLayer2();

            layer.setLineWidth(2);
            layer.addDataSet(CD4, 0xff0000, "CD4 Count").setDataSymbol(Chart.CircleShape, 5);
            layer.setXData(YearCD4);

            LineLayer layer1 = c.addLineLayer2();
            layer1.setLineWidth(2);
            layer1.setUseYAxis2();
            layer1.addDataSet(ViralLoad, 0x008800, "Viral load").setDataSymbol(Chart.CircleShape, 5);
            layer1.setXData(YearVL);

             //Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);
            viewer.ImageMap = c.getHTMLImageMap("", "",
                "title='{dataSetName} on {x|dd-MMM-yyyy}={value}'");

            c.makeChart(Server.MapPath("~/Touch/mychart.png"));
            viewer.ImageUrl = "~/Touch/mychart.png";
        }
        private void createChartWeight(WebChartViewer Wviewer, Double[] Weight, Double[] BMI, DateTime[] YearWeightBMI)
        {
            XYChart c = new XYChart(600, 400, 0xddddff, 0x000000, 1);
            c.addLegend(90, 10, false, "Arial Bold", 7).setBackground(0xcccccc);
            c.setPlotArea(60, 60, 470, 250, 0xffffff).setGridColor(0xcccccc, 0xccccccc);
            c.xAxis().setTitle("Year");
            c.xAxis().setLabelStyle("Arial", 8, 1).setFontAngle(90);
            c.yAxis().setLinearScale(0, 75, 5, 0);
            c.yAxis2().setLinearScale(0, 50, 5, 0);
            c.yAxis().setTitle("Weight");
            c.yAxis2().setTitle("BMI");
            c.yAxis2().setLogScale(0, 500, 10);

            LineLayer layer = c.addLineLayer2();
            layer.setLineWidth(2);
            layer.addDataSet(Weight, 0xff0000, "Weight").setDataSymbol(Chart.CircleShape, 5);
            int count = YearWeightBMI.Length;
            layer.setXData(YearWeightBMI);

            LineLayer layer1 = c.addLineLayer2();
            layer1.setLineWidth(2);
            layer1.setUseYAxis2();
            layer1.addDataSet(BMI, 0x008800, "BMI").setDataSymbol(Chart.CircleShape, 5);
            layer1.setXData(YearWeightBMI);

            // Output the chart
            Wviewer.Image = c.makeWebImage(Chart.PNG);
            //Include tool tip for the chart
            //Wviewer.ImageMap = c.getHTMLImageMap("", "",
             //"title='{dataSetName} Count on {xLabel}={value}'");

            Wviewer.ImageMap = c.getHTMLImageMap("", "",
             "title='{dataSetName} on {x|dd-MMM-yyyy}={value}'");

            

            c.makeChart(Server.MapPath("~/Touch/mychart1.png"));
            Wviewer.ImageUrl = "~/Touch/mychart1.png";

        }

        #endregion

        #region OldCode
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetResultsGridDataSource();
            rgResults.DataBind();
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';CloseMinLoading('rwFindAdd_C_divFind');", true);

        }
        private static bool FolderNoExists = false;
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(2000);
            //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "AddUser", "document.getElementById('hdAddPatient').click();", true);
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

                    //updtFindPatient.Update();
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

        protected void ClearFVState()
        {
            foreach (Control item in phForms.Controls)
            {
                phForms.Controls.Remove(item);

            }
            Session["FormIsLoaded"] = null; //Session["CurrentFormName"] = null;

            phForms.Controls.Clear();
            updtForms.Update();
            BindGraph();
            updtSideBar.Update();

            //Session["JustAddedRec"] = "true";
        }
        protected void btnClearViewState_Click(object sender, EventArgs e)
        {
            ClearFVState();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SlideBackAfter", "BackToMain();", true);
            
        }
        protected void btnGoToVisit_Click(object sender, EventArgs e)
        {
            Session["FormIsLoaded"] = null;
            Session["CurrentFormName"] = "frmVisitTouch";
            Session["IsFirstLoad"] = "true";
            Session["VisitEditMode"] = "false";

            LoadFormControl();

            //Handle Save button
            string FormID = "ID" + Session["CurrentFormName"].ToString();
            hdSaveBtnVal.Value = FormID + "_btnSave"; updtPatientSave.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);

        }
        protected void btnGoToRegistration_Click(object sender, EventArgs e)
        {

            Session["FormIsLoaded"] = null;
            Session["CurrentFormName"] = "frmRegistrationTouch"; // "frmRegistrationTouch";
            Session["IsFirstLoad"] = "true";

            LoadFormControl();

            //Handle Save button
            string FormID = "ID" + Session["CurrentFormName"].ToString();
            hdSaveBtnVal.Value = FormID + "_btnSave"; updtPatientSave.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

        }
        protected void btnGotoLab_Click(object sender, EventArgs e)
        {

                    Session["FormIsLoaded"] = null;
                    Session["CurrentFormName"] = "frmLaboratoryTouch";
                    Session["IsFirstLoad"] = "true";

                    LoadFormControl();

                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);


        }
        protected void btnGoToHist_Click(object sender, EventArgs e)
        {
            phForms.Controls.Clear();
            phForms.EnableViewState = false;
            Touch.Custom_Forms.frmHistoryTouch fr = (frmHistoryTouch)Page.LoadControl("Custom Forms/frmHistoryTouch.ascx");
            phForms.Controls.Add(fr);
            updtForms.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
        }
        protected void btnGoToImmun_Click(object sender, EventArgs e)
        {
                    Session["FormIsLoaded"] = null;

                    Session["CurrentFormName"] = "frmImmunisationTouch";
                    Session["IsFirstLoad"] = "true";

                    LoadFormControl();

                    string FormID = "ID" + Session["CurrentFormName"].ToString();
                    hdSaveBtnVal.Value = FormID + "_btnSave";
                    updtPatientSave.Update();
                    updtForms.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
        }
        protected void btnGoToPharm_Click(object sender, EventArgs e)
        {
            if (TouchGlobals.ModuleName == "KNH")
            {
                Session["FormIsLoaded"] = null;
                Session["CurrentFormName"] = "frmKNHPharmacyOrderManagement";
                Session["IsFirstLoad"] = "true";
                Touch.Custom_Forms.frmKNHPharmacyOrderManagement fr = (frmKNHPharmacyOrderManagement)Page.LoadControl("Custom Forms/frmKNHPharmacyOrderManagement.ascx");
                FormCount = FormCount + 1;
                fr.ID = "ID" + Session["CurrentFormName"].ToString();
                foreach (Control item in phForms.Controls)
                {
                    phForms.Controls.Remove(item);

                }
                frmKNHPharmacyOrderManagement theFrm = (frmKNHPharmacyOrderManagement)phForms.FindControl("ID" + Session["CurrentFormName"].ToString());
                if (theFrm != null)
                {
                    theFrm.Visible = true;
                }
                else
                {
                    if (phForms.Controls.Count == 0)
                    {
                        phForms.Controls.Add(fr);
                    }
                }
                updtForms.Update();
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
            }
            else
            {
               
                        Session["FormIsLoaded"] = null;
                        Session["CurrentFormName"] = "frmPharmacyOrderManagementTouch";
                        Session["IsFirstLoad"] = "true";

                        LoadFormControl();

                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);

            }
        }
        protected void btnGoToNon_Click(object sender, EventArgs e)
        {

                    Session["FormIsLoaded"] = null;

                    Session["CurrentFormName"] = "frmClinicalNotesTouch";
                    Session["IsFirstLoad"] = "true";
                    Session["ClinicalNoteEditMode"] = "false";

                    LoadFormControl();

                    //Handle Save button
                    string FormID = "ID" + Session["CurrentFormName"].ToString();
                    hdSaveBtnVal.Value = FormID + "_btnSave"; updtPatientSave.Update();
                    updtForms.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);

        }
        protected void btnGoToRep_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "swipe", "SwipeLeft();", true);
            Session["CurrentFormName"] = "frmReportsTouch";
            LoadFormControl();

            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
        }
        protected void btnGoToCare_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "swipe", "SwipeLeft();", true);
            Session["FormIsLoaded"] = null;
            Session["CurrentFormName"] = "frmCareEndedTouch";
            Session["IsFirstLoad"] = "true";

            LoadFormControl();

            //Handle Save button
            string FormID = "ID" + Session["CurrentFormName"].ToString();
            hdSaveBtnVal.Value = FormID + "_btnSave"; updtPatientSave.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
        }
        protected void btnExpress_Click(object sender, EventArgs e)
        {
            Session["FormIsLoaded"] = null;

            Session["CurrentFormName"] = "frmKNHExpress";
            Session["IsFirstLoad"] = "true";

            Touch.Custom_Forms.frmKNHExpress fr = (frmKNHExpress)Page.LoadControl("Custom Forms/frmKNHExpress.ascx");
            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            foreach (Control item in phForms.Controls)
            {
                phForms.Controls.Remove(item);
            }
            frmKNHExpress theFrm = (frmKNHExpress)phForms.FindControl("ID" + Session["CurrentFormName"].ToString());
            if (theFrm != null)
            {
                theFrm.Visible = true;
            }
            else
            {
                phForms.Controls.Add(fr);
            }
            hdSaveBtnVal.Value = fr.ID + "_btnSave";
            updtPatientSave.Update();
            updtForms.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);

        }
        protected void btnAdultIE_Click(object sender, EventArgs e)
        {
            Session["FormIsLoaded"] = null;

            Session["CurrentFormName"] = "KNHAdultIEvaluationForm";
            Session["IsFirstLoad"] = "true";

            //Touch.Custom_Forms.KNHAdultIEvaluationForm fr = (KNHAdultIEvaluationForm)Page.LoadControl("Custom Forms/KNHAdultIEvaluationForm.ascx");
            Touch.Custom_Controls.TheModal fr = (Touch.Custom_Controls.TheModal)Page.LoadControl("Custom Controls/TheModal.ascx");
            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            foreach (Control item in phForms.Controls)
            {
                phForms.Controls.Remove(item);
            }
            Touch.Custom_Controls.TheModal theFrm = (Touch.Custom_Controls.TheModal)phForms.FindControl("ID" + Session["CurrentFormName"].ToString());
            //KNHAdultIEvaluationForm theFrm = (KNHAdultIEvaluationForm)phForms.FindControl("ID" + Session["CurrentFormName"].ToString());
            if (theFrm != null)
            {
                theFrm.Visible = true;
            }
            else
            {
                phForms.Controls.Add(fr);
            }
            hdSaveBtnVal.Value = fr.ID + "_btnSave";
            updtPatientSave.Update();
            updtForms.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);

        }

        private void LoadFC()
        {
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "clearFormMode", "$('#FormMode').val('Loaded');", true);
            ClearFVState();
            UserControl fr = (UserControl)Page.LoadControl("Custom Forms/" + Session["CurrentFormName"].ToString() + ".ascx");
            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            phForms.Controls.Add(fr);
            updtForms.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);
        }
        protected void LoadFormControl()
        {
            try
            {
                LoadFC();
            }
            catch (Exception e)
            {
                IErrorLogging ErrManager = (IErrorLogging)ObjectFactory.CreateInstance("BusinessProcess.Administration.BErrorLogging, BusinessProcess.Administration");
                ErrManager.LogError("Patient Home Page", e.Message, ErrorType.Error);
                //sError = true;
                //Response.Redirect("~/Touch/frmTouchPatientHome.aspx");
            }
            finally
            {
                if (IsError)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenFormFail", "alert('An errror occured and has been logged. Please try again')", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "clsLoadingPanelDueToError", "parent.CloseLoading();", true);
                }
                IsError = false;
            }

        }

        #endregion

        protected void btnStatusActivate_Click(object sender, EventArgs e)
        {
            try
            {
                IPatientHome ReactivatePtnMgr;
                ReactivatePtnMgr = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
                System.Data.DataSet theDS1 = ReactivatePtnMgr.ReActivateTouchExceptionPatient(Convert.ToInt32(Session["PatientId"]), Convert.ToInt32(Session["TechnicalAreaId"]), true);
                Session["HIVPatientStatus"] = 0;
                lblStatus.Text = "Active";
                btnStatusActivate.Visible = false;
                updtStatusActive.Update();
                updtForms.Update();
                //string Url = string.Format("{0}?PatientId={1}", "../ClinicalForms/frmPatient_Home.aspx", Session["PatientId"].ToString());
                //Response.Redirect(Url);
                //Server.Transfer(Url);

            }
            catch (Exception err)
            {
                Application.Common.MsgBuilder theBuilder = new Application.Common.MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }
        }
        public static int GetPatientAge(DateTime birthdate, DateTime AgeOnThisDate)
        {
            int age = AgeOnThisDate.Year - birthdate.Year;
            if (AgeOnThisDate.Month < birthdate.Month || (AgeOnThisDate.Month == birthdate.Month && AgeOnThisDate.Day < birthdate.Day))
                age--;
            if (age < 0)
                return 0;
            else
                return age;
        }
        [WebMethod]
        public static string GetMessageFromWebPage(string formatdate)
        {
            return frmVisitTouch.GetDuplicateRecord(formatdate);
        }

        protected void btnInvisibleUpdateLabHistory_Click(object sender, EventArgs e)
        {
            IPatientHome PatientManager;
            PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
            rgvlabhistory.DataSource = PatientManager.GetPatientLabHistory(Convert.ToInt32(Request.QueryString["PatientID"])).Tables[0];
            rgvlabhistory.Rebind();
        }

    }
}
