<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="True"
    Inherits="PresentationApp.AdminForms.frmAdmin_UserGroup" Title="Untitled Page"
    MaintainScrollPositionOnPostback="true" CodeBehind="frmAdmin_UserGroup.aspx.cs" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <div>
        <script language="C#" runat="server" type="text/javascript">
            /////////////////////////////////////////////////////////////////////
            // Code Written By   : Jayanta Kumar Das
            // Written Date      : 31st Aug 2006
            // Modification Date : 
            // Description       : Add/Edit UserGroup  
            // Code Rewritten By : Jayanta Kumar Das
            // Written Start Date: 17 Oct 2013
            // Modification Date : 
            // Description       : Add/Edit UserGroup  
            //
            /// /////////////////////////////////////////////////////////////////
        </script>
        <script language="javascript" type="text/javascript">
            
            function UpdateAllChildren(nodes, checked, eventArgs) {                
                                  
                var i;
                var test;
                for (i = 0; i < nodes.get_count(); i++) {                   
                    if (checked) {
                        nodes.getNode(i).check();
                    }
                    else {
                        if ((i == 0) && (nodes.getNode(i).get_text() == 'Records')) {                            
                            
                            if (nodes.getNode(i).get_checked() == false) {
                                nodes.getNode(0).set_checked(true);
                                
                            }
                        }
                        else {                            
                            nodes.getNode(i).set_checked(false);
                        }
                    }

                    if (nodes.getNode(i).get_nodes().get_count() > 0) {
                        UpdateAllChildren(nodes.getNode(i).get_nodes(), checked, eventArgs);
                    }
                }
            }
            function clientNodeChecked(sender, eventArgs) {
                var childNodes = eventArgs.get_node().get_nodes();
                var isChecked = eventArgs.get_node().get_checked();
                UpdateAllChildren(childNodes, isChecked, eventArgs);
            }

              function pageLoad() {
                var tree = $find("<%=TreeViewUserGroupForms.ClientID %>");
                for (var i = 0; i < tree.get_allNodes().length; i++) {
                    var node = tree.get_allNodes()[i];
                    if (node.get_attributes().getAttribute("flow") == "horizontal") {
                        node._nodeListElement.className += " horizontal";
                     }

                }
                var treecommon = $find("<%=TreeViewUserGroupAdminForms.ClientID %>");
                for (var i = 0; i < treecommon.get_allNodes().length; i++) {
                    var nodecommon = treecommon.get_allNodes()[i];
                    if (nodecommon.get_attributes().getAttribute("flow") == "horizontal") {
                        nodecommon._nodeListElement.className += " horizontal";
                    }
                }

                var treeCL = $find("<%=TreeViewUserGroupCommonForms.ClientID %>");
                for (var i = 0; i < treeCL.get_allNodes().length; i++) {
                    var nodeCL = treeCL.get_allNodes()[i];
                    if (nodeCL.get_attributes().getAttribute("flow") == "horizontal") {
                        nodeCL._nodeListElement.className += " horizontal";
                    }
                }

            }
        </script>
        <h3 class="margin" style="padding-left: 10px;">
            <asp:Label ID="lblh3" runat="server" Text="Add User Group"></asp:Label></h3>
        <div class="" style="padding: 5px;">
            <%--            <asp:ScriptManager ID="ss" runat="server" />--%>
            <div class="border">
                <table cellspacing="5" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="border pad5 whitebg center">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 40%">
                                            <label class="right15" for="UserName">
                                                User Group Name:</label>&nbsp;
                                            <asp:TextBox ID="txtusergroupname" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <label class="right40" for="CopyUserName">
                                                Copy Group Name:</label>&nbsp;
                                            <asp:DropDownList ID="ddGroupName" runat="server" Height="16px" Width="95px" OnSelectedIndexChanged="ddGroupName_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="pad5 border">
                                <act:TabContainer ID="tabControl" runat="server" Height="550px" ActiveTabIndex="0"
                                    CssClass="ajax__tab_technorati-theme">
                                    <act:TabPanel ID="tbpnlgeneral" runat="server" Font-Size="Medium" HeaderText="General">
                                        <HeaderTemplate>
                                            General
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                                <tr>
                                                    <td>
                                                        <div style="height: 500px; width: 100%;">
                                                            <div style="overflow: auto; max-height: 100%; border: 1px;">
                                                                <telerik:radtreeview id="TreeViewUserGroupForms" runat="server" skin="Vista" showlineimages="true" 
                                                                    singleexpandpath="true" onclientnodechecked="clientNodeChecked" checkboxes="True"
                                                                    TriStateCheckBoxes="true" style="border: 1px solid #CBE7F5;" enableembeddedskins="true" tabindex="1">
                                                                </telerik:radtreeview>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </act:TabPanel>
                                    <act:TabPanel ID="TbpnlAdminForms" runat="server" Font-Size="Medium" HeaderText="General">
                                        <HeaderTemplate>
                                            Admin Forms-Customize List
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                                <tr>
                                                    <td>
                                                        <div style="height: 500px; width: 100%;">
                                                            <div style="overflow: auto; max-height: 100%; border: 1px;">
                                                                <telerik:radtreeview id="TreeViewUserGroupAdminForms" runat="server" skin="Vista"
                                                                    showlineimages="true" singleexpandpath="true" onclientnodechecked="clientNodeChecked"
                                                                    checkboxes="True" TriStateCheckBoxes="true" style="border: 1px solid #CBE7F5;" 
                                                                    enableembeddedskins="true" tabindex="2">
                                                                </telerik:radtreeview>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </act:TabPanel>
                                    <act:TabPanel ID="tbpnlCFeature" runat="server" Font-Size="Medium" HeaderText="General">
                                        <HeaderTemplate>
                                            Common Features
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                                <tr>
                                                    <td>
                                                        <div style="height: 500px; width: 100%;">
                                                            <div style="overflow: auto; max-height: 100%; border: 1px;">
                                                                <telerik:radtreeview id="TreeViewUserGroupCommonForms" runat="server" skin="Vista"
                                                                    showlineimages="true" singleexpandpath="true" onclientnodechecked="clientNodeChecked"
                                                                    checkboxes="True" TriStateCheckBoxes="true" style="border: 1px solid #CBE7F5;" 
                                                                    enableembeddedskins="true" tabindex="2">
                                                                </telerik:radtreeview>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </act:TabPanel>
                                    <act:TabPanel ID="tbpnlSplPrivledges" runat="server" Font-Size="Medium" HeaderText="General">
                                        <HeaderTemplate>
                                            Special Privileges
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                                                <tr>
                                                    <td>
                                                        <div style="height: 500px; width: 100%;">
                                                            <div style="overflow: auto; max-height: 100%; border: 1px;">
                                                                <telerik:radtreeview id="TreeViewUserGroupSplPrivledges" runat="server" skin="Vista"
                                                                    showlineimages="true" singleexpandpath="true" onclientnodechecked="clientNodeChecked"
                                                                    checkboxes="True" TriStateCheckBoxes="true" style="border: 1px solid #CBE7F5;" 
                                                                    enableembeddedskins="true" tabindex="2">
                                                                </telerik:radtreeview>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </act:TabPanel>
                                </act:TabContainer>
                            </td>
                        </tr>
                        <tr>
                            <td class="pad5 center" style="height: 31px">
                                <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" />
                                <asp:Button ID="btnback" runat="server" Text="Cancel" OnClick="btnback_Click" />
                                <asp:Button ID="btncancel" runat="server" Text="Back" OnClick="btncancel_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
