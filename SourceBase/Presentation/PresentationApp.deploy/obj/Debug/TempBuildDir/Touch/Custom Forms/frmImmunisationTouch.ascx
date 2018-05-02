<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmImmunisationTouch.ascx.cs"
    Inherits="Touch.Custom_Forms.frmImmunisationTouch" %>
<%--<script type="text/javascript">
    function OpenModal(button, args) {
        var oWnd = $find("customRegistration_" + args._commandArgument.toString());
        oWnd.show();
        return false;
    }
</script>--%>
<%--
<script type="text/javascript">

  

   

</script>--%>
<div id="FormContent">
    
    <telerik:RadSplitter ID="rwPrint" runat="server" BorderSize="0" BorderStyle="None"
        Width="861px" Height="520px">
        <telerik:RadPane ID="rdPane" runat="server">
            <asp:UpdatePanel ID="updtFormUpdate" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div id="tabs" style="width: 800px">
                        <ul>
                            <li><a href="#ImmunContent">Immunisations</a></li>
                        </ul>
                        <div id="ImmunContent" class="scroll-pane jspScrollable tabwidth" style="width: 811px;
                            overflow: hidden; height: 380px;">
                            <table id="immunisations" cellpadding="10px" class="Section" style="width: 750px;">
                            </table>
                            <table id="Birth" cellpadding="10px" class="Section" style="width: 750px;">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:Label ID="lblerr" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    Road to health card available
                                                </td>
                                                <td style="width: 28%;">
                                                    <telerik:RadButton ID="rbtnCardLostYes" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    *Do not administer catch up vaccine after XX years.
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            Birth</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Vaccine</b>
                                                </td>
                                                <td>
                                                    <b>Administered</b>
                                                </td>
                                                <td>
                                                    <b>Date Given</b>
                                                </td>
                                                <td>
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    BCG
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnBCG" runat="server" Width="52px" GroupName="BirthBCG" ToggleType="CustomToggle"
                                                        AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateBCG" runat="server" Skin="MetroTouch" 
                                                        Culture="(Default)">
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    OPV0
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnOPV0" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtOPV0Date" runat="server" Skin="MetroTouch" 
                                                        Culture="(Default)">
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
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            6 Weeks</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 20%">
                                                    <b>Vaccine</b>
                                                </td>
                                                <td style="width: 10%">
                                                    <b>Administered</b>
                                                </td>
                                                <td style="width: 10%">
                                                    <b>Date Given</b>
                                                </td>
                                                <td style="width: 20%">
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    OPV1
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnOPV1" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateOPV1" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_OPV1" runat="server" Width="52px" GroupName="BirthBCG"
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
                                                    RV1
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnRV1" runat="server" Width="52px" GroupName="BirthBCG" ToggleType="CustomToggle"
                                                        AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateRV1" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    DTaP-IPV-Hib1
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnDTaP1" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateDTaP1" runat="server" Skin="MetroTouch" 
                                                        Culture="(Default)">
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
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Hep B1
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnHEP1" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateHepB1" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_HEP1" runat="server" Width="52px" GroupName="BirthBCG"
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
                                                    PCV1
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btmPCV1" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDatePCV1" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_PCV1" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            10 Weeks</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Vaccine</b>
                                                </td>
                                                <td>
                                                    <b>Administered</b>
                                                </td>
                                                <td>
                                                    <b>Date Given</b>
                                                </td>
                                                <td>
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    DTaP-IPV-Hib2
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnDTaP2" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateDTaP2" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Hep B2
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnHEP2" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateHepB2" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_HEP2" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            14 Weeks</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Vaccine</b>
                                                </td>
                                                <td>
                                                    <b>Administered</b>
                                                </td>
                                                <td>
                                                    <b>Date Given</b>
                                                </td>
                                                <td>
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    DTaP-IPV-Hib3
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnDTaP3" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateDTaP3" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Hep B3
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnHEP3" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateHepB3" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_HEP3" runat="server" Width="52px" GroupName="BirthBCG"
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
                                                    PCV2
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnPCV2" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDatePCV2" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                <td>
                                                    <telerik:RadButton ID="btnCUGT_PCV2" runat="server" Width="52px" GroupName="BirthBCG"
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
                                                    RV2
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnRV2" runat="server" Width="52px" GroupName="BirthBCG" ToggleType="CustomToggle"
                                                        AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateRV2" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            9 Months</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Vaccine</b>
                                                </td>
                                                <td>
                                                    <b>Administered</b>
                                                </td>
                                                <td>
                                                    <b>Date Given</b>
                                                </td>
                                                <td>
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Measles1
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnMeasles1" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateMeasles1" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                <td>
                                                    <telerik:RadButton ID="btnCUGT_Measles1" runat="server" Width="52px" GroupName="BirthBCG"
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
                                                    PCV3
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnPCV3" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDatePVC3" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_PCV3" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            18 Months</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Vaccine</b>
                                                </td>
                                                <td>
                                                    <b>Administered</b>
                                                </td>
                                                <td>
                                                    <b>Date Given</b>
                                                </td>
                                                <td>
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    DTaP-IPV-Hib4
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnDTaP4" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateDTaP4" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                <td>
                                                    &nbsp
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Measles2
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnMeasles2" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateMeasles2" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_Measles2" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            6 Years</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Vaccine</b>
                                                </td>
                                                <td>
                                                    <b>Administered</b>
                                                </td>
                                                <td>
                                                    <b>Date Given</b>
                                                </td>
                                                <td>
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Td - 6 yrs
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnTD6yrs" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateTd6Yrs" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                <td>
                                                    <telerik:RadButton ID="btnCUGT_Td6yrs" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            12 Years</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Vaccine</b>
                                                </td>
                                                <td>
                                                    <b>Administered</b>
                                                </td>
                                                <td>
                                                    <b>Date Given</b>
                                                </td>
                                                <td>
                                                    <b>Catch Up Given Today</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Td - 12 yrs
                                                </td>
                                                <td>
                                                    <telerik:RadButton ID="btnTd12yrs" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtDateTd23Yrs" runat="server" Skin="MetroTouch" Culture="(Default)">
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
                                                    <telerik:RadButton ID="btnCUGT_Td12yrs" runat="server" Width="52px" GroupName="BirthBCG"
                                                        ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                                        <ToggleStates>
                                                            <telerik:RadButtonToggleState Text="No" />
                                                            <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                        </ToggleStates>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="SectionheaderTxt" style="width: 100%">
                                        <div>
                                            Other Immunisation</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblerrImmOther" runat="server" Font-Names="verdana" Font-Size="10pt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <div id="DivImmunisationOther">
                                                        <asp:UpdatePanel ID="updtPharmdrugs" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadGrid AutoGenerateColumns="false" ID="RadOtherVaccine" runat="server"
                                                                                Width="100%" PageSize="5" ShowFooter="true" Font-Size="8pt" Font-Names="verdana"
                                                                                OnItemDataBound="RadOtherVaccine_ItemDataBound" OnItemCommand="RadOtherVaccine_ItemCommand"
                                                                                OnDeleteCommand="RadOtherVaccine_DeleteCommand" CellPadding="0" OnCancelCommand="RadOtherVaccine_CancelCommand"
                                                                                OnUpdateCommand="RadOtherVaccine_UpdateCommand" 
                                                                                OnEditCommand="RadOtherVaccine_EditCommand" Culture="(Default)">
                                                                                <PagerStyle Mode="NextPrevAndNumeric" />
                                                                                <ClientSettings>
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Resizing AllowColumnResize="True" EnableRealTimeResize="True"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                                                                                    DataKeyNames="ID" CellSpacing="0" CellPadding="0">
                                                                                    <NoRecordsTemplate>
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    No Records Found
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </NoRecordsTemplate>
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn HeaderText="Vaccine" HeaderStyle-Font-Bold="true">
                                                                                            <HeaderStyle Font-Size="10px" Wrap="False" Width="172px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblImmunisation_name" runat="server" Text='<%# Eval("ImmunisationOther") %>'></asp:Label>
                                                                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                                                                                <asp:Label ID="lblEditMode" runat="server" Visible="false" Text='<%# Eval("EditMode") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <%--<EditItemTemplate>
                                                                    <telerik:RadTextBox ID="txtEditRadVaccineName" runat="server" Text='<%# Eval("ImmunisationOther") %>'>
                                                                    </telerik:RadTextBox>
                                                                </EditItemTemplate>--%>
                                                                                            <FooterTemplate>
                                                                                                <telerik:RadTextBox ID="txtFooterRadVaccineName" runat="server">
                                                                                                </telerik:RadTextBox>
                                                                                            </FooterTemplate>
                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Administered" HeaderStyle-Font-Bold="true">
                                                                                            <HeaderStyle Font-Size="10px" Wrap="False" Width="87px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAdministered" runat="server" Visible="false" Text='<%# Eval("Administered") %>'></asp:Label>
                                                                                                <telerik:RadButton ID="btnOthers" runat="server" GroupName="BirthBCG" ToggleType="CustomToggle"
                                                                                                    AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch" Enabled="false">
                                                                                                    <ToggleStates>
                                                                                                        <telerik:RadButtonToggleState Text="No" />
                                                                                                        <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                                                                    </ToggleStates>
                                                                                                </telerik:RadButton>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <telerik:RadButton ID="btnFooterOthers" runat="server" GroupName="BirthBCG" ToggleType="CustomToggle"
                                                                                                    AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch" Enabled="false">
                                                                                                    <ToggleStates>
                                                                                                        <telerik:RadButtonToggleState Text="Yes" />
                                                                                                        <telerik:RadButtonToggleState Text="No" CssClass="BlueBG" />
                                                                                                    </ToggleStates>
                                                                                                </telerik:RadButton>
                                                                                            </FooterTemplate>
                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Date Given" HeaderStyle-Font-Bold="true">
                                                                                            <HeaderStyle Font-Size="10px" Wrap="False" Width="186px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblOtherDateGiven" runat="server" Text='<%# Eval("ImmunisationDate") %>'>></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <telerik:RadDatePicker ID="dtFooterOtherDate" runat="server" Skin="MetroTouch" Calendar-ShowRowHeaders="false" Culture="(Default)">
                                                                                                    <Calendar ID="Calendar19" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                                                                        UseRowHeadersAsSelectors="False">
                                                                                                    </Calendar>
                                                                                                    <DateInput ID="DateInputFooterOther" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
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
                                                                                            </FooterTemplate>
                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Catch Up Given Today " HeaderStyle-Font-Bold="true">
                                                                                            <HeaderStyle Font-Size="10px" Wrap="False" Width="130px" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblcatchUp" runat="server" Visible="false" Text='<%# Eval("ImmunisationCU") %>'></asp:Label>
                                                                                                <telerik:RadButton ID="btnCatchupOthers" runat="server" GroupName="BirthBCG11" ToggleType="CustomToggle"
                                                                                                    AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch" Enabled="false">
                                                                                                    <ToggleStates>
                                                                                                        <telerik:RadButtonToggleState Text="No" />
                                                                                                        <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                                                                    </ToggleStates>
                                                                                                </telerik:RadButton>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <telerik:RadButton ID="btnCatchupFooterOthers" runat="server" ButtonType="StandardButton"
                                                                                                    ToggleType="CustomToggle" AutoPostBack="false" Checked="true" GroupName="BirthBCG12"
                                                                                                    BackColor="White" ToolTip="Toogle Folders">
                                                                                                    <ToggleStates>
                                                                                                        <telerik:RadButtonToggleState Text="No" />
                                                                                                        <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                                                                    </ToggleStates>
                                                                                                </telerik:RadButton>
                                                                                            </FooterTemplate>
                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderStyle-Font-Bold="true">
                                                                                            <HeaderStyle Font-Size="10px" Wrap="False" Width="74px" />
                                                                                            <ItemTemplate>
                                                                                                <telerik:RadButton ID="btnRemove" runat="server" Skin="MetroTouch" Text="Remove"
                                                                                                    ForeColor="Blue" CommandName="Delete" ButtonType="LinkButton">
                                                                                                </telerik:RadButton>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <telerik:RadButton ID="btnFooterAdd" runat="server" Skin="MetroTouch" Text="Add"
                                                                                                    CommandName="Insert">
                                                                                                </telerik:RadButton>
                                                                                            </FooterTemplate>
                                                                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridEditCommandColumn>
                                                                                        </telerik:GridEditCommandColumn>
                                                                                    </Columns>
                                                                                    <EditFormSettings EditFormType="Template">
                                                                                        <FormTemplate>
                                                                                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                                <tr class="EditFormHeader">
                                                                                                    <td colspan="2" style="font-size: small">
                                                                                                        <b>Other Details</b>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        Vaccine:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <telerik:RadTextBox ID="txtEditRadVaccineName" Skin="MetroTouch" Enabled="false"
                                                                                                            runat="server" Text='<%# Eval("ImmunisationOther") %>'>
                                                                                                        </telerik:RadTextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        Administered:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <telerik:RadButton ID="btnEditOthers" runat="server" GroupName="BirthBCG" ToggleType="CustomToggle"
                                                                                                            AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch" Checked="true"
                                                                                                            Enabled="false">
                                                                                                            <ToggleStates>
                                                                                                                <telerik:RadButtonToggleState Text="Yes" />
                                                                                                                <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                                                                            </ToggleStates>
                                                                                                        </telerik:RadButton>
                                                                                                        <asp:Label ID="lblIDEdit" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        Date Given:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <telerik:RadDatePicker ID="dtEditOtherDate" runat="server" Skin="MetroTouch" Calendar-ShowRowHeaders="false" Culture="(Default)">
                                                                                                            <Calendar ID="Calendar19" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                                                                                UseRowHeadersAsSelectors="False">
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
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        Catch Up Given Today:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <telerik:RadButton ID="btnCatchupEditOthers" runat="server" Width="52px" GroupName="BirthBCG"
                                                                                                            ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch"
                                                                                                            Checked="true">
                                                                                                            <ToggleStates>
                                                                                                                <telerik:RadButtonToggleState Text="No" />
                                                                                                                <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                                                                                            </ToggleStates>
                                                                                                        </telerik:RadButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="right" colspan="2">
                                                                                                        <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                                                                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>'>
                                                                                                        </asp:Button>&nbsp;
                                                                                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                                                                                            CommandName="Cancel"></asp:Button>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </FormTemplate>
                                                                                    </EditFormSettings>
                                                                                </MasterTableView>
                                                                                <%--<FooterStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />--%>
                                                                            </telerik:RadGrid>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <a id="sGrid" href="#sGrid"></a>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div id="OtherImmunisationButton">
                                <asp:HiddenField ID="hddErrormsg" runat="server" />
                                <asp:HiddenField ID="HddErrorFlag" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div style="float: left; position: relative; margin: 5px 5px 5px 0px;">
                        <telerik:RadButton ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_OnClick">
                        </telerik:RadButton>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <div style="visibility: collapse">
        <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClientClicking="LoadingPanel"
            Skin="MetroTouch" Visible="true" OnClick="btnSave_Click">
        </telerik:RadButton>
    </div>
</div>
<div id="frmImmunisation_block" runat="server" style="display: none;">
    <script type="text/javascript" language="javascript">
        function FormValidatedOnSubmit(Msg) {

            alert(Msg);
            return false;
        }
        function LoadingPanel(sender, args) {
            parent.ShowLoading();
        }

    </script>
</div>
