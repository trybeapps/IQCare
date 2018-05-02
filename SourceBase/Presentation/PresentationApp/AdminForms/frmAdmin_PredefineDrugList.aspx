<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/IQCare.master"
    CodeBehind="frmAdmin_PredefineDrugList.aspx.cs" Inherits="PresentationApp.AdminForms.frmAdmin_PredefineDrugList" %>

<%@ MasterType VirtualPath="~/MasterPage/IQCare.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <%-- <form id="addeditDisease" method="post" runat="server">--%>
    <script type="text/javascript">

        function MoveItemUpDown(goUp) {

            var list = document.getElementById('<%= lstSelectedDrug.ClientID %>');
            //Get the current selected items index
            var selectedIndex = list.selectedIndex;

            //tell the user to select one if he hasn't
            if (selectedIndex == -1) {
                alert("Select an item for re-ordering.");
            }
            else {
                //check if we need to move up or down
                var newIndex = selectedIndex + (goUp ? -1 : 1);
                if (newIndex < 0) {
                    //If we have reached top cycle it to end
                    newIndex = list.length - 1;
                }
                else if (newIndex >= list.length) {
                    //If we have reached end cycle it to top
                    newIndex = 0;
                }

                //Lets take the old items value and text
                var oldVal = list[selectedIndex].value;
                var oldText = list[selectedIndex].text;

                //Swap the value and text of old and new items.  
                list[selectedIndex].value = list[newIndex].value;
                list[selectedIndex].text = list[newIndex].text;
                list[newIndex].value = oldVal;
                list[newIndex].text = oldText;
                list.selectedIndex = newIndex;

                //Done, the element is moved now
            }
        }                                           

    </script>
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <h3 class="margin" align="left" style="padding-left: 10px;">
                    <asp:Label ID="lblHeader" runat="server" Text="Predefined Drug List"></asp:Label>
                </h3>
                <div class="center" style="padding: 5px;">
                    <div class="border center">
                        <br />
                        <table cellpadding="18" width="100%" height="70%" border="0" class="border whitebg pad18">
                            <tbody>
                                <tr>
                                    <td class="border formbg">
                                        <asp:ListBox ID="lstDrugList" runat="server" Height="180px" Width="300px"></asp:ListBox>
                                    </td>
                                    <td>
                                        <div>
                                            <asp:Button ID="btnAdd" runat="server" Width="80px" Text="Add >>" OnClick="btnAdd_Click" />
                                        </div>
                                        <br />
                                        <div>
                                            <asp:Button ID="btnRemove" runat="server" Width="80px" Text="<< Remove" OnClick="btnRemove_Click" />
                                        </div>
                                    </td>
                                    <td class="border formbg">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lstSelectedDrug" runat="server" Height="180px" Width="300px"></asp:ListBox>
                                                </td>
                                                <td valign="middle">
                                                    <asp:ImageButton ID="btn_Up" Width="35px" Height="35px" runat="server" ImageUrl="../Touch/images/upknh.png"
                                                        OnClick="btn_Up_Click" />
                                                    <%--<img class="link" onclick="MoveItemUpDown(true)" src="../Touch/images/upknh.png"
                                                alt="Up" style="cursor: hand; width: 52px; height: 52px;" />--%>
                                                    <br />
                                                    <asp:ImageButton ID="btn_down" Width="35px" Height="35px" runat="server" ImageUrl="../Touch/images/downknh.png"
                                                        OnClick="btn_down_Click" />
                                                    <%--<img class="link" onclick="MoveItemUpDown(false)" src="../Touch/images/downknh.png"
                                                alt="Down" style="cursor: hand; width: 52px; height: 52px;" />--%>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <table width="100%">
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" />
                                        <asp:Button ID="btnExit" runat="server" Text="Close" OnClick="btnExit_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnCancel"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnExit"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btn_Up"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btn_down"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnAdd"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnRemove"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
