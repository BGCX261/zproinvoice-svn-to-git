<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Manufacturers.ListView" Codebehind="ListView.ascx.cs" %>

<script runat="server">
</script>

<div id="divListView">
    <%@ register tagprefix="CRM" tagname="ExportHeader" src="~/CRM/UserControls/ExportHeader.ascx" %>
    <CRM:ExportHeader ID="ctlExportHeader" Module="Manufacturers" Title="Manufacturers.LBL_LIST_FORM_TITLE"
        runat="Server" />
    <asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
        <asp:Button ID="btnCreate" CommandName="Manufacturers.Create" OnCommand="Page_Command"
            CssClass="button" Text='<%# "  " + Translation.GetTranslation.Term(".LBL_CREATE_BUTTON_LABEL") + "  " %>'
            ToolTip='<%# Translation.GetTranslation.Term(".LBL_CREATE_BUTTON_LABEL") %>' 
            runat="server" />
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
            <CRM:CRMGrid GridLines="None" ID="grdMain" AllowPaging="false" AllowSorting="true"
                EnableViewState="true" runat="server">
                <Columns>
                    <asp:TemplateColumn HeaderText="Manufacturers.LBL_LIST_NAME" SortExpression="NAME"
                        ItemStyle-Width="60%" ItemStyle-CssClass="listViewTdLinkS1">
                        <ItemTemplate>
                            <asp:HyperLink Text='<%# Eval( "NAME") %>' NavigateUrl='<%# "edit.aspx?ID=" + Eval( "ID") %>'
                                CssClass="listViewTdLinkS1" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn HeaderText="Manufacturers.LBL_LIST_STATUS" DataField="STATUS" SortExpression="STATUS"
                        ItemStyle-Width="20%" />
                    <asp:BoundColumn HeaderText="Manufacturers.LBL_LIST_ORDER" DataField="LIST_ORDER"
                        SortExpression="LIST_ORDER" ItemStyle-Width="12%" />
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <span onclick="return confirm('<%= Translation.GetTranslation.Term(".NTC_DELETE_CONFIRMATION") %>')">
                                <asp:ImageButton CommandName="Manufacturers.Delete" CommandArgument='<%# Eval( "ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_DELETE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton CommandName="Manufacturers.Delete" CommandArgument='<%# Eval( "ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# Translation.GetTranslation.Term(".LNK_DELETE") %>'
                                    runat="server" />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
