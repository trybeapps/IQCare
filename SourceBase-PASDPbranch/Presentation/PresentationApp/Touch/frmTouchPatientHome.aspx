<%@ Page Language="C#" AutoEventWireup="True" Inherits="Touch.frmTouchPatientHome"
    CodeBehind="frmTouchPatientHome.aspx.cs" %>

<%@ Register TagPrefix="KNHuc" TagName="KNHModal" Src="~/Touch/KNH/KNHModal.ascx" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title runat="server" id="AppTitle">Patient Home Screen</title>
    <!-- Jquery standard files -->
    <script type="text/javascript" src="Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="Styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="Stylesheet" href="Styles/custom-theme/jquery-ui-1.10.3.custom.min.css"
        type="text/css" />
    <!-- PASDP Style sheet -->
    <link id="lnkhomepage" rel="Stylesheet" runat="server" type="text/css" media="all" />
    <!-- mbExtruder includes for the slide out bar -->
    <link href="Styles/mbExtruder.css" media="all" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/mbExtruder.js"></script>
    <script type="text/javascript" src="Scripts/jquery.mb.flipText.js"></script>
    <!-- Scroller for menu -->
    <script type="text/javascript" src="Scripts/jquery.scrollTo-1.4.3.1-min.js"></script>
    <!-- Scroller for DIV -->
    <script src="Scripts/jquery.jscrollpane.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="Scripts/mwheelIntent.js" type="text/javascript"></script>
    <link href="Styles/jquery.jscrollpane.css?reload" rel="stylesheet" type="text/css" />
    <link id="lnkscroll" rel="stylesheet" type="text/css" runat="server" />
    <link href="Scripts/keyb/keyboard.css" rel="stylesheet" />
    <!-- IQTouch js files -->
    <script src="scripts/Pharmacy.js?reload=true" type="text/javascript"></script>
    <script src="scripts/KNHExpress.js?reload=true" type="text/javascript"></script>
    <style type="text/css">
        .rgRow
        {
            cursor: pointer !important;
        }
        .rgAltRow
        {
            cursor: pointer !important;
        }
        .preference .rwWindowContent
        {
            background-color: White !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="rsm" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" EnableAJAX="true" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAdd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divAdd" LoadingPanelID="statsRadLPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <!--hidden objects  -->
    <input type="button" id="hdAddPatient" style="visibility: collapse;" />
    <asp:HiddenField ID="hdSaveBtnVal" runat="server" />
    <asp:HiddenField ID="hddSavealert" runat="server" />
    <asp:HiddenField ID="hidmodule" runat="server" />
    <asp:HiddenField ID="Ptn_PkVal" runat="server" />
    <asp:HiddenField ID="locationIDVal" runat="server" />
    <asp:HiddenField ID="Ptn_DOB" runat="server" />
    <input type="hidden" id="hidFormLoaded" runat="server" />
    <input type="hidden" id="hidJustAddPat" runat="server" />
    <input type="hidden" id="FormMode" runat="server" />
    
    <!--menu buttons -->
    <asp:UpdatePanel ID="updtHdButtons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnGoToVisit" runat="server" CssClass="hiddenButtons" OnClick="btnGoToVisit_Click" />
            <asp:Button ID="btnGoToRegistration" runat="server" CssClass="hiddenButtons" OnClick="btnGoToRegistration_Click" />
            <asp:Button ID="btnGotoLab" runat="server" CssClass="hiddenButtons" OnClick="btnGotoLab_Click" />
            <asp:Button ID="btnGoToHist" runat="server" CssClass="hiddenButtons" OnClick="btnGoToHist_Click" />
            <asp:Button ID="btnGoToImmun" runat="server" CssClass="hiddenButtons" OnClick="btnGoToImmun_Click" />
            <asp:Button ID="btnGoToPharm" runat="server" CssClass="hiddenButtons" OnClick="btnGoToPharm_Click" />
            <asp:Button ID="btnGoToNon" runat="server" CssClass="hiddenButtons" OnClick="btnGoToNon_Click" />
            <asp:Button ID="btnGoToRep" runat="server" CssClass="hiddenButtons" OnClick="btnGoToRep_Click" />
            <asp:Button ID="btnGoToCare" runat="server" CssClass="hiddenButtons" OnClick="btnGoToCare_Click" />
            <asp:Button ID="btnClearViewState" runat="server" CssClass="hiddenButtons" OnClick="btnClearViewState_Click" />
            <asp:Button ID="btnExpress" runat="server" CssClass="hiddenButtons" OnClick="btnExpress_Click" />
            <asp:Button ID="btnAdultIE" runat="server" CssClass="hiddenButtons" OnClick="btnAdultIE_Click" />
            <asp:Button ID="btnGoToPat" runat="server" CssClass="hiddenButtons" OnClick="btnGoToPat_Click" />
            <asp:HiddenField ID="refreshViewExisting" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <div id="wrapper">
            <div id="TopPane">
                <telerik:RadScriptBlock runat="server" ID="rscb1">
                    <div style="float: left; padding: 10px 0 0 100px;">
                        <div class="btnNav" id="btnHome" onclick="GoToFacility('Home', $('#FormMode').val())">
                            <asp:Image ID="imghome" AlternateText="Home" runat="server" />
                            <br />
                            <span class="lablename">Home</span>
                        </div>
                        <div class="btnNav" onclick="GoToFacility('Search', $('#FormMode').val())">
                            <asp:Image ID="imgfindadd" AlternateText="Find / Add" runat="server" />
                            <br />
                            <span class="lablename">Find/Add</span>
                        </div>
                        <div class="btnNav">
                            <asp:UpdatePanel ID="updtPatientSave" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div onclick="$('#<%=hdSaveBtnVal.Value%>').click();" style="display: none;" id="divSave">
                                        <asp:Image ID="imgsave" AlternateText="Save" runat="server" />
                                        <br />
                                        <span class="lablename">Save</span>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="btnNav" onclick="BackBtnClick();" style="display: none;" id="divBackBtn">
                            <asp:Image ID="imgback" AlternateText="Back" runat="server"  />
                           
                            <br />
                            <span class="lablename">Back</span>
                        </div>
                        <div class="btnNav" style="display: block;" id="divMore">
                            <asp:Image ID="imgmore" AlternateText="More" runat="server" />
                            <br />
                            <span class="lablename">More</span>
                        </div>
                        <div class="btnNav" id="divFacilityName" style="width: 400px; text-align: center;">
                            <h3 class="lablename">
                                <asp:Label ID="lblFacilityName" runat="server"></asp:Label></h3>
                        </div>
                    </div>
                </telerik:RadScriptBlock>
                <div class="btnNav">
                    <div class="more-menu" style="display: none;" id="moreMenu">
                        <div>
                            <ul>
                                <li id="viewBut"><a href="#" onclick="OpenModalFromClient('rwViewExistingForms');HideMoreMenu('moreMenu');">
                                    View existing forms</a></li>
                                <li><a href="../AdminForms/frmAdmin_FacilityList.aspx" target="_blank" onclick="HideMoreMenu('moreMenu')">
                                    Administration</a></li>
                                <li><a href="../Touch/Help/index.html" target="_blank" onclick="HideMoreMenu('moreMenu')">
                                    User Guide</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div id="detailSection">
                <div id="middlePane" style="position: relative; width: 1050px; height: 509px; float: left;
                    z-index: 99; overflow: hidden">
                    <div id="phMenu" style="position: relative; float: left; margin-left: -250px;">
                        &nbsp;</div>
                    <table>
                        <tr>
                            <td>
                                <div id="theContent">
                                    <asp:Image ID="imgvisit" CssClass="MenuButs" alt="Visit" onclick="ShowPage(1)" runat="server" />
                                    <asp:Image ID="imgreg" CssClass="MenuButs" alt="Registration" onclick="ShowPage(2)"
                                        runat="server" />
                                    <asp:Image ID="imgpharmacy" CssClass="MenuButs" alt="Pharmacy" onclick="ShowPage(6)"
                                        runat="server" />
                                    <asp:Image ID="imgimu" CssClass="MenuButs" alt="Immunisation" onclick="ShowPage(5)"
                                        runat="server" />
                                    <asp:Image ID="imglogo" Style="cursor: default" CssClass="MenuButs" alt="Logo" runat="server" />
                                    <asp:Image ID="imglab" CssClass="MenuButs" alt="Laboratory" onclick="ShowPage(3)"
                                        runat="server" />
                                    <asp:Image ID="imgnov" CssClass="MenuButs" alt="Non Visit" onclick="ShowPage(7)"
                                        runat="server" />
                                    <asp:Image ID="imgreport" CssClass="MenuButs" alt="Reports" onclick="ShowPage(8)"
                                        runat="server" />
                                    <asp:Image ID="imgcare" CssClass="MenuButs" alt="Care Ended" onclick="ShowPage(9)"
                                        runat="server" />
                                    <asp:Image ID="imgAdultFuKNH" runat="server" CssClass="MenuButs" onclick="ShowPage(10)" />
                                    <asp:Image ID="imgAdultIEKNH" runat="server" CssClass="MenuButs" onclick="ShowPage(11)" />
                                    <asp:Image ID="imgExpressKNH" runat="server" CssClass="MenuButs" onclick="ShowPage(12)" />
                                    <asp:Image ID="imgPedFUKNH" runat="server" CssClass="MenuButs" onclick="ShowPage(13)" />
                                    <asp:Image ID="imgPedIEKNH" runat="server" CssClass="MenuButs" onclick="ShowPage(14)" />
                                    <asp:Image ID="imgPsychotherapyFormknh" runat="server" CssClass="MenuButs" onclick="ShowPage(15)" />
                                </div>
                            </td>
                            <td>
                                <div id="theForm" runat="server" style="width: 1200px; overflow: hidden; height: 509px;
                                    float: left; padding: 30px 0 0 50px;">
                                    <div id="somediv" runat="server" style="margin: 0 0 0 50px; padding: 0 0 0 0; width: 861px;
                                        height: 509px; overflow: hidden;">
                                        <asp:UpdatePanel ID="updtAllModals" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <futures:allmodals ID="allmodalsControl" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <KNHuc:KNHModal ID="TheModalWin" runat="server" />
                                        <div class="updtForms">
                                            <asp:UpdatePanel ID="updtForms" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                                <ContentTemplate>
                                                    <asp:PlaceHolder ID="phForms" runat="server"></asp:PlaceHolder>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="footer">
            </div>
        </div>
    </div>
    
    <div id="extruderLeft" class="a {title:'<% Response.Write(lblName.Text); %>'}" style="font-size: 12px;
        z-index: 100">
        <div id="LeftPane" class="LeftPane">
        
            <asp:Image ID="imgleftlogo" Style="height: 44px; width: 187px; margin-top: 10px;"
                runat="server" />
            <br />
            <table>
                <tr>
                    <td colspan="2" style="color: rgb(12, 25, 141); font-weight: bold; font-size: 18px;">
                        <asp:Label ID="lblName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Folder #:
                    </td>
                    <td>
                        <asp:Label ID="lblFolderNo" Text="No D9 Available" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Sex:
                    </td>
                    <td>
                        <asp:Label ID="lblSex" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        DOB:
                    </td>
                    <td>
                        <asp:Label ID="lblDOB" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Age:
                    </td>
                    <td>
                        <asp:Label ID="lblAge" runat="server"></asp:Label><!-- 0 yrs, 0 mths-->
                    </td>
                </tr>
                <tr>
                    <td>
                        District:
                    </td>
                    <td>
                        <asp:Label ID="lblDistrict" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Phone:
                    </td>
                    <td>
                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Status:
                    </td>
                    <td style="color: Green">
                        <asp:UpdatePanel ID="updtStatusActive" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                &nbsp;
                                <asp:LinkButton ForeColor="#FBB613" ID="btnStatusActivate" Visible="false" Text="Activate"
                                    runat="server" OnClick="btnStatusActivate_Click" OnClientClick="javascript:event.stopPropagation();"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <img src="images\divider.png" alt="divider" />
            <br />
            <asp:UpdatePanel ID="updtSideBar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td></td>
                        <td>Last Visit</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Label ID="lblsidelastvisit" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="color: rgb(251, 182, 19)">
                            Rx
                        </td>
                        <td colspan="2">
                            | Last Regimen
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="color: rgb(251, 182, 19)">
                            <asp:Label ID="lbllastregimen" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 7px; padding-left: 6px;">
                            <img src="images/upArrow.jpg" alt="↑" />
                        </td>
                        <td>
                            | Most Recent CD4
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblmostresentcd4" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 7px; padding-left: 10px; color: rgb(251, 182, 19)">
                            !
                        </td>
                        <td>
                            | Next CD4 Due
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblnextcd4date" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 7px; padding-left: 10px; color: rgb(251, 182, 19)">
                            !
                        </td>
                        <td>
                            | Viral Load
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblviralload" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <img src="images\divider.png" alt="divider" />
                <br />
                <span class="quickbarLinklab" onclick="ShowGraphs('divlabAlert')">Lab Alerts</span><br />
                <span class="quickbarLink" onclick="ShowGraphs('divCD4Graph')">CD4-Viral Load Chart</span><br />
                <span class="quickbarLink" onclick="ShowGraphs('divBMIGraph')">Weight-BMI Chart</span><br />
                <span class="quickbarLink" onclick="ShowGraphs('divshowlabs')">Latest labs</span><br />
<!--                <span class="quickbarLink" onclick="ShowGraphs('divshowlabs')">Latest labs</span><br />-->
                <!--<span class="quickbarLink" onclick="ShowGraphs('divARTHistory')">ARV History</span><br />-->
                <!--<span class="quickbarLink">Growth Chart</span><br />
                <span class="quickbarLink">Latest Labs</span><br />
                <span class="quickbarLink">ARV History</span><br />-->
                <div id="divlabAlert" onclick="ShowGraphs('divlabAlert')" style="overflow: scroll;
                    width: 700px; height: 400px; position: fixed; left: 250px; top: 154px; padding-left: 10px; background-color: white;
                    display: none">
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2" align="right">
                                <telerik:RadButton ID="btnprintalert" runat="server" OnClientClicked="fnprint" Text="Print">
                                </telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadGrid ID="rgvlabalerts" Skin="MetroTouch" AutoGenerateColumns="false"
                                    runat="server">
                                    <MasterTableView PageSize="5" Font-Size="10" DataKeyNames="subTestName">
                                        <Columns>
                                            <telerik:GridBoundColumn SortExpression="subTestName" HeaderText="Lab Test Name"
                                                HeaderButtonType="TextButton" DataField="subTestName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="OrderedByDate" HeaderText="Date Ordered"
                                                HeaderButtonType="TextButton" DataField="OrderedByDate">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="Results" HeaderText="Result" HeaderButtonType="TextButton"
                                                DataField="Results">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="PnlConFields" runat="server" Wrap="true">
                    </asp:Panel>
                </div>
                <div id="divCD4Graph" onclick="ShowGraphs('divCD4Graph')" style="width: 600px; height: 400px;
                    position: fixed; left: 250px; top: 154px; background-color: white; display: none">
                    <chart:WebChartViewer ID="WebChartViewerCD4VL" runat="server" Height="400px" Width="600px" />
                </div>
                <div id="divBMIGraph" onclick="ShowGraphs('divBMIGraph')" style="width: 600px; height: 400px;
                    position: fixed; left: 250px; top: 154px; background-color: white; display: none">
                    <chart:WebChartViewer ID="WebChartViewerWeight" runat="server" Height="400px" Width="600px" />
                </div>

                <div id="divshowlabs" onclick="DivShowLabsUpdate()" style="width: 700px; height: 350px; overflow:scroll;
                    position: fixed; left: 250px; top: 154px; background-color: white; display: none">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="2" align="right">
                                <telerik:RadButton ID="btnRefreshLabs" runat="server" Text="Refresh" AutoPostBack="true" onclick="btnInvisibleUpdateLabHistory_Click">
                                </telerik:RadButton>
                                <telerik:RadButton ID="btnprint" runat="server" OnClientClicked="fnprint" Text="Print">
                                </telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lbllabname"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbldate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                    <telerik:RadGrid ID="rgvlabhistory" Skin="MetroTouch" AutoGenerateColumns="false"
                                        runat="server">
                                        <MasterTableView PageSize="5" Font-Size="10" DataKeyNames="subTestName">
                                            <Columns>
                                                <telerik:GridBoundColumn SortExpression="subTestName" HeaderText="Lab Test Name"
                                                    HeaderButtonType="TextButton" DataField="subTestName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="TestResults" HeaderText="Result" HeaderButtonType="TextButton"
                                                    DataField="TestResults">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="Reportedbydate" HeaderText="Date Reported"
                                                    HeaderButtonType="TextButton" DataField="Reportedbydate">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divARTHistory" onclick="ShowGraphs('divARTHistory')" style="width: 700px;
                    height: 400px; position: fixed; left: 250px; top: 154px; background-color: white;
                    display: none">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 25%">
                                ART/Palliative Care:
                            </td>
                            <td style="width: 25%">
                                <asp:Label ID="lblprogram" runat="server"></asp:Label>
                            </td>
                            <td style="width: 25%">
                                Current ARV Regimen
                            </td>
                            <td style="width: 25%">
                                <asp:Label ID="lblarvregimen" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Current ARV Start Date:
                            </td>
                            <td>
                                <asp:Label ID="lblarvstartdate" runat="server"></asp:Label>
                            </td>
                            <td>
                                ARV Start Date At This Facility:
                            </td>
                            <td>
                                <asp:Label ID="lblaidsrstartdate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Historical ART Start Date:
                            </td>
                            <td>
                                <asp:Label ID="lblhistoricalsdate" runat="server"></asp:Label>
                            </td>
                            <td>
                                Last Visit:
                            </td>
                            <td>
                                <asp:Label ID="lbllstvisit" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Next Appointment:
                            </td>
                            <td>
                                <asp:Label ID="lblnextapp" runat="server"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger  ControlID="btnRefreshLabs"/>
            </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div style="position: absolute; float: left; top: 40px; left: 700px; z-index: 150;">
        <a class="dr-icon dr-icon-user" href="#" style="color: White; text-decoration: none;
            margin-right: 10px;">
            <asp:Label class="lablename" ID="lblUserName" runat="server"></asp:Label></a>
        <a class="dr-icon dr-icon-switch" href="../frmLogOff.aspx?Touch=true" style="color: White;
            text-decoration: none;"><span class="lablename">Logout</span> </a>
    </div>
    <telerik:RadWindow runat="server" ID="rwFindAdd" VisibleOnPageLoad="false" Title="Find/Add Patient"
        Modal="true" Width="880px" Height="470px" Behaviors="Move,Close">
        <ContentTemplate>
            <asp:UpdatePanel ID="updtWindow" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div id="jAccordion" style="width: 850px">
                        <h3>
                            Find Patient <span style="font-size: 12px">-- Enter search criteria (full or partial
                                text)</span>
                        </h3>
                        <div id="divFind" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="800px" id="findtable" cellpadding="5px">
                                        <tr>
                                            <td colspan="1">
                                                Identification Number
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadTextBox ID="txtIdNo" runat="server" Width="538px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                First Name:
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtFName" runat="server">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                Last Name:
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtLName" runat="server">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblChangeName" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtFolderNo" runat="server">
                                                </telerik:RadTextBox>
                                                <telerik:RadComboBox ID="cmbsex" runat="server">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                Date of Birth:
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtpDOBs" runat="server">
                                                    <Calendar ID="Calendar2" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        ShowRowHeaders="false" runat="server">
                                                    </Calendar>
                                                    <DateInput ID="DateInput2" DisplayDateFormat="dd MMM yyyy" DateFormat="dd-MMM-yyyy"
                                                        LabelWidth="0px" runat="server">
                                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                        <FocusedStyle Resize="None"></FocusedStyle>
                                                        <DisabledStyle Resize="None"></DisabledStyle>
                                                        <InvalidStyle Resize="None"></InvalidStyle>
                                                        <HoveredStyle Resize="None"></HoveredStyle>
                                                        <EnabledStyle Resize="None"></EnabledStyle>
                                                    </DateInput>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1">
                                                <!--Service-->
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadComboBox ID="rcbService" Visible="false" runat="server" Width="538px">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div id="divfacility" runat="server">
                                                    <table width="800px" border="0" cellpadding="0px">
                                                        <tr>
                                                            <td style='width: 23%'>
                                                                Patient Status:
                                                            </td>
                                                            <td style='width: 22%'>
                                                                <telerik:RadComboBox ID="rcbpatientstatus" runat="server">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="0" Text="Acitve" />
                                                                        <telerik:RadComboBoxItem Value="1" Text="Care Ended" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                            <td align="center" style='width: 25%'>
                                                                Facility:
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="cmbfacility" Width="180px" runat="server">
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadButton ID="btnSearch" runat="server" Text="Find" OnClientClicked="showbar"
                                                    OnClick="btnSearch_Click">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div style="cursor: pointer !important">
                                                    <telerik:RadGrid ID="rgResults" runat="server" Visible="false" AllowPaging="true"
                                                        OnNeedDataSource="rgResults_NeedDataSource" OnColumnCreated="rgResults_ColumnCreated">
                                                        <MasterTableView PageSize="5" ClientDataKeyNames="Ptn_Pk,locationID" Font-Size="8">
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Selecting AllowRowSelect="true" />
                                                            <ClientEvents OnRowSelected="RowSelected" />
                                                        </ClientSettings>
                                                    </telerik:RadGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <a id="sGrid" href="#sGrid"></a>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <h3 id="divAddHdr" runat="server">
                            Add Patient</h3>
                        <div id="divAdd" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="addtable" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 15%">
                                                First Name:
                                            </td>
                                            <td style="width: 35%">
                                                <telerik:RadTextBox ID="txtAFName" runat="server" BackColor="#FFFFCC">
                                                    <ClientEvents OnBlur="OnBlur" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 15%">
                                                Middle Name:
                                            </td>
                                            <td style="width: 35%">
                                                <telerik:RadTextBox ID="txtMidName" runat="server">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">
                                                Last Name:
                                            </td>
                                            <td style="width: 35%">
                                                <telerik:RadTextBox ID="txtALName" runat="server" BackColor="#FFFFCC">
                                                    <ClientEvents OnBlur="OnBlur" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 15%">
                                                Sex:
                                            </td>
                                            <td style="width: 35%">
                                                <telerik:RadComboBox ID="rcbSex" runat="server">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="Male" Value="Male" Selected="true" />
                                                        <telerik:RadComboBoxItem Text="Female" Value="Female" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 15%">
                                                Date of Birth:
                                            </td>
                                            <td style="width: 35%">
                                                <telerik:RadDatePicker ID="dtpDOB" runat="server" EnableTyping="false">
                                                    <ClientEvents OnDateSelected="OnBlurDateP" />
                                                    <Calendar ID="Calendar1" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        ShowRowHeaders="false" runat="server">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput1" DisplayDateFormat="dd MMM yyyy" DateFormat="dd-MMM-yyyy"
                                                        LabelWidth="0px" runat="server">
                                                        <ClientEvents OnBlur="OnBlur" />
                                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                        <FocusedStyle Resize="None"></FocusedStyle>
                                                        <DisabledStyle Resize="None"></DisabledStyle>
                                                        <InvalidStyle Resize="None"></InvalidStyle>
                                                        <HoveredStyle Resize="None"></HoveredStyle>
                                                        <EnabledStyle Resize="None"></EnabledStyle>
                                                    </DateInput>
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Label ID="lbladdfolder" Text="Folder No:" runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 35%">
                                                <telerik:RadTextBox ID="txtNewFolderNo" runat="server" BackColor="#FFFFCC">
                                                    <ClientEvents OnBlur="OnBlur" />
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadButton ID="btnAdd" runat="server" OnClientClicking="CheckValsFindAdd" Text="Add"
                                                    OnClick="btnAdd_Click">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="dialog-addPatient" title="Patient Added" style="display: none">
                                        <div class="ui-widget">
                                            <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
                                                <p>
                                                    <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em; margin-top: .1em">
                                                    </span><strong>Saved !</strong><br />
                                                    The patient has been successfully saved.
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>
    <telerik:RadAjaxLoadingPanel ID="statsRadLPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    </form>
    <script src="scripts/PASDPBaseScripts.js?reload" type="text/javascript"></script>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {
                if (document.getElementById("<%=hidmodule.ClientID %>").value == 'PASDP') {
                    ChangeColor("White");
                }
                else {
                    ChangeColor("Black");
                }

                var $win = $(window);
                var $box = $("#moreMenu");
                var $menuBut = $("#divMore");
                var $statusWrapper = $(".ext_wrapper");


                $menuBut.click(function (e) {
                    ShowMoreMenu('moreMenu');
                    e.stopPropagation();
                });


                $win.on("click.Bst", function (event) {
                    //check the menu
                    if (
                        $box.has(event.target).length == 0
                        &&
                        !$box.is(event.target)
                      ) {
                        HideMoreMenu('moreMenu');
                    }

                    //also check the extruder
                    if (
                        $statusWrapper.has(event.target).length == 0
                        &&
                        !$statusWrapper.is(event.target)
                      ) {
                        $("#extruderLeft").closeMbExtruder();
                    }
                });

            });

            function ChangeColor(theColor) {
                $(".dr-icon").css("color", theColor);
            }

            function fnprint() {
                document.getElementById('btnprint').style.display = 'none';
                var printContent = document.getElementById("divshowlabs");
                var windowUrl = 'about:blank';
                var uniqueName = new Date();
                var windowName = 'Print' + uniqueName.getTime();
                var printWindow = window.open(windowUrl, windowName, 'scrollbars=1,width=1050,height=800');
                printWindow.document.open();
                printWindow.document.write(printContent.innerHTML);
                printWindow.document.close();
                printWindow.focus();
                printWindow.print();
            }
            function fnprintalert() {
                document.getElementById('btnprintalert').style.display = 'none';
                var printContent = document.getElementById("divlabAlert");
                var windowUrl = 'about:blank';
                var uniqueName = new Date();
                var windowName = 'Print' + uniqueName.getTime();
                var printWindow = window.open(windowUrl, windowName, 'scrollbars=1,width=1050,height=800');
                printWindow.document.open();
                printWindow.document.write(printContent.innerHTML);
                printWindow.document.close();
                printWindow.focus();
                printWindow.print();
            }

            $(PatientHome_Load);
            $(function () {
                //add the "Add Patient" dialog to click
                $("#hdAddPatient").click(function () {
                    $("#dialog-addPatient").dialog({
                        resizable: false,
                        height: 300,
                        appendTo: "#<%=divAdd.ClientID%>",
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                });
                if ($("#hidJustAddPat").val() == "true") {
                    $("#hidJustAddPat").val("");
                    ShowPage(2);
                }
            });
            // Accordion for the find/add window 
            $(document).ready(function () {
                InIEvent();
            });
            function CloseLoading() {
                currentLoadingPanel = $find(GetLPanelID());
                var theInterval = setInterval(function () { currentLoadingPanel.hide("somediv"); clearInterval(theInterval); }, 1000);

            }
            function ShowLoading() {
                currentLoadingPanel = $find(GetLPanelID());
                currentLoadingPanel.show("somediv");
            }
            function SwipeLeft() {
                //ShowLoading();
                $('#middlePane').scrollTo($('#theForm'), 600, {
                    onAfter: function () {
                        ShowLoading();
                    }
                });

                $("#divBackBtn").css("display", "block");
                $("#divMore").css("display", "none");
                $("#extruderLeft").closeMbExtruder();
            }
            function ShowPage(thePage) {
                if ($('#<%=lblStatus.ClientID%>').html() == "Inactive") {
                    switch (thePage) {
                        case 8:
                        case 2:
                            break;
                        default:
                            alert("This Patient is inactive. You can:\nRe-activate the patient\nPrint the Registration Information\nView Reports\nOr view an existing form");
                            return false;
                            break;
                    }
                }

                SwipeLeft();

                switch (thePage) {
                    case 1:
                        $('#<%=btnGoToVisit.ClientID%>').click();
                        break;
                    case 2:
                        $('#<%=btnGoToRegistration.ClientID%>').click();
                        break;
                    case 3:
                        document.getElementById('hddSavealert').value = 1;
                        $('#<%=btnGotoLab.ClientID%>').click();
                        break;
                    case 4:
                        $('#<%=btnGoToHist.ClientID%>').click();
                        break;
                    case 5:
                        $('#<%=btnGoToImmun.ClientID%>').click();
                        break;
                    case 6:
                        document.getElementById('hddSavealert').value = 1;
                        $('#<%=btnGoToPharm.ClientID%>').click();
                        break;
                    case 7:
                        $('#<%=btnGoToNon.ClientID%>').click();
                        break;
                    case 8:
                        document.getElementById('hddSavealert').value = 1;
                        $('#<%=btnGoToRep.ClientID%>').click();
                        break;
                    case 9:
                        $('#<%=btnGoToCare.ClientID%>').click();
                        break;
                    case 12:
                        $('#<%=btnExpress.ClientID %>').click();
                        break;
                    case 11:
                        $('#<%=btnAdultIE.ClientID %>').click();
                        break;
                    default:

                        break;

                }

                return false;
            }
            function HospitalYN(sender, eventArgs) {
                //var item = eventArgs.get_item();
                ShowHide("hideHospitalYN");
            }
            function GetLPanelID() {
                return "<%= statsRadLPanel.ClientID %>";
            }
            function BackBtnClick() {

                if (document.getElementById('hddSavealert').value == '1') {
                    $("#FormMode").val("Unload");
                    BackWithoutDialog();
                }
                else {
                    if (window.confirm('Are you sure you want to leave this form without saving?')) {
                        $("#FormMode").val("Unload");
                        
                        //$("#divBackBtn").css("display", "none");
                        //$("#divSave").css("display", "none");
                        document.getElementById('hddSavealert').value = "";
                        //$('#<%=btnClearViewState.ClientID%>').click();
                        BackWithoutDialog();

                    }
                }
            }

            function BackWithoutDialog() {
                //$('#middlePane').scrollTo($('#phMenu'), 800);
                ShowLoading();
                //BackToMain();
                $("#divBackBtn").css("display", "none");
                $("#divSave").css("display", "none");
                document.getElementById('hddSavealert').value = "";
                $("#<%=refreshViewExisting.ClientID%>").val("true");
                $('#<%=btnClearViewState.ClientID%>').click();

            }

            //Go to PatientHome
            function GoToPatientHome(PatientId) {
                $('body').fadeOut(500, function () {
                    document.location.href = "frmTouchPatientHome.aspx?PatientId=" + PatientId;
                });
            }
            function CheckValsFindAdd(sender, args) {
                var ReqVals = new Array();
                ReqVals[0] = "<%= txtAFName.ClientID %>|First Name";
                ReqVals[1] = "<%= txtALName.ClientID %>|Last Name";
                ReqVals[2] = "<%= txtNewFolderNo.ClientID %>|Folder number";
                ReqVals[3] = "<%= rcbSex.ClientID %>|Patient sex";
                ReqVals[4] = "<%= dtpDOB.ClientID %>|Date of birth";

                var theNames = new Array();
                var ReqIsFilled = true;
                for (index = 0; index < ReqVals.length; ++index) {

                    var arr = ReqVals[index].split("|");

                    var theFirstControl = null;
                    if (index != 3) {
                        if ($('#' + arr[0]).val() == "") {
                            theNames[index] = arr[1];

                            if (theFirstControl == null) theFirstControl = arr[0];
                            ReqIsFilled = false;
                        }
                    } else {
                        if ($('#' + arr[0]).val() == "Select") {
                            theNames[index] = arr[1];

                            if (theFirstControl == null) theFirstControl = arr[0];
                            ReqIsFilled = false;
                        }
                    }
                }

                if (ReqIsFilled) {
                    var theAnswer = window.confirm("Are you sure you want to save this form?");
                    if (!theAnswer) {
                        args.set_cancel(true);
                    }
                }
                else {
                    gotToTabVal(theFirstControl, theNames, args);
                }
            }
            function gotToTabVal(thecontrol, thenames, args) {
                //$("#tabs").tabs("option", "active", 0);
                var theMessage = "You have not given a value for the required field(s): ";
                for (i = 0; i < thenames.length; ++i) {
                    if (typeof thenames[i] != "undefined")
                        theMessage = theMessage + "\n" + thenames[i];
                }

                alert(theMessage);
                $('#' + thecontrol).focus();
                args.set_cancel(true);
            }

            function RowSelected(sender, args) {
                $("#<%=Ptn_PkVal.ClientID %>").val(args.getDataKeyValue("Ptn_Pk"));
                $("#<%=locationIDVal.ClientID %>").val(args.getDataKeyValue("locationID"));
                $("#<%=Ptn_DOB.ClientID %>").val(args.getDataKeyValue("dob"));
                $('#<%=btnGoToPat.ClientID %>').click();

            }

            function PrintTouchForm(FormName, FormID) {
                var rdpane = $find(FormID + "_rwPrint");
                var pane = rdpane.getPaneById(FormID + "_rdPane");
                var radComboBoxBaseStylesheet = '<%= Page.ClientScript.GetWebResourceUrl(typeof(RadComboBox), "Telerik.Web.UI.Skins.ComboBox.css") %>';
                var radComboBoxDefaultSkinStylesheet = '<%= Page.ClientScript.GetWebResourceUrl(typeof(RadComboBox), "Telerik.Web.UI.Skins.Default.ComboBox.Default.css") %>';
                if (!pane) return;
                var arrExtStylsheetFiles = getTelerikCssLinks();

                //pane.Print(arrExtStylsheetFiles);
                PrintFormExec(arrExtStylsheetFiles, FormName);
            }

            function PrintFormExec(arrExtStylsheetFiles, FormName) {

                var previewWnd = window.open('about:blank', '', '', false);

                var styleStr = "<html><head><title>" + FormName + "</title>";

                for (var i = 0; i < arrExtStylsheetFiles.length; i++) {
                    styleStr += "<link href = '" + arrExtStylsheetFiles[i].toString() + "' rel='stylesheet' type='text/css'></link>";
                }

                styleStr += "</head>";
                var theContent = $('#tabs')[0].outerHTML;
                var s = theContent.indexOf("<ul");
                var e = theContent.indexOf("</ul>");
                var theSlice = theContent.slice(s, e);
                
                theContent = theContent.replace(theSlice, "");
                theContent = theContent.replace(/display: none;/gi, "display:inline");
                
                
                theContent = theContent.replace(/height: 380px;/gi, "");
                theContent = theContent.replace(/Open the calendar popup./gi, "");
                theContent = theContent.replace(/help guide/gi, "");
                theContent = theContent.replace(/select/gi, "");
                theContent = theContent.replace(/WHO Clinical Staging Guide/gi, "");


                var htmlcontent = styleStr + "<body>" + theContent + "</body></html>";
                htmlcontent = htmlcontent.replace("Open the calendar popup.", "");
                htmlcontent = htmlcontent.replace("height: 380px;", "");

                previewWnd.document.open();
                console.log(htmlcontent);
                previewWnd.document.write(htmlcontent);
                previewWnd.document.close();
                previewWnd.print();
                previewWnd.close();
            }

            function getTelerikCssLinks() {
                var result = new Array();

                var links = document.getElementsByTagName("LINK"); //get all link elements on the page

                for (var i = 0; i < links.length; i++) {
                    if (links[i].getAttribute("class") == "Telerik_stylesheet")//check if the link element is a Telerik Stylesheet
                        result.push(links[i].getAttribute("href", 2)); //add link href attribute to the result
                }

                return result;
            }

            $('#moreMenu').mouseleave(function () {
                HideMoreMenu('moreMenu');
            });
            
            function showbar(s, e) {
                ShowMinLoading('rwFindAdd_C_divFind');
            }

            function DivShowLabsUpdate() {
                //ShowGraphs('divshowlabs');
                var btnUpdateLabsButton = document.getElementById('btnInvisibleUpdateLabHistory');
                btnUpdateLabsButton.click();
            }

            console.error = function () {
                alert('An error has occured and has been logged and handled./n The page will now reload');
                window.location.reload()
            }

            
        </script>
    </telerik:RadCodeBlock>
</body>
</html>
