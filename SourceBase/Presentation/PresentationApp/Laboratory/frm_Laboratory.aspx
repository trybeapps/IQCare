<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true"
    CodeBehind="frm_Laboratory.aspx.cs" Inherits="PresentationApp.Laboratory.frm_Laboratory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function Close() {
            var curl = document.URL;
            var w = window.opener;
            var rurl = '..';
            if (curl.toLowerCase().indexOf("iqcare") != -1) {
                rurl = rurl + '/IQCare';
            }

            if (w == null || w.closed) {
                window.location = rurl + "../ClinicalForms/frmPatient_Home.aspx";
            }
            else {
                window.close();
            }
            return false;
        }
    </script>
    <style type="text/css">
        .RadAutoCompleteBox_Metro .ui .li
        {
            float: left !important;
            text-align: left;
        }
        .RadComboBoxDropDown .rcbItem, .RadComboBoxDropDown .rcbHovered, .RadComboBoxDropDown .rcbDisabled, .RadComboBoxDropDown .rcbLoading, .RadComboBoxDropDown .rcbCheckAllItems, .RadComboBoxDropDown .rcbCheckAllItemsHovered
        {
            min-height: 10px;
            font: 12px Arial, sans-serif;
        }
        
        .RadAutoCompleteBox_Metro .racItem, .RadAutoCompleteBox_Metro .racInput, .RadAutoCompleteBox_Metro .racHovered
        {
            float: left !important;
        }
        .RadGrid_Metro tr.Row50
        {
            height: 10px;
        }
        .RadComboBox_Metro, .RadComboBox_Metro .rcbInput, .RadComboBoxDropDown_Metro
        {
            font: 12px Arial, sans-serif;
            color: #333;
            text-align: left;
        }
        .RadAutoCompleteBoxPopup_Default, .RadAutoCompleteBoxPopup_Metro, .RadAutoCompleteBoxPopup .racList
        {
            max-height: 250px !important;
            text-align: left;
        }
    </style>
    <div class="center" style="padding: 8px;">
        <%--        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>--%>
        <asp:UpdatePanel ID="Updatepanel" runat="server">
            <ContentTemplate>
                <div class="center border formbg">
                    <table cellspacing="6" cellpadding="0">
                        <tbody>
                            <tr>
                                <td class="form" align="center" valign="middle" width="50%">
                                    <label>
                                        Preclinic Labs:</label><asp:CheckBox ID="preclinicLabs" runat="server" />
                                </td>
                                <td class="form" align="center" valign="middle">
                                    <label for="LabtobeDone" class="right35">
                                        Lab to be done on:</label>
                                    <asp:TextBox ID="txtLabtobeDone" MaxLength="11" runat="server" Width="70px"></asp:TextBox>
                                    <img onclick="w_displayDatePicker('<%= txtLabtobeDone.ClientID %>');" height="22"
                                        alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="20" border="0" />
                                    <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="form pad7">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2" align="left">
                                                Pre-Selected Labs
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" style="width: 70%">
                                                <telerik:radcombobox id="rcbPreSelectedLabTest" runat="server" text="aSomeTest" autopostback="true"
                                                    skin="Metro" checkboxes="true" enableloadondemand="false" enablecheckallitemscheckbox="true"
                                                    checkeditemstexts="FitInInput" width="100%">
                                            </telerik:radcombobox>
                                            </td>
                                            <td valign="bottom" align="left" style="width: 30%">
                                                <telerik:radbutton id="btnAddDrug" text="Add" runat="server" skin="Metro" onclick="BtnAddDrugClick">
                                            </telerik:radbutton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="form pad7">
                                    <table width="90%">
                                        <tr>
                                            <td align="left" style="width: 10%">
                                                Select Lab:
                                            </td>
                                            <td valign="top" style="width: 70%; text-align: left">
                                                <telerik:radautocompletebox id="AutoselectLabTest" runat="server" skin="Metro" dropdownwidth="500px"
                                                    minfilterlength="2" width="555" onentryadded="Autoselectdrug_EntryAdded">
                                                <WebServiceSettings Path="frm_Laboratory.aspx" Method="GetLabsNames" />
                                                
                                            </telerik:radautocompletebox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="Tr2" class="border" runat="server">
                                <td class="form bold" align="left" colspan="2">
                                    <a id="sGrid" href="#sGrid"></a>
                                    <telerik:radgrid autogeneratecolumns="false" id="RadGridLabTest" runat="server" width="100%"
                                        pagesize="5" allowpaging="false" allowmultirowselection="false" clientsettings-selecting-allowrowselect="true"
                                        clientsettings-resizing-allowcolumnresize="true" showfooter="true" clientsettings-resizing-enablerealtimeresize="true"
                                        cellpadding="0" skin="Metro" onitemcreated="RadGridLabTest_ItemCreated" onneeddatasource="RadGridLabTest_NeedDataSource"
                                        onitemcommand="RadGridLabTest_ItemCommand" ondeletecommand="RadGridLabTest_DeleteCommand"
                                        culture="(Default)">
                                            <PagerStyle Mode="NextPrevAndNumeric" />
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="True"></Selecting>
                                                <Resizing AllowColumnResize="True" EnableRealTimeResize="True"></Resizing>
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                                                DataKeyNames="LabTestID" CellSpacing="0" CellPadding="0">
                                                <%--<NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="center">
                                                                No Records Found
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>--%>
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="Laboratory Test Name" HeaderStyle-Font-Bold="true">
                                                        <HeaderStyle Font-Size="10px" Wrap="False" Width="40%" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabTestName" runat="server" Text='<%# Eval("SubTestName") %>'></asp:Label>
                                                            <asp:Label ID="lblLabSubTestID" runat="server" Visible="false" Text='<%# Eval("SubTestId") %>'></asp:Label>
                                                            <asp:Label ID="lblLabTestID" runat="server" Visible="false" Text='<%# Eval("LabTestId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Units" HeaderStyle-Font-Bold="true">
                                                        <HeaderStyle Font-Size="10px" Wrap="False" Width="10%" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabTestUom" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Test Type" HeaderStyle-Font-Bold="true">
                                                        <HeaderStyle Font-Size="10px" Wrap="False" Width="20%" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabTestType" runat="server" Text='<%# Eval("LabDepartmentName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <telerik:RadButton ID="btnRemove" runat="server" Skin="Metro" Text="Remove"
                                                                ForeColor="Blue" CommandName="Delete" ButtonType="LinkButton">
                                                            </telerik:RadButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                        <NestedViewSettings>
                                        <ParentTableRelation>
                                         <telerik:GridRelationFields DetailKeyField="SubTestId" MasterKeyField="SubTestId" />  
                                        </ParentTableRelation>
                                        </NestedViewSettings>
                                        <NestedViewTemplate>
                                            <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridLabResult" runat="server" 
                                                Width="100%" PageSize="5" AllowPaging="true" AllowMultiRowSelection="false" ClientSettings-Selecting-AllowRowSelect="false"
                                                ClientSettings-Resizing-AllowColumnResize="false" ShowFooter="false" ClientSettings-Resizing-EnableRealTimeResize="false"
                                                CellPadding="0"  ShowHeader="false" Skin="Metro" 
                                                 HierarchyLoadMode="ServerOnDemand" OnItemDataBound="RadGridResut_ItemDataBound" DataKeyNames="LabTestID" 
                                                   OnNeedDataSource="RadGridLabResult_NeedDataSource">
                                              <MasterTableView Name="ChildGrid">
                                           <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Lab Test Name" HeaderStyle-Font-Bold="true" UniqueName="SubTestId">
                                                 <ItemTemplate>
                                                 <telerik:RadNumericTextBox ID="txtRadValue" runat="server"  Text='<%# Eval("TestResults") %>' Skin="Metro" >
                                                 </telerik:RadNumericTextBox>
                                                 <telerik:RadTextBox ID="txtAlphaRadValue" runat="server" Text='<%# Eval("TestResults1") %>' Skin="Metro">
                                                 </telerik:RadTextBox>
                                                    <asp:RadioButtonList id="btnRadRadiolist" runat ="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                                                    <asp:CheckBoxList ID="chkBoxList" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
                                                    <asp:DropDownList ID="ddlList" runat="server"></asp:DropDownList>                                                 
                                                 <asp:Label ID="lblUnitName" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                                 <asp:Label ID="lblControlType" runat="server" Text='<%# Eval("Control_type") %>' Visible="false"></asp:Label>
                                                   <asp:Label ID="lblLabSubTestId" runat="server"  Visible="false" Text='<%# Eval("SubTestID") %>'></asp:Label>
                                                   <asp:Label ID="lblLabTestName" runat="server"  Visible="false" Text='<%# Eval("SubTestName") %>'></asp:Label>
                                                   <asp:Label ID="lblMinBoundaryVal" runat="server"  Visible="false" Text='<%# Eval("MinBoundaryValue") %>'></asp:Label>
                                                   <asp:Label ID="lblMaxBoundaryVal" runat="server"  Visible="false" Text='<%# Eval("MaxBoundaryValue") %>'></asp:Label>
                                                   <asp:Label ID="lblTestResultId" runat="server"  Visible="false" Text='<%# Eval("TestResultId") %>'></asp:Label>
                                                   <asp:Label ID="lblTestResults" runat="server"  Visible="false" Text='<%# Eval("TestResults") %>'></asp:Label>
                                                  <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridArvMutation" runat="server"
                                                    Width="100%" PageSize="5" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false"
                                                    ClientSettings-Selecting-AllowRowSelect="true" ClientSettings-Resizing-AllowColumnResize="true"
                                                    ShowFooter="true" ClientSettings-Resizing-EnableRealTimeResize="true"  OnItemDataBound="RadGridArvMutation_ItemDataBound"
                                                    OnItemCommand="RadGridArvMutation_ItemCommand" OnDeleteCommand="RadGridArvMutation_DeleteCommand"
                                                    CellPadding="0" Skin="Metro" >
                                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                        <Resizing AllowColumnResize="True" EnableRealTimeResize="True"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="False" 
                                                        DataKeyNames="ID" CellSpacing="0" CellPadding="0">                                                        
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="ARV Type" HeaderStyle-Font-Bold="true">
                                                                 <HeaderStyle Font-Size="10px" Wrap="False" Width="180px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                                                    <asp:Label ID="lblArvType" runat="server"  Text='<%# Eval("ArvType") %>'></asp:Label>
                                                                    <asp:Label ID="lblArvTypeID" runat="server"  Visible="false" Text='<%# Eval("ArvTypeID") %>'></asp:Label>
                                                                    <asp:Label ID="lblDeleteFlag" runat="server"  Visible="false" Text='<%# Eval("DeleteFlag") %>'></asp:Label>
                                                                    <asp:Label ID="lblMutationID" runat="server"  Visible="false" Text='<%# Eval("ArvMutationID") %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="rcbEditArvType" runat="server" Text="aSomeTest" AutoPostBack="false"
                                                                        Skin="Metro" CheckedItemsTexts="FitInInput">
                                                                    </telerik:RadComboBox>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>                                                                  
                                                                    <telerik:RadComboBox ID="rcbFooterArvType" runat="server" Text="--Select--" AutoPostBack="true"
                                                                        Skin="Metro" CheckedItemsTexts="FitInInput"  EnableLoadOnDemand="true" OnSelectedIndexChanged="rcbFooterArvType_SelectedIndexChanged" >
                                                                    </telerik:RadComboBox>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Mutation" HeaderStyle-Font-Bold="true">
                                                                 <HeaderStyle Font-Size="10px" Wrap="False" Width="200px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMutation" runat="server" Text='<%# Eval("ArvMutation") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="rcbEditMutation" runat="server" Text="--Select--" AutoPostBack="false"
                                                                        Skin="Metro" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                                    </telerik:RadComboBox>   

                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                   <telerik:RadComboBox ID="rcbFooterMutation" runat="server" Text="--Select--" AutoPostBack="true"
                                                                        Skin="Metro" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" OnSelectedIndexChanged="rcbFooterMutation_SelectedIndexChanged">
                                                                    </telerik:RadComboBox>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Culture/Sensitivity" HeaderStyle-Font-Bold="true">
                                                                 <HeaderStyle Font-Size="10px" Wrap="False" Width="200px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCulture" runat="server" Text='<%# Eval("ArvMutationOther") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <telerik:RadComboBox ID="rcbEditCulture" runat="server" Text="--Select--" AutoPostBack="false"
                                                                        Skin="Metro" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                                    </telerik:RadComboBox>   

                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                   <telerik:RadComboBox ID="rcbFooterCulture" runat="server" Text="--Select--" AutoPostBack="false"
                                                                        Skin="Metro" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true" >
                                                                    </telerik:RadComboBox>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn HeaderText="Other Mutation" HeaderStyle-Font-Bold="true">
                                                                <HeaderStyle Font-Size="10px" Wrap="False" Width="150px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOtherMutation" Skin="Metro" runat="server" Text='<%# Eval("ArvMutationOther") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                 <telerik:RadTextBox ID="txtOtherEditMutation"  Skin="Metro" runat="server" Text='<%# Eval("ArvMutationOther") %>'></telerik:RadTextBox>  
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <telerik:RadTextBox ID="txtOtherFooterMutation" Skin="Metro" runat="server" Text='<%# Eval("ArvMutationOther") %>' Width="120px" Wrap="true"></telerik:RadTextBox>  
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>
                                                           
                                                            <telerik:GridTemplateColumn HeaderStyle-Font-Bold="true">
                                                            <HeaderStyle Font-Size="10px" Wrap="False" Width="90px" />
                                                                <ItemTemplate>
                                                                    <telerik:RadButton ID="btnRemove" runat="server"  Skin="Metro"  Text="Remove"
                                                                        ForeColor="Blue" CommandName="Delete" ButtonType="LinkButton" >
                                                                    </telerik:RadButton>
                                                                </ItemTemplate>
                                                              
                                                                <FooterTemplate>
                                                                    <telerik:RadButton ID="btnFooterAdd" runat="server" Skin="Metro" Text="Add"
                                                                        CommandName="Insert">
                                                                    </telerik:RadButton>
                                                                </FooterTemplate>
                                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                            </telerik:GridTemplateColumn>
                                                          <%--  <telerik:GridEditCommandColumn ButtonType="PushButton"  ItemStyle-Font-Names="verdana" ItemStyle-Font-Size="10pt"  >
                                                            </telerik:GridEditCommandColumn>--%>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <%--<FooterStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                                    <HeaderStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />--%>
                                                </telerik:RadGrid>
                                                 </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                              </Columns>
                                              </MasterTableView>
                                            </telerik:RadGrid>                                         
                                          
                                        </NestedViewTemplate>
                                            </MasterTableView>
                                            <%--<FooterStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                            <HeaderStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />--%>
                                        </telerik:radgrid>
                                </td>
                            </tr>
                            <tr>
                                <td class="form" colspan="2">
                                    <asp:Panel ID="pnlCustomList" Visible="false" runat="server" Height="100%" Width="100%"
                                        Wrap="true">
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="form pad7" style="width: 50%" align="center">
                                    <label class="required">
                                        *Ordered by:</label>
                                    <asp:DropDownList ID="ddlaborderedbyname" runat="Server">
                                    </asp:DropDownList>
                                </td>
                                <td class="form pad7" style="width: 50%" align="center">
                                    <label class="required" for="LabOrderedbyDate">
                                        *Ordered By Date:</label>
                                    <asp:TextBox ID="txtlaborderedbydate" MaxLength="12" size="11" runat="server"></asp:TextBox>
                                    <img id="appDateimg1" onclick="w_displayDatePicker('<%=txtlaborderedbydate.ClientID%>');"
                                        height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                        border="0" name="appdateimg">
                                    <span class="smallerlabel" id="appdatespan1">(DD-MMM-YYYY)</span>
                                </td>
                            </tr>
                            <tr id="trReported" runat="server">
                                <td class="form pad7" style="width: 50%" align="center">
                                    <label class="required">
                                        *Reported by:</label>
                                    <asp:DropDownList ID="ddlreportedby" runat="Server">
                                    </asp:DropDownList>
                                </td>
                                <td class="form pad7" style="width: 50%" align="center">
                                    <label class="required" for="repordtedbydate">
                                        *Reported By Date:</label>
                                    <asp:TextBox ID="txtrepordtedbydate" MaxLength="12" size="11" runat="server"></asp:TextBox>
                                    <img id="Img1" onclick="w_displayDatePicker('<%=txtrepordtedbydate.ClientID%>');"
                                        height="22" alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22"
                                        border="0" name="appdateimg">
                                    <span class="smallerlabel" id="Span1">(DD-MMM-YYYY)</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="pad5 center" colspan="2">
                                    <%--<input type="button"  value="Save" id="btnsave" style="font-size: 12px;
                                    font-weight: 75px" onclick="BtnSaveClick()" />--%>
                                    <asp:Button ID="btnsave" runat="server" Font-Size="12px" Width="75px" Text="Save"
                                        OnClick="BtnSaveClick" />
                                    <asp:Button ID="btnclose" runat="server" Font-Size="12px" Width="75px" Text="Close"
                                        OnClick="btnclose_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:HiddenField ID="hiddTestID" runat="server" />
                    <asp:HiddenField ID="hiddTestAddTestID" runat="server" />
                    <asp:HiddenField ID="hdnTestAddTestID" runat="server" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnclose"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <asp:HiddenField ID="txthdnfield" runat="server" />
    </div>
</asp:Content>
