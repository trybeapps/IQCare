<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="ClinicalForms_frmClinical_ARTHistory"
    EnableEventValidation="false" Codebehind="frmClinical_ARTHistory.aspx.cs" %>

<asp:Content ID="ConARTHistory" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    
    <br />
    <div style="padding-left: 8px; padding-right: 8px;">
        <script language="javascript" type="text/javascript">
            function GetControl() {
                document.forms[0].submit();
            } 

            function WindowPrint() {

                window.print();
            }

            function EnableDis(a) {
                var rdoVal = a;
                if (rdoVal.value == "Y") {
                    document.getElementById('<%=ddlpurpose.ClientID%>').disabled = false;
                    document.getElementById('<%=txtRegimen.ClientID%>').disabled = false;
                    document.getElementById('<%=txtLastUsed.ClientID%>').disabled = false;
                    document.getElementById('<%=btnRegimen.ClientID%>').disabled = false;
                    document.getElementById('<%=btnAddPriorART.ClientID%>').disabled = false;
                    document.getElementById('Img1').disabled = false;
                }
                else {
                    document.getElementById('<%=ddlpurpose.ClientID%>').disabled = true;
                    document.getElementById('<%=txtRegimen.ClientID%>').disabled = true;
                    document.getElementById('<%=txtLastUsed.ClientID%>').disabled = true;
                    document.getElementById('<%=btnRegimen.ClientID%>').disabled = true;
                    document.getElementById('<%=btnAddPriorART.ClientID%>').disabled = true;
                    document.getElementById('Img1').disabled = true;
                }
            }
        </script>
        <div class="border center formbg">
            <br />
            <h2 class="forms" align="left">
                Transfer In</h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0" id="divwhoto">
                <tbody>
                    <tr>
                        <td class="border pad6 whitebg" align="center" width="50%">
                            <label class="" for="transferInDate">
                                Transfer In Date:</label>
                            <input id="txtTransferInDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                maxlength="11" size="11" name="txtTransferInDate" runat="server" />
                            <img id="Img4" onclick="w_displayDatePicker('<%=txtTransferInDate.ClientID%>');"
                                height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                border="0" name="appDateimg" />
                            <span class="smallerlabel" id="Span3">(DD-MMM-YYYY)</span>
                        </td>
                        <td class="border pad6 whitebg" align="center" width="50%">
                            <label id="lblenroldate" class="margin35" for="District">
                                From District:</label>
                            <asp:DropDownList ID="dddistrict" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad6 whitebg" align="center" width="50%">
                            <label style="padding-left: 0px" for="transferInDate">
                                Facility:</label>
                            <asp:DropDownList ID="ddfacility" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="border pad6 whitebg" align="center" width="50%">
                            <label id="Label2" class="margin50" for="District">
                                Date Started ART:</label>
                            <input id="txtDateARTStarted" onblur="DateFormat(this,this.value,event,false,'3')"
                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                maxlength="11" size="11" name="txtTransferInDate" runat="server" />
                            <img id="Img5" onclick="w_displayDatePicker('<%=txtDateARTStarted.ClientID%>');"
                                height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                border="0" name="appDateimg" />
                            <span class="smallerlabel" id="Span5">(DD-MMM-YYYY)</span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <h2 class="forms" align="left">
                Prior ART</h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="form">
                            <table width="100%">
                                <tr>
                                    <td colspan="4" align="left">
                                        <label style="padding-left: 10px" id="lblpurpose" runat="server">
                                            Prior ART:</label>
                                        <input id="rbtnknownYes" runat="server" onmouseup="up(this);" onfocus="up(this);" onclick="down(this); EnableDis(this)"
                                            type="radio" value="Y" name="PriorART" />
                                        <label for="y">
                                            Yes</label>
                                        <input id="rbtnknownNo" runat="server" onmouseup="up(this);" onfocus="up(this);" onclick="down(this); EnableDis(this)"
                                            type="radio" value="N" name="PriorART" />
                                        <label for="n">
                                            No</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%" align="right">
                                        <label class="" id="Label1" runat="server">
                                            Purpose:</label>
                                        <asp:DropDownList ID="ddlpurpose" runat="server" Width="110px" Style="z-index: 2">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 4%" align="right">
                                    </td>
                                    <td style="width: 25%" align="center">
                                        <label class="">
                                            Regimen:</label>
                                        <asp:TextBox ID="txtRegimen" MaxLength="50" runat="server" Enabled="False"></asp:TextBox>
                                        <asp:Button ID="btnRegimen" MaxLength="20" runat="server" Text="..." OnClick="btnRegimen_Click">
                                        </asp:Button>
                                    </td>
                                    <td style="width: 30%" align="left">
                                        <label class="">
                                            Date Last Used:</label>
                                        <asp:TextBox ID="txtLastUsed" runat="server" Width="25%" MaxLength="11"></asp:TextBox>
                                        <img onclick="w_displayDatePicker('<%= txtLastUsed.ClientID %>');" height="22" alt="Date Helper"
                                            hspace="3" src="../images/cal_icon.gif" width="22" border="0" id="Img1" />
                                        <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="pad5 formbg border" colspan="2">
                            <div id="divbtnPriorART" class="whitebg" align="center">
                                <asp:Button ID="btnAddPriorART" Text="Add Prior ART" runat="server" OnClick="btnAddPriorART_Click" /></div>
                        </td>
                    </tr>
                    <br />
                    <tr>
                        <td class="pad5 formbg border" colspan="2">
                            <div class="grid" id="divDrugAllergyMedicalAlr" style="width: 100%;">
                                <div class="rounded">
                                  <div class="top-outer">
                                            <div class="top-inner">
                                                <div class="top">
                                                    
                                                </div>
                                            </div>
                                        </div>
                                  <div class="mid-outer">
                                            <div class="mid-inner">
                                                <div class="mid" style="height: 200px; overflow: auto">
                                                    <div id="div-gridview" class="GridView whitebg">
                                                        <asp:GridView Height="50px" ID="GrdPriorART" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" AllowSorting="true" BorderWidth="1" GridLines="None" CssClass="datatable"
                                                        CellPadding="0" CellSpacing="0" HeaderStyle-HorizontalAlign="Left" RowStyle-CssClass="row"
                                                        OnRowDataBound="GrdPriorART_RowDataBound" OnSelectedIndexChanging="GrdPriorART_SelectedIndexChanging"
                                                        OnRowDeleting="GrdPriorART_RowDeleting">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle CssClass="row" />
                                                    </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                    <div class="bottom-outer">
                                            <div class="bottom-inner">
                                                <div class="bottom">
                                                </div>
                                            </div>
                                        </div>
                                    
                                </div>
                               
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <h2 class="forms" align="left">
                HIV Care</h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad6 whitebg" align="center" width="50%">
                            <label class="margin20" id="lblHIVConfirmHIVPosDate">
                                Date Confirmed HIV Positive:</label>
                            <input id="txtHIVConfirmHIVPosDate" onblur="DateFormat(this,this.value,event,false,'3')"
                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                maxlength="11" size="11" name="HIVConfirmHIVPosDate" runat="server" />
                            <img id="Img3" onclick="w_displayDatePicker('<%=txtHIVConfirmHIVPosDate.ClientID%>');"
                                height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                border="0" name="appDateimg" />
                            <span class="smallerlabel" id="Span4">(DD-MMM-YYYY)</span>
                        </td>
                        <td class="border pad6 whitebg" width="50%" align="center">
                            <label id="lblwhere" class="margin35" for="District">
                                Where:</label>
                            <asp:TextBox ID="txtWhere" MaxLength="50" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad6 whitebg" width="50%" align="center">
                            <label id="lblEnrolHIVCare">
                                Date Enrolled in HIV Care:</label>
                            <input id="txtEnrolledinHIVCare" onblur="DateFormat(this,this.value,event,false,'3')"
                                onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                maxlength="11" size="11" name="EnrolledinHIVCare" runat="server" />
                            <img id="Img2" onclick="w_displayDatePicker('<%=txtEnrolledinHIVCare.ClientID%>');"
                                height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                border="0" name="appDateimg" />
                            <span class="smallerlabel" id="Span2">(DD-MMM-YYYY)</span>
                        </td>
                        <td class="border pad6 whitebg" width="50%" align="center" >
                            <label id="lblWHOStage" for="WHOStage">
                                WHO Stage:</label>
                            <asp:DropDownList ID="ddlWHOStage" runat="server" Width="110px" Style="z-index: 2">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <h2 class="forms" align="left">
                Drug Allergies
            </h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0" style="height: 73px">
                <tbody>
                    <tr>
                        <td class="form">
                            <textarea id="txtAreaAllergy" rows="4" cols="1" runat="server" style="width: 99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="form" >
                            <asp:Panel ID="pnlCustomList" Visible="false" runat="server" Height="100%" Width="100%"
                                Wrap="true">
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr align="center">
                        <td class="form">
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" />
                            <asp:Button ID="btncomplete" runat="server" Text="Data Quality Check" OnClick="btncomplete_Click" />
                            <asp:Button ID="btnback" runat="server" Text="Close" OnClick="btnback_Click" />
                            <asp:Button ID="btnPrint" Text="Print" OnClientClick="WindowPrint()" runat="server"
                                OnClick="btnPrint_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
