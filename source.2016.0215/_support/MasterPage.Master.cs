using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MainHeading.InnerText = Page.Title;
        Page.Title += " | Material Purchase Request Form | Libraries | Colorado State University";
    }
}
