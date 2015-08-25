<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductCategories.SearchPopup" Codebehind="SearchPopup.ascx.cs" %>
<div id="divSearch">
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Table SkinID="tabSearchView" runat="server">
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel" Wrap="false"><%= Translation.GetTranslation.Term("ProductCategories.LBL_NAME") %>&nbsp;&nbsp;<asp:TextBox ID="txtNAME" CssClass="dataField" Runat="server" /></asp:TableCell>
						<asp:TableCell HorizontalAlign="Right">
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# Translation.GetTranslation.Term(".LBL_SEARCH_BUTTON_LABEL") %>' ToolTip='<%# Translation.GetTranslation.Term(".LBL_SEARCH_BUTTON_TITLE") %>' Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<%= Utils.RegisterEnterKeyPress(txtNAME.ClientID, btnSearch.ClientID) %>
</div>
