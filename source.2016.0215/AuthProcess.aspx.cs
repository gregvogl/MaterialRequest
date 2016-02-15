using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

using Edu.Colostate.Acns.WebAuth;

public partial class AuthProcess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetAuthProcessResult();
    }

    private void GetAuthProcessResult()
    {
        // if no user session
        if (Session["Ename"] == null)
        {
            // Instantiate the the process result return value object
            WebAuthReturnValues war = new WebAuthReturnValues();

            // Instantiate the the WebAuthProcess class
            WebAuthProcess wap = new WebAuthProcess();

            //Load the AuthProcessReult into the local instance of WebAuthReturnValues
            war = wap.AuthProcessResult("Default.aspx");

            //Is the authentication attempt successful?
            if (war.successfulAuthentication != true)
            {
                divAuthProResult.InnerHtml = war.html;
                return;
            }

            // get Ename and CSUID from WebAuth
            Session["Ename"] = war.eName;
            Session["CSUID"] = war.CSUID;
        }

        // get person details from WEID table using Ename 
        if (!getPersonDetails())
        {
            divAuthProResult.InnerHtml = "Sorry, you are not authorized to request this item. You must be a current CSU student, faculty or staff member.";
            return;
        }

        // TODO: move these checks to Default.aspx.cs since they shouldn't require login?

        // assure that there is a referring URL from a catalog item
        if (Session["ReferringURL"] == "")
        {
            divAuthProResult.InnerHtml = "Cannot find the requested item: No referring URL.";
            return;
        }

        // assure that there is a bib number
        string bibNumber = Session["BibNumber"].ToString();
        if (bibNumber == "")
        {
            divAuthProResult.InnerHtml = "Cannot find the requested item: No bibliographic number was found in the referring or link URL.";
            return;
        }
        
        // get item information from solr record
        if (!getItemDetails(bibNumber))
        {
            divAuthProResult.InnerHtml = "Cannot find the requested item with bib number " + bibNumber + " in Discovery.";
            return;
        }

        // get Prospector availability
        // TODO: report Prospector is unavailable for checking, if necessary
        //getRegionalAvailability(bibNumber);
        // get first 13-digit ISBN, if any (otherwise use 10-digit ISBN which is the older format)
        string isbn = "";
        string[] isbns = getSessionParam("ISBN").Split(new string[] { "<br />" }, StringSplitOptions.None);
        foreach (string i in isbns)
            if (isbn.Length < 13 && i.Length >= 10) isbn = Regex.Replace(i, @"[^0-9]", "");
        
        // Send to Prospector information or directly to request form
        Session["ProspectorCopies"] = 0;
        Session["ProspectorAvailable"] = 0;
        Session["ProspectorURL"] = "";
        Session["ProspectorRequest"] = "";
        if (isbn != "" && getRegionalAvailability(isbn) && !Session["ProspectorAvailable"].ToString().Equals("0"))
            Response.Redirect("~/Prospector.aspx");
        else {
            Response.Redirect("~/Order.aspx");
        }
    }

    private string getSessionParam(string name)
    {
        return Session[name] == null ? "" : Session[name].ToString();
    }

    // get person information from WEID table 
    private bool getPersonDetails()
    {
        var wdc = new WEIDDataContext();
        var weid = wdc.WEID_DIR_PERSON_00s.Where(p => p.ENAME == Session["Ename"]).First();

        // only allow current students and all types of employees (not applicants or community members)
        Session["StudentType"] = weid.STUDENT_TYPE ?? String.Empty;
        Session["EmployeeType"] = weid.EMPLOYEE_TYPE ?? String.Empty;
        if (!Session["StudentType"].Equals("Student") && Session["EmployeeType"].Equals(String.Empty))
            return false;

        // get person contact information
        Session["Name"] = weid.LAST_NAME + ", " + weid.FIRST_NAME + " " + weid.MIDDLE_NAME;
        Session["Phone"] = validPhone(weid.EMPLOYEE_PHONE) ?? validPhone(weid.EMPLOYEE_PHONE_2) ?? validPhone(weid.PHONE) ?? String.Empty;
        Session["Email"] = weid.PROPER_NAME_ADDRESS ?? String.Empty;
        Session["EmployeeDepartmentNumber"] = weid.EMPLOYEE_DEPARTMENT_NUMBER ?? String.Empty;
        return true;
    }

    private string validPhone(string phone)
    {
        return phone == null || phone.Equals("0000000000") ? null : phone;
    }

    // get item information from solr record
    private bool getItemDetails(string id)
    {
        Session["Title"] = "";
        Session["CallNumber"] = "";
        Session["Author"] = "";
        Session["Publisher"] = "";
        Session["ISBN"] = "";
        string solrUrl = "http://discovery.library.colostate.edu:8080/solr/biblio/select/?q=id%3A.b" + id + "*";
        var xml = XDocument.Load(solrUrl);
        if (xml == null)
            return false;
        var doc = xml.Elements("response").Elements("result").Elements("doc").FirstOrDefault();
        if (doc == null)
            return false;
        Session["Title"] = joinXmlElements(doc, "str", "title");
        Session["CallNumber"] = joinXmlElements(doc, "str", "callnumberStr");
        Session["Author"] = joinXmlElements(doc, "arr", "author");
        Session["Publisher"] = joinXmlElements(doc, "arr", "publisher");
        Session["ISBN"] = joinXmlElements(doc, "arr", "isbn");
        return true;
    }

    private string joinXmlElements(XElement doc, string strArr, string fieldName)
    {
        return string.Join("<br />", strArr.Equals("str")
            ? doc.Elements("str").Where(f => f.Attribute("name").Value.Equals(fieldName)).Select(s => s.Value)
            : doc.Elements("arr").Where(f => f.Attribute("name").Value.Equals(fieldName)).Elements("str").Select(s => s.Value));
    }

    // get Prospector availability from first isbn
    //private bool getRegionalAvailability(string id)
    private bool getRegionalAvailability(string isbn)
    {
        const string regionalHome = "http://prospector.coalliance.org/";
        const string regionalSearch = regionalHome + "search~S0?/";
        string backlink = "&backlink=" + Session["ReferringURL"].ToString();
        string i = "i" + isbn;
        Session["ProspectorItem"] = regionalSearch + i + "/" + i + "/1,1,1,B/detlframeset&FF=" + i + "&1,1," + backlink;
        Session["ProspectorRequest"] = regionalSearch + i + "/" + i + "/1,1,1,B/request&FF=" + i + "&1,1," + backlink;
        Session["ProspectorItems"] = regionalHome + "search/?searchtype=i&searcharg=" + isbn + backlink;
        /*
        var b = "z9csup+b" + id;
        Session["ProspectorURL"] = regionalSearch + b + "/" + b + "/1,1,1,B/detlframeset&FF=" + b + "&1,1," + backlink;
        Session["ProspectorRequest"] = regionalSearch + b + "/" + b + "/1,1,1,B/request&FF=" + b + "&1,1," + backlink;
         */

        // make sure the ISBN is actually found
        // otherwise regionalHtml returns a false positive with a different ISBN! - gvogl 2013-06-26
        string regionalItems;
        try
        {
            using (var wc = new System.Net.WebClient())
                regionalItems = wc.DownloadString(Session["ProspectorItems"].ToString());
        }
        catch
        {
            return false;
        }
        if (regionalItems.Contains("No matches found"))
            return false;

        string regionalHtml;
        try
        {
            using(var wc = new System.Net.WebClient())
                regionalHtml = wc.DownloadString(Session["ProspectorItem"].ToString());
        }
        catch
        {
            return false;
        }

        // ignore rows with links containing notes about additional volumes, table of contents, etc.
        regionalHtml = Regex.Replace(regionalHtml, @"<tr><td>[^<]+</td><td[^>]+><a href=""[^""]+"">CLICK HERE</a><font color=""red""> to view additional volumes at</font>[^<]+</td></tr>", "");
        regionalHtml = Regex.Replace(regionalHtml, @"<tr><td><a name=""[a-z0-9]+""></a>[^<]+</td>\s*<td>&nbsp;</td>\s*<td[^>]+><a[^>]+>[^>]+</a>\s*<td></td>\s*<td></td>\s*</tr>", "");

        // count non-CSU copies and available copies
        int copies = 0;
        int available = 0;

        Match match = Regex.Match(regionalHtml, @"(<tr class=""holdings[a-z0-9]+""><td><a name=""([a-z0-9]+)""><\/a>[^>]+<\/td>(\s*<td>[^>]+<\/td>\s*)+<\/tr>\s*)+", RegexOptions.None);
        if (match.Success)
            for (var j = 0; j < match.Groups[2].Captures.Count; j++)
                if (!match.Groups[2].Captures[j].Value.Contains("9csup"))
                {
                    copies++;
                    if (match.Groups[3].Captures[4 * (j + 1) - 1].Value.Contains("AVAILABLE"))
                        available++;
                }

        Session["ProspectorCopies"] = copies;
        Session["ProspectorAvailable"] = available;
        return true;
    }
}