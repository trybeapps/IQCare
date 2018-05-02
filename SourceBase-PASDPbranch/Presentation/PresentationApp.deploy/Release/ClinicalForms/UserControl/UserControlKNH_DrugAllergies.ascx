<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_DrugAllergies.ascx.cs" Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_DrugAllergies" %>
<link href="../../Style/styles.css" rel="stylesheet" type="text/css" />
<link href="../../Style/calendar.css" rel="stylesheet" type="text/css" />
<link href="../../Style/_assets/css/grid.css" rel="stylesheet" type="text/css" />
<link href="../../Style/_assets/css/round.css" rel="stylesheet" type="text/css" />
<link href="../../Style/StyleSheet.css" rel="stylesheet" type="text/css" />
<div class="center formbg">
<table cellspacing="6" cellpadding="0" width="100%" border="0">
    <tr>
        <td class="form">
            <div class="GridView whitebg" style="cursor: pointer;">
                                            <div class="grid">
                                                <div class="rounded">
                                                    <div class="top-outer">
                                                        <div class="top-inner">
                                                            <div class="top">
                                                                <h2>
                                                                    Allergy Details</h2>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="mid-outer">
                                                        <div class="mid-inner">
                                                            <div class="mid" style="height: auto; overflow: auto">
                                                                <div id="div-gridview" class="GridView whitebg">
                                                                    <asp:GridView ID="grdDrugAllergy" runat="server" 
                                                                        AutoGenerateColumns="False" Width="100%"  BorderWidth="0"
                                                                        GridLines="None" CssClass="datatable" CellPadding="0" CellSpacing="0">
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
                                        </div>
        </td>
    </tr>
    <tr>
    <td class="form">
        
        <asp:Button ID="btnAllergies" runat="server" OnClick="btnAllergies_Click" 
            Text="Add Allergies" />

    
    </td>
    </tr>
</table>
</div>

