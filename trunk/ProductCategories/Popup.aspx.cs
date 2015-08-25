using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.ProductCategories
{
    /// <summary>
    /// Summary description for Popup.
    /// </summary>
    public partial class Popup : CRMPopup
    {
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Search")
            {
                grdMain.CurrentPageIndex = 0;
                grdMain.ApplySort();
                grdMain.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term(m_sMODULE + ".LBL_LIST_FORM_TITLE"));

            var oQuery = new InlineQueryDBManager();
            string innerSql = ApplicationSQL.SQL["Administration_ProductCategories_Popup"].ToString();
            oQuery.CommandText = innerSql;
            ctlSearch.SqlSearchClause(oQuery);

            DataTable dt = oQuery.GetTable();
            vwMain = dt.DefaultView;
            grdMain.DataSource = vwMain;
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(grdMain.SortColumn))
                {
                    grdMain.SortColumn = "NAME";
                    grdMain.SortOrder = "asc";
                }
                grdMain.ApplySort();
                grdMain.DataBind();
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
            ctlSearch.Command = new CommandEventHandler(Page_Command);
            m_sMODULE = "ProductCategories";
            this.AppendGridColumns(grdMain, m_sMODULE + ".PopupView");
            // ctlDynamicButtons.AppendButtons(m_sMODULE + ".PopupView", Guid.Empty, Guid.Empty);
            // if (!IsPostBack)
            //     ctlDynamicButtons.ShowButton("Clear", !TypeConvert.ToBoolean(Request["ClearDisabled"]));
        }

        #endregion
    }
}