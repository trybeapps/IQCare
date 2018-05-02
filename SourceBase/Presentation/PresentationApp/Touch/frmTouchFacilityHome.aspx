<%@ Page Language="C#" AutoEventWireup="True" Inherits="Touch.frmTouchFacilityHome"
    CodeBehind="frmTouchFacilityHome.aspx.cs" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="pageHead" runat="server">
    <title runat="server" id="AppTitle"></title>
    <!-- Jquery standard files -->
    <script type="text/javascript" src="scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="Stylesheet" href="styles/custom-theme/jquery-ui-1.10.3.custom.min.css?reload"
        type="text/css" />
    <!-- Pattern lock Files -->
    <link rel="stylesheet" type="text/css" href="styles/patternlock.css" />
    <script type="text/javascript" src="scripts/patternlock.js"></script>
    <!-- PASDP Style sheet -->
    <link id="lnkfacilityhome" rel="Stylesheet" type="text/css" runat="server" media="all" />
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
    <script type="text/javascript">
        $(function () {
            var element = document.getElementById('patLock');
            patternlock.generate(element);
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hidtab" Value="0" runat="server" />
    <asp:HiddenField ID="hidmodule" runat="server" />
    <asp:HiddenField ID="Ptn_PkVal" runat="server" />
    <asp:HiddenField ID="locationIDVal" runat="server" />
    <telerik:RadAjaxManager ID="ram1" EnableAJAX="true" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbFacility">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="statsDiv" LoadingPanelID="statsRadLPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divFind" LoadingPanelID="statsRadLPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAdd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divAdd" LoadingPanelID="statsRadLPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadScriptManager runat="server" ID="ScriptManager1">
    </telerik:RadScriptManager>
    <!--redirect buttons *** DO NOT change to display:none as this will render the layout incorrectly-->
    <input type="button" id="hdGoToPatient" style="visibility: collapse;" />
    <input type="button" id="hdAddPatient" style="visibility: collapse;" />
    <!-- END -->
    <!-- hidden buttons -->
    <asp:UpdatePanel ID="updtHdButtons" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnGoToPat" runat="server" CssClass="hiddenButtons" OnClick="btnGoToPat_Click" />
            <asp:Button ID="btnSetPattern" runat="server" CssClass="hiddenButtons" OnClick="btnSetPattern_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END -->
    <div>
        <div id="wrapper">
            <div id="TopPane">
                <div style="float: left; padding: 10px 0 0 100px;">
                    <div class="btnNav" onclick="OpenFindAdd()">
                        <asp:Image ID="imgfindadd" AlternateText="Home" runat="server" /><br />
                        <span class="lablename">Find/Add</span>
                    </div>
                    <div class="btnNav" id="divFacilityName" style="width: 500px; text-align: center;">
                        <h3 class="lablename">
                            <asp:Label ID="lblFacilityName" runat="server"></asp:Label></h3>
                    </div>
                    
                </div>
            </div>
            <telerik:RadAjaxPanel ID="rdpStats" runat="server" HorizontalAlign="NotSet">
                <div id="detailSection">
                    <div id="middlePane" style="position: absolute; width: 900px; height: 520px; float: left;
                        z-index: 99; overflow: hidden">
                        <div id="theContent">
                            <div style="background-color: White; height: 280px; padding: 10px; width: 750px;
                                margin-top: 70px" runat="server" id="statsDiv">
                                <div class="theContentInner">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 70px;">
                                                <h3>
                                                    Facility:</h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadComboBox ID="rcbFacility" Width="250px" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rcbFacility_SelectedIndexChanged1">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="theContentInner">
                                    <table style="width: 100%" cellpadding="15">
                                        <tr>
                                            <td style="padding-top: -30px">
                                                <h3>
                                                    Statistics</h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 75%">
                                                Total Patients:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTot" runat="server" Text="3724"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Total Active Patients:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTotAct" runat="server" Text="2561"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <a href="#" onclick="OpenLostToFollow()" class="Emphasis" style="text-decoration: none">
                                                    Lost To Follow Up List:</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <a href="#" onclick="OpenDueCareEnded()" class="Emphasis" style="text-decoration: none">
                                                    Due For Care Ended List:</a>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <telerik:RadWindow runat="server" ID="rwLostToFollow" VisibleOnPageLoad="false" Title="Lost To Follow Up List"
                    Modal="true" Width="880px" Height="350px" Behaviors="Move,Close">
                    <ContentTemplate>
                        <div style="float: right; position: relative; margin: 5px 5px 5px 0px;">
                            <telerik:RadButton ID="btnPrint" runat="server" Text="Print" AutoPostBack="false"
                                OnClientClicking="PrintLTFRadGrid">
                            </telerik:RadButton>
                        </div>
                        <telerik:RadGrid ID="rgdLostTF" runat="server" CssClass="printGrid">
                            <MasterTableView Font-Size="8">
                            </MasterTableView>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadWindow>
                <telerik:RadWindow runat="server" ID="rwDueCareEnded" VisibleOnPageLoad="false" Title="Due for Care Ended List"
                    Modal="true" Width="880px" Height="450px" Behaviors="Move,Close">
                    <ContentTemplate>
                        <div style="float: right; position: relative; margin: 5px 5px 5px 0px;">
                            <telerik:RadButton ID="RadButton1" runat="server" Text="Print" AutoPostBack="false"
                                OnClientClicking="PrintDCERadGrid">
                            </telerik:RadButton>
                        </div>
                        <telerik:RadGrid ID="rgdDueForCare" runat="server">
                            <MasterTableView Font-Size="8">
                            </MasterTableView>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadWindow>
            </telerik:RadAjaxPanel>
            <div id="footer">
            </div>
        </div>
    </div>
    <div id="divUserButtons" style="position: absolute; float: left; top: 40px; left: 650px;
        z-index: 150;">
        <div class="dr-icon-pattern" onclick="patternClick(this)">
            <img src="images/pattern.jpg" />
            <br />
            <span style="color: White; text-decoration: none; margin-right: 10px;">
                <asp:Label ID="Label1" runat="server">Set Pattern</asp:Label></span>
        </div>
        <span class="dr-icon dr-icon-user" style="color: White; text-decoration: none; margin-right: 10px;">
            <asp:Label ID="lblUserName" runat="server"></asp:Label></span> <a class="dr-icon dr-icon-switch"
                href="../frmLogOff.aspx?Touch=true" style="color: White; text-decoration: none;">
                Logout</a>
    </div>
    <telerik:RadWindow runat="server" ID="rwPattern" VisibleOnPageLoad="false" Title="Set Pattern"
        Modal="true" Width="880px" Height="470px" Behaviors="Move,Close" Skin="BlackMetroTouch">
        <ContentTemplate>
            <asp:UpdatePanel ID="updtPattern" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div style="text-align: center; width: 100%; color: White;">
                        Slide your finger accross the dots to select a pattern to save as your unlock code.
                        (A dot can be selected more than once) This code can then be used to unlock the
                        system if your session expires.
                    </div>
                    <br />
                    <div>
                        <input id="patLock" type="password" name="password" class="patternlock" runat="server"
                            clientidmode="Static" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>
    <telerik:RadWindow runat="server" ID="rwFindAdd" VisibleOnPageLoad="True" Title="Find/Add Patient"
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
                                                <asp:Label ID="lblname" runat="server"></asp:Label>
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
                                                <telerik:RadButton ID="btnSearch" runat="server" Text="Find" OnClick="btnSearch_Click">
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
                                                <telerik:RadButton ID="btnAdd" runat="server" OnClientClicking="CheckVals" Text="Add"
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
    <!-- PASDP jscript -->
    <script type="text/javascript" src="scripts/PASDPBaseScripts.js"></script>
    <telerik:RadCodeBlock ID="rcblkPageScript" runat="server">
        <script type="text/javascript">

            $(document).ready(function () {
                if (document.getElementById("<%=hidmodule.ClientID %>").value == 'PASDP') {
                    ChangeColor("White");
                }
                else {
                    ChangeColor("Black");
                }


            });

            function ChangeColor(theColor) {
                $(".dr-icon").css("color", theColor);
            }
            // to fade in on page load
            $(document).ready(function () {
                $("body").css("display", "none");
                $("body").fadeIn(500);
                // to fade out before redirect
                $("#hdGoToPatient").click(function (e) {
                    redirect = $(this).attr('href');
                    e.preventDefault();
                    $('body').fadeOut(500, function () {
                        document.location.href = "PatientHome.aspx";
                    });
                });

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
            });

            //Open the FindAddRadwindow
            function OpenFindAdd() {
                var oWnd = $find("<%=rwFindAdd.ClientID%>");
                oWnd.show();
            }

            //Open the LostToFollowRadwindow
            function OpenLostToFollow() {
                var oWnd = $find("<%=rwLostToFollow.ClientID%>");
                oWnd.show();
            }

            //Open the DueCareEndedRadwindow
            function OpenDueCareEnded() {
                var oWnd = $find("<%=rwDueCareEnded.ClientID%>");
                oWnd.show();
            }

            // Accordion for the find/add window 
            $(document).ready(function () {
                InIEvent();
            });
            function InIEvent() {

                if ($("#dialog-addPatient").is(":visible")) {

                    $("#jAccordion").accordion({
                        heightStyle: "content",
                        active: 1
                    });

                } else {

                    $("#jAccordion").accordion({
                        heightStyle: "content"
                    });

                }
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InIEvent);

            //Print for grids
            function PrintLTFRadGrid() {
                var previewWnd = window.open('about:blank', '', '', false);
                var sh = '<%= ClientScript.GetWebResourceUrl(rgdLostTF.GetType(),String.Format("Telerik.Web.UI.Skins.{0}.Grid.{0}.css",rgdLostTF.Skin)) %>';
                var styleStr = "<html><head><title>Lost to Follow Up</title><link href = '" + sh + "' rel='stylesheet' type='text/css'></link></head>";
                var htmlcontent = styleStr + "<body>" + $find('<%= rgdLostTF.ClientID %>').get_element().outerHTML + "</body></html>";
                previewWnd.document.open();
                previewWnd.document.write(htmlcontent);
                previewWnd.document.close();

                var millisecondsToWait = 500;
                setTimeout(function () {
                    previewWnd.print();
                    previewWnd.close();
                }, millisecondsToWait);

            }

            function PrintDCERadGrid() {
                var previewWnd = window.open('about:blank', '', '', false);
                var sh = '<%= ClientScript.GetWebResourceUrl(rgdDueForCare.GetType(),String.Format("Telerik.Web.UI.Skins.{0}.Grid.{0}.css",rgdDueForCare.Skin)) %>';
                var styleStr = "<html><head><title>Due for Care Ended</title><link href = '" + sh + "' rel='stylesheet' type='text/css'></link></head>";
                var htmlcontent = styleStr + "<body>" + $find('<%= rgdDueForCare.ClientID %>').get_element().outerHTML + "</body></html>";
                previewWnd.document.open();
                previewWnd.document.write(htmlcontent);
                previewWnd.document.close();
                var millisecondsToWait = 500;
                setTimeout(function () {
                    previewWnd.print();
                    previewWnd.close();
                }, millisecondsToWait);

//                previewWnd.print();
//                previewWnd.close();
            }

            //Go to PatientHome
            function GoToPatientHome(PatientId) {
                $('body').fadeOut(500, function () {
                    document.location.href = "frmTouchPatientHome.aspx?PatientId=" + PatientId;
                });
            }

            function CheckVals(sender, args) {
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
                $('#<%=btnGoToPat.ClientID %>').click();

            }

            function patternClick(sender, args) {
                var oWnd = $find('rwPattern');
                oWnd.show();
            }

            function closePatternWindow() {
                var oWnd = $find('rwPattern');
                resetButtons();
                oWnd.close();
            }


        </script>
    </telerik:RadCodeBlock>
</body>
</html>
