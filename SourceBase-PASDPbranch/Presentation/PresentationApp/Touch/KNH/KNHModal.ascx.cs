using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

using Interface.Clinical;
using Application.Common;
using Application.Presentation;
using Interface.Security;
using Interface.Administration;
using Touch.Custom_Forms;

namespace Touch
{
    public partial class TestModal : System.Web.UI.UserControl
    {
        
        private static string temperatureLocalModal;
        private static string respirationRateLocalModal;
        private static string heartRateLocalModal;
        private static string systollicBloodPressureLocalModal;
        private static string diastolicBloodPressureLocalModal;
        private static string heightLocalModal;
        private static string weightLocalModal;
        
        #region property set for page
        public static string TemperatureModal
        {
            get
            {
                return temperatureLocalModal;
            }
            set
            {
                temperatureLocalModal = value;
            }
        }
        public static string RespirationRateModal
        {
            get
            {
                return respirationRateLocalModal;
            }
            set
            {
                respirationRateLocalModal = value;
            }
        }
        public static string HeartRateModal
        {
            get
            {
                return heartRateLocalModal;
            }
            set
            {
                heartRateLocalModal = value;
            }
        }
        public static string SystollicBloodPressureModal
        {
            get
            {
                return systollicBloodPressureLocalModal;
            }
            set
            {
                systollicBloodPressureLocalModal = value;
            }
        }
        public static string DiastolicBloodPressureModal
        {
            get
            {
                return diastolicBloodPressureLocalModal;
            }
            set
            {
                diastolicBloodPressureLocalModal = value;
            }
        }
        public static string HeightModal
        {
            get
            {
                return heightLocalModal;
            }
            set
            {
                heightLocalModal = value;
            }
        }
        public static string WeightModal
        {
            get
            {
                return weightLocalModal;
            }
            set
            {
                weightLocalModal = value;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {





            BindrcbmedicalCondition();// Presending history
            BindGrid();// Medical History
            Session["ModalLoad"] = "Load";
           


        }

        protected DataTable GetDataTable(string flag)
        {
            BIQTouchExpressFields objExpressFields = new BIQTouchExpressFields();
            objExpressFields.Flag = flag;
            objExpressFields.PtnPk = Convert.ToInt32(Session["PatientID"].ToString());
            objExpressFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
            objExpressFields.ID = 0;

            IQTouchKNHExpress theExpressManager;
            theExpressManager = (IQTouchKNHExpress)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchKNHExpress, BusinessProcess.Clinical");
            DataTable dt = theExpressManager.IQTouchGetKnhExpressData(objExpressFields);
            return dt;
        }

        #region VitalSign
        protected void btnClose_Click(object sender, EventArgs e)
        {
            TemperatureModal = txtRadTemperatureModal.Text;
            RespirationRateModal = txtRadRespirationRate.Text;
            HeartRateModal = txtRadHeartRate.Text;
            SystollicBloodPressureModal = txtRadSystollicBloodPressure.Text;
            DiastolicBloodPressureModal = txtRadDiastolicBloodPressure.Text;
            HeightModal = txtRadHeight.Text;
            WeightModal = txtRadWeight.Text;


            Page thePage = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)thePage.FindControl("phForms");

            //Touch.Custom_Forms.KNHAdultIEvaluationForm fr = (Touch.Custom_Forms.KNHAdultIEvaluationForm)Page.LoadControl("Custom forms/KNHAdultIEvaluationForm.ascx");
            //fr.ID = "ID" + Session["CurrentFormName"].ToString();
            // ph.Controls.Add(fr);
            Touch.Custom_Forms.KNHAdultIEvaluationForm theFrm = (Touch.Custom_Forms.KNHAdultIEvaluationForm)ph.FindControl("ID" + Session["CurrentFormName"].ToString());
            Touch.Custom_Forms.KNHAdultIEvaluationForm.Temperature = TemperatureModal;
            Touch.Custom_Forms.KNHAdultIEvaluationForm.RespirationRate = RespirationRateModal;
            Touch.Custom_Forms.KNHAdultIEvaluationForm.HeartRate = HeartRateModal;
            Touch.Custom_Forms.KNHAdultIEvaluationForm.SystollicBloodPressure = SystollicBloodPressureModal;
            Touch.Custom_Forms.KNHAdultIEvaluationForm.DiastolicBloodPressure = DiastolicBloodPressureModal;
            Touch.Custom_Forms.KNHAdultIEvaluationForm.Weight = WeightModal;
            Touch.Custom_Forms.KNHAdultIEvaluationForm.Height = HeightModal;


            if (theFrm != null)
            {
                theFrm.SetTextBox();
            }

            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseModalASPX('IDKNHAdultIEvaluationForm_TheModal_rwVital');", true);



        }
        #endregion
        #region Presenting Complaints
        protected void BindrcbmedicalCondition()
        {
            // ScriptManager.RegisterStartupScript(this, GetType(), "close", "setValues();", true);
            DataTable dt = GetDataTable("PresentingComplaints");
            RadGridPresenting.DataSource = dt;
            RadGridPresenting.DataBind();
            //dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
            //if (Session["PCValue"] != null)
            //{
            //    string[] pcRow = Session["PCValue"].ToString().Split('#');
            //    foreach (string val in pcRow)
            //    {
            //        string[] pcCellvalue = val.Split(',');
            //        if (dt.Rows.Find(pcCellvalue[0]) != null)
            //        {
            //            DataRow dr = dt.Rows.Find(pcCellvalue[0]);
            //            dr["ChkVal"] = "1";
            //            dr["ChkValText"] = pcCellvalue[2].ToString();
            //        }
            //    }

            //}

        }
        protected void RadGridPresenting_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                Label lblchkval = (Label)item.FindControl("lblchkval");
                CheckBox chkPresenting = (CheckBox)item.FindControl("ChkPresenting");
                if (lblchkval.Text == "1")
                {
                    chkPresenting.Checked = true;
                }


            }
            //{
            //    
            //    Label lblLabId = (Label)item.FindControl("lblLabId");
            //    Telerik.Web.UI.RadComboBox rcbAbf = (Telerik.Web.UI.RadComboBox)item.FindControl("rcbAbf");
            //    Telerik.Web.UI.RadComboBox rcbGenXpert = (Telerik.Web.UI.RadComboBox)item.FindControl("rcbGenXpert");

        }
        public DataTable CreatePresentingPCTable()
        {
            DataTable dtlPc = new DataTable();
            dtlPc.Columns.Add("ID", typeof(string));
            dtlPc.Columns.Add("NAME", typeof(string));
            dtlPc.Columns.Add("chkValText", typeof(string));
            dtlPc.PrimaryKey = new DataColumn[] { dtlPc.Columns["ID"] };
            return dtlPc;

        }

        protected void BtnClosePCClick(object sender, EventArgs e)
        {
            DataTable dt = CreatePresentingPCTable();
          

            foreach (GridDataItem item in RadGridPresenting.Items)
            {

                Label lblPresenting = (Label)item.FindControl("lblPresenting");
                CheckBox chkPresenting = (CheckBox)item.FindControl("ChkPresenting");
                RadTextBox txtPresenting = (RadTextBox)item.FindControl("txtPresenting");

                if (chkPresenting.Checked == true)
                {
                    DataRow DR = dt.NewRow();
                    DR["ID"] = lblPresenting.Text;
                    DR["NAME"] = chkPresenting.Text;
                    DR["chkValText"] = txtPresenting.Text;
                    dt.Rows.Add(DR);
                }
            }

            Page thePage = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)thePage.FindControl("phForms");
            Touch.Custom_Forms.KNHAdultIEvaluationForm.DtPresenting = dt;
            Touch.Custom_Forms.KNHAdultIEvaluationForm.AditionalComplaints = txtAdditionPresentingComplaints.Text;
            
            Touch.Custom_Forms.KNHAdultIEvaluationForm theFrm = (Touch.Custom_Forms.KNHAdultIEvaluationForm)ph.FindControl("ID" + Session["CurrentFormName"].ToString());

            if (theFrm != null)
            {
                theFrm.BindPresentingComp();
            }
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseModalASPX('IDKNHAdultIEvaluationForm_TheModal_rwPresentingComplaints');", true);
             
            
            
        }

        #endregion
        #region Medical History
        protected void RadGridSection_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            {

                ((GridDataItem)e.Item).ChildItem.FindControl("RadGridFieldName").Visible = !e.Item.Expanded;

            }
            


        }
        public DataTable CreateMedicalHistoryTable()
        {
            DataTable dtlMH = new DataTable();
            dtlMH.Columns.Add("SectionID", typeof(string));
            dtlMH.Columns.Add("FieldID", typeof(string));
            dtlMH.Columns.Add("FieldValue", typeof(string));
            dtlMH.PrimaryKey = new DataColumn[] { dtlMH.Columns["FieldID"] };
            return dtlMH;

        }
        protected void RadGridSection_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridNestedViewItem)
            {

                e.Item.FindControl("RadGridFieldName").Visible = ((GridNestedViewItem)e.Item).ParentItem.Expanded;
                RadGrid radGridFieldName = (RadGrid)e.Item.FindControl("RadGridFieldName");
               // radGridFieldName.NeedDataSource += new GridNeedDataSourceEventHandler(RadGridFieldName_NeedDataSource);
                radGridFieldName.ItemDataBound += new GridItemEventHandler(RadGridFieldName_ItemDataBound);
                radGridFieldName.ItemCreated += new GridItemEventHandler(RadGridFieldName_ItemCreated);

            }

        }
        protected void RadGridFieldName_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {


            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "ChildGrid")
            {
                GridDataItem item = (GridDataItem)e.Item;
                Label lblControlFlag = (Label)item.FindControl("lblControlFlag");


                //Telerik.Web.UI.RadButton btnradRadioButtonList = (Telerik.Web.UI.RadButton)item.FindControl("btnRadRadiolist");
                RadButton rbtnYesNo = (RadButton)item.FindControl("rbtnYesNo");
                Telerik.Web.UI.RadTextBox txtRadText = (Telerik.Web.UI.RadTextBox)item.FindControl("txtRadText");
                RadDatePicker dtDateValue = (RadDatePicker)item.FindControl("dtDateValue");

                rbtnYesNo.Visible = false;
                txtRadText.Visible = false;
                dtDateValue.Visible = false;

                if (lblControlFlag.Text == "Text SingleLine")
                {
                    txtRadText.Visible = true;

                }

                else if (lblControlFlag.Text == "Date")
                {

                    dtDateValue.Visible = true;
                }





            }

        }
        protected void RadGridFieldName_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
            }
        }
        protected void BindGrid()
        {
            if (RadGridSection.Items.Count == 0)
            {
                DataTable dt = GetDataTable("SectionName");

                RadGridSection.DataSource = dt;
                RadGridSection.DataBind();
            }

        }
        protected void RadGridFieldName_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;

                Label lblSectionID = (Label)parentItem.FindControl("lblSectionID");
                //lblID
                DataTable dt = GetSectionFieldName("SectionFieldDetails", Convert.ToInt32(lblSectionID.Text.ToString()));
                (sender as RadGrid as RadGrid).DataSource = dt;//new Object[0];
                //(sender as RadGrid as RadGrid).DataBind();


                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

            }
            catch (Exception ex)
            {
                //lblerr.Text = ex.Message.ToString();


            }
        }
        protected DataTable GetSectionFieldName(string flag, int sectionID)
        {
            BIQTouchAdultIE objAdultIEFields = new BIQTouchAdultIE();
            objAdultIEFields.Flag = flag;
            objAdultIEFields.PtnPk = Convert.ToInt32(Session["PatientID"].ToString());
            objAdultIEFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
            objAdultIEFields.ID = sectionID;

            IQTouchKNHAdultIE theExpressManager;
            theExpressManager = (IQTouchKNHAdultIE)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchKNHAdultIE, BusinessProcess.Clinical");
            DataTable dt = theExpressManager.IQTouchGetKnhAdultIEData(objAdultIEFields);
            return dt;
        }
        #endregion





    }
}