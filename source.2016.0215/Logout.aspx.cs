using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ReferringURL"] == null)
            ReturnURL.Visible = false;
        else
            ReturnURL.NavigateUrl = Session["ReferringURL"].ToString();

        // Clear all session variables
        if (Session["Ename"] != null)
            Session.Remove("Ename");
        if (Session["CSUID"] != null)
            Session.Remove("CSUID");

        //Clear the current session.
        Session.Clear();
        Session.Abandon();
    }
}