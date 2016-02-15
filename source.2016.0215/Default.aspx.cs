using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;

using Edu.Colostate.Acns.WebAuth;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // forward to librequest to wsnet since WebAuth requires an SSL certificate
        const string requestUrl = "http://librequest.colostate.edu/";
        const string productionUrl = "https://wsnet.colostate.edu/cwis6/MaterialRequest/";
        if (Request.Url.ToString().Contains(requestUrl))
            Response.Redirect(Request.Url.ToString().Replace(requestUrl, productionUrl));

        // TODO: simplify this. 
        // It is complicated because the referer and request parameter must be preserved if authentication fails.

        string sessionRef = getSessionParam("ReferringURL");
        string requestRef = "";
        if (Request.UrlReferrer != null) requestRef = Request.UrlReferrer.ToString();
        // set referring URL if not set or not equal to current referring URL
        if (sessionRef == "" || (requestRef != "" && requestRef != sessionRef && !requestRef.Contains("AuthProcess")))
        {
            // get referring URL 
            sessionRef = requestRef;
            Session["ReferringURL"] = sessionRef;
        }

        // get bib number from referring URL
        var bibRef = "";
        // Sage: record=b1234567 Discovery: Record/.b1234567X
        Match m = Regex.Match(sessionRef, @"record(=|\/\.)b([0-9]{7})", RegexOptions.IgnoreCase);
        if (m.Success)
            bibRef = m.Groups[2].Value;
        var sessionBibRef = getSessionParam("BibRef");
        if (sessionBibRef == "" || (bibRef != null && bibRef != sessionBibRef))
        {
            sessionBibRef = bibRef;
            Session["BibRef"] = sessionBibRef;
        }

        // get bib number from record query parameter
        var bibParam = "";
        if (Request.Params["record"] != null)
        {
            var recordParam = Request.Params["record"].ToString();
            Match m2 = Regex.Match(recordParam, @"b([0-9]{7})", RegexOptions.IgnoreCase);
            if (m2.Success)
                bibParam = m2.Groups[1].Value;
        }
        var sessionBibParam = getSessionParam("BibParam");
        if (sessionBibParam == "" || (bibParam != "" && bibParam != sessionBibParam))
        {
            sessionBibParam = bibParam;
            Session["BibParam"] = sessionBibParam;
        }

        Session["BibNumber"] = sessionBibRef != "" ? sessionBibRef
            : sessionBibParam != "" ? sessionBibParam : "";

        if (Session["Ename"] != null) 
            // User is already logged in; allow multiple requests
            Response.Redirect("~/AuthProcess.aspx");
        else
            // WebAuth call
            LoadWebAuthForm();
    }

    #region Methods

    private string getSessionParam(string name)
    {
        return Session[name] == null ? "" : Session[name].ToString();
    }

    private void LoadWebAuthForm()
    {
        //Be sure the eService token is in the web.config under appSettings with a key named WebAuthToken
        //<appSettings>
        //    <add key="WebAuthToken" value="xxxxxxxxxxxxxxxxxxxx"/>
        //</appSettings>

        // Instantiate the the WebAuthProcess class
        WebAuthProcess wap = new WebAuthProcess();
        //Set the return into the login_form div
        divLoginForm.InnerHtml = wap.ReturnLoginForm(this.Session.SessionID, "");
    }

    #endregion

}
