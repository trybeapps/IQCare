<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/IQCare.master"
    CodeBehind="frmClinical_KNH_AdultIE.aspx.cs" Inherits="PresentationApp.ClinicalForms.frmClinical_KNH_AdultIE" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="UcVitalSign" TagName="Uc1" Src="~/ClinicalForms/UserControl/UserControlKNH_VitalSigns.ascx" %>
<%@ Register TagPrefix="UcTBScreening" TagName="Uc1" Src="~/ClinicalForms/UserControl/UserControlKNH_TBScreening.ascx" %>
<%@ Register TagPrefix="UcMedicalHistory" TagName="Uc2" Src="~/ClinicalForms/UserControl/UserControlKNH_MedicalHistory.ascx" %>
<%@ Register TagPrefix="UcPresentingComplaints" TagName="Uc3" Src="~/ClinicalForms/UserControl/UserControlKNHPresentingComplaints.ascx" %>
<%@ Register TagPrefix="UcPhysicalExamination" TagName="Uc4" Src="~/ClinicalForms/UserControl/UserControlKNH_PhysicalExamination.ascx" %>
<%@ Register TagPrefix="UcWhoStaging" TagName="Uc5" Src="~/ClinicalForms/UserControl/UserControlKNH_WHOStaging.ascx" %>
<%@ Register TagPrefix="UcDrugAllergies" TagName="Uc6" Src="~/ClinicalForms/UserControl/UserControlKNH_DrugAllergies.ascx" %>
<%@ Register TagPrefix="UcPharmacy" TagName="Uc7" Src="~/ClinicalForms/UserControl/UserControlKNH_Pharmacy.ascx" %>
<%@ Register TagPrefix="UcPWP" TagName="Uc9" Src="~/ClinicalForms/UserControl/UserControlKNH_PwP.ascx" %>
<%@ Register Src="UserControl/UserControlKNH_Extruder.ascx" TagName="UserControlKNH_Extruder"
    TagPrefix="uc10" %>
<%@ Register Src="~/ClinicalForms/UserControl/UserControlKNH_LabEvaluation.ascx"
    TagName="Uclabeval" TagPrefix="uc11" %>
<%@ Register Src="~/ClinicalForms/UserControl/UserControlKNH_Signature.ascx" TagName="UserControlKNH_Signature"
    TagPrefix="uc12" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<asp:Content ID="content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
    <link href="UserControl/mbExtruder.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Touch/Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="../Touch/Styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(function () {

            $("#tabs").tabs();

        });
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
        function WindowPrintAll() {
            window.print();
        }
    </script>
    <div style="padding-left: 8px; padding-right: 8px; padding-top: 2px;">
        <div class="border center formbg">
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tr>
                    <td class="form" align="center">
                        <label class="required">
                            *Visit date:
                        </label>
                        <input id="txtVisitDate" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                            onfocus="javascript:vDateType='3'" maxlength="11" size="11" runat="server" type="text" />
                        <img id="appDateimg1" onclick="w_displayDatePicker('<%=txtVisitDate.ClientID%>');"
                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                            border="0" name="appDateimg" style="vertical-align: text-bottom;" /><span class="smallerlabel"
                                id="appDatespan1">(DD-MMM-YYYY)</span>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <asp:ScriptManager ID="mst" runat="server">
        </asp:ScriptManager>
        <%--<asp:UpdatePanel ID="UpdateMasterLink" runat="server">
            <ContentTemplate>--%>
        <div class="border formbg">
            <br></br>
            <act:TabContainer ID="tabControl" runat="server" ActiveTabIndex="0">
                <act:TabPanel ID="TabPanel3" runat="server" Font-Size="Large" HeaderText="Triage">
                    <ContentTemplate>
                        <div class="border center formbg">
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlHIVCare" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgHIVCare" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                <asp:Label ID="lblheadTriage" runat="server" Text="Triage/HIV Care and Support Evaluation"></asp:Label></h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlHivCareDetail" runat="server" Style="padding: 6px">
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td style='width: 50%' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label for="rblDiagnosisYesNo">
                                                            Diagnosis confirmed:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:RadioButtonList ID="rblDiagnosisYesNo" runat="server" onclick="rblSelectedValue(this,'DIVHIVDiagnosis');"
                                                            RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <div id="DIVHIVDiagnosis" style="display: none;">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label>
                                                                Date of HIV diagnosis:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <input id="txtdtConfirmHIVPosDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                maxlength="11" size="11" runat="server" type="text" />
                                                            &nbsp; &nbsp;
                                                            <img id="Img1" onclick="w_displayDatePicker('<%=txtdtConfirmHIVPosDate.ClientID%>');"
                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" /><span class="smallerlabel"
                                                                    id="Span1">(DD-MMM-YYYY)</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" cellspacing="6" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td style='width: 50%' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <asp:Label ID="lblcaregiver" runat="server" CssClass="required" Font-Bold="True"
                                                            Text="*Patient accompanied by caregiver:"></asp:Label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:RadioButtonList ID="rblChildAccompaniedBy" runat="server" RepeatDirection="Horizontal"
                                                            OnClick="rblSelectedValue(this,'divTreatmentSupporterRelationship')">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width='100%' id="divTreatmentSupporterRelationship" style="display: none;">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Caregiver relationship:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:DropDownList ID="ddlTreatmentSupporterRelationship" runat="server" Width="130px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
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
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td style='width: 50%' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            If adolescent, disclosure status:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:DropDownList ID="ddlDisclosureStatus" runat="server" Width="130px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <div id="divSpecifyreason" style="display: none;">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label>
                                                                Specify reason not disclosed:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <asp:DropDownList ID="ddreasondisclosed" runat="server" Width="130px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divSpecifyotherdisclosurestatus" style="display: none;">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label>
                                                                Specify other disclosure status:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <asp:TextBox ID="txtotherdisclosurestatus" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="50%" colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            If adolescent, schooling status:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:DropDownList ID="ddlSchoolingStatus" runat="server" Width="130px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <div id="divHighestLevelattained" style="display: none">
                                                <table width="100%">
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label>
                                                                Highest level attained:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <asp:DropDownList ID="ddHighestlevelattained" runat="server" Width="130px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="50%" colspan="2" align="left">
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            Is client a member of a support group?</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:RadioButtonList ID="rblHIVSupportgroup" runat="server" RepeatDirection="Horizontal"
                                                            onclick="rblSelectedValue(this,'DIVsupportgroup');">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <div id="DIVsupportgroup" style="display: none;">
                                                <table width="100%">
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label>
                                                                HIV support group membership:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <asp:TextBox ID="txtHIVSupportGroup" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                </td> </tr> </table>
                                <table width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td class="form" align="left">
                                            <table width="100%">
                                                <tr>
                                                    <td width="18%" align="right">
                                                        <label class="required">
                                                            <asp:Label ID="lblPatRef" runat="server" CssClass="required" Font-Bold="True" Text="*Patient referred from:"></asp:Label>
                                                        </label>
                                                    </td>
                                                    <td width="32%" align="left">
                                                        <asp:DropDownList ID="ddlPatientReferred" runat="server" Width="130px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="50%" align="left">
                                                        <div id="divPatRefother" style="display: none">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <label for="txtPatRefother">
                                                                            Other specify:
                                                                        </label>
                                                                        <asp:TextBox ID="txtPatRefother" runat="server" Width="180px"></asp:TextBox>
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
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlvitalsign" runat="server" CssClass="border center formbg" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgvitalsign" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                <asp:Label ID="lblVitalSigns" runat="server" Text="Vital Signs"></asp:Label></h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" width="100%">
                                        <asp:Panel ID="pnlVitalSignDetail" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <UcVitalSign:Uc1 ID="idVitalSign" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <act:CollapsiblePanelExtender ID="CPEHIVCare" runat="server" SuppressPostBack="True"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlHivCareDetail" CollapseControlID="pnlHIVCare"
                            ExpandControlID="pnlHIVCare" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
                            ImageControlID="imgHIVCare" Enabled="True">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEVitalSign" runat="server" SuppressPostBack="True"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlVitalSignDetail" CollapseControlID="pnlVitalSign"
                            ExpandControlID="pnlVitalSign" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
                            ImageControlID="imgVitalSign" Enabled="True">
                        </act:CollapsiblePanelExtender>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr align="center">
                                        <td class="form">
                                            <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureTriage" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="form">
                                            <asp:Button ID="btnSaveTriage" runat="server" Text="Save" OnClick="btnSaveTriage_Click" />
                                            <asp:Button ID="btnCloseTriage" Text="Close" runat="server" OnClick="btncloseTriage_Click" />
                                            <asp:Button ID="btnPrintTriage" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabClinicalHistory" runat="server" Font-Size="Medium" HeaderText="Clinical History">
                    <HeaderTemplate>
                        Clinical History
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="border center formbg">
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlPComp" runat="server" CssClass="border center formbg" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="ImgPC" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                <asp:Label ID="lblPresComp" runat="server" Text="Presenting Complaints"></asp:Label></h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" width="100%">
                                        <asp:Panel ID="pnlTPComp" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <UcPresentingComplaints:Uc3 ID="UcPc" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlFP" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="ImgFP" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                <asp:Label ID="lblheadMedicalCond" runat="server" Text="Pre-Existing (Known Conditions)-Medical Conditions"></asp:Label></h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlFPDetail" runat="server">
                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td width="50%" class="form" align="left">
                                            <asp:Label ID="lblchlongtermmedication" runat="server" CssClass="required" Font-Bold="True"
                                                Text="*Long term medications:"></asp:Label>
                                            <div class="customdivbordermultiselect">
                                                <asp:CheckBoxList ID="cblPreExistingMedConditions" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                            <br />
                                            <div id="divothermedcondition" style="display: none">
                                                <label>
                                                    Other medical conditions :</label>
                                                <asp:TextBox ID="txtOthermedicalconditions" runat="server" Width="60%"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="form">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 20%" align="right">
                                                        <label>
                                                            Are you on followup?</label>
                                                    </td>
                                                    <td style="width: 30%" align="left">
                                                        <asp:RadioButtonList ID="rblFP" runat="server" RepeatDirection="Horizontal" OnClick="rblSelectedValue(this,'hideFP')">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td style="width: 50%" align="right">
                                                        <div id="hideFP" style="display: none;">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <label>
                                                                            Last follow up/treatment :</label>
                                                                        <asp:TextBox ID="txtLastFP" runat="server" Rows="4" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <%-- <td style="padding-left: 20%">
                                                             <br />
                                                              
                                                            </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <act:CollapsiblePanelExtender ID="CPPC" runat="server" SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif"
                            TargetControlID="pnlTPComp" CollapseControlID="pnlPComp" ExpandControlID="pnlPComp"
                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgPC">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEFP" runat="server" SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif"
                            TargetControlID="pnlFPDetail" CollapseControlID="pnlFP" ExpandControlID="pnlFP"
                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgFP">
                        </act:CollapsiblePanelExtender>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr align="center">
                                        <td class="form">
                                            <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureClinical" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="form">
                                            <asp:Button ID="btnSaveCHistory" runat="server" Text="Save" OnClick="btnSaveCHistory_Click" />
                                            <asp:Button ID="btncloseCHistory" Text="Close" runat="server" OnClick="btncloseCHistory_Click" />
                                            <asp:Button ID="btnPrintCHistory" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabpnlTBSCreening" runat="server" Font-Size="Medium" HeaderText="TB Screening">
                    <HeaderTemplate>
                        TB Screening
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="border center formbg">
                            <table class="center formbg" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2" width="100%">
                                        <asp:Panel ID="Panel3" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <UcTBScreening:Uc1 ID="UcTBScreening" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabpnlExamination" runat="server" Font-Size="Medium" HeaderText="Examination">
                    <HeaderTemplate>
                        Examination
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="border center formbg">
                            <table id="TabExamination" class="center formbg" cellspacing="6" cellpadding="0"
                                width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlLTM" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="ImgLTM" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                <asp:Label ID="lblCurrentLongTermMedications" runat="server" Text="Current Long Term Medications"></asp:Label></h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlLTMedicine" runat="server">
                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td width="50%" class="form" colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 50%" align="left">
                                                        <asp:Label ID="lblchkLTMedications" runat="server" CssClass="required" Font-Bold="True"
                                                            Text="*Long term medications:"></asp:Label>
                                                        <div class="customdivbordermultiselect">
                                                            <asp:CheckBoxList ID="chkLTMedications" RepeatLayout="Flow" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                    <td style="width: 50%; display: none;" align="left" id="hideOtherLTM">
                                                        <label for="txOtherLongTermMedications">
                                                            Other current long term medications:</label>
                                                        <asp:TextBox ID="txOtherLongTermMedications" runat="server" Width="100%">
                                                        </asp:TextBox>
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
                                        <asp:Panel ID="pnlPE" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgPE" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                <asp:Label ID="lblPhysicalExamination" runat="server" Text="Physical Examination"></asp:Label></h2>
                                            </h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlPEDetail" runat="server">
                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td colspan="2" width="100%">
                                            <UcPhysicalExamination:Uc4 ID="UcPE" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="PnlHReHis" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="ImgRH" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                HIV Related History</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlTHReHis" runat="server" Style="padding: 6px">
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="15%" colspan="5">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 100%' align='left'>
                                                        <label>
                                                            Initial CD4:</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="15%">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            Count:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtInitialCD4" runat="server" Width="90%" TextMode="SingleLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 8%'>
                                        </td>
                                        <td style='width: 23%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Percent:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:TextBox ID="txtInitialCD4Percent" runat="server" Width="60%" TextMode="SingleLine"></asp:TextBox>
                                                        <label>
                                                            %</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label>
                                                            Date:</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <input id="dtInitialCD4Date" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="VisitDate" runat="server" />
                                                        <img id="Img3" onclick="w_displayDatePicker('<%=dtInitialCD4Date.ClientID%>');" height="22 "
                                                            alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                            name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span3">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="15%" colspan="4">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='left'>
                                                        <label>
                                                            Highest CD4 ever:</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="15%">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            Count:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtHighestCD4Ever" Width="90%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 8%'>
                                        </td>
                                        <td style='width: 23%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Percent:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:TextBox ID="txtHighestCD4Percent" Width="60%" runat="server"></asp:TextBox>
                                                        <label>
                                                            %</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label>
                                                            Date:</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <input id="dtHighestCD4Date" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="VisitDate" runat="server" />
                                                        <img id="Img23" onclick="w_displayDatePicker('<%=dtHighestCD4Date.ClientID%>');"
                                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span23">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="15%" colspan="5">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='left'>
                                                        <label>
                                                            Most Recent CD4:</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="15%">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            Count:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtMostRecentCD4" Width="90%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 8%'>
                                        </td>
                                        <td style='width: 23%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Percent:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:TextBox ID="txtCD4PercentAtARTInitiation" Width="60%" runat="server"></asp:TextBox>
                                                        <label>
                                                            %</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label>
                                                            Date:</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <input id="dtCD4atARTinitiationDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="VisitDate" runat="server" />
                                                        <img id="Img24" onclick="w_displayDatePicker('<%=dtCD4atARTinitiationDate.ClientID%>');"
                                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span24">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="15%" colspan="5">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='left'>
                                                        <label>
                                                            CD4 at ART initiation:</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="15%">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            Count:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtCD4atARTinitiation" Width="90%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 8%'>
                                        </td>
                                        <td style='width: 23%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Percent:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <asp:TextBox ID="txtMostRecentCD4Percent" Width="60%" runat="server"></asp:TextBox>
                                                        <label>
                                                            %</label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label>
                                                            Date:</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <input id="dtMostRecentCD4Date" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="VisitDate" runat="server" />
                                                        <img id="Img25" onclick="w_displayDatePicker('<%=dtMostRecentCD4Date.ClientID%>');"
                                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span25">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="30%" colspan="3">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            Previous viral load:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtPreviousViralLoad" Width="60%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Date:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <input id="dtPreviousViralLoadDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="VisitDate" runat="server" />
                                                        <img id="Img26" onclick="w_displayDatePicker('<%=dtPreviousViralLoadDate.ClientID%>');"
                                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span26">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td colspan="3">
                                            <table width="100%" align="left">
                                                <tr>
                                                    <td align='left'>
                                                        <label for="txtOtherHIVRelatedHistory">
                                                            Other HIV related history:</label>
                                                        <asp:TextBox ID="txtOtherHIVRelatedHistory" Width="100%" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                </td> </tr> </table>
                            </asp:Panel>
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="pnlARVExposure" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgARVExposure" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                ARV Exposure</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlARVExposureDetail" runat="server" Style="padding: 6px">
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="30%" colspan="3">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            PMTCT Regimen:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtPMTCTRegimen" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Start Date:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <input id="dtPMTCTStartDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="DateofHIVDiagnosis" runat="server" />
                                                        <img id="Img27" onclick="w_displayDatePicker('<%=dtPMTCTStartDate.ClientID%>');"
                                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span27">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="30%" colspan="3">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            PEP Regimen:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtPEP" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Start Date:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <input id="dtPEP1StartDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="VisitDate" runat="server" />
                                                        <img id="Img28" onclick="w_displayDatePicker('<%=dtPEP1StartDate.ClientID%>');" height="22 "
                                                            alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                            name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span28">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="30%" colspan="3">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 60%' align='right'>
                                                        <label>
                                                            HAART Regimen:</label>
                                                    </td>
                                                    <td style='width: 40%' align='left'>
                                                        <asp:TextBox ID="txtHAARTRegimen" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <table width="100%">
                                                <tr>
                                                    <td style='width: 50%' align='right'>
                                                        <label>
                                                            Start Date:</label>
                                                    </td>
                                                    <td style='width: 50%' align='left'>
                                                        <input id="dtHAART1StartDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                            maxlength="11" size="11" name="VisitDate" runat="server" />
                                                        <img id="Img29" onclick="w_displayDatePicker('<%=dtHAART1StartDate.ClientID%>');"
                                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                        <span class="smallerlabel" id="Span29">(DD-MMM-YYYY)</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td style='width: 50%' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label>
                                                            Any doses missed:</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <asp:RadioButtonList ID="rblARVdosesmissed" runat="server" RepeatDirection="Horizontal"
                                                            onclick="rblSelectedValue(this, 'DIVStopINHDate')">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <div id="DIVStopINHDate" style="display: none;">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label>
                                                                Delayed doses:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <asp:RadioButtonList ID="rblARVDelayeddoses" runat="server" RepeatDirection="Horizontal">
                                                                <%--                                                                        onclick="rblSelectedValueNo(this, 'DIVStopINHDate')">--%>
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>
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
                                        <asp:Panel ID="pnlWHOStage" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgWHOStage" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                WHO Stage</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlWHOStageDetail" runat="server">
                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td colspan="2" width="100%">
                                            <UcWhoStaging:Uc5 ID="UcWhostaging" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="pnlDiagnosis" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgDiagnosis" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                Diagnosis</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlDiagnosisDetail" runat="server">
                                <table class="formbg" cellspacing="6" cellpadding="0" width="100%" border="0" style="margin-bottom: 6px;">
                                    <tr>
                                        <td width="50%" class="border pad5 whitebg">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" style="width: 50%">
                                                        <label>
                                                            Diagnosis and current illness at this visit:</label>
                                                        <div id="divDiagnosis" class="customdivbordermultiselect">
                                                            <asp:CheckBoxList ID="cblDiagnosis" RepeatLayout="Flow" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div id="DIVHIVrelatedOI" style="display: none;">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="right">
                                                                        <label>
                                                                            Specify HIV related OI:</label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtHIVRelatedOI" runat="server" Skin="MetroTouch" Width="100%">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="DIVNonHIVrelatedOI" style="display: none;">
                                                            <table class="tbl-right">
                                                                <tr>
                                                                    <td align="right" class="data-lable">
                                                                        <label>
                                                                            Specify Non HIV related OI:</label>
                                                                    </td>
                                                                    <td align="left" class="data-control">
                                                                        <asp:TextBox ID="txtNonHIVRelatedOI" runat="server" Skin="MetroTouch" Width="100%">
                                                                        </asp:TextBox>
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
                                        <td align="left" class="border pad5 whitebg">
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <label>
                                                            Impression:
                                                        </label>
                                                        <asp:TextBox ID="txtImpression" runat="server" Columns="20" Rows="2" Width="100%"
                                                            TextMode="MultiLine">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <act:CollapsiblePanelExtender ID="CPELTM" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlLTMedicine" CollapseControlID="pnlLTM"
                            ExpandControlID="pnlLTM" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgLTM">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEPE" runat="server" SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif"
                            TargetControlID="pnlPEDetail" CollapseControlID="pnlPE" ExpandControlID="pnlPE"
                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="ImgLTM">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEHIVRH1" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlTHReHis" CollapseControlID="pnlHReHis"
                            ExpandControlID="pnlHReHis" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgRH">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPARVExposure" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlARVExposureDetail"
                            CollapseControlID="pnlARVExposure" ExpandControlID="pnlARVExposure" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="true" ImageControlID="ImgARVExposure">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPDiagnosis" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlDiagnosisDetail" CollapseControlID="pnlDiagnosis"
                            ExpandControlID="pnlDiagnosis" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgDiagnosis">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPWHOStage" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlWHOStageDetail" CollapseControlID="pnlWHOStage"
                            ExpandControlID="pnlWHOStage" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgWHOStage">
                        </act:CollapsiblePanelExtender>
                        <%--<act:CollapsiblePanelExtender ID="CPEpnlStaging" runat="server" SuppressPostBack="true"
                                    ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlStagingDetail" CollapseControlID="pnlStaging"
                                    ExpandControlID="pnlStaging" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                    ImageControlID="ImgCPEStage">
                                </act:CollapsiblePanelExtender>--%>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr align="center">
                                        <td class="form">
                                            <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureExam" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="form">
                                            <asp:Button ID="btnSaveExam" runat="server" Text="Save" OnClick="btnSaveExam_Click" />
                                            <asp:Button ID="btncloseExam" Text="Close" runat="server" OnClick="btncloseExam_Click" />
                                            <asp:Button ID="btnPrintExam" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabpnlManagement" runat="server" Font-Size="Medium" HeaderText="Management">
                    <HeaderTemplate>
                        Management
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div class="border center formbg">
                            <table id="TabManagement" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="pnlDAToxities" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgDAToxities" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                Drug Allergies</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlDAToxitiesDetail" runat="server">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <UcDrugAllergies:Uc6 ID="UCDrugAllergies" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="pnlARVSideeffects" CssClass="border center formbg" runat="server"
                                            Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="ImgARVSideEffect" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                ARV Side Effects</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <%--                                     <asp:RadioButtonList ID="radbtnARVSideEffects" runat="server" RepeatDirection="Horizontal"
                                                                onclick="rblSelectedValue(this,'hideARVSideEffect');">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>--%>
                            <asp:Panel ID="pnlARVSideEffectDetail" runat="server">
                                <table width="100%" class="formbg" cellspacing="6" cellpadding="0" border="0">
                                    <tr>
                                        <td width="50%" class="border pad5 whitebg">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <label>
                                                            Short term effects:</label>
                                                        <div id="divShortTermEffects" class="customdivbordermultiselect" style="margin-bottom: 20px">
                                                            <asp:CheckBoxList ID="cblShortTermEffects" RepeatLayout="Flow" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <div id="divshorttermeffecttxt" style="display: none">
                                                            <label>
                                                                Specify other short term effects:</label>
                                                            <asp:TextBox ID="txtOtherShortTermEffects" runat="server" Skin="MetroTouch" Width="100%">
                                                            </asp:TextBox>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="50%" class="border pad5 whitebg">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <label>
                                                            Long term effects:</label>
                                                        <div id="divLongTermEffects" class="customdivbordermultiselect" style="margin-bottom: 20px">
                                                            <asp:CheckBoxList ID="cblLongTermEffects" RepeatLayout="Flow" runat="server">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                        <div id="divlongtermeffecttxt" style="display: none">
                                                            <label>
                                                                Specify other long term effects:</label>
                                                            <asp:TextBox ID="txtOtherLongtermEffects" runat="server" Skin="MetroTouch" Width="100%">
                                                            </asp:TextBox>
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
                                        <asp:Panel ID="PnlWupplan" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="Imgwupplan" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                Work up plan</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="PnlWupplandetails" runat="server">
                                <table width="100%" class="formbg" cellspacing="6" cellpadding="0" border="0">
                                    <tr>
                                        <td colspan="2" class="border pad5 whitebg">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <label>
                                                            Work up plan:</label>
                                                        <asp:TextBox ID="txtWorkUpPlan" runat="server" Skin="MetroTouch" Columns="20" Rows="2"
                                                            Width="100%" TextMode="MultiLine">
                                                        </asp:TextBox>
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
                                        <asp:Panel ID="pnllabEvaluation" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imglabevaluation" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                Lab Evaluation</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <%--                                       <asp:RadioButtonList ID="radbtnLabEvaluationAdult" runat="server" RepeatDirection="Horizontal"
                                                                    onclick="rblSelectedValue(this,'DIVLabEvaluation')">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <asp:CheckBoxList ID="cblSpecifyLabEvaluation" RepeatLayout="Flow" runat="server">
                                                                        </asp:CheckBoxList>
                                                                    <asp:TextBox ID="txtLabEvaluation" Width="200%" runat="server" Skin="MetroTouch">
                                                                    </asp:TextBox>
                            --%>
                            <asp:Panel ID="pnllabEvaluationDetail" runat="server">
                                <table width="100%" class="formbg" cellspacing="6" cellpadding="0" border="0">
                                    <tr>
                                        <td class="border pad5 whitebg">
                                            <uc11:Uclabeval ID="UcLabEval" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="pnlTreatment" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgtreatment" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                <asp:Label ID="lblheadregimenpresc" runat="server" Text="Treatment"></asp:Label>
                                            </h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlTreatmentdetail" runat="server" Style="padding: 6px">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <UcPharmacy:Uc7 ID="UCPharmacy" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="pnlOITreatment" CssClass="border center formbg" runat="server" Style="padding: 6px">
                                            <h2 class="forms" align="left">
                                                <asp:ImageButton ID="imgOITreatment" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                OI Treatment</h2>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlOITreatmentDetail" runat="server" Style="padding: 6px">
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td style='width: 50%' colspan="2">
                                            <table width='100%'>
                                                <tr>
                                                    <td style='width: 40%' align='right'>
                                                        <label>
                                                            OI Prophylaxis:</label>
                                                    </td>
                                                    <td style='width: 60%' align='left'>
                                                        <asp:DropDownList ID="rcbOIProphylaxis" runat="server" AutoPostBack="false" Width="130px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style='width: 50%'>
                                            <div id="DIVCotrimoxazole" style="display: none;">
                                                <table width='100%'>
                                                    <tr>
                                                        <td style='width: 50%' align='right'>
                                                            <label>
                                                                Cotrimoxazole prescribed for:</label>
                                                        </td>
                                                        <td style='width: 50%' align='left'>
                                                            <asp:DropDownList ID="rcbReasonCTXPrescribed" runat="server" AutoPostBack="false"
                                                                Width="130px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
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
                                </table>
                                <table class="border center whitebg" width="100%" style="margin-bottom: 6px;">
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <label>
                                                            Other treatment:</label>
                                                        <asp:TextBox ID="txtOtherTreatment" runat="server" Skin="MetroTouch" Width="99%"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <act:CollapsiblePanelExtender ID="CPLWupplan" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWupplandetails" CollapseControlID="PnlWupplan"
                            ExpandControlID="PnlWupplan" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="ImgWupplan">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEDAT" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlDAToxitiesDetail" CollapseControlID="pnlDAToxities"
                            ExpandControlID="pnlDAToxities" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="imgDAToxities">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEARVSE" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlARVSideEffectDetail"
                            CollapseControlID="pnlARVSideeffects" ExpandControlID="pnlARVSideeffects" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="true" ImageControlID="ImgARVSideEffect">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPELabEval" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnllabEvaluationDetail"
                            CollapseControlID="pnllabEvaluation" ExpandControlID="pnllabEvaluation" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="true" ImageControlID="imglabevaluation">
                        </act:CollapsiblePanelExtender>
                        <%--<act:CollapsiblePanelExtender ID="CPEPlan" runat="server" SuppressPostBack="true"
                                    ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlPlanDetail" CollapseControlID="pnlPlan"
                                    ExpandControlID="pnlPlan" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                    ImageControlID="imgPlan">
                                </act:CollapsiblePanelExtender>--%>
                        <act:CollapsiblePanelExtender ID="CPETreatment" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlTreatmentdetail" CollapseControlID="pnlTreatment"
                            ExpandControlID="pnlTreatment" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                            ImageControlID="imgtreatment">
                        </act:CollapsiblePanelExtender>
                        <act:CollapsiblePanelExtender ID="CPEOITreatment" runat="server" SuppressPostBack="true"
                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlOITreatmentDetail"
                            CollapseControlID="pnlOITreatment" ExpandControlID="pnlOITreatment" CollapsedImage="~/images/arrow-up.gif"
                            Collapsed="true" ImageControlID="imgOITreatment">
                        </act:CollapsiblePanelExtender>
                        <br />
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                <tbody>
                                    <tr align="center">
                                        <td class="form">
                                            <uc12:UserControlKNH_Signature ID="UserControlKNH_SignatureMgt" runat="server" />
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="form">
                                            <asp:Button ID="btnSaveManagement" runat="server" Text="Save" OnClick="btnSaveManagement_Click" />
                                            <asp:Button ID="btncloseMgt" Text="Close" runat="server" OnClick="btncloseMgt_Click" />
                                            <asp:Button ID="btnPrintManagement" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </act:TabPanel>
                <act:TabPanel ID="TabpnlPrev" runat="server" Font-Size="Larger" HeaderText="Prevention with Positives">
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
            <br />
            <div class="border center formbg" style="display: none">
                <table cellspacing="6" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr align="center">
                            <td class="form">
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btncomplete" runat="server" Text="Data Quality Check" />
                                <%--<asp:Button ID="btnback" runat="server" Text="Close" />--%>
                                <asp:Button ID="btnPrint" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <%--</ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncomplete"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>--%>
            <uc10:UserControlKNH_Extruder ID="UserControlKNH_Extruder1" runat="server" />
        </div>
</asp:Content>
