using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Telerik.Web.UI;
using Interface.Clinical;
using Interface.Security;
using Interface.Pharmacy;
using Application.Common;
using Application.Presentation;
using Interface.Administration;
using System.Xml.Linq;
using System.Linq;

namespace PresentationApp.Pharmacy
{
    public partial class frmPharmacy_Custom : System.Web.UI.Page
    {
        string ObjFactoryPharmacyParameter = "BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy";
        string ObjFactoryParameter = "BusinessProcess.Clinical.BCustomForm, BusinessProcess.Clinical";
        DataSet theDSXML = new DataSet();
        int FeatureID = 0, PatientID = 0, VisitID = 0, LocationID = 0;
        DataSet thePharmacyDS;
        DataTable tbldrug = new DataTable();
        DataTable tblOrder = new DataTable("Table1");
        DataTable tblDispense = new DataTable("Table3");
        DataTable tblRefill = new DataTable("Table2");
        DataTable tblInstruct = new DataTable("Table4");
        DataTable theDT;
        DataTable theOrder;
        DataTable theDispense;
        DataTable theRefill;
        DataTable theInstruct;
        static int RegimenLineId = 0;
        static int WeightFieldId = 0;
        static int HeightFieldId = 0;
        static int BMIFieldId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AppLocation"] == null || Session.Count == 0 || Session["AppUserID"].ToString() == "")
            {
                IQCareMsgBox.Show("SessionExpired", this);
                Response.Redirect("~/frmlogin.aspx",true);
            }

            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Text = "Pharmacy Forms >> ";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "Pharmacy Form";
            (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblformname") as Label).Text = "Pharmacy Form";

            if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                FeatureID = Convert.ToInt32(Session["FeatureID"]);
            }
            else
            {
                FeatureID = Convert.ToInt32(Session["FeatureID"]);
            }
            PatientID = Convert.ToInt32(Session["PatientId"]);
            LocationID = Convert.ToInt32(Session["ServiceLocationId"]);
            VisitID = Convert.ToInt32(Session["PatientVisitId"]);


            LoadPredefinedLabel_Field(FeatureID, PatientID);
            if (!IsPostBack)
            {
                BindAutoCompleteDrug();
            }
            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                Session["Orderid"] = 0;
                if (Session["Paperless"].ToString() == "0")
                {
                    if (Session["SCMModule"] == null)
                    {
                        ddlPharmDispensedbyName.Enabled = true;
                        txtpharmdispensedbydate.Disabled = false;
                    }
                }
                else
                {
                    ddlPharmDispensedbyName.Enabled = false;
                    txtpharmdispensedbydate.Disabled = true;
                }

            }
            else if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                ddlPharmOrderedbyName.Enabled = false;
                txtpharmOrderedbyDate.Disabled = true;
                Session["Orderid"] = 1;
                if (!IsPostBack)
                {
                    BindValue(PatientID, VisitID, LocationID, PnlDynamicElements);
                }
            }
            if ((WeightFieldId == 303) && (HeightFieldId == 304) && (BMIFieldId == 305))
            {
                ClientScriptManager cs = Page.ClientScript;
                String yourScript = "CalcualteBMIGet();";
                cs.RegisterStartupScript(this.GetType(), "key script", yourScript, true);
            }
        }
        #region "RadAutoComplete DataBind"
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static AutoCompleteBoxData GetDrugNames(object context)
        {
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
            //frmPharmacy_Custom cus = new frmPharmacy_Custom();
            //DataTable data = cus.GetChildNodes(searchString);
            DataView dvRadAuto = ((DataTable)HttpContext.Current.Session["RadAutoDrugs"]).DefaultView;
            dvRadAuto.RowFilter = "Drugname like '%" + searchString + "%'";
            DataTable data = dvRadAuto.ToTable();
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();
            foreach (DataRow row in data.Rows)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                childNode.Text = row["DrugName"].ToString();
                childNode.Value = row["Drug_pk"].ToString();
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();

            return res;
        }
        private DataTable GetChildNodes(string searchString)
        {
            DataView dvRadAuto = ((DataTable)Session["RadAutoDrugs"]).DefaultView;
            dvRadAuto.RowFilter = "Drugname like '%" + searchString + "%'";
            DataTable dtFilter = dvRadAuto.ToTable();
            return dtFilter;
        }
        #endregion
        private void BindValue(int PatientID, int VisitID, int LocationID, Control theControl)
        {
            IIQCareSystem IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            DateTime theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
            ICustomForm MgrBindValue = (ICustomForm)ObjectFactory.CreateInstance(ObjFactoryParameter);
            DataTable theDT = SetControlIDs(theControl);
            //DataTable TempDT = ((DataTable)ViewState["LnkTable"]).DefaultView.ToTable(true, "PDFTableName").Copy();
            DataTable TempDT = theDT.DefaultView.ToTable(true, "TableName").Copy();

            String GetVisitDate = "Select VisitDate, Signature,DataQuality from ord_visit where Ptn_Pk=" + PatientID + " and Visit_Id=" + VisitID + " and LocationID=" + LocationID + "";
            GetVisitDate = GetVisitDate + " Select OrderedBy, OrderedByDate,DispensedBy,DispensedByDate from ord_patientpharmacyorder where Ptn_Pk=" + PatientID + " and VisitId=" + VisitID + " and LocationID=" + LocationID + "";
            GetVisitDate = GetVisitDate + " Select * from dtl_PatientPharmacyOrder PD inner join Dtl_PatientBillTransaction PB on PD.ptn_pharmacy_pk=PB.PharmacyId and PD.Drug_Pk=PB.ItemId where PD.ptn_pharmacy_pk = (Select ptn_pharmacy_pk from ord_patientpharmacyorder where Ptn_Pk=" + PatientID + " and VisitId=" + VisitID + " and LocationID=" + LocationID + ")";
            DataSet theDS = new DataSet();
            DataSet TmpDS = MgrBindValue.Common_GetSaveUpdate(GetVisitDate);
            ddlPharmOrderedbyName.SelectedValue = TmpDS.Tables[1].Rows[0]["OrderedBy"].ToString();
            ddlPharmDispensedbyName.SelectedValue = TmpDS.Tables[1].Rows[0]["DispensedBy"].ToString();
            txtpharmOrderedbyDate.Value = String.Format("{0:dd-MMM-yyyy}", TmpDS.Tables[1].Rows[0]["OrderedByDate"]);
            if (String.Format("{0:dd-MMM-yyyy}", TmpDS.Tables[1].Rows[0]["DispensedByDate"]) == "01-Jan-1900")
            {
                txtpharmdispensedbydate.Value = "";
            }
            else
            {
                txtpharmdispensedbydate.Value = String.Format("{0:dd-MMM-yyyy}", TmpDS.Tables[1].Rows[0]["DispensedByDate"]);
            }
            try
            {
                StringBuilder SBGetValue = new StringBuilder();
                foreach (DataRow TempDR in TempDT.Rows)
                {
                    if (TempDR["TableName"].ToString().ToUpper() == "DTL_CUSTOMFIELD")
                    {
                        DataTable theDTNoMulti = ((DataTable)ViewState["NoMulti"]);
                        string TableName = "DTL_FBCUSTOMFIELD_" + theDTNoMulti.Rows[0][1].ToString().Replace(' ', '_');
                        SBGetValue.Append("Select * from [" + TableName + "] where Ptn_pk=" + PatientID + " and Visit_Pk=" + VisitID + " and LocationId=" + LocationID + ";");
                    }
                    else if (Convert.ToString(TempDR["TableName"]).ToUpper() == "ord_patientpharmacyorder".ToUpper() || Convert.ToString(TempDR["TableName"]).ToUpper() == "Dtl_PatientBillTransaction".ToUpper())
                    {
                        SBGetValue.Append("Select * from [" + TempDR["TableName"] + "] where Ptn_pk=" + PatientID + " and Visitid=" + VisitID + " and LocationId=" + LocationID + ";");
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "AGE")
                    {
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "DOB")
                    {
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "BSA")
                    {
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "PRINTPRESCRIPTION")
                    {
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "PRINTLABEL")
                    {
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "LASTDISPENSEDREGIMEN")
                    {
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "LASTDISPENSEDDATE")
                    {
                    }
                    else if (TempDR["TableName"].ToString().ToUpper() == "DTL_PATIENTPHARMACYRETURN")
                    {

                    }
                    else
                    {
                        SBGetValue.Append("Select * from [" + TempDR["TableName"] + "] where Ptn_pk=" + PatientID + " and Visit_Pk=" + VisitID + " and LocationId=" + LocationID + ";");
                    }
                }
                DataSet TempDSValue = new DataSet();
                if (!String.IsNullOrEmpty(SBGetValue.ToString()))
                {
                    TempDSValue = MgrBindValue.Common_GetSaveUpdate(SBGetValue.ToString());
                }
                DataTable theBUssDT = (DataTable)ViewState["BusRule"];
                foreach (DataRow TempDR in TempDT.Rows)
                {
                    if (TempDSValue.Tables.Count > 0)
                    {
                        for (int n = 0; n < TempDSValue.Tables.Count; n++)
                        {
                            for (int i = 0; i <= TempDSValue.Tables[n].Columns.Count - 1; i++)
                            {
                                foreach (object x in theControl.Controls)
                                {
                                    if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                                    {
                                        string[] remStr = ((TextBox)x).ID.Split('-');
                                        string str = remStr[0] + "-" + remStr[1] + "-" + remStr[2];
                                        if ("TXTMulti-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                        {
                                            if (TempDSValue.Tables[n].Rows.Count > 0)
                                            {
                                                ((TextBox)x).Text = Convert.ToString(TempDSValue.Tables[n].Rows[0][i]);
                                            }
                                        }
                                        if ("TXTSingle-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                        {
                                            if (TempDSValue.Tables[n].Rows.Count > 0)
                                            {
                                                ((TextBox)x).Text = Convert.ToString(TempDSValue.Tables[n].Rows[0][i]);
                                            }
                                        }

                                        if ("TXT-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                        {
                                            if (TempDSValue.Tables[n].Rows.Count > 0)
                                            {
                                                ((TextBox)x).Text = Convert.ToString(TempDSValue.Tables[n].Rows[0][i]);

                                                if (((TextBox)x).Text != "")
                                                {
                                                    ((TextBox)x).ForeColor = System.Drawing.Color.Black;
                                                }
                                            }
                                        }

                                        if ("TXTNUM-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                        {
                                            if (TempDSValue.Tables[n].Rows.Count > 0)
                                            {
                                                ((TextBox)x).Text = Convert.ToString(TempDSValue.Tables[n].Rows[0][i]);
                                                if (((TextBox)x).Text != "")
                                                {
                                                    ((TextBox)x).ForeColor = System.Drawing.Color.Black;
                                                }
                                            }
                                        }

                                        if ("TXTDT-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                        {
                                            if (TempDSValue.Tables[n].Rows.Count > 0)
                                            {
                                                //Date format for like "MMM-yyyy"
                                                DataView dvBusDtl = theBUssDT.DefaultView;
                                                dvBusDtl.RowFilter = "BusRuleId = 21";
                                                DataTable dtFilter = dvBusDtl.ToTable();
                                                bool isDateValidate = true;
                                                if (dtFilter.Rows.Count > 0)
                                                {
                                                    for (int rowcount = 0; rowcount < dtFilter.Rows.Count; rowcount++)
                                                    {
                                                        if (dtFilter.Rows[rowcount]["FieldName"].ToString().Trim() == TempDSValue.Tables[n].Columns[i].ToString().Trim() && dtFilter.Rows[rowcount]["TableName"].ToString().Trim() == TempDR["TableName"].ToString().Trim())
                                                        {
                                                            isDateValidate = false;
                                                            ((TextBox)x).Text = String.Format("{0:MMM-yyyy}", TempDSValue.Tables[n].Rows[0][i]);
                                                        }
                                                        else
                                                        {
                                                            if (isDateValidate == true)
                                                            {
                                                                ((TextBox)x).Text = String.Format("{0:dd-MMM-yyyy}", TempDSValue.Tables[n].Rows[0][i]);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ((TextBox)x).Text = String.Format("{0:dd-MMM-yyyy}", TempDSValue.Tables[n].Rows[0][i]);
                                                }
                                            }
                                        }

                                    }

                                    else if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                                    {
                                        string[] remStr = ((DropDownList)x).ID.Split('-');
                                        string a = "SELECTLIST-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"];
                                        string str = remStr[0] + "-" + remStr[1] + "-" + remStr[2];
                                        if ("SELECTLIST-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                        {
                                            if (TempDSValue.Tables[n].Rows.Count > 0)
                                            {
                                                ((DropDownList)x).SelectedValue = Convert.ToString(TempDSValue.Tables[n].Rows[0][i]);
                                            }
                                        }
                                    }
                                    else if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                                    {
                                        string[] remStr = ((HtmlInputRadioButton)x).ID.Split('-');
                                        string str = remStr[0] + "-" + remStr[1] + "-" + remStr[2];
                                        if (TempDSValue.Tables[n].Columns[i].ToString() == ((HtmlInputRadioButton)x).Name)
                                        {
                                            for (int k = 0; k < TempDSValue.Tables[n].Rows.Count; k++)
                                            {
                                                if (TempDSValue.Tables[n].Rows[k][TempDSValue.Tables[n].Columns[i]].ToString() == "True" || TempDSValue.Tables[n].Rows[k][TempDSValue.Tables[n].Columns[i]].ToString() == "1")
                                                {
                                                    if ("RADIO1-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                                    {
                                                        ((HtmlInputRadioButton)x).Checked = true;
                                                    }
                                                }
                                                else if (TempDSValue.Tables[n].Rows[k][TempDSValue.Tables[n].Columns[i]].ToString() == "False" || TempDSValue.Tables[n].Rows[k][TempDSValue.Tables[n].Columns[i]].ToString() == "0")
                                                {
                                                    if ("RADIO2-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                                    {
                                                        ((HtmlInputRadioButton)x).Checked = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputCheckBox))
                                    {
                                        string[] remStr = ((HtmlInputCheckBox)x).ID.Split('-');
                                        string str = remStr[0] + "-" + remStr[1] + "-" + remStr[2];
                                        if ("Chk-" + TempDSValue.Tables[n].Columns[i].ToString() + "-" + TempDR["TableName"] == str)
                                        {
                                            for (int k = 0; k < TempDSValue.Tables[n].Rows.Count; k++)
                                            {
                                                if (TempDSValue.Tables[n].Rows[k][TempDSValue.Tables[n].Columns[i]].ToString() == "True")
                                                {
                                                    ((HtmlInputCheckBox)x).Checked = true;
                                                }
                                                else { ((HtmlInputCheckBox)x).Checked = false; }
                                            }
                                        }
                                    }

                                    else if (x.GetType() == typeof(System.Web.UI.WebControls.Panel))
                                    {
                                        foreach (Control z in ((Control)x).Controls)
                                        {
                                            if (z.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                                            {
                                                StringBuilder SBMultiGetValue = new StringBuilder();
                                                DataSet theDSMulti = new DataSet();
                                                DataView DVMulti = new DataView(((DataTable)ViewState["LnkTable"]));
                                                DVMulti.RowFilter = "ControlId=9";
                                                foreach (DataRow theDR in DVMulti.ToTable().Rows)
                                                {
                                                    string TableName = "DTL_FB_" + theDR["FieldName"].ToString().Replace(' ', '_');
                                                    SBMultiGetValue.Append("Select * from [" + TableName + "] where Ptn_pk=" + PatientID + " and Visit_Pk=" + VisitID + " and LocationId=" + LocationID + ";");
                                                    if (!String.IsNullOrEmpty(SBGetValue.ToString()))
                                                    {
                                                        theDSMulti = MgrBindValue.Common_GetSaveUpdate(SBMultiGetValue.ToString());
                                                    }
                                                    foreach (DataRow DRMulti in theDSMulti.Tables[0].Rows)
                                                    {
                                                        string[] remStr = ((CheckBox)z).ID.Split('-');
                                                        string str = remStr[0] + "-" + remStr[1] + "-" + remStr[2];
                                                        if ("CHKMULTI-" + DRMulti[2] + "-" + theDR["FieldName"] == str)
                                                        {
                                                            ((CheckBox)z).Checked = true;
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
                ViewState["TableDrug"] = null;
                ViewState["TableOrder"] = null;
                ViewState["TableDispense"] = null;
                ViewState["TableRefill"] = null;
                ViewState["TableInstruct"] = null;
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

                if (ViewState["TableInstruct"] == null)
                {
                    theInstruct = CreateInstructTable();
                }
                else
                {
                    theInstruct = (DataTable)ViewState["TableInstruct"];
                }
                foreach (DataRow theDR in TmpDS.Tables[2].Rows)
                {
                    if (theDR["Drug_Pk"] != System.DBNull.Value)
                    {
                        string theExpirationtDate = Convert.ToDateTime(theDR["RefillExpirationdate"]).ToShortDateString();
                        string currentdate = theCurrentDate.ToShortDateString();
                        //if (Convert.ToDateTime(theExpirationtDate) >= Convert.ToDateTime(currentdate))
                        //{
                        DataSet theAutoDS = (DataSet)ViewState["MasterData"];
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
                        }
                        DataRow DR1 = theOrder.NewRow();
                        DR1["DrugID"] = Convert.ToInt32(theDR["drug_pk"].ToString());
                        DR1["GenericID"] = Convert.ToInt32(theDR["GenericId"].ToString());
                        DR1["Dose"] = Convert.ToDecimal(theDR["SingleDose"].ToString());
                        DR1["Frequency"] = Convert.ToInt32(theDR["FrequencyId"].ToString());
                        DR1["Duration"] = Convert.ToDecimal(theDR["Duration"].ToString());
                        DR1["QtyPrescribed"] = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
                        DR1["Prophylaxis"] = Convert.ToInt32(theDR["Prophylaxis"].ToString());
                        DR1["Instructions"] = Convert.ToString(theDR["PatientInstructions"].ToString());
                        theOrder.Rows.Add(DR1);
                        //---------dispense
                        DataRow DR2 = theDispense.NewRow();
                        DR2["DrugID"] = theDR["drug_pk"];
                        DR2["QtyDispensed"] = Convert.ToDecimal(theDR["DispensedQuantity"].ToString());
                        DR2["QtyPrescribed"] = Convert.ToDecimal(theDR["OrderedQuantity"].ToString());
                        DR2["BatchNo"] = Convert.ToInt32(theDR["BatchNo"].ToString());
                        DR2["ExpiryDate"] = String.Format("{0:dd-MMM-yyyy}", theDR["ExpiryDate"]).ToString();
                        DR2["SellPrice"] = Convert.ToDecimal(theDR["SellingPrice"].ToString());
                        DR2["BillAmount"] = Convert.ToDecimal(theDR["BillAmount"].ToString());
                        theDispense.Rows.Add(DR2);

                        // refill
                        DataRow DR3 = theRefill.NewRow();
                        DR3["DrugID"] = theDR["drug_pk"];
                        DR3["Refill"] = Convert.ToDecimal(theDR["Refill"].ToString()); ;
                        DR3["RefillExpiration"] = theDR["RefillExpirationdate"].ToString();
                        theRefill.Rows.Add(DR3);

                        DataRow DR4 = theInstruct.NewRow();
                        DR4["DrugID"] = theDR["drug_pk"];
                        DR4["Instructions"] = Convert.ToString(theDR["PatientInstructions"].ToString());
                        theInstruct.Rows.Add(DR4);

                        // }
                        //}
                    }
                }
                ViewState["TableDrug"] = theDT;
                ViewState["TableOrder"] = theOrder;
                ViewState["TableDispense"] = theDispense;
                ViewState["TableRefill"] = theRefill;
                ViewState["TableInstruct"] = theInstruct;
                //btnSave.Enabled = true;
                BindDrugGrid(0);
            }
            catch (Exception err)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }

        }
        private void SectionHeading(String H2)
        {
            PnlDynamicElements.Controls.Add(new LiteralControl("<br>"));
            PnlDynamicElements.Controls.Add(new LiteralControl("<h2 class='forms' align='left'>" + H2 + "</h2>"));
        }
        private void BindDropdownOrderBy(int EmployeeId)
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
                    if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0 && EmployeeId > 0)
                    {
                        theDV = new DataView(theDT);
                        theDV.RowFilter = "EmployeeId IN(" + Session["AppUserEmployeeId"].ToString() + "," + EmployeeId + ")";
                        if (theDV.Count > 0)
                            theDT = theUtils.CreateTableFromDataView(theDV);
                        BindManager.BindCombo(ddlPharmOrderedbyName, theDT, "EmployeeName", "EmployeeId");
                    }
                    else
                    {
                        theDV = new DataView(theDT);
                        theDV.RowFilter = "EmployeeId =" + Session["AppUserEmployeeId"].ToString();
                        if (theDV.Count > 0)
                            theDT = theUtils.CreateTableFromDataView(theDV);
                        BindManager.BindCombo(ddlPharmOrderedbyName, theDT, "EmployeeName", "EmployeeId");
                    }

                }
            }

        }
        private void BindDropdownReportedBy(int EmployeeId)
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
                    if (Convert.ToInt32(Session["AppUserEmployeeId"]) > 0 && EmployeeId > 0)
                    {
                        theDV = new DataView(theDT);
                        theDV.RowFilter = "EmployeeId IN(" + Session["AppUserEmployeeId"].ToString() + "," + EmployeeId + ")";
                        if (theDV.Count > 0)
                            theDT = theUtils.CreateTableFromDataView(theDV);
                        BindManager.BindCombo(ddlPharmDispensedbyName, theDT, "EmployeeName", "EmployeeId");
                    }
                    else
                    {
                        theDV = new DataView(theDT);
                        theDV.RowFilter = "EmployeeId =" + Session["AppUserEmployeeId"].ToString();
                        if (theDV.Count > 0)
                            theDT = theUtils.CreateTableFromDataView(theDV);
                        BindManager.BindCombo(ddlPharmDispensedbyName, theDT, "EmployeeName", "EmployeeId");

                    }
                }
            }

        }
        private void LoadPredefinedLabel_Field(int FeatureID, int PatientID)
        {
            theDSXML.ReadXml(MapPath("..\\XMLFiles\\AllMasters.con"));
            if (!IsPostBack)
            {
                IPediatric PediatricManager;
                PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
                thePharmacyDS = PediatricManager.GetPediatricFields(PatientID);
                ViewState["MasterData"] = thePharmacyDS;
                Session["Age"] = thePharmacyDS.Tables[6].Rows[0]["Age"];
                if (!IsPostBack)
                {
                    if (VisitID > 0)
                    {
                        BindDropdownOrderBy(0);
                        BindDropdownReportedBy(0);
                    }
                    else
                    {
                        BindDropdownOrderBy(0);
                        BindDropdownReportedBy(0);
                    }
                }

                Session["Frequency"] = thePharmacyDS.Tables[8];

                string ObjFactoryParameterBCustom = "BusinessProcess.Clinical.BCustomForm, BusinessProcess.Clinical";
                ICustomForm IPatientCustomFormMgr = (ICustomForm)ObjectFactory.CreateInstance(ObjFactoryParameterBCustom);
                DataSet theDS = IPatientCustomFormMgr.GetFieldName_and_LabelPharmacy(FeatureID, PatientID);
                Session["AllData"] = theDS;
                ViewState["LnkTable"] = theDS.Tables[1];
                ViewState["BusRule"] = theDS.Tables[2];
                Session["SessionBusRule"] = theDS.Tables[2];

                DataTable DT = theDS.Tables[1].DefaultView.ToTable(true, "SectionID", "SectionName").Copy();
                int Numtds = 2, td = 1;
                PnlDynamicElements.Controls.Clear();
                foreach (DataRow dr in DT.Rows)
                {
                    SectionHeading(dr["SectionName"].ToString());
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table cellspacing='6' cellpadding='0' width='100%' border='0'>"));
                    foreach (DataRow DRLnkTable in theDS.Tables[1].Rows)
                    {
                        if (DRLnkTable["FieldID"].ToString() == "309")
                        {
                            RegimenLineId = 309;
                        }
                        if (DRLnkTable["FieldID"].ToString() == "303")
                        {
                            WeightFieldId = 303;
                        }
                        if (DRLnkTable["FieldID"].ToString() == "304")
                        {
                            HeightFieldId = 304;
                        }
                        if (DRLnkTable["FieldID"].ToString() == "305")
                        {
                            BMIFieldId = 305;
                        }

                        if (dr["SectionID"].ToString() == DRLnkTable["SectionID"].ToString())
                        {
                            if (td <= Numtds)
                            {
                                if (td == 1)
                                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));

                                if (DRLnkTable["Controlid"].ToString() == "19")
                                {

                                    LoadFieldTypeControl(DRLnkTable["Controlid"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["FieldID"].ToString(), DRLnkTable["CodeID"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["PDFTableName"].ToString(), DRLnkTable["BindSource"].ToString(), true);
                                }
                                else
                                {
                                    if (td == 1)
                                    {
                                        PnlDynamicElements.Controls.Add(new LiteralControl("<td class='border center pad5 whitebg' style='width: 50%'>"));
                                        LoadFieldTypeControl(DRLnkTable["Controlid"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["FieldID"].ToString(), DRLnkTable["CodeID"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["PDFTableName"].ToString(), DRLnkTable["BindSource"].ToString(), true);
                                        PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                                        td++;
                                    }
                                    else
                                    {
                                        PnlDynamicElements.Controls.Add(new LiteralControl("<td class='border center pad5 whitebg' style='width: 50%'>"));
                                        LoadFieldTypeControl(DRLnkTable["Controlid"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["FieldID"].ToString(), DRLnkTable["CodeID"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["PDFTableName"].ToString(), DRLnkTable["BindSource"].ToString(), true);
                                        PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                                        PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                                        td = 1;
                                    }
                                }
                            }
                        }
                    }
                    td = 1;
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</br>"));
                }
                ViewState["NoMulti"] = theDS.Tables[3];
            }
            else
            {
                thePharmacyDS = (DataSet)ViewState["MasterData"];
                Session["Age"] = thePharmacyDS.Tables[6].Rows[0]["Age"];
                if (!IsPostBack)
                {
                    if (VisitID > 0)
                    {
                        BindDropdownOrderBy(0);
                        BindDropdownReportedBy(0);
                    }
                    else
                    {
                        BindDropdownOrderBy(0);
                        BindDropdownReportedBy(0);
                    }
                }

                Session["Frequency"] = thePharmacyDS.Tables[8];

                DataSet theDS = (DataSet)Session["AllData"];
                ViewState["LnkTable"] = theDS.Tables[1];
                ViewState["BusRule"] = theDS.Tables[2];
                Session["SessionBusRule"] = theDS.Tables[2];

                DataTable DT = theDS.Tables[1].DefaultView.ToTable(true, "SectionID", "SectionName").Copy();
                int Numtds = 2, td = 1;
                PnlDynamicElements.Controls.Clear();
                foreach (DataRow dr in DT.Rows)
                {
                    SectionHeading(dr["SectionName"].ToString());
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table cellspacing='6' cellpadding='0' width='100%' border='0'>"));
                    foreach (DataRow DRLnkTable in theDS.Tables[1].Rows)
                    {
                        if (DRLnkTable["FieldID"].ToString() == "309")
                        {
                            RegimenLineId = 309;
                        }
                        if (DRLnkTable["FieldID"].ToString() == "303")
                        {
                            WeightFieldId = 303;
                        }
                        if (DRLnkTable["FieldID"].ToString() == "304")
                        {
                            HeightFieldId = 304;
                        }
                        if (DRLnkTable["FieldID"].ToString() == "305")
                        {
                            BMIFieldId = 305;
                        }

                        if (dr["SectionID"].ToString() == DRLnkTable["SectionID"].ToString())
                        {
                            if (td <= Numtds)
                            {
                                if (td == 1)
                                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));

                                if (DRLnkTable["Controlid"].ToString() == "19")
                                {

                                    LoadFieldTypeControl(DRLnkTable["Controlid"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["FieldID"].ToString(), DRLnkTable["CodeID"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["PDFTableName"].ToString(), DRLnkTable["BindSource"].ToString(), true);
                                }
                                else
                                {
                                    if (td == 1)
                                    {
                                        PnlDynamicElements.Controls.Add(new LiteralControl("<td class='border center pad5 whitebg' style='width: 50%'>"));
                                        LoadFieldTypeControl(DRLnkTable["Controlid"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["FieldID"].ToString(), DRLnkTable["CodeID"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["PDFTableName"].ToString(), DRLnkTable["BindSource"].ToString(), true);
                                        PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                                        td++;
                                    }
                                    else
                                    {
                                        PnlDynamicElements.Controls.Add(new LiteralControl("<td class='border center pad5 whitebg' style='width: 50%'>"));
                                        LoadFieldTypeControl(DRLnkTable["Controlid"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["FieldID"].ToString(), DRLnkTable["CodeID"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["PDFTableName"].ToString(), DRLnkTable["BindSource"].ToString(), true);
                                        PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                                        PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                                        td = 1;
                                    }
                                }
                            }
                        }
                    }
                    td = 1;
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</br>"));
                }
                ViewState["NoMulti"] = theDS.Tables[3];
            }

        }
        private Boolean SetBusinessrule(string FieldID, string FieldLabel)
        {

            DataTable theDT = (DataTable)ViewState["BusRule"];
            foreach (DataRow DR in theDT.Rows)
            {
                if (Convert.ToString(DR["FieldID"]) == FieldID && Convert.ToString(DR["FieldName"]) == FieldLabel && Convert.ToString(DR["BusRuleId"]) == "1")
                {
                    return true;
                }
            }
            if (Session["SCMModule"] != null)
            {
                List<string> checkSCMValues = new List<string> { "317" };
                if (checkSCMValues.Contains(FieldID))
                {
                    return true;
                }
            }

            List<string> checkValues = new List<string> { "303", "304" };
            if (checkValues.Contains(FieldID))
            {
                return true;
            }

            return false;
        }

        public void BindAutoCompleteDrug()
        {

            string sqlQuery;
            IDrug objRptFields;
            objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
            if (RegimenLineId == 0)
                sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0 and dbo.fn_GetDrugTypeId_futures (Drug_pk) <>37");
            else
                sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0");
            DataTable dataTable = objRptFields.ReturnDatatableQuery(sqlQuery);
            Autoselectdrug.DataTextField = "DrugName";
            Autoselectdrug.DataValueField = "Drug_pk";
            Session["RadAutoDrugs"] = dataTable;
            Autoselectdrug.DataSource = dataTable;
            Autoselectdrug.DataBind();

        }
        private void LoadFieldTypeControl(string ControlID, string Column, string FieldId, string CodeID, string Label, string Table, string BindSource, Boolean theEnable)
        {
            try
            {
                DataTable theBusinessRuleDT = (DataTable)ViewState["BusRule"];
                DataView theBusinessRuleDV = new DataView(theBusinessRuleDT);
                DataView theAutoDV = new DataView();
                theBusinessRuleDV.RowFilter = "BusRuleId=17 and FieldId = " + FieldId.ToString();
                if (ControlID == "1") ///SingleLine Text Box
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "' >" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));
                    TextBox theSingleText = new TextBox();
                    theSingleText.ID = "TXT-" + Column + "-" + Table + "-" + FieldId;
                    theSingleText.Width = 180;
                    theSingleText.MaxLength = 50;
                    theSingleText.Enabled = theEnable;
                    PnlDynamicElements.Controls.Add(theSingleText);
                    ApplyBusinessRules(theSingleText, ControlID, theEnable);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));

                }
                else if (ControlID == "2")///DecimalTextBox
                {

                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));
                    if (FieldId == "301")
                    {
                        TextBox theAgeYrDecimalText = new TextBox();
                        theAgeYrDecimalText.ID = "TXTAgeYr-" + Column + "-" + Table + "-" + FieldId;
                        theAgeYrDecimalText.Width = 80;
                        theAgeYrDecimalText.MaxLength = 50;
                        theAgeYrDecimalText.Text = Convert.ToString(((DataSet)ViewState["MasterData"]).Tables[6].Rows[0]["Age"]);
                        theAgeYrDecimalText.Enabled = false;
                        PnlDynamicElements.Controls.Add(theAgeYrDecimalText);
                        PnlDynamicElements.Controls.Add(new LiteralControl("Years "));
                        TextBox theAgeMnthDecimalText = new TextBox();
                        theAgeMnthDecimalText.ID = "TXTMntYr-" + Column + "-" + Table + "-" + FieldId;
                        theAgeMnthDecimalText.Width = 80;
                        theAgeMnthDecimalText.MaxLength = 50;
                        theAgeMnthDecimalText.Text = Convert.ToString(((DataSet)ViewState["MasterData"]).Tables[6].Rows[0]["Age1"]);
                        theAgeMnthDecimalText.Enabled = false;
                        PnlDynamicElements.Controls.Add(theAgeMnthDecimalText);
                        PnlDynamicElements.Controls.Add(new LiteralControl("Months"));
                    }
                    else if (FieldId == "303")
                    {
                        TextBox theSingleDecimalText = new TextBox();
                        theSingleDecimalText.ID = "TXT-" + Column + "-" + Table + "-" + FieldId;
                        theSingleDecimalText.Width = 180;
                        theSingleDecimalText.MaxLength = 50;
                        theSingleDecimalText.Enabled = theEnable;
                        PnlDynamicElements.Controls.Add(theSingleDecimalText);
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label> Kg</label>"));
                        theSingleDecimalText.Attributes.Add("onkeyup", "chkNumeric('" + theSingleDecimalText.ClientID + "')");
                        theSingleDecimalText.Attributes.Add("onblur", "CalcualteBMIGet()");
                    }
                    else if (FieldId == "304")
                    {
                        TextBox theSingleDecimalText = new TextBox();
                        theSingleDecimalText.ID = "TXT-" + Column + "-" + Table + "-" + FieldId;
                        theSingleDecimalText.Width = 180;
                        theSingleDecimalText.MaxLength = 50;
                        theSingleDecimalText.Enabled = theEnable;
                        PnlDynamicElements.Controls.Add(theSingleDecimalText);
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label> cm</label>"));
                        theSingleDecimalText.Attributes.Add("onkeyup", "chkNumeric('" + theSingleDecimalText.ClientID + "')");
                        theSingleDecimalText.Attributes.Add("onblur", "CalcualteBMIGet()");
                        ApplyBusinessRules(theSingleDecimalText, ControlID, theEnable);
                    }
                    else if (FieldId == "305")
                    {
                        TextBox theSingleDecimalText = new TextBox();
                        theSingleDecimalText.ID = "TXT-" + Column + "-" + Table + "-" + FieldId;
                        theSingleDecimalText.Width = 180;
                        theSingleDecimalText.MaxLength = 50;
                        theSingleDecimalText.Enabled = theEnable;
                        PnlDynamicElements.Controls.Add(theSingleDecimalText);
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label> m<sup>2</label>"));
                        ApplyBusinessRules(theSingleDecimalText, ControlID, theEnable);
                    }
                    else
                    {
                        TextBox theSingleDecimalText = new TextBox();
                        theSingleDecimalText.ID = "TXT-" + Column + "-" + Table + "-" + FieldId;
                        theSingleDecimalText.Width = 180;
                        theSingleDecimalText.MaxLength = 50;
                        theSingleDecimalText.Enabled = theEnable;
                        PnlDynamicElements.Controls.Add(theSingleDecimalText);
                        theSingleDecimalText.Attributes.Add("onkeyup", "chkNumeric('" + theSingleDecimalText.ClientID + "')");
                        ApplyBusinessRules(theSingleDecimalText, ControlID, theEnable);
                    }

                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));

                }
                else if (ControlID == "3")   /// Numeric (Integer)
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));
                    TextBox theNumberText = new TextBox();
                    theNumberText.ID = "TXTNUM-" + Column + "-" + Table + "-" + FieldId;
                    theNumberText.Width = 100;
                    theNumberText.MaxLength = 9;
                    theNumberText.Enabled = theEnable;
                    PnlDynamicElements.Controls.Add(theNumberText);
                    theNumberText.Attributes.Add("onkeyup", "chkInteger('" + theNumberText.ClientID + "')");
                    ApplyBusinessRules(theNumberText, ControlID, theEnable);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                }
                else if (ControlID == "4") /// Dropdown
                {

                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));

                    DropDownList ddlSelectList = new DropDownList();
                    BindFunctions BindManager = new BindFunctions();
                    ddlSelectList.ID = "SELECTLIST-" + Column + "-" + Table + "-" + FieldId;
                    if (CodeID == "")
                    {
                        CodeID = "0";
                    }
                    DataView theDV = new DataView(theDSXML.Tables[BindSource]);
                    if (VisitID == 0)
                    {
                        DataTable theDT = new DataTable();
                        if (BindSource.ToUpper() == "MST_DECODE")
                        {
                            DataTable theDT4 = new DataTable();
                            DataView theDV4 = new DataView(((DataSet)Session["AllData"]).Tables[1]);
                            theDV4.RowFilter = "FieldId=" + FieldId + "";
                            theDT4 = theDV4.ToTable();
                            if (theDT4.Rows.Count > 0)
                            {
                                theDV.RowFilter = "(ModuleId IN(0, " + theDT4.Rows[0]["ModuleId"] + ") or ModuleId Is null) and SystemID IN(0," + Convert.ToString(Session["SystemId"]) + ") and CodeID=" + CodeID + "";
                            }
                            if (theDV.Count > 1)
                            {
                                theDT = theDV.ToTable();
                                BindManager.BindCombo(ddlSelectList, theDT, "Name", "CodeID");
                            }
                        }
                        else if (BindSource.ToUpper() == "MST_PMTCTDECODE" || BindSource.ToUpper() == "MST_MODDECODE")
                        {
                            theDV.RowFilter = "DeleteFlag=0 and SystemID IN(0," + Convert.ToString(Session["SystemId"]) + ") and CodeID=" + CodeID + "";
                            if (theDV.Count > 1)
                            {
                                theDT = theDV.ToTable();
                                BindManager.BindCombo(ddlSelectList, theDT, "Name", "ID");
                            }
                        }
                        else
                        {
                            theDV.RowFilter = "DeleteFlag=0";
                            if (theDV.Count > 1)
                            {
                                theDT = theDV.ToTable();
                                BindManager.BindCombo(ddlSelectList, theDT, "Name", "ID");
                            }
                        }

                    }
                    else
                    {
                        DataTable theDT = new DataTable();
                        if (BindSource.ToUpper() == "MST_DECODE")
                        {
                            DataView theDV4 = new DataView(((DataSet)Session["AllData"]).Tables[1]);
                            theDV4.RowFilter = "FieldId=" + FieldId + "";
                            DataTable theDT4 = theDV4.ToTable();
                            if (theDT4.Rows.Count > 0)
                            {
                                theDV.RowFilter = "(ModuleId IN(0, " + theDT4.Rows[0]["ModuleId"] + ") or ModuleId Is null) and SystemID IN(0," + Convert.ToString(Session["SystemId"]) + ") and CodeID=" + CodeID + "";
                            }
                            if (theDV.Count > 1)
                            {
                                theDT = theDV.ToTable();
                                BindManager.BindCombo(ddlSelectList, theDT, "Name", "CodeID");
                            }
                        }
                        else if (BindSource.ToUpper() == "MST_PMTCTDECODE" || BindSource.ToUpper() == "MST_MODDECODE")
                        {

                            theDV.RowFilter = "SystemID IN(0," + Convert.ToString(Session["SystemId"]) + ") and CodeID=" + CodeID + "";
                            if (theDV.Count > 1)
                            {
                                theDT = theDV.ToTable();
                                BindManager.BindCombo(ddlSelectList, theDT, "Name", "ID");
                            }
                        }
                        else
                        {
                            theDV.RowFilter = "DeleteFlag=0";
                            if (theDV.Count > 1)
                            {
                                theDT = theDV.ToTable();
                                BindManager.BindCombo(ddlSelectList, theDT, "Name", "ID");
                            }
                        }
                    }
                    ddlSelectList.Width = 180;
                    PnlDynamicElements.Controls.Add(ddlSelectList);
                    ApplyBusinessRules(ddlSelectList, ControlID, theEnable);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                }
                else if (ControlID == "5") ///Date
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));
                    if (Label == "DOB")
                    {
                        TextBox theDateText = new TextBox();
                        theDateText.ID = "TXTDT-" + Column + "-" + Table + "-" + FieldId;
                        Control ctl = (TextBox)theDateText;
                        theDateText.Width = 83;
                        theDateText.MaxLength = 11;
                        theDateText.Text = String.Format("{0:dd-MMM-yyyy}", ((DataSet)ViewState["MasterData"]).Tables[6].Rows[0]["DOB"]);
                        theDateText.Enabled = false;
                        PnlDynamicElements.Controls.Add(theDateText);
                        theDateText.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
                        theDateText.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
                        PnlDynamicElements.Controls.Add(new LiteralControl("&nbsp;"));

                        Image theDateImage = new Image();
                        theDateImage.ID = "img" + theDateText.ID;
                        theDateImage.Height = 22;
                        theDateImage.Width = 22;
                        theDateImage.Visible = theEnable;
                        theDateImage.ToolTip = "Date Helper";
                        theDateImage.ImageUrl = "~/images/cal_icon.gif";
                        PnlDynamicElements.Controls.Add(theDateImage);
                    }
                    else
                    {
                        TextBox theDateText = new TextBox();
                        theDateText.ID = "TXTDT-" + Column + "-" + Table + "-" + FieldId;
                        Control ctl = (TextBox)theDateText;
                        theDateText.Width = 83;
                        theDateText.MaxLength = 11;
                        theDateText.Enabled = theEnable;
                        PnlDynamicElements.Controls.Add(theDateText);
                        theDateText.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
                        theDateText.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
                        ApplyBusinessRules(theDateText, ControlID, theEnable);
                        PnlDynamicElements.Controls.Add(new LiteralControl("&nbsp;"));

                        Image theDateImage = new Image();
                        theDateImage.ID = "img" + theDateText.ID;
                        theDateImage.Height = 22;
                        theDateImage.Width = 22;
                        theDateImage.Visible = theEnable;
                        theDateImage.ToolTip = "Date Helper";
                        theDateImage.ImageUrl = "~/images/cal_icon.gif";
                        theDateImage.Attributes.Add("onClick", "w_displayDatePicker('" + ((TextBox)ctl).ClientID + "');");
                        PnlDynamicElements.Controls.Add(theDateImage);
                        ApplyBusinessRules(theDateImage, ControlID, theEnable);
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("<span class='smallerlabel'>(DD-MMM-YYYY)</span>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                }
                else if (ControlID == "6")  /// Radio Button
                {

                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));

                    HtmlInputRadioButton theYesNoRadio1 = new HtmlInputRadioButton();
                    theYesNoRadio1.ID = "RADIO1-" + Column + "-" + Table + "-" + FieldId;
                    theYesNoRadio1.Value = "Yes";
                    theYesNoRadio1.Name = "" + Column + "";
                    theYesNoRadio1.Attributes.Add("onclick", "down(this);");
                    theYesNoRadio1.Attributes.Add("onfocus", "up(this)");
                    PnlDynamicElements.Controls.Add(theYesNoRadio1);
                    theYesNoRadio1.Visible = theEnable;
                    ApplyBusinessRules(theYesNoRadio1, ControlID, theEnable);
                    theYesNoRadio1.Visible = theEnable;
                    PnlDynamicElements.Controls.Add(new LiteralControl("<label align='labelright' id='lblYes-" + FieldId + "'>Yes</label>"));

                    HtmlInputRadioButton theYesNoRadio2 = new HtmlInputRadioButton();
                    theYesNoRadio2.ID = "RADIO2-" + Column + "-" + Table + "-" + FieldId;
                    theYesNoRadio2.Value = "No";
                    theYesNoRadio2.Name = "" + Column + "";
                    theYesNoRadio2.Attributes.Add("onclick", "down(this);");
                    theYesNoRadio2.Attributes.Add("onchange", "up(this)");
                    PnlDynamicElements.Controls.Add(theYesNoRadio2);
                    ApplyBusinessRules(theYesNoRadio2, ControlID, theEnable);
                    theYesNoRadio2.Visible = theEnable;
                    PnlDynamicElements.Controls.Add(new LiteralControl("<label align='labelright' id='lblNo-" + FieldId + "'>No</label>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));

                }
                else if (ControlID == "7") //Checkbox
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));
                    HtmlInputCheckBox theChk = new HtmlInputCheckBox();
                    theChk.ID = "Chk-" + Column + "-" + Table + "-" + FieldId;
                    theChk.Visible = theEnable;
                    PnlDynamicElements.Controls.Add(theChk);
                    ApplyBusinessRules(theChk, ControlID, theEnable);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));

                }
                else if (ControlID == "8")  /// MultiLine TextBox
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));
                    TextBox theMultiText = new TextBox();
                    theMultiText.ID = "TXTMulti-" + Column + "-" + Table + "-" + FieldId;
                    theMultiText.Width = 200;
                    theMultiText.TextMode = TextBoxMode.MultiLine;
                    theMultiText.MaxLength = 200;
                    theMultiText.Enabled = theEnable;
                    PnlDynamicElements.Controls.Add(theMultiText);
                    ApplyBusinessRules(theMultiText, ControlID, theEnable);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                }
                else if (ControlID == "9") ///  MultiSelect List 
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));
                    //WithPanel
                    Panel PnlMulti = new Panel();
                    PnlMulti.ID = "Pnl_" + FieldId;
                    PnlMulti.ToolTip = Label;
                    PnlMulti.Enabled = theEnable;
                    PnlMulti.Controls.Add(new LiteralControl("<DIV class = 'customdivbordermultiselect'  runat='server' nowrap='nowrap'>"));
                    if (CodeID == "")
                    {
                        CodeID = "0";
                    }
                    string theCodeFldName = "";
                    DataTable theBindTbl = theDSXML.Tables[BindSource];
                    if (theBindTbl.Columns.Contains("CategoryId") == true)
                        theCodeFldName = "CategoryId";
                    else if (theBindTbl.Columns.Contains("SectionId") == true)
                        theCodeFldName = "SectionId";
                    else
                        theCodeFldName = "CodeId";
                    DataView theDV = new DataView(theDSXML.Tables[BindSource]);
                    theDV.RowFilter = "DeleteFlag=0 and SystemID IN(0," + Convert.ToString(Session["SystemId"]) + ") and " + theCodeFldName + "=" + CodeID + "";
                    IQCareUtils theUtils = new IQCareUtils();
                    BindFunctions BindManager = new BindFunctions();
                    DataTable theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                    if (theDT != null)
                    {
                        for (int i = 0; i < theDT.Rows.Count; i++)
                        {
                            CheckBox chkbox = new CheckBox();
                            chkbox.ID = Convert.ToString("CHKMULTI-" + theDT.Rows[i][0] + "-" + Column + "-" + Table + "-" + FieldId);
                            chkbox.Text = Convert.ToString(theDT.Rows[i]["Name"]);
                            PnlMulti.Controls.Add(chkbox);
                            chkbox.Width = 300;
                            PnlMulti.Controls.Add(new LiteralControl("<br>"));
                        }
                    }
                    PnlMulti.Controls.Add(new LiteralControl("</DIV>"));
                    PnlDynamicElements.Controls.Add(PnlMulti);
                    ApplyBusinessRules(PnlMulti, ControlID, theEnable);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                }
                else if (ControlID == "13")  /// Placeholder
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:100%;Height:25px' align='right'>"));
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    div1.ID = "DIVPLC-" + Column + "-" + FieldId;
                    PlaceHolder thePlchlderText = new PlaceHolder();
                    thePlchlderText.ID = "plchlder-" + Column + "-" + FieldId;
                    thePlchlderText.Controls.Add(div1);
                    PnlDynamicElements.Controls.Add(thePlchlderText);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                }

                else if (ControlID == "14")
                {
                    PnlDynamicElements.Controls.Add(new LiteralControl("<table width='100%'>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:40%' align='right'>"));
                    DropDownList ddlSelectListAuto = new DropDownList();
                    if (SetBusinessrule(FieldId, Column) == true)
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label class='required' align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    else
                    {
                        PnlDynamicElements.Controls.Add(new LiteralControl("<label align='center' id='lbl" + Label + "-" + FieldId + "'>" + Label + " :</label>"));
                    }
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("<td style='width:60%' align='left'>"));

                    DropDownList ddlSelectList = new DropDownList();
                    ddlSelectList.ID = "SELECTLIST-" + Column + "-" + Table + "-" + FieldId;
                    IQCareUtils theUtil = new IQCareUtils();
                    DataTable theDT = theUtil.CreateTimeTable(15);
                    DataRow theDR = theDT.NewRow();
                    theDR[0] = "0";
                    theDR[1] = "Select";
                    theDT.Rows.InsertAt(theDR, 0);
                    ddlSelectList.DataSource = theDT;
                    ddlSelectList.DataTextField = "Time";
                    ddlSelectList.DataValueField = "Id";
                    ddlSelectList.DataBind();
                    ddlSelectList.SelectedValue = "Select";
                    ddlSelectList.Width = 180;
                    ddlSelectList.Enabled = theEnable;
                    PnlDynamicElements.Controls.Add(ddlSelectList);
                    ApplyBusinessRules(ddlSelectList, ControlID, theEnable);
                    PnlDynamicElements.Controls.Add(new LiteralControl("</td>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</tr>"));
                    PnlDynamicElements.Controls.Add(new LiteralControl("</table>"));
                }
                else if (ControlID == "19")
                {
                    Label lblPreselected = new Label();
                    lblPreselected.ID = "lbl-" + Column + "-" + Table + "-" + FieldId;
                    lblPreselected.Text = "   " + Label + ": ";
                    lblPreselected.Font.Bold = true;
                    PnlPreSelectedDrug.Controls.Add(lblPreselected);
                    PnlPreSelectedDrug.Controls.Add(new LiteralControl("</br>"));
                    ViewState["RefillClick"] = "false";
                    DataView theDV = new DataView(((DataSet)Session["AllData"]).Tables[4]);
                    theDV.RowFilter = "FieldId=" + FieldId + "";
                    DataTable theDT = theDV.ToTable();
                    RadComboBox rcbPreSelectedDrugs = new RadComboBox();
                    rcbPreSelectedDrugs.ID = "RCBCMBLIST-" + Column + "-" + Table + "-" + FieldId;
                    rcbPreSelectedDrugs.CheckBoxes = true;
                    rcbPreSelectedDrugs.EnableCheckAllItemsCheckBox = true;
                    rcbPreSelectedDrugs.CheckedItemsTexts = RadComboBoxCheckedItemsTexts.FitInInput;
                    rcbPreSelectedDrugs.Skin = "Outlook";
                    rcbPreSelectedDrugs.Width = 700;
                    rcbPreSelectedDrugs.AutoPostBack = true;
                    rcbPreSelectedDrugs.DataTextField = "DrugName";
                    rcbPreSelectedDrugs.DataValueField = "DrugId";
                    rcbPreSelectedDrugs.DataSource = theDT;
                    rcbPreSelectedDrugs.DataBind();
                    Button btnPreselecteddrug = new Button();
                    btnPreselecteddrug.ID = "btnPreselecteddrug-" + Column + "-" + Table + "-" + FieldId;
                    btnPreselecteddrug.Text = "Add";
                    btnPreselecteddrug.Font.Bold = true;
                    btnPreselecteddrug.Click += new EventHandler(btnPreselecteddrug_Click);
                    PnlPreSelectedDrug.Controls.Add(rcbPreSelectedDrugs);
                    PnlPreSelectedDrug.Controls.Add(btnPreselecteddrug);
                    PnlPreSelectedDrug.Controls.Add(new LiteralControl("</br>"));
                    //string sqlQuery;
                    //IDrug objRptFields;
                    //objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
                    //sqlQuery = string.Format("SELECT Drug_pk,DrugName FROM Mst_Drug WHERE DeleteFlag=0 and Drug_pk Not IN(Select DrugID from lnk_CustomFieldDrugId where FieldId=" + FieldId + ")");
                    //DataTable DTAutoSelectedDrug = objRptFields.ReturnDatatableQuery(sqlQuery);
                    //Autoselectdrug.DataTextField = "DrugName";
                    //Autoselectdrug.DataValueField = "Drug_pk";
                    //Autoselectdrug.DataSource = DTAutoSelectedDrug;
                    //Autoselectdrug.DataBind();
                }

            }
            catch (Exception err)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }
        }
        private void ApplyBusinessRules(object theControl, string ControlID, bool theConField)
        {
            try
            {
                DataTable theDT = (DataTable)ViewState["BusRule"];
                string Max = "", Min = "", Column = "";
                string MaxNormalval = "", MinNormalVal = "";
                bool theEnable = theConField;
                string[] Field, wihifield;
                if (ControlID == "9")
                {
                    Field = ((Control)theControl).ID.Split('_');
                }
                else
                {
                    Field = ((Control)theControl).ID.Split('-');
                }
                foreach (DataRow DR in theDT.Rows)
                {
                    if (Field[0] == "Pnl")
                    {
                        string[] PnlBus;
                        int splitlength = ((Control)theControl).ID.Split('_').Length - 1;
                        PnlBus = Field[splitlength].Split('-');

                        if (PnlBus[1] == Convert.ToString(DR["FieldId"]) && Convert.ToString(DR["BusRuleId"]) == "14"
                            && Session["PatientSex"].ToString() != "Male")
                            theEnable = false;

                        if (PnlBus[1] == Convert.ToString(DR["FieldId"]) && Convert.ToString(DR["BusRuleId"]) == "15"
                            && Session["PatientSex"].ToString() != "Female")
                            theEnable = false;

                        if (PnlBus[1] == Convert.ToString(DR["FieldId"]) && Convert.ToString(DR["BusRuleId"]) == "16")
                        {
                            if ((DR["Value"] != System.DBNull.Value) && (DR["Value1"] != System.DBNull.Value))
                            {
                                if (Convert.ToDecimal(Session["PatientAge"]) >= Convert.ToDecimal(DR["Value"]) && Convert.ToDecimal(Session["PatientAge"]) <= Convert.ToDecimal(DR["Value1"]))
                                {
                                }
                                else
                                    theEnable = false;
                            }
                        }
                    }
                    else
                    {
                        if (Field[1] == Convert.ToString(DR["FieldName"]) && Field[2] == Convert.ToString(DR["TableName"]) && Field[3] == Convert.ToString(DR["FieldId"]) && Convert.ToString(DR["BusRuleId"]) == "2")
                        {
                            Max = Convert.ToString(DR["Value"]);
                            Column = Convert.ToString(DR["FieldLabel"]);
                        }
                        if (Field[1] == Convert.ToString(DR["FieldName"]) && Field[2] == Convert.ToString(DR["TableName"]) && Field[3] == Convert.ToString(DR["FieldId"]) && Convert.ToString(DR["BusRuleId"]) == "3")
                        {
                            Min = Convert.ToString(DR["Value"]);

                        }
                        if (Field[1] == Convert.ToString(DR["FieldName"]) && Field[2] == Convert.ToString(DR["TableName"]) && Field[3] == Convert.ToString(DR["FieldId"]) && Convert.ToString(DR["BusRuleId"]) == "26")
                        {
                            MaxNormalval = Convert.ToString(DR["Value"]);

                        }
                        if (Field[1] == Convert.ToString(DR["FieldName"]) && Field[2] == Convert.ToString(DR["TableName"]) && Field[3] == Convert.ToString(DR["FieldId"]) && Convert.ToString(DR["BusRuleId"]) == "27")
                        {
                            MinNormalVal = Convert.ToString(DR["Value"]);

                        }


                        if (Field[1] == Convert.ToString(DR["FieldName"]) && Field[2] == Convert.ToString(DR["TableName"]) && Field[3] == Convert.ToString(DR["FieldId"])
                            && Convert.ToString(DR["BusRuleId"]) == "14" && Session["PatientSex"].ToString() != "Male")
                            theEnable = false;

                        if (Field[1] == Convert.ToString(DR["FieldName"]) && Field[2] == Convert.ToString(DR["TableName"]) && Field[3] == Convert.ToString(DR["FieldId"])
                        && Convert.ToString(DR["BusRuleId"]) == "15" && Session["PatientSex"].ToString() != "Female")
                            theEnable = false;

                        if (Field[1] == Convert.ToString(DR["FieldName"]) && Field[2] == Convert.ToString(DR["TableName"]) && Field[3] == Convert.ToString(DR["FieldId"])
                        && Convert.ToString(DR["BusRuleId"]) == "16")
                        {
                            if (Convert.ToDecimal(Session["PatientAge"]) >= Convert.ToDecimal(DR["Value"]) && Convert.ToDecimal(Session["PatientAge"]) <= Convert.ToDecimal(DR["Value1"]))
                            {
                            }
                            else
                                theEnable = false;
                        }
                    }
                }

                if (theControl.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                {
                    Field = ((Control)theControl).ID.Split('_');
                    wihifield = Field[0].Split('-');
                    TextBox tbox = (TextBox)theControl;
                    if (ControlID == "1")
                    { }
                    else if (ControlID == "2" && Field[0] == "TXT")
                    {
                        if (!tbox.ClientID.Contains("ctl00_IQCareContentPlaceHolder_"))
                        {
                            tbox.Attributes.Add("onkeyup", "chkDecimal('ctl00_IQCareContentPlaceHolder_" + tbox.ClientID + "')");
                        }
                    }
                    else if (ControlID == "3" && Field[0] == "TXTNUM")
                    {
                        if (!tbox.ClientID.Contains("ctl00_IQCareContentPlaceHolder_"))
                        {
                            tbox.Attributes.Add("onkeyup", "chkDecimal('ctl00_IQCareContentPlaceHolder_" + tbox.ClientID + "')");
                        }
                    }
                    else if (ControlID == "5" && Field[0] == "TXTDT")
                    {

                        tbox.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
                        tbox.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
                    }
                    if (Max != "" && Min != "" && MaxNormalval != "" && MinNormalVal != "")
                    {
                        if (!tbox.ClientID.Contains("ctl00_IQCareContentPlaceHolder_"))
                        {
                            if (wihifield[1].ToUpper() == "HEIGHT" || wihifield[1].ToUpper() == "WEIGHT")
                            {
                                tbox.Attributes.Add("onblur", "CalcualteBMIGet(); isCheckNormal('ctl00_IQCareContentPlaceHolder_" + tbox.ClientID + "', '" + Column + "', '" + Min + "', '" + Max + "', '" + MinNormalVal + "', '" + MaxNormalval + "')");
                            }
                            else
                                tbox.Attributes.Add("onblur", "isCheckNormal('ctl00_IQCareContentPlaceHolder_" + tbox.ClientID + "', '" + Column + "', '" + Min + "', '" + Max + "', '" + MinNormalVal + "', '" + MaxNormalval + "')");
                        }
                    }
                    else if (Max != "" && Min != "")
                    {
                        if (!tbox.ClientID.Contains("ctl00_IQCareContentPlaceHolder_"))
                        {
                            tbox.Attributes.Add("onblur", "isBetween('ctl00_IQCareContentPlaceHolder_" + tbox.ClientID + "', '" + Column + "', '" + Min + "', '" + Max + "')");
                        }
                    }
                    else if (Max != "")
                    {
                        if (!tbox.ClientID.Contains("ctl00_IQCareContentPlaceHolder_"))
                        {
                            tbox.Attributes.Add("onblur", "checkMax('ctl00_IQCareContentPlaceHolder_" + tbox.ClientID + "', '" + Column + "', '" + Max + "')");

                        }
                    }
                    else if (Min != "")
                    {
                        if (!tbox.ClientID.Contains("ctl00_IQCareContentPlaceHolder_"))
                        {
                            tbox.Attributes.Add("onblur", "checkMin('ctl00_IQCareContentPlaceHolder" + tbox.ClientID + "', '" + Column + "', '" + Min + "')");
                        }
                    }
                }
                else if (theControl.GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                {
                    DropDownList ddList = (DropDownList)theControl;
                    ddList.Enabled = theEnable;

                }
                else if (theControl.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                {
                    CheckBox Multichk = (CheckBox)theControl;
                    Multichk.Enabled = theEnable;
                }
                else if (theControl.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlInputRadioButton")
                {
                    HtmlInputRadioButton Rdobtn = (HtmlInputRadioButton)theControl;
                }
                else if (theControl.GetType().ToString() == "System.Web.UI.WebControls.Image")
                {
                    Image img = (Image)theControl;
                    img.Visible = theEnable;
                }
                else if (theControl.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                {
                    Panel pnl = (Panel)theControl;
                    pnl.Enabled = theEnable;
                }
                else if (theControl.GetType().ToString() == "System.Web.UI.WebControls.Button")
                {
                    Button tbtn = (Button)theControl;
                    tbtn.Enabled = theEnable;
                }
            }
            catch (Exception err)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }
        }

        void btnPreselecteddrug_Click(object sender, EventArgs e)
        {
            foreach (object x in PnlPreSelectedDrug.Controls)
            {
                if (x.GetType() == typeof(Telerik.Web.UI.RadComboBox))
                {
                    var collection = ((RadComboBox)x).CheckedItems;
                    string DrugId = "";
                    if (collection.Count != 0)
                    {
                        foreach (var item in collection)
                        {
                            DrugId = item.Value;
                            BindGridWithData(DrugId);
                        }
                    }
                }
            }
        }
        public DataTable CreateDrugTable()
        {
            tbldrug.Columns.Add("DrugID", typeof(Int32));
            tbldrug.Columns.Add("DrugName", typeof(string));
            tbldrug.Columns.Add("DispensingUnit", typeof(string));
            tbldrug.Columns.Add("GenericID", typeof(string));
            tbldrug.Columns.Add("DrugType", typeof(string));
            tbldrug.Columns.Add("DrugTypeID", typeof(Int32));
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
            tblOrder.Columns.Add("Instructions", typeof(string));
            tblOrder.PrimaryKey = new DataColumn[] { tblOrder.Columns["DrugID"] };
            return tblOrder;
        }
        public DataTable CreateDispensTable()
        {
            tblDispense.Columns.Add("DrugID", typeof(Int32));
            tblDispense.Columns.Add("QtyPrescribed", typeof(decimal));
            tblDispense.Columns.Add("QtyDispensed", typeof(decimal));
            tblDispense.Columns.Add("BatchNo", typeof(Int32));
            tblDispense.Columns.Add("ExpiryDate", typeof(string));
            tblDispense.Columns.Add("SellPrice", typeof(decimal));
            tblDispense.Columns.Add("BillAmount", typeof(decimal));
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
        public DataTable CreateInstructTable()
        {
            tblInstruct.Columns.Add("DrugID", typeof(Int32));
            tblInstruct.Columns.Add("Instructions", typeof(string));
            tblInstruct.PrimaryKey = new DataColumn[] { tblInstruct.Columns["DrugID"] };
            return tblInstruct;
        }
        public void BindGridWithData(string Drugpk)
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
                //------------Instruct
                if (ViewState["TableInstruct"] == null)
                {
                    theInstruct = CreateInstructTable();
                }
                else
                {
                    theInstruct = (DataTable)ViewState["TableInstruct"];
                }
                DataSet theAutoDS = (DataSet)ViewState["MasterData"];
                DataRow[] result = theAutoDS.Tables[33].Select("drug_pk=" + Drugpk + "");
                DataRow[] findRow = theDT.Select("DrugID=" + Drugpk + "");
                int len = findRow.Length;
                int drugid = 0;
                if (len == 0)
                {
                    foreach (DataRow row in result)
                    {

                        DataRow DR = theDT.NewRow();
                        DR["DrugID"] = row["drug_pk"];
                        drugid = Convert.ToInt32(row["drug_pk"]);
                        DR["DrugName"] = row["drugname"];
                        DR["DrugType"] = row["drugtypename"];
                        DR["DrugTypeID"] = row["drugtypeId"];
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

                        //Instruction
                        DataRow DR4 = theInstruct.NewRow();
                        DR4["DrugID"] = row["drug_pk"];
                        theInstruct.Rows.Add(DR4);
                    }
                }
                ViewState["TableDrug"] = theDT;
                ViewState["TableOrder"] = theOrder;
                ViewState["TableDispense"] = theDispense;
                ViewState["TableRefill"] = theRefill;
                ViewState["TableInstruct"] = theInstruct;
                BindDrugGrid(drugid);
            }
            catch { }
        }
        public void BindDrugGrid(int drugid)
        {
            if (((DataTable)ViewState["TableDrug"]).Rows.Count > 0)
            {
                rgdrugmain.DataSource = (DataTable)ViewState["TableDrug"];
                rgdrugmain.DataBind();
                if (drugid > 0)
                {
                    foreach (GridDataItem parentItem in rgdrugmain.Items)
                    {
                        string a = parentItem.GetDataKeyValue("DrugID").ToString();
                        if (drugid == Convert.ToInt32(a))
                        {
                            //GridDataItem parentItem = e.Item as GridDataItem;
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
                                        dtupdateOrder.Rows[i]["Prophylaxis"] = CheckedValue(chkProphylaxis.SelectedToggleState.Text);

                                        dtupdateOrder.AcceptChanges();

                                    }
                                }

                            }
                            ViewState["TableOrder"] = dtupdateOrder;
                            gridorder.Rebind();

                            RadGrid gridInstruction = parentItem.ChildItem.FindControl("PatientInstruction") as RadGrid;
                            DataTable dtInstruction = (DataTable)ViewState["TableInstruct"];
                            foreach (GridDataItem item in gridInstruction.Items)
                            {
                                string drugID = item.GetDataKeyValue("DrugID").ToString();
                                RadTextBox txtInstruction = (RadTextBox)item.FindControl("txtPatientInstruction");
                                foreach (DataRow theDR in dtupdateOrder.Rows)
                                {
                                    if (theDR["DrugID"].ToString() == drugID)
                                    {

                                        if (txtInstruction.Text != "")
                                        {
                                            theDR["Instructions"] = Convert.ToString(txtInstruction.Text);
                                            theDR.EndEdit();
                                            dtupdateOrder.AcceptChanges();
                                        }
                                    }

                                }
                            }
                            ViewState["TableInstruct"] = dtInstruction;
                            gridInstruction.Rebind();

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

                            parentItem.Expanded = true;
                        }
                    }

                    foreach (GridNestedViewItem item in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
                    {
                        RadTabStrip TabStrip = (RadTabStrip)item.FindControl("TabStip1");
                        if (Session["Paperless"].ToString() == "0" && Session["SCMModule"] == null)
                        {
                            TabStrip.Tabs[2].Enabled = true;
                            TabStrip.Tabs[0].Focus();
                            RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
                            radpage.SelectedIndex = 0;
                        }
                        else
                        {
                            if (Session["Orderid"].ToString() == "0")
                            {
                                TabStrip.Tabs[2].Enabled = false;
                                TabStrip.Tabs[0].Focus();
                                RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
                                radpage.SelectedIndex = 0;
                            }
                        }
                        if (ViewState["RefillClick"] != null)
                        {
                            if (ViewState["RefillClick"].ToString() == "True")
                            {
                                TabStrip.Tabs[1].Enabled = false;
                                RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
                                radpage.SelectedIndex = 0;
                            }
                        }
                    }
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

                }
            }
            else
            {
                rgdrugmain.DataSource = (DataTable)ViewState["TableDrug"];
                rgdrugmain.DataBind();
                foreach (GridDataItem parentItem in rgdrugmain.Items)
                {
                    parentItem.Expanded = true;
                }
            }
        }
        protected void Autoselectdrug_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {
            string Drugpk = Autoselectdrug.Entries[0].Value;
            BindGridWithData(Drugpk);
            Autoselectdrug.Entries.Clear();
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
                DispenseGrid.ItemCreated += new GridItemEventHandler(DispenseGrid_ItemCreated);
                DispenseGrid.ItemDataBound += new GridItemEventHandler(DispenseGrid_ItemDataBound);
                RadGrid gridRefill = (RadGrid)e.Item.FindControl("Refill");
                gridRefill.ItemCreated += new GridItemEventHandler(gridRefill_ItemCreated);
                //RadGrid gridInstruction = (RadGrid)e.Item.FindControl("PatientInstruction");
                //gridInstruction.ItemCreated += new GridItemEventHandler(gridInstruction_ItemCreated);
                //gridInstruction.ItemDataBound += new GridItemEventHandler(gridInstruction_ItemDataBound);
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

            }
        }
        protected void DispenseGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                DataView dv = new DataView(((DataSet)ViewState["MasterData"]).Tables[32]);
                dv.RowFilter = "ItemId=" + parentItem.GetDataKeyValue("DrugID").ToString() + "";
                BindFunctions theBind = new BindFunctions();
                RadComboBox combo = ((RadComboBox)e.Item.FindControl("rdcmbBatch"));
                RadTextBox txtExpiry = ((RadTextBox)e.Item.FindControl("txtExpiryDate"));
                RadNumericTextBox txtSPrice = ((RadNumericTextBox)e.Item.FindControl("txtSellPrice"));
                RadNumericTextBox txtBAmount = ((RadNumericTextBox)e.Item.FindControl("txtBillAmount"));
                RadNumericTextBox txtAvailQty = ((RadNumericTextBox)e.Item.FindControl("txtAvailQty"));
                theBind.BindCombo(combo, dv.ToTable(), "name", "id");
                if (DataBinder.Eval(e.Item.DataItem, "BatchNo") != null)
                {
                    combo.SelectedValue = DataBinder.Eval(e.Item.DataItem, "BatchNo").ToString();
                }
                if (DataBinder.Eval(e.Item.DataItem, "ExpiryDate") != null)
                {
                    txtExpiry.Text = String.Format("{0:dd-MMM-yyyy}", DataBinder.Eval(e.Item.DataItem, "ExpiryDate"));
                }
                if (DataBinder.Eval(e.Item.DataItem, "SellPrice") != null)
                {
                    txtSPrice.Text = DataBinder.Eval(e.Item.DataItem, "SellPrice").ToString();
                }
                if (DataBinder.Eval(e.Item.DataItem, "BillAmount") != null)
                {
                    txtBAmount.Text = DataBinder.Eval(e.Item.DataItem, "BillAmount").ToString();
                }

                RadComboBox ddBatch = (RadComboBox)e.Item.FindControl("rdcmbBatch");
                ddBatch.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(ddBatch_SelectedIndexChanged);
                ddBatch.AutoPostBack = true;
                IDrug objRptFields;
                objRptFields = (IDrug)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BDrug,BusinessProcess.Pharmacy");
                string sqlQuery;
                sqlQuery = "select md.Drug_pk,convert(varchar(100),md.DrugName)[Drugname], ISNULL(Convert(varchar,SUM(st.Quantity)),0)[QTY] from dtl_stocktransaction st ";
                sqlQuery += " Right outer join mst_drug md on md.Drug_pk=st.ItemId where md.Drug_pk ='" + parentItem.GetDataKeyValue("DrugID").ToString() + "' Group by md.Drug_pk,md.Drugname";
                DataTable dataTable = objRptFields.ReturnDatatableQuery(sqlQuery);
                txtAvailQty.Text = dataTable.Rows[0]["QTY"].ToString();
            }
        }
        protected void gridInstruction_ItemDataBound(object sender, GridItemEventArgs e)
        {
        }
        protected void gridInstruction_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            DataView dv = new DataView((DataTable)ViewState["TableRefill"]);
            dv.RowFilter = "DrugID=" + parentItem.GetDataKeyValue("DrugID").ToString() + "";
            DataTable dtfilter = dv.ToTable();
            (sender as RadGrid).DataSource = dtfilter;

        }
        protected void gridRefill_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                RadDatePicker dtrefillexpiration = (RadDatePicker)e.Item.FindControl("dtRefillExpiration");
                if (DataBinder.Eval(e.Item.DataItem, "RefillExpiration") != null)
                {
                    if (((System.Data.DataRowView)(e.Item.DataItem)).Row.ItemArray[2].ToString() != "1/1/1900 12:00:00 AM")
                    {
                        dtrefillexpiration.DbSelectedDate = DataBinder.Eval(e.Item.DataItem, "RefillExpiration").ToString();
                    }
                }
            }
        }
        protected void DispenseGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {

                RadNumericTextBox txtqtydispensed = (RadNumericTextBox)e.Item.FindControl("txtQtyDispensed");
                RadNumericTextBox txtqtyprec = (RadNumericTextBox)e.Item.FindControl("txtQtyPrescribeddispense");
                RadNumericTextBox txtsellingprice = (RadNumericTextBox)e.Item.FindControl("txtSellPrice");
                RadNumericTextBox txttotalprice = (RadNumericTextBox)e.Item.FindControl("txtBillAmount");
                RadComboBox cmbox = (RadComboBox)e.Item.FindControl("rdcmbBatch");
                cmbox.Attributes.Add("OnClientBlur", "return Validate('" + txtqtydispensed.ClientID + "','" + txtqtyprec.ClientID + "','" + txtsellingprice.ClientID + "','" + txttotalprice.ClientID + "')");
                txtqtydispensed.Attributes.Add("onblur", "return Validate('" + txtqtydispensed.ClientID + "','" + txtqtyprec.ClientID + "','" + txtsellingprice.ClientID + "','" + txttotalprice.ClientID + "')");
            }
        }
        protected void ddBatch_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            DataView dv = new DataView(((DataSet)ViewState["MasterData"]).Tables[32]);
            dv.RowFilter = "Id=" + e.Value + "";
            DataView dvsp = new DataView(((DataSet)ViewState["MasterData"]).Tables[33]);
            dvsp.RowFilter = "Drug_pk=" + e.Value + "";
            foreach (GridNestedViewItem nestedView in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
            {
                RadGrid DispenseGrid = (RadGrid)nestedView.FindControl("Dispense");

                foreach (GridDataItem item in DispenseGrid.Items)
                {
                    RadTextBox txtExpDate = (RadTextBox)item.FindControl("txtExpiryDate");
                    RadNumericTextBox txtSP = (RadNumericTextBox)item.FindControl("txtSellPrice");

                    if (dvsp.ToTable().Rows.Count > 0)
                    {
                        txtExpDate.Text = String.Format("{0:dd-MMM-yyyy}", dv.ToTable().Rows[0]["ExpiryDate"]);
                        txtSP.Text = dvsp.ToTable().Rows[0]["Selling Price"].ToString();
                        //RadNumericTextBox txtBAmount = (RadNumericTextBox)item.FindControl("txtBillAmount");
                        //long BillAmount = Math.BigMul(Convert.ToInt32(txtSP.Text), Convert.ToInt32(txtqtydispense.Text));
                        //txtBAmount.Text = BillAmount.ToString();
                    }
                    else
                    {
                        txtExpDate.Text = "";
                        txtSP.Text = "";
                    }
                }
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
        protected void OrderGrid_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                BindFunctions theBind = new BindFunctions();
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
        protected void Dispense_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            DataView dv = new DataView((DataTable)ViewState["TableDispense"]);
            dv.RowFilter = "DrugID=" + parentItem.GetDataKeyValue("DrugID").ToString() + "";
            DataTable dtfilter = dv.ToTable();
            (sender as RadGrid).DataSource = dtfilter;
        }
        protected void PatientInstruction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            DataView dv = new DataView((DataTable)ViewState["TableInstruct"]);
            dv.RowFilter = "DrugID=" + parentItem.GetDataKeyValue("DrugID").ToString() + "";
            DataTable dtInstruct = dv.ToTable();
            (sender as RadGrid).DataSource = dtInstruct;
        }
        protected void rgdrugmain_ItemCommand(object sender, GridCommandEventArgs e)
        {
            bool returnFlag = false;
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            {
                ((GridDataItem)e.Item).ChildItem.FindControl("InnerContainer").Visible = !e.Item.Expanded;
                returnFlag = ((GridDataItem)e.Item).ChildItem.FindControl("InnerContainer").Visible;
            }
            if (e.Item.Expanded)
                return;

            //if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            //{
            //    //GridDataItem parentItem = e.Item as GridDataItem;
            //    //RadGrid griddispense = parentItem.ChildItem.FindControl("Dispense") as RadGrid;
            //    //DataTable dtDispense = (DataTable)ViewState["TableDispense"];
            //    //foreach (GridDataItem item in griddispense.Items)
            //    //{
            //    //    string drugID = item.GetDataKeyValue("DrugID").ToString();

            //    //    RadNumericTextBox txtqtydispensed = (RadNumericTextBox)item.FindControl("txtQtyDispensed");
            //    //    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribeddispense");
            //    //    for (int i = 0; i < dtDispense.Rows.Count; i++)
            //    //    {
            //    //        if (dtDispense.Rows[i]["DrugID"].ToString() == drugID)
            //    //        {

            //    //            if (txtqtydispensed.Text != "")
            //    //            {
            //    //                dtDispense.Rows[i]["QtyDispensed"] = Convert.ToDecimal(txtqtydispensed.Text);
            //    //            }
            //    //            if (txtqtyprec.Text != "")
            //    //            {
            //    //                dtDispense.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);
            //    //            }
            //    //        }

            //    //    }
            //    //}
            //    //ViewState["TableDispense"] = dtDispense;
            //    //griddispense.Rebind();
            //    //RadGrid gridorder = parentItem.ChildItem.FindControl("OrdersGrid") as RadGrid;
            //    //DataTable dtupdateOrder = (DataTable)ViewState["TableOrder"];
            //    //foreach (GridDataItem item in gridorder.Items)
            //    //{
            //    //    string drugID = item.GetDataKeyValue("DrugID").ToString();
            //    //    RadNumericTextBox txtdose = (RadNumericTextBox)item.FindControl("txtDose");
            //    //    RadComboBox ddlfrequency = (RadComboBox)item.FindControl("rdcmbfrequency");
            //    //    RadNumericTextBox txtduration = (RadNumericTextBox)item.FindControl("txtDuration");
            //    //    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribed");
            //    //    RadButton chkProphylaxis = (RadButton)item.FindControl("chkProphylaxis");


            //    //    for (int i = 0; i < dtupdateOrder.Rows.Count; i++)
            //    //    {
            //    //        if (dtupdateOrder.Rows[i]["DrugID"].ToString() == drugID)
            //    //        {
            //    //            if (txtdose.Text != "")
            //    //            {
            //    //                dtupdateOrder.Rows[i]["Dose"] = Convert.ToDecimal(txtdose.Text);
            //    //            }
            //    //            if (ddlfrequency.SelectedItem.Value.ToString() != "")
            //    //            {
            //    //                dtupdateOrder.Rows[i]["Frequency"] = Convert.ToInt32(ddlfrequency.SelectedItem.Value);
            //    //            }
            //    //            if (txtduration.Text != "")
            //    //            {
            //    //                dtupdateOrder.Rows[i]["Duration"] = Convert.ToDecimal(txtduration.Text);
            //    //            }
            //    //            if (txtqtyprec.Text != "")
            //    //            {
            //    //                dtupdateOrder.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);
            //    //            }
            //    //            dtupdateOrder.Rows[i]["Prophylaxis"] = CheckedValue(chkProphylaxis.SelectedToggleState.Text);

            //    //            dtupdateOrder.AcceptChanges();

            //    //        }
            //    //    }

            //    //}
            //    //ViewState["TableOrder"] = dtupdateOrder;
            //    //gridorder.Rebind();

            //    //RadGrid gridInstruction = parentItem.ChildItem.FindControl("PatientInstruction") as RadGrid;
            //    //DataTable dtInstruction = (DataTable)ViewState["TableInstruct"];
            //    //foreach (GridDataItem item in gridInstruction.Items)
            //    //{
            //    //    string drugID = item.GetDataKeyValue("DrugID").ToString();
            //    //    RadTextBox txtInstruction = (RadTextBox)item.FindControl("txtPatientInstruction");
            //    //    foreach (DataRow theDR in dtupdateOrder.Rows)
            //    //    {
            //    //        if (theDR["DrugID"].ToString() == drugID)
            //    //        {

            //    //            if (txtInstruction.Text != "")
            //    //            {
            //    //                theDR["Instructions"] = Convert.ToString(txtInstruction.Text);
            //    //                theDR.EndEdit();
            //    //                dtupdateOrder.AcceptChanges();
            //    //            }
            //    //        }

            //    //    }
            //    //}
            //    //ViewState["TableInstruct"] = dtInstruction;
            //    //gridInstruction.Rebind();

            //    ////-----------------Refill
            //    //RadGrid gridRefill = parentItem.ChildItem.FindControl("Refill") as RadGrid;
            //    //DataTable dtRefill = (DataTable)ViewState["TableRefill"];
            //    //foreach (GridDataItem item in gridRefill.Items)
            //    //{
            //    //    string drugID = item.GetDataKeyValue("DrugID").ToString();

            //    //    RadNumericTextBox txtnooffile = (RadNumericTextBox)item.FindControl("txtRefill");
            //    //    RadDatePicker dtrefillexpiration = (RadDatePicker)item.FindControl("dtRefillExpiration");
            //    //    for (int i = 0; i < dtRefill.Rows.Count; i++)
            //    //    {
            //    //        if (dtRefill.Rows[i]["DrugID"].ToString() == drugID)
            //    //        {

            //    //            if (txtnooffile.Text != "")
            //    //            {
            //    //                dtRefill.Rows[i]["Refill"] = Convert.ToInt32(txtnooffile.Text);
            //    //            }
            //    //            if (dtrefillexpiration.SelectedDate != null)
            //    //            {
            //    //                if (dtrefillexpiration.SelectedDate.Value.ToString() != "")
            //    //                {
            //    //                    dtRefill.Rows[i]["RefillExpiration"] = dtrefillexpiration.SelectedDate.Value.ToString();
            //    //                }
            //    //            }
            //    //        }

            //    //    }
            //    //}
            //    //ViewState["TableRefill"] = dtRefill;
            //    //gridRefill.Rebind();
            //}
            //foreach (GridNestedViewItem item in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
            //{
            //    RadTabStrip TabStrip = (RadTabStrip)item.FindControl("TabStip1");
            //    if (Session["Orderid"].ToString() == "0")
            //    {
            //        TabStrip.Tabs[2].Enabled = false;
            //        TabStrip.Tabs[0].Focus();
            //        RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
            //        radpage.SelectedIndex = 0;
            //    }
            //    if (ViewState["RefillClick"] != null)
            //    {
            //        if (ViewState["RefillClick"].ToString() == "True")
            //        {
            //            TabStrip.Tabs[1].Enabled = false;
            //            RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
            //            radpage.SelectedIndex = 0;
            //        }
            //    }
            //}
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
        }
        int CheckedValue(string btnToggeState)
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
        private StringBuilder InsertMultiSelectList(DataTable DTMulti)
        {
            StringBuilder Insertcbl = new StringBuilder();
            foreach (object y in PnlDynamicElements.Controls)
            {
                if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                {
                    string TableExact = string.Empty;

                    foreach (Control x in ((Control)y).Controls)
                    {
                        if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                        {
                            string[] TableName = ((CheckBox)x).ID.Split('-');
                            if (TableName.Length == 5)
                            {
                                TableExact = TableName[3];
                                if (TableExact == "dtl_FB_multiselect")
                                {
                                    TableExact = "DTL_FB_" + TableName[2] + "";
                                    TableExact = TableExact.Replace(' ', '_');
                                }
                                //For DtlMultiSelect Field Table
                                foreach (DataRow theDRMulti in DTMulti.Rows)
                                {
                                    if (Convert.ToInt32(TableName[4]) == Convert.ToInt32(theDRMulti["FieldId"]))
                                    {
                                        if (((CheckBox)x).Checked == true)
                                        {
                                            Insertcbl.Append("Insert into [" + TableExact + "]([ptn_pk], [Visit_Pk], [LocationID], [" + TableName[2] + "], [UserID], [CreateDate])");
                                            Insertcbl.Append("values (" + PatientID + ",  IDENT_CURRENT('Ord_Visit')," + Session["AppLocationId"].ToString() + "," + TableName[1] + ",");
                                            Insertcbl.Append("" + Session["AppUserId"].ToString() + ", Getdate())");
                                        }

                                    }
                                }
                            }
                        }

                    }
                }

            }
            return Insertcbl;

        }
        private StringBuilder UpdateMultiSelectList(DataTable DTMulti)
        {
            StringBuilder Insertcbl = new StringBuilder();
            foreach (object y in PnlDynamicElements.Controls)
            {
                if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                {
                    string TableExact = string.Empty;

                    foreach (Control x in ((Control)y).Controls)
                    {
                        if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                        {
                            string[] TableName = ((CheckBox)x).ID.Split('-');
                            if (TableName.Length == 5)
                            {
                                TableExact = TableName[3];
                                if (TableExact == "dtl_FB_multiselect")
                                {
                                    TableExact = "DTL_FB_" + TableName[2] + "";
                                    TableExact = TableExact.Replace(' ', '_');
                                }
                                //For DtlMultiSelect Field Table
                                foreach (DataRow theDRMulti in DTMulti.Rows)
                                {
                                    if (Convert.ToInt32(TableName[4]) == Convert.ToInt32(theDRMulti["FieldId"]))
                                    {
                                        if (((CheckBox)x).Checked == true)
                                        {
                                            Insertcbl.Append("Insert into [" + TableExact + "]([ptn_pk], [Visit_Pk], [LocationID], [" + TableName[2] + "], [UserID], [CreateDate])");
                                            Insertcbl.Append("values (" + PatientID + ",  " + VisitID + "," + Session["AppLocationId"].ToString() + "," + TableName[1] + ",");
                                            Insertcbl.Append("" + Session["AppUserId"].ToString() + ", Getdate())");
                                        }

                                    }
                                }
                            }
                        }

                    }
                }

            }
            return Insertcbl;
        }
        private StringBuilder SaveCustomFormData(int PatientID, DataSet DS, int DQSaveChk)
        {
            ICustomForm MgrSaveUpdate = (ICustomForm)ObjectFactory.CreateInstance(ObjFactoryParameter);
            StringBuilder SbInsert = new StringBuilder();
            DataTable theDT = SetControlIDs(PnlDynamicElements);
            DataView theViewDT = new DataView(theDT);
            theDT = theViewDT.ToTable();
            DataTable theDTNoMulti = ((DataTable)ViewState["NoMulti"]);
            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                DataTable LnkDTUnique = theDTNoMulti.DefaultView.ToTable(true, "PDFTableName", "FeatureName").Copy();

                string GetValue = "";
                GetValue = "Select VisitTypeID from mst_visittype where (DeleteFlag = 0 or DeleteFlag is null) and VisitTypeId>100 and VisitName='" + LnkDTUnique.Rows[0][1] + "'";
                DataSet TempDS = MgrSaveUpdate.Common_GetSaveUpdate(GetValue);

                SbInsert.Append("Insert into [ord_visit](ptn_pk, LocationID, VisitDate, VisitType, DataQuality, UserID, Signature, CreateDate, ModuleId)");
                SbInsert.Append("values(" + PatientID + ", " + Session["AppLocationId"] + ", '" + txtpharmOrderedbyDate.Value + "', " + TempDS.Tables[0].Rows[0]["VisitTypeID"].ToString() + ", " + DQSaveChk + ", " + Session["AppUserId"] + ", " + ddlPharmOrderedbyName.SelectedValue + ", getdate()," + Session["TechnicalAreaId"].ToString() + ")");
                //              
                foreach (DataRow DR in LnkDTUnique.Rows)
                {
                    string quotes = "''''";
                    StringBuilder SbValues = new StringBuilder();
                    if (DR[0].ToString().ToUpper() == "DTL_CUSTOMFIELD")
                    {
                        string TableName = "DTL_FBCUSTOMFIELD_" + DR[1].ToString().Replace(' ', '_');
                        SbInsert.Append("Insert into [" + TableName + "](Ptn_pk,Visit_Pk,LocationId,UserID,CreateDate,");
                        SbValues.Append("Values(" + PatientID + ",IDENT_CURRENT('Ord_Visit')," + Session["AppLocationId"] + "," + Session["AppUserId"] + ", GetDate(),");
                    }
                    else if (DR[0].ToString().ToUpper() == "DTL_FB_MULTISELECT")
                    {
                        DataView DVMulti = new DataView(((DataTable)ViewState["LnkTable"]));
                        DVMulti.RowFilter = "ControlId=9";
                        StringBuilder SBMSelect = InsertMultiSelectList(DVMulti.ToTable());
                        SbInsert.Append(SBMSelect);
                    }
                    else if (Convert.ToString(DR[0]).ToUpper() == "ORD_PATIENTPHARMACYORDER")
                    {
                        SbInsert.Append("Insert into [" + DR[0] + "](Ptn_pk,Visitid,LocationId, UserID,CreateDate, OrderedBy,OrderedByDate,DispensedBy,DispensedByDate,");
                        SbValues.Append("Values(" + PatientID + ",IDENT_CURRENT('Ord_Visit')," + Session["AppLocationId"] + "," + Session["AppUserId"] + ", GetDate(),");
                        SbValues.Append("" + ddlPharmOrderedbyName.SelectedValue + ",'" + txtpharmOrderedbyDate.Value + "'," + ddlPharmDispensedbyName.SelectedValue + ", '" + txtpharmdispensedbydate.Value + "',");

                    }
                    else if (Convert.ToString(DR[0]).ToUpper() == "DTL_PATIENTBILLTRANSACTION".ToUpper())
                    {
                        SbInsert.Append("Insert into [" + DR[0] + "](Ptn_pk,Visitid,LocationId,UserID,CreateDate,");
                        SbValues.Append("Values(" + PatientID + ",IDENT_CURRENT('Ord_Visit')," + Session["AppLocationId"] + "," + Session["AppUserId"] + ", GetDate(),");
                    }
                    else if (DR[0].ToString().ToUpper() == "DTL_PATIENTPHARMACYRETURN")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "DTL_CUSTOMFORM")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "DTL_PATIENTPHARMACYORDER")
                    {
                        SbInsert.Append(" if not exists (Select * from ORD_PATIENTPHARMACYORDER where VisitId=IDENT_CURRENT('Ord_Visit')) Begin");
                        SbInsert.Append(" Insert into ORD_PATIENTPHARMACYORDER(Ptn_pk,Visitid,LocationId, UserID,CreateDate, OrderedBy,OrderedByDate,DispensedBy,DispensedByDate)");
                        SbValues.Append(" Values(" + PatientID + ",IDENT_CURRENT('Ord_Visit')," + Session["AppLocationId"] + "," + Session["AppUserId"] + ", GetDate(),");
                        SbValues.Append(" " + ddlPharmOrderedbyName.SelectedValue + ",'" + txtpharmOrderedbyDate.Value + "'," + ddlPharmDispensedbyName.SelectedValue + ", '" + txtpharmdispensedbydate.Value + "') End ");
                    }
                    else if (DR[0].ToString().ToUpper() == "BSA")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "AGE")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "DOB")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "PRINTPRESCRIPTION")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "PRINTLABEL")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "LASTDISPENSEDREGIMEN")
                    {
                    }
                    else if (DR[0].ToString().ToUpper() == "LASTDISPENSEDDATE")
                    {
                    }
                    else if (DR[0] != System.DBNull.Value)
                    {
                        SbInsert.Append("Insert into [" + DR[0] + "](Ptn_pk,Visit_Pk,LocationId,UserID,CreateDate,");
                        SbValues.Append("Values(" + PatientID + ",IDENT_CURRENT('Ord_Visit')," + Session["AppLocationId"] + "," + Session["AppUserId"] + ", GetDate(),");
                    }
                    //Generating Query to Insert values other than MultiSelect
                    foreach (DataRow DRlnk in theDT.Rows)
                    {
                        if (DR["PDFTableName"].ToString().ToUpper() == DRlnk["TableName"].ToString().ToUpper() && DRlnk["TableName"].ToString().ToUpper() != "DTL_PATIENTPHARMACYRETURN")
                        {
                            if (DRlnk["Value"].ToString() != "" && DRlnk["Column"].ToString() != "")
                            {
                                SbInsert.Append("[" + DRlnk["Column"] + "],");
                                if (DRlnk["Value"].ToString() == "")
                                {
                                    SbValues.Append(DRlnk["Value"] + "" + quotes + "" + ",");
                                }
                                else
                                {
                                    SbValues.Append("'" + DRlnk["Value"] + "',");
                                }
                            }
                        }
                    }
                    if (DR[0] != System.DBNull.Value && DR[0].ToString().ToUpper() != "DTL_PATIENTPHARMACYORDER")
                    {
                        SbInsert.Remove(SbInsert.Length - 1, 1);
                        SbInsert.Append(" )");
                    }
                    if (DR[0] != System.DBNull.Value && DR[0].ToString().ToUpper() != "DTL_PATIENTPHARMACYORDER")
                    {
                        if (SbValues.Length > 0)
                        {
                            SbValues.Remove(SbValues.Length - 1, 1);
                            SbValues.Append(" )");
                        }
                    }
                    SbInsert.Append(SbValues);
                    TempDS.Dispose();

                }
                SbInsert.Append("Select LocationID, Ident_Current('Ord_Visit')[VisitID] from ord_visit where Visit_ID=Ident_Current('Ord_Visit')");
                SbInsert.Append("Select ptn_pharmacy_pk[PharmacyID] from ord_patientpharmacyorder where VisitID=Ident_Current('Ord_Visit')");
            }
            return SbInsert;
        }
        private StringBuilder UpdateCustomFormData(int PatientID, int FeatureID, int VisitID, int LocationID, DataSet DS, int DQChk)
        {
            ICustomForm MgrSaveUpdate = (ICustomForm)ObjectFactory.CreateInstance(ObjFactoryParameter);
            DataTable theDT = SetControlIDs(PnlDynamicElements);
            DataView theViewDT = new DataView(theDT);
            theDT = theViewDT.ToTable();
            DataTable theDTNoMulti = ((DataTable)ViewState["NoMulti"]);
            DataTable LnkDT = theDTNoMulti.DefaultView.ToTable(true, "PDFTableName", "FeatureName").Copy();
            StringBuilder SbUpdateParam = new StringBuilder();
            StringBuilder SbUpdateColMstPatient = new StringBuilder();
            StringBuilder SbUpdateValMstPatient = new StringBuilder();
            //Update statement if already exists
            foreach (DataRow DR in LnkDT.Rows)
            {
                Boolean Valuethere = false;
                foreach (DataRow DRlnk in theDT.Rows)
                {
                    if (DR["PDFTableName"].ToString().ToUpper() == DRlnk["TableName"].ToString().ToUpper() && Convert.ToString(DRlnk["Value"]) != "")
                    {
                        Valuethere = true;
                    }

                }
                StringBuilder builder = new StringBuilder();
                if (DR[0].ToString().ToUpper() == "DTL_CUSTOMFIELD" && Valuethere == true)
                {
                    builder = new StringBuilder(" if exists(Select * from [DTL_FBCUSTOMFIELD_" + DR[1].ToString().Replace(' ', '_') + "] where ptn_pk=" + PatientID + "");
                    builder.Append(" and Visit_pk=" + VisitID + " and LocationID=" + LocationID + ")");
                    builder.Append(" Begin ");
                    builder.Append("UPDATE [DTL_FBCUSTOMFIELD_" + DR[1].ToString().Replace(' ', '_') + "] SET ");
                }
                else if (DR[0].ToString().ToUpper() == "DTL_FB_MULTISELECT")
                {
                    builder = new StringBuilder();
                    StringBuilder SBMSelect = new StringBuilder();
                    DataView DVMulti = new DataView(((DataTable)ViewState["LnkTable"]));
                    DVMulti.RowFilter = "ControlId=9";
                    foreach (DataRow theDRSelect in DVMulti.ToTable().Rows)
                    {
                        String TableExact = "DTL_FB_" + theDRSelect[5] + "";
                        TableExact = TableExact.Replace(' ', '_');
                        builder.Append("Delete From [" + TableExact + "] where [ptn_pk]=" + PatientID + " and [Visit_Pk]=" + VisitID + " and [LocationID]=" + Session["AppLocationId"].ToString() + "");
                    }
                    SBMSelect = UpdateMultiSelectList(DVMulti.ToTable());
                    builder.Append(SBMSelect);
                    SbUpdateParam.Append(builder);
                }
                else if (Convert.ToString(DR[0]).ToUpper() == "ord_patientpharmacyorder".ToUpper() || Convert.ToString(DR[0]).ToUpper() == "Dtl_PatientBillTransaction".ToUpper() && Valuethere == true)
                {
                    builder = new StringBuilder(" if exists(Select * from " + DR[0] + " where ptn_pk=" + PatientID + "");
                    builder.Append(" and Visitid=" + VisitID + " and LocationID=" + LocationID + ")");
                    builder.Append(" Begin ");
                    builder.Append("UPDATE " + DR[0] + " SET ");
                    if (DR[0].ToString().ToUpper() == "ord_patientpharmacyorder".ToUpper())
                    {
                        builder.Append(" OrderedBy=" + ddlPharmOrderedbyName.SelectedValue + ", OrderedByDate='" + txtpharmOrderedbyDate.Value + "',");
                        builder.Append(" DispensedBy=" + ddlPharmDispensedbyName.SelectedValue + ", DispensedByDate='" + txtpharmdispensedbydate.Value + "',");
                    }
                }
                else if (DR[0].ToString().ToUpper() == "AGE")
                {
                }
                else if (DR[0].ToString().ToUpper() == "DOB")
                {
                }
                else if (DR[0].ToString().ToUpper() == "BSA")
                {
                }
                else if (DR[0].ToString().ToUpper() == "PRINTPRESCRIPTION")
                {
                }
                else if (DR[0].ToString().ToUpper() == "PRINTLABEL")
                {
                }
                else if (DR[0].ToString().ToUpper() == "LASTDISPENSEDREGIMEN")
                {
                }
                else if (DR[0].ToString().ToUpper() == "LASTDISPENSEDDATE")
                {
                }
                else if (DR[0].ToString().ToUpper() == "DTL_FB_MULTISELECT")
                {
                }
                else if (DR[0].ToString().ToUpper() == "DTL_PATIENTPHARMACYORDER")
                {

                }
                else if (Valuethere == true)
                {
                    builder = builder = new StringBuilder(" if exists(Select * from " + DR[0] + " where ptn_pk=" + PatientID + "");
                    builder.Append(" and Visit_pk=" + VisitID + " and LocationID=" + LocationID + ")");
                    builder.Append(" Begin ");
                    builder.Append("UPDATE " + DR[0] + " SET ");
                }

                foreach (DataRow DRlnk in theDT.Rows)
                {
                    if (DR["PDFTableName"].ToString().ToUpper() == DRlnk["TableName"].ToString().ToUpper() && Convert.ToString(DRlnk["column"]) != "" && Convert.ToString(DRlnk["value"]) != "")
                    {
                        builder.Append("[" + DRlnk["column"] + "]").Append(" = ").Append("'" + DRlnk["value"] + "'").Append(",");
                    }
                }

                if (Convert.ToString(DR[0]).ToUpper() == "ord_patientpharmacyorder".ToUpper() || Convert.ToString(DR[0]).ToUpper() == "Dtl_PatientBillTransaction".ToUpper() && Valuethere == true)
                {
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append(" where Ptn_Pk=" + PatientID + " and Visitid=" + VisitID + " and LocationID=" + LocationID + "");
                    builder.Append(" End ");
                    SbUpdateParam.Append(builder);
                }
                else if (Valuethere == true && builder.Length > 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append(" where Ptn_Pk=" + PatientID + " and Visit_pk=" + VisitID + " and LocationID=" + LocationID + "");
                    builder.Append(" End ");
                    SbUpdateParam.Append(builder);
                }
            }
            //Insert Statement
            foreach (DataRow DR in LnkDT.Rows)
            {
                StringBuilder builder = new StringBuilder();
                if (DR[0].ToString().ToUpper() == "DTL_CUSTOMFIELD")
                {
                }
                else if (DR[0].ToString().ToUpper() == "DTL_FB_MULTISELECT")
                {
                }
                else if (DR[0].ToString().ToUpper() == "AGE")
                {
                }
                else if (DR[0].ToString().ToUpper() == "DOB")
                {
                }
                else if (DR[0].ToString().ToUpper() == "BSA")
                {
                }
                else if (DR[0].ToString().ToUpper() == "PRINTPRESCRIPTION")
                {
                }
                else if (DR[0].ToString().ToUpper() == "PRINTLABEL")
                {
                }
                else if (DR[0].ToString().ToUpper() == "LASTDISPENSEDREGIMEN")
                {
                }
                else if (DR[0].ToString().ToUpper() == "LASTDISPENSEDDATE")
                {
                }
                else if (DR[0].ToString().ToUpper() == "dtl_patientpharmacyreturn".ToUpper())
                {
                }
                else if (DR[0].ToString().ToUpper() == "DTL_PATIENTPHARMACYORDER")
                {
                }
                else if (Convert.ToString(DR[0]).ToUpper() == "ord_patientpharmacyorder".ToUpper() || Convert.ToString(DR[0]).ToUpper() == "Dtl_PatientBillTransaction".ToUpper())
                {
                    builder = builder = new StringBuilder(" if not exists(Select * from " + DR[0] + " where ptn_pk=" + PatientID + "");
                    builder.Append(" and Visitid=" + VisitID + " and LocationID=" + LocationID + ")");
                    builder.Append(" Begin ");
                    builder.Append(" Insert into [" + DR[0] + "](ptn_pk, Visitid, LocationID,UserID,CreateDate,");
                    foreach (DataRow DRlnk in theDT.Rows)
                    {
                        if (DRlnk["value"].ToString() != "" && DR["PDFTableName"].ToString().ToUpper() == DRlnk["TableName"].ToString().ToUpper())
                        {
                            builder.Append(DRlnk["column"]).Append(",");
                        }
                    }
                    builder.Remove(builder.Length - 1, 1).Append(")");
                    builder.Append(" Values(" + PatientID + "," + VisitID + ", " + LocationID + "," + Session["AppUserId"] + ", GetDate(),");
                    foreach (DataRow DRlnk in theDT.Rows)
                    {
                        if (DRlnk["value"].ToString() != "" && DR["PDFTableName"].ToString().ToUpper() == DRlnk["TableName"].ToString().ToUpper())
                        {
                            builder.Append("'" + DRlnk["value"] + "'").Append(",");
                        }
                    }
                    builder.Remove(builder.Length - 1, 1).Append(")");
                    builder.Append(" End ");
                    SbUpdateParam.Append(builder);
                }
                else
                {
                    builder = builder = new StringBuilder(" if not exists(Select * from " + DR[0] + " where ptn_pk=" + PatientID + "");
                    builder.Append(" and Visit_pk=" + VisitID + " and LocationID=" + LocationID + ")");
                    builder.Append(" Begin ");
                    builder.Append(" Insert into [" + DR[0] + "](ptn_pk, Visit_pk, LocationID,UserID,CreateDate,");
                    foreach (DataRow DRlnk in theDT.Rows)
                    {
                        if (DRlnk["value"].ToString() != "" && DR["PDFTableName"].ToString() == DRlnk["TableName"].ToString())
                        {
                            builder.Append(DRlnk["column"]).Append(",");
                        }
                    }
                    builder.Remove(builder.Length - 1, 1).Append(")");
                    builder.Append(" Values(" + PatientID + "," + VisitID + ", " + LocationID + "," + Session["AppUserId"] + ", GetDate(),");
                    foreach (DataRow DRlnk in theDT.Rows)
                    {
                        if (DRlnk["value"].ToString() != "" && DR["PDFTableName"].ToString() == DRlnk["TableName"].ToString())
                        {
                            builder.Append("'" + DRlnk["value"] + "'").Append(",");
                        }
                    }
                    builder.Remove(builder.Length - 1, 1).Append(")");
                    builder.Append(" End ");
                    SbUpdateParam.Append("Select Visit_Id[VisitID] from ord_visit where Ptn_Pk=" + PatientID + " and Visit_Id=" + VisitID + " and LocationID=" + LocationID + "");
                    SbUpdateParam.Append("Select ptn_pharmacy_pk[PharmacyID] from ord_patientpharmacyorder where VisitID=" + VisitID + "");
                    SbUpdateParam.Append(builder);
                }
            }
            return SbUpdateParam;
        }
        private DataTable SetControlIDs(Control theControl)
        {
            DataTable TempDT = new DataTable();
            DataColumn Column = new DataColumn("Column");
            Column.DataType = System.Type.GetType("System.String");
            TempDT.Columns.Add(Column);

            DataColumn Control = new DataColumn("FieldID");
            Control.DataType = System.Type.GetType("System.String");
            TempDT.Columns.Add(Control);

            DataColumn Value = new DataColumn("Value");
            Value.DataType = System.Type.GetType("System.String");
            TempDT.Columns.Add(Value);

            DataColumn TableName = new DataColumn("TableName");
            TableName.DataType = System.Type.GetType("System.String");
            TempDT.Columns.Add(TableName);

            DataRow DRTemp;
            DRTemp = TempDT.NewRow();
            String Time24 = "", Time12 = "", TimeAMPM = "";
            foreach (object x in theControl.Controls)
            {
                if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    string[] str = ((TextBox)x).ID.Split('-');
                    if (str[2] != "BMI")
                    {
                        DRTemp = TempDT.NewRow();
                        DRTemp["Column"] = str[1];
                        if (str[0].ToString() == "TXTDT")
                        {
                            if (((TextBox)x).Text.Length > 8)
                            {
                                DRTemp["Value"] = ((TextBox)x).Text;
                            }
                            else
                            {
                                if (((TextBox)x).Text != "")
                                {
                                    DRTemp["Value"] = "01-" + ((TextBox)x).Text;
                                }
                            }
                        }
                        else
                        {

                            if (((TextBox)x).Text != "")
                            {
                                DRTemp["Value"] = ((TextBox)x).Text;
                            }
                            else
                            {
                                DRTemp["Value"] = "";
                            }
                        }

                        DRTemp["TableName"] = str[2];
                        DRTemp["FieldID"] = str[3];
                        TempDT.Rows.Add(DRTemp);
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                {

                    DRTemp = TempDT.NewRow();
                    string[] str = ((HtmlInputRadioButton)x).ID.Split('-');
                    if (((HtmlInputRadioButton)x).ID == "RADIO1-" + str[1] + "-" + str[2] + "-" + str[3])
                    {
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            DRTemp["Column"] = str[1];
                            if (((HtmlInputRadioButton)x).Visible == true)
                                DRTemp["Value"] = "1";
                            else
                                DRTemp["Value"] = "";
                        }
                    }
                    else if (((HtmlInputRadioButton)x).ID == "RADIO2-" + str[1] + "-" + str[2] + "-" + str[3])
                    {
                        if (((HtmlInputRadioButton)x).Checked == true)
                        {
                            DRTemp["Column"] = str[1];
                            if (((HtmlInputRadioButton)x).Visible == true)
                                DRTemp["Value"] = "0";
                            else
                                DRTemp["Value"] = "";
                        }

                    }

                    DRTemp["TableName"] = str[2];
                    DRTemp["FieldID"] = str[3];
                    TempDT.Rows.Add(DRTemp);
                }
                if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    DRTemp = TempDT.NewRow();
                    string[] str = ((DropDownList)x).ID.Split('-');
                    if (str[3].Contains("12Hr"))
                    {
                        Time12 = ((DropDownList)x).SelectedValue;
                    }
                    else if (str[3].Contains("24Hr"))
                    {
                        Time24 = ((DropDownList)x).SelectedValue;
                    }
                    else if (str[3].Contains("Min") && Time12 != "")
                    {
                        TimeAMPM = Time12 + ":" + ((DropDownList)x).SelectedValue;
                        Time12 = "";
                    }
                    else if (str[3].Contains("Min") && Time24 != "")
                    {
                        Time24 = Time24 + ":" + ((DropDownList)x).SelectedValue;
                        DRTemp["Column"] = str[1];
                        DRTemp["Value"] = Time24;
                        DRTemp["TableName"] = str[2];
                        DRTemp["FieldID"] = str[3];
                        TempDT.Rows.Add(DRTemp);
                        Time24 = "";
                    }
                    else if (str[3].Contains("AMPM"))
                    {
                        TimeAMPM = TimeAMPM + " " + ((DropDownList)x).SelectedValue;
                        DRTemp["Column"] = str[1];
                        DRTemp["Value"] = TimeAMPM;
                        DRTemp["TableName"] = str[2];
                        DRTemp["FieldID"] = str[3];
                        TempDT.Rows.Add(DRTemp);
                        TimeAMPM = "";
                    }
                    else
                    {
                        if (str[0] != "SELECTLISTAuto")
                        {
                            DRTemp["Column"] = str[1];
                            if (((DropDownList)x).Enabled == true)
                                DRTemp["Value"] = ((DropDownList)x).SelectedValue;
                            else
                                DRTemp["Value"] = "";
                            DRTemp["TableName"] = str[2];
                            DRTemp["FieldID"] = str[3];
                            TempDT.Rows.Add(DRTemp);
                        }
                    }
                }
                if (x.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                {
                    DRTemp = TempDT.NewRow();
                    string[] str = ((CheckBox)x).ID.Split('-');
                    DRTemp["Column"] = str[1];
                    if (((CheckBox)x).Visible == true)
                    {
                        if (((CheckBox)x).Checked == true)
                        {
                            DRTemp["Value"] = 1;
                        }
                        else
                        {
                            DRTemp["Value"] = "";
                        }
                    }
                    else
                    {
                        DRTemp["Value"] = "";
                    }
                    DRTemp["TableName"] = str[2];
                    DRTemp["FieldID"] = str[3];
                    TempDT.Rows.Add(DRTemp);
                }

                if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputCheckBox))
                {
                    DRTemp = TempDT.NewRow();
                    string[] str = ((HtmlInputCheckBox)x).ID.Split('-');
                    DRTemp["Column"] = str[1];
                    if (((HtmlInputCheckBox)x).Visible == true)
                    {
                        if (((HtmlInputCheckBox)x).Checked == true)
                        {
                            DRTemp["Value"] = 1;
                        }
                        else
                        {
                            DRTemp["Value"] = 0;
                        }
                    }
                    else
                    {
                        DRTemp["Value"] = "";
                    }
                    DRTemp["TableName"] = str[2];
                    DRTemp["FieldID"] = str[3];
                    TempDT.Rows.Add(DRTemp);

                }

            }
            return TempDT;
        }
        private void SaveCancel()
        {
            //--- For Cancel event, on saving the form ---
            string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
            script += "var ans;\n";
            script += "ans=window.confirm('Data on Pharmacy Form saved successfully?');\n";
            script += "if (ans==true)\n";
            script += "{\n";
            script += "window.location.href='../ClinicalForms/frmPatient_History.aspx?sts=" + 0 + "';\n";
            script += "}\n";
            script += "else \n";
            script += "{\n";
            script += "window.location.href='frmPharmacy_Custom.aspx';\n";
            script += "}\n";
            script += "</script>\n";
            RegisterStartupScript("confirm", script);
        }
        private void UpdateCancel()
        {
            //--- For Cancel event, on updating the form ---
            string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
            script += "var ans;\n";
            script += "ans=window.confirm('Data on Pharmacy Form updated successfully?');\n";
            script += "if (ans==true)\n";
            script += "{\n";
            script += "window.location.href='../ClinicalForms/frmPatient_History.aspx?sts=" + 0 + "';\n";
            script += "}\n";
            script += "else \n";
            script += "{\n";
            script += "window.location.href='frmPharmacy_Custom.aspx';\n";
            script += "}\n";
            script += "</script>\n";
            RegisterStartupScript("confirm", script);
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
        private DataSet ReadDrugs()
        {
            DataSet theDS = new DataSet();
            DataTable dtupdateOrder = new DataTable();
            DataTable dtRefill = new DataTable();
            DataTable dtDispense = new DataTable();
            foreach (GridNestedViewItem nestedView in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
            {
                RadGrid gridOrdersGrid = (RadGrid)nestedView.FindControl("OrdersGrid");
                dtupdateOrder = (DataTable)ViewState["TableOrder"];
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

                RadGrid gridPInstructionGrid = (RadGrid)nestedView.FindControl("PatientInstruction");
                foreach (GridDataItem item in gridPInstructionGrid.Items)
                {
                    string drugID = item.GetDataKeyValue("DrugID").ToString();
                    RadTextBox txtInstruction = (RadTextBox)item.FindControl("txtPatientInstruction");
                    foreach (DataRow row in dtupdateOrder.Rows)
                    {
                        if (row["DrugID"].ToString() == drugID)
                        {
                            objDetails.Drug_Pk = Convert.ToInt32(drugID);
                            if (txtInstruction.Text != "")
                            {
                                row["Instructions"] = Convert.ToString(txtInstruction.Text);
                                row.EndEdit();
                                dtupdateOrder.AcceptChanges();
                            }
                        }

                    }
                }
                ViewState["TableOrder"] = dtupdateOrder;

                //-----------Refill
                RadGrid gridRefill = (RadGrid)nestedView.FindControl("Refill");
                dtRefill = (DataTable)ViewState["TableRefill"];
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

                //-----------Dispense
                RadGrid gridDispense = (RadGrid)nestedView.FindControl("Dispense");
                dtDispense = (DataTable)ViewState["TableDispense"];
                foreach (GridDataItem item in gridDispense.Items)
                {
                    string drugID = item.GetDataKeyValue("DrugID").ToString();
                    RadNumericTextBox txtqtydispensed = (RadNumericTextBox)item.FindControl("txtQtyDispensed");
                    RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribeddispense");
                    RadComboBox rdcmbBatch = (RadComboBox)item.FindControl("rdcmbBatch");
                    RadTextBox txtexpdate = (RadTextBox)item.FindControl("txtExpiryDate");
                    RadNumericTextBox txtSPrice = (RadNumericTextBox)item.FindControl("txtSellPrice");
                    RadNumericTextBox txtBillamount = (RadNumericTextBox)item.FindControl("txtBillAmount");
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
                            dtDispense.Rows[i]["BatchNo"] = rdcmbBatch.SelectedValue;

                            if (txtexpdate.Text != "")
                            {
                                dtDispense.Rows[i]["ExpiryDate"] = txtexpdate.Text;

                            }

                            if (txtSPrice.Text != "")
                            {
                                dtDispense.Rows[i]["SellPrice"] = Convert.ToDecimal(txtSPrice.Text);

                            }

                            if (txtBillamount.Text != "")
                            {
                                dtDispense.Rows[i]["BillAmount"] = Convert.ToDecimal(txtBillamount.Text);

                            }
                        }

                    }
                }
                ViewState["TableDispense"] = dtDispense;
            }
            theDS.Tables.Add(dtupdateOrder);
            theDS.Tables.Add(dtRefill);
            theDS.Tables.Add(dtDispense);
            //end main grid
            return theDS;
        }
        protected void rgdrugmain_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DrugID"].ToString();
            DataTable table = (DataTable)ViewState["TableDrug"];
            if (table.Rows.Find(ID) != null)
            {
                table.Rows.Find(ID).Delete();
                table.AcceptChanges();
                if (table.Rows.Count == 0)
                {
                    ViewState["TableDrug"] = null;
                }
                else
                {
                    ViewState["TableDrug"] = table;
                }

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
            if (!object.Equals(ViewState["TableDrug"], null))
            {
                if (((DataTable)ViewState["TableDrug"]).Rows.Count > 0)
                {
                    rgdrugmain.DataSource = (DataTable)ViewState["TableDrug"];
                    rgdrugmain.DataBind();
                }
            }

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {

            IPediatric MgrSaveUpdate = (IPediatric)ObjectFactory.CreateInstance(ObjFactoryPharmacyParameter);
            DataSet theDS = new DataSet();
            if (Convert.ToInt32(Session["PatientVisitId"]) == 0)
            {
                theDS = ReadDrugs();
                if (FieldValidation(theDS.Tables[0]) == false)
                {
                    return;
                }
                string msg = ValidationMessage();
                if (msg.Length > 51)
                {
                    MsgBuilder theBuilder1 = new MsgBuilder();
                    theBuilder1.DataElements["MessageText"] = msg;
                    IQCareMsgBox.Show("#C1", theBuilder1, this);
                    return;
                }
                int PatientID = Convert.ToInt32(Session["PatientId"]);
                StringBuilder Insert = SaveCustomFormData(PatientID, theDS, 0);
                DataSet TempDS = MgrSaveUpdate.SaveUpdate_CustomPharmacy(Insert.ToString(), theDS, Convert.ToInt32(Session["AppUserId"]));
                Session["PatientVisitId"] = TempDS.Tables[0].Rows[0]["VisitID"].ToString();
                Session["ServiceLocationId"] = TempDS.Tables[0].Rows[0]["LocationID"].ToString();
                SaveCancel();
            }
            else if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
            {
                int FeatureID = Convert.ToInt32(Session["FeatureID"]);
                int PatientID = Convert.ToInt32(Session["PatientId"]);
                int VisitID = Convert.ToInt32(Session["PatientVisitId"]);
                int LocationID = Convert.ToInt32(Session["ServiceLocationId"]);
                theDS = ReadDrugs();
                if (FieldValidation(theDS.Tables[2]) == false)
                {
                    return;
                }
                string msg = ValidationMessage();
                if (msg.Length > 51)
                {
                    MsgBuilder theBuilder1 = new MsgBuilder();
                    theBuilder1.DataElements["MessageText"] = msg;
                    IQCareMsgBox.Show("#C1", theBuilder1, this);
                    return;
                }
                StringBuilder Update = UpdateCustomFormData(PatientID, FeatureID, VisitID, LocationID, theDS, 0);
                DataSet TempDS = MgrSaveUpdate.SaveUpdate_CustomPharmacy(Update.ToString(), theDS, Convert.ToInt32(Session["AppUserId"]));
                Session["PatientVisitId"] = TempDS.Tables[0].Rows[0]["VisitID"].ToString();
                UpdateCancel();
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string theScript;
            theScript = "<script language='javascript' id='DrgPopup'>\n";
            theScript += "window.open('../Pharmacy/frmprintdialog.aspx?visitID=" + VisitID + "&ptnpk=" + PatientID + "' ,'DrugSelection','toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=700,height=350,scrollbars=yes');\n";
            theScript += "</script>\n";
            Page.RegisterStartupScript("DrgPopup", theScript);

        }
        private Boolean checkARVmedication(DataTable theDT)
        {
            if (theDT.Rows.Count > 0)
            {
                IQCareUtils theUtils = new IQCareUtils();
                if (((DataSet)ViewState["MasterData"]).Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in theDT.Rows)
                    {

                        DataView theDV = new DataView(((DataSet)ViewState["MasterData"]).Tables[0]);
                        theDV.RowFilter = "DrugTypeId=37 and Drug_Pk = " + row["DrugId"];
                        DataTable theSelExistsDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
                        if (theSelExistsDT.Rows.Count > 0)
                            return true;
                    }


                }
            }
            return false;
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session["Orderid"] != null)
            {
                if (Convert.ToInt32(Session["Orderid"]) == 1)
                {
                    foreach (GridDataItem griditem in rgdrugmain.Items)
                    {
                        CommandEventArgs arg = new CommandEventArgs("ExpandCollapse", griditem);
                        GridCommandEventArgs evarg = new GridCommandEventArgs(griditem, null, arg);
                        rgdrugmain_ItemCommand(sender, evarg);
                        griditem.Expanded = true;
                        griditem.ChildItem.FindControl("InnerContainer").Visible = true;
                        //Session["IsFirstLoad"] = null;
                    }
                }
            }

        }
        private Boolean FieldValidation(DataTable theDrugDT)
        {
            IIQCareSystem IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            DateTime theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
            IQCareUtils theUtils = new IQCareUtils();

            IEnumerable<TextBox> textBoxes = PnlDynamicElements.Controls.OfType<TextBox>();
            foreach (TextBox textBox in textBoxes)
            {
                // do stuff
                if (textBox.ID.ToString().ToUpper() == "TXT-WEIGHT-ORD_PATIENTPHARMACYORDER-303")
                {
                    //TextBox txtWeight = (TextBox)Page.FindControl("TXT-Weight-ord_PatientPharmacyOrder-303");
                    if (textBox.Text.Trim() == "")
                    {

                        MsgBuilder theMsg = new MsgBuilder();
                        theMsg.DataElements["Control"] = "Weight";
                        IQCareMsgBox.Show("BlankTextBox", theMsg, this);
                        return false;

                    }
                    else
                    {
                        if (Convert.ToDecimal(textBox.Text.ToString()) <= 0)
                        {
                            MsgBuilder theMsg = new MsgBuilder();
                            theMsg.DataElements["Control"] = "Weight";
                            IQCareMsgBox.Show("GreatThanZero", theMsg, this);
                            return false;
                        }
                    }
                }

                else if (textBox.ID.ToString().ToUpper() == "TXT-HEIGHT-ORD_PATIENTPHARMACYORDER-304")
                {
                    //TextBox txtHeight = (TextBox)Page.FindControl("TXT-Height-ord_PatientPharmacyOrder-317");
                    if (textBox.Text.Trim() == "")
                    {

                        MsgBuilder theMsg = new MsgBuilder();
                        theMsg.DataElements["Control"] = "Height";
                        IQCareMsgBox.Show("BlankTextBox", theMsg, this);
                        return false;

                    }
                    else
                    {
                        if (Convert.ToDecimal(textBox.Text.ToString()) <= 0)
                        {
                            MsgBuilder theMsg = new MsgBuilder();
                            theMsg.DataElements["Control"] = "Height";
                            IQCareMsgBox.Show("GreatThanZero", theMsg, this);
                            return false;
                        }
                    }
                }
            }
            if (checkARVmedication(theDrugDT))
            {
                IEnumerable<DropDownList> dropboxes = PnlDynamicElements.Controls.OfType<DropDownList>();
                foreach (DropDownList dropbox in dropboxes)
                {
                    // do stuff
                    if (dropbox.ID.ToString().ToUpper() == "SELECTLIST-PROGID-ORD_PATIENTPHARMACYORDER-306")
                    {

                        if (dropbox.SelectedIndex == 0)
                        {

                            MsgBuilder theMsg = new MsgBuilder();
                            theMsg.DataElements["Control"] = "Treatment Program";
                            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
                            return false;

                        }

                    }
                    // do stuff
                    if (dropbox.ID.ToString().ToUpper() == "SELECTLIST-REGIMENLINE-ORD_PATIENTPHARMACYORDER-309")
                    {
                        //TextBox txtWeight = (TextBox)Page.FindControl("TXT-Weight-ord_PatientPharmacyOrder-303");
                        if (dropbox.SelectedIndex == 0)
                        {

                            MsgBuilder theMsg = new MsgBuilder();
                            theMsg.DataElements["Control"] = "Regimen line";
                            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
                            return false;

                        }

                    }
                }
            }
            if (ddlPharmOrderedbyName.SelectedIndex == 0)
            {
                MsgBuilder theMsg = new MsgBuilder();
                theMsg.DataElements["Control"] = "Prescribed By";
                IQCareMsgBox.Show("BlankDropDown", theMsg, this);
                return false;
            }
            if (Session["SCMModule"] != null)
            {
                IEnumerable<DropDownList> dropdown = PnlDynamicElements.Controls.OfType<DropDownList>();
                foreach (DropDownList DDL in dropdown)
                {
                    if (DDL.ID.ToString().ToUpper() == "SELECTLIST-STOREID-ORD_PATIENTPHARMACYORDER-317")
                    {
                        //DropDownList theDDlPharmStore = (DropDownList)Page.FindControl("SELECTLIST-StoreId-ord_PatientPharmacyOrder-317");
                        if (DDL.SelectedIndex == 0)
                        {
                            MsgBuilder theMsg = new MsgBuilder();
                            theMsg.DataElements["Control"] = "Pharmacy Store";
                            IQCareMsgBox.Show("BlankDropDown", theMsg, this);
                            return false;
                        }
                    }
                }
            }

            if (theDrugDT.Rows.Count == 0)
            {

                IQCareMsgBox.Show("PharmacyIncompleteData", this);
                return false;
            }
            else
            {

                for (int i = 0; i < theDrugDT.Rows.Count; i++)
                {
                    for (int y = 0; y < theDrugDT.Columns.Count - 1; y++)
                    {
                        if (VisitID == 0)
                        {
                            if (Convert.ToString(theDrugDT.Rows[i][y]) == "")
                            {
                                IQCareMsgBox.Show("PharmacyIncompleteData", this);
                                return false;
                            }
                        }
                        else if (VisitID > 0)
                        {
                            if (Convert.ToString(theDrugDT.Rows[i]["QtyDispensed"]) == "0")
                            {
                                IQCareMsgBox.Show("PharmacyIncompleteData", this);
                                return false;
                            }
                        }
                    }
                }
            }

            if (Session["Paperless"].ToString() == "0")
            {
                if (Session["SCMModule"] == null)
                {
                    if (ddlPharmDispensedbyName.SelectedIndex == 0)//dispence by
                    {
                        MsgBuilder theMsg = new MsgBuilder();
                        theMsg.DataElements["Control"] = "Dispensed By";
                        IQCareMsgBox.Show("BlankDropDown", theMsg, this);
                        return false;
                    }
                    else if (txtpharmdispensedbydate.Value.Trim() == "") // dispensed by date
                    {
                        MsgBuilder theMsg = new MsgBuilder();
                        theMsg.DataElements["Control"] = "DispensedByDate";
                        IQCareMsgBox.Show("BlankTextBox", theMsg, this);
                        return false;

                    }
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
            if (txtpharmdispensedbydate.Value.Trim() != "")
            {
                DateTime theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmdispensedbydate.Value.Trim()));
                if (Session["EnrolmentDate"] != null)
                {
                    DateTime theEnrolmentDate = Convert.ToDateTime(Session["EnrolmentDate"].ToString());
                    if (theEnrolmentDate > theVisitDate)
                    {
                        IQCareMsgBox.Show("PharmacyDetailDispensedDate", this);
                        txtpharmdispensedbydate.Focus();
                        return false;
                    }
                    else if (theVisitDate > theCurrentDate)
                    {
                        IQCareMsgBox.Show("PharmacyDetailDispensedTDate", this);
                        txtpharmdispensedbydate.Focus();
                        return false;
                    }
                }
            }
            if ((txtpharmOrderedbyDate.Value.Trim() != "") && (txtpharmdispensedbydate.Value.Trim() != ""))
            {
                DateTime theOrdByDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmOrderedbyDate.Value.Trim()));
                DateTime theDispByDate = Convert.ToDateTime(theUtils.MakeDate(txtpharmdispensedbydate.Value.Trim()));
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
            DataSet dsExistNonP_less = PharmacyManager.GetExistPharmacyFormDespensedbydate(Convert.ToInt32(Session["PatientId"]), Convert.ToDateTime(theUtils.MakeDate(txtpharmdispensedbydate.Value)));
            if (dsExistNonP_less.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Session["PatientVisitId"]) != Convert.ToInt32(dsExistNonP_less.Tables[0].Rows[0][0]))
                {
                    IQCareMsgBox.Show("PharmacyDetailExists", this);
                    return false;
                }
            }



            return true;
        }
        protected void rgdrugmain_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            //foreach (GridDataItem mainitem in rgdrugmain.Items)
            //{
            //    RadGrid griddispense = (RadGrid)mainitem.FindControl("Dispense");
            //    DataTable dtDispense = (DataTable)ViewState["TableDispense"];
            //    foreach (GridDataItem item in griddispense.Items)
            //    {
            //        string drugID = mainitem.GetDataKeyValue("DrugID").ToString();
            //        RadNumericTextBox txtqtydispensed = (RadNumericTextBox)item.FindControl("txtQtyDispensed");
            //        RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribeddispense");
            //        for (int i = 0; i < dtDispense.Rows.Count; i++)
            //        {
            //            if (dtDispense.Rows[i]["DrugID"].ToString() == drugID)
            //            {

            //                if (txtqtydispensed.Text != "")
            //                {
            //                    dtDispense.Rows[i]["QtyDispensed"] = Convert.ToDecimal(txtqtydispensed.Text);
            //                }
            //                if (txtqtyprec.Text != "")
            //                {
            //                    dtDispense.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);
            //                }
            //            }

            //        }
            //    }
            //    ViewState["TableDispense"] = dtDispense;
            //    griddispense.Rebind();
            //    RadGrid gridorder = (RadGrid)mainitem.FindControl("order");
            //    DataTable dtupdateOrder = (DataTable)ViewState["TableOrder"];
            //    foreach (GridDataItem item in gridorder.Items)
            //    {
            //        string drugID = mainitem.GetDataKeyValue("DrugID").ToString();
            //        RadNumericTextBox txtdose = (RadNumericTextBox)item.FindControl("txtDose");
            //        RadComboBox ddlfrequency = (RadComboBox)item.FindControl("rdcmbfrequency");
            //        RadNumericTextBox txtduration = (RadNumericTextBox)item.FindControl("txtDuration");
            //        RadNumericTextBox txtqtyprec = (RadNumericTextBox)item.FindControl("txtQtyPrescribed");
            //        RadButton chkProphylaxis = (RadButton)item.FindControl("chkProphylaxis");
            //        for (int i = 0; i < dtupdateOrder.Rows.Count; i++)
            //        {
            //            if (dtupdateOrder.Rows[i]["DrugID"].ToString() == drugID)
            //            {
            //                if (txtdose.Text != "")
            //                {
            //                    dtupdateOrder.Rows[i]["Dose"] = Convert.ToDecimal(txtdose.Text);
            //                }
            //                if (ddlfrequency.SelectedItem.Value.ToString() != "")
            //                {
            //                    dtupdateOrder.Rows[i]["Frequency"] = Convert.ToInt32(ddlfrequency.SelectedItem.Value);
            //                }
            //                if (txtduration.Text != "")
            //                {
            //                    dtupdateOrder.Rows[i]["Duration"] = Convert.ToDecimal(txtduration.Text);
            //                }
            //                if (txtqtyprec.Text != "")
            //                {
            //                    dtupdateOrder.Rows[i]["QtyPrescribed"] = Convert.ToDecimal(txtqtyprec.Text);
            //                }
            //                dtupdateOrder.Rows[i]["Prophylaxis"] = CheckedValue(chkProphylaxis.SelectedToggleState.Text);

            //                dtupdateOrder.AcceptChanges();

            //            }
            //        }
            //    }
            //    ViewState["TableOrder"] = dtupdateOrder;
            //    gridorder.Rebind();
            //    RadGrid gridInstruction = (RadGrid)mainitem.FindControl("PatientInstruction");
            //    DataTable dtInstruction = (DataTable)ViewState["TableInstruct"];
            //    foreach (GridDataItem item in gridInstruction.Items)
            //    {
            //        string drugID = item.GetDataKeyValue("DrugID").ToString();
            //        RadTextBox txtInstruction = (RadTextBox)item.FindControl("txtPatientInstruction");
            //        foreach (DataRow theDR in dtupdateOrder.Rows)
            //        {
            //            if (theDR["DrugID"].ToString() == drugID)
            //            {

            //                if (txtInstruction.Text != "")
            //                {
            //                    theDR["Instructions"] = Convert.ToString(txtInstruction.Text);
            //                    theDR.EndEdit();
            //                    dtupdateOrder.AcceptChanges();
            //                }
            //            }

            //        }
            //    }
            //    ViewState["TableInstruct"] = dtInstruction;
            //    gridInstruction.Rebind();

            //    //-----------------Refill
            //    RadGrid gridRefill = (RadGrid)mainitem.FindControl("Refill") as RadGrid;
            //    DataTable dtRefill = (DataTable)ViewState["TableRefill"];
            //    foreach (GridDataItem item in gridRefill.Items)
            //    {
            //        string drugID = item.GetDataKeyValue("DrugID").ToString();

            //        RadNumericTextBox txtnooffile = (RadNumericTextBox)item.FindControl("txtRefill");
            //        RadDatePicker dtrefillexpiration = (RadDatePicker)item.FindControl("dtRefillExpiration");
            //        for (int i = 0; i < dtRefill.Rows.Count; i++)
            //        {
            //            if (dtRefill.Rows[i]["DrugID"].ToString() == drugID)
            //            {

            //                if (txtnooffile.Text != "")
            //                {
            //                    dtRefill.Rows[i]["Refill"] = Convert.ToInt32(txtnooffile.Text);
            //                }
            //                if (dtrefillexpiration.SelectedDate != null)
            //                {
            //                    if (dtrefillexpiration.SelectedDate.Value.ToString() != "")
            //                    {
            //                        dtRefill.Rows[i]["RefillExpiration"] = dtrefillexpiration.SelectedDate.Value.ToString();
            //                    }
            //                }
            //            }

            //        }
            //    }
            //    ViewState["TableRefill"] = dtRefill;
            //    gridRefill.Rebind();
            //}
            //foreach (GridNestedViewItem item in rgdrugmain.MasterTableView.GetItems(GridItemType.NestedView))
            //{
            //    RadTabStrip TabStrip = (RadTabStrip)item.FindControl("TabStip1");
            //    if (Session["Orderid"].ToString() == "0")
            //    {
            //        TabStrip.Tabs[2].Enabled = false;
            //        TabStrip.Tabs[0].Focus();
            //        RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
            //        radpage.SelectedIndex = 0;
            //    }
            //    if (ViewState["RefillClick"].ToString() == "True")
            //    {
            //        TabStrip.Tabs[1].Enabled = false;
            //        RadMultiPage radpage = (RadMultiPage)item.FindControl("Multipage1");
            //        radpage.SelectedIndex = 0;
            //    }
            //}
            //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
            //BindDrugGrid();

        }
        protected void btncancel_Click(object sender, EventArgs e)
        {
            string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
            script += "var ans;\n";
            script += "ans=window.confirm('Want to close Pharmacy Form?');\n";
            script += "if (ans==true)\n";
            script += "{\n";
            script += "window.location.href='../ClinicalForms/frmPatient_Home.aspx?sts=" + 0 + "';\n";
            script += "}\n";
            script += "else \n";
            script += "{\n";
            script += "window.location.href='frmPharmacy_Custom.aspx';\n";
            script += "}\n";
            script += "</script>\n";
            RegisterStartupScript("confirm", script);
        }

        private String ValidationMessage()
        {
            IIQCareSystem IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            DateTime theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
            string strmsg = "Following values are required to complete this:\\n\\n";
            DataTable theDT = (DataTable)ViewState["BusRule"];
            String Radio1 = "", Radio2 = "", MultiSelectName = "", MultiSelectLabel = "";
            int TotCount = 0, FalseCount = 0;
            try
            {
                foreach (Control x in PnlDynamicElements.Controls)
                {
                    if (x.GetType() == typeof(System.Web.UI.WebControls.TextBox))
                    {
                        string[] Field = ((TextBox)x).ID.Split('-');
                        foreach (DataRow theDR in theDT.Rows)
                        {
                            if ((((TextBox)x).ID.Contains("=") == true) && (((TextBox)x).Enabled == true))
                            {
                                string[] Field10 = ((TextBox)x).ID.Replace('=', '-').Split('-');
                                if (Field10[1] == Convert.ToString(theDR["FieldName"]) && Field10[2].ToLower() == Convert.ToString(theDR["TableName"].ToString().ToLower()) && Field10[3] == Convert.ToString(theDR["FieldId"]) && Convert.ToString(theDR["BusRuleId"]) == "1")
                                {
                                    if (((TextBox)x).Text == "")
                                    {
                                        strmsg += theDR["FieldLabel"] + " is " + theDR["Name"];
                                        strmsg = strmsg + "\\n";
                                    }
                                }

                            }

                            if (Field[1] == Convert.ToString(theDR["FieldName"]) && Field[2].ToLower() == Convert.ToString(theDR["TableName"].ToString().ToLower()) && Field[3] == Convert.ToString(theDR["FieldId"]) && Convert.ToString(theDR["BusRuleId"]) == "1")
                            {
                                if ((((TextBox)x).Text == "") && (((TextBox)x).Enabled == true))
                                {
                                    strmsg += theDR["FieldLabel"] + " is " + theDR["Name"];
                                    strmsg = strmsg + "\\n";
                                }
                            }
                            //Date Greater than Today's Date
                            if (Field[1] == Convert.ToString(theDR["FieldName"]) && Field[2].ToLower() == Convert.ToString(theDR["TableName"].ToString().ToLower()) && Field[3] == Convert.ToString(theDR["FieldId"]) && Convert.ToString(theDR["BusRuleId"]) == "7")
                            {
                                if (((TextBox)x).Text != "")
                                {
                                    DateTime GetDate = Convert.ToDateTime(((TextBox)x).Text);
                                    if (GetDate <= theCurrentDate)
                                    {
                                        strmsg += theDR["Name"] + " for " + theDR["FieldLabel"];
                                        strmsg = strmsg + "\\n";
                                    }
                                }
                            }
                            //Date Less than Today's Date
                            if (Field[1] == Convert.ToString(theDR["FieldName"]) && Field[2].ToLower() == Convert.ToString(theDR["TableName"].ToString().ToLower()) && Field[3] == Convert.ToString(theDR["FieldId"]) && Convert.ToString(theDR["BusRuleId"]) == "8")
                            {
                                if (((TextBox)x).Text != "")
                                {
                                    DateTime GetDate = Convert.ToDateTime(((TextBox)x).Text);

                                    if (GetDate >= theCurrentDate)
                                    {
                                        strmsg += theDR["Name"] + " for " + theDR["FieldLabel"];
                                        strmsg = strmsg + "\\n";
                                    }
                                }
                            }
                        }
                    }
                    if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputRadioButton))
                    {
                        string[] Field = ((HtmlInputRadioButton)x).ID.Split('-');
                        if (Field[0] == "RADIO1" && ((HtmlInputRadioButton)x).Checked == false)
                        {
                            Radio1 = Field[3];
                        }
                        if (Field[0] == "RADIO2" && ((HtmlInputRadioButton)x).Checked == false)
                        {
                            Radio2 = Field[3];
                        }

                        foreach (DataRow theDR in theDT.Rows)
                        {

                            if (Radio1 == Field[3] && Radio2 == Field[3])
                            {
                                if (Field[1] == Convert.ToString(theDR["FieldName"]) && Field[2].ToLower() == Convert.ToString(theDR["TableName"].ToString().ToLower()) && Field[3] == Convert.ToString(theDR["FieldId"]) && Convert.ToString(theDR["BusRuleId"]) == "1")
                                {
                                    strmsg += theDR["FieldLabel"] + " is " + theDR["Name"];
                                    strmsg = strmsg + "\\n";
                                }

                            }

                        }
                    }
                    if (x.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                    {
                        string[] Field = ((DropDownList)x).ID.Split('-');
                        foreach (DataRow theDR in theDT.Rows)
                        {
                            if (Field[1] == Convert.ToString(theDR["FieldName"]) && Field[2].ToLower() == Convert.ToString(theDR["TableName"].ToString().ToLower()) && Field[3] == Convert.ToString(theDR["FieldId"]) && Convert.ToString(theDR["BusRuleId"]) == "1" && Field[0].ToString() != "SELECTLISTAuto")
                            {
                                if (((DropDownList)x).SelectedValue == "0")
                                {
                                    strmsg += theDR["FieldLabel"] + " is " + theDR["Name"];
                                    strmsg = strmsg + "\\n";
                                }
                            }
                        }
                    }

                    if (x.GetType() == typeof(System.Web.UI.HtmlControls.HtmlInputCheckBox))
                    {
                        string[] Field = ((HtmlInputCheckBox)x).ID.Split('-');
                        foreach (DataRow theDR in theDT.Rows)
                        {
                            if (Field[1] == Convert.ToString(theDR["FieldName"]) && Field[2].ToLower() == Convert.ToString(theDR["TableName"].ToString().ToLower()) && Field[3] == Convert.ToString(theDR["FieldId"]) && Convert.ToString(theDR["BusRuleId"]) == "1")
                            {
                                if (((HtmlInputCheckBox)x).Checked == false)
                                {
                                    strmsg += theDR["FieldLabel"] + " is " + theDR["Name"];
                                    strmsg = strmsg + "\\n";
                                }
                            }
                        }

                    }

                }

                //MultiSelect Validation

                foreach (Control y in PnlDynamicElements.Controls)
                {
                    if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                    {
                        foreach (Control z in y.Controls)
                        {

                            if (z.GetType() == typeof(System.Web.UI.WebControls.CheckBox))
                            {
                                TotCount++;
                                if (((CheckBox)z).Checked == false)
                                {
                                    FalseCount++;

                                }
                            }
                        }
                        foreach (DataRow theDR in theDT.Rows)
                        {
                            if (Convert.ToString(theDR["ControlId"]) == "9" && ((Panel)y).ID.Substring(4, (((Panel)y).ID.Length - 4)) == Convert.ToString(theDR["FieldID"]) && Convert.ToInt32(theDR["BusRuleId"]) < 13)
                            {
                                MultiSelectName = Convert.ToString(theDR["Name"]);
                                MultiSelectLabel = Convert.ToString(theDR["FieldLabel"]);
                                if (TotCount == FalseCount)
                                {
                                    strmsg += MultiSelectLabel + " is " + MultiSelectName;
                                    strmsg = strmsg + "\\n";
                                }
                            }
                        }

                        TotCount = 0; FalseCount = 0;
                        MultiSelectName = ""; MultiSelectLabel = "";
                    }
                }

            }

            catch (Exception err)
            {

                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
            }
            finally { }

            return strmsg;
        }
    }
}
