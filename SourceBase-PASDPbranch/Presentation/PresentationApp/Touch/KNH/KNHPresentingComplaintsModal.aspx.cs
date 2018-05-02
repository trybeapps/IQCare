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

namespace Touch.Custom_Forms
{
    public partial class KNHPresentingComplaintsModal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 Init_Form();
                //Response.Write(Request.QueryString["flagMode"]);


            }
        }
        private void Init_Form()
        {
            BindrcbmedicalCondition();
        }
        protected void BindrcbmedicalCondition()
        {
           // ScriptManager.RegisterStartupScript(this, GetType(), "close", "setValues();", true);
            DataTable dt = GetDataTable("PresentingComplaints");
            
            dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
            if (Session["PCValue"] != null)
            {
                string[] pcRow = Session["PCValue"].ToString().Split('#');
                foreach (string val in pcRow)
                {
                    string[] pcCellvalue = val.Split(',');
                    if (dt.Rows.Find(pcCellvalue[0]) != null)
                    {
                        DataRow dr = dt.Rows.Find(pcCellvalue[0]);
                        dr["ChkVal"] = "1";
                        dr["ChkValText"] = pcCellvalue[2].ToString();
                    }
                }
                
            }


            RadGridPresenting.DataSource = dt;
            RadGridPresenting.DataBind();
            //foreach (DataRow row in dt.Rows)
            //{
            //    string itemName = row["Name"].ToString();
            //    string itemVal = row["ID"].ToString();
            //    RadComboBoxItem item = new RadComboBoxItem(itemName, itemVal);
            //    //if (Convert.ToInt32(row["CheckedVal"]) > 0)
            //    //{
            //    //    item.Checked = true;
            //    //}

            //    rcbPresentingComplaints.Items.Add(item);
            //}
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
        protected void CallJavaScript()
        {
            string Separator = "";
            string pcValue = "";


            foreach (GridDataItem item in RadGridPresenting.Items)
            {

                Label lblPresenting = (Label)item.FindControl("lblPresenting");
                CheckBox chkPresenting = (CheckBox)item.FindControl("ChkPresenting");
                RadTextBox txtPresenting = (RadTextBox)item.FindControl("txtPresenting");

                if (chkPresenting.Checked == true)
                {
                    pcValue = pcValue + Separator + lblPresenting.Text + "," + chkPresenting.Text + "," + txtPresenting.Text;
                    Separator = "#";
                }


            }
            Session["PCValue"] = pcValue;
            hiddPcValueModal.Value = pcValue;
            ScriptManager.RegisterStartupScript(this, GetType(), "close", "returnToParent();", true);

            //string csName = "PopupScript";
            //Type csType = this.GetType();
            //ClientScriptManager csm = Page.ClientScript;

            //if (!csm.IsStartupScriptRegistered(csType, csName))
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("<script>");
            //    sb.Append("var oArg = new Object();");
            //    sb.Append("oArg.flagmode = 'PC';");
            //    sb.Append("oWnd.close(oArg);");
            //   // sb.Append("}");
            //    sb.Append("</script>");


            //    csm.RegisterStartupScript(csType, csName, sb.ToString());
            //}


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CallJavaScript();
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
    }
}