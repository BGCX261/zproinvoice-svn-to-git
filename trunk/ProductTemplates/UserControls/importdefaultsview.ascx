<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTemplates.ImportDefaultsView" Codebehind="ImportDefaultsView.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="EditButtons" Src="~/CRM/UserControls/EditButtons.ascx" %>

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

<div id="divDefaultsView">
    <CRM:ModuleHeader ID="ctlModuleHeader" Module="ProductTemplates" EnablePrint="false"
        HelpName="EditView" EnableHelp="true" runat="Server" />
    <p>
        <CRM:EditButtons ID="ctlEditButtons" Visible="<%# !PrintView %>" runat="Server" />

        <script type="text/javascript">
	function ChangeAccount(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "ACCOUNT_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "ACCOUNT_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function AccountPopup()
	{
		return window.open('../../Accounts/Popup.aspx','AccountPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	function ChangeOpportunity(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "OPPORTUNITY_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "OPPORTUNITY_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function OpportunityPopup()
	{
		return window.open('../../Opportunities/Popup.aspx','OpportunityPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
	function ChangeProductCategory(sPARENT_ID, sPARENT_NAME)
	{
		document.getElementById('<%= new DynamicControl(this, "CATEGORY_ID"  ).ClientID %>').value = sPARENT_ID  ;
		document.getElementById('<%= new DynamicControl(this, "CATEGORY_NAME").ClientID %>').value = sPARENT_NAME;
	}
	function ProductCategoryPopup()
	{
		return window.open('../ProductCategories/Popup.aspx','ProductCategoryPopup','width=600,height=400,resizable=1,scrollbars=1');
	}
        </script>

        <asp:Table ID="Table1" SkinID="tabForm" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <table id="tblMain" class="tabEditView" runat="server">
                    </table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </p>
</div>
