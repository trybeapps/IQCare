using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Application.Common;
using Application.Presentation;
using Interface.Administration;


public partial class AdminForms_frmAdmin_PMTCT_CustomItems : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["PatientId"] = 0;
            BindGrid();
        }

        //(Master.FindControl("lblRoot") as Label).Visible = false;
        //(Master.FindControl("lblMark") as Label).Visible = false;
        //(Master.FindControl("lblheader") as Label).Text = "Customize Lists";
        (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblRoot") as Label).Visible = false;
        (Master.FindControl("levelOneNavigationUserControl1").FindControl("lblheader") as Label).Text = "Customize Lists";
    }
    private void BindGrid()
    {
        if (Session["SystemId"].ToString() == "3")
        {
            TreeNode root = new TreeNode();
            TreeNode theChildRoot;
            TreeNode theMRoot;
            tvcustomlist.LeafNodeStyle.Font.Underline = true;
            tvcustomlist.LeafNodeStyle.ForeColor = System.Drawing.Color.Blue;
            bool flagroot = true;
            string stralpha = string.Empty;
            AuthenticationManager Authentication = new AuthenticationManager();
            root.Text = "List";
            if (flagroot)
            {
                root.Expand();
                flagroot = false;
            }
            else
            {
                root.Collapse();
            }
            tvcustomlist.Nodes.Add(root);
            #region "Universal Node"
            theMRoot = new TreeNode();
            theMRoot.Text = "PASDP Universal";
            theMRoot.Value = "0";
            theMRoot.Target = "_blank";
            theMRoot.Expand();
            root.ChildNodes.Add(theMRoot);
            
            string filePath = Server.MapPath("~/XMLFiles/customizelist.xml");
            DataSet dscustomiseList = new DataSet();
            dscustomiseList.ReadXml(filePath);
            DataView theDV = new DataView(dscustomiseList.Tables[0]);
            DataTable theXMLDT = new DataTable();
            theDV.RowFilter = "SystemId = 3";
            theDV.Sort = "ListName asc";
            DataTable objTable = theDV.ToTable();
            foreach (DataRow theUniDR in objTable.Rows)
            {
                string url = string.Empty;
                theChildRoot = new TreeNode();
                theChildRoot.Text = theUniDR["ListName"].ToString();
                theChildRoot.Target = "";
                url = string.Format("{0}?TableName={1}&CategoryId={2}&LstName={3}&Fid={4}&Upd={5}&CCID={6}&ModId={7}", theUniDR["FormName"].ToString(), theUniDR["TableName"].ToString(), theUniDR["CategoryId"].ToString(), theUniDR["ListName"].ToString(), theUniDR["FeatureID"].ToString(), theUniDR["Update"].ToString(), theUniDR["CountryID"].ToString(), theUniDR["ModuleId"]);
                theChildRoot.NavigateUrl = url;
                theMRoot.ChildNodes.Add(theChildRoot);
            }
            #endregion

        }
        else
        {
            string filePath = Server.MapPath("~/XMLFiles/customizelist.xml");
            DataSet dscustomiseList = new DataSet();
            dscustomiseList.ReadXml(filePath);

            DataView theDV = new DataView(dscustomiseList.Tables[0]);
            DataTable theXMLDT = new DataTable();
            IFacilitySetup FacilityManager = (IFacilitySetup)ObjectFactory.CreateInstance("BusinessProcess.Administration.BFacility, BusinessProcess.Administration");
            DataSet theDSFacility = FacilityManager.GetModuleName();
            DataTable theDTPubModule = (DataTable)Session["AppModule"];
            DataTable theDTModule = theDSFacility.Tables[1];

            string theModList = "";
            foreach (DataRow theDR in theDTModule.Rows)
            {
                if (theModList == "")
                    theModList = theDR["ModuleId"].ToString();
                else
                    theModList = theModList + "," + theDR["ModuleId"].ToString();
            }
            IQCareUtils theUtil = new IQCareUtils();
            theDV.RowFilter = "ModuleId in (0," + theModList + ")";
            if (theDV.Count > 0)
            {
                theDV.RowFilter = "(SystemId=" + Session["SystemId"].ToString() + " or SystemId=0)";
                theXMLDT = theUtil.CreateTableFromDataView(theDV);
            }
            ICustomList CustomManager = (ICustomList)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomList, BusinessProcess.Administration");
            DataTable theDT = CustomManager.GetCustomFieldList(Convert.ToInt32(Session["SystemId"]), Convert.ToInt32(Session["AppLocationId"]));
            if (theDT != null && theDT.Rows.Count > 0)
            {
                theXMLDT.Merge(theDT);
            }
            //////DataTable theDTPmtct = CustomManager.GetCustomFieldListPMTCT();
            //////if (theDTPmtct != null && theDTPmtct.Rows.Count > 0)
            //////{
            //////    theXMLDT.Merge(theDTPmtct);
            //////    theXMLDT.AcceptChanges();
            //////}

            #region "ParentNode"
            TreeNode root = new TreeNode();
            TreeNode theChildRoot;
            TreeNode theMRoot;
            tvcustomlist.LeafNodeStyle.Font.Underline = true;
            tvcustomlist.LeafNodeStyle.ForeColor = System.Drawing.Color.Blue;
            bool flag = true;
            bool flagroot = true;
            string stralpha = string.Empty;
            AuthenticationManager Authentication = new AuthenticationManager();
            root.Text = "List";
            if (flagroot)
            {
                root.Expand();
                flagroot = false;
            }
            else
            {
                root.Collapse();
            }
            tvcustomlist.Nodes.Add(root);
            #endregion

            #region "Universal Node"
            theMRoot = new TreeNode();
            theMRoot.Text = "Universal";
            theMRoot.Value = "0";
            theMRoot.Target = "_blank";
            theMRoot.Expand();
            root.ChildNodes.Add(theMRoot);

            theDV = new DataView(theXMLDT);
            theDV.RowFilter = "ModuleId = 0";
            theDV.Sort = "ListName asc";
            DataTable objTable = theDV.ToTable();
            foreach (DataRow theUniDR in objTable.Rows)
            {
                string url = string.Empty;
                theChildRoot = new TreeNode();
                theChildRoot.Text = theUniDR["ListName"].ToString();

                theChildRoot.Target = "";
                if ((theUniDR["FeatureID"].ToString() != "0") && (Authentication.HasFeatureRight(Convert.ToInt32(theUniDR["FeatureID"].ToString()), theXMLDT) == false))
                {
                    //theChildRoot.ImageUrl = "~/Images/lock.jpg";
                    //theChildRoot.ImageToolTip = "You are Not Authorized to Access this Functionality";
                    //theChildRoot.SelectAction = TreeNodeSelectAction.None;
                }
                else
                {
                    url = string.Format("{0}?TableName={1}&CategoryId={2}&LstName={3}&Fid={4}&Upd={5}&CCID={6}&ModId={7}", theUniDR["FormName"].ToString(), theUniDR["TableName"].ToString(), theUniDR["CategoryId"].ToString(), theUniDR["ListName"].ToString(), theUniDR["FeatureID"].ToString(), theUniDR["Update"].ToString(), theUniDR["CountryID"].ToString(), "0");
                    theChildRoot.NavigateUrl = url;
                    theChildRoot.ImageUrl = "~/Images/lock.jpg";
                    theChildRoot.ImageToolTip = "You are Not Authorized to Access this Functionality";
                    theChildRoot.SelectAction = TreeNodeSelectAction.None;
                }
                theMRoot.ChildNodes.Add(theChildRoot);
            }
            theDV = new DataView((DataTable)Session["UserFeatures"]);
            theDV.RowFilter = "ModuleId = 0 and FacilityId=0";
            theDV.Sort = "FeatureName asc";
            DataTable FrmRightsTable = theDV.ToTable();
            foreach (DataRow theFrmDR in FrmRightsTable.Rows)
            {
                for (int m = 0; m < theMRoot.ChildNodes.Count; m++)
                {
                    if (theFrmDR["FeatureName"].ToString() == theMRoot.ChildNodes[m].Text)
                    {
                        theMRoot.ChildNodes[m].ImageUrl = "";
                        theMRoot.ChildNodes[m].ImageToolTip = "";
                        theMRoot.ChildNodes[m].SelectAction = TreeNodeSelectAction.Select;
                    }
                }
            }
            if (Convert.ToInt32(Session["AppUserId"]) == 1)
            {
                for (int m = 0; m < theMRoot.ChildNodes.Count; m++)
                {

                    theMRoot.ChildNodes[m].ImageUrl = "";
                    theMRoot.ChildNodes[m].ImageToolTip = "";
                    theMRoot.ChildNodes[m].SelectAction = TreeNodeSelectAction.Select;

                }

            }


            #endregion

            #region "Module Nodes"
            //////////////////////////////////////////////////////////////////////
            string thePubModList = "";
            DataTable objPubTable;
            foreach (DataRow thepubDR in theDTPubModule.Rows)
            {
                if (thePubModList == "")
                    thePubModList = thepubDR["ModuleId"].ToString();
                else
                    thePubModList = thePubModList + "," + thepubDR["ModuleId"].ToString();
            }
            DataView thepubDV = new DataView(theXMLDT);
            thepubDV.RowFilter = "ModuleId in(" + thePubModList.ToString() + ")";
            thepubDV.Sort = "ListName asc";
            objPubTable = thepubDV.ToTable();
            ////////////////////////////////////////////////////////////
            foreach (DataRow theModRow in theDTModule.Rows)
            {
                theDV = new DataView(theXMLDT);
                theDV.RowFilter = "ModuleId =" + theModRow["ModuleId"].ToString();
                theDV.Sort = "ListName asc";
                objTable = theDV.ToTable();

                if (objTable.Rows.Count > 0)
                {
                    theMRoot = new TreeNode();
                    theMRoot.Text = theModRow["ModuleName"].ToString();
                    theMRoot.Value = theModRow["ModuleId"].ToString();
                    theMRoot.Target = "_blank";
                    root.ChildNodes.Add(theMRoot);

                    foreach (DataRow theUniDR in objTable.Rows)
                    {
                        string url = string.Empty;
                        theChildRoot = new TreeNode();
                        theChildRoot.Text = theUniDR["ListName"].ToString();

                        theChildRoot.Target = "";

                        if ((theUniDR["ModuleID"].ToString() != "0") && (Authentication.HasModuleRight(Convert.ToInt32(theUniDR["ModuleID"].ToString()), objPubTable) == false))
                        {
                            //    theChildRoot.ImageUrl = "~/Images/lock.jpg";
                            //    theChildRoot.ImageToolTip = "You are Not Authorized to Access this Functionality";
                            //    theChildRoot.SelectAction = TreeNodeSelectAction.None;
                        }
                        else
                        {
                            //url = string.Format("{0}?TableName={1}&CategoryId={2}&LstName={3}&Fid={4}&Upd={5}&CCID={6}", theUniDR["FormName"].ToString(), theUniDR["TableName"].ToString(), theUniDR["CategoryId"].ToString(), theUniDR["ListName"].ToString(), theUniDR["FeatureID"].ToString(), theUniDR["Update"].ToString(), theUniDR["CountryID"].ToString());
                            url = string.Format("{0}?TableName={1}&CategoryId={2}&LstName={3}&Fid={4}&Upd={5}&CCID={6}&ModId={7}", theUniDR["FormName"].ToString(), theUniDR["TableName"].ToString(), theUniDR["CategoryId"].ToString(), theUniDR["ListName"].ToString(), theUniDR["FeatureID"].ToString(), theUniDR["Update"].ToString(), theUniDR["CountryID"].ToString(), theUniDR["ModuleID"].ToString());
                            theChildRoot.NavigateUrl = url;
                            theChildRoot.ImageUrl = "~/Images/lock.jpg";
                            theChildRoot.ImageToolTip = "You are Not Authorized to Access this Functionality";
                            theChildRoot.SelectAction = TreeNodeSelectAction.None;
                        }
                        theMRoot.ChildNodes.Add(theChildRoot);
                    }
                }
                theDV = new DataView((DataTable)Session["UserFeatures"]);
                theDV.RowFilter = "ModuleId = " + theModRow["ModuleId"].ToString() + " and FacilityId=0";
                theDV.Sort = "FeatureName asc";
                FrmRightsTable = theDV.ToTable();
                foreach (DataRow theFrmDR in FrmRightsTable.Rows)
                {
                    for (int m = 0; m < theMRoot.ChildNodes.Count; m++)
                    {
                        if (theFrmDR["FeatureName"].ToString() == theMRoot.ChildNodes[m].Text)
                        {
                            theMRoot.ChildNodes[m].ImageUrl = "";
                            theMRoot.ChildNodes[m].ImageToolTip = "";
                            theMRoot.ChildNodes[m].SelectAction = TreeNodeSelectAction.Select;
                        }
                    }
                }

                if (Convert.ToInt32(Session["AppUserId"]) == 1)
                {
                    for (int m = 0; m < theMRoot.ChildNodes.Count; m++)
                    {
                        theMRoot.ChildNodes[m].ImageUrl = "";
                        theMRoot.ChildNodes[m].ImageToolTip = "";
                        theMRoot.ChildNodes[m].SelectAction = TreeNodeSelectAction.Select;
                    }
                }
            }
            #endregion

            ////ArrayList myData = new ArrayList();
            ////myData.Add("hello world");
            ////Session["AppModule"] = myData; //Add the data to session.
            ////ArrayList myDataRetrieved = (ArrayList)Session["AppModule"];

            ////IQCareUtils theUtil = new IQCareUtils();
            ////DataSet dscustomiseList;
            ////string[] arr = new string[100];
            ////arr.SetValue("Universal", 0);
            ////foreach (DataRow theDR in ((DataTable)Session["AppModule"]).Rows)
            ////{
            ////    arr.SetValue(theDR["ModuleName"].ToString(), Convert.ToInt32(theDR["ModuleId"]));
            ////}

            ////string filePath = Server.MapPath("~/XMLFiles/customizelist.xml");
            ////dscustomiseList = new DataSet();
            ////dscustomiseList.ReadXml(filePath);

            ////////
            ////dscustomiseList.WriteXmlSchema("c:\\testxml1.xml");
            ///////

            ////DataView theDV = new DataView(dscustomiseList.Tables[0]);
            ////DataTable theXMLDT = new DataTable();
            ////DataTable theDTModule = (DataTable)Session["AppModule"];
            ////string theModList = "";
            ////foreach (DataRow theDR in theDTModule.Rows)
            ////{
            ////    if (theModList == "")
            ////        theModList = theDR["ModuleId"].ToString();
            ////    else
            ////        theModList = theModList + "," + theDR["ModuleId"].ToString();
            ////}
            ////DataTable dt = theDV.ToTable();
            ////theDV.Sort = "ModuleId asc,Listname asc";
            ////theDV.RowFilter = "ModuleId in (0," + theModList + ")";
            ////if (theDV.Count > 0)
            ////{
            ////    theDV.RowFilter = "(SystemId=" + Session["SystemId"].ToString() + " or SystemId=0)";
            ////    theXMLDT = theUtil.CreateTableFromDataView(theDV);
            ////}

            ////ICustomList CustomManager = (ICustomList)ObjectFactory.CreateInstance("BusinessProcess.Administration.BCustomList, BusinessProcess.Administration");
            ////DataTable theDT = CustomManager.GetCustomFieldList(Convert.ToInt32(Session["SystemId"]), Convert.ToInt32(Session["AppLocationId"]));
            ////if (theDT != null && theDT.Rows.Count > 0)
            ////{
            ////    theXMLDT.Merge(theDT);
            ////}
            ////DataTable theDTPmtct = CustomManager.GetCustomFieldListPMTCT();
            ////if (theDTPmtct != null && theDTPmtct.Rows.Count > 0)
            ////{
            ////    theXMLDT.Merge(theDTPmtct);
            ////}

            ////theDV = new DataView(theXMLDT);
            ////theDV.Sort = "ModuleId asc,Listname asc";
            ////DataTable objTable = new DataTable();
            ////objTable = theDV.ToTable();
            ////TreeNode root = new TreeNode();
            ////TreeNode theChileRoot = new TreeNode();
            ////TreeNode theMRoot = new TreeNode();
            ////tvcustomlist.LeafNodeStyle.Font.Underline = true;
            ////tvcustomlist.LeafNodeStyle.ForeColor = System.Drawing.Color.Blue;
            ////bool flag = true;
            ////bool flagroot = true;
            ////string stralpha = string.Empty;
            ////AuthenticationManager Authentication = new AuthenticationManager();
            ////root.Text = "List";
            ////if (flagroot)
            ////{
            ////    root.Expand();
            ////    flagroot = false;
            ////}
            ////else
            ////{
            ////    root.Collapse();
            ////}
            ////tvcustomlist.Nodes.Add(root);

            ////foreach (DataRow theDR in objTable.Rows)
            ////{
            ////    if (stralpha != theDR["ModuleId"].ToString())
            ////    {
            ////        theChileRoot = new TreeNode();
            ////        theChileRoot.Text = arr[Convert.ToInt32(theDR["ModuleId"])];
            ////        theChileRoot.Target = "_blank";

            ////        if (flag)
            ////        {
            ////            theChileRoot.Expand();
            ////            flag = false;
            ////        }
            ////        else
            ////        {
            ////            theChileRoot.Collapse();
            ////        }
            ////        root.ChildNodes.Add(theChileRoot);
            ////        stralpha = theDR["ModuleId"].ToString();
            ////    }
            ////    if (stralpha == theDR["ModuleId"].ToString())
            ////    {
            ////        string url = string.Empty;
            ////        theMRoot = new TreeNode();
            ////        theMRoot.Text = theDR["ListName"].ToString();

            ////        theMRoot.Target = "";
            ////        if ((theDR["FeatureID"].ToString() != "0") && (Authentication.HasFeatureRight(Convert.ToInt32(theDR["FeatureID"].ToString()), dt) == false))
            ////        {
            ////            theMRoot.ImageUrl = "~/Images/lock.jpg";
            ////            theMRoot.ImageToolTip = "You are Not Authorized to Access this Functionality";
            ////            theMRoot.SelectAction = TreeNodeSelectAction.None;
            ////        }
            ////        else
            ////        {
            ////            url = string.Format("{0}?TableName={1}&CategoryId={2}&LstName={3}&Fid={4}&Upd={5}&CCID={6}", theDR["FormName"].ToString(), theDR["TableName"].ToString(), theDR["CategoryId"].ToString(), theDR["ListName"].ToString(), theDR["FeatureID"].ToString(), theDR["Update"].ToString(), theDR["CountryID"].ToString());
            ////            theMRoot.NavigateUrl = url;
            ////        }
            ////        theChileRoot.ChildNodes.Add(theMRoot);
            ////    }
            ////}
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../frmFacilityHome.aspx");
    }


}
