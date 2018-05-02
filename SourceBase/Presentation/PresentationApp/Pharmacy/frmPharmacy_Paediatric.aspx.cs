#region "Namespace"
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
using System.Collections.Generic;
using System.Text;

using Interface.Security;
using Interface.Administration;
using Application.Common;
using Application.Presentation;
using Interface.Pharmacy;
using Interface.Clinical;
using AjaxControlToolkit;
#endregion

public partial class frmPharmacy_Paediatric : BasePage
{
    ////////////////////////////////////////////////////////////////////
    // Code Written By   : Sanjay Rana 
    // Modification Date : 19th Jan 2007
    // Description       : Padeatric Pharmacy
    // Modified By       : Amitava Sinha
    // Modification Date : 5th May 2007
    /// /////////////////////////////////////////////////////////////////


    #region Declaration of variable
    public DataTable theDrugTable;
    public DataTable AddARV;
    public DataTable OtherDrugs;
    public DataTable TBDrugs;
    public DataTable OIDrugs;
    public DataTable NonARVDrugs;
    int VisitType = 4;
    int CountFixedComb = 0;
    DataSet theDS;
    DataSet theExistDS = new DataSet();
    DataView theDV = new DataView();
    DateTime thepharmOrderedbyDate, thepharmReportedbyDate, theCurrentDate;
    IIQCareSystem IQCareSecurity;
    int theTreatmentID;
    int theRegimenLine;
    int theProviderID; // rupesh 19-sep-07
    //Amitava Sinha
    int icount;
    StringBuilder sbParameter;
    StringBuilder sbValues;
    string strmultiselect;
    String TableName;
    ArrayList arl;
    //Amitava Sinha 
    #endregion
    Application.Common.Utility theUti = new Application.Common.Utility();
    #region "AutoComplete Method"
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<string> SearchDrugs(string prefixText, int count)
    {
        List<string> Drugs = new List<string>();
        IDrug objRptFields;
        objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
        string sqlQuery;
        //creating Sql Query
        if (HttpContext.Current.Session["Paperless"].ToString() == "1" && HttpContext.Current.Session["SCMModule"] != null)
        {
            if (HttpContext.Current.Session["ARTEndedStatus"].ToString() == "ART Stopped")
            {
                sqlQuery = "select md.Drug_pk,convert(varchar(100),md.DrugName + isnull(replicate(' ',100-len(md.drugname)),''))+'    '+ISNULL(Convert(varchar,SUM(st.Quantity)),0)[Drugname] from dtl_stocktransaction st ";
                sqlQuery += " Right outer join mst_drug md on md.Drug_pk=st.ItemId where dbo.fn_GetDrugTypeId_futures (md.Drug_pk) <>37 and DrugName LIKE '%" + prefixText + "%' Group by md.Drug_pk,md.Drugname";
            }
            else
            {
                sqlQuery = "select md.Drug_pk,convert(varchar(100),md.DrugName + isnull(replicate(' ',100-len(md.drugname)),''))+'    '+ISNULL(Convert(varchar,SUM(st.Quantity)),0)[Drugname] from dtl_stocktransaction st ";
                sqlQuery += " Right outer join mst_drug md on md.Drug_pk=st.ItemId where DrugName LIKE '%" + prefixText + "%' Group by md.Drug_pk,md.Drugname";

            }
        }
        else
        {
            if (HttpContext.Current.Session["ARTEndedStatus"].ToString() == "ART Stopped")
            {
                sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0 and dbo.fn_GetDrugTypeId_futures (Drug_pk) <>37 and  DrugName LIKE '%{0}%'", prefixText);
            }
            else
            {
                if (HttpContext.Current.Session["TreatmentProg"] != null)
                {
                    if (HttpContext.Current.Session["TreatmentProg"].ToString() == "225")
                    {

                        sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0 and dbo.fn_GetDrugTypeId_futures (Drug_pk) <>37 and DrugName LIKE '%{0}%'", prefixText);
                    }
                    else
                    {
                        sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0 and DrugName LIKE '%{0}%'", prefixText);
                    }
                }
                else
                {
                    sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0 and DrugName LIKE '%{0}%'", prefixText);
                }
            }

        }
        //filling data from database
        DataTable dataTable = objRptFields.ReturnDatatableQuery(sqlQuery);
        string custItem = string.Empty;
        if (dataTable.Rows.Count > 0)
        {

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                custItem = AutoCompleteExtender.CreateAutoCompleteItem(dataTable.Rows[i][1].ToString(), dataTable.Rows[i][0].ToString());
                Drugs.Add(custItem);

            }

        }
        return Drugs;

    }

    protected void txtautoDrugName_TextChanged(object sender, EventArgs e)
    {

        try
        {
            IQCareUtils theUtils = new IQCareUtils();
            DataView theAutoDV;
            DataView theExistsDV;
            DataSet theAutoDS = (DataSet)ViewState["MasterData"];
            int DrugId;
            if (hdCustID.Value != "")
            {
                if ((Convert.ToInt32(hdCustID.Value) != 0))
                {
                    DrugId = Convert.ToInt32(hdCustID.Value);
                    theAutoDV = new DataView(theAutoDS.Tables[0]);

                    //if (HttpContext.Current.Session["Paperless"].ToString() == "1" && HttpContext.Current.Session["SCMModule"] != null)
                    //{
                    //    theAutoDV.RowFilter = "Drug_Pk = " + DrugId + " and Stock>0";
                    //    DataTable theFiltDT = (DataTable)theUtils.CreateTableFromDataView(theAutoDV);
                    //    if (theFiltDT.Rows.Count < 1)
                    //    {
                    //        IQCareMsgBox.Show("DrugOutofStock", this);
                    //        txtautoDrugName.Text = "";
                    //        return;

                    //    }

                    //}
                    //else
                        theAutoDV.RowFilter = "Drug_Pk = " + DrugId;
                    DataTable theAutoDT = (DataTable)theUtils.CreateTableFromDataView(theAutoDV);
                    if (theAutoDT.Rows[0]["DrugTypeId"].ToString() == "37")
                    {
                        if (Session["AddARV"] == null)
                        {
                            DataTable theDT = new DataTable();
                            theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
                            theDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
                            theDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
                            theDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
                            //theDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
                            Session["AddARV"] = theDT;
                        }
                        DataTable ExistDT = (DataTable)Session["AddARV"];
                        theExistsDV = new DataView(ExistDT);
                        theExistsDV.RowFilter = "DrugId =" + theAutoDT.Rows[0]["Drug_pk"];
                        DataTable theSelExistsDT = (DataTable)theUtils.CreateTableFromDataView(theExistsDV);
                        if (theSelExistsDT.Rows.Count == 0)
                        {
                            DataRow DR = ExistDT.NewRow();
                            DR[0] = theAutoDT.Rows[0]["Drug_pk"];
                            DR[1] = theAutoDT.Rows[0]["DrugName"];
                            DR[2] = 0;
                            DR[3] = theAutoDT.Rows[0]["DrugTypeId"];
                            //DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistDT, pnlPedia);
                            Session["AddARV"] = ExistDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistDT, pnlPedia);
                            return;

                        }
                    }
                    else if (theAutoDT.Rows[0]["DrugTypeId"].ToString() == "31")
                    {
                        if (Session["AddTB"] == null)
                        {
                            DataTable theTBDT = new DataTable();
                            theTBDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
                            theTBDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
                            theTBDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
                            theTBDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
                            //theTBDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
                            Session["AddTB"] = theTBDT;
                        }
                        DataTable ExistTBDT = (DataTable)Session["AddTB"];
                        theExistsDV = new DataView(ExistTBDT);
                        theExistsDV.RowFilter = "DrugId =" + theAutoDT.Rows[0]["Drug_pk"];
                        DataTable theSelExistsDT = (DataTable)theUtils.CreateTableFromDataView(theExistsDV);
                        if (theSelExistsDT.Rows.Count == 0)
                        {
                            DataRow DR = ExistTBDT.NewRow();
                            DR[0] = theAutoDT.Rows[0]["Drug_pk"];
                            DR[1] = theAutoDT.Rows[0]["DrugName"];
                            DR[2] = 0;
                            DR[3] = theAutoDT.Rows[0]["DrugTypeId"];
                            //DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistTBDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistTBDT, pnlOtherTBMedicaton);
                            Session["AddTB"] = ExistTBDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistTBDT, pnlOtherTBMedicaton);
                            return;

                        }
                    }
                    else if (theAutoDT.Rows[0]["DrugTypeId"].ToString() == "36")
                    {
                        if (Session["OIDrugs"] == null)
                        {
                            DataTable theOthDT = new DataTable();
                            theOthDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
                            theOthDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
                            theOthDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
                            theOthDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
                            //theOthDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
                            Session["OIDrugs"] = theOthDT;
                        }
                        DataTable ExistOIDT = (DataTable)Session["OIDrugs"];
                        theExistsDV = new DataView(ExistOIDT);
                        theExistsDV.RowFilter = "DrugId =" + theAutoDT.Rows[0]["Drug_pk"];
                        DataTable theSelExistsDT = (DataTable)theUtils.CreateTableFromDataView(theExistsDV);
                        if (theSelExistsDT.Rows.Count == 0)
                        {
                            DataRow DR = ExistOIDT.NewRow();
                            DR[0] = theAutoDT.Rows[0]["Drug_pk"];
                            DR[1] = theAutoDT.Rows[0]["DrugName"];
                            DR[2] = 0;
                            DR[3] = theAutoDT.Rows[0]["DrugTypeId"];
                            //DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistOIDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistOIDT, PnlOIARV);
                            Session["OIDrugs"] = ExistOIDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistOIDT, PnlOIARV);
                            return;

                        }
                    }
                    else
                    {
                        if (Session["OtherDrugs"] == null)
                        {
                            DataTable theOthDT = new DataTable();
                            theOthDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
                            theOthDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
                            theOthDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
                            theOthDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
                            //theOthDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
                            Session["OtherDrugs"] = theOthDT;
                        }
                        DataTable ExistOthDT = (DataTable)Session["OtherDrugs"];
                        theExistsDV = new DataView(ExistOthDT);
                        theExistsDV.RowFilter = "DrugId =" + theAutoDT.Rows[0]["Drug_pk"];
                        DataTable theSelExistsDT = (DataTable)theUtils.CreateTableFromDataView(theExistsDV);
                        if (theSelExistsDT.Rows.Count == 0)
                        {
                            DataRow DR = ExistOthDT.NewRow();
                            DR[0] = theAutoDT.Rows[0]["Drug_pk"];
                            DR[1] = theAutoDT.Rows[0]["DrugName"];
                            DR[2] = 0;
                            DR[3] = theAutoDT.Rows[0]["DrugTypeId"];
                            //DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistOthDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistOthDT, PnlOtMed);
                            Session["OtherDrugs"] = ExistOthDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistOthDT, PnlOtMed);
                            return;

                        }
                    }
                    if (Convert.ToInt32(Session["PatientVisitId"]) > 0 && Session["ExistPharmacyData"] != null)
                    {
                        foreach (DataRow dr in ((DataTable)(Session["ExistPharmacyData"])).Rows)
                        {

                            FillOldData(PnlOIARV, dr, false);
                            FillOldData(pnlPedia, dr, false);
                            FillOldData(PnlOtMed, dr, false);
                            FillOldData(pnlOtherTBMedicaton, dr, false);
                            //FillMaternalHealth(dr);

                        }
                    }

                    txtautoDrugName.Text = "";

                }
            }
            //gvCustomer.DataSource = GetCustomerDetail(Convert.ToInt32(hdCustID.Value));
            //gvCustomer.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //}
    }


    #endregion
    //protected void Page_Close(object sender, EventArgs e1)
    //{
    //    Session["PatientVisitId"] = 0;
    //}
    protected void Page_Load(object sender, EventArgs e1)
    {
        if (Session["AppLocation"] == null || Session.Count == 0 || Session["AppUserID"].ToString() == "")
        {
            IQCareMsgBox.Show("SessionExpired", this);
            Response.Redirect("~/frmlogin.aspx",true);
        }

        int thePtnID = 0;
        Session["PtnRegCTC"] = "";
        Session["CustomfrmDrug"] = "";
        Session["CustomfrmLab"] = "";


        //(Master.FindControl("lblRoot") as Label).Text = "Pharmacy Forms >>";
        //(Master.FindControl("lblMark") as Label).Visible = false;
        //(Master.FindControl("lblheader") as Label).Text = "Paediatric Pharmacy";
        (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Text = "Pharmacy Forms >> ";
        (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "Paediatric Pharmacy";

        if (Request.QueryString["Prog"] == "ART")
        {
            (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["HIVPatientStatus"]);
        }
        else
        {
            (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["PMTCTPatientStatus"]);
        }
        //////if(Session["PatientStatus"]!=null)
        //////    (Master.FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["PatientStatus"]);
        //(Master.FindControl("lblformname") as Label).Text = "Paediatric Pharmacy Form";
        (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblformname") as Label).Text = "Paediatric Pharmacy Form";
        if ((Session["HIVPatientStatus"].ToString() == "1") && (Session["PMTCTPatientStatus"].ToString() == "1"))
        {
            btnsave.Enabled = false;
        }
        if (Session["Paperless"].ToString() == "0")
        {
            lbldispensedby.Attributes.Add("Class", "required");
            lbldispensedbydate.Attributes.Add("Class", "required");
        }
        DataTable theDTModule = (DataTable)Session["AppModule"];
        string ModuleId = "";
        foreach (DataRow theDR in theDTModule.Rows)
        {
            if (ModuleId == "")
                ModuleId = theDR["ModuleId"].ToString();
            else
                ModuleId = ModuleId + "," + theDR["ModuleId"].ToString();
        }
        //DataView theSCMDV = new DataView(theDTModule);
        //theSCMDV.RowFilter = "ModuleId=201";
        //if (theSCMDV.Count > 0)
        //    Session["SCMModule"] = theSCMDV[0]["ModuleName"];

        // opento=ArtForm

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            //theUti.SetSession();
        }

        PutCustomControl();


        /***************** Check For User Rights ****************/
        AuthenticationManager Authentiaction = new AuthenticationManager();
        if (Authentiaction.HasFunctionRight(ApplicationAccess.PaediatricPharmacy, FunctionAccess.Print, (DataTable)Session["UserRight"]) == false)
        {
            btnPrint.Enabled = false;

        }

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            if (Authentiaction.HasFunctionRight(ApplicationAccess.PaediatricPharmacy, FunctionAccess.Add, (DataTable)Session["UserRight"]) == false)
            {
                btnsave.Enabled = false;
            }
            if (Request.QueryString["Prog"] == "PMTCT")
            {
                ddlTreatment.SelectedValue = "223";
                //ddlTreatment.Enabled = false;

            }

        }
        else if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
        {
            if (Authentiaction.HasFunctionRight(ApplicationAccess.PaediatricPharmacy, FunctionAccess.View, (DataTable)Session["UserRight"]) == false)
            {
                int PatientID = Convert.ToInt32(Session["PatientId"]);
                string theUrl = "";
                theUrl = string.Format("{0}", "../ClinicalForms/frmPatient_History.aspx");
                Response.Redirect(theUrl);
            }
            else if (Authentiaction.HasFunctionRight(ApplicationAccess.PaediatricPharmacy, FunctionAccess.Update, (DataTable)Session["UserRight"]) == false)
            {
                btnsave.Enabled = false;

            }

            int PID = Convert.ToInt32(Session["PatientId"]);
            Session["PharmacyId"] = Convert.ToInt32(Session["PatientVisitId"]);
            //FillOldCustomData(pnlCustomList, PID);
            FillOldCustomData(PID);
        }
        else if (Request.QueryString["name"] == "Delete")
        {
            int PID = Convert.ToInt32(Session["PatientId"]);
            if (Authentiaction.HasFunctionRight(ApplicationAccess.PaediatricPharmacy, FunctionAccess.View, (DataTable)Session["UserRight"]) == false)
            {
                int PatientID = Convert.ToInt32(Session["PatientId"]);
                string theUrl = "";
                theUrl = string.Format("{0}", "../ClinicalForms/frmClinical_DeleteForm.aspx");
                Response.Redirect(theUrl);
            }
            else if (Authentiaction.HasFunctionRight(ApplicationAccess.PaediatricPharmacy, FunctionAccess.Delete, (DataTable)Session["UserRight"]) == false)
            {
                btnsave.Text = "Delete";

                btnsave.Enabled = false;
                // btnQualityCheck.Visible = false;
            }
            FillOldCustomData(PID);
        }


        if (!IsPostBack)
        {
            //Original ord date when the form was first loaded
            if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
            {
                ViewState["OrigOrdDate"] = theExistDS.Tables[0].Rows[0]["OrderedByDate"];
                if (theExistDS.Tables[0].Rows[0]["DispensedByDate"].ToString() != "")
                {
                    Session["OrigDispensbyDate"] = theExistDS.Tables[0].Rows[0]["DispensedByDate"].ToString();
                }
                else
                {
                    Session["OrigDispensbyDate"] = null;
                }
            }
            else
            //ViewState["OrigOrdDate"] = null;
            {

                Session["OrigDispensbyDate"] = null;
                ViewState.Remove("OrigOrdDate");
            }

            txtWeight.Attributes.Add("onkeyup", "chkNumeric('" + txtWeight.ClientID + "')");
            txtHeight.Attributes.Add("onkeyup", "chkNumeric('" + txtHeight.ClientID + "')");
            txtBSA.Attributes.Add("onkeyup", "chkNumeric('" + txtBSA.ClientID + "')");

            txtpharmOrderedbyDate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
            txtpharmOrderedbyDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");

            txtpharmReportedbyDate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
            txtpharmReportedbyDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
            //ddlTreatment.Attributes.Add("onchange", "fnCheckUnCheck()");
            Session["AddARV"] = null;
            Session["OtherDrugs"] = null;
            Session["MasterDrugTable"] = null;
            Session["AddTB"] = null;
            Session["OIDrugs"] = null;
            Session["PharmacyId"] = null;
            thePtnID = Convert.ToInt32(Session["PatientId"]);
            ViewState["PtnID"] = thePtnID;
            Session["PtnID"] = thePtnID;
            ViewState["UserID"] = Session["AppUserId"].ToString();
            ViewState["LocationId"] = Convert.ToInt32(Session["AppLocationId"]);
            ViewState["SelectedDrug"] = theDrugTable;
            ViewState["MasterData"] = theDS;
            if (Request.QueryString["Prog"] == "ART")
            {
                ViewState["Status"] = Session["HIVPatientStatus"].ToString();
                Session["Status"] = Session["HIVPatientStatus"].ToString();
            }
            else
            {
                ViewState["Status"] = Session["PMTCTPatientStatus"].ToString();
                Session["Status"] = Session["PMTCTPatientStatus"].ToString();
            }

            if (theDS != null) // rupesh 18-sep-07
            {
                DataView theEnollDV = new DataView(theDS.Tables[10]);
                theEnollDV.RowFilter = "ModuleId=" + Session["TechnicalAreaId"].ToString();
                if (theEnollDV.Count > 0)
                    ViewState["EnrolmentDate"] = theEnollDV[0]["StartDate"];
                else
                    Response.Redirect("../frmFindAddPatient.aspx");

            }
            BSF_Attributes();
            #region "DrugDataTable"
            if (theDS != null)
            {
                DataTable theDT = new DataTable();
                // --- check inactive /active cases
                theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
                theDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
                theDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
                theDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
                theDT.Columns.Add("Abbr", System.Type.GetType("System.String"));


                foreach (DataRow dr in theDS.Tables[0].Rows)
                {
                    DataRow theDR = theDT.NewRow();
                    theDR[0] = dr["Drug_Pk"];
                    theDR[1] = dr["DrugName"];
                    theDR[2] = 0;
                    theDR[3] = dr["DrugTypeId"];
                    theDR[4] = dr["GenericAbbrevation"];
                    theDT.Rows.Add(theDR);
                }
                //foreach (DataRow dr in theDS.Tables[4].Rows)
                //{
                //    DataRow theDR = theDT.NewRow();
                //    theDR[0] = dr["GenericId"];
                //    theDR[1] = dr["GenericName"];
                //    theDR[2] = 1;
                //    theDR[3] = dr["DrugTypeId"];
                //    theDT.Rows.Add(theDR);
                //}

                //foreach (DataRow dr in theDrugTable.Rows)
                //{
                //    if (Convert.ToInt32(dr["GenericId"]) > 0)
                //    {
                //        DataRow[] theDR = theDT.Select("DrugId = " + dr["GenericId"].ToString() + " and Generic = 1");
                //        theDT.Rows.Remove(theDR[0]);
                //    }
                //    else if (Convert.ToInt32(dr["DrugId"]) < 10000)
                //    {
                //        DataRow[] theDR = theDT.Select("DrugId = " + dr["DrugId"].ToString() + " and Generic = 0");
                //        theDT.Rows.Remove(theDR[0]);
                //    }
                //}
                //Changes for Duplicate Drug Name--Amitava Sinha
                //DataTable dtDuplicate = theDT.Copy();
                //SortDataTable(dtDuplicate, "DrugName asc");

                //String drgNameDup = string.Empty;
                //foreach (DataRow drduplicate in dtDuplicate.Rows)
                //{
                //    if (drgNameDup == Convert.ToString(drduplicate["DrugName"]))
                //    {
                //        DataRow[] theDDR = theDT.Select("DrugName='" + drduplicate["DrugName"].ToString() + "' and Generic=0");
                //        if (theDDR.Length > 0)
                //            theDT.Rows.Remove(theDDR[0]);
                //    }
                //    drgNameDup = Convert.ToString(drduplicate["DrugName"]);
                //}
                //Changes for Duplicate Drug Name--Amitava Sinha
                ViewState["MasterDrugTable"] = theDT;
            }
            #endregion
            if (ViewState["OldDS"] == null)
            {
                if (theExistDS.Tables.Count > 0 && theExistDS.Tables[0].Rows.Count == 0)
                {
                    IQCareMsgBox.Show("NoPaediatricRecordExists", this);
                    return;
                }
                else
                {
                    ViewState["OldDS"] = theExistDS;
                }
            }
            if (Convert.ToInt32(Session["PatientVisitId"]) > 0 && Convert.ToInt32(Session["PatientVisitId"]) != 0)
            {
                Session["AddARV"] = AddARV;
                Session["OtherDrugs"] = OtherDrugs;
                Session["AddTB"] = TBDrugs;
                //Session["AddNonARV"] = NonARVDrugs;
                Session["OIDrugs"] = OIDrugs;
                ViewState["PharmacyId"] = Session["PatientVisitId"].ToString();

                //MsgBuilder theBuilder = new MsgBuilder();
                //theBuilder.DataElements["FormName"] = "Paediatric detail";
                //IQCareMsgBox.ShowConfirm("UpdateClinicalRecord", theBuilder, btnOk);

            }

            else if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                //MsgBuilder theBuilder = new MsgBuilder();
                //theBuilder.DataElements["FormName"] = "Paediatric detail";
                //IQCareMsgBox.ShowConfirm("AddClinicalRecord", theBuilder, btnOk);
            }
            else if (Request.QueryString["name"] == "Delete")
            {
                //MsgBuilder theBuilder = new MsgBuilder();
                //theBuilder.DataElements["FormName"] = "Paediatric detail";
                //IQCareMsgBox.ShowConfirm("DeleteForm", theBuilder, btnsave);
            }


        }
        else
        {
            //#region "Immunization"
            ////if (ViewState["MasterData"] != null)
            ////{
            //    BindInfantHealthSection((DataSet)ViewState["MasterData"]);
            ////}
            //#endregion
            #region "Additional ARV"
            if ((DataTable)Application["AddARV"] != null)
            {
                Session["AddARV"] = (DataTable)Application["AddARV"];
                ViewState["MasterDrugTable"] = (DataTable)Application["MasterData"];
                Application.Remove("MasterData");
                Application.Remove("AddARV");
            }


            if ((DataTable)Session["AddARV"] != null)
            {
                DataTable theDT = (DataTable)Session["AddARV"];
                // divAddARV.Visible = false;
                //LoadAdditionalDrugs(theDT, PnlAdARV);
                LoadAdditionalDrugs(theDT, pnlPedia);

            }
            #endregion

            #region "Additional Other Medications"
            if ((DataTable)Application["OtherDrugs"] != null)
            {
                Session["OtherDrugs"] = (DataTable)Application["OtherDrugs"];
                Application.Remove("OtherDrugs");
                ViewState["MasterDrugTable"] = (DataTable)Application["MasterData"];
                Application.Remove("MasterData");
            }
            if ((DataTable)Session["OtherDrugs"] != null)
            {
                DataTable theDT = (DataTable)Session["OtherDrugs"];
                // divAddOI.Visible = false;
                LoadAdditionalDrugs(theDT, PnlOtMed);
            }
            #endregion
            #region "Additional OI Medications"
            if ((DataTable)Application["OIDrugs"] != null)
            {
                Session["OIDrugs"] = (DataTable)Application["OIDrugs"];
                Application.Remove("OIDrugs");
                ViewState["MasterDrugTable"] = (DataTable)Application["MasterData"];
                Application.Remove("MasterData");
            }
            if ((DataTable)Session["OIDrugs"] != null)
            {
                DataTable theDT = (DataTable)Session["OIDrugs"];
                // divAddOI.Visible = false;
                LoadAdditionalDrugs(theDT, PnlOIARV);
            }
            #endregion
            #region "TB Drugs"
            if ((DataTable)Application["AddTB"] != null)
            {
                Session["AddTB"] = (DataTable)Application["AddTB"];
                ViewState["MasterDrugTable"] = (DataTable)Application["MasterData"];
                Application.Remove("MasterData");
                Application.Remove("AddTB");
            }
            if ((DataTable)Session["AddTB"] != null)
            {
                DataTable theDT = (DataTable)Session["AddTB"];
                //divAddARV.Visible = false;
                LoadAdditionalDrugs(theDT, pnlOtherTBMedicaton);

            }
            #endregion

            CreateControls((DataSet)ViewState["MasterData"]);

        }


        if (Session["HIVPatientStatus"].ToString() == "1" && Session["PMTCTPatientStatus"].ToString() == "1")
        {
            btnsave.Enabled = false;
        }
        if (Session["CareEndFlag"].ToString() == "1")
        {
            btnsave.Enabled = true;
        }
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "onload", "<script language='javascript' type='text/javascript'>fnCheckUnCheck();</script>");

        #region "Check ARTStop"
        IDrug theValidationManger = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
        DataTable theValidateDT = theValidationManger.CheckARTStopStatus(Convert.ToInt32(Session["PatientId"]));
        if ((theValidateDT.Rows.Count > 0 && theValidateDT.Rows[0]["ARTStatus"].ToString() == "ART Stopped" && Convert.ToInt32(Session["PatientVisitId"]) == 0 && Convert.ToInt32(ddlTreatment.SelectedValue) == 222) ||
            (theValidateDT.Rows.Count > 0 && theValidateDT.Rows[0]["ARTStatus"].ToString() == "ART Stopped" && Convert.ToInt32(Session["PatientVisitId"]) > 0
            && Convert.ToDateTime(txtpharmReportedbyDate.Value) >= Convert.ToDateTime(theValidateDT.Rows[0]["ARTEndDate"]) && Convert.ToInt32(ddlTreatment.SelectedValue) == 222))
        {
            pnlPedia.Enabled = false;
            //PnlAdARV.Enabled = false;
            //PnlFixed.Enabled = false;
            //BtnAddARV.Enabled = false;
        }
        else
        {
            pnlPedia.Enabled = true;
            //PnlAdARV.Enabled = true;
            //PnlFixed.Enabled = true;
            //BtnAddARV.Enabled = true;
        }
        theValidationManger = null;
        theValidateDT.Dispose();
        #endregion

    }

    #region "User Function"

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

    void Control_Load(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;
        tbox.MaxLength = 3;
        //tbox.Attributes.Add("onkeyup", "chkNumber('" + tbox.ClientID + "')");
        if (Session["SCMModule"] != null)
        {
            if (tbox.ClientID.Contains("QtyDispensed"))
            {

                tbox.Attributes.Add("readOnly", "true");
               
            }
            else
            {
                tbox.Attributes.Add("onkeyup", "chkDecimal('" + tbox.ClientID + "')");
            }
        }
        else
        {
            tbox.Attributes.Add("onkeyup", "chkDecimal('" + tbox.ClientID + "')");
        }
    }
    void DecimalText_Load(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;
        tbox.MaxLength = 3;
        tbox.Attributes.Add("onkeyup", "chkDecimal('" + tbox.ClientID + "')");
    }

    void ddlFixDrugname_SelectedChanged(object sender, EventArgs e)
    {
        //DropDownList ddlfixdrugname = (DropDownList)sender;
        //BindFunctions theFixBindMgr = new BindFunctions();
        //DataSet theDSFix=((DataSet)ViewState["MasterData"]);
        //foreach (Control y in divFixed.Controls)
        //{
        //    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
        //    {
        //        //
        //        foreach (Control x in y.Controls)
        //        {
        //            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel) && x.ID == "theFixPnlCntrl")
        //            {
        //                foreach (Control z in x.Controls)
        //                {
        //                    if (z.GetType() == typeof(System.Web.UI.WebControls.DropDownList) && z.ID == "theFixedDrugStrength")
        //                    {
        //                        DataTable theFixDTStrnth = new DataTable();
        //                        DataView theFixDVStrength = new DataView((DataTable)Session["FixDrugStrength"]);

        //                        if (Convert.ToInt32(ddlfixdrugname.SelectedValue) > 0)
        //                        {
        //                            theFixDVStrength.RowFilter = "Drug_pk = " + Convert.ToInt32(ddlfixdrugname.SelectedValue);

        //                            if (theFixDVStrength.Count > 0)
        //                            {
        //                                IQCareUtils theUtils = new IQCareUtils();
        //                                theFixDTStrnth = theUtils.CreateTableFromDataView(theFixDVStrength);
        //                                theFixBindMgr.BindCombo((DropDownList)z, theFixDTStrnth, "StrengthName", "StrengthId");
        //                            }
        //                        }

        //                    }
        //                     if (z.GetType() == typeof(System.Web.UI.WebControls.DropDownList) && z.ID == "drgFixFrequency")
        //                    {
        //                        DataTable theFixDTFreq = new DataTable();
        //                        DataView theFixDVFreq = new DataView((DataTable)Session["FixDrugFreq"]);

        //                        if (Convert.ToInt32(ddlfixdrugname.SelectedValue) > 0)
        //                        {
        //                            theFixDVFreq.RowFilter = "Drug_pk = " + Convert.ToInt32(ddlfixdrugname.SelectedValue);

        //                            if (theFixDVFreq.Count > 0)
        //                            {
        //                                IQCareUtils theUtils = new IQCareUtils();
        //                                theFixDTFreq = theUtils.CreateTableFromDataView(theFixDVFreq);
        //                                theFixBindMgr.BindCombo((DropDownList)z, theFixDTFreq, "FrequencyName", "FrequencyId");
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

    }

    //private void BindPanelInfantHealthSection(int DrugPk, String DrugName, int count)
    //{
    //    BindFunctions theBindManager = new BindFunctions();
    //    Panel Pnl = new Panel();
    //    if (count == 0)
    //    {
    //        Pnl.ID = "PnlInHealth" + DrugPk;
    //    }
    //    HtmlInputCheckBox chkInfantHealth = new HtmlInputCheckBox();
    //    chkInfantHealth.ID = DrugPk.ToString();
    //    chkInfantHealth.Value = DrugName;
    //    Pnl.Controls.Add(chkInfantHealth);
    //    DataView theDV_IH = new DataView(theDS.Tables[19]);
    //    theDV_IH.RowFilter = "DrugID = " + DrugPk + "";
    //    DropDownList ddlInfantHealth = new DropDownList();
    //    ddlInfantHealth.ID = "ddl" + DrugPk;
    //    theBindManager.BindCombo(ddlInfantHealth, theDV_IH.ToTable(), "Name", "ID");

    //    Pnl.Controls.Add(ddlInfantHealth);
    //    //PnlInfantHealth.Controls.Add(Pnl);
    //}


    private void BindInfantHealthSection(DataSet theDS)
    {
        BindFunctions theBindManager = new BindFunctions();
        DataTable theDT;
        DataView theDV = new DataView(theDS.Tables[22]);
        theDV.RowFilter = "drugtypeid=60 and DrugName <> '' and GenericID=0";
        theDT = theDV.ToTable();
        //theBindManager.BindCheckedList(chkImmunizations, theDT, "DrugName", "Drug_pk");
        int j = 0, k = 0;
        TableRow trInfantHealth = new TableRow();
        for (int i = 0; i < theDT.Rows.Count; i++)
        {
            if (j == 0)
            {
                TableCell tcchkInfantHealth1 = new TableCell();
                TableCell tcDDInfantHealth1 = new TableCell();
                HtmlInputCheckBox chkInfantHealth = new HtmlInputCheckBox();
                chkInfantHealth.ID = theDT.Rows[i]["Drug_pk"] + "-" + theDT.Rows[i]["GenericID"].ToString();
                chkInfantHealth.Value = theDT.Rows[i]["DrugName"].ToString();
                tcchkInfantHealth1.Controls.Add(chkInfantHealth);
                tcchkInfantHealth1.Controls.Add(new LiteralControl(chkInfantHealth.Value));
                trInfantHealth.Cells.Add(tcchkInfantHealth1);
                DataView theDV_IH = new DataView(theDS.Tables[19]);
                theDV_IH.RowFilter = "DrugID = " + Convert.ToInt32(theDT.Rows[i]["Drug_pk"]) + "";
                DropDownList ddlInfantHealth = new DropDownList();
                ddlInfantHealth.ID = "ddl" + theDT.Rows[i]["Drug_pk"] + theDT.Rows[i]["GenericID"];
                theBindManager.BindCombo(ddlInfantHealth, theDV_IH.ToTable(), "Name", "ID");
                tcDDInfantHealth1.Controls.Add(ddlInfantHealth);
                trInfantHealth.Cells.Add(tcDDInfantHealth1);
                j = 1;
                k++;
            }
            else if (j == 1)
            {
                TableCell tcchkInfantHealth2 = new TableCell();
                TableCell tcDDInfantHealth2 = new TableCell();
                HtmlInputCheckBox chkInfantHealth = new HtmlInputCheckBox();
                chkInfantHealth.ID = theDT.Rows[i]["Drug_pk"] + "-" + theDT.Rows[i]["GenericID"].ToString();
                chkInfantHealth.Value = theDT.Rows[i]["DrugName"].ToString();
                tcchkInfantHealth2.Controls.Add(chkInfantHealth);
                tcchkInfantHealth2.Controls.Add(new LiteralControl(chkInfantHealth.Value));
                trInfantHealth.Cells.Add(tcchkInfantHealth2);
                DataView theDV_IH = new DataView(theDS.Tables[19]);
                theDV_IH.RowFilter = "DrugID = " + Convert.ToInt32(theDT.Rows[i]["Drug_pk"]) + "";
                DropDownList ddlInfantHealth = new DropDownList();
                ddlInfantHealth.ID = "ddl" + theDT.Rows[i]["Drug_pk"] + theDT.Rows[i]["GenericID"];
                theBindManager.BindCombo(ddlInfantHealth, theDV_IH.ToTable(), "Name", "ID");
                tcDDInfantHealth2.Controls.Add(ddlInfantHealth);
                trInfantHealth.Cells.Add(tcDDInfantHealth2);
                j = 2;
                k++;
            }
            else if (j == 2)
            {
                TableCell tcchkInfantHealth3 = new TableCell();
                TableCell tcDDInfantHealth3 = new TableCell();
                HtmlInputCheckBox chkInfantHealth = new HtmlInputCheckBox();
                chkInfantHealth.ID = theDT.Rows[i]["Drug_pk"] + "-" + theDT.Rows[i]["GenericID"].ToString();
                chkInfantHealth.Value = theDT.Rows[i]["DrugName"].ToString();
                tcchkInfantHealth3.Controls.Add(chkInfantHealth);
                tcchkInfantHealth3.Controls.Add(new LiteralControl(chkInfantHealth.Value));
                trInfantHealth.Cells.Add(tcchkInfantHealth3);
                DataView theDV_IH = new DataView(theDS.Tables[19]);
                theDV_IH.RowFilter = "DrugID = " + Convert.ToInt32(theDT.Rows[i]["Drug_pk"]) + "";
                DropDownList ddlInfantHealth = new DropDownList();
                ddlInfantHealth.ID = "ddl" + theDT.Rows[i]["Drug_pk"] + theDT.Rows[i]["GenericID"];
                theBindManager.BindCombo(ddlInfantHealth, theDV_IH.ToTable(), "Name", "ID");
                tcDDInfantHealth3.Controls.Add(ddlInfantHealth);
                trInfantHealth.Cells.Add(tcDDInfantHealth3);
                j = 0;
                k++;
            }
            if (k == 3)
            {
                //tblInfantHealth.Rows.Add(trInfantHealth);
                k = 0;
                trInfantHealth = new TableRow();
            }
        }

    }
    private void MakeRegimenGenericTable(DataSet theDS)
    {
        DataTable theDT;
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            theDT = theDS.Tables[17];
        }
        else
        {
            theDT = theDS.Tables[17];
        }

        DataView theDV;//= new DataView();
        BindFunctions theBindMgr = new BindFunctions();
        int RegimenId = -1;
        string GenericID = string.Empty;
        string GenericName = string.Empty;

        DataTable theDT1 = new DataTable();
        theDT1.Columns.Add("ID", System.Type.GetType("System.Int32"));
        theDT1.Columns.Add("TBRegimenID", System.Type.GetType("System.Int32"));
        theDT1.Columns.Add("Name", System.Type.GetType("System.String"));
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
            if (Convert.ToInt32(theDT.Rows[i]["ID"]) > 0)
            {
                if (RegimenId != Convert.ToInt32(theDT.Rows[i]["ID"]))
                {
                    RegimenId = Convert.ToInt32(theDT.Rows[i]["ID"]);

                    theDV = new DataView(theDT);
                    theDV.RowFilter = "ID = " + RegimenId;

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
                        theDR["ID"] = Convert.ToInt32(theDT.Rows[i]["ID"]);
                        theDR["TBRegimenID"] = Convert.ToInt32(theDT.Rows[i]["TBRegimenID"]);
                        theDR["Name"] = Convert.ToString(theDT.Rows[i]["Name"]);
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
                theDR["ID"] = Convert.ToInt32(theDT.Rows[i]["ID"]);
                theDR["TBRegimenID"] = Convert.ToInt32(theDT.Rows[i]["TBRegimenID"]);
                theDR["Name"] = Convert.ToString(theDT.Rows[i]["Name"]);
                theDR["UserID"] = Convert.ToInt32(theDT.Rows[i]["UserID"]);
                theDR["GenericID"] = Convert.ToString(theDT.Rows[0]["GenericID"]); ;
                theDR["GenericName"] = Convert.ToString(theDT.Rows[i]["GenericName"]);
                theDR["Status"] = Convert.ToString(theDT.Rows[i]["Status"]);
                theDT1.Rows.Add(theDR);
            }

        }


        DV = new DataView(theDT1);
        DV.Sort = "TBRegimenID asc";
        theDT1 = theUtil.CreateTableFromDataView(DV);
        //theBindMgr.BindCombo(ddlARVCombReg, theDT1, "Name", "ID");



    }
    private void Init_Form()
    {
        try
        {
            txtYr.Attributes.Add("readonly", "true");
            txtMon.Attributes.Add("readonly", "true");
            txtDOB.Attributes.Add("readonly", "true");
            DataSet objPatientStatus = new DataSet();
            IPediatric PediatricManager;
            int PatientID = Convert.ToInt32(Session["PatientId"]);
            PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            objPatientStatus = PediatricManager.GetPatientRecordformStatus(Convert.ToInt32(PatientID));

            if (theExistDS.Tables.Count > 0)
                ddlTreatment.SelectedValue = theExistDS.Tables[0].Rows[0]["ProgId"].ToString();

            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                ViewState["PtnID"] = Convert.ToInt32(Session["PatientId"]);
                GetPediatricFields(PatientID);
            }
            else
            {
                GetExistPediatricFields();
            }


            //if (objPatientStatus != null)
            //{
            //    if (objPatientStatus.Tables[3].Rows.Count > 0)
            //    {
            //        //if (Request.QueryString["Prog"] == "PMTCT")
            //        //{
            //        //    ddlTreatment.SelectedValue = "223";
            //        //}
            //        //else
            //        //{
            //        //    ddlTreatment.SelectedValue = "222";
            //        //}
            //        //string ARTStatus = string.Empty;
            //        //if (ddlTreatment.SelectedItem.Value.ToString() == "222")
            //        //{
            //        //    ARTStatus = objPatientStatus.Tables[3].Rows[0]["status"].ToString();

            //        //    if (ARTStatus == "ART" || ARTStatus == "UnKnown" || ARTStatus == "Non-ART" || ARTStatus == "PMTCT" || ARTStatus == "Due for Termination" || ARTStatus == "")
            //        //    {
            //        //        btnsave.Enabled = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        btnsave.Enabled = false;
            //        //    }
            //        //}
            //        //if (ddlTreatment.SelectedItem.Value.ToString() == "223")
            //        //{
            //        //    ARTStatus = objPatientStatus.Tables[7].Rows[0]["status"].ToString();

            //        //    if (ARTStatus == "ART" || ARTStatus == "UnKnown" || ARTStatus == "Non-ART" || ARTStatus == "PMTCT" || ARTStatus == "Due for Termination" || ARTStatus=="")
            //        //    {
            //        //        btnsave.Enabled = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        btnsave.Enabled = false;
            //        //    }
            //        //}
            //    }
            //}

        }
        catch (Exception er)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = er.Message.ToString();
            IQCareMsgBox.Show("C1#", theBuilder, this);
        }

    }

    protected void GetExistPediatricFields()
    {
        IPediatric PediatricManager;
        try
        {
            PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            int PharmacyID = Convert.ToInt32(Session["PatientVisitId"]);
            int PatientID = Convert.ToInt32(Session["PatientId"]);
            ViewState["PatientId"] = PatientID;
            //pr_Pharmacy_GetExistPaediatricDetails_Constella
            theExistDS = PediatricManager.GetExistPaediatricDetails(PharmacyID);
            if (theExistDS.Tables.Count == 0)
            {
                IQCareMsgBox.Show("NoPharmacyRecordExists", this);
                return;
            }
            Session["ExistPharmacyData"] = theExistDS.Tables[0];
            //pr_Pharmacy_GetPediatricDetails_Constella
            DataSet theDrugDS = PediatricManager.GetPediatricFields(Convert.ToInt32(theExistDS.Tables[0].Rows[0]["Ptn_pk"].ToString()));
            #region "FixDoseCombination"
            theDS = new DataSet();
            theDS.Tables.Add(theDrugDS.Tables[17].Copy());//--0--performance - gen abbr & only active drugs
            theDS.Tables.Add(theDrugDS.Tables[1].Copy());//--1--
            theDS.Tables.Add(theDrugDS.Tables[2].Copy());//--2--
            theDS.Tables.Add(theDrugDS.Tables[3].Copy());//--3--
            theDS.Tables.Add(theDrugDS.Tables[4].Copy());//--4--
            theDS.Tables.Add(theDrugDS.Tables[5].Copy());//--5--
            theDS.Tables.Add(theDrugDS.Tables[6].Copy());//--6--
            theDS.Tables.Add(theDrugDS.Tables[15].Copy());//--7-- for inactive units in case of edit;
            theDS.Tables.Add(theDrugDS.Tables[8].Copy());//--8--
            theDS.Tables.Add(theDrugDS.Tables[9].Copy());//--9--
            theDS.Tables.Add(theDrugDS.Tables[10].Copy());//--10--
            theDS.Tables.Add(theDrugDS.Tables[11].Copy());//--11--
            theDS.Tables.Add(theDrugDS.Tables[12].Copy());//--12-- stores all (both active/inactive) drugs
            theDS.Tables.Add(theDrugDS.Tables[13].Copy());//--13--  rupesh 04-sep for OI and other medication - for custom frq list
            //theDS.Tables.Add(theDrugDS.Tables[14]);//  rupesh 19-sep-07 for ARV Provider
            theDS.Tables.Add(theDrugDS.Tables[16].Copy());//--14--  rupesh 19-sep-07 for active/inactive ARV Provider
            theDS.Tables.Add(theDrugDS.Tables[21].Copy());//--15--  29Feb08 -- Non-ARTDate
            theDS.Tables.Add(theDrugDS.Tables[22].Copy());//-period taken
            theDS.Tables.Add(theDrugDS.Tables[23].Copy());//-TB Regimen
            theDS.Tables.Add(theDrugDS.Tables[24].Copy());
            theDS.Tables.Add(theDrugDS.Tables[25].Copy());
            theDS.Tables.Add(theDrugDS.Tables[26].Copy());
            theDS.Tables.Add(theDrugDS.Tables[27].Copy());
            theDS.Tables.Add(theDrugDS.Tables[28].Copy());
            theDS.Tables.Add(theDrugDS.Tables[29].Copy());



            #endregion

            //---rupesh -- for fixed drug strength / frequency -----
            Session["FixDrugStrength"] = theDrugDS.Tables[18];
            Session["FixDrugFreq"] = theDrugDS.Tables[19];
            //---------------------------------------------------------------

            pnlPedia.Controls.Clear();
            ViewState["MasterData"] = theDS;
            //lblPatientName.Text = theDS.Tables[6].Rows[0]["Name"].ToString();

            //////////

            //string theEnroll = Session["AppCountryId"].ToString() + "-" + Session["AppPosID"].ToString() + "-" + Session["AppSatelliteId"].ToString() + "-" + theDS.Tables[6].Rows[0]["PatientEnrollmentID"].ToString();
            //lblpatientenrolment.Text = theDS.Tables[6].Rows[0]["PatientId"].ToString();

            ////////

            //lblExisPatientID.Text = theDS.Tables[6].Rows[0]["PatientClinicID"].ToString();
            if ((theExistDS.Tables[0].Rows[0]["Weight"] != System.DBNull.Value) || (theExistDS.Tables[0].Rows[0]["Height"] != System.DBNull.Value))
            {
                decimal theWeight = Convert.ToDecimal(theExistDS.Tables[0].Rows[0]["Weight"].ToString());
                if (theWeight > 0)
                    txtWeight.Text = Convert.ToString(theWeight);
                decimal theHeight = Convert.ToDecimal(theExistDS.Tables[0].Rows[0]["Height"].ToString());
                if (theHeight > 0)
                    txtHeight.Text = Convert.ToString(theHeight);
                decimal theBSA = theWeight * theHeight / 3600;
                theBSA = (decimal)Math.Sqrt(Convert.ToDouble(theBSA));
                theBSA = Math.Round(theBSA, 2);
                txtBSA.Text = Convert.ToString(theBSA);
            }
            DateTime theDOBirth = (DateTime)theDS.Tables[6].Rows[0]["DOB"];
            txtDOB.Text = theDOBirth.ToString(Session["AppDateFormat"].ToString());
            txtYr.Text = theDS.Tables[6].Rows[0]["Age"].ToString();
            txtMon.Text = theDS.Tables[6].Rows[0]["Age1"].ToString();

            BindddlControls(theDS);
            MakeRegimenGenericTable(theDS);
            BindInfantHealthSection(theDS);
            CreateControls(theDS);
            //BindControls(theDS);
            //BindControls();
            //TB Regimen
            //foreach (DataRow theDR in theExistDS.Tables[0].Rows)
            //{
            //    //if (theDR["TB_RegimenID"].ToString() != "0" && theDR["TB_RegimenID"].ToString()!="")
            //    //{
            //    //    ddlARVCombReg.SelectedValue = Convert.ToString(theDR["TB_RegimenID"].ToString());

            //    //    txtARVCombRegDuraton.Text = Convert.ToString(theDR["Duration"].ToString());
            //    //    if (theDR["OrderedQuantity"].ToString() != "0")
            //    //    {
            //    //        txtARVCombRegQtyPres.Text = Convert.ToString(theDR["OrderedQuantity"].ToString());
            //    //    }
            //    //    if (theDR["DispensedQuantity"].ToString() != "0")
            //    //    {
            //    //        txtARVCombRegQtyDesc.Text = Convert.ToString(theDR["DispensedQuantity"].ToString());
            //    //    }
            //    //    ddlTreatmentphase.SelectedValue = Convert.ToString(theDR["TreatmentPhase"].ToString());
            //    //    ddTrMonths.SelectedValue = Convert.ToString(theDR["Month"].ToString());
            //    //}
            //}

            if (theExistDS.Tables[0].Rows[0]["PharmacyPeriodTaken"].ToString() != "")
            {
                ddlPeriodTaken.SelectedValue = theExistDS.Tables[0].Rows[0]["PharmacyPeriodTaken"].ToString();
            }

            BindDropdownOrderBy(theExistDS.Tables[0].Rows[0]["OrderedBy"].ToString());
            BindDropdownReportedBy(theExistDS.Tables[0].Rows[0]["DispensedBy"].ToString());
            BindDropdownSignature(theExistDS.Tables[0].Rows[0]["EmployeeID"].ToString());

            ddlPharmOrderedbyName.SelectedValue = theExistDS.Tables[0].Rows[0]["OrderedBy"].ToString();
            ddlPharmReportedbyName.SelectedValue = theExistDS.Tables[0].Rows[0]["DispensedBy"].ToString();
            //---Rupesh : 31Jan08 : Signature was not saving
            ddlPharmSignature.SelectedValue = theExistDS.Tables[0].Rows[0]["EmployeeID"].ToString();


            ddlTreatment.SelectedValue = theExistDS.Tables[0].Rows[0]["ProgID"].ToString();
            if (theExistDS.Tables[0].Rows[0]["ProgID"].ToString() == "225")
            {
                Session["TreatmentProg"] = theExistDS.Tables[0].Rows[0]["ProgID"].ToString();

            }
            else
            {
                Session["TreatmentProg"] = "";
            }
            //rupesh 19-sep-07 for ARV Provider
            ddlProvider.SelectedValue = Convert.ToString(theExistDS.Tables[0].Rows[0]["ProviderID"].ToString());
            if (theExistDS.Tables[0].Rows[0]["OrderedByDate"] != System.DBNull.Value)
            {
                DateTime theOrderedByDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["OrderedByDate"].ToString());
                txtpharmOrderedbyDate.Value = theOrderedByDate.ToString(Session["AppDateFormat"].ToString());
            }
            //txtpharmOrderedbyDate.Attributes.Add("readonly", "true");



            //DateTime theReportedbyDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["DispensedByDate"]);
            if (theExistDS.Tables[0].Rows[0]["DispensedByDate"].ToString() != "")
            {
                DateTime theReportedbyDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["DispensedByDate"]);
                txtpharmReportedbyDate.Value = theReportedbyDate.ToString(Session["AppDateFormat"].ToString());
            }

            if (theExistDS.Tables[0].Rows[0]["PharmacyNotes"].ToString() != "")
            {
                txtClinicalNotes.Text = theExistDS.Tables[0].Rows[0]["PharmacyNotes"].ToString();
            }

            //txtpharmReportedbyDate.Value = theReportedbyDate.ToString(Session["AppDateFormat"].ToString());
            //txtpharmReportedbyDate.Attributes.Add("readonly", "true"); 
            string theSign = theExistDS.Tables[0].Rows[0]["Signature"].ToString();

            if (theExistDS.Tables[0].Rows[0]["EmployeeID"].ToString() != "0")
            {
                //ddlCounselerName.Visible = true;
                //ddlCounselerName.SelectedValue = theExistDS.Tables[0].Rows[0]["EmployeeID"].ToString();
                //lblCounselorName.Visible = true;
                //txtCounselorName.Visible = true;
                //txtCounselorName.Text = ddlCounselerName.SelectedItem.Text;
                //   ddlPharmSignature.SelectedIndex = 3;//---Rupesh : 31Jan08 : Signature was not saving 

            }
            else if (theExistDS.Tables[0].Rows[0]["EmployeeID"].ToString() == "0" && theSign == "1")
            {
                //   ddlPharmSignature.SelectedIndex = 2;//---Rupesh : 31Jan08 : Signature was not saving 
                //ddlCounselerName.Visible = false;
                //lblCounselorName.Visible = false;
                //txtCounselorName.Visible = false;

            }
            else
            {
                //  ddlPharmSignature.SelectedIndex = 1; //---Rupesh : 31Jan08 : Signature was not saving 
            }
            //---Rupesh : 31Jan08 : Signature was not saving 


            #region "CreateAdditional Controls"

            #region "TableCreation"
            DataTable theDT1 = new DataTable();
            theDT1.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
            theDT1.Columns.Add("DrugName", System.Type.GetType("System.String"));
            theDT1.Columns.Add("Generic", System.Type.GetType("System.Int32"));
            theDT1.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
            AddARV = theDT1.Copy();
            OtherDrugs = theDT1.Copy();
            TBDrugs = theDT1.Copy();
            OIDrugs = theDT1.Copy();
            //NonARVDrugs = theDT1.Copy();
            #endregion

            foreach (DataRow theDR in theExistDS.Tables[0].Rows)
            {
                if (theDR["Drug_Pk"] != System.DBNull.Value)
                {
                    DataView theDV = new DataView(theDS.Tables[12]);//rupesh for showing inactive drug
                    theDV.RowFilter = "Drug_Pk = " + theDR["Drug_Pk"].ToString();
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
                        else if (Convert.ToInt32(theDV[0]["DrugTypeId"]) == 31)
                        {
                            DataRow DR = TBDrugs.NewRow();
                            DR[0] = theDR["Drug_Pk"];
                            DR[1] = theDV[0]["DrugName"];
                            DR[2] = 0;
                            TBDrugs.Rows.Add(DR);
                        }
                        else if (Convert.ToInt32(theDV[0]["DrugTypeId"]) == 36)
                        {
                            DataRow DR = OIDrugs.NewRow();
                            DR[0] = theDR["Drug_Pk"];
                            DR[1] = theDV[0]["DrugName"];
                            DR[2] = 0;
                            OIDrugs.Rows.Add(DR);
                        }
                        else
                        {
                            //if (Convert.ToInt32(theDV[0]["DrugTypeId"]) != 60)
                            //{
                            DataRow DR = OtherDrugs.NewRow();
                            DR[0] = theDR["Drug_Pk"];
                            DR[1] = theDV[0]["DrugName"];
                            DR[2] = 0;
                            OtherDrugs.Rows.Add(DR);
                            //}
                        }
                    }
                    //}
                    //}
                }
            }
            //LoadAdditionalDrugs(AddARV, PnlAdARV);
            LoadAdditionalDrugs(AddARV, pnlPedia);
            LoadAdditionalDrugs(OtherDrugs, PnlOtMed);
            LoadAdditionalDrugs(TBDrugs, pnlOtherTBMedicaton);
            LoadAdditionalDrugs(OIDrugs, PnlOIARV);
            //LoadAdditionalDrugs(NonARVDrugs, PnlNonArv);
            #endregion
            //  divAddARV.Visible = false;
            //  divAddOI.Visible = false;

            foreach (DataRow dr in theExistDS.Tables[0].Rows)
            {
                FillOldData(pnlPedia, dr, true);
                FillOldData(PnlOIARV, dr, true);
                FillOldData(PnlRegiment, dr,true);
                FillOldData(PnlOtMed, dr, false);
                FillOldData(pnlOtherTBMedicaton, dr, false);
                //if (dr["Drug_Pk"].ToString() != "0")
                //    FillOldFixedData(PnlFixed, dr, true);
            }
            //BindChkImmunizations(theExistDS.Tables[0]);
        }
        catch (Exception er)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = er.Message.ToString();
            IQCareMsgBox.Show("C1#", theBuilder, this);
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
    //private void BindChkImmunizations(DataTable dt)
    //{
    //    int DrugId = 0, GenericId=0;

    //    foreach (TableRow tr in tblInfantHealth.Rows)
    //    {
    //        foreach (TableCell tc in tr.Cells)
    //        {
    //            foreach (Control ct in tc.Controls)
    //            {

    //                if (ct.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputCheckBox))
    //                {
    //                    foreach (DataRow DR in dt.Rows)
    //                    {
    //                        if (((HtmlInputCheckBox)ct).ID.Remove(3,2) == Convert.ToString(DR["Drug_pk"]))
    //                        {
    //                            DrugId = Convert.ToInt32(((HtmlInputCheckBox)ct).ID.Remove(3, 2));
    //                            ((HtmlInputCheckBox)ct).Checked = true;
    //                        }

    //                        else if (((HtmlInputCheckBox)ct).ID.Remove(3, 2) == Convert.ToString(DR["GenericId"]))
    //                        {
    //                            GenericId = Convert.ToInt32(((HtmlInputCheckBox)ct).ID.Remove(3, 2));
    //                            ((HtmlInputCheckBox)ct).Checked = true;
    //                        }
    //                    }
    //                }
    //                if (ct.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
    //                {
    //                    if (DrugId == Convert.ToInt32(((DropDownList)ct).ID.Substring(3, 3)))
    //                    {
    //                        foreach (DataRow DR in dt.Rows)
    //                        {
    //                            if (DrugId == Convert.ToInt32(DR["Drug_pk"]))
    //                            {
    //                                ((DropDownList)ct).SelectedValue = Convert.ToString(DR["DrugSchedule"]);
    //                            }
    //                        }
    //                    }
    //                    else if (GenericId == Convert.ToInt32(((DropDownList)ct).ID.Substring(3, 3)))
    //                    {
    //                        foreach (DataRow DR in dt.Rows)
    //                        {
    //                            if (GenericId == Convert.ToInt32(DR["GenericId"]))
    //                            {
    //                                ((DropDownList)ct).SelectedValue = Convert.ToString(DR["DrugSchedule"]);
    //                            }
    //                        }

    //                    }

    //                }
    //            }
    //        }
    //    }

    //}
    private void FillOldData(Control Cntrl, DataRow theDR, Boolean fillTotDose)
    {
        if (theDR["Drug_Pk"] != System.DBNull.Value)
        {
            int y = 0;
            int DrugId;
            string FrequencyNm = string.Empty;
            Int32 Frequency = 0;
            Int32 ipos;

            if (Convert.ToInt32(theDR["Drug_Pk"]) == 0)
            {
                DrugId = Convert.ToInt32(theDR["GenericId"]);
            }
            else
            {
                DrugId = Convert.ToInt32(theDR["Drug_Pk"]);

            }
            foreach (Control x in Cntrl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                {
                    //FillOldData(x, theDR); // 27Feb08
                    if (fillTotDose == true)
                        FillOldData(x, theDR, true);
                    else
                        FillOldData(x, theDR, false);
                }
                else
                {
                    if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                    {
                        if (x.ID.StartsWith("DDRegimenLine"))
                        {
                            ((DropDownList)x).SelectedValue = theDR["RegimenLine"].ToString();
                        }

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
                                if (Convert.ToInt32(theDR["StrengthId"]) > 0)
                                {
                                    ((DropDownList)x).SelectedValue = theDR["StrengthId"].ToString();
                                }
                                else if (Convert.ToInt32(theDR["UnitId"]) > 0)
                                {
                                    ((DropDownList)x).SelectedValue = theDR["UnitId"].ToString();
                                }
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
                                FrequencyNm = ((DropDownList)x).SelectedItem.ToString();
                            }
                        }
                        if (x.ID.StartsWith("drgTreatmenPhase"))
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

                            if (y == DrugId)
                            {
                                ((DropDownList)x).SelectedValue = theDR["TreatmentPhase"].ToString();
                            }
                        }
                        if (x.ID.StartsWith("drgTreatmenMonth"))
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

                            if (y == DrugId)
                            {
                                ((DropDownList)x).SelectedValue = theDR["Month"].ToString();
                            }
                        }
                    }
                    if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                    {
                        if (x.ID.StartsWith("drgTotalDose"))
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
                                if (FrequencyNm == "OD")
                                {
                                    Frequency = 1;
                                }
                                else if (FrequencyNm == "BD")
                                {
                                    Frequency = 2;
                                }
                                else if (FrequencyNm == "1OD")
                                {
                                    Frequency = 1;
                                }
                                else if (FrequencyNm == "2OD")
                                {
                                    Frequency = 2;
                                }
                                else if (FrequencyNm == "1BD")
                                {
                                    Frequency = 2;
                                }
                                else if (FrequencyNm == "3OD" || FrequencyNm == "TD")
                                {
                                    Frequency = 3;
                                }
                                else if (FrequencyNm == "qid")
                                {
                                    Frequency = 4;
                                }
                                if (theDR["SingleDose"].ToString() != "")
                                {
                                    int DecPos = theDR["SingleDose"].ToString().IndexOf(".");
                                    //int DecValue = Convert.ToInt32(theDR["SingleDose"].ToString().Substring(DecPos + 1, 2));
                                    #region "14-Jun-07 -2 "
                                    //int DecValue = Convert.ToInt32(theDR["SingleDose"].ToString().Substring(DecPos + 1, theDR["SingleDose"].ToString().Trim().Length));
                                    int DecValue = Convert.ToInt32(theDR["SingleDose"].ToString().Substring(DecPos + 1, theDR["SingleDose"].ToString().Trim().Length - (DecPos + 1)));
                                    #endregion
                                    if (DecValue > 0)
                                    {
                                        //((TextBox)x).Text = theDR["SingleDose"].ToString();
                                        if (fillTotDose == true)
                                            ((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToDecimal(theDR["SingleDose"].ToString()));
                                        else
                                            ((TextBox)x).Text = theDR["TotDailyDose"].ToString();
                                    }
                                    else
                                    {
                                        if (fillTotDose == true)
                                            ((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToInt32(theDR["SingleDose"]));
                                        else
                                            ((TextBox)x).Text = theDR["TotDailyDose"].ToString();
                                    }
                                    //((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToInt32(theDR["SingleDose"].ToString()));
                                }
                                else
                                {
                                    ((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToInt32(theDR["Dose"].ToString()));
                                }
                                FrequencyNm = string.Empty;
                            }
                        }
                        if (x.ID.StartsWith("drgDose"))
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
                                int DecPos = theDR["SingleDose"].ToString().IndexOf(".");
                                if (DecPos != -1)
                                {
                                    //int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    decimal DecValue = Convert.ToDecimal(theDR["SingleDose"].ToString().Substring(DecPos + 1, 2));
                                    if (DecValue > 0)
                                    {
                                        ((TextBox)x).Text = theDR["SingleDose"].ToString();

                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["SingleDose"]));
                                    }
                                }
                                else
                                {
                                    ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["SingleDose"]));
                                }

                            }
                        }
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
                            //y = Convert.ToInt32(x.ID.Substring(7, x.ID.Length - 7));
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
                                int DecPos = theDR["Duration"].ToString().IndexOf(".");
                                if (DecPos != -1)
                                {
                                    //int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    decimal DecValue = Convert.ToDecimal(theDR["Duration"].ToString().Substring(DecPos + 1, 2));
                                    if (DecValue > 0)
                                    {
                                        ((TextBox)x).Text = theDR["Duration"].ToString();

                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["Duration"]));
                                    }
                                }
                                else
                                {
                                    if (theDR["Duration"] != System.DBNull.Value)
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["Duration"]));
                                    }
                                }
                                //((TextBox)x).Text = theDR["Duration"].ToString();
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
                                //rupesh
                                //int DecValue = Convert.ToInt32(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1, 2));

                                if (DecPos != -1)
                                {
                                    decimal DecValue = Convert.ToDecimal(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    if (DecValue > 0)
                                    {
                                        ((TextBox)x).Text = theDR["OrderedQuantity"].ToString();

                                    }
                                    else
                                    {
                                        if (theDR["Duration"] != System.DBNull.Value)
                                        {
                                            ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["OrderedQuantity"]));
                                        }
                                    }
                                }
                                else
                                {
                                    if (theDR["Duration"] != System.DBNull.Value)
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["OrderedQuantity"]));
                                    }
                                }
                                //((TextBox)x).Text = theDR["OrderedQuantity"].ToString();
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
                                if (DecPos != -1)
                                {

                                    decimal DecValue = Convert.ToDecimal(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    if (theDR["DispensedQuantity"].ToString() != "0.00")
                                    {
                                        if (DecValue > 0)
                                        {
                                            ((TextBox)x).Text = theDR["DispensedQuantity"].ToString();

                                        }
                                        else
                                        {
                                            ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                        }
                                    }
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
                    if (x.GetType() == typeof(System.Web.UI.WebControls.LinkButton))
                    {
                        if (x.ID.StartsWith("lnkrmv"))
                        {
                            ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                            if (ipos > 0)
                            {
                                ipos = ipos + 1;
                                y = Convert.ToInt32(x.ID.Substring(ipos, x.ID.Length - ipos));
                            }
                            //else
                            //{
                            //    y = Convert.ToInt32(x.ID.Substring(5, x.ID.Length - 5));
                            //}
                            if (y == DrugId)
                            {
                                ((LinkButton)x).Visible = false;
                            }
                        }

                    }
                }
            }
        }
    }

    private void FillOldFixedData(Control Cntrl, DataRow theDR, Boolean fillTotDose)
    {

        int DrugId;
        string FrequencyNm = string.Empty;
        Int32 Frequency = 0;


        if (Convert.ToInt32(theDR["Drug_Pk"]) != 0)
        {
            foreach (DataRow theDRFixDose in theDS.Tables[23].Rows)
            {
                if (Convert.ToInt32(theDRFixDose["drug_pk"]) == Convert.ToInt32(theDR["drug_pk"]))
                {
                    DrugId = Convert.ToInt32(theDR["Drug_Pk"]);
                    foreach (Control x in Cntrl.Controls)
                    {
                        if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            //FillOldData(x, theDR); // 27Feb08
                            if (fillTotDose == true)
                                FillOldFixedData(x, theDR, true);
                            else
                                FillOldFixedData(x, theDR, false);
                        }
                        else
                        {
                            if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                            {
                                if (x.ID == "theFixedDrugName")
                                {
                                    ((DropDownList)x).SelectedValue = DrugId.ToString();
                                    EventArgs s = new EventArgs();
                                    ddlFixDrugname_SelectedChanged((DropDownList)x, s);


                                }
                                if (x.ID == "theFixedDrugStrength")
                                {
                                    if (Convert.ToInt32(theDR["StrengthId"]) > 0)
                                    {
                                        ((DropDownList)x).SelectedValue = theDR["StrengthId"].ToString();
                                    }
                                    else if (Convert.ToInt32(theDR["UnitId"]) > 0)
                                    {
                                        ((DropDownList)x).SelectedValue = theDR["UnitId"].ToString();
                                    }

                                }
                                if (x.ID.StartsWith("theUnit"))
                                {
                                    ((DropDownList)x).SelectedValue = theDR["UnitId"].ToString();

                                }
                                if (x.ID == "drgFixFrequency")
                                {
                                    ((DropDownList)x).SelectedValue = theDR["FrequencyId"].ToString();
                                    FrequencyNm = ((DropDownList)x).SelectedItem.ToString();

                                }
                                if (x.ID.StartsWith("drgTreatmenPhase"))
                                {
                                    ((DropDownList)x).SelectedValue = theDR["TreatmentPhase"].ToString();

                                }
                                if (x.ID.StartsWith("drgTreatmenMonth"))
                                {
                                    ((DropDownList)x).SelectedValue = theDR["Month"].ToString();

                                }
                            }
                            if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                            {
                                if (x.ID == "drgFixTotalDose")
                                {
                                    if (FrequencyNm == "OD")
                                    {
                                        Frequency = 1;
                                    }
                                    else if (FrequencyNm == "BD")
                                    {
                                        Frequency = 2;
                                    }
                                    else if (FrequencyNm == "1OD")
                                    {
                                        Frequency = 1;
                                    }
                                    else if (FrequencyNm == "2OD")
                                    {
                                        Frequency = 2;
                                    }
                                    else if (FrequencyNm == "1BD")
                                    {
                                        Frequency = 2;
                                    }
                                    else if (FrequencyNm == "3OD" || FrequencyNm == "TD")
                                    {
                                        Frequency = 3;
                                    }
                                    else if (FrequencyNm == "qid")
                                    {
                                        Frequency = 4;
                                    }
                                    if (theDR["SingleDose"].ToString() != "")
                                    {
                                        int DecPos = theDR["SingleDose"].ToString().IndexOf(".");
                                        //int DecValue = Convert.ToInt32(theDR["SingleDose"].ToString().Substring(DecPos + 1, 2));
                                        #region "14-Jun-07 -2 "
                                        //int DecValue = Convert.ToInt32(theDR["SingleDose"].ToString().Substring(DecPos + 1, theDR["SingleDose"].ToString().Trim().Length));
                                        int DecValue = Convert.ToInt32(theDR["SingleDose"].ToString().Substring(DecPos + 1, theDR["SingleDose"].ToString().Trim().Length - (DecPos + 1)));
                                        #endregion
                                        if (DecValue > 0)
                                        {
                                            //((TextBox)x).Text = theDR["SingleDose"].ToString();
                                            if (fillTotDose == true)
                                                ((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToDecimal(theDR["SingleDose"].ToString()));
                                            else
                                                ((TextBox)x).Text = theDR["TotDailyDose"].ToString();
                                        }
                                        else
                                        {
                                            if (fillTotDose == true)
                                                ((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToInt32(theDR["SingleDose"]));
                                            else
                                                ((TextBox)x).Text = theDR["TotDailyDose"].ToString();
                                        }
                                        //((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToInt32(theDR["SingleDose"].ToString()));
                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Frequency * Convert.ToInt32(theDR["Dose"].ToString()));
                                    }
                                    FrequencyNm = string.Empty;

                                }
                                if (x.ID == "drgFixDose")
                                {

                                    int DecPos = theDR["SingleDose"].ToString().IndexOf(".");
                                    if (DecPos != -1)
                                    {
                                        //int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                        decimal DecValue = Convert.ToDecimal(theDR["SingleDose"].ToString().Substring(DecPos + 1, 2));
                                        if (DecValue > 0)
                                        {
                                            ((TextBox)x).Text = theDR["SingleDose"].ToString();

                                        }
                                        else
                                        {
                                            ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["SingleDose"]));
                                        }
                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["SingleDose"]));
                                    }


                                }
                                if (x.ID.StartsWith("theDose"))
                                {
                                    ((TextBox)x).Text = theDR["Dose"].ToString();

                                }
                                if (x.ID == "drgFixDuration")
                                {

                                    int DecPos = theDR["Duration"].ToString().IndexOf(".");
                                    if (DecPos != -1)
                                    {
                                        //int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                        decimal DecValue = Convert.ToDecimal(theDR["Duration"].ToString().Substring(DecPos + 1, 2));
                                        if (DecValue > 0)
                                        {
                                            ((TextBox)x).Text = theDR["Duration"].ToString();

                                        }
                                        else
                                        {
                                            ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["Duration"]));
                                        }
                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["Duration"]));
                                    }
                                    //((TextBox)x).Text = theDR["Duration"].ToString();

                                }
                                if (x.ID == "drgFixQtyPrescribed")
                                {
                                    int DecPos = theDR["OrderedQuantity"].ToString().IndexOf(".");
                                    //rupesh
                                    //int DecValue = Convert.ToInt32(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1, 2));

                                    if (DecPos != -1)
                                    {
                                        decimal DecValue = Convert.ToDecimal(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1, 2));
                                        if (DecValue > 0)
                                        {
                                            ((TextBox)x).Text = theDR["OrderedQuantity"].ToString();

                                        }
                                        else
                                        {
                                            ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["OrderedQuantity"]));
                                        }
                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["OrderedQuantity"]));
                                    }
                                    //((TextBox)x).Text = theDR["OrderedQuantity"].ToString();

                                }
                                if (x.ID == "drgFixQtyDispensed")
                                {
                                    int DecPos = theDR["DispensedQuantity"].ToString().IndexOf(".");
                                    //int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    if (DecPos != -1)
                                    {

                                        decimal DecValue = Convert.ToDecimal(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                        if (theDR["DispensedQuantity"].ToString() != "0.00")
                                        {
                                            if (DecValue > 0)
                                            {
                                                ((TextBox)x).Text = theDR["DispensedQuantity"].ToString();

                                            }
                                            else
                                            {
                                                ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                    }
                                    //((TextBox)x).Text = theDR["DispensedQuantity"].ToString();

                                }
                            }

                            if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                            {
                                if (x.ID.StartsWith("FinChk"))
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
                                if (x.ID == "chkFixProphylaxis")
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

    private bool ValidateFixedCombData(Control Cntrl)
    {

        foreach (Control x in Cntrl.Controls)
        {
            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
            {
                return ValidateFixedCombData(x);
            }
            else
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList) && x.ID == "theFixedDrugName")
                {
                    if (Convert.ToInt32(((DropDownList)x).SelectedValue) > 0)
                    {
                        if (CountFixedComb == 0)
                        {
                            return false;
                        }

                    }

                }

            }
        }

        return true;
    }

    //private void LoadAdditionalDrugs(DataTable theDT, Panel thePanel)
    //{
    //    thePanel.Controls.Clear();
    //    if (thePanel.ID == "pnlPedia")
    //    {
    //        int theDrugId;

    //        string pnlName = "pnlPedia";

    //        Label lblSpace0 = new Label();
    //        lblSpace0.Width = 35;
    //        lblSpace0.ID = "lblSpace_0";
    //        lblSpace0.Text = "";


    //        Label lblDrugName = new Label();
    //        lblDrugName.Text = "Drug Name";
    //        lblDrugName.ID = "lblDrugName";
    //        lblDrugName.Font.Bold = true;
    //        lblDrugName.Visible = true;

    //        Label lblSpace = new Label();
    //        lblSpace.Width = 184;
    //        lblSpace.ID = "lblSpace";
    //        lblSpace.Text = "";

    //        //Label lblFormulation = new Label();
    //        //lblFormulation.Text = "Formulation";
    //        //lblFormulation.ID = "lblFormulation";
    //        //lblFormulation.Font.Bold = true;
    //        //lblFormulation.Visible = true;

    //        Label lblSpace1 = new Label();
    //        lblSpace1.Width = 22;
    //        lblSpace1.ID = "lblSpace_1";
    //        lblSpace1.Text = "";

    //        Label lblDose = new Label();
    //        lblDose.Text = "Single Dose";
    //        lblDose.ID = "lblDose";
    //        lblDose.Font.Bold = true;
    //        lblDose.Visible = true;

    //        Label lblSpace2 = new Label();
    //        lblSpace2.Width = 30;
    //        lblSpace2.ID = "lblSpace_2";
    //        lblSpace2.Text = "";

    //        Label lblFrequency = new Label();
    //        lblFrequency.Text = "Frequency";
    //        lblFrequency.ID = "lblFrequency";
    //        lblFrequency.Font.Bold = true;
    //        lblFrequency.Visible = true;

    //        //Label lblSpace3 = new Label();
    //        //lblSpace3.Width = 30;
    //        //lblSpace3.ID = "lblSpace_3";
    //        //lblSpace3.Text = "";

    //        //Label lblTotalDose = new Label();
    //        //lblTotalDose.Text = "Total Daily Dose";
    //        //lblTotalDose.ID = "lblTotalDose";
    //        //lblTotalDose.Font.Bold = true;
    //        //lblTotalDose.Visible = true;

    //        Label lblSpace4 = new Label();
    //        lblSpace4.Width = 40;
    //        lblSpace4.ID = "lblSpace_4";
    //        lblSpace4.Text = "";

    //        Label lblDuration = new Label();
    //        lblDuration.Text = "Duration";
    //        lblDuration.ID = "lblDuration";
    //        lblDuration.Font.Bold = true;
    //        lblDuration.Visible = true;

    //        Label lblSpace5 = new Label();
    //        lblSpace5.Width = 40;
    //        lblSpace5.ID = "lblSpace_5";
    //        lblSpace5.Text = "";

    //        Label lblQtyPrescribed = new Label();
    //        lblQtyPrescribed.Text = "Qty Prescribed";
    //        lblQtyPrescribed.ID = "lblQuantityPres";
    //        lblQtyPrescribed.Font.Bold = true;
    //        lblQtyPrescribed.Visible = true;

    //        Label lblSpace6 = new Label();
    //        lblSpace6.Width = 20;
    //        lblSpace6.ID = "lblSpace_6";
    //        lblSpace6.Text = "";

    //        Label lblQtyDispensed = new Label();
    //        lblQtyDispensed.Text = "Qty Dispensed";
    //        lblQtyDispensed.ID = "lblQuantityDisp";
    //        lblQtyDispensed.Font.Bold = true;
    //        lblQtyDispensed.Visible = true;


    //        Label lblSpace7 = new Label();
    //        lblSpace7.Width = 20;
    //        lblSpace7.ID = "lblSpace_10";
    //        lblSpace7.Text = "";

    //        Label lblProphylaxis = new Label();
    //        lblProphylaxis.Text = "Prophylaxis";
    //        lblProphylaxis.ID = "lblProphylaxis";
    //        lblProphylaxis.Font.Bold = true;
    //        lblProphylaxis.Visible = true;

    //        pnlPedia.Controls.Add(lblDrugName);
    //        pnlPedia.Controls.Add(lblSpace);
    //        //pnlPedia.Controls.Add(lblFormulation);
    //        pnlPedia.Controls.Add(lblSpace1);
    //        //pnlPedia.Controls.Add(lblDose);
    //        //pnlPedia.Controls.Add(lblSpace2);
    //        pnlPedia.Controls.Add(lblFrequency);
    //        //pnlPedia.Controls.Add(lblSpace3);
    //        //pnlPedia.Controls.Add(lblTotalDose);
    //        pnlPedia.Controls.Add(lblSpace4);
    //        pnlPedia.Controls.Add(lblDuration);
    //        pnlPedia.Controls.Add(lblSpace5);
    //        pnlPedia.Controls.Add(lblQtyPrescribed);
    //        pnlPedia.Controls.Add(lblSpace6);
    //        pnlPedia.Controls.Add(lblQtyDispensed);
    //        pnlPedia.Controls.Add(lblSpace7);
    //        pnlPedia.Controls.Add(lblProphylaxis);
    //    }
    //    foreach (DataRow theDR in theDT.Rows)
    //    {
    //        //if (thePanel.ID == "PnlAdARV")
    //        if (thePanel.ID == "pnlPedia")
    //        {
    //            //Boolean Flag = true;
    //            //if (theDS != null)
    //            //{
    //            //foreach (DataRow theDRFixDose in ((DataSet)ViewState["MasterData"]).Tables[23].Rows)
    //            //    {
    //            //        if (Convert.ToInt32(theDRFixDose["drug_pk"]) == Convert.ToInt32(theDR["drugId"]))
    //            //        {
    //            //            Flag = false;
    //            //        }
    //            //    }
    //            //    if (Flag == true)
    //            //    {
    //                    BindCustomControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);
    //               // }
    //            //}
    //        }
    //        else if (thePanel.ID == "pnlOtherTBMedicaton")
    //        {
    //            BindTBDrugControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);
    //        }
    //        else if (thePanel.ID == "PnlOtMed")
    //        {
    //            BindAdditionalDrugControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);
    //        }
    //        else if (thePanel.ID == "PnlNonArv")
    //        {
    //            BindAdditionalDrugControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);
    //        }
    //    }
    //}
    private void LoadAdditionalDrugs(DataTable theDT, Panel thePanel)
    {
        //thePanel.Controls.Clear();
        if (theDT.Rows.Count > 0)
        {
            if (thePanel.ID == "pnlPedia")
            {
                pnlARV.Visible = true;

            }
            else if (thePanel.ID == "pnlOtherTBMedicaton")
            {
                pnlTB.Visible = true;
            }
            else if (thePanel.ID == "PnlOIARV")
            {
                pnlOI.Visible = true;
            }
            else if (thePanel.ID == "PnlOtMed")
            {
                pnlOther.Visible = true;

            }
        }
        #region "ARV Drugs Heading"
        //if (thePanel.ID == "pnlPedia" && theDT.Rows.Count > 0)
        //{
        //    Control theFndCntrl = FindControlRecursive(thePanel, "pnlARVDrug");
        //    if (theFndCntrl == null)
        //    {
        //        Panel thelblPnl = new Panel();
        //        thelblPnl.ID = "pnlARVDrug";
        //        thelblPnl.Height = 20;
        //        thelblPnl.Width = 900;
        //        thelblPnl.Controls.Clear();

        //        Label theLabel = new Label();
        //        theLabel.ID = "lblARVDrug";
        //        theLabel.Text = "ARV Medications";//earlier it was "OI Treatment and Other Medications";
        //        theLabel.Font.Bold = true;
        //        thelblPnl.Controls.Add(theLabel);
        //        thePanel.Controls.Add(thelblPnl);

        //        Label lblDrugName = new Label();
        //        lblDrugName.Text = "Drug Name";
        //        lblDrugName.ID = "lblDrugName";
        //        lblDrugName.Font.Bold = true;
        //        lblDrugName.Visible = true;
        //        lblDrugName.Width = 200;

        //        Label lblSpace = new Label();
        //        lblSpace.Width = 15;
        //        //lblSpace.Width = 35;
        //        lblSpace.ID = "lblSpace_1";
        //        lblSpace.Text = "";

        //        //Label lblStrength = new Label();
        //        //lblStrength.Text = "Dose";
        //        //lblStrength.ID = "lblStrength";
        //        //lblStrength.Font.Bold = true;
        //        //lblStrength.Visible = true;

        //        //Label lblSpace1 = new Label();
        //        //lblSpace1.Width = 40;
        //        //lblSpace1.ID = "lblSpace_2";
        //        //lblSpace1.Text = "";

        //        Label lblFrequency = new Label();
        //        lblFrequency.Text = "Frequency";
        //        lblFrequency.ID = "lblFrequency";
        //        lblFrequency.Font.Bold = true;
        //        lblFrequency.Visible = true;

        //        Label lblSpace2 = new Label();
        //        lblSpace2.Width = 38;
        //        lblSpace2.ID = "lblSpace_3";
        //        lblSpace2.Text = "";

        //        Label lblDuration = new Label();
        //        lblDuration.Text = "Duration";
        //        lblDuration.ID = "lblDuration";
        //        lblDuration.Font.Bold = true;
        //        lblDuration.Visible = true;

        //        Label lblSpace3 = new Label();
        //        lblSpace3.Width = 48;
        //        lblSpace3.ID = "lblSpace_4";
        //        lblSpace3.Text = "";

        //        Label lblQtyPrescribed = new Label();
        //        lblQtyPrescribed.Text = "Qty. Prescribed";
        //        lblQtyPrescribed.ID = "lblQuantityPres";
        //        lblQtyPrescribed.Font.Bold = true;
        //        lblQtyPrescribed.Visible = true;

        //        Label lblSpace4 = new Label();
        //        lblSpace4.Width = 12;
        //        lblSpace4.ID = "lblSpace_5";
        //        lblSpace4.Text = "";

        //        Label lblQtyDispensed = new Label();
        //        lblQtyDispensed.Text = "Qty. Dispensed";
        //        lblQtyDispensed.ID = "lblQuantityDisp";
        //        lblQtyDispensed.Font.Bold = true;
        //        lblQtyDispensed.Visible = true;

        //        Label lblSpace5 = new Label();
        //        lblSpace5.Width = 20;
        //        lblSpace5.ID = "lblSpace_6";
        //        lblSpace5.Text = "";

        //        Label lblProphylaxis = new Label();
        //        lblProphylaxis.Text = "Prophylaxis";
        //        lblProphylaxis.ID = "lblProphylaxis";
        //        lblProphylaxis.Font.Bold = true;
        //        lblProphylaxis.Visible = true;

        //        Label lblSpace6 = new Label();
        //        lblSpace6.Width = 5;
        //        lblSpace6.ID = "lblSpace_7";
        //        lblSpace6.Text = "";



        //        Panel theHeaderPanel = new Panel();
        //        theHeaderPanel.ID = "Header";
        //        theHeaderPanel.Width = 900;
        //        theHeaderPanel.Height = 5;

        //        Label lblSp = new Label();
        //        lblSp.Width = 10;
        //        lblSp.ID = "stSpace";
        //        lblSp.Text = "";
        //        theHeaderPanel.Controls.Add(lblSp);
        //        theHeaderPanel.Controls.Add(lblSpace);
        //        theHeaderPanel.Controls.Add(lblDrugName);
        //        theHeaderPanel.Controls.Add(lblSpace);
        //        //theHeaderPanel.Controls.Add(lblStrength);
        //        //theHeaderPanel.Controls.Add(lblSpace1);
        //        theHeaderPanel.Controls.Add(lblFrequency);
        //        theHeaderPanel.Controls.Add(lblSpace2);
        //        theHeaderPanel.Controls.Add(lblDuration);
        //        theHeaderPanel.Controls.Add(lblSpace3);
        //        theHeaderPanel.Controls.Add(lblQtyPrescribed);
        //        theHeaderPanel.Controls.Add(lblSpace4);
        //        theHeaderPanel.Controls.Add(lblQtyDispensed);
        //        theHeaderPanel.Controls.Add(lblSpace5);
        //        theHeaderPanel.Controls.Add(lblProphylaxis);
        //        theHeaderPanel.Controls.Add(lblSpace6);
        //        pnlPedia.Controls.Add(theHeaderPanel);
        //    }
        //}
        #endregion
        foreach (DataRow theDR in theDT.Rows)
        {
            //if (thePanel.ID == "PnlAddARV")
            if (thePanel.ID == "pnlPedia")
            {
                BindCustomControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);

            }
            else if (thePanel.ID == "pnlOtherTBMedicaton")
            {
                BindTBDrugControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);
            }
            else
            {
                BindAdditionalDrugControls(Convert.ToInt32(theDR[0]), Convert.ToInt32(theDR[2]), thePanel);

            }
        }

    }
    void Dose_Load(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;
        tbox.MaxLength = 8;
        Int32 ipos = Convert.ToInt32(tbox.ID.IndexOf("^"));
        int DrugId = Convert.ToInt32(tbox.ID.Substring(7, ipos - 7));
        //int DrugId = Convert.ToInt32(tbox.ID.Substring(7, tbox.ID.Length - 7));
        DataView theDV = new DataView(((DataSet)ViewState["MasterData"]).Tables[9]);
        if (Convert.ToDecimal(tbox.Text) == 0)
        {
            theDV.RowFilter = "GenericId= " + DrugId.ToString();
        }
        else
        {
            theDV.RowFilter = "DrugId= " + DrugId.ToString();
        }
        if (ViewState["PharmacyId"] == null)
            tbox.Text = "";
        if (theDV.Count > 0)
        {
            tbox.Attributes.Add("onkeyup", "chkNumeric('" + tbox.ClientID + "'); AddBoundary('" + tbox.ClientID + "','" + theDV[0]["MinDose"] + "','" + theDV[0]["MaxDose"] + "')");
        }
        else
        {
            tbox.Attributes.Add("onkeyup", "chkNumeric('" + tbox.ClientID + "'); AddBoundary('" + tbox.ClientID + "','0','99999999')");
        }

    }
    private DataTable MakeTreatmentPhase()
    {
        DataTable theDT = new DataTable();
        theDT.Columns.Add("Id", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Name", System.Type.GetType("System.String"));
        DataRow DR1 = theDT.NewRow();
        DR1[0] = 0;
        DR1[1] = "Select";
        theDT.Rows.Add(DR1);

        DataRow DR2 = theDT.NewRow();
        DR2[0] = 1;
        DR2[1] = "Intensive";
        theDT.Rows.Add(DR2);

        DataRow DR3 = theDT.NewRow();
        DR3[0] = 2;
        DR3[1] = "Continue";
        theDT.Rows.Add(DR3);

        return theDT;


    }
    private DataTable MakeTreatmentMonth()
    {
        DataTable theDT = new DataTable();
        theDT.Columns.Add("Id", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Name", System.Type.GetType("System.String"));
        DataRow DR1 = theDT.NewRow();
        DR1[0] = 0;
        DR1[1] = "Select";
        theDT.Rows.Add(DR1);

        for (int i = 1; i <= 12; i++)
        {
            DataRow DR = theDT.NewRow();
            DR[0] = i;
            DR[1] = Convert.ToString(i);
            theDT.Rows.Add(DR);
        }





        return theDT;


    }
    private void Remove_panel(object s, EventArgs e)
    {
        LinkButton lnkremove = (LinkButton)s;
        //lnkremove.Attributes.Add("onclick", "return confirm('Are you sure you want to Remove this Drug?')");
        string lnkId = lnkremove.ID.ToString();
        int ipos = 0;
        int Drugid = 0;
        string pnlName = "";
        Control pnlremoving;
        Control pnlheading;
        if (lnkId.StartsWith("lnkrmv"))
        {
            ipos = Convert.ToInt32(lnkId.IndexOf("^"));
            if (ipos > 0)
            {
                ipos = ipos + 1;
                Drugid = Convert.ToInt32(lnkId.Substring(ipos, lnkId.Length - ipos));
            }
            ipos = Convert.ToInt32(lnkId.IndexOf("%"));
            if (ipos > 0)
            {
                ipos = ipos + 1;
                pnlName = lnkId.Substring(ipos, lnkId.IndexOf("^") - ipos);
            }
            #region "Additional ARV"
            if (pnlName.ToString() == "pnlPedia")
            {
                if ((DataTable)Session["AddARV"] != null)
                {
                    DataTable theDT = (DataTable)Session["AddARV"];
                    DataRow[] theDR = theDT.Select("DrugId = " + Drugid.ToString());

                    theDT.Rows.Remove(theDR[0]);
                    Session["AddARV"] = theDT;
                    pnlremoving = pnlPedia.FindControl("pnl_" + Drugid);
                    if (pnlremoving != null)
                    {
                        if (pnlremoving.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            pnlPedia.Controls.Remove(pnlremoving);
                        }
                    }
                    if (theDR[0] != null && theDT.Rows.Count == 0)
                    {
                        pnlheading = pnlPedia.FindControl("pnlARVDrug");
                        pnlPedia.Controls.Remove(pnlheading);
                        pnlheading = pnlPedia.FindControl("Header");
                        pnlPedia.Controls.Remove(pnlheading);
                        pnlARV.Visible = false;
                    }

                }
            }
            #endregion
            #region "TB Drugs"
            if (pnlName.ToString() == "pnlOtherTBMedicaton")
            {
                if ((DataTable)Session["AddTB"] != null)
                {
                    DataTable theDT = (DataTable)Session["AddTB"];
                    DataRow[] theDR = theDT.Select("DrugId = " + Drugid.ToString());

                    theDT.Rows.Remove(theDR[0]);
                    Session["AddTB"] = theDT;
                    pnlremoving = pnlOtherTBMedicaton.FindControl("pnl" + Drugid + "^0");
                    if (pnlremoving != null)
                    {
                        if (pnlremoving.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            pnlOtherTBMedicaton.Controls.Remove(pnlremoving);
                        }
                    }
                    if (theDR[0] != null && theDT.Rows.Count == 0)
                    {
                        pnlheading = pnlOtherTBMedicaton.FindControl("pnlTBDrug");
                        pnlOtherTBMedicaton.Controls.Remove(pnlheading);
                        pnlheading = pnlOtherTBMedicaton.FindControl("pnlHeaderTBDrug");
                        pnlOtherTBMedicaton.Controls.Remove(pnlheading);
                        pnlTB.Visible = false;
                    }

                }
            }
            #endregion
            #region "Additional OI and Other Medications"
            if (pnlName.ToString() == "PnlOtMed")
            {
                if ((DataTable)Session["OtherDrugs"] != null)
                {
                    DataTable theDT = (DataTable)Session["OtherDrugs"];
                    DataRow[] theDR = theDT.Select("DrugId = " + Drugid.ToString());

                    theDT.Rows.Remove(theDR[0]);
                    Session["OtherDrugs"] = theDT;
                    pnlremoving = PnlOtMed.FindControl("pnl" + Drugid + "^0");
                    if (pnlremoving != null)
                    {
                        if (pnlremoving.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            PnlOtMed.Controls.Remove(pnlremoving);
                        }
                    }
                    if (theDR[0] != null && theDT.Rows.Count == 0)
                    {
                        pnlheading = PnlOtMed.FindControl("pnlDrugPnlOtherMedication");
                        PnlOtMed.Controls.Remove(pnlheading);
                        pnlheading = PnlOtMed.FindControl("pnlHeaderOtherDrugPnlOtherMedication");
                        PnlOtMed.Controls.Remove(pnlheading);
                        pnlOther.Visible = false;
                    }

                }
            }
            #endregion
            #region "Additional OI"
            if (pnlName.ToString() == "PnlOIARV")
            {
                if ((DataTable)Session["OIDrugs"] != null)
                {
                    DataTable theDT = (DataTable)Session["OIDrugs"];
                    DataRow[] theDR = theDT.Select("DrugId = " + Drugid.ToString());

                    theDT.Rows.Remove(theDR[0]);
                    Session["OIDrugs"] = theDT;
                    pnlremoving = PnlOIARV.FindControl("pnl" + Drugid + "^0");
                    if (pnlremoving != null)
                    {
                        if (pnlremoving.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            PnlOIARV.Controls.Remove(pnlremoving);
                        }
                    }
                    if (theDR[0] != null && theDT.Rows.Count == 0)
                    {
                        pnlheading = PnlOIARV.FindControl("pnlDrugPnlOIARV");
                        PnlOIARV.Controls.Remove(pnlheading);
                        pnlheading = PnlOIARV.FindControl("pnlHeaderOtherDrugPnlOIARV");
                        PnlOIARV.Controls.Remove(pnlheading);
                        pnlOI.Visible = false;
                    }

                }
            }
            #endregion
            //Page_Load(s, e);


        }




    }
    //private void BindTBDrugControls(int DrugId, int Generic, Panel MstPanel)
    //{
    //    if (MstPanel.Controls.Count < 1)
    //    {
    //        #region "OI & Other Medication"
    //        Panel thelblPnl = new Panel();
    //        thelblPnl.ID = "pnlTBDrug";
    //        thelblPnl.Height = 20;
    //        thelblPnl.Width = 860;
    //        thelblPnl.Controls.Clear();

    //        Label theLabel = new Label();
    //        theLabel.ID = "lblTBDrug";
    //        theLabel.Text = "TB Medications";//earlier it was "OI Treatment and Other Medications";
    //        theLabel.Font.Bold = true;
    //        thelblPnl.Controls.Add(theLabel);
    //        MstPanel.Controls.Add(thelblPnl);

    //        /////////////////////////////////////////////////
    //        Panel theheaderPnl = new Panel();
    //        theheaderPnl.ID = "pnlHeaderTBDrug";
    //        theheaderPnl.Height = 20;
    //        theheaderPnl.Width = 860;
    //        theheaderPnl.Font.Bold = true;
    //        theheaderPnl.Controls.Clear();

    //        Label theSP = new Label();
    //        theSP.ID = "lblTBDrgSp";
    //        theSP.Width = 5;
    //        theSP.Text = "";
    //        theheaderPnl.Controls.Add(theSP);

    //        Label theLabel1 = new Label();
    //        theLabel1.ID = "lblTBDrgNm";
    //        theLabel1.Text = "Drug Name";
    //        theLabel1.Width = 240;
    //        theheaderPnl.Controls.Add(theLabel1);

    //        //Label theLabel2 = new Label();
    //        //theLabel2.ID = "lblTBDrgDose";
    //        //theLabel2.Text = "Dose";
    //        //theLabel2.Width = 50;
    //        //theheaderPnl.Controls.Add(theLabel2);


    //        Label theLabel4 = new Label();
    //        theLabel4.ID = "lblTBDrgFrequency";
    //        theLabel4.Text = "Frequency";
    //        theLabel4.Width = 110;
    //        theheaderPnl.Controls.Add(theLabel4);

    //        Label theLabel5 = new Label();
    //        theLabel5.ID = "lblTBDrgDuration";
    //        theLabel5.Text = "Duration";
    //        theLabel5.Width = 100;
    //        theheaderPnl.Controls.Add(theLabel5);

    //        Label theLabel6 = new Label();
    //        theLabel6.ID = "lblTBDrgPrescribed";
    //        theLabel6.Text = "Qty. Prescribed";
    //        theLabel6.Width = 100;
    //        theheaderPnl.Controls.Add(theLabel6);

    //        Label theLabel7 = new Label();
    //        theLabel7.ID = "lblTBDrgDispensed";
    //        theLabel7.Text = "Qty. Dispensed";
    //        theLabel7.Width = 110;
    //        theheaderPnl.Controls.Add(theLabel7);


    //        Label theLabel9 = new Label();
    //        theLabel9.ID = "lblTBTreatmentPhase";
    //        theLabel9.Text = "Treatment Phase";
    //        theLabel9.Width = 120;
    //        theheaderPnl.Controls.Add(theLabel9);

    //        Label theLabel10 = new Label();
    //        theLabel10.ID = "lblTBTreatmentMonth";
    //        theLabel10.Text = "Month";
    //        theLabel10.Width = 70;
    //        theheaderPnl.Controls.Add(theLabel10);


    //        MstPanel.Controls.Add(theheaderPnl);
    //        #endregion
    //    }

    //    Panel thePnl = new Panel();
    //    thePnl.ID = "pnl" + DrugId + "^" + Generic;
    //    thePnl.Height = 20;
    //    thePnl.Width = 860;
    //    thePnl.Controls.Clear();

    //    Label lblStSp = new Label();
    //    lblStSp.Width = 5;
    //    lblStSp.ID = "stSpace" + DrugId + "^" + Generic;
    //    lblStSp.Text = "";
    //    thePnl.Controls.Add(lblStSp);

    //    DataView theDV;
    //    DataSet theDS = (DataSet)ViewState["MasterData"];
    //    if (Generic == 0)
    //    {

    //        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
    //            theDV = new DataView(theDS.Tables[0]);
    //        else
    //            theDV = new DataView(theDS.Tables[12]);

    //        theDV.RowFilter = "Drug_Pk = " + DrugId;
    //    }
    //    else
    //    {
    //        theDV = new DataView(theDS.Tables[4]);
    //        theDV.RowFilter = "GenericId = " + DrugId;


    //    }

    //    Label theDrugNm = new Label();
    //    theDrugNm.ID = "drgNm" + DrugId + "^" + Generic;
    //    theDrugNm.Text = theDV[0][1].ToString();
    //    theDrugNm.Width = 220;
    //    thePnl.Controls.Add(theDrugNm);

    //    ///////// Space//////
    //    //Label theSpace = new Label();
    //    //theSpace.ID = "theSpace" + DrugId + "^" + Generic;
    //    //theSpace.Width = 20;
    //    //theSpace.Text = "";
    //    //thePnl.Controls.Add(theSpace);
    //    //////////////////////

    //    //TextBox theDose = new TextBox();
    //    //theDose.ID = "theDose" + DrugId + "^" + Generic;
    //    //theDose.Text = "";
    //    //theDose.Width = 80;
    //    //theDose.Load += new EventHandler(Control_Load);//Rupes 16Jan08 

    //    #endregion

    //    //thePnl.Controls.Add(theDose);



    //    BindFunctions theBindMgr = new BindFunctions();

    //    /////// Space//////
    //    Label theSpace2 = new Label();
    //    theSpace2.ID = "theSpace2*" + DrugId + "^" + Generic;
    //    theSpace2.Width = 15;
    //    theSpace2.Text = "";
    //    thePnl.Controls.Add(theSpace2);
    //    ////////////////////

    //    DropDownList theFrequency = new DropDownList();
    //    theFrequency.ID = "drgFrequency" + DrugId + "^" + Generic;
    //    theFrequency.Width = 80;
    //    DataTable DTFreq = new DataTable();
    //    DTFreq = theDS.Tables[13];
    //    theBindMgr.BindCombo(theFrequency, DTFreq, "FrequencyName", "FrequencyId");
    //    thePnl.Controls.Add(theFrequency);

    //    /////// Space//////
    //    Label theSpace3 = new Label();
    //    theSpace3.ID = "theSpace3*" + DrugId + "^" + Generic;
    //    theSpace3.Width = 15;
    //    theSpace3.Text = "";
    //    thePnl.Controls.Add(theSpace3);
    //    ////////////////////

    //    TextBox theDuration = new TextBox();
    //    theDuration.ID = "drgDuration" + DrugId + "^" + Generic;
    //    theDuration.Width = 100;
    //    theDuration.Text = "";
    //    theDuration.Load += new EventHandler(Control_Load);
    //    thePnl.Controls.Add(theDuration);

    //    ////////////Space////////////////////////
    //    Label theSpace4 = new Label();
    //    theSpace4.ID = "theSpace4*" + DrugId + "^" + Generic;
    //    theSpace4.Width = 15;
    //    theSpace4.Text = "";
    //    thePnl.Controls.Add(theSpace4);
    //    ////////////////////////////////////////

    //    TextBox theQtyPrescribed = new TextBox();
    //    theQtyPrescribed.ID = "drgQtyPrescribed" + DrugId + "^" + Generic;
    //    theQtyPrescribed.Width = 100;
    //    theQtyPrescribed.Text = "";
    //    //theQtyPrescribed.Load += new EventHandler(DecimalText_Load);
    //    theQtyPrescribed.Load += new EventHandler(Control_Load);
    //    thePnl.Controls.Add(theQtyPrescribed);

    //    ////////////Space////////////////////////
    //    Label theSpace5 = new Label();
    //    theSpace5.ID = "theSpace5*" + DrugId + "^" + Generic;
    //    theSpace5.Width = 15;
    //    theSpace5.Text = "";
    //    thePnl.Controls.Add(theSpace5);
    //    ////////////////////////////////////////

    //    TextBox theQtyDispensed = new TextBox();
    //    theQtyDispensed.ID = "drgQtyDispensed" + DrugId + "^" + Generic;
    //    theQtyDispensed.Width = 100;
    //    theQtyDispensed.Text = "";
    //    #region "13-Jun-07 -3"

    //    theQtyDispensed.Load += new EventHandler(Control_Load); // rupesh
    //    #endregion
    //    thePnl.Controls.Add(theQtyDispensed);

    //    ////////////Space////////////////////////
    //    Label theSpace6 = new Label();
    //    theSpace6.ID = "theSpace6*" + DrugId + "^" + Generic;
    //    theSpace6.Width = 15;
    //    theSpace6.Text = "";
    //    thePnl.Controls.Add(theSpace6);
    //    ////////////////////////////////////////



    //    //Treatment Phase
    //    DropDownList theTreatmenPhase = new DropDownList();
    //    theTreatmenPhase.ID = "drgTreatmenPhase" + DrugId + "^" + Generic;
    //    theTreatmenPhase.Width = 80;
    //    DataTable DTTrPhase = new DataTable();
    //    DTTrPhase = MakeTreatmentPhase();
    //    theBindMgr.BindCombo(theTreatmenPhase, DTTrPhase, "Name", "Id");
    //    thePnl.Controls.Add(theTreatmenPhase);

    //    ////////////Space////////////////////////
    //    Label theSpace8 = new Label();
    //    theSpace8.ID = "theSpace8*" + DrugId + "^" + Generic;
    //    theSpace8.Width = 15;
    //    theSpace8.Text = "";
    //    thePnl.Controls.Add(theSpace8);
    //    //////////////////////////////////////

    //    //Treatment Months

    //    DropDownList theTreatmenMonth = new DropDownList();
    //    theTreatmenMonth.ID = "drgTreatmenMonth" + DrugId + "^" + Generic;
    //    theTreatmenMonth.Width = 80;
    //    DataTable DTTrMonth = new DataTable();
    //    DTTrMonth = MakeTreatmentMonth();
    //    theBindMgr.BindCombo(theTreatmenMonth, DTTrMonth, "Name", "Id");
    //    thePnl.Controls.Add(theTreatmenMonth);

    //    LinkButton thelnkRemove = new LinkButton();
    //    thelnkRemove.ID = "lnkrmv%" + MstPanel.ID + "^" + DrugId;
    //    thelnkRemove.Width = 10;
    //    thelnkRemove.Text = "Remove";
    //    thelnkRemove.Click += new EventHandler(Remove_panel);

    //    thelnkRemove.OnClientClick = "return confirm('Are you sure you want to Remove this Drug?')";
    //    thePnl.Controls.Add(thelnkRemove);


    //    /////////Space panel/////////////////////////
    //    Panel thePnlspace = new Panel();
    //    thePnlspace.ID = "pnlspace_" + DrugId;
    //    thePnlspace.Height = 3;
    //    thePnlspace.Width = 900;
    //    thePnlspace.Controls.Clear();
    //    thePnl.Controls.Add(thePnlspace);
    //    MstPanel.Controls.Add(thePnl);
    //}
    private void BindTBDrugControls(int DrugId, int Generic, Panel MstPanel)
    {
        //oi and other medications
        //if (MstPanel.Controls.Count < 1)
        //{
        Control thehdrCntrl = FindControlRecursive(MstPanel, "pnlTBDrug" + MstPanel.ID);
        if (thehdrCntrl == null)
        {
            #region "TB Medication"
            Panel thelblPnl = new Panel();
            thelblPnl.ID = "pnlTBDrug" + MstPanel.ID;
            thelblPnl.Height = 20;
            thelblPnl.Width = 900;
            thelblPnl.Controls.Clear();

            Label theLabel = new Label();
            theLabel.ID = "lblTBDrug";
            theLabel.Text = "TB Medications";//earlier it was "OI Treatment and Other Medications";
            theLabel.Font.Bold = true;
            thelblPnl.Controls.Add(theLabel);
            MstPanel.Controls.Add(thelblPnl);

            /////////////////////////////////////////////////
            Panel theheaderPnl = new Panel();
            theheaderPnl.ID = "pnlHeaderTBDrug";
            theheaderPnl.Height = 20;
            theheaderPnl.Width = 900;
            theheaderPnl.Font.Bold = true;
            theheaderPnl.Controls.Clear();

            Label theSP = new Label();
            theSP.ID = "lblTBDrgSp";
            theSP.Width = 5;
            theSP.Text = "";
            theheaderPnl.Controls.Add(theSP);

            Label theLabel1 = new Label();
            theLabel1.ID = "lblTBDrgNm";
            theLabel1.Text = "Drug Name";
            theLabel1.Width = 220;
            theheaderPnl.Controls.Add(theLabel1);

            //Label theLabel2 = new Label();
            //theLabel2.ID = "lblTBDrgDose";
            //theLabel2.Text = "Dose";
            //theLabel2.Width = 50;
            //theheaderPnl.Controls.Add(theLabel2);

            //Label theLabel3 = new Label();
            //theLabel3.ID = "lblTBDrgUnits";
            //theLabel3.Text = "Unit";
            //theLabel3.Width = 90;
            //theheaderPnl.Controls.Add(theLabel3);

            Label theLabel4 = new Label();
            theLabel4.ID = "lblTBDrgFrequency";
            theLabel4.Text = "Frequency";
            theLabel4.Width = 95;
            theheaderPnl.Controls.Add(theLabel4);

            Label theLabel5 = new Label();
            theLabel5.ID = "lblTBDrgDuration";
            theLabel5.Text = "Duration";
            theLabel5.Width = 100;
            theheaderPnl.Controls.Add(theLabel5);

            Label theLabel6 = new Label();
            theLabel6.ID = "lblTBDrgPrescribed";
            theLabel6.Text = "Qty. Prescribed";
            theLabel6.Width = 100;
            theheaderPnl.Controls.Add(theLabel6);

            Label theLabel7 = new Label();
            theLabel7.ID = "lblTBDrgDispensed";
            theLabel7.Text = "Qty. Dispensed";
            theLabel7.Width = 90;
            theheaderPnl.Controls.Add(theLabel7);

            //Label theLabel8 = new Label();
            //theLabel8.ID = "lblTBDrgFinanced";
            //theLabel8.Text = "AR";
            //theLabel8.Width = 20;
            //theheaderPnl.Controls.Add(theLabel8);

            Label theLabel9 = new Label();
            theLabel9.ID = "lblTBTreatmentPhase";
            theLabel9.Text = "Treatment Phase";
            theLabel9.Width = 100;
            theheaderPnl.Controls.Add(theLabel9);

            Label lblStSp2 = new Label();
            lblStSp2.Width = 10;
            lblStSp2.ID = "stSpace2" + DrugId + "^" + Generic;
            lblStSp2.Text = "";
            theheaderPnl.Controls.Add(lblStSp2);

            Label theLabel10 = new Label();
            theLabel10.ID = "lblTBTreatmentMonth";
            theLabel10.Text = "Month";
            theLabel10.Width = 50;
            theheaderPnl.Controls.Add(theLabel10);

            Label lblProphylaxis = new Label();
            lblProphylaxis.Text = "Prophylaxis";
            lblProphylaxis.ID = "lblProphylaxis" + MstPanel.ID;
            lblProphylaxis.Font.Bold = true;
            lblProphylaxis.Visible = true;
            theheaderPnl.Controls.Add(lblProphylaxis);

            MstPanel.Controls.Add(theheaderPnl);
            #endregion
        }
        //}
        Control thedrgCntrl = FindControlRecursive(MstPanel, "pnl" + DrugId + "^" + Generic);
        if (thedrgCntrl == null)
        {
            Panel thePnl = new Panel();
            thePnl.ID = "pnl" + DrugId + "^" + Generic;
            thePnl.Height = 20;
            thePnl.Width = 900;
            thePnl.Controls.Clear();

            Label lblStSp = new Label();
            lblStSp.Width = 5;
            lblStSp.ID = "stSpace" + DrugId + "^" + Generic;
            lblStSp.Text = "";
            thePnl.Controls.Add(lblStSp);

            DataView theDV;
            DataSet theDS = (DataSet)ViewState["MasterData"];
            if (Generic == 0)
            {

                //theDV = new DataView(theDS.Tables[0]);
                theDV = new DataView(theDS.Tables[12]); // rupesh for both active & inactive drug 31/07/07
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
                //theDV = new DataView(theDS.Tables[4]);
                //theDV.RowFilter = "GenericId = " + DrugId;
            }

            Label theDrugNm = new Label();
            theDrugNm.ID = "drgNm" + DrugId + "^" + Generic;
            theDrugNm.Text = theDV[0][1].ToString();
            theDrugNm.Width = 200;
            thePnl.Controls.Add(theDrugNm);

            /////// Space//////
            Label theSpace = new Label();
            theSpace.ID = "theSpace" + DrugId + "^" + Generic;
            theSpace.Width = 10;
            theSpace.Text = "";
            thePnl.Controls.Add(theSpace);
            ////////////////////



            BindFunctions theBindMgr = new BindFunctions();

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
            DTFreq = theDS.Tables[8];
            theBindMgr.BindCombo(theFrequency, DTFreq, "FrequencyName", "FrequencyId");
            thePnl.Controls.Add(theFrequency);

            /////// Space//////
            Label theSpace3 = new Label();
            theSpace3.ID = "theSpace3*" + DrugId + "^" + Generic;
            theSpace3.Width = 15;
            theSpace3.Text = "";
            thePnl.Controls.Add(theSpace3);
            ////////////////////

            TextBox theDuration = new TextBox();
            theDuration.ID = "drgDuration" + DrugId + "^" + Generic;
            theDuration.Width = 85;
            theDuration.Text = "";
            theDuration.Load += new EventHandler(Control_Load);
            thePnl.Controls.Add(theDuration);

            ////////////Space////////////////////////
            Label theSpace4 = new Label();
            theSpace4.ID = "theSpace4*" + DrugId + "^" + Generic;
            theSpace4.Width = 15;
            theSpace4.Text = "";
            thePnl.Controls.Add(theSpace4);
            ////////////////////////////////////////

            TextBox theQtyPrescribed = new TextBox();
            theQtyPrescribed.ID = "drgQtyPrescribed" + DrugId + "^" + Generic;
            theQtyPrescribed.Width = 85;
            theQtyPrescribed.Text = "";
            //theQtyPrescribed.Load += new EventHandler(DecimalText_Load);
            theQtyPrescribed.Load += new EventHandler(Control_Load);
            thePnl.Controls.Add(theQtyPrescribed);

            ////////////Space////////////////////////
            Label theSpace5 = new Label();
            theSpace5.ID = "theSpace5*" + DrugId + "^" + Generic;
            theSpace5.Width = 15;
            theSpace5.Text = "";
            thePnl.Controls.Add(theSpace5);
            ////////////////////////////////////////

            TextBox theQtyDispensed = new TextBox();
            theQtyDispensed.ID = "drgQtyDispensed" + DrugId + "^" + Generic;
            theQtyDispensed.Width = 85;
            theQtyDispensed.Text = "";
            #region "13-Jun-07 -3"
            //theQtyDispensed.Load += new EventHandler(DecimalText_Load); 
            theQtyDispensed.Load += new EventHandler(Control_Load); // rupesh
            #endregion
            //if (Session["SCMModule"] != null)
            //    theQtyDispensed.Attributes.Add("onkeyup", "chknotwrite('" + theQtyDispensed.ClientID + "')");
            //    //theQtyDispensed.Enabled = false;
            thePnl.Controls.Add(theQtyDispensed);

            ////////////Space////////////////////////
            Label theSpace6 = new Label();
            theSpace6.ID = "theSpace6*" + DrugId + "^" + Generic;
            theSpace6.Width = 15;
            theSpace6.Text = "";
            thePnl.Controls.Add(theSpace6);
            ////////////////////////////////////////


            //Treatment Phase
            DropDownList theTreatmenPhase = new DropDownList();
            theTreatmenPhase.ID = "drgTreatmenPhase" + DrugId + "^" + Generic;
            theTreatmenPhase.Width = 70;
            DataTable DTTrPhase = new DataTable();
            DTTrPhase = MakeTreatmentPhase();
            theBindMgr.BindCombo(theTreatmenPhase, DTTrPhase, "Name", "Id");
            thePnl.Controls.Add(theTreatmenPhase);

            ////////////Space////////////////////////
            Label theSpace8 = new Label();
            theSpace8.ID = "theSpace8*" + DrugId + "^" + Generic;
            theSpace8.Width = 15;
            theSpace8.Text = "";
            thePnl.Controls.Add(theSpace8);
            //////////////////////////////////////

            //Treatment Months

            DropDownList theTreatmenMonth = new DropDownList();
            theTreatmenMonth.ID = "drgTreatmenMonth" + DrugId + "^" + Generic;
            theTreatmenMonth.Width = 70;
            DataTable DTTrMonth = new DataTable();
            DTTrMonth = MakeTreatmentMonth();
            theBindMgr.BindCombo(theTreatmenMonth, DTTrMonth, "Name", "Id");
            thePnl.Controls.Add(theTreatmenMonth);

            ////////////Space///////////////////////
            Label theSpace7 = new Label();
            theSpace7.ID = "theSpace7" + DrugId;
            theSpace7.Width = 10;
            theSpace7.Text = "";
            thePnl.Controls.Add(theSpace7);

            CheckBox theOtherARTProPhChk = new CheckBox();
            theOtherARTProPhChk.ID = "chkProphylaxis" + DrugId;
            theOtherARTProPhChk.Width = 10;
            theOtherARTProPhChk.Text = "";
            theOtherARTProPhChk.Enabled = true;
            //if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
            //{
            //    theOtherARTProPhChk.Enabled = true;
            //}
            //else
            //    theOtherARTProPhChk.Enabled = false;
            thePnl.Controls.Add(theOtherARTProPhChk);
            //}
            ////////////Space///////////////////////
            Label theSpace9 = new Label();
            theSpace9.ID = "theSpace9" + DrugId;
            theSpace9.Width = 30;
            theSpace9.Text = "";
            thePnl.Controls.Add(theSpace9);

            LinkButton thelnkRemove = new LinkButton();
            thelnkRemove.ID = "lnkrmv%" + MstPanel.ID + "^" + DrugId;
            thelnkRemove.Width = 10;
            thelnkRemove.Text = "Remove";
            thelnkRemove.Click += new EventHandler(Remove_panel);
            if (Session["ExistPharmacyData"] != null)
            {
                if (((DataTable)Session["ExistPharmacyData"]).Rows.Count > 0)
                {
                    IQCareUtils theUtilsCF = new IQCareUtils();
                    DataView theExistDrgDV = new DataView((DataTable)Session["ExistPharmacyData"]);
                    theExistDrgDV.RowFilter = "Drug_Pk=" + DrugId;
                    DataTable theDTfiltdrg = (DataTable)theUtilsCF.CreateTableFromDataView(theExistDrgDV);
                    if (Convert.ToInt32(Session["PatientVisitId"]) > 0 && theDTfiltdrg.Rows.Count > 0)
                    {
                        thelnkRemove.Visible = false;
                    }
                }
            }
            thelnkRemove.OnClientClick = "return confirm('Are you sure you want to Remove this Drug?')";
            thePnl.Controls.Add(thelnkRemove);

            MstPanel.Controls.Add(thePnl);
            /////////Space panel/////////////////////////
            Panel thePnlspace = new Panel();
            thePnlspace.ID = "pnlspace_" + DrugId;
            thePnlspace.Height = 3;
            thePnlspace.Width = 900;
            thePnlspace.Controls.Clear();
            MstPanel.Controls.Add(thePnlspace);
        }

    }
    //private void BindCustomControls(int DrugId ,int Generic, Panel MstPanel)
    //{
    //    if (MstPanel.Controls.Count <1)
    //    {
    //        //if (MstPanel.ID == "PnlAdARV")
    //        if (MstPanel.ID == "pnlPedia")
    //        {
    //            #region "Additional ARV"

    //            Panel thelblPnl = new Panel();
    //            thelblPnl.ID = "pnlAdditionalARV";
    //            thelblPnl.Height = 20;

    //            thelblPnl.Width = 860;
    //            thelblPnl.Controls.Clear();

    //            Label theLabel = new Label();
    //            theLabel.ID = "lblADDARV";
    //            theLabel.Text = "Additinal ARV";
    //            theLabel.Font.Bold = true;
    //            thelblPnl.Controls.Add(theLabel);

    //            MstPanel.Controls.Add(thelblPnl);
    //            #endregion

    //        }
    //        else
    //        {
    //            #region "OI & Other Medication"
    //            Panel thelblPnl = new Panel();
    //            thelblPnl.ID = "pnlOtherDrug";
    //            thelblPnl.Height = 20;
    //            thelblPnl.Width = 860;
    //            thelblPnl.Controls.Clear();

    //            Label theLabel = new Label();
    //            theLabel.ID = "lblOtherDrug";
    //            theLabel.Text = "OI Treatment and Other Medications";
    //            theLabel.Font.Bold = true;
    //            thelblPnl.Controls.Add(theLabel);
    //            MstPanel.Controls.Add(thelblPnl);
    //            #endregion
    //        }
    //    }

    //    Panel thePnl = new Panel();
    //    thePnl.ID =  "pnl" + DrugId;
    //    thePnl.Height = 20;
    //    thePnl.Width = 860;
    //    thePnl.Controls.Clear();

    //    /////// Space//////
    //    Label theSpace = new Label();
    //    theSpace.ID =  "theSpace" + DrugId;
    //    theSpace.Width = 25;
    //    theSpace.Text = "";
    //    ////////////////////

    //    DataSet theDS = (DataSet)ViewState["MasterData"];
    //    if (Generic == 0)
    //    {
    //        //-----------rupesh for showing inactive drugs -------------- 
    //        //theDV = new DataView(theDS.Tables[0]); 
    //        if(Convert.ToInt32(Session["PatientVisitId"]) == 0)
    //            theDV = new DataView(theDS.Tables[0]);
    //        else
    //            theDV = new DataView(theDS.Tables[12]);

    //        theDV.RowFilter = "Drug_Pk = " + DrugId;
    //    }
    //    else
    //    {
    //        //--------- rupesh 22-Nov-07 for Inactive Generics -- change 27Dec07----------
    //        theDV = new DataView(theDS.Tables[4]);
    //        theDV.RowFilter = "GenericId = " + DrugId;

    //        ////////if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
    //        ////////    theDV = new DataView(theDS.Tables[4]);
    //        ////////else
    //        ////////    theDV = new DataView(theDS.Tables[19]);
    //        ////////theDV.RowFilter = "GenericId = " + DrugId;
    //        //--------- rupesh 22-Nov-07 for Inactive Generics----------
    //    }

    //    Label theDrugNm = new Label();
    //    theDrugNm.ID =  "drgNm" + DrugId;
    //    theDrugNm.Text = theDV[0][1].ToString();
    //    theDrugNm.Width = 236;
    //    thePnl.Controls.Add(theDrugNm);
    //    thePnl.Controls.Add(theSpace);

    //    //DataTable theDTS = new DataTable();
    //    //DataView theDVFormulation = new DataView(theDS.Tables[1]);
    //    //if (Generic == 0)
    //    //{
    //    //    theDTS = (DataTable)Session["FixDrugStrength"];
    //    //    theDVFormulation = new DataView(theDTS);
    //    //    theDVFormulation.RowFilter = "Drug_pk = " + DrugId + " and StrengthId>0";
    //    //}
    //    //else
    //    //{
    //    //    theDVFormulation.RowFilter = "GenericId = " + DrugId;
    //    //}

    //    //DataTable theDTFormulation = new DataTable();
    //    //if (theDVFormulation.Count > 0)
    //    //{
    //    //    IQCareUtils theUtils = new IQCareUtils();
    //    //    theDTFormulation = theUtils.CreateTableFromDataView(theDVFormulation);
    //    //}

    //    BindFunctions theBindMgr = new BindFunctions();
    //    //DropDownList ddlFormulation = new DropDownList();
    //    //ddlFormulation.ID = "drgStrength" + DrugId;
    //    //ddlFormulation.Width = 86;
    //    //theBindMgr.BindCombo(ddlFormulation, theDTFormulation, "StrengthName", "StrengthId");
    //    //thePnl.Controls.Add(ddlFormulation);

    //    //////////////Space////////////////////////
    //    //Label theSpace1 = new Label();
    //    //theSpace1.ID = "lblSp2" + DrugId;
    //    //theSpace1.Width = 10;
    //    //theSpace1.Text = "";
    //    //thePnl.Controls.Add(theSpace1);
    //    ////////////////////////////////////////////

    //    //TextBox txtDose = new TextBox();
    //    //txtDose.ID = "drgDose" + DrugId;
    //    //txtDose.Width = 80;
    //    ////txtDose.AutoPostBack = true; // 29Jan08
    //    ////txtDose.TextChanged += new EventHandler(txtDose_TextChanged); // 29Jan08
    //    //txtDose.Load += new EventHandler(Control_Load);

    //    //thePnl.Controls.Add(txtDose);

    //    //////////////Space////////////////////////
    //    Label theSpace2 = new Label();
    //    theSpace2.ID =  "lblSp3" + DrugId;
    //    theSpace2.Width = 10;
    //    theSpace2.Text = "";
    //    thePnl.Controls.Add(theSpace2);
    //    //////////////////////////////////////////

    //    DataTable theDTF = new DataTable();

    //    DataView theDVFrequency = new DataView(theDS.Tables[2]);
    //    if (Generic == 0)
    //    {

    //        theDTF = (DataTable)Session["FixDrugFreq"];
    //        theDVFrequency = new DataView(theDTF);
    //        theDVFrequency.RowFilter = "Drug_pk = " + DrugId + " and FrequencyId>0";

    //    }
    //    else
    //    {
    //        theDVFrequency.RowFilter = "GenericId = " + DrugId;
    //    }
    //    //theDVFrequency.RowFilter = "DrugId=" + DrugId;
    //    DataTable theDTFrequency = new DataTable();
    //    if (theDVFrequency.Count > 0)
    //    {
    //        IQCareUtils theUtils = new IQCareUtils();
    //        theDTFrequency = theUtils.CreateTableFromDataView(theDVFrequency);
    //    }

    //    DropDownList ddlFrequency = new DropDownList();
    //    ddlFrequency.ID = "drgFrequency" + DrugId;
    //    ddlFrequency.Width = 86;

    //    ddlFrequency.SelectedIndex = 0;
    //    theBindMgr.BindCombo(ddlFrequency, theDTFrequency, "FrequencyName", "FrequencyId");


    //    thePnl.Controls.Add(ddlFrequency);

    //    //Label theSpace3 = new Label();
    //    //theSpace3.ID =  "lblSp4" + DrugId;
    //    //theSpace3.Width = 10;
    //    //theSpace3.Text = "";
    //    //thePnl.Controls.Add(theSpace3);

    //    //TextBox txtTotalDose = new TextBox();
    //    //txtTotalDose.ID =  "drgTotalDose" + DrugId;
    //    //txtTotalDose.Width = 86;
    //    //txtTotalDose.Load += new EventHandler(Control_Load);
    //    ////------29Jan08--------
    //    ////ddlFrequency.Attributes.Add("onClick", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgDose" + DrugId + "','ctl00_clinicalheaderfooter_drgFrequency" + DrugId + "','ctl00_clinicalheaderfooter_drgTotalDose" + DrugId + "');");
    //    ////txtDose.Attributes.Add("onBlur", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgDose" + DrugId + "','ctl00_clinicalheaderfooter_drgFrequency" + DrugId + "','ctl00_clinicalheaderfooter_drgTotalDose" + DrugId + "');");
    //    ////--------------------
    //    //thePnl.Controls.Add(txtTotalDose);

    //    //////////////Space////////////////////////
    //    Label theSpace4 = new Label();
    //    theSpace4.ID =  "lblSp5" + DrugId;
    //    theSpace4.Width = 10;
    //    theSpace4.Text = "";
    //    thePnl.Controls.Add(theSpace4);
    //    //////////////////////////////////////////

    //    TextBox txtDuration = new TextBox();
    //    txtDuration.ID =  "drgDuration" + DrugId;
    //    txtDuration.Width = 86;
    //    txtDuration.Load += new EventHandler(DecimalText_Load); 
    //    thePnl.Controls.Add(txtDuration);

    //    //////////////Space////////////////////////
    //    Label theSpace5 = new Label();
    //    theSpace5.ID =  "lblSp6" + DrugId;
    //    theSpace5.Width = 10;
    //    theSpace5.Text = "";
    //    thePnl.Controls.Add(theSpace5);
    //    //////////////////////////////////////////

    //    TextBox theQtyPrescribed = new TextBox();
    //    theQtyPrescribed.ID =  "drgQtyPrescribed" + DrugId;
    //    theQtyPrescribed.Width = 86;
    //    theQtyPrescribed.Load += new EventHandler(DecimalText_Load);
    //    thePnl.Controls.Add(theQtyPrescribed);

    //    //////////////Space////////////////////////
    //    Label theSpace6 = new Label();
    //    theSpace6.ID =  "lblSp7" + DrugId;
    //    theSpace6.Width = 10;
    //    theSpace6.Text = "";
    //    thePnl.Controls.Add(theSpace6);
    //    //////////////////////////////////////////

    //    TextBox theQtyDispensed = new TextBox();
    //    theQtyDispensed.ID =  "drgQtyDispensed" + DrugId;
    //    theQtyDispensed.Width = 86;
    //    theQtyDispensed.Load += new EventHandler(DecimalText_Load); 
    //    thePnl.Controls.Add(theQtyDispensed);

    //    //////////////Space////////////////////////
    //    Label theSpace7 = new Label();
    //    theSpace7.ID =  "lblSp8" + DrugId;
    //    theSpace7.Width = 40;
    //    theSpace7.Text = "";
    //    thePnl.Controls.Add(theSpace7);
    //    //////////////////////////////////////////
    //    if (ddlTreatment.SelectedItem.Value.ToString() == "223")
    //    {
    //        CheckBox theOtherARTProPhChk = new CheckBox();
    //        theOtherARTProPhChk.ID = "chkProphylaxis" + DrugId;
    //        theOtherARTProPhChk.Width = 10;
    //        theOtherARTProPhChk.Text = "";
    //        thePnl.Controls.Add(theOtherARTProPhChk);
    //    }
    //    LinkButton thelnkRemove = new LinkButton();
    //    thelnkRemove.ID = "lnkrmv%" + MstPanel.ID + "^" + DrugId;
    //    thelnkRemove.Width = 20;
    //    thelnkRemove.Text = "Remove";
    //    thelnkRemove.Click += new EventHandler(Remove_panel);

    //    thelnkRemove.OnClientClick = "return confirm('Are you sure you want to Remove this Drug?')";
    //    thePnl.Controls.Add(thelnkRemove);

    //    //MstPanel.Controls.Add(thePnl);
    //    Panel thePnlspace = new Panel();
    //    thePnlspace.ID = "pnlspace_" + DrugId;
    //    thePnlspace.Height = 3;
    //    thePnlspace.Width = 900;
    //    thePnlspace.Controls.Clear();
    //    thePnl.Controls.Add(thePnlspace);

    //    MstPanel.Controls.Add(thePnl);
    //    AddControlsAttributes(MstPanel);

    //}
    private void BindCustomControls(int DrugId, int Generic, Panel MstPanel)
    {
        Control thehdrCntrl = FindControlRecursive(MstPanel, "pnlARVDrug" + MstPanel.ID);
        if (thehdrCntrl == null)
        {
            #region "ARV Medication"
            Panel thelblPnl = new Panel();
            thelblPnl.ID = "pnlARVDrug" + MstPanel.ID;
            thelblPnl.Height = 20;
            thelblPnl.Width = 900;
            thelblPnl.Controls.Clear();

            Label theLabel = new Label();
            theLabel.ID = "lblDrug" + MstPanel.ID;
            theLabel.Text = "ARV Medications";//earlier it was "OI Treatment and Other Medications";
            theLabel.Font.Bold = true;
            thelblPnl.Controls.Add(theLabel);
            MstPanel.Controls.Add(thelblPnl);

            /////////////////////////////////////////////////
            Panel theheaderPnl = new Panel();
            theheaderPnl.ID = "pnlARVhdrDrug" + MstPanel.ID;
            theheaderPnl.Height = 20;
            theheaderPnl.Width = 900;
            theheaderPnl.Font.Bold = true;
            theheaderPnl.Controls.Clear();

            Label theSP = new Label();
            theSP.ID = "lblDrgSp" + MstPanel.ID;
            theSP.Width = 5;
            theSP.Text = "";
            theheaderPnl.Controls.Add(theSP);

            Label theLabel1 = new Label();
            theLabel1.ID = "lblDrgNm" + MstPanel.ID;
            theLabel1.Text = "Drug Name";
            theLabel1.Width = 200;
            theheaderPnl.Controls.Add(theLabel1);


            Label lblSpace = new Label();
            lblSpace.Width = 20;
            lblSpace.ID = "lblSpaceAdd_1" + MstPanel.ID;
            lblSpace.Text = "";
            theheaderPnl.Controls.Add(lblSpace);

            Label theLabel4 = new Label();
            theLabel4.ID = "lblDrgFrequency" + MstPanel.ID;
            theLabel4.Text = "Frequency";
            //theLabel4.Width = 80;
            theheaderPnl.Controls.Add(theLabel4);

            Label lblSpace2 = new Label();
            lblSpace2.Width = 40;
            lblSpace2.ID = "lblSpaceAdd_2" + MstPanel.ID;
            lblSpace2.Text = "";
            theheaderPnl.Controls.Add(lblSpace2);

            Label theLabel5 = new Label();
            theLabel5.ID = "lblDrgDuration" + MstPanel.ID;
            theLabel5.Text = "Duration";
            //theLabel5.Width = 110;
            theheaderPnl.Controls.Add(theLabel5);

            Label lblSpace3 = new Label();
            lblSpace3.Width = 48;
            lblSpace3.ID = "lblSpaceAdd_4" + MstPanel.ID;
            lblSpace3.Text = "";
            theheaderPnl.Controls.Add(lblSpace3);

            Label theLabel6 = new Label();
            theLabel6.ID = "lblDrgPrescribed" + MstPanel.ID;
            theLabel6.Text = "Qty. Prescribed";
            //theLabel6.Width = 110;
            theheaderPnl.Controls.Add(theLabel6);

            Label lblSpace4 = new Label();
            lblSpace4.Width = 15;
            lblSpace4.ID = "lblSpaceAdd_5" + MstPanel.ID;
            lblSpace4.Text = "";
            theheaderPnl.Controls.Add(lblSpace4);

            Label theLabel7 = new Label();
            theLabel7.ID = "lblDrgDispensed" + MstPanel.ID;
            theLabel7.Text = "Qty. Dispensed";
            //theLabel7.Width = 110;
            theheaderPnl.Controls.Add(theLabel7);

            Label lblSpace5 = new Label();
            lblSpace5.Width = 20;
            lblSpace5.ID = "lblpropSpace" + MstPanel.ID;
            lblSpace5.Text = "";
            theheaderPnl.Controls.Add(lblSpace5);

            Label lblProphylaxis = new Label();
            lblProphylaxis.Text = "Prophylaxis";
            lblProphylaxis.ID = "lblProphylaxis" + MstPanel.ID;
            lblProphylaxis.Font.Bold = true;
            lblProphylaxis.Visible = true;
            theheaderPnl.Controls.Add(lblProphylaxis);



            MstPanel.Controls.Add(theheaderPnl);
            #endregion
        }
        Control thedrgCntrl = FindControlRecursive(MstPanel, "pnl_" + DrugId);
        if (thedrgCntrl == null)
        {
            Panel thePnl = new Panel();
            thePnl.ID = "pnl_" + DrugId;
            thePnl.Height = 20;
            thePnl.Width = 900;
            thePnl.Controls.Clear();


            Label lblStSp = new Label();
            lblStSp.Width = 5;
            lblStSp.ID = "stSpace" + DrugId;
            lblStSp.Text = "";
            thePnl.Controls.Add(lblStSp);

            DataView theDV;
            DataSet theDS_Custom = (DataSet)ViewState["MasterData"];
            theDV = new DataView(theDS_Custom.Tables[12]);
            if (DrugId.ToString().LastIndexOf("8888") > 0)
            {

                DrugId = Convert.ToInt32(DrugId.ToString().Substring(0, DrugId.ToString().Length - 4));
            }
            theDV.RowFilter = "Drug_Pk = " + DrugId;

            Label theDrugNm = new Label();
            theDrugNm.ID = "drgNm" + DrugId;
            theDrugNm.Text = theDV[0][1].ToString();
            theDrugNm.Width = 200;
            thePnl.Controls.Add(theDrugNm);

            /////// Space//////
            Label theSpace = new Label();
            theSpace.ID = "theSpace_" + DrugId;
            theSpace.Width = 10;
            theSpace.Text = "";
            ////////////////////

            thePnl.Controls.Add(theSpace);

            BindFunctions theBindMgr = new BindFunctions();

            ////////////Space////////////////////////
            Label theSpace1 = new Label();
            theSpace1.ID = "theSpace1" + DrugId;
            theSpace1.Width = 10;
            theSpace1.Text = "";
            thePnl.Controls.Add(theSpace1);
            ////////////////////////////////////////

            DropDownList theDrugFrequency = new DropDownList();
            theDrugFrequency.ID = "drgFrequency" + DrugId;
            theDrugFrequency.Width = 80;
            #region "BindCombo"
            DataTable theDTF = new DataTable();
            DataView theDVFrequency = new DataView(theDS_Custom.Tables[8]);

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
            theDuration.Width = 85;
            theDuration.Load += new EventHandler(Control_Load);
            thePnl.Controls.Add(theDuration);

            ////////////Space////////////////////////
            Label theSpace3 = new Label();
            theSpace3.ID = "theSpace3" + DrugId;
            theSpace3.Width = 15;
            theSpace3.Text = "";
            thePnl.Controls.Add(theSpace3);
            ////////////////////////////////////////

            TextBox theQtyPrescribed = new TextBox();
            theQtyPrescribed.ID = "drgQtyPrescribed" + DrugId;
            theQtyPrescribed.Width = 85;
            theQtyPrescribed.Load += new EventHandler(Control_Load);
            thePnl.Controls.Add(theQtyPrescribed);
            ////////////Space////////////////////////
            Label theSpace4 = new Label();
            theSpace4.ID = "theSpace4" + DrugId;
            theSpace4.Width = 15;
            theSpace4.Text = "";
            thePnl.Controls.Add(theSpace4);
            ////////////////////////////////////////

            TextBox theQtyDispensed = new TextBox();
            theQtyDispensed.ID = "drgQtyDispensed" + DrugId;
            theQtyDispensed.Width = 85;
            theQtyDispensed.Load += new EventHandler(Control_Load);
            //if (Session["SCMModule"] != null)
            //    theQtyDispensed.Attributes.Add("onkeyup", "chknotwrite('" + theQtyDispensed.ClientID + "')");
            //    //theQtyDispensed.Enabled = false;
            thePnl.Controls.Add(theQtyDispensed);
            ////////////Space////////////////////////
            Label theSpace5 = new Label();
            theSpace5.ID = "theSpace5" + DrugId;
            theSpace5.Width = 15;
            theSpace5.Text = "";
            thePnl.Controls.Add(theSpace5);

            ////////////Space///////////////////////
            Label theSpace6 = new Label();
            theSpace6.ID = "theSpace6" + DrugId;
            theSpace6.Width = 15;
            theSpace6.Text = "";
            thePnl.Controls.Add(theSpace6);

            //if (ddlTreatment.SelectedItem.Value.ToString() == "223")
            //{
            CheckBox theOtherARTProPhChk = new CheckBox();
            theOtherARTProPhChk.ID = "chkProphylaxis" + DrugId;
            theOtherARTProPhChk.Width = 10;
            theOtherARTProPhChk.Text = "";
            if (ddlTreatment.SelectedItem.Value.ToString() == "222")
            {
                theOtherARTProPhChk.Enabled = false;
            }
            else
                theOtherARTProPhChk.Enabled = true;
            thePnl.Controls.Add(theOtherARTProPhChk);
            //}
            ////////////Space///////////////////////
            Label theSpace7 = new Label();
            theSpace7.ID = "theSpace7" + DrugId;
            theSpace7.Width = 185;
            theSpace7.Text = "";
            thePnl.Controls.Add(theSpace7);

            LinkButton thelnkRemove = new LinkButton();
            thelnkRemove.ID = "lnkrmv%" + MstPanel.ID + "^" + DrugId;
            thelnkRemove.Width = 20;
            thelnkRemove.Text = "Remove";
            thelnkRemove.Click += new EventHandler(Remove_panel);
            if (Session["ExistPharmacyData"] != null)
            {
                if (((DataTable)Session["ExistPharmacyData"]).Rows.Count > 0)
                {
                    IQCareUtils theUtilsCF = new IQCareUtils();
                    DataView theExistDrgDV = new DataView((DataTable)Session["ExistPharmacyData"]);
                    theExistDrgDV.RowFilter = "Drug_Pk=" + DrugId;
                    DataTable theDTfiltdrg = (DataTable)theUtilsCF.CreateTableFromDataView(theExistDrgDV);
                    if (Convert.ToInt32(Session["PatientVisitId"]) > 0 && theDTfiltdrg.Rows.Count > 0)
                    {
                        thelnkRemove.Visible = false;
                    }
                }
            }
            thelnkRemove.OnClientClick = "return confirm('Are you sure you want to Remove this Drug?')";
            thePnl.Controls.Add(thelnkRemove);

            MstPanel.Controls.Add(thePnl);
            Panel thePnlspace = new Panel();
            thePnlspace.ID = "pnlspace_" + DrugId;
            thePnlspace.Height = 3;
            thePnlspace.Width = 900;
            thePnlspace.Controls.Clear();
            MstPanel.Controls.Add(thePnlspace);
            //hidchkbox1.Value = hidchkbox1.Value + "," + theOtherARTProPhChk.ID;
            //}
            //else
            //{

            //}
        }
    }
    //private void BindAdditionalDrugControls(int DrugId, int Generic, Panel MstPanel)
    //{
    //    //OI and other medications

    //    if (MstPanel.Controls.Count < 1)
    //    {
    //        #region "OI & Other Medication"
    //        Panel thelblPnl = new Panel();
    //        thelblPnl.ID = "pnlOtherDrug";
    //        thelblPnl.Height = 20;
    //        thelblPnl.Width = 840;
    //        thelblPnl.Controls.Clear();

    //        Label theLabel = new Label();
    //        theLabel.ID = "lblOtherDrug";
    //        theLabel.Text = "OI Treatment and Other Medications";
    //        theLabel.Font.Bold = true;
    //        thelblPnl.Controls.Add(theLabel);
    //        MstPanel.Controls.Add(thelblPnl);

    //        /////////////////////////////////////////////////
    //        Panel theheaderPnl = new Panel();
    //        theheaderPnl.ID = "pnlHeaderOtherDrug";
    //        theheaderPnl.Height = 20;
    //        theheaderPnl.Width = 840;
    //        theheaderPnl.Font.Bold = true;
    //        theheaderPnl.Controls.Clear();

    //        Label theSP = new Label();
    //        theSP.ID = "lblDrgSp";
    //        theSP.Width = 5;
    //        theSP.Text = "";
    //        theheaderPnl.Controls.Add(theSP);

    //        Label theLabel1 = new Label();
    //        theLabel1.ID = "lblDrgNm";
    //        theLabel1.Text = "Drug Name";
    //        theLabel1.Width = 240;
    //        theheaderPnl.Controls.Add(theLabel1);

    //        //Label theLabel2 = new Label();
    //        //theLabel2.ID = "lblDrgDose";
    //        //theLabel2.Text = "Single Dose";
    //        //theLabel2.Width = 90;
    //        //theheaderPnl.Controls.Add(theLabel2);

    //        Label theLabel3 = new Label();
    //        theLabel3.ID = "lblDrgUnits";
    //        theLabel3.Text = "Unit";
    //        theLabel3.Width = 90;
    //        theheaderPnl.Controls.Add(theLabel3);

    //        Label theLabel4 = new Label();
    //        theLabel4.ID = "lblDrgFrequency";
    //        theLabel4.Text = "Frequency";
    //        theLabel4.Width = 90;
    //        theheaderPnl.Controls.Add(theLabel4);

    //        //Label theLabel9 = new Label();
    //        //theLabel9.ID = "lblDrgTotalDose";
    //        //theLabel9.Text = "Total Daily Dose";
    //        //theLabel9.Width = 110;
    //        //theheaderPnl.Controls.Add(theLabel9);

    //        Label theLabel5 = new Label();
    //        theLabel5.ID = "lblDrgDuration";
    //        theLabel5.Text = "Duration";
    //        theLabel5.Width = 100;
    //        theheaderPnl.Controls.Add(theLabel5);

    //        Label theLabel6 = new Label();
    //        theLabel6.ID = "lblDrgPrescribed";
    //        theLabel6.Text = "Qty. Prescribed";
    //        theLabel6.Width = 100;
    //        theheaderPnl.Controls.Add(theLabel6);

    //        Label theLabel7 = new Label();
    //        theLabel7.ID = "lblDrgDispensed";
    //        theLabel7.Text = "Qty. Dispensed";
    //        theLabel7.Width = 100;
    //        theheaderPnl.Controls.Add(theLabel7);

    //        //Label theLabel8 = new Label();
    //        //theLabel8.ID = "lblDrgFinanced";
    //        //theLabel8.Text = "AR";
    //        //theLabel8.Width = 20;
    //        //theheaderPnl.Controls.Add(theLabel8);

    //        MstPanel.Controls.Add(theheaderPnl);
    //        #endregion
    //    }

    //    Panel thePnl = new Panel();
    //    thePnl.ID = "pnl" + DrugId + "^" + Generic;
    //    thePnl.Height = 20;
    //    thePnl.Width = 840;
    //    thePnl.Controls.Clear();

    //    Label lblStSp = new Label();
    //    lblStSp.Width = 5;
    //    lblStSp.ID = "stSpace" + DrugId + "^" + Generic;
    //    lblStSp.Text = "";
    //    thePnl.Controls.Add(lblStSp);

    //    DataView theDV;
    //    DataSet theDS = (DataSet)ViewState["MasterData"];

    //    if (Generic == 0)
    //    {
    //        //theDV = new DataView(theDS.Tables[0]); rupesh for both active and inactive drug - 31-07-07
    //        theDV = new DataView(theDS.Tables[12]);
    //        theDV.RowFilter = "drug_pk = " + DrugId;
    //    }
    //    else
    //    {
    //        theDV = new DataView(theDS.Tables[4]);
    //        theDV.RowFilter = "GenericId = " + DrugId;
    //    }

    //    Label theDrugNm = new Label();
    //    theDrugNm.ID = "drgNm" + DrugId + "^" + Generic;
    //    theDrugNm.Text = theDV[0][1].ToString();
    //    theDrugNm.Width = 210;
    //    thePnl.Controls.Add(theDrugNm);

    //    /////// Space//////
    //    Label theSpace = new Label();
    //    theSpace.ID = "theSpace" + DrugId + "^" + Generic;
    //    theSpace.Width = 5;
    //    theSpace.Text = "";
    //    thePnl.Controls.Add(theSpace);
    //    ////////////////////

    //    //TextBox theDose = new TextBox();
    //    //theDose.ID = "drgDose" + DrugId + "^" + Generic;
    //    //theDose.Width = 80;
    //    ////theDose.Text = Generic.ToString();
    //    //theDose.Text = "";
    //    ////theDose.Load += new EventHandler(DecimalText_Load); // rupesh
    //    //theDose.Load += new EventHandler(DecimalText_Load); // rupesh -- OI and Other Medications

    //    //thePnl.Controls.Add(theDose);

    //    ///////// Space//////
    //    //Label theSpace1 = new Label();
    //    //theSpace1.ID = "theSpace1*" + DrugId + "^" + Generic;
    //    //theSpace1.Width = 15;
    //    //theSpace1.Text = "";
    //    //thePnl.Controls.Add(theSpace1);
    //    //////////////////////

    //    BindFunctions theBindMgr = new BindFunctions();
    //    //DropDownList theUnit = new DropDownList();
    //    //theUnit.ID = "theUnit" + DrugId + "^" + Generic;
    //    //theUnit.Width = 80;
    //    //DataTable DTUnit = new DataTable();
    //    //DTUnit = theDS.Tables[7];
    //    //theBindMgr.BindCombo(theUnit, DTUnit, "UnitName", "UnitId");
    //    //thePnl.Controls.Add(theUnit);

    //    /////// Space//////
    //    Label theSpace2 = new Label();
    //    theSpace2.ID = "theSpace2*" + DrugId + "^" + Generic;
    //    theSpace2.Width = 15;
    //    theSpace2.Text = "";
    //    thePnl.Controls.Add(theSpace2);
    //    ////////////////////

    //    DropDownList ddlFrequency = new DropDownList();
    //    ddlFrequency.ID = "drgFrequency" + DrugId + "^" + Generic;
    //    ddlFrequency.Width = 80;
    //    DataTable DTFreq = new DataTable();
    //    //------------rupesh 04-sep --- for OI and other Medication - for custom frq list - starts
    //    //DTFreq = theDS.Tables[8];
    //    if (MstPanel.ID.ToString() == "PnlOtMed")
    //        DTFreq = theDS.Tables[13];
    //    else
    //        DTFreq = theDS.Tables[8];
    //    //----------rupesh 04-sep --- for OI and other Medication - for custom frq list - ends

    //    //ddlFrequency.AutoPostBack = true; // 29Jan08
    //    theBindMgr.BindCombo(ddlFrequency, DTFreq, "FrequencyName", "FrequencyId");
    //    //ddlFrequency.SelectedIndexChanged += new EventHandler(ddlFrequency_SelectedIndexChanged); //29Jan08
    //    thePnl.Controls.Add(ddlFrequency);

    //    ///////// Space//////
    //    //Label theSpace8 = new Label();
    //    //theSpace8.ID = "theSpace8*" + DrugId + "^" + Generic;
    //    //theSpace8.Width = 10;
    //    //theSpace8.Text = "";
    //    //thePnl.Controls.Add(theSpace8);
    //    //////////////////////

    //    //TextBox theTotalDailyDose = new TextBox();
    //    //theTotalDailyDose.ID = "drgTotalDose" + DrugId + "^" + Generic;
    //    //theTotalDailyDose.Width = 100;
    //    //theTotalDailyDose.Text = "";
    //    //theTotalDailyDose.Load += new EventHandler(Control_Load); // rupesh OI and other medications

    //    ////27Feb08
    //    ////ddlFrequency.Attributes.Add("onclick", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgDose" + DrugId + "^" + Generic + "', 'ctl00_clinicalheaderfooter_drgFrequency" + DrugId + "^"+ Generic + "','ctl00_clinicalheaderfooter_drgTotalDose" + DrugId + "^" + Generic + "');");
    //    ////theDose.Attributes.Add("OnBlur", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgDose" + DrugId+ "^" + Generic + "', 'ctl00_clinicalheaderfooter_drgFrequency" + DrugId + "^" + Generic + "','ctl00_clinicalheaderfooter_drgTotalDose" + DrugId + "^" + Generic + "');");

    //    //thePnl.Controls.Add(theTotalDailyDose);

    //    /////// Space//////
    //    Label theSpace3 = new Label();
    //    theSpace3.ID = "theSpace3*" + DrugId + "^" + Generic;
    //    theSpace3.Width = 15;
    //    theSpace3.Text = "";
    //    thePnl.Controls.Add(theSpace3);
    //    ////////////////////

    //    TextBox theDuration = new TextBox();
    //    theDuration.ID = "drgDuration" + DrugId + "^" + Generic;
    //    theDuration.Width = 100;
    //    theDuration.Text = "";
    //    theDuration.Load += new EventHandler(DecimalText_Load);
    //    thePnl.Controls.Add(theDuration);

    //    ////////////Space////////////////////////
    //    Label theSpace4 = new Label();
    //    theSpace4.ID = "theSpace4*" + DrugId + "^" + Generic;
    //    theSpace4.Width = 15;
    //    theSpace4.Text = "";
    //    thePnl.Controls.Add(theSpace4);
    //    ////////////////////////////////////////

    //    TextBox theQtyPrescribed = new TextBox();
    //    theQtyPrescribed.ID = "drgQtyPrescribed" + DrugId + "^" + Generic;
    //    theQtyPrescribed.Width = 100;
    //    theQtyPrescribed.Text = "";
    //    #region "13-Jun-07 - 8"
    //    //theQtyPrescribed.Load += new EventHandler(DecimalText_Load);//rupesh
    //    theQtyPrescribed.Load += new EventHandler(DecimalText_Load); // rupesh
    //    #endregion
    //    thePnl.Controls.Add(theQtyPrescribed);

    //    ////////////Space////////////////////////
    //    Label theSpace5 = new Label();
    //    theSpace5.ID = "theSpace5*" + DrugId + "^" + Generic;
    //    theSpace5.Width = 15;
    //    theSpace5.Text = "";
    //    thePnl.Controls.Add(theSpace5);
    //    ////////////////////////////////////////

    //    TextBox theQtyDispensed = new TextBox();
    //    theQtyDispensed.ID = "drgQtyDispensed" + DrugId + "^" + Generic;
    //    theQtyDispensed.Width = 100;
    //    theQtyDispensed.Text = "";
    //    #region "13-Jun-07 - 9"
    //    //theQtyDispensed.Load += new EventHandler(DecimalText_Load);
    //    theQtyDispensed.Load += new EventHandler(DecimalText_Load); // rupesh
    //    #endregion
    //    thePnl.Controls.Add(theQtyDispensed);

    //    //Label theSpace6 = new Label();
    //    //theSpace6.ID = "theSpace6*" + DrugId + "^" + Generic;
    //    //theSpace6.Width = 10;
    //    //theSpace6.Text = "";
    //    //thePnl.Controls.Add(theSpace6);

    //    //CheckBox theFinChk = new CheckBox();
    //    //theFinChk.ID = "FinChk" + DrugId + "^" + Generic;
    //    //theFinChk.Width = 10;
    //    //theFinChk.Text = "";
    //    //thePnl.Controls.Add(theFinChk);

    //    ////////////Space////////////////////////
    //    Label theSpace7 = new Label();
    //    theSpace7.ID = "theSpace7*" + DrugId + "^" + Generic;
    //    theSpace7.Width = 30;
    //    theSpace7.Text = "";
    //    thePnl.Controls.Add(theSpace7);
    //    ////////////////////////////////////////
    //    LinkButton thelnkRemove = new LinkButton();
    //    thelnkRemove.ID = "lnkrmv%" + MstPanel.ID + "^" + DrugId;
    //    thelnkRemove.Width = 20;
    //    thelnkRemove.Text = "Remove";
    //    thelnkRemove.Click += new EventHandler(Remove_panel);
    //    thelnkRemove.OnClientClick = "return confirm('Are you sure you want to Remove this Drug?')";
    //    thePnl.Controls.Add(thelnkRemove);


    //    /////////Space panel/////////////////////////
    //    Panel thePnlspace = new Panel();
    //    thePnlspace.ID = "pnlspace_" + DrugId;
    //    thePnlspace.Height = 3;
    //    thePnlspace.Width = 900;
    //    thePnlspace.Controls.Clear();
    //    thePnl.Controls.Add(thePnlspace);

    //    MstPanel.Controls.Add(thePnl);

    //}
    private void BindAdditionalDrugControls(int DrugId, int Generic, Panel MstPanel)
    {
        //oi and other medications
        //if (MstPanel.Controls.Count < 1)
        //{
        Control thehdrCntrl = FindControlRecursive(MstPanel, "pnlDrug" + MstPanel.ID);
        if (thehdrCntrl == null)
        {
            #region "OI & Other Medication"
            Panel thelblPnl = new Panel();
            thelblPnl.ID = "pnlDrug" + MstPanel.ID;
            thelblPnl.Height = 20;
            thelblPnl.Width = 900;
            thelblPnl.Controls.Clear();

            Label theLabel = new Label();
            theLabel.ID = "lblDrug" + MstPanel.ID;
            if (MstPanel.ID == "PnlOIARV")
            {
                theLabel.Text = "OI Treatment and Non-HIV/AIDS Medications";//earlier it was "OI Treatment and Other Medications";
            }
            else
                theLabel.Text = "Other Medications";//earlier it was "OI Treatment and Other Medications";

            theLabel.Font.Bold = true;
            thelblPnl.Controls.Add(theLabel);
            MstPanel.Controls.Add(thelblPnl);

            /////////////////////////////////////////////////
            Panel theheaderPnl = new Panel();
            theheaderPnl.ID = "pnlHeaderOtherDrug" + MstPanel.ID;
            theheaderPnl.Height = 20;
            theheaderPnl.Width = 900;
            theheaderPnl.Font.Bold = true;
            theheaderPnl.Controls.Clear();

            Label theSP = new Label();
            theSP.ID = "lblDrgSp" + MstPanel.ID;
            theSP.Width = 5;
            theSP.Text = "";
            theheaderPnl.Controls.Add(theSP);

            Label theLabel1 = new Label();
            theLabel1.ID = "lblDrgNm" + MstPanel.ID;
            theLabel1.Text = "Drug Name";
            theLabel1.Width = 200;
            theheaderPnl.Controls.Add(theLabel1);

            //Label theLabel2 = new Label();
            //theLabel2.ID = "lblDrgDose";
            //theLabel2.Text = "Dose";
            //theLabel2.Width = 90;
            //theheaderPnl.Controls.Add(theLabel2);

            //Label theLabel3 = new Label();
            //theLabel3.ID = "lblDrgUnits";
            //theLabel3.Text = "Unit";
            //theLabel3.Width = 90;
            //theheaderPnl.Controls.Add(theLabel3);
            Label lblSpace = new Label();
            lblSpace.Width = 20;
            lblSpace.ID = "lblSpaceAdd_1" + MstPanel.ID;
            lblSpace.Text = "";
            theheaderPnl.Controls.Add(lblSpace);

            Label theLabel4 = new Label();
            theLabel4.ID = "lblDrgFrequency" + MstPanel.ID;
            theLabel4.Text = "Frequency";
            //theLabel4.Width = 80;
            theheaderPnl.Controls.Add(theLabel4);

            Label lblSpace2 = new Label();
            lblSpace2.Width = 40;
            lblSpace2.ID = "lblSpaceAdd_2" + MstPanel.ID;
            lblSpace2.Text = "";
            theheaderPnl.Controls.Add(lblSpace2);

            Label theLabel5 = new Label();
            theLabel5.ID = "lblDrgDuration" + MstPanel.ID;
            theLabel5.Text = "Duration";
            //theLabel5.Width = 110;
            theheaderPnl.Controls.Add(theLabel5);

            Label lblSpace3 = new Label();
            lblSpace3.Width = 48;
            lblSpace3.ID = "lblSpaceAdd_4" + MstPanel.ID;
            lblSpace3.Text = "";
            theheaderPnl.Controls.Add(lblSpace3);

            Label theLabel6 = new Label();
            theLabel6.ID = "lblDrgPrescribed" + MstPanel.ID;
            theLabel6.Text = "Qty. Prescribed";
            //theLabel6.Width = 110;
            theheaderPnl.Controls.Add(theLabel6);

            Label lblSpace4 = new Label();
            lblSpace4.Width = 15;
            lblSpace4.ID = "lblSpaceAdd_5" + MstPanel.ID;
            lblSpace4.Text = "";
            theheaderPnl.Controls.Add(lblSpace4);

            Label theLabel7 = new Label();
            theLabel7.ID = "lblDrgDispensed" + MstPanel.ID;
            theLabel7.Text = "Qty. Dispensed";
            //theLabel7.Width = 110;
            theheaderPnl.Controls.Add(theLabel7);

            Label lblSpace5 = new Label();
            lblSpace5.Width = 20;
            lblSpace5.ID = "lblSpace_6" + MstPanel.ID;
            lblSpace5.Text = "";
            theheaderPnl.Controls.Add(lblSpace5);

            Label lblProphylaxis = new Label();
            lblProphylaxis.Text = "Prophylaxis";
            lblProphylaxis.ID = "lblProphylaxis" + MstPanel.ID; ;
            lblProphylaxis.Font.Bold = true;
            lblProphylaxis.Visible = true;
            theheaderPnl.Controls.Add(lblProphylaxis);

            //Label theLabel8 = new Label();
            //theLabel8.ID = "lblDrgFinanced";
            //theLabel8.Text = "AR";
            //theLabel8.Width = 20;
            //theheaderPnl.Controls.Add(theLabel8);

            MstPanel.Controls.Add(theheaderPnl);
            #endregion
        }
        //}
        Control thedrgCntrl = FindControlRecursive(MstPanel, "pnl" + DrugId + "^" + Generic);
        if (thedrgCntrl == null)
        {
            Panel thePnl = new Panel();
            thePnl.ID = "pnl" + DrugId + "^" + Generic;
            thePnl.Height = 20;
            thePnl.Width = 900;
            thePnl.Controls.Clear();

            Label lblStSp = new Label();
            lblStSp.Width = 5;
            lblStSp.ID = "stSpace" + DrugId + "^" + Generic;
            lblStSp.Text = "";
            thePnl.Controls.Add(lblStSp);

            DataView theDV;
            DataSet theDS = (DataSet)ViewState["MasterData"];
            if (Generic == 0)
            {
                //theDV = new DataView(theDS.Tables[0]);
                theDV = new DataView(theDS.Tables[12]); // rupesh for both active & inactive drug 31/07/07
                theDV.RowFilter = "drug_pk = " + DrugId;
            }
            else
            {
                theDV = new DataView(theDS.Tables[4]);
                theDV.RowFilter = "GenericId = " + DrugId;

            }

            Label theDrugNm = new Label();
            theDrugNm.ID = "drgNm" + DrugId + "^" + Generic;
            theDrugNm.Text = theDV[0][1].ToString();
            theDrugNm.Width = 200;
            thePnl.Controls.Add(theDrugNm);

            /////// Space//////
            Label theSpace = new Label();
            theSpace.ID = "theSpace" + DrugId + "^" + Generic;
            theSpace.Width = 10;
            theSpace.Text = "";
            thePnl.Controls.Add(theSpace);

            //////////////////////

            BindFunctions theBindMgr = new BindFunctions();

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
            DTFreq = theDS.Tables[8];
            theBindMgr.BindCombo(theFrequency, DTFreq, "FrequencyName", "FrequencyId");
            thePnl.Controls.Add(theFrequency);

            /////// Space//////
            Label theSpace3 = new Label();
            theSpace3.ID = "theSpace3*" + DrugId + "^" + Generic;
            theSpace3.Width = 15;
            theSpace3.Text = "";
            thePnl.Controls.Add(theSpace3);
            ////////////////////

            TextBox theDuration = new TextBox();
            theDuration.ID = "drgDuration" + DrugId + "^" + Generic;
            theDuration.Width = 85;
            theDuration.Text = "";
            theDuration.Load += new EventHandler(Control_Load);
            thePnl.Controls.Add(theDuration);

            ////////////Space////////////////////////
            Label theSpace4 = new Label();
            theSpace4.ID = "theSpace4*" + DrugId + "^" + Generic;
            theSpace4.Width = 15;
            theSpace4.Text = "";
            thePnl.Controls.Add(theSpace4);
            ////////////////////////////////////////

            TextBox theQtyPrescribed = new TextBox();
            theQtyPrescribed.ID = "drgQtyPrescribed" + DrugId + "^" + Generic;
            theQtyPrescribed.Width = 85;
            theQtyPrescribed.Text = "";
            //theQtyPrescribed.Load += new EventHandler(DecimalText_Load);
            theQtyPrescribed.Load += new EventHandler(Control_Load);
            thePnl.Controls.Add(theQtyPrescribed);

            ////////////Space////////////////////////
            Label theSpace5 = new Label();
            theSpace5.ID = "theSpace5*" + DrugId + "^" + Generic;
            theSpace5.Width = 15;
            theSpace5.Text = "";
            thePnl.Controls.Add(theSpace5);
            ////////////////////////////////////////

            TextBox theQtyDispensed = new TextBox();
            theQtyDispensed.ID = "drgQtyDispensed" + DrugId + "^" + Generic;
            theQtyDispensed.Width = 85;
            theQtyDispensed.Text = "";
            #region "13-Jun-07 -3"
            //theQtyDispensed.Load += new EventHandler(DecimalText_Load); 
            theQtyDispensed.Load += new EventHandler(Control_Load); // rupesh
            #endregion
            //if (Session["SCMModule"] != null)
            //    theQtyDispensed.Attributes.Add("onkeyup", "chknotwrite('" + theQtyDispensed.ClientID + "')");
            //    //theQtyDispensed.Enabled = false;
            thePnl.Controls.Add(theQtyDispensed);

            ////////////Space////////////////////////
            Label theSpace6 = new Label();
            theSpace6.ID = "theSpace6*" + DrugId + "^" + Generic;
            theSpace6.Width = 30;
            theSpace6.Text = "";
            thePnl.Controls.Add(theSpace6);
            //////////////////////////////////////////

            ////////////Space///////////////////////

            CheckBox theOtherARTProPhChk = new CheckBox();
            theOtherARTProPhChk.ID = "chkProphylaxis" + DrugId;
            theOtherARTProPhChk.Width = 10;
            theOtherARTProPhChk.Text = "";
            theOtherARTProPhChk.Enabled = true;
            //if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
            //{
            //    theOtherARTProPhChk.Enabled = true;
            //}
            //else
            //    theOtherARTProPhChk.Enabled = false;
            thePnl.Controls.Add(theOtherARTProPhChk);
            //}
            ////////////Space///////////////////////
            Label theSpace9 = new Label();
            theSpace9.ID = "theSpace9" + DrugId;
            theSpace9.Width = 185;
            theSpace9.Text = "";
            thePnl.Controls.Add(theSpace9);

            LinkButton thelnkRemove = new LinkButton();
            thelnkRemove.ID = "lnkrmv%" + MstPanel.ID + "^" + DrugId;
            thelnkRemove.Width = 20;
            thelnkRemove.Text = "Remove";
            thelnkRemove.Click += new EventHandler(Remove_panel);
            if (Session["ExistPharmacyData"] != null)
            {
                if (((DataTable)Session["ExistPharmacyData"]).Rows.Count > 0)
                {
                    IQCareUtils theUtilsCF = new IQCareUtils();
                    DataView theExistDrgDV = new DataView((DataTable)Session["ExistPharmacyData"]);
                    theExistDrgDV.RowFilter = "Drug_Pk=" + DrugId;
                    DataTable theDTfiltdrg = (DataTable)theUtilsCF.CreateTableFromDataView(theExistDrgDV);
                    if (Convert.ToInt32(Session["PatientVisitId"]) > 0 && theDTfiltdrg.Rows.Count > 0)
                    {
                        thelnkRemove.Visible = false;
                    }
                }
            }
            thelnkRemove.OnClientClick = "return confirm('Are you sure you want to Remove this Drug?')";
            thePnl.Controls.Add(thelnkRemove);
            MstPanel.Controls.Add(thePnl);

            /////////Space panel/////////////////////////
            Panel thePnlspace = new Panel();
            thePnlspace.ID = "pnlspace_" + DrugId;
            thePnlspace.Height = 3;
            thePnlspace.Width = 900;
            thePnlspace.Controls.Clear();
            MstPanel.Controls.Add(thePnlspace);
        }

    }

    public Control FindControlRecursive(Control container, string name)
    {
        if (container.ID == name)
            return container;

        foreach (Control ctrl in container.Controls)
        {
            Control foundCtrl = FindControlRecursive(ctrl, name);
            if (foundCtrl != null)
                return foundCtrl;
        }
        return null;
    }
    protected void GetPediatricFields(int PatientID)
    {
        IPediatric PediatricManager;
        try
        {
            PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            //pr_Pharmacy_GetPediatricDetails_Constella
            DataSet theDrugDS = (DataSet)PediatricManager.GetPediatricFields(PatientID);
            #region "FixDoseCombination"

            theDS = new DataSet();
            theDS.Tables.Add(theDrugDS.Tables[17].Copy());//--0--// performance generic abbrv
            theDS.Tables.Add(theDrugDS.Tables[1].Copy());//--1--
            theDS.Tables.Add(theDrugDS.Tables[2].Copy());//--2--
            theDS.Tables.Add(theDrugDS.Tables[3].Copy());//--3--

            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                theDS.Tables.Add(theDrugDS.Tables[4].Copy());//--4-- // only active generics
            else
                theDS.Tables.Add(theDrugDS.Tables[20].Copy());//--4-- // all generics

            theDS.Tables.Add(theDrugDS.Tables[5].Copy());//--5--
            theDS.Tables.Add(theDrugDS.Tables[6].Copy());//--6--
            theDS.Tables.Add(theDrugDS.Tables[7].Copy());//--7--
            theDS.Tables.Add(theDrugDS.Tables[8].Copy());//--8--
            theDS.Tables.Add(theDrugDS.Tables[9].Copy());//--9--
            theDS.Tables.Add(theDrugDS.Tables[10].Copy());//--10--
            theDS.Tables.Add(theDrugDS.Tables[11].Copy());//--11--
            theDS.Tables.Add(theDrugDS.Tables[12].Copy());//--12--//stores both active and inactive drug
            theDS.Tables.Add(theDrugDS.Tables[13].Copy());//--13--//rupesh - custom freq OI treat and other med - 18-sep-07
            theDS.Tables.Add(theDrugDS.Tables[14].Copy());//--14--//rupesh - ARV PRovider - 19-sep-07
            theDS.Tables.Add(theDrugDS.Tables[21].Copy());//--15--//29Feb08 -- Non-ARTDate
            theDS.Tables.Add(theDrugDS.Tables[22].Copy());//--16period taken
            theDS.Tables.Add(theDrugDS.Tables[23].Copy());//-17TB Regimen
            theDS.Tables.Add(theDrugDS.Tables[24].Copy());//-18
            theDS.Tables.Add(theDrugDS.Tables[25].Copy());//-19
            theDS.Tables.Add(theDrugDS.Tables[26].Copy());//-20
            theDS.Tables.Add(theDrugDS.Tables[27].Copy());//-21
            theDS.Tables.Add(theDrugDS.Tables[28].Copy());//-22
            theDS.Tables.Add(theDrugDS.Tables[29].Copy());//-23
            theDS.Tables.Add(theDrugDS.Tables[18].Copy());//--24
            theDS.Tables.Add(theDrugDS.Tables[19].Copy());//--25
            #endregion
            //---rupesh - 09-oct -- for fixed drug strength / frequency -----
            Session["FixDrugStrength"] = theDrugDS.Tables[18];
            Session["FixDrugFreq"] = theDrugDS.Tables[19];
            //---------------------------------------------------------------

            //lblPatientName.Text = theDS.Tables[6].Rows[0]["Name"].ToString();
            ////string theEnroll = Session["AppCountryId"].ToString() + "-" + Session["AppPosID"].ToString() + "-" + Session["AppSatelliteId"].ToString() + "-" + theDS.Tables[6].Rows[0]["PatientEnrollmentID"].ToString();
            //lblpatientenrolment.Text = theDS.Tables[6].Rows[0]["PatientId"].ToString();
            //lblExisPatientID.Text = theDS.Tables[6].Rows[0]["PatientClinicID"].ToString();
            DateTime theDOB = (DateTime)theDS.Tables[6].Rows[0]["DOB"];
            txtDOB.Text = theDOB.ToString(Session["AppDateFormat"].ToString());
            txtYr.Text = theDS.Tables[6].Rows[0]["Age"].ToString();
            txtMon.Text = theDS.Tables[6].Rows[0]["Age1"].ToString();
            ViewState["MasterData"] = theDS;
            BindddlControls(theDS);
            CreateControls(theDS);
            //BindControls(theDS);
            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                BindControls();
            }
            MakeRegimenGenericTable(theDS);
            BindInfantHealthSection((DataSet)ViewState["MasterData"]);
            DataSet objPatientStatus = new DataSet();
            IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
            objPatientStatus = DrugManager.GetPatientRecordformStatus(Convert.ToInt32(PatientID));
            //if (objPatientStatus != null)
            //{
            //    if (objPatientStatus.Tables[3].Rows.Count > 0)
            //    {
            //        string ARTStatus = objPatientStatus.Tables[3].Rows[0]["status"].ToString();

            //        if (ARTStatus == "ART" || ARTStatus == "UnKnown" || ARTStatus == "Non-ART" || ARTStatus == "PMTCT")
            //        {
            //            btnsave.Enabled = true;
            //        }
            //        else
            //        {
            //            btnsave.Enabled = false;
            //        }
            //    }
            //}

        }
        catch (Exception er)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = er.Message.ToString();
            IQCareMsgBox.Show("C1#", theBuilder, this);
        }

    }

    private void CreateControls(DataSet theCntlDS)
    {
        DataSet theDS = new DataSet();
        //theDS.ReadXml(Server.MapPath("..\\XMLFiles\\pediatricpharmacylist.xml"));

        #region CreateTable"
        //DataTable theDrugTable = new DataTable();

        if (ViewState["SelectedDrug"] != null)
        {
            theDrugTable = (DataTable)ViewState["SelectedDrug"];
        }
        else
        {
            theDrugTable = MakeTable();
            //foreach (DataRow dr in theDS.Tables[0].Rows)
            //{
            //    DataRow theDR = theDrugTable.NewRow();
            //    if (Convert.ToInt32(dr[2]) == 1)
            //    {
            //        theDR["DrugId"] = 0;
            //        theDR["DrugName"] = "";
            //        theDR["GenericId"] = dr[0];
            //        theDR["GenericName"] = dr[1];
            //    }
            //    else
            //    {
            //        theDR["DrugId"] = dr[0];
            //        theDR["DrugName"] = dr[1];
            //        theDR["GenericId"] = 0;
            //        theDR["GenericName"] = "";
            //    }
            //    theDrugTable.Rows.Add(theDR);
            //}
        }

        #endregion


        int theDrugId;

        //string pnlName = "pnlPedia";

        //Label lblSpace0 = new Label();
        //lblSpace0.Width = 35;
        //lblSpace0.ID = "lblSpace_0";
        //lblSpace0.Text = "";


        //Label lblDrugName = new Label();
        //lblDrugName.Text = "Drug Name";
        //lblDrugName.ID = "lblDrugName";
        //lblDrugName.Font.Bold = true;
        //lblDrugName.Visible = true;

        //Label lblSpace = new Label();
        //lblSpace.Width = 50;
        //lblSpace.ID = "lblSpace";
        //lblSpace.Text = "";

        //Label lblFormulation = new Label();
        //lblFormulation.Text = "Formulation";
        //lblFormulation.ID = "lblFormulation";
        //lblFormulation.Font.Bold = true;
        //lblFormulation.Visible = true;

        //Label lblSpace1 = new Label();
        //lblSpace1.Width = 22;
        //lblSpace1.ID = "lblSpace_1";
        //lblSpace1.Text = "";

        //Label lblDose = new Label();
        //lblDose.Text = "Single Dose";
        //lblDose.ID = "lblDose";
        //lblDose.Font.Bold = true;
        //lblDose.Visible = true;

        //Label lblSpace2 = new Label();
        //lblSpace2.Width = 30;
        //lblSpace2.ID = "lblSpace_2";
        //lblSpace2.Text = "";

        //Label lblFrequency = new Label();
        //lblFrequency.Text = "Frequency";
        //lblFrequency.ID = "lblFrequency";
        //lblFrequency.Font.Bold = true;
        //lblFrequency.Visible = true;

        //Label lblSpace3 = new Label();
        //lblSpace3.Width = 30;
        //lblSpace3.ID = "lblSpace_3";
        //lblSpace3.Text = "";

        //Label lblTotalDose = new Label();
        //lblTotalDose.Text = "Total Daily Dose";
        //lblTotalDose.ID = "lblTotalDose";
        //lblTotalDose.Font.Bold = true;
        //lblTotalDose.Visible = true;

        //Label lblSpace4 = new Label();
        //lblSpace4.Width = 40;
        //lblSpace4.ID = "lblSpace_4";
        //lblSpace4.Text = "";

        //Label lblDuration = new Label();
        //lblDuration.Text = "Duration";
        //lblDuration.ID = "lblDuration";
        //lblDuration.Font.Bold = true;
        //lblDuration.Visible = true;

        //Label lblSpace5 = new Label();
        //lblSpace5.Width = 40;
        //lblSpace5.ID = "lblSpace_5";
        //lblSpace5.Text = "";

        //Label lblQtyPrescribed = new Label();
        //lblQtyPrescribed.Text = "Qty Prescribed";
        //lblQtyPrescribed.ID = "lblQuantityPres";
        //lblQtyPrescribed.Font.Bold = true;
        //lblQtyPrescribed.Visible = true;

        //Label lblSpace6 = new Label();
        //lblSpace6.Width = 20;
        //lblSpace6.ID = "lblSpace_6";
        //lblSpace6.Text = "";

        //Label lblQtyDispensed = new Label();
        //lblQtyDispensed.Text = "Qty Dispensed";
        //lblQtyDispensed.ID = "lblQuantityDisp";
        //lblQtyDispensed.Font.Bold = true;
        //lblQtyDispensed.Visible = true;


        //Label lblSpace7 = new Label();
        //lblSpace7.Width = 20;
        //lblSpace7.ID = "lblSpace_10";
        //lblSpace7.Text = "";

        //Label lblProphylaxis = new Label();
        //lblProphylaxis.Text = "Prophylaxis";
        //lblProphylaxis.ID = "lblProphylaxis";
        //lblProphylaxis.Font.Bold = true;
        //lblProphylaxis.Visible = true;

        //pnlPedia.Controls.Add(lblDrugName);
        //pnlPedia.Controls.Add(lblSpace);
        //pnlPedia.Controls.Add(lblFormulation);
        //pnlPedia.Controls.Add(lblSpace1);
        //pnlPedia.Controls.Add(lblDose);
        //pnlPedia.Controls.Add(lblSpace2);
        //pnlPedia.Controls.Add(lblFrequency);
        //pnlPedia.Controls.Add(lblSpace3);
        //pnlPedia.Controls.Add(lblTotalDose);
        //pnlPedia.Controls.Add(lblSpace4);
        //pnlPedia.Controls.Add(lblDuration);
        //pnlPedia.Controls.Add(lblSpace5);
        //pnlPedia.Controls.Add(lblQtyPrescribed);
        //pnlPedia.Controls.Add(lblSpace6);
        //pnlPedia.Controls.Add(lblQtyDispensed);
        //pnlPedia.Controls.Add(lblSpace7);
        //pnlPedia.Controls.Add(lblProphylaxis);
        Panel thePnlNRTI = new Panel();
        thePnlNRTI.ID = "PnlRegline";
        Label Space = new Label();
        Label DDSpace = new Label();
        Space.Width = 10;
        DDSpace.Width = 10;
        Label theHeadingRegimenLine = new Label();
        theHeadingRegimenLine.Text = "*Regimen Line";
        theHeadingRegimenLine.CssClass = "required";
        theHeadingRegimenLine.Font.Bold = true;
        DropDownList theRegimenLine = new DropDownList();
        theRegimenLine.ID = "DDRegimenLine";
        DataView theDV = new DataView(theCntlDS.Tables[20]);
        BindFunctions BindManager = new BindFunctions();
        theDV.RowFilter = "DeleteFlag='0'";
        if (theDV.Table != null)
        {
            BindManager.BindCombo(theRegimenLine, theDV.Table, "Name", "ID");
        }

        if (theCntlDS.Tables[21].Rows[0]["RegimenLine"] != System.DBNull.Value)
        {
            theRegimenLine.SelectedValue = Convert.ToString(theCntlDS.Tables[21].Rows[0]["RegimenLine"]);
        }
        //thePnlNRTI.Controls.Add(Space);
        //thePnlNRTI.Controls.Add(theHeadingRegimenLine);
        //thePnlNRTI.Controls.Add(DDSpace);
        //thePnlNRTI.Controls.Add(theRegimenLine);
        //thePnl.Controls.Add(thePnlNRTI);

        PnlRegiment.Controls.Add(Space);
        PnlRegiment.Controls.Add(theHeadingRegimenLine);
        PnlRegiment.Controls.Add(DDSpace);
        PnlRegiment.Controls.Add(theRegimenLine);
        PnlRegiment.Controls.Add(thePnlNRTI);
        try
        {
            int i = 0;
            int theGenericId = 0;
            foreach (DataRow dr in theDrugTable.Rows)
            {
                theDrugId = 0;
                theDrugId = Convert.ToInt32(dr[0].ToString());

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
                //thePnl.Height = 20;
                thePnl.Width = 870;
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
                    if (theHeading.Text == "NRTI")
                    {
                        //Panel thePnlNRTI = new Panel();
                        //thePnlNRTI.ID = "PnlRegline";
                        //Label Space = new Label();
                        //Label DDSpace = new Label();
                        //Space.Width = 10;
                        //DDSpace.Width = 10;
                        //Label theHeadingRegimenLine = new Label();
                        //theHeadingRegimenLine.Text = "*Regimen Line";
                        //theHeadingRegimenLine.CssClass = "required";
                        //theHeadingRegimenLine.Font.Bold = true;
                        //DropDownList theRegimenLine = new DropDownList();
                        //theRegimenLine.ID = "DDRegimenLine";
                        //DataView theDV = new DataView(theCntlDS.Tables[20]);
                        //BindFunctions BindManager = new BindFunctions();
                        //theDV.RowFilter = "DeleteFlag='0'";
                        //if (theDV.Table != null)
                        //{
                        //    BindManager.BindCombo(theRegimenLine, theDV.Table, "Name", "ID");
                        //}

                        //if (theCntlDS.Tables[21].Rows[0]["RegimenLine"] != System.DBNull.Value)
                        //{
                        //    theRegimenLine.SelectedValue = Convert.ToString(theCntlDS.Tables[21].Rows[0]["RegimenLine"]);
                        //}
                        ////thePnlNRTI.Controls.Add(Space);
                        ////thePnlNRTI.Controls.Add(theHeadingRegimenLine);
                        ////thePnlNRTI.Controls.Add(DDSpace);
                        ////thePnlNRTI.Controls.Add(theRegimenLine);
                        ////thePnl.Controls.Add(thePnlNRTI);

                        //PnlRegiment.Controls.Add(Space);
                        //PnlRegiment.Controls.Add(theHeadingRegimenLine);
                        //PnlRegiment.Controls.Add(DDSpace);
                        //PnlRegiment.Controls.Add(theRegimenLine);
                        //PnlRegiment.Controls.Add(thePnlNRTI);
                    }

                }
                else
                {

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
                    theDrugNm.Width = 80;
                    thePnl.Controls.Add(theDrugNm);

                    Label theSpace = new Label();
                    theSpace.ID = "lblSp1" + theGenericId;
                    theSpace.Width = 18;
                    theSpace.Text = "";
                    thePnl.Controls.Add(theSpace);

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
                    Label theSpace6 = new Label();
                    theSpace6.ID = "lblSp6" + theGenericId;
                    theSpace6.Width = 20;
                    theSpace6.Text = "";
                    thePnl.Controls.Add(theSpace6);
                    //////////////////////////////////////////

                    TextBox theDose = new TextBox();
                    theDose.ID = "drgDose" + theGenericId;
                    theDose.Width = 86;
                    theDose.Load += new EventHandler(DecimalText_Load);
                    thePnl.Controls.Add(theDose);

                    //////////////Space////////////////////////
                    Label theSpace1 = new Label();
                    theSpace1.ID = "lblSp2" + theGenericId;
                    theSpace1.Width = 20;
                    theSpace1.Text = "";
                    thePnl.Controls.Add(theSpace1);
                    //////////////////////////////////////////

                    DropDownList ddlFrequency = new DropDownList();
                    ddlFrequency.ID = "drgFrequency" + theGenericId;
                    ddlFrequency.Width = 80;
                    //ddlFrequency.AutoPostBack = true; //29Jan08 -- changed for doing in javascript
                    #region "BindCombo"
                    DataTable theDTF = new DataTable();
                    DataView theDVFrequency = new DataView(theCntlDS.Tables[2]);
                    if (Convert.ToInt32(dr["GenericId"]) > 0)
                    {
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
                        theBindMgr.BindCombo(ddlFrequency, theDTFrequency, "FrequencyName", "FrequencyId");
                    }
                    #endregion
                    //29Jan08 -- changed for doing in javascript
                    //ddlFrequency.SelectedIndexChanged += new EventHandler(ddlFrequency_SelectedIndexChanged);
                    thePnl.Controls.Add(ddlFrequency);

                    //////////////Space////////////////////////
                    //Label theSpace7 = new Label();
                    //theSpace7.ID = "lblSp7" + theGenericId;
                    //theSpace7.Width = 20;
                    //theSpace7.Text = "";
                    //thePnl.Controls.Add(theSpace7);
                    ////////////////////////////////////////////

                    //TextBox theTotalDose = new TextBox();
                    //theTotalDose.ID = "drgTotalDose" + theGenericId;
                    //theTotalDose.Width = 86;
                    //theTotalDose.Load += new EventHandler(Control_Load);

                    ////Rupesh 14Jan08 - to fill totaldose thru javascript 
                    //theDrugStrength.Attributes.Add("onblur", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgDose" + theGenericId + "', 'ctl00_clinicalheaderfooter_drgFrequency" + theGenericId + "','ctl00_clinicalheaderfooter_drgTotalDose" + theGenericId + "');");
                    //ddlFrequency.Attributes.Add("Onclick", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgDose" + theGenericId + "', 'ctl00_clinicalheaderfooter_drgFrequency" + theGenericId + "','ctl00_clinicalheaderfooter_drgTotalDose" + theGenericId + "');");
                    //theDose.Attributes.Add("OnBlur", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgDose" + theGenericId + "', 'ctl00_clinicalheaderfooter_drgFrequency" + theGenericId + "','ctl00_clinicalheaderfooter_drgTotalDose" + theGenericId + "');");
                    ////Rupesh 14Jan08
                    //thePnl.Controls.Add(theTotalDose);

                    //////////////Space////////////////////////
                    Label theSpace2 = new Label();
                    theSpace2.ID = "lblSp3" + theGenericId;
                    theSpace2.Width = 20;
                    theSpace2.Text = "";
                    thePnl.Controls.Add(theSpace2);
                    //////////////////////////////////////////

                    TextBox theDuration = new TextBox();
                    theDuration.ID = "drgDuration" + theGenericId;
                    theDuration.Width = 86;
                    theDuration.Load += new EventHandler(DecimalText_Load);
                    thePnl.Controls.Add(theDuration);

                    //////////////Space////////////////////////
                    Label theSpace3 = new Label();
                    theSpace3.ID = "lblSp4" + theGenericId;
                    theSpace3.Width = 25;
                    theSpace3.Text = "";
                    thePnl.Controls.Add(theSpace3);
                    //////////////////////////////////////////

                    TextBox theQtyPrescribed = new TextBox();
                    theQtyPrescribed.ID = "drgQtyPrescribed" + theGenericId;
                    theQtyPrescribed.Width = 86;
                    theQtyPrescribed.Load += new EventHandler(DecimalText_Load);
                    thePnl.Controls.Add(theQtyPrescribed);
                    //////////////Space////////////////////////
                    Label theSpace4 = new Label();
                    theSpace4.ID = "lblSp5" + theGenericId;
                    theSpace4.Width = 25;
                    theSpace4.Text = "";
                    thePnl.Controls.Add(theSpace4);
                    //////////////////////////////////////////

                    TextBox theQtyDispensed = new TextBox();
                    theQtyDispensed.ID = "drgQtyDispensed" + theGenericId;
                    theQtyDispensed.Width = 86;
                    theQtyDispensed.Load += new EventHandler(DecimalText_Load);
                    thePnl.Controls.Add(theQtyDispensed);
                    //////////////Space////////////////////////
                    Label theSpace5 = new Label();
                    theSpace5.ID = "lblSp8" + theGenericId;
                    theSpace5.Width = 20;
                    theSpace5.Text = "";
                    thePnl.Controls.Add(theSpace5);

                    //Prophylaxis
                    CheckBox ProphylaxisChk = new CheckBox();
                    ProphylaxisChk.ID = "chkProphylaxis" + theGenericId;
                    ProphylaxisChk.Width = 10;
                    ProphylaxisChk.Text = "";
                    thePnl.Controls.Add(ProphylaxisChk);

                    ////////////Space///////////////////////
                    Label theSpace8 = new Label();
                    theSpace8.ID = "lblSp11" + theGenericId;
                    theSpace8.Width = 20;
                    theSpace8.Text = "";
                    thePnl.Controls.Add(theSpace8);
                    //////////////////////////////////////////
                    if (theGenericId == 281 || theGenericId == 150)
                    {
                        ProphylaxisChk.Visible = true;
                        Label theSpacehidden = new Label();
                        theSpacehidden.ID = "theSpacehidden" + theGenericId;
                        theSpacehidden.Width = 20;
                        theSpacehidden.Text = "";
                        thePnl.Controls.Add(theSpacehidden);
                    }
                    if (i == 0)
                    {
                        hidchkbox.Value = ProphylaxisChk.ID;
                    }
                    else
                    {
                        hidchkbox.Value = hidchkbox.Value + "," + ProphylaxisChk.ID;
                    }
                    i = i + 1;

                }

                if (theGenericId == 10006 || theGenericId == 281 || theGenericId == 150)
                {
                    //PnlOIARV.Controls.Add(thePnl);
                }
                else
                {
                    pnlPedia.Controls.Add(thePnl);
                }

                AddControlsAttributes(pnlPedia);
            }

            #region "Fixed Combination"
            //Panel FixedLabel = new Panel();

            ////Label lblFixedlabelSpace0 = new Label();
            ////lblFixedlabelSpace0.Width = 10;
            ////lblFixedlabelSpace0.ID = "lblFixedlabelSpace_0";
            ////lblFixedlabelSpace0.Text = "";

            ////FixedLabel.Controls.Add(lblFixedlabelSpace0);


            ////Label lblFixedlabelName = new Label();
            ////lblFixedlabelName.Text = "Fixed Dose Combinations";
            ////lblFixedlabelName.ID = "lblFixedlabelName";
            ////lblFixedlabelName.Font.Bold = true;
            ////lblFixedlabelName.Font.Size = 11;
            ////lblFixedlabelName.Visible = true;

            ////FixedLabel.Controls.Add(lblFixedlabelName);

            //PnlFixed.Controls.Add(FixedLabel);

            //Label lblFixedSpace0 = new Label();
            //lblFixedSpace0.Width = 35;
            //lblFixedSpace0.ID = "lblFixedSpace_0";
            //lblFixedSpace0.Text = "";


            //Label lblFixedDrugName = new Label();
            //lblFixedDrugName.Text = "Drug Name";
            //lblFixedDrugName.ID = "lblFixedDrugName";
            //lblFixedDrugName.Font.Bold = true;
            //lblFixedDrugName.Visible = true;

            //Label lblFixedSpace = new Label();
            //lblFixedSpace.Width = 60;
            //lblFixedSpace.ID = "lblFixedSpace";
            //lblFixedSpace.Text = "";

            //Label lblFixedFormulation = new Label();
            //lblFixedFormulation.Text = "Formulation";
            //lblFixedFormulation.ID = "lblFixedFormulation";
            //lblFixedFormulation.Font.Bold = true;
            //lblFixedFormulation.Visible = true;

            //Label lblFixedSpace1 = new Label();
            //lblFixedSpace1.Width = 22;
            //lblFixedSpace1.ID = "lblFixedSpace_1";
            //lblFixedSpace1.Text = "";

            //Label lblFixedDose = new Label();
            //lblFixedDose.Text = "Single Dose";
            //lblFixedDose.ID = "lblFixedDose";
            //lblFixedDose.Font.Bold = true;
            //lblFixedDose.Visible = false;

            //Label lblFixedSpace2 = new Label();
            //lblFixedSpace2.Width = 30;
            //lblFixedSpace2.ID = "lblFixedSpace_2";
            //lblFixedSpace2.Text = "";

            //Label lblFixedFrequency = new Label();
            //lblFixedFrequency.Text = "Frequency";
            //lblFixedFrequency.ID = "lblFixedFrequency";
            //lblFixedFrequency.Font.Bold = true;
            //lblFixedFrequency.Visible = true;

            //Label lblFixedSpace3 = new Label();
            //lblFixedSpace3.Width = 20;
            //lblFixedSpace3.ID = "lblFixedSpace_3";
            //lblFixedSpace3.Text = "";

            //Label lblFixedTotalDose = new Label();
            //lblFixedTotalDose.Text = "Total Daily Dose";
            //lblFixedTotalDose.ID = "lblFixedTotalDose";
            //lblFixedTotalDose.Font.Bold = true;
            //lblFixedTotalDose.Visible = false;

            //Label lblFixedSpace4 = new Label();
            //lblFixedSpace4.Width = 40;
            //lblFixedSpace4.ID = "lblFixedSpace_4";
            //lblFixedSpace4.Text = "";

            //Label lblFixedDuration = new Label();
            //lblFixedDuration.Text = "Duration";
            //lblFixedDuration.ID = "lblFixedDuration";
            //lblFixedDuration.Font.Bold = true;
            //lblFixedDuration.Visible = true;

            //Label lblFixedSpace5 = new Label();
            //lblFixedSpace5.Width = 75;
            //lblFixedSpace5.ID = "lblFixedSpace_5";
            //lblFixedSpace5.Text = "";

            //Label lblFixedQtyPrescribed = new Label();
            //lblFixedQtyPrescribed.Text = "Qty Prescribed";
            //lblFixedQtyPrescribed.ID = "lblFixedQuantityPres";
            //lblFixedQtyPrescribed.Font.Bold = true;
            //lblFixedQtyPrescribed.Visible = true;

            //Label lblFixedSpace6 = new Label();
            //lblFixedSpace6.Width = 40;
            //lblFixedSpace6.ID = "lblFixedSpace_6";
            //lblFixedSpace6.Text = "";

            //Label lblFixedQtyDispensed = new Label();
            //lblFixedQtyDispensed.Text = "Qty Dispensed";
            //lblFixedQtyDispensed.ID = "lblFixedQuantityDisp";
            //lblFixedQtyDispensed.Font.Bold = true;
            //lblFixedQtyDispensed.Visible = true;


            //Label lblFixedSpace7 = new Label();
            //lblFixedSpace7.Width = 45;
            //lblFixedSpace7.ID = "lblFixedSpace_10";
            //lblFixedSpace7.Text = "";

            //Label lblFixedProphylaxis = new Label();
            //lblFixedProphylaxis.Text = "Prophylaxis";
            //lblFixedProphylaxis.ID = "lblFixedProphylaxis";
            //lblFixedProphylaxis.Font.Bold = true;
            //lblFixedProphylaxis.Visible = true;

            //PnlFixed.Controls.Add(lblFixedDrugName);
            //PnlFixed.Controls.Add(lblFixedSpace);
            //PnlFixed.Controls.Add(lblFixedFormulation);
            //PnlFixed.Controls.Add(lblFixedSpace1);
            //PnlFixed.Controls.Add(lblFixedDose);
            //PnlFixed.Controls.Add(lblFixedSpace2);
            //PnlFixed.Controls.Add(lblFixedFrequency);
            //PnlFixed.Controls.Add(lblFixedSpace3);
            //PnlFixed.Controls.Add(lblFixedTotalDose);
            //PnlFixed.Controls.Add(lblFixedSpace4);
            //PnlFixed.Controls.Add(lblFixedDuration);
            //PnlFixed.Controls.Add(lblFixedSpace5);
            //PnlFixed.Controls.Add(lblFixedQtyPrescribed);
            //PnlFixed.Controls.Add(lblFixedSpace6);
            //PnlFixed.Controls.Add(lblFixedQtyDispensed);
            //PnlFixed.Controls.Add(lblFixedSpace7);
            //PnlFixed.Controls.Add(lblFixedProphylaxis);
            #region "Add Fixed Control and BindControls"
            //Panel theFixPnlCntrl = new Panel();
            //theFixPnlCntrl.ID = "theFixPnlCntrl";
            //string strBrowser1 = Request.Browser.Browser;
            //if (strBrowser1 == "IE")
            //{
            //    theFixPnlCntrl.Height = 20;
            //}
            //else
            //{
            //    theFixPnlCntrl.Height = 30;
            //}
            //theFixPnlCntrl.ID = "theFixPnlCntrl";
            //theFixPnlCntrl.Width = 870;
            //theFixPnlCntrl.Controls.Clear();
            //BindFunctions theFixedBindMgr = new BindFunctions();
            //DropDownList theFixedDrugName = new DropDownList();
            //theFixedDrugName.ID = "theFixedDrugName";
            //theFixedDrugName.Width = 80;
            //theFixedDrugName.AutoPostBack = true;
            //theFixedDrugName.Font.Size = 8;
            //#region "BindCombo"

            //DataTable theFixDTName = new DataTable();
            //DataView theDVDrgnm = new DataView(theCntlDS.Tables[23]);

            //if (theDVDrgnm.Count > 0)
            //{
            //    IQCareUtils theUtils = new IQCareUtils();
            //    theFixDTName = theUtils.CreateTableFromDataView(theDVDrgnm);
            //    theFixedBindMgr.BindCombo(theFixedDrugName, theFixDTName, "FixedDrug", "drug_pk");
            //}

            //#endregion
            ////theFixedDrugName.SelectedIndexChanged += new EventHandler(theFixedDrugName_SelectedIndexChanged);
            //theFixedDrugName.SelectedIndexChanged += new EventHandler(ddlFixDrugname_SelectedChanged);
            //theFixPnlCntrl.Controls.Add(theFixedDrugName);

            //Label theFixSpace = new Label();
            //theFixSpace.ID = "lblFixSp1";
            //theFixSpace.Width = 40;
            //theFixSpace.Text = "";
            //theFixPnlCntrl.Controls.Add(theFixSpace);

            ////BindFunctions theBindMgr = new BindFunctions();
            //DropDownList theFixedDrugStrength = new DropDownList();
            //theFixedDrugStrength.ID = "theFixedDrugStrength";
            //theFixedDrugStrength.Width = 80;
            //#region "BindCombo"

            ////DataTable theDTS = new DataTable();
            ////DataView theDVStrength = new DataView(theCntlDS.Tables[1]);

            ////if (Convert.ToInt32(dr["GenericId"]) > 0)
            ////{
            ////    theDVStrength.RowFilter = "GenericId = " + theGenericId;
            ////}
            ////else
            ////{
            ////    theDTS = (DataTable)Session["FixDrugStrength"];
            ////    theDVStrength = new DataView(theDTS);
            ////    theDVStrength.RowFilter = "Drug_pk = " + Convert.ToInt32(dr["DrugId"]) + " and StrengthId>0";
            ////}
            ////DataTable theDTStrength = new DataTable();
            ////if (theDVStrength.Count > 0)
            ////{
            ////    IQCareUtils theUtils = new IQCareUtils();
            ////    theDTStrength = theUtils.CreateTableFromDataView(theDVStrength);
            ////    theBindMgr.BindCombo(theDrugStrength, theDTStrength, "StrengthName", "StrengthId");
            ////}

            //#endregion
            //theFixPnlCntrl.Controls.Add(theFixedDrugStrength);

            ////////////////Space////////////////////////
            //Label theFixSpace6 = new Label();
            //theFixSpace6.ID = "lblFixSp6";
            //theFixSpace6.Width = 50;
            //theFixSpace6.Text = "";
            ////theFixPnlCntrl.Controls.Add(theFixSpace6);
            ////////////////////////////////////////////

            //TextBox theFixDose = new TextBox();
            //theFixDose.ID = "drgFixDose";
            //theFixDose.Width = 86;
            //theFixDose.Load += new EventHandler(DecimalText_Load);
            ////theFixPnlCntrl.Controls.Add(theFixDose);

            ////////////////Space////////////////////////
            //Label theFixSpace1 = new Label();
            //theFixSpace1.ID = "lblFixSp2";
            //theFixSpace1.Width = 40;
            //theFixSpace1.Text = "";
            //theFixPnlCntrl.Controls.Add(theFixSpace1);
            ////////////////////////////////////////////

            //DropDownList ddlFixFrequency = new DropDownList();
            //ddlFixFrequency.ID = "drgFixFrequency";
            //ddlFixFrequency.Width = 80;
            ////ddlFrequency.AutoPostBack = true; //29Jan08 -- changed for doing in javascript
            ////#region "BindCombo"
            ////       DataTable theDTF = new DataTable();
            ////       DataView theDVFrequency = new DataView(theCntlDS.Tables[2]);
            ////       if (Convert.ToInt32(dr["GenericId"]) > 0)
            ////       {
            ////           theDVFrequency.RowFilter = "GenericId = " + theGenericId;
            ////       }
            ////       else
            ////       {
            ////           theDTF=(DataTable)Session["FixDrugFreq"];
            ////           theDVFrequency = new DataView(theDTF);
            ////           theDVFrequency.RowFilter = "Drug_pk = " + Convert.ToInt32(dr["DrugId"]) + " and FrequencyId>0";
            ////       }
            ////       DataTable theDTFrequency = new DataTable();
            ////       if (theDVFrequency.Count > 0)
            ////       {
            ////           IQCareUtils theUtils = new IQCareUtils();
            ////           theDTFrequency = theUtils.CreateTableFromDataView(theDVFrequency);
            ////           theBindMgr.BindCombo(ddlFrequency, theDTFrequency, "FrequencyName", "FrequencyId");
            ////       }
            ////#endregion
            ////29Jan08 -- changed for doing in javascript
            ////ddlFrequency.SelectedIndexChanged += new EventHandler(ddlFrequency_SelectedIndexChanged);
            //theFixPnlCntrl.Controls.Add(ddlFixFrequency);

            ////////////////Space////////////////////////
            //Label theFixSpace7 = new Label();
            //theFixSpace7.ID = "lblFixSp7";
            //theFixSpace7.Width = 40;
            //theFixSpace7.Text = "";
            ////theFixPnlCntrl.Controls.Add(theFixSpace7);
            ////////////////////////////////////////////

            //TextBox theFixTotalDose = new TextBox();
            //theFixTotalDose.ID = "drgFixTotalDose";
            //theFixTotalDose.Width = 86;
            //theFixTotalDose.Load += new EventHandler(Control_Load);

            ////Rupesh 14Jan08 - to fill totaldose thru javascript 
            ////theFixedDrugStrength.Attributes.Add("onblur", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgFixDose', 'ctl00_clinicalheaderfooter_drgFixFrequency','ctl00_clinicalheaderfooter_drgFixTotalDose');");
            ////ddlFixFrequency.Attributes.Add("Onclick", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgFixDose', 'ctl00_clinicalheaderfooter_drgFixFrequency','ctl00_clinicalheaderfooter_drgFixTotalDose');");
            ////theFixDose.Attributes.Add("OnBlur", "CalculateTotalDailyDose('ctl00_clinicalheaderfooter_drgFixDose', 'ctl00_clinicalheaderfooter_drgFixFrequency','ctl00_clinicalheaderfooter_drgFixTotalDose');");
            ////Rupesh 14Jan08
            ////theFixPnlCntrl.Controls.Add(theFixTotalDose);

            ////////////////Space////////////////////////
            //Label theFixSpace2 = new Label();
            //theFixSpace2.ID = "lblFixSp3";
            //theFixSpace2.Width = 40;
            //theFixSpace2.Text = "";
            //theFixPnlCntrl.Controls.Add(theFixSpace2);
            ////////////////////////////////////////////

            //TextBox theFixDuration = new TextBox();
            //theFixDuration.ID = "drgFixDuration";
            //theFixDuration.Width = 86;
            //theFixDuration.Load += new EventHandler(DecimalText_Load);
            //theFixPnlCntrl.Controls.Add(theFixDuration);

            ////////////////Space////////////////////////
            //Label theFixSpace3 = new Label();
            //theFixSpace3.ID = "lblFixSp4";
            //theFixSpace3.Width = 40;
            //theFixSpace3.Text = "";
            //theFixPnlCntrl.Controls.Add(theFixSpace3);
            ////////////////////////////////////////////

            //TextBox theFixQtyPrescribed = new TextBox();
            //theFixQtyPrescribed.ID = "drgFixQtyPrescribed";
            //theFixQtyPrescribed.Width = 86;
            //theFixQtyPrescribed.Load += new EventHandler(DecimalText_Load);
            //theFixPnlCntrl.Controls.Add(theFixQtyPrescribed);
            ////////////////Space////////////////////////
            //Label theFixSpace4 = new Label();
            //theFixSpace4.ID = "lblFixSp5";
            //theFixSpace4.Width = 40;
            //theFixSpace4.Text = "";
            //theFixPnlCntrl.Controls.Add(theFixSpace4);
            ////////////////////////////////////////////

            //TextBox theFixQtyDispensed = new TextBox();
            //theFixQtyDispensed.ID = "drgFixQtyDispensed";
            //theFixQtyDispensed.Width = 86;
            //theFixQtyDispensed.Load += new EventHandler(DecimalText_Load);
            //theFixPnlCntrl.Controls.Add(theFixQtyDispensed);
            ////////////////Space////////////////////////
            //Label theFixSpace5 = new Label();
            //theFixSpace5.ID = "lblFixSp8";
            //theFixSpace5.Width = 40;
            //theFixSpace5.Text = "";
            //theFixPnlCntrl.Controls.Add(theFixSpace5);

            ////Prophylaxis
            //CheckBox ProphylaxisFixChk = new CheckBox();
            //ProphylaxisFixChk.ID = "chkFixProphylaxis";
            //ProphylaxisFixChk.Width = 10;
            //ProphylaxisFixChk.Text = "";
            //theFixPnlCntrl.Controls.Add(ProphylaxisFixChk);

            //////////////Space///////////////////////
            //Label theFixSpace8 = new Label();
            //theFixSpace8.ID = "lblFixSp11";
            //theFixSpace8.Width = 40;
            //theFixSpace8.Text = "";
            //theFixPnlCntrl.Controls.Add(theFixSpace8);
            ////////////////////////////////////////////
            //if (theGenericId == 281 || theGenericId == 150)
            //{
            //    ProphylaxisFixChk.Visible = true;
            //    Label theFixSpacehidden = new Label();
            //    theFixSpacehidden.ID = "theFixSpacehidden";
            //    theFixSpacehidden.Width = 20;
            //    theFixSpacehidden.Text = "";
            //    theFixPnlCntrl.Controls.Add(theFixSpacehidden);
            //}
            //if (i == 0)
            //{
            //    hidchkbox.Value = ProphylaxisFixChk.ID;
            //}
            //else
            //{
            //    hidchkbox.Value = hidchkbox.Value + "," + ProphylaxisFixChk.ID;
            //}

            //PnlFixed.Controls.Add(theFixPnlCntrl);
            //AddControlsAttributes(PnlFixed);
            #endregion
            #endregion
        }


        catch (Exception er)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = er.Message.ToString();
            IQCareMsgBox.Show("#C1", theBuilder, this);

        }
    }

    void theFixedDrugName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string a;
    }

    private void AddControlsAttributes(Control theContainer)
    {
        foreach (Control x in theContainer.Controls)
        {
            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
            {
                foreach (Control y in x.Controls)
                {
                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                    {
                        AddControlsAttributes(y);
                    }
                    else
                    {
                        if (y.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                        {
                            ((TextBox)y).Attributes.Add("onkeyup", "chkNumeric('" + ((TextBox)y).ClientID + "')");
                        }
                    }
                }
            }
        }
    }
    private void BindControls()
    {

        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        BindFunctions theBindMgr = new BindFunctions();
        DataTable theDT = new DataTable();
        DataSet theDSXML = new DataSet();
        IQCareUtils theUtils = new IQCareUtils();
        theDSXML.ReadXml(Server.MapPath("..\\XMLFiles\\AllMasters.con"));

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            if (theDSXML.Tables["Mst_Employee"] != null)
            {
                theDV = new DataView(theDSXML.Tables["Mst_Employee"]);
                theDV.RowFilter = "DeleteFlag=0";
                //
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
                //
                //DataTable theDT = theDV.ToTable();
                //theBindMgr.BindCombo(ddlPharmOrderedbyName, theDT, "EmployeeName", "EmployeeId");
                //theBindMgr.BindCombo(ddlPharmReportedbyName, theDT, "EmployeeName", "EmployeeId");
                //theBindMgr.BindCombo(ddlPharmSignature, theDT, "EmployeeName", "EmployeeId");
            }

            ////IInitialEval IEManager;
            ////IEManager = (IInitialEval)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BInitialEval, BusinessProcess.Clinical");
            ////DataSet theDS = IEManager.GetAllDropDowns();

            ////DataView theDV1 = new DataView(theDS.Tables[6]);
            ////theDV1.RowFilter = "DeleteFlag=0";
            ////IQCareUtils theUtils = new IQCareUtils();
            ////DataTable theDT1 = (DataTable)theUtils.CreateTableFromDataView(theDV1);
            ////if (Application["AppEmployee"] != null)
            ////{
            ////    DataTable theMaster_1 = (DataTable)Application["AppEmployee"];
            ////    DataView theDVPharmOrdered = new DataView(theMaster_1);
            ////    theDVPharmOrdered.RowFilter = "DeleteFlag=0";
            ////    DataTable theDTPharmOrdered = theUtils.CreateTableFromDataView(theDVPharmOrdered);
            ////    theBindMgr.BindCombo(ddlPharmOrderedbyName, theDTPharmOrdered, "EmployeeName", "EmployeeId");
            ////    theDVPharmOrdered.Dispose();
            ////    theDTPharmOrdered.Clear();
            ////}
            //////theBindMgr.BindCombo(ddlPharmOrderedbyName, theDT1, "Name", "employeeid");
            ////if (Application["AppEmployee"] != null)
            ////{
            ////    DataTable theMaster_2 = (DataTable)Application["AppEmployee"];
            ////    DataView theDVPharmReported = new DataView(theMaster_2);
            ////    theDVPharmReported.RowFilter = "DeleteFlag=0";
            ////    DataTable theDTPharmReported = theUtils.CreateTableFromDataView(theDVPharmReported);
            ////    theBindMgr.BindCombo(ddlPharmReportedbyName, theDTPharmReported, "EmployeeName", "EmployeeId");
            ////    theDVPharmReported.Dispose();
            ////    theDTPharmReported.Clear();
            ////}
            //////theBindMgr.BindCombo(ddlPharmReportedbyName, theDT1, "Name", "employeeid");
            ////theBindMgr.BindCombo(ddlPharmSignature, theDT1, "Name", "employeeid");
            ////theDV1.Dispose();
            ////theDT1.Clear();
        }
        else
        {
            if (theDSXML.Tables["Mst_Employee"] != null)
            {
                theBindMgr.BindCombo(ddlPharmOrderedbyName, theDSXML.Tables["Mst_Employee"], "EmployeeName", "EmployeeId");
                theBindMgr.BindCombo(ddlPharmReportedbyName, theDSXML.Tables["Mst_Employee"], "EmployeeName", "EmployeeId");
                theBindMgr.BindCombo(ddlPharmSignature, theDSXML.Tables["Mst_Employee"], "EmployeeName", "EmployeeId");
            }

            ////if (Application["AppEmployee"] != null)
            ////{
            ////    DataTable theMaster_1 = (DataTable)Application["AppEmployee"];
            ////    DataTable theDTPharmOrdered = theMaster_1.Copy();
            ////    theBindMgr.BindCombo(ddlPharmOrderedbyName, theDTPharmOrdered, "EmployeeName", "EmployeeId");
            ////    theDTPharmOrdered.Clear();
            ////}
            ////if (Application["AppEmployee"] != null)
            ////{
            ////    DataTable theMaster_2 = (DataTable)Application["AppEmployee"];
            ////    DataTable theDTPharmReported = theMaster_2.Copy();
            ////    theBindMgr.BindCombo(ddlPharmReportedbyName, theDTPharmReported, "EmployeeName", "EmployeeId");
            ////    theDTPharmReported.Clear();
            ////}
            ////theBindMgr.BindCombo(ddlPharmSignature, DrugManager.GetEmployeeDetails(), "EmployeeName", "EmployeeID");
        }
    }
    private void BindddlControls(DataSet theDS)
    {
        /*******/
        BindFunctions BindManager = new BindFunctions();
        IQCareUtils theUtils = new IQCareUtils();
        DataView theDVTreat = new DataView(theDS.Tables[11]);
        theDVTreat.RowFilter = "DeleteFlag=0";
        DataTable theDT = (DataTable)theUtils.CreateTableFromDataView(theDVTreat);
        BindManager.BindCombo(ddlTreatment, theDT, theDT.Columns[1].ColumnName, theDT.Columns[0].ColumnName);
        //ddlTreatment.DataSource = theDT;
        //ddlTreatment.DataTextField = theDT.Columns[1].ColumnName;
        //ddlTreatment.DataValueField = theDT.Columns[0].ColumnName;
        //ddlTreatment.DataBind();
        //BindManager.BindCombo(ddlTreatment, theDT, "Name", "ID");
        theDVTreat.Dispose();
        theDT.Clear();

        //--------Rupesh 19-sep-07 - for ARV Provider
        DataView theDVProvider = new DataView(theDS.Tables[14]);
        //theDVProvider.RowFilter = "DeleteFlag=0";
        theDT = (DataTable)theUtils.CreateTableFromDataView(theDVProvider);
        BindManager.BindCombo(ddlProvider, theDT, theDT.Columns[1].ColumnName, theDT.Columns[0].ColumnName);
        //ddlProvider.DataSource = theDT;
        //ddlProvider.DataTextField = theDT.Columns[1].ColumnName;
        //ddlProvider.DataValueField = theDT.Columns[0].ColumnName;
        //ddlProvider.DataBind();
        theDVProvider.Dispose();
        theDT.Clear();
        //ddlProvider.SelectedIndex = 1;

        //Period Taken
        DataView DVPeriodTaken = new DataView(theDS.Tables[16]);
        DataTable dtPeriodTaken = (DataTable)theUtils.CreateTableFromDataView(DVPeriodTaken);
        BindManager.BindCombo(ddlPeriodTaken, dtPeriodTaken, dtPeriodTaken.Columns[1].ColumnName.ToString(), dtPeriodTaken.Columns[0].ColumnName.ToString());
        //ddlPeriodTaken.DataSource = dtPeriodTaken;
        //ddlPeriodTaken.DataTextField = dtPeriodTaken.Columns[1].ColumnName;
        //ddlPeriodTaken.DataValueField = dtPeriodTaken.Columns[0].ColumnName;
        //ddlPeriodTaken.DataBind();
        //ddlPeriodTaken.Dispose();
        //dtPeriodTaken.Clear();
        //ddlPeriodTaken.SelectedIndex = 0;
    }

    private DataTable CreateTable()
    {
        DataTable theDT = new DataTable();
        DataColumn theDrugName;
        DataColumn theStrength;
        DataColumn theFrequency;
        DataColumn theDose;
        DataColumn theDuration;
        DataColumn theDrgSchedule;
        DataColumn theQtyPrescribed;
        DataColumn theQtyDispensed;


        theDrugName = new DataColumn("DrugID");
        theDrugName.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(theDrugName);

        theStrength = new DataColumn("StrengthId");
        theStrength.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(theStrength);

        theFrequency = new DataColumn("Frequencyid");
        theFrequency.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(theFrequency);

        theDose = new DataColumn("SingleDose");
        theDose.DataType = System.Type.GetType("System.Decimal");
        theDT.Columns.Add(theDose);

        theDuration = new DataColumn("Duration");
        theDuration.DataType = System.Type.GetType("System.Decimal");
        theDT.Columns.Add(theDuration);

        theDrgSchedule = new DataColumn("DrugSchedule");
        theDrgSchedule.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(theDrgSchedule);

        theQtyPrescribed = new DataColumn("QtyPrescribed");
        theQtyPrescribed.DataType = System.Type.GetType("System.Decimal");
        theDT.Columns.Add(theQtyPrescribed);

        theQtyDispensed = new DataColumn("QtyDispensed");
        theQtyDispensed.DataType = System.Type.GetType("System.Decimal");
        theDT.Columns.Add(theQtyDispensed);

        DataColumn theFinanced = new DataColumn("Financed");
        theFinanced.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(theFinanced);

        DataColumn theDrgName = new DataColumn("DrugName");
        theDrgName.DataType = System.Type.GetType("System.String");
        theDT.Columns.Add(theDrgName);

        DataColumn theGenericName = new DataColumn("GenericId");
        theGenericName.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(theGenericName);

        DataColumn theqtyDose = new DataColumn("Dose");
        theqtyDose.DataType = System.Type.GetType("System.Decimal");
        theDT.Columns.Add(theqtyDose);

        DataColumn thUnitName = new DataColumn("UnitId");
        thUnitName.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(thUnitName);

        //27Feb08
        DataColumn thTotDailyDose = new DataColumn("TotDailyDose");
        thTotDailyDose.DataType = System.Type.GetType("System.Decimal");
        theDT.Columns.Add(thTotDailyDose);

        DataColumn thTBRegimen = new DataColumn("TBRegimenId");
        thTBRegimen.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(thTBRegimen);

        DataColumn theTreatmentPhase = new DataColumn("TreatmentPhase");
        theTreatmentPhase.DataType = System.Type.GetType("System.String");
        theDT.Columns.Add(theTreatmentPhase);

        DataColumn thTrMonth = new DataColumn("TrMonth");
        thTrMonth.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(thTrMonth);

        DataColumn thProphylaxis = new DataColumn("Prophylaxis");
        thProphylaxis.DataType = System.Type.GetType("System.Int32");
        theDT.Columns.Add(thProphylaxis);

        return theDT;

    }

    //private DataTable MakeInfantHealthSection()
    //{
    //    DataTable theDT = new DataTable();
    //    if (ViewState["Data"] == null)
    //    {
    //        theDT = CreateTable();
    //    }
    //    else
    //    {
    //        theDT = (DataTable)ViewState["Data"];
    //    }
    //    DataRow theDR;
    //    int DrugId = 0;
    //    int GenericId = 0;
    //    string [] DrugType;
    //    foreach (TableRow tr in tblInfantHealth.Rows)
    //    {
    //        foreach (TableCell tc in tr.Cells)
    //        {
    //            foreach (Control ct in tc.Controls)
    //            {

    //                if (ct.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputCheckBox))
    //                {
    //                    if (((HtmlInputCheckBox)ct).Checked == true)
    //                    {
    //                        DrugType = ((HtmlInputCheckBox)ct).ID.Split('-');
    //                        if (DrugType[1] == "0")
    //                        {
    //                            DrugId = Convert.ToInt32(DrugType[0]);
    //                            GenericId = 0;
    //                        }
    //                        else
    //                        {
    //                            GenericId = Convert.ToInt32(DrugType[0]);
    //                            DrugId = 0;
    //                        }
    //                    }
    //                }
    //                if (ct.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
    //                {
    //                    if (DrugId == Convert.ToInt32(((DropDownList)ct).ID.Substring(3, 3)))
    //                    {
    //                        theDR = theDT.NewRow();
    //                        theDR["DrugID"] = DrugId;
    //                        theDR["DrugSchedule"] = ((DropDownList)ct).SelectedValue;
    //                        theDR["GenericId"] = 0;
    //                        theDR["UnitId"] = 0;
    //                        theDR["Dose"] = 0.0;
    //                        theDR["StrengthId"] = 0;
    //                        theDR["SingleDose"] = 0;
    //                        theDR["FrequencyId"] = 0;
    //                        theDR["Duration"] = 0.0;
    //                        theDR["QtyPrescribed"] = 0.0;
    //                        theDR["QtyDispensed"] = 0.0;
    //                        theDR["Financed"] = 0;
    //                        theDR["Prophylaxis"] = 0;
    //                        theDT.Rows.Add(theDR);
    //                    }
    //                    else if (GenericId == Convert.ToInt32(((DropDownList)ct).ID.Substring(3, 3)))
    //                    {
    //                        theDR = theDT.NewRow();
    //                        theDR["DrugID"] = 0;
    //                        theDR["DrugSchedule"] = ((DropDownList)ct).SelectedValue;
    //                        theDR["GenericId"] = GenericId;
    //                        theDR["UnitId"] = 0;
    //                        theDR["Dose"] = 0.0;
    //                        theDR["StrengthId"] = 0;
    //                        theDR["SingleDose"] = 0;
    //                        theDR["FrequencyId"] = 0;
    //                        theDR["Duration"] = 0.0;
    //                        theDR["QtyPrescribed"] = 0.0;
    //                        theDR["QtyDispensed"] = 0.0;
    //                        theDR["Financed"] = 0;
    //                        theDR["Prophylaxis"] = 0;
    //                        theDT.Rows.Add(theDR);
    //                    }


    //                }
    //            }
    //        }
    //    }
    //    return theDT;
    //}
    //private DataTable MakeDrugTable(Control theContainer)
    //{
    //    int c = 0; // total length of ID
    //    DataTable theDT = new DataTable();

    //    if (ViewState["Data"] == null)
    //    {
    //        theDT = CreateTable();
    //    }
    //    else
    //    {
    //        theDT = (DataTable)ViewState["Data"];
    //    }

    //    #region "Variables"
    //    decimal Dose = 0;
    //    int UnitId = 0;
    //    int theStrengthId = 0;
    //    int theDrugId = 0;
    //    decimal theQtyDose = 0;
    //    int theFrequencyId = 0;
    //    decimal theDuration = 0;
    //    decimal theQtyPrescribed = 0;
    //    decimal theQtyDispensed = 0;
    //    decimal theQtyPrescribed1 = 0;
    //    decimal theQtyDispensed1 = 0;
    //    string theTreatmentPhase = "";
    //    int theMonth = 0;
    //    int theFinanced = 99;
    //    int theProphylaxis = 0;
    //    if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
    //    {
    //        theProphylaxis = 999;
    //    }
    //    decimal theTotDailyDose = 0;
    //    #endregion
    //    //Regimen Line Enhancement
    //    if (theContainer.ID == "PnlRegline" || theContainer.ID == "PnlRegiment")
    //    {
    //        DataTable theARVDrug = (DataTable)ViewState["SelectedDrug"];
    //        foreach (DataRow theDR in theARVDrug.Rows)
    //        {
    //            foreach (Control y in theContainer.Controls)
    //            {
    //                if (y.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
    //                {
    //                    //if (Convert.ToInt32(theDR["GenericId"]) > 0)
    //                    //{
    //                        if (y.ID == "DDRegimenLine")
    //                        {
    //                            ViewState["RegimenLine"] = Convert.ToInt32(((DropDownList)y).SelectedValue);
    //                        }
    //                    //}
    //                }
    //            }
    //        }
    //    }
    //    //if (theContainer.ID == "pnlPedia" || theContainer.ID == "PnlOIARV" || theContainer.ID == "PnlRegiment")
    //    if (theContainer.ID == "PnlOIARV")
    //    {
    //        #region "ARV and OI"
    //        if (Session["OIDrugs"] != null)
    //        {
    //            int TotColFilled = 0; // rupesh
    //            DataTable theARVDrug = (DataTable)Session["OIDrugs"];
    //            foreach (DataRow theDR in theARVDrug.Rows)
    //            {
    //                DataRow theRow;
    //                string R = theDR[2].ToString();
    //                foreach (Control y in theContainer.Controls)
    //                {
    //                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                    {
    //                        foreach (Control x in y.Controls)
    //                        {
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                            {
    //                                MakeDrugTable(x);
    //                            }
    //                            else
    //                            {
    //                                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
    //                                {
    //                                    if (Convert.ToInt32(theDR["GenericId"]) > 0)
    //                                    {
    //                                        //if (x.ID == "DDRegimenLine")
    //                                        //{
    //                                        //    ViewState["RegimenLine"] = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                        //}

    //                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgStrength"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgStrength" + theDR["GenericId"].ToString())
    //                                            {
    //                                                theStrengthId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                                if (theStrengthId != 0)
    //                                                    TotColFilled++;
    //                                            }

    //                                        }
    //                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgFrequency"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgFrequency" + theDR["GenericId"].ToString())
    //                                            {
    //                                                theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                                if (theFrequencyId != 0)
    //                                                    TotColFilled++;
    //                                            }

    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        //if (x.ID == "DDRegimenLine")
    //                                        //{
    //                                        //    ViewState["RegimenLine"] = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                        //}

    //                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgStrength"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgStrength" + theDR["DrugId"].ToString())
    //                                            {
    //                                                if ((((DropDownList)x).SelectedValue != null) && (((DropDownList)x).SelectedValue != ""))
    //                                                    theStrengthId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                                #region "18-Jun-07 - 4"
    //                                                if (theStrengthId != 0)
    //                                                    TotColFilled++;
    //                                                #endregion
    //                                            }
    //                                        }
    //                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgFrequency"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgFrequency" + theDR["DrugId"].ToString())
    //                                            {
    //                                                if ((((DropDownList)x).SelectedValue != null) && (((DropDownList)x).SelectedValue != ""))
    //                                                    theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                                #region "18-Jun-07 - 5"
    //                                                if (theFrequencyId != 0)
    //                                                    TotColFilled++;
    //                                                #endregion
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
    //                                {
    //                                    if (Convert.ToInt32(theDR["GenericId"]) > 0)
    //                                    {
    //                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgDose"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgDose" + theDR["GenericId"].ToString())
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theQtyDose = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    #region "18-Jun-07 - 6"
    //                                                    if (theQtyDose != 0)
    //                                                        TotColFilled++;
    //                                                    #endregion
    //                                                }
    //                                            }
    //                                        }
    //                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgDuration"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgDuration" + theDR["GenericId"].ToString())
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theDuration = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    #region "18-Jun-07 - 7"
    //                                                    if (theDuration != 0)
    //                                                        TotColFilled++;
    //                                                    #endregion
    //                                                }
    //                                            }
    //                                        }
    //                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgQtyPrescribed" + theDR["GenericId"].ToString())
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theQtyPrescribed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    #region "18-Jun-07 - 8"
    //                                                    if (theQtyPrescribed != 0)
    //                                                        TotColFilled++;
    //                                                    #endregion
    //                                                }
    //                                            }
    //                                        }
    //                                        if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgQtyDispensed" + theDR["GenericId"].ToString())
    //                                            {
    //                                                if (Session["Paperless"].ToString() == "1")
    //                                                {
    //                                                    if (TotColFilled > 3)
    //                                                    {
    //                                                        if (((TextBox)x).Text == "")
    //                                                        {
    //                                                            theQtyDispensed = 99;
    //                                                            #region "18-Jun-07 - 8"
    //                                                            if (theQtyDispensed != 0)
    //                                                                TotColFilled++;
    //                                                            #endregion
    //                                                        }
    //                                                        else
    //                                                        {
    //                                                            if (((TextBox)x).Text != "")
    //                                                            {
    //                                                                theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                                #region "18-Jun-07 - 8"
    //                                                                if (theQtyDispensed != 0)
    //                                                                    TotColFilled++;
    //                                                                #endregion
    //                                                            }
    //                                                        }
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    if (((TextBox)x).Text != "")
    //                                                    {
    //                                                        theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                        #region "18-Jun-07 - 8"
    //                                                        if (theQtyDispensed != 0)
    //                                                            TotColFilled++;
    //                                                        #endregion
    //                                                    }
    //                                                }
    //                                                //if (((TextBox)x).Text != "")
    //                                                //{
    //                                                //    theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                //    #region "18-Jun-07 - 9"
    //                                                //    if (theQtyDispensed != 0)
    //                                                //        TotColFilled++;
    //                                                //    #endregion
    //                                                //}
    //                                            }
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgDose"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgDose" + theDR["DrugId"].ToString())
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theQtyDose = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    #region "18-Jun-07 - 10"
    //                                                    if (theQtyDose != 0)
    //                                                        TotColFilled++;
    //                                                    #endregion
    //                                                }
    //                                            }
    //                                        }
    //                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgDuration"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgDuration" + theDR["DrugId"].ToString())
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theDuration = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    #region "18-Jun-07 - 11"
    //                                                    if (theDuration != 0)
    //                                                        TotColFilled++;
    //                                                    #endregion

    //                                                }
    //                                            }
    //                                        }
    //                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgQtyPrescribed" + theDR["DrugId"].ToString())
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theQtyPrescribed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    #region "18-Jun-07 - 12"
    //                                                    if (theQtyPrescribed != 0)
    //                                                        TotColFilled++;
    //                                                    #endregion
    //                                                }
    //                                            }
    //                                        }
    //                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
    //                                        {
    //                                            if (x.ID.ToString() == "drgQtyDispensed" + theDR["DrugId"].ToString())
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    if (theQtyDispensed != 0)
    //                                                        TotColFilled++;
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                //Prophylaxis
    //                                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
    //                                {
    //                                    if (Convert.ToInt32(theDR["GenericId"]) > 0)
    //                                    {
    //                                        if (x.ID.StartsWith("chkProphylaxis"))
    //                                        {
    //                                            c = x.ID.Length;
    //                                            if (x.ID.EndsWith(theDR["GenericId"].ToString()) && x.ID.StartsWith("chkProphylaxis"))
    //                                            {

    //                                                if (((CheckBox)x).Checked == true)
    //                                                {
    //                                                    theProphylaxis = 1;
    //                                                    TotColFilled++;
    //                                                }

    //                                            }
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        //Prophylaxis

    //                                        if (x.ID.StartsWith("chkProphylaxis"))
    //                                        {
    //                                            c = x.ID.Length;
    //                                            if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("chkProphylaxis"))
    //                                            {

    //                                                if (((CheckBox)x).Checked == true)
    //                                                {
    //                                                    theProphylaxis = 1;
    //                                                    TotColFilled++;
    //                                                }

    //                                            }
    //                                        }

    //                                    }
    //                                }
    //                            }
    //                            if (theContainer.ID == "pnlPedia")
    //                            {

    //                                if (theStrengthId != 0 && theQtyDose != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theProphylaxis != 999)
    //                                {
    //                                    theRow = theDT.NewRow();
    //                                    theRow["DrugId"] = 0;
    //                                    theRow["GenericId"] = theDR["GenericId"];
    //                                    theRow["Dose"] = 0;
    //                                    theRow["SingleDose"] = theQtyDose;
    //                                    theRow["UnitId"] = 0;
    //                                    theRow["StrengthId"] = theStrengthId;
    //                                    theRow["FrequencyId"] = theFrequencyId;
    //                                    theRow["Duration"] = theDuration;
    //                                    theRow["QtyPrescribed"] = theQtyPrescribed;
    //                                    if (Session["Paperless"].ToString() == "1")
    //                                    {
    //                                        if (theQtyDispensed == 99)
    //                                        {
    //                                            theRow["QtyDispensed"] = 0;
    //                                        }
    //                                        else
    //                                        {
    //                                            theRow["QtyDispensed"] = theQtyDispensed;
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        theRow["QtyDispensed"] = theQtyDispensed;
    //                                    }
    //                                    theRow["Financed"] = 1;
    //                                    theRow["Prophylaxis"] = theProphylaxis;
    //                                    theDT.Rows.Add(theRow);
    //                                    #region "Reset Variables
    //                                    Dose = 0;
    //                                    UnitId = 0;
    //                                    theStrengthId = 0;
    //                                    theFrequencyId = 0;
    //                                    theQtyDose = 0;
    //                                    theDuration = 0;
    //                                    theQtyPrescribed = 0;
    //                                    theQtyDispensed = 0;
    //                                    theFinanced = 99;
    //                                    TotColFilled = 0;
    //                                    if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
    //                                    {
    //                                        theProphylaxis = 999;
    //                                    }
    //                                    else
    //                                    {
    //                                        theProphylaxis = 0;
    //                                    }
    //                                    #endregion
    //                                }

    //                            }
    //                        }
    //                        if (Session["Paperless"].ToString() != "1")
    //                        {
    //                            if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
    //                            {
    //                                if ((TotColFilled > 0 && TotColFilled < 7) && (theContainer.ID == "pnlPedia"))
    //                                {
    //                                    theDT.Rows.Clear();
    //                                    theRow = theDT.NewRow();
    //                                    theRow[0] = 99999;
    //                                    theDT.Rows.Add(theRow);
    //                                    return theDT;
    //                                }
    //                                else
    //                                    TotColFilled = 0;
    //                            }
    //                            else
    //                            {
    //                                if ((TotColFilled > 0 && TotColFilled < 6) && (theContainer.ID == "pnlPedia"))
    //                                {
    //                                    theDT.Rows.Clear();
    //                                    theRow = theDT.NewRow();
    //                                    theRow[0] = 99999;
    //                                    theDT.Rows.Add(theRow);
    //                                    return theDT;
    //                                }
    //                                else
    //                                    TotColFilled = 0;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
    //                            {
    //                                if ((TotColFilled > 0 && TotColFilled < 7) && (theContainer.ID == "pnlPedia"))
    //                                {
    //                                    theDT.Rows.Clear();
    //                                    theRow = theDT.NewRow();
    //                                    theRow[0] = 99999;
    //                                    theDT.Rows.Add(theRow);
    //                                    return theDT;
    //                                }
    //                                else
    //                                    TotColFilled = 0;
    //                            }
    //                        }

    //                    }
    //                }
    //                //if (theContainer.ID == "PnlOIARV")
    //                //{
    //                //    if (theStrengthId != 0 || theQtyDose != 0 || theFrequencyId != 0 || theDuration != 0 || theQtyPrescribed != 0 || theQtyDispensed != 0)
    //                //    {
    //                //        theRow = theDT.NewRow();
    //                //        theRow["DrugId"] = theDR["Drugid"];
    //                //        theRow["GenericId"] = theDR["GenericId"];
    //                //        theRow["SingleDose"] = theQtyDose;
    //                //        theRow["Dose"] = 0;
    //                //        theRow["UnitId"] = theStrengthId;
    //                //        theRow["StrengthId"] = theStrengthId;
    //                //        theRow["FrequencyId"] = theFrequencyId;
    //                //        theRow["Duration"] = theDuration;
    //                //        theRow["QtyPrescribed"] = theQtyPrescribed;
    //                //        if (Session["Paperless"].ToString() == "1")
    //                //        {
    //                //            if (theQtyDispensed == 99)
    //                //            {
    //                //                theRow["QtyDispensed"] = 0;
    //                //            }
    //                //            else
    //                //            {
    //                //                theRow["QtyDispensed"] = theQtyDispensed;
    //                //            }
    //                //        }
    //                //        else
    //                //        {
    //                //            theRow["QtyDispensed"] = theQtyDispensed;
    //                //        }
    //                //        theRow["Prophylaxis"] = theProphylaxis;
    //                //        theRow["Financed"] = 1;
    //                //        theDT.Rows.Add(theRow);
    //                //        #region "Reset Variables
    //                //        Dose = 0;
    //                //        UnitId = 0;
    //                //        theStrengthId = 0;
    //                //        theFrequencyId = 0;
    //                //        theDuration = 0;
    //                //        theQtyDose = 0;
    //                //        theQtyPrescribed = 0;
    //                //        theQtyDispensed = 0;
    //                //        theFinanced = 99;
    //                //        if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
    //                //        {
    //                //            theProphylaxis = 999;
    //                //        }
    //                //        else
    //                //        {
    //                //            theProphylaxis = 0;
    //                //        }
    //                //        #endregion
    //                //    }
    //                //}
    //            }
    //        }
    //        #endregion
    //    }

    //    //else if (theContainer.ID == "PnlAdARV")
    //    else if (theContainer.ID == "pnlPedia")
    //    {
    //        #region "Additional ARV"
    //        if (Session["AddARV"] != null)
    //        {
    //            int TotelColFilled = 0;
    //            DataTable theADDARVDrug = (DataTable)Session["AddARV"];
    //            if (theADDARVDrug == null)
    //                return theDT;
    //            foreach (DataRow theDR in theADDARVDrug.Rows)
    //            {
    //                DataRow theRow;
    //                foreach (Control y in theContainer.Controls)
    //                {
    //                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                    {
    //                        foreach (Control x in y.Controls)
    //                        {
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                            {
    //                                MakeDrugTable(x);
    //                            }
    //                            else
    //                            {
    //                                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
    //                                {
    //                                    //(x.ID.Substring(15, c - 15) ==
    //                                    //if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgStrength"))

    //                                    if (x.ID.StartsWith("drgStrength"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.Substring(11, c - 11) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgStrength"))
    //                                        {
    //                                            theStrengthId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                            TotelColFilled++;
    //                                        }
    //                                    }
    //                                    if (x.ID.StartsWith("drgFrequency"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.Substring(12, c - 12) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgFrequency"))
    //                                        {
    //                                            theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                            TotelColFilled++;
    //                                        }
    //                                    }
    //                                }
    //                                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
    //                                {
    //                                    if (x.ID.StartsWith("drgDose"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.Substring(7, c - 7) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgDose"))
    //                                        {
    //                                            if (((TextBox)x).Text != "")
    //                                            {
    //                                                theQtyDose = Convert.ToDecimal(((TextBox)x).Text);
    //                                                TotelColFilled++;
    //                                            }
    //                                        }
    //                                    }
    //                                    if (x.ID.StartsWith("drgDuration"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.Substring(11, c - 11) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgDuration"))
    //                                        {
    //                                            if (((TextBox)x).Text != "")
    //                                            {
    //                                                theDuration = Convert.ToDecimal(((TextBox)x).Text);
    //                                                TotelColFilled++;
    //                                            }
    //                                        }
    //                                    }
    //                                    if (x.ID.StartsWith("drgQtyPrescribed"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.Substring(16, c - 16) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgQtyPrescribed"))
    //                                        {
    //                                            if (((TextBox)x).Text != "")
    //                                            {
    //                                                theQtyPrescribed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                TotelColFilled++;
    //                                            }
    //                                        }
    //                                    }
    //                                    if (x.ID.StartsWith("drgQtyDispensed"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.Substring(15, c - 15) == theDR["DrugId"].ToString() && x.ID.StartsWith("drgQtyDispensed"))
    //                                        {
    //                                            //for paperless
    //                                            if (Session["Paperless"].ToString() == "1")
    //                                            {

    //                                                if (((TextBox)x).Text == "")
    //                                                {
    //                                                    theQtyDispensed = 99;
    //                                                    TotelColFilled++;

    //                                                }
    //                                                else
    //                                                {
    //                                                    if (((TextBox)x).Text != "")
    //                                                    {
    //                                                        theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                        TotelColFilled++;
    //                                                    }
    //                                                }

    //                                            }
    //                                            else
    //                                            {
    //                                                if (((TextBox)x).Text != "")
    //                                                {
    //                                                    theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
    //                                                    TotelColFilled++;
    //                                                }
    //                                            }

    //                                        }
    //                                    }
    //                                }
    //                                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
    //                                {
    //                                    if (x.ID.StartsWith("FinChk"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.Substring(6, c - 6) == theDR["DrugId"].ToString() && x.ID.StartsWith("FinChk"))
    //                                        {
    //                                            if (((CheckBox)x).Checked == true)
    //                                                theFinanced = 1;
    //                                            else
    //                                                theFinanced = 0;
    //                                        }
    //                                        TotelColFilled++;
    //                                    }
    //                                    if (x.ID.StartsWith("chkProphylaxis"))
    //                                    {
    //                                        c = x.ID.Length;
    //                                        if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("chkProphylaxis"))
    //                                        {

    //                                            if (((CheckBox)x).Checked == true)
    //                                            {
    //                                                theProphylaxis = 1;
    //                                                TotelColFilled++;
    //                                            }

    //                                        }
    //                                    }
    //                                }
    //                            }
    //                            if (theStrengthId != 0 && theQtyDose != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theProphylaxis != 999)
    //                            {
    //                                theRow = theDT.NewRow();
    //                                if (Convert.ToInt32(theDR["Generic"]) == 0)
    //                                {
    //                                    theRow["DrugId"] = theDR["DrugId"];
    //                                    theRow["GenericId"] = 0;
    //                                }
    //                                else
    //                                {
    //                                    theRow["DrugId"] = 0;
    //                                    theRow["GenericId"] = theDR["DrugId"];
    //                                }
    //                                theRow["SingleDose"] = theQtyDose;
    //                                theRow["Dose"] = 0;
    //                                theRow["UnitId"] = 0;
    //                                theRow["StrengthId"] = theStrengthId;
    //                                theRow["FrequencyId"] = theFrequencyId;
    //                                theRow["Duration"] = theDuration;
    //                                theRow["QtyPrescribed"] = theQtyPrescribed;
    //                                theRow["Prophylaxis"] = theProphylaxis;
    //                                if (Session["Paperless"].ToString() == "1")
    //                                {
    //                                    if (theQtyDispensed == 99)
    //                                    {
    //                                        theRow["QtyDispensed"] = 0;
    //                                    }
    //                                    else
    //                                    {
    //                                        theRow["QtyDispensed"] = theQtyDispensed;
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    theRow["QtyDispensed"] = theQtyDispensed;
    //                                }
    //                                theRow["Financed"] = 1;
    //                                theDT.Rows.Add(theRow);
    //                                #region "Reset Variables
    //                                Dose = 0;
    //                                UnitId = 0;
    //                                theStrengthId = 0;
    //                                theFrequencyId = 0;
    //                                theDuration = 0;
    //                                theQtyDose = 0;
    //                                theQtyPrescribed = 0;
    //                                theQtyDispensed = 0;
    //                                theFinanced = 99;
    //                                if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
    //                                {
    //                                    theProphylaxis = 999;
    //                                }
    //                                else
    //                                {
    //                                    theProphylaxis = 0;
    //                                }
    //                                #endregion
    //                            }
    //                        }
    //                    }
    //                }
    //                if (Session["Paperless"].ToString() != "1")
    //                {
    //                    if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224")
    //                    {
    //                        //if ((TotelColFilled > 0 && TotelColFilled < 7) && (theContainer.ID == "PnlAdARV"))
    //                        if ((TotelColFilled > 0 && TotelColFilled < 7) && (theContainer.ID == "pnlPedia"))
    //                        {
    //                            theDT.Rows.Clear();
    //                            theRow = theDT.NewRow();
    //                            theRow[0] = 99999;
    //                            theDT.Rows.Add(theRow);
    //                            return theDT;
    //                        }
    //                        else
    //                            TotelColFilled = 0;
    //                    }
    //                    else
    //                    {
    //                        //if ((TotelColFilled > 0 && TotelColFilled < 6) && (theContainer.ID == "PnlAdARV"))
    //                        if ((TotelColFilled > 0 && TotelColFilled < 6) && (theContainer.ID == "pnlPedia"))
    //                        {
    //                            theDT.Rows.Clear();
    //                            theRow = theDT.NewRow();
    //                            theRow[0] = 99999;
    //                            theDT.Rows.Add(theRow);
    //                            return theDT;
    //                        }
    //                        else
    //                            TotelColFilled = 0;
    //                    }
    //                }

    //            }
    //        }
    //        #endregion
    //    }
    //    else if (theContainer.ID == "pnlOtherTBMedicaton")
    //    {
    //        #region "TB Drug"
    //        //pnl4 - id = PnlOtherMedication btn-txt = "OI Treatment & Other Medications"
    //        DataTable theADDTBDrug = (DataTable)Session["AddTB"];
    //        int DrugID = 0;
    //        if (theADDTBDrug == null)
    //            return theDT;
    //        foreach (DataRow theDR in theADDTBDrug.Rows)
    //        {
    //            DataRow theRow;
    //            foreach (Control y in theContainer.Controls)
    //            {
    //                if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                {
    //                    foreach (Control x in y.Controls)
    //                    {
    //                        if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                        {
    //                            MakeDrugTable(x);
    //                        }
    //                        else
    //                        {
    //                            if (theDR["DrugId"].ToString().LastIndexOf("8888") > 0) //--- if '8888' is added at the end of id - drug
    //                            {
    //                                DrugID = Convert.ToInt32(theDR["DrugId"].ToString().Substring(0, theDR["DrugId"].ToString().Length - 4));
    //                            }
    //                            else if (theDR["DrugId"].ToString().LastIndexOf("9999") > 0) //--- if '9999' is added at the end of id  - generic
    //                            {

    //                                DrugID = Convert.ToInt32(theDR["DrugId"].ToString().Substring(0, theDR["DrugId"].ToString().Length - 4));
    //                            }
    //                            else
    //                            {
    //                                DrugID = Convert.ToInt32(theDR["DrugId"]);
    //                            }
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
    //                            {
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("theUnit"))
    //                                {
    //                                    UnitId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                }
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgFrequency"))
    //                                {
    //                                    theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                }
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgTreatmenPhase"))
    //                                {
    //                                    theTreatmentPhase = Convert.ToString(((DropDownList)x).SelectedValue);
    //                                }
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgTreatmenMonth"))
    //                                {
    //                                    theMonth = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                }
    //                            }
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
    //                            {
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("theDose"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        Dose = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }

    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgDuration"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        theDuration = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        theQtyPrescribed1 = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        theQtyDispensed1 = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }
    //                            }
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
    //                            {
    //                                if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("FinChk"))
    //                                {
    //                                    if (((CheckBox)x).Checked == true)
    //                                        theFinanced = 1;
    //                                    else
    //                                        theFinanced = 0;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }

    //            if (UnitId != 0 || theFrequencyId != 0 || Dose != 0 || theDuration != 0 || theQtyPrescribed != 0 || theQtyDispensed != 0 || theTreatmentPhase != "0" || theMonth!=0 || theFinanced != 99)
    //            {
    //                theRow = theDT.NewRow();
    //                if (Convert.ToInt32(theDR["Generic"]) == 0)
    //                {
    //                    theRow["DrugId"] = DrugID;
    //                    theRow["GenericId"] = 0;
    //                }
    //                else
    //                {
    //                    theRow["DrugId"] = 0;
    //                    theRow["GenericId"] = DrugID;
    //                }
    //                theRow["Dose"] = Dose;
    //                theRow["UnitId"] = UnitId;
    //                theRow["StrengthId"] = 0;
    //                theRow["FrequencyId"] = theFrequencyId;
    //                theRow["Duration"] = theDuration;
    //                theRow["QtyPrescribed"] = theQtyPrescribed1;
    //                //theRow["QtyDispensed"] = theQtyDispensed;
    //                theRow["SingleDose"] = 0;
    //                theRow["QtyDispensed"] = theQtyDispensed1;
    //                theRow["Financed"] = theFinanced;
    //                theRow["TreatmentPhase"] = theTreatmentPhase;
    //                theRow["TrMonth"] = theMonth;
    //                theDT.Rows.Add(theRow);
    //                #region "Reset Variables
    //                Dose = 0;
    //                UnitId = 0;
    //                theStrengthId = 0;
    //                theFrequencyId = 0;
    //                theDuration = 0;
    //                theQtyPrescribed = 0;
    //                theQtyDispensed = 0;
    //                theFinanced = 99;
    //                #endregion
    //            }

    //        }
    //        #endregion
    //    }
    //    else if(theContainer.ID=="PnlOtMed")
    //    {
    //        #region "Additional Drugs"
    //        DataTable theOtherDrug = (DataTable)Session["OtherDrugs"];
    //        if (theOtherDrug == null)
    //            return theDT;
    //        foreach (DataRow theDR in theOtherDrug.Rows)
    //        {
    //            DataRow theRow;
    //            foreach (Control y in theContainer.Controls)
    //            {
    //                if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                {
    //                    foreach (Control x in y.Controls)
    //                    {
    //                        if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
    //                        {
    //                            MakeDrugTable(x);
    //                        }
    //                        else
    //                        {
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
    //                            {
    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("theUnit"))
    //                                {
    //                                    UnitId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                }
    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgFrequency"))
    //                                {
    //                                    theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
    //                                }




    //                            }
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
    //                            {
    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgDose"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        Dose = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }

    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgDuration"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        theDuration = Convert.ToInt32(((TextBox)x).Text);
    //                                    }
    //                                }
    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        theQtyPrescribed1 = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }
    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        theQtyDispensed1 = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }
    //                                //24Feb08
    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgTotalDose"))
    //                                {
    //                                    if (((TextBox)x).Text != "")
    //                                    {
    //                                        theTotDailyDose = Convert.ToDecimal(((TextBox)x).Text);
    //                                    }
    //                                }


    //                            }
    //                            if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
    //                            {
    //                                if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("FinChk"))
    //                                {
    //                                    if (((CheckBox)x).Checked == true)
    //                                        theFinanced = 1;
    //                                    else
    //                                        theFinanced = 0;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            if (UnitId != 0 || theFrequencyId != 0 || Dose != 0 || theDuration != 0 || theQtyPrescribed != 0 || theQtyDispensed != 0 || theTotDailyDose != 0 || theFinanced != 99)
    //            {
    //                theRow = theDT.NewRow();
    //                if (Convert.ToInt32(theDR["Generic"]) == 0)
    //                {
    //                    theRow["DrugId"] = theDR["DrugId"];
    //                    theRow["GenericId"] = 0;
    //                }
    //                else
    //                {
    //                    theRow["DrugId"] = 0;
    //                    theRow["GenericId"] = theDR["DrugId"];
    //                }
    //                theRow["SingleDose"] = Dose;
    //                theRow["Dose"] = 0;
    //                theRow["UnitId"] = UnitId;
    //                theRow["StrengthId"] = 0;
    //                theRow["FrequencyId"] = theFrequencyId;
    //                theRow["Duration"] = theDuration;
    //                theRow["QtyPrescribed"] = theQtyPrescribed1;
    //                theRow["QtyDispensed"] = theQtyDispensed1;
    //                theRow["Financed"] = theFinanced;
    //                theRow["TotDailyDose"] = theTotDailyDose;//27Feb08
    //                theDT.Rows.Add(theRow);
    //                #region "Reset Variables
    //                Dose = 0;
    //                UnitId = 0;
    //                theStrengthId = 0;
    //                theFrequencyId = 0;
    //                theDuration = 0;
    //                theQtyPrescribed = 0;
    //                theQtyDispensed = 0;
    //                theTotDailyDose = 0;//27Feb08
    //                theFinanced = 99;
    //                #endregion
    //            }
    //        }
    //        #endregion
    //    }
    //    return theDT;
    //}
    private DataTable MakeDrugTable(Control theContainer)
    {
        int c = 0;//c=total length of id
        DataTable theDT = new DataTable();

        if (ViewState["Data"] == null)
        {
            theDT = CreateTable();
        }
        else
        {
            theDT = (DataTable)ViewState["Data"];
        }

        #region "Variables"
        decimal Dose = 0;
        int UnitId = 0;
        int theStrengthId = 0;
        int theFixedDrugId = 0;
        int theFrequencyId = 0;
        Decimal theDuration = 0;
        Decimal theQtyPrescribed = 0;
        Decimal theQtyDispensed = 0;
        Decimal theQtyPrescribed1 = 0;
        Decimal theQtyDispensed1 = 0;
        string theTreatmentPhase = "";
        int theMonth = 0;
        int theProphylaxis = 0;
        if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224" || ddlTreatment.SelectedItem.Value.ToString() == "225")
        {
            theProphylaxis = 999;
        }
        int theFinanced = 99;

        #endregion

        //pnl 1 - id=PnlDrug - no btn , pnl 2 -id= PnlAddARV  btn-txt = "Other ARV Medications", 
        //pnl 3 - id =PnlOIARV - no btn , pnl4 - id = PnlOtherMedication btn-txt = "OI Treatment & Other Medications"
        if (theContainer.ID == "PnlRegiment")
        {
            foreach (Control x in theContainer.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {

                    if (x.ID == "DDRegimenLine")
                    {

                        ViewState["RegimenLine"] = Convert.ToInt32(((DropDownList)x).SelectedValue);

                    }
                }
            }
            #region "18-Jun-07 - 12"
            if (ddlTreatment.SelectedValue == "222")
            {

                if (Session["AddARV"] != null)
                {
                    if (Convert.ToInt32(ViewState["RegimenLine"]) == 0 && (((DataTable)Session["AddARV"]).Rows.Count > 0))
                    {

                        theDT.Rows.Clear();
                        DataRow theRow = theDT.NewRow();
                        theRow[0] = 99999;
                        theDT.Rows.Add(theRow);
                        return theDT;
                    }
                }

            }


            #endregion
        }
        else if (theContainer.ID == "PnlOIARV") //--ARV
        {
            #region "ARV and OI"
            //pnl 1 - id=PnlDrug - no btn  AND pnl 3 - id =PnlOIARV - no btn
            //DataTable theARVDrug = (DataTable)Session["SelectedDrug"];
            if (Session["OIDrugs"] != null)
            {
                DataTable theARVDrug = (DataTable)Session["OIDrugs"];
                #region "18-Jun-07 - 1"
                int TotColFilled = 0; // rupesh
                #endregion
                foreach (DataRow theDR in theARVDrug.Rows)
                {
                    Dose = 0;
                    UnitId = 0;
                    theStrengthId = 0;
                    theFixedDrugId = 0;
                    theFrequencyId = 0;
                    theDuration = 0;
                    theQtyPrescribed = 0;
                    theQtyDispensed = 0;
                    theQtyPrescribed1 = 0;
                    theQtyDispensed1 = 0;
                    theTreatmentPhase = "";
                    DataRow theRow;
                    DataRow[] DRStrength = ((DataSet)ViewState["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
                    if (DRStrength[0]["StrengthId"] != System.DBNull.Value)
                        theStrengthId = Convert.ToInt32(DRStrength[0]["StrengthId"]);
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

                                        if (x.ID != null)
                                        {
                                            if (x.ID == "DDRegimenLine")
                                            {
                                                ViewState["RegimenLine"] = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            }

                                            if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^0") && x.ID.StartsWith("drgFrequency"))
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
                                        if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["GenericId"].ToString()) && x.ID.StartsWith("drgDuration"))
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
                                        if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["GenericId"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
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

                                        if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["GenericId"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                        {
                                           
                                            if (Session["Paperless"].ToString() == "1")
                                            {
                                                if (TotColFilled > 3)
                                                {
                                                    if (((TextBox)x).Text == "" || ((TextBox)x).Text == "0")
                                                    {
                                                        theQtyDispensed = 99;
                                                        #region "18-Jun-07 - 8"
                                                        if (theQtyDispensed != 0)
                                                            TotColFilled++;
                                                        #endregion
                                                    }
                                                    else
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
                                            }
                                            else
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


                                        //}
                                    }
                                    //Prophylaxis
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                    {
                                        //Prophylaxis
                                        if (x.ID.StartsWith("chkProphylaxis"))
                                        {
                                            c = x.ID.Length;
                                            if (x.ID.EndsWith(theDR["DrugId"].ToString() + "^" + theDR["GenericId"].ToString()) && x.ID.StartsWith("chkProphylaxis"))
                                            {

                                                if (((CheckBox)x).Checked == true)
                                                {
                                                    theProphylaxis = 1;
                                                    TotColFilled++;
                                                }

                                            }
                                        }

                                        //}
                                    }
                                }

                                //if (theContainer.ID == "PnlOIARV")
                                //{
                                //paper less
                                if (Session["Paperless"].ToString() == "1")
                                {
                                    if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theProphylaxis != 999)
                                    {

                                        theRow = theDT.NewRow();
                                        theRow["DrugId"] = theDR["DrugId"];
                                        theRow["GenericId"] = 0;

                                        theRow["Dose"] = 0;
                                        theRow["UnitId"] = 0;
                                        theRow["StrengthId"] = theStrengthId;
                                        theRow["FrequencyId"] = theFrequencyId;
                                        theRow["Duration"] = theDuration;
                                        theRow["QtyPrescribed"] = theQtyPrescribed;
                                        if (theQtyDispensed == 99)
                                        {
                                            theRow["QtyDispensed"] = 0;
                                        }
                                        else
                                        {
                                            theRow["QtyDispensed"] = theQtyDispensed;
                                        }

                                        theRow["Financed"] = 1;
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
                                        if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224" || ddlTreatment.SelectedItem.Value.ToString() == "225")
                                        {
                                            theProphylaxis = 999;
                                        }
                                        else
                                        {
                                            theProphylaxis = 0;
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theProphylaxis != 999)
                                    {
                                        theRow = theDT.NewRow();

                                        theRow["DrugId"] = theDR["DrugId"];
                                        theRow["GenericId"] = 0;

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
                                        theStrengthId = 0;
                                        theFrequencyId = 0;
                                        theDuration = 0;
                                        theQtyPrescribed = 0;
                                        theQtyDispensed = 0;
                                        theFinanced = 99;
                                        if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224" || ddlTreatment.SelectedItem.Value.ToString() == "225")
                                        {
                                            theProphylaxis = 999;
                                        }
                                        else
                                        {
                                            theProphylaxis = 0;
                                        }
                                        #endregion
                                    }
                                }
                                //}

                            }
                            #region "18-Jun-07 - 12"
                            if (Session["Paperless"].ToString() != "1")
                            {
                                int ChkColFilled;
                                if (ddlTreatment.SelectedItem.Value.ToString() == "223")
                                {
                                    if (Session["SCMModule"] != null)
                                        ChkColFilled = 2;
                                    else
                                        ChkColFilled = 3;
                                    if ((TotColFilled > 0 && TotColFilled < ChkColFilled) && (theContainer.ID == "PnlOIARV"))
                                    {
                                        theDT.Rows.Clear();
                                        theRow = theDT.NewRow();
                                        theRow[0] = 99999;
                                        theDT.Rows.Add(theRow);
                                        return theDT;
                                    }
                                    else
                                        TotColFilled = 0;
                                }
                                else
                                {
                                    if ((TotColFilled > 0 && TotColFilled < 2) && (theContainer.ID == "PnlOIARV"))
                                    {
                                        theDT.Rows.Clear();
                                        theRow = theDT.NewRow();
                                        theRow[0] = 99999;
                                        theDT.Rows.Add(theRow);
                                        return theDT;
                                    }
                                    else
                                        TotColFilled = 0;
                                }
                            }

                            #endregion
                        }
                    }

                }
            }
            #endregion
        }


        else if (theContainer.ID == "pnlPedia")
        {
            #region "Additional ARV"
            //pnl 2 -id= PnlAddARV  btn-txt = "Other ARV Medications"
            if (Session["AddARV"] != null)
            {
                DataTable theADDARVDrug = (DataTable)Session["AddARV"];
                int TotelColFilled = 0;
                int DrugID = 0;
                if (theADDARVDrug == null)
                    return theDT;
                foreach (DataRow theDR in theADDARVDrug.Rows)
                {
                    theStrengthId = 0;
                    theFrequencyId = 0;
                    theDuration = 0;
                    theQtyPrescribed = 0;
                    theQtyDispensed = 0;
                    //theFinanced = 99;
                    DataRow[] DRStrength = ((DataSet)ViewState["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
                    if (DRStrength[0]["StrengthId"] != System.DBNull.Value)
                        theStrengthId = Convert.ToInt32(DRStrength[0]["StrengthId"]);
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

                                    DrugID = Convert.ToInt32(theDR["DrugId"]);
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                                    {
                                        c = x.ID.Length;

                                        if (x.ID.StartsWith("drgFrequency"))
                                        {
                                            if (x.ID.Substring(12, c - 12) == DrugID.ToString() && x.ID.StartsWith("drgFrequency"))
                                            {
                                                theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                                TotelColFilled++;
                                            }
                                        }
                                    }
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                    {
                                        //if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgDuration"))
                                        if (x.ID.StartsWith("drgDuration"))
                                        {
                                            c = x.ID.Length;
                                            if (x.ID.Substring(11, c - 11) == DrugID.ToString() && x.ID.StartsWith("drgDuration"))
                                            {
                                                if (((TextBox)x).Text != "")
                                                {
                                                    theDuration = Convert.ToDecimal(((TextBox)x).Text);
                                                    TotelColFilled++;
                                                }
                                            }
                                        }
                                        //if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
                                        if (x.ID.StartsWith("drgQtyPrescribed"))
                                        {
                                            c = x.ID.Length;
                                            if (x.ID.Substring(16, c - 16) == DrugID.ToString() && x.ID.StartsWith("drgQtyPrescribed"))
                                            {
                                                if (((TextBox)x).Text != "")
                                                {
                                                    theQtyPrescribed = Convert.ToDecimal(((TextBox)x).Text);
                                                    TotelColFilled++;
                                                }
                                            }
                                        }
                                        //if (x.ID.EndsWith(theDR["DrugId"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                        if (x.ID.StartsWith("drgQtyDispensed"))
                                        {
                                            c = x.ID.Length;
                                            if (x.ID.Substring(15, c - 15) == DrugID.ToString() && x.ID.StartsWith("drgQtyDispensed"))
                                            {
                                                //for paperless
                                                if (Session["Paperless"].ToString() == "1")
                                                {

                                                    if (((TextBox)x).Text == "")
                                                    {
                                                        theQtyDispensed = 99;
                                                        TotelColFilled++;

                                                    }
                                                    else
                                                    {
                                                        if (((TextBox)x).Text != "")
                                                        {
                                                            theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
                                                            TotelColFilled++;

                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    if (((TextBox)x).Text != "")
                                                    {
                                                        theQtyDispensed = Convert.ToDecimal(((TextBox)x).Text);
                                                        TotelColFilled++;
                                                    }
                                                }


                                            }
                                        }
                                    }
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                    {

                                        if (x.ID.StartsWith("chkProphylaxis"))
                                        {
                                            c = x.ID.Length;
                                            if (x.ID.EndsWith(DrugID.ToString()) && x.ID.StartsWith("chkProphylaxis"))
                                            {

                                                if (((CheckBox)x).Checked == true)
                                                {
                                                    theProphylaxis = 1;
                                                    TotelColFilled++;
                                                }
                                                else
                                                {
                                                    if (ddlTreatment.SelectedItem.Value.ToString() == "224" || ddlTreatment.SelectedItem.Value.ToString() == "225")
                                                    {
                                                        theProphylaxis = 0;
                                                        TotelColFilled++;
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                                if (Session["Paperless"].ToString() == "1")
                                {
                                    //if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theFinanced != 99 && theProphylaxis != 999)
                                    if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theProphylaxis != 999)
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
                                        theRow["Dose"] = 0;
                                        theRow["UnitId"] = 0;
                                        theRow["StrengthId"] = theStrengthId;
                                        theRow["FrequencyId"] = theFrequencyId;
                                        theRow["Duration"] = theDuration;
                                        theRow["QtyPrescribed"] = theQtyPrescribed;
                                        if (theQtyDispensed == 99)
                                        {
                                            theRow["QtyDispensed"] = 0;
                                        }
                                        else
                                        {
                                            theRow["QtyDispensed"] = theQtyDispensed;
                                        }
                                        //theRow["QtyDispensed"] = theQtyDispensed;

                                        theRow["Prophylaxis"] = theProphylaxis;
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
                                        if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224" || ddlTreatment.SelectedItem.Value.ToString() == "225")
                                        {
                                            theProphylaxis = 999;
                                        }
                                        else
                                        {
                                            theProphylaxis = 0;
                                        }

                                        #endregion
                                    }
                                }
                                else
                                {
                                    //if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theFinanced != 99 && theProphylaxis != 999)
                                    if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theQtyDispensed != 0 && theProphylaxis != 999)
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
                                        theRow["Prophylaxis"] = theProphylaxis;

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
                                        if (ddlTreatment.SelectedItem.Value.ToString() == "223" || ddlTreatment.SelectedItem.Value.ToString() == "224" || ddlTreatment.SelectedItem.Value.ToString() == "225")
                                        {
                                            theProphylaxis = 999;
                                        }
                                        else
                                        {
                                            theProphylaxis = 0;
                                        }

                                        #endregion
                                    }
                                }
                                int total = TotelColFilled;

                            }

                        }
                    }
                    #region "18-Jun-07 - 12"
                    int ChkColFilled;
                    if (ddlTreatment.SelectedItem.Value.ToString() == "222" || ddlTreatment.SelectedItem.Value.ToString() == "224" || ddlTreatment.SelectedItem.Value.ToString() == "225")
                    {
                        if (Session["SCMModule"] != null)
                            ChkColFilled = 3;
                        else
                            ChkColFilled = 4;
                        if ((TotelColFilled > 0 && TotelColFilled < ChkColFilled) && (theContainer.ID == "pnlPedia"))
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
                    else
                    {
                        if (Session["SCMModule"] != null)
                            ChkColFilled = 4;
                        else
                            ChkColFilled = 5;

                        if ((TotelColFilled > 0 && TotelColFilled < ChkColFilled) && (theContainer.ID == "pnlPedia"))
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


                    #endregion
                }
            }
            #endregion
        }
        else if (theContainer.ID == "pnlOtherTBMedicaton")
        {
            #region "TB Drug"
            //pnl4 - id = PnlOtherMedication btn-txt = "OI Treatment & Other Medications"
            if (Session["AddTB"] != null)
            {
                DataTable theADDTBDrug = (DataTable)Session["AddTB"];
                int DrugID = 0;
                int TotelColFilled = 0;
                if (theADDTBDrug == null)
                    return theDT;
                foreach (DataRow theDR in theADDTBDrug.Rows)
                {
                    DrugID = 0;
                    theStrengthId = 0;
                    theFrequencyId = 0;
                    theDuration = 0;
                    theQtyPrescribed1 = 0;
                    theQtyDispensed1 = 0;
                    theFinanced = 0;
                    theTreatmentPhase = "";
                    theMonth = 0;
                    DataRow[] DRStrength = ((DataSet)ViewState["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
                    if (DRStrength[0]["StrengthId"] != System.DBNull.Value)
                        theStrengthId = Convert.ToInt32(DRStrength[0]["StrengthId"]);
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

                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgFrequency"))
                                        {
                                            theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            TotelColFilled++;
                                        }
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgTreatmenPhase"))
                                        {
                                            theTreatmentPhase = Convert.ToString(((DropDownList)x).SelectedValue);
                                            if (theTreatmentPhase != "0")
                                                TotelColFilled++;
                                        }
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgTreatmenMonth"))
                                        {
                                            theMonth = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            if (theMonth != 0)
                                                TotelColFilled++;
                                        }

                                    }
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                    {


                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgDuration"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theDuration = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyPrescribed1 = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyDispensed1 = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                    }
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                    {

                                        if (x.ID.StartsWith("chkProphylaxis"))
                                        {
                                            c = x.ID.Length;
                                            if (x.ID.EndsWith(DrugID.ToString()) && x.ID.StartsWith("chkProphylaxis"))
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
                            }
                        }
                    }
                    if (Session["Paperless"].ToString() == "1")
                    {
                        //if (UnitId != 0 || theFrequencyId != 0 || Dose != 0 || theDuration != 0 || theQtyPrescribed != 0 || theQtyDispensed != 0 || theTreatmentPhase != "0" || theMonth != 0 || theFinanced != 99)
                        if (theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed1 != 0 && theTreatmentPhase != "0" && theMonth != 0 || theProphylaxis != 999)
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
                            theRow["StrengthId"] = theStrengthId;
                            theRow["FrequencyId"] = theFrequencyId;
                            theRow["Duration"] = theDuration;
                            theRow["QtyPrescribed"] = theQtyPrescribed1;
                            //theRow["QtyDispensed"] = theQtyDispensed;

                            theRow["QtyDispensed"] = theQtyDispensed1;
                            theRow["Financed"] = theFinanced;
                            theRow["TreatmentPhase"] = theTreatmentPhase;
                            theRow["TrMonth"] = theMonth;
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
                            if (ddlTreatment.SelectedItem.Value.ToString() == "223")
                            {
                                theProphylaxis = 999;
                            }
                            else
                            {
                                theProphylaxis = 0;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        if (theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed1 != 0 && theQtyDispensed1 != 0 && theTreatmentPhase != "0" && theMonth != 0 && theProphylaxis != 999)
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
                            theRow["StrengthId"] = theStrengthId;
                            theRow["FrequencyId"] = theFrequencyId;
                            theRow["Duration"] = theDuration;
                            theRow["QtyPrescribed"] = theQtyPrescribed1;
                            //theRow["QtyDispensed"] = theQtyDispensed;

                            theRow["QtyDispensed"] = theQtyDispensed1;
                            theRow["Financed"] = theFinanced;
                            theRow["TreatmentPhase"] = theTreatmentPhase;
                            theRow["TrMonth"] = theMonth;
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
                            if (ddlTreatment.SelectedItem.Value.ToString() == "223")
                            {
                                theProphylaxis = 999;
                            }
                            else
                            {
                                theProphylaxis = 0;
                            }
                            #endregion
                        }

                    }

                    #region "18-Jun-07 - 12"
                    int ChkColFilled;

                    if (Session["SCMModule"] != null)
                        ChkColFilled = 5;
                    else
                        ChkColFilled = 6;
                    if ((TotelColFilled > 0 && TotelColFilled < ChkColFilled) && (theContainer.ID == "pnlOtherTBMedicaton"))
                    {
                        theDT.Rows.Clear();
                        theRow = theDT.NewRow();
                        theRow[0] = 99999;
                        theDT.Rows.Add(theRow);
                        return theDT;
                    }
                    else
                        TotelColFilled = 0;


                    #endregion

                }
            }
            #endregion
        }
        else if (theContainer.ID == "PnlOtMed")
        {
            #region "Additional Drugs"
            //pnl4 - id = PnlOtherMedication btn-txt = "OI Treatment & Other Medications"
            if (Session["OtherDrugs"] != null)
            {
                DataTable theOtherDrug = (DataTable)Session["OtherDrugs"];
                int DrugID = 0;
                int TotelColFilled = 0;
                if (theOtherDrug == null)
                    return theDT;
                foreach (DataRow theDR in theOtherDrug.Rows)
                {
                    DrugID = 0;
                    theStrengthId = 0;
                    theFrequencyId = 0;
                    theDuration = 0;
                    theQtyPrescribed1 = 0;
                    theQtyDispensed1 = 0;
                    DataRow[] DRStrength = ((DataSet)ViewState["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
                    if (DRStrength[0]["StrengthId"] != System.DBNull.Value)
                        theStrengthId = Convert.ToInt32(DRStrength[0]["StrengthId"]);
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
                                        //if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("theUnit"))
                                        //{
                                        //    UnitId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                        //}
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgFrequency"))
                                        {
                                            theFrequencyId = Convert.ToInt32(((DropDownList)x).SelectedValue);
                                            TotelColFilled++;
                                        }
                                    }
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                    {
                                        //if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("theDose"))
                                        //{
                                        //    if (((TextBox)x).Text != "")
                                        //    {
                                        //        Dose = Convert.ToDecimal(((TextBox)x).Text);
                                        //    }
                                        //}

                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgDuration"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theDuration = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyPrescribed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyPrescribed1 = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgQtyDispensed"))
                                        {
                                            if (((TextBox)x).Text != "")
                                            {
                                                theQtyDispensed1 = Convert.ToDecimal(((TextBox)x).Text);
                                                TotelColFilled++;
                                            }
                                        }
                                    }
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                    {

                                        if (x.ID.StartsWith("chkProphylaxis"))
                                        {
                                            c = x.ID.Length;
                                            if (x.ID.EndsWith(DrugID.ToString()) && x.ID.StartsWith("chkProphylaxis"))
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
                            }
                        }
                    }
                    if (Session["Paperless"].ToString() == "1")
                    {
                        if (theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed1 != 0 || theProphylaxis != 999)
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
                            theRow["StrengthId"] = theStrengthId;
                            theRow["FrequencyId"] = theFrequencyId;
                            theRow["Duration"] = theDuration;
                            theRow["QtyPrescribed"] = theQtyPrescribed1;
                            //theRow["QtyDispensed"] = theQtyDispensed;

                            theRow["QtyDispensed"] = theQtyDispensed1;
                            theRow["Financed"] = 99;
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
                            if (ddlTreatment.SelectedItem.Value.ToString() == "223")
                            {
                                theProphylaxis = 999;
                            }
                            else
                            {
                                theProphylaxis = 0;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        if (theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed1 != 0 && theQtyDispensed1 != 0 || theProphylaxis != 999)
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
                            theRow["StrengthId"] = theStrengthId;
                            theRow["FrequencyId"] = theFrequencyId;
                            theRow["Duration"] = theDuration;
                            theRow["QtyPrescribed"] = theQtyPrescribed1;
                            //theRow["QtyDispensed"] = theQtyDispensed;

                            theRow["QtyDispensed"] = theQtyDispensed1;
                            theRow["Financed"] = 99;
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
                            if (ddlTreatment.SelectedItem.Value.ToString() == "223")
                            {
                                theProphylaxis = 999;
                            }
                            else
                            {
                                theProphylaxis = 0;
                            }
                            #endregion
                        }
                    }
                    #region "18-Jun-07 - 12"
                    ///////////
                    int ChkColFilled;
                    if (Session["SCMModule"] != null)
                        ChkColFilled = 3;
                    else
                        ChkColFilled = 4;

                    if ((TotelColFilled > 0 && TotelColFilled < ChkColFilled) && (theContainer.ID == "PnlOtMed"))
                    {
                        theDT.Rows.Clear();
                        theRow = theDT.NewRow();
                        theRow[0] = 99999;
                        theDT.Rows.Add(theRow);
                        return theDT;
                    }
                    else
                        TotelColFilled = 0;


                    //////////


                    #endregion

                }
            }
            #endregion
        }
        return theDT;
    }

    private void SelectFrequency(Control theContainer)
    {
        string pnlName = theContainer.ID;
        string temp = "";
        decimal SingleDose = 0;
        int Frequency = 0;
        int DrgId = 0;
        int doseDrgId = 0;
        int totalDrgId = 0;
        int olddrgid = 0;
        int cpos;
        foreach (Control y in theContainer.Controls)
        {

            if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                foreach (Control x in y.Controls)
                {

                    if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                    {
                        SelectFrequency(x);
                    }
                    else
                    {
                        if (x.GetType() == typeof(System.Web.UI.WebControls.Label))
                        {
                            if (x.ID.StartsWith("drgNm"))
                            {
                                cpos = Convert.ToInt32(x.ID.IndexOf("^"));
                                if (cpos > 0)
                                {
                                    DrgId = Convert.ToInt32(x.ID.Substring(5, cpos - 5));
                                }
                                else
                                {
                                    DrgId = Convert.ToInt32(x.ID.Substring(5, x.ID.Length - 5));
                                }
                                //DrgId = Convert.ToInt32(x.ID.Substring(5));
                                if (DrgId != olddrgid)
                                {
                                    olddrgid = DrgId;
                                    SingleDose = 0;
                                    Frequency = 0;

                                }
                            }
                        }
                        if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                        {
                            if (x.ID.StartsWith("drgFrequency"))
                            //if (x.FindControl( "drgFrequency" + Convert.ToString(DrgId)))
                            {
                                cpos = Convert.ToInt32(x.ID.IndexOf("^"));
                                if (cpos > 0)
                                {
                                    DrgId = Convert.ToInt32(x.ID.Substring(12, cpos - 12));
                                }
                                else
                                {
                                    DrgId = Convert.ToInt32(x.ID.Substring(12, x.ID.Length - 12));
                                }
                                //DrgId = Convert.ToInt32(x.ID.Substring(12));

                                if (((DropDownList)x).SelectedItem != null)
                                    temp = ((DropDownList)x).SelectedItem.Text.ToString();

                                if (temp == "OD")
                                {
                                    Frequency = 1;
                                }
                                else if (temp == "BD" || temp == "bid")
                                {
                                    Frequency = 2;
                                }
                                else if (temp == "1OD")
                                {
                                    Frequency = 1;
                                }
                                else if (temp == "2OD")
                                {
                                    Frequency = 2;
                                }
                                else if (temp == "1BD")
                                {
                                    Frequency = 2;
                                }
                                else if (temp == "3OD" || temp == "TD")
                                {
                                    Frequency = 3;
                                }
                                else if (temp == "qid" || temp == "QID" || temp == "QD")
                                {
                                    Frequency = 4;
                                }
                                else if (temp == "Weekly")
                                {
                                    Frequency = 7;
                                }
                            }
                        }
                        if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                        {
                            if (x.ID.StartsWith("drgDose"))
                            {
                                cpos = Convert.ToInt32(x.ID.IndexOf("^"));
                                if (cpos > 0)
                                {
                                    doseDrgId = Convert.ToInt32(x.ID.Substring(7, cpos - 7));
                                }
                                else
                                {
                                    doseDrgId = Convert.ToInt32(x.ID.Substring(7, x.ID.Length - 7));
                                }
                                //doseDrgId = Convert.ToInt32(x.ID.Substring(7));
                                olddrgid = doseDrgId;
                                if (DrgId == doseDrgId)
                                {

                                    if (((TextBox)x).Text != "")
                                    {
                                        string sValue = ((TextBox)x).Text.ToString();
                                        int DecPos = sValue.IndexOf(".");
                                        int DecValue = 0;
                                        if (DecPos > 0)
                                        {
                                            DecValue = Convert.ToInt32(sValue.Substring(DecPos + 1, sValue.Length - (DecPos + 1)));
                                        }
                                        if (DecValue > 0)
                                        {
                                            SingleDose = Convert.ToDecimal(((TextBox)x).Text);

                                        }
                                        else
                                        {
                                            SingleDose = Convert.ToDecimal(((TextBox)x).Text);
                                        }
                                        //SingleDose = Convert.ToInt32(((TextBox)x).Text);
                                    }
                                }
                            }
                            if (x.ID.StartsWith("drgTotalDose"))
                            {
                                cpos = Convert.ToInt32(x.ID.IndexOf("^"));
                                if (cpos > 0)
                                {
                                    totalDrgId = Convert.ToInt32(x.ID.Substring(12, cpos - 12));
                                }
                                else
                                {
                                    totalDrgId = Convert.ToInt32(x.ID.Substring(12, x.ID.Length - 12));
                                }
                                //totalDrgId = Convert.ToInt32(x.ID.Substring(12));
                                olddrgid = doseDrgId;
                                if (DrgId == totalDrgId)
                                {

                                    ((TextBox)x).Text = Convert.ToString(Frequency * SingleDose);
                                    if (((TextBox)x).Text == "0")
                                    {
                                        ((TextBox)x).Text = "";
                                    }
                                }

                                //theDose = Convert.ToInt32(((TextBox)x).Text);

                            }
                        }
                        //}
                    }
                }

            //if (ddlFrequency.SelectedItem.Text == "OD")
            //{
            //    int a = 1;
            //    txtTotalDose = txtDose.Text * a;

            //}
        }
    }
    protected void BSF_Attributes()
    {

        txtHeight.Attributes.Add("OnBlur", "CalcualteBSF('" + txtBSA.ClientID + "','" + txtWeight.ClientID + "','" + txtHeight.ClientID + "')");
        txtWeight.Attributes.Add("OnBlur", "CalcualteBSF('" + txtBSA.ClientID + "','" + txtWeight.ClientID + "','" + txtHeight.ClientID + "')");
    }

    /***************** Delete Record *****************/

    private void DeleteForm()
    {
        int theResultRow, OrderNo;
        string FormName;
        OrderNo = Convert.ToInt32(Session["PatientVisitId"]);
        FormName = "Paediatric Pharmacy";

        IPediatric PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
        theResultRow = (int)PediatricManager.DeletePediatricForms(FormName, OrderNo, Convert.ToInt32(Session["PatientId"].ToString()), Convert.ToInt32(ViewState["UserID"]));

        if (theResultRow == 0)
        {
            IQCareMsgBox.Show("RemoveFormError", this);
            return;
        }
        else
        {
            string theUrl;
            theUrl = string.Format("{0}", "../ClinicalForms/frmPatient_Home.aspx");
            Response.Redirect(theUrl);
        }

    }


    # region "Amitava"
    // Create Custom Controls 
    // Creation Date : 05-May-2007 
    // Amitava Sinha
    private void PutCustomControl()
    {
        ICustomFields CustomFields;
        CustomFieldClinical theCustomField = new CustomFieldClinical();
        try
        {
             
            CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields,BusinessProcess.Administration");
            DataSet theDS = CustomFields.GetCustomFieldListforAForm(Convert.ToInt32(ApplicationAccess.PaediatricPharmacy));

            if (theDS.Tables[0].Rows.Count != 0)
            {
                theCustomField.CreateCustomControlsForms(pnlCustomList, theDS, "PPharm");
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

        //ICustomFields CustomFields;
        //string pnlName = "PnlCustomList";
        //CustomFieldClinical theCustomField = new CustomFieldClinical();
        //BindFunctions theBindMgr = new BindFunctions();
        //TableName = string.Empty;
        //Int32 ii = 0;
        //try
        //{

        //    CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields,BusinessProcess.Administration");
        //    DataSet theDS = CustomFields.GetCustomFieldListforAForm(Convert.ToInt32(ApplicationAccess.PaediatricPharmacy));
        //    if (theDS != null && theDS.Tables[0].Rows.Count > 0)
        //    {
        //        sbParameter = new StringBuilder();
        //        TableName = theDS.Tables[0].Rows[0]["FeatureName"].ToString().Replace(" ", "_");

        //        pnlCustomList.Visible = true;
        //        pnlCustomList.Controls.Clear();
        //        arl = new ArrayList();
        //        pnlCustomList.Controls.Add(new LiteralControl("<TABLE border='1' cellpadding=6 cellspacing=0 width=100%>"));

        //        foreach (DataRow dr in theDS.Tables[0].Rows)
        //        {
        //            if (ii % 2 == 0)
        //                pnlCustomList.Controls.Add(new LiteralControl("<TR >"));
        //            if (dr[1].ToString() == "1")
        //                pnlCustomList.Controls.Add(new LiteralControl("<TD >"));
        //            else if (dr[1].ToString() == "6")
        //                pnlCustomList.Controls.Add(new LiteralControl("<TD align='left' nowrap='noWrap' >"));
        //            else if ((dr[1].ToString() == "3") || (dr[1].ToString() == "4") || (dr[1].ToString() == "5") || (dr[1].ToString() == "7") || (dr[1].ToString() == "8") || (dr[1].ToString() == "9"))
        //                pnlCustomList.Controls.Add(new LiteralControl("<TD align='left'>"));

        //            //Select List
        //            if (dr[1].ToString() == "4")
        //            {
        //                Label customLabel = new Label();
        //                customLabel.ID = pnlName + "lbl" + ii.ToString();
        //                customLabel.Text = dr[0].ToString().Replace("_", " ");
        //                customLabel.Text = customLabel.Text.Replace("PPharm", "");
        //                sbParameter.Append(",[" + dr[0].ToString() + "]");
        //                customLabel.Width = 200;
        //                customLabel.CssClass = "labelright";
        //                customLabel.Font.Bold = true;

        //                pnlCustomList.Controls.Add(customLabel);

        //                pnlCustomList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

        //                DropDownList ddlSelectList = new DropDownList();
        //                ddlSelectList.ID = pnlName + "Selectlist" + dr[0].ToString();
        //                ddlSelectList.Width = 180;

        //                DataSet dsSelectList = CustomFields.GetCustomList(Convert.ToInt32(dr[2].ToString()));
        //                if (dsSelectList != null && dsSelectList.Tables[0].Rows.Count > 0)
        //                {
        //                    if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        //                    {
        //                        IQCareUtils theUtilsCF = new IQCareUtils();
        //                        DataView theDVCF = new DataView(dsSelectList.Tables[0]);
        //                        theDVCF.RowFilter = "DeleteFlag=0";
        //                        DataTable theDTCF = (DataTable)theUtilsCF.CreateTableFromDataView(theDVCF);
        //                        /*******/
        //                        theBindMgr.BindCombo(ddlSelectList, theDTCF, "Name", "ID");
        //                        theDVCF.Dispose();
        //                        theDTCF.Clear();
        //                    }
        //                    else
        //                    {
        //                        theBindMgr.BindCombo(ddlSelectList, dsSelectList.Tables[0], "Name", "Id");
        //                    }

        //                }
        //                pnlCustomList.Controls.Add(ddlSelectList);

        //            }
        //            //Multi Select List
        //            else if (dr[1].ToString() == "9")
        //            {
        //                if (arl.Count == 0)
        //                {
        //                    arl.Add("dtl_CustomField_" + TableName.Replace("-", "_").ToString() + "_" + dr[0].ToString());
        //                }
        //                foreach (object obj in arl)
        //                {
        //                    if (obj.ToString() != "dtl_CustomField_" + TableName.Replace("-", "_").ToString() + "_" + dr[0].ToString())
        //                    {
        //                        arl.Add("dtl_CustomField_" + TableName.Replace("-", "_").ToString() + "_" + dr[0].ToString());
        //                        break;
        //                    }
        //                }
        //                Label theMultiSelectlbl = new Label();
        //                theMultiSelectlbl.ID = pnlName + "lbl" + ii.ToString();
        //                theMultiSelectlbl.Text = dr[0].ToString().Replace("_", " ");
        //                theMultiSelectlbl.Text = theMultiSelectlbl.Text.Replace("PPharm", "");
        //                theMultiSelectlbl.Width = 200;
        //                theMultiSelectlbl.CssClass = "labelright";
        //                theMultiSelectlbl.Font.Bold = true;
        //                pnlCustomList.Controls.Add(theMultiSelectlbl);

        //                pnlCustomList.Controls.Add(new LiteralControl("<div class = 'Customdivborder' nowrap='nowrap'>"));

        //                CheckBoxList chkMultiList = new CheckBoxList();
        //                chkMultiList.ID = pnlName + "Multiselectlist" + dr[0].ToString();
        //                chkMultiList.RepeatLayout = RepeatLayout.Flow;
        //                chkMultiList.CssClass = "check";
        //                chkMultiList.Width = 300;

        //                DataSet dsMultiSelectList = CustomFields.GetCustomList(Convert.ToInt32(dr[2].ToString()));
        //                if (dsMultiSelectList != null && dsMultiSelectList.Tables[0].Rows.Count > 0)
        //                {
        //                    if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        //                    {
        //                        IQCareUtils theUtilsCF = new IQCareUtils();
        //                        DataView theDVCF = new DataView(dsMultiSelectList.Tables[0]);
        //                        theDVCF.RowFilter = "DeleteFlag=0";
        //                        DataTable theDTCF = (DataTable)theUtilsCF.CreateTableFromDataView(theDVCF);
        //                        /*******/
        //                        theBindMgr.BindCheckedList(chkMultiList, theDTCF, "Name", "Id");

        //                        theDVCF.Dispose();
        //                        theDTCF.Clear();
        //                    }
        //                    else
        //                    {
        //                        theBindMgr.BindCheckedList(chkMultiList, dsMultiSelectList.Tables[0], "Name", "Id");
        //                    }
        //                }
        //                pnlCustomList.Controls.Add(chkMultiList);

        //                pnlCustomList.Controls.Add(new LiteralControl("</div>"));

        //            }

        //            theCustomField.CreateCustomControls(pnlCustomList, pnlName, ref sbParameter, dr, ref TableName, "PPharm", ii);

        //            ii++;
        //        }
        //    }
        //    ViewState["ControlCreated"] = "CC";
        //    pnlCustomList.Controls.Add(new LiteralControl("</TABLE>"));

        //}
        //catch
        //{

        //}
        //finally
        //{
        //    CustomFields = null;
        //}

    }

    //Amitava Sinha
    //Generating full DML Statement 
    private void UpdateCustomFieldsValues()
    {
        GenerateCustomFieldsValues(pnlCustomList);
        string sqlstr = string.Empty;
        Int32 PatientId = Convert.ToInt32(Session["PatientId"]);
        string sqlselect;
        string strdelete;
        Int32 PharmacyId = 0;
        DateTime OrderedbyDate = System.DateTime.Now;

        if (ViewState["PharmacyId"] != null)
            PharmacyId = Convert.ToInt32(ViewState["PharmacyId"]);
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
                sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedByDate " + sbParameter.ToString() + " )";
                //sqlstr += " VALUES(" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
                sqlstr += " VALUES(" + PatientId.ToString() + "," + Session["AppLocationId"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
                ViewState["CustomFieldsData"] = 1;
                //Session["AppLocationId"]

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
                            //strdelete = "DELETE from [" + obj.ToString() + "] where ptn_pk= " + PatientId.ToString() + " and LocationID=" + Application["AppLocationID"] + " and ptn_pharmacy_pk=" + PharmacyId;
                            strdelete = "DELETE from [" + obj.ToString() + "] where ptn_pk= " + PatientId.ToString() + " and LocationID=" + Session["AppLocationId"] + " and ptn_pharmacy_pk=" + PharmacyId;
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

                                        //sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate, [" + obj.ToString().Substring(ispos, iepos) + "])";
                                        sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedByDate, [" + obj.ToString().Substring(ispos, iepos) + "])";
                                        //sqlselect += " VALUES (" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";
                                        sqlselect += " VALUES (" + PatientId.ToString() + "," + Session["AppLocationId"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";



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
    //Amitava Sinha
    //Generating full DML Statement 
    private void InsertCustomFieldsValues()
    {
        GenerateCustomFieldsValues(pnlCustomList);
        string sqlstr = string.Empty;
        string sqlselect;
        Int32 PharmacyId = 0;
        DateTime OrderedbyDate = System.DateTime.Now;
        Int32 PatientId = Convert.ToInt32(Session["PatientId"]);
        ICustomFields CustomFields;

        if (ViewState["PharmacyId"] != null)
            PharmacyId = Convert.ToInt32(ViewState["PharmacyId"]);
        if (txtpharmOrderedbyDate.Value.ToString() != "")
            OrderedbyDate = Convert.ToDateTime(txtpharmOrderedbyDate.Value.ToString());

        if (sbValues.ToString().Trim() != "")
        {
            //Rupesh
            //sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate " + sbParameter.ToString() + " )";
            sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedByDate " + sbParameter.ToString() + " )";
            //sqlstr += " VALUES(" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
            sqlstr += " VALUES(" + PatientId.ToString() + "," + Session["AppLocationId"].ToString() + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";

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
                                    //sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate, [" + obj.ToString().Substring(ispos, iepos) + "])";
                                    //sqlselect += " VALUES (" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";
                                    sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedByDate, [" + obj.ToString().Substring(ispos, iepos) + "])";
                                    sqlselect += " VALUES (" + PatientId.ToString() + "," + Session["AppLocationId"].ToString() + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";

                                    try
                                    {
                                        CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
                                        icount = CustomFields.SaveCustomFieldValues(sqlselect.ToString());
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
                            }
                        }
                    }
                    p += 1;
                }
            }
        }
    }
    //Amitava Sinha
    //Generate a string builder for Insert Query Values 
    //and Update Query Values 
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
        if (ViewState["CustomFieldsMulti"] != null || ViewState["CustomFieldsMulti"] == null)
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
    //Amitava Sinha 
    //Populate Old Data in the Custom Field
    //private void FillOldCustomData(Control Cntrl, Int32 PatID)
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
            string theTblName = string.Empty;
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
            dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + theTblName.ToString().Replace("-", "_"), theColName, Convert.ToInt32(PatID.ToString()), 0, 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.PaediatricPharmacy));
            CustomFieldClinical theCustomManager = new CustomFieldClinical();

            theCustomManager.FillCustomFieldData(theCustomFields, dsvalues, pnlCustomList, "PPharm");
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
        //    //dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + TableName.ToString().Replace("-", "_"), sbParameter.ToString(), Convert.ToInt32(PatID.ToString()), Convert.ToInt32(Application["AppLocationID"].ToString()), 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.PaediatricPharmacy));
        //    dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + TableName.ToString().Replace("-", "_"), sbParameter.ToString(), Convert.ToInt32(PatID.ToString()), 0,0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.PaediatricPharmacy));

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
        //                    //dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()), Convert.ToInt32(Application["AppLocationID"].ToString()), 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.PaediatricPharmacy));
        //                    dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()), 0, 0, 0,Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.PaediatricPharmacy));
        //                    if (dsmvalues != null && dsmvalues.Tables[0].Rows.Count > 0)
        //                        ViewState["CustomFieldsMulti"] = 1;
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
        //                        ViewState["CustomFieldsMulti"] = 1;


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
    #endregion


    #region Validation Functions
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

                //else if (DS.Tables[1].Rows[0]["PrevDate"].ToString() != "0" && DS.Tables[2].Rows[0]["LaterDate"].ToString() != "0")
                //{
                //    if (Convert.ToInt32(DS.Tables[3].Rows[0]["LocationID"]) != Convert.ToInt32(DS.Tables[1].Rows[0]["TransferredToID"]))
                //    {
                //        if (Convert.ToDateTime(txtpharmOrderedbyDate.Value) < Convert.ToDateTime(DS.Tables[1].Rows[0]["PrevDate"]) || Convert.ToDateTime(txtpharmOrderedbyDate.Value) > Convert.ToDateTime(DS.Tables[2].Rows[0]["LaterDate"]))
                //        {
                //            IQCareMsgBox.Show("TransferDate_6", this);
                //            txtpharmOrderedbyDate.Focus();
                //            return false;
                //        }
                //    }
                //}
                //else if (DS.Tables[1].Rows[0]["PrevDate"].ToString() != "0" && DS.Tables[2].Rows[0]["LaterDate"].ToString() == "0")
                //{
                //    if (Convert.ToDateTime(txtpharmOrderedbyDate.Value) < Convert.ToDateTime(DS.Tables[1].Rows[0]["PrevDate"]))
                //    {
                //        IQCareMsgBox.Show("TransferDate_7", this);
                //        txtpharmOrderedbyDate.Focus();
                //        return false;
                //    }
                //}
            }

        }
        return true;
    }
    private Boolean fieldValidationPaperLess()
    {
        //
        IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
        theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
        IQCareUtils theUtils = new IQCareUtils();

        if (ddlPharmOrderedbyName.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Prescribed By";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }

        //if (ddlPharmReportedbyName.SelectedIndex == 0)
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "Dispensed By";
        //    IQCareMsgBox.Show("BlankDropDown", theMsg, this);
        //    return false;
        //}

        if (ddlPharmSignature.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Signature";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }

        if (txtpharmOrderedbyDate.Value.Trim() == "")
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "PrescribedByDate";
            IQCareMsgBox.Show("BlankTextBox", theMsg, this);
            return false;
        }
        if (txtpharmOrderedbyDate.Value.Trim() != "")
        {
            DateTime theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value.Trim()));

            if (ViewState["EnrolmentDate"] != null)
            {
                DateTime theEnrolmentDate = Convert.ToDateTime(ViewState["EnrolmentDate"].ToString());
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

        //if (txtpharmReportedbyDate.Value.Trim() == "")
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "DispensedByDate";
        //    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
        //    return false;
        //}

        if (txtpharmReportedbyDate.Value.Trim() != "")
        {
            DateTime theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value.Trim()));

            if (ViewState["EnrolmentDate"] != null)
            {
                DateTime theEnrolmentDate = Convert.ToDateTime(ViewState["EnrolmentDate"].ToString());
                if (theEnrolmentDate > theVisitDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailReportedDate", this);
                    txtpharmReportedbyDate.Focus();
                    return false;
                }
                else if (theVisitDate > theCurrentDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailReportedTDate", this);
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

        //if (txtWeight.Text.Trim() == "")
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "Weight";
        //    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
        //    return false;
        //}
        if (txtWeight.Text.Trim() != "")
        {
            if (Convert.ToDecimal(txtWeight.Text.ToString()) <= 0)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Weight";
                IQCareMsgBox.Show("GreatThanZero", theMsg, this);
                return false;
            }
        }

        //if (txtHeight.Text.Trim() == "")
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "Height";
        //    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
        //    return false;
        //}
        if (txtHeight.Text.Trim() != "")
        {
            if (Convert.ToDecimal(txtHeight.Text.ToString()) <= 0)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Height";
                IQCareMsgBox.Show("GreatThanZero", theMsg, this);
                return false;
            }
        }

        Decimal AgeD = Convert.ToDecimal(txtYr.Text.Trim().ToString()) + Convert.ToDecimal(txtMon.Text.Trim().ToString()) / 12;
        if (AgeD > 17)
        {
            IQCareMsgBox.Show("PharmacyDetailAge", this);
            return false;
        }

        //---Non-ART already filled : starts-- 29Feb08//

        DataTable theDT = ((DataSet)ViewState["MasterData"]).Tables[15];
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

        //---Non-ART already filled : ends-- 29Feb08 //


        int PtnID = Convert.ToInt32(Session["PatientId"]);
        IPediatric PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
        DataSet dsExist = PediatricManager.GetExistPharmacyForm(PtnID, Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)));

        if (dsExist != null && dsExist.Tables[0].Rows.Count > 0)
        {
            if ((Convert.ToInt32(Session["PatientVisitId"]) != 0) && (Convert.ToInt32(dsExist.Tables[0].Rows[0][0]) == 0))
            {
                if (Convert.ToDateTime(ViewState["OrigOrdDate"]) != Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)))
                {
                    IQCareMsgBox.Show("PharmacyDetailExists", this);
                    return false;
                }
            }
            //for patient transfer date check
            int PatientID = Convert.ToInt32(Session["PatientId"]);
            if (TransferValidation(PatientID) == false)
            {
                return false;
            }
            if ((Convert.ToInt32(Session["PatientVisitId"]) == 0) && (ViewState["PharmacyDetail"] == null))
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
    private Boolean FieldValidation()
    {

        //
        IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
        theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
        IQCareUtils theUtils = new IQCareUtils();

        if (ddlTreatment.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Treatment Program";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }


        if (ddlProvider.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "ART Provider";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }


        if (ddlPharmOrderedbyName.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Prescribed By";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }
        if (Session["Paperless"].ToString() == "0")
        {
            if (Session["SCMModule"] == null)
            {
                if (ddlPharmReportedbyName.SelectedIndex == 0)
                {
                    MsgBuilder theMsg = new MsgBuilder();
                    theMsg.DataElements["Control"] = "Dispensed By";
                    IQCareMsgBox.Show("BlankDropDown", theMsg, this);
                    return false;
                }
            }
        }
        if (ddlPharmSignature.SelectedIndex == 0)
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "Signature";
            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
            return false;
        }

        if (txtpharmOrderedbyDate.Value.Trim() == "")
        {
            MsgBuilder theMsg = new MsgBuilder();
            theMsg.DataElements["Control"] = "PrescribedByDate";
            IQCareMsgBox.Show("BlankTextBox", theMsg, this);
            return false;
        }
        if (txtpharmOrderedbyDate.Value.Trim() != "")
        {
            DateTime theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value.Trim()));

            if (ViewState["EnrolmentDate"] != null)
            {
                DateTime theEnrolmentDate = Convert.ToDateTime(ViewState["EnrolmentDate"].ToString());
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
        if (Session["Paperless"].ToString() == "0")
        {
            if (Session["SCMModule"] == null)
            {
                if (txtpharmReportedbyDate.Value.Trim() == "")
                {
                    MsgBuilder theMsg = new MsgBuilder();
                    theMsg.DataElements["Control"] = "DispensedByDate";
                    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
                    return false;
                }
            }
        }
        if (txtpharmReportedbyDate.Value.Trim() != "")
        {
            DateTime theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value.Trim()));

            if (ViewState["EnrolmentDate"] != null)
            {
                DateTime theEnrolmentDate = Convert.ToDateTime(ViewState["EnrolmentDate"].ToString());
                if (theEnrolmentDate > theVisitDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailReportedDate", this);
                    txtpharmReportedbyDate.Focus();
                    return false;
                }
                else if (theVisitDate > theCurrentDate)
                {
                    IQCareMsgBox.Show("PharmacyDetailReportedTDate", this);
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

        //if (txtWeight.Text.Trim() == "")
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "Weight";
        //    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
        //    return false;
        //}
        if (txtWeight.Text.Trim() != "")
        {
            if (Convert.ToDecimal(txtWeight.Text.ToString()) <= 0)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Weight";
                IQCareMsgBox.Show("GreatThanZero", theMsg, this);
                return false;
            }
        }

        //if (txtHeight.Text.Trim() == "")
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "Height";
        //    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
        //    return false;
        //}
        if (txtHeight.Text.Trim() != "")
        {
            if (Convert.ToDecimal(txtHeight.Text.ToString()) <= 0)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Height";
                IQCareMsgBox.Show("GreatThanZero", theMsg, this);
                return false;
            }
        }

        Decimal AgeD = Convert.ToDecimal(txtYr.Text.Trim().ToString()) + Convert.ToDecimal(txtMon.Text.Trim().ToString()) / 12;
        if (AgeD > 17)
        {
            IQCareMsgBox.Show("PharmacyDetailAge", this);
            return false;
        }

        //---Non-ART already filled : starts-- 29Feb08//

        DataTable theDT = ((DataSet)ViewState["MasterData"]).Tables[15];
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
        //---------------------
        IDrug PharmacyManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        DataSet dsExistNonP_less = PharmacyManager.GetExistPharmacyFormDespensedbydate(Convert.ToInt32(Session["PatientId"]), Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value)));
        if (dsExistNonP_less.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(Session["PatientVisitId"]) != Convert.ToInt32(dsExistNonP_less.Tables[0].Rows[0][0]))
            {
                IQCareMsgBox.Show("PharmacyDetailExists", this);
                return false;
            }
        }
        //if (ddlTreatment.SelectedValue != "225")
        //{

        //    if (Convert.ToInt32(ViewState["RegimenLine"]) == 0)
        //    {

        //        IQCareMsgBox.Show("RegimenLineExists", this);
        //        return false;
        //    }
        //}
        return true;
    }
    private Boolean DuplicateRegimenValidate(DataTable DrugTable, DataSet Master)
    {
        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
        IQCareUtils theUtils = new IQCareUtils();
        DataSet objPatientStatus = new DataSet();
        #region "Regimen"

        string theRegimen = "";

        for (int i = 0; i < DrugTable.Rows.Count; i++)
        {
            if (Convert.ToInt32(DrugTable.Rows[i]["GenericId"]) == 0)
            {
                DataView theDV = new DataView(Master.Tables[0]);
                theDV.RowFilter = "Drug_Pk = " + DrugTable.Rows[i]["DrugId"] + " and DrugTypeID = 37"; ///DrugAbbreviation = " + theDrgMst.Rows[i][2];    
                if (theDV.Count > 0)
                {
                    if (theRegimen == "")
                    {
                        theRegimen = theDV[0]["GenericAbbrevation"].ToString();
                    }
                    else
                    {
                        theRegimen = theRegimen + "/" + theDV[0]["GenericAbbrevation"].ToString();
                    }
                }
                theRegimen = theRegimen.Trim();

                //Fixed Dose Combination
                DataView theDV1 = new DataView(Master.Tables[23]);
                theDV1.RowFilter = "Drug_Pk = " + DrugTable.Rows[i]["DrugId"] + " and DrugTypeID = 37"; ///DrugAbbreviation = " + theDrgMst.Rows[i][2];    
                if (theDV1.Count > 0)
                {
                    theRegimen = "";
                    if (theRegimen == "")
                    {
                        theRegimen = theDV1[0]["GenericAbbrevation"].ToString();
                    }
                    else
                    {
                        theRegimen = theRegimen + "/" + theDV1[0]["GenericAbbrevation"].ToString();
                    }
                }
                theRegimen = theRegimen.Trim();
            }
            else
            {
                DataView theDV = new DataView(Master.Tables[4]);
                theDV.RowFilter = "GenericId = " + DrugTable.Rows[i]["GenericId"] + " and DrugTypeID = 37"; ///DrugAbbreviation = " + theDrgMst.Rows[i][2];    
                if (theDV.Count > 0)
                {
                    if (theRegimen == "")
                    {
                        theRegimen = theDV[0]["GenericAbbrevation"].ToString();
                    }
                    else
                    {
                        theRegimen = theRegimen + "/" + theDV[0]["GenericAbbrevation"].ToString();
                    }
                }
                theRegimen = theRegimen.Trim();
            }
        }

        #endregion
        string strreporteddate = "";
        if (Session["SCMModule"] == null)
        {
            strreporteddate = txtpharmReportedbyDate.Value;
        }
        else
        {
            strreporteddate = txtpharmOrderedbyDate.Value;

        }
        objPatientStatus = DrugManager.GetPatientRecordformStatus(Convert.ToInt32(Session["PatientId"]));
        if (objPatientStatus.Tables[4].Rows.Count > 0)
        {
            if (theRegimen != "")
            {
                if ((Convert.ToInt32(Session["PatientVisitId"]) == 0) && (Session["PharmacyId"] == null))
                {

                    if (objPatientStatus.Tables[5].Rows.Count > 0)
                    {
                        string ARVTherapy = objPatientStatus.Tables[5].Rows[0][0].ToString();
                        DateTime dtVisitDate = Convert.ToDateTime(objPatientStatus.Tables[5].Rows[0][1].ToString());
                        string PreRegimeType = objPatientStatus.Tables[6].Rows[0][0].ToString();
                        int Regimen = 0;
                        int PreRegimen = 0;

                        foreach (char a in PreRegimeType)
                        {
                            PreRegimen += System.Convert.ToInt32(a);
                        }

                        foreach (char b in theRegimen)
                        {
                            Regimen += System.Convert.ToInt32(b);
                        }

                        if (PreRegimen != Regimen)
                        {
                            if (ARVTherapy == "95")
                            {
                                if (dtVisitDate <= Convert.ToDateTime(theUtils.MakeDate(strreporteddate)))
                                {
                                    IQCareMsgBox.Show("currentregimenchange", this);
                                    return false;
                                }
                            }
                        }
                        else if (PreRegimen == Regimen)
                        {
                            if (ARVTherapy == "98")
                            {
                                if (dtVisitDate <= Convert.ToDateTime(theUtils.MakeDate(strreporteddate)))
                                {
                                    IQCareMsgBox.Show("Changeregimen", this);
                                    return false;
                                }
                            }
                        }
                    }

                }
                if ((Convert.ToInt32(Session["PatientVisitId"]) != 0))
                {
                    if (objPatientStatus.Tables[4].Rows.Count != 1)
                    {
                        if (objPatientStatus.Tables[5].Rows.Count > 0)
                        {
                            string ARVTherapy = objPatientStatus.Tables[5].Rows[0][0].ToString();
                            DateTime dtVisitDate = Convert.ToDateTime(objPatientStatus.Tables[5].Rows[0][1].ToString());
                            string PreRegimeType = objPatientStatus.Tables[6].Rows[0][0].ToString();
                            int Regimen = 0;
                            int PreRegimen = 0;

                            foreach (char a in PreRegimeType)
                            {
                                PreRegimen += System.Convert.ToInt32(a);
                            }

                            foreach (char b in theRegimen)
                            {
                                Regimen += System.Convert.ToInt32(b);
                            }

                            if (PreRegimen != Regimen)
                            {
                                if (ARVTherapy == "95")
                                {
                                    if (dtVisitDate <= Convert.ToDateTime(theUtils.MakeDate(strreporteddate)))
                                    {
                                        IQCareMsgBox.Show("currentregimenchange", this);
                                        return false;
                                    }
                                }
                            }
                            else if ((PreRegimen == Regimen) && (PreRegimeType != theRegimen))
                            {
                                if (ARVTherapy == "98")
                                {
                                    if (dtVisitDate <= Convert.ToDateTime(theUtils.MakeDate(strreporteddate)))
                                    {
                                        IQCareMsgBox.Show("Changeregimen", this);
                                        return false;
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

    #endregion


    #region "User Events"
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["AppLocation"] == null || Session.Count == 0 || Session["AppUserID"].ToString() == "")
        {
            IQCareMsgBox.Show("SessionExpired", this);
            Response.Redirect("~/frmlogin.aspx",true);
        }
        DataView theSCMDV = new DataView((DataTable)Session["AppModule"]);
        theSCMDV.RowFilter = "ModuleId=201";

        if (Session["SCMModule"] != null)
        {
            EnableDisableControl();
            ddlPharmReportedbyName.Enabled = false;
            txtpharmReportedbyDate.Disabled = true;
        }
        if (Session["Paperless"].ToString() == "0" && Session["SCMModule"] != null)
        {
            txtautoDrugName.Enabled = false;
            lbldispensedby.Attributes.Remove("Class");
            lbldispensedbydate.Attributes.Remove("Class");
            //btnsave.Enabled = false;
        }
        else if (Session["Paperless"].ToString() == "1" && Session["SCMModule"] != null)
        {
            txtautoDrugName.Enabled = true;
        }
        else if (Session["SCMModule"] == null)
        {
            txtautoDrugName.Enabled = true;
        }

        if (IsPostBack != true)
        {

            if (Request.QueryString["opento"] == "ArtForm")
            {

                Session["PatientVisitId"] = 0;

            }

            if (Request.QueryString["name"] == "Delete")
            {
                btnsave.Text = "Delete";

            }
            Init_Form();
        }
    }
    private void EnableDisableControl()
    {
        //--- For Cancel event, on saving the form ---
        string script = "<script language = 'javascript' defer ='defer' id = 'Disable'>\n";
        script += "var dddispBy=document.getElementById('" + ddlPharmReportedbyName.ClientID + "');\n";
        script += "var txtdispDate=document.getElementById('" + txtpharmReportedbyDate.ClientID + "');\n";
        script += "document.getElementById('appDateimg2').disabled = true;\n";
        //script += "alert('Hello'+ImgCalDate);\n";
        //script += "ImgCalDate.visible=false;\n";
        //script += "dddispBy.disabled=true;\n";
        //script += "txtdispDate.disabled=true;\n";
        script += "</script>\n";
        Page.RegisterStartupScript("Disable", script);
    }
    protected void btnOtherTBMedicaton_Click(object sender, EventArgs e)
    {
        string theScript;

        //Application.Add("MasterData", (DataSet)ViewState["MasterData"]);
        Application.Add("MasterData", ViewState["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)ViewState["AddTB"]);
        theScript = "<script language='javascript' id='DrgPopup'>\n";
        //theScript += "window.open('frmDrugSelector.aspx?DrugType=31','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "window.open('frmPharmacySelector.aspx?DrugType=31','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "</script>\n";
        Page.RegisterClientScriptBlock("DrgPopup", theScript);

    }
    protected void ddlFrequency_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectFrequency(pnlPedia);
        //SelectFrequency(PnlOIARV);
        //SelectFrequency(PnlAdARV);
        SelectFrequency(PnlOtMed);
    }

    protected void ddlPharmSignature_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPharmSignature.SelectedIndex == 3)
        {
            ddlCounselerName.Visible = true;
            if (ddlCounselerName.Items.Count > 0)
                ddlCounselerName.SelectedIndex = 0;
        }
        else
        {
            ddlCounselerName.Visible = false;
            lblCounselorName.Visible = false;
            txtCounselorName.Visible = false;
        }

    }

    protected void ddlCounselerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblCounselorName.Visible = true;
        txtCounselorName.Visible = true;
        txtCounselorName.Text = ddlCounselerName.SelectedItem.Text;
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["opento"] == "ArtForm")
        {
            string script;
            script = "";
            script = "<script language = 'javascript' defer ='defer' id = 'closeself_00'>\n";
            script += "self.close();\n";
            script += "</script>\n";
            RegisterStartupScript("closeself_00", script);
            return;
        }

        string theUrl;
        if (Request.QueryString["name"] == "Delete")
        {
            theUrl = string.Format("{0}?sts={1}", "../ClinicalForms/frmClinical_DeleteForm.aspx",  ViewState["Status"].ToString());
        }
        else
        {
            theUrl = string.Format("{0}?", "../ClinicalForms/frmPatient_Home.aspx");
        }
        Response.Redirect(theUrl);
    }

    protected void BtnAddARV_Click(object sender, EventArgs e)
    {
        string theScript;

        //Application.Add("MasterData", (DataSet)ViewState["MasterData"]);
        Application.Add("MasterData", ViewState["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)ViewState["AddARV"]);
        theScript = "<script language='javascript' id='DrgPopup'>\n";
        //theScript += "window.open('frmDrugSelector.aspx?DrugType=37','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "window.open('frmPharmacySelector.aspx?DrugType=37','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "</script>\n";
        Page.RegisterClientScriptBlock("DrgPopup", theScript);
    }
    private Boolean QuentityDispence()
    {
        if (ddlPharmReportedbyName.SelectedIndex == 0 || txtpharmReportedbyDate.Value.Trim() == "")//dispance by
        {
            //IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            //theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
            //IQCareUtils theUtils = new IQCareUtils();
            IQCareMsgBox.Show("PharmacyDispensedDateby", this);
            return false;

        }
        return true;
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
        string strStatus = ViewState["Status"].ToString();
        string strPharmacyID = Session["PharmacyId"].ToString();
        string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
        script += "var ans;\n";
        script += "ans=window.confirm('Drug Order saved successfully. Do you want to close?');\n";
        script += "if (ans==true)\n";

        script += "{\n";

        if (Request.QueryString["opento"] == "ArtForm")
        {
            if (Convert.ToInt32(Session["ArtEncounterPatientVisitId"]) > 0)
            {
                Session["PatientVisitId"] = Session["ArtEncounterPatientVisitId"];
            }
            else
            {
                Session["LabId"] = 0;
                Session["PatientVisitId"] = 0;
            }
            script += "self.close();";
        }
        else
        {
            script += "Redirect('" + strSession + "','" + strRequest + "','" + strStatus + "');\n";
        }
        script += "}\n";
        //script += "else \n";
        //script += "{\n";
        //script += "window.location.href('../Pharmacy/frmPharmacy_Paediatric.aspx');\n";
        //script += "}\n";
        script += "</script>\n";
        RegisterStartupScript("confirm", script);




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
        string theTreatmentPhase = "";
        int theMonth = 0;
        int theFinanced = 99;
        DataSet theGenericDS = new DataSet();

        #endregion
        #region "pnltbregimen"
        //if (theContainer.ID == "pnltbregimen")
        //{
        //    theRegimenID = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
        //    if (theRegimenID.ToString() != "0")
        //    {
        //        theGenericDS = DrugManager.Get_TBRegimen_Detail(Convert.ToInt32(theRegimenID));
        //    }

        //    if (theGenericDS.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in theGenericDS.Tables[0].Rows)
        //        {
        //            theRegimenID = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
        //            if (ddlARVCombReg.SelectedItem.Value.ToString() != "0")
        //            {
        //                theFrequencyId = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
        //            }
        //            if (txtARVCombRegQtyPres.Text != "")
        //            {
        //                theQtyPrescribed = Convert.ToDecimal(txtARVCombRegQtyPres.Text);
        //            }
        //            if (txtARVCombRegDuraton.Text != "")
        //            {
        //                theDuration = Convert.ToDecimal(txtARVCombRegDuraton.Text);
        //            }
        //            if (txtARVCombRegQtyDesc.Text != "")
        //            {
        //                theQtyDispensed = Convert.ToDecimal(txtARVCombRegQtyDesc.Text);
        //            }
        //            if (ddlTreatmentphase.SelectedItem.Value.ToString() != "0")
        //            {
        //                theTreatmentPhase = ddlTreatmentphase.SelectedItem.Value.ToString();
        //            }
        //            if (ddTrMonths.SelectedItem.Value.ToString() != "0")
        //            {
        //                theMonth = Convert.ToInt32(ddTrMonths.SelectedItem.Value);
        //            }
        //            if (theRegimenID != 0 && theDuration != 0)
        //            {

        //                DataRow theRow;
        //                theRow = theDT.NewRow();
        //                theRow["GenericId"] = Convert.ToInt32(dr["GenericID"]);
        //                theRow["DrugId"] = 0;
        //                theRow["TBRegimenId"] = theRegimenID;
        //                theRow["Dose"] = 0;
        //                theRow["UnitId"] = 0;
        //                theRow["SingleDose"] = 0;
        //                theRow["StrengthId"] = 1;
        //                theRow["FrequencyId"] = theFrequencyId;
        //                theRow["Duration"] = theDuration;
        //                theRow["QtyPrescribed"] = theQtyPrescribed;
        //                theRow["QtyDispensed"] = theQtyDispensed;
        //                theRow["Financed"] = 1;
        //                theRow["TreatmentPhase"] = theTreatmentPhase;
        //                theRow["TrMonth"] = theMonth;
        //                theDT.Rows.Add(theRow);
        //                #region "Reset Variables
        //                Dose = 0;
        //                UnitId = 0;
        //                theRegimenID = 0;
        //                theStrengthId = 0;
        //                theFrequencyId = 0;
        //                theDuration = 0;
        //                theQtyPrescribed = 0;
        //                theQtyDispensed = 0;
        //                theFinanced = 99;
        //                theTreatmentPhase = "";
        //                theMonth = 0;
        //                #endregion
        //            }

        //        }
        //    }
        //}
        #endregion
        return theDT;
    }
    public Boolean NonARTProPhalasxisCheck(DataTable dt, string drug_id)
    {
        Boolean blnCheck = false;
        DataRow[] theFilDT = dt.Select("Drug_pk=" + drug_id + "");
        string drugtypeid = theFilDT[0]["drugtypeid"].ToString();
        if (drugtypeid != "37")
        {
            blnCheck = true;
        }
        return blnCheck;
    }
    private Boolean ProPhalaxisCheck(DataTable dt)
    {
        Boolean blnProCheck = true;
        DataTable theDTDrug = ((DataSet)ViewState["MasterData"]).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            if (r["Prophylaxis"].ToString() == "999")
            {
                blnProCheck = NonARTProPhalasxisCheck(theDTDrug, r["DrugId"].ToString());
            }
        }
        return blnProCheck;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        Boolean blnQtyDispensed = false;
        if (Request.QueryString["name"] == "Delete")
        {
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
            int PatientID, DispensedBy, OrderBy, OrderType, EmployeeID = 0, Signature, LocationID;
            decimal Height = 0, Weight = 0;
            int PeriodTaken = 0;
            Boolean flag1 = false;
            if (FieldValidation() == false)
            {
                return;
            }
            DataSet theDrgMst = ((DataSet)ViewState["MasterData"]);
            ViewState.Remove("Data");
            ViewState["Data"] = MakeDrugTable(PnlRegiment);
            if (((DataTable)ViewState["Data"]).Rows.Count > 0)
            {
                DataRow[] theFilDT = ((DataTable)ViewState["Data"]).Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    ViewState.Remove("Data");
                    IQCareMsgBox.Show("RegimenLineExists", this);
                    return;
                }

            }
            if (ddlTreatment.SelectedValue.ToString() != "225")
            {
                ViewState["Data"] = MakeDrugTable(pnlPedia);
            }
            //--rupesh
            if (((DataTable)ViewState["Data"]).Rows.Count > 0)
            {
                DataRow[] theFilDT = ((DataTable)ViewState["Data"]).Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    ViewState.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }

            }
            if (ddlTreatment.SelectedValue.ToString() == "222")
            {
                if (DuplicateRegimenValidate((DataTable)ViewState["Data"], (DataSet)ViewState["MasterData"]) == false)
                {
                    return;
                }
            }

            //ViewState["Data"] = MakeDrugTable(PnlOIARV);
            //////////Done by Sanjay On 22-02-2011///////////////////////////////////////
            //////////
            //////////DataTable dt = (DataTable)ViewState["Data"];
            //////////if (dt.Rows.Count == 0)
            //////////{
            //////////    IQCareMsgBox.Show("PharmacyIncompleteData", this);
            //////////    return;
            //////////}
            //////////for (int i = 0; i < dt.Rows.Count; i++)
            //////////{
            //////////    if (Session["Paperless"].ToString() == "1")
            //////////    {
            //////////        if (dt.Rows[i]["Strengthid"].ToString() == "0" || dt.Rows[i]["FrequencyId"].ToString() == "0" || dt.Rows[i]["Duration"].ToString() == "0" || dt.Rows[i]["QtyPrescribed"].ToString() == "0")
            //////////        {
            //////////            IQCareMsgBox.Show("PharmacyIncompleteData", this);
            //////////            return;
            //////////        }
            //////////    }
            //////////    else
            //////////    {
            //////////        if (dt.Rows[i]["Strengthid"].ToString() == "0" || dt.Rows[i]["FrequencyId"].ToString() == "0" || dt.Rows[i]["Duration"].ToString() == "0" || dt.Rows[i]["QtyPrescribed"].ToString() == "0" || dt.Rows[i]["QtyDispensed"].ToString() == "0")
            //////////        {
            //////////            IQCareMsgBox.Show("PharmacyIncompleteData", this);
            //////////            return;
            //////////        }
            //////////    }
            //////////}
            ///////////////////////////////////////////////////////////////////////////////
            ViewState["Data"] = MakeDrugTable(PnlOtMed);
            if (((DataTable)ViewState["Data"]).Rows.Count > 0)
            {
                DataRow[] theFilDT = ((DataTable)ViewState["Data"]).Select("DrugId=99999");

                if (theFilDT.Length > 0)
                {
                    ViewState.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }
            }
            //ViewState["Data"] = MakeInfantHealthSection();
            DataTable theDT = MakeDrugTable(pnlOtherTBMedicaton);
            if (theDT.Rows.Count > 0)
            {
                DataRow[] theFilDT = theDT.Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    ViewState.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }
            }

            if (theDT.Rows.Count == 0)
            {
                IQCareMsgBox.Show("PharmacyIncompleteData", this);
                return;
            }



            if (ddlTreatment.SelectedValue.ToString() == "223")
            {
                if (ProPhalaxisCheck(theDT) == false)
                {
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }
            }

            //if (theDT.Rows.Count < 1)
            //{
            //    IQCareMsgBox.Show("PharmacyNoData", this);
            //    return;
            //}

            //if(!ValidateFixedCombData(PnlFixed))
            //{
            //    IQCareMsgBox.Show("FixedDoseCombNoData", this);
            //    return;
            //}
            if (Session["Paperless"].ToString() == "1")
            {
                if (Session["SCMModule"] != null)
                {
                    //blnQtyDispensed = true;
                }
                else
                {
                    for (int i = 0; i < theDT.Rows.Count; i++)
                    {
                        if (theDT.Rows[i]["QtyDispensed"].ToString() != "0" && theDT.Rows[i]["QtyDispensed"].ToString() != "")
                        {
                            blnQtyDispensed = true;
                        }
                    }
                }

                if (blnQtyDispensed == true)
                {
                    if (fieldValidationPaperLess() == false)
                    {
                        return;
                    }
                    //else
                    //{
                    //    for (int i = 0; i < theDT.Rows.Count; i++)
                    //    {

                    //        if (dt.Rows[i]["Strengthid"].ToString() == "0" || dt.Rows[i]["FrequencyId"].ToString() == "0" || dt.Rows[i]["Duration"].ToString() == "0" || dt.Rows[i]["QtyPrescribed"].ToString() == "0" || dt.Rows[i]["QtyDispensed"].ToString() == "0")
                    //        {
                    //            IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    //            return;
                    //        }

                    //    }
                    //}
                }
                if (blnQtyDispensed == true)
                {
                    if (QuentityDispence() == false)
                    {
                        return;
                    }
                }
            }

            IQCareUtils theUtils = new IQCareUtils();
            IPediatric PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            try
            {
                PatientID = Convert.ToInt32(Session["PatientId"]);
                int LocationId = 0;
                if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                    LocationId = Convert.ToInt32(Session["AppLocationId"]);
                else
                    LocationId = Convert.ToInt32(Session["ServiceLocationId"].ToString());

                PeriodTaken = Convert.ToInt32(ddlPeriodTaken.SelectedItem.Value);
                OrderBy = Convert.ToInt32(ddlPharmOrderedbyName.SelectedValue.ToString());
                DispensedBy = Convert.ToInt32(ddlPharmReportedbyName.SelectedValue.ToString());
                theTreatmentID = Convert.ToInt32(ddlTreatment.SelectedValue.ToString());
                EmployeeID = Convert.ToInt32(ddlPharmSignature.SelectedValue.ToString());
                Signature = 1;
                theProviderID = Convert.ToInt32(ddlProvider.SelectedValue.ToString());
                if (txtHeight.Text != "")
                    Height = Convert.ToDecimal(txtHeight.Text);
                if (txtWeight.Text != "")
                    Weight = Convert.ToDecimal(txtWeight.Text);
                if (txtpharmOrderedbyDate.Value == "")
                {
                    thepharmOrderedbyDate = Convert.ToDateTime(theUtils.MakeDate("01-01-1900"));
                }
                else
                {
                    thepharmOrderedbyDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value));
                }


                if (txtpharmReportedbyDate.Value == "")
                {
                    thepharmReportedbyDate = Convert.ToDateTime(theUtils.MakeDate("01-01-1900"));
                }
                else
                {
                    thepharmReportedbyDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value));
                }
                OrderType = 117;

                ////////////// Signature Varification///////////
                //if (ddlPharmSignature.SelectedIndex == 1)
                //{
                //    EmployeeID = 0;
                //    flag1 = false;
                //}
                //else if (ddlPharmSignature.SelectedIndex == 2)
                //{

                //    EmployeeID = 0;
                //    flag1 = true;
                //}
                //else if (ddlPharmSignature.SelectedIndex == 3)
                //{
                //    EmployeeID = Convert.ToInt32(ddlCounselerName.SelectedValue);
                //    flag1 = true;
                //}
                int SCMFlag;//if SCM Module is On then SCMFlag=1 else SCMFlag=2
                if (Session["SCMModule"] != null)
                    SCMFlag = 1;
                else
                    SCMFlag = 2;
                DateTime dtappnt = Convert.ToDateTime(theUtils.MakeDate("01-01-1900"));
                if ((Convert.ToInt32(Session["PatientVisitId"]) == 0) && (ViewState["PharmacyDetail"] == null))
                {

                    if (ViewState["PharmacyDetail"] == null)
                    {
                        CustomFieldClinical theCustomManager = new CustomFieldClinical();
                        DataTable theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Insert", ApplicationAccess.PaediatricPharmacy, (DataSet)ViewState["CustomFieldsDS"]);
                        
                        ViewState["PharmacyDetail"] = PediatricManager.SaveUpdatePaediatricDetail(PatientID, 0, Convert.ToInt32(ViewState["LocationId"]), Convert.ToInt32(ViewState["RegimenLine"]), txtClinicalNotes.Text, theDT, theDrgMst, OrderBy, Convert.ToDateTime(thepharmOrderedbyDate), DispensedBy, Convert.ToDateTime(thepharmReportedbyDate), Signature, EmployeeID, OrderType, VisitType, Convert.ToInt32(ViewState["UserID"]), Height, Weight, Convert.ToInt32(ViewState["flagFDC"]), theTreatmentID, theProviderID, theCustomDataDT, PeriodTaken, 1, SCMFlag, dtappnt,0); // rupesh 19-sep-07 for ARVprovider
                        ViewState["PharmacyId"] = Convert.ToInt32(ViewState["PharmacyDetail"].ToString());
                        Session["PharmacyId"] = Convert.ToInt32(ViewState["PharmacyDetail"].ToString());
                        Session["PatientVisitId"] = Session["PharmacyId"];
                        if (Convert.ToInt32(ViewState["PharmacyDetail"]) == 0)
                        {
                            IQCareMsgBox.Show("PharmacyDetailExists", this);
                            return;
                        }
                        else
                        {
                            SaveCancel();


                        }

                        theDT.Rows.Clear();
                    }


                }
                else
                {

                    int PharmacyID = Convert.ToInt32(Session["PharmacyId"]);
                    if (PharmacyID != 0)
                    {
                        ViewState["PharmacyDetail"] = PharmacyID;
                    }


                    CustomFieldClinical theCustomManager = new CustomFieldClinical();
                    DataTable theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Update", ApplicationAccess.PaediatricPharmacy, (DataSet)ViewState["CustomFieldsDS"]);
                    ViewState["PharmacyDetail"] = PediatricManager.SaveUpdatePaediatricDetail(PatientID, Convert.ToInt32(ViewState["PharmacyDetail"]), Convert.ToInt32(Session["AppLocationId"]), Convert.ToInt32(ViewState["RegimenLine"]), txtClinicalNotes.Text, theDT, theDrgMst, OrderBy, Convert.ToDateTime(thepharmOrderedbyDate), DispensedBy, Convert.ToDateTime(thepharmReportedbyDate), Signature, EmployeeID, OrderType, VisitType, Convert.ToInt32(ViewState["UserID"]), Height, Weight, Convert.ToInt32(ViewState["flagFDC"]), theTreatmentID, theProviderID, theCustomDataDT, PeriodTaken, 2, SCMFlag, dtappnt,0); // rupesh 19-sep-07 for ARV Provider
                    ViewState["PharmacyId"] = Convert.ToInt32(PharmacyID);
                    Session["PharmacyId"] = PharmacyID.ToString();


                    if (Convert.ToInt32(ViewState["PharmacyDetail"]) == 0)
                    {
                        IQCareMsgBox.Show("ErrorinSavingPaediatricDetail", this);
                    }
                    else
                    {
                        //IQCareMsgBox.Show("PaediatricDetailUpdate", this);
                    }
                    if (Convert.ToInt32(ViewState["PharmacyDetail"]) != 0)
                    {
                        SaveCancel();
                        //Session["PatientVisitId"] = 0;

                    }

                }

            }
            catch (Exception err)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("C1#", theMsg, this);
            }
            finally
            {
                PediatricManager = null;
            }
        }
    }

    protected void OtherMedication_Click(object sender, EventArgs e)
    {
        string theScript;

        //Application.Add("MasterData", (DataSet)ViewState["MasterData"]);
        Application.Add("MasterData", (DataTable)ViewState["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)ViewState["OtherDrugs"]);

        theScript = "<script language='javascript' id='DrgPopup'>\n";
        //theScript += "window.open('frmDrugSelector.aspx?DrugType=0','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "window.open('frmPharmacySelector.aspx?DrugType=36','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "</script>\n";
        Page.RegisterClientScriptBlock("DrgPopup", theScript);
    }

    protected void txtDose_TextChanged(object sender, EventArgs e)
    {
        SelectFrequency(pnlPedia);
        //SelectFrequency(PnlOIARV);
        //SelectFrequency(PnlAdARV);
        SelectFrequency(PnlOtMed);
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //int PatientId = 0;
        //int LocationID = 0;
        //PatientId = Convert.ToInt32(ViewState["PtnID"]);
        //string theUrl = "";
        //if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        //{
        //    theUrl = string.Format("{0}?PatientId={1}&sts={2}&locationid={3}", "../ClinicalForms/frmPatient_Home.aspx", PatientId, ViewState["Status"].ToString(), LocationID); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        //}
        //else if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
        //{
        //    theUrl = string.Format("{0}?PatientId={1}&sts={2}&locationid={3}", "../ClinicalForms/frmPatient_History.aspx", PatientId, ViewState["Status"].ToString(),LocationID); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        //}
        //ClearObjects();
        //Response.Redirect(theUrl);
        DeleteForm();
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

    #endregion

    private void ClearObjects()
    {
        ViewState.Remove("OrigOrdDate");
        ViewState.Remove("PtnID");
        ViewState.Remove("UserID");
        ViewState.Remove("LocationId");
        ViewState.Remove("SelectedDrug");
        ViewState.Remove("MasterData");
        ViewState.Remove("Status");
        ViewState.Remove("EnrolmentDate");
        ViewState.Remove("MasterDrugTable");
        ViewState.Remove("OldDS");
        ViewState.Remove("AddARV");
        ViewState.Remove("OtherDrugs");
        ViewState.Remove("PharmacyId");
        ViewState.Remove("PatientId");
        ViewState.Remove("Data");
        ViewState.Remove("ControlCreated");
        ViewState.Remove("CustomFieldsData");
        ViewState.Remove("CustomFieldsMulti");
        ViewState.Remove("PharmacyDetail");
    }

    protected void ddlTreatment_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region "Check ARTStop"
        Session["TreatmentProg"] = ddlTreatment.SelectedValue.ToString();
        if (Session["TreatmentProg"].ToString() == "225")
        {
            pnlPedia.Enabled = false;
        }
        else
        {
            pnlPedia.Enabled = true;
        }
        IDrug theValidationManger = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
        DataTable theValidateDT = theValidationManger.CheckARTStopStatus(Convert.ToInt32(Session["PatientId"]));
        if ((theValidateDT.Rows.Count > 0 && theValidateDT.Rows[0]["ARTStatus"].ToString() == "ART Stopped" && Convert.ToInt32(Session["PatientVisitId"]) == 0 && Convert.ToInt32(ddlTreatment.SelectedValue) == 222) ||
            (theValidateDT.Rows.Count > 0 && theValidateDT.Rows[0]["ARTStatus"].ToString() == "ART Stopped" && Convert.ToInt32(Session["PatientVisitId"]) > 0
            && Convert.ToDateTime(txtpharmReportedbyDate.Value) >= Convert.ToDateTime(theValidateDT.Rows[0]["ARTEndDate"]) && Convert.ToInt32(ddlTreatment.SelectedValue) == 222))
        {
            pnlPedia.Enabled = false;
            //PnlAdARV.Enabled = false;
            //PnlFixed.Enabled = false;
            //BtnAddARV.Enabled = false;
        }
        //else
        //{
        //    pnlPedia.Enabled = true;
        //    //PnlAdARV.Enabled = true;
        //    //PnlFixed.Enabled = true;
        //    //BtnAddARV.Enabled = true;
        //}
        theValidationManger = null;
        theValidateDT.Dispose();
        #endregion
        if (Convert.ToInt32(ddlTreatment.SelectedValue) == 222)
        {
            EnableDisableAllCheckBoxControls(pnlPedia, false);
        }
    }
    private void EnableDisableAllCheckBoxControls(Control parent, bool checkedVal)
    {
        try
        {
            foreach (Control y in parent.Controls)
            {
                if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                {
                    //
                    foreach (Control x in y.Controls)
                    {
                        if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            EnableDisableAllCheckBoxControls(x, checkedVal);
                        }
                        else if (x.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                        {

                            CheckBox chk = (CheckBox)x;
                            chk.Checked = checkedVal;
                        }
                    }
                }
            }

        }
        catch (Exception exp)
        {
            throw new Exception("SelectAllCheckBoxControls  " + exp.Message);
        }
    }
    protected void btnNonARV_Click(object sender, EventArgs e)
    {
        string theScript;

        //Application.Add("MasterData", (DataSet)ViewState["MasterData"]);
        Application.Add("MasterData", ViewState["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)ViewState["AddTB"]);
        theScript = "<script language='javascript' id='DrgPopup'>\n";
        //theScript += "window.open('frmDrugSelector.aspx?DrugType=31','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "window.open('frmPharmacySelector.aspx?DrugType=100','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "</script>\n";
        Page.RegisterClientScriptBlock("DrgPopup", theScript);

    }
}
    #endregion