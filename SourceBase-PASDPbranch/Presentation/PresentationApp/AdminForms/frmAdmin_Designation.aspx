<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="True" Inherits="AdminDesignation" Title="Untitled Page" Codebehind="frmAdmin_Designation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <div>
        <h1 class="margin" style="padding-left: 10px;">
         Designation</h1>
        <div class="border center formbg">
            <h2 class="forms" align="left">
                <asp:Label ID="lblH2" runat="server" Text=""></asp:Label></h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad5 whitebg" width="50%">
                            <label class="right" for="LastName">
                                Employee Designation:</label>
                            <asp:TextBox ID="txtDesignationName" runat="server"></asp:TextBox>
                        </td>
                        <td class="border pad5 whitebg" width="50%">
                            <label class="right" for="LastName">
                                Form Signature:</label>
                            <asp:RadioButton ID="rdouserName" GroupName="usr" Text="Display username only" runat="server" />
                            <asp:RadioButton ID="rdouserlist" GroupName="usr" Text="Display username list" runat="server" />
                               
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg right" width="50%">
                            <label class="right">
                                Priority:</label>
                            <asp:TextBox ID="txtSeq" runat="server">
                            </asp:TextBox>
                        </td>
                        <td class="border pad5 whitebg" width="50%">
                            <asp:Label class="right30" runat="server" ID="lblStatus">Status:</asp:Label>
                            <asp:DropDownList ID="ddStatus" runat="server">
                                <asp:ListItem Text="Active" Value="0"></asp:ListItem>
                                <asp:ListItem Text="InActive" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table>
                <tbody>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /><asp:Button
                        ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></tbody></table>
            <br />
        </div>
    </div>
</asp:Content>
