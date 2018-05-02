<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.Custom_Forms.frmClinicalNotesTouch"
    CodeBehind="frmClinicalNotesTouch.ascx.cs" %>
<div id="FormContent">
    <telerik:RadSplitter ID="rwPrint" runat="server" BorderSize="0" BorderStyle="None"
        Width="861px" Height="520px">
        <telerik:RadPane ID="rdPane" runat="server">
            <asp:UpdatePanel ID="updtFormUpdate" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div id="tabs" style="width: 800px">
                        <ul>
                            <li><a href="#ClincalNoteContent">Non-Visit Clinical Notes</a></li>
                        </ul>
                        <div id="ClincalNoteContent" class="scroll-pane jspScrollable tabwidth" style="width: 811px;
                            overflow: hidden; height: 380px;">
                            <table id="ClinicalNotes" cellpadding="10px" class="Section">
                                <tr>
                                    <td class="SectionheaderTxt" colspan="4" style="width: 100%">
                                        <div>
                                            Non-Visit Clinical Notes</div>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table1" cellpadding="10px" class="Section">
                                <tr>
                                    <td style="width: 20%;">
                                        Date
                                    </td>
                                    <td style="width: 80%;">
                                        <telerik:RadDatePicker ID="dtClincalNoteDate" runat="server" Skin="MetroTouch">
                                            <ClientEvents OnDateSelected="OnBlurDateP" />
                                            <Calendar ID="Calendar3" runat="server" Skin="MetroTouch" UseColumnHeadersAsSelectors="False"
                                                UseRowHeadersAsSelectors="False">
                                            </Calendar>
                                            <DateInput ID="DateInput3" runat="server" DateFormat="dd/MM/yyyy" DisplayDateFormat="dd MMM yyyy"
                                                LabelWidth="0px">
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
                                <tr>
                                    <td style="vertical-align: text-top;">
                                        Notes
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtClinicalNote" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                            Width="580px" Height="200px">
                                            <ClientEvents OnBlur="OnBlur" />
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
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
</div>
<!-- PASDP jscript -->
<script type="text/javascript" src="../scripts/PASDPBaseScripts.js"></script>
<div id="frm_NCN_ScriptBlock" runat="server" style="display: none;">
    <script type="text/javascript">
        function CheckVals(sender, args) {
            var ReqVals = new Array();
            ReqVals[0] = "IDfrmClinicalNotesTouch_dtClincalNoteDate|Clinical Note Date";
            ReqVals[1] = "IDfrmClinicalNotesTouch_txtClinicalNote|Clinical Note";

            var theNames = new Array();
            var ReqIsFilled = true;
            for (index = 0; index < ReqVals.length; ++index) {

                var arr = ReqVals[index].split("|");

                var theFirstControl = null;
                if (index != 4) {
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
                    args.set_cancel(!theAnswer);
                }
            }
            else {
                gotToTabVal(theFirstControl, theNames, args);
            }
        }
        function gotToTabVal(thecontrol, thenames, args) {
            var theMessage = "You have not given a value for the required field(s): ";
            for (i = 0; i < thenames.length; ++i) {
                if (typeof thenames[i] != "undefined")
                    theMessage = theMessage + "\n" + thenames[i];
            }

            alert(theMessage);
            $('#' + thecontrol).focus();
            args.set_cancel(true);
        }
    </script>
</div>
<div style="visibility: collapse">
    <telerik:RadButton ID="btnSave" runat='server' OnClientClicking="CheckVals" Text="Test"
        OnClick="btnSave_Click">
    </telerik:RadButton>
</div>
