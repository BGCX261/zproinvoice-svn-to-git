<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Payments.Invoices" Codebehind="Invoices.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<div id="divPaymentsInvoices">
    <CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" runat="Server" />
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
            <CRM:CRMGrid GridLines="None" ID="grdMain" AllowPaging="false" AllowSorting="false"
                EnableViewState="true" runat="server">
                <Columns>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit", TypeConvert.ToGuid(Eval( "ASSIGNED_USER_ID"))) >= 0 %>'
                                CommandName="Invoices.Edit" CommandArgument='<%# Eval( "INVOICE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                SkinID="edit_inline" runat="server" />
                            <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit", TypeConvert.ToGuid(Eval( "ASSIGNED_USER_ID"))) >= 0 %>'
                                CommandName="Invoices.Edit" CommandArgument='<%# Eval( "INVOICE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
