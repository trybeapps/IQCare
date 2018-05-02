<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="True" Inherits="DrugMaster_List" Title="Untitled Page" Codebehind="frmAdmin_DrugList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <%--<link type="text/css" href="../Style/_assets/css/grid.css" rel="stylesheet" />
<link type="text/css" href="../Style/_assets/css/round.css" rel="stylesheet" />
<form id ="masterdruglist" method="post" runat="server"> --%>
    <div>
        <h1 class="margin" style="padding-left: 10px;">
            Items</h1>
        <div class="center" style="padding: 5px;">
            <div class="border center formbg">
                <table width="100%" border="0" cellpadding="0" cellspacing="6">
                    <tbody>
                        <tr>
                            <td class="border pad5 formbg" valign="top">
                                <div class="grid">
                                    <div class="rounded">
                                        <div class="top-outer">
                                            <div class="top-inner">
                                                <div class="top">
                                                    <h2>
                                                    </h2>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mid-outer">
                                            <div class="mid-inner">
                                                <div class="mid">
                                                    <div id="grd_custom" class="GridView whitebg" style="cursor: pointer; width: 900px;
                                                        height: 300px; overflow: scroll;">
                                                        <asp:GridView ID="grdMasterDrugs" CssClass="datatable" CellPadding="0" CellSpacing="0"
                                                            runat="server" OnRowDataBound="grdMasterDrugs_RowDataBound" AutoGenerateColumns="False"
                                                            PageSize="1" AllowSorting="True" BorderWidth="0" GridLines="None" Height="99px"
                                                            OnSelectedIndexChanging="grdMasterDrugs_SelectedIndexChanging" OnSorting="grdMasterDrugs_Sorting">
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
                            </td>
                        </tr>
                        <tr>
                            <td class="pad5 center" align="center">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
