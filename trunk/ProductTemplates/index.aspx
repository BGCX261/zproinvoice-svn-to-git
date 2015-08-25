<%@ Page Language="c#" MasterPageFile="~/DefaultView.Master" Inherits="CRM.InvoiceManagement.ProductTemplates.Index"
    CodeBehind="index.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="Shortcuts" Src="~/CRM/UserControls/Shortcuts.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ListView" Src="~/CRM/Invoice Management/ProductTemplates/UserControls/ListView.ascx" %>
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
    <CRM:Shortcuts ID="ctlShortcuts" SubMenu="Invoices" runat="Server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:ListView ID="ctlListView" Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "list") >= 0 %>'
        runat="Server" />
    <asp:Label ID="lblAccessError" ForeColor="Red" EnableViewState="false" Text='<%# Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS") %>'
        Visible="<%# !ctlListView.Visible %>" runat="server" />
</asp:Content>
