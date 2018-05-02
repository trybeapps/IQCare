<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_PwP.ascx.cs" Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_PwP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<%@ Register src="UserControlKNH_NextAppointment.ascx" tagname="UserControlKNH_NextAppointment" tagprefix="uc11" %>

<%@ Register src="UserControlKNH_Signature.ascx" tagname="UserControlKNH_Signature" tagprefix="uc1" %>


    <script type="text/javascript" language="javascript">
//        $(function () {

//            $("#tabs").tabs();

//        });
        function ShowHide(theDiv, YN, theFocus) {

            $(document).ready(function () {

                if (YN == "show") {
                    //                    $("#" + theDiv).slideDown();
                    $("#" + theDiv).show();

                }
                if (YN == "hide") {
                    //                    $("#" + theDiv).slideUp();
                    $("#" + theDiv).hide();


                }

            });

        }

        function ShowMore(sender, eventArgs) {
            var substr = eventArgs._commandArgument.toString().split('|')
            ShowHide(substr[0], substr[1]);
        }

        function rblSelectedValue(rbl, divID) {
            var selectedvalue = $("#" + rbl.id + " input:radio:checked").val();
            if (selectedvalue == "1") {
                YN = "show";
            }
            else {
                YN = "hide";
            }
            ShowHide(divID, YN);
        }

        function rblSelectedValueNo(rbl, divID) {
            var selectedvalue = $("#" + rbl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                YN = "show";
            }
            else {
                YN = "hide";
            }
            ShowHide(divID, YN);
        }
        //Checkbox list
        function getCheckBoxValue(DivId, chktext, str) {
            var YN = "";
            var id = "#" + CheckBoxID;
            if ($(id).is(':checked')) {
                YN = "show";
            }
            else {
                YN = "hide";
            }
            ShowHide(CheckBoxID, YN);
        }
        //DropDown list
        function getSelectedValue(DivId, DDText, str) {
            var e = document.getElementById(DDText);
            var text = e.options[e.selectedIndex].innerText;
            var YN = "";
            if (text == str) {
                YN = "show";
            }
            else {
                YN = "hide";
            }
            ShowHide(DivId, YN);
        }
        function CheckBoxToggleShowHideKNHAdultIE(divID, val, txt) {
            var checkList = document.getElementById(val);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();
            for (var i = 0; i < checkBoxList.length; i++) {

                if (checkBoxList[i].checked) {
                    if (arrayOfCheckBoxLabels[i].innerText.trim() == txt) {
                        ShowHide(divID, "show");
                        break;
                    }
                }
                else {
                    if (arrayOfCheckBoxLabels[i].innerText == txt) {
                        ShowHide(divID, "hide");
                    }

                }
            }

        }
        function WindowPrint() {
            window.print();
        }

        function CheckBoxHideUnhideOtherSpecialistClinic(strcblcontrolId, txtControlId) {
            
            var checkList = document.getElementById(strcblcontrolId);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();

            for (var i = 0; i < checkBoxList.length; i++) {

                if (checkBoxList[i].checked) {
                    if (arrayOfCheckBoxLabels[i].innerHTML == 'Other Specialist Clinic') {
                        //alert(arrayOfCheckBoxLabels[i].innerHTML + ' show ' + strdivId);
                        ShowHide('divReferToSpecialistClinic', "show");
                        return;
                    }
                    else {
                        //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                        document.getElementById(txtControlId).value = '';
                        ShowHide('divReferToSpecialistClinic', "hide");
                    }
                }
                else {
                    //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                    document.getElementById(txtControlId).value = '';
                    ShowHide('divReferToSpecialistClinic', "hide");
                }
            }
        }

        function CheckBoxHideUnhideOtherReferral(strcblcontrolId, txtControlId) {

            var checkList = document.getElementById(strcblcontrolId);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();

            for (var i = 0; i < checkBoxList.length; i++) {

                if (checkBoxList[i].checked) {
                    if (arrayOfCheckBoxLabels[i].innerHTML == 'Other (Specify)') {
                        //alert(arrayOfCheckBoxLabels[i].innerHTML + ' show ' + strdivId);
                        ShowHide('divReferToOther', "show");
                        return;
                    }
                    else {
                        //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                        document.getElementById(txtControlId).value = '';
                        ShowHide('divReferToOther', "hide");
                    }
                }
                else {
                    //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                    document.getElementById(txtControlId).value = '';
                    ShowHide('divReferToOther', "hide");
                }
            }
        }

        function CheckBoxHideUnhideOtherCounselling(strcblcontrolId, txtControlId) {

            var checkList = document.getElementById(strcblcontrolId);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();

            for (var i = 0; i < checkBoxList.length; i++) {

                if (checkBoxList[i].checked) {
                    if (arrayOfCheckBoxLabels[i].innerHTML == 'Other') {
                        //alert(arrayOfCheckBoxLabels[i].innerHTML + ' show ' + strdivId);
                        ShowHide('divOtherCounselling', "show");
                        return;
                    }
                    else {
                        document.getElementById(txtControlId).value = '';
                        ShowHide('divOtherCounselling', "hide");
                    }
                }
                else {
                    // alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                    document.getElementById(txtControlId).value = '';
                    ShowHide('divOtherCounselling', "hide");
                }
            }
        }

        function CheckBoxHideUnhideDivCounselling(strcblcontrolId, ControlId) {

            var checkList = document.getElementById(strcblcontrolId);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();

            for (var i = 0; i < checkBoxList.length; i++) {

                if (checkBoxList[i].checked) {
                    if (arrayOfCheckBoxLabels[i].innerHTML == 'Psychologist') {
                        //alert(arrayOfCheckBoxLabels[i].innerHTML + ' show ' + strdivId);
                        ShowHide('DivCounselling', "show");
                        return;
                    }
                    else {
                        ClearMultiSelect(ControlId);
                        ShowHide('DivCounselling', "hide");
                    }
                }
                else {
                    // alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                    ClearMultiSelect(ControlId);
                    ShowHide('DivCounselling', "hide");
                }
            }
        }

//        function ClearSelectList(controlID) {
//            document.getElementById(controlID).selectedIndex = 0;
//        }

//        function ClearTextBox(controlID) {
//            document.getElementById(controlID).value = "";
//        }

//        function ClearRadioButtons(RadioYes, RadioNo) {
//            document.getElementById(RadioYes).checked = false;
//            document.getElementById(RadioNo).checked = false;
//        }

//        function clearRadioButtonList(controlID) {

//            var elementRef = document.getElementById(controlID);
//            var inputElementArray = elementRef.getElementsByTagName('input');

//            for (var i = 0; i < inputElementArray.length; i++) {
//                var inputElement = inputElementArray[i];

//                inputElement.checked = false;
//            }
//            return false;
//        }
////        function ClearRadioButtonList(controlID) {
////            document.getElementById(controlID).selectedIndex = 0;
////        }

//        function ClearMultiSelect(controlID) {
//            var elementRef = document.getElementById(controlID);
//            var checkBoxArray = elementRef.getElementsByTagName('input');
//            for (var i = 0; i < checkBoxArray.length; i++) {
//                checkBoxArray[i].checked = false;
//            }
//        }
        function ClearCheckBox(controlID) {
            document.getElementById(controlID).checked = false;
        }

        function clearvaluesSexuallyActive(mainControl,controlID1, controlID2, controlID3, controlID4) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                ClearSelectList(controlID1);
                ClearMultiSelect(controlID2);
                clearRadioButtonList(controlID3);
                ClearSelectList(controlID4);
            }
        }

        function clearLMP(mainControl, controlID1, controlID2) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "1") {
                ClearSelectList(controlID1);
                
            }
            else if (selectedvalue == "0") {
                ClearTextBox(controlID2);
            }
        }

        function clearPTD(mainControl, controlID1, controlID2, controlID3) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                clearRadioButtonList(controlID1);
                clearRadioButtonList(controlID2);
                ClearTextBox(controlID3);
                ShowHide('trIfYesPregnant', 'hide');
            }

        }

        function clearClientPreg(mainControl, controlID1, controlID2) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                clearRadioButtonList(controlID1);
                ClearTextBox(controlID2);
            }

        }

        function clearPregInt(mainControl, controlID1, controlID2) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                clearRadioButtonList(controlID1);
            }
            else if (selectedvalue == "1") {
                clearRadioButtonList(controlID2);
            }

        }

        function clearCondomsIssued(mainControl, controlID1) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "1") {
                ClearTextBox(controlID1);
            }
        }

        function clearSTIScreened(mainControl, controlID1, controlID2, controlID3, controlID4, controlID5) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                ClearCheckBox(controlID1);
                ClearCheckBox(controlID2);
                ClearCheckBox(controlID3);
                ClearTextBox(controlID4);
                ClearTextBox(controlID5);
            }
        }

        function clearOnFP(mainControl, controlID1, controlID2) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                ClearSelectList(controlID1);
            }
            else if (selectedvalue == "1") {
                ClearSelectList(controlID2);
            }

        }

        function clearCervicalCancer(mainControl, controlID1, controlID2) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                ClearSelectList(controlID1);
            }
            else if (selectedvalue == "1") {
                clearRadioButtonList(controlID2);
            }

        }

        function clearHPV(mainControl, controlID1, controlID2) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                ClearSelectList(controlID1);
                ClearTextBox(controlID2);
            }

        }

        function clearPatientReferred(mainControl, controlID1, controlID2, controlID3) {
            var selectedvalue = $("#" + mainControl.id + " input:radio:checked").val();
            if (selectedvalue == "0") {
                ClearSelectList(controlID1);
                ClearTextBox(controlID2);
            }

        }

        function fnCheckUncheckPwP(strcblcontrolId) {
            var checkList = document.getElementById(strcblcontrolId);
            var checkBoxList = checkList.getElementsByTagName("input");
            var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
            var checkBoxSelectedItems = new Array();

            if (checkBoxList[0].checked == true && arrayOfCheckBoxLabels[0].innerHTML == 'None') {
                for (var i = 1; i < checkBoxList.length; i++) {
                    checkBoxList[i].checked = false;
                    checkBoxList[i].disabled = true;
                }
            }
            else {
                checkBoxList[0].checked = false;
                for (var i = 1; i < checkBoxList.length; i++) {
                    checkBoxList[i].disabled = false;
                }
            }
        }
    </script>

<div class="border center formbg">
    <table id="TabPrevWithPos" class="center formbg" cellspacing="6" cellpadding="0"
        width="100%" border="0">
        <tr>
            <td colspan="2" align="left">
                <asp:Panel ID="pnlSexAssessment" CssClass="border center formbg" runat="server" Style="padding: 6px">
                    <h2 class="forms" align="left">
                        <asp:ImageButton ID="imgSexAss" ImageUrl="~/images/arrow-up.gif" runat="server" />
                        <asp:Label ID="lblSexualityAssessment" runat="server" Text="Sexuality Assessment"></asp:Label>
                        
                    </h2>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlSexAssessmentDetail" runat="server">
        <table width="100%" class="formbg" cellspacing="6" cellpadding="0" border="0">
            <tr>
                <td colspan="2" class="border pad5 whitebg">
                    <table>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblSexuallyActiveLast6Months" runat="server" 
                                    Text="*Have you been sexually active in the past 6 months?:" Font-Bold="True" 
                                    CssClass="required"></asp:Label>
                                
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="radbtnSexualActiveness" runat="server" RepeatDirection="Horizontal"
                                    
                                    
                                    onselectedindexchanged="radbtnSexualActiveness_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="50%" class="border pad5 whitebg" id="DIVSexualOrientation" style="display: none;">
                    <div>
                        <table class="tbl-left">
                            <tr>
                                <td align="right" class="data-lable">
                                    <label>
                                        Sexual orientation:</label>
                                </td>
                                <td align="left" class="data-control">
                                    <asp:DropDownList ID="rcbSexualOrientation" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td width="50%" class="border pad5 whitebg" id="DIVSexualHighrisk" style="display: none;">
                    <div>
                        <table class="tbl-right">
                            <tr>
                                <td align="left" class="data-lable">
                                    <label>
                                        Sexuality high risk factor:</label>
                                    <div id="divHighRisk" class="customdivbordermultiselect">
                                        <asp:CheckBoxList ID="cblHighRisk" RepeatLayout="Flow" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="50%" class="border pad5 whitebg" id="DIVDiscloseSexualPartner" style="display: none;">
                    <div>
                        <table class="tbl-left" width="100%">
                            <tr>
                                <td align="right" width="75%">
                                    <asp:Label ID="lblDisclosedToSexualPartner" runat="server" 
                                        Text="*Have you disclosed your status to sexual partner:" Font-Bold="True" 
                                        CssClass="required"></asp:Label>
                                    
                                </td>
                                <td align="left" width="25%">
                                    <asp:RadioButtonList ID="radbtnDisclosedstatusToSexualPartner" runat="server" 
                                        RepeatDirection="Horizontal" 
                                        onselectedindexchanged="radbtnDisclosedstatusToSexualPartner_SelectedIndexChanged">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td width="50%" class="border pad5 whitebg" id="DIVStatusSexualPartner" style="display: none;">
                    <div>
                        <table class="tbl-right">
                            <tr>
                                <td align="right" class="data-lable">
                                    <asp:Label ID="lblStatusOfSexualPartner" runat="server" 
                                        Text="*HIV Status of sexual partner:" Font-Bold="True" CssClass="required"></asp:Label>
                                </td>
                                <td align="left" class="data-control">
                                    <asp:DropDownList ID="rcbPartnerHIVStatus" runat="server" Width="130px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
        <tr>
            <td colspan="2" align="left">
                <asp:Panel ID="pnlPWP" CssClass="border center formbg" runat="server" Style="padding: 6px">
                    <h2 class="forms" align="left">
                        <asp:ImageButton ID="imgPWP" ImageUrl="~/images/arrow-up.gif" runat="server" />
                        PWP Interventions</h2>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlPWPDetail" runat="server">
        <table width="100%" class="formbg" cellspacing="6" cellpadding="0" border="0">
            <tr>
                <td width="50%" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td align="right" style='width: 40%'>
                                <label>
                                    PWP messages given:</label>
                            </td>
                            <td align="left" style='width: 60%'>
                                <asp:RadioButtonList ID="radbtnGivenPWPMessages" runat="server" 
                                    RepeatDirection="Horizontal" 
                                    onselectedindexchanged="radbtnGivenPWPMessages_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td align="right" width="65%">
                                <label>
                                    Importance of safe sex explained:</label>
                            </td>
                            <td align="left" width="35%">
                                <asp:RadioButtonList ID="radbtnSaferSexImportanceExplained" runat="server" 
                                    RepeatDirection="Horizontal" 
                                    onselectedindexchanged="radbtnSaferSexImportanceExplained_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trLMP">
                <td colspan="2" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td style='width: 50%'>
                                <table width='100%'>
                                    <tr>
                                        <td style='width: 40%' align='right'>
                                            <label>
                                                LMP assessed:</label>
                                        </td>
                                        <td style='width: 60%' align='left'>
                                            <asp:RadioButtonList ID="radbtnLMP" runat="server" RepeatDirection="Horizontal" 
                                                 
                                                onselectedindexchanged="radbtnLMP_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style='width: 50%'>
                                <table id="DIVLMPDate" style="display: none" width="100%">
                                    <tr>
                                        <td align="right" width="50%">
                                            <label>
                                                LMP Date:</label>
                                        </td>
                                        <td align="left" width="50%">
                                            <input id="txtLMPDate"
                                                onfocus="javascript:vDateType='3'" maxlength="11" size="11" runat="server" type="text" />
                                            
                                            <img id="Img2" onclick="w_displayDatePicker('<%=txtLMPDate.ClientID%>');" height="22 "
                                                alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                name="appDateimg" style="vertical-align: text-bottom;" />
                                            <span class="smallerlabel" id="Span2">(DD-MMM-YYYY)</span>
                                        </td>
                                    </tr>
                                </table>
                                <table id="DIVMenopausal" style="display: none" width="100%">
                                    <tr>
                                        <td align="right" width="50%">
                                            <label>
                                                Reason LMP not assessed:</label>
                                        </td>
                                        <td align="left" width="50%">
                                            <asp:DropDownList ID="DDLReasonLMP" runat="server" Width="130px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPregnancyTest">
                <td colspan="2" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td style='width: 50%'>
                                <table width='100%'>
                                    <tr>
                                        <td style='width: 40%' align='right'>
                                            <label>
                                                Pregnancy test done:</label>
                                        </td>
                                        <td style='width: 60%' align='left'>
                                            <asp:RadioButtonList ID="radbtnPDTDone" runat="server" RepeatDirection="Horizontal"
                                                 
                                                onselectedindexchanged="radbtnPDTDone_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style='width: 50%'>
                                <table id="DIVClientPregnant" style="display: none" width="100%">
                                    <tr>
                                        <td align="right" style='width: 50%'>
                                            <label>
                                                If yes, client pregnant?:</label>
                                        </td>
                                        <td align="left" style='width: 50%'>
                                            <asp:RadioButtonList ID="rblClientPregnant" runat="server" RepeatDirection="Horizontal"
                                                 
                                                onselectedindexchanged="rblClientPregnant_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trIfYesPregnant" style="display: none">
                <td class="border pad5 whitebg" width="60%">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <label>
                                    If yes for pregnant, has patient been offered or referred to PMTCT?</label>
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="radbtnPMTCTOffered" runat="server" 
                                    RepeatDirection="Horizontal" 
                                    onselectedindexchanged="radbtnPMTCTOffered_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="border pad5 whitebg" width="40%">
                    <table>
                        <tr>
                            <td align="left">
                                <label>
                                    EDD:</label>
                            
                                <asp:TextBox runat="server" MaxLength="11" ID="txtEDD" Width="100px"></asp:TextBox>

                                <img alt="Date Helper" border="0" height="22" hspace="3" 
                                    onclick="w_displayDatePicker('<%= txtEDD.ClientID%>');" 
                                    src="../images/cal_icon.gif" width="22" />
                                        <span class="smallerlabel">(DD-MMM-YYYY)</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trIntendPreg">
                <td colspan="2" class="border pad5 whitebg">
                    <table>
                        <tr>
                            <td align="left">
                                <label>
                                    Patient or partner intend to become pregnant before next visit:</label>
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="radbtnIntentionOfPregnancy" runat="server" RepeatDirection="Horizontal"
                                    
                                     
                                    onselectedindexchanged="radbtnIntentionOfPregnancy_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td align="right">
                                <div id="DIVdisfertility" style="display: none;">
                                    <table>
                                        <tr>
                                            <td align="right">
                                                <label>
                                                    If yes, discussed fertility options:</label>
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList ID="radbtnDiscussedFertilityOptions" runat="server" 
                                                    RepeatDirection="Horizontal" 
                                                    onselectedindexchanged="radbtnDiscussedFertilityOptions_SelectedIndexChanged">
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="DIVDiscusdualcontraception" style="display: none;">
                                    <table class="tbl-right">
                                        <tr>
                                            <td align="right">
                                                <label>
                                                    If No discussed dual contraception:</label>
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList ID="radbtnDiscussedDualContraception" runat="server" 
                                                    RepeatDirection="Horizontal" 
                                                    onselectedindexchanged="radbtnDiscussedDualContraception_SelectedIndexChanged">
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
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
                <td colspan="2" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td style='width: 50%'>
                                <table width='100%'>
                                    <tr>
                                        <td style='width: 40%' align='right'>
                                            <label>
                                                Condoms issued:</label>
                                        </td>
                                        <td style='width: 60%' align='left'>
                                            <asp:RadioButtonList ID="radbtnCondomsIssued" runat="server" RepeatDirection="Horizontal"
                                                 
                                                onselectedindexchanged="radbtnCondomsIssued_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style='width: 50%'>
                                <table id="DIVnotICon" style="display: none">
                                    <tr>
                                        <td align="left">
                                            <label>
                                                Reasons condoms not issued:</label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtCondomNotIssued" runat="server" Skin="MetroTouch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td style='width: 50%'>
                                <table width='100%'>
                                    <tr>
                                        <td style='width: 40%' align='right'>
                                            <label>
                                                Screened for STI:</label>
                                        </td>
                                        <td style='width: 60%' align='left'>
                                            <asp:RadioButtonList ID="radbtnSTIScreened" runat="server" RepeatDirection="Horizontal"
                                                
                                                
                                                onselectedindexchanged="radbtnSTIScreened_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style='width: 50%'>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="DIVUrethral" style="display: none">
                <td width="50%" class="border pad5 whitebg">
                    <table class="tbl-right">
                        <tr>
                            <td align="right" class="data-lable">
                                <label>
                                    Urethral Discharge:</label>
                            </td>
                            <td align="left" class="data-control">
                                <asp:CheckBox ID="chkUrethralDischarge" runat="server" 
                                    oncheckedchanged="chkUrethralDischarge_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%" class="border pad5 whitebg">
                    <table class="tbl-left">
                        <tr>
                            <td align="right" class="data-lable">
                                <label>
                                    Vaginal Discharge:</label>
                            </td>
                            <td align="left" class="data-control">
                                <asp:CheckBox ID="chkVaginalDischarge" runat="server" 
                                    oncheckedchanged="chkVaginalDischarge_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="DIVGenUlceration" style="display: none">
                <td width="50%" class="border pad5 whitebg">
                    <table class="tbl-right">
                        <tr>
                            <td align="right" class="data-lable">
                                <label>
                                    Genital Ulceration:</label>
                            </td>
                            <td align="left" class="data-control">
                                <asp:CheckBox ID="chkGenitalUlceration" runat="server" 
                                    oncheckedchanged="chkGenitalUlceration_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%" class="border pad5 whitebg">
                    <table class="tbl-left" width="100%">
                        <tr>
                            <td align="left" class="data-lable">
                                <label>
                                    STI treatment plan:</label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="data-control">
                                <asp:TextBox ID="txtSTITreatmentPlan" runat="server" Skin="MetroTouch" Rows="3" 
                                    TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="STITreatmentOther" style="display: none">
            <td colspan="2" class="border pad5 whitebg">
            <table width="100%">
            <tr>
            <td align="left">
                <asp:Label ID="Label1" runat="server" Text="Other STI Treatment" 
                    Font-Bold="True"></asp:Label>
            </td>
            </tr>
            <tr>
            <td align="left">
                <asp:TextBox ID="txtOtherSTITreatment" runat="server" Rows="3" 
                    TextMode="MultiLine" Width="100%"></asp:TextBox>
            </td>
            </tr>
            </table>
            </td>
            </tr>
            <tr>
                <td colspan="2" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td align="right" width="30%">
                                <label>
                                    Are you on any family planning method?:</label>
                            </td>
                            <td align="left" width="20%">
                                <asp:RadioButtonList ID="radbtnOnFP" runat="server" RepeatDirection="Horizontal"
                                    
                                    
                                    onselectedindexchanged="radbtnOnFP_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <table id="DIVFPMethod" style="display: none;" width="100%">
                                    <tr>
                                        <td align="right" width="50%">
                                            <label>
                                                Specify other FP method :</label>
                                        </td>
                                        <td align="left" width="50%">
                                            <asp:DropDownList ID="rcbFPMethod" runat="server" Width="130px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table id="DIVReferredFP" style="display: none;" width="100%">
                                    <tr>
                                        <td align="right" width="50%">
                                            <label>
                                                If no, referred for family planning?:</label>
                                        </td>
                                        <td align="left" width="50%">
                                            <asp:DropDownList ID="ddlReferredFP" runat="server" Width="130px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trCervicalCancer">
                <td colspan="2" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td align="right" style='width: 35%'>
                                <label>
                                    Have you been screened for cervical cancer:</label>
                            </td>
                            <td align="left" style='width: 20%'>
                                <asp:RadioButtonList ID="radbtnCervicalCancerScreened" runat="server" RepeatDirection="Horizontal"
                                    
                                     
                                    onselectedindexchanged="radbtnCervicalCancerScreened_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <table id="DIVCacervix" style="display: none;" width="100%">
                                    <tr>
                                        <td align="right" style='width: 50%'>
                                            <label>
                                                CaCervix screening results:</label>
                                        </td>
                                        <td align="left" style='width: 50%'>
                                            <asp:DropDownList ID="rcbCervicalCancerScreeningResults" runat="server" Width="130px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table id="DIVRefCervCancer" style="display: none;" width="100%">
                                    <tr>
                                        <td align="right" style='width: 70%'>
                                            <label>
                                                If No, referred for cervical cancer screening:</label>
                                        </td>
                                        <td align="left" style='width: 30%'>
                                            <asp:RadioButtonList ID="radbtnReferredForCervicalCancerScreening" runat="server"
                                                RepeatDirection="Horizontal" 
                                                onselectedindexchanged="radbtnReferredForCervicalCancerScreening_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="border pad5 whitebg">
                    <table width="100%">
                        <tr>
                            <td style='width: 50%'>
                                <table width='100%'>
                                    <tr>
                                        <td style='width: 40%' align='right'>
                                            <label>
                                                HPV offered:</label>
                                        </td>
                                        <td style='width: 60%' align='left'>
                                            <asp:RadioButtonList ID="radbtnHPVOffered" runat="server" RepeatDirection="Horizontal"
                                                
                                                 
                                                onselectedindexchanged="radbtnHPVOffered_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style='width: 50%'>
                                <table id="DIVHPVVaccine" style="display: none" width="100%">
                                    <tr>
                                        <td style='width: 50%' align="right">
                                            <label>
                                                HPV Vaccine Offered:</label>
                                        </td>
                                        <td style='width: 50%' align="left">
                                            <asp:DropDownList ID="rcbOfferedHPVVaccine" runat="server" Width="130px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table id="DIVHPVVaccineDate" style="display: none" width="100%">
                                    <tr>
                                        <td style='width: 50%' align="right">
                                            <label>
                                                Date of HPV vaccine:</label>
                                        </td>
                                        <td style='width: 50%' align="left">
                                            <input id="dtHPVDoseDate" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                                                onfocus="javascript:vDateType='3'" maxlength="11" size="11" runat="server" type="text" />
                                            <img id="Img30" onclick="w_displayDatePicker('<%=dtHPVDoseDate.ClientID%>');" height="22 "
                                                alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                name="appDateimg" style="vertical-align: text-bottom;" />
                                            <span class="smallerlabel" id="Span30">(DD-MMM-YYYY)</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
            <td colspan="2" class="border pad5 whitebg">
            <table width="100%">
            <tr>
            <td align="left">
            <label>Transition preparation (for 10 - 19 years old):</label>
            </td>
            </tr>
            <tr>
            <td>
                <div class="customdivbordermultiselectAutoHeight" nowrap="noWrap">
                    <asp:CheckBoxList ID="cblTransitionPreparation" runat="server">
                    </asp:CheckBoxList>
                </div>
            </td>
            </tr>
            </table>
            </td>
            </tr>
        </table>
    </asp:Panel>

    <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
        <tr>
            <td colspan="2" align="left">
                <asp:Panel ID="pnlAppointmentsHeader" CssClass="border center formbg" runat="server" Style="padding: 6px">
                    <h2 class="forms" align="left">
                        <asp:ImageButton ID="imgAppointments" ImageUrl="~/images/arrow-up.gif" runat="server" />
                        <asp:Label ID="lblReferralAndAppointments" runat="server" 
                            Text="Referral and Appointments"></asp:Label>
                    </h2>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlAppointmentsBody" runat="server">
        <table width="100%" class="formbg" cellspacing="6" cellpadding="0" border="0">
            <tr>
            <td class="border pad5 whitebg">
                <table width="100%">
                <tr>
                <td align="right">
                    <label>
                    Ward Admission:</label>
                </td>
                <td>
                <asp:RadioButtonList ID="rblWardAdmission" runat="server" 
                        RepeatDirection="Horizontal" 
                        onselectedindexchanged="rblWardAdmission_SelectedIndexChanged">
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                </tr>
                </table>
             </td>
             </tr>
             <tr>
             <td class="border pad5 whitebg">
             <table width="100%">
             <tr>
             <td>
                <table width="100%">
                <tr>
                <td align="left">
                <label>Patient referred to:</label>
                </td>
                </tr>
                <tr>
                <td>
                    <div class="customdivbordermultiselect" nowrap="noWrap">
                    <asp:CheckBoxList ID="cblReferredTo" runat="server">
                    </asp:CheckBoxList>
                    </div>
                </td>
                </tr>
                    <tr id="divReferToSpecialistClinic" style="display: none">
                        <td align="left">
                            <label>
                            Refer to specialist clinic:</label><asp:TextBox 
                                ID="txtReferToSpecialistClinic" runat="server" Width="60%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="divReferToOther" style="display: none">
                        <td align="left">
                            <label>
                            Specify other referral point:<asp:TextBox ID="txtSpecifyOtherRefferedTo" 
                                runat="server" Skin="MetroTouch" Width="55%"></asp:TextBox>
                            </label></td>
                    </tr>
                </table>
             </td>
             <td valign="top" width="50%" id="DivCounselling" style="display: none">
                <table width="100%">
                <tr>
                <td align="left">
                <label>Counselling:</label>
                </td>
                </tr>
                <tr>
                <td>
                    <div class="customdivbordermultiselect" nowrap="noWrap">
                    <asp:CheckBoxList ID="cblCounselling" runat="server">
                    </asp:CheckBoxList>
                    </div>
                </td>
                </tr>
                    <tr id="divOtherCounselling" style="display: none">
                        <td align="left">
                           <label>Other:</label><asp:TextBox ID="txtOtherCounselling" runat="server" 
                                Width="70%"></asp:TextBox></td>
                    </tr>
                </table>
             </td>
             </tr>
             </table>
                
                
             </td>
            </tr>
            
        </table>
        <uc11:UserControlKNH_NextAppointment ID="UserControlKNH_NextAppointment1" 
                    runat="server" />
    </asp:Panel>
    
</div>
<act:CollapsiblePanelExtender ID="CPESexAssess" runat="server" SuppressPostBack="True"
    ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlSexAssessmentDetail"
    CollapseControlID="pnlSexAssessment" ExpandControlID="pnlSexAssessment" CollapsedImage="~/images/arrow-up.gif"
    Collapsed="True" ImageControlID="imgSexAss" Enabled="True">
</act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="CPEPWP" runat="server" SuppressPostBack="True"
    ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlPWPDetail" CollapseControlID="pnlPWP"
    ExpandControlID="pnlPWP" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
    ImageControlID="imgPWP" Enabled="True">
</act:CollapsiblePanelExtender>
<act:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" SuppressPostBack="True"
    ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlAppointmentsBody" CollapseControlID="pnlAppointmentsHeader"
    ExpandControlID="pnlAppointmentsHeader" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
    ImageControlID="imgAppointments" Enabled="True">
</act:CollapsiblePanelExtender>
<br />
<div class="border center formbg">
    <table cellspacing="6" cellpadding="0" width="100%" border="0">
        <tbody>
        <tr align="center">
        <td class="form">
        
            <uc1:UserControlKNH_Signature ID="UserControlKNH_SignaturePwP" runat="server" />
        
        </td>
        </tr>
            <tr align="center">
                <td class="form">
                    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
                    <asp:Button ID="btnSubmitPositive" runat="server" Text="Data Quality Check" 
                        Visible="False" />
                    <asp:Button ID="btnPwPClose" runat="server" onclick="Button1_Click" 
                        Text="Close" />
                    <asp:Button ID="btnPrintPositive" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
</div>
