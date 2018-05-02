<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="test.ascx.cs" Inherits="Touch.Custom_Forms.test" %>

<div id="FormContent">

<asp:HiddenField ID="hidtab" Value="0" runat="server" />

<div id="tabs" style="width:800px">
        <ul>
        <li><a href="#tab1">Patient Info</a></li>
        <li><a href="#tab2">Caregiver Info</a></li>
        <li><a href="#tab4">Mother's History</a>&nbsp; </li>
        <li><a href="#tab5">HIV Care</a></li>
        <li><a href="#tab6">Transfer In</a></li>
        <li><a href="#tab8">Drug Allergies</a></li>
        </ul>
        
        <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width:811px; overflow:hidden; height: 380px;">
        <asp:UpdatePanel ID="uptTab1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <table id="PatientIdentifiers" cellpadding="10px" class="Section" >
                    <tr>
                        <td class="SectionheaderTxt">
                            <div>Patient Identification</div>
                        </td>
                    </tr>
                </table>

                <table cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        First 
                        name *</td>
                    <td style="width:27%;">
                        <telerik:RadTextBox ID="txtFirstName" runat="server" BackColor="#FFFFCC" Skin="MetroTouch" Width="200px" >
                            <ClientEvents OnBlur="OnBlur" />
                        </telerik:RadTextBox>
                    </td>
                    <td style="width:23%;">
                        Last name *</td>
                    <td style="width:27%;">
                            <telerik:RadTextBox ID="txtLastName" runat="server" BackColor="#FFFFCC" Skin="MetroTouch" Width="200px" >
                            <ClientEvents OnBlur="OnBlur" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:23%;">
                        Birth Date *</td>
                    <td style="width:27%;">
                        <telerik:RadDatePicker ID="dtPatientDOB" runat="server" Skin="MetroTouch" AutoPostBack="true" MinDate="01/01/1800" OnSelectedDateChanged="dtPatientDOB_SelectedDateChanged" >
                            <ClientEvents OnDateSelected="OnBlurDateP" />
                            <Calendar ID="Calendar1" runat="server" Skin="MetroTouch" 
                                UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
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
                    <td style="width:23%;">
                        Age</td>
                      
                    <td style="width:27%;"><table>
                    <tr>
                        <td style="font-size:12px">Years</td>
                        <td><telerik:RadTextBox ID="AgeYearZZZ" runat="server" Skin="MetroTouch" Width="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td style="font-size:12px">Months</td>
                        <td><telerik:RadTextBox ID="AgeMonthZZZ" runat="server" Skin="MetroTouch" Width="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnCalCDOB" runat="server" Skin="MetroTouch" Text="Cal DOB" OnClick="btnCalCDOB_Click">
                        </telerik:RadButton>
                    </td>
                    </tr>
                    </table></td>
                    </tr>
                    <tr>
                    <td style="width:23%;">
                        Sex *</td>
                    <td style="width:27%;">
                        <telerik:RadComboBox ID="cbSex" runat="server" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Male" Value="16" />
                                <telerik:RadComboBoxItem runat="server" Text="Female" Value="17" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width:23%;">
                        Registration<br /> Date *</td>
                    <td style="width:27%;">
                        <telerik:RadDatePicker ID="dtRegistrationDate" runat="server" Skin="MetroTouch">
                            <ClientEvents OnDateSelected="OnBlurDateP" />
                            <Calendar ID="Calendar2" runat="server" Skin="MetroTouch" 
                                UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
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

                <table cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>Patient Physical Address</div>
                    </td>
                </tr>
                </table>
                <table cellpadding="10px" class="Section" >
                    <tr>
                        <td style="width:20%;">
                            Street - house number</td>
                        <td style="width:30%;">
                            <telerik:RadTextBox ID="txtAddress" runat="server" Skin="MetroTouch" Width="200px" >
                            </telerik:RadTextBox>
                        </td>
                        <td style="width:20%;">
                            Suburb / Village / Farm</td>
                        <td style="width:30%;">
                            <telerik:RadTextBox ID="txtVillage" runat="server" Skin="MetroTouch" Width="200px" >
                            </telerik:RadTextBox>
                            <%-- <telerik:RadComboBox ID="cbDistrict" runat="server" Skin="MetroTouch">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                    <telerik:RadComboBoxItem runat="server" Text="Village 1" Value="Village 1" />
                                    <telerik:RadComboBoxItem runat="server" Text="Village 2" Value="Village 2" />
                                    <telerik:RadComboBoxItem runat="server" Text="Village 3" Value="Village 3" />
                                    <telerik:RadComboBoxItem runat="server" Text="Village 4" Value="Village 4" />
                                </Items>
                            </telerik:RadComboBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:23%;">
                            District</td>
                        <td style="width:27%;">
                            <telerik:RadComboBox ID="rcbDistrict" runat="server" Skin="MetroTouch"  Width="200px">
                            </telerik:RadComboBox>
                        </td>
                        <td style="width:23%;">
                            Sub district</td>
                        <td style="width:27%;">
                            <telerik:RadComboBox ID="rcbSubDistrict" runat="server" Skin="MetroTouch" Width="200px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:23%;">
                            Phone number</td>
                        <td style="width:27%;">
                            <telerik:RadMaskedTextBox ID="txtPatientPhoneNo" runat="server" Width="200px" Mask="(###) ###-####" Skin="MetroTouch">
                                </telerik:RadMaskedTextBox>
                        </td>
                        <td style="width:23%;">
                            Address comment</td>
                        <td style="width:27%;">
                            <telerik:RadMaskedTextBox ID="txtAddressComment" runat="server" Width="200px" Skin="MetroTouch">
                                </telerik:RadMaskedTextBox>
                        </td>
                    </tr>
                </table>
                <table cellpadding="10px" class="Section" >
                    <tr>
                        <td class="SectionheaderTxt">
                            <div>Patient Postal Address</div>
                        </td>
                    </tr>
                    </table>
                <table cellpadding="10px" class="Section" >
                    <tr>
                        <td style="width:23%;vertical-align:text-top">
                            Postal Address</td>
                        <td style="width:27%;">
                            <telerik:RadTextBox ID="RadTextBox1" TextMode="MultiLine" Height="75px" runat="server" Skin="MetroTouch" Width="200px" >
                            </telerik:RadTextBox>
                        </td>
                        <td style="width:23%;vertical-align:text-top">
                            Postal code</td>
                        <td style="width:27%;vertical-align:text-top">
                            <telerik:RadTextBox ID="RadTextBox2" runat="server" Skin="MetroTouch" Width="200px" >
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>

                <table cellpadding="10px" class="Section">
                        <tr>
                            <td class="SectionheaderTxt">
                                <div>Patient Source</div>
                            </td>
                        </tr>
                </table>
                <table cellpadding="10px" class="Section">
                        <tr>
                            <td style="width:23%;">
                                Entry Point</td>
                            <td style="width:77%;">
                                <telerik:RadComboBox ID="cbEntryPoint" runat="server" AutoPostBack="false" OnClientSelectedIndexChanged="ShowMoreMulti" Skin="MetroTouch"  Width="300px">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                        <telerik:RadComboBoxItem runat="server" Text="PMTCT" Value="PMTCT" />
                                        <telerik:RadComboBoxItem runat="server" Text="VCT" Value="VCT" />
                                        <telerik:RadComboBoxItem runat="server" Text="TB Clinic" Value="TB Clinic" />
                                        <telerik:RadComboBoxItem runat="server" Text="OPD" Value="OPD" />
                                        <telerik:RadComboBoxItem runat="server" Text="IPD-Paediatric" 
                                            Value="IPD-Paediatric" />
                                        <telerik:RadComboBoxItem runat="server" Text="MCH-child" Value="MCH-child" />
                                        <telerik:RadComboBoxItem runat="server" Text="Closure of clinic" 
                                            Value="Closure of clinic" />
                                        <telerik:RadComboBoxItem runat="server" Text="Other" Value="Other|ShowIfEntryOther" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        </table>
                <div id="wrapper_cbEntryPoint">
                <div id="ShowIfEntryOther" style="display:none">
                    <table cellpadding="10px"   class="Section" >
                        <tr>
                            <td style="width:23%;">
                                Other entry point</td>
                            <td style="width:77%;">
                                <telerik:RadTextBox ID="txtEntryPointOther" runat="server" Skin="MetroTouch">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    </div>
                </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        </div>

        
        <div id="tab2" class="scroll-pane jspScrollable tabwidth" style="width:811px; overflow:hidden; height: 380px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <table cellpadding="10px"   class="Section">
                <tr>
                    <td  colspan="4" class="SectionheaderTxt">
                        <div>Caregiver Contact</div>
                    </td>
                </tr>
                <tr>
                    <td style="width:23%;">
                        <span>Caregiver first name</span></td>
                    <td style="width:27%;">
                        <telerik:RadTextBox ID="txtCGFirstName" runat="server" Skin="MetroTouch" Width="200px" >
                        </telerik:RadTextBox>
                    </td>
                    <td style="width:23%;">
                        Caregiver last name</td>
                    <td style="width:27%;">
                         <telerik:RadTextBox ID="txtCGLastName" runat="server" Skin="MetroTouch" Width="200px" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                 <td style="width:23%;">
                        Caregiver DOB</td>
                    <td style="width:27%;">
                        <telerik:RadDatePicker ID="dtCGDOB" runat="server" Skin="MetroTouch" MinDate="01/01/1800" AutoPostBack="true" OnSelectedDateChanged="dtCGDOB_SelectedDateChanged">
                            <Calendar ID="Calendar3" runat="server" Skin="MetroTouch" 
                                UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                            </Calendar>
                            <DateInput ID="DateInput3" runat="server" DateFormat="dd/MM/yyyy" 
                                DisplayDateFormat="dd-MMM-yyyy" LabelWidth="0px">
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
                    <td style="width:23%;">
                        Age</td>
                   <td style="width:27%;"><table>
                    <tr>
                        <td>Year</td>
                       <td><telerik:RadTextBox ID="txtCGYearZZZ" runat="server" Skin="MetroTouch" Width="30px">
                        </telerik:RadTextBox>
                    </td>
                    <td>Month</td>
                     <td><telerik:RadTextBox ID="txtCGMonthZZZ" runat="server" Skin="MetroTouch" Width="30px">
                        </telerik:RadTextBox>
                    </td>
                      
                    <td>
                        <telerik:RadButton ID="CGCalDOB" runat="server" Skin="MetroTouch" Text="Cal DOB" OnClick="CGCalDOB_Click">
                        </telerik:RadButton>
                    </td>
                    </tr>
                    </table></td>
                    </tr>
                    <tr>
                    <td style="width:23%;">
                        Caregiver sex</td>
                    <td style="width:27%;">
                        <telerik:RadComboBox ID="cbCGSex" runat="server" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Male" Value="Male" />
                                <telerik:RadComboBoxItem runat="server" Text="Female" Value="Female" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width:23%;">
                        Caregiver relationship</td>
                    <td style="width:27%;">
                        <telerik:RadComboBox ID="cbRelationship" runat="server" OnClientSelectedIndexChanged="ShowMoreSingle" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Mother" Value="Mother" />
                                <telerik:RadComboBoxItem runat="server" Text="Father" Value="Father" />
                                <telerik:RadComboBoxItem runat="server" Text="Grandmother" 
                                    Value="Grandmother" />
                                <telerik:RadComboBoxItem runat="server" Text="Grandfather" 
                                    Value="Grandfather" />
                                <telerik:RadComboBoxItem runat="server" Text="Aunt" Value="Aunt" />
                                <telerik:RadComboBoxItem runat="server" Text="Uncle" Value="Uncle" />
                                <telerik:RadComboBoxItem runat="server" Text="Sister" Value="Sister" />
                                <telerik:RadComboBoxItem runat="server" Text="Brother" Value="Brother" />
                                <telerik:RadComboBoxItem runat="server" Text="Other" Value="Other|ShowIfOtherRelationship" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                </table>

                <div id="ShowIfOtherRelationship" style="display:none">
                <table cellpadding="10px" class="Section" >
                      <tr>
                         <td style="width:23%;">
                             Other relationship</td>
                         <td style="width:27%;">
                              <telerik:RadTextBox ID="txtOtherRelationship" runat="server" Skin="MetroTouch" Width="200px">
                              </telerik:RadTextBox>
                         </td>
                         <td style="width:23%;"> &nbsp</td>
                         <td style="width:27%;"> &nbsp</td>
                     </tr>
                    </table>
                  </div>
                
                <table cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        Caregiver phone no:</td>
                    <td style="width:27%;">
                    <telerik:RadMaskedTextBox ID="txtCGPhoneNo" runat="server" Width="200px" Mask="(###) ###-####" Skin="MetroTouch">
                            </telerik:RadMaskedTextBox>
                    </td>
                    <td style="width:23%;"> &nbsp</td>
                         <td style="width:27%;"> &nbsp</td>
                </tr>
                </table>
        </ContentTemplate>
        </asp:UpdatePanel> 
        </div>

        <div id="tab4" class="scroll-pane jspScrollable tabwidth" style="width:811px; overflow:hidden; height: 380px;">
            <table id="referrals" cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>Mother&#39;s ART/PMTCT History</div>
                    </td>
                </tr>
            </table>
             <table cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        Name of Mother</td>
                    <td style="width:28%;">
                        <telerik:RadTextBox ID="MotherName" runat="server" Skin="MetroTouch" Width="200px" >
                        </telerik:RadTextBox>
                    </td>
                    <td style="width:23%;">
                        Alive?</td>
                    <td style="width:28%;">
                         <telerik:RadButton ID="rbtnMotherAliveYes" runat="server" ToggleType="Radio" Skin="MetroTouch" GroupName="MotherAlive"
                            Text="Yes">
                        </telerik:RadButton> 
                        &nbsp;
                        <telerik:RadButton ID="rbtnMotherAliveNo" runat="server" ToggleType="Radio" Skin="MetroTouch" GroupName="MotherAlive"
                            Text="No">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td style="width:23%;">
                        Mother received drugs for PMTCT?</td>
                    <td style="width:28%;">
                        <telerik:RadComboBox ID="cbMotherReceived" runat="server" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="Yes" />
                                <telerik:RadComboBoxItem runat="server" Text="No" Value="No" />
                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="Unknown" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                  </tr>
                  </table>
            <div id="ShowIfMotherRecievedYes" style="display:none;">
                <table cellpadding="10px"   class="Section" >
                <tr>
                <td style="width:23%;">
                    Select drug combination</td>
                <td style="width:78%;">
                   Modal here       
                </td>
                </tr>
               </table>
           </div>
           <table cellpadding="10px"   class="Section" >
                <tr>
                <td style="width:23%;">
                    Has the child received drugs for PMTCT?</td>
                <td style="width:78%;">
                        <telerik:RadComboBox ID="cbChildReceived" runat="server" EnableViewState="true" AutoPostBack="false" OnClientSelectedIndexChanged="drugcombo"  Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="Yes" />
                                <telerik:RadComboBoxItem runat="server" Text="No" 
                                    Value="No" />
                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="Unknown" />
                            </Items>
                        </telerik:RadComboBox>
                </td>
                </tr>
               </table>
               <div id="ShowIfChildReceivedYes" style="display:none">
                <table cellpadding="10px"   class="Section" >
                <tr>
                <td style="width:23%;">
                    Select drug combination</td>
                <td style="width:78%;">
                   Modal here       
                </td>
                </tr>
               </table>
           </div>
           <table cellpadding="10px"   class="Section" >
                <tr>
                <td style="width:23%;">
                    Is mother on ART?</td>
                <td style="width:78%;">
                        <telerik:RadComboBox ID="cbMotherOnART" runat="server" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="Yes" />
                                <telerik:RadComboBoxItem runat="server" Text="No" 
                                    Value="No" />
                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="Unknown" />
                            </Items>
                        </telerik:RadComboBox>
                </td>
                </tr>
                <tr>
                <td style="width:23%;">
                    Feeding option up to point of failure or 6 months - which ever is earlier</td>
                <td style="width:78%;">
                        <telerik:RadComboBox ID="cbFeedingOption" runat="server" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Exclusive breast " 
                                    Value="Exclusive breast " />
                                <telerik:RadComboBoxItem runat="server" Text="Mixed feeding" 
                                    Value="Mixed feeding" />
                                <telerik:RadComboBoxItem runat="server" Text="Exclusive formula" 
                                    Value="Exclusive formula" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
               </table>
            </div>

        <div id="tab5" class="scroll-pane jspScrollable tabwidth" style="width:811px; overflow:hidden; height: 380px;">
            <table cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>HIV Care</div>
                    </td>
                </tr>
            </table>
            <table cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        Date confirmed HIV positive</td>
                    <td style="width:28%;">
                        <telerik:RadDatePicker ID="dtConfirmedPos" runat="server" Skin="MetroTouch">
                            <Calendar ID="Calendar4" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput4" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy" LabelWidth="0px" runat="server">
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
                    <td style="width:23%;">
                        Date enrolled in HIV Care</td>
                    <td style="width:28%;">
                         <telerik:RadDatePicker ID="dtEnrolledHIVCare" runat="server" Skin="MetroTouch">
                            <Calendar ID="Calendar5" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput5" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy" LabelWidth="0px" runat="server">
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
                    <td style="width:23%;">
                        WHO Stage at enrollment</td>
                    <td style="width:28%;">
                        <telerik:RadComboBox ID="cbWHOEnrollment" runat="server" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="1" Value="1" />
                                <telerik:RadComboBoxItem runat="server" Text="2" Value="2" />
                                <telerik:RadComboBoxItem runat="server" Text="3" Value="3" />
                                <telerik:RadComboBoxItem runat="server" Text="4" Value="4" />
                                <telerik:RadComboBoxItem runat="server" Text="NA" Value="NA" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                  </tr>
                  </table>
            </div>

        <div id="tab6" class="scroll-pane jspScrollable tabwidth" style="width:811px; overflow:hidden; height: 380px;">
        <table cellpadding="10px" class="Section" >
            <tr>
                <td class="SectionheaderTxt">
                    <div>Transfer In</div>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtWeight" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtHeight" EventName="TextChanged" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        Transfer In Date</td>
                    <td style="width:28%;">
                        <telerik:RadDatePicker ID="dtTransferIn" runat="server" Skin="MetroTouch">
                            <Calendar ID="Calendar6" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput6" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy" LabelWidth="0px" runat="server">
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
                    <td style="width:23%;">
                        From District</td>
                    <td style="width:28%;">
                         <telerik:RadComboBox ID="rcbSubDistrictTransferIn" runat="server" Skin="MetroTouch">
                         </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:23%;">
                        Facility</td>
                    <td style="width:28%;">
                         <telerik:RadComboBox ID="rcbFacilityIn" runat="server" Skin="MetroTouch">
                         </telerik:RadComboBox>
                    </td>
                    <td style="width:23%;">
                        Date Started on 1st Line Regimen</td>
                    <td style="width:28%;">
                        <telerik:RadDatePicker ID="dtTransferIn0" runat="server" Skin="MetroTouch">
                            <Calendar ID="Calendar7" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput7" DisplayDateFormat="dd MMM yyyy" 
                                DateFormat="dd/MM/yyyy" LabelWidth="0px" runat="server">
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
                    <td style="width:23%;">
                        Regimen</td>
                    <td style="width:28%;">
                        <telerik:RadButton id="btnRegimen" runat="server" AutoPostBack="false" Text="Select Regimen"></telerik:RadButton> </td>
                  </tr>
                  <tr>
                    <td style="width:23%;">
                        Weight (Kgs)</td>
                    <td style="width:28%;">
                        <telerik:RadTextBox ID="txtWeight" runat="server" Skin="MetroTouch"  AutoPostBack="true" OnTextChanged="txtWeight_TextChanged" 
                            Width="200px" >
                        </telerik:RadTextBox>
                    </td>
                    <td style="width:23%;">
                        Height (cms)</td>
                    <td style="width:28%;">
                        <telerik:RadTextBox ID="txtHeight" runat="server" Skin="MetroTouch" AutoPostBack="true" OnTextChanged="txtHeight_TextChanged"
                             Width="200px" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:23%;">
                        BMI (Adults)</td>
                    <td style="width:28%;">
                        <telerik:RadTextBox ID="txtBMICalculated" runat="server" Skin="MetroTouch" 
                             Width="200px" enabled=" false">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width:23%;">
                        WHO Stage</td>
                    <td style="width:28%;">
                        <telerik:RadComboBox ID="cbWHOStage" runat="server" Skin="MetroTouch">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="1" Value="1" />
                                <telerik:RadComboBoxItem runat="server" Text="2" 
                                    Value="2" />
                                <telerik:RadComboBoxItem runat="server" Text="3" Value="3" />
                                <telerik:RadComboBoxItem runat="server" Text="4" Value="4" />
                                <telerik:RadComboBoxItem runat="server" Text="NA" Value="NA" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                  </tr>
                  </table>
        </ContentTemplate>
        </asp:UpdatePanel>

                  <table cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>Prior ART (excluding PMTCT)</div>
                    </td>
                </tr>
            </table>
            <table cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        Prior ART</td>
                    <td style="width:78%;">
                        <telerik:RadComboBox ID="cbPriorART" runat="server" Skin="MetroTouch"  OnClientSelectedIndexChanged="ShowMoreMulti" AutoPostBack="false">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                <telerik:RadComboBoxItem runat="server" Text="Yes" Value="Yes|ShowIfPriorARTYes" />
                                <telerik:RadComboBoxItem runat="server" Text="No" 
                                    Value="No" />
                                <telerik:RadComboBoxItem runat="server" Text="Unknown" Value="Unknown" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                  </tr>
                </table>
        <div id ="wrapper_cbPriorART">
            <div id="ShowIfPriorARTYes" style="display:none">
                <table cellpadding="10px"   class="Section" >
                <tr>
                    <td  style="width:100%">
                        <telerik:RadGrid ID="rgPriorART" runat="server" AutoGenerateColumns="False" CellSpacing="0" GridLines="None">
                            <MasterTableView>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Regimen" 
                                    FilterControlAltText="Filter Regimen column" HeaderText="Regimen" 
                                    UniqueName="Regimen" DataType="System.String" />
                                    <telerik:GridBoundColumn DataField="Date" 
                                    FilterControlAltText="Filter Date column" HeaderText="Date" 
                                    UniqueName="Date" DataType="System.String" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
               </table>
            </div>
        </div>
            </div>
</div>
</div>