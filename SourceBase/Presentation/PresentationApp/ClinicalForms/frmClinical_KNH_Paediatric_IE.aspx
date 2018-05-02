<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/IQCare.master"
    CodeBehind="frmClinical_KNH_Paediatric_IE.aspx.cs" Inherits="PresentationApp.ClinicalForms.frmClinical_KNH_Paediatric_IE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="UcVitalSign" TagName="Uc1" Src="~/ClinicalForms/UserControl/UserControlKNH_VitalSigns.ascx" %>
<%@ Register TagPrefix="UcMedicalHistory" TagName="Uc2" Src="~/ClinicalForms/UserControl/UserControlKNH_MedicalHistory.ascx" %>
<%@ Register TagPrefix="UcPresentingComplaints" TagName="Uc3" Src="~/ClinicalForms/UserControl/UserControlKNHPresentingComplaints.ascx" %>
<%@ Register TagPrefix="UcPhysicalExamination" TagName="Uc4" Src="~/ClinicalForms/UserControl/UserControlKNH_PhysicalExamination.ascx" %>
<%@ Register TagPrefix="UcWhoStaging" TagName="Uc5" Src="~/ClinicalForms/UserControl/UserControlKNH_WHOStaging.ascx" %>
<%--Nidhi--%>
<%--<%@ Register TagPrefix="uc3" TagName="UserControlKNH_Extruder" Src="UserControl/UserControlKNH_Extruder.ascx" %>--%>
<%@ Register TagPrefix="UcTBScreening" TagName="Uc8" Src="~/ClinicalForms/UserControl/UserControlKNH_TBScreening.ascx" %>
<%@ Register TagPrefix="UcDrugAllergy" TagName="Uc6" Src="~/ClinicalForms/UserControl/UserControlKNH_DrugAllergies.ascx" %>
<%@ Register TagPrefix="UcLabEvalution" TagName="Uc7" Src="~/ClinicalForms/UserControl/UserControlKNH_LabEvaluation.ascx" %>
<%@ Register TagPrefix="UcPharmacy" TagName="Uc9" Src="~/ClinicalForms/UserControl/UserControlKNH_Pharmacy.ascx" %>
<%@ Register TagPrefix="UcPwP" TagName="Uc10" Src="~/ClinicalForms/UserControl/UserControlKNH_PwP.ascx" %>
<%@ Register Src="UserControl/UserControlKNH_Signature.ascx" TagName="UserControlKNHSignature"
    TagPrefix="UCSignature" %>
<%@ Register src="UserControl/UserControlKNH_BackToTop.ascx" tagname="UserControlKNH_BackToTop" tagprefix="uc12" %>
<%--Nidhi--%>
<%--<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>--%>
<asp:Content ID="content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
    <br />
    <div style="padding-left: 8px; padding-right: 8px; padding-top: 2px;">
        
        <script language="javascript" type="text/javascript">            buildWeeklyCalendar(0);</script>
        <script type="text/javascript" language="javascript">
//            $(function () {
//                $("#tabs").tabs();
//            });
            function ShowHide(theDiv, YN) {
                if (YN == "show") {
                    $("#" + theDiv).slideDown();

                }
                if (YN == "hide") {
                    $("#" + theDiv).slideUp();

                }
            }
            function rblSelectedValue1(val, divID, txtControlId) {
                var selectedvalue = $("#" + val.id + " input:radio:checked").val();
                var YN = "";
                if (selectedvalue == "1") {
                    YN = "show";
                }
                else {

                    document.getElementById(txtControlId).value = '';
                    YN = "hide";
                }
                ShowHide(divID, YN);
            }
            function rblSelectedValueShowHide(val, divID, txtControlId) {
                var selectedvalue = $("#" + val.id + " input:radio:checked").val();
                var YN = "";
                if (selectedvalue == "1") {
                    YN = "hide";
                }
                else {

                    document.getElementById(txtControlId).value = '';
                    YN = "show";
                }
                ShowHide(divID, YN);
            }
            function ShowMore(sender, eventArgs) {
                var substr = eventArgs._commandArgument.toString().split('|')
                ShowHide(substr[0], substr[1]);
            }
            function rblSelectedValueFordpList(val, divID, txtControlId) {
                var YN = "";
                var selectedvalue = $("#" + val.id + " input:radio:checked").val();
                if (selectedvalue == "1") {
                    YN = "show";
                }
                else {

                    document.getElementById(txtControlId).value = '0';
                    YN = "hide";
                }
                ShowHide(divID, YN);
            }

            function CheckBoxToggleShowHide(val, divID, txt, txtControlId) {

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
                            document.getElementById(txtControlId).value = '';
                            ShowHide(divID, "hide");
                        }
                    }
                }
            }

            function CheckBoxToggleShowHide1(val, divID, txt) {

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

            function fnControlUnableDesable(val, txt, dd) {
                var checkList = document.getElementById(val);
                var checkBoxList = checkList.getElementsByTagName("input");
                var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
                var checkBoxSelectedItems = new Array();

                for (var i = 0; i < checkBoxList.length; i++) {

                    if (checkBoxList[i].checked) {
                        if (arrayOfCheckBoxLabels[i].innerText == txt) {


                            $("#" + dd + " option:contains('Fluconazole')").attr('disabled', true);
                            $("#" + dd + " option:contains('CTX and Fluconazol')").attr('disabled', true);
                        }

                    }
                    else {
                        if (arrayOfCheckBoxLabels[i].innerText == txt) {

                            $("#" + dd + " option:contains('Fluconazole')").attr('disabled', false);
                            $("#" + dd + " option:contains('CTX and Fluconazol')").attr('disabled', false);
                        }
                    }
                }
            }


            function dropdownchange(ddl, theDiv, txtControlId) {

                if (ddl.selectedIndex == "1") {
                    ShowHide(theDiv, "show");
                }
                else {
                    document.getElementById(txtControlId).value = '0';
                    ShowHide(theDiv, "hide");
                }
            }

            function dropdownchangetext(ddl, theDiv, txt, txtControlId) {
                var e = document.getElementById(ddl);
                var strtxt = e.options[e.selectedIndex].innerText;
                if (strtxt == txt) {
                    ShowHide(theDiv, "show");

                }
                else {
                    document.getElementById(txtControlId).value = '';
                    ShowHide(theDiv, "hide");

                }
            }

            function fnReset(ctrlid, ctrltype) {
                if (ctrltype == "dd") {
                    document.getElementById(ctrlid).selectedIndex = 0;
                }
                else if (ctrltype == "txt") {
                    document.getElementById(ctrlid).value = "";
                }


            }

            function dropdownFluconazole(ddl, theDiv, txt) {
                var e = document.getElementById(ddl);
                var strtxt = e.options[e.selectedIndex].innerText;
                if (strtxt == 'Fluconazole' || strtxt == 'CTX and Fluconazol') {
                    ShowHide(theDiv, "show");

                }
                else {
                    ShowHide(theDiv, "hide");

                }
            }

            function fnalertTreatment(ddl) {
                if (document.getElementById('<%=hidupdate.ClientID %>').value == '1') {
                    var e = document.getElementById(ddl);
                    var strtxt = e.options[e.selectedIndex].innerText;
                    if (strtxt != 'Select') {
                        alert('you are changed the treatment');
                    }
                }
            }

            function fnalertregimen(ddl) {
                if (document.getElementById('<%=hidregimen.ClientID %>').value == '1') {
                    var e = document.getElementById(ddl);
                    var strtxt = e.options[e.selectedIndex].innerText;
                    if (strtxt != 'Select') {
                        alert('you are changed the regimen');
                    }
                }
            }
                
         
        </script>
        <%--<asp:ScriptManager ID="mst" runat="server">
        </asp:ScriptManager>--%>
        <%--<asp:UpdatePanel ID="UpdateMasterLink" runat="server">
            <ContentTemplate>--%>
                <table width="100%">
                    <tr>
                        <td class="border whitebg">
                            <table class="tbl-left">
                                <tr>
                                    <td align="center" class="data-lable">
                                        <label class="required">
                                            *Visit Date:</label>
                                        <%--<td align="left" class="data-control">--%>
                                        <input id="txtVisitDate" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                                            onfocus="javascript:vDateType='3'" maxlength="11" size="11" runat="server" type="text" />
                                        <img id="appDateimg1" onclick="w_displayDatePicker('<%=txtVisitDate.ClientID%>');"
                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" /><span class="smallerlabel"
                                                id="appDatespan1">(DD-MMM-YYYY)</span>
                                        <%--</td>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TabContainer ID="tabControl" runat="server" ActiveTabIndex="0">
                                <asp:TabPanel ID="TabTriage" runat="server" Font-Size="Medium" HeaderText="Triage">
                                    <HeaderTemplate>
                                        Triage
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="border center formbg">
                                            <div class="center formbg">
                                                <table class="leftallign formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
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
                                                                                            Client Information
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnlHivCareDetail" runat="server">
                                                    <table id="Triage" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 60%" align="right">
                                                                                <label>
                                                                                    Child accompanied by:
                                                                                </label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtchildaccompaniedby" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 50%" align="right">
                                                                                <label>
                                                                                    Child primary caregiver:
                                                                                </label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtchildcaregiver" runat="server"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 60%" align="right">
                                                                                <label>
                                                                                    Child diagnosis confirmed:</label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlchilddiagnosis" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 50%" align="right">
                                                                                <label>
                                                                                    Date of HIV Diagnosis:</label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <input id="txtdateofhivdiagnosis" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                    maxlength="11" size="11" runat="server" type="text" />
                                                                                &nbsp;&nbsp;
                                                                                &nbsp;
                                                                                <img id="Img19" onclick="w_displayDatePicker('<%=txtdateofhivdiagnosis.ClientID%>');"
                                                                                    height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                    border="0" name="appDateimg" style="vertical-align: text-bottom;" /><span class="smallerlabel"
                                                                                        id="Span19">(DD-MMM-YYYY)</span>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 20%" align="right">
                                                                                <label>
                                                                                    Disclosure status:</label>
                                                                            </td>
                                                                            <td style="width: 30%" align="left">
                                                                                <asp:DropDownList ID="ddldisclosurestatus" onchange="dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_ddldisclosurestatus','divreasonnotdisclosed','Not ready','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_txtspecifyreason');dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_ddldisclosurestatus','divotherdisclosurestatus','Other specify','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_txtspecifyotherdisclosurestatus');"
                                                                                    runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div id="divreasonnotdisclosed" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 51%" align="right">
                                                                                                    <label>
                                                                                                        Specify reason not disclosed:</label>
                                                                                                </td>
                                                                                                <td style="width: 60%" align="left">
                                                                                                    <asp:TextBox ID="txtspecifyreason" runat="server"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                                <div id="divotherdisclosurestatus" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 51%" align="right">
                                                                                                    <label>
                                                                                                        Specify other disclosure status:</label>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <asp:TextBox ID="txtspecifyotherdisclosurestatus" Width="60%" runat="server"></asp:TextBox>
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
                                                            <td class="border center pad5 whitebg" colspan="2" style="width: 100%">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 19%" align="right">
                                                                                <label>
                                                                                    Father alive:</label>
                                                                            </td>
                                                                            <td style="width: 32%" align="left">
                                                                                <asp:RadioButtonList ID="rbListFatherAlive" runat="server" RepeatDirection="Horizontal"
                                                                                    OnClick="rblSelectedValueShowHide(this,'divdatefatherdeath','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_txtdateoffatherdeath')">
                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divdatefatherdeath" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 50%" align="right">
                                                                                                    <label>
                                                                                                        Date of father's death:</label>
                                                                                                </td>
                                                                                                <td style="width: 60%" align="left">
                                                                                                    <input id="txtdateoffatherdeath" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                        onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                        maxlength="11" size="11" runat="server" type="text" />
                                                                                                    &nbsp;&nbsp;
                                                                                                    &nbsp;
                                                                                                    <img id="Img1" onclick="w_displayDatePicker('<%=txtdateoffatherdeath.ClientID%>');"
                                                                                                        height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                        border="0" name="appDateimg" style="vertical-align: text-bottom;" /><span class="smallerlabel"
                                                                                                            id="Span1">(DD-MMM-YYYY)</span>
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
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 19%" align="right">
                                                                                <label>
                                                                                    Mother alive:</label>
                                                                            </td>
                                                                            <td style="width: 36%" align="left">
                                                                                <asp:RadioButtonList ID="rbListMotherAlive" runat="server" RepeatDirection="Horizontal"
                                                                                    OnClick="rblSelectedValueShowHide(this,'divdateofmotherdeath','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_txtdateofmotherdeath')">
                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divdateofmotherdeath" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 46%" align="right">
                                                                                                    <label>
                                                                                                        Date of mother's death:</label>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <input id="txtdateofmotherdeath" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                        onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                        maxlength="11" size="11" runat="server" type="text" />
                                                                                                    &nbsp;&nbsp;
                                                                                                    &nbsp;
                                                                                                    <img id="Img22" onclick="w_displayDatePicker('<%=txtdateofmotherdeath.ClientID%>');"
                                                                                                        height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                        border="0" name="appDateimg" style="vertical-align: text-bottom;" /><span class="smallerlabel"
                                                                                                            id="Span22">(DD-MMM-YYYY)</span>
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
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 36%" align="right">
                                                                                <label>
                                                                                    Have you been referred from another facility:</label>
                                                                            </td>
                                                                            <td style="width: 15%" align="left">
                                                                                <asp:RadioButtonList ID="rbListRefFacility" runat="server" RepeatDirection="Horizontal"
                                                                                    OnClick="rblSelectedValue1(this,'divspecifyfacility','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_txtspecifyfacility')">
                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divspecifyfacility" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 50%" align="right">
                                                                                                    <label>
                                                                                                        Specify facility referred from:</label>
                                                                                                </td>
                                                                                                <td style="width: 60%" align="left">
                                                                                                    <asp:TextBox ID="txtspecifyfacility" runat="server"></asp:TextBox>
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
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td style="width: 22%" align="right">
                                                                                            <label>
                                                                                                Is the child on ART:</label>
                                                                                        </td>
                                                                                        <td style="width: 40%" align="left">
                                                                                            <asp:RadioButtonList ID="rbListChildART" runat="server" RepeatDirection="Horizontal"
                                                                                                OnClick="rblSelectedValue1(this,'divARTDate','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_txtcurrentartregimendate'); rblSelectedValueFordpList(this,'divcurrentregimenline','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_ddlcurrentartregimen');rblSelectedValueFordpList(this,'divARTRegimen','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_ddlcurrentregimenline');">
                                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <table width="100%">
                                                                                                <tr>
                                                                                                    <td style="width: 33%;">
                                                                                                        <div id="divcurrentregimenline" style="display: none;">
                                                                                                            <table width="100%" border="0">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td style="width: 22%" align="right">
                                                                                                                            <label>
                                                                                                                                Current regimen line:</label>
                                                                                                                        </td>
                                                                                                                        <td style="width: 10%" align="left">
                                                                                                                            <asp:DropDownList ID="ddlcurrentregimenline" runat="server">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                    <td style="width: 32%;">
                                                                                                        <div id="divARTRegimen" style="display: none;">
                                                                                                            <table width="100%" border="0" align="center">
                                                                                                                <tr>
                                                                                                                    <td style="width: 15%" align="right">
                                                                                                                        <label>
                                                                                                                            ART regimen:</label>
                                                                                                                    </td>
                                                                                                                    <td style="width: 15%" align="left">
                                                                                                                        <asp:DropDownList ID="ddlcurrentartregimen" runat="server">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <div id="divARTDate" style="display: none;">
                                                                                                            <table width="100%" border="0">
                                                                                                                <tr>
                                                                                                                    <td style="width: 14%" align="right">
                                                                                                                        <label>
                                                                                                                            Date:</label>
                                                                                                                    </td>
                                                                                                                    <td style="width: 30%" align="left">
                                                                                                                        <input id="txtcurrentartregimendate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                                            maxlength="11" size="11" runat="server" type="text" />
                                                                                                                        &nbsp; <img id="Img20" onclick="w_displayDatePicker('<%=txtcurrentartregimendate.ClientID%>');"
                                                                                                                            height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                                            border="0" name="appDateimg" style="vertical-align: text-bottom;" /><span class="smallerlabel"
                                                                                                                                id="Span20">(DD-MMM-YYYY)</span>
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
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border center pad5 whitebg">
                                                                <table width="100%" border="0" align="center">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 41%" align="right">
                                                                                <label>
                                                                                    Is the child on CTX:</label>
                                                                            </td>
                                                                            <td style="width: 60%" align="left">
                                                                                <asp:RadioButtonList ID="rbListChildCTX" runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                            <td class="border center pad5 whitebg">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 50%" align="right">
                                                                                <label>
                                                                                    Health education given:</label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:RadioButtonList ID="rbListHealthEducation" runat="server" RepeatDirection="Horizontal">
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
                                                            <td class="border center pad5 whitebg" colspan="2">
                                                                <div id="divschoolingstatus">
                                                                    <table width="100%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style="width: 21%" align="right">
                                                                                    <label>
                                                                                        Schooling Status:</label>
                                                                                </td>
                                                                                <td align="left" style="width: 20%">
                                                                                    <asp:DropDownList ID="ddlschoolingstatus" onchange="dropdownchange(this,'divhighestlevel','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_ddlhighestlevelattained');"
                                                                                        runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td>
                                                                                    <div id="divhighestlevel" style="display: none;">
                                                                                        <table width="100%" border="0">
                                                                                            <tr>
                                                                                                <td style="width: 60%" align="right">
                                                                                                    <label>
                                                                                                        Highest level attained:</label>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <asp:DropDownList ID="ddlhighestlevelattained" runat="server">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 35%" align="right">
                                                                                <label>
                                                                                    Is client a member of a support group:</label>
                                                                            </td>
                                                                            <td style="width: 18%" align="left">
                                                                                <asp:RadioButtonList ID="rbListSupportGroup" runat="server" RepeatDirection="Horizontal"
                                                                                    OnClick="rblSelectedValue1(this,'divsupportgroup','ctl00_IQCareContentPlaceHolder_tabControl_TabTriage_txthivsupportgroupmembership')">
                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divsupportgroup" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 49%" align="right">
                                                                                                    <label>
                                                                                                        HIV support group membership:</label>
                                                                                                </td>
                                                                                                <td style="width: 60%" align="left">
                                                                                                    <asp:TextBox ID="txthivsupportgroupmembership" Width="150px" runat="server"></asp:TextBox>
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
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlvitalsign" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgvitalsign" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            <asp:Literal ID="literalVitalSign" Text="Vital Signs" runat="server"></asp:Literal>
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                                        </div>
                                        <asp:CollapsiblePanelExtender ID="CPEHIVCare" runat="server" SuppressPostBack="True"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlHivCareDetail" CollapseControlID="pnlHIVCare"
                                            ExpandControlID="pnlHIVCare" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
                                            ImageControlID="imgHIVCare" Enabled="True">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CPEVitalSign" runat="server" SuppressPostBack="True"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlVitalSignDetail" CollapseControlID="pnlVitalSign"
                                            ExpandControlID="pnlVitalSign" CollapsedImage="~/images/arrow-up.gif" Collapsed="True"
                                            ImageControlID="imgVitalSign" Enabled="True">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <div class="border center formbg">
                                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="form" align="center">
                                                            <UCSignature:UserControlKNHSignature ID="UserControlKNHSignature_Triage" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="tblSaveButton" align="center" valign="top">
                                                        <td class="form">
                                                            <asp:Button ID="btnSaveTriage" runat="server" Text="Save" OnClick="btnSaveTriage_Click" />
                                                            <asp:Button ID="btnCloseTriage" runat="server" Text="Close" OnClick="btnCloseTriage_Click" />
                                                            <asp:Button ID="btnPrintTriage" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="tblDeleteButton" style="display: none" align="center">
                                                        <td align="center" class="form" width="100%">
                                                            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" 
                                                                Text="Delete" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabClinicalHistory" runat="server" Font-Size="Medium" HeaderText="Clinical History">
                                    <HeaderTemplate>
                                        Clinical History
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="border center formbg">
                                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <div class="center formbg">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlpresenticomplains" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgpresenticomplains" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            <asp:Literal ID="literPresenting" Text="Presenting Complaints" runat="server"></asp:Literal>
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" width="100%">
                                                            <asp:Panel ID="pnltargetpresentingcomplain" runat="server">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <UcPresentingComplaints:Uc3 ID="UcPc" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <%--nidhi--%>
                                                                    <tr align="left">
                                                                        <td class="border pad5 whitebg">
                                                                            <div id="divschoolperformance">
                                                                                <table width="100%" align="left">
                                                                                    <tr align="left">
                                                                                        <td>
                                                                                            <label id="Label155">
                                                                                                If schooling,current school perfomance:
                                                                                            </label>
                                                                                            <asp:DropDownList ID="ddlschoolperfomance" runat="server">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="display: none;">
                                                                        <td style="padding-left: 12px;" class="border pad6 whitebg" width="100%" align="left"
                                                                            colspan="2">
                                                                            <label>
                                                                                Presenting complaints additional notes:
                                                                            </label>
                                                                            <asp:TextBox ID="txtAdditionPresentingComplaints" runat="server" TextMode="MultiLine"
                                                                                Width="98.5%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                            </table>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="headermedicalhistory" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgheadermedicalhistory" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            <asp:Literal ID="literalMedicalHistory" Text="Medical History (Disease, Diagnosis and Treatment)"
                                                                                                runat="server"></asp:Literal>
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnlmadicalhistory" runat="server">
                                                    <table id="Table1" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" colspan="2" align="left">
                                                                <table width="100%" style="padding-left: 10px;">
                                                                    <tr>
                                                                        <td valign="top" colspan="2">
                                                                            <label>
                                                                                Specify Medical history:
                                                                            </label>
                                                                            <br />
                                                                            <asp:TextBox runat="server" ID="txtspecifymedicalhistory" TextMode="MultiLine" Width="98.5%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td style="width: 50%" align="left">
                                                                            <label class="required" for="chkLTMedications">
                                                                                <asp:Label ID="lblHeadingChronic" Text="*Chronic condition:" runat="server"></asp:Label>
                                                                            </label>
                                                                            <div class="customdivbordermultiselect">
                                                                                <asp:CheckBoxList ID="cblchroniccondition" RepeatLayout="Flow" onclick="CheckBoxToggleShowHide('ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_cblchroniccondition','hideOtherLTM','Other specify','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtotherchroniccondition');"
                                                                                    Width="190px" runat="server">
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 50%; display: none;" align="left" id="hideOtherLTM">
                                                                            <label id="Label20" for="txtotherchroniccondition">
                                                                                Specify other chronic condition:</label><br />
                                                                            <asp:TextBox runat="server" ID="txtotherchroniccondition" Width="99%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td class="border pad6 whitebg" width="100%" align="left" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td width="50%;" style="padding-left: 1%;">
                                                                            <label>
                                                                                Chronic condition :</label>
                                                                            <br />
                                                                            <div id="divchroniccondition" enableviewstate="true" runat="server">
                                                                                <div class="customdivbordermultiselect" id="divPresenting" nowrap="noWrap">
                                                                                    <asp:CheckBoxList ID="cblchroniccondition" RepeatLayout="Flow" onclick="CheckBoxToggleShowHide('ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_cblchroniccondition','divothercondition','Other specify');"
                                                                                        runat="server">
                                                                                    </asp:CheckBoxList>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 2%;">
                                                                        </td>
                                                                        <td valign="middle">
                                                                            <div id="divothercondition" style="display: none;">
                                                                                <label id="Label20" for="WHOStage">
                                                                                    Specify other chronic condition :</label>
                                                                                <asp:TextBox runat="server" ID="txtotherchroniccondition" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td style="width: 15%" align="left">
                                                                            <label>
                                                                                Previously admitted:</label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:RadioButtonList ID="rbListPreviouslyAdmitt" runat="server" RepeatDirection="Horizontal"
                                                                                OnClick="rblSelectedValue1(this,'divdiagnosis','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtdiagnosis');rblSelectedValue1(this,'divadmissionstart','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtadmissionstart');rblSelectedValue1(this,'divadmissionend','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtadmissionend');">
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <%--  <table width="100%" border="1">
                                                                    <tr>
                                                                        <td colspan="2">--%>
                                                                <table border="0" width="100%">
                                                                    <tr>
                                                                        <td style="width: 25%">
                                                                            <div id="divdiagnosis" style="display: none;">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="left">
                                                                                                <label>
                                                                                                    Diagnosis:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:TextBox runat="server" ID="txtdiagnosis"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                        <td style="width: 38%">
                                                                            <div id="divadmissionstart" style="display: none;">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 30%" align="right">
                                                                                                <label>
                                                                                                    Admission start:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtadmissionstart" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img2" onclick="w_displayDatePicker('<%=txtadmissionstart.ClientID%>');"
                                                                                                    height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                    border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span2">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divadmissionend" style="display: none;">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 30%" align="right">
                                                                                                <label>
                                                                                                    Admission end:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtadmissionend" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img3" onclick="w_displayDatePicker('<%=txtadmissionend.ClientID%>');" height="22 "
                                                                                                    alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                    name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span3">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <%--   </td>
                                                                    </tr>
                                                                </table>--%>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none;">
                                                            <td colspan="2">
                                                                <table>
                                                                    <tr>
                                                                        <td class="border pad6 whitebg" width="50%" align="center">
                                                                            <label id="Label26" class="">
                                                                                Medical history:
                                                                            </label>
                                                                            <input id="rdomedicalhistoryyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);"
                                                                                type="radio" name="medicalhistoryyes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rdomedicalhistoryno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);"
                                                                                type="radio" name="medicalhistoryyes" runat="server" />
                                                                            <label>
                                                                                No</label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheadertbhistory" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgpnlheadertbhistory" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            TB History
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnltbhistory" runat="server">
                                                    <table id="Table2" class="formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 9%" align="right">
                                                                                <label>
                                                                                    TB History:</label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <%-- <asp:RadioButtonList ID="rbListTBHistory" onclick="rblSelectedValue2(this,'divcompletetxdate','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtcompletetxdate');rblSelectedValue2(this,'divretreatmentdate','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtretreatmentdate');"
                                                                                    runat="server" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Text="Yes" Value="1" ></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                            </asp:RadioButtonList>--%>
                                                                                <asp:RadioButtonList ID="rbListTBHistory" runat="server" RepeatDirection="Horizontal"
                                                                                    OnClick="rblSelectedValue1(this,'divcompletetxdate','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtcompletetxdate');rblSelectedValue1(this,'divretreatmentdate','ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_txtretreatmentdate')">
                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div id="divcompletetxdate" style="display: none;">
                                                                                    <table width="100%" border="0">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 40%" align="left">
                                                                                                    <label>
                                                                                                        Complete TX Date:</label>
                                                                                                </td>
                                                                                                <td style="width: 60%" align="left">
                                                                                                    <input id="txtcompletetxdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                        onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                        maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                    <img id="Img4" onclick="w_displayDatePicker('<%=txtcompletetxdate.ClientID%>');"
                                                                                                        height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                        border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                    <span class="smallerlabel" id="Span4">(DD-MMM-YYYY)</span>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divretreatmentdate" style="display: none;">
                                                                                    <label id="Label28" class="margin35" for="District">
                                                                                        Retreatment Date:</label>
                                                                                    <input id="txtretreatmentdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                        onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                        maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                    <img id="Img34" onclick="w_displayDatePicker('<%=txtretreatmentdate.ClientID%>');"
                                                                                        height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                        border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                    <span class="smallerlabel" id="Span34">(DD-MMM-YYYY)</span>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left" class="formbg">
                                                                <br />
                                                                <h2 class="forms" align="left">
                                                                    Immunisation Status</h2>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" colspan="2" align="center">
                                                                <label id="Label29">
                                                                    Immunisation Status:</label>
                                                                <asp:DropDownList ID="ddlimmunisationstatus" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <%--  <td class="border pad6 whitebg" width="50%" align="center">
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderarvhistory" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgpnlheaderarvhistory" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            ARV Exposure
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnlarvhistory" runat="server">
                                                    <table id="Table3" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td width="100%" colspan="2">
                                                                <%--  <table width="100%;" class=" center">
                                                                    <tr align="left">
                                                                        <td>--%>
                                                                <%-- <label id="Label30" class="required" style="padding-left: 1%;">
                                                                                *ARV exposure:
                                                                            </label>
                                                                            <input id="rdoarvexposureyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divarvexposure');"
                                                                                type="radio" name="arvexposureyes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rdoarvexposureno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divarvexposure');"
                                                                                type="radio" name="arvexposureyes" runat="server" checked />
                                                                            <label>
                                                                                No</label>--%>
                                                                <div id="divarvexposure">
                                                                    <table width="100%" class="border center whitebg" style="margin-bottom: 6px;">
                                                                        <tr>
                                                                            <td style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    PMTCT Regimen:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:TextBox runat="server" ID="txtpmtctregimen"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Start Date:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtpmtctdate" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onfocus="javascript:vDateType='3'" maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img35" onclick="w_displayDatePicker('<%=txtpmtctdate.ClientID%>');" height="22 "
                                                                                                    alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                    name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span35">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="100%" class="border center whitebg" style="margin-bottom: 6px;">
                                                                        <tr>
                                                                            <td style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    PEP Regimen:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:TextBox runat="server" ID="txtpepregimen"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Start Date:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtpepstartdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img37" onclick="w_displayDatePicker('<%=txtpepstartdate.ClientID%>');" height="22 "
                                                                                                    alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                    name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span37">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table width="100%" class="border center whitebg" style="margin-bottom: 6px;">
                                                                        <tr>
                                                                            <td style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    HAART Regimen:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:TextBox runat="server" ID="txthaartregimen"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Start Date:</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txthaartstartdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img36" onclick="w_displayDatePicker('<%=txthaartstartdate.ClientID%>');"
                                                                                                    height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                    border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span36">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <%--  </td>
                                                                    </tr>
                                                                </table>--%>
                                                                <%--  <table width="100%;" align="left">
                                                                    <tr>
                                                                        <td align="left">
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </table>--%>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none;">
                                                            <td class="border pad6 whitebg" width="100%" colspan="2" align="center">
                                                                <table width="100%;">
                                                                    <tr align="left" class="whitebg border pad6">
                                                                        <td>
                                                                            <label id="Label37" style="padding-left: 1%;" for="District">
                                                                                HIV related history:</label>
                                                                            <asp:DropDownList ID="ddlhivhistory" onchange="dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabClinicalHistory_ddlhivhistory','divhivrelatedhistory','Yes');"
                                                                                runat="server">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100%;">
                                                                            <div id="divhivrelatedhistory" style="display: none;">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlHeaderHIV" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgHIVHistory" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            HIV Related History
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="Panel1" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="pad5">
                                                                <table width="100%" border="0" class="border center whitebg" style="margin-bottom: 6px;">
                                                                    <tr>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%" border="0">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align="left" style="width: 57%;">
                                                                                            <label>
                                                                                                Initial CD4:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <label>
                                                                                                Count: &nbsp;</label><asp:TextBox runat="server" Width="50%" ID="txtcd4"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align="right" style="width: 40%;">
                                                                                            <label>
                                                                                                Percent:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox runat="server" Width="50%" ID="txtcd4per"></asp:TextBox>%
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align="right" style="width: 18%;">
                                                                                            <label>
                                                                                                Date:</label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <input id="txtcd4date" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                                                                                                onfocus="javascript:vDateType='3'" maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                            <img id="Img38" onclick="w_displayDatePicker('<%=txtcd4date.ClientID%>');" height="22 "
                                                                                                alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                            <span class="smallerlabel" id="Span38">(DD-MMM-YYYY)</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" class="border center whitebg" style="margin-bottom: 6px;" border="0">
                                                                    <tr>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 57%" align="left">
                                                                                            <label>
                                                                                                Highest CD4 ever:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <label>
                                                                                                Count: &nbsp;</label><asp:TextBox runat="server" Width="50%" ID="txthighCD4ever"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 40%" align="right">
                                                                                            <label>
                                                                                                Percent:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox runat="server" Width="50%" ID="txthighestcd4everper"></asp:TextBox>%
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 18%" align="right">
                                                                                            <label>
                                                                                                Date:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <input id="txthigcd4everdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                            <img id="Img39" onclick="w_displayDatePicker('<%=txthigcd4everdate.ClientID%>');"
                                                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                            <span class="smallerlabel" id="Span39">(DD-MMM-YYYY)</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" class="border center whitebg" style="margin-bottom: 6px;">
                                                                    <tr>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 57%" align="left">
                                                                                            <label>
                                                                                                Most recent CD4:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <label>
                                                                                                Count: &nbsp;</label><asp:TextBox runat="server" Width="50%" ID="txtmostrecent_cd4"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 40%" align="right">
                                                                                            <label>
                                                                                                Percent:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox runat="server" Width="50%" ID="txtmostrecentcd4per"></asp:TextBox>%
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 13%" align="right">
                                                                                            <label>
                                                                                                Date:</label>
                                                                                        </td>
                                                                                        <td style="width: 60%" align="left">
                                                                                            <input id="txtmostrecentcd4date" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                            <img id="Img41" onclick="w_displayDatePicker('<%=txtmostrecentcd4date.ClientID%>');"
                                                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                            <span class="smallerlabel" id="Span41">(DD-MMM-YYYY)</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" class="border center whitebg" style="margin-bottom: 6px;" border="0">
                                                                    <tr>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 57%" align="left">
                                                                                            <label>
                                                                                                CD4 at ART initiation:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <label>
                                                                                                Count: &nbsp;</label><asp:TextBox runat="server" Width="50%" ID="txtcd4artinitiation"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 40%" align="right">
                                                                                            <label>
                                                                                                Percent:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox runat="server" Width="50%" ID="txtcd4artinitper"></asp:TextBox>%
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 13%" align="right">
                                                                                            <label>
                                                                                                Date:</label>
                                                                                        </td>
                                                                                        <td style="width: 60%" align="left">
                                                                                            <input id="txtcd4artinitdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                            <img id="Img40" onclick="w_displayDatePicker('<%=txtcd4artinitdate.ClientID%>');"
                                                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                            <span class="smallerlabel" id="Span40">(DD-MMM-YYYY)</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table class="border center whitebg" style="margin-bottom: 6px;" width="100%">
                                                                    <tr>
                                                                        <td style="width: 35%;">
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 57%" align="left">
                                                                                            <label>
                                                                                                Previous viral load:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox runat="server" Width="50%" ID="txtpreviousviral_load"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 23%;">
                                                                        </td>
                                                                        <td>
                                                                            <table width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td style="width: 43%" align="right">
                                                                                            <label>
                                                                                                Date:</label>
                                                                                        </td>
                                                                                        <td style="width: 60%" align="left">
                                                                                            <input id="txtpreviousviralloaddate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                            <img id="Img42" onclick="w_displayDatePicker('<%=txtpreviousviralloaddate.ClientID%>');"
                                                                                                height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                            <span class="smallerlabel" id="Span42">(DD-MMM-YYYY)</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td class="border pad6 whitebg" width="100%" colspan="2">
                                                                            <table width="100%;">
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <label id="Label52" class="">
                                                                                            Other HIV related history:<br />
                                                                                        </label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="txtotherhivrelated_history" Width="100%"></asp:TextBox>
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
                                        </div>
                                        <asp:CollapsiblePanelExtender ID="cpeprecomplain" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnltargetpresentingcomplain"
                                            CollapseControlID="pnlpresenticomplains" ExpandControlID="pnlpresenticomplains"
                                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="imgpresenticomplains">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="cpemedicalhistory" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlmadicalhistory" CollapseControlID="headermedicalhistory"
                                            ExpandControlID="headermedicalhistory" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgheadermedicalhistory">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="cpetbhistory" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnltbhistory" CollapseControlID="pnlheadertbhistory"
                                            ExpandControlID="pnlheadertbhistory" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                            ImageControlID="imgpnlheadertbhistory">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="cpearvhistory" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlarvhistory" CollapseControlID="pnlheaderarvhistory"
                                            ExpandControlID="pnlheaderarvhistory" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgpnlheaderarvhistory">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="Panel1" CollapseControlID="pnlHeaderHIV"
                                            ExpandControlID="pnlHeaderHIV" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                            ImageControlID="imgHIVHistory">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <div class="border center formbg">
                                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="form" align="center">
                                                            <UCSignature:UserControlKNHSignature ID="UserControlKNHSignature_ClinicalHistory"
                                                                runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr align="center" valign="top">
                                                        <td class="form">
                                                            <asp:Button ID="btnSaveCHistory" runat="server" Text="Save" OnClick="btnSaveCHistory_Click" />
                                                            <asp:Button ID="btnCloseHistory" runat="server" Text="Close" OnClick="btnCloseHistory_Click" />
                                                            <%--<asp:Button ID="btnSubmitCHistory" runat="server" Text="Data Quality Check" OnClick="btnSubmitCHistory_Click" />--%>
                                                            <asp:Button ID="btnPrintCHistory" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabTBSCreening" runat="server" Font-Size="Medium" HeaderText="TB Screening">
                                    <HeaderTemplate>
                                        TB Screening
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="border center formbg">
                                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <tr valign="top">
                                                    <td colspan="2" width="100%">
                                                        <asp:Panel ID="Panel3" runat="server">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <UcTBScreening:Uc8 ID="UcTBScreening" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <%--  <div class="border center formbg">
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border center formbg">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheadertbscreening" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ImgTBScreening" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            TB Screening ICF(2 signs & 2 symptoms - TB likely)
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnltbscreening" runat="server">
                                                    <table id="Table4" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" colspan="2" align="center">
                                                                <div id="divtbassessmentshowhide">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    TB Findings :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:DropDownList ID="ddltbfinding" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Sputum smear :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:DropDownList ID="ddlsputum_smear" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Tissue Biopsy :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:DropDownList ID="ddltissuebiopsy" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Chest X ray results :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:DropDownList ID="ddlchestxrayresults" runat="server">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Other CXR (specify) :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:TextBox runat="server" ID="txtothercxrspecify"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Tissue Biopsy results :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <asp:TextBox runat="server" ID="txttissuebiopsyresults"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="border pad6 whitebg" width="100%" colspan="2" align="left">
                                                                                <label id="Label54" style="padding-left: 2%;" for="District">
                                                                                    TB Assessment :</label>
                                                                                <div class="checkbox" id="divtbassessment" nowrap="noWrap">
                                                                                    <asp:CheckBoxList ID="cbltbassessment" RepeatLayout="Flow" runat="server">
                                                                                    </asp:CheckBoxList>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border center formbg">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheadertbevaluationtp" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgtbevaluation" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            TB Evaluation and Treatment Plan
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnltbevaluation" runat="server">
                                                    <table id="Table5" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 40%" align="right">
                                                                                <label>
                                                                                    TB Type :</label>
                                                                            </td>
                                                                            <td style="width: 60%" align="left">
                                                                                <asp:DropDownList ID="ddltbtype" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 40%" align="right">
                                                                                <label>
                                                                                    Patient type :</label>
                                                                            </td>
                                                                            <td style="width: 60%" align="left">
                                                                                <asp:DropDownList ID="ddlpatienttype" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 20%" align="right">
                                                                                <label>
                                                                                    TB plan :</label>
                                                                            </td>
                                                                            <td style="width: 32%" align="left">
                                                                                <asp:DropDownList ID="ddltbplan" onchange="dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabTBSCreening_ddltbplan','divtbplanshowhine','Other (Specify)');"
                                                                                    runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divtbplanshowhine" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 38%" align="right">
                                                                                                    <label>
                                                                                                        Specify Other TB plan :</label>
                                                                                                </td>
                                                                                                <td style="width: 60%" align="left">
                                                                                                    <asp:TextBox runat="server" ID="txtothertbplan"></asp:TextBox>
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
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 20%" align="right">
                                                                                <label>
                                                                                    TB regimen :</label>
                                                                            </td>
                                                                            <td style="width: 32%" align="left">
                                                                                <asp:DropDownList ID="ddltbregimen" onchange="dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabTBSCreening_ddltbregimen','divothertbregimen','Other');"
                                                                                    runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divothertbregimen" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 39%" align="right">
                                                                                                    <label>
                                                                                                        Other TB regimen :</label>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <asp:TextBox runat="server" ID="txtothertbregimen"></asp:TextBox>
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
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 40%" align="right">
                                                                                <label>
                                                                                    TB regimen start date :</label>
                                                                            </td>
                                                                            <td style="width: 60%" align="left">
                                                                                <input id="txttbregimentstartdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                <img id="Img5" onclick="w_displayDatePicker('<%=txttbregimentstartdate.ClientID%>');"
                                                                                    height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                    border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                <span class="smallerlabel" id="Span5">(DD-MMM-YYYY)</span>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 40%" align="right">
                                                                                <label>
                                                                                    TB regimen end date :</label>
                                                                            </td>
                                                                            <td style="width: 60%" align="left">
                                                                                <input id="txttbregimentenddate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                <img id="Img6" onclick="w_displayDatePicker('<%=txttbregimentenddate.ClientID%>');"
                                                                                    height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                    border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                <span class="smallerlabel" id="Span6">(DD-MMM-YYYY)</span>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border pad6 whitebg" colspan="2" width="100%" align="center">
                                                                <label id="Label71" class="">
                                                                    TB treatment outcome :
                                                                </label>
                                                                <asp:DropDownList ID="ddltbtreatmentoutcome" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border center formbg">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderIPT" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="ImgIPT" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            IPT (Patients with no signs and symptoms)
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnlIPTDetails" runat="server">
                                                    <table id="Table6" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr align="left">
                                                            <td class="border pad6 whitebg" width="100%" colspan="2">
                                                                <label id="Label72" class="" style="padding-left: 11%;">
                                                                    INH Started :
                                                                </label>
                                                                <input id="rdoinhstartedyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divinhstartedshowhide');"
                                                                    type="radio" name="inhstartedyes" runat="server" />
                                                                <label>
                                                                    Yes</label>
                                                                <input id="rdoinhstartedno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divinhstartedshowhide');"
                                                                    type="radio" name="inhstartedyes" runat="server" />
                                                                <label>
                                                                    No</label><br />
                                                                <div id="divinhstartedshowhide" style="display: none;">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td class="border center pad5 whitebg" colspan="2" style="width: 100%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="left">
                                                                                                <label>
                                                                                                    If yes,stop reason :</label>
                                                                                                <div class="customdivbordermultiselect" id="divstopreason" nowrap="noWrap">
                                                                                                    <asp:CheckBoxList ID="cblstopreason" RepeatLayout="Flow" runat="server">
                                                                                                    </asp:CheckBoxList>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    INH Start Date :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtinhstartdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img7" onclick="w_displayDatePicker('<%=txtinhstartdate.ClientID%>');" height="22 "
                                                                                                    alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                    name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span7">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    INH End Date :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtinhenddate" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onfocus="javascript:vDateType='3'" maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img8" onclick="w_displayDatePicker('<%=txtinhenddate.ClientID%>');" height="22 "
                                                                                                    alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                    name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span8">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Pyriodoxine Start Date :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtpyriodoxinestartdate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img9" onclick="w_displayDatePicker('<%=txtpyriodoxinestartdate.ClientID%>');"
                                                                                                    height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                    border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span9">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                            <td class="border center pad5 whitebg" style="width: 50%">
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td style="width: 40%" align="right">
                                                                                                <label>
                                                                                                    Pyriodoxine End Date :</label>
                                                                                            </td>
                                                                                            <td style="width: 60%" align="left">
                                                                                                <input id="txtpyriodoxineenddate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="txtdtLMP" runat="server" />
                                                                                                <img id="Img10" onclick="w_displayDatePicker('<%=txtpyriodoxineenddate.ClientID%>');"
                                                                                                    height="22 " alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                                                                    border="0" name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span10">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" colspan="2" align="center">
                                                                <label id="Label79" class="margin35" for="District">
                                                                    Has adherence been addressed?</label>
                                                                <input id="rdohasadherenceassessedyes" onmouseup="up(this);" onfocus="up(this);"
                                                                    onclick="down(this);" type="radio" name="hasadherenceassessedyes" runat="server"
                                                                    checked />
                                                                <label>
                                                                    Yes</label>
                                                                <input id="rdohasadherenceassessedno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);"
                                                                    type="radio" name="hasadherenceassessedyes" runat="server" />
                                                                <label>
                                                                    No</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" align="center" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td style="width: 20%;" align="right">
                                                                            <label>
                                                                                Any missed doses :
                                                                            </label>
                                                                        </td>
                                                                        <td style="width: 25%;">
                                                                            <input id="rdoanymisseddosesyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divreferredforadherence');"
                                                                                type="radio" name="anymisseddosesyes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rdoanymisseddosesno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divreferredforadherence');"
                                                                                type="radio" name="anymisseddosesyes" runat="server" />
                                                                            <label>
                                                                                No</label>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divreferredforadherence" style="display: none;">
                                                                                <label id="Label81" class="margin35" for="District">
                                                                                    If yes referred for adherence :
                                                                                </label>
                                                                                <input id="rdoyesreferredadherenceyes" onmouseup="up(this);" onfocus="up(this);"
                                                                                    onclick="down(this);" type="radio" name="yesreferredadherenceyes" runat="server" />
                                                                                <label>
                                                                                    Yes</label>
                                                                                <input id="rdoyesreferredadherenceno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);"
                                                                                    type="radio" name="yesreferredadherenceyes" runat="server" />
                                                                                <label>
                                                                                    No</label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="border pad6 whitebg" width="50%" align="center">
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border center pad5 whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <label>
                                                                                    Review checklist :</label>
                                                                                <div class="customdivbordermultiselect" id="divreviewchecklist" nowrap="noWrap">
                                                                                    <asp:CheckBoxList ID="cblreviewlist" RepeatLayout="Flow" onclick="CheckBoxToggleShowHide1('ctl00_IQCareContentPlaceHolder_tabControl_TabTBSCreening_cblreviewlist','divothertbshowhide','Other Side effects (specify)');"
                                                                                        Width="185px" runat="server">
                                                                                    </asp:CheckBoxList>
                                                                                </div>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divothertbshowhide" style="display: none;">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 55%" align="left">
                                                                                                    <label>
                                                                                                        Specify other TB side effects :</label><br />
                                                                                                    <asp:TextBox ID="othertbsideeffects" Width="100%" runat="server"></asp:TextBox>
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
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border center formbg">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlconfirmtbsuspect" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgconfirmtb" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            Confirmed or TB Suspected
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnlconfirmtbsuspected" runat="server">
                                                    <table id="Table7" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td width="100%" class="border pad5 whitebg">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td width="35%;" align="right">
                                                                            <label>
                                                                                Confirmed or Suspected TB (Stop INH) :</label>
                                                                        </td>
                                                                        <td width="15%;">
                                                                            <input id="rdostopINHYes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divstopinhdateshowhide');"
                                                                                type="radio" name="stopINHYes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rdostopINHno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divstopinhdateshowhide');fnReset('ctl00_IQCareContentPlaceHolder_tabControl_TabTBSCreening_dtStopINHDate','txt');"
                                                                                type="radio" name="stopINHYes" runat="server" checked />
                                                                            <label>
                                                                                No</label>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divstopinhdateshowhide" style="display: none;">
                                                                                <table class="tbl-left">
                                                                                    <tr>
                                                                                        <td align="right" style="width: 38%;" class="data-lable">
                                                                                            <label>
                                                                                                Stop INH Date:</label>
                                                                                        </td>
                                                                                        <td align="left" class="data-control">
                                                                                            <input id="dtStopINHDate" onblur="DateFormat(this,this.value,event,false,'3')" onkeyup="DateFormat(this,this.value,event,false,'3')"
                                                                                                onfocus="javascript:vDateType='3'" maxlength="11" size="11" name="VisitDate"
                                                                                                runat="server" />
                                                                                            <img id="Img14" onclick="w_displayDatePicker('<%=dtStopINHDate.ClientID%>');" height="22 "
                                                                                                alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                            <span class="smallerlabel" id="Span14">(DD-MMM-YYYY)</span>
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
                                                            <td width="100%" class="border pad5 whitebg">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td width="35%;" align="right">
                                                                            <label>
                                                                                Household contacts screened for TB :</label>
                                                                        </td>
                                                                        <td width="15%;">
                                                                            <input id="rdohouseholdtbyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divhouseholdshowhide');fnReset('ctl00_IQCareContentPlaceHolder_tabControl_TabTBSCreening_txtTBNotScreenedSpecify','txt');"
                                                                                type="radio" name="householdtbyes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rdohouseholdtbno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divhouseholdshowhide');"
                                                                                type="radio" name="householdtbyes" runat="server" checked />
                                                                            <label>
                                                                                No</label>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divhouseholdshowhide" style="display: none;">
                                                                                <table class="tbl-left">
                                                                                    <tr>
                                                                                        <td align="right" style="width: 38%;" class="data-lable">
                                                                                            <label>
                                                                                                If No specify why :</label>
                                                                                        </td>
                                                                                        <td align="left" class="data-control">
                                                                                            <asp:TextBox ID="txtTBNotScreenedSpecify" runat="server" Width="50%">
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
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                        </div>--%>
                                        <%--  <asp:CollapsiblePanelExtender ID="cpetbscreening" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnltbscreening" CollapseControlID="pnlheadertbscreening"
                                            ExpandControlID="pnlheadertbscreening" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="ImgTBScreening">
                                        </asp:CollapsiblePanelExtender>--%>
                                        <%-- <asp:CollapsiblePanelExtender ID="CPETBEvaluationTP" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnltbevaluation" CollapseControlID="pnlheadertbevaluationtp"
                                            ExpandControlID="pnlheadertbevaluationtp" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgtbevaluation">
                                        </asp:CollapsiblePanelExtender>--%>
                                        <%--  <asp:CollapsiblePanelExtender ID="CPEIPT" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlIPTDetails" CollapseControlID="pnlheaderIPT"
                                            ExpandControlID="pnlheaderIPT" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                            ImageControlID="ImgIPT">
                                        </asp:CollapsiblePanelExtender>--%>
                                        <%--  <asp:CollapsiblePanelExtender ID="CPEConfirmTB" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlconfirmtbsuspected"
                                            CollapseControlID="pnlconfirmtbsuspect" ExpandControlID="pnlconfirmtbsuspect"
                                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="imgconfirmtb">
                                        </asp:CollapsiblePanelExtender>--%>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabExamination" runat="server" Font-Size="Medium" HeaderText="Examination">
                                    <HeaderTemplate>
                                        Examination
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="border center formbg">
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderlongtermmedication" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imglongtermmedication" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            <asp:Literal ID="literalCurrentLong" Text="Current Long Term Medications" runat="server"></asp:Literal>
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnllongtermmedication" runat="server">
                                                    <table id="Table8" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td class="border whitebg" style="width: 100%" colspan="2">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td style="width: 50%" align="left">
                                                                            <%-- <label class="required" for="chkLTMedications">
                                                                                </label>--%>
                                                                            <asp:Label ID="lblLongTermText" CssClass="required" Font-Bold="true" runat="server">*Long term medications:</asp:Label>
                                                                            <div class="customdivbordermultiselect">
                                                                                <asp:CheckBoxList ID="chkLongTermMedication" RepeatLayout="Flow" onclick="CheckBoxToggleShowHide('ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_chkLongTermMedication','divLongTermMedication','Other','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txOtherLongTermMedications');"
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
                                                        <tr style="display: none;">
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table width="100%;" border="0">
                                                                    <tr>
                                                                        <td style="width: 52%;" align="right">
                                                                            <label class="required">
                                                                                *Long term medications:</label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <input id="rblbtnLongTermMedicationsyes" onmouseup="up(this);" onfocus="up(this);"
                                                                                onclick="down(this);" type="radio" name="rblbtnLongTermMedicationsyes" runat="server"
                                                                                checked />
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
                                                        <tr style="display: none;">
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
                                                        <tr style="display: none;">
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
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderphysicalexam" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgphysicalexam" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            <asp:Literal ID="literalPhysicalExam" runat="server" Text="Physical Examination"></asp:Literal>
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" width="100%">
                                                            <asp:Panel ID="pnlphysicalexam" runat="server">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <UcPhysicalExamination:Uc4 ID="UcPE" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderdevmil" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgdevmilestones" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            Developmental Milestones
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnldevemilestones" runat="server">
                                                    <table id="Table9" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" align="center" colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td style="width: 23%;" align="right">
                                                                            <label>
                                                                                Milestone appropriate:
                                                                            </label>
                                                                        </td>
                                                                        <td style="width: 25%;" align="left">
                                                                            <asp:RadioButtonList ID="rbListMilestone" onclick="rblSelectedValueShowHide(this,'divmilestoneshowhide','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtspecifywhyinappropriate');"
                                                                                runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                            <%--<input id="rdomilestoneappropriateyes" onmouseup="up(this);" onfocus="up(this);"
                                                                                onclick="down(this);rblSelectedValue1(0,'divmilestoneshowhide','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtspecifywhyinappropriate');fnReset('ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtspecifywhyinappropriate','txt');"
                                                                                type="radio" name="milestoneappropriateyes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rdomilestoneappropriateno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divmilestoneshowhide','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtspecifywhyinappropriate');"
                                                                                type="radio" name="milestoneappropriateyes" runat="server" />
                                                                            <label>
                                                                                No</label>--%>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divmilestoneshowhide" style="display: none; text-indent: inherit">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td style="width: 50%;" align="right">
                                                                                            <label id="Label85" class="margin35" for="District">
                                                                                                If No specify why inappropriate:</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox runat="server" ID="txtspecifywhyinappropriate" TextMode="MultiLine"
                                                                                                Width="200px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <%--  <td class="border pad6 whitebg" width="50%" align="center" valign="top">
                                                              
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheadertestsandlabs" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgtestsandlab" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            <%-- Tests and Labs--%>
                                                                                            Lab Evaluation
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnltestsandlabs" runat="server">
                                                    <table id="Table10" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td class="border" width="50%" align="center">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <UcLabEvalution:Uc7 ID="UCLabEval" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <%--display false cause now using ascx control (Nidhi)--%>
                                                                    <tr style="display: none;">
                                                                        <td style="width: 23%;" align="right">
                                                                            <label id="Label86" class="">
                                                                                Lab evaluation:
                                                                            </label>
                                                                        </td>
                                                                        <td style="width: 25%;">
                                                                            <input id="rdolabevaluationyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divlabevaluatinshowhide');"
                                                                                type="radio" name="labevaluationyes" runat="server" />
                                                                            <label>
                                                                                Yes</label>
                                                                            <input id="rdolabevaluationno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divlabevaluatinshowhide');"
                                                                                type="radio" name="labevaluationyes" runat="server" />
                                                                            <label>
                                                                                No</label>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divlabevaluatinshowhide" style="display: none;">
                                                                                <label id="Label87" for="District" style="padding-left: 5%;">
                                                                                    If yes specify lab evaluation:</label>
                                                                                <div class="checkbox" id="divlabevaluation" nowrap="noWrap">
                                                                                    <asp:CheckBoxList ID="cbllabevaluation" RepeatLayout="Flow" runat="server">
                                                                                    </asp:CheckBoxList>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderwhostaging" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgwhostaging" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                           <asp:Literal ID="literalWHOStaging" runat="server" Text="WHO Staging"></asp:Literal>  </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" width="100%">
                                                            <asp:Panel ID="pnlwhostaging" runat="server">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <UcWhoStaging:Uc5 ID="UcWhostaging" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg" style="display: none;">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border center formbg">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderstaging" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgstagingat" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            Staging at Initial Evaluation
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <%--                                            <div class="center formbg" style="display:none;">
                                                <asp:Panel ID="pnlstagingatinitial" runat="server">
                                                    <table id="Table11" class=" center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="right" style="width: 55%;">
                                                                            <label>
                                                                                WHO Stage at initiation (Transfer in):</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <asp:DropDownList ID="ddlInitiationWHOstage" runat="server" AutoPostBack="false"
                                                                                Width="130px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table class="tbl-left">
                                                                    <tr>
                                                                        <td align="right" class="data-lable">
                                                                            <label class="required">
                                                                                *HIV associated conditions:</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <asp:DropDownList ID="ddlhivassociated" runat="server" Width="130px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="right" style="width: 55%;">
                                                                            <label class="required">
                                                                                *Current WHO Stage:</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <asp:DropDownList ID="ddlwhostage1" runat="server" Width="130px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table class="tbl-left">
                                                                    <tr>
                                                                        <td align="right" class="data-lable">
                                                                            <label class="required">
                                                                                *WAB Stage:</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <asp:DropDownList ID="ddlwabstage1" runat="server" Width="130px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div id="divmernarcheshow">
                                                                  <table class="border pad6 whitebg" width="100%">
                                                                        <tr>
                                                                            <td style="width: 27%;" align="right" class="pad5">
                                                                                <label>
                                                                                    Mernarche:
                                                                                </label>
                                                                            </td>
                                                                            <td style="width: 37%;" align="left">
                                                                                <input id="radbtnMernarcheyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(1,'divmenarchedatehshowhide','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtmenarchedate');"
                                                                                    type="radio" name="labevaluationyes" runat="server" />
                                                                                <label>
                                                                                    Yes</label>
                                                                                <input id="radbtnMernarcheno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue1(0,'divmenarchedatehshowhide','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtmenarchedate');"
                                                                                    type="radio" name="labevaluationyes" runat="server" checked />
                                                                                <label>
                                                                                    No</label>
                                                                            </td>
                                                                            <td>
                                                                                <div id="divmenarchedatehshowhide" style="display: none;">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <label id="Label88">
                                                                                                    Menarche Date:
                                                                                                </label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <input id="txtmenarchedate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="VisitDate" runat="server" />
                                                                                                <img id="Img11" onclick="w_displayDatePicker('<%=txtmenarchedate.ClientID%>');" height="22 "
                                                                                                    alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                    name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span11">(DD-MMM-YYYY)</span>
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
                                                        <tr>
                                                            <td width="50%" class="border pad5 whitebg" colspan="2">
                                                                <div id="divtannerstaging">
                                                                    <table class="tbl-left">
                                                                        <tr>
                                                                            <td align="right" class="data-lable">
                                                                                <label>
                                                                                    Tanner Staging:</label>
                                                                            </td>
                                                                            <td align="left" class="data-control">
                                                                                <asp:DropDownList ID="ddltannerstaging" runat="server" AutoPostBack="false" Width="130px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            --%>
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
                                                        <td class="border pad5 whitebg">
                                                            <table width="100%">
                                                                <tr class="border pad5 whitebg">
                                                                    <td style="width: 50%" align="left">
                                                                        <label>
                                                                            Diagnosis and current illness at this visit:</label>
                                                                        <div id="div1" class="customdivbordermultiselect">
                                                                            <asp:CheckBoxList ID="cblDiagnosis" RepeatLayout="Flow" runat="server" Width="100%"
                                                                                onclick="CheckBoxToggleShowHide('ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_cblDiagnosis','divNonHIV','Non-HIV related illness','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtNonHIVRelatedOI');CheckBoxToggleShowHide('ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_cblDiagnosis','divHIV','HIV-Related illness','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtHIVRelatedOI');">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                    </td>
                                                                    <td style="width: 50%;" align="left">
                                                                        <div id="divHIV" style="display: none;">
                                                                            <label id="Label3" for="txtotherchroniccondition">
                                                                                Specify HIV related OI:</label><br />
                                                                            <asp:TextBox ID="txtHIVRelatedOI" runat="server" Skin="MetroTouch" Width="100%">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                        <div style="display: none;" id="divNonHIV">
                                                                            <label id="Label4" for="txtotherchroniccondition">
                                                                                Specify Non HIV related OI:</label><br />
                                                                            <asp:TextBox ID="txtNonHIVRelatedOI" runat="server" Skin="MetroTouch" Width="100%">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table class="formbg" cellspacing="6" cellpadding="0" width="100%" border="0" style="margin-bottom: 6px;">
                                                    <tr>
                                                        <td width="50%" class="border pad5 whitebg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <div id="DIVHIVrelatedOI" style="display: none; width: 50%;">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <label>
                                                                                        </label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div id="DIVNonHIVrelatedOI" style="display: none;">
                                                                            <table class="tbl-right">
                                                                                <tr>
                                                                                    <td align="right" class="data-lable">
                                                                                        <label>
                                                                                        </label>
                                                                                    </td>
                                                                                    <td align="left" class="data-control">
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
                                                            <table width='100%'>
                                                                <tr>
                                                                    <td style='width: 15%' align='right'>
                                                                        <label align='center' id='lblImpression-8888925'>
                                                                            Impression:</label>
                                                                    </td>
                                                                    <td style='width: 35%' align='left'>
                                                                        <asp:DropDownList runat="server" ID="ddlHAARTImpression" Width="180px" onchange="dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_ddlHAARTImpression','divSpecifyotherimpression','Other specify','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtOtherHAARTImpression');">
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
                                                </table>
                                            </asp:Panel>
                                        </div>
                                        <asp:CollapsiblePanelExtender ID="CPElongtermmedication" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnllongtermmedication"
                                            CollapseControlID="pnlheaderlongtermmedication" ExpandControlID="pnlheaderlongtermmedication"
                                            CollapsedImage="~/images/arrow-up.gif" Collapsed="true" ImageControlID="imglongtermmedication">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CPEPhysicalexam" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlphysicalexam" CollapseControlID="pnlheaderphysicalexam"
                                            ExpandControlID="pnlheaderphysicalexam" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgphysicalexam">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="cpedevmil" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnldevemilestones" CollapseControlID="pnlheaderdevmil"
                                            ExpandControlID="pnlheaderdevmil" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                            ImageControlID="imgdevmilestones">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CPETestandlabs" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnltestsandlabs" CollapseControlID="pnlheadertestsandlabs"
                                            ExpandControlID="pnlheadertestsandlabs" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgtestsandlab">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CPEWhostaging" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlwhostaging" CollapseControlID="pnlheaderwhostaging"
                                            ExpandControlID="pnlheaderwhostaging" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgwhostaging">
                                        </asp:CollapsiblePanelExtender>
                                        <%--  <asp:CollapsiblePanelExtender ID="CPEStaging" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlstagingatinitial" CollapseControlID="pnlheaderstaging"
                                            ExpandControlID="pnlheaderstaging" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                            ImageControlID="imgstagingat">
                                        </asp:CollapsiblePanelExtender>--%>
                                        <asp:CollapsiblePanelExtender ID="CPDiagnosis" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlDiagnosisDetail" CollapseControlID="pnlDiagnosis"
                                            ExpandControlID="pnlDiagnosis" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                            ImageControlID="ImgDiagnosis">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <div class="border center formbg">
                                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="form" align="center">
                                                            <UCSignature:UserControlKNHSignature ID="UserControlKNHSignature_Examination" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr align="center" valign="top">
                                                        <td class="form">
                                                            <asp:Button ID="btnSaveExam" runat="server" Text="Save" OnClick="btnSaveExam_Click" />
                                                            <asp:Button ID="btnCloseExam" runat="server" Text="Close" OnClick="btnCloseExam_Click" />
                                                            <%--<asp:Button ID="btnSubmitExam" runat="server" Text="Data Quality Check" OnClick="btnSubmitExam_Click" />--%>
                                                            <asp:Button ID="btnPrintExam" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabManagement" runat="server" Font-Size="Medium" HeaderText="Management">
                                    <HeaderTemplate>
                                        Management
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="border center formbg">
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheaderdrugallergy" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgdrugallergy" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            Drug Allergy
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnldrugallergy" runat="server">
                                                    <table id="Table12" class="center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr>
                                                            <td width="50%" class="border pad5">
                                                                <table class="tbl-right" border="0" width="100%">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <UcDrugAllergy:Uc6 ID="UCDrugAllergy" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="display: none;">
                                                                        <td align="left">
                                                                            <label class="required">
                                                                                *Drug allergy toxicities:</label>
                                                                            <div id="divDrugAllergiesToxicitiesPaeds" class="customdivbordermultiselect">
                                                                                <asp:CheckBoxList ID="cblDrugAllergiesToxicitiesPaeds" onclick="CheckBoxToggleShowHide1('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_cblDrugAllergiesToxicitiesPaeds','divspecifyotherdrugshowhide','Other');fnControlUnableDesable('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_cblDrugAllergiesToxicitiesPaeds','Sulphur','ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddloiprophylaxis');"
                                                                                    RepeatLayout="Flow" runat="server" Width="250px">
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                            <%--User Control--%>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divspecifyotherdrugshowhide" style="display: none;">
                                                                                <table class="tbl-right" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td class="data-lable" align="left" style="width: 30%;">
                                                                                            <label>
                                                                                                Specify allergy:</label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="data-control" align="left">
                                                                                            <asp:TextBox ID="txtotherdrugallergy" runat="server" Width="100%">
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
                                                        <tr style="display: none;">
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <div id="divspecifyantibioticshowhide" style="display: none;">
                                                                    <table class="tbl-right">
                                                                        <tr>
                                                                            <td align="right" class="data-lable">
                                                                                <label>
                                                                                    Specify antibiotic allergy:</label>
                                                                            </td>
                                                                            <td align="left" class="data-control">
                                                                                <asp:TextBox ID="txtantibioticallergy" runat="server" Width="100%">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <div id="divspecifyarvallergyshowhide" style="display: none;">
                                                                    <table class="tbl-left">
                                                                        <tr>
                                                                            <td align="right" class="data-lable">
                                                                                <label>
                                                                                    Specify ARV allergy:</label>
                                                                            </td>
                                                                            <td align="left" class="data-control">
                                                                                <asp:TextBox ID="txtarvallergy" runat="server" Width="100%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <table class="center formbg" cellspacing="4" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Panel ID="pnlARVSideeffects" CssClass="border center formbg" runat="server"
                                                            Style="padding: 4px">
                                                            <h2 class="forms" align="left">
                                                                <asp:ImageButton ID="ImgARVSideEffect" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                ARV Side Effects</h2>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
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
                                                                            <asp:CheckBoxList ID="cblshorttermeffects" onclick="CheckBoxToggleShowHide1('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_cblshorttermeffects','divshorttermeffecttxt','Other Specify','ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_txtspecityothershortterm');"
                                                                                RepeatLayout="Flow" runat="server" Width="200px">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                        <div id="divshorttermeffecttxt" style="display: none">
                                                                            <label>
                                                                                Specify other short term effects:</label>
                                                                            <asp:TextBox ID="txtspecityothershortterm" runat="server" Skin="MetroTouch" Width="100%">
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
                                                                            <asp:CheckBoxList ID="cbllongtermeffects" onclick="CheckBoxToggleShowHide1('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_cbllongtermeffects','divlongtermeffecttxt','Other specify','ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_txtspecifyotherlongterm');"
                                                                                RepeatLayout="Flow" runat="server" Width="200px">
                                                                            </asp:CheckBoxList>
                                                                        </div>
                                                                        <div id="divlongtermeffecttxt" style="display: none">
                                                                            <label>
                                                                                Specify other long term effects:</label>
                                                                            <asp:TextBox ID="txtspecifyotherlongterm" runat="server" Skin="MetroTouch" Width="100%">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr align="left">
                                                        <td colspan="2" class="border pad5 whitebg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <label>
                                                                            Work up plan:</label>
                                                                        <asp:TextBox ID="txtworkupplan" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                                                            Width="100%">
                                                                        </asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            <div class="center formbg">
                                                <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td colspan="2" class="border leftallign formbg">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlheadertreatment" runat="server">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:ImageButton ID="imgtreatment" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <h2 class="forms" align="left">
                                                                                            <asp:Literal ID="literalTreatment" runat="server" Text="Treatment"></asp:Literal>
                                                                                        </h2>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="center formbg">
                                                <asp:Panel ID="pnltreatment" runat="server">
                                                   
                                                    <UcPharmacy:Uc9 ID="UcPharmacy" runat="server" />
                                                    
                                                </asp:Panel>
                                            </div>
                                            <div class="center formbg">
                                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Panel ID="pnlOITreatment" CssClass="border center formbg" runat="server" Style="padding: 3px">
                                                            <h2 class="forms" align="left">
                                                                <asp:ImageButton ID="imgOITreatment" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                OI Treatment</h2>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                         </div>
                                         <div class="center formbg" style="padding : 6px">
                                            <asp:Panel ID="pnlOITreatmentDetail" runat="server" Width="100%">
                                                <table class="center" width="100%" style="margin-bottom: 6px;">
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" align="center">
                                                                <table border="0" width="100%" cellspacing="4">
                                                                    <tr>
                                                                        <td align="right" width="26%">
                                                                            <label id="Label108" class="margin35" for="District">
                                                                                OI Prophylaxis:</label>
                                                                        </td>
                                                                        <td align="left" width="31%">
                                                                            <asp:DropDownList runat="server" ID="ddloiprophylaxis" onchange="dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddloiprophylaxis','divoiprophylaxisshowhide','Cotrimoxazole','ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddlcotrimoxazole');dropdownchangetext('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddloiprophylaxis','divoiprophylasixothershowhide','Other','ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_txtothercotrimoxazole');dropdownFluconazole('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddloiprophylaxis','divFluconazoleshowhide','Fluconazole','ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddlfluconazole');dropdownFluconazole('ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddloiprophylaxis','divFluconazoleshowhide','CTX and Fluconazol','ctl00_IQCareContentPlaceHolder_tabControl_TabManagement_ddlcotrimoxazole');">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <div id="divoiprophylaxisshowhide" style="display: none;">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td style="width: 50%;" align="right">
                                                                                            <label id="Label109" class="">
                                                                                                Cotrimoxazole prescribed for:
                                                                                            </label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList runat="server" ID="ddlcotrimoxazole">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
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
                                                                            
                                                                            <div id="divoiprophylasixothershowhide" style="display: none;">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td style="width: 49%;" align="right">
                                                                                            <label id="Label110" class="margin35" for="District">
                                                                                                Other (Specify):</label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox runat="server" ID="txtothercotrimoxazole"></asp:TextBox>
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
                                                            <td style="height: 1px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border pad6 whitebg" width="100%" align="left" style="padding-left: 1%;">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <label id="Label111" class="">
                                                                                Other treatment:
                                                                            </label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox runat="server" ID="txtothertreatementcatrimozazole" Width="99%" TextMode="MultiLine"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                          
                                                </table>
                                            </asp:Panel>
                                            </div>
                                            
 
                                        </div>
                                        <asp:CollapsiblePanelExtender ID="CPEOITreatment" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlOITreatmentDetail"
                                            CollapseControlID="pnlOITreatment" ExpandControlID="pnlOITreatment" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgOITreatment">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CPEARVSE" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlARVSideEffectDetail"
                                            CollapseControlID="pnlARVSideeffects" ExpandControlID="pnlARVSideeffects" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="ImgARVSideEffect">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CPEdrugallergy" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnldrugallergy" CollapseControlID="pnlheaderdrugallergy"
                                            ExpandControlID="pnlheaderdrugallergy" CollapsedImage="~/images/arrow-up.gif"
                                            Collapsed="true" ImageControlID="imgdrugallergy">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:CollapsiblePanelExtender ID="CPETreatment" runat="server" SuppressPostBack="true"
                                            ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnltreatment" CollapseControlID="pnlheadertreatment"
                                            ExpandControlID="pnlheadertreatment" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
                                            ImageControlID="imgtreatment">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <div class="border center formbg">
                                            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="form" align="center">
                                                            <UCSignature:UserControlKNHSignature ID="UserControlKNHSignature_Management" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td class="form">
                                                            <asp:Button ID="btnSaveManagement" runat="server" Text="Save" OnClick="btnSaveManagement_Click" />
                                                            <asp:Button ID="btnCloseManagement" runat="server" Text="Close" OnClick="btnCloseManagement_Click" />
                                                            <%--<asp:Button ID="btnSubmitManagement" runat="server" Text="Data Quality Check" OnClick="btnSubmitManagement_Click" />--%>
                                                            <asp:Button ID="btnPrintManagement" Text="Print" OnClientClick="WindowPrint()" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="TabPrevwithpositive" runat="server" Font-Size="Medium" HeaderText="Prev With +ve">
                                    <HeaderTemplate>
                                        Prevention With Positives
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="border center formbg">
                                            <table class="center formbg" cellspacing="6" cellpadding="0" width="100%" border="0">
                                                <tr valign="top">
                                                    <td colspan="2" width="100%">
                                                        <asp:Panel ID="Panel2" runat="server">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <UcPwP:Uc10 ID="UcPWP" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        
                                    </ContentTemplate>
                                </asp:TabPanel>
                            </asp:TabContainer>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>
                            <div class="border center formbg">
                                <table cellspacing="6" cellpadding="0" width="100%" border="0">
                                    <tbody>
                                        <tr style="display: none;">
                                            <td class="border pad6 whitebg" width="100%" align="center">
                                                <label id="Label151">
                                                    Signature:</label>
                                                <asp:DropDownList runat="server" ID="ddlsingature">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hidupdate" runat="server" />
                                                <asp:HiddenField ID="hidregimen" runat="server" />
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td class="form">
                                                <asp:Button ID="Button1" runat="server" Visible="false" Text="Save" OnClick="Button1_Click" />
                                                <%-- <asp:Button ID="btncomplete" runat="server" Text="Data Quality Check" OnClick="btncomplete_Click" />
                                                <asp:Button ID="btnback" runat="server" Visible="false" Text="Close" />
                                                <asp:Button ID="btnPrint" Text="Print" OnClientClick="WindowPrint()" runat="server" />--%>
                                                <asp:CheckBox ID="chkpyridoxine" Visible="false" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            <%--</ContentTemplate>
            <Triggers>
                  <asp:PostBackTrigger ControlID="Button1"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncomplete"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>--%>
        <%--<uc3:UserControlKNH_Extruder ID="UserControlKNH_Extruder1" runat="server" />--%>
        <uc12:UserControlKNH_BackToTop ID="UserControlKNH_BackToTop1" runat="server" />
    </div>
</asp:Content>
