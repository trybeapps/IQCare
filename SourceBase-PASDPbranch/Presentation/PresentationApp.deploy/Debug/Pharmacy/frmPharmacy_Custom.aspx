<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true"
    CodeBehind="frmPharmacy_Custom.aspx.cs" Inherits="PresentationApp.Pharmacy.frmPharmacy_Custom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
    <%--<telerik:RadScriptManager ID="rsm" runat="server" EnablePartialRendering="true">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager2" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgdrugmain2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgdrugmain" LoadingPanelID="RadAjaxLoadingPanel2">
                    </telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server">
    </telerik:RadAjaxLoadingPanel>--%>
    <asp:ScriptManager ID="mst" runat="server">
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        window.onload = CalcualteBMIGet; 
            function CalcualteBMIGet() {
                var wid = document.getElementById('ctl00_IQCareContentPlaceHolder_TXT-Weight-ord_patientpharmacyorder-303').value;
                var hid = document.getElementById('ctl00_IQCareContentPlaceHolder_TXT-Height-ord_patientpharmacyorder-304').value;
                var weight = "";
                var height = "";
                if (wid != "")
                    weight = wid;
                if (hid != "")
                    height = hid;

                if (weight == "" || height == "") {
                    weight = 0;
                    height = 0;
                }
                weight = parseFloat(weight);
                height = parseFloat(height);
                var BMI = weight / ((height / 100) * (height / 100));
                BMI = BMI.toFixed(2);
                if (BMI != "" && BMI != "NaN")
                    document.getElementById('ctl00_IQCareContentPlaceHolder_TXT--BSA-305').value = BMI;
            }
    </script>
    <asp:UpdatePanel ID="UpdateMasterLink" runat="server">
        <ContentTemplate>
            <div style="padding: 10px;">
                <br />
                <asp:Panel class="border center formbg" ID="PnlDynamicElements" Width="100%" runat="server">
                    <br />
                </asp:Panel>
                <br />
                <div class="border center formbg">
                    <table cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="form pad35" style="width: 100%;  padding-left: 30px" align="left">
                                  <asp:Panel ID="PnlPreSelectedDrug" runat="server"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="form" style="width: 100%;" align="left">
                                    <table align="left">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSelectDrug" runat="server" Font-Bold="true" Text="Select Additional Drug"></asp:Label>
                                            </td>
                                            <td  align="left">
                                                <telerik:RadAutoCompleteBox ID="Autoselectdrug" runat="server" Skin="Outlook"  
                                                    Width="700px" DropDownWidth="500" onentryadded="Autoselectdrug_EntryAdded">
                                                </telerik:RadAutoCompleteBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="form">
                                <div class="mid" style="cursor: pointer; height: 280px; overflow: auto;border: 1px solid #666699;">
                                    <telerik:RadGrid ID="rgdrugmain" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                        AllowMultiRowSelection="False" AllowPaging="false" PageSize="5" GridLines="None" 
                                        Width="100%" ShowGroupPanel="false" onitemcommand="rgdrugmain_ItemCommand"  
                                        onitemcreated="rgdrugmain_ItemCreated" 
                                        OnDeleteCommand="rgdrugmain_DeleteCommand" 
                                        onpageindexchanged="rgdrugmain_PageIndexChanged" 
                                        RetainExpandStateOnRebind="True" ShowDesignTimeSmartTagMessage="False">
                                        <HeaderStyle Font-Size="Smaller" />
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <GroupPanel Text="">
                                        </GroupPanel>
                                        <MasterTableView DataKeyNames="DrugID" AllowMultiColumnSorting="True" 
                                            GroupLoadMode="Server" RetainExpandStateOnRebind="True">
                                            <NestedViewTemplate>
                                                <asp:Panel runat="server" ID="InnerContainer" CssClass="viewWrap" Visible="false">
                                                    <telerik:RadTabStrip runat="server" ID="TabStip1" MultiPageID="Multipage1" SelectedIndex="0">
                                                        <Tabs>
                                                            <telerik:RadTab runat="server" Text="Prescription Order" PageViewID="PageView1">
                                                            </telerik:RadTab>
                                                            <telerik:RadTab runat="server" Text="Refills" PageViewID="PageView3">
                                                            </telerik:RadTab>
                                                            <telerik:RadTab runat="server" Text="Dispense" PageViewID="PageView2">
                                                            </telerik:RadTab>
                                                            <telerik:RadTab runat="server" Text="Patient Instruction" PageViewID="PageView4">
                                                            </telerik:RadTab>
                                                        </Tabs>
                                                    </telerik:RadTabStrip>
                                                    <telerik:RadMultiPage runat="server" ID="Multipage1" SelectedIndex="0" RenderSelectedPageOnly="false">
                                                        <telerik:RadPageView runat="server" ID="PageView1">
                                                            <asp:Label ID="lbldrugid" Font-Bold="true" Font-Italic="true" Text='<%# Eval("DrugID") %>' Visible="false" runat="server"></asp:Label>
                                                            <telerik:RadGrid runat="server" ID="OrdersGrid" ShowFooter="true" AllowSorting="true"
                                                                EnableLinqExpressions="false" OnNeedDataSource="OrdersGrid_NeedDataSource">
                                                                <MasterTableView ShowHeader="true" AutoGenerateColumns="False" AllowPaging="true"
                                                                    DataKeyNames="DrugID" PageSize="7" HierarchyLoadMode="ServerOnDemand">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn UniqueName="DrugID" DataField="DrugID" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn UniqueName="GenericID" DataField="GenericID" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn UniqueName="FrequencyID" DataField="Frequency" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn UniqueName="ProphylaxisID" DataField="Prophylaxis" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Dose" DataField="Dose">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" NumberFormat-DecimalDigits="2" Width="50px"
                                                                                    ID="txtDose" Text='<%# Eval("Dose") %>'>
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Frequency" DataField="Frequency">
                                                                            <ItemTemplate>
                                                                                <telerik:RadComboBox runat="server" ID="rdcmbfrequency" DataTextField="FrequencyName"
                                                                                    DataValueField="FrequencyId">
                                                                                </telerik:RadComboBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Durations  (days)" DataField="Duration">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" NumberFormat-DecimalDigits="2" Width="50px"
                                                                                    ID="txtDuration" Text='<%# Eval("Duration") %>'>
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Quantity Prescribed" DataField="QtyPrescribed">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" NumberFormat-DecimalDigits="2" Width="50px"
                                                                                    ID="txtQtyPrescribed" Text='<%# Eval("QtyPrescribed") %>'>
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Prophylaxis" DataField="Prophylaxis">
                                                                            <ItemTemplate>
                                                                                <telerik:RadButton ID="chkProphylaxis" runat="server" Width="40px" ToggleType="CustomToggle"
                                                                                    AutoPostBack="false" ButtonType="StandardButton" Skin="Outlook">
                                                                                    <ToggleStates>
                                                                                        <telerik:RadButtonToggleState Text="No" />
                                                                                        <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                                                    </ToggleStates>
                                                                                </telerik:RadButton>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadPageView>
                                                        <telerik:RadPageView runat="server" ID="PageView2" Width="460px">
                                                            <telerik:RadGrid runat="server" ID="Dispense" ShowFooter="true" AllowSorting="true"
                                                                EnableLinqExpressions="false" OnNeedDataSource="Dispense_NeedDataSource">
                                                                <ValidationSettings EnableValidation="true" CommandsToValidate="PefrormInsert" />
                                                                <MasterTableView ShowHeader="true" AutoGenerateColumns="False" AllowPaging="true"
                                                                    DataKeyNames="DrugID" PageSize="7" HierarchyLoadMode="ServerOnDemand">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn UniqueName="DrugID" DataField="DrugID" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Quantity Dispensed" DataField="QtyDispensed">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" NumberFormat-DecimalDigits="2" ID="txtQtyDispensed"
                                                                                    Width="50px" Text='<%# Eval("QtyDispensed") %>'>
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Quantity Prescribed" DataField="QtyPrescribed">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" ReadOnly="true" NumberFormat-DecimalDigits="2" Width="50px" ID="txtQtyPrescribeddispense" Text='<%# Eval("QtyPrescribed") %>'>
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Total Available Quantity" DataField="AvailQty">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" ReadOnly="true" NumberFormat-DecimalDigits="2" Width="50px" ID="txtAvailQty">
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Batch No" DataField="BatchNo">
                                                                            <ItemTemplate>
                                                                                <telerik:RadComboBox runat="server" ID="rdcmbBatch" DataTextField="BatchNo" DataValueField="id">
                                                                                </telerik:RadComboBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                         <telerik:GridTemplateColumn HeaderText="Expiry Date" DataField="ExpiryDate">
                                                                        <ItemTemplate>
                                                                        <telerik:RadTextBox ID="txtExpiryDate" ReadOnly="true" Width="120px" runat="server" DataTextField="ExpiryDate"></telerik:RadTextBox>
                                                                        </ItemTemplate>
                                                                           </telerik:GridTemplateColumn>
                                                                         <telerik:GridTemplateColumn HeaderText="Selling Price" DataField="SellPrice">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" NumberFormat-DecimalDigits="2" DataTextField="SellPrice" Width="50px" ID="txtSellPrice">
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Bill Amount" DataField="BillAmount">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" NumberFormat-DecimalDigits="2"
                                                                                    Width="50px" ID="txtBillAmount" DataTextField="BillAmount">
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadPageView>
                                                        <telerik:RadPageView runat="server" ID="PageView3" Width="460px">
                                                            <telerik:RadGrid runat="server" ID="Refill" ShowFooter="true" AllowSorting="true"
                                                                OnNeedDataSource="Refill_NeedDataSource" EnableLinqExpressions="false">
                                                                <ValidationSettings EnableValidation="true" CommandsToValidate="PefrormInsert" />
                                                                <MasterTableView ShowHeader="true" AutoGenerateColumns="False" AllowPaging="true"
                                                                    DataKeyNames="DrugID" PageSize="7" HierarchyLoadMode="ServerOnDemand">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn UniqueName="DrugID" DataField="DrugID" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn UniqueName="RefillExp" DataField="RefillExpiration" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="No. of Refills" DataField="Refill">
                                                                            <ItemTemplate>
                                                                                <telerik:RadNumericTextBox runat="server" Width="50px" ID="txtRefill" Text='<%# Eval("Refill") %>'>
                                                                                </telerik:RadNumericTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Refill Expiration Date" DataField="RefillExpiration">
                                                                            <ItemTemplate>
                                                                                <telerik:RadDatePicker ID="dtRefillExpiration" runat="server" Skin="Outlook">
                                                                                    <Calendar ID="Calendar19" runat="server" Skin="Outlook" ShowRowHeaders="false" UseColumnHeadersAsSelectors="False"
                                                                                        UseRowHeadersAsSelectors="False">
                                                                                    </Calendar>
                                                                                    <DateInput ID="DateInputEditOther" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
                                                                                        LabelWidth="0px">
                                                                                        <EmptyMessageStyle Resize="None" />
                                                                                        <ReadOnlyStyle Resize="None" />
                                                                                        <FocusedStyle  Resize="None" />
                                                                                        <DisabledStyle Resize="None" />
                                                                                        <InvalidStyle  Resize="None" />
                                                                                        <HoveredStyle  Resize="None" />
                                                                                        <EnabledStyle  Resize="None" />
                                                                                    </DateInput>
                                                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                                </telerik:RadDatePicker>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadPageView>
                                                        <telerik:RadPageView runat="server" ID="PageView4" Width="460px">
                                                            <telerik:RadGrid runat="server" ID="PatientInstruction" ShowFooter="true" AllowSorting="true"  
                                                                  OnNeedDataSource="PatientInstruction_NeedDataSource" EnableLinqExpressions="false" >
                                                                <ValidationSettings EnableValidation="true" CommandsToValidate="PefrormInsert" />
                                                                <MasterTableView ShowHeader="true" AutoGenerateColumns="False" AllowPaging="true"
                                                                    DataKeyNames="DrugID" PageSize="7" HierarchyLoadMode="ServerOnDemand">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn UniqueName="DrugID" DataField="DrugID" Visible="false">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Patient Instruction" DataField="Instructions">
                                                                            <ItemTemplate>
                                                                             <telerik:RadTextBox ID="txtPatientInstruction" TextMode="MultiLine" Width="100%" runat="server" Text='<%# Eval("Instructions") %>'></telerik:RadTextBox>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </telerik:RadPageView>
                                                    </telerik:RadMultiPage>
                                                </asp:Panel>
                                            </NestedViewTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="DrugID" DataField="DrugID" Visible="false" >
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="DrugName" HeaderText="Drug Name" HeaderStyle-Height= "10px"  HeaderButtonType="TextButton"
                                                    DataField="DrugName"  ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle Height="10px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="DispensingUnit" HeaderText="Dispensing Unit" HeaderStyle-Height= "10px"
                                                    HeaderButtonType="TextButton" DataField="DispensingUnit"  ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle Height="10px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="DrugType" HeaderText="Drug Type" HeaderButtonType="TextButton" HeaderStyle-Height= "10px"
                                                    DataField="DrugType">
                                                    <HeaderStyle Height="10px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn Text="Remove" CommandName="Delete" />
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings AllowDragToGroup="true">
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br />
                <div class="border center formbg">
                    <br />
                    <h2 class="forms" align="left">
                        Approval and Signatures</h2>
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="form">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" style="width: 50%">
                                                <label class="required">
                                                    *Prescribed by:</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPharmOrderedbyName" runat="Server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="form">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" style="width: 50%">
                                                <label class="required" for="pharmOrderedbyDate">
                                                    *Prescribed By Date:</label>
                                            </td>
                                            <td>
                                                <input id="txtpharmOrderedbyDate" maxlength="11" size="11" name="pharmOrderedbyDate"
                                                    runat="server" />
                                                <img id="appDateimg1" onclick="w_displayDatePicker('<%=txtpharmOrderedbyDate.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                    border="0" name="appDateimg" />
                                                <span class="smallerlabel" id="appDatespan1">(DD-MMM-YYYY)</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trDispense" runat="server">
                                <td class="form">
                                    <asp:Panel runat="server" ID="pnlDispBy">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="right" style="width: 50%">
                                                    <label id="lbldispensedby" class="Required" runat="server">
                                                        Dispensed by:</label>
                                                </td>
                                                <td style="width: 50%">
                                                    <asp:DropDownList ID="ddlPharmDispensedbyName" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td class="form">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" style="width: 50%">
                                                <label id="lbldispensedbydate" class="Required" runat="server" for="pharmdispensedbyDate">
                                                    Dispensed by Date:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <input id="txtpharmdispensedbydate" maxlength="11" size="11" name="pharmdispensedbydate"
                                                    runat="server" />
                                                <img id="appDateimg2" onclick="w_displayDatePicker('<%=txtpharmdispensedbydate.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                                    border="0" name="appDateimg" /><span class="smallerlabel" id="appDatespan2">(DD-MMM-YYYY)</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="pad5 center" colspan="2">
                                    <asp:Button ID="btnsave" runat="server" Text="Save" onclick="btnsave_Click" />
                                    <asp:Button ID="btncancel" runat="server" Text="Close" 
                                        onclick="btncancel_Click" />
                                    <asp:Button ID="btnPrint" Text="PrintLabel" runat="server" 
                                        onclick="btnPrint_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </ContentTemplate>
         <Triggers>
                <asp:PostBackTrigger ControlID="btnsave"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncancel"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnPrint"></asp:PostBackTrigger>
            </Triggers>
    </asp:UpdatePanel>
</asp:Content>
