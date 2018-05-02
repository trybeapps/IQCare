<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmScheduler_AppointmentNew"
    Title="Untitled Page" Codebehind="frmScheduler_AppointmentNew.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <%--<link type="text/css" href="../Style/_assets/css/grid.css" rel="stylesheet" />
<link type="text/css" href="../Style/_assets/css/round.css" rel="stylesheet" />--%>
    <%--  <form id="appointmentnew" method="post" runat="server" enableviewstate="true" title="patientAppointmentNew">
    --%>
    <div style="padding-top: 20px;padding-left:10px;padding-right:10px;">
        <!--Modified 20June 2007 (6)-->
        <script language="javascript" type="text/javascript">
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
        </script>
        <!--Modified 20June 2007 (6)-->
        <h1 class="nomargin" id="header" runat="server">
            New Appointment</h1>
        <div class="border center formbg">
            <table class="center" width="100%" border="0" cellpadding="0" cellspacing="6">
                <%-- <tr>
                    <td class="form" align="center" colspan="2">
                        <label class="patientInfo">
                            Patient Name:</label>
                        <asp:Label ID="lblPatientNameValue" runat="server" Text="Mary Longlastname"></asp:Label>
                        <label id="lblpatientenrol" class="patientInfo" runat="server" ></label>
                        <asp:Label ID="lblPatientEnrollmentNoValue" runat="server" Text="444545"></asp:Label>
                        <label id="lblExisclinicID" class="patientInfo" runat="server"></label>
                        <asp:Label ID="lblHospitalNoValue" runat="server" Text="12345678-444444-596A"></asp:Label>
                       <%-- <label class="patientInfo">
                            Sex:</label>
                        <asp:Label ID="lblPatientGenderValue" runat="server" Text="female"></asp:Label>--%>
                <%--</td>--%>
                <%--</tr>--%>
                <tr>
                    <td class="border pad5 whitebg" valign="top" width="50%" style="height: 39px">
                        <label class="required">
                            *Appointment Date:</label>
                        <asp:TextBox ID="txtAppDate" MaxLength="11" runat="server"></asp:TextBox>
                        <img src="../images/cal_icon.gif" height="22" alt="Date Helper" onclick="w_displayDatePicker('<%= txtAppDate.ClientID %>');" />
                        <span class="smallerlabel">(DD-MMM-YYYY)</span>
                    </td>
                    <td class="border pad5 whitebg" align="center" width="50%" style="height: 39px">
                        <label class="required">
                            *Purpose:</label>
                        <asp:DropDownList ID="ddAppPurpose" runat="server" AutoPostBack="false" Width="125px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="border pad5 whitebg" align="center" colspan="2" style="height: 32px">
                        <label class="right15 required" style="width: 17%">
                            *Provider:&nbsp;</label><asp:DropDownList ID="ddAppProvider" runat="server" AutoPostBack="false"
                                Width="125px">
                            </asp:DropDownList>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="border pad5 whitebg" align="center" colspan="2" style="height: 24px">
                        <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" Enabled="True" />
                        <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_Click"
                            Visible="false" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Enabled="True" />
                        <asp:Button ID="theBtn" Text="OK" CssClass="textstylehidden" runat="server" OnClick="theBtn_Click" />
                    </td>
                </tr>
                <tr>
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
                                        <div class="mid">
                                            <div id="div-gridview" class="GridView whitebg" style="height: 300px">
                                                <asp:GridView ID="grdSearchResult" AllowSorting="True" runat="server" CssClass="datatable"
                                                    CellPadding="0" CellSpacing="0" AutoGenerateColumns="False" Width="100%" PageSize="1"
                                                    BorderWidth="0" GridLines="None" OnSorting="grdSearchResult_Sorting" OnRowDataBound="grdSearchResult_RowDataBound"
                                                    OnSelectedIndexChanging="grdSearchResult_SelectedIndexChanging">
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
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
    </script>
</asp:Content>
