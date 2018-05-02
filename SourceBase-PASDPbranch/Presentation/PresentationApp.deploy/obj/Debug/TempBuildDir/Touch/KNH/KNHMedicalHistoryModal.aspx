<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KNHMedicalHistoryModal.aspx.cs" Inherits="PresentationApp.Touch.KNH.KNHMedicalHistoryModal" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../scripts/PASDPBaseScripts.js" type="text/javascript"></script>
   


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
          //  oArg.PcValue = document.getElementById('hiddPcValueModal').value;
            oArg.Form_Name = fromName;
            oArg.flagmode = flag;

             var oWnd = GetRadWindow();
             oWnd.close(oArg);




         }
       
 </script>

</head>
<body>
    <form id="form1" runat="server">
      
     <Telerik:radscriptmanager ID="RadScriptManager1" runat="server">
    </Telerik:radscriptmanager>
   <%-- <asp:ScriptManager ID="mst" runat="server">
    </asp:ScriptManager>--%>
    <div>
    <table id="Medical History" width="100%">
      <tr>
      <td>
       <asp:UpdatePanel ID="uptdLabResults" runat="server" UpdateMode="Conditional">
                   <ContentTemplate>
          <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridSection" runat="server" Width="100%"
              AllowPaging="false" ShowFooter="false" CellPadding="0" 
                            skin="MetroTouch" GroupRenderMode="Flat" GridLines="None" ShowHeader="true" 
              ShowGroupPanel="true"  onitemcommand="RadGridSection_ItemCommand" 
              onitemcreated="RadGridSection_ItemCreated"  >
              
              
               <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                  DataKeyNames="SectionID" CellSpacing="0" CellPadding="0">
                  <Columns>
                      <telerik:GridTemplateColumn HeaderText="Section Name" HeaderStyle-Font-Bold="true">
                         
                          <ItemTemplate>
                              <asp:Label ID="lblSectionName" runat="server" Text='<%# Eval("SectionName") %>'></asp:Label>
                              <asp:Label ID="lblSectionID" runat="server" Visible="false" Text='<%# Eval("SectionID") %>'></asp:Label>
                          </ItemTemplate>
                          <ItemStyle BackColor="LightGray" />
                          <%--<HeaderStyle Font-Bold="True"></HeaderStyle>--%>
                      </telerik:GridTemplateColumn>
                  </Columns>
                   <NestedViewSettings>
                      <ParentTableRelation>
                          <telerik:GridRelationFields DetailKeyField="sectionID" MasterKeyField="sectionID" />
                      </ParentTableRelation>
                  </NestedViewSettings>
                   <NestedViewTemplate>
                       <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridFieldName" runat="server"
                           Width="100%" AllowPaging="false" AllowMultiRowSelection="false" ShowFooter="false"
                           CellPadding="0"  ShowHeader="false" Skin="MetroTouch"
                           HierarchyLoadMode="ServerOnDemand" DataKeyNames="SectionID" OnItemDataBound="RadGridFieldName_ItemDataBound" OnNeedDataSource="RadGridFieldName_NeedDataSource" >
                            <MasterTableView Name="ChildGrid">

                             <%-- <NoRecordsTemplate>
                                  <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                      <tr>
                                          <td align="center">
                                              No Records Found
                                          </td>
                                      </tr>
                                  </table>
                              </NoRecordsTemplate>--%>
                              <Columns>
                                  <telerik:GridTemplateColumn HeaderText="Section Details" HeaderStyle-Font-Bold="true"
                                      UniqueName="SectionID">
                                      <ItemTemplate>
                                          <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("FieldLabel") %>'></asp:Label>
                                          <asp:Label ID="lblControlFlag" runat="server" Visible="false" Text='<%# Eval("name") %>'></asp:Label>

                                      </ItemTemplate>
                                  </telerik:GridTemplateColumn>
                                  <telerik:GridTemplateColumn HeaderText="Section Details" HeaderStyle-Font-Bold="true"
                                      UniqueName="SectionID">
                                      <ItemTemplate>
                                          <telerik:RadTextBox ID="txtRadText" runat="server" Width="120px" Skin="MetroTouch">
                                          </telerik:RadTextBox>
                                          <telerik:RadDatePicker ID="dtDateValue" runat="server" Skin="MetroTouch">
                                              <Calendar ID="Calendar1" UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False"
                                                  Skin="MetroTouch" runat="server">
                                              </Calendar>
                                              <DateInput ID="DateInput1" DisplayDateFormat="dd MMM yyyy" DateFormat="dd/MM/yyyy"
                                                  LabelWidth="0px" runat="server">
                                                  <EmptyMessageStyle Resize="None"></EmptyMessageStyle>
                                                  <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                  <FocusedStyle Resize="None"></FocusedStyle>
                                                  <DisabledStyle Resize="None"></DisabledStyle>
                                                  <InvalidStyle Resize="None"></InvalidStyle>
                                                  <HoveredStyle Resize="None"></HoveredStyle>
                                                  <EnabledStyle Resize="None"></EnabledStyle>
                                              </DateInput>
                                              <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                          </telerik:RadDatePicker>
                                          <telerik:RadButton ID="rbtnYesNo" runat="server" Width="52px" GroupName="BirthBCG"
                                              ToggleType="CustomToggle" AutoPostBack="false" ButtonType="StandardButton" Skin="MetroTouch">
                                              <ToggleStates>
                                                  <telerik:RadButtonToggleState Text="No" />
                                                  <telerik:RadButtonToggleState Text="Yes" CssClass="BlueBG" />
                                              </ToggleStates>
                                          </telerik:RadButton>
                                      </ItemTemplate>
                                  </telerik:GridTemplateColumn>
                              </Columns>
                            </MasterTableView>
                       </telerik:RadGrid>
                   
                   </NestedViewTemplate>
                  
                  </MasterTableView>
                
              </telerik:RadGrid>
        </ContentTemplate>
        </asp:UpdatePanel>     

     </td>
     </tr>
     <tr>
     <td align="center">
      <telerik:RadButton ID="radbtnSubmit" runat="server" Text="Submit"></telerik:RadButton>
     </td>
     </tr>

      </table>
    
       
    
    </div>
    </form>
</body>
</html>
