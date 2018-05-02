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
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using Interface.Security;
using Interface.Clinical;
using ChartDirector;
using Application.Common;
using Application.Presentation;
using Interface.Reports;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using Graph = Microsoft.Office.Interop.Owc11;
using Touch;

namespace Touch.Custom_Forms
{
    
    public partial class frmReportsTouch : TouchUserControlBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Page != null && Page.IsCallback)
                EnsureChildControls();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            String script = frmReport_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);

            base.Page_Load(sender, e);
            Session["PatientID"] = Request.QueryString["PatientID"];

            DateTime dtnow = System.DateTime.Now;
            string dt = dtnow.ToString("D");
            lblname.Text = "Latest Laboratory Results for: " + Session["patientname"].ToString() + "";
            lbldate.Text = "Report Date :" + dt + "";
            int patientid = Convert.ToInt32(Session["PatientID"].ToString());
            IPatientHome PatientManager;
            PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
            System.Data.DataSet theDS = PatientManager.GetPatientLabHistory(patientid);
            rgvlabhistory.DataSource = theDS.Tables[0];
            //BindGraph();
        }

        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            uptdpharmacyResults.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmReport_ScriptBlock", "PrintLabHistory('Lab History Form');", true);
        }
        //public void BindGraph()
        //{
        //    IPatientHome PatientManager;
        //    PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
        //    System.Data.DataSet theDS = PatientManager.IQTouchGetPatientDetails(Convert.ToInt32(Request.QueryString["PatientID"]));

        //    /*CD4 and Viral Load Graph */
        //    double[] CD4 = new Double[theDS.Tables[0].Rows.Count];
        //    for (Int32 a = 0, l = CD4.Length; a < l; a++)
        //    {
        //        if (theDS.Tables[0].Rows[a]["TestResult"] != System.DBNull.Value)
        //        {
        //            CD4.SetValue(Convert.ToDouble(theDS.Tables[0].Rows[a]["TestResult"]), a);
        //        }
        //    }

        //    double[] ViralLoad = new Double[theDS.Tables[1].Rows.Count];
        //    for (Int32 a = 0, l = ViralLoad.Length; a < l; a++)
        //    {
        //        if (theDS.Tables[1].Rows[a]["TestResult"] != System.DBNull.Value)
        //        {
        //            ViralLoad.SetValue(Convert.ToDouble(theDS.Tables[1].Rows[a]["TestResult"]), a);
        //        }
        //    }

        //    DateTime[] YearCD4 = new DateTime[theDS.Tables[0].Rows.Count];
        //    for (Int32 a = 0, l = YearCD4.Length; a < l; a++)
        //    {
        //        YearCD4.SetValue((DateTime)theDS.Tables[0].Rows[a]["DATE"], a);
        //    }

        //    DateTime[] YearVL = new DateTime[theDS.Tables[1].Rows.Count];
        //    for (Int32 a = 0, l = YearVL.Length; a < l; a++)
        //    {
        //        YearVL.SetValue(theDS.Tables[1].Rows[a]["DATE"], a);
        //    }

        //    DateTime[] Year = new DateTime[theDS.Tables[2].Rows.Count];
        //    for (Int32 a = 0, l = Year.Length; a < l; a++)
        //    {
        //        Year.SetValue(theDS.Tables[2].Rows[a]["DATE"], a);
        //    }
        //    //18thAug2009 createChartCD4(CD4, ViralLoad, YearCD4, YearVL, Year);
        //    Chart.setLicenseCode("DEVP-2AC2-336W-54FM-EAB2-F8E2");
        //    createChartCD4(WebChartViewerCD4VL, CD4, ViralLoad, YearCD4, YearVL, Year);

        //    // BMI
        //    double[] Height = new Double[theDS.Tables[3].Rows.Count];
        //    for (Int32 a = 0, l = Height.Length; a < l; a++)
        //    {
        //        Height.SetValue(Convert.ToDouble(theDS.Tables[3].Rows[a]["Height"]), a);
        //    }
        //    double[] Weight = new Double[theDS.Tables[3].Rows.Count];
        //    for (Int32 a = 0, l = Weight.Length; a < l; a++)
        //    {
        //        Weight.SetValue(Convert.ToDouble(theDS.Tables[3].Rows[a]["Weight"]), a);
        //    }

        //    double[] BMI = new Double[theDS.Tables[3].Rows.Count];
        //    for (Int32 a = 0, l = Weight.Length; a < l; a++)
        //    {
        //        if (theDS.Tables[3].Rows[a]["BMI"] != System.DBNull.Value)
        //        { BMI.SetValue(Convert.ToDouble(theDS.Tables[3].Rows[a]["BMI"]), a); }
        //    }

        //    DateTime[] YearWeightBMI = new DateTime[theDS.Tables[3].Rows.Count];
        //    for (Int32 a = 0, l = YearWeightBMI.Length; a < l; a++)
        //    {
        //        YearWeightBMI.SetValue(theDS.Tables[3].Rows[a]["Visit_OrderbyDate"], a);
        //    }
        //    // 18thAug2009 createChartWeight( Weight, BMI, YearWeightBMI);
        //    createChartWeight(WebChartViewerWeight, Weight, BMI, YearWeightBMI);
        
       
        //}
        //private void createChartCD4(WebChartViewer viewer, Double[] CD4, Double[] ViralLoad, DateTime[] YearCD4, DateTime[] YearVL, DateTime[] Year)
        //{
        //    XYChart c = new XYChart(300, 190, 0xddddff, 0x000000, 1);
        //    c.addLegend(90, 10, false, "Arial Bold", 7).setBackground(0xcccccc);
        //    c.setPlotArea(60, 60, 180, 45, 0xffffff).setGridColor(0xcccccc, 0xccccccc);
        //    c.xAxis().setTitle("Year");
        //    c.xAxis().setLabelStyle("Arial", 8, 1).setFontAngle(90);
        //    c.yAxis().setLinearScale(0, 1500, 500, 0);
        //    c.yAxis2().setLogScale(10, 10000, 10);

        //    LineLayer layer = c.addLineLayer2();

        //    layer.setLineWidth(2);
        //    layer.addDataSet(CD4, 0xff0000, "CD4").setDataSymbol(Chart.CircleShape, 5);
        //    layer.setXData(YearCD4);

        //    LineLayer layer1 = c.addLineLayer2();
        //    layer1.setLineWidth(2);
        //    layer1.setUseYAxis2();
        //    layer1.addDataSet(ViralLoad, 0x008800, "Viralload").setDataSymbol(Chart.CircleShape, 5);
        //    layer1.setXData(YearVL);

        //    // Output the chart
        //    //viewer.Image = c.makeWebImage(Chart.PNG);
        //    //viewer.ImageMap = c.getHTMLImageMap("", "",
        //    //    "title='{dataSetName} Count on {xLabel}={value}'");

        //    c.makeChart(Server.MapPath("/Touch/mychart.png"));
        //    viewer.ImageUrl = "/Touch/mychart.png";

            
            


        //}
        //private void createChartWeight(WebChartViewer Wviewer, Double[] Weight, Double[] BMI, DateTime[] YearWeightBMI)
        //{
        //    XYChart c = new XYChart(300, 180, 0xddddff, 0x000000, 1);
        //    c.addLegend(90, 10, false, "Arial Bold", 7).setBackground(0xcccccc);
        //    c.setPlotArea(60, 60, 180, 45, 0xffffff).setGridColor(0xcccccc, 0xccccccc);
        //    c.xAxis().setTitle("Year");
        //    c.xAxis().setLabelStyle("Arial", 8, 1).setFontAngle(90);
        //    c.yAxis().setLinearScale(0, 200, 50, 0);
        //    c.yAxis2().setLogScale(0, 1000, 10);

        //    LineLayer layer = c.addLineLayer2();
        //    layer.setLineWidth(2);
        //    layer.addDataSet(Weight, 0xff0000, "Weight").setDataSymbol(Chart.CircleShape, 5);
        //    int count = YearWeightBMI.Length;
        //    layer.setXData(YearWeightBMI);

        //    LineLayer layer1 = c.addLineLayer2();
        //    layer1.setLineWidth(2);
        //    layer1.setUseYAxis2();
        //    layer1.addDataSet(BMI, 0x008800, "BMI").setDataSymbol(Chart.CircleShape, 5);
        //    layer1.setXData(YearWeightBMI);

        //    // Output the chart
        //    //Wviewer.Image = c.makeWebImage(Chart.PNG);
        //    //Include tool tip for the chart
        //    //Wviewer.ImageMap = c.getHTMLImageMap("", "",
        //     // "title='{dataSetName} Count on {xLabel}={value}'");

        //    c.makeChart(Server.MapPath("/Touch/mychart1.png"));
        //    Wviewer.ImageUrl = "/Touch/mychart1.png";

        //}

    }
}