<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Invoices.LineItems" Codebehind="LineItems.ascx.cs" %>
<div id="divInvoicesLineItems">
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
            <CRM:CRMGrid GridLines="None" ID="grdMain" SkinID="grdLineItemView" EnableViewState="true"
                runat="server">
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
