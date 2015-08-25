<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Products.Notes" CodeBehind="Notes.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<div id="divProductsNotes">
    <br>
    <CRM:ListHeader Title="Notes.LBL_MODULE_NAME" runat="Server" />
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
                            <asp:ImageButton CommandName="Notes.Edit" CommandArgument='<%# Eval( "NOTE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                SkinID="edit_inline" runat="server" />
                            <asp:LinkButton CommandName="Notes.Edit" CommandArgument='<%# Eval( "NOTE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                runat="server" />
                            &nbsp; <span onclick="return confirm('<%= Translation.GetTranslation.Term("Products.NTC_REMOVE_INVITEE") %>')">
                                <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Notes", "delete", TypeConvert.ToGuid(Eval( "ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Notes.Delete" CommandArgument='<%# Eval( "NOTE_ID") %>' OnCommand="Page_Command"
                                    CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_DELETE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess("Notes", "delete", TypeConvert.ToGuid(Eval( "ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Notes.Delete" CommandArgument='<%# Eval( "NOTE_ID") %>' OnCommand="Page_Command"
                                    CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_DELETE") %>'
                                    runat="server" />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
