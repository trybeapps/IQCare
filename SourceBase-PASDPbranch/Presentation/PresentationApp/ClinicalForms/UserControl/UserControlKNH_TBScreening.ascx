<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_TBScreening.ascx.cs"
    Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_TBScreening" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<%@ Register src="UserControlKNH_Signature.ascx" tagname="UserControlKNH_Signature" tagprefix="uc1" %>

<script src="../../Incl/IQCareScript.js" type="text/javascript"></script>
<script type="text/javascript">
    function show_hide(controlID, status) {
        var s = document.getElementById(controlID);
        if (status == "notvisible") {
            s.style.display = "none";
        }
        else {
            s.style.display = "block";
        }
    }

    function SelectOther(selectId, show_hide_control, otherControlID) {
        if (document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text == "Other") {
            show_hide(show_hide_control, 'visible');
        }
        else {
            document.getElementById(otherControlID).value = "";
            show_hide(show_hide_control, 'notvisible');
        }
    }
        

    function SelectOtherSpecify(selectId, show_hide_control, otherControlID) {
        if (document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text == "Other (Specify)") {
            show_hide(show_hide_control, 'visible');
        }
        else {
            document.getElementById(otherControlID).value = "";
            show_hide(show_hide_control, 'notvisible');
        }
    }

    function SelectCotrimoxazole(selectId, show_hide_control, otherControlID) {
        if (document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text == "Cotrimoxazole") {
            show_hide(show_hide_control, 'visible');
        }
        else {
            document.getElementById(otherControlID).selectedIndex = 0;
            show_hide(show_hide_control, 'notvisible');
        }
    }

//    function ClearSelectList(controlID) {
//    try{
//        document.getElementById(controlID).selectedIndex = 0;
//        }
//    catch (err) {
//    }
//    }

//    function ClearTextBox(controlID) {
//    try{
//        document.getElementById(controlID).value = "";
//        }
//    catch (err) {
//    }
//    }

//    function ClearRadioButtons(RadioYes, RadioNo) {
//    try{
//        document.getElementById(RadioYes).checked = false;
//        document.getElementById(RadioNo).checked = false;
//        }
//        catch(err)
//        {
//        }
//    }

//    function ClearMultiSelect(controlID) {
//    try{
//        var elementRef = document.getElementById(controlID);
//        var checkBoxArray = elementRef.getElementsByTagName('input');
//        for (var i = 0; i < checkBoxArray.length; i++) {
//            checkBoxArray[i].checked = false;
//        }
//        }
//    catch (err) {
//    }
//    }

//    function clearRadioButtonList(controlID) {
//    try{
//        var elementRef = document.getElementById(controlID);
//        var inputElementArray = elementRef.getElementsByTagName('input');

//        for (var i = 0; i < inputElementArray.length; i++) {
//            var inputElement = inputElementArray[i];

//            inputElement.checked = false;
//        }
//        return false;
//        }
//    catch (err) {
//    }
//    }

    function SelectOtherReviewChkList(controlID, show_hide_control, otherControlID) {
        debugger;
        var elementRef = document.getElementById(controlID);
        var checkBoxArray = elementRef.getElementsByTagName('input');
        var checkedValues = '';
        var checkedValues1 = '';

        for (var i = 0; i < checkBoxArray.length; i++) {
            var checkBoxRef = checkBoxArray[i];

            if (checkBoxRef.checked == true) {
                var labelArray = checkBoxRef.parentNode.getElementsByTagName('label');

                if (labelArray.length > 0) {
                    checkedValues = labelArray[0].innerHTML;
                    //alert(checkedValues);
                    if (checkedValues == "Other Side effects (specify)") {
                        show_hide(show_hide_control, 'visible');
                    }
                    else {
                        //document.getElementById(otherControlID).value = "";
                        //show_hide(show_hide_control, 'notvisible');
                    }
                }
            }
            else {
                var labelArray1 = checkBoxRef.parentNode.getElementsByTagName('label');

                if (labelArray1.length > 0) {
                    checkedValues1 = labelArray1[0].innerHTML;
                    //alert(checkedValues1);
                    if (checkedValues1 == "Other Side effects (specify)") {
                        //document.getElementById(otherControlID).value = '';
                        //document.getElementsByName(otherControlID)[0].value = '';
                        ClearTextBox(otherControlID);
                        show_hide(show_hide_control, 'notvisible');
                    }
                }
                    
            }
        }
    }

    function SignsOfHepatitisReviewChkList(controlID, show_hide_control, otherControlID) {
        var elementRef = document.getElementById(controlID);
        var checkBoxArray = elementRef.getElementsByTagName('input');
        var checkedValues = '';
        var checkedValues1 = '';

        for (var i = 0; i < checkBoxArray.length; i++) {
            var checkBoxRef = checkBoxArray[i];

            if (checkBoxRef.checked == true) {
                var labelArray = checkBoxRef.parentNode.getElementsByTagName('label');

                if (labelArray.length > 0) {
                    checkedValues = labelArray[0].innerHTML;
                    if (checkedValues == "Signs of Hepatitis") {
                        show_hide(show_hide_control, 'visible');
                    }
                    else {
                        //document.getElementById(otherControlID).value = "";
                        //show_hide(show_hide_control, 'notvisible');
                    }
                }
            }
            else {
                var labelArray1 = checkBoxRef.parentNode.getElementsByTagName('label');

                if (labelArray1.length > 0) {
                    checkedValues1 = labelArray1[0].innerHTML;
                    //alert(checkedValues1);
                    if (checkedValues1 == "Signs of Hepatitis") {
                        //document.getElementById(otherControlID).value = '';
                        //document.getElementsByName(otherControlID)[0].value = '';
                        ClearMultiSelect(otherControlID);
                        show_hide(show_hide_control, 'notvisible');
                    }
                }

            }
        }
    }

    function SelectTBFindings(selectId, RadioYes, RadioNo) {
        var valueSelected = document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text;
        if (valueSelected == 'TB Confirmed' || valueSelected == 'On Treatment') {
            document.getElementById(RadioYes).checked = true;
            document.getElementById(RadioNo).checked = false;
            show_hide('TBAvailableResults', 'visible');
            document.getElementById(RadioYes).disabled = true;
            document.getElementById(RadioNo).disabled = true;
        }
        else if (valueSelected == 'No signs or symptoms') {
            document.getElementById(RadioYes).checked = false;
            document.getElementById(RadioNo).checked = true;
            show_hide('TBAvailableResults', 'notvisible');
            document.getElementById(RadioYes).disabled = true;
            document.getElementById(RadioNo).disabled = true;
        }
        else {
            document.getElementById(RadioYes).disabled = false;
            document.getElementById(RadioNo).disabled = false;
        }


    }

    function SelectTBFindings(selectId, selSmear, dtSmear, selGene, dtGene, selDST, dtDST, rdoXrayYes, rdoXrayNo, dtxray, selCXR, txtCXR, rdoBiopsyYes, rdoBiopsyNo,
    dtBiopsy, selTBClassification, selPtnClassification, selTBPlan, txtTBPlan, selTBReg, txtTBReg, txtTBStart, txtTBEnd, selTBTreat, rdoScreenedTBYes, rdoScreenedTBNo,
    txtSpecifyWhy, selFacilityReferred, rdoIPT, dtINHStart, dtINHEnd, dtPyStart, dtPyEnd, rdoAdheAddYes, rdoAdheAddNo, rdoMisseddoseYes, rdoMisseddoseNo,
    rdoRefAdheYes, rdoRefAdheNo, cblRevChklst,cblhepatitis,txtOthersideeff) {
        var valueSelected = document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text;
        if (valueSelected == 'TB Confirmed') {
            show_hide('AvailableTBResultsBodyDiv', 'visible');
            show_hide('divContactsScreenedforTB', 'visible');
            show_hide('divIPTBody', 'notvisible');

            clearRadioButtonList(rdoIPT); ClearTextBox(dtINHStart); ClearTextBox(dtINHEnd); ClearTextBox(dtPyStart); ClearTextBox(dtPyEnd);
            ClearRadioButtons(rdoAdheAddYes, rdoAdheAddNo); ClearRadioButtons(rdoMisseddoseYes, rdoMisseddoseNo); ClearRadioButtons(rdoRefAdheYes, rdoRefAdheNo);
            ClearMultiSelect(cblRevChklst); ClearMultiSelect(cblhepatitis); ClearTextBox(txtOthersideeff);
        }
        else if (valueSelected == 'On Treatment') {
            show_hide('AvailableTBResultsBodyDiv', 'visible');
            
            show_hide('divContactsScreenedforTB', 'notvisible');
            show_hide('divIPTBody', 'notvisible');
            ClearRadioButtons(rdoScreenedTBYes, rdoScreenedTBNo); ClearTextBox(txtSpecifyWhy); ClearSelectList(selFacilityReferred);

            clearRadioButtonList(rdoIPT); ClearTextBox(dtINHStart); ClearTextBox(dtINHEnd); ClearTextBox(dtPyStart); ClearTextBox(dtPyEnd);
            ClearRadioButtons(rdoAdheAddYes, rdoAdheAddNo); ClearRadioButtons(rdoMisseddoseYes, rdoMisseddoseNo); ClearRadioButtons(rdoRefAdheYes, rdoRefAdheNo);
            ClearMultiSelect(cblRevChklst); ClearMultiSelect(cblhepatitis); ClearTextBox(txtOthersideeff);
        }
        else if (valueSelected == 'No signs or symptoms') {
            show_hide('AvailableTBResultsBodyDiv', 'notvisible');
            
            show_hide('divContactsScreenedforTB', 'notvisible');
            show_hide('divIPTBody', 'visible');
            ClearRadioButtons(rdoScreenedTBYes, rdoScreenedTBNo); ClearTextBox(txtSpecifyWhy); ClearSelectList(selFacilityReferred);
            
            ClearSelectList(selSmear); ClearTextBox(dtSmear);ClearSelectList(selGene); ClearTextBox(dtGene); ClearSelectList(selDST); ClearTextBox(dtDST);
            ClearRadioButtons(rdoXrayYes, rdoXrayNo);
            ClearTextBox(dtxray);ClearSelectList(selCXR);ClearTextBox(txtCXR);
            ClearRadioButtons(rdoBiopsyYes, rdoBiopsyNo);
            ClearTextBox(dtBiopsy);ClearSelectList(selTBClassification); ClearSelectList(selPtnClassification);ClearSelectList(selTBPlan);
            ClearTextBox(txtTBPlan);ClearSelectList(selTBReg);ClearTextBox(txtTBReg); ClearTextBox(txtTBStart); ClearTextBox(txtTBEnd);  ClearSelectList(selTBTreat);
        }
        else{
            show_hide('AvailableTBResultsBodyDiv', 'visible');
            show_hide('divContactsScreenedforTB', 'notvisible');
            show_hide('divIPTBody', 'notvisible');
            ClearRadioButtons(rdoScreenedTBYes, rdoScreenedTBNo); ClearTextBox(txtSpecifyWhy); ClearSelectList(selFacilityReferred);

            clearRadioButtonList(rdoIPT); 
            ClearTextBox(dtINHStart); ClearTextBox(dtINHEnd); ClearTextBox(dtPyStart); ClearTextBox(dtPyEnd);
            ClearRadioButtons(rdoAdheAddYes, rdoAdheAddNo); ClearRadioButtons(rdoMisseddoseYes, rdoMisseddoseNo); ClearRadioButtons(rdoRefAdheYes, rdoRefAdheNo);
            ClearMultiSelect(cblRevChklst); ClearMultiSelect(cblhepatitis); ClearTextBox(txtOthersideeff);
        }


    }



    function SelectIPT(selectId, controlID1, controlID2, controlID3, controlID4) {
        var list = document.getElementById(selectId);
        var inputs = list.getElementsByTagName("input");
        var valueSelected;
        for (var i = 0; i < inputs.length; i++) {
            if (inputs[i].checked) {
                var selector = 'label[for=' + inputs[i].id + ']';
                var label = document.querySelector(selector);
                valueSelected = label.innerHTML;

                //valueSelected = inputs[i];
                break;
            }
        }

        if (valueSelected == 'Start IPT' || valueSelected == 'Continue IPT') {
            show_hide('INHStartEndDates', 'visible');
            show_hide('PyridoxineStartEnd', 'visible');
            //show_hide('ifYesStopReason', 'notvisible');

        }
        //        else if (valueSelected == 'Stop IPT') {
        //            
        //            show_hide('ifYesStopReason', 'visible');
        //            show_hide('INHStartEndDates', 'visible');
        //            show_hide('PyridoxineStartEnd', 'visible');
        //            
        //        }
        else if (valueSelected == 'Completed IPT') {
            //show_hide('ifYesStopReason', 'notvisible');
            show_hide('INHStartEndDates', 'visible');
            show_hide('PyridoxineStartEnd', 'visible');
        }
        else {
            //show_hide('ifYesStopReason', 'notvisible');
            ClearTextBox(controlID1); ClearTextBox(controlID2); ClearTextBox(controlID3); ClearTextBox(controlID4);
            show_hide('INHStartEndDates', 'notvisible');
            show_hide('PyridoxineStartEnd', 'notvisible');
        }


    }

    function WindowPrint() {
        window.print();
    }

</script>
<link href="../../Style/styles.css" rel="stylesheet" type="text/css" />
<link href="../../Style/calendar.css" rel="stylesheet" type="text/css" />
<link href="../../Style/_assets/css/grid.css" rel="stylesheet" type="text/css" />
<link href="../../Style/_assets/css/round.css" rel="stylesheet" type="text/css" />
<link href="../../Style/StyleSheet.css" rel="stylesheet" type="text/css" />
<%--<div class="border center formbg">
    <br />--%>
    <style type="text/css">
        .style2
        {
            height: 27px;
        }
    </style>
    <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="TBAssessmentHeader" runat="server" Style="padding: 6px" CssClass="border">
                    <h2 class="forms" align="left">
                        &nbsp;<asp:ImageButton ID="imgTBAssessment" runat="server" ImageUrl="~/images/arrow-up.gif" />
                        &nbsp;<asp:Literal ID="literTBAssessment" Text="TB Assessment" runat="server"></asp:Literal>
                        <%--<asp:Label ID="lblTBAssessment" runat="server" Text="TB Assessment"></asp:Label>--%>
                    </h2>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="TBAssessmentBody" runat="server">
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
            <tr>
                <td class="form" align="left" style="width: 50%">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <label>
                                    TB assessment (ICF):</label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div class="customdivbordermultiselect" runat="server">
                                    <asp:CheckBoxList ID="cblTBAssessmentICF" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="form" align="center" width="50%">
                    <asp:Label ID="lblTBFindings" runat="server" CssClass="required" Font-Bold="True"
                        Text="*TB findings:"></asp:Label>
                    <asp:DropDownList ID="ddlTBFindings" runat="server" OnSelectedIndexChanged="ddlTBFindings_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    <div id="divContactsScreenedforTB" style="display: block">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <label>
                                        Contacts screened for TB?</label>
                                    <asp:RadioButton ID="rdoContactsScreenedForTBYes" runat="server" Text="Yes" GroupName="rdoContactsScreenedForTB"
                                        OnCheckedChanged="rdoContactsScreenedForTBYes_CheckedChanged" />
                                    <asp:RadioButton ID="rdoContactsScreenedForTBNo" runat="server" Text="No" GroupName="rdoContactsScreenedForTB"
                                        OnCheckedChanged="rdoContactsScreenedForTBNo_CheckedChanged" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td id="IfNoContactsScreenedSpecifyWhy" style="display: none" align="center" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" valign="middle" width="40%">
                                                <asp:Label ID="Label1" runat="server" Text="If no, specify why:" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td align="left" width="60%">
                                                <asp:TextBox ID="txtSpecifyWhyContactNotScreenedForTB" runat="server" Width="100%"
                                                    Columns="3" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            <table width="100%">
                            <tr>
                            <td>
                            <asp:Label ID="Label2" runat="server" 
                                    Text="Facility patient referred for treatment:" Font-Bold="True"></asp:Label>
                                
                            </td>
                            </tr>
                                <tr>
                                    <td>
                                    <asp:DropDownList ID="ddlPatientReferredForTreatment" runat="server">
                                </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            
                            </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" SuppressPostBack="true"
        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="TBAssessmentBody" CollapseControlID="TBAssessmentHeader"
        ExpandControlID="TBAssessmentHeader" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
        ImageControlID="imgTBAssessment">
    </act:CollapsiblePanelExtender>
    <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="AvailableTBResultsHeader" runat="server" Style="padding: 6px" CssClass="border">
                    <h2 class="forms" align="left">
                        <asp:ImageButton ID="imgAvailableTBResults" runat="server" ImageUrl="~/images/arrow-up.gif" />
                        <asp:Label ID="lblAvailableTBResults" runat="server" Text="Available TB Results"></asp:Label>
                        <asp:RadioButton ID="rdoAvailableTBResultsYes" runat="server" Text="Yes" GroupName="rdoAvailableTBResults"
                            OnCheckedChanged="rdoAvailableTBResultsYes_CheckedChanged" Visible="False" />
                        <asp:RadioButton ID="rdoAvailableTBResultsNo" runat="server" Text="No" GroupName="rdoAvailableTBResults"
                            OnCheckedChanged="rdoAvailableTBResultsNo_CheckedChanged" Visible="False" /></h2>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <div id="AvailableTBResultsBodyDiv">
        <asp:Panel ID="AvailableTBResultsBody" runat="server">
            <table id="TBAvailableResults" cellspacing="6" cellpadding="0" width="100%" border="0">
                <tr id="sputumSmear">
                    <td class="form" align="center" style="width: 100%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="20%" align="left">
                                    <label>
                                        Sputum smear:</label>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="40%">
                                                <label>
                                                    Result:</label>&nbsp;
                                            </td>
                                            <td align="left" width="60%">
                                                <asp:DropDownList ID="ddlSputumSmear" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="20%">
                                                <label>
                                                    &nbsp;Date:
                                                </label>
                                            </td>
                                            <td align="left" width="80%">
                                                <label>
                                                    <asp:TextBox ID="txtSputumSmearDate" runat="server" MaxLength="11"></asp:TextBox>
                                                    <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtSputumSmearDate.ClientID%>');"
                                                        src="../Images/cal_icon.gif" width="22" />
                                                    <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr1">
                    <td class="form" align="center" style="width: 100%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="20%" align="left">
                                    <label>
                                        GeneExpert:</label>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="40%">
                                                <label>
                                                    Result:</label>&nbsp;
                                            </td>
                                            <td align="left" width="60%">
                                                <asp:DropDownList ID="ddlGeneExpert" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="20%">
                                                <label>
                                                    &nbsp;Date:
                                                </label>
                                            </td>
                                            <td align="left" width="80%">
                                                <label>
                                                    <asp:TextBox ID="txtGeneExpertDate" runat="server" MaxLength="11"></asp:TextBox>
                                                    <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtGeneExpertDate.ClientID%>');"
                                                        src="../Images/cal_icon.gif" width="22" />
                                                    <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr2">
                    <td class="form" align="center" style="width: 100%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="20%" align="left">
                                    <label>
                                        Sputum for DST:</label>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="40%">
                                                <label>
                                                    Result:</label>&nbsp;
                                            </td>
                                            <td align="left" width="60%">
                                                <asp:DropDownList ID="ddlSputumDST" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="20%">
                                                <label>
                                                    Date:
                                                </label>
                                            </td>
                                            <td align="left" width="80%">
                                                <label>
                                                    <asp:TextBox ID="txtSputumDSTDate" runat="server" MaxLength="11"></asp:TextBox>
                                                    <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtSputumDSTDate.ClientID%>');"
                                                        src="../Images/cal_icon.gif" width="22" />
                                                    <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="chestXRay">
                    <td class="form" align="center" style="width: 100%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="20%" align="left">
                                    <label>
                                        Chest X-Ray:</label>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="40%">
                                                <label>
                                                    Result:</label>
                                            </td>
                                            <td align="left" width="60%">
                                                <asp:RadioButton ID="rdoChestXrayYes" runat="server" GroupName="rdoChestXray" Text="Yes"
                                                    OnCheckedChanged="rdoChestXrayYes_CheckedChanged" />
                                                <asp:RadioButton ID="rdoChestXrayNo" runat="server" GroupName="rdoChestXray" Text="No"
                                                    OnCheckedChanged="rdoChestXrayNo_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="20%">
                                                <label>
                                                    Date:
                                                </label>
                                            </td>
                                            <td align="left" width="80%">
                                                <label>
                                                    <asp:TextBox ID="txtChestXrayDate" runat="server" MaxLength="11"></asp:TextBox>
                                                    <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtChestXrayDate.ClientID%>');"
                                                        src="../Images/cal_icon.gif" width="22" />
                                                    <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="CXRResults" style="display: none">
                    <td align="center" class="form" colspan="2" width="50%">
                        <table width="100%" style="table-layout: fixed">
                            <tr>
                                <td width="50%" align="left">
                                    <label>
                                        CXR results :</label>
                                    <asp:DropDownList ID="ddlCXRResults" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td id="OtherCRXSpecify" style="display: none" width="100%">
                                    <label>
                                        Other CXR (Specify):</label>
                                    <asp:TextBox ID="txtOtherCXRResults" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="tissueBiopsy">
                    <td class="form" align="center" style="width: 100%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="20%" align="left">
                                    <label>
                                        Tissue Biopsy:</label>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="40%">
                                                <label>
                                                    Result:</label>&nbsp;
                                            </td>
                                            <td align="left" width="60%">
                                                <asp:RadioButton ID="rdoTissueBiopsyYes" runat="server" GroupName="rdoTissueBiopsy"
                                                    Text="Yes" OnCheckedChanged="rdoTissueBiopsyYes_CheckedChanged" />
                                                <asp:RadioButton ID="rdoTissueBiopsyNo" runat="server" GroupName="rdoTissueBiopsy"
                                                    Text="No" OnCheckedChanged="rdoTissueBiopsyNo_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="20%">
                                                <label>
                                                    Date:
                                                </label>
                                            </td>
                                            <td align="left" width="80%">
                                                <label>
                                                    <asp:TextBox ID="txtTissueBiopsyDate" runat="server" MaxLength="11"></asp:TextBox>
                                                    <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtTissueBiopsyDate.ClientID%>');"
                                                        src="../Images/cal_icon.gif" width="22" />
                                                    <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                <tr>
                    <td align="center" class="form" style="width: 50%">
                        <table width="100%">
                            <tr>
                                <td align="right" width="40%">
                                    <label>
                                        TB classification:</label>&nbsp;
                                </td>
                                <td align="left" width="60%">
                                    <asp:DropDownList ID="ddlTBClassification" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" class="form" width="50%">
                        <table width="100%">
                            <tr>
                                <td align="right" width="40%">
                                    <label>
                                        Patient classification:</label>&nbsp;
                                </td>
                                <td align="left" width="60%">
                                    <asp:DropDownList ID="ddlPatientClassification" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="form" align="center" width="50%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="50%" align="left">
                                    <label>
                                        TB plan :</label><asp:DropDownList ID="ddlTBPLan" runat="server">
                                        </asp:DropDownList>
                                </td>
                                <td id="OtherTBPlanSpecify" style="display: none" width="100%">
                                    <label>
                                        Specify other TB plan:</label><asp:TextBox ID="txtOtherTBPlan" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="form" width="50%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="50%" align="left" class="style2">
                                    <label>
                                        TB regimen :</label><asp:DropDownList ID="ddlTBRegimen" runat="server">
                                        </asp:DropDownList>
                                </td>
                                <td id="OtherTBRegimenSpecify" style="display: none" width="100%" 
                                    class="style2">
                                    <label>
                                        Other TB regimen:</label><asp:TextBox ID="txtOtherTBRegimen" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="form" style="width: 50%" colspan="2">
                        <table width="100%">
                            <tr>
                                <td width="15%" align="left">
                                    <label>
                                        TB Regimen :&nbsp;
                                    </label>
                                </td>
                                <td width="45%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="20%">
                                                <label>
                                                    Start date:&nbsp;
                                                </label>
                                            </td>
                                            <td align="left" width="80%">
                                                <label>
                                                    <asp:TextBox ID="txtTBRegimenStartDate" runat="server" MaxLength="11" 
                                                    ontextchanged="txtTBRegimenStartDate_TextChanged"></asp:TextBox>
                                                    <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtTBRegimenStartDate.ClientID%>');"
                                                        src="../Images/cal_icon.gif" width="22" />
                                                    <span class="smallerlabel">(DD-MMM-YYYY)</span></label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="40%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" width="20%">
                                                <label>
                                                    End date:
                                                </label>
                                            </td>
                                            <td align="left" width="80%">
                                                <label>
                                                    <asp:TextBox ID="txtTBRegimenEndDate" runat="server" MaxLength="11"></asp:TextBox>
                                                    <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtTBRegimenEndDate.ClientID%>');"
                                                        src="../Images/cal_icon.gif" width="22" />
                                                    <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="form" colspan="2">
                        <table width="100%">
                            <tr>
                                <td align="left" width="50%">
                                    <label>
                                        TB treatment outcome:</label>
                                    <asp:DropDownList ID="ddlTBTreatment" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td id="specifyOtheroutcome" align="left" style="display: none" width="50%">
                                    <label>
                                        Specify other:</label>
                                    <asp:TextBox ID="txtOtherTreatmentOutcome" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" SuppressPostBack="true"
        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="AvailableTBResultsBody"
        CollapseControlID="AvailableTBResultsHeader" ExpandControlID="AvailableTBResultsHeader"
        CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="imgAvailableTBResults">
    </act:CollapsiblePanelExtender>
    <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="IPTHeader" runat="server" Style="padding: 6px" CssClass="border">
                    <h2 class="forms" align="left">
                        <asp:ImageButton ID="imgIPT" runat="server" ImageUrl="~/images/arrow-up.gif" />
                        &nbsp;IPT (Patients with No Signs and Symptoms)</h2>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <div id="divIPTBody">
    <asp:Panel ID="IPTBody" runat="server">
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
            <tr>
                <td align="center" class="form">
                    <asp:RadioButtonList ID="rdoLstIPT" runat="server" RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td id="INHStartEndDates" style="display: none" class="form">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="15%" align="left">
                                <label>
                                    INH :</label>
                            </td>
                            <td align="center" width="45%">
                                <table width="100%">
                                    <tr>
                                        <td align="right" width="25%">
                                            <label>
                                                Start date:
                                            </label>
                                        </td>
                                        <td align="left" width="75%">
                                            <label>
                                                <asp:TextBox ID="txtINHStartDate" runat="server" MaxLength="11"></asp:TextBox>
                                                <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtINHStartDate.ClientID%>');"
                                                    src="../Images/cal_icon.gif" width="22" />
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="center" width="40%">
                                <table width="100%">
                                    <tr>
                                        <td align="right" width="20%">
                                            <label>
                                                End date:
                                            </label>
                                        </td>
                                        <td align="left" width="80%">
                                            <label>
                                                <asp:TextBox ID="txtINHEndDate" runat="server" MaxLength="11"></asp:TextBox>
                                                <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtINHEndDate.ClientID%>');"
                                                    src="../Images/cal_icon.gif" width="22" />
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="PyridoxineStartEnd" style="display: none" class="form">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td width="15%" align="left">
                                <label>
                                    Pyridoxine :</label>
                            </td>
                            <td align="center" style="width: 45%">
                                <table width="100%">
                                    <tr>
                                        <td align="right" width="25%">
                                            <label>
                                                Start date:
                                            </label>
                                        </td>
                                        <td align="left" width="75%">
                                            <label>
                                                <asp:TextBox ID="txtPyridoxineStartDate" runat="server" MaxLength="11"></asp:TextBox>
                                                <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtPyridoxineStartDate.ClientID%>');"
                                                    src="../Images/cal_icon.gif" width="22" />
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td align="right" width="20%">
                                            <label>
                                                End date:
                                            </label>
                                        </td>
                                        <td align="left" width="80%">
                                            <label>
                                                <asp:TextBox ID="txtPyridoxineEndDate" runat="server" MaxLength="11"></asp:TextBox>
                                                <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtPyridoxineEndDate.ClientID%>');"
                                                    src="../Images/cal_icon.gif" width="22" />
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="form" align="center" style="width: 50%;">
                                <table width="100%">
                                    <tr>
                                        <td align="right" width="60%">
                                            <label>
                                                Has adherence been addressed?</label>
                                        </td>
                                        <td align="left" width="40%">
                                            <asp:RadioButton ID="rdoAdherenceBeenAddressedYes" runat="server" GroupName="rdoAdherenceBeenAddressed"
                                                Text="Yes" OnCheckedChanged="rdoAdherenceBeenAddressedYes_CheckedChanged" />
                                            <asp:RadioButton ID="rdoAdherenceBeenAddressedNo" runat="server" GroupName="rdoAdherenceBeenAddressed"
                                                Text="No" OnCheckedChanged="rdoAdherenceBeenAddressedNo_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="6px">
                            </td>
                            <td class="form" align="center">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" width="50%">
                                                        <label>
                                                            Any missed doses?</label>
                                                    </td>
                                                    <td align="left" width="60%">
                                                        <asp:RadioButton ID="rdoMissedAnyTBDosesYes" runat="server" GroupName="rdoMissedAnyTBDoses"
                                                            Text="Yes" OnCheckedChanged="rdoMissedAnyTBDosesYes_CheckedChanged" />
                                                        <asp:RadioButton ID="rdoMissedAnyTBDosesNo" runat="server" GroupName="rdoMissedAnyTBDoses"
                                                            Text="No" OnCheckedChanged="rdoMissedAnyTBDosesNo_CheckedChanged" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="MissedDosesYesReferredforadherence" style="display: none">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" width="50%">
                                                        <label>
                                                            If yes, referred for adherence?</label>&nbsp;
                                                    </td>
                                                    <td align="left" width="50%">
                                                        <asp:RadioButton ID="rdoReferredForAdherenceYes" runat="server" GroupName="rdoReferredForAdherence"
                                                            Text="Yes" OnCheckedChanged="rdoReferredForAdherenceYes_CheckedChanged" />
                                                        <asp:RadioButton ID="rdoReferredForAdherenceNo" runat="server" GroupName="rdoReferredForAdherence"
                                                            Text="No" OnCheckedChanged="rdoReferredForAdherenceNo_CheckedChanged" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="form" align="center" width="50%" colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="height: 45px">
                                            <label>
                                                Review Checklist:</label><asp:CheckBoxList ID="cblReviewChecklist" runat="server"
                                                    RepeatDirection="Horizontal">
                                                </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        <div id="divSignsOfHepatitis" style="display: none" class="customdivbordermultiselectAutoHeight">
                                            <asp:CheckBoxList ID="cblSignsOfHepatitis" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                        <br />
                                        <div id="ReviewChkListSpecifyOtherTBSideEffects" style="display: none">
                                            <label>
                                                Specify other TB side effects:</label>
                                            <asp:TextBox ID="txtSpecifyOtherTBSideEffects" runat="server" Width="70%"></asp:TextBox>
                                        </div>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </div>
    <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" SuppressPostBack="true"
        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="IPTBody" CollapseControlID="IPTHeader"
        ExpandControlID="IPTHeader" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
        ImageControlID="imgIPT">
    </act:CollapsiblePanelExtender>
    <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="DiscontinueIPTHeader" runat="server" Style="padding: 6px" CssClass="border">
                    <h2 class="forms" align="left">
                        <asp:ImageButton ID="imgDiscontinueIPT" runat="server" ImageUrl="~/images/arrow-up.gif" />
                        &nbsp;Discontinue IPT</h2>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="DiscontinueIPTBody" runat="server">
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
            <tr>
                <td align="left" class="form" style="height: 59px">
                    <label>
                        Stop reason:</label>
                    <asp:CheckBoxList ID="cblStopTBReason" runat="server" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" SuppressPostBack="true"
        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="DiscontinueIPTBody" CollapseControlID="DiscontinueIPTHeader"
        ExpandControlID="DiscontinueIPTHeader" CollapsedImage="~/images/arrow-up.gif"
        Collapsed="true" ImageControlID="imgDiscontinueIPT">
    </act:CollapsiblePanelExtender>
<%--</div>--%>
<br />
<div class="border center formbg">
    <table cellspacing="6" cellpadding="0" width="100%" border="0" id="tblsavebtn" runat="server">
    <tr>
    <td class="form" align="center">
    
        <uc1:UserControlKNH_Signature ID="UserControlKNH_SignatureTB" runat="server" />
    
    </td>
    </tr>
        <tr>
            <td class="form" align="center">
                <asp:Button ID="btnTBSave" runat="server" Text="Save" OnClick="btnTBSave_Click" /><asp:Button
                    ID="btnTBDQC" runat="server" Text="Data Quality Check" Visible="False" />
                <asp:Button ID="btnTBClose" runat="server" Text="Close" OnClick="btnTBClose_Click" /><asp:Button
                    ID="btnTBPrint" runat="server" Text="Print" OnClientClick="WindowPrint();" />
            </td>
        </tr>
    </table>
</div>
