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
    public partial class frmLaboratoryTouch : TouchUserControlBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            Session["CurrentForm"] = this;
            Session["FormIsLoaded"] = true;
            if (Session["IsFirstLoad"].ToString() == "true")
            {
              // Code Here 
                
                Session["CurrentForm"] = "frmLaboratoryTouch";
                Session["IsFirstLoad"] = "Load";
            }
            BindLabOrder();
        }
        //protected DataTable GetDataTable(string flag, Int32 labId, string LabName)
        //{
        //    BIQTouchLabFields objLabFields = new BIQTouchLabFields();
        //    objLabFields.Flag = flag;
        //    objLabFields.LabTestID = labId;
        //    objLabFields.LabTestName = LabName;
        //    ILabFunctions theILabManager;
        //    theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
        //    DataSet Ds = theILabManager.IQTouchGetlabDemo(objLabFields);
        //    DataTable dt = Ds.Tables[0];
        //    return dt;
        //}
        protected void BindLabOrder()
        {
            BIQTouchLabFields objLabFields = new BIQTouchLabFields();
            ILabFunctions theILabManager;
            theILabManager = (ILabFunctions)ObjectFactory.CreateInstance("BusinessProcess.Laboratory.BLabFunctions, BusinessProcess.Laboratory");
            objLabFields.Flag = "";
            objLabFields.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]);
            objLabFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());


            DataSet Ds = theILabManager.IQTouchLaboratory_GetLabOrder(objLabFields);
                        
            DataTable dtOrder = Ds.Tables[0];
            RadGridLabOrder.DataSource = dtOrder;
            RadGridLabOrder.DataBind();
            if (RadGridLabOrder.Items.Count == 0)
            {
                RadGridLabOrder.DataSource = new Object[0];
            }

        }

        protected void BtnNewOrderClick(object sender, EventArgs e)
        {

            Session["IsFirstLoad"] = "true";
            Page mp = (Page)this.Parent.Page;
            PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
            UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

            Session["CurrentFormName"] = "frmLabOrderTouch";

            Touch.Custom_Forms.frmLabOrderTouch fr = (frmLabOrderTouch)mp.LoadControl("~/Touch/Custom Forms/frmLabOrderTouch.ascx");

            fr.ID = "ID" + Session["CurrentFormName"].ToString(); 
            frmLabOrderTouch theFrm = (frmLabOrderTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());


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

        protected void RadGridLabOrder_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            this.RadGridLabOrder.CurrentPageIndex=e.NewPageIndex;
            BindLabOrder();

        }

        protected void RadGridLabOrder_ItemDataBound(object sender, GridItemEventArgs e)
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton btn = (LinkButton)item["ResultsButton"].Controls[0];
                btn.Attributes.Add("onclick", "document.getElementById('IDfrmLaboratoryTouch_btnInvisShowLoading').click()");
            }
           // if (e.Item is GridDataItem)
           // {
           //     GridDataItem item = (GridDataItem)e.Item;
           //     string strLabOrderID = item.GetDataKeyValue("LabOrderID").ToString();
           //     Session["LabOrderID"] = strLabOrderID;



           //}
            
        }

        

        protected void RadGridLabOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataItem = RadGridLabOrder.SelectedItems[0] as GridDataItem;
             if (dataItem != null)  
             {

                 Session["IsFirstLoad"] = "true";
                 Page mp = (Page)this.Parent.Page;
                 string strLabOrderID = dataItem.GetDataKeyValue("LabOrderID").ToString();
                 string strStatus = ((Label)dataItem.FindControl("lblstatus")).Text.ToString();
                 Session["LabOrderStatus"] = strStatus;
                 Session["LabOrderID"] = strLabOrderID;

                 PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
                 UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");

                 Session["CurrentFormName"] = "frmLabResultsTouch";

                 Touch.Custom_Forms.frmLabResultsTouch fr = (frmLabResultsTouch)mp.LoadControl("~/Touch/Custom Forms/frmLabResultsTouch.ascx");
                 Session["Orderid"] = 0;
                 fr.ID = "ID" + Session["CurrentFormName"].ToString();
                 frmLabResultsTouch theFrm = (frmLabResultsTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());


                 // Handle Save button
                 HiddenField Phf = (HiddenField)mp.FindControl("hdSaveBtnVal");
                 UpdatePanel updt = (UpdatePanel)mp.FindControl("updtPatientSave");
                 Phf.Value = fr.ID + "_btnSave_input";
                 updt.Update();

                 if (Session["LabOrderStatus"].ToString() == "Completed")
                 {

                     RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'none');", true);
                 }
                 else
                 {
                     RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);
                 }
                 // END 


                 foreach (Control item in ph.Controls)
                 {
                     
                     ph.Controls.Remove(item);
                     //item.Visible = false;
                     //if (item.ID == "ID" + Session["CurrentFormName"].ToString())
                     //{
                     //    item.Visible = true;
                     //}
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


        }

        protected void btnNewOrderCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
       
    }

}