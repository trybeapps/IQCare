<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true"
    Inherits="frmFindAddPatient" Title="Untitled Page"
    EnableViewState="true" Codebehind="frmFindAddPatient.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function fnSetSession(url) { 
            var result = frmFindAddPatient.SetPatientId_Session(url).value;
            window.location.href = result;
        }
        function fnSetSessionfamily(url) {
            var result = frmFindAddPatient.SetPatientIdFamily_Session(url).value;
            window.location.href = result;
        }

        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
            var tbl = document.getElementById(gridId);
            if (tbl) {
                var DivHR = document.getElementById('DivHeaderRow');
                var DivMC = document.getElementById('DivMainContent');
                var DivFR = document.getElementById('DivFooterRow');

                //*** Set divheaderRow Properties ****
                DivHR.style.height = headerHeight + 'px';
                DivHR.style.width = (parseInt(width) - 16) + 'px';
                DivHR.style.position = 'relative';
                DivHR.style.top = '0px';
                DivHR.style.zIndex = '10';
                DivHR.style.verticalAlign = 'top';

                //*** Set divMainContent Properties ****
                DivMC.style.width = width + 'px';
                DivMC.style.height = height + 'px';
                DivMC.style.position = 'relative';
                DivMC.style.top = -headerHeight + 'px';
                DivMC.style.zIndex = '1';

                //*** Set divFooterRow Properties ****
                DivFR.style.width = (parseInt(width) - 16) + 'px';
                DivFR.style.position = 'relative';
                DivFR.style.top = -headerHeight + 'px';
                DivFR.style.verticalAlign = 'top';
                DivFR.style.paddingtop = '2px';

                if (isFooter) {
                    var tblfr = tbl.cloneNode(true);
                    tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
                    var tblBody = document.createElement('tbody');
                    tblfr.style.width = '100%';
                    tblfr.cellSpacing = "0";

                    tblfr.border = "0px";
                    tblfr.rules = "none";
                    //*****In the case of Footer Row *******
                    tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
                    tblfr.appendChild(tblBody);
                    DivFR.appendChild(tblfr);
                }
                //****Copy Header in divHeaderRow****
                DivHR.appendChild(tbl.cloneNode(true));
            }
        }



        function OnScrollDiv(Scrollablediv) {

            //document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
            //document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
        }
    </script>
    <div style="padding-top: 10px">
        <h1 class="nomargin" id="tHeading" runat="server" style="padding-left: 10px;">
            Find/Add Patient</h1>
        <div class="center" style="padding: 10px;">
            <div id="divshow" class="border formbg">
              <%--  <asp:ScriptManager ID="scm_patient" runat="server">
                </asp:ScriptManager> --%>
                <asp:UpdatePanel ID="up_patient" runat="server">
                    <ContentTemplate>
                        <!-- Begin Stacey's code -->
                        <table align="center" width="100%" border="1">
                            <tr>
                                <td class="border pad5 whitebg" valign="top" width="280" colspan="2" align="left">
                                    <table width="350" border="0">
                                        <tr>
                                            <td align="left" >
                                                <label id="lblidentificationno" class="center" runat="server">
                                                    Patient Identification Number:</label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtidentificationno" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap>
                                                <label>
                                                    Facility/Satellite:</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddFacility" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Service:</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlServices" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="border pad5 whitebg" valign="top" width="250" colspan="2" align="left">
                                    <table width="230" border="0">
                                        <tr>
                                            <td nowrap="nowrap">
                                                <label for="lastname">
                                                    Last Name:</label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtlastname" runat="server"></asp:TextBox><a></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap">
                                                <label runat="server" id="lblmiddlename" for="middlename">
                                                    Middle Name:</label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmiddlename" runat="server"></asp:TextBox><a></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="nowrap">
                                                <label for="firstname">
                                                    First Name:</label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtfirstname" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%--</table>     
                                --%>
                                <td id="tdPatientOtherDetails" class="border pad5 whitebg" colspan="2" runat="server"
                                    align="left">
                                    <table width="320" border="0">
                                        <tr>
                                            <td>
                                                <label>
                                                    Date of Birth:</label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDOB" runat="server" MaxLength="11" Width="75px"></asp:TextBox>
                                                <img onclick="w_displayDatePicker('<%= txtDOB.ClientID %>');" height="22" alt="Date Helper"
                                                    hspace="3" src="./images/cal_icon.gif" width="22" border="0" style="vertical-align:text-bottom;" />
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label for="Sex">
                                                    Sex:</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddSex" runat="server">
                                                    <asp:ListItem Selected="True" Value="">-Select-</asp:ListItem>
                                                    <asp:ListItem Value="16">Male</asp:ListItem>
                                                    <asp:ListItem Value="17">Female</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label for="Careendedstatus">
                                                    Patient Status:</label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddCareEndedStatus" runat="server">
                                                    <asp:ListItem Selected="True" Value="">-Select-</asp:ListItem>
                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                    <asp:ListItem Value="1">Care Ended</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%-- </td>--%>
                            </tr>
                        </table>
                        <!-- End Stacey's code -->
                        <table class="center" cellspacing="6" cellpadding="0" width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" align="center">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" Text=" Find " />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add Patient" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Back" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="center" align="center" style="padding-left: 10px; padding-right: 15px"
                                        colspan="2">
                                        <div class="grid">
                                            <div class="rounded">
                                                <div class="top-outer">
                                                    <div class="top-inner">
                                                        <div class="top">
                                                            <h2>
                                                                Searched Result</h2>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="mid-outer">
                                                    <div class="mid-inner">
                                                        <div class="mid" style="cursor: pointer; height: 280px; overflow: auto;border: 1px solid #666699;">
                                                            <%--<div style="overflow: hidden;" id="DivHeaderRow"> comment by deepak(New code for fixed header)
                                                        </div>--%>
                                                            <div id="div-gridview" class="whitebg" >
                                                                <!-- Content Goes Here! -->
                                                                <asp:GridView ID="grdSearchResult" runat="server" EnableViewState="False" Width="884"
                                                                    OnRowDataBound="grdSearchResult_RowDataBound" OnSorting="grdSearchResult_Sorting"
                                                                    PageSize="1" CssClass="datatable" AutoGenerateColumns="False" CellPadding="0"
                                                                    CellSpacing="0" BorderWidth="0" GridLines="None" AllowSorting="True">
                                                                      <HeaderStyle CssClass="searchresultfixedheader" Height="20px"></HeaderStyle>
                                                                    <%-- <AlternatingRowStyle BackColor="White" BorderColor="Silver" />--%>
                                                                    <RowStyle CssClass="row" />
                                                                    <%--  <HeaderStyle CssClass="SearchResultFixedHeader" />--%>
                                                                </asp:GridView>
                                                            </div>
                                                            <%--<div id="DivFooterRow" style="overflow: hidden">
                                                        </div>--%>
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
                                        <input id="rdoExact" class="textstylehidden" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);"
                                            type="radio" value="0" name="dobPrecision" runat="Server" />
                                        <input id="rdoEstimated" class="textstylehidden" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);"
                                            type="radio" value="1" name="dobPrecision" runat="Server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnView"></asp:PostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdSearchResult"></asp:AsyncPostBackTrigger>
                        <%--<asp:PostBackTrigger ControlID="btnAdd"></asp:PostBackTrigger>--%>
                        <asp:PostBackTrigger ControlID="btnCancel"></asp:PostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
