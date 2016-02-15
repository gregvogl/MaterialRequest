<%@ Page Title="Your message was sent." Language="C#" MasterPageFile="~/_support/MasterPage.Master"
    AutoEventWireup="true" CodeFile="Confirmation.aspx.cs" Inherits="Confirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <pre><asp:Literal ID="ConfirmationMessage" runat="server"></asp:Literal></pre>
    <h2><a href="javascript:print()">Print this message</a></h2>
    <h2><asp:HyperLink ID="ReturnURL" runat="server">Go back to this item in the catalog</asp:HyperLink></h2>
    <h2><asp:HyperLink ID="LogoutURL" runat="server" NavigateUrl="Logout.aspx">Logout</asp:HyperLink></h2>
</asp:Content>
