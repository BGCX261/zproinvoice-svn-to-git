<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTemplates.DetailView" Codebehind="DetailView.ascx.cs" %>
<div id="divDetailView">
	<%@ Register TagPrefix="CRM" Tagname="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
	<CRM:ModuleHeader ID="ctlModuleHeader" Module="ProductTemplates" EnablePrint="true" HelpName="DetailView" EnableHelp="true" Runat="Server" />

	<%@ Register TagPrefix="CRM" Tagname="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
	<CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" Runat="Server" />

	<%@ Register TagPrefix="CRM" Tagname="DetailNavigation" Src="~/CRM/UserControls/DetailNavigation.ascx" %>
	<CRM:DetailNavigation ID="ctlDetailNavigation" Module="<%# m_sMODULE %>" Visible="<%# !PrintView %>" Runat="Server" />

	<table ID="tblMain" class="tabDetailView" runat="server">
	</table>
	<div id="divDetailSubPanel" name="ProductTemplates.DetailView">
		<asp:PlaceHolder ID="plcSubPanel" Runat="server" />
	</div>
</div>



