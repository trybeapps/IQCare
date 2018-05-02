<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.Custom_Forms.frmRegistrationTouch"
    CodeBehind="frmRegistrationTouch.ascx.cs" %>
<div id="FormContent">
    <telerik:RadSplitter ID="rwPrint" runat="server" BorderSize="0" BorderStyle="None"
        Width="861px" Height="520px">
        <telerik:RadPane ID="rdPane" runat="server">
            <asp:UpdatePanel ID="updtFormUpdate" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <asp:HiddenField ID="hidtab" Value="0" runat="server" />
                    <div id="tabs" style="width: 800px">
                        <ul>
                            <li><a href="#tab1">Patient Info</a></li>
                            <li><a href="#tab2">Caregiver Info</a></li>
                            <li><a href="#tab4">Mother's History</a>&nbsp; </li>
                            <li><a href="#tab5">HIV Care</a></li>
                            <li><a href="#tab6">Transfer In</a></li>
                            <li><a href="#tab8">Drug Allergies</a></li>
                        </ul>
                        <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <asp:UpdatePanel ID="uptTab1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="PatientIdentifiers" cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Patient Identification</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                First name *
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtFirstName" runat="server" BackColor="#FFFFCC" Skin="MetroTouch"
                                                    Width="200px">
                                                    <ClientEvents OnBlur="OnBlur" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 15%;">
                                                Last name *
                                            </td>
                                            <td style="width: 35%; text-align: right;">
                                                <telerik:RadTextBox ID="txtLastName" runat="server" BackColor="#FFFFCC" Skin="MetroTouch"
                                                    Width="200px">
                                                    <ClientEvents OnBlur="OnBlur" />
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Birth Date *
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtPatientDOB" runat="server" Skin="MetroTouch" AutoPostBack="true"
                                                    MinDate="01/01/1800" OnSelectedDateChanged="dtPatientDOB_SelectedDateChanged">
                                                    <ClientEvents OnDateSelected="OnBlurDateP" />
                                                    <Calendar ID="Calendar1" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                        UseRowHeadersAsSelectors="False">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" BackColor="#FFFFCC"
                                                        DisplayDateFormat="dd-MMM-yyyy" LabelWidth="0px">
                                                        <ClientEvents OnBlur="OnBlur" />
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
                                            </td>
                                            <td>
                                                Age
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="font-size: 12px">
                                                            Years
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="AgeYearZZZ" runat="server" Skin="MetroTouch" Width="30px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td style="font-size: 12px">
                                                            Months
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="AgeMonthZZZ" runat="server" Skin="MetroTouch" Width="30px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton ID="btnCalCDOB" runat="server" Skin="MetroTouch" Text="Cal DOB"
                                                                OnClick="btnCalCDOB_Click">
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Sex *
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cbSex" runat="server" Skin="MetroTouch" BackColor="#FFFFCC">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                Registration<br />
                                                Date *
                                            </td>
                                            <td style="text-align: right;">
                                                <telerik:RadDatePicker ID="dtRegistrationDate" runat="server" Skin="MetroTouch">
                                                    <ClientEvents OnDateSelected="OnBlurDateP" />
                                                    <Calendar ID="Calendar2" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                        UseRowHeadersAsSelectors="False">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput2" runat="server" DateFormat="dd/MM/yyyy" BackColor="#FFFFCC"
                                                        DisplayDateFormat="dd-MMM-yyyy" LabelWidth="0px">
                                                        <ClientEvents OnBlur="OnBlur" />
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
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Patient Physical Address</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Street - house number
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtAddress" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 20%;">
                                                Suburb / Village / Farm
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtSuburb" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                District
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadComboBox ID="rcbDistrict" runat="server" Skin="MetroTouch" Width="400px">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Sub district
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadComboBox ID="rcbSubDistrict" runat="server" Skin="MetroTouch" Width="400px">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Phone number
                                            </td>
                                            <td>
                                                <telerik:RadMaskedTextBox ID="txtPatientPhoneNo" runat="server" Width="200px" Mask="(###) ###-####"
                                                    Skin="MetroTouch">
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td>
                                                Address comment
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtAddressComment" runat="server" Width="200px" Skin="MetroTouch">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Patient Postal Address</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%; vertical-align: text-top">
                                                Postal Address
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtPatientPostalAddress" TextMode="MultiLine" Height="75px"
                                                    runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 20%; vertical-align: text-top">
                                                Postal code
                                            </td>
                                            <td style="width: 30%; vertical-align: text-top">
                                                <telerik:RadTextBox ID="txtPatientPostalCode" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Patient Source</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Entry Point
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadComboBox ID="cbEntryPoint" runat="server" AutoPostBack="false" OnClientSelectedIndexChanged="ShowMoreSingle"
                                                    Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td style="width: 50%;">
                                                <div id="ShowIfEntryOther" style="display: none">
                                                    <table cellpadding="0px">
                                                        <tr>
                                                            <td style="width: 50%;">
                                                                Other entry point
                                                            </td>
                                                            <td style="width: 50%;">
                                                                <telerik:RadTextBox ID="txtEntryPointOther" runat="server" Skin="MetroTouch">
                                                                </telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab2" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Caregiver Contact</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                <span>Caregiver first name</span>
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtCGFirstName" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 15%;">
                                                Caregiver last name
                                            </td>
                                            <td style="width: 35%; text-align: right">
                                                <telerik:RadTextBox ID="txtCGLastName" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Caregiver DOB
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtCGDOB" runat="server" Skin="MetroTouch" MinDate="01/01/1800"
                                                    AutoPostBack="true" OnSelectedDateChanged="dtCGDOB_SelectedDateChanged">
                                                    <Calendar ID="Calendar3" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                        UseRowHeadersAsSelectors="False">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput3" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd-MMM-yyyy"
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
                                            </td>
                                            <td>
                                                Age
                                            </td>
                                            <td style="padding-left: 23px;">
                                                <table>
                                                    <tr>
                                                        <td style="font-size: 12px">
                                                            Year
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="txtCGYearZZZ" runat="server" Skin="MetroTouch" Width="30px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td style="font-size: 12px;">
                                                            Month
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="txtCGMonthZZZ" runat="server" Skin="MetroTouch" Width="30px">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton ID="CGCalDOB" runat="server" Skin="MetroTouch" Text="Cal DOB"
                                                                OnClick="CGCalDOB_Click">
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Caregiver sex
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cbCGSex" runat="server" Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                Caregiver relationship
                                            </td>
                                            <td style="text-align: right">
                                                <telerik:RadComboBox ID="cbCGRelationship" runat="server" OnClientSelectedIndexChanged="ShowMoreSingle"
                                                    Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Caregiver phone no:
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadMaskedTextBox ID="txtCGPhoneNo" runat="server" Width="200px" Mask="(###) ###-####"
                                                    Skin="MetroTouch">
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td style="width: 50%">
                                                <div id="ShowIfOtherRelationship" style="display: none">
                                                    <table cellpadding="0px">
                                                        <tr>
                                                            <td style="width: 50%;">
                                                                Other relationship
                                                            </td>
                                                            <td style="width: 50%;">
                                                                <telerik:RadTextBox ID="txtOtherRelationship" runat="server" Skin="MetroTouch" Width="200px">
                                                                </telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="tab4" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <table id="referrals" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Mother&#39;s ART/PMTCT History</div>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 23%;">
                                        Name of Mother
                                    </td>
                                    <td style="width: 27%;">
                                        <telerik:RadTextBox ID="txtMotherName" runat="server" Skin="MetroTouch" Width="200px">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        Alive?
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="rbtnMotherAliveYes" runat="server" ToggleType="Radio" AutoPostBack="false"
                                            Skin="MetroTouch" GroupName="MotherAlive" Text="Yes">
                                        </telerik:RadButton>
                                        &nbsp;
                                        <telerik:RadButton ID="rbtnMotherAliveNo" runat="server" ToggleType="Radio" AutoPostBack="false"
                                            Skin="MetroTouch" GroupName="MotherAlive" Text="No">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 60%">
                                        Mother received drugs for PMTCT?
                                    </td>
                                    <td style="width: 40%">
                                        <telerik:RadComboBox ID="cbMotherReceivedPMTCT" runat="server" OnClientSelectedIndexChanged="ShowMoreSingle"
                                            Skin="MetroTouch">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="3" />
                                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="1|ShowIfMotherRecievedYes|show" />
                                                <telerik:RadComboBoxItem runat="server" Text="No" Value="0|ShowIfMotherRecievedYes|hide" />
                                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="2|ShowIfMotherRecievedYes|hide" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="ShowIfMotherRecievedYes" style="display: none;">
                                <table cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 60%">
                                            Select drug combination
                                        </td>
                                        <td style="width: 40%">
                                            <telerik:RadComboBox ID="cbMotherPMTCTDrugs" runat="server" CheckBoxes="true" CheckedItemsTexts="FitInInput"
                                                Width="300px" AutoPostBack="false">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 60%">
                                        Has the child received drugs for PMTCT?
                                    </td>
                                    <td style="width: 40%">
                                        <telerik:RadComboBox ID="cbChildReceivedPMTCT" runat="server" EnableViewState="true"
                                            OnClientSelectedIndexChanged="ShowMoreSingle" AutoPostBack="false" Skin="MetroTouch">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="3" />
                                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="1|ShowIfChildReceivedYes|show" />
                                                <telerik:RadComboBoxItem runat="server" Text="No" Value="0|ShowIfChildReceivedYes|hide" />
                                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="2|ShowIfChildReceivedYes|hide" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="ShowIfChildReceivedYes" style="display: none">
                                <table cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 60%;">
                                            Select drug combination
                                        </td>
                                        <td style="width: 40%;">
                                            <telerik:RadComboBox ID="cbChildPMTCTDrugs" runat="server" CheckBoxes="true" CheckedItemsTexts="FitInInput"
                                                Width="300px" AutoPostBack="false">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 60%;">
                                        Is mother on ART?
                                    </td>
                                    <td style="width: 40%;">
                                        <telerik:RadComboBox ID="cbMotherOnART" runat="server" Skin="MetroTouch">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="3" />
                                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="1" />
                                                <telerik:RadComboBoxItem runat="server" Text="No" Value="0" />
                                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="2" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Feeding option up to point of failure or 6 months - which ever is earlier
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cbFeedingOption" runat="server" Skin="MetroTouch" Width="300px">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tab5" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div style="float: left">
                                            HIV Care</div>
                                        <div style="float: right; cursor: pointer; font-size: 16px !important; text-decoration: underline !important;
                                            margin-top: 5px;" onclick="$('#helpDocs').slideToggle('slow');">
                                            Help Guide
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 23%;">
                                        Date confirmed HIV positive
                                    </td>
                                    <td style="width: 28%;">
                                        <telerik:RadDatePicker ID="dtConfirmedPos" runat="server" Skin="MetroTouch">
                                            <Calendar ID="Calendar4" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                Skin="MetroTouch" runat="server">
                                                <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                            </Calendar>
                                            <DateInput ID="DateInput4" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                                    <td style="width: 23%;">
                                        Date enrolled in HIV Care
                                    </td>
                                    <td style="width: 28%;">
                                        <telerik:RadDatePicker ID="dtEnrolledHIVCare" runat="server" Skin="MetroTouch">
                                            <Calendar ID="Calendar5" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                Skin="MetroTouch" runat="server">
                                                <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                            </Calendar>
                                            <DateInput ID="DateInput5" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                                        WHO Stage at enrollment
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cbWHOEnrollment" runat="server" Skin="MetroTouch">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="bubble" id="helpDocs" style="display: none; position: relative; width: 200px;
                                left: 500px; z-index: 99; top: -130px; color: Green;">
                                <a href="PDF/WHO Staging 2007.pdf" target="_blank" onclick="$('#helpDocs').slideToggle('slow');">
                                    WHO Clinical Staging Guide</a>
                            </div>
                        </div>
                        <div id="tab6" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Transfer In</div>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtWeight" EventName="TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtHeight" EventName="TextChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Transfer In Date
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadDatePicker ID="dtTransferIn" runat="server" Skin="MetroTouch">
                                                    <Calendar ID="Calendar6" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        Skin="MetroTouch" runat="server">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput6" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                                            <td style="width: 20%;">
                                                From Sub District
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadComboBox ID="rcbSubDistrictTransferIn" runat="server" Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Facility
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbFacilityIn" runat="server" Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                Date Started on 1st Line Regimen
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtDateRegimenStarted" runat="server" Skin="MetroTouch">
                                                    <Calendar ID="Calendar7" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        Skin="MetroTouch" runat="server">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput7" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                                                Regimen
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cbTransferInRegimen" runat="server" CheckBoxes="true" CheckedItemsTexts="FitInInput"
                                                    Width="400px" AutoPostBack="false" OnItemDataBound="cbTransferInRegimen_ItemDataBound">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Weight (Kgs)
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtWeight" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    ClientEvents-OnValueChanged="CalcBMI" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                Height (cms)
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtHeight" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    ClientEvents-OnValueChanged="CalcBMI" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                BMI
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtBMICalculated" runat="server" Skin="MetroTouch" Width="200px"
                                                    Enabled=" false">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                WHO Stage
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cbWHOStage" runat="server" Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Prior ART (excluding PMTCT)</div>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 20%;">
                                        Prior ART
                                    </td>
                                    <td style="width: 80%;">
                                        <telerik:RadComboBox ID="cbPriorART" runat="server" Skin="MetroTouch" OnClientSelectedIndexChanged="ShowMoreSingle"
                                            AutoPostBack="false">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="3|ShowIfPriorARTYes" />
                                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="1|ShowIfPriorARTYes|Yes|PriorGrid" />
                                                <telerik:RadComboBoxItem runat="server" Text="No" Value="0|ShowIfPriorARTYes" />
                                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="4|ShowIfPriorARTYes" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                            <div id="wrapper_cbPriorART">
                                <div id="ShowIfPriorARTYes" style="display: none">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="10px" class="Section">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        Regimen
                                                    </td>
                                                    <td style="width: 30%;">
                                                        <telerik:RadComboBox ID="cbPriorARTRegimen" runat="server" Skin="MetroTouch" Width="200px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td style="width: 20%;">
                                                        Date Last Used
                                                    </td>
                                                    <td style="width: 30%;">
                                                        <telerik:RadDatePicker ID="dtPriorDate" runat="server" Skin="MetroTouch">
                                                            <ClientEvents OnDateSelected="OnBlurDateP" />
                                                            <Calendar ID="Calendar9" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                                Skin="MetroTouch" runat="server">
                                                                <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                            </Calendar>
                                                            <DateInput ID="DateInput9" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
                                                                LabelWidth="0px" runat="server">
                                                                <ClientEvents OnBlur="OnBlur" />
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
                                                        <telerik:RadButton ID="btnPriorArt" runat="server" Text="Add to list" OnClientClicking="CheckPriorARTVals"
                                                            OnClick="btnPriorArt_Click">
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellpadding="10px" class="Section">
                                                <tr>
                                                    <td style="width: 100%">
                                                        <div id="PriorARTDiv">
                                                            <telerik:RadGrid ID="rgPriorART" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                                GridLines="None" OnItemCommand="rgPriorART_ItemCommand">
                                                                <MasterTableView DataKeyNames="RegimenID">
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="RegimenID" Display="false" UniqueName="RegimenID"
                                                                            DataType="System.String" HeaderText="RegimenID">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Regimen" FilterControlAltText="Filter Regimen column"
                                                                            UniqueName="Regimen" DataType="System.String" HeaderText="Regimen">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="DateLastUsed" FilterControlAltText="Filter Date Last Used column"
                                                                            UniqueName="DateLastUsed" DataType="System.String" DataFormatString="{0:dd-MMM-yyyy}"
                                                                            HeaderText="Date last used">
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" FilterControlAltText="Filter DeleteColumn column"
                                                                            Text="Remove" UniqueName="DeleteColumn" ConfirmDialogType="RadWindow" Resizable="false"
                                                                            ConfirmText="Delete record?">
                                                                        </telerik:GridButtonColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                            <a href="#" id="PriorGrid"></a>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div id="tab8" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <asp:UpdatePanel ID="updtTab8" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Drug Allergies</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Allergen
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadComboBox ID="cbAllergen" runat="server" Width="200px">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td style="width: 20%;">
                                                Type of reaction
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtTOR" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Date
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtReactionDate" runat="server" Skin="MetroTouch">
                                                    <Calendar ID="Calendar8" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        Skin="MetroTouch" runat="server">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput8" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                                            <td>
                                                Relevant medical conditions
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtRMC" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadButton ID="btnAddAllergen" runat="server" Text="Add to list" OnClientClicking="ValidateAllergies"
                                                    OnClick="btnAddAllergen_Click">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 100%">
                                                <div id="divAllergies">
                                                    <telerik:RadGrid ID="rgAllergies" runat="server" AutoGenerateColumns="False" OnItemCommand="rgAllergies_OnItemCommand"
                                                        CellSpacing="0" GridLines="None">
                                                        <MasterTableView DataKeyNames="AllergenID">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="AllergenID" Display="false" UniqueName="AllergenID"
                                                                    DataType="System.String" HeaderText="AllergenID">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="AllergenName" FilterControlAltText="Filter AllergenName column"
                                                                    UniqueName="AllergenName" DataType="System.String" HeaderText="Allergen">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="TOR" FilterControlAltText="Filter TOR column"
                                                                    UniqueName="TOR" DataType="System.String" HeaderText="Type of Reaction">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="ADate" FilterControlAltText="Filter ADate column"
                                                                    UniqueName="ADate" DataType="System.String" DataFormatString="{0:dd-MMM-yyyy}"
                                                                    HeaderText="Date">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="RMC" FilterControlAltText="Filter RMC column"
                                                                    UniqueName="RMC" DataType="System.String" HeaderText="Relevant Medical Conditions">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" FilterControlAltText="Filter DeleteColumn column"
                                                                    Text="Remove" UniqueName="DeleteColumn" ConfirmDialogType="RadWindow" Resizable="false"
                                                                    ConfirmText="Delete record?">
                                                                </telerik:GridButtonColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <!-- END HISTORY TABS  -->
                    </div>
                    <div style="float: left; position: relative; margin: 5px 5px 5px 0px;">
                        <telerik:RadButton ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_OnClick">
                        </telerik:RadButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </telerik:RadPane>
    </telerik:RadSplitter>
</div>
<div id="frmReg_ScriptBlock" runat="server" style="display: none;">
    <script type="text/javascript">
        function CloseMinLoading(theDivName) {
            currentLoadingPanel = $find(parent.GetLPanelID());
            var theInterval = setInterval(function () { currentLoadingPanel.hide(theDivName); clearInterval(theInterval); }, 1000);

        }
        function ShowMinLoading(theDivName) {
            currentLoadingPanel = $find(parent.GetLPanelID());
            currentLoadingPanel.show(theDivName);
        }

        function CalcBMI(sender, args) {
            SetBMI();
        }

        function SetBMI() {

            var txtWeight = $find("IDfrmRegistrationTouch_txtWeight").get_value();
            var txtHeight = $find("IDfrmRegistrationTouch_txtHeight").get_value();
            if ((txtWeight != "") && (txtHeight != "")) {

                var BMI = txtWeight / ((txtHeight / 100) * (txtHeight / 100));
                var thePos = BMI.toString().indexOf(".");
                var theVal = '';
                if (thePos > 0)
                    theVal = BMI.toString().substring(0, thePos + 2);
                else
                    theVal = BMI.ToString();

                var txtBMI = $find("IDfrmRegistrationTouch_txtBMICalculated");
                txtBMI.set_value(theVal);
            }

        }


        function CheckVals(sender, args) {
            var ReqVals = new Array();
            ReqVals[0] = "IDfrmRegistrationTouch_txtFirstName|First Name";
            ReqVals[1] = "IDfrmRegistrationTouch_txtLastName|Last Name";
            ReqVals[2] = "IDfrmRegistrationTouch_dtPatientDOB_dateInput|Date of Birth";
            ReqVals[3] = "IDfrmRegistrationTouch_cbSex_Input|Patient Sex";
            ReqVals[4] = "IDfrmRegistrationTouch_dtRegistrationDate_dateInput|Date Registered";

            var theNames = new Array();
            var ReqIsFilled = true;
            for (index = 0; index < ReqVals.length; ++index) {

                var arr = ReqVals[index].split("|");

                var theFirstControl = null;
                if (index != 3) {
                    if ($('#' + arr[0]).val() == "") {
                        theNames[index] = arr[1];

                        if (theFirstControl == null) theFirstControl = arr[0];
                        ReqIsFilled = false;
                    }
                } else {
                    if ($('#' + arr[0]).val() == "Select") {
                        theNames[index] = arr[1];

                        if (theFirstControl == null) theFirstControl = arr[0];
                        ReqIsFilled = false;
                    }
                }
            }

            if (ReqIsFilled) {
                var theAnswer = window.confirm("Are you sure you want to save this form?");
                if (theAnswer) {
                    parent.ShowLoading();
                    args.set_cancel(false);
                }
                else {
                    args.set_cancel(true);
                }
            }
            else {
                gotToTabVal(theFirstControl, theNames, args);
            }
        }


        function CheckAllergenVals(sender, args) {
            var ReqVals = new Array();
            ReqVals[0] = "IDfrmRegistrationTouch_cbAllergen_Input|Allergen Name";
            ReqVals[1] = "IDfrmRegistrationTouch_txtTOR|Type of Reaction";
            ReqVals[2] = "IDfrmRegistrationTouch_dtReactionDate_dateInput|Date";
            ReqVals[3] = "IDfrmRegistrationTouch_txtRMC|Relevant Medical Conditions";

            var theNames = new Array();
            var ReqIsFilled = true;
            for (index = 0; index < ReqVals.length; ++index) {

                var arr = ReqVals[index].split("|");

                var theFirstControl = null;
                if (index != 0) {
                    if ($('#' + arr[0]).val() == "") {
                        theNames[index] = arr[1];

                        if (theFirstControl == null) theFirstControl = arr[0];
                        ReqIsFilled = false;
                    }
                } else {
                    if ($('#' + arr[0]).val() == "Select") {
                        theNames[index] = arr[1];

                        if (theFirstControl == null) theFirstControl = arr[0];
                        ReqIsFilled = false;
                    }
                }
            }

            if (ReqIsFilled) {
                var theAnswer = window.confirm("Are you sure you want to save this form?");
                if (theAnswer) {
                    parent.ShowLoading();
                    args.set_cancel(false);
                }
                else {
                    args.set_cancel(true);
                }
            }
            else {
                gotToTabVal(theFirstControl, theNames, args);
            }
        }
        function CheckPriorARTVals(sender, args) {
            var ReqVals = new Array();
            ReqVals[0] = "IDfrmRegistrationTouch_cbPriorARTRegimen_Input|Regimen";
            ReqVals[1] = "IDfrmRegistrationTouch_dtPriorDate_dateInput|Date Last Used";

            var theNames = new Array();
            var ReqIsFilled = true;
            for (index = 0; index < ReqVals.length; ++index) {

                var arr = ReqVals[index].split("|");

                var theFirstControl = null;
                if (index != 0) {
                    if ($('#' + arr[0]).val() == "") {
                        theNames[index] = arr[1];

                        if (theFirstControl == null) theFirstControl = arr[0];
                        ReqIsFilled = false;
                    }
                } else {
                    if ($('#' + arr[0]).val() == "Select") {
                        theNames[index] = arr[1];

                        if (theFirstControl == null) theFirstControl = arr[0];
                        ReqIsFilled = false;
                    }
                }
            }

            if (ReqIsFilled) {
                ShowMinLoading('tab6');
            }
            else {
                args.set_cancel(true);
                gotToTabValPriorART(theFirstControl, theNames);
            }
        }
        function gotToTabVal(thecontrol, thenames, args) {
            $("#tabs").tabs("option", "active", 0);
            var theMessage = "You have not given a value for the required field(s): ";
            for (i = 0; i < thenames.length; ++i) {
                if (typeof thenames[i] != "undefined")
                    theMessage = theMessage + "\n" + thenames[i];
            }

            alert(theMessage);
            $('#' + thecontrol).focus();
            args.set_cancel(true);
        }
        function gotToTabValPriorART(thecontrol, thenames) {
            $("#tabs").tabs("option", "active", 4);
            var theMessage = "You have not given a value for the required field(s) before adding a Prior ART: ";
            for (i = 0; i < thenames.length; ++i) {
                if (typeof thenames[i] != "undefined")
                    theMessage = theMessage + "\n" + thenames[i];
            }

            alert(theMessage);
            $('#' + thecontrol).focus();

        }

        function ValidateAllergies(sender, args) {
            var cblAllergen = $('#IDfrmRegistrationTouch_cbAllergen_Input').val();
            var txtTOR = $("IDfrmRegistrationTouch_txtTOR").val();
            var txtReaction = $("IDfrmRegistrationTouch_dtReactionDate_dateInput").val();
            var txtRMC = $("IDfrmRegistrationTouch_txtRMC").val();
            if ((txtTOR == "") || (txtReaction == "") || (txtRMC == "") ||  (cblAllergen == "Select")) {
                alert("You have not given a value for the required field(s) before adding drug allergies");
                return false;
            }
        }
      
    </script>
</div>
<div style="visibility: collapse">
    <telerik:RadButton ID="btnSave" runat='server' OnClientClicking="CheckVals" Text="Test"
        OnClick="btnSave_Click">
    </telerik:RadButton>
</div>
<telerik:RadWindow runat="server" ID="rwMPMTCT" Title="Mother PMTCT Drug List" Modal="true"
    Skin="BlackMetroTouch" Width="880px" VisibleOnPageLoad="false" Height="450px"
    Behaviors="Move,Close">
    <ContentTemplate>
        <telerik:RadGrid ID="rgMotherPMTCT" runat="server" AllowMultiRowSelection="true"
            AutoGenerateColumns="True" CellSpacing="0" GridLines="None" Skin="BlackMetroTouch">
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </ContentTemplate>
</telerik:RadWindow>
