<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmLabResultsTouch.ascx.cs" Inherits="Touch.Custom_Forms.frmLabResultsTouch" %>

<div style="visibility: collapse">
    <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClientClicking="LoadingPanel" Skin="MetroTouch" OnClick="BtnSaveClick">
    </telerik:RadButton>
</div>

<div id="FormContent" >
            <div id="tabs" style="width: 800px">
                <ul>
                    <li><a href="#tab1">Laboratory Order Results</a></li>
                </ul>
                <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden; height: 380px;">
                   
                   
                   <asp:UpdatePanel ID="uptdLabResults" runat="server" UpdateMode="Conditional">
                   <ContentTemplate>
                    <table id="LabOrderTable" style="width: 100%;" cellpadding="10px" class="Section">
                        <tr>
                        <td colspan="4">
                        <asp:Label ID="lblerr" runat="server" ></asp:Label>
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
                                        Width="500px" DropDownWidth="500px" OnEntryAdded="Autoselectdrug_EntryAdded" Enabled="true">
                                    </telerik:RadAutoCompleteBox>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                              
                             <a id="sGrid" href="#sGrid"></a>
                                <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridLabTest" runat="server" Width="100%"
                                    PageSize="5" AllowPaging="false" AllowMultiRowSelection="false" ClientSettings-Selecting-AllowRowSelect="true"
                                    ClientSettings-Resizing-AllowColumnResize="true" ShowFooter="true" ClientSettings-Resizing-EnableRealTimeResize="true"
                                    CellPadding="0" Font-Names="Verdana" Font-Size="10pt" Skin="MetroTouch" 
                                    onitemcreated="radGridLabResult_ItemCreated" 
                                    onitemcommand="RadGridLabTest_ItemCommand" GroupRenderMode="Flat"  OnDeleteCommand="RadGridLabTest_ItemDeleted" 
                                    onitemdatabound="RadGridLabTest_ItemDataBound">
                                    
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
                                            <telerik:GridTemplateColumn HeaderText="Test Name" HeaderStyle-Font-Bold="true">
                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabTestName" runat="server" Text='<%# Eval("SubTestName") %>'></asp:Label>
                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("SubTestId") %>'></asp:Label>
                                                    <asp:Label ID="lblDeleteFlag" runat="server" Visible="false" Text='<%# Eval("DeleteFlag") %>'></asp:Label>

                                                </ItemTemplate>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            </telerik:GridTemplateColumn>
                                           <%-- <telerik:GridTemplateColumn HeaderText="Units" HeaderStyle-Font-Bold="true">
                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabTestUom" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            </telerik:GridTemplateColumn>--%>
                                            <telerik:GridTemplateColumn HeaderText="Department Name" HeaderStyle-Font-Bold="true">
                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="130px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLabTestType" runat="server" Text='<%# Eval("LabDepartmentName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn  HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <telerik:RadButton ID="btnRemove" runat="server" Skin="MetroTouch" Text="Remove"
                                                        ForeColor="Blue" CommandName="Delete" ButtonType="LinkButton">
                                                    </telerik:RadButton>
                                               </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <NestedViewSettings>
                                        <ParentTableRelation>
                                         <telerik:GridRelationFields DetailKeyField="SubTestId" MasterKeyField="SubTestId" />  
                                        </ParentTableRelation>
                                        </NestedViewSettings>
                                        <NestedViewTemplate>
                                            <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridLabResult" runat="server"
                                                Width="100%" PageSize="5" AllowPaging="true" AllowMultiRowSelection="false" ClientSettings-Selecting-AllowRowSelect="false"
                                                ClientSettings-Resizing-AllowColumnResize="false" ShowFooter="false" ClientSettings-Resizing-EnableRealTimeResize="false"
                                                CellPadding="0" Font-Names="Verdana" Font-Size="10pt" ShowHeader="false" Skin="MetroTouch" 
                                                 HierarchyLoadMode="ServerOnDemand" OnItemDataBound="RadGridResut_ItemDataBound" DataKeyNames="LabTestID" 
                                                   OnNeedDataSource="RadGridLabResult_NeedDataSource">
                                              <MasterTableView Name="ChildGrid">
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
                                                <telerik:GridTemplateColumn HeaderText="Lab Test Name" HeaderStyle-Font-Bold="true" UniqueName="SubTestId">
                                                 <ItemTemplate>
                                                 <telerik:RadNumericTextBox ID="txtRadValue" runat="server"  Text='<%# Eval("TestResults") %>' Skin="MetroTouch" >
                                                 </telerik:RadNumericTextBox>
                                                 <telerik:RadTextBox ID="txtAlphaRadValue" runat="server" Text='<%# Eval("TestResults1") %>' Skin="MetroTouch">
                                                 </telerik:RadTextBox>
                                                    <asp:RadioButtonList id="btnRadRadiolist" runat ="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                                                    <asp:CheckBoxList ID="chkBoxList" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
                                                    <asp:DropDownList ID="ddlList" runat="server"></asp:DropDownList>
                                                    <%-- <telerik:RadButton ID="btnRadRadiolist" runat="server" Width="52px" GroupName="BirthBCG"
                                                         ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                     </telerik:RadButton>--%>
                                                 
                                                 <asp:Label ID="lblUnitName" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                 <asp:Label ID="lblControlType" runat="server" Text='<%# Eval("Control_type") %>' Visible="false"></asp:Label>
                                                   <asp:Label ID="lblLabSubTestId" runat="server"  Visible="false" Text='<%# Eval("SubTestID") %>'></asp:Label>
                                                   <%--<asp:Label ID="lblLabTestID" runat="server"  Visible="false" Text='<%# Eval("LabTestId") %>'></asp:Label>--%>
                                                   <asp:Label ID="lblMinBoundaryVal" runat="server"  Visible="false" Text='<%# Eval("MinBoundaryValue") %>'></asp:Label>
                                                   <asp:Label ID="lblMaxBoundaryVal" runat="server"  Visible="false" Text='<%# Eval("MaxBoundaryValue") %>'></asp:Label>
                                                   <asp:Label ID="lblTestResultId" runat="server"  Visible="false" Text='<%# Eval("TestResultId") %>'></asp:Label>
                                                   <asp:Label ID="lblTestResults" runat="server"  Visible="false" Text='<%# Eval("TestResults") %>'></asp:Label>
                                                  <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridArvMutation" runat="server"
                                                    Width="100%" PageSize="5" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false"
                                                    ClientSettings-Selecting-AllowRowSelect="true" ClientSettings-Resizing-AllowColumnResize="true"
                                                    ShowFooter="true" ClientSettings-Resizing-EnableRealTimeResize="true"  OnItemDataBound="RadGridArvMutation_ItemDataBound"
                                                    OnItemCommand="RadGridArvMutation_ItemCommand" OnDeleteCommand="RadGridArvMutation_DeleteCommand"
                                                    CellPadding="0" Font-Names="Verdana" Font-Size="10pt">
                                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                        <Resizing AllowColumnResize="True" EnableRealTimeResize="True"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                                                        DataKeyNames="ID" CellSpacing="0" CellPadding="0">
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
                                                            <telerik:GridTemplateColumn HeaderText="ARV Type" HeaderStyle-Font-Bold="true">
                                                                 <HeaderStyle Font-Size="10px" Wrap="False" Width="180px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                                                    <asp:Label ID="lblArvType" runat="server"  Text='<%# Eval("ArvType") %>'></asp:Label>
                                                                    <asp:Label ID="lblArvTypeID" runat="server"  Visible="false" Text='<%# Eval("ArvTypeID") %>'></asp:Label>
                                                                    <asp:Label ID="lblDeleteFlag" runat="server"  Visible="false" Text='<%# Eval("DeleteFlag") %>'></asp:Label>
                                                                    <asp:Label ID="lblMutationID" runat="server"  Visible="false" Text='<%# Eval("ArvMutationID") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="rcbEditArvType" runat="server" Text="aSomeTest" AutoPostBack="false"
                                                                        Skin="MetroTouch" CheckedItemsTexts="FitInInput">
                                                                    </telerik:RadComboBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                  <%-- <asp:Label ID="lblTesting" runat="server" Text="HELO"></asp:Label>--%>
                                                                    <telerik:RadComboBox ID="rcbFooterArvType" runat="server" Text="--Select--" AutoPostBack="true"
                                                                        Skin="MetroTouch" CheckedItemsTexts="FitInInput"  EnableLoadOnDemand="true" OnSelectedIndexChanged="rcbFooterArvType_SelectedIndexChanged" >
                                                                    </telerik:RadComboBox>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Mutation" HeaderStyle-Font-Bold="true">
                                                                 <HeaderStyle Font-Size="10px" Wrap="False" Width="200px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMutation" runat="server" Text='<%# Eval("ArvMutation") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="rcbEditMutation" runat="server" Text="--Select--" AutoPostBack="false"
                                                                        Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                                    </telerik:RadComboBox>   

                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                   <telerik:RadComboBox ID="rcbFooterMutation" runat="server" Text="--Select--" AutoPostBack="true"
                                                                        Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" OnSelectedIndexChanged="rcbFooterMutation_SelectedIndexChanged">
                                                                    </telerik:RadComboBox>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Culture/Sensitivity" HeaderStyle-Font-Bold="true">
                                                                 <HeaderStyle Font-Size="10px" Wrap="False" Width="200px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCulture" runat="server" Text='<%# Eval("ArvMutationOther") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="rcbEditCulture" runat="server" Text="--Select--" AutoPostBack="false"
                                                                        Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                                    </telerik:RadComboBox>   

                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                   <telerik:RadComboBox ID="rcbFooterCulture" runat="server" Text="--Select--" AutoPostBack="false"
                                                                        Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" >
                                                                    </telerik:RadComboBox>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Other Mutation" HeaderStyle-Font-Bold="true">
                                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="150px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOtherMutation" runat="server" Text='<%# Eval("ArvMutationOther") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                 <telerik:RadTextBox ID="txtOtherEditMutation" runat="server" Text='<%# Eval("ArvMutationOther") %>'></telerik:RadTextBox>  
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <telerik:RadTextBox ID="txtOtherFooterMutation" runat="server" Text='<%# Eval("ArvMutationOther") %>' Width="120px" Wrap="true"></telerik:RadTextBox>  
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>
                                                           
                                                            <telerik:GridTemplateColumn HeaderStyle-Font-Bold="true">
                                                            <HeaderStyle Font-Size="10px" Wrap="False" Width="90px" />
                                                                <ItemTemplate>
                                                                    <telerik:RadButton ID="btnRemove" runat="server"  Text="Remove"
                                                                        ForeColor="Blue" CommandName="Delete" ButtonType="LinkButton" ItemStyle-Font-Names="verdana" ItemStyle-Font-Size="10pt">
                                                                    </telerik:RadButton>
                                                                </ItemTemplate>
                                                              
                                                                <FooterTemplate>
                                                                    <telerik:RadButton ID="btnFooterAdd" runat="server" Skin="MetroTouch" Text="Add"
                                                                        CommandName="Insert">
                                                                    </telerik:RadButton>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>
                                                          <%--  <telerik:GridEditCommandColumn ButtonType="PushButton"  ItemStyle-Font-Names="verdana" ItemStyle-Font-Size="10pt"  >
                                                            </telerik:GridEditCommandColumn>--%>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <FooterStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                                </telerik:RadGrid>
                                                 </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                              </Columns>
                                              </MasterTableView>
                                            </telerik:RadGrid>
                                          
                                          
                                        </NestedViewTemplate>
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
                                                    Skin="MetroTouch"  CheckedItemsTexts="FitInInput"  >
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
                            <telerik:RadComboBox ID="rcbReportedBy" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                                    Skin="MetroTouch"  CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                    
                                                </telerik:RadComboBox>

                                             


                            </td>
                            <td>
                            *Reported Date: 
                            </td>
                            <td>
                            <telerik:RadDatePicker ID="RadDateReportDate" runat="server" Skin="MetroTouch" Culture="(Default)">
                                    <Calendar ID="Calendar3" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                        Skin="MetroTouch" runat="server">
                                    </Calendar>
                                    <DateInput ID="DateInput3" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                          </table>
                        
                        </fieldset>


                        </td>
                        </tr>
                        <tr>
                         <td colspan="4">
                          <table align="center">
                          <tr>
                           <td>
                          
                           </td>
                           <td>
                           <telerik:RadButton ID="btnBack" runat="server" Text="Back" OnClick="BtnBack_Click" Skin="MetroTouch" OnClientClicked="parent.ShowLoading" Visible="false"></telerik:RadButton>
                           </td>
                           <td>
                           <telerik:RadButton ID="btnPrint" runat="server" Text="Print" Skin="MetroTouch" OnClientClicked="parent.ShowLoading" Visible="false"></telerik:RadButton>
                           </td>
                          </tr>
                          </table>
                         </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hiddTestID" runat="server" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                   
                </div>
            </div>
        </div>

        <div id="frmLabOrderResult_ScriptBlock" runat="server" style="display: none;">
   <script type="text/javascript" language="javascript">
       function FormValidatedOnSubmit(Msg) {
           alert(Msg);
           return false;
       }
       function LoadingPanel(sender, args) {
           parent.ShowLoading();
       }
   </script>

</div>