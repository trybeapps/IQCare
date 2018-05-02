using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Touch
{
    public partial class frmFindAddTouch : TouchUserControlBase
    {
        protected void btnFind_Click(object sender, EventArgs e)
        {
            results.Visible = true;
        }
    }
}