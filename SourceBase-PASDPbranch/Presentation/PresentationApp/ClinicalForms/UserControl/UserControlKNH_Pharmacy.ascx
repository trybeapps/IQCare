<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_Pharmacy.ascx.cs" Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_Pharmacy" %>

<link href="../../Style/styles.css" rel="stylesheet" type="text/css" />
<link href="../../Style/calendar.css" rel="stylesheet" type="text/css" />
<link href="../../Style/_assets/css/grid.css" rel="stylesheet" type="text/css" />
<link href="../../Style/_assets/css/round.css" rel="stylesheet" type="text/css" />
<link href="../../Style/StyleSheet.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../Touch/Scripts/jquery-1.10.1.min.js"></script>
<script type="text/javascript" src="../../Touch/Styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>
<script type="text/javascript">

    function ShowHide(theDiv, YN) {

        $(document).ready(function () {

            if (YN == "show") {
                $("#" + theDiv).show();

            }
            if (YN == "hide") {
                $("#" + theDiv).hide();

            }

        });

    }
//    function fnSubsituations() {
//        var e = document.getElementById("=ddlSubstitutionInterruption.ClientID%>");
//        var strtext = e.options[e.selectedIndex].text;
//        if (strtext == "Change regimen") {
//            show('arvTherapyChange');
//            hide('arvTherapyStop');

//        }
//        else if (strtext == "Stop treatment") {
//            show('arvTherapyStop');
//            hide('arvTherapyChange');
//        }
//        else {
//            hide('arvTherapyChange');
//            hide('arvTherapyStop');
//        }
//    }
//    function fnRegimenChange() {
//        var e = document.getElementById("=ddlArvTherapyChangeCode.ClientID%>");
//        var strtext = e.options[e.selectedIndex].text;
//        if (strtext == "Other reason (specify)") {
//            show('otherarvTherapyChangeCode');

//        }
//        else {
//            hide('otherarvTherapyChangeCode');

//        }
//    }
//    function fnStopReason() {
//        var e = document.getElementById("=ddlArvTherapyStopCode.ClientID%>");
//        var strtext = e.options[e.selectedIndex].text;
//        if (strtext == "Other (specify)") {
//            show('otherarvTherapyStopCode');

//        }
//        else {
//            hide('otherarvTherapyStopCode');

//        }
//    }


    //followup
    function getSelectedtableValue(DivId, DDText, str, tableId) {
        var e = document.getElementById(DDText);
        var text = e.options[e.selectedIndex].innerHTML;
        var YN = "";
//        if (str == "Start new treatment (naive patient)" || str == "Change regimen" || str == "Switch to second line" || str == "Stop treatment") {
//            ShowHide(tableId, "show");
//        }
//        else {
//            ShowHide(tableId, "hide");
//        }

        if (text == str) {
            YN = "show";
        }
        else {
            YN = "hide";
        }
//        if (YN == "hide") {
//            hideChklistEligiblethrough();
//        }
        //ShowHide(tableId, "show");
        hideChklistEligiblethrough('<%=chklistEligiblethrough.ClientID %>');
        hideChklistEligiblethrough('<%=chklistARTchangecode.ClientID %>');
        hideChklistEligiblethrough('<%=chklistARTstopcode.ClientID %>');
        ShowHide(DivId, YN);
    }
    //Nidhi
    //Unselect all the selected list onselection changed of dropdownlist
    function hideChklistEligiblethrough(controlId) {
        var elementRef = document.getElementById(controlId);
        var inputElementArray = elementRef.getElementsByTagName('INPUT');
        for (var i = 0; i < inputElementArray.length; i++) {
            if (inputElementArray[i].type == 'checkbox')
                inputElementArray[i].checked = false; ;
        }
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

    
</script>
<style type="text/css">
    .style2
    {
        width: 15%;
    }
</style>
<div class="center formbg">
<table cellspacing="6" cellpadding="0" width="100%" border="0">
<tr>
    <td class="form" align="left">
        <label>
        Last regimen 
        dispensed:</label><asp:Label ID="lblLastRegimenDispensed" 
            runat="server" Font-Bold="True"></asp:Label>
    </td>
</tr>
<tr align="left">
    <td class="form" 
        align="left">
        <%--<label class="required margin10">
            *Substitutions/Interruptions:
        </label>
        <asp:DropDownList ID="ddlSubstitutionInterruption" onchange="fnSubsituations();"
            runat="server">
        </asp:DropDownList>
        <div id="arvTherapyChange" style="display: none">
            <label class="required margin10">
                *Change Regimen Reason:</label>
            <asp:DropDownList ID="ddlArvTherapyChangeCode" onchange="fnRegimenChange();" runat="server">
            </asp:DropDownList>
            <div id="otherarvTherapyChangeCode" style="display: none">
                <label class="required margin10" for="arvTherapyChangeCodeOtherName">
                    *Specify:</label>
                <input id="txtarvTherapyChangeCodeOtherName" maxlength="20" size="10" name="arvTherapyChangeCodeOtherName"
                    runat="server"/></div>
        </div>
        <div id="arvTherapyStop" style="display: none">
            <label id="lblrARTdate" class="required margin10">
                *Date ART Ended</label>
            <input id="txtARTEndeddate" runat="server" maxlength="11" size="10" name="txtARTEndeddate" />
            <img id="imgdate" onclick="w_displayDatePicker('<%=txtARTEndeddate.ClientID%>');"
                height="22" alt="Date Helper" hspace="3" src="../../images/cal_icon.gif" width="22"
                border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
            <br />
            <br />
            <label class="required margin10">
                *Stop Regimen Reason:</label>
            <asp:DropDownList ID="ddlArvTherapyStopCode" onchange="fnStopReason();" runat="server">
            </asp:DropDownList>
            <div id="otherarvTherapyStopCode" style="display: none">
                <label class="required margin10" for="arvTherapyStopCodeOtherName">
                    *Specify:</label>
                <input id="txtarvTherapyStopCodeOtherName" maxlength="20" size="10" name="arvTherapyStopCodeOtherName"
                    runat="server"/></div>
        </div>--%>
        <asp:Label ID="lblTreatmentplan" runat="server" CssClass="required" Font-Bold="True" 
                                                                                        Text="*Treatment plan:"></asp:Label>
        <asp:DropDownList ID="ddlTreatmentplan" 
            runat="server" onselectedindexchanged="ddlTreatmentplan_SelectedIndexChanged">
        </asp:DropDownList>
    </td>
</tr>
 <tr id="divEligiblethrough" style="display: none;">
    <td id="DIVTreatmentplan" class="border center pad5 whitebg" align="left">
        <div>
            <table width="100%">
                <tbody>
                    <tr>
                        <td align="left">
                            <label id="lblEligible through-8888551" align="center">
                                Eligible through :</label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div id="divchklistEligiblethrough" enableviewstate="true" class="customdivbordermultiselect"
                                runat="server">
                                <asp:CheckBoxList ID="chklistEligiblethrough" RepeatLayout="Flow" runat="server">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                        <div id="divOtherEligibility" style="display: none;">
                                <label id="Label2" align="center">
                                                    Specify other:</label>
                                                    <asp:TextBox ID="txtSpecifyOtherEligibility" 
                                    runat="server" Width="70%"></asp:TextBox>
                                
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        
        
        
    </td>
</tr>
<tr align="left" id="divARTchangecode" style="display: none;">
    <td class="form" >
    <div id="divNoofdrugssubstituted">
            <table width="100%">
                <tbody>
                    <tr>
                        <td align="left" width="20%">
                            <label id="lblNo of drugs substituted-8888482">
                                No of drugs substituted :</label>
                        </td>
                        <td align="left" width="80%">
                            <asp:TextBox ID="txtNoofdrugssubstituted" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div>
            <table width="100%">
                <tbody>
                    <tr>
                        <td align="left">
                            <label id="lblART change code-8888552" align="center">
                                ART change code :</label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div id="divchklistARTchangecode" enableviewstate="true" class="customdivbordermultiselect"
                                runat="server">
                                <asp:CheckBoxList ID="chklistARTchangecode" RepeatLayout="Flow" runat="server">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div id="divSpecifyotherARTchangereason" style="display: none;">
                                <label id="lblSpecify other ART change reason-8888873" align="center">
                                                    Specify other ART change reason:</label>
                                                    <asp:TextBox ID="txtSpecifyotherARTchangereason" 
                                    runat="server" Width="70%"></asp:TextBox>
                                
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </td>
</tr>
<tr align="left" id="divReasonforswitchto2ndlineregimen" style="display: none;">
    <td class="form">
    <div>
    <label id="lblReason for switch to 2nd line regimen-8888483" align="center">
                                Reason for switch to 2nd line regimen:</label>
                                <asp:DropDownList runat="server" 
            ID="ddlReasonforswitchto2ndlineregimen">
                            </asp:DropDownList>
        </div>
    </td>
</tr>
<tr align="left"  id="divARTstopcode" style="display: none;">
    <td class="form" >
    <div>
            <table width="100%">
                <tbody>
                    <tr>
                        <td align="left">
                            <label id="lblART stop code-8888553" align="center">
                                ART stop code :</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divchklistARTstopcode" enableviewstate="true" class="customdivbordermultiselect"
                                runat="server">
                                <asp:CheckBoxList ID="chklistARTstopcode" RepeatLayout="Flow" runat="server">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <div id="divARTstopcodeother" style="display: none;">
                                <label id="Label1" align="center">
                                                    Specify other:</label>
                                                    <asp:TextBox ID="txtSpecifyOtherStopCode" 
                                    runat="server" Width="70%"></asp:TextBox>
                                
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </td>
</tr>
<tr align="left">
    <td class="form" align="left">
        <asp:Button ID="btnPrescribeDrugs" runat="server" Text="Prescribe Regimen" 
            onclick="btnPrescribeDrugs_Click" />
    </td>
</tr>
</table>
</div>

