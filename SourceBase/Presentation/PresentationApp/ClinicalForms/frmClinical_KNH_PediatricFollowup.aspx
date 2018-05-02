<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true"
    CodeBehind="frmClinical_KNH_PediatricFollowup.aspx.cs" Inherits="PresentationApp.ClinicalForms.frmClinical_KNH_PediatricFollowup" %>

<%@ Register TagPrefix="UcVitalSign" TagName="Uc1" Src="~/ClinicalForms/UserControl/UserControlKNH_VitalSigns.ascx" %>
<%--<%@ Register TagPrefix="UcPastMedicalHistory" TagName="Uc2" Src="~/ClinicalForms/UserControl/UserControlKNH_PastMedicalHistory.ascx" %>--%>
<%@ Register TagPrefix="UcWHOStaging" TagName="Uc3" Src="~/ClinicalForms/UserControl/UserControlKNH_WHOStaging.ascx" %>
<%@ Register TagPrefix="UcPhysExam" TagName="Uc4" Src="~/ClinicalForms/UserControl/UserControlKNH_PhysicalExamination.ascx" %>
<%@ Register TagPrefix="UcAllergies" TagName="Uc5" Src="~/ClinicalForms/UserControl/UserControlKNH_DrugAllergies.ascx" %>
<%@ Register TagPrefix="UcPresComp" TagName="Uc6" Src="~/ClinicalForms/UserControl/UserControlKNHPresentingComplaints.ascx" %>
<%@ Register TagPrefix="UcTBScreen" TagName="Uc7" Src="~/ClinicalForms/UserControl/UserControlKNH_TBScreening.ascx" %>
<%@ Register TagPrefix="UcPharmacy" TagName="Uc8" Src="~/ClinicalForms/UserControl/UserControlKNH_Pharmacy.ascx" %>
<%@ Register TagPrefix="UcPWP" TagName="Uc9" Src="~/ClinicalForms/UserControl/UserControlKNH_PwP.ascx" %>
<%--<%@ Register src="UserControl/UserControlKNH_Extruder.ascx" tagname="UserControlKNH_Extruder" tagprefix="uc10" %>--%>
<%@ Register src="~/ClinicalForms/UserControl/UserControlKNH_LabEvaluation.ascx" tagname="Uclabeval" tagprefix="uc11" %>
<%@ Register src="~/ClinicalForms/UserControl/UserControlKNH_Signature.ascx" tagname="UserControlKNH_Signature" tagprefix="uc12" %>
<%@ Register src="UserControl/UserControlKNH_BackToTop.ascx" tagname="UserControlKNH_BackToTop" tagprefix="uc13" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
    <br />
    <div style="padding-left: 8px; padding-right: 8px; padding-top: 2px;">
    
        <script language="javascript" type="text/javascript">            buildWeeklyCalendar(0);</script>
        <script type="text/javascript" language="javascript">

//            $(function () {

//                $("#tabs").tabs();

//            });
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

            function CheckBoxHideUnhide(strcblcontrolId, strdivId, strfieldname) {
                //alert(strcblcontrolId);
                var checkList = document.getElementById(strcblcontrolId);
                var checkBoxList = checkList.getElementsByTagName("input");
                var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
                var checkBoxSelectedItems = new Array();

                for (var i = 0; i < checkBoxList.length; i++) {

                    if (checkBoxList[i].checked) {
                        if (arrayOfCheckBoxLabels[i].innerHTML == strfieldname) {
                            ShowHide(strdivId, "show");
                        }
                        else {
                            ShowHide(strdivId, "hide");
                        }
                    }
                    else {
                        ShowHide(strdivId, "hide");
                    }
                }



            }
            function ShowHidePE(theDiv, YN, theFocus) {

                $(document).ready(function () {

                    if (YN == "show") {
                        $("#" + theDiv).slideDown();

                    }
                    if (YN == "hide") {
                        $("#" + theDiv).slideUp();

                    }

                });

            }

            function CheckBoxToggleShowHidePE(val, divID, txt) {
                var checkList = document.getElementById(val);
                var checkBoxList = checkList.getElementsByTagName("input");
                var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
                var checkBoxSelectedItems = new Array();
                var arrayOfCheckBoxLabelsnew;
                alert('aa');
                for (var i = 0; i < checkBoxList.length; i++) {
                    if (checkBoxList[i].checked) {
                        if (arrayOfCheckBoxLabels[i].innerHTML == txt) {
                            ShowHidePE(divID, "show");
                        }

                    }
                    else {

                        if (arrayOfCheckBoxLabels[i].innerHTML == txt) {
                            ShowHidePE(divID, "hide");
                        }
                    }
                }



            }

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
                        if (arrayOfCheckBoxLabels[i].innerText == txt) {
                            ShowHide(divID, "show");
                        }

                    }
                    else {

                        if (arrayOfCheckBoxLabels[i].innerText == txt) {
                            ShowHide(divID, "hide");
                        }
                    }
                }



            }
            function StringASCII(TabId) {



                var tabhid1 = document.getElementById("<%=hidtab1.ClientID%>").value;
                var TabArray1 = tabhid1.split("^");
                var TabDataNew = "";
                for (var i = 0; i < TabArray1.length; i++) {
                    var ctrltype = TabArray1[i].split('_');

                    var newctrltype = ctrltype[4].substring(0, 3);
                    if (newctrltype.toUpperCase() == "TXT") {
                        if (ctrltype[3] == TabId) {
                            TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
                        }
                    }
                    else if (newctrltype.toUpperCase() == "RDO") {
                        if (ctrltype[3] == TabId) {
                            if (document.getElementById(TabArray1[i]).checked == true) {
                                TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
                            }
                        }
                    }
                    else if (newctrltype.toUpperCase() == "DDL") {
                        if (ctrltype[3] == TabId) {
                            if (document.getElementById(TabArray1[i]).selectedIndex != 0) {
                                TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).selectedIndex;
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

                                if (checkBoxList[k].checked) {
                                    TabDataNew = TabDataNew + "chkTrue";
                                }
                            }
                        }
                    }

                }


                document.getElementById("<%= hdnStringASCIIValue.ClientID%>").value = TabDataNew;
            }
            function ValidateSave(sender, args) {

                var PrevTabName = document.getElementById("<%=hdnPrevTabName.ClientID%>").value;
                var TabId = document.getElementById("<%=hdnPrevTabId.ClientID%>").value;
                var PrevTabIndex = document.getElementById("<%=hdnPrevTabIndex.ClientID%>").value;
                var tabhid1 = document.getElementById("<%=hidtab1.ClientID%>").value;
                var TabArray1 = tabhid1.split("^");
                var TabDataNew = "";
//                for (var i = 0; i < TabArray1.length; i++) {


//                    var ctrltype = TabArray1[i].split('_');

//                    var newctrltype = ctrltype[4].substring(0, 3);
//                    if (newctrltype.toUpperCase() == "TXT") {
//                        if (ctrltype[3] == TabId) {
//                            TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
//                        }
//                    }
//                    else if (newctrltype.toUpperCase() == "RDO") {
//                        if (ctrltype[3] == TabId) {
//                            if (document.getElementById(TabArray1[i]).checked == true) {
//                                TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).value;
//                            }
//                        }
//                    }
//                    else if (newctrltype.toUpperCase() == "DDL") {
//                        if (ctrltype[3] == TabId) {
//                            if (document.getElementById(TabArray1[i]).selectedIndex != 0) {
//                                TabDataNew = TabDataNew + document.getElementById(TabArray1[i]).selectedIndex;
//                            }
//                        }
//                    }
//                    else if (newctrltype.toUpperCase() == "CBL") {

//                        if (ctrltype[3] == TabId) {
//                            var checkList = document.getElementById(TabArray1[i]);
//                            var checkBoxList = checkList.getElementsByTagName("input");
//                            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
//                            var checkBoxSelectedItems = new Array();

//                            for (var k = 0; k < checkBoxList.length; k++) {

//                                if (checkBoxList[k].checked) {
//                                    TabDataNew = TabDataNew + "chkTrue";
//                                }
//                            }
//                        }
//                    }

//                }


//                var PrevTabData = document.getElementById("<%= hdnStringASCIIValue.ClientID%>").value

//                if (TabDataNew != PrevTabData) {

//                    var userSelectedYesElement = confirm("" + PrevTabName + " Tab has unsaved data. Do you want to save?");
//                    //get the hidden field reference:
//                    var CurrenttabId = sender.get_activeTab().get_id().split('_');
//                    var CurrentTabIndex = sender._activeTabIndex;
//                    var CurrentTabName = sender.get_activeTab()._header.innerHTML;
//                    CurrenttabId = CurrenttabId[3];
//                    document.getElementById("<%=hdnCurrentTabId.ClientID%>").value = CurrenttabId;
//                    document.getElementById("<%=hdnSaveTabData.ClientID%>").value = userSelectedYesElement;
//                    document.getElementById("<%=hdnCurrenTabName.ClientID%>").value = CurrentTabName;
//                    //document.getElementById("<%=hdnCurrenTabIndex.ClientID%>").value = CurrentTabIndex;
//                    if (userSelectedYesElement) {
//                        document.getElementById("<%=hdnCurrenTabIndex.ClientID%>").value = CurrentTabIndex;
//                        document.getElementById("<%=hidtab2.ClientID %>").value = PrevTabName;
//                        
//                    }
//                    else {
//                        document.getElementById("<%=hdnPrevTabIndex.ClientID%>").value = CurrentTabIndex;
//                        document.getElementById("<%=hdnPrevTabId.ClientID%>").value = CurrenttabId;
//                        document.getElementById("<%=hdnPrevTabName.ClientID%>").value = CurrentTabName;
//                        if (document.getElementById("<%=hdnVisitId.ClientID%>").value == "0") {
//                            ClearTabData(TabId);
//                        }
//                        else {

//                            StringASCII(CurrenttabId);
//                        }
//                    }

//                    return userSelectedYesElement;
//                }
//                else {
//                    var CurrenttabId = sender.get_activeTab().get_id().split('_');
//                    var CurrentTabName = sender.get_activeTab()._header.innerHTML;
//                    var CurrentTabIndex = sender._activeTabIndex;
//                    CurrenttabId = CurrenttabId[3];
//                    document.getElementById("<%=hdnSaveTabData.ClientID%>").value = false;
//                    document.getElementById("<%=hdnPrevTabId.ClientID%>").value = CurrenttabId;
//                    document.getElementById("<%=hdnPrevTabName.ClientID%>").value = CurrentTabName;
//                    document.getElementById("<%=hdnPrevTabIndex.ClientID%>").value = CurrentTabIndex;
//                    StringASCII(CurrenttabId);
//                }
            }    

        </script>
        <%--<asp:ScriptManager ID="mst" runat="server">
        </asp:ScriptManager>--%>
        <%--<asp:UpdatePanel ID="UpdateMasterLink" runat="server">
            <ContentTemplate>--%>
                <div style="padding: 8px;">
                    <div class="border center formbg">
                        <table cellspacing="6" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td class="border pad5 whitebg" align="center" width="100%">
                                        <label id="lblvdate" runat="server" class="required right35">
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
                        <act:TabContainer ID="tabControl" runat="server" ActiveTabIndex="0" >
                            <act:TabPanel ID="tbpnlgeneral" runat="server" Font-Size="Medium" HeaderText="Triage">
                                <ContentTemplate>
                                    <div class="border center formbg">
                                        <br />
                                        <div class="formbg pad5">
                                            <table class="border formbg " width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Panel ID="pnlHIVCare" runat="server" >
                                                            <table>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="imgHIVCare" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <h2 class="forms" align="left">
                                                                        <asp:Label ID="lblClientInfo" runat="server" Text="Patient Information"></asp:Label>
                                                                            </h2>
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
                                                        <td class="border pad5 whitebg" colspan="2">
                                                            <table width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='width: 25%' align="left">
                                                                        <asp:Label ID="lblPtnAccByCareGiver" runat="server" CssClass="required" Font-Bold="True" 
                                                                            Text="*Patient accompanied by caregiver:"></asp:Label>
                                                                            
                                                                        </td>
                                                                        <td style='width: 25%' align="left">
                                                                            <asp:RadioButtonList ID="rdopatientcaregiver" runat="server" RepeatDirection="Horizontal"
                                                                                OnClick="rblSelectedValue(this,'divcarrelationYN')">
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td style='width: 50%' align="center">
                                                                            <div id="divcarrelationYN" style="display: none;">
                                                                                <table>
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
                                                        <td class="border center pad5 whitebg" colspan="2">
                                                            <table width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='width: 43%' align="left">
                                                                            <table>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align="left" style="width: 67%;">
                                                                                        <asp:Label ID="lbladdresschanged" runat="server" CssClass="required" Font-Bold="True" 
                                                                                        Text="*Has your address or phone changed:"></asp:Label>
                                                                                            
                                                                                        </td>
                                                                                        <td align="left" style="width: 33%;">
                                                                                            <asp:RadioButtonList ID="rdoaddresschanged" runat="server" RepeatDirection="Horizontal"
                                                                                                OnClick="rblSelectedValue(this,'hideaddchangeUpdateYN');rblSelectedValue(this,'divUpdated_phone')">
                                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td style='width: 57%' align="left">
                                                                            <div id="hideaddchangeUpdateYN" style="display: none;">
                                                                                <table>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align="right" style="width: 65%;">
                                                                                                <label id="lblAddress change update-8888519" align="center">
                                                                                                    Address change update:</label>
                                                                                            </td>
                                                                                            <td align="left" style="width: 35%;">
                                                                                                <asp:TextBox runat="server" ID="txtAddresschange"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                            <div id="divUpdated_phone" style="display: none;">
                                                                                <table>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align="right" style="width: 65%;">
                                                                                                <label id="lblUpdated phone No-8888593" align="center">
                                                                                                    Updated phone No:</label>
                                                                                            </td>
                                                                                            <td align="left" style="width: 35%;">
                                                                                                <asp:TextBox runat="server" ID="txtUpdated_phone"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="border center pad5 whitebg" colspan="2" align="center">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style='width: 50%' align='right'>
                                                                        <label align='center' id='lblChild Primary Caregiver-8888803'>
                                                                            Child Primary Caregiver:</label>
                                                                    </td>
                                                                    <td style='width: 50%' align='left'>
                                                                        <asp:TextBox runat="server" ID="txtPrimaryCareGiver" Width="180px"></asp:TextBox>
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
                                                                        <td align="left" style='width: 32%'>
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style='width: 50%' align='right'>
                                                                                            <label align='center' id='lblDisclosure status-8888813'>
                                                                                                Disclosure status:</label>
                                                                                        </td>
                                                                                        <td style='width: 50%' align='left'>
                                                                                            <asp:DropDownList runat="server" ID="ddlDisclosureStatus">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td align="right" style='width: 68%'>
                                                                            <div id="divReasonNotDisclosed" style="display: none;">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style='width: 40%' align='right'>
                                                                                            <label align='center' id='lblSpecify reason not disclosed-88881312'>
                                                                                                Specify reason not disclosed:</label>
                                                                                        </td>
                                                                                        <td style='width: 60%' align='left'>
                                                                                            <asp:TextBox runat="server" ID="txtReasonNotDisclosed"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <div id="divOtherDisclosureReason" style="display: none;">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style='width: 40%' align='right'>
                                                                                            <label align='center' id='lblSpecify other disclosure status-88881313'>
                                                                                                Specify other disclosure status:</label>
                                                                                        </td>
                                                                                        <td style='width: 60%' align='left'>
                                                                                            <asp:TextBox runat="server" ID="txtOtherDisclosureReason"></asp:TextBox>
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
                                                        <td class='border center pad5 whitebg' colspan="2" align="left">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style='width: 20%' align='right'>
                                                                        <label align='center' id='lblFather alive?-88881364'>
                                                                            Father alive?:</label>
                                                                    </td>
                                                                    <td style='width: 30%' align='left'>
                                                                        <input id="rdofatheraliveyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divDateOfDeathFather')"
                                                                            type="radio" name="fatheralive" runat="server" />
                                                                         &nbsp; &nbsp; <label>Yes</label>
                                                                        <input id="rdofatheraliveno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divDateOfDeathFather');"
                                                                            type="radio" name="fatheralive" runat="server" />
                                                                         &nbsp; &nbsp; <label>No</label>
                                                                    </td>
                                                                    <td style='width: 50%' align="right">
                                                                        <div id="divDateOfDeathFather" style="display: none;">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td style='width: 40%' align='right'>
                                                                                        <label align='center' id='lblDate of fathers death-8888809'>
                                                                                            Date of father's death:</label>
                                                                                    </td>
                                                                                    <td style='width: 60%' align='left'>
                                                                                        <input id="txtDateOfDeathFather" maxlength="11" size="8" runat="server" type="text" />
                                                                                        
                                                                                        &nbsp; &nbsp; <img onclick="w_displayDatePicker('<%= txtDateOfDeathFather.ClientID%>');" height="22"
                                                                                            alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0" /><span
                                                                                                class="smallerlabel">(DD-MMM-YYYY)</span>
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
                                                        <td class='border center pad5 whitebg' colspan="2" align="left">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style='width: 20%' align='right'>
                                                                        <label align='center' id='lblMother alive?-88881363'>
                                                                            Mother alive?:</label>
                                                                    </td>
                                                                    <td style='width: 30%' align='left'>
                                                                        <input id="rdomotheraliveyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divDateOfDeathMother');"
                                                                            type="radio" name="motheraliveyes" runat="server" />
                                                                         &nbsp; &nbsp; <label>Yes</label>
                                                                        <input id="rdomotheraliveno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divDateOfDeathMother');"
                                                                            type="radio" name="motheraliveyes" runat="server" />
                                                                         &nbsp; &nbsp; <label>No</label>
                                                                    </td>
                                                                    <td style='width: 50%' align="right">
                                                                        <div id="divDateOfDeathMother" style="display: none;">
                                                                            <table width='100%'>
                                                                                <tr>
                                                                                    <td style='width: 40%' align='right'>
                                                                                        <label align='center' id='lblDate of mothers death-8888807'>
                                                                                            Date of mother's death:</label>
                                                                                    </td>
                                                                                    <td style='width: 60%' align='left'>
                                                                                        <input id="txtDateOfDeathMother" maxlength="11" size="8" runat="server" type="text" />
                                                                                        
                                                                                        &nbsp; &nbsp; <img onclick="w_displayDatePicker('<%= txtDateOfDeathMother.ClientID%>');" height="22"
                                                                                            alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0" /><span
                                                                                                class="smallerlabel">(DD-MMM-YYYY)</span>
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
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left" style='width: 30%'>
                                                                        <table width='100%'>
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style='width: 50%' align='right'>
                                                                                        <label align='center' id='lblSchooling status-8888805'>
                                                                                            Schooling status:</label>
                                                                                    </td>
                                                                                    <td style='width: 50%' align='left'>
                                                                                        <asp:DropDownList runat="server" ID="ddlSchoolingStatus">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                    <td style='width: 70%' align="left">
                                                                        <div id="divHighestLevelAttained" style="display: none;">
                                                                            <table width='100%'>
                                                                                <tr>
                                                                                    <td style='width: 60%' align='right'>
                                                                                        <label align='center' id='lblHighest level attained-88881456'>
                                                                                            Highest level attained:</label>
                                                                                    </td>
                                                                                    <td style='width: 40%' align='left'>
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
                                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 60%' align='right'>
                                                                        <label align='center' id='lblIs client a member of a support group?-8888504'>
                                                                            Is client a member of a support group?:</label>
                                                                    </td>
                                                                    <td style='width: 40%' align='left'>
                                                                        <asp:RadioButtonList ID="rdoHIVSupportGroup" runat="server" RepeatDirection="Horizontal">
                                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td class='border center pad5 whitebg' style='width: 50%'>
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 50%' align='right'>
                                                                        <label align='center' id='Label1'>
                                                                            HIV support group membership:</label>
                                                                    </td>
                                                                    <td style='width: 50%' align='left'>
                                                                        <asp:TextBox runat="server" ID="txtHIVSupportGroupMembership" Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class='border pad5 whitebg' align="center" colspan="2">
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
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        
                                        <div class="center formbg pad5">
                                            <table class="border center formbg" width="100%">
                                                <tr>
                                                    <td align="left">
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
                                            <table cellspacing="6" cellpadding="0" width="100%" border="0" id="Table1">
                                                <tr align="center">
                                                    <td class="form">
                                                        <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureTriage" 
                                                            runat="server" />
                                                    </td>
                                                </tr>
                                                <tr id="tblSaveButton" align="center">
                                                    <td class="form">
                                                        <asp:Button ID="btnTriagesave" runat="server" OnClick="btnTriagesave_Click" 
                                                            Text="Save" />
                                                            <asp:Button ID="btncloseTriage" Text="Close"  runat="server" OnClick="btncloseTriage_Click"/>
                                                        <asp:Button ID="btnTriagePrint" runat="server" OnClientClick="WindowPrint()" 
                                                            Text="Print" />
                                                    </td>
                                                </tr>
                                                <tr id="tblDeleteButton" style="display: none" align="center">
                                                    <td align="center" class="form" width="100%">
                                                        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" 
                                                            Text="Delete" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="TabPanel1" runat="server" Font-Size="Medium" HeaderText="Clinical History">
                                <ContentTemplate>
                                    <div class="border center formbg">
                                        <br />
                                        <div class="center formbg pad5">
                                            <table class="border center formbg" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Panel ID="PnlMedicalHistory" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImgMedicalHistory" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <h2 align="left" class="forms">
                                                                            Medical History (Disease, Diagnosis and Treatment)</h2>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Panel ID="PnlMedicalHistorydetails" runat="server">
                                            <table cellspacing='6' cellpadding='0' width='100%' border='0'>
                                                <tr>
                                                    <td class='border center pad5 whitebg' style="width: 50%">
                                                        <table width='100%'>
                                                            <tr>
                                                                <td style='width: 50%' align='right'>
                                                                    <label align='center' id='lblMedical History-8888917'>
                                                                        Medical history:</label>
                                                                </td>
                                                                <td style='width: 50%' align='left'>
                                                                    <asp:RadioButtonList ID="rdoMedicalHistory" runat="server" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class='border center pad5 whitebg' style="width: 50%">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align='left'>
                                                                    <label align='center' id='lblChronic condition-88881301'>
                                                                        Chronic condition:</label>
                                                                </td>
                                                                <tr>
                                                                </tr>
                                                                <td>
                                                                    <div id="div2" class="customdivbordermultiselect" style="width: 100%" runat="server">
                                                                        <asp:CheckBoxList ID="cblChronicCondition" Width="100%" runat="server">
                                                                        </asp:CheckBoxList>
                                                                    </div>
                                                                </td>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divOtherChronicCondition" style="display: none;">
                                                                            <table width='100%'>
                                                                                <tr>
                                                                                    <td align='left'>
                                                                                        <label align='center' id='lblSpecify other chronic condition-88881302'>
                                                                                            Specify other chronic condition:</label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align='left'>
                                                                                        <asp:TextBox ID="txtOtherChronicCondition" Columns="2" Rows="3" TextMode="MultiLine"
                                                                                            Width="200px" runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class='border center pad5 whitebg' colspan='2'>
                                                        <table width='100%'>
                                                            <tr>
                                                                <td style='width: 100%' align='left'>
                                                                    <label align='center' id='lblSpecify Medical History-8888829'>
                                                                        Specify medical history:</label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style='width: 100%'>
                                                                    <asp:TextBox ID="txtOtherMedicalHistorySpecify" Columns="20" Rows="3" TextMode="MultiLine"
                                                                        runat="server" Width="100%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            </td> </tr> </tbody> </table>
                                        </asp:Panel>

                                        <div class="center formbg pad5">
                                            <table class="border center formbg" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Panel ID="PnlPresentingComplaint" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="ImgPresentingComplaint" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <h2 align="left" class="forms">
                                                                            <asp:Label ID="lblPresComp" runat="server" Text="Presenting Complaints"></asp:Label> </h2>
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
                                                            <%--<asp:Panel ID="Whopnl" runat="server"></asp:Panel>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class='border center pad5 whitebg'>
                                                        <table width='100%'>
                                                            <tr>
                                                                <td style='width: 50%' align='right'>
                                                                    <label align='center' id='lblIf schooling, current school perfomance-88881464'>
                                                                        If schooling, current school perfomance:</label>
                                                                </td>
                                                                <td style='width: 50%' align='left'>
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
                                                    <td align="left">
                                                        <asp:Panel ID="PnlImmunisationStatus" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="ImgImmunisationStatus" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <h2 align="left" class="forms">
                                                                            Immunisation Status</h2>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Panel ID="PnlImmunisationStatusDetails" runat="server">
                                            <table cellspacing='6' cellpadding='0' width='100%' border='0'>
                                                <tr>
                                                    <td class='border center pad5 whitebg' colspan="2">
                                                        <table width='100%'>
                                                            <tr>
                                                                <td style='width: 50%' align='right'>
                                                                    <label align='center' id='lblImmunisation Status-8888833'>
                                                                        Immunisation Status:</label>
                                                                </td>
                                                                <td style='width: 50%' align='left'>
                                                                    <asp:DropDownList runat="server" ID="ddlImmunisationStatus" Width="180px">
                                                                    </asp:DropDownList>
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
                                                    <td align="left">
                                                        <asp:Panel ID="PnlTBHistory" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImgTBHistory" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <h2 align="left" class="forms">
                                                                            TB History</h2>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Panel ID="PnlTBHistoryDetails" runat="server">
                                            <table cellspacing='6' cellpadding='0' width='100%' border='0'>
                                                <tr>
                                                    <td class='border center pad5 whitebg' colspan="2">
                                                        <table width='100%'>
                                                            <tr>
                                                                <td style='width: 20%' align='right'>
                                                                    <label align='center' id='lblTB History-8888830'>
                                                                        TB History:</label>
                                                                </td>
                                                                <td style='width: 30%' align='left'>
                                                                    <asp:RadioButtonList ID="rdoTBHistory" runat="server" RepeatDirection="Horizontal"
                                                                        OnClick="rblSelectedValue(this,'divTBrxCompleteDate');rblSelectedValue(this,'divTBRetreatmentDate')">
                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td style='width: 50%'>
                                                                    <div id="divTBrxCompleteDate" style="display: none;">
                                                                        <table width='100%'>
                                                                            <tr>
                                                                                <td style='width: 40%' align='right'>
                                                                                    <label align='center' id='lbl Complete TX Date-8888831'>
                                                                                        Complete TX Date:</label>
                                                                                </td>
                                                                                <td style='width: 60%' align='left'>
                                                                                    <input id="txtTBrxCompleteDate" maxlength="11" size="8" name="visitDate" runat="server" />
                                                                                    <img onclick="w_displayDatePicker('<%= txtTBrxCompleteDate.ClientID%>');" height="22"
                                                                                        alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0" /><span
                                                                                            class="smallerlabel">(DD-MMM-YYYY)</span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="divTBRetreatmentDate" style="display: none;">
                                                                        <table width='100%'>
                                                                            <tr>
                                                                                <td style='width: 40%' align='right'>
                                                                                    <label align='center' id='lblRetreatment Date-8888832'>
                                                                                        Retreatment Date:</label>
                                                                                </td>
                                                                                <td style='width: 60%' align='left'>
                                                                                    <input id="txtTBRetreatmentDate" maxlength="11" size="8" name="visitDate" runat="server" />
                                                                                    <img onclick="w_displayDatePicker('<%= txtTBRetreatmentDate.ClientID%>');" height="22"
                                                                                        alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0" /><span
                                                                                            class="smallerlabel">(DD-MMM-YYYY)</span>
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
                                    </div>
                                    <act:CollapsiblePanelExtender ID="CPPnlMedicalHistory" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlMedicalHistoryDetails"
                                        CollapseControlID="PnlMedicalHistory" ExpandControlID="PnlMedicalHistory" CollapsedImage="~/images/arrow-up.gif"
                                        Collapsed="true" ImageControlID="ImgMedicalHistory">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlPresentingComplaint" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlPresentingComplaintdetails"
                                        CollapseControlID="PnlPresentingComplaint" ExpandControlID="PnlPresentingComplaint"
                                        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgPresentingComplaint">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlImmunisationStatus" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlImmunisationStatusDetails"
                                        CollapseControlID="PnlImmunisationStatus" ExpandControlID="PnlImmunisationStatus"
                                        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgImmunisationStatus">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlTBHistory" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlTBHistoryDetails" CollapseControlID="PnlTBHistory"
                                        ExpandControlID="PnlTBHistory" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                        ImageControlID="ImgTBHistory">
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
                                                            <asp:Button ID="btnClinicalHistorySave" runat="server" OnClick="btnClinicalHistorySave_Click" Text="Save" />
                                                            <%--<asp:Button ID="btnClinicalHistoryDQSave" runat="server" OnClick="btnClinicalHistoryDQSave_Click" Text="Data Quality Check" />--%>
                                                            <asp:Button ID="btncloseCHistory" Text="Close"  runat="server" OnClick="btncloseCHistory_Click"/>
                                                            <asp:Button ID="btnClinicalHistoryPrint" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="TabPanel2" runat="server" Font-Size="Medium" HeaderText="TB Screening">
                                <ContentTemplate>
                                    <div class="border center formbg">
                                        <table class="border center formbg" width="100%">
                                            <tr>
                                                <td>
                                                    <UcTBScreen:Uc7 ID="UcTBScreen" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="TabPanel3" runat="server" Font-Size="Medium" HeaderText="Examination">
                                <ContentTemplate>
                                    <div class="border center formbg">
                                        <br />
                                        <div class="center formbg pad5">
                                            <table class="border center formbg" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Panel ID="PnlCurrentLongTermMedications" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImgCurrentLongTermMedications" ImageUrl="~/images/arrow-up.gif"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <h2 align="left" class="forms">
                                                                            <asp:Label ID="lblheadcurrentlongterm" runat="server" Text="Current Long Term Medications"></asp:Label></h2>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Panel ID="PnlCurrentLongTermMedicationsDetails" runat="server">
                                            
                                            <table id="Table8" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td class="border whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td style="width: 50%" align="left">
                                                                        <asp:Label ID="lblchlongtermmed" runat="server" CssClass="required" Font-Bold="True" 
                                                                                        Text="*Long term medications:"></asp:Label>                                                                           
                                                                            <div class="customdivbordermultiselect">
                                                                                <asp:CheckBoxList ID="chkLongTermMedication" RepeatLayout="Flow" 
                                                                                    Width="190px" runat="server">
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 50%; display: none;" align="left" id="divLongTermMedication">
                                                                            <label id="Label2" for="txtotherchroniccondition">
                                                                                Other current long term medications:</label><br />
                                                                            <asp:TextBox ID="txOtherLongTermMedications" runat="server" Width="99%">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none;">
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table width="100%;" border="0">
                                                                    <tr>
                                                                        <td style="width: 52%;" align="right">
                                                                            <label class="required">
                                                                                *Long term medications:</label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <input id="rblbtnLongTermMedicationsyes" onmouseup="up(this);" onfocus="up(this);"
                                                                                onclick="down(this);" type="radio" name="rblbtnLongTermMedicationsyes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rblbtnLongTermMedicationsno" onmouseup="up(this);" onfocus="up(this);"
                                                                                onclick="down(this);" type="radio" name="rblbtnLongTermMedicationsyes" runat="server" />
                                                                            <label>
                                                                                No</label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table class="tbl-left" border="1">
                                                                    <tr>
                                                                        <td style="width: 54%;" align="right" class="data-lable">
                                                                            <table width="100%;" border="0">
                                                                                <tr>
                                                                                    <td style="width: 60%;" align="right">
                                                                                        <label>
                                                                                            Other current long term medications:</label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none;">
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table>
                                                                    <tr>
                                                                        <td align="right" style="width: 52%;">
                                                                            <label>
                                                                                Antifungals:</label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <input id="dtAntifungalsDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                maxlength="11" size="11" name="VisitDate" runat="server" />
                                                                            <img id="Img17" onclick="w_displayDatePicker('<%=dtAntifungalsDate.ClientID%>');"
                                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                            <span class="smallerlabel" id="Span17">(DD-MMM-YYYY)</span>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table class="tbl-left">
                                                                    <tr>
                                                                        <td align="right" class="data-lable">
                                                                            <label>
                                                                                Anticonvulsants:</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <input id="dtAntihypertensivesDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                maxlength="11" size="11" name="VisitDate" runat="server" />
                                                                            <img id="Img16" onclick="w_displayDatePicker('<%=dtAntihypertensivesDate.ClientID%>');"
                                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                            <span class="smallerlabel" id="Span16">(DD-MMM-YYYY)</span>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none;">
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table width="100%" class="tbl-right" border="0">
                                                                    <tr>
                                                                        <td style="width: 52%;" align="right">
                                                                            <label>
                                                                                Sulfa TMP:</label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <input id="dtSulfaTMPDate" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                                                                                onfocus="javascript:vDateType='3'" maxlength="11" size="11" name="VisitDate"
                                                                                runat="server" />
                                                                            <img id="Img15" onclick="w_displayDatePicker('<%=dtSulfaTMPDate.ClientID%>');" height="22 "
                                                                                alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                name="appDateimg" style="vertical-align: text-bottom;" />
                                                                            <span class="smallerlabel" id="Span15">(DD-MMM-YYYY)</span>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table class="tbl-left">
                                                                    <tr>
                                                                        <td align="right" class="data-lable">
                                                                            <label>
                                                                                Other long term medications:</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <input id="dtOtherCurrentLongTermMedications" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                maxlength="11" size="11" name="VisitDate" runat="server" />
                                                                            <img id="Img21" onclick="w_displayDatePicker('<%=dtOtherCurrentLongTermMedications.ClientID%>');"
                                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                            <span class="smallerlabel" id="Span21">(DD-MMM-YYYY)</span>
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
                                                    <td align="left">
                                                        <asp:Panel ID="PnlMedicalConditions" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImgMedicalConditions" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <h2 align="left" class="forms">
                                                                            <asp:Label ID="lblMedicalConditions" runat="server" Text="Medical Conditions"></asp:Label></h2>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Panel ID="PnlMedicalConditionsDetails" runat="server">
                                            <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td class='border center pad5 whitebg' colspan="2">
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 22%' align='right'>
                                                                        <label align='center' id='lblMilestone appropriate for age?-88881303'>
                                                                            Milestone appropriate for age?:</label>
                                                                    </td>
                                                                    <td style='width: 28%' align='left'>
                                                                        <input id="rdoMilestoneAppropriateYes" onmouseup="up(this);" onfocus="up(this);"
                                                                            onclick="down(this);rblSelectedValue1(0,'divResonMilestoneInappropriate')" type="radio"
                                                                            name="MilestoneAppropriate" runat="server" />
                                                                        <label>
                                                                            Yes</label>
                                                                        <input id="rdoMilestoneAppropriateNo" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divResonMilestoneInappropriate');"
                                                                            type="radio" name="MilestoneAppropriate" runat="server" />
                                                                        <label>
                                                                            No</label>
                                                                        <%--<asp:RadioButtonList ID="rdoMilestoneAppropriate" runat="server" RepeatDirection="Horizontal"
                                                                        OnClick="rblSelectedValue1(1,'divResonMilestoneInappropriate')">
                                                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                    </asp:RadioButtonList>--%>
                                                                    </td>
                                                                    <td style='width: 50%'>
                                                                        <div id="divResonMilestoneInappropriate" style="display: none;">
                                                                            <table width='100%'>
                                                                                <tr>
                                                                                    <td style='width: 100%' align='left'>
                                                                                        <label align='center' id='lblIf No specify why inappropriate-88881304'>
                                                                                            If No specify why inappropriate:</label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style='width: 100%'>
                                                                                        <asp:TextBox ID="txtResonMilestoneInappropriate" Columns="20" Rows="2" Width="100%"
                                                                                            TextMode="MultiLine" runat="server"></asp:TextBox>
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
                                                        <td colspan="2">
                                                            <UcPhysExam:Uc4 ID="UcPhysExam" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                        
                                        <div class="center formbg pad5">
                                            <table class="border center formbg" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Panel ID="PnlWHOStaging" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImgWHOStaging" ImageUrl="~/images/arrow-up.gif" runat="server" />
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
                                        <asp:Panel ID="PnlWHOStagingDetails" runat="server">
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
                                        
                                        <div class="center formbg pad5">
                                            <table class="border center formbg" width="100%">
                                                <tr>
                                                    <td align="left">
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
                                        <asp:Panel ID="PnlDiagnosisDetails" runat="server">
                                            <table cellspacing='6' cellpadding='0' width='100%' border='0'>
                                                <tr>
                                                    <td class='border center pad5 whitebg' colspan="2">
                                                        <table width='100%'>
                                                            <tr>
                                                                <td style='width: 15%' align='right'>
                                                                    <label align='center' id='lblImpression-8888925'>
                                                                        Impression:</label>
                                                                </td>
                                                                <td style='width: 35%' align='left'>
                                                                    <asp:DropDownList runat="server" ID="ddlHAARTImpression" Width="180px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style='width: 50%'>
                                                                    <div id="divSpecifyHAART" style="display: none;">
                                                                        <table width='100%'>
                                                                            <tr>
                                                                                <td style='width: 50%' align='right'>
                                                                                    <label align='center' id='lblSpecify HAART patient impression-8888926'>
                                                                                        Specify HAART patient impression:</label>
                                                                                </td>
                                                                                <td style='width: 50%' align='left'>
                                                                                    <asp:DropDownList runat="server" ID="ddlHAARTexperienced" Width="180px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="divSpecifyotherimpression" style="display: none;">
                                                                        <table width='100%'>
                                                                            <tr>
                                                                                <td style='width: 50%'>
                                                                                    <label align='center' id='lblSpecify other impression-88881307'>
                                                                                        Specify other impression:</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style='width: 50%'>
                                                                                    <asp:TextBox ID="txtOtherHAARTImpression" Columns="20" Rows="6" Width="100%" TextMode="MultiLine"
                                                                                        runat="server"></asp:TextBox>
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
                                                                    <asp:TextBox ID="txtResultsReviewComments" Columns="20" Rows="6" Width="100%" TextMode="MultiLine"
                                                                        runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class='border center pad5 whitebg' colspan="2">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style='width: 50%'>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <label align='center' id='lblDiagnosis-88881366'>
                                                                                    Diagnosis:</label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <div id="divcblDiagnosis2" enableviewstate="true" class="customdivbordermultiselect"
                                                                                    runat="server">
                                                                                    <asp:CheckBoxList ID="cblDiagnosis2" Width="100%" RepeatLayout="Flow" runat="server">
                                                                                    </asp:CheckBoxList>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <div id="divSpecifyHIVrelatedOI" style="display: none;">
                                                                        <table width='100%'>
                                                                            <tr>
                                                                                <td style='width: 40%' align='right'>
                                                                                    <label align='center' id='lblSpecify HIV related OI-8888641'>
                                                                                        Specify HIV related OI:</label>
                                                                                </td>
                                                                                <td style='width: 60%' align='left'>
                                                                                    <asp:TextBox runat="server" MaxLength="50" ID="txtHIVRelatedOI" Width="180px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="divNonHIVRelatedOI" style="display: none;">
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
                                        
                                    </div>
                                    <act:CollapsiblePanelExtender ID="CPPnlCurrentLongTermMedications" runat="server"
                                        SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlCurrentLongTermMedicationsDetails"
                                        CollapseControlID="PnlCurrentLongTermMedications" ExpandControlID="PnlCurrentLongTermMedications"
                                        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgCurrentLongTermMedications">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlMedicalConditions" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlMedicalConditionsDetails"
                                        CollapseControlID="PnlMedicalConditions" ExpandControlID="PnlMedicalConditions"
                                        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgMedicalConditions">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlWHOStaging" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWHOStagingDetails"
                                        CollapseControlID="PnlWHOStaging" ExpandControlID="PnlWHOStaging" CollapsedImage="~/images/arrow-up.gif"
                                        Collapsed="true" ImageControlID="ImgWHOStaging">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlDiagnosis" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlDiagnosisDetails" CollapseControlID="PnlDiagnosis"
                                        ExpandControlID="PnlDiagnosis" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                        ImageControlID="ImgDiagnosis">
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
                                                            <%--<asp:Button ID="btnExaminationDQSave" runat="server" OnClick="btnExaminationDQSave_Click" Text="Data Quality Check" />--%>
                                                            <asp:Button ID="btncloseExam" Text="Close"  runat="server" OnClick="btncloseExam_Click"/>
                                                            <asp:Button ID="btnExaminationPrint" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                </ContentTemplate>
                            </act:TabPanel>
                            <act:TabPanel ID="TabPanel4" runat="server" Font-Size="Medium" HeaderText="Management">
                                <ContentTemplate>
                                    <div class="border center formbg">
                                        <br />
                                        <div class="center formbg pad5">
                                            <table class="border center formbg" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Panel ID="PnlAdherenceAssessment" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImgAdherenceAssessment" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <h2 align="left" class="forms">
                                                                            Adherence Assessment</h2>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Panel ID="PnlAdherenceAssessmentDetails" runat="server">
                                            <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="border center pad5 whitebg" colspan="2">
                                                            <table width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="right" style="width: 22%;">
                                                                            <label id="lblHave you missed any doses?-8888904" align="center">
                                                                                Have you missed any doses?:</label>
                                                                        </td>
                                                                        <td align="left" style="width: 28%;">
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
                                                                                            <td align="left" style="width: 40%;">
                                                                                                <label id="lblSpecify why doses missed?-88881305" align="center">
                                                                                                    Specify why doses missed?:</label>
                                                                                            </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                            <td align="left" style="width: 60%;">
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
                                                        <td class="border center pad5 whitebg" colspan="2">
                                                            <table width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="right" style="width: 50%;">
                                                                            <label id="lblHave you delayed in taking medication?-8888905" align="center">
                                                                                Have you delayed in taking medication?:</label>
                                                                        </td>
                                                                        <td align="left" style="width: 50%;">
                                                                            <asp:RadioButtonList ID="rdohavedelayed" runat="server" RepeatDirection="Horizontal">
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
                                                    <td align="left">
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
                                        <asp:Panel ID="PnlDrugAllergiesToxicitiesDetails" runat="server">
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
                                                    <td align="left">
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
                                        <asp:Panel ID="PnlARVsideeffectsDetails" runat="server">
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
                                                                            <asp:RadioButtonList ID="rdoARVsideeffects" runat="server" RepeatDirection="Horizontal"
                                                                                OnClick="rblSelectedValue(this,'divshorttermeffects');rblSelectedValue(this,'divLongtermeffects')">
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="border center pad5 whitebg" style="width: 50%;">
                                                            <%--<div id="divshorttermeffects" style="display: none;">--%>
                                                            <table width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <label id="lblShort term effects-8888862" align="center">
                                                                                Short term effects:</label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <div id="divcblShorttermeffects" enableviewstate="true" class="customdivbordermultiselect"
                                                                                runat="server">
                                                                                <asp:CheckBoxList ID="cblShorttermeffects" RepeatLayout="Flow" runat="server">
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
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
                                                                        <td align="left">
                                                                            <div id="divchklistlongtermeffect" enableviewstate="true" class="customdivbordermultiselect"
                                                                                runat="server">
                                                                                <asp:CheckBoxList ID="chklistlongtermeffect" RepeatLayout="Flow" runat="server">
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
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
                                                    <td align="left">
                                                        <asp:Panel ID="PnlCurrentARTRegimen" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="ImgCurrentARTRegimen" ImageUrl="~/images/arrow-up.gif" runat="server" />
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
                                        <asp:Panel ID="PnlCurrentARTRegimenDetails" runat="server">
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
                                                                                            <label id="Label3" class="">
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
                                    <act:CollapsiblePanelExtender ID="CPPnlCurrentARTRegimen" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlCurrentARTRegimenDetails"
                                        CollapseControlID="PnlCurrentARTRegimen" ExpandControlID="PnlCurrentARTRegimen"
                                        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgCurrentARTRegimen">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlAdherenceAssessment" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlAdherenceAssessmentDetails"
                                        CollapseControlID="PnlAdherenceAssessment" ExpandControlID="PnlAdherenceAssessment"
                                        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgAdherenceAssessment">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlDrugAllergiesToxicities" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlDrugAllergiesToxicitiesDetails"
                                        CollapseControlID="PnlDrugAllergiesToxicities" ExpandControlID="PnlDrugAllergiesToxicities"
                                        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgDrugAllergiesToxicities">
                                    </act:CollapsiblePanelExtender>
                                    <act:CollapsiblePanelExtender ID="CPPnlARVsideeffects" runat="server" SuppressPostBack="true"
                                        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlARVsideeffectsDetails"
                                        CollapseControlID="PnlARVsideeffects" ExpandControlID="PnlARVsideeffects" CollapsedImage="~/images/arrow-up.gif"
                                        Collapsed="true" ImageControlID="ImgARVsideeffects">
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
                                                            <%--<asp:Button ID="btnManagementDQSave" runat="server" Text="Data Quality Check" OnClick="btnManagementDQSave_Click" />--%>
                                                            <asp:Button ID="btncloseMgt" Text="Close"  runat="server" OnClick="btncloseMgt_Click"/>
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
                                    <td align="center" valign="top" colspan="2">
                                        <label for="VisitDate">
                                            Signature:</label>
                                        <asp:DropDownList ID="ddSignature" runat="server">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hidupdate" runat="server" />
                                        <asp:HiddenField ID="hidregimen" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
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
                                        <asp:Button ID="btnCancel" Text="Close" runat="server" OnClick="btnCancel_Click">
                                        </asp:Button>
                                        <asp:Button ID="btnPrint" Text="Print All" Visible="false" runat="server" OnClientClick="WindowPrintAll()" />--%>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            <%--</ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncomplete"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>--%>
        <%--<uc10:UserControlKNH_Extruder ID="UserControlKNH_Extruder1" runat="server" />--%>
        <uc13:UserControlKNH_BackToTop ID="UserControlKNH_BackToTop1" runat="server" />
    </div>
</asp:Content>
