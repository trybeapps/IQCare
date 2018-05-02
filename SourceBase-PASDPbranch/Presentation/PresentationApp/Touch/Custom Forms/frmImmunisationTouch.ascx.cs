using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//IQCare Libs
using Application.Presentation;
using Interface.Pharmacy;
using Application.Common;
//Third party Libs
using Telerik.Web.UI;

namespace Touch.Custom_Forms
{

    public partial class frmImmunisationTouch : TouchUserControlBase
    {
        DataTable DtImmunisation;
        Boolean ImmunisationFlag = false;
        static Boolean IsError = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            lblerr.Text = "";
            lblerrImmOther.Text = "";
            HddErrorFlag.Value = "0";
            
            String script = frmImmunisation_block.InnerText.Replace(Environment.NewLine, "");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "regScripts", script, false);
           
            Session["CurrentForm"] = "frmImmunisationTouch";
            Session["FormIsLoaded"] = true;


            if (Session["IsFirstLoad"] == "true")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "UpdateScollers", "resizeScrollbars();", true);
                GetImmunisationDetails();
                GetImmunisationDetaisOther();
                LoadBlankGrid();
                Session["IsFirstLoad"] = "false";

            }

            /***************** Check For User Rights ****************/
            AuthenticationManager Authentication = new AuthenticationManager();

            btnPrint.Visible = Authentication.HasFunctionRight(ApplicationAccess.PASDPImmunisation, FunctionAccess.Print, (DataTable)Session["UserRight"]);

            if (!Authentication.HasFunctionRight(ApplicationAccess.PASDPImmunisation, FunctionAccess.Update, (DataTable)Session["UserRight"]))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showsave", "$('#divSave').hide();", true);
            }
        }

        protected void btnPrint_OnClick(object sender, EventArgs e)
        {
            updtFormUpdate.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "printScript", "PrintTouchForm('Immunisation Form', '" + this.ID + "');", true);
        }

        private Boolean ValidationFormData()
        {


            if (dtDateBCG.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateBCG.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtOPV0Date.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtOPV0Date.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateOPV1.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateOPV1.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateRV1.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateRV1.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateDTaP1.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateDTaP1.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateHepB1.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateHepB1.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDatePCV1.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDatePCV1.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateDTaP2.SelectedDate != null)
            {
            if (Convert.ToDateTime(dtDateDTaP2.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateHepB2.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateHepB2.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateDTaP3.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateDTaP3.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateHepB3.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateHepB3.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDatePCV2.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDatePCV2.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateRV2.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateRV2.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateMeasles1.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateMeasles1.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDatePVC3.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDatePVC3.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateMeasles2.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateMeasles2.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateDTaP4.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateDTaP4.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateTd6Yrs.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateTd6Yrs.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }
            if (dtDateTd23Yrs.SelectedDate != null)
            {
                if (Convert.ToDateTime(dtDateTd23Yrs.SelectedDate.Value) > Convert.ToDateTime(DateTime.Today))
                {
                    RawMessage theMsg = MsgRepository.GetMessage("IQTouchImmunisationDate");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "src1", "FormValidatedOnSubmit('" + theMsg.Text + "');", true);
                    return false;
                }
            }

            return true;



        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (ValidationFormData() == false)
            {
                return;

            }


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "loadpharm", "parent.ShowLoading()", true);
            Session["IsFirstLoad"] = "true";
            

            try
            {
                List<BIQTouchmmunisationFields> list = new List<BIQTouchmmunisationFields>();
                List<RadButton> lstRadButton = new List<RadButton>();
                lstRadButton.Add(btnBCG);
                lstRadButton.Add(btnOPV0);
                lstRadButton.Add(btnOPV1);
                lstRadButton.Add(btnRV1);
                lstRadButton.Add(btnDTaP1);
                lstRadButton.Add(btnHEP1);
                lstRadButton.Add(btmPCV1);
                lstRadButton.Add(btnDTaP2);
                lstRadButton.Add(btnHEP2);
                lstRadButton.Add(btnDTaP3);
                lstRadButton.Add(btnHEP3);
                lstRadButton.Add(btnPCV2);
                lstRadButton.Add(btnRV2);
                lstRadButton.Add(btnMeasles1);
                lstRadButton.Add(btnPCV3);
                lstRadButton.Add(btnDTaP4);
                lstRadButton.Add(btnMeasles2);
                lstRadButton.Add(btnTD6yrs);
                lstRadButton.Add(btnTd12yrs);


                
                

                foreach (RadButton radbtn in lstRadButton)
                {
                    //s = s + radbtn.SelectedToggleState.Text;   

                    if (radbtn.SelectedToggleState.Text.ToString().ToUpper() == "YES")
                    {

                        BIQTouchmmunisationFields obj = new BIQTouchmmunisationFields();
                        obj.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]); ;
                        obj.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
                        obj.UserId = Int32.Parse(Session["AppUserId"].ToString());
                        obj.ImmunisationCU = 0;
                        obj.Flag = 1;

                        //obj.ImmunisationDate = Convert.ToDateTime(dtDateBCG.SelectedDate.ToString());
                        obj.CardAvailable = CheckedVaue(rbtnCardLostYes.SelectedToggleState.Text.ToString());
                        obj.ImmunisationOther = "";


                        switch (radbtn.ID)
                        {
                            case "btnBCG": //1

                                obj.ImmunisationDate = DateGiven(dtDateBCG.SelectedDate.ToString());
                                obj.ImmunisationCode = "BCG";
                                ImmunisationFlag = true;
                                break;
                            case "btnOPV0"://2
                               
                                obj.ImmunisationDate = DateGiven(dtOPV0Date.SelectedDate.ToString());
                                obj.ImmunisationCode = "OPV0";
                                ImmunisationFlag = true;
                                break;
                            case "btnOPV1"://3
                                obj.ImmunisationDate = DateGiven(dtDateOPV1.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_OPV1.SelectedToggleState.Text);
                                obj.ImmunisationCode = "OPV1";
                                ImmunisationFlag = true;
                                break;
                            case "btnRV1"://4
                                obj.ImmunisationDate = DateGiven(dtDateRV1.SelectedDate.ToString());
                                obj.ImmunisationCode = "RV1";
                                ImmunisationFlag = true;
                                break;
                            case "btnDTaP1"://5
                                obj.ImmunisationDate = DateGiven(dtDateDTaP1.SelectedDate.ToString());
                                obj.ImmunisationCode = "DTaP-IPV- Hib1";
                                ImmunisationFlag = true;
                                break;
                            case "btnHEP1"://6
                                obj.ImmunisationDate = DateGiven(dtDateHepB1.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_HEP1.SelectedToggleState.Text);
                                obj.ImmunisationCode = "Hep B1";
                                ImmunisationFlag = true;
                                break;
                            case "btmPCV1"://7
                                obj.ImmunisationDate = DateGiven(dtDatePCV1.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_PCV1.SelectedToggleState.Text);
                                obj.ImmunisationCode = "PCV1";
                                ImmunisationFlag = true;
                                break;
                            case "btnDTaP2"://8
                                obj.ImmunisationDate = DateGiven(dtDateDTaP2.SelectedDate.ToString());
                                obj.ImmunisationCode = "DTaP-IPV- Hib2";
                                ImmunisationFlag = true;
                                break;
                            case "btnHEP2"://9
                                obj.ImmunisationDate = DateGiven(dtDateHepB2.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_HEP2.SelectedToggleState.Text);
                                obj.ImmunisationCode = "Hep B2";
                                ImmunisationFlag = true;
                                break;
                            case "btnDTaP3"://10
                                obj.ImmunisationDate = DateGiven(dtDateDTaP3.SelectedDate.ToString());
                                obj.ImmunisationCode = "DTaP-IPV- Hib3";
                                ImmunisationFlag = true;
                                break;
                            case "btnHEP3"://11
                                obj.ImmunisationDate = DateGiven(dtDateHepB3.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_HEP3.SelectedToggleState.Text);
                                obj.ImmunisationCode = "Hep B3";
                                ImmunisationFlag = true;
                                break;
                            case "btnPCV2"://12
                                obj.ImmunisationDate = DateGiven(dtDatePCV2.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_PCV2.SelectedToggleState.Text);
                                obj.ImmunisationCode = "PCV2";
                                ImmunisationFlag = true;
                                break;
                            case "btnRV2"://13
                                obj.ImmunisationDate = DateGiven(dtDateRV2.SelectedDate.ToString());
                                obj.ImmunisationCode = "RV2";
                                ImmunisationFlag = true;
                                break;
                            case "btnMeasles1"://14
                                obj.ImmunisationDate = DateGiven(dtDateMeasles1.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_Measles1.SelectedToggleState.Text);
                                obj.ImmunisationCode = "Measles1";
                                ImmunisationFlag = true;
                                break;
                            case "btnPCV3": //15
                                obj.ImmunisationDate = DateGiven(dtDatePVC3.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_PCV3.SelectedToggleState.Text);
                                obj.ImmunisationCode = "PCV3";
                                ImmunisationFlag = true;
                                break;
                            case "btnDTaP4": //16
                                obj.ImmunisationDate = DateGiven(dtDateDTaP4.SelectedDate.ToString());
                                obj.ImmunisationCode = "DTaP-IPV- Hib4";
                                ImmunisationFlag = true;
                                break;
                            case "btnMeasles2"://17
                                obj.ImmunisationDate = DateGiven(dtDateMeasles2.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_Measles2.SelectedToggleState.Text);
                                obj.ImmunisationCode = "Measles2";
                                ImmunisationFlag = true;
                                break;
                            case "btnTD6yrs"://18
                                obj.ImmunisationDate = DateGiven(dtDateTd6Yrs.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_Td6yrs.SelectedToggleState.Text);
                                obj.ImmunisationCode = "Td - 6 yrs";
                                ImmunisationFlag = true;
                                break;
                            case "btnTd12yrs": //19
                                obj.ImmunisationDate = DateGiven(dtDateTd23Yrs.SelectedDate.ToString());
                                obj.ImmunisationCU = CheckedVaue(btnCUGT_Td12yrs.SelectedToggleState.Text);
                                obj.ImmunisationCode = "Td - 12 yrs";
                                ImmunisationFlag = true;
                                break;

                        }

                        list.Add(obj);
                    }
                }

                /// Other Immunisation details


                if (ViewState["TblImmunisation"] != null)
                {
                    DataTable dt = (DataTable)ViewState["TblImmunisation"];
                    foreach (DataRow dr in dt.Rows)
                    {


                        if (dr["Administered"].ToString() == "Yes" && dr["ImmunisationDate"].ToString() != "")
                        {
                            BIQTouchmmunisationFields obj1 = new BIQTouchmmunisationFields();
                            obj1.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]); ;
                            obj1.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
                            obj1.UserId = Int32.Parse(Session["AppUserId"].ToString());
                            obj1.ImmunisationCU = 0;
                            obj1.Flag = 1;

                            obj1.ImmunisationDate = Convert.ToDateTime(dr["ImmunisationDate"].ToString());
                            obj1.ImmunisationOther = dr["ImmunisationOther"].ToString();
                            obj1.ImmunisationCU = CheckedVaue(dr["ImmunisationCU"].ToString());
                            obj1.ImmunisationCode = "";
                            ImmunisationFlag = true;
                            list.Add(obj1);
                        }



                    }

                }

                if (ImmunisationFlag == false)
                {
                    MsgBuilder theBuilder = new MsgBuilder();
                    theBuilder.DataElements["MessageText"] = "At least one immisation needs to be checked to save the form.";
                    IQCareMsgBox.Show("#C1", theBuilder, this);
                    return;
                    //ViewState["Error"]=ViewState["Error"]+"At least one immisation needs to be checked to save the form<br>";
                    //Page_Load(sender, e);
                    //return;
                    //lblerr.Text = "At least one immisation needs to be checked to save the form";
                    //lblerr.ForeColor = System.Drawing.Color.Red;
                    //lblerr.Font.Bold = true;
                    //return;
                    //HddErrorFlag.Value = "1";
                    //hddErrormsg.Value = "At least one immisation needs to be checked to save the form.";
                }

                IBIQTouchImmunisation theImmunisationManager;
                theImmunisationManager = (IBIQTouchImmunisation)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BIQTouchImmunisation, BusinessProcess.Pharmacy");
                int result = theImmunisationManager.SaveUpdateImmunisationDetail(list);
                ViewState["Error"] = null;

                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveSuc", "alert('Form saved successfully')", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "goBack", "BackWithoutDialog();", true);
                }
                //Response.Redirect(Request.RawUrl);






            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "saveFail", "alert('An errror occured please contact your Administrator')", true);

                //lblerr.Text = ex.Message.ToString();
                //lblerr.ForeColor = System.Drawing.Color.Red;
                //lblerr.Font.Bold = true;
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

        private void GetImmunisationDetaisOther()
        {
            BIQTouchmmunisationFields obj = new BIQTouchmmunisationFields();
            obj.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]);
            obj.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
            obj.Flag = 0;

            IBIQTouchImmunisation theImmunisationManager;
            theImmunisationManager = (IBIQTouchImmunisation)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BIQTouchImmunisation, BusinessProcess.Pharmacy");
            DataSet Ds = theImmunisationManager.GetImmunisationDetails(obj);
            DataTable dtImm = Ds.Tables[0];
            ViewState["TblImmunisation"] = dtImm;
            RadOtherVaccine.DataSource = dtImm;
            RadOtherVaccine.DataBind();
        }
        private void GetImmunisationDetails()
        {


            BIQTouchmmunisationFields obj = new BIQTouchmmunisationFields();
            obj.Ptnpk = Convert.ToInt32(Request.QueryString["PatientID"]);
            obj.LocationId = Int32.Parse(Session["AppLocationId"].ToString());
            obj.Flag = 1;

            IBIQTouchImmunisation theImmunisationManager;
            theImmunisationManager = (IBIQTouchImmunisation)ObjectFactory.CreateInstance("BusinessProcess.Pharmacy.BIQTouchImmunisation, BusinessProcess.Pharmacy");
            DataSet Ds = theImmunisationManager.GetImmunisationDetails(obj);
            // For each row, print the values of each column. 
            if (Ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in Ds.Tables[0].Rows)
                {


                    rbtnCardLostYes.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["CardAvailable"])));
                    switch (row["Immunisation_name"].ToString())
                    {
                        case "BCG": //1

                            btnBCG.SetSelectedToggleStateByText("Yes");
                            dtDateBCG.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "OPV0"://2
                            btnOPV0.SetSelectedToggleStateByText("Yes");
                            dtOPV0Date.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "OPV1"://3
                            btnOPV1.SetSelectedToggleStateByText("Yes");
                            dtDateOPV1.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_OPV1.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "RV1"://4
                            btnRV1.SetSelectedToggleStateByText("Yes");
                            dtDateRV1.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "DTaP-IPV- Hib1"://5
                            btnDTaP1.SetSelectedToggleStateByText("Yes");
                            dtDateDTaP1.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "Hep B1"://6
                            btnHEP1.SetSelectedToggleStateByText("Yes");
                            dtDateHepB1.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_HEP1.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "PCV1"://7
                            btmPCV1.SetSelectedToggleStateByText("Yes");
                            dtDatePCV1.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_PCV1.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "DTaP-IPV- Hib2"://8
                            btnDTaP2.SetSelectedToggleStateByText("Yes");
                            dtDateDTaP2.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "Hep B2"://9
                            btnHEP2.SetSelectedToggleStateByText("Yes");
                            dtDateHepB2.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_HEP2.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "DTaP-IPV- Hib3"://10
                            btnDTaP3.SetSelectedToggleStateByText("Yes");
                            dtDateDTaP3.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "Hep B3"://11
                            btnHEP3.SetSelectedToggleStateByText("Yes");
                            dtDateHepB3.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_HEP3.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "PCV2"://12
                            btnPCV2.SetSelectedToggleStateByText("Yes");
                            dtDatePCV2.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_PCV2.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "RV2"://13
                            btnRV2.SetSelectedToggleStateByText("Yes");
                            dtDateRV2.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "Measles1"://14
                            btnMeasles1.SetSelectedToggleStateByText("Yes");
                            dtDateMeasles1.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_Measles1.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "PCV3": //15
                            btnPCV3.SetSelectedToggleStateByText("Yes");
                            dtDatePVC3.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_PCV3.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "DTaP-IPV- Hib4": //16
                            btnDTaP4.SetSelectedToggleStateByText("Yes");
                            dtDateDTaP4.DbSelectedDate = row["ImmunisationDate"].ToString();
                            break;
                        case "Measles2"://17
                            btnMeasles2.SetSelectedToggleStateByText("Yes");
                            dtDateMeasles2.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_Measles2.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "Td - 6 yrs"://18
                            btnTD6yrs.SetSelectedToggleStateByText("Yes");
                            dtDateTd6Yrs.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_Td6yrs.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;
                        case "Td - 12 yrs": //19
                            btnTd12yrs.SetSelectedToggleStateByText("Yes");
                            dtDateTd23Yrs.DbSelectedDate = row["ImmunisationDate"].ToString();
                            btnCUGT_Td12yrs.SetSelectedToggleStateByText(CatchUpVaue(Convert.ToInt32(row["ImmunisationCU"])));
                            break;

                    }

                }
            }






        }
        DateTime DateGiven(string dateVal)
        {
            DateTime dt = Convert.ToDateTime("01/01/1900");
            if (!string.IsNullOrEmpty(dateVal))
            {
                dt = DateTime.Parse(dateVal);
            }
            return dt;
        }

        int CheckedVaue(string btnToggeState)
        {
            int retval = 0;
            if (btnToggeState.ToUpper() == "YES")
            {
                retval = 1;
            }
            else
            {
                retval = 0;
            }
            return retval;
        }
        string CatchUpVaue(int intval)
        {
            string retval = "No";
            if (intval == 1)
            {
                retval = "Yes";
            }
            else
            {
                retval = "No";
            }
            return retval;
        }



        
        protected void LoadBlankGrid()
        {
            if (RadOtherVaccine.Items.Count == 0)
            {
                RadOtherVaccine.DataSource = new Object[0];
            }

        }
        public DataTable CreateImmunisationTable()
        {
            DataTable dtlImmu = new DataTable();
            dtlImmu.Columns.Add("ID", typeof(string));
            dtlImmu.Columns.Add("ImmunisationOther", typeof(string));
            dtlImmu.Columns.Add("ImmunisationCU", typeof(string));
            dtlImmu.Columns.Add("ImmunisationDate", typeof(string));
            dtlImmu.Columns.Add("Administered", typeof(string));
            dtlImmu.Columns.Add("EditMode", typeof(string));
            dtlImmu.PrimaryKey = new DataColumn[] { dtlImmu.Columns["ID"] };
            return dtlImmu;

        }

        #region Grid Events
        protected void RadOtherVaccine_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataBoundItem = e.Item as GridDataItem;

                Label lblImmunisationName = (Label)dataBoundItem.FindControl("lblImmunisation_name");

                if (lblImmunisationName.Text.ToString() == "")
                {
                    e.Item.Display = false;
                }
                else
                {
                    Label lblAdministered = (Label)dataBoundItem.FindControl("lblAdministered");
                    Label lblcatchUp = (Label)dataBoundItem.FindControl("lblcatchUp");
                    Label lblEditMode = (Label)dataBoundItem.FindControl("lblEditMode");
                    Telerik.Web.UI.RadButton btnOthers = (Telerik.Web.UI.RadButton)dataBoundItem.FindControl("btnOthers");
                    Telerik.Web.UI.RadButton btnCatchupOthers = (Telerik.Web.UI.RadButton)dataBoundItem.FindControl("btnCatchupOthers");
                    Telerik.Web.UI.RadButton btnRemove = (Telerik.Web.UI.RadButton)dataBoundItem.FindControl("btnRemove");

                    btnOthers.SetSelectedToggleStateByText(lblAdministered.Text);
                    btnCatchupOthers.SetSelectedToggleStateByText(lblcatchUp.Text);
                    btnRemove.Visible = true;
                    if (lblEditMode.Text.ToString().Trim() == "EDIT")
                    {
                        // Button Enable for EDIT
                        btnRemove.Visible = false;
                    }
                }



            }



            //if (e.Item is GridFooterItem)
            //{

            //    GridFooterItem itemFooter = e.Item as GridFooterItem;
            //    Telerik.Web.UI.RadButton btnFooterAdd = (Telerik.Web.UI.RadButton)itemFooter.FindControl("btnFooterAdd");
            //    Telerik.Web.UI.RadTextBox txtFooterRadVaccineName = (Telerik.Web.UI.RadTextBox)itemFooter.FindControl("txtFooterRadVaccineName");
            //    Telerik.Web.UI.RadDatePicker dtFooterOtherDate = (Telerik.Web.UI.RadDatePicker)itemFooter.FindControl("dtFooterOtherDate");
            //    Telerik.Web.UI.RadButton btnOthers = (Telerik.Web.UI.RadButton)itemFooter.FindControl("btnFooterOthers");
            //    btnFooterAdd.Attributes.Add("onclick", "return ShowValidatemsgImmuOthers('" + txtFooterRadVaccineName.ClientID + "','" + dtFooterOtherDate.ClientID + "','" + btnOthers.ClientID+ "');");

            //}

            // Edit control

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                if (e.Item is GridEditFormInsertItem)
                {
                    // insert
                }
                else
                {
                    // Edit
                    // Please add below code in your page
                    //GridEditableItem item = e.Item as GridEditableItem;
                    //Telerik.Web.UI.RadButton btnRemove = (Telerik.Web.UI.RadButton)item.FindControl("btnRemove");
                    //Telerik.Web.UI.RadTextBox txtEditRadVaccineName = (Telerik.Web.UI.RadTextBox)item.FindControl("txtEditRadVaccineName");
                    //btnRemove.Enabled = false;
                    //txtEditRadVaccineName.Enabled = false;
                    //item["Name"].Controls[0].Visible = false;

                }
            }






        }
        protected Boolean ValidateAddImmunisation(string immuOther, object immuDate, string administerd)
        {
            Boolean flag = true;
            string errorMsg = "";

            if (immuOther == "")
            {
                flag = false;
                errorMsg = errorMsg + "Immunisation Details Can Not Empty!.</br>";

            }
            if (immuDate == "" || immuDate == null)
            {
                flag = false;
                errorMsg = errorMsg + "Immunisation Date Can Not Empty!.</br>";
            }
            if (administerd != "Yes")
            {
                flag = false;
                errorMsg = errorMsg + "Immunisation Should Check.";
            }
            lblerrImmOther.Text = errorMsg;
            lblerrImmOther.ForeColor = System.Drawing.Color.Red;
            return flag;

        }
        protected void RadOtherVaccine_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {



            if (ViewState["TblImmunisation"] == null)
            {
                DtImmunisation = CreateImmunisationTable();
            }
            else
            {
                DtImmunisation = (DataTable)ViewState["TblImmunisation"];
            }
            DataRow DR = DtImmunisation.NewRow();

            int dtnextId = Convert.ToInt32(DtImmunisation.Rows.Count) + 1;


            if (e.CommandName == "Insert")
            {
                if (RadOtherVaccine != null)
                {
                    // GridItem[] footerItems = RadOtherVaccine.MasterTableView.GetItems(GridItemType.Footer);
                    GridFooterItem footeritem = (GridFooterItem)RadOtherVaccine.MasterTableView.GetItems(GridItemType.Footer)[0];



                    if (footeritem != null)
                    {
                        Telerik.Web.UI.RadTextBox txtimmuOther = (Telerik.Web.UI.RadTextBox)footeritem.FindControl("txtFooterRadVaccineName");
                        Telerik.Web.UI.RadButton btnFooterOthers = (Telerik.Web.UI.RadButton)footeritem.FindControl("btnFooterOthers");
                        Telerik.Web.UI.RadButton btnCatchupFooterOthers = (Telerik.Web.UI.RadButton)footeritem.FindControl("btnCatchupFooterOthers");
                        Telerik.Web.UI.RadDatePicker dtFooterOtherDate = (Telerik.Web.UI.RadDatePicker)footeritem.FindControl("dtFooterOtherDate");

                        if (ValidateAddImmunisation(txtimmuOther.Text.ToString(), dtFooterOtherDate.SelectedDate, btnFooterOthers.SelectedToggleState.Text.ToString()) == false)
                        {

                            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "JumpToGrid", "document.location = '#sGrid';", true);
                            return;


                        }


                        DR["ImmunisationOther"] = txtimmuOther.Text;
                        DR["ImmunisationDate"] = Convert.ToDateTime(dtFooterOtherDate.SelectedDate.ToString()).ToShortDateString();
                        DR["ImmunisationCU"] = btnCatchupFooterOthers.SelectedToggleState.Text;
                        DR["Administered"] = btnFooterOthers.SelectedToggleState.Text;
                        DR["ID"] = dtnextId.ToString() + txtimmuOther.Text;
                        DR["EditMode"] = "DELETE";
                        DtImmunisation.Rows.Add(DR);
                        ViewState["TblImmunisation"] = DtImmunisation;
                        RadOtherVaccine.DataSource = DtImmunisation;
                        RadOtherVaccine.DataBind();




                    }
                }

            }


            //GridDataItem row = RadGrid1.Items[rowNo];

        }
        protected void RadOtherVaccine_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItm = e.Item as GridDataItem;
            Label lblID = (Label)dataItm.FindControl("lblID");
            string Id = lblID.Text;
            DataTable table = (DataTable)ViewState["TblImmunisation"];
            table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

            if (table.Rows.Find(Id) != null)
            {
                table.Rows.Find(Id).Delete();
                table.AcceptChanges();
                ViewState["TblImmunisation"] = table;
                RadOtherVaccine.DataSource = table;
                RadOtherVaccine.DataBind();
            }
            else
            {
                RadOtherVaccine.DataSource = (DataTable)ViewState["TblImmunisation"];
                RadOtherVaccine.DataBind();

            }

        }
        protected void RadOtherVaccine_CancelCommand(object sender, GridCommandEventArgs e)
        {
            GetImmunisationDetails();
            //GetImmunisationDetaisOther();
            RadOtherVaccine.DataSource = (DataTable)ViewState["TblImmunisation"];
            RadOtherVaccine.DataBind();
            

        }
        protected void RadOtherVaccine_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            //GetImmunisationDetails();
            //GetImmunisationDetaisOther();
            //List<BIQTouchmmunisationFields> list1 = new List<BIQTouchmmunisationFields>();

            if (e.CommandName == RadGrid.UpdateCommandName)
            {
                if (e.Item is GridEditFormItem)
                {
                    GridEditFormItem item = (GridEditFormItem)e.Item;
                    Telerik.Web.UI.RadDatePicker dtEditOtherDate = (Telerik.Web.UI.RadDatePicker)item.FindControl("dtEditOtherDate");
                    Telerik.Web.UI.RadButton btnCatchupEditOthers = (Telerik.Web.UI.RadButton)item.FindControl("btnCatchupEditOthers");
                    Telerik.Web.UI.RadTextBox txtEditRadVaccineName = (Telerik.Web.UI.RadTextBox)item.FindControl("txtEditRadVaccineName");
                    Label lblIDEdit = (Label)item.FindControl("lblIDEdit");

                    string id = lblIDEdit.Text;
                    DataTable table = (DataTable)ViewState["TblImmunisation"];
                    table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };

                    if (table.Rows.Find(id) != null)
                    {
                        DataRow dr = table.Rows.Find(id);
                        dr["ImmunisationOther"] = txtEditRadVaccineName.Text.ToString();
                        dr["ImmunisationDate"] = Convert.ToDateTime(dtEditOtherDate.SelectedDate.ToString()).ToShortDateString();
                        dr["ImmunisationCU"] = CheckedVaue(btnCatchupEditOthers.SelectedToggleState.Text);
                        dr["EditMode"] = "New";
                        

                        table.AcceptChanges();
                        ViewState["TblImmunisation"] = table;
                        RadOtherVaccine.DataSource = table;
                        RadOtherVaccine.DataBind();
                    }




                }
            }







        }
        protected void RadOtherVaccine_EditCommand(object sender, GridCommandEventArgs e)
        {
            //GetImmunisationDetaisOther();
            RadOtherVaccine.DataSource = (DataTable)ViewState["TblImmunisation"];
            RadOtherVaccine.DataBind();


            




        }
        #endregion



    }

}