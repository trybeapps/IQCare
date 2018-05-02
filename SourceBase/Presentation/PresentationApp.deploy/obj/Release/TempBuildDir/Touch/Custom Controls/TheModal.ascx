<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TheModal.ascx.cs" Inherits="Touch.Custom_Controls.TheModal" %>


<div>
<asp:UpdatePanel ID="updtTheModal" runat="server" UpdateMode="Conditional">
<ContentTemplate>

<telerik:RadTextBox ID="txtSetBox" runat="server"></telerik:RadTextBox><br />

<telerik:RadButton ID="btnOpenWin" Text="open dialog" AutoPostBack="false"  CommandArgument="TheModalWin_rwVital" OnClientClicked="OpenModalASPX" runat="server"></telerik:RadButton>



</ContentTemplate>
</asp:UpdatePanel>


</div>