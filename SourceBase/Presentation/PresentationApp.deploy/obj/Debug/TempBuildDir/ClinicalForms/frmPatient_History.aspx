<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" Inherits="frmPatient_History" Title="Untitled Page" Codebehind="frmPatient_History.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <%--   <form id="IETreeview" method="post" runat="server">--%>
    <div class="center" style="padding: 8px;">
        <asp:Label ID="lblDate" CssClass="smallerlabel" runat="server" Text=""></asp:Label></h3>
        <%--   <h1 class="margin">
            Existing Forms</h1> 
        --%>
        <div class="border center formbg">
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <%--    <tr> 
                        <td class="form" align="center" colspan="2">
                            <label class="patientInfo">
                                Patient Name:</label>
                            <asp:Label ID="lblpatientname" runat="server" Text="Mary Longlastname"></asp:Label>
                            <label class="patientInfo">
                                Patient ID:</label>
                            <asp:Label ID="lblpatientenrolment" runat="server" Text="444545"></asp:Label>
                            <label id="lblFileRef" runat="server" class="patientInfo">
                                Existing Hosp/Clinic #:</label>
                            <asp:Label ID="lblexisclinicid" runat="server" Text="12345678-444444-596A"></asp:Label></td>
                    </tr>--%>
                    <tr>
                        <td class="form" align="center" colspan="2"  >
                            <div class="treeview">
                                <h1>
                                    <asp:TreeView ID="TreeViewExisForm" ForeColor="#000000" runat="server" Width="100%"
                                        OnSelectedNodeChanged="TreeViewExisForm_SelectedNodeChanged">
                                    </asp:TreeView>
                                </h1>
                            </div>
                            <asp:Button ID="btnExit" runat="server" Text="Back" OnClick="btnExit_Click" /><%--<asp:GridView ID="GrdViewlistIE" Width=100% AutoGenerateColumns="False" runat="server" BorderColor="#666999" CellPadding="1" CellSpacing="1"  OnRowDataBound="ItemDataBoundEventHandler1">
    <HeaderStyle ForeColor="#000066" CssClass="tableheaderstyle" HorizontalAlign="Left" Wrap="False"></HeaderStyle>
    <AlternatingRowStyle BackColor="White" BorderColor="White" />
    </asp:GridView>--%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
