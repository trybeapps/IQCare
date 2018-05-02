<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmKNHPharmacyTouch.ascx.cs"
    Inherits="Touch.Custom_Forms.frmKNHPharmacyTouch" %>
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rgdrugmain1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgdrugmain" LoadingPanelID="RadAjaxLoadingPanel1">
                </telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
</telerik:RadAjaxLoadingPanel>
<div style="visibility: collapse">
    <telerik:RadButton ID="btnSave" runat="server" OnClick="RadButton1_Click" Visible="true"
        Text="Save">
    </telerik:RadButton>
</div>
<div id="FormContent">
    <asp:HiddenField ID="hdfSelectedDrugs" runat="server" />
    <div id="tabs" style="width: 800px">
        <ul>
            <li><a href="#tab1">Pharmacy Order</a></li>
        </ul>
        <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
            height: 380px;">
            <asp:UpdatePanel ID="updtPharmdrugs" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table id="sometable" cellpadding="10px" class="Section">
                        <tr>
                            <td>
                                <div>
                                    <asp:Label ID="lblmessage" ForeColor="Red" runat="server"></asp:Label></div>
                            </td>
                        </tr>
                    </table>
                    <table id="Table2" cellpadding="10px" class="Section">
                        <tr>
                            <td class="SectionheaderTxt">
                                <div>
                                    Patient Dosing Information</div>
                            </td>
                        </tr>
                    </table>
                    <table id="Table3" cellpadding="10px" class="Section">
                        <tr>
                            <td>
                                <span class="">Age :</span> &nbsp; Year
                            </td>
                            <td>
                                <telerik:RadTextBox ID="ZZZCurrentAgeYear" runat="server" Skin="MetroTouch" Width="60px"
                                    Enabled="False">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                Month
                            </td>
                            <td>
                                <telerik:RadTextBox ID="ZZZCurrentAgeMonth" runat="server" Skin="MetroTouch" Width="60px"
                                    Enabled="False">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                DOB &nbsp;
                            </td>
                            <td>
                                <telerik:RadTextBox ID="ZZZPatDOB" runat="server" Enabled="False" Skin="MetroTouch"
                                    Width="105px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <table id="Table4" cellpadding="10px" class="Section">
                        <tr>
                            <td>
                                Weight &nbsp;(Kg)
                            </td>
                            <td>
                                <telerik:RadNumericTextBox runat="server" Skin="MetroTouch" NumberFormat-DecimalDigits="2"
                                    Width="60px" ID="ZZZPharmWeight">
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                Height &nbsp;(cm)
                            </td>
                            <td>
                                <telerik:RadNumericTextBox runat="server" Skin="MetroTouch" NumberFormat-DecimalDigits="2"
                                    Width="60px" ID="ZZZPharmHeight">
                                </telerik:RadNumericTextBox>
                                &nbsp;
                            </td>
                            <td>
                                BSA &nbsp;(m<sup>2</sup>)
                            </td>
                            <td class="auto-style1">
                                <telerik:RadTextBox ID="ZZZPharmBSA" runat="server" Enabled="False" Skin="MetroTouch"
                                    Width="60px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <table id="Table5" cellpadding="10px" class="Section">
                        <tr>
                            <td class="SectionheaderTxt">
                                <div>
                                    ARV Related Information</div>
                            </td>
                        </tr>
                    </table>
                    <table id="Table1" cellpadding="10px" class="Section">
                        <tr>
                            <td style="width: 20%;">
                                *Treatment Program
                            </td>
                            <td style="width: 30%; padding-top: 18px">
                                <telerik:RadComboBox ID="rcbtreatment" runat="server" Skin="MetroTouch">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 20%">
                                Period Taken
                            </td>
                            <td style="width: 30%">
                                <telerik:RadComboBox ID="rcbperiod" runat="server" Skin="MetroTouch">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%;">
                                *Drug Provider
                            </td>
                            <td style="width: 30%; padding-top: 18px">
                                <telerik:RadComboBox ID="rcbdrugprovider" runat="server" Skin="MetroTouch">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 20%;">
                                Regimen line
                                
                            </td>
                            <td style="width: 30%; padding-top: 18px">
                                <telerik:RadComboBox ID="ddregimenline" runat="server" Skin="MetroTouch">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                Next Appointment Date
                            </td>
                            <td style="width: 30%">
                                <telerik:RadDatePicker ID="appdate" runat="server" Skin="MetroTouch">
                                    <Calendar ID="Calendar2" UseRowHeadersAsSelectors="False" ShowRowHeaders="false"
                                        UseColumnHeadersAsSelectors="False" Skin="MetroTouch" runat="server">
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
                            <td style="width: 20%">
                                Reason
                            </td>
                            <td style="width: 30%">
                                <telerik:RadComboBox ID="rcbreason" runat="server" Skin="MetroTouch">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                    <table id="Table6" cellpadding="10px" class="Section">
                        <tr>
                            <td class="SectionheaderTxt">
                                <div>
                                    Prescription Notes</div>
                            </td>
                        </tr>
                    </table>
                    <table id="Table7" cellpadding="10px" class="Section">
                        <tr>
                            <td>
                                <telerik:RadTextBox ID="PharmNotes" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                    Width="700px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="10px" class="Section">
                        <tr>
                            <td class="SectionheaderTxt">
                                <div>
                                    <a id="sGrid" href="#sGrid"></a>Order Drugs</div>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="10px" class="Section">
                        <tr>
                            <td style="width: 15%;">
                                Pre-Selected Drugs:
                            </td>
                            <td style="width: 70%;">
                                <telerik:RadComboBox ID="rcbPreSelectedDrugs" runat="server" Text="aSomeTest" AutoPostBack="false"
                                    CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                    Width="500px">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 15%; padding-bottom: 21px">
                                <telerik:RadButton ID="btnAddDrug" OnClick="btnsubmit_Click" Text="Add" runat="server"
                                    SkinID="MetroTouch">
                                </telerik:RadButton>
                            </td>
                            <td style="display: none">
                                <div>
                                    <telerik:RadButton ID="rdbpaediatric" runat="server" Text="Paediatric Pre-Selected Drugs"
                                        OnClick="rdbpaediatric_Click">
                                    </telerik:RadButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Select drug:
                            </td>
                            <td colspan="2">
                                <telerik:RadAutoCompleteBox ID="Autoselectdrug" runat="server" Skin="MetroTouch"
                                    Width="500px" DropDownWidth="500" OnEntryAdded="Autoselectdrug_EntryAdded">
                                </telerik:RadAutoCompleteBox>
                            </td>
                        </tr>
                    </table>
                    <table class="Section" cellpadding="10px">
                        <tr>
                            <td style="width: 700px">
                                <a id="druggrid" href="#sGrid"></a>
                                <telerik:RadGrid ID="rgdrugmain" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                    AllowMultiRowSelection="False" AllowPaging="True" PageSize="5" GridLines="None"
                                    Width="690px" ShowGroupPanel="false" OnItemCommand="rgdrugmain_ItemCommand" OnItemCreated="rgdrugmain_ItemCreated"
                                    OnDeleteCommand="rgdrugmain_DeleteCommand">
                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                    <GroupPanel Text="">
                                    </GroupPanel>
                                    <MasterTableView DataKeyNames="DrugID" AllowMultiColumnSorting="True" GroupLoadMode="Server">
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
                                                    </Tabs>
                                                </telerik:RadTabStrip>
                                                <telerik:RadMultiPage runat="server" ID="Multipage1" SelectedIndex="0" RenderSelectedPageOnly="false">
                                                    <telerik:RadPageView runat="server" ID="PageView1">
                                                        <asp:Label ID="lbldrugid" Font-Bold="true" Font-Italic="true" Text='<%# Eval("DrugID") %>'
                                                            Visible="false" runat="server"></asp:Label>
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
                                                                            <telerik:RadComboBox runat="server" ID="rdcmbfrequency" OnSelectedIndexChanged="rdcmbfrequency_SelectedIndexChanged"
                                                                                DataTextField="FrequencyName" DataValueField="FrequencyId">
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
                                                                                AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
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
                                                                            <telerik:RadNumericTextBox runat="server" ReadOnly="true" NumberFormat-DecimalDigits="2"
                                                                                Width="50px" ID="txtQtyPrescribeddispense" Text='<%# Eval("QtyPrescribed") %>'>
                                                                            </telerik:RadNumericTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </telerik:RadPageView>
                                                    <telerik:RadPageView runat="server" ID="PageView3" Width="460px">
                                                        <telerik:RadGrid runat="server" ID="Refill" ShowFooter="true" AllowSorting="true"
                                                            EnableLinqExpressions="false" OnNeedDataSource="Refill_NeedDataSource">
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
                                                                            <telerik:RadDatePicker ID="dtRefillExpiration" runat="server" Skin="MetroTouch">
                                                                                <Calendar ID="Calendar19" runat="server" Skin="MetroTouch" ShowRowHeaders="false"
                                                                                    UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                                </Calendar>
                                                                                <DateInput ID="DateInputEditOther" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
                                                                                    LabelWidth="0px">
                                                                                    <EmptyMessageStyle Resize="None" />
                                                                                    <ReadOnlyStyle Resize="None" />
                                                                                    <FocusedStyle Resize="None" />
                                                                                    <DisabledStyle Resize="None" />
                                                                                    <InvalidStyle Resize="None" />
                                                                                    <HoveredStyle Resize="None" />
                                                                                    <EnabledStyle Resize="None" />
                                                                                </DateInput>
                                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                            </telerik:RadDatePicker>
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
                                            <telerik:GridBoundColumn UniqueName="DrugID" DataField="DrugID" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="DrugName" HeaderText="Drug Name" HeaderButtonType="TextButton"
                                                DataField="DrugName">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="DispensingUnit" HeaderText="Dispensing Unit"
                                                HeaderButtonType="TextButton" DataField="DispensingUnit">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn SortExpression="DrugType" HeaderText="Drug Type" HeaderButtonType="TextButton"
                                                DataField="DrugType">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn Text="Remove" CommandName="Delete" />
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings AllowDragToGroup="true">
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                    <table class="Section" cellpadding="10px">
                        <tr>
                            <td class="SectionheaderTxt">
                                <div>
                                    Signature</div>
                            </td>
                        </tr>
                    </table>
                    <table id="Table8" cellpadding="10px" class="Section">
                        <tr>
                            <td style="width: 22%">
                                *Prescribed by
                                <br />
                            </td>
                            <td style="width: 28%; padding-top: 18px">
                                <telerik:RadComboBox ID="rcbprescribed" runat="server" Skin="MetroTouch">
                                </telerik:RadComboBox>
                            </td>
                            <td style="width: 22%">
                                *Prescription Date
                            </td>
                            <td style="width: 28%">
                                <telerik:RadDatePicker ID="prescribedbydate" runat="server" Skin="MetroTouch">
                                    <Calendar ID="Calendar3" UseRowHeadersAsSelectors="False" ShowRowHeaders="false"
                                        UseColumnHeadersAsSelectors="False" Skin="MetroTouch" runat="server">
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
                        <tr>
                            <td>
                                *Dispensed by
                            </td>
                            <td style="padding-top: 18px">
                                <telerik:RadComboBox ID="rcbdispensed" runat="server" Skin="MetroTouch">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                *Dispensed Date
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dispensedbydate" runat="server" Skin="MetroTouch">
                                    <Calendar ID="Calendar1" UseRowHeadersAsSelectors="False" ShowRowHeaders="false"
                                        UseColumnHeadersAsSelectors="False" Skin="MetroTouch" runat="server">
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
                                    <DatePopupButton></DatePopupButton>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                    </table>
                    <table id="Table9" cellpadding="10px" class="Section">
                        <tr>
                            <td class="SectionheaderTxt">
                                <div>
                                    &nbsp</div>
                            </td>
                        </tr>
                    </table>
                    <table id="Table10" cellpadding="10px" class="Section">
                        <tr>
                            <td style="width: 100%" align="center">
                                <telerik:RadButton ID="PrintPrescription" runat="server" Skin="MetroTouch" OnClick="btnrefill_Click" Text="Print Prescription">
                                </telerik:RadButton>
                                &nbsp;&nbsp;
                                <telerik:RadButton ID="btnrefill" runat="server" OnClick="btnrefill_Click" Text="Refill At This Facility ">
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<div id="frmKNHPharmacy_ScriptBlock" runat="server" style="display: none;">
    <script type="text/javascript">
        function fnShowMessage(hmsg) {
            alert(hmsg);
            return false;
        }
        function fnConfirm() {
            var theAnswer = window.confirm("Are you sure you want to save this form?");
            if (theAnswer) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</div>
