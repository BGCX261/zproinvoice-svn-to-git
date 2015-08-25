<%@ Page Language="c#" MasterPageFile="~/DefaultView.Master" Inherits="CRM.InvoiceManagement.ProductCategories.Edit" Codebehind="edit.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="Shortcuts" Src="~/CRM/UserControls/Shortcuts.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="EditView" Src="~/CRM/Invoice Management/ProductCategories/UserControls/EditView.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ListView" Src="~/CRM/Invoice Management/ProductCategories/UserControls/ListView.ascx" %>
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
    <CRM:Shortcuts ID="ctlShortcuts" SubMenu="ProductCategories" runat="Server" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:ModuleHeader ID="ctlModuleHeader" Module="ProductCategories" Title="ProductCategories.LBL_MODULE_NAME"
        EnableModuleLabel="false" EnablePrint="false" EnableHelp="true" runat="Server" />
    <CRM:ListView ID="ctlListView" Visible='<%# CRM.CRMSecurity.IS_ADMIN %>' runat="Server" />
    <br>
    <CRM:EditView ID="ctlEditView" Visible='<%# CRM.CRMSecurity.IS_ADMIN %>' runat="Server" />
    <asp:Label Text='<%# Translation.GetTranslation.Term(".LBL_UNAUTH_ADMIN") %>' Visible='<%# !CRM.CRMSecurity.IS_ADMIN %>'
        runat="Server" />
</asp:Content>
