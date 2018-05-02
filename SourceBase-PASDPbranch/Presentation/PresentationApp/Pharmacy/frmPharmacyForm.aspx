<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="Pharmacy_frmPharmacyForm" Codebehind="frmPharmacyForm.aspx.cs" %>

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
                document.getElementById('<%=pnlPedia.ClientID %>').disabled = true;



            }
            else {
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
        function CalculateTotalDailyDose(valsingleDose, valFrequency, valduration,valTotDose) {

            var vardose = document.getElementById(valsingleDose).value; // to get value of text box
            var selText = document.getElementById(valFrequency).options[document.getElementById(valFrequency).selectedIndex].text // to get selected text
            var varduration = document.getElementById(valduration).value;
            var result = Pharmacy_frmPharmacyForm.fnGetFrequencyMultiplier(selText).value;
            if (result != "0" && vardose!="" && valduration!="" ) {
                var TotalDose = vardose * varduration * result
                document.getElementById(valTotDose).value = TotalDose;
            }
            else {
                document.getElementById(valTotDose).value = "";
            }



        }


        function fnchecked(blnchecked, drugInstructions) {
            var drugid = blnchecked.substring(20);
            //alert(drugid);
            
            if (document.getElementById("ctl00_IQCareContentPlaceHolder_" + blnchecked).checked) {
                //document.getElementById("divPtnIns" + drugid).style.display = "";
                
                document.getElementById("ctl00_IQCareContentPlaceHolder_lblPtnInstructions" + drugid).style.display = "";
                document.getElementById("ctl00_IQCareContentPlaceHolder_txtPtnInstructions" + drugid).style.display = "";
                document.getElementById("ctl00_IQCareContentPlaceHolder_txtPtnInstructions" + drugid).value = drugInstructions;
            }
            else {
                
                //document.getElementById("ptnIns" + drugid).value = "";
                //document.getElementById("ctl00_IQCareContentPlaceHolder_ptnIns" + drugid).value = "";
                //document.getElementById("divPtnIns" + drugid).style.display = "none";
                document.getElementById("ctl00_IQCareContentPlaceHolder_lblPtnInstructions" + drugid).style.display = "none";
                document.getElementById("ctl00_IQCareContentPlaceHolder_txtPtnInstructions" + drugid).value = "";
                document.getElementById("ctl00_IQCareContentPlaceHolder_txtPtnInstructions" + drugid).style.display = "none";
            }
            
        }
 
    </script>
    <script type="text/javascript">
        function ace1_itemSelected(source, e) {
        var results = eval('('  + e.get_value() + ')');
            var index = source._selectIndex;
            if (index != -1) {
                //source.get_element().value = removeHTMLTags(source.get_completionList().childNodes[index]._value);
                var hdCustID = $get('<%= hdCustID.ClientID %>');
                hdCustID.value = results.DrugId;
            }
        }


        function onClientPopulated(sender, e) {
            var propertyPeople = sender.get_completionList().childNodes;
            for (var i = 0; i < propertyPeople.length; i++) {
                            var div = document.createElement("span");
                            var results = eval('(' + propertyPeople[i]._value + ')');
                            div.innerHTML = "<span style=' float:right; font-weight:bold;margin-right: 5px;'> " + results.AvlQty + "</span>";
                            //div.innerHTML = results.AvlQty;
                            propertyPeople[i].appendChild(div);
                            
                        }

            }

        
    </script>
    <style type="text/css" >
        .autoextender
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
        .autoextenderlist
        {
            cursor: pointer;
            color: black;
        }
        .autoextenderhighlight
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
    <%--    <form id="frmPharmacyForm" method="post" runat="server" title="Pharmacy Form">--%>
    <div class="center" style="padding-top: 8px; padding-left:8px; padding-right:8px;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" 
            ScriptMode="Release">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="Updatepanel" runat="server">
            <ContentTemplate>
                <div class="border center formbg">
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
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
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 35%" align="center">
                                                <label class="right35" style="width: 5%">
                                                    Weight:</label>
                                                <asp:TextBox ID="txtWeight" MaxLength="5" runat="server"></asp:TextBox>Kg
                                                <label class="smalllabel" id="dtwt" runat="server" ></label>
                                            </td>
                                            <td style="width: 35%" align="center">
                                                <label class="right35" style="width: 7%">
                                                    Height:</label>
                                                <asp:TextBox ID="txtHeight" MaxLength="4" runat="server"></asp:TextBox>cm
                                                <label class="smalllabel" id="dtht" runat="server" ></label>
                                            </td>
                                            <td style="width: 30%" align="center">
                                                <label class="patientInfo">
                                                    Body Surface Area:</label>
                                                <asp:Label ID="lblBSA" runat="server" Text=""></asp:Label>
                                                <asp:TextBox ID="txtBSA" runat="server" />m<sup>2</sup>
                                            </td>
                                        </tr>
                                    </table>
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
                                                    *Drug Provider:</label>
                                                <asp:DropDownList ID="ddlProvider" runat="server" Style="z-index: 2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trHIVsetFields" runat="server" visible="true">
                                <td class="form" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 30%" align="center">
                                                <label class="required">
                                                    *Regimen Line:</label>
                                                <asp:DropDownList ID="ddlregimenLine" runat="server" Style="z-index: 2">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 40%" align="center">
                                                <label class="">
                                                    Next Appointment Date:</label>
                                                <input id="txtpharmAppntDate" maxlength="11" size="11" name="pharmAppointmentDate"
                                                    runat="server"/>
                                                <img id="Img1" onclick="w_displayDatePicker('<%=txtpharmAppntDate.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                    border="0" name="appDateimg"/><span class="smallerlabel" id="Span1">(DD-MMM-YYYY)</span>
                                            </td>
                                            <td style="width: 30%" align="center">
                                                <label>
                                                    Reason:</label>
                                                <asp:DropDownList ID="ddlAppntReason" runat="server" Style="z-index: 2">
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
                                    <%--<div class="border pad5 whitebg">
                            <asp:Panel ID="PnlRegiment" runat="server" Height="100%" Width="100%" Wrap="true">
                            </asp:Panel>
                        </div>
                        <br />--%>
                                    <div class="border pad5">
                                        <label class="" style="margin-left: 10px">
                                            Select Drug:</label>
                                        <asp:TextBox ID="txtautoDrugName" Width="400px" runat="server" AutoPostBack="true"
                                            AutoComplete="off" OnTextChanged="txtautoDrugName_TextChanged" Font-Names="Courier New"></asp:TextBox>
                                        <div id="divwidth" >
                                        </div>
                                        
                                        <act:AutoCompleteExtender ServiceMethod="SearchDrugs" MinimumPrefixLength="2" CompletionInterval="30"
                                            EnableCaching="false" CompletionSetCount="10" TargetControlID="txtautoDrugName" BehaviorID="AutoCompleteEx"
                                            OnClientItemSelected="ace1_itemSelected" ID="AutoCompleteExtender1" runat="server" OnClientPopulated="onClientPopulated"
                                            FirstRowSelected="false" CompletionListCssClass="autoextender" CompletionListItemCssClass="autoextenderlist"
                                            CompletionListHighlightedItemCssClass="autoextenderhighlight" CompletionListElementID="divwidth">
                                           <Animations>
                                              <OnShow>
                                              <Sequence>
                                              <%-- Make the completion list transparent and then show it --%>
                                              <OpacityAction Opacity="0" />
                                              <HideAction Visible="true" />

                                              <%--Cache the original size of the completion list the first time
                                                the animation is played and then set it to zero --%>
                                              <ScriptAction Script="// Cache the size and setup the initial size
                                                                            var behavior = $find('AutoCompleteEx');
                                                                            if (!behavior._height) {
                                                                                var target = behavior.get_completionList();
                                                                                behavior._height = target.offsetHeight - 2;
                                                                                target.style.height = '0px';
                                                                            }" />
                                              <%-- Expand from 0px to the appropriate size while fading in --%>
                                              <Parallel Duration=".4">
                                              <FadeIn />
                                              <Length PropertyKey="height" StartValue="0" 
	                                            EndValueScript="$find('AutoCompleteEx')._height" />
                                              </Parallel>
                                              </Sequence>
                                              </OnShow>
                                              <OnHide>
                                              <%-- Collapse down to 0px and fade out --%>
                                              <Parallel Duration=".4">
                                              <FadeOut />
                                              <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                                              </Parallel>
                                              </OnHide>
                                              </Animations>
                                        </act:AutoCompleteExtender>
                                        <asp:HiddenField ID="hdCustID" runat="server" />
                                    </div>
                                    <br />
                                    <div class="border pad5 whitebg" id="pnlARV" runat="server" visible="false">
                                        <asp:Panel ID="pnlPedia" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
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

                                            YR1 = parseInt(YR1);
                                            YR2 = parseInt(YR2);

                                            var BSF = Math.sqrt(YR1 * YR2 / 3600);
                                            BSF = BSF.toFixed(2);
                                            document.getElementById(txtBSF).value = BSF;
                                        }



                                        
                                    </script>
                                    <br />
                                    <div class="border" id="pnlOther" runat="server" visible="false">
                                        <asp:Panel ID="PnlOtMed" runat="server" Height="100%" Width="100%" Wrap="true">
                                        </asp:Panel>
                                    </div>
                                    <br />
                                    <div class="border" id="pnlTB" runat="server" visible="false">
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
                        Prescription Notes</h2>
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="form">
                                    <asp:TextBox ID="txtClinicalNotes" TextMode="MultiLine" CssClass="textarea" Width="100%" runat="server"></asp:TextBox>
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
                                <td class="form" width="50%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" >
                                                <label class="required">
                                                    *Prescribed by:</label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlPharmOrderedbyName" runat="Server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="form" width="50%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right">
                                                <label class="required" for="pharmOrderedbyDate">
                                                    *Prescribed By Date:</label>
                                            </td>
                                            <td align="left">
                                                <input id="txtpharmOrderedbyDate" maxlength="11" size="11" name="pharmOrderedbyDate"
                                                    runat="server"/>
                                                <img id="appDateimg1" onclick="w_displayDatePicker('<%=txtpharmOrderedbyDate.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                    border="0" name="appDateimg"/>
                                                <span class="smallerlabel" id="appDatespan1">(DD-MMM-YYYY)</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trDispense" runat="server">
                                <td class="form" width="50%">
                                    <asp:Panel runat="server" ID="pnlDispBy">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="right">
                                                    <label id="lbldispensedby" class="required" runat="server">
                                                        Dispensed by :</label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlPharmReportedbyName" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td class="form" width="50%">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" >
                                                <label id="lbldispensedbydate" class="required" runat="server" for="pharmReportedbyDate">
                                                    Dispensed by Date:</label>
                                            </td>
                                            <td align="left">
                                                <input id="txtpharmReportedbyDate" maxlength="11" size="11" name="pharmReportedbyDate"
                                                    runat="server"/>
                                                <img id="appDateimg2" onclick="w_displayDatePicker('<%=txtpharmReportedbyDate.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                    border="0" name="appDateimg"/><span class="smallerlabel" id="appDatespan2">(DD-MMM-YYYY)</span>
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
                                    <asp:Button ID="btnPrint" Text="Print Pharmacy Form" runat="server" OnClientClick="WindowPrint()" />
                                    <asp:Button ID="btnPresPrint" Text="Print Prescription" runat="server" 
                                        onclick="btnPresPrint_Click" />
                                   
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
                <asp:PostBackTrigger ControlID="btnPresPrint"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <br/>
    </div>
</asp:Content>
