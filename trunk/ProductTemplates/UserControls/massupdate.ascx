<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTemplates.MassUpdate" Codebehind="MassUpdate.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="DatePicker" Src="~/CRM/UserControls/DatePicker.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>

<script type="text/javascript">
function CalendarPopup(ctlDate, clientX, clientY)
{
    CalendarText = ctlDate;
	show_cal(this);
//	clientX = window.screenLeft + parseInt(clientX);
//	clientY = window.screenTop  + parseInt(clientY);
//	if ( clientX < 0 )
//		clientX = 0;
//	if ( clientY < 0 )
//		clientY = 0;
//	return window.open('../../Calendar/Popup.aspx?Date=' + ctlDate.value,'CalendarPopup','width=193,height=155,resizable=1,scrollbars=0,left=' + clientX + ',top=' + clientY);
}
</script>

<div id="divMassUpdate">
    <br />
    <CRM:ListHeader Title=".LBL_MASS_UPDATE_TITLE" runat="Server" />
    <asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
        <asp:Button ID="btnUpdate" CommandName="MassUpdate" OnCommand="Page_Command" CssClass="button"
            Text='<%# Translation.GetTranslation.Term(".LBL_UPDATE") %>' runat="server" />
        <asp:Button ID="btnDelete" CommandName="MassDelete" OnCommand="Page_Command" CssClass="button"
            Text='<%# Translation.GetTranslation.Term(".LBL_DELETE") %>' runat="server" />
    </asp:Panel>

    <script type="text/javascript">
	function MassUpdateChangeAccount(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= txtACCOUNT_ID.ClientID   %>').value = sPARENT_ID  ;
		document.getElementById('<%= txtACCOUNT_NAME.ClientID %>').value = sPARENT_NAME;
	}
	function MassUpdateAccountPopup()
	{
		ChangeAccount = MassUpdateChangeAccount;
		return window.open('../../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
    </script>

    <asp:Table Width="100%" CellPadding="0" CellSpacing="0" CssClass="tabForm" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Table Width="100%" CellPadding="0" CellSpacing="0" runat="server">
                    <asp:TableRow>
                        <asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("ProductTemplates.LBL_DATE_COST_PRICE") %>' runat="server" /></asp:TableCell>
                        <asp:TableCell Width="35%" CssClass="dataField">
                            <CRM:DatePicker ID="ctlDATE_COST_PRICE" runat="Server" />
                        </asp:TableCell>
                        <asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("ProductTemplates.LBL_STATUS") %>' runat="server" /></asp:TableCell>
                        <asp:TableCell Width="35%" CssClass="dataField">
                            <asp:DropDownList ID="lstSTATUS" DataValueField="NAME" DataTextField="DISPLAY_NAME"
                                runat="server" /></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("ProductTemplates.LBL_TAX_CLASS") %>' runat="server" /></asp:TableCell>
                        <asp:TableCell Width="35%" CssClass="dataField">
                            <asp:DropDownList ID="lstTAX_CLASS" DataValueField="NAME" DataTextField="DISPLAY_NAME"
                                runat="server" /></asp:TableCell>
                        <asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("ProductTemplates.LBL_DATE_AVAILABLE") %>' runat="server" /></asp:TableCell>
                        <asp:TableCell Width="35%" CssClass="dataField">
                            <CRM:DatePicker ID="ctlDATE_AVAILABLE" runat="Server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("ProductTemplates.LBL_SUPPORT_TERM") %>' runat="server" /></asp:TableCell>
                        <asp:TableCell Width="35%" CssClass="dataField">
                            <asp:DropDownList ID="lstSUPPORT_TERM" DataValueField="NAME" DataTextField="DISPLAY_NAME"
                                runat="server" /></asp:TableCell>
                        <asp:TableCell Width="15%" CssClass="dataLabel"><asp:Label Text='<%# Translation.GetTranslation.Term("ProductTemplates.LBL_ACCOUNT_NAME") %>' runat="server" /></asp:TableCell>
                        <asp:TableCell Width="35%" CssClass="dataField">
                            <asp:TextBox ID="txtACCOUNT_NAME" ReadOnly="True" runat="server" />
                            <asp:HiddenField ID="txtACCOUNT_ID" runat="server" />
                            &nbsp;
                            <asp:Button ID="btnACCOUNT_ID" OnClientClick="MassUpdateAccountPopup(); return false;"
                                Text='<%# Translation.GetTranslation.Term(".LBL_CHANGE_BUTTON_LABEL") %>' ToolTip='<%# Translation.GetTranslation.Term(".LBL_CHANGE_BUTTON_TITLE") %>'
                                CssClass="button" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</div>
