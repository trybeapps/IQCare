<%@ Page Language="C#" AutoEventWireup="True" Inherits="Touch.tester" Codebehind="frmTouchPatientHome.iframe.aspx.cs" %>

<!DOCTYPE html>

<html>

<head id="Head1" runat="server">
    <title runat="server" id="AppTitle">Patient Home Screen</title>
    <!-- Jquery standard files -->
    <script type="text/javascript" src="Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="Styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="Stylesheet" href="Styles/custom-theme/jquery-ui-1.10.3.custom.min.css" type="text/css" />
    <!-- PASDP Style sheet -->
    <link rel="Stylesheet" href="Styles/PASDP.css?reload" type="text/css" media="all" />
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
    <link href="Styles/jquery.jscrollpane.lozenge.css?reload" rel="stylesheet" type="text/css" />

    <link href="Scripts/keyb/keyboard.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="rsm" runat="server"></telerik:RadScriptManager>

    <!--hidden buttons -->
    <input type="button" id="hdAddPatient" style="visibility:collapse;" />

    <!--menu buttons -->
    <asp:UpdatePanel ID="updtHdButtons" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnGoToVisit" runat="server" CssClass="hiddenButtons" onclick="btnGoToVisit_Click"/>
            <asp:Button ID="btnGoToRegistration" runat="server" CssClass="hiddenButtons"
                onclick="btnGoToRegistration_Click" />
            <asp:Button ID="btnGotoLab" runat="server" CssClass="hiddenButtons" onclick="btnGotoLab_Click"/>
            <asp:Button ID="btnGoToHist" runat="server" CssClass="hiddenButtons" onclick="btnGoToHist_Click"/>
            <asp:Button ID="btnGoToImmun" runat="server" CssClass="hiddenButtons" onclick="btnGoToImmun_Click"/>
            <asp:Button ID="btnGoToPharm" runat="server" CssClass="hiddenButtons" onclick="btnGoToPharm_Click"/>
            <asp:Button ID="btnGoToNon" runat="server" CssClass="hiddenButtons" onclick="btnGoToNon_Click"/>
            <asp:Button ID="btnGoToRep" runat="server" CssClass="hiddenButtons" onclick="btnGoToRep_Click"/>
            <asp:Button ID="btnGoToCare" runat="server" CssClass="hiddenButtons" onclick="btnGoToCare_Click"/>
            <asp:Button ID="btnClearViewState" runat="server" CssClass="hiddenButtons" onclick="btnClearViewState_Click"/>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div>
    
        <div id="wrapper">
            <div id="TopPane">
            <telerik:RadAjaxManager ID="RadAjaxManager1" EnableAJAX="true" runat="server">
                <AjaxSettings>
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
            
            <telerik:RadScriptBlock runat="server" ID="rscb1">
            <div style="float:left;padding: 10px 0 0 100px;">
                <div class="btnNav" id="btnHome" onclick="GoToFacility()">
                    <img src="images/home.png" alt="Home" /><br />
                    <span style="color:White">Home</span>
                </div>
                <div class="btnNav"  onclick="OpenFindAdd('<%=rwFindAdd.ClientID%>')">
                    <img src="images/findadd.png" alt="Find / Add" /><br />
                    <span style="color:White">Find/Add</span>
                </div>
                <div class="btnNav" onclick="BackBtnClick()" style="display:none;" id="divBack">
                    <img src="images/btnSave.jpg" alt="Home" /><br />
                    <span style="color:White">Save</span>
                </div>
                <div class="btnNav" style="display:block;" id="divMore" onclick="ShowMoreMenu('moreMenu')">
                    <img src="images/more.jpg" alt="More" /><br />
                    <span style="color:White">More</span>
                </div>
                <div class="btnNav" id="divFacilityName" style="width:400px;text-align:center;">
                    <h3 style="color:White"><asp:Label ID="lblFacilityName" runat="server"></asp:Label></h3>
                </div>
            </div>
            </telerik:RadScriptBlock>
            <div class="btnNav">
                <div class="more-menu"  style="display:none;" id="moreMenu">
                    <div>
                        <ul>
                            <li><a href="#" onclick="OpenModalFromClient('rwViewExistingForms');ShowMoreMenu('moreMenu');">View existing forms</a></li>
                            <li><a href="#">Administration</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
            
            <div id="detailSection">
                <div id="middlePane" style="position:relative;width:1050px;height:509px; float:left;z-index:99;overflow:hidden">
                    <div id="phMenu" style="position:relative;float:left;margin-left:-250px;">&nbsp;</div>
                    <table>
                    <tr>
                    <td>
                
                            <div id="theContent">
                                <img src="images/menuButtons/VisitSA.png" class="MenuButs" alt="Visit" onclick="ShowPage(1)"/>
                                <img src="images/menuButtons/RegistrationSA.png" class="MenuButs" alt="Registration" onclick="ShowPage(2)" />
                                <img src="images/menuButtons/PharmacySA.png" class="MenuButs" alt="Pharmacy" onclick="ShowPage(6)"/>
                                <img src="images/menuButtons/ImmunisationSA.png" class="MenuButs" alt="Immunisation"  onclick="ShowPage(5)"/>
                                <img src="images/menuButtons/ECapeCOA.png" style="cursor:default" class="MenuButs" alt="Logo"/>
                                <img src="images/menuButtons/LaboratorySA.png" class="MenuButs" alt="Laboratory" onclick="ShowPage(3)" />
                                <img src="images/menuButtons/NonSA.png" class="MenuButs" alt="Non Visit" onclick="ShowPage(7)"/>
                                <img src="images/menuButtons/ReportsSA.png" class="MenuButs" alt="Reports" onclick="ShowPage(8)"/>
                                <img src="images/menuButtons/CareEndedSA.png" class="MenuButs" alt="Care Ended" onclick="ShowPage(9)"/>

                            </div>
                    </td>
                    <td>
                                

                            <div id="theForm" runat="server" style="width:1200px; overflow:hidden; height: 509px; float:left; padding: 30px 0 0 50px;">
                                <div id="somediv" style="margin: 0 0 0 50px;padding: 0 0 0 0;width:861px;height:440px;overflow:hidden;background-color:black">
                                    <div class="updtForms">
                                    <asp:UpdatePanel ID="updtForms" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <iframe id="theFrame" runat="server" scrolling="no" style="overflow:hidden;border: 0px none transparent; padding: 0px;" width="861" height="440"></iframe>
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

    <div id="extruderLeft" class="a {title:'<% Response.Write(lblName.Text); %>'}" style="font-size:12px;z-index:100">
        <div id="LeftPane" class="LeftPane">
            <img alt="PASDP Logo" src="images/menuButtons/ELHospLogo.png" 
                    style=" height: 44px; width: 187px; margin-top:10px;" />
                    <br />
            <table>
                <tr>
                    <td colspan="2" style="color:rgb(12, 25, 141);font-weight:bold; font-size:18px;"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Folder #:</td>
                    <td><asp:Label id="lblFolderNo" Text="No D9 Available" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Sex:</td>
                    <td><asp:Label ID="lblSex" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>DOB:</td> 
                    <td><asp:Label ID="lblDOB" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Age:</td> <td><asp:Label ID="lblAge" runat="server"></asp:Label><!-- 0 yrs, 0 mths--></td>
                </tr>
                <tr>
                    <td>District:</td>
                    <td><asp:Label ID="lblDistrict" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Phone:</td> 
                    <td><asp:Label ID="lblPhone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Status:</td> 
                    <td style="color:Green"><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                 </tr>
            </table>
                    <img src="images\divider.png" alt="divider" />
                    <br />
                    <table>
                        <tr>
                            <td style="color:rgb(251, 182, 19)">Rx</td>
                            <td>|</td>
                            <td>Last Regimen</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td style="color:rgb(251, 182, 19)">AZT/3TC/NVP</td>
                        </tr>
                        <tr>
                            <td style="padding-top:7px; padding-left:6px;"><img src="images/upArrow.jpg" alt="↑" /></td>
                            <td>|</td>
                            <td>203 CD4</td>
                        </tr>
                        <tr>
                            <td style="padding-top:7px; padding-left:10px;color:rgb(251, 182, 19)">!</td>
                            <td>|</td>
                            <td>Next CD4 03-Aug-2013</td>
                        </tr>
                    </table>
                 <img src="images\divider.png" alt="divider" />
                 <br />
                 <span class="quickbarLink">CD4-Viral Load Chart</span><br />
                 <span class="quickbarLink">Weight-BMI Chart</span><br />
                 <span class="quickbarLink">Growth Chart</span><br />
                 <span class="quickbarLink">Latest Labs</span><br />
                 <span class="quickbarLink">ARV History</span><br />

        </div>
    </div>
    <div style="position:absolute;float:left;top:40px;left:700px;z-index:150;">  
        <a class="dr-icon dr-icon-user" href="#" style="color:White;text-decoration:none;margin-right:10px;"><asp:Label ID="lblUserName" runat="server"></asp:Label></a>
		<a class="dr-icon dr-icon-switch"  href="../frmLogOff.aspx?Touch=true"  style="color:White;text-decoration:none;">Logout</a>
    </div>

    <telerik:radwindow runat="server" id="rwFindAdd" visibleonpageload="false" Title="Find/Add Patient"
     Modal="true" Skin="BlackMetroTouch" Width="880px" Height="453px" Behaviors="Move,Close" >
        <ContentTemplate>
            <asp:UpdatePanel ID="updtFindPatient" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="jAccordion" style="width:850px">
                        <h3>Find Patient <span style="font-size:12px"> -- Enter search criteria (full or partial text)</span> </h3>
                        <div id="divFind" runat="server">
                            <table width="800px" id="findtable" cellpadding="10px">
                                <tr>
                                    <td>
                                        Folder No:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtFolderNo" runat="server" ></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        Date of Birth:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="dtpDOBs" runat="server" >
                                            <Calendar ID="Calendar2" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                                 runat="server"></Calendar>
                                            <DateInput ID="DateInput2" DisplayDateFormat="dd MMM yyyy" DateFormat="dd-MMM-yyyy" LabelWidth="0px" runat="server">
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
                                    <td>
                                        First Name:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtFName" runat="server" ></telerik:RadTextBox>
                                    </td>
                                    <td>
                                        Last Name:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtLName" runat="server" ></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1">
                                        Identification Number
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadTextBox ID="txtIdNo" runat="server" Width="538px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1">
                                        Service
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadComboBox ID="rcbService" runat="server" Width="538px"></telerik:RadComboBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <a id="sGrid" href="#sGrid"></a>
                                        <telerik:RadButton ID="btnSearch" runat="server" Text="Find"  OnClick="btnSearch_Click"></telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <telerik:RadGrid ID="rgResults" runat="server"  Visible="false"
                                        AllowPaging="true" OnSelectedIndexChanged="rgResults_SelectedCellChanged" OnNeedDataSource="rgResults_NeedDataSource"
                                            OnColumnCreated="rgResults_ColumnCreated"  >
                                            <MasterTableView PageSize="5" DataKeyNames="Ptn_Pk" Font-Size="8">
                                                <Columns>
                                                    <telerik:GridButtonColumn Text="Select" CommandName="Select">
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <h3>Add Patient</h3>
                        <div id="divAdd" runat="server">
                            <table  width="800px" id="addtable" cellpadding="10px" class="Section" >
                                <tr>
                                    <td style="width:15%">
                                        Folder No:
                                    </td>
                                    <td colspan="3" style="width:85%">
                                        <telerik:RadTextBox ID="txtNewFolderNo" runat="server" ></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%">
                                        First Name:
                                    </td>
                                    <td style="width:35%">
                                        <telerik:RadTextBox ID="RadTextBox3" runat="server" ></telerik:RadTextBox>
                                    </td>
                                    <td style="width:15%">
                                        Middle Name:
                                    </td>
                                    <td style="width:35%">
                                        <telerik:RadTextBox ID="RadTextBox4" runat="server" ></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%">
                                        Last Name:
                                    </td>
                                    <td colspan="3" style="width:85%">
                                        <telerik:RadTextBox ID="RadTextBox5" runat="server" ></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%">
                                        Sex:
                                    </td>
                                    <td style="width:35%">
                                        <telerik:RadComboBox ID="rcbSex" runat="server" >
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Male" Value="Male" Selected="true" />
                                                <telerik:RadComboBoxItem Text="Female" Value="Female" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td style="width:15%">
                                        Date of Birth:
                                    </td>
                                    <td style="width:35%">
                                        <telerik:RadDatePicker ID="dtpDOB" runat="server" >
                                            <Calendar ID="Calendar1" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                                 runat="server"></Calendar>
                                            <DateInput ID="DateInput1" DisplayDateFormat="dd MMM yyyy" DateFormat="dd-MMM-yyyy" LabelWidth="0px" runat="server">
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
                                    <td colspan="4">
                                        <telerik:RadButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" ></telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                            <div id="dialog-addPatient" title="Patient Added" style="display:none">
                                <div class="ui-widget">
	                                <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
		                                <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em; margin-top: .1em"></span>
		                                <strong>Saved !</strong><br />
                                            The patient has been successfully saved.
                                        </p>
	                                </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:radwindow>

    <telerik:RadAjaxLoadingPanel ID="statsRadLPanel" runat="server"></telerik:RadAjaxLoadingPanel>

    

    </form>
<script src="scripts/PASDPBaseScripts.js" type="text/javascript"></script>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
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
        });
        function CloseLoading() {
            //for (var i = 0; i < 10000; i++) { }
            currentLoadingPanel = $find(GetLPanelID());
            var theInterval = setInterval(function () { currentLoadingPanel.hide("somediv"); clearInterval(theInterval); }, 2000);
            
        }
        function ShowLoading() {
            currentLoadingPanel = $find(GetLPanelID());
            currentLoadingPanel.show("somediv");
        }
        function ShowPage(thePage) {
            //ShowLoading();
            $('#middlePane').scrollTo($('#theForm'), 600, {
                onAfter: function () {
                    ShowLoading();
                }
            });
            
            $("#divBack").css("display", "block");
            $("#divMore").css("display", "none");
            $("#extruderLeft").closeMbExtruder();
            switch (thePage) {
                case 1:
                    $('#<%=btnGoToVisit.ClientID%>').click();
                    break;
                case 2:
                    $('#<%=btnGoToRegistration.ClientID%>').click();
                    break;
                case 3:
                    $('#<%=btnGotoLab.ClientID%>').click();
                    break;
                case 4:
                    $('#<%=btnGoToHist.ClientID%>').click();
                    break;
                case 5:
                    $('#<%=btnGoToImmun.ClientID%>').click();
                    break;
                case 6:
                    $('#<%=btnGoToPharm.ClientID%>').click();
                    break;
                case 7:
                    $('#<%=btnGoToNon.ClientID%>').click();
                    break;
                case 8:
                    $('#<%=btnGoToRep.ClientID%>').click();
                    break;
                case 9:
                    $('#<%=btnGoToCare.ClientID%>').click();
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
            BackToMain();
            $('#<%=btnClearViewState.ClientID%>').click();

        }
        //Go to PatientHome
        function GoToPatientHome(PatientId) {
            $('body').fadeOut(500, function () {
                document.location.href = "frmTouchPatientHome.aspx?PatientId=" + PatientId;
            });
        }
    </script>
</telerik:RadCodeBlock>
</body>
</html>
