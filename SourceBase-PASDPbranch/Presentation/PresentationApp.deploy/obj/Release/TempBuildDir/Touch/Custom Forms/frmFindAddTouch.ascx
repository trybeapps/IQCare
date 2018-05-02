<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.frmFindAddTouch" Codebehind="frmFindAddTouch.ascx.cs" %>

<telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Panel1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1">
                    </telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxPanel runat="server" ID="Panel1">
<table>
    <tr>
        <td>
            Folder No:
        </td>
        <td>
            <telerik:RadTextBox ID="txtFolder" runat="server" Skin="MetroTouch">
            </telerik:RadTextBox>
        </td>
        <td>
            ID Number:
        </td>
        <td>
            <telerik:RadTextBox ID="txtIDno" runat="server" Skin="MetroTouch">
            </telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            First Name:
        </td>
        <td>
            <telerik:RadTextBox ID="txtFName" runat="server" Skin="MetroTouch">
            </telerik:RadTextBox>
        </td>
        <td>
            Last Name:
        </td>
        <td>
            <telerik:RadTextBox ID="txtLName" runat="server" Skin="MetroTouch">
            </telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:Radbutton ID="btnFind" runat="server" Skin="MetroTouch" Text="Find" 
                onclick="btnFind_Click"></telerik:Radbutton>
        </td>
        <td colspan="3">
            <telerik:RadButton ID="btnAdd" runat="server" Skin="MetroTouch" Text="Add"></telerik:RadButton>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <telerik:RadListBox ID="results" runat="server" Skin="MetroTouch" 
                Visible="False">
                <ButtonSettings TransferButtons="All" />
                <Items>
                    <telerik:RadListBoxItem runat="server" Text="John Jacobus" />
                    <telerik:RadListBoxItem runat="server" Text="John Peters" />
                    <telerik:RadListBoxItem runat="server" Text="John Smith" />
                </Items>
            </telerik:RadListBox>
        </td>
    </tr>
</table>

    </telerik:RadAjaxPanel>