<%@ Page Language="C#" AutoEventWireup="True" Inherits="Touch.frmTouchLogin" CodeBehind="frmTouchLogin.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title runat="server" id="AppTitle"></title>
    <script type="text/javascript" src="scripts/jquery-1.10.1.min.js"></script>
    <script src="styles/custom-theme/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
    <link rel="Stylesheet" id="lnklogin" type="text/css" runat="server" />
    <style type="text/css">
        .preference .rwWindowContent
        {
            background-color: White !important;
            color: Black !important;
        }
    </style>
</head>
<body style="background-color: White">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div id="theWrapper" style="position: absolute; top: 0; bottom: 0; left: 0; right: 0;">
        <input type="button" id="GoToFacility" style="visibility: collapse;" />
        <div id="loginDiv" class="target">
            <div id="toggle">
                <asp:Image ID="imgLogin" runat="server" />
                <br />
                <br />
                <asp:UpdatePanel ID="updtLogin" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="margin-left: 5em">
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtUname" runat="server" EmptyMessage="Username" Width="275px"
                                        Skin="MetroTouch">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtPass" runat="server" EmptyMessage="********" TextMode="Password"
                                        Width="275px" Skin="MetroTouch">
                                        <ClientEvents OnFocus="Focus" />
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadComboBox ID="ddFacility" runat="server" Skin="MetroTouch" Width="275px">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadButton ID="btnlogin" OnClientClicked="ShowLoading" runat="server" Text="Login"
                                        Skin="MetroTouch" OnClick="btnlogin_Click">
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Image ID="imgdownlogo" runat="server" alt="IQ Care" Style="float: right; margin-right: 50px;
                    margin-top: 20px;" />
                <div style="position: relative; clear: both; padding-top: 5px;">
                    <div style="float: left">
                        <a href="http://www.futuresgroup.com" target="_blank">
                            <img src="images/futures_logo.png" alt="Futures Group" border="0" /></a></div>
                    <div style="float: right; padding-top: 10px; font-size: 10px;">
                        <a href="http://creativecommons.org/licenses/by-nc-sa/3.0/" target="_blank">
                            <img src="images/cc_logo_resized.png" alt="Futures Group" border="0" /></a>
                        <br />
                        <br />
                        <a href="#" onclick="OpenContributors()">IQCare Contributors</a>
                    </div>
                    <div style="position: relative; float: left; clear: left; font-size: 10px;">
                        <table>
                            <tr>
                                <td>
                                    <b>Version :</b>
                                </td>
                                <td>
                                    <asp:Label CssClass="blue11 nomargin" ID="lblversion" Text="Version B1.0" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Release Date :</b>
                                </td>
                                <td>
                                    <asp:Label CssClass="blue11 nomargin" ID="lblrelDate" Text="Date" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <telerik:RadWindow runat="server" ID="rwContributors" VisibleOnPageLoad="false" Title="IQCare Contributors"
        Modal="true" Width="650px" Height="450px" Behaviors="Move,Close" CssClass="preference"
        Skin="BlackMetroTouch">
        <ContentTemplate>
            <div style="float: right; position: relative; margin: 5px; font-color: #FFF">
                <asp:Image ID="ContribLogo" runat="server" />
                <p>
                    The success of PADMT is directly dependent on the people who contributed to the
                    content of the software, specifying features, workflow, business rules and data
                    indicators. The dedication of these contributors and others not named here is greatly
                    appreciated and acknowledged
                </p>
                <b>Child Health, Department of Health Province of the Eastern Cape, South Africa</b>
                <ul>
                    <li>Dr. Gerald Boon</li>
                    <li>Dr. Cheree Goldswain</li>
                    <li>Dr. Kim Harper</li>
                    <li>Dr. Verena Linder</li>
                </ul>
                <b>ICT, Department of Health Province of the Eastern Cape, South Africa</b>
                <ul>
                    <li>Anton Strydom</li>
                    <li>Manesh Matthews</li>
                </ul>
                <b>Project Manager</b>
                <ul>
                    <li>Craig Carty, MSc.</li>
                </ul>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <telerik:RadAjaxLoadingPanel ID="statsRadLPanel" runat="server">
    </telerik:RadAjaxLoadingPanel>
    </form>
    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
        <script type="text/javascript">
            function ShowLoading() {
                currentLoadingPanel = $find("<%=statsRadLPanel.ClientID %>");
                currentLoadingPanel.show("toggle");
            }
            function CloseLoading() {
                currentLoadingPanel = $find("<%= statsRadLPanel.ClientID %>");
                currentLoadingPanel.hide("toggle");
            }
            function go(facilityHome) {
                $('body').fadeOut(300, function () {
                    document.location.href = facilityHome;
                });
            }
            function Focus(sender, eventArgs) {
                $('#' + '<%= txtPass.ClientID %>').val("");
            }
            $().ready(function () {
                if ($('#' + '<%= txtPass.ClientID %>').val() == "") {
                    $('#' + '<%= txtPass.ClientID %>').val("********");
                }
            });

            function pInvalid() {
                $("#toggle").effect("shake", { times: 3 }, 800);
                $("#txtUname").css("border", "solid 1px #ff0000");
                $("#txtPass").css("border", "solid 1px #ff0000");
            }

            //Open the Contributors Radwindow
            function OpenContributors() {
                var oWnd = $find("<%=rwContributors.ClientID%>");
                oWnd.show();
            }
        </script>
    </telerik:RadScriptBlock>
</body>
</html>
