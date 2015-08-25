<%@ Page Language="c#" MasterPageFile="~/DefaultView.Master" Inherits="CRM.InvoiceManagement.Products.View"
    CodeBehind="view.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="Shortcuts" Src="~/CRM/UserControls/Shortcuts.ascx" %>
<%@ Register TagPrefix="CRM" TagName="DetailView" Src="~/CRM/Invoice Management/Products/UserControls/DetailView.ascx" %>
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
    <CRM:Shortcuts ID="ctlShortcuts" SubMenu="Invoices" runat="Server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:DetailView ID="ctlDetailView" Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "view") >= 0 %>'
        runat="Server" />
    <asp:Label ID="lblAccessError" ForeColor="Red" EnableViewState="false" Text='<%# Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS") %>'
        Visible="<%# !ctlDetailView.Visible %>" runat="server" />
</asp:Content>
