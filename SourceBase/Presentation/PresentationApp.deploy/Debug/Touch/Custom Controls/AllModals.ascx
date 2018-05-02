<%@ Control Language="C#" AutoEventWireup="True" Inherits="Touch.Custom_Controls.AllModals" Codebehind="AllModals.ascx.cs" %>
    

    <telerik:radwindow runat="server" id="rwContactRecTreatment" Title="Contact TB Treatment"
     Modal="true" Skin="BlackMetroTouch" Width="880px" VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close" >
            
            <ContentTemplate>
                <telerik:RadGrid ID="rgContactRecTreatment" runat="server" AllowMultiRowSelection="true"
                AutoGenerateColumns="False" CellSpacing="0" GridLines="None" 
    AllowPaging="True" AllowSorting="True" Skin="BlackMetroTouch">
                 <ClientSettings>
                        <Selecting AllowRowSelect="true" />
                 </ClientSettings>
                    <MasterTableView PageSize="10">
                        <Columns>
                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderText="Select All">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridBoundColumn DataField="Drug" 
                                FilterControlAltText="Filter Drug column" HeaderText="TB Drugs" 
                                UniqueName="Drug" DataType="System.String" />
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </ContentTemplate>
    </telerik:radwindow>
    
    <telerik:radwindow runat="server" id="rwAdverseName" Title="Adverse Events"
     Modal="true" Skin="BlackMetroTouch" Width="880px" VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close" >
            <ContentTemplate>
                 Adverse Event listing with severity select lists here
            </ContentTemplate>
    </telerik:radwindow>

    <telerik:radwindow runat="server" id="rwPrescribeDrugs" Title="Prescribe Drugs"
     Modal="true" Skin="BlackMetroTouch" Width="880px" VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close" >
            <ContentTemplate>
                 Placeholder - pharmacy form
                
            </ContentTemplate>
    </telerik:radwindow>
    <telerik:radwindow runat="server" id="rwOrderLabs" Title="Order Lab Tests"
     Modal="true" Skin="BlackMetroTouch" Width="880px" VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close" >
            <ContentTemplate>
                 Placeholder - Laboratory form
                
            </ContentTemplate>
    </telerik:radwindow>
    <telerik:radwindow runat="server" id="rwCareEnded" Title="Care Ended"
     Modal="true" Skin="BlackMetroTouch" Width="880px" VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close" >
            <ContentTemplate>
                 Placeholder Care Ended Form
                
            </ContentTemplate>
    </telerik:radwindow>
    <telerik:RadWindow runat="server" ID="rwViewExistingForms" Title="View existing forms" Modal="true" Skin="BlackMetroTouch" Width="480px"
    VisibleOnPageLoad="false" Height="450px" Behaviors="Move,Close" CssClass="preference">
        <ContentTemplate>
        <script type="text/javascript">
            function CheckValsP(s, e) {
                if (e.get_node().get_text() != "Clinical Status")
                    return false;

            }
            function CloseMinLoading(theDivName) {
                currentLoadingPanel = $find(parent.GetLPanelID());
                var theInterval = setInterval(function () { currentLoadingPanel.hide(theDivName); clearInterval(theInterval); }, 1000);

            }
            function ShowMinLoading(theDivName) {
                currentLoadingPanel = $find(parent.GetLPanelID());
                currentLoadingPanel.show(theDivName);
                return true;
            }
        </script>
        <div style="width:100%; height:100%;">
         <asp:UpdatePanel ID="updtVEF" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
            
                <asp:TreeView ID="TreeViewExisForm" ForeColor="#000000" runat="server" Width="100%" Height="100%" OnSelectedNodeChanged="TreeViewExisForm_SelectedNodeChanged">
                </asp:TreeView>
            </ContentTemplate>
         </asp:UpdatePanel>
        </div>
        </ContentTemplate>
    </telerik:RadWindow>
   