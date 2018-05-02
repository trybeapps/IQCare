<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmDBBackup" Codebehind="frmDBBackup.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/IQCare.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <div>
        <h1 class="topmargin">
            System Back-Up/Restore</h1>
        <div class="center" style="padding: 5px;">
            <div class="border formbg center">
                <table class="pad5 formbg" width="100%" cellspacing="6">
                    <tr>
                        <td class="whitebg center border" valign="top" width="100%">
                            <asp:Label ID="Label1" runat="server" Text="Backup Directory :"></asp:Label>
                            <asp:TextBox ID="txtbakuppath" runat="server" Width="406px" Text="c:\IQCareDBBackup"></asp:TextBox>
                            <div width="50%">
                                <input id="chkDeidentified" type="checkbox" runat="server" />
                                <label>
                                    Make Backup of the Database with Identifiers Removed</label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="center">
                            <asp:Button ID="btnBackup" runat="server" Text="Backup" OnClick="btnBackup_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div class="border formbg center">
                <table class="pad5 formbg" width="100%" cellspacing="6">
                    <tr>
                        <td class="whitebg center border" valign="top" width="100%">
                            <asp:Label ID="Label2" runat="server" Text="Restore File :"></asp:Label>
                            <asp:TextBox ID="txtRestore" runat="server" Width="406px"></asp:TextBox>
                            <asp:Button ID="btnBrowse" runat="server" Text="Browse" OnClick="btnBrowse_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="center">
                            <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table cellspacing="6" width="100%" border="0">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        function GetControl() {
            document.forms[0].submit();
        }
    </script>
</asp:Content>
