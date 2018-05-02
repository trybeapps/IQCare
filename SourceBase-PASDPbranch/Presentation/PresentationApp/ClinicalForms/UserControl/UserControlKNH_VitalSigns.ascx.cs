using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Application.Common;
using Application.Presentation;

namespace PresentationApp.ClinicalForms.UserControl
{
    public partial class UserControlKNH_VitalSigns : System.Web.UI.UserControl
    {
        DataSet theDSXML = new DataSet();
        DataView theDVCodeID, theDV;
        DataTable theDT;
        BindFunctions BindManager = new BindFunctions();
        IQCareUtils theUtils = new IQCareUtils();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                addAttributes();
                Bind_Select_Lists();
            }
            JavaScriptFunctionsOnLoad();

        }
        private void Bind_Select_Lists()
        {
            theDSXML.ReadXml(MapPath("..\\..\\XMLFiles\\AllMasters.con"));

            theDVCodeID = new DataView(theDSXML.Tables["Mst_Code"]);
            theDVCodeID.RowFilter = "Name='WeightForAge'";
            theDV = new DataView(theDSXML.Tables["Mst_Decode"]);
            theDV.RowFilter = "CodeID=" + ((DataTable)theDVCodeID.ToTable()).Rows[0]["CodeID"].ToString();
            theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
            BindManager.BindCombo(ddlweightforage, theDT, "Name", "ID");

            theDVCodeID = new DataView(theDSXML.Tables["Mst_Code"]);
            theDVCodeID.RowFilter = "Name='WeightForAge'";
            theDV = new DataView(theDSXML.Tables["Mst_Decode"]);
            theDV.RowFilter = "CodeID=" + ((DataTable)theDVCodeID.ToTable()).Rows[0]["CodeID"].ToString();
            theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
            BindManager.BindCombo(ddlweightforheight, theDT, "Name", "ID");

            theDVCodeID = new DataView(theDSXML.Tables["Mst_Code"]);
            theDVCodeID.RowFilter = "Name='RefferedToFUpF'";
            theDV = new DataView(theDSXML.Tables["Mst_Decode"]);
            theDV.RowFilter = "CodeID=" + ((DataTable)theDVCodeID.ToTable()).Rows[0]["CodeID"].ToString();
            theDV.Sort = "SRNo asc, Name";
            theDT = (DataTable)theUtils.CreateTableFromDataView(theDV);
            BindManager.BindCheckedList(cblReferredTo, theDT, "Name", "ID");
        }

        //public void BindCheckUncheklogic(CheckBoxList chklst)
        //{
        //    for (int i = 0; i < chklst.Items.Count; i++)
        //    {
        //        ListItem li = chklst.Items[i];
        //        if (li.Text == "None")
        //        {
        //            li.Attributes.Add("OnClick", "fnUncheckall('" + chklst.ClientID + "')");
        //        }
        //        else
        //        {
        //            li.Attributes.Add("OnClick", "fnUncheckNormal('" + chklst.ClientID + "')");
        //        }

        //    }
        //}

        private void JavaScriptFunctionsOnLoad()
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "BMI", "CalcualteBMI('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtWeight.ClientID + "','" + txtHeight.ClientID + "','" + txtBMI.ClientID + "','" + lblBMIClassification.ClientID + "');", true);
            double age = Convert.ToDouble(Session["patientageinyearmonth"].ToString());

            if (age >= 15)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HighLightAbnormalValuesAdults", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showHidePeads", "show_hide('divshowvitalsign','notvisible');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "HighLightAbnormalValuesPeads", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showHidePeads", "show_hide('divshowvitalsign','visible');", true);
            }
        }

        private void addAttributes()
        {
            //cblReferredTo.Attributes.Add("OnClick", "TriageCheckBoxHideUnhideOtherSpecialistClinic('" + cblReferredTo.ClientID + "');TriageCheckBoxHideUnhideOtherReferral('" + cblReferredTo.ClientID + "');");

            cblReferredTo.Attributes.Add("OnClick", "TriageCheckBoxHideUnhideOtherSpecialistClinic('" + cblReferredTo.ClientID + "','" + txtReferToSpecialistClinic.ClientID + "');TriageCheckBoxHideUnhideOtherReferral('" + cblReferredTo.ClientID + "','" + txtSpecifyOtherRefferedTo.ClientID + "');fnUncheckallVitals('" + cblReferredTo.ClientID + "');");
            txtHeight.Attributes.Add("OnBlur", "CalcualteBMI('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtWeight.ClientID + "','" + txtHeight.ClientID + "','" + txtBMI.ClientID + "','" + lblBMIClassification.ClientID + "');");
            txtWeight.Attributes.Add("OnBlur", "CalcualteBMI('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtWeight.ClientID + "','" + txtHeight.ClientID + "','" + txtBMI.ClientID + "','" + lblBMIClassification.ClientID + "');");
            txtHeight.Attributes.Add("onkeyup", "chkDecimal('" + txtHeight.ClientID + "');");
            txtWeight.Attributes.Add("onkeyup", "chkDecimal('" + txtWeight.ClientID + "');");
            txtheadcircumference.Attributes.Add("onkeyup", "chkDecimal('" + txtheadcircumference.ClientID + "');");
            //txtweightforheight.Attributes.Add("onkeyup", "chkDecimal('" + txtweightforheight.ClientID + "');");

            double age = Convert.ToDouble(Session["patientageinyearmonth"].ToString());

            if (age >= 15)
            {
                txtTemp.Attributes.Add("OnBlur", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtTemp.Attributes.Add("onkeyup", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtTemp.ClientID + "')");

                txtBPSystolic.Attributes.Add("OnBlur", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtBPSystolic.Attributes.Add("onkeyup", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtBPSystolic.ClientID + "');");

                txtBPDiastolic.Attributes.Add("OnBlur", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtBPDiastolic.Attributes.Add("onkeyup", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtBPDiastolic.ClientID + "');");

                txtRR.Attributes.Add("OnBlur", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtRR.Attributes.Add("onkeyup", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtRR.ClientID + "');");

                txtHR.Attributes.Add("OnBlur", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtHR.Attributes.Add("onkeyup", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtHR.ClientID + "');");

                //txtBMI.Attributes.Add("OnBlur", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                //txtBMI.Attributes.Add("onkeyup", "HighLightAbnormalValuesAdults('" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
            }
            else
            {
                txtTemp.Attributes.Add("OnBlur", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtTemp.Attributes.Add("onkeyup", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtTemp.ClientID + "')");

                txtBPSystolic.Attributes.Add("OnBlur", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtBPSystolic.Attributes.Add("onkeyup", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtBPSystolic.ClientID + "');");

                txtBPDiastolic.Attributes.Add("OnBlur", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtBPDiastolic.Attributes.Add("onkeyup", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtBPDiastolic.ClientID + "');");

                txtRR.Attributes.Add("OnBlur", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtRR.Attributes.Add("onkeyup", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtRR.ClientID + "');");

                txtHR.Attributes.Add("OnBlur", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                txtHR.Attributes.Add("onkeyup", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');chkDecimal('" + txtHR.ClientID + "');");

                //txtBMI.Attributes.Add("OnBlur", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
                //txtBMI.Attributes.Add("onkeyup", "HighLightAbnormalValuesPeads('" + Convert.ToDouble(Session["patientageinyearmonth"].ToString()) + "','" + txtTemp.ClientID + "','" + txtRR.ClientID + "','" + txtHR.ClientID + "','" + txtBPSystolic.ClientID + "','" + txtBPDiastolic.ClientID + "');");
            }


            
            
            
        }

        
    }
}