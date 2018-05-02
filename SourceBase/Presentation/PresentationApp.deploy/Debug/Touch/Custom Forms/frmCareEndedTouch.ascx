<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.Custom_Forms.frmCareEndedTouch"
    CodeBehind="frmCareEndedTouch.ascx.cs" %>
<div id="FormContent">
    <telerik:RadSplitter ID="rwPrint" runat="server" BorderSize="0" BorderStyle="None"
        Width="861px" Height="520px">
        <telerik:RadPane ID="rdPane" runat="server">
            <asp:UpdatePanel ID="updtFormUpdate" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div id="tabs" style="width: 800px">
                        <ul>
                            <li><a href="#tab1">Care Ended</a></li>
                        </ul>
                        <div id="CareEndedContent" class="scroll-pane jspScrollable tabwidth" style="width: 811px;
                            overflow: hidden; height: 380px;">
                            <asp:UpdatePanel ID="updtcareend" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="CareEnded" cellpadding="10px" class="Section" style="width: 100%;">
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Label ID="lblmessage" ForeColor="Red" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50%;">
                                                Date of Missed Scheduled Appointment:
                                            </td>
                                            <td style="width: 50%;">
                                                <telerik:RadDatePicker ID="txtMissedAppDate" runat="server" Width="200px" Skin="MetroTouch">
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
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50%;">
                                                Date of Last Actual Contact:
                                            </td>
                                            <td style="width: 50%;">
                                                <telerik:RadDatePicker ID="txtDateLastContact" runat="server" Width="200px" Skin="MetroTouch">
                                                    <Calendar ID="Calendar4" UseRowHeadersAsSelectors="False" ShowRowHeaders="false"
                                                        UseColumnHeadersAsSelectors="False" Skin="MetroTouch" runat="server">
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
                                        </tr>
                                        <tr id="Artandnonart" runat="server">
                                            <td style="width: 50%;">
                                                *Patient Exit Reason:
                                            </td>
                                            <td style="width: 50%;">
                                                <telerik:RadComboBox ID="cmbPatientExitReason" runat="server" AutoPostBack="true"
                                                    Width="200px" Skin="MetroTouch" OnSelectedIndexChanged="cmbPatientExitReason_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr id="Tr_Deathreason" runat="server">
                                            <td id="td1">
                                                <label>
                                                    Death Reason:</label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cmbDeathReason" Skin="MetroTouch" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cmbDeathReason_SelectedIndexChanged" Width="200px">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr id="Tr_Deathreason1" runat="server">
                                            <td style="width: 50%;">
                                                <label for="AppDate1">
                                                    *Death Date:</label>
                                            </td>
                                            <td style="width: 50%;">
                                                <telerik:RadDatePicker ID="txtDeathDate" runat="server" Skin="MetroTouch" Width="200px">
                                                    <Calendar ID="Calendar5" UseRowHeadersAsSelectors="False" ShowRowHeaders="false"
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
                                                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="PnlConFields" Visible="false" runat="server" Height="100%" Width="100%"
                                            Wrap="true">
                                        </asp:Panel>
                                        <asp:Panel ID="DIVCustomItem" Visible="false" runat="server" Height="100%" Width="100%"
                                            Wrap="true">
                                        </asp:Panel>
                                        <tr>
                                            <td style="width: 50%;">
                                                <label>
                                                    *Signature:</label>
                                            </td>
                                            <td style="width: 50%;">
                                                <telerik:RadComboBox ID="ddinterviewer" Skin="MetroTouch" runat="server" Width="200px">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50%;">
                                                <label id="lblcareendeddate" visible="true" runat="server">
                                                    *Date Care Ended:</label>
                                            </td>
                                            <td style="width: 50%;">
                                                <telerik:RadDatePicker ID="txtCareEndDate" Width="200px" runat="server" Skin="MetroTouch">
                                                    <Calendar ID="Calendar6" UseRowHeadersAsSelectors="False" ShowRowHeaders="false"
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
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hidID" runat="server" />
                                    <asp:HiddenField ID="hidcheckbox" runat="server" />
                                    <asp:HiddenField ID="hidradio" runat="server" />
                                    <asp:HiddenField ID="hiddropdown" runat="server" />
                                    <asp:HiddenField ID="hidchkbox" runat="server" />
                                    <asp:HiddenField ID="hidIDQty" runat="server" />
                                    <asp:HiddenField ID="hidcheckboxQty" runat="server" />
                                    <asp:HiddenField ID="hidradioQty" runat="server" />
                                    <asp:HiddenField ID="hidchkboxQty" runat="server" />
                                    <asp:HiddenField ID="theHitCntrl" runat="server" />
                                    <asp:HiddenField ID="HiddenMsgBuilderfield" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div style="visibility: collapse">
                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" Skin="MetroTouch" OnClick="btnsave_Click">
                        </telerik:RadButton>
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
<div id="frmCareEnded_ScriptBlock" runat="server" style="display: none;">
    <script type="text/javascript">
        function fnShowMessage(hmsg) {
            alert(hmsg);
            return false;
        }

        function fnvalidate(hid, hidradio, hidCheck, hiddrop) {
            var hidval = hid.value;
            var hidrdo = hidradio.value;
            var hidsinglechk = hidCheck.value;
            var hiddropdown = hiddrop.value;

            var rdo = hidrdo.split('%');
            var chksingle = hidsinglechk.split('%');
            var drop = hiddropdown.split('%');
            var findradio = "";
            var rdoid = "";




            var ctrlid = hidval.split('%');
            for (var i = 0; i < ctrlid.length; i++) {
                var cid = "IDfrmCareEndedTouch_" + ctrlid[i];



                if (document.getElementById(cid) != null) {
                    if (document.getElementById(cid).value == '') {
                        var id = "IDfrmCareEndedTouch_LBL" + cid.substring(cid.indexOf("-"), cid.length);
                        var txt = document.getElementById(id).innerText;
                        document.getElementById(id).style.color = 'blue';
                        alert("Please enter the value for :" + txt);
                        document.getElementById(cid).focus();
                        return false;
                    }
                    if (document.getElementById(cid).value == "0.00") {
                        var id = "IDfrmCareEndedTouch_LBL" + cid.substring(cid.indexOf("-"), cid.length);
                        var txt = document.getElementById(id).innerText;
                        document.getElementById(id).style.color = 'blue';
                        alert("Please select for :" + txt);
                        document.getElementById(cid).focus();
                        return false;

                    }

                }

            }
            for (var i = 0; i < rdo.length; i++) {
                var cid = "IDfrmCareEndedTouch_" + rdo[i];

                rdoid = cid;
                var button = $find(rdoid);
                if (button != null) {
                    if (button.get_checked()) {
                        findradio = "find";
                    }
                    if (findradio == '') {
                        var id = "IDfrmCareEndedTouch_LBL" + rdoid.substring(rdoid.indexOf("-"), rdoid.length);
                        var txt = document.getElementById(id).innerText;
                        document.getElementById(id).style.color = 'blue';
                        alert("please select :" + txt);
                        return false;
                    }
                }

            }


            for (var i = 0; i < chksingle.length; i++) {
                var cid = "IDfrmCareEndedTouch_" + chksingle[i];
                var button = $find(rdoid);
                if (button != null) {
                    if (button.get_checked() == false) {
                        var id = "IDfrmCareEndedTouch_LBL" + cid.substring(cid.indexOf("-"), cid.length);
                        var txt = document.getElementById(id).innerText;
                        document.getElementById(id).style.color = 'blue';
                        alert("please select for :" + txt);

                        return false;
                    }
                }
            }

            for (var i = 0; i < drop.length; i++) {
                var cid = "IDfrmCareEndedTouch_" + drop[i];
                var combo = $find(cid);
                if (combo != null) {
                    if (combo.get_selectedItem().get_value() == "0") {
                        var id = "IDfrmCareEndedTouch_LBL" + cid.substring(cid.indexOf("-"), cid.length);
                        var txt = document.getElementById(id).innerText;
                        document.getElementById(id).style.color = 'blue';
                        alert("Please enter the value for :" + txt);

                        return false;

                    }
                }
            }
            return true;
        }
    </script>
</div>
