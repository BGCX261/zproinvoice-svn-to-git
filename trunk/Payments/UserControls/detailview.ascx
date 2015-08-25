<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Payments.DetailView" Codebehind="DetailView.ascx.cs" %>
<%@ Register TagPrefix="CRM" Tagname="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="DetailNavigation" Src="~/CRM/UserControls/DetailNavigation.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="Invoices" Src="Invoices.ascx" %>


<div id="divDetailView">
	<CRM:ModuleHeader ID="ctlModuleHeader" Module="Payments" EnablePrint="true" HelpName="DetailView" EnableHelp="true" Runat="Server" />

	<CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" Runat="Server" />

	<CRM:DetailNavigation ID="ctlDetailNavigation" Module="<%# m_sMODULE %>" Visible="<%# !PrintView %>" Runat="Server" />

	<table ID="tblMain" class="tabDetailView" runat="server">
	</table>

	<CRM:Invoices ID="ctlInvoices" Runat="Server" />

	<table ID="tblSummary" class="tabDetailView" runat="server">
	</table>

	<div id="divDetailSubPanel" name="Payments.DetailView">
		<asp:PlaceHolder ID="plcSubPanel" Runat="server" />
	</div>
</div>


