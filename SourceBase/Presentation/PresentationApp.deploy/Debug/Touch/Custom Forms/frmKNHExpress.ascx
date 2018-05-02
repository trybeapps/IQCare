<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmKNHExpress.ascx.cs" Inherits="Touch.Custom_Forms.frmKNHExpress" %>
<div style="visibility: collapse">
     <telerik:RadButton ID="btnSave" runat="server" Text="Save" Skin="MetroTouch" 
                       onclick="btnSave_Click"></telerik:RadButton>

  
</div>
<asp:HiddenField ID="hidtab" Value="0" runat="server" />
<div id="FormContent">
    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Move,Close" Width="800px"
                Height="450px" NavigateUrl="~/Touch/KNH/KNHVitalSignShowModal.aspx" Title="Vital Signs"
                Modal="true" OnClientClose="OnClientClose">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div id="tabs" style="width: 800px">
        <ul>
            <li><a href="#tab1">Triage</a></li>
            <li><a href="#tab2">Clinical assessment</a></li>
        </ul>
        <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
            height: 380px;">
            <asp:UpdatePanel ID="uptTab1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    
                    <table id="Triage" cellpadding="10px" class="Section">
                        <tr>
                            <td>
                                <asp:Label ID="lblerr" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                       <tr>
                       <td>
                           <table>
                               <tr>
                                   <td>
                                       Visit Date:
                                   </td>
                                   <td>
                                       <telerik:RadDatePicker ID="dtVisit" runat="server" Skin="MetroTouch">
                                           <Calendar ID="Calendar3" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                               UseRowHeadersAsSelectors="False">
                                           </Calendar>
                                           <DateInput ID="DateInput3" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                        Patient accompanied by caregiver:
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="btnChildAccompaniedByCaregiver" runat="server" Width="52px"
                                            GroupName="BirthBCG" ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton"
                                            Skin="MetroTouch">
                                            <ToggleStates>
                                                <telerik:RadButtonToggleState Text="No" />
                                                <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                            </ToggleStates>
                                        </telerik:RadButton>
                                    </td>
                                    <td>
                                        Caregivers relationship:
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="rcbcareGiverRelationship" runat="server" EmptyMessage="Select"
                                            AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true"
                                            Enabled="false">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                        
                        </td>
                        </tr>
                        
                        <tr>
                            <td align="left">
                                <li><a href="#" onclick="openWin();return false;">Vital Signs</a></li>
                                <asp:HiddenField ID="HiddRadTemperature" runat="server" />
                                <asp:HiddenField ID="HiddRadRespirationRate" runat="server" />
                                <asp:HiddenField ID="HiddRadHeartRate" runat="server" />
                                <asp:HiddenField ID="HiddRadSystollicBloodPressure" runat="server" />
                                <asp:HiddenField ID="HiddRadDiastolicBloodPressure" runat="server" />
                                <asp:HiddenField ID="HiddRadHeight" runat="server" />
                                <asp:HiddenField ID="HiddRadWeight" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblVitalSign" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="SectionheaderTxt" style="width: 100%">
                                <div>
                                    Pre-Existing (Known conditions)
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="PreExistingKnownConditions" width="100%">
                                    <tr>
                                        <td valign="top">
                                            Medical Condition?:
                                        </td>
                                        <td valign="top">
                                            <telerik:RadButton ID="radbtnMedicalCondition" runat="server" Width="52px" GroupName="BirthBCG"
                                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="No" />
                                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                </ToggleStates>
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            Pre existing medical condition:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbmedicalCondition" runat="server" Text="aSomeTest" AutoPostBack="false"
                                                Enabled="false" Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                CheckedItemsTexts="FitInInput" Width="250px">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Are you on follow up?:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtnFollowup" runat="server" Width="52px" GroupName="BirthBCG"
                                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="No" />
                                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                </ToggleStates>
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            Last Follow up date:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="RadDateLastFolowup" runat="server" Skin="MetroTouch">
                                                <Calendar ID="Calendar1" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                    UseRowHeadersAsSelectors="False">
                                                </Calendar>
                                                <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                            Previously admitted in the last 2 weeks?:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="RadBtnAdmitted" runat="server" Width="52px" GroupName="BirthBCG"
                                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="No" />
                                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                </ToggleStates>
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            Diagnosis:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtradDiagnosis" runat="server" Wrap="true" Skin="Metro">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Admission Start:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="RadDateAdmissionDate" runat="server" Skin="MetroTouch">
                                                <Calendar ID="Calendar2" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                    UseRowHeadersAsSelectors="False">
                                                </Calendar>
                                                <DateInput ID="DateInput2" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                            Admission End:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="RadDateAdmissionEnd" runat="server" Skin="MetroTouch">
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
             <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                 <asp:HiddenField ID="HiddVisit_pk" runat="server" />
                    <table cellpadding="10px" class="Section">
                        <tr>
                            <td colspan="2" align="left" class="SectionheaderTxt" style="width:100%;" >
                                TB Assessment
                            </td>
                        </tr>
                        <tr>
                         <td colspan="2">
                         <table width="100%">
                         <tr>
                          <td >
                                TB Screening:
                            </td>
                            <td >
                                <telerik:RadComboBox ID="rcbTBAassessment" runat="server" Text="aSomeTest" AutoPostBack="false"
                                    Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput" Width="120px">
                                    
                                </telerik:RadComboBox>
                            </td>
                            <td >
                                TB Findings :
                            </td>
                            <td >
                                <telerik:RadComboBox ID="rcbTBFindings" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                    Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" Width="120px">
                                </telerik:RadComboBox>
                            </td>
                         </tr>
                         </table>
                         </td>
                            
                        </tr>
                        <tr>
                            <td colspan="2" class="SectionheaderTxt">
                                Regimen Precsribed
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            Regimen Precsribed
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbRegimenPrescribed" runat="server" EmptyMessage="Select"
                                                AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            Other regimen (specify):
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtradOtherRegimen" runat="server" Wrap="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="SectionheaderTxt" >
                                Available Results
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td valign="top">
                                            Lab Evaluation:
                                        </td>
                                        <td valign="top">
                                            <telerik:RadButton ID="RadbtnLabEvalution" runat="server" Width="52px" GroupName="Evalution"
                                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="No" />
                                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                </ToggleStates>
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            If Yes, specify lab evaluation:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbLabEvalution" runat="server" Text="aSomeTest" AutoPostBack="false"
                                                Enabled="false" Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                CheckedItemsTexts="FitInInput" Width="250px">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            OI Prophylaxis:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbProphylaxis" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                                Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            Cotrimoxazole prescribed for?:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbCotrimoxazole" runat="server" Width="250" Height="150"
                                                EmptyMessage="Select" EnableLoadOnDemand="True" Skin="MetroTouch">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Other (specify):
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtOtherSpecify" runat="server" Wrap="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="2" class="SectionheaderTxt" >
                                Pharmacey and Laboratory
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnlnkLab" runat="server" ButtonType="LinkButton" Skin="MetroTouch"
                                                Text="Order Lab Test">
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="btnlnkPharmacy" runat="server" ButtonType="LinkButton" Skin="MetroTouch"
                                                Text="Prescribe Drugs">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="SectionheaderTxt" >
                                Treatment Plan
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            Plan:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTreatmentplan" runat="server" Width="500px" TextMode="MultiLine"
                                                Skin="MetroTouch">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                       
                        <tr>
                            <td colspan="2" class="SectionheaderTxt" >
                                PWP Interventions
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            PWP messages given:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radpwpmessagegiven" runat="server" Width="52px" GroupName="Evalution"
                                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="No" />
                                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                </ToggleStates>
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            Patient issued with condoms:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtnissueCondoms" runat="server" Width="52px" GroupName="Evalution"
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
                                            Reasons for not issuing condoms:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtReasonforCondomNotIssued" runat="server" TextMode="MultiLine"
                                                Skin="MetroTouch">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            Pregnancy intention before next visit:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtnPregncyIntfnxtvisit" runat="server" Width="52px" GroupName="Evalution"
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
                                            If Yes, discussed fertility options:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtnddiscussferti" runat="server" Width="52px" GroupName="Evalution"
                                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="No" />
                                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                </ToggleStates>
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            If No, discussed dual contraception:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtnNoDualconta" runat="server" Width="52px" GroupName="Evalution"
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
                                            Apart from condoms other family planning method:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtnfpmethord" runat="server" Width="52px" GroupName="Evalution"
                                                ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                <ToggleStates>
                                                    <telerik:RadButtonToggleState Text="No" />
                                                    <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                </ToggleStates>
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            Specify other FP method other than condoms:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbFpMethord" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                                Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Have you been screened for cervical cancer:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtncervicalcancer" runat="server" Width="52px" GroupName="Evalution"
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
                                            <telerik:RadComboBox ID="rcbCCScreeningResults" runat="server" EmptyMessage="Select"
                                                AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            If No, reffered for cervical cancer screening:
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="radbtnccscreeningref" runat="server" Width="52px" GroupName="Evalution"
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
                                            Signature:
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="rcbSignature" runat="server" AutoPostBack="false" EmptyMessage="Select"
                                                Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
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
        
      

    </div>
</div>

<div id="frmExpressForm_ScriptBlock" runat="server" style="display: none;">
   <script type="text/javascript" language="javascript">
       function FormValidatedOnSubmit(Msg) {
           alert(Msg);
           return false;
       }

       function OpenModalFromClient1(TheWindow) {
           var oWnd = $find("allmodalsControl_" + TheWindow);
           oWnd.show();
           return false;
       }
       function OpenModalFromClientClose(TheWindow) {
           var oWnd = $find("allmodalsControl_" + TheWindow);
           oWnd.close();
           return false;
       }
       function openWin() {


           var oWnd = radopen("KNH/KNHVitalSignShowModal.aspx?Qry=IDfrmKNHExpress-VITALSIGN", "RadWindow1");

       }
       //       
       //       function OnClientShow(oWnd) {
       //           //Create a new Object to be used as an argument to the radWindow
       //           var arg = new Object();
       //           //Using an Object as a argument is convenient as it allows setting many properties.
       //           arg.text = document.getElementById("txtRadTemperature").value;
       //           //Set the argument object to the radWindow  
       //           oWnd.Argument = arg;
       //       }
       //       function ClientCallBackFunction(radWindow, returnValue) {
       //           //check if a value is returned from the dialog
       //           if (returnValue.newtext) {
       //               alert(returnValue.newtext);
       //               
       //               //document.getElementById("Hidden1").value = returnValue.newtext;
       //               //alert("HiddenValue: " + document.getElementById("Hidden1").value);
       //           }
       //       }



   </script>
</div>
