<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmLaboratoryTouch.ascx.cs" Inherits="Touch.Custom_Forms.frmLaboratoryTouch" %>

<div id="FormContent">
     <div id="tabs" style="width:800px">
        <ul>
        <li><a href="#tab1">Laboratory Order Summary</a></li>
        </ul>
        <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width:811px; overflow:hidden; height: 380px;">
         <asp:UpdatePanel ID="uptdLabResults" runat="server" UpdateMode="Conditional">
                   <ContentTemplate>
            <table id="referrals" style="width:100%;" cellpadding="10px" class="Section" >
             <tr>
             <td colspan="2">
             
              <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridLabOrder" runat="server" Width="100%"
                                    PageSize="4" AllowPaging="true" 
                     AllowMultiRowSelection="false" ClientSettings-Selecting-AllowRowSelect="true"
                                    ClientSettings-Resizing-AllowColumnResize="true" 
                     ShowFooter="false" ClientSettings-Resizing-EnableRealTimeResize="true"
                                    CellPadding="0" Font-Names="Verdana" Font-Size="8pt" 
                     Skin="MetroTouch" onitemdatabound="RadGridLabOrder_ItemDataBound" 
                     onpageindexchanged="RadGridLabOrder_PageIndexChanged" 
                     onselectedindexchanged="RadGridLabOrder_SelectedIndexChanged" 
                     Culture="(Default)">
                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Resizing AllowColumnResize="True" EnableRealTimeResize="True"></Resizing>
                                    </ClientSettings>
                                    <AlternatingItemStyle BorderStyle="None" />
                                    <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                                        DataKeyNames="LabOrderID" CellSpacing="0" CellPadding="0" 
                                        Font-Names="Verdana" Font-Size="8pt">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center">
                                                        No Records Found
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <RowIndicatorColumn Visible="False">
                                            <HeaderStyle Width="41px" />
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn Created="True">
                                            <HeaderStyle Width="41px" />
                                        </ExpandCollapseColumn>
                                        <Columns>
                                           
                                           <telerik:GridBoundColumn UniqueName="LabOrderDate" DataField="LabOrderDate" HeaderText="Date" >
                                           <HeaderStyle Width="100px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="LabOrderID" DataField="LabOrderID" HeaderText="Order Id">
                                            <HeaderStyle Width="100px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="Location_name" DataField="Location_name" HeaderText="Area" >
                                             <HeaderStyle Width="180px" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="OrderedBy" DataField="OrderedBy" HeaderText="Ordered By">
                                            <HeaderStyle Width="100px" />
                                            </telerik:GridBoundColumn>
                                            <%--<telerik:GridBoundColumn UniqueName="Result_status" DataField="Result_status" HeaderText="Status">
                                            </telerik:GridBoundColumn>--%>
                                           <telerik:GridTemplateColumn HeaderText="Status" >
                                           <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                             <asp:Label ID="lblstatus" runat="server"  Text='<%# Eval("Result_status") %>'></asp:Label>
                                            </ItemTemplate>
                                           </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridButtonColumn Text="Results" CommandName="Select" ButtonType="LinkButton"  HeaderText="Next Action" /> 
                                          <%-- <telerik:GridTemplateColumn HeaderText="Next Action">
                                           <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                             <telerik:RadButton ID="btnReportResults" runat="server" Text="Results" ButtonType="LinkButton" ForeColor="Blue" OnClick="BtnReportResultsClick"   ></telerik:RadButton>
                                            
                                            </ItemTemplate>
                                           </telerik:GridTemplateColumn>--%>
                                        </Columns>
                                        <AlternatingItemStyle BorderStyle="None" />
                                        </MasterTableView>
                                       <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" />
                                    <ItemStyle Font-Names="Verdana" Font-Size="8pt" />
                                       </telerik:RadGrid> 
             </td>
             </tr>
               
               <tr>
               <td colspan="2" align="center">
               <table width="50%">
                <tr>
                <td>
                 <telerik:RadButton ID="btnNewOrder" runat="server" Text="New Order" ButtonType="LinkButton" OnClientClicked="parent.ShowLoading" OnClick="BtnNewOrderClick" ></telerik:RadButton>
                </td>

                <td>
               <telerik:RadButton ID="btnNewOrderCancel" runat="server" Text="Cancel" Visible="false" 
                        ButtonType="LinkButton" onclick="btnNewOrderCancel_Click"  ></telerik:RadButton>
               </td>
                </tr>
                
               </table>
               </td>
               </tr>
               
  
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    
    </div>