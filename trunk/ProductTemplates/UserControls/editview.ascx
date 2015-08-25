<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.ProductTemplates.EditView" Codebehind="EditView.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="ModuleHeader" Src="~/CRM/UserControls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="DynamicButtons" Src="~/CRM/UserControls/DynamicButtons.ascx" %>


<script type="text/javascript">
function CalendarPopup(ctlDate, clientX, clientY)
{
    CalendarText = ctlDate;
	show_cal(this);
}
</script>

<div id="divEditView">
    <CRM:ModuleHeader ID="ctlModuleHeader" Module="ProductTemplates" EnablePrint="false"
        HelpName="EditView" EnableHelp="true" runat="Server" />
    <p>
        <CRM:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" ShowRequired="true"
            runat="Server" />

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

        <asp:Table SkinID="tabForm" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <table id="tblMain" class="tabEditView" runat="server">
                    </table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </p>
</div>

