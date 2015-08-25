<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Invoices.ListView" CodeBehind="ListView.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="SearchView" Src="~/CRM/UserControls/SearchView.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ExportHeader" Src="~/CRM/UserControls/ExportHeader.ascx" %>
<%@ Register Src="~/CRM/UserControls/GridPageSize.ascx" TagName="GridPageSize" TagPrefix="CRM" %>
<%@ Register TagPrefix="CRM" TagName="MassUpdate" Src="MassUpdate.ascx" %>
<div id="divListView">
    <div style="width: 100%">
        <CRM:ModuleHeader ID="ctlModuleHeader" Module="Invoices" Title=".moduleList.Home"
            EnablePrint="true" HelpName="index" EnableHelp="true" runat="Server" />
    </div>
    <div>
        <table width="100%">
            <tr>
                <td align="left" valign="top">
                    <span >
                        <CRM:MassUpdate ID="ctlMassUpdate" Visible="<%# !PrintView %>" runat="Server" />
                    </span><span style="margin-left: -5px;">
                        <CRM:SearchView ID="ctlSearchView" Module="Invoices" Visible="true" IS_LISTDISPLAY="true"
                            runat="Server" />
                    </span>
                </td>
                <td align="right" style="padding-left: 2px;">
                    <CRM:GridPageSize ID="GridPageSize1" runat="server"></CRM:GridPageSize>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%">
        <CRM:ExportHeader ID="ctlExportHeader" Module="Invoices" Title="Invoices.LBL_LIST_FORM_TITLE"
            runat="Server" />
    </div>
    <div>

        <script type="text/javascript">
            function ChangeSearchAccount(sPARENT_ID, sPARENT_NAME) {
                document.getElementById('<%= new DynamicControl(ctlSearchView, "ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID;
                document.getElementById('<%= new DynamicControl(ctlSearchView, "ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
            }
            function SearchAccountPopup() {
                ChangeAccount = ChangeSearchAccount;
                return window.open('../Accounts/Popup.aspx', 'AccountPopup', 'width=600,height=400,resizable=1,scrollbars=1');
            }
        </script>

        <asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
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
    </div>

    <script language="javascript" type="text/javascript">
        function OpenStatusPopup(sURL) {
            var oWin = window.open(sURL, 'EditControl', 'width=600,height=200,resizable=1,scrollbars=1');
            oWin.focus();
            return false;
        }
    </script>

</div>