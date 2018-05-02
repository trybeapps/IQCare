<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KNHModal.ascx.cs" Inherits="Touch.TestModal" %>
 <telerik:radwindow runat="server" id="rwVital" Title="Vital Sign" Skin="MetroTouch" Width="880px" VisibleOnPageLoad="false"  Height="450px" Behaviors="Move,Close" >
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="updtVital" runat="server" UpdateMode="Conditional">
                <br />
               


                <table id="VitalSign" width="100%" class="AdultIE">
                  <tr>
                     <td >
                     <br />
                    

                         <table width="100%" cellspacing="0" cellpadding="0">
                             <tr>
                                 <td>
                                     Temperature (Celsius):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadTemperatureModal" runat="server"  >
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     RR (Bpm):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadRespirationRate" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td>
                                     Heart Rate (Bpm):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadHeartRate" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     Systollic Blood <br />Pressure mmHg:
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadSystollicBloodPressure" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td>
                                     Diastolic Blood <br /> Pressure mmHg:
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadDiastolicBloodPressure" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     Height (cms):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadHeight" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td>
                                     Weight (kgs):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadWeight" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     BMI:
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadBMI" runat="server"  Enabled="false" Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                              <tr>
                               <td colspan="4" >
                                <asp:Panel ID="pnlContolsPediatric" runat="server" Width="100%" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                Head Circumference :
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtRadHeadCircumference" runat="server" Skin="MetroTouch">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                Weight for age:
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbWeightForAge" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                                    Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Weight for height :
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbWeightforHeight" runat="server" EmptyMessage="Select"
                                                    AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                Nurses Comments :
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtRadNursesComments" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                                    Width="250px">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                
                               </td>
                              </tr>

                            
                         </table>

                     </td>

                    </tr>
                   <tr>
                   <td colspan="4" align="center">
                   <br />
                   <br />
                   <br />

                    <telerik:RadButton ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" Skin="MetroTouch"></telerik:RadButton>
                   </td>
                   </tr>
                    
                   
                    

                </table>

                </telerik:RadAjaxPanel>
            </ContentTemplate>

     </telerik:radwindow>
<telerik:RadWindow runat="server" ID="rwPresentingComplaints" Title="Presenting Complaints" Skin="MetroTouch"
    Width="880px" VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close">
    <ContentTemplate>
        <telerik:RadAjaxPanel ID="updtpresentingcomp" runat="server" UpdateMode="Conditional">
            <br />

              <table id="Presenting Complaints"  width="100%" class="AdultIE">
      <tr>
       <td colspan="2">
           <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridPresenting" runat="server"
               Width="100%" PageSize="10" AllowPaging="false" AllowMultiRowSelection="false"
               ClientSettings-Selecting-AllowRowSelect="true" ShowFooter="false" Skin="MetroTouch"
               ShowHeader="False" OnItemDataBound="RadGridPresenting_ItemDataBound">
               <PagerStyle Mode="NextPrevAndNumeric" />
               <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                   CellSpacing="0" CellPadding="0">
                   <Columns>
                       <telerik:GridTemplateColumn>
                           <HeaderStyle Width="150px" />
                           <ItemTemplate>
                               <asp:Label ID="lblPresenting" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                               <asp:Label ID="lblchkval" runat="server" Text='<%# Eval("ChkVal") %>' Visible="false"></asp:Label>
                               <asp:CheckBox ID="ChkPresenting" runat="server" Text='<%# Eval("NAME") %>' />
                           </ItemTemplate>
                       </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn>
                           <HeaderStyle Width="100px" />
                           <ItemTemplate>
                               <telerik:RadTextBox ID="txtPresenting" runat="server" Skin="MetroTouch" Text='<%# Eval("ChkValText") %>'
                                   Width="150px">
                               </telerik:RadTextBox>
                           </ItemTemplate>
                       </telerik:GridTemplateColumn>
                   </Columns>
                   <AlternatingItemStyle BorderStyle="None" />
               </MasterTableView>
               <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" />
           </telerik:RadGrid>
          
       </td>
      </tr>
      <tr>
       <td  colspan="2" class="SectionheaderTxt" style="width: 100%">
         Additional Presenting Complaints
       </td>
      </tr>
      <tr>
      <td colspan="2">
        Presenting Complaint(s) - description or additional comments :
      </td>
      </tr>
      <tr>
      <td width="100%">
        <telerik:RadTextBox ID="txtAdditionPresentingComplaints" runat="server" Width="100%" TextMode="MultiLine" Skin="MetroTouch"></telerik:RadTextBox>
      </td>
      </tr>
      <tr>
      <td colspan="2" align="center">
       
              <telerik:RadButton ID="btnClosePC" runat="server" Text="Close" OnClick="BtnClosePCClick" Skin="MetroTouch"></telerik:RadButton>
        
      </td>
      </tr>
      
     
     </table>
        </telerik:RadAjaxPanel>
    </ContentTemplate>
</telerik:RadWindow>
<telerik:RadWindow runat="server" ID="rwMedicalHistory" Title="Medical History" Skin="MetroTouch"
    Width="880px" VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close">
    <ContentTemplate>
      <asp:UpdatePanel ID="uptdLabResults" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <table id="Table1" width="100%" class="AdultIE">
            <tr>
                <td>
                    <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridSection" runat="server" Width="100%"
                        AllowPaging="false" ShowFooter="false" CellPadding="0" Skin="MetroTouch" GroupRenderMode="Flat"
                        GridLines="None" ShowHeader="true" ShowGroupPanel="true" OnItemCommand="RadGridSection_ItemCommand"
                        OnItemCreated="RadGridSection_ItemCreated">
                        <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                            DataKeyNames="SectionID" CellSpacing="0" CellPadding="0">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Section Name" HeaderStyle-Font-Bold="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSectionName" runat="server" Text='<%# Eval("SectionName") %>'></asp:Label>
                                        <asp:Label ID="lblSectionID" runat="server" Visible="false" Text='<%# Eval("SectionID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle BackColor="LightGray" />
                                    <%--<HeaderStyle Font-Bold="True"></HeaderStyle>--%>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <NestedViewSettings>
                                <ParentTableRelation>
                                    <telerik:GridRelationFields DetailKeyField="sectionID" MasterKeyField="sectionID" />
                                </ParentTableRelation>
                            </NestedViewSettings>
                            <NestedViewTemplate>
                                <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridFieldName" runat="server"
                                    Width="100%" AllowPaging="false" AllowMultiRowSelection="false" ShowFooter="false"
                                    CellPadding="0" ShowHeader="false" Skin="MetroTouch" HierarchyLoadMode="ServerOnDemand"
                                    DataKeyNames="SectionID" OnItemDataBound="RadGridFieldName_ItemDataBound" OnNeedDataSource="RadGridFieldName_NeedDataSource">
                                    <MasterTableView Name="ChildGrid">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Section Details" HeaderStyle-Font-Bold="true"
                                                UniqueName="SectionID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("FieldLabel") %>'></asp:Label>
                                                    <asp:Label ID="lblControlFlag" runat="server" Visible="false" Text='<%# Eval("name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Section Details" HeaderStyle-Font-Bold="true"
                                                UniqueName="SectionID">
                                                <ItemTemplate>
                                                    <telerik:RadTextBox ID="txtRadText" runat="server" Width="120px" Skin="MetroTouch">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadDatePicker ID="dtDateValue" runat="server" Skin="MetroTouch">
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
                                                    <telerik:RadButton ID="rbtnYesNo" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
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
                            </NestedViewTemplate>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
      </ContentTemplate>
      </asp:UpdatePanel>

    </ContentTemplate>
    
         
  
</telerik:RadWindow>


     

