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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.Drawing;
using System.Drawing.Imaging;
using Interface.Security;
using Interface.Clinical;
using Graph = Microsoft.Office.Interop.Owc11;
using Touch;

namespace PresentationApp.Touch.Custom_Forms
{
    public partial class frmReportView : System.Web.UI.Page
    {
        int patientid = 0;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            patientid = Convert.ToInt32(Session["PatientID"].ToString());
            if (Request.QueryString["report"] == "visit")
            {
                System.Data.DataSet ds;
                IReports PatientSummary;
                PatientSummary = (IReports)ObjectFactory.CreateInstance("BusinessProcess.Reports.BReports,BusinessProcess.Reports");
                //ds = PatientSummary.IQTouchGetPatientVisitSummary(Convert.ToInt32(Request.QueryString["PatientID"]));
                ds = PatientSummary.IQTouchGetPatientVisitSummary(patientid);

                
                ReportDocument rptDocument;
                rptDocument = new ReportDocument();
                rptDocument.Load(Server.MapPath("..\\..\\Touch\\Report\\PatientVisit.rpt"));
                rptDocument.SetDataSource(ds);
                rptDocument.SetParameterValue("facilityname", Session["AppLocation"].ToString());
                CrystalReportViewer1.ReportSource = rptDocument;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            }
            if (Request.QueryString["report"] == "summary")
            {
                System.Data.DataSet ds;
                IReports PatientSummary;
                PatientSummary = (IReports)ObjectFactory.CreateInstance("BusinessProcess.Reports.BReports,BusinessProcess.Reports");
                ds = PatientSummary.IQTouchGetPatientSummary(patientid);
                //string strxml = ds.GetXmlSchema();
                

                #region Format PhysicalFindingsHistory
                DataTable dtPhysicalFindingsHistory = ds.Tables[10].Copy();
                dtPhysicalFindingsHistory.Rows.Clear();
                for (int i = 0; i < ds.Tables[10].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        dtPhysicalFindingsHistory.Rows.Add(
                        ds.Tables[10].Rows[0]["visitdate"].ToString(),
                        ds.Tables[10].Rows[0]["Illness"].ToString(),
                        ds.Tables[10].Rows[0]["symptomdescription"].ToString());
                    }
                    else
                    {
                        if(ds.Tables[10].Rows[i]["visitdate"].ToString().Equals(dtPhysicalFindingsHistory.Rows[dtPhysicalFindingsHistory.Rows.Count - 1]["visitdate"].ToString()))
                            dtPhysicalFindingsHistory.Rows[dtPhysicalFindingsHistory.Rows.Count-1]["Illness"] = dtPhysicalFindingsHistory.Rows[dtPhysicalFindingsHistory.Rows.Count-1]["Illness"] + ","+ ds.Tables[10].Rows[i]["Illness"].ToString();
                        else
                        {
                            dtPhysicalFindingsHistory.Rows.Add(
                                ds.Tables[10].Rows[i]["visitdate"].ToString(),
                                ds.Tables[10].Rows[i]["Illness"].ToString(),
                                ds.Tables[10].Rows[i]["symptomdescription"].ToString());
                        }
                    }
                }
                ds.Tables[10].Rows.Clear();

                int c = -1;
                foreach (DataRow dr in dtPhysicalFindingsHistory.Rows)
                {
                    c++;
                    ds.Tables[10].Rows.Add();
                    foreach (DataColumn dc in dtPhysicalFindingsHistory.Columns)
                    {
                        ds.Tables[10].Rows[c][dc.ColumnName] = dr[dc.ColumnName];
                    }
                }
#endregion

                ReportDocument rptDocument;
                rptDocument = new ReportDocument();
                rptDocument.Load(Server.MapPath("..\\..\\Touch\\Report\\PatientSummaryMain.rpt"));
                rptDocument.SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryCaregiver.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryARVTreatmentHistory.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryRegimenHistory.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryMostResent.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryHistoryofPhysicalFindings.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryHistoryofAdverseEvents.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryImmunisations.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryARVMutations.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryLabHistory.rpt"].SetDataSource(ds);
                rptDocument.Subreports["PatientSummaryWHB.rpt"].SetDataSource(ds);

                CrystalReportViewer1.EnableDrillDown = false;
                CrystalReportViewer1.ReportSource = rptDocument;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            }
            if (Request.QueryString["report"] == "chart")
            {
                BindGraph();
            }
        }
        public void BindGraph()
        {
            IPatientHome PatientManager;
            PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
            System.Data.DataSet theDS = PatientManager.IQTouchGetPatientDetails(patientid);

            /*CD4 and Viral Load Graph */
            double[] CD4 = new Double[theDS.Tables[0].Rows.Count];
            for (Int32 a = 0, l = CD4.Length; a < l; a++)
            {
                if (theDS.Tables[0].Rows[a]["TestResult"] != System.DBNull.Value)
                {
                    CD4.SetValue(Convert.ToDouble(theDS.Tables[0].Rows[a]["TestResult"]), a);
                }
            }

            double[] ViralLoad = new Double[theDS.Tables[1].Rows.Count];
            for (Int32 a = 0, l = ViralLoad.Length; a < l; a++)
            {
                if (theDS.Tables[1].Rows[a]["TestResult"] != System.DBNull.Value)
                {
                    ViralLoad.SetValue(Convert.ToDouble(theDS.Tables[1].Rows[a]["TestResult"]), a);
                }
            }

            DateTime[] YearCD4 = new DateTime[theDS.Tables[0].Rows.Count];
            for (Int32 a = 0, l = YearCD4.Length; a < l; a++)
            {
                YearCD4.SetValue((DateTime)theDS.Tables[0].Rows[a]["DATE"], a);
            }

            DateTime[] YearVL = new DateTime[theDS.Tables[1].Rows.Count];
            for (Int32 a = 0, l = YearVL.Length; a < l; a++)
            {
                YearVL.SetValue(theDS.Tables[1].Rows[a]["DATE"], a);
            }

            DateTime[] Year = new DateTime[theDS.Tables[2].Rows.Count];
            for (Int32 a = 0, l = Year.Length; a < l; a++)
            {
                Year.SetValue(theDS.Tables[2].Rows[a]["DATE"], a);
            }
            //18thAug2009 createChartCD4(CD4, ViralLoad, YearCD4, YearVL, Year);
            Chart.setLicenseCode("DEVP-2AC2-336W-54FM-EAB2-F8E2");
            createChartCD4(WebChartViewerCD4VL, CD4, ViralLoad, YearCD4, YearVL, Year);

        }
        private void createChartCD4(WebChartViewer viewer, Double[] CD4, Double[] ViralLoad, DateTime[] YearCD4, DateTime[] YearVL, DateTime[] Year)
        {
            XYChart c = new XYChart(300, 180, 0xddddff, 0x000000, 1);
            c.addLegend(90, 10, false, "Arial Bold", 7).setBackground(0xcccccc);
            c.setPlotArea(60, 60, 180, 45, 0xffffff).setGridColor(0xcccccc, 0xccccccc);
            c.xAxis().setTitle("Year");
            c.xAxis().setLabelStyle("Arial", 8, 1).setFontAngle(90);
            c.yAxis().setLinearScale(0, 1500, 500, 0);
            c.yAxis2().setLogScale(10, 10000, 10);

            LineLayer layer = c.addLineLayer2();

            layer.setLineWidth(2);
            layer.addDataSet(CD4, 0xff0000, "CD4").setDataSymbol(Chart.CircleShape, 5);
            layer.setXData(YearCD4);

            LineLayer layer1 = c.addLineLayer2();
            layer1.setLineWidth(2);
            layer1.setUseYAxis2();
            layer1.addDataSet(ViralLoad, 0x008800, "Viralload").setDataSymbol(Chart.CircleShape, 5);
            layer1.setXData(YearVL);

            // Output the chart
            viewer.Image = c.makeWebImage(Chart.PNG);
            viewer.ImageMap = c.getHTMLImageMap("", "",
                "title='{dataSetName} Count on {xLabel}={value}'");






        }
    }
}