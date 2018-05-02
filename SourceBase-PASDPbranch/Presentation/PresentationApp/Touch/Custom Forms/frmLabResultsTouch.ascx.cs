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
using System.Linq;



namespace Touch.Custom_Forms
{
    public partial class frmLabResultsTouch : TouchUserControlBase
    {
        Hashtable theHT = new Hashtable();
        static Boolean IsError = false;
        static Boolean IsRemove = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            String script = frmLabOrderResult_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            if (IsPostBack) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);
            Session["CurrentForm"] = "frmLabResultsTouch";
            Session["FormIsLoaded"] = true;
            BindAutoSelectLabTest("");
            if (Session["IsFirstLoad"].ToString() == "true")
            {
                // Code Here 
                Session["IsFirstLoad"] = "false";
                ViewState["theHT"] = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);
                BindPreSelectedLabOrder();
                LoadBlankGrid();
                BindEmpLoyee(rcbOrderBy);
                BindEmpLoyee(rcbReportedBy);
                BindLabTestGrid();
                BindLabOrderDetails();
            }
            base.Page_Load(sender, e);

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
        protected void BindLabTestGrid()
        {
            DataTable dt = GetDataTable("LabSubTestID", "", Convert.ToInt32(Session["LabOrderID"].ToString()));
            ViewState["tableLabSubTestID"] = dt;
            RadGridLabTest.DataSource = dt;
            RadGridLabTest.DataBind();
        }
        protected void BindLabOrderDetails()
        {
            DataTable dt = GetDataTable("LabOrderDetails", "", Convert.ToInt32(Session["LabOrderID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    rcbOrderBy.SelectedValue = row["OrderedbyName"].ToString();
                    if (row["ReportedbyName"].ToString() != "0")
                    {
                        rcbReportedBy.SelectedValue = row["ReportedbyName"].ToString();
                    }
                    RadDateOrder.DbSelectedDate = row["OrderedbyDate"].ToString();
                    RadDateReportDate.DbSelectedDate = row["ReportedbyDate"].ToString();
                    dtVisitDate.DbSelectedDate = row["PreClinicLabDate"].ToString();

                }
            }

            btnSave.Enabled = true;
            if (Session["LabOrderStatus"].ToString() == "Completed")
            {
                btnSave.Enabled = false;
            }

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
            DataSet Ds = theILabManager.IQTouchGetPatientLabTestID(objLabFields);
            DataTable dt = Ds.Tables[0];
            return dt;



        }
        protected void BindPreSelectedLabOrder()
        {

            DataTable dt = GetDataTable("LabTestID", "", Convert.ToInt32(Session["LabOrderID"].ToString()));
            foreach (DataRow row in dt.Rows)
            {
                string itemName = row["LabName"].ToString();
                string itemVal = row["LabTestID"].ToString();
                RadComboBoxItem item = new RadComboBoxItem(itemName, itemVal);
                if (Convert.ToInt32(row["OrderCount"].ToString()) > 0)
                {
                    item.Checked = true;
                    item.Enabled = false;
                }
                rcbPreSelectedLabTest.Items.Add(item);


            }
            //rcbPreSelectedLabTest.DataTextField = "LabName";
            //  rcbPreSelectedLabTest.DataValueField = "LabTestID";
            //   rcbPreSelectedLabTest.DataSource = dt;
            /// rcbPreSelectedLabTest.DataBind();









        }
        protected void BindAutoSelectLabTest(string inputval)
        {
            DataTable dt = GetDataTable("LabTestID", "", 0);
            AutoselectLabTest.DataTextField = "LabName";
            AutoselectLabTest.DataValueField = "LabTestID";
            AutoselectLabTest.DataSource = dt;
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

            if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            {

                RadGrid detsGrid = (RadGrid)((GridDataItem)e.Item).ChildItem.FindControl("RadGridLabResult");
                detsGrid.Visible =

                    !e.Item.Expanded;

                if (!e.Item.Expanded)
                    detsGrid.Rebind();

            }

        }

        protected void RadGridLabTest_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridNestedViewItem)
            {
                e.Item.FindControl("RadGridLabResult").Visible = ((GridNestedViewItem)e.Item).ParentItem.Expanded;
                // (e.Item.FindControl("RadGridLabResult") as RadGrid).NeedDataSource += new GridNeedDataSourceEventHandler(RadGridLabResult_NeedDataSource);


                RadGrid radGridLabResult = (RadGrid)e.Item.FindControl("RadGridLabResult");
                radGridLabResult.ItemCreated += new GridItemEventHandler(radGridLabResult_ItemCreated);
                radGridLabResult.ItemDataBound += new GridItemEventHandler(RadGridResut_ItemDataBound);


                //radGridLabResult.DataBind();

            }



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
        protected void RadGridLabResult_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Label lblID = (Label)parentItem.FindControl("lblID");
                //lblID
                if (IsRemove == false)
                {
                    DataTable dt = GetDataTable("QRY_CHILDGRID", lblID.Text, Convert.ToInt32(Session["LabOrderID"].ToString()));
                    (sender as RadGrid as RadGrid).DataSource = dt;//new Object[0];
                }
                else
                {
                    DataTable dt = GetDataTable("QRY_CHILDGRID", lblID.Text, Convert.ToInt32(Session["LabOrderID"].ToString()));
                    if (!object.Equals(ViewState["theHT"], ""))
                    {
                        dt.PrimaryKey = new DataColumn[] { dt.Columns["SubTestId"] };
                        foreach (KeyValuePair<string, string> value in (Dictionary<string, string>)ViewState["theHT"])
                        {
                            if (dt.Rows.Find(value.Key) != null && lblID.Text == value.Key)
                            {
                                DataRow dr = dt.Rows.Find(value.Key);
                                if (value.Value != "")
                                {
                                    dr["TestResults"] = value.Value;
                                    dr["TestResults1"] = value.Value;
                                    dt.AcceptChanges();
                                }
                            }
                        }
                    }
                    (sender as RadGrid as RadGrid).DataSource = dt;//new Object[0];
                }
                //(sender as RadGrid as RadGrid).DataBind();
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message.ToString();
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
            objLabFields.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());

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
                Label lblControlType = (Label)item.FindControl("lblControlType");
                Label lblUnitName = (Label)item.FindControl("lblUnitName");
                Label lblMinBoundaryVal = (Label)item.FindControl("lblMinBoundaryVal");
                Label lblMaxBoundaryVal = (Label)item.FindControl("lblMaxBoundaryVal");
                Label lblTestResultId = (Label)item.FindControl("lblTestResultId");
                Label lblTestResults = (Label)item.FindControl("lblTestResults");
                //Telerik.Web.UI.RadButton btnradRadioButtonList = (Telerik.Web.UI.RadButton)item.FindControl("btnRadRadiolist");
                RadioButtonList btnradRadioButtonList = (RadioButtonList)item.FindControl("btnRadRadiolist");
                Telerik.Web.UI.RadNumericTextBox txtRadValue = (Telerik.Web.UI.RadNumericTextBox)item.FindControl("txtRadValue");
                Telerik.Web.UI.RadTextBox txtAlphaRadValue = (Telerik.Web.UI.RadTextBox)item.FindControl("txtAlphaRadValue");
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
                    if (lblTestResultId.Text.ToString() != "")
                    {
                        if (Convert.ToInt32(lblTestResultId.Text.ToString()) > 0)
                        {
                            ddlList.SelectedValue = lblTestResultId.Text;
                        }
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
                    if (lblLabSubTestId.Text.ToString() == "17" || lblLabSubTestId.Text.ToString() == "18" || lblLabSubTestId.Text.ToString() == "19" || lblLabSubTestId.Text.ToString() == "131")
                    {
                        txthdnfield.Value = lblLabSubTestId.Text;
                        radGridArvMutation.Columns[0].HeaderText = "ABF";
                        radGridArvMutation.Columns[1].HeaderText = "GeneXpert";
                        radGridArvMutation.Columns[3].Visible = false;
                        radGridArvMutation.Columns[4].Visible = false;
                        DataTable dt = GetGenXpertGrid("GENXPERT", Convert.ToInt32(lblLabSubTestId.Text));
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["TblArvMutation"] = dt;
                            radGridArvMutation.DataSource = GetGenXpertGrid("GENXPERT", Convert.ToInt32(lblLabSubTestId.Text));
                            radGridArvMutation.DataBind();
                        }
                        else
                        {
                            radGridArvMutation.DataSource = new Object[0];
                            radGridArvMutation.DataBind();
                        }

                    }
                    else
                    {
                        txthdnfield.Value = lblLabSubTestId.Text;
                        radGridArvMutation.Columns[2].Visible = false;
                        //BindArvType
                        DataTable dt = GetArvMutationGrid("MUTATION_GRID");
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["TblArvMutation"] = dt;
                            radGridArvMutation.DataSource = GetArvMutationGrid("MUTATION_GRID");
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
                else
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
                        txthdnfield.Value = "115";
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
                if (txthdnfield.Value == "17" || txthdnfield.Value == "18" || txthdnfield.Value == "19" || txthdnfield.Value == "131")
                {
                    rcbFooterArvType.Items.Add(new RadComboBoxItem("Positive", "1"));
                    rcbFooterArvType.Items.Add(new RadComboBoxItem("Negative", "0"));
                }
                else if (txthdnfield.Value == "115")
                {
                    DataTable dt = GetArvMutationDataTable("ARV_TYPE");
                    rcbFooterArvType.DataSource = dt;
                    rcbFooterArvType.DataTextField = "ITEM_NAME";
                    rcbFooterArvType.DataValueField = "ID";
                    rcbFooterArvType.DataBind();
                }
                rcbFooterArvType.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(rcbFooterArvType_SelectedIndexChanged);
            }
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
        }
        //Jayant-Start
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
        //Jayant-End
        protected void BindChildGridDdl(Telerik.Web.UI.RadComboBox rcbComobo, string flag, string dataTextName, string dataValueName)
        {

            //DataTable dt = GetDataTable(flag, 0, "");
            //rcbComobo.DataTextField = dataTextName;
            //rcbComobo.DataValueField = dataValueName;
            //rcbComobo.DataSource = dt;
            //rcbComobo.DataBind();
            //rcbComobo.SelectedValue = "";
        }
        #endregion

        protected void BtnAddDrugClick(object sender, EventArgs e)
        {

            //var collection = rcbPreSelectedLabTest.CheckedItems;
            string labIdstr = SelectedLabTest();

            if (labIdstr != "")
            {
                DataTable dt = GetDataTable("LabSubTestID", labIdstr, 0);
                ViewState["tableLabSubTestID"] = dt;
                RadGridLabTest.DataSource = dt;
                RadGridLabTest.DataBind();

            }

        }
        protected string SelectedLabTest()
        {
            DataTable dt = GetDataTable("LabSubTestID", "", Convert.ToInt32(Session["LabOrderID"].ToString()));
            var collection = dt.Rows;
            var collectionnew = rcbPreSelectedLabTest.CheckedItems;
            string labIdstr = "";
            string commastr = "";
            if (collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    labIdstr = labIdstr + commastr + ((System.Data.DataRow)(item)).ItemArray[2];
                    commastr = ",";
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
            if (labIdstr == "")
            {
                labIdstr = labIdstr + AutoselectLabTest.Entries[0].Value;
            }
            else
            {
                labIdstr = labIdstr + "," + AutoselectLabTest.Entries[0].Value;
            }
            if (labIdstr != "")
            {
                DataTable dt = GetDataTable("LabSubTestID", labIdstr, 0);
                ViewState["tableLabSubTestID"] = dt;
                RadGridLabTest.DataSource = dt;
                RadGridLabTest.DataBind();

            }
            AutoselectLabTest.Entries.Clear();

        }

        protected void BtnBack_Click(object sender, EventArgs e)
        {
            //Response.Redirect("frmLaboratoryTouch.aspx?patientId=" + Request.QueryString["PatientID"].ToString());

            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmLaboratoryTouch";

            Touch.Custom_Forms.frmLaboratoryTouch fr = (frmLaboratoryTouch)mp.LoadControl("~/Touch/Custom Forms/frmLaboratoryTouch.ascx");
            Session["Orderid"] = 0;
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

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (ValidationFormData() == false)
            {
                return;

            }
            SaveLabResults();

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
            if (rcbReportedBy.SelectedValue == "")
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchReportedByName");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;

            }
            if (RadDateOrder.DbSelectedDate == null)
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchOrderDate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;

            }
            if (RadDateReportDate.DbSelectedDate == null)
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchReportedDate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;

            }

            if (Convert.ToDateTime(RadDateOrder.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
            {
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchLabOrderDate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;
            }

            if (Convert.ToDateTime(RadDateReportDate.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
            {
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchlabOrderReportedByDate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;
            }
            if (Convert.ToDateTime(RadDateOrder.SelectedDate.Value) > Convert.ToDateTime(RadDateReportDate.SelectedDate.Value))
            {
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchlabOrderDateCompReportedDate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;

            }
            // Check count Result Grid after user deleted Test ID
            DataTable table = (DataTable)ViewState["tableLabSubTestID"];



            var query = from inv in table.AsEnumerable()
                        where inv.Field<string>("DeleteFlag") == "N"
                        select new
                        {
                            DeleteFlag = inv["DeleteFlag"]
                        };
            Int32 totrec = Convert.ToInt32(query.Count());
            if (totrec == 0)
            {
                RawMessage theMsg = MsgRepository.GetMessage("IQTouchSubTestID");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                return false;
            }

            return true;

        }

        protected DateTime DateGiven(string dateVal)
        {
            DateTime dt = Convert.ToDateTime("01/01/1900");
            if (!string.IsNullOrEmpty(dateVal))
            {
                dt = DateTime.Parse(dateVal);
            }
            return dt;
        }

        #region -- Save Lab Order----
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
        protected void SaveLabResults()
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
                objLabFields.OrderedByDate = DateGiven(RadDateOrder.DbSelectedDate.ToString());
                objLabFields.IntFlag = 2;
                objLabFields.LabTestID = 0;

                if (dtVisitDate.DbSelectedDate != null)
                {
                    objLabFields.PreClinicLabDate = DateGiven(dtVisitDate.DbSelectedDate.ToString());
                }
                else
                {
                    objLabFields.PreClinicLabDate = DateGiven("");
                }
                objLabFields.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());
                objLabFields.TestResults = "";
                objLabFields.TestResultId = 0;
                objLabFields.ReportedByDate = DateGiven(RadDateReportDate.DbSelectedDate.ToString());
                objLabFields.ReportedByName = Convert.ToInt32(rcbReportedBy.SelectedValue.ToString());
                //For GenXpert
                DataTable dtGenXpert = new DataTable();
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
                        //Label lblLabSubTestId = (Label)item.FindControl("lblLabSubTestId");
                        //Label lblLabTestID = (Label)item.FindControl("lblLabTestID");

                        Label lblControlType = (Label)item.FindControl("lblControlType");
                        RadioButtonList btnradRadioButtonList = (RadioButtonList)item.FindControl("btnRadRadiolist");
                        Telerik.Web.UI.RadNumericTextBox txtRadValue = (Telerik.Web.UI.RadNumericTextBox)item.FindControl("txtRadValue");
                        Telerik.Web.UI.RadTextBox txtAlphaRadValue = (Telerik.Web.UI.RadTextBox)item.FindControl("txtAlphaRadValue");
                        CheckBoxList chkBoxList = (CheckBoxList)item.FindControl("chkBoxList");
                        DropDownList ddlList = (DropDownList)item.FindControl("ddlList");
                        BIQTouchLabFields objLabFields1 = new BIQTouchLabFields();
                        string strResuts = "";
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
                            if (lblLabSubTestId.Text == "115")
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
                        //if (lblControlType.Text != "GridView" || listArv.Count==0)
                        //{

                        objLabFields1.TestResults = strResuts;
                        objLabFields1.TestResultId = intRestutID;
                        objLabFields1.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]);
                        objLabFields1.LabTestIDs = "0";
                        objLabFields1.LabTestName = "";
                        objLabFields1.LocationId = Convert.ToInt32(Session["AppLocationId"].ToString());
                        objLabFields1.UserId = Convert.ToInt32(Session["AppUserId"].ToString());
                        objLabFields1.OrderedByName = Convert.ToInt32(rcbOrderBy.SelectedValue.ToString());
                        objLabFields1.OrderedByDate = DateGiven(RadDateOrder.DbSelectedDate.ToString());
                        objLabFields1.IntFlag = 3;
                        objLabFields1.LabTestID = 0;
                        objLabFields1.SubTestID = Convert.ToInt32(lblLabSubTestId.Text.ToString());
                        objLabFields1.PreClinicLabDate = DateGiven("");
                        objLabFields1.LabOrderId = Convert.ToInt32(Session["LabOrderID"].ToString());
                        objLabFields1.ReportedByDate = DateGiven(RadDateReportDate.DbSelectedDate.ToString());
                        objLabFields1.ReportedByName = Convert.ToInt32(rcbReportedBy.SelectedValue.ToString());
                        objLabFields1.Flag = GetSubTestIDDeleteFlag(lblLabSubTestId.Text.ToString());
                        //}
                        if (entryFlag == true)
                        {
                            list.Add(objLabFields1);
                        }
                    }

                }
                ILabFunctions theILabManager;
                theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
                if (list.Count > 0 || listArv.Count > 0 || dtGenXpert.Rows.Count > 0)
                {
                    int result = theILabManager.IQTouchSaveLabOrderTests(objLabFields, list, listArv, dtGenXpert);
                    if (result > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                        LabSummaryPage();
                        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackBtnClick();", true);
                    }
                }


                //Response.Redirect(Request.RawUrl);

            }
            catch (Exception ex)
            {
                //lblerr.Text = ex.Message.ToString();
                //lblerr.ForeColor = System.Drawing.Color.Red;
                //lblerr.Font.Bold = true;
                IsError = true;
            }
            finally
            {
                if (IsError)
                {
                    //Comment by Jayant - 20/5/2014
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

        protected void RadGridLabTest_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    Label lblDeleteFlag = (Label)item.FindControl("lblDeleteFlag");
                    RadButton btnRemove = (RadButton)item.FindControl("btnRemove");
                    if (lblDeleteFlag.Text.ToString() == "Y")
                    {
                        btnRemove.Enabled = false;
                        btnRemove.Text = "Deleted";
                        btnRemove.ForeColor = System.Drawing.Color.Gray;
                    }
                    else
                    {
                        btnRemove.Enabled = true;
                    }
                }
                //Jayant - 30-10-2014
                foreach (GridItem item in RadGridLabTest.MasterTableView.Items)
                {
                    Session["IsFirstLoad"] = true;
                    item.Expanded = true;
                }
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message.ToString();
            }

        }

        protected void RadGridLabTest_ItemDeleted(object sender, GridCommandEventArgs e)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                GridDataItem dataItm = e.Item as GridDataItem;
                Label lblID = (Label)dataItm.FindControl("lblID");
                string id = lblID.Text;
                String strResuts = "";
                if (Convert.ToString(ViewState["theHT"]) == "")
                {
                    foreach (GridNestedViewItem nestedView in RadGridLabTest.MasterTableView.GetItems(GridItemType.NestedView))
                    {
                        RadGrid radGridLabResult = (RadGrid)nestedView.FindControl("RadGridLabResult");
                        foreach (GridDataItem item in radGridLabResult.Items)
                        {
                            Label lblLabSubTestId = (Label)item.FindControl("lblLabSubTestId");
                            if (id != lblLabSubTestId.Text)
                            {
                                Telerik.Web.UI.RadNumericTextBox txtRadValue = (Telerik.Web.UI.RadNumericTextBox)item.FindControl("txtRadValue");
                                Telerik.Web.UI.RadTextBox txtAlphaRadValue = (Telerik.Web.UI.RadTextBox)item.FindControl("txtAlphaRadValue");
                                if (txtRadValue.Text != "0" && txtRadValue.Text != "")
                                {
                                    strResuts = txtRadValue.DisplayText.ToString();
                                }
                                else if (txtAlphaRadValue.Text != "")
                                {
                                    strResuts = txtAlphaRadValue.DisplayText.ToString();
                                }
                                data.Add(lblLabSubTestId.Text, strResuts);
                            }
                        }
                    }
                    ViewState["theHT"] = data;
                }

                DataTable table = (DataTable)ViewState["tableLabSubTestID"];
                table.PrimaryKey = new DataColumn[] { table.Columns["SubTestId"] };
                if (table.Rows.Find(id) != null)
                {
                    IsRemove = true;
                    DataRow dr = table.Rows.Find(id);
                    dr["DeleteFlag"] = "Y";
                    table.AcceptChanges();
                    ViewState["tableLabSubTestID"] = table;
                    RadGridLabTest.DataSource = table;
                    RadGridLabTest.DataBind();
                }
                else
                {
                    RadGridLabTest.DataSource = (DataTable)ViewState["tableLabSubTestID"];
                    RadGridLabTest.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message.ToString();
            }
        }

        protected void RadGridLabTest_PreRender(object sender, EventArgs e)
        {
            if (Session["IsFirstLoad"].ToString() == "false")
            {
                RadGridLabTest.Rebind();
                foreach (GridItem item in RadGridLabTest.MasterTableView.Items)
                {
                    Session["IsFirstLoad"] = true;
                    item.Expanded = true;
                }
            }
        }

        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "printScript", "PrintTouchForm('Laboratory Form', '" + this.ID + "');", true);
        }
    }
}
