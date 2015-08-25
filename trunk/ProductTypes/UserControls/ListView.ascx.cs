using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.ProductTypes
{
    /// <summary>
    ///		Summary description for ListView.
    /// </summary>
    public partial class ListView : CRMControl
    {
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "ProductTypes.Create")
            {
                Response.Redirect("edit.aspx");
            }
            else if (e.CommandName == "ProductTypes.Delete")
            {
                Guid gID = CommonTypeConvert.ToGuid(e.CommandArgument);
                CommonProcedure.ProductTypesDelete(gID);
                Cache.Remove("vwPRODUCT_TYPES_LISTBOX");
                Response.Redirect("Index.aspx");
            }
            else if (e.CommandName == "Export")
            {
                int nACLACCESS = Security.GetUserAccess(m_sMODULE, "export");
                if (nACLACCESS >= 0)
                {
                    string[] arrID = Request.Form.GetValues("chkMain");
                    CRMExport.Export(vwMain, m_sMODULE, ctlExportHeader.ExportFormat, ctlExportHeader.ExportRange,
                                     grdMain.CurrentPageIndex, grdMain.PageSize, arrID);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term("ProductTypes.LBL_LIST_FORM_TITLE"));
            if (!Visible)
                return;

            var oQuery = new InlineQueryDBManager();
            string innerSql = ApplicationSQL.SQL["Administration_ProductTypes_ListView"].ToString();
            oQuery.CommandText = innerSql;
            DataTable dt = oQuery.GetTable();
            vwMain = dt.DefaultView;
            grdMain.DataSource = vwMain;
            if (!IsPostBack)
            {
                grdMain.SortColumn = "LIST_ORDER";
                grdMain.SortOrder = "asc";
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            m_sMODULE = "ProductTypes";
            ctlExportHeader.Command = new CommandEventHandler(Page_Command);
        }

        #endregion
    }
}