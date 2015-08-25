<%@ Page Language="c#" MasterPageFile="~/DefaultView.Master" Inherits="CRM.InvoiceManagement.Manufacturers.Edit" Codebehind="edit.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="Shortcuts" Src="~/CRM/UserControls/Shortcuts.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="EditView" Src="~/CRM/Invoice Management/Manufacturers/UserControls/EditView.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ListView" Src="~/CRM/Invoice Management/Manufacturers/UserControls/ListView.ascx" %>
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
    <CRM:Shortcuts ID="ctlShortcuts" SubMenu="Invoices" runat="Server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:ModuleHeader ID="ctlModuleHeader" Module="Manufacturers" Title="Manufacturers.LBL_MODULE_NAME"
        EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" runat="Server" />
    <CRM:ListView ID="ctlListView" Visible='<%# CRM.CRMSecurity.IS_ADMIN %>' runat="Server" />
    <br>
    <CRM:EditView ID="ctlEditView" Visible='<%# CRM.CRMSecurity.IS_ADMIN %>' Runat="Server" />
    <asp:Label Text='<%# Translation.GetTranslation.Term(".LBL_UNAUTH_ADMIN") %>' Visible='<%# !CRM.CRMSecurity.IS_ADMIN %>'
        runat="Server" />
</asp:Content>
