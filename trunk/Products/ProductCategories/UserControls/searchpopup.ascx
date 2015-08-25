<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Products.ProductCategories.SearchPopup" Codebehind="SearchPopup.ascx.cs" %>
<%@ Register TagPrefix="CRM" Tagname="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>

<CRM:ListHeader Title="ProductCategories.LBL_SEARCH_FORM_TITLE" Runat="Server" />
<div id="divSearch">
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Table SkinID="tabSearchView" runat="server">
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel" Wrap="false"><%= Translation.GetTranslation.Term("ProductCategories.LBL_NAME") %></asp:TableCell>
						<asp:TableCell CssClass="dataLabel" Wrap="false"><asp:TextBox ID="txtNAME"   CssClass="dataField" size="20" Runat="server" /></asp:TableCell>
						<asp:TableCell align="right">
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# Translation.GetTranslation.Term(".LBL_SEARCH_BUTTON_LABEL") %>' ToolTip='<%# Translation.GetTranslation.Term(".LBL_SEARCH_BUTTON_TITLE") %>' Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<%= Utils.RegisterEnterKeyPress(txtNAME.ClientID, btnSearch.ClientID) %>
	<%= Utils.RegisterSetFocus(txtNAME.ClientID) %>
</div>
