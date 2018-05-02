<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLabHistory.aspx.cs"
    Inherits="PresentationApp.Touch.Custom_Forms.frmLabHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">

        function fnprint() {
            document.getElementById('btnprint').style.display = 'none';
            var printContent = document.getElementById("divhistory");

            var windowUrl = 'about:blank';

            var uniqueName = new Date();

            var windowName = 'Print' + uniqueName.getTime();
            alert("<html><head><title>Lost to Follow Up</title></head><body>" + printContent.innerHTML + "</body></html>");
//            var printWindow = window.open(windowUrl, windowName, 'scrollbars=1,width=1050,height=800', false);
//            alert("<html><head><title>Lost to Follow Up</title></head><body>" + printContent.innerHTML + "</body></html>");
//            printWindow.document.open();
//            
//            printWindow.document.write("<html><head><title>Lost to Follow Up</title></head><body>" + printContent.innerHTML + "</body></html>");

//            printWindow.document.close();

//            printWindow.focus();

//            printWindow.print();

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divhistory">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <asp:UpdatePanel ID="uptdpharmacyResults" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td colspan="2" align="right">
                            <telerik:RadButton ID="btnprint" runat="server" OnClientClicked="fnprint" AutoPostBack="false" Text="Print">
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblname"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbldate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <telerik:RadGrid ID="rgvlabhistory" Skin="MetroTouch" AutoGenerateColumns="false"
                                runat="server" Culture="(Default)">
                                <MasterTableView PageSize="5" Font-Size="10" DataKeyNames="subTestName">
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
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnprint" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
