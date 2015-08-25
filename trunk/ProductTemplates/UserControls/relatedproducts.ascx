<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTemplates.RelatedProductTemplates" Codebehind="RelatedProducts.ascx.cs" %>

<script type="text/javascript">
function ChangeProductTemplate(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= txtPRODUCT_TEMPLATE_ID.ClientID %>').value = sPARENT_ID  ;
	document.forms[0].submit();
}
function ProductTemplatePopup()
{
	return window.open('../ProductTemplates/Popup.aspx?ClearDisabled=1','ProductTemplatePopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>

<div id="divProductTemplatesProductTemplates">
    <input id="txtPRODUCT_TEMPLATE_ID" type="hidden" runat="server" />
    <br />
    <%@ register tagprefix="CRM" tagname="ListHeader" src="~/CRM/UserControls/ListHeader.ascx" %>
    <CRM:ListHeader Title="ProductTemplates.LBL_MODULE_NAME" runat="Server" />
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
                            <asp:ImageButton CommandName="ProductTemplates.Edit" CommandArgument='<%# Eval( "PRODUCT_TEMPLATE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                SkinID="edit_inline" runat="server" />
                            <asp:LinkButton CommandName="ProductTemplates.Edit" CommandArgument='<%# Eval( "PRODUCT_TEMPLATE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                runat="server" />
                            &nbsp; <span onclick="return confirm('<%= Translation.GetTranslation.Term("ProductTemplates.NTC_REMOVE_INVITEE") %>')">
                                <asp:ImageButton CommandName="ProductTemplates.Remove" CommandArgument='<%# Eval( "PRODUCT_TEMPLATE_ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_REMOVE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton CommandName="ProductTemplates.Remove" CommandArgument='<%# Eval( "PRODUCT_TEMPLATE_ID") %>'
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
