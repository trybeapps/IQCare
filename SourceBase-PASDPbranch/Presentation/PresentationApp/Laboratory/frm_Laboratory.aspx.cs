using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Interface.Laboratory;
using Application.Common;
using System.Collections.Generic;
using System.Text;
using Application.Presentation;
using Interface.Administration;
using AjaxControlToolkit;
using System.Web.Script.Serialization;
using Interface.Security;
using Interface.Clinical;

//Third party Libs
using Telerik.Web.UI;
using System.Data;
using System.IO;

namespace PresentationApp.Laboratory
{
    public partial class frm_Laboratory : System.Web.UI.Page
    {
        static DataTable dtLabResult;
        static DataTable dtLabTest;
        DataView theDV = new DataView();
        DataTable DTtestDate = new DataTable();
        IIQCareSystem IQCareSecurity;
        DateTime theVisitDate, theCurrentDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(frm_Laboratory));
            txtlaborderedbydate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
            txtlaborderedbydate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");

            txtLabtobeDone.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
            txtLabtobeDone.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
            BindAutoSelectLabTest("a");
            if (!IsPostBack)
            {
                BindMasterData();
                LoadBlankGrid();
                BindControls();
                
            }

        }
        private void BindControls()
        {
            BindFunctions theBindManager = new BindFunctions();
            IQCareUtils theUtils = new IQCareUtils();
            DataSet theDSXML = new DataSet();
            DataTable theDT = new DataTable();
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

                    theBindManager.BindCombo(ddlaborderedbyname, theDT, "EmployeeName", "EmployeeId");
                    theDV.Dispose();
                    theDT.Clear();
                }

            }
            if (Convert.ToInt32(Session["Lab_ID"]) > 0)
            {

                BindDropdownOrderBy(DTtestDate.Rows[0]["OrderedbyName"].ToString());
                ddlaborderedbyname.SelectedValue = DTtestDate.Rows[0]["OrderedbyName"].ToString();
                txtlaborderedbydate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(DTtestDate.Rows[0]["OrderedbyDate"]));
                if (DTtestDate.Rows[0].IsNull("PreClinicLabDate") == false)
                    if (((DateTime)DTtestDate.Rows[0]["PreClinicLabDate"]).ToString(Session["AppDateFormat"].ToString()) != "01-Jan-1900")
                        this.txtLabtobeDone.Text = (((DateTime)DTtestDate.Rows[0]["PreClinicLabDate"]).ToString(Session["AppDateFormat"].ToString())).ToString();

                if (this.txtLabtobeDone.Text != "")
                {
                    this.preclinicLabs.Checked = true;
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
                    BindManager.BindCombo(ddlaborderedbyname, theDT, "EmployeeName", "EmployeeId");

                }
            }

        }
        protected void BtnAddDrugClick(object sender, EventArgs e)
        {
            string labIdstr = SelectedLabTest();
            if (hdnTestAddTestID.Value != "")
            {
                labIdstr = labIdstr + ',' + hdnTestAddTestID.Value;
            }

            hiddTestID.Value = "";

            if (labIdstr != "")
            {
                DataTable dt = GetDataTable("LabSubTestID", labIdstr, "");
                RadGridLabTest.DataSource = dt;
                RadGridLabTest.DataBind();
                hiddTestID.Value = RadGridLabTest.Items.Count.ToString();
                ViewState["tblLabtestID"] = dt;
                hdnTestAddTestID.Value = labIdstr;

            }

        }

        protected string SelectedLabTest()
        {
            var collection = rcbPreSelectedLabTest.CheckedItems;
            string labIdstr = "";
            string commastr = "";

            if (collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    labIdstr = labIdstr + commastr + item.Value;
                    commastr = ",";

                }


            }
            return labIdstr;


        }

        protected void Autoselectdrug_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {
            string labIdstr = SelectedLabTest();
            //if (labIdstr == "")
            //{
            //    labIdstr =  AutoselectLabTest.Entries[0].Value;
            //}
            //else
            //{
            //    labIdstr = labIdstr + "," + AutoselectLabTest.Entries[0].Value;
            //}

            if (hiddTestAddTestID.Value == "")
            {
                hiddTestAddTestID.Value = AutoselectLabTest.Entries[0].Value;
            }
            else
            {
                hiddTestAddTestID.Value = hiddTestAddTestID.Value + "," + AutoselectLabTest.Entries[0].Value;
            }

            if (labIdstr != "")
            {
                labIdstr = labIdstr + "," + hiddTestAddTestID.Value;
            }
            else
            {
                labIdstr = hiddTestAddTestID.Value;
            }


            //labIdstr = labIdstr+","+hiddTestAddTestID.Value;
            //if (labIdstr.IndexOf(",") == 0)
            //{
            //    labIdstr = labIdstr.Substring(1);
            //}
            //hiddTestID.Value = "";
            if (labIdstr != "")
            {
                DataTable dt = GetDataTable("LabSubTestID", labIdstr, "");
                RadGridLabTest.DataSource = dt;
                RadGridLabTest.DataBind();
                hiddTestID.Value = RadGridLabTest.Items.Count.ToString();
                labIdstr = labIdstr + "," + labIdstr;
                ViewState["tblLabtestID"] = dt;
                hdnTestAddTestID.Value = labIdstr;

            }
            AutoselectLabTest.Entries.Clear();

        }
        protected DataTable GetDataTable(string flag, string labIds, string labName)
        {
            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            objLabFields.Flag = flag;
            objLabFields.LabTestIDs = labIds;
            objLabFields.LabTestName = labName;
            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataSet Ds = theILabManager.IQTouchGetPatientLabTestID(objLabFields);
            DataTable dt = Ds.Tables[0];
            return dt;
        }
        protected void BindMasterData()
        {
            //Lab test id
            DataTable dtLabTest = GetDataTable("LabTestID", "", "");
            rcbPreSelectedLabTest.DataTextField = "LabName";
            rcbPreSelectedLabTest.DataValueField = "LabTestID";
            rcbPreSelectedLabTest.DataSource = dtLabTest;
            rcbPreSelectedLabTest.DataBind();

        }
        protected void BindAutoSelectLabTest(string inputval)
        {
            DataTable dtLabTest = GetDataTable("LabTestID", "", inputval);
            AutoselectLabTest.DataTextField = "LabName";
            AutoselectLabTest.DataValueField = "LabTestID";
            AutoselectLabTest.DataSource = dtLabTest;
            AutoselectLabTest.DataBind();
        }
        protected void LoadBlankGrid()
        {
            if (RadGridLabTest.Items.Count == 0)
            {
                RadGridLabTest.DataSource = new Object[0];
            }

        }
        protected void RadGridLabTest_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridDataItem dataItm = e.Item as GridDataItem;
            RadGrid radGridArvMutation = (RadGrid)sender;

            Label lblLabSubTestID = (Label)dataItm.FindControl("lblLabSubTestID");
            string id = lblLabSubTestID.Text;
            DataTable table = (DataTable)ViewState["tblLabtestID"];
            table.PrimaryKey = new DataColumn[] { table.Columns["SubTestId"] };

            if (table.Rows.Find(id) != null)
            {
                DataRow dr = table.Rows.Find(id);

                dr.Delete();
                table.AcceptChanges();

                ViewState["tblLabtestID"] = table;
                radGridArvMutation.DataSource = table;
                radGridArvMutation.DataBind();
            }
        }

        protected void RadGridLabTest_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void RadGridLabTest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                string labIdstr = SelectedLabTest();
                hiddTestID.Value = "";
                if (labIdstr != "")
                {
                    DataTable dt = GetDataTable("LabSubTestID", labIdstr, "");
                    RadGridLabTest.DataSource = dt;
                    hiddTestID.Value = RadGridLabTest.Items.Count.ToString();
                    ViewState["tblLabtestID"] = dt;
                }
            }
            catch (Exception ex)
            {
                //lblerr.Text = ex.Message.ToString();


            }
        }

        protected void RadGridLabTest_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void txtLabtobeDone_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnclose_Click(object sender, EventArgs e)
        {
            Page.RegisterClientScriptBlock("onclick", "<script language='javascript' type='text/javascript'>window.close();</script>");
        }
    }
}