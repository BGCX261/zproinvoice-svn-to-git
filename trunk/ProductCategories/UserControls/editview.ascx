<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductCategories.EditView" Codebehind="EditView.ascx.cs" %>
<script type="text/javascript">
function ChangeProductCategory(sPARENT_ID, sPARENT_NAME)
{
	var frm = document.forms[0];
	frm['<%= txtPARENT_ID.ClientID   %>'].value = sPARENT_ID  ;
	frm['<%= txtPARENT_NAME.ClientID %>'].value = sPARENT_NAME;
}
function ClearParent()
{
	var frm = document.forms[0];
	frm['<%= txtPARENT_ID.ClientID   %>'].value = '';
	frm['<%= txtPARENT_NAME.ClientID %>'].value = '';
}
function ProductCategoryPopup()
{
	return window.open('Popup.aspx','ProductCategoryPopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>
<div id="divEditView">
	<%@ Register TagPrefix="CRM" Tagname="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
	<CRM:ListHeader ID="ctlListHeader" Title="ProductCategories.LBL_NAME" Runat="Server" />
	<p>
	<%@ Register TagPrefix="CRM" Tagname="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
	<CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" ShowRequired="true" Runat="Server" />

	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Table SkinID="tabEditView" runat="server">
					<asp:TableRow>
						<asp:TableCell Width="15%" CssClass="dataLabel" VerticalAlign="top" Wrap="false"><%= Translation.GetTranslation.Term("ProductCategories.LBL_NAME") %> <asp:Label CssClass="required" Text='<%# Translation.GetTranslation.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></asp:TableCell>
						<asp:TableCell Width="85%" CssClass="dataField">
							<asp:TextBox ID="txtNAME" TabIndex="1" size="60" MaxLength="50" Runat="server" />
							<asp:RequiredFieldValidator ID="reqNAME" ControlToValidate="txtNAME" ErrorMessage='<%# Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel" VerticalAlign="top"><%= Translation.GetTranslation.Term("ProductCategories.LBL_PARENT") %></asp:TableCell>
						<asp:TableCell CssClass="dataField">
							<asp:TextBox ID="txtPARENT_NAME" ReadOnly="True" Runat="server" />
							<input ID="txtPARENT_ID" type="hidden" runat="server" NAME="txtPARENT_ID"/>
							<input ID="btnChangeParent" type="button" CssClass="button" onclick="return ProductCategoryPopup();" title="<%# Translation.GetTranslation.Term(".LBL_SELECT_BUTTON_TITLE") %>" value="<%# Translation.GetTranslation.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
							<input ID="btnClearParent"  type="button" CssClass="button" onclick="return ClearParent();"          title="<%# Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_TITLE" ) %>" value="<%# Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_LABEL" ) %>" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel" VerticalAlign="top"><%= Translation.GetTranslation.Term("ProductCategories.LBL_DESCRIPTION") %></asp:TableCell>
						<asp:TableCell CssClass="dataField">
							<asp:TextBox ID="txtDESCRIPTION" TabIndex="3"  TextMode="MultiLine" Rows="8" Columns="50" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= Translation.GetTranslation.Term("ProductCategories.LBL_ORDER") %> <asp:Label CssClass="required" Text='<%# Translation.GetTranslation.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></asp:TableCell>
						<asp:TableCell CssClass="dataField">
							<asp:TextBox ID="txtLIST_ORDER" TabIndex="4" size="5" MaxLength="10" Runat="server" />
							<asp:RequiredFieldValidator ID="reqLIST_ORDER" ControlToValidate="txtLIST_ORDER" ErrorMessage='<%# Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Display="Dynamic" Runat="server" />&nbsp;
							<em><%= Translation.GetTranslation.Term("ProductCategories.LBL_ORDER_DESC") %></em>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
</div>
