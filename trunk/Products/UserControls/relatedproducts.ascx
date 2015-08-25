<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Products.RelatedProducts"
    CodeBehind="RelatedProducts.ascx.cs" %>

<script type="text/javascript">
    function ChangeProduct(sPARENT_ID, sPARENT_NAME) {
        document.getElementById('<%= txtCHILD_ID.ClientID %>').value = sPARENT_ID;
        document.forms[0].submit();
    }
    function ProductPopup() {
        return window.open('../Products/Popup.aspx?ClearDisabled=1&ACCOUNT_ID=<%= gACCOUNT_ID.ToString() %>', 'ProductPopup', 'width=600,height=400,resizable=1,scrollbars=1');
    }
</script>

<div id="divProductsProducts">
    <input id="txtCHILD_ID" type="hidden" runat="server" />
    <br />
    <%@ register tagprefix="CRM" tagname="ListHeader" src="~/CRM/UserControls/ListHeader.ascx" %>
    <CRM:ListHeader Title="Products.LBL_MODULE_NAME" runat="Server" />
    <%@ register tagprefix="CRM" tagname="DynamicButtons" src="~/CRM/UserControls/DynamicButtons.ascx" %>
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
                            <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit") >= 0 %>'
                                CommandName="Products.Edit" CommandArgument='<%# Eval( "ID") %>' OnCommand="Page_Command"
                                CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                SkinID="edit_inline" runat="server" />
                            <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit") >= 0 %>'
                                CommandName="Products.Edit" CommandArgument='<%# Eval( "ID") %>' OnCommand="Page_Command"
                                CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                runat="server" />
                            &nbsp; <span onclick="return confirm('<%= Translation.GetTranslation.Term("Products.NTC_REMOVE_INVITEE") %>')">
                                <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit") >= 0 %>'
                                    CommandName="Products.Remove" CommandArgument='<%# Eval( "ID") %>' OnCommand="Page_Command"
                                    CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_REMOVE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Invoices", "edit") >= 0 %>'
                                    CommandName="Products.Remove" CommandArgument='<%# Eval( "ID") %>' OnCommand="Page_Command"
                                    CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_REMOVE") %>'
                                    runat="server" />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
