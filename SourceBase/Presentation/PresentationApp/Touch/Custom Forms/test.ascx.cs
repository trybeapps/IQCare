using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//IQCare usings
using Interface.Clinical;
using Touch.FormObjects;
using Application.Presentation;
using Application.Common;
using Telerik.Web.UI;

namespace Touch.Custom_Forms
{
    public partial class test : TouchUserControlBase
    {
        private static List<Allergen> AllergenArray = new List<Allergen>();
        string ObjFactoryParameter = "BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical";
        static objRegistration rg = new objRegistration();
        Hashtable GetValuefromHT; int patientID = 0;
        int flag = 0;

        protected void Page_Load(object s, EventArgs e)
        {
            //AjaxifyControls();

            //if (IsPostBack) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);
            Page.MaintainScrollPositionOnPostBack = true;

            Session["CurrentForm"] = "frmRegistrationTouch";
            Session["FormIsLoaded"] = true;

            if (Session["IsFirstLoad"] != null)
            {
                if (Session["IsFirstLoad"].ToString() == "true")
                {
                    Session["IsFirstLoad"] = "false";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);
                    Init_Form();

                }
            }

            if (AllergenArray != null)
            {
                //rgAllergies.DataSource = AllergenArray;
                //rgAllergies.DataBind();
            }
            base.Page_Load(s, e);
        }
        protected void Init_Form()
        {
            objRegistration theRegistration = new objRegistration();
            IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);

            patientID = int.Parse(Request.QueryString["patientId"].ToString());

            DataSet regDT = ptnMgr.GetRegistrationDetails(patientID, Session["AppLocationId"].ToString());

            SetFieldVals(regDT.Tables[0]);

            SetFormVals(regDT.Tables[0]);

            DataSet theDS = ptnMgr.GetPatientRegistration(patientID, 12);
        }
        protected void SetFormVals(DataTable theDT)
        {
            txtFirstName.Text = rg.FirstName;
            txtLastName.Text = rg.LastName;
            dtPatientDOB.SelectedDate = DateTime.Parse(rg.DOB);
            cbSex.SelectedValue = rg.Sex.ToString();
            dtRegistrationDate.SelectedDate = DateTime.Parse(rg.RegistrationDate);


        }
        protected objRegistration SetFieldVals(DataTable regDT)
        {
            rg.LastName = regDT.Rows[0][0].ToString();
            rg.FirstName = regDT.Rows[0][1].ToString();
            rg.DOB = regDT.Rows[0][2].ToString();
            if (regDT.Rows[0][3].ToString() != "") rg.Sex = int.Parse(regDT.Rows[0][3].ToString());
            rg.RegistrationDate = regDT.Rows[0][4].ToString();
            rg.Address = regDT.Rows[0][5].ToString();
            rg.Suburb = regDT.Rows[0][6].ToString();
            rg.District = regDT.Rows[0][7].ToString();
            rg.TelephoneNo = regDT.Rows[0][8].ToString();
            rg.Addresscomments = regDT.Rows[0][9].ToString();
            rg.PostalAddress = regDT.Rows[0][10].ToString();
            rg.PostalCode = regDT.Rows[0][11].ToString();
            rg.EntryPoint = regDT.Rows[0][12].ToString();
            rg.OtherEntryPoint = regDT.Rows[0][13].ToString();
            rg.CareGiverName = regDT.Rows[0][14].ToString();
            rg.CareGiverDOB = regDT.Rows[0][15].ToString();
            if (regDT.Rows[0][16].ToString() != "") rg.CareGiverGender = int.Parse(regDT.Rows[0][16].ToString());
            if (regDT.Rows[0][17].ToString() != "") rg.CareGiverRelationship = int.Parse(regDT.Rows[0][17].ToString());
            rg.OtherCareGiver = regDT.Rows[0][18].ToString();
            rg.CareGiverTelephone = regDT.Rows[0][19].ToString();
            rg.MotherName = regDT.Rows[0][20].ToString();
            if (regDT.Rows[0][21].ToString() != "") rg.MotherAliveYN = bool.Parse(regDT.Rows[0][21].ToString());
            //if (regDT.Rows[0][22].ToString() != "") rg.MotherPMTCTdrugsYN = bool.Parse(regDT.Rows[0][22].ToString());
            //if (regDT.Rows[0][23].ToString() != "") rg.ChildPMTCTdrugsYN = bool.Parse(regDT.Rows[0][23].ToString());
            //if (regDT.Rows[0][24].ToString() != "") rg.MotherARTYN = bool.Parse(regDT.Rows[0][24].ToString());
            if (regDT.Rows[0][25].ToString() != "") rg.FeedingOption = int.Parse(regDT.Rows[0][25].ToString());
            rg.DateConfirmedHIVPositive = regDT.Rows[0][26].ToString();
            rg.DateEnrolledHIVCare = regDT.Rows[0][27].ToString();
            if (regDT.Rows[0][28].ToString() != "") rg.WHOStageAtEnrollment = int.Parse(regDT.Rows[0][28].ToString());
            rg.TransferInDate = regDT.Rows[0][29].ToString();
            if (regDT.Rows[0][30].ToString() != "") rg.FromDistrict = int.Parse(regDT.Rows[0][30].ToString());
            rg.Facility = regDT.Rows[0][31].ToString();
            rg.DateStart = regDT.Rows[0][32].ToString();
            rg.Weight = regDT.Rows[0][33].ToString();
            rg.Height = regDT.Rows[0][34].ToString();
            rg.PriorART = regDT.Rows[0][35].ToString();
            //rg.PriorARTDateLastUsed = regDT.Rows[0][36].ToString();
            return rg;
        }

        protected void dtPatientDOB_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            Age COGAge = new Age((DateTime)e.NewDate, DateTime.Now);
            AgeYearZZZ.Text = COGAge.Years.ToString();
            AgeMonthZZZ.Text = COGAge.Months.ToString();
            //uptPatientDOB.Update();
        }

        protected void btnCalCDOB_Click(object sender, EventArgs e)
        {
            if (AgeYearZZZ.Text.Trim() == "")
            {
                AgeYearZZZ.Text = "0";
            }
            if (AgeMonthZZZ.Text != "")
            {
                if ((Convert.ToInt32(AgeMonthZZZ.Text) < 0) || (Convert.ToInt32(AgeMonthZZZ.Text) > 11))
                {
                    MsgBuilder theMsg = new MsgBuilder();
                    theMsg.DataElements["Control"] = "Age (Month)";
                    IQCareMsgBox.Show("AgeMonthRange", theMsg, this);
                    return;
                }
            }
            else
                AgeMonthZZZ.Text = "0";

            int age = 0;
            int months = 0;
            DateTime currentdate;
            age = Convert.ToInt32(AgeYearZZZ.Text);
            currentdate = DateTime.Now;

            DateTime birthdate = currentdate.AddYears(age * -1);
            if (AgeMonthZZZ.Text != "")
            {
                months = Convert.ToInt32(AgeMonthZZZ.Text);
                birthdate = birthdate.AddMonths(months * -1);
            }

            dtPatientDOB.SelectedDate = birthdate;
        }

        protected void CGCalDOB_Click(object sender, EventArgs e)
        {
            if (txtCGYearZZZ.Text.Trim() == "")
            {
                txtCGYearZZZ.Text = "0";
                //MsgBuilder theBuilder = new MsgBuilder();
                //theBuilder.DataElements["Control"] = "Age (Years)";
                //IQCareMsgBox.Show("BlankTextBox", theBuilder, this);
                //txtCGYearZZZ.Focus();
                //return;
            }
            if (txtCGMonthZZZ.Text != "")
            {
                if ((Convert.ToInt32(txtCGMonthZZZ.Text) < 0) || (Convert.ToInt32(txtCGMonthZZZ.Text) > 11))
                {
                    MsgBuilder theMsg = new MsgBuilder();
                    theMsg.DataElements["Control"] = "Age (Month)";
                    IQCareMsgBox.Show("AgeMonthRange", theMsg, this);
                    return;
                }
            }
            else
                txtCGMonthZZZ.Text = "0";

            int age = 0;
            int months = 0;
            DateTime currentdate;
            age = Convert.ToInt32(txtCGYearZZZ.Text);
            currentdate = DateTime.Now;

            DateTime birthdate = currentdate.AddYears(age * -1);
            if (txtCGMonthZZZ.Text != "")
            {
                months = Convert.ToInt32(txtCGMonthZZZ.Text);
                birthdate = birthdate.AddMonths(months * -1);
            }

            dtCGDOB.SelectedDate = birthdate;
        }

        protected void dtCGDOB_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            Age COGAge = new Age((DateTime)e.NewDate, DateTime.Now);
            txtCGYearZZZ.Text = COGAge.Years.ToString();
            txtCGMonthZZZ.Text = COGAge.Months.ToString();
        }

        protected void txtHeight_TextChanged(object sender, EventArgs e)
        {
            GetBMI();
            cbWHOStage.Focus();
        }
        protected void txtWeight_TextChanged(object sender, EventArgs e)
        {
            GetBMI();
            txtHeight.Focus();
        }
        protected void GetBMI()
        {
            //formula for BMI = Weight (kg) / (Height (m) x Height (m))
            if ((txtWeight.Text != "") && (txtHeight.Text != ""))
            {
                decimal wgt = decimal.Parse(txtWeight.Text);
                decimal hgt = decimal.Parse(txtHeight.Text);
                decimal BMI = wgt / ((hgt / 100) * (hgt / 100));
                var thePos = BMI.ToString().IndexOf(".");
                var theVal = string.Empty;
                if (thePos > 0)
                    theVal = BMI.ToString().Substring(0, thePos + 2);
                else
                    theVal = BMI.ToString();
                txtBMICalculated.Text = theVal;
            }
        }
    }
}