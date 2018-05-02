<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_Extruder.ascx.cs"
    Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_Extruder" %>
<%@ Register Src="UserControlKNH_DrugAllergies.ascx" TagName="UserControlKNH_DrugAllergies"
    TagPrefix="uc1" %>
<%@ Register Src="UserControl_VitalsExtruder.ascx" TagName="UserControl_VitalsExtruder"
    TagPrefix="uc2" %>
<%@ Register Src="UserControl_AllergyExtruder.ascx" TagName="UserControl_AllergyExtruder"
    TagPrefix="uc3" %>
<%@ Register Src="UserControlKNH_LabResults.ascx" TagName="UserControlKNH_LabResults"
    TagPrefix="uc4" %>
<%@ Register Src="UserControlKNH_WorkPlanExtruder.ascx" TagName="UserControlKNH_WorkPlanExtruder"
    TagPrefix="uc5" %>
<%@ Register Src="UserControl_Nutrition.ascx" TagName="UserControl_Nutrition" TagPrefix="uc6" %>
<style type="text/css">
    body
    {
        font-family: Arial, Helvetica, sans-serif;
        margin: 10px;
    }
    .wrapper
    {
        position: relative;
        padding-top: 90px;
        padding-left: 50px;
        width: 80%;
        margin: auto;
    }
    .wrapper .text
    {
        padding-top: 50px;
    }
    .wrapper h1
    {
        font-size: 26px;
    }
    .longText
    {
        margin-top: 20px;
        width: 600px;
        color: gray;
    }
    span.btn
    {
        padding: 10px;
        display: inline-block;
        cursor: pointer;
        font: 12px/14px Arial, Helvetica, sans-serif;
        color: #aaa;
        background-color: #eee;
        -moz-border-radius: 10px;
        -webkit-border-radius: 10px;
        -moz-box-shadow: #999 2px 0px 3px;
        -webkit-box-shadow: #999 2px 0px 3px;
    }
    span.btn:hover
    {
        background-color: #000;
    }
    
    /*
            custom style for extruder
            */
    
    .extruder.left.a .flap
    {
        font-size: 18px;
        color: white;
        top: 0;
        padding: 10px 0 10px 10px;
        
        width: 30px;
        position: absolute;
        right: 0;
        -moz-border-radius: 0 10px 10px 0;
        -webkit-border-top-right-radius: 10px;
        -webkit-border-bottom-right-radius: 10px;
        -moz-box-shadow: #666 2px 0px 3px;
        -webkit-box-shadow: #666 2px 0px 3px;
    }
    
    .extruder.left.a .content
    {
        border-right: 3px solid #772B14;
    }
    
    .extruder.top .optionsPanel .panelVoice a:hover
    {
        color: #fff;
        background: url("elements/black_op_30.png");
        border-bottom: 1px solid #000;
    }
    .extruder.top .optionsPanel .panelVoice a
    {
        border-bottom: 1px solid #000;
    }
    
    .extruder.left.a .flap .flapLabel
    {
        background: #2C7F96;
    }
     .extruder.left.a1 .flap
    {
        background: #4765EB;
    }
    .extruder.left.a1 .flapLabel
    {
        background: #4765EB;
    }
     .extruder.left.a2 .flap 
    {
        background: #A62241;
    }
    .extruder.left.a2 .flapLabel
    {
        background: #A62241;
    }
</style>

<script type="text/javascript">
//    $(document).ready(function () {
    $(function () {
        //debugger;
        $("#extruderLeft").buildMbExtruder({
            position: "left",
            width: "400px",
            //extruderOpacity: .8,
            hidePanelsOnClose: true,
            accordionPanels: true,
            onExtOpen: function () { },
            onExtContentLoad: function () { },
            onExtClose: function () { }
        });

        $("#extruderLeft3").buildMbExtruder({
            position: "left",
            width: 600,
            //extruderOpacity: .8,
            onExtOpen: function () { },
            onExtContentLoad: function () { },
            onExtClose: function () { }
        });

        $("#extruderLeft1").buildMbExtruder({
            position: "left",
            width: 400,
            //extruderOpacity: .8,
            onExtOpen: function () { },
            onExtContentLoad: function () { },
            onExtClose: function () { }
        });
        
        var b_ISKNH = "<%=Session["isKNHEnabled"]%>";
        //debugger;
        if (b_ISKNH=="True") {
            $("#extruderLeft2").buildMbExtruder({
                position: "left",
                width: 400,
                //extruderOpacity: 1,
                onExtOpen: function () { },
                onExtContentLoad: function () { },
                onExtClose: function () { }
            });
        }
        else
        {
            document.getElementById("extruderLeft2").style.display = "none";
        }
             
    });

</script>
<div>
    <div class="longText" style="position: relative; padding-left: 50px">
        <div id="extruderLeft" class="{title:'Allergies / ARV History'}" style="background-color:#A62241;">
            <uc2:UserControl_VitalsExtruder ID="UserControl_VitalsExtruder1" runat="server" />
        </div>
        <div id="extruderLeft1" class="a1 {title:'Lab Results'}" style="background-color:#4765EB;">
            <uc4:UserControlKNH_LabResults ID="UserControlKNH_LabResults1" runat="server" />
        </div>
        <div id="extruderLeft2" class="a2 {title:'Work Plan'}" style="background-color:#2C7F96;">
            <uc5:UserControlKNH_WorkPlanExtruder ID="UserControlKNH_WorkPlanExtruder1" runat="server" />
        </div>
        <%--<div id="extruderLeft3" class="a {title:'Nutrition'}">
            <uc6:UserControl_Nutrition ID="UserControl_Nutrition1" runat="server" />
        </div>--%>
    </div>
</div>
