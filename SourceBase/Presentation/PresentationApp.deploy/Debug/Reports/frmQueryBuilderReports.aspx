<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="Reports_frmQueryBuilderReports"
    Title="Untitled Page" Codebehind="frmQueryBuilderReports.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <div style="padding-left: 5px; padding-right: 5px; padding-top: 0px;">
        <%--<form id="QueryBuilderReports" method="post" runat="server">--%>
        <div class="nomargin">
            <h2 class="nomargin">
                QueryBuilder Reports
            </h2>
        </div>
        <div class="border center formbg">
            <table class="center" cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td style="width: 65%" align="right">
                            &nbsp;
                        </td>
                        <td style="width: 35%">
                            &nbsp;
                        </td>
                    </tr>
                        <tr>
                                <td class="border pad5 whitebg" valign="top" colspan="2">
                                    <table width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="width: 37%" align="right">
                                                    <label>
                                                        ReportsCategory:</label>&nbsp;
                                                    </td>
                                                <td style="width: 10%" align="left">
                                                    <asp:DropDownList ID="ddlCategory" Width="120px" runat="server" onselectedindexchanged="ddlCategory_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    
                                                </td>
                                                <td style="width: 30%">
                                                    &nbsp;&nbsp;
                                                    &nbsp;&nbsp;
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:Label ID="Label1" runat="server" Text="Reports Name:" Font-Bold="True"></asp:Label>
                                                </td>
                                                    
                                            </tr>
                                             <tr>
                                                <td colspan="10">
                                                    <asp:Panel ID="dyanamicRadiobutton" CssClass="left whitebg" align="left" runat="server">
                                                        <asp:RadioButtonList RepeatLayout ="Flow" Width ="80%"  ID="rdButtonList" runat="server">
                                                        </asp:RadioButtonList>
                                                    </asp:Panel>
                                                 </td>
                                                    
                                            </tr> 
                                            <tr>
                                                <td colspan="10">
                                                    &nbsp;</td>
                                                    
                                            </tr>
                                             <tr>
                                                <td colspan="10">
                                                    &nbsp;</td>
                                                    
                                            </tr> <tr>
                                                <td colspan="10">
                                                    &nbsp;</td>
                                                    
                                            </tr> <tr>
                                                <td colspan="10">
                                                    &nbsp;</td>
                                                    
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                    <tr>
                        <td align="center" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
