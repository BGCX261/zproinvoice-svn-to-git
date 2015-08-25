<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Invoices.EditView" Codebehind="EditView.ascx.cs" %>
<%@ Register TagPrefix="CRM" Tagname="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="TeamAssignedPopupScripts" Src="~/CRM/UserControls/TeamAssignedPopupScripts.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="EditLineItemsView" Src="~/CRM/UserControls/EditLineItemsView.ascx" %>
<script type="text/javascript">
var ChangeAccount = null;
var ChangeContact = null;
function ChangeOpportunity(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "OPPORTUNITY_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "OPPORTUNITY_NAME").ClientID %>').value = sPARENT_NAME;
}
function OpportunityPopup()
{
	return window.open('../Opportunities/Popup.aspx?ClearDisabled=1','OpportunitiesPopup','width=600,height=400,resizable=1,scrollbars=1');
}

function BillingChangeAccount(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "BILLING_ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "BILLING_ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
	document.forms[0].submit();
}
function BillingAccountPopup()
{
	ChangeAccount = BillingChangeAccount;
	return window.open('../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
}
function ShippingChangeAccount(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
	document.forms[0].submit();
}
function ShippingAccountPopup()
{
	ChangeAccount = ShippingChangeAccount;
	return window.open('../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
}

function BillingChangeContact(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "BILLING_CONTACT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "BILLING_CONTACT_NAME").ClientID %>').value = sPARENT_NAME;
}
function BillingContactPopup()
{
	ChangeContact = BillingChangeContact;
	var sACCOUNT_NAME = document.getElementById('<%= new DynamicControl(this, "BILLING_ACCOUNT_NAME"  ).ClientID %>').value;
	return window.open('../Contacts/Popup.aspx?ACCOUNT_NAME=' + sACCOUNT_NAME,'ContactPopup','width=600,height=400,resizable=1,scrollbars=1');
}
function ShippingChangeContact(sPARENT_ID, sPARENT_NAME)
{
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_CONTACT_ID"  ).ClientID %>').value = sPARENT_ID  ;
	document.getElementById('<%= new DynamicControl(this, "SHIPPING_CONTACT_NAME").ClientID %>').value = sPARENT_NAME;
}
function ShippingContactPopup()
{
	ChangeContact = ShippingChangeContact;
	var sACCOUNT_NAME = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ACCOUNT_NAME"  ).ClientID %>').value;
	return window.open('../Contacts/Popup.aspx?ACCOUNT_NAME=' + sACCOUNT_NAME,'ContactPopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>
<div id="divEditView">
	
	<CRM:ModuleHeader ID="ctlModuleHeader" Module="Invoices" EnablePrint="false" HelpName="EditView" EnableHelp="true" Runat="Server" />
	<p>
	
	<CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" ShowRequired="true" Runat="Server" />

	
	<CRM:TeamAssignedPopupScripts ID="ctlTeamAssignedPopupScripts" Runat="Server" />
	<asp:HiddenField ID="QUOTE_ID" runat="server" />
	<asp:HiddenField ID="ORDER_ID" runat="server" />
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table id="tblMain" class="tabEditView" runat="server">
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
	<p>
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table id="tblAddress" class="tabEditView" runat="server">
					<tr>
						<th colspan="2"><h4><asp:Label Text='<%# Translation.GetTranslation.Term("Invoices.LBL_BILLING_TITLE" ) %>' runat="server" /></h4></th>
						<th>&nbsp;</th>
						<th colspan="2"><h4><asp:Label Text='<%# Translation.GetTranslation.Term("Invoices.LBL_SHIPPING_TITLE") %>' runat="server" /></h4></th>
					</tr>
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
	<p>
		
		<CRM:EditLineItemsView ID="ctlEditLineItemsView" MODULE="Invoices" MODULE_KEY="INVOICE_ID" ShowCostPrice="false" Runat="Server" />
	</p>
	<p>
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table ID="tblDescription" class="tabEditView" runat="server">
					<tr>
						<th colspan="2"><h4><asp:Label Text='<%# Translation.GetTranslation.Term("Invoices.LBL_DESCRIPTION_TITLE") %>' runat="server" /></h4></th>
					</tr>
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>

	<script type="text/javascript">
	function copyAddressRight()
	{
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STREET"    ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STREET"    ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_CITY"      ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_CITY"      ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STATE"     ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STATE"     ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_POSTALCODE").ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_POSTALCODE").ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_COUNTRY"   ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_COUNTRY"   ).ClientID %>').value;
		return true;
	}
	function copyAddressLeft()
	{
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STREET"    ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STREET"    ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_CITY"      ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_CITY"      ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_STATE"     ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_STATE"     ).ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_POSTALCODE").ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_POSTALCODE").ClientID %>').value;
		document.getElementById('<%= new DynamicControl(this, "BILLING_ADDRESS_COUNTRY"   ).ClientID %>').value = document.getElementById('<%= new DynamicControl(this, "SHIPPING_ADDRESS_COUNTRY"   ).ClientID %>').value;
		return true;
	}
	</script>
</div>
