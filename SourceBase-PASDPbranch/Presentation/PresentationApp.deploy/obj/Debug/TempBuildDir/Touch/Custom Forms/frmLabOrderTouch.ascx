<%@ Control Language="C#" AutoEventWireup="True" Inherits=" Touch.Custom_Forms.frmLabOrderTouch" Codebehind="frmLabOrderTouch.ascx.cs" %>
<div style="visibility: collapse">
    <telerik:RadButton ID="btnSave" runat="server" onclick="BtnSaveClick" OnClientClicking="LoadingPanel" Visible="true"
        Text="Save">
    </telerik:RadButton>
</div>

<div id="FormContent">

            <div id="tabs" style="width: 800px">
                <ul>
                    <li><a href="#tab1">Laboratory New Order</a></li>
                </ul>
                <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden; height: 380px;">
                    <asp:UpdatePanel ID="updtLabOrder" runat="server">
                    <ContentTemplate>
                    <table id="LabOrderTable" style="width: 100%;" cellpadding="10px" class="Section">
                        <tr>
                        <td colspan="4">
                        <asp:Label ID="lblerr" runat="server" Text=""></asp:Label>
                        </td>
                        </tr>
                        <tr>
                            <td style="width: 23%;">
                                Preclinic lab
                            </td>
                            <td style="width: 28%;">
                                <telerik:RadButton ID="btnScheduledYes" runat="server" Width="52px" GroupName="Schedue"
                                    ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="No" />
                                        <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                    </ToggleStates>
                                </telerik:RadButton>
                            </td>
                            <td style="width: 23%;">
                                Lab to be done on
                            </td>
                            <td style="width: 28%;">
                                <telerik:RadDatePicker ID="dtVisitDate" runat="server" Skin="MetroTouch" 
                                    Culture="(Default)">
                                    <Calendar ID="Calendar1" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                        Skin="MetroTouch" runat="server">
                                    </Calendar>
                                    <DateInput ID="DateInput1" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
                                        LabelWidth="0px" runat="server">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                        <FocusedStyle Resize="None"></FocusedStyle>
                                        <DisabledStyle Resize="None"></DisabledStyle>
                                        <InvalidStyle Resize="None"></InvalidStyle>
                                        <HoveredStyle Resize="None"></HoveredStyle>
                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2" align="left">
                                                Pre-Selected Labs
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <telerik:RadComboBox ID="rcbPreSelectedLabTest" runat="server" Text="aSomeTest" AutoPostBack="false"
                                                    Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                                    Width="500px">
                                                </telerik:RadComboBox>
                                                <%--<asp:CheckBoxList ID="LabTest" runat="server">
                     <asp:ListItem Text="CD4Count"></asp:ListItem>
                     <asp:ListItem Text="CD4Count"></asp:ListItem>
                     <asp:ListItem Text="CD4Count"></asp:ListItem>
                     <asp:ListItem Text="CD4Count"></asp:ListItem>
                     <asp:ListItem Text="CD4Count"></asp:ListItem>
                     <asp:ListItem Text="CD4Count"></asp:ListItem>
                     <asp:ListItem Text="CD4Count"></asp:ListItem>
                    </asp:CheckBoxList>--%>
                                            </td>
                                            <td valign="bottom" align="left">
                                                <telerik:RadButton ID="btnAddDrug" Text="Add" runat="server" Skin="MetroTouch" OnClick="BtnAddDrugClick">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                Select Lab Test
                            </td>
                            <td colspan="3" align="left">
                                <fieldset style="width: 400px">
                                    <telerik:RadAutoCompleteBox ID="AutoselectLabTest" runat="server" Skin="MetroTouch"
                                        Width="500px" DropDownWidth="500px" OnEntryAdded="Autoselectdrug_EntryAdded">
                                    </telerik:RadAutoCompleteBox>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                             <a id="sGrid" href="#sGrid"></a>
                                <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridLabTest" runat="server" Width="100%"
                                    PageSize="5" AllowPaging="false"  AllowMultiRowSelection="false" ClientSettings-Selecting-AllowRowSelect="true"
                                    ClientSettings-Resizing-AllowColumnResize="true" ShowFooter="true" ClientSettings-Resizing-EnableRealTimeResize="true"
                                    CellPadding="0" Font-Names="Verdana" Font-Size="10pt" Skin="MetroTouch"
                                    onitemcreated="RadGridLabTest_ItemCreated" OnNeedDataSource="RadGridLabTest_NeedDataSource"
                                    onitemcommand="RadGridLabTest_ItemCommand" 
                                    OnDeleteCommand="RadGridLabTest_DeleteCommand" Culture="(Default)">
                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Resizing AllowColumnResize="True" EnableRealTimeResize="True"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                                        DataKeyNames="LabTestID" CellSpacing="0" CellPadding="0">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center">
                                                        No Records Found
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Laboratory Test Name" HeaderStyle-Font-Bold="true">
                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabTestName" runat="server" Text='<%# Eval("SubTestName") %>'></asp:Label>
                                                    <asp:Label ID="lblLabSubTestID" runat="server" Visible="false" Text='<%# Eval("SubTestId") %>'></asp:Label>
                                                    <asp:Label ID="lblLabTestID" runat="server" Visible="false" Text='<%# Eval("LabTestId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Units" HeaderStyle-Font-Bold="true">
                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabTestUom" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Test Type" HeaderStyle-Font-Bold="true">
                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabTestType" runat="server" Text='<%# Eval("LabDepartmentName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Test Type" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnRemove" runat="server" Skin="MetroTouch" Text="Remove"
                                                        ForeColor="Blue" CommandName="Delete" ButtonType="LinkButton">
                                                    </telerik:RadButton>
                                                </ItemTemplate>
                                                
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <FooterStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                    <HeaderStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                </telerik:RadGrid>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4">
                        <fieldset>
                          <table>
                          <tr>
                           <td>
                           *Ordered by:
                           </td>
                           <td>
                             <telerik:RadComboBox ID="rcbOrderBy" runat="server" Text="Select" AutoPostBack="false"
                                                    Skin="MetroTouch"  CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"  >
                                                </telerik:RadComboBox>
                           </td>
                           <td>
                           *Order Date:
                           </td>
                           <td>
                            <telerik:RadDatePicker ID="RadDateOrder" runat="server" Skin="MetroTouch" Culture="(Default)">
                                    <Calendar ID="Calendar2" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                        Skin="MetroTouch" runat="server">
                                    </Calendar>
                                    <DateInput ID="DateInput2" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
                                        LabelWidth="0px" runat="server">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                        <FocusedStyle Resize="None"></FocusedStyle>
                                        <DisabledStyle Resize="None"></DisabledStyle>
                                        <InvalidStyle Resize="None"></InvalidStyle>
                                        <HoveredStyle Resize="None"></HoveredStyle>
                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                           </td>
                          </tr>
                          <tr>
                            <td>
                            Report By:
                            </td>
                            <td>
                            <telerik:RadComboBox ID="rcbReportedBy" runat="server" Text="Select" AutoPostBack="false"
                                                    Skin="MetroTouch"  CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" Enabled="false" >
                                                    
                                                </telerik:RadComboBox>
                            </td>
                            <td>
                            *Reported Date: 
                            </td>
                            <td>
                            <telerik:RadDatePicker ID="RadDateReportDate" runat="server" Skin="MetroTouch" Culture="(Default)">
                                    <Calendar ID="Calendar3" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                        Skin="MetroTouch" runat="server" Enabled="false">
                                    </Calendar>
                                    <DateInput ID="DateInput3" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
                                        LabelWidth="0px" runat="server" Enabled="false">
                                        <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                        <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                        <FocusedStyle Resize="None"></FocusedStyle>
                                        <DisabledStyle Resize="None"></DisabledStyle>
                                        <InvalidStyle Resize="None"></InvalidStyle>
                                        <HoveredStyle Resize="None"></HoveredStyle>
                                        <EnabledStyle Resize="None"></EnabledStyle>
                                    </DateInput>
                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                </telerik:RadDatePicker>
                            </td>
                          </tr>
                          </table>
                        
                        </fieldset>


                        </td>
                        </tr>
                        <tr>
                         <td colspan="4">
                          <table align="center">
                          <tr>
                           <td>
                           <%-- <telerik:RadButton ID="btnSave" runat="server" Text="Save" Skin="MetroTouch" OnClientClicked="parent.ShowLoading" OnClick="BtnSaveClick"></telerik:RadButton>--%>
                           </td>
                           <td>
                           <telerik:RadButton ID="btnBack" runat="server" Text="Back" OnClick="BtnBack_Click" OnClientClicked="parent.ShowLoading" Skin="MetroTouch" Visible="false"></telerik:RadButton>
                           </td>
                           <td>
                           <telerik:RadButton ID="btnPrint" runat="server" Text="Print" Skin="MetroTouch" Visible="false"></telerik:RadButton>
                           </td>
                          </tr>
                          </table>
                         </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hiddTestID" runat="server" />
                    <asp:HiddenField ID="hiddTestAddTestID" runat="server" />
                    <asp:HiddenField ID="hdnTestAddTestID" runat="server" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

<div id="frmLabOrder_ScriptBlock" runat="server" style="display: none;">
   <script type="text/javascript" language="javascript">
       function FormValidatedOnSubmit(Msg) {
           alert(Msg);
           return false;
       }
       function LoadingPanel(sender, args) {
           parent.ShowLoading();
       }

       $('.rgPageFirst').live('click', function () {
           __doPostBack(this.name, '');
           return false;
       });

       $('.rgPageNext').live('click', function () {
           __doPostBack(this.name, '');
           return false;
       });

       $('.rgPagePrev').live('click', function () {
           __doPostBack(this.name, '');
           return false;
       });

       $('.rgPageLast').live('click', function () {
           __doPostBack(this.name, '');
           return false;
       });

       $('.rgCollapse').live('click', function () {
           __doPostBack(this.name, '');
           return false;
       });

       $('.rgExpand').live('click', function () {
           __doPostBack(this.name, '');
           return false;
       });

       $('.rgExrgNumPart').live('click', function () {
           __doPostBack(this.name, '');
           return false;
       });

   </script>

</div>
