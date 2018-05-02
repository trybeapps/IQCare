<%@ Page Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="True"
    Inherits="frmadmin_adduser" Title="Untitled Page" CodeBehind="frmadmin_adduser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    <div id="unique_id" style="display: none;">
        The Password must meet the following Requirements: Minimum of 6 character length.
        Atleast one upper case letter. Atleast one numeric character. Atleast one non alpha
        character. No more than 3 consecutive characters e.g '1234' or 'abcd'. You may not
        use the word 'password',firstname,lastname,username.
    </div>
    <script language="javascript" type="text/javascript">
        function validstrngpwd(fname, lname, uname) {
            //var txtnewpwdid = badword.value;
            if (!validateStrongPassword('ctl00_IQCareContentPlaceHolder_txtPassword', { length: [6, Infinity], lower: 1, upper: 1, numeric: 1, alpha: 1, special: 1, badWords: ['password', fname, lname, uname], badSequenceLength: 4 })) {
                //alert(badword.toString());
                return true;
            }
            else {
                return false;
            }
        }
        function validnewuserstrngpwd() {
            //var txtnewpwdid = badword.value;
            if (!validateStrongPassword('ctl00_IQCareContentPlaceHolder_txtPassword', { length: [6, Infinity], lower: 1, upper: 1, numeric: 1, alpha: 1, special: 1, badWords: ['password'], badSequenceLength: 4 })) {
                //alert(badword.toString());
                return true;
            }
            else {
                return false;
            }
        }

        function phonenumber(inputtxt) {
            var phoneno = /^\d{10}$/;

            if (document.getElementById('<%= txtPhone.ClientID%>').value == "") {
                return true;
            }
            else if (inputtxt.value.match(phoneno)) {
                return true;
            }

            else {
                alert("Not a valid Phone Number");
                document.getElementById('<%= txtPhone.ClientID%>').focus();
                return false;
            }
        }

        function ValidateEmail(inputText) {
            var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            if (document.getElementById('<%= txtEmail.ClientID%>').value == "") {
                return true;
            }
            else if (inputText.value.match(mailformat)) {
                return true;
            }
            else {
                alert("You have entered an invalid email address!");
                document.getElementById('<%= txtEmail.ClientID%>').focus();
                return false;
            }
        }  


    </script>
    <div>
        <%--  <form id="adduser" method="post" runat="server">--%>
        <h3 class="margin" align="left" style="padding-left: 10px;">
            <asp:Label ID="lblh2" runat="server" Text="Add User"></asp:Label></h3>
        <div class="center" style="padding: 5px;">
            <div class="border center formbg">
                <br />
                <table cellspacing="6" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td style="width: 50%">
                            </td>
                            <td align="right" style="width: 50%">
                                <label for="LastName" style="color: red">
                                    *New User would be asked for password change for the first time</label>
                            </td>
                        </tr>
                        <tr>
                            <td class="border pad5 whitebg center" align="center" style="width: 50%">
                                <label class="right required" for="LastName">
                                    *Last Name:</label>
                                <asp:TextBox ID="txtlastname" runat="server"></asp:TextBox>
                            </td>
                            <td class="border pad5 whitebg center" align="center" width="50%">
                                <label class="right required" for="FirstName">
                                    *First Name:</label>
                                <asp:TextBox ID="txtfirstname" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="border pad5 whitebg center" align="center" style="width: 50%">
                                <label class="right required" for="UserName">
                                    *User Name:</label>
                                <asp:TextBox ID="txtusername" runat="server"></asp:TextBox>
                            </td>
                            <td class="border pad5 whitebg" align="center" nowrap="noWrap">
                                <label class="right" for="UserName">
                                    Designation:</label>
                                <asp:DropDownList ID="dddesignation" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="border pad5 whitebg center" align="center" style="width: 50%">
                                <label class="right required" for="passverification">
                                    *Password:</label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                            <td class="border pad5 whitebg center" align="center" width="50%">
                                <label class="right required" for="password">
                                    *Confirm Password:</label>
                                <asp:TextBox ID="txtConfirmpassword" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="border pad5 whitebg center" align="center" style="width: 50%">
                                <label class="right" for="email">
                                    Email:</label>
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            </td>
                            <td class="border pad5 whitebg center" align="center" width="50%">
                                <label class="right" for="phone">
                                    Phone Number:</label>
                                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="border pad5 whitebg center" align="center" style="width: 50%">
                                <div class="center">
                                    <label class="left required" for="usergroup" style="vertical-align: top;">
                                        *User Group:</label></div>
                                <div id="grdchkboldugrp" style="width: 90%; padding-left: 35px">
                                    <div nowrap='nowrap' class="divborder">
                                        <asp:CheckBoxList ID="lstUsergroup" runat="server" Width="90%" CausesValidation="True"
                                            CssClass="margin10">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </td>
                            <td class="border pad5 whitebg center" colspan="2" align="center">
                                <div class="center">
                                    <label class="left" for="usergroup" style="vertical-align: top;">
                                        Stores:</label></div>
                                <div id="grdchkboldstore" style="width: 90%; padding-left: 35px">
                                    <div nowrap='nowrap' class="divborder">
                                        <asp:CheckBoxList ID="chklistStores" runat="server" Width="90%" CausesValidation="True"
                                            CssClass="margin10">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <%--<td class="border pad5 whitebg center" colspan="2" align="center">
                                <label class="right">
                                    Employee:</label>
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                </asp:DropDownList>
                            </td>--%>
                            <td class="center" colspan="2" style="height: 31px">
                                &nbsp;
                                <asp:Button ID="btnDelete" runat="server" Text="Remove" OnClick="btnDelete_Click" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Reset" />
                                <asp:Button ID="btnExit" runat="server" Text="Back" OnClick="btnExit_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
