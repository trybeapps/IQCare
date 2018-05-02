<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" CodeBehind="frmClinical_KNH_ExpressForm.aspx.cs" Inherits="PresentationApp.ClinicalForms.frmClinical_KNH_ExpressForm" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>

<%@ Register src="UserControl/UserControlKNH_VitalSigns.ascx" tagname="UserControlKNH_VitalSigns" tagprefix="uc1" %>

<%@ Register src="UserControl/UserControlKNH_TBScreening.ascx" tagname="UserControlKNH_TBScreening" tagprefix="uc2" %>

<%@ Register src="UserControl/UserControlKNH_Pharmacy.ascx" tagname="UserControlKNH_Pharmacy" tagprefix="uc3" %>

<%@ Register src="UserControl/UserControlKNH_NextAppointment.ascx" tagname="UserControlKNH_NextAppointment" tagprefix="uc4" %>

<%@ Register src="UserControl/UserControlKNH_LabEvaluation.ascx" tagname="UserControlKNH_LabEvaluation" tagprefix="uc5" %>

<%@ Register src="UserControl/UserControlKNH_DrugAllergies.ascx" tagname="UserControlKNH_DrugAllergies" tagprefix="uc6" %>

<%--<%@ Register src="UserControl/UserControlKNH_Extruder.ascx" tagname="UserControlKNH_Extruder" tagprefix="uc7" %>--%>

<%@ Register src="UserControl/UserControlKNH_Signature.ascx" tagname="UserControlKNH_Signature" tagprefix="uc8" %>


<%@ Register src="UserControl/UserControlKNH_BackToTop.ascx" tagname="UserControlKNH_BackToTop" tagprefix="uc9" %>


<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
    
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

    function SelectCotrimoxazoleFluconazole(selectId, show_hide_cotrimoxazole, show_hide_fluconazole, CotrimoxazoleID, FluconazoleID) {
        if (document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text == "Cotrimoxazole") {
            show_hide(show_hide_cotrimoxazole, 'visible');
            show_hide(show_hide_fluconazole, 'notvisible');
            document.getElementById(FluconazoleID).selectedIndex = 0;
          
        }
        else if (document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text == "Fluconazole") {
            show_hide(show_hide_fluconazole, 'visible');
            show_hide(show_hide_cotrimoxazole, 'notvisible');
            document.getElementById(CotrimoxazoleID).selectedIndex = 0;
        }
        else if (document.getElementById(selectId)[document.getElementById(selectId).selectedIndex].text == "CTX and Fluconazol") {
            show_hide(show_hide_fluconazole, 'visible');
            show_hide(show_hide_cotrimoxazole, 'visible');
        }
        else {
            document.getElementById(CotrimoxazoleID).selectedIndex = 0;
            document.getElementById(FluconazoleID).selectedIndex = 0;
            show_hide(show_hide_fluconazole, 'notvisible');
            show_hide(show_hide_cotrimoxazole, 'notvisible');
        }
    }

//    function ClearSelectList(controlID) {
//        document.getElementById(controlID).selectedIndex = 0;
//    }

//    function ClearTextBox(controlID) {
//        document.getElementById(controlID).value = "";
//    }

//    function ClearRadioButtons(RadioYes,RadioNo) {
//        document.getElementById(RadioYes).checked = false;
//        document.getElementById(RadioNo).checked = false;
//    }

//    function ClearMultiSelect(controlID) {
//        var elementRef = document.getElementById(controlID);
//        var checkBoxArray = elementRef.getElementsByTagName('input');
//        for (var i = 0; i < checkBoxArray.length; i++) {
//            checkBoxArray[i].checked=false;
//        }
//    }

    function SelectOtherReviewChkList(controlID, show_hide_control, otherControlID) {
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
                        show_hide(show_hide_control, 'notvisible');
                    }
                }

            }
        }
    }

    //function pageLoad() {
    function admittedLastTwoWeeks() {
        if(document.getElementById('_ispostback').value == 'False') {
            var visitID = '<%= Session["PatientVisitId"] %>';
            if (visitID == 0) {
                var modalPopupBehavior = $find('admittedInTheLast2Weeks');
                modalPopupBehavior.show();
            }
            else {
                CalcualteBMIOnPageLoad();
            }
        }

        var gender = '<%= Session["PatientSex"] %>';
        if (gender == "Male") {
            show_hide('pregIntBeforeNextVisitTD', 'notvisible');
            show_hide('OtherFPMethodsTD', 'notvisible');
            show_hide('screenedForCancerTD', 'notvisible');
        }
    }

    function onOK() {
        alert('Kindly see patient using the Revised Adult Followup Form. Redirecting...');
        window.location.replace("frmClinical_RevisedAdultfollowup.aspx");
        //window.location.replace("../ClinicalForms/frmPatient_Home.aspx");
    }

    function CalcualteBMIOnPageLoad() {

        var weight = document.getElementById('ctl00_IQCareContentPlaceHolder_tabControl_tbTriage_UserControlKNH_VitalSigns1_txtWeight').value;
        var height = document.getElementById('ctl00_IQCareContentPlaceHolder_tabControl_tbTriage_UserControlKNH_VitalSigns1_txtHeight').value;
        if (weight == "" || height == "") {
            weight = 0;
            height = 0;
            document.getElementById('ctl00_IQCareContentPlaceHolder_tabControl_tbTriage_UserControlKNH_VitalSigns1_txtBMI').value = "";
        }
        else {
            var BMI = weight / ((height / 100) * (height / 100));
            BMI = BMI.toFixed(2);
            document.getElementById('ctl00_IQCareContentPlaceHolder_tabControl_tbTriage_UserControlKNH_VitalSigns1_txtBMI').value = BMI;
        }
    }


    function WindowPrint() {
        window.print();
    }

    function CheckBoxHideUnhideExpress(strcblcontrolId, strdivId, strfieldname, otherControlID) {
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
                    //document.getElementById(otherControlID).value = "";
                    ClearTextBox(otherControlID);
                    ShowHide(strdivId, "hide");
                }
            }
            else {
                //document.getElementById(otherControlID).value = "";
                if (arrayOfCheckBoxLabels[i].innerHTML != strfieldname) {
                    ClearTextBox(otherControlID);
                }

                
                ShowHide(strdivId, "hide");
            }
        }



    }

</script>

<style type="text/css">
    body
    {
        font-family: Arial;
        font-size: 10pt;
    }
    .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=40);
        opacity: 0.4;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        width: 300px;
        border: 1px solid #666699;
    }
    .modalPopup .header
    {
        background-color: #666699;
        height: 30px;
        color: White;
        line-height: 30px;
        text-align: left;
        font-weight: bold;
    }
    .modalPopup .body
    {
        min-height: 50px;
        line-height: 30px;
        text-align: center;
        font-weight: bold;
    }
    .modalPopup .footer
    {
        padding: 3px;
    }
    .modalPopup .yes, .modalPopup .no
    {
        height: 23px;
        color: White;
        line-height: 23px;
        text-align: center;
        font-weight: bold;
        cursor: pointer;
    }
    .modalPopup .yes
    {
        background-color: #2FBDF1;
        border: 1px solid #0DA9D0;
    }
    .modalPopup .no
    {
        background-color: #9F9F9F;
        border: 1px solid #5C5C5C;
    }
</style>
<link href="UserControl/mbExtruder.css" rel="stylesheet" type="text/css" />

    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>

<act:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="btnYes" OkControlID = "btnYes"
    CancelControlID="btnNo" BackgroundCssClass="modalBackground" BehaviorID="admittedInTheLast2Weeks" OnOkScript="onOK();">
</act:ModalPopupExtender>


<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
    <div class="header">
        Admitted in the last 2 weeks?
    </div>
    <div class="body">
        Has the patient been admitted in the last 2 weeks ?
    </div>
    <div class="footer" align="right">
        <asp:Button ID="btnYes" runat="server" Text="Yes" />
        <asp:Button ID="btnNo" runat="server" Text="No" />
    </div>
</asp:Panel>
<input type="hidden" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" />

<div style="padding: 8px;">
<div class="border center formbg">
<table cellspacing="6" cellpadding="0" width="100%" border="0">
<tr>
<td class="form" align="center">

    <label class="required">*Visit date: </label>
                    <asp:TextBox runat="server" MaxLength="11" ID="txtVisitDate"></asp:TextBox>

            <img alt="Date Helper" border="0" height="22" hspace="3" 
                onclick="w_displayDatePicker('<%= txtVisitDate.ClientID%>');" 
                src="../images/cal_icon.gif" width="22" />
                    <span class="smallerlabel">(DD-MMM-YYYY)</span> 
</td>
</tr>
</table>
</div>
<br />

<act:TabContainer ID="tabControl" runat="server" ActiveTabIndex="0"
        Width="100%">
<act:TabPanel ID="tbTriage" runat="server" Font-Size="Medium" HeaderText="Triage">
<HeaderTemplate>Triage</HeaderTemplate>
<ContentTemplate><div class="border center formbg">
    <br />

<table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
    <tr>
        <td>
            <asp:Panel ID="ClientInfoHeader" runat="server" CssClass="border center formbg" style="padding:6px">
            <h2 class="forms" align="left">
                <asp:ImageButton ID="imgClientInfo" runat="server" 
                    ImageUrl="~/images/arrow-up.gif" />
                &nbsp;<asp:Label ID="lblClientInfo" runat="server" Text="Client Information"></asp:Label>
                </h2>
            </asp:Panel>
                   
        </td>
    </tr>
</table>




<asp:Panel ID="ClientInfoBody" runat="server">
<table cellspacing="6" cellpadding="0" width="100%" border="0"><tr><td class="form" align="center">

<table width="100%"><tr align="left"><td width="50%">
    <asp:Label 
        ID="lblPtnAccByCareGiver" runat="server" CssClass="required" Font-Bold="True" 
        Text="*Patient accompanied by caregiver:"></asp:Label>
    &nbsp;<asp:RadioButton ID="rdoPtnAccByCareGiverYes" runat="server" Text="Yes" 
        GroupName="rdoPtnAccByCareGiver" 
         oncheckedchanged="rdoPtnAccByCareGiverYes_CheckedChanged" /><asp:RadioButton ID="rdoPtnAccByCareGiverNo" runat="server" Text="No" 
        GroupName="rdoPtnAccByCareGiver" 
         oncheckedchanged="rdoPtnAccByCareGiverNo_CheckedChanged" />
         </td>
         <td id="CaregiverRelationship" style="display:none" width="100%"><label class="right35">
             Caregiver relationship:</label> 
         <asp:DropDownList ID="ddlCareGiverRelationship" runat="server"></asp:DropDownList></td></tr></table></td></tr>
         </table>
         </asp:Panel>

         <act:CollapsiblePanelExtender ID="CollapsiblePanelExtenderClientInfo" 
        runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="ClientInfoBody" CollapseControlID="ClientInfoHeader"
    ExpandControlID="ClientInfoHeader" CollapsedImage="~/images/arrow-up.gif" 
        Collapsed="True" ImageControlID="imgClientInfo" Enabled="True">
    </act:CollapsiblePanelExtender>
         
    <table cellspacing="6" cellpadding="0" width="100%" border="0">
         <tr>
         <td>
         <asp:Panel ID="VitalSignsHeader" runat="server" style="padding:6px" cssclass="border">
        <h2 class="forms" align="left">
            <asp:ImageButton ID="imgVitalSigns" runat="server" ImageUrl="~/images/arrow-up.gif" />
            &nbsp;<asp:Label ID="lblVitalSigns" runat="server" Text="Vital Signs"></asp:Label>
        </h2>
    </asp:Panel>
         </td>
         </tr>
         </table>

         <asp:Panel ID="VitalSignsBody" runat="server">
         <uc1:UserControlKNH_VitalSigns ID="UserControlKNH_VitalSigns1" runat="server" />
         </asp:Panel>

         
         
         <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" 
        runat="server" SuppressPostBack="True"
        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="VitalSignsBody" CollapseControlID="VitalSignsHeader"
        ExpandControlID="VitalSignsHeader" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
        ImageControlID="imgVitalSigns" Enabled="True">
    </act:CollapsiblePanelExtender>


         <table cellspacing="6" cellpadding="0" width="100%" border="0">
         <tr>
         <td>
         <asp:Panel ID="PreexistingConditionsHeader" runat="server" CssClass="border center formbg" style="padding: 6px">
         <h2 class="forms" align="left">
         <asp:ImageButton ID="imgPreExistingConditions" runat="server" 
        ImageUrl="~/images/arrow-up.gif" />
             &nbsp;Pre-Existing (Known Conditions)
             <asp:Label ID="Label4" runat="server" Text="Medical Conditions?" 
                 Visible="False"></asp:Label>
             &nbsp;</h2>
         </asp:Panel>
         </td>
         </tr>
         </table>

         <asp:Panel ID="PreexistingConditionsBody" runat="server">

                <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tr>
                    <td ID="preExistingMedicalConditionsItems" align="left" class="form" 
                        width="50%">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <label>
                                    Pre existing Medical Condition:</label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <div runat="server" class="customdivbordermultiselect">
                                        <asp:CheckBoxList ID="cblPreExistingMedConditions" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                            <tr id="divSpecifyOthermedicalCondition" style="display: none">
                            <td align="left">
                                <asp:Label ID="Label1" runat="server" Text="Specify other:" Font-Bold="True"></asp:Label>
                                <asp:TextBox ID="txtSpecifyOthermedicalCondition"
                                    runat="server" Width="70%"></asp:TextBox>
                            </td>
                            </tr>
                        </table>
                    </td>
                    <td ID="AreYouOnFollowUp" align="center" class="form" valign="top" width="50%">
                        <table width="100%">
                            <tr valign="middle">
                                <td align="left" valign="middle" width="100%">
                                    <label>
                                    Are you on follow up ?</label>
                                    <asp:RadioButton ID="rdoOnFollowUpYes" runat="server" GroupName="rdoOnFollowUp" 
                                        OnCheckedChanged="rdoOnFollowUpYes_CheckedChanged" Text="Yes" />
                                    <asp:RadioButton ID="rdoOnFollowUpNo" runat="server" GroupName="rdoOnFollowUp" 
                                        OnCheckedChanged="rdoOnFollowUpNo_CheckedChanged" Text="No" />
                                </td>
                            </tr>
                            <tr style="width:100%">
                                <td ID="lastFollowUpDate" align="left" style="display:none" valign="top" 
                                    width="100%">
                                    <label>
                                    Last follow up :</label>
                                    <asp:TextBox ID="txtLastFollowUp" runat="server" MaxLength="11" Rows="7" 
                                        TextMode="MultiLine" Width="95%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
            </tr>
                 </table>

          </asp:Panel>

          <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" 
        runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="PreexistingConditionsBody" CollapseControlID="PreexistingConditionsHeader"
    ExpandControlID="PreexistingConditionsHeader" CollapsedImage="~/images/arrow-up.gif" 
        Collapsed="True" ImageControlID="imgPreExistingConditions" Enabled="True">
    </act:CollapsiblePanelExtender>
                 
                 
    </div>
    
    <br />
    <div class="border center formbg" style="width: 100%">
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
        <tr>
        <td class="form" align="center">
        
            <uc8:UserControlKNH_Signature ID="UserControlKNH_SignatureTriage" runat="server" />
        
        </td>
        </tr>
        <tr id="tblSaveButton"><td class="form" align="center"><asp:Button ID="btnTriageSave" runat="server" Text="Save" 
                onclick="btnTriageSave_Click" />
            <asp:Button ID="btnTraigeDQC" 
                runat="server" Text="Data Quality Check" Visible="False" /><asp:Button ID="btnTriageClose" 
                runat="server" Text="Close" onclick="btnTriageClose_Click" />
            <asp:Button ID="btnTraigePrint" runat="server" 
                Text="Print" OnClientClick="WindowPrint();" /></td></tr>
            <tr id="tblDeleteButton" style="display: none; width: 100%;" align="center">
                <td align="center" class="form" width="100%">
                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" 
                        Text="Delete" />
                </td>
            </tr>
        </table>   
    </div>   
            
    </ContentTemplate>

</act:TabPanel>

    <act:TabPanel ID="clinicalAssessment" runat="server" HeaderText="Clinical Assessment">
    <ContentTemplate><div class="border center formbg">
        <br />
        <table cellspacing="6" cellpadding="0" width="100%" border="0" >
         <tr>
         <td>
         <asp:Panel ID="DrugAllergiesHeader" runat="server" style="padding:6px" CssClass="border">
    <h2 class="forms" align="left">
        <asp:ImageButton ID="imgDrugAllergies" runat="server" 
            ImageUrl="~/images/arrow-up.gif" />
        &nbsp;Drug Allergies</h2>
        </asp:Panel>
         </td>
         </tr>
         </table>

          <asp:Panel ID="DrugAllergiesBody" runat="server">
              <uc6:UserControlKNH_DrugAllergies ID="UserControlKNH_DrugAllergies1" 
                  runat="server" />
        </asp:Panel>
        <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" 
            runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="DrugAllergiesBody" CollapseControlID="DrugAllergiesHeader"
    ExpandControlID="DrugAllergiesHeader" CollapsedImage="~/images/arrow-up.gif" 
            Collapsed="True" ImageControlID="imgDrugAllergies" Enabled="True">
    </act:CollapsiblePanelExtender>

    <table cellspacing="6" cellpadding="0" width="100%" border="0" >
         <tr>
         <td>
         <asp:Panel ID="ARVSideEffectsHeader" runat="server" style="padding:6px" CssClass="border">
    <h2 class="forms" align="left">
        <asp:ImageButton ID="imgARVSideEffects" runat="server" 
            ImageUrl="~/images/arrow-up.gif" />
        &nbsp;ARV Side Effects</h2>
        </asp:Panel>
         </td>
         </tr>
         </table>

          <asp:Panel ID="ARVSideEffectsBody" runat="server">
              <table cellspacing="6" cellpadding="0" width="100%" border="0"></tr>
                <tr>
                <td class="form" width="50%" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" 
                                    Text="Short term effects:"></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                        <td>
                        <div class="customdivbordermultiselect" id="div3" nowrap="noWrap">
                            <asp:CheckBoxList ID="cblshorttermeffects" RepeatLayout="Flow"
                                runat="server">
                            </asp:CheckBoxList>
                        </div>
                        </td>
                        <tr id="OtherShortTermSideEffect" style="display:none">
                        <td align="left">
                        
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="Other:"></asp:Label>
                        
                            &nbsp;<asp:TextBox ID="txtOtherShortTermSideEffect" runat="server" Width="80%"></asp:TextBox>
                        
                        </td>
                        </tr>
                        
                        </tr>
                    </table>
                </td>
                
                <td class="form" width="50%" valign="top">
                <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" 
                                    Text="Long term effects:"></asp:Label>
                            </td>
                            
                        </tr>
                        <tr>
                        <td align="left">
                        <div class="customdivbordermultiselect" id="div1" nowrap="noWrap">
                            <asp:CheckBoxList ID="cbllongtermeffects" RepeatLayout="Flow"
                                runat="server">
                            </asp:CheckBoxList>
                        </div>
                        </td>
                        </tr>
                        <tr id="OtherLongTermSideEffect" style="display:none">
                            <td align="left">
                                <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Other:"></asp:Label>
                                &nbsp;<asp:TextBox ID="txtOtherLongTermSideEffect" runat="server" Width="80%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </asp:Panel>
        <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" 
            runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="ARVSideEffectsBody" CollapseControlID="ARVSideEffectsHeader"
    ExpandControlID="ARVSideEffectsHeader" CollapsedImage="~/images/arrow-up.gif" 
            Collapsed="True" ImageControlID="imgARVSideEffects" Enabled="True">
    </act:CollapsiblePanelExtender>



    <table cellspacing="6" cellpadding="0" width="100%" border="0" >
         <tr>
         <td>
         <asp:Panel ID="RegimenPrescribedHeader" runat="server" style="padding:6px" CssClass="border">
    <h2 class="forms" align="left">
        <asp:ImageButton ID="imgRegimenPrescribed" runat="server" 
            ImageUrl="~/images/arrow-up.gif" />
        &nbsp;<asp:Label ID="lblRegimenPrescribed" runat="server" Text="Regimen Prescribed"></asp:Label></h2>
        </asp:Panel>
         </td>
         </tr>
         </table>
    
        <asp:Panel ID="RegimenPrescribedBody" runat="server">
        <uc3:UserControlKNH_Pharmacy ID="UserControlKNH_Pharmacy1" runat="server" />
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
            <tr>
                <td align="center" class="form">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <label>
                                OI prophylaxis :</label>
                                <asp:DropDownList ID="ddlOIProphylaxis" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                            <table width="100%">
                            <tr ID="Cotrimoxazoleprescribedfor" style="display:none">
                            <td>
                                    <label>
                                    Cotrimoxazole prescribed for :</label><asp:DropDownList 
                                        ID="ddlCotrimoxazolePrescribed" runat="server">
                                    </asp:DropDownList>
                            </td>
                            </tr>
                            <tr ID="Fluconazoleprescribedfor" style="display:none">
                            <td>
                                    <label>
                                    Fluconazole prescribed for :</label><asp:DropDownList 
                                        ID="ddlFluconazolePrescribedFor" runat="server">
                                    </asp:DropDownList>
                            </td>
                            </tr>
                            <tr ID="OtherOIProphylaxis" style="display:none">
                            <td>
                                    <label>
                                    Other (Specify):</label><asp:TextBox ID="txtSpecifyOtherOIProphylaxis" 
                                        runat="server"></asp:TextBox>
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
        <act:CollapsiblePanelExtender ID="CollapsiblePanelExtenderRegimenPrescribed" 
            runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="RegimenPrescribedBody" CollapseControlID="RegimenPrescribedHeader"
    ExpandControlID="RegimenPrescribedHeader" CollapsedImage="~/images/arrow-up.gif" 
            Collapsed="True" ImageControlID="imgRegimenPrescribed" Enabled="True">
    </act:CollapsiblePanelExtender>

     
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
         <tr>
         <td>
         <asp:Panel ID="AdherenceHeader" runat="server" style="padding:6px" 
                 CssClass="border">
        <h2 class="forms" align="left">
            <asp:ImageButton ID="imgAdherence" runat="server" 
                ImageUrl="~/images/arrow-up.gif" />
            &nbsp;Adherence</h2>
            </asp:Panel>
         </td>
         </tr>
         </table>
        

            <asp:Panel ID="AdherenceBody" runat="server">
            <table cellspacing="6" cellpadding="0" width="100%" border="0"><tr>
        <td align="center" class="form" style="width: 50%"><table width="100%"><tr>
            <td align="left"><label>Have you missed any doses?</label> <asp:RadioButton 
                ID="rdoMissedAnyDosesYes" runat="server" GroupName="rdoMissedAnyDoses" 
                Text="Yes" 
        oncheckedchanged="rdoMissedAnyDosesYes_CheckedChanged" /><asp:RadioButton ID="rdoMissedAnyDosesNo" runat="server" 
                GroupName="rdoMissedAnyDoses" Text="No" 
        oncheckedchanged="rdoMissedAnyDosesNo_CheckedChanged" /></td></tr><tr><td 
                    ID="specifyMissedDoses" align="left" style="display:none" valign="middle"><label>Specify why doses missed :</label> <asp:TextBox 
                    ID="txtSpecifyWhyDosesMissed" runat="server" TextMode="MultiLine" Width="80%"></asp:TextBox></td></tr></table></td><td 
            class="form" align="center" width="50%"><label>Have you delayed in taking medication?</label> <asp:RadioButton 
                ID="rdoDelayedTakingMedicationYes" runat="server" 
                GroupName="rdoDelayedTakingMedication" Text="Yes" 
        oncheckedchanged="rdoDelayedTakingMedicationYes_CheckedChanged" /><asp:RadioButton 
                ID="rdoDelayedTakingMedicationNo" runat="server" 
                GroupName="rdoDelayedTakingMedication" Text="No" 
        oncheckedchanged="rdoDelayedTakingMedicationNo_CheckedChanged" /></td></tr></table>
        </asp:Panel>
        <act:CollapsiblePanelExtender ID="CollapsiblePanelExtenderAdherence" 
            runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="AdherenceBody" CollapseControlID="AdherenceHeader"
    ExpandControlID="AdherenceHeader" CollapsedImage="~/images/arrow-up.gif" 
            Collapsed="True" ImageControlID="imgAdherence" Enabled="True">
    </act:CollapsiblePanelExtender>

        
        
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
         <tr>
         <td>
         <asp:Panel ID="LabEvaluationHeader" runat="server" style="padding:6px" CssClass="border">
        <h2 class="forms" align="left">
        <asp:ImageButton ID="imgLabEvaluation" runat="server" 
                ImageUrl="~/images/arrow-up.gif" />
            Lab Evaluation 
            <asp:RadioButton ID="rdoLabEvaluationYes" runat="server" Text="Yes" 
                GroupName="rdoLabEvaluation" 
                oncheckedchanged="rdoLabEvaluationYes_CheckedChanged" Visible="False" />
            <asp:RadioButton ID="rdoLabEvaluationNo" runat="server" Text="No" 
                GroupName="rdoLabEvaluation" 
                oncheckedchanged="rdoLabEvaluationNo_CheckedChanged" Visible="False" /></h2>
                </asp:Panel>
         </td>
         </tr>
         </table>
        

                <asp:Panel ID="LabEvaluationBody" runat="server">
                    <uc5:UserControlKNH_LabEvaluation ID="UserControlKNH_LabEvaluation1" 
                        runat="server" />
            </asp:Panel>

            <act:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" 
            runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="LabEvaluationBody" CollapseControlID="LabEvaluationHeader"
    ExpandControlID="LabEvaluationHeader" CollapsedImage="~/images/arrow-up.gif" 
            Collapsed="True" ImageControlID="imgLabEvaluation" Enabled="True">
    </act:CollapsiblePanelExtender>
            
            
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
         <tr>
         <td>
         <asp:Panel ID="WorkPlanHeader" runat="server" style="padding:6px" CssClass="border">
            <h2 class="forms" align="left">
            <asp:ImageButton ID="imgWorkPlan" runat="server" 
                ImageUrl="~/images/arrow-up.gif" />
                &nbsp;Work Plan</h2>
            </asp:Panel>
         </td>
         </tr>
         </table>
            
            <asp:Panel ID="WorkPlanBody" runat="server">
            <table cellspacing="6" cellpadding="0" width="100%" border="0"><tr><td align="left" class="form">
                <asp:TextBox ID="txtPlan" runat="server" Rows="3" TextMode="MultiLine" 
                    Width="100%"></asp:TextBox></td></tr></table>
                    </asp:Panel>

                    <act:CollapsiblePanelExtender ID="CollapsiblePanelExtenderWorkPlan" 
            runat="server" SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="WorkPlanBody" CollapseControlID="WorkPlanHeader"
    ExpandControlID="WorkPlanHeader" CollapsedImage="~/images/arrow-up.gif" 
            Collapsed="True" ImageControlID="imgWorkPlan" Enabled="True">
    </act:CollapsiblePanelExtender>
                    
                    
         <table cellspacing="6" cellpadding="0" width="100%" border="0">
         <tr>
         <td>
         <asp:Panel ID="PwPHeader" runat="server" style="padding:6px" cssclass="border">
                    <h2 class="forms" align="left">
            <asp:ImageButton ID="imgPwP" runat="server" 
                ImageUrl="~/images/arrow-up.gif" />
                        &nbsp;PwP Intervention </h2>
            </asp:Panel>
         </td>
         </tr>
         </table>
                    
            <asp:Panel ID="PwPBody" runat="server">
            <table cellspacing="6" cellpadding="0" width="100%" border="0"><tr>
        <td class="form" align="center" style="width: 50%"><label>PwP message given :</label><asp:DropDownList 
            ID="ddlPwPMessageGiven" runat="server"></asp:DropDownList></td><td 
            class="form" align="center"><table width="100%"><tr><td align="center"><label>Patient issued with condoms :</label><asp:RadioButton 
                    ID="rdoIssuedWithCondomsYes" runat="server" GroupName="rdoIssuedWithCondoms" 
                    Text="Yes" 
            oncheckedchanged="rdoIssuedWithCondomsYes_CheckedChanged" /><asp:RadioButton ID="rdoIssuedWithCondomsNo" runat="server" 
                    GroupName="rdoIssuedWithCondoms" Text="No" 
            oncheckedchanged="rdoIssuedWithCondomsNo_CheckedChanged" /></td></tr><tr>
                            <td 
                        ID="ReasonsCondomNotIssued" style="display:none" align="center"><label>Reasons condom not issued :</label><asp:TextBox 
                        ID="txtReasonCondomsNotIssued" runat="server" Width="50%"></asp:TextBox></td></tr></table></td></tr><tr><td id="pregIntBeforeNextVisitTD" align="center" class="form" colspan="2"><table width="100%"><tr>
                    <td align="left"><label>
                        Pregnancy intention before next visit :</label> <asp:RadioButton 
                    ID="rdoPregBeforeNextVisitYes" runat="server" 
                    GroupName="rdoPregBeforeNextVisit" Text="Yes" 
         oncheckedchanged="rdoPregBeforeNextVisitYes_CheckedChanged" /><asp:RadioButton 
                    ID="rdoPregBeforeNextVisitNo" runat="server" GroupName="rdoPregBeforeNextVisit" 
                    Text="No" 
         oncheckedchanged="rdoPregBeforeNextVisitNo_CheckedChanged" /></td><td align="left"><div ID="discussedFertilityOptions" style="display:none"><label>If yes, discussed fertility options?</label><asp:RadioButton 
                        ID="rdoDiscussedFertilityOptionsYes" runat="server" 
                        GroupName="rdoDiscussedFertilityOptions" Text="Yes" 
        oncheckedchanged="rdoDiscussedFertilityOptionsYes_CheckedChanged" /><asp:RadioButton 
                        ID="rdoDiscussedFertilityOptionsNo" runat="server" 
                        GroupName="rdoDiscussedFertilityOptions" Text="No" 
        oncheckedchanged="rdoDiscussedFertilityOptionsNo_CheckedChanged" /></div><div 
                        ID="discussedDualContraception" style="display:none"><label>If no, discussed dual contraception?</label><asp:RadioButton 
                            ID="rdoDiscussedDualContraceptionYes" runat="server" 
                            GroupName="rdoDiscussedDualContraception" Text="Yes" /><asp:RadioButton 
                            ID="rdoDiscussedDualContraceptionNo" runat="server" 
                            GroupName="rdoDiscussedDualContraception" Text="No" /></div></td></tr></table></td></tr><tr><td id="OtherFPMethodsTD" align="center" class="form" colspan="2"><table width="100%"><tr>
                    <td align="left"><label>Are you on any family planning method?:</label> <asp:RadioButton 
                    ID="rdoOtherFPMethodYes" runat="server" GroupName="rdoOtherFPMethod" 
                    Text="Yes" 
         oncheckedchanged="rdoOtherFPMethodYes_CheckedChanged" /><asp:RadioButton ID="rdoOtherFPMethodNo" runat="server" 
                    GroupName="rdoOtherFPMethod" Text="No" 
         oncheckedchanged="rdoOtherFPMethodNo_CheckedChanged" /></td>
                    <td ID="specifyOtherFPMethod" 
                    style="display:none" align="left"><label>Specify other FP method:</label><asp:DropDownList 
                        ID="ddlSpecifyOtherFPMethod" runat="server"></asp:DropDownList></td></tr></table></td></tr><tr><td id="screenedForCancerTD" align="center" class="form" colspan="2"><table width="100%"><tr>
                    <td align="left"><label>Have you been screened for cervical cancer?</label> <asp:RadioButton 
                    ID="rdoscreenedForCervicalCancerYes" runat="server" 
                    GroupName="rdoscreenedForCervicalCancer" Text="Yes" 
         oncheckedchanged="rdoscreenedForCervicalCancerYes_CheckedChanged" /><asp:RadioButton 
                    ID="rdoscreenedForCervicalCancerNo" runat="server" 
                    GroupName="rdoscreenedForCervicalCancer" Text="No" 
         oncheckedchanged="rdoscreenedForCervicalCancerNo_CheckedChanged" /></td><td align="left"><div 
                        ID="cacervixscreenresults" style="display:none"><label>Ca cervix screening results:</label><asp:DropDownList 
                        ID="ddlCaCervixScreeningResults" runat="server"></asp:DropDownList></div><div 
                        ID="referredForCervicalScreening" style="display:none"><label>If no, referred for cervical screening?</label><asp:RadioButton 
                            ID="rdoReferredForCervicalScreeningYes" runat="server" 
                            GroupName="rdoReferredForCervicalScreening" Text="Yes" 
        oncheckedchanged="rdoReferredForCervicalScreeningYes_CheckedChanged" /><asp:RadioButton 
                            ID="rdoReferredForCervicalScreeningNo" runat="server" 
                            GroupName="rdoReferredForCervicalScreening" Text="No" 
        oncheckedchanged="rdoReferredForCervicalScreeningNo_CheckedChanged" /></div></td></tr></table></td></tr></table>

                
        </asp:Panel>

        <act:CollapsiblePanelExtender ID="CollapsiblePanelExtenderPwP" runat="server" 
            SuppressPostBack="True" ExpandedImage="~/images/arrow-dn.gif" 
    TargetControlID="PwPBody" CollapseControlID="PwPHeader"
    ExpandControlID="PwPHeader" CollapsedImage="~/images/arrow-up.gif" 
            Collapsed="True" ImageControlID="imgPwP" Enabled="True">
    </act:CollapsiblePanelExtender>
        
        
                </div>
                
                <br />
                <div class="border center formbg">
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                    <tr>
                    <td class="form" align="center">
                    
                        <uc8:UserControlKNH_Signature ID="UserControlKNH_SignatureCA" runat="server" />
                    
                    </td>
                    </tr>
                    <tr>
                    <td class="form" align="center">
                    <asp:Button ID="btnCASave" runat="server" Text="Save" onclick="btnCASave_Click"  />
                    <asp:Button ID="btnCADQC" runat="server" Text="Data Quality Check" Visible="False" />
                    <asp:Button ID="btnCAClose" runat="server" Text="Close" onclick="btnCAClose_Click" />
                    <asp:Button ID="btnCAPrint" runat="server" Text="Print" OnClientClick="WindowPrint();" />
                    </td>
                    </tr>
                    </table>
                </div>
    </ContentTemplate>
 
</act:TabPanel>

    <act:TabPanel ID="TBPanel" runat="server" HeaderText="TB Screening">
        
        <ContentTemplate>
        <div class="border center formbg">
            <uc2:UserControlKNH_TBScreening ID="UserControlKNH_TBScreening1" 
                runat="server" ></uc2:UserControlKNH_TBScreening>
         </div>
        </ContentTemplate>
    </act:TabPanel>

</act:TabContainer>

    <uc9:UserControlKNH_BackToTop ID="UserControlKNH_BackToTop1" runat="server" />

    <%--<uc7:UserControlKNH_Extruder ID="UserControlKNH_Extruder1" runat="server" />--%>

</div>

</asp:Content>
