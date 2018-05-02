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
using Application.Common;
using Application.Presentation;
using Interface.Scheduler;
using Interface.Security;
using Interface.Administration;
using System.Text;
using System.Collections.Generic;
using Interface.Clinical;
using Telerik.Web.UI;
using Touch;

namespace Touch.Custom_Forms
{
    public partial class frmCareEndedTouch : TouchUserControlBase
    {
        BindFunctions theBind = new BindFunctions();
        DataSet theDSXML = new DataSet();
        DataView theDVReq = new DataView();
        string patexitval = "0";
        string EmpId = "0";
        bool theConditional;
        int VisitID = 0;
        Panel theCheckBoxList = new Panel();
        protected void Page_Load(object sender, EventArgs e)
        {
            //register script
            String script = frmCareEnded_ScriptBlock.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
            if (IsPostBack) ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);

            
            Session["CurrentForm"] = this;
            Session["FormIsLoaded"] = true;
            VisitID = Convert.ToInt32(Session["PatientVisitIdCareended"]); ;
            ClearHiddenfield();
            btnSave.Attributes.Add("onclick", "return fnvalidate(" + hidID.ClientID + "," + hidradio.ClientID + "," + hidchkbox.ClientID + "," + hiddropdown.ClientID + ");");
            if (Session["IsFirstLoad"] != null)
            {
                if (Session["IsFirstLoad"].ToString() == "true")
                {
                    Session["OldData"] = null;
                    Tr_Deathreason.Visible = false;
                    Tr_Deathreason1.Visible = false;
                    PnlConFields.EnableViewState = false;
                    DIVCustomItem.EnableViewState = false;
                    Bind_Combo();
                    LoadPredefinedLabel_Field();
                    DataSet theDS;
                    IContactCare CareManager = (IContactCare)ObjectFactory.CreateInstance("BusinessProcess.Scheduler.BContactCare,BusinessProcess.Scheduler");
                    theDS = (DataSet)CareManager.GetFieldsforID(Convert.ToInt32(Session["PatientId"]), Convert.ToInt32(Session["AppLocationId"]), Convert.ToInt32(Session["SystemId"]), 1, 8);
                    txtDateLastContact.DbSelectedDate = ((DateTime)theDS.Tables[2].Rows[0]["Last_Ac_Con_date"]).ToString(Session["AppDateFormat"].ToString());
                    DisplaySavedFormData();
                    EnableDisableControls();
                    Session["IsFirstLoad"] = "false";
                }
                else
                {
                    LoadPredefinedLabel_Field();
                    RadComboBoxSelectedIndexChangedEventArgs args = new RadComboBoxSelectedIndexChangedEventArgs("", "", "", "");
                    if (ViewState["OldSelectedValue"] != null && cmbPatientExitReason.SelectedValue.ToString() == ViewState["OldSelectedValue"].ToString())
                    {
                        cmbPatientExitReason_SelectedIndexChanged(cmbPatientExitReason, args);
                        
                    }
                    if (ViewState["OldDeathSelectedValue"] != null && cmbDeathReason.SelectedValue.ToString() == ViewState["OldDeathSelectedValue"].ToString())
                    {
                        cmbDeathReason_SelectedIndexChanged(cmbDeathReason, args);
                    }
                }

            }

            base.Page_Load(sender, e);
           
            
        }

        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            updtFormUpdate.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "printScript", "PrintTouchForm('Registration Form', '" + this.ID + "');", true);
        }

        private void DisplaySavedFormData()
        {
            int i = 0;
            ICareEnded CEControl;

            if (VisitID == 0)
            {
                return;
            }

            CEControl = (ICareEnded)ObjectFactory.CreateInstance("BusinessProcess.Scheduler.BCareEnded,BusinessProcess.Scheduler");
            DataSet DSView = CEControl.GetSavedFormData(VisitID, Convert.ToInt32(Session["TechnicalAreaId"]));
            if (DSView.Tables[0].Rows[0]["DateLastContact"].ToString() != "")
            {
                txtDateLastContact.DbSelectedDate = Convert.ToDateTime(DSView.Tables[0].Rows[0]["DateLastContact"]).ToString(Session["AppDateFormat"].ToString());
            }
            ddinterviewer.SelectedValue = DSView.Tables[0].Rows[0]["EmployeeId"].ToString();

            Session["OldData"] = DSView;
            foreach (DataTable theDT in DSView.Tables)
            {
                if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
               
                    if (theDT.Rows[0]["TableName"].ToString().ToUpper() == "DTL_PATIENTCAREENDED")
                    {
                        cmbPatientExitReason.SelectedValue = theDT.Rows[0]["PatientExitReason"].ToString();
                        if ((theDT.Rows[0]["MissedAppDate"]).ToString() != "")
                        {
                            txtMissedAppDate.DbSelectedDate= Convert.ToDateTime(theDT.Rows[0]["MissedAppDate"]).ToString(Session["AppDateFormat"].ToString());
                        }
                        if ((theDT.Rows[0]["CareEndedDate"]).ToString() != "")
                        {
                            txtCareEndDate.DbSelectedDate = Convert.ToDateTime(theDT.Rows[0]["CareEndedDate"]).ToString(Session["AppDateFormat"].ToString());

                        }

                        cmbDeathReason.SelectedValue = theDT.Rows[0]["DeathReason"].ToString();

                        if ((theDT.Rows[0]["DeathDate"]).ToString() != "")
                        {
                            txtDeathDate.DbSelectedDate = Convert.ToDateTime(theDT.Rows[0]["DeathDate"]).ToString(Session["AppDateFormat"].ToString());

                        }

                        EventArgs s = new EventArgs();
                        RadComboBoxSelectedIndexChangedEventArgs args=new RadComboBoxSelectedIndexChangedEventArgs("","","","");
                       
                        cmbPatientExitReason_SelectedIndexChanged(cmbPatientExitReason, args);
                        cmbDeathReason_SelectedIndexChanged(cmbDeathReason,args);
                    }
                
            }
            
        }
        private void EnableDisableControls()
        {
            DataSet theControlsDS = (DataSet)Session["CareEndFields"];
            /////////Table -0
            foreach (DataRow theCntrlDR in theControlsDS.Tables[0].Rows)
            {
                DataView theCntrlDV = new DataView(theControlsDS.Tables[6]);
                theCntrlDV.RowFilter = "ConFieldId=" + theCntrlDR["FieldId"].ToString();

                if (theCntrlDV.Count > 0)
                {
                    foreach (Control x in DIVCustomItem.Controls)
                    {
                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                        {
                            foreach (Control thePnlCntrl in x.Controls)
                            {
                                if (thePnlCntrl.GetType().ToString() == "Telerik.Web.UI.RadButton")
                                {
                                    RadButton theCntrl = (RadButton)thePnlCntrl;
                                    string[] theID = theCntrl.ID.Split('-');
                                    if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                                    {
                                        if (theCntrl.Checked == true)
                                            HtmlCheckBoxSelect(theCntrl);
                                    }
                                }
                            }
                        }
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox")
                        {
                            RadComboBox theCntrl = (RadComboBox)x;
                            string[] theID = theCntrl.ID.Split('-');
                            if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                            {
                                EventArgs s = new EventArgs();
                                RadComboBoxSelectedIndexChangedEventArgs args = new RadComboBoxSelectedIndexChangedEventArgs("", "", "", "");
                                ddlSelectList_SelectedIndexChanged(theCntrl, args);
                            }
                        }
                        
                    }
                    foreach (Control x in PnlConFields.Controls)
                    {
                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                        {
                            foreach (Control thePnlCntrl in x.Controls)
                            {
                                if (thePnlCntrl.GetType().ToString() == "Telerik.Web.UI.RadButton")
                                {
                                    RadButton theCntrl = (RadButton)thePnlCntrl;
                                    string[] theID = theCntrl.ID.Split('-');
                                    if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                                    {
                                        if (theCntrl.Checked == true)
                                            HtmlCheckBoxSelect(theCntrl);
                                    }
                                }
                            }
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox")
                        {
                            RadComboBox theCntrl = (RadComboBox)x;
                            string[] theID = theCntrl.ID.Split('-');
                            if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                            {
                                EventArgs s = new EventArgs();
                                RadComboBoxSelectedIndexChangedEventArgs args = new RadComboBoxSelectedIndexChangedEventArgs("", "", "", "");
                                ddlSelectList_SelectedIndexChanged(theCntrl, args);
                            }
                        }
                        
                    }
                }
            }
            //////////////////Table - 1
            foreach (DataRow theCntrlDR in theControlsDS.Tables[1].Rows)
            {
                DataView theCntrlDV = new DataView(theControlsDS.Tables[6]);
                theCntrlDV.RowFilter = "ConFieldId=" + theCntrlDR["FieldId"].ToString();
                if (theCntrlDV.Count > 0)
                {
                    foreach (Control x in DIVCustomItem.Controls)
                    {
                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                        {
                            foreach (Control thePnlCntrl in x.Controls)
                            {
                                if (thePnlCntrl.GetType().ToString() == "Telerik.Web.UI.RadButton")
                                {
                                    RadButton theCntrl = (RadButton)thePnlCntrl;
                                    string[] theID = theCntrl.ID.Split('-');
                                    if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                                    {
                                        if (theCntrl.Checked == true)
                                            HtmlCheckBoxSelect(theCntrl);
                                    }
                                }
                            }
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox")
                        {
                            RadComboBox theCntrl = (RadComboBox)x;
                            string[] theID = theCntrl.ID.Split('-');
                            if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                            {
                                EventArgs s = new EventArgs();
                                RadComboBoxSelectedIndexChangedEventArgs args = new RadComboBoxSelectedIndexChangedEventArgs("", "", "", "");
                                ddlSelectList_SelectedIndexChanged(theCntrl, args);
                            }
                        }
                        

                    }
                    foreach (Control x in PnlConFields.Controls)
                    {
                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                        {
                            foreach (Control thePnlCntrl in x.Controls)
                            {
                                if (thePnlCntrl.GetType().ToString() == "Telerik.Web.UI.RadButton")
                                {
                                    RadButton theCntrl = (RadButton)thePnlCntrl;
                                    string[] theID = theCntrl.ID.Split('-');
                                    if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                                    {
                                        if (theCntrl.Checked == true)
                                            HtmlCheckBoxSelect(theCntrl);
                                    }
                                }
                            }
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox")
                        {
                            RadComboBox theCntrl = (RadComboBox)x;
                            string[] theID = theCntrl.ID.Split('-');
                            if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                            {
                                EventArgs s = new EventArgs();
                                RadComboBoxSelectedIndexChangedEventArgs args = new RadComboBoxSelectedIndexChangedEventArgs("", "", "", "");
                                ddlSelectList_SelectedIndexChanged(theCntrl, args);
                            }
                        }
                        
                    }


                    foreach (Control x in DIVCustomItem.Controls)
                    {
                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                        {
                            foreach (Control thePnlCntrl in x.Controls)
                            {
                                if (thePnlCntrl.GetType().ToString() == "Telerik.Web.UI.RadButton")
                                {
                                    RadButton theCntrl = (RadButton)thePnlCntrl;
                                    string[] theID = theCntrl.ID.Split('-');
                                    if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                                    {
                                        if (theCntrl.Checked == true)
                                            HtmlCheckBoxSelect(theCntrl);
                                    }
                                }
                            }
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox")
                        {
                            RadComboBox theCntrl = (RadComboBox)x;
                            string[] theID = theCntrl.ID.Split('-');
                            if (theID[1] == theCntrlDR["FieldName"].ToString() && theID[3] == theCntrlDR["FieldId"].ToString())
                            {
                                EventArgs s = new EventArgs();
                                RadComboBoxSelectedIndexChangedEventArgs args = new RadComboBoxSelectedIndexChangedEventArgs("", "", "", "");
                                ddlSelectList_SelectedIndexChanged(theCntrl, args);
                            }
                        }
                        
                    }





                }
            }
        }
        public void HtmlCheckBoxSelect(object theObj)
        {
            CheckBox theButton = ((CheckBox)theObj);
            string[] theControlId = theButton.ID.ToString().Split('-');
            DataSet theDS = (DataSet)Session["CareEndFields"];

            //DataSet theDS = (DataSet)Session["AllData"];
            int theValue = 0;
            if (theButton.Checked == true)
                theValue = 1;
            else
                theValue = 0;

            foreach (DataRow theDR in theDS.Tables[6].Rows)
            {
                foreach (Control x in DIVCustomItem.Controls)
                {
                    if (x.ID != null)
                    {
                        string[] theIdent = x.ID.Split('-');
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadTextBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadTextBox)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                            {
                                ((RadTextBox)x).Enabled = false;
                                ((RadTextBox)x).Text = "";
                            }
                        }
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadNumericTextBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadNumericTextBox)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                            {
                                ((RadNumericTextBox)x).Enabled = false;
                                ((RadNumericTextBox)x).Text = "";
                            }
                        }
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadComboBox)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                            {
                                ((RadComboBox)x).Enabled = false;
                                ((RadComboBox)x).SelectedValue = "0";
                            }
                        }

                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel" && theIdent[0] == "Pnl")
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1" && theDR["FieldName"].ToString() == theIdent[1].ToString())
                                ((Panel)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0" && theDR["FieldName"].ToString() == theIdent[1].ToString())
                            {
                                ((Panel)x).Enabled = false;
                            }
                        }


                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Image" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((Image)x).Visible = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                                ((Image)x).Visible = false;
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadButton" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadButton)x).Visible = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                                ((RadButton)x).Visible = false;
                        }

                        
                    }
                }

                /////////////////////Child Panel/////////////////

                foreach (Control x in PnlConFields.Controls)
                {
                    if (x.ID != null)
                    {
                        string[] theIdent = x.ID.Split('-');
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadTextBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadTextBox)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                            {
                                ((RadTextBox)x).Enabled = false;
                                ((RadTextBox)x).Text = "";
                            }
                        }
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadNumericTextBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadNumericTextBox)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                            {
                                ((RadNumericTextBox)x).Enabled = false;
                                ((RadNumericTextBox)x).Text = "";
                            }
                        }
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadComboBox)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                            {
                                ((RadComboBox)x).Enabled = false;
                                ((RadComboBox)x).SelectedValue = "0";
                            }
                        }

                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel" && theIdent[0] == "Pnl")
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((Panel)x).Enabled = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                            {
                                ((Panel)x).Enabled = false;
                            }
                        }


                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Image" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((Image)x).Visible = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                                ((Image)x).Visible = false;
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadButton" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadButton)x).Visible = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                                ((RadButton)x).Visible = false;
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadButton" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "1")
                                ((RadButton)x).Visible = true;
                            else if (theControlId[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theControlId[5].ToString() && theValue.ToString() == "0")
                                ((RadButton)x).Visible = false;
                        }
                    }
                }


                ////////////////////////////////////////////////

            }
        }

        private void Bind_Combo()
        {
            string ObjFactoryParameter = "BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical";
            BindFunctions theBindManager = new BindFunctions();
            theDSXML.ReadXml(MapPath(@"~\XMLFiles\AllMasters.con"));
            Session["XMLTables"] = theDSXML;
            IQCareUtils theUtils = new IQCareUtils();
            Session["TechnicalAreaId"] = TouchGlobals.ModuleId;
            ICareEnded CEControl = (ICareEnded)ObjectFactory.CreateInstance("BusinessProcess.Scheduler.BCareEnded,BusinessProcess.Scheduler");
            DataSet theDS = CEControl.GetDynamicControl(Convert.ToInt32(Session["TechnicalAreaId"]));
            Session["CareEndFields"] = theDS;
            theBind.BindCombo(cmbPatientExitReason, theDS.Tables[2], "Name", "ID");

            IIQTouchPatientRegistration ptnMgr = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance(ObjFactoryParameter);
            string sqlQuery1;
            sqlQuery1 = string.Format("SELECT EmployeeId,FirstName+ ' '+LastName[FirstName] FROM Mst_Employee where DeleteFlag=0");
            DataTable DT = ptnMgr.ReturnDatatableQuery(sqlQuery1);
            theBind.BindCombo(ddinterviewer, DT, "FirstName", "EmployeeId");

            DisplayDeathreasonpnl();

        }
        private void DisplayDeathreasonpnl()
        {
            ICareEnded CEControl = (ICareEnded)ObjectFactory.CreateInstance("BusinessProcess.Scheduler.BCareEnded,BusinessProcess.Scheduler");
            DataSet dsCareEndedDeath = CEControl.GetCareEndedDeathReason(Convert.ToInt32(Session["TechnicalAreaId"]));
            BindFunctions theBindDeath = new BindFunctions();

            theBind.BindCombo(cmbDeathReason, dsCareEndedDeath.Tables[0], "Name", "ID");

        }
        

        protected void cmbPatientExitReason_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ViewState["OldSelectedValue"] = cmbPatientExitReason.SelectedValue;
            DataSet theDS = (DataSet)Session["CareEndFields"];
            PnlConFields.Controls.Clear();
            if (Convert.ToInt32(cmbPatientExitReason.SelectedValue) > 0)
            {

                if (cmbPatientExitReason.SelectedValue.ToString() == "93" || cmbPatientExitReason.SelectedValue.ToString() == "419")
                {
                    Tr_Deathreason.Visible = false;
                    Tr_Deathreason1.Visible = true;
                    if (cmbDeathReason.SelectedValue.ToString() != "" && Convert.ToInt32(cmbDeathReason.SelectedValue) != 0)
                    {
                        cmbDeathReason_SelectedIndexChanged(this, e);
                    }
                    
                }
                else
                {
                    Tr_Deathreason.Visible = false;
                    Tr_Deathreason1.Visible = false;
                    cmbDeathReason.SelectedIndex = 0;
                }


                if (cmbPatientExitReason.SelectedValue.ToString() == "91" || cmbPatientExitReason.SelectedValue.ToString() == "416" )
                {
                    DataSet thedS;
                    IContactCare CareEnddate = (IContactCare)ObjectFactory.CreateInstance("BusinessProcess.Scheduler.BContactCare,BusinessProcess.Scheduler");
                    thedS = (DataSet)CareEnddate.GetCareEndDate(Convert.ToInt32(Request.QueryString["PatientID"]), "");
                    if (thedS.Tables[0].Rows[0][0].ToString() != "")
                    {
                        txtCareEndDate.DbSelectedDate = String.Format("{0:dd-MMM-yyyy}", thedS.Tables[0].Rows[0][0]);
                        GblIQCare.strfoll = "1";
                    }
                    
                }
                else if((cmbPatientExitReason.SelectedValue.ToString() == "115")||(cmbPatientExitReason.SelectedValue.ToString() == "118"))
                {
                    if (this.txtDateLastContact.SelectedDate != null)
                    {
                        this.txtCareEndDate.DbSelectedDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(this.txtDateLastContact.SelectedDate.Value));

                    }
                
                
               }
             else if ((cmbPatientExitReason.SelectedValue.ToString() == "92")||(cmbPatientExitReason.SelectedValue.ToString() == "114"))
             {
                 if (this.txtDateLastContact.SelectedDate != null)
                 {
                     this.txtCareEndDate.DbSelectedDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(this.txtDateLastContact.SelectedDate.Value));

                 }
             }
             else if (cmbPatientExitReason.SelectedValue.ToString() == "93" || cmbPatientExitReason.SelectedValue.ToString() == "419")
             {
                 if (this.txtDeathDate.SelectedDate != null)
                 {
                     this.txtCareEndDate.DbSelectedDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(this.txtDeathDate.SelectedDate.Value));

                 }
             }
            else
             {
                 if (GblIQCare.strfoll.ToString() == "1")
                 {
                     GblIQCare.strfoll = "0";
                 }
                 
             }

                DataView theDV = new DataView(theDS.Tables[1].Copy());
                theDVReq = new DataView(theDS.Tables[3].Copy());
                theDV.RowFilter = "SectionId=" + cmbPatientExitReason.SelectedValue.ToString();
                theDV.Sort = "FieldId asc";
                DataTable theDT = theDV.ToTable();

                ViewState["cmbbox"] = theDT;
                if (theDT.Rows.Count > 0)
                {
                    PnlConFields.Visible = true;
                    cmbboxLoad(theDT, theDVReq, patexitval);
                }
                if (ViewState["TopControl"] != null)
                {
                    if (((DataTable)ViewState["TopControl"]).Rows.Count > 0)
                        DIVCustomItem.Visible = true;
                    LoadPredefinedLabel_Field();
                }
            }
        }
        private void cmbboxLoad(DataTable theDT, DataView DVReq, string patval)
        {
            DataView DVReqCurr = new DataView();
            if (theDT.Rows.Count > 0)
            {
                DataSet theDS = (DataSet)Session["CareEndFields"];
                int i = 0;
                PnlConFields.EnableViewState = false;
                foreach (DataRow theDR in theDT.Rows)
                {
                    #region "CheckConditionalFields"
                    DataView theDVConditionalField = new DataView(theDS.Tables[6]);
                    theDVConditionalField.RowFilter = "ConFieldId=" + theDR["FieldID"].ToString() + " and ConFieldPredefined=" + theDR["Predefined"].ToString();
                    theDVConditionalField.Sort = "Seq asc";
                    if (theDVConditionalField.Count > 0)
                        theConditional = true;
                    else
                        theConditional = false;
                    #endregion

                    DVReqCurr = DVReq;

                    

                    PnlConFields.Controls.Add(new LiteralControl("<tr>"));
                   
                    LoadFieldTypeControl(DVReqCurr, PnlConFields, theDR["FeatureName"].ToString(), theDR["SectionId"].ToString(), "", theDR["FieldId"].ToString(), theDR["FieldLabel"].ToString(),
                      theDR["FieldName"].ToString(), theDR["ControlId"].ToString(), theDR["SavingTable"].ToString(), theDR["BindingTable"].ToString(), theDR["PreDefined"].ToString(), true);
                    PnlConFields.Controls.Add(new LiteralControl("</tr>"));
                    if (i == 1)
                    {
                        
                        i = 0;
                    }
                    else
                    {
                        i = i + 1;
                    }
                    #region "Create Conditional Fields"
                    if (theConditional == true)
                    {
                        for (int row = 0; row < theDVConditionalField.Count; row++)
                        {
                            DVReqCurr = DVReq;

                            DVReqCurr.RowFilter = "FieldID=" + theDR["FieldId"].ToString() + " and Predefined =" + theDR["PreDefined"].ToString() + " ";

                            PnlConFields.Controls.Add(new LiteralControl("<tr>"));
                            LoadFieldTypeControl(DVReqCurr, PnlConFields, theDVConditionalField[row]["FeatureName"].ToString(), "", "", theDVConditionalField[row]["FieldId"].ToString(), theDVConditionalField[row]["FieldLabel"].ToString(),
                              theDVConditionalField[row]["FieldName"].ToString(), theDVConditionalField[row]["ControlId"].ToString(), theDVConditionalField[row]["PdfTableName"].ToString(), theDVConditionalField[row]["BindSource"].ToString(), theDVConditionalField[row]["PreDefined"].ToString(), false);
                            PnlConFields.Controls.Add(new LiteralControl("</tr>"));
                            if (i == 1)
                            {
                                
                                i = 0;
                            }
                            else
                            {
                                i = i + 1;
                            }
                        }
                    }
                    #endregion
                }

                if (ViewState["TopControl"] != null)
                {
                    EmpId = ddinterviewer.SelectedValue.ToString();
                    if (((DataTable)ViewState["TopControl"]).Rows.Count > 0)
                        DIVCustomItem.Visible = true;
                    LoadPredefinedLabel_Field();
                }

               

            }
           


        }
        private void SetBusinessrule(Control theLabel, Control theControl, Int32 FieldID)
        {
            int i = 0;
            DataTable theDT = ((DataSet)Session["CareEndFields"]).Tables[3];
            DataView theDVBus = new DataView(theDT.Copy());
            theDVBus.RowFilter = "FieldId=" + FieldID.ToString();
            theDVBus.Sort = "FieldId asc";
            theDT = theDVBus.ToTable();


            string theOnKeyUp = "";
            foreach (DataRow DR in theDT.Rows)
            {
                if (DR["Name"].ToString() == "Required Field")
                {
                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 1 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidID.Value == "")
                        {
                            hidID.Value = theControl.ID;
                        }
                        else
                        {
                            hidID.Value = hidID.Value + "%" + theControl.ID;
                        }
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }
                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 2 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidID.Value == "")
                        {
                            hidID.Value = theControl.ID;
                        }
                        else
                        {
                            hidID.Value = hidID.Value + "%" + theControl.ID;
                        }
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 3 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidID.Value == "")
                        {
                            hidID.Value = theControl.ID;
                        }
                        else
                        {
                            hidID.Value = hidID.Value + "%" + theControl.ID;
                        }
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 4 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hiddropdown.Value == "")
                        {
                            hiddropdown.Value = theControl.ID;
                        }
                        else
                        {
                            hiddropdown.Value = hiddropdown.Value + "%" + theControl.ID;
                        }
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 5 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidID.Value == "")
                        {
                            hidID.Value = theControl.ID;
                        }
                        else
                        {
                            hidID.Value = hidID.Value + "%" + theControl.ID;
                        }
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }
                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 6 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidradio.Value == "")
                        {
                            hidradio.Value = theControl.ID;
                            i = 1;
                        }
                        else
                        {
                            hidradio.Value = hidradio.Value + "%" + theControl.ID;

                            i = 0;
                        }


                        
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 7 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidchkbox.Value == "")
                        {
                            hidchkbox.Value = theControl.ID;
                        }
                        else
                        {
                            hidchkbox.Value = hidchkbox.Value + "%" + theControl.ID;
                        }
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 8 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidID.Value == "")
                        {
                            hidID.Value = theControl.ID;
                        }
                        else
                        {
                            hidID.Value = hidID.Value + "%" + theControl.ID;
                        }

                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 9 && DR["Name"].ToString() == "Required Field")
                    {
                        if (hidcheckbox.Value == "")
                        {
                            hidcheckbox.Value = theControl.ID;


                            

                            foreach (Control x in PnlConFields.Controls)
                            {
                                if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                                {
                                    if (((Panel)x).Enabled == false)
                                    {
                                        hidcheckbox.Value = "";


                                    }
                                }
                            }
                            foreach (Control x in DIVCustomItem.Controls)
                            {
                                if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel")
                                {
                                    if (((Panel)x).Enabled == false)
                                    {
                                        hidcheckbox.Value = "";


                                    }
                                }
                            }

                        }
                        else
                        {
                            hidcheckbox.Value = hidcheckbox.Value + "%" + theControl.ID;
                        }
                        ((Label)theLabel).CssClass = ((Label)theLabel).CssClass + " required";
                        ((Label)theLabel).Text = "*" + ((Label)theLabel).Text;
                    }
                }

                else if (DR["Name"].ToString() == "Data Quality Check")
                {
                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 1 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidIDQty.Value == "")
                        {
                            hidIDQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidIDQty.Value = hidIDQty.Value + "%" + theControl.ID;
                        }

                        

                    }
                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 2 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidIDQty.Value == "")
                        {
                            hidIDQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidIDQty.Value = hidIDQty.Value + "%" + theControl.ID;
                        }
                                     
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 3 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidIDQty.Value == "")
                        {
                            hidIDQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidIDQty.Value = hidIDQty.Value + "%" + theControl.ID;
                        }
                        



                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 4 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidIDQty.Value == "")
                        {
                            hidIDQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidIDQty.Value = hidIDQty.Value + "%" + theControl.ID;
                        }
                       



                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 5 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidIDQty.Value == "")
                        {
                            hidIDQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidIDQty.Value = hidIDQty.Value + "%" + theControl.ID;
                        }
                       

                    }
                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 6 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidradioQty.Value == "")
                        {
                            hidradioQty.Value = theControl.ID;
                            i = 1;
                        }
                        else
                        {
                            hidradioQty.Value = hidradioQty.Value + "%" + theControl.ID;

                            i = 0;
                        }


                        

                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 7 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidchkboxQty.Value == "")
                        {
                            hidchkboxQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidchkboxQty.Value = hidchkboxQty.Value + "%" + theControl.ID;
                        }
                        

                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 8 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidIDQty.Value == "")
                        {
                            hidIDQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidIDQty.Value = hidIDQty.Value + "%" + theControl.ID;
                        }

                      

                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["ControlId"]) == 9 && DR["Name"].ToString() == "Data Quality Check")
                    {
                        if (hidcheckboxQty.Value == "")
                        {
                            hidcheckboxQty.Value = theControl.ID;
                        }
                        else
                        {
                            hidcheckboxQty.Value = hidcheckboxQty.Value + "%" + theControl.ID;
                        }
                       

                    }
                }
                else
                {
                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["Id"]) == 2)
                    {
                        theOnKeyUp = theOnKeyUp + ";checkMax('" + ((RadNumericTextBox)theControl).ClientID + "', '" + ((RadNumericTextBox)theControl).Text + "', '" + DR["value"].ToString() + "')";
                        theOnKeyUp = theOnKeyUp + ";chkDecimal('" + ((RadNumericTextBox)theControl).ClientID + "')";
                        ((TextBox)theControl).Attributes.Add("onkeyup", theOnKeyUp);
                    }

                    if (Convert.ToInt32(DR["FieldId"]) == FieldID && Convert.ToInt32(DR["Id"]) == 3)
                    {
                        theOnKeyUp = theOnKeyUp + ";checkMin('" + ((RadNumericTextBox)theControl).ClientID + "', '" + ((RadNumericTextBox)theControl).Text + "', '" + DR["value"].ToString() + "');";
                        theOnKeyUp = theOnKeyUp + ";chkNumber('" + ((RadNumericTextBox)theControl).ClientID + "');";
                        ((TextBox)theControl).Attributes.Add("onkeyup", theOnKeyUp);
                    }

                }

                //return false;
            }
        }
        private void LoadFieldTypeControl(DataView ReqDv, Panel thePnl, string FeatureName, string SectionId, string SectionName, string CFieldId, string FieldLabel, string FieldName, string ControlID, string SavingTable, string BindingTable, string PreDefined, bool theEnable)
        {
            IQCareUtils theUtils = new IQCareUtils();
            BindFunctions BindManager = new BindFunctions();
            DataView theDV;
            DataView theDVModeCode = new DataView();
            DataView theDVModCode = new DataView();
            DataSet theDSxml = (DataSet)Session["XMLTables"];
            DataTable ReqDT = new DataTable();
            if (ReqDv.Table != null)
            {
                ReqDT = ReqDv.ToTable();
                ViewState["BusRule"] = ReqDT;
            }
            
            if (ControlID == "1") ///SingleLine Text Box
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                //TextBox theSingleText = new TextBox();
                RadTextBox theSingleText = new RadTextBox();
                theSingleText.ID = "TXT-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theSingleText.Skin = "MetroTouch";

                theSingleText.MaxLength = 50;
                theSingleText.Enabled = theEnable;
                thePnl.Controls.Add(theSingleText);
                SetBusinessrule(theLbl, theSingleText, Convert.ToInt32(CFieldId));

                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                                theSingleText.Text = theDT.Rows[0][FieldName].ToString();
                            }
                        }
                    }

                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion

            }
            else if (ControlID == "2") ///DecimalTextBox
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));

                //TextBox theSingleDecimalText = new TextBox();
                RadNumericTextBox theSingleDecimalText = new RadNumericTextBox();
                theSingleDecimalText.ID = "TXTDCM-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theSingleDecimalText.Skin = "MetroTouch";
                theSingleDecimalText.NumberFormat.DecimalDigits = 2;
                theSingleDecimalText.MaxLength = 50;
                theSingleDecimalText.Enabled = theEnable;
                thePnl.Controls.Add(theSingleDecimalText);
                SetBusinessrule(theLbl, theSingleDecimalText, Convert.ToInt32(CFieldId));

                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                                theSingleDecimalText.Text = theDT.Rows[0][FieldName].ToString();
                            }
                        }
                    }
                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion

            }
            else if (ControlID == "3")   /// Numeric (Integer)
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                
                //TextBox theNumberText = new TextBox();
                RadNumericTextBox theNumberText = new RadNumericTextBox();
                theNumberText.ID = "TXTNUM-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theNumberText.MaxLength = 9;
                theNumberText.Skin = "MetroTouch";
                theNumberText.Enabled = theEnable;
                thePnl.Controls.Add(theNumberText);
               
                SetBusinessrule(theLbl, theNumberText, Convert.ToInt32(CFieldId));

                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                                theNumberText.Text = theDT.Rows[0][FieldName].ToString();
                            }
                        }
                    }
                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion

            }

            else if (ControlID == "4") /// Dropdown
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                //DropDownList ddlSelectList = new DropDownList();
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                RadComboBox ddlSelectList = new RadComboBox();
                ddlSelectList.Skin = "MetroTouch";
                ddlSelectList.Width = Unit.Pixel(200);
                
                ddlSelectList.ID = "SelectList-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theDV = new DataView(theDSxml.Tables[BindingTable]);
                if (BindingTable == "Mst_ModDecode")
                {
                    theDVModCode = new DataView(theDSxml.Tables["mst_modcode"].Copy());
                    theDVModCode.RowFilter = "Name='" + FieldName.ToString() + "' and (DeleteFlag=0 or DeleteFlag is null)";
                    DataTable theDTModCode = theDVModCode.ToTable();
                    if (theDVModCode.Count > 0)
                    {
                        theDVModeCode = new DataView(theDSxml.Tables["mst_modDecode"].Copy());
                        theDVModeCode.RowFilter = "CodeId=" + theDTModCode.Rows[0]["CodeId"].ToString() + " and (DeleteFlag=0 or DeleteFlag is null)";
                        if (theDVModeCode.Count > 0)
                        {
                            DataTable theDT = theDVModeCode.ToTable();
                            ddlSelectList.DataSource = null;
                            BindManager.BindCombo(ddlSelectList, theDT, "Name", "Id");
                        }
                        
                    }
                }
                else if (BindingTable == "mst_LPTF")
                {
                    theDVModCode = new DataView(theDSxml.Tables["mst_LPTF"].Copy());
                    theDVModCode.RowFilter = "Name='" + FieldName.ToString() + "' and (DeleteFlag=0 or DeleteFlag is null) and SystemId=3";
                    DataTable theDTModCode = theDVModCode.ToTable();
                    if (theDVModCode.Count > 0)
                    {
                        theDVModeCode = new DataView(theDSxml.Tables["mst_modDecode"].Copy());
                        theDVModeCode.RowFilter = "CodeId=" + theDTModCode.Rows[0]["CodeId"].ToString() + " and (DeleteFlag=0 or DeleteFlag is null)";
                        if (theDVModeCode.Count > 0)
                        {
                            DataTable theDT = theDVModeCode.ToTable();
                            ddlSelectList.DataSource = null;
                            BindManager.BindCombo(ddlSelectList, theDT, "Name", "Id");
                        }
                        
                    }
                }

                else
                {
                    DataTable theDT = new DataTable();
                    theDV = new DataView(theDSxml.Tables[BindingTable].Copy());
                    if (BindingTable == "Mst_Decode")
                    {
                        theDVModCode = new DataView(theDSxml.Tables["mst_code"].Copy());
                        theDVModCode.RowFilter = "Name='" + FieldName.ToString() + "' and (DeleteFlag=0 or DeleteFlag is null)";
                        DataTable theDTModCode = theDVModCode.ToTable();
                        theDV.RowFilter = "CodeId=" + theDTModCode.Rows[0]["CodeId"].ToString() + " and (DeleteFlag=0 or DeleteFlag is null)";
                    }
                    else
                    {
                        
                        theDV.RowFilter = "SystemId=3 and (DeleteFlag=0 or DeleteFlag is null)";
                    }
                    if (theDV.Count > 0)
                    {
                        theDT = theDV.ToTable();
                        ddlSelectList.DataSource = null;
                        BindManager.BindCombo(ddlSelectList, theDT, "Name", "Id");
                    }
                    
                }
                ddlSelectList.Width = 180;
                ddlSelectList.Enabled = theEnable;

                if (theConditional == true && theEnable == true)
                {
                    ddlSelectList.AutoPostBack = true;
                    //ddlSelectList.SelectedIndexChanged += new EventHandler(ddlSelectList_SelectedIndexChanged);
                    ddlSelectList.SelectedIndexChanged+=new RadComboBoxSelectedIndexChangedEventHandler(ddlSelectList_SelectedIndexChanged);
                }
                thePnl.Controls.Add(ddlSelectList);

                SetBusinessrule(theLbl, ddlSelectList, Convert.ToInt32(CFieldId));


                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                                foreach (DataRow DR in theDT.Rows)
                                {
                                    if (DR[FieldName].ToString() != "")
                                    {
                                        //ddlSelectList.SelectedValue = theDT.Rows[0][FieldName].ToString();
                                        ddlSelectList.SelectedValue = DR[FieldName].ToString();

                                    }
                                }

                            }
                        }
                    }
                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion
            }
            else if (ControlID == "5") ///Date
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                //theDateText_Load.Load += new EventHandler(theDateText_Load);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));

                RadDatePicker theDateText = new RadDatePicker();
                theDateText.ViewStateMode = ViewStateMode.Enabled;
                theDateText.ID = "TXTDT-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                //theDateText.ClientIDMode = ClientIDMode.Static;
                theDateText.AutoPostBack = false;
                theDateText.Skin = "MetroTouch";
                theDateText.Width = Unit.Pixel(200);

             

                theDateText.Calendar.ShowRowHeaders = false;
                theDateText.Calendar.UseColumnHeadersAsSelectors = false;
                theDateText.Calendar.UseRowHeadersAsSelectors = false;


                theDateText.DatePopupButton.ImageUrl = ""; ;
                theDateText.DatePopupButton.HoverImageUrl = "";
                theDateText.DateInput.DateFormat = "dd/MM/yyyy";
                theDateText.DateInput.DisplayDateFormat = "dd MMM yyyy";
                thePnl.Controls.Add(theDateText);
                SetBusinessrule(theLbl, theDateText, Convert.ToInt32(CFieldId));

                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                                
                                foreach (DataRow DR in theDT.Rows)
                                {
                                    if (DR[FieldName].ToString() != "")
                                    {
                                        //theDateText.Text = Convert.ToDateTime(DR[FieldName].ToString());

                                        theDateText.DbSelectedDate = Convert.ToDateTime(DR[FieldName]).ToString(Session["AppDateFormat"].ToString());

                                    }
                                }
                            }
                        }
                    }
                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion

            }

            else if (ControlID == "6")  /// Radio Button
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));

                RadButton theYesNoRadio1 = new RadButton();
                theYesNoRadio1.ID = "RADIO1-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theYesNoRadio1.ToggleType = ButtonToggleType.Radio;
                theYesNoRadio1.AutoPostBack = false;
                theYesNoRadio1.Text = "Yes";
                theYesNoRadio1.Skin = "MetroTouch";
                theYesNoRadio1.GroupName = "" + FieldName + "";
                theYesNoRadio1.Visible = theEnable;

               

                thePnl.Controls.Add(theYesNoRadio1);
                thePnl.Controls.Add(new LiteralControl("<label align='labelright' id='lblYes-" + CFieldId + "'>&nbsp;&nbsp;</label>"));

                SetBusinessrule(theLbl, theYesNoRadio1, Convert.ToInt32(CFieldId));

                RadButton theYesNoRadio2 = new RadButton();
                
                theYesNoRadio2.ID = "RADIO2-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;


                theYesNoRadio2.ToggleType = ButtonToggleType.Radio;
                theYesNoRadio2.Text = "No";
                theYesNoRadio2.AutoPostBack = false;
                theYesNoRadio2.Skin = "MetroTouch";
                theYesNoRadio2.GroupName = "" + FieldName + "";
                theYesNoRadio2.Visible = theEnable;

                
                thePnl.Controls.Add(theYesNoRadio2);

                thePnl.Controls.Add(new LiteralControl("<label align='labelright' id='lblNo-" + CFieldId + "'></label>"));
                SetBusinessrule(theLbl, theYesNoRadio2, Convert.ToInt32(CFieldId));

                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                                

                                foreach (DataRow DR in theDT.Rows)
                                {
                                    if (DR[FieldName].ToString() != "")
                                    {
                                        if (DR[FieldName].ToString() == "True")
                                        {
                                            theYesNoRadio1.Checked = true;

                                        }
                                        else
                                        {
                                            theYesNoRadio2.Checked = true;
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion
            }
            else if (ControlID == "7") //Checkbox
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));

                RadButton theChk = new RadButton();
                //HtmlInputCheckBox theChk = new HtmlInputCheckBox();
                theChk.ID = "Chk-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theChk.ToggleType = ButtonToggleType.CheckBox;
                theChk.Skin = "MetroTouch";
                theChk.AutoPostBack = false;
                theChk.ViewStateMode = ViewStateMode.Enabled;
                theChk.Visible = theEnable;
                thePnl.Controls.Add(theChk);
                SetBusinessrule(theLbl, theChk, Convert.ToInt32(CFieldId));

                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                               

                                foreach (DataRow DR in theDT.Rows)
                                {
                                    if (DR[FieldName].ToString() != "")
                                    {
                                        if (DR[FieldName].ToString() == "1")
                                        {
                                            theChk.Checked = true;
                                        }
                                        else
                                        {
                                            theChk.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion
            }
            else if (ControlID == "8")  /// MultiLine TextBox
            {
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));

                RadTextBox theMultiText = new RadTextBox();
                //TextBox theMultiText = new TextBox();
                theMultiText.ID = "TXTMulti-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theMultiText.Width = Unit.Pixel(400);
                theMultiText.Skin = "MetroTouch";
                theMultiText.TextMode = InputMode.MultiLine;
                theMultiText.MaxLength = 200;
                thePnl.Controls.Add(theMultiText);
                theMultiText.Enabled = theEnable;
                SetBusinessrule(theLbl, theMultiText, Convert.ToInt32(CFieldId));

                #region "Fill Old Data"
                DataSet theDS = (DataSet)Session["OldData"];
                if (theDS != null)
                {
                    foreach (DataTable theDT in theDS.Tables)
                    {
                        if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                        {
                            if (theDT.Rows[0]["TableName"].ToString().ToUpper() == SavingTable.ToUpper())
                            {
                                

                                foreach (DataRow DR in theDT.Rows)
                                {
                                    if (DR[FieldName].ToString() != "")
                                    {
                                        theMultiText.Text = DR[FieldName].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                #endregion
            }

            else if (ControlID == "9") ///  MultiSelect List 
            {
                
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                Label theLbl = new Label();
                theLbl.ID = "LBL-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theLbl.Text = FieldLabel + " : ";
                theLbl.CssClass = "bold";
                thePnl.Controls.Add(theLbl);
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
                PnlConFields.Controls.Add(new LiteralControl("<td   width='50%'>"));
                RadComboBox radChkList = new RadComboBox();
                radChkList.ID = "PnlMulti-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                radChkList.Skin = "MetroTouch";
                radChkList.AutoPostBack = false;
                radChkList.ViewStateMode = ViewStateMode.Enabled;
                radChkList.CheckBoxes = true;
                radChkList.Width = Unit.Pixel(350);
                Panel PnlMulti = new Panel();
                PnlMulti.CssClass = "checkbox";
                //PnlMulti.ID = "Pnl-" + FieldName + "-" + SavingTable + "-" + CFieldId + "-" + thePnl.ID;
                theDV = new DataView(theDSxml.Tables[BindingTable]);

                DataTable theBindDT = new DataTable();
                DataSet theCareEndFields = (DataSet)Session["CareEndFields"];
                DataView theXMLDV = new DataView(theDSxml.Tables[BindingTable]);

                DataView convw = new DataView(theCareEndFields.Tables[0]);
                convw.RowFilter = "ControlId=" + ControlID.ToString() + " and FieldId=" + CFieldId.ToString();
                if (theXMLDV.Table.Rows.Count > 0)
                {
                    DataView theFView = new DataView();
                    if (thePnl.ID == "DIVCustomItem")
                    {
                        if (convw.Count > 0 && theEnable == false)
                        {
                            theFView = new DataView(theCareEndFields.Tables[6]);
                        }
                        else
                        {
                            theFView = new DataView(theCareEndFields.Tables[0]);

                        }

                    }
                    else
                    {

                        if (convw.Count > 0)
                        {
                            theFView = new DataView(theCareEndFields.Tables[6]);
                        }
                        else
                        {
                            theFView = new DataView(theCareEndFields.Tables[5]);

                        }



                    }

                    
                    theFView.RowFilter = "FieldId=" + CFieldId;
                    if (theFView.Count > 0)
                    {
                        if (convw.Count > 0)
                        {

                            for (int i = 0; i < theFView.Table.Columns.Count; i++)
                            {
                                if (theFView.Table.Columns[i].ToString() == "Codeid")
                                {
                                    if (theFView[0]["Codeid"] != DBNull.Value)
                                    {
                                         theDV.RowFilter = "Codeid = " + theFView[0]["Codeid"].ToString();
                                         theBindDT = theDV.ToTable();

                                    }


                                }

                                else if (theFView[0]["FilterColumn"] != DBNull.Value)
                                {
                                   
                                    theDV.RowFilter = theFView[0]["FilterColumn"].ToString() + " = " + theFView[0]["CategoryId"].ToString();
                                    theBindDT = theDV.ToTable();

                                }

                            }


                        }
                        else
                        {
                            if (theFView[0]["FilterColumn"] != DBNull.Value)
                            {
                               
                                theDV.RowFilter = theFView[0]["FilterColumn"].ToString() + " = " + theFView[0]["CategoryId"].ToString();
                                theBindDT = theDV.ToTable();

                            }
                            else
                            {
                                theBindDT = theXMLDV.ToTable();
                            }


                        }


                    }
                    else
                    {
                        DataView theCView = new DataView(theCareEndFields.Tables[1]);
                        theCView.RowFilter = "FieldId=" + CFieldId;
                        if (theCView.Count > 0)
                        {
                            if (theCView[0]["FilterColumn"] != DBNull.Value)
                            {
                               
                                theDV.RowFilter = theCView[0]["FilterColumn"].ToString() + " = " + theCView[0]["CategoryId"].ToString();
                                theBindDT = theDV.ToTable();

                            }
                            else
                            {
                                theBindDT = theXMLDV.ToTable();
                            }
                        }
                    }
                }
                
                if (theBindDT != null)
                {
                    
                    foreach (DataRow row in theBindDT.Rows)
                    {
                        string itemName = row["Name"].ToString();
                        string itemVal = row["ID"].ToString();
                        RadComboBoxItem item = new RadComboBoxItem(itemName, itemVal);

                        if (CheckedMultiselectlist(itemVal, SavingTable))
                        {
                            item.Checked = true;

                        }
                        radChkList.Items.Add(item);


                    }
                }
               
                thePnl.Controls.Add(radChkList);

                //SetBusinessrule(theLbl, PnlMulti, Convert.ToInt32(CFieldId));

                
                
                PnlConFields.Controls.Add(new LiteralControl("</td>"));
               
            }
        }

        public Boolean CheckedMultiselectlist(string id,string stable)
        {
            Boolean blnfind = false;
            DataSet theCareEndDS = (DataSet)Session["CareEndFields"];
            DataView theFieldDV = new DataView(theCareEndDS.Tables[4]);
            theFieldDV.RowFilter = "SavingTable='" + stable + "'";
            DataSet theDS = (DataSet)Session["OldData"];
            if (theDS != null)
            {
                foreach (DataTable theDT in theDS.Tables)
                {
                    if (theDT.Columns.Contains("TableName") == true && theDT.Rows.Count > 0)
                    {
                        if (theDT.Rows[0]["TableName"].ToString().ToUpper() == stable.ToUpper())
                        {
                            foreach (DataRow DR in theDT.Rows)
                            {
                                if (DR[3].ToString() == id)
                                {
                                    blnfind = true;
                                }
                            }
                        }
                    }
                }
            }
            return blnfind;
        }

        void ddlSelectList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox theDList = ((RadComboBox)sender);
            DataSet theDS = (DataSet)Session["CareEndFields"];
            
            string[] theCntrl = theDList.ID.Split('-');
            foreach (DataRow theDR in theDS.Tables[6].Rows)
            {
                foreach (Control x in DIVCustomItem.Controls)
                {
                    if (x.ID != null)
                    {
                        string[] theIdent = x.ID.Split('-');
                        if ((x.GetType().ToString() == "Telerik.Web.UI.RadTextBox") && (theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString()))
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadTextBox)x).Enabled = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                            {
                                ((RadTextBox)x).Enabled = false;
                                ((RadTextBox)x).Text = "";
                            }
                        }
                        if ((x.GetType().ToString() == "Telerik.Web.UI.RadNumericTextBox") && (theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString()))
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadTextBox)x).Enabled = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                            {
                                ((RadTextBox)x).Enabled = false;
                                ((RadTextBox)x).Text = "";
                            }
                        }
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadComboBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadComboBox)x).Enabled = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                            {
                                ((RadComboBox)x).Enabled = false;
                                ((RadComboBox)x).SelectedValue = "0";
                            }
                        }


                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel" && theIdent[0] == "Pnl")
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString() && theDR["FieldName"].ToString() == theIdent[1].ToString())
                                ((Panel)x).Enabled = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString() && theDR["FieldName"].ToString() == theIdent[1].ToString())
                            {
                                ((Panel)x).Enabled = false;
                            }
                        }


                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Image" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((Image)x).Visible = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                                ((Image)x).Visible = false;
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadButton" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = false;
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadButton" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = false;
                        }
                    }
                }

                foreach (Control x in PnlConFields.Controls)
                {
                    if (x.ID != null)
                    {
                        string[] theIdent = x.ID.Split('-');
                        if (x.GetType().ToString() == "Telerik.Web.UI.RadTextBox" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadTextBox)x).Enabled = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                            {
                                ((RadTextBox)x).Enabled = false;
                                ((RadTextBox)x).Text = "";
                            }
                        }

                        if (x.GetType().ToString() == "System.Web.UI.WebControls.DropDownList" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadComboBox)x).Enabled = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                            {
                                ((RadComboBox)x).Enabled = false;
                                ((RadComboBox)x).SelectedValue = "0";
                            }
                        }


                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Panel" && theIdent[0] == "Pnl")
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString() && theDR["FieldName"].ToString() == theIdent[1].ToString())
                                ((Panel)x).Enabled = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString() && theDR["FieldName"].ToString() == theIdent[1].ToString())
                            {
                                ((Panel)x).Enabled = false;
                            }
                        }


                        if (x.GetType().ToString() == "System.Web.UI.WebControls.Image" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((Image)x).Visible = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                                ((Image)x).Visible = false;
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadButton" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = false;
                        }

                        if (x.GetType().ToString() == "Telerik.Web.UI.RadButton" && theIdent[1] + "-" + theIdent[2] + "-" + theIdent[3] == theDR["FieldName"].ToString() + "-" + theDR["PdfTableName"].ToString() + "-" + theDR["FieldId"].ToString())
                        {
                            if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() == theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = true;
                            else if (theCntrl[3] == theDR["ConFieldId"].ToString() && theDR["ConditionalFieldSectionId"].ToString() != theDList.SelectedValue.ToString())
                                ((RadButton)x).Visible = false;
                        }
                    }
                }

            }
        }


        private void FillCheckBoxListData(DataTable theDT, Panel pnl, string FieldName, string theFieldName)
        {



            foreach (Control y in pnl.Controls)
            {
                if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                {
                    FillCheckBoxListData(theDT, pnl, FieldName, theFieldName);
                }
                else
                {
                    
                    
                    if (y.GetType() == typeof(Telerik.Web.UI.RadComboBox))
                    {
                        string[] theDBInfo = y.ID.Split('-');
                        if (theDBInfo.GetValue(0).ToString() == "PnlMulti")
                        {
                            ((RadComboBox)y).FindItemByValue("1").Checked = true;
                          
                        }
                    }
                    
                   
                   
                }
            }
            
        }
        private void LoadPredefinedLabel_Field()
        {
            

                int td = 0;
               
                DIVCustomItem.Controls.Clear();
              
                DataSet theDS = (DataSet)Session["CareEndFields"];

                
                if (theDS.Tables[0].Rows.Count > 0)
                {
                    ViewState["TopControl"] = theDS.Tables[0];
                    DIVCustomItem.Visible = true;
                    DIVCustomItem.Controls.Clear();
                    foreach (DataRow DRLnkTable in theDS.Tables[0].Rows)
                    {
                        #region "CheckConditionalFields"
                        DataView theDVConditionalField = new DataView(theDS.Tables[6]);
                        theDVConditionalField.RowFilter = "ConFieldId=" + DRLnkTable["FieldID"].ToString() + " and ConFieldPredefined=" + DRLnkTable["Predefined"].ToString();
                        theDVConditionalField.Sort = "Seq asc";
                        if (theDVConditionalField.Count > 0)
                            theConditional = true;
                        else
                            theConditional = false;
                        #endregion

                        theDVReq.RowFilter = "FieldID=" + DRLnkTable["FieldId"].ToString() + " and Predefined =" + DRLnkTable["PreDefined"].ToString() + " ";

                        if (td == 0)
                        {
                            DIVCustomItem.Controls.Add(new LiteralControl("<tr>"));
                            LoadFieldTypeControl(theDVReq, DIVCustomItem, DRLnkTable["FeatureName"].ToString(), DRLnkTable["SectionId"].ToString(), DRLnkTable["SectionName"].ToString(), DRLnkTable["FieldId"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["ControlId"].ToString(), DRLnkTable["SavingTable"].ToString(), DRLnkTable["BindingTable"].ToString(), DRLnkTable["PreDefined"].ToString(), true);
                            DIVCustomItem.Controls.Add(new LiteralControl("</tr>"));
                            td = 1;
                        }
                        else
                        {
                            DIVCustomItem.Controls.Add(new LiteralControl("<tr>"));
                            LoadFieldTypeControl(theDVReq, DIVCustomItem, DRLnkTable["FeatureName"].ToString(), DRLnkTable["SectionId"].ToString(), DRLnkTable["SectionName"].ToString(), DRLnkTable["FieldId"].ToString(), DRLnkTable["FieldLabel"].ToString(), DRLnkTable["FieldName"].ToString(), DRLnkTable["ControlId"].ToString(), DRLnkTable["SavingTable"].ToString(), DRLnkTable["BindingTable"].ToString(), DRLnkTable["PreDefined"].ToString(), true);
                            DIVCustomItem.Controls.Add(new LiteralControl("</tr>"));
                            td = 0;
                        }
                        #region "Create Conditional Fields"
                        if (theConditional == true)
                        {
                            for (int i = 0; i < theDVConditionalField.Count; i++)
                            {
                                if (td == 0)
                                {
                                    DIVCustomItem.Controls.Add(new LiteralControl("<tr>"));
                                    LoadFieldTypeControl(theDVReq, DIVCustomItem, theDVConditionalField[i]["FeatureName"].ToString(), "", "",
                                        theDVConditionalField[i]["FieldId"].ToString(), theDVConditionalField[i]["FieldLabel"].ToString(), theDVConditionalField[i]["FieldName"].ToString(), theDVConditionalField[i]["ControlId"].ToString(),
                                        theDVConditionalField[i]["PdfTableName"].ToString(), theDVConditionalField[i]["BindSource"].ToString(), theDVConditionalField[i]["PreDefined"].ToString(), false);
                                    DIVCustomItem.Controls.Add(new LiteralControl("</tr>"));
                                    td = 1;
                                }
                                else
                                {
                                    DIVCustomItem.Controls.Add(new LiteralControl("<tr>"));
                                    LoadFieldTypeControl(theDVReq, DIVCustomItem, DRLnkTable["FeatureName"].ToString(), "", "",
                                        theDVConditionalField[i]["FieldId"].ToString(), theDVConditionalField[i]["FieldLabel"].ToString(), theDVConditionalField[i]["FieldName"].ToString(), theDVConditionalField[i]["ControlId"].ToString(),
                                        theDVConditionalField[i]["PdfTableName"].ToString(), theDVConditionalField[i]["BindSource"].ToString(), theDVConditionalField[i]["PreDefined"].ToString(), false);
                                    DIVCustomItem.Controls.Add(new LiteralControl("</tr>"));
                                    td = 0;
                                }
                            }
                        }
                        #endregion
                    }
                   
                }
           

        }
        protected void cmbDeathReason_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ViewState["OldDeathSelectedValue"] = cmbDeathReason.SelectedValue;
            if (cmbDeathReason.SelectedValue.ToString() != "" && Convert.ToInt32(cmbDeathReason.SelectedValue) != 0)
            {
                PnlConFields.Controls.Clear();
                if (cmbPatientExitReason.SelectedValue.ToString() == "93")
                {
                    Tr_Deathreason.Visible = false;
                    Tr_Deathreason1.Visible = true;

                }
                else
                {
                    Tr_Deathreason.Visible = false;
                    Tr_Deathreason1.Visible = false;
                }

                DataSet DSDeath = (DataSet)Session["CareEndFields"];
                DataView DVDeath = new DataView(DSDeath.Tables[5]);
                DVDeath.RowFilter = "SectionId=" + cmbDeathReason.SelectedValue.ToString();
                DVDeath.Sort = "FieldId asc";
                DataTable DTDeath = new DataTable();
                DTDeath = DVDeath.ToTable();
                DataView DVDDeathReason = new DataView(DSDeath.Tables[1]);
                DVDDeathReason.RowFilter = "SectionId=" + "93";
                DTDeath.Merge(DVDDeathReason.ToTable());
                theDVReq = new DataView(DSDeath.Tables[3].Copy());
                ViewState["cmbbox"] = DTDeath;
                if (DTDeath.Rows.Count > 0)
                {
                    PnlConFields.Visible = true;
                    cmbboxLoad(DTDeath, theDVReq, patexitval);
                }

                if (ViewState["TopControl"] != null)
                {
                    if (((DataTable)ViewState["TopControl"]).Rows.Count > 0)
                        DIVCustomItem.Visible = true;
                    LoadPredefinedLabel_Field();
                }
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            if (VisitID == 0)
            {
                if (ValidationFormData() == true)
                {
                    SaveCustomFormData(0);
                }
            }
        }
        private void SaveCustomFormData(int dtqty)
        {

            #region "GetFieldValues"
            string MissedAppDate = txtMissedAppDate.SelectedDate.HasValue ? txtMissedAppDate.SelectedDate.Value.ToString() : null;
            DataTable CEFields = new DataTable();
            CEFields.Columns.Add("TableName", typeof(System.String));
            CEFields.Columns.Add("FieldName", typeof(System.String));
            CEFields.Columns.Add("Value", typeof(System.String));
            CEFields.Columns.Add("OtherDesc", typeof(System.String));
            CEFields.Columns.Add("Priority", typeof(System.Int32));

            DataRow theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientTrackingCare";
            theDR["FieldName"] = "DateLastContact";
            theDR["Value"] = txtDateLastContact.SelectedDate.Value.ToString();
            theDR["Priority"] = "1";
            CEFields.Rows.Add(theDR);
            theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientTrackingCare";
            theDR["FieldName"] = "EmployeeId";
            theDR["Value"] = ddinterviewer.SelectedValue.ToString();
            theDR["Priority"] = "1";
            CEFields.Rows.Add(theDR);
            theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientTrackingCare";
            theDR["FieldName"] = "ModuleId";
            theDR["Value"] = Session["TechnicalAreaId"].ToString();
            theDR["Priority"] = "1";
            CEFields.Rows.Add(theDR);
            theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientTrackingCare";
            theDR["FieldName"] = "DataQuality";
            theDR["Value"] = dtqty.ToString();
            theDR["Priority"] = "1";
            CEFields.Rows.Add(theDR);
            theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientCareEnded";
            theDR["FieldName"] = "MissedAppDate";
            theDR["Value"] = MissedAppDate;
            theDR["Priority"] = "2";
            CEFields.Rows.Add(theDR);
            theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientCareEnded";
            theDR["FieldName"] = "CareEnded";
            theDR["Value"] = "1";
            theDR["Priority"] = "2";
            CEFields.Rows.Add(theDR);
            theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientCareEnded";
            theDR["FieldName"] = "PatientExitReason";
            theDR["Value"] = cmbPatientExitReason.SelectedValue.ToString();
            theDR["Priority"] = "2";
            CEFields.Rows.Add(theDR);
            theDR = CEFields.NewRow();
            theDR["TableName"] = "Dtl_PatientCareEnded";
            theDR["FieldName"] = "CareEndedDate";
            theDR["Value"] = txtCareEndDate.SelectedDate.Value.ToString();
            theDR["Priority"] = "2";
            CEFields.Rows.Add(theDR);
            if (cmbPatientExitReason.SelectedValue.ToString() == "93")
            {
                theDR = CEFields.NewRow();
                theDR["TableName"] = "Dtl_PatientCareEnded";
                theDR["FieldName"] = "DeathDate";
                theDR["Value"] = txtDeathDate.SelectedDate.Value.ToString();
                theDR["Priority"] = "2";
                CEFields.Rows.Add(theDR);
            }
            CEFields = GetSaveStatement(DIVCustomItem, CEFields);
            DataTable theConField = new DataTable();
            theConField.Columns.Add("TableName", typeof(System.String));
            theConField.Columns.Add("FieldName", typeof(System.String));
            theConField.Columns.Add("Value", typeof(System.String));
            theConField.Columns.Add("OtherDesc", typeof(System.String));
            theConField.Columns.Add("Priority", typeof(System.Int32));

            theConField = GetSaveStatement(PnlConFields, theConField);
            CEFields.Merge(theConField);

            DataView theDV = new DataView(CEFields);
            theDV.Sort = "Priority,TableName asc";
            DataTable theNewDT = theDV.ToTable();
            string TblName = "";
            string theValue = "";
            string theCESQL = "";
            foreach (DataRow theRow in theNewDT.Rows)
            {
                if (TblName != theRow["TableName"].ToString().ToUpper())
                {
                    if (theCESQL != "" && theValue != "")
                    {
                        theCESQL = theCESQL + ")" + theValue + ");";

                        if (TblName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                        {
                            string theBL = " declare @TrackId int; select @TrackId = ident_current('DTL_PATIENTTRACKINGCARE');";
                            theCESQL = theCESQL + theBL;
                        }

                        else if (TblName.ToUpper() == "DTL_PATIENTCAREENDED")
                        {
                            string theBL = "declare @CareEndId int; select @CareEndId = ident_current('DTL_PATIENTCAREENDED');";
                            theCESQL = theCESQL + theBL;
                        }
                    }
                    if (theRow["TableName"].ToString().ToUpper() == "DTL_PATIENTTRACKINGCARE")
                    {
                        theCESQL = theCESQL + " Insert into [" + theRow["TableName"].ToString() + "](Ptn_Pk,LocationId,UserId,CreateDate";
                        theValue = " values(" + Session["PatientId"].ToString() + "," + Session["AppLocationId"].ToString() + "," + Session["AppUserId"].ToString() + ",getdate()";
                    }
                    else if (theRow["TableName"].ToString().ToUpper() == "DTL_PATIENTCAREENDED")
                    {
                        theCESQL = theCESQL + " Insert into [" + theRow["TableName"].ToString() + "](TrackingId,Ptn_Pk,LocationId,UserId,CreateDate";
                        theValue = " values(@TrackId," + Session["PatientId"].ToString() + "," + Session["AppLocationId"].ToString() + "," + Session["AppUserId"].ToString() + ",getdate()";
                    }
                    else
                    {
                        theCESQL = theCESQL + " Insert into [" + theRow["TableName"].ToString() + "](CareEndedId,Ptn_Pk,LocationId,UserId,CreateDate";
                        theValue = " values(@CareEndId," + Session["PatientId"].ToString() + "," + Session["AppLocationId"].ToString() + "," + Session["AppUserId"].ToString() + ",getdate()";
                    }
                    TblName = theRow["TableName"].ToString().ToUpper();
                }
                if (theCESQL.Contains("," + theRow["FieldName"].ToString()) == true)
                {
                    if (theRow["OtherDesc"].ToString() != "")
                    {
                        DataSet ds = new DataSet();
                        ds = (DataSet)Session["CareEndFields"];
                        DataView theDVOther = new DataView(ds.Tables[4]);
                        theDVOther.RowFilter = "FieldName='" + theRow["FieldName"].ToString() + "'";
                        if (theDVOther.Count >= 1)
                        {
                            theCESQL = theCESQL + ")" + theValue + ");" + " Insert into [" + theRow["TableName"].ToString() + "](CareEndedId,Ptn_Pk,LocationId,UserId,CreateDate, " + theRow["FieldName"].ToString() + "," + theDVOther[0]["OtherDesCol"].ToString() + "";
                            theValue = "";
                            theValue = " values(@CareEndId," + Session["PatientId"].ToString() + "," + Session["AppLocationId"].ToString() + "," + Session["AppUserId"].ToString() + ",getdate()" + ",'" + theRow["Value"].ToString() + "','" + theRow["OtherDesc"].ToString() + "'";
                        }
                    }
                    else
                    {
                        if (theRow["TableName"].ToString().ToUpper() != "DTL_PATIENTCAREENDED")
                        {
                            theCESQL = theCESQL + ")" + theValue + ");" + " Insert into [" + theRow["TableName"].ToString() + "](CareEndedId,Ptn_Pk,LocationId,UserId,CreateDate, " + theRow["FieldName"].ToString() + "";
                            theValue = "";
                            theValue = " values(@CareEndId," + Session["PatientId"].ToString() + "," + Session["AppLocationId"].ToString() + "," + Session["AppUserId"].ToString() + ",getdate()" + ",'" + theRow["Value"].ToString() + "'";
                        }
                    }

                    
                }
                else
                {
                    if (theRow["Value"].ToString() != "")
                    {
                        theCESQL = theCESQL + "," + theRow["FieldName"].ToString();
                        theValue = theValue + ",'" + theRow["Value"].ToString() + "'";
                    }
                }
            }

            IContactCare CCControl = (IContactCare)ObjectFactory.CreateInstance("BusinessProcess.Scheduler.BContactCare,BusinessProcess.Scheduler");
            DataSet theCheckDS = CCControl.CheckModuleTrackingStatus(Convert.ToInt32(Session["PatientId"]), Convert.ToInt32(Session["TechnicalAreaId"]));

            theCESQL = theCESQL + ")" + theValue + ");";

            theCESQL = theCESQL + " Update mst_patient set status='" + 1 + "' where ptn_pk= '" + Session["PatientId"].ToString() + "'";
            //if (Convert.ToInt32(cmbPatientExitReason.SelectedValue) == 93 || (Convert.ToInt32(theCheckDS.Tables[0].Rows[0][0]) == Convert.ToInt32(theCheckDS.Tables[1].Rows[0][0])))
            //{
            //    theCESQL = theCESQL + " Update mst_patient set status='" + 1 + "' where ptn_pk= '" + Session["PatientId"].ToString() + "'";

            //}

            theCESQL = theCESQL + "exec Pr_Scheduler_PatientCareEnded @TrackId," + Session["PatientId"].ToString() + "";

            #endregion

            int i = 0;
            int row = 0;
            bool IsError = false;

            ICareEnded CEControl;
            try
            {
                List<ICareEndedFields> CareEndedList = new List<ICareEndedFields>();
                ICareEndedFields objCareEndedFields = new ICareEndedFields();
                objCareEndedFields.Query = theCESQL;
                objCareEndedFields.PatientID = Convert.ToInt32(Session["PatientId"].ToString());
                objCareEndedFields.CareEndedDate = Convert.ToDateTime(txtCareEndDate.SelectedDate.Value.ToString());
                CareEndedList.Add(objCareEndedFields);

                CEControl = (ICareEnded)ObjectFactory.CreateInstance("BusinessProcess.Scheduler.BCareEnded,BusinessProcess.Scheduler");
                if (VisitID == 0)
                {
                    row = CEControl.IQTouchSaveCareEnded(CareEndedList);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackWithoutDialog();", true);
                }
                //row = CEControl.SaveGetDynamicControlDatat(theCESQL, Session["PatientId"].ToString(), txtCareEndDate.SelectedDate.Value.ToString());
              
            }
            catch (Exception e)
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
        private DataTable GetSaveStatement(Control thePnl, DataTable theDT)
        {

            string fName = "";
            string FtbName = "";
            //For Other ID and Other Value
            string OtherTXTID = "", OtherFtbName = "", OtherfName = "", OtherValue = "";

            foreach (Control y in thePnl.Controls)
            {
                if (y.GetType() == typeof(System.Web.UI.WebControls.Panel))
                {
                    GetSaveStatement(y, theDT);
                }
                else
                {
                    if (y.GetType() == typeof(Telerik.Web.UI.RadTextBox))
                    {
                        if (((RadTextBox)y).Text != "")
                        {
                            
                            string[] theDBInfo = y.ID.Split('-');
                            DataRow theDR = theDT.NewRow();
                            fName = theDBInfo.GetValue(1).ToString();
                            FtbName = theDBInfo.GetValue(2).ToString();
                            theDR["TableName"] = FtbName;
                            theDR["FieldName"] = fName;
                            if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                                theDR["Priority"] = "1";
                            else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                                theDR["Priority"] = "2";
                            else
                                theDR["Priority"] = "999";

                            if (theDBInfo.GetValue(0).ToString() == "TXT")
                                theDR["Value"] = ((RadTextBox)y).Text;
                            
                            else if (theDBInfo.GetValue(0).ToString() == "TXTMulti")
                                theDR["Value"] = ((RadTextBox)y).Text;
                            
                            
                                theDT.Rows.Add(theDR);
                        }
                    }
                    else if (y.GetType() == typeof(Telerik.Web.UI.RadNumericTextBox))
                    {
                        if (((RadNumericTextBox)y).Text != "")
                        {
                            string[] theDBInfo = y.ID.Split('-');
                            DataRow theDR = theDT.NewRow();
                            fName = theDBInfo.GetValue(1).ToString();
                            FtbName = theDBInfo.GetValue(2).ToString();
                            theDR["TableName"] = FtbName;
                            theDR["FieldName"] = fName;
                            if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                                theDR["Priority"] = "1";
                            else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                                theDR["Priority"] = "2";
                            else
                                theDR["Priority"] = "999";

                            if (theDBInfo.GetValue(0).ToString() == "TXTDCM")
                            {
                                if (((RadNumericTextBox)y).Text == "")
                                {
                                    theDR["Value"] = 0;
                                }
                                else
                                {
                                    theDR["Value"] = Convert.ToDecimal(((RadNumericTextBox)y).Text);
                                }
                            }
                            else if (theDBInfo.GetValue(0).ToString() == "TXTNUM")
                            {
                                if (((RadNumericTextBox)y).Text == "")
                                {
                                    theDR["Value"] = 0;
                                }
                                else
                                {
                                    theDR["Value"] = Convert.ToInt32(((RadNumericTextBox)y).Text);
                                }
                            }
                            theDT.Rows.Add(theDR);
                        }
                    }
                    else if (y.GetType() == typeof(Telerik.Web.UI.RadDatePicker))
                    {
                        if (((RadDatePicker)y).SelectedDate != null)
                        {
                            string[] theDBInfo = y.ID.Split('-');
                            DataRow theDR = theDT.NewRow();
                            fName = theDBInfo.GetValue(1).ToString();
                            FtbName = theDBInfo.GetValue(2).ToString();
                            theDR["TableName"] = FtbName;
                            theDR["FieldName"] = fName;
                            if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                                theDR["Priority"] = "1";
                            else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                                theDR["Priority"] = "2";
                            else
                                theDR["Priority"] = "999";
                            if (theDBInfo.GetValue(0).ToString() == "TXTDT")
                            {
                                if (((RadDatePicker)y).SelectedDate != null)
                                {
                                    theDR["Value"] = ((RadDatePicker)y).SelectedDate.Value;
                                }
                            }
                            theDT.Rows.Add(theDR);
                        }
                    }
                    else if (y.GetType() == typeof(Telerik.Web.UI.RadComboBox))
                    {
                        string[] theDBInfo = y.ID.Split('-');
                        if (theDBInfo.GetValue(0).ToString() == "SelectList")
                        {
                            DataRow theDR = theDT.NewRow();
                            fName = theDBInfo.GetValue(1).ToString();
                            FtbName = theDBInfo.GetValue(2).ToString();
                            theDR["TableName"] = FtbName;
                            theDR["FieldName"] = fName;
                            theDR["Value"] = ((RadComboBox)y).SelectedValue.ToString();
                            if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                                theDR["Priority"] = "1";
                            else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                                theDR["Priority"] = "2";
                            else
                                theDR["Priority"] = "999";

                            theDT.Rows.Add(theDR);
                        }
                        if (theDBInfo.GetValue(0).ToString() == "PnlMulti")
                        {
                            var collection = ((RadComboBox)y).CheckedItems;
                            if (collection.Count != 0)
                            {
                                foreach (var item in collection)
                                {
                                    
                                    DataRow theDR = theDT.NewRow();
                                    fName = theDBInfo.GetValue(1).ToString();
                                    FtbName = theDBInfo.GetValue(2).ToString();
                                    theDR["TableName"] = FtbName;
                                    theDR["FieldName"] = fName;
                                    theDR["Value"] = item.Value; 
                                    if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                                        theDR["Priority"] = "1";
                                    else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                                        theDR["Priority"] = "2";
                                    else
                                        theDR["Priority"] = "999";

                                    theDT.Rows.Add(theDR);
                                }
                            }

                            
                        }
                    }
                    else if (y.GetType() == typeof(Telerik.Web.UI.RadButton))
                    {
                        string[] theDBInfo = y.ID.Split('-');
                        DataRow theDR = theDT.NewRow();
                        fName = theDBInfo.GetValue(1).ToString();
                        FtbName = theDBInfo.GetValue(2).ToString();
                        theDR["TableName"] = FtbName;
                        theDR["FieldName"] = fName;
                        
                        if (((RadButton)y).Checked == true)
                            theDR["Value"] = "1";
                        else if (((RadButton)y).Checked == true)
                            theDR["Value"] = "0";
                        if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                            theDR["Priority"] = "1";
                        else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                            theDR["Priority"] = "2";
                        else
                            theDR["Priority"] = "999";
                        if (theDR["Value"].ToString() != "")
                            theDT.Rows.Add(theDR);
                    }
                    else if (y.GetType() == typeof(Telerik.Web.UI.RadButton))
                    {
                        string[] theDBInfo = y.ID.Split('-');
                        DataRow theDR = theDT.NewRow();
                        fName = theDBInfo.GetValue(1).ToString();
                        FtbName = theDBInfo.GetValue(2).ToString();
                        theDR["TableName"] = FtbName;
                        theDR["FieldName"] = fName;
                        if (((RadButton)y).Checked == true)
                            theDR["Value"] = "1";
                        else if (((RadButton)y).Checked == true)
                            theDR["Value"] = "0";
                        if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                            theDR["Priority"] = "1";
                        else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                            theDR["Priority"] = "2";
                        else
                            theDR["Priority"] = "999";

                        theDT.Rows.Add(theDR);
                    }
                    else if (y.GetType() == typeof(Telerik.Web.UI.RadButton))
                    {
                        if (((RadButton)y).Checked == true)
                        {
                            string[] theDBInfo = y.ID.Split('-');

                            DataRow theDR = theDT.NewRow();
                            fName = theDBInfo.GetValue(1).ToString();
                            FtbName = theDBInfo.GetValue(2).ToString();
                            theDR["TableName"] = FtbName;
                            theDR["FieldName"] = fName;
                            theDR["Value"] = theDBInfo.GetValue(5).ToString();
                            if (FtbName.ToUpper() == "DTL_PATIENTTRACKINGCARE")
                                theDR["Priority"] = "1";
                            else if (FtbName.ToUpper() == "DTL_PATIENTCAREENDED")
                                theDR["Priority"] = "2";
                            else
                                theDR["Priority"] = "999";
                            theDT.Rows.Add(theDR);
                        }
                    }
                }
            }
            return theDT;
        }
        private Boolean ValidationFormData()
        {

            lblmessage.Text = "";
                
            string[] theBRInfo = hidID.Value.Split('%');

            if (txtDateLastContact.SelectedDate == null)
            {
                //lblmessage.Text = "Date of Last Actual Contact cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("DateLastActualContact");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + theMsg.Text + "');", true);
                return false;

                

            }
            if (cmbPatientExitReason.SelectedValue.ToString() == "0")
            {
                RawMessage theMsg = MsgRepository.GetMessage("PatientExitReason");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + theMsg.Text + "');", true);
                //lblmessage.Text="Select Patient Exit Reason  first.";
                return false;

            }

            if (cmbPatientExitReason.SelectedValue.ToString() == "93")
            {
                if (txtDeathDate.SelectedDate == null)
                {
                     //lblmessage.Text="Please enter Death date.";
                    RawMessage theMsg = MsgRepository.GetMessage("Deathdate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + theMsg.Text + "');", true);
                     return false;

                }

            }


            if (ddinterviewer.SelectedValue.ToString() == "0")
            {
                RawMessage theMsg = MsgRepository.GetMessage("Signature");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + theMsg.Text + "');", true);
                return false;
            }

            if (txtCareEndDate.SelectedDate == null)
            {
                //lblmessage.Text = "Date Care Ended cannot be blank.";
                RawMessage theMsg = MsgRepository.GetMessage("DateCareEnded");
                 RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + theMsg.Text + "');", true);
                return false;
            }

            if ((Convert.ToDateTime(txtCareEndDate.SelectedDate.Value)) < (Convert.ToDateTime(txtDateLastContact.SelectedDate.Value)))
            {
                //lblmessage.Text = "Date Care Ended cannot less then Date of Last Actual Contact.";
                RawMessage theMsg = MsgRepository.GetMessage("DateCareEndedactual");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + theMsg.Text + "');", true);
                return false;
                
            }

           
            if (txtDeathDate.SelectedDate != null)
            {
                if (Convert.ToDateTime(txtDeathDate.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today.AddDays(1)))
                {
                    
                    //lblmessage.Text = "Death date cannot be a future date.";
                RawMessage theMsg = MsgRepository.GetMessage("InvalidDeathdate");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "fnShowMessage('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            
            return true;

        }
        private void ClearHiddenfield()
        {
            hidID.Value = "";
            hidcheckbox.Value = "";
            hidradio.Value = ""; ;
            hidchkbox.Value = "";
            hidIDQty.Value = "";
            hidcheckboxQty.Value = "";
            hidradioQty.Value = "";
            hidchkboxQty.Value = "";
            theHitCntrl.Value = "";
            HiddenMsgBuilderfield.Value = "";
            if ((cmbDeathReason.SelectedValue != "0") && (cmbPatientExitReason.SelectedValue != "93"))
            {
                txtCareEndDate.DbSelectedDate = null;
            }
        }
    }
}
