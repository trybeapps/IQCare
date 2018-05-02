<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" CodeBehind="frm_Laboratory.aspx.cs" Inherits="PresentationApp.Laboratory.frm_Laboratory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
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
    </style>
<div class="center" style="padding: 8px;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
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
                                <asp:TextBox ID="txtLabtobeDone" MaxLength="11" runat="server" OnTextChanged="txtLabtobeDone_TextChanged"></asp:TextBox>
                                <img onclick="w_displayDatePicker('<%= txtLabtobeDone.ClientID %>');" height="22"
                                    alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="22" border="0" />
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
                                        <td valign="top" align="left" style="width:70%">
                                            <telerik:RadComboBox ID="rcbPreSelectedLabTest" runat="server" Text="aSomeTest" AutoPostBack="true"
                                                Skin="Metro" CheckBoxes="true" EnableLoadOnDemand="false" EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="FitInInput"
                                                Width="100%">
                                            </telerik:RadComboBox>
                                            
                                        </td>
                                        <td valign="bottom" align="left" style="width:30%">
                                            <telerik:RadButton ID="btnAddDrug" Text="Add" runat="server" Skin="Metro" OnClick="BtnAddDrugClick">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="form pad7">
                            <table width="90%">
                                    <tr>
                                        <td align="left" style="width:10%">
                                        Select Lab:
                                        </td>
                                    
                                    <td valign="top" align="left" style="width:70%">
                                            <telerik:RadAutoCompleteBox ID="AutoselectLabTest" runat="server" Skin="Metro" DropDownWidth="500px" MinFilterLength="2" 
                                                Width="100%"  OnEntryAdded="Autoselectdrug_EntryAdded">
                                            </telerik:RadAutoCompleteBox>
                             </td>
                             </tr>
                             </table>
                                
                            </td>
                        </tr>
                        <tr id="Tr2" class="border" runat="server">
                            <td class="form bold" align="left" colspan="2" >
                            <a id="sGrid" href="#sGrid"></a>
                                <telerik:RadGrid AutoGenerateColumns="false" ID="RadGridLabTest" runat="server" Width="100%"
                                            PageSize="5" AllowPaging="false" AllowMultiRowSelection="false" ClientSettings-Selecting-AllowRowSelect="true"
                                            ClientSettings-Resizing-AllowColumnResize="true" ShowFooter="true" ClientSettings-Resizing-EnableRealTimeResize="true"
                                            CellPadding="0" Font-Names="Verdana" Font-Size="10pt" Skin="Metro" OnItemCreated="RadGridLabTest_ItemCreated"
                                            OnNeedDataSource="RadGridLabTest_NeedDataSource" OnItemCommand="RadGridLabTest_ItemCommand"
                                            OnDeleteCommand="RadGridLabTest_DeleteCommand" Culture="(Default)">
                                            <PagerStyle Mode="NextPrevAndNumeric" />
                                            <ClientSettings>
                                                <Selecting AllowRowSelect="True"></Selecting>
                                                <Resizing AllowColumnResize="True" EnableRealTimeResize="True"></Resizing>
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="False" NoMasterRecordsText="No Records Found"
                                                DataKeyNames="LabTestID" CellSpacing="0" CellPadding="0">
                                                <NoRecordsTemplate>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="center">
                                                                No Records Found
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </NoRecordsTemplate>
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
                                            </MasterTableView>
                                            <FooterStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                            <HeaderStyle Font-Names="Verdana" Font-Size="10pt" HorizontalAlign="Left" />
                                        </telerik:RadGrid>
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
                        <tr>
                            <td class="pad5 center" colspan="2">
                                <input type="button" onclick="fnSave();" value="Save" id="btnsave" style="font-size: 12px;
                                    font-weight: 75px" />
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
    </div>
</asp:Content>
