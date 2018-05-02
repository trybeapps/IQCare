<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmPatientRegistration" Title="Untitled Page" Codebehind="frmPatientRegistration.aspx.cs" %>

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
                document.getElementById(txtAge).value = Math.round((dateDiff("d", dt1, dt2, "", "") / 365)) - 1;
                var yr = Math.round(dateDiff("d", dt1, dt2, "", "") / 365) - 1;
                document.getElementById(txtmonth).value = Math.round((dateDiff("d", dt1, dt2, "", "") - (365 * yr)) / 30);
            }
            else {
                document.getElementById(txtAge).value = Math.round((dateDiff("d", dt1, dt2, "", "") / 365));
                var yr = Math.round(dateDiff("d", dt1, dt2, "", "") / 365);
                document.getElementById(txtmonth).value = Math.round((dateDiff("d", dt1, dt2, "", "") - (365 * yr)) / 30);
            }
        }

        function Button1_onclick() {

        }

        function Button2_onclick() {

        }

        function Button1_onclick() {

        }

        function Button2_onclick() {

        }

        function fnshow() {

            
                var fname = document.getElementById('<%=txtfirstName.ClientID %>').value;
                var mname = "";
                var lname = document.getElementById('<%=txtlastName.ClientID %>').value;
                var address = "";
                var phone = "";
                if (fname != '') {
                    var result = frmPatientRegistration.GetDuplicateRecord(fname, mname, lname, address, phone).value;
                    if (result != "") {
                        document.getElementById('search_popup').style.display = 'inline';
                        document.getElementById('showresult').innerHTML = result;
                    }
                    else {
                        document.getElementById('search_popup').style.display = 'none';
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

        function checkLoadedObjects(evt) {
            if (pickedUp[1] == true) {
                var currentSelection = document.getElementById(pickedUp[0]);

                currentSelection.style.position = "absolute";
                currentSelection.style.top = (evt.clientY + 1) + "px";
                currentSelection.style.left = (evt.clientX + 1) + "px";
            }
        }

        function dropLoadedObject(evt) {
            if (pickedUp[1] == true) {
                var currentSelection = document.getElementById(pickedUp[0]);
                currentSelection.style.position = "absolute";
                currentSelection.style.top = (evt.clientY + 1) + "px";
                currentSelection.style.left = (evt.clientX + 1) + "px";

                pickedUp = new Array("", false);
            }

        }
        function WindowPrint() {
            window.print();
        }
        function CheckFileExistence() {

            var filePath = document.getElementById('<%= this.ImgfileUploader.ClientID %>').value;
            var valid = true; //var barvalid = true;

            if (filePath.length > 0) {

                var filext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
                var validExtensions = new Array();
                validExtensions[0] = 'jpg';
                validExtensions[1] = 'jpeg';
                for (var i = 0; i < validExtensions.length; i++) {
                    if (filext == validExtensions[i])
                    { valid = true; }

                }
            }


            if (valid == false) {
                alert('The file extension is not allowed!');
                return false
            }

        }

    </script>
    <div style="padding-top: 1px;">
        <asp:UpdatePanel ID="UpdateMasterLink" runat="server">
            <ContentTemplate>
                <h1>
                    IQCare Registration</h1>
                <div class="border center formbg">
                    <div class="popupWindow" id='search_popup' onclick="javascript:getReadyToMove('search_popup', event);"
                        name='styled_popup' style="display: none;">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" border="0">
                            <tr bgcolor="#666699">
                                <td align="right">
                                    <span style="cursor: hand" onclick="jsAreaClose('search_popup')">
                                        <img alt="Hide Popup" src="../Images/close.gif" border="0"></span>
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
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="border pad5 whitebg">
                                    <label id="lblPName" class="required" for="patientname">
                                        *Patient Name:</label>
                                    <span id="FName" class="smallerlabel">First: </span>
                                    <asp:TextBox ID="txtfirstName" MaxLength="50" runat="server"></asp:TextBox>
                                    <span id="MName" class="smallerlabel">Middle: </span>
                                    <asp:TextBox ID="txtmiddleName" MaxLength="50" runat="server"></asp:TextBox>
                                    <span id="LName" class="smallerlabel">Last: </span>
                                    <asp:TextBox ID="txtlastName" onchange="fnshow();" onblur="fnshow();" MaxLength="50"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td class="border pad5 whitebg" width="30%">
                                    <label id="lblIQRef">
                                        IQCare Reference:</label>
                                    <asp:TextBox ID="txtIQCareRef" MaxLength="11" runat="server" Width="60%" TabIndex="100"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="border pad5 whitebg" nowrap="nowrap">
                                    <label id="lblgender" class="required" for="gender">
                                        *Sex:</label>
                                    <asp:DropDownList ID="ddgender" runat="server">
                                    </asp:DropDownList>
                                    <label id="lblDOB" class="required margin15" for="DOB">
                                        *Date of Birth:</label>
                                    <asp:TextBox ID="TxtDOB" MaxLength="11" runat="server" Width="70px"></asp:TextBox>
                                    <img onclick="w_displayDatePicker('<%= TxtDOB.ClientID %>');" height="22" alt="Date Helper"
                                        hspace="3" src="./images/cal_icon.gif" width="20" border="0">
                                    <span class="smallerlabel">DD-MMM-YYYY </span>
                                    <input id="rbtndobPrecExact" onmouseup="up(this);" onfocus="up(this);" onclick="down(this)" type="radio"
                                        value="1" name="dobPrecision" runat="server">
                                    <span class="smalllabel">Exact </span>
                                    <input id="rbtndobPrecEstimated" onmouseup="up(this);" onfocus="up(this);" onclick="down(this)" type="radio"
                                        value="0" name="dobPrecision" runat="server">
                                    <span class="smalllabel">Estimated</span>
                                </td>
                                <td class="border pad5 whitebg">
                                    <label for="Age">
                                        Age:</label>
                                    <asp:TextBox ID="txtageCurrentYears" ReadOnly="true" MaxLength="2" runat="server"
                                        Width="10%"></asp:TextBox>
                                    <span class="smallerlabel">yrs</span>
                                    <asp:TextBox ID="txtageCurrentMonths" ReadOnly="true" MaxLength="2" runat="server"
                                        Width="10%"></asp:TextBox>
                                    <span class="smallerlabel">mths</span>
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
                                                <asp:TextBox ID="txtRegDate" runat="server" MaxLength="11" Width="70px"></asp:TextBox>
                                                <img alt="Date Helper" border="0" height="22" hspace="3" onclick="w_displayDatePicker('<%= txtRegDate.ClientID %>');"
                                                    src="images/cal_icon.gif" width="20"> <span class="smallerlabel">DD-MMM-YYYY </span>
                                                </img>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
                <div class="border center formbg">
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="border pad5 whitebg" width="45%" style="height: 100px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%" colspan="2" align="center">
                                                <img runat="server" id="imgpatient" src="~/PatientImages/nouser.jpg" width="135"
                                                    height="150" style="border: #666699 1px solid;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 32%" align="left">
                                                <label for="lblfileuploader">
                                                    Image Upload:</label>
                                            </td>
                                            <td style="width: 68%" align="left">
                                                <asp:FileUpload ID="ImgfileUploader" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="border pad5 whitebg" width="55%" style="height: 100px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%" colspan="2" align="center">
                                                <img runat="server" id="imgbarcode" style="border: #666699 1px solid;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%" align="left">
                                            </td>
                                            <td style="width: 60%">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
                <div class="border center formbg">
                    <br>
                    <h2 class="forms" align="left">
                        Contact Information</h2>
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="border pad5 whitebg" width="50%" style="height: 34px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label for="localCouncil">
                                                    Chief/Local Council:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtlocalCouncil" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="border pad5 whitebg" width="50%" style="height: 34px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label id="lblvillageName" for="villageName">
                                                    Village/Town/City Name:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:DropDownList ID="ddvillageName" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="border pad5 whitebg">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label id="lbldistrictName" for="districtName">
                                                    District/ County/Ward:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:DropDownList ID="dddistrictName" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="border pad5 whitebg">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label for="province">
                                                    Province/State:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:DropDownList ID="ddProvince" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="border pad5 whitebg">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label for="address">
                                                    Residence/Address/PO Box:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtaddress" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="border pad5 whitebg">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label for="phone">
                                                    Phone Number:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtphone" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <h2 class="forms" align="left">
                        Emergency Contact Information</h2>
                    <table cellspacing="6" cellpadding="0" width="100%" border="0">
                        <tbody>
                            <tr>
                                <td class="border pad5 whitebg" width="50%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label id="lblemergContactName" for="emergContactName">
                                                    Emergency Contact Name:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtemergContactName" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="border pad5 whitebg" width="50%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label for="emergContactRelation">
                                                    Emergency Contact Relationship:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:DropDownList ID="ddEmergContactRelation" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="border pad5 whitebg" style="height: 34px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label for="emergContactPhone">
                                                    Emergency Contact Phone Number:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtemergContactPhone" runat="server"> </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="border pad5 whitebg" style="height: 34px">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%" align="right">
                                                <label for="emergContactAddress">
                                                    Emergency Contact Address:</label>
                                            </td>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtemergContactAddress" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br>
                <div class="border center formbg">
                    <table cellspacing="6" cellpadding="0" width="100%">
                        <tbody>
                            <tr>
                                <asp:TextBox ID="txtSysDate" CssClass="textstylehidden" runat="server"> </asp:TextBox>
                                <td class="pad5 center" colspan="2" style="height: 53px">
                                    <asp:Button ID="btncontinue" runat="server" Text="Continue" OnClientClick="return CheckFileExistence()"
                                        OnClick="btncontinue_Click" />
                                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" Visible="False"
                                        OnClientClick="return CheckFileExistence()" />
                                    <asp:Button ID="btncomplete" runat="server" Text="Data quality check" OnClick="btncomplete_Click"
                                        Visible="False" OnClientClick="return CheckFileExistence()" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnCancel_Click" />
                                    <asp:Button ID="btnPrint" Text="Print" runat="server" OnClientClick="WindowPrint()" />&nbsp;&nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnsave"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncomplete"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btncontinue"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        if (typeof (Sys) !== 'undefined')
            Sys.Application.notifyScriptLoaded();
        var pageManager = Sys.WebForms.PageRequestManager.getInstance();
        var uiId = '';
        pageManager.add_beginRequest(myBeginRequestCallback);
        function myBeginRequestCallback(sender, args) {
            ;
            var postbackElem = args.get_postBackElement();
            uiId = postbackElem.id;
            postbackElem.disabled = true;

        }
        pageManager.add_endRequest(myEndRequestCallback);
        function myEndRequestCallback(sender, args) {
            var status = '<%=Request.QueryString["name"]%>';
            if (status != 'Edit') {
                fnshow();
            }
        }
    </script>
</asp:Content>
