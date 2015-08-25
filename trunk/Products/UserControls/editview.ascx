<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Products.EditView" Codebehind="EditView.ascx.cs" %>
<%@ Register TagPrefix="CRM" Tagname="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>
<%@ Register TagPrefix="CRM" Tagname="TeamAssignedPopupScripts" Src="~/CRM/UserControls/TeamAssignedPopupScripts.ascx" %>


<div id="divEditView">
	<CRM:ModuleHeader ID="ctlModuleHeader" Module="Products" EnablePrint="false" HelpName="EditView" EnableHelp="true" Runat="Server" />
	<p>
	<CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" ShowRequired="true" Runat="Server" />

	<CRM:TeamAssignedPopupScripts ID="ctlTeamAssignedPopupScripts" Runat="Server" />
	<script type="text/javascript">
	function ChangeAccount(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function AccountPopup()
	{
		return window.open('../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	function ChangeContact(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "CONTACT_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "CONTACT_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function ContactPopup()
	{
		var sACCOUNT_NAME = document.getElementById('<%= new DynamicControl(this, "ACCOUNT_NAME"  ).ClientID %>').value;
		return window.open('../Contacts/Popup.aspx?ACCOUNT_NAME=' + sACCOUNT_NAME,'ContactPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	function ChangeProductCategory(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "CATEGORY_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "CATEGORY_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function ProductCategoryPopup()
	{
		return window.open('ProductCategories/Popup.aspx','ProductCategoryPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	function ChangeProductTemplate(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "PRODUCT_TEMPLATE_ID").ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "NAME"               ).ClientID %>').value = sPARENT_NAME;
		var btnProductChanged = document.getElementById('<%= btnProductChanged.ClientID %>');
		btnProductChanged.click();
	}
	function ProductCatalogPopup()
	{
		return window.open('ProductCatalog/Popup.aspx','ProductCatalogPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	</script>
	<asp:Button ID="btnProductChanged" Text="Product Changed" OnClick="btnProductChanged_Clicked" style="DISPLAY:none" runat="server" />
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table ID="tblMain" class="tabEditView" runat="server">
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table ID="tblCost" class="tabEditView" runat="server">
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br />
	<asp:Table SkinID="tabForm" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<table ID="tblManufacturer" class="tabEditView" runat="server">
				</table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
</div>


