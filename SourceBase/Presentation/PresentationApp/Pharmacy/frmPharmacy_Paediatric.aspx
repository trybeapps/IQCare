<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmPharmacy_Paediatric" Title="Untitled Page"
    MaintainScrollPositionOnPostback="true" Codebehind="frmPharmacy_Paediatric.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function WindowPrint() {
            window.print();
        }

        function fnCheckUnCheck() {
            var chk = document.getElementById('<%=hidchkbox.ClientID %>').value;
            var chksplit = chk.split(',');
            if (document.getElementById('<%=ddlTreatment.ClientID %>').value == "222") {

                for (var i = 0; i < chksplit.length; i++) {
                    var cid = "ctl00_IQCareContentPlaceHolder_" + chksplit[i];
                    if (document.getElementById(cid) != null) {
                        document.getElementById(cid).disabled = true;
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
            }
            if (document.getElementById('<%=ddlTreatment.ClientID %>').value == "225") {
                document.getElementById('<%=PnlRegiment.ClientID %>').disabled = true;
                document.getElementById('<%=pnlPedia.ClientID %>').disabled = true;



            }
            else {
                document.getElementById('<%=PnlRegiment.ClientID %>').disabled = false;
                document.getElementById('<%=pnlPedia.ClientID %>').disabled = false;


            }
        }

        function Redirect(id, name, status) {

            if (name == "Add") {
                window.location.href = "../ClinicalForms/frmPatient_Home.aspx";
            }
            if (name == "Edit") {
                window.location.href = "../ClinicalForms/frmPatient_History.aspx";
            }
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
        <%--  <form id="frmPediatric_Pharmacy" method="post" runat="server" title="Pediatric Pharmacy">--%>
        <%--<h1 class="margin">
        Paediatric Pharmacy Form</h1>--%>
        
        <asp:UpdatePanel ID="Updatepanel" runat="server">
            <ContentTemplate>
                <div class="border center formbg">
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <%--<tr>
                    <td class="form" align="center" colspan="2">
                        <label class="patientinfo">
                            Patient Name:</label>
                        <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                        <label class="patientInfo">
                            Patient ID #:</label>
                        <asp:Label ID="lblpatientenrolment" runat="server" Text=""></asp:Label>
                        <label class="patientInfo">
                            Existing Hosp/Clinic #:</label>
                        <asp:Label ID="lblExisPatientID" runat="server" Text=""></asp:Label>
                        
                    </td>
                </tr>--%>
                            <tr>
                                <td class="form" align="center" width="50%">
                                    <label class="right35">
                                        Age:</label>
                                    <asp:TextBox ID="txtYr" runat="server" Width="50"></asp:TextBox>Yrs
                                    <asp:TextBox ID="txtMon" runat="server" Width="50"></asp:TextBox>Months
                                </td>
                                <td class="form" align="center" width="50%">
                                    <label class="right35">
                                        DOB:</label>
                                    <asp:TextBox ID="txtDOB" runat="server" Width="80"></asp:TextBox>
                                    <asp:HiddenField ID="hidchkbox" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="form" align="center" colspan="2">
                                    <label class="right35" style="width: 5%">
                                        Weight:</label>
                                    <asp:Label ID="lblWeight" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtWeight" MaxLength="4" runat="server"></asp:TextBox>Kg
                                    <label class="right35" style="width: 7%">
                                        Height:</label>
                                    <asp:Label ID="lblHeight" runat="server" Text=""></asp:Label>
                                    <asp:TextBox ID="txtHeight" MaxLength="4" runat="server"></asp:TextBox>cm
                                    <label class="patientInfo">
                                        Body Surface Area:</label>
                                    <asp:Label ID="lblBSA" runat="server" Text=""></asp:Label>
                                    <asp:TextBox ID="txtBSA" runat="server" />m<sup>2</sup>
                                </td>
                            </tr>
                            <tr>
                                <td class="form" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 30%" align="center">
                                                <label class="required">
                                                    *Treatment Program:</label>
                                                <asp:DropDownList ID="ddlTreatment" runat="server" Style="z-index: 2" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlTreatment_SelectedIndexChanged">
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
                                <td class="border pad5 whitebg" align="left">
                                    <div class="border pad5 whitebg">
                                        <asp:Panel ID="PnlRegiment" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <br />
                                    <div class="border pad5">
                                        <label class="" style="margin-left: 10px">
                                            Select Drug:</label>
                                        <asp:TextBox ID="txtautoDrugName" Width="400px" runat="server" AutoPostBack="true"
                                            AutoComplete="off" OnTextChanged="txtautoDrugName_TextChanged" Font-Names="Courier New"></asp:TextBox>
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
                                    <div class="border pad5 whitebg" id="pnlARV" runat="server" visible="false">
                                        <%-- <div align="center" id="div1" runat="server">
                                <asp:Button ID="BtnAddARV" runat="server" Text="ARV Medications" OnClick="BtnAddARV_Click" />
                            </div>--%>
                                        <asp:Panel ID="pnlPedia" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <%--<br />
                        <div class="border pad5 whitebg" id="divFixed" runat="server">
                            <asp:Panel ID="PnlFixed" runat="server" Height="100%" Width="100%" Wrap="true">
                                <asp:Label ID="lblFixedDose0" Font-Bold="True" Text="Fixed Dose Combination" runat="server"
                                    Style="margin-left: 5px"></asp:Label>
                            </asp:Panel>
                        </div>--%>
                                    <br />
                                    <div class="border" id="pnlOI" runat="server" visible="false">
                                        <asp:Panel ID="PnlOIARV" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <script language="javascript" type="text/javascript">
                                        function GetControl() {
                                            document.forms[0].submit();
                                        }

                                        function CalcualteBSF(txtBSF, Weight, Height) {
                                            var YR1 = document.getElementById(Weight).value;
                                            var YR2 = document.getElementById(Height).value;
                                            if (YR1 == "" || YR2 == "") {
                                                YR1 = 0;
                                                YR2 = 0;
                                            }
                                            // if (YR2 = "")
                                            // {
                                            //     YR2 = 0;
                                            // }
                                            YR1 = parseInt(YR1);
                                            YR2 = parseInt(YR2);
                                            // alert(YR1);
                                            //  alert(YR2);
                                            var BSF = Math.sqrt(YR1 * YR2 / 3600);
                                            BSF = BSF.toFixed(2);
                                            document.getElementById(txtBSF).value = BSF;
                                        }

                                        /**********************************************************
                                        Function		: Calculate Total Dose	
                                        Created By		: Amitava Sinha
                                        Created On		: 13-Feb-2006
                                        ***********************************************************/

                                        function CalculateTotalDailyDose(singleDose, Frequency, TotDose) {
                                            // alert('Rupesh');
                                            // alert(singleDose);
                                            //alert(Frequency);
                                            //alert(TotDose);
                                            var dose = document.getElementById(singleDose).value; // to get value of text box
                                            //var v2 = document.getElementById(Frequency).selectedIndex;
                                            //var v2=document.getElementById(Frequency).options[document.getElementById(Frequency).selectedIndex].value // to get selected value
                                            var selText = document.getElementById(Frequency).options[document.getElementById(Frequency).selectedIndex].text // to get selected text

                                            var Frequency;
                                            if (selText == "OD") {
                                                Frequency = 1;
                                            }
                                            else if (selText == "BD" || selText == "bid") {
                                                Frequency = 2;
                                            }
                                            else if (selText == "1OD") {
                                                Frequency = 1;
                                            }
                                            else if (selText == "2OD") {
                                                Frequency = 2;
                                            }

                                            else if (selText == "1BD") {
                                                Frequency = 2;
                                            }
                                            else if (selText == "3OD" || selText == "TD") {
                                                Frequency = 3;
                                            }
                                            else if ((selText == "qid") || (selText == "QID") || (selText == "QD")) {
                                                Frequency = 4;
                                            }
                                            else if (selText == "Weekly") {
                                                Frequency = 4;
                                            }
                                            else if (selText = "Select") {
                                                Frequency = 0;
                                            }
                                            TotalDose = dose * Frequency;

                                            if (TotalDose == 0) {
                                                TotalDose = "";
                                            }
                                            //selBox.options[selBox.selectedIndex].value;
                                            document.getElementById(TotDose).value = TotalDose;
                                        }
 
                                    </script>
                                    <br />
                                    <div class="border" id="pnlOther" runat="server" visible="false">
                                        <%--<div align="center" id="div2" runat="server">
                                <asp:Button ID="OtherMedication" runat="server" Text="OI Treatment Medications"
                                    OnClick="OtherMedication_Click" />
                            </div>--%>
                                        <asp:Panel ID="PnlOtMed" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <br />
                                    <div class="border" id="pnlTB" runat="server" visible="false">
                                        <%--<div align="center" id="div3" runat="server">
                                <asp:Button ID="btnOtherTBMedicaton" runat="server" Text="Other TB Medications" OnClick="btnOtherTBMedicaton_Click" />
                            </div>--%>
                                        <asp:Panel ID="pnlOtherTBMedicaton" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
                <div class="border center formbg">
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
                                                    *Prescribed By Date:</label>
                                            </td>
                                            <td>
                                                <input id="txtpharmOrderedbyDate" maxlength="11" size="11" name="pharmOrderedbyDate"
                                                    runat="server">
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
                                    <asp:Panel runat="server" ID="pnlDispBy">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="right" style="width: 50%">
                                                    <label id="lbldispensedby" class="Required" runat="server">
                                                        Dispensed by:</label>
                                                </td>
                                                <td style="width: 50%">
                                                    <asp:DropDownList ID="ddlPharmReportedbyName" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td class="form">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" style="width: 50%">
                                                <label id="lbldispensedbydate" class="Required" runat="server" for="pharmReportedbyDate">
                                                    Dispensed by Date:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <input id="txtpharmReportedbyDate" maxlength="11" size="11" name="pharmReportedbyDate"
                                                    runat="server">
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
                                    <label class="required right35" style="width: 1%">
                                        *Signature:</label>
                                    <asp:DropDownList ID="ddlPharmSignature" runat="server" OnSelectedIndexChanged="ddlPharmSignature_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
                                        <asp:ListItem Text="No Signature"></asp:ListItem>
                                        <asp:ListItem Text="Patient's Signature"></asp:ListItem>
                                        <asp:ListItem Text="Adherance counsellor signature"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlCounselerName" runat="server" Visible="false" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlCounselerName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%--<DIV id=otherpharmSignature style="DISPLAY: none"></DIV>--%>
                                    <label id="lblCounselorName" class="margin20" visible="false" runat="server">
                                        Specify Counselor Name:
                                    </label>
                                    <asp:TextBox ID="txtCounselorName" Visible="false" runat="server">
                                    </asp:TextBox>
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
                            </tr>
                        </tbody>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="txtautoDrugName"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnsave"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncancel"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnPrint"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <br/>
    </div>
</asp:Content>
