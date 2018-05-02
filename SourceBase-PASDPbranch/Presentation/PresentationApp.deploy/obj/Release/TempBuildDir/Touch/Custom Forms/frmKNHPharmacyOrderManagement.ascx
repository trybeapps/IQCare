<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmKNHPharmacyOrderManagement.ascx.cs"
    Inherits="Touch.Custom_Forms.frmKNHPharmacyOrderManagement" %>
<div id="tabs" style="width: 800px">
    <ul>
        <li><a href="#tab1">Pharmacy Order Management</a></li>
    </ul>
    <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
        height: 350px;">
        <asp:UpdatePanel ID="uptdpharmacyResults" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <telerik:RadGrid ID="rgviewpharmacyform" Skin="MetroTouch" AutoGenerateColumns="false"
                                runat="server" OnItemCommand="rgviewpharmacyform_ItemCommand" OnItemDataBound="rgviewpharmacyform_ItemDataBound">
                                <MasterTableView PageSize="5" Font-Size="10" DataKeyNames="OrderID">
                                    <Columns>
                                        <telerik:GridBoundColumn SortExpression="Date" HeaderText="Date" HeaderButtonType="TextButton"
                                            DataField="Date" UniqueName="Date" DataFormatString="{0:dd-MMM-yyyy}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="OrderID" HeaderText="Order ID" HeaderButtonType="TextButton"
                                            DataField="OrderID">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="Servicearea" HeaderText="Service Area" HeaderButtonType="TextButton"
                                            DataField="ServiceArea">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="Prescriber" HeaderText="Prescriber" HeaderButtonType="TextButton"
                                            DataField="Prescriber">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="DispenseStatus" HeaderText="Dispense Status"
                                            HeaderButtonType="TextButton" DataField="DispenseStatus">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="NoRefills" HeaderText="No. Refills" HeaderButtonType="TextButton"
                                            DataField="NoRefills">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="visitid" DataField="visit_id" Display="false">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn SortExpression="RefillExpiration" HeaderText="Refill Expiration"
                                    HeaderButtonType="TextButton" DataField="RefillExpiration" UniqueName="RefillExpiration"
                                    DataFormatString="{0:dd-MMM-yyyy}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="OriginalOrderID" HeaderText="Original Order ID"
                                    HeaderButtonType="TextButton" DataField="OriginalOrderID">
                                </telerik:GridBoundColumn>--%>
                                        <telerik:GridButtonColumn SortExpression="Edit" Visible="True" CommandName="Edit1"
                                            HeaderText="Edit" DataTextField="Edit">
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn Visible="false" HeaderButtonType="TextButton" DataField="NextAction">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn SortExpression="NextAction" ButtonType="LinkButton" UniqueName="NextAction"
                                            CommandName="Next Action" HeaderText="Action" DataTextField="NextAction">
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <table style="width: 100%">
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadButton ID="rdneworder" runat="server" Text="New Order" OnClientClicked="parent.ShowLoading"
                                    OnClick="rdneworder_Click">
                                </telerik:RadButton>
                            </td>
                            <td>
                                <telerik:RadButton ID="rdcancel" runat="server" Text="Cancel">
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
