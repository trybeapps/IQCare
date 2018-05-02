<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="ClinicalForms_frmClinical_Transfer" Codebehind="frmClinical_Transfer.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/IQCare.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <%--<link type="text/css" href="../Style/_assets/css/grid.css" rel="stylesheet" />
<link type="text/css" href="../Style/_assets/css/round.css" rel="stylesheet" />--%>
    <script language="javascript" type="text/javascript">
        function WindowPrint() {
            window.print();
        } 
    </script>
    <%--<form id="Transfer" method="post" runat="server" title="">--%>
    <%--  <h1 class="margin"> 
            Patient Transfer</h1>--%>
    <div class="center" style="padding: 8px;">
        <%--   <div class="border center formbg">
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="form" align="center" colspan="2">
                            <label class="patientInfo">
                                Patient Name&nbsp;:</label>
                            <asp:Label ID="lblpatientname" runat="server" Text="Mary Longlastname"></asp:Label>
                            <label class="patientInfo">
                                Patient ID&nbsp;:</label>
                            <asp:Label ID="lblpatientenrolment" runat="server" Text="444545"></asp:Label>
                            <label id="lblFileRef" runat="server" class="patientInfo">
                                Existing Hosp/Clinic #&nbsp;:</label>
                            <asp:Label ID="lblexisclinicid" runat="server" Text="12345678-444444-596A"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />--%>
        <div class="border center formbg">
            <table width="100%" border="0" cellpadding="0" cellspacing="6">
                <tbody>
                    <tr id="tradd" runat="server">
                        <td class="pad5 whitebg border" style="width: 65%">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <label id="lblLocation" for="Location">
                                            Location:
                                        </label>
                                    </td>
                                    <td>
                                        <span id="Current" class="patientInfo">Current</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLocationName" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span id="New" class="required">New</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddSatellite" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="pad5 whitebg border" style="width: 35%">
                            <label id="lblTransferDate" class="required" for="TransferDate">
                                *Transfer Date:</label>
                            <asp:TextBox ID="txtTransferDate" MaxLength="11" runat="server" Width="25%"></asp:TextBox>
                            <img onclick="w_displayDatePicker('<%= txtTransferDate.ClientID %>');" alt="Date Helper"
                                height="22" hspace="3" src="../images/cal_icon.gif" width="22" border="0"/>
                            <span class="smallerlabel">(DD-MMM-YYYY)</span>
                        </td>
                    </tr>
                    <tr id="tredit" runat="server">
                        <td class="pad5 whitebg border">
                            <label id="lblLocationEdit" for="Location">
                                Location:
                            </label>
                            <span id="CurrentEdit" class="smallerlabel" style="font-weight: bold">Current </span>
                            <asp:TextBox ID="txtLocationNameEdit" runat="server"></asp:TextBox>
                            <span id="From" class="smallerlabel" style="font-weight: bold">From </span>
                            <asp:TextBox ID="txtFromSatellite" runat="server"></asp:TextBox>
                            <span id="To" class="smallerlabel required" style="font-weight: bold">To </span>
                            <asp:DropDownList ID="ddSatelliteEdit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="pad5 whitebg border">
                            <label id="LblTransDate" class="required" for="TransferDate">
                                *Transfer Date:</label>
                            <asp:TextBox ID="TxtTransDateEdit" MaxLength="11" runat="server" Width="25%"></asp:TextBox>
                            <img onclick="w_displayDatePicker('<%= TxtTransDateEdit.ClientID %>');" alt="Date Helper"
                                height="22" hspace="3" src="../images/cal_icon.gif" width="22" border="0"/>
                            <span class="smallerlabel">(DD-MMM-YYYY)</span>
                        </td>
                    </tr>
                    <tr>
                        <td  class="center" align="center" style="padding-left: 10px; padding-right: 15px"
                                        colspan="2">
                            <div class="grid">
                                <div class="rounded">
                                    <div class="top-outer">
                                        <div class="top-inner">
                                            <div class="top leftallign">
                                                <h2>
                                                    Transfers</h2>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mid-outer">
                                        <div class="mid-inner" >
                                            <div class="mid" style="cursor: pointer; height: 280px; border: 1px solid #666699;">
                                                <div id="div-gridview" class="gridview whitebg">
                                                    <asp:GridView ID="GrdTransfer" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                                        Width="100%" PageIndex="1" HorizontalAlign="Left" BorderWidth="0" GridLines="None"
                                                        CssClass="datatable" CellPadding="0" CellSpacing="0" BackColor="White" OnRowDataBound="GrdTransfer_RowDataBound"
                                                        OnSelectedIndexChanging="GrdTransfer_SelectedIndexChanging" OnSorting="GrdTransfer_Sorting">
                                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
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
                    <tr>
                        <td class="pad5 center" colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
                            <asp:Button ID="theBtn" Text="OK" CssClass="textstylehidden" runat="server" OnClick="theBtn_Click" />
                            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClientClick="WindowPrint()" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
