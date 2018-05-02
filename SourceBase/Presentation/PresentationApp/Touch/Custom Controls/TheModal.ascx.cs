using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Touch;

namespace Touch.Custom_Controls
{
    public partial class TheModal : TouchUserControlBase
    {
        private static string TextBoxValLocal;
        public static string TextBoxVal
        {
            get
            {
                return TextBoxValLocal;
            }
            set
            {
                TextBoxValLocal = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        public void SetTextBox()
        {
            txtSetBox.Text = TextBoxVal;
            updtTheModal.Update();
        }
    }
}