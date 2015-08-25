<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Invoices.Activities" Codebehind="Activities.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<div id="divInvoicesActivitiesOpen">
    <br />
    <CRM:ListHeader Title="Activities.LBL_OPEN_ACTIVITIES" runat="Server" />
    <CRM:DynamicButtons ID="ctlDynamicButtonsOpen" Visible="<%# !PrintView %>" runat="Server" />
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
            <CRM:CRMGrid GridLines="None" ID="grdOpen" SkinID="grdSubPanelView" AllowPaging="<%# !PrintView %>"
                EnableViewState="true" runat="server">
                <Columns>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <CRM:DynamicImage ImageSkinID='<%# Eval( "ACTIVITY_TYPE") %>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Activities.LBL_LIST_CLOSE" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink Visible='<%# CRM.CRMSecurity.GetUserAccess(TypeConvert.ToString(Eval( "ACTIVITY_TYPE")), "edit", TypeConvert.ToGuid(Eval( "ACTIVITY_ASSIGNED_USER_ID"))) >= 0 %>'
                                NavigateUrl='<%# "~/CRM/" + Eval( "ACTIVITY_TYPE") + "/edit.aspx?id=" + Eval( "ACTIVITY_ID") + "&Status=Close" + "&PARENT_ID=" + gID.ToString() %>'
                                runat="server">
						<asp:Image SkinID="close_inline" AlternateText='<%# Translation.GetTranslation.Term("Activities.LBL_LIST_CLOSE") %>' Runat="server" />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:HyperLink Visible='<%# CRM.CRMSecurity.GetUserAccess(TypeConvert.ToString(Eval( "ACTIVITY_TYPE")), "edit", TypeConvert.ToGuid(Eval( "ACTIVITY_ASSIGNED_USER_ID"))) >= 0 %>'
                                NavigateUrl='<%# "~/CRM/" + Eval( "ACTIVITY_TYPE") + "/edit.aspx?id=" + Eval( "ACTIVITY_ID") %>'
                                CssClass="listViewTdToolsS1" runat="server">
						<asp:Image SkinID="edit_inline" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>' Runat="server" />&nbsp;<%# Translation.GetTranslation.Term(".LNK_EDIT") %>
                            </asp:HyperLink>
                            &nbsp; <span onclick="return confirm('<%= Translation.GetTranslation.Term(".NTC_DELETE_CONFIRMATION") %>')">
                                <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess(TypeConvert.ToString(Eval( "ACTIVITY_TYPE")), "delete", TypeConvert.ToGuid(Eval( "ACTIVITY_ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Activities.Delete" CommandArgument='<%# Eval( "ACTIVITY_ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_DELETE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess(TypeConvert.ToString(Eval( "ACTIVITY_TYPE")), "delete", TypeConvert.ToGuid(Eval( "ACTIVITY_ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Activities.Delete" CommandArgument='<%# Eval( "ACTIVITY_ID") %>'
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
<div id="divInvoicesActivitiesHistory">
    <br />
    <CRM:ListHeader Title="Activities.LBL_HISTORY" runat="Server" />
    <CRM:DynamicButtons ID="ctlDynamicButtonsHistory" Visible="<%# !PrintView %>" runat="Server" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div id="loader" class="loader" align="center">
                <img src="<%=ResolveUrl("~/CRM/image/loading-spinner.gif") %>" alt="Loading..." /><span
                    id="loaderText" class="loaderText">Loading...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <CRM:CRMGrid GridLines="None" ID="grdHistory" SkinID="grdSubPanelView" AllowPaging="<%# !PrintView %>"
                EnableViewState="true" runat="server">
                <Columns>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <CRM:DynamicImage ImageSkinID='<%# Eval( "ACTIVITY_TYPE") %>'
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:HyperLink Visible='<%# CRM.CRMSecurity.GetUserAccess(TypeConvert.ToString(Eval( "ACTIVITY_TYPE")), "edit", TypeConvert.ToGuid(Eval( "ACTIVITY_ASSIGNED_USER_ID"))) >= 0 %>'
                                NavigateUrl='<%# "~/CRM/" + Eval( "ACTIVITY_TYPE") + "/edit.aspx?id=" + Eval( "ACTIVITY_ID") %>'
                                CssClass="listViewTdToolsS1" runat="server">
						<asp:Image SkinID="edit_inline" AlternateText='<%# Translation.GetTranslation.Term(".LNK_EDIT") %>' Runat="server" />&nbsp;<%# Translation.GetTranslation.Term(".LNK_EDIT") %>
                            </asp:HyperLink>
                            &nbsp; <span onclick="return confirm('<%= Translation.GetTranslation.Term(".NTC_DELETE_CONFIRMATION") %>')">
                                <asp:ImageButton Visible='<%# CRM.CRMSecurity.GetUserAccess(TypeConvert.ToString(Eval( "ACTIVITY_TYPE")), "delete", TypeConvert.ToGuid(Eval( "ACTIVITY_ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Activities.Delete" CommandArgument='<%# Eval( "ACTIVITY_ID") %>'
                                    OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# Translation.GetTranslation.Term(".LNK_DELETE") %>'
                                    SkinID="delete_inline" runat="server" />
                                <asp:LinkButton Visible='<%# CRM.CRMSecurity.GetUserAccess(TypeConvert.ToString(Eval( "ACTIVITY_TYPE")), "delete", TypeConvert.ToGuid(Eval( "ACTIVITY_ASSIGNED_USER_ID"))) >= 0 %>'
                                    CommandName="Activities.Delete" CommandArgument='<%# Eval( "ACTIVITY_ID") %>'
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
