<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Payments.MassUpdate" Codebehind="MassUpdate.ascx.cs" %>
<%@ Register TagPrefix="CRM" Tagname="DatePicker" Src="~/CRM/UserControls/DatePicker.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="TeamAssignedMassUpdate" Src="~/CRM/UserControls/TeamAssignedMassUpdate.ascx" %>

<div id="divMassUpdate">
	<br />
	<CRM:ListHeader Title=".LBL_MASS_UPDATE_TITLE" Runat="Server" />

	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Button ID="btnUpdate" CommandName="MassUpdate" OnCommand="Page_Command" CssClass="button" Text='<%# Translation.GetTranslation.Term(".LBL_UPDATE") %>' Runat="server" />
		<asp:Button ID="btnDelete" CommandName="MassDelete" OnCommand="Page_Command" CssClass="button" Text='<%# Translation.GetTranslation.Term(".LBL_DELETE") %>' Runat="server" />
	</asp:Panel>

	<asp:Table Width="100%" CellPadding="0" CellSpacing="0" CssClass="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<CRM:TeamAssignedMassUpdate ID="ctlTeamAssignedMassUpdate" Runat="Server" />
				<asp:Table Width="100%" CellPadding="0" CellSpacing="0" runat="server">
					<asp:TableRow>
						<asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("Payments.LBL_PAYMENT_DATE") %>' runat="server" /></asp:TableCell>
						<asp:TableCell Width="35%" CssClass="dataField"><CRM:DatePicker ID="ctlPAYMENT_DATE" Runat="Server" /></asp:TableCell>
						<asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("Payments.LBL_PAYMENT_TYPE") %>' runat="server" /></asp:TableCell>
						<asp:TableCell Width="35%" CssClass="dataField"><asp:DropDownList ID="lstPAYMENT_TYPE" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</div>
