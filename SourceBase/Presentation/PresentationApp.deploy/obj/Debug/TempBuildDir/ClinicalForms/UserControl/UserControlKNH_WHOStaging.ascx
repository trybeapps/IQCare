<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_WHOStaging.ascx.cs"
    Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_WHOStaging" %>
    <%@ Register Assembly="AjaxControlToolkit" TagPrefix="UCact" Namespace="AjaxControlToolkit" %>

      <script type="text/javascript" src="../../Touch/Scripts/jquery-1.10.1.min.js"></script>
        <script type="text/javascript" src="../../Touch/Styles/custom-theme/jquery-ui-1.10.3.custom.min.js"></script>

<script type="text/javascript">

    function rblSelectedValue2(val, divID, txtControlId) {
        
        var selectedvalue = val;
        var YN = "";
        if (selectedvalue == "1") {
            YN = "show";
        }
        else {

            //document.getElementById(txtControlId).value = '';
            YN = "hide";
        }
        ShowHideDiv(divID);

    }
    function ShowHideDiv(theDiv) {
        if ($("#" + theDiv).is(':visible'))
        { $("#" + theDiv).hide(); }
        else
        { $("#" + theDiv).show(); }
    }
</script>
<table class="border center formbg" cellspacing="6" cellpadding="0" width="100%"
    border="0">
    <tr>
        <td colspan="2">
        <div id="divprogressioninwhoshowhide">
            <table width="100%">
                <tr id="TRProgressionWHO" runat="server">
                    <td colspan="2" class="border pad5 whitebg">
                    <table  width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlProgressioninWHO" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgProgressioninWHO" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <h2 align="left" class="forms">
                                                                        Progression in WHO - WAB Evaluation</h2>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                        <%--<table>
                            <tr>
                                <td>
                                    <h4 class="forms" align="left">
                                        Progression in WHO - WAB Evalaution
                                    </h4>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RadioButtonList4" runat="server" RepeatDirection="Horizontal"
                                        OnClick="rblSelectedValue(this,'hideProgressioninWHOYN')">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="PnlProgressioninWHODetails" runat="server">
                            <table width="100%" border="0" cellspacing="6" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td class="border center pad5 whitebg" style="width: 50%;">
                                            <table width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td align="right" style="width: 22%;">
                                                            <label id="lblHas WHO Stage progressed-8888902" align="center">
                                                                Has WHO Stage progressed :</label>
                                                        </td>
                                                        <td align="left" style="width: 28%;">
                                                            <asp:RadioButtonList ID="rdoProgressionInWHOstage" runat="server" RepeatDirection="Horizontal"
                                                                OnClick="rblSelectedValue(this,'divspecifyprog')">
                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0" ></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td style="width: 50%;">
                                            <div id="divspecifyprog" style="display: none;">
                                                <table width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right" style="width: 40%;">
                                                                <label id="lblSpecify WHO progression-8888903" align="center">
                                                                    Specify WHO progression :</label>
                                                            </td>
                                                            <td align="left" style="width: 60%;">
                                                                <asp:TextBox runat="server" ID="txtSpecifyWHOprogression" Width="180px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            </div>
        </td>
        </tr>
    <tr>
        <td colspan="2" class="border pad5 whitebg">
        <table  width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlWHOStageI" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgWHOStageI" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <h2 align="left" class="forms">
                                                                        WHO Stage I</h2>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
            <%--<table>
                <tr>
                    <td>
                        <h4 align="left">
                            WHO Stage I
                        </h4>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblWHOStageI" runat="server" RepeatDirection="Horizontal"
                            OnClick="rblSelectedValue(this,'hideWHOStageIYN')">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>--%>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <%--<div id="hideWHOStageIYN" style="display: none;">--%>
            <asp:Panel ID="PnlWHOStageIDetails" runat="server">
                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                    <tbody>
                        <tr>
                            <td class="border center pad5 whitebg" style="width: 50%;" colspan="2">
                                <asp:Panel ID="PnlWHO1" runat="server" ScrollBars="Vertical" Height="100px">
                                    <asp:GridView ID="gvWHO1" runat="server" AutoGenerateColumns="false" Width="800px"
                                        ShowFooter="false" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Presenting WHO Stage I Conditions" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwho1" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                    <asp:CheckBox ID="Chkwho1" runat="server" Text='<%# Eval("NAME") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWho1Date" maxlength="11" size="8" name="visitDate" runat="server"  />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWho1Date").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Historic" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWho1Date1" maxlength="11" size="8" name="visitDate" runat="server" />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWho1Date1").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="border pad5 whitebg">
        <table  width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlWHOStageII" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgPnlWHOStageII" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <h2 align="left" class="forms">
                                                                        WHO Stage II</h2>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
            <%--<table>
                <tr>
                    <td>
                        <h4 class="forms" align="left">
                            WHO Stage II
                        </h4>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblWHOStageII" runat="server" RepeatDirection="Horizontal"
                            OnClick="rblSelectedValue(this,'hideWHOStageIIYN')">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>--%>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="PnlWHOStageIIDetails" runat="server">
                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                    <tbody>
                        <tr>
                            <td class="border center pad5 whitebg" style="width: 50%;" colspan="2">
                                <asp:Panel ID="PnlWHO2" runat="server" ScrollBars="Vertical" Height="200px">
                                    <asp:GridView ID="gvWHO2" runat="server" AutoGenerateColumns="false" Width="800px"
                                        ShowFooter="false" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Presenting WHO Stage II Conditions" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblwho2" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                    <asp:CheckBox ID="Chkwho2" runat="server" Text='<%# Eval("NAME") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWho2Date" maxlength="11" size="8" name="visitDate" runat="server" />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWho2Date").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Historic" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWho2Date1" maxlength="11" size="8" name="visitDate" runat="server" />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWho2Date1").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="border pad5 whitebg">
        <table  width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlWHOStageIII" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgWHOStageIII" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <h2 align="left" class="forms">
                                                                        WHO Stage III</h2>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
            <%--<table>
                <tr>
                    <td>
                        <h4 class="forms" align="left">
                            WHO Stage III
                        </h4>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblWHOStageIII" runat="server" RepeatDirection="Horizontal"
                            OnClick="rblSelectedValue(this,'hideWHOStageIIIYN')">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>--%>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="PnlWHOStageIIIDetails" runat="server">
                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                    <tbody>
                        <tr>
                            <td class="border center pad5 whitebg" colspan="2">
                                <asp:Panel ID="PnlWHO3" runat="server" ScrollBars="Vertical" Height="200px">
                                    <asp:GridView ID="gvWHO3" runat="server" AutoGenerateColumns="false" Width="800px"
                                        ShowFooter="false" BorderStyle="None" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Presenting WHO Stage III Conditions" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWHO3" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                    <asp:CheckBox ID="ChkWHO3" runat="server" Text='<%# Eval("NAME") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWHO3Date" maxlength="11" size="8" name="visitDate" runat="server" />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWHO3Date").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Historic" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWho3Date1" maxlength="11" size="8" name="visitDate" runat="server" />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWho3Date1").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="border pad5 whitebg">
        <table  width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlWHOStageIV" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgWHOStageIV" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <h2 align="left" class="forms">
                                                                        WHO Stage IV</h2>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
            <%--<table>
                <tr>
                    <td>
                        <h4 class="forms" align="left">
                            WHO Stage IV
                        </h4>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblWHOStageIV" runat="server" RepeatDirection="Horizontal"
                            OnClick="rblSelectedValue(this,'hideWHOStageIVYN')">
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>--%>
        </td>
    </tr>
    <tr>
        <td colspan="2">
             <asp:Panel ID="PnlWHOStageIVDetails" runat="server">
                <table width="100%" border="0" cellspacing="6" cellpadding="0">
                    <tbody>
                        <tr>
                            <td class="border center pad5 whitebg" style="width: 50%;" colspan="2">
                                <asp:Panel ID="PnlWHO4" runat="server" ScrollBars="Vertical" Height="200px">
                                    <asp:GridView ID="gvWHO4" runat="server" AutoGenerateColumns="false" Width="800px"
                                        ShowFooter="false" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Presenting WHO Stage IV Conditions" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWHO4" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                    <asp:CheckBox ID="ChkWHO4" runat="server" Text='<%# Eval("NAME") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWHO4Date" maxlength="11" size="8" name="visitDate" runat="server" />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWHO4Date").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Historic" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <input id="txtCurrentWho4Date1" maxlength="11" size="8" name="visitDate" runat="server" />
                                                    <img onclick="w_displayDatePicker('<%# ((GridViewRow)Container).FindControl("txtCurrentWho4Date1").ClientID %>');"
                                                        height="22" alt="Date Helper" hspace="5" src="../../images/cal_icon.gif" width="22"
                                                        border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="border pad5 whitebg">
        <table  width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlstagingYN" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="Imgstaging" ImageUrl="~/images/arrow-up.gif" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <h2 align="left" class="forms">
                                                                        Staging</h2>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
            
        </td>
    </tr>
    <tr>
        <td colspan="2">
             <asp:Panel ID="pnlstaging" runat="server">
                                                    <table id="Table11" class=" center formbg" cellspacing="6" cellpadding="0" width="100%"
                                                        border="0">
                                                        <tr id="trnonadultfollowup" style="display: none;">
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="right" style="width: 55%;">
                                                                            <label>
                                                                                WHO Stage at initiation (Transfer in):</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <asp:DropDownList ID="ddlInitiationWHOstage" runat="server" AutoPostBack="false"
                                                                                Width="130px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table class="tbl-left">
                                                                    <tr>
                                                                        <td align="right" class="data-lable">
                                                                            <label class="required">
                                                                                *HIV associated conditions:</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <asp:DropDownList ID="ddlhivassociated" runat="server" Width="130px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="right" style="width: 55%;">
                                                                            <label class="required">
                                                                                *Current WHO Stage:</label>
                                                                        </td>
                                                                        <td align="left" class="data-control">
                                                                            <asp:DropDownList ID="ddlwhostage1" runat="server" Width="130px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td width="50%" class="border pad5 whitebg">
                                                                <table class="tbl-left">
                                                                    <tr>
                                                        <td align="right" style="width: 40%;">
                                                            <label class="required" id="lblWAB Stage-8888554" align="center">
                                                                WAB Stage :</label>
                                                        </td>
                                                        <td align="left" >
                                                            <asp:DropDownList Width="100%" runat="server" ID="ddlWABStage">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 1%;">
                                                            <asp:HiddenField ID="hiddateshow" runat="server" />
                                                        </td>
                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div id="divmernarcheshow">
                                                                    <%--<table width="100%">
                                                                        <tr>
                                                                            <td class="border pad6 whitebg" width="50%" >--%>
                                                                    <table class="border pad6 whitebg" width="100%">
                                                                        <tr>
                                                                            <td style="width: 15%;" align="right" class="pad5">
                                                                                <label>
                                                                                    Mernarche:
                                                                                </label>
                                                                            </td>
                                                                            <td style="width: 35%;" align="left">
                                                                                <input id="radbtnMernarcheyes" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue2(1,'divmenarchedatehshowhide','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtmenarchedate');"
                                                                                    type="radio" name="labevaluationyes" runat="server" />                                   
                                                                                <label>
                                                                                    Yes</label>
                                                                                <input id="radbtnMernarcheno" onmouseup="up(this);" onfocus="up(this);" onclick="down(this);rblSelectedValue2(0,'divmenarchedatehshowhide','ctl00_IQCareContentPlaceHolder_tabControl_TabExamination_txtmenarchedate');"
                                                                                    type="radio" name="labevaluationyes" runat="server" checked />
                                                                                <label>
                                                                                    No</label>
                                                                            </td>
                                                                            <td style="width: 35%;" align="left">
                                                                                <div id="divmenarchedatehshowhide" style="display: none;">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <label id="Label88">
                                                                                                    Menarche Date:
                                                                                                </label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <input id="txtmenarchedate" onblur="DateFormat(this,this.value,event,false,'3')"
                                                                                                    onkeyup="DateFormat(this,this.value,event,false,'3')" onfocus="javascript:vDateType='3'"
                                                                                                    maxlength="11" size="11" name="VisitDate" runat="server" />
                                                                                                <img id="Img11" onclick="w_displayDatePicker('<%=txtmenarchedate.ClientID%>');" height="22 "
                                                                                                    alt="Date Helper" hspace="5" src="../images/cal_icon.gif" width="22" border="0"
                                                                                                    name="appDateimg" style="vertical-align: text-bottom;" />
                                                                                                <span class="smallerlabel" id="Span11">(DD-MMM-YYYY)</span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%-- </td>
                                                                        </tr>
                                                                    </table>--%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="50%" class="border pad5 whitebg" colspan="2">
                                                                <div id="divtannerstaging">
                                                                    <table class="tbl-left">
                                                                        <tr>
                                                                            <td align="right" class="data-lable">
                                                                                <label>
                                                                                    Tanner Staging:</label>
                                                                            </td>
                                                                            <td align="left" class="data-control">
                                                                                <asp:DropDownList ID="ddltannerstaging" runat="server" AutoPostBack="false" Width="130px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPatFUstatus" style="display: none;">
                                                            <td width="50%" class="border pad5 whitebg" colspan="2">
                                                                <div id="div1">
                                                                    <table class="tbl-left">
                                                                        <tr>
                                                                            <td align="right" class="data-lable">
                                                                                <label>
                                                                                    Patient Follow Up Status:</label>
                                                                            </td>
                                                                            <td align="left" class="data-control">
                                                                                <asp:DropDownList ID="ddlPatFUstatus" runat="server" AutoPostBack="false" Width="130px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="2">
           
            <UCact:CollapsiblePanelExtender ID="CPPnlWHOStageI" runat="server"
                                    SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWHOStageIDetails"
                                    CollapseControlID="PnlWHOStageI" ExpandControlID="PnlWHOStageI"
                                    CollapsedImage="~/Images/arrow-up.gif" Collapsed="true" ImageControlID="ImgWHOStageI">
                                </UCact:CollapsiblePanelExtender>
                                <UCact:CollapsiblePanelExtender ID="CPPnlWHOStageII" runat="server"
                                    SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWHOStageIIDetails"
                                    CollapseControlID="PnlWHOStageII" ExpandControlID="PnlWHOStageII"
                                    CollapsedImage="~/Images/arrow-up.gif" Collapsed="true" ImageControlID="ImgWHOStageII">
                                </UCact:CollapsiblePanelExtender>
                                <UCact:CollapsiblePanelExtender ID="CPPnlWHOStageIII" runat="server"
                                    SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWHOStageIIIDetails"
                                    CollapseControlID="PnlWHOStageIII" ExpandControlID="PnlWHOStageIII"
                                    CollapsedImage="~/Images/arrow-up.gif" Collapsed="true" ImageControlID="ImgWHOStageIDetails">
                                </UCact:CollapsiblePanelExtender>
                                <UCact:CollapsiblePanelExtender ID="CPPnlWHOStageIV" runat="server"
                                    SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlWHOStageIVDetails"
                                    CollapseControlID="PnlWHOStageIV" ExpandControlID="PnlWHOStageIV"
                                    CollapsedImage="~/Images/arrow-up.gif" Collapsed="true" ImageControlID="ImgWHOStageIV">
                                </UCact:CollapsiblePanelExtender>
                                <UCact:CollapsiblePanelExtender ID="CPPnlProgressioninWHO" runat="server"
                                    SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif" TargetControlID="PnlProgressioninWHODetails"
                                    CollapseControlID="PnlProgressioninWHO" ExpandControlID="PnlProgressioninWHO"
                                    CollapsedImage="~/Images/arrow-up.gif" Collapsed="true" ImageControlID="ImgProgressioninWHO">
                                </UCact:CollapsiblePanelExtender>
                                <UCact:CollapsiblePanelExtender ID="CPpnlstaging" runat="server"
                SuppressPostBack="true" ExpandedImage="~/images/arrow-dn.gif" TargetControlID="pnlstaging"
                CollapseControlID="pnlstagingYN" ExpandControlID="pnlstagingYN"
                CollapsedImage="~/Images/arrow-up.gif" Collapsed="true" ImageControlID="Imgstaging">
            </UCact:CollapsiblePanelExtender>
        </td>
    </tr>
</table>
