<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="True" Inherits="AdminForms_frmAdmin_AuditTrail" EnableEventValidation="false" Codebehind="frmAdmin_AuditTrail.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <h3 class="margin" align="left" style="padding-left: 5px;">
        <asp:Label ID="lblh2" runat="server" Text="Audit Trail"></asp:Label></h3>
        <div class="border center formbg">
            <table width="100%" border="0" cellpadding="0" cellspacing="6">
                <tbody>
                    <tr>
                        <td class="pad5 whitebg border" style="width: 50%" colspan="2">
                            <label id="lblForms" for="Forms">
                                Forms:
                            </label>
                            <asp:DropDownList ID="ddAuditTrail" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="pad5 whitebg border" width="50%">
                            <label id="lblFromDate" class="required" for="FromDate">
                                *From Date:</label>
                            <asp:TextBox ID="txtFromDate" MaxLength="11" runat="server" Width="25%"></asp:TextBox>
                            <img onclick="w_displayDatePicker('<%= txtFromDate.ClientID %>');" alt="Date Helper"
                                height="22" hspace="3" src="../images/cal_icon.gif" width="22" border="0" />
                            <span class="smallerlabel">(DD-MMM-YYYY)</span>
                        </td>
                        <td class="pad5 whitebg border" width="50%">
                            <label id="lblToDate" class="required" for="ToDate">
                                *To Date:</label>
                            <asp:TextBox ID="txtToDate" MaxLength="11" runat="server" Width="25%"></asp:TextBox>
                            <img onclick="w_displayDatePicker('<%= txtToDate.ClientID %>');" alt="Date Helper"
                                height="22" hspace="3" src="../Images/cal_icon.gif" width="22" border="0" />
                            <span class="smallerlabel">(DD-MMM-YYYY)</span>
                        </td>
                    </tr>
                     <tr>
                        <td class="pad5 whitebg border" width="50%" colspan="2">
                        <asp:Button ID="btnView" runat="server" Text="View" onclick="btnView_Click" />
                        <asp:Button ID="btnExporttoExcel" runat="server" Text="Export to Excel" 
                                onclick="btnExporttoExcel_Click" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                      <td class="center" align="center" style="padding-left: 10px; padding-right: 15px"
                                        colspan="2">
                                        <div class="grid">
                                            <div class="rounded">
                                                <div class="top-outer">
                                                    <div class="top-inner">
                                                        <div class="top">
                                                            <h2>
                                                                Searched Result</h2>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="mid-outer">
                                                    <div class="mid-inner">
                                                        <div class="mid" style="cursor: pointer; height: 280px; overflow:auto; border: 1px solid #666699;">
                                                            <div id="div-gridview" class="whitebg" >
                                                                <asp:GridView ID="GrdAuditTrail" runat="server" Width="100%" OnRowDataBound="GrdAuditTrail_RowDataBound"
                                                                    PageSize="1" CssClass="datatable" AutoGenerateColumns="false" CellPadding="0"
                                                                    CellSpacing="0" BorderWidth="0" GridLines="None" AllowSorting="True">
                                                                      <HeaderStyle CssClass="searchresultfixedheader" Height="20px"></HeaderStyle>
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
                        <td>
                        </td>
                    </tr>
                </tbody>
            </table>

        </div>
</asp:Content>
