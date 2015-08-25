using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.Controls;

namespace CRM.InvoiceManagement.Products.ProductCatalog
{
    /// <summary>
    /// Summary description for Popup.
    /// </summary>
    public partial class Popup : CRMPopup
    {
        protected UniqueStringCollection arrSelectFields;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Search")
            {
                grdMain.CurrentPageIndex = 0;
                grdMain.DataBind();
            }
            else if (e.CommandName == "SortGrid")
            {
                grdMain.SetSortFields(e.CommandArgument as string[]);
                arrSelectFields.Add(grdMain.SortColumn);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term("ProductTemplates.LBL_LIST_FORM_TITLE"));
            var oQuery = new InlineQueryDBManager();
            string innerSql = "  from vwPRODUCT_TEMPLATES_List" + ControlChars.CrLf
                              + " where 1 = 1                   " + ControlChars.CrLf;
            oQuery.CommandText = innerSql;
            if (!IsPostBack)
            {
                new DynamicControl(ctlSearchView, "NAME").Text = CommonTypeConvert.ToString(Request["NAME"]);
            }
            grdMain.OrderByClause("NAME", "asc");
            ctlSearchView.SqlSearchClause(oQuery);
            oQuery.CommandText = "select " + CommonTypeConvert.FormatSelectFields(arrSelectFields)
                                 + oQuery.CommandText
                                 + grdMain.OrderByClause();
            DataTable dt = oQuery.GetTable();
            vwMain = dt.DefaultView;
            grdMain.DataSource = vwMain;
            if (!IsPostBack)
            {
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
            ctlSearchView.Command = new CommandEventHandler(Page_Command);
            m_sMODULE = "ProductCatalog";
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("NAME");
            this.AppendGridColumns(grdMain, m_sMODULE + ".PopupView", arrSelectFields);
            //ctlDynamicButtons.AppendButtons(m_sMODULE + ".PopupView", Guid.Empty, Guid.Empty);
            //if (!IsPostBack)
            //    ctlDynamicButtons.ShowButton("Clear", !TypeConvert.ToBoolean(Request["ClearDisabled"]));
        }

        #endregion
    }
}