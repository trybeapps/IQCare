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
            if (Session["AppLocation"] == null || Session.Count == 0 || Session["AppUserID"].ToString() == "")
            {
                IQCareMsgBox.Show("SessionExpired", this);
                Response.Redirect("~/frmlogin.aspx",true);
            }
            if (Request.QueryString["opento"] == "ArtForm")
            {
                Session["PatientVisitId"] = 0;
                Session["LabOrderID"] = null;
            }

            Ajax.Utility.RegisterTypeForAjax(typeof(frm_Laboratory));
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Text = "Clinical Forms >> ";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "Laboratory";
            (Master.FindControl("levelTwoNavigationUserControl1").FindControl("lblformname") as Label).Text = "Laboratory";
            txtlaborderedbydate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
            txtlaborderedbydate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
            txtrepordtedbydate.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
            txtrepordtedbydate.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
            txtLabtobeDone.Attributes.Add("onkeyup", "DateFormat(this,this.value,event,false,'3')");
            txtLabtobeDone.Attributes.Add("onblur", "DateFormat(this,this.value,event,true,'3')");
            PutCustomControl();
            //BindAutoSelectLabTest("a");
            if (!IsPostBack)
            {
                if (Session["LabOrderID"] != null)
                {
                    DataTable dt = GetDataTable("LAB_STATUS", "", Convert.ToInt32(Session["LabOrderID"].ToString()));
                    Session["LabOrderStatus"] = dt.Rows[0]["LabStatus"].ToString();

                }
                else
                {
                    showhide();
                    Session["LabOrderStatus"] = "";
                }

                BindMasterData();
                LoadBlankGrid();
                BindControls();
                BindLabTestGrid();
                BindLabOrderDetails();
                
            }
            
            if (Request.QueryString["name"] == "Delete")
            {
                btnsave.Text = "Delete";
            }
            int PID = Convert.ToInt32(Session["PatientId"]);
            
            FillOldCustomData(PID);

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LabOrderID"] != null)
                {
                    if (Convert.ToInt32(Session["LabOrderID"].ToString()) > 0)
                    {

                        foreach (GridDataItem griditem in RadGridLabTest.Items)
                        {
                            CommandEventArgs arg = new CommandEventArgs("ExpandCollapse", griditem);
                            GridCommandEventArgs evarg = new GridCommandEventArgs(griditem, null, arg);
                            RadGridLabTest_ItemCommand(sender, evarg);
                            griditem.Expanded = true;
                        }
                    }
                }
            }

        }
        private void showhide()
        {
            //if ((Session["LabOrderStatus"].ToString() == "") || (Session["LabOrderStatus"].ToString() == "Not Specified"))
            //{
                trReported.Visible = false;
           // }

        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static AutoCompleteBoxData GetLabsNames(object context)
        {
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();            
            DataTable data = GetDataTable("LabTestID", "", searchString);
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

            foreach (DataRow row in data.Rows)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                childNode.Text = row["LabName"].ToString();
                childNode.Value = row["LabTestID"].ToString();
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();

            return res;
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
                    theBindManager.BindCombo(ddlreportedby, theDT, "EmployeeName", "EmployeeId");
                    theDV.Dispose();
                    theDT.Clear();
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
        private void BindDropdownReportedBy(String EmployeeId)
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
            
            var collectionnew = rcbPreSelectedLabTest.CheckedItems;
            string labIdstr = "";
            string commastr = "";
            if (Session["LabOrderID"] != null)
            {
                DataTable dt = GetDataTable("LabSubTestID", "", Convert.ToInt32(Session["LabOrderID"].ToString()));
                var collection = dt.Rows;
                if (collection.Count > 0)
                {
                    foreach (var item in collection)
                    {
                        labIdstr = labIdstr + commastr + ((System.Data.DataRow)(item)).ItemArray[2];
                        commastr = ",";
                    }
                }
            }
            if (collectionnew.Count > 0)
            {
                foreach (var item in collectionnew)
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
        protected static DataTable GetDataTable(string flag, string labIds, string labName)
        {
            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            objLabFields.Flag = flag;
            objLabFields.LabTestIDs = labIds;
            objLabFields.LabTestName = labName;
            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataSet Ds = theILabManager.GetPatientLabTestID(objLabFields);
            DataTable dt = Ds.Tables[0];
            return dt;
        }
        protected void BindLabTestGrid()
        {
            if (Session["LabOrderID"] != null)
            {
                DataTable dt = GetDataTable("LabSubTestID", "", Convert.ToInt32(Session["LabOrderID"].ToString()));
                ViewState["tableLabSubTestID"] = dt;
                RadGridLabTest.DataSource = dt;
                RadGridLabTest.DataBind();
            }
        }
        protected void BindLabOrderDetails()
        {
            if (Session["LabOrderID"] != null)
            {
                DataTable dt = GetDataTable("LabOrderDetails", "", Convert.ToInt32(Session["LabOrderID"].ToString()));

                if (dt.Rows.Count > 0)
                {
                    ddlaborderedbyname.SelectedValue = dt.Rows[0]["OrderedbyName"].ToString();
                    if (dt.Rows[0]["OrderedbyDate"] != DBNull.Value)
                    {
                        txtlaborderedbydate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dt.Rows[0]["OrderedbyDate"]));
                        //ViewState["VisitDate"] = dt.Rows[0]["OrderedbyDate"].ToString();
                    }
                    if (dt.Rows[0]["PreClinicLabDate"] != DBNull.Value)
                        txtLabtobeDone.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dt.Rows[0]["PreClinicLabDate"]));
                    ddlreportedby.SelectedValue = dt.Rows[0]["ReportedbyName"].ToString();
                    if (dt.Rows[0]["ReportedbyDate"] != DBNull.Value)
                    {
                        txtrepordtedbydate.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(dt.Rows[0]["ReportedbyDate"]));                        
                    }
                }

                //btnSave.Enabled = true;
                if (Session["LabOrderStatus"].ToString() == "Completed" && Request.QueryString["name"] != "Delete")
                {
                    btnsave.Enabled = false;
                }
            }

        }
        private Boolean FieldValidation()
        {

            IQCareSecurity = (IIQCareSystem)ObjectFactory.CreateInstance("BusinessProcess.Security.BIQCareSystem, BusinessProcess.Security");
            theCurrentDate = (DateTime)IQCareSecurity.SystemDate();
            IQCareUtils theUtils = new IQCareUtils();
            if (ddlaborderedbyname.SelectedIndex == 0)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["Control"] = "Lab OrderedBy Name";
                IQCareMsgBox.ShowforUpdatePanel("BlankTextBox", theBuilder, this);
                ddlaborderedbyname.Focus();
                return false;
            }
            if (txtlaborderedbydate.Text.ToString() == "")
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["Control"] = "Lab OrderedBy Date";
                IQCareMsgBox.ShowforUpdatePanel("BlankTextBox", theBuilder, this);
                txtlaborderedbydate.Focus();
                return false;
            }
            //else if (txtlaborderedbydate.Text.ToString() != "")
            //{
            //    theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtlaborderedbydate.Text));

            //        //DateTime theIEVisitDate = Convert.ToDateTime(ViewState["IEVisitDate"].ToString());
            //        DateTime theVisitDate1 = Convert.ToDateTime(ViewState["VisitDate"].ToString());
            //        if (theVisitDate1 > theVisitDate)
            //        {
            //            #region  "21-Jun-07 - 1"
            //            IQCareMsgBox.ShowforUpdatePanel("CompareLabVisitDate", this);
            //            txtlaborderedbydate.Focus();
            //            return false;
            //            #endregion
            //        }
            //        else if (theVisitDate > theCurrentDate)
            //        {
            //            IQCareMsgBox.ShowforUpdatePanel("NonARTVisitDate", this);
            //            txtlaborderedbydate.Focus();
            //            return false;
            //        }
                
            //}
            
            //if (Session["Paperless"].ToString() == "0")
            //{
            //    if (txtlabReportedbyDate.Text.ToString() == "")
            //    {
            //        MsgBuilder theBuilder = new MsgBuilder();
            //        theBuilder.DataElements["Control"] = "Lab Reported By Date";
            //        IQCareMsgBox.Show("BlankTextBox", theBuilder, this);
            //        txtlabReportedbyDate.Focus();
            //        return false;
            //    }
            //    else if (txtlabReportedbyDate.Text.ToString() != "")
            //    {
            //        theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtlabReportedbyDate.Text));
            //        if (Convert.ToDateTime(theUtils.MakeDate(txtlabReportedbyDate.Text)) < theVisitDate)
            //        {
            //            IQCareMsgBox.Show("LabReportedDate", this);
            //            txtlabReportedbyDate.Focus();
            //            return false;
            //        }
            //    }
            //}
            if ((Session["LabOrderStatus"].ToString() == "")  || (Session["LabOrderStatus"].ToString() == "Not Specified"))
            {

            }
            else
            {
                if (ddlreportedby.SelectedIndex == 0)
                {
                    MsgBuilder theBuilder = new MsgBuilder();
                    theBuilder.DataElements["Control"] = "Reported by Name";
                    IQCareMsgBox.ShowforUpdatePanel("BlankTextBox", theBuilder, this);
                    ddlreportedby.Focus();
                    return false;
                }
                if (txtrepordtedbydate.Text.ToString() == "")
                {
                    MsgBuilder theBuilder = new MsgBuilder();
                    theBuilder.DataElements["Control"] = "Lab Reported By Date";
                    IQCareMsgBox.ShowforUpdatePanel("BlankTextBox", theBuilder, this);
                    txtrepordtedbydate.Focus();
                    return false;
                }
                //else if (txtrepordtedbydate.Text.ToString() != "")
                //{
                //    theVisitDate = Convert.ToDateTime(theUtils.MakeDate(txtlabReportedbyDate.Text));
                //    if (Convert.ToDateTime(theUtils.MakeDate(txtlabReportedbyDate.Text)) < theVisitDate)
                //    {
                //        IQCareMsgBox.Show("LabReportedDate", this);
                //        txtlabReportedbyDate.Focus();
                //        return false;
                //    }
                //}
            }

            if ((txtlaborderedbydate.Text.Trim() != "") && (txtrepordtedbydate.Text.Trim() != ""))
            {
                DateTime theOrdByDate = Convert.ToDateTime(theUtils.MakeDate(txtlaborderedbydate.Text.Trim()));
                DateTime theDispByDate = Convert.ToDateTime(theUtils.MakeDate(txtrepordtedbydate.Text.Trim()));
                if (theOrdByDate > theDispByDate)
                {
                    IQCareMsgBox.ShowforUpdatePanel("LabOrderReportedByDate", this);
                    txtlaborderedbydate.Focus();
                    return false;
                }
            }
            

            return true;
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
        private bool gridhasvalues(RadGrid radGridLabResult)
        {
            if (Session["LabOrderID"] != null)
            {
                DataTable dtGenXpert = new DataTable();                
                //For GenXpert
                List<BIQTouchLabFields> listArv = new List<BIQTouchLabFields>();                
                dtGenXpert = CreateDtGenXpertTable();

                foreach (GridDataItem item in radGridLabResult.Items)
                {
                    Label lblLabSubTestId = (Label)item.FindControl("lblLabSubTestId");
                    Label lblLabSubTestName = (Label)item.FindControl("lblLabTestName");

                    Label lblControlType = (Label)item.FindControl("lblControlType");
                    RadioButtonList btnradRadioButtonList = (RadioButtonList)item.FindControl("btnRadRadiolist");
                    Telerik.Web.UI.RadNumericTextBox txtRadValue = (Telerik.Web.UI.RadNumericTextBox)item.FindControl("txtRadValue");
                    Telerik.Web.UI.RadTextBox txtAlphaRadValue = (Telerik.Web.UI.RadTextBox)item.FindControl("txtAlphaRadValue");
                    CheckBoxList chkBoxList = (CheckBoxList)item.FindControl("chkBoxList");
                    DropDownList ddlList = (DropDownList)item.FindControl("ddlList");

                    string strResuts = "0";

                    if (lblControlType.Text == "Radio")
                    {
                        if (btnradRadioButtonList.SelectedValue != "")
                        {
                            return true;
                        }
                    }
                    else if (lblControlType.Text == "Combo Box")
                    {

                        if (ddlList.SelectedValue.ToString() != "")
                        {
                            return true;
                        }

                    }
                    else if (lblControlType.Text == "Check box")
                    {

                        if (chkBoxList.SelectedValue.ToString() != "")
                        {
                            return true;
                        }
                    }
                    else if (lblControlType.Text == "GridView")
                    {
                        if (lblLabSubTestName.Text.ToUpper().Equals("ARV MUTATIONS"))
                        {
                            listArv = ArvMutationData(lblLabSubTestId.Text.ToString());
                            if (listArv.Count > 0)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            DataRow dR = dtGenXpert.NewRow();
                            RadGrid radSubGridItems = (RadGrid)item.FindControl("RadGridArvMutation");
                            GridFooterItem radSubFooterItems = (GridFooterItem)radSubGridItems.MasterTableView.GetItems(GridItemType.Footer)[0];
                            Telerik.Web.UI.RadComboBox rcbFooterArvType = (Telerik.Web.UI.RadComboBox)radSubFooterItems.FindControl("rcbFooterArvType");
                            Telerik.Web.UI.RadComboBox rcbFooterMutation = (Telerik.Web.UI.RadComboBox)radSubFooterItems.FindControl("rcbFooterMutation");
                            Telerik.Web.UI.RadComboBox rcbFooterCulture = (Telerik.Web.UI.RadComboBox)radSubFooterItems.FindControl("rcbFooterCulture");
                            dR["LabId"] = Convert.ToInt32(Session["LabOrderID"].ToString());
                            if (rcbFooterArvType.SelectedValue != "")
                            {
                                dR["ABFID"] = Convert.ToInt32(rcbFooterArvType.SelectedValue);
                                dR["ABFText"] = rcbFooterArvType.SelectedItem.Text;
                            }
                            if (rcbFooterMutation.SelectedValue != "")
                            {
                                dR["GeneXpertID"] = Convert.ToInt32(rcbFooterMutation.SelectedValue);
                                dR["GeneXpertText"] = rcbFooterMutation.SelectedItem.Text;
                            }
                            if (rcbFooterCulture.SelectedValue != "")
                            {
                                dR["CultSens"] = Convert.ToInt32(rcbFooterCulture.SelectedValue);
                                dR["CultSensText"] = rcbFooterCulture.SelectedItem.Text;
                            }
                            dR["ParameterID"] = Convert.ToInt32(lblLabSubTestId.Text);
                            dtGenXpert.Rows.Add(dR);
                            return true;
                        }
                    }
                    else
                    {

                        if (txtRadValue.Text != "0" && txtRadValue.Text != "")
                        {
                            strResuts = txtRadValue.Text.ToString();
                            return true;
                        }
                        else if (txtAlphaRadValue.Text != "")
                        {
                            strResuts = txtAlphaRadValue.Text.ToString();
                            return true;
                        }
                    }

                }
                
            }
            return false;

        }
        protected void RadGridLabTest_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            {
                if (Session["LabOrderStatus"].ToString() != "")
                {
                    RadGrid detsGrid = (RadGrid)((GridDataItem)e.Item).ChildItem.FindControl("RadGridLabResult");
                    detsGrid.Visible = !e.Item.Expanded;

                    if (!e.Item.Expanded)
                    {                       
                        if ((Convert.ToInt32(Session["LabOrderID"].ToString()) > 0) && (gridhasvalues(detsGrid) == false))
                            detsGrid.Rebind();
                    }
                }

            }

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
            if (e.Item is GridNestedViewItem)
            {
                e.Item.FindControl("RadGridLabResult").Visible = ((GridNestedViewItem)e.Item).ParentItem.Expanded;
                (e.Item.FindControl("RadGridLabResult") as RadGrid).NeedDataSource += new GridNeedDataSourceEventHandler(RadGridLabResult_NeedDataSource);


                RadGrid radGridLabResult = (RadGrid)e.Item.FindControl("RadGridLabResult");
                radGridLabResult.ItemCreated += new GridItemEventHandler(radGridLabResult_ItemCreated);
                radGridLabResult.ItemDataBound += new GridItemEventHandler(RadGridResut_ItemDataBound);


                //radGridLabResult.DataBind();

            }
        }
        protected void btnclose_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["opento"] == "ArtForm")
            {
                if (Convert.ToInt32(Session["ArtEncounterPatientVisitId"]) > 0)
                {
                    Session["PatientVisitId"] = Session["ArtEncounterPatientVisitId"];
                }
                Page.RegisterClientScriptBlock("onclick", "<script language='javascript' type='text/javascript'>Close();</script>");

            }
            else
            {
                string theUrl = string.Format("{0}", "../ClinicalForms/frmPatient_Home.aspx");                
                Response.Redirect(theUrl);

            }
            
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
        private void SaveCancel()
        {
            int PatientID = Convert.ToInt32(Session["PatientId"]);

            //string script = "<script language = 'javascript' defer ='defer' id = 'confirm'>\n";
            string script = "var ans;\n";
            script += "ans=window.confirm('Laboratory Form saved successfully. Do you want to close?');\n";
            script += "if (ans==true)\n";
            script += "{\n";
            
            script += "window.location.href='../ClinicalForms/frmPatient_History.aspx';\n";
           
            script += "}\n";
            script += "else \n";
            script += "{\n";
            //script += "window.location.href('frmClinical_ARTFollowup.aspx?name=Edit&PatientId=" + PatientID + "&visitid=" + visitPK + "&LocationId=" + locationid + "&sts=" + Request.QueryString["sts"].ToString() + "');\n";
            script += "window.location.href='frm_Laboratory.aspx'\n";
            script += "}\n";
            //script += "</script>\n";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "confirm", script, true);
            //RegisterStartupScript("confirm", script);
        }
        protected void SaveLabOrder()
        {
            //Default code for User Control Load 
            DataTable dtGenXpert = new DataTable();

            Session["IsFirstLoad"] = "true";
            List<BIQTouchLabFields> list = new List<BIQTouchLabFields>();
            List<BIQTouchLabFields> listArv = new List<BIQTouchLabFields>();

            try
            {
                // Validation Function Code to be write here

                // Asigning value for saving
                IQCareUtils theUtils = new IQCareUtils();
                BIQTouchLabFields objLabFields = new BIQTouchLabFields();
                objLabFields.Ptnpk = Convert.ToInt32(Session["PatientID"]);
                objLabFields.LocationId = Convert.ToInt32(Session["AppLocationId"].ToString());
                objLabFields.UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                objLabFields.OrderedByName = Convert.ToInt32(ddlaborderedbyname.SelectedValue.ToString());
                objLabFields.OrderedByDate = Convert.ToDateTime(theUtils.MakeDate(txtlaborderedbydate.Text));
                
                objLabFields.LabTestID = 0;
                objLabFields.ReportedByDate = DateGiven(txtrepordtedbydate.Text);               
                objLabFields.ReportedByName = Convert.ToInt32(ddlreportedby.SelectedValue);
                objLabFields.TestResults = "";
                objLabFields.TestResultId = 0;
                objLabFields.LabOrderId = 0;
                objLabFields.SystemId = Convert.ToInt32(Session["SystemId"]);
                if (txtLabtobeDone.Text != "")
                {
                    objLabFields.PreClinicLabDate = Convert.ToDateTime(theUtils.MakeDate(txtLabtobeDone.Text));
                }
                else
                {
                    objLabFields.PreClinicLabDate = DateGiven("");
                }
                if(Session["LabOrderID"] !=null)
                {
                    objLabFields.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());
                    objLabFields.IntFlag = 2;
                    //For GenXpert
                    
                    if (ViewState["TblGenXpert"] == null)
                    {
                        dtGenXpert = CreateDtGenXpertTable();
                    }
                    
                    foreach (GridNestedViewItem nestedView in RadGridLabTest.MasterTableView.GetItems(GridItemType.NestedView))
                    {
                        RadGrid radGridLabResult = (RadGrid)nestedView.FindControl("RadGridLabResult");

                        foreach (GridDataItem item in radGridLabResult.Items)
                        {
                            Boolean entryFlag = false;
                            Label lblLabSubTestId = (Label)item.FindControl("lblLabSubTestId");
                            Label lblLabSubTestName = (Label)item.FindControl("lblLabTestName");
                            //Label lblLabTestID = (Label)item.FindControl("lblLabTestID");

                            Label lblControlType = (Label)item.FindControl("lblControlType");
                            RadioButtonList btnradRadioButtonList = (RadioButtonList)item.FindControl("btnRadRadiolist");
                            Telerik.Web.UI.RadNumericTextBox txtRadValue = (Telerik.Web.UI.RadNumericTextBox)item.FindControl("txtRadValue");
                            Telerik.Web.UI.RadTextBox txtAlphaRadValue = (Telerik.Web.UI.RadTextBox)item.FindControl("txtAlphaRadValue");
                            CheckBoxList chkBoxList = (CheckBoxList)item.FindControl("chkBoxList");
                            DropDownList ddlList = (DropDownList)item.FindControl("ddlList");
                            BIQTouchLabFields objLabFields1 = new BIQTouchLabFields();
                            string strResuts = "0";
                            int intRestutID = 0;
                            if (lblControlType.Text == "Radio")
                            {
                                if (btnradRadioButtonList.SelectedValue != "")
                                {
                                    entryFlag = true;
                                    intRestutID = Convert.ToInt32(btnradRadioButtonList.SelectedValue.ToString());
                                }
                            }
                            else if (lblControlType.Text == "Combo Box")
                            {

                                if (ddlList.SelectedValue.ToString() != "")
                                {
                                    entryFlag = true;
                                    intRestutID = Convert.ToInt32(ddlList.SelectedValue.ToString());
                                    //objLabFields1.TestResultId = Convert.ToInt32(ddlList.SelectedValue.ToString());
                                }

                            }
                            else if (lblControlType.Text == "Check box")
                            {

                                if (chkBoxList.SelectedValue.ToString() != "")
                                {
                                    entryFlag = true;
                                    intRestutID = Convert.ToInt32(chkBoxList.SelectedValue.ToString());
                                    //objLabFields1.TestResultId = Convert.ToInt32(chkBoxList.SelectedValue.ToString());
                                }
                            }
                            else if (lblControlType.Text == "GridView")
                            {
                                if (lblLabSubTestName.Text.ToUpper().Equals("ARV MUTATIONS"))
                                {
                                    listArv = ArvMutationData(lblLabSubTestId.Text.ToString());// Code Here
                                    entryFlag = true;
                                }
                                else
                                {
                                    DataRow dR = dtGenXpert.NewRow();
                                    RadGrid radSubGridItems = (RadGrid)item.FindControl("RadGridArvMutation");
                                    GridFooterItem radSubFooterItems = (GridFooterItem)radSubGridItems.MasterTableView.GetItems(GridItemType.Footer)[0];
                                    Telerik.Web.UI.RadComboBox rcbFooterArvType = (Telerik.Web.UI.RadComboBox)radSubFooterItems.FindControl("rcbFooterArvType");
                                    Telerik.Web.UI.RadComboBox rcbFooterMutation = (Telerik.Web.UI.RadComboBox)radSubFooterItems.FindControl("rcbFooterMutation");
                                    Telerik.Web.UI.RadComboBox rcbFooterCulture = (Telerik.Web.UI.RadComboBox)radSubFooterItems.FindControl("rcbFooterCulture");
                                    dR["LabId"] = Convert.ToInt32(Session["LabOrderID"].ToString());
                                    if (rcbFooterArvType.SelectedValue != "")
                                    {
                                        dR["ABFID"] = Convert.ToInt32(rcbFooterArvType.SelectedValue);
                                        dR["ABFText"] = rcbFooterArvType.SelectedItem.Text;
                                    }
                                    if (rcbFooterMutation.SelectedValue != "")
                                    {
                                        dR["GeneXpertID"] = Convert.ToInt32(rcbFooterMutation.SelectedValue);
                                        dR["GeneXpertText"] = rcbFooterMutation.SelectedItem.Text;
                                    }
                                    if (rcbFooterCulture.SelectedValue != "")
                                    {
                                        dR["CultSens"] = Convert.ToInt32(rcbFooterCulture.SelectedValue);
                                        dR["CultSensText"] = rcbFooterCulture.SelectedItem.Text;
                                    }
                                    dR["ParameterID"] = Convert.ToInt32(lblLabSubTestId.Text);
                                    dtGenXpert.Rows.Add(dR);
                                    ViewState["TblGenXpert"] = dtGenXpert;
                                }
                            }
                            else
                            {
                                entryFlag = true;
                                if (txtRadValue.Text != "0" && txtRadValue.Text != "")
                                {
                                    strResuts = txtRadValue.Text.ToString();
                                }
                                else if (txtAlphaRadValue.Text != "")
                                {
                                    strResuts = txtAlphaRadValue.Text.ToString();
                                }
                            }
                            objLabFields1.TestResults = strResuts;
                            objLabFields1.TestResultId = intRestutID;
                            objLabFields1.Ptnpk = Convert.ToInt32(Session["PatientID"]);
                            objLabFields1.LabTestIDs = "0";
                            objLabFields1.LabTestName = "";
                            objLabFields1.LocationId = Convert.ToInt32(Session["AppLocationId"].ToString());
                            objLabFields1.UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                            objLabFields1.OrderedByName = Convert.ToInt32(ddlaborderedbyname.SelectedValue.ToString());
                            objLabFields1.OrderedByDate = Convert.ToDateTime(theUtils.MakeDate(txtlaborderedbydate.Text));                            
                            objLabFields1.IntFlag = 3;
                            objLabFields1.LabTestID = 0;
                            objLabFields1.SubTestID = Convert.ToInt32(lblLabSubTestId.Text.ToString());
                            objLabFields1.PreClinicLabDate = DateGiven("");
                            objLabFields1.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());
                            objLabFields1.ReportedByDate = DateGiven(txtrepordtedbydate.Text);
                            objLabFields1.ReportedByName = Convert.ToInt32(ddlreportedby.SelectedValue);
                            objLabFields1.Flag = GetSubTestIDDeleteFlag(lblLabSubTestId.Text.ToString());
                            //}
                            if (entryFlag == true)
                            {
                                list.Add(objLabFields1);
                            }
                        }
                    }
                }
                else
                {
                    objLabFields.LabOrderId = 0;
                    objLabFields.IntFlag = 0;
                    foreach (GridDataItem item in RadGridLabTest.Items)
                    {

                        Label lblLabSubTestID = (Label)item.FindControl("lblLabSubTestID");
                        Label lblLabTestID = (Label)item.FindControl("lblLabTestID");
                        BIQTouchLabFields objLabFields1 = new BIQTouchLabFields();

                        objLabFields1.Ptnpk = Convert.ToInt32(Session["PatientID"]);
                        objLabFields1.LocationId = Convert.ToInt32(Session["AppLocationId"].ToString());
                        objLabFields1.UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                        objLabFields1.OrderedByDate = Convert.ToDateTime(theUtils.MakeDate(txtlaborderedbydate.Text));
                        objLabFields1.IntFlag = 1;

                        objLabFields1.LabTestID = Convert.ToInt32(lblLabTestID.Text.ToString());
                        objLabFields1.SubTestID = Convert.ToInt32(lblLabSubTestID.Text.ToString());
                        objLabFields1.PreClinicLabDate = objLabFields.PreClinicLabDate;
                        objLabFields1.ReportedByDate = DateGiven(txtrepordtedbydate.Text);

                        //objLabFields1.ReportedByDate = DateGiven(RadDateReportDate.DbSelectedDate.ToString());
                        objLabFields1.ReportedByName = Convert.ToInt32(ddlreportedby.SelectedValue);
                        objLabFields1.Flag = "N";


                        objLabFields1.LabOrderId = 0;
                        objLabFields1.TestResults = "";
                        objLabFields1.TestResultId = 0;

                        list.Add(objLabFields1);
                    }
                }
                CustomFieldClinical theCustomManager = new CustomFieldClinical();
                DataTable theCustomDataDT = new DataTable();
                if (Session["LabOrderID"] != null)
                {
                    theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Update", ApplicationAccess.Laboratory, (DataSet)ViewState["CustomFieldsDS"]);
                }
                else
                    theCustomDataDT = theCustomManager.GenerateInsertUpdateStatement(pnlCustomList, "Insert", ApplicationAccess.Laboratory, (DataSet)ViewState["CustomFieldsDS"]);
                ILabFunctions theILabManager;
                theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
                int result = theILabManager.IQTouchSaveLabOrderTests(objLabFields, list, listArv, dtGenXpert, theCustomDataDT);
                if (result > 0)
                {
                    

                    if (Request.QueryString["opento"] == "ArtForm")
                    {
                        if (Convert.ToInt32(Session["ArtEncounterPatientVisitId"]) > 0)
                        {
                            Session["PatientVisitId"] = Session["ArtEncounterPatientVisitId"];
                        }
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "Close();", true);                        
                        return;
                    }
                    SaveCancel();
                   

                }                

            }
            catch (Exception ex)
            {

            }
            finally
            {
               
            }
        }

        protected string GetSubTestIDDeleteFlag(string subTestID)
        {
            DataTable table = (DataTable)ViewState["tableLabSubTestID"];
            table.PrimaryKey = new DataColumn[] { table.Columns["subTestID"] };
            string deleteFlag = "N";

            if (table.Rows.Find(subTestID) != null)
            {
                DataRow dr = table.Rows.Find(subTestID);
                deleteFlag = dr["DeleteFlag"].ToString();

            }
            return deleteFlag;


        }
        protected List<BIQTouchLabFields> ArvMutationData(string subtestID)
        {
            List<BIQTouchLabFields> listArv = new List<BIQTouchLabFields>();
           
            if (ViewState["TblArvMutation"] != null)
            {
                DataTable dt = (DataTable)ViewState["TblArvMutation"];
                foreach (DataRow dr in dt.Rows)
                {

                    BIQTouchLabFields obj1 = new BIQTouchLabFields();
                    obj1.UserId = Int32.Parse(Session["AppUserId"].ToString());
                    obj1.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());
                    obj1.SubTestID = Convert.ToInt32(subtestID);
                    obj1.MutationID = Convert.ToInt32(dr["ArvMutationID"].ToString());
                    obj1.OtherMutation = dr["ArvMutationOther"].ToString();
                    obj1.ArvTypeID = Convert.ToInt32(dr["ArvTypeID"].ToString());
                    
                    if (GetSubTestIDDeleteFlag(subtestID) == "Y")
                    {
                        obj1.Flag = "X";  // Removing Parameter ID from arvMutation Table when user deleting testID from parent grid
                    }
                    else
                    {
                        obj1.Flag = dr["DeleteFlag"].ToString();
                    }
                    listArv.Add(obj1);

                }
            }
            return listArv;

        }

        protected void radGridLabResult_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {


            if (e.Item is GridDataItem)
            {
                //TextBox tb = new TextBox();
                //tb.ID = tbID;
                //tb.EnabeViewState = true;
                //gdi["column name"].controls.add(tb);




                //RadComboBox combo = ((RadComboBox)e.Item.FindControl("rdcmbfrequency"));
                //combo.DataSource = ((DataTable)Session["Frequency"]);
                //combo.DataValueField = "FrequencyId";
                //combo.DataTextField = "FrequencyName";
                //combo.DataBind();
            }
            //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
        }
        protected DataTable GetDataTable(string flag, string labtestids, int LabOrderID)
        {

            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            objLabFields.Flag = flag;
            objLabFields.LabTestIDs = labtestids;
            objLabFields.LabOrderId = LabOrderID;
            objLabFields.LabTestName = "";

            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataSet Ds = theILabManager.GetPatientLabTestID(objLabFields);
            DataTable dt = Ds.Tables[0];
            return dt;



        }

        protected void RadGridLabResult_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;

                Label lblID = (Label)parentItem.FindControl("lblLabSubTestID");
                //lblID
                DataTable dt = GetDataTable("QRY_CHILDGRID", lblID.Text, Convert.ToInt32(Session["LabOrderID"].ToString()));
                (sender as RadGrid as RadGrid).DataSource = dt;//new Object[0];


                //(sender as RadGrid as RadGrid).DataBind();
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

            }
            catch (Exception ex)
            {
                //lblerr.Text = ex.Message.ToString();


            }
        }

        protected void BindRadioButtonList(RadioButtonList rbList, string labSubTestID)
        {
            DataTable dt = GetDataTable("QRY_CHILDRB", labSubTestID, 0);
            rbList.DataSource = dt;
            rbList.DataTextField = "Result";
            rbList.DataValueField = "ResultID";
            rbList.DataBind();


        }
        protected void BindDropdownist(DropDownList rbList, string labSubTestID)
        {
            DataTable dt = GetDataTable("QRY_CHILDRB", labSubTestID, 0);
            rbList.DataSource = dt;
            rbList.DataTextField = "Result";
            rbList.DataValueField = "ResultID";
            rbList.DataBind();
            rbList.Items.Insert(0, new ListItem("Select", ""));

        }
        protected void BindCheckBoxList(CheckBoxList rbList, string labSubTestID)
        {
            DataTable dt = GetDataTable("QRY_CHILDRB", labSubTestID, 0);
            rbList.DataSource = dt;
            rbList.DataTextField = "Result";
            rbList.DataValueField = "ResultID";
            rbList.DataBind();


        }
        protected DataTable GetArvMutationDataTable(string flag)
        {
            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            objLabFields.Flag = flag;
            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataSet Ds = theILabManager.IQTouchLaboratoryGetArvMutationMasterList(objLabFields);
            DataTable dt = Ds.Tables[0];
            return dt;
        }

        protected DataTable GetArvMutationGrid(string flag)
        {
            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            objLabFields.Flag = flag;
            objLabFields.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());

            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataSet Ds = theILabManager.IQTouchLaboratoryGetArvMutationDetails(objLabFields);
            DataTable dt = Ds.Tables[0];
            return dt;
        }

        protected DataTable GetGenXpertGrid(string flag, int TestId)
        {
            
            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            objLabFields.Flag = flag;
            if (Session["LabOrderID"] != null)
                objLabFields.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());
            else
                objLabFields.LabOrderId = 0;

            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataSet Ds = theILabManager.IQTouchLaboratoryGetGenXpertDetails(objLabFields, TestId);
            DataTable dt = Ds.Tables[0];
            return dt;
        }


        protected void BindArvType(RadComboBox rcb)
        {

            DataTable dt = GetArvMutationDataTable("ARV_TYPE");
            rcb.DataSource = dt;
            rcb.DataTextField = "ITEM_NAME";
            rcb.DataValueField = "ID";
            rcb.DataBind();




        }
        protected void BindArvMutation(RadComboBox rcb, Int32 ArvTypeID)
        {

            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            objLabFields.Flag = "ARV_MUTATION";
            objLabFields.ArvTypeID = ArvTypeID;
            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataSet Ds = theILabManager.IQTouchLaboratoryGetArvMutationMasterList(objLabFields);
            DataTable dt = Ds.Tables[0];
            rcb.DataSource = dt;
            rcb.DataTextField = "ITEM_NAME";
            rcb.DataValueField = "ID";
            rcb.DataBind();
        }
        protected void RadGridResut_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "ChildGrid")
            {
                GridDataItem item = (GridDataItem)e.Item;
                Label lblLabSubTestId = (Label)item.FindControl("lblLabSubTestId");
                Label lblLabSubTestName = (Label)item.FindControl("lblLabTestName");
                Label lblControlType = (Label)item.FindControl("lblControlType");
                Label lblUnitName = (Label)item.FindControl("lblUnitName");
                Label lblMinBoundaryVal = (Label)item.FindControl("lblMinBoundaryVal");
                Label lblMaxBoundaryVal = (Label)item.FindControl("lblMaxBoundaryVal");
                Label lblTestResultId = (Label)item.FindControl("lblTestResultId");

                //Telerik.Web.UI.RadButton btnradRadioButtonList = (Telerik.Web.UI.RadButton)item.FindControl("btnRadRadiolist");
                RadioButtonList btnradRadioButtonList = (RadioButtonList)item.FindControl("btnRadRadiolist");
                Telerik.Web.UI.RadNumericTextBox txtRadValue = (Telerik.Web.UI.RadNumericTextBox)item.FindControl("txtRadValue");
                Telerik.Web.UI.RadTextBox txtAlphaRadValue = (Telerik.Web.UI.RadTextBox)item.FindControl("txtAlphaRadValue");
                Label lblresult = (Label)item.FindControl("lblTestResults");
                CheckBoxList chkBoxList = (CheckBoxList)item.FindControl("chkBoxList");
                DropDownList ddlList = (DropDownList)item.FindControl("ddlList");
                RadGrid radGridArvMutation = (RadGrid)item.FindControl("RadGridArvMutation");


                lblUnitName.Visible = false;
                txtRadValue.Visible = false;
                txtAlphaRadValue.Visible = false;
                btnradRadioButtonList.Visible = false;
                chkBoxList.Visible = false;
                ddlList.Visible = false;
                radGridArvMutation.Visible = false;
                if (lblControlType.Text == "Radio")
                {
                    BindRadioButtonList(btnradRadioButtonList, lblLabSubTestId.Text);
                    if (Convert.ToInt32(lblTestResultId.Text.ToString()) > 0)
                    {
                        btnradRadioButtonList.SelectedValue = lblTestResultId.Text;
                    }

                    btnradRadioButtonList.Visible = true;

                }
                else if (lblControlType.Text == "Combo Box")
                {
                    BindDropdownist(ddlList, lblLabSubTestId.Text);
                    if (Convert.ToInt32(lblTestResultId.Text.ToString()) > 0)
                    {
                        ddlList.SelectedValue = lblTestResultId.Text;
                    }

                    ddlList.Visible = true;
                }
                else if (lblControlType.Text == "Check box")
                {
                    BindCheckBoxList(chkBoxList, lblLabSubTestId.Text);
                    if (Convert.ToInt32(lblTestResultId.Text.ToString()) > 0)
                    {
                        chkBoxList.SelectedValue = lblTestResultId.Text;
                    }
                    chkBoxList.Visible = true;
                }
                else if (lblControlType.Text == "GridView")
                {
                    radGridArvMutation.Visible = true;
                    //if (lblLabSubTestId.Text.ToString() == "17" || lblLabSubTestId.Text.ToString() == "18" || lblLabSubTestId.Text.ToString() == "19" || lblLabSubTestId.Text.ToString() == "131")
                    //{
                    if (lblLabSubTestName.Text.ToString().ToUpper().Contains("SPUTUM AFB") || lblLabSubTestName.Text.ToString().ToUpper().Equals("GENEXPERT"))
                    {

                        txthdnfield.Value = lblLabSubTestName.Text;
                        radGridArvMutation.Columns[0].HeaderText = "ABF";
                        radGridArvMutation.Columns[1].HeaderText = "GeneXpert";
                        radGridArvMutation.Columns[3].Visible = false;
                        radGridArvMutation.Columns[4].Visible = false;
                        DataTable dt = GetGenXpertGrid("GENXPERT", Convert.ToInt32(lblLabSubTestId.Text));
                        if (dt.Rows.Count > 0)
                        {
                            //ViewState["TblArvMutation"] = dt;
                            radGridArvMutation.DataSource = dt;
                            radGridArvMutation.DataBind();
                        }
                        else
                        {
                            radGridArvMutation.DataSource = new Object[0];
                            radGridArvMutation.DataBind();
                        }
                    }
                    else if(lblLabSubTestName.Text.ToUpper().Equals("ARV MUTATIONS"))
                    {
                        txthdnfield.Value = lblLabSubTestName.Text;
                        radGridArvMutation.Columns[2].Visible = false;
                        //BindArvType
                        DataTable dt = GetArvMutationGrid("MUTATION_GRID");
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["TblArvMutation"] = dt;
                            radGridArvMutation.DataSource = dt;
                            radGridArvMutation.DataBind();
                        }
                        else
                        {
                            radGridArvMutation.DataSource = new Object[0];
                            radGridArvMutation.DataBind();
                        }
                        // Bind GridView RadGridArvMutation
                    }
                    radGridArvMutation.ItemDataBound += new GridItemEventHandler(RadGridArvMutation_ItemDataBound);
                    radGridArvMutation.ItemCommand += new GridCommandEventHandler(RadGridArvMutation_ItemCommand);
                    radGridArvMutation.DeleteCommand += new GridCommandEventHandler(RadGridArvMutation_DeleteCommand);
                }
                else if (lblControlType.Text == "Single line text box")
                {
                    lblUnitName.Visible = true;
                    txtAlphaRadValue.Visible = true;
                }
                else if (lblControlType.Text == "Int Text Box")
                {
                    lblUnitName.Visible = true;
                    txtRadValue.Visible = true;
                    if (Convert.ToDouble(lblMinBoundaryVal.Text.ToString()) == 0 && Convert.ToDouble(lblMaxBoundaryVal.Text.ToString()) == 0)
                    {
                        txtRadValue.MinValue = 0;
                        txtRadValue.MaxValue = 99999;
                    }
                    else
                    {

                        txtRadValue.MinValue = Convert.ToDouble(lblMinBoundaryVal.Text.ToString());
                        txtRadValue.MaxValue = Convert.ToDouble(lblMaxBoundaryVal.Text.ToString());
                    }
                    txtRadValue.Text = "";
                    txtRadValue.Text = lblresult.Text;
                }
                else
                {
                    lblUnitName.Visible = true;
                    txtAlphaRadValue.Visible = true;                    

                }
            }

        }
        public DataTable CreateDtArvMutationTable()
        {
            DataTable dtlArvMutation = new DataTable();
            dtlArvMutation.Columns.Add("ID", typeof(string));
            dtlArvMutation.Columns.Add("ArvTypeID", typeof(string));
            dtlArvMutation.Columns.Add("ArvType", typeof(string));
            dtlArvMutation.Columns.Add("ArvMutationID", typeof(string));
            dtlArvMutation.Columns.Add("ArvMutation", typeof(string));
            dtlArvMutation.Columns.Add("ArvMutationOther", typeof(string));
            dtlArvMutation.Columns.Add("DeleteFlag", typeof(string));
            dtlArvMutation.Columns.Add("DeleteFlagdb", typeof(string));

            dtlArvMutation.PrimaryKey = new DataColumn[] { dtlArvMutation.Columns["ID"] };
            return dtlArvMutation;

        }

        public DataTable CreateDtGenXpertTable()
        {
            DataTable dtlGenXpert = new DataTable();
            dtlGenXpert.Columns.Add("LabId", typeof(int));
            dtlGenXpert.Columns.Add("ABFID", typeof(int));
            dtlGenXpert.Columns.Add("ABFText", typeof(string));
            dtlGenXpert.Columns.Add("GeneXpertID", typeof(int));
            dtlGenXpert.Columns.Add("GeneXpertText", typeof(string));
            dtlGenXpert.Columns.Add("CultSens", typeof(int));
            dtlGenXpert.Columns.Add("CultSensText", typeof(string));
            dtlGenXpert.Columns.Add("ParameterID", typeof(int));
            return dtlGenXpert;
        }



        protected void RadGridArvMutation_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItm = e.Item as GridDataItem;
            RadGrid radGridArvMutation = (RadGrid)sender;

            Label lblID = (Label)dataItm.FindControl("lblID");
            string id = lblID.Text;
            DataTable table = (DataTable)ViewState["TblArvMutation"];
            table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

            if (table.Rows.Find(id) != null)
            {
                DataRow dr = table.Rows.Find(id);


                if (dr["DeleteFlagdb"].ToString() == "N")
                {
                    table.Rows.Find(id).Delete();
                }
                else
                {
                    dr["DeleteFlag"] = "Y";
                }


                // 
                table.AcceptChanges();
                var query = from inv in table.AsEnumerable()
                            where inv.Field<string>("DeleteFlag") == "N"
                            select new
                            {
                                ID = inv["ID"],
                                ArvTypeID = inv["ArvTypeID"],
                                ArvType = inv["ArvType"],
                                ArvMutationID = inv["ArvMutationID"],
                                ArvMutation = inv["ArvMutation"],
                                ArvMutationOther = inv["ArvMutationOther"],
                                DeleteFlag = inv["DeleteFlag"]
                            };


                ViewState["TblArvMutation"] = table;
                radGridArvMutation.DataSource = query;
                radGridArvMutation.DataBind();
            }
            else
            {
                radGridArvMutation.DataSource = (DataTable)ViewState["TblArvMutation"];
                radGridArvMutation.DataBind();

            }

        }

        protected void RadGridSputum_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItm = e.Item as GridDataItem;
            RadGrid radGridArvMutation = (RadGrid)sender;

            Label lblID = (Label)dataItm.FindControl("lblID");
            string id = lblID.Text;
            DataTable table = (DataTable)ViewState["TblArvMutation"];
            table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

            if (table.Rows.Find(id) != null)
            {
                DataRow dr = table.Rows.Find(id);


                if (dr["DeleteFlagdb"].ToString() == "N")
                {
                    table.Rows.Find(id).Delete();
                }
                else
                {
                    dr["DeleteFlag"] = "Y";
                }


                // 
                table.AcceptChanges();
                var query = from inv in table.AsEnumerable()
                            where inv.Field<string>("DeleteFlag") == "N"
                            select new
                            {
                                ID = inv["ID"],
                                ArvTypeID = inv["ArvTypeID"],
                                ArvType = inv["ArvType"],
                                ArvMutationID = inv["ArvMutationID"],
                                ArvMutation = inv["ArvMutation"],
                                ArvMutationOther = inv["ArvMutationOther"],
                                DeleteFlag = inv["DeleteFlag"]
                            };


                ViewState["TblArvMutation"] = table;
                radGridArvMutation.DataSource = query;
                radGridArvMutation.DataBind();
            }
            else
            {
                radGridArvMutation.DataSource = (DataTable)ViewState["TblArvMutation"];
                radGridArvMutation.DataBind();

            }

        }
        protected void RadGridArvMutation_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            DataTable dtMutation;

            if (ViewState["TblArvMutation"] == null)
            {
                dtMutation = CreateDtArvMutationTable();
            }
            else
            {
                dtMutation = (DataTable)ViewState["TblArvMutation"];
            }
            DataRow dR = dtMutation.NewRow();

            int dtnextId = Convert.ToInt32(dtMutation.Rows.Count) + 1;
            RadGrid radGridArvMutation = (RadGrid)sender;


            if (e.CommandName == "Insert")
            {
                if (radGridArvMutation != null)
                {
                    // GridItem[] footerItems = RadOtherVaccine.MasterTableView.GetItems(GridItemType.Footer);
                    GridFooterItem footeritem = (GridFooterItem)radGridArvMutation.MasterTableView.GetItems(GridItemType.Footer)[0];
                    if (footeritem != null)
                    {
                        txthdnfield.Value = "ARV MUTATIONS";
                        Telerik.Web.UI.RadComboBox rcbFooterArvType = (Telerik.Web.UI.RadComboBox)footeritem.FindControl("rcbFooterArvType");
                        Telerik.Web.UI.RadComboBox rcbFooterMutation = (Telerik.Web.UI.RadComboBox)footeritem.FindControl("rcbFooterMutation");
                        Telerik.Web.UI.RadTextBox txtOtherFooterMutation = (Telerik.Web.UI.RadTextBox)footeritem.FindControl("txtOtherFooterMutation");
                        dR["ID"] = dtnextId.ToString();// +rcbFooterMutation.SelectedItem.Text;
                        dR["ArvTypeID"] = rcbFooterArvType.SelectedValue.ToString();
                        dR["ArvType"] = rcbFooterArvType.SelectedItem.Text.ToString();
                        dR["ArvMutationID"] = rcbFooterMutation.SelectedValue.ToString();
                        dR["ArvMutation"] = rcbFooterMutation.SelectedItem.Text.ToString();
                        dR["ArvMutationOther"] = txtOtherFooterMutation.Text.ToString();
                        dR["DeleteFlag"] = "N";
                        dR["DeleteFlagdb"] = "N";
                        dtMutation.Rows.Add(dR);
                        ViewState["TblArvMutation"] = dtMutation;

                        var query = from inv in dtMutation.AsEnumerable()
                                    where inv.Field<string>("DeleteFlag") == "N"
                                    select new
                                    {
                                        ID = inv["ID"],
                                        ArvTypeID = inv["ArvTypeID"],
                                        ArvType = inv["ArvType"],
                                        ArvMutationID = inv["ArvMutationID"],
                                        ArvMutation = inv["ArvMutation"],
                                        ArvMutationOther = inv["ArvMutationOther"],
                                        DeleteFlag = inv["DeleteFlag"]
                                    };


                        radGridArvMutation.DataSource = query;
                        radGridArvMutation.DataBind();
                        // RadGridArvMutation_ItemDataBound(sender, e);
                    }
                }

            }


        }

        protected void RadGridSputum_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            DataTable dtMutation;
            if (ViewState["TblArvMutation"] == null)
            {
                dtMutation = CreateDtArvMutationTable();
            }
            else
            {
                dtMutation = (DataTable)ViewState["TblArvMutation"];
            }
            DataRow dR = dtMutation.NewRow();

            int dtnextId = Convert.ToInt32(dtMutation.Rows.Count) + 1;
            RadGrid radGridArvMutation = (RadGrid)sender;
            if (e.CommandName == "Insert")
            {
                if (radGridArvMutation != null)
                {
                    // GridItem[] footerItems = RadOtherVaccine.MasterTableView.GetItems(GridItemType.Footer);
                    GridFooterItem footeritem = (GridFooterItem)radGridArvMutation.MasterTableView.GetItems(GridItemType.Footer)[0];
                    if (footeritem != null)
                    {
                        Telerik.Web.UI.RadComboBox rcbFooterArvType = (Telerik.Web.UI.RadComboBox)footeritem.FindControl("rcbFooterArvType");
                        Telerik.Web.UI.RadComboBox rcbFooterMutation = (Telerik.Web.UI.RadComboBox)footeritem.FindControl("rcbFooterMutation");
                        Telerik.Web.UI.RadTextBox txtOtherFooterMutation = (Telerik.Web.UI.RadTextBox)footeritem.FindControl("txtOtherFooterMutation");
                        dR["ID"] = dtnextId.ToString() + rcbFooterMutation.SelectedItem.Text;
                        dR["ArvTypeID"] = rcbFooterArvType.SelectedValue.ToString();
                        dR["ArvType"] = rcbFooterArvType.SelectedItem.Text.ToString();
                        dR["ArvMutationID"] = rcbFooterMutation.SelectedValue.ToString();
                        dR["ArvMutation"] = rcbFooterMutation.SelectedItem.Text.ToString();
                        dR["ArvMutationOther"] = txtOtherFooterMutation.Text.ToString();
                        dR["DeleteFlag"] = "N";
                        dR["DeleteFlagdb"] = "N";
                        dtMutation.Rows.Add(dR);
                        ViewState["TblArvMutation"] = dtMutation;

                        var query = from inv in dtMutation.AsEnumerable()
                                    where inv.Field<string>("DeleteFlag") == "N"
                                    select new
                                    {
                                        ID = inv["ID"],
                                        ArvTypeID = inv["ArvTypeID"],
                                        ArvType = inv["ArvType"],
                                        ArvMutationID = inv["ArvMutationID"],
                                        ArvMutation = inv["ArvMutation"],
                                        ArvMutationOther = inv["ArvMutationOther"],
                                        DeleteFlag = inv["DeleteFlag"]
                                    };


                        radGridArvMutation.DataSource = query;
                        radGridArvMutation.DataBind();
                        // RadGridArvMutation_ItemDataBound(sender, e);
                    }
                }

            }


        }
        protected void RadGridArvMutation_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            RadGrid RdGrd = (RadGrid)sender;
            if (e.Item is GridFooterItem)
            {
                GridFooterItem footeritem = (GridFooterItem)e.Item;
                RadComboBox rcbFooterArvType = (RadComboBox)footeritem.FindControl("rcbFooterArvType");
                if (txthdnfield.Value.ToString().ToUpper().Contains("SPUTUM AFB") || txthdnfield.Value.ToString().ToUpper().Equals("GENEXPERT"))
                {
                    rcbFooterArvType.Items.Clear();
                    rcbFooterArvType.Items.Add(new RadComboBoxItem("Positive", "1"));
                    rcbFooterArvType.Items.Add(new RadComboBoxItem("Negative", "0"));
                }
                else if (txthdnfield.Value.ToString().ToUpper().Contains("ARV MUTATIONS"))
                {
                    DataTable dt = GetArvMutationDataTable("ARV_TYPE");
                    rcbFooterArvType.DataSource = dt;
                    rcbFooterArvType.DataTextField = "ITEM_NAME";
                    rcbFooterArvType.DataValueField = "ID";
                    rcbFooterArvType.DataBind();
                }
                rcbFooterArvType.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbFooterArvType_SelectedIndexChanged);
            }

            //if (e.Item is GridFooterItem)
            //{
            //    GridFooterItem footeritem = (GridFooterItem)e.Item;
            //    RadComboBox rcbFooterArvType = (RadComboBox)footeritem.FindControl("rcbFooterArvType");
            //    if (ViewState["Seputm"].ToString() == "Seputm")
            //    {
            //        rcbFooterArvType.Items.Add(new RadComboBoxItem("Positive", "1"));
            //        rcbFooterArvType.Items.Add(new RadComboBoxItem("Negative", "0"));
            //    }
            //    else
            //    {
            //        BindArvType(rcbFooterArvType);
            //    }
            //    rcbFooterArvType.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbFooterArvType_SelectedIndexChanged);
            //}
        }

        protected void rcbFooterArvType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox combo = (RadComboBox)sender;
            GridFooterItem footeritem = (GridFooterItem)combo.NamingContainer;
            RadComboBox rcbFooterMutation = (RadComboBox)footeritem.FindControl("rcbFooterMutation");
            //Jayant-Start 
            if (e.Value == "0" && e.Text == "Negative")
            {
                rcbFooterMutation.Items.Clear();
                rcbFooterMutation.Items.Add(new RadComboBoxItem("Positive-Resistant", "1"));
                rcbFooterMutation.Items.Add(new RadComboBoxItem("Positive-Sensitive", "2"));
                rcbFooterMutation.Items.Add(new RadComboBoxItem("Negative", "0"));
            }
            else if (e.Value == "1" && e.Text == "Positive")
            {
                rcbFooterMutation.Items.Clear();
            }
            else if (e.Value == "1" && e.Text == "NRTI")
            {
                rcbFooterMutation.Items.Clear();
                BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            }
            else if (e.Value == "2" && e.Text == "NNRTI")
            {
                rcbFooterMutation.Items.Clear();
                BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            }
            else if (e.Value == "3" && e.Text == "PI")
            {
                rcbFooterMutation.Items.Clear();
                BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            }
            else if (e.Value == "4" && e.Text == "INI")
            {
                rcbFooterMutation.Items.Clear();
                BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            }
            rcbFooterMutation.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbFooterMutation_SelectedIndexChanged);

            //RadComboBox combo = (RadComboBox)sender;
            //GridFooterItem footeritem = (GridFooterItem)combo.NamingContainer;
            //RadComboBox rcbFooterMutation = (RadComboBox)footeritem.FindControl("rcbFooterMutation");
            //if (ViewState["Seputm"].ToString() == "Seputm")
            //{
            //    if (e.Value == "0")
            //    {
            //        rcbFooterMutation.Items.Clear();
            //        rcbFooterMutation.Items.Add(new RadComboBoxItem("Positive-Resistant", "1"));
            //        rcbFooterMutation.Items.Add(new RadComboBoxItem("Positive-Sensitive", "2"));
            //        rcbFooterMutation.Items.Add(new RadComboBoxItem("Negative", "0"));
            //    }
            //    else if (e.Value == "1")
            //    {
            //        BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            //    }
            //    else if (e.Value == "2")
            //    {
            //        BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            //    }
            //    else if (e.Value == "3")
            //    {
            //        BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            //    }
            //    else if (e.Value == "4")
            //    {
            //        BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            //    }
            //    else
            //    {
            //        rcbFooterMutation.Items.Clear();
            //    }

            //}
            //else
            //{
            //    BindArvMutation(rcbFooterMutation, Convert.ToInt32(e.Value));
            //}
            //rcbFooterMutation.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbFooterMutation_SelectedIndexChanged);

        }
        protected void rcbFooterMutation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox combo = (RadComboBox)sender;
            GridFooterItem footeritem = (GridFooterItem)combo.NamingContainer;
            RadComboBox rcbFooterCulture = (RadComboBox)footeritem.FindControl("rcbFooterCulture");
            if (e.Value == "1")
            {
                rcbFooterCulture.Items.Clear();
                rcbFooterCulture.Items.Add(new RadComboBoxItem("Confirmed-Resistant", "1"));
                rcbFooterCulture.Items.Add(new RadComboBoxItem("Not-Resistant", "2"));
            }
            else
            {
                rcbFooterCulture.Items.Clear();
            }
        }
        protected void BindChildGridDdl(Telerik.Web.UI.RadComboBox rcbComobo, string flag, string dataTextName, string dataValueName)
        {

            //DataTable dt = GetDataTable(flag, 0, "");
            //rcbComobo.DataTextField = dataTextName;
            //rcbComobo.DataValueField = dataValueName;
            //rcbComobo.DataSource = dt;
            //rcbComobo.DataBind();
            //rcbComobo.SelectedValue = "";
        }
        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (Request.QueryString["name"] == "Delete")
            {
                DeleteForm();
            }
            else if (FieldValidation() == false)
            {
                return;
            }
            else
                SaveLabOrder();
            // BtnBack_Click(sender, e);
        }
        private void DeleteForm()
        {
            int theResultRow, OrderNo;
            string FormName;
            OrderNo = Convert.ToInt32(Session["PatientVisitId"]);
            FormName = "Laboratory";

            ILabFunctions LabResultManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            theResultRow = (int)LabResultManager.DeleteLabForms(FormName, OrderNo, Convert.ToInt32(Session["PatientId"]), Convert.ToInt32(Session["AppUserId"].ToString()));

            if (theResultRow == 0)
            {
                IQCareMsgBox.Show("RemoveFormError", this);
                return;
            }
            else
            {
                string theUrl;
                theUrl = string.Format("../ClinicalForms/frmPatient_Home.aspx");
                Response.Redirect(theUrl);

            }
        }
        private void PutCustomControl()
        {
            ICustomFields CustomFields;
            CustomFieldClinical theCustomField = new CustomFieldClinical();
            try
            {

                CustomFields = (ICustomFields)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomFields,BusinessProcess.Administration");
                DataSet theDS = CustomFields.GetCustomFieldListforAForm(Convert.ToInt32(ApplicationAccess.Laboratory));

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
        }
        private void FillOldCustomData(Int32 PatID)
        {
            DataSet dsvalues = null;
            ICustomFields CustomFields;
            Int32 LabId = 0;
            if (Session["PatientVisitId"] != null)
                LabId = Convert.ToInt32(Session["PatientVisitId"]);
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
                dsvalues = CustomFields.GetCustomFieldValues("dtl_CustomField_" + theTblName.ToString().Replace("-", "_"), theColName, Convert.ToInt32(PatID.ToString()), 0, 0, Convert.ToInt32(LabId), 0, Convert.ToInt32(ApplicationAccess.Laboratory));
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
        }
    }
}