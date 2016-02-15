<%@ Page Title="Order Request" Language="C#" MasterPageFile="~/_support/MasterPage.Master"
    AutoEventWireup="true" CodeFile="Order.aspx.cs" Inherits="Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link rel="Stylesheet" href="_support/css/request.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <p>Order a print copy of this book for Colorado State University Libraries.</p>
    <p>There is no charge for this service.</p>
    <form runat="server" id="form1">
    <div id="OrderForm" runat="server">
        <h2>Your Contact Information</h2>
        <asp:Label ID="LabelName" AssociatedControlID="Name" Text="Name" runat="server" />
        <span class="data" id="Name" runat="server"></span>
        <asp:Label ID="LabelPhone" AssociatedControlID="Phone" Text="Phone" runat="server" />
        <asp:TextBox ID="Phone" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredPhone" runat="server" ErrorMessage="Phone number is required."
            ControlToValidate="Phone" Display="Dynamic" CssClass="validator"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="ValidPhone" ControlToValidate="Phone" CssClass="validator"
            runat="server" Display="Dynamic" ErrorMessage="Enter a 10-digit phone number."
            ValidationExpression="[\( ]*\d{3}[\)\-\. ]*\d{3}[\-\. ]*\d{4}"></asp:RegularExpressionValidator>
        <asp:Label ID="LabelEmail" AssociatedControlID="Email" Text="Email" runat="server" />
        <asp:TextBox ID="Email" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredEmail" runat="server" ErrorMessage="Email address is required."
            ControlToValidate="Email" Display="Dynamic" CssClass="validator"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="ValidEmail" ControlToValidate="Email" CssClass="validator"
            runat="server" Display="Dynamic" ErrorMessage="Enter a valid email address."
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
        <h2>Requested Item Information</h2>
        <asp:Label ID="LabelURL" AssociatedControlID="URL" Text="URL" runat="server" />
        <asp:HyperLink ID="URL" runat="server" CssClass="link" Target="_blank" />
        <asp:Label ID="LabelTitle" AssociatedControlID="ItemTitle" Text="Title" runat="server" />
        <span class="data" id="ItemTitle" runat="server"></span>
        <asp:Label ID="LabelAuthor" AssociatedControlID="Author" Text="Author" runat="server" />
        <span class="data" id="Author" runat="server"></span>
        <asp:Label ID="LabelPublisher" AssociatedControlID="Publisher" Text="Publisher" runat="server" />
        <span class="data" id="Publisher" runat="server"></span>
        <%--        <asp:Label ID="LabelCallNumber" AssociatedControlID="CallNumber" Text="Call Number"
            runat="server" />
        <span class="data" id="CallNumber" runat="server"></span>
        --%>
        <asp:Label ID="LabelISBN" AssociatedControlID="ISBN" Text="ISBN" runat="server" />
        <span class="data" id="ISBN" runat="server"></span>
        <h2>Delivery Options</h2>
        <asp:RadioButtonList ID="Delivery" runat="server">
            <asp:ListItem Value="Order" Selected="True">Order (about 2-3 weeks)</asp:ListItem>
            <asp:ListItem Value="Order+Notify">Order and notify me when it arrives (about 2-3 weeks)</asp:ListItem>
            <%--<asp:ListItem Value="Rush">Rush Order and notify me (4-6 business days, extra cost of $15.00 for CSU Libraries)</asp:ListItem>--%>
            <%--<asp:ListItem Value="Order" Selected="True">Order (about 6-8 weeks due to the end of the CSU budget year)</asp:ListItem>
            <asp:ListItem Value="Order+Notify">Order and notify me when it arrives (about 6-8 weeks due to the end of the CSU budget year)</asp:ListItem>--%>
        </asp:RadioButtonList>
        <%-- 
        <asp:Label ID="LabelEmployeeDepartment" AssociatedControlID="EmployeeDepartment"
            Text="Employee Department" runat="server" />
        <span class="data" id="EmployeeDepartment" runat="server"></span>
        --%>
        <div id="CommentsArea" runat="server">
            <h2>Comments by Libraries/ACNS Staff</h2>
            <asp:Label ID="LabelComments" AssociatedControlID="Comments" Text="(e.g. requested for a faculty member, volume information)" runat="server" />
            <asp:TextBox ID="Comments" runat="server" Rows="5" Columns="50" TextMode="MultiLine" />
            <%-- 
            <asp:Label ID="LabelSpacer" Text="*" AssociatedControlID="Comments" runat="server"
                Style="visibility: hidden;" />
            <span class="data"></span>
            --%>
        </div>
        <div style="clear: left">
            <asp:Label ID="LabelSend" Text="*" AssociatedControlID="Send" runat="server" Style="visibility: hidden;" />
            <asp:Button ID="Send" runat="server" Text="Send Order Request" OnClick="Send_Click" />
        </div>
    </div>
    <h2>
        <asp:HyperLink ID="LogoutURL" runat="server" NavigateUrl="~/Logout.aspx">Logout</asp:HyperLink></h2>
    </form>
</asp:Content>
