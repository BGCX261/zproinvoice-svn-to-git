<%@ Page Language="c#" MasterPageFile="~/DefaultView.Master" Inherits="CRM.InvoiceManagement.Manufacturers.Index"
    CodeBehind="index.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="Shortcuts" Src="~/CRM/UserControls/Shortcuts.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ListView" Src="~/CRM/Invoice Management/Manufacturers/UserControls/ListView.ascx" %>
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
    <CRM:Shortcuts ID="ctlShortcuts" SubMenu="Invoices" runat="Server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:ModuleHeader ID="ctlModuleHeader" Module="Manufacturers" Title="Manufacturers.LBL_MODULE_NAME"
        EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" runat="Server" />
    <CRM:ListView ID="ctlListView" Visible="true" runat="Server" />
    <asp:Label Text='<%# Translation.GetTranslation.Term(".LBL_UNAUTH_ADMIN") %>' Visible='<%# !CRM.CRMSecurity.IS_ADMIN %>'
        runat="Server" />
</asp:Content>
