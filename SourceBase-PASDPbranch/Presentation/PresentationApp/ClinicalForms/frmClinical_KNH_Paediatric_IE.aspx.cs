using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Interface.Security;
using Application.Presentation;
using Application.Common;
using Interface.Clinical;
using System.Text;
using Interface.Administration;
using System.Drawing;
using PresentationApp.ClinicalForms.UserControl;

namespace PresentationApp.ClinicalForms
{

    public partial class frmClinical_KNH_Paediatric_IE : System.Web.UI.Page
    {
        IPatientKNHPEP KNHPEP;
        DataSet dsBind;
        static String startTime;
        IKNHStaticForms KNHStatic;
        public int triageTabID, featureID, clinicalHistoryTabId, tbScreeningTabId, examinationTabId, managementTabId, pwpTabId = 0;
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindExistingData();
            }
            getAllTabId();
            checkIfPreviuosTabSaved();
        }
        void tabControl_Handler()
        {
            tabControl.ActiveTabIndex = 3;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            UcTBScreening.btnHandler += new UserControlKNH_TBScreening.OnButtonClick(tabControl_Handler);
            KNHStatic = (IKNHStaticForms)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BKNHStaticForms, BusinessProcess.Clinical");
            if (!IsPostBack)
            {
                startTime = String.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now);
                hdnCurrentTabId.Value = tabControl.ActiveTab.ID;
                hdnPrevTabId.Value = tabControl.ActiveTab.ID;
                hdnCurrenTabName.Value = tabControl.ActiveTab.HeaderText;
                hdnPrevTabName.Value = tabControl.ActiveTab.HeaderText;
                ViewState["ActiveTabIndex"] = tabControl.ActiveTabIndex;
                hdnPrevTabIndex.Value = Convert.ToString(tabControl.ActiveTabIndex);
                hdnCurrenTabIndex.Value = Convert.ToString(tabControl.ActiveTabIndex);
                this.UcWhostaging.hiddateshow.Value = "SHOW";
                BindControl();
                //BindHidfortab();
                //Validate();
                ShowHideBusinessRules();
                getVisitId();
            }
            GblIQCare.FormId = 174;
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Text = "Clinical Forms >> ";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "KNH Paediatric IE";
            (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblformname") as Label).Text = "Paediatric Initial Evaluation";
        }
        private void getAllTabId()
        {
            KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
            //DataTable dtTabId = KNHPEP.GetTabID(Convert.ToInt32(ApplicationAccess.KNHPaediatricInitialEvaulation));
            /********** getting the data from XML file *******************************/
            DataSet XMLDS = new DataSet();
            XMLDS.ReadXml(MapPath("..\\XMLFiles\\AllMasters.con"));
            DataView theDV = new DataView(XMLDS.Tables["Mst_FormBuilderTab"]);
            DataTable dtTabId = theDV.ToTable();
            featureID = ApplicationAccess.KNHPaediatricInitialEvaulation;
            if (dtTabId.Rows.Count > 0)
            {
                foreach (DataRow drTabsId in dtTabId.Rows)
                {
                    if (drTabsId["TabName"].ToString() == "PaediatricIETriage")
                        triageTabID = Convert.ToInt32(drTabsId["TabId"]);
                    if (drTabsId["TabName"].ToString() == "PaediatricIEClinicalHistory")
                        clinicalHistoryTabId = Convert.ToInt32(drTabsId["TabId"]);
                    if (drTabsId["TabName"].ToString() == "PaediatricIETBScreening")
                        tbScreeningTabId = Convert.ToInt32(drTabsId["TabId"]);
                    if (drTabsId["TabName"].ToString() == "PaediatricIEExamination")
                        examinationTabId = Convert.ToInt32(drTabsId["TabId"]);
                    if (drTabsId["TabName"].ToString() == "PaediatricIEManagement")
                        managementTabId = Convert.ToInt32(drTabsId["TabId"]);
                    if (drTabsId["TabName"].ToString() == "PaediatricIEPwP")
                        pwpTabId = Convert.ToInt32(drTabsId["TabId"]);
                }
            }
        }
        public void ShowHideBusinessRules()
        {
            if (Convert.ToDecimal(Session["PatientAge"]) >= 3 && Convert.ToDecimal(Session["PatientAge"]) < 24)
                visibleDiv("divschoolingstatusy");

            if (Convert.ToDecimal(Session["PatientAge"]) >= 5)
                visibleDiv("divschoolperformancey");

            if (Convert.ToDecimal(Session["PatientAge"]) >= 10)
                visibleDiv("divtannerstagingy");

            if (Convert.ToDecimal(Session["PatientAge"]) >= 8 && Session["PatientSex"].ToString() == "Female")
                visibleDiv("divmernarcheshowy");
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            // Save(0);
        }
        private void visibleDiv(String divId)
        {
            String script = "";
            script = "<script language = 'javascript' defer ='defer' id = '" + divId + "'>\n";
            script += "ShowHide('" + divId + "','show');\n";
            script += "</script>\n";
            RegisterStartupScript("'" + divId + "'", script);
        }
        private void getVisitId()
        {
            //Created by-Nidhi (Start)
            //Desc- getting the visitid based on patientId coz PED IE is single visit form
            KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
            DataSet dsVisitId = KNHPEP.getVisitIdByPatient(Convert.ToInt32(Session["PatientId"].ToString()));
            if (dsVisitId.Tables[0].Rows.Count > 0)
            {
                Session["PatientVisitId"] = dsVisitId.Tables[0].Rows[0]["Visit_Id"];
            }
            //End
        }
        //get the added values from db and bind with controls
        public void BindExistingData()
        {
            getVisitId();
            string script = string.Empty;
            if (Convert.ToInt32(Session["PatientVisitId"].ToString()) > 0)
            {
                hdnVisitId.Value = Session["PatientVisitId"].ToString();
                KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
                DataSet dsGet = KNHPEP.GetPaediatric_IE(Convert.ToInt32(Session["PatientId"].ToString()), Convert.ToInt32(Session["PatientVisitId"].ToString()));
                if (dsGet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drRow in dsGet.Tables[32].Rows)
                    {
                        for (int i = 0; i < cblDiagnosis.Items.Count; i++)
                        {
                            if (Convert.ToInt32(drRow["ValueId"]) == Convert.ToInt32(cblDiagnosis.Items[i].Value))
                            {
                                cblDiagnosis.Items[i].Selected = true;
                                if (drRow["Name"].ToString().Trim() == "HIV-Related illness")
                                {
                                    visibleDiv("divHIV");
                                    txtHIVRelatedOI.Text = dsGet.Tables[0].Rows[0]["HIVRelatedOI"].ToString();
                                }
                                if (drRow["Name"].ToString().Trim() == "Non-HIV related illness")
                                {
                                    visibleDiv("divNonHIV");
                                    txtNonHIVRelatedOI.Text = dsGet.Tables[0].Rows[0]["NonHIVRelatedOI"].ToString();
                                }
                            }
                        }
                    }
                    //for vital control referred to list
                    CheckBoxList cblReferredTo = (CheckBoxList)this.idVitalSign.FindControl("cblReferredTo");
                    TextBox txtReferToSpecialistClinic = (TextBox)this.idVitalSign.FindControl("txtReferToSpecialistClinic");
                    TextBox txtSpecifyOtherRefferedTo = (TextBox)this.idVitalSign.FindControl("txtSpecifyOtherRefferedTo");
                    foreach (DataRow drRow in dsGet.Tables[33].Rows)
                    {
                        for (int j = 0; j <= dsGet.Tables[33].Rows.Count - 1; j++)
                        {
                            for (int i = 0; i < cblReferredTo.Items.Count; i++)
                            {
                                if (Convert.ToInt32(drRow["ValueId"]) == Convert.ToInt32(cblReferredTo.Items[i].Value))
                                {
                                    cblReferredTo.Items[i].Selected = true;
                                    if (drRow["Name"].ToString().Trim() == "Other Specialist Clinic")
                                    {
                                        visibleDiv("TriagedivReferToSpecialistClinic");
                                        txtReferToSpecialistClinic.Text = dsGet.Tables[0].Rows[0]["PatientReferredOtherSpecialistClinic"].ToString();
                                    }
                                    if (drRow["Name"].ToString().Trim() == "Other (Specify)")
                                    {
                                        visibleDiv("TriagedivReferToOther");
                                        txtSpecifyOtherRefferedTo.Text = dsGet.Tables[0].Rows[0]["PatientReferredOtherSpecify"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    /****************************************/
                    if (dsGet.Tables[0].Rows[0]["HAARTImpression"] != DBNull.Value)
                    {
                        ddlHAARTImpression.SelectedValue = dsGet.Tables[0].Rows[0]["HAARTImpression"].ToString();
                        if (ddlHAARTImpression.SelectedItem.Text == "Other specify")
                        {
                            visibleDiv("divSpecifyotherimpression");
                            txtOtherHAARTImpression.Text = dsGet.Tables[0].Rows[0]["OtherHAARTImpression"].ToString();
                        }
                    }
                    /*******Management***************/

                    if (dsGet.Tables[35].Rows.Count > 0)
                    {
                        if (dsGet.Tables[35].Rows[0]["TherapyPlan"] != DBNull.Value)
                        {
                            UcPharmacy.ddlTreatmentplan.SelectedValue = dsGet.Tables[35].Rows[0]["TherapyPlan"].ToString();
                            if (UcPharmacy.ddlTreatmentplan.SelectedItem.Text.ToLower() == "switch to second line")
                            {
                                UcPharmacy.ddlReasonforswitchto2ndlineregimen.SelectedValue = dsGet.Tables[35].Rows[0]["reasonForSwitchTo2ndLineRegimen"].ToString();
                                visibleDiv("divReasonforswitchto2ndlineregimen");
                            }
                            /*********** ART stop code :*******************/
                            if (UcPharmacy.ddlTreatmentplan.SelectedItem.Text.ToLower() == "stop treatment")
                            {
                                if (dsGet.Tables[38].Rows.Count > 0)
                                {
                                    foreach (DataRow row in dsGet.Tables[38].Rows)
                                    {
                                        for (int i = 0; i < this.UcPharmacy.chklistARTstopcode.Items.Count; i++)
                                        {
                                            if (Convert.ToInt32(this.UcPharmacy.chklistARTstopcode.Items[i].Value) == Convert.ToInt32(row["ValueId"]))
                                            {
                                                UcPharmacy.chklistARTstopcode.Items[i].Selected = true;
                                                if (UcPharmacy.chklistARTstopcode.Items[i].Text.ToLower() == "other patient decisi")
                                                {
                                                    visibleDiv("divARTstopcodeother");
                                                    UcPharmacy.txtSpecifyOtherStopCode.Text = dsGet.Tables[35].Rows[0]["specifyOtherStopCode"].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                                visibleDiv("divARTstopcode");
                            }
                        }
                        /*******Eligible through :********/
                        if (UcPharmacy.ddlTreatmentplan.SelectedItem.Text.ToLower() == "start new treatment (naive patient)")
                        {
                            if (dsGet.Tables[36].Rows.Count > 0)
                            {
                                foreach (DataRow row in dsGet.Tables[36].Rows)
                                {
                                    for (int i = 0; i < this.UcPharmacy.chklistEligiblethrough.Items.Count; i++)
                                    {
                                        if (Convert.ToInt32(this.UcPharmacy.chklistEligiblethrough.Items[i].Value) == Convert.ToInt32(row["ValueId"]))
                                        {
                                            UcPharmacy.chklistEligiblethrough.Items[i].Selected = true;
                                            if (UcPharmacy.chklistEligiblethrough.Items[i].Text.ToLower() == "other")
                                            {
                                                visibleDiv("divOtherEligibility");
                                                UcPharmacy.txtSpecifyOtherEligibility.Text = dsGet.Tables[35].Rows[0]["specifyOtherEligibility"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            visibleDiv("divEligiblethrough");
                        }

                        /*******ART change code :********/
                        if (UcPharmacy.ddlTreatmentplan.SelectedItem.Text.ToLower() == "change regimen")
                        {
                            UcPharmacy.txtNoofdrugssubstituted.Text = dsGet.Tables[35].Rows[0]["NoOfDrugsSubstituted"].ToString();
                            if (dsGet.Tables[37].Rows.Count > 0)
                            {
                                foreach (DataRow row in dsGet.Tables[37].Rows)
                                {
                                    for (int i = 0; i < this.UcPharmacy.chklistARTchangecode.Items.Count; i++)
                                    {
                                        if (Convert.ToInt32(this.UcPharmacy.chklistARTchangecode.Items[i].Value) == Convert.ToInt32(row["ValueId"]))
                                        {
                                            UcPharmacy.chklistARTchangecode.Items[i].Selected = true;
                                            if (UcPharmacy.chklistARTchangecode.Items[i].Text.ToLower() == "other")
                                            {
                                                visibleDiv("divSpecifyotherARTchangereason");
                                                UcPharmacy.txtSpecifyotherARTchangereason.Text = dsGet.Tables[35].Rows[0]["specifyOtherARTChangeReason"].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            visibleDiv("divARTchangecode");
                        }
                        if (dsGet.Tables[35].Rows[0]["reasonForSwitchTo2ndLineRegimen"] != DBNull.Value)
                            UcPharmacy.ddlReasonforswitchto2ndlineregimen.SelectedValue = dsGet.Tables[35].Rows[0]["reasonForSwitchTo2ndLineRegimen"].ToString();
                    }

                    //------------------------------section client information
                    if (dsGet.Tables[0].Rows[0]["visitdate"] != DBNull.Value)
                        txtVisitDate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["visitdate"]);

                    txtchildaccompaniedby.Text = dsGet.Tables[0].Rows[0]["ChildAccompaniedBy"].ToString();

                    if (dsGet.Tables[0].Rows[0]["ChildDiagnosisConfirmed"] != DBNull.Value)
                        ddlchilddiagnosis.SelectedValue = dsGet.Tables[0].Rows[0]["ChildDiagnosisConfirmed"].ToString();

                    txtchildcaregiver.Text = dsGet.Tables[0].Rows[0]["PrimaryCareGiver"].ToString();


                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["DisclosureStatus"].ToString()))
                    {
                        ddldisclosurestatus.SelectedValue = dsGet.Tables[0].Rows[0]["DisclosureStatus"].ToString();
                        if (ddldisclosurestatus.SelectedItem.Text == "Not ready")
                        {
                            visibleDiv("divreasonnotdisclosed");
                        }
                        if (ddldisclosurestatus.SelectedItem.Text == "Other specify")
                        {
                            visibleDiv("divotherdisclosurestatus");
                        }
                    }

                    txtspecifyreason.Text = dsGet.Tables[0].Rows[0]["ReasonNotDisclosed"].ToString();
                    txtspecifyotherdisclosurestatus.Text = dsGet.Tables[0].Rows[0]["OtherDisclosureReason"].ToString();
                    //For Mother
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["MotherAlive"].ToString()))
                    {
                        rbListMotherAlive.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["MotherAlive"])) ? "1" : "0";
                        txtdateofmotherdeath.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["DateOfDeathMother"]);
                        if (rbListMotherAlive.SelectedValue == "0")
                            visibleDiv("divdateofmotherdeath");
                    }
                    //For Father 
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["FatherAlive"].ToString()))
                    {
                        rbListFatherAlive.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["FatherAlive"])) ? "1" : "0";
                        txtdateoffatherdeath.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["DateOfDeathFather"]);
                        if (rbListFatherAlive.SelectedValue == "0")
                            visibleDiv("divdatefatherdeath");
                    }
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["ChildReferred"].ToString()))
                    {
                        rbListRefFacility.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["ChildReferred"])) ? "1" : "0";
                        txtspecifyfacility.Text = dsGet.Tables[0].Rows[0]["ChildReferredFrom"].ToString();
                        if (rbListRefFacility.SelectedValue == "1")
                            visibleDiv("divspecifyfacility");
                    }
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["CurrentlyOnHAART"].ToString()))
                    {
                        rbListChildART.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["CurrentlyOnHAART"])) ? "1" : "0";
                        if (rbListChildART.SelectedValue == "1")
                        {
                            visibleDiv("divcurrentregimenline");
                            visibleDiv("divARTRegimen");
                            visibleDiv("divARTDate");
                        }
                    }
                    if (dsGet.Tables[0].Rows[0]["CurrentARTRegimenLine"] != DBNull.Value)
                        ddlcurrentregimenline.SelectedValue = dsGet.Tables[0].Rows[0]["CurrentARTRegimenLine"].ToString();

                    if (dsGet.Tables[0].Rows[0]["CurrentARTRegimen"] != DBNull.Value)
                        ddlcurrentartregimen.SelectedValue = dsGet.Tables[0].Rows[0]["CurrentARTRegimen"].ToString();

                    if (dsGet.Tables[0].Rows[0]["CurrentARTRegimenDate"] != DBNull.Value)
                        txtcurrentartregimendate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["CurrentARTRegimenDate"]);

                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["CurrentlyOnCTX"].ToString()))
                        rbListChildCTX.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["CurrentlyOnCTX"])) ? "1" : "0";

                    if (dsGet.Tables[0].Rows[0]["SchoolingStatus"] != DBNull.Value)
                    {
                        ddlschoolingstatus.SelectedValue = dsGet.Tables[0].Rows[0]["SchoolingStatus"].ToString();
                        if (ddlschoolingstatus.SelectedIndex == 1)
                        {
                            visibleDiv("divhighestlevel");
                        }
                    }

                    if (dsGet.Tables[0].Rows[0]["HighestLevelAttained"] != DBNull.Value)
                        ddlhighestlevelattained.SelectedValue = dsGet.Tables[0].Rows[0]["HighestLevelAttained"].ToString();

                    //16 Ju
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["HealthEducation"].ToString()))
                    {
                        rbListHealthEducation.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["HealthEducation"])) ? "1" : "0";
                    }
                    //16 Ju
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["HIVSupportGroup"].ToString()))
                    {
                        rbListSupportGroup.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["HIVSupportGroup"])) ? "1" : "0";
                        if (rbListSupportGroup.SelectedValue == "1")
                            visibleDiv("divsupportgroup");
                    }
                    if (dsGet.Tables[0].Rows[0]["HIVSupportGroupMembership"] != DBNull.Value)
                        txthivsupportgroupmembership.Text = dsGet.Tables[0].Rows[0]["HIVSupportGroupMembership"].ToString();
                    //------vital sign

                    if (dsGet.Tables[0].Rows[0]["WeightForAge"] != DBNull.Value)
                        this.idVitalSign.ddlweightforage.SelectedValue = dsGet.Tables[0].Rows[0]["WeightForAge"].ToString();

                    if (dsGet.Tables[3].Rows.Count > 0)
                    {
                        if (dsGet.Tables[3].Rows[0]["HeadCircumference"] != DBNull.Value)
                            this.idVitalSign.txtheadcircumference.Text = dsGet.Tables[3].Rows[0]["HeadCircumference"].ToString();
                    }
                    if (dsGet.Tables[0].Rows[0]["WeightForHeight"] != DBNull.Value)
                        this.idVitalSign.ddlweightforheight.SelectedValue = dsGet.Tables[0].Rows[0]["WeightForHeight"].ToString();

                    if (dsGet.Tables[0].Rows[0]["NursesComments"] != DBNull.Value)
                        this.idVitalSign.txtnursescomments.Text = dsGet.Tables[0].Rows[0]["NursesComments"].ToString();

                    //If schooling,current school perfomance :
                    if (dsGet.Tables[0].Rows[0]["SchoolPerfomance"] != DBNull.Value)
                        ddlschoolperfomance.SelectedValue = dsGet.Tables[0].Rows[0]["SchoolPerfomance"].ToString();

                    //-------------Medical history (Disease, diagnosis and treatment)
                    // Medical history
                    if (dsGet.Tables[0].Rows[0]["MedicalHistory"] != System.DBNull.Value)
                    {
                        if (dsGet.Tables[0].Rows[0]["MedicalHistory"].ToString() == "True")
                        {
                            this.rdomedicalhistoryyes.Checked = true;

                        }
                        else if (dsGet.Tables[0].Rows[0]["MedicalHistory"].ToString() == "False")
                        {
                            this.rdomedicalhistoryno.Checked = true;
                        }
                    }
                    //Specify Medical history :
                    if (dsGet.Tables[0].Rows[0]["OtherMedicalHistorySpecify"] != DBNull.Value)
                        txtspecifymedicalhistory.Text = dsGet.Tables[0].Rows[0]["OtherMedicalHistorySpecify"].ToString();

                    //Specify other chronic condition
                    if (dsGet.Tables[0].Rows[0]["OtherChronicCondition"] != DBNull.Value)
                        txtotherchroniccondition.Text = dsGet.Tables[0].Rows[0]["OtherChronicCondition"].ToString();

                    //Previously admitted :
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["PreviousAdmission"].ToString()))
                    {
                        rbListPreviouslyAdmitt.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["PreviousAdmission"])) ? "1" : "0";
                        if (rbListPreviouslyAdmitt.SelectedValue == "1")
                        {
                            visibleDiv("divdiagnosis");
                            visibleDiv("divadmissionstart");
                            visibleDiv("divadmissionend");
                            // Diagnosis :
                            if (dsGet.Tables[0].Rows[0]["PreviousAdmissionDiagnosis"] != DBNull.Value)
                                txtdiagnosis.Text = dsGet.Tables[0].Rows[0]["PreviousAdmissionDiagnosis"].ToString();

                            //Admission start 
                            if (dsGet.Tables[0].Rows[0]["PreviousAdmissionStart"] != DBNull.Value)
                                txtadmissionstart.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["PreviousAdmissionStart"]);

                            //Admission end
                            if (dsGet.Tables[0].Rows[0]["PreviousAdmissionEnd"] != DBNull.Value)
                                txtadmissionend.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["PreviousAdmissionEnd"]);
                        }
                    }


                    //----------------TB History--------------------------
                    //TB History :
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["TBHistory"].ToString()))
                    {
                        rbListTBHistory.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["TBHistory"])) ? "1" : "0";
                        if (rbListTBHistory.SelectedValue == "1")
                        {
                            visibleDiv("divcompletetxdate");
                            visibleDiv("divretreatmentdate");
                        }
                    }
                    //Complete TX Date
                    if (dsGet.Tables[0].Rows[0]["TBrxCompleteDate"] != DBNull.Value)
                        txtcompletetxdate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["TBrxCompleteDate"]);

                    //Retreatment Date
                    if (dsGet.Tables[0].Rows[0]["TBRetreatmentDate"] != DBNull.Value)
                        txtretreatmentdate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["TBRetreatmentDate"]);

                    //----------------------Immunisation Status----------------
                    //Immunisation Status
                    if (dsGet.Tables[0].Rows[0]["ImmunisationStatus"] != DBNull.Value)
                        ddlimmunisationstatus.SelectedValue = dsGet.Tables[0].Rows[0]["ImmunisationStatus"].ToString();

                    //PMTCT
                    if (dsGet.Tables[0].Rows[0]["PMTCT1StartDate"] != DBNull.Value)
                        txtpmtctdate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["PMTCT1StartDate"]);

                    // PMTCT Regimen :
                    if (dsGet.Tables[0].Rows[0]["PMTCT1Regimen"] != DBNull.Value)
                        txtpmtctregimen.Text = dsGet.Tables[0].Rows[0]["PMTCT1Regimen"].ToString();

                    //HAART Start date
                    if (dsGet.Tables[0].Rows[0]["HAART1StartDate"] != DBNull.Value)
                        txthaartstartdate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["HAART1StartDate"]);

                    // HAART Regimen :
                    if (dsGet.Tables[0].Rows[0]["HAART1Regimen"] != DBNull.Value)
                        txthaartregimen.Text = dsGet.Tables[0].Rows[0]["HAART1Regimen"].ToString();

                    //PEP Start date
                    if (dsGet.Tables[0].Rows[0]["PEP1StartDate"] != DBNull.Value)
                        txtpepstartdate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["PEP1StartDate"]);

                    //PEP Regimen
                    if (dsGet.Tables[0].Rows[0]["PEP1Regimen"] != DBNull.Value)
                        txtpepregimen.Text = dsGet.Tables[0].Rows[0]["PEP1Regimen"].ToString();


                    //Initial CD4 :
                    if (dsGet.Tables[0].Rows[0]["InitialCD4"] != DBNull.Value)
                        txtcd4.Text = dsGet.Tables[0].Rows[0]["InitialCD4"].ToString();

                    // Initial CD4% 
                    if (dsGet.Tables[0].Rows[0]["InitialCD4Percent"] != DBNull.Value)
                        txtcd4per.Text = dsGet.Tables[0].Rows[0]["InitialCD4Percent"].ToString();

                    //Initial CD4 date :
                    if (dsGet.Tables[0].Rows[0]["InitialCD4Date"] != DBNull.Value)
                        txtcd4date.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["InitialCD4Date"]);

                    //Highest CD4 ever
                    if (dsGet.Tables[0].Rows[0]["HighestCD4Ever"] != DBNull.Value)
                        txthighCD4ever.Text = dsGet.Tables[0].Rows[0]["HighestCD4Ever"].ToString();

                    //Highest CD4 ever % :
                    if (dsGet.Tables[0].Rows[0]["HighestCD4Percent"] != DBNull.Value)
                        txthighestcd4everper.Text = dsGet.Tables[0].Rows[0]["HighestCD4Percent"].ToString();

                    //Highest CD4 ever date
                    if (dsGet.Tables[0].Rows[0]["HighestCD4EverDate"] != DBNull.Value)
                        txthigcd4everdate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["HighestCD4EverDate"]);

                    //CD4 at ART initiation :
                    if (dsGet.Tables[0].Rows[0]["CD4atARTInitiation"] != DBNull.Value)
                        txtcd4artinitiation.Text = dsGet.Tables[0].Rows[0]["CD4atARTInitiation"].ToString();

                    //CD4 at ART initiation %
                    if (dsGet.Tables[0].Rows[0]["CD4AtARTInitiationPercent"] != DBNull.Value)
                        txtcd4artinitper.Text = dsGet.Tables[0].Rows[0]["CD4AtARTInitiationPercent"].ToString();

                    //CD4 at ART initiation date :
                    if (dsGet.Tables[0].Rows[0]["CD4atARTInitiationDate"] != DBNull.Value)
                        txtcd4artinitdate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["CD4atARTInitiationDate"]);

                    //Most recent CD4
                    if (dsGet.Tables[0].Rows[0]["MostRecentCD4"] != DBNull.Value)
                        txtmostrecent_cd4.Text = dsGet.Tables[0].Rows[0]["MostRecentCD4"].ToString();

                    //Most Recent CD4% :
                    if (dsGet.Tables[0].Rows[0]["MostRecentCD4Percent"] != DBNull.Value)
                        txtmostrecentcd4per.Text = dsGet.Tables[0].Rows[0]["MostRecentCD4Percent"].ToString();

                    //Most recent CD4 date :
                    if (dsGet.Tables[0].Rows[0]["MostRecentCD4Date"] != DBNull.Value)
                        txtmostrecentcd4date.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["MostRecentCD4Date"]);

                    // Previous viral load :
                    if (dsGet.Tables[0].Rows[0]["PreviousViralLoad"] != DBNull.Value)
                        txtpreviousviral_load.Text = dsGet.Tables[0].Rows[0]["PreviousViralLoad"].ToString();

                    //Previous viral load date 
                    if (dsGet.Tables[0].Rows[0]["PreviousViralLoadDate"] != DBNull.Value)
                        txtpreviousviralloaddate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["PreviousViralLoadDate"]);

                    //Other HIV related history :
                    if (dsGet.Tables[0].Rows[0]["OtherHIVRelatedHistory"] != DBNull.Value)
                        txtotherhivrelated_history.Text = dsGet.Tables[0].Rows[0]["OtherHIVRelatedHistory"].ToString();


                    //pyridoxine 5mg\kg and INH 10mg/kg :
                    if (dsGet.Tables[0].Rows[0]["ReminderIPT"] != System.DBNull.Value)
                    {
                        if (dsGet.Tables[0].Rows[0]["ReminderIPT"].ToString() == "True")
                        {
                            this.chkpyridoxine.Checked = true;
                        }
                    }
                    //-----------------Physical Examination ?------------------------
                    if (dsGet.Tables[0].Rows[0]["OtherGeneralConditions"] != DBNull.Value)
                    {
                        //visibleDiv("divgeneralothercondition");
                        this.UcPE.txtOtherGeneralConditions.Text = dsGet.Tables[0].Rows[0]["OtherGeneralConditions"].ToString();
                    }
                    if (dsGet.Tables[0].Rows[0]["OtherAbdomenConditions"] != DBNull.Value)
                    {
                        //visibleDiv("divOtherAbdomenConditions");
                        this.UcPE.txtOtherAbdomenConditions.Text = dsGet.Tables[0].Rows[0]["OtherAbdomenConditions"].ToString();
                    }
                    if (dsGet.Tables[0].Rows[0]["OtherCardiovascularConditions"] != DBNull.Value)
                    {
                        //visibleDiv("divOtherCardiovascularConditions");
                        this.UcPE.txtOtherCardiovascularConditions.Text = dsGet.Tables[0].Rows[0]["OtherCardiovascularConditions"].ToString();
                    }

                    if (dsGet.Tables[0].Rows[0]["OtherOralCavityConditions"] != DBNull.Value)
                    {
                        //visibleDiv("divOtherOralCavityConditions");
                        this.UcPE.txtOtherOralCavityConditions.Text = dsGet.Tables[0].Rows[0]["OtherOralCavityConditions"].ToString();
                    }

                    if (dsGet.Tables[0].Rows[0]["OtherGenitourinaryConditions"] != DBNull.Value)
                    {
                       // visibleDiv("divOtherGenitourinaryConditions");
                        this.UcPE.txtOtherGenitourinaryConditions.Text = dsGet.Tables[0].Rows[0]["OtherGenitourinaryConditions"].ToString();
                    }

                    if (dsGet.Tables[0].Rows[0]["OtherCNSConditions"] != DBNull.Value)
                    {
                        //visibleDiv("divOtherCNSConditions");
                        this.UcPE.txtOtherCNSConditions.Text = dsGet.Tables[0].Rows[0]["OtherCNSConditions"].ToString();
                    }

                    if (dsGet.Tables[0].Rows[0]["OtherChestLungsConditions"] != DBNull.Value)
                    {
                        //visibleDiv("divOtherChestLungsConditions");
                        this.UcPE.txtOtherChestLungsConditions.Text = dsGet.Tables[0].Rows[0]["OtherChestLungsConditions"].ToString();
                    }

                    if (dsGet.Tables[0].Rows[0]["OtherSkinConditions"] != DBNull.Value)
                    {
                        //visibleDiv("divOtherSkinConditions");
                        this.UcPE.txtOtherSkinConditions.Text = dsGet.Tables[0].Rows[0]["OtherSkinConditions"].ToString();
                    }
                    if (dsGet.Tables[0].Rows[0]["OtherMedicalConditionNotes"] != DBNull.Value)
                        this.UcPE.txtOtherMedicalConditionNotes.Text = dsGet.Tables[0].Rows[0]["OtherMedicalConditionNotes"].ToString();

                    //----------------------Developmental milestones
                    // Milestone appropriate? :
                    if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["MilestoneAppropriate"].ToString()))
                    {
                        rbListMilestone.SelectedValue = (Convert.ToBoolean(dsGet.Tables[0].Rows[0]["MilestoneAppropriate"])) ? "1" : "0";
                        if (rbListMilestone.SelectedValue == "0")
                            visibleDiv("divmilestoneshowhide");
                        //If No specify why inappropriate
                        if (dsGet.Tables[0].Rows[0]["ResonMilestoneInappropriate"] != DBNull.Value)
                            txtspecifywhyinappropriate.Text = dsGet.Tables[0].Rows[0]["ResonMilestoneInappropriate"].ToString();

                    }
                    //if (dsGet.Tables[0].Rows[0]["MilestoneAppropriate"] != System.DBNull.Value)
                    //{
                    //    if (dsGet.Tables[0].Rows[0]["MilestoneAppropriate"].ToString() == "True")
                    //    {
                    //        this.rdomilestoneappropriateyes.Checked = true;
                    //        visibleDiv("divmilestoneshowhidey");
                    //    }
                    //    else if (dsGet.Tables[0].Rows[0]["MilestoneAppropriate"].ToString() == "False")
                    //    {
                    //        this.rdomilestoneappropriateno.Checked = true;
                    //        visibleDiv("divmilestoneshowhiden");
                    //    }
                    //}



                    //------------------- Tests and labs-------------------
                    // Lab evaluation 
                    if (dsGet.Tables[0].Rows[0]["LabEvaluationPeads"] != System.DBNull.Value)
                    {
                        if (dsGet.Tables[0].Rows[0]["LabEvaluationPeads"].ToString() == "True")
                        {
                            this.rdolabevaluationyes.Checked = true;
                            visibleDiv("divlabevaluatinshowhidey");
                        }
                        else if (dsGet.Tables[0].Rows[0]["LabEvaluationPeads"].ToString() == "False")
                        {
                            this.rdolabevaluationno.Checked = true;
                        }
                    }

                    TextBox txtlabdiagnosticinput = (TextBox)UCLabEval.FindControl("txtlabdiagnosticinput");
                    if (dsGet.Tables[0].Rows[0]["LabEvaluationDiagnosticInput"] != DBNull.Value)
                        txtlabdiagnosticinput.Text = dsGet.Tables[0].Rows[0]["LabEvaluationDiagnosticInput"].ToString();

                    //-------------  Drug Allergy and Toxicities ?---------------
                    //Specify ARV allergy 
                    if (dsGet.Tables[0].Rows[0]["SpecifyARVallergy"] != DBNull.Value)
                        txtarvallergy.Text = dsGet.Tables[0].Rows[0]["SpecifyARVallergy"].ToString();

                    //Specify antibiotic allergy 
                    if (dsGet.Tables[0].Rows[0]["SpecifyAntibioticAllery"] != DBNull.Value)
                        txtantibioticallergy.Text = dsGet.Tables[0].Rows[0]["SpecifyAntibioticAllery"].ToString();

                    //Specify other drug allergy
                    if (dsGet.Tables[0].Rows[0]["OtherDrugAllergy"] != DBNull.Value)
                        txtotherdrugallergy.Text = dsGet.Tables[0].Rows[0]["OtherDrugAllergy"].ToString();

                    //Specify other short term effects\
                    if (dsGet.Tables[0].Rows[0]["OtherShortTermEffects"] != DBNull.Value)
                    {
                        //visibleDiv("divshorttermeffecttxt");
                        txtspecityothershortterm.Text = dsGet.Tables[0].Rows[0]["OtherShortTermEffects"].ToString();
                    }
                    // Specify Other long term effects 
                    if (dsGet.Tables[0].Rows[0]["OtherLongtermEffects"] != DBNull.Value)
                    {
                        //visibleDiv("divlongtermeffecttxt");
                        txtspecifyotherlongterm.Text = dsGet.Tables[0].Rows[0]["OtherLongtermEffects"].ToString();
                    }
                    //Work up plan :
                    if (dsGet.Tables[0].Rows[0]["WorkUpPlan"] != DBNull.Value)
                        txtworkupplan.Text = dsGet.Tables[0].Rows[0]["WorkUpPlan"].ToString();

                    //OI Prophylaxis
                    if (dsGet.Tables[0].Rows[0]["OIProphylaxis"] != DBNull.Value)
                    {
                        ddloiprophylaxis.SelectedValue = dsGet.Tables[0].Rows[0]["OIProphylaxis"].ToString(); //dsGet.Tables[0].Rows[0]["RegimenPrescribed"].ToString();
                        if (ddloiprophylaxis.SelectedItem.Text == "Cotrimoxazole")
                        {
                            visibleDiv("divoiprophylaxisshowhidey");
                        }
                        if (ddloiprophylaxis.SelectedItem.Text == "Fluconazole")
                        {
                            visibleDiv("divFluconazoleshowhide");
                            //Fluconazole prescribed for :
                            if (dsGet.Tables[0].Rows[0]["ReasonFluconazolepresribed"] != DBNull.Value)
                                ddlfluconazole.SelectedValue = dsGet.Tables[0].Rows[0]["ReasonFluconazolepresribed"].ToString();
                        }
                        if (ddloiprophylaxis.SelectedItem.Text == "CTX and Fluconazol")
                        {
                            visibleDiv("divFluconazoleshowhide");
                            //Cotrimoxazole prescribed for
                            if (dsGet.Tables[0].Rows[0]["ReasonCTXpresribed"] != DBNull.Value)
                                ddlcotrimoxazole.SelectedValue = dsGet.Tables[0].Rows[0]["ReasonCTXpresribed"].ToString();
                        }
                        if (ddloiprophylaxis.SelectedItem.Text == "Other")
                        {
                            visibleDiv("divoiprophylasixothershowhide");
                            //Other (Specify)
                            if (dsGet.Tables[0].Rows[0]["OtherOIProphylaxis"] != DBNull.Value)
                                txtothercotrimoxazole.Text = dsGet.Tables[0].Rows[0]["OtherOIProphylaxis"].ToString();
                        }
                    }
                    this.UcPc.txtAdditionPresentingComplaints.Text = dsGet.Tables[0].Rows[0]["PresentingComplaintsAdditionalNotes"].ToString();

                    //Other treatment :
                    if (dsGet.Tables[0].Rows[0]["OtherTreatment"] != DBNull.Value)
                        txtothertreatementcatrimozazole.Text = dsGet.Tables[0].Rows[0]["OtherTreatment"].ToString();
                }

                if (dsGet.Tables[1].Rows.Count > 0)
                {
                    if (dsGet.Tables[1].Rows[0]["ConfirmHIVPosDate"] != DBNull.Value)
                        txtdateofhivdiagnosis.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[1].Rows[0]["ConfirmHIVPosDate"]);

                }
                //---------------section vital sign-------------------
                if (dsGet.Tables[3].Rows.Count > 0)
                {
                    if (dsGet.Tables[3].Rows[0]["Temp"] != DBNull.Value)
                        this.idVitalSign.txtTemp.Text = dsGet.Tables[3].Rows[0]["Temp"].ToString();
                    if (dsGet.Tables[3].Rows[0]["RR"] != DBNull.Value)
                        this.idVitalSign.txtRR.Text = dsGet.Tables[3].Rows[0]["RR"].ToString();
                    if (dsGet.Tables[3].Rows[0]["HR"] != DBNull.Value)
                        this.idVitalSign.txtHR.Text = dsGet.Tables[3].Rows[0]["HR"].ToString();
                    if (dsGet.Tables[3].Rows[0]["BPDiastolic"] != DBNull.Value)
                        this.idVitalSign.txtBPDiastolic.Text = dsGet.Tables[3].Rows[0]["BPDiastolic"].ToString();
                    if (dsGet.Tables[3].Rows[0]["BPSystolic"] != DBNull.Value)
                        this.idVitalSign.txtBPSystolic.Text = dsGet.Tables[3].Rows[0]["BPSystolic"].ToString();
                    if (dsGet.Tables[3].Rows[0]["Height"] != DBNull.Value)
                        this.idVitalSign.txtHeight.Text = dsGet.Tables[3].Rows[0]["Height"].ToString();
                    if (dsGet.Tables[3].Rows[0]["Weight"] != DBNull.Value)
                        this.idVitalSign.txtWeight.Text = dsGet.Tables[3].Rows[0]["Weight"].ToString();

                    visibleDiv("hideVitalYNy");

                    if (dsGet.Tables[3].Rows[0]["Height"] != DBNull.Value && dsGet.Tables[3].Rows[0]["Weight"] != DBNull.Value)
                    {
                        decimal bmi = Convert.ToDecimal(this.idVitalSign.txtWeight.Text) / (Convert.ToDecimal(this.idVitalSign.txtHeight.Text) / 100 * Convert.ToDecimal(this.idVitalSign.txtHeight.Text) / 100);
                        this.idVitalSign.txtBMI.Text = Convert.ToString(Math.Round(bmi, 2));

                    }
                }
                FillCheckboxlist(cblchroniccondition, dsGet.Tables[5], "ChronicCondition");
                //FillCheckboxlist(chkLongTermMedication, dsGet.Tables[34], "PaedCurrentLongTermMedications");
                foreach (DataRow drRow in dsGet.Tables[34].Rows)
                {
                    //for (int j = 0; j <= dsGet.Tables[33].Rows.Count - 1; j++)
                    //{
                    for (int i = 0; i < chkLongTermMedication.Items.Count; i++)
                    {
                        if (Convert.ToInt32(drRow["ValueId"]) == Convert.ToInt32(chkLongTermMedication.Items[i].Value))
                        {
                            chkLongTermMedication.Items[i].Selected = true;
                            if (drRow["Name"].ToString().Trim() == "Other")
                            {
                                visibleDiv("divLongTermMedication");
                                txOtherLongTermMedications.Text = dsGet.Tables[0].Rows[0]["OtherCurrentLongTermMedications"].ToString();
                            }
                        }
                    }
                    // }
                }
                //UcWhostaging.ddlWABStage.SelectedItem.Value = dsGet.Tables[0].Rows[0]["WABStage"].ToString();
                //UcWhostaging.ddltannerstaging.SelectedItem.Value = dsGet.Tables[0].Rows[0]["TannerStaging"].ToString();

                FillCheckboxlist(cbllabevaluation, dsGet.Tables[9], "SpecifyLabEvaluation");
                FillCheckboxlist(cblDrugAllergiesToxicitiesPaeds, dsGet.Tables[10], "DrugAllergiesToxicitiesPaeds");
                FillCheckboxlist(cblshorttermeffects, dsGet.Tables[11], "ShortTermEffects");
                FillCheckboxlist(cbllongtermeffects, dsGet.Tables[12], "LongTermEffects");

                if (dsGet.Tables[19].Rows.Count > 0)
                {
                    ViewState["presentingcomplains"] = dsGet.Tables[19];
                    visibleDiv("hidePresentingYNy");
                }
                //binding the data in Presenting user control 
                bindPresentingComplaint(dsGet);

                TextBox txtAdditionalComplaints = (TextBox)UcPc.FindControl("txtAdditionalComplaints");
                txtAdditionalComplaints.Text = dsGet.Tables[0].Rows[0]["otherpresentingcomplaints"].ToString();

                CheckBoxList chkPEGeneral = (CheckBoxList)this.UcPE.FindControl("cblGeneralConditions");
                FillCheckboxlist(chkPEGeneral, dsGet.Tables[20], "GeneralConditions");
                //Cardiovascular conditions
                CheckBoxList chkPECardiovascular = (CheckBoxList)this.UcPE.FindControl("cblCardiovascularConditions");
                FillCheckboxlist(chkPECardiovascular, dsGet.Tables[21], "CardiovascularConditions");
                //CNS 
                CheckBoxList chkPECNS = (CheckBoxList)this.UcPE.FindControl("cblCNSConditions");
                FillCheckboxlist(chkPECNS, dsGet.Tables[22], "CNSConditions");
                //Oral cavity
                CheckBoxList chkPEOralCavity = (CheckBoxList)this.UcPE.FindControl("cblOralCavityConditions");
                FillCheckboxlist(chkPEOralCavity, dsGet.Tables[23], "OralCavityConditions");
                //Chest Lungs
                CheckBoxList chkPEChestLungs = (CheckBoxList)this.UcPE.FindControl("cblChestLungsConditions");
                FillCheckboxlist(chkPEChestLungs, dsGet.Tables[24], "ChestLungsConditions");
                //Genitourinary 
                CheckBoxList chkPEGenitourinary = (CheckBoxList)this.UcPE.FindControl("cblGenitalUrinaryConditions");
                FillCheckboxlist(chkPEGenitourinary, dsGet.Tables[25], "GenitalUrinaryConditions");
                //Skin 
                CheckBoxList chkPESkin = (CheckBoxList)this.UcPE.FindControl("cblSkinConditions");
                FillCheckboxlist(chkPESkin, dsGet.Tables[26], "SkinConditions");
                //Abdomen conditions
                CheckBoxList chkPEabdomen = (CheckBoxList)this.UcPE.FindControl("cblAbdomenConditions");
                FillCheckboxlist(chkPEabdomen, dsGet.Tables[27], "AbdomenConditions");

                ////WHO Stage I CurrentWHOStageIConditions
                //Bind the values in user control WHO Stage 1
                if (dsGet.Tables[28].Rows.Count > 0)
                {
                    foreach (DataRow row in dsGet.Tables[28].Rows)
                    {
                        for (int i = 0; i < this.UcWhostaging.gvWHO1.Rows.Count; i++)
                        {
                            Label lblWHOId = (Label)UcWhostaging.gvWHO1.Rows[i].FindControl("lblwho1");
                            CheckBox chkWHOId = (CheckBox)UcWhostaging.gvWHO1.Rows[i].FindControl("Chkwho1");
                            HtmlInputText txtWHODate1 = (HtmlInputText)UcWhostaging.gvWHO1.Rows[i].FindControl("txtCurrentWho1Date");
                            HtmlInputText txtWHODate2 = (HtmlInputText)UcWhostaging.gvWHO1.Rows[i].FindControl("txtCurrentWho1Date1");
                            if (Convert.ToInt32(row["ValueId"]) == Convert.ToInt32(lblWHOId.Text))
                            {
                                chkWHOId.Checked = true;
                                txtWHODate1.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField1"]);
                                txtWHODate2.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField2"]);
                            }
                        }
                    }
                }
                ////WHO Stage 2
                //Bind the values in user control WHO Stage 2
                if (dsGet.Tables[29].Rows.Count > 0)
                {
                    foreach (DataRow row in dsGet.Tables[29].Rows)
                    {
                        for (int i = 0; i < this.UcWhostaging.gvWHO2.Rows.Count; i++)
                        {
                            Label lblWHOId = (Label)UcWhostaging.gvWHO2.Rows[i].FindControl("lblwho2");
                            CheckBox chkWHOId = (CheckBox)UcWhostaging.gvWHO2.Rows[i].FindControl("Chkwho2");
                            HtmlInputText txtWHODate1 = (HtmlInputText)UcWhostaging.gvWHO2.Rows[i].FindControl("txtCurrentWho2Date");
                            HtmlInputText txtWHODate2 = (HtmlInputText)UcWhostaging.gvWHO2.Rows[i].FindControl("txtCurrentWho2Date1");
                            if (Convert.ToInt32(row["ValueId"]) == Convert.ToInt32(lblWHOId.Text))
                            {
                                chkWHOId.Checked = true;
                                txtWHODate1.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField1"]);
                                txtWHODate2.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField2"]);
                            }
                        }
                    }
                }
                ////WHO Stage 3
                //Bind the values in user control WHO Stage 3
                if (dsGet.Tables[30].Rows.Count > 0)
                {
                    foreach (DataRow row in dsGet.Tables[30].Rows)
                    {
                        for (int i = 0; i < this.UcWhostaging.gvWHO3.Rows.Count; i++)
                        {
                            Label lblWHOId = (Label)UcWhostaging.gvWHO3.Rows[i].FindControl("lblwho3");
                            CheckBox chkWHOId = (CheckBox)UcWhostaging.gvWHO3.Rows[i].FindControl("Chkwho3");
                            HtmlInputText txtWHODate1 = (HtmlInputText)UcWhostaging.gvWHO3.Rows[i].FindControl("txtCurrentWho3Date");
                            HtmlInputText txtWHODate2 = (HtmlInputText)UcWhostaging.gvWHO3.Rows[i].FindControl("txtCurrentWho3Date1");
                            if (Convert.ToInt32(row["ValueId"]) == Convert.ToInt32(lblWHOId.Text))
                            {
                                chkWHOId.Checked = true;
                                txtWHODate1.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField1"]);
                                txtWHODate2.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField2"]);
                            }
                        }
                    }
                }
                ////WHO Stage 4
                //Bind the values in user control WHO Stage 4
                if (dsGet.Tables[31].Rows.Count > 0)
                {
                    foreach (DataRow row in dsGet.Tables[31].Rows)
                    {
                        for (int i = 0; i < this.UcWhostaging.gvWHO4.Rows.Count; i++)
                        {
                            Label lblWHOId = (Label)UcWhostaging.gvWHO4.Rows[i].FindControl("lblwho4");
                            CheckBox chkWHOId = (CheckBox)UcWhostaging.gvWHO4.Rows[i].FindControl("Chkwho4");
                            HtmlInputText txtWHODate1 = (HtmlInputText)UcWhostaging.gvWHO4.Rows[i].FindControl("txtCurrentWho4Date");
                            HtmlInputText txtWHODate2 = (HtmlInputText)UcWhostaging.gvWHO4.Rows[i].FindControl("txtCurrentWho4Date1");
                            if (Convert.ToInt32(row["ValueId"]) == Convert.ToInt32(lblWHOId.Text))
                            {
                                chkWHOId.Checked = true;
                                txtWHODate1.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField1"]);
                                txtWHODate2.Value = String.Format("{0:dd-MMM-yyyy}", row["DateField2"]);
                            }
                        }
                    }
                }
                //------------Staging at initial evaluation ?

                if (dsGet.Tables[0].Rows[0]["InitiationWHOstage"] != DBNull.Value)
                    UcWhostaging.ddlInitiationWHOstage.SelectedValue = dsGet.Tables[0].Rows[0]["InitiationWHOstage"].ToString();

                //HIV associated conditions
                if (dsGet.Tables[0].Rows[0]["HIVAssociatedConditionsPeads"] != DBNull.Value)
                    UcWhostaging.ddlhivassociated.SelectedValue = dsGet.Tables[0].Rows[0]["HIVAssociatedConditionsPeads"].ToString();

                //HIV associated conditions
                if (dsGet.Tables[2].Rows.Count > 0)
                {
                    if (dsGet.Tables[2].Rows[0]["WhoStage"] != DBNull.Value)
                        UcWhostaging.ddlwhostage1.SelectedValue = dsGet.Tables[2].Rows[0]["WhoStage"].ToString();
                    //WHO Stage at initiation (Transfer in)
                    if (dsGet.Tables[2].Rows[0]["WABStage"] != DBNull.Value)
                        UcWhostaging.ddlWABStage.SelectedValue = dsGet.Tables[2].Rows[0]["WABStage"].ToString();

                }
                //Tanner staging
                if (dsGet.Tables[0].Rows[0]["TannerStaging"] != DBNull.Value)
                    UcWhostaging.ddltannerstaging.SelectedValue = dsGet.Tables[0].Rows[0]["TannerStaging"].ToString();

               

                //Mernarche 16 ju
                if (dsGet.Tables[0].Rows[0]["Menarche"] != System.DBNull.Value)
                {
                    if (dsGet.Tables[0].Rows[0]["Menarche"].ToString() == "True")
                    {
                        UcWhostaging.radbtnMernarcheyes.Checked = true;
                        UcWhostaging.radbtnMernarcheno.Checked = false;
                        visibleDiv("divmenarchedatehshowhide");
                        ////Menarche Date :
                        if (!String.IsNullOrEmpty(dsGet.Tables[0].Rows[0]["MenarcheDate"].ToString()))
                            UcWhostaging.txtmenarchedate.Value = String.Format("{0:dd-MMM-yyyy}", dsGet.Tables[0].Rows[0]["MenarcheDate"]);

                    }
                    else if (dsGet.Tables[0].Rows[0]["Menarche"].ToString() == "False")
                    {
                        UcWhostaging.radbtnMernarcheyes.Checked = false;
                        UcWhostaging.radbtnMernarcheno.Checked = true;
                    }
                }
            }
            else
            {
                hdnVisitId.Value = "0";
                txtVisitDate.Value = Application["AppCurrentDate"].ToString();
            }
        }
        private void bindPresentingComplaint(DataSet ds)
        {
            for (int j = 0; j <= ds.Tables[19].Rows.Count - 1; j++)
            {
                for (int i = 0; i < this.UcPc.gvPresentingComplaints.Rows.Count; i++)
                {
                    Label lblPComplaintsId = (Label)UcPc.gvPresentingComplaints.Rows[i].FindControl("lblPresenting");
                    CheckBox chkPComplaints = (CheckBox)UcPc.gvPresentingComplaints.Rows[i].FindControl("ChkPresenting");
                    TextBox txtPComplaints = (TextBox)UcPc.gvPresentingComplaints.Rows[i].FindControl("txtPresenting");
                    if (Convert.ToInt32(ds.Tables[19].Rows[j]["ValueId"]) == Convert.ToInt32(lblPComplaintsId.Text))
                    {
                        if (ds.Tables[19].Rows[j]["Name"].ToString().ToLower() == "other")
                        {
                            visibleDiv("DivOther");
                        }
                        chkPComplaints.Checked = true;
                        txtPComplaints.Text = ds.Tables[19].Rows[j]["Other_notes"].ToString();
                    }
                }
            }
        }
        public void FillCheckboxlist(CheckBoxList chk, DataTable dt, string name)
        {
            string script = string.Empty;
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < chk.Items.Count; j++)
                    {
                        if (chk.Items[j].Value == dt.Rows[i]["ValueID"].ToString())
                        {
                            chk.Items[j].Selected = true;
                            
                            if (name == "GeneralConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divgeneralothercondition");
                                }
                            }
                            if (name == "CardiovascularConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divOtherCardiovascularConditions");
                                }
                            }
                            if (name == "CNSConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divOtherCNSConditions");
                                }
                            }
                            if (name == "OralCavityConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divOtherOralCavityConditions");
                                }
                            }
                            if (name == "ChestLungsConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divOtherChestLungsConditions");
                                }
                            }
                            if (name == "GenitalUrinaryConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divOtherGenitourinaryConditions");
                                }
                            }
                            if (name == "AbdomenConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divOtherAbdomenConditions");
                                }
                            }
                            if (name == "SkinConditions")
                            {
                                if (chk.Items[j].Text.Trim().ToLower() == "other")
                                {
                                    visibleDiv("divOtherSkinConditions");
                                }
                            }
                            if (name == "ChronicCondition")
                            {
                                if (chk.Items[j].Text == "Other specify")
                                {
                                    visibleDiv("hideOtherLTM");
                                }
                            }
                            if (name == "TBSideEffects")
                            {
                                if (chk.Items[j].Text == "Other Side effects (specify)")
                                {
                                    visibleDiv("divothertbshowhideyes");
                                }
                            }
                            if (name == "DrugAllergiesToxicitiesPaeds")
                            {
                                if (chk.Items[j].Text == "ARV")
                                {
                                    visibleDiv("divspecifyarvallergyshowhideA");
                                }
                                if (chk.Items[j].Text == "Antibiotic")
                                {
                                    visibleDiv("divspecifyantibioticshowhideA");
                                }
                                if (chk.Items[j].Text == "Other")
                                {
                                    visibleDiv("divspecifyotherdrugshowhideA");
                                }
                            }
                            if (name == "LongTermEffects")
                            {
                                if (chk.Items[j].Text == "Other specify")
                                {
                                    visibleDiv("divlongtermeffecttxt");
                                }
                            }
                            if (name == "ShortTermEffects")
                            {
                                if (chk.Items[j].Text == "Other Specify")
                                {
                                    visibleDiv("divshorttermeffecttxt");
                                }
                            }
                            if (name == "Counselling")
                            {
                                if (chk.Items[j].Text == "Other")
                                {
                                    visibleDiv("divothercounsellingshowhidey");
                                }
                            }
                        }
                    }
                }

            }
        }
        private void triage_Save(int qltyFlag)
        {
            Hashtable theHT = new Hashtable();
            DataTable dtSave = CreateTempTable();
            try
            {
                theHT.Add("visitDate", txtVisitDate.Value);
                theHT.Add("patientID", Session["PatientId"]);
                theHT.Add("visitID", Session["PatientVisitId"]);
                theHT.Add("locationID", Session["AppLocationId"]);
                //Child accompanied by
                theHT.Add("ChildAccompaniedBy", txtchildaccompaniedby.Text);
                //Child Primary Caregiver
                theHT.Add("PrimaryCareGiver", txtchildcaregiver.Text);
                //Child diagnosis confirmed
                theHT.Add("ChildDiagnosisConfirmed", ddlchilddiagnosis.SelectedItem.Value);
                //Date of HIV Diagnosis
                theHT.Add("ConfirmHIVPosDate", txtdateofhivdiagnosis.Value);

                //Disclosure status
                theHT.Add("DisclosureStatus", ddldisclosurestatus.SelectedItem.Value);
                //Specify reason not discloesd
                theHT.Add("ReasonNotDisclosed", txtspecifyreason.Text);
                //Specify other disclosure status
                theHT.Add("OtherDisclosureReason", txtspecifyotherdisclosurestatus.Text);

                //Father alive
                theHT.Add("FatherAlive", rbListFatherAlive.SelectedValue);
                //Date of fathers death
                theHT.Add("DateOfDeathFather", txtdateoffatherdeath.Value);
                //Mother alive
                theHT.Add("MotherAlive", rbListMotherAlive.SelectedValue);
                //Date of mothers death
                theHT.Add("DateOfDeathMother", txtdateofmotherdeath.Value);
                //Have you been referred from another facility :
                //int referredanotherfacility = rdoreferedfromfacilityyes.Checked ? 1 : rdoreferedfromfacilityno.Checked ? 0 : 9;
                theHT.Add("ChildReferred", rbListRefFacility.SelectedValue);
                //Specify facility referred from :
                theHT.Add("ChildReferredFrom", txtspecifyfacility.Text);

                //Is the child on ART
                theHT.Add("CurrentlyOnHAART", rbListChildART.SelectedValue);
                //Current regimen line 
                theHT.Add("CurrentARTRegimenLine", ddlcurrentregimenline.SelectedItem.Value);
                //Current ART regimen
                theHT.Add("CurrentARTRegimen", ddlcurrentartregimen.SelectedItem.Value);
                //Current ART Regimen Began Date 
                theHT.Add("CurrentARTRegimenDate", txtcurrentartregimendate.Value);

                //Is the child on CTX :                
                theHT.Add("CurrentlyOnCTX", rbListChildCTX.SelectedValue);
                //Health education given?
                theHT.Add("HealthEducation", rbListHealthEducation.SelectedValue);
                //Schooling Status
                theHT.Add("SchoolingStatus", ddlschoolingstatus.SelectedItem.Value);
                //Highest level attained
                theHT.Add("HighestLevelAttained", ddlhighestlevelattained.SelectedItem.Value);
                //Is client a member of a support group
                theHT.Add("HIVSupportGroup", rbListSupportGroup.SelectedValue);
                //HIV support group membership
                theHT.Add("HIVSupportGroupMembership", txthivsupportgroupmembership.Text);

                //--------------section vital sign
                if (this.idVitalSign.txtTemp.Text != "")
                {
                    theHT.Add("Temperature", this.idVitalSign.txtTemp.Text.ToString());
                }
                else
                {
                    theHT.Add("Temperature", "0");
                }
                if (this.idVitalSign.txtRR.Text != "")
                {
                    theHT.Add("RespirationRate", this.idVitalSign.txtRR.Text);
                }
                else
                {
                    theHT.Add("RespirationRate", "0");
                }
                if (this.idVitalSign.txtHR.Text != "")
                {
                    theHT.Add("HeartRate", this.idVitalSign.txtHR.Text);
                }
                else
                {
                    theHT.Add("HeartRate", "0");
                }
                if (this.idVitalSign.txtHeight.Text != "")
                {
                    theHT.Add("Height", this.idVitalSign.txtHeight.Text);
                }
                else
                {
                    theHT.Add("Height", "0");
                }
                if (this.idVitalSign.txtWeight.Text != "")
                {
                    theHT.Add("Weight", this.idVitalSign.txtWeight.Text);
                }
                else
                {
                    theHT.Add("Weight", "0");
                }
                if (this.idVitalSign.txtBPDiastolic.Text != "")
                {
                    theHT.Add("DiastolicBloodPressure", this.idVitalSign.txtBPDiastolic.Text);
                }
                else
                {
                    theHT.Add("DiastolicBloodPressure", "0");
                }
                if (this.idVitalSign.txtBPSystolic.Text != "")
                {
                    theHT.Add("SystolicBloodPressure", this.idVitalSign.txtBPSystolic.Text);
                }
                else
                {
                    theHT.Add("SystolicBloodPressure", "0");
                }

                theHT.Add("BMI", 0);
                theHT.Add("HeadCircumference", this.idVitalSign.txtheadcircumference.Text);
                theHT.Add("WeightForAge", this.idVitalSign.ddlweightforage.SelectedValue);
                theHT.Add("WeightForHeight", this.idVitalSign.ddlweightforheight.SelectedValue);
                theHT.Add("NursesComments", this.idVitalSign.txtnursescomments.Text);
                theHT.Add("qltyFlag", qltyFlag);

                //Patient referred to:
                CheckBoxList cblReferredTo = (CheckBoxList)this.idVitalSign.FindControl("cblReferredTo");
                dtSave = GetCheckBoxListValues(cblReferredTo, dtSave, "PatientReferTo");
                theHT.Add("PatientReferredOtherSpecialistClinic", this.idVitalSign.txtReferToSpecialistClinic.Text);
                theHT.Add("PatientReferredOtherSpecify", this.idVitalSign.txtSpecifyOtherRefferedTo.Text);

                theHT.Add("startTime", startTime);
                theHT.Add("userID", Convert.ToInt32(Session["AppUserId"]));
                KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
                DataSet DsReturns = KNHPEP.SaveUpdatePaediatricIE_TriageTab(theHT, dtSave);
                if (DsReturns.Tables[0].Rows.Count > 0)
                {
                    Session["PatientVisitId"] = DsReturns.Tables[0].Rows[0]["Visit_Id"].ToString();
                }
                //string saveupdate = string.Empty;
                if (Convert.ToInt32(DsReturns.Tables[0].Rows[0]["Visit_Id"]) > 0)
                {
                    //if (Convert.ToInt32(Session["PatientVisitId"].ToString()) > 0)
                    //{
                    //    saveupdate = "Update";
                    //}
                    //else
                    //{
                    //    saveupdate = "Save";
                    //}
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "saveSuc", "alert('Data on Triage tab saved successfully.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data on Triage tab saved successfully.')", true);
                    BindExistingData();
                    tabControl.ActiveTabIndex = 1;
                }
            }
            catch (Exception err)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theMsg, this);
            }
        }
        private void clinicalHistory_Save(int qltyFlag)
        {
            Hashtable theHT = new Hashtable();
            DataTable dtSave = CreateTempTable();
            try
            {
                ////------Presenting Complaints------------------
                dtSave = PresentingComplaints(dtSave, "PresentingComplaints");
                //Presenting complaints additional notes
                theHT.Add("PresentingComplaintsAdditionalNotes", UcPc.txtAdditionPresentingComplaints.Text);
                TextBox txtAdditionalComplaints = (TextBox)UcPc.FindControl("txtAdditionalComplaints");
                theHT.Add("OtherPresentingComplaints", txtAdditionalComplaints.Text);
                //If schooling,current school perfomance :
                theHT.Add("SchoolPerfomance", ddlschoolperfomance.SelectedItem.Value);

                //-------------Medical history (Disease, diagnosis and treatment)
                // Medical history
                int medicalhistory = rdomedicalhistoryyes.Checked ? 1 : rdomedicalhistoryno.Checked ? 0 : 9;
                theHT.Add("MedicalHistory", medicalhistory);
                //Specify Medical history :
                theHT.Add("OtherMedicalHistorySpecify", txtspecifymedicalhistory.Text);
                //Chronic condition
                dtSave = GetCheckBoxListValues(cblchroniccondition, dtSave, "ChronicCondition");
                //Specify other chronic condition
                theHT.Add("OtherChronicCondition", txtotherchroniccondition.Text);

                //Previously admitted :
                //int previouslyadmitted = rdopreviouslyadmittedyes.Checked ? 1 : rdopreviouslyadmittedno.Checked ? 0 : 9;
                theHT.Add("PreviousAdmission", rbListPreviouslyAdmitt.SelectedValue);
                // Diagnosis :
                theHT.Add("PreviousAdmissionDiagnosis", txtdiagnosis.Text);
                //Admission start 
                theHT.Add("PreviousAdmissionStart", txtadmissionstart.Value);
                //Admission end
                theHT.Add("PreviousAdmissionEnd", txtadmissionend.Value);

                //----------------TB History--------------------------
                //TB History :
                //int tbhistory = rdotvhistoryyes.Checked ? 1 : rdotvhistoryno.Checked ? 0 : 9;
                theHT.Add("TBHistory", rbListTBHistory.SelectedValue);
                //Complete TX Date
                theHT.Add("TBrxCompleteDate", txtcompletetxdate.Value);
                //Retreatment Date
                theHT.Add("TBRetreatmentDate", txtretreatmentdate.Value);
                //----------------------Immunisation Status----------------
                //Immunisation Status
                theHT.Add("ImmunisationStatus", ddlimmunisationstatus.SelectedItem.Value);
                //---------------------ARV Exposure----------------------
                //PMTCT
                theHT.Add("PMTCT1StartDate", txtpmtctdate.Value);
                // PMTCT Regimen :
                theHT.Add("PMTCT1Regimen", txtpmtctregimen.Text);
                //HAART Start date
                theHT.Add("HAART1StartDate", txthaartstartdate.Value);
                // HAART Regimen :
                theHT.Add("HAART1Regimen", txthaartregimen.Text);
                //PEP Start date
                theHT.Add("PEP1StartDate", txtpepstartdate.Value);
                //PEP Regimen
                theHT.Add("PEP1Regimen", txtpepregimen.Text);
                //--------------------HIV Related History----------------
                //Initial CD4 :
                theHT.Add("InitialCD4", txtcd4.Text);
                // Initial CD4% 
                theHT.Add("InitialCD4Percent", txtcd4per.Text);
                //Initial CD4 date :
                theHT.Add("InitialCD4Date", txtcd4date.Value);
                //Highest CD4 ever
                theHT.Add("HighestCD4Ever", txthighCD4ever.Text);
                //Highest CD4 ever % :
                theHT.Add("HighestCD4Percent", txthighestcd4everper.Text);
                //Highest CD4 ever date
                theHT.Add("HighestCD4EverDate", txthigcd4everdate.Value);
                //CD4 at ART initiation :
                theHT.Add("CD4atARTInitiation", txtcd4artinitiation.Text);
                //CD4 at ART initiation %
                theHT.Add("CD4AtARTInitiationPercent", txtcd4artinitper.Text);
                //CD4 at ART initiation date :
                theHT.Add("CD4atARTInitiationDate", txtcd4artinitdate.Value);
                //Most recent CD4
                theHT.Add("MostRecentCD4", txtmostrecent_cd4.Text);
                //Most Recent CD4% :
                theHT.Add("MostRecentCD4Percent", txtmostrecentcd4per.Text);
                //Most recent CD4 date :
                theHT.Add("MostRecentCD4Date", txtmostrecentcd4date.Value);
                // Previous viral load :
                theHT.Add("PreviousViralLoad", txtpreviousviral_load.Text);
                //Previous viral load date 
                theHT.Add("PreviousViralLoadDate", txtpreviousviralloaddate.Value);
                //Other HIV related history :
                theHT.Add("OtherHIVRelatedHistory", txtotherhivrelated_history.Text);

                theHT.Add("visitDate", txtVisitDate.Value);
                theHT.Add("startTime", startTime);
                theHT.Add("userID", Convert.ToInt32(Session["AppUserId"]));
                theHT.Add("patientID", Session["PatientId"]);
                theHT.Add("visitID", Session["PatientVisitId"]);
                KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
                DataSet DsReturns = KNHPEP.SaveUpdatePaediatricIE_ClinicalHistoryTab(theHT, dtSave);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "saveSuc", "alert('Data on Clinical History tab saved successfully.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data on Clinical History tab saved successfully.')", true);
                BindExistingData();
                tabControl.ActiveTabIndex = 2;
            }
            catch (Exception err)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theMsg, this);
            }
        }
        private void examination_Save(int qltyFlag)
        {
            Hashtable theHT = new Hashtable();
            DataTable dtSave = CreateTempTable();
            try
            {
                //Logn Term
                dtSave = GetCheckBoxListValues(chkLongTermMedication, dtSave, "PaedCurrentLongTermMedications");
                //Diagnosis          
                dtSave = GetCheckBoxListValues(cblDiagnosis, dtSave, "Diagnosis");
                //LabEvaluation
                dtSave = GetCheckBoxListValues(cbllabevaluation, dtSave, "SpecifyLabEvaluation");
                CheckBoxList chkPEGeneral = (CheckBoxList)this.UcPE.FindControl("cblGeneralConditions");
                dtSave = GetCheckBoxListValues(chkPEGeneral, dtSave, "GeneralConditions");
                //Cardiovascular conditions
                CheckBoxList chkPECardiovascular = (CheckBoxList)this.UcPE.FindControl("cblCardiovascularConditions");
                dtSave = GetCheckBoxListValues(chkPECardiovascular, dtSave, "CardiovascularConditions");
                //CNS 
                CheckBoxList chkPECNS = (CheckBoxList)this.UcPE.FindControl("cblCNSConditions");
                dtSave = GetCheckBoxListValues(chkPECNS, dtSave, "CNSConditions");
                //Oral cavity
                CheckBoxList chkPEOralCavity = (CheckBoxList)this.UcPE.FindControl("cblOralCavityConditions");
                dtSave = GetCheckBoxListValues(chkPEOralCavity, dtSave, "OralCavityConditions");
                //Chest Lungs
                CheckBoxList chkPEChestLungs = (CheckBoxList)this.UcPE.FindControl("cblChestLungsConditions");
                dtSave = GetCheckBoxListValues(chkPEChestLungs, dtSave, "ChestLungsConditions");
                //Genitourinary 
                CheckBoxList chkPEGenitourinary = (CheckBoxList)this.UcPE.FindControl("cblGenitalUrinaryConditions");
                dtSave = GetCheckBoxListValues(chkPEGenitourinary, dtSave, "GenitalUrinaryConditions");
                //Skin 
                CheckBoxList chkPESkin = (CheckBoxList)this.UcPE.FindControl("cblSkinConditions");
                dtSave = GetCheckBoxListValues(chkPESkin, dtSave, "SkinConditions");
                //Abdomen conditions
                CheckBoxList chkPEabdomen = (CheckBoxList)this.UcPE.FindControl("cblAbdomenConditions");
                dtSave = GetCheckBoxListValues(chkPEabdomen, dtSave, "AbdomenConditions");
                //WHO Stage I
                GridView gdviewwho1 = (GridView)UcWhostaging.FindControl("gvWHO1");
                dtSave = InsertMultiSelectList(gdviewwho1, dtSave, "WHOStageIConditions");
                // WHO Stage II
                GridView gdviewwho2 = (GridView)UcWhostaging.FindControl("gvWHO2");
                dtSave = InsertMultiSelectList(gdviewwho2, dtSave, "WHOStageIIConditions");
                // WHO Stage III
                GridView gdviewwho3 = (GridView)UcWhostaging.FindControl("gvWHO3");
                dtSave = InsertMultiSelectList(gdviewwho3, dtSave, "WHOStageIIIConditions");
                // WHO Stage IV
                GridView gdviewwho4 = (GridView)UcWhostaging.FindControl("gvWHO4");
                dtSave = InsertMultiSelectList(gdviewwho4, dtSave, "WHOStageIVConditions");

                //-----------------Physical Examination ?------------------------
                theHT.Add("OtherCurrentLongTermMedications", txOtherLongTermMedications.Text);
                theHT.Add("OtherMedicalConditionNotes", this.UcPE.txtOtherMedicalConditionNotes.Text);
                theHT.Add("OtherGeneralConditions", this.UcPE.txtOtherGeneralConditions.Text);
                theHT.Add("OtherAbdomenConditions", this.UcPE.txtOtherAbdomenConditions.Text);
                theHT.Add("OtherCardiovascularConditions", this.UcPE.txtOtherCardiovascularConditions.Text);
                theHT.Add("OtherOralCavityConditions", this.UcPE.txtOtherOralCavityConditions.Text);
                theHT.Add("OtherGenitourinaryConditions", this.UcPE.txtOtherGenitourinaryConditions.Text);
                theHT.Add("OtherCNSConditions", this.UcPE.txtOtherCNSConditions.Text);
                theHT.Add("OtherChestLungsConditions", this.UcPE.txtOtherChestLungsConditions.Text);
                theHT.Add("OtherSkinConditions", this.UcPE.txtOtherSkinConditions.Text);
                theHT.Add("HAARTImpression", ddlHAARTImpression.SelectedValue);
                theHT.Add("Diagnosis", 0);
                theHT.Add("HIVRelatedOI", txtHIVRelatedOI.Text.Trim());
                theHT.Add("NonHIVRelatedOI", txtNonHIVRelatedOI.Text.Trim());
                theHT.Add("HAARTexperienced", "");
                theHT.Add("OtherHAARTImpression", txtOtherHAARTImpression.Text.Trim());

                TextBox txtlabdiagnosticinput = (TextBox)UCLabEval.FindControl("txtlabdiagnosticinput");
                theHT.Add("LabEvaluationDiagnosticInput", txtlabdiagnosticinput.Text);
                //----------------------Developmental milestones
                // Milestone appropriate? :
                //int milestone = rdomilestoneappropriateyes.Checked ? 1 : rdomilestoneappropriateno.Checked ? 0 : 9;
                theHT.Add("MilestoneAppropriate", rbListMilestone.SelectedValue);
                //If No specify why inappropriate
                theHT.Add("ResonMilestoneInappropriate", txtspecifywhyinappropriate.Text);

                //------------------- Tests and labs-------------------
                // Lab evaluation 
                int labevaluation = rdolabevaluationyes.Checked ? 1 : rdolabevaluationno.Checked ? 0 : 9;
                theHT.Add("LabEvaluationPeads", labevaluation);

                //------------Staging at initial evaluation ?
                //------------Staging at initial evaluation ?
                //WHO Stage at initiation (Transfer in)
                theHT.Add("InitiationWHOstage", UcWhostaging.ddlInitiationWHOstage.SelectedItem.Value);
                //HIV associated conditions
                theHT.Add("HIVAssociatedConditionsPeads", UcWhostaging.ddlhivassociated.SelectedItem.Value);
                //Current WHO Stage

                theHT.Add("PeadiatricNutritionAssessment", UcWhostaging.ddlwhostage1.SelectedItem.Value);
                //WAB Stage
                theHT.Add("WABStage", UcWhostaging.ddlWABStage.SelectedItem.Value);
                //Tanner staging
                theHT.Add("TannerStaging", UcWhostaging.ddltannerstaging.SelectedItem.Value);
                //Mernarche
                String mernache = UcWhostaging.radbtnMernarcheyes.Checked ? "1" : UcWhostaging.radbtnMernarcheno.Checked ? "0" : "NULL";
                theHT.Add("Menarche", mernache);
                //Menarche Date :
                theHT.Add("MenarcheDate", UcWhostaging.txtmenarchedate.Value);

                theHT.Add("visitDate", txtVisitDate.Value);
                theHT.Add("startTime", startTime);
                theHT.Add("userID", Convert.ToInt32(Session["AppUserId"]));
                theHT.Add("patientID", Session["PatientId"]);
                theHT.Add("visitID", Session["PatientVisitId"]);

                KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
                DataSet DsReturns = KNHPEP.SaveUpdatePaediatricIE_ExaminationTab(theHT, dtSave);
                //Session["Redirect"] = "0";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "saveSuc", "alert('Data on Examination tab saved successfully.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data on Examination tab saved successfully.')", true);
                BindExistingData();
                tabControl.ActiveTabIndex = 4;
            }
            catch (Exception err)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theMsg, this);
            }
        }
        private void management_Save(int qltyFlag)
        {
            Hashtable theHT = new Hashtable();
            DataTable dtSave = CreateTempTable();
            try
            {
                dtSave = GetCheckBoxListValues(cblshorttermeffects, dtSave, "ShortTermEffects");
                dtSave = GetCheckBoxListValues(cbllongtermeffects, dtSave, "LongTermEffects");
                /*********Treatment***************************************/
                dtSave = GetCheckBoxListValues(this.UcPharmacy.chklistEligiblethrough, dtSave, "ARTEligibility");
                dtSave = GetCheckBoxListValues(this.UcPharmacy.chklistARTchangecode, dtSave, "ARTchangecode");
                dtSave = GetCheckBoxListValues(this.UcPharmacy.chklistARTstopcode, dtSave, "ARTstopcode");
                //-------------  Drug Allergy and Toxicities ?---------------
                theHT.Add("OtherShortTermEffects", txtspecityothershortterm.Text);
                // Specify Other long term effects 
                theHT.Add("OtherLongtermEffects", txtspecifyotherlongterm.Text);
                theHT.Add("reasonForSwitchTo2ndLineRegimen", this.UcPharmacy.ddlReasonforswitchto2ndlineregimen.SelectedValue);
                theHT.Add("NumberDrugsSubstituted", this.UcPharmacy.txtNoofdrugssubstituted.Text);
                //Work up plan :
                theHT.Add("WorkUpPlan", txtworkupplan.Text);
                //OI Treatment
                theHT.Add("OIProphylaxis", ddloiprophylaxis.SelectedItem.Value);
                //Cotrimoxazole prescribed for
                theHT.Add("ReasonCTXpresribed", ddlcotrimoxazole.SelectedItem.Value);
                //Fluconazole prescribed for
                theHT.Add("ReasonFluconazolepresribed", ddlfluconazole.SelectedItem.Value);
                //Other (Specify)
                theHT.Add("OtherOIProphylaxis", txtothercotrimoxazole.Text);
                //Other treatment :
                theHT.Add("OtherTreatment", txtothertreatementcatrimozazole.Text);
                theHT.Add("startTime", startTime);
                theHT.Add("userID", Convert.ToInt32(Session["AppUserId"]));
                theHT.Add("patientID", Session["PatientId"]);
                theHT.Add("visitID", Session["PatientVisitId"]);
                theHT.Add("locationID", Session["AppLocationId"]);
                theHT.Add("treatmentPlan", UcPharmacy.ddlTreatmentplan.SelectedValue);
                theHT.Add("Noofdrugssubstituted", UcPharmacy.txtNoofdrugssubstituted.Text);
                theHT.Add("reasonforswitchto2ndlineregimen", UcPharmacy.ddlReasonforswitchto2ndlineregimen.SelectedValue);
                theHT.Add("specifyOtherEligibility", UcPharmacy.txtSpecifyOtherEligibility.Text);
                theHT.Add("specifyotherARTchangereason", UcPharmacy.txtSpecifyotherARTchangereason.Text);
                theHT.Add("specifyOtherStopCode", UcPharmacy.txtSpecifyOtherStopCode.Text);
                theHT.Add("visitDate", txtVisitDate.Value);
                KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
                DataSet DsReturns = KNHPEP.SaveUpdatePaediatricIE_ManagementTab(theHT, dtSave);
                //Session["Redirect"] = "0";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "saveSuc", "alert('Data on Management tab saved successfully.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Data on Management tab saved successfully.')", true);
                BindExistingData();
                tabControl.ActiveTabIndex = 5;
                //commanMSG(DsReturns, 5);
            }
            catch (Exception err)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theMsg, this);
            }
        }
        public void BindControl()
        {
            IQCareUtils iQCareUtils = new IQCareUtils();
            BindFunctions BindManager = new BindFunctions();
            KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
            dsBind = KNHPEP.GetDetailsPaediatric_IE();
            BindManager.BindCombo(ddlHAARTImpression, dsBind.Tables[55], "Name", "Id"); //"HAARTImpression");
            BindManager.BindCheckedList(chkLongTermMedication, dsBind.Tables[56], "Name", "Id");
            BindManager.BindCombo(ddlchilddiagnosis, dsBind.Tables[0], "Name", "Id");
            BindManager.BindCheckedList(cblDiagnosis, dsBind.Tables[54], "NAME", "ID");
            BindManager.BindCombo(ddldisclosurestatus, dsBind.Tables[1], "Name", "Id");
            //BindManager.BindCombo(ddldisclosurestatus, dsBind.Tables[1], "Name", "Id");
            BindManager.BindCombo(ddlcurrentregimenline, dsBind.Tables[3], "Name", "Id");
            BindManager.BindCombo(ddlcurrentartregimen, dsBind.Tables[4], "Name", "Id");
            BindManager.BindCombo(ddlschoolingstatus, dsBind.Tables[5], "Name", "Id");
            BindManager.BindCombo(ddlhighestlevelattained, dsBind.Tables[6], "Name", "Id");
            BindManager.BindCombo(this.idVitalSign.ddlweightforage, dsBind.Tables[7], "Name", "Id");
            BindManager.BindCombo(ddlschoolperfomance, dsBind.Tables[9], "Name", "Id");
            BindManager.BindCheckedList(cblchroniccondition, dsBind.Tables[10], "Name", "ID");
            BindManager.BindCombo(ddlimmunisationstatus, dsBind.Tables[11], "Name", "Id");
            BindManager.BindCheckedList(cbllabevaluation, dsBind.Tables[25], "Name", "ID");
            //BindManager.BindCombo(ddlInitiationWHOstage, dsBind.Tables[26], "Name", "Id");
            BindManager.BindCheckedList(cblDrugAllergiesToxicitiesPaeds, dsBind.Tables[31], "Name", "ID");
            BindManager.BindCombo(ddloiprophylaxis, dsBind.Tables[34], "Name", "Id");
            BindManager.BindCheckedList(cblshorttermeffects, dsBind.Tables[35], "Name", "ID");
            BindManager.BindCheckedList(cbllongtermeffects, dsBind.Tables[36], "Name", "ID");
            BindManager.BindCombo(ddlcotrimoxazole, dsBind.Tables[41], "Name", "Id");
            BindManager.BindCombo(ddlsingature, dsBind.Tables[52], "name", "EmployeeID");
            BindManager.BindCombo(ddlfluconazole, dsBind.Tables[53], "Name", "Id");
        }
        private DataTable PresentingComplaints(DataTable dtprescompl, string name)
        {

            GridView gdview = (GridView)UcPc.FindControl("gvPresentingComplaints");
            foreach (GridViewRow row in gdview.Rows)
            {
                DataRow dr = dtprescompl.NewRow();
                CheckBox chk = (CheckBox)row.FindControl("ChkPresenting");
                TextBox txt = (TextBox)row.FindControl("txtPresenting");
                Label lbl = (Label)row.FindControl("lblPresenting");
                if (chk.Checked)
                {
                    dr["ID"] = Convert.ToInt32(lbl.Text);
                    dr["FieldName"] = name;
                }
                if (txt.Text != "")
                {
                    dr["Other_Notes"] = txt.Text;
                }
                dtprescompl.Rows.Add(dr);
            }
            return dtprescompl;
        }
        private DataTable GetCheckBoxListValues(CheckBoxList chklist, DataTable dt, string name)
        {
            DataRow dr;
            for (int i = 0; i < chklist.Items.Count; i++)
            {
                if (chklist.Items[i].Selected)
                {
                    dr = dt.NewRow();
                    dr["ID"] = Convert.ToInt32(chklist.Items[i].Value);
                    dr["FieldName"] = name;
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }
        private DataTable InsertMultiSelectList(GridView gdview, DataTable dt, string fieldname)
        {
            foreach (GridViewRow row in gdview.Rows)
            {
                DataRow dr = dt.NewRow();

                if (fieldname == "WHOStageIConditions")
                {
                    CheckBox chk = (CheckBox)row.FindControl("Chkwho1");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)row.FindControl("txtCurrentWho1Date");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)row.FindControl("txtCurrentWho1Date1");
                    Label lbl = (Label)row.FindControl("lblwho1");

                    if (chk.Checked)
                    {
                        dr["ID"] = Convert.ToInt32(lbl.Text);
                        dr["FieldName"] = fieldname;
                        if (txtdt1.Value != "")
                        {
                            dr["DateField1"] = txtdt1.Value;
                        }
                        if (txtdt2.Value != "")
                        {
                            dr["DateField2"] = txtdt2.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (fieldname == "WHOStageIIConditions")
                {
                    CheckBox chk = (CheckBox)row.FindControl("Chkwho2");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)row.FindControl("txtCurrentWho2Date");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)row.FindControl("txtCurrentWho2Date1");
                    Label lbl = (Label)row.FindControl("lblwho2");

                    if (chk.Checked)
                    {
                        dr["ID"] = Convert.ToInt32(lbl.Text);
                        dr["FieldName"] = fieldname;
                        if (txtdt1.Value != "")
                        {
                            dr["DateField1"] = txtdt1.Value;
                        }
                        if (txtdt2.Value != "")
                        {
                            dr["DateField2"] = txtdt2.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (fieldname == "WHOStageIIIConditions")
                {
                    CheckBox chk = (CheckBox)row.FindControl("Chkwho3");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)row.FindControl("txtCurrentWho3Date");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)row.FindControl("txtCurrentWho3Date1");
                    Label lbl = (Label)row.FindControl("lblwho3");

                    if (chk.Checked)
                    {
                        dr["ID"] = Convert.ToInt32(lbl.Text);
                        dr["FieldName"] = fieldname;
                        if (txtdt1.Value != "")
                        {
                            dr["DateField1"] = txtdt1.Value;
                        }
                        if (txtdt2.Value != "")
                        {
                            dr["DateField2"] = txtdt2.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (fieldname == "WHOStageIVConditions")
                {
                    CheckBox chk = (CheckBox)row.FindControl("Chkwho4");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)row.FindControl("txtCurrentWho4Date");
                    System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)row.FindControl("txtCurrentWho4Date1");
                    Label lbl = (Label)row.FindControl("lblwho4");

                    if (chk.Checked)
                    {
                        dr["ID"] = Convert.ToInt32(lbl.Text);
                        dr["FieldName"] = fieldname;
                        if (txtdt1.Value != "")
                        {
                            dr["DateField1"] = txtdt1.Value;
                        }
                        if (txtdt2.Value != "")
                        {
                            dr["DateField2"] = txtdt2.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
        private DataTable CreateTempTable()
        {
            DataTable dtprescompl = new DataTable();

            DataColumn theID = new DataColumn("ID");
            theID.DataType = System.Type.GetType("System.Int32");
            dtprescompl.Columns.Add(theID);

            DataColumn theFieldName = new DataColumn("FieldName");
            theFieldName.DataType = System.Type.GetType("System.String");
            dtprescompl.Columns.Add(theFieldName);

            DataColumn theDateValue1 = new DataColumn("DateField1");
            theDateValue1.DataType = System.Type.GetType("System.DateTime");
            dtprescompl.Columns.Add(theDateValue1);

            DataColumn theDateValue2 = new DataColumn("DateField2");
            theDateValue2.DataType = System.Type.GetType("System.DateTime");
            dtprescompl.Columns.Add(theDateValue2);

            DataColumn theValue1 = new DataColumn("NumericField");
            theValue1.DataType = System.Type.GetType("System.String");
            dtprescompl.Columns.Add(theValue1);

            DataColumn theOther = new DataColumn("Other_Notes");
            theOther.DataType = System.Type.GetType("System.String");
            dtprescompl.Columns.Add(theOther);
            return dtprescompl;
        }
        public void checkIfPreviuosTabSaved()
        {
            /**********check previous values are inserted ************************/
            securityPertab("PaediatricIETriage");
            DataSet ds = KNHStatic.CheckIfPreviuosTabSaved("PaediatricIETriage", Convert.ToInt32(Session["PatientVisitId"]));
            buttonEnabledAndDisabled(ds, btnSaveCHistory, btnPrintCHistory, "PaediatricIEClinicalHistory");
        
            ds = KNHStatic.CheckIfPreviuosTabSaved("PaediatricIEClinicalHistory", Convert.ToInt32(Session["PatientVisitId"]));
            buttonEnabledAndDisabled(ds, UcTBScreening.btnTBSave, UcTBScreening.btnTBPrint, "PaediatricIETBScreening");

            ds = KNHStatic.CheckIfPreviuosTabSaved("PaediatricIETBScreening", Convert.ToInt32(Session["PatientVisitId"]));
            buttonEnabledAndDisabled(ds, btnSaveExam, btnPrintExam, "PaediatricIEExamination");
            
            ds = KNHStatic.CheckIfPreviuosTabSaved("PaediatricIEExamination", Convert.ToInt32(Session["PatientVisitId"]));
            buttonEnabledAndDisabled(ds, btnSaveManagement, btnPrintManagement, "PaediatricIEManagement");

            Button btnSave = (Button)UcPWP.FindControl("btnSave");
            Button btnSubmitPositive = (Button)UcPWP.FindControl("btnSubmitPositive");
            Button btnPrintPositive = (Button)UcPWP.FindControl("btnPrintPositive");
            ds = KNHStatic.CheckIfPreviuosTabSaved("PaediatricIEManagement", Convert.ToInt32(Session["PatientVisitId"]));
            buttonEnabledAndDisabled(ds, btnSave, btnPrintPositive, "PaediatricIEPwP");
            
            /*******************************************************/
            //For Signature
            DataTable dtSignature = KNHPEP.GetSignature(Convert.ToInt32(ApplicationAccess.KNHPaediatricInitialEvaulation), Convert.ToInt32(Session["PatientVisitId"]));
            if (dtSignature.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSignature.Rows)
                {
                    if (dr["TabName"].ToString() == "PaediatricIETriage")
                        this.UserControlKNHSignature_Triage.lblSignature.Text = dr["UserName"].ToString();
                    if (dr["TabName"].ToString() == "PaediatricIEClinicalHistory")
                        this.UserControlKNHSignature_ClinicalHistory.lblSignature.Text = dr["UserName"].ToString();
                    if (dr["TabName"].ToString() == "PaediatricIEExamination")
                        this.UserControlKNHSignature_Examination.lblSignature.Text = dr["UserName"].ToString();
                    if (dr["TabName"].ToString() == "PaediatricIEManagement")
                        this.UserControlKNHSignature_Management.lblSignature.Text = dr["UserName"].ToString();
                }
            }
        }
        private void securityPertab(String tabName)
        {
            //triage
            if (tabName == "PaediatricIETriage")
                UserRights(btnSaveTriage, btnPrintTriage, featureID, triageTabID);

            //Clinical History
            else if (tabName == "PaediatricIEClinicalHistory")
                UserRights(btnSaveCHistory, btnPrintCHistory, featureID, clinicalHistoryTabId);

            //TB Screening
            else if (tabName == "PaediatricIETBScreening")
                UserRights(UcTBScreening.btnTBSave, UcTBScreening.btnTBPrint, featureID, tbScreeningTabId);

            //Examination
            else if (tabName == "PaediatricIEExamination")
                UserRights(btnSaveExam,  btnPrintExam, featureID, examinationTabId);

            //Management
            else if (tabName == "PaediatricIEManagement")
                UserRights(btnSaveManagement,  btnPrintManagement, featureID, managementTabId);
            //Pwp
            Button btnSave = (Button)UcPWP.FindControl("btnSave");
            Button btnSubmitPositive = (Button)UcPWP.FindControl("btnSubmitPositive");
            Button btnPrintPositive = (Button)UcPWP.FindControl("btnPrintPositive");
            if (tabName == "PaediatricIEPwP")
            {
                UserRights(btnSave, btnPrintPositive, featureID, pwpTabId);
            }
        }
        private void buttonEnabledAndDisabled(DataSet ds, Button btnSave, Button btnPrint,String tabName)
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                btnSave.Enabled = false;
                btnPrint.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnPrint.Enabled = true;
                securityPertab(tabName);
            }
        }
        private void setTabIdAndColor(int tabId,Label lblControlName)
        {
            tabControl.ActiveTabIndex = tabId;
            lblControlName.ForeColor = Color.Red;
        }
        private Boolean fieldValidation_Triage()
        {
            if (this.idVitalSign.txtHeight.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValHeight", "alert('Enter Height - Triage Tab')", true);
                setTabIdAndColor(0, this.idVitalSign.lblHeight);
                return false;
            }
            else
            {
                this.idVitalSign.lblHeight.ForeColor = Color.Black;
            }

            if (this.idVitalSign.txtWeight.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValWeight", "alert('Enter Weight - Triage Tab')", true);
                setTabIdAndColor(0, this.idVitalSign.lblWeight);
                return false;
            }
            else
            {
                this.idVitalSign.lblWeight.ForeColor = Color.Black;
            }

            if (this.idVitalSign.txtBPSystolic.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValSBP", "alert('Enter Systolic Blood pressure - Triage Tab')", true);
                setTabIdAndColor(0, this.idVitalSign.lblBP);
                return false;
            }
            else
            {
                this.idVitalSign.lblBP.ForeColor = Color.Black;
            }
            if (this.idVitalSign.txtBPDiastolic.Text == "")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValDBP", "alert('Enter Diastolic Blood pressure - Triage Tab')", true);
                setTabIdAndColor(0, this.idVitalSign.lblBP);
                return false;
            }
            else
            {
                this.idVitalSign.lblBP.ForeColor = Color.Black;
            }
            return true;
        }
        private Boolean fieldValidation_Clinical()
        {
            int count = 0;
            for (int i = 0; i < this.cblchroniccondition.Items.Count; i++)
            {
                if (this.cblchroniccondition.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCC", "alert('Select Chronic conditions - Clinical History')", true);
                setTabIdAndColor(1, lblHeadingChronic);
                literalMedicalHistory.Text = @"<font size=3 color=Red>Medical History (Disease, Diagnosis and Treatment)</font>";
                return false;
            }
            else
            {                
                lblHeadingChronic.ForeColor = Color.Black;
                literalMedicalHistory.Text = @"<font size=3 Color.FromArgb(0,0,142)>Medical History (Disease, Diagnosis and Treatment)</font>";
            }
            count = 0;
            GridView gvPresentingComplaints = (GridView)UcPc.FindControl("gvPresentingComplaints");
            foreach (GridViewRow row in gvPresentingComplaints.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("ChkPresenting");
                if (chk.Checked == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCC", "alert('Select Presenting Complaints - Clinical History')", true);
                literPresenting.Text = @"<font size=3 color=Red>Presenting Complaints</font>";
                return false;
            }
            else
            {
                literPresenting.Text = @"<font size=3 Color.FromArgb(0,0,142)>Presenting Complaints</font>";
            }
            return true;
        }
        private Boolean fieldValidation_Examination()
        {
            int count = 0;
            for (int i = 0; i < chkLongTermMedication.Items.Count; i++)
            {
                if (chkLongTermMedication.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValGC", "alert('Select Long Term Medications - Examination Tab')", true);
                literalCurrentLong.Text = @"<font size=3 color=Red>Current Long Term Medications</font>";
                lblLongTermText.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalCurrentLong.Text = @"<font size=3 Color.FromArgb(0,0,142)>Current Long Term Medications</font>";
                lblLongTermText.ForeColor = Color.FromArgb(0, 0, 153);
            }

            count = 0;
            for (int i = 0; i < this.UcPE.cblGeneralConditions.Items.Count; i++)
            {
                if (this.UcPE.cblGeneralConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValGC", "alert('Select General Condition - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblGeneral.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblGeneral.ForeColor = Color.FromArgb(0, 0, 153);
            }

            count = 0;
            for (int i = 0; i < this.UcPE.cblCardiovascularConditions.Items.Count; i++)
            {
                if (this.UcPE.cblCardiovascularConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCC", "alert('Select Cardiovascular Conditions - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblCardiovarscular.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblCardiovarscular.ForeColor = Color.FromArgb(0, 0, 153);
            }
            count = 0;
            for (int i = 0; i < this.UcPE.cblOralCavityConditions.Items.Count; i++)
            {
                if (this.UcPE.cblOralCavityConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValOC", "alert('Select Oral Cavity Conditions - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblOralCavity.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblOralCavity.ForeColor = Color.FromArgb(0, 0, 153);
            }
            count = 0;
            for (int i = 0; i < this.UcPE.cblGenitalUrinaryConditions.Items.Count; i++)
            {
                if (this.UcPE.cblGenitalUrinaryConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValGU", "alert('Select GenitalUrinary Conditions - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblGenitourinary.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblGenitourinary.ForeColor = Color.FromArgb(0, 0, 153);
            }
            count = 0;
            for (int i = 0; i < this.UcPE.cblCNSConditions.Items.Count; i++)
            {
                if (this.UcPE.cblCNSConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCNS", "alert('Select CNS Conditions - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblCNS.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblCNS.ForeColor = Color.FromArgb(0, 0, 153);
            }
            count = 0;
            for (int i = 0; i < this.UcPE.cblChestLungsConditions.Items.Count; i++)
            {
                if (this.UcPE.cblChestLungsConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValChest", "alert('Select ChestLung Conditions - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblChest.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblChest.ForeColor = Color.FromArgb(0, 0, 153);
            }
            count = 0;
            for (int i = 0; i < this.UcPE.cblSkinConditions.Items.Count; i++)
            {
                if (this.UcPE.cblSkinConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValSkin", "alert('Select Skin Conditions - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblSkin.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblSkin.ForeColor = Color.FromArgb(0, 0, 153);
            }
            count = 0;
            for (int i = 0; i < this.UcPE.cblAbdomenConditions.Items.Count; i++)
            {
                if (this.UcPE.cblAbdomenConditions.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValAbdomen", "alert('Select Abdomen Conditions - Examination Tab')", true);
                literalPhysicalExam.Text = @"<font size=3 color=Red>Physical Examination</font>";
                this.UcPE.lblAbdomen.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalPhysicalExam.Text = @"<font size=3 Color.FromArgb(0,0,142)>Physical Examination</font>";
                this.UcPE.lblAbdomen.ForeColor = Color.FromArgb(0, 0, 153);
            }

            DropDownList ddlwhostage1 = (DropDownList)UcWhostaging.FindControl("ddlwhostage1");
            if (ddlwhostage1.SelectedItem.Text.ToLower() == "select")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValHeight", "alert('Select Current WHO Stage- Examination Tab')", true);
                literalWHOStaging.Text = @"<font size=3 color=Red>WHO Staging</font>";
                UcWhostaging.literalStaging.Text = @"<font size=3 color=Red>Staging</font>";
                setTabIdAndColor(3, this.UcWhostaging.lblWHOStage);
                return false;

            }
            else
            {
                literalWHOStaging.Text = @"<font size=3 Color.FromArgb(0,0,142)>WHO Staging</font>";
                UcWhostaging.literalStaging.Text = @"<font size=3 Color.FromArgb(0,0,142)>Staging</font>";
                UcWhostaging.lblWHOStage.ForeColor = Color.Black;
            }

            DropDownList ddlWABStage1 = (DropDownList)UcWhostaging.FindControl("ddlWABStage");
            if (ddlWABStage1.SelectedItem.Text.ToLower() == "select")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValHeight", "alert('Select WAB Stage - Examination Tab')", true);
                literalWHOStaging.Text = @"<font size=3 color=Red>WHO Staging</font>";
                setTabIdAndColor(3, this.UcWhostaging.lblWABStage);
                return false;

            }
            else
            {
                literalWHOStaging.Text = @"<font size=3 Color.FromArgb(0,0,142)>WHO Staging</font>";
                UcWhostaging.literalStaging.Text = @"<font size=3 Color.FromArgb(0,0,142)>WAB Stage</font>";
                UcWhostaging.lblWABStage.ForeColor = Color.Black;
            }

            return true;
        }
        private Boolean fieldValidation_Management()
        {
            DropDownList ddlTreatmentplan = (DropDownList)UcPharmacy.FindControl("ddlTreatmentplan");
            if (ddlTreatmentplan.SelectedItem.Text.ToLower() == "select")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValHeight", "alert('Select Treatment Plan - Management Tab')", true);
                literalTreatment.Text = @"<font size=3 color=Red>Treatment</font>";
                this.UcPharmacy.lblTreatmentplan.ForeColor = Color.Red;
                return false;
            }
            else
            {
                literalTreatment.Text = @"<font size=3 Color.FromArgb(0,0,142)>Treatment</font>";
                this.UcPharmacy.lblTreatmentplan.ForeColor = Color.FromArgb(0, 0, 153);
            }
            return true;
        }
        public void ErrorLoad()
        {
            string script = string.Empty;
            if (this.idVitalSign.txtWeight.Text != "" && this.idVitalSign.txtHeight.Text != "")
            {
                decimal bmi = Convert.ToDecimal(this.idVitalSign.txtWeight.Text) / (Convert.ToDecimal(this.idVitalSign.txtHeight.Text) / 100 * Convert.ToDecimal(this.idVitalSign.txtHeight.Text) / 100);
                this.idVitalSign.txtBMI.Text = Convert.ToString(Math.Round(bmi, 2));
            }
            if (rbListFatherAlive.SelectedValue == "0")
            {
                visibleDiv("divdatefatherdeath");
            }
            if (rbListMotherAlive.SelectedValue == "0")
            {
                visibleDiv("divdateofmotherdeath");
            }
            if (rbListRefFacility.SelectedValue == "1")
            {
                visibleDiv("divspecifyfacility");
            }
            

            if (ddlschoolingstatus.SelectedIndex == 1)
            {
                visibleDiv("divhighestlevel");
            }
            if (ddldisclosurestatus.SelectedItem.Text == "Not ready")
            {
                visibleDiv("divreasonnotdisclosed");
            }
            if (ddldisclosurestatus.SelectedItem.Text == "Other specify")
            {
                visibleDiv("divotherdisclosurestatus");
            }
            if (rbListRefFacility.SelectedValue == "1")
                visibleDiv("divspecifyfacility");
            //if (rdoreferedfromfacilityyes.Checked)
            //{
            //    visibleDiv("divspecifyfacility");
            //}
            //16 Ju
            //int i = rdochildonartyes.Checked ? 1 : rdochildonartno.Checked ? 0 : 9;
            //if (1 == 1)
            //{
            //    visibleDiv("divcurrentregimenline");
            //    visibleDiv("divARTRegimen");
            //    visibleDiv("divARTDate");
            //}
            //if (this.rdochildonartyes.Checked)
            //{
            //    visibleDiv("divcurrentregimenline");
            //}
            
            //if (this.rdoclientsupportgroupyes.Checked)
            //{
            //    visibleDiv("divsupportgroup");
            //}

            if (this.rbListPreviouslyAdmitt.SelectedValue == "1")
            {
                visibleDiv("divdiagnosis");
                visibleDiv("divadmissionstart");
                visibleDiv("divadmissionend");
            }
            if (this.rbListTBHistory.SelectedValue == "1")
            {
                visibleDiv("divcompletetxdate");
                visibleDiv("divretreatmentdate");
            }
            //if (this.rdotvhistoryyes.Checked)
            //{
            //    visibleDiv("divcompletetxdate");
            //    visibleDiv("divretreatmentdate");
            //}
            if (this.rbListMilestone.SelectedValue == "0")
            {
                visibleDiv("divmilestoneshowhide");
            }
            if (this.rdolabevaluationyes.Checked)
            {
                visibleDiv("divlabevaluatinshowhidey");
            }
            //16 Ju
            //if (this.radbtnMernarcheyes.Checked)
            //{
            //    visibleDiv("divmenarchedatehshowhidey");
            //}
            if (ddloiprophylaxis.SelectedItem.Text == "Cotrimoxazole")
            {
                visibleDiv("divoiprophylaxisshowhidey");
            }
            if (ddloiprophylaxis.SelectedItem.Text == "Other")
            {
                visibleDiv("divoiprophylasixothershowhidey");
            }
        }
        //Save - Triage Tab 
        protected void btnSaveTriage_Click(object sender, EventArgs e)
        {
            //First check the required values if valid the call the save function
            if (fieldValidation_Triage() == false)
            {
                literalVitalSign.Text = @"<font size=3 color=red>Vital Signs</font>";
                ErrorLoad();
                return;
            }
            literalVitalSign.Text = @"<font size=3 Color.FromArgb(0,0,142)>Vital Signs</font>";
            triage_Save(0);
        }
        //Clinical History 
        //Save click - History Tab
        protected void btnSaveCHistory_Click(Object o, EventArgs e)
        {
            if (fieldValidation_Clinical() == false)
            {
                ErrorLoad();
                return;
            }
            clinicalHistory_Save(0);
        }
        //Save- Examination Tab
        protected void btnSaveExam_Click(Object o, EventArgs e)
        {
            if (fieldValidation_Examination() == false)
            {
                ErrorLoad();
                return;
            }
            examination_Save(0);
        }
        //Save - Management Tab
        protected void btnSaveManagement_Click(Object o, EventArgs e)
        {
            if (fieldValidation_Management() == false)
            {
                ErrorLoad();
                return;
            }
            management_Save(1);
        }
        private void UserRights(Button save, Button print, int FeatureID, int TabID)
        {
            AuthenticationManager Authentication = new AuthenticationManager();
            bool bCanView = !Authentication.HasFunctionRight(FeatureID, TabID, FunctionAccess.View, (DataTable)Session["UserRight"]);

            //if user have view permission
            save.Enabled = bCanView;
            //first time  - new user form creation
            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                bool bCanAdd = Authentication.HasFunctionRight(FeatureID, TabID, FunctionAccess.Add, (DataTable)Session["UserRight"]);
                save.Enabled = bCanAdd;
            }
            else if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                bool bCanUpdate = Authentication.HasFunctionRight(FeatureID, TabID, FunctionAccess.Update, (DataTable)Session["UserRight"]);
                if (Convert.ToInt32(Session["AppUserID"]) == 1)
                {
                    bCanUpdate = true;
                }
                save.Enabled = bCanUpdate;
            }

            print.Enabled = Authentication.HasFunctionRight(FeatureID, TabID, FunctionAccess.Print, (DataTable)Session["UserRight"]);
        }
        protected void btnCloseTriage_Click(Object o, EventArgs e)
        {
            if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                Response.Redirect("frmPatient_History.aspx?&sts=0");
            }
            else
            {
                Response.Redirect("../frmFindAddPatient.aspx");
            }
        }
        protected void btnCloseManagement_Click(Object o, EventArgs e)
        {
            if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                Response.Redirect("frmPatient_History.aspx?&sts=0");
            }
            else
            {
                Response.Redirect("../frmFindAddPatient.aspx");
            }
        }
        protected void btnCloseExam_Click(Object o, EventArgs e)
        {
            if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                Response.Redirect("frmPatient_History.aspx?&sts=0");
            }
            else
            {
                Response.Redirect("../frmFindAddPatient.aspx");
            }
        }
        protected void btnCloseHistory_Click(Object o, EventArgs e)
        {
            if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                Response.Redirect("frmPatient_History.aspx?&sts=0");
            }
            else
            {
                Response.Redirect("../frmFindAddPatient.aspx");
            }
        }

    }
}
//8-Jun,14
//// Data Quality - Triage Tab 
//        protected void btnSubmitTriage_Click(Object o, EventArgs e)
//        {
//            //First check the required values if valid the call the save function
//            if (fieldValidation_Triage() == false)
//            {
//                literalVitalSign.Text = @"<font size=3 color=red>Vital Signs</font>";
//                ErrorLoad();
//                return;
//            }
//            literalVitalSign.Text = @"<font size=3 Color.FromArgb(0,0,142)>Vital Signs</font>";
//            triage_Save(0);
//        }
////Data Quality Check - Management Tab
//protected void btnSubmitManagement_Click(Object o, EventArgs e)
//{
//    if (fieldValidation_Management() == false)
//    {
//        ErrorLoad();
//        return;
//    }
//    management_Save(0);
//}
//Data quality check - History Tab
//protected void btnSubmitCHistory_Click(Object o, EventArgs e)
//{
//    if (fieldValidation_Clinical() == false)
//    {
//        ErrorLoad();
//        return;
//    }
//    clinicalHistory_Save(1);
//}

////Data Quality Check -  Examination Tab
//protected void btnSubmitExam_Click(Object o, EventArgs e)
//{
//    if (fieldValidation_Examination() == false)
//    {
//        ErrorLoad();
//        return;
//    }
//    examination_Save(1);
//}
//-------------------------------------------------------------------------------------------------------------------------------------------------
//private void SaveCancel()
//{

//    string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
//    script += "var ans;\n";

//    script += "ans=window.confirm('Form saved successfully. Do you want to close?');\n";
//    script += "if (ans==true)\n";
//    script += "{\n";
//    if (ViewState["Redirect"] == "0")
//    {
//        script += "window.location.href='frmPatient_Home.aspx';\n";
//    }
//    else
//    {
//        script += "window.location.href='frmPatient_History.aspx?sts=" + 0 + "';\n";
//    }
//    script += "}\n";
//    script += "else \n";
//    script += "{\n";
//    script += "window.location.href='frmClinical_KNH_Paediatric_IE.aspx';\n";
//    script += "}\n";
//    script += "</script>\n";
//    RegisterStartupScript("confirm", script);
//}
//protected void gvWHO1_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    DataTable dt = (DataTable)ViewState["WHOStage1"];
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        CheckBox chk = (CheckBox)e.Row.FindControl("Chkwho1");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)e.Row.FindControl("txtCurrentWho1Date");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)e.Row.FindControl("txtCurrentWho1Date1");
//        Label lbl = (Label)e.Row.FindControl("lblwho1");
//        foreach (DataRow r in dt.Rows)
//        {
//            if (r[1].ToString() == lbl.Text)
//            {
//                chk.Checked = true;
//                if (r[2].ToString() != "")
//                {
//                    txtdt1.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[2].ToString()));
//                }
//                if (r[3].ToString() != "")
//                {
//                    txtdt2.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[3].ToString()));
//                }
//            }
//        }

//    }


//}
//protected void gvWHO2_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    DataTable dt = (DataTable)ViewState["WHOStage2"];
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        CheckBox chk = (CheckBox)e.Row.FindControl("Chkwho2");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)e.Row.FindControl("txtCurrentWho2Date");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)e.Row.FindControl("txtCurrentWho2Date1");
//        Label lbl = (Label)e.Row.FindControl("lblwho2");
//        foreach (DataRow r in dt.Rows)
//        {
//            if (r[1].ToString() == lbl.Text)
//            {
//                chk.Checked = true;
//                if (r[2].ToString() != "")
//                {
//                    txtdt1.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[2].ToString()));
//                }
//                if (r[3].ToString() != "")
//                {
//                    txtdt2.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[3].ToString()));
//                }
//            }
//        }

//    }


//}
//protected void gvWHO3_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    DataTable dt = (DataTable)ViewState["WHOStage3"];
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        CheckBox chk = (CheckBox)e.Row.FindControl("Chkwho3");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)e.Row.FindControl("txtCurrentWho3Date");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)e.Row.FindControl("txtCurrentWho3Date1");
//        Label lbl = (Label)e.Row.FindControl("lblwho3");
//        foreach (DataRow r in dt.Rows)
//        {
//            if (r[1].ToString() == lbl.Text)
//            {
//                chk.Checked = true;
//                if (r[2].ToString() != "")
//                {
//                    txtdt1.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[2].ToString()));
//                }
//                if (r[3].ToString() != "")
//                {
//                    txtdt2.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[3].ToString()));
//                }
//            }
//        }

//    }


//}
//protected void gvWHO4_RowDataBound(object sender, GridViewRowEventArgs e)
//{
//    DataTable dt = (DataTable)ViewState["WHOStage4"];
//    if (e.Row.RowType == DataControlRowType.DataRow)
//    {
//        CheckBox chk = (CheckBox)e.Row.FindControl("Chkwho4");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt1 = (HtmlInputText)e.Row.FindControl("txtCurrentWho4Date");
//        System.Web.UI.HtmlControls.HtmlInputText txtdt2 = (HtmlInputText)e.Row.FindControl("txtCurrentWho4Date1");
//        Label lbl = (Label)e.Row.FindControl("lblwho4");
//        foreach (DataRow r in dt.Rows)
//        {
//            if (r[1].ToString() == lbl.Text)
//            {
//                chk.Checked = true;
//                if (r[2].ToString() != "")
//                {
//                    txtdt1.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[2].ToString()));
//                }
//                if (r[3].ToString() != "")
//                {
//                    txtdt2.Value = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(r[3].ToString()));
//                }
//            }
//        }

//    }


//}

//private Boolean fieldValidation(string TabName)
//{
//    IQCareUtils theUtil = new IQCareUtils();
//    MsgBuilder totalMsgBuilder = new MsgBuilder();
//    //ShowHide();
//    if (txtVisitDate.Value.Trim() == "")
//    {
//        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValVisitDate", "alert('Enter Visit Date')", true);
//        return false;
//    }
//    int count = 0;
//    if (TabName == "btnSaveTriage" || TabName == "btnSubmitTriage")
//    {
//        if (this.idVitalSign.txtHeight.Text == "")
//        {
//            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValHeight", "alert('Enter Height')", true);
//            return false;
//        }
//        if (this.idVitalSign.txtWeight.Text == "")
//        {
//            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValWeight", "alert('Enter Weight')", true);
//            return false;
//        }
//        if (this.idVitalSign.txtBPSystolic.Text == "")
//        {
//            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValSBP", "alert('Enter Systolic Blood pressure')", true);
//            return false;
//        }
//        if (this.idVitalSign.txtBPDiastolic.Text == "")
//        {
//           ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValDBP", "alert('Enter Diastolic Blood pressure')", true);
//            return false;
//        }
//    }
//    //else if (TabName == "btnSaveExam" || TabName == "btnSubmitExam")
//    //{
//    //    if (ddlWhoStage.SelectedValue == "0")
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select Current WHO Stage";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValWHO", "alert('Select Current WHO Stage')", true);
//    //        return false;
//    //    }

//    //    for (int i = 0; i < chkLTMedications.Items.Count; i++)
//    //    {
//    //        if (chkLTMedications.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select Long Term Medication";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValLTM", "alert('Select Long Term Medication')", true);
//    //        return false;
//    //    }
//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblGeneralConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblGeneralConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select General Condition";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValGC", "alert('Select General Condition')", true);
//    //        return false;
//    //    }
//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblCardiovascularConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblCardiovascularConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select Cardiovascular Conditions";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCC", "alert('Select Cardiovascular Conditions')", true);
//    //        return false;
//    //    }
//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblOralCavityConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblOralCavityConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select Oral Cavity Conditions";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValOC", "alert('Select Oral Cavity Conditions')", true);
//    //        return false;
//    //    }
//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblGenitalUrinaryConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblGenitalUrinaryConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select GenitalUrinary Conditions";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValGU", "alert('Select GenitalUrinary Conditions')", true);
//    //        return false;
//    //    }

//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblCNSConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblCNSConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select CNS Conditions";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCNS", "alert('Select CNS Conditions')", true);
//    //        return false;
//    //    }

//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblChestLungsConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblChestLungsConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select ChestLung Conditions";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValChest", "alert('Select ChestLung Conditions')", true);
//    //        return false;
//    //    }

//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblSkinConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblSkinConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select Skin Conditions";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValSkin", "alert('Select Skin Conditions')", true);
//    //        return false;
//    //    }

//    //    count = 0;
//    //    for (int i = 0; i < this.UcPE.cblAbdomenConditions.Items.Count; i++)
//    //    {
//    //        if (this.UcPE.cblAbdomenConditions.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select Abdomen Conditions";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValAbdomen", "alert('Select Abdomen Conditions')", true);
//    //        return false;
//    //    }
//    //}
//    //else if (TabName == "btnSaveManagement" || TabName == "btnSubmitManagement")
//    //{
//    //    count = 0;
//    //    for (int i = 0; i < cblDrugAllergiesToxicities.Items.Count; i++)
//    //    {
//    //        if (cblDrugAllergiesToxicities.Items[i].Selected == true)
//    //        {
//    //            count++;
//    //        }
//    //    }
//    //    if (count == 0)
//    //    {
//    //        //totalMsgBuilder.DataElements["MessageText"] = "Select Drug Allergy Toxicities";
//    //        //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValDAllergy", "alert('Select Drug Allergy Toxicities')", true);
//    //        return false;
//    //    }
//    //}
//    return true;

//}
//Triage Tab - Save button click


//private Boolean fieldValidation()
//{
//    string savetabname = hidtab2.Value;
//    IQCareUtils theUtil = new IQCareUtils();
//    MsgBuilder totalMsgBuilder = new MsgBuilder();

//    if (txtVisitDate.Value.Trim() == "")
//    {

//        totalMsgBuilder.DataElements["MessageText"] = "Enter Encounter Date";
//        IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//        return false;

//    }

//    if (savetabname == "Triage" || tabControl.ActiveTabIndex == 0)
//    {
//        tabControl.ActiveTabIndex = 0;
//        int childonart = rdochildonartyes.Checked ? 1 : rdochildonartno.Checked ? 0 : 9;
//        if (childonart == 9)
//        {
//            totalMsgBuilder.DataElements["MessageText"] = "child on ART required";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;
//        }
//        if (this.idVitalSign.txtHeight.Text == "")
//        {

//            totalMsgBuilder.DataElements["MessageText"] = "Enter Height";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;
//        }
//        if (this.idVitalSign.txtWeight.Text == "")
//        {

//            totalMsgBuilder.DataElements["MessageText"] = "Enter Weight";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;
//        }
//        if (this.idVitalSign.txtBPSystolic.Text == "")
//        {
//            totalMsgBuilder.DataElements["MessageText"] = "EnterSystolic Blood pressure";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;
//        }
//        if (this.idVitalSign.txtBPDiastolic.Text == "")
//        {
//            totalMsgBuilder.DataElements["MessageText"] = "Enter Diastolic Blood pressure";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;
//        }
//    }
//    //if (savetabname == "Clinical History" || tabControl.ActiveTabIndex == 1)
//    //{
//    //    tabControl.ActiveTabIndex = 1;
//    //    int arvexposer = rdoarvexposureyes.Checked ? 1 : rdoarvexposureno.Checked ? 0 : 9;
//    //    if (arvexposer == 9)
//    //    {
//    //        totalMsgBuilder.DataElements["MessageText"] = "ARV exposure required";
//    //        IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        return false;
//    //    }
//    //}
//    if (savetabname == "Examination" || tabControl.ActiveTabIndex == 3)
//    {
//        tabControl.ActiveTabIndex = 3;
//        if (ddlhivassociated.SelectedIndex == 0)
//        {
//            totalMsgBuilder.DataElements["MessageText"] = "Select HIV associated conditions";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;

//        }


//        if (ddlwhostage1.SelectedIndex == 0)
//        {
//            totalMsgBuilder.DataElements["MessageText"] = "Select Current WHO Stage";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;
//        }
//        if (ddlwabstage1.SelectedIndex == 0)
//        {
//            totalMsgBuilder.DataElements["MessageText"] = "Select Current WAB Stage";
//            IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//            return false;
//        }
//    }
//    ////7,May
//    ////Updated by - Nidhi
//    ////Desc-Now we are using user control pharmacy
//    //if (savetabname == "Management" || tabControl.ActiveTabIndex == 4)
//    //{
//    //    tabControl.ActiveTabIndex = 4;
//    //    int arvsideeffects = rdoarvsideeffectsyes.Checked ? 1 : rdoarvsideeffectsno.Checked ? 0 : 9;
//    //    if (arvsideeffects == 9)
//    //    {
//    //        totalMsgBuilder.DataElements["MessageText"] = "ARV side effects required";
//    //        IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//    //        return false;
//    //    }
//    //}

//    if (savetabname == "Prev With +ve" || tabControl.ActiveTabIndex == 5)
//    {
//        tabControl.ActiveTabIndex = 5;
//        //int lmp = rdolmpassessedyes.Checked ? 1 : rdolmpassessedno.Checked ? 0 : 0;
//        //if (lmp == 9)
//        //{
//        //    totalMsgBuilder.DataElements["MessageText"] = "LMP Assessed required";
//        //    IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//        //    return false;
//        //}

//        //int admittoward = rdoadmittowardyes.Checked ? 1 : rdoadmittowardno.Checked ? 0 : 0;
//        //if (admittoward == 9)
//        //{
//        //    totalMsgBuilder.DataElements["MessageText"] = "Admit to ward required";
//        //    IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
//        //    return false;
//        //}
//    }
//    return true;

//}
//private void SaveCancel(string tabname, string status)
//{
//    string script = string.Empty;
//    int tabindex = tabControl.ActiveTabIndex;
//    if (tabname == "Triage")
//    {
//        script = "";
//        script = "<script language = 'javascript' defer ='defer' id = 'stringascii'>\n";
//        script += "StringASCII('TabTriage');\n";
//        script += "</script>\n";
//        RegisterStartupScript("stringascii", script);
//    }
//    else if (tabname == "Clinical Assessment")
//    {
//        script = "";
//        script = "<script language = 'javascript' defer ='defer' id = 'stringascii'>\n";
//        script += "StringASCII('TabClinicalHistory');\n";
//        script += "</script>\n";
//        RegisterStartupScript("stringascii", script);
//    }
//    else if (tabname == "TB Screening")
//    {
//        script = "";
//        script = "<script language = 'javascript' defer ='defer' id = 'stringascii'>\n";
//        script += "StringASCII('TabTBSCreening');\n";
//        script += "</script>\n";
//        RegisterStartupScript("stringascii", script);
//    }
//    else if (tabname == "Examination")
//    {
//        script = "";
//        script = "<script language = 'javascript' defer ='defer' id = 'stringascii'>\n";
//        script += "StringASCII('TabExamination');\n";
//        script += "</script>\n";
//        RegisterStartupScript("stringascii", script);
//    }
//    else if (tabname == "Management")
//    {
//        script = "";
//        script = "<script language = 'javascript' defer ='defer' id = 'stringascii'>\n";
//        script += "StringASCII('TabManagement');\n";
//        script += "</script>\n";
//        RegisterStartupScript("stringascii", script);
//    }
//    else if (tabname == "PrevWith")
//    {
//        script = "";
//        script = "<script language = 'javascript' defer ='defer' id = 'stringascii'>\n";
//        script += "StringASCII('TabPrevwithpositive');\n";
//        script += "</script>\n";
//        RegisterStartupScript("stringascii", script);
//    }
//    script = "";
//    script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
//    script += "alert('Data on " + tabname + " " + status + " successfully');\n";
//    script += "</script>\n";
//    RegisterStartupScript("confirm", script);
//}

 ////Checking all the required values 
 //       private Boolean fieldValidation(string TabName)
 //       {
 //           IQCareUtils theUtil = new IQCareUtils();
 //           MsgBuilder totalMsgBuilder = new MsgBuilder();
 //           //ShowHide();
 //           if (txtVisitDate.Value.Trim() == "")
 //           {
 //               ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValVisitDate", "alert('Enter Visit Date')", true);
 //               return false;
 //           }
 //           int count = 0;
 //           if (TabName == "btnSaveTriage" || TabName == "btnSubmitTriage")
 //           {
 //               if (this.idVitalSign.txtHeight.Text == "")
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValHeight", "alert('Enter Height')", true);
 //                   return false;
 //               }
 //               if (this.idVitalSign.txtWeight.Text == "")
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValWeight", "alert('Enter Weight')", true);
 //                   return false;
 //               }
 //               if (this.idVitalSign.txtBPSystolic.Text == "")
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValSBP", "alert('Enter Systolic Blood pressure')", true);
 //                   return false;
 //               }
 //               if (this.idVitalSign.txtBPDiastolic.Text == "")
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValDBP", "alert('Enter Diastolic Blood pressure')", true);
 //                   return false;
 //               }
 //           }
 //           else if (TabName == "btnSaveExam" || TabName == "btnSubmitExam")
 //           {
 //               //if (count == 0)
 //               //{
 //               //    //totalMsgBuilder.DataElements["MessageText"] = "Select Long Term Medication";
 //               //    //IQCareMsgBox.Show("#C1", totalMsgBuilder, this);
 //               //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValLTM", "alert('Select Long Term Medication')", true);
 //               //    return false;
 //               //}
 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblGeneralConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblGeneralConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValGC", "alert('Select General Condition')", true);
 //                   return false;
 //               }
 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblCardiovascularConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblCardiovascularConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
                  
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCC", "alert('Select Cardiovascular Conditions')", true);
 //                   return false;
 //               }
 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblOralCavityConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblOralCavityConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValOC", "alert('Select Oral Cavity Conditions')", true);
 //                   return false;
 //               }
 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblGenitalUrinaryConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblGenitalUrinaryConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
                 
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValGU", "alert('Select GenitalUrinary Conditions')", true);
 //                   return false;
 //               }

 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblCNSConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblCNSConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
                    
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValCNS", "alert('Select CNS Conditions')", true);
 //                   return false;
 //               }

 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblChestLungsConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblChestLungsConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
                    
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValChest", "alert('Select ChestLung Conditions')", true);
 //                   return false;
 //               }

 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblSkinConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblSkinConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValSkin", "alert('Select Skin Conditions')", true);
 //                   return false;
 //               }

 //               count = 0;
 //               for (int i = 0; i < this.UcPE.cblAbdomenConditions.Items.Count; i++)
 //               {
 //                   if (this.UcPE.cblAbdomenConditions.Items[i].Selected == true)
 //                   {
 //                       count++;
 //                   }
 //               }
 //               if (count == 0)
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValAbdomen", "alert('Select Abdomen Conditions')", true);
 //                   return false;
 //               }
 //           }
 //           else if (TabName == "btnSaveManagement" || TabName == "btnSubmitManagement")
 //           {
 //               count = 0;
 //               //for (int i = 0; i < cblDrugAllergiesToxicities.Items.Count; i++)
 //               //{
 //               //    if (cblDrugAllergiesToxicities.Items[i].Selected == true)
 //               //    {
 //               //        count++;
 //               //    }
 //               //}
 //               if (count == 0)
 //               {
 //                   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ValDAllergy", "alert('Select Drug Allergy Toxicities')", true);
 //                   return false;
 //               }
 //           }
 //           return true;

 //       }


/////////////////
//see this later important
//public void BindHidfortab()
//{
//    // First Encounter Date

//    //------------------------------section client information
//    //Child accompanied by
//    hidtab1.Value = txtchildaccompaniedby.ClientID;
//    //Child diagnosis confirmed
//    hidtab1.Value = hidtab1.Value + "^" + ddlchilddiagnosis.ClientID;

//    //Child Primary Caregiver
//    hidtab1.Value = hidtab1.Value + "^" + txtchildcaregiver.ClientID;
//    //Date of HIV Diagnosis
//    hidtab1.Value = hidtab1.Value + "^" + txtdateofhivdiagnosis.ClientID;
//    //Disclosure status
//    hidtab1.Value = hidtab1.Value + "^" + ddldisclosurestatus.ClientID;
//    //Specify reason not discloesd
//    hidtab1.Value = hidtab1.Value + "^" + txtspecifyreason.ClientID;
//    //Specify other disclosure status
//    hidtab1.Value = hidtab1.Value + "^" + txtspecifyotherdisclosurestatus.ClientID;
//    //Father alive
//    //hidtab1.Value = hidtab1.Value + "^" + rdofatheraliveyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdofatheraliveno.ClientID;
//    //Date of fathers death
//    hidtab1.Value = hidtab1.Value + "^" + txtdateoffatherdeath.ClientID;
//    //Have you been referred from another facility :
//    hidtab1.Value = hidtab1.Value + "^" + rdoreferedfromfacilityyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdoreferedfromfacilityno.ClientID;

//    //Specify facility referred from :
//    hidtab1.Value = hidtab1.Value + "^" + txtspecifyfacility.ClientID;
//    //Is the child on ART
//    hidtab1.Value = hidtab1.Value + "^" + rdochildonartyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdochildonartno.ClientID;

//    //Current regimen line 
//    hidtab1.Value = hidtab1.Value + "^" + ddlcurrentregimenline.ClientID;
//    //Current ART regimen
//    hidtab1.Value = hidtab1.Value + "^" + ddlcurrentartregimen.ClientID;
//    //Current ART Regimen Began Date 
//    hidtab1.Value = hidtab1.Value + "^" + txtcurrentartregimendate.ClientID;
//    //Is the child on CTX :
//    hidtab1.Value = hidtab1.Value + "^" + rdochildonctxyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdochildonctxno.ClientID;

//    //Mother alive
//    //hidtab1.Value = hidtab1.Value + "^" + rdomotheraliveyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdomotheraliveno.ClientID;
//    //Date of mothers death
//    hidtab1.Value = hidtab1.Value + "^" + txtdateofmotherdeath.ClientID;
//    //Schooling Status
//    hidtab1.Value = hidtab1.Value + "^" + ddlschoolingstatus.ClientID;
//    //Highest level attained
//    hidtab1.Value = hidtab1.Value + "^" + ddlhighestlevelattained.ClientID;
//    //Health education given?
//    hidtab1.Value = hidtab1.Value + "^" + rdohealtheducatingivenyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdohealtheducatingivenno.ClientID;

//    //Is client a member of a support group
//    hidtab1.Value = hidtab1.Value + "^" + rdoclientsupportgroupyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdoclientsupportgroupno.ClientID;

//    //HIV support group membership
//    hidtab1.Value = hidtab1.Value + "^" + txthivsupportgroupmembership.ClientID;

//    //--------------section vital sign
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtTemp.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtRR.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtHR.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtHeight.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtWeight.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtBPDiastolic.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtBPSystolic.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtheadcircumference.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.ddlweightforage.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtweightforheight.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.idVitalSign.txtnursescomments.ClientID;
//    // above working fine

//    ////------Presenting Complaints------------------
//    //Presenting complaints additional notes
//    hidtab1.Value = hidtab1.Value + "^" + txtAdditionPresentingComplaints.ClientID;
//    //If schooling,current school perfomance :
//    hidtab1.Value = hidtab1.Value + "^" + ddlschoolperfomance.ClientID;

//    //-------------Medical history (Disease, diagnosis and treatment)
//    // Medical history
//    hidtab1.Value = hidtab1.Value + "^" + rdomedicalhistoryyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdomedicalhistoryno.ClientID;

//    //Specify Medical history :
//    hidtab1.Value = hidtab1.Value + "^" + txtspecifymedicalhistory.ClientID;
//    //Specify other chronic condition
//    hidtab1.Value = hidtab1.Value + "^" + txtotherchroniccondition.ClientID;
//    //Previously admitted :
//    hidtab1.Value = hidtab1.Value + "^" + rdopreviouslyadmittedyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdopreviouslyadmittedno.ClientID;

//    // Diagnosis :
//    hidtab1.Value = hidtab1.Value + "^" + txtdiagnosis.ClientID;
//    //Admission start 
//    hidtab1.Value = hidtab1.Value + "^" + txtadmissionstart.ClientID;
//    //Admission end
//    hidtab1.Value = hidtab1.Value + "^" + txtadmissionend.ClientID;

//    //----------------TB History--------------------------
//    //TB History :
//    hidtab1.Value = hidtab1.Value + "^" + rdotvhistoryyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdotvhistoryno.ClientID;

//    //Complete TX Date
//    hidtab1.Value = hidtab1.Value + "^" + txtcompletetxdate.ClientID;
//    //Retreatment Date
//    hidtab1.Value = hidtab1.Value + "^" + txtretreatmentdate.ClientID;


//    //----------------------Immunisation Status----------------
//    //Immunisation Status
//    hidtab1.Value = hidtab1.Value + "^" + ddlimmunisationstatus.ClientID;
//    //---------------------ARV history----------------------
//    //ARV exposure
//    //Nidhi
//    //hidtab1.Value = hidtab1.Value + "^" + rdoarvexposureyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoarvexposureno.ClientID;

//    //PMTCT
//    hidtab1.Value = hidtab1.Value + "^" + txtpmtctdate.ClientID;
//    // PMTCT Regimen :
//    hidtab1.Value = hidtab1.Value + "^" + txtpmtctregimen.ClientID;
//    //HAART Start date
//    hidtab1.Value = hidtab1.Value + "^" + txthaartstartdate.ClientID;
//    // HAART Regimen :
//    hidtab1.Value = hidtab1.Value + "^" + txthaartregimen.ClientID;
//    //PEP Start date
//    hidtab1.Value = hidtab1.Value + "^" + txtpepstartdate.ClientID;
//    //PEP Regimen
//    hidtab1.Value = hidtab1.Value + "^" + txtpepregimen.ClientID;
//    //HIV related history
//    //nidhi
//    //hidtab1.Value = hidtab1.Value + "^" + ddlhivhistory.ClientID;
//    //Initial CD4 :
//    hidtab1.Value = hidtab1.Value + "^" + txtcd4.ClientID;
//    // Initial CD4% 
//    hidtab1.Value = hidtab1.Value + "^" + txtcd4per.ClientID;
//    //Initial CD4 date :
//    hidtab1.Value = hidtab1.Value + "^" + txtcd4date.ClientID;
//    //Highest CD4 ever
//    hidtab1.Value = hidtab1.Value + "^" + txthighCD4ever.ClientID;
//    //Highest CD4 ever % :
//    hidtab1.Value = hidtab1.Value + "^" + txthighestcd4everper.ClientID;
//    //Highest CD4 ever date
//    hidtab1.Value = hidtab1.Value + "^" + txthigcd4everdate.ClientID;
//    //CD4 at ART initiation :
//    hidtab1.Value = hidtab1.Value + "^" + txtcd4artinitiation.ClientID;
//    //CD4 at ART initiation %
//    hidtab1.Value = hidtab1.Value + "^" + txtcd4artinitper.ClientID;
//    //    CD4 at ART initiation date :
//    hidtab1.Value = hidtab1.Value + "^" + txtcd4artinitdate.ClientID;
//    //Most recent CD4
//    //hidtab1.Value = hidtab1.Value + "^" + txtmostrecent_cd4.ClientID;
//    //Most Recent CD4% :
//    hidtab1.Value = hidtab1.Value + "^" + txtmostrecentcd4per.ClientID;
//    //Most recent CD4 date :
//    hidtab1.Value = hidtab1.Value + "^" + txtmostrecentcd4date.ClientID;
//    // Previous viral load :
//    //hidtab1.Value = hidtab1.Value + "^" + txtpreviousviral_load.ClientID;
//    //Previous viral load date 
//    hidtab1.Value = hidtab1.Value + "^" + txtpreviousviralloaddate.ClientID;
//    //Other HIV related history :
//    //hidtab1.Value = hidtab1.Value + "^" + txtotherhivrelated_history.ClientID;


//    //--------------  //TB Screening ICF(2 signs & 2 symptoms - TB likely)
//    //TB Assessment


//    //TB Findings :
//    //5 May,2014
//    //Updated By - Nidhi 
//    //Desc- coz we are adding TB control(.ascx) 
//    //hidtab1.Value = hidtab1.Value + "^" + ddltbfinding.ClientID;
//    ////Sputum smear
//    ////hidtab1.Value = hidtab1.Value + "^" + ddlsputum_smear.ClientID;
//    ////Tissue Biopsy :


//    ////Tissue Biopsy
//    //hidtab1.Value = hidtab1.Value + "^" + ddltissuebiopsy.ClientID;
//    //// Chest X ray :


//    ////Chest X ray results
//    //hidtab1.Value = hidtab1.Value + "^" + ddlchestxrayresults.ClientID;
//    //// Other CXR (specify) 


//    ////Tissue Biopsy results 

//    //hidtab1.Value = hidtab1.Value + "^" + txttissuebiopsyresults.ClientID;


//    //--------------TB Evaluation and Treatment Plan------------------

//    ////  TB Type
//    //5 May,2014
//    //Updated By - Nidhi 
//    //Desc- coz we are adding TB control(.ascx) 
//    //hidtab1.Value = hidtab1.Value + "^" + ddltbtype.ClientID;
//    ////Patient type
//    //hidtab1.Value = hidtab1.Value + "^" + ddlpatienttype.ClientID;
//    //// TB plan :
//    //hidtab1.Value = hidtab1.Value + "^" + ddltbplan.ClientID;
//    ////Specify Other TB plan
//    //hidtab1.Value = hidtab1.Value + "^" + txtothertbplan.ClientID;
//    ////TB regimen :
//    //hidtab1.Value = hidtab1.Value + "^" + ddltbregimen.ClientID;
//    ////Other TB regimen
//    //hidtab1.Value = hidtab1.Value + "^" + txtothertbregimen.ClientID;
//    ////TB regimen start date :
//    //hidtab1.Value = hidtab1.Value + "^" + txttbregimentstartdate.ClientID;
//    ////TB regimen end date 
//    //hidtab1.Value = hidtab1.Value + "^" + txttbregimentenddate.ClientID;
//    ////TB treatment outcome
//    //hidtab1.Value = hidtab1.Value + "^" + ddltbtreatmentoutcome.ClientID;

//    //--------------IPT(Patients with No signs and Symptoms) ?--------------------------

//    //INH Started? :
//    //5 May,2014
//    //Updated By - Nidhi 
//    //Desc- coz we are adding TB control(.ascx) 
//    //hidtab1.Value = hidtab1.Value + "^" + rdoinhstartedyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoinhstartedno.ClientID;

//    ////pyridoxine 5mg\kg and INH 10mg/kg :
//    //hidtab1.Value = hidtab1.Value + "^" + chkpyridoxine.ClientID;

//    ////INH Start Date 
//    //hidtab1.Value = hidtab1.Value + "^" + txtinhstartdate.ClientID;
//    //// INH End Date :
//    //hidtab1.Value = hidtab1.Value + "^" + txtinhenddate.ClientID;
//    ////Pyriodoxine Start Date
//    //hidtab1.Value = hidtab1.Value + "^" + txtpyriodoxinestartdate.ClientID;
//    ////Pyriodoxine End Date :
//    //hidtab1.Value = hidtab1.Value + "^" + txtpyriodoxineenddate.ClientID;
//    ////Has adherence been assessed
//    //hidtab1.Value = hidtab1.Value + "^" + rdohasadherenceassessedyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdohasadherenceassessedno.ClientID;

//    //// Any missed doses? 
//    //hidtab1.Value = hidtab1.Value + "^" + rdoanymisseddosesyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoanymisseddosesno.ClientID;

//    ////If yes referred for adherence? :
//    //hidtab1.Value = hidtab1.Value + "^" + rdoyesreferredadherenceyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoyesreferredadherenceno.ClientID;

//    //// Specify other TB side effects 
//    //5 May,2014
//    //Updated By - Nidhi 
//    //Desc- coz we are adding TB control(.ascx) 
//    //hidtab1.Value = hidtab1.Value + "^" + othertbsideeffects.ClientID;

//    ////---------------Confirmed or TB suspected-------------------------------

//    ////Confirmed or Suspected TB (Stop INH)
//    //hidtab1.Value = hidtab1.Value + "^" + rdostopINHYes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdostopINHno.ClientID;

//    //// Stop INH Date
//    //hidtab1.Value = hidtab1.Value + "^" + dtStopINHDate.ClientID;
//    ////Household contacts screened for TB
//    //hidtab1.Value = hidtab1.Value + "^" + rdostopINHYes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdostopINHno.ClientID;

//    ////If No specify why
//    //hidtab1.Value = hidtab1.Value + "^" + txtTBNotScreenedSpecify.ClientID;

//    //-------------------Long term medications
//    ////Long term medications
//    //hidtab1.Value = hidtab1.Value + "^" + rblbtnLongTermMedicationsyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rblbtnLongTermMedicationsno.ClientID;

//    ////Sulfa TMP :
//    //hidtab1.Value = hidtab1.Value + "^" + dtSulfaTMPDate.ClientID;
//    ////Antifungals
//    //hidtab1.Value = hidtab1.Value + "^" + dtAntifungalsDate.ClientID;
//    ////Anticonvulsants
//    //hidtab1.Value = hidtab1.Value + "^" + dtAntihypertensivesDate.ClientID;
//    ////Other current long term medications
//    //hidtab1.Value = hidtab1.Value + "^" + txOtherLongTermMedications.ClientID;
//    ////Other long term medications
//    //hidtab1.Value = hidtab1.Value + "^" + dtOtherCurrentLongTermMedications.ClientID;

//    //-----------------------------------------------------------------------------------------------
//    //-----------------Physical Examination ?------------------------
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherMedicalConditionNotes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherGeneralConditions.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherAbdomenConditions.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherCardiovascularConditions.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherOralCavityConditions.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherGenitourinaryConditions.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherCNSConditions.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherChestLungsConditions.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.txtOtherSkinConditions.ClientID;

//    //----------------------Developmental milestones
//    // Milestone appropriate? :
//    hidtab1.Value = hidtab1.Value + "^" + rdomilestoneappropriateyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdomilestoneappropriateno.ClientID;

//    //If No specify why inappropriate
//    hidtab1.Value = hidtab1.Value + "^" + txtspecifywhyinappropriate.ClientID;

//    //------------------- Tests and labs-------------------
//    // Lab evaluation 
//    hidtab1.Value = hidtab1.Value + "^" + rdolabevaluationyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + rdolabevaluationno.ClientID;


//    //------------Staging at initial evaluation ?
//    //WHO Stage at initiation (Transfer in)
//    hidtab1.Value = hidtab1.Value + "^" + ddlInitiationWHOstage.ClientID;
//    //HIV associated conditions
//    hidtab1.Value = hidtab1.Value + "^" + ddlhivassociated.ClientID;
//    //Current WHO Stage

//    hidtab1.Value = hidtab1.Value + "^" + ddlwhostage1.ClientID;
//    //WAB Stage
//    hidtab1.Value = hidtab1.Value + "^" + ddlwabstage1.ClientID;
//    //Tanner staging
//    hidtab1.Value = hidtab1.Value + "^" + ddltannerstaging.ClientID;
//    //Mernarche
//    hidtab1.Value = hidtab1.Value + "^" + radbtnMernarcheyes.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + radbtnMernarcheno.ClientID;


//    //-----------------------------------------------------------------------------
//    //Menarche Date :
//    hidtab1.Value = hidtab1.Value + "^" + txtmenarchedate.ClientID;
//    //-------------  Drug Allergy and Toxicities ?---------------
//    //Specify ARV allergy 
//    hidtab1.Value = hidtab1.Value + "^" + txtarvallergy.ClientID;
//    //Specify antibiotic allergy 
//    hidtab1.Value = hidtab1.Value + "^" + txtantibioticallergy.ClientID;
//    //Specify other drug allergy
//    hidtab1.Value = hidtab1.Value + "^" + txtotherdrugallergy.ClientID;

//    //------------------------Treatment: ?-------------------
//    // ARV Side Effects :
//    ////7,May
//    ////Updated by - Nidhi
//    ////Desc-Now we are using user control pharmacy
//    //hidtab1.Value = hidtab1.Value + "^" + rdoarvsideeffectsyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoarvsideeffectsno.ClientID;

//    //Specify other short term effects\
//    hidtab1.Value = hidtab1.Value + "^" + txtspecityothershortterm.ClientID;
//    // Specify Other long term effects 
//    hidtab1.Value = hidtab1.Value + "^" + txtspecifyotherlongterm.ClientID;
//    //Work up plan :
//    hidtab1.Value = hidtab1.Value + "^" + txtworkupplan.ClientID;
//    ////Treatment
//    ////7,May
//    ////Updated by - Nidhi
//    ////Desc-Now we are using user control
//    //hidtab1.Value = hidtab1.Value + "^" + ddltreatment.ClientID;
//    //// Start ART? :
//    //hidtab1.Value = hidtab1.Value + "^" + rdostartartyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdostartartno.ClientID;

//    ////Specify Other Eligibility Criteria :

//    ////Substitute Regimen?
//    //hidtab1.Value = hidtab1.Value + "^" + rdosubstituteregimenyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdosubstituteregimenno.ClientID;


//    //// Number of drugs substituted :
//    //hidtab1.Value = hidtab1.Value + "^" + ddlnumberofdrugsubstituted.ClientID;
//    //Stop treatment
//    //Updated by - Nidhi
//    //hidtab1.Value = hidtab1.Value + "^" + rdostoptreatmentyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdostoptreatmentno.ClientID;

//    //Regimen Prescribed
//    //hidtab1.Value = hidtab1.Value + "^" + ddlregimenprescribed.ClientID;
//    ////Other regimen (specify)
//    //hidtab1.Value = hidtab1.Value + "^" + txtothreregimenspecify.ClientID;
//    //OI Prophylaxis
//    hidtab1.Value = hidtab1.Value + "^" + ddloiprophylaxis.ClientID;
//    //Cotrimoxazole prescribed for
//    hidtab1.Value = hidtab1.Value + "^" + ddlcotrimoxazole.ClientID;
//    //Fluconazole prescribed for
//    hidtab1.Value = hidtab1.Value + "^" + ddlfluconazole.ClientID;
//    //Other (Specify)
//    hidtab1.Value = hidtab1.Value + "^" + txtothercotrimoxazole.ClientID;
//    //Other treatment :
//    hidtab1.Value = hidtab1.Value + "^" + txtothertreatementcatrimozazole.ClientID;

//    //-------------------Sexuallity Assessment ?
//    //created user control 
//    //Patient sexually active in the last 6 months? :
//    //hidtab1.Value = hidtab1.Value + "^" + rdosexuallyactiveyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdosexuallyactiveno.ClientID;

//    ////Sexual orientation
//    //hidtab1.Value = hidtab1.Value + "^" + ddlsexualorientation.ClientID;
//    ////If tested for HIV, is the status disclosed?
//    //hidtab1.Value = hidtab1.Value + "^" + rdohivtestedyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdohivtestedno.ClientID;

//    ////Sexual partners HIV status :
//    //hidtab1.Value = hidtab1.Value + "^" + ddlsexualpartnershiv.ClientID;
//    ////LMP Assessed
//    //hidtab1.Value = hidtab1.Value + "^" + rdolmpassessedyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdolmpassessedno.ClientID;

//    ////LMP Date
//    //hidtab1.Value = hidtab1.Value + "^" + txtlmpdate.ClientID;
//    ////Reason LMP not assessed
//    //hidtab1.Value = hidtab1.Value + "^" + ddlreasonlmpnotassessed.ClientID;
//    //// Pregnancy test done? :
//    //hidtab1.Value = hidtab1.Value + "^" + rdopregnancytestdoneyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdopregnancytestdoneno.ClientID;

//    ////If Yes, client pregnant?
//    //hidtab1.Value = hidtab1.Value + "^" + rdoclientpregnantyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoclientpregnantno.ClientID;

//    ////If yes for pregnant, has patient been offered or reffered to PMTCT? :
//    //hidtab1.Value = hidtab1.Value + "^" + rdopregnantofferedpmtctyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdopregnantofferedpmtctno.ClientID;


//    ////EDD
//    //hidtab1.Value = hidtab1.Value + "^" + txtedddate.ClientID;




//    //-----------------------------------------------------------------------------------------



//    //---------PWP Interventions--------------------
//    //created user control
//    ////pwp messages given:
//    //hidtab1.Value = hidtab1.Value + "^" + ddlpwpmessagesgiven.ClientID;
//    ////importance of sefer sex explained
//    //hidtab1.Value = hidtab1.Value + "^" + rdoimportanceofsefersexyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoimportanceofsefersexno.ClientID;

//    ////Condoms issued
//    //hidtab1.Value = hidtab1.Value + "^" + rdocondomsissuedyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdocondomsissuedno.ClientID;

//    ////Reasons condoms not issued
//    //hidtab1.Value = hidtab1.Value + "^" + txtreasonscondoms.ClientID;
//    ////Do you or your partner intend to become pregnant before next visit? :
//    //hidtab1.Value = hidtab1.Value + "^" + rdopregnantbefornextvisityes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdopregnantbefornextvisitno.ClientID;

//    ////If yes discussed fertility options
//    //hidtab1.Value = hidtab1.Value + "^" + rdofertilityoptinsyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdofertilityoptinsno.ClientID;

//    ////If No discussed dual contraception :
//    //hidtab1.Value = hidtab1.Value + "^" + rdodualcontraceptionyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdodualcontraceptionno.ClientID;

//    //Are you on family planning mehod other than condoms? :
//    //user control created
//    //hidtab1.Value = hidtab1.Value + "^" + rdofpmothercondomsyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdofpmothercondomsno.ClientID;

//    ////Specify other FP method other than condoms :
//    //hidtab1.Value = hidtab1.Value + "^" + ddlfpmethodother.ClientID;
//    ////Have you been screened for cervical cancer? 
//    //hidtab1.Value = hidtab1.Value + "^" + rdoscreenedccyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoscreenedccno.ClientID;

//    //// Ca Cervix screening results :
//    //hidtab1.Value = hidtab1.Value + "^" + ddlcacervix.ClientID;
//    ////If No referred for cervical screening
//    //hidtab1.Value = hidtab1.Value + "^" + rdoreferredcsyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoreferredcsno.ClientID;

//    //// HPV Offered? :
//    //hidtab1.Value = hidtab1.Value + "^" + rdohpvofferedyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdohpvofferedno.ClientID;

//    ////Has HPV vaccine been offered?
//    //hidtab1.Value = hidtab1.Value + "^" + ddlhpvvaccine.ClientID;
//    //// Date vaccine given :
//    //hidtab1.Value = hidtab1.Value + "^" + txtdatevaccinegiven.ClientID;
//    ////Treatment Plan
//    //hidtab1.Value = hidtab1.Value + "^" + txttreatmentplan.ClientID;
//    ////Counselling

//    //// Other counselling 
//    //hidtab1.Value = hidtab1.Value + "^" + txtothercounselling.ClientID;
//    //// HPV Vaccine given :
//    //hidtab1.Value = hidtab1.Value + "^" + rdohpvvaccineyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdohpvvaccineno.ClientID;

//    ////Contact tracing for social support
//    //hidtab1.Value = hidtab1.Value + "^" + rdoctsocialsupportyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoctsocialsupportno.ClientID;

//    ////Screened for STI?
//    //hidtab1.Value = hidtab1.Value + "^" + ddlscreenedforstt.ClientID;
//    ////Urethral Discharge :
//    //hidtab1.Value = hidtab1.Value + "^" + chkurethral.ClientID;

//    ////Vaginal Discharge 
//    //hidtab1.Value = hidtab1.Value + "^" + chkvaginal.ClientID;

//    ////Genital Ulceration :
//    //hidtab1.Value = hidtab1.Value + "^" + chkgenital.ClientID;

//    ////STI Treatment plan
//    //hidtab1.Value = hidtab1.Value + "^" + txtsttplan.ClientID;
//    ////Admit to ward :
//    //hidtab1.Value = hidtab1.Value + "^" + rdoadmittowardyes.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + rdoadmittowardno.ClientID;

//    ////Refer to specialist clinic (specify clinic)
//    //hidtab1.Value = hidtab1.Value + "^" + txtrefertospecialist.ClientID;
//    ////Transfer to another facility(specify facility)
//    //hidtab1.Value = hidtab1.Value + "^" + txttransfertoanother.ClientID;


//    //----------------------------------------------------------------------------------------
//    //hidtab1.Value = hidtab1.Value + "^" + ddlsingature.ClientID;

//    hidtab1.Value = hidtab1.Value + "^" + cblchroniccondition.ClientID;
//    //5 May,2014
//    //Updated By - Nidhi 
//    //Desc- coz we are adding TB control(.ascx) 
//    //hidtab1.Value = hidtab1.Value + "^" + cbltbassessment.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + cblstopreason.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + cblreviewlist.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + cbllabevaluation.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + cblDrugAllergiesToxicitiesPaeds.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + cblshorttermeffects.ClientID;
//    hidtab1.Value = hidtab1.Value + "^" + cbllongtermeffects.ClientID;
//    ////7,May
//    ////Updated by - Nidhi
//    ////Desc-Now we are using user control pharmacy
//    //hidtab1.Value = hidtab1.Value + "^" + cblswithcingregimen.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + cbleligiblethrough.ClientID;
//    //Updated by - Nidhi 
//    //hidtab1.Value = hidtab1.Value + "^" + cblartstopreason.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + cblsexualityhighrisk.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + cblcounselling.ClientID;
//    //hidtab1.Value = hidtab1.Value + "^" + cbltransitionpreparation.ClientID;



//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblGeneralConditions.ClientID;

//    //Cardiovascular conditions
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblCardiovascularConditions.ClientID;

//    //CNS 
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblCNSConditions.ClientID;

//    //Oral cavity
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblOralCavityConditions.ClientID;

//    //Chest Lungs
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblChestLungsConditions.ClientID;

//    //Genitourinary 
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblGenitalUrinaryConditions.ClientID;

//    //Skin 
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblSkinConditions.ClientID;

//    //Abdomen conditions
//    hidtab1.Value = hidtab1.Value + "^" + this.UcPE.cblAbdomenConditions.ClientID;

//}

//part of validate function
//public void Validate()
//5 May,2014
//Updated By - Nidhi 
//Desc- coz we are adding TB control(.ascx) 
//txttbregimentstartdate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//txttbregimentenddate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//txtinhstartdate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//txtinhenddate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//txtpyriodoxinestartdate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//txtpyriodoxineenddate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//dtStopINHDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//created a user control
//txtlmpdate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//txtedddate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
// txtdatevaccinegiven.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");

//5 May,2014
//Updated By - Nidhi 
//Desc- coz we are adding TB control(.ascx) 
//txttbregimentstartdate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//txttbregimentenddate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//txtinhstartdate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//txtinhenddate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//txtpyriodoxinestartdate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//txtpyriodoxineenddate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//dtStopINHDate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//created a user control
//txtlmpdate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//txtedddate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//txtdatevaccinegiven.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");

//dtSulfaTMPDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//dtAntifungalsDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");
//dtAntihypertensivesDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3');");

//dtSulfaTMPDate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//dtAntifungalsDate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//dtAntihypertensivesDate.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");
//dtOtherCurrentLongTermMedications.Attributes.Add("OnKeyup", "DateFormat(this,this.value,event,true,'3');");

////////////
 //public void ErrorLoad()
 //       {
 //           string script = string.Empty;
 //           if (this.idVitalSign.txtWeight.Text != "" && this.idVitalSign.txtHeight.Text != "")
 //           {
 //               decimal bmi = Convert.ToDecimal(this.idVitalSign.txtWeight.Text) / (Convert.ToDecimal(this.idVitalSign.txtHeight.Text) / 100 * Convert.ToDecimal(this.idVitalSign.txtHeight.Text) / 100);
 //               this.idVitalSign.txtBMI.Text = Convert.ToString(Math.Round(bmi, 2));
 //           }
 //           if (ddldisclosurestatus.SelectedItem.Text == "Not ready")
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divreasonnotdisclosed'>\n";
 //               script += "ShowHide('divreasonnotdisclosed','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divreasonnotdisclosed", script);
 //           }
 //           if (ddldisclosurestatus.SelectedItem.Text == "Other specify")
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divotherdisclosurestatus'>\n";
 //               script += "ShowHide('divotherdisclosurestatus','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divotherdisclosurestatus", script);
 //           }
 //           //if (rdofatheraliveno.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divdatefatherdeath'>\n";
 //           //    script += "ShowHide('divdatefatherdeath','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divdatefatherdeath", script);
 //           //}

 //           if (rdoreferedfromfacilityyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divspecifyfacility'>\n";
 //               script += "ShowHide('divspecifyfacility','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divspecifyfacility", script);
 //           }

 //           if (this.rdochildonartyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divcurrentregimenline'>\n";
 //               script += "ShowHide('divcurrentregimenline','show');ShowHide('divcurrentartregimen','show');ShowHide('divcurrentartregimendate','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divcurrentregimenline", script);
 //           }

 //           //if (rdomotheraliveno.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divdateofmotherdeath'>\n";
 //           //    script += "ShowHide('divdateofmotherdeath','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divdateofmotherdeath", script);
 //           //}

 //           if (ddlschoolingstatus.SelectedIndex == 1)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divhighestlevel'>\n";
 //               script += "ShowHide('divhighestlevel','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divhighestlevel", script);
 //           }

 //           if (this.rdoclientsupportgroupyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divsupportgroup'>\n";
 //               script += "ShowHide('divsupportgroup','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divsupportgroup", script);

 //           }

 //           if (this.rdopreviouslyadmittedyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divdiagnosis'>\n";
 //               script += "ShowHide('divdiagnosis','show');ShowHide('divadmissionstart','show');ShowHide('divadmissionend','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divdiagnosis", script);
 //           }

 //           if (this.rdotvhistoryyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divcompletetxdate'>\n";
 //               script += "ShowHide('divcompletetxdate','show');ShowHide('divretreatmentdate','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divcompletetxdate", script);
 //           }

 //           //if (this.rdoarvexposureyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divarvexposure'>\n";
 //           //    script += "ShowHide('divarvexposure','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divarvexposure", script);
 //           //}
 //           //nidhi
 //           //if (ddlhivhistory.SelectedItem.Text == "Yes")
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divhivrelatedhistoryyes'>\n";
 //           //    script += "ShowHide('divhivrelatedhistory','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divhivrelatedhistoryyes", script);
 //           //}


 //           //5 May,2014
 //           //Updated By - Nidhi 
 //           //Desc- coz we are adding TB control(.ascx) 
 //           //if(ddltbplan.SelectedItem.Text == "Other (Specify)")
 //           //{
 //           //script = "";
 //           //script = "<script language = 'javascript' defer ='defer' id = 'divtbplanshowhiney'>\n";
 //           //script += "ShowHide('divtbplanshowhine','show');\n";
 //           //script += "</script>\n";
 //           //RegisterStartupScript("divtbplanshowhiney", script);
 //           //}

 //           //if(ddltbregimen.SelectedItem.Text == "Other")
 //           //{
 //           //script = "";
 //           //script = "<script language = 'javascript' defer ='defer' id = 'divothertbregimenyes'>\n";
 //           //script += "ShowHide('divothertbregimen','show');\n";
 //           //script += "</script>\n";
 //           //RegisterStartupScript("divothertbregimenyes", script);
 //           //}

 //           //if(this.rdoinhstartedyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divinhstartedshowhideyes'>\n";
 //           //    script += "ShowHide('divinhstartedshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divinhstartedshowhideyes", script);
 //           //}
 //           //5 May,2014
 //           //Updated By - Nidhi 
 //           //Desc- coz we are adding TB control(.ascx) 
 //           //if(this.rdostopINHYes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divstopinhdateshowhidey'>\n";
 //           //    script += "ShowHide('divstopinhdateshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divstopinhdateshowhidey", script);
 //           //}
 //           //if(this.rdostopINHYes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divhouseholdshowhidey'>\n";
 //           //    script += "ShowHide('divhouseholdshowhide','hide');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divhouseholdshowhidey", script);
 //           //}

 //           //if(this.rdostopINHno.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divhouseholdshowhiden'>\n";
 //           //    script += "ShowHide('divhouseholdshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divhouseholdshowhiden", script);
 //           //}

 //           if (this.rdomilestoneappropriateyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divmilestoneshowhidey'>\n";
 //               script += "ShowHide('divmilestoneshowhide','hide');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divmilestoneshowhidey", script);
 //           }

 //           if (this.rdomilestoneappropriateno.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divmilestoneshowhiden'>\n";
 //               script += "ShowHide('divmilestoneshowhide','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divmilestoneshowhiden", script);
 //           }
 //           if (this.rdolabevaluationyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divlabevaluatinshowhidey'>\n";
 //               script += "ShowHide('divlabevaluatinshowhide','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divlabevaluatinshowhidey", script);
 //           }

 //           if (this.radbtnMernarcheyes.Checked)
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divmenarchedatehshowhidey'>\n";
 //               script += "ShowHide('divmenarchedatehshowhide','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divmenarchedatehshowhidey", script);
 //           }
 //           //Updated by - Nidhi
 //           //Desc-Now we are using user control
 //           //if (this.rdoarvsideeffectsyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divshorttermeffectsY'>\n";
 //           //    script += "ShowHide('divshorttermeffects','show');ShowHide('divlongtermeffects','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divshorttermeffectsY", script);
 //           //}

 //           //if (this.rdostartartyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'diveligiblethroughshowhidey'>\n";
 //           //    script += "ShowHide('diveligiblethroughshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("diveligiblethroughshowhidey", script);
 //           //}
 //           //if (this.rdosubstituteregimenyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divdrugssubstitutedshowhidey'>\n";
 //           //    script += "ShowHide('divdrugssubstitutedshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divdrugssubstitutedshowhidey", script);
 //           //}

 //           //if (ddlregimenprescribed.SelectedItem.Text == "Other")
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divotherregimenshowhidey'>\n";
 //           //    script += "ShowHide('divotherregimenshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divotherregimenshowhidey", script);
 //           //}

 //           if (ddloiprophylaxis.SelectedItem.Text == "Cotrimoxazole")
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divoiprophylaxisshowhidey'>\n";
 //               script += "ShowHide('divoiprophylaxisshowhide','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divoiprophylaxisshowhidey", script);
 //           }
 //           if (ddloiprophylaxis.SelectedItem.Text == "Other")
 //           {
 //               script = "";
 //               script = "<script language = 'javascript' defer ='defer' id = 'divoiprophylasixothershowhidey'>\n";
 //               script += "ShowHide('divoiprophylasixothershowhide','show');\n";
 //               script += "</script>\n";
 //               RegisterStartupScript("divoiprophylasixothershowhidey", script);
 //           }
 //           //created a user control
 //           //if (this.rdosexuallyactiveyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divsexualorientationshowhidey'>\n";
 //           //    script += "ShowHide('divsexualorientationshowhide','show');ShowHide('divsexualityhighriskshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divsexualorientationshowhidey", script);
 //           //}
 //           //if (this.rdolmpassessedyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divlmpdateshowhidey'>\n";
 //           //    script += "ShowHide('divlmpdateshowhide','show');ShowHide('divlmpnotassessedshowhide','hide');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divlmpdateshowhidey", script);
 //           //}

 //           //if (this.rdolmpassessedno.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divlmpdateshowhidey'>\n";
 //           //    script += "ShowHide('divlmpdateshowhide','hide');ShowHide('divlmpnotassessedshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divlmpdateshowhidey", script);
 //           //}
 //           //if (this.rdopregnancytestdoneyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divpregnancytestdoneshowhidey'>\n";
 //           //    script += "ShowHide('divpregnancytestdoneshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divpregnancytestdoneshowhidey", script);
 //           //}
 //           //if (this.rdocondomsissuedyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divreasonscondomsshowhidey'>\n";
 //           //    script += "ShowHide('divreasonscondomsshowhide','hide');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divreasonscondomsshowhidey", script);
 //           //}
 //           //if (this.rdocondomsissuedno.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divreasonscondomsshowhidey'>\n";
 //           //    script += "ShowHide('divreasonscondomsshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divreasonscondomsshowhidey", script);
 //           //}
 //           //if (this.rdopregnantbefornextvisityes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divnodiscussedshowhidey'>\n";
 //           //    script += "ShowHide('divnodiscussedshowhide','hide');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divnodiscussedshowhidey", script);
 //           //}
 //           //if (this.rdopregnantbefornextvisitno.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divnodiscussedshowhidey'>\n";
 //           //    script += "ShowHide('divnodiscussedshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divnodiscussedshowhidey", script);
 //           //}
 //           //user control created
 //           //if (this.rdofpmothercondomsyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divfamilyplanningshowhidey'>\n";
 //           //    script += "ShowHide('divfamilyplanningshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divfamilyplanningshowhidey", script);
 //           //}
 //           //if (this.rdoscreenedccyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divcacervixshowhidey'>\n";
 //           //    script += "ShowHide('divcacervixshowhide','show');ShowHide('divcacervisnoshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divcacervixshowhidey", script);
 //           //}

 //           //if (this.rdohpvofferedyes.Checked)
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divhpvofferedyesshowhidey'>\n";
 //           //    script += "ShowHide('divhpvofferedyesshowhide','show');ShowHide('divhpvvaccineshowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divhpvofferedyesshowhidey", script);
 //           //}
 //           //if (ddlscreenedforstt.SelectedItem.Text == "Yes")
 //           //{
 //           //    script = "";
 //           //    script = "<script language = 'javascript' defer ='defer' id = 'divstishowhidey'>\n";
 //           //    script += "ShowHide('divstishowhide','show');\n";
 //           //    script += "</script>\n";
 //           //    RegisterStartupScript("divstishowhidey", script);
 //           //}

 //       }

//protected Hashtable HT(int qltyFlag)
//{

//     Hashtable theHT = new Hashtable();
//    try
//    {

//        //--------------  //TB Screening ICF(2 signs & 2 symptoms - TB likely)
//        ////TB Assessment
//        //5 May,2014
//        //Updated By - Nidhi 
//        //Desc- coz we are adding TB control(.ascx) 
//        //int tbassessment = 0;
//        //theHT.Add("TBAssessed", tbassessment);
//        ////TB Findings :
//        //theHT.Add("TBFindings", ddltbfinding.SelectedItem.Value);
//        ////Sputum smear
//        //theHT.Add("SputumSmear", ddlsputum_smear.SelectedItem.Value);
//        ////Tissue Biopsy :
//        //int tissuebiopsy = 0;
//        //theHT.Add("TissueBiopsy", tissuebiopsy);
//        ////Tissue Biopsy
//        //theHT.Add("TissueBiopsyTest", ddltissuebiopsy.SelectedItem.Value);
//        //// Chest X ray :
//        //int chestxray = 0;
//        //theHT.Add("ChestXRay", chestxray);
//        ////Chest X ray results
//        //theHT.Add("CXR", ddlchestxrayresults.SelectedItem.Value);
//        //// Other CXR (specify) 


//        ////Tissue Biopsy results
//        //theHT.Add("TissueBiopsyResults", txttissuebiopsyresults.Text);


//        //--------------TB Evaluation and Treatment Plan------------------
//        //5 May,2014
//        //Updated By - Nidhi 
//        //Desc- coz we are adding TB control(.ascx) 
//        ////  TB Type
//        //theHT.Add("TBTypePeads", ddltbtype.SelectedItem.Value);
//        ////Patient type
//        //theHT.Add("PeadsTBPatientType", ddlpatienttype.SelectedItem.Value);
//        //// TB plan :
//        //theHT.Add("TBPlan", ddltbplan.SelectedItem.Value);
//        ////Specify Other TB plan
//        //theHT.Add("OtherTBPlan", txtothertbplan.Text);
//        ////TB regimen :
//        //theHT.Add("TBRegimen", ddltbregimen.SelectedItem.Value);
//        ////Other TB regimen
//        //theHT.Add("OtherTBRegimen", txtothertbregimen.Text);
//        ////TB regimen start date :
//        //theHT.Add("TBRegimenStartDate", txttbregimentstartdate.Value);
//        ////TB regimen end date 
//        //theHT.Add("TBRegimenEndDate", txttbregimentenddate.Value);
//        ////TB treatment outcome
//        //theHT.Add("TBTreatmentOutcomesPeads", ddltbtreatmentoutcome.SelectedItem.Value);

//        //--------------IPT(Patients with No signs and Symptoms) ?--------------------------

//        //INH Started? :
//        //5 May,2014
//        //Updated By - Nidhi 
//        //Desc- coz we are adding TB control(.ascx) 
//        //int inhstarted = rdoinhstartedyes.Checked ? 1 : rdoinhstartedno.Checked ? 0 : 9;
//        //theHT.Add("NoTB", inhstarted);
//        //pyridoxine 5mg\kg and INH 10mg/kg :

//        //INH Start Date 
//        //5 May,2014
//        //Updated By - Nidhi 
//        //Desc- coz we are adding TB control(.ascx) 
//        //theHT.Add("INHStartDate", txtinhstartdate.Value);
//        //// INH End Date :
//        //theHT.Add("INHEndDate", txtinhenddate.Value);
//        ////Pyriodoxine Start Date
//        //theHT.Add("PyridoxineStartDate", txtpyriodoxinestartdate.Value);
//        ////Pyriodoxine End Date :
//        //theHT.Add("PyridoxineEndDate", txtpyriodoxineenddate.Value);
//        ////Has adherence been assessed
//        //int adherenceassessed = rdohasadherenceassessedyes.Checked ? 1 : rdohasadherenceassessedno.Checked ? 0 : 9;
//        //theHT.Add("TBAdherenceAssessed", adherenceassessed);
//        //// Any missed doses? 
//        //int misseddoses = rdoanymisseddosesyes.Checked ? 1 : rdoanymisseddosesno.Checked ? 0 : 9;
//        //theHT.Add("MissedTBdoses", misseddoses);
//        ////If yes referred for adherence? :
//        //int referredadherence = rdoyesreferredadherenceyes.Checked ? 1 : rdoyesreferredadherenceno.Checked ? 0 : 9;
//        //theHT.Add("ReferredForAdherence", referredadherence);
//        // Specify other TB side effects 
//        // theHT.Add("OtherTBsideEffects", othertbsideeffects.Text);

//        //---------------Confirmed or TB suspected-------------------------------

//        //Confirmed or Suspected TB (Stop INH)
//        // int confirmedtb = rdostopINHYes.Checked ? 1 : rdostopINHno.Checked ? 0 : 9;
//        //theHT.Add("SuspectTB", confirmedtb);
//        // Stop INH Date
//        // theHT.Add("StopINHDate", dtStopINHDate.Value);
//        //Household contacts screened for TB
//        //int household = rdostopINHYes.Checked ? 1 : rdostopINHno.Checked ? 0 : 9;
//        // theHT.Add("ContactsScreenedForTB", household);
//        //If No specify why
//        // theHT.Add("TBnotScreenedSpecify", txtTBNotScreenedSpecify.Text);

//        //-------------------Long term medications
//        ////Long term medications
//        //int longtermmedications = rblbtnLongTermMedicationsyes.Checked ? 1 : rblbtnLongTermMedicationsno.Checked ? 0 : 9;
//        //theHT.Add("LongTermMedications", longtermmedications);
//        ////Sulfa TMP :
//        //theHT.Add("SulfaTMPDate", dtSulfaTMPDate.Value);
//        ////Antifungals
//        //theHT.Add("AntifungalsDate", dtAntifungalsDate.Value);
//        ////Anticonvulsants
//        //theHT.Add("AnticonvulsantsDate", dtAntihypertensivesDate.Value);
//        ////Other current long term medications
//        //theHT.Add("OtherLongTermMedications", txOtherLongTermMedications.Text);
//        ////Other long term medications
//        //theHT.Add("OtherCurrentLongTermMedications", dtOtherCurrentLongTermMedications.Value);

//        //int remindeript = chkpyridoxine.Checked ? 1 : 0;
//        //theHT.Add("ReminderIPT", remindeript);


//        //------------------------Treatment: ?-------------------
//        // ARV Side Effects :
//        ////7,May
//        ////Updated by - Nidhi
//        ////Desc-Now we are using user control
//        //int arvsideeffects = rdoarvsideeffectsyes.Checked ? 1 : rdoarvsideeffectsno.Checked ? 0 : 9;
//        //theHT.Add("ARVSideEffects", arvsideeffects);
//        //Specify other short term effects\

//        //Treatment
//        ////7,May
//        ////Updated by - Nidhi
//        ////Desc-Now we are using user control
//        //theHT.Add("ARTtreatmentPlanPeads", ddltreatment.SelectedItem.Value);
//        //// Start ART? :
//        //int startart = rdostartartyes.Checked ? 1 : rdostartartno.Checked ? 0 : 9;
//        //theHT.Add("StartART", startart);
//        ////Specify Other Eligibility Criteria :

//        ////Substitute Regimen?
//        //int substitute = rdosubstituteregimenyes.Checked ? 1 : rdosubstituteregimenno.Checked ? 0 : 9;
//        //theHT.Add("SubstituteRegimen", substitute);

//        //// Number of drugs substituted :
//        //theHT.Add("NumberDrugsSubstituted", ddlnumberofdrugsubstituted.SelectedItem.Value);
//        //Stop treatment
//        //5 May,2014
//        //Updated By - Nidhi 
//        //int stiptreatment = rdostoptreatmentyes.Checked ? 1 : rdostoptreatmentno.Checked ? 0 : 9;
//        //theHT.Add("StopTreatment", stiptreatment);
//        //Regimen Prescribed
//        //theHT.Add("RegimenPrescribed", ddlregimenprescribed.SelectedItem.Value);
//        ////Other regimen (specify)
//        //theHT.Add("OtherRegimenPrescribed", txtothreregimenspecify.Text);
//        //OI Prophylaxis

//        //created a user control
//        ////-------------------Sexuallity Assessment ?
//        ////Patient sexually active in the last 6 months? :
//        //int sexuallyactive = rdosexuallyactiveyes.Checked ? 1 : rdosexuallyactiveno.Checked ? 0 : 9;
//        //theHT.Add("SexualActiveness", sexuallyactive);
//        ////Sexual orientation
//        //theHT.Add("SexualOrientation", ddlsexualorientation.SelectedItem.Value);
//        ////If tested for HIV, is the status disclosed?
//        //int testedforhiv = rdohivtestedyes.Checked ? 1 : rdohivtestedno.Checked ? 0 : 9;
//        //theHT.Add("ChildHIVStatusDisclosed", testedforhiv);
//        ////Sexual partners HIV status :
//        //theHT.Add("PartnerHIVStatus", ddlsexualpartnershiv.SelectedItem.Value);
//        ////LMP Assessed
//        //int lmpassessed = rdolmpassessedyes.Checked ? 1 : rdolmpassessedno.Checked ? 0 : 9;
//        //theHT.Add("LMPassessmentValid", lmpassessed);
//        ////LMP Date
//        //theHT.Add("LMP", txtlmpdate.Value);
//        ////Reason LMP not assessed
//        //theHT.Add("LMPNotaccessedReason", ddlreasonlmpnotassessed.SelectedItem.Value);
//        //// Pregnancy test done? :
//        //int pregnancytestdone = rdopregnancytestdoneyes.Checked ? 1 : rdopregnancytestdoneno.Checked ? 0 : 9;
//        //theHT.Add("PDTdone", pregnancytestdone);
//        ////If Yes, client pregnant?
//        //int clientpregnant = rdoclientpregnantyes.Checked ? 1 : rdoclientpregnantno.Checked ? 0 : 9;

//        ////If yes for pregnant, has patient been offered or reffered to PMTCT? :
//        //int pmtctoffered = rdopregnantofferedpmtctyes.Checked ? 1 : rdopregnantofferedpmtctno.Checked ? 0 : 9;
//        //theHT.Add("PMTCToffered", pmtctoffered);

//        ////EDD
//        //theHT.Add("EDD", txtedddate.Value);

//        ////---------PWP Interventions--------------------
//        ////pwp messages given:
//        //theHT.Add("GivenPWPMessages", ddlpwpmessagesgiven.SelectedItem.Value);
//        ////importance of sefer sex explained
//        //int importanceofsex = rdoimportanceofsefersexyes.Checked ? 1 : rdoimportanceofsefersexno.Checked ? 0 : 9;
//        //theHT.Add("SaferSexImportanceExplained", importanceofsex);
//        ////Condoms issued
//        //int condomsissued = rdocondomsissuedyes.Checked ? 1 : rdocondomsissuedno.Checked ? 0 : 9;
//        //theHT.Add("CondomsIssued", condomsissued);
//        ////Reasons condoms not issued
//        //theHT.Add("ReasonfornotIssuingCondoms", txtreasonscondoms.Text);
//        ////Do you or your partner intend to become pregnant before next visit? :
//        //int pregnantbefornext = rdopregnantbefornextvisityes.Checked ? 1 : rdopregnantbefornextvisitno.Checked ? 0 : 9;
//        //theHT.Add("IntentionOfPregnancy", pregnantbefornext);
//        ////If yes discussed fertility options
//        //int fertilityoptions = rdofertilityoptinsyes.Checked ? 1 : rdofertilityoptinsno.Checked ? 0 : 9;
//        //theHT.Add("DiscussedFertilityOption", fertilityoptions);
//        ////If No discussed dual contraception :
//        //int dualcontraception = rdodualcontraceptionyes.Checked ? 1 : rdodualcontraceptionno.Checked ? 0 : 9;
//        //theHT.Add("DiscussedDualContraception", dualcontraception);
//        //Are you on family planning mehod other than condoms? :
//        //user control creatd
//        //int fpmethod = rdofpmothercondomsyes.Checked ? 1 : rdofpmothercondomsno.Checked ? 0 : 9;
//        //theHT.Add("OnFP", fpmethod);
//        ////Specify other FP method other than condoms :
//        //theHT.Add("FPmethod", ddlfpmethodother.SelectedItem.Value);
//        ////Have you been screened for cervical cancer? 
//        //int cervicalcancer = rdoscreenedccyes.Checked ? 1 : rdoscreenedccno.Checked ? 0 : 9;
//        //theHT.Add("CervicalCancerScreened", cervicalcancer);
//        //// Ca Cervix screening results :
//        //theHT.Add("CervicalCancerScreeningResults", ddlcacervix.SelectedItem.Value);
//        ////If No referred for cervical screening
//        //int referredcervicalscreening = rdoreferredcsyes.Checked ? 1 : rdoreferredcsno.Checked ? 0 : 9;
//        //theHT.Add("ReferredForCervicalCancerScreening", referredcervicalscreening);
//        //// HPV Offered? :
//        //int hpvoffered = rdohpvofferedyes.Checked ? 1 : rdohpvofferedno.Checked ? 0 : 9;
//        //theHT.Add("HPVOffered", hpvoffered);
//        ////Has HPV vaccine been offered?
//        //theHT.Add("OfferedHPVaccine", ddlhpvvaccine.SelectedItem.Value);
//        //// Date vaccine given :
//        //theHT.Add("HPVDoseDate", txtdatevaccinegiven.Value);
//        ////Treatment Plan
//        //theHT.Add("TreatmentPlan", txttreatmentplan.Text);
//        ////Counselling
//        //theHT.Add("Counselling", 0);
//        //// Other counselling 
//        //theHT.Add("OtherCounselling", txtothercounselling.Text);
//        //// HPV Vaccine given :
//        //int hpvvaccinegiven = rdohpvvaccineyes.Checked ? 1 : rdohpvvaccineno.Checked ? 0 : 9;
//        //theHT.Add("HPVvaccine", hpvvaccinegiven);
//        ////Contact tracing for social support
//        //int socialsupport = rdoctsocialsupportyes.Checked ? 1 : rdoctsocialsupportno.Checked ? 0 : 9;
//        //theHT.Add("ContactTracing", socialsupport);
//        ////Screened for STI?
//        //theHT.Add("STIscreenedPeads", ddlscreenedforstt.SelectedItem.Value);
//        ////Urethral Discharge :
//        //int urethral = chkurethral.Checked ? 1 : 0;
//        //theHT.Add("UrethralDischarge", urethral);
//        ////Vaginal Discharge 
//        //int vaginal = chkvaginal.Checked ? 1 : 0;
//        //theHT.Add("VaginalDischarge", vaginal);
//        ////Genital Ulceration :
//        //int genital = chkgenital.Checked ? 1 : 0;
//        //theHT.Add("GenitalUlceration", genital);
//        ////STI Treatment plan
//        //theHT.Add("STItreatmentPlan", txtsttplan.Text);
//        ////Admit to ward :
//        //int admittoward = rdoadmittowardyes.Checked ? 1 : rdoadmittowardno.Checked ? 0 : 9;
//        //theHT.Add("WardAdmission", admittoward);
//        ////Refer to specialist clinic (specify clinic)
//        //theHT.Add("ReferToSpecialistClinic", txtrefertospecialist.Text);
//        ////Transfer to another facility(specify facility)
//        //theHT.Add("TransferOut", txttransfertoanother.Text);
//        //theHT.Add("TransitionPreparation", 0);

//        theHT.Add("signature", ddlsingature.SelectedItem.Value);

//        if (qltyFlag == 1)
//        {
//            theHT.Add("qltyFlag", "1");
//        }
//        else
//        {
//            theHT.Add("qltyFlag", "0");
//        }



//    }
//    catch (Exception err)
//    {
//        MsgBuilder theMsg = new MsgBuilder();
//        theMsg.DataElements["MessageText"] = err.Message.ToString();
//        IQCareMsgBox.Show("#C1", theMsg, this);
//    }
//    return theHT;

//}
////Added by - Nidhi
//protected DataTable GetDataTable(string flag, string fieldName)
//{
//    BIQAdultIE objAdultIEFields = new BIQAdultIE();
//    objAdultIEFields.Flag = flag;
//    objAdultIEFields.PtnPk = 0;//Convert.ToInt32(Session["PatientID"].ToString());
//    objAdultIEFields.LocationId = 0;// Int32.Parse(Session["AppLocationId"].ToString());
//    objAdultIEFields.FieldName = fieldName;

//    IKNHAdultIE theExpressManager;
//    theExpressManager = (IKNHAdultIE)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BAdultIE, BusinessProcess.Clinical");
//    DataTable dt = theExpressManager.GetKnhAdultIEData(objAdultIEFields);
//    return dt;

//}
//Bind all checkboxlist and dropdown controls from the database


//public void Save(int dqchk)
//{
//    string savetabname = hidtab2.Value;
//    //if (fieldValidation() == false)
//    //{
//    //    ErrorLoad();
//    //    return;
//    //}
//    //Hashtable theHT = HT(dqchk);
//    DataTable dtSave = CreateTempTable();
//    string tabname = string.Empty;
//    if (savetabname == "Triage" || tabControl.ActiveTabIndex == 0)
//    {
//        tabname = "Triage";
//    }
//    else if (savetabname == "Clinical History" || tabControl.ActiveTabIndex == 1)
//    {
//        tabname = "Clinical History";
//        dtSave = GetCheckBoxListValues(cblchroniccondition, dtSave, "ChronicCondition");
//        dtSave = PresentingComplaints(dtSave, "PresentingComplaints");
//    }
//    //Updated by-Nidhi
//    //Desc- We have created a .ascx control for it
//    //else if (savetabname == "TB Screening" || tabControl.ActiveTabIndex == 2)
//    //{
//    //    tabname = "TB Screening";
//    //    //5 May,2014
//    //    //Updated By - Nidhi 
//    //    //Desc- coz we are adding TB control(.ascx) 
//    //    //dtSave = GetCheckBoxListValues(cbltbassessment, dtSave, "TBICFPaeds");
//    //    //dtSave = GetCheckBoxListValues(cblstopreason, dtSave, "TBStopReason");
//    //    //dtSave = GetCheckBoxListValues(cblreviewlist, dtSave, "TBSideEffects");
//    //}
//    else if (savetabname == "Examination" || tabControl.ActiveTabIndex == 3)
//    {
//        tabname = "Examination";




//    }
//    else if (savetabname == "Management" || tabControl.ActiveTabIndex == 4)
//    {
//        tabname = "Management";

//        ////7,May
//        ////Updated by - Nidhi
//        ////Desc-Now we are using user control pharmacy
//        //dtSave = GetCheckBoxListValues(cblswithcingregimen, dtSave, "SwitchReason");
//        //dtSave = GetCheckBoxListValues(cbleligiblethrough, dtSave, "ARTeligibilityCriteria");
//        //Nidhi
//        //dtSave = GetCheckBoxListValues(cblartstopreason, dtSave, "StopTreatmentCodes");
//    }
//    //Not required - Created a user control
//    //else if (savetabname == "Prev With +ve" || tabControl.ActiveTabIndex == 5)
//    //{
//    //    tabname = "PrevWith";
//    //    //dtSave = GetCheckBoxListValues(cblsexualityhighrisk, dtSave, "HighRisk");
//    //    //user control created
//    //    //dtSave = GetCheckBoxListValues(cblcounselling, dtSave, "Counselling");
//    //    //dtSave = GetCheckBoxListValues(cbltransitionpreparation, dtSave, "TransitionPreparation");
//    //}


//    KNHPEP = (IPatientKNHPEP)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientKNHPEP, BusinessProcess.Clinical");
//    DataSet DsReturns = KNHPEP.SaveUpdatePaediatric_IE(theHT, dtSave, tabname);
//    Session["Redirect"] = "0";
//    string saveupdate = string.Empty;
//    if (Convert.ToInt32(DsReturns.Tables[0].Rows[0]["Visit_Id"]) > 0)
//    {
//        if (Convert.ToInt32(Session["PatientVisitId"].ToString()) > 0)
//        {
//            saveupdate = "Update";
//        }
//        else
//        {
//            saveupdate = "Save";
//        }
//        Session["PatientVisitId"] = Convert.ToInt32(DsReturns.Tables[0].Rows[0]["Visit_Id"]);
//        hdnVisitId.Value = DsReturns.Tables[0].Rows[0]["Visit_Id"].ToString();
//        if (savetabname == "Triage" || tabControl.ActiveTabIndex == 0)
//        {
//            tabControl.ActiveTabIndex = 0;
//        }
//        else if (savetabname == "Clinical Assessment" || tabControl.ActiveTabIndex == 1)
//        {
//            tabControl.ActiveTabIndex = 1;
//        }
//        else if (savetabname == "TB Screening" || tabControl.ActiveTabIndex == 2)
//        {
//            tabControl.ActiveTabIndex = 2;
//        }
//        else if (savetabname == "Examination" || tabControl.ActiveTabIndex == 3)
//        {
//            tabControl.ActiveTabIndex = 3;
//        }
//        else if (savetabname == "Management" || tabControl.ActiveTabIndex == 4)
//        {
//            tabControl.ActiveTabIndex = 4;
//        }
//        else if (savetabname == "Prev With +ve" || tabControl.ActiveTabIndex == 5)
//        {
//            tabControl.ActiveTabIndex = 5;
//        }
//        //SaveCancel(tabname, saveupdate);

//        //SaveCancel();
//        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Tab Data saved successfully')", true);
//        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackBtnClick();", true);
//    }
//}

//protected void Button1_Click(object sender, EventArgs e)
//{
//    Save(0);
//}
//protected void btncomplete_Click(object sender, EventArgs e)
//{
//    Save(1);
//}

//____________________________________________________________________________
