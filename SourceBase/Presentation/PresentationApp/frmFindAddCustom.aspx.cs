using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Interface.Clinical;
using Application.Presentation;
using System.Data;

namespace IQCare.Web
{
    public partial class frmFindAddCustom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AppLocation"] == null || Session.Count == 0 || Session["AppUserID"].ToString() == "")
            {
                IQCareMsgBox.Show("SessionExpired", this);
                Response.Redirect("~/frmlogin.aspx",true);
            }
            if (IsPostBack) return;

            HtmlGenericControl lblServiceArea = (HtmlGenericControl)FindPatient.FindControl("lblServiceArea");
            lblServiceArea.InnerText = Request.QueryString["srvNm"];


            Session["HIVPatientStatus"] = 0;
            Session["PMTCTPatientStatus"] = 0;
            //SetEnrollmentCombo();
            Session["PatientId"] = 0;
            Session["TechnicalAreaName"] = Request.QueryString["srvNm"];
            Session["TechnicalAreaId"] = Request.QueryString["mod"];


        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Request.QueryString["mod"] == "0")
            {

                // FindPatient.Attributes.Add("IncludeEnrollement", "True");
                FindPatient.IncludeEnrollement = true;
            }
            else
            {
                FindPatient.IncludeEnrollement = false;
                if(Request.QueryString["mod"] !="")
                    FindPatient.SelectedServiceLine = int.Parse(Request.QueryString["mod"]);
            }
            FindPatient.SelectedPatientChanged += new CommandEventHandler(FindPatient_SelectedPatientChanged);

        }
        void FindPatient_SelectedPatientChanged(object sender, CommandEventArgs e)
        {
            int LocationID, PatientID;

            List<KeyValuePair<string, int>> param = e.CommandArgument as List<KeyValuePair<string, int>>;
            PatientID = param.Find(l => l.Key == "PatientID").Value;
            LocationID = param.Find(l => l.Key == "LocationID").Value;

            if (LocationID == Convert.ToInt32(base.Session["AppLocationId"]))
            {
                openPatientDetails(PatientID);
            }
            else
            {
                string script = "alert('This Patient belongs to a different Location. Please log-in with the patient\\'s location.'); return false;";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FindPatientAlert", script, true);
            }
        }

        private void openPatientDetails(int patientID)
        {

            HttpContext.Current.Session["PatientId"] = patientID;
            HttpContext.Current.Session["PatientVisitId"] = 0;
            HttpContext.Current.Session["ServiceLocationId"] = 0;
            HttpContext.Current.Session["LabId"] = 0;
            /* Session["TechnicalAreaName"] = null;
             Session["TechnicalAreaId"] = 0;*/


            #region "Refresh Patient Records"
            IPatientHome PManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
            System.Data.DataSet thePDS = PManager.GetPatientDetails(Convert.ToInt32(HttpContext.Current.Session["PatientId"]), Convert.ToInt32(HttpContext.Current.Session["SystemId"]), Convert.ToInt32(HttpContext.Current.Session["TechnicalAreaId"]));
            

            HttpContext.Current.Session["PatientInformation"] = thePDS.Tables[0];
            #endregion
            string theUrl = "";
            if (Request.QueryString["srvNm"] == "Records")
            {
                Session["TechnicalAreaName"] = "Records";
                theUrl = string.Format("{0}", "./frmAddTechnicalArea.aspx");
                Response.Redirect(theUrl);
            }
            else
            {
                //Check if the patient is enrolled and go directly to patient home page if patient is enrolled
                IPatientRegistration PatRegMgr = (IPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientRegistration, BusinessProcess.Clinical");
                DataSet theDS = PatRegMgr.GetFieldNames(int.Parse(Request.QueryString["mod"]), Convert.ToInt32(Session["PatientId"]));
                if (theDS.Tables[3].Rows.Count > 0)//check if patient is care ended for reenrollment
                {
                    //theUrl = string.Format("{0}?PatientId={1}&mod={2}", "./frmAddTechnicalArea.aspx", patientID, Request.QueryString["mod"]);
                    /*
                     * Code is commented for Care end patient, when patient is under Care End It should redirected to patient home page.
                     * Dated: 14 jan 2015
                     */

                    //theUrl = string.Format("{0}?mod={1}", "./frmAddTechnicalArea.aspx", Request.QueryString["mod"]);
                    theUrl = "./ClinicalForms/frmPatient_Home.aspx";
                }
                else if (theDS.Tables[2].Rows.Count > 0 && theDS.Tables[2].Rows[0]["StartDate"].ToString() != "")
                {

                    theUrl = "./ClinicalForms/frmPatient_Home.aspx";

                }
                else
                {
                    //theUrl = string.Format("{0}?PatientId={1}&mod={2}", "./frmAddTechnicalArea.aspx", patientID, Request.QueryString["mod"]);
                    theUrl = string.Format("{0}?mod={1}", "./frmAddTechnicalArea.aspx", Request.QueryString["mod"]);

                }
                Response.Redirect(theUrl, false);


            }
        }

    }
}