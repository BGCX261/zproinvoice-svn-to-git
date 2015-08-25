<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Invoices.Payments" CodeBehind="Payments.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<div id="divInvoicesPayments">
    <br />
    <CRM:ListHeader Title="Invoices.LBL_PAYMENTS" runat="Server" />
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
            <CRM:CRMGrid GridLines="None" ID="grdMain" SkinID="grdSubPanelView" AllowPaging="<%# !PrintView %>"
                EnableViewState="true" runat="server">
                <Columns>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit", TypeConvert.ToGuid(Eval( "ASSIGNED_USER_ID"))) >= 0 %>'
                                CommandName="Payments.Edit" CommandArgument='<%# Eval( "PAYMENT_ID") %>' OnCommand="Page_Command"
                                CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                SkinID="edit_inline" runat="server" />
                            <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit", TypeConvert.ToGuid(Eval( "ASSIGNED_USER_ID"))) >= 0 %>'
                                CommandName="Payments.Edit" CommandArgument='<%# Eval( "PAYMENT_ID") %>' OnCommand="Page_Command"
                                CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                runat="server" />
                            &nbsp; <span onclick="return confirm('<%= Translation.GetTranslation.Term(".NTC_DELETE_CONFIRMATION") %>')">
                                <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit", TypeConvert.ToGuid(Eval( "INVOICE_ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Payments.Remove" CommandArgument='<%# Eval( "INVOICE_PAYMENT_ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_REMOVE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit", TypeConvert.ToGuid(Eval( "INVOICE_ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Payments.Remove" CommandArgument='<%# Eval( "INVOICE_PAYMENT_ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_REMOVE") %>'
                                    runat="server" />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
