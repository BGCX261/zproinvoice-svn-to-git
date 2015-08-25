<%@ Page Language="c#" MasterPageFile="~/PopupView.Master" Inherits="CRM.InvoiceManagement.Invoices.Popup" Codebehind="Popup.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="SearchView" Src="~/CRM/UserControls/SearchViewPopup.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:SearchView ID="ctlSearchView" Module="Invoices" IsPopupSearch="true" ShowSearchTabs="false"
        Visible="<%# !PrintView %>" runat="Server" />

    <script type="text/javascript">
function SelectInvoice(sPARENT_ID, sPARENT_NAME)
{
	if ( p != null && p.ChangeInvoice != null )
	{
		p.ChangeInvoice(sPARENT_ID, sPARENT_NAME);
		window.close();
	}
	else
	{
		alert('Original window has closed.  Invoice cannot be assigned.' + '\n' + sPARENT_ID + '\n' + sPARENT_NAME);
	}
}
function Clear()
{
	if ( p != null && p.ChangeInvoice != null )
	{
		p.ChangeInvoice('', '');
		window.close();
	}
	else
	{
		alert('Original window has closed.  Invoice cannot be assigned.');
	}
}
function Cancel()
{
	window.close();
}
    </script>

    <CRM:ListHeader Title="Invoices.LBL_LIST_FORM_TITLE" runat="Server" />
    <asp:UpdateProgress ID="UpdateProgressMain" runat="server" AssociatedUpdatePanelID="UpdatePanelMain">
        <ProgressTemplate>
            <div id="loader" class="loader" align="center">
                <img src="<%=ResolveUrl("~/CRM/image/loading-spinner.gif") %>" alt="Loading..." /><span
                    id="loaderText" class="loaderText">Loading...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server">
        <ContentTemplate>
            <CRM:CRMGrid GridLines="None" ID="grdMain" SkinID="grdPopupView" EnableViewState="true"
                runat="server">
            </CRM:CRMGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
