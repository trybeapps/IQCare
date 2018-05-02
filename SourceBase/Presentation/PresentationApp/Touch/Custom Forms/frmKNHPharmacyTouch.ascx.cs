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

using Application.Common;
using Application.Presentation;
using Interface.Pharmacy;
using Interface.Clinical;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using System.Collections.Generic;

namespace Touch.Custom_Forms
{
    public partial class frmKNHPharmacyTouch : TouchUserControlBase
    {
        DataTable tbldrug = new DataTable();
        DataTable tblOrder = new DataTable();
        DataTable tblDispense = new DataTable();
        DataTable tblRefill = new DataTable();
        DataTable dt = new DataTable();
        DataSet theDS;
        DataTable theDT;
        DataTable theOrder;
        DataTable theDispense;
        DataTable theRefill;
        BindFunctions theBind = new BindFunctions();
        DataSet theExistDS = new DataSet();
        int PharmacyID = 0;
        DateTime theCurrentDate;
        IIQCareSystem IQCareSecurity;
       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null && Page.IsCallback)
                EnsureChildControls();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            String script = frmKNHPharmacy_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            base.Page_Load(sender, e);
            if (IsPostBack) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);
            Session["CurrentForm"] = "frmKNHPharmacyTouch";
            Session["FormIsLoaded"] = true;

            ZZZPharmHeight.Attributes.Add("onblur", "fnSetBMI('" + ZZZPharmWeight.ClientID + "','" + ZZZPharmHeight.ClientID + "','" + ZZZPharmBSA.ClientID+ "')");
            if (Session["IsFirstLoad"] != null)
            {
                if (Session["IsFirstLoad"].ToString() == "true")
                {
                    ViewState["RefillClick"] = "false";
                    BindMasterTable();
                    BindControls();
                    BindPreSelectedDrug();
                    if (Session["Refill"].ToString() == "0")
                    {
                        btnrefill.Enabled = false;
                    }
                    if (Session["Orderid"].ToString() != "0")
                    {
                        BindExistPaediatricDetails();
                        rcbprescribed.Enabled = false;
                        prescribedbydate.Enabled = false;
                    }
                    if (Session["Orderid"].ToString() == "0")
                    {
                        rcbdispensed.Enabled = false;
                        dispensedbydate.Enabled = false;
                    }
                    Session["IsFirstLoad"] = "false";
                }
            }
            else
            {
                theDS = (DataSet)Session["MasterData"];
            }

            IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            theCurrentDate = (DateTime)IQCareSecurity.SystemDate();

            BindAutoCompleteDrug();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "closeloadpharm", "parent.CloseLoading()", true);
        }
        public void BindExistPaediatricDetails()
        {
            IPediatric PediatricManager;
            PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            PharmacyID = Convert.ToInt32(Session["Orderid"].ToString());
            theExistDS = PediatricManager.GetExistPaediatricDetails(PharmacyID);
            ViewState["ExistsDS"] = theExistDS;
            if (theExistDS.Tables.Count == 0)
            {
                IQCareMsgBox.Show("NoPharmacyRecordExists", this);
                return;
            }
            if (theExistDS.Tables[0].Rows.Count > 0)
            {
                if ((theExistDS.Tables[0].Rows[0]["Weight"] != System.DBNull.Value) || (theExistDS.Tables[0].Rows[0]["Height"] != System.DBNull.Value))
                {
                    decimal theWeight = Convert.ToDecimal(theExistDS.Tables[0].Rows[0]["Weight"].ToString());
                    if (theWeight > 0)
                        ZZZPharmWeight.Text = Convert.ToString(theWeight);
                    decimal theHeight = Convert.ToDecimal(theExistDS.Tables[0].Rows[0]["Height"].ToString());
                    if (theHeight > 0)
                        ZZZPharmHeight.Text = Convert.ToString(theHeight);
                    decimal theBSA = theWeight * theHeight / 3600;
                    theBSA = (decimal)Math.Sqrt(Convert.ToDouble(theBSA));
                    theBSA = Math.Round(theBSA, 2);
                    ZZZPharmBSA.Text = Convert.ToString(theBSA);
                }
            }
            if (theExistDS.Tables[1].Rows.Count > 0)
            {
                if (theExistDS.Tables[1].Rows[0]["AppDate"].ToString() != "1/1/1900 12:00:00 AM")
                {
                    DateTime theAppntmentDate = Convert.ToDateTime(theExistDS.Tables[1].Rows[0]["AppDate"].ToString());
                    appdate.DbSelectedDate = theAppntmentDate.ToString(Session["AppDateFormat"].ToString());
                }
                if (theExistDS.Tables[1].Rows[0]["AppReason"] != System.DBNull.Value)
                {
                    rcbreason.SelectedValue = theExistDS.Tables[1].Rows[0]["AppReason"].ToString();
                }
            }
            if (theExistDS.Tables[0].Rows.Count > 0)
            {
                if (theExistDS.Tables[0].Rows[0]["RegimenLine"] != System.DBNull.Value)
                {
                    ddregimenline.SelectedValue = theExistDS.Tables[0].Rows[0]["RegimenLine"].ToString();
                }
                if (theExistDS.Tables[0].Rows[0]["ProgID"] != System.DBNull.Value)
                {
                    rcbtreatment.SelectedValue = theExistDS.Tables[0].Rows[0]["ProgID"].ToString();
                }
                if (theExistDS.Tables[0].Rows[0]["ProviderID"] != System.DBNull.Value)
                {
                    rcbdrugprovider.SelectedValue = theExistDS.Tables[0].Rows[0]["ProviderID"].ToString();
                }
                if (theExistDS.Tables[0].Rows[0]["pharmacyperiodtaken"] != System.DBNull.Value)
                {
                    rcbperiod.SelectedValue = theExistDS.Tables[0].Rows[0]["pharmacyperiodtaken"].ToString();
                }
                
                PharmNotes.Text = theExistDS.Tables[0].Rows[0]["PharmacyNotes"].ToString();
                if (theExistDS.Tables[0].Rows[0]["OrderedByDate"] != System.DBNull.Value)
                {
                    DateTime theOrderedByDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["OrderedByDate"].ToString());
                    prescribedbydate.DbSelectedDate = theOrderedByDate.ToString(Session["AppDateFormat"].ToString());

                }

                if (theExistDS.Tables[0].Rows[0]["DispensedByDate"].ToString() != "")
                {
                    DateTime theReportedbyDate = Convert.ToDateTime(theExistDS.Tables[0].Rows[0]["DispensedByDate"]);
                    dispensedbydate.DbSelectedDate = theReportedbyDate.ToString(Session["AppDateFormat"].ToString());
                    //btnSave.Enabled = false;
                }

                rcbprescribed.SelectedValue = theExistDS.Tables[0].Rows[0]["OrderedBy"].ToString();
                rcbdispensed.SelectedValue = theExistDS.Tables[0].Rows[0]["DispensedBy"].ToString();
            }
            if (ViewState["TableDrug"] == null)
            {
                theDT = CreateDrugTable();
            }
            else
            {
                theDT = (DataTable)ViewState["TableDrug"];
            }
            //---------Order-----------
            if (ViewState["TableOrder"] == null)
            {
                theOrder = CreateOrderTable();
            }
            else
            {
                theOrder = (DataTable)ViewState["TableOrder"];
            }
            //--------------Dispense ------------
            if (ViewState["TableDispense"] == null)
            {
                theDispense = CreateDispensTable();
            }
            else
            {
                theDispense = (DataTable)ViewState["TableDispense"];
            }

            //------------Refill
            if (ViewState["TableRefill"] == null)
            {
                theRefill = CreateRefillTable();
            }
            else
            {
                theRefill = (DataTable)ViewState["TableRefill"];
            }
            foreach (DataRow theDR in theExistDS.Tables[0].Rows)
            {
                if (theDR["Drug_Pk"] != System.DBNull.Value)
                {
                    DataSet theAutoDS = (DataSet)Session["MasterData"];
                    DataRow[] result = theAutoDS.Tables[33].Select("drug_pk=" + theDR["Drug_Pk"].ToString() + "");

                    DataRow[] findRow = theDT.Select("DrugID=" + theDR["Drug_Pk"].ToString() + "");
                    int len = findRow.Length;
                    if (len == 0)
                    {
                        foreach (DataRow row in result)
                        {

                            DataRow DR = theDT.NewRow();
                            DR["DrugID"] = row["drug_pk"];
                            DR["DrugName"] = row["drugname"];
                            DR["DrugType"] = row["drugtypename"];
                            DR["DispensingUnit"] = row["Dispensing Unit"];
                            DR["GenericID"] = row["GenericID"];
                            theDT.Rows.Add(DR);
                        }
                        DataRow DR1 = theOrder.NewRow();
                        DR1["DrugID"] = Convert.ToInt32(theDR["drug_pk"].ToString());
                        DR1["GenericID"] = Convert.ToInt32(theDR["GenericId"].ToString());
                        DR1["Dose"] = Convert.ToDecimal(theDR["SingleDose"].ToString());
                        DR1["Frequency"] = Convert.ToInt32(theDR["FrequencyId"].ToString());
                        DR1["Duration"] = Convert.ToDecimal(theDR["Duration"].ToString());
                        DR1["QtyPrescribed"] = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
                        DR1["Prophylaxis"] = Convert.ToInt32(theDR["Prophylaxis"].ToString());
                        theOrder.Rows.Add(DR1);
                        //---------dispense
                        DataRow DR2 = theDispense.NewRow();
                        DR2["DrugID"] = theDR["drug_pk"];
                        DR2["QtyDispensed"] = Convert.ToDecimal(theDR["DispensedQuantity"].ToString());
                        DR2["QtyPrescribed"] = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
                        theDispense.Rows.Add(DR2);

                        // refill
                        DataRow DR3 = theRefill.NewRow();
                        DR3["DrugID"] = theDR["drug_pk"];
                        if (theDR["Refill"].ToString() != "")
                        {
                            DR3["Refill"] = Convert.ToInt32(theDR["Refill"].ToString());
                        }
                        if(theDR["RefillExpirationdate"].ToString()!="")
                        {
                            DR3["RefillExpiration"] = theDR["RefillExpirationdate"].ToString();
                        }
                        theRefill.Rows.Add(DR3);
                    }
                }
            }
            ViewState["TableDrug"] = theDT;
            ViewState["TableOrder"] = theOrder;
            ViewState["TableDispense"] = theDispense;
            ViewState["TableRefill"] = theRefill;
            BindDrugGrid();
        }
        public void BindExistRefillPaediatricDetails()
        {
            DataSet theExistDSRefill = (DataSet)ViewState["ExistsDS"];
            IQCareUtils theUtils = new IQCareUtils();
            if (theExistDSRefill.Tables.Count == 0)
            {
                IQCareMsgBox.Show("NoPharmacyRecordExists", this);
                return;
            }
            if (theExistDSRefill.Tables[0].Rows.Count > 0)
            {
                if ((theExistDSRefill.Tables[0].Rows[0]["Weight"] != System.DBNull.Value) || (theExistDSRefill.Tables[0].Rows[0]["Height"] != System.DBNull.Value))
                {
                    decimal theWeight = Convert.ToDecimal(theExistDSRefill.Tables[0].Rows[0]["Weight"].ToString());
                    if (theWeight > 0)
                        ZZZPharmWeight.Text = Convert.ToString(theWeight);
                    decimal theHeight = Convert.ToDecimal(theExistDSRefill.Tables[0].Rows[0]["Height"].ToString());
                    if (theHeight > 0)
                        ZZZPharmHeight.Text = Convert.ToString(theHeight);
                    decimal theBSA = theWeight * theHeight / 3600;
                    theBSA = (decimal)Math.Sqrt(Convert.ToDouble(theBSA));
                    theBSA = Math.Round(theBSA, 2);
                    ZZZPharmBSA.Text = Convert.ToString(theBSA);
                }
            }
            if (theExistDSRefill.Tables[1].Rows.Count > 0)
            {
                DateTime theAppntmentDate = Convert.ToDateTime(theExistDSRefill.Tables[1].Rows[0]["AppDate"].ToString());
                appdate.DbSelectedDate = theAppntmentDate.ToString(Session["AppDateFormat"].ToString());

            }
            if (theExistDSRefill.Tables[0].Rows[0]["RegimenLine"] != System.DBNull.Value)
            {
                ddregimenline.SelectedValue = theExistDSRefill.Tables[0].Rows[0]["RegimenLine"].ToString();

            }
            PharmNotes.Text = theExistDSRefill.Tables[0].Rows[0]["PharmacyNotes"].ToString();
            if (theExistDSRefill.Tables[0].Rows[0]["OrderedByDate"] != System.DBNull.Value)
            {
                DateTime theOrderedByDate = Convert.ToDateTime(theExistDSRefill.Tables[0].Rows[0]["OrderedByDate"].ToString());
                prescribedbydate.DbSelectedDate = theOrderedByDate.ToString(Session["AppDateFormat"].ToString());

            }
            dispensedbydate.DbSelectedDate = "";
            rcbdispensed.SelectedValue = "0";
            rcbprescribed.SelectedValue = theExistDSRefill.Tables[0].Rows[0]["OrderedBy"].ToString();
            ViewState["TableDrug"] = null;
            ViewState["TableOrder"] = null;
            ViewState["TableDispense"] = null;
            ViewState["TableRefill"] = null;
            if (ViewState["TableDrug"] == null)
            {
                theDT = CreateDrugTable();
            }
            else
            {
                theDT = (DataTable)ViewState["TableDrug"];
            }
            //---------Order-----------
            if (ViewState["TableOrder"] == null)
            {
                theOrder = CreateOrderTable();
            }
            else
            {
                theOrder = (DataTable)ViewState["TableOrder"];
            }
            //--------------Dispense ------------
            if (ViewState["TableDispense"] == null)
            {
                theDispense = CreateDispensTable();
            }
            else
            {
                theDispense = (DataTable)ViewState["TableDispense"];
            }

            //------------Refill
            if (ViewState["TableRefill"] == null)
            {
                theRefill = CreateRefillTable();
            }
            else
            {
                theRefill = (DataTable)ViewState["TableRefill"];
            }
            foreach (DataRow theDR in theExistDSRefill.Tables[0].Rows)
            {
                if (theDR["Drug_Pk"] != System.DBNull.Value && theDR["Refill"].ToString() != "0" && theDR["RefillExpirationdate"].ToString()!="")
                {
                    string theExpirationtDate = Convert.ToDateTime(theDR["RefillExpirationdate"]).ToShortDateString();
                    string currentdate = theCurrentDate.ToShortDateString();
                    if (Convert.ToDateTime(theExpirationtDate) >= Convert.ToDateTime(currentdate))
                    {
                        DataSet theAutoDS = (DataSet)Session["MasterData"];
                        DataRow[] result = theAutoDS.Tables[33].Select("drug_pk=" + theDR["Drug_Pk"].ToString() + "");

                        DataRow[] findRow = theDT.Select("DrugID=" + theDR["Drug_Pk"].ToString() + "");
                        int len = findRow.Length;
                        if (len == 0)
                        {
                            foreach (DataRow row in result)
                            {

                                DataRow DR = theDT.NewRow();
                                DR["DrugID"] = row["drug_pk"];
                                DR["DrugName"] = row["drugname"];
                                DR["DrugType"] = row["drugtypename"];
                                DR["DispensingUnit"] = row["Dispensing Unit"];
                                DR["GenericID"] = row["GenericID"];
                                theDT.Rows.Add(DR);
                            }
                            DataRow DR1 = theOrder.NewRow();
                            DR1["DrugID"] = Convert.ToInt32(theDR["drug_pk"].ToString());
                            DR1["GenericID"] = Convert.ToInt32(theDR["GenericId"].ToString());
                            DR1["Dose"] = Convert.ToDecimal(theDR["SingleDose"].ToString());
                            DR1["Frequency"] = Convert.ToInt32(theDR["FrequencyId"].ToString());
                            DR1["Duration"] = Convert.ToDecimal(theDR["Duration"].ToString());
                            DR1["QtyPrescribed"] = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
                            DR1["Prophylaxis"] = Convert.ToInt32(theDR["Prophylaxis"].ToString());
                            theOrder.Rows.Add(DR1);
                            //---------dispense
                            DataRow DR2 = theDispense.NewRow();
                            DR2["DrugID"] = theDR["drug_pk"];
                            DR2["QtyDispensed"] = 0;
                            DR2["QtyPrescribed"] = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
                            theDispense.Rows.Add(DR2);

                            // refill
                            DataRow DR3 = theRefill.NewRow();
                            DR3["DrugID"] = theDR["drug_pk"];
                            DR3["Refill"] = 0;
                            DR3["RefillExpiration"] = "";
                            theRefill.Rows.Add(DR3);
                        }
                    }
                }
            }
            ViewState["TableDrug"] = theDT;
            ViewState["TableOrder"] = theOrder;
            ViewState["TableDispense"] = theDispense;
            ViewState["TableRefill"] = theRefill;
            //btnSave.Enabled = true;
            BindDrugGrid();
        }
        public DataTable CreateDrugTable()
        {
            tbldrug.Columns.Add("DrugID", typeof(Int32));
            tbldrug.Columns.Add("DrugName", typeof(string));
            tbldrug.Columns.Add("DispensingUnit", typeof(string));
            tbldrug.Columns.Add("GenericID", typeof(string));
            tbldrug.Columns.Add("DrugType", typeof(string));
            tbldrug.PrimaryKey = new DataColumn[] { tbldrug.Columns["DrugID"] };
            return tbldrug;


        }
        public DataTable CreateOrderTable()
        {
            tblOrder.Columns.Add("DrugID", typeof(Int32));
            tblOrder.Columns.Add("Dose", typeof(decimal));
            tblOrder.Columns.Add("Frequency", typeof(Int32));
            tblOrder.Columns.Add("Duration", typeof(decimal));
            tblOrder.Columns.Add("QtyPrescribed", typeof(decimal));
            tblOrder.Columns.Add("Prophylaxis", typeof(Int32));
            tblOrder.Columns.Add("GenericID", typeof(string));
            tblOrder.PrimaryKey = new DataColumn[] { tblOrder.Columns["DrugID"] };

            return tblOrder;
        }
        public DataTable CreateDispensTable()
        {
            tblDispense.Columns.Add("DrugID", typeof(Int32));
            tblDispense.Columns.Add("QtyPrescribed", typeof(decimal));
            tblDispense.Columns.Add("QtyDispensed", typeof(decimal));
            tblDispense.PrimaryKey = new DataColumn[] { tblDispense.Columns["DrugID"] };

            return tblDispense;
        }
        public DataTable CreateRefillTable()
        {

            tblRefill.Columns.Add("DrugID", typeof(Int32));
            tblRefill.Columns.Add("Refill", typeof(Int32));
            tblRefill.Columns.Add("RefillExpiration", typeof(string));
            tblRefill.PrimaryKey = new DataColumn[] { tblRefill.Columns["DrugID"] };

            return tblRefill;
        }
        public void BindMasterTable()
        {
            IPediatric PediatricManager;
            PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            theDS = PediatricManager.GetPediatricFields(Convert.ToInt32(Request.QueryString["PatientID"]));
            Session["MasterData"] = theDS;
            Session["Frequency"] = theDS.Tables[8];
            DateTime theDOBirth = (DateTime)theDS.Tables[6].Rows[0]["DOB"];
            ZZZPatDOB.Text = theDOBirth.ToString(Session["AppDateFormat"].ToString());
            ZZZCurrentAgeYear.Text = theDS.Tables[6].Rows[0]["Age"].ToString();
            ZZZCurrentAgeMonth.Text = theDS.Tables[6].Rows[0]["Age1"].ToString();

            if (theDS.Tables[31].Rows.Count > 0)
            {
                if ((theDS.Tables[31].Rows[0]["Weight"] != System.DBNull.Value) || (theDS.Tables[31].Rows[0]["Height"] != System.DBNull.Value))
                {
                    decimal theWeight = Convert.ToDecimal(theDS.Tables[31].Rows[0]["Weight"].ToString());
                    if (theWeight > 0)
                        ZZZPharmWeight.Text = Convert.ToString(theWeight);
                    decimal theHeight = Convert.ToDecimal(theDS.Tables[31].Rows[0]["Height"].ToString());
                    if (theHeight > 0)
                        ZZZPharmHeight.Text = Convert.ToString(theHeight);
                    decimal theBSA = theWeight * theHeight / 3600;
                    theBSA = (decimal)Math.Sqrt(Convert.ToDouble(theBSA));
                    theBSA = Math.Round(theBSA, 2);
                    ZZZPharmBSA.Text = Convert.ToString(theBSA);
                }
            }

        }
        public void BindPreSelectedDrug()
        {
            IDrug objRptFields;
            objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
            string sqlQuery1;
            sqlQuery1 = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0 and drug_pk in (1159,1161,1147,1220,1094,1127,1125,1126,1173,1172,1198,1209,1213,1155,1153,1131,1132,1108,1107,973,971,460,233,227,720,867) order by drugname asc");
            DataTable dataTable1 = objRptFields.ReturnDatatableQuery(sqlQuery1);
            rcbPreSelectedDrugs.DataTextField = "DrugName";
            rcbPreSelectedDrugs.DataValueField = "Drug_pk";
            rcbPreSelectedDrugs.DataSource = dataTable1;
            rcbPreSelectedDrugs.DataBind();

        }
        public void BindAutoCompleteDrug()
        {

            string sqlQuery;
            IDrug objRptFields;
            objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
            sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0");
            DataTable dataTable = objRptFields.ReturnDatatableQuery(sqlQuery);
            Autoselectdrug.DataTextField = "DrugName";
            Autoselectdrug.DataValueField = "Drug_pk";
            Autoselectdrug.DataSource = dataTable;
            Autoselectdrug.DataBind();





        }
        protected void Autoselectdrug_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {

            string strptn_pk = Autoselectdrug.Entries[0].Value;
            BindGridWithData(strptn_pk);
            Autoselectdrug.Entries.Clear();

        }
        public void BindGridWithData(string ptnpk)
        {
            try
            {
                if (ViewState["TableDrug"] == null)
                {
                    theDT = CreateDrugTable();
                }
                else
                {
                    theDT = (DataTable)ViewState["TableDrug"];
                }
                //---------Order-----------
                if (ViewState["TableOrder"] == null)
                {
                    theOrder = CreateOrderTable();
                }
                else
                {
                    theOrder = (DataTable)ViewState["TableOrder"];
                }
                //--------------Dispense ------------
                if (ViewState["TableDispense"] == null)
                {
                    theDispense = CreateDispensTable();
                }
                else
                {
                    theDispense = (DataTable)ViewState["TableDispense"];
                }
                //------------Refill
                if (ViewState["TableRefill"] == null)
                {
                    theRefill = CreateRefillTable();
                }
                else
                {
                    theRefill = (DataTable)ViewState["TableRefill"];
                }

                DataSet theAutoDS = (DataSet)Session["MasterData"];
                DataRow[] result = theAutoDS.Tables[33].Select("drug_pk=" + ptnpk + "");

                DataRow[] findRow = theDT.Select("DrugID=" + ptnpk + "");
                int len = findRow.Length;
                if (len == 0)
                {
                    foreach (DataRow row in result)
                    {

                        DataRow DR = theDT.NewRow();
                        DR["DrugID"] = row["drug_pk"];
                        DR["DrugName"] = row["drugname"];
                        DR["DrugType"] = row["drugtypename"];
                        DR["DispensingUnit"] = row["Dispensing Unit"];
                        DR["GenericID"] = row["GenericID"];
                        theDT.Rows.Add(DR);
                        //--------order
                        DataRow DR1 = theOrder.NewRow();
                        DR1["DrugID"] = row["drug_pk"];
                        DR1["GenericID"] = row["GenericID"];
                        theOrder.Rows.Add(DR1);
                        //---------dispense
                        DataRow DR2 = theDispense.NewRow();
                        DR2["DrugID"] = row["drug_pk"];
                        theDispense.Rows.Add(DR2);

                        //Refill
                        DataRow DR3 = theRefill.NewRow();
                        DR3["DrugID"] = row["drug_pk"];
                        theRefill.Rows.Add(DR3);
                    }
                }
                ViewState["TableDrug"] = theDT;
                ViewState["TableOrder"] = theOrder;
                ViewState["TableDispense"] = theDispense;
                ViewState["TableRefill"] = theRefill;
                BindDrugGrid();
            }
            catch { }
        }
        public void BindDrugGrid()
        {

            if (((DataTable)ViewState["TableDrug"]).Rows.Count > 0)
            {
                rgdrugmain.DataSource = (DataTable)ViewState["TableDrug"];
                rgdrugmain.DataBind();
            }


        }
        protected void rgdrugmain_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridNestedViewItem)
            {

                e.Item.FindControl("InnerContainer").Visible = ((GridNestedViewItem)e.Item).ParentItem.Expanded;
                RadGrid OrderGrid = (RadGrid)e.Item.FindControl("OrdersGrid");
                OrderGrid.ItemCreated += new GridItemEventHandler(OrderGrid_ItemCreated);
                OrderGrid.ItemDataBound += new GridItemEventHandler(OrderGrid_ItemDataBound);

                RadGrid DispenseGrid = (RadGrid)e.Item.FindControl("Dispense");
                DispenseGrid.ItemDataBound += new GridItemEventHandler(DispenseGrid_ItemDataBound);

                RadGrid gridRefill = (RadGrid)e.Item.FindControl("Refill");
                gridRefill.ItemCreated += new GridItemEventHandler(gridRefill_ItemCreated);
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);




            }
        }
        protected void OrderGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadNumericTextBox txtdose = (RadNumericTextBox)e.Item.FindControl("txtDose");
                RadNumericTextBox txtduration = (RadNumericTextBox)e.Item.FindControl("txtDuration");
                RadComboBox ddfrequency = (RadComboBox)e.Item.FindControl("rdcmbfrequency");
                RadNumericTextBox txtPrescribed = (RadNumericTextBox)e.Item.FindControl("txtQtyPrescribed");
                txtduration.Attributes.Add("onblur", "CalculateTotalDailyDose('" + txtdose.ClientID + "','" + txtduration.ClientID + "','" + ddfrequency.ClientID + "','" + txtPrescribed.ClientID + "')");
            }
        }
        protected void DispenseGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadNumericTextBox txtqtydispensed = (RadNumericTextBox)e.Item.FindControl("txtQtyDispensed");
                RadNumericTextBox txtqtyprec = (RadNumericTextBox)e.Item.FindControl("txtQtyPrescribeddispense");
                txtqtydispensed.Attributes.Add("onblur", "return Validate('" + txtqtydispensed.ClientID + "','" + txtqtyprec.ClientID + "')");
            }
        }
        protected void Dispense_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            DataView dv = new DataView((DataTable)ViewState["TableDispense"]);
            dv.RowFilter = "DrugID=" + parentItem.GetDataKeyValue("DrugID").ToString() + "";
            DataTable dtfilter = dv.ToTable();
            (sender as RadGrid).DataSource = dtfilter;
        }
        protected void OrdersGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            DataView dv = new DataView((DataTable)ViewState["TableOrder"]);
            dv.RowFilter = "DrugID=" + parentItem.GetDataKeyValue("DrugID").ToString() + "";
            DataTable dtfilter = dv.ToTable();
            (sender as RadGrid).DataSource = dtfilter;
        }
        protected void Refill_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            DataView dv = new DataView((DataTable)ViewState["TableRefill"]);
            dv.RowFilter = "DrugID=" + parentItem.GetDataKeyValue("DrugID").ToString() + "";
            DataTable dtfilter = dv.ToTable();
            (sender as RadGrid).DataSource = dtfilter;
        }
        protected void gridRefill_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadDatePicker dtrefillexpiration = (RadDatePicker)e.Item.FindControl("dtRefillExpiration");
                if (DataBinder.Eval(e.Item.DataItem, "RefillExpiration") != null)
                {
                    dtrefillexpiration.DbSelectedDate = DataBinder.Eval(e.Item.DataItem, "RefillExpiration").ToString();
                }

            }
        }
        protected void OrderGrid_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

            if (e.Item is GridDataItem)
            {
                RadComboBox combo = ((RadComboBox)e.Item.FindControl("rdcmbfrequency"));
                theBind.BindCombo(combo, (DataTable)Session["Frequency"], "FrequencyName", "FrequencyId");
                if (DataBinder.Eval(e.Item.DataItem, "Frequency") != null)
                {
                    combo.SelectedValue = DataBinder.Eval(e.Item.DataItem, "Frequency").ToString();
                }
                RadButton chkprop = ((RadButton)e.Item.FindControl("chkProphylaxis"));
                if (DataBinder.Eval(e.Item.DataItem, "Prophylaxis") != null)
                {
                    if (DataBinder.Eval(e.Item.DataItem, "Prophylaxis").ToString() == "1")
                    {
                        chkprop.SetSelectedToggleStateByText("Yes");
                    }

                }
            }
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
        }
        protected void rgdrugmain_ItemCommand(object sender, GridCommandEventArgs e)
        {


            if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            {
                ((GridDataItem)e.Item).ChildItem.FindControl("InnerContainer").Visible =
                    !e.Item.Expanded;
            }

            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                GridDataItem parentItem = e.Item as GridDataItem;
                RadGrid griddispense = parentItem.ChildItem.FindControl("Dispense") as RadGrid;
                DataTable dtDispense = (DataTable)ViewState["TableDispense"];
                foreach (GridDataItem item in griddispense.Items)
                {
                    string drugID = item.GetDataKeyValue("DrugID").ToString();

                    RadNumericTextBox txtqtydispensed = (RadNumericTextBox)item.FindControl("txtQtyDispensed");
                    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribeddispense");
                    for (int i = 0; i < dtDispense.Rows.Count; i++)
                    {
                        if (dtDispense.Rows[i]["DrugID"].ToString() == drugID)
                        {

                            if (txtqtydispensed.Text != "")
                            {
                                dtDispense.Rows[i]["QtyDispensed"] = Convert.ToDecimal(txtqtydispensed.Text);
                            }
                            if (txtqtyprec.Text != "")
                            {
                                dtDispense.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);
                            }
                        }

                    }
                }
                ViewState["TableDispense"] = dtDispense;
                griddispense.Rebind();

                RadGrid gridorder = parentItem.ChildItem.FindControl("OrdersGrid") as RadGrid;
                DataTable dtupdateOrder = (DataTable)ViewState["TableOrder"];
                foreach (GridDataItem item in gridorder.Items)
                {
                    string drugID = item.GetDataKeyValue("DrugID").ToString();
                    RadNumericTextBox txtdose = (RadNumericTextBox)item.FindControl("txtDose");
                    RadComboBox ddlfrequency = (RadComboBox)item.FindControl("rdcmbfrequency");
                    RadNumericTextBox txtduration = (RadNumericTextBox)item.FindControl("txtDuration");
                    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribed");
                    RadButton chkProphylaxis = (RadButton)item.FindControl("chkProphylaxis");


                    for (int i = 0; i < dtupdateOrder.Rows.Count; i++)
                    {
                        if (dtupdateOrder.Rows[i]["DrugID"].ToString() == drugID)
                        {
                            if (txtdose.Text != "")
                            {
                                dtupdateOrder.Rows[i]["Dose"] = Convert.ToDecimal(txtdose.Text);
                            }
                            if (ddlfrequency.SelectedItem.Value.ToString() != "")
                            {
                                dtupdateOrder.Rows[i]["Frequency"] = Convert.ToInt32(ddlfrequency.SelectedItem.Value);
                            }
                            if (txtduration.Text != "")
                            {
                                dtupdateOrder.Rows[i]["Duration"] = Convert.ToDecimal(txtduration.Text);
                            }
                            if (txtqtyprec.Text != "")
                            {
                                dtupdateOrder.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);
                            }
                            dtupdateOrder.Rows[i]["Prophylaxis"] = CheckedVaue(chkProphylaxis.SelectedToggleState.Text);

                            dtupdateOrder.AcceptChanges();

                        }
                    }

                }
                ViewState["TableOrder"] = dtupdateOrder;
                gridorder.Rebind();

                //-----------------Refill
                RadGrid gridRefill = parentItem.ChildItem.FindControl("Refill") as RadGrid;
                DataTable dtRefill = (DataTable)ViewState["TableRefill"];
                foreach (GridDataItem item in gridRefill.Items)
                {
                    string drugID = item.GetDataKeyValue("DrugID").ToString();

                    RadNumericTextBox txtnooffile = (RadNumericTextBox)item.FindControl("txtRefill");
                    RadDatePicker dtrefillexpiration = (RadDatePicker)item.FindControl("dtRefillExpiration");
                    for (int i = 0; i < dtRefill.Rows.Count; i++)
                    {
                        if (dtRefill.Rows[i]["DrugID"].ToString() == drugID)
                        {

                            if (txtnooffile.Text != "")
                            {
                                dtRefill.Rows[i]["Refill"] = Convert.ToInt32(txtnooffile.Text);
                            }
                            if (dtrefillexpiration.SelectedDate != null)
                            {
                                if (dtrefillexpiration.SelectedDate.Value.ToString() != "")
                                {
                                    dtRefill.Rows[i]["RefillExpiration"] = dtrefillexpiration.SelectedDate.Value.ToString();
                                }
                            }
                        }

                    }
                }
                ViewState["TableRefill"] = dtRefill;
                gridRefill.Rebind();
            }
            foreach (GridNestedViewItem item in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
            {
                RadTabStrip TabStrip = (RadTabStrip)item.FindControl("TabStip1");
                if (Session["Orderid"].ToString() == "0")
                {
                    TabStrip.Tabs[2].Enabled = false;
                    TabStrip.Tabs[0].Focus();


                    RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
                    radpage.SelectedIndex = 0;
                }
                if (ViewState["RefillClick"].ToString() == "True")
                {
                    TabStrip.Tabs[1].Enabled = false;
                    RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
                    radpage.SelectedIndex = 0;
                }
            }
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
        }
        private void BindControls()
        {
            string sqlQuery;
            string sqlQuery1;
            string sqlQuery2;
            string sqlQuery3;
            string sqlQuery4;
            string sqlQuery5;

            IDrug objRptFields;
            objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
            sqlQuery = string.Format("SELECT ID,Name FROM mst_RegimenLine where DeleteFlag=0");
            sqlQuery1 = string.Format("SELECT EmployeeId,FirstName FROM Mst_Employee where DeleteFlag=0");
            sqlQuery2 = string.Format("SELECT ID,name,DeleteFlag from mst_Decode  where codeId = 33 order by SRNO asc");
            sqlQuery3 = string.Format("SELECT ID,name,DeleteFlag from mst_Provider order by SRNO asc");
            sqlQuery4 = string.Format("select ID,Name from mst_decode where codeID=(select CodeID from mst_code where Name='Pharmacy Period Taken') and (DeleteFlag=0 or Deleteflag is null)");
            sqlQuery5 = string.Format("select ID,Name from mst_decode where CodeId=26 and (DeleteFlag=0 or DeleteFlag IS NULL)");

            DataTable dataTable = objRptFields.ReturnDatatableQuery(sqlQuery);
            DataTable dataTable1 = objRptFields.ReturnDatatableQuery(sqlQuery1);
            DataTable dt = objRptFields.ReturnDatatableQuery(sqlQuery2);
            DataTable dtprovider = objRptFields.ReturnDatatableQuery(sqlQuery3);
            DataTable dtPerid = objRptFields.ReturnDatatableQuery(sqlQuery4);
            DataTable dtreason = objRptFields.ReturnDatatableQuery(sqlQuery5);

            theBind.BindCombo(ddregimenline, dataTable, "Name", "ID");
            theBind.BindCombo(rcbprescribed, dataTable1, "FirstName", "EmployeeId");
            theBind.BindCombo(rcbdispensed, dataTable1, "FirstName", "EmployeeId");
            theBind.BindCombo(rcbtreatment, dt, "Name", "ID");
            theBind.BindCombo(rcbdrugprovider, dtprovider, "Name", "ID");
            theBind.BindCombo(rcbperiod, dtPerid, "Name", "ID");
            theBind.BindCombo(rcbreason, dtreason, "Name", "ID");





        }
        protected void rdbpaediatric_Click(object sender, EventArgs e)
        {

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            var collection = rcbPreSelectedDrugs.CheckedItems;
            string strptn_pk = "";
            if (collection.Count != 0)
            {
                foreach (var item in collection)
                {
                    strptn_pk = item.Value;
                    BindGridWithData(strptn_pk);
                }
            }

            updtPharmdrugs.Update();


           // RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

        }
        protected void rgdrugmain_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DrugID"].ToString();
            DataTable table = (DataTable)ViewState["TableDrug"];
            if (table.Rows.Find(ID) != null)
            {
                table.Rows.Find(ID).Delete();
                table.AcceptChanges();
                ViewState["TableDrug"] = table;


            }
            DataTable dtorder = (DataTable)ViewState["TableOrder"];
            if (dtorder.Rows.Find(ID) != null)
            {
                dtorder.Rows.Find(ID).Delete();
                dtorder.AcceptChanges();
                ViewState["TableOrder"] = dtorder;
            }
            DataTable dtdispense = (DataTable)ViewState["TableDispense"];
            if (dtdispense.Rows.Find(ID) != null)
            {
                dtdispense.Rows.Find(ID).Delete();
                dtdispense.AcceptChanges();
                ViewState["TableDispense"] = dtdispense;
            }
            DataTable dtRefill = (DataTable)ViewState["TableRefill"];
            if (dtRefill.Rows.Find(ID) != null)
            {
                dtRefill.Rows.Find(ID).Delete();
                dtRefill.AcceptChanges();
                ViewState["TableRefill"] = dtRefill;
            }
            if (((DataTable)ViewState["TableDrug"]).Rows.Count > 0)
            {
                rgdrugmain.DataSource = (DataTable)ViewState["TableDrug"];
                rgdrugmain.DataBind();
            }

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
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
        private Boolean FieldValidation()
        {

            //
            IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
            IQCareUtils theUtils = new IQCareUtils();
            lblmessage.Text = "";
            if (ZZZPharmWeight.Text.Trim() != "")
            {
                if (Convert.ToDecimal(ZZZPharmWeight.Text.ToString()) <= 0)
                {
                    lblmessage.Text = "Weight should be greater than 0.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                    return false;
                    
                }
            }


            if (ZZZPharmHeight.Text.Trim() != "")
            {
                if (Convert.ToDecimal(ZZZPharmHeight.Text.ToString()) <= 0)
                {
                    lblmessage.Text = "Height should be greater than 0.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                    return false;
                    
                }
            }

            if (rcbtreatment.SelectedIndex == 0)
            {
                lblmessage.Text = "Please select Treatment Program";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                return false;
            }
            if(rcbdrugprovider.SelectedIndex==0)
            {
                lblmessage.Text = "Please select Drug Provider";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                return false;
            }
            
            if (rcbprescribed.SelectedIndex == 0)
            {
                lblmessage.Text="Prescribed by is not selected.";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                
                return false;
            }

            if (prescribedbydate.SelectedDate==null)
            {
                lblmessage.Text="Prescription Date cannot be blank.";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                return false;
                
            }
            if (ViewState["RefillClick"].ToString() != "True")
            {
                if (Session["Orderid"].ToString() != "0")
                {
                    if (rcbdispensed.SelectedIndex == 0)
                    {
                        lblmessage.Text="Dispensed by is not selected.";
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                        return false;
                    }

                    if (dispensedbydate.SelectedDate==null)
                    {
                        lblmessage.Text = "Dispensed Date cannot be blank.";
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                        return false;
                    }
                }
            }
            if (((prescribedbydate.SelectedDate != null) && (dispensedbydate.SelectedDate!= null)))
            {
                if (((prescribedbydate.SelectedDate.Value.ToString() != "") && (dispensedbydate.SelectedDate.Value.ToString() != "")))
                {
                    string theOrdByDate = Convert.ToDateTime(prescribedbydate.SelectedDate.Value).ToShortDateString();
                    string theDispByDate = Convert.ToDateTime(dispensedbydate.SelectedDate.Value).ToShortDateString();
                    if (Convert.ToDateTime(theOrdByDate) > Convert.ToDateTime(theDispByDate))
                    {
                        lblmessage.Text = "Ordered by date cannot be greater than the dispensed by date.";
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                        return false;
                    }
                }
            }
            // validate grid
            int count = rgdrugmain.MasterTableView.Items.Count;
            if (count.ToString() == "0")
            {
                lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                return false;
            }
            foreach (GridNestedViewItem nestedView in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
            {
                RadGrid gridOrdersGrid = (RadGrid)nestedView.FindControl("OrdersGrid");
                if (gridOrdersGrid.Items.Count == 0)
                {
                    lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                    return false;
                }
                foreach (GridDataItem item in gridOrdersGrid.Items)
                {

                    string drugID = item.GetDataKeyValue("DrugID").ToString();
                    RadNumericTextBox txtdose = (RadNumericTextBox)item.FindControl("txtDose");
                    RadComboBox ddlfrequency = (RadComboBox)item.FindControl("rdcmbfrequency");
                    RadNumericTextBox txtduration = (RadNumericTextBox)item.FindControl("txtDuration");
                    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribed");
                    RadButton chkProphylaxis = (RadButton)item.FindControl("chkProphylaxis");

                    if (txtdose.Text == "")
                    {
                        lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                        return false;
                    }
                    if (ddlfrequency.SelectedItem.Value.ToString() == "0")
                    {
                        lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                        return false;
                    }
                    if (txtduration.Text == "")
                    {
                        lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                        return false;
                    }
                    if (txtqtyprec.Text == "")
                    {
                        lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                        return false;
                    }
                }
                


                RadGrid gridDispense = (RadGrid)nestedView.FindControl("Dispense");
                if (gridDispense.Items.Count == 0)
                {
                    lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                    return false;
                }
                foreach (GridDataItem item in gridDispense.Items)
                {

                    string drugID = item.GetDataKeyValue("DrugID").ToString();

                    RadNumericTextBox txtqtydispensed = (RadNumericTextBox)item.FindControl("txtQtyDispensed");
                    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribeddispense");
                    if (ViewState["RefillClick"].ToString() != "True")
                    {
                        if (Session["Orderid"].ToString() != "0")
                        {
                            if (txtqtydispensed.Text == "0")
                            {
                                lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                                return false;
                            }
                        }
                    }
                    
                }
                
                //-----------Refill
                RadGrid gridRefill = (RadGrid)nestedView.FindControl("Refill");
             
                foreach (GridDataItem item in gridRefill.Items)
                {

                    string drugID = item.GetDataKeyValue("DrugID").ToString();

                    RadNumericTextBox txtnooffile = (RadNumericTextBox)item.FindControl("txtRefill");
                    RadDatePicker dtrefillexpiration = (RadDatePicker)item.FindControl("dtRefillExpiration");
                    if (txtnooffile.Text != "" && txtnooffile.Text != "0")
                    {
                        if (dtrefillexpiration.SelectedDate == null)
                        {
                            lblmessage.Text = "Incomplete Data for one or more of your drug selections.";
                            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + lblmessage.Text + "');", true);
                            return false;
                        }
                    }
                   
                    
                }
               

            }
           
            
            //end main grid

            //end grid
            return true;
        }
        protected void rdcmbfrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RadComboBox dropdownlist1 = (RadComboBox)sender;
            //GridDataItem item = (GridDataItem)dropdownlist1.NamingContainer;
            //RadNumericTextBox txtdose = (RadNumericTextBox)item.FindControl("txtDose");
            //RadNumericTextBox textbox1 = (RadNumericTextBox)item.FindControl("txtQtyPrescribed");
            //if (dropdownlist1.SelectedItem.Text == "OD" || dropdownlist1.SelectedItem.Text == "BD" || dropdownlist1.SelectedItem.Text == "TID" || dropdownlist1.SelectedItem.Text == "QID")
            //{
            //    textbox1.Visible = false;
            //}
            //else
            //{
            //    textbox1.Visible = true;
            //    textbox1.Text = dropdownlist1.SelectedItem.Text;
            //}
        }
        public void PharmacySave()
        {
            lblmessage.Text = "";
            if (FieldValidation() == false)
            {
                return;
            }

            IPediatric PediatricManager;
            PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            List<IPharmacyFields> PharmacyList = new List<IPharmacyFields>();

            IPharmacyFields objPharmacyFields = new IPharmacyFields();

            List<DrugDetails> objlist = new List<DrugDetails>();


            objPharmacyFields.Ptn_pk = Convert.ToInt32(Request.QueryString["PatientID"]);
            objPharmacyFields.LocationID = Int32.Parse(Session["AppLocationId"].ToString());
            objPharmacyFields.userid = Int32.Parse(Session["AppUserId"].ToString());
            objPharmacyFields.VisitType = 4;
            objPharmacyFields.AppntReason = Convert.ToInt32(rcbreason.SelectedItem.Value);
            objPharmacyFields.TreatmentProgram = Convert.ToInt32(rcbtreatment.SelectedItem.Value);
            objPharmacyFields.PeriodTaken = Convert.ToInt32(rcbperiod.SelectedItem.Value);
            objPharmacyFields.Drugprovider = Convert.ToInt32(rcbdrugprovider.SelectedItem.Value);
            
            if (ViewState["RefillClick"].ToString() == "True")
            {
                objPharmacyFields.ptn_pharmacy_pk_old = Convert.ToInt32(Session["Orderid"].ToString());
                Session["Orderid"] = "0";
            }
            else
            {
                objPharmacyFields.ptn_pharmacy_pk_old = 0;
            }

            objPharmacyFields.ptn_pharmacy_pk = Convert.ToInt32(Session["Orderid"].ToString());
            if (Convert.ToInt32(Session["Orderid"].ToString()) != 0)
            {
                objPharmacyFields.flag = 2;
            }
            else
            {
                objPharmacyFields.flag = 1;
            }
            if (ZZZPharmWeight.Text != "")
            {
                objPharmacyFields.Weight = Convert.ToDecimal(ZZZPharmWeight.Text);
            }
            if (ZZZPharmHeight.Text != "")
            {
                objPharmacyFields.Height = Convert.ToDecimal(ZZZPharmHeight.Text);
            }
            if (ddregimenline.SelectedItem.Value.ToString() != "0")
            {
                objPharmacyFields.RegimenLine = Convert.ToInt32(ddregimenline.SelectedItem.Value);
            }
            if (appdate.SelectedDate.ToString() != "")
            {
                //objPharmacyFields.PharmacyRefillDate = DateGiven(appdate.SelectedDate.ToString());
                objPharmacyFields.PharmacyRefillDate = appdate.SelectedDate.ToString();
            }
            if (PharmNotes.Text != "")
            {
                objPharmacyFields.PharmacyNotes = PharmNotes.Text;
            }
            else
            {
                objPharmacyFields.PharmacyNotes = "";
            }
            if (rcbprescribed.SelectedItem.Value != "0")
            {
                objPharmacyFields.OrderedBy = Convert.ToInt32(rcbprescribed.SelectedItem.Value);
            }
            if (prescribedbydate.SelectedDate.ToString() != "")
            {
                //objPharmacyFields.OrderedByDate = DateGiven(prescribedbydate.SelectedDate.ToString());
                objPharmacyFields.OrderedByDate = prescribedbydate.SelectedDate.ToString();
            }

            if (rcbdispensed.SelectedItem.Value.ToString() != "0")
            {
                objPharmacyFields.DispensedBy = Convert.ToInt32(rcbdispensed.SelectedItem.Value.ToString());
            }
            if (dispensedbydate.SelectedDate.ToString() != "")
            {
                objPharmacyFields.DispensedByDate = dispensedbydate.SelectedDate.ToString();
                //objPharmacyFields.DispensedByDate = DateGiven(dispensedbydate.SelectedDate.ToString());
            }


            //Start main grid
            foreach (GridNestedViewItem nestedView in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
            {
                RadGrid gridOrdersGrid = (RadGrid)nestedView.FindControl("OrdersGrid");
                DataTable dtupdateOrder = (DataTable)ViewState["TableOrder"];
                DrugDetails objDetails = new DrugDetails();
                foreach (GridDataItem item in gridOrdersGrid.Items)
                {

                    string drugID = item.GetDataKeyValue("DrugID").ToString();
                    RadNumericTextBox txtdose = (RadNumericTextBox)item.FindControl("txtDose");
                    RadComboBox ddlfrequency = (RadComboBox)item.FindControl("rdcmbfrequency");
                    RadNumericTextBox txtduration = (RadNumericTextBox)item.FindControl("txtDuration");
                    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribed");
                    RadButton chkProphylaxis = (RadButton)item.FindControl("chkProphylaxis");

                    for (int i = 0; i < dtupdateOrder.Rows.Count; i++)
                    {
                        if (dtupdateOrder.Rows[i]["DrugID"].ToString() == drugID)
                        {
                            objDetails.Drug_Pk = Convert.ToInt32(drugID);
                            objDetails.GenericId = Convert.ToInt32(dtupdateOrder.Rows[i]["GenericID"].ToString());
                            if (txtdose.Text != "")
                            {
                                dtupdateOrder.Rows[i]["Dose"] = Convert.ToDecimal(txtdose.Text);
                                objDetails.SingleDose = Convert.ToDecimal(txtdose.Text);
                            }
                            if (ddlfrequency.SelectedItem.Value.ToString() != "0")
                            {
                                dtupdateOrder.Rows[i]["Frequency"] = Convert.ToInt32(ddlfrequency.SelectedItem.Value);
                                objDetails.FrequencyID = Convert.ToInt32(ddlfrequency.SelectedItem.Value); ;
                            }
                            if (txtduration.Text != "")
                            {
                                dtupdateOrder.Rows[i]["Duration"] = Convert.ToDecimal(txtduration.Text);
                                objDetails.Duration = Convert.ToDecimal(txtduration.Text);
                            }
                            if (txtqtyprec.Text != "")
                            {
                                dtupdateOrder.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);
                                objDetails.OrderedQuantity = Convert.ToDecimal(txtqtyprec.Text);
                            }
                            if (chkProphylaxis.SelectedToggleState.Text == "Yes")
                            {
                                dtupdateOrder.Rows[i]["Prophylaxis"] = 1;
                                objDetails.Prophylaxis = 1;
                            }
                            else
                            {
                                dtupdateOrder.Rows[i]["Prophylaxis"] = 0;
                                objDetails.Prophylaxis = 0;
                            }

                            dtupdateOrder.AcceptChanges();

                        }

                    }

                }
                ViewState["TableOrder"] = dtupdateOrder;


                RadGrid gridDispense = (RadGrid)nestedView.FindControl("Dispense");
                DataTable dtDispense = (DataTable)ViewState["TableDispense"];
                foreach (GridDataItem item in gridDispense.Items)
                {

                    string drugID = item.GetDataKeyValue("DrugID").ToString();

                    RadNumericTextBox txtqtydispensed = (RadNumericTextBox)item.FindControl("txtQtyDispensed");
                    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribeddispense");

                    for (int i = 0; i < dtDispense.Rows.Count; i++)
                    {
                        if (dtDispense.Rows[i]["DrugID"].ToString() == drugID)
                        {

                            if (txtqtydispensed.Text != "")
                            {
                                dtDispense.Rows[i]["QtyDispensed"] = Convert.ToDecimal(txtqtydispensed.Text);
                                objDetails.DispensedQuantity = Convert.ToDecimal(txtqtydispensed.Text);
                            }
                            if (txtqtyprec.Text != "")
                            {
                                dtDispense.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);

                            }
                        }

                    }
                }
                ViewState["TableDispense"] = dtDispense;
                //-----------Refill
                RadGrid gridRefill = (RadGrid)nestedView.FindControl("Refill");
                DataTable dtRefill = (DataTable)ViewState["TableRefill"];
                foreach (GridDataItem item in gridRefill.Items)
                {

                    string drugID = item.GetDataKeyValue("DrugID").ToString();

                    RadNumericTextBox txtnooffile = (RadNumericTextBox)item.FindControl("txtRefill");
                    RadDatePicker dtrefillexpiration = (RadDatePicker)item.FindControl("dtRefillExpiration");

                    for (int i = 0; i < dtRefill.Rows.Count; i++)
                    {
                        if (dtRefill.Rows[i]["DrugID"].ToString() == drugID)
                        {

                            if (txtnooffile.Text != "")
                            {
                                dtRefill.Rows[i]["Refill"] = Convert.ToInt32(txtnooffile.Text);
                                objDetails.refill = Convert.ToInt32(txtnooffile.Text);
                            }
                            if (dtrefillexpiration.SelectedDate != null)
                            {
                                if (dtrefillexpiration.SelectedDate.Value.ToString() != "")
                                {
                                    dtRefill.Rows[i]["RefillExpiration"] = dtrefillexpiration.SelectedDate.Value.ToString();
                                    objDetails.RefillExpiration = DateGiven(dtrefillexpiration.SelectedDate.Value.ToString());
                                }
                            }

                        }

                    }
                }
                ViewState["TableRefill"] = dtRefill;

                objlist.Add(objDetails);

            }
            //end main grid
            objPharmacyFields.Druginfo = objlist;
            PharmacyList.Add(objPharmacyFields);
            bool IsError = false;
            try
            {
                int result = PediatricManager.IQTouchSaveUpdatePharmacy(PharmacyList);
                if (result > 0)
                {
                    
                    //btnSave.Enabled = false;
                    btnrefill.Enabled = false;
                    Session["FormIsLoaded"] = null;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                    PharmacySummaryPage();
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackBtnClick();", true);
                }
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
        protected void RadButton1_Click(object sender, EventArgs e)
        {
            PharmacySave();
        
        }
        protected void PharmacySummaryPage()
        {
            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmKNHPharmacyOrderManagement";

            Touch.Custom_Forms.frmKNHPharmacyOrderManagement fr = (frmKNHPharmacyOrderManagement)mp.LoadControl("~/Touch/Custom Forms/frmKNHPharmacyOrderManagement.ascx");

            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            frmKNHPharmacyOrderManagement theFrm = (frmKNHPharmacyOrderManagement)ph.FindControl("ID" + Session["CurrentFormName"].ToString());
            foreach (Control item in ph.Controls)
            {
                ph.Controls.Remove(item);
                //item.Visible = false;
            }

            if (theFrm != null)
            {
                theFrm.Visible = true;
            }
            else
            {
                ph.Controls.Add(fr);
            }
            ph.DataBind();
            upt.Update();
            mp.ClientScript.RegisterStartupScript(mp.GetType(), "settabschild", "setTabs();");


        }
        protected void btnrefill_Click(object sender, EventArgs e)
        {

            ViewState["RefillClick"] = "True";
            BindExistRefillPaediatricDetails();
          
        }
        protected void PrintPrescription_Click(object sender, EventArgs e)
        {
            if (Session["Visit_id"].ToString() != "")
            {
                int PatientID = Convert.ToInt32(Request.QueryString["PatientID"]);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "drugpopup", "window.open('../Pharmacy/frmprintdialog.aspx?visitID=" + Session["Visit_id"] + "&ptnpk=" + PatientID + "' ,'DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');", true);
            }
        }
    }
}