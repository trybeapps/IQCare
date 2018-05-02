using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//IQCare Libs
using Application.Presentation;
using Application.Common;
using Interface.Clinical;
//Third party Libs
using Telerik.Web.UI;
using System.Data;
using System.IO;


namespace PresentationApp.Touch.Custom_Forms
{
    public partial class KNHVitalSignShowModal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // Init_Form();
                //Response.Write(Request.QueryString["flagMode"]);


            }

        }
        private void Init_Form()
        {
            // code here for the future

        }
        //protected DataTable GetDataTable(string flag)
        //{
        //    //BIQTouchExpressFields objExpressFields = new BIQTouchExpressFields();
        //    //objExpressFields.Flag = flag;
        //    //objExpressFields.PtnPk = Convert.ToInt32(Session["PatientID"].ToString());
        //    //objExpressFields.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
        //    //objExpressFields.ID = Int32.Parse(Session["Visit_pk"].ToString());

        //    //IQTouchKNHExpress theExpressManager;
        //    //theExpressManager = (IQTouchKNHExpress)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchKNHExpress, BusinessProcess.Clinical");
        //    //DataTable dt = theExpressManager.IQTouchGetKnhExpressData(objExpressFields);
        //    //return dt;
        //}

       
    }
}