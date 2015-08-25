<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Payments.EditView" Codebehind="EditView.ascx.cs" %>
<%@ Register TagPrefix="CRM" Tagname="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="TeamAssignedPopupScripts" Src="~/CRM/UserControls/TeamAssignedPopupScripts.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="AllocationsView" Src="AllocationsView.ascx" %>

<script type="text/javascript">
Sys.CultureInfo.CurrentCulture.numberFormat.CurrencySymbol = '';

function ChangeAccount(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
	document.forms[0].submit();
}
function AccountPopup()
{
	return window.open('../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
}

function ChangeCreditCard(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "CREDIT_CARD_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "CREDIT_CARD_NAME").ClientID %>').value = sPARENT_NAME;
}
function CreditCardPopup()
{
	var fldACCOUNT_ID = document.getElementById('<%= new DynamicControl(this, "ACCOUNT_ID"  ).ClientID %>');
	return window.open('../CreditCards/Popup.aspx?ACCOUNT_ID=' + fldACCOUNT_ID.value,'CreditCardPopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>
<div id="divEditView">
	<CRM:ModuleHeader ID="ctlModuleHeader" Module="Payments" EnablePrint="false" HelpName="EditView" EnableHelp="true" Runat="Server" />
	<p>
	<CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" ShowRequired="true" Runat="Server" />

	<CRM:TeamAssignedPopupScripts ID="ctlTeamAssignedPopupScripts" Runat="Server" />
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table ID="tblMain" class="tabEditView" runat="server">
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
	<p>
		<CRM:AllocationsView ID="ctlAllocationsView" Runat="Server" />
	</p>
</div>
