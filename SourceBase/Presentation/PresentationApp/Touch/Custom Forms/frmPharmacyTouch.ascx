<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmPharmacyTouch.ascx.cs"
    Inherits="Touch.Custom_Forms.frmPharmacyTouch" %>
<telerik:radajaxmanager id="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rgdrugmain1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgdrugmain" LoadingPanelID="RadAjaxLoadingPanel1">
                </telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:radajaxmanager>
<telerik:radajaxloadingpanel id="RadAjaxLoadingPanel1" runat="server">
</telerik:radajaxloadingpanel>
<div style="visibility: collapse">
    <telerik:radbutton id="btnSave" runat="server" onclientclicking="loadsavepanel" onclick="RadButton1_Click"
        visible="true" text="Save">
    </telerik:radbutton>
        <telerik:radbutton id="btnSaveContinue" runat="server" onclientclicking="loadsavepanelcontinue" onclick="RadButtonContinue_Click"
        visible="true" text="Save">
    </telerik:radbutton>
</div>
<div id="FormContent">
    <asp:HiddenField ID="hdfSelectedDrugs" runat="server" />
    <asp:HiddenField ID="txthiddenfieldARV" runat="server" Value="Save"/>
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
                                <span class="Emphasis">Age :</span> &nbsp; Year
                            </td>
                            <td>
                                <telerik:radtextbox id="ZZZCurrentAgeYear" runat="server" skin="MetroTouch" width="60px"
                                    enabled="False">
                                </telerik:radtextbox>
                            </td>
                            <td>
                                Month
                            </td>
                            <td>
                                <telerik:radtextbox id="ZZZCurrentAgeMonth" runat="server" skin="MetroTouch" width="60px"
                                    enabled="False">
                                </telerik:radtextbox>
                            </td>
                            <td>
                                DOB &nbsp;
                            </td>
                            <td>
                                <telerik:radtextbox id="ZZZPatDOB" runat="server" enabled="False" skin="MetroTouch"
                                    width="105px">
                                </telerik:radtextbox>
                            </td>
                        </tr>
                    </table>
                    <table id="Table4" cellpadding="10px" class="Section">
                        <tr>
                            <td>
                                Weight &nbsp;(Kg)
                            </td>
                            <td>
                                <telerik:radtextbox runat="server" enabled="False" skin="MetroTouch" cssclass="hex"
                                    width="60px" id="ZZZPharmWeight">
                                </telerik:radtextbox>
                            </td>
                            <td>
                                Height &nbsp;(cm)
                            </td>
                            <td>
                                <telerik:radtextbox runat="server" enabled="False" skin="MetroTouch" cssclass="hex"
                                    width="60px" id="ZZZPharmHeight">
                                </telerik:radtextbox>
                                &nbsp;
                            </td>
                            <td>
                                BSA &nbsp;(m<sup>2</sup>)
                            </td>
                            <td class="auto-style1">
                                <telerik:radtextbox id="ZZZPharmBSA" runat="server" enabled="False" skin="MetroTouch"
                                    width="60px">
                                </telerik:radtextbox>
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
                                Regimen line
                                <br />
                                <br />
                            </td>
                            <td style="width: 30%; padding-top: 18px">
                                <telerik:radcombobox id="ddregimenline" runat="server" skin="MetroTouch">
                                </telerik:radcombobox>
                            </td>
                            <td style="width: 20%">
                                Pharmacy Refill Date
                            </td>
                            <td style="width: 30%">
                                <telerik:raddatepicker id="appdate" runat="server" skin="MetroTouch" culture="(Default)">
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
                                </telerik:raddatepicker>
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
                                <telerik:radtextbox id="PharmNotes" runat="server" skin="MetroTouch" textmode="MultiLine"
                                    width="700px">
                                </telerik:radtextbox>
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
                                <telerik:radcombobox id="rcbPreSelectedDrugs" runat="server" text="aSomeTest" autopostback="false"
                                    checkboxes="true" enablecheckallitemscheckbox="true" checkeditemstexts="FitInInput"
                                    width="500px">
                                </telerik:radcombobox>
                            </td>
                            <td style="width: 15%; padding-bottom: 21px">
                                <telerik:radbutton id="btnAddDrug" onclick="btnsubmit_Click" text="Add" runat="server"
                                    skinid="MetroTouch">
                                </telerik:radbutton>
                            </td>
                            <td style="display: none">
                                <div>
                                    <telerik:radbutton id="rdbpaediatric" runat="server" text="Paediatric Pre-Selected Drugs"
                                        onclick="rdbpaediatric_Click">
                                    </telerik:radbutton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Select drug:
                            </td>
                            <td colspan="2">
                                <telerik:radautocompletebox id="Autoselectdrug" runat="server" skin="MetroTouch"
                                    ondatasourceselect="Autoselectdrug_DataSourceSelect" filter="Contains" width="500px"
                                    dropdownwidth="500" onentryadded="Autoselectdrug_EntryAdded" datasourceid="objDSDrugs">
                                </telerik:radautocompletebox>
                                <asp:ObjectDataSource ID="objDSDrugs" runat="server" TypeName="Touch.Custom_Forms.frmPharmacyTouch"
                                    SelectMethod="GetAutoDrugs">
                                    <SelectParameters>
                                        <asp:Parameter Name="filter" Type="String" DefaultValue="" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                    </table>
                    <table class="Section" cellpadding="10px">
                        <tr>
                            <td style="width: 700px">
                                <a id="druggrid" href="#sGrid"></a>
                                <telerik:radgrid id="rgdrugmain" runat="server" autogeneratecolumns="False" allowsorting="True"
                                    allowmultirowselection="False" allowpaging="True" pagesize="5" gridlines="None"
                                    width="690px" showgrouppanel="false" onitemcommand="rgdrugmain_ItemCommand" onitemcreated="rgdrugmain_ItemCreated"
                                    onitemdatabound="rgdrugmain_ItemDataBound" ondeletecommand="rgdrugmain_DeleteCommand"
                                    onprerender="rgdrugmain_PreRender">
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
                                                                            <telerik:RadTextBox runat="server"   CssClass="hex" Width="50px"
                                                                                ID="txtDose" Text='<%# Eval("Dose") %>'>
                                                                            </telerik:RadTextBox>
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
                                                                            <telerik:RadTextBox runat="server"   CssClass="hex" Width="50px"
                                                                                ID="txtDuration" Text='<%# Eval("Duration") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Quantity Prescribed" DataField="QtyPrescribed">
                                                                        <ItemTemplate>
                                                                            <telerik:RadTextBox runat="server"   CssClass="hex" Width="50px"
                                                                                ID="txtQtyPrescribed" Text='<%# Eval("QtyPrescribed") %>'>
                                                                            </telerik:RadTextBox>
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
                                                                    <telerik:GridTemplateColumn HeaderText="Quantity Prescribed" DataField="QtyPrescribed">
                                                                        <ItemTemplate>
                                                                            <telerik:RadTextBox runat="server" ReadOnly="true" CssClass="hex"
                                                                                Width="50px" ID="txtQtyPrescribeddispense" Text='<%# Eval("QtyPrescribed") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Quantity Dispensed" DataField="QtyDispensed">
                                                                        <ItemTemplate>
                                                                            <telerik:RadTextBox runat="server"  CssClass="hex" ID="txtQtyDispensed"
                                                                                Width="50px" Text='<%# Eval("QtyDispensed") %>'>
                                                                            </telerik:RadTextBox>
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
                                                                            <telerik:RadTextBox runat="server" Width="50px" ID="txtRefill" Text='<%# Eval("Refill") %>'>
                                                                            </telerik:RadTextBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Refill Expiration Date" DataField="RefillExpiration">
                                                                        <ItemTemplate>
                                                                            <telerik:RadDatePicker ID="dtRefillExpiration" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                </telerik:radgrid>
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
                                <telerik:radcombobox id="rcbprescribed" runat="server" skin="MetroTouch">
                                </telerik:radcombobox>
                            </td>
                            <td style="width: 22%">
                                *Prescription Date
                            </td>
                            <td style="width: 28%">
                                <telerik:raddatepicker id="prescribedbydate" runat="server" skin="MetroTouch" culture="(Default)">
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
                                </telerik:raddatepicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                *Dispensed by
                            </td>
                            <td style="padding-top: 18px">
                                <telerik:radcombobox id="rcbdispensed" runat="server" skin="MetroTouch">
                                </telerik:radcombobox>
                            </td>
                            <td>
                                *Dispensed Date
                            </td>
                            <td>
                                <telerik:raddatepicker id="dispensedbydate" runat="server" skin="MetroTouch" culture="(Default)">
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
                                </telerik:raddatepicker>
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
                                <telerik:radbutton id="PrintPrescription" runat="server" skin="MetroTouch" onclick="PrintPrescription_Click"
                                    text="Print Prescription">
                                </telerik:radbutton>
                                &nbsp;&nbsp;
                                <telerik:radbutton id="btnrefill" runat="server" onclick="btnrefill_Click" text="Refill At This Facility ">
                                </telerik:radbutton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<div id="frmPharmacy_ScriptBlock" runat="server" style="display: none;">
    <script type="text/javascript">
        function fnShowMessage(hmsg) {
            alert(hmsg);
            return false;
        }
        function loadsavepanel(sender, args) {
            parent.ShowLoading();
        }
        function loadsavepanelcontinue(sender, args) {
            args.set_cancel(!window.confirm("Confirm ARV dose, frequency and durations are correct?"));
        }
        function fnConfirm() {
            var theAnswer = window.confirm("Are you sure you want to save this form?");
            if (theAnswer) {
                parent.ShowLoading();
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</div>
