using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

//IQCare usings
using Interface.Clinical;
using Interface.Pharmacy;
using Interface.Administration;
using Touch.FormObjects;
using Application.Presentation;
using Application.Common;
using BusinessProcess.Clinical;
using Telerik.Web.UI;

namespace Touch.Custom_Forms
{
    public partial class frmRegistrationTouch : TouchUserControlBase
    {
        private static List<Allergen> AllergenArray = new List<Allergen>();
        private static List<PriorArtClass> PriorARTArray = new List<PriorArtClass>();
        string ObjFactoryParameter = "BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical";
        string ObjFactoryParameterDrug = "BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy";
        static objRegistration rg = new objRegistration();
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

        static int patientID = 0;

        protected void Page_Load(object s, EventArgs e)
        {
            

            Session["CurrentForm"] = "frmRegistrationTouch";
            Session["FormIsLoaded"] = true;

            if (Session["IsFirstLoad"] != null)
            {
                if (Session["IsFirstLoad"].ToString() == "true")
                {
                    Session["IsFirstLoad"] = "false";
                    var rgReset = new objRegistration();
                    rg = rgReset;
                    Init_Form();
                }
            }

            GetAllGrids();

            base.Page_Load(s, e);

            /***************** Check For User Rights ****************/
            AuthenticationManager Authentication = new AuthenticationManager();

            btnPrint.Visible = Authentication.HasFunctionRight(ApplicationAccess.PASDPRegistrationForm, FunctionAccess.Print, (DataTable)Session["UserRight"]);

            if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPRegistrationForm, FunctionAccess.Update, (DataTable)Session["UserRight"]))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showsave", "$('#divSave').hide();", true);
            }

            //then load JS
            //register javascript script
            String script = frmReg_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            if (IsPostBack) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "$(function(){resizeScrollbars();});", true);
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "loading", TouchGlobals.OnScKeyboard, true);
        }

        protected void Init_Form()
        {
            objRegistration theRegistration = new objRegistration();
            IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);

            patientID = int.Parse(Request.QueryString["patientId"].ToString());

            DataSet regDT = ptnMgr.GetRegistrationDetails(patientID, Session["AppLocationId"].ToString());

            SetFieldVals(regDT);

            SetFormVals();

            btnPrint.Visible = true;
            //Check if status is active
            Label labelStatus = (Label)Parent.FindControl("lblStatus");
            if (labelStatus.Text == "InActive")
                DisableControls(updtFormUpdate, false);
            else
                DisableControls(updtFormUpdate, true);
            updtFormUpdate.Update();
        }

        protected void SetFormVals()
        {
            CleanVals();
            GetAllDropDowns();
            //GetAllGrids();

            //////////// Set Patient Info Tab //////////////
            txtFirstName.Text = rg.FirstName;
            txtLastName.Text = rg.LastName;

            DateTime PDOBVal; if (DateTime.TryParse(rg.DOB, out PDOBVal))
            {
                dtPatientDOB.SelectedDate = PDOBVal;
                SetAge(PDOBVal, AgeMonthZZZ, AgeYearZZZ);
            }

            if (rg.Sex > 0) cbSex.SelectedValue = rg.Sex.ToString();

            DateTime dtRegVal; if (DateTime.TryParse(rg.RegistrationDate, out dtRegVal)) dtRegistrationDate.SelectedDate = dtRegVal;

            txtAddress.Text = rg.Address;
            txtSuburb.Text = rg.Suburb;

            if (rg.SubDistrict != "") rcbSubDistrict.SelectedValue = rg.SubDistrict;
            if (rg.District != "") rcbDistrict.SelectedValue = rg.District;

            txtPatientPhoneNo.Text = rg.TelephoneNo;
            txtAddressComment.Text = rg.Addresscomments;
            txtPatientPostalAddress.Text = rg.PostalAddress;
            txtPatientPostalCode.Text = rg.PostalCode;

            if (rg.EntryPoint != "")
            {
                foreach (RadComboBoxItem item in cbEntryPoint.Items)
                {
                    if (item.Value.ToString().Contains(rg.EntryPoint))
                    {
                        item.Selected = true;
                        if (item.Text == "Other")
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "shentry", "ShowHide('ShowIfEntryOther', 'show', null)", true);
                        break;
                    }
                }
            }
            txtEntryPointOther.Text = rg.OtherEntryPoint;

            ////// end of Patient Info form /////

            //---------------------------------------------------------------

            //////////// Set Caregiver Info Tab //////////////

            string[] CGNames = rg.CareGiverName.Split('|');
            txtCGFirstName.Text = CGNames[0];
            if (CGNames.Length > 1) txtCGLastName.Text = CGNames[1];

            DateTime CGDOBVal; if (DateTime.TryParse(rg.CareGiverDOB, out CGDOBVal))
            {
                dtCGDOB.SelectedDate = CGDOBVal;
                SetAge(CGDOBVal, txtCGMonthZZZ, txtCGYearZZZ);
            }

            if (rg.CareGiverRelationship.ToString() != "")
            {
                foreach (RadComboBoxItem item in cbCGRelationship.Items)
                {
                    if (item.Value.ToString().Contains(rg.CareGiverRelationship.ToString()))
                    {
                        item.Selected = true;
                        if (item.Text == "Other")
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "shCG", "ShowHide('ShowIfOtherRelationship', 'show', null)", true);
                        break;
                    }
                }
            }

            if (rg.CareGiverGender > 0) cbCGSex.SelectedValue = rg.CareGiverGender.ToString();

            //if (rg.CareGiverRelationship > 0) cbCGRelationship.SelectedValue = rg.CareGiverRelationship.ToString();
            txtOtherRelationship.Text = rg.OtherCareGiver;

            txtCGPhoneNo.Text = rg.CareGiverTelephone;

            ////// end of Caregiver Info form /////

            //---------------------------------------------------------------

            //////////// Set Mothers History Tab //////////////

            txtMotherName.Text = rg.MotherName;

            if (rg.MotherAliveYN != null)
            {
                if ((bool)rg.MotherAliveYN)
                    rbtnMotherAliveYes.Checked = true;
                else
                    rbtnMotherAliveNo.Checked = true;
            }

            SetYNCombo(cbMotherReceivedPMTCT, rg.MotherPMTCTdrugsYN.ToString(), "ShowIfMotherRecievedYes");

            //TODO: cycle through regimen value and tick appropriate generic drugs for Mother PMTCT
            if (rg.MotherPMTCTdrugs.Count > 0)
            {
                foreach (int li in rg.MotherPMTCTdrugs)
                {
                    foreach (RadComboBoxItem item in cbMotherPMTCTDrugs.Items)
                    {
                        if (item.Value.ToString().Contains(li.ToString()))
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                }
            }

            SetYNCombo(cbChildReceivedPMTCT, rg.ChildPMTCTdrugsYN.ToString(), "ShowIfChildReceivedYes");

            //TODO: cycle through regimen value and tick appropriate generic drugs for Child PMTCT
            if (rg.ChildPMTCTdrugs.Count > 0)
            {
                foreach (int li in rg.ChildPMTCTdrugs)
                {
                    foreach (RadComboBoxItem item in cbChildPMTCTDrugs.Items)
                    {
                        if (item.Value.ToString().Contains(li.ToString()))
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                }
            }

            cbMotherOnART.SelectedValue = rg.MotherARTYN.ToString();
            cbFeedingOption.SelectedValue = rg.FeedingOption.ToString();

            ////// end of Mothers History form /////

            //---------------------------------------------------------------

            //////////// Set HIV Care Tab //////////////

            DateTime ConfirmedHIVP; if (DateTime.TryParse(rg.DateConfirmedHIVPositive, out ConfirmedHIVP))
                dtConfirmedPos.SelectedDate = ConfirmedHIVP;
            DateTime DateEnrolledHIVC; if (DateTime.TryParse(rg.DateEnrolledHIVCare, out DateEnrolledHIVC))
                dtEnrolledHIVCare.SelectedDate = DateEnrolledHIVC;

            cbWHOEnrollment.SelectedValue = rg.WHOStageAtEnrollment.ToString();

            ////// end of HIV Care form /////

            //---------------------------------------------------------------

            //////////// Set Transfer In Tab //////////////

            DateTime TransferInVal;
            if (DateTime.TryParse(rg.TransferInDate, out TransferInVal))
                if (TransferInVal.Year != 1900)
                    dtTransferIn.SelectedDate = TransferInVal;


            rcbSubDistrictTransferIn.SelectedValue = rg.FromDistrict.ToString();
            rcbFacilityIn.SelectedValue = rg.Facility;

            DateTime regimenVal;
            if (DateTime.TryParse(rg.DateStart, out regimenVal))
                if (regimenVal.Year != 1900)
                    dtDateRegimenStarted.SelectedDate = regimenVal;

            //TODO: cycle through regimen value and tick appropriate regimen for Transfer IN - Regimen
            //This has been done in OnItemBound Event of radcombobox

            txtWeight.Text = rg.Weight;
            txtHeight.Text = rg.Height;
            if ((rg.Weight != "0.0") && (rg.Height != "0.0"))
                GetBMI();
            cbWHOStage.SelectedValue = rg.WHOStageAtTransfer.ToString();

            if (rg.PriorART == "1")
            {
                cbPriorART.SelectedValue = rg.PriorART + "|ShowIfPriorARTYes|Yes|PriorGrid";
                foreach (var item in rg.PriorARTRegimens)
                {
                    PriorArtClass prc = new PriorArtClass(item.RegimenID.ToString(), item.Regimen, _CheckDate(item.PriorARTDateLastUsed));
                    if (!PriorArtClass.CheckPriorART(prc.RegimenID, ref PriorARTArray)) PriorARTArray.Add(prc);
                    rgPriorART.DataSource = PriorARTArray;
                }
                rgPriorART.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "shPriorG", "ShowHide('ShowIfPriorARTYes', 'show', 'PriorGrid')", true);
            }
            else
            {
                cbPriorART.SelectedValue = rg.PriorART + "|ShowIfPriorARTYes";
            }



            ////// end of Transfer In form /////

            //---------------------------------------------------------------

            //////////// Set Drug Allergy Tab //////////////

            foreach (var item in rg.DrugAllergies)
            {
                Allergen alg = new Allergen(item.AllergenID.ToString(), item.Allergen, item.TypeOfReaction, _CheckDate(item.DateOfAllergy), item.MedicalConditions);
                if (!Allergen.CheckAllergen(item.AllergenID.ToString(), ref AllergenArray)) AllergenArray.Add(alg);
                rgAllergies.DataSource = AllergenArray;
            }
            rgAllergies.DataBind();

            ////// end of Drug Allergy Tab/////

            //---------------------------------------------------------------
        }

        /// <summary>
        /// Sets the Registration object type for the form
        /// Note: uses column name instead of index
        /// </summary>
        /// <param name="regDS">The Dataset holding the tables with the reg data</param>
        /// <returns>the Registration object</returns>
        protected void SetFieldVals(DataSet regDS)
        {
            PriorARTArray.Clear();
            AllergenArray.Clear();

            DataTable regDT = regDS.Tables[0]; //### Patient info 
            DataTable conDT = regDS.Tables[1]; //### Caregiver info
            DataTable mohDT = regDS.Tables[2]; //### Mother's history info
            DataTable traDT = regDS.Tables[5]; //### Transfer In
            DataTable tr1DT = regDS.Tables[6]; //### Transfer In patient details
            DataTable tr2DT = regDS.Tables[7]; //### Transfer In Prior ART Regimen
            DataTable dagDT = regDS.Tables[8]; //### Drug Allergies

            //#### Patient Info Tab ########//
            if (regDT.Rows.Count > 0)
            {
                rg.LastName = regDT.Rows[0]["LastName"].ToString();
                rg.FirstName = regDT.Rows[0]["FirstName"].ToString();
                rg.DOB = regDT.Rows[0]["DOB"].ToString();
                if (regDT.Rows[0]["Sex"].ToString() != "") rg.Sex = _CheckInt(regDT.Rows[0]["Sex"].ToString());
                rg.RegistrationDate = regDT.Rows[0]["RegistrationDate"].ToString();
                rg.Address = regDT.Rows[0]["Address"].ToString();
                rg.Suburb = regDT.Rows[0]["Suburb"].ToString();
                rg.District = regDT.Rows[0]["District"].ToString();
                rg.SubDistrict = regDT.Rows[0]["Town"].ToString();
                rg.TelephoneNo = regDT.Rows[0]["Phone"].ToString();
                rg.EntryPoint = regDT.Rows[0]["ReferredFrom"].ToString();
                rg.OtherEntryPoint = regDT.Rows[0]["ReferredFromSpecify"].ToString();
                rg.SubDistrict = regDT.Rows[0]["Town"].ToString();
            }
            if (conDT.Rows.Count > 0)
            {
                rg.Addresscomments = conDT.Rows[0]["AddressComments"].ToString();
                rg.PostalAddress = conDT.Rows[0]["OtherAddress"].ToString();
                rg.PostalCode = conDT.Rows[0]["OtherPOBoxNo"].ToString();

                //#### Caregiver Info Tab ########//

                rg.CareGiverName = conDT.Rows[0]["GuardianName"].ToString();
                rg.CareGiverDOB = conDT.Rows[0]["Guardian_DOB"].ToString();
                rg.CareGiverGender = _CheckInt(conDT.Rows[0]["GuardianGender"].ToString());
                rg.CareGiverRelationship = _CheckInt(conDT.Rows[0]["EmergContactRelation"].ToString());
                rg.OtherCareGiver = conDT.Rows[0]["EmergContactRelationOther"].ToString();
                rg.CareGiverTelephone = conDT.Rows[0]["EmergContactPhone"].ToString();
            }

            if (mohDT.Rows.Count > 0)
            {
                //#### Mother's History Tab ########//

                rg.MotherName = mohDT.Rows[0]["MotherName"].ToString();
                if (mohDT.Rows[0]["status"] != DBNull.Value)
                {
                    if ((int)mohDT.Rows[0]["status"] < 2)
                    {
                        if (mohDT.Rows[0]["status"].ToString() != "") rg.MotherAliveYN = Convert.ToBoolean((int)mohDT.Rows[0]["status"]);
                    }
                }
                if (mohDT.Rows[0]["MotherReceivedDrugforPMTCT"].ToString() != "") rg.MotherPMTCTdrugsYN = _CheckInt(mohDT.Rows[0]["MotherReceivedDrugforPMTCT"].ToString());
                if (mohDT.Rows[0]["MotherRegimen"].ToString().Length > 0)
                {
                    string[] MPDarr = mohDT.Rows[0]["MotherRegimen"].ToString().Split('|');
                    foreach (var item in MPDarr)
                    {
                        int theVal = new int();
                        if (int.TryParse(item, out theVal))
                            rg.MotherPMTCTdrugs.Add(theVal);
                    }
                }
                if (mohDT.Rows[0]["childReceivedDrugforPMTCT"].ToString() != "") rg.ChildPMTCTdrugsYN = _CheckInt(mohDT.Rows[0]["childReceivedDrugforPMTCT"].ToString());
                if (mohDT.Rows[0]["ChildRegimen"].ToString().Length > 0)
                {
                    string[] MPDarr = mohDT.Rows[0]["ChildRegimen"].ToString().Split('|');
                    foreach (var item in MPDarr)
                    {
                        int theVal = new int();
                        if (int.TryParse(item, out theVal))
                            rg.ChildPMTCTdrugs.Add(theVal);
                    }
                }
                if (mohDT.Rows[0]["MotherArtStatus"].ToString() != "") rg.MotherARTYN = _CheckInt(mohDT.Rows[0]["MotherArtStatus"].ToString());
                if (mohDT.Rows[0]["FeedingOption"].ToString() != "") rg.FeedingOption = _CheckInt(mohDT.Rows[0]["FeedingOption"].ToString());
            }

            if (regDS.Tables.Contains("Table3"))
            {
                if (regDS.Tables[3].Rows.Count > 0)
                {
                    rg.DateConfirmedHIVPositive = regDS.Tables[3].Rows[0]["ConfirmHIVPosDate"].ToString();
                    rg.DateEnrolledHIVCare = regDS.Tables[3].Rows[0]["ARTStartDate"].ToString();
                }
            }

            if (regDS.Tables.Contains("Table4"))
            {
                if (regDS.Tables[4].Rows.Count > 0)
                    rg.WHOStageAtEnrollment = _CheckInt(regDS.Tables[4].Rows[0]["WHOStage"].ToString());
            }

            if (traDT.Rows.Count > 0)
            {
                //#### Transfer In Tab ########//
                //ARTTransferInDate, FromDistrict, ARTTransferInFrom, Regimen1_DateLastUsed, PrevARVRegimen, PrevARVRegimenIDs
                rg.TransferInDate = traDT.Rows[0]["ARTTransferInDate"].ToString();
                rg.FromDistrict = _CheckInt(traDT.Rows[0]["FromDistrict"].ToString());
                rg.Facility = traDT.Rows[0]["ARTTransferInFrom"].ToString();
                rg.DateStart = traDT.Rows[0]["Regimen1_DateLastUsed"].ToString();
                if (traDT.Rows[0]["PrevARVRegimenIDs"].ToString().Length > 0)
                {
                    string[] traDarr = traDT.Rows[0]["PrevARVRegimenIDs"].ToString().Split('|');
                    foreach (var item in traDarr)
                    {
                        int theVal = new int();
                        if (int.TryParse(item, out theVal))
                            rg.Regimen.Add(theVal);
                    }
                }
                rg.RegimenAbbreviations = traDT.Rows[0]["PrevARVRegimen"].ToString();
                rg.WHOStageAtTransfer = _CheckInt(traDT.Rows[0]["PrevWHOStage"].ToString());

            }

            if (regDS.Tables.Contains("Table6"))
            {
                if (tr1DT.Rows.Count > 0)
                {
                    rg.Weight = tr1DT.Rows[0]["weight"].ToString();
                    rg.Height = tr1DT.Rows[0]["height"].ToString();
                }
            }

            if (regDS.Tables.Contains("Table7"))
            {
                if (tr2DT.Rows.Count > 0)
                {
                    rg.PriorART = tr2DT.Rows[0]["PriorART"].ToString();
                    if (rg.PriorART == "1")
                    {
                        foreach (DataRow item in tr2DT.Rows)
                        {
                            objRegistration.PriorARTRegimen Nrg = new objRegistration.PriorARTRegimen();
                            Nrg.Regimen = item["Regimen"].ToString();
                            Nrg.RegimenID = _CheckInt(item["RegimenID"].ToString());
                            Nrg.PriorARTDateLastUsed = item["DateLastUsed"].ToString();
                            rg.PriorARTRegimens.Add(Nrg);
                        }
                    }
                }
            }

            if (regDS.Tables.Contains("Table8"))
            {
                if (dagDT.Rows.Count > 0)
                {
                    foreach (DataRow item in dagDT.Rows)
                    {
                        objRegistration.DrugAllergy DAG = new objRegistration.DrugAllergy();
                        DAG.Allergen = item["Allergen"].ToString();
                        DAG.AllergenID = _CheckInt(item["AllergenID"].ToString());
                        DAG.DateOfAllergy = item["DateOfAllergy"].ToString();
                        DAG.MedicalConditions = item["RelevantMedicalCondition"].ToString();
                        DAG.TypeOfReaction = item["TypeOfReaction"].ToString();
                        rg.DrugAllergies.Add(DAG);
                    }
                }
            }
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
        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            updtFormUpdate.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "printScript", "PrintTouchForm('Registration Form', '" + this.ID + "');", true);
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
                    if (((RadButton)(c)).ID != "btnPrint")
                    {
                        ((RadButton)(c)).Enabled = State;
                    }
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

        protected void CleanVals()
        {
            Type type = rg.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    if (property.GetValue(rg, null) == null)
                    {
                        property.SetValue(rg, "", null);
                    }
                }
                else if ((property.PropertyType == typeof(int)) || (property.PropertyType == typeof(decimal)))
                {
                    if (property.GetValue(rg, null) == null)
                    {
                        property.SetValue(rg, -1, null);
                    }
                }
                else if (property.PropertyType == typeof(bool))
                {
                    if (property.GetValue(rg, null) == null)
                    {
                        property.SetValue(rg, false, null);
                    }
                }
            }
        }
        protected void GetAllDropDowns()
        {

            IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);
            IDrug DrugMngr = (IDrug)ObjectFactory.CreateInstance(ObjFactoryParameterDrug);
            BindFunctions theBind = new BindFunctions();

            string GetSex = "select ID, Name from mst_decode where codeid = 4 and deleteflag = 0";
            theBind.BindCombo(cbSex, ptnMgr.ReturnDatatableQuery(GetSex), "Name", "ID");

            string GetDistricts = "select Name, ID from mst_district WHERE DeleteFlag = 0 and systemId = 3 order by ID Asc";
            theBind.BindCombo(rcbDistrict, ptnMgr.ReturnDatatableQuery(GetDistricts), "Name", "ID");

            string GetSubDistricts = "select Name, ID from mst_ward WHERE DeleteFlag = 0 and systemid = 3 order by ID Asc";
            theBind.BindCombo(rcbSubDistrict, ptnMgr.ReturnDatatableQuery(GetSubDistricts), "Name", "ID");
            theBind.BindCombo(rcbSubDistrictTransferIn, ptnMgr.ReturnDatatableQuery(GetSubDistricts), "Name", "ID");

            string GetEntryPoint = "select ID, Name from mst_decode where codeid = 17 and systemid = 3 and deleteflag = 0";
            DataTable epDT = GetInvisiblateClone(ptnMgr.ReturnDatatableQuery(GetEntryPoint), "ShowIfEntryOther");
            BindCombo(cbEntryPoint, epDT, "Name", "ID");

            string GetFacilitiesIn = "Select Distinct FacilityName as Name, FacilityId as Id from mst_facility where deleteflag = 0 and systemid = 3 order by FacilityName Asc";
            theBind.BindCombo(rcbFacilityIn, ptnMgr.ReturnDatatableQuery(GetFacilitiesIn), "Name", "ID");

            theBind.BindCombo(cbCGSex, ptnMgr.ReturnDatatableQuery(GetSex), "Name", "ID");

            string GetCGrelationship = "select ID, Name from mst_decode where codeid = 8 and systemid = 3 and deleteflag = 0 order by Name Asc";
            DataTable cgDT = GetInvisiblateClone(ptnMgr.ReturnDatatableQuery(GetCGrelationship), "ShowIfOtherRelationship");
            BindCombo(cbCGRelationship, cgDT, "Name", "ID");

            string GetAVRDrugs = "select mg.GenericID, mg.GenericName from mst_generic mg left join lnk_Drugtypegeneric ld on mg.GenericId = ld.genericid where mg.DeleteFlag = 0 AND ld.drugtypeid = 37 and mg.genericid != 591 order by mg.GenericName";
            theBind.BindCombo(cbMotherPMTCTDrugs, ptnMgr.ReturnDatatableQuery(GetAVRDrugs), "GenericName", "GenericID");
            theBind.BindCombo(cbChildPMTCTDrugs, ptnMgr.ReturnDatatableQuery(GetAVRDrugs), "GenericName", "GenericID");

            string GetFeeding = "select ID, Name from mst_pmtctdecode where CodeID = 4 and systemid = 3 and deleteflag = 0";
            theBind.BindCombo(cbFeedingOption, ptnMgr.ReturnDatatableQuery(GetFeeding), "Name", "ID");

            string GetWHO = "select ID, Name from mst_decode where codeid = 22 and UpdateFlag = 0 order by Name";
            theBind.BindCombo(cbWHOEnrollment, ptnMgr.ReturnDatatableQuery(GetWHO), "Name", "ID");
            theBind.BindCombo(cbWHOStage, ptnMgr.ReturnDatatableQuery(GetWHO), "Name", "ID");

            //Note: 19-Aug-2014: Changed below query, reincorporating the line "and InlineDrug.DrugTypeId = 37" unsure as to why it was commented out

            string GetRegimenTransferIn = "Select InlineDrug.ID as ID, InlineDrug.Name as Name," +
                                          "InlineDrug.DrugTypeId from (select mg.GenericID as ID, mg.GenericName as Name, " +
                                          "ld.DrugTypeId as DrugTypeID FROM mst_Generic AS mg LEFT OUTER JOIN lnk_DrugTypeGeneric AS ld " +
                                          "ON mg.GenericID = ld.GenericId WHERE (mg.DeleteFlag = 0 or mg.DeleteFlag IS NULL)) " +
                                          "InlineDrug inner join Lnk_ItemDrugType itemdrg on InlineDrug.DrugTypeId=itemdrg.DrugTypeID " +
                                          "and itemdrg.ItemTypeID=300 and InlineDrug.DrugTypeId = 37 ORDER BY InlineDrug.Name "; 

            //string GetRegimenTransferIn = "select mg.GenericID as ID, mg.GenericName as Name " +
            //                              "FROM mst_Generic AS mg LEFT OUTER JOIN " +
            //                              "lnk_DrugTypeGeneric AS ld ON mg.GenericID = ld.GenericId " +
            //                              "WHERE (mg.DeleteFlag = 0 or mg.DeleteFlag IS NULL)" +
            //                              "ORDER BY mg.GenericName ";
            DataTable DTDrugs = ptnMgr.ReturnDatatableQuery(GetRegimenTransferIn);
            theBind.BindCombo(cbTransferInRegimen, DTDrugs, "Name", "ID");
            theBind.BindCombo(cbPriorARTRegimen, DTDrugs, "Name", "ID");
            theBind.BindCombo(cbAllergen, DTDrugs, "Name", "ID");

            //get Drug List
            //DataSet DSDrug = DrugMngr.GetPharmacyMasters(patientID);




        }
        protected void GetAllGrids()
        {
            if (AllergenArray != null)
            {
                rgAllergies.DataSource = AllergenArray;
                rgAllergies.DataBind();
            }
            if (PriorARTArray != null)
            {
                rgPriorART.DataSource = PriorARTArray;
                rgPriorART.DataBind();
            }
        }


        protected void BindCombo(RadComboBox rcb, DataTable dt, string textField, string valueField)
        {
            rcb.DataSource = dt;
            rcb.DataValueField = valueField;
            rcb.DataTextField = textField;
            rcb.DataBind();
        }

        protected void cbInsertSelectValue(ref DataTable dt)
        {

            DataRow theDR = dt.NewRow();
            theDR["Name"] = "Select";
            theDR["ID"] = "0";
            dt.Rows.InsertAt(theDR, 0);

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
                if (dtCloned.Rows[dtCloned.Rows.Count - 1]["Name"].ToString() == "Other")
                    dtCloned.Rows[dtCloned.Rows.Count - 1]["ID"] += "|" + TableToInvisiblate + "|show";
                else
                    dtCloned.Rows[dtCloned.Rows.Count - 1]["ID"] += "|" + TableToInvisiblate + "|hide";
            }

            return dtCloned;
        }



        protected void btnCalCDOB_Click(object sender, EventArgs e)
        {
            SetDOB(AgeMonthZZZ, AgeYearZZZ, dtPatientDOB);
        }

        protected void CGCalDOB_Click(object sender, EventArgs e)
        {
            SetDOB(txtCGMonthZZZ, txtCGYearZZZ, dtCGDOB);
        }

        protected void SetDOB(RadTextBox monthTxtBox, RadTextBox yearTxtBox, RadDatePicker theDatePicker)
        {
            if (yearTxtBox.Text.Trim() == "")
            {
                yearTxtBox.Text = "0";
            }
            if (monthTxtBox.Text != "")
            {
                if ((Convert.ToInt32(monthTxtBox.Text) < 0) || (Convert.ToInt32(monthTxtBox.Text) > 11))
                {
                    MsgBuilder theMsg = new MsgBuilder();
                    theMsg.DataElements["Control"] = "Age (Month)";
                    IQCareMsgBox.Show("AgeMonthRange", theMsg, this);
                    return;
                }
            }


            int age = 0;
            int months = 0;
            DateTime currentdate;
            age = Convert.ToInt32(yearTxtBox.Text);
            if (monthTxtBox.Text != "")
            {
                currentdate = DateTime.Now;
            }
            else
                currentdate = Convert.ToDateTime("06-15-" + DateTime.Now.Year);

            DateTime birthdate = currentdate.AddYears(age * -1);
            if (monthTxtBox.Text != "")
            {
                months = Convert.ToInt32(monthTxtBox.Text);
                birthdate = birthdate.AddMonths(months * -1);
            }

            theDatePicker.SelectedDate = birthdate;
        }

        protected void SetAge(DateTime SelectedDate, RadTextBox monthTxtBox, RadTextBox yearTxtBox)
        {
            Age theAge = new Age(SelectedDate, DateTime.Now);
            yearTxtBox.Text = theAge.Years.ToString();
            monthTxtBox.Text = theAge.Months.ToString();
        }

        protected void dtCGDOB_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            SetAge((DateTime)e.NewDate, txtCGMonthZZZ, txtCGYearZZZ);
        }

        protected void dtPatientDOB_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            if (e.NewDate != null)
                SetAge((DateTime)e.NewDate, AgeMonthZZZ, AgeYearZZZ);
        }

        protected void txtHeight_TextChanged(object sender, EventArgs e)
        {
            GetBMI();
            cbWHOStage.Focus();
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
                decimal BMI = wgt / ((hgt / 100) * (hgt / 100));
                var thePos = BMI.ToString().IndexOf(".");
                var theVal = string.Empty;
                if (thePos > 0)
                    theVal = BMI.ToString().Substring(0, thePos + 2);
                else
                    theVal = BMI.ToString();
                txtBMICalculated.Text = theVal;
            }
        }

        protected void btnAddAllergen_Click(object sender, EventArgs e)
        {
            if (dtReactionDate.IsEmpty == false)
            {
                Allergen ag = new Allergen(cbAllergen.SelectedValue, cbAllergen.SelectedItem.Text, txtTOR.Text, (DateTime)dtReactionDate.SelectedDate, txtRMC.Text);
                if (!Allergen.CheckAllergen(ag.AllergenID, ref AllergenArray)) AllergenArray.Add(ag);
                rgAllergies.DataSource = AllergenArray;
                rgAllergies.DataBind();
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "closeMinLoad", "CloseMinLoading('divAllergies')", true);
            }
        }

        protected void btnPriorArt_Click(object sender, EventArgs e)
        {
            PriorArtClass prc = new PriorArtClass(cbPriorARTRegimen.SelectedValue, cbPriorARTRegimen.SelectedItem.Text, (DateTime)dtPriorDate.SelectedDate);

            if (!PriorArtClass.CheckPriorART(prc.RegimenID, ref PriorARTArray)) PriorARTArray.Add(prc);
            rgPriorART.DataSource = PriorARTArray;
            rgPriorART.DataBind();
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "closeMinLoad", "CloseMinLoading('tab6')", true);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "closasdf", "$('#ShowIfPriorARTYes').show();focusAndLayout('PriorGrid');", true);
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRegistration();
        }

        static bool IsError = false;
        public void SaveRegistration()
        {
            try
            {
                SetSavedVals(ref rg);
                IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);
                int theRes = (int)ptnMgr.SaveRegistrationDetails(rg, patientID.ToString(), Session["AppLocationId"].ToString(), (Session["AppUserId"]).ToString());

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackWithoutDialog();", true);
                IsError = false;

                //Clean up static Arrays
                PriorARTArray.Clear();
                AllergenArray.Clear();

            }
            catch (Exception e)
            {
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

        private void SetSavedVals(ref objRegistration theRegObj)
        {
            //#### Patient Info Tab ########//
            theRegObj.FirstName = txtFirstName.Text;
            theRegObj.LastName = txtLastName.Text;
            theRegObj.Sex = int.Parse(cbSex.SelectedValue);
            theRegObj.DOB = dtPatientDOB.SelectedDate.ToString();
            theRegObj.RegistrationDate = dtRegistrationDate.SelectedDate.ToString();
            theRegObj.Address = txtAddress.Text;
            theRegObj.Suburb = txtSuburb.Text;
            theRegObj.SubDistrict = rcbSubDistrict.SelectedValue;
            theRegObj.District = rcbDistrict.SelectedValue;
            theRegObj.TelephoneNo = txtPatientPhoneNo.Text;
            theRegObj.EntryPoint = cbEntryPoint.SelectedValue;
            theRegObj.OtherEntryPoint = txtEntryPointOther.Text;
            theRegObj.SubDistrict = rcbSubDistrict.SelectedValue;
            theRegObj.Addresscomments = txtAddressComment.Text;
            theRegObj.PostalAddress = txtPatientPostalAddress.Text;
            theRegObj.PostalCode = txtPatientPostalCode.Text;

            //#### Caregiver Info Tab ########//
            theRegObj.CareGiverName = txtCGFirstName.Text + "|" + txtCGLastName.Text;
            theRegObj.CareGiverDOB = dtCGDOB.SelectedDate.ToString();
            theRegObj.CareGiverGender = int.Parse(cbCGSex.SelectedValue);
            theRegObj.CareGiverRelationship = int.Parse(cbCGRelationship.SelectedValue.Split('|')[0]);
            theRegObj.OtherCareGiver = txtOtherRelationship.Text;
            theRegObj.CareGiverTelephone = txtCGPhoneNo.Text;

            //#### Mother's History Tab ########//
            theRegObj.MotherName = txtMotherName.Text;
            if (rbtnMotherAliveYes.Checked)
                theRegObj.MotherAliveYN = true;
            else if (rbtnMotherAliveNo.Checked)
                theRegObj.MotherAliveYN = false;

            theRegObj.MotherPMTCTdrugsYN = int.Parse(cbMotherReceivedPMTCT.SelectedValue.Split('|')[0]);

            if (theRegObj.MotherPMTCTdrugs != null) theRegObj.MotherPMTCTdrugs.Clear();
            if (cbMotherPMTCTDrugs.CheckedItems.Count > 0)
            {
                foreach (var item in cbMotherPMTCTDrugs.CheckedItems)
                {
                    int theVal = new int();
                    if (int.TryParse(item.Value, out theVal))
                        theRegObj.MotherPMTCTdrugs.Add(theVal);
                }
            }

            theRegObj.ChildPMTCTdrugsYN = int.Parse(cbChildReceivedPMTCT.SelectedValue.Split('|')[0]);

            if (theRegObj.ChildPMTCTdrugs != null) theRegObj.ChildPMTCTdrugs.Clear();
            if (cbChildPMTCTDrugs.CheckedItems.Count > 0)
            {
                foreach (var item in cbChildPMTCTDrugs.CheckedItems)
                {
                    int theVal = new int();
                    if (int.TryParse(item.Value, out theVal))
                        theRegObj.ChildPMTCTdrugs.Add(theVal);
                }
            }

            theRegObj.MotherARTYN = int.Parse(cbMotherOnART.SelectedValue);
            theRegObj.FeedingOption = int.Parse(cbFeedingOption.SelectedValue);


            //#### HIV Care Tab ########//
            theRegObj.DateConfirmedHIVPositive = dtConfirmedPos.SelectedDate.ToString();
            theRegObj.DateEnrolledHIVCare = dtEnrolledHIVCare.SelectedDate.ToString();
            theRegObj.WHOStageAtEnrollment = _CheckInt(cbWHOEnrollment.SelectedValue);

            //#### Transfer In Tab ########//
            theRegObj.TransferInDate = dtTransferIn.SelectedDate.ToString();
            theRegObj.FromDistrict = _CheckInt(rcbSubDistrictTransferIn.SelectedValue);
            theRegObj.Facility = rcbFacilityIn.SelectedValue;
            theRegObj.DateStart = dtDateRegimenStarted.SelectedDate.ToString();

            //cycle through the regimen
            if (theRegObj.Regimen != null) theRegObj.Regimen.Clear();
            if (cbTransferInRegimen.CheckedItems.Count > 0)
            {
                string drugAbbs = string.Empty;
                foreach (var item in cbTransferInRegimen.CheckedItems)
                {
                    int theVal = new int();
                    if (int.TryParse(item.Value, out theVal))
                    {
                        theRegObj.Regimen.Add(theVal);

                        IDrugMst DrugManager = (IDrugMst)ObjectFactory.CreateInstance("BusinessProcess.Administration.BDrugMst, BusinessProcess.Administration");
                        DataSet theDS = (DataSet)DrugManager.GetDrugMst();

                        //DataRow theDR = theDT.NewRow();
                        DataView theDV = new DataView(theDS.Tables[0]);
                        theDV.RowFilter = "Drug_Pk = " + theVal;
                        if (theDV.Count > 0)
                        {
                            if (drugAbbs.Length > 0)
                                drugAbbs += "//";
                            drugAbbs += theDV[0]["GenericAbbrevation"].ToString();
                        }
                    }
                }
            }

            theRegObj.Weight = _CheckDecimal(txtWeight.Text).ToString();
            theRegObj.Height = _CheckDecimal(txtHeight.Text).ToString();
            theRegObj.BMI = _CheckDecimal(txtBMICalculated.Text).ToString();
            theRegObj.WHOStageAtTransfer = _CheckInt(cbWHOStage.SelectedValue);
            theRegObj.PriorART = cbPriorART.SelectedValue.Split('|')[0].ToString();
            if (theRegObj.PriorART == "1")
            {
                theRegObj.PriorARTRegimens.Clear();
                foreach (GridDataItem item in rgPriorART.Items)
                {
                    objRegistration.PriorARTRegimen PriorR = new objRegistration.PriorARTRegimen();
                    PriorR.RegimenID = _CheckInt(item["RegimenID"].Text);
                    PriorR.Regimen = item["Regimen"].Text;
                    PriorR.PriorARTDateLastUsed = item["DateLastUsed"].Text;
                    theRegObj.PriorARTRegimens.Add(PriorR);
                }
            }

            if (rgAllergies.Items.Count > 0)
            {
                theRegObj.DrugAllergies.Clear();
                foreach (GridDataItem item in rgAllergies.Items)
                {
                    objRegistration.DrugAllergy DA = new objRegistration.DrugAllergy();
                    DA.Allergen = item["AllergenName"].Text;
                    DA.DateOfAllergy = item["ADate"].Text;
                    DA.MedicalConditions = item["RMC"].Text;
                    DA.TypeOfReaction = item["TOR"].Text;
                    DA.AllergenID = _CheckInt(item["AllergenID"].Text);
                    theRegObj.DrugAllergies.Add(DA);
                }
            }
            else
            {
                theRegObj.DrugAllergies.Clear();
            }

        }

        protected void rgAllergies_OnItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem gi = (GridDataItem)e.Item;
            if (e.CommandName == "Delete")
            {
                string theKey = gi.GetDataKeyValue("AllergenID").ToString();
                Allergen DelAllergen = Allergen.FindAllerGen(theKey, ref AllergenArray);
                AllergenArray.Remove(DelAllergen);
                rgAllergies.DataSource = AllergenArray;
                rgAllergies.DataBind();
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "closeMinLoad", "CloseMinLoading('divAllergies')", true);
            }
        }

        protected void cbTransferInRegimen_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            RadComboBoxItem theItem = e.Item;
            foreach (var item in rg.Regimen)
            {
                if (theItem.Value == item.ToString())
                {
                    theItem.Checked = true;
                }
            }

        }

        protected void rgPriorART_ItemCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem gi = (GridDataItem)e.Item;
            if (e.CommandName == "Delete")
            {
                string theKey = gi.GetDataKeyValue("RegimenID").ToString();
                PriorArtClass DelPriorART = PriorArtClass.FindPriorART(theKey, ref PriorARTArray);
                PriorARTArray.Remove(DelPriorART);
                rgPriorART.DataSource = PriorARTArray;
                rgPriorART.DataBind();
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "closeMinLoad", "CloseMinLoading('divAllergies')", true);
            }
        }
    }

    public class PriorArtClass
    {
        public PriorArtClass(string RegimenID, string Regimen, DateTime DateLastUsed)
        {
            _regid = RegimenID;
            _reg = Regimen;
            _dlused = DateLastUsed;
        }
        public static PriorArtClass FindPriorART(string RegimenID, ref List<PriorArtClass> PriorARTArray)
        {
            foreach (var item in PriorARTArray)
            {
                if (item.RegimenID == RegimenID)
                {
                    return item;
                }
            }
            return null;
        }
        public static bool CheckPriorART(string RegimenID, ref List<PriorArtClass> PriorARTArray)
        {
            foreach (var item in PriorARTArray)
            {
                if (item.RegimenID == RegimenID)
                {
                    return true;
                }
            }
            return false;
        }
        private string _regid;
        public string RegimenID
        {
            get { return _regid; }
            set { _regid = value; }
        }
        private string _reg;
        public string Regimen
        {
            get { return _reg; }
            set { _reg = value; }
        }
        private DateTime _dlused;
        public DateTime DateLastUsed
        {
            get { return _dlused; }
            set { _dlused = value; }
        }
    }
    public class Allergen
    {
        public Allergen(string AllergenID, string AllergenName, string TOR, DateTime ADate, string RMC)
        {
            _allergenID = AllergenID;
            _allergen = AllergenName;
            _tor = TOR;
            _adate = ADate;
            _rmc = RMC;

        }
        public static Allergen FindAllerGen(string AllergenID, ref List<Allergen> AllergenArray)
        {
            foreach (var item in AllergenArray)
            {
                if (item.AllergenID == AllergenID)
                {
                    return item;
                }
            }
            return null;
        }
        public static bool CheckAllergen(string AllerGenID, ref List<Allergen> AllergenArray)
        {
            foreach (var item in AllergenArray)
            {
                if (item.AllergenID == AllerGenID)
                {
                    return true;
                }
            }
            return false;
        }
        private string _allergenID;
        public string AllergenID
        {
            get { return _allergenID; }
            set { _allergenID = value; }
        }
        private string _allergen;
        public string AllergenName
        {
            get { return _allergen; }
            set { _allergen = value; }
        }
        private string _tor;
        public string TOR
        {
            get { return _tor; }
            set { _tor = value; }
        }
        private DateTime _adate;
        public DateTime ADate
        {
            get { return _adate; }
            set { _adate = value; }
        }
        private string _rmc;
        public string RMC
        {
            get { return _rmc; }
            set { _rmc = value; }
        }
    }
}