<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KNHPresentingComplaintsModal.aspx.cs" Inherits="Touch.Custom_Forms.KNHPresentingComplaintsModal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
     function GetRadWindow() {

            var oWindow = null;

            if (window.radWindow) oWindow = window.radWindow;

            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;

            return oWindow;

        }

        function AdjustRadWidow() {

            var oWindow = GetRadWindow();

            setTimeout(function () { oWindow.autoSize(true); if ($telerik.isChrome || $telerik.isSafari) ChromeSafariFix(oWindow); }, 500);

        }
        function ChromeSafariFix(oWindow) {

            var iframe = oWindow.get_contentFrame();

            var body = iframe.contentWindow.document.body;



            setTimeout(function () {

                var height = body.scrollHeight;

                var width = body.scrollWidth;



                var iframeBounds = $telerik.getBounds(iframe);

                var heightDelta = height - iframeBounds.height;

                var widthDelta = width - iframeBounds.width;



                if (heightDelta > 0) oWindow.set_height(oWindow.get_height() + heightDelta);

                if (widthDelta > 0) oWindow.set_width(oWindow.get_width() + widthDelta);

                oWindow.center();



            }, 310);
        }
        function returnToParent() {
          //create the argument that will be returned to the parent page
            var oArg = new Object();
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 5).split('&');

            hash = hashes[0].split('=');
            var x = hash[0];
           var queryString = x.split('-');
         fromName = queryString[0];
         flag = queryString[1];
           
         //get a reference to the current RadWindow
            //alert(fromName + flag);
            oArg.PcValue = document.getElementById('hiddPcValueModal').value;
            oArg.Form_Name = fromName;
            oArg.flagmode = flag;

             var oWnd = GetRadWindow();
             oWnd.close(oArg);


         

        }



       
    
    </script>
    <link href="../styles/KNH.css" rel="stylesheet" type="text/css" />
    
</head>
<body  >
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
   
     <table id="Presenting Complaints" width="100%">
         <tr>
             <td colspan="2">
                 <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridPresenting" runat="server"
                     Width="100%" PageSize="10" AllowPaging="false" AllowMultiRowSelection="false"
                     ClientSettings-Selecting-AllowRowSelect="true" ShowFooter="false" Skin="MetroTouch"
                     ShowHeader="False" OnItemDataBound="RadGridPresenting_ItemDataBound">
                     <PagerStyle Mode="NextPrevAndNumeric" />
                     <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                         CellSpacing="0" CellPadding="0">
                         <Columns>
                             <telerik:GridTemplateColumn>
                                 <HeaderStyle Width="150px" />
                                 <ItemTemplate>
                                     <asp:Label ID="lblPresenting" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                     <asp:Label ID="lblchkval" runat="server" Text='<%# Eval("ChkVal") %>' Visible="false"></asp:Label>
                                     <asp:CheckBox ID="ChkPresenting" runat="server" Text='<%# Eval("NAME") %>' />
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn>
                                 <HeaderStyle Width="100px" />
                                 <ItemTemplate>
                                     <telerik:RadTextBox ID="txtPresenting" runat="server" Skin="MetroTouch" Text='<%# Eval("ChkValText") %>'
                                         Width="150px">
                                     </telerik:RadTextBox>
                                 </ItemTemplate>
                             </telerik:GridTemplateColumn>
                         </Columns>
                         <AlternatingItemStyle BorderStyle="None" />
                     </MasterTableView>
                     <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" />
                     <%-- <ItemStyle Font-Names="Verdana" Font-Size="8pt" />--%>
                 </telerik:RadGrid>
                 <%-- <telerik:RadComboBox ID="rcbPresentingComplaints" runat="server" Text="aSomeTest" AutoPostBack="false" 
                                        Skin="MetroTouch" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                        Width="250px">
                                    </telerik:RadComboBox>--%>
             </td>
         </tr>
      <tr>
       <td  colspan="2" class="SectionheaderTxt" style="width: 100%">
         Additional Presenting Complaints
       </td>
      </tr>
      <tr>
      <td colspan="2">
        Presenting Complaint(s) - description or additional comments :
      </td>
      </tr>
      <tr>
      <td width="100%">
        <telerik:RadTextBox ID="txtAdditionPresentingComplaints" runat="server" Width="100%" TextMode="MultiLine" Skin="MetroTouch"></telerik:RadTextBox>
      </td>
      </tr>
      <tr>
      <td colspan="2" align="center">
        <telerik:RadButton ID="btnSubmit" runat="server" Text="Submit" 
              onclick="btnSubmit_Click"></telerik:RadButton>
         <asp:HiddenField ID="hiddPcValueModal" runat="server" />
              
        
      </td>
      </tr>
      
     
     </table>
    
    </div>
    </form>
</body>
</html>
