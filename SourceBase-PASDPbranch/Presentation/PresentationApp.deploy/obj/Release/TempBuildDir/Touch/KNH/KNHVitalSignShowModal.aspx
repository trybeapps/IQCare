<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KNHVitalSignShowModal.aspx.cs" Inherits="PresentationApp.Touch.Custom_Forms.KNHVitalSignShowModal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title></title>
    <link href="../styles/KNH.css" rel="stylesheet" type="text/css" />



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
            var hash = hashes[0].split('=');
            var x = hash[0];
            var queryString = x.split('-');
            fromName = queryString[0];
            flag = queryString[1];
         oArg.RadTemperature = document.getElementById("txtRadTemperatureModal").value;
         oArg.RadRespirationRate = document.getElementById("txtRadRespirationRate").value;
         oArg.RadHeartRate = document.getElementById("txtRadHeartRate").value;
         oArg.RadSystollicBloodPressure = document.getElementById("txtRadSystollicBloodPressure").value;
         oArg.RadDiastolicBloodPressure = document.getElementById("txtRadDiastolicBloodPressure").value;
         oArg.RadHeight = document.getElementById("txtRadHeight").value;
         oArg.RadWeight = document.getElementById("txtRadWeight").value;
         oArg.Form_Name = fromName;
         oArg.flagmode = flag;
         




          
            //get a reference to the current RadWindow
            var oWnd = GetRadWindow();

            //Close the RadWindow and send the argument to the parent page

            if (oArg.RadTemperature && oArg.RadRespirationRate && oArg.RadHeartRate && oArg.RadSystollicBloodPressure && oArg.RadDiastolicBloodPressure && oArg.RadHeight && oArg.RadWeight) {

                oWnd.close(oArg);

            }

            else {

                alert("Please fill All fields");

            }

        }



        function setValues() {

           var hashes = window.location.href.slice(window.location.href.indexOf('?') + 5).split('&');
           hash = hashes[0].split('=');
           var x = hash[0];
            var queryString = x.split('-');
            fromName = queryString[0];
           // flag = queryString[1];

           
            

            document.getElementById('txtRadTemperatureModal').value = parent.document.getElementById(fromName+'_HiddRadTemperature').value;
            document.getElementById('txtRadRespirationRate').value = parent.document.getElementById(fromName + '_HiddRadRespirationRate').value;
            document.getElementById('txtRadHeartRate').value = parent.document.getElementById(fromName + '_HiddRadHeartRate').value;
            document.getElementById('txtRadSystollicBloodPressure').value = parent.document.getElementById(fromName + '_HiddRadSystollicBloodPressure').value;
            document.getElementById('txtRadDiastolicBloodPressure').value = parent.document.getElementById(fromName + '_HiddRadDiastolicBloodPressure').value;
            document.getElementById('txtRadHeight').value = parent.document.getElementById(fromName + '_HiddRadHeight').value;
            document.getElementById('txtRadWeight').value = parent.document.getElementById(fromName + '_HiddRadWeight').value;

        }
        
    
    </script>
</head>
<body  onload="setValues();">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <div >
    <table id="VitalSign" width="100%" class="AdultIE">
                <%--  <tr>
                      <td class="SectionheaderTxt" style="width: 100%">
                          <div>
                              Vital Signs


                          </div>
                      </td>
                    

                   
                      </tr>--%>
                  <tr>
                     <td >
                     <br />
                    

                         <table width="100%" cellspacing="0" cellpadding="0">
                             <tr>
                                 <td>
                                     Temperature (Celsius):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadTemperatureModal" runat="server"  >
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     RR (Bpm):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadRespirationRate" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td>
                                     Heart Rate (Bpm):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadHeartRate" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     Systollic Blood <br />Pressure mmHg:
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadSystollicBloodPressure" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td>
                                     Diastolic Blood <br /> Pressure mmHg:
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadDiastolicBloodPressure" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     Height (cms):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadHeight" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td>
                                     Weight (kgs):
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadWeight" runat="server"  Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                                 <td>
                                     BMI:
                                 </td>
                                 <td>
                                     <telerik:RadNumericTextBox ID="txtRadBMI" runat="server"  Enabled="false" Skin="MetroTouch">
                                     </telerik:RadNumericTextBox>
                                 </td>
                             </tr>
                              <tr>
                               <td colspan="4" >
                                <asp:Panel ID="pnlContolsPediatric" runat="server" Width="100%" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                Head Circumference :
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtRadHeadCircumference" runat="server" Skin="MetroTouch">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                Weight for age:
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbWeightForAge" runat="server" EmptyMessage="Select" AutoPostBack="false"
                                                    Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Weight for height :
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="rcbWeightforHeight" runat="server" EmptyMessage="Select"
                                                    AutoPostBack="false" Skin="MetroTouch" CheckedItemsTexts="FitInInput" EnableLoadOnDemand="true">
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                Nurses Comments :
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtRadNursesComments" runat="server" Skin="MetroTouch" TextMode="MultiLine"
                                                    Width="250px">
                                                </telerik:RadTextBox>
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
                   <td colspan="4" align="center">
                   <br />
                   <br />
                   <br />

                       <button title="Submit" id="close" onclick="returnToParent(); return false;">
                           Submit</button>
                   </td>
                   </tr>
                    
                   
                    

                </table>
    </div>
    </form>
</body>
</html>
