using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//IQCare Libs
using Application.Presentation;
using Application.Common;
using Interface.Laboratory;

//Third party Libs
using Telerik.Web.UI;
using System.Data;
using System.IO;

namespace PresentationApp.Laboratory
{
    public partial class frm_LaboratoryOrdered : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindLabOrder();
        }

        protected void grdSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                Label lblOrdDate = new Label();
                lblOrdDate.ID = "lblOrdDate";                
                lblOrdDate.Text = (e.Row.DataItem as DataRowView).Row["LabOrderDateformatted"].ToString();
                e.Row.Cells[0].Controls.Add(lblOrdDate);

                if ((e.Row.DataItem as DataRowView).Row["Result_status"].ToString().ToUpper() == "PENDING")
                {
                    LinkButton lnkView = new LinkButton();
                    lnkView.ID = "lnkEdit";
                    lnkView.Text = "Edit";
                    //lnkView.Click += ViewDetails;
                    lnkView.CommandArgument = (e.Row.DataItem as DataRowView).Row["LabOrderID"].ToString();
                    e.Row.Cells[5].Controls.Add(lnkView);

                    LinkButton lnkAction = new LinkButton();
                    lnkAction.ID = "lnkAction";
                    lnkAction.Text = "Report Results";
                    //lnkAction.Click += ViewDetails;
                    lnkAction.CommandArgument = (e.Row.DataItem as DataRowView).Row["LabOrderID"].ToString();
                    e.Row.Cells[6].Controls.Add(lnkAction);
                }
            }
        }

        protected void grdSearchResult_Sorting(object sender, GridViewSortEventArgs e)
        {

        }
        protected void BindLabOrder()
        {
            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            objLabFields.Flag = "";
            objLabFields.Ptnpk = Convert.ToInt32(Session["PatientID"]);
            objLabFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());


            DataSet Ds = theILabManager.IQTouchLaboratory_GetLabOrder(objLabFields);            
            this.grdSearchResult.DataSource = Ds.Tables[0];
            BindGrid();

        }
        private void BindGrid()
        {
            TemplateField tfield = new TemplateField();
            tfield.HeaderText = "Date";
            tfield.ItemStyle.Width = Unit.Percentage(5);
            grdSearchResult.Columns.Add(tfield);
            //BoundField theCol1 = new BoundField();
            //theCol1.HeaderText = "Date";
            //theCol1.DataField = "LabOrderDate";
            //theCol1.DataFormatString = "dd-MMM-yyyy";
            //theCol1.ItemStyle.Width = Unit.Percentage(5);
            //grdSearchResult.Columns.Add(theCol1);

            BoundField theCol0 = new BoundField();
            theCol0.HeaderText = "OrderId";
            theCol0.DataField = "LabOrderID";
            theCol0.ItemStyle.Width = Unit.Percentage(3);
            theCol0.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdSearchResult.Columns.Add(theCol0);

            BoundField theCol2 = new BoundField();
            theCol2.HeaderText = "Area";
            theCol2.DataField = "Location_name";            
            theCol2.ItemStyle.Width = Unit.Percentage(5);
            grdSearchResult.Columns.Add(theCol2);

            BoundField theCol3 = new BoundField();
            theCol3.HeaderText = "Ordered By";
            theCol3.DataField = "OrderedBy";            
            theCol3.ItemStyle.Width = Unit.Percentage(5);
            grdSearchResult.Columns.Add(theCol3);

            BoundField theCol4 = new BoundField();
            theCol4.HeaderText = "Result Status";
            theCol4.DataField = "Result_status"; 
            theCol4.ItemStyle.Width = Unit.Percentage(5);
            grdSearchResult.Columns.Add(theCol4);            

            tfield = new TemplateField();
            tfield.HeaderText = "Edit";
            tfield.ItemStyle.Width = Unit.Percentage(5);
            grdSearchResult.Columns.Add(tfield);
            

            tfield = new TemplateField();
            tfield.HeaderText = "Next Action";
            tfield.ItemStyle.Width = Unit.Percentage(5);
            grdSearchResult.Columns.Add(tfield);
            grdSearchResult.DataBind();
            
           
        }
    }
}