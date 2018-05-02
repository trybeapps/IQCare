<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_Signature.ascx.cs" Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_Signature" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>

<table class="style1">
    <tr>
        <td align="right" width="50%">
            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Signature:"></asp:Label>
        </td>
        <td align="left">
            <asp:Label ID="lblSignature" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
</table>

