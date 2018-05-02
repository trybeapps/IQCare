using System;
using System.Web.UI; 
using System.Web.UI.WebControls;
using System.ComponentModel; 
using System.Text; 
using Application.Common; 

    namespace Application.Presentation
    {
    public abstract class IQCareMsgBox
    {

        #region "Alert Msgbox"

        public static void Show(string MessageId, Control frmName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            Show(theMsg.ToString(), theMsg.Type.ToString(), theMsg.Buttons.ToString(), frmName);
        }

        public static void Show(string MessageId, MsgBuilder MessageBuilder, Control frmName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            MessageBuilder.MsgRepository[MessageId] = theMsg.ToString();
            string theDynamicMsg = MessageBuilder.BuildMessage(MessageId);
            Show(theDynamicMsg, theMsg.Type.ToString(), theMsg.Buttons.ToString(), frmName);
        }

        public static void Show(string Msg, string Style, string Buttons, Control frmName)
        {
            string theAlert = "";
            switch (Style)
            {
                case "!":
                    theAlert = "alert";
                    break;

            }

            ///// At this movement we are not using Buttons Parameter as 
            ///// in ASP.Net the msgbox Button Styling is predefined.

            ///// Converting into ASP
            StringBuilder theSB;
            Msg = Msg.Replace("'", "");
            Msg = Msg.Replace("\n","\\n");
            Msg = Msg.Replace("\r", "");
            string tmpMsg = Msg;

            Msg = "<script language = javascript> " + theAlert + "('" + Msg + "')</script>";            

            theSB = new StringBuilder();
            theSB.Append(Msg);            
            frmName.Controls.AddAt(frmName.Controls.Count, new LiteralControl(theSB.ToString()));
        }
        public static void ShowforUpdatePanel(string MessageId, Control frmName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            ShowforUpdatePanel(theMsg.ToString(), theMsg.Type.ToString(), theMsg.Buttons.ToString(), frmName);
        }
        public static void ShowforUpdatePanel(string MessageId, MsgBuilder MessageBuilder, Control frmName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            MessageBuilder.MsgRepository[MessageId] = theMsg.ToString();
            string theDynamicMsg = MessageBuilder.BuildMessage(MessageId);
            ShowforUpdatePanel(theDynamicMsg, theMsg.Type.ToString(), theMsg.Buttons.ToString(), frmName);
        }
        public static void ShowforUpdatePanel(string Msg, string Style, string Buttons, Control frmName)
        {
            string theAlert = "";
            switch (Style)
            {
                case "!":
                    theAlert = "alert";
                    break;

            }

            StringBuilder theSB;
            Msg = Msg.Replace("'", "");
            Msg = Msg.Replace("\n", "\\n");
            Msg = Msg.Replace("\r", "");
            string tmpMsg = Msg;

            
            Msg = theAlert + "('" + Msg + "')";

            theSB = new StringBuilder();
            theSB.Append(Msg);
            ScriptManager.RegisterStartupScript(frmName.Page, frmName.Page.GetType(), Msg, theSB.ToString(), true);
           
        }

        #endregion

        #region "Confirm Msgbox"

        public static void ShowConfirm(string MessageId, WebControl cntrlName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            ShowConfirm(theMsg.ToString(), theMsg.Type.ToString(), theMsg.Buttons.ToString(), cntrlName);
        }

        public static void ShowConfirm(string MessageId, MsgBuilder MessageBuilder, WebControl cntrlName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            MessageBuilder.MsgRepository[MessageId] = theMsg.ToString();
            string theDynamicMsg = MessageBuilder.BuildMessage(MessageId);
            ShowConfirm(theDynamicMsg, theMsg.Type.ToString(), theMsg.Buttons.ToString(), cntrlName);
        }

        public static void ShowConfirm(string Msg, string Style, string Buttons, WebControl cntrlName)
        {
            string theAlert = "";
            switch (Style)
            {
                case "?":
                    theAlert = "return confirm";
                    break;
            }

            ///// At this movement we are not using Buttons Parameter as 
            ///// in ASP.Net the msgbox Button Styling is predefined.

            ///// Converting into ASP
            StringBuilder theSB;
            Msg = Msg.Replace("'", "\'");
            //Msg = Msg.Replace(Char(34),"'\'" + Char(34)); 
            string tmpMsg = Msg;

            Msg = theAlert + "('" + Msg + "')";

            theSB = new StringBuilder();
            theSB.Append(Msg);
            cntrlName.Attributes.Add("onclick", theSB.ToString());

        }

        #endregion

        #region "GetMessageText"

        public static string GetMessage(string MessageId, Control frmName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            return theMsg.ToString();
        }

        public static string GetMessage(string MessageId, MsgBuilder MessageBuilder, Control frmName)
        {
            RawMessage theMsg = MsgRepository.GetMessage(MessageId);
            MessageBuilder.MsgRepository[MessageId] = theMsg.ToString();
            string theDynamicMsg = MessageBuilder.BuildMessage(MessageId);
            return theDynamicMsg.ToString();
        }
        #endregion

    }
  }
