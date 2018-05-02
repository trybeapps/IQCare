using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Application.Presentation;
using Application.Common;

namespace PresentationApp.ClinicalForms.UserControl
{
    public partial class UserControlKNH_NextAppointment : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            addAttributes();
        }

        protected void btnPrescribeDrugs_Click(object sender, EventArgs e)
        {
            IQCareUtils.Redirect("../Scheduler/frmScheduler_AppointmentNewHistory.aspx?name=Add&FormName=Appointment History New&opento=popup", "_blank", "toolbars=no,location=no,directories=no,dependent=yes,top=100,left=30,maximize=no,resize=no,width=1000,height=800,scrollbars=yes");
        }

        public void addAttributes()
        {
            rdoTCAYes.Attributes.Add("OnClick", "ShowHide('trNextAppointment','show');ShowHide('trCareEnd','hide');");
            rdoTCANo.Attributes.Add("OnClick", "ShowHide('trCareEnd','show');ShowHide('trNextAppointment','hide');");
        }

        protected void btnCareEnd_Click(object sender, EventArgs e)
        {
            IQCareUtils.Redirect("../Scheduler/frmScheduler_ContactCareTracking.aspx?opento=popup", "_blank", "toolbars=no,location=no,directories=no,dependent=yes,top=100,left=30,maximize=no,resize=no,width=1000,height=500,scrollbars=yes");
        }
    }
}