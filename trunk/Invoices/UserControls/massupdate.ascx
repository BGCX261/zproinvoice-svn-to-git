<%@ Control Language="c#" Inherits="CRM.InvoiceManagement.Invoices.MassUpdate" CodeBehind="MassUpdate.ascx.cs" %>
<%@ Register TagPrefix="CRM" TagName="DatePicker" Src="~/CRM/UserControls/DatePicker.ascx" %>
<%@ Register TagPrefix="CRM" TagName="ListHeader" Src="~/CRM/UserControls/ListHeader.ascx" %>
<%@ Register TagPrefix="CRM" TagName="TeamAssignedMassUpdate" Src="~/CRM/UserControls/TeamAssignedMassUpdate.ascx" %>
<div id="divMassUpdate">
    <ajaxToolkit:ModalPopupExtender ID="MPE" runat="server" TargetControlID="btnVisibletable"
        PopupControlID="MoreAction" BackgroundCssClass="modalBackground" DropShadow="true"
        CancelControlID="ImgClose" />
    <table style="display: inline;">
        <tr >
            <td valign="bottom" >
                <asp:Button ID="btnDelete" CommandName="MassDelete" OnCommand="Page_Command" CssClass="button"
                    Text='<%# Translation.GetTranslation.Term(".LBL_DELETE") %>' runat="server" />
            </td>
            <td valign="middle" >
                <asp:Button ID="btnVisibletable" runat="server" CssClass="button" Text="Mass Update" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" type="text/javascript">
        function HidePopup() {
            document.getElementById('<%=MoreAction.ClientID %>').style.display = "none";
        }
    </script>
    <asp:Panel runat="server" ID="MoreAction" CssClass="tabForm" Style="width: 300px;
        display: none;" HorizontalAlign="center">
        <asp:Panel ID="pnlMove" runat="server" Style="z-index: 1000;">
            <asp:ImageButton ID="ImgClose" runat="server" ImageUrl="~/App_Themes/Default/images/exit.gif"
                AlternateText="Close" ToolTip="Close" Style="float: right; margin-right: 5px;"
                OnClientClick="HidePopup();" />
        </asp:Panel>
        <table cellpadding="2" cellspacing="0" border="0" width="300">
            <tr>
                <td colspan="2" style="height: 5px;">
                </td>
            </tr>
            <CRM:TeamAssignedMassUpdate ID="ctlTeamAssignedMassUpdate" runat="Server" />
            <tr style="display: none; visibility: hidden;">
                <td align="right">
                    <asp:Label ID="Label3" Text='<%# Translation.GetTranslation.Term("Invoices.LBL_DUE_DATE") %>'
                        runat="server" />
                </td>
                <td align="left">
                    <CRM:DatePicker ID="ctlDUE_DATE" runat="Server" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label1" Text='<%# Translation.GetTranslation.Term("Invoices.LBL_PAYMENT_TERMS") %>'
                        runat="server" />
                </td>
                <td align="left">
                    <asp:DropDownList ID="lstPAYMENT_TERMS" DataValueField="NAME" DataTextField="DISPLAY_NAME"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" Text='<%# Translation.GetTranslation.Term("Invoices.LBL_INVOICE_STAGE") %>'
                        runat="server" />
                </td>
                <td align="left">
                    <asp:DropDownList ID="lstINVOICE_STAGE" DataValueField="NAME" DataTextField="DISPLAY_NAME"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="left">
                    <asp:Button ID="btnUpdate" CommandName="MassUpdate" OnCommand="Page_Command" CssClass="button"
                        Text='<%# Translation.GetTranslation.Term(".LBL_UPDATE") %>' runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
