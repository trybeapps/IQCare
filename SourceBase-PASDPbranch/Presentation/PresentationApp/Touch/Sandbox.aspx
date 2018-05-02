<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sandbox.aspx.cs" Inherits="PresentationApp.Touch.Sandbox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Jquery standard files -->
    <script type="text/javascript" src="Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="Styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>
    <link rel="Stylesheet" href="Styles/custom-theme/jquery-ui-1.10.3.custom.min.css"
        type="text/css" />
    <!-- PASDP Style sheet -->
    <link id="lnkhomepage" rel="Stylesheet" runat="server" type="text/css" media="all" />
    <!-- mbExtruder includes for the slide out bar -->
    <link href="Styles/mbExtruder.css" media="all" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/mbExtruder.js"></script>
    <script type="text/javascript" src="Scripts/jquery.mb.flipText.js"></script>
    <!-- Scroller for menu -->
    <script type="text/javascript" src="Scripts/jquery.scrollTo-1.4.3.1-min.js"></script>
    <!-- Scroller for DIV -->
    <script src="Scripts/jquery.jscrollpane.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="Scripts/mwheelIntent.js" type="text/javascript"></script>
    <link href="Styles/jquery.jscrollpane.css?reload" rel="stylesheet" type="text/css" />
    <link id="lnkscroll" rel="stylesheet" type="text/css" runat="server" />
    <link href="Scripts/keyb/keyboard.css" rel="stylesheet" />
    <!-- IQTouch js files -->
    <script src="scripts/Pharmacy.js?reload=true" type="text/javascript"></script>
    <script src="scripts/KNHExpress.js?reload=true" type="text/javascript"></script>
    <script type="text/javascript">
        //Global variable
        //        var selectedMaintenanceTabIndex;

        //        function BeginRequestHandler(sender, args) {

        //            var maintenancetabs = $("#tabsMaintainTables").tabs();
        //            selectedMaintenanceTabIndex = maintenancetabs.tabs('option', 'selected');

        //        }
        //        function EndRequestHandler(sender, args) {
        //            var maintenancetabs = $("#tabsMaintainTables").tabs();
        //            maintenancetabs.tabs('select', selectedMaintenanceTabIndex);
        //        }  

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="rsm" runat="server">
    </telerik:RadScriptManager>
    <asp:UpdatePanel ID="asdf" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <div>
                <div id="tabsMaintainTables">
                    <ul>
                        <li><a href="#tab-Owners">Owners</a></li>
                        <li><a href="#tab-Status">Status</a></li>
                        <li><a href="#tab-Roles">Roles</a></li>
                    </ul>
                    <div id="tab-Owners">
                        <asp:UpdatePanel runat="server" ID="TableMaintStatusUpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadButton ID="RadButton1" runat="server" Text="but 1">
                                </telerik:RadButton>
                                <br />
                                1
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-Status">
                        <asp:UpdatePanel runat="server" ID="TableMaintStatusUpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                2
                                <telerik:RadButton ID="RadButton2" runat="server" Text="but 2">
                                </telerik:RadButton>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tab-Roles">
                        <asp:UpdatePanel runat="server" ID="TableMaintRolesUpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadButton ID="btn3" runat="server" Text="but 3" OnClick="btn3_Click">
                                </telerik:RadButton>
                                <br />
                                3
                                <br />
                                <telerik:RadTextBox ID="txt1" runat="server">
                                </telerik:RadTextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(function () {
            $("#tabsMaintainTables").tabs();
        });
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    </script>
    </form>
</body>
</html>
