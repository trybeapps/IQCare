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
    public partial class frmKNHExpress : TouchUserControlBase
    {
        BIQTouchExpressFields objExp = new BIQTouchExpressFields();
        static Boolean IsError = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            //txtRadHeight.Attributes.Add("onblur", "fnSetBMI('" + txtRadWeight.ClientID + "','" + txtRadHeight.ClientID + "','" + txtRadBMI.ClientID + "')");
            //txtRadWeight.Attributes.Add("onblur", "fnSetBMI('" + txtRadWeight.ClientID + "','" + txtRadHeight.ClientID + "','" + txtRadBMI.ClientID + "')");
            btnChildAccompaniedByCaregiver.Attributes.Add("onclick", "fnGetToggleText('" + btnChildAccompaniedByCaregiver.ClientID + "','" + rcbcareGiverRelationship.ClientID + "')");
            radbtnMedicalCondition.Attributes.Add("onclick", "fnGetToggleText('" + radbtnMedicalCondition.ClientID + "','" + rcbmedicalCondition.ClientID + "')");
            rcbRegimenPrescribed.Attributes.Add("onChange", "fnGetComboOnchange('" + rcbRegimenPrescribed.ClientID + "','" + txtradOtherRegimen.ClientID + "')");
            RadbtnLabEvalution.Attributes.Add("onclick", "fnGetToggleText('" + RadbtnLabEvalution.ClientID + "','" + rcbLabEvalution.ClientID + "')");
            rcbProphylaxis.Attributes.Add("onChange", "fnGetComboOnchange('" + rcbProphylaxis.ClientID + "','" + txtOtherSpecify.ClientID + "')");
            //RadWindow1.Attributes.Add("OnClientClose","OnClientClose("

            String script = frmExpressForm_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            Session["CurrentForm"] = "frmKNHExpress";
            Session["FormIsLoaded"] = true;

            if (Session["IsFirstLoad"].ToString() == "true")
            {
                Session["Visit_pk"] = "0";// "179120";
                Form_Init();
                Session["IsFirstLoad"] = "false";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);


            }
            base.Page_Load(sender, e);
        }

        private void Form_Init()
        {
            BindCareGiver();
            BindrcbmedicalCondition();
            BindTBAassessment();
            BindTbFindings();
            BindRegimentPrescribed();
            BindLabEvaluation();
            BindCotrimoxazole();
            BindFpMethord();
            BindCCScreeningResults();
            BindOIProphylaxis();
            BindEmpoyee();
            //RadTabStrip1.SelectedIndex = 0;
            SetFieldValues();
            SetFieldVauesToControls();

        }
      


        #region Binding All dropdownlist
        
        protected void BindCareGiver()
        {
            rcbcareGiverRelationship.DataTextField = "Name";
            rcbcareGiverRelationship.DataValueField = "ID";
            rcbcareGiverRelationship.DataSource = GetDataTable("CareGiver"); 
            rcbcareGiverRelationship.DataBind();
        }
        
        protected void BindrcbmedicalCondition()
        {
            DataTable dt = GetDataTable("MedicalCondition");
            foreach (DataRow row in dt.Rows)
            {
                string itemName = row["Name"].ToString();
                string itemVal = row["ID"].ToString();
                RadComboBoxItem item = new RadComboBoxItem(itemName, itemVal);
                if (Convert.ToInt32(row["CheckedVal"]) > 0)
                {
                    item.Checked = true;
                }
                
                rcbmedicalCondition.Items.Add(item);
            }
        }
        
        protected void BindTBAassessment()
        {
            DataTable dt = GetDataTable("TBAassessment");
            rcbTBAassessment.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string itemName = row["Name"].ToString();
                string itemVal = row["ID"].ToString();
                RadComboBoxItem item = new RadComboBoxItem(itemName, itemVal);
                if (Convert.ToInt32(row["CheckedVal"]) > 0)
                {
                    item.Checked = true;
                }
                
                rcbTBAassessment.Items.Add(item);
            }
        }
        
        protected void BindTbFindings()
        {
            rcbTBFindings.DataTextField = "Name";
            rcbTBFindings.DataValueField = "ID";
            rcbTBFindings.DataSource = GetDataTable("TBFindings");
            rcbTBFindings.DataBind();
        }
        
        protected void BindRegimentPrescribed()
        {
            rcbRegimenPrescribed.DataTextField = "Name";
            rcbRegimenPrescribed.DataValueField = "ID";
            rcbRegimenPrescribed.DataSource = GetDataTable("RegimentPrescribed");
            rcbRegimenPrescribed.DataBind();
        }
        
        protected void BindLabEvaluation()
        {
            DataTable dt = GetDataTable("LabEvaluation");
            foreach (DataRow row in dt.Rows)
            {
                string itemName = row["Name"].ToString();
                string itemVal = row["ID"].ToString();
                RadComboBoxItem item = new RadComboBoxItem(itemName, itemVal);
                if (Convert.ToInt32(row["CheckedVal"]) > 0)
                {
                    item.Checked = true;
                }
                
                rcbLabEvalution.Items.Add(item);
            }
        }
        string SetRadbtnToggleState(int intval)
        {
            string retval = "No";
            if (intval == 1)
            {
                retval = "Yes";
            }
            else
            {
                retval = "No";
            }
            return retval;
        }
        protected void SetFieldValues()
        {
           

            DataTable dt = GetDataTable("ExpressFormDetail");
            if (dt.Rows.Count == 0)
            {
                return;
            }

            objExp.ID = int.Parse(dt.Rows[0]["visit_pk"].ToString());
            objExp.ChildAccompaniedByCaregiver= Convert.ToInt32((dt.Rows[0]["ChildAccompaniedByCaregiver"]));
            objExp.TreatmentSupporterRelationship= int.Parse(dt.Rows[0]["TreatmentSupporterRelationship"].ToString());
            objExp.Temperature= decimal.Parse(dt.Rows[0]["Temperature"].ToString());
            objExp.RespirationRate= decimal.Parse(dt.Rows[0]["RespirationRate"].ToString());
            objExp.HeartRate= decimal.Parse(dt.Rows[0]["HeartRate"].ToString());
            objExp.SystolicBloodPressure= decimal.Parse(dt.Rows[0]["SystolicBloodPressure"].ToString());
            objExp.SystolicBloodPressure= decimal.Parse(dt.Rows[0]["DiastolicBloodPressure"].ToString());
            objExp.MedicalCondition = Convert.ToInt32(dt.Rows[0]["MedicalCondition"]);
            objExp.OnFollowUp = Convert.ToInt32(dt.Rows[0]["OnFollowUp"]);
            objExp.LastFollowUpDate= DateTime.Parse(dt.Rows[0]["LastFollowUpDate"].ToString());
            objExp.PreviousAdmission = Convert.ToInt32(dt.Rows[0]["PreviousAdmission"]);
            objExp.PreviousAdmissionDiagnosis= dt.Rows[0]["PreviousAdmissionDiagnosis"].ToString();
            objExp.PreviousAdmissionStart=DateTime.Parse(dt.Rows[0]["PreviousAdmissionStart"].ToString());
            objExp.PreviousAdmissionEnd=DateTime.Parse(dt.Rows[0]["PreviousAdmissionEnd"].ToString());
            objExp.TBAssessmentIcf = Convert.ToInt32(dt.Rows[0]["TBAssessmentIcf"]);
            objExp.TBFindings= int.Parse(dt.Rows[0]["TBFindings"].ToString());
            objExp.RegimenPrescribedFup= dt.Rows[0]["RegimenPrescribedFup"].ToString();
            objExp.LabEvaluationPeads = Convert.ToInt32(dt.Rows[0]["LabEvaluationPeads"]);
            objExp.OIProphylaxis = Convert.ToInt32(dt.Rows[0]["OIProphylaxis"]);
            objExp.OtherOIProphylaxis = dt.Rows[0]["OtherOIProphylaxis"].ToString();
            objExp.TreatmentPlan = dt.Rows[0]["TreatmentPlan"].ToString();
            objExp.PwPMessagesGiven = Convert.ToInt32(dt.Rows[0]["PwPMessagesGiven"]);
            objExp.CondomsIssued = Convert.ToInt32(dt.Rows[0]["CondomsIssued"]);
            objExp.ReasonfornotIssuingCondoms = dt.Rows[0]["ReasonfornotIssuingCondoms"].ToString();
            objExp.IntentionOfPregnancy = Convert.ToInt32(dt.Rows[0]["IntentionOfPregnancy"]);
            objExp.DiscussedDualContraception = Convert.ToInt32(dt.Rows[0]["DiscussedDualContraception"]);
            objExp.DiscussedFertilityOption = Convert.ToInt32(dt.Rows[0]["DiscussedFertilityOption"]);
            objExp.OnFP = Convert.ToInt32(dt.Rows[0]["OnFP"]);
            objExp.FPmethod = Convert.ToInt32(dt.Rows[0]["FPmethod"]);
            objExp.CervicalCancerScreened = Convert.ToInt32(dt.Rows[0]["CervicalCancerScreened"]);
            objExp.ReferredForCervicalCancerScreening = Convert.ToInt32(dt.Rows[0]["ReferredForCervicalCancerScreening"]);
            objExp.CervicalCancerScreeningResults = Convert.ToInt32(dt.Rows[0]["CervicalCancerScreeningResults"]);
            if (dt.Rows[0]["NextApointmentDate"] != DBNull.Value)
            {
                objExp.NextApointmentDate = Convert.ToDateTime(dt.Rows[0]["NextApointmentDate"]);
            }
            objExp.RegimenPrescribed = Convert.ToInt32(dt.Rows[0]["RegimenPrescribed"]);
            objExp.OtherRegimenPrescribed = dt.Rows[0]["OtherRegimenPrescribed"].ToString();
            if (dt.Rows[0]["LatestViralLoad"] != DBNull.Value)
            {
                objExp.LatestViralLoad = Convert.ToInt32(dt.Rows[0]["LatestViralLoad"]);
            }
            if (dt.Rows[0]["LatestViralLoadDate"] != DBNull.Value)
            {
                objExp.LatestViralLoadDate = Convert.ToDateTime(dt.Rows[0]["LatestViralLoadDate"]);
            }
            objExp.ResultsCervicalCancer = Convert.ToInt32(dt.Rows[0]["ResultsCervicalCancer"]);
            objExp.ReasonCTXpresribed = Convert.ToInt32(dt.Rows[0]["ReasonCTXpresribed"]);
            objExp.VisitDate = Convert.ToDateTime(dt.Rows[0]["VisitDate"]);
            objExp.Signature = Convert.ToInt32(dt.Rows[0]["signature"]);
            objExp.Height = decimal.Parse(dt.Rows[0]["Height"].ToString());
            objExp.Weight = decimal.Parse(dt.Rows[0]["Weight"].ToString());
        }
        protected void SetFieldVauesToControls()
        {
            if (objExp.ID == null || objExp.ID == 0)
            {
                return;
            }

            HiddVisit_pk.Value = objExp.ID.ToString();
            btnChildAccompaniedByCaregiver.SetSelectedToggleStateByText(CatchUpVaue(objExp.ChildAccompaniedByCaregiver));
            rcbcareGiverRelationship.SelectedValue = objExp.TreatmentSupporterRelationship.ToString();
            rcbcareGiverRelationship.EnableLoadOnDemand = false;
            HiddRadTemperature.Value = objExp.Temperature.ToString();
            HiddRadRespirationRate.Value = objExp.RespirationRate.ToString();
            HiddRadHeartRate.Value = objExp.HeartRate.ToString();
            HiddRadSystollicBloodPressure.Value = objExp.SystolicBloodPressure.ToString();
            HiddRadDiastolicBloodPressure.Value = objExp.DiastolicBloodPressure.ToString();
            HiddRadHeight.Value = objExp.Height.ToString();
            HiddRadWeight.Value = objExp.Weight.ToString();
            radbtnMedicalCondition.SetSelectedToggleStateByText(CatchUpVaue(objExp.MedicalCondition));
            radbtnFollowup.SetSelectedToggleStateByText(CatchUpVaue(objExp.OnFollowUp));
            RadDateLastFolowup.DbSelectedDate = objExp.LastFollowUpDate.ToString();
            RadBtnAdmitted.SetSelectedToggleStateByText(CatchUpVaue(objExp.PreviousAdmission));
            txtradDiagnosis.Text = objExp.PreviousAdmissionDiagnosis.ToString();
            RadDateAdmissionDate.DbSelectedDate = objExp.PreviousAdmissionStart.ToString();
            RadDateAdmissionEnd.DbSelectedDate = objExp.PreviousAdmissionEnd.ToString();
            dtVisit.DbSelectedDate = objExp.VisitDate.ToString();
            rcbSignature.SelectedValue = objExp.Signature.ToString();
            rcbSignature.EnableLoadOnDemand = false;
            rcbTBFindings.SelectedValue = objExp.TBFindings.ToString();
            rcbTBFindings.EnableLoadOnDemand = false;
            rcbRegimenPrescribed.SelectedValue = objExp.RegimenPrescribed.ToString();
            rcbRegimenPrescribed.EnableLoadOnDemand = false;
            txtradOtherRegimen.Text = objExp.OtherRegimenPrescribed.ToString();
            RadbtnLabEvalution.SetSelectedToggleStateByText(CatchUpVaue(objExp.LabEvaluationPeads));
            rcbProphylaxis.SelectedValue = objExp.OIProphylaxis.ToString();
            rcbProphylaxis.EnableLoadOnDemand = false;
            txtOtherSpecify.Text = objExp.OtherOIProphylaxis.ToString();
            txtTreatmentplan.Text = objExp.TreatmentPlan.ToString();
            radpwpmessagegiven.SetSelectedToggleStateByText(CatchUpVaue(objExp.PwPMessagesGiven));
            radbtnissueCondoms.SetSelectedToggleStateByText(CatchUpVaue(objExp.CondomsIssued));
            txtReasonforCondomNotIssued.Text = objExp.ReasonfornotIssuingCondoms.ToString();
            radbtnPregncyIntfnxtvisit.SetSelectedToggleStateByText(CatchUpVaue(objExp.IntentionOfPregnancy));
            radbtnNoDualconta.SetSelectedToggleStateByText(CatchUpVaue(objExp.DiscussedDualContraception));
            radbtnddiscussferti.SetSelectedToggleStateByText(CatchUpVaue(objExp.DiscussedFertilityOption));
            radbtnfpmethord.SetSelectedToggleStateByText(CatchUpVaue(objExp.OnFP));
            rcbFpMethord.SelectedValue = objExp.FPmethod.ToString();
            rcbFpMethord.EnableLoadOnDemand = false;
            radbtncervicalcancer.SetSelectedToggleStateByText(CatchUpVaue(objExp.CervicalCancerScreened));
            rcbCCScreeningResults.SelectedValue = objExp.CervicalCancerScreeningResults.ToString();
            rcbCCScreeningResults.EnableLoadOnDemand = false;
            radbtnccscreeningref.SetSelectedToggleStateByText(CatchUpVaue(objExp.ReferredForCervicalCancerScreening));
        }
        
        protected void BindCotrimoxazole()
        {
            rcbCotrimoxazole.DataTextField = "Name";
            rcbCotrimoxazole.DataValueField = "ID";
            rcbCotrimoxazole.DataSource = GetDataTable("Cotrimoxazole");
            rcbCotrimoxazole.DataBind();
        }
        
        protected void BindFpMethord()
        {
            rcbFpMethord.DataTextField = "Name";
            rcbFpMethord.DataValueField = "ID";
            rcbFpMethord.DataSource = GetDataTable("FPMethod");
            rcbFpMethord.DataBind();
        }
        
        protected void BindCCScreeningResults()
        {
            rcbCCScreeningResults.DataTextField = "Name";
            rcbCCScreeningResults.DataValueField = "ID";
            rcbCCScreeningResults.DataSource = GetDataTable("CCScreeningResults");
            rcbCCScreeningResults.DataBind();
        }
        
        protected void BindOIProphylaxis()
        {
            rcbProphylaxis.DataTextField = "Name";
            rcbProphylaxis.DataValueField = "ID";
            rcbProphylaxis.DataSource = GetDataTable("OIProphylaxis");
            rcbProphylaxis.DataBind();
        }
        protected void BindEmpoyee()
        {
            rcbSignature.DataTextField = "EmployeeName";
            rcbSignature.DataValueField = "EmployeeId";
            rcbSignature.DataSource = GetDataTable("Employee_Master");
            rcbSignature.DataBind();
        }

        
        
        //private void BindEmpoyee(RadComboBox rcb)
        //{
        //    DataSet theDS = new DataSet();
        //    theDS.ReadXml(MapPath("..\\..\\XMLFiles\\ALLMasters.con"));
        //    BindFunctions BindManager = new BindFunctions();
        //    IQCareUtils theUtils = new IQCareUtils();
            
        //    rcb.DataSource = null;
        //    rcb.Items.Clear();
            
        //    if (theDS.Tables["Mst_Employee"] != null)
        //    {
        //        DataView theDV = new DataView(theDS.Tables["Mst_Employee"]);
        //        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        //        {
        //            theDV.RowFilter = "DeleteFlag=0";
        //            if (theDV.Table != null)
        //            {
        //                DataTable theDT = theUtils.CreateTableFromDataView(theDV);
        //                if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0)
        //                {
        //                    theDV = new DataView(theDT);
        //                    theDV.RowFilter = "EmployeeId =" + Session["AppUserEmployeeId"].ToString();
        //                    if (theDV.Count > 0)
        //                        theDT = theUtils.CreateTableFromDataView(theDV);
        //                }
        //                BindManager.BindCombo(rcb, theDT, "EmployeeName", "EmployeeId");
        //            }
        //        }
        //        else
        //        {
        //            if (theDV.Table != null)
        //            {
        //                DataTable theDT = theUtils.CreateTableFromDataView(theDV);
        //                if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0)
        //                {
        //                    theDV = new DataView(theDT);
        //                    theDV.RowFilter = "EmployeeId =" + Session["AppUserEmployeeId"].ToString();
        //                    if (theDV.Count > 0)
        //                        theDT = theUtils.CreateTableFromDataView(theDV);
        //                }
        //                BindManager.BindCombo(rcb, theDT, "EmployeeName", "EmployeeId");
        //            }
        //        }
        //    }
        //}
        
        #endregion
        
        protected DataTable GetDataTable(string flag)
        {
            BIQTouchExpressFields objExpressFields = new BIQTouchExpressFields();
            objExpressFields.Flag = flag;
            objExpressFields.PtnPk = Convert.ToInt32(Session["PatientID"].ToString());
            objExpressFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
            objExpressFields.ID = Int32.Parse(Session["Visit_pk"].ToString());

            IQTouchKNHExpress theExpressManager;
            theExpressManager = (IQTouchKNHExpress)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchKNHExpress, BusinessProcess.Clinical");
            DataTable dt = theExpressManager.IQTouchGetKnhExpressData(objExpressFields);
            return dt;
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
        
        string CatchUpVaue(int intval)
        {
            string retval = "No";
            if (intval == 1)
            {
                retval = "Yes";
            }
            else
            {
                retval = "No";
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
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == false)
            {
                return;
            }
            SaveForm();
        }
        
        #region Save Express form details
            
        protected Boolean ValidateForm()
        {
            if (btnChildAccompaniedByCaregiver.SelectedToggleState.Text.ToString() == "Yes" && rcbcareGiverRelationship.SelectedValue=="")
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                theMsg.Text = "Select Caregivers relationship.";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                rcbcareGiverRelationship.Focus();
                return false;

            }
            if (radbtnMedicalCondition.SelectedToggleState.Text.ToString() == "Yes" && rcbmedicalCondition.SelectedValue == "")
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                theMsg.Text = "Select Pre existing medical condition.";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                rcbmedicalCondition.Focus();
                return false;

            }
            if (radbtnFollowup.SelectedToggleState.Text.ToString() == "Yes" && RadDateLastFolowup.DbSelectedDate ==null)
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                theMsg.Text = "Enter Last Follow up date.";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                RadDateLastFolowup.Focus();
                return false;

            }
            if (radbtnFollowup.SelectedToggleState.Text.ToString() == "Yes" && RadDateLastFolowup.DbSelectedDate == null)
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                theMsg.Text = "Enter Last Follow up date.";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                RadDateLastFolowup.Focus();
                return false;

            }

            if (RadBtnAdmitted.SelectedToggleState.Text.ToString() == "Yes")
            {
                
                if (RadDateAdmissionDate.SelectedDate == null)
                {
                    RawMessage theMsg;
                    theMsg.Text = "Enter Admission Start Date.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;

                }
            }
            if (RadBtnAdmitted.SelectedToggleState.Text.ToString() == "Yes")
            {

                if (RadDateAdmissionEnd.SelectedDate == null)
                {
                    RawMessage theMsg;
                    theMsg.Text = "Enter Admission End Date.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;

                }
            }




            if (RadDateAdmissionDate.SelectedDate != null && RadDateAdmissionEnd.SelectedDate != null)
            {
                if (Convert.ToDateTime(RadDateAdmissionDate.SelectedDate.Value) > Convert.ToDateTime(RadDateAdmissionEnd.SelectedDate.Value))
                {
                    RawMessage theMsg;
                    theMsg.Text = "Admission Start Date should less than equal to Admission End date.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;

                }
            }
            if (dtVisit.SelectedDate == null )
            {
                    RawMessage theMsg;
                    theMsg.Text = "Enter Visit Date.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;

                
            }

            if (rcbRegimenPrescribed.SelectedItem != null)
            {
                if (rcbRegimenPrescribed.SelectedItem.Text.ToString() == "Other" && txtradOtherRegimen.Text == "")
                {
                    //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                    RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                    theMsg.Text = "Other regimen can not blank.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    txtradOtherRegimen.Focus();
                    return false;

                }
            }
            if (RadbtnLabEvalution.SelectedToggleState.Text.ToString() == "Yes" && rcbLabEvalution.CheckedItems.Count == 0)
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                theMsg.Text = "Select any specify lab evaluation";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                rcbLabEvalution.Focus();
                return false;

            }

            if (radbtnfpmethord.SelectedToggleState.Text.ToString() == "Yes" && rcbFpMethord.SelectedValue == "")
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                theMsg.Text = "Select any FP Methord";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                rcbFpMethord.Focus();
                return false;

            }

            if (radbtncervicalcancer.SelectedToggleState.Text.ToString() == "Yes" && rcbCCScreeningResults.SelectedValue == "")
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                theMsg.Text = "Select any Ca cervix screening results";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                rcbCCScreeningResults.Focus();
                return false;

            }

            if (rcbProphylaxis.SelectedItem!= null)
            {
                if (rcbProphylaxis.SelectedItem.Text.ToString() == "Other" && txtOtherSpecify.Text == "")
                {
                    //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                    RawMessage theMsg;// = MsgRepository.GetMessage("IQTouchOrderedByName");
                    theMsg.Text = "Other (specify) can not blank.";
                    txtOtherSpecify.Focus();
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;

                }
            }

            


            return true;

        }

      
        protected void SaveForm()
        {
            List<BIQTouchExpressFields> list = new List<BIQTouchExpressFields>();
            BIQTouchExpressFields objExpressFields = new BIQTouchExpressFields();
            var checkedItemMedicalCondition = rcbmedicalCondition.CheckedItems;
            var checkedItemTBScreening = rcbTBAassessment.CheckedItems;
            var checkedItemLabEvaluation = rcbLabEvalution.CheckedItems;

            
            try
            {



                if (HiddVisit_pk.Value != "")
                {
                    objExpressFields.ID = Convert.ToInt32(HiddVisit_pk.Value);
                }
                else
                {
                    objExpressFields.ID = 0;

                }


                 
                    // Asigning value to Property Triage Tab
                    objExpressFields.PtnPk = Convert.ToInt32(Request.QueryString["PatientID"]); 
                    objExpressFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
                    objExpressFields.UserId = Int32.Parse(Session["AppUserId"].ToString());
                    objExpressFields.ChildAccompaniedByCaregiver = CheckedVaue(btnChildAccompaniedByCaregiver.SelectedToggleState.Text.ToString());
                    if (btnChildAccompaniedByCaregiver.SelectedToggleState.Text.ToString() == "Yes")
                    {
                        objExpressFields.TreatmentSupporterRelationship = Convert.ToInt32(rcbcareGiverRelationship.SelectedValue.ToString());
                    }


                    objExpressFields.Temperature = Decimal.Parse(HiddRadTemperature.Value.ToString());
                    objExpressFields.RespirationRate = Decimal.Parse(HiddRadRespirationRate.Value.ToString());
                    objExpressFields.HeartRate = Decimal.Parse(HiddRadHeartRate.Value.ToString());
                    objExpressFields.SystolicBloodPressure = Decimal.Parse(HiddRadSystollicBloodPressure.Value.ToString());
                    objExpressFields.DiastolicBloodPressure = Decimal.Parse(HiddRadDiastolicBloodPressure.Value.ToString());
                    objExpressFields.Height = Decimal.Parse(HiddRadHeight.Value.ToString());
                    objExpressFields.Weight = Decimal.Parse(HiddRadWeight.Value.ToString());

                  
                    objExpressFields.MedicalCondition = CheckedVaue(radbtnMedicalCondition.SelectedToggleState.Text.ToString());
                    objExpressFields.SpecificMedicalCondition = 0;
                    objExpressFields.OnFollowUp = CheckedVaue(radbtnFollowup.SelectedToggleState.Text.ToString());
                    if (radbtnFollowup.SelectedToggleState.Text.ToString() == "Yes")
                    {
                        objExpressFields.LastFollowUpDate = DateGiven(RadDateLastFolowup.DbSelectedDate.ToString());
                    }
                    else
                    {
                        objExpressFields.LastFollowUpDate = DateGiven("");
                    }

                    
                    objExpressFields.PreviousAdmission = CheckedVaue(RadBtnAdmitted.SelectedToggleState.Text.ToString());
                    objExpressFields.PreviousAdmissionDiagnosis = txtradDiagnosis.Text.ToString();
                    if (RadDateAdmissionDate.DbSelectedDate != null && RadDateAdmissionEnd.DbSelectedDate != null)
                    {
                        objExpressFields.PreviousAdmissionStart = DateGiven(RadDateAdmissionDate.DbSelectedDate.ToString());
                        objExpressFields.PreviousAdmissionEnd = DateGiven(RadDateAdmissionEnd.DbSelectedDate.ToString());
                    }
                    else
                    {
                        objExpressFields.PreviousAdmissionStart = DateGiven("");
                        objExpressFields.PreviousAdmissionEnd = DateGiven("");
                    }
                    objExpressFields.VisitDate = DateGiven(dtVisit.DbSelectedDate.ToString());
                    if(rcbSignature.SelectedValue!="")
                    {
                    objExpressFields.Signature = Int32.Parse(rcbSignature.SelectedValue.ToString());
                    }
                    else
                    {
                        objExpressFields.Signature=0;
                    }
                    // Asigning value top property Clinical assessment
                    //rcbTBAassessment Multi value
                    //SpecifyLabEvaluation Multi value
                    if (rcbTBFindings.SelectedValue != "")
                    {
                        objExpressFields.TBFindings = Int32.Parse(rcbTBFindings.SelectedValue.ToString());
                    }
                    if (rcbRegimenPrescribed.SelectedValue != "")
                    {
                        objExpressFields.RegimenPrescribed = Int32.Parse(rcbRegimenPrescribed.SelectedValue.ToString());
                    }
                    objExpressFields.OtherRegimenPrescribed = txtradOtherRegimen.Text.ToString();
                    objExpressFields.LabEvaluationPeads = CheckedVaue(RadbtnLabEvalution.SelectedToggleState.Text.ToString());
                    if (rcbProphylaxis.SelectedValue != "")
                    {
                        objExpressFields.OIProphylaxis = Int32.Parse(rcbProphylaxis.SelectedValue.ToString());
                    }
                    objExpressFields.OtherOIProphylaxis = txtOtherSpecify.Text.ToString();
                    objExpressFields.TreatmentPlan = txtTreatmentplan.Text.ToString();
                    objExpressFields.PwPMessagesGiven = CheckedVaue(radpwpmessagegiven.SelectedToggleState.Text.ToString());
                    objExpressFields.CondomsIssued = CheckedVaue(radbtnissueCondoms.SelectedToggleState.Text.ToString());
                    objExpressFields.ReasonfornotIssuingCondoms = txtReasonforCondomNotIssued.Text.ToString();
                    objExpressFields.IntentionOfPregnancy = CheckedVaue(radbtnPregncyIntfnxtvisit.SelectedToggleState.Text.ToString());
                    objExpressFields.DiscussedDualContraception = CheckedVaue(radbtnNoDualconta.SelectedToggleState.Text.ToString());
                    objExpressFields.DiscussedFertilityOption = CheckedVaue(radbtnddiscussferti.SelectedToggleState.Text.ToString());
                    objExpressFields.OnFP = CheckedVaue(radbtnfpmethord.SelectedToggleState.Text.ToString());
                    if (radbtnfpmethord.SelectedToggleState.Text.ToString() == "Yes")
                    {
                        objExpressFields.FPmethod = Int32.Parse(rcbFpMethord.SelectedValue.ToString());
                    }
                    objExpressFields.CervicalCancerScreened = CheckedVaue(radbtncervicalcancer.SelectedToggleState.Text.ToString());
                    if (radbtncervicalcancer.SelectedToggleState.Text.ToString() == "Yes")
                    {
                        objExpressFields.CervicalCancerScreeningResults = Int32.Parse(rcbCCScreeningResults.SelectedValue.ToString());
                    }
                    objExpressFields.ReferredForCervicalCancerScreening = CheckedVaue(radbtnccscreeningref.SelectedToggleState.Text.ToString());
                    objExpressFields.Flag = "1";

                    list.Add(objExpressFields);

                
                // Adding MultiSelect value Pre-Existing Medical Value

                if (checkedItemMedicalCondition.Count > 0 && radbtnMedicalCondition.SelectedToggleState.Text.ToString() == "Yes")
                {
                    foreach (var item in checkedItemMedicalCondition)
                    {
                        BIQTouchExpressFields obj = new BIQTouchExpressFields();
                        obj.PtnPk = Convert.ToInt32(Request.QueryString["PatientID"]);
                        obj.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
                        obj.UserId = Int32.Parse(Session["AppUserId"].ToString());
                        obj.MedicalCondition = CheckedVaue(radbtnMedicalCondition.SelectedToggleState.Text.ToString());
                        obj.SpecificMedicalCondition = Int32.Parse(item.Value);
                        obj.LastFollowUpDate=DateGiven("");
                        obj.PreviousAdmissionStart=DateGiven("");
                        obj.PreviousAdmissionEnd=DateGiven("");
                        obj.VisitDate = DateGiven(dtVisit.DbSelectedDate.ToString());
                        obj.Flag = "2";
                        obj.ID = objExp.ID;
                        list.Add(obj);
                    }
                }

                // Adding MultiSelect value TBScreening

                if (checkedItemTBScreening.Count > 0)
                {
                    foreach (var item in checkedItemTBScreening)
                    {
                        BIQTouchExpressFields obj = new BIQTouchExpressFields();
                        obj.PtnPk = Convert.ToInt32(Session["PatientID"].ToString());
                        obj.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
                        obj.UserId = Int32.Parse(Session["AppUserId"].ToString());
                        obj.TBAssessmentIcf = Int32.Parse(item.Value);
                        obj.LastFollowUpDate = DateGiven("");
                        obj.PreviousAdmissionStart = DateGiven("");
                        obj.PreviousAdmissionEnd = DateGiven("");
                        obj.VisitDate = DateGiven(dtVisit.DbSelectedDate.ToString());
                        obj.Flag = "2";
                        obj.ID = objExp.ID;
                        list.Add(obj);
                    }
                }

                // Adding MultiSelect value LAB Evaluation

                if (checkedItemLabEvaluation.Count > 0 && RadbtnLabEvalution.SelectedToggleState.Text.ToString() == "Yes")
                {
                    foreach (var item in checkedItemLabEvaluation)
                    {
                        BIQTouchExpressFields obj = new BIQTouchExpressFields();
                        obj.PtnPk = Convert.ToInt32(Request.QueryString["PatientID"]);
                        obj.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
                        obj.UserId = Int32.Parse(Session["AppUserId"].ToString());
                        obj.LabEvaluationPeads = CheckedVaue(RadbtnLabEvalution.SelectedToggleState.Text.ToString());
                        obj.SpecifyLabEvaluation = Int32.Parse(item.Value);
                        obj.LastFollowUpDate = DateGiven("");
                        obj.PreviousAdmissionStart = DateGiven("");
                        obj.PreviousAdmissionEnd = DateGiven("");
                        obj.VisitDate = DateGiven(dtVisit.DbSelectedDate.ToString());
                        obj.Flag = "2";
                        obj.ID = objExp.ID;
                        list.Add(obj);
                    }
                }
                


                
                IQTouchKNHExpress theExpressManager;
                theExpressManager = (IQTouchKNHExpress)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchKNHExpress, BusinessProcess.Clinical");
                int result = theExpressManager.IQTouchSaveExpressDetails(list);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackBtnClick();", true);

                   
                }

            }
            catch (Exception ex)
            {
                IsError = true;
                
            }
            if (IsError)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('An errror occured please contact your Administrator')", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "clsLoadingPanelDueToError", "parent.CloseLoading();", true);
            }
        }

        #endregion
    }
}