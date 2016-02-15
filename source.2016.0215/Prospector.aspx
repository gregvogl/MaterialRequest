<%@ Page Title="This item is available in Prospector." Language="C#" MasterPageFile="~/_support/MasterPage.Master" AutoEventWireup="true"
    CodeFile="Prospector.aspx.cs" Inherits="Prospector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link rel="Stylesheet" href="_support/css/request.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="Form1" runat="server">
    <p>It is usually faster to get an item from Prospector than to order it for CSU.</p>
    <h2><asp:HyperLink ID="ProspectorRequest" runat="server" CssClass="link" Target="_blank">Request this item from Prospector</asp:HyperLink></h2>
    <h3><asp:HyperLink ID="ProspectorItem" runat="server" Target="_blank">View availability in Prospector</asp:HyperLink></h3>
    <asp:Label ID="LabelProspectorCopies" AssociatedControlID="ProspectorCopies" Text="Non-CSU copies"
        runat="server" />
    <span class="data" id="ProspectorCopies" runat="server"></span>
    <asp:Label ID="LabelProspectorAvailable" AssociatedControlID="ProspectorAvailable" Text="Available copies"
        runat="server" />
    <span class="data" id="ProspectorAvailable" runat="server"></span>
    <h2><asp:HyperLink ID="OrderCSU" runat="server" CssClass="link">Order this item for CSU</asp:HyperLink></h2>
    <h2><asp:HyperLink ID="LogoutURL" runat="server" NavigateUrl="~/Logout.aspx">Logout</asp:HyperLink></h2>
    </form>
</asp:Content>
