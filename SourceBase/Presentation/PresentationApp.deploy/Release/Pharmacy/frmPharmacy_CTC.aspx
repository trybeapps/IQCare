<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="Pharmacy_frmPharmacy_CTC" Codebehind="frmPharmacy_CTC.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function GetControl() {
            var browserName = navigator.appName;
            if (browserName != "Microsoft Internet Explorer") {

                document.forms[0].submit();


            }
            else {
                document.forms[0].submit();
            }

        }
        function Redirect(id, name, status) {

            if (name == "Add") {
                window.location.href = "../ClinicalForms/frmPatient_Home.aspx";
                //window.location.href="../ClinicalForms/frmPatient_Home.aspx?PatientId="+ id +"&sts="+status;
            }
            if (name == "Edit") {
                //window.location.href="../ClinicalForms/frmPatient_History.aspx?PatientId="+ id +"&sts="+status;
                window.location.href = "../ClinicalForms/frmPatient_History.aspx";
            }
        }
        function fnDissableAllControl(chk, drop1, drop2, txt1, txt2, txt3) {

            document.getElementById(drop1).disabled = "disabled";
            document.getElementById(drop2).disabled = "disabled";
            document.getElementById(txt1).disabled = "disabled";
            document.getElementById(txt2).disabled = "disabled";
            document.getElementById(txt3).disabled = "disabled";
        }
        function WindowPrint() {
            window.print();
        }
        function fnDissableControl(chk, drop1, drop2, txt1, txt2, txt3) {

            if (document.getElementById(chk).checked == true) {
                document.getElementById(drop1).selectedIndex = 0;
                document.getElementById(drop2).selectedIndex = 0;
                document.getElementById(txt1).value = "";
                document.getElementById(txt2).value = "";
                document.getElementById(txt3).value = "";
                document.getElementById(drop1).disabled = "disabled";
                document.getElementById(drop2).disabled = "disabled";
                document.getElementById(txt1).disabled = "disabled";
                document.getElementById(txt2).disabled = "disabled";
                document.getElementById(txt3).disabled = "disabled";

            }
            if (document.getElementById(chk).checked == false) {
                document.getElementById(drop1).disabled = "";
                document.getElementById(drop2).disabled = "";
                document.getElementById(txt1).disabled = "";
                document.getElementById(txt2).disabled = "";
                document.getElementById(txt3).disabled = "";

            }


        }
        function fnCallRequired() {
            if (document.getElementById('<%=ddlARVCombReg.ClientID %>').value != 0) {
                document.getElementById('ctl00_IQCareContentPlaceHolder_lblDuration1').className = "required"
                document.getElementById('ctl00_IQCareContentPlaceHolder_lblDuration1').innerText = '*Duration';
            }
            else {
                document.getElementById('ctl00_IQCareContentPlaceHolder_lblDuration1').className = ""
                document.getElementById('ctl00_IQCareContentPlaceHolder_lblDuration1').innerText = 'Duration';
            }
        }
        function fnChangeClass() {
            document.getElementById('ctl00_IQCareContentPlaceHolder_lblDuration1').className = "required"
            document.getElementById('ctl00_IQCareContentPlaceHolder_lblDuration1').innerText = '*Duration';
        }

    </script>
    <div style="padding-top: 1px;">
        <%--    <form id="frmAdult_Pharmacy" name="frmAdult_Pharmacy" method="post" runat="server"
        enableviewstate="true" title="Pharmacy">--%>
        <%--<h1 class="margin">
            Pharmacy Form</h1>--%>
        <div class="border center formbg">
            <!-- DAL: using tables for form layout. Note that there are classes on labels and td. For custom fields, just use the 2 column layout, if there is an uneven number of cells, set last cell colspan="2" and align="center". Probably should talk through this -->
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <%--<tr>
                        <td class="form" align="center" colspan="2">
                            <label class="patientInfo">
                                Patient Name:</label><asp:Label ID="lblName" CssClass="patientInfo" runat="server"></asp:Label><asp:Label ID="lblPatientName" runat="server"></asp:Label>
                            
                            <label class="patientInfo">
                                Patient ID:</label><asp:Label ID="lblid" CssClass="patientInfo" runat="server"></asp:Label><asp:Label ID="lblpatientenrol" runat="server"></asp:Label>
                           <asp:Label ID="lblNumber" Font-Bold="true" CssClass="patientInfo" runat="server"></asp:Label><asp:Label ID="lblExisPatientID" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="form" align="center" valign="middle">
                            <input id="chkpharmDispensePU" type="checkbox" value="no" name="pharmDispensePU"
                                runat="server">
                            <label class="alert">
                                DO NOT DISPENSE ARV'S TO PATIENT. HOLD FOR ADHERENCE STAFF PICKUP.</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="form">
                            <table width="100%">
                                <tr>
                                    <td align="center" style="width: 50%">
                                        <label class="required">
                                            *Treatment Program:</label>
                                        <asp:DropDownList ID="ddlTreatment" runat="server" Style="z-index: 2" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlTreatment_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center" style="width: 50%">
                                        <label class=" required">
                                            *ARV Provider:</label>
                                        <asp:DropDownList ID="ddlProvider" runat="server" Style="z-index: 2">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <br>
            <table cellspacing="6" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <td class="border pad5 whitebg">
                            <div class="border">
                                <asp:Panel ID="PnlDrug" runat="server" Height="100%" Width="100%" Wrap="true" Font-Bold="true">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="5">
                                                <asp:Label ID="lblARVMedications" Font-Bold="true" Text="1. ARV Medications" runat="server"
                                                    Style="margin-left: 5px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblARVCombRegimen" CssClass="" Font-Bold="true" Text="ARV Combination Regimen"
                                                    Style="margin-left: 5px" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFrq1" Visible="false" Font-Bold="true" Text="Frequency" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDuration1" Font-Bold="true" Text="Duration" runat="server" CssClass=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblQtyPres1" Font-Bold="true" Text="Qty. Prescribed" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblQtyDesp" Font-Bold="true" Text="Qty. Dispensed" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProphylaxis" Font-Bold="true" Text="Prophylaxis" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlARVCombReg" onchange="fnCallRequired()" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlARVCombRegFrqARV" Visible="false" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtARVCombRegDuraton" Text="" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtARVCombRegQtyPres" Text="" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtARVCombRegQtyDesc" Text="" runat="server"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkProphylaxis" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <br />
                            <div class="border">
                                <div align="center" id="divAddARV" runat="server">
                                    <asp:Button ID="BtnAddARV" runat="server" Text="Other ARV Medications" OnClick="BtnAddARV_Click" />
                                </div>
                                <asp:Panel ID="PnlAddARV" runat="server" Height="100%" Width="100%" Wrap="true" Font-Bold="true">
                                </asp:Panel>
                            </div>
                            <br />
                            <div class="border">
                                <asp:Panel ID="PnlOIARV" runat="server" Height="100%" Width="100%" Wrap="true">
                                </asp:Panel>
                            </div>
                            <br />
                            <div class="border">
                                <div align="center" id="divAddOI" runat="server">
                                    <asp:Button ID="OtherMedication" runat="server" Text="Other OI Treatment and Non-HIV/AIDS Medications"
                                        OnClick="OtherMedication_Click" />
                                </div>
                                <asp:Panel ID="PnlOtherMedication" runat="server" Height="100%" Width="100%" Wrap="true">
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="border center formbg">
            <br>
            <h2 class="forms" align="left">
                Approval and Signatures</h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="form">
                            <table width="100%">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <label class="required">
                                            *Ordered by:</label>
                                    </td>
                                    <td style="width: 50%">
                                        <asp:DropDownList ID="ddlPharmOrderedbyName" runat="Server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="form">
                            <table width="100%">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <label class="required" for="pharmOrderedbyDate">
                                            *Ordered by Date:</label>
                                    </td>
                                    <td style="width: 50%">
                                        <input id="txtpharmOrderedbyDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                            maxlength="11" size="11" name="pharmOrderedbyDate" runat="server">
                                        <img id="appDateimg1" onclick="w_displayDatePicker('<%=txtpharmOrderedbyDate.ClientID%>');"
                                            height="22" alt="Date Helper" hspace="5" src="../Images/cal_icon.gif" width="22"
                                            border="0" name="appDateimg">
                                        <span class="smallerlabel" id="appDatespan1">(DD-MMM-YYYY)</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="form">
                            <table width="100%">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <label class="required">
                                            *Dispensed by:</label>
                                    </td>
                                    <td style="width: 50%">
                                        <asp:DropDownList ID="ddlPharmReportedbyName" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="form">
                            <table width="100%">
                                <tr>
                                    <td align="right" style="width: 50%">
                                        <label class="required" for="pharmReportedbyDate">
                                            *Dispensed by Date:</label>
                                    </td>
                                    <td style="width: 50%">
                                        <input id="txtpharmReportedbyDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                            maxlength="11" size="11" name="pharmReportedbyDate" runat="server">
                                        <img id="appDateimg2" onclick="w_displayDatePicker('<%=txtpharmReportedbyDate.ClientID%>');"
                                            height="22" alt="Date Helper" hspace="5" src="../Images/cal_icon.gif" width="22"
                                            border="0" name="appDateimg"><span class="smallerlabel" id="appDatespan2">(DD-MMM-YYYY)</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="form" valign="middle" colspan="3">
                            <table width="100%" border="0">
                                <tr>
                                    <td align="right" style="width: 40%">
                                        <label style="display: inline" class="required">
                                            *Signature:
                                        </label>
                                    </td>
                                    <td style="width: 60%" align="left">
                                        <table width="100%">
                                            <tr>
                                                <td align="left" style="width: 30%">
                                                    <input id="btnPatientSignature" type="radio" runat="server" value="1" onfocus="up(this)"
                                                        onclick="down(this);hide('ddSignature');" /><label>Patient Signature</label>
                                                </td>
                                                <td align="left" style="width: 30%">
                                                    <input id="btnCounsellorSignature" type="radio" runat="server" value="2" onfocus="up(this)"
                                                        onclick="show('ddSignature')" /><label>Adherence Counselor</label>
                                                </td>
                                                <td style="width: 40%">
                                                    <div id="ddSignature" style="display: none">
                                                        <asp:DropDownList ID="ddlPharmSignature" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--<DIV id=otherpharmSignature style="DISPLAY: none"></DIV>--%>
                                        <asp:Label ID="Label2" runat="server" Visible="false" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="form" colspan="2">
                            <asp:Panel ID="pnlCustomList" Visible="false" runat="server" Height="100%" Width="100%"
                                Wrap="true">
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="pad5 center" colspan="2">
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" />
                            <asp:Button ID="btncancel" runat="server" Text="Close" OnClick="btncancel_Click" />
                            <asp:Button ID="btnOk" runat="server" CssClass="textstylehidden" Text="OK" OnClick="btnOk_Click" />
                            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClientClick="WindowPrint()" />
                        </td>
                        <asp:XmlDataSource ID="FxdDrugs" runat="server" DataFile="~/XMLFiles/adultpharmacylist.xml">
                        </asp:XmlDataSource>
                    </tr>
                </tbody>
            </table>
        </div>
        <br>
    </div>
</asp:Content>
