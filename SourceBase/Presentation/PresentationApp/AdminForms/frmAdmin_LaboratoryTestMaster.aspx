<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="True" Inherits="AdminLaboratoryTest_Master"
    Title="Untitled Page" Codebehind="frmAdmin_LaboratoryTestMaster.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/IQCare.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <script language="javascript">
        function ShowHideBoundary() {

            var pnlBoundary = document.getElementById("<%= pnlBoundary.ClientID %>");
            var pnlBoundary2 = document.getElementById("<%= pnlBoundary2.ClientID %>");
            var dataType = document.getElementById("<%= ddlDataType.ClientID %>");
            var btnselectList = document.getElementById("<%= btnselectList.ClientID %>");

            if (dataType[dataType.selectedIndex].text == 'Numeric') {
                pnlBoundary.style.display = 'block';
                pnlBoundary2.style.display = 'block';
                btnselectList.style.visibility = "hidden";
            }
            else if (dataType[dataType.selectedIndex].text == 'Select List') {
                pnlBoundary.style.display = 'none';
                pnlBoundary2.style.display = 'none';
                btnselectList.style.visibility = "visible";
            }
            else {
                pnlBoundary.style.display = 'none';
                pnlBoundary2.style.display = 'none';
                btnselectList.style.visibility = "hidden";
            }

        }
        function selectwinopen() {
            window.open('frmAdmin_LaboratorySelectList.aspx?LabId=' + '<%= LabIdforselectList %>', 'Selection', 'toolbars=no,location=no,directories=no,dependent=yes,top=10,left=30,maximize=no,resize=no,width=330,height=310,scrollbars=yes');
        }
    </script>
    <%--<form id="adduser" method="post" runat="server">--%>
    <div class="center" style="padding: 5px;">
        <h3 class="margin" align="left">
            <asp:Label ID="lblH2" runat="server" Text="Add/Edit LaboratoryTest"></asp:Label></h3>
        <div class="border center formbg">
            <br>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border center pad5 whitebg" width="50%">
                            <label class="right30">
                                Laboratory Test :</label>
                            <asp:TextBox ID="txtLabName" runat="server" MaxLength="50" Width="160"></asp:TextBox>
                        </td>
                        <td class="border center pad5 whitebg" width="50%">
                            <label class="right30">
                                Department :</label>
                            <asp:DropDownList ID="ddDepartment" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td id="tdDataType" class="border center pad5 whitebg" runat="server">
                            <label>
                                Data Type :</label>
                            <asp:DropDownList ID="ddlDataType" runat="server">
                                <asp:ListItem Text="Text" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Numeric" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Select List" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Group" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                            <input type="button" id="btnselectList" name="btnselectList" onclick="javascript:selectwinopen();return false;"
                                runat="server" value="..." />
                            <asp:TextBox ID="txtSeq" Visible="false" runat="server">1</asp:TextBox>
                        </td>
                        <asp:Label runat="server" ID="lblStatus1">
                            <td class="border center pad5 whitebg" width="33%">
                                <label class="right20">
                                    Status :</label>
                                <asp:DropDownList ID="ddStatus" runat="server">
                                    <asp:ListItem Text="Active" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="InActive" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </asp:Label>
                    </tr>
                    <tr>
                        <td class="border center pad5 whitebg" width="33%">
                            <asp:Panel ID="pnlBoundary" runat="server">
                                <label class="right30">
                                    Lower Boundary :</label>
                                &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtLowerBoundary" runat="server" MaxLength="19"></asp:TextBox>
                            </asp:Panel>
                        </td>
                        <td class="border center pad5 whitebg" width="33%">
                            <asp:Panel ID="pnlBoundary2" runat="server">
                                <label class="right35">
                                    Upper Boundary :</label>
                                &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtUpperBoundary" runat="server" MaxLength="19"></asp:TextBox>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%">
                <tbody>
                    <tr>
                        <td class="pad5 center" align="center">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnExit" runat="server" Text="Close" OnClick="btnExit_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
        </div>
    </div>
    <script language="javascript">
        // var pnlBoundary=document.getElementById("<%= pnlBoundary.ClientID %>");
        // var pnlBoundary2=document.getElementById("<%= pnlBoundary2.ClientID %>");
        // pnlBoundary.style.display='none';
        // pnlBoundary2.style.display='none';

        var pnlBoundary = document.getElementById("<%= pnlBoundary.ClientID %>");
        var pnlBoundary2 = document.getElementById("<%= pnlBoundary2.ClientID %>");
        var dataType = document.getElementById("<%= ddlDataType.ClientID %>");
        var btnselectList = document.getElementById("<%= btnselectList.ClientID %>");

        if (dataType[dataType.selectedIndex].text == 'Numeric') {
            pnlBoundary.style.display = 'block';
            pnlBoundary2.style.display = 'block';
            btnselectList.style.visibility = "hidden";
        }
        else if (dataType[dataType.selectedIndex].text == 'Select List') {
            pnlBoundary.style.display = 'none';
            pnlBoundary2.style.display = 'none';
            btnselectList.style.visibility = "visible";
        }
        else {
            pnlBoundary.style.display = 'none';
            pnlBoundary2.style.display = 'none';
            btnselectList.style.visibility = "hidden";
        }
        
    </script>
</asp:Content>
