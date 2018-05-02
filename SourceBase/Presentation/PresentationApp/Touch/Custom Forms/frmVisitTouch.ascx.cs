using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Interface.Clinical;
using Application.Presentation;
using Telerik.Web.UI;
using Interface.Administration;
using Application.Common;

namespace Touch.Custom_Forms
{
    public partial class frmVisitTouch : TouchUserControlBase
    {
        #region Local vars
        string ObjFactoryParameter = "BusinessProcess.Clinical.BIQTouchVisit, BusinessProcess.Clinical";
        BindFunctions theBind = new BindFunctions();
        static objVisit TV = new objVisit();
        static bool IsError = false;
        //static string _patientID = "0";
        static string _LocationID = "0";
        static string _UserID = "0";
        static string _VisitID = string.Empty;
        static DataTable dtPFindings;
        static DataTable dtTBDrugSens;
        static DataTable dtNewSens;
        static string _CurrentTab = "0";
        static string _CurrentAnchor = "";
        static DataTable dtAE;
        static string PatientId = "0";
        static string VDate = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CurrentForm"] = "frmVisitTouch";
            Session["FormIsLoaded"] = true;
            String script = frmVisit_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            if (Session["IsFirstLoad"] != null)
            {
                if (Session["IsFirstLoad"].ToString() == "true")
                {
                    Session["IsFirstLoad"] = "false";
                    //_patientID = Request.QueryString["patientId"].ToString();

                    

                    _LocationID = Session["AppLocationId"].ToString();
                    _UserID = Session["AppUserId"].ToString();
                    Init_Form();

                }
            }

            //bindSelectGrids();

            base.Page_Load(sender, e);

            /***************** Check For User Rights ****************/
            AuthenticationManager Authentication = new AuthenticationManager();

            btnPrint.Visible = Authentication.HasFunctionRight(ApplicationAccess.PASDPInitialandFollowup, FunctionAccess.Print, (DataTable)Session["UserRight"]);

            if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPInitialandFollowup, FunctionAccess.Update, (DataTable)Session["UserRight"]))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showsave", "$('#divSave').hide();", true);
            }

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "loading", TouchGlobals.OnScKeyboard, true);
        }
        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            updtFormUpdate.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "printScript", "PrintTouchForm('Visit Form', '" + this.ID + "');", true);
        }
        private void Init_Form()
        {
            PatientId = Request.QueryString["patientId"].ToString();
            //first set ART date diff
            DataTable dtDiffart = GetDataFromQuery("SELECT [dbo].[fn_GetPatientARTStartDate] (" + Convert.ToInt32(Request.QueryString["patientId"].ToString()) + ")" );
            DateTime dtmeart = _CheckDate(dtDiffart.Rows[0][0].ToString());
            int regdateart = ((DateTime.Now.Year - dtmeart.Year) * 12) + DateTime.Now.Month - dtmeart.Month;
            if (regdateart > 216)
                txtDurationStartART.Text = "0";
            else
                txtDurationStartART.Text = regdateart.ToString();
            //set Regiment date diff
            //check data diff
            DataTable dtDiff = GetDataFromQuery("select a.DispensedByDate from ord_PatientPharmacyOrder a inner join dtl_PatientPharmacyOrder b on a.ptn_pharmacy_pk=b.ptn_pharmacy_pk where a.DispensedByDate is not null and a.ptn_pk = " + Convert.ToInt32(Request.QueryString["patientId"].ToString()) + " and dbo.fn_GetDrugTypeId_futures(b.Drug_Pk) = 37 order by a.DispensedByDate desc");
            //select DispensedByDate from ord_PatientPharmacyOrder where DispensedByDate is not null and ptn_pk = " + Convert.ToInt32(Request.QueryString["patientId"].ToString()) + " order by DispensedByDate desc");
            if (dtDiff.Rows.Count > 0)
            {
                DateTime dtme = _CheckDate(dtDiff.Rows[0][0].ToString());
                int regdate = ((DateTime.Now.Year - dtme.Year) * 12) + DateTime.Now.Month - dtme.Month;
                if (regdate > 216)
                    txtDurationCurrentReg.Text = "0";
                else
                    txtDurationCurrentReg.Text = regdate.ToString();
            }
            //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "loadingkeyboard", TouchGlobals.OnScKeyboard, true);
            SetTBDrugSensitivity();

            //set VisitType List
            theBind.BindCombo(cbVisitType, GetBindTables("1003"), "Name", "ID");

            //set Present List
            theBind.BindCombo(rcbPresent, GetBindTables("1009"), "Name", "ID");

            //set Hospital List
            DataTable DT = GetDataFromQuery("Select Distinct FacilityName as Name, FacilityId as Id from mst_facility where deleteflag = 0 and systemid = 3 order by FacilityName Asc");
            theBind.BindCombo(rcbWhereHosp, DT, "Name", "ID");

            //set Developmental Status List
            theBind.BindCombo(cbDevScreen, GetBindTables("1010"), "Name", "ID");

            //set Tanner Stage List
            theBind.BindCombo(cbTannerStage, GetBindTables("1011"), "Name", "ID");

            //set Family planning method
            DataTable DTFam = GetDataFromQuery("select * from mst_pmtctDeCode where codeid = 6 and systemid = 3");
            theBind.BindCombo(cbOtherFamilyPlanning, DTFam, "Name", "ID");

            //set Physical findings
            DataTable DT1 = GetDataFromQuery("Select distinct ID, Name as PName, 0 as YN from mst_Decode WHERE deleteflag=0 and systemID = 3 and CodeId=1122");
            rgdPhysicalFindings.DataSource = DT1;
            dtPFindings = DT1;
            rgdPhysicalFindings.DataBind();


            // set TB Treatment
            DataTable dtTBtreatment = GetDataFromQuery("select ID, Name from mst_deCode where codeid=(select codeid from mst_code where name='Forms of treatment') and deleteflag = 0 ");
            BindCombo(cbContactTreatement, GetInvisiblateClone(dtTBtreatment, "ShowIfTreatmentOther"), "Name", "ID");

            //set adverse events and create static DT for gridview use
            SetAdverseEvents();
            dtAE = AEDets();

            //set referred to
            DataTable dtRefferedTo = GetDataFromQuery("select ID, Name from mst_deCode where codeid=1005 and deleteflag = 0 and SRNo > 0");
            BindCombo(cbReferredTo, GetInvisiblateClone(dtRefferedTo, "ShowIfReferredToOther"), "Name", "ID");

            //set WHO
            string GetWHO = "select ID, Name from mst_decode where codeid = 22 and UpdateFlag = 0 order by Name";
            theBind.BindCombo(cbClinicalStage, GetDataFromQuery(GetWHO), "Name", "ID");

            //set Nutritional problems
            DataTable DTnp = GetDataFromQuery("select ID, Name from mst_nutritionalProblem where systemid = 3");
            theBind.BindCombo(cbNutionalProblems, DTnp, "Name", "ID");

            //create the Nutrional DD according to business rules
            /// If less than 2 yrs display only TF
            /// If female and >= 9 years display IFC AND FS
            /// All others display FS only
            DataTable dtNutrition = GetDataFromQuery("select * from mst_ModDeCode where systemid = 3 and codeid = 9");
            Label lblAGE = (Label)Parent.FindControl("lblAge");
            Label lblSEX = (Label)Parent.FindControl("lblSex");
            DataTable dtNutrRules = dtNutrition.Clone();
            int theAge = 0;
            if ((lblAGE != null) && (lblSEX != null))
            {
                string theSex = lblSEX.Text;
                theAge = _CheckInt(lblAGE.Text);

                if (theAge < 2)
                {
                    DataRow[] DR = dtNutrition.Select("Name = 'Therapeutic Feeding'");
                    foreach (DataRow row in DR)
                    {
                        dtNutrRules.ImportRow(row);
                    }
                }
                else if ((theSex == "Female") && (theAge >= 9))
                {
                    DataRow[] DR = dtNutrition.Select("Name = 'Infant Feeding Counselling' OR Name = 'Food Support'");
                    foreach (DataRow row in DR)
                    {
                        dtNutrRules.ImportRow(row);
                    }
                }
                else
                {
                    DataRow[] DR = dtNutrition.Select("Name = 'Food Support' OR Name = 'Nutrition Counselling only'");
                    foreach (DataRow row in DR)
                    {
                        dtNutrRules.ImportRow(row);
                    }
                }
            }
            theBind.BindCombo(cbNurtionalSupport, dtNutrRules, "Name", "ID");

            //set Feeding options
            if (theAge < 2)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showfeedingpractice", "ShowHide('IDfrmVisitTouch_divFeedingPractice', 'show', 'divFeedingPractice')", true);
                //divFeedingPractice.Visible = true;
                DataTable DTfo = GetDataFromQuery("select ID, Name from mst_pmtctdecode where systemid = 3 and codeid=4 and SRNo = 1");
                theBind.BindCombo(cbFeedingPractice, DTfo, "Name", "ID");
            }

            //set Caregiver info (if any)

            //first check last visit
            IIQTouchPatientRegistration getCGdetsMng1 = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");
            DataTable DTCG = (DataTable)getCGdetsMng1.GetCareGiverInfoFromVisit(Request.QueryString["patientId"].ToString());
            if (DTCG.Rows.Count > 0)
            {
                if (DTCG.Rows[0]["SupporterName"].ToString() != "")
                {
                    txtCaregiver.Text = DTCG.Rows[0]["SupporterName"].ToString();
                    txtCareGiverContactNo.Text = DTCG.Rows[0]["TreatmentSupporterContact"].ToString();
                }
            }
            else
            {

                IIQTouchPatientRegistration getCGdetsMng2 = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");
                DataTable dtCG = (DataTable)getCGdetsMng2.GetCareGiverInfo(Request.QueryString["patientId"].ToString());
                if (dtCG.Rows.Count > 0)
                {
                    if (dtCG.Rows[0]["GuardianName"].ToString() != "")
                    {
                        txtCaregiver.Text = dtCG.Rows[0]["GuardianName"].ToString().Replace("|", " ");
                        txtCareGiverContactNo.Text = dtCG.Rows[0]["EmergContactPhone"].ToString();
                    }
                }
            }

            //set Patient TB Status
            DataTable DTTBStat = GetDataFromQuery("Select ID, Name from mst_pmtctdecode where codeid=32 and systemid= 3");
            theBind.BindCombo(cbPtnTBStatus, DTTBStat, "Name", "ID");
            

            //set CTX and ARV ahderenceto
            theBind.BindCombo(cbCTXAdherence, GetBindTables("1134"), "Name", "ID");
            theBind.BindCombo(cbARVAdherence, GetBindTables("1135"), "Name", "ID");

            //set arv reason why list
            string GetARV = "select ID, Name from mst_Reason where systemid = 3 AND deleteflag = 0 and CategoryID = 5";
            theBind.BindCombo(cbARVWhyReason, GetDataFromQuery(GetARV), "Name", "ID");
            
            //set diagnosis made
            cbTBPtnDiagMade.DataSource = GetBindTables("1013");
            cbTBPtnDiagMade.DataValueField = "ID";
            cbTBPtnDiagMade.DataTextField = "Name";
            cbTBPtnDiagMade.DataBind();
            //theBind.BindCombo(cbTBPtnDiagMade, GetBindTables("1013"), "Name", "ID");

            //set patients TB treatment 
            theBind.BindCombo(cbPtnTBTreatment, GetBindTables("1012"), "Name", "ID");

            //set change regimen reasons
            DataTable DTChangeregreasons = GetDataFromQuery("select * from mst_reason where  categoryID=8 and systemID = 3");
            theBind.BindCombo(rcbARVChangeReason, DTChangeregreasons, "Name", "ID");
            //set stop regimen reasons
            DataTable DTstopregreasons = GetDataFromQuery("select * from mst_reason where  categoryID=7 and systemID = 3");
            theBind.BindCombo(cbARVStopReason, DTstopregreasons, "Name", "ID");

            //set disclosure to child list
            theBind.BindCombo(rcbDisclosedLvl, GetBindTables("1015"), "Name", "ID");

            //Substitutions and Interruptions
            theBind.BindCombo(cbARVSubstitutions, GetBindTables("1137"), "Name", "ID");


            //check if in EditMode
            if (Session["VisitEditMode"] != null)
            {
                if (Session["VisitEditMode"].ToString() == "true")
                {
                    SetFormVals();
                    btnPrint.Visible = true;
                    //Check if status is active
                    Label labelStatus = (Label)Parent.FindControl("lblStatus");
                    //if (labelStatus.Text == "InActive")
                    //    DisableControls(updtFormUpdate, false);
                    //else
                    //    DisableControls(updtFormUpdate, true);
                    //updtFormUpdate.Update();
                    bindSelectGrids();
                }
            }
            else
            {
                Session["VisitEditMode"] = "false";
            }
        }
        protected void SetFormVals()
        {
            _VisitID = Session["VisitID"].ToString();
            IIQTouchVisit ptnMgr = (IIQTouchVisit)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchVisit, BusinessProcess.Clinical");
            DataSet ds = (DataSet)ptnMgr.GetVisitDetails(Request.QueryString["patientId"].ToString(), _LocationID, _UserID, _VisitID);

            if (ds.Tables.Count > 0)
            {
                //let's get started
                //set tables 
                DataTable dtVC = ds.Tables[0];   // Visit and caregiver info Table
                DataTable dtHA = ds.Tables[1];   // Hospital admission table
                DataTable dtCS = ds.Tables[2];   // Clinical Status table
                DataTable dtDS1 = ds.Tables[3];  // Developmental Status 1 table
                DataTable dtDS2 = ds.Tables[4];  // Developmental Status 2 table
                DataTable dtTB = ds.Tables[5];   // TB Contact table
                DataTable dtTBS1 = ds.Tables[6]; // TB Patient Screening 1 table
                DataTable dtTBS2 = ds.Tables[7]; // TB Patient Screening 2 table
                DataTable dtTBS3 = ds.Tables[8]; // TB Patient Screening 3 table
                DataTable dtTBS4 = ds.Tables[9]; // TB Patient Screening 4 table
                DataTable dtCF = ds.Tables[10];  // Clinical Findings table
                DataTable dtPF = ds.Tables[11];  // Physical Findings table
                DataTable dtTBC = ds.Tables[12]; // TB contact
                DataTable dtTBCT = ds.Tables[13];// TB contact Treatment
                DataTable dtSIT = ds.Tables[14];  // Substitutions and Interruptions table
                DataTable dtNut = ds.Tables[15]; // Nutrition table
                DataTable dtADH = ds.Tables[16]; // Adherence table
                DataTable dtADHR = ds.Tables[17]; // Adherence reasons table
                DataTable dtSIR = ds.Tables[18]; // Sub and Interrupts reasons table
                DataTable dtSI = ds.Tables[19]; // Sub and Interrupts table
                DataTable dtARTEnd = ds.Tables[20]; //ART End date
                DataTable dtdtc = ds.Tables[21]; //Disclosure to Child
                DataTable dtReft = ds.Tables[22]; //reffered to
                DataTable dttransf = ds.Tables[23]; //transfer out
                DataTable dtnextapp = ds.Tables[24]; //next appointment
                dtAE.Rows.Clear();
                DataTable dtAEdets = ds.Tables[25];  // Adverse Event
                DataTable dtTBDiagnosis = ds.Tables[26]; //[dtl_TBDiagnosis] 

                //DataTable dtDI = ds.Tables[14]; // Disclosure table
                //DataTable dtRF = ds.Tables[15]; // Referral table
                //DataTable dtNA = ds.Tables[16]; // Next Appointment table
                //DataTable dtAEget = ds.Tables[17]; // AdverseEvents



                if (dtVC.Rows.Count > 0)
                {
                    //### Visit and caregiver info ###
                    if (_CheckDate(dtVC.Rows[0]["VisitDate"].ToString()).Year != 1900)
                        dtVisitDate.SelectedDate = _CheckDate(dtVC.Rows[0]["VisitDate"].ToString());
                    VDate = String.Format("{0:dd-MMM-yyyy}", dtVC.Rows[0]["VisitDate"]).Replace('-', ' ');
                    int cbIndex = cbVisitType.FindItemIndexByValue(_CheckInt(dtVC.Rows[0]["IQTouchVisitType"].ToString()).ToString());
                    cbVisitType.SelectedIndex = cbIndex;

                    if (_CheckBool(_CheckInt(dtVC.Rows[0]["Scheduled"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtVC.Rows[0]["Scheduled"].ToString()).ToString()) == true)
                            btnScheduledYes.Checked = true;
                        else
                            btnScheduledNo.Checked = true;
                        rcbPresent.Items.FindItemByValue(_CheckInt(dtVC.Rows[0]["PresentID"].ToString()).ToString()).Selected = true;
                    }
                }

                if (dtHA.Rows.Count > 0)
                {
                    //### Hospital Admission
                    if (_CheckBool(_CheckInt(dtHA.Rows[0]["Admittedtohospital"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtHA.Rows[0]["Admittedtohospital"].ToString()).ToString()) == true)
                        {
                            chkHospYes.Checked = true;
                            ShowCheckedYes("hideHospitalYN");
                            txtNumDayHosp.Text = dtHA.Rows[0]["HospitalizedNumberofdays"].ToString();
                            rcbWhereHosp.SelectedValue = _CheckInt(dtHA.Rows[0]["HospitalName"].ToString()).ToString();
                            txtDischargeDiagnosis.Text = dtHA.Rows[0]["Dischargediagnosis"].ToString();
                            txtDischargeNote.Text = dtHA.Rows[0]["Dischargenote"].ToString();
                        }
                        else if (_CheckBool(_CheckInt(dtHA.Rows[0]["Admittedtohospital"].ToString()).ToString()) == false)
                        {
                            chkHospNo.Checked = true;
                        }
                    }
                }

                if (dtCS.Rows.Count > 0)
                {

                    txtTemp.Text = _CheckDecimal(dtCS.Rows[0]["Temp"].ToString()) > 0 ? dtCS.Rows[0]["Temp"].ToString() : "";
                    txtWeight.Text = _CheckDecimal(dtCS.Rows[0]["Weight"].ToString()) > 0 ? dtCS.Rows[0]["Weight"].ToString() : "";
                    txtHeight.Text = _CheckDecimal(dtCS.Rows[0]["Height"].ToString()) > 0 ? dtCS.Rows[0]["Height"].ToString() : "";
                    txtRespRate.Text = _CheckDecimal(dtCS.Rows[0]["RR"].ToString()) > 0 ? dtCS.Rows[0]["RR"].ToString() : "";
                    txtPulse.Text = _CheckDecimal(dtCS.Rows[0]["HR"].ToString()) > 0 ? dtCS.Rows[0]["HR"].ToString() : "";
                    txtSystolic.Text = _CheckDecimal(dtCS.Rows[0]["BPSystolic"].ToString()) > 0 ? dtCS.Rows[0]["BPSystolic"].ToString() : "";
                    txtDiastolic.Text = _CheckDecimal(dtCS.Rows[0]["BPDiastolic"].ToString()) > 0 ? dtCS.Rows[0]["BPDiastolic"].ToString() : "";
                    txtHeadCirc.Text = _CheckDecimal(dtCS.Rows[0]["Headcircumference"].ToString()) > 0 ? dtCS.Rows[0]["Headcircumference"].ToString() : "";
                    txtMUAC.Text = _CheckInt(dtCS.Rows[0]["MUAC"].ToString()) > 0 ? dtCS.Rows[0]["MUAC"].ToString() : "";

                    GetBMI();
                }

                if (dtDS1.Rows.Count > 0)
                {
                    cbDevScreen.SelectedValue = _CheckInt(dtDS1.Rows[0]["DevelopmentalScreening"].ToString()).ToString();
                    cbTannerStage.SelectedValue = _CheckInt(dtDS1.Rows[0]["TannerStage"].ToString()).ToString();

                    if (_CheckBool(_CheckInt(dtDS1.Rows[0]["SexuallyActive"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtDS1.Rows[0]["SexuallyActive"].ToString()).ToString()) == true)
                            btnSexActiveYes.Checked = true;
                        else
                            btnSexActiveNo.Checked = true;
                    }

                    if (_CheckBool(_CheckInt(dtDS1.Rows[0]["Protectedsex"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtDS1.Rows[0]["Protectedsex"].ToString()).ToString()) == true)
                            btncondomsYes.Checked = true;
                        else
                            btncondomsNo.Checked = true;
                    }
                }
                #region min
                if (dtDS2.Rows.Count > 0)
                {
                    if (_CheckBool(_CheckInt(dtDS2.Rows[0]["Pregnancystatus"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtDS2.Rows[0]["Pregnancystatus"].ToString()).ToString()) == true)
                            btnPregnantYes.Checked = true;
                        else
                            btnPregnantNo.Checked = true;
                    }
                    cbOtherFamilyPlanning.SelectedValue = _CheckInt(dtDS2.Rows[0]["familyplanningmethod"].ToString()).ToString();
                    txtFamilyPlanningOther.Text = dtDS2.Rows[0]["otherfpmethods"].ToString();
                }

                if (dtTB.Rows.Count > 0)
                {
                    if (_CheckBool(_CheckInt(dtTB.Rows[0]["NewTBContact"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtTB.Rows[0]["NewTBContact"].ToString()).ToString()) == true)
                            btnTBContactYes.Checked = true;
                        else
                            btnTBContactNo.Checked = true;
                    }
                    if (_CheckBool(_CheckInt(dtTB.Rows[0]["ContactSensitiveTB"].ToString()).ToString()).HasValue)
                    {
                        if (dtTB.Rows[0]["ContactSensitiveTB"] != System.DBNull.Value)
                        {
                            if (_CheckBool(_CheckInt(dtTB.Rows[0]["ContactSensitiveTB"].ToString()).ToString()) == true)
                                btnKnownSensitivityYes.Checked = true;
                            else
                                btnKnownSensitivityNo.Checked = true;
                        }
                    }
                    if (_CheckBool(_CheckInt(dtTB.Rows[0]["ContactTBTreatment"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtTB.Rows[0]["ContactTBTreatment"].ToString()).ToString()) == true)
                            btnConRecTreatmentYes.Checked = true;
                        else
                            btnConRecTreatmentNo.Checked = true;
                    }
                    if (_CheckBool(_CheckInt(dtTB.Rows[0]["ContactDailyInjection"].ToString()).ToString()).HasValue)
                    {
                        if (_CheckBool(_CheckInt(dtTB.Rows[0]["ContactDailyInjection"].ToString()).ToString()) == true)
                            btnInjectionsYes.Checked = true;
                        else
                            btnInjectionsNo.Checked = true;
                    }

                    bool bFoundContactTreatment = false;
                    foreach (RadComboBoxItem item in cbContactTreatement.Items)
                    {
                        if (!bFoundContactTreatment && item.Value.Contains(dtTB.Rows[0]["ContactTBTreatmentRcvd"].ToString()))
                        {
                            bFoundContactTreatment = true;
                            cbContactTreatement.SelectedValue = item.Value;
                        }
                    }

                    if (cbContactTreatement.SelectedItem.Text.Contains("Other"))
                        txtTBContactOtherProph.Text = dtTB.Rows[0]["ContactOtherTBProphylaxis"].ToString();
                }


                if (dtTBS1.Rows.Count > 0)
                {
                    foreach (DataRow item in dtTBS1.Rows)
                    {
                        cbPtnTBStatus.SelectedValue = item["TBStatus"].ToString();
                        if(_CheckDate(item["TBRxStartDate"].ToString()).Year != 1900)
                            dtPtnRxStartDate.SelectedDate = _CheckDate(item["TBRxStartDate"].ToString());

                        if (item["StillTreatement"].ToString() == "1")
                            rbtnOnTreatmentYes.Checked = true;

                        if (item["StillTreatement"].ToString() == "0")
                            rbtnOnTreatmentNo.Checked = true;

                        if(_CheckDate(item["TBRxEndDate"].ToString()).Year !=1900)
                            dtPtnRxEndDate.SelectedDate = _CheckDate(item["TBRxEndDate"].ToString());

                        if (item["NewSensitiveInformation"].ToString() == "1")
                            btnPtnNewSensitivityYes.Checked = true;

                        if (item["NewSensitiveInformation"].ToString() == "0")
                            btnPtnNewSensitivityNo.Checked = true;

                        cbPtnTBTreatment.SelectedValue = item["PatientTBTreatmentRcvd"].ToString();

                        if (cbPtnTBTreatment.SelectedItem.Text.Contains("Other"))
                            txtPtnOtherProph.Text = item["PatientOtherTBProphylaxis"].ToString();

                        if (item["AdverseEventYN"].ToString() == "0")
                            btnAENo.Checked = true;
                        if (item["AdverseEventYN"].ToString() == "1")
                            btnAEYes.Checked = true;

                    }
                }

                dtTBDrugSens.PrimaryKey = new DataColumn[] { dtTBDrugSens.Columns["ID"] };
                dtNewSens.PrimaryKey = new DataColumn[] { dtNewSens.Columns["ID"] };

                if (dtTBS4.Rows.Count > 0)
                {
                    DataTable ClonedDB = dtTBDrugSens.Clone();
                    DataTable ClonedDBNewSens = dtNewSens.Clone();
                    
                    foreach (DataRow item in dtTBS4.Rows)
                    {
                        if (item["IsPatient"].ToString() == "0")
                        {
                            object[] findVals = new object[1];
                            findVals[0] = item["Drug"].ToString();

                            DataRow theDR = dtTBDrugSens.Rows.Find(findVals);

                            if (item["Sensitivity"].ToString() == "1")
                                theDR["Type"] = "Sensitive";
                            if (item["Resistance"].ToString() == "1")
                                theDR["Type"] = "Resistant";

                            ClonedDB.ImportRow(theDR);


                            foreach (GridDataItem grItem in rgdTBDrugsSensitivity.Items)
                            {
                                RadComboBox YNVal = (RadComboBox)grItem.FindControl("rcbResSen");
                                if (YNVal != null)
                                {
                                    if (grItem["ID"].Text == item["Drug"].ToString())
                                    {
                                        if (item["Sensitivity"].ToString() == "1")
                                            YNVal.SelectedIndex = 1;
                                        if (item["Resistance"].ToString() == "1")
                                            YNVal.SelectedIndex = 2;

                                    }
                                }
                            }
                        }
                        else if (item["IsPatient"].ToString() == "1")
                        {
                            object[] findVals = new object[1];
                            findVals[0] = item["Drug"].ToString();

                            DataRow theDR = dtNewSens.Rows.Find(findVals);

                            if (item["Sensitivity"].ToString() == "1")
                                theDR["Type"] = "Sensitive";
                            if (item["Resistance"].ToString() == "1")
                                theDR["Type"] = "Resistant";

                            ClonedDBNewSens.ImportRow(theDR);


                            foreach (GridDataItem grItem in rgdNewSensitivity.Items)
                            {
                                RadComboBox YNVal = (RadComboBox)grItem.FindControl("rcbNewResSens");
                                if (YNVal != null)
                                {
                                    if (grItem["ID"].Text == item["Drug"].ToString())
                                    {
                                        if (item["Sensitivity"].ToString() == "1")
                                            YNVal.SelectedIndex = 1;
                                        if (item["Resistance"].ToString() == "1")
                                            YNVal.SelectedIndex = 2;

                                    }
                                }
                            }
                            
                        }

                    }

                    if (ClonedDB.Rows.Count > 0)
                    {

                        rgTBStatus.DataSource = ClonedDB;
                        rgTBStatus.DataBind();
                        updtTBStatus.Update();
                        updtTBSens.Update();
                    }

                    if (ClonedDBNewSens.Rows.Count > 0)
                    {
                        rgNewSens.DataSource = ClonedDBNewSens;
                        rgNewSens.DataBind();
                        uptSensNew.Update();
                        uptSensNew.Update();
                    }
                    if (rgTBStatus.Items.Count > 0) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showtbstatussensresdiv", "$('#divTBStatus').show();ShowHide('HideTBSContactSensitiveYN', 'show', 'rgTBStatus');focusAndLayout('rgTBStatus');", true);
                    if (rgNewSens.Items.Count > 0) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showtnewsensdivs", "$('#divSensNew').show();ShowHide('indicateSenseYN', 'show', 'rgNewSens');focusAndLayout('rgNewSens');", true);
                }

                if (dtTBCT.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTBCT.Rows.Count; i++)
                    {

                        foreach (RadComboBoxItem item in rcbTreatment.Items)
                        {
                            if (item.Value == dtTBCT.Rows[i][0].ToString())
                            item.Checked = true;
                        }
                    }
                }

                if (dtCF.Rows.Count > 0)
                {
                    cbClinicalStage.SelectedValue = dtCF.Rows[0]["WHOStage"].ToString();
                    txtClinicalNotes.Text = dtCF.Rows[0]["ClinicalNotes"].ToString();
                }

                // Handle Physical Findings
                if (dtPF.Rows.Count > 0)
                {
                    rgFindings.DataSource = dtPF;
                    rgFindings.DataBind();
                    foreach (DataRow item in dtPF.Rows)
                    {
                        foreach (GridDataItem gi in rgdPhysicalFindings.Items)
                        {
                            RadButton YNVal = (RadButton)gi.FindControl("btnYN");
                            if (gi["ID"].Text == item["ID"].ToString())
                            {
                                YNVal.SelectedToggleStateIndex = 1;
                            }
                        }

                        if (item["Pname"].ToString() == "Other (specify)")
                        {
                            txtOtherFindings.Text = item["Name"].ToString();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showfindingsotherdiv", "$('#divFindingsother').show();", true);
                        }
                    }
                    uptPFindings.Update();
                    if (rgFindings.Items.Count > 0) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showFindingsdivOnPageLoad", "$('#divFindings').show();focusAndLayout('rgFindings');", true);
                }

                if (dtNut.Rows.Count > 0)
                {
                    foreach (DataRow item in dtNut.Rows)
                    {
                        cbFeedingPractice.SelectedValue = item["FeedingOption"].ToString();
                        cbNurtionalSupport.SelectedValue = item["NutritionalSupport"].ToString();
                        cbNutionalProblems.SelectedValue = item["NutritionalProblem"].ToString();
                    }
                }

                if (dtADH.Rows.Count > 0)
                {
                    foreach (DataRow item in dtADH.Rows)
                    {
                        if (item["CorrectlyDispensed"].ToString() == "1")
                            btnDispensedYes.Checked = true;
                        else if (item["CorrectlyDispensed"].ToString() == "0")
                        {
                            btnDispensedNo.Checked = true;
                            txtPharmacyNotes.Text = item["NotDispensedNote"].ToString();
                        }
                        cbCTXAdherence.SelectedValue = item["CotrimoxazoleAdhere"].ToString();
                        cbARVAdherence.SelectedValue = item["ARVAdhere"].ToString();
                    }
                }

                if (dtADHR.Rows.Count > 0)
                {
                    foreach (DataRow item in dtADHR.Rows)
                    {
                        foreach (RadComboBoxItem itm in cbARVWhyReason.Items)
                        {
                            if (itm.Value == item["MissedReasonID"].ToString())
                            {
                                itm.Checked = true;
                                if (itm.Text.Contains("Other"))
                                    txtARVWhyOther.Text = item["Other_Desc"].ToString();
                            }
                        }
                    }
                }

                if (dtSI.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSI.Rows) {

                        cbARVSubstitutions.SelectedValue = dr["TherapyPlan"].ToString();

                        if (cbARVSubstitutions.SelectedItem.Text == "Change regimen")
                        {
                            string[] theReasons = dtSIR.Rows[0]["TherapyReasons"].ToString().Split('|');
                            foreach(string sdr in theReasons) {
                                foreach (RadComboBoxItem ri in rcbARVChangeReason.Items)
                                {
                                    if (ri.Value == sdr)
                                    {
                                        ri.Checked = true;
                                        if (ri.Text.Contains("Other"))
                                            txtARVChangeOther.Text = dtSI.Rows[0]["TherapyOther"].ToString();
                                    }
                                }
                            }

                        }
                        else if (cbARVSubstitutions.SelectedItem.Text == "Stop treatment")
                        {
                            string[] theReasons = dtSIR.Rows[0]["TherapyReasons"].ToString().Split('|');
                            foreach (string sdr in theReasons)
                            {
                                foreach (RadComboBoxItem ri in cbARVStopReason.Items)
                                {
                                    if (ri.Value == sdr)
                                    {
                                        ri.Checked = true;
                                        if (ri.Text.Contains("Other"))
                                            txtStopReasonOther.Text = dtSI.Rows[0]["TherapyOther"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                if (dtARTEnd.Rows.Count > 0)
                {
                    if ((_CheckDate(dtARTEnd.Rows[0]["ARTenddate"].ToString()).Year != 1900) && (_CheckDate(dtARTEnd.Rows[0]["ARTenddate"].ToString()).Year > 1900))
                    {
                        dtARVEndDate.SelectedDate = (_CheckDate(dtARTEnd.Rows[0]["ARTenddate"].ToString()));
                    }
                }


                if (dtdtc.Rows.Count > 0)
                {
                    if (dtdtc.Rows[0]["DisclosureChild"].ToString() == "1")
                    {
                        btnDisclosedYes.Checked = true;
                        rcbDisclosedLvl.SelectedValue = dtdtc.Rows[0]["DisclosureID"].ToString();
                    }
                    else if (dtdtc.Rows[0]["DisclosureChild"].ToString() == "0")
                    {
                        btnDisclosedNo.Checked = true;
                    }
                }

                if (dtReft.Rows.Count > 0)
                {
                    foreach (RadComboBoxItem item in cbReferredTo.Items)
                    {
                        string[] refValID = dtReft.Rows[0]["PatientRefID"].ToString().Split('|');
                        if (refValID.Length > 0)
                        {
                            if (item.Value.ToString().Split('|')[0].ToString() == refValID[0].ToString())
                            {
                                item.Selected = true;
                            }
                        }
                        
                    }
                    //cbReferredTo.SelectedValue = dtReft.Rows[0]["PatientRefID"].ToString();
                    if (cbReferredTo.SelectedItem.Text.Contains("Other"))
                        txtReferredOther.Text = dtReft.Rows[0]["PatientRefDesc"].ToString();
                }

                if (dttransf.Rows.Count > 0)
                {
                    if (dttransf.Rows[0]["patientexitreason"].ToString() == "1")
                    {
                        btnTransOutYes.Checked = true;
                    }
                    else if (dttransf.Rows[0]["patientexitreason"].ToString() == "0")
                        btnTransOutNo.Checked = true;
                    
                }

                if (dtnextapp.Rows.Count > 0)
                {
                    if (_CheckDate(dtnextapp.Rows[0]["appdate"].ToString()).Year != 1900)
                    {
                        dtNextAppointment.SelectedDate = _CheckDate(dtnextapp.Rows[0]["appdate"].ToString());
                    }
                }

                if (dtAEdets.Rows.Count > 0)
                {
                    btnAEYes.Checked = true;
                    Session["AEFirstLoad"] = true;
                    foreach (DataRow dr in dtAEdets.Rows)
                    {
                        DataRow r = dtAE.NewRow();
                        r["EventCatID"] = dr["EventCatID"];
                        r["EventCatName"] = dr["EventCatName"];
                        r["EventID"] = dr["EventID"];
                        r["EventName"] = dr["EventName"];
                        r["SeverityID"] = dr["SeverityID"];
                        dtAE.Rows.Add(r);
                        if (dr["EventCatName"].ToString().Contains("Other"))
                            txtAdverseEventOther.Text = dr["Other"].ToString();
                        if (dr["EventDescription"].ToString() != string.Empty)
                            txtAdverseEventComment.Text = dr["EventDescription"].ToString();
                    }
                    dtAE.PrimaryKey = new DataColumn[] { dtAE.Columns["EventID"] };
                    rgAE.DataSource = dtAE;
                    rgAE.DataBind();
                    updtAE.Update();
                }
                

                //tvs.SubsInterruptions = _CheckInt(cbARVSubstitutions.SelectedValue);

                //if (cbARVSubstitutions.SelectedItem.Text == "Change regimen")
                //{
                //    List<objVisit.ChangeRegimenReason> chr = new List<objVisit.ChangeRegimenReason>();
                //    foreach (RadComboBoxItem ri in rcbARVChangeReason.Items)
                //    {
                //        if (ri.Checked)
                //        {
                //            objVisit.ChangeRegimenReason cr = new objVisit.ChangeRegimenReason();
                //            cr.ChangeReasonID = _CheckInt(ri.Value);
                //            chr.Add(cr);
                //            if (ri.Text.Contains("Other"))
                //            {
                //                tvs.ChangeReasonOther = txtARVChangeOther.Text;
                //            }
                //        }
                //    }
                //    tvs.ChangeRegimenReasons = chr;
                //}
                //else if (cbARVSubstitutions.SelectedItem.Text == "Stop treatment")
                //{
                //    List<objVisit.StopRegimenReason> chr = new List<objVisit.StopRegimenReason>();
                //    foreach (RadComboBoxItem ri in cbARVStopReason.Items)
                //    {
                //        if (ri.Checked)
                //        {
                //            objVisit.StopRegimenReason cr = new objVisit.StopRegimenReason();
                //            cr.StopReasonID = _CheckInt(ri.Value);
                //            chr.Add(cr);
                //        }
                //    }
                //    if ((_CheckDate(dtARVEndDate.SelectedDate.ToString()).Year != 1900))
                //        tvs.ARTEndDate = dtARVEndDate.SelectedDate.ToString();
                //}

                if (dtAEdets.Rows.Count > 0)
                {
                    foreach (DataRow item in dtAEdets.Rows)
                    {
                        List<RadTreeNode> alls = (List<RadTreeNode>)rtwAEvents.GetAllNodes();
                        if (alls.Count > 0)
                        {
                            foreach (RadTreeNode node in alls)
                            {
                                if (node.Value == item["EventID"].ToString())
                                {
                                    node.Checked = true;
                                }
                            }
                        }
                    }
                }

                if (dtTBDiagnosis.Rows.Count > 0)
                {
                    foreach (DataRow item in dtTBDiagnosis.Rows)
                    {
                        for (int i = 0; i < cbTBPtnDiagMade.Items.Count; i++)
                        {
                            if (cbTBPtnDiagMade.Items[i].Value == item["DiagnosisMade"].ToString())
                            {
                                RadComboBoxItem rci = cbTBPtnDiagMade.Items[i];
                                rci.Checked = true;
                                i = cbTBPtnDiagMade.Items.Count;
                            }
                        }

                    }
                }
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
                SaveVisit();
        }
                #endregion
        protected void ShowCheckedYes(string theDivToShow)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sh" + theDivToShow, "ShowHide('" + theDivToShow + "', 'show', null)", true);
        }
        protected void SetYNCombo(RadComboBox rcb, string valueToSelect, string DivToShow = null)
        {
            foreach (RadComboBoxItem item in rcb.Items)
            {
                string[] theVals = item.Value.Split('|');
                if (theVals.Length > -1)
                {
                    if (theVals[0] == valueToSelect)
                    {
                        item.Selected = true;
                        if (item.Text == "Yes")
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sh" + DivToShow, "ShowHide('" + DivToShow + "', 'show', null)", true);
                        break;
                    }
                }

            }
        }
        private void SaveVisit()
        {
            string sWhere = "";
            try
            {
                sWhere = "start";
                SetSavedVals(ref TV);
                sWhere = "SetSavedVals";
                TV.LocationID = _LocationID;
                TV.UserID = _UserID;
                
                IIQTouchVisit ptnMgr = (IIQTouchVisit)ObjectFactory.CreateInstance(ObjFactoryParameter);
                sWhere = "CreateInstance(ObjFactoryParameter";
                if (Session["VisitEditMode"].ToString() == "true")
                {
                    TV.OldVisitID = int.Parse(Session["VisitID"].ToString());
                    int theRes = (int)ptnMgr.SaveVisitDetails(TV, true);
                    sWhere = "SaveVisitDetailsVisitEditModeTrue";
                    Session["VisitEditMode"] = "false";
                }
                else
                {
                    int theRes = (int)ptnMgr.SaveVisitDetails(TV);
                    sWhere = "SaveVisitDetailsVisitEditModeFalse";
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully');$('#FormMode').val('Unload');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackWithoutDialog();", true);

                Session["JustAddedRec"] = "true";

                IsError = false;
            }
            catch (Exception e)
            {
                IErrorLogging ErrManager = (IErrorLogging)ObjectFactory.CreateInstance("BusinessProcess.Administration.BErrorLogging, BusinessProcess.Administration");
                ErrManager.LogError("Visit Page",sWhere + "-"+ e.Message, ErrorType.Error);
                IsError = true;
            }
            finally
            {
                if (IsError)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('An errror occured please contact your Administrator')", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "clsLoadingPanelDueToError", "parent.CloseLoading();", true);
                }
            }
        }
        private void SetSavedVals(ref objVisit tvs)
        {
            tvs.PatientID = int.Parse(Request.QueryString["patientId"].ToString());
            //set Clinical Status tab
            tvs.VisitDate = dtVisitDate.SelectedDate.ToString();
            if (btnScheduledYes.Checked)
                tvs.Scheduled = 1;
            else if (btnScheduledNo.Checked)
                tvs.Scheduled = 0;
            tvs.VisitType = _CheckInt(cbVisitType.SelectedValue);
            tvs.Present = _CheckInt(rcbPresent.SelectedValue);
            tvs.CGName = txtCaregiver.Text;
            tvs.CGPhoneNumber = txtCareGiverContactNo.Text;
            if (chkHospYes.Checked)
            {
                tvs.AdmittedtoHospital = 1;
                if (txtNumDayHosp.Text != "") tvs.NumDaysHosp = _CheckInt(txtNumDayHosp.Text);
                tvs.WhereHosp = _CheckInt(rcbWhereHosp.SelectedValue);
                tvs.DischargeDiagnosis = txtDischargeDiagnosis.Text;
                tvs.DischargeNote = txtDischargeNote.Text;
            }
            else if (chkHospNo.Checked)
                tvs.AdmittedtoHospital = 0;
            else if (!chkHospYes.Checked && !chkHospNo.Checked)
                tvs.AdmittedtoHospital = 2;

            tvs.Temp = _CheckDecimal(txtTemp.Text);

            tvs.Weight = _CheckDecimal(txtWeight.Text);
            tvs.Height = _CheckDecimal(txtHeight.Text);

            tvs.BMI = _CheckDecimal(txtBMI.Text);
            tvs.RespRate = _CheckInt(txtRespRate.Text);
            tvs.Pulse = _CheckInt(txtPulse.Text);
            tvs.BPSyst = _CheckInt(txtSystolic.Text);
            tvs.BPDiast = _CheckInt(txtDiastolic.Text);
            tvs.HeadCirc = _CheckDecimal(txtHeadCirc.Text);
            tvs.MUAC = _CheckDecimal(txtMUAC.Text);
            tvs.DevScreening = _CheckInt(cbDevScreen.SelectedValue);
            tvs.TannerStage = _CheckInt(cbTannerStage.SelectedValue);

            tvs.SexuallyActiveYN = 2;
            tvs.ProtectedSexYN = 2;
            if (btnSexActiveYes.Checked)
            {
                tvs.SexuallyActiveYN = 1;
                if (btnPregnantYes.Checked)
                    tvs.PregnantYN = 1;
                if (btnPregnantNo.Checked)
                    tvs.PregnantYN = 0;
                if (btncondomsYes.Checked)
                    tvs.ProtectedSexYN = 1;
                if (btncondomsNo.Checked)
                    tvs.ProtectedSexYN = 0;
                tvs.FamilyPlanning = _CheckInt(cbOtherFamilyPlanning.SelectedValue);
                if (cbOtherFamilyPlanning.SelectedItem.Text.Contains("Other"))
                    tvs.FamilyPlanningOther = txtFamilyPlanningOther.Text;
            }
            else if (btnSexActiveNo.Checked)
                tvs.SexuallyActiveYN = 0;

            //set Clinical History/Findings Tab
            tvs.ClinicalStage = _CheckInt(cbClinicalStage.SelectedValue);
            tvs.ClinicalNotes = txtClinicalNotes.Text;

            List<objVisit.PhysicalFinding> pfList = new List<objVisit.PhysicalFinding>();
            foreach (GridDataItem item in rgFindings.Items)
            {
                objVisit.PhysicalFinding theFP = new objVisit.PhysicalFinding();
                theFP.SymptomID = _CheckInt(item["ID"].Text);
                if (item["PName"].Text == "Other (specify)")
                    theFP.SymptomDescription = txtOtherFindings.Text;
                pfList.Add(theFP);

            }
            tvs.PhysicalFindings = pfList;

            tvs.AdverseEvents.Clear();
            if (btnAEYes.Checked)
            {
                tvs.AdverseEventYN = 1;
                foreach (GridDataItem item in rgAE.Items)
                {
                    objVisit.AdverseEvent theAE = new objVisit.AdverseEvent();
                    theAE.AdverseEventID = _CheckInt(item["EventID"].Text);
                    theAE.AdverseEventDescription = txtAdverseEventComment.Text;
                    RadComboBox cbSev = (RadComboBox)item.FindControl("rcbAESeverity");
                    theAE.AdvereEventSeverityID = _CheckInt(cbSev.SelectedValue);
                    if (item["EventCatName"].Text.Contains("Other"))
                        theAE.AdverseEventOther = txtAdverseEventOther.Text;

                    tvs.AdverseEvents.Add(theAE);
                }
            }
            else if (btnAENo.Checked)
            {
                tvs.AdverseEventYN = 0;
            }


            if (btnTBContactYes.Checked)
            {
                tvs.NewTBContactYN = 1;
                if (btnKnownSensitivityYes.Checked)
                {
                    tvs.SensitivityTBYN = 1;
                    List<objVisit.SensitivityResistance> SensRes = new List<objVisit.SensitivityResistance>();
                    foreach (GridDataItem item in rgTBStatus.Items)
                    {
                        objVisit.SensitivityResistance theSensRes = new objVisit.SensitivityResistance();
                        theSensRes.RegimenID = _CheckInt(item["ID"].Text);
                        if (item["Type"].Text == "Sensitive") {
                            theSensRes.ResistanceYN = 0;
                            theSensRes.SensitivityYN = 1;
                        } else {
                            theSensRes.ResistanceYN = 1;
                            theSensRes.SensitivityYN = 0;
                        }
                        SensRes.Add(theSensRes);
                    }
                    tvs.SensitvityList = SensRes;
                }
                else if (btnKnownSensitivityNo.Checked)
                {
                    tvs.SensitivityTBYN = 0;
                }
                else
                {
                    tvs.SensitivityTBYN = 9;
                }
            }
            else if (btnTBContactNo.Checked)
            {
                tvs.NewTBContactYN = 0;
            }

            if (btnConRecTreatmentYes.Checked)
                tvs.TreatmentYN = 1;
            if (btnConRecTreatmentNo.Checked)
                tvs.TreatmentYN = 0;

            if (btnConRecTreatmentYes.Checked)
            {
                foreach (RadComboBoxItem checkeditem in rcbTreatment.CheckedItems)
                {
                    tvs.Treatment.Add(_CheckInt(checkeditem.Value));
                }
            }
            else
                tvs.Treatment.Clear();

            if (btnInjectionsYes.Checked)
            {
                tvs.DailyInjectionsYN = 1;
                tvs.FormOfTreatment = _CheckInt(cbContactTreatement.SelectedValue.Split('|').GetValue(0).ToString());

                if (cbContactTreatement.SelectedValue.Contains("Other"))
                {
                    tvs.ContactOtherTBProphylaxis = txtTBContactOtherProph.Text;
                }
            }
            if (btnInjectionsNo.Checked)
                tvs.DailyInjectionsYN = 0;

            tvs.TBStatus = _CheckInt(cbPtnTBStatus.SelectedValue);

            if (cbPtnTBStatus.SelectedItem.Text == "TB Rx")
            {
                tvs.TBRxStartDate = dtPtnRxStartDate.SelectedDate.ToString();
                if (rbtnOnTreatmentYes.Checked)
                    tvs.StillOnTreatment = 1;
                if (rbtnOnTreatmentNo.Checked)
                {
                    tvs.StillOnTreatment = 0;
                    tvs.TBRxEndDate = dtPtnRxEndDate.SelectedDate.ToString();
                }


                tvs.TBDiagnosisList.Clear();
                //tvs.TBRxDiagnosis = _CheckInt(cbTBPtnDiagMade.SelectedValue);

                for (int iDiagCounter = 0; iDiagCounter < cbTBPtnDiagMade.CheckedItems.Count; iDiagCounter++)
                {
                    objVisit.TBRxDiagnosis theTBDiagnosis = new objVisit.TBRxDiagnosis();
                    theTBDiagnosis.TBDiagnosisID = _CheckInt(cbTBPtnDiagMade.CheckedItems[iDiagCounter].Value);
                    tvs.TBDiagnosisList.Add(theTBDiagnosis);
                }

                

                //var collection = cbTBPtnDiagMade.CheckedItems;
                //if (collection.Count != 0)
                //{
                //    foreach (var item in collection)
                //    {
                //        objVisit.TBRxDiagnosis theTBDiagnosis = new objVisit.TBRxDiagnosis();
                //        theTBDiagnosis.TBDiagnosisID = _CheckInt(item.Value);
                //        newTBDiagnosis.Add(theTBDiagnosis);
                //    }
                //    tvs.TBDiagnosisList = newTBDiagnosis;
                //}


                if (btnPtnNewSensitivityYes.Checked)
                {
                    tvs.NewSensitivityInfoYN = 1;
                    List<objVisit.SensitivityResistance> NewSensRes = new List<objVisit.SensitivityResistance>();
                    foreach (GridDataItem item in rgNewSens.Items)
                    {
                        objVisit.SensitivityResistance theSensRes = new objVisit.SensitivityResistance();
                        theSensRes.RegimenID = _CheckInt(item["ID"].Text);
                        if (item["Type"].Text == "Sensitive")
                        {
                            theSensRes.ResistanceYN = 0;
                            theSensRes.SensitivityYN = 1;
                        }
                        else
                        {
                            theSensRes.ResistanceYN = 1;
                            theSensRes.SensitivityYN = 0;
                        }
                        NewSensRes.Add(theSensRes);
                    }
                    tvs.NewSensitvityList = NewSensRes;
                }

                if (btnPtnNewSensitivityNo.Checked)
                    tvs.NewSensitivityInfoYN = 0;

                tvs.PatientTBTreatment = _CheckInt(cbPtnTBTreatment.SelectedValue);

                if (cbPtnTBTreatment.SelectedItem.Text.Contains("Other"))
                    tvs.PatientOtherTBProphylaxis = txtPtnOtherProph.Text;
            }
            else if (cbPtnTBStatus.SelectedItem.Text == "Completing TB treatment now")
            {
                tvs.TBRxEndDate = dtPtnRxEndDate.SelectedDate.ToString();
            }

            

            tvs.FeedingPractice = _CheckInt(cbFeedingPractice.SelectedValue);
            tvs.NutritionalProblems = _CheckInt(cbNutionalProblems.SelectedValue);
            tvs.NutrionalSupport = _CheckInt(cbNurtionalSupport.SelectedValue);

            //Pharmacy and Lab
            if (btnDispensedYes.Checked)
                tvs.DispensedYN = 1;
            if (btnDispensedNo.Checked)
                tvs.DispensedYN = 0;
            

            tvs.ReasonNotDispensed = txtPharmacyNotes.Text;

            tvs.CTXAdherance = _CheckInt(cbCTXAdherence.SelectedValue);
            tvs.ARVAdherance = _CheckInt(cbARVAdherence.SelectedValue);

            tvs.WhyARVAdherances.Clear();
            if ((cbARVAdherence.Text == "Fair") || (cbARVAdherence.Text == "Poor"))
            {
                foreach (RadComboBoxItem checkeditem in cbARVWhyReason.CheckedItems)
                {
                    if (checkeditem.Text == "Other Specify")
                    {
                        tvs.OtherARVReason = txtARVWhyOther.Text;
                        objVisit.WhyARVAdherance wah = new objVisit.WhyARVAdherance();
                        wah.ARVAdheranceID = _CheckInt(checkeditem.Value);
                        tvs.WhyARVAdherances.Add(wah);
                    }
                    else
                    {
                        tvs.OtherARVReason = "";
                        objVisit.WhyARVAdherance wah = new objVisit.WhyARVAdherance();
                        wah.ARVAdheranceID = _CheckInt(checkeditem.Value);
                        tvs.WhyARVAdherances.Add(wah);
                    }
                }
            }

            tvs.SubsInterruptions = _CheckInt(cbARVSubstitutions.SelectedValue);

            if (cbARVSubstitutions.SelectedItem.Text == "Change regimen")
            {
                List<objVisit.ChangeRegimenReason> chr = new List<objVisit.ChangeRegimenReason>();
                foreach (RadComboBoxItem ri in rcbARVChangeReason.Items) {
                    if (ri.Checked) {
                        objVisit.ChangeRegimenReason cr = new objVisit.ChangeRegimenReason();
                        cr.ChangeReasonID = _CheckInt(ri.Value);
                        chr.Add(cr);
                        if (ri.Text.Contains("Other"))
                        {
                            tvs.ChangeReasonOther = txtARVChangeOther.Text;
                        }
                    }
                }
                tvs.ChangeRegimenReasons = chr;
            }
            else if (cbARVSubstitutions.SelectedItem.Text == "Stop treatment")
            {
                List<objVisit.StopRegimenReason> chr = new List<objVisit.StopRegimenReason>();
                foreach (RadComboBoxItem ri in cbARVStopReason.Items)
                {
                    if (ri.Checked)
                    {
                        objVisit.StopRegimenReason cr = new objVisit.StopRegimenReason();
                        cr.StopReasonID = _CheckInt(ri.Value);
                        chr.Add(cr);
                        if (ri.Text.Contains("Other"))
                        {
                            tvs.StopReasonOther = txtStopReasonOther.Text;
                        }
                    }
                }
                if ((_CheckDate(dtARVEndDate.SelectedDate.ToString()).Year != 1900))
                    tvs.ARTEndDate = dtARVEndDate.SelectedDate.ToString();
                tvs.StopRegimenReasons = chr;
            }


            //Visit Finalization
            if (btnDisclosedYes.Checked)
            {
                tvs.DisclosedToChild = 1;
                tvs.LevelofDisclosure = _CheckInt(rcbDisclosedLvl.SelectedValue);
            }
            else if (btnDisclosedNo.Checked)
                tvs.DisclosedToChild = 0;

            tvs.ReferredToServiceList.Clear();
            objVisit.ReferredTo rt = new objVisit.ReferredTo();
            string[] refValID = cbReferredTo.SelectedValue.Split('|');
            if (refValID.Length > 0)
            {
                
                rt.RefferredID = _CheckInt(refValID[0].ToString());
                tvs.ReferredToServiceList.Add(rt);

                if (cbReferredTo.SelectedItem.Text.Contains("Other"))
                    tvs.RefferredToOther = txtReferredOther.Text;

            }
            if (btnTransOutYes.Checked)
                tvs.TransferOut = 1;
            else if (btnTransOutNo.Checked)
                tvs.TransferOut = 0;
            if (_CheckDate(dtNextAppointment.SelectedDate.ToString()).Year != 1900)
                tvs.NextAppointmentDate = dtNextAppointment.SelectedDate.ToString();

        }
        protected void DisableControls(Control parent, bool State)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is RadTextBox)
                {
                    ((RadTextBox)(c)).Enabled = State;
                }
                if (c is RadComboBox)
                {
                    ((RadComboBox)(c)).Enabled = State;
                }
                if (c is RadButton)
                {
                    ((RadButton)(c)).Enabled = State;
                }
                if (c is RadDatePicker)
                {
                    ((RadDatePicker)(c)).Enabled = State;
                }
                if (c is RadMaskedTextBox)
                {
                    ((RadMaskedTextBox)(c)).Enabled = State;
                }

                DisableControls(c, State);
            }
        }

        // using (_) to help intellisense find the method quicker as it gets typed alot
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
        private bool? _CheckBool(string TheBool)
        {
            switch (TheBool)
            {
                case "1":
                    TheBool = "true";
                    break;
                case "0":
                    TheBool = "false";
                    break;
                default:
                    TheBool = string.Empty;
                    break;
            }
            bool theVal = new bool();
            if (bool.TryParse(TheBool, out theVal))
                return theVal;
            else
                return null;
        }

        protected DataTable GetDataFromQuery(string SQLQuery)
        {

            IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");
            return ptnMgr.ReturnDatatableQuery(SQLQuery);
        }

        protected void SetAdverseEvents()
        {
            DataTable ParentDT = GetDataFromQuery("select Name, ID from mst_AdverseEventcategory");
            DataTable ChildDT = GetDataFromQuery("select Distinct Name, ID, adverseEventCategoryID from mst_AdverseEventname where adverseEventCategoryID in (select ID from mst_AdverseEventcategory) ");
            for (int i = 0; i < ParentDT.Rows.Count; i++)
            {
                RadTreeNode node = new RadTreeNode();
                node.Text = ParentDT.Rows[i][0].ToString();
                node.Value = ParentDT.Rows[i][1].ToString();
                node.ExpandMode = TreeNodeExpandMode.ClientSide;
                node.Checkable = false;
                for (int j = 0; j < ChildDT.Rows.Count; j++)
                {
                    if (ChildDT.Rows[j][2].ToString() == ParentDT.Rows[i][1].ToString())
                    {
                        RadTreeNode childnode = new RadTreeNode();
                        childnode.Text = ChildDT.Rows[j][0].ToString();
                        childnode.Value = ChildDT.Rows[j][1].ToString();
                        node.Nodes.Add(childnode);
                    }
                }

                rtwAEvents.Nodes.Add(node);
            }
            rtwAEvents.DataBind();

        }

        protected void SetTBDrugSensitivity()
        {

            DataTable dtTBSensRes = GetDataFromQuery("select ID, Name, 0 as Sensitive, 0 as Resistant from mst_deCode where codeid=(select codeid from mst_code where name='TB Sensitivity and Resistance') and deleteflag = 0");
            rgdTBDrugsSensitivity.DataSource = dtTBSensRes;
            rgdTBDrugsSensitivity.DataBind();
            rgdNewSensitivity.DataSource = dtTBSensRes;
            rgdNewSensitivity.DataBind();

            dtTBDrugSens = dtTBSensRes.Copy();
            dtNewSens = dtTBSensRes.Copy();
            dtNewSens.Columns.Add("Type");
            dtTBDrugSens.Columns.Add("Type");

            DataTable dtTreatment = dtTBSensRes.Clone();
            dtTBSensRes.Columns.Remove("Sensitive");
            dtTBSensRes.Columns.Remove("Resistant");

            theBind.BindCombo(rcbTreatment, dtTBSensRes, "Name", "ID");



        }

        protected DataTable GetInvisiblateClone(DataTable TableToClone, string TableToInvisiblate)
        {
            DataTable dtCloned = TableToClone.Clone();
            dtCloned.Columns[0].DataType = typeof(string);
            DataRow[] DR = TableToClone.Select("ID = 0");
            if (DR.Length < 1)
            {
                cbInsertSelectValue(ref dtCloned);
            }
            foreach (DataRow row in TableToClone.Rows)
            {
                dtCloned.ImportRow(row);
                if (dtCloned.Rows[dtCloned.Rows.Count - 1]["Name"].ToString().Contains("Other"))
                    dtCloned.Rows[dtCloned.Rows.Count - 1]["ID"] += "|" + TableToInvisiblate + "|show";
                else
                    dtCloned.Rows[dtCloned.Rows.Count - 1]["ID"] += "|" + TableToInvisiblate + "|hide";
            }

            return dtCloned;
        }
        protected void cbInsertSelectValue(ref DataTable dt)
        {

            DataRow theDR = dt.NewRow();
            theDR["Name"] = "Select";
            theDR["ID"] = "0";
            dt.Rows.InsertAt(theDR, 0);

        }
        private class Meds
        {
            public Meds(string Drug, bool Sensitive, bool Resistant)
            {
                _drug = Drug;
                _sensitive = Sensitive;
                _resistant = Resistant;

            }
            private string _drug;
            public string Drug
            {
                get { return _drug; }
                set { _drug = value; }
            }
            private bool _sensitive;
            public bool Sensitive
            {
                get { return _sensitive; }
                set { _sensitive = value; }
            }
            private bool _resistant;
            public bool Resistant
            {
                get { return _resistant; }
                set { _resistant = value; }
            }
        }

        protected void txtHeight_TextChanged(object sender, EventArgs e)
        {
            GetBMI();
            txtRespRate.Focus();
        }
        protected void txtWeight_TextChanged(object sender, EventArgs e)
        {
            GetBMI();
            txtHeight.Focus();
        }
        protected void GetBMI()
        {
            //formula for BMI = Weight (kg) / (Height (m) x Height (m))
            if ((txtWeight.Text != "") && (txtHeight.Text != ""))
            {
                decimal wgt = decimal.Parse(txtWeight.Text);
                decimal hgt = decimal.Parse(txtHeight.Text);
                if ((wgt != 0) && (hgt != 0))
                {
                    decimal BMI = wgt / ((hgt / 100) * (hgt / 100));
                    var thePos = BMI.ToString().IndexOf(".");
                    var theVal = string.Empty;
                    if (thePos > 0)
                        theVal = BMI.ToString().Substring(0, thePos + 2);
                    else
                        theVal = BMI.ToString();
                    txtBMI.Text = theVal;
                }
            }
        }
        protected void BindCombo(RadComboBox rcb, DataTable dt, string textField, string valueField)
        {
            rcb.DataSource = dt;
            rcb.DataValueField = valueField;
            rcb.DataTextField = textField;
            rcb.DataBind();
        }

        private void bindSelectGrids() {
            //if (Session["VisitEditMode"].ToString() != "true")
            //{
                OnFindingsClose();
                //if (Session["VisitEditMode"] == "true")
                OnTBsensClose();
                //if (rgAE.Items.Count < 1)
                if (Session["AEFirstLoad"] != null)
                {
                    if (!(bool)Session["AEFirstLoad"])
                    {
                        onAEClose();

                    }
                    else
                    {
                        Session["AEFirstLoad"] = false;
                    }
                }
                else
                {
                    onAEClose();
                }
                OnTBsensNewClose();
            //}
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "pftabjump", "$('#tabs').tabs('option', 'active', 1);", true);
            
            //check yes/nos first
            if (chkHospYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowHosp", "$('#hideHospitalYN').show();", true);
            if (_CheckInt(cbTannerStage.SelectedItem.Text) >= 3) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowTanner", "$('#HideSexActiveYN').show();", true);
            if (btnSexActiveYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowSexActive", "$('.ShowIfSexuallyActive').show(); if ($('#lblSex').html() == 'Female') $('.ShowIfSexuallyActiveFemale').show();", true);
            if (btnPregnantYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowPreg", "$('.ShowIfSexuallyActive').hide();", true);
            if (btncondomsYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowcondoms", "$('.ShowProtectedSexYN').show();", true);
            if (cbOtherFamilyPlanning.SelectedItem.Text.Contains("Other")) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowSFam", "$('#ShowIfPlanningOther').show();", true);
            if (btnTBContactYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowtbContact", "$('#HideTBSContactSensitiveYN').show();", true);
            if (btnKnownSensitivityYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowtbknownSens", "$('#IndContactSensitivity').show();", true);
            if (btnConRecTreatmentYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowtbconrecTreat", "$('#ShowIfContactRecTreatment').show();", true);
            if (btnInjectionsYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowInjections", "$('#ShowIfInjectionsYes').show();", true);
            if (cbContactTreatement.SelectedItem.Text.Contains("Other")) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowOtherProph", "$('#ShowIfTreatmentOther').show();", true);
            switch (cbPtnTBStatus.SelectedItem.Text)
            {
                    case "TB Rx":
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStbrx", "$('.ShowIfTBRxQ1').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStreat", "$('.ShowIfOnTreatmentYes').hide();", true);
                    break;
                case "Completing TB treatment now":
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStbrx", "$('.ShowIfTBRxQ1').hide();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStreat", "$('.ShowIfOnTreatmentYes').show();", true);
                    break;
                default:
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStbrx", "$('.ShowIfTBRxQ1').hide();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStreat", "$('.ShowIfOnTreatmentYes').hide();", true);
                    break;
            }
            if (rbtnOnTreatmentNo.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStbrx", "$('.ShowIfOnTreatmentYes').show();",true);
            if (btnPtnNewSensitivityYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowStNewSens", "$('#indicateSenseYN').show();", true);
            if (cbPtnTBTreatment.SelectedItem.Text.Contains("Other")) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sHowSTBTreat", "$('.ShowIfPtnTBTreatment').show();", true);
            if (btnDispensedNo.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showDispensed", "$('#HidePharmNotes').show();", true);
            if((cbARVAdherence.SelectedItem.Text == "Fair") || (cbARVAdherence.SelectedItem.Text == "Poor")) 
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showARVAdherance", "$('#ShowIfARVAdherenceFairPoor').show();", true);
            foreach (RadComboBoxItem item in cbARVWhyReason.Items)
            {
                if (item.Checked)
                {
                    if (item.Text.Contains("Other"))
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showhyarvreason", "$('#ShowIfOtherARVReason').show();", true);
                }
            }
            if (cbARVSubstitutions.SelectedItem.Text == "Change regimen") ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showhchangereg", "$('#ShowIfChange').show();", true);
            foreach (RadComboBoxItem item in rcbARVChangeReason.Items)
            {
                if (item.Checked)
                {
                    if (item.Text.Contains("Other"))
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showregothersasa", "$('#ShowIfChangeOther').show();", true);
                }
            }
            if (cbARVSubstitutions.SelectedItem.Text == "Stop treatment") ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showrearvsubr", "$('#ShowIfStopped').show();", true);
            foreach (RadComboBoxItem item in cbARVStopReason.Items)
            {
                if (item.Checked)
                {
                    if (item.Text.Contains("Other"))
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showrearvsusfbr", "$('#ShowIfStoppedOther').show();", true);
                }
            }
            if (btnDisclosedYes.Checked) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showdisclosed", "$('#HideDisclosedNote').show();", true);
            if (cbReferredTo.SelectedItem.Text.Contains("Other")) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showreferrredto", "$('#ShowIfReferredToOther').show();", true);


            //set grids
            if (rgFindings.Items.Count > 0) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showFindingsdiv", "$('#divFindings').show();focusAndLayout('rgFindings');", true);
            if (rgAE.Items.Count > 0) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showtaesdiv", "ShowHide('HideAEventYN', 'show', 'divAE');$('#divAE').show();focusAndLayout('rgAE');", true);
            if (rgTBStatus.Items.Count > 0) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showtbstatusdiv", "$('#divTBStatus').show();ShowHide('HideTBSContactSensitiveYN', 'show', 'rgTBStatus');focusAndLayout('rgTBStatus');", true);
            if (rgNewSens.Items.Count > 0) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showtnewsensdiv", "$('#divSensNew').show();ShowHide('indicateSenseYN', 'show', 'rgNewSens');focusAndLayout('rgNewSens');", true);

            //Check AE grid for other
            foreach (GridDataItem item in rgAE.Items)
            {
                if (item["EventCatName"].Text.Contains("Other"))
                {
                  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showAEOthersdivs", "$('#HideAEOtherdiv').show();", true);  
                }
            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "checkTheDivSize", "resizeScrollbars();", true);
        }

        protected void btnHidPFindings_Click(object sender, EventArgs e)
        {
            bindSelectGrids();
        }
        private void OnFindingsClose()
        {
            uptPFindings.Update();
            dtPFindings.PrimaryKey = new DataColumn[] { dtPFindings.Columns["ID"] };
            DataTable ClonedDB = dtPFindings.Clone();
            foreach (GridDataItem item in rgdPhysicalFindings.Items)
            {
                RadButton YNVal = (RadButton)item.FindControl("btnYN");
                if (YNVal.SelectedToggleStateIndex == 1)
                {
                    object[] findVals = new object[1];
                    findVals[0] = item["ID"].Text;

                    DataRow theDR = dtPFindings.Rows.Find(findVals);
                    ClonedDB.ImportRow(theDR);

                    if (item["Pname"].Text.Contains("Other"))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showfindingsotherdiv", "$('#divFindingsother').show();", true);
                    }
                }
            }

            rgFindings.DataSource = ClonedDB;
            rgFindings.DataBind();
            updtFindings.Update();
        }


        protected void rgFindings_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            _CurrentTab = "1";
            _CurrentAnchor = "absFindings";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showupdtfindings", "ShowMinLoading('somediv');", true);
            GridDataItem gi = (GridDataItem)e.Item;
            if (e.CommandName == "Delete")
            {
                DataTable ClonedDB = dtPFindings.Clone();
                string theKey = gi.GetDataKeyValue("ID").ToString();
                foreach (GridDataItem item in rgdPhysicalFindings.Items)
                {
                    RadButton YNVal = (RadButton)item.FindControl("btnYN");
                    if (item["ID"].Text == theKey)
                    {
                        YNVal.SelectedToggleStateIndex = 0;
                    }
                    if (YNVal.SelectedToggleStateIndex == 1)
                    {
                        object[] findVals = new object[1];
                        findVals[0] = item["ID"].Text;
                        if (item["Pname"].Text.Contains("Other"))
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showfindingsotherdiv", "$('#divFindingsother').show();", true);
                        }
                        DataRow theDR = dtPFindings.Rows.Find(findVals);
                        ClonedDB.ImportRow(theDR);
                    }
                }
                rgFindings.DataSource = ClonedDB;
                rgFindings.DataBind();
                uptPFindings.Update();
                updtFindings.Update();
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "pftabjump", "$('#tabs').tabs('option', 'active', " + _CurrentTab + ");", true);
                _CurrentTab = "0";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showfindingsdiv", "$('#divFindings').show();GoToPanePos(220);", true);
            }
        }

        protected void btnHidTBSens_Click(object sender, EventArgs e)
        {
            bindSelectGrids();
        }
        private void OnTBsensClose()
        {
            updtTBStatus.Update();
            dtTBDrugSens.PrimaryKey = new DataColumn[] { dtTBDrugSens.Columns["ID"] };

            DataTable ClonedDB = dtTBDrugSens.Clone();
            foreach (GridDataItem item in rgdTBDrugsSensitivity.Items)
            {
                RadComboBox YNVal = (RadComboBox)item.FindControl("rcbResSen");
                if (YNVal.SelectedValue != "Unknown")
                {
                    object[] findVals = new object[1];
                    findVals[0] = item["ID"].Text;

                    DataRow theDR = dtTBDrugSens.Rows.Find(findVals);

                    theDR["Type"] = YNVal.SelectedItem.Text;

                    ClonedDB.ImportRow(theDR);

                }
            }

            rgTBStatus.DataSource = ClonedDB;
            rgTBStatus.DataBind();
            updtTBStatus.Update();

            
        }

        protected void rgTBStatus_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            _CurrentTab = "1";
            _CurrentAnchor = "absTBStatus";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showupdtTBStatus", "ShowMinLoading('somediv');", true);
            GridDataItem gi = (GridDataItem)e.Item;
            if (e.CommandName == "Delete")
            {
                DataTable ClonedDB = dtTBDrugSens.Clone();
                string theKey = gi.GetDataKeyValue("ID").ToString();
                foreach (GridDataItem item in rgdTBDrugsSensitivity.Items)
                {
                    RadComboBox YNVal = (RadComboBox)item.FindControl("rcbResSen");
                    if (YNVal != null)
                    {
                        if (item["ID"].Text == theKey)
                        {
                            YNVal.SelectedIndex = 0;
                        }

                        if (YNVal.SelectedIndex != 0)
                        {
                            object[] findVals = new object[1];
                            findVals[0] = item["ID"].Text;

                            DataRow theDR = dtTBDrugSens.Rows.Find(findVals);
                            ClonedDB.ImportRow(theDR);
                        }
                    }
                }

                rgTBStatus.DataSource = ClonedDB;
                rgTBStatus.DataBind();
                updtTBStatus.Update();
                updtTBSens.Update();

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "pftabjump", "$('#tabs').tabs('option', 'active', " + _CurrentTab + ");", true);
                _CurrentTab = "0";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "pfanchorjump", "GoToElem('" + _CurrentAnchor + "');", true);
                _CurrentAnchor = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showtbstatusdiv", "$('#divTBStatus').show();", true);
            }
        }



        protected void btnHidTBSensNew_Click(object sender, EventArgs e)
        {
            bindSelectGrids();
        }
        private void OnTBsensNewClose()
        {
            //updtTBStatus.Update();
            dtNewSens.PrimaryKey = new DataColumn[] { dtNewSens.Columns["ID"] };

            DataTable ClonedDB = dtNewSens.Clone();
            foreach (GridDataItem item in rgdNewSensitivity.Items)
            {
                RadComboBox YNVal = (RadComboBox)item.FindControl("rcbNewResSens");
                if (YNVal.SelectedIndex != 0)
                {
                    object[] findVals = new object[1];
                    findVals[0] = item["ID"].Text;

                    DataRow theDR = dtNewSens.Rows.Find(findVals);

                    theDR["Type"] = YNVal.SelectedItem.Text;

                    ClonedDB.ImportRow(theDR);

                }
            }

            rgNewSens.DataSource = ClonedDB;
            rgNewSens.DataBind();
            uptSensNew.Update();


        }

        protected void rgNewSens_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            _CurrentTab = "1";
            _CurrentAnchor = "absTBStatus";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showupdtTBStatus", "ShowMinLoading('somediv');", true);
            GridDataItem gi = (GridDataItem)e.Item;
            if (e.CommandName == "Delete")
            {
                DataTable ClonedDB = dtNewSens.Clone();
                string theKey = gi.GetDataKeyValue("ID").ToString();
                foreach (GridDataItem item in rgdNewSensitivity.Items)
                {
                    RadComboBox YNVal = (RadComboBox)item.FindControl("rcbNewResSens");
                    if (YNVal != null)
                    {
                        if (item["ID"].Text == theKey)
                        {
                            YNVal.SelectedIndex = 0;
                        }

                        if (YNVal.SelectedIndex != 0)
                        {
                            object[] findVals = new object[1];
                            findVals[0] = item["ID"].Text;

                            DataRow theDR = dtNewSens.Rows.Find(findVals);
                            ClonedDB.ImportRow(theDR);
                        }
                    }
                }

                rgNewSens.DataSource = ClonedDB;
                rgNewSens.DataBind();
                uptSensNew.Update();
                uptSensNew.Update();

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "pftabjump", "$('#tabs').tabs('option', 'active', " + _CurrentTab + ");", true);
                _CurrentTab = "0";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "pfanchorjump", "GoToElem('" + _CurrentAnchor + "');", true);
                _CurrentAnchor = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showtbstatusdiv", "$('#divSensNew').show();", true);
            }
        }


        protected void rcbAESeverity_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var combobox = sender as RadComboBox;
            GridDataItem dataItem = combobox.Parent.Parent as GridDataItem;
            var text = dataItem["EventID"].Text;
            DataRow[] DRfind = dtAE.Select("EventID = '" + text + "'");
            if (DRfind.Length > 0)
            {
                DRfind[0]["SeverityID"] = combobox.SelectedValue;
            }
            bindSelectGrids();
        }

        protected void btnHidAE_Click(object sender, EventArgs e)
        {
            bindSelectGrids();
            
        }
        private void onAEClose()
        {
            //clear the Datatable
            //dtAE.Clear();

            List<RadTreeNode> alls = (List<RadTreeNode>)rtwAEvents.GetAllNodes();
            if (alls.Count > 0)
            {
                foreach (RadTreeNode node in alls)
                {
                    //if it is checked and at the right level
                    if ((node.Checked == true) && (node.Level > 0))
                    {
                        DataRow[] DRfind = dtAE.Select("EventID = '" + node.Value + "'");
                        if (DRfind.Length < 1)
                        {
                            DataRow dr = dtAE.NewRow();
                            RadTreeNode theParentNode = alls.Find(item => item.Text == node.ParentNode.Text);
                            dr[0] = theParentNode.Value;
                            dr[1] = theParentNode.Text;
                            dr[2] = node.Value;
                            dr[3] = node.Text;
                            dtAE.Rows.Add(dr);
                            if (theParentNode.Text.Contains("Other"))
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showAEOthersdiv", "$('#HideAEOtherdiv').show();", true);
                        }
                    }
                        //if is not checked and at the right level
                    else if ((node.Checked == false) && (node.Level > 0))
                    {
                        DataRow[] DRfind = dtAE.Select("EventID = '" + node.Value + "'");
                        if (DRfind.Length > 0)
                        {
                            dtAE.Rows.Remove(DRfind[0]);
                        }
                    }
                }
            }

            rgAE.DataSource = dtAE;
            rgAE.DataBind();
            updtAE.Update();

            
        }


        protected void rgAE_OnItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                TableCell cell = item["Severity"];
                RadComboBox cmb = (RadComboBox)cell.FindControl("rcbAESeverity");
                BindFunctions BindManager = new BindFunctions();
                BindManager.BindCombo(cmb, GetBindTables("210"), "Name", "ID");

                int theKey = int.Parse(item.GetDataKeyValue("EventCatID").ToString());
                //if there is a val then set it
                DataRow[] DRDel = dtAE.Select("EventCatID = " + theKey);
                if (DRDel[0]["SeverityID"].ToString() != "")
                {
                    cmb.SelectedValue = DRDel[0]["SeverityID"].ToString();
                }

            }
        }
        protected DataTable GetBindTables(string CodeID)
        {
            DataTable theDT = new DataTable();
            DataSet theDS = new DataSet();
            theDS.ReadXml(MapPath("..\\..\\XMLFiles\\ALLMasters.con"));
            IQCareUtils theUtils = new IQCareUtils();
            if (theDS.Tables["mst_decode"] != null)
            {
                DataView theDV = new DataView(theDS.Tables["mst_decode"]);
                theDV.RowFilter = "codeid=" + CodeID + " AND DeleteFlag = 0 AND systemID = 3";
                if (theDV.Table != null)
                {
                    theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                    theDT = theUtils.CreateTableFromDataView(theDV);
                }
            }
            return theDT; 
        }
        protected void rgAE_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            _CurrentTab = "1";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showupdtAE", "ShowMinLoading('somediv');", true);
            GridDataItem gi = (GridDataItem)e.Item;
            if (e.CommandName == "Delete")
            {
                

                //Update the DataTable
                foreach (GridDataItem item in rgAE.Items)
                {
                    RadComboBox Severity = (RadComboBox)item.FindControl("rcbAESeverity");
                    DataRow[] DRfind = dtAE.Select("EventCatID = " + item.GetDataKeyValue("EventCatID").ToString());
                    DRfind[0]["SeverityID"] = Severity.SelectedValue.ToString();
                }

                DataRow[] DRDel = dtAE.Select("EventCatID = " + gi.GetDataKeyValue("EventCatID").ToString());
                dtAE.Rows.Remove(DRDel[0]);

                rgAE.DataSource = dtAE;
                rgAE.DataBind();
                updtAE.Update();

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "pftabjump", "$('#tabs').tabs('option', 'active', " + _CurrentTab + ");", true);
                _CurrentTab = "0";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showaeafterdel", "$('#divAE').show();GoToPanePos(150);", true);
            }
        }
        public DataTable AEDets()
        {
            //Generate DataTable
            DataTable DT = new DataTable("AEdets");
            DT.Columns.Add("EventCatID");
            DT.Columns.Add("EventCatName");
            DT.Columns.Add("EventID");
            DT.Columns.Add("EventName");
            DT.Columns.Add("SeverityID");
            return DT;
        }
        public static string GetDuplicateRecord(string formatdate)
        {
            frmVisitTouch frm = new frmVisitTouch();
            string value = "0";
            if (_VisitID == "")
            {
                DataTable theDT = frm.GetDataFromQuery("select count(*) from ord_visit where ptn_pk='" + PatientId + "' and LocationId='" + _LocationID + "' and VisitDate='" + formatdate + "' and VisitType='27'");
                value = theDT.Rows[0][0].ToString();
            }
            else
            {
                if (VDate != formatdate)
                {
                    DataTable theDT = frm.GetDataFromQuery("select count(*) from ord_visit where ptn_pk='" + PatientId + "' and LocationId='" + _LocationID + "' and VisitDate='" + formatdate + "' and VisitType='27'");
                    value = theDT.Rows[0][0].ToString();
               }

            }
            return value;
        }

    }
}
