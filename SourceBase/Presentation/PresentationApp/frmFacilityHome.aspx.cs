using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Interface.Clinical;
using Application.Presentation;
using System.Drawing;

namespace IQCare.Web
{
    public partial class frmFacilityHome2 : System.Web.UI.Page
    {
        string ObjFactoryParameter = "BusinessProcess.Clinical.BPatientRegistration, BusinessProcess.Clinical";
        String[] aTileBackgroundColor = {"", "#A62241","#4765EB","#2C7F96","#CA4D2F","#2F3FB0","#890094","#479ADA","#1E8C00","#5724A9","#479ADA"};

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AppLocation"] == null || Session.Count == 0 || Session["AppUserID"].ToString() == "")
            {
                IQCareMsgBox.Show("SessionExpired", this);
                Response.Redirect("~/frmlogin.aspx",true);
            }
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "Facility Home";
            (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Visible = false;
            if(!IsPostBack)
                Init_page();
        }
        private void Init_page()
        {
            Session["PatientId"] = 0;
            IPatientRegistration ptnMgr = (IPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientRegistration, BusinessProcess.Clinical");
            DataSet DSModules = ptnMgr.GetModuleNames(Convert.ToInt32(Session["AppLocationId"]));

            DataTable theDT = new DataTable();
            theDT = DSModules.Tables[0];
            createserviceButtons(theDT);
        }
        private  void createserviceButtons(DataTable modules)
        {
            IPatientRegistration ptnMgr = (IPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);
            DataSet DSModules = new DataSet();
            DataTable theDT = new DataTable();
            if (Convert.ToInt32(Session["AppUserId"]) > 1)
            {
                DSModules = ptnMgr.GetModuleNames(Convert.ToInt32(Session["AppLocationId"]), Convert.ToInt32(Session["AppUserId"]));
                theDT = DSModules.Tables[2];
            }
            else
            {
                DSModules = ptnMgr.GetModuleNames(Convert.ToInt32(Session["AppLocationId"]));
                theDT = DSModules.Tables[0];
            }
            //removing Workplan if KNH Static form service is not available
            Session["isKNHEnabled"] = false;
            DataTable dt = theDT;

            if (!object.Equals(theDT, null))
            {
                int tmp = dt.AsEnumerable().Where(p => p.Field<int>("moduleid") == Convert.ToInt32("204")).Select(p => p.Field<int>("moduleid")).FirstOrDefault();
                if(tmp>0)
                    Session["isKNHEnabled"] = true;
            }

            //first add the records button which is default present
            int i = 0;
            int cntr = 0;
            TableRow serviceRow = new TableRow();
            TableCell recordsCell = new TableCell();
            recordsCell.Attributes.Add("align", "center");
            recordsCell.Attributes.Add("width", "25%");
            recordsCell.Attributes.Add("height", "100px");

            Button btnRecords = new Button();
            btnRecords.Text = "Records";
            btnRecords.BackColor = System.Drawing.ColorTranslator.FromHtml(aTileBackgroundColor[1].ToString());
            btnRecords.Attributes.Add("class", "tileButton");
            btnRecords.BorderStyle = BorderStyle.None;
            btnRecords.OnClientClick = String.Format("window.location = 'frmFindAddCustom.aspx?srvNm={0}&mod={1}'; return false;", "Records", 0);
            btnRecords.Height = Unit.Pixel(100);
                       
            if (theDT.Rows.Count == 1)
                btnRecords.Width = Unit.Percentage(70);
            else if (theDT.Rows.Count == 2)
                btnRecords.Width = Unit.Percentage(90);
            else
                btnRecords.Width = Unit.Percentage(100);

            recordsCell.Controls.Add(btnRecords);
            serviceRow.Cells.Add(recordsCell);
            mainTable.Controls.Add(serviceRow);
            i = 1;
            cntr = 1;
                
            foreach (DataRow dr in theDT.Rows)
            {
                if (Convert.ToInt32(dr["ModuleID"]) != 201)
                {
                    if (i % 4 == 0)
                        serviceRow = new TableRow();
                        
                   
                    TableCell serviceCell = new TableCell();
                    serviceCell.Attributes.Add("align", "center");
                    serviceCell.Attributes.Add("width", "25%");
                    serviceCell.Attributes.Add("height", "100px");

                    Button btnService = new Button();
                    btnService.Text = dr[2].ToString();
                    btnService.Attributes.Add("data-moduleID", dr[1].ToString());
                    btnService.BackColor = System.Drawing.ColorTranslator.FromHtml(aTileBackgroundColor[cntr].ToString());
                    btnService.Attributes.Add("class", "tileButton");
                    btnService.BorderStyle = BorderStyle.None;
                    btnService.OnClientClick = String.Format("window.location = 'frmFindAddCustom.aspx?srvNm={0}&mod={1}'; return false;", btnService.Text, dr[1]);
                    btnService.Height = Unit.Pixel(100);
                    if (theDT.Rows.Count == 1)
                        btnService.Width = Unit.Percentage(70);
                    else if (theDT.Rows.Count == 2)
                        btnService.Width = Unit.Percentage(90);
                    else
                        btnService.Width = Unit.Percentage(100);
                        
                    serviceCell.Controls.Add(btnService);
                    serviceRow.Cells.Add(serviceCell);
                    if (i % 4 == 0)
                        mainTable.Controls.Add(serviceRow);
                        
                    i += 1;
                    cntr += 1;
                    //resetting color counter
                    if (i > cntr)
                        cntr = 1;
                }
            }
            ptnMgr = null;
        }
    }
}