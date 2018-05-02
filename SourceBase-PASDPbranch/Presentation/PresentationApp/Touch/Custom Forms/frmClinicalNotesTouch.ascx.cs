using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Interface.Clinical;
using Application.Presentation;
using System.Data;
using Application.Common;

namespace Touch.Custom_Forms
{
    public partial class frmClinicalNotesTouch : TouchUserControlBase
    {
        static int patientID = 0;
        bool IsError = false;

        protected void Page_Load(object s, EventArgs e)
        {
            //register javascript script
            String script = frm_NCN_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            //if (IsPostBack) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);

            Session["CurrentForm"] = "frmClinicalNotesTouch";
            Session["FormIsLoaded"] = true;

            if (Session["IsFirstLoad"] != null)
            {
                if (Session["IsFirstLoad"].ToString() == "true")
                {
                    Session["IsFirstLoad"] = "false";
                    patientID = int.Parse(Request.QueryString["patientId"].ToString());
                    Init_Form();
                }
            }
            base.Page_Load(s, e);

            /***************** Check For User Rights ****************/
            AuthenticationManager Authentication = new AuthenticationManager();

            btnPrint.Visible = Authentication.HasFunctionRight(ApplicationAccess.PASDPNonVisitClinicalNote, FunctionAccess.Print, (DataTable)Session["UserRight"]);

            if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPNonVisitClinicalNote, FunctionAccess.Update, (DataTable)Session["UserRight"]))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showsave", "$('#divSave').hide();", true);
            }
        }

        protected void Init_Form()
        {
            IIQTouchClinicalNote ptnMgr = (IIQTouchClinicalNote)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchClinicalNote, BusinessProcess.Clinical");
            //DataTable dt = (DataTable)ptnMgr.GetClinicalNote(patientID.ToString(), "");
            DataTable dt = (DataTable)ptnMgr.GetClinicalNote(patientID.ToString(), Convert.ToString(Session["PatientVisitId"]));
            
            if (dt.Rows.Count > 0)
            {
                txtClinicalNote.Text = dt.Rows[0][1].ToString();
                DateTime CNDate; if (DateTime.TryParse(dt.Rows[0][0].ToString(), out CNDate))
                    dtClincalNoteDate.SelectedDate = CNDate;
            }
        }
        protected void btnSave_Click(object s, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Session["PatientVisitId"]) > 0)
                {
                    UpdateClinicalNote();
                }
                else 
                {
                SaveClinicalNote();
                }
            }
            catch (Exception ex)
            {
                IsError = true;
            }
            finally
            {
                if (IsError)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('An errror occured please contact your Administrator')", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "clsLoadingPanelDueToError", "parent.CloseLoading();", true);
                }
            }
            
        }
        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            updtFormUpdate.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "printScript", "PrintTouchForm('Registration Form', '" + this.ID + "');", true);
        }
        protected void SaveClinicalNote()
        {
            IIQTouchClinicalNote ptnMgr = (IIQTouchClinicalNote)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchClinicalNote, BusinessProcess.Clinical");
            int theRes = (int)ptnMgr.SaveClinicalnote(patientID.ToString(), dtClincalNoteDate.SelectedDate.ToString(), txtClinicalNote.Text, Session["AppLocationId"].ToString(), (Session["AppUserId"]).ToString());
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackWithoutDialog();", true);
        }

        protected void UpdateClinicalNote()
        {
            IIQTouchClinicalNote ptnMgr = (IIQTouchClinicalNote)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchClinicalNote, BusinessProcess.Clinical");
            int theRes = (int)ptnMgr.EditClinicalnote(patientID.ToString(), Session["PatientVisitId"].ToString(), dtClincalNoteDate.SelectedDate.ToString(), txtClinicalNote.Text, Session["AppLocationId"].ToString(), (Session["AppUserId"]).ToString());
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackWithoutDialog();", true);
        }
    }
}