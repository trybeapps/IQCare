using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Interface.Security;
using Application.Common;
using Application.Presentation;
using Interface.Pharmacy;
using Interface.Administration;
using Interface.Clinical;
using System.Text;

public partial class Pharmacy_frmPharmacy_CTC : BasePage
{
    public DataTable theDrugTable;
    public DataTable AddARV;
    public DataTable OtherDrugs;
    DateTime theCurrentDate;
    IIQCareSystem IQCareSecurity;
    int icount;
    StringBuilder sbParameter;
    StringBuilder sbValues;
    string strmultiselect;
    String TableName;
    ArrayList arl;

    DataSet theDS;
    DataSet theExistDS = new DataSet();
    DataView theDV = new DataView();
    Utility theUti = new Utility();
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            //if(Session["PatientStatus"]!=null)
            //    (Master.FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["PatientStatus"]);
            if (Request.QueryString["Prog"] == "ART")
            {
                (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["HIVPatientStatus"]);
            }
            else
            {
                (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["PMTCTPatientStatus"]);
            }
            //(Master.FindControl("lblRoot") as Label).Text = "Pharmacy Form >>";
            //(Master.FindControl("lblMark") as Label).Visible = false;
            //(Master.FindControl("lblheader") as Label).Text = "Pharmacy";
            //(Master.FindControl("lblformname") as Label).Text = "Pharmacy Form";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Text = "Pharmacy Form >> ";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "Pharmacy";
            (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblformname") as Label).Text = "Pharmacy Form";

            if ((Session["HIVPatientStatus"].ToString() == "1") && (Session["PMTCTPatientStatus"].ToString() == "1"))
            {
                btnsave.Enabled = false;
            }
            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                //theUti.SetSession();
            }
            #region "Authentication"
            Session["PtnRegCTC"] = "";
            DataTable theDTModule = (DataTable)Session["AppModule"];
            string ModuleId = "";
            foreach (DataRow theDR in theDTModule.Rows)
            {
                if (ModuleId == "")
                    ModuleId = theDR["ModuleId"].ToString();
                else
                    ModuleId = ModuleId + "," + theDR["ModuleId"].ToString();
            }
            AuthenticationManager Authentiaction = new AuthenticationManager();
            if (Authentiaction.HasFunctionRight(ApplicationAccess.Pharmacy, FunctionAccess.Print, (DataTable)Session["UserRight"]) == false)
            {
                btnPrint.Enabled = false;

            }

            if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
            {

                if (Authentiaction.HasFunctionRight(ApplicationAccess.Pharmacy, FunctionAccess.View, (DataTable)Session["UserRight"]) == false)
                {
                    int PatientID = Convert.ToInt32(Session["PatientId"].ToString());
                    string theUrl = "";
                    theUrl = string.Format("{0}?PatientId={1}", "../ClinicalForms/frmPatient_History.aspx", PatientID);
                    Response.Redirect(theUrl);
                }
                else if (Authentiaction.HasFunctionRight(ApplicationAccess.Pharmacy, FunctionAccess.Update, (DataTable)Session["UserRight"]) == false)
                {
                    btnsave.Enabled = false;
                }


            }
            else if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                if (Authentiaction.HasFunctionRight(ApplicationAccess.Pharmacy, FunctionAccess.Add, (DataTable)Session["UserRight"]) == false)
                {
                    btnsave.Enabled = false;
                }
                if (Request.QueryString["Prog"] == "PMTCT")
                {
                    ddlTreatment.SelectedValue = "223";
                    ddlTreatment.Enabled = false;

                }
            }
            else if (Request.QueryString["name"] == "Delete")
            {
                MsgBuilder theBuilder1 = new MsgBuilder();
                theBuilder1.DataElements["FormName"] = "Pharmacy CTC";
                IQCareMsgBox.ShowConfirm("DeleteForm", theBuilder1, btnsave);
                if (Authentiaction.HasFunctionRight(ApplicationAccess.Pharmacy, FunctionAccess.View, (DataTable)Session["UserRight"]) == false)
                {
                    int PatientID = Convert.ToInt32(Session["PatientId"]);
                    string theUrl = "";
                    theUrl = string.Format("{0}?PatientId={1}", "../ClinicalForms/frmClinical_DeleteForm.aspx", PatientID);
                    Response.Redirect(theUrl);
                }
                else if (Authentiaction.HasFunctionRight(ApplicationAccess.Pharmacy, FunctionAccess.Delete, (DataTable)Session["UserRight"]) == false)
                {
                    btnsave.Text = "Delete";
                    btnsave.Enabled = false;
                    // btnQualityCheck.Visible = false;
                }
            }
            #endregion

            //CreateCustomControls();
            PutCustomControl();
            if (IsPostBack != true)
            {
                txtpharmOrderedbyDate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
                txtpharmOrderedbyDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");

                txtpharmReportedbyDate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
                txtpharmReportedbyDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
                txtARVCombRegDuraton.Attributes.Add("onkeyup", "chkInteger('" + txtARVCombRegDuraton.ClientID + "')");
                txtARVCombRegQtyPres.Attributes.Add("onkeyup", "chkInteger('" + txtARVCombRegQtyPres.ClientID + "')");
                txtARVCombRegQtyDesc.Attributes.Add("onkeyup", "chkInteger('" + txtARVCombRegQtyDesc.ClientID + "')");

                
                if (Request.QueryString["name"] == "Delete")
                {
                    btnsave.Text = "Delete";
                }

                Session["AddARV"] = null;
                Session["OtherDrugs"] = null;
                Session["MasterDrugTable"] = null;
                Session["PharmacyId"] = null;

                Init_Form();
                int thePtnID = 0;
                if ((Convert.ToInt32(Session["PatientVisitId"]) != 0) || (Request.QueryString["name"] == "Delete"))
                {
                    Int32 PID = Convert.ToInt32(Session["PatientId"].ToString());
                    FillOldCustomData(PID);
                }


                if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
                    Session["OrigOrdDate"] = theExistDS.Tables[0].Rows[0]["OrderedByDate"].ToString();
                else
                    Session["OrigOrdDate"] = null;

                thePtnID = Convert.ToInt32(Session["PatientId"]);
                Session["PtnID"] = thePtnID;
                Session["UserID"] = Session["AppUserId"].ToString();
                Session["LocationId"] = Convert.ToInt32(Session["AppLocationId"]);
                Session["SelectedDrug"] = theDrugTable;
                Session["MasterData"] = theDS;

                if (theDS.Tables[8].Rows.Count > 0)
                {
                    if (theDS.Tables[8].Rows[0][1] != null)
                        Session["EnrolmentDate"] = theDS.Tables[8].Rows[0][1];
                }
                if (Request.QueryString["Prog"] == "ART")
                {
                    Session["Status"] = Session["HIVPatientStatus"];
                }
                else
                {
                    Session["Status"] =Session["PMTCTPatientStatus"];
                }
                //Session["Status"] = Session["PatientStatus"].ToString();
                Session["Age"] = Convert.ToDecimal(theDS.Tables[3].Rows[0]["Age"].ToString()) + Convert.ToDecimal(theDS.Tables[3].Rows[0]["Age1"].ToString()) / 12;

                #region "DrugDataTable"
                DataTable theDT = new DataTable();

                theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
                theDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
                theDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
                theDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
                theDT.Columns.Add("Abbr", System.Type.GetType("System.String"));

                foreach (DataRow dr in theDS.Tables[0].Rows) // drug
                {
                    DataRow theDR = theDT.NewRow();
                    theDR[0] = dr["Drug_Pk"];
                    theDR[1] = dr["DrugName"];
                    theDR[2] = 0;
                    theDR[3] = dr["DrugTypeId"];
                    theDR[4] = dr["GenericAbbrevation"];
                    theDT.Rows.Add(theDR);
                    System.Diagnostics.Debug.WriteLine(dr["Drug_Pk"].ToString() + "-Drug-" + dr["DrugName"].ToString() + "--" + theDR[2]);
                }
                foreach (DataRow dr in theDS.Tables[4].Rows) // generics
                {
                    DataRow theDR = theDT.NewRow();
                    theDR[0] = dr["GenericId"];
                    theDR[1] = dr["GenericName"];
                    theDR[2] = 1;
                    theDR[3] = dr["DrugTypeId"];
                    theDT.Rows.Add(theDR);
                    System.Diagnostics.Debug.WriteLine(dr["GenericId"].ToString() + "-Generic-" + dr["GenericName"].ToString() + "--" + theDR[2]);
                }

                foreach (DataRow dr in theDrugTable.Rows)
                {
                    if (Convert.ToInt32(dr["GenericId"]) > 0)
                    {
                        DataRow[] theDR = theDT.Select("DrugId = " + dr["GenericId"].ToString() + " and Generic = 1");
                        theDT.Rows.Remove(theDR[0]);
                    }
                    else if (Convert.ToInt32(dr["DrugId"]) < 10000)
                    {
                        DataRow[] theDR = theDT.Select("DrugId = " + dr["DrugId"].ToString() + " and Generic = 0");
                        theDT.Rows.Remove(theDR[0]);
                    }
                }
                DataTable dtDuplicate = theDT.Copy();
                SortDataTable(dtDuplicate, "DrugName asc");

                String drgNameDup = string.Empty;
                foreach (DataRow drduplicate in dtDuplicate.Rows)
                {
                    if (drgNameDup == Convert.ToString(drduplicate["DrugName"]))
                    {
                        DataRow[] theDDR = theDT.Select("DrugName='" + drduplicate["DrugName"].ToString() + "' and Generic=0");
                        if (theDDR.Length > 0)
                            theDT.Rows.Remove(theDDR[0]);
                    }
                    drgNameDup = Convert.ToString(drduplicate["DrugName"]);
                }

                Session["MasterDrugTable"] = theDT;
                #endregion

                if (Convert.ToInt32(Session["PatientVisitId"]) > 0 && Convert.ToInt32(Session["PatientVisitId"]) != 0)
                {
                    Session["AddARV"] = AddARV;
                    Session["OtherDrugs"] = OtherDrugs;
                    Session["PharmacyId"] = Session["PatientVisitId"].ToString();

                    //MsgBuilder theBuilder = new MsgBuilder();
                    //theBuilder.DataElements["FormName"] = "Drug Order";
                    //IQCareMsgBox.ShowConfirm("UpdateClinicalRecord", theBuilder, btnOk);

                }
                else if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                {
                    //MsgBuilder theBuilder = new MsgBuilder();
                    //theBuilder.DataElements["FormName"] = "Drug Order";
                    //IQCareMsgBox.ShowConfirm("AddClinicalRecord", theBuilder, btnOk);
                }

            }
            else
            {
                if (btnCounsellorSignature.Checked == true)
                {
                    string script = "<script language = 'javascript' defer ='defer' id = 'showSignatureCombo'>\n";
                    script += "show('ddSignature');\n";
                    script += "</script>\n";
                    RegisterStartupScript("showSignatureCombo", script);
                }

                #region "Additional ARV"
                if ((DataTable)Application["AddARV"] != null)
                {
                    Session["AddARV"] = (DataTable)Application["AddARV"];
                    Session["MasterDrugTable"] = (DataTable)Application["MasterData"];
                    Application.Remove("MasterData");
                    Application.Remove("AddARV");
                }
                if ((DataTable)Session["AddARV"] != null)
                {
                    DataTable theDT = (DataTable)Session["AddARV"];
                    //divAddARV.Visible = false;
                    LoadAdditionalDrugs(theDT, PnlAddARV);
                }
                #endregion

                #region "Additional OI and Other Medications"
                if ((DataTable)Application["OtherDrugs"] != null)
                {
                    Session["OtherDrugs"] = (DataTable)Application["OtherDrugs"];
                    Session["MasterDrugTable"] = (DataTable)Application["MasterData"];
                    Application.Remove("OtherDrugs");
                    Application.Remove("MasterData");
                }
                if ((DataTable)Session["OtherDrugs"] != null)
                {
                    DataTable theDT = (DataTable)Session["OtherDrugs"];
                    //divAddOI.Visible = false;
                    LoadAdditionalDrugs(theDT, PnlOtherMedication);
                }
                #endregion

                CreateControls((DataSet)Session["MasterData"]);
            }
            if (ddlTreatment.SelectedItem.Text == "PMTCT" || ddlTreatment.SelectedItem.Text == "PEP" || ddlTreatment.SelectedItem.Text == "Palliative Care")
            {
                PnlDrug.Enabled = true;
                BtnAddARV.Enabled = true;
            }
            if (Session["lblpntstatus"].ToString() == "1")
            {
                btnsave.Enabled = false;
            }

            if (ddlTreatment.SelectedItem.Value == "222")
            {
                chkProphylaxis.Enabled = false;
            }
            else
            {
                chkProphylaxis.Enabled = true;
            }
            
            
            if (Session["HIVPatientStatus"].ToString() == "1" && Session["PMTCTPatientStatus"].ToString() == "1")
            {
                btnsave.Enabled = false;
            }
            if (Session["CareEndFlag"].ToString() == "1")
            {
                btnsave.Enabled = true;
            }

            Form.EnableViewState = true;
        }
        catch (Exception err)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = err.Message.ToString();
            IQCareMsgBox.Show("#C1", theBuilder, this);
        }
    }
    private void Init_Form()
    {

        Session["PatientId"] = Session["PatientId"].ToString();
        IQCareUtils theUtils = new IQCareUtils();
        IFacilitySetup FacilityMaster = (IFacilitySetup)ObjectFactory.CreateInstance("BusinessProcess.Administration.BFacility, BusinessProcess.Administration");
        //check patient record form status

        //

        DataSet objDs = new DataSet();
        DataSet objPatientStatus = new DataSet();
        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        
        DataSet theDrugDS = DrugManager.GetPharmacyMasters(Convert.ToInt32(Session["PatientId"]));
        #region "FixDoseCombination"
        if (theDrugDS.Tables[24].Rows.Count > 0)
        {
            Session["ARVStatus"] = Convert.ToString(theDrugDS.Tables[24].Rows[0][0].ToString());
        }
        Session["FixDrugStrength"] = theDrugDS.Tables[17];
        Session["FixDrugFreq"] = theDrugDS.Tables[18];

        theDS = new DataSet();
        theDS.Tables.Add(theDrugDS.Tables[16].Copy());
        theDS.Tables.Add(theDrugDS.Tables[1].Copy());
        theDS.Tables.Add(theDrugDS.Tables[2].Copy());

        theDS.Tables.Add(theDrugDS.Tables[3].Copy());
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            theDS.Tables.Add(theDrugDS.Tables[4].Copy());
        else
            theDS.Tables.Add(theDrugDS.Tables[19].Copy());


        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            theDS.Tables.Add(theDrugDS.Tables[5].Copy());
        else
            theDS.Tables.Add(theDrugDS.Tables[14].Copy());

        theDS.Tables.Add(theDrugDS.Tables[6].Copy());//--7--
        theDS.Tables.Add(theDrugDS.Tables[7].Copy());//--8--
        theDS.Tables.Add(theDrugDS.Tables[8].Copy());//--9--
        theDS.Tables.Add(theDrugDS.Tables[9].Copy());//--10--
        theDS.Tables.Add(theDrugDS.Tables[10].Copy());

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            theDS.Tables.Add(theDrugDS.Tables[11].Copy());
        else
            theDS.Tables.Add(theDrugDS.Tables[12].Copy());

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            theDS.Tables.Add(theDrugDS.Tables[13].Copy());
        else
            theDS.Tables.Add(theDrugDS.Tables[15].Copy());

        theDS.Tables.Add(theDrugDS.Tables[20].Copy());
        theDS.Tables.Add(theDrugDS.Tables[21].Copy());
        theDS.Tables.Add(theDrugDS.Tables[22].Copy());
        theDS.Tables.Add(theDrugDS.Tables[23].Copy());


        #endregion

        if (theDS.Tables[3].Rows.Count == 0)
            Response.Redirect("../frmFindAddPatient.aspx");

        //lblPatientName.Text = theDS.Tables[3].Rows[0]["Name"].ToString();
        //lblpatientenrol.Text = theDS.Tables[3].Rows[0]["PatientId"].ToString();
        objDs = FacilityMaster.GetSystemBasedLabels(Convert.ToInt32(Session["SystemId"]), ApplicationAccess.Pharmacy, 0);
        //lblNumber.CssClass = "patientInfo";
        //lblNumber.Text = objDs.Tables[0].Rows[0][0].ToString() + " : ";
        //lblExisPatientID.Text = theDS.Tables[3].Rows[0]["PatientClinicID"].ToString();
        BindddlControls(theDS);
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            BindControls();
        }

        MakeRegimenGenericTable(theDS);
        theDrugTable = MakeTable();
        objPatientStatus = DrugManager.GetPatientRecordformStatus(Convert.ToInt32(Session["PatientId"]));
        if (objPatientStatus != null)
        {
            if (Request.QueryString["Prog"] == "ART")
            {
                if (objPatientStatus.Tables[0].Rows.Count > 0)
                {
                    btnsave.Enabled = true;
                }
                else
                {
                    btnsave.Enabled = false;
                    Page.Controls.Add(new LiteralControl("<script language='javascript'> window.alert('Please fill patient record form first')</script>"));
                }
            }

            if (objPatientStatus.Tables[1].Rows.Count > 0)
            {
                string ARVStatus = objPatientStatus.Tables[1].Rows[0]["ARVStatus"].ToString();
                if (objPatientStatus.Tables[1].Rows.Count == 1)
                {

                    if (ARVStatus == "1" || ARVStatus == "" || ARVStatus == "5")
                    {
                        PnlDrug.Enabled = false;
                        BtnAddARV.Enabled = false;
                    }
                }
                else
                {
                    if (ARVStatus == "1" || ARVStatus == "5")
                    {
                        PnlDrug.Enabled = false;
                        BtnAddARV.Enabled = false;
                    }
                }

            }
            if (objPatientStatus.Tables[2].Rows.Count > 0)
            {
                string ARTEnded = objPatientStatus.Tables[2].Rows[0]["ARTEnded"].ToString();

                if (ARTEnded == "1")
                {
                    PnlDrug.Enabled = false;
                    BtnAddARV.Enabled = false;
                }
            }
            
            if (objPatientStatus.Tables[3].Rows.Count > 0)
            {
                string ARTStatus = string.Empty;
                if (Request.QueryString["Prog"] == "PMTCT")
                {
                    ddlTreatment.SelectedValue = "223";
                }
                else
                {
                    if (ddlTreatment.SelectedValue != "224")
                    {
                        ddlTreatment.SelectedValue = "222";
                    }
                }
                if (ddlTreatment.SelectedItem.Value.ToString() == "222")
                {
                    ARTStatus = objPatientStatus.Tables[3].Rows[0]["status"].ToString();

                    if (ARTStatus == "ART" || ARTStatus == "UnKnown" || ARTStatus == "Non-ART" || ARTStatus == "PMTCT" || ARTStatus=="Due for Termination")
                    {
                        btnsave.Enabled = true;
                    }
                    else
                    {
                        btnsave.Enabled = false;
                    }
                }
                if (ddlTreatment.SelectedItem.Value.ToString() == "223")
                {
                    ARTStatus = objPatientStatus.Tables[7].Rows[0]["status"].ToString();

                    if (ARTStatus == "ART" || ARTStatus == "UnKnown" || ARTStatus == "Non-ART" || ARTStatus == "PMTCT" || ARTStatus == "Due for Termination" || ARTStatus=="")
                    {
                        btnsave.Enabled = true;
                    }
                    else
                    {
                        btnsave.Enabled = false;
                    }
                }
            }

        }
        else
        {
            btnsave.Enabled = false;
            IQCareMsgBox.Show("fillPatinetRecord", this);
            //Page.Controls.Add(new LiteralControl("<script language='javascript'> window.alert('Please fill patient record form first')</script>"));
        }

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {

            CreateControls(theDS);
        }
        else
        {

            theExistDS = DrugManager.GetExistPharmacy_CTC_Detail(Convert.ToInt32(Session["PatientVisitId"]));
            if (theExistDS.Tables.Count == 0)
            {
                IQCareMsgBox.Show("NoPharmacyRecordExists", this);
                return;
            }


            CreateControls(theDS);

            BindDropdownOrderBy(theExistDS.Tables[0].Rows[0]["OrderedBy"].ToString());
            BindDropdownReportedBy(theExistDS.Tables[0].Rows[0]["DispensedBy"].ToString());
            BindDropdownSignature(theExistDS.Tables[0].Rows[0]["Signature"].ToString());

            ddlPharmOrderedbyName.SelectedValue = theExistDS.Tables[0].Rows[0]["OrderedBy"].ToString();
            ddlPharmReportedbyName.SelectedValue = theExistDS.Tables[0].Rows[0]["DispensedBy"].ToString();
            if (theExistDS.Tables[0].Rows[0]["ProgID"].ToString() == "223")
            {
                ddlTreatment.Enabled = false;
            }
            ddlTreatment.SelectedValue = theExistDS.Tables[0].Rows[0]["ProgID"].ToString();
            ddlProvider.SelectedValue = Convert.ToString(theExistDS.Tables[0].Rows[0]["ProviderID"].ToString());
            //For ARV Regimen
            if (theExistDS.Tables[0].Rows[0]["ProgID"].ToString() == "223" || theExistDS.Tables[0].Rows[0]["ProgID"].ToString() == "224")
            {
                PnlDrug.Enabled = true;
                BtnAddARV.Enabled = true;
            }
            foreach (DataRow theDR in theExistDS.Tables[0].Rows)
            {
                if (theDR["RegimenID"].ToString() != "0" && theDR["RegimenID"].ToString() != "")
                {
                    ddlARVCombReg.SelectedValue = Convert.ToString(theDR["RegimenID"].ToString());
                    ddlARVCombRegFrqARV.SelectedValue = Convert.ToString(theDR["FrequencyID"].ToString());
                    txtARVCombRegDuraton.Text = Convert.ToString(theDR["Duration"].ToString());
                    if (theDR["OrderedQuantity"].ToString() != "0")
                    {
                        txtARVCombRegQtyPres.Text = Convert.ToString(theDR["OrderedQuantity"].ToString());
                    }
                    if (theDR["DispensedQuantity"].ToString() != "0")
                    {
                        txtARVCombRegQtyDesc.Text = Convert.ToString(theDR["DispensedQuantity"].ToString());
                    }
                    if (theDR["Prophylaxis"].ToString() != "")
                    {
                        chkProphylaxis.Checked = true;
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "csschange", "fnChangeClass();", true);
                }
            }
            //
            if (theExistDS.Tables[0].Rows[0]["Signature"].ToString() == "0")
            {
                btnPatientSignature.Checked = true;
            }
            else
            {
                string script = "<script language = 'javascript' defer ='defer' id = 'showsignature'>\n";
                script += "document.getElementById('" + btnCounsellorSignature.ClientID + "').click();\n";
                script += "</script>\n";
                RegisterStartupScript("showsignature", script);
                ddlPharmSignature.SelectedValue = theExistDS.Tables[0].Rows[0]["Signature"].ToString();
            }

            DateTime theOrderedByDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["OrderedByDate"].ToString());
            txtpharmOrderedbyDate.Value = theOrderedByDate.ToString(Session["AppDateFormat"].ToString());

            DateTime theReportedbyDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["DispensedByDate"]);
            txtpharmReportedbyDate.Value = theReportedbyDate.ToString(Session["AppDateFormat"].ToString());
            //txtpharmReportedbyDate.Attributes.Add("readonly", "true");

            DataSet objDsArtEnded = new DataSet();
            int PID = Convert.ToInt32(Session["PatientId"]);
            objDsArtEnded = DrugManager.GetARVStatus(PID, theReportedbyDate);
            if (objDsArtEnded.Tables[0].Rows.Count > 0)
            {
                string ARVStatus = objDsArtEnded.Tables[0].Rows[0]["ARVStatus"].ToString();

                if (ARVStatus == "2")
                {
                    PnlDrug.Enabled = true;
                    BtnAddARV.Enabled = true;
                }
                if (ARVStatus == "")
                {
                    PnlDrug.Enabled = false;
                    BtnAddARV.Enabled = false;
                }
                if (ARVStatus == "1")
                {
                    PnlDrug.Enabled = false;
                    BtnAddARV.Enabled = false;
                }
            }
            if (objDsArtEnded.Tables[2].Rows.Count > 0)
            {
                string ARTEnded = objDsArtEnded.Tables[2].Rows[0]["ARTEnded"].ToString();

                if (ARTEnded == "1")
                {
                    PnlDrug.Enabled = false;
                    BtnAddARV.Enabled = false;
                }
            }

            if (theExistDS.Tables[0].Rows[0]["HoldMedicine"].ToString() == "0")
            {
                chkpharmDispensePU.Checked = false;
            }
            else
            {
                chkpharmDispensePU.Checked = true;
            }
            Session["MasterData"] = theDS;

            #region Fill Existing Data

            #region "CreateAdditional Controls"

            #region "TableCreation"
            DataTable theDT = new DataTable();
            theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
            theDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
            theDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
            theDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));

            AddARV = theDT.Copy();
            OtherDrugs = theDT.Copy();
            #endregion

            foreach (DataRow theDR in theExistDS.Tables[0].Rows)
            {
                if (theDR["RegimenID"].ToString() == "0")
                {
                    if (Convert.ToInt32(theDR["Drug_Pk"]) == 0)
                    {
                        DataView theDV = new DataView(theDS.Tables[4]);
                        theDV.RowFilter = "GenericId = " + theDR["GenericId"].ToString() + " and GenericId not in (175,307,274,280,102,182)";
                        if (theDV.Count > 0)
                        {
                            if (Convert.ToInt32(theDV[0]["DrugTypeId"]) == 37)
                            {
                                DataRow DR = AddARV.NewRow();
                                DR[0] = theDR["GenericId"];
                                DR[1] = theDV[0]["GenericName"];
                                DR[2] = 1;
                                AddARV.Rows.Add(DR);
                            }
                            else
                            {
                                DataRow DR = OtherDrugs.NewRow();
                                DR[0] = theDR["GenericId"];
                                DR[1] = theDV[0]["GenericName"];
                                DR[2] = 1;
                                DR[3] = theDV[0]["DrugTypeId"];
                                OtherDrugs.Rows.Add(DR);
                            }
                        }
                    }
                    else
                    {

                        DataView theDV = new DataView(theDS.Tables[10]);
                        theDV.RowFilter = "Drug_Pk = " + theDR["Drug_Pk"].ToString() + " and Drug_Pk not in (85,150,486)";
                        if (theDV.Count > 0)
                        {
                            if (Convert.ToInt32(theDV[0]["DrugTypeId"]) == 37)
                            {
                                DataRow DR = AddARV.NewRow();
                                DR[0] = theDR["Drug_Pk"];
                                DR[1] = theDV[0]["DrugName"];
                                DR[2] = 0;
                                AddARV.Rows.Add(DR);
                            }
                            else
                            {
                                DataRow DR = OtherDrugs.NewRow();
                                DR[0] = theDR["Drug_Pk"];
                                DR[1] = theDV[0]["DrugName"];
                                DR[2] = 0;
                                DR[3] = theDV[0]["DrugTypeId"];
                                OtherDrugs.Rows.Add(DR);
                            }
                        }
                    }
                }
            }
            LoadAdditionalDrugs(AddARV, PnlAddARV);
            LoadAdditionalDrugs(OtherDrugs, PnlOtherMedication);
            #endregion


            foreach (DataRow dr in theExistDS.Tables[0].Rows)
            {
                //FillOldData(PnlDrug, dr);
                if (dr["RegimenID"].ToString() == "0")
                {
                    if (dr["Dose"].ToString() == "0.00")
                    {
                        FillOldData(PnlOIARV, dr);
                        FillOldData(PnlAddARV, dr);
                    }
                    if (dr["Dose"].ToString() != "0.00")
                    {
                        FillOldData(PnlOtherMedication, dr);
                    }



                }
            }

            #endregion

        }



    }
    private void BindDropdownOrderBy(String EmployeeId)
    {
        DataSet theDS = new DataSet();
        theDS.ReadXml(MapPath("..\\XMLFiles\\ALLMasters.con"));
        BindFunctions BindManager = new BindFunctions();
        IQCareUtils theUtils = new IQCareUtils();
        if (theDS.Tables["Mst_Employee"] != null)
        {
            DataView theDV = new DataView(theDS.Tables["Mst_Employee"]);
            theDV.RowFilter = "DeleteFlag = 0";
            if (theDV.Table != null)
            {
                DataTable theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0)
                {
                    theDV = new DataView(theDT);
                    theDV.RowFilter = "EmployeeId IN(" + Session["AppUserEmployeeId"].ToString() + "," + EmployeeId + ")";
                    if (theDV.Count > 0)
                        theDT = theUtils.CreateTableFromDataView(theDV);
                }
                BindManager.BindCombo(ddlPharmOrderedbyName, theDT, "EmployeeName", "EmployeeId");

            }
        }

    }
    private void BindDropdownReportedBy(String EmployeeId)
    {
        DataSet theDS = new DataSet();
        theDS.ReadXml(MapPath("..\\XMLFiles\\ALLMasters.con"));
        BindFunctions BindManager = new BindFunctions();
        IQCareUtils theUtils = new IQCareUtils();
        if (theDS.Tables["Mst_Employee"] != null)
        {
            DataView theDV = new DataView(theDS.Tables["Mst_Employee"]);
            theDV.RowFilter = "DeleteFlag = 0";
            if (theDV.Table != null)
            {
                DataTable theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0)
                {
                    theDV = new DataView(theDT);
                    theDV.RowFilter = "EmployeeId IN(" + Session["AppUserEmployeeId"].ToString() + "," + EmployeeId + ")";
                    if (theDV.Count > 0)
                        theDT = theUtils.CreateTableFromDataView(theDV);
                }

                BindManager.BindCombo(ddlPharmReportedbyName, theDT, "EmployeeName", "EmployeeId");

            }
        }

    }
    private void BindDropdownSignature(String EmployeeId)
    {
        DataSet theDS = new DataSet();
        theDS.ReadXml(MapPath("..\\XMLFiles\\ALLMasters.con"));
        BindFunctions BindManager = new BindFunctions();
        IQCareUtils theUtils = new IQCareUtils();
        if (theDS.Tables["Mst_Employee"] != null)
        {
            DataView theDV = new DataView(theDS.Tables["Mst_Employee"]);
            theDV.RowFilter = "DeleteFlag = 0";
            if (theDV.Table != null)
            {
                DataTable theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0)
                {
                    theDV = new DataView(theDT);
                    theDV.RowFilter = "EmployeeId IN(" + Session["AppUserEmployeeId"].ToString() + "," + EmployeeId + ")";
                    if (theDV.Count > 0)
                        theDT = theUtils.CreateTableFromDataView(theDV);
                }
                BindManager.BindCombo(ddlPharmSignature, theDT, "EmployeeName", "EmployeeId");
            }
        }

    }
    private void PutCustomControl()
    {
        ICustomFields CustomFields;
        CustomFieldClinical theCustomField = new CustomFieldClinical();
        try
        {

            CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields,BusinessProcess.Administration");
            DataSet theDS = CustomFields.GetCustomFieldListforAForm(Convert.ToInt32(ApplicationAccess.Pharmacy));
            if (theDS.Tables[0].Rows.Count != 0)
            {
                theCustomField.CreateCustomControlsForms(pnlCustomList, theDS, "CTCPharm");
            }
            ViewState["CustomFieldsDS"] = theDS;
            pnlCustomList.Visible = true;
        }
        catch (Exception err)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = err.Message.ToString();
            IQCareMsgBox.Show("#C1", theBuilder, this);
        }
        finally
        {
            CustomFields = null;
        }

    }
    private void InsertCustomFieldsValues()
    {
        try
        {

            GenerateCustomFieldsValues(pnlCustomList);
            string sqlstr = string.Empty;
            string sqlselect;
            Int32 PharmacyId = 0;
            DateTime OrderedbyDate = System.DateTime.Now;
            Int32 PatientId = Convert.ToInt32(Session["PatientId"]);
            ICustomFields CustomFields;

            if (Session["PharmacyId"] != null)
                PharmacyId = Convert.ToInt32(Session["PharmacyId"]);
            if (txtpharmOrderedbyDate.Value.ToString() != "")
                OrderedbyDate = Convert.ToDateTime(txtpharmOrderedbyDate.Value.ToString());

            if (sbValues.ToString().Trim() != "")
            {
                sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate " + sbParameter.ToString() + " )";
                //sqlstr += " VALUES(" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
                sqlstr += " VALUES(" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";

                CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
                icount = CustomFields.SaveCustomFieldValues(sqlstr.ToString());
                if (icount == -1)
                {
                    return;
                }
            }
            if (strmultiselect.ToString() != "")
            {
                string[] FieldValues = strmultiselect.Split(new char[] { '^' });
                if (arl.Count != 0)
                {
                    int p = 0;
                    foreach (object obj in arl)
                    {
                        sqlselect = "";
                        if (obj.ToString() != "")
                        {
                            if (FieldValues[p].ToString() != "")
                            {
                                string[] mValues = FieldValues[p].Split(new char[] { ',' });
                                foreach (string str in mValues)
                                {
                                    if (str.ToString() != "")
                                    {
                                        string strtab = "dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_";
                                        Int32 ispos = Convert.ToInt32(strtab.Length);
                                        Int32 iepos = Convert.ToInt32(obj.ToString().Length) - ispos;
                                        sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate, [" + obj.ToString().Substring(ispos, iepos) + "])";
                                        //sqlselect += " VALUES (" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";
                                        sqlselect += " VALUES (" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";
                                        CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
                                        icount = CustomFields.SaveCustomFieldValues(sqlselect.ToString());
                                        if (icount == -1)
                                        {
                                            return;
                                        }

                                    }
                                }
                            }
                        }
                        p += 1;
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

    private string InsertCustomFieldsValuesString()
    {
        string sqlstr = string.Empty;
        try
        {

            GenerateCustomFieldsValues(pnlCustomList);

            string sqlselect;
            Int32 PharmacyId = 0;
            DateTime OrderedbyDate = System.DateTime.Now;
            Int32 PatientId = Convert.ToInt32(Session["PatientId"]);
            ICustomFields CustomFields;

            PharmacyId = 99999;

            if (txtpharmOrderedbyDate.Value.ToString() != "")
                OrderedbyDate = Convert.ToDateTime(txtpharmOrderedbyDate.Value.ToString());

            if (sbValues.ToString().Trim() != "")
            {
                sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate " + sbParameter.ToString() + " )";
                sqlstr += " VALUES(" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";



            }
            if (strmultiselect.ToString() != "")
            {
                string[] FieldValues = strmultiselect.Split(new char[] { '^' });
                if (arl.Count != 0)
                {
                    int p = 0;
                    foreach (object obj in arl)
                    {
                        sqlselect = "";
                        if (obj.ToString() != "")
                        {
                            if (FieldValues[p].ToString() != "")
                            {
                                string[] mValues = FieldValues[p].Split(new char[] { ',' });
                                foreach (string str in mValues)
                                {
                                    if (str.ToString() != "")
                                    {
                                        string strtab = "dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_";
                                        Int32 ispos = Convert.ToInt32(strtab.Length);
                                        Int32 iepos = Convert.ToInt32(obj.ToString().Length) - ispos;
                                        sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate, [" + obj.ToString().Substring(ispos, iepos) + "])";

                                        sqlselect += " VALUES (" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";

                                        if (sqlstr == "")
                                        {
                                            sqlstr = sqlselect;
                                        }
                                        else
                                        {
                                            sqlstr = sqlstr + "!" + sqlselect;
                                        }


                                    }
                                }
                            }
                        }
                        p += 1;
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
        return sqlstr;

    }
    private string UpdateCustomFieldsValuesString()
    {
        string sqlstr = string.Empty;
        try
        {
            GenerateUpdateCustomFieldsValues(pnlCustomList);

            Int32 PatientId = Convert.ToInt32(Session["PatientId"]);
            string sqlselect;
            string strdelete;
            Int32 PharmacyId = 0;
            DateTime OrderedbyDate = System.DateTime.Now;

            if (Session["PharmacyId"] != null)
                PharmacyId = 99999;
            if (txtpharmOrderedbyDate.Value.ToString() != "")
                OrderedbyDate = Convert.ToDateTime(txtpharmOrderedbyDate.Value.ToString());
            ICustomFields CustomFields;

            if (sbValues.ToString().Trim() != "")
            {
                if (ViewState["CustomFieldsData"] != null)
                {
                    sbValues = sbValues.Remove(0, 1);
                    sqlstr = "UPDATE dtl_CustomField_" + TableName.ToString().Replace("-", "_") + " SET ";

                    sqlstr += sbValues.ToString() + " where Ptn_pk= " + PatientId.ToString() + " and ptn_pharmacy_pk=" + PharmacyId.ToString();
                }
                else
                {
                    sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate " + sbParameter.ToString() + " )";

                    sqlstr += " VALUES(" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
                    ViewState["CustomFieldsData"] = 1;
                }


                CustomFields = null;

            }
            if (strmultiselect.ToString() != "")
            {
                string[] FieldValues = strmultiselect.Split(new char[] { '^' });
                if (arl.Count != 0)
                {
                    int p = 0;
                    foreach (object obj in arl)
                    {
                        sqlselect = "";
                        strdelete = "";
                        if (obj.ToString() != "")
                        {
                            CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");

                            strdelete = "DELETE from [" + obj.ToString() + "] where ptn_pk= " + PatientId.ToString() + "  and ptn_pharmacy_pk=" + PharmacyId;
                            if (sqlstr == "")
                            {
                                sqlstr = strdelete;
                            }
                            else
                            {
                                sqlstr = sqlstr + "!" + strdelete;
                            }


                            if (FieldValues[p].ToString() != "")
                            {
                                string[] mValues = FieldValues[p].Split(new char[] { ',' });

                                foreach (string str in mValues)
                                {
                                    if (str.ToString() != "")
                                    {
                                        string strtab = "dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_";
                                        Int32 ispos = Convert.ToInt32(strtab.Length);
                                        Int32 iepos = Convert.ToInt32(obj.ToString().Length) - ispos;

                                        sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate, [" + obj.ToString().Substring(ispos, iepos) + "])";

                                        sqlselect += " VALUES (" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";

                                        if (sqlstr == "")
                                        {
                                            sqlstr = sqlselect;
                                        }
                                        else
                                        {
                                            sqlstr = sqlstr + "!" + sqlselect;
                                        }


                                    }
                                }
                            }


                        }
                        p += 1;
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
        return sqlstr;
    }
    private void UpdateCustomFieldsValues()
    {
        GenerateUpdateCustomFieldsValues(pnlCustomList);
        string sqlstr = string.Empty;
        Int32 PatientId = Convert.ToInt32(Session["PatientId"]);
        string sqlselect;
        string strdelete;
        Int32 PharmacyId = 0;
        DateTime OrderedbyDate = System.DateTime.Now;

        if (Session["PharmacyId"] != null)
            PharmacyId = Convert.ToInt32(Session["PharmacyId"]);
        if (txtpharmOrderedbyDate.Value.ToString() != "")
            OrderedbyDate = Convert.ToDateTime(txtpharmOrderedbyDate.Value.ToString());
        ICustomFields CustomFields;

        if (sbValues.ToString().Trim() != "")
        {
            if (ViewState["CustomFieldsData"] != null)
            {
                sbValues = sbValues.Remove(0, 1);
                sqlstr = "UPDATE dtl_CustomField_" + TableName.ToString().Replace("-", "_") + " SET ";
                //sqlstr += sbValues.ToString() + " where Ptn_pk= " + PatientId.ToString() + " and LocationID=" + Application["AppLocationID"] + " and ptn_pharmacy_pk=" + PharmacyId.ToString();
                sqlstr += sbValues.ToString() + " where Ptn_pk= " + PatientId.ToString() + " and ptn_pharmacy_pk=" + PharmacyId.ToString();
            }
            else
            {
                sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate " + sbParameter.ToString() + " )";
                //sqlstr += " VALUES(" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
                sqlstr += " VALUES(" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
                ViewState["CustomFieldsData"] = 1;
            }

            try
            {
                CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
                icount = CustomFields.SaveCustomFieldValues(sqlstr.ToString());
                if (icount == -1)
                {
                    return;
                }
            }
            catch
            {
            }
            finally
            {
                CustomFields = null;
            }
        }
        if (strmultiselect.ToString() != "")
        {
            string[] FieldValues = strmultiselect.Split(new char[] { '^' });
            if (arl.Count != 0)
            {
                int p = 0;
                foreach (object obj in arl)
                {
                    sqlselect = "";
                    strdelete = "";
                    if (obj.ToString() != "")
                    {
                        try
                        {
                            CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
                            //strdelete = "DELETE from [" + obj.ToString() + "] where ptn_pk= " + PatientId.ToString() + " and LocationID=" + Application["AppLocationID"] + " and ptn_pharmacy_pk=" + PharmacyId ;
                            strdelete = "DELETE from [" + obj.ToString() + "] where ptn_pk= " + PatientId.ToString() + "  and ptn_pharmacy_pk=" + PharmacyId;
                            icount = CustomFields.SaveCustomFieldValues(strdelete.ToString());

                            if (FieldValues[p].ToString() != "")
                            {
                                string[] mValues = FieldValues[p].Split(new char[] { ',' });

                                foreach (string str in mValues)
                                {
                                    if (str.ToString() != "")
                                    {
                                        string strtab = "dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_";
                                        Int32 ispos = Convert.ToInt32(strtab.Length);
                                        Int32 iepos = Convert.ToInt32(obj.ToString().Length) - ispos;

                                        sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate, [" + obj.ToString().Substring(ispos, iepos) + "])";
                                        //sqlselect += " VALUES (" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";
                                        sqlselect += " VALUES (" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";



                                        icount = CustomFields.SaveCustomFieldValues(sqlselect.ToString());
                                        if (icount == -1)
                                        {
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            CustomFields = null;
                        }
                    }
                    p += 1;
                }
            }
        }
    }


    private void GenerateCustomFieldsValues(Control Cntrl)
    {
        string pnlName = Cntrl.ID;
        sbValues = new StringBuilder();
        strmultiselect = string.Empty;
        string strfName = string.Empty;
        Boolean radioflag = false;

        Int32 stpos = 0;
        Int32 enpos = 0;
        if (ViewState["CustomFieldsData"] != null)
        {
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "TXT")
                    {
                        strfName = pnlName.ToUpper() + "TXT";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",'" + ((TextBox)x).Text.ToString() + "'");
                            //sbValues.Append(",[" + strfName + "] = '" + ((TextBox)x).Text.ToString() + "'");
                        }
                        else
                        {
                            sbValues.Append(",'" + "'");
                            //sbValues.Append(",[" + strfName + "] = ' " + "'");
                        }
                    }
                    else if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "NUM")
                    {
                        strfName = pnlName.ToUpper() + "NUM";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",'" + ((TextBox)x).Text.ToString() + "'");
                            //sbValues.Append(",[" + strfName + "]=" + ((TextBox)x).Text.ToString());
                        }
                        else
                        {
                            sbValues.Append(",' " + "'");
                            //sbValues.Append("," + strfName + "=Null");
                        }

                    }
                    else if (x.ID.Substring(0, 15).ToString().ToUpper() == pnlName.ToUpper() + "DT")
                    {
                        strfName = pnlName.ToUpper() + "DT";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((TextBox)x).Text != "")
                        {

                            sbValues.Append(",'" + Convert.ToDateTime(((TextBox)x).Text.ToString()) + "'");
                        }
                        else
                        {
                            sbValues.Append("," + "Null");
                        }
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                {
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO1")
                    {
                        radioflag = false;
                        strfName = pnlName.ToUpper() + "RADIO1";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append("," + "1");
                            radioflag = true;
                        }
                    }
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO2")
                    {
                        strfName = pnlName.ToUpper() + "RADIO2";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append("," + "0");
                            radioflag = true;
                        }
                        if (radioflag == false)
                        {
                            sbValues.Append("," + "Null");
                        }
                    }

                }
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    if (x.ID.Substring(0, 23).ToString().ToUpper() == pnlName.ToUpper() + "SELECTLIST")
                    {
                        strfName = pnlName.ToUpper() + "SELECTLIST";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((DropDownList)x).SelectedValue != "0")
                        {
                            sbValues.Append("," + ((DropDownList)x).SelectedValue.ToString() + " ");
                        }
                        else
                        {
                            sbValues.Append("," + "0");
                        }

                    }
                }

            }
        }

        if (ViewState["CustomFieldsData"] == null)
        {
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "TXT")
                    {
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",'" + ((TextBox)x).Text.ToString() + "'");
                        }
                        else
                        {
                            sbValues.Append(",' " + "'");
                        }
                    }
                    else if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "NUM")
                    {
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append("," + ((TextBox)x).Text.ToString());
                        }
                        else
                        {
                            sbValues.Append(",0");
                        }

                    }
                    else if (x.ID.Substring(0, 15).ToString().ToUpper() == pnlName.ToUpper() + "DT")
                    {
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",'" + Convert.ToDateTime(((TextBox)x).Text.ToString()) + "'");
                        }
                        else
                        {
                            sbValues.Append(",Null");
                        }
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                {
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO1")
                    {
                        radioflag = false;
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append(",1");
                            radioflag = true;
                        }
                    }
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO2")
                    {
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append(",0");
                            radioflag = true;
                        }
                        if (radioflag == false)
                        {
                            sbValues.Append(",Null");
                        }
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    if (x.ID.Substring(0, 23).ToString().ToUpper() == pnlName.ToUpper() + "SELECTLIST")
                    {
                        if (((DropDownList)x).SelectedValue != "0")
                        {
                            sbValues.Append("," + ((DropDownList)x).SelectedValue.ToString() + " ");
                        }
                        else
                        {
                            sbValues.Append(", " + "0");
                        }

                    }
                }
            }
        }
        if (Session["CustomFieldsMulti"] != null || Session["CustomFieldsMulti"] == null)
        {
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBoxList))
                {

                    if (x.ID.Substring(0, 28).ToString().ToUpper() == pnlName.ToUpper() + "MULTISELECTLIST")
                    {

                        foreach (ListItem li in ((CheckBoxList)x).Items)
                        {
                            if (Convert.ToInt32(li.Selected) == 1)
                            {
                                strmultiselect += " " + li.Value.ToString() + ",";
                            }
                        }
                        strmultiselect += "^";
                    }
                }
            }
        }
    }

    private void GenerateUpdateCustomFieldsValues(Control Cntrl)
    {
        string pnlName = Cntrl.ID;
        sbValues = new StringBuilder();
        strmultiselect = string.Empty;
        string strfName = string.Empty;
        Boolean radioflag = false;

        Int32 stpos = 0;
        Int32 enpos = 0;
        if (ViewState["CustomFieldsData"] != null)
        {
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "TXT")
                    {
                        strfName = pnlName.ToUpper() + "TXT";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((TextBox)x).Text != "")
                        {
                            //sbValues.Append(",'" + ((TextBox)x).Text.ToString() + "'");
                            sbValues.Append(",[" + strfName + "] = '" + ((TextBox)x).Text.ToString() + "'");
                        }
                        else
                        {
                            sbValues.Append(",[" + strfName + "] = ' " + "'");
                        }
                    }
                    else if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "NUM")
                    {
                        strfName = pnlName.ToUpper() + "NUM";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",[" + strfName + "]=" + ((TextBox)x).Text.ToString());
                        }
                        else
                        {
                            sbValues.Append("," + strfName + "=Null");
                        }

                    }
                    else if (x.ID.Substring(0, 15).ToString().ToUpper() == pnlName.ToUpper() + "DT")
                    {
                        strfName = pnlName.ToUpper() + "DT";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",[" + strfName + "]='" + Convert.ToDateTime(((TextBox)x).Text.ToString()) + "'");
                        }
                        else
                        {
                            sbValues.Append(",[" + strfName + "]=" + "Null");
                        }
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                {
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO1")
                    {
                        radioflag = false;
                        strfName = pnlName.ToUpper() + "RADIO1";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append(",[" + strfName + "]=" + "1");
                            radioflag = true;
                        }
                    }
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO2")
                    {
                        strfName = pnlName.ToUpper() + "RADIO2";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append(",[" + strfName + "]=" + "0");
                            radioflag = true;
                        }
                        if (radioflag == false)
                        {
                            sbValues.Append(",[" + strfName + "]=" + "Null");
                        }
                    }

                }
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    if (x.ID.Substring(0, 23).ToString().ToUpper() == pnlName.ToUpper() + "SELECTLIST")
                    {
                        strfName = pnlName.ToUpper() + "SELECTLIST";
                        stpos = strfName.Length;
                        enpos = x.ID.Length - stpos;
                        strfName = x.ID.Substring(stpos, enpos).ToString();
                        if (((DropDownList)x).SelectedValue != "0")
                        {
                            sbValues.Append(",[" + strfName + "] = " + ((DropDownList)x).SelectedValue.ToString() + " ");
                        }
                        else
                        {
                            sbValues.Append(",[" + strfName + "] =  " + "0");
                        }

                    }
                }

            }
        }

        if (ViewState["CustomFieldsData"] == null)
        {
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "TXT")
                    {
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",'" + ((TextBox)x).Text.ToString() + "'");
                        }
                        else
                        {
                            sbValues.Append(",' " + "'");
                        }
                    }
                    else if (x.ID.Substring(0, 16).ToString().ToUpper() == pnlName.ToUpper() + "NUM")
                    {
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append("," + ((TextBox)x).Text.ToString());
                        }
                        else
                        {
                            sbValues.Append(",0");
                        }

                    }
                    else if (x.ID.Substring(0, 15).ToString().ToUpper() == pnlName.ToUpper() + "DT")
                    {
                        if (((TextBox)x).Text != "")
                        {
                            sbValues.Append(",'" + Convert.ToDateTime(((TextBox)x).Text.ToString()) + "'");
                        }
                        else
                        {
                            sbValues.Append(",Null");
                        }
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                {
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO1")
                    {
                        radioflag = false;
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append(",1");
                            radioflag = true;
                        }
                    }
                    if (x.ID.Substring(0, 19).ToString().ToUpper() == pnlName.ToUpper() + "RADIO2")
                    {
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            sbValues.Append(",0");
                            radioflag = true;
                        }
                        if (radioflag == false)
                        {
                            sbValues.Append(",Null");
                        }
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    if (x.ID.Substring(0, 23).ToString().ToUpper() == pnlName.ToUpper() + "SELECTLIST")
                    {
                        if (((DropDownList)x).SelectedValue != "0")
                        {
                            sbValues.Append("," + ((DropDownList)x).SelectedValue.ToString() + " ");
                        }
                        else
                        {
                            sbValues.Append(", " + "0");
                        }

                    }
                }
            }
        }
        if (Session["CustomFieldsMulti"] != null || Session["CustomFieldsMulti"] == null)
        {
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBoxList))
                {

                    if (x.ID.Substring(0, 28).ToString().ToUpper() == pnlName.ToUpper() + "MULTISELECTLIST")
                    {

                        foreach (ListItem li in ((CheckBoxList)x).Items)
                        {
                            if (Convert.ToInt32(li.Selected) == 1)
                            {
                                strmultiselect += " " + li.Value.ToString() + ",";
                            }
                        }
                        strmultiselect += "^";
                    }
                }
            }
        }
    }


    //Populate Old Data in the Custom Field


    private void FillOldCustomData(Int32 PatID)
    {
        DataSet dsvalues = null;
        ICustomFields CustomFields;
        Int32 PharmacyId = 0;
        if (Session["PatientVisitId"] != null)
            PharmacyId = Convert.ToInt32(Session["PatientVisitId"]);
        try
        {
            DataSet theCustomFields = (DataSet)ViewState["CustomFieldsDS"];
            string theTblName = "";
            if (theCustomFields.Tables[0].Rows.Count > 0)
            {
               theTblName = theCustomFields.Tables[0].Rows[0]["FeatureName"].ToString().Replace(" ", "_");
            }
            string theColName = "";
            foreach (DataRow theDR in theCustomFields.Tables[0].Rows)
            {
                if (theDR["ControlId"].ToString() != "9")
                {
                    if (theColName == "")
                        theColName = theDR["Label"].ToString();
                    else
                        theColName = theColName + "," + theDR["Label"].ToString();
                }
            }

            CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
            dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + theTblName.ToString().Replace("-", "_"), theColName, Convert.ToInt32(PatID.ToString()), 0, 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.Pharmacy));
            CustomFieldClinical theCustomManager = new CustomFieldClinical();
            theCustomManager.FillCustomFieldData(theCustomFields, dsvalues, pnlCustomList, "CTCPharm");
        }
        catch (Exception err)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = err.Message.ToString();
            IQCareMsgBox.Show("#C1", theBuilder, this);
        }
        finally
        {
            CustomFields = null;
        }


        //string pnlName = Cntrl.ID;

        //DataSet dsvalues = null;
        //ICustomFields CustomFields;
        //Int32 PharmacyId = 0;
        //if (Session["PatientVisitId"] != null)
        //    PharmacyId = Convert.ToInt32(Session["PatientVisitId"]);

        //try
        //{
        //    CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
        //    //dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + TableName.ToString().Replace("-", "_"), sbParameter.ToString(), Convert.ToInt32(PatID.ToString()), Convert.ToInt32(Application["AppLocationID"].ToString()), 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
        //    dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + TableName.ToString().Replace("-", "_"), sbParameter.ToString(), Convert.ToInt32(PatID.ToString()), 0, 0,0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
        //}
        //catch
        //{
        //}
        //finally
        //{
        //    CustomFields = null;
        //}
        //try
        //{
        //    Boolean blnflag = false;
        //    foreach (DataTable dt in dsvalues.Tables)
        //    {
        //        blnflag = true;
        //    }

        //    if (dsvalues != null && blnflag && dsvalues.Tables[0].Rows.Count > 0)
        //    {
        //        //if any data exist then set the View State
        //        ViewState["CustomFieldsData"] = 1;
        //        foreach (Control x in Cntrl.Controls)
        //        {

        //            if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
        //            {
        //                foreach (DataColumn dc in dsvalues.Tables[0].Columns)
        //                {
        //                    if (pnlName.ToUpper() + "SELECTLIST" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                    {
        //                        if (((DropDownList)x).SelectedValue == "0")
        //                        {
        //                            ((DropDownList)x).SelectedValue = dsvalues.Tables[0].Rows[0][dc.ColumnName].ToString();
        //                        }
        //                    }

        //                }
        //            }
        //            if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
        //            {
        //                foreach (DataColumn dc in dsvalues.Tables[0].Columns)
        //                {
        //                    if (pnlName.ToUpper() + "RADIO1" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                    {
        //                        if (dsvalues.Tables[0].Rows[0][dc.ColumnName].ToString() == "True")
        //                        {
        //                            ((HtmlInputRadioButton)x).Checked = true;
        //                        }
        //                    }
        //                    if (pnlName.ToUpper() + "RADIO2" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                    {
        //                        if (dsvalues.Tables[0].Rows[0][dc.ColumnName].ToString() == "False")
        //                        {
        //                            ((HtmlInputRadioButton)x).Checked = true;
        //                        }
        //                    }
        //                }
        //            }
        //            if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
        //            {
        //                foreach (DataColumn dc in dsvalues.Tables[0].Columns)
        //                {
        //                    if (pnlName.ToUpper() + "TXT" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                    {
        //                        if (((TextBox)x).Text == "")
        //                        {
        //                            ((TextBox)x).Text = dsvalues.Tables[0].Rows[0][dc.ColumnName].ToString();
        //                            break;
        //                        }
        //                    }
        //                    if (pnlName.ToUpper() + "NUM" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                    {
        //                        if (((TextBox)x).Text == "")
        //                        {
        //                            ((TextBox)x).Text = dsvalues.Tables[0].Rows[0][dc.ColumnName].ToString();
        //                            break;
        //                        }
        //                    }
        //                    if (pnlName.ToUpper() + "DT" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                    {
        //                        if (((TextBox)x).Text == "")
        //                        {
        //                            if (dsvalues.Tables[0].Rows[0][dc.ColumnName] != System.DBNull.Value)
        //                            {
        //                                //((TextBox)x).Text = ((DateTime)dsvalues.Tables[0].Rows[0][dc.ColumnName]).ToString(Application["AppDateFormat"].ToString());
        //                                ((TextBox)x).Text = ((DateTime)dsvalues.Tables[0].Rows[0][dc.ColumnName]).ToString(Session["AppDateFormat"].ToString());
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBoxList))
        //            {
        //                DataSet dsmvalues = null;
        //                try
        //                {
        //                    string strfldName = pnlName.ToUpper() + "MULTISELECTLIST";
        //                    Int32 stpos = strfldName.Length;
        //                    Int32 enpos = x.ID.Length - stpos;
        //                    strfldName = x.ID.Substring(stpos, enpos).ToString();
        //                    CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
        //                    //dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()), Convert.ToInt32(Application["AppLocationID"].ToString()), 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
        //                    dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()), 0, 0,0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
        //                    if (dsmvalues != null && dsmvalues.Tables[0].Rows.Count > 0)
        //                        Session["CustomFieldsMulti"] = 1;
        //                    foreach (DataRow dr in dsmvalues.Tables[0].Rows)
        //                    {
        //                        foreach (DataColumn dc in dsmvalues.Tables[0].Columns)
        //                        {
        //                            if (pnlName.ToUpper() + "MULTISELECTLIST" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                            {
        //                                foreach (ListItem li in ((CheckBoxList)x).Items)
        //                                {
        //                                    if (li.Value == dr[dc.ColumnName].ToString())
        //                                    {
        //                                        li.Selected = true;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                }
        //                finally
        //                {
        //                    CustomFields = null;
        //                    dsmvalues = null;
        //                }

        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (Control x in Cntrl.Controls)
        //            if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBoxList))
        //            {
        //                DataSet dsmvalues = null;
        //                try
        //                {
        //                    string strfldName = pnlName.ToUpper() + "MULTISELECTLIST";
        //                    Int32 stpos = strfldName.Length;
        //                    Int32 enpos = x.ID.Length - stpos;
        //                    strfldName = x.ID.Substring(stpos, enpos).ToString();
        //                    CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
        //                    //dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()), Convert.ToInt32(Application["AppLocationID"].ToString()), 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
        //                    dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()), 0, 0,0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
        //                    if (dsmvalues != null && dsmvalues.Tables[0].Rows.Count > 0)
        //                        Session["CustomFieldsMulti"] = 1;


        //                    foreach (DataRow dr in dsmvalues.Tables[0].Rows)
        //                    {
        //                        foreach (DataColumn dc in dsmvalues.Tables[0].Columns)
        //                        {
        //                            if (pnlName.ToUpper() + "MULTISELECTLIST" + dc.ColumnName.ToUpper() == x.ID.ToUpper())
        //                            {
        //                                foreach (ListItem li in ((CheckBoxList)x).Items)
        //                                {
        //                                    if (li.Value == dr[dc.ColumnName].ToString())
        //                                    {
        //                                        li.Selected = true;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                }
        //                finally
        //                {
        //                    CustomFields = null;
        //                    dsmvalues = null;
        //                }

        //            }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ex.Message.ToString();
        //}

    }
    private void LoadAdditionalDrugs(DataTable theDT, Panel thePanel)
    {
        thePanel.Controls.Clear();
        foreach (DataRow theDR in theDT.Rows)
        {
            if (thePanel.ID == "PnlAddARV")
            {
                BindCustomControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);
            }
            else
            {
                BindAdditionalDrugControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);
            }
        }
    }
    private void BindCustomControls(int DrugId, int Generic, Panel MstPanel)
    {
        string strBrowser = Request.Browser.Browser;
        if (MstPanel.Controls.Count < 1)
        {
            #region "Additional ARV"
            Panel thelblPnl = new Panel();
            thelblPnl.ID = "pnlAdditionalARV";

            if (strBrowser == "IE")
            {
                thelblPnl.Height = 20;
            }
            else
            {
                thelblPnl.Height = 30;
            }

            thelblPnl.Width = 840;
            thelblPnl.Controls.Clear();

            Label theLabel = new Label();
            theLabel.ID = "lblADDARV";
            theLabel.Text = "Additional ARV";
            theLabel.Font.Bold = true;
            thelblPnl.Controls.Add(theLabel);

            Label theAddSpace = new Label();
            theAddSpace.ID = "lblAddARVSpace";
            theAddSpace.Text = "";
            theAddSpace.Width = 860;
            thelblPnl.Controls.Add(theAddSpace);



            Label theAddSpace1 = new Label();
            theAddSpace1.ID = "lblAddARVSpace1";
            theAddSpace1.Text = "";
            theAddSpace1.Width = 220;
            thelblPnl.Controls.Add(theAddSpace1);

            Label theLabel2 = new Label();
            theLabel2.ID = "lblAddARVDrgDose";
            theLabel2.Text = "Dose";
            theLabel2.Width = 90;
            thelblPnl.Controls.Add(theLabel2);

            Label theLabel4 = new Label();
            theLabel4.ID = "lblAddARVFrequency";
            theLabel4.Text = "Frequency";
            theLabel4.Width = 90;
            thelblPnl.Controls.Add(theLabel4);

            Label theLabel5 = new Label();
            theLabel5.ID = "lblAddARVDuration";
            theLabel5.Text = "*Duration";
            theLabel5.CssClass = "required";
            theLabel5.Width = 120;
            thelblPnl.Controls.Add(theLabel5);

            Label theLabel6 = new Label();
            theLabel6.ID = "lblAddARVPrescribed";
            theLabel6.Text = "Qty. Prescribed";
            theLabel6.Width = 110;
            thelblPnl.Controls.Add(theLabel6);

            Label theLabel7 = new Label();
            theLabel7.ID = "lblAddARVDispensed";
            theLabel7.Text = "Qty. Dispensed";
            theLabel7.Width = 120;
            thelblPnl.Controls.Add(theLabel7);

            Label theFinLbl = new Label();
            theFinLbl.ID = "lblAddARVFin";
            //theFinLbl.Text = "AR";
            theFinLbl.Font.Bold = true;
            thelblPnl.Controls.Add(theFinLbl);

            if (ddlTreatment.SelectedItem.Value.ToString() == "223")
            {
                Label theAddSpacePro = new Label();
                theAddSpacePro.ID = "lblAddARVproSpace";
                theAddSpacePro.Text = "";
                theAddSpacePro.Width = 20;
                thelblPnl.Controls.Add(theAddSpacePro);

                Label theProphyLbl = new Label();
                theProphyLbl.ID = "lblAddARVProphy";
                theProphyLbl.Text = "Prophylaxis";
                theProphyLbl.Font.Bold = true;
                thelblPnl.Controls.Add(theProphyLbl);
            }

            MstPanel.Controls.Add(thelblPnl);
            #endregion
        }
        Panel thePnl = new Panel();
        thePnl.ID = "pnl_" + DrugId;

        if (strBrowser == "IE")
        {
            thePnl.Height = 20;
        }
        else
        {
            thePnl.Height = 30;
        }

        thePnl.Width = 840;
        thePnl.Controls.Clear();


        Label lblStSp = new Label();
        lblStSp.Width = 5;
        lblStSp.ID = "stSpace" + DrugId;
        lblStSp.Text = "";
        thePnl.Controls.Add(lblStSp);

        DataView theDV;
        DataSet theDS = (DataSet)Session["MasterData"];
        if (Generic == 0)
        {
            //theDV = new DataView(theDS.Tables[0]); rupesh - for showing inactive drug
            theDV = new DataView(theDS.Tables[10]);
            theDV.RowFilter = "Drug_Pk = " + DrugId;
        }
        else
        {
            theDV = new DataView(theDS.Tables[4]);
            theDV.RowFilter = "GenericId = " + DrugId;
        }

        Label theDrugNm = new Label();
        theDrugNm.ID = "drgNm" + DrugId;
        theDrugNm.Text = theDV[0][1].ToString();
        theDrugNm.Width = 180;
        thePnl.Controls.Add(theDrugNm);

        /////// Space//////
        Label theSpace = new Label();
        theSpace.ID = "theSpace_" + DrugId;
        theSpace.Width = 20;
        theSpace.Text = "";
        ////////////////////

        thePnl.Controls.Add(theSpace);

        BindFunctions theBindMgr = new BindFunctions();
        DropDownList theDrugStrength = new DropDownList();
        theDrugStrength.ID = "drgStrength" + DrugId;
        theDrugStrength.Width = 80;
        #region "BindCombo"
        DataTable theDTS = new DataTable();

        DataView theDVStrength = new DataView(theDS.Tables[1]);
        if (Generic == 0)
        {

            theDTS = (DataTable)Session["FixDrugStrength"];
            theDVStrength = new DataView(theDTS);
            theDVStrength.RowFilter = "Drug_pk = " + DrugId + " and StrengthId>0";

        }
        else
        {
            theDVStrength.RowFilter = "GenericId = " + DrugId;
        }
        DataTable theDTStrength = new DataTable();

        if (theDVStrength.Count > 0)
        {
            IQCareUtils theUtils = new IQCareUtils();
            theDTStrength = theUtils.CreateTableFromDataView(theDVStrength);
            theBindMgr.BindCombo(theDrugStrength, theDTStrength, "StrengthName", "StrengthId");
        }

        #endregion
        thePnl.Controls.Add(theDrugStrength);

        ////////////Space////////////////////////
        Label theSpace1 = new Label();
        theSpace1.ID = "theSpace1" + DrugId;
        theSpace1.Width = 20;
        theSpace1.Text = "";
        thePnl.Controls.Add(theSpace1);
        ////////////////////////////////////////

        DropDownList theDrugFrequency = new DropDownList();
        theDrugFrequency.ID = "drgFrequency" + DrugId;
        theDrugFrequency.Width = 80;
        #region "BindCombo"
        DataTable theDTF = new DataTable();
        DataView theDVFrequency = new DataView(theDS.Tables[2]);
        if (Generic == 0)
        {

            theDTF = (DataTable)Session["FixDrugFreq"];
            theDVFrequency = new DataView(theDTF);
            theDVFrequency.RowFilter = "Drug_pk = " + DrugId + " and FrequencyId>0";

        }
        else
        {
            theDVFrequency.RowFilter = "GenericId = " + DrugId;
        }
        DataTable theDTFrequency = new DataTable();
        if (theDVFrequency.Count > 0)
        {
            IQCareUtils theUtils = new IQCareUtils();
            theDTFrequency = theUtils.CreateTableFromDataView(theDVFrequency);
            theBindMgr.BindCombo(theDrugFrequency, theDTFrequency, "FrequencyName", "FrequencyId");
        }
        #endregion
        thePnl.Controls.Add(theDrugFrequency);

        ////////////Space////////////////////////
        Label theSpace2 = new Label();
        theSpace2.ID = "theSpace2" + DrugId;
        theSpace2.Width = 15;
        theSpace2.Text = "";
        thePnl.Controls.Add(theSpace2);
        ////////////////////////////////////////

        TextBox theDuration = new TextBox();
        theDuration.ID = "drgDuration" + DrugId;
        theDuration.Width = 100;
        theDuration.Load += new EventHandler(Control_Load);
        thePnl.Controls.Add(theDuration);

        ////////////Space////////////////////////
        Label theSpace3 = new Label();
        theSpace3.ID = "theSpace3" + DrugId;
        theSpace3.Width = 20;
        theSpace3.Text = "";
        thePnl.Controls.Add(theSpace3);
        ////////////////////////////////////////

        TextBox theQtyPrescribed = new TextBox();
        theQtyPrescribed.ID = "drgQtyPrescribed" + DrugId;
        theQtyPrescribed.Width = 100;
        theQtyPrescribed.Load += new EventHandler(Control_Load);
        thePnl.Controls.Add(theQtyPrescribed);
        ////////////Space////////////////////////
        Label theSpace4 = new Label();
        theSpace4.ID = "theSpace4" + DrugId;
        theSpace4.Width = 20;
        theSpace4.Text = "";
        thePnl.Controls.Add(theSpace4);
        ////////////////////////////////////////

        TextBox theQtyDispensed = new TextBox();
        theQtyDispensed.ID = "drgQtyDispensed" + DrugId;
        theQtyDispensed.Width = 100;
        theQtyDispensed.Load += new EventHandler(Control_Load);
        thePnl.Controls.Add(theQtyDispensed);
        ////////////Space////////////////////////
        Label theSpace5 = new Label();
        theSpace5.ID = "theSpace5" + DrugId;
        theSpace5.Width = 20;
        theSpace5.Text = "";
        thePnl.Controls.Add(theSpace5);
        ////////////////////////////////////////
        CheckBox theFinChk = new CheckBox();
        theFinChk.ID = "FinChk" + DrugId;
        theFinChk.Width = 10;
        theFinChk.Text = "";
        theFinChk.Visible = false;
        thePnl.Controls.Add(theFinChk);
        ////////////Space///////////////////////
        Label theSpace6 = new Label();
        theSpace6.ID = "theSpace6" + DrugId;
        theSpace6.Width = 30;
        theSpace6.Text = "";
        thePnl.Controls.Add(theSpace6);
        if (ddlTreatment.SelectedItem.Value.ToString() == "223")
        {
            CheckBox theOtherARTProPhChk = new CheckBox();
            theOtherARTProPhChk.ID = "chkProphylaxis" + DrugId;
            theOtherARTProPhChk.Width = 10;
            theOtherARTProPhChk.Text = "";
            thePnl.Controls.Add(theOtherARTProPhChk);
        }

        MstPanel.Controls.Add(thePnl);
    }
    private static void SortDataTable(DataTable dt, string sort)
    {

        DataTable newDT = dt.Clone();
        int rowCount = dt.Rows.Count;

        DataRow[] foundRows = dt.Select(null, sort);
        for (int i = 0; i < rowCount; i++)
        {
            object[] arr = new object[dt.Columns.Count];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                arr[j] = foundRows[i][j];
            }
            DataRow data_row = newDT.NewRow();
            data_row.ItemArray = arr;
            newDT.Rows.Add(data_row);
        }

        //clear the incoming dt 
        dt.Rows.Clear();

        for (int i = 0; i < newDT.Rows.Count; i++)
        {
            object[] arr = new object[dt.Columns.Count];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                arr[j] = newDT.Rows[i][j];
            }

            DataRow data_row = dt.NewRow();
            data_row.ItemArray = arr;
            dt.Rows.Add(data_row);
        }

    }
    private void FillOldData(Control Cntrl, DataRow theDR)
    {
        int y = 0;
        int DrugId;
        Int32 ipos;
        if (Convert.ToInt32(theDR["Drug_Pk"]) == 0)
        {
            DrugId = Convert.ToInt32(theDR["GenericId"]);
        }
        else
        {
            DrugId = Convert.ToInt32(theDR["Drug_Pk"]);
        }
        //add by santosh
        if ((DrugId == 85 || DrugId == 150 || DrugId == 486) && (theDR["Dose"].ToString() == "0.00" && theDR["FrequencyId"].ToString() == "0" && theDR["Duration"].ToString() == "0" && theDR["OrderedQuantity"].ToString() == "0" && theDR["DispensedQuantity"].ToString() == "0"))
        {
            foreach (Control x in Cntrl.Controls)
            {
                if (Cntrl.ID == "PnlOIARV")
                {

                    string strDropID1 = "drgStrength" + DrugId;
                    string strDropID2 = "drgFrequency" + DrugId;
                    string strText1 = "drgDuration" + DrugId;
                    string strText2 = "drgQtyDispensed" + DrugId;
                    string strText3 = "drgQtyPrescribed" + DrugId;



                    if (DrugId == 85)
                    {
                        string CtrlID = "theSaveChk" + DrugId;
                        ((CheckBox)Cntrl.FindControl(CtrlID)).Checked = true;
                        ((DropDownList)Cntrl.FindControl(strDropID1)).Enabled = false;
                        ((DropDownList)Cntrl.FindControl(strDropID2)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText1)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText2)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText3)).Enabled = false;
                        ((DropDownList)Cntrl.FindControl(strDropID1)).SelectedIndex = 0;
                        ((DropDownList)Cntrl.FindControl(strDropID2)).SelectedIndex = 0;
                        ((TextBox)Cntrl.FindControl(strText1)).Text = "";
                        ((TextBox)Cntrl.FindControl(strText2)).Text = "";
                        ((TextBox)Cntrl.FindControl(strText3)).Text = "";

                    }
                    else if (DrugId == 150)
                    {
                        string CtrlID = "theSaveChk" + DrugId;
                        ((CheckBox)Cntrl.FindControl(CtrlID)).Checked = true;
                        ((DropDownList)Cntrl.FindControl(strDropID1)).Enabled = false;
                        ((DropDownList)Cntrl.FindControl(strDropID2)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText1)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText2)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText3)).Enabled = false;
                        ((DropDownList)Cntrl.FindControl(strDropID1)).SelectedIndex = 0;
                        ((DropDownList)Cntrl.FindControl(strDropID2)).SelectedIndex = 0;
                        ((TextBox)Cntrl.FindControl(strText1)).Text = "";
                        ((TextBox)Cntrl.FindControl(strText2)).Text = "";
                        ((TextBox)Cntrl.FindControl(strText3)).Text = "";

                    }
                    else if (DrugId == 486)
                    {
                        string CtrlID = "theSaveChk" + DrugId;
                        ((CheckBox)Cntrl.FindControl(CtrlID)).Checked = true;
                        ((DropDownList)Cntrl.FindControl(strDropID1)).Enabled = false;
                        ((DropDownList)Cntrl.FindControl(strDropID2)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText1)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText2)).Enabled = false;
                        ((TextBox)Cntrl.FindControl(strText3)).Enabled = false;
                        ((DropDownList)Cntrl.FindControl(strDropID1)).SelectedIndex = 0;
                        ((DropDownList)Cntrl.FindControl(strDropID2)).SelectedIndex = 0;
                        ((TextBox)Cntrl.FindControl(strText1)).Text = "";
                        ((TextBox)Cntrl.FindControl(strText2)).Text = "";
                        ((TextBox)Cntrl.FindControl(strText3)).Text = "";

                    }

                }
            }

        }
        else
        {
            //
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                {
                    FillOldData(x, theDR);
                }
                else
                {
                    if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                    {
                        if (x.ID.StartsWith("drgStrength"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(11, ipos - 11));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(11, x.ID.Length - 11));
                            }
                            //y = Convert.ToInt32(x.ID.Substring(11, x.ID.Length - 11));
                            if (y == DrugId)
                            {
                                ((DropDownList)x).SelectedValue = theDR["StrengthId"].ToString();
                            }
                        }
                        if (x.ID.StartsWith("theUnit"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(7, ipos - 7));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(7, x.ID.Length - 7));
                            }
                            //y = Convert.ToInt32(x.ID.Substring(7, x.ID.Length - 7));
                            if (y == DrugId)
                            {
                                ((DropDownList)x).SelectedValue = theDR["UnitId"].ToString();
                            }
                        }
                        if (x.ID.StartsWith("drgFrequency"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(12, ipos - 12));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(12, x.ID.Length - 12));
                            }
                            //y = Convert.ToInt32(x.ID.Substring(12, x.ID.Length - 12));
                            if (y == DrugId)
                            {
                                ((DropDownList)x).SelectedValue = theDR["FrequencyId"].ToString();
                            }
                        }
                    }
                    if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                    {
                        if (x.ID.StartsWith("theDose"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(7, ipos - 7));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(7, x.ID.Length - 7));
                            }
                            if (y == DrugId)
                            {
                                ((TextBox)x).Text = theDR["Dose"].ToString();
                            }
                        }
                        if (x.ID.StartsWith("drgDuration"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(11, ipos - 11));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(11, x.ID.Length - 11));
                            }
                            //y = Convert.ToInt32(x.ID.Substring(11, x.ID.Length - 11));
                            if (y == DrugId)
                            {
                                ((TextBox)x).Text = theDR["Duration"].ToString();
                            }
                        }
                        if (x.ID.StartsWith("drgQtyPrescribed"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(16, ipos - 16));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(16, x.ID.Length - 16));
                            }
                            //y = Convert.ToInt32(x.ID.Substring(16, x.ID.Length - 16));
                            if (y == DrugId)
                            {
                                int DecPos = theDR["OrderedQuantity"].ToString().IndexOf(".");
                                //int DecValue=Convert.ToInt32(theDR["OrderedQuantity"].ToString().Substring(DecPos+1, 2));
                                int DecValue = Convert.ToInt32(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1,2));
                                if (DecValue > 0)
                                {
                                    ((TextBox)x).Text = theDR["OrderedQuantity"].ToString();

                                }
                                else
                                {
                                    ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["OrderedQuantity"]));
                                }
                            }
                        }
                        if (x.ID.StartsWith("drgQtyDispensed"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(15, ipos - 15));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(15, x.ID.Length - 15));
                            }
                            //y = Convert.ToInt32(x.ID.Substring(15, x.ID.Length - 15));
                            if (y == DrugId)
                            {
                                int DecPos = theDR["DispensedQuantity"].ToString().IndexOf(".");
                                //int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1,2));
                                if (DecValue > 0)
                                {
                                    ((TextBox)x).Text = theDR["DispensedQuantity"].ToString();

                                }
                                else
                                {
                                    ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                }
                                //((TextBox)x).Text = theDR["DispensedQuantity"].ToString();
                            }
                        }
                    }

                    if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                    {
                        if (x.ID.StartsWith("FinChk"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(6, ipos - 6));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(6, x.ID.Length - 6));
                            }
                            //y = Convert.ToInt32(x.ID.Substring(6, x.ID.Length - 6));
                            if (y == DrugId)
                            {
                                if (Convert.ToInt32(theDR["Financed"].ToString()) == 1)
                                {
                                    ((CheckBox)x).Checked = true;
                                }
                                else
                                {
                                    ((CheckBox)x).Checked = false;
                                }

                            }
                        }
                        if (x.ID.StartsWith("chkProphylaxis"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                y = Convert.ToInt32(x.ID.Substring(14, ipos - 14));
                            }
                            else
                            {
                                y = Convert.ToInt32(x.ID.Substring(14, x.ID.Length - 14));
                            }

                            if (y == DrugId)
                            {
                                if (Convert.ToString(theDR["Prophylaxis"]) != "")
                                {
                                    if (Convert.ToInt32(theDR["Prophylaxis"].ToString()) == 1)
                                    {
                                        ((CheckBox)x).Checked = true;
                                    }
                                    else
                                    {
                                        ((CheckBox)x).Checked = false;
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
    }
    private void BindAdditionalDrugControls(int DrugId, int Generic, Panel MstPanel)
    {
        //oi and other medications
        string strBrowser = Request.Browser.Browser;
        if (MstPanel.Controls.Count < 1)
        {
            #region "OI & Other Medication"
            Panel thelblPnl = new Panel();
            thelblPnl.ID = "pnlOtherDrug";
            if (strBrowser == "IE")
            {
                thelblPnl.Height = 20;
            }
            else
            {
                thelblPnl.Height = 30;
            }
            thelblPnl.Width = 840;
            thelblPnl.Controls.Clear();

            Label theLabel = new Label();
            theLabel.ID = "lblOtherDrug";
            theLabel.Text = "OI Treatment and Non-HIV/AIDS Medications";//earlier it was "OI Treatment and Other Medications";
            theLabel.Font.Bold = true;
            thelblPnl.Controls.Add(theLabel);
            MstPanel.Controls.Add(thelblPnl);

            /////////////////////////////////////////////////
            Panel theheaderPnl = new Panel();
            theheaderPnl.ID = "pnlHeaderOtherDrug";
            theheaderPnl.Height = 20;
            theheaderPnl.Width = 840;
            theheaderPnl.Font.Bold = true;
            theheaderPnl.Controls.Clear();

            Label theSP = new Label();
            theSP.ID = "lblDrgSp";
            theSP.Width = 5;
            theSP.Text = "";
            theheaderPnl.Controls.Add(theSP);

            Label theLabel1 = new Label();
            theLabel1.ID = "lblDrgNm";
            theLabel1.Text = "Drug Name";
            theLabel1.Width = 200;
            theheaderPnl.Controls.Add(theLabel1);

            Label theLabel2 = new Label();
            theLabel2.ID = "lblDrgDose1";
            theLabel2.Text = "Dose";
            theLabel2.Width = 90;
            theheaderPnl.Controls.Add(theLabel2);

            Label theLabel3 = new Label();
            theLabel3.ID = "lblDrgUnits";
            theLabel3.Text = "Unit";
            theLabel3.Width = 90;
            theheaderPnl.Controls.Add(theLabel3);

            Label theLabel4 = new Label();
            theLabel4.ID = "lblDrgFrequency";
            theLabel4.Text = "Frequency";
            theLabel4.Width = 90;
            theheaderPnl.Controls.Add(theLabel4);

            Label theLabel5 = new Label();
            theLabel5.ID = "lblDrgDuration";
            theLabel5.Text = "Duration";
            theLabel5.Width = 110;
            theheaderPnl.Controls.Add(theLabel5);

            Label theLabel6 = new Label();
            theLabel6.ID = "lblDrgPrescribed";
            theLabel6.Text = "Qty. Prescribed";
            theLabel6.Width = 110;
            theheaderPnl.Controls.Add(theLabel6);

            Label theLabel7 = new Label();
            theLabel7.ID = "lblDrgDispensed";
            theLabel7.Text = "Qty. Dispensed";
            theLabel7.Width = 110;
            theheaderPnl.Controls.Add(theLabel7);

            Label theLabel8 = new Label();
            theLabel8.ID = "lblDrgFinanced";
            theLabel8.Text = "AR";
            theLabel8.Width = 20;
            theheaderPnl.Controls.Add(theLabel8);

            MstPanel.Controls.Add(theheaderPnl);
            #endregion
        }

        Panel thePnl = new Panel();
        thePnl.ID = "pnl" + DrugId + "^" + Generic;
        if (strBrowser == "IE")
        {
            thePnl.Height = 20;
        }
        else
        {
            thePnl.Height = 30;
        }
        thePnl.Width = 840;
        thePnl.Controls.Clear();

        Label lblStSp = new Label();
        lblStSp.Width = 5;
        lblStSp.ID = "stSpace" + DrugId + "^" + Generic;
        lblStSp.Text = "";
        thePnl.Controls.Add(lblStSp);

        DataView theDV;
        DataSet theDS = (DataSet)Session["MasterData"];
        if (Generic == 0)
        {
            //theDV = new DataView(theDS.Tables[0]);
            theDV = new DataView(theDS.Tables[10]);
            theDV.RowFilter = "drug_pk = " + DrugId;
        }
        else
        {
            theDV = new DataView(theDS.Tables[4]);
            if (DrugId.ToString().LastIndexOf("9999") > 0)
            {

                DrugId = Convert.ToInt32(DrugId.ToString().Substring(0, DrugId.ToString().Length - 4));
            }
            theDV.RowFilter = "GenericId = " + DrugId;
        }

        Label theDrugNm = new Label();
        theDrugNm.ID = "drgNm" + DrugId + "^" + Generic;
        if (theDV.Count > 0)
        {
            theDrugNm.Text = theDV[0][1].ToString();
        }
        theDrugNm.Width = 180;
        thePnl.Controls.Add(theDrugNm);

        /////// Space//////
        Label theSpace = new Label();
        theSpace.ID = "theSpace" + DrugId + "^" + Generic;
        theSpace.Width = 20;
        theSpace.Text = "";
        thePnl.Controls.Add(theSpace);
        ////////////////////

        TextBox theDose = new TextBox();
        theDose.ID = "theDose" + DrugId + "^" + Generic;
        theDose.Text = "";
        theDose.Width = 80;
        theDose.Load += new EventHandler(Control_Load);//Rupes 16Jan08 
        //theDose.Text = Generic.ToString();
        #region "13-Jun-07 -2"
        //theDose.Load += new EventHandler(DecimalText_Load);
        #region "14-Jun-07 -1"
        //---- allow decimal value for Dose
        // theDose.Load += new EventHandler(Control_Load); //rupesh - OI and Other Medication 
        #endregion
        #endregion
        thePnl.Controls.Add(theDose);

        /////// Space//////
        Label theSpace1 = new Label();
        theSpace1.ID = "theSpace1*" + DrugId + "^" + Generic;
        theSpace1.Width = 10;
        theSpace1.Text = "";
        thePnl.Controls.Add(theSpace1);
        ////////////////////

        BindFunctions theBindMgr = new BindFunctions();
        DropDownList theUnit = new DropDownList();
        theUnit.ID = "theUnit" + DrugId + "^" + Generic;
        theUnit.Width = 80;
        DataTable DTUnit = new DataTable();
        DTUnit = theDS.Tables[5];
        theBindMgr.BindCombo(theUnit, DTUnit, "UnitName", "UnitId");
        thePnl.Controls.Add(theUnit);

        /////// Space//////
        Label theSpace2 = new Label();
        theSpace2.ID = "theSpace2*" + DrugId + "^" + Generic;
        theSpace2.Width = 10;
        theSpace2.Text = "";
        thePnl.Controls.Add(theSpace2);
        ////////////////////

        DropDownList theFrequency = new DropDownList();
        theFrequency.ID = "drgFrequency" + DrugId + "^" + Generic;
        theFrequency.Width = 80;
        DataTable DTFreq = new DataTable();
        //DTFreq = theDS.Tables[6]; // Rupesh 03-Sept 
        DTFreq = theDS.Tables[11];
        theBindMgr.BindCombo(theFrequency, DTFreq, "FrequencyName", "FrequencyId");
        thePnl.Controls.Add(theFrequency);

        /////// Space//////
        Label theSpace3 = new Label();
        theSpace3.ID = "theSpace3*" + DrugId + "^" + Generic;
        theSpace3.Width = 10;
        theSpace3.Text = "";
        thePnl.Controls.Add(theSpace3);
        ////////////////////

        TextBox theDuration = new TextBox();
        theDuration.ID = "drgDuration" + DrugId + "^" + Generic;
        theDuration.Width = 100;
        theDuration.Text = "";
        theDuration.Load += new EventHandler(Control_Load);
        thePnl.Controls.Add(theDuration);

        ////////////Space////////////////////////
        Label theSpace4 = new Label();
        theSpace4.ID = "theSpace4*" + DrugId + "^" + Generic;
        theSpace4.Width = 10;
        theSpace4.Text = "";
        thePnl.Controls.Add(theSpace4);
        ////////////////////////////////////////

        TextBox theQtyPrescribed = new TextBox();
        theQtyPrescribed.ID = "drgQtyPrescribed" + DrugId + "^" + Generic;
        theQtyPrescribed.Width = 100;
        theQtyPrescribed.Text = "";
        //theQtyPrescribed.Load += new EventHandler(DecimalText_Load);
        theQtyPrescribed.Load += new EventHandler(Control_Load);
        thePnl.Controls.Add(theQtyPrescribed);

        ////////////Space////////////////////////
        Label theSpace5 = new Label();
        theSpace5.ID = "theSpace5*" + DrugId + "^" + Generic;
        theSpace5.Width = 10;
        theSpace5.Text = "";
        thePnl.Controls.Add(theSpace5);
        ////////////////////////////////////////

        TextBox theQtyDispensed = new TextBox();
        theQtyDispensed.ID = "drgQtyDispensed" + DrugId + "^" + Generic;
        theQtyDispensed.Width = 100;
        theQtyDispensed.Text = "";
        #region "13-Jun-07 -3"
        //theQtyDispensed.Load += new EventHandler(DecimalText_Load); 
        theQtyDispensed.Load += new EventHandler(Control_Load); // rupesh
        #endregion
        thePnl.Controls.Add(theQtyDispensed);

        ////////////Space////////////////////////
        Label theSpace6 = new Label();
        theSpace6.ID = "theSpace6*" + DrugId + "^" + Generic;
        theSpace6.Width = 10;
        theSpace6.Text = "";
        thePnl.Controls.Add(theSpace6);
        ////////////////////////////////////////

        CheckBox theFinChk = new CheckBox();
        theFinChk.ID = "FinChk" + DrugId + "^" + Generic;
        theFinChk.Width = 10;
        theFinChk.Text = "";
        thePnl.Controls.Add(theFinChk);

        ////////////Space////////////////////////
        Label theSpace7 = new Label();
        theSpace7.ID = "theSpace7*" + DrugId + "^" + Generic;
        theSpace7.Width = 30;
        theSpace7.Text = "";
        thePnl.Controls.Add(theSpace7);
        ////////////////////////////////////////

        MstPanel.Controls.Add(thePnl);

    }
    private void MakeRegimenGenericTable(DataSet theDS)
    {
        DataTable theDT;
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            theDT = theDS.Tables[16];
        }
        else
        {
            theDT = theDS.Tables[14];
        }

        DataView theDV;//= new DataView();
        BindFunctions theBindMgr = new BindFunctions();
        int RegimenId = -1;
        string GenericID = string.Empty;
        string GenericName = string.Empty;

        DataTable theDT1 = new DataTable();
        theDT1.Columns.Add("RegimenID", System.Type.GetType("System.Int32"));
        theDT1.Columns.Add("RegimenName", System.Type.GetType("System.String"));
        theDT1.Columns.Add("Stage", System.Type.GetType("System.String"));
        theDT1.Columns.Add("UserID", System.Type.GetType("System.Int32"));
        theDT1.Columns.Add("GenericID", System.Type.GetType("System.String"));
        theDT1.Columns.Add("GenericName", System.Type.GetType("System.String"));
        theDT1.Columns.Add("Status", System.Type.GetType("System.String"));


        DataView DV = new DataView(theDT);
        //DV.Sort = "GenericID asc";
        IQCareUtils theUtil = new IQCareUtils();
        theDT = theUtil.CreateTableFromDataView(DV);

        #region "fillTable"
        for (int i = 0; i < theDT.Rows.Count; i++)
        {
            if (Convert.ToInt32(theDT.Rows[i]["RegimenID"]) > 0)
            {
                if (RegimenId != Convert.ToInt32(theDT.Rows[i]["RegimenID"]))
                {
                    RegimenId = Convert.ToInt32(theDT.Rows[i]["RegimenID"]);

                    theDV = new DataView(theDT);
                    theDV.RowFilter = "RegimenID = " + RegimenId;

                    if (theDV.Count > 0)
                    {

                        for (int j = 0; j < theDV.Count; j++)
                        {
                            if (GenericID.Trim() == "")
                            {
                                GenericID = Convert.ToString(theDV[j].Row["GenericID"]);
                            }
                            else
                            {
                                if (GenericID.Contains(Convert.ToString(theDV[j].Row["GenericID"])) == false)
                                    GenericID = GenericID + "/" + " " + Convert.ToString(theDV[j].Row["GenericID"]);
                            }

                            if (GenericName.Trim() == "")
                            {
                                GenericName = Convert.ToString(theDV[j].Row["GenericName"]);
                            }
                            else
                            {
                                if (GenericName.Contains(Convert.ToString(theDV[j].Row["GenericName"])) == false)
                                    GenericName = GenericName + "/" + " " + Convert.ToString(theDV[j].Row["GenericName"]);
                            }


                        }
        #endregion
                        DataRow theDR = theDT1.NewRow();
                        theDR["RegimenID"] = Convert.ToInt32(theDT.Rows[i]["RegimenID"]);
                        theDR["RegimenName"] = Convert.ToString(theDT.Rows[i]["RegimenCode"]) + "-" + theDT.Rows[i]["RegimenName"];
                        theDR["Stage"] = Convert.ToString(theDT.Rows[i]["Stage"]);
                        theDR["UserID"] = Convert.ToInt32(theDT.Rows[i]["UserID"]);
                        theDR["GenericID"] = GenericID;
                        theDR["GenericName"] = GenericName;
                        theDR["Status"] = Convert.ToString(theDT.Rows[i]["Status"]);
                        theDT1.Rows.Add(theDR);
                        GenericID = "";
                        GenericName = "";


                    }
                }
            }
            else
            {
                DataRow theDR = theDT1.NewRow();
                theDR["RegimenID"] = Convert.ToInt32(theDT.Rows[i]["RegimenID"]);
                theDR["RegimenName"] = Convert.ToString(theDT.Rows[i]["RegimenName"]);
                theDR["Stage"] = Convert.ToString(theDT.Rows[i]["Stage"]);
                theDR["UserID"] = Convert.ToInt32(theDT.Rows[i]["UserID"]);
                theDR["GenericID"] = Convert.ToString(theDT.Rows[0]["GenericID"]); ;
                theDR["GenericName"] = Convert.ToString(theDT.Rows[i]["GenericName"]);
                theDR["Status"] = Convert.ToString(theDT.Rows[i]["Status"]);
                theDT1.Rows.Add(theDR);
            }

        }


        DV = new DataView(theDT1);
        DV.Sort = "Stage asc";
        theDT1 = theUtil.CreateTableFromDataView(DV);
        theBindMgr.BindCombo(ddlARVCombReg, theDT1, "RegimenName", "RegimenID");
        theBindMgr.BindCombo(ddlARVCombRegFrqARV, theDS.Tables[15], "Name", "ID");


    }
    private DataTable MakeTable()
    {
        DataTable theDT = new DataTable();
        DataColumn[] theKey = new DataColumn[2];
        theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
        theDT.Columns.Add("GenericId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("GenericName", System.Type.GetType("System.String"));
        theKey[0] = theDT.Columns[0];
        theKey[1] = theDT.Columns[2];
        theDT.PrimaryKey = theKey;
        return theDT;
    }
    private void CreateControls(DataSet theCntlDS)
    {
        DataSet theDS = new DataSet();
        theDS.ReadXml(Server.MapPath("..\\XMLFiles\\adultpharmacylist.xml"));

        #region CreateTable"
        //DataTable theDrugTable = new DataTable();
        if (Session["SelectedDrug"] != null)
        {
            theDrugTable = (DataTable)Session["SelectedDrug"];
        }
        else
        {
            foreach (DataRow dr in theDS.Tables[0].Rows)
            {
                DataRow theDR = theDrugTable.NewRow();
                if (Session["SystemId"].ToString() == "2")
                {
                    if (Convert.ToInt32(dr[0]) == 10001 || Convert.ToInt32(dr[0]) == 486 || Convert.ToInt32(dr[0]) == 85 || Convert.ToInt32(dr[0]) == 150)
                    {
                        if (Convert.ToInt32(dr[2]) == 1)
                        {
                            theDR["DrugId"] = 0;
                            theDR["DrugName"] = "";
                            theDR["GenericId"] = dr[0];
                            theDR["GenericName"] = dr[1];
                        }
                        else
                        {
                            theDR["DrugId"] = dr[0];
                            theDR["DrugName"] = dr[1];
                            theDR["GenericId"] = 0;
                            theDR["GenericName"] = "";
                        }
                        theDrugTable.Rows.Add(theDR);
                    }
                }
            }
        }
        #endregion

        Label lblDrugName = new Label();
        lblDrugName.Text = "Drug Name";
        lblDrugName.ID = "lblDrugName";
        lblDrugName.Font.Bold = true;
        lblDrugName.Visible = true;

        Label lblSpace = new Label();
        lblSpace.Width = 190;
        lblSpace.ID = "lblSpace_1";
        lblSpace.Text = "";

        Label lblStrength = new Label();
        lblStrength.Text = "Dose";
        lblStrength.ID = "lblStrength";
        lblStrength.Font.Bold = true;
        lblStrength.Visible = true;

        Label lblSpace1 = new Label();
        lblSpace1.Width = 70;
        lblSpace1.ID = "lblSpace_2";
        lblSpace1.Text = "";

        Label lblFrequency = new Label();
        lblFrequency.Text = "Frequency";
        lblFrequency.ID = "lblFrequency";
        lblFrequency.Font.Bold = true;
        lblFrequency.Visible = true;

        Label lblSpace2 = new Label();
        lblSpace2.Width = 70;
        lblSpace2.ID = "lblSpace_3";
        lblSpace2.Text = "";

        Label lblDuration = new Label();
        lblDuration.Text = "Duration";
        lblDuration.ID = "lblDuration";
        lblDuration.Font.Bold = true;
        lblDuration.Visible = true;

        Label lblSpace3 = new Label();
        lblSpace3.Width = 70;
        lblSpace3.ID = "lblSpace_4";
        lblSpace3.Text = "";

        Label lblQtyPrescribed = new Label();
        lblQtyPrescribed.Text = "Qty. Prescribed";
        lblQtyPrescribed.ID = "lblQuantityPres";
        lblQtyPrescribed.Font.Bold = true;
        lblQtyPrescribed.Visible = true;

        Label lblSpace4 = new Label();
        lblSpace4.Width = 70;
        lblSpace4.ID = "lblSpace_5";
        lblSpace4.Text = "";

        Label lblQtyDispensed = new Label();
        lblQtyDispensed.Text = "Qty. Dispensed";
        lblQtyDispensed.ID = "lblQuantityDisp";
        lblQtyDispensed.Font.Bold = true;
        lblQtyDispensed.Visible = true;

        Label lblSpace5 = new Label();
        lblSpace5.Width = 70;
        lblSpace5.ID = "lblSpace_6";
        lblSpace5.Text = "";

        Label lblFin = new Label();
        lblFin.ID = "lblARVFin";
        lblFin.Text = "";
        lblFin.Font.Bold = true;



        Panel theHeaderPanel = new Panel();
        theHeaderPanel.ID = "Header";
        theHeaderPanel.Width = 840;
        theHeaderPanel.Height = 20;

        Label lblSp = new Label();
        lblSp.Width = 5;
        lblSp.ID = "stSpace";
        lblSp.Text = "";

        theHeaderPanel.Controls.Add(lblSp);
        theHeaderPanel.Controls.Add(lblSpace);
        theHeaderPanel.Controls.Add(lblDrugName);
        theHeaderPanel.Controls.Add(lblSpace);
        theHeaderPanel.Controls.Add(lblStrength);
        theHeaderPanel.Controls.Add(lblSpace1);
        theHeaderPanel.Controls.Add(lblFrequency);
        theHeaderPanel.Controls.Add(lblSpace2);
        theHeaderPanel.Controls.Add(lblDuration);
        theHeaderPanel.Controls.Add(lblSpace3);
        theHeaderPanel.Controls.Add(lblQtyPrescribed);
        theHeaderPanel.Controls.Add(lblSpace4);
        theHeaderPanel.Controls.Add(lblQtyDispensed);
        theHeaderPanel.Controls.Add(lblSpace5);
        theHeaderPanel.Controls.Add(lblFin);
        //PnlDrug.Controls.Add(theHeaderPanel);
        PnlOIARV.Controls.Add(theHeaderPanel);

        int i = 0;
        int theGenericId = 0;
        foreach (DataRow dr in theDrugTable.Rows)
        {
            theGenericId = 0;
            if (Convert.ToInt32(dr["GenericId"]) > 0)
            {
                theGenericId = Convert.ToInt32(dr["GenericId"].ToString());
            }
            else
            {

                theGenericId = Convert.ToInt32(dr["DrugId"].ToString());
            }

            Panel thePnl = new Panel();
            thePnl.ID = "pnl" + theGenericId;
            string strBrowser = Request.Browser.Browser;
            if (strBrowser == "IE")
            {
                thePnl.Height = 20;
            }
            else
            {
                thePnl.Height = 30;
            }
            thePnl.Width = 850;
            thePnl.Controls.Clear();

            if (theGenericId > 10000)
            {
                Label theHeading = new Label();
                if (Convert.ToInt32(dr["GenericId"]) > 0)
                {
                    theHeading.Text = dr["GenericName"].ToString();
                    theHeading.ID = "lbl" + dr["GenericName"].ToString();
                }
                else
                {
                    theHeading.Text = dr["DrugName"].ToString();
                    theHeading.ID = "lbl" + dr["DrugName"].ToString();
                }
                theHeading.Font.Bold = true;
                thePnl.Controls.Add(theHeading);
            }
            else
            {
                ///////// Space//////
                Label lblStSp = new Label();
                lblStSp.Width = 5;
                lblStSp.ID = "stSpace_" + theGenericId.ToString();
                lblStSp.Text = "";
                thePnl.Controls.Add(lblStSp);
                //////////////////////

                //

                Label theDrugNm = new Label();
                theDrugNm.ID = "drgNm" + theGenericId;
                if (Convert.ToInt32(dr["GenericId"]) > 0)
                {
                    theDrugNm.Text = dr["GenericName"].ToString();
                }
                else
                {
                    theDrugNm.Text = dr["DrugName"].ToString();
                }
                theDrugNm.Width = 180;
                thePnl.Controls.Add(theDrugNm);

                Label theSpace = new Label();
                theSpace.ID = "lblSp1_" + theGenericId;
                theSpace.Width = 20;
                theSpace.Text = "";
                thePnl.Controls.Add(theSpace);

                CheckBox theSaveChk = new CheckBox();
                theSaveChk.ID = "theSaveChk" + theGenericId;
                theSaveChk.Width = 10;
                string strChkid = "ctl00_IQCareContentPlaceHolder_theSaveChk" + theGenericId;
                string strDropID1 = "ctl00_IQCareContentPlaceHolder_drgStrength" + theGenericId;
                string strDropID2 = "ctl00_IQCareContentPlaceHolder_drgFrequency" + theGenericId;
                string strText1 = "ctl00_IQCareContentPlaceHolder_drgDuration" + theGenericId;
                string strText2 = "ctl00_IQCareContentPlaceHolder_drgQtyDispensed" + theGenericId;
                string strText3 = "ctl00_IQCareContentPlaceHolder_drgQtyPrescribed" + theGenericId;
                theSaveChk.Attributes.Add("onclick", "fnDissableControl('" + strChkid + "','" + strDropID1 + "','" + strDropID2 + "','" + strText1 + "','" + strText2 + "','" + strText3 + "')");
                theSaveChk.Text = "";
                thePnl.Controls.Add(theSaveChk);

                Label theSpacechk = new Label();
                theSpacechk.ID = "lblSp1chk_" + theGenericId;
                theSpacechk.Width = 20;
                theSpacechk.Text = "";
                thePnl.Controls.Add(theSpacechk);

                BindFunctions theBindMgr = new BindFunctions();
                DropDownList theDrugStrength = new DropDownList();
                theDrugStrength.ID = "drgStrength" + theGenericId;
                theDrugStrength.Width = 80;

                #region "BindCombo"
                DataTable theDTS = new DataTable();

                DataView theDVStrength = new DataView(theCntlDS.Tables[1]);
                if (Convert.ToInt32(dr["GenericId"]) > 0)
                {
                    theDVStrength.RowFilter = "GenericId = " + theGenericId;
                }
                else
                {

                    theDTS = (DataTable)Session["FixDrugStrength"];
                    theDVStrength = new DataView(theDTS);
                    theDVStrength.RowFilter = "Drug_pk = " + Convert.ToInt32(dr["DrugId"]) + " and StrengthId>0";


                }
                DataTable theDTStrength = new DataTable();
                if (theDVStrength.Count > 0)
                {
                    IQCareUtils theUtils = new IQCareUtils();
                    theDTStrength = theUtils.CreateTableFromDataView(theDVStrength);
                    theBindMgr.BindCombo(theDrugStrength, theDTStrength, "StrengthName", "StrengthId");
                }

                #endregion



                thePnl.Controls.Add(theDrugStrength);
                //////////////Space////////////////////////
                Label theSpace1 = new Label();
                theSpace1.ID = "lblSp2_" + theGenericId;
                theSpace1.Width = 20;
                theSpace1.Text = "";
                thePnl.Controls.Add(theSpace1);
                //////////////////////////////////////////

                DropDownList theDrugFrequency = new DropDownList();
                theDrugFrequency.ID = "drgFrequency" + theGenericId;
                theDrugFrequency.Width = 80;
                #region "BindCombo"

                DataTable theDTF = new DataTable();
                DataView theDVFrequency = new DataView(theCntlDS.Tables[2]);
                if (Convert.ToInt32(dr["GenericId"]) > 0)
                {
                    //rupesh
                    //theDVFrequency.RowFilter = "DrugId = 0 and GenericId = " + theGenericId;
                    theDVFrequency.RowFilter = "GenericId = " + theGenericId;
                }
                else
                {

                    theDTF = (DataTable)Session["FixDrugFreq"];
                    theDVFrequency = new DataView(theDTF);
                    theDVFrequency.RowFilter = "Drug_pk = " + Convert.ToInt32(dr["DrugId"]) + " and FrequencyId>0";

                }
                DataTable theDTFrequency = new DataTable();
                if (theDVFrequency.Count > 0)
                {
                    IQCareUtils theUtils = new IQCareUtils();
                    theDTFrequency = theUtils.CreateTableFromDataView(theDVFrequency);
                    theBindMgr.BindCombo(theDrugFrequency, theDTFrequency, "FrequencyName", "FrequencyId");
                }
                #endregion
                thePnl.Controls.Add(theDrugFrequency);

                //////////////Space////////////////////////
                Label theSpace2 = new Label();
                theSpace2.ID = "lblSp3_" + theGenericId;
                theSpace2.Width = 15;
                theSpace2.Text = "";
                thePnl.Controls.Add(theSpace2);
                //////////////////////////////////////////

                TextBox theDuration = new TextBox();
                theDuration.ID = "drgDuration" + theGenericId;
                theDuration.Width = 120;
                theDuration.Load += new EventHandler(Control_Load);
                thePnl.Controls.Add(theDuration);

                //////////////Space////////////////////////
                Label theSpace3 = new Label();
                theSpace3.ID = "lblSp4_" + theGenericId;
                theSpace3.Width = 20;
                theSpace3.Text = "";
                thePnl.Controls.Add(theSpace3);
                //////////////////////////////////////////

                TextBox theQtyPrescribed = new TextBox();
                theQtyPrescribed.ID = "drgQtyPrescribed" + theGenericId;
                theQtyPrescribed.Width = 120;
                theQtyPrescribed.Load += new EventHandler(Control_Load);
                thePnl.Controls.Add(theQtyPrescribed);
                //////////////Space////////////////////////
                Label theSpace4 = new Label();
                theSpace4.ID = "lblSp5_" + theGenericId;
                theSpace4.Width = 20;
                theSpace4.Text = "";
                thePnl.Controls.Add(theSpace4);
                //////////////////////////////////////////

                TextBox theQtyDispensed = new TextBox();
                theQtyDispensed.ID = "drgQtyDispensed" + theGenericId;
                theQtyDispensed.Width = 120;
                #region "13-Jun-07 -4"
                theQtyDispensed.Load += new EventHandler(Control_Load);
                thePnl.Controls.Add(theQtyDispensed);
                //theQtyDispensed.Attributes.Add("onkeyup", "chkNumeric('" + theQtyDispensed.ClientID + "')"); // rupesh
                #endregion
                //////////////Space////////////////////////
                Label theSpace5 = new Label();
                theSpace5.ID = "lblSp6_" + theGenericId;
                theSpace5.Width = 20;
                theSpace5.Text = "";
                thePnl.Controls.Add(theSpace5);
                //////////////////////////////////////////
                CheckBox theFinChk = new CheckBox();
                theFinChk.ID = "FinChk" + theGenericId;
                theFinChk.Width = 10;
                theFinChk.Text = "";
                theFinChk.Visible = false;
                thePnl.Controls.Add(theFinChk);
                /////////////
                Label theSpace6 = new Label();
                theSpace6.ID = "lblSp7_" + theGenericId;
                theSpace6.Width = 20;
                theSpace6.Text = "";
                thePnl.Controls.Add(theSpace6);


                i = i + 1;

            }
            if (theGenericId == 10006 || theGenericId == 486 || theGenericId == 85 || theGenericId == 150)
            {

                PnlOIARV.Controls.Add(thePnl);
            }
            else
            {
                //PnlDrug.Controls.Add(thePnl);
            }

        }

    }
    void Control_Load(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;
        tbox.MaxLength = 3;
        #region "13-Jun-07 - 1"
        //tbox.Attributes.Add("onkeyup", "chkNumeric('" + tbox.ClientID + "')");
        tbox.Attributes.Add("onkeyup", "chkNumber('" + tbox.ClientID + "')"); // rupesh
        #endregion
    }
    private void BindControls()
    {
        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        BindFunctions theBindMgr = new BindFunctions();
        DataTable theDT = new DataTable();
        IQCareUtils theUtils = new IQCareUtils();
      



        DataSet theDSXML = new DataSet();
        theDSXML.ReadXml(Server.MapPath("..\\XMLFiles\\AllMasters.con"));

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            if (theDSXML.Tables["Mst_Employee"] != null)
            {
                theDV = new DataView(theDSXML.Tables["Mst_Employee"]);
                theDV.RowFilter = "DeleteFlag=0";
                if (theDV.Table != null)
                {
                    theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                    if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0)
                    {
                        theDV = new DataView(theDT);
                        theDV.RowFilter = "EmployeeId =" + Session["AppUserEmployeeId"].ToString();
                        if (theDV.Count > 0)
                            theDT = theUtils.CreateTableFromDataView(theDV);
                    }
                    theBindMgr.BindCombo(ddlPharmOrderedbyName, theDT, "EmployeeName", "EmployeeId");
                    theBindMgr.BindCombo(ddlPharmReportedbyName, theDT, "EmployeeName", "EmployeeId");
                    theBindMgr.BindCombo(ddlPharmSignature, theDT, "EmployeeName", "EmployeeId");
                    theDV.Dispose();
                    theDT.Clear();
                }
                //DataTable theDT = theDV.ToTable();
                //theBindMgr.BindCombo(ddlPharmOrderedbyName, theDT, "EmployeeName", "EmployeeId");
                //theBindMgr.BindCombo(ddlPharmReportedbyName, theDT, "EmployeeName", "EmployeeId");
                //theBindMgr.BindCombo(ddlPharmSignature, theDT, "EmployeeName", "EmployeeId");
            }


        }
        else
        {
            if (theDSXML.Tables["Mst_Employee"] != null)
            {
                theBindMgr.BindCombo(ddlPharmOrderedbyName, theDSXML.Tables["Mst_Employee"], "EmployeeName", "EmployeeId");
                theBindMgr.BindCombo(ddlPharmReportedbyName, theDSXML.Tables["Mst_Employee"], "EmployeeName", "EmployeeId");
                theBindMgr.BindCombo(ddlPharmSignature, theDSXML.Tables["Mst_Employee"], "EmployeeName", "EmployeeId");
            }

        }
    }
    private void BindddlControls(DataSet theDS)
    {
        /*******/
        BindFunctions BindManager = new BindFunctions();
        IQCareUtils theUtils = new IQCareUtils();
        DataView theDVTreat = new DataView(theDS.Tables[9]);
        theDVTreat.RowFilter = "DeleteFlag=0";
        DataTable theDT = (DataTable)theUtils.CreateTableFromDataView(theDVTreat);
        ddlTreatment.DataSource = theDT;
        ddlTreatment.DataTextField = theDT.Columns[1].ColumnName;
        ddlTreatment.DataValueField = theDT.Columns[0].ColumnName;
        ddlTreatment.DataBind();
        theDVTreat.Dispose();
        theDT.Clear();


        DataView theDVProvider = new DataView(theDS.Tables[12]);
        theDT = (DataTable)theUtils.CreateTableFromDataView(theDVProvider);
        ddlProvider.DataSource = theDT;
        ddlProvider.DataTextField = theDT.Columns[1].ColumnName;
        ddlProvider.DataValueField = theDT.Columns[0].ColumnName;
        ddlProvider.DataBind();
        ddlProvider.Dispose();
        theDT.Clear();
        ddlProvider.SelectedIndex = 0;

    }

    protected void BtnAddARV_Click(object sender, EventArgs e)
    {
        string theScript;

        Application.Add("MasterData", Session["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)Session["AddARV"]);
        theScript = "<script language='javascript' id='DrgPopup'>\n";
        theScript += "window.open('frmDrugSelector.aspx?DrugType=37','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "</script>\n";
        Page.RegisterStartupScript("DrgPopup", theScript);
        if (btnCounsellorSignature.Checked == true)
        {
            string script = "<script language = 'javascript' defer ='defer' id = 'showsignature'>\n";
            script += "document.getElementById('" + btnCounsellorSignature.ClientID + "').click();\n";
            script += "</script>\n";
            RegisterStartupScript("showsignature", script);
        }
    }
    protected void OtherMedication_Click(object sender, EventArgs e)
    {
        string theScript;

        Application.Add("MasterData", (DataTable)Session["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)Session["OtherDrugs"]);

        theScript = "<script language='javascript' id='DrgPopup'>\n";
        theScript += "window.open('frmDrugSelector.aspx?DrugType=0','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "</script>\n";
        Page.RegisterStartupScript("DrgPopup", theScript);
        if (btnCounsellorSignature.Checked == true)
        {
            string script = "<script language = 'javascript' defer ='defer' id = 'showsignature'>\n";
            script += "document.getElementById('" + btnCounsellorSignature.ClientID + "').click();\n";
            script += "</script>\n";
            RegisterStartupScript("showsignature", script);
        }

    }
    private DataTable CreateTable()
    {
        DataTable theDT = new DataTable();
        theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("GenericId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("RegimenId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Dose", System.Type.GetType("System.Decimal"));
        theDT.Columns.Add("UnitId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("FrequencyId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Duration", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("QtyPrescribed", System.Type.GetType("System.Decimal"));
        theDT.Columns.Add("QtyDispensed", System.Type.GetType("System.Decimal"));
        theDT.Columns.Add("Financed", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Prophylaxis", System.Type.GetType("System.Int32"));

        return theDT;

    }
    private DataTable MakeDrugTableRegimen(Control theContainer)
    {
        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");

        DataTable theDT = new DataTable();

        if (Session["Data"] == null)
        {
            theDT = CreateTable();
        }
        else
        {
            theDT = (DataTable)Session["Data"];
        }

        #region "Variables"
        decimal Dose = 0;
        int UnitId = 0;
        int theStrengthId = 0;
        int theRegimenID = 0;
        int theFrequencyId = 0;
        Decimal theDuration = 0;
        Decimal theQtyPrescribed = 0;
        Decimal theQtyDispensed = 0;
        Decimal theQtyPrescribed1 = 0;
        Decimal theQtyDispensed1 = 0;
        int theFinanced = 99;
        int theProphylaxis = 0;
        DataSet theGenericDS = new DataSet();

        #endregion
        if (theContainer.ID == "PnlDrug")
        {
            theRegimenID = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
            if (theRegimenID.ToString() != "0")
            {
                theGenericDS = DrugManager.GetGenericID_CTC_Detail(Convert.ToInt32(theRegimenID));
            }

            if (theGenericDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in theGenericDS.Tables[0].Rows)
                {
                    theRegimenID = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
                    if (ddlARVCombRegFrqARV.SelectedItem.Value.ToString() != "0")
                    {
                        theFrequencyId = Convert.ToInt32(ddlARVCombRegFrqARV.SelectedItem.Value);
                    }
                    if (txtARVCombRegQtyPres.Text != "")
                    {
                        theQtyPrescribed = Convert.ToDecimal(txtARVCombRegQtyPres.Text);
                    }
                    if (txtARVCombRegDuraton.Text != "")
                    {
                        theDuration = Convert.ToDecimal(txtARVCombRegDuraton.Text);
                    }
                    if (txtARVCombRegQtyDesc.Text != "")
                    {
                        theQtyDispensed = Convert.ToDecimal(txtARVCombRegQtyDesc.Text);
                    }
                    if (chkProphylaxis.Checked == true)
                    {
                        theProphylaxis = 1;
                    }
                    if (ddlTreatment.SelectedItem.Value.ToString() == "223")
                    {
                        if (theRegimenID != 0 && theDuration != 0 && theProphylaxis!=0)
                        {

                            DataRow theRow;
                            theRow = theDT.NewRow();
                            theRow["GenericId"] = Convert.ToInt32(dr["GenericID"]);
                            theRow["DrugId"] = 0;
                            theRow["RegimenId"] = theRegimenID;
                            theRow["Dose"] = 0;
                            theRow["UnitId"] = 0;
                            theRow["StrengthId"] = theStrengthId;
                            theRow["FrequencyId"] = theFrequencyId;
                            theRow["Duration"] = theDuration;
                            theRow["QtyPrescribed"] = theQtyPrescribed;
                            theRow["QtyDispensed"] = theQtyDispensed;
                            theRow["Financed"] = 1;
                            theRow["Prophylaxis"] = theProphylaxis;
                            theDT.Rows.Add(theRow);
                            #region "Reset Variables
                            Dose = 0;
                            UnitId = 0;
                            theRegimenID = 0;
                            theStrengthId = 0;
                            theFrequencyId = 0;
                            theDuration = 0;
                            theQtyPrescribed = 0;
                            theQtyDispensed = 0;
                            theProphylaxis = 0;
                            theFinanced = 99;
                            #endregion
                        }

                    }
                    else
                    {
                        if (theRegimenID != 0 && theDuration != 0)
                        {

                            DataRow theRow;
                            theRow = theDT.NewRow();
                            theRow["GenericId"] = Convert.ToInt32(dr["GenericID"]);
                            theRow["DrugId"] = 0;
                            theRow["RegimenId"] = theRegimenID;
                            theRow["Dose"] = 0;
                            theRow["UnitId"] = 0;
                            theRow["StrengthId"] = theStrengthId;
                            theRow["FrequencyId"] = theFrequencyId;
                            theRow["Duration"] = theDuration;
                            theRow["QtyPrescribed"] = theQtyPrescribed;
                            theRow["QtyDispensed"] = theQtyDispensed;
                            theRow["Financed"] = 1;
                            theRow["Prophylaxis"] = theProphylaxis;
                            theDT.Rows.Add(theRow);
                            #region "Reset Variables
                            Dose = 0;
                            UnitId = 0;
                            theRegimenID = 0;
                            theStrengthId = 0;
                            theFrequencyId = 0;
                            theDuration = 0;
                            theQtyPrescribed = 0;
                            theQtyDispensed = 0;
                            theProphylaxis = 0;
                            theFinanced = 99;
                            #endregion
                        }

                    }
                    
                }
            }
        }
        return theDT;
    }
    private Boolean CheckOtherARV_Duration(Control theContainer)
    {

        int c = 0;
        int theStrengthId = 0;
        int theFrequencyId = 0;
        if (theContainer.ID == "PnlAddARV")
        {
            DataTable theADDARVDrug = (DataTable)Session["AddARV"];
            if (theADDARVDrug == null)
                return true;
            foreach (DataRow theDR in theADDARVDrug.Rows)
            {

                foreach (Control y in theContainer.Controls)
                {
                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                    {
                        foreach (Control x in y.Controls)
                        {
                            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                            {
                                CheckOtherARV_Duration(x);
                            }
                            else
                            {
                                //
                                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                                {
                                    c = x.ID.Length;
                                    if (x.ID.StartsWith("drgStrength"))
                                    {
                                        if (x.ID.Substring(11, c - 11) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgStrength"))
                                        {
                                            theStrengthId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                        }
                                    }
                                    if (x.ID.StartsWith("drgFrequency"))
                                    {
                                        if (x.ID.Substring(12, c - 12) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgFrequency"))
                                        {
                                            theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                        }
                                    }
                                }
                                //
                                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                {

                                    if (x.ID.StartsWith("drgDuration"))
                                    {
                                        c = x.ID.Length;
                                        if (x.ID.Substring(11, c - 11) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgDuration"))
                                        {
                                            if (theStrengthId != 0 || theFrequencyId != 0)
                                            {
                                                if (((TextBox)x).Text == "")
                                                {
                                                    //IQCareMsgBox.Show("CannotaddARVStatus", this);
                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }
    private DataTable MakeDrugTable(Control theContainer)
    {
        int c = 0;//c=total length of id
        DataTable theDT = new DataTable();

        if (Session["Data"] == null)
        {
            theDT = CreateTable();
        }
        else
        {
            theDT = (DataTable)Session["Data"];
        }

        #region "Variables"
        decimal Dose = 0;
        int UnitId = 0;
        int theStrengthId = 0;
        int theRegimenID = 0;
        int theFrequencyId = 0;
        Decimal theDuration = 0;
        Decimal theQtyPrescribed = 0;
        Decimal theQtyDispensed = 0;
        Decimal theQtyPrescribed1 = 0;
        Decimal theQtyDispensed1 = 0;
        int theFinanced = 99;
        int theSaveCheck = 0;
        int theProphylaxis = 0;
        if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
        {
            theProphylaxis = 999;
        }

        #endregion


        if (theContainer.ID == "PnlOIARV") //--ARV
        {
            #region "ARV and OI"
            //pnl 1 - id=PnlDrug - no btn  AND pnl 3 - id =PnlOIARV - no btn
            DataTable theARVDrug = (DataTable)Session["SelectedDrug"];
            #region "18-Jun-07 - 1"
            int TotColFilled = 0; // rupesh
            #endregion
            foreach (DataRow theDR in theARVDrug.Rows)
            {
                DataRow theRow;
                foreach (Control y in theContainer.Controls)
                {
                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                    {
                        //
                        foreach (Control x in y.Controls)
                        {
                            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                            {
                                MakeDrugTable(x);
                            }
                            else
                            {


                                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                                {
                                    if (Convert.ToInt32(theDR["GenericId"]) > 0)
                                    {
                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgStrength"))
                                        {
                                            theStrengthId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            #region "18-Jun-07 - 2"
                                            if (theStrengthId != 0)
                                                TotColFilled++;
                                            #endregion
                                        }
                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgFrequency"))
                                        {
                                            theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            #region "18-Jun-07 - 3"
                                            if (theFrequencyId != 0)
                                                TotColFilled++;
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgStrength"))
                                        {
                                            theStrengthId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            #region "18-Jun-07 - 4"
                                            if (theStrengthId != 0)
                                                TotColFilled++;
                                            #endregion
                                        }
                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgFrequency"))
                                        {
                                            theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            #region "18-Jun-07 - 5"
                                            if (theFrequencyId != 0)
                                                TotColFilled++;
                                            #endregion
                                        }

                                    }
                                }
                                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                {
                                    if (Convert.ToInt32(theDR["GenericId"]) > 0)
                                    {
                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgDuration"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theDuration = Convert.ToDecimal(((TextBox)x).Text);
                                                #region "18-Jun-07 - 6"
                                                if (theDuration != 0)
                                                    TotColFilled++;
                                                #endregion
                                            }
                                        }
                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyPrescribed = Convert.ToDecimal(((TextBox)x).Text);
                                                #region "18-Jun-07 - 7"
                                                if (theQtyPrescribed != 0)
                                                    TotColFilled++;
                                                #endregion
                                            }
                                        }
                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
                                                #region "18-Jun-07 - 8"
                                                if (theQtyDispensed != 0)
                                                    TotColFilled++;
                                                #endregion
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgDuration"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theDuration = Convert.ToDecimal(((TextBox)x).Text);
                                                #region "18-Jun-07 - 9"
                                                if (theDuration != 0)
                                                    TotColFilled++;
                                                #endregion
                                            }
                                        }
                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyPrescribed = Convert.ToDecimal(((TextBox)x).Text);
                                                #region "18-Jun-07 - 10"
                                                if (theQtyPrescribed != 0)
                                                    TotColFilled++;
                                                #endregion
                                            }
                                        }
                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
                                                #region "18-Jun-07 - 11"
                                                if (theQtyDispensed != 0)
                                                    TotColFilled++;
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                //add by santosh
                                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                {
                                    if (x.ID.StartsWith("FinChk"))
                                    {
                                        c = x.ID.Length;
                                        //if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("FinChk"))
                                        if (x.ID.Substring(6, c - 6) == theDR["DrugId"].ToString() && x.ID.StartsWith("FinChk"))
                                        {
                                            if (((CheckBox)x).Checked == true)
                                                theFinanced = 1;
                                            else
                                                theFinanced = 0;
                                        }
                                    }
                                    //add by santosh
                                    if (x.ID.StartsWith("theSaveChk"))
                                    {
                                        c = x.ID.Length;
                                        //if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("FinChk"))
                                        if (x.ID.Substring(10, c - 10) == theDR["DrugId"].ToString() && x.ID.StartsWith("theSaveChk"))
                                        {
                                            if (((CheckBox)x).Checked == true)
                                                theSaveCheck = 1;
                                            else
                                                theSaveCheck = 0;
                                        }
                                    }

                                }
                                //end
                            }
                            if (theContainer.ID == "PnlDrug")
                            {
                                theRegimenID = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
                                theFrequencyId = Convert.ToInt32(ddlARVCombRegFrqARV.SelectedItem.Value);
                                theQtyPrescribed = Convert.ToInt32(txtARVCombRegQtyPres.Text);
                                theDuration = Convert.ToInt32(txtARVCombRegDuraton.Text);
                                theQtyDispensed = Convert.ToInt32(txtARVCombRegQtyDesc.Text);
                                if (theRegimenID != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0)
                                {
                                    theRow = theDT.NewRow();
                                    if (Convert.ToInt32(theDR["GenericId"]) > 0)
                                    {
                                        theRow["DrugId"] = 0;
                                        theRow["GenericId"] = theDR["GenericId"];
                                    }
                                    else
                                    {
                                        theRow["DrugId"] = theDR["DrugId"];
                                        theRow["GenericId"] = 0;
                                    }
                                    theRow["RegimenId"] = theRegimenID;
                                    theRow["Dose"] = 0;
                                    theRow["UnitId"] = 0;
                                    theRow["StrengthId"] = theStrengthId;
                                    theRow["FrequencyId"] = theFrequencyId;
                                    theRow["Duration"] = theDuration;
                                    theRow["QtyPrescribed"] = theQtyPrescribed;
                                    theRow["QtyDispensed"] = theQtyDispensed;
                                    theRow["Financed"] = 1;
                                    theRow["Prophylaxis"] = 0;
                                    theDT.Rows.Add(theRow);
                                    #region "Reset Variables
                                    Dose = 0;
                                    UnitId = 0;
                                    theRegimenID = 0;
                                    theStrengthId = 0;
                                    theFrequencyId = 0;
                                    theDuration = 0;
                                    theQtyPrescribed = 0;
                                    theQtyDispensed = 0;
                                    theFinanced = 99;
                                    #endregion
                                }
                            }

                        }
                        #region "18-Jun-07 - 12"
                        if ((TotColFilled > 0 && TotColFilled < 5) && (theContainer.ID == "PnlDrug"))
                        {
                            theDT.Rows.Clear();
                            theRow = theDT.NewRow();
                            theRow[0] = 99999;
                            theDT.Rows.Add(theRow);
                            return theDT;
                        }
                        else
                            TotColFilled = 0;
                        #endregion
                    }
                }
                if (theContainer.ID == "PnlOIARV")
                {
                    int chkResult = theSaveCheck;
                    if (chkResult == 1)
                    {
                        theRow = theDT.NewRow();
                        theRow["DrugId"] = theDR["DrugId"];
                        theRow["GenericId"] = theDR["GenericId"];
                        theRow["Dose"] = 0;
                        theRow["UnitId"] = 0;
                        theRow["StrengthId"] = 0;
                        theRow["FrequencyId"] = 0;
                        theRow["Duration"] = 0;
                        theRow["QtyPrescribed"] = 0;
                        theRow["QtyDispensed"] = 0;
                        //santosh
                        theRow["Financed"] = 1;
                        theRow["Prophylaxis"] = 0;
                        theDT.Rows.Add(theRow);
                        #region "Reset Variables
                        Dose = 0;
                        UnitId = 0;
                        theStrengthId = 0;
                        theFrequencyId = 0;
                        theDuration = 0;
                        theQtyPrescribed = 0;
                        theQtyDispensed = 0;
                        theFinanced = 99;
                        theSaveCheck = 0;
                        #endregion
                    }
                    else if (theStrengthId != 0 || theFrequencyId != 0 || theDuration != 0 || theQtyPrescribed != 0 || theQtyDispensed != 0)
                    {
                        theRow = theDT.NewRow();
                        theRow["DrugId"] = theDR["DrugId"];
                        theRow["GenericId"] = theDR["GenericId"];
                        theRow["Dose"] = 0;
                        theRow["UnitId"] = 0;
                        theRow["StrengthId"] = theStrengthId;
                        theRow["FrequencyId"] = theFrequencyId;
                        theRow["Duration"] = theDuration;
                        theRow["QtyPrescribed"] = theQtyPrescribed;
                        theRow["QtyDispensed"] = theQtyDispensed;
                        theRow["Prophylaxis"] = 0;
                        //santosh
                        theRow["Financed"] = theFinanced;
                        theDT.Rows.Add(theRow);
                        #region "Reset Variables
                        Dose = 0;
                        UnitId = 0;
                        theStrengthId = 0;
                        theFrequencyId = 0;
                        theDuration = 0;
                        theQtyPrescribed = 0;
                        theQtyDispensed = 0;
                        theFinanced = 99;
                        theSaveCheck = 0;
                        #endregion
                    }
                }
            }
            #endregion
        }
        else if (theContainer.ID == "PnlAddARV")
        {
            #region "Additional ARV"
            int TotelColFilled = 0;
            //pnl 2 -id= PnlAddARV  btn-txt = "Other ARV Medications"
            DataTable theADDARVDrug = (DataTable)Session["AddARV"];
            if (theADDARVDrug == null)
                return theDT;
            foreach (DataRow theDR in theADDARVDrug.Rows)
            {
                DataRow theRow;
                foreach (Control y in theContainer.Controls)
                {
                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                    {
                        foreach (Control x in y.Controls)
                        {
                            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                            {
                                MakeDrugTable(x);
                            }
                            else
                            {
                                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                                {
                                    c = x.ID.Length;
                                    if (x.ID.StartsWith("drgStrength"))
                                    {
                                        if (x.ID.Substring(11, c - 11) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgStrength"))
                                        {
                                            theStrengthId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            TotelColFilled++;
                                        }
                                    }
                                    if (x.ID.StartsWith("drgFrequency"))
                                    {
                                        if (x.ID.Substring(12, c - 12) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgFrequency"))
                                        {
                                            theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            TotelColFilled++;
                                        }
                                    }
                                }
                                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                {

                                    if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
                                    //if (x.ID.StartsWith("drgQtyPrescribed"))
                                    {
                                        c = x.ID.Length;
                                        if (x.ID.Substring(16, c - 16) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgQtyPrescribed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyPrescribed = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                    }
                                    if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                    //if (x.ID.StartsWith("drgQtyDispensed"))
                                    {
                                        c = x.ID.Length;
                                        if (x.ID.Substring(15, c - 15) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgQtyDispensed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;

                                            }
                                        }
                                    }
                                    if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgDuration"))
                                    //if (x.ID.StartsWith("drgDuration"))
                                    {
                                        c = x.ID.Length;
                                        if (x.ID.Substring(11, c - 11) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgDuration"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theDuration = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                    }
                                }
                                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                {
                                    if (x.ID.StartsWith("FinChk"))
                                    {
                                        c = x.ID.Length;
                                        //if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("FinChk"))
                                        if (x.ID.Substring(6, c - 6) == theDR["DrugId"].ToString() && x.ID.StartsWith("FinChk"))
                                        {
                                            if (((CheckBox)x).Checked == true)
                                            {
                                                theFinanced = 1;
                                                TotelColFilled++;
                                            }
                                            else
                                            {
                                                theFinanced = 0;
                                            }
                                        }
                                       
                                    }
                                    if (x.ID.StartsWith("chkProphylaxis"))
                                    {
                                        c = x.ID.Length;
                                        if (x.ID.Substring(14, c - 14) == theDR["DrugId"].ToString() && x.ID.StartsWith("chkProphylaxis"))
                                        {

                                            if (((CheckBox)x).Checked == true)
                                            {
                                                theProphylaxis = 1;
                                                TotelColFilled++;
                                            }

                                        }
                                    }
                                }
                            }
                            //if(theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theFinanced != 99)
                            if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theFinanced != 99 && theProphylaxis != 999)
                            {
                                theRow = theDT.NewRow();
                                if (Convert.ToInt32(theDR["Generic"]) == 0)
                                {
                                    theRow["DrugId"] = theDR["DrugId"];
                                    theRow["GenericId"] = 0;
                                }
                                else
                                {
                                    theRow["DrugId"] = 0;
                                    theRow["GenericId"] = theDR["DrugId"];
                                }
                                theRow["Dose"] = 0;
                                theRow["UnitId"] = 0;
                                theRow["StrengthId"] = theStrengthId;
                                theRow["FrequencyId"] = theFrequencyId;
                                theRow["Duration"] = theDuration;
                                theRow["QtyPrescribed"] = theQtyPrescribed;
                                theRow["QtyDispensed"] = theQtyDispensed;
                                theRow["Financed"] = theFinanced;
                                theRow["Prophylaxis"] = theProphylaxis;
                                theDT.Rows.Add(theRow);
                                #region "Reset Variables
                                Dose = 0;
                                UnitId = 0;
                                theStrengthId = 0;
                                theFrequencyId = 0;
                                theDuration = 0;
                                theQtyPrescribed = 0;
                                theQtyDispensed = 0;
                                theFinanced = 99;

                                if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
                                {
                                    theProphylaxis = 999;
                                }
                                else
                                {
                                    theProphylaxis = 0;
                                }
                                #endregion
                            }
                            int total = TotelColFilled;
                            
                        }

                    }
                }
                if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
                {
                    if ((TotelColFilled > 0 && TotelColFilled < 6) && (theContainer.ID == "PnlAddARV"))
                    {
                        theDT.Rows.Clear();
                        theRow = theDT.NewRow();
                        theRow[0] = 99999;
                        theDT.Rows.Add(theRow);
                        return theDT;
                    }
                    else
                        TotelColFilled = 0;
                }
            }
            #endregion
        }
        else
        {
            #region "Additional Drugs"
            int DrugID = 0;
            //pnl4 - id = PnlOtherMedication btn-txt = "OI Treatment & Other Medications"
            DataTable theOtherDrug = (DataTable)Session["OtherDrugs"];
            if (theOtherDrug == null)
                return theDT;

            foreach (DataRow theDR in theOtherDrug.Rows)
            {
                DataRow theRow;
                foreach (Control y in theContainer.Controls)
                {
                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                    {
                        foreach (Control x in y.Controls)
                        {
                            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                            {
                                MakeDrugTable(x);
                            }
                            else
                            {

                                if (theDR["DrugId"].ToString().LastIndexOf("8888") > 0) //--- if '8888' is added at the end of id - drug
                                {
                                    DrugID = Convert.ToInt32(theDR["DrugId"].ToString().Substring(0, theDR["DrugId"].ToString().Length - 4));
                                }
                                else if (theDR["DrugId"].ToString().LastIndexOf("9999") > 0) //--- if '9999' is added at the end of id  - generic
                                {

                                    DrugID = Convert.ToInt32(theDR["DrugId"].ToString().Substring(0, theDR["DrugId"].ToString().Length - 4));
                                }
                                else
                                {
                                    DrugID = Convert.ToInt32(theDR["DrugId"]);
                                }
                                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                                {
                                    if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("theUnit"))
                                    {
                                        UnitId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                    }
                                    if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgFrequency"))
                                    {
                                        theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                    }
                                }
                                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                {
                                    if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("theDose"))
                                    {
                                        if (((TextBox)x).Text != "")
                                        {
                                            Dose = Convert.ToDecimal(((TextBox)x).Text);
                                        }
                                    }

                                    if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgDuration"))
                                    {
                                        if (((TextBox)x).Text != "")
                                        {
                                            theDuration = Convert.ToDecimal(((TextBox)x).Text);
                                        }
                                    }
                                    if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
                                    {
                                        if (((TextBox)x).Text != "")
                                        {
                                            theQtyPrescribed1 = Convert.ToDecimal(((TextBox)x).Text);
                                        }
                                    }
                                    if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                    {
                                        if (((TextBox)x).Text != "")
                                        {
                                            theQtyDispensed1 = Convert.ToDecimal(((TextBox)x).Text);
                                        }
                                    }
                                }
                                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                {
                                    if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("FinChk"))
                                    {
                                        if (((CheckBox)x).Checked == true)
                                            theFinanced = 1;
                                        else
                                            theFinanced = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                if (UnitId != 0 && theFrequencyId != 0 && Dose != 0 && theDuration != 0 && theQtyPrescribed1 != 0 && theQtyDispensed1 != 0 && theFinanced != 99)
                {
                    theRow = theDT.NewRow();
                    if (Convert.ToInt32(theDR["Generic"]) == 0)
                    {
                        theRow["DrugId"] = DrugID;
                        theRow["GenericId"] = 0;
                    }
                    else
                    {
                        theRow["DrugId"] = 0;
                        theRow["GenericId"] = DrugID;
                    }
                    theRow["Dose"] = Dose;
                    theRow["UnitId"] = UnitId;
                    theRow["StrengthId"] = 0;
                    theRow["FrequencyId"] = theFrequencyId;
                    theRow["Duration"] = theDuration;
                    theRow["QtyPrescribed"] = theQtyPrescribed1;
                    theRow["QtyDispensed"] = theQtyDispensed1;
                    theRow["Financed"] = theFinanced;
                    theRow["Prophylaxis"] = 0;
                    theDT.Rows.Add(theRow);
                    #region "Reset Variables
                    Dose = 0;
                    UnitId = 0;
                    theStrengthId = 0;
                    theFrequencyId = 0;
                    theDuration = 0;
                    theQtyPrescribed = 0;
                    theQtyDispensed = 0;
                    theFinanced = 99;
                    #endregion
                }
            }
            #endregion
        }
        return theDT;
    }
    private Boolean TransferValidation(int PId)
    {
        IPatientTransfer IPTransferMgr;
        IPTransferMgr = (IPatientTransfer)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientTransfer, BusinessProcess.Clinical");
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            DataSet DS = IPTransferMgr.GetLatestTransferDate(PId, 0);
            if (DS.Tables[0].Rows[0]["NotExistTransferDate"].ToString() != "0")
            {
                if (Convert.ToDateTime(txtpharmOrderedbyDate.Value) < Convert.ToDateTime(DS.Tables[0].Rows[0]["TransferDate"]))
                {
                    IQCareMsgBox.Show("TransferDate_4", this);
                    txtpharmOrderedbyDate.Focus();
                    return false;
                }
            }
        }
        else if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
        {
            int visitPK = Convert.ToInt32(Request.QueryString["visitid"]);
            DataSet DS = IPTransferMgr.GetLatestTransferDate(PId, visitPK);
            if (DS.Tables[0].Rows[0]["NotExistTransferDate"].ToString() != "0")
            {

                if (DS.Tables[1].Rows[0]["PrevDate"].ToString() == "0" && DS.Tables[2].Rows[0]["LaterDate"].ToString() != "0")
                {
                    if (Convert.ToDateTime(txtpharmOrderedbyDate.Value) > Convert.ToDateTime(DS.Tables[2].Rows[0]["LaterDate"]))
                    {
                        IQCareMsgBox.Show("TransferDate_5", this);
                        txtpharmOrderedbyDate.Focus();
                        return false;
                    }
                }
                else if (DS.Tables[1].Rows[0]["PrevDate"].ToString() != "0" && DS.Tables[2].Rows[0]["LaterDate"].ToString() != "0")
                {
                    if (Convert.ToDateTime(txtpharmOrderedbyDate.Value) < Convert.ToDateTime(DS.Tables[1].Rows[0]["PrevDate"]) || Convert.ToDateTime(txtpharmOrderedbyDate.Value) > Convert.ToDateTime(DS.Tables[2].Rows[0]["LaterDate"]))
                    {
                        IQCareMsgBox.Show("TransferDate_6", this);
                        txtpharmOrderedbyDate.Focus();
                        return false;
                    }
                }
                else if (DS.Tables[1].Rows[0]["PrevDate"].ToString() != "0" && DS.Tables[2].Rows[0]["LaterDate"].ToString() == "0")
                {
                    if (Convert.ToDateTime(txtpharmOrderedbyDate.Value) < Convert.ToDateTime(DS.Tables[1].Rows[0]["PrevDate"]))
                    {
                        IQCareMsgBox.Show("TransferDate_7", this);
                        txtpharmOrderedbyDate.Focus();
                        return false;
                    }
                }
            }

        }
        return true;
    }
    protected void ClearObjects()
    {
        Session.Remove("EnrolmentDate");
        Session.Remove("Age");
        Session.Remove("MasterData");
        Session.Remove("OrigOrdDate");
        Session.Remove("PharmacyId");
        Session.Remove("PatientId");
        Session.Remove("SelectedDrug");
        Session.Remove("UserID");
        Session.Remove("ControlCreated");
        Session.Remove("CustomFieldsData");
        Session.Remove("CustomFieldsMulti");
        Session.Remove("OrigOrdDate");
        Session.Remove("PtnID");
        Session.Remove("LocationId");
        Session.Remove("Status");
        Session.Remove("MasterDrugTable");
        Session.Remove("AddARV");
        Session.Remove("OtherDrugs");
        Session.Remove("Data");

        Session.Remove("FixDrugStrength");
        Session.Remove("FixDrugFreq");
        Session.Remove("PharmacyId");



    }
    private void DeleteForm()
    {
        int theResultRow, OrderNo;
        string FormName;
        OrderNo = Convert.ToInt32(Session["PatientVisitId"]);
        FormName = "Pharmacy";

        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");

        theResultRow = (int)DrugManager.DeleteDrugForms(FormName, OrderNo, Convert.ToInt32(Session["PatientId"].ToString()), Convert.ToInt32(Session["UserID"]));

        if (theResultRow == 0)
        {
            IQCareMsgBox.Show("RemoveFormError", this);
            return;
        }
        else
        {
            string theUrl;
            theUrl = string.Format("{0}?PatientId={1}", "../ClinicalForms/frmPatient_Home.aspx", Session["PatientId"].ToString());
            Response.Redirect(theUrl);
        }

    }
    private Boolean FieldValidation()
    {
        //int PatientID = Convert.ToInt32(Session["PatientId"]);
        //if (TransferValidation(PatientID) == false)
        //{
        //    return false;
        //}


        IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
        theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
        IDrug PManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        IQCareUtils theUtils = new IQCareUtils();
        //if (ddlARVCombReg.SelectedIndex == 0)
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "ARV Combination Regimen";
        //    IQCareMsgBox.Show("BlankDropDown", theMsg, this);
        //    return false;
        //}

        if (ddlARVCombReg.SelectedIndex != 0)
        {


            if (txtARVCombRegDuraton.Text == "")
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Duration";
                IQCareMsgBox.Show("BlankTextBox", theMsg, this);
                return false;
            }
        }
        if (CheckOtherARV_Duration(PnlAddARV) == false)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Duration";
            IQCareMsgBox.Show("BlankTextBox", theMsg, this);
            return false;
        }
        if (ddlPharmOrderedbyName.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Ordered By";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }
        else if (ddlPharmReportedbyName.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Dispensed By";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }

        if (btnCounsellorSignature.Checked == false)
        {
            if (btnPatientSignature.Checked == false)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Signature";
                IQCareMsgBox.Show("UncheckedButton", theMsg, this);
                return false;
            }
            // return true;
        }
        if (txtpharmOrderedbyDate.Value.Trim() == "")
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "OrderedByDate";
            IQCareMsgBox.Show("BlankTextBox", theMsg, this);
            return false;
        }
        if (txtpharmOrderedbyDate.Value.Trim() != "")
        {
            DateTime theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value.Trim()));

            if (Session["EnrolmentDate"] != null)
            {
                DateTime theEnrolmentDate = Convert.ToDateTime(Session["EnrolmentDate"].ToString());
                if (theEnrolmentDate > theVisitDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailOrderDate", this);
                    txtpharmOrderedbyDate.Focus();
                    return false;
                }
                else if (theVisitDate > theCurrentDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailOrderTDate", this);
                    txtpharmOrderedbyDate.Focus();
                    return false;

                }
            }
        }
        if (txtpharmReportedbyDate.Value.Trim() == "") // dispensed by date
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "DispensedByDate";
            IQCareMsgBox.Show("BlankTextBox", theMsg, this);
            return false;
        }
        if ((btnCounsellorSignature.Checked == true) && (ddlPharmSignature.SelectedIndex == 0))
        {
            IQCareMsgBox.Show("PharmacySelectAdCounselor", this);
            return false;
        }
        if (txtpharmReportedbyDate.Value.Trim() != "")
        {
            DateTime theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value.Trim()));

            if (Session["EnrolmentDate"] != null)
            {
                DateTime theEnrolmentDate = Convert.ToDateTime(Session["EnrolmentDate"].ToString());
                if (theEnrolmentDate > theVisitDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailDispensedDate", this);
                    txtpharmReportedbyDate.Focus();
                    return false;
                }
                else if (theVisitDate > theCurrentDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailDispensedTDate", this);
                    txtpharmReportedbyDate.Focus();
                    return false;

                }
            }
        }
        if ((txtpharmOrderedbyDate.Value.Trim() != "") && (txtpharmReportedbyDate.Value.Trim() != ""))
        {
            DateTime theOrdByDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value.Trim()));
            DateTime theDispByDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value.Trim()));
            if (theOrdByDate > theDispByDate)
            {
                IQCareMsgBox.Show("PharmacyOrderDispenseDate", this);
                txtpharmOrderedbyDate.Focus();
                return false;
            }
        }
        //if (Convert.ToDecimal(Session["Age"]) < 14)
        //{
        //    IQCareMsgBox.Show("PharmacyAdultDetailAge", this);
        //    return false;
        //}
        //---Non-ART already filled : starts-- 29Feb08//

        //DataTable theDT = ((DataSet)Session["MasterData"]).Tables[13];// not working for concurrent users

        //----------- for concurrent users----------
        //pr_Pharmacy_GetNonARTDate_Constella
        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
        DataTable theDT = DrugManager.GetNonARTDate(Convert.ToInt32(Session["PatientId"])).Tables[0];

        if ((txtpharmOrderedbyDate.Value.Trim() != "") && (theDT.Rows.Count > 0))
        {
            DateTime theOrdByDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value.Trim()));
            DateTime theNonARTDate;
            foreach (DataRow theDR in theDT.Rows)
            {
                theNonARTDate = Convert.ToDateTime(theDR["VisitDate"].ToString());
                if (theOrdByDate == theNonARTDate)
                {
                    IQCareMsgBox.Show("PharmacyOrderNonARTDate", this);
                    txtpharmOrderedbyDate.Focus();
                    return false;
                }
            }
        }
        //
        DataTable theADDARVDrug = (DataTable)Session["AddARV"];
        int ARVRowCount = 0;
        if (theADDARVDrug != null)
        {
            ARVRowCount = theADDARVDrug.Rows.Count;
        }
        DataSet objDs = new DataSet();
        int PID = Convert.ToInt32(Session["PatientId"]);
        objDs = PManager.GetARVStatus(PID, Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value)));

        if (ddlARVCombReg.SelectedIndex != 0 || ARVRowCount > 0)
        {

            if (objDs.Tables[0].Rows.Count > 0)
            {
                string ARVStatus = string.Empty;
                for (int i = 0; i < objDs.Tables[0].Rows.Count; i++)
                {
                    ARVStatus = objDs.Tables[0].Rows[i]["ARVStatus"].ToString();
                    if (ddlTreatment.SelectedItem.Text != "PMTCT" && ddlTreatment.SelectedItem.Text != "PEP" && ddlTreatment.SelectedItem.Text != "Palliative Care")
                    {
                        if (ARVStatus == "1")
                        {
                            IQCareMsgBox.Show("CannotaddARVStatus", this);
                            return false;

                        }
                    }
                }
            }
        }
        if (objDs != null)
        {
            if (objDs.Tables[1].Rows.Count > 0)
            {
                DateTime VistiDate = Convert.ToDateTime(objDs.Tables[1].Rows[0]["VisitDate"].ToString());
                DateTime theOrdByDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value.Trim()));
                if (objDs.Tables[1].Rows[0]["VisitDate"].ToString() != "")
                {
                    if (VistiDate > theOrdByDate)
                    {
                        IQCareMsgBox.Show("PatientRecordExits", this);
                        //Page.Controls.Add(new LiteralControl("<script language='javascript'> window.alert('Form can not be saved as Patient record form exists after this date')</script>"));
                        return false;
                    }
                }
            }
        }




        //---Non-ART already filled : ends-- 29Feb08 //

        int PtnID = Convert.ToInt32(Session["PatientId"]);
        IDrug PharmacyManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        //pr_Pharmacy_AgeValidate_Constella
        DataSet dsExist = PharmacyManager.GetExistPharmacyForm(PtnID, Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)));
        if (dsExist != null && dsExist.Tables[0].Rows.Count > 0)
        {
            if ((Convert.ToInt32(Session["PatientVisitId"]) != 0) && (Convert.ToInt32(dsExist.Tables[0].Rows[0][0]) == 0))
            {
                if (Convert.ToDateTime(Session["OrigOrdDate"]) != Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)))
                {
                    IQCareMsgBox.Show("PharmacyDetailExists", this);
                    return false;
                }
            }
            int PatientID = Convert.ToInt32(Session["PatientId"]);
            if (TransferValidation(PatientID) == false)
            {
                return false;
            }
            if ((Convert.ToInt32(Session["PatientVisitId"]) == 0) && (Session["PharmacyId"] == null))// rupesh - for cancel btn
            {
                if (Convert.ToInt32(dsExist.Tables[0].Rows[0][0]) == 0)
                {
                    IQCareMsgBox.Show("PharmacyDetailExists", this);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return true;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string strCustomField = string.Empty;
        if (Request.QueryString["name"] == "Delete")
        {
            //DeleteForm();
            //DeleteForm();
            //******* show the message to the user*******//
            string msgString;

            msgString = "Are you sure you want to delete Adult Pharmacy form? \\n";

            string script = "<script language = 'javascript' defer ='defer' id = 'aftersavefunction'>\n";
            script += "var ans;\n";
            script += "ans=window.confirm('" + msgString + "');\n";
            script += "if (ans==true)\n";
            script += "{\n";
            script += "document.getElementById('" + btnOk.ClientID + "').click();\n";
            script += "}\n";
            script += "</script>\n";
            RegisterStartupScript("aftersavefunction", script);

            return;
        }
        else
        {
            if (FieldValidation() == false)
            {
                return;
            }
            Session.Remove("Data");
            if (ddlARVCombReg.SelectedItem.Value.ToString() != "0")
            {
                Session["Data"] = MakeDrugTableRegimen(PnlDrug);
                if (((DataTable)Session["Data"]).Rows.Count <=0)
                {
                    
                    Session.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                   
                }

            }

            Session["Data"] = MakeDrugTable(PnlAddARV);
            if (((DataTable)Session["Data"]).Rows.Count > 0)
            {
                if (((DataTable)Session["Data"]).Rows[0][0].ToString() == "99999")
                {
                    Session.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }
            }
            Session["Data"] = MakeDrugTable(PnlOIARV);
            DataTable dt = (DataTable)Session["Data"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Session["Paperless"].ToString() == "1")
                {
                    if (dt.Rows[i]["Strengthid"].ToString() == "0" || dt.Rows[i]["FrequencyId"].ToString() == "0" || dt.Rows[i]["Duration"].ToString() == "0" || dt.Rows[i]["QtyPrescribed"].ToString() == "0")
                    {
                        IQCareMsgBox.Show("PharmacyNoData", this);
                        return;
                    }
                }
                else
                {
                    if (dt.Rows[i]["Strengthid"].ToString() == "0" || dt.Rows[i]["FrequencyId"].ToString() == "0" || dt.Rows[i]["Duration"].ToString() == "0" || dt.Rows[i]["QtyPrescribed"].ToString() == "0" || dt.Rows[i]["QtyDispensed"].ToString() == "0")
                    {
                        IQCareMsgBox.Show("PharmacyNoData", this);
                        return;
                    }
                }
            }
            if (((DataTable)Session["Data"]).Rows.Count > 0)
            {
                if (((DataTable)Session["Data"]).Rows[0][0].ToString() == "99999")
                {
                    Session.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }
            }

            DataTable theDT = MakeDrugTable(PnlOtherMedication);
            if (theDT.Rows.Count < 1)
            {
                IQCareMsgBox.Show("PharmacyNoData", this);
                return;
            }
            IQCareUtils theUtils = new IQCareUtils();
            IDrug PharmacyManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");

            try
            {
                int PatientID, LocationId, UserId, EmpSignature = 0, Dispense = 0;
                PatientID = Convert.ToInt32(Session["PatientId"]);
                if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                    LocationId = Convert.ToInt32(Session["AppLocationId"]);
                else
                    LocationId = Convert.ToInt32(Session["ServiceLocationId"].ToString());
                UserId = Convert.ToInt32(Session["AppUserId"]);
                if (chkpharmDispensePU.Checked == true)
                {
                    Dispense = 1;
                }
                else
                {
                    Dispense = 0;
                }
                if (btnCounsellorSignature.Checked == true)
                    EmpSignature = Convert.ToInt32(ddlPharmSignature.SelectedValue);

                int theTreatmentID;
                theTreatmentID = Convert.ToInt32(ddlTreatment.SelectedValue);

                int theProviderID;
                theProviderID = Convert.ToInt32(ddlProvider.SelectedValue);


                if ((Convert.ToInt32(Session["PatientVisitId"]) == 0) && (Session["PharmacyId"] == null)) // Rupesh for Cancel Button
                {
                    CustomFieldClinical theCustomManager = new CustomFieldClinical();
                    DataTable theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Insert", ApplicationAccess.Pharmacy, (DataSet)ViewState["CustomFieldsDS"]);

                    int pharmaID = (int)PharmacyManager.SaveUpdateDrugOrder_CTC(PatientID, LocationId,0, Convert.ToInt32(ddlPharmOrderedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)), Convert.ToInt32(ddlPharmReportedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value)),
                        Dispense, EmpSignature, 1, 116, UserId, theDT, (DataSet)Session["MasterData"], theTreatmentID, theProviderID, theCustomDataDT,1);
                                  
                    Session["PharmacyId"] = pharmaID;
                    Session["PatientVisitId"] = pharmaID;
                    if (pharmaID == 0)
                    {
                        IQCareMsgBox.Show("PharmacyDetailExists", this);
                        return;
                    }
                    else
                    {
                        SaveCancel();

                    }
                }
                else 
                {
                    int PharmacyId = Convert.ToInt32(Session["PharmacyId"]);
                    if (PharmacyId == 0)
                    {
                        PharmacyId = Convert.ToInt32(Session["PatientVisitId"]);
                    }
                    CustomFieldClinical theCustomManager = new CustomFieldClinical();
                    DataTable theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Update", ApplicationAccess.Pharmacy, (DataSet)ViewState["CustomFieldsDS"]);

                    int RowsAffected = (int)PharmacyManager.SaveUpdateDrugOrder_CTC(PatientID,LocationId, PharmacyId, Convert.ToInt32(ddlPharmOrderedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)), Convert.ToInt32(ddlPharmReportedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value)),
                       Dispense, EmpSignature, 1, 307, UserId, theDT, (DataSet)Session["MasterData"], theTreatmentID, theProviderID, theCustomDataDT,2);
                   
                   
                    SaveCancel();

                }


                


            }
            catch (Exception err)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theMsg, this);
            }
            finally
            {
                PharmacyManager = null;
            }
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        int PatientId = 0;
        PatientId = Convert.ToInt32(Session["PtnID"]);
        string theUrl;
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            theUrl = string.Format("{0}?PatientId={1}&sts={2}", "../ClinicalForms/frmPatient_Home.aspx", PatientId, Session["Status"].ToString()); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        }
        else if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
        {
            theUrl = string.Format("{0}?PatientId={1}&sts={2}", "../ClinicalForms/frmPatient_History.aspx", PatientId, Session["Status"].ToString()); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        }
        else
        {
            theUrl = string.Format("{0}?PatientId={1}&sts={2}", "../ClinicalForms/frmClinical_DeleteForm.aspx", PatientId, Session["Status"].ToString());
        }
        Response.Redirect(theUrl);
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        DeleteForm();
        //int PatientId = 0;
        //PatientId = Convert.ToInt32(Session["PtnID"]);
        //string theUrl = "";
        //if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        //{
        //    theUrl = string.Format("{0}?PatientId={1}&sts={2}", "../ClinicalForms/frmPatient_Home.aspx", PatientId, Session["Status"].ToString()); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        //}
        //else if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
        //{
        //    theUrl = string.Format("{0}?PatientId={1}&sts={2}", "../ClinicalForms/frmPatient_History.aspx", PatientId, Session["Status"].ToString()); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        //}
        //else
        //{
        //    DeleteForm();
        //}
        //ClearObjects();
        //Response.Redirect(theUrl);
    }
    private void SaveCancel()
    {

        string strSession = Session["PtnID"].ToString();
        string strRequest = string.Empty;
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            strRequest = "Add";
        }
        else
        {
            strRequest = "Edit";
        }
        string strStatus = Session["Status"].ToString();
        string strPharmacyID = Session["PharmacyId"].ToString();
        string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
        script += "var ans;\n";
        script += "ans=window.confirm('Drug Order saved successfully. Do you want to close?');\n";
        script += "if (ans==true)\n";
        script += "{\n";
        script += "Redirect('" + strSession + "','" + strRequest + "','" + strStatus + "');\n";
        //script += "window.location.href('../ClinicalForms/frmPatient_Home.aspx?PatientId=" + Session["Ptn_Pk"].ToString() + "');\n";
        script += "}\n";
        //script += "else \n";
        //script += "{\n";
        //script += "window.location.href('../Pharmacy/frmPharmacy_CTC.aspx');\n";
        //script += "}\n";
        script += "</script>\n";
        RegisterStartupScript("confirm", script);
        //string theUrl = string.Format("../ClinicalForms/frmPatient_Home.aspx?PatientId=" + Session["Ptn_Pk"].ToString());
    }
    protected void ddlTreatment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTreatment.SelectedItem.Text == "PMTCT" || ddlTreatment.SelectedItem.Text == "PEP" || ddlTreatment.SelectedItem.Text == "Palliative Care")
        {
            PnlDrug.Enabled = true;
            BtnAddARV.Enabled = true;
        }
        else
        {
            DataSet objPatientStatus = new DataSet();
            IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
            objPatientStatus = DrugManager.GetPatientRecordformStatus(Convert.ToInt32(Session["PatientId"]));
            string ARVStatus = objPatientStatus.Tables[1].Rows[0]["ARVStatus"].ToString();
            if (ARVStatus == "1")
            {
                PnlDrug.Enabled = false;
                BtnAddARV.Enabled = false;
            }

        }
    }
}
