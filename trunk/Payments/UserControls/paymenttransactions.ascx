<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Payments.PaymentTransactions"
    CodeBehind="PaymentTransactions.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<div id="divPaymentsPaymentTransactions">
    <br />
    <CRM:ListHeader Title="Payments.LBL_PAYMENT_TRANSACTIONS" runat="Server" />
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
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <div visible='<%# (TypeConvert.ToString(Eval( "TRANSACTION_TYPE")) == "Sale") && (TypeConvert.ToString(Eval( "STATUS")) == "Success") %>'
                                runat="server">
                                <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit") >= 0 %>'
                                    CommandName="Refund" CommandArgument='<%# Eval( "ID") %>' OnCommand="Page_Command"
                                    CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term("Payments.LBL_REFUND_BUTTON_TITLE") %>'
                                    SkinID="edit_inline" runat="server" />
                                <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit") >= 0 %>'
                                    CommandName="Refund" CommandArgument='<%# Eval( "ID") %>' OnCommand="Page_Command"
                                    CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term("Payments.LBL_REFUND_BUTTON_LABEL") %>'
                                    runat="server" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
