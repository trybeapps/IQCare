<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_LabEvaluation.ascx.cs" Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_LabEvaluation" %>


<div class="center formbg">
<table cellspacing="6" cellpadding="0" width="100%" border="0">
    <tr>
        <td class="form" style="width: 100%" align="left">
            <asp:Button ID="btnLab" runat="server" Font-Bold="True"
                Text="Order Lab Tests" onclick="btnLab_Click" />
            <br />
        </td>
    </tr>
    <tr>
        <td class="form" style="width: 100%; ">
            <div ID="divlabdiagostic" style="display: block;">
                <table width="100%">
                    <tbody>
                        <tr>
                            <td align="left" style="width: 40%">
                                <label>
                                Review of lab/other diagnostic test:</label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="txtlabdiagnosticinput" runat="server" Rows="4" 
                                    TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </td>
    </tr>
</table>
</div>

