<%@ Page Language="c#" MasterPageFile="~/PopupView.Master" Inherits="CRM.InvoiceManagement.ProductCategories.Popup" Codebehind="Popup.aspx.cs" %>

<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="SearchPopup" Src="~/CRM/Invoice Management/ProductCategories/UserControls/SearchPopup.ascx" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <CRM:SearchPopup ID="ctlSearch" runat="Server" />
    <br />

    <script type="text/javascript">
function SelectProductCategory(sPARENT_ID, sPARENT_NAME)
{
	if ( p != null && p.ChangeProductCategory != null )
	{
		p.ChangeProductCategory(sPARENT_ID, sPARENT_NAME);
		window.close();
	}
	else
	{
		alert('Original window has closed.  Product Category cannot be assigned.' + '\n' + sPARENT_ID + '\n' + sPARENT_NAME);
	}
}
function Clear()
{
	if ( p != null && p.ChangeProductCategory != null )
	{
		p.ChangeProductCategory('', '');
		window.close();
	}
	else
	{
		alert('Original window has closed.  Product Category cannot be assigned.');
	}
}
function Cancel()
{
	window.close();
}
    </script>

    <CRM:ListHeader Title="ProductCategories.LBL_LIST_FORM_TITLE" runat="Server" />
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
