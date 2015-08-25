<%@ Page language="c#" MasterPageFile="~/DefaultView.Master" Inherits="CRM.InvoiceManagement.ProductTemplates.Import" Codebehind="import.aspx.cs" %>
<script runat="server">
</script>
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
	<%@ Register TagPrefix="CRM" Tagname="Shortcuts" Src="~/CRM/UserControls/Shortcuts.ascx" %>
	<CRM:Shortcuts ID="ctlShortcuts" SubMenu="ProductTemplates" Runat="Server" />
</asp:Content>

<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
	<%@ Register TagPrefix="CRM" Tagname="ImportView" Src="~/CRM/Import/ImportView.ascx" %>
	<CRM:ImportView ID="ctlImportView" Module="ProductTemplates" Visible='<%# CRM.CRMSecurity.IS_ADMIN %>' Runat="Server" />
	<asp:Label ID="lblAccessError" ForeColor="Red" EnableViewState="false" Text='<%# Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS") %>' Visible="<%# !ctlImportView.Visible %>" Runat="server" />
</asp:Content>
