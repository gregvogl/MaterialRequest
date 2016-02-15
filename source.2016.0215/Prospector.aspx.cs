using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Prospector : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Prospector information details
        ProspectorCopies.InnerText = Session["ProspectorCopies"].ToString();
        ProspectorAvailable.InnerText = Session["ProspectorAvailable"].ToString();
        ProspectorItem.NavigateUrl = Session["ProspectorItem"].ToString();
        ProspectorRequest.NavigateUrl = Session["ProspectorRequest"].ToString();
        /*
        ProspectorCopies.Visible = false;
        ProspectorAvailable.Visible = false;
        LabelProspectorCopies.Visible = false;
        LabelProspectorAvailable.Visible = false;
         */
        OrderCSU.NavigateUrl = "Order.aspx";
    }
}