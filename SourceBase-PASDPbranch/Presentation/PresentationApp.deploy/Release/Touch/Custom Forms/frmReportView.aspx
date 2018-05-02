<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportView.aspx.cs"
    Inherits="PresentationApp.Touch.Custom_Forms.frmReportView" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javaScript" type="text/javascript" src="../../crystalreportviewers13/js/crviewer/crv.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
        <chart:WebChartViewer ID="WebChartViewerCD4VL" runat="server" Height="400px" Width="300px" />
    </div>
    </form>
</body>
</html>
