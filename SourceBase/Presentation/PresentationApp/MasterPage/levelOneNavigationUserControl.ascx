<%@ Control Language="C#" AutoEventWireup="true" Inherits="MasterPage_levelOneNavigationUserControl"
    CodeBehind="levelOneNavigationUserControl.ascx.cs" %>
<div runat="server" id="divmenu1">
    <script type="text/javascript">

        function openWin() {
            var path = '/frmDBBackupSetup.aspx';
            if (window.location.href.toLowerCase().indexOf("iqcare") > -1) {
                path = '/IQCare' + path;
            }
            window.open(path, '', 'ToolBar=no,Location=no,status=yes,scrollbars=no,top=200,left=300,resizable=no,width=450,height=500');
        }

    </script>
</div>
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td valign="bottom">
                            <asp:Menu ID="mainMenu" runat="server" Orientation="Horizontal" CssClass="levelOneMenu"
                                Width="570px" RenderingMode="Table" StaticEnableDefaultPopOutImage="False" StaticSubMenuIndent=""
                                StaticSelectedStyle-ItemSpacing="0px" StaticMenuItemStyle-ItemSpacing="0px">
                                <StaticMenuItemStyle CssClass="levelOneButton" Height="24px" />
                                <DynamicMenuItemStyle CssClass="levelOneDropDown" Height="24px" HorizontalPadding="25px" />
                                <DynamicHoverStyle CssClass="levelOneDropDownHover" />
                                <StaticHoverStyle CssClass="levelOneMenuHover" />
                                <Items>
                                    <asp:MenuItem Text="Select Service" Value="Facility Home" NavigateUrl="~/frmFacilityHome.aspx">
                                    </asp:MenuItem>
                                    <asp:MenuItem Text="Reports" Value="Reports" Selectable="False">
                                       <%-- <asp:MenuItem Text="Facility Reports" Enabled="true" Value="Facility Reports" NavigateUrl="~/Reports/frmReportFacilityJump.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="Donor Reports" Enabled="true" Value="Donor Reports" NavigateUrl="~/Reports/frmReportDonorJump.aspx">
                                        </asp:MenuItem>--%>
                                        <asp:MenuItem Text="Custom Reports" Value="Custom Reports" NavigateUrl="~/Reports/frmReportCustom.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="Query Builder Reports" Value="Query Builder Reports" NavigateUrl="~/Reports/frmQueryBuilderReports.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="IQTools Reports" Value="IQToolsReportsmain" Selectable="False">
                                            <asp:MenuItem Text="IQTools Query" Value="IQToolsQuery" NavigateUrl="~/IQTools/frmIQToolsQuery.aspx">
                                            </asp:MenuItem>
                                            <asp:MenuItem Text="IQTools Reports" Value="IQToolsReports" NavigateUrl="~/IQTools/frmIQToolsReports.aspx">
                                            </asp:MenuItem>
                                            <asp:MenuItem Text="Refresh IQTools Cache" Value="Refresh IQTools Cache" NavigateUrl="~/frmSystemCache.aspx?Code=2">
                                            </asp:MenuItem>
                                        </asp:MenuItem>
                                    </asp:MenuItem>
                                    <asp:MenuItem Text="Scheduler" Value="Scheduler" NavigateUrl="~/Scheduler/frmScheduler_AppointmentMain.aspx">
                                    </asp:MenuItem>
                                    <asp:MenuItem Text="Administration" Value="Administration" Selectable="False" NavigateUrl="~/frmFacilityHome.aspx">
                                        <asp:MenuItem Text="Facility Setup" Value="Facility Setup" NavigateUrl="~/AdminForms/frmAdmin_FacilityList.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="Customize Lists" Value="Customize Lists" NavigateUrl="~/AdminForms/frmAdmin_PMTCT_CustomItems.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="User Administration" Value="User Administration" NavigateUrl="~/AdminForms/frmAdmin_UserList.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="User Group Administration" Value="User Group Administration"
                                            NavigateUrl="~/AdminForms/frmAdmin_UserGroupList.aspx"></asp:MenuItem>
                                        <asp:MenuItem Text="Delete Patient" Value="Delete Patient" NavigateUrl="~/frmFindAddPatient.aspx?mnuClicked=DeletePatient">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="Configure Custom Fields" Value="Configure Custom Fields" NavigateUrl="~/AdminForms/frmConfig_Customfields.aspx">
                                        </asp:MenuItem>
                                        <%--<asp:MenuItem Text="Export" Value="Export" NavigateUrl="~/AdminForms/frmAdmin_Export.aspx">
                                        </asp:MenuItem>--%>
                                        <asp:MenuItem Text="Audit Trail" Value="AuditTrail" NavigateUrl="~/AdminForms/frmAdmin_AuditTrail.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="Refresh System Cache" Value="Refresh System Cache" NavigateUrl="~/frmSystemCache.aspx?Code=1">
                                        </asp:MenuItem>
                                    </asp:MenuItem>
                                    <asp:MenuItem Text="Back Up" Value="Back Up" Selectable="False" NavigateUrl="~/frmDBBackup.aspx">
                                        <asp:MenuItem Text="Backup/Restore Database" Value="Backup/Restore Database" NavigateUrl="~/frmDBBackup.aspx">
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="Backup/Restore setup" Value="Backup/Restore setup" NavigateUrl="javascript:openWin();"
                                            Target="_self"></asp:MenuItem>
                                    </asp:MenuItem>
                                    <asp:MenuItem Text="Facility Statistics" Value="Facility Statistics" NavigateUrl="~/frmFacilityStatistics.aspx">
                                    </asp:MenuItem>
                                </Items>
                                <StaticSelectedStyle ItemSpacing="0px"></StaticSelectedStyle>
                            </asp:Menu>
                        </td>
                        <td valign="bottom">
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/menu_end.png" Height="26px"
                                Style="margin-left: 0px" Width="30px" ImageAlign="AbsBottom" BorderStyle="None"
                                BorderWidth="0px" />
                        </td>
                        <td class="breadcrumb" valign="bottom" height="21" wrap="nowrap">
                            <!-- Modified 18June 2007 (3) -->
                            <a class="breadcrumb">
                                <asp:Label ID="lblRoot" runat="server" Text=""></asp:Label></a> <a class="breadcrumb">
                                    <asp:Label ID="lblheader" runat="server"></asp:Label></a>
                            <!-- Modified 18June 2007 (3) -->
                        </td>
                    </tr>
                </table>
            </div>
        </td>
        <%--   <td align="right">
            <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True">Help</asp:LinkButton>
            &nbsp;|
            <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True">Password</asp:LinkButton>
            &nbsp;|
            <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True">Report Defect</asp:LinkButton>
            &nbsp;|
            <asp:LinkButton ID="LinkButton4" runat="server" Font-Bold="True">Logout</asp:LinkButton>
            &nbsp;
        </td>--%>
    </tr>
</table>
