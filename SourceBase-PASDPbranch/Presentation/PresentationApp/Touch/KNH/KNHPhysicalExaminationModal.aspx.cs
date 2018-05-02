using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Application.Presentation;
using Application.Common;
using Interface.Clinical;
//Third party Libs
using Telerik.Web.UI;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PresentationApp.Touch.KNH
{
    public partial class KNHPhysicalExaminationModal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init_Form();
                //Response.Write(Request.QueryString["flagMode"]);


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
        protected void Init_Form()
        {
            BindGrid();

        }


        protected void BindGrid()
        {


            DataTable dt = GetSectionFieldName("SectionNamePhyExam", 0);
            RadGridSection.DataSource = dt;
            RadGridSection.DataBind();


        }
        #region GridEvents

        protected void RadGridSection_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridNestedViewItem)
            {

                e.Item.FindControl("RadGridFieldName").Visible = ((GridNestedViewItem)e.Item).ParentItem.Expanded;
                RadGrid radGridFieldName = (RadGrid)e.Item.FindControl("RadGridFieldName");
                //radGridFieldName.NeedDataSource += new GridNeedDataSourceEventHandler(RadGridFieldName_NeedDataSource);
                radGridFieldName.ItemDataBound += new GridItemEventHandler(RadGridFieldName_ItemDataBound);
                radGridFieldName.ItemCreated += new GridItemEventHandler(RadGridFieldName_ItemCreated);







            }
        }

        protected void RadGridSection_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            {

                ((GridDataItem)e.Item).ChildItem.FindControl("RadGridFieldName").Visible = !e.Item.Expanded;

            }
        }
        protected void RadGridFieldName_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
        }
        protected void RadGridFieldName_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {


            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "ChildGrid")
            {
                GridDataItem item = (GridDataItem)e.Item;
                Label lblConditionName = (Label)item.FindControl("lblConditionName");
                Telerik.Web.UI.RadTextBox txtRemakrs = (Telerik.Web.UI.RadTextBox)item.FindControl("txtRemakrs");
                txtRemakrs.Visible = false;
                if (lblConditionName.Text.ToString().ToUpper() == "OTHER")
                {
                    txtRemakrs.Visible = true;

                }
            }

               




            //}

        }
        protected void RadGridFieldName_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;

                Label lblCodeID = (Label)parentItem.FindControl("lblCodeID");
                //lblID
                DataTable dt = GetSectionFieldName("SectionFieldDetailsPhyExam", Convert.ToInt32(lblCodeID.Text.ToString()));
                (sender as RadGrid as RadGrid).DataSource = dt;//new Object[0];
                //(sender as RadGrid as RadGrid).DataBind();
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);

            }
            catch (Exception ex)
            {
                //lblerr.Text = ex.Message.ToString();


            }
        }




        #endregion
    }
}