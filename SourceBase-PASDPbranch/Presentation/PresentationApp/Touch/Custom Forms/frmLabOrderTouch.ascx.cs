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
using Interface.Laboratory;

//Third party Libs
using Telerik.Web.UI;
using System.Data;
using System.IO;

namespace Touch.Custom_Forms
{
    public partial class frmLabOrderTouch : TouchUserControlBase
    {
        static Boolean IsError=false;

        protected void Page_Load(object sender, EventArgs e)
        {

           
            String script = frmLabOrder_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            if (IsPostBack) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);
            
            Session["CurrentForm"] = this;
            Session["FormIsLoaded"] = true;
            BindAutoSelectLabTest("a");
            //btnSave.Attributes.Add("onclick", "FormValidatedOnSubmit()");
            if (Session["IsFirstLoad"].ToString() == "true")
            {
                // Code Here 
                Session["CurrentForm"] = "frmLabOrderTouch";
                Session["IsFirstLoad"] = "Load";
                BindMasterData();
                LoadBlankGrid();
                BindEmpLoyee(rcbOrderBy);
                BindEmpLoyee(rcbReportedBy);
            }
            base.Page_Load(sender, e);

            /***************** Check For User Rights ****************/
            AuthenticationManager Authentication = new AuthenticationManager();

            btnPrint.Visible = Authentication.HasFunctionRight(ApplicationAccess.PASDPLabrotary, FunctionAccess.Print, (DataTable)Session["UserRight"]);

            if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPLabrotary, FunctionAccess.Update, (DataTable)Session["UserRight"]))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showsave", "$('#divSave').hide();", true);
            }
           

        }
        protected void BindEmpLoyee(Telerik.Web.UI.RadComboBox rfbEmployee)
        {
            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            DataTable dt = theILabManager.GetEmployeeDetails();
            rfbEmployee.DataTextField = "EmployeeName";
            rfbEmployee.DataValueField = "EmployeeID";
            rfbEmployee.DataSource = dt;
            rfbEmployee.DataBind();
            rfbEmployee.SelectedValue = "";


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

        #region RadGridEvents

        protected void RadGridLabTest_ItemCommand(object source, GridCommandEventArgs e)
        {

            //if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            //{

            //    ((GridDataItem)e.Item).ChildItem.FindControl("RadGridLabResult").Visible =

            //        !e.Item.Expanded;

            //}

        }
        protected void RadGridLabTest_DeleteCommand(object sender, GridCommandEventArgs e)
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

        protected void RadGridLabTest_ItemCreated(object sender, GridItemEventArgs e)
        {
            //if (e.Item is GridNestedViewItem)
            //{
            //    e.Item.FindControl("RadGridLabResult").Visible = ((GridNestedViewItem)e.Item).ParentItem.Expanded;
            //    // (e.Item.FindControl("RadGridLabResult") as RadGrid).NeedDataSource += new GridNeedDataSourceEventHandler(RadGridLabResult_NeedDataSource);


            //        RadGrid radGridLabResult = (RadGrid)e.Item.FindControl("RadGridLabResult");
            //        radGridLabResult.ItemCreated += new GridItemEventHandler(radGridLabResult_ItemCreated);
            //        radGridLabResult.ItemDataBound += new GridItemEventHandler(RadGridResut_ItemDataBound);


            //}



        }
        //protected void radGridLabResult_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        //{

        //    if (e.Item is GridDataItem)
        //    {
        //        //TextBox tb = new TextBox();
        //        //tb.ID = tbID;
        //        //tb.EnabeViewState = true;
        //        //gdi["column name"].controls.add(tb);




        //        //RadComboBox combo = ((RadComboBox)e.Item.FindControl("rdcmbfrequency"));
        //        //combo.DataSource = ((DataTable)Session["Frequency"]);
        //        //combo.DataValueField = "FrequencyId";
        //        //combo.DataTextField = "FrequencyName";
        //        //combo.DataBind();
        //    }
        //    //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
        //}

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
                lblerr.Text = ex.Message.ToString();


            }
        }

        protected void RadGridResut_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {


            //if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "ChildGrid")
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    Label lblLabId = (Label)item.FindControl("lblLabId");
            //    Telerik.Web.UI.RadComboBox rcbAbf = (Telerik.Web.UI.RadComboBox)item.FindControl("rcbAbf");
            //    Telerik.Web.UI.RadComboBox rcbGenXpert = (Telerik.Web.UI.RadComboBox)item.FindControl("rcbGenXpert");


            //    //
            //    RadGrid radabResult = (sender as RadGrid as RadGrid);


            //    radabResult.MasterTableView.GetColumn("LabId").Visible = false;

            //    if (lblLabId.Text.ToString() == "1")
            //    {
            //        radabResult.MasterTableView.GetColumn("ABF").Visible = false;
            //        radabResult.MasterTableView.GetColumn("GeneXpert").Visible = false;
            //        radabResult.MasterTableView.GetColumn("Cul_Sen").Visible = false;
            //    }
            //    else if (lblLabId.Text.ToString() == "2")
            //    {

            //        radabResult.MasterTableView.GetColumn("Result").Visible = false;
            //        radabResult.MasterTableView.GetColumn("NormalRange").Visible = false;
            //        BindChildGridDdl(rcbAbf, "ABF_List", "DataText", "DataValue");
            //        BindChildGridDdl(rcbGenXpert, "GENEXPERT", "DataText", "DataValue");
            //    }
            //    else
            //    {
            //        radabResult.MasterTableView.GetColumn("ABF").Visible = false;
            //        radabResult.MasterTableView.GetColumn("GeneXpert").Visible = false;
            //        radabResult.MasterTableView.GetColumn("Cul_Sen").Visible = false;
            //    }

            //    // your logic should come here 

            //}

        }

        protected void BindChildGridDdl(Telerik.Web.UI.RadComboBox rcbComobo, string flag, string dataTextName, string dataValueName)
        {

            DataTable dt = GetDataTable(flag, "", "");
            rcbComobo.DataTextField = dataTextName;
            rcbComobo.DataValueField = dataValueName;
            rcbComobo.DataSource = dt;
            rcbComobo.DataBind();
            rcbComobo.SelectedValue = "";
        }
        //public void BindSubRequirments(RadGrid RadGridLabResult)
        //{
        //}
        #endregion

        protected void BtnAddDrugClick(object sender, EventArgs e)
        {

            //var collection = rcbPreSelectedLabTest.CheckedItems;
           
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


        protected void BtnBack_Click(object sender, EventArgs e)
        {
            //Response.Redirect("frmLaboratoryTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString());
            
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "loadpharm", "parent.ShowLoading()", true);
            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmLaboratoryTouch";

            Touch.Custom_Forms.frmLaboratoryTouch fr = (frmLaboratoryTouch)mp.LoadControl("~/Touch/Custom Forms/frmLaboratoryTouch.ascx");

            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            frmLaboratoryTouch theFrm = (frmLaboratoryTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());

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
        DateTime DateGiven(string dateVal)
        {
            DateTime dt = Convert.ToDateTime("01/01/1900");
            if (!string.IsNullOrEmpty(dateVal))
            {
                dt = DateTime.Parse(dateVal);
            }
            return dt;
        }

        private Boolean ValidationFormData()
        {
            if (rcbOrderBy.SelectedValue == "")
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchOrderedByName");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;

            }
            //if (rcbReportedBy.SelectedValue == "")
            //{
            //    //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
            //    RawMessage theMsg = MsgRepository.GetMessage("IQTouchReportedByName");
            //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
            //    return false;

            //}
            if (RadDateOrder.DbSelectedDate == null)
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchOrderDate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;

            }
            //if (RadDateReportDate.DbSelectedDate == null)
            //{
            //    //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
            //    RawMessage theMsg = MsgRepository.GetMessage("IQTouchReportedDate");
            //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
            //    return false;

            //}
            if (hiddTestID.Value =="")
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchSubTestID");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;

            }
            // Date Validation

            if (Convert.ToDateTime(RadDateOrder.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
            {
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchLabOrderDate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;
            }

            //if (Convert.ToDateTime(RadDateReportDate.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
            //{
            //    RawMessage theMsg = MsgRepository.GetMessage("IQTouchlabOrderReportedByDate");
            //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
            //    return false;
            //}
            //if (Convert.ToDateTime(RadDateOrder.SelectedDate.Value) > Convert.ToDateTime(RadDateReportDate.SelectedDate.Value))
            //{
            //    RawMessage theMsg = MsgRepository.GetMessage("IQTouchlabOrderDateCompReportedDate");
            //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
            //    return false;
            
            //}

            
            
            


           
           

            return true;

        }

        #region -- Save Lab Order----
        protected void SaveLabOrder()
        {
            //Default code for User Control Load 
           

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "loadpharm", "parent.ShowLoading()", true);
           

            Session["IsFirstLoad"] = "true";
            List<BIQTouchLabFields> list = new List<BIQTouchLabFields>();
            List<BIQTouchLabFields> listArv = new List<BIQTouchLabFields>();

            try
            {
               // Validation Function Code to be write here

               // Asigning value for saving
                BIQTouchLabFields objLabFields = new BIQTouchLabFields();
                objLabFields.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]);
                objLabFields.LocationId = Convert.ToInt32(Session["AppLocationId"].ToString());
                objLabFields.UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                objLabFields.OrderedByName = Convert.ToInt32(rcbOrderBy.SelectedValue.ToString());
                objLabFields.OrderedByDate = DateGiven(RadDateOrder.SelectedDate.ToString());
                objLabFields.IntFlag = 0;
                objLabFields.LabTestID = 0;
               
               

                if (dtVisitDate.DbSelectedDate != null)
                {
                    objLabFields.PreClinicLabDate = DateGiven(dtVisitDate.SelectedDate.ToString());
                }
                else
                {
                    objLabFields.PreClinicLabDate = DateGiven("");
                }

                objLabFields.LabOrderId = 0;
                objLabFields.TestResults="";
                objLabFields.TestResultId = 0;
                if (RadDateReportDate.DbSelectedDate != null)
                {
                    objLabFields.ReportedByDate = DateGiven(RadDateReportDate.SelectedDate.ToString());
                }
                else
                {
                    objLabFields.ReportedByDate = DateGiven("");
                }

               // objLabFields.ReportedByDate = DateGiven(RadDateReportDate.DbSelectedDate.ToString());
                if (rcbReportedBy.SelectedValue != "")
                {
                    objLabFields.ReportedByName = Convert.ToInt32(rcbReportedBy.SelectedValue.ToString());
                }
                else
                {
                    objLabFields.ReportedByName = 0;
                }


                foreach (GridDataItem item in RadGridLabTest.Items)
                {

                    Label lblLabSubTestID = (Label)item.FindControl("lblLabSubTestID");
                    Label lblLabTestID = (Label)item.FindControl("lblLabTestID");
                    BIQTouchLabFields objLabFields1 = new BIQTouchLabFields();

                    objLabFields1.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]);
                    objLabFields1.LocationId = Convert.ToInt32(Session["AppLocationId"].ToString());
                    objLabFields1.UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                    objLabFields1.OrderedByDate = DateGiven(RadDateOrder.SelectedDate.ToString());
                    objLabFields1.IntFlag = 1;
                    
                    objLabFields1.LabTestID = Convert.ToInt32(lblLabTestID.Text.ToString());
                    objLabFields1.SubTestID = Convert.ToInt32(lblLabSubTestID.Text.ToString());
                    objLabFields1.PreClinicLabDate = DateGiven("");
                    objLabFields1.ReportedByDate = DateGiven("");

                    //objLabFields1.ReportedByDate = DateGiven(RadDateReportDate.DbSelectedDate.ToString());
                    objLabFields1.ReportedByName = 0;
                    objLabFields1.Flag = "N";


                    objLabFields1.LabOrderId = 0;
                    objLabFields1.TestResults = "";
                    objLabFields1.TestResultId = 0;

                    list.Add(objLabFields1);
                }
                DataTable theDT = new DataTable();
                ILabFunctions theILabManager;
                theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
                int result = theILabManager.IQTouchSaveLabOrderTests(objLabFields, list, listArv, theDT);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                    LabSummaryPage();
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackBtnClick();", true);
                   
                }
               // Response.Redirect(Request.RawUrl);
             
            }
            catch (Exception ex)
            {
               

                IsError = true;

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('An errror occured please contact your Administrator')", true);
            }
            finally
            {
                if (IsError)
                {
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('An errror occured please contact your Administrator')", true);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "clsLoadingPanelDueToError", "parent.CloseLoading();", true);
                }
            }
        }

        protected void LabSummaryPage()
        {
            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmLaboratoryTouch";

            Touch.Custom_Forms.frmLaboratoryTouch fr = (frmLaboratoryTouch)mp.LoadControl("~/Touch/Custom Forms/frmLaboratoryTouch.ascx");

            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            frmLaboratoryTouch theFrm = (frmLaboratoryTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());
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
        #endregion
        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (ValidationFormData() == false)
            {
                return;
            }
            SaveLabOrder();
           // BtnBack_Click(sender, e);
        }
        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "printScript", "PrintTouchForm('Laboratory Form', '" + this.ID + "');", true);
        }
    }
}
