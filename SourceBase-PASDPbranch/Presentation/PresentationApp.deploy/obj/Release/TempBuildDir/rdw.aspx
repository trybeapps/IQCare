<%@ Page Language="C#" AutoEventWireup="true" Inherits="rdw" Codebehind="rdw.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td style="width: 97px">
                    <asp:DropDownList ID="ddregion" runat="server">
                    </asp:DropDownList></td>
                <td style="width: 97px">
                    &nbsp;&nbsp;
                    <asp:DropDownList ID="dddistrict" runat="server">
                    </asp:DropDownList></td>
                <td style="width: 97px">
                    <asp:Button ID="btnsearch" runat="server" Text="Button" /></td>
            </tr>
            <tr>
                <td style="width: 97px">
                </td>
                <td style="width: 97px">
                </td>
                <td style="width: 97px">
                </td>
            </tr>
            <tr>
                <td style="width: 97px">
                </td>
                <td style="width: 97px">
                    <asp:GridView ID="grdsearch" runat="server">
                    </asp:GridView>
                </td>
                <td style="width: 97px">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
