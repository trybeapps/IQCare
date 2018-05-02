using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Threading;
using System.Globalization;
using System.Text;

public abstract class BasePage : System.Web.UI.Page
{
    private ObjectStateFormatter _formatter = 
        new ObjectStateFormatter();
   

    protected override void
        SavePageStateToPersistenceMedium(object viewState)
    {
        MemoryStream ms = new MemoryStream();
        _formatter.Serialize(ms, viewState);
        byte[] viewStateArray = ms.ToArray();
        ClientScript.RegisterHiddenField("__COMPRESSEDVIEWSTATE",
            Convert.ToBase64String(CompressViewState.Compress(viewStateArray)));
    }
    protected override object
        LoadPageStateFromPersistenceMedium()
    {
        string vsString = Request.Form["__COMPRESSEDVIEWSTATE"];
        byte[] bytes = Convert.FromBase64String(vsString);
        bytes = CompressViewState.Decompress(bytes);
        return _formatter.Deserialize(Convert.ToBase64String(bytes));
    }
    //protected override void OnUnload(EventArgs e)
    //{
    //    Session["PatientVisitId"] = 0;
    //    Session["ServiceLocationId"] = 0;
    //}
    
}
