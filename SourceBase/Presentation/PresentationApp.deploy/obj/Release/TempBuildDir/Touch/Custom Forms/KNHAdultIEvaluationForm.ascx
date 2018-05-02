<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNHAdultIEvaluationForm.ascx.cs" Inherits="Touch.Custom_Forms.KNHAdultIEvaluationForm" %>
<%@ Register TagPrefix="KNHuc" TagName="KNHModal" Src="~/Touch/KNH/KNHModal.ascx" %>
<div style="visibility: collapse">
        <telerik:RadButton ID="btnSave" runat="server" Text="Save" Skin="MetroTouch" Visible="true"
            OnClick="btnSave_Click">
        </telerik:RadButton>
    </div>
<asp:HiddenField ID="hidtab" Value="0" runat="server" />
<div id="FormContent" >
<telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
            ReloadOnShow="true" runat="server" EnableShadow="true" >
            <Windows>
              <%--  <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Move,Close" Width="800px"  Height="450px" 
                    NavigateUrl="~/Touch/KNH/KNHVitalSignShowModal.aspx" Title="Vital Signs" Modal="true" OnClientClose="OnClientClose" >
                </telerik:RadWindow>--%>
                <%--<telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Move,Close" Width="800px"  Height="450px" 
                    NavigateUrl="~/Touch/KNH/KNHPresentingComplaintsModal.aspx" Title="Presenting Complaints" Modal="true"  OnClientClose="OnClientClose" >
                </telerik:RadWindow>--%>
               <%--  <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Move,Close" Width="800px"  Height="450px" 
                    NavigateUrl="~/Touch/KNH/KNHMedicalHistoryModal.aspx" Title="Medical History" Modal="true"   >
                </telerik:RadWindow>--%>
                <telerik:RadWindow ID="RadWindow4" runat="server" Behaviors="Move,Close" Width="800px"  Height="450px" 
                    NavigateUrl="~/Touch/KNH/KNHWhoStagingModal.aspx" Title="WHO Staging" Modal="true"   >
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow5" runat="server" Behaviors="Move,Close" Width="800px"  Height="450px" 
                    NavigateUrl="~/Touch/KNH/KNHPhysicalExaminationModal.aspx" Title="Physical Examination" Modal="true"   >
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
    <div >
    
        <asp:PlaceHolder runat="server" ID="PlaceHolder1"></asp:PlaceHolder>
        
    
    </div>    
  <div id="tabs" style="width: 800px">
        <ul>
            <li><a href="#tab1">Triage</a></li>
            <li><a href="#tab2">Clinical History</a></li>
            <li><a href="#tab3">TB Screening</a></li>
            <li><a href="#tab4">Examination</a></li>
            <li><a href="#tab5">Management</a></li>
            <li><a href="#tab6">Prev With +ve</a></li>
        </ul>

    <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
        height: 380px;">
        <asp:UpdatePanel ID="uptTab1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="Triage" cellpadding="10px" class="Section">
                 <tr>
                    <td class="SectionheaderTxt" style="width: 100%">
                   
                      Client Information
                    </td>
                    </tr> 
                   
                 <tr>
                 <td>
                 <table class="AdultIE">
                    <tr>
                    <td>
                     Visit date:
                    </td>
                    <td>
                     <telerik:RadDatePicker ID="dtVistiDate" runat="server" Skin="MetroTouch">
                                 <Calendar ID="Calendar31" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                     Skin="MetroTouch" runat="server">
                                 </Calendar>
                                 <DateInput ID="DateInput31" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                    </td>
                    <td>
                    </td>
                    </tr>
                     <tr>
                         <td>
                             Diagnosis Confirmed :
                         </td>
                         <td>
                             <telerik:RadButton ID="RadbtnDiagnosisYesNo" runat="server" Width="52px" GroupName="BirthBCG"
                                 ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                 <ToggleStates>
                                     <telerik:RadButtonToggleState Text="No" />
                                     <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                 </ToggleStates>
                             </telerik:RadButton>
                         </td>
                         <td>
                             Date of HIV Diagnosis :
                         </td>
                         <td>
                             <telerik:RadDatePicker ID="dtConfirmHIVPosDate" runat="server" Skin="MetroTouch">
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
                         <td>
                             Patient accompanied by caregiver:
                         </td>
                         <td>
                             <telerik:RadButton ID="radbtnChildAccompaniedBy" runat="server" Width="52px" GroupName="BirthBCG"
                                 ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                 <ToggleStates>
                                     <telerik:RadButtonToggleState Text="No" />
                                     <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                 </ToggleStates>
                             </telerik:RadButton>
                         </td>
                         <td>
                             Caregiver relationship:
                         </td>
                         <td>
                             <telerik:RadComboBox ID="rcbTreatmentSupporterRelationship" runat="server" EmptyMessage="Select"
                                 AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"
                                 Enabled="true">
                             </telerik:RadComboBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Health education given?:
                         </td>
                         <td>
                             <telerik:RadButton ID="radbtnHealthEducation" runat="server" Width="52px" GroupName="BirthBCG"
                                 ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                 <ToggleStates>
                                     <telerik:RadButtonToggleState Text="No" />
                                     <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                 </ToggleStates>
                             </telerik:RadButton>
                         </td>
                         <td>
                             If adolescent, disclosure Status:
                         </td>
                         <td>
                             <telerik:RadComboBox ID="rcbDisclosureStatus" runat="server" EmptyMessage="Select"
                                 AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"
                                 Enabled="true">
                             </telerik:RadComboBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             If adolescent, schooling Status:
                         </td>
                         <td>
                             <telerik:RadComboBox ID="rcbSchoolingStatus" runat="server" EmptyMessage="Select"
                                 AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"
                                 Enabled="true">
                             </telerik:RadComboBox>
                         </td>
                         <td>
                             Is client a member of a support group?:
                         </td>
                         <td>
                             <telerik:RadButton ID="radbtnHIVSupportgroup" runat="server" Width="52px" GroupName="BirthBCG"
                                 ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                 <ToggleStates>
                                     <telerik:RadButtonToggleState Text="No" />
                                     <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                 </ToggleStates>
                             </telerik:RadButton>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Patient reffered from?:
                         </td>
                         <td>
                             <telerik:RadComboBox ID="rcbPatientReferred" runat="server" EmptyMessage="Select"
                                 AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"
                                 Enabled="true">
                             </telerik:RadComboBox>
                         </td>
                         <td>
                             Nurse Comments:
                         </td>
                         <td>
                             <telerik:RadTextBox ID="txtNursesComments" runat="server" Width="200px" TextMode="MultiLine"
                                 Skin="MetroTouch">
                             </telerik:RadTextBox>
                         </td>
                     </tr>
                 </table>
                 </td>
                 </tr>
                 <tr>
                 <td colspan="4">
                     <table width="100%" class="AdultIE">
                         <tr>
                             <td>
                                 <telerik:RadButton ID="btnDisplayVitalSignModal" runat="server" Text="Vital Sign"
                                     AutoPostBack="false" CommandArgument="IDKNHAdultIEvaluationForm_TheModal_rwVital"
                                     OnClientClicked="OpenModalASPX">
                                 </telerik:RadButton>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Temperature (Celsius):
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadTemperatureModalparent" runat="server" Enabled="false">
                                 </telerik:RadNumericTextBox>
                             </td>
                             <td>
                                 RR (Bpm):
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadRespirationRateparent" runat="server" Enabled="false"
                                     Skin="MetroTouch">
                                 </telerik:RadNumericTextBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Heart Rate (Bpm):
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadHeartRateparent" runat="server" Enabled="false"
                                     Skin="MetroTouch">
                                 </telerik:RadNumericTextBox>
                             </td>
                             <td>
                                 Systollic Blood
                                 <br />
                                 Pressure mmHg:
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadSystollicBloodPressureparent" runat="server"
                                     Enabled="false" Skin="MetroTouch">
                                 </telerik:RadNumericTextBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Diastolic Blood
                                 <br />
                                 Pressure mmHg:
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadDiastolicBloodPressureparent" runat="server"
                                     Enabled="false" Skin="MetroTouch">
                                 </telerik:RadNumericTextBox>
                             </td>
                             <td>
                                 Height (cms):
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadHeightparent" runat="server" Enabled="false"
                                     Skin="MetroTouch">
                                 </telerik:RadNumericTextBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Weight (kgs):
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadWeightparent" runat="server" Enabled="false"
                                     Skin="MetroTouch">
                                 </telerik:RadNumericTextBox>
                             </td>
                             <td>
                                 BMI:
                             </td>
                             <td>
                                 <telerik:RadNumericTextBox ID="txtRadBMIparent" runat="server" Enabled="false" Skin="MetroTouch">
                                 </telerik:RadNumericTextBox>
                             </td>
                         </tr>
                        
                     </table>
                 </td>
                 </tr>

               
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="tab2" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
        height: 380px;">
        <asp:UpdatePanel ID="updtPc" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="ClinicalHistory" cellpadding="10px" class="Section">
                    <tr>
                        <td>
                                <table class="AdultIE" width="100%">
                                    <tr>
                                        <td align="left">
                                            <telerik:RadButton ID="radbtnPreComplaints" runat="server" Text="Presenting Complaints" AutoPostBack="false"
                                                CommandArgument="IDKNHAdultIEvaluationForm_TheModal_rwPresentingComplaints" OnClientClicked="OpenModalASPX">
                                            </telerik:RadButton>

                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                        <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridPresentingParent" runat="server"
                                            Width="100%" PageSize="10" AllowPaging="false" AllowMultiRowSelection="false"
                                            ClientSettings-Selecting-AllowRowSelect="true" ShowFooter="false" Skin="MetroTouch"
                                            ShowHeader="false" >
                                            <PagerStyle Mode="NextPrevAndNumeric" />
                                            <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                                                CellSpacing="0" CellPadding="0">
                                                <Columns>
                                                    <telerik:GridTemplateColumn>
                                                        <HeaderStyle Width="150px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPresenting" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                           
                                                            <asp:CheckBox ID="ChkPresenting" Checked="true" runat="server" Enabled="false"  Text='<%# Eval("NAME") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="SelectedItems">
                                                        <HeaderStyle Width="100px" />
                                                        <ItemTemplate>
                                                            <telerik:RadTextBox ID="txtPresenting" runat="server" Skin="MetroTouch" Enabled="false" Text='<%# Eval("chkValText") %>'
                                                                Width="150px">
                                                            </telerik:RadTextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <AlternatingItemStyle BorderStyle="None" />
                                            </MasterTableView>
                                            <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" />
                                            <%-- <ItemStyle Font-Names="Verdana" Font-Size="8pt" />--%>
                                        </telerik:RadGrid>

                                    </td>
                                    </tr>
                                    <tr>
                                    <td>
                                      <table width="100%">
                                       <tr>
                                       <td>
                                        Additional Complaints:
                                       </td>
                                       <td>
                                        <telerik:RadTextBox ID="txtAdditionPresentingComplaints" runat="server" Enabled="false" Width="100%" TextMode="MultiLine" Skin="MetroTouch"></telerik:RadTextBox>
                                       </td>
                                       </tr>
                                      </table>
                                    </td>
                                    </tr>
                                </table>
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                         
                                <table class="section" width="100%">
                                    <tr>
                                        <td>
                                              <telerik:RadButton ID="radbtnMedicalhistory" runat="server" Text="Medical History" AutoPostBack="false"
                                                CommandArgument="IDKNHAdultIEvaluationForm_TheModal_rwMedicalHistory" OnClientClicked="OpenModalASPX">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                             <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridFieldName" runat="server"
                                    Width="100%" AllowPaging="false" AllowMultiRowSelection="false" ShowFooter="false"
                                    CellPadding="0" ShowHeader="false" Skin="MetroTouch" HierarchyLoadMode="ServerOnDemand"
                                    DataKeyNames="SectionID" >
                                    <MasterTableView Name="ChildGrid">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Section Details" HeaderStyle-Font-Bold="true"
                                                UniqueName="SectionID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSectionID" runat="server"  Visible="false" Text='<%# Eval("SectionID") %>'></asp:Label>
                                                    <asp:Label ID="lblFieldID" runat="server"  Visible="false" Text='<%# Eval("FieldID") %>'></asp:Label>
                                                    <asp:Label ID="lblfieldName" runat="server"  Text='<%# Eval("fieldName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="FieldValue" HeaderStyle-Font-Bold="true">
                                                <ItemTemplate>
                                                    <telerik:RadTextBox ID="txtRadText" runat="server" Width="120px"  Enabled="false" Skin="MetroTouch" Text='<%# Eval("FieldVaue") %>'>
                                                    </telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                           
                        </td>
                    </tr>
                    <tr>
                        <td class="SectionheaderTxt" style="width: 100%">
                            Other Medical History:
                        </td>
                    </tr>
                    <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    LMP assessed?:
                                </td>
                                <td>
                                    <telerik:RadButton ID="radbtnLMPAssessmentValid" runat="server" Width="52px" GroupName="BirthBCG"
                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="No" />
                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                                </td>
                                <td>
                                    LMP Date:
                                </td>
                                <td>
                                <telerik:RadDatePicker ID="dtLMPAssessmentValid" runat="server" Skin="MetroTouch">
                                        <Calendar ID="Calendar30" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                            Skin="MetroTouch" runat="server">
                                        </Calendar>
                                        <DateInput ID="DateInput30" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
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
                                    Reason LMP not assessed:
                                </td> 
                                <td>
                                    <telerik:RadComboBox ID="rcbLMPNotaccessedReason" runat="server" EmptyMessage="Select"
                                        AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"
                                        Enabled="true">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    EDD:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dtEDDDate" runat="server" Skin="MetroTouch">
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
                                    Other disease name:
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtOtherDiseaseName" runat="server" Skin="MetroTouch" Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    Other disease date:
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dtOtherDiseaseDate" runat="server" Skin="MetroTouch">
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
                            <tr>
                                <td>
                                    Other disease treatment:
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtOtherDiseaseTreatment" runat="server" Skin="MetroTouch"
                                        Width="150px">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    If adolescent and schooling, current school perfomance:
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="rcbSchoolPerfomance" runat="server" EmptyMessage="Select"
                                        AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"
                                        Enabled="true">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    </tr>
                  
                    


                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="tab3" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
        height: 380px;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table id="tbScreening" cellpadding="10px" class="Section">
                <tr>
                 <td class="SectionheaderTxt" style="width: 100%">
                  TB Assessment
                 </td>
                </tr>
                 <tr>
                 <td style="width: 100%">
                     <table width="100%">
                         <tr>
                             <td>
                                 TB Assessment (ICF):
                             </td>
                             <td>
                                    <telerik:RadComboBox ID="rcbTBAssessmentICF" runat="server" Text="aSomeTest" AutoPostBack="false"
                                                Enabled="true" Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                CheckedItemsTexts="FitInInput" Width="250px">
                                            </telerik:RadComboBox>
                             </td>
                             <td>
                                 TB Findings:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbTBFindings" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                     Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" Enabled="true">
                                 </telerik:RadComboBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Available TB results:
                             </td>
                             <td>
                                 <telerik:RadButton ID="radbtnTBresultsAvailable" runat="server" Width="52px" GroupName="BirthBCG"
                                     ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                     <ToggleStates>
                                         <telerik:RadButtonToggleState Text="No" />
                                         <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                     </ToggleStates>
                                 </telerik:RadButton>
                             </td>
                             <td>
                                 Sputum smear:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbSputumSmear" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                     Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" Enabled="true">
                                 </telerik:RadComboBox>
                             </td>
                         </tr>

                         <tr>
                             <td>
                                 Sputum smear date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtSputumSmearDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar4" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput4" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                 Chest X-ray:
                             </td>
                             <td>
                                 <telerik:RadButton ID="radbtnChestray" runat="server" AutoPostBack="false" ButtonType="StandardButton"
                                     GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle" Width="52px">
                                     <ToggleStates>
                                         <telerik:RadButtonToggleState Text="No" />
                                         <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                     </ToggleStates>
                                 </telerik:RadButton>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Chest X-ray date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtChestrayDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar5" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput5" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                 Tissue Biopsy:
                             </td>
                             <td>
                                 <telerik:RadButton ID="radbtnTissueBiopsy" runat="server" AutoPostBack="false" ButtonType="StandardButton"
                                     GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle" Width="52px">
                                     <ToggleStates>
                                         <telerik:RadButtonToggleState Text="No" />
                                         <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                     </ToggleStates>
                                 </telerik:RadButton>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Tissue Biopsy date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtTissueBiopsyDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar6" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput6" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                 CXR results:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbCXR" runat="server" AutoPostBack="false" CheckedItemsTexts="FitInInput"
                                     EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true" Skin="MetroTouch">
                                 </telerik:RadComboBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Other CXR (specify)
                             </td>
                             <td colspan="3">
                                 <telerik:RadTextBox ID="txtOtherCXR" runat="server" Skin="MetroTouch" Width="100%">
                                 </telerik:RadTextBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 TB Type:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbTBTypePeads" runat="server" AutoPostBack="false" CheckedItemsTexts="FitInInput"
                                     EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true" Skin="MetroTouch">
                                 </telerik:RadComboBox>
                             </td>
                             <td>
                                 Patient Type:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbPeadsTBPatientType" runat="server" AutoPostBack="false"
                                     CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                     Skin="MetroTouch">
                                 </telerik:RadComboBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 TB Plan:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbTBPlan" runat="server" AutoPostBack="false" CheckedItemsTexts="FitInInput"
                                     EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true" Skin="MetroTouch">
                                 </telerik:RadComboBox>
                             </td>
                             <td>
                                 Specify Other TB plan:
                             </td>
                             <td>
                                 <telerik:RadTextBox ID="txTBPlanOther" runat="server" Skin="MetroTouch" Width="100%">
                                 </telerik:RadTextBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 TB Regimen
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbTBRegimen" runat="server" AutoPostBack="false" CheckedItemsTexts="FitInInput"
                                     EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true" Skin="MetroTouch">
                                 </telerik:RadComboBox>
                             </td>
                             <td>
                                 Other TB regimen :
                             </td>
                             <td>
                                 <telerik:RadTextBox ID="txTBRegimenOther" runat="server" Skin="MetroTouch" Width="100%">
                                 </telerik:RadTextBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 TB Regimen start date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtTBRegimenStartDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar7" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput7" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                 TB Regimen end date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtTBRegimenEndDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar8" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput8" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                         </tr>
                         <tr>
                             <td>
                                 TB Treatment outcome:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbTBTreatmentOutcomesPeads" runat="server" AutoPostBack="false"
                                     CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                     Skin="MetroTouch">
                                 </telerik:RadComboBox>
                             </td>
                         </tr>
                         <tr>
                             <td colspan="4" class="SectionheaderTxt">
                                 IPT(Patients with No signs and Symptoms)
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 No TB signs? (Start INH):
                             </td>
                             <td>
                                 <telerik:RadButton ID="radbtnNoTB" runat="server" AutoPostBack="false" ButtonType="StandardButton"
                                     GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle" Width="52px">
                                     <ToggleStates>
                                         <telerik:RadButtonToggleState Text="No" />
                                         <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                     </ToggleStates>
                                 </telerik:RadButton>
                             </td>
                             <td>
                                 If Yes, TB reason?:
                             </td>
                             <td>
                                 <telerik:RadComboBox ID="rcbTBReason" runat="server" Text="aSomeTest" AutoPostBack="false"
                                     Enabled="true" Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                     CheckedItemsTexts="FitInInput" Width="250px">
                                 </telerik:RadComboBox>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 INH Start date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtINHStartDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar9" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput9" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                 INH End date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtINHEndDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar10" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput10" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                         </tr>
                         <tr>
                             <td>
                                 Pyridoxine Start date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtPyridoxineStartDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar11" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput11" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                 Pyridoxine End date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtPyridoxineEndDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar12" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput12" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                         </tr>
                         <tr>
                             <td colspan="4" class="SectionheaderTxt" style="width: 100%">
                                 Discontinue IPT
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Confirmed or TB Suspected? (Stop INH):
                             </td>
                             <td>
                                 <telerik:RadButton ID="radbtnSuspectTB" runat="server" AutoPostBack="false" ButtonType="StandardButton"
                                     GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle" Width="52px">
                                     <ToggleStates>
                                         <telerik:RadButtonToggleState Text="No" />
                                         <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                     </ToggleStates>
                                 </telerik:RadButton>
                             </td>
                             <td>
                                 Stop INH Date:
                             </td>
                             <td>
                                 <telerik:RadDatePicker ID="dtStopINHDate" runat="server" Skin="MetroTouch">
                                     <Calendar ID="Calendar13" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                         UseRowHeadersAsSelectors="False">
                                     </Calendar>
                                     <DateInput ID="DateInput13" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                         </tr>
                         <tr>
                             <td>
                                 Contacts screened for TB:
                             </td>
                             <td>
                                 <telerik:RadButton ID="radbtnContactsScreenedForTB" runat="server" AutoPostBack="false" ButtonType="StandardButton"
                                     GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle" Width="52px">
                                     <ToggleStates>
                                         <telerik:RadButtonToggleState Text="No" />
                                         <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                     </ToggleStates>
                                 </telerik:RadButton>
                             </td>
                             <td>
                                 If No, specify?
                             </td>
                             <td>
                                 <telerik:RadTextBox ID="txtTBNotScreenedSpecify" runat="server" Skin="MetroTouch" Width="100%">
                                 </telerik:RadTextBox>
                             </td>
                         </tr>
                     </table>
                 </td>
                 </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
      <div id="tab4" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
          height: 380PX;">
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                  <table id="TabExamination" cellpadding="10px" class="Section">
                      <tr>
                          <td class="SectionheaderTxt" style="width: 100%">
                              Current Long Term Medications
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <table>
                                  <tr>
                                      <td>
                                          Long term medications:
                                      </td>
                                      <td>
                                          <telerik:RadButton ID="radbtnLongTermMedications" runat="server" AutoPostBack="false"
                                              ButtonType="StandardButton" GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle"
                                              Width="52px">
                                              <ToggleStates>
                                                  <telerik:RadButtonToggleState Text="No" />
                                                  <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                              </ToggleStates>
                                          </telerik:RadButton>
                                      </td>
                                      <td>
                                          Cotrimoxazole:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtSulfaTMPDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar14" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput14" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                  </tr>
                                  <tr>
                                      <td>
                                          Hormonal contraceptives:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtHormonalContraceptivesDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar15" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput15" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                          Antihypertensives:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtAntihypertensivesDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar16" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput16" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                  </tr>
                                  <tr>
                                      <td>
                                          Hypoglycemics:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtHypoglycemicsDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar17" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput17" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                          Fluconazole:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtAntifungalsDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar18" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput18" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                  </tr>
                                  <tr>
                                      <td>
                                          Antincovulsants:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtAntincovulsantsDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar19" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput19" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                          &nbsp;
                                      </td>
                                      <td>
                                          &nbsp;
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          Other current long term medications:
                                      </td>
                                      <td>
                                          <telerik:RadTextBox ID="txOtherLongTermMedications" runat="server" Skin="MetroTouch"
                                              Width="100%">
                                          </telerik:RadTextBox>
                                      </td>
                                      <td>
                                          Other long term medications:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtOtherCurrentLongTermMedications" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar22" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput22" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                  </tr>
                              </table>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <table>
                                  <tr>
                                      <td>
                                          <div>
                                              <%-- <li><a href="#" onclick="openWinPhyExam();return false;">
                                    Physical Examination</a></li>--%>
                                              <telerik:RadButton ID="RadButton1" runat="server" Skin="MetroTouch" Text="Physical Examination"
                                                  ButtonType="LinkButton" OnClientClicked="openWinPhyExam">
                                              </telerik:RadButton>
                                              <asp:HiddenField ID="hiddpcValue" runat="server" />
                                          </div>
                                      </td>
                                  </tr>
                              </table>
                          </td>
                      </tr>
                      <tr>
                          <td class="SectionheaderTxt" style="width: 100%">
                              HIV-Related Tests
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <table width="100%">
                                  <tr>
                                      <td>
                                          HIV related history:
                                      </td>
                                      <td>
                                          <telerik:RadComboBox ID="rcbHIVRelatedHistory" runat="server" AutoPostBack="false"
                                              CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                              Skin="MetroTouch">
                                          </telerik:RadComboBox>
                                      </td>
                                      <td>
                                          Initial CD4:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtInitialCD4" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          Initial CD4%:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtInitialCD4Percent" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                      <td>
                                          Initial CD4 Date:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtInitialCD4Date" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar23" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput23" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                  </tr>
                                  <tr>
                                      <td>
                                          Highest CD4 ever:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtHighestCD4Ever" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                      <td>
                                          Highest CD4 %:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtHighestCD4Percent" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          Highest CD4 ever date:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtHighestCD4Date" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar24" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput24" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                          CD4 at ART initiation:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtCD4atARTinitiation" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          CD4 % at ART initiation:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtCD4PercentAtARTInitiation" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                      <td>
                                          ART initiation CD4 Date:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtCD4atARTinitiationDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar25" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput25" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                  </tr>
                                  <tr>
                                      <td>
                                          Most Recent CD4:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtMostRecentCD4" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                      <td>
                                          Most Recent CD4 %
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtMostRecentCD4Percent" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          Most recent CD4 Date:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtMostRecentCD4Date" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar26" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput26" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                          Previous viral load:
                                      </td>
                                      <td>
                                          <telerik:RadNumericTextBox ID="txtPreviousViralLoad" runat="server" Skin="MetroTouch">
                                          </telerik:RadNumericTextBox>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td>
                                          Previous viral load date
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtPreviousViralLoadDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar27" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput27" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                          Other HIV related History:
                                      </td>
                                      <td>
                                          <telerik:RadTextBox ID="txtOtherHIVRelatedHistory" runat="server" Skin="MetroTouch"
                                              Width="100%">
                                          </telerik:RadTextBox>
                                      </td>
                                  </tr>
                              </table>
                          </td>
                      </tr>
                      <tr>
                          <td class="SectionheaderTxt" style="width: 100%">
                              ARV Exposure
                          </td>
                      </tr>
                      <tr>
                          <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Any Exposure to ARVs?:
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="radbtnARVExposure" runat="server" AutoPostBack="false"
                                            ButtonType="StandardButton" GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle"
                                            Width="52px">
                                            <ToggleStates>
                                                <telerik:RadButtonToggleState Text="No" />
                                                <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                            </ToggleStates>
                                        </telerik:RadButton>
                                    </td>
                                    <td>
                                        PMTCT Start Date:
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="dtPMTC1StartDate" runat="server" Skin="MetroTouch">
                                            <Calendar ID="Calendar20" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                UseRowHeadersAsSelectors="False">
                                            </Calendar>
                                            <DateInput ID="DateInput20" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                </tr>
                                <tr>
                                    <td>
                                        PMTCT Regimen:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPMTC1Regimen" runat="server" AutoPostBack="True"
                                            Skin="MetroTouch" Width="100%">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        PEP Regimen:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtPEP1Regimen" runat="server" AutoPostBack="True" Skin="MetroTouch"
                                            Width="100%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                      <td>
                                          PEP Start Date:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtPEP1StartDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar28" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput28" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                          HAART Regimen:
                                      </td>
                                      <td>
                                          <telerik:RadTextBox ID="txtHAART1Regimen" runat="server" AutoPostBack="True" Skin="MetroTouch"
                                              Width="120px">
                                          </telerik:RadTextBox>
                                      </td>
                                  </tr>
                                <tr>
                                      <td>
                                          HAART Start date:
                                      </td>
                                      <td>
                                          <telerik:RadDatePicker ID="dtHAART1StartDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar21" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput21" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                      </td>
                                      <td>
                                      </td>
                                  </tr>
                            </table>
                          </td>
                      </tr>
                      <tr>
                                      <td class="SectionheaderTxt" style="width: 100%">
                                          Impression
                                      </td>
                                  </tr>
                      <tr>
                          <td style="width: 100%">
                              <telerik:RadTextBox ID="txtImpression" runat="server" AutoPostBack="True" Skin="MetroTouch" Width="500px">
                              </telerik:RadTextBox>
                          </td>
                      </tr>
                      <tr>
                          <td class="SectionheaderTxt">
                              Diagnosis
                          </td>
                      </tr>
                      <tr>
                          <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Diagnosis and current illness at this visit:
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="rcbDiagnosis" runat="server" Text="aSomeTest" AutoPostBack="false"
                                            Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        Specify HIV related OI:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtHIVRelatedOI" runat="server" AutoPostBack="false" Skin="MetroTouch">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Specify Non HIV related OI:
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNonHIVRelatedOI" runat="server" AutoPostBack="false" Skin="MetroTouch">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <table width="100%">
                                  <tr>
                                      <td>
                                          <telerik:RadButton ID="radbtnstaging" runat="server" Skin="MetroTouch" Text=" Click To Enter Who Staging"
                                              ButtonType="LinkButton" OnClientClicked="openWinWhoStaging">
                                          </telerik:RadButton>
                                      </td>
                                  </tr>
                              </table>
                          </td>
                      </tr>
                      <tr>
                      <td>
                          <fieldset>
                              <div style="height: 75PX;">
                              </div>
                          </fieldset>
                      </td>
                      </tr>
                      <tr>
                       <td class="SectionheaderTxt" style="width: 100%">
                        Staging at initial evaluation
                       </td>
                      </tr>
                      <tr>
                      <td>
                       <table>
                         <tr>
                         <td>
                          WHO stage at initiation (transfer in):
                         </td>
                         <td>
                          <telerik:RadComboBox ID="rcbInitiationWHOstage" runat="server" AutoPostBack="false"
                                     CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                     Skin="MetroTouch">
                                 </telerik:RadComboBox>
                         </td>
                         <td>
                          Current WHO stage:
                         </td>
                         <td>
                         <telerik:RadComboBox ID="rcbWhoStage" runat="server" AutoPostBack="false"
                                     CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                     Skin="MetroTouch">
                                 </telerik:RadComboBox>
                         </td>

                         </tr>
                         <tr>
                         <td>
                          WAB:
                         </td>
                         <td>
                          <telerik:RadComboBox ID="rcbWABStage" runat="server" AutoPostBack="false"
                                     CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                     Skin="MetroTouch">
                                 </telerik:RadComboBox>
                          
                         </td>
                         <td>
                          Tanner Staging:
                         </td>
                         <td>
                           <telerik:RadComboBox ID="rcbTannerStaging" runat="server" AutoPostBack="false"
                                     CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                     Skin="MetroTouch">
                                 </telerik:RadComboBox>
                         </td>
                         </tr>
                         <tr>
                         <td>
                          Mernarche:
                         </td>
                         <td>
                           <telerik:RadButton ID="radbtnMernarche" runat="server" AutoPostBack="false" ButtonType="StandardButton"
                                     GroupName="BirthBCG" Skin="MetroTouch" ToggleType="CustomToggle" Width="52px">
                                     <ToggleStates>
                                         <telerik:RadButtonToggleState Text="No" />
                                         <telerik:RadButtonToggleState CssClass="BlueBG" Text="Yes" />
                                     </ToggleStates>
                                 </telerik:RadButton>
                         </td>
                         </tr>
                        

                       </table>
                      
                      </td>
                      </tr>
                     <tr>
                     <td align="center">
                        <table width="50%">
                        <tr>
                         <td>
                           Signature:
                         </td>
                         <td>
                          <telerik:RadComboBox ID="rcbSignature" runat="server" AutoPostBack="false"
                                     CheckedItemsTexts="FitInInput" EmptyMessage="Select" Enabled="true" EnableLoadOnDemand="true"
                                     Skin="MetroTouch">
                                 </telerik:RadComboBox>
                         </td>
                        </tr>
                        </table>
                     </td>
                     </tr>

                  </table>
              </ContentTemplate>
          </asp:UpdatePanel>
      </div>
      <div id="tab5" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
          height: 380PX;">
          <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                  <table id="TabManagement" cellpadding="10px" class="Section">
                  <tr>
                  <td class="SectionheaderTxt" style="width: 100%">
                   Drug Allergy and Toxicities
                  </td>
                  </tr>
                  <tr>
                          <td style="width: 100%">
                              <table>
                                  <tr>
                                      <td>
                                          Drug allergy toxicities:
                                      </td>
                                      <td>
                                          <telerik:RadComboBox ID="rcbDrugAllergiesToxicitiesPaeds" runat="server" Text="aSomeTest" AutoPostBack="false"
                                              Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                              Width="120px">
                                          </telerik:RadComboBox>
                                      </td>
                                      <td>
                                          Specify antibiotic allergy:
                                      </td>
                                      <td>
                                          <telerik:RadTextBox ID="txtSpecifyAntibioticAllery" runat="server" Wrap="true">
                                          </telerik:RadTextBox>
                                      </td>
                                  </tr>
                                  <tr>
                                  <td>
                                    Specify other drug allergy:
                                  </td>
                                  <td>
                                     <telerik:RadTextBox ID="txtOtherDrugAllergy" runat="server" Wrap="true">
                                          </telerik:RadTextBox>
                                  </td>
                                  </tr>
                              </table>
                          </td>
                      </tr>
                  <tr>
                  <td class="SectionheaderTxt" style="width: 100%">
                   ARV Side Effects:
                  </td>
                  </tr>
                  <tr>
                  <td style="width: 100%">
                   <table>
                    <tr>
                    <td>
                      Any ARV side effects?:
                    </td>
                    <td>
                      <telerik:RadButton ID="radbtnARVSideEffects" runat="server" Width="52px" GroupName="BirthBCG"
                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="No" />
                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                    </td>
                    <td>
                      Short term effects:
                    </td>
                    <td>
                      <telerik:RadComboBox ID="rcbShortTermEffects" runat="server" Text="aSomeTest" AutoPostBack="false"
                                              Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                              Width="120px">
                                          </telerik:RadComboBox>
                    </td>
                    </tr> 
                    <tr>
                    <td>
                     Specify other short term effect:
                    </td>
                    <td>
                      <telerik:RadTextBox ID="txtOtherShortTermEffects" runat="server" Wrap="true">
                                            </telerik:RadTextBox>
                    </td>
                    <td>
                     Long term effects:
                    </td>
                    <td>
                     <telerik:RadComboBox ID="rcbLongTermEffects" runat="server" Text="aSomeTest" AutoPostBack="false"
                                              Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                              Width="120px">
                                          </telerik:RadComboBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                     Specify other longterm effects:
                    </td>
                    <td>
                     <telerik:RadTextBox ID="radtxtOtherLongtermEffects" runat="server" Wrap="true">
                                            </telerik:RadTextBox>
                    </td>
                    </tr>

                   </table>
                  </td>
                  </tr>
                  <tr>
                          <td class="SectionheaderTxt" style="width: 100%">
                              Work up plan
                          </td>
                      </tr>
                  <tr>
                   <td style="width: 100%">
                       <telerik:RadTextBox ID="txtWorkUpPlan" runat="server" TextMode="MultiLine" Width="500px">
                       </telerik:RadTextBox>
                   </td>
                   </tr>
                   <tr>
                   <td class="SectionheaderTxt" style="width: 100%">
                     Lab Evaluation
                   </td>
                   </tr>
                   <tr>
                   <td style="width: 100%">
                     <table>
                     <tr>
                       <td>
                         Lab Evaluation:
                       </td>
                      <td>
                        <telerik:RadButton ID="radbtnLabEvaluationPeads" runat="server" Width="52px" GroupName="BirthBCG"
                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="No" />
                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                      </td>
                      <td>
                       If yes specify lab evaluation:
                      </td>
                      <td>
                        <telerik:RadComboBox ID="rcbSpecifyLabEvaluation" runat="server" Text="aSomeTest" AutoPostBack="false"
                                              Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                              Width="120px">
                                          </telerik:RadComboBox>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       Counselling:
                      </td>
                      <td>
                         <telerik:RadComboBox ID="rcbCounselling" runat="server" Text="aSomeTest" AutoPostBack="false"
                                              Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                              Width="120px">
                                          </telerik:RadComboBox>
                      </td>
                      <td>
                       Other Counselling:
                      </td>
                      <td>
                         <telerik:RadTextBox ID="txtOtherCounselling" runat="server" Width="150px">
                       </telerik:RadTextBox>
                      </td>
                     </tr>
                     </table>
                   </td>
                   </tr>
                   <tr>
                   <td class="SectionheaderTxt" style="width: 100%" >
                     Plan
                   </td>
                   </tr>
                   <tr>
                   <td  style="width: 100%">
                    <table>
                     <tr>
                      <td>
                       Admit to ward:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnWardAdmission" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                      Refer to specialist clinic(specify clinic):
                      </td>
                      <td>
                       <telerik:RadTextBox ID="txtReferToSpecialistClinic" runat="server" TextMode="MultiLine" Width="100%">
                       </telerik:RadTextBox>
                      </td>
                     </tr>
                     <tr>
                     <td>
                      Transfer to another facility (specify facility):
                     </td>
                     <td>
                      <telerik:RadTextBox ID="txtTransferOut" runat="server"  Width="100%">
                       </telerik:RadTextBox>
                     </td>
                     </tr>
                    </table>
                   </td>
                   </tr>
                   <tr>
                   <td class="SectionheaderTxt" style="width: 100%">
                    Treatment
                   </td>
                   </tr>
                   <tr>
                   <td style="width: 100%">
                   <table>
                    <tr>
                     <td>
                     ART treatment plan:
                     </td>
                     <td>
                         <telerik:RadComboBox ID="rcbARTTreatmentPlanPeads" runat="server" Text="aSomeTest" AutoPostBack="false"
                                              Skin="MetroTouch"  CheckedItemsTexts="FitInInput"
                                              Width="120px">
                                          </telerik:RadComboBox>
                     </td>
                     <td>
                     Specify reason for switching regimen:
                     </td>
                     <td>
                         <telerik:RadComboBox ID="rcbSwitchReason" runat="server" Text="aSomeTest" AutoPostBack="false"
                             Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                             Width="120px">
                         </telerik:RadComboBox>
                     </td>
                    </tr>

                    <tr>
                    <td>
                     Start ART?:
                    </td>
                    <td>
                        <telerik:RadButton ID="radbtnStartART" runat="server" Width="52px" GroupName="BirthBCG"
                            ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                            <ToggleStates>
                                <telerik:RadButtonToggleState Text="No" />
                                <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                            </ToggleStates>
                        </telerik:RadButton>
                    </td>
                    <td>
                     Eligible through:
                    </td>
                    <td>
                        <telerik:RadComboBox ID="rcbARTEligibilityCriteria" runat="server" Text="aSomeTest" AutoPostBack="false"
                            Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                            Width="120px">
                        </telerik:RadComboBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                      Specify other eligibility Criteria:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherARTEligibilityCriteria" runat="server" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                     Substitute Regimen?:
                    </td>
                     <td>
                         <telerik:RadButton ID="radbtnSubstituteRegimen" runat="server" Width="52px" GroupName="BirthBCG"
                             ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                             <ToggleStates>
                                 <telerik:RadButtonToggleState Text="No" />
                                 <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                             </ToggleStates>
                         </telerik:RadButton>
                     </td>

                    </tr>
                    <tr>
                     <td>
                       Number of drugs substituted:
                     </td>
                     <td>
                      <telerik:RadComboBox ID="rcbNumberDrugsSubstituted" runat="server" Text="aSomeTest" AutoPostBack="false"
                                              Skin="MetroTouch"  CheckedItemsTexts="FitInInput"
                                              Width="120px">
                                          </telerik:RadComboBox>
                     </td>
                     <td>
                      Stop treatment?:
                     </td>
                        <td>
                            <telerik:RadButton ID="radbtnStopTreatment" runat="server" Width="52px" GroupName="BirthBCG"
                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="No" />
                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                </ToggleStates>
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                     <td>
                      ART Stop reason:
                     </td>
                     <td>
                         <telerik:RadComboBox ID="rcbStopTreatmentCodes" runat="server" Text="aSomeTest" AutoPostBack="false"
                             Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                             Width="120px">
                         </telerik:RadComboBox>
                     </td>
                     <td>
                      Regimen prescribed:
                     </td>
                     <td>
                         <telerik:RadComboBox ID="rcbRegimenPrescribed" runat="server" Text="aSomeTest" AutoPostBack="false"
                             Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                         </telerik:RadComboBox>
                     </td>
                    </tr>
                    <tr>
                    <td>
                     Specify other regimen prescribed:
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOtherRegimenPrescribed" runat="server" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                    </tr>
                   </table>
                   </td>
                   </tr>
                   <tr>
                   <td class="SectionheaderTxt" style="width: 100%">
                     OI Treatment
                   </td> 
                   </tr>
                    <tr>
                    <td style="width: 100%">
                     <table>
                      <tr>
                       <td>
                        OI Prophylaxis:
                       </td>
                       <td>
                           <telerik:RadComboBox ID="rcbOIProphylaxis" runat="server" Text="aSomeTest" AutoPostBack="false"
                               Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                           </telerik:RadComboBox>
                       </td>
                       <td>
                        Cotrimoxazole prescribed for?:
                       </td>
                       <td>
                           <telerik:RadComboBox ID="rcbReasonCTXPrescribed" runat="server" Text="aSomeTest" AutoPostBack="false"
                               Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                           </telerik:RadComboBox>
                       </td>
                      </tr>
                      <tr>
                       <td>
                        Other Treatment:
                       </td>
                       <td colspan="3">
                           <telerik:RadTextBox ID="txtOtherTreatment" runat="server" Width="100%" TextMode="MultiLine">
                           </telerik:RadTextBox>
                       </td>
                      
                      </tr>
                     </table>
                    </td>
                   </tr>

                   
                  </table>
              </ContentTemplate>
          </asp:UpdatePanel>
      </div>
      <div id="tab6" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
          height: 380PX;">
          <asp:UpdatePanel ID="UpdatePanel51" runat="server" UpdateMode="Conditional">
              <contenttemplate>
                  <table id="TabPrevWithPos" cellpadding="10px" class="Section" >
                   <tr>
                   <td class="SectionheaderTxt" style="width: 100%">
                      Sexuallity Assessment
                   </td>
                   </tr>
                   <tr>
                    <td style="width: 100%">
                      <table>
                        <tr>
                        <td>
                         Have you been sexually active in the past 6 months?:
                        </td>
                        <td>
                        <telerik:RadButton ID="radbtnSexualActiveness" runat="server" Width="52px" GroupName="BirthBCG"
                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="No" />
                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                        </ToggleStates>
                                    </telerik:RadButton>        
                                          

                        </td>
                        <td>
                          Sexual orientation:
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbSexualOrientation" runat="server" Text="aSomeTest" AutoPostBack="false"
                                Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                            </telerik:RadComboBox>
                        </td>
                        </tr>
                        <tr>
                        <td>
                         Sexuality High risk factor:
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbHighRisk" runat="server" Text="aSomeTest" AutoPostBack="false"
                                Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                Width="120px">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                          Have you disclosed your status to sexual partner?:
                        </td>
                        <td>
                            <telerik:RadButton ID="radbtnKnowSexualPartnerHIVStatus" runat="server" Width="52px" GroupName="BirthBCG"
                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState Text="No" />
                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                </ToggleStates>
                            </telerik:RadButton>
                        </td>
                        </tr>
                        <tr>
                        <td>
                          HIV Status of sexual partner:
                        </td>
                        <td>
                            <telerik:RadComboBox ID="rcbPartnerHIVStatus" runat="server" Text="aSomeTest" AutoPostBack="false"
                                Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                            </telerik:RadComboBox>
                        </td>
                        </tr>
                      </table>
                    </td>
                   </tr>
                   <tr>
                   <td class="SectionheaderTxt" style="width: 100%">
                    PWP Interventions
                   </td>
                   </tr>
                   <tr>
                    <td style="width: 100%">
                     <table>
                     <tr>
                      <td>
                       PWP messages given:
                      </td>
                      <td>
                       <telerik:RadButton ID="radbtnGivenPWPMessages" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       Importance of safe sex explained:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnSaferSexImportanceExplained" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>

                     <tr>
                      <td>
                        Negative consequences of unsafe sex explained:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnUnsafeSexImportanceExplained" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       Pregnancy test done?:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnPDTDone" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       If yes, client pregnant?:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnPregnant" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       If yes for pregnant, has patient been offered or reffered to PMTCT?:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnPMTCTOffered" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       Patient or partner intend to become pregnant before next visit?:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnIntentionOfPregnancy" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       If yes, discussed fertility options:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnDiscussedFertilityOptions" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>
                     <tr>
                     <td>
                      If No discussed dual contraception:
                     </td>
                      <td>
                          <telerik:RadButton ID="radbtnDiscussedDualContraception" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       Condoms issued:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnCondomsIssued" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       Reasons condoms not issued:
                      </td>
                      <td>
                          <telerik:RadTextBox ID="txtCondomNotIssued" runat="server" Width="100%">
                          </telerik:RadTextBox>
                      </td>
                      <td>
                       Screened for STI?:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnSTIScreened" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       Urethral Discharge:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnUrethralDischarge" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       Vaginal discharge:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnVaginalDischarge" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       Genital Ulceration:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnGenitalUlceration" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                      STI Treatment Plan:
                      </td>
                      <td>
                          <telerik:RadTextBox ID="txtSTITreatmentPlan" runat="server"  TextMode="MultiLine" Width="250px">
                          </telerik:RadTextBox>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       Are you on any family planning method apart from condom?:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnOnFP" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       Specify FP method other than condoms:
                      </td>
                      <td>
                          <telerik:RadComboBox ID="rcbFPMethod" runat="server" Text="aSomeTest" AutoPostBack="false"
                              Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                          </telerik:RadComboBox>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       Have you been screened for cervical cancer:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnCervicalCancerScreened" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       Ca cervix screening results:
                      </td>
                      <td>
                          <telerik:RadComboBox ID="rcbCervicalCancerScreeningResults" runat="server" Text="aSomeTest" AutoPostBack="false"
                              Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                          </telerik:RadComboBox>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       If No, referred for cervical cancer screening:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnReferredForCervicalCancerScreening" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                      <td>
                       HPV offered?:
                      </td>
                      <td>
                          <telerik:RadButton ID="radbtnHPVOffered" runat="server" Width="52px" GroupName="BirthBCG"
                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                              <ToggleStates>
                                  <telerik:RadButtonToggleState Text="No" />
                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                              </ToggleStates>
                          </telerik:RadButton>
                      </td>
                     </tr>
                     <tr>
                      <td>
                       HPV Vaccine Offered:
                      </td>
                      <td>
                          <telerik:RadComboBox ID="rcbOfferedHPVVaccine" runat="server" Text="aSomeTest" AutoPostBack="false"
                              Skin="MetroTouch" CheckedItemsTexts="FitInInput" Width="120px">
                          </telerik:RadComboBox>

                      </td>
                      <td>
                       Date of HPV vaccine:
                      </td>
                      <td>
                        <telerik:RadDatePicker ID="dtHPVDoseDate" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar29" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                  UseRowHeadersAsSelectors="False">
                                              </Calendar>
                                              <DateInput ID="DateInput29" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                      
                     </tr>
                     <tr>
                      <td>
                       Patient referred to:
                      </td>
                      <td>
                          <telerik:RadComboBox ID="rcbRefferedToFupF" runat="server" Text="aSomeTest" AutoPostBack="false"
                              Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                              Width="120px">
                          </telerik:RadComboBox>
                      </td>
                      <td>
                       Specify other referral point:
                      </td>
                      <td>
                          <telerik:RadTextBox ID="txtSpecifyOtherRefferedTo" runat="server" Width="100%">
                          </telerik:RadTextBox>
                      </td>
                     </tr>
                     </table>
                    </td>
                   </tr>
                  </table>
               </contenttemplate>
            </asp:UpdatePanel>
        </div>


</div>
</div>

<KNHuc:KNHModal ID="TheModal" runat="server" />


<div id="frmAdultIE_ScriptBlock" runat="server" style="display: none;">
<script type="text/javascript" language="javascript">
</script>
</div>
  <script type="text/javascript" language="javascript">
    
       function openWinVitalSign() {


           //            var oWnd = radopen("KNH/KNHVitalSignShowModal.aspx?Qry=IDKNHAdultIEvaluationForm-VITALSIGN", "RadWindow1");
           var oWnd = $find("rwVital");
           oWnd.show();
           return false;

       

       }
       function openWinPresentingComplaints() {


           var oWnd = radopen("KNH/KNHPresentingComplaintsModal.aspx?Qry=IDKNHAdultIEvaluationForm-PC", "RadWindow2");
          
       }

       function openWinMedicalHistory() {


           var oWnd = radopen("KNH/KNHMedicalHistoryModal.aspx?Qry=IDKNHAdultIEvaluationForm-MH", "RadWindow3");
           
       }
       function openWinWhoStaging() {


           var oWnd = radopen("KNH/KNHWhoStagingModal.aspx?Qry=IDKNHAdultIEvaluationForm-Staging", "RadWindow4");
          
       }
       function openWinPhyExam() {


           var oWnd = radopen("KNH/KNHPhysicalExaminationModal.aspx?Qry=IDKNHAdultIEvaluationForm-PhyExam", "RadWindow5");
           
       }

       
      



   </script>
