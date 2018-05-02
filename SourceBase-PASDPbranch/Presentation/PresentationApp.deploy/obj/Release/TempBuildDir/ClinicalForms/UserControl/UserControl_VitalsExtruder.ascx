<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControl_VitalsExtruder.ascx.cs" Inherits="PresentationApp.ClinicalForms.UserControl.UserControl_VitalsExtruder" %>
<%@ Register src="UserControl_AllergyExtruder.ascx" tagname="UserControl_AllergyExtruder" tagprefix="uc1" %>
<%@ Register src="UserControlKNH_LabResults.ascx" tagname="UserControlKNH_LabResults" tagprefix="uc2" %>
<%@ Register src="UserControl_ARVHistoryExtruder.ascx" tagname="UserControl_ARVHistoryExtruder" tagprefix="uc3" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
    
    
</style>
<div style="height: 900px; background-color: #FFFFFF;">
<table cellpadding="2">
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Sex:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblSex" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="DOB:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDOB" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Age:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblAge" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="Phone:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblPhone" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="District:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDistrict" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="BMI:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblBMI" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
</table>
<hr style="height:3px;border:none;color:#333;background-color:#800000;" />


<uc1:UserControl_AllergyExtruder ID="UserControl_AllergyExtruder1" 
    runat="server" />

    <hr style="height:3px;border:none;color:#333;background-color:#800000;" />


<uc3:UserControl_ARVHistoryExtruder ID="UserControl_ARVHistoryExtruder1" 
    runat="server" />

    </div>


