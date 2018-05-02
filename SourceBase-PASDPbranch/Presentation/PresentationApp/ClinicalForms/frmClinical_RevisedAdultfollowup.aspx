<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true"
    CodeBehind="frmClinical_RevisedAdultfollowup.aspx.cs" Inherits="PresentationApp.ClinicalForms.frmClinical_RevisedAdultfollowup" %>

<%@ Register TagPrefix="UcVitalSign" TagName="Uc1" Src="~/ClinicalForms/UserControl/UserControlKNH_VitalSigns.ascx" %>
<%@ Register TagPrefix="UcPastMedicalHistory" TagName="Uc2" Src="~/ClinicalForms/UserControl/UserControlKNH_PastMedicalHistory.ascx" %>
<%@ Register TagPrefix="UcWHOStaging" TagName="Uc3" Src="~/ClinicalForms/UserControl/UserControlKNH_WHOStaging.ascx" %>
<%@ Register TagPrefix="UcPhysExam" TagName="Uc4" Src="~/ClinicalForms/UserControl/UserControlKNH_PhysicalExamination.ascx" %>
<%@ Register TagPrefix="UcAllergies" TagName="Uc5" Src="~/ClinicalForms/UserControl/UserControlKNH_DrugAllergies.ascx" %>
<%@ Register TagPrefix="UcPresComp" TagName="Uc6" Src="~/ClinicalForms/UserControl/UserControlKNHPresentingComplaints.ascx" %>
<%@ Register TagPrefix="UcTBScreen" TagName="Uc7" Src="~/ClinicalForms/UserControl/UserControlKNH_TBScreening.ascx" %>
<%@ Register TagPrefix="UcPharmacy" TagName="Uc8" Src="~/ClinicalForms/UserControl/UserControlKNH_Pharmacy.ascx" %>
<%@ Register TagPrefix="UcPWP" TagName="Uc9" Src="~/ClinicalForms/UserControl/UserControlKNH_PwP.ascx" %>
<%@ Register Src="UserControl/UserControlKNH_Extruder.ascx" TagName="UserControlKNH_Extruder"
    TagPrefix="uc10" %>
<%@ Register Src="~/ClinicalForms/UserControl/UserControlKNH_LabEvaluation.ascx"
    TagName="Uclabeval" TagPrefix="uc11" %>
<%@ Register Src="~/ClinicalForms/UserControl/UserControlKNH_Signature.ascx" TagName="UserControlKNH_Signature"
    TagPrefix="uc12" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
    <link href="UserControl/mbExtruder.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Touch/Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="../Touch/Styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>
    <script language="javascript" type="text/javascript" src="../incl/weeklycalendar.js"></script>
    <script language="javascript" type="text/javascript" src="../incl/dateformat.js"></script>
    <script language="javascript" type="text/javascript" src="../incl/jsDate.js"></script>
    <script language="javascript" type="text/javascript">        buildWeeklyCalendar(0);</script>
    <script type="text/javascript" language="javascript">

        $(function () {

            $("#tabs").tabs();

        });
        function ShowHide(theDiv, YN) {

            $(document).ready(function () {

                if (YN == "show") {
                    $("#" + theDiv).slideDown();

                }
                if (YN == "hide") {
                    $("#" + theDiv).slideUp();

                }

            });

        }
        function ShowMore(sender, eventArgs) {
            var substr = eventArgs._commandArgument.toString().split('|')
            ShowHide(substr[0], substr[1]);
        }

        function rblSelectedValue(rbl, divID) {
            var selectedvalue = $("#" + rbl.id + " input:radio:checked").val();
            var YN = "";
            if (selectedvalue == "1") {
                YN = "show";
            }
            else {
                YN = "hide";
            }
            ShowHide(divID, YN);

        }
        function rblSelectedValue1(val, divID) {
            var selectedvalue = val;
            var YN = "";
            if (selectedvalue == "1") {
                YN = "show";
            }
            else {
                YN = "hide";
            }

            ShowHide(divID, YN);

        }
        function rblNoSelectedValue(rbl, divID) {
            var selectedvalue = $("#" + rbl.id + " input:radio:checked").val();
            var YN = "";
            if (selectedvalue == "1") {
                YN = "hide";
            }
            else {
                YN = "show";
            }
            ShowHide(divID, YN);

        }
        function CheckBoxHideUnhide(val, divID, txt) {

            var checkList = document.getElementById(val);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();

            for (var i = 0; i < checkBoxList.length; i++) {

                if (checkBoxList[i].checked) {
                    if (arrayOfCheckBoxLabels[i].innerHTML == txt) {
                        ShowHide(divID, "show");
                    }

                }
                else {

                    if (arrayOfCheckBoxLabels[i].innerHTML == txt) {
                        ShowHide(divID, "hide");
                    }
                }
            }



        }

        //        function CheckBoxHideUnhide(strcblcontrolId, strdivId, strfieldname) {
        //            //alert(strcblcontrolId);
        //            var checkList = document.getElementById(strcblcontrolId);
        //            var checkBoxList = checkList.getElementsByTagName("input");
        //            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
        //            var checkBoxSelectedItems = new Array();

        //            for (var i = 0; i < checkBoxList.length; i++) {

        //                if (checkBoxList[i].checked) {
        //                    if (arrayOfCheckBoxLabels[i].innerHTML == strfieldname) {
        //                        //alert(strfieldname);
        //                        ShowHide(strdivId, "show");
        //                    }
        //                    else {
        //                        ShowHide(strdivId, "hide");
        //                    }
        //                }
        //                else {
        //                    ShowHide(strdivId, "hide");
        //                }
        //            }



        //        }

        function getSelectedValue(DivId, DDText, str) {
            var e = document.getElementById(DDText);
            var text = e.options[e.selectedIndex].innerHTML;
            var YN = "";
            if (text == str) {
                YN = "show";
            }
            else {
                YN = "hide";
            }

            ShowHide(DivId, YN);
        }
        function getMultiDivSelectedValue(DivId, DivId2, DDText, str) {
            var e = document.getElementById(DDText);
            var text = e.options[e.selectedIndex].innerHTML;
            var YN = "";
            if (text == str) {
                YN = "show";
            }
            else {
                YN = "hide";
            }
            ShowHide(DivId, YN);
            ShowHide(DivId2, YN);
        }
        function getSelectedtableValue(DivId, DDText, str, tableId) {
            var e = document.getElementById(DDText);
            var text = e.options[e.selectedIndex].innerHTML;
            var YN = "";
            if (text == str) {
                YN = "show";

            }
            else {
                YN = "hide";

            }
            ShowHide(tableId, "show");

            ShowHide(DivId, YN);

        }

        function ClearTabData(TabId) {
            var tabhid1 = document.getElementById("<%=hidtab1.ClientID%>").value;
            var TabArray1 = tabhid1.split("^");
            var TabDataNew = "";
            for (var i = 0; i < TabArray1.length; i++) {
                var ctrltype = TabArray1[i].split('_');

                var newctrltype = ctrltype[4].substring(0, 3);
                if (newctrltype.toUpperCase() == "TXT") {
                    if (ctrltype[3] == TabId) {
                        document.getElementById(TabArray1[i]).value = "";
                    }
                }
                else if (newctrltype.toUpperCase() == "RDO") {
                    if (ctrltype[3] == TabId) {
                        if (document.getElementById(TabArray1[i]).checked == true) {
                            document.getElementById(TabArray1[i]).checked = false;
                        }
                    }
                }
                else if (newctrltype.toUpperCase() == "DDL") {
                    if (ctrltype[3] == TabId) {
                        if (document.getElementById(TabArray1[i]).selectedIndex != 0) {
                            document.getElementById(TabArray1[i]).selectedIndex = 0;
                        }
                    }
                }
                else if (newctrltype.toUpperCase() == "CBL") {

                    if (ctrltype[3] == TabId) {
                        var checkList = document.getElementById(TabArray1[i]);
                        var checkBoxList = checkList.getElementsByTagName("input");
                        var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
                        var checkBoxSelectedItems = new Array();

                        for (var k = 0; k < checkBoxList.length; k++) {

                            checkBoxList[k].checked = false;
                        }
                    }
                }

            }

        }

        function CheckBoxToggleShowHide(val, divID, txt) {

            var checkList = document.getElementById(val);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();

            for (var i = 0; i < checkBoxList.length; i++) {

                if (checkBoxList[i].checked) {
                    if (arrayOfCheckBoxLabels[i].innerHTML == txt) {
                        ShowHide(divID, "show");
                    }

                }
                else {

                    if (arrayOfCheckBoxLabels[i].innerHTML == txt) {
                        ShowHide(divID, "hide");
                    }
                }
            }



        }
        //        function StringASCII(TabId) {

        //            var tabhid1 = document.getElementById("<%=hidtab1.ClientID%>").value;
        //            var TabArray1 = tabhid1.split("^");
        //            var TabDataNew = "";
        //            for (var i = 0; i < TabArray1.length; i++) {
        //                var ctrltype = TabArray1[i].split('_');

        //                var newctrltype = ctrltype[4].substring(0, 3);
        //                if (newctrltype.toUpperCase() == "TXT") {
        //                    if (ctrltype[3] == TabId) {
        //                        TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
        //                    }
        //                }
        //                else if (newctrltype.toUpperCase() == "RDO") {
        //                    if (ctrltype[3] == TabId) {
        //                        if (document.getElementById(TabArray1[i]).checked == true) {
        //                            TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
        //                        }
        //                    }
        //                }
        //                else if (newctrltype.toUpperCase() == "DDL") {
        //                    if (ctrltype[3] == TabId) {
        //                        if (document.getElementById(TabArray1[i]).selectedIndex != 0) {
        //                            TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).selectedIndex;
        //                        }
        //                    }
        //                }
        //                else if (newctrltype.toUpperCase() == "CBL") {

        //                    if (ctrltype[3] == TabId) {
        //                        var checkList = document.getElementById(TabArray1[i]);
        //                        var checkBoxList = checkList.getElementsByTagName("input");
        //                        var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
        //                        var checkBoxSelectedItems = new Array();

        //                        for (var k = 0; k < checkBoxList.length; k++) {

        //                            if (checkBoxList[k].checked) {
        //                                TabDataNew = TabDataNew + "chkTrue";
        //                            }
        //                        }
        //                    }
        //                }

        //            }


        //            document.getElementById("<%= hdnStringASCIIValue.ClientID%>").value = TabDataNew;
        //        }
        //        function ValidateSave(sender, args) {

        //            var PrevTabName = document.getElementById("<%=hdnPrevTabName.ClientID%>").value;
        //            var TabId = document.getElementById("<%=hdnPrevTabId.ClientID%>").value;
        //            var PrevTabIndex = document.getElementById("<%=hdnPrevTabIndex.ClientID%>").value;
        //            var tabhid1 = document.getElementById("<%=hidtab1.ClientID%>").value;
        //            var TabArray1 = tabhid1.split("^");
        //            var TabDataNew = "";
        //            for (var i = 0; i < TabArray1.length; i++) {


        //                var ctrltype = TabArray1[i].split('_');

        //                var newctrltype = ctrltype[4].substring(0, 3);
        //                if (newctrltype.toUpperCase() == "TXT") {
        //                    if (ctrltype[3] == TabId) {
        //                        TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
        //                    }
        //                }
        //                else if (newctrltype.toUpperCase() == "RDO") {
        //                    if (ctrltype[3] == TabId) {
        //                        if (document.getElementById(TabArray1[i]).checked == true) {
        //                            TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
        //                        }
        //                    }
        //                }
        //                else if (newctrltype.toUpperCase() == "DDL") {
        //                    if (ctrltype[3] == TabId) {
        //                        if (document.getElementById(TabArray1[i]).selectedIndex != 0) {
        //                            TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).selectedIndex;
        //                        }
        //                    }
        //                }
        //                else if (newctrltype.toUpperCase() == "CBL") {

        //                    if (ctrltype[3] == TabId) {
        //                        var checkList = document.getElementById(TabArray1[i]);
        //                        var checkBoxList = checkList.getElementsByTagName("input");
        //                        var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
        //                        var checkBoxSelectedItems = new Array();

        //                        for (var k = 0; k < checkBoxList.length; k++) {

        //                            if (checkBoxList[k].checked) {
        //                                TabDataNew = TabDataNew + "chkTrue";
        //                            }
        //                        }
        //                    }
        //                }

        //            }


        //            var PrevTabData = document.getElementById("<%= hdnStringASCIIValue.ClientID%>").value

        //            if (TabDataNew != PrevTabData) {

        //                var userSelectedYesElement = confirm("" + PrevTabName + " Tab has unsaved data. Do you want to save?");
        //                //get the hidden field reference:
        //                var CurrenttabId = sender.get_activeTab().get_id().split('_');
        //                var CurrentTabIndex = sender._activeTabIndex;
        //                var CurrentTabName = sender.get_activeTab()._header.innerHTML;
        //                CurrenttabId = CurrenttabId[3];
        //                document.getElementById("<%=hdnCurrentTabId.ClientID%>").value = CurrenttabId;
        //                document.getElementById("<%=hdnSaveTabData.ClientID%>").value = userSelectedYesElement;
        //                document.getElementById("<%=hdnCurrenTabName.ClientID%>").value = CurrentTabName;
        //                //document.getElementById("<%=hdnCurrenTabIndex.ClientID%>").value = CurrentTabIndex;
        //                if (userSelectedYesElement) {
        //                    document.getElementById("<%=hdnCurrenTabIndex.ClientID%>").value = CurrentTabIndex;
        //                    document.getElementById("<%=hidtab2.ClientID %>").value = PrevTabName;

        //                }
        //                else {
        //                    document.getElementById("<%=hdnPrevTabIndex.ClientID%>").value = CurrentTabIndex;
        //                    document.getElementById("<%=hdnPrevTabId.ClientID%>").value = CurrenttabId;
        //                    document.getElementById("<%=hdnPrevTabName.ClientID%>").value = CurrentTabName;
        //                    if (document.getElementById("<%=hdnVisitId.ClientID%>").value == "0") {
        //                        ClearTabData(TabId);
        //                    }
        //                    else {

        //                        StringASCII(CurrenttabId);
        //                    }
        //                }

        //                return userSelectedYesElement;
        //            }
        //            else {
        //                var CurrenttabId = sender.get_activeTab().get_id().split('_');
        //                var CurrentTabName = sender.get_activeTab()._header.innerHTML;
        //                var CurrentTabIndex = sender._activeTabIndex;
        //                CurrenttabId = CurrenttabId[3];
        //                document.getElementById("<%=hdnSaveTabData.ClientID%>").value = false;
        //                document.getElementById("<%=hdnPrevTabId.ClientID%>").value = CurrenttabId;
        //                document.getElementById("<%=hdnPrevTabName.ClientID%>").value = CurrentTabName;
        //                document.getElementById("<%=hdnPrevTabIndex.ClientID%>").value = CurrentTabIndex;
        //                StringASCII(CurrenttabId);
        //            }
        //        }    
        //        function togglePC(strcblcontrolId) {

        //            var GV = document.getElementById("<%= UCPresComp.gvPresentingComplaints.ClientID %>");
        //            //var GridView = strcblcontrolId.parentNode.parentNode;
        //            var inputList = GV.getElementsByTagName("input");
        //            var arrayOfCheckBoxLabels = GV.getElementsByTagName("label");
        //            if ((inputList[0].checked == true) && (arrayOfCheckBoxLabels[0].innerText == "None")) {
        //                if (GV.rows.length > 0) {
        //                    for (var i = 1; i < GV.rows.length; i++) {
        //                        var inputs = GV.rows[i].getElementsByTagName('input');
        //                        var lbl = GV.rows[i].getElementsByTagName('label');
        //                        var txt = GV.rows[i].getElementsByTagName('text');
        //                        if (lbl[0].innerText != "None") {
        //                            inputs[0].checked = false;
        //                            var txtbx = GV.rows[i].cells[1].children[0];
        //                            txtbx.disabled = true;
        //                        }
        //                        else if (lbl[0].innerText == "None") {
        //                            var txtbx = GV.rows[i].cells[1].children[0];
        //                            txtbx.disabled = false;
        //                        }
        //                    }
        //                }
        //            }
        //            else if ((inputList[0].checked == true) && (arrayOfCheckBoxLabels[0].innerText != "None")) {
        //                if (GV.rows.length > 0) {
        //                    for (var i = 1; i < GV.rows.length; i++) {
        //                        var inputs = GV.rows[i].getElementsByTagName('input');
        //                        var lbl = GV.rows[i].getElementsByTagName('label');
        //                        if (lbl[0].innerText == "None") {
        //                            inputs[0].checked = false;
        //                            var txtbx = GV.rows[i].cells[1].children[0];
        //                            txtbx.disabled = true;
        //                        }
        //                        else {
        //                            var txtbx = GV.rows[i].cells[1].children[0];
        //                            txtbx.disabled = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
    </script>
    <asp:ScriptManager ID="mst" runat="server">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="UpdateMasterLink" runat="server">
        <ContentTemplate>--%>
    <div style="padding: 8px;">
        <div class="border center formbg">
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad5 whitebg" align="center" width="100%">
                            <label id="lblvdate" class="required right35">
                                *Visit Date:</label>
                            <input id="txtvisitDate" maxlength="11" size="8" name="visitDate" runat="server" />
                            <img onclick="w_displayDatePicker('<%= txtvisitDate.ClientID%>');" height="22" alt="Date Helper"
                                hspace="5" src="../images/cal_icon.gif" width="22" border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                            <input id="hdnVisitIDIE" type="hidden" value="0" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border formbg">
            <br />
            <act:TabContainer ID="tabControl" runat="server" ActiveTabIndex="0" OnActiveTabChanged="tabControl_ActiveTabChanged"
                AutoPostBack="False">
                <act:TabPanel ID="tbpnlgeneral" runat="server" Font-Size="Large" HeaderText="Triage">
                    <HeaderTemplate>
                        Triage
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="border center formbg">
                            <br />
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlHIVCare" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="imgHIVCare" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 class="forms" align="left">
                                                                <asp:Label ID="lblClientInfo" runat="server" Text="Patient Information"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="pnlHivCareDetail" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="border center pad5 whitebg" colspan="2">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 25%;">
                                                                <asp:Label ID="lblPtnAccByCareGiver" runat="server" CssClass="required" Font-Bold="True"
                                                                    Text="*Patient accompanied by caregiver:"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 30%;">
                                                                <asp:RadioButtonList ID="rdopatientcaregiver" runat="server" RepeatDirection="Horizontal"
                                                                    OnClick="rblSelectedValue(this,'divcarrelationYN')">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td style="width: 50%;">
                                                                <div id="divcarrelationYN" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="right" style="width: 50%;">
                                                                                    <label id="lblCaregiver relationship-8888358" align="center">
                                                                                        Caregiver relationship:</label>
                                                                                </td>
                                                                                <td align="left" style="width: 60%;">
                                                                                    <asp:DropDownList Width="70%" runat="server" ID="ddlcaregiverrelationship">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class='border center pad5 whitebg' colspan="2">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label align='center' id='lblHealth education given?-88881455'>
                                                                Health education given?:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <asp:RadioButtonList ID="rdoHealthEducation" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class='border pad5 whitebg' colspan="2" align="left">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 23%' align='right'>
                                                            <label align='center' id='lblDisclosure status-8888813'>
                                                                If adolescent, disclosure status:</label>
                                                        </td>
                                                        <td style='width: 27%' align='left'>
                                                            <asp:DropDownList runat="server" ID="ddlDisclosureStatus">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style='width: 50%'>
                                                            <div id="divReasonNotDisclosed" style="display: none;">
                                                                <table width='100%'>
                                                                    <tr>
                                                                        <td style='width: 50%' align='right'>
                                                                            <label align='center' id='lblSpecify reason not disclosed-88881312'>
                                                                                Specify reason not disclosed:</label>
                                                                        </td>
                                                                        <td style='width: 50%' align='left'>
                                                                            <asp:TextBox runat="server" ID="txtReasonNotDisclosed" Width="180px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div id="divOtherDisclosureReason" style="display: none;">
                                                                <table width='100%'>
                                                                    <tr>
                                                                        <td style='width: 50%' align='right'>
                                                                            <label align='center' id='lblSpecify other disclosure status-88881313'>
                                                                                Specify other disclosure status:</label>
                                                                        </td>
                                                                        <td style='width: 50%' align='left'>
                                                                            <asp:TextBox runat="server" ID="txtOtherDisclosureReason" Width="180px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class='border center pad5 whitebg' colspan="2">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 23%' align='right'>
                                                            <label align='center' id='lblIf adolescent, schooling status-8888805'>
                                                                If adolescent, schooling status:</label>
                                                        </td>
                                                        <td style='width: 27%' align='left'>
                                                            <asp:DropDownList runat="server" ID="ddlSchoolingStatus">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style='width: 50%'>
                                                            <div id="divHighestLevelAttained" style="display: none;">
                                                                <table width='100%'>
                                                                    <tr>
                                                                        <td style='width: 40%' align='right'>
                                                                            <label align='center' id='lblHighest level attained-88881456'>
                                                                                Highest level attained:</label>
                                                                        </td>
                                                                        <td style='width: 60%' align='left'>
                                                                            <asp:DropDownList runat="server" ID="ddlHighestLevelAttained" Width="180px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class='border center pad5 whitebg' colspan="2">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 30%' align='right'>
                                                            <label align='center' id='lblIs client a member of a support group?-8888504'>
                                                                Is client a member of a support group?:</label>
                                                        </td>
                                                        <td style='width: 20%' align='left'>
                                                            <asp:RadioButtonList ID="rdoHIVSupportGroup" runat="server" RepeatDirection="Horizontal"
                                                                OnClick="rblSelectedValue(this,'divHIVmembership');">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="width: 50%;">
                                                            <div id="divHIVmembership" style="display: none;">
                                                                <table width='100%'>
                                                                    <tr>
                                                                        <td style='width: 45%' align='right'>
                                                                            <label align='center' id='lblHIV support group membership-8888505'>
                                                                                HIV support group membership:</label>
                                                                        </td>
                                                                        <td style='width: 55%' align='left'>
                                                                            <asp:TextBox runat="server" MaxLength="50" ID="txtHIVSupportGroupMembership" Width="180px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="border center pad5 whitebg" colspan="2">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 28%;">
                                                                <asp:Label ID="lbladdresschanged" runat="server" CssClass="required" Font-Bold="True"
                                                                    Text="*Has your address or phone changed:"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 22%;">
                                                                <asp:RadioButtonList ID="rdoaddresschanged" runat="server" RepeatDirection="Horizontal"
                                                                    OnClick="rblSelectedValue(this,'hideaddchangeUpdateYN');rblSelectedValue(this,'divUpdatePhone')">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td style="width: 50%;">
                                                                <div id="hideaddchangeUpdateYN" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="right" style="width: 50%;">
                                                                                    <label id="lblAddress change update-8888519" align="center">
                                                                                        Address change update:</label>
                                                                                </td>
                                                                                <td align="left" style="width: 60%;">
                                                                                    <asp:TextBox runat="server" ID="txtAddress_change"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div id="divUpdatePhone" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="right" style="width: 50%;">
                                                                                    <label id="lblUpdated phone No-8888593" align="center">
                                                                                        Updated phone No:</label>
                                                                                </td>
                                                                                <td align="left" style="width: 60%;">
                                                                                    <asp:TextBox runat="server" ID="txtUpdated_phone"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlVitalSigns" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImageButton1" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                <asp:Label ID="lblVitalSigns" runat="server" Text="Vital Signs"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlVitalSignsDetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <UcVitalSign:Uc1 ID="idVitalSign" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </div>
                        <act:CollapsiblePanelExtender ID="CPEHIVCare" runat="server" SuppressPostBack="True"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlHivCareDetail" CollapseControlID="pnlHIVCare"
                            ExpandControlID="pnlHIVCare" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
                            ImageControlID="imgHIVCare" Enabled="True">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEVitalSign" runat="server" SuppressPostBack="True"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlVitalSignsDetails"
                            CollapseControlID="PnlVitalSigns" ExpandControlID="PnlVitalSigns" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="True" ImageControlID="imgVitalSigns" Enabled="True">
                        </act:CollapsiblePanelExtender>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0" id="Table1" runat="server">
                                <tr runat="server" align="center">
                                    <td runat="server" class="form">
                                        <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureTriage" runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" align="center">
                                    <td runat="server" class="form">
                                        <asp:Button ID="btnTriagesave" runat="server" OnClick="btnTriagesave_Click" Text="Save" />
                                        <asp:Button ID="btncloseTriage" Text="Close" runat="server" OnClick="btncloseTriage_Click" />
                                        <asp:Button ID="btnTriagePrint" runat="server" OnClientClick="WindowPrint()" Text="Print" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabPanel1" runat="server" Font-Size="Medium" HeaderText="Clinical History">
                    <HeaderTemplate>
                        Clinical History
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="border center formbg">
                            <br />
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlPresentingComplaint" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgPresentingComplaint" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                <asp:Label ID="lblPresComp" runat="server" Text="Presenting Complaints"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlPresentingComplaintdetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <UcPresComp:Uc6 ID="UCPresComp" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class='border center pad5 whitebg' colspan="2">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 40%' align='right'>
                                                            <label align='center' id='lblIf adolescent and schooling, current school perfomance-88881464'>
                                                                If adolescent and schooling, current school perfomance:</label>
                                                        </td>
                                                        <td style='width: 60%' align='left'>
                                                            <asp:DropDownList runat="server" ID="ddlSchoolPerfomance" Width="180px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlPastmedicalHistory" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgPastmedicalHistory" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                <asp:Label ID="lblheadPastmedicalHistory" runat="server" Text="Past medical History"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlPastmedicalHistorydetails" runat="server">
                                <%--<table width="100%" border="0" cellspacing="6" cellpadding="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <UcPastMedicalHistory:Uc2 ID="UC2" runat="server" />
                                                        
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>--%>
                                <table cellspacing='6' cellpadding='0' width='100%' border='0'>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 18%' align='right'>
                                                        <asp:Label ID="lblMedicalcondition" runat="server" CssClass="required" Font-Bold="True"
                                                            Text="*Medical condition:"></asp:Label>
                                                    </td>
                                                    <td style='width: 32%' align='left'>
                                                        <asp:RadioButtonList ID="rdoMedicalCondition" runat="server" RepeatDirection="Horizontal"
                                                            OnClick="rblSelectedValue(this,'divcblSpecificMedicalCondition')">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td style='width: 50%'>
                                                        <div id="divcblSpecificMedicalCondition" style="display: none;">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align='left'>
                                                                        <label align='center' id='lblPre existing medical condition-8888899'>
                                                                            Pre existing medical condition :</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style='width: 100%'>
                                                                        <div id="divSpecificMedicalCondition" enableviewstate="true" class="customdivbordermultiselect"
                                                                            runat="server">
                                                                            <asp:CheckBoxList ID="cblSpecificMedicalCondition" Width="100%" runat="server">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <div id="divothermedconditon" style="display: none;">
                                                                            <table width='100%'>
                                                                                <tr>
                                                                                    <td style='width: 55%' align='right'>
                                                                                        <label align='center' id='Label3'>
                                                                                            Specify other medical condition :</label>
                                                                                    </td>
                                                                                    <td style='width: 50%' align='left'>
                                                                                        <asp:TextBox ID="txtothermedconditon" Width="180px" runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width="50%">
                                                <tr>
                                                    <td align='left'>
                                                        <label align='center' id='lblSurgical Conditions-8888596'>
                                                            Surgical Conditions :</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div id="divSurgicalConditions" enableviewstate="true" style="border-right: #666699 1px solid;
                                                            border-top: #666699 1px solid; border-left: #666699 1px solid; border-bottom: #666699 1px solid;
                                                            width: 100%; height: 100px; overflow: auto; text-align: left;" runat="server">
                                                            <asp:CheckBoxList ID="cblSurgicalConditions" onclick="CheckBoxToggleShortTerm();"
                                                                Width="100%" RepeatLayout="Flow" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <div id="divCurrentSurgicalConditionYN" style="display: none;">
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 55%' align='right'>
                                                                        <label align='center' id='lblSpecify current surgical condition-8888597'>
                                                                            Specify current surgical condition :</label>
                                                                    </td>
                                                                    <td style='width: 50%' align='left'>
                                                                        <asp:TextBox ID="txtCurrentSurgicalCondition" Width="180px" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divPreviousSurgicalCondition" style="display: none;">
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 55%' align='right'>
                                                                        <label align='center' id='lblSpecify previous surgical condition-8888598'>
                                                                            Specify previous surgical condition :</label>
                                                                    </td>
                                                                    <td style='width: 50%' align='left'>
                                                                        <asp:TextBox ID="txtPreviousSurgicalCondition" Width="180px" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label align='center' id='lblCurrent long term medications-8888900'>
                                                            Current long term medications :</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:RadioButtonList ID="rdoPreExistingMedicalConditionsFUP" runat="server" RepeatDirection="Horizontal"
                                                            OnClick="rblSelectedValue(this,'trAntihypertensives');rblSelectedValue(this,'trHypoglycemics');rblSelectedValue(this,'trothers');">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trAntihypertensives" style="display: none;">
                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                            <%--<div id="divAntihypertensives" style="display: none;">--%>
                                            <table width='100%'>
                                                <tr>
                                                    <td align='left'>
                                                        <label align='center' id='lblAntihypertensives-8888426'>
                                                            Antihypertensives :</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style='width: 60%' align='left'>
                                                        <asp:TextBox ID="txtAntihypertensives" Columns="20" Rows="3" Width="100%" TextMode="MultiLine"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</div>--%>
                                        </td>
                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                            <%--<div id="divAnticonvulsants" style="display: none;">--%>
                                            <table width='100%'>
                                                <tr>
                                                    <td align='left'>
                                                        <label align='center' id='lblAnticonvulsants-8888429'>
                                                            Anticonvulsants :</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left'>
                                                        <asp:TextBox ID="txtAnticonvulsants" Columns="20" Rows="3" Width="100%" TextMode="MultiLine"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</div>--%>
                                        </td>
                                    </tr>
                                    <tr id="trHypoglycemics" style="display: none;">
                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                            <%--<div id="divHypoglycemics" style="display: none;">--%>
                                            <table width='100%'>
                                                <tr>
                                                    <td align='left'>
                                                        <label align='center' id='lblHypoglycemics-8888427'>
                                                            Hypoglycemics :</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left'>
                                                        <asp:TextBox ID="txtHypoglycemics" Columns="20" Rows="3" Width="100%" TextMode="MultiLine"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</div>--%>
                                        </td>
                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                            <table width='100%'>
                                                <tr>
                                                    <td align='left'>
                                                        <label align='center' id='lblRadiotherapy/Chemotherapy -8888901'>
                                                            Radiotherapy/Chemotherapy :</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left'>
                                                        <asp:TextBox ID="txtRadiotherapyChemotherapy" Columns="20" Rows="3" Width="100%"
                                                            TextMode="MultiLine" runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trothers" style="display: none;">
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td align='left'>
                                                        <label align='center' id='Label4'>
                                                            Others :</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='left'>
                                                        <asp:TextBox ID="txtothers" Columns="20" Rows="3" Width="100%" TextMode="MultiLine"
                                                            runat="server">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 30%' align='right'>
                                                        <label align='center' id='lblPreviously admitted since last clinic-8888599'>
                                                            Previously admitted since last clinic :</label>
                                                    </td>
                                                    <td style='width: 20%' align='left'>
                                                        <asp:RadioButtonList ID="rdoPreviousAdmission" runat="server" RepeatDirection="Horizontal"
                                                            OnClick="rblSelectedValue(this,'divPreviousAdmissionDiagnosis');rblSelectedValue(this,'trPreviousAdmission');">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td style='width: 50%'>
                                                        <div id="divPreviousAdmissionDiagnosis" style="display: none;">
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 40%' align='right'>
                                                                        <label align='center' id='lblDiagnosis-8888600'>
                                                                            Diagnosis :</label>
                                                                    </td>
                                                                    <td style='width: 60%' align='left'>
                                                                        <asp:TextBox ID="txtPreviousAdmissionDiagnosis" Width="180px" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trPreviousAdmission" style="display: none;">
                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                            <%--<div id="divPreviousAdmissionStart" style="display: none;">--%>
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label align='center' id='lblAdmission start-8888601'>
                                                            Admission start :</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <input id="txtPreviousAdmissionStart" maxlength="11" size="8" name="visitDate" runat="server" />
                                                        <img onclick="w_displayDatePicker('<%= txtPreviousAdmissionStart.ClientID%>');" height="22"
                                                            alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22" border="0" /><span
                                                                class="smallerlabel">(DD-MMM-YYYY)</span>
                                                        <input id="Hidden4" type="hidden" value="0" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</div>--%>
                                        </td>
                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                            <%--<div id="divPreviousAdmissionEnd" style="display: none;">--%>
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label align='center' id='lblAdmission end-8888602'>
                                                            Admission end :</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <input id="txtPreviousAdmissionEnd" maxlength="11" size="8" name="visitDate" runat="server" />
                                                        <img onclick="w_displayDatePicker('<%= txtPreviousAdmissionEnd.ClientID%>');" height="22"
                                                            alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22" border="0" /><span
                                                                class="smallerlabel">(DD-MMM-YYYY)</span>
                                                        <input id="Hidden5" type="hidden" value="0" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</div>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <asp:Label ID="lblHIVAssociatedCond" runat="server" CssClass="required" Font-Bold="True"
                                                            Text="*HIV associated conditions:"></asp:Label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:DropDownList runat="server" ID="ddlHIVAssociatedConditionsPeads" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <act:CollapsiblePanelExtender ID="CPPnlPresentingComplaint" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlPresentingComplaintdetails"
                            CollapseControlID="PnlPresentingComplaint" ExpandControlID="PnlPresentingComplaint"
                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgPresentingComplaint">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlPastmedicalHistory" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlPastmedicalHistorydetails"
                            CollapseControlID="PnlPastmedicalHistory" ExpandControlID="PnlPastmedicalHistory"
                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgPastmedicalHistory">
                        </act:CollapsiblePanelExtender>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0" id="Table2" runat="server">
                                <tbody>
                                    <tr align="center">
                                        <td class="form">
                                            <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureClinical" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="form">
                                            <asp:Button ID="btnClinicalHistorySave" runat="server" OnClick="btnClinicalHistorySave_Click"
                                                Text="Save" />
                                            <asp:Button ID="btncloseCHistory" Text="Close" runat="server" OnClick="btncloseCHistory_Click"/>
                                            <asp:Button ID="btnClinicalHistoryPrint" Text="Print" OnClientClick="WindowPrint()"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabPanel2" runat="server" Font-Size="Medium" HeaderText="TB Screening">
                    <ContentTemplate>
                        <%--<div class="border center formbg">--%>
                        <table class="border center formbg" width="100%">
                            <tr>
                                <td>
                                    <UcTBScreen:Uc7 ID="UcTBScreen" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <%--</div>--%>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabPanel3" runat="server" Font-Size="Medium" HeaderText="Examination">
                    <ContentTemplate>
                        <div class="border center formbg">
                            <br />
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlPhysicalExam" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgPhysicalExam" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                <asp:Label ID="lblheadPhysicalExam" runat="server" Text="Physical Exam"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlPhysicalExamdetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <UcPhysExam:Uc4 ID="UcPhysicalExam" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlARVsideeffects" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgARVsideeffects" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                ARV side effects</h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlARVsideeffectsdetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="border center pad5 whitebg" colspan="2">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 50%;">
                                                                <label class="required" id="lblARV side effects-8888346" align="center">
                                                                    ARV side effects:</label>
                                                            </td>
                                                            <td align="left" style="width: 60%;">
                                                                <input id="rdoARVsideeffectsyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'trshorttermeffects');"
                                                                    type="radio" name="ARVsideeffects" runat="server" />
                                                                <label>
                                                                    Yes</label>
                                                                <input id="rdoARVsideeffectsno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'trshorttermeffects');"
                                                                    type="radio" name="ARVsideeffects" runat="server" />
                                                                <label>
                                                                    No</label>
                                                                <%--<asp:RadioButtonList ID="rdoARVsideeffects" runat="server" RepeatDirection="Horizontal"
                                                                            OnClick="rblSelectedValue(this,'trshorttermeffects');">
                                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="No" Value="0" ></asp:ListItem>
                                                                        </asp:RadioButtonList>--%>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trshorttermeffects" style="display: none;">
                                            <td class="border center pad5 whitebg" style="width: 50%;">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="left">
                                                                <label id="lblShort term effects-8888862" align="center">
                                                                    Short term effects:</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="divcblShorttermeffects" enableviewstate="true" class="customdivbordermultiselect"
                                                                    runat="server">
                                                                    <asp:CheckBoxList ID="cblShorttermeffects" RepeatLayout="Flow" runat="server">
                                                                    </asp:CheckBoxList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <div id="divSpecifyothershorttermeffects" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="right" style="width: 50%;">
                                                                                    <label id="lblSpecify other short term effects-8888863" align="center">
                                                                                        Specify other short term effects:</label>
                                                                                </td>
                                                                                <td align="left" style="width: 60%;">
                                                                                    <asp:TextBox ID="txtSpecifyothershorttermeffects" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <td class="border center pad5 whitebg" style="width: 50%;">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="left">
                                                                <label id="lblLong term effects-8888864" align="center">
                                                                    Long term effects:</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="divchklistlongtermeffect" enableviewstate="true" class="customdivbordermultiselect"
                                                                    runat="server" style="width: 100%">
                                                                    <asp:CheckBoxList ID="chklistlongtermeffect" RepeatLayout="Flow" runat="server">
                                                                    </asp:CheckBoxList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="divSpecifyOtherlongtermeffects" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="right" style="width: 50%;">
                                                                                    <label id="lblSpecify Other long term effects-8888865" align="center">
                                                                                        Specify Other long term effects:</label>
                                                                                </td>
                                                                                <td align="left" style="width: 60%;">
                                                                                    <asp:TextBox ID="txtlistlongtermeffect" runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlDiagnosis" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgDiagnosis" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                Diagnosis</h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlDiagnosisdetails" runat="server">
                                <table cellspacing='6' cellpadding='0' width='100%' border='0'>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label align='center' id='lblHave you reviewed previuos results-88881378'>
                                                            Have you reviewed previous results:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:RadioButtonList ID="rdoReviewedPreviousResults" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan='2'>
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 100%' align='left'>
                                                        <label align='center' id='lblAdditional information-88881379'>
                                                            Additional information:</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style='width: 100%'>
                                                        <asp:TextBox ID="txtResultsReviewComments" Columns="20" Rows="3" Width="100%" TextMode="MultiLine"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class='border center pad5 whitebg' colspan="2">
                                            <table width="50%">
                                                <tr>
                                                    <td align='left'>
                                                        <label align='center' id='lblDiagnosis-88881366'>
                                                            Diagnosis and current illness at this visit:</label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <div id="divcblDiagnosis2" enableviewstate="true" style="border-right: #666699 1px solid;
                                                            border-top: #666699 1px solid; border-left: #666699 1px solid; border-bottom: #666699 1px solid;
                                                            width: 100%; height: 100px; overflow: auto; text-align: left;" runat="server">
                                                            <asp:CheckBoxList ID="cblDiagnosis2" Width="100%" RepeatLayout="Flow" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <div id="divHIVrelated" style="display: none;">
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 35%' align='right'>
                                                                        <label align='center' id='lblSpecify HIV related OI-8888641'>
                                                                            Specify HIV related OI:</label>
                                                                    </td>
                                                                    <td style='width: 65%' align='left'>
                                                                        <asp:TextBox runat="server" MaxLength="50" ID="txtHIVRelatedOI" Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="divNonHIVrelated" style="display: none;">
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 40%' align='right'>
                                                                        <label align='center' id='lblSpecify Non HIV related OI-8888642'>
                                                                            Specify Non HIV related OI:</label>
                                                                    </td>
                                                                    <td style='width: 60%' align='left'>
                                                                        <asp:TextBox runat="server" MaxLength="50" ID="txtNonHIVRelatedOI" Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlWHOStage" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgWHOStage" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                <asp:Label ID="lblheadWHOStage" runat="server" Text="WHO Stage"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlWHOStagedetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <UcWHOStaging:Uc3 ID="UCWHO" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </div>
                        <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlPhysicalExamDetails"
                            CollapseControlID="PnlPhysicalExam" ExpandControlID="PnlPhysicalExam" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="true" ImageControlID="ImgPhysicalExam">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlARVsideeffects" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlARVsideeffectsdetails"
                            CollapseControlID="PnlARVsideeffects" ExpandControlID="PnlARVsideeffects" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="true" ImageControlID="ImgARVsideeffects">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlDiagnosis" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlDiagnosisdetails" CollapseControlID="PnlDiagnosis"
                            ExpandControlID="PnlDiagnosis" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgDiagnosis">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlWHOStage" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWHOStagedetails" CollapseControlID="PnlWHOStage"
                            ExpandControlID="PnlWHOStage" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgWHOStage">
                        </act:CollapsiblePanelExtender>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0" id="Table3" runat="server">
                                <tbody>
                                    <tr align="center">
                                        <td class="form">
                                            <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureExam" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="form">
                                            <asp:Button ID="btnExaminationSave" runat="server" Text="Save" OnClick="btnExaminationSave_Click" />
                                            <asp:Button ID="btncloseExam" Text="Close" runat="server" OnClick="btncloseExam_Click"/>
                                            <asp:Button ID="btnExaminationPrint" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabPanel4" runat="server" Font-Size="Medium" HeaderText="Management">
                    <HeaderTemplate>
                        Management
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="border center formbg">
                            <br />
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlAdherence" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgAdherence" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                Adherence</h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlAdherencedetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="border center pad5 whitebg" colspan="2">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 25%;">
                                                                <label id="lblHave you missed any doses?-8888904" align="center">
                                                                    Have you missed any doses?:</label>
                                                            </td>
                                                            <td align="left" style="width: 25%;">
                                                                <asp:RadioButtonList ID="rdoHaveyoumissedanydoses" runat="server" RepeatDirection="Horizontal"
                                                                    OnClick="rblSelectedValue(this,'divSpecifywhydosesmissed')">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td style="width: 50%;" align="left">
                                                                <div id="divSpecifywhydosesmissed" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <label id="lblSpecify why doses missed?-88881305" align="center">
                                                                                        Specify why doses missed?:</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="txtSpecifywhydosesmissed" Columns="20" Rows="3" TextMode="MultiLine"
                                                                                        runat="server" Width="100%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="border center pad5 whitebg">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 65%;">
                                                                <label id="lblHave you delayed in taking medication?-8888905" align="center">
                                                                    Have you delayed in taking medication?:</label>
                                                            </td>
                                                            <td align="left" style="width: 35%;">
                                                                <asp:RadioButtonList ID="rdohavedelayed" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <td class="border center pad5 whitebg">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 70%;">
                                                                <label id="Label2" align="center">
                                                                    If missed or delayed dosage refered to counsellor?:</label>
                                                            </td>
                                                            <td align="left" style="width: 30%;">
                                                                <asp:RadioButtonList ID="rdomissdosrefcons" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlDrugAllergiesToxicities" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgDrugAllergiesToxicities" ImageUrl="~/images/arrow-up.gif"
                                                                runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                Drug Allergies Toxicities</h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlDrugAllergiesToxicitiesdetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <UcAllergies:Uc5 ID="UCAllergies" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlWorkupplan" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImageButton12" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                Work up plan</h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlWorkupplandetails" runat="server">
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="border center pad5 whitebg" colspan="2">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="left">
                                                                <label id="lblWork up plan-8888866" align="center">
                                                                    Work up plan:</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 50%;" align="left">
                                                                <asp:TextBox ID="txtworkupplan" Columns="20" Rows="3" Width="100%" TextMode="MultiLine"
                                                                    runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <%--<table width="100%">
                                                            <tbody>
                                                                <tr>
                                                                    <td align="left">
                                                                        <label id="lblLab Evaluations-8888868" align="center">
                                                                            Lab Evaluations:</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divchklistlabevaluation" enableviewstate="true" class="customdivbordermultiselect"
                                                                            runat="server" style="width: 100%">
                                                                            <asp:CheckBoxList ID="chklistlabevaluation" RepeatLayout="Flow" runat="server">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>--%>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:Panel ID="PnlPlan" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgPlan" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                Lab Evaluation</h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlPlanDetails" runat="server">
                                <table cellspacing='6' cellpadding='0' width='100%' border='0'>
                                    <tr>
                                        <td colspan="2">
                                            <uc11:Uclabeval ID="UcLabEval" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div class="center formbg pad5">
                                <table class="border center formbg" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlRegimenPrescribed" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="ImgRegimenPrescribed" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                        </td>
                                                        <td>
                                                            <h2 align="left" class="forms">
                                                                <asp:Label ID="lblheadregimenpresc" runat="server" Text="Regimen Prescribed"></asp:Label></h2>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="PnlRegimenPrescribeddetails" runat="server">
                                <UcPharmacy:Uc8 ID="UserControlKNH_Pharmacy1" runat="server" />
                                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td class="border center pad5 whitebg" colspan="2">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 15%;">
                                                                <label id="lblOI Prophylaxis-8888796" align="center">
                                                                    OI Prophylaxis:</label>
                                                            </td>
                                                            <td align="left" style="width: 35%;">
                                                                <asp:DropDownList Width="70%" runat="server" ID="ddlOIProphylaxis">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 50%;">
                                                                <div id="divCotrimoxazoleprescribedfor" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="right" style="width: 50%;">
                                                                                    <label id="lblCotrimoxazole prescribed for-88881192" align="center">
                                                                                        Cotrimoxazole prescribed for:</label>
                                                                                </td>
                                                                                <td align="left" style="width: 60%;">
                                                                                    <asp:DropDownList Width="70%" runat="server" ID="ddlCotrimoxazoleprescribed">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div id="divFluconazoleshowhide" style="display: none;">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td style="width: 49%;" align="right">
                                                                                <label id="Label1" class="">
                                                                                    Fluconazole prescribed for:
                                                                                </label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList runat="server" ID="ddlfluconazole">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="divOtherOIPropholyxis" style="display: none;">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td align="right" style="width: 50%;">
                                                                                    <label id="lblOther (Specify)-8888578" align="center">
                                                                                        Other (Specify):</label>
                                                                                </td>
                                                                                <td align="left" style="width: 60%;">
                                                                                    <asp:TextBox ID="txtOtherOIPropholyxis" runat="server" Width="180px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="border center pad5 whitebg" style="width: 50%;" colspan="2">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="left" style="width: 50%;">
                                                                <label id="lblOther treatment-8888643" align="center">
                                                                    Other treatment:</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 50%;">
                                                                <asp:TextBox ID="txtOthertreatment" Columns="20" Rows="6" Width="100%" TextMode="MultiLine"
                                                                    runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </div>
                        <act:CollapsiblePanelExtender ID="CPPnlAdherence" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlAdherenceDetails" CollapseControlID="PnlAdherence"
                            ExpandControlID="PnlAdherence" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgAdherence">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlDrugAllergiesToxicities" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlDrugAllergiesToxicitiesdetails"
                            CollapseControlID="PnlDrugAllergiesToxicities" ExpandControlID="PnlDrugAllergiesToxicities"
                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgDrugAllergiesToxicities">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlWorkupplan" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWorkupplandetails"
                            CollapseControlID="PnlWorkupplan" ExpandControlID="PnlWorkupplan" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="true" ImageControlID="ImgDiagnosis">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlRegimenPrescribed" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlRegimenPrescribeddetails"
                            CollapseControlID="PnlRegimenPrescribed" ExpandControlID="PnlRegimenPrescribed"
                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgRegimenPrescribed">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPPnlPlan" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlPlanDetails" CollapseControlID="PnlPlan"
                            ExpandControlID="PnlPlan" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgPlan">
                        </act:CollapsiblePanelExtender>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0" id="Table4" runat="server">
                                <tbody>
                                    <tr align="center">
                                        <td class="form">
                                            <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureMgt" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="form">
                                            <asp:Button ID="btnManagementSave" runat="server" Text="Save" OnClick="btnManagementSave_Click" />
                                            <asp:Button ID="btncloseMgt" Text="Close" runat="server" OnClick="btncloseMgt_Click"/>
                                            <asp:Button ID="btnManagementPrint" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabPanel5" runat="server" Font-Size="Medium" HeaderText="Prevention with Positives">
                    <ContentTemplate>
                        <div class="border center formbg">
                            <table class="border center formbg" width="100%">
                                <tr>
                                    <td>
                                        <UcPWP:Uc9 ID="UcPwp" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
            </act:TabContainer>
        </div>
        <div style="display: none;">
            <table cellspacing="6" cellpadding="0" width="100%">
                <tbody>
                    <tr id="TrSignatureAll" runat="server" visible="false">
                        <td class="form" align="center" valign="top" colspan="2">
                            <label for="VisitDate">
                                Signature:</label>
                            <asp:DropDownList ID="ddSignature" runat="server">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hidupdate" runat="server" />
                            <asp:HiddenField ID="hidregimen" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form" align="center">
                            <asp:HiddenField ID="hdfldDOB" runat="server" />
                            <asp:HiddenField ID="theHitCntrl" runat="server" />
                            <asp:HiddenField ID="hdnPrevTabId" runat="server" />
                            <asp:HiddenField ID="hdnCurrentTabId" runat="server" />
                            <asp:HiddenField ID="hdnPrevTabIndex" runat="server" />
                            <asp:HiddenField ID="hdnCurrenTabIndex" runat="server" />
                            <asp:HiddenField ID="hdnSaveTabData" runat="server" />
                            <asp:HiddenField ID="hdnStringASCIIValue" runat="server" />
                            <asp:HiddenField ID="hdnVisitId" runat="server" />
                            <asp:HiddenField ID="hdnPrevTabName" runat="server" />
                            <asp:HiddenField ID="hdnCurrenTabName" runat="server" />
                            <asp:HiddenField ID="hidtab1" runat="server" />
                            <asp:HiddenField ID="hidtab2" runat="server" />
                            <%--<asp:Button ID="btnsave" Text="Save" runat="server" OnClick="btnsave_Click" />
                                    <asp:Button ID="btncomplete" runat="server" Text="Data Quality Check" OnClick="btncomplete_Click" />
                                    <asp:Button ID="btnCancel" Text="Close" runat="server"></asp:Button>
                                    <asp:Button ID="btnPrint" Text="Print All" Visible="false" runat="server" OnClientClick="WindowPrintAll()" />--%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <uc10:UserControlKNH_Extruder ID="UserControlKNH_Extruder1" runat="server" />
</asp:Content>
