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
using ChartDirector;
using Interface.Security;
using Application.Common;
using Application.Presentation;
using Interface.Reports;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using System.Drawing;
using System.Drawing.Imaging;
using Interface.Security;
using Interface.Clinical;
using Graph = Microsoft.Office.Interop.Owc11;
using Touch;

namespace PresentationApp.Touch.Custom_Forms
{
    public partial class frmLabHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtnow = System.DateTime.Now;
            string dt = dtnow.ToString("D");
            lblname.Text = "Latest Laboratory Results for: " + Session["patientname"].ToString() + "";
            lbldate.Text = "Report Date: " + dt + "";
           int patientid = Convert.ToInt32(Session["PatientID"].ToString());
           IPatientHome PatientManager;
           PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
           System.Data.DataSet theDS = PatientManager.GetPatientLabHistory(patientid);
           rgvlabhistory.DataSource = theDS.Tables[0];
        }
    }
}