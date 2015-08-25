<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTemplates.Notes" Codebehind="Notes.ascx.cs" %>

<script type="text/javascript">
function ChangeNote(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= txtNOTE_ID.ClientID %>').value = sPARENT_ID  ;
	document.forms[0].submit();
}
function NotePopup()
{
	return window.open('../Notes/Popup.aspx?ClearDisabled=1','NotePopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>

<div id="divProductTemplatesNotes">
    <input id="txtNOTE_ID" type="hidden" runat="server" />
    <br />
    <%@ register tagprefix="CRM" tagname="ListHeader" src="~/CRM/UserControls/ListHeader.ascx" %>
    <CRM:ListHeader Title="Notes.LBL_MODULE_NAME" runat="Server" />
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
                            <asp:ImageButton CommandName="Notes.Edit" CommandArgument='<%# Eval( "NOTE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                SkinID="edit_inline" runat="server" />
                            <asp:LinkButton CommandName="Notes.Edit" CommandArgument='<%# Eval( "NOTE_ID") %>'
                                OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>'
                                runat="server" />
                            &nbsp; <span onclick="return confirm('<%= Translation.GetTranslation.Term("ProductTemplates.NTC_REMOVE_INVITEE") %>')">
                                <asp:ImageButton CommandName="Notes.Remove" CommandArgument='<%# Eval( "NOTE_ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_REMOVE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton CommandName="Notes.Remove" CommandArgument='<%# Eval( "NOTE_ID") %>'
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
