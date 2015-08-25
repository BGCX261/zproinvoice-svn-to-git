<%@ Page Language="c#" MasterPageFile="~/DefaultView.Master" Inherits="CRM.InvoiceManagement.ProductTemplates.Edit"
    CodeBehind="edit.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="Shortcuts" Src="~/CRM/UserControls/Shortcuts.ascx" %>
<%@ Register TagPrefix="CRM" TagName="EditView" Src="~/CRM/Invoice Management/ProductTemplates/UserControls/EditView.ascx" %>
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
    <CRM:Shortcuts ID="ctlShortcuts" SubMenu="Invoices" runat="Server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:EditView ID="ctlEditView" Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit") >= 0 %>'
        runat="Server" />
    <asp:Label ID="lblAccessError" ForeColor="Red" EnableViewState="false" Text='<%# Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS") %>'
        Visible="<%# !ctlEditView.Visible %>" runat="server" />
</asp:Content>
