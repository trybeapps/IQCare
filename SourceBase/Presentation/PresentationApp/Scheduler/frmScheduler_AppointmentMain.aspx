<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmScheduler_AppointmentMain"
    Title="Untitled Page" Codebehind="frmScheduler_AppointmentMain.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <%-- <link type="text/css" href="../Style/_assets/css/grid.css" rel="stylesheet" />
    <link type="text/css" href="../Style/_assets/css/round.css" rel="stylesheet" />--%>
    <%--<form id="appointment" method="post" runat="server" enableviewstate="true" title="patientAppointment">--%>
    <div style="padding-top: 20px;">
        <script language="javascript" type="text/javascript">

            //Modified 20June 2007 (5)
            function CheckDate(vDateName) {

                var mYear = vDateName.value.substr(7, 4)
                if (mYear != '') {
                    if (mYear < 1930) {
                        alert("Selected year should be between 1930 and Current year. Reenter..");
                        vDateName.value = "";
                        vDateName.focus();

                    }
                }
            }
            function fnredirect(url) {

                window.location.href = url;
            }
            //Modified 20June 2007 (5)
            function ResetDateAndGrid(YearVal, MonthVal, DayVal, previousFormName) {
                var BYearVal = YearVal;
                if (MonthVal = 1) {
                    BYearVal = BYearVal - 1;
                }
                var ddlStatus = document.getElementById('<%=ddAppointmentStatus.ClientID %>')[document.getElementById('<%=ddAppointmentStatus.ClientID %>').selectedIndex].value;
                var fromDate = document.getElementById('<%= txtFrom.ClientID %>');
                var toDate = document.getElementById('<%= txtTo.ClientID %>');
                var grid = document.getElementById('<%=grdSearchResult.ClientID %>');

                if (DayVal < 10) {
                    DayVal = "0" + DayVal;
                }
                var months = new Array(13);
                var currDate = new Date();

                months[0] = "Jan";
                months[1] = "Feb";
                months[2] = "Mar";
                months[3] = "Apr";
                months[4] = "May";
                months[5] = "Jun";
                months[6] = "Jul";
                months[7] = "Aug";
                months[8] = "Sep";
                months[9] = "Oct";
                months[10] = "Nov";
                months[11] = "Dec";

                MonthVal = MonthVal - 1;
                currDate = DayVal + "-" + months[MonthVal] + "-" + YearVal;
                if ((previousFormName == 'FacilityHomePending') && (ddlStatus == 12)) {

                    fromDate.value = currDate;
                    toDate.value = currDate;

                }
                else if (ddlStatus == 15) {
                    //Calculate the date of 1 month previous to current date

                    newDate = new Date(YearVal, MonthVal, DayVal);

                    var mnth = newDate.getMonth();
                    var yr = newDate.getYear();

                    var day = newDate.getDate();
                    var daydiff = 0;

                    if (DayVal > 30) {
                        day = DayVal - 30;
                    }
                    else {
                        daydiff = 30 - DayVal;
                        if (daydiff == 0) {
                            day = 1;
                        }
                        else {
                            if (mnth == 0) {
                                mnth = 11;
                                yr = yr - 1;
                            }
                            else {
                                mnth = mnth - 1;
                            }
                            daysinmonth = 32 - new Date(yr, mnth, 32).getDate();
                            day = daysinmonth - daydiff;
                        }
                    }
                    var monthname = months[mnth];
                    var dt = new Date();

                    if (day < 10) {
                        day = "0" + day;
                    }

                    dt = day + "-" + monthname + "-" + BYearVal;
                    fromDate.value = dt;
                    toDate.value = currDate;

                }
                else {
                    fromDate.value = " ";
                    toDate.value = " ";
                }
                //grid.innerText = "";
            } 
              
        </script>
        <h1 class="nomargin" style="padding-left: 10px;">
            Appointments</h1>
        <div class="center" style="padding: 5px;">
            <div id="divApp" class="border center formbg">
                <asp:UpdatePanel ID="up_appointment" runat="server">
                    <ContentTemplate>
                        <table width="100%" border="0" cellpadding="0" cellspacing="6">
                            <table border="0" cellpadding="0" cellspacing="6" width="100%">
                                <tr>
                                    <td align="left" width="50%">
                                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" OnClick="btnExcel_Click"
                                            Text="Export to Excel" />
                                    </td>
                                    <td align="right" width="50%">
                                        <asp:Button ID="btnNewAppointment" runat="server" Font-Bold="true" OnClick="btnNewAppointment_Click1"
                                            Text="New Appointment" />
                                    </td>
                                </tr>
                            </table>
                            <tr>
                                <td class="border pad5 whitebg" align="right">
                                    <table class="center" width="100%" border="0" cellpadding="0" cellspacing="6">
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:UpdateProgress runat="server" ID="updateProgress" AssociatedUpdatePanelID="up_appointment">
                                                    <ProgressTemplate>
                                                        Searching....<asp:Image runat="server" ID="imggif" ImageUrl="~/Images/loading.gif"
                                                            ImageAlign="AbsMiddle" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="border pad5 whitebg" valign="top" width="30%">
                                                <label>
                                                    Appointment Status:</label>
                                                <asp:DropDownList ID="ddAppointmentStatus" runat="server" Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="border pad5 whitebg" id="tdDate" runat="server" visible="true" valign="top"
                                                width="70%">
                                                <label class="margin20">
                                                    Date Range:</label>
                                                <span class="smallerlabel margin10">From:</span>
                                                <asp:TextBox ID="txtFrom" MaxLength="11" runat="server" Width="70px"></asp:TextBox>
                                                <img src="../images/cal_icon.gif" height="22" alt="Date Helper" onclick="w_displayDatePicker('<%= txtFrom.ClientID %>');" />
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span> <span class="smallerlabel margin15">
                                                    To:</span>
                                                <asp:TextBox ID="txtTo" MaxLength="11" runat="server" Width="70px"></asp:TextBox>
                                                <img src="../images/cal_icon.gif" height="22" alt="Date Helper" onclick="w_displayDatePicker('<%= txtTo.ClientID %>');" />
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="border pad5 whitebg" align="center">
                                    <asp:Button ID="btnSubmit" runat="server" Text="View" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                                    <asp:HiddenField ID="hidappointment" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <br />
                                <tr>
                                    <br />
                                    <td class="border pad5 whitebg" valign="top" colspan="2">
                                        <div class="grid">
                                            <div class="rounded">
                                                <div class="top-outer">
                                                    <div class="top-inner">
                                                        <div class="top">
                                                            <h2>
                                                                Appointments</h2>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="mid-outer">
                                                    <div class="mid-inner">
                                                        <div class="mid" style="height: 300px; overflow: auto">
                                                            <div id="div-gridview" class="GridView whitebg">
                                                                <asp:GridView ID="grdSearchResult" AllowSorting="True" runat="server" CssClass="datatable"
                                                                    CellPadding="0" CellSpacing="0" Width="100%" PageSize="1" AutoGenerateColumns="false"
                                                                    OnSorting="grdSearchResult_Sorting" BorderWidth="0" GridLines="None" OnRowDataBound="grdSearchResult_RowDataBound">
                                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                                    <%--<AlternatingRowStyle BackColor="White" BorderColor="White" />--%>
                                                                    <RowStyle CssClass="row" />
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="bottom-outer">
                                                    <div class="bottom-inner">
                                                        <div class="bottom">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!--<input class="textstylehidden" type="radio" id="rdoExact"  name="dobPrecision"  value="0"  onfocus="up(this);" onclick="down(this);"  runat="Server"/> 
                        <input class="textstylehidden" type="radio" id="rdoEstimated"  name="dobPrecision" value="1" onfocus="up(this);" onclick="down(this);" runat="Server"/> -->
                                    </td>
                                </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                        <asp:PostBackTrigger ControlID="btnBack" />
                        <asp:PostBackTrigger ControlID="btnExcel" />
                    </Triggers>
                </asp:UpdatePanel>
                <script language="javascript" type="text/javascript">
                    if (typeof (Sys) !== 'undefined')
                        Sys.Application.notifyScriptLoaded();
                    var pageManager = Sys.WebForms.PageRequestManager.getInstance();
                    var uiId = '';
                    pageManager.add_beginRequest(myBeginRequestCallback);
                    function myBeginRequestCallback(sender, args) {
                        var postbackElem = args.get_postBackElement();
                        uiId = postbackElem.id;
                        postbackElem.disabled = true;
                        document.getElementById('divApp').disabled = "disabled";

                    }
                    pageManager.add_endRequest(myEndRequestCallback);

                    function myEndRequestCallback(sender, args) {
                        var hidbox = $get('<%=hidappointment.ClientID %>');
                        document.getElementById('divApp').disabled = "";
                        if (hidbox.value == "No") {

                            document.getElementById('<%=txtFrom.ClientID %>').value = "";
                            document.getElementById('<%=txtTo.ClientID %>').value = "";
                            document.getElementById('<%=ddAppointmentStatus.ClientID %>').selectedIndex = 0;
                            alert("No Record Found");
                        }
                    }
                </script>
            </div>
        </div>
        <br />
    </div>
</asp:Content>
