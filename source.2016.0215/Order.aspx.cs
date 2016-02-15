using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Net.Mail;

using System.Text.RegularExpressions;

using Edu.Colostate.Acns.WebAuth;

public partial class Order : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // if no user session, go to login page (referring URL is lost, so user will close window)
        if (Session["Ename"] == null)
        {
            Response.Redirect("~/Logout.aspx");
        }

        // display person details
        Name.InnerHtml = Session["Name"].ToString();
        if (!IsPostBack)
        {
            Phone.Text = Session["Phone"].ToString();
            Email.Text = Session["Email"].ToString();
        }

        // display item details
        URL.NavigateUrl = Session["ReferringURL"].ToString();
        URL.Text = Session["ReferringURL"].ToString();
        ItemTitle.InnerHtml = Session["Title"].ToString();
        Author.InnerHtml = Session["Author"].ToString();
        Publisher.InnerHtml = Session["Publisher"].ToString();
        //CallNumber.InnerHtml = Session["CallNumber"].ToString();
        ISBN.InnerHtml = Session["ISBN"].ToString();

        //EmployeeDepartment.InnerHtml = Session["EmployeeDepartmentNumber"].ToString();
        if (Session["EmployeeDepartmentNumber"].ToString() != "1018" && Session["EmployeeDepartmentNumber"].ToString() != "1019")
        {
            CommentsArea.Visible = false;
        }
    }

    protected void Send_Click(object sender, EventArgs e)
    {
        saveInput();
        logSubmission();
        makeConfirmationMessage();
        sendEmail();
        Response.Redirect("~/Confirmation.aspx");
    }

    protected void saveInput()
    {
        Session["Phone"] = Regex.Replace(Phone.Text, @"[\(\)\-\. ]+", "");
        Session["Email"] = Email.Text;
        Session["Delivery"] = Delivery.SelectedValue;
        Session["DeliveryDisplay"] = Delivery.SelectedItem.Text;
        Session["Comments"] = Comments.Text;
    }

    private string sessionVar(string str, int maxLength)
    {
        str = Session[str].ToString();
        return str == null ? null : str.Substring(0, Math.Min(maxLength, str.Length));
    }

    protected void logSubmission()
    {
        var logData = new MaterialRequestDataContext();

        var request = new Request()
        {
            SubmitDate = DateTime.Now,
            Delivery = sessionVar("Delivery", 50),
            Name = sessionVar("Name", 100),
            Phone = sessionVar("Phone", 10),
            Email = sessionVar("Email", 100),
            Ename = sessionVar("Ename", 15),
            CSUID = sessionVar("CSUID", 9),
            EmployeeType = sessionVar("EmployeeType", 50),
            StudentType = sessionVar("StudentType", 50),
            URL = sessionVar("ReferringURL", 1023),
            Title = sessionVar("Title", 511),
            Author = sessionVar("Author", 255),
            Publisher = sessionVar("Publisher", 255),
            CallNumber = sessionVar("CallNumber", 255),
            ISBN = sessionVar("ISBN", 511),
            ProspectorURL = sessionVar("ProspectorURL", 1023),
            ProspectorCopies = Int32.Parse(sessionVar("ProspectorCopies", 5)),
            ProspectorAvailable = Int32.Parse(sessionVar("ProspectorAvailable", 5)),
            BibNumber = sessionVar("BibNumber", 7),
            Comments = sessionVar("Comments", 1023)
        };
        logData.Requests.InsertOnSubmit(request);
        logData.SubmitChanges();
    }

    protected void makeConfirmationMessage()
    {
        string msgBody = "The following request was submitted at " + DateTime.Now.ToString() + Environment.NewLine;

        msgBody += Environment.NewLine + "Contact Information: " + Environment.NewLine + Environment.NewLine;

        string[] personDetails = { "Name", "Phone", "Email", "Ename", "EmployeeType", "StudentType" };
        foreach (var v in personDetails)
            msgBody += v + ": " + Session[v].ToString() + Environment.NewLine;           

        msgBody += Environment.NewLine + "Requested Item Information: " + Environment.NewLine + Environment.NewLine;

        string[] itemDetails = { "ReferringURL", "BibNumber", "Title", "Author", "Publisher", "ISBN", 
                                   "ProspectorURL", "ProspectorCopies", "ProspectorAvailable" };
        foreach (var v in itemDetails)
            msgBody += v + ": " + Session[v].ToString() + Environment.NewLine;           

        msgBody += Environment.NewLine + "Delivery Option: " + Session["DeliveryDisplay"] + Environment.NewLine;

        if (Session["Comments"].ToString() != "")
            msgBody += Environment.NewLine + "Comments: " + Session["Comments"] + Environment.NewLine;

        Session["ConfirmationMessage"] = msgBody;
    }

    private void sendEmail()
    {
        using (var message = new MailMessage())
        {
            message.From = new MailAddress("noreply@buzzard.library.colostate.edu", "Collections and Contracts Staff");
            message.To.Add(new MailAddress(Email.Text, Name.InnerText));
            message.Bcc.Add(new MailAddress("Library_ElecMono@mail.colostate.edu", "Collections and Contracts Staff"));
            message.Bcc.Add(new MailAddress("gvogl@mail.colostate.edu", "Greg Vogl"));
            message.Subject = "CSU Libraries material request";
            message.Body = Session["ConfirmationMessage"].ToString();

            //Send the message
            string server = "smtp.colostate.edu";
            SmtpClient client = new SmtpClient(server);
            client.Send(message);
        }
    }
}