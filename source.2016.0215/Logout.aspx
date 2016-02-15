<%@ Page Title="You have been logged out." Language="C#" MasterPageFile="~/_support/MasterPage.Master"
    AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="Logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <p>Your authenticated session has been ended. For further security, completely exit
        your browser.</p>
    <h2><asp:HyperLink ID="ReturnURL" runat="server">Go back to this item in the catalog</asp:HyperLink></h2>
</asp:Content>
