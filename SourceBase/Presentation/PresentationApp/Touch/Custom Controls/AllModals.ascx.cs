using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using Telerik.Web.UI;

using Interface.Clinical;
using Application.Common;
using Application.Presentation;
using Interface.Security;
using Interface.Administration;
using Touch.Custom_Forms;

namespace Touch.Custom_Controls
{
    public partial class AllModals : System.Web.UI.UserControl
    {
        public string PId, PtnSts, DQ;
        int LocationID;

        //protected void Page_Init(object sender, EventArgs e)
        //{

        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            LocationID = Convert.ToInt32(Session["AppLocationId"]);
            PId = Session["PatientId"].ToString();

            Meds[] myArray = new Meds[10] { 
                    new Meds("Rifampicin",false,false),
                    new Meds("Isoniazid",false,false),
                    new Meds("Ethambutol",false,true),
                    new Meds("Pyrazinamide",false,false),
                    new Meds("Streptomycin",false,false),
                    new Meds("Amikacin",true,false),
                    new Meds("Kanamycin",false,false),
                    new Meds("Capreomycin",false,false),
                    new Meds("Ofloxacin",false,false),
                    new Meds("Levofloxacin",false,false)
                };
            //rgdTBDrugsSensitivity.DataSource = myArray;
            //rgdTBDrugsSensitivity.DataBind();
            //rgdTBDrugsSensitivity.Visible = true;


            rgContactRecTreatment.DataSource = myArray;
            rgContactRecTreatment.DataBind();

            //Complaints[] mycomplaints = new Complaints[5]{
            //    new Complaints("Fever", false),
            //    new Complaints("Shortness of breath", false),
            //    new Complaints("Cough", false),
            //    new Complaints("Fever 1", false),
            //    new Complaints("Fever 2", false)
            //};
            //rgComplaints.DataSource = mycomplaints;
            //rgComplaints.DataBind();
            //rgComplaints.Visible = true;


            //view current forms code

            #region "Refresh Patient Records"
            IPatientHome PManager;
            PManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
            System.Data.DataSet thePDS = PManager.GetPatientDetails(Convert.ToInt32(Session["PatientId"]), Convert.ToInt32(Session["SystemId"]), Convert.ToInt32(Session["TechnicalAreaId"]));
            //System.Data.DataSet thePDS = PManager.GetPatientDetails(Convert.ToInt32(Request.QueryString["PatientId"]), Convert.ToInt32(Session["SystemId"]));

            Session["PatientInformation"] = thePDS.Tables[0];
            if (thePDS.Tables[0].Rows[0]["AGE"] != null)
            {
                Session["PatientAge"] = Convert.ToInt32(thePDS.Tables[0].Rows[0]["AGE"]);
            }
            #endregion

            IPatientHome PatientManager;
            PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");

            try
            {

                IFacilitySetup FacilityMaster = (IFacilitySetup)ObjectFactory.CreateInstance("BusinessProcess.Administration.BFacility, BusinessProcess.Administration");
                if (Session["PatientId"] == null || Convert.ToString(Session["PatientId"]) == "0")
                {
                    Session["PatientId"] = Request.QueryString["PatientId"];  //remove it after session of patient set on find add when patient selected from grid.
                }

                PId = Convert.ToString(Session["PatientId"]);
                PtnSts = Convert.ToString(Session["PatientStatus"]);
                if (Session["PatientId"] != null && Convert.ToInt32(Session["PatientId"]) != 0)
                {
                    PId = Session["PatientId"].ToString();
                }
                if (Session["PatientStatus"] != null)
                {
                    PtnSts = Session["PatientStatus"].ToString();
                }
                DataSet theDS = PatientManager.IQTouchGetPatientHistory(Convert.ToInt32(PId));
                ViewState["theCFDT"] = theDS.Tables[3].Copy();
               
                Page ParentPage = this.Parent.Page;
                UpdatePanel refreshUpt = (UpdatePanel)ParentPage.FindControl("updtAllModals");
                refreshUpt.Update();
                if (TreeViewExisForm.Nodes.Count == 0)
                {
                    TreeViewExisForm.Nodes.Clear();
                    FormIQCare(theDS);
                }
                if (Session["JustAddedRec"] != null)
                {
                    if (Session["JustAddedRec"].ToString() == "true")
                    {
                        TreeViewExisForm.Nodes.Clear();
                        FormIQCare(theDS);
                        Session["JustAddedRec"] = null;
                    }
                }

                if (Session["GoingBack"] != null)
                {
                    if (Session["GoingBack"].ToString() == "true")
                    {
                        TreeViewExisForm.Nodes.Clear();
                        FormIQCare(theDS);
                        Session["GoingBack"] = null;
                    }
                }

                updtVEF.Update();

            }

            catch (Exception err)
            {
                MsgBuilder theBuilder = new MsgBuilder();
                theBuilder.DataElements["MessageText"] = err.Message.ToString();
                IQCareMsgBox.Show("#C1", theBuilder, this);
                return;
            }
            finally
            {
                PatientManager = null;
            }
        }

        //protected void rdtExistingForms_OnNodeClick(object sender, RadTreeNodeEventArgs e)
        //{
        //    if (e.Node.Text == "Clinical Status")
        //    {
        //        divClinicalStatus.Visible = true;
        //        divNoData.Visible = false;
        //    }
        //    else
        //    {
        //        divClinicalStatus.Visible = false;
        //        divNoData.Visible = true;
        //    }
        //}
        public class Meds
        {
            public Meds(string Drug, bool Sensitive, bool Resistant)
            {
                _drug = Drug;
                _sensitive = Sensitive;
                _resistant = Resistant;

            }
            private string _drug;
            public string Drug
            {
                get { return _drug; }
                set { _drug = value; }
            }
            private bool _sensitive;
            public bool Sensitive
            {
                get { return _sensitive; }
                set { _sensitive = value; }
            }
            private bool _resistant;
            public bool Resistant
            {
                get { return _resistant; }
                set { _resistant = value; }
            }
        }

        public class Complaints
        {
            public Complaints(string Symptoms, bool YN)
            {
                _symptoms = Symptoms;
                _yn = YN;

            }
            private string _symptoms;
            public string Symptoms
            {
                get { return _symptoms; }
                set { _symptoms = value; }
            }
            private bool _yn;
            public bool YN
            {
                get { return _yn; }
                set { _yn = value; }
            }
        }


        // View Existing forms Code //

        private void FormIQCare(DataSet theDS)
        {
            try
            {
                IPatientHome PatientManager;
                PatientManager = (IPatientHome)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientHome, BusinessProcess.Clinical");
                int tmpYear = 0;
                int tmpMonth = 0;
                TreeNode root = new TreeNode();
                TreeNode theMRoot = new TreeNode();
                bool flagyear = true;
                int PtnPMTCTStatus = 0;
                int PtnARTStatus = 0;
                if (PtnSts == "0" || PtnSts == "")
                {
                    if (Session["PtnPrgStatus"] != null)
                    {
                        DataTable theStatusDT = (DataTable)Session["PtnPrgStatus"];
                        DataTable theCEntedStatusDT = (DataTable)Session["CEndedStatus"];
                        string PatientExitReason = string.Empty;
                        string PMTCTCareEnded = string.Empty;
                        string CareEnded = string.Empty;
                        if (theCEntedStatusDT.Rows.Count > 0)
                        {
                            PatientExitReason = Convert.ToString(theCEntedStatusDT.Rows[0]["PatientExitReason"]);
                            PMTCTCareEnded = Convert.ToString(theCEntedStatusDT.Rows[0]["PMTCTCareEnded"]);
                            CareEnded = Convert.ToString(theCEntedStatusDT.Rows[0]["CareEnded"]);
                        }
                        if ((theStatusDT.Rows[0]["PMTCTStatus"].ToString() == "PMTCT Care Ended") || (PatientExitReason == "93" && PMTCTCareEnded == "1"))
                            PtnPMTCTStatus = 1;
                        else
                            PtnPMTCTStatus = 0;
                        if ((theStatusDT.Rows[0]["ART/PalliativeCare"].ToString() == "Care Ended") || (PatientExitReason == "93" && CareEnded == "1"))
                            PtnARTStatus = 1;
                        else
                            PtnARTStatus = 0;
                    }
                }
                else
                {
                    PtnPMTCTStatus = 1;
                    PtnARTStatus = 1;
                }

                int YearIntCount = 0;
                int MonthIntCount = 0;
                foreach (DataRow theDR in theDS.Tables[1].Rows)
                {

                    if (((DateTime)theDR["TranDate"]).Year != 1900)
                    {
                        DQ = "";
                        if (tmpYear != ((DateTime)theDR["TranDate"]).Year)
                        {
                            root = new TreeNode();
                            root.Text = ((DateTime)theDR["TranDate"]).Year.ToString();
                            root.Value = "";
                            if (flagyear)
                            {
                                root.Expand();
                                flagyear = false;
                            }
                            else
                            {
                                root.Collapse();
                            }
                            TreeViewExisForm.Nodes.Add(root);
                            tmpYear = ((DateTime)theDR["TranDate"]).Year;
                            tmpMonth = 0;
                        }

                        if (tmpYear == ((DateTime)theDR["TranDate"]).Year && tmpMonth != ((DateTime)theDR["TranDate"]).Month)
                        {
                            theMRoot = new TreeNode();
                            theMRoot.Text = ((DateTime)theDR["TranDate"]).ToString("MMMM");
                            theMRoot.Value = "";
                            root.ChildNodes.Add(theMRoot);
                            tmpMonth = ((DateTime)theDR["TranDate"]).Month;
                        }

                        if (theDR["DataQuality"].ToString() == "1")
                        {
                            DQ = "Data Quality Done";

                        }

                        if (tmpYear == ((DateTime)theDR["TranDate"]).Year && tmpMonth == ((DateTime)theDR["TranDate"]).Month)
                        {
                            this.LocationID = Convert.ToInt32(theDS.Tables[0].Rows[0]["LocationID"].ToString());
                            TreeNode theFrmRoot = new TreeNode();
                            theFrmRoot.Text = "<span onclick='ShowMinLoading(\"RadWindowWrapper_allmodalsControl_rwViewExistingForms\");'>" + theDR["FormName"].ToString() + " ( " + ((DateTime)theDR["TranDate"]).ToString(Session["AppDateFormat"].ToString()) + " )</span>";
                            if (Convert.ToString(Session["TechnicalAreaId"]) == Convert.ToString(theDR["Module"]) || theDR["FormName"].ToString() == "Patient Registration")
                            {
                                if (DQ != "")
                                {
                                    theFrmRoot.ImageUrl = "~/images/15px-Yes_check.svg.png";
                                }
                                else
                                {
                                    theFrmRoot.ImageUrl = "~/Images/No_16x.ico";
                                }
                            }
                            else
                            {
                                //if (Convert.ToInt32(theDR["Module"]) > 2)
                                //{
                                //    theFrmRoot.ImageUrl = "~/Images/lock.jpg";
                                //    theFrmRoot.ImageToolTip = "You are Not Authorized to Access this Functionality";
                                //    theFrmRoot.SelectAction = TreeNodeSelectAction.None;
                                //}
                                if (Convert.ToString(Session["TechnicalAreaId"]) == Convert.ToString(theDR["Module"]) || (theDR["FormName"].ToString() == "Pharmacy") || (theDR["FormName"].ToString() == "Laboratory") || (theDR["FormName"].ToString() == "Paediatric Pharmacy"))
                                {
                                    if (Session["Paperless"].ToString() == "1")
                                    {
                                        if ((theDR["FormName"].ToString() == "Pharmacy") || (theDR["FormName"].ToString() == "Laboratory") || (theDR["FormName"].ToString() == "Paediatric Pharmacy"))
                                        {
                                            if (theDR["CAUTION"].ToString() == "1")
                                            {
                                                theFrmRoot.ImageUrl = "~/images/caution.png";
                                            }
                                            else
                                                theFrmRoot.ImageUrl = "~/images/15px-Yes_check.svg.png";
                                        }

                                    }
                                    else
                                    {
                                        if (DQ != "")
                                        {
                                            theFrmRoot.ImageUrl = "~/images/15px-Yes_check.svg.png";
                                        }
                                        else
                                        {
                                            theFrmRoot.ImageUrl = "~/Images/No_16x.ico";
                                        }
                                    }
                                }
                                else
                                {
                                    //theFrmRoot.ImageUrl = "~/Images/lock.jpg";
                                    //theFrmRoot.ImageToolTip = "You are Not Authorized to Access this Functionality";
                                    //theFrmRoot.SelectAction = TreeNodeSelectAction.None;
                                }
                            }
                            theFrmRoot.NavigateUrl = "";
                            theFrmRoot.Value = Convert.ToInt32(PId) + "%" + theDR["OrderNo"].ToString() + "%" + theDR["LocationID"].ToString() + "%" + PtnARTStatus + "%" + theDR["Module"].ToString() + "%" + theDR["FormName"].ToString() + "%" + theDR["ID"].ToString();
                            theMRoot.ChildNodes.Add(theFrmRoot);
                        }
                    }

                }

                //Commented by Jayant

                //foreach (DataRow theDR in theDS.Tables[4].Rows)
                //{
                //    bool YearExists = false; bool MonthExists = false;
                //    TreeNode YearExistsNode = new TreeNode();
                //    if (((DateTime)theDR["NoteDate"]).Year != 1900)
                //    {
                //        foreach (TreeNode _yearnode in TreeViewExisForm.Nodes)
                //        {
                //            if ((_yearnode.Text == ((DateTime)theDR["NoteDate"]).Year.ToString()) && (!YearExists))
                //            {
                //                YearExists = true;
                //                YearExistsNode = _yearnode;
                //                foreach (TreeNode _MonthNode in _yearnode.ChildNodes)
                //                {
                //                    if (_MonthNode.Text == ((DateTime)theDR["NoteDate"]).ToString("MMMM"))
                //                    {
                //                        MonthExists = true;
                //                        this.LocationID = Convert.ToInt32(theDS.Tables[0].Rows[0]["LocationID"].ToString());
                //                        TreeNode _ValueNode = new TreeNode();
                //                        _ValueNode.Text = "<span onclick='ShowMinLoading(\"RadWindowWrapper_allmodalsControl_rwViewExistingForms\");'>NV Clinical Note ( " + ((DateTime)theDR["NoteDate"]).ToString(Session["AppDateFormat"].ToString()) + " )</span>";
                //                        _ValueNode.ImageUrl = "~/images/15px-Yes_check.svg.png";
                //                        _ValueNode.NavigateUrl = "";
                //                        _ValueNode.Value = Convert.ToInt32(PId) + "%" + theDR["NoteID"].ToString() + "%" + LocationID.ToString() + "%1%0%NV Clinical Note";
                //                        _MonthNode.ChildNodes.Add(_ValueNode);
                //                    }
                //                }
                //            }
                //        }
                //        TreeNode _Yroot = new TreeNode();
                //        TreeNode _Mroot = new TreeNode();
                //        TreeNode _MValue = new TreeNode();
                //        if (!YearExists)
                //        {

                //            _Yroot.Text = ((DateTime)theDR["NoteDate"]).Year.ToString();
                //            _Yroot.Value = "";
                //            if (flagyear)
                //            {
                //                _Yroot.Expand();
                //                flagyear = false;
                //            }
                //            else
                //            {
                //                _Yroot.Collapse();
                //            }
                //            TreeViewExisForm.Nodes.Add(_Yroot);
                //            YearExists = true;
                //        }
                //        if (!MonthExists)
                //        {
                //            //add month node

                //            _Mroot.Text = ((DateTime)theDR["NoteDate"]).ToString("MMMM");
                //            _Mroot.Value = "";
                //            if (YearExists)
                //            {
                //                int ik = 0;
                //                for (int i = 0; i < TreeViewExisForm.Nodes.Count; i++)
                //                {
                //                    if (TreeViewExisForm.Nodes[i].Text.Equals(((DateTime)theDR["NoteDate"]).Year.ToString()))
                //                    {
                //                        ik = i;
                //                        i = TreeViewExisForm.Nodes.Count;
                //                    }
                //                }
                //                TreeNode YearNodefound = TreeViewExisForm.Nodes[ik];
                //                YearNodefound.ChildNodes.Add(_Mroot);
                //            }
                //            else
                //                _Yroot.ChildNodes.Add(_Mroot);

                //            //add value node
                //            MonthExists = true;
                //            this.LocationID = Convert.ToInt32(theDS.Tables[0].Rows[0]["LocationID"].ToString());

                //            _MValue.Text = "<span onclick='ShowMinLoading(\"RadWindowWrapper_allmodalsControl_rwViewExistingForms\");'>NV Clinical Note ( " + ((DateTime)theDR["NoteDate"]).ToString(Session["AppDateFormat"].ToString()) + " )</span>";
                //            _MValue.ImageUrl = "~/images/15px-Yes_check.svg.png";
                //            _MValue.NavigateUrl = "";
                //            _MValue.Value = Convert.ToInt32(PId) + "%" + theDR["NoteID"].ToString() + "%" + LocationID.ToString() + "%1%0%NV Clinical Note";
                //            _Mroot.ChildNodes.Add(_MValue);
                //        }
                //    }
                //}
                flagyear = true;
            }
            catch(Exception ex)
            {
                throw ex;

            }
        }
        protected void TreeViewExisForm_SelectedNodeChanged(object sender, EventArgs e)
        {   
            //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showradloading", "ShowMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
            //if (TreeViewExisForm.SelectedNode == null)
            //    return;
            string theNameVal = TreeViewExisForm.SelectedNode.Text.Replace("<span onclick='ShowMinLoading(\"RadWindowWrapper_allmodalsControl_rwViewExistingForms\");'>", "");
            theNameVal = theNameVal.Replace("</span>", "");
            string[] theName = theNameVal.Split('(');
            string[] theValue = TreeViewExisForm.SelectedNode.Value.Split('%');
            string url = "";
            string PgName;
            Session["PatientId"] = Convert.ToInt32(theValue[0]);
            Session["PatientVisitId"] = Convert.ToInt32(theValue[1]);
            Session["ServiceLocationId"] = Convert.ToInt32(theValue[2]);
            Session["PatientStatus"] = Convert.ToInt32(theValue[3]);
            Session["CEModule"] = theValue[4].ToString();
            Session["Visit_id"] = theValue[6].ToString();
            Session["Redirect"] = "1";

            switch (theName[0].Trim())
            {
                case "Patient Registration":
                    
                    string strOrderIDReg = Session["PatientVisitId"].ToString();
                    string strrefillReg = "";
                    Session["Refill"] = strrefillReg;
                    Session["IsFirstLoad"] = "true";
                    Page mpReg = (Page)this.Parent.Page;
                    PlaceHolder phReg = (PlaceHolder)mpReg.FindControl("phForms");
                    UpdatePanel uptReg = (UpdatePanel)mpReg.FindControl("updtForms");
                    Session["CurrentFormName"] = "frmRegistrationTouch";
                    Touch.Custom_Forms.frmRegistrationTouch frReg = (frmRegistrationTouch)mpReg.LoadControl("~/Touch/Custom Forms/frmRegistrationTouch.ascx");


                    Session["Orderid"] = Convert.ToInt32(strOrderIDReg);
                    frReg.ID = "ID" + Session["CurrentFormName"].ToString();
                    frmRegistrationTouch theFrmReg = (frmRegistrationTouch)phReg.FindControl("ID" + Session["CurrentFormName"].ToString());

                    // Handle Save button
                    HiddenField PhfReg = (HiddenField)mpReg.FindControl("hdSaveBtnVal");
                    UpdatePanel updtReg = (UpdatePanel)mpReg.FindControl("updtPatientSave");
                    PhfReg.Value = frReg.ID + "_btnSave_input";
                    updtReg.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                    // END 
                    foreach (Control item in phReg.Controls)
                    {
                        phReg.Controls.Remove(item);

                    }

                    if (theFrmReg != null)
                    {
                        theFrmReg.Visible = true;
                    }
                    else
                    {
                        phReg.Controls.Add(frReg);
                    }
                    //ph.DataBind();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                    //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);
                    uptReg.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "ShowPage(2);", true);
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);
                    break;

                case "HIV-Enrollment":
                    if (Convert.ToInt32(Session["TechnicalAreaId"]) == 2)
                    {
                        if (Session["SystemId"].ToString() == "1")
                        { PgName = "frmClinical_Enrolment.aspx"; }
                        else { PgName = "frmClinical_PatientRegistrationCTC.aspx"; }
                        url = string.Format("{0}", "" + PgName + "");
                        //Response.Redirect(url);
                    }
                    break;

                //case "Initial Evaluation":
                //    if (Convert.ToInt32(Session["TechnicalAreaId"]) == 2)
                //    {
                //        url = string.Format("{0}", "./frmClinical_InitialEvaluation.aspx");
                //        //Response.Redirect(url);
                //    }
                //    break;


                case "Prior ART/HIV Care":
                    if (Convert.ToInt32(Session["TechnicalAreaId"]) == 202)
                    {
                        url = string.Format("{0}", "./frm_PriorArt_HivCare.aspx");
                        //Response.Redirect(url);
                    }
                    break;

                case "ART Care":
                    if (Convert.ToInt32(Session["TechnicalAreaId"]) == 202)
                    {
                        url = string.Format("{0}", "./frmClinical_ARTCare.aspx");
                        //Response.Redirect(url);
                    }
                    break;

                //********************//
                //john - start
                case "ART Therapy":
                    if (Convert.ToInt32(Session["TechnicalAreaId"]) == 203)
                    {
                        url = string.Format("{0}", "./frmClinical_ARVTherapy.aspx");
                        //Response.Redirect(url);
                    }
                    break;
                //john - end

                case "ART History":
                    if (Convert.ToInt32(Session["TechnicalAreaId"]) == 203)
                    {
                        url = string.Format("{0}", "./frmClinical_ARTHistory.aspx");
                        //Response.Redirect(url);
                    }
                    break;

                case "Non Visit clinical note": 
                case  "Non Visit Clinical Note":
                    string strNoteID = Session["PatientVisitId"].ToString();
                    Session["ClinicalNoteEditMode"] = "true";
                    Session["IsFirstLoad"] = "true";
                    Session["ClinicalNoteID"] = strNoteID;
                    Page mpCN = (Page)this.Parent.Page;
                    PlaceHolder phCN = (PlaceHolder)mpCN.FindControl("phForms");
                    UpdatePanel uptCN = (UpdatePanel)mpCN.FindControl("updtForms");
                    Session["CurrentFormName"] = "frmClinicalNotesTouch";
                    Touch.Custom_Forms.frmClinicalNotesTouch frCN = (frmClinicalNotesTouch)mpCN.LoadControl("~/Touch/Custom Forms/frmClinicalNotesTouch.ascx");


                    frCN.ID = "ID" + Session["CurrentFormName"].ToString();
                    frmClinicalNotesTouch theFrmCN = (frmClinicalNotesTouch)phCN.FindControl("ID" + Session["CurrentFormName"].ToString());

                    // Handle Save button
                    HiddenField PhfCN = (HiddenField)mpCN.FindControl("hdSaveBtnVal");
                    UpdatePanel updtCN = (UpdatePanel)mpCN.FindControl("updtPatientSave");
                    PhfCN.Value = frCN.ID + "_btnSave_input";
                    updtCN.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                    // END 
                    foreach (Control item in phCN.Controls)
                    {
                        phCN.Controls.Remove(item);

                    }

                    if (theFrmCN != null)
                    {
                        theFrmCN.Visible = true;
                    }
                    else
                    {
                        phCN.Controls.Add(frCN);
                    }
                    //ph.DataBind();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);
                    uptCN.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "ShowPage(7);", true);
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);

                    break;
                
                case "Initial and Follow up Visits":
                case "Initial Evaluation":
                case "Visit":
                        string strVisitID = Session["PatientVisitId"].ToString();
                        Session["VisitEditMode"] = "true";
                        Session["IsFirstLoad"] = "true";
                        Session["VisitID"] = strVisitID;

                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "clearFormMode", "$('#FormMode').val('Loaded');", true);
                        ClearFVState();
                        Page mpVS = (Page)this.Parent.Page;
                        PlaceHolder phVS = (PlaceHolder)mpVS.FindControl("phForms");
                        UpdatePanel uptVS = (UpdatePanel)mpVS.FindControl("updtForms");
                        Session["CurrentFormName"] = "frmVisitTouch";
                        Touch.Custom_Forms.frmVisitTouch frVS = (frmVisitTouch)mpVS.LoadControl("~/Touch/Custom Forms/frmVisitTouch.ascx");


                        frVS.ID = "ID" + Session["CurrentFormName"].ToString();

                        phVS.Controls.Add(frVS);
                        uptVS.Update();
                        //Handle Save button
                        string FormID = "ID" + Session["CurrentFormName"].ToString();

                        HiddenField PhfVS = (HiddenField)mpVS.FindControl("hdSaveBtnVal");
                        UpdatePanel updtVS = (UpdatePanel)mpVS.FindControl("updtPatientSave");
                        PhfVS.Value = FormID + "_btnSave"; updtVS.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "swipe", "SwipeLeft();", true);
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);

                        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabs", "setTabs();", true);

                        //frmVisitTouch theFrmVS = (frmVisitTouch)phVS.FindControl("ID" + Session["CurrentFormName"].ToString());

                        //// Handle Save button
                        //HiddenField PhfVS = (HiddenField)mpVS.FindControl("hdSaveBtnVal");
                        //UpdatePanel updtVS = (UpdatePanel)mpVS.FindControl("updtPatientSave");
                        //PhfVS.Value = frVS.ID + "_btnSave";
                        //updtVS.Update();
                        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "swipe", "SwipeLeft();", true);
                        //// END 
                        //foreach (Control item in phVS.Controls)
                        //{
                        //    phVS.Controls.Remove(item);

                        //}
                    
                        //phVS.Controls.Clear();
                        //updtVS.Update();
                        ////Thread.Sleep(1000);

                        //    phVS.Controls.Add(frVS);

                
                        ////ph.DataBind();
                        
                        //uptVS.Update();
                        ////RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "ShowPage(1);", true);
                        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);
                        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                        ////RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);

                        break;

                case "Paediatric Pharmacy":
                case "Pharmacy":
                case "Touch Pharmacy":
                    if (TouchGlobals.ModuleName == "KNH")
                    {
                        string strOrderID = Session["PatientVisitId"].ToString();
                        string strrefill = "";
                        Session["Refill"] = strrefill;
                        Session["IsFirstLoad"] = "true";

                        Page mp2 = (Page)this.Parent.Page;
                        PlaceHolder ph2 = (PlaceHolder)mp2.FindControl("phForms");
                        UpdatePanel upt2 = (UpdatePanel)mp2.FindControl("updtForms");
                        Session["CurrentFormName"] = "frmKNHPharmacyTouch";
                        Touch.Custom_Forms.frmKNHPharmacyTouch fr2 = (frmKNHPharmacyTouch)mp2.LoadControl("~/Touch/Custom Forms/frmKNHPharmacyTouch.ascx");


                        Session["Orderid"] = Convert.ToInt32(strOrderID);
                        fr2.ID = "ID" + Session["CurrentFormName"].ToString();
                        frmKNHPharmacyTouch theFrm2 = (frmKNHPharmacyTouch)ph2.FindControl("ID" + Session["CurrentFormName"].ToString());

                        // Handle Save button
                        HiddenField Phf2 = (HiddenField)mp2.FindControl("hdSaveBtnVal");
                        UpdatePanel updt2 = (UpdatePanel)mp2.FindControl("updtPatientSave");
                        Phf2.Value = fr2.ID + "_btnSave_input";
                        updt2.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                        // END 
                        foreach (Control item in ph2.Controls)
                        {
                            ph2.Controls.Remove(item);

                        }

                        if (theFrm2 != null)
                        {
                            theFrm2.Visible = true;
                        }
                        else
                        {
                            ph2.Controls.Add(fr2);
                        }
                        //ph.DataBind();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);
                        upt2.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "ShowPage(6);", true);
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);

                    }
                    else
                    {


                        string strOrderID = Session["PatientVisitId"].ToString();
                        string strrefill = "";
                        Session["Refill"] = strrefill;
                        Session["IsFirstLoad"] = "true";
                        Page mp = (Page)this.Parent.Page;
                        PlaceHolder ph = (PlaceHolder)mp.FindControl("phForms");
                        UpdatePanel upt = (UpdatePanel)mp.FindControl("updtForms");
                        Session["CurrentFormName"] = "frmPharmacyTouch";
                        Touch.Custom_Forms.frmPharmacyTouch fr = (frmPharmacyTouch)mp.LoadControl("~/Touch/Custom Forms/frmPharmacyTouch.ascx");
                        

                        Session["Orderid"] = Convert.ToInt32(strOrderID);
                        fr.ID = "ID" + Session["CurrentFormName"].ToString();
                        frmPharmacyTouch theFrm = (frmPharmacyTouch)ph.FindControl("ID" + Session["CurrentFormName"].ToString());

                        // Handle Save button
                        HiddenField Phf = (HiddenField)mp.FindControl("hdSaveBtnVal");
                        UpdatePanel updt = (UpdatePanel)mp.FindControl("updtPatientSave");
                        Phf.Value = fr.ID + "_btnSave_input";
                        updt.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                        // END 
                        foreach (Control item in ph.Controls)
                        {
                            ph.Controls.Remove(item);

                        }

                        if (theFrm != null)
                        {
                            theFrm.Visible = true;
                        }
                        else
                        {
                            ph.Controls.Add(fr);
                        }
                        //ph.DataBind();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);
                        upt.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "SwipeLeft();", true);
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);

                    }
                    //}
                    break;
                //case "Pharmacy":
                //    url = string.Format("{0}", "~/./Pharmacy/frmPharmacyForm.aspx");                    
                //    break;
                
                case "ART Follow-Up":
                   
                    url = string.Format("{0}", "./frmClinical_ARTFollowup.aspx");
                    //theFrmRoot.NavigateUrl = url;
                    //Response.Redirect(url);
                    //}
                    break;
                case "HIV Care/ART Encounter":
                    url = string.Format("{0}", "./frmClinical_HIVCareARTCardEncounter.aspx");
                    //Response.Redirect(url);
                    break;
                //case "Initial and Follow up Visits":
                //    url = string.Format("{0}", "./frmClinical_InitialFollowupVisit.aspx");
                //    //Response.Redirect(url);
                //    break;

                case "Laboratory":
                case "Touch Laboratory":
                    //url = string.Format("{0}", "~/./Laboratory/frmLabOrder.aspx");
                        string strOrderID3 = Session["PatientVisitId"].ToString();
                        string strrefill3 = "";
                        Session["Refill"] = strrefill3;
                        Session["IsFirstLoad"] = "true";
                        Page mp3 = (Page)this.Parent.Page;
                        PlaceHolder ph3 = (PlaceHolder)mp3.FindControl("phForms");
                        UpdatePanel upt3 = (UpdatePanel)mp3.FindControl("updtForms");
                        Session["CurrentFormName"] = "frmLaboratoryTouch";
                        Touch.Custom_Forms.frmLaboratoryTouch fr3 = (frmLaboratoryTouch)mp3.LoadControl("~/Touch/Custom Forms/frmLaboratoryTouch.ascx");
                        

                        Session["Orderid"] = Convert.ToInt32(strOrderID3);
                        fr3.ID = "ID" + Session["CurrentFormName"].ToString();
                        frmLaboratoryTouch theFrm3 = (frmLaboratoryTouch)ph3.FindControl("ID" + Session["CurrentFormName"].ToString());

                        // Handle Save button
                        HiddenField Phf3 = (HiddenField)mp3.FindControl("hdSaveBtnVal");
                        UpdatePanel updt3 = (UpdatePanel)mp3.FindControl("updtPatientSave");
                        Phf3.Value = fr3.ID + "_btnSave_input";
                        updt3.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                        // END 
                        foreach (Control item in ph3.Controls)
                        {
                            ph3.Controls.Remove(item);

                        }

                        if (theFrm3 != null)
                        {
                            theFrm3.Visible = true;
                        }
                        else
                        {
                            ph3.Controls.Add(fr3);
                        }
                        //ph.DataBind();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                        //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);
                        upt3.Update();
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "ShowPage(3);", true);
                        RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);
                    break;
                case "Immunisation":
                    //url = string.Format("{0}", "~/./Laboratory/frmLabOrder.aspx");
                    string strOrderIDImm = Session["PatientVisitId"].ToString();
                    string strrefillImm = "";
                    Session["Refill"] = strrefillImm;
                    Session["IsFirstLoad"] = "true";
                    Page mpImm = (Page)this.Parent.Page;
                    PlaceHolder phImm = (PlaceHolder)mpImm.FindControl("phForms");
                    UpdatePanel uptImm = (UpdatePanel)mpImm.FindControl("updtForms");
                    Session["CurrentFormName"] = "frmImmunisationTouch";
                    Touch.Custom_Forms.frmImmunisationTouch frImm = (frmImmunisationTouch)mpImm.LoadControl("~/Touch/Custom Forms/frmImmunisationTouch.ascx");


                    Session["Orderid"] = Convert.ToInt32(strOrderIDImm);
                    frImm.ID = "ID" + Session["CurrentFormName"].ToString();
                    frmImmunisationTouch theFrmImm = (frmImmunisationTouch)phImm.FindControl("ID" + Session["CurrentFormName"].ToString());

                    // Handle Save button
                    HiddenField PhfImm = (HiddenField)mpImm.FindControl("hdSaveBtnVal");
                    UpdatePanel updtImm = (UpdatePanel)mpImm.FindControl("updtPatientSave");
                    PhfImm.Value = frImm.ID + "_btnSave_input";
                    updtImm.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                    // END 
                    foreach (Control item in phImm.Controls)
                    {
                        phImm.Controls.Remove(item);

                    }

                    if (theFrmImm != null)
                    {
                        theFrmImm.Visible = true;
                    }
                    else
                    {
                        phImm.Controls.Add(frImm);
                    }
                    //ph.DataBind();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                    //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);
                    uptImm.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "ShowPage(5);", true);
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);
                    break;
                

                case "Home Visit":
                    url = string.Format("{0}", "~/./Scheduler/frmScheduler_HomeVisit.aspx");
                    //Response.Redirect(url);
                    break;
                case "Non-ART Follow-Up":
                    if (Convert.ToInt32(Session["TechnicalAreaId"]) == 2)
                    {
                        url = string.Format("{0}", "./frmClinical_NonARTFollowUp.aspx");
                        //Response.Redirect(url);
                    }
                    break;
                case "Care Tracking":

                    string strOrderID1 = Session["PatientVisitId"].ToString();
                    Session["PatientVisitIdCareended"] = strOrderID1;
                    Session["IsFirstLoad"] = "true";
                    Page mp1 = (Page)this.Parent.Page;
                    PlaceHolder ph1 = (PlaceHolder)mp1.FindControl("phForms");
                    UpdatePanel upt1 = (UpdatePanel)mp1.FindControl("updtForms");

                    Session["CurrentFormName"] = "frmCareEndedTouch";

                    Touch.Custom_Forms.frmCareEndedTouch fr1 = (frmCareEndedTouch)mp1.LoadControl("~/Touch/Custom Forms/frmCareEndedTouch.ascx");
                    fr1.ID = "ID" + Session["CurrentFormName"].ToString();
                    frmCareEndedTouch theFrm1 = (frmCareEndedTouch)ph1.FindControl("ID" + Session["CurrentFormName"].ToString());

                    // Handle Save button
                    HiddenField Phf1 = (HiddenField)mp1.FindControl("hdSaveBtnVal");
                    UpdatePanel updt1 = (UpdatePanel)mp1.FindControl("updtPatientSave");
                    Phf1.Value = fr1.ID + "_btnSave_input";
                    updt1.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "shwSve", "$('#divSave').css('display', 'block');", true);

                    // END 
                    foreach (Control item in ph1.Controls)
                    {
                        ph1.Controls.Remove(item);
                        
                    }

                    if (theFrm1 != null)
                    {
                        theFrm1.Visible = true;
                    }
                    else
                    {
                        ph1.Controls.Add(fr1);
                    }
                    //ph.DataBind();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "closeradloading", "CloseMinLoading('RadWindowWrapper_allmodalsControl_rwViewExistingForms');", true);
                    //RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseWinFormView", "parent.CloseModalFromClient('rwViewExistingForms');", true);
                    upt1.Update();
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "showPrevPharm", "ShowPage(9);", true);
                    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "settabschild", "setTabs();", true);


                    break;

                //default: break;

            }

            //foreach (DataRow DRCustomFrm in ((DataTable)ViewState["theCFDT"]).Rows)
            //{
            //    if (DRCustomFrm["FeatureName"].ToString() == theName[0].Trim())
            //    {
            //        DataView theDV = new DataView((DataTable)ViewState["theCFDT"]);
            //        theDV.RowFilter = "FeatureName='" + theName[0].Trim() + "'";
            //        DataTable dtview = theDV.ToTable();
            //        Session["FeatureID"] = Convert.ToString(dtview.Rows[0]["FeatureID"]);
            //        AuthenticationManager Authentication = new AuthenticationManager();
            //        if (Authentication.HasFunctionRight(Convert.ToInt32(dtview.Rows[0]["FeatureID"]), FunctionAccess.View, (DataTable)Session["UserRight"]) == true)
            //        {
            //            url = string.Format("{0}", "./frmClinical_CustomForm.aspx");
            //            //Response.Redirect(url);
            //        }
            //        else
            //        {
            //            MsgBuilder theBuilder = new MsgBuilder();
            //            theBuilder.DataElements["MessageText"] = "You are Not Authorized to Access this Form.";
            //            IQCareMsgBox.Show("#C1", theBuilder, this);
            //        }

            //        //url = string.Format("{0}&patientid={1}&visitid={2}&locationid={3}&FormID={4}&sts={5}", "./frmClinical_CustomForm.aspx?name=Edit", Convert.ToInt32(PId), theDR["OrderNo"].ToString(), theDR["LocationID"].ToString(), DRCustomFrm["FeatureID"].ToString(), PtnPMTCTStatus);
            //        //theFrmRoot.NavigateUrl = url;
            //    }
            //}

            //TreeViewExisForm.SelectedNode.NavigateUrl = url;

        }

        protected void ClearFVState()
        {
            foreach (Control item in ((PlaceHolder)Parent.FindControl("phForms")).Controls)
            {
                ((PlaceHolder)Parent.FindControl("phForms")).Controls.Remove(item);
            }
            Session["FormIsLoaded"] = null; //Session["CurrentFormName"] = null;

            ((PlaceHolder)Parent.FindControl("phForms")).Controls.Clear();
            ((UpdatePanel)Parent.FindControl("updtForms")).Update();

            //updtAllModals.Update();
            //Thread.Sleep(2000);
        }
        // ### END ## //
    }
}