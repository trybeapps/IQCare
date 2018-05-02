using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Touch;

namespace Touch.Custom_Forms
{
    public partial class frmHistoryTouch : TouchUserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ART[] myArray = new ART[6] { 
            new ART("Atripla","26 Jan 2012"),
            new ART("Reyataz","12 Mar 2012"),
            new ART("Norvir","07 May 2012"),
            new ART("Truvada","23 Jun 2012"),
            new ART("Prezista","19 Sep 2012"),
            new ART("Isentress","15 Dec 2012") };
            rgPriorART.DataSource = myArray;
            rgPriorART.DataBind();
            rgPriorART.Visible = true;
        }
    }

    public class ART
    {
        public ART(string Regimen, string Date)
        {
            _regimen = Regimen;
            _date = Date;

        }
        private string _regimen;
        public string Regimen
        {
            get { return _regimen; }
            set { _regimen = value; }
        }
        private string _date;
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}