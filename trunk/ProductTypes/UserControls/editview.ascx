<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTypes.EditView" Codebehind="EditView.ascx.cs" %>
<div id="divEditView">
	<%@ Register TagPrefix="CRM" Tagname="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
	<CRM:ListHeader ID="ctlListHeader" Title="ProductTypes.LBL_NAME" Runat="Server" />
	<p>
	<%@ Register TagPrefix="CRM" Tagname="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
	<CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" ShowRequired="true" Runat="Server" />

	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Table SkinID="tabEditView" runat="server">
					<asp:TableRow>
						<asp:TableCell Width="15%" CssClass="dataLabel" VerticalAlign="top" Wrap="false"><%= Translation.GetTranslation.Term("ProductTypes.LBL_NAME") %> <asp:Label CssClass="required" Text='<%# Translation.GetTranslation.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></asp:TableCell>
						<asp:TableCell Width="35%" CssClass="dataField">
							<asp:TextBox ID="txtNAME" TabIndex="1" size="60" MaxLength="50" Runat="server" />
							<asp:RequiredFieldValidator ID="reqNAME" ControlToValidate="txtNAME" ErrorMessage='<%# Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Runat="server" />
						</asp:TableCell>
						<asp:TableCell Width="15%" CssClass="dataLabel">&nbsp;</asp:TableCell>
						<asp:TableCell Width="35%" CssClass="dataField">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel" VerticalAlign="top"><%= Translation.GetTranslation.Term("ProductTypes.LBL_DESCRIPTION") %></asp:TableCell>
						<asp:TableCell CssClass="dataField">
							<asp:TextBox ID="txtDESCRIPTION" TabIndex="2"  TextMode="MultiLine" Rows="8" Columns="50" Runat="server" />
						</asp:TableCell>
						<asp:TableCell CssClass="dataLabel">&nbsp;</asp:TableCell>
						<asp:TableCell CssClass="dataField">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell CssClass="dataLabel"><%= Translation.GetTranslation.Term("ProductTypes.LBL_ORDER") %> <asp:Label CssClass="required" Text='<%# Translation.GetTranslation.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></asp:TableCell>
						<asp:TableCell CssClass="dataField" ColumnSpan="3">
							<asp:TextBox ID="txtLIST_ORDER" TabIndex="3" size="5" MaxLength="10" Runat="server" />
							<asp:RequiredFieldValidator ID="reqLIST_ORDER" ControlToValidate="txtLIST_ORDER" ErrorMessage='<%# Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" Display="Dynamic" Runat="server" />&nbsp;
							<em><%= Translation.GetTranslation.Term("ProductTypes.LBL_ORDER_DESC") %></em>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
</div>
