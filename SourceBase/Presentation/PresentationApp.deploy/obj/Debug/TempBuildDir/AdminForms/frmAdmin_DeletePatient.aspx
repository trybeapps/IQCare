<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="True" Inherits="frmAdmin_DeletePatient" Title="Untitled Page" Codebehind="frmAdmin_DeletePatient.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <%--<form id="deleteform" method="post" enableviewstate="true" runat="server"  title="DeleteForm" >--%>
    <div>
        <h1 class="margin" style="padding-left: 10px;">
            Delete Patient</h1>
        <div class="center" style="padding: 5px;">
            <div class="border center formbg">
                <%--    <div class="contentpad">
                --%>
                <%--<asp:Panel id="PanelPatiInfo" class="border center formbg" runat="server">--%>
                <table cellspacing="6" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr id="trPatientInfo" class="border">
                            <td class="form" align="center">
                                <label class="bold">
                                    Patient Name:
                                    <asp:Label ID="lblpatientname" runat="server"></asp:Label></label>
                            </td>
                        </tr>
                        <tr id="trARTNo" class="border" runat="server">
                            <td class="form" align="center">
                                <label class="bold">
                                    <asp:Label ID="lblenroll" runat="server"></asp:Label>
                                    <asp:Label ID="lblptnenrollment" runat="server"></asp:Label>
                                </label>
                                <label class="bold">
                                    <asp:Label ID="lblClinicNo" runat="server"></asp:Label>
                                    <asp:Label ID="lblexistingid" runat="server"></asp:Label>
                                </label>
                            </td>
                        </tr>
                        <tr id="trPMTCTNo" class="border" runat="server">
                            <td class="form" align="center">
                                <label class="bold">
                                    <asp:Label ID="lblanc" runat="server">ANC Number:</asp:Label>
                                    <asp:Label ID="lblancno" runat="server"></asp:Label>
                                </label>
                                <label class="bold">
                                    <asp:Label ID="lblpmtct" runat="server">PMTCT Number:</asp:Label>
                                    <asp:Label ID="lblpmtctno" runat="server"></asp:Label>
                                </label>
                                <label class="bold">
                                    <asp:Label ID="lbladmission" runat="server">Admission Number:</asp:Label>
                                    <asp:Label ID="lbladmissionno" runat="server"></asp:Label></asp:Label>
                                </label>
                                <label class="bold">
                                    <asp:Label ID="lbloutpatient" runat="server">Outpatient Number:</asp:Label>
                                    <asp:Label ID="lbloutpatientno" runat="server"></asp:Label>
                                </label>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <%-- </asp:Panel>--%>
                <table cellspacing="6" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="form" align="center">
                                <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_Click"
                                    Enabled="false" />
                                <asp:Button ID="btnBack" Text="Back" runat="server" OnClick="btnBack_Click" />
                                <asp:Button ID="theBtn" Text="OK" CssClass="textstylehidden" runat="server" OnClick="theBtn_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <%-- </div>--%>
            </div>
        </div>
    </div>
</asp:Content>
