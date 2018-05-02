<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmPatientCustomRegistration"
    MaintainScrollPositionOnPostback="true" Codebehind="frmPatientCustomRegistration.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
     
        function CalcualteAge(txtAge, txtmonth, txtDT1, txtDT2) {
            var YR1 = document.getElementById(txtDT1).value.toString().substr(7, 4);
            var YR2 = document.getElementById(txtDT2).value.toString().substr(7, 4);

            var mm1 = document.getElementById(txtDT1).value.toString().substr(3, 3);
            var mm2 = document.getElementById(txtDT2).value.toString().substr(3, 3);

            var dd1 = document.getElementById(txtDT1).value.toString().substr(0, 2);
            var dd2 = document.getElementById(txtDT2).value.toString().substr(0, 2);

            var nmm1;
            switch (mm1.toLowerCase()) {
                case "jan": nmm1 = "01";
                    break;
                case "feb": nmm1 = "02";
                    break;
                case "mar": nmm1 = "03";
                    break;
                case "apr": nmm1 = "04";
                    break;
                case "may": nmm1 = "05";
                    break;
                case "jun": nmm1 = "06";
                    break;
                case "jul": nmm1 = "07";
                    break;
                case "aug": nmm1 = "08";
                    break;
                case "sep": nmm1 = "09";
                    break;
                case "oct": nmm1 = "10";
                    break;
                case "nov": nmm1 = "11";
                    break;
                case "dec": nmm1 = "12";
                    break;
            }
            var nmm2;
            switch (mm2.toLowerCase()) {
                case "jan": nmm2 = "01";
                    break;
                case "feb": nmm2 = "02";
                    break;
                case "mar": nmm2 = "03";
                    break;
                case "apr": nmm2 = "04";
                    break;
                case "may": nmm2 = "05";
                    break;
                case "jun": nmm2 = "06";
                    break;
                case "jul": nmm2 = "07";
                    break;
                case "aug": nmm2 = "08";
                    break;
                case "sep": nmm2 = "09";
                    break;
                case "oct": nmm2 = "10";
                    break;
                case "nov": nmm2 = "11";
                    break;
                case "dec": nmm2 = "12";
                    break;
            }
            var dt1 = nmm1 + "/" + dd1 + "/" + YR1;
            var dt2 = nmm2 + "/" + dd2 + "/" + YR2;

            var val1 = dateDiff("d", dt1, dt2, "", "") / 365;
            var val2 = Math.round((dateDiff("d", dt1, dt2, "", "") / 365));
            if (val2 > val1) {
                if (dt1.length < 11) {
                    document.getElementById(txtAge).value = Math.round((dateDiff("d", dt1, dt2, "", "") / 365)) - 1;
                    var yr = Math.round(dateDiff("d", dt1, dt2, "", "") / 365) - 1;

                    document.getElementById(txtmonth).value = Math.round((dateDiff("d", dt1, dt2, "", "") - (365 * yr)) / 30);
                }
                else {
                    document.getElementById(txtAge).value = "";
                    document.getElementById(txtmonth).value = "";
                }
            }
            else {
                if (dt1.length < 11) {
                    document.getElementById(txtAge).value = Math.round((dateDiff("d", dt1, dt2, "", "") / 365));
                    var yr = Math.round(dateDiff("d", dt1, dt2, "", "") / 365);
                    document.getElementById(txtmonth).value = Math.round((dateDiff("d", dt1, dt2, "", "") - (365 * yr)) / 30);
                }
                else {
                    document.getElementById(txtAge).value = "";
                    document.getElementById(txtmonth).value = "";
                }
            }
        }
        function jsAreaClose(id) {
            document.getElementById(id).style.display = 'none';
        }
        var pickedUp = new Array("", false);
        function getReadyToMove(element, evt) {
            pickedUp[0] = element;
            pickedUp[1] = true;
        }
        function SetValue(theObject, theValue) {
            document.getElementById('ctl00_IQCareContentPlaceHolder_' + theObject).value = theValue;
            document.forms[0].submit();
        }
        function WindowPrint() {
            window.print();
        }

        function ValidateAge() {
            var hidid = document.getElementById('<%=hdnIds.ClientID %>').value;
            var Age = document.getElementById('<%=txtageCurrentYears.ClientID %>').value;
            var result = frmPatientCustomRegistration.EnableControlAge(hidid, Age).value;
            if (result != '') {
                document.getElementById('ctl00_IQCareContentPlaceHolder_' + result).disabled = true;
            }
            if (document.getElementById('ctl00_IQCareContentPlaceHolder_img' + result) != null) {
                document.getElementById('ctl00_IQCareContentPlaceHolder_' + result).disabled = true;
            }
        }
        function fnshow() {

            var status = '<%=Request.QueryString["name"]%>';
            if (status != 'Edit') {
                var fname = document.getElementById('<%=txtfirstName.ClientID %>').value;
                var mname = "";
                var lname = document.getElementById('<%=txtlastName.ClientID %>').value;
                var address = "";
                var phone = "";
                if (fname != '') {
                    var result = frmPatientCustomRegistration.GetDuplicateRecord(fname, mname, lname, address, phone).value;
                    if (result != "") {
                        document.getElementById('search_popup').style.display = 'inline';
                        document.getElementById('showresult').innerHTML = result;
                    }
                    else {
                        document.getElementById('search_popup').style.display = 'none';
                    }
                }
            }
        }
    </script>
    <%--<div style="padding-top: 1px;">--%>
    <div style="padding: 10px;">
        <table cellspacing="0" cellpadding="0" width="930" border="0">
            <tbody>
                <tr>
                    <td >
                        <h1 style="padding-left: 10px;">
                            IQCare Registration</h1>
                        <div class="border center formbg">
                            <div class="popupWindow" id='search_popup' onclick="javascript:getReadyToMove('search_popup', event);"
                                name='styled_popup' style="display: none;">
                                <table cellspacing="0" cellpadding="0" style="width: 100%" border="0">
                                    <tr bgcolor="#666699">
                                        <td align="right">
                                            <span style="cursor: hand" onclick="jsAreaClose('search_popup')">
                                                <img alt="Hide Popup" src="./Images/close.gif" border="0"/></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="height: 150px">
                                            <div id="showresult">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="alert">
                                            Please check for duplicates
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table cellspacing="6" cellpadding="0" width="99%" border="0">
                                <tbody>
                                    <tr>
                                        <td class="border pad5 whitebg" align="left">
                                            <label id="lblPName" class="required" for="patientname">
                                                *Patient Name:</label>
                                            <span id="FName" class="smallerlabel">First: </span>
                                            <asp:TextBox ID="txtfirstName" MaxLength="40" runat="server"></asp:TextBox>
                                            <span id="MName" class="smallerlabel">Middle: </span>
                                            <asp:TextBox ID="txtmiddleName" MaxLength="40" runat="server"></asp:TextBox>
                                            <span id="LName" class="smallerlabel">Last: </span>
                                            <asp:TextBox ID="txtlastName" MaxLength="40" onchange="fnshow();" onblur="fnshow();" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="border pad5 whitebg" width="28%" align="left">
                                            <label id="lblIQRef">
                                                IQCare Reference:</label>
                                            <asp:TextBox ID="txtIQCareRef" MaxLength="11" runat="server" Width="60%" TabIndex="100"
                                                ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="border pad5 whitebg leftallign" nowrap="nowrap">
                                            <label id="lblgender" class="required" for="gender">
                                                *Sex:</label>
                                            <asp:DropDownList ID="ddgender" runat="server">
                                            </asp:DropDownList>
                                            <label id="lblDOB" class="required margin15" for="DOB">
                                                *Date of Birth:</label>
                                            <asp:TextBox ID="TxtDOB" MaxLength="11" runat="server" Width="70px"></asp:TextBox>
                                            <img onclick="w_displayDatePicker('<%= TxtDOB.ClientID %>');" height="22" alt="Date Helper"
                                                hspace="3" src="./images/cal_icon.gif" width="20" border="0"/>
                                            <span class="smallerlabel">DD-MMM-YYYY </span>
                                            <input id="rbtndobPrecExact" onmouseup="up(this);" onfocus="up(this);" onclick="down(this)" type="radio"
                                                value="1" name="dobPrecision" runat="server"/>
                                            <span class="smalllabel">Exact </span>
                                            <input id="rbtndobPrecEstimated" onmouseup="up(this);" onfocus="up(this);" onclick="down(this)" type="radio"
                                                value="0" name="dobPrecision" runat="server"/>
                                            <span class="smalllabel">Estimated</span>
                                        </td>
                                        <td class="border pad5 whitebg" align="left">
                                            <label for="Age">
                                                Age:</label>
                                            <asp:TextBox ID="txtageCurrentYears" MaxLength="2" runat="server" Width="10%"></asp:TextBox>
                                            <span class="smallerlabel">yrs</span>
                                            <asp:TextBox ID="txtageCurrentMonths" MaxLength="2" ReadOnly="true" runat="server" Width="10%"></asp:TextBox>
                                            <span class="smallerlabel">mths</span> <span style="width: 10px"></span>
                                            <asp:Button ID="btncalculate_DOB" runat="server" Text="Calculate DOB" OnClick="btncalculate_DOB_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" width="100%" style="height: 34px">
                                            <table width="100%">
                                                <tr>
                                                    <td class="border pad6 whitebg" align="center" width="48%">
                                                        <label class="margin20" for="maritalstatus">
                                                            Marital Status:</label>
                                                        <asp:DropDownList ID="ddmaritalStatus" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="border pad6 whitebg" align="center" width="48%">
                                                        <label id="lblenroldate" class="required margin35" for="RegDate">
                                                            *Registration Date:</label>
                                                        <asp:TextBox ID="txtRegDate" runat="server" MaxLength="11" Width="70px" onblur="javascript:ValidateAge()"></asp:TextBox>
                                                        <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtRegDate.ClientID %>');"
                                                            src="images/cal_icon.gif" width="20"/> <span class="smallerlabel">DD-MMM-YYYY </span>
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <br />
                        <asp:Panel class="border center formbg" ID="PnlDynamicElements" Width="100%" runat="server">
                            <br />
                        </asp:Panel>
                        <br/>
                        <div class="border center formbg">
                            <table cellspacing="6" cellpadding="0" width="99%">
                                <tbody>
                                    <tr>
                                        <asp:TextBox ID="txtSysDate" CssClass="textstylehidden" runat="server"> </asp:TextBox>
                                        <asp:HiddenField ID="HdnCntrl" runat="server" />
                                        <asp:HiddenField ID="hdnIds" runat="server" />
                                        <td class="pad5 center" style="height: 53px">
                                            <asp:Button ID="btncontinue" runat="server" Text="Continue" OnClick="btncontinue_Click" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnCancel_Click" />
                                            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClientClick="WindowPrint()" />&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <%--</div>--%>
</asp:Content>
