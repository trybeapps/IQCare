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
    public partial class frmPharmacyOrderManagementTouch : TouchUserControlBase
    {
        DataTable table = new DataTable();
        DataSet dsHistory = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);


            Session["CurrentForm"] = this;
            Session["FormIsLoaded"] = true;
            if (Session["IsFirstLoad"] == "true")
            {
                BindPharmacyHistoryData();
                Session["IsFirstLoad"] = "false";
            }
        }
        public void BindPharmacyHistoryData()
        {
            IPediatric PediatricManager;
            PediatricManager = (IPediatric)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BPediatric, BusinessProcess.Pharmacy");
            dsHistory = PediatricManager.IQTouchGetPharmacyDetails(Convert.ToInt32(Request.QueryString["PatientID"]));
            rgviewpharmacyform.DataSource = dsHistory.Tables[0];
        }
        protected void rgviewpharmacyform_ItemCommand(object sender, GridCommandEventArgs e)
        {

            GridDataItem item1 = (GridDataItem)e.Item;
            string strOrderID = item1.GetDataKeyValue("OrderID").ToString();
            string straction = item1["ShowAction"].Text;
            Session["Action"] = straction;
            string strrefill = item1["NextAction"].Text;
            Session["Refill"] = strrefill;
            Session["Visit_id"] = item1["visitid"].Text.ToString();

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "loadpharm", "parent.ShowLoading()", true);
            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmPharmacyTouch";

            Touch.Custom_Forms.frmPharmacyTouch fr = (frmPharmacyTouch)mp.LoadControl("~/Touch/Custom Forms/frmPharmacyTouch.ascx");
            if (((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text == "Dispense")
            {
                Session["Orderid"] = Convert.ToInt32(strOrderID);
                Session["Mode"] = "Dispense";
            }
            else
            {
                Session["Orderid"] = Convert.ToInt32(strOrderID);
                Session["Mode"] = "Edit";
            }
            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            frmVisitTouch theFrm = (frmVisitTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());

            // Handle Save button
            if (Session["Action"].ToString() != "View")
            {
                HiddenField Phf = (HiddenField)mp.FindControl("hdSaveBtnVal");
                UpdatePanel updt = (UpdatePanel)mp.FindControl("updtPatientSave");
                Phf.Value = fr.ID + "_btnSave_input";
                updt.Update();
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);
            }
          

            // END 
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
            //ph.DataBind();
            upt.Update();
            mp.ClientScript.RegisterStartupScript(mp.GetType(), "settabschild", "setTabs();");


        }
        protected void rdneworder_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "loadpharm", "parent.ShowLoading()", true);
            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmPharmacyTouch";
            Session["Refill"] = "0";
            Session["Visit_id"] = "0";
            Touch.Custom_Forms.frmPharmacyTouch fr = (frmPharmacyTouch)mp.LoadControl("~/Touch/Custom Forms/frmPharmacyTouch.ascx");
            Session["Orderid"] = 0;
            Session["Mode"] = "newARV";
            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            frmVisitTouch theFrm = (frmVisitTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());

            // Handle Save button
            HiddenField Phf = (HiddenField)mp.FindControl("hdSaveBtnVal");
            UpdatePanel updt = (UpdatePanel)mp.FindControl("updtPatientSave");
            Phf.Value = fr.ID + "_btnSave_input"; 
            updt.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

            // END 

            foreach (Control item in ph.Controls)
            {
                ph.Controls.Remove(item);
                //item.Visible = false;
                //if (item.ID == "ID" + Session["CurrentFormName"].ToString())
                //{
                //    item.Visible = true;
                //}
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
            //ph.DataBind();
            upt.Update();
            mp.ClientScript.RegisterStartupScript(mp.GetType(), "settabschild", "setTabs();");
        }

        protected void rdcontinueorder_Click(object sender, EventArgs e)
        {
            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmPharmacyTouch";
            Session["Refill"] = "0";
            Session["Visit_id"] = "0";
            Touch.Custom_Forms.frmPharmacyTouch fr = (frmPharmacyTouch)mp.LoadControl("~/Touch/Custom Forms/frmPharmacyTouch.ascx");
            Session["Orderid"] = 0;
            Session["Mode"] = "ContARV";
            fr.ID = "ID" + Session["CurrentFormName"].ToString();
            frmVisitTouch theFrm = (frmVisitTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());

            // Handle Save button
            HiddenField Phf = (HiddenField)mp.FindControl("hdSaveBtnVal");
            UpdatePanel updt = (UpdatePanel)mp.FindControl("updtPatientSave");
            Phf.Value = fr.ID + "_btnSaveContinue_input";
            updt.Update();
            RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);
            // END 

            foreach (Control item in ph.Controls)
            {
                ph.Controls.Remove(item);
            }

            if (theFrm != null)
            {
                theFrm.Visible = true;
            }
            else
            {
                ph.Controls.Add(fr);
            }
            upt.Update();
            mp.ClientScript.RegisterStartupScript(mp.GetType(), "settabschild", "setTabs();");
        }

        protected void rgviewpharmacyform_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                string strRefill = item["NoRefills"].Text;
                string strNextAction = item["NextAction"].Text;
                LinkButton btnNextAction = (LinkButton)item["NextAction"].Controls[0];
                LinkButton btnEditPharmacy = (LinkButton)item["EditPharmacy"].Controls[0];
                btnNextAction.Attributes.Add("onclick", "document.getElementById('IDfrmPharmacyOrderManagementTouch_btnInvisShowLoading').click()");
                btnEditPharmacy.Attributes.Add("onclick", "document.getElementById('IDfrmPharmacyOrderManagementTouch_btnInvisShowLoading').click()");
                //if (item["RefillExpiration"].Text == "01-Jan-1900")
                //{
                //    item["RefillExpiration"].Text = " ";
                //}
            }

        }
    }
}