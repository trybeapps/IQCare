<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.Custom_Forms.frmVisitTouch"
    CodeBehind="frmVisitTouch.ascx.cs" %>
<style type="text/css"></style>
<div id="FormContent">
    <telerik:radsplitter id="rwPrint" runat="server" bordersize="0" borderstyle="None"
        width="861px" height="520px">
        <telerik:RadPane ID="rdPane" runat="server">
            <asp:UpdatePanel ID="updtFormUpdate" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div id="tabs" style="width: 800px">
                        <ul>
                            <li><a href="#tab1">Clinical Status</a></li>
                            <li><a href="#tab3">Clinical History/Findings</a></li>
                            <li><a href="#tab4">Pharmacy & Lab</a></li>
                            <li><a href="#tab5">Visit Finalisation</a></li>
                        </ul>
                        <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <asp:UpdatePanel ID="uptTab1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="PatientIdentifiers" cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Visit Details</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table1" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Visit date *
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadDatePicker ID="dtVisitDate" runat="server" Skin="MetroTouch">
                                                    <ClientEvents OnDateSelected="OnBlurDateP" />
                                                    <Calendar ID="Calendar1" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        Skin="MetroTouch" runat="server">
                                                        <ClientEvents OnLoad="RadDatePicker_SetMaxDateToCurrentDate" />
                                                    </Calendar>
                                                    <DateInput ID="DateInput1" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
                                                        LabelWidth="0px" runat="server" BackColor="#FFFFCC">
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
                                            <td style="width: 15%;">
                                                Scheduled
                                            </td>
                                            <td style="width: 35%;">
                                                <telerik:RadButton ID="btnScheduledYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" GroupName="Scheduled" Text="Yes">
                                                </telerik:RadButton>
                                                &nbsp;
                                                <telerik:RadButton ID="btnScheduledNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" GroupName="Scheduled" Text="No">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Visit type
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cbVisitType" runat="server" Width="200px" Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                Present
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbPresent" runat="server" Width="250px" Skin="MetroTouch">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table2" cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Caregiver Details</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table3" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%">
                                                Caregiver name
                                            </td>
                                            <td style="width: 30%">
                                                <telerik:RadTextBox ID="txtCaregiver" runat="server" Skin="MetroTouch" Width="200px">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 20%">
                                                Caregiver contact number
                                            </td>
                                            <td style="width: 30%">
                                                <telerik:RadMaskedTextBox ID="txtCareGiverContactNo" runat="server" Width="200px"
                                                    Mask="(###) ###-####" Skin="MetroTouch">
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table4" cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Hospital Admission</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table5" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 40%">
                                                Admitted to hospital from last visit?
                                            </td>
                                            <td style="width: 60%">
                                                <telerik:RadButton ID="chkHospYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="hideHospitalYN|show"
                                                    GroupName="admithosp" Text="Yes">
                                                </telerik:RadButton>
                                                &nbsp;
                                                <telerik:RadButton ID="chkHospNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="hideHospitalYN|hide"
                                                    GroupName="admithosp" Text="No">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="hideHospitalYN" style="display: none">
                                        <table id="Table6" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%">
                                                    Number of days hospitalised
                                                </td>
                                                <td style="width: 30%">
                                                    <telerik:RadTextBox ID="txtNumDayHosp" runat="server" CssClass="hex" Skin="MetroTouch">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td style="width: 10%;">
                                                    Where?
                                                </td>
                                                <td style="width: 40%;">
                                                    <telerik:RadComboBox ID="rcbWhereHosp" runat="server" Width="300px" Skin="MetroTouch">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 20%;">
                                                    Discharge diagnosis
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadTextBox ID="txtDischargeDiagnosis" runat="server" Skin="MetroTouch">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 20%;">
                                                    Discharge note
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadTextBox ID="txtDischargeNote" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                                        Width="600px">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table id="Table7" cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Clinical Status</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="Table8" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Duration (months) since: Start ART
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtDurationStartART" runat="server" Skin="MetroTouch" Enabled="False"
                                                    Text="">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 20%;">
                                                Duration (months) since: Current Regimen
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadTextBox ID="txtDurationCurrentReg" runat="server" Skin="MetroTouch" Enabled="False"
                                                    Text="">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="Emphasis">Temperature (C)</span>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtTemp" runat="server" CssClass="hex" NumberFormat-DecimalDigits="2">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <span class="Emphasis">Weight (kg)</span>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtWeight" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CalcBMI">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="Emphasis">Height (cm)</span>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtHeight" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    NumberFormat-DecimalDigits="2" ClientEvents-OnValueChanged="CalcBMI">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                BMI
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtBMI" runat="server" Skin="MetroTouch" Enabled="False">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="Emphasis">Respiratory rate (bpm)</span>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtRespRate" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    NumberFormat-DecimalDigits="2">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <span class="Emphasis">Pulse (bpm)</span>
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtPulse" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    NumberFormat-DecimalDigits="2">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                BP - Systolic
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtSystolic" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    NumberFormat-DecimalDigits="2">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                BP - Diastolic
                                            </td>
                                            <td>
                                                <telerik:RadNumericTextBox ID="txtDiastolic" runat="server" Skin="MetroTouch" CssClass="hex"
                                                    NumberFormat-DecimalDigits="2">
                                                </telerik:RadNumericTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="HCMUAC">
                                        <table id="Table22" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Head circumference (cm)
                                                </td>
                                                <td style="width: 30%;">
                                                    <telerik:RadNumericTextBox ID="txtHeadCirc" runat="server" Skin="MetroTouch" CssClass="hex"
                                                        NumberFormat-DecimalDigits="0">
                                                    </telerik:RadNumericTextBox>
                                                </td>
                                                <td style="width: 20%;">
                                                    MUAC (cm)
                                                </td>
                                                <td style="width: 30%;">
                                                    <telerik:RadNumericTextBox ID="txtMUAC" runat="server" CssClass="hex" Type="Number"
                                                        NumberFormat-DecimalDigits="0">
                                                    </telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table id="Table9" cellpadding="10px" class="Section">
                                        <tr>
                                            <td class="SectionheaderTxt">
                                                <div>
                                                    Developmental Status</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- ShowIf < 6yrs style="display:none" (unhidden for UI UAT) -->
                                    <div id="DevScreen" style="display: none">
                                        <table id="Table10" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Developmental screening
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadComboBox ID="cbDevScreen" runat="server" Skin="MetroTouch">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <script type="text/javascript">  </script>
                                    <!-- Show if >= 10 yrs  style="display:none" (unhidden for UI UAT) -->
                                    <div id="Tanner" style="display: none">
                                        <table id="Table11" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Tanner stage
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadComboBox ID="cbTannerStage" runat="server" Skin="MetroTouch" OnClientSelectedIndexChanged="SetSexActiveYN">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <script type="text/javascript"> </script>
                                    <!-- Show if tanner >= 3 (unhidden for UI UAT) -->
                                    <div id="HideSexActiveYN" style="display: none">
                                        <table id="Table12" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Sexually active
                                                </td>
                                                <td style="width: 30%;">
                                                    <telerik:RadButton ID="btnSexActiveYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                        ToggleType="Radio" OnClientCheckedChanged="ShowSexActiveYN" CommandArgument="ShowIfSexuallyActive|show"
                                                        GroupName="SexuallyActive" Text="Yes">
                                                    </telerik:RadButton>
                                                    &nbsp;
                                                    <telerik:RadButton ID="btnSexActiveNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                        ToggleType="Radio" OnClientCheckedChanged="ShowSexActiveYN" CommandArgument="ShowIfSexuallyActive|hide|1"
                                                        GroupName="SexuallyActive" Text="No">
                                                    </telerik:RadButton>
                                                </td>
                                                <td style="width: 50%;">
                                                    <div class="ShowIfSexuallyActiveFemale" style="display: none;">
                                                        <table id="Table13">
                                                            <tr>
                                                                <td style="width: 50%;">
                                                                    Pregnant?
                                                                </td>
                                                                <td style="width: 50%;">
                                                                    <telerik:RadButton ID="btnPregnantYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                                        ToggleType="Radio" GroupName="Pregnant" Text="Yes" OnClientCheckedChanged="ShowSexActiveYN"
                                                                        CommandArgument="ShowIfSexuallyActive|hide">
                                                                    </telerik:RadButton>
                                                                    &nbsp;
                                                                    <telerik:RadButton ID="btnPregnantNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                                        ToggleType="Radio" GroupName="Pregnant" Text="No" OnClientCheckedChanged="ShowSexActiveYN"
                                                                        CommandArgument="ShowIfSexuallyActive|show">
                                                                    </telerik:RadButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <!-- Show if female and active -->
                                    <div class="ShowIfSexuallyActive" style="display: none;">
                                        <table id="Table14" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%">
                                                    Protected sex?
                                                </td>
                                                <td style="width: 30%;">
                                                    <telerik:RadButton ID="btncondomsYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                        ToggleType="Radio" GroupName="Condoms" Text="Yes" OnClientCheckedChanged="ShowFamilyPlanning">
                                                    </telerik:RadButton>
                                                    &nbsp;
                                                    <telerik:RadButton ID="btncondomsNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                        ToggleType="Radio" GroupName="Condoms" Text="No" OnClientCheckedChanged="ShowFamilyPlanning">
                                                    </telerik:RadButton>
                                                    
                                                </td>
                                                <td style="width: 50%">
                                                    <div class="ShowProtectedSexYN" style="display: none">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 80%;">
                                                                    Family planning methods used?
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <telerik:RadComboBox ID="cbOtherFamilyPlanning" 
                                                                        runat="server" Skin="MetroTouch" OnClientSelectedIndexChanged="ShowOtherFPlanning">
                                                                    </telerik:RadComboBox>
                                                                    <!--CheckBoxes="true" CheckedItemsTexts="FitInInput"-->
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="ShowIfPlanningOther" style="display: none">
                                            <table id="Table15" cellpadding="10px" class="Section">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        Other method
                                                    </td>
                                                    <td style="width: 80%;">
                                                        <telerik:RadTextBox ID="txtFamilyPlanningOther" runat="server" Width="400px" Skin="MetroTouch">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <a id="absBottom"></a>
                        </div>
                        <!-- TAB 3 -->
                        <div id="tab3" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <table id="Table16" cellpadding="10px" class="Section">
                                <tr>
                                    <td colspan="4" class="SectionheaderTxt">
                                        <div style="float: left">
                                            Clinical Findings</div>
                                        <div style="float: right; cursor: pointer; font-size: 16px !important; text-decoration: underline !important;
                                            margin-top: 5px;" onclick="$('#helpDocs').slideToggle('slow');">
                                            Help Guide
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table18" cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 20%;">
                                        Clinical stage
                                    </td>
                                    <td style="width: 80%;">
                                        <telerik:RadComboBox ID="cbClinicalStage" runat="server" Skin="MetroTouch">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; vertical-align: text-top;">
                                        Clinical notes
                                    </td>
                                    <td colspan="3" style="width: 80%;">
                                        <telerik:RadTextBox ID="txtClinicalNotes" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                            Width="600px" Height="75px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="updtFindings" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table20" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Physical findings
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadButton ID="btnFindings" Skin="MetroTouch" AutoPostBack="false" CommandArgument="IDfrmVisitTouch_rwFindings"
                                                    OnClientClicked="OpenModalASPX" Text="Select" runat="server">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="divFindings" style="display: none">
                                        <table id="Table17" cellpadding="10px" class="Section">
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadGrid ID="rgFindings" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                        OnItemCommand="rgFindings_OnItemCommand" GridLines="None">
                                                        <MasterTableView DataKeyNames="ID">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                                                                    UniqueName="ID" DataType="System.String" Display="false" />
                                                                <telerik:GridBoundColumn DataField="PName" FilterControlAltText="Filter Name column"
                                                                    HeaderText="Physical Findings" UniqueName="PName" DataType="System.String" />
                                                                <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" FilterControlAltText="Filter DeleteColumn column"
                                                                    Text="Remove" UniqueName="DeleteColumn" ConfirmDialogType="RadWindow" Resizable="false"
                                                                    HeaderText="Actions" ConfirmText="Delete record?">
                                                                </telerik:GridButtonColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divFindingsother" style="display: none">
                                        <table id="Table19" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%">
                                                    Other Findings
                                                </td>
                                                <td style="width: 80%">
                                                    <telerik:RadTextBox ID="txtOtherFindings" runat="server" Width="400px">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div id="AdverseEvent">
                                <table cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            One or more adverse events?
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadButton ID="btnAEYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HideAEventYN|show"
                                                GroupName="NewAEvent" Text="Yes">
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="btnAENo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HideAEventYN|hide"
                                                GroupName="NewAEvent" Text="No">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!-- Show if contact = yes -->
                            <div id="HideAEventYN" style="display: none">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Adverse event(s)
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadButton ID="btnAdverseName" Skin="MetroTouch" AutoPostBack="false" CommandArgument="IDfrmVisitTouch_rwAdverseEvent"
                                                OnClientClicked="OpenModalASPX" Text="Select" runat="server">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">
                                            Comment on adverse events
                                        </td>
                                        <td colspan="3" style="width: 80%;">
                                            <telerik:RadTextBox ID="txtAdverseEventComment" runat="server" Skin="MetroTouch"
                                                TextMode="MultiLine" Width="600px">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="updtAE" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="divAE" style="display: none">
                                            <table id="Table23" cellpadding="10px" class="Section">
                                                <tr>
                                                    <td colspan="2">
                                                        <telerik:RadGrid ID="rgAE" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                            OnItemCommand="rgAE_OnItemCommand" GridLines="None" OnItemCreated="rgAE_OnItemCreated">
                                                            <MasterTableView DataKeyNames="EventCatID">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="EventCatID" FilterControlAltText="Filter ID column"
                                                                        HeaderText="EventCatID" UniqueName="EventCatID" DataType="System.String" Display="false" />
                                                                    <telerik:GridBoundColumn DataField="EventCatName" FilterControlAltText="Filter Event Category Name column"
                                                                        HeaderText="Event Category" UniqueName="EventCatName" DataType="System.String" />
                                                                    <telerik:GridBoundColumn DataField="EventID" FilterControlAltText="Filter ID column"
                                                                        HeaderText="EventID" UniqueName="EventID" DataType="System.String" Display="false" />
                                                                    <telerik:GridBoundColumn DataField="EventName" FilterControlAltText="Filter Event Name column"
                                                                        HeaderText="Event" UniqueName="EventName" DataType="System.String" />
                                                                    <telerik:GridTemplateColumn HeaderText="Severity" UniqueName="Severity" DataField="SeverityID" DataType="System.String">
                                                                        <ItemTemplate>
                                                                            <telerik:RadComboBox ID="rcbAESeverity" runat="server" OnSelectedIndexChanged="rcbAESeverity_OnSelectedIndexChanged" AutoPostBack="true">
                                                                            </telerik:RadComboBox>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="HideAEOtherdiv" style="display: none">
                                            <table id="Table24" cellpadding="10px" class="Section">
                                                <tr>
                                                    <td style="width: 20%">
                                                        Other Adverse Event
                                                    </td>
                                                    <td style="width: 80%">
                                                        <telerik:RadTextBox ID="txtAdverseEventOther" runat="server" Width="400px">
                                                        </telerik:RadTextBox>
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <table cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            TB Contact</div>
                                    </td>
                                </tr>
                            </table>
                            <div id="TBContact">
                                <table cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Is there any new TB contact?
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadButton ID="btnTBContactYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HideTBSContactSensitiveYN|show"
                                                GroupName="NewTBContact" Text="Yes">
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="btnTBContactNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HideTBSContactSensitiveYN|hide"
                                                GroupName="NewTBContact" Text="No">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!-- Show if contact = yes -->
                            <div id="HideTBSContactSensitiveYN" style="display: none">
                                <table cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Sensitivity of the TB known?
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadButton ID="btnKnownSensitivityYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="IndContactSensitivity|show"
                                                GroupName="KnownSensitivity" Text="Yes">
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="btnKnownSensitivityNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="IndContactSensitivity|hide"
                                                GroupName="KnownSensitivity" Text="No">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                                <!-- Show if Contact Sensitivity = yes -->
                                <div id="IndContactSensitivity" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Indicate sensitivity
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadButton ID="btnFirstModal" Skin="MetroTouch" AutoPostBack="false" CommandArgument="IDfrmVisitTouch_rwTBSensitivity"
                                                    OnClientClicked="OpenModalASPX" Text="Select" runat="server">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:UpdatePanel ID="updtTBStatus" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="divTBStatus" style="display: none">
                                            <table id="Table21" cellpadding="10px" class="Section">
                                                <tr>
                                                    <td colspan="2">
                                                        <telerik:RadGrid ID="rgTBStatus" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                            OnItemCommand="rgTBStatus_OnItemCommand" GridLines="None" AllowPaging="false">
                                                            <MasterTableView DataKeyNames="ID">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                                                                        UniqueName="ID" DataType="System.String" Display="false" />
                                                                    <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Drug column"
                                                                        HeaderText="TB Drugs" UniqueName="Name" DataType="System.String" />
                                                                    <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Drug column"
                                                                        HeaderText="Sensitive/Resitant" UniqueName="Type" DataType="System.String" />
                                                                    <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" FilterControlAltText="Filter DeleteColumn column"
                                                                        Text="Remove" UniqueName="DeleteColumn" ConfirmDialogType="RadWindow" Resizable="false"
                                                                        HeaderText="Actions" ConfirmText="Delete record?">
                                                                    </telerik:GridButtonColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <a id="absTBStatus"></a>
                                <div>
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Contact receiving treatment?
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadButton ID="btnConRecTreatmentYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="ShowIfContactRecTreatment|show"
                                                    GroupName="ContactRecTreat" Text="Yes">
                                                </telerik:RadButton>
                                                &nbsp;
                                                <telerik:RadButton ID="btnConRecTreatmentNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="ShowIfContactRecTreatment|hide"
                                                    GroupName="ContactRecTreat" Text="No">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="ShowIfContactRecTreatment" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Indicate treatment
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadComboBox ID="rcbTreatment" runat="server" CheckBoxes="true" Width="400"
                                                    CheckedItemsTexts="FitInInput">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="ShowIfContactSensitivityNo">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Is contact receiving daily injections?
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadButton ID="btnInjectionsYes" OnClientCheckedChanged="ShowMore" CommandArgument="ShowIfInjectionsYes|show"
                                                    runat="server" Skin="MetroTouch" AutoPostBack="false" ToggleType="Radio" GroupName="Injection"
                                                    Text="Yes">
                                                </telerik:RadButton>
                                                &nbsp;
                                                <telerik:RadButton ID="btnInjectionsNo" OnClientCheckedChanged="ShowMore" CommandArgument="ShowIfInjectionsYes|hide"
                                                    runat="server" Skin="MetroTouch" AutoPostBack="false" ToggleType="Radio" GroupName="Injection"
                                                    Text="No">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="ShowIfInjectionsYes" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Likely treatment receiving?
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadComboBox ID="cbContactTreatement" runat="server" Skin="MetroTouch" Width="400"
                                                    OnClientSelectedIndexChanged="ShowMoreSingle" AutoPostBack="false">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="wrapper_cbContactTreatement">
                                    <div id="ShowIfTreatmentOther" style="display: none">
                                        <table style="width: 100%;" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Other TB Prophylaxis
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadTextBox ID="txtTBContactOtherProph" runat="server" Skin="MetroTouch"
                                                        Width="400">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Patient TB Status</div>
                                    </td>
                                </tr>
                            </table>
                            <div id="TBStatus">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            TB Status
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="cbPtnTBStatus" runat="server" Skin="MetroTouch" Width="400px" 
                                                OnClientSelectedIndexChanged="ShowTBStatusList" AutoPostBack="false">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="wrapper_cbPtnTBStatus">
                                <div class="ShowIfTBRxQ1" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                TB Rx start date
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadDatePicker ID="dtPtnRxStartDate" runat="server" Skin="MetroTouch">
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
                                            <td style="width: 20%;">
                                                Still on treatment?
                                            </td>
                                            <td style="width: 30%;">
                                                <telerik:RadButton ID="rbtnOnTreatmentYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" GroupName="PtnOnTreatment" Text="Yes" OnClientCheckedChanged="ShowStillOnTreatment">
                                                </telerik:RadButton>
                                                &nbsp;
                                                <telerik:RadButton ID="rbtnOnTreatmentNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" GroupName="PtnOnTreatment" Text="No" OnClientCheckedChanged="ShowStillOnTreatment">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="ShowIfOnTreatmentYes" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                TB Rx end date
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadDatePicker ID="dtPtnRxEndDate" runat="server" Skin="MetroTouch">
                                                    <Calendar ID="Calendar34" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                        Skin="MetroTouch" runat="server">
                                                    </Calendar>
                                                    <DateInput ID="DateInput34" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                                </div>
                                <div class="ShowIfTBRxQ1" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                How was the diagnosis made?
                                            </td>
                                            <td style="width: 80%;">
                                             <telerik:RadComboBox ID="cbTBPtnDiagMade" runat="server" AutoPostBack="false"
                                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"></telerik:RadComboBox>
                                               
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                New sensitivity information?
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadButton ID="btnPtnNewSensitivityYes" runat="server" OnClientCheckedChanged="ShowMore"
                                                    CommandArgument="indicateSenseYN|show" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" GroupName="PtnNewSensitivity" Text="Yes">
                                                </telerik:RadButton>
                                                &nbsp;
                                                <telerik:RadButton ID="btnPtnNewSensitivityNo" runat="server" OnClientCheckedChanged="ShowMore"
                                                    CommandArgument="indicateSenseYN|hide" Skin="MetroTouch" AutoPostBack="false"
                                                    ToggleType="Radio" GroupName="PtnNewSensitivity" Text="No">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="indicateSenseYN" style="display: none">
                                        <table style="width: 100%" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Indicate sensitivity
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadButton ID="btnNewSensitivity" Skin="MetroTouch" AutoPostBack="false" CommandArgument="IDfrmVisitTouch_rwNewSensitivity"
                                                    OnClientClicked="OpenModalASPX" Text="Select" runat="server">
                                                </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:UpdatePanel ID="uptSensNew" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="divSensNew" style="display: none">
                                                    <table id="Table25" cellpadding="10px" class="Section">
                                                        <tr>
                                                            <td colspan="2">
                                                                <telerik:RadGrid ID="rgNewSens" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                                                    OnItemCommand="rgNewSens_OnItemCommand" GridLines="None" AllowPaging="false">
                                                                    <MasterTableView DataKeyNames="ID">
                                                                        <Columns>
                                                                            <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                                                                                UniqueName="ID" DataType="System.String" Display="false" />
                                                                            <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Drug column"
                                                                                HeaderText="TB Drugs" UniqueName="Name" DataType="System.String" />
                                                                            <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Drug column"
                                                                                HeaderText="Sensitive/Resitant" UniqueName="Type" DataType="System.String" />
                                                                            <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="Delete" FilterControlAltText="Filter DeleteColumn column"
                                                                                Text="Remove" UniqueName="DeleteColumn" ConfirmDialogType="RadWindow" Resizable="false"
                                                                                HeaderText="Actions" ConfirmText="Delete record?">
                                                                            </telerik:GridButtonColumn>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="PatientTBTreatment">
                                        <table style="width: 100%;" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Patient&#39;s TB Treatment
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadComboBox ID="cbPtnTBTreatment" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                        OnClientSelectedIndexChanged="ShowPrnTBTreat">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="wrapper_cbPtnTBTreatment">
                                        <div class="ShowIfPtnTBTreatment" style="display: none">
                                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        Other TB prophylaxis
                                                    </td>
                                                    <td style="width: 80%;">
                                                        <telerik:RadTextBox ID="txtPtnOtherProph" runat="server" Skin="MetroTouch">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div style="float: left">
                                            Nutrition</div>
                                        <div style="float: right; cursor: pointer; font-size: 16px !important; text-decoration: underline !important;
                                            margin-top: 5px;" onclick="$('#helpDocs2').slideToggle('slow');">
                                            Help Guide
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divFeedingPractice" runat="server" style="display: none">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Feeding practice
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="cbFeedingPractice" runat="server" Width="400px" Skin="MetroTouch">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="ShowForAllNutProblems">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Nutritional problems
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="cbNutionalProblems" runat="server" Skin="MetroTouch" Width="400px">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">
                                            Nutritional support
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="cbNurtionalSupport" runat="server" Skin="MetroTouch" Width="400px">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="bubble" id="helpDocs" style="display: none; position: relative; width: 200px;
                                left: 500px; z-index: 99; top: -740px; color: Green;">
                                <a href="PDF/WHO Staging 2007.pdf" target="_blank" onclick="$('#helpDocs').slideToggle('slow');">
                                    WHO Clinical Staging Guide</a>
                                <br />
                                <br />
                                <a href="PDF/TableForGradingSeverityofAdverseEvents.pdf" target="_blank" onclick="$('#helpDocs').slideToggle('slow');">
                                    Severity Grading of Adverse Events Guide</a>
                            </div>
                            <div class="bubble" id="helpDocs2" style="display: none; position: relative; width: 200px;
                                left: 500px; z-index: 99; top: -183px; color: Green;">
                                <a href="PDF/Growth_charts_PASDP.pdf" target="_blank" onclick="$('#helpDocs2').slideToggle('slow');">
                                    Growth curves for boys and girls</a>
                            </div>
                        </div>
                        <!-- TAB 2 -->
                        <%--<div id="tab2" class="scroll-pane jspScrollable tabwidth" style="width:811px; overflow:hidden; height: 380px;">
            

    </div>--%>
                        <!-- TAB 4 -->
                        <div id="tab4" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Adherence and Dispensing Note</div>
                                    </td>
                                </tr>
                            </table>
                            <div id="PreviousPrescription">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Previous prescription dispensed as prescribed?
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadButton ID="btnDispensedYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HidePharmNotes|hide"
                                                GroupName="DrugDispensed" Text="Yes">
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="btnDispensedNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HidePharmNotes|show"
                                                GroupName="DrugDispensed" Text="No">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!-- Show if prior prescription = no" -->
                            <div id="HidePharmNotes" style="display: none">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Pharmacy Notes
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadTextBox ID="txtPharmacyNotes" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                                Width="600px">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="ShowForAll">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            CTX Adherence
                                        </td>
                                        <td style="width: 30%;">
                                            <telerik:RadComboBox ID="cbCTXAdherence" runat="server" Skin="MetroTouch">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 20%;">
                                            ARV Adherence
                                        </td>
                                        <td style="width: 30%;">
                                            <telerik:RadComboBox ID="cbARVAdherence" runat="server" AutoPostBack="false" OnClientSelectedIndexChanged="CheckARVAdherence"
                                                Skin="MetroTouch">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="wrapper_cbARVAdherence">
                                <div id="ShowIfARVAdherenceFairPoor" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Why poor/fair ARV Adherence
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadComboBox ID="cbARVWhyReason" runat="server" Skin="MetroTouch" CheckBoxes="true"
                                                    CheckedItemsTexts="FitInInput" Width="400px" OnClientDropDownClosed="ShowARVWhy"
                                                    AutoPostBack="false">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="wrapper_cbARVWhyReason">
                                        <div id="ShowIfOtherARVReason" style="display: none">
                                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        Other poor/fair ARV reason
                                                    </td>
                                                    <td style="width: 80%;">
                                                        <telerik:RadTextBox ID="txtARVWhyOther" runat="server" Skin="MetroTouch" Width="400px">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div style="float: left">
                                            Regimen Plan</div>
                                        <div style="float: right; cursor: pointer; font-size: 16px !important; text-decoration: underline !important;
                                            margin-top: 5px;" onclick="$('#helpDocs3').slideToggle('slow');">
                                            Help Guide
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="SubstitutionsInterruptions">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            *Substitutions/ Interruptions
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="cbARVSubstitutions" Width="400px" runat="server" Skin="MetroTouch"
                                                OnClientSelectedIndexChanged="ShowSubInter" AutoPostBack="false">

                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="wrapper_cbARVSubstitutions">
                                <div id="ShowIfChange" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Change regimen reason
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadComboBox ID="rcbARVChangeReason" runat="server" Width="400px" Skin="MetroTouch"
                                                    CheckBoxes="true" CheckedItemsTexts="FitInInput" OnClientDropDownClosed="ShowChangeRegOther"
                                                    AutoPostBack="false">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="ShowIfChangeOther" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Change reason other
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadTextBox ID="txtARVChangeOther" Width="400px" runat="server" Skin="MetroTouch">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="ShowIfStopped" style="display: none">
                                    <table style="width: 100%;" cellpadding="10px" class="Section">
                                        <tr>
                                            <td style="width: 20%;">
                                                Stop regimen reason
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadComboBox ID="cbARVStopReason" runat="server" Width="400px" Skin="MetroTouch"
                                                    CheckBoxes="true" CheckedItemsTexts="FitInInput" OnClientDropDownClosed="ShowStopOther"
                                                    AutoPostBack="false">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%;">
                                                ART end date
                                            </td>
                                            <td style="width: 80%;">
                                                <telerik:RadDatePicker ID="dtARVEndDate" runat="server" Skin="MetroTouch">
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
                                </div>
                                <div id="wrapper_cbARVStopReason">
                                    <div id="ShowIfStoppedOther" style="display: none">
                                        <table style="width: 100%;" cellpadding="10px" class="Section">
                                            <tr>
                                                <td style="width: 20%;">
                                                    Stop reason other
                                                </td>
                                                <td style="width: 80%;">
                                                    <telerik:RadTextBox ID="txtStopReasonOther" Width="400px" runat="server" Skin="MetroTouch">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="ShowToAllPrescribe" style="display: none">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            <telerik:RadButton ID="btnPrescribeDrugs" Skin="MetroTouch" AutoPostBack="false"
                                                CommandArgument="rwPrescribeDrugs" OnClientClicked="OpenModal" Text="Prescribe Drugs"
                                                runat="server">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="SectionheaderTxt">
                                            <div>
                                                Laboratory Investigations</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">
                                            <telerik:RadButton ID="btnLabTests" Skin="MetroTouch" AutoPostBack="false" CommandArgument="rwOrderLabs"
                                                OnClientClicked="OpenModal" Text="Order Lab Tests" runat="server">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="bubble" id="helpDocs3" style="display: none; position: relative; width: 200px;
                                left: 500px; z-index: 99; top: -225px; color: Green;">
                                <a href="PDF/ARVDosing_2013.pdf" target="_blank" onclick="$('#helpDocs3').slideToggle('slow');">
                                    Paediatric Dosing Chart</a>
                            </div>
                        </div>
                        <!-- TAB 5 -->
                        <div id="tab5" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
                            height: 380px;">
                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Disclosure</div>
                                    </td>
                                </tr>
                            </table>
                            <div id="ShowIfDisclosureYes">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Disclosed to child?
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadButton ID="btnDisclosedYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HideDisclosedNote|show"
                                                GroupName="Disclosed" Text="Yes">
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="btnDisclosedNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" OnClientCheckedChanged="ShowMore" CommandArgument="HideDisclosedNote|hide"
                                                GroupName="Disclosed" Text="No">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <!-- Show if disclosed = yes -->
                            <div id="HideDisclosedNote" style="display: none">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Level of disclosure
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="rcbDisclosedLvl" runat="server" Skin="MetroTouch">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Referral and Consultations</div>
                                    </td>
                                </tr>
                            </table>
                            <div id="ReferredTo">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Referred To
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="cbReferredTo" runat="server" OnClientSelectedIndexChanged="ShowMoreSingle"
                                                Skin="MetroTouch" Width="400px">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="ShowIfReferredToOther" style="display: none">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Referred To Other
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadTextBox ID="txtReferredOther" runat="server" Skin="MetroTouch">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table style="width: 100%;" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt">
                                        <div>
                                            Next Appointment</div>
                                    </td>
                                </tr>
                            </table>
                            <div id="TransferOut">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Transfer out?
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadButton ID="btnTransOutYes" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" GroupName="transfered" Text="Yes">
                                            </telerik:RadButton>
                                            &nbsp;
                                            <telerik:RadButton ID="btnTransOutNo" runat="server" Skin="MetroTouch" AutoPostBack="false"
                                                ToggleType="Radio" GroupName="transfered" Text="No">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="NextAppointment">
                                <table style="width: 100%;" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Next appointment in
                                        </td>
                                        <td style="width: 28%;">
                                            <telerik:RadComboBox ID="cbNextAppointment" runat="server" Skin="MetroTouch" OnClientSelectedIndexChanged="SetDate">
                                                <Items>
                                                    <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                                    <telerik:RadComboBoxItem runat="server" Text="2 weeks" Value="2" />
                                                    <telerik:RadComboBoxItem runat="server" Text="4 weeks" Value="4" />
                                                    <telerik:RadComboBoxItem runat="server" Text="8 weeks" Value="8 weeks" />
                                                    <telerik:RadComboBoxItem runat="server" Text="12 weeks" Value="12" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 20%;">
                                            Next appointment date
                                        </td>
                                        <td style="width: 28%;">
                                            <telerik:RadDatePicker ID="dtNextAppointment" runat="server" Skin="MetroTouch">
                                                <Calendar ID="Calendar4" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                    Skin="MetroTouch" runat="server">
                                                </Calendar>
                                                <DateInput ID="DateInput4" DisplayDateFormat="dd-MMM-yyyy" DateFormat="dd/MM/yyyy"
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
                            </div>
                            <div id="Signature">
                                <table style="width: 100%; display: none" cellpadding="10px" class="Section">
                                    <tr>
                                        <td style="width: 20%;">
                                            Signature
                                        </td>
                                        <td style="width: 80%;">
                                            <telerik:RadComboBox ID="cbSignature" runat="server" Skin="MetroTouch">
                                                <Items>
                                                    <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; position: relative; margin: 5px 5px 5px 0px;">
                        <telerik:RadButton ID="btnPrint" runat="server" Visible="false" Text="Print" OnClick="btnPrint_OnClick">
                        </telerik:RadButton>
                    </div>
                    <!--Hidden Buttons -->
                    <asp:UpdatePanel ID="updtHdButtons" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <asp:Button ID="btnHidPFindings" runat="server" CssClass="hiddenButtons" OnClick="btnHidPFindings_Click" />
                            <asp:Button ID="btnHidTBSens" runat="server" CssClass="hiddenButtons" OnClick="btnHidTBSens_Click" />
                            <asp:Button ID="btnHidAE" runat="server" CssClass="hiddenButtons" OnClick="btnHidAE_Click" />
                            <asp:Button ID="btnHidTBSensNew" runat="server" CssClass="hiddenButtons" OnClick="btnHidTBSensNew_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!-- End of Hidden Buttons -->
                </ContentTemplate>
            </asp:UpdatePanel>
        </telerik:RadPane>
    </telerik:radsplitter>
</div>
<div id="ModalWindows">
    <telerik:radwindow runat="server" id="rwTBSensitivity" title="Contact TB Sensitivity"
        iconurl="/" modal="true" skin="BlackMetroTouch" width="500px" visibleonpageload="false"
        onclientclose="SetTBGrid" height="400px" behaviors="Move,Close">
        <ContentTemplate>
            <asp:UpdatePanel ID="updtTBSens" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="rgdTBDrugsSensitivity" runat="server" AutoGenerateColumns="False"
                        CellSpacing="0" GridLines="None" AllowPaging="false" AllowSorting="True">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" Display="false" UniqueName="ID" DataType="System.String" />
                                <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Drug column"
                                    HeaderText="TB Drugs" UniqueName="Name" DataType="System.String" />
                                <telerik:GridTemplateColumn DataField="Type" ReadOnly="false" FilterControlAltText="Filter Type column"
                                    HeaderText="Type" UniqueName="Type" DataType="System.Boolean">
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="rcbResSen" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Unknown" Value="Unknown" />
                                                <telerik:RadComboBoxItem Text="Sensitive" Value="Sensitive" />
                                                <telerik:RadComboBoxItem Text="Resistant" Value="Resistant" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:radwindow>
    <telerik:radwindow runat="server" id="rwNewSensitivity" title="New Sensitivity" modal="true"
        skin="BlackMetroTouch" width="500px" visibleonpageload="false" height="400px"
        behaviors="Move,Close" onclientclose="SetNewSensGrid">
        <ContentTemplate>
            <asp:UpdatePanel ID="uptNewSensitivity" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="rgdNewSensitivity" runat="server" AutoGenerateColumns="False"
                        CellSpacing="0" GridLines="None" AllowPaging="True" AllowSorting="True">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" Display="false" UniqueName="ID" DataType="System.String" />
                                <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Drug column"
                                    HeaderText="TB Drugs" UniqueName="Name" DataType="System.String" />
                                <telerik:GridTemplateColumn DataField="Type" ReadOnly="false" FilterControlAltText="Filter Type column"
                                    HeaderText="Type" UniqueName="Type" DataType="System.Boolean">
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="rcbNewResSens" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Unknown" Value="Unknown" />
                                                <telerik:RadComboBoxItem Text="Sensitive" Value="Sensitive" />
                                                <telerik:RadComboBoxItem Text="Resistant" Value="Resistant" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:radwindow>
    <telerik:radwindow runat="server" id="rwFindings" title="Physical Findings" modal="true"
        skin="BlackMetroTouch" width="500px" visibleonpageload="false" height="400px"
        onclientclose="SetPFGrid" behaviors="Move,Close">
        <ContentTemplate>
            <asp:UpdatePanel ID="uptPFindings" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadGrid ID="rgdPhysicalFindings" runat="server" AutoGenerateColumns="False"
                        CellSpacing="0" GridLines="None" AllowPaging="false" AllowSorting="True">
                        <MasterTableView>
                            <Columns>
                                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                                    UniqueName="ID" DataType="System.String" Display="false" />
                                <telerik:GridBoundColumn DataField="PName" FilterControlAltText="Filter Name column"
                                    HeaderText="Physical Finding" UniqueName="PName" DataType="System.String" />
                                <telerik:GridTemplateColumn DataField="YN" ReadOnly="false" FilterControlAltText="Filter YN column"
                                    HeaderText="Yes/No" UniqueName="YN" DataType="System.Boolean">
                                    <ItemTemplate>
                                        <telerik:RadButton ID="btnYN" runat="server" Width="52px" GroupName="GroupPF" ToggleType="CustomToggle"
                                            AutoPostBack="false" ButtonType="StandardButton">
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:radwindow>
    <telerik:radwindow runat="server" id="rwAdverseEvent" title="Adverse Events" modal="true"
        skin="BlackMetroTouch" width="600px" visibleonpageload="false" height="400px"
        onclientclose="SetAEGrid" behaviors="Move,Close">
        <ContentTemplate>
            <asp:UpdatePanel ID="updtAevents" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadTreeView ID="rtwAEvents" runat="server" Skin="BlackMetroTouch" CheckBoxes="true">
                    </telerik:RadTreeView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:radwindow>
    <telerik:radwindow runat="server" id="rwContactRecTreatment" title="Contact TB Treatment"
        modal="true" skin="BlackMetroTouch" width="500px" visibleonpageload="false" height="400px"
        behaviors="Move,Close">
        <ContentTemplate>
            <telerik:RadGrid ID="rgContactRecTreatment" runat="server" AllowMultiRowSelection="true"
                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" AllowPaging="True"
                AllowSorting="True" Skin="MetroTouch">
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                </ClientSettings>
                <MasterTableView PageSize="10">
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="Select All">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridBoundColumn DataField="Drug" FilterControlAltText="Filter Drug column"
                            HeaderText="TB Drugs" UniqueName="Drug" DataType="System.String" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </telerik:radwindow>
    <asp:HiddenField ID="hdnalreadydone" runat="server" />
</div>
<div style="visibility: collapse">
    <telerik:radbutton id="btnSave" runat='server' text="Test" causesvalidation="true"
        onclientclicking="CheckVals" onclick="btnSave_Click">
    </telerik:radbutton>
</div>
<div id="frmVisit_ScriptBlock" runat="server" style="display: none;">
    <script type="text/javascript">
        setTabs();
        if (parseInt($("#lblAge").html()) < 6) $("#DevScreen").show(); else $("#DevScreen").hide();
        if (parseInt($("#lblAge").html()) >= 10) $("#Tanner").show(); else $("#Tanner").hide();
        if (parseInt($("#lblAge").html()) < 14) $("#HCMUAC").show(); else $("#HCMUAC").hide();
        resizeScrollbars();
        Date.prototype.AddDays = function (noOfDays) {
            this.setTime(this.getTime() + (noOfDays * (1000 * 60 * 60 * 24)));
            return this;
        };
        function SetDate(sender, args) {

            var dp = $find("IDfrmVisitTouch_dtVisitDate_dateInput");
            var dt = dp.get_selectedDate();
            if (dt == null) {
                alert("No visit date selected");
            } else {
                var weekS = args.get_item().get_value();
                var datepicker = $find("IDfrmVisitTouch_dtNextAppointment_dateInput");

                dt.AddDays(parseInt(weekS) * 7);

                datepicker.set_selectedDate(dt);
            }
        }

        function CalcBMI(sender, args) {
            SetBMI();
        }

        function SetBMI() {
            var txtWeight = $find("IDfrmVisitTouch_txtWeight").get_value();
            var txtHeight = $find("IDfrmVisitTouch_txtHeight").get_value();
            if ((txtWeight != "") && (txtHeight != "")) {

                var BMI = txtWeight / ((txtHeight / 100) * (txtHeight / 100));
                var thePos = BMI.toString().indexOf(".");
                var theVal = '';
                if (thePos > 0)
                    theVal = BMI.toString().substring(0, thePos + 2);
                else
                    theVal = BMI.ToString();

                var txtBMI = $find("IDfrmVisitTouch_txtBMI");
                txtBMI.set_value(theVal);
            }
        }

        function CheckVals(sender, args) {
            var ReqVals = new Array();
            ReqVals[0] = "IDfrmVisitTouch_dtVisitDate_dateInput|Visit Date";
            ReqVals[1] = "IDfrmVisitTouch_cbARVSubstitutions_Input|Substitutions and Interruptions";
            var theNames = new Array();
            var ReqIsFilled = true; var thePos = 0;
            for (index = 0; index < ReqVals.length; ++index) {
                var arr = ReqVals[index].split("|");
                var theFirstControl = null;
                if (index != 1) {
                    if ($('#' + arr[0]).val() == "") {
                        theNames[index] = arr[1];
                        if (theFirstControl == null) {
                            theFirstControl = arr[0];
                            ReqIsFilled = false;
                        }

                    }

                } else {
                    if ($('#' + arr[0]).val() == "Select") {
                        theNames[index] = arr[1];
                        if (theFirstControl == null) theFirstControl = arr[0];
                        ReqIsFilled = false;
                    }
                }
                thePos = index;
            }

            var masterTable = $find("IDfrmVisitTouch_rgAE").get_masterTableView();
            if (masterTable != null) {
                if (thePos > 0) thePos += 1;
                for (var row = 0; row < masterTable.get_dataItems().length; row++) {

                    var drop1 = masterTable.get_dataItems()[row].findElement('rcbAESeverity');
                    if (drop1.value == "Select") {
                        theNames[thePos] = "Adverse Event Severity";
                        if (theFirstControl == null) theFirstControl = "IDfrmVisitTouch_rgAE";
                        ReqIsFilled = false;
                    }
                }
            }
            var txtsdate = document.getElementById('IDfrmVisitTouch_dtVisitDate_dateInput').value;
            $.ajax({
                type: "POST",
                async: false,
                url: "./frmTouchPatientHome.aspx/GetMessageFromWebPage",
                data: '{"formatdate":"' + txtsdate + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d > 0 && msg.d != '') {
                        if (theNames.length == 0) {
                            alert('Visit Already Exists');
                            ReqIsFilled = false;
                        }
                    }
                },
                failure: function(msg)
                {
                alert('Contact your administrator');
                }
            });

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
                if (theNames.length > 0) {
                    gotToTabVal(theFirstControl, theNames, args);
                }
                else {
                    args.set_cancel(true);
                }
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


        function SetSexActiveYN(combo, eventargs) {
            if (parseInt(eventargs.get_item().get_text()) >= 3) {
                $("#HideSexActiveYN").show();
                GoToElem('absBottom');
            }
            else {
                $("#HideSexActiveYN").hide();
                GoToElem('absBottom');
            }
        }
        function ShowSexActiveYN(object, args) {
            var substr = args._commandArgument.toString().split('|');
            if (substr[1] == "show") {
                $("." + substr[0]).show();
                if ($("#lblSex").html() == "Female") $(".ShowIfSexuallyActiveFemale").show();
                GoToElem('absBottom');
            } else {
                $("." + substr[0]).hide();
                if (substr[2] == "1") {
                    if ($("#lblSex").html() == "Female") $(".ShowIfSexuallyActiveFemale").hide();
                }
                GoToElem('absBottom');
            }
        }
        function ShowFamilyPlanning(object, args) {
            switch (object.get_uniqueID()) {
                case "IDfrmVisitTouch$btncondomsYes":
                    $(".ShowProtectedSexYN").show();
                    break;
                case "IDfrmVisitTouch$btncondomsNo":
                    $(".ShowProtectedSexYN").hide();
                    $("#ShowIfPlanningOther").hide();
                    break;
                default:
                    $(".ShowProtectedSexYN").hide();
                    $("#ShowIfPlanningOther").hide();
                    break;
            }
            GoToElem('absBottom');
        }
        function ShowStillOnTreatment(object, args) {
            switch (object.get_uniqueID()) {
                case "IDfrmVisitTouch$rbtnOnTreatmentYes":
                    ShowHideClass("ShowIfOnTreatmentYes", "hide");
                    break;
                case "IDfrmVisitTouch$rbtnOnTreatmentNo":
                    ShowHideClass("ShowIfOnTreatmentYes", "show");
                    break;
                default:
                    ShowHideClass("ShowIfOnTreatmentYes", "hide");
                    break;
            }
            GoToElem('absBottom');
        }

        function ShowARVWhy(comboBox) {
            var items = comboBox.get_items();
            for (var i = 0; i < items.get_count(); i++) {
                var item = items.getItem(i);
                if (item.get_checked())
                    if (item.get_text().indexOf("Other") > -1)
                        ShowHide("ShowIfOtherARVReason", "show");
            }
        }

        function ShowOtherFPlanning(object, args) {
            if (args.get_item().get_text() == "Other") {
                $("#ShowIfPlanningOther").show();
            } else {
                $("#ShowIfPlanningOther").hide();
            }
            GoToElem('absBottom');
        }

        function SetPFGrid(sender, args) {
            ShowMinLoading("somediv");
            $("#IDfrmVisitTouch_btnHidPFindings").click();
        }

        function SetTBGrid(sender, args) {
            ShowMinLoading("somediv");
            $("#IDfrmVisitTouch_btnHidTBSens").click();
        }

        function SetAEGrid(sender, args) {
            ShowMinLoading("somediv");
            $("#IDfrmVisitTouch_btnHidAE").click();
        }

        function SetNewSensGrid(sender, args) {
            ShowMinLoading("somediv");
            $("#IDfrmVisitTouch_btnHidTBSensNew").click();
        }

        function ShowChangeRegOther(s, args) {
            var items = s.get_items();
            for (var i = 0; i < items.get_count(); i++) {
                var item = items.getItem(i);
                if (item.get_checked()) {
                    if (item.get_text().indexOf("Other") > -1) {
                        ShowHide("ShowIfChangeOther", "show");
                    }
                }
            }

        }

        function ShowStopOther(s, args) {
            var items = s.get_items();
            for (var i = 0; i < items.get_count(); i++) {
                var item = items.getItem(i);
                if (item.get_checked()) {
                    if (item.get_text().indexOf("Other") > -1) {
                        ShowHide("ShowIfStoppedOther", "show");
                    }
                }
            }
        }

        function ShowTBStatusList(sender, args) {
            switch (args.get_item().get_text()) {

                case "TB Rx":
                    ShowHideClass("ShowIfTBRxQ1", "show");
                    ShowHideClass("ShowIfOnTreatmentYes", "hide");
                    break;
                case "Completing TB treatment now":
                    ShowHideClass("ShowIfTBRxQ1", "hide");
                    ShowHideClass("ShowIfOnTreatmentYes", "show");
                    break;
                default:
                    ShowHideClass("ShowIfTBRxQ1", "hide");
                    ShowHideClass("ShowIfOnTreatmentYes", "hide");
                    break;

            }
        }

        function ShowPrnTBTreat(s, args) {
            if (args.get_item().get_text().indexOf("Other") > -1) {
                ShowHideClass("ShowIfPtnTBTreatment", "show");
            } else {
                ShowHideClass("ShowIfPtnTBTreatment", "hide");
            }
        }

        function CheckARVAdherence(s, args) {
            if ((args.get_item().get_text() == "Fair") || (args.get_item().get_text() == "Poor")) {
                ShowHide("ShowIfARVAdherenceFairPoor", "show");
                resizeScrollbars();
            }
        }

        function ShowSubInter(s, args) {
            switch (args.get_item().get_text()) {

                case "Change regimen":
                    ShowHide("ShowIfChange", "show");
                    ShowHide("ShowIfStopped", "hide");
                    break;
                case "Stop treatment":
                    ShowHide("ShowIfChange", "hide");
                    ShowHide("ShowIfStopped", "show");
                    break;
                default:
                    ShowHide("ShowIfChange", "hide");
                    ShowHide("ShowIfStopped", "hide");
                    break;

            }
        }
    </script>
</div>
