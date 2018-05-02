<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_VitalSigns.ascx.cs"
    Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_VitalSigns" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="act" Namespace="AjaxControlToolkit" %>
<script type="text/javascript">

    function CalcualteBMI(age, txtWeight, txtHeight, txtBMI, lblBMIClassification) {
        var weight = document.getElementById(txtWeight).value;
        var height = document.getElementById(txtHeight).value;
        if (weight == "" || height == "") {
            weight = 0;
            height = 0;
            document.getElementById(txtBMI).value = "";
        }
        else {
            var BMI = weight / ((height / 100) * (height / 100));
            BMI = BMI.toFixed(2);
            document.getElementById(txtBMI).value = BMI;

            if (age > 15) {
                if (BMI < 18.5) {
                    //underweight
                    document.getElementById(lblBMIClassification).innerHTML = 'UnderWeight';
                    document.getElementById(lblBMIClassification).style.color = "red";
                    document.getElementById(txtBMI).style.color = "red";
                    document.getElementById(lblBMIClassification).style.fontWeight = 'bold';
                }

                if (BMI >= 18.5 && BMI < 25) {
                    document.getElementById(lblBMIClassification).innerHTML = 'Normal';
                    document.getElementById(lblBMIClassification).style.color = "black";
                    document.getElementById(txtBMI).style.color = "black";
                }

                if (BMI > 25 && BMI < 30) {
                    //OverWeight
                    document.getElementById(lblBMIClassification).innerHTML = 'OverWeight';
                    document.getElementById(lblBMIClassification).style.color = "red";
                    document.getElementById(txtBMI).style.color = "red";
                    document.getElementById(lblBMIClassification).style.fontWeight = 'bold';
                }

                if (BMI >= 30) {
                    //Obese
                    document.getElementById(lblBMIClassification).innerHTML = 'Obese';
                    document.getElementById(lblBMIClassification).style.color = "red";
                    document.getElementById(txtBMI).style.color = "red";
                    document.getElementById(lblBMIClassification).style.fontWeight = 'bold';
                }
            }
        }
    }

    function CalculateZScores() {

    }




    function HighLightAbnormalValuesAdults(txtTemp, txtRR, txtHR, txtBPSystolic, txtBPDiastolic) {
        //temperature
        var tempValue = document.getElementById(txtTemp).value;
        var temp = document.getElementById(txtTemp);
        if (tempValue < 36.1 || tempValue > 37.8) {
            temp.style.color = "red";
            temp.style.fontWeight = 'bold';
            //temp.style.fontSize = "large"
        }
        else {
            temp.style.color = "black";
            temp.style.fontWeight = 'normal';
        }

        //Respiratory Rate
        var RRValue = document.getElementById(txtRR).value;
        var RR = document.getElementById(txtRR);
        if (RRValue < 16 || RRValue > 18) {
            RR.style.color = "red";
            temp.style.fontWeight = 'bold';
            //temp.style.fontSize = "large"
        }
        else {
            RR.style.color = "black";
            temp.style.fontWeight = 'normal';
        }

        //Heart Rate
        var HRValue = document.getElementById(txtHR).value;
        var HR = document.getElementById(txtHR);
        if (HRValue < 60 || HRValue > 100) {
            HR.style.color = "red";
            temp.style.fontWeight = 'bold';
            //temp.style.fontSize = "large"
        }
        else {
            HR.style.color = "black";
            temp.style.fontWeight = 'normal';
        }

        //Systolic
        var SystolicValue = document.getElementById(txtBPSystolic).value;
        var Systolic = document.getElementById(txtBPSystolic);
        if (SystolicValue < 90 || SystolicValue > 120) {
            Systolic.style.color = "red";
            temp.style.fontWeight = 'bold';
            //temp.style.fontSize = "large"
        }
        else {
            Systolic.style.color = "black";
            temp.style.fontWeight = 'normal';
        }

        //Diastolic
        var DiastolicValue = document.getElementById(txtBPDiastolic).value;
        var Diastolic = document.getElementById(txtBPDiastolic);
        if (DiastolicValue < 60 || DiastolicValue > 80) {
            Diastolic.style.color = "red";
            temp.style.fontWeight = 'bold';
            //temp.style.fontSize = "large"
        }
        else {
            Diastolic.style.color = "black";
            temp.style.fontWeight = 'normal';
        }

    }


    function HighLightAbnormalValuesPeads(age, txtTemp, txtRR, txtHR, txtBPSystolic, txtBPDiastolic) {
        //alert(age);
        //var age = '<%= Session["patientageinyearmonth"] %>';
        //temperature
        var tempValue = document.getElementById(txtTemp).value;
        var temp = document.getElementById(txtTemp);
        if (tempValue < 36.1 || tempValue > 37.8) {
            temp.style.color = "red";
            temp.style.fontWeight = 'bold';
            //temp.style.fontSize = "large"
        }
        else {
            temp.style.color = "black";
            temp.style.fontWeight = 'normal';
        }

        //Respiratory Rate
        var RRValue = document.getElementById(txtRR).value;
        var RR = document.getElementById(txtRR);
        if (age >= 0.0 || age <= 0.167) {
            if (RRValue < 35 || RRValue > 60) {
                RR.style.color = "red";
                RR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                RR.style.color = "black";
                RR.style.fontWeight = 'normal';
            }
        }

        if (age > 0.167 || age <= 0.99) {
            if (RRValue < 30 || RRValue > 50) {
                RR.style.color = "red";
                RR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                RR.style.color = "black";
                RR.style.fontWeight = 'normal';
            }
        }

        if (age > 0.99 || age <= 2.0) {
            if (RRValue < 24 || RRValue > 40) {
                RR.style.color = "red";
                RR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                RR.style.color = "black";
                RR.style.fontWeight = 'normal';
            }
        }

        if (age > 2.0 || age <= 5.0) {
            if (RRValue < 22 || RRValue > 34) {
                RR.style.color = "red";
                RR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                RR.style.color = "black";
                RR.style.fontWeight = 'normal';
            }
        }

        if (age > 5.0 || age <= 12.0) {
            if (RRValue < 18 || RRValue > 30) {
                RR.style.color = "red";
                RR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                RR.style.color = "black";
                RR.style.fontWeight = 'normal';
            }
        }

        if (age > 12.0) {
            if (RRValue < 12 || RRValue > 16) {
                RR.style.color = "red";
                RR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                RR.style.color = "black";
                RR.style.fontWeight = 'normal';
            }
        }


        //Heart Rate
        var HRValue = document.getElementById(txtHR).value;
        var HR = document.getElementById(txtHR);
        if (age >= 0 || age <= 0.167) {
            if (HRValue < 100 || HRValue > 160) {
                HR.style.color = "red";
                HR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                HR.style.color = "black";
                HR.style.fontWeight = 'normal';
            }
        }

        if (age > 0.167 || age <= 0.99) {
            if (HRValue < 100 || HRValue > 160) {
                HR.style.color = "red";
                HR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                HR.style.color = "black";
                HR.style.fontWeight = 'normal';
            }
        }

        if (age > 0.99 || age <= 2) {
            if (HRValue < 90 || HRValue > 150) {
                HR.style.color = "red";
                HR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                HR.style.color = "black";
                HR.style.fontWeight = 'normal';
            }
        }

        if (age > 2 || age <= 5) {
            if (HRValue < 80 || HRValue > 140) {
                HR.style.color = "red";
                HR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                HR.style.color = "black";
                HR.style.fontWeight = 'normal';
            }
        }

        if (age > 5.0 || age <= 12) {
            if (HRValue < 70 || HRValue > 120) {
                HR.style.color = "red";
                HR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                HR.style.color = "black";
                HR.style.fontWeight = 'normal';
            }
        }

        if (age > 12) {
            if (HRValue < 60 || HRValue > 100) {
                HR.style.color = "red";
                HR.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                HR.style.color = "black";
                HR.style.fontWeight = 'normal';
            }
        }




        //Systolic
        var SystolicValue = document.getElementById(txtBPSystolic).value;
        var Systolic = document.getElementById(txtBPSystolic);
        if (age >= 0 || age <= 0.25) {
            if (SystolicValue < 65 || SystolicValue > 85) {
                Systolic.style.color = "red";
                Systolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Systolic.style.color = "black";
                Systolic.style.fontWeight = 'normal';
            }
        }

        if (age > 0.25 || age <= 0.5) {
            if (SystolicValue < 70 || SystolicValue > 90) {
                Systolic.style.color = "red";
                Systolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Systolic.style.color = "black";
                Systolic.style.fontWeight = 'normal';
            }
        }

        if (age > 0.5 || age <= 1) {
            if (SystolicValue < 80 || SystolicValue > 100) {
                Systolic.style.color = "red";
                Systolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Systolic.style.color = "black";
                Systolic.style.fontWeight = 'normal';
            }
        }

        if (age > 1 || age <= 3) {
            if (SystolicValue < 90 || SystolicValue > 105) {
                Systolic.style.color = "red";
                Systolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Systolic.style.color = "black";
                Systolic.style.fontWeight = 'normal';
            }
        }

        if (age > 3 || age <= 6) {
            if (SystolicValue < 95 || SystolicValue > 110) {
                Systolic.style.color = "red";
                Systolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Systolic.style.color = "black";
                Systolic.style.fontWeight = 'normal';
            }
        }

        if (age > 6 || age <= 12) {
            if (SystolicValue < 100 || SystolicValue > 120) {
                Systolic.style.color = "red";
                Systolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Systolic.style.color = "black";
                Systolic.style.fontWeight = 'normal';
            }
        }

        if (age > 12) {
            if (SystolicValue < 110 || SystolicValue > 135) {
                Systolic.style.color = "red";
                Systolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Systolic.style.color = "black";
                Systolic.style.fontWeight = 'normal';
            }
        }


        //Diastolic
        var DiastolicValue = document.getElementById(txtBPDiastolic).value;
        var Diastolic = document.getElementById(txtBPDiastolic);
        if (age > 0 || age <= 0.25) {
            if (DiastolicValue < 45 || DiastolicValue > 55) {
                Diastolic.style.color = "red";
                Diastolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Diastolic.style.color = "black";
                Diastolic.style.fontWeight = 'normal';
            }
        }

        if (age > 0.25 || age <= 0.5) {
            if (DiastolicValue < 50 || DiastolicValue > 65) {
                Diastolic.style.color = "red";
                Diastolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Diastolic.style.color = "black";
                Diastolic.style.fontWeight = 'normal';
            }
        }

        if (age > 0.5 || age <= 1) {
            if (DiastolicValue < 55 || DiastolicValue > 65) {
                Diastolic.style.color = "red";
                Diastolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Diastolic.style.color = "black";
                Diastolic.style.fontWeight = 'normal';
            }
        }

        if (age > 1 || age <= 3) {
            if (DiastolicValue < 55 || DiastolicValue > 70) {
                Diastolic.style.color = "red";
                Diastolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Diastolic.style.color = "black";
                Diastolic.style.fontWeight = 'normal';
            }
        }

        if (age > 3 || age <= 6) {
            if (DiastolicValue < 60 || DiastolicValue > 75) {
                Diastolic.style.color = "red";
                Diastolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Diastolic.style.color = "black";
                Diastolic.style.fontWeight = 'normal';
            }
        }

        if (age > 6 || age <= 12) {
            if (DiastolicValue < 60 || DiastolicValue > 75) {
                Diastolic.style.color = "red";
                Diastolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Diastolic.style.color = "black";
                Diastolic.style.fontWeight = 'normal';
            }
        }

        if (age > 12) {
            if (DiastolicValue < 65 || DiastolicValue > 85) {
                Diastolic.style.color = "red";
                Diastolic.style.fontWeight = 'bold';
                //temp.style.fontSize = "large"
            }
            else {
                Diastolic.style.color = "black";
                Diastolic.style.fontWeight = 'normal';
            }
        }

    }

    function show_hide(controlID, status) {
        var s = document.getElementById(controlID);
        if (status == "notvisible") {
            s.style.display = "none";
        }
        else {
            s.style.display = "block";
        }
    }

    function TriageCheckBoxHideUnhideOtherSpecialistClinic(strcblcontrolId, txtControlId) {

        var checkList = document.getElementById(strcblcontrolId);
        var checkBoxList = checkList.getElementsByTagName("input");
        var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
        var checkBoxSelectedItems = new Array();

        for (var i = 0; i < checkBoxList.length; i++) {

            if (checkBoxList[i].checked) {
                if (arrayOfCheckBoxLabels[i].innerHTML == 'Other Specialist Clinic') {
                    //alert(arrayOfCheckBoxLabels[i].innerHTML + ' show ' + strdivId);
                    ShowHide('TriagedivReferToSpecialistClinic', "show");
                    return;
                }
                else {
                    //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                    document.getElementById(txtControlId).value = '';
                    ShowHide('TriagedivReferToSpecialistClinic', "hide");
                }
            }
            else {
                //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                document.getElementById(txtControlId).value = '';
                ShowHide('TriagedivReferToSpecialistClinic', "hide");
            }
        }
    }

    function TriageCheckBoxHideUnhideOtherReferral(strcblcontrolId, txtControlId) {

        var checkList = document.getElementById(strcblcontrolId);
        var checkBoxList = checkList.getElementsByTagName("input");
        var arrayOfCheckBoxLabels = checkList.getElementsByTagName("label");
        var checkBoxSelectedItems = new Array();

        for (var i = 0; i < checkBoxList.length; i++) {

            if (checkBoxList[i].checked) {
                if (arrayOfCheckBoxLabels[i].innerHTML == 'Other (Specify)') {
                    //alert(arrayOfCheckBoxLabels[i].innerHTML + ' show ' + strdivId);
                    ShowHide('TriagedivReferToOther', "show");
                    return;
                }
                else {
                    //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                    document.getElementById(txtControlId).value = '';
                    ShowHide('TriagedivReferToOther', "hide");
                }
            }
            else {
                //alert(arrayOfCheckBoxLabels[i].innerHTML + ' hide ' + strdivId);
                document.getElementById(txtControlId).value = '';
                ShowHide('TriagedivReferToOther', "hide");
            }
        }
    }

    

</script>

<div class="center formbg">
    
    <%--<table cellspacing="6" cellpadding="0" width="100%" border="0">
         <tr>
         <td>
         <asp:Panel ID="VitalSignsHeader" runat="server" style="padding:6px" cssclass="border">
        <h2 class="forms" align="left">
            <asp:ImageButton ID="imgVitalSigns" runat="server" ImageUrl="~/images/arrow-up.gif" />
            &nbsp;<asp:Label ID="lblVitalSigns" runat="server" Text="Vital Signs"></asp:Label>
        </h2>
    </asp:Panel>
         </td>
         </tr>
         </table>--%>
    
    <asp:Panel ID="VitalSignsUserControlBody" runat="server">
        <table cellspacing="6" cellpadding="0" width="100%" border="0">
            <tbody>
                <tr>
                    <td class="form center" valign="middle">
                        <table width="100%">
                            <tr>
                                <td width="25%" align="center">
                                    <asp:Label ID="lblTemp" runat="server" 
                                        Font-Bold="True" Text="Temperature:"></asp:Label>
                                    &nbsp;<asp:TextBox ID="txtTemp" runat="server" Width="35px"></asp:TextBox>&nbsp;<span class="smallerlabel">C</span>
                                </td>
                                <td width="25%" align="center">
                                    <label class="margin5">
                                        RR:</label><asp:TextBox ID="txtRR" runat="server" Width="35px"></asp:TextBox>
                                    &nbsp;<span class="smallerlabel">bpm</span>
                                </td>
                                <td width="25%" align="center">
                                    <label class="margin5">
                                        HR:</label>
                                    <asp:TextBox ID="txtHR" runat="server" Width="35px"></asp:TextBox>
                                    <span class="smallerlabel">&nbsp;bpm</span>
                                </td>
                                <td width="25%" align="center">
                                    <asp:Label ID="lblBP" runat="server" CssClass="required" 
                                        Font-Bold="True" Text="*BP:"></asp:Label>
&nbsp;<asp:TextBox ID="txtBPSystolic" runat="server" Width="35px"></asp:TextBox>
                                    &nbsp;/
                                    <asp:TextBox ID="txtBPDiastolic" runat="server" Width="35px"></asp:TextBox>
                                    <span class="smallerlabel">&nbsp;(mm/Hg)</span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="form" align="center" valign="middle">
                        <table width="100%">
                            <tr>
                                <td width="33%" align="center">
                                    <asp:Label ID="lblHeight" runat="server" CssClass="required" 
                                        Font-Bold="True" Text="*Height:"></asp:Label>
                                    &nbsp;<asp:TextBox ID="txtHeight" runat="server" Width="40px" 
                                        ontextchanged="txtHeight_TextChanged" AutoPostBack="True"></asp:TextBox>
                                    &nbsp;<span class="smallerlabel">cm</span>
                                </td>
                                <td width="34%" align="center">
                                    <asp:Label ID="lblWeight" runat="server" CssClass="required" 
                                        Font-Bold="True" Text="*Weight:"></asp:Label>
                                    &nbsp;<asp:TextBox ID="txtWeight" runat="server" Width="40px" 
                                        AutoPostBack="True" ontextchanged="txtWeight_TextChanged"></asp:TextBox>
                                    &nbsp;<span class="smallerlabel">kg</span>
                                </td>
                                <td width="33%" align="center">
                                    <label class="margin5">
                                        BMI:</label>
                                    <asp:TextBox ID="txtBMI" runat="server" Width="50px" ReadOnly="True"></asp:TextBox>
                                    &nbsp;
                                    <asp:Label ID="lblBMIClassification" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td id="divshowvitalsign" class="form" align="center" valign="middle">
                        <table width="100%">
                                <tr>
                                    <td align="center" width="25%">
                                        <label id="Label1">
                                            Head circumference:</label>
                                        <asp:TextBox ID="txtheadcircumference" runat="server" Width="40px"></asp:TextBox>
                                    </td>
                                    <td align="center" width="25%">
                                    <table>
                                    <tr>
                                    <td>
                                        <asp:Label ID="lblWALabel" runat="server" Text="Weight for age:" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                    <asp:UpdatePanel ID="UpdatePanelWA" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblWA" runat="server" Font-Bold="True"></asp:Label>
                                            &nbsp;
                                            <asp:Label ID="lblWAClassification" runat="server" Font-Bold="True"></asp:Label>

                                            &nbsp;
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtHeight" EventName="TextChanged" />
                                         </Triggers>
                                         <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtWeight" EventName="TextChanged" />
                                         </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    </tr>
                                    </table>
                                        
                                        
                                        
                                        
                                        
                                    </td>
                                    <td align="center" width="25%">
                                    <table>
                                    <tr>
                                    <td>
                                    <asp:Label ID="lblWHLabel" runat="server" Text="Weight for height:" Font-Bold="True"></asp:Label>
                                    
                                    </td>
                                    <td>
                                    <asp:UpdatePanel ID="UpdatePanelWH" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblWH" runat="server" Font-Bold="True"></asp:Label>
                                            &nbsp;
                                            <asp:Label ID="lblWHClassification" runat="server" Font-Bold="True"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtHeight" EventName="TextChanged" />
                                            
                                         </Triggers>
                                         <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtWeight" EventName="TextChanged" />
                                         </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    </tr>
                                    </table>   
                                    </td>
                                    <td valign="middle" align="center" width="25%">      
                                    <table>
                                    <tr>
                                    <td>
                                    <asp:Label ID="lblBMIzLabel" runat="server" Font-Bold="True" Text="BMI ZScore:"></asp:Label>
                                    </td>
                                    <td>
                                    <asp:UpdatePanel ID="UpdatePanelBMIz" runat="server">
                                    <ContentTemplate>
                                    <asp:Label ID="lblBMIz" runat="server" Font-Bold="True"></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblBMIzClassification" runat="server" Font-Bold="True"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="txtHeight" EventName="TextChanged" />
                                    </Triggers>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="txtWeight" EventName="TextChanged" />
                                    </Triggers>
                                    </asp:UpdatePanel>
                                    </td>
                                    </tr>
                                    </table>                 
                                </td>
                                </tr>
                            </table>
                        
                    </td>
                </tr>
                <tr>
                <td class="form">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <label class="">
                                Nurse comments:</label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtnursescomments" TextMode="MultiLine" Width="100%" 
                                runat="server" Rows="4"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                    </td>
                    </tr>
                    <tr>
                    <td class="form">
                    <table width="100%">
                    <tr>
                        <td align="left">
                            <table width="100%">
                                <tr>
                                <td align="left">
                                <label>Patient referred to:</label>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                    <div class="customdivbordermultiselect" nowrap="noWrap">
                                    <asp:CheckBoxList ID="cblReferredTo" runat="server">
                                    </asp:CheckBoxList>
                                    </div>
                                </td>
                                </tr>
                                    <tr id="TriagedivReferToSpecialistClinic" style="display: none">
                                        <td align="left">
                                            <label>
                                            Refer to specialist clinic:</label><asp:TextBox 
                                                ID="txtReferToSpecialistClinic" runat="server" Width="60%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="TriagedivReferToOther" style="display: none">
                                        <td align="left">
                                            <label>
                                            Specify other referral point:<asp:TextBox ID="txtSpecifyOtherRefferedTo" 
                                                runat="server" Skin="MetroTouch" Width="60%"></asp:TextBox>
                                            </label></td>
                                    </tr>
                                </table>
                        </td>
                    </tr>
                </table>
                </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    <%--<act:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" SuppressPostBack="true"
        ExpandedImage="~/images/arrow-dn.gif" TargetControlID="VitalSignsBody" CollapseControlID="VitalSignsHeader"
        ExpandControlID="VitalSignsHeader" CollapsedImage="~/images/arrow-up.gif" Collapsed="true"
        ImageControlID="imgVitalSigns">
    </act:CollapsiblePanelExtender>--%>
</div>
