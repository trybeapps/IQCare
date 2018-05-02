<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmPharmacy_Adult" Title="Untitled Page"
    MaintainScrollPositionOnPostback="true" Codebehind="frmPharmacy_Adult.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function WindowPrint() {
            window.print();
        }
        function fnCheckUnCheck() {
            var chk = document.getElementById('<%=hidchkbox.ClientID %>').value;
            var chk1 = document.getElementById('<%=hidchkbox1.ClientID %>').value;
            var chksplit = chk.split(',');
            var chksplit1 = chk1.split(',');
            if (document.getElementById('<%=ddlTreatment.ClientID %>').value == "222") {

                for (var i = 0; i < chksplit.length; i++) {
                    var cid = "ctl00_IQCareContentPlaceHolder_" + chksplit[i];
                    if (document.getElementById(cid) != null) {

                        document.getElementById(cid).disabled = true;
                    }
                }

                for (var i = 0; i < chksplit1.length; i++) {
                    var cid1 = "ctl00_IQCareContentPlaceHolder_" + chksplit1[i];
                    if (document.getElementById(cid1) != null) {

                        document.getElementById(cid1).disabled = true;
                    }
                }
            }
            else {
                for (var i = 0; i < chksplit.length; i++) {
                    var cid = "ctl00_IQCareContentPlaceHolder_" + chksplit[i];
                    if (document.getElementById(cid) != null) {
                        document.getElementById(cid).disabled = false;
                    }
                }

                for (var i = 0; i < chksplit1.length; i++) {
                    var cid1 = "ctl00_IQCareContentPlaceHolder_" + chksplit1[i];
                    if (document.getElementById(cid1) != null) {
                        document.getElementById(cid1).disabled = false;
                    }
                }

            }
            if (document.getElementById('<%=ddlTreatment.ClientID %>').value == "225") {
                document.getElementById('<%=PnlRegiment.ClientID %>').disabled = true;
                document.getElementById('<%=PnlDrug.ClientID %>').disabled = true;



            }
            else {
                document.getElementById('<%=PnlRegiment.ClientID %>').disabled = false;
                document.getElementById('<%=PnlDrug.ClientID %>').disabled = false;




            }
        }
        function Redirect(id, name, status) {

            if (name == "Add") {
                //window.location.href="../ClinicalForms/frmPatient_Home.aspx";
                window.location.href = "../ClinicalForms/frmPatient_Home.aspx?sts=" + status;
            }
            if (name == "Edit") {
                //window.location.href="../ClinicalForms/frmPatient_History.aspx?PatientId="+ id +"&sts="+status;
                window.location.href = "../ClinicalForms/frmPatient_History.aspx";
            }
        }
        function HidUnhide() {
        }
    </script>
    <script type="text/javascript">

        function ace1_itemSelected(sender, e) {
            var hdCustID = $get('<%= hdCustID.ClientID %>');
            hdCustID.value = e.get_value();
            //alert(hdCustID.value);
        }

    </script>
    <style>
        .AutoExtender
        {
            font-family: Courier New, Arial, sans-serif;
            font-size: 11px;
            font-weight: 100;
            border: solid 1px #006699;
            line-height: 15px;
            padding: 0px;
            background-color: White;
            margin-left: 0px;
            width: 800px;
        }
        .AutoExtenderList
        {
            cursor: pointer;
            color: black;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
            width: 800px !important;
        }
        #divwidth div
        {
            width: 800px !important;
        }
    </style>
    <div class="center" style="padding: 8px;">
        <%-- <form id="frmAdult_Pharmacy" method="post" runat="server" enableviewstate="true"
    title="Adult Pharmacy">--%>
        <%--<h1 class="margin">
        Adult Pharmacy Form</h1>--%>
        <asp:UpdatePanel ID="Updatepanel" runat="server">
            <ContentTemplate>
                <div class="border center formbg">
                    <!-- DAL: using tables for form layout. Note that there are classes on labels and td. For custom fields, just use the 2 column layout, if there is an uneven number of cells, set last cell colspan="2" and align="center". Probably should talk through this -->
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="form">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 30%" align="center">
                                                <label class="required">
                                                    *Treatment Program:</label>
                                                <asp:DropDownList ID="ddlTreatment" runat="server" Style="z-index: 2" OnSelectedIndexChanged="ddlTreatment_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 30%" align="center">
                                                <label class="">
                                                    Period Taken:</label>
                                                <asp:DropDownList ID="ddlPeriodTaken" runat="server" Style="z-index: 2">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 30%" align="center">
                                                <label class="required">
                                                    *ARV Provider:</label>
                                                <asp:DropDownList ID="ddlProvider" runat="server" Style="z-index: 2">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hidchkbox" runat="server" />
                                                <asp:HiddenField ID="hidchkbox1" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
                <div class="border center formbg">
                    <br>
                    <table cellspacing="6" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td class="border pad5 whitebg">
                                    <div class="border">
                                        <asp:Panel ID="PnlRegiment" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <br />
                                    <div class="border pad5">
                                        <label class="" style="margin-left: 10px">
                                            Select Drug:</label>
                                        <asp:TextBox ID="txtautoDrugName" Width="400px" runat="server" AutoPostBack="true"
                                            AutoComplete="off" OnTextChanged="txtautoDrugName_TextChanged"></asp:TextBox>
                                        <div id="divwidth">
                                        </div>
                                        <act:AutoCompleteExtender ServiceMethod="SearchDrugs" MinimumPrefixLength="2" CompletionInterval="30"
                                            EnableCaching="false" CompletionSetCount="10" TargetControlID="txtautoDrugName"
                                            OnClientItemSelected="ace1_itemSelected" ID="AutoCompleteExtender1" runat="server"
                                            FirstRowSelected="false" CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                            CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListElementID="divwidth">
                                        </act:AutoCompleteExtender>
                                        <asp:HiddenField ID="hdCustID" runat="server" />
                                    </div>
                                    <br />
                                    <div class="border" id="pnlARV" runat="server" visible="false">
                                        <%--<div align="center" id="divAddARV" runat="server">
                                <asp:Button ID="BtnAddARV" runat="server" Text="Other ARV Medications" OnClick="BtnAddARV_Click" />
                            </div>--%>
                                        <asp:Panel ID="PnlDrug" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <%--<br />
                        <div class="border pad5 whitebg" id="divFixed" runat="server">
                            <asp:Panel ID="PnlFixeddose" runat="server" Height="100%" Width="100%" Wrap="true">
                                <asp:Label ID="lblFixedDose0" Font-Bold="True" Text="Fixed Dose Combination" runat="server"
                                    Style="margin-left: 5px"></asp:Label>
                            </asp:Panel>
                        </div>--%>
                                    <%--<br />
                        <div class="border">
                            
                            <asp:Panel ID="PnlAddARV" runat="server" Height="100%" Width="100%" Wrap="true">
                            </asp:Panel>
                        </div>--%>
                                    <script language="javascript" type="text/javascript">
                                        function GetControl() {
                                            document.forms[0].submit(); 
                                        }
                                    </script>
                                    <br />
                                    <div class="border" id="pnlOI" runat="server" visible="false">
                                        <asp:Panel ID="PnlOIARV" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <br />
                                    <div class="border" id="pnlOther" runat="server" visible="false">
                                        <%-- <div align="center" id="divAddOI" runat="server">
                                <asp:Button ID="OtherMedication" runat="server" Text="Other OI Treatment and Non-HIV/AIDS Medications"
                                    OnClick="OtherMedication_Click" />
                            </div>--%>
                                        <asp:Panel ID="PnlOtherMedication" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <%--<br />
                        <div class="border">
                            <table cellspacing="6" cellpadding="0" border="0" width="100%">
                                <tbody>
                                    <tr>
                                        <td class="">
                                            <div class="">
                                                <asp:Panel ID="pnltbregimen" runat="server" Height="100%" Width="100%" Wrap="true"
                                                    Font-Bold="true">
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="5">
                                                                <asp:Label ID="lblARVMedications" Font-Bold="true" Text="3. TB Treatment" runat="server"
                                                                    Style="margin-left: 5px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCombRegimen" CssClass="" Font-Bold="true" Text="Combination Regimen"
                                                                    Style="margin-left: 5px" runat="server"></asp:Label>
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
                                                                <asp:Label ID="lblTreatmentphase" Font-Bold="true" Text="Treatment Phase" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblmonth" Font-Bold="true" Text="Month" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlARVCombReg" runat="server">
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
                                                                <asp:DropDownList ID="ddlTreatmentphase" runat="server">
                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Intensive" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Continue" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddTrMonths" runat="server">
                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>--%>
                                    <br />
                                    <div class="border" id="pnlTB" runat="server" visible="false">
                                        <%--<div align="center" id="div1" runat="server">
                                <asp:Button ID="btnOtherTBMedicaton" runat="server" Text="Other TB Medications" OnClick="btnOtherTBMedicaton_Click" />
                            </div>--%>
                                        <asp:Panel ID="pnlOtherTBMedicaton" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <%--<br />
                        <div class="border">
                            <table cellspacing="6" cellpadding="0" border="0" width="100%">
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblmaternalhealth" Font-Bold="true" Text="4.Vitamins and Other Drugs"
                                            runat="server" Style="margin-left: 5px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblmultiVitamin" Font-Bold="true" Text="Multivitamin" runat="server"
                                            CssClass=""></asp:Label>
                                        <asp:CheckBox ID="chkmultiVitamin" runat="server" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbliron" Font-Bold="true" Text="Iron" runat="server" CssClass=""></asp:Label>
                                        <asp:CheckBox ID="chkiron" runat="server" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblvitaminC" Font-Bold="true" Text="Vitamin C" runat="server" CssClass=""></asp:Label>
                                        <asp:CheckBox ID="chkvitaminC" runat="server" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lbltetanus" Font-Bold="true" Text="Tetanus Toxoid" runat="server"
                                            CssClass=""></asp:Label>
                                        <asp:DropDownList ID="ddltetanus" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblfolicacid" Font-Bold="true" Text="Folic Acid" runat="server" CssClass=""></asp:Label>
                                        <asp:CheckBox ID="chkfolicacid" runat="server" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblvitaminA" Font-Bold="true" Text="Vitamin A" runat="server" CssClass=""></asp:Label>
                                        <asp:CheckBox ID="chkvitaminA" runat="server" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblMebendazole" Font-Bold="true" Text="Mebendazole" runat="server"
                                            CssClass=""></asp:Label>
                                        <asp:CheckBox ID="chkmebendazole" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div class="border">
                            <div align="center" id="div2" visible="false" runat="server">
                                <asp:Button ID="btnOtherNonHivMedicane" runat="server" Text="Other Non HIV/AIDS Medications" />
                            </div>
                            <asp:Panel ID="pnlothernonhivaidsmedicane" runat="server" Height="100%" Width="100%"
                                Wrap="true">
                            </asp:Panel>
                        </div>--%>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
                <div class="border center formbg">
                    <br>
                    <h2 class="forms" align="left">
                        Pharmacy Notes</h2>
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="form">
                                    <asp:TextBox ID="txtClinicalNotes" CssClass="textarea" TextMode="MultiLine" Width="100%" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
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
                                                    *Prescribed by:</label>
                                            </td>
                                            <td>
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
                                                    *Prescribed by Date:</label>
                                            </td>
                                            <td>
                                                <input id="txtpharmOrderedbyDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                    maxlength="11" size="11" name="pharmOrderedbyDate" runat="server">
                                                <img id="appDateimg1" onclick="w_displayDatePicker('<%=txtpharmOrderedbyDate.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                    border="0" name="appDateimg">
                                                <span class="smallerlabel" id="appDatespan1">(DD-MMM-YYYY)</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trDispense" runat="server">
                                <td class="form">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" style="width: 50%">
                                                <label id="lbldispensedby" class="Required" runat="server">
                                                    *Dispensed by:</label>
                                            </td>
                                            <td>
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
                                                <label id="lbldispensedbydate" class="Required" for="pharmReportedbyDate" runat="server">
                                                    Dispensed by Date:</label>
                                            </td>
                                            <td>
                                                <input id="txtpharmReportedbyDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                    maxlength="11" size="11" name="pharmReportedbyDate" runat="server">
                                                <img id="appDateimg2" onclick="w_displayDatePicker('<%=txtpharmReportedbyDate.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                    border="0" name="appDateimg"><span class="smallerlabel" id="appDatespan2">(DD-MMM-YYYY)</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="form" align="center" valign="middle" colspan="2">
                                    <div>
                                        <label style="display: inline" class="required">
                                            *Signature:
                                        </label>
                                        <input id="btnPatientSignature" type="radio" runat="server" value="1" onfocus="up(this)"
                                            onclick="down(this);hide('ddSignature');" /><label class="required">Patient Signature</label>
                                        <input id="btnCounsellorSignature" type="radio" runat="server" value="2" onfocus="up(this)"
                                            onclick="show('ddSignature')" /><label class="required">Adherence Counselor</label>
                                        <div id="ddSignature" style="display: none">
                                            <asp:DropDownList ID="ddlPharmSignature" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                        <%--<DIV id=otherpharmSignature style="DISPLAY: none"></DIV>--%>
                                    </div>
                                    <asp:Label ID="Label2" runat="server" Visible="false" Text="Label"></asp:Label>
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
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="txtautoDrugName"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnsave"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncancel"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnPrint"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
