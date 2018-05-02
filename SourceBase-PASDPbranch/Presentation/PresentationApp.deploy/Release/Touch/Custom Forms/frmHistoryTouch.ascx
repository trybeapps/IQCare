<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.Custom_Forms.frmHistoryTouch" Codebehind="frmHistoryTouch.ascx.cs" %>


<div id="FormContent">
     <div id="tabs">
        <ul>
        <li><a href="#tab1">Mother's History</a>&nbsp; </li>
        <li><a href="#tab2">HIV Care</a></li>
        <li><a href="#tab3">Transfer In</a></li>
        <li><a href="#tab4">Prior ART</a></li>
        <li><a href="#tab5">Drug Allergies</a></li>
        </ul>

<!-- TAB 1 -->
        <div id="tab1" class="scroll-pane jspScrollable" style="width:920px; overflow:hidden; height: 380px;">
            <table id="referrals" style="width:870px;" cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>Mother&#39;s ART/PMTCT History</div>
                    </td>
                </tr>
            </table>
             <table style="width:870px;" cellpadding="10px" class="Section" >
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
                <table style="width:870px;" cellpadding="10px"   class="Section" >
                <tr>
                <td style="width:23%;">
                    Select drug combination</td>
                <td style="width:78%;">
                   Modal here       
                </td>
                </tr>
               </table>
           </div>
           <table style="width:870px;" cellpadding="10px"   class="Section" >
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
                <table style="width:870px;" cellpadding="10px"   class="Section" >
                <tr>
                <td style="width:23%;">
                    Select drug combination</td>
                <td style="width:78%;">
                   Modal here       
                </td>
                </tr>
               </table>
           </div>
           <table style="width:870px;" cellpadding="10px"   class="Section" >
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

<!-- TAB 2 -->
            <div id="tab2" class="scroll-pane jspScrollable" style="width:920px; overflow:hidden; height: 380px;">
            <table style="width:870px;" cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>HIV Care</div>
                    </td>
                </tr>
            </table>
            <table style="width:870px;" cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        Date confirmed HIV positive</td>
                    <td style="width:28%;">
                        <telerik:RadDatePicker ID="dtConfirmedPos" runat="server" Skin="MetroTouch">
                            <Calendar ID="Calendar1" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput1" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy" LabelWidth="0px" runat="server">
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
                            <Calendar ID="Calendar2" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput2" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy" LabelWidth="0px" runat="server">
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
                                <telerik:RadComboBoxItem runat="server" Text="2" 
                                    Value="2" />
                                <telerik:RadComboBoxItem runat="server" Text="3" Value="3" />
                                <telerik:RadComboBoxItem runat="server" Text="4" Value="4" />
                                <telerik:RadComboBoxItem runat="server" Text="T1" Value="T1" />
                                <telerik:RadComboBoxItem runat="server" Text="T2" Value="T2" />
                                <telerik:RadComboBoxItem runat="server" Text="T3" Value="T3" />
                                <telerik:RadComboBoxItem runat="server" Text="T4" Value="T4" />
                                <telerik:RadComboBoxItem runat="server" Text="NA" Value="NA" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                  </tr>
                  </table>
            </div>

<!-- TAB 3 -->
            <div id="tab3" style="width:920px; overflow:hidden; height: 380px;">
            <table style="width:870px;" cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>Tranfer In</div>
                    </td>
                </tr>
            </table>
            <table style="width:870px;" cellpadding="10px" class="Section" >
                <tr>
                    <td style="width:23%;">
                        Transfer In Date</td>
                    <td style="width:28%;">
                        <telerik:RadDatePicker ID="dtTransferIn" runat="server" Skin="MetroTouch">
                            <Calendar ID="Calendar3" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput3" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy" LabelWidth="0px" runat="server">
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
                         <telerik:RadComboBox ID="RadComboBox2" runat="server" Skin="MetroTouch">
                             <Items>
                                 <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 1" Value="District 1" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 2" Value="District 2" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 3" Value="District 3" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 4" Value="District 4" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 5" Value="District 5" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 6" Value="District 6" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 7" Value="District 7" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 8" Value="District 8" />
                                 <telerik:RadComboBoxItem runat="server" Text="District 9" Value="District 9" />
                             </Items>
                         </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:23%;">
                        Facility</td>
                    <td style="width:28%;">
                         <telerik:RadComboBox ID="RadComboBox1" runat="server" Skin="MetroTouch">
                             <Items>
                                 <telerik:RadComboBoxItem runat="server" Text="Select" Value="Select" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 1" Value="Facility 1" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 2" Value="Facility 2" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 3" Value="Facility 3" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 4" Value="Facility 4" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 5" Value="Facility 5" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 6" Value="Facility 6" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 7" Value="Facility 7" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 8" Value="Facility 8" />
                                 <telerik:RadComboBoxItem runat="server" Text="Facility 9" Value="Facility 9" />
                             </Items>
                         </telerik:RadComboBox>
                    </td>
                    <td style="width:23%;">
                        Date Started on 1st Line Regimen</td>
                    <td style="width:28%;">
                        <telerik:RadDatePicker ID="dtTransferIn0" runat="server" Skin="MetroTouch">
                            <Calendar ID="Calendar4" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" 
                                                Skin="MetroTouch" runat="server"></Calendar>
                            <DateInput ID="DateInput4" DisplayDateFormat="dd MMM yyyy" 
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
                        Modal here</td>
                  </tr>
                  <tr>
                    <td style="width:23%;">
                        Weight (Kgs)</td>
                    <td style="width:28%;">
                        <telerik:RadTextBox ID="txtWeight" runat="server" Skin="MetroTouch" 
                            Width="200px" >
                        </telerik:RadTextBox>
                    </td>
                    <td style="width:23%;">
                        Height (cms)</td>
                    <td style="width:28%;">
                        <telerik:RadTextBox ID="txtHeight" runat="server" Skin="MetroTouch" 
                             Width="200px" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width:23%;">
                        BMI (Adults)</td>
                    <td style="width:28%;">
                        <telerik:RadTextBox ID="txtBMICalculated" runat="server" Skin="MetroTouch" 
                             Width="200px" >
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
                                <telerik:RadComboBoxItem runat="server" Text="T1" Value="T1" />
                                <telerik:RadComboBoxItem runat="server" Text="T2" Value="T2" />
                                <telerik:RadComboBoxItem runat="server" Text="T3" Value="T3" />
                                <telerik:RadComboBoxItem runat="server" Text="T4" Value="T4" />
                                <telerik:RadComboBoxItem runat="server" Text="NA" Value="NA" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                  </tr>
                  </table>

            </div>

<!-- TAB 4 -->
            <div id="tab4" class="scroll-pane jspScrollable" style="width:920px; overflow:hidden; height: 380px;">
            <table style="width:870px;" cellpadding="10px" class="Section" >
                <tr>
                    <td class="SectionheaderTxt">
                        <div>Prior ART (excluding PMTCT)</div>
                    </td>
                </tr>
            </table>
            <table style="width:870px;" cellpadding="10px" class="Section" >
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
                <table style="width:870px;" cellpadding="10px"   class="Section" >
                <tr>
                    <td colspan="4"  style="width:100%">
                        <telerik:RadGrid ID="rgPriorART" runat="server" AutoGenerateColumns="False" CellSpacing="0" GridLines="None" 
    AllowPaging="True" AllowSorting="True">
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

<!-- TAB 5 -->
            <div id="tab5">

            <table id="Table2" style="width:100%;" cellpadding="10px" class="Section" >
                <tr>
                    <td colspan="4" class="SectionheaderTxt">
                        <div>Drug Allergies</div>
                    </td>
                </tr>
                </table>
                <table style="width:870px;" cellpadding="10px"   class="Section" >
                <tr>
                    <td colspan="4"  style="width:100%">
                        <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="False" 
                            CellSpacing="0" GridLines="None">
                            <MasterTableView>
                                <Columns>
                                    <telerik:GridAttachmentColumn FileName="attachment" 
                                        FilterControlAltText="Filter Regimen column" UniqueName="Regimen" 
                                        HeaderText="Allergen">
                                    </telerik:GridAttachmentColumn>
                                    <telerik:GridAttachmentColumn FileName="attachment" 
                                        FilterControlAltText="Filter Date column" UniqueName="Date" 
                                        HeaderText="Type of Reaction">
                                    </telerik:GridAttachmentColumn>
                                    <telerik:GridAttachmentColumn FileName="attachment" 
                                        FilterControlAltText="Filter column column" HeaderText="Date" 
                                        UniqueName="column">
                                    </telerik:GridAttachmentColumn>
                                    <telerik:GridAttachmentColumn FileName="attachment" 
                                        FilterControlAltText="Filter column1 column" 
                                        HeaderText="Relevant Medical Conditions" UniqueName="column1">
                                    </telerik:GridAttachmentColumn>
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
