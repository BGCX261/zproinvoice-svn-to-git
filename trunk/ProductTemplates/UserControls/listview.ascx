<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTemplates.ListView" Codebehind="ListView.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="SearchView" Src="~/CRM/UserControls/SearchView.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ExportHeader" Src="~/CRM/UserControls/ExportHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="MassUpdate" Src="MassUpdate.ascx" %>

<script type="text/javascript">
function CalendarPopup(ctlDate, clientX, clientY)
{
    CalendarText = ctlDate;
	show_cal(this);
}
</script>

<div id="divListView">
    <CRM:ModuleHeader ID="ctlModuleHeader" Module="ProductTemplates" Title=".moduleList.Home"
        EnablePrint="true" HelpName="index" EnableHelp="true" runat="Server" />
    <CRM:SearchView ID="ctlSearchView" Module="ProductTemplates" Visible="<%# !PrintView %>"
        runat="Server" />
    <CRM:ExportHeader ID="ctlExportHeader" Module="ProductTemplates" Title="ProductTemplates.LBL_LIST_FORM_TITLE"
        runat="Server" />
    <asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
        <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
    </asp:Panel>
    <asp:UpdateProgress ID="UpdateProgressMain" runat="server" AssociatedUpdatePanelID="UpdatePanelMain">
        <ProgressTemplate>
            <div id="loader" class="loader" align="center">
                <img src="<%=ResolveUrl("~/CRM/image/loading-spinner.gif") %>" alt="Loading..." /><span
                    id="loaderText" class="loaderText">Loading...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server">
        <ContentTemplate>
            <CRM:CRMGrid GridLines="None" ID="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>"
                EnableViewState="true" runat="server">
                <Columns>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="1%">
                        <ItemTemplate>
                            <input name="chkMain" class="checkbox" type="checkbox" value="<%# Eval( "ID") %>" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
    <CRM:MassUpdate ID="ctlMassUpdate" Visible="<%# !PrintView %>" runat="Server" />
</div>
