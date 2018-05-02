<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlKNH_Extruder.ascx.cs" Inherits="PresentationApp.ClinicalForms.UserControl.UserControlKNH_Extruder" %>
<%@ Register src="UserControlKNH_DrugAllergies.ascx" tagname="UserControlKNH_DrugAllergies" tagprefix="uc1" %>
<%@ Register src="UserControl_VitalsExtruder.ascx" tagname="UserControl_VitalsExtruder" tagprefix="uc2" %>
<%@ Register src="UserControl_AllergyExtruder.ascx" tagname="UserControl_AllergyExtruder" tagprefix="uc3" %>
<%@ Register src="UserControlKNH_LabResults.ascx" tagname="UserControlKNH_LabResults" tagprefix="uc4" %>
<style type="text/css">
        body{
            font-family:Arial, Helvetica, sans-serif;
            margin:10px;
        }
        .wrapper{
            position:relative;
            font-family:Arial, Helvetica, sans-serif;
            padding-top:90px;
            padding-left:50px;
            width:80%;
            margin:auto
        }
        .wrapper .text{
            font-family:Arial, Helvetica, sans-serif;
            padding-top:50px;
        }
        .wrapper h1{
            font-family:Arial, Helvetica, sans-serif;
            font-size:26px;
        }
        .longText{
            margin-top:20px;
            width:600px;
            font:18px/24px Arial, Helvetica, sans-serif;
            color:gray;
        }
        span.btn{
            padding:10px;
            display:inline-block;
            cursor:pointer;
            font:12px/14px Arial, Helvetica, sans-serif;
            color:#aaa;
            background-color:#eee;
            -moz-border-radius:10px;
            -webkit-border-radius:10px;
            -moz-box-shadow:#999 2px 0px 3px;
            -webkit-box-shadow:#999 2px 0px 3px;
        }
        span.btn:hover{
            background-color:#000;
        }

            /*
            custom style for extruder
            */

        .extruder.left.a .flap{
            font-size:18px;
            color:white;
            top:0;
            padding:10px 0 10px 10px;
            background:#772B14;
            width:30px;
            position:absolute;
            right:0;
            -moz-border-radius:0 10px 10px 0;
            -webkit-border-top-right-radius:10px;
            -webkit-border-bottom-right-radius:10px;
            -moz-box-shadow:#666 2px 0px 3px;
            -webkit-box-shadow:#666 2px 0px 3px;
        }

        .extruder.left.a .content{
            border-right:3px solid #772B14;
        }

        .extruder.top .optionsPanel .panelVoice a:hover{
            color:#fff;
            background: url("elements/black_op_30.png");
            border-bottom:1px solid #000;
        }
        .extruder.top .optionsPanel .panelVoice a{
            border-bottom:1px solid #000;
        }

        .extruder.left.a .flap .flapLabel{
            background:#772B14;
        }
    </style>

<%--<link href="../../Touch/styles/mbExtruder.css" media="all" rel="stylesheet" type="text/css" />--%>
<link href="mbExtruder.css" rel="stylesheet" type="text/css" /> 
<script src="../Touch/scripts/jquery-1.10.1.js" type="text/javascript"></script>
<script src="../Touch/styles/custom-theme/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
<script src="../Touch/scripts/jquery.mb.flipText.js" type="text/javascript"></script>
<script src="../Touch/scripts/mbExtruder.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {

            $("#extruderLeft").buildMbExtruder({
                position: "left",
                width: 300,
                //extruderOpacity: .8,
                hidePanelsOnClose: true,
                accordionPanels: true,
                onExtOpen: function () { },
                onExtContentLoad: function () { },
                onExtClose: function () { }
             });

            $("#extruderLeft3").buildMbExtruder({
                position: "left",
                width: 200,
                //extruderOpacity: .8,
                onExtOpen: function () { },
                onExtContentLoad: function () { },
                onExtClose: function () { }
            });

            $("#extruderLeft1").buildMbExtruder({
                position: "left",
                width: 300,
                //extruderOpacity: .8,
                onExtOpen: function () { },
                onExtContentLoad: function () { },
                onExtClose: function () { }
            });

            $("#extruderLeft2").buildMbExtruder({
                position: "left",
                width: 300,
                //extruderOpacity: 1,
                onExtOpen: function () { },
                onExtContentLoad: function () { },
                onExtClose: function () { }
            });
        });

    </script>

    <div>
    <div class="longText" style="position:relative;padding-left:50px">
        <div id="extruderLeft" class="{title:'Allergies / ARV History'}">
            <uc2:UserControl_VitalsExtruder ID="UserControl_VitalsExtruder1" 
                runat="server" />
        </div>
        <%--<div id="extruderLeft3" class="{title:'Vitals'}">
        </div>--%>
         <div id="extruderLeft1" class="a {title:'Lab Results'}">
            <uc4:UserControlKNH_LabResults ID="UserControlKNH_LabResults1" runat="server" /></div>
        <%--<div id="extruderLeft2" class="a {title:'Allergies'}">
            <uc3:UserControl_AllergyExtruder ID="UserControl_AllergyExtruder1" 
                runat="server" />
        </div>--%>
        
    </div>

</div>