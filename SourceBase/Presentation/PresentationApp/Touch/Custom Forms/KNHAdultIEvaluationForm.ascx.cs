using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
//IQCare Libs
using Application.Presentation;
using Application.Common;
using Interface.Clinical;
//Third party Libs
using Telerik.Web.UI;
using System.Data;
using System.IO;
using System.Linq;

namespace Touch.Custom_Forms
{
    public partial class KNHAdultIEvaluationForm : TouchUserControlBase
    {
        static Boolean isError = false;
        private static string temperatureLocal;
        private static string respirationRateLocal;
        private static string heartRateLocal;
        private static string systollicBloodPressureLocal;
        private static string diastolicBloodPressureLocal;
        private static string heightLocal;
        private static string weightLocal;
        public static DataTable DtPresenting;
        public static string AditionalComplaints;

        #region property set for page
        
        public static string Temperature
        {
            get
            {
                return temperatureLocal;
            }
            set
            {
                temperatureLocal = value;
            }
        }
        public static string RespirationRate
        {
            get
            {
                return respirationRateLocal;
            }
            set
            {
                respirationRateLocal = value;
            }
        }
        public static string HeartRate
        {
            get
            {
                return heartRateLocal;
            }
            set
            {
                heartRateLocal = value;
            }
        }
        public static string SystollicBloodPressure
        {
            get
            {
                return systollicBloodPressureLocal;
            }
            set
            {
                systollicBloodPressureLocal = value;
            }
        }
        public static string DiastolicBloodPressure
        {
            get
            {
                return diastolicBloodPressureLocal;
            }
            set
            {
                diastolicBloodPressureLocal = value;
            }
        }
        public static string Height
        {
            get
            {
                return heightLocal;
            }
            set
            {
                heightLocal = value;
            }
        }
        public static string Weight
        {
            get
            {
                return weightLocal;
            }
            set
            {
                weightLocal = value;
            }
        }
        #endregion
        public void SetTextBox()
        {
            txtRadTemperatureModalparent.Text = Temperature;
            txtRadRespirationRateparent.Text = RespirationRate;
            txtRadHeartRateparent.Text = HeartRate;
            txtRadSystollicBloodPressureparent.Text = SystollicBloodPressure;
            txtRadDiastolicBloodPressureparent.Text = DiastolicBloodPressure;
            txtRadHeightparent.Text = Height;
            txtRadWeightparent.Text = Weight;
            uptTab1.Update();
        }
        public void BindPresentingComp()
        {
            RadGridPresentingParent.DataSource = DtPresenting;
            RadGridPresentingParent.DataBind();
            txtAdditionPresentingComplaints.Text = AditionalComplaints;
            updtPc.Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String script = frmAdultIE_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            Session["CurrentForm"] = "KNHAdultIEvaluationForm"; 
            Session["FormIsLoaded"] = true;
            LoadUserControl();


            if (Session["IsFirstLoad"].ToString() == "true")
            {
               // Session["Visit_pk"] = "0";// "179120";
                Form_Init();
                Session["IsFirstLoad"] = "false";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);


            }
           base.Page_Load(sender, e);
        }
        private void LoadUserControl()
        {


            //PresentationApp.Touch.KNH.TestModal ucVitalSign = (PresentationApp.Touch.KNH.TestModal)Page.LoadControl("~/Touch/KNH/KNHModal.ascx");
            //PlaceHolder1.Controls.Add(ucVitalSign);
            //PlaceHolder1.DataBind();



        }


      


        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveForm();
        }
        protected DataTable GetResult(string flag, int ID)
        {
            BIQTouchAdultIE objAdultIEFields = new BIQTouchAdultIE();
            objAdultIEFields.Flag = flag;
            objAdultIEFields.PtnPk = Convert.ToInt32(Session["PatientID"].ToString());
            objAdultIEFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
            objAdultIEFields.ID = ID;

            IQTouchKNHAdultIE theExpressManager;
            theExpressManager = (IQTouchKNHAdultIE)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchKNHAdultIE, BusinessProcess.Clinical");
            DataTable dt = theExpressManager.IQTouchGetKnhAdultIEData(objAdultIEFields);
            return dt;
        }
        private void Form_Init()
        {
            LoadAllPreDefinedCombo();
        }
        public static DataTable GetDistinctRecords(DataTable dt, string[] columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, columns);
            return dtUniqRecords;
        }
        protected void LoadAllPreDefinedCombo()
        {
            DataTable dtAll = GetResult("AllComboData", 1061);
            string[] tobeDistinct = { "fieldName" };
            DataTable dtDistinct = GetDistinctRecords(dtAll, tobeDistinct);
            if (dtDistinct.Rows.Count > 0)
            {
                foreach (DataRow row in dtDistinct.Rows)
                {
                    string strfieldName = row["fieldName"].ToString();

                    
                    RadComboBox rcb = (RadComboBox)this.FindControl("rcb" + strfieldName);

                    if (rcb != null)
                    {
                        var query = from inv in dtAll.AsEnumerable()
                                    where inv.Field<string>("fieldName") == strfieldName
                                    select new
                                    {
                                        Field_Name = inv["Name"],
                                        ID = inv["ID"]
                                    };

                        rcb.DataTextField = "Field_Name";
                        rcb.DataValueField = "ID";
                        rcb.DataSource = query;
                        rcb.DataBind();
                    }
                    
                    
                }
            }
            


                        


            //rcb.DataTextField = "Name";
            //rcb.DataValueField = "ID";
            //rcb.DataSource = dtAll;
            //rcb.DataBind();
        }
        int CheckedVaue(string btnToggeState)
        {
            int retval = 0;
            if (btnToggeState.ToUpper() == "YES")
            {
                retval = 1;
            }
            else
            {
                retval = 0;
            }
            return retval;
        }
        DateTime DateGiven(string dateVal)
        {
            DateTime dt = Convert.ToDateTime("01/01/1900");
            if (!string.IsNullOrEmpty(dateVal))
            {
                dt = DateTime.Parse(dateVal);
            }
            return dt;
        }
        int rcbSelectedValue(RadComboBox rcb)
        {
            int retval = 0;
            if (rcb.SelectedValue !="")
            {
                retval = Convert.ToInt32(rcb.SelectedValue.ToString());
            }
            return retval;


        }
        decimal GettxtValue(string strtxtVal)
        {
            decimal retval = 0;
            if (strtxtVal != "")
            {
                retval = Convert.ToDecimal(strtxtVal);
            }
            return retval;

        }



        protected void SaveForm()
        {
            List<BIQTouchAdultIE> list = new List<BIQTouchAdultIE>();
            BIQTouchAdultIE objAdultIE = new BIQTouchAdultIE();
            string error = "";
            try
            {

                objAdultIE.ID = 0;
                objAdultIE.PtnPk = Convert.ToInt32(Request.QueryString["PatientID"]);
                objAdultIE.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
                objAdultIE.UserId = Int32.Parse(Session["AppUserId"].ToString());
                objAdultIE.VisitDate = DateGiven(dtVistiDate.DbSelectedDate.ToString());
                // Modal Vital Sign
                //objAdultIE.Temperature = 0;
                //objAdultIE.RespirationRate = 0;
                //objAdultIE.HeartRate = 0;
                //objAdultIE.SystolicBloodPressure = 0;
                //objAdultIE.DiastolicBloodPressure = 0;
                objAdultIE.DiagnosisConfirmed = CheckedVaue(RadbtnDiagnosisYesNo.SelectedToggleState.Text.ToString());
                objAdultIE.ConfirmHIVPosDate = DateGiven(dtConfirmHIVPosDate.DbSelectedDate.ToString());
                objAdultIE.ChildAccompaniedByCaregiver = CheckedVaue(radbtnChildAccompaniedBy.SelectedToggleState.Text.ToString());
                if (rcbTreatmentSupporterRelationship.SelectedValue != null && radbtnChildAccompaniedBy.SelectedToggleState.Text.ToString() == "Yes")
                {
                    objAdultIE.TreatmentSupporterRelationship = rcbSelectedValue(rcbTreatmentSupporterRelationship);
                }
                objAdultIE.HealthEducation = CheckedVaue(radbtnHealthEducation.SelectedToggleState.Text.ToString());
                objAdultIE.DisclosureStatus = rcbSelectedValue(rcbDisclosureStatus);
                objAdultIE.SchoolingStatus = rcbSelectedValue(rcbSchoolingStatus);
                objAdultIE.HIVSupportgroup = CheckedVaue(radbtnHIVSupportgroup.SelectedToggleState.Text.ToString());
                objAdultIE.PatientReferredFrom = rcbSelectedValue(rcbPatientReferred);
                objAdultIE.NursesComments = txtNursesComments.Text;
                // Section Cient Information
                objAdultIE.PresentingComplaints = 0;// Id of PresentingComplaints Modal
                objAdultIE.LMPassessmentValid = CheckedVaue(radbtnLMPAssessmentValid.SelectedToggleState.Text.ToString());
                objAdultIE.LMPDate = DateGiven(dtLMPAssessmentValid.DbSelectedDate.ToString());
                objAdultIE.LMPNotaccessedReason = rcbSelectedValue(rcbLMPNotaccessedReason);
                objAdultIE.EDD = DateGiven(dtEDDDate.DbSelectedDate.ToString());

                objAdultIE.OtherDiseaseName = txtOtherDiseaseName.Text;
                objAdultIE.OtherDiseaseDate = DateGiven(dtOtherDiseaseDate.DbSelectedDate.ToString());
                objAdultIE.OtherDiseaseTreatment = txtOtherDiseaseTreatment.Text;
                objAdultIE.SchoolPerfomance = rcbSelectedValue(rcbSchoolPerfomance);
                objAdultIE.TBAssessmentICF = 1;
                objAdultIE.TBFindings = rcbSelectedValue(rcbTBFindings);
                objAdultIE.TBresultsAvailable = CheckedVaue(radbtnTBresultsAvailable.SelectedToggleState.Text.ToString());
                objAdultIE.SputumSmear = rcbSelectedValue(rcbSputumSmear);
                objAdultIE.SputumSmearDate = DateGiven(dtSputumSmearDate.DbSelectedDate.ToString());
                objAdultIE.ChestXRay = CheckedVaue(radbtnChestray.SelectedToggleState.Text.ToString());
                objAdultIE.ChestXRayDate = DateGiven(dtChestrayDate.DbSelectedDate.ToString());
                objAdultIE.TissueBiopsy = CheckedVaue(radbtnTissueBiopsy.SelectedToggleState.Text.ToString());
                objAdultIE.TissueBiopsyDate = DateGiven(dtTissueBiopsyDate.DbSelectedDate.ToString());
                objAdultIE.CXR = rcbSelectedValue(rcbCXR);
                objAdultIE.OtherCXR = txtOtherCXR.Text;
                objAdultIE.TBTypePeads = rcbSelectedValue(rcbTBTypePeads);
                objAdultIE.PeadsTBPatientType = rcbSelectedValue(rcbPeadsTBPatientType);
                objAdultIE.TBPlan = rcbSelectedValue(rcbTBPlan);
                objAdultIE.TBPlanOther = txTBPlanOther.Text;
                objAdultIE.TBRegimen = rcbSelectedValue(rcbTBRegimen);
                objAdultIE.TBRegimenStartDate = DateGiven(dtTBRegimenStartDate.DbSelectedDate.ToString());
                objAdultIE.TBRegimenEndDate = DateGiven(dtTBRegimenEndDate.DbSelectedDate.ToString());
                objAdultIE.TBTreatmentOutcomesPeads = rcbSelectedValue(rcbTBTreatmentOutcomesPeads);
                objAdultIE.NoTB = CheckedVaue(radbtnNoTB.SelectedToggleState.Text.ToString());
                objAdultIE.TBReason = 1;// Multi select
                objAdultIE.INHStartDate = DateGiven(dtINHStartDate.DbSelectedDate.ToString());
                objAdultIE.INHEndDate = DateGiven(dtINHEndDate.DbSelectedDate.ToString());
                objAdultIE.PyridoxineStartDate = DateGiven(dtPyridoxineStartDate.DbSelectedDate.ToString());
                objAdultIE.PyridoxineEndDate = DateGiven(dtPyridoxineEndDate.DbSelectedDate.ToString());
                objAdultIE.SuspectTB = CheckedVaue(radbtnSuspectTB.SelectedToggleState.Text.ToString());
                objAdultIE.StopINHDate = DateGiven(dtStopINHDate.DbSelectedDate.ToString());
                objAdultIE.ContactsScreenedForTB = CheckedVaue(radbtnContactsScreenedForTB.SelectedToggleState.Text.ToString());
                objAdultIE.TBNotScreenedSpecify = txtTBNotScreenedSpecify.Text;
                objAdultIE.LongTermMedications = CheckedVaue(radbtnLongTermMedications.SelectedToggleState.Text.ToString());
                objAdultIE.SulfaTMPDate = DateGiven(dtSulfaTMPDate.DbSelectedDate.ToString());
                objAdultIE.HormonalContraceptivesDate = DateGiven(dtHormonalContraceptivesDate.DbSelectedDate.ToString());
                objAdultIE.AntihypertensivesDate = DateGiven(dtAntihypertensivesDate.DbSelectedDate.ToString());
                objAdultIE.HypoglycemicsDate = DateGiven(dtHypoglycemicsDate.DbSelectedDate.ToString());
                objAdultIE.AntifungalsDate = DateGiven(dtAntifungalsDate.DbSelectedDate.ToString());
                objAdultIE.AnticonvulsantsDate = DateGiven(dtAntincovulsantsDate.DbSelectedDate.ToString());
                objAdultIE.OtherLongTermMedications = txOtherLongTermMedications.Text;
                objAdultIE.OtherCurrentLongTermMedications = DateGiven(dtOtherCurrentLongTermMedications.DbSelectedDate.ToString());
                objAdultIE.HIVRelatedHistory = rcbSelectedValue(rcbHIVRelatedHistory);
                objAdultIE.InitialCD4 = GettxtValue(txtInitialCD4.Text);
                objAdultIE.InitialCD4Percent = GettxtValue(txtInitialCD4Percent.Text);
                objAdultIE.InitialCD4Date = DateGiven(dtInitialCD4Date.DbSelectedDate.ToString());
                objAdultIE.HighestCD4Ever = GettxtValue(txtHighestCD4Ever.Text);
                objAdultIE.HighestCD4Percent = GettxtValue(txtHighestCD4Percent.Text);
                objAdultIE.HighestCD4EverDate = DateGiven(dtHighestCD4Date.DbSelectedDate.ToString());
                objAdultIE.CD4atARTInitiation = GettxtValue(txtCD4atARTinitiation.Text);
                objAdultIE.CD4atARTInitiationDate = DateGiven(dtCD4atARTinitiationDate.DbSelectedDate.ToString());
                objAdultIE.CD4AtARTInitiationPercent = GettxtValue(txtCD4PercentAtARTInitiation.Text);
                objAdultIE.MostRecentCD4 = GettxtValue(txtMostRecentCD4.Text);
                objAdultIE.MostRecentCD4Percent = GettxtValue(txtMostRecentCD4Percent.Text);
                objAdultIE.MostRecentCD4Date = DateGiven(dtMostRecentCD4Date.DbSelectedDate.ToString());
                objAdultIE.PreviousViralLoad = GettxtValue(txtPreviousViralLoad.Text);
                objAdultIE.PreviousViralLoadDate = DateGiven(dtPreviousViralLoadDate.DbSelectedDate.ToString());
                objAdultIE.OtherHIVRelatedHistory = txtOtherHIVRelatedHistory.Text;
                objAdultIE.ARVExposure = CheckedVaue(radbtnARVExposure.SelectedToggleState.Text.ToString());
                objAdultIE.PMTC1StartDate = DateGiven(dtPMTC1StartDate.DbSelectedDate.ToString());
                objAdultIE.PMTC1Regimen = txtPMTC1Regimen.Text;
                objAdultIE.PEP1Regimen = txtPEP1Regimen.Text;
                objAdultIE.PEP1StartDate = DateGiven(dtPEP1StartDate.DbSelectedDate.ToString());
                objAdultIE.HAART1Regimen = txtHAART1Regimen.Text;
                objAdultIE.HAART1StartDate = DateGiven(dtHAART1StartDate.DbSelectedDate.ToString());
                objAdultIE.Impression = txtImpression.Text;
                objAdultIE.Diagnosis = 1; // MultiSelect
                objAdultIE.HIVRelatedOI = txtHIVRelatedOI.Text;
                objAdultIE.NonHIVRelatedOI = txtNonHIVRelatedOI.Text;
                objAdultIE.WHOStageIConditions = 1;// Line table
                objAdultIE.WHOStageIIConditions = 1;// Line Table
                objAdultIE.WHOStageIIIConditions = 1;// Line table
                objAdultIE.WHOStageIVConditions = 1;// Line Table
                objAdultIE.InitiationWHOstage = rcbSelectedValue(rcbInitiationWHOstage);
                objAdultIE.WHOStage = rcbSelectedValue(rcbWhoStage);
                objAdultIE.WABStage = rcbSelectedValue(rcbWABStage);
                objAdultIE.TannerStaging = rcbSelectedValue(rcbTannerStaging);
                objAdultIE.Mernarche = CheckedVaue(radbtnMernarche.SelectedToggleState.Text.ToString());
                objAdultIE.DrugAllergiesToxicitiesPaeds = 1;// Multi Select
                objAdultIE.SpecifyAntibioticAllery = txtSpecifyAntibioticAllery.Text;
                objAdultIE.OtherDrugAllergy = txtOtherDrugAllergy.Text;
                objAdultIE.ARVSideEffects = CheckedVaue(radbtnARVSideEffects.SelectedToggleState.Text.ToString());
                objAdultIE.ShortTermEffects = 1; // multiSelect
                objAdultIE.OtherShortTermEffects = txtOtherShortTermEffects.Text;
                objAdultIE.LongTermEffects = 1;// Multiple select 
                objAdultIE.OtherLongtermEffects = "";
                objAdultIE.WorkUpPlan = txtWorkUpPlan.Text;
                objAdultIE.LabEvaluationPeads = CheckedVaue(radbtnLabEvaluationPeads.SelectedToggleState.Text.ToString());
                objAdultIE.SpecifyLabEvaluation = 1;// MultiSelect
                objAdultIE.Counselling = 1;// MultiSelect
                objAdultIE.OtherCounselling = txtOtherCounselling.Text;
                objAdultIE.WardAdmission = CheckedVaue(radbtnWardAdmission.SelectedToggleState.Text.ToString());
                objAdultIE.ReferToSpecialistClinic = txtReferToSpecialistClinic.Text;
                objAdultIE.TransferOut = txtTransferOut.Text;
                objAdultIE.ARTTreatmentPlanPeads = rcbSelectedValue(rcbARTTreatmentPlanPeads);
                objAdultIE.SwitchReason = 1;// MultiSelect
                objAdultIE.StartART = CheckedVaue(radbtnStartART.SelectedToggleState.Text.ToString());
                objAdultIE.ARTEligibilityCriteria = 1;// MultiSelect
                objAdultIE.OtherARTEligibilityCriteria = txtOtherARTEligibilityCriteria.Text;
                objAdultIE.SubstituteRegimen = CheckedVaue(radbtnSubstituteRegimen.SelectedToggleState.Text.ToString());
                objAdultIE.NumberDrugsSubstituted = rcbSelectedValue(rcbNumberDrugsSubstituted);
                objAdultIE.StopTreatment = CheckedVaue(radbtnStopTreatment.SelectedToggleState.Text.ToString());
                objAdultIE.StopTreatmentCodes = 1;// MultiSelect
                objAdultIE.RegimenPrescribed = rcbSelectedValue(rcbRegimenPrescribed);
                objAdultIE.OtherRegimenPrescribed = txtOtherRegimenPrescribed.Text;
                objAdultIE.OIProphylaxis = rcbSelectedValue(rcbOIProphylaxis);
                objAdultIE.ReasonCTXPrescribed = rcbSelectedValue(rcbReasonCTXPrescribed);
                objAdultIE.OtherTreatment = txtOtherTreatment.Text;
                objAdultIE.SexualActiveness = CheckedVaue(radbtnSexualActiveness.SelectedToggleState.Text.ToString());
                objAdultIE.SexualOrientation = rcbSelectedValue(rcbSexualOrientation);
                objAdultIE.HighRisk = 1;// rcbHighRisk
                objAdultIE.KnowSexualPartnerHIVStatus = CheckedVaue(radbtnKnowSexualPartnerHIVStatus.SelectedToggleState.Text.ToString());
                objAdultIE.ParnerHIVStatus = rcbSelectedValue(rcbPartnerHIVStatus);
                objAdultIE.GivenPWPMessages = CheckedVaue(radbtnGivenPWPMessages.SelectedToggleState.Text.ToString());
                objAdultIE.SaferSexImportanceExplained = CheckedVaue(radbtnGivenPWPMessages.SelectedToggleState.Text.ToString());
                objAdultIE.UnsafeSexImportanceExplained = CheckedVaue(radbtnUnsafeSexImportanceExplained.SelectedToggleState.Text.ToString());
                objAdultIE.PDTDone = CheckedVaue(radbtnPDTDone.SelectedToggleState.Text.ToString());
                objAdultIE.Pregnant = CheckedVaue(radbtnPregnant.SelectedToggleState.Text.ToString());
                objAdultIE.PMTCTOffered = CheckedVaue(radbtnPMTCTOffered.SelectedToggleState.Text.ToString());
                objAdultIE.IntentionOfPregnancy = CheckedVaue(radbtnIntentionOfPregnancy.SelectedToggleState.Text.ToString());
                objAdultIE.DiscussedFertilityOptions = CheckedVaue(radbtnDiscussedFertilityOptions.SelectedToggleState.Text.ToString());
                objAdultIE.DiscussedDualContraception = CheckedVaue(radbtnDiscussedDualContraception.SelectedToggleState.Text.ToString());
                objAdultIE.CondomsIssued = CheckedVaue(radbtnCondomsIssued.SelectedToggleState.Text.ToString());
                objAdultIE.CondomNotIssued = txtCondomNotIssued.Text;
                objAdultIE.STIScreened = CheckedVaue(radbtnSTIScreened.SelectedToggleState.Text.ToString());
                objAdultIE.VaginalDischarge = CheckedVaue(radbtnVaginalDischarge.SelectedToggleState.Text.ToString());
                objAdultIE.UrethralDischarge = CheckedVaue(radbtnUrethralDischarge.SelectedToggleState.Text.ToString());
                objAdultIE.GenitalUlceration = CheckedVaue(radbtnGenitalUlceration.SelectedToggleState.Text.ToString());
                objAdultIE.STITreatmentPlan = txtSTITreatmentPlan.Text;
                objAdultIE.OnFP = CheckedVaue(radbtnOnFP.SelectedToggleState.Text.ToString());
                objAdultIE.FPMethod = rcbSelectedValue(rcbFPMethod);
                objAdultIE.CervicalCancerScreened = CheckedVaue(radbtnCervicalCancerScreened.SelectedToggleState.Text.ToString());
                objAdultIE.CervicalCancerScreeningResults = rcbSelectedValue(rcbCervicalCancerScreeningResults);
                objAdultIE.ReferredForCervicalCancerScreening = CheckedVaue(radbtnReferredForCervicalCancerScreening.SelectedToggleState.Text.ToString());
                objAdultIE.HPVOffered = CheckedVaue(radbtnHPVOffered.SelectedToggleState.Text.ToString());
                objAdultIE.OfferedHPVVaccine = rcbSelectedValue(rcbOfferedHPVVaccine);
                objAdultIE.HPVDoseDate = DateGiven(dtHPVDoseDate.DbSelectedDate.ToString());
                objAdultIE.RefferedToFupF = 1;// Multi select
                objAdultIE.SpecifyOtherRefferedTo = txtSpecifyOtherRefferedTo.Text;
                list.Add(objAdultIE);
                IQTouchKNHAdultIE theExpressManager;
                theExpressManager = (IQTouchKNHAdultIE)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchKNHAdultIE, BusinessProcess.Clinical");
                int result = theExpressManager.IQTouchSaveAdultIE(list);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackBtnClick();", true);


                }
            }
            catch (Exception ex)
            {
                isError = true;
                error = ex.Message.ToString();

            }
            if (isError)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('"+error+"')", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "clsLoadingPanelDueToError", "parent.CloseLoading();", true);
            }



        }


      





       

    }
}
