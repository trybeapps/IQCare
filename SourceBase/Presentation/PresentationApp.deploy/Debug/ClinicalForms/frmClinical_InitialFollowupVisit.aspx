<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/IQCare.master" Inherits="ClinicalForms_frmClinical_InitialFollowupVisit" Codebehind="frmClinical_InitialFollowupVisit.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPage/IQCare.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="Server">
    
    <br />
    <div style="padding-left: 8px; padding-right: 8px;"> 
        <script language="javascript" type="text/javascript"> 
            function WindowPrint() {
                window.print();
            }
            function fnPageOpen(pageopen) {
                if (pageopen == "Pharmacy") {
                    window.open('../Pharmacy/frmPharmacyform.aspx?opento=ArtForm');
                }
                else if (pageopen == "Labratory") {
                    window.open('../Laboratory/frmLabOrder.aspx?opento=ArtForm');
                }
                else if (pageopen == "LabTest") {
                    window.open('../Laboratory/LabOrderForm.aspx?opento=ArtForm');
                }
            }

            function WindowHistory() {
                history.go(-1);
                return false;
            }
            function fnchange() {
                var e = document.getElementById("<%=ddlpregnancy.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;


                if (strtext == "Select") {
                    hide('spanEDD');
                    hide('miscarriage');
                    hide('Abortion');
                    document.getElementById("<%=ddlFamilyPanningStatus.ClientID%>").disabled = false;
                    document.getElementById("<%=txtdatemiscarriage.ClientID %>").value = "";
                    document.getElementById("<%=txtdateinducedabortion.ClientID %>").value = "";
                    document.getElementById("<%=txtEDD.ClientID %>").value = "";
                    document.getElementById("<%=txtANCNumber.ClientID %>").value = "";
                    document.getElementById("<%=chkrefpmtct.ClientID %>").checked = false;
                }
                else if (strtext == "Yes") {
                    show('spanEDD');
                    document.getElementById("<%=ddlFamilyPanningStatus.ClientID%>").selectedIndex = "0";
                    document.getElementById("<%=ddlFamilyPanningStatus.ClientID%>").disabled = true;
                    hide('divFamilyPlanningMethod');
                    hide('notfamilyplanning');
                    hide('miscarriage');
                    hide('Abortion');
                    document.getElementById("<%=txtdatemiscarriage.ClientID %>").value = "";
                    document.getElementById("<%=txtdateinducedabortion.ClientID %>").value = "";
                }
                else if (strtext == "No") {
                    hide('spanEDD');
                    hide('miscarriage');
                    hide('Abortion');
                    document.getElementById("<%=ddlFamilyPanningStatus.ClientID%>").disabled = false;
                    document.getElementById("<%=txtEDD.ClientID %>").value = "";
                    document.getElementById("<%=txtANCNumber.ClientID %>").value = "";
                    document.getElementById("<%=chkrefpmtct.ClientID %>").checked = false;
                }
                else if (strtext == "No - Induced Abortion (ab)") {
                    hide('spanEDD')
                    hide('miscarriage')
                    show('Abortion');
                    document.getElementById("<%=ddlFamilyPanningStatus.ClientID%>").disabled = false;
                    document.getElementById("<%=txtEDD.ClientID %>").value = "";
                    document.getElementById("<%=txtANCNumber.ClientID %>").value = "";
                    document.getElementById("<%=chkrefpmtct.ClientID %>").checked = false;
                }
                else if (strtext == "No - Miscarriage (mc)") {
                    show('miscarriage');
                    hide('spanEDD');
                    hide('Abortion');
                    document.getElementById("<%=ddlFamilyPanningStatus.ClientID%>").disabled = false;
                    document.getElementById("<%=txtEDD.ClientID %>").value = "";
                    document.getElementById("<%=txtANCNumber.ClientID %>").value = "";
                    document.getElementById("<%=chkrefpmtct.ClientID %>").checked = false;
                }
            }
            function fnfamilyplanning() {
                var e = document.getElementById("<%=ddlFamilyPanningStatus.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;

                if (strtext == "Select") {
                    hide('divFamilyPlanningMethod');
                    hide('notfamilyplanning');

                }
                else if (strtext == "Currently on Family Planning" || strtext == "Wants Family Planning") {
                    show('divFamilyPlanningMethod');
                    hide('notfamilyplanning');
                }
                else if (strtext == "Not on Family Planning") {
                    show('notfamilyplanning');
                    hide('divFamilyPlanningMethod');
                }
            }
            function fnTBStatus() {
                var e = document.getElementById("<%=ddlTBStatus.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;
                if (strtext == "Select") {
                    hide('tbstartdate');
                    hide('tbtreatment');
                    document.getElementById("<%=txttbstartdate.ClientID %>").value = "";
                    document.getElementById("<%=txtTBtreatmentNumber.ClientID %>").value = "";

                }
                else if (strtext == "TB Rx") {
                    show('tbstartdate');
                    show('tbtreatment');
                }
                else {
                    hide('tbstartdate');
                    hide('tbtreatment');
                    document.getElementById("<%=txttbstartdate.ClientID %>").value = "";
                    document.getElementById("<%=txtTBtreatmentNumber.ClientID %>").value = "";
                }
            }
            function fnSubsituations() {
                var e = document.getElementById("<%=ddlsubsituationInterruption.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;
                if (strtext == "Change regimen") {
                    show('arvTherapyChange');
                    hide('arvTherapyStop');

                }
                else if (strtext == "Stop treatment") {
                    show('arvTherapyStop');
                    hide('arvTherapyChange');
                }
                else {
                    hide('arvTherapyChange');
                    hide('arvTherapyStop');
                }
            }
            function fnRegimenChange() {
                var e = document.getElementById("<%=ddlArvTherapyChangeCode.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;
                if (strtext == "Other reason (specify)") {
                    show('otherarvTherapyChangeCode');

                }
                else {
                    hide('otherarvTherapyChangeCode');

                }
            }
            function fnStopReason() {
                var e = document.getElementById("<%=ddlArvTherapyStopCode.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;
                if (strtext == "Other (specify)") {
                    show('otherarvTherapyStopCode');

                }
                else {
                    hide('otherarvTherapyStopCode');

                }
            }
            function fnarvdrugother() {
                var e = document.getElementById("<%=ddlwhypoorfair.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;
                if (strtext == "Other (specify)") {
                    show('divReasonARVDrugsother');

                }
                else {
                    hide('divReasonARVDrugsother');

                }
            }
            function fnARVDrug() {
                var e = document.getElementById("<%=ddlarvdrugadhere.ClientID%>");
                var strtext = e.options[e.selectedIndex].text;

                if (strtext == "G=Good") {
                    document.getElementById("<%=ddlwhypoorfair.ClientID %>").disabled = true;
                }
                else {
                    document.getElementById("<%=ddlwhypoorfair.ClientID %>").disabled = false;
                }
            }
            function fnBMI() {
                var weight = document.getElementById("<%=txtPhysWeight.ClientID %>").value;
                var height = document.getElementById("<%=txtPhysHeight.ClientID %>").value;

                var bmi = weight / (height / 100 * height / 100);
                if (weight != "" && height != "") {
                    document.getElementById("<%=txtBMI.ClientID %>").value = Math.round(bmi, 2);
                }
            }

            function CalcualteBMI(txtBMI, txtWeight, txtHeight) {
                var weight = document.getElementById(txtWeight).value;
                var height = document.getElementById(txtHeight).value;
                if (weight == "" || height == "") {
                    weight = 0;
                    height = 0;
                }

                weight = parseFloat(weight);
                height = parseFloat(height);

                var BMI = weight / ((height / 100) * (height / 100));
                BMI = BMI.toFixed(2);
                document.getElementById(txtBMI).value = BMI;
            }
        </script>
        <div class="border center formbg">
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad5 whitebg" align="center" width="50%">
                            <label class="required left" id="lblVisitDate" runat="server">
                                *Visit Date:</label>
                            <asp:TextBox ID="txtVisitDate" MaxLength="11" Columns="8" runat="server"></asp:TextBox>
                            <img height="22" alt="Date Helper" onclick="w_displayDatePicker('<%=txtVisitDate.ClientID%>');"
                                hspace="5" src="../images/cal_icon.gif" width="22" border="0">
                            <span class="smallerlabel">(DD-MMM-YYYY)</span>
                            <label>
                                If Scheduled:</label>
                            <input id="chkifschedule" type="checkbox" runat="server" />
                        </td>
                        <td class="border pad5 whitebg" align="center">
                            <label class="center">
                                Visit Type:</label>
                            <asp:DropDownList ID="ddlvisittype" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg" align="center">
                            <label class="center">
                                If drug pick only, Treatment Supporter Name:
                            </label>
                            <asp:TextBox ID="txtTreatmentSupporterName" Columns="23" MaxLength="23" runat="server"></asp:TextBox>
                        </td>
                        <td class="border pad5 whitebg" align="center" valign="top">
                            <label class="center" runat="server">
                                Treatment Supporter Contact:
                            </label>
                            <asp:TextBox ID="txtTreatmentSupporterContact" Columns="23" MaxLength="23" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg formcenter" align="center" colspan="2">
                            <label class="center">
                                Duration Since:</label>
                            <strong>ART Start
                            </strong>
                            <asp:TextBox ID="txtARTStart" CssClass="margin10" MaxLength="3" Columns="2" ReadOnly="true"
                                runat="server"></asp:TextBox>
                            <span class="smalllabel">Months</span> <strong>Starting Current
                                Regimen  </strong>
                            <asp:TextBox ID="txtStartingCurrentRegimen" CssClass="margin10" MaxLength="3" Columns="2"
                                ReadOnly="true" runat="server"></asp:TextBox>
                              <span class="smalllabel">Months</span> 
                        </td>
                    </tr>
                    </tbody>
            </table>
        </div>
        <br/>
        <div class="border center formbg">
            <br/>
            <h2 class="forms" align="left">
                Clinical Status</h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad5 whitebg" colspan="2">
                            <table width="100%" border="0">
                                <tr>
                                <td align="center" style="width: 19%;">
                                    <label>Temperature:</label>
                                <asp:TextBox ID="txtphysTemp" runat="server" MaxLength="4" Columns="4"></asp:TextBox>
                                <span class="smallerlabel">C</span>
                                </td>
                                    <td align="center" style="width: 19%;">
                                        <label id="lblWeight" runat="server">
                                            Weight:</label>
                                        <asp:TextBox ID="txtPhysWeight" runat="server" MaxLength="5" Columns="4"></asp:TextBox>
                                        <span class="smallerlabel">kg</span>
                                    </td>
                                    <td align="center" style="width: 19%;">
                                        <label id="lblHeight" runat="server">
                                            Height:</label>
                                        <asp:TextBox ID="txtPhysHeight" runat="server" MaxLength="3" Columns="4"></asp:TextBox>
                                        <span class="smallerlabel">cm</span>
                                    </td>
                                    <td align="center" style="width: 19%;">
                                        <label id="lblBmi" runat="server">
                                            BMI:</label>
                                        <asp:TextBox ID="txtBMI" runat="server" MaxLength="6" Columns="4"></asp:TextBox>
                                    </td>
                                    <td align="center" style="width: 24%;" runat="server">
                                        <label>
                                            BP:</label>
                                        <asp:TextBox ID="txtBPSystolic" runat="server" MaxLength="3" Columns="4"></asp:TextBox>
                                        <label>
                                            /</label>
                                        <asp:TextBox ID="txtBPDiastolic" runat="server" MaxLength="3" Columns="4"></asp:TextBox>
                                        <span class="smallerlabel">(mm/Hg)</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg formcenter" id="tdPregnant" colspan="2" runat="server">
                            <table width="100%" border="0">
                                <tr align="center">
                                    <td align="center" style="width: 100%; display: inline-table">
                                        <label id="lblPregnant" runat="server">
                                            Pregnancy:
                                        </label>
                                        <asp:DropDownList ID="ddlpregnancy" onchange="fnchange()" runat="server">
                                        </asp:DropDownList>
                                        <span id="spanEDD" style="display: none">
                                            <label id="lblEDD" class="margin20" for="EDD">
                                                EDD:</label>
                                            <input id="txtEDD" runat="server" maxlength="11" size="11" />
                                            <img id="imgEDD" onclick="w_displayDatePicker('<%=txtEDD.ClientID%>');" height="22"
                                                alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="22" border="0"/>
                                            <span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            <label id="lblanc" for="anc" class="margin20">
                                                ANC No:</label>
                                            <asp:TextBox ID="txtANCNumber" runat="server" MaxLength="4" Columns="13"></asp:TextBox>
                                            <label id="lblrefpmtct" class="margin20">
                                                Referred to PMTCT (ac):
                                            </label>
                                            <asp:CheckBox ID="chkrefpmtct" runat="server" />
                                        </span>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td align="center" style="width: 100%;">
                                        <span id="miscarriage" style="display: none">
                                            <label id="Label2" class="margin20">
                                                Date of Miscarriage:
                                            </label>
                                            <input id="txtdatemiscarriage" runat="server" maxlength="11" size="11" />
                                            <img id="img1" onclick="w_displayDatePicker('<%=txtdatemiscarriage.ClientID%>');"
                                                height="22" alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="22"
                                                border="0"/>
                                            <span class="smallerlabel">(DD-MMM-YYYY)</span> </span><span id="Abortion" style="display: none">
                                                <label id="Label3" class="margin20">
                                                    Date of Induced Abortion:
                                                </label>
                                                <input id="txtdateinducedabortion" runat="server" maxlength="11" size="11" />
                                                <img id="img2" onclick="w_displayDatePicker('<%=txtdateinducedabortion.ClientID%>');"
                                                    height="22" alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="22"
                                                    border="0"/>
                                                <span class="smallerlabel">(DD-MMM-YYYY)</span> </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad10 whitebg" valign="top" align="left" colspan="2" id="tdFamilyPlanning"
                            runat="server">
                            <table width="100%" border="0">
                                <tr align="center">
                                    <td align="center" style="width: 50%;">
                                        <label class="margin20">
                                            Family Planning:</label>
                                        <asp:DropDownList ID="ddlFamilyPanningStatus" onchange="fnfamilyplanning();" runat="server">
                                        </asp:DropDownList>
                                        <div class="divborder checkbox" id="divFamilyPlanningMethod" style="display: none">
                                            <asp:Panel ID="PnlFamilyPlanningMethod" runat="server">
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td align="center" valign="top" style="width: 50%;">
                                        <span id="notfamilyplanning" style="display: none">
                                            <label class="margin20">
                                                Reason Not on Family Planning:
                                            </label>
                                            <asp:DropDownList ID="ddlnotfamilyplanning" runat="server">
                                            </asp:DropDownList>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg" colspan="2" width="100%" style="height: 35px">
                            <table width="100%" border="0">
                                <tr align="center">
                                    <td style="width: 100%;" align="center">
                                        <label id="lblTBStatus" runat="server">
                                            TB Status:</label>
                                        <asp:DropDownList ID="ddlTBStatus" onchange="fnTBStatus();" runat="server">
                                        </asp:DropDownList>
                                        <span id="tbstartdate" style="display: none">
                                            <label>
                                                TB Treatment Start Date:
                                            </label>
                                            <input id="txttbstartdate" runat="server" maxlength="11" size="11" />
                                            <img id="img3" onclick="w_displayDatePicker('<%=txttbstartdate.ClientID%>');" height="22"
                                                alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="22" border="0"/>
                                            <span class="smallerlabel">(DD-MMM-YYYY)</span> </span><span id="tbtreatment" style="display: none">
                                                <label>
                                                    TB Treatment #:
                                                </label>
                                                <input id="txtTBtreatmentNumber" maxlength="8" size="8" name="txtTBtreatmentNumber"
                                                    runat="server"/>
                                            </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg" colspan="2" width="100%" style="height: 35px"  >
                            <table width="100%" border="0">
                                <tr>
                                    <td class="border pad10 whitebg" align="left" width="50%" valign="top">
                                        <label class="margin20">
                                            Potential Side Effects:</label>
                                        <br />
                                        <div id="divPotentialSideEffect" class="divborder margin10" style="margin-top: 10;
                                            margin-bottom: 10">
                                            <asp:Panel ID="PnlPotentialSideEffect" runat="server">
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td class="border pad10 whitebg" width="50%" align="left" valign="top">
                                        <label class="margin20">
                                            New OIs, Other Problems:</label>
                                        <br />
                                        <div class="divborder margin20" style="margin-top: 10; margin-bottom: 10">
                                            <asp:Panel ID="PnlNewOIsProblemsOther" runat="server">
                                            </asp:Panel>
                                        </div>
                                        <label class="margin20" id="lblNutritionalProblems" style="visibility: hidden" runat="server">
                                            Nutritional Problems:</label>
                                        <asp:DropDownList ID="ddlNutritionalProblems" Style="visibility: hidden" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad10 whitebg" align="center" colspan="2" width="100%" valign="top">
                            <label id="lblWHOStage" runat="server">
                                WHO Stage:</label>
                            <asp:DropDownList ID="ddlWHOStage" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <br/>
            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                <tr>
                    <td style="width: 50%;">
                        <h2 id="H1" class="forms" align="left">
                            Pharmacy</h2>
                    </td>
                    <td style="width: 50%;">
                        <label class="margin10">
                            Use pharmacy form for INH dispensing and other medications dispensed.
                        </label>
                    </td>
                </tr>
            </table>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="form">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr align="left">
                                    <td style="width: 30%; display: inline" align="left">
                                        <label class="margin10">
                                            CTX:
                                        </label>
                                        <label class="margin10">
                                            Adherence:
                                        </label>
                                        <asp:DropDownList ID="ddlCotrimoxazoleAdhere" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td style="width: 100%; display: inline" align="left">
                                        <label class="margin10" id="lblARVDrugsAdhere" runat="server">
                                            ARV:
                                        </label>
                                        <label class="margin10">
                                            Adherence:
                                        </label>
                                        <asp:DropDownList ID="ddlarvdrugadhere" onchange="fnARVDrug();" runat="server">
                                        </asp:DropDownList>
                                        <label class="margin10">
                                            Why Poor/Fair:
                                        </label>
                                        <asp:DropDownList ID="ddlwhypoorfair" onchange="fnarvdrugother();" runat="server">
                                        </asp:DropDownList>
                                        <div id="divReasonARVDrugsother" style="display: none">
                                            <label class="right15">
                                                Other Reason:</label>
                                            <asp:TextBox ID="txtReasonARVDrugsPoorFairOther" runat="server" MaxLength="10" Columns="10"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td style="width: 30%; display: inline" align="left">
                                        <label class="required margin10">
                                            *Substitutions/Interruptions:
                                        </label>
                                        <asp:DropDownList ID="ddlsubsituationInterruption" onchange="fnSubsituations();"
                                            runat="server">
                                        </asp:DropDownList>
                                        <div id="arvTherapyChange" style="display: none">
                                            <label class="required margin10">
                                                *Change Regimen Reason:</label>
                                            <asp:DropDownList ID="ddlArvTherapyChangeCode" onchange="fnRegimenChange();" runat="server">
                                            </asp:DropDownList>
                                            <div id="otherarvTherapyChangeCode" style="display: none">
                                                <label class="required margin10" for="arvTherapyChangeCodeOtherName">
                                                    *Specify:</label>
                                                <input id="txtarvTherapyChangeCodeOtherName" maxlength="20" size="10" name="arvTherapyChangeCodeOtherName"
                                                    runat="server"/></div>
                                        </div>
                                        <div id="arvTherapyStop" style="display: none">
                                            <label id="lblrARTdate" class="required margin10">
                                                *Date ART Ended</label>
                                            <input id="txtARTEndeddate" runat="server" maxlength="11" size="10" name="txtARTEndeddate" />
                                            <img id="imgdate" onclick="w_displayDatePicker('<%=txtARTEndeddate.ClientID%>');"
                                                height="22" alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="22"
                                                border="0" /><span class="smallerlabel">(DD-MMM-YYYY)</span>
                                            <br />
                                            <br />
                                            <label class="required margin10">
                                                *Stop Regimen Reason:</label>
                                            <asp:DropDownList ID="ddlArvTherapyStopCode" onchange="fnStopReason();" runat="server">
                                            </asp:DropDownList>
                                            <div id="otherarvTherapyStopCode" style="display: none">
                                                <label class="required margin10" for="arvTherapyStopCodeOtherName">
                                                    *Specify:</label>
                                                <input id="txtarvTherapyStopCodeOtherName" maxlength="20" size="10" name="arvTherapyStopCodeOtherName"
                                                    runat="server"/></div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg center">
                            <div id="divAdultPharmacy" runat="server">
                                <asp:Button ID="btnpharmacy" Text="Prescribe Drugs" runat="server" OnClick="btnpharmacy_Click">
                                </asp:Button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <br/>
            <table cellspacing="2" cellpadding="0" width="100%" border="0">
                <tr>
                    <td style="width: 50%;">
                        <h2 id="H2" class="forms" align="left">
                            Laboratory Investigations</h2>
                    </td>
                    <td style="width: 50%;">
                        <label class="margin10">
                            Use lab order test button to order labs and document results.
                        </label>
                    </td>
                </tr>
            </table>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad5 whitebg formcenter" align="center" colspan="2">
                            <div id="divLaboratory" runat="server">
                                <asp:Button ID="btnLabratory" Text="Laboratory" runat="server" OnClick="btnLabratory_Click" />
                            </div>
                            <div id="divLabOrderTest" runat="server">
                                <asp:Button ID="btnOrderLabTest" Text="Order Lab Tests" runat="server" OnClick="btnOrderLabTest_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <br/>
            <h2 id="H3" class="forms" align="left">
                Referrals and Consultations</h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad10 whitebg" valign="top" width="50%" align="left">
                            <label class="margin20">
                                Referred To:
                            </label>
                            <div class="divborder margin20" style="margin-top: 10; margin-bottom: 10">
                                <div>
                                    <asp:Panel ID="PnlReferredTo" runat="server">
                                    </asp:Panel>
                                </div>
                            </div>
                        </td>
                        <td class="border pad5 whitebg" align="center">
                            <label id="Label17" class="margin15">
                                # of Days Hospitalized:</label>
                            <asp:TextBox ID="txtNumOfDaysHospitalized" runat="server" MaxLength="4" Columns="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg" align="center" width="50%">
                            <label>
                                Nutritional Support:</label>
                            <asp:DropDownList ID="ddlNutritionalSupport" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td id="tdInfantFeedingPractice" style="display: none" runat="server" class="border pad5 whitebg"
                            align="center">
                            <label>
                                Infant Feeding Practice:</label>
                            <asp:DropDownList ID="ddlInfantFeedingPractice" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div class="border center formbg">
            <br/>
            <h2 id="H4" class="forms" align="left">
                Positive Prevention
            </h2>
            <table cellspacing="6" cellpadding="0" width="100%" border="0">
                <tbody>
                    <tr>
                        <td class="border pad10 whitebg" align="left" valign="top" width="50%">
                            <label class="margin20">
                                At Risk Population:
                            </label>
                            <div class="divborder margin20" style="margin-top: 10; margin-bottom: 10">
                                <div>
                                    <asp:Panel ID="pnlriskpopulation" runat="server">
                                    </asp:Panel>
                                </div>
                            </div>
                        </td>
                        <td class="border pad10 whitebg" align="left" valign="top" width="50%">
                            <label class="margin20">
                                At Risk Population Services:
                            </label>
                            <div class="divborder margin20" style="margin-top: 10; margin-bottom: 10">
                                <div>
                                    <asp:Panel ID="pnlriskpopulationservice" runat="server">
                                    </asp:Panel>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad10 whitebg" valign="top" align="left" width="50%">
                            <label class="margin20">
                                Prevention with positives (PwP):
                            </label>
                            <div class="divborder margin20" style="margin-top: 10; margin-bottom: 10">
                                <div>
                                    <asp:Panel ID="pnlprewithpositive" runat="server">
                                    </asp:Panel>
                                </div>
                            </div>
                        </td>
                        <td class="border pad10 whitebg" valign="top" width="50%">
                        </td>
                    </tr>
                    <tr>
                        <td class="border pad5 whitebg" align="center" width="50%">
                            <label id="lblNextAppointment" class="center" runat="server">
                                Date of Next Appointment:
                            </label>
                            <input id="txtdatenextappointment" runat="server" maxlength="11" size="11" />
                            <img id="img4" onclick="w_displayDatePicker('<%=txtdatenextappointment.ClientID%>');"
                                height="22" alt="Date Helper" hspace="3" src="../images/cal_icon.gif" width="22"
                                border="0"/>
                            <span class="smallerlabel">(DD-MMM-YYYY)</span>
                        </td>
                        <td class="border pad5 whitebg" align="center" width="50%">
                            <label>
                                Attending Clinician:
                            </label>
                            <asp:DropDownList ID="ddlattendingclinician" runat="server">
                            </asp:DropDownList>
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
                        <td class="pad5 center whitebg border" colspan="2">
                            <br/>
                            <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" />
                            <asp:Button ID="btnDataQualityCheck" Text="Data Quality check" runat="server" OnClick="btnDataQualityCheck_Click" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" OnClick="btnClose_Click" />
                            <asp:Button ID="btnPrint" Text="Print" runat="server" OnClientClick="WindowPrint()" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
