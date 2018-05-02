<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.Custom_Forms.frmReportsTouch"
    CodeBehind="frmReportsTouch.ascx.cs" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>

<div id="FormContent">
    <div id="tabs" style="width: 800px">
        <ul>
            <li><a href="#tab1">Reports</a></li>
        </ul>
        <div id="tab1" class="scroll-pane jspScrollable tabwidth" style="width: 811px; overflow: hidden;
            height: 380px;">
            <asp:UpdatePanel ID="updtreport" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
                        ReloadOnShow="true" runat="server" EnableShadow="true">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Move,Close" Skin="BlackMetroTouch" Width="1200px"
                                Height="550px" NavigateUrl="~/Touch/Custom Forms/frmReportView.aspx" Title="Report"
                                Modal="true">
                            </telerik:RadWindow>
                            <telerik:RadWindow ID="RadWindow2" runat="server" Skin="BlackMetroTouch" Behaviors="Maximize,Close" Width="1000px"
                                Height="550px" NavigateUrl="~/Touch/Custom Forms/frmLabHistory.aspx" Title="Lab History"
                                Modal="true">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                    <table style="width: 100%">
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <a href="#" onclick="openWin();return false;">Patient Visit Report</a>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <a href="#" onclick="openWin1();return false;">Patient Summary Report</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <a href="#" onclick="openLab();return false;">Patient Lab History</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <%--                        <tr>
                            <td>
                                <h3 align="center">
                                    CD4 Count and Viral Load over time</h3>
                                <chart:WebChartViewer ID="WebChartViewerCD4VL" runat="server" Height="400px" Width="300px" />
                            </td>
                            <td>
                                <h3 align="center">
                                    Weight and BMI over time</h3>
                                <chart:WebChartViewer ID="WebChartViewerWeight" runat="server" Height="400px" Width="300px" />
                            </td>
                        </tr>--%>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <telerik:RadWindow runat="server" ID="rwLabHistory" Skin="BlackMetroTouch" VisibleOnPageLoad="false" CssClass="preference" Title="Lab History"
        Modal="true" Width="880px" Height="550px" Behaviors="Move,Close">
        <ContentTemplate>
            <div id="divhistory">
                <asp:UpdatePanel ID="uptdpharmacyResults" runat="server">
                    <ContentTemplate>
                        <table style="width: 90%">
                            <tr>
                                <td colspan="2" align="right">
                                    <telerik:RadButton ID="btnprint" runat="server" OnClientClicked="PrintLabHistory"
                                        Text="Print" AutoPostBack="false">
                                    </telerik:RadButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color:Black"><asp:Label runat="server" ID="lblname"></asp:Label></span>
                                </td>
                                <td>
                                     <span style="color:Black"><asp:Label ID="lbldate" runat="server"></asp:Label></span>
                                </td>
                            </tr></table>
                            
                                    <telerik:RadGrid ID="rgvlabhistory" Skin="MetroTouch" AutoGenerateColumns="false"
                                        runat="server" ClientSettings-Scrolling-AllowScroll="false">
                                        <MasterTableView Font-Size="10" DataKeyNames="subTestName">
                                            <Columns>
                                                <telerik:GridBoundColumn SortExpression="subTestName" HeaderText="Lab Test Name"
                                                    HeaderButtonType="TextButton" DataField="subTestName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="TestResults" HeaderText="Result" HeaderButtonType="TextButton"
                                                    DataField="TestResults">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn SortExpression="Reportedbydate" HeaderText="Date Reported"
                                                    HeaderButtonType="TextButton" DataField="Reportedbydate">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                    
                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnprint" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <div id="frmReport_ScriptBlock" runat="server">
        <script type="text/javascript">
            function openWin() {
                var oWnd = radopen("Custom Forms/frmReportView.aspx?report=visit", "RadWindow1");
            }
            function openWin1() {
                var oWnd = radopen("Custom Forms/frmReportView.aspx?report=summary", "RadWindow1");
            }
            function openLab() {
                var oWnd = $find("IDfrmReportsTouch_rwLabHistory");
                oWnd.show();

            }
            function PrintLabHistory(FormName) {
                var previewWnd = window.open('about:blank', '', 'toolbar=0', false);
                var styleStr = "<html><head><title>Lab Report </title>";
                var htmlcontent = document.getElementById("divhistory");

                var htmlcontent = styleStr + "<body>" + htmlcontent.innerHTML.toString() + "</body></html>";

                previewWnd.document.open();
                previewWnd.document.write(htmlcontent);
                previewWnd.document.close();
                previewWnd.print();
                previewWnd.onfocus = function () { previewWnd.close(); }

            }

  </script>
    </div>
</div>
