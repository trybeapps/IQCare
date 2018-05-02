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
//using System.Diagnostics;
using Interface.Security;
using Application.Common;
using Application.Presentation;
using Interface.Pharmacy;
using Interface.Administration;
using Interface.Clinical;
using System.Text;
using System.Collections.Generic;
using AjaxControlToolkit;
#endregion

public partial class frmPharmacy_Adult : BasePage
{
    ////////////////////////////////////////////////////////////////////
    // Code Written By   : Rakhi Tyagi
    // Written Date      : 8th Oct 2006
    // Modified By       : Sanjay Rana 
    // Modification Date : 19th Jan 2007
    // Description       : Adult Pharmacy
    // Modified By       : Amitava Sinha
    // Modification Date : 5th Feb 2007
    /// /////////////////////////////////////////////////////////////////

    #region Declaration of variable
    public DataTable theDrugTable;
    public DataTable AddARV;
    public DataTable OtherDrugs;
    public DataTable TBDrugs;
    public DataTable OIDrugs;
    DateTime theCurrentDate;
    IIQCareSystem IQCareSecurity;
    //Amitava Sinha
    int icount;
    StringBuilder sbParameter;
    StringBuilder sbValues;
    string strmultiselect;
    String TableName;
    ArrayList arl;
    int HeadingRegimen = 0;
    int CountFixedComb = 0;
    DataSet theDS;
    DataSet theExistDS = new DataSet();
    DataView theDV = new DataView();

    Application.Common.Utility theUti = new Application.Common.Utility();
    #endregion

    #region "AutoComplete Method"
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static List<string> SearchDrugs(string prefixText, int count)
    {

        List<string> Drugs = new List<string>();
        IDrug objRptFields;
        objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
        //creating Sql Query
        string sqlQuery;
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
            DataSet theAutoDS = (DataSet)Session["MasterData"];
            int DrugId;

            if (hdCustID.Value.ToString() != "")
            {
                if ((Convert.ToInt32(hdCustID.Value) != 0))
                {
                    DrugId = Convert.ToInt32(hdCustID.Value);
                    theAutoDV = new DataView(theAutoDS.Tables[0]);
                    //if (HttpContext.Current.Session["Paperless"].ToString() == "1" && HttpContext.Current.Session["SCMModule"] != null)
                    //{
                    //    theAutoDV.RowFilter = "Drug_Pk = " + DrugId +" and Stock>0";
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
                            theDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
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
                            DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistDT, 0, PnlDrug);
                            Session["AddARV"] = ExistDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistDT, 0, PnlDrug);
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
                            theTBDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
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
                            DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistTBDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistTBDT, 0, pnlOtherTBMedicaton);
                            Session["AddTB"] = ExistTBDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistTBDT, 0, pnlOtherTBMedicaton);
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
                            theOthDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
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
                            DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistOIDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistOIDT, 0, PnlOIARV);
                            Session["OIDrugs"] = ExistOIDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistOIDT, 0, PnlOIARV);
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
                            theOthDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
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
                            DR[4] = theAutoDT.Rows[0]["StrengthId"];
                            ExistOthDT.Rows.Add(DR);
                            LoadAdditionalDrugs(ExistOthDT, 0, PnlOtherMedication);
                            Session["OtherDrugs"] = ExistOthDT;
                        }
                        else
                        {
                            IQCareMsgBox.Show("DrugExists", this);
                            txtautoDrugName.Text = "";
                            LoadAdditionalDrugs(ExistOthDT, 0, PnlOtherMedication);
                            return;

                        }
                    }
                    if (Convert.ToInt32(Session["PatientVisitId"]) > 0 && Session["ExistPharmacyData"] != null)
                    {
                        foreach (DataRow dr in ((DataTable)(Session["ExistPharmacyData"])).Rows)
                        {

                            FillOldData(PnlOIARV, dr);
                            FillOldData(PnlDrug, dr);
                            FillOldData(PnlOtherMedication, dr);
                            FillOldData(pnlOtherTBMedicaton, dr);
                            //FillMaternalHealth(dr);

                        }
                    }

                    txtautoDrugName.Text = "";

                }
            }

            //gvCustomer.DataSource = GetCustomerDetail(Convert.ToInt32(hdCustID.Value));
            //gvCustomer.DataBind();
        }
        catch (Exception err)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = err.Message.ToString();
            IQCareMsgBox.Show("#C1", theBuilder, this);
        }
        //}
    }
    private void EnableDisableControl()
    {
        string script = "<script language = 'javascript' defer ='defer' id = 'Disable'>\n";
        script += "document.getElementById('appDateimg2').disabled = true;\n";
        script += "</script>\n";
        Page.RegisterStartupScript("Disable", script);
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["AppLocation"] == null || Session.Count == 0 || Session["AppUserID"].ToString() == "")
            {
                IQCareMsgBox.Show("SessionExpired", this);
                Response.Redirect("~/frmlogin.aspx",true);
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["opento"] == "ArtForm")
                {
                    Session["PatientVisitId"] = 0;
                }

            }


            if (Request.QueryString["Prog"] == "ART")
            {
                (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["HIVPatientStatus"]);
            }
            else
            {
                (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblpntStatus") as Label).Text = Convert.ToString(Session["PMTCTPatientStatus"]);
            }
            //(Master.FindControl("lblRoot") as Label).Text = "Pharmacy Forms >>";
            //(Master.FindControl("lblMark") as Label).Visible = false;
            //(Master.FindControl("lblheader") as Label).Text = "Adult Pharmacy";
            //(Master.FindControl("lblformname") as Label).Text = "Adult Pharmacy Form";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Text = "Pharmacy Forms >> ";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "Adult Pharmacy";
            (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblformname") as Label).Text = "Adult Pharmacy Form";
            Session["PtnRegCTC"] = "";
            Session["CustomfrmDrug"] = "";

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



            PutCustomControl();
            if (Session["SCMModule"] != null)
            {
                EnableDisableControl();
                ddlPharmReportedbyName.Enabled = false;
                txtpharmReportedbyDate.Disabled = true;
            }
            if (Session["Paperless"].ToString() == "0" && Session["SCMModule"] != null)
            {
                txtautoDrugName.Enabled = false;
                //btnsave.Enabled = false;
                lbldispensedby.Attributes.Remove("Class");
                lbldispensedbydate.Attributes.Remove("Class");
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
                if (Session["PatientVisitId"] != null)
                    //ViewState["PatientVisitId"] = Session["PatientVisitId"];
                    txtpharmOrderedbyDate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
                txtpharmOrderedbyDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");

                txtpharmReportedbyDate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
                txtpharmReportedbyDate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
                //ddlTreatment.Attributes.Add("onchange", "fnCheckUnCheck()");

                if (Session["TechnicalAreaId"].ToString() == "1")
                {
                    ddlTreatment.SelectedIndex = 1;
                }
                if (Request.QueryString["name"] == "Delete")
                {
                    btnsave.Text = "Delete";
                }
                //if (Convert.ToInt32(ViewState["PatientVisitId"]) == 0)
                //{
                //    // theUti.SetSession();
                //}
                Session["AddARV"] = null;
                Session["OIDrugs"] = null;
                Session["OtherDrugs"] = null;
                Session["MasterDrugTable"] = null;
                ViewState["PharmacyId"] = null;
                Session["AddTB"] = null;

                Init_Form();

                #region "Authentication"
                AuthenticationManager Authentiaction = new AuthenticationManager();
                if (Authentiaction.HasFunctionRight(ApplicationAccess.AdultPharmacy, FunctionAccess.Print, (DataTable)Session["UserRight"]) == false)
                {
                    btnPrint.Enabled = false;

                }
                if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
                {
                    if (Authentiaction.HasFunctionRight(ApplicationAccess.AdultPharmacy, FunctionAccess.View, (DataTable)Session["UserRight"]) == false)
                    {
                        int PatientID = Convert.ToInt32(Session["PatientId"].ToString());
                        string theUrl = "";
                        theUrl = string.Format("{0}", "../ClinicalForms/frmPatient_History.aspx");
                        Response.Redirect(theUrl);
                    }
                    else if (Authentiaction.HasFunctionRight(ApplicationAccess.AdultPharmacy, FunctionAccess.Update, (DataTable)Session["UserRight"]) == false)
                    {
                        btnsave.Enabled = false;
                    }
                }
                else if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                {
                    if (Authentiaction.HasFunctionRight(ApplicationAccess.AdultPharmacy, FunctionAccess.Add, (DataTable)Session["UserRight"]) == false)
                    {
                        btnsave.Enabled = false;
                    }
                    if (Request.QueryString["Prog"] == "PMTCT")
                    {
                        ddlTreatment.SelectedValue = "223";
                        //ddlTreatment.Enabled = true;

                    }
                }
                else if (Request.QueryString["name"] == "Delete")
                {
                    btnsave.Text = "Delete";
                    if (Authentiaction.HasFunctionRight(ApplicationAccess.AdultPharmacy, FunctionAccess.View, (DataTable)Session["UserRight"]) == false)
                    {
                        int PatientID = Convert.ToInt32(Session["PatientId"]);
                        string theUrl = "";
                        theUrl = string.Format("{0}", "../ClinicalForms/frmClinical_DeleteForm.aspx");
                        Response.Redirect(theUrl);
                    }
                    else if (Authentiaction.HasFunctionRight(ApplicationAccess.AdultPharmacy, FunctionAccess.Delete, (DataTable)Session["UserRight"]) == false)
                    {
                        btnsave.Text = "Delete";
                        btnsave.Enabled = false;
                        // btnQualityCheck.Visible = false;
                    }
                }


                #endregion

                int thePtnID = 0;

                if ((Convert.ToInt32(Session["PatientVisitId"]) != 0) || (Request.QueryString["name"] == "Delete"))
                {
                    Int32 PID = Convert.ToInt32(Session["PatientId"].ToString());
                    FillOldCustomData(PID);
                }

                //ie the ord date when the form was loaded the first time
                if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
                {
                    Session["OrigOrdDate"] = theExistDS.Tables[0].Rows[0]["OrderedByDate"].ToString();
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
                {
                    Session["OrigOrdDate"] = null;
                    Session["OrigDispensbyDate"] = null;
                }

                thePtnID = Convert.ToInt32(Session["PatientId"]);
                Session["PtnID"] = thePtnID;
                Session["UserID"] = Session["AppUserId"].ToString();
                if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                    Session["LocationId"] = Convert.ToInt32(Session["AppLocationId"]);
                else
                    Session["LocationId"] = Convert.ToInt32(Session["ServiceLocationId"]);

                Session["SelectedDrug"] = theDrugTable;
                Session["MasterData"] = theDS;

                if (theDS.Tables[8].Rows.Count > 0)
                {
                    DataView theEnollDV = new DataView(theDS.Tables[8]);
                    theEnollDV.RowFilter = "ModuleId=" + Session["TechnicalAreaId"].ToString();
                    if (theEnollDV.Count > 0)
                        Session["EnrolmentDate"] = theEnollDV[0]["StartDate"];
                }

                if (Request.QueryString["Prog"] == "ART")
                {
                    Session["Status"] = Session["HIVPatientStatus"];
                }
                else
                {
                    Session["Status"] = Session["PMTCTPatientStatus"];
                }



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
                        if (dr["GenericId"].ToString() != "1")
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
                    Session["AddTB"] = TBDrugs;
                    Session["OIDrugs"] = OIDrugs;
                    ViewState["PharmacyId"] = Session["PatientVisitId"].ToString();
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
                //if (Convert.ToInt32(ViewState["PatientVisitId"]) > 0 && Convert.ToInt32(ViewState["PatientVisitId"]) != 0)
                //{
                #region "Disabled Region"

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
                    //LoadAdditionalDrugs(theDT, PnlAddARV);
                    LoadAdditionalDrugs(theDT, 1, PnlDrug);
                }
                #endregion
                #region "TB Drugs"
                if ((DataTable)Application["AddTB"] != null)
                {
                    Session["AddTB"] = (DataTable)Application["AddTB"];
                    Session["MasterDrugTable"] = (DataTable)Application["MasterData"];
                    Application.Remove("MasterData");
                    Application.Remove("AddTB");
                }
                if ((DataTable)Session["AddTB"] != null)
                {
                    DataTable theDT = (DataTable)Session["AddTB"];
                    //divAddARV.Visible = false;
                    LoadAdditionalDrugs(theDT, 1, pnlOtherTBMedicaton);
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
                    LoadAdditionalDrugs(theDT, 1, PnlOtherMedication);
                }
                #endregion
                #region "Additional OI"
                if ((DataTable)Application["OIDrugs"] != null)
                {
                    Session["OIDrugs"] = (DataTable)Application["OIDrugs"];
                    Session["MasterDrugTable"] = (DataTable)Application["MasterData"];
                    Application.Remove("OIDrugs");
                    Application.Remove("MasterData");
                }
                if ((DataTable)Session["OIDrugs"] != null)
                {
                    DataTable theDT = (DataTable)Session["OIDrugs"];
                    //divAddOI.Visible = false;
                    LoadAdditionalDrugs(theDT, 1, PnlOIARV);
                }
                #endregion

                //}

                CreateControls((DataSet)Session["MasterData"]);
                #endregion
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

            Form.EnableViewState = true;

        }
        catch (Exception err)
        {
            MsgBuilder theBuilder = new MsgBuilder();
            theBuilder.DataElements["MessageText"] = err.Message.ToString();
            IQCareMsgBox.Show("#C1", theBuilder, this);
        }
    }

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

            }
        }
        return true;
    }
    private Boolean FieldValidationPaperLess()
    {
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

        //if (ddlPharmReportedbyName.SelectedIndex == 0)//dispance by
        //{
        //    MsgBuilder theMsg = new MsgBuilder();
        //    theMsg.DataElements["Control"] = "Dispensed By";
        //    IQCareMsgBox.Show("BlankDropDown", theMsg, this);
        //    return false;
        //}


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
            theMsg.DataElements["Control"] = "PrescribedByDate";
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

        ////if (txtpharmReportedbyDate.Value.Trim() == "") // dispensed by date
        ////{
        ////    MsgBuilder theMsg = new MsgBuilder();
        ////    theMsg.DataElements["Control"] = "DispensedByDate";
        ////    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
        ////    return false;

        ////}

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
        if (Convert.ToDecimal(Session["Age"]) < 14)
        {
            IQCareMsgBox.Show("PharmacyAdultDetailAge", this);
            return false;
        }

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
            if (DrugTable.Rows[i]["GenericId"] != System.DBNull.Value)
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
            else
            {
                DataView theDV = new DataView(Master.Tables[19]);
                theDV.RowFilter = "Drug_pk = " + DrugTable.Rows[i]["DrugId"] + " and DrugTypeID = 37"; ///DrugAbbreviation = " + theDrgMst.Rows[i][2];    
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
                if ((Convert.ToInt32(Session["PatientVisitId"]) == 0) && (ViewState["PharmacyId"] == null))
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

                            //if (PreRegimeType != theRegimen)
                            //{
                            //    if (ARVTherapy == "95")
                            //    {
                            //        if (dtVisitDate <= Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value)))
                            //        {
                            //            IQCareMsgBox.Show("currentregimenchange", this);
                            //            return false;
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
        }
        return true;
    }

    private Boolean FieldValidation()
    {

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
            theMsg.DataElements["Control"] = "ARV Provider";
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
                if (ddlPharmReportedbyName.SelectedIndex == 0)//dispance by
                {
                    MsgBuilder theMsg = new MsgBuilder();
                    theMsg.DataElements["Control"] = "Dispensed By";
                    IQCareMsgBox.Show("BlankDropDown", theMsg, this);
                    return false;
                }
            }
        }

        if (btnCounsellorSignature.Checked == false)
        {
            if (btnPatientSignature.Checked == false)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Signature ";
                IQCareMsgBox.Show("UncheckedButton", theMsg, this);
                return false;
            }

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
        if (Session["Paperless"].ToString() == "0")
        {
            if (Session["SCMModule"] == null)
            {
                if (txtpharmReportedbyDate.Value.Trim() == "") // dispensed by date
                {
                    MsgBuilder theMsg = new MsgBuilder();
                    theMsg.DataElements["Control"] = "DispensedByDate";
                    IQCareMsgBox.Show("BlankTextBox", theMsg, this);
                    return false;

                }
            }
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
        if (Convert.ToDecimal(Session["Age"]) < 14)
        {
            IQCareMsgBox.Show("PharmacyAdultDetailAge", this);
            return false;
        }

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
    #endregion


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

    private void Init_Form()
    {
        //try
        //{

        //Session["PatientId"] = Session["PatientId"].ToString();
        //Session["PatientId"] = ViewState["PatientVisitId"];

        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        //pr_Pharmacy_GetMasterDetails_Constella
        DataSet theDrugDS = DrugManager.GetPharmacyMasters(Convert.ToInt32(Session["PatientId"]));

        DataSet objPatientStatus = new DataSet();

        #region "FixDoseCombination"

        Session["FixDrugStrength"] = theDrugDS.Tables[17];
        Session["FixDrugFreq"] = theDrugDS.Tables[18];

        theDS = new DataSet();
        theDS.Tables.Add(theDrugDS.Tables[16].Copy());//--1-- rupesh - performance 05-oct-07
        theDS.Tables.Add(theDrugDS.Tables[1].Copy()); //--2--   
        theDS.Tables.Add(theDrugDS.Tables[2].Copy());//--3--
        theDS.Tables.Add(theDrugDS.Tables[3].Copy());//--4--



        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            theDS.Tables.Add(theDrugDS.Tables[4].Copy());//--5--
        else
            theDS.Tables.Add(theDrugDS.Tables[19].Copy());//--1--

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            theDS.Tables.Add(theDrugDS.Tables[5].Copy());//--6--
        else
            theDS.Tables.Add(theDrugDS.Tables[14].Copy());//--6--

        theDS.Tables.Add(theDrugDS.Tables[6].Copy());//--7--
        theDS.Tables.Add(theDrugDS.Tables[7].Copy());//--8--
        theDS.Tables.Add(theDrugDS.Tables[8].Copy());//--9--
        theDS.Tables.Add(theDrugDS.Tables[9].Copy());//--10--
        theDS.Tables.Add(theDrugDS.Tables[10].Copy());////--1-- rupesh -- used only for showing previously used but NOW Inactive Drug

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)  //rupesh - 04-sept -for inactive freq 
            theDS.Tables.Add(theDrugDS.Tables[11].Copy());//--11-- rupesh 03-sep for OI & Other frequency
        else
            theDS.Tables.Add(theDrugDS.Tables[12].Copy());//--11--rupesh - 04-sept -for inactive freq 

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            theDS.Tables.Add(theDrugDS.Tables[13].Copy());//--12-- rupesh 20-sep-07 for active/inactive ARV Provider
        else
            theDS.Tables.Add(theDrugDS.Tables[15].Copy());//--12--rupesh 20-sep-07 for active/inactive ARV Provider

        theDS.Tables.Add(theDrugDS.Tables[20].Copy());//--13-- 29Feb08 -- Non-ARTFollowup date

        theDS.Tables.Add(theDrugDS.Tables[25].Copy());//Period Taken
        theDS.Tables.Add(theDrugDS.Tables[26].Copy());//TB Regimen
        theDS.Tables.Add(theDrugDS.Tables[27].Copy());
        theDS.Tables.Add(theDrugDS.Tables[28].Copy());
        theDS.Tables.Add(theDrugDS.Tables[29].Copy());
        theDS.Tables.Add(theDrugDS.Tables[30].Copy());
        theDS.Tables.Add(theDrugDS.Tables[31].Copy());

        MakeRegimenGenericTable(theDS);

        #endregion

        if (theDS.Tables[3].Rows.Count == 0)
            Response.Redirect("../frmFindAddPatient.aspx");

        //lblPatientName.Text = theDS.Tables[3].Rows[0]["Name"].ToString();
        //lblpatientenrol.Text = theDS.Tables[3].Rows[0]["PatientId"].ToString();
        //lblExisPatientID.Text = theDS.Tables[3].Rows[0]["PatientClinicID"].ToString();
        BindddlControls(theDS);
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            BindControls();
        }

        //BindTetanusVaccine(theDS);
        theDrugTable = MakeTable();
        objPatientStatus = DrugManager.GetPatientRecordformStatus(Convert.ToInt32(Session["PatientId"]));

        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            PnlDrug.Controls.Clear();
            CreateControls(theDS);
        }
        else
        {
            //pr_Pharmacy_GetExistPharmacyDetails
            theExistDS = DrugManager.GetExistPharmacyDetail(Convert.ToInt32(Session["PatientVisitId"]));
            if (theExistDS.Tables.Count == 0)
            {
                IQCareMsgBox.Show("NoPharmacyRecordExists", this);
                return;
            }
            Session["ExistPharmacyData"] = theExistDS.Tables[0];

            if (theExistDS.Tables[0].Rows.Count > 0)
            {
                BindDropdownOrderBy(theExistDS.Tables[0].Rows[0]["OrderedBy"].ToString());
                BindDropdownReportedBy(theExistDS.Tables[0].Rows[0]["DispensedBy"].ToString());
                BindDropdownSignature(theExistDS.Tables[0].Rows[0]["Signature"].ToString());
            }
            PnlDrug.Controls.Clear();
            CreateControls(theDS);
            if (theExistDS.Tables[0].Rows.Count > 0)
            {
                ddlPharmOrderedbyName.SelectedValue = theExistDS.Tables[0].Rows[0]["OrderedBy"].ToString();
                ddlPharmReportedbyName.SelectedValue = theExistDS.Tables[0].Rows[0]["DispensedBy"].ToString();
                txtClinicalNotes.Text = theExistDS.Tables[0].Rows[0]["PharmacyNotes"].ToString();
                ddlTreatment.SelectedValue = theExistDS.Tables[0].Rows[0]["ProgID"].ToString();

                //if (theExistDS.Tables[0].Rows[0]["ProgID"].ToString() == "223")
                //{
                //    // ddlTreatment.Enabled = false;
                //}
                //rupesh 18-sep-07 - for ProviderId
                ddlProvider.SelectedValue = Convert.ToString(theExistDS.Tables[0].Rows[0]["ProviderID"].ToString());
                if (theExistDS.Tables[0].Rows[0]["EmployeeID"] != System.DBNull.Value)
                {
                    if (Convert.ToInt32(theExistDS.Tables[0].Rows[0]["EmployeeID"]) > 0)
                    {
                        string script = "<script language = 'javascript' defer ='defer' id = 'showsignature'>\n";
                        script += "document.getElementById('" + btnCounsellorSignature.ClientID + "').click();\n";
                        script += "</script>\n";
                        RegisterStartupScript("showsignature", script);
                        ddlPharmSignature.SelectedValue = theExistDS.Tables[0].Rows[0]["EmployeeID"].ToString();

                    }
                    else
                    {
                        btnPatientSignature.Checked = true;
                    }
                }

                if (theExistDS.Tables[0].Rows[0]["OrderedByDate"] != System.DBNull.Value)
                {
                    DateTime theOrderedByDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["OrderedByDate"].ToString());
                    txtpharmOrderedbyDate.Value = theOrderedByDate.ToString(Session["AppDateFormat"].ToString());
                }
                if (theExistDS.Tables[0].Rows[0]["DispensedByDate"].ToString() != "")
                {
                    DateTime theReportedbyDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["DispensedByDate"]);
                    txtpharmReportedbyDate.Value = theReportedbyDate.ToString(Session["AppDateFormat"].ToString());
                }
                if (theExistDS.Tables[0].Rows[0]["PharmacyPeriodTaken"].ToString() != "")
                {
                    ddlPeriodTaken.SelectedValue = theExistDS.Tables[0].Rows[0]["PharmacyPeriodTaken"].ToString();
                }
                if (theExistDS.Tables[0].Rows[0]["PharmacyNotes"].ToString() != "")
                {
                    txtClinicalNotes.Text = theExistDS.Tables[0].Rows[0]["PharmacyNotes"].ToString();
                }
            }

            Session["MasterData"] = theDS;

            #region Fill Existing Data

            #region "CreateAdditional Controls"

            #region "TableCreation"
            DataTable theDT = new DataTable();
            theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
            theDT.Columns.Add("DrugName", System.Type.GetType("System.String"));
            theDT.Columns.Add("Generic", System.Type.GetType("System.Int32"));
            //Ajay--08-May-09
            theDT.Columns.Add("DrugTypeId", System.Type.GetType("System.Int32"));
            theDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
            AddARV = theDT.Copy();
            OtherDrugs = theDT.Copy();
            TBDrugs = theDT.Copy();
            OIDrugs = theDT.Copy();
            #endregion

            foreach (DataRow theDR in theExistDS.Tables[0].Rows)
            {
                if (theDR["Drug_Pk"] != System.DBNull.Value)
                {
                    //DataView theDV = new DataView(theDS.Tables[0]); // rupesh
                    DataView theDV = new DataView(theDS.Tables[10]); // rupesh - for inactive drug
                    //theDV.RowFilter = "Drug_Pk = " + theDR["Drug_Pk"].ToString() + " and Drug_Pk not in (65,325,486,85,150)";
                    theDV.RowFilter = "Drug_Pk = " + theDR["Drug_Pk"].ToString();
                    if (theDV.Count > 0)
                    {
                        if (Convert.ToInt32(theDV[0]["DrugTypeId"]) == 37)
                        {
                            DataRow DR = AddARV.NewRow();
                            DR[0] = theDR["Drug_Pk"];
                            DR[1] = theDV[0]["DrugName"];
                            DR[2] = 0;
                            DR[3] = theDV[0]["StrengthId"];
                            AddARV.Rows.Add(DR);
                        }
                        else if (Convert.ToInt32(theDV[0]["DrugTypeId"]) == 31)
                        {
                            DataRow DR = TBDrugs.NewRow();
                            DR[0] = theDR["Drug_Pk"];
                            DR[1] = theDV[0]["DrugName"];
                            DR[2] = 0;
                            DR[3] = theDV[0]["StrengthId"];
                            TBDrugs.Rows.Add(DR);
                        }
                        else if (Convert.ToInt32(theDV[0]["DrugTypeId"]) == 36)
                        {
                            DataRow DR = OIDrugs.NewRow();
                            DR[0] = theDR["Drug_Pk"];
                            DR[1] = theDV[0]["DrugName"];
                            DR[2] = 0;
                            DR[3] = theDV[0]["StrengthId"];
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
                            DR[3] = theDV[0]["StrengthId"];
                            OtherDrugs.Rows.Add(DR);
                            //}
                        }
                    }

                    //}
                }
                //LoadAdditionalDrugs(AddARV, PnlAddARV);
                LoadAdditionalDrugs(AddARV, 1, PnlDrug);
                LoadAdditionalDrugs(OIDrugs, 1, PnlOIARV);
                LoadAdditionalDrugs(OtherDrugs, 1, PnlOtherMedication);
                LoadAdditionalDrugs(TBDrugs, 1, pnlOtherTBMedicaton);
            #endregion

                //divAddARV.Visible = false;
                //divAddOI.Visible = false;
                foreach (DataRow dr in theExistDS.Tables[0].Rows)
                {

                    FillOldData(PnlRegiment, dr);
                    FillOldData(PnlOIARV, dr);
                    //FillOldData(PnlAddARV, dr);
                    FillOldData(PnlDrug, dr);
                    FillOldData(PnlOtherMedication, dr);
                    FillOldData(pnlOtherTBMedicaton, dr);
                    //FillMaternalHealth(dr);
                }

            #endregion

            }

            if (theExistDS.Tables.Count > 0)
                if (theExistDS.Tables[0].Rows.Count > 0)
                {
                    ddlTreatment.SelectedValue = theExistDS.Tables[0].Rows[0]["ProgId"].ToString();
                    if (theExistDS.Tables[0].Rows[0]["ProgId"].ToString() == "225")
                    {
                        Session["TreatmentProg"] = theExistDS.Tables[0].Rows[0]["ProgId"].ToString();

                    }
                    else
                    {
                        Session["TreatmentProg"] = "";
                    }

                }
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
    //private void BindTetanusVaccine(DataSet theDS)
    //{
    //    BindFunctions theBindManager = new BindFunctions();
    //    DataTable theDT;
    //    DataView theDV = new DataView(theDS.Tables[20]);
    //    theDV.RowFilter = "MstDrugName='Tetanus Toxoid'";// and Drug_pk in(506,507,508,509,510)";
    //    theDT = theDV.ToTable();
    //    theBindManager.BindCombo(ddltetanus, theDT, "DrugName", "Drug_pk");
    //}
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
    private void MakeRegimenGenericTable(DataSet theDS)
    {
        DataTable theDT;
        if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
        {
            theDT = theDS.Tables[15];
        }
        else
        {
            theDT = theDS.Tables[15];
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
    //private void FillMaternalHealth(DataRow theDR)
    //{
    //    int DrugId;
    //    if (Convert.ToInt32(theDR["Drug_Pk"]) == 0)
    //    {
    //        DrugId = Convert.ToInt32(theDR["GenericId"]);
    //    }
    //    else
    //    {
    //        DrugId = Convert.ToInt32(theDR["Drug_Pk"]);
    //    }

    //    if (DrugId == 203)
    //    {
    //        chkmultiVitamin.Checked = true;
    //    }
    //    else if (DrugId == 190)
    //    {
    //        chkmebendazole.Checked = true;
    //    }
    //    else if (DrugId == 476)
    //    {
    //        chkiron.Checked = true;
    //    }
    //     else if (DrugId == 531) 
    //    {
    //        ddltetanus.SelectedValue = Convert.ToString(theDR["DrugSchedule"]);
    //    }
    //    else if (DrugId == 138)
    //    {
    //        chkfolicacid.Checked = true;
    //    }
    //    else if (DrugId == 350)
    //    {
    //        chkvitaminA.Checked = true;
    //    }
    //    else if (DrugId == 353)
    //    {
    //        chkvitaminC.Checked = true;
    //    }


    //}
    private void FillOldData(Control Cntrl, DataRow theDR)
    {
        if (theDR["Drug_Pk"] != System.DBNull.Value)
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
                                //((DropDownList)x).SelectedValue = theDR["StrengthId"].ToString();
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
                                if (theDR["Dose"] != System.DBNull.Value)
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
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["Duration"]));
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
                                //int DecValue=Convert.ToInt32(theDR["OrderedQuantity"].ToString().Substring(DecPos+1, 2));
                                if (DecPos != -1)
                                {
                                    decimal DecValue = Convert.ToDecimal(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    //decimal DecValue = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
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
                                    if (theDR["OrderedQuantity"] != System.DBNull.Value)
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
                                if (DecPos != -1)
                                {
                                    decimal DecValue = Convert.ToDecimal(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));

                                    if (DecValue > 0)
                                    {
                                        ((TextBox)x).Text = theDR["DispensedQuantity"].ToString();

                                    }
                                    else
                                    {
                                        if (theDR["DispensedQuantity"].ToString() != "0.00")
                                        {
                                            ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                        }
                                    }
                                }
                                else
                                {
                                    if (theDR["DispensedQuantity"] != System.DBNull.Value)
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                }
                                //((TextBox)x).Text = theDR["DispensedQuantity"].ToString();
                            }
                        }
                    }

                    if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                    {
                        //if (x.ID.StartsWith("FinChk"))
                        //{
                        //    ipos = Convert.ToInt32(x.ID.IndexOf("^"));
                        //    if (ipos > 0)
                        //    {
                        //        y = Convert.ToInt32(x.ID.Substring(6, ipos - 6));
                        //    }
                        //    else
                        //    {
                        //        y = Convert.ToInt32(x.ID.Substring(6, x.ID.Length - 6));
                        //    }
                        //    //y = Convert.ToInt32(x.ID.Substring(6, x.ID.Length - 6));
                        //    if (y == DrugId)
                        //    {
                        //        if (Convert.ToInt32(theDR["Financed"].ToString()) == 1)
                        //        {
                        //            ((CheckBox)x).Checked = true;
                        //        }
                        //        else
                        //        {
                        //            ((CheckBox)x).Checked = false;
                        //        }

                        //    }
                        //}
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

    private void FillOldFixedData(Control Cntrl, DataRow theDR)
    {
        int DrugId = 0;
        if (Convert.ToInt32(theDR["Drug_Pk"]) != 0)
        {
            foreach (DataRow theDRFixDose in theDS.Tables[19].Rows)
            {
                if (Convert.ToInt32(theDRFixDose["drug_pk"]) == Convert.ToInt32(theDR["drug_pk"]))
                {
                    DrugId = Convert.ToInt32(theDR["Drug_Pk"]);



                    foreach (Control x in Cntrl.Controls)
                    {
                        if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            FillOldFixedData(x, theDR);
                        }
                        else
                        {
                            if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                            {
                                if (x.ID == "dddrug")
                                {

                                    ((DropDownList)x).SelectedValue = theDR["StrengthId"].ToString();
                                    if (Convert.ToInt32(theDR["Drug_pk"]) > 0)
                                    {
                                        ((DropDownList)x).SelectedValue = DrugId.ToString();
                                        EventArgs s = new EventArgs();
                                        // ddlFixDrugname_SelectedChanged((DropDownList)x, s);
                                    }


                                }
                                if (x.ID == "dddose")
                                {

                                    ((DropDownList)x).SelectedValue = theDR["StrengthId"].ToString();
                                    if (Convert.ToInt32(theDR["StrengthId"]) > 0)
                                    {
                                        ((DropDownList)x).SelectedValue = theDR["StrengthId"].ToString();
                                    }


                                }

                                if (x.ID == "ddfreq")
                                {
                                    ((DropDownList)x).SelectedValue = theDR["FrequencyId"].ToString();

                                }

                            }
                            if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                            {

                                if (x.ID == "txtDuration")
                                {
                                    int DecPos = theDR["Duration"].ToString().IndexOf(".");
                                    if (DecPos != -1)
                                    {
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
                                    ((TextBox)x).Text = theDR["Duration"].ToString();

                                }
                                if (x.ID == "txtqtyprescribed")
                                {

                                    int DecPos = theDR["OrderedQuantity"].ToString().IndexOf(".");
                                    int DecValue = Convert.ToInt32(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    if (DecPos != -1)
                                    {
                                        // decimal DecValue = Convert.ToDecimal(theDR["OrderedQuantity"].ToString().Substring(DecPos + 1, 2));
                                        // decimal DecValue = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
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

                                }
                                if (x.ID == "txtqtydispensed")
                                {
                                    int DecPos = theDR["DispensedQuantity"].ToString().IndexOf(".");
                                    int DecValue = Convert.ToInt32(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));
                                    if (DecPos != -1)
                                    {
                                        //decimal DecValue = Convert.ToDecimal(theDR["DispensedQuantity"].ToString().Substring(DecPos + 1, 2));

                                        if (DecValue > 0)
                                        {
                                            ((TextBox)x).Text = theDR["DispensedQuantity"].ToString();

                                        }
                                        else
                                        {
                                            if (theDR["DispensedQuantity"].ToString() != "0.00")
                                            {
                                                ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ((TextBox)x).Text = Convert.ToString(Convert.ToInt32(theDR["DispensedQuantity"]));
                                    }


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
                                if (x.ID == "chkProphylaxis")
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
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList) && x.ID == "dddrug")
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

    private DataTable CreateTable()
    {
        DataTable theDT = new DataTable();
        theDT.Columns.Add("DrugId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("GenericId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("TBRegimenId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Dose", System.Type.GetType("System.Decimal"));
        theDT.Columns.Add("UnitId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("StrengthId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("FrequencyId", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Duration", System.Type.GetType("System.Decimal"));
        theDT.Columns.Add("DrugSchedule", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("QtyPrescribed", System.Type.GetType("System.Decimal"));
        theDT.Columns.Add("QtyDispensed", System.Type.GetType("System.Decimal"));
        theDT.Columns.Add("Financed", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("Prophylaxis", System.Type.GetType("System.Int32"));
        theDT.Columns.Add("TreatmentPhase", System.Type.GetType("System.String"));
        theDT.Columns.Add("TrMonth", System.Type.GetType("System.Int32"));

        return theDT;

    }

    private void BindControls()
    {
        IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
        BindFunctions theBindMgr = new BindFunctions();

        IQCareUtils theUtils = new IQCareUtils();
        DataTable theDT = new DataTable();


        DataSet theDSXML = new DataSet();
        theDSXML.ReadXml(Server.MapPath("..\\XMLFiles\\AllMasters.con"));


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
        BindManager.BindCombo(ddlTreatment, theDT, theDT.Columns[1].ColumnName, theDT.Columns[0].ColumnName);

        //ddlTreatment.DataSource = theDT;
        //ddlTreatment.DataTextField = theDT.Columns[1].ColumnName;
        //ddlTreatment.DataValueField = theDT.Columns[0].ColumnName;
        //ddlTreatment.DataBind();
        theDVTreat.Dispose();
        theDT.Clear();

        //rupesh 18-sep-07 - for ARV Provider
        DataView theDVProvider = new DataView(theDS.Tables[12]);
        //theDVProvider.RowFilter = "DeleteFlag=0"; rupesh for inactive / active ARV Provider 20-sep-07
        theDT = (DataTable)theUtils.CreateTableFromDataView(theDVProvider);
        BindManager.BindCombo(ddlProvider, theDT, theDT.Columns[1].ColumnName, theDT.Columns[0].ColumnName);
        //ddlProvider.DataSource = theDT;
        //ddlProvider.DataTextField = theDT.Columns[1].ColumnName;
        //ddlProvider.DataValueField = theDT.Columns[0].ColumnName;
        //ddlProvider.DataBind();
        //ddlProvider.Dispose();
        theDT.Clear();
        //ddlProvider.SelectedIndex = 1;

        //Period Taken
        DataView DVPeriodTaken = new DataView(theDS.Tables[14]);
        DataTable dtPeriodTaken = (DataTable)theUtils.CreateTableFromDataView(DVPeriodTaken);
        BindManager.BindCombo(ddlPeriodTaken, dtPeriodTaken, dtPeriodTaken.Columns[1].ColumnName.ToString(), dtPeriodTaken.Columns[0].ColumnName.ToString());



    }
    #region Create Controls Dynamically

    private void CreateControls(DataSet theCntlDS)
    {
        Control theRegLineCntrl = FindControlRecursive(PnlRegiment, "pnlRegimen");
        if (theRegLineCntrl == null)
        {
            System.Web.UI.WebControls.Panel thePnl = new System.Web.UI.WebControls.Panel();
            thePnl.ID = "pnlRegimen";
            string strBrowser = Request.Browser.Browser;
            if (strBrowser == "IE")
            {
                thePnl.Height = 20;
            }
            else
            {
                thePnl.Height = 30;
            }
            thePnl.Width = 800;
            thePnl.Controls.Clear();
            System.Web.UI.WebControls.Label Space = new System.Web.UI.WebControls.Label();
            System.Web.UI.WebControls.Label DDSpace = new System.Web.UI.WebControls.Label();
            Space.Width = 10;
            DDSpace.Width = 10;
            System.Web.UI.WebControls.Label theHeadingRegimenLine = new System.Web.UI.WebControls.Label();
            theHeadingRegimenLine.Text = "*Regimen Line";
            theHeadingRegimenLine.CssClass = "required";
            theHeadingRegimenLine.Font.Bold = true;
            System.Web.UI.WebControls.DropDownList theRegimenLine = new System.Web.UI.WebControls.DropDownList();
            theRegimenLine.ID = "DDRegimenLine";
            DataView theDV = new DataView(theCntlDS.Tables[17]);
            BindFunctions BindManager = new BindFunctions();
            theDV.RowFilter = "DeleteFlag='0'";
            if (theDV.Table != null)
            {
                //theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                BindManager.BindCombo(theRegimenLine, theDV.Table, "Name", "ID");
            }

            if (theCntlDS.Tables[18].Rows[0]["RegimenLine"] != System.DBNull.Value)
            {
                theRegimenLine.SelectedValue = Convert.ToString(theCntlDS.Tables[18].Rows[0]["RegimenLine"]);
            }
            thePnl.Controls.Add(Space);
            thePnl.Controls.Add(theHeadingRegimenLine);
            thePnl.Controls.Add(DDSpace);
            thePnl.Controls.Add(theRegimenLine);
            ///////////////////////////////
            PnlRegiment.Controls.Add(Space);
            PnlRegiment.Controls.Add(theHeadingRegimenLine);
            PnlRegiment.Controls.Add(DDSpace);
            PnlRegiment.Controls.Add(theRegimenLine);
            //////////////////////////////
            #region CreateTable"

            //if (Session["SelectedDrug"] != null)
            //{
            //    theDrugTable = (DataTable)Session["SelectedDrug"];
            //}




            //int i = 0;
            //int theGenericId = 0;
            //foreach (DataRow dr in theDrugTable.Rows)
            //{
            //    theGenericId = 0;
            //    if (Convert.ToInt32(dr["GenericId"]) > 0)
            //    {
            //        theGenericId = Convert.ToInt32(dr["GenericId"].ToString());
            //    }
            //    else
            //    {

            //        theGenericId = Convert.ToInt32(dr["DrugId"].ToString());
            //    }




            //    if (theGenericId > 10000)
            //    {
            //        Label theHeading = new Label();
            //        if (Convert.ToInt32(dr["GenericId"]) > 0)
            //        {
            //            theHeading.Text = dr["GenericName"].ToString();
            //            theHeading.ID = "lbl" + dr["GenericName"].ToString();
            //        }
            //        else
            //        {
            //            theHeading.Text = dr["DrugName"].ToString();
            //            theHeading.ID = "lbl" + dr["DrugName"].ToString();
            //        }
            //        theHeading.Font.Bold = true;
            //        thePnl.Controls.Add(theHeading);


            //        if (theHeading.Text == "NRTI")
            //        {


            //        }

            //    }

            //}
            #endregion
        }
    }




    void Control_Load(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;
        tbox.MaxLength = 3;
        #region "13-Jun-07 - 1"
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
        #endregion
    }
    void DecimalText_Load(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;
        tbox.MaxLength = 4;
        tbox.Attributes.Add("onkeyup", "chkDecimal('" + tbox.ClientID + "')");
    }

    void Dose_Load(object sender, EventArgs e)
    {
        TextBox tbox = (TextBox)sender;
        tbox.MaxLength = 8;
        Int32 ipos = Convert.ToInt32(tbox.ID.IndexOf("^"));
        int DrugId = Convert.ToInt32(tbox.ID.Substring(7, ipos - 7));
        DataView theDV = new DataView(((DataSet)Session["MasterData"]).Tables[7]);
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
    private bool ChkPanel(int DrugId, Panel MstPanel)
    {

        foreach (Control c in MstPanel.Controls)
        {
            if (c is Panel)
            {
                if (c.ID == "pnl_" + DrugId)
                {
                    return true;
                }
                // do something with textbox
            }

        }
        return false;

    }

    private void BindCustomControls(int DrugId, int Generic, Panel MstPanel)
    {
        //foreach (DataRow theDRFixDose in theDS.Tables[19].Rows)
        //{
        //    if (DrugId != Convert.ToInt32(theDRFixDose[0]))
        //    {

        //if (!ChkPanel(DrugId, MstPanel))
        //{
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
            DataSet theDS_Custom = (DataSet)Session["MasterData"];
            theDV = new DataView(theDS_Custom.Tables[10]);
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

            /////// Space//////
            Label theSpace28 = new Label();
            theSpace28.ID = "theSpace28*" + DrugId + "^" + Generic;
            theSpace28.Width = 10;
            theSpace28.Text = "";
            thePnl.Controls.Add(theSpace28);

            BindFunctions theBindMgr = new BindFunctions();

            ////////////////////////////////////////

            DropDownList theDrugFrequency = new DropDownList();
            theDrugFrequency.ID = "drgFrequency" + DrugId;
            theDrugFrequency.Width = 80;
            #region "BindCombo"

            DataTable theDTFrequency = ((DataSet)Session["MasterData"]).Tables[6];
            if (theDTFrequency.Rows.Count > 0)
            {
                IQCareUtils theUtils = new IQCareUtils();
                //theDTFrequency = theUtils.CreateTableFromDataView(theDVFrequency);
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
            lblSpace5.ID = "lblSpace_56";
            lblSpace5.Text = "";
            theheaderPnl.Controls.Add(lblSpace5);

            Label lblProphylaxis = new Label();
            lblProphylaxis.Text = "Prophylaxis";
            lblProphylaxis.ID = "lblProphylaxis" + MstPanel.ID;
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
            DataSet theDS = (DataSet)Session["MasterData"];
            if (Generic == 0)
            {
                //theDV = new DataView(theDS.Tables[0]);
                theDV = new DataView(theDS.Tables[10]); // rupesh for both active & inactive drug 31/07/07
                theDV.RowFilter = "drug_pk = " + DrugId;
            }
            else
            {
                //theDV = new DataView(theDS.Tables[4]);
                //theDV.RowFilter = "GenericId = " + DrugId;
                theDV = new DataView(theDS.Tables[4]);
                if (DrugId.ToString().LastIndexOf("9999") > 0)
                {

                    DrugId = Convert.ToInt32(DrugId.ToString().Substring(0, DrugId.ToString().Length - 4));
                }
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
            DTFreq = ((DataSet)Session["MasterData"]).Tables[6];
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
           ;
            thePnl.Controls.Add(theQtyDispensed);

            ////////////Space////////////////////////
            Label theSpace6 = new Label();
            theSpace6.ID = "theSpace6*" + DrugId + "^" + Generic;
            theSpace6.Width = 30;
            theSpace6.Text = "";
            thePnl.Controls.Add(theSpace6);
            //////////////////////////////////////////
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

            /////////Space panel/////////////////////////
            Panel thePnlspace = new Panel();
            thePnlspace.ID = "pnlspace_" + DrugId;
            thePnlspace.Height = 3;
            thePnlspace.Width = 900;
            thePnlspace.Controls.Clear();
            MstPanel.Controls.Add(thePnlspace);
        }

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
            if (pnlName.ToString() == "PnlDrug")
            {
                if ((DataTable)Session["AddARV"] != null)
                {
                    DataTable theDT = (DataTable)Session["AddARV"];
                    DataRow[] theDR = theDT.Select("DrugId = " + Drugid.ToString());

                    theDT.Rows.Remove(theDR[0]);
                    Session["AddARV"] = theDT;
                    pnlremoving = PnlDrug.FindControl("pnl_" + Drugid);
                    if (pnlremoving != null)
                    {
                        if (pnlremoving.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            PnlDrug.Controls.Remove(pnlremoving);
                        }
                    }
                    if (theDR[0] != null && theDT.Rows.Count == 0)
                    {
                        pnlheading = PnlDrug.FindControl("pnlARVDrug");
                        PnlDrug.Controls.Remove(pnlheading);
                        pnlheading = PnlDrug.FindControl("Header");
                        PnlDrug.Controls.Remove(pnlheading);
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
            if (pnlName.ToString() == "PnlOtherMedication")
            {
                if ((DataTable)Session["OtherDrugs"] != null)
                {
                    DataTable theDT = (DataTable)Session["OtherDrugs"];
                    DataRow[] theDR = theDT.Select("DrugId = " + Drugid.ToString());

                    theDT.Rows.Remove(theDR[0]);
                    Session["OtherDrugs"] = theDT;
                    pnlremoving = PnlOtherMedication.FindControl("pnl" + Drugid + "^0");
                    if (pnlremoving != null)
                    {
                        if (pnlremoving.GetType() == typeof(System.Web.UI.WebControls.Panel))
                        {
                            PnlOtherMedication.Controls.Remove(pnlremoving);
                        }
                    }
                    if (theDR[0] != null && theDT.Rows.Count == 0)
                    {
                        pnlheading = PnlOtherMedication.FindControl("pnlDrugPnlOtherMedication");
                        PnlOtherMedication.Controls.Remove(pnlheading);
                        pnlheading = PnlOtherMedication.FindControl("pnlHeaderOtherDrugPnlOtherMedication");
                        PnlOtherMedication.Controls.Remove(pnlheading);
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
    private void BindTBDrugControls(int DrugId, int Generic, Panel MstPanel)
    {
        //TB medications
        //if (MstPanel.Controls.Count < 1)
        //{
        Control thehdrCntrl = FindControlRecursive(MstPanel, "pnlTBDrug" + MstPanel.ID);
        if (thehdrCntrl == null)
        {
            #region "OI & Other Medication"
            Panel thelblPnl = new Panel();
            thelblPnl.ID = "pnlTBDrug" + MstPanel.ID;
            thelblPnl.Height = 20;
            thelblPnl.Width = 860;
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
            DataSet theDS = (DataSet)Session["MasterData"];
            if (Generic == 0)
            {
                //theDV = new DataView(theDS.Tables[0]);
                theDV = new DataView(theDS.Tables[10]); // rupesh for both active & inactive drug 31/07/07
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
            DTFreq = ((DataSet)Session["MasterData"]).Tables[6];
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
            Label theSpace10 = new Label();
            theSpace10.ID = "theSpace10" + DrugId;
            theSpace10.Width = 10;
            theSpace10.Text = "";
            thePnl.Controls.Add(theSpace10);

            //if (ddlTreatment.SelectedItem.Value.ToString() == "223")
            //{
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
            Label theSpace7 = new Label();
            theSpace7.ID = "theSpace7" + DrugId;
            theSpace7.Width = 30;
            theSpace7.Text = "";
            thePnl.Controls.Add(theSpace7);

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
    #endregion

    private void LoadAdditionalDrugs(DataTable theDT, int flag, Panel thePanel)
    {
        //thePanel.Controls.Clear();
        if (theDT.Rows.Count > 0)
        {
            if (thePanel.ID == "PnlDrug")
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
            else if (thePanel.ID == "PnlOtherMedication")
            {
                pnlOther.Visible = true;

            }
        }
        #region "ARV Drugs Heading"
        //if (thePanel.ID == "PnlDrug" && theDT.Rows.Count>0)
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

        //        Control hdrpnl=FindControlRecursive(PnlDrug,"Header");
        //        if (hdrpnl == null)
        //        {
        //            Panel theHeaderPanel = new Panel();
        //            theHeaderPanel.ID = "Header";
        //            theHeaderPanel.Width = 900;
        //            theHeaderPanel.Height = 5;

        //            Label lblSp = new Label();
        //            lblSp.Width = 10;
        //            lblSp.ID = "stSpace";
        //            lblSp.Text = "";
        //            theHeaderPanel.Controls.Add(lblSp);
        //            theHeaderPanel.Controls.Add(lblSpace);
        //            theHeaderPanel.Controls.Add(lblDrugName);
        //            theHeaderPanel.Controls.Add(lblSpace);
        //            //theHeaderPanel.Controls.Add(lblStrength);
        //            //theHeaderPanel.Controls.Add(lblSpace1);
        //            theHeaderPanel.Controls.Add(lblFrequency);
        //            theHeaderPanel.Controls.Add(lblSpace2);
        //            theHeaderPanel.Controls.Add(lblDuration);
        //            theHeaderPanel.Controls.Add(lblSpace3);
        //            theHeaderPanel.Controls.Add(lblQtyPrescribed);
        //            theHeaderPanel.Controls.Add(lblSpace4);
        //            theHeaderPanel.Controls.Add(lblQtyDispensed);
        //            theHeaderPanel.Controls.Add(lblSpace5);
        //            theHeaderPanel.Controls.Add(lblProphylaxis);
        //            theHeaderPanel.Controls.Add(lblSpace6);
        //            PnlDrug.Controls.Add(theHeaderPanel);
        //        }
        //    }
        //}
        #endregion
        foreach (DataRow theDR in theDT.Rows)
        {
            if (thePanel.ID == "PnlDrug")
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

    /***************** Find Control in Container *****************/
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
    private void VisibleLnkControls(Control parent, bool checkedVal)
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
                            VisibleLnkControls(x, checkedVal);
                        }
                        else if (x.GetType().ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                        {

                            LinkButton lnk = (LinkButton)x;
                            if (lnk.Visible == true)
                                lnk.Visible = checkedVal;
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
            theUrl = string.Format("{0}", "../ClinicalForms/frmPatient_Home.aspx");
            Response.Redirect(theUrl);
        }

    }


    # region "Amitava"

    private void PutCustomControl()
    {
        ICustomFields CustomFields;
        CustomFieldClinical theCustomField = new CustomFieldClinical();
        try
        {

            CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields,BusinessProcess.Administration");
            DataSet theDS = CustomFields.GetCustomFieldListforAForm(Convert.ToInt32(ApplicationAccess.AdultPharmacy));
            if (theDS.Tables[0].Rows.Count != 0)
            {
                theCustomField.CreateCustomControlsForms(pnlCustomList, theDS, "APharm");
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
        //    DataSet theDS = CustomFields.GetCustomFieldListforAForm(Convert.ToInt32(ApplicationAccess.AdultPharmacy));
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
        //                customLabel.Text = customLabel.Text.Replace("APharm", "");
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
        //                    if (Convert.ToInt32(ViewState["PatientVisitId"]) == 0)
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
        //                theMultiSelectlbl.Text = theMultiSelectlbl.Text.Replace("APharm", "");
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
        //                    if (Convert.ToInt32(ViewState["PatientVisitId"]) == 0)
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

        //            theCustomField.CreateCustomControls(pnlCustomList, pnlName, ref sbParameter, dr, ref TableName, "APharm", ii);

        //            ii++;
        //        }
        //    }
        //    Session["ControlCreated"] = "CC";
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
    //private void CreateCustomControls()
    //{
    //    ICustomFields CustomFields;
    //    string pnlName = "PnlCustomList";
    //    BindFunctions theBindMgr = new BindFunctions();
    //    TableName = string.Empty;
    //    Int32 ii = 0;
    //    try
    //    {

    //        CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields,BusinessProcess.Administration");
    //        DataSet theDS = CustomFields.GetCustomFieldListforAForm(Convert.ToInt32(ApplicationAccess.AdultPharmacy));
    //        if (theDS != null && theDS.Tables[0].Rows.Count > 0)
    //        {
    //            sbParameter = new StringBuilder();
    //            TableName = theDS.Tables[0].Rows[0]["FeatureName"].ToString().Replace(" ", "_");
    //            //sbParameter.Append(TableName+",");
    //            pnlCustomList.Visible = true;
    //            pnlCustomList.Controls.Clear();
    //            arl = new ArrayList();
    //            pnlCustomList.Controls.Add(new LiteralControl("<TABLE border='1' cellpadding=6 cellspacing=0 width=100%>"));
    //            foreach (DataRow dr in theDS.Tables[0].Rows)
    //            {

    //                //single line Text Box
    //                if (dr[1].ToString() == "1")
    //                {
    //                    if (ii % 2 == 0)
    //                        pnlCustomList.Controls.Add(new LiteralControl("<TR >"));

    //                    pnlCustomList.Controls.Add(new LiteralControl("<TD >"));

    //                    Label theSinglelbl = new Label();
    //                    theSinglelbl.ID = pnlName + "lbl" + ii.ToString();
    //                    theSinglelbl.Text = dr[0].ToString().Replace("_", " ");
    //                    theSinglelbl.Text = theSinglelbl.Text.Replace("APharm", "");
    //                    sbParameter.Append(",[" + dr[0].ToString() + "]");
    //                    //theSinglelbl.Load += new EventHandler(theSinglelbl_Load);
    //                    theSinglelbl.Width = 200;
    //                    theSinglelbl.CssClass = "labelright";
    //                    theSinglelbl.Font.Bold = true;

    //                    pnlCustomList.Controls.Add(theSinglelbl);

    //                    pnlCustomList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

    //                    TextBox theSingleText = new TextBox();
    //                    theSingleText.ID = pnlName + "Txt" + dr[0].ToString();
    //                    theSingleText.Width = 200;
    //                    theSingleText.MaxLength = 80;
    //                    pnlCustomList.Controls.Add(theSingleText);
    //                    pnlCustomList.Controls.Add(new LiteralControl("</TD>"));
    //                    if (ii % 2 != 0)
    //                    {
    //                        pnlCustomList.Controls.Add(new LiteralControl("</TR>"));

    //                    }


    //                }
    //                //Multi line Text Box
    //                else if (dr[1].ToString() == "8")
    //                {
    //                    if (ii % 2 == 0)
    //                        pnlCustomList.Controls.Add(new LiteralControl("<TR>"));

    //                    pnlCustomList.Controls.Add(new LiteralControl("<TD align='left'>"));

    //                    Label themultilbl = new Label();
    //                    themultilbl.ID = pnlName + "lbl" + ii.ToString();
    //                    themultilbl.Text = dr[0].ToString().Replace("_", " ");
    //                    themultilbl.Text = themultilbl.Text.Replace("APharm", "");
    //                    sbParameter.Append(",[" + dr[0].ToString() + "]");
    //                    themultilbl.Width = 200;
    //                    themultilbl.CssClass = "labelright";
    //                    themultilbl.Font.Bold = true;
    //                    pnlCustomList.Controls.Add(themultilbl);

    //                    pnlCustomList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

    //                    TextBox theMultiText = new TextBox();
    //                    theMultiText.ID = pnlName + "Txt" + dr[0].ToString();
    //                    theMultiText.Width = 200;
    //                    theMultiText.TextMode = TextBoxMode.MultiLine;
    //                    theMultiText.MaxLength = 200;
    //                    pnlCustomList.Controls.Add(theMultiText);
    //                    pnlCustomList.Controls.Add(new LiteralControl("</TD>"));
    //                    if (ii % 2 != 0)
    //                    {
    //                        pnlCustomList.Controls.Add(new LiteralControl("</TR>"));

    //                    }
    //                }
    //                //Date Picker
    //                else if (dr[1].ToString() == "5")
    //                {
    //                    if (ii % 2 == 0)
    //                        pnlCustomList.Controls.Add(new LiteralControl("<TR>"));

    //                    pnlCustomList.Controls.Add(new LiteralControl("<TD align='left'>"));

    //                    Label thedatelbl = new Label();
    //                    thedatelbl.ID = pnlName + "lbl" + ii.ToString();
    //                    thedatelbl.Text = dr[0].ToString().Replace("_", " ");
    //                    thedatelbl.Text = thedatelbl.Text.Replace("APharm", "");
    //                    sbParameter.Append(",[" + dr[0].ToString() + "]");
    //                    thedatelbl.Width = 200;
    //                    thedatelbl.CssClass = "labelright";
    //                    thedatelbl.Font.Bold = true;
    //                    pnlCustomList.Controls.Add(thedatelbl);

    //                    pnlCustomList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

    //                    TextBox theDateText = new TextBox();
    //                    theDateText.ID = pnlName + "Dt" + dr[0].ToString();
    //                    Control ctl = (TextBox)theDateText;

    //                    theDateText.Width = 70;
    //                    theDateText.MaxLength = 11;
    //                    pnlCustomList.Controls.Add(theDateText);
    //                    theDateText.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
    //                    theDateText.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");

    //                    pnlCustomList.Controls.Add(new LiteralControl("&nbsp;"));


    //                    Image theDateImage = new Image();
    //                    theDateImage.ID = pnlName + "img" + ii.ToString();
    //                    theDateImage.Height = 22;
    //                    theDateImage.Width = 22;
    //                    theDateImage.ToolTip = "Date Helper";

    //                    theDateImage.ImageUrl = "~/images/cal_icon.gif";
    //                    pnlCustomList.Controls.Add(theDateImage);

    //                    theDateImage.Attributes.Add("onClick", "w_displayDatePicker('" + ((TextBox)ctl).ClientID + "');");

    //                    Label theformatlbl = new Label();
    //                    theformatlbl.ID = pnlName + "lblfmt" + ii.ToString();
    //                    theformatlbl.Text = " (DD-MMM-YYYY)";
    //                    pnlCustomList.Controls.Add(theformatlbl);


    //                    pnlCustomList.Controls.Add(new LiteralControl("</TD>"));
    //                    if (ii % 2 != 0)
    //                    {
    //                        pnlCustomList.Controls.Add(new LiteralControl("</TR>"));

    //                    }

    //                }
    //                //Numeric Field 
    //                else if (dr[1].ToString() == "3")
    //                {
    //                    if (ii % 2 == 0)
    //                        pnlCustomList.Controls.Add(new LiteralControl("<TR>"));

    //                    pnlCustomList.Controls.Add(new LiteralControl("<TD align='left'>"));

    //                    Label thenumberlbl = new Label();
    //                    thenumberlbl.ID = pnlName + "lbl" + ii.ToString();
    //                    thenumberlbl.Text = dr[0].ToString().Replace("_", " ");
    //                    thenumberlbl.Text = thenumberlbl.Text.Replace("APharm", "");
    //                    sbParameter.Append(",[" + dr[0].ToString() + "]");
    //                    thenumberlbl.Width = 200;
    //                    thenumberlbl.CssClass = "labelright";
    //                    thenumberlbl.Font.Bold = true;
    //                    pnlCustomList.Controls.Add(thenumberlbl);

    //                    pnlCustomList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

    //                    TextBox theNumberText = new TextBox();
    //                    theNumberText.ID = pnlName + "Num" + dr[0].ToString();
    //                    theNumberText.Width = 100;
    //                    theNumberText.MaxLength = 9;
    //                    Control ctl = (TextBox)theNumberText;
    //                    pnlCustomList.Controls.Add(theNumberText);
    //                    theNumberText.Attributes.Add("onkeyup", "chkInteger('" + ((TextBox)ctl).ClientID + "')");

    //                    pnlCustomList.Controls.Add(new LiteralControl("</TD>"));
    //                    if (ii % 2 != 0)
    //                    {
    //                        pnlCustomList.Controls.Add(new LiteralControl("</TR>"));

    //                    }
    //                }
    //                //Radio Button 
    //                else if (dr[1].ToString() == "6")
    //                {
    //                    if (ii % 2 == 0)
    //                        pnlCustomList.Controls.Add(new LiteralControl("<TR>"));

    //                    pnlCustomList.Controls.Add(new LiteralControl("<TD align='left' nowrap='noWrap' >"));

    //                    Label theYesNolbl = new Label();
    //                    theYesNolbl.ID = pnlName + "lbl" + ii.ToString();
    //                    theYesNolbl.Text = dr[0].ToString().Replace("_", " ");
    //                    theYesNolbl.Text = theYesNolbl.Text.Replace("APharm", "");
    //                    sbParameter.Append(",[" + dr[0].ToString() + "]");
    //                    theYesNolbl.Width = 200;
    //                    theYesNolbl.CssClass = "labelright";
    //                    theYesNolbl.Font.Bold = true;
    //                    pnlCustomList.Controls.Add(theYesNolbl);

    //                    pnlCustomList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

    //                    RadioButton theYesNoRadio1 = new RadioButton();
    //                    theYesNoRadio1.ID = pnlName + "Radio1" + dr[0].ToString();
    //                    theYesNoRadio1.Width = 1;
    //                    theYesNoRadio1.Text = "Yes";
    //                    theYesNoRadio1.GroupName = "Radio" + ii.ToString();
    //                    pnlCustomList.Controls.Add(theYesNoRadio1);


    //                    RadioButton theYesNoRadio2 = new RadioButton();
    //                    theYesNoRadio2.ID = pnlName + "Radio2" + dr[0].ToString();
    //                    theYesNoRadio2.Width = 1;
    //                    theYesNoRadio2.Text = "No";
    //                    theYesNoRadio2.GroupName = "Radio" + ii.ToString();
    //                    pnlCustomList.Controls.Add(theYesNoRadio2);


    //                    pnlCustomList.Controls.Add(new LiteralControl("</TD>"));
    //                    if (ii % 2 != 0)
    //                    {
    //                        pnlCustomList.Controls.Add(new LiteralControl("</TR>"));

    //                    }
    //                }
    //                //Select List
    //                else if (dr[1].ToString() == "4")
    //                {
    //                    if (ii % 2 == 0)
    //                        pnlCustomList.Controls.Add(new LiteralControl("<TR>"));

    //                    pnlCustomList.Controls.Add(new LiteralControl("<TD align='left'>"));
    //                    Label theSelectlbl = new Label();
    //                    theSelectlbl.ID = pnlName + "lbl" + ii.ToString();
    //                    theSelectlbl.Text = dr[0].ToString().Replace("_", " ");
    //                    theSelectlbl.Text = theSelectlbl.Text.Replace("APharm", "");
    //                    sbParameter.Append(",[" + dr[0].ToString() + "]");
    //                    theSelectlbl.Width = 200;
    //                    theSelectlbl.CssClass = "labelright";
    //                    theSelectlbl.Font.Bold = true;
    //                    pnlCustomList.Controls.Add(theSelectlbl);


    //                    pnlCustomList.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

    //                    DropDownList ddlSelectList = new DropDownList();
    //                    ddlSelectList.ID = pnlName + "Selectlist" + dr[0].ToString();
    //                    ddlSelectList.Width = 100;

    //                    DataSet dsSelectList = CustomFields.GetCustomList(Convert.ToInt32(dr[2].ToString()));
    //                    if (dsSelectList != null && dsSelectList.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (Convert.ToInt32(ViewState["PatientVisitId"]) == 0)
    //                        {
    //                            IQCareUtils theUtilsCF = new IQCareUtils();
    //                            DataView theDVCF = new DataView(dsSelectList.Tables[0]);
    //                            theDVCF.RowFilter = "DeleteFlag=0";
    //                            DataTable theDTCF = (DataTable)theUtilsCF.CreateTableFromDataView(theDVCF);
    //                            /*******/
    //                            theBindMgr.BindCombo(ddlSelectList, theDTCF, "Name", "ID");
    //                            theDVCF.Dispose();
    //                            theDTCF.Clear();
    //                        }
    //                        else 
    //                        {
    //                            theBindMgr.BindCombo(ddlSelectList, dsSelectList.Tables[0], "Name", "Id");
    //                        }

    //                    }

    //                    pnlCustomList.Controls.Add(ddlSelectList);
    //                    pnlCustomList.Controls.Add(new LiteralControl("</TD>"));
    //                    if (ii % 2 != 0)
    //                    {
    //                        pnlCustomList.Controls.Add(new LiteralControl("</TR>"));
    //                    }
    //                }
    //                //Multi Select List
    //                else if (dr[1].ToString() == "9")
    //                {
    //                    if (ii % 2 == 0)
    //                        pnlCustomList.Controls.Add(new LiteralControl("<TR>"));

    //                    pnlCustomList.Controls.Add(new LiteralControl("<TD align='left'>"));
    //                    if (arl.Count == 0)
    //                    {
    //                        arl.Add("dtl_CustomField_" + TableName.Replace("-", "_").ToString() + "_" + dr[0].ToString());
    //                    }
    //                    foreach (object obj in arl)
    //                    {
    //                        if (obj.ToString() != "dtl_CustomField_" + TableName.Replace("-", "_").ToString() + "_" + dr[0].ToString())
    //                        {
    //                            arl.Add("dtl_CustomField_" + TableName.Replace("-", "_").ToString() + "_" + dr[0].ToString());
    //                            break;
    //                        }
    //                    }
    //                    Label theMultiSelectlbl = new Label();
    //                    theMultiSelectlbl.ID = pnlName + "lbl" + ii.ToString();
    //                    theMultiSelectlbl.Text = dr[0].ToString().Replace("_", " ");
    //                    theMultiSelectlbl.Text = theMultiSelectlbl.Text.Replace("APharm", "");

    //                    theMultiSelectlbl.Width = 200;
    //                    theMultiSelectlbl.CssClass = "labelright";
    //                    theMultiSelectlbl.Font.Bold = true;
    //                    pnlCustomList.Controls.Add(theMultiSelectlbl);

    //                    pnlCustomList.Controls.Add(new LiteralControl("<div class = 'Customdivborder' nowrap='nowrap'>"));

    //                    CheckBoxList chkMultiList = new CheckBoxList();
    //                    chkMultiList.ID = pnlName + "Multiselectlist" + dr[0].ToString();
    //                    chkMultiList.RepeatLayout = RepeatLayout.Flow;
    //                    chkMultiList.CssClass = "check";
    //                    chkMultiList.Width = 300;

    //                    DataSet dsMultiSelectList = CustomFields.GetCustomList(Convert.ToInt32(dr[2].ToString()));
    //                    if (dsMultiSelectList != null && dsMultiSelectList.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (Convert.ToInt32(ViewState["PatientVisitId"]) == 0)
    //                        {
    //                            IQCareUtils theUtilsCF = new IQCareUtils();
    //                            DataView theDVCF = new DataView(dsMultiSelectList.Tables[0]);
    //                            theDVCF.RowFilter = "DeleteFlag=0";
    //                            DataTable theDTCF = (DataTable)theUtilsCF.CreateTableFromDataView(theDVCF);
    //                            /*******/
    //                            theBindMgr.BindCheckedList(chkMultiList, theDTCF, "Name", "Id");

    //                            theDVCF.Dispose();
    //                            theDTCF.Clear();
    //                        }
    //                        else 
    //                        {
    //                            theBindMgr.BindCheckedList(chkMultiList, dsMultiSelectList.Tables[0], "Name", "Id");
    //                        }
    //                    }

    //                    pnlCustomList.Controls.Add(chkMultiList);

    //                    pnlCustomList.Controls.Add(new LiteralControl("</div>"));
    //                    pnlCustomList.Controls.Add(new LiteralControl("</TD>"));
    //                    if (ii % 2 != 0)
    //                    {
    //                        pnlCustomList.Controls.Add(new LiteralControl("</TR>"));

    //                    }
    //                }
    //                ii++;
    //            }
    //            Session["ControlCreated"] = "CC";
    //            pnlCustomList.Controls.Add(new LiteralControl("</TABLE>"));
    //        }
    //    }
    //    catch
    //    {

    //    }
    //    finally
    //    {
    //        CustomFields = null;
    //    }
    //}

    //Amitava Sinha
    //Generating full DML Statement 
    //-------------
    private void GenerateUpdateCustomFieldsValues(Control Cntrl)
    {
        string pnlName = Cntrl.ID;
        sbValues = new StringBuilder();
        strmultiselect = string.Empty;
        string strfName = string.Empty;
        Boolean radioflag = false;

        Int32 stpos = 0;
        Int32 enpos = 0;
        if (Session["CustomFieldsData"] != null)
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

        if (Session["CustomFieldsData"] == null)
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
    //----------
    private void UpdateCustomFieldsValues()
    {
        GenerateUpdateCustomFieldsValues(pnlCustomList);
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
            if (Session["CustomFieldsData"] != null)
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
                Session["CustomFieldsData"] = 1;
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
                            strdelete = "DELETE from [" + obj.ToString() + "] where ptn_pk= " + PatientId.ToString() + " and LocationID=" + Session["AppLocationID"] + " and ptn_pharmacy_pk=" + PharmacyId;
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
            sqlstr = "INSERT INTO dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "(ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate " + sbParameter.ToString() + " )";
            //sqlstr += " VALUES(" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";
            sqlstr += " VALUES(" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'" + sbValues.ToString() + ")";

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
                                    sqlselect = "INSERT INTO [" + obj.ToString() + "](ptn_pk,LocationID,ptn_pharmacy_pk,OrderedbyDate, [" + obj.ToString().Substring(ispos, iepos) + "])";
                                    //sqlselect += " VALUES (" + PatientId.ToString() + "," + Application["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";
                                    sqlselect += " VALUES (" + PatientId.ToString() + "," + Session["AppLocationID"] + "," + PharmacyId + ",'" + OrderedbyDate + "'," + str.ToString() + ")";
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
        if (Session["CustomFieldsData"] != null)
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

        if (Session["CustomFieldsData"] == null)
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
    //Amitava Sinha 
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
            dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + theTblName.ToString().Replace("-", "_"), theColName, Convert.ToInt32(PatID.ToString()), 0, 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
            CustomFieldClinical theCustomManager = new CustomFieldClinical();
            theCustomManager.FillCustomFieldData(theCustomFields, dsvalues, pnlCustomList, "APharm");
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

        //start-------------------------------- 
        //string pnlName = Cntrl.ID;

        //DataSet dsvalues = null;
        //ICustomFields CustomFields;
        //Int32 PharmacyId = 0;
        //if (ViewState["PatientVisitId"] != null)
        //    PharmacyId = Convert.ToInt32(ViewState["PatientVisitId"]);

        //try
        //{
        //    CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields, BusinessProcess.Administration");
        //    //dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + TableName.ToString().Replace("-", "_"), sbParameter.ToString(), Convert.ToInt32(PatID.ToString()), Convert.ToInt32(Application["AppLocationID"].ToString()), 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
        //    dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + TableName.ToString().Replace("-", "_"), sbParameter.ToString(),  Convert.ToInt32(PatID.ToString()),0, 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
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
        //        Session["CustomFieldsData"] = 1;
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
        //                    dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()),0, 0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
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
        //                    dsmvalues = CustomFields.GetCustomFieldValues("[dtl_CustomField_" + TableName.ToString().Replace("-", "_") + "_" + strfldName.ToString() + "]", ",[" + strfldName.ToString() + "]", Convert.ToInt32(PatID.ToString()), 0,0, 0, Convert.ToInt32(PharmacyId), Convert.ToInt32(ApplicationAccess.AdultPharmacy));
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
    #endregion
    #endregion
    protected void BtnAddARV_Click(object sender, EventArgs e)
    {
        string theScript;

        Application.Add("MasterData", Session["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)Session["AddARV"]);
        theScript = "<script language='javascript' id='DrgPopup'>\n";
        //theScript += "window.open('frmDrugSelector.aspx?DrugType=37','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "window.open('frmPharmacySelector.aspx?DrugType=37','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
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
        //theScript += "window.open('frmDrugSelector.aspx?DrugType=0','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "window.open('frmPharmacySelector.aspx?DrugType=0','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
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
    protected void btnOtherTBMedicaton_Click(object sender, EventArgs e)
    {
        string theScript;

        Application.Add("MasterData", (DataTable)Session["MasterDrugTable"]);
        Application.Add("SelectedDrug", (DataTable)Session["AddTB"]);

        theScript = "<script language='javascript' id='DrgPopup'>\n";
        //theScript += "window.open('frmDrugSelector.aspx?DrugType=31','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
        theScript += "window.open('frmPharmacySelector.aspx?DrugType=31','DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
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
        }
        else if (theContainer.ID == "PnlOIARV") //--ARV
        {
            #region "OI Drugs"
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
                    DataRow[] DRStrength = ((DataSet)Session["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
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
                                    if (theStrengthId != 0 && theFrequencyId != 0 && theDuration != 0 && theQtyPrescribed != 0 && theProphylaxis != 999)
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
                                        ChkColFilled = 3;
                                    else
                                        ChkColFilled = 4;
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
                                    if ((TotColFilled > 0 && TotColFilled < 3) && (theContainer.ID == "PnlOIARV"))
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


        else if (theContainer.ID == "PnlDrug")
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
                    DataRow[] DRStrength = ((DataSet)Session["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
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
                        if ((TotelColFilled > 0 && TotelColFilled < ChkColFilled) && (theContainer.ID == "PnlDrug"))
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

                        if ((TotelColFilled > 0 && TotelColFilled < ChkColFilled) && (theContainer.ID == "PnlDrug"))
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
                    DataRow[] DRStrength = ((DataSet)Session["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
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
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgTreatmenPhase"))
                                        {
                                            theTreatmentPhase = Convert.ToString(((DropDownList)x).SelectedValue);
                                            TotelColFilled++;
                                        }
                                        if (x.ID.EndsWith(DrugID.ToString() + "^" + theDR["Generic"].ToString()) && x.ID.StartsWith("drgTreatmenMonth"))
                                        {
                                            theMonth = Convert.ToInt32(((DropDownList)x).SelectedValue);
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

                    //if (UnitId != 0 || theFrequencyId != 0 || Dose != 0 || theDuration != 0 || theQtyPrescribed != 0 || theQtyDispensed != 0 || theTreatmentPhase != "0" || theMonth != 0 || theFinanced != 99)
                    if (theFrequencyId != 0 || theDuration != 0 || theQtyPrescribed1 != 0 || theQtyDispensed1 != 0 || theTreatmentPhase != "0" || theMonth != 0 || theProphylaxis != 999)
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
        else if (theContainer.ID == "PnlOtherMedication")
        {
            #region "Other Medication Drugs"
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

                    DataRow[] DRStrength = ((DataSet)Session["MasterData"]).Tables[1].Select("GenericId=" + Convert.ToInt32(theDR["DrugId"]));
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

                    if (theFrequencyId != 0 || theDuration != 0 || theQtyPrescribed1 != 0 || theQtyDispensed1 != 0 || theProphylaxis != 999)
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
                    #region "18-Jun-07 - 12"
                    int ChkColFilled;
                    if (Session["SCMModule"] != null)
                        ChkColFilled = 3;
                    else
                        ChkColFilled = 4;
                    if ((TotelColFilled > 0 && TotelColFilled < ChkColFilled) && (theContainer.ID == "PnlOtherMedication"))
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
        return theDT;
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
    //private DataTable MakeMaternalHealth()
    //{
    //    DataTable theDT = new DataTable();

    //    if (Session["Data"] == null)
    //    {
    //        theDT = CreateTable();
    //    }
    //    else
    //    {
    //        theDT = (DataTable)Session["Data"];
    //    }
    //    if (chkmultiVitamin.Checked == true)
    //    {
    //        DataRow[] theChkRow = theDT.Select("GenericId=203");
    //        if (theChkRow.Length < 1)
    //        {
    //            DataRow theRow;
    //            theRow = theDT.NewRow();
    //            theRow["GenericId"] = 203;
    //            theRow["DrugId"] = 0;
    //            theRow["UnitId"] = 0;
    //            theRow["Dose"] = 0.0;
    //            theRow["StrengthId"] = 0;
    //            theRow["FrequencyId"] = 0;
    //            theRow["Duration"] = 0.0;
    //            theRow["DrugSchedule"] = 0;
    //            theRow["QtyPrescribed"] = 0.0;
    //            theRow["QtyDispensed"] = 0.0;
    //            theRow["Financed"] = 0;
    //            theRow["Prophylaxis"] = 0;

    //            theDT.Rows.Add(theRow);
    //        }
    //    }

    //    if (chkiron.Checked == true)
    //    {
    //        DataRow[] theChkRow = theDT.Select("GenericId=476");
    //        if (theChkRow.Length < 1)
    //        {

    //            DataRow theRow;
    //            theRow = theDT.NewRow();
    //            theRow["GenericId"] = 476;
    //            theRow["DrugId"] = 0;
    //            theRow["UnitId"] = 0;
    //            theRow["Dose"] = 0.0;
    //            theRow["StrengthId"] = 0;
    //            theRow["FrequencyId"] = 0;
    //            theRow["Duration"] = 0.0;
    //            theRow["DrugSchedule"] = 0;
    //            theRow["QtyPrescribed"] = 0.0;
    //            theRow["QtyDispensed"] = 0.0;
    //            theRow["Financed"] = 0;
    //            theRow["Prophylaxis"] = 0;

    //            theDT.Rows.Add(theRow);
    //        }
    //    }
    //    if (chkfolicacid.Checked == true)
    //    {
    //        DataRow[] theChkRow = theDT.Select("GenericId=138");
    //        if (theChkRow.Length < 1)
    //        {

    //            DataRow theRow;
    //            theRow = theDT.NewRow();

    //            theRow["GenericId"] = 138;
    //            theRow["DrugId"] = 0;
    //            theRow["UnitId"] = 0;
    //            theRow["Dose"] = 0.0;
    //            theRow["StrengthId"] = 0;
    //            theRow["FrequencyId"] = 0;
    //            theRow["Duration"] = 0.0;
    //            theRow["DrugSchedule"] = 0;
    //            theRow["QtyPrescribed"] = 0.0;
    //            theRow["QtyDispensed"] = 0.0;
    //            theRow["Financed"] = 0;
    //            theRow["Prophylaxis"] = 0;
    //            theDT.Rows.Add(theRow);
    //        }
    //    }
    //    if (chkvitaminA.Checked == true)
    //    {
    //        DataRow[] theChkRow = theDT.Select("GenericId=350");
    //        if (theChkRow.Length < 1)
    //        {

    //            DataRow theRow;
    //            theRow = theDT.NewRow();
    //            theRow["GenericId"] = 350;
    //            theRow["DrugId"] = 0;
    //            theRow["UnitId"] = 0;
    //            theRow["Dose"] = 0.0;
    //            theRow["StrengthId"] = 0;
    //            theRow["FrequencyId"] = 0;
    //            theRow["Duration"] = 0.0;
    //            theRow["DrugSchedule"] = 0;
    //            theRow["QtyPrescribed"] = 0.0;
    //            theRow["QtyDispensed"] = 0.0;
    //            theRow["Financed"] = 0;
    //            theRow["Prophylaxis"] = 0;
    //            theDT.Rows.Add(theRow);
    //        }
    //    }
    //    if (chkvitaminC.Checked == true)
    //    {
    //        DataRow[] theChkRow = theDT.Select("GenericId=353");
    //        if (theChkRow.Length < 1)
    //        {

    //            DataRow theRow;
    //            theRow = theDT.NewRow();
    //            theRow["GenericId"] = 353;
    //            theRow["DrugId"] = 0;
    //            theRow["UnitId"] = 0;
    //            theRow["Dose"] = 0.0;
    //            theRow["StrengthId"] = 0;
    //            theRow["FrequencyId"] = 0;
    //            theRow["Duration"] = 0.0;
    //            theRow["DrugSchedule"] = 0;
    //            theRow["QtyPrescribed"] = 0.0;
    //            theRow["QtyDispensed"] = 0.0;
    //            theRow["Financed"] = 0;
    //            theRow["Prophylaxis"] = 0;
    //            theDT.Rows.Add(theRow);
    //        }
    //    }
    //    if (ddltetanus.SelectedItem.Value.ToString() != "0")
    //    {
    //        DataRow theRow;
    //        theRow = theDT.NewRow();
    //        //theRow["GenericId"] = Convert.ToInt32(ddltetanus.SelectedItem.Value);
    //        theRow["GenericId"] = 531;
    //        theRow["DrugId"] = 0;
    //        theRow["UnitId"] = 0;
    //        theRow["Dose"] = 0.0;
    //        theRow["StrengthId"] = 0;
    //        theRow["FrequencyId"] = 0;
    //        theRow["Duration"] = 0.0;
    //        theRow["DrugSchedule"] = Convert.ToInt32(ddltetanus.SelectedItem.Value); 
    //        theRow["QtyPrescribed"] = 0.0;
    //        theRow["QtyDispensed"] = 0.0;
    //        theRow["Financed"] = 0;
    //        theRow["Prophylaxis"] = 0;
    //        theDT.Rows.Add(theRow);
    //    }
    //    if (chkmebendazole.Checked == true)
    //    {
    //        DataRow[] theChkRow = theDT.Select("GenericId=190");
    //        if (theChkRow.Length < 1)
    //        {

    //            DataRow theRow;
    //            theRow = theDT.NewRow();
    //            theRow["GenericId"] = 190;
    //            theRow["DrugId"] = 0;
    //            theRow["UnitId"] = 0;
    //            theRow["Dose"] = 0.0;
    //            theRow["StrengthId"] = 0;
    //            theRow["FrequencyId"] = 0;
    //            theRow["Duration"] = 0.0;
    //            theRow["DrugSchedule"] = 0;
    //            theRow["QtyPrescribed"] = 0.0;
    //            theRow["QtyDispensed"] = 0.0;
    //            theRow["Financed"] = 0;
    //            theRow["Prophylaxis"] = 0;
    //            theDT.Rows.Add(theRow);
    //        }
    //    }
    //    return theDT;
    //}
    //private DataTable MakeDrugTableRegimen(Control theContainer)
    //{
    //    IDrug DrugManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");

    //    DataTable theDT = new DataTable();

    //    if (Session["Data"] == null)
    //    {
    //        theDT = CreateTable();
    //    }
    //    else
    //    {
    //        theDT = (DataTable)Session["Data"];
    //    }

    //    #region "Variables"
    //    decimal Dose = 0;
    //    int UnitId = 0;
    //    int theStrengthId = 0;
    //    int theFixedDrugId = 0;
    //    int theRegimenID = 0;
    //    int theFrequencyId = 0;
    //    Decimal theDuration = 0;
    //    Decimal theQtyPrescribed = 0;
    //    Decimal theQtyDispensed = 0;
    //    Decimal theQtyPrescribed1 = 0;
    //    Decimal theQtyDispensed1 = 0;
    //    string theTreatmentPhase = "";
    //    int theMonth = 0;
    //    int theFinanced = 99;
    //    DataSet theGenericDS = new DataSet();

    //    #endregion
    //    if (theContainer.ID == "pnltbregimen")
    //    {
    //        //theRegimenID = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
    //        if (theRegimenID.ToString() != "0")
    //        {
    //            theGenericDS = DrugManager.Get_TBRegimen_Detail(Convert.ToInt32(theRegimenID));
    //        }

    //        if (theGenericDS.Tables[0].Rows.Count > 0)
    //        {
    //            foreach (DataRow dr in theGenericDS.Tables[0].Rows)
    //            {
    //                //theRegimenID = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
    //                //if (ddlARVCombReg.SelectedItem.Value.ToString() != "0")
    //                //{
    //                //    theFrequencyId = Convert.ToInt32(ddlARVCombReg.SelectedItem.Value);
    //                //}
    //                //if (txtARVCombRegQtyPres.Text != "")
    //                //{
    //                //    theQtyPrescribed = Convert.ToDecimal(txtARVCombRegQtyPres.Text);
    //                //}
    //                //if (txtARVCombRegDuraton.Text != "")
    //                //{
    //                //    theDuration = Convert.ToDecimal(txtARVCombRegDuraton.Text);
    //                //}
    //                //if (txtARVCombRegQtyDesc.Text != "")
    //                //{
    //                //    theQtyDispensed = Convert.ToDecimal(txtARVCombRegQtyDesc.Text);
    //                //}
    //                //if (ddlTreatmentphase.SelectedItem.Value.ToString() != "0")
    //                //{
    //                //    theTreatmentPhase = ddlTreatmentphase.SelectedItem.Value.ToString();
    //                //}
    //                //if (ddTrMonths.SelectedItem.Value.ToString() != "0")
    //                //{
    //                //    theMonth = Convert.ToInt32(ddTrMonths.SelectedItem.Value);
    //                //}
    //                if (theRegimenID != 0 && theDuration != 0)
    //                {

    //                    DataRow theRow;
    //                    theRow = theDT.NewRow();
    //                    theRow["GenericId"] = Convert.ToInt32(dr["GenericID"]);
    //                    theRow["DrugId"] = 0;
    //                    theRow["TBRegimenId"] = theRegimenID;
    //                    theRow["Dose"] = 0;
    //                    theRow["UnitId"] = 0;
    //                    theRow["StrengthId"] = 1;
    //                    theRow["FrequencyId"] = theFrequencyId;
    //                    theRow["Duration"] = theDuration;
    //                    theRow["QtyPrescribed"] = theQtyPrescribed;
    //                    theRow["QtyDispensed"] = theQtyDispensed;
    //                    theRow["Financed"] = 1;
    //                    theRow["TreatmentPhase"] = theTreatmentPhase;
    //                    theRow["TrMonth"] = theMonth;
    //                    theDT.Rows.Add(theRow);
    //                    #region "Reset Variables
    //                    Dose = 0;
    //                    UnitId = 0;
    //                    theRegimenID = 0;
    //                    theStrengthId = 0;
    //                    theFrequencyId = 0;
    //                    theDuration = 0;
    //                    theQtyPrescribed = 0;
    //                    theQtyDispensed = 0;
    //                    theFinanced = 99;
    //                    theTreatmentPhase = "";
    //                    theMonth = 0;
    //                    #endregion
    //                }

    //            }
    //        }
    //    }
    //    return theDT;
    //}
    private Boolean ProPhalaxisCheck(DataTable dt)
    {
        Boolean blnProCheck = true;
        DataTable theDTDrug = ((DataSet)Session["MasterData"]).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            if (r["Prophylaxis"].ToString() == "999")
            {
                blnProCheck = NonARTProPhalasxisCheck(theDTDrug, r["DrugId"].ToString());
                if (!(blnProCheck))
                    break;
            }
        }
        return blnProCheck;
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
            Session.Remove("Data");
            Session["Data"] = MakeDrugTable(PnlRegiment);
            if (((DataTable)Session["Data"]).Rows.Count > 0)
            {
                DataRow[] theFilDT = ((DataTable)Session["Data"]).Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    Session.Remove("Data");
                    IQCareMsgBox.Show("RegimenLineExists", this);
                    return;
                }

            }
            if (ddlTreatment.SelectedValue.ToString() != "225")
            {
                Session["Data"] = MakeDrugTable(PnlDrug);
            }

            if (((DataTable)Session["Data"]).Rows.Count > 0)
            {

                DataRow[] theFilDT = ((DataTable)Session["Data"]).Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    Session.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }


            }
            if (ddlTreatment.SelectedValue.ToString() == "222")
            {
                if (DuplicateRegimenValidate((DataTable)Session["Data"], (DataSet)Session["MasterData"]) == false)
                {
                    return;
                }
            }

            if (FieldValidation() == false)
            {
                return;
            }
            if (ddlTreatment.SelectedValue.ToString() != "225")
            {
                Session["Data"] = MakeDrugTable(PnlOIARV);
            }
            if (((DataTable)Session["Data"]).Rows.Count > 0)
            {

                DataRow[] theFilDT = ((DataTable)Session["Data"]).Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    Session.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }


            }

            Session["Data"] = MakeDrugTable(PnlOtherMedication);
            if (((DataTable)Session["Data"]).Rows.Count > 0)
            {
                DataRow[] theFilDT = ((DataTable)Session["Data"]).Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    Session.Remove("Data");
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }

            }
            //Session["Data"] = MakeMaternalHealth();
            DataTable theDT = MakeDrugTable(pnlOtherTBMedicaton);
            if (theDT.Rows.Count > 0)
            {
                DataRow[] theFilDT = theDT.Select("DrugId=99999");
                if (theFilDT.Length > 0)
                {
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }

            }
            if (ddlTreatment.SelectedValue.ToString() == "223")
            {
                if (ProPhalaxisCheck(theDT) == false)
                {
                    IQCareMsgBox.Show("PharmacyIncompleteData", this);
                    return;
                }
            }
            
            // if (theDT.Rows.Count == 0)
            // {

            //     IQCareMsgBox.Show("PharmacyIncompleteData", this);
            //     return;

            // }


            // if (ddlTreatment.SelectedValue.ToString() == "223")
            // {
            //     if (ProPhalaxisCheck(theDT) == false)
            //     {
            //         IQCareMsgBox.Show("PharmacyIncompleteData", this);
            //         return;
            //     }
            // }
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
                    if (FieldValidationPaperLess() == false)
                    {
                        return;
                    }
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
            IDrug PharmacyManager = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug, BusinessProcess.Pharmacy");
            try
            {
                int PatientID, LocationId, UserId, EmpSignature = 0, Dispense = 0; ;
                int PeriodTaken = 0;
                PatientID = Convert.ToInt32(Session["PatientId"]);
                if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                    LocationId = Convert.ToInt32(Session["AppLocationId"]);
                else
                    LocationId = Convert.ToInt32(Session["ServiceLocationId"].ToString());

                UserId = Convert.ToInt32(Session["AppUserId"]);
                if (btnCounsellorSignature.Checked == true)
                    EmpSignature = Convert.ToInt32(ddlPharmSignature.SelectedValue);

                int theTreatmentID;
                theTreatmentID = Convert.ToInt32(ddlTreatment.SelectedValue);

                int theProviderID;
                theProviderID = Convert.ToInt32(ddlProvider.SelectedValue);

                PeriodTaken = Convert.ToInt32(ddlPeriodTaken.SelectedItem.Value);
                int SCMFlag;//if SCM Module is On then SCMFlag=1 else SCMFlag=2
                if (Session["SCMModule"] != null)
                    SCMFlag = 1;
                else
                    SCMFlag = 2;

                if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
                {

                    CustomFieldClinical theCustomManager = new CustomFieldClinical();
                    DataTable theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Insert", ApplicationAccess.AdultPharmacy, (DataSet)ViewState["CustomFieldsDS"]);

                    int pharmaID = (int)PharmacyManager.SaveUpdateDrugOrder(PatientID, LocationId, 0, Convert.ToInt32(ViewState["RegimenLine"]), txtClinicalNotes.Text, Convert.ToInt32(ddlPharmOrderedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)), Convert.ToInt32(ddlPharmReportedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value)),
                        Dispense, 1, EmpSignature, 116, UserId, theDT, (DataSet)Session["MasterData"], theTreatmentID, theProviderID, theCustomDataDT, PeriodTaken, 1, SCMFlag);
                    ViewState["PharmacyId"] = Convert.ToInt32(pharmaID.ToString());
                    Session["PatientVisitId"] = Convert.ToInt32(pharmaID.ToString());

                    if (pharmaID == 0)
                    {
                        IQCareMsgBox.Show("PharmacyDetailExists", this);
                        return;
                    }
                    else
                    {
                        SaveCancel();
                        //Session["PatientVisitId"] = 0;
                    }
                }
                else
                {
                    int PharmacyId = Convert.ToInt32(ViewState["PharmacyId"]);
                    if (PharmacyId == 0)
                    {
                        PharmacyId = Convert.ToInt32(Session["PatientVisitId"]);
                    }
                    CustomFieldClinical theCustomManager = new CustomFieldClinical();
                    DataTable theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Update", ApplicationAccess.AdultPharmacy, (DataSet)ViewState["CustomFieldsDS"]);

                    //SaveUpdateDrugOrder(int patientID, int LocationID,int PharmacyId, int OrderedBy, DateTime OrderedByDate, int DispensedBy, DateTime DispensedByDate, int HoldMedicine, int Signature, int EmployeeID, int OrderType, int UserID, DataTable DrugTable, DataSet Master, int ProgID, int ProviderID, DataTable theCustomFieldData, int PeriodTaken,int flag)

                    int RowsAffected = (int)PharmacyManager.SaveUpdateDrugOrder(PatientID, LocationId, PharmacyId, Convert.ToInt32(ViewState["RegimenLine"]), txtClinicalNotes.Text, Convert.ToInt32(ddlPharmOrderedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value)), Convert.ToInt32(ddlPharmReportedbyName.SelectedValue), Convert.ToDateTime(theUtils.MakeDate(txtpharmReportedbyDate.Value)),
                       Dispense, 1, EmpSignature, 307, UserId, theDT, (DataSet)Session["MasterData"], theTreatmentID, theProviderID, theCustomDataDT, PeriodTaken, 2, SCMFlag);
                    SaveCancel();
                    //Session["PatientVisitId"] = 0;
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
            theUrl = string.Format("{0}?sts={1}", "../ClinicalForms/frmPatient_Home.aspx", Session["Status"].ToString()); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        }
        else if (Convert.ToInt32(Session["PatientVisitId"]) != 0)
        {
            theUrl = string.Format("{0}?sts={1}", "../ClinicalForms/frmPatient_History.aspx",  Session["Status"].ToString()); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        }
        else
        {
            theUrl = string.Format("{0}?sts={1}", "../ClinicalForms/frmClinical_DeleteForm.aspx",  Session["Status"].ToString());
        }
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
            string script = "<script language = 'javascript' defer ='defer' id = 'Showclose_0'>\n";
            script += "self.close();";
            script += "</script>\n";
            RegisterStartupScript("Showclose_0", script);
            return;
        }
        Response.Redirect(theUrl);
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

        DeleteForm();
        //string theUrl = "";
        //if (Convert.ToInt32(ViewState["PatientVisitId"]) == 0)
        //{
        //    theUrl = string.Format("{0}", "../ClinicalForms/frmPatient_Home.aspx"); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
        //}
        //else if (Convert.ToInt32(ViewState["PatientVisitId"]) != 0)
        //{
        //    theUrl = string.Format("{0}", "../ClinicalForms/frmPatient_History.aspx"); //"frmAdultPharmacyList.aspx?PatientId";    //=PtnID";
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
        string strPharmacyID = ViewState["PharmacyId"].ToString();
        string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
        script += "var ans;\n";
        script += "ans=window.confirm('Drug Order saved successfully. Do you want to close?');\n";
        script += "if (ans==true)\n";
        script += "{\n";
        // opento=ArtForm
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
        //script += "window.location.href('../Pharmacy/frmPharmacy_Adult.aspx');\n";
        //script += "}\n";
        script += "</script>\n";
        RegisterStartupScript("confirm", script);




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

    protected void ddlTreatment_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region "Check ARTStop"

        Session["TreatmentProg"] = ddlTreatment.SelectedValue.ToString();
        if (Session["TreatmentProg"].ToString() == "225")
        {
            PnlDrug.Enabled = false;
        }
        else
        {
            PnlDrug.Enabled = true;
        }

        IDrug theValidationManger = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
        DataTable theValidateDT = theValidationManger.CheckARTStopStatus(Convert.ToInt32(Session["PatientId"]));
        if ((theValidateDT.Rows.Count > 0 && theValidateDT.Rows[0]["ARTStatus"].ToString() == "ART Stopped" && Convert.ToInt32(Session["PatientVisitId"]) == 0 && Convert.ToInt32(ddlTreatment.SelectedValue) == 222) ||
            (theValidateDT.Rows.Count > 0 && theValidateDT.Rows[0]["ARTStatus"].ToString() == "ART Stopped" && Convert.ToInt32(Session["PatientVisitId"]) > 0
            && Convert.ToDateTime(txtpharmReportedbyDate.Value) >= Convert.ToDateTime(theValidateDT.Rows[0]["ARTEndDate"]) && Convert.ToInt32(ddlTreatment.SelectedValue) == 222))
        {
            if (PnlDrug.HasControls())
            {
                PnlDrug.Enabled = false;
            }

        }
        //else
        //{
        //    if (PnlDrug.HasControls())
        //    {
        //        PnlDrug.Enabled = true;
        //        VisibleLnkControls(PnlDrug, false);
        //    }

        //}
        theValidationManger = null;
        theValidateDT.Dispose();

        #endregion



        if (Convert.ToInt32(ddlTreatment.SelectedValue) == 222)
        {
            EnableDisableAllCheckBoxControls(PnlDrug, false);
        }
    }
}




