using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Products
{
    /// <summary>
    ///		Summary description for ListView.
    /// </summary>
    public partial class ListView : CRMControl
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
            else if (e.CommandName == "MassUpdate")
            {
                string[] arrID = Request.Form.GetValues("chkMain");
                if (arrID != null)
                {
                    Stack stk = Utils.FilterByACL_Stack(m_sMODULE, "edit", arrID, "PRODUCTS");
                    if (stk.Count > 0)
                    {
                        var oQuery = new InlineQueryDBManager();
                        while (stk.Count > 0)
                        {
                            string sIDs = Utils.BuildMassIDs(stk);
                        }
                        Response.Redirect("Index.aspx");
                    }
                }
            }
            else if (e.CommandName == "MassDelete")
            {
                string[] arrID = Request.Form.GetValues("chkMain");
                if (arrID != null)
                {
                    Stack stk = Utils.FilterByACL_Stack(m_sMODULE, "delete", arrID, "PRODUCTS");
                    if (stk.Count > 0)
                    {
                        while (stk.Count > 0)
                        {
                            string sIDs = Utils.BuildMassIDs(stk);
                            CommonProcedure.ProductMassDelete(sIDs);
                        }
                        Response.Redirect("Index.aspx");
                    }
                }
            }
            else if (e.CommandName == "Export")
            {
                int nACLACCESS = Security.GetUserAccess(m_sMODULE, "export");
                if (nACLACCESS >= 0)
                {
                    if (nACLACCESS == ACL_ACCESS.OWNER)
                        vwMain.RowFilter = "ASSIGNED_USER_ID = '" + Security.USER_ID + "'";
                    string[] arrID = Request.Form.GetValues("chkMain");
                    CRMExport.Export(vwMain, m_sMODULE, ctlExportHeader.ExportFormat, ctlExportHeader.ExportRange,
                                     grdMain.CurrentPageIndex, grdMain.PageSize, arrID);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term(m_sMODULE + ".LBL_LIST_FORM_TITLE"));
            Visible = (Security.GetUserAccess(m_sMODULE, "list") >= 0);
            if (!Visible)
                return;

            if (IsMobile && grdMain.Columns.Count > 0)
                grdMain.Columns[0].Visible = false;
            var oQuery = new InlineQueryDBManager();

            string innerSql = "  from vwPRODUCTS_List" + ControlChars.CrLf;

            oQuery.CommandText = innerSql;
            CRMSecurity.Filter(oQuery, m_sMODULE, "list");
            grdMain.OrderByClause("NAME", "asc");
            ctlSearchView.SqlSearchClause(oQuery);
            oQuery.CommandText = "select " +
                                 (Request.Form[ctlExportHeader.ExportUniqueID] != null
                                      ? "*"
                                      : CommonTypeConvert.FormatSelectFields(arrSelectFields))
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
            // CODEGEN: This Product is required by the ASP.NET Web Form Designer.
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
            m_sMODULE = "Products";
            SetMenu(m_sMODULE);
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("NAME");
            this.AppendGridColumns(grdMain, m_sMODULE + ".ListView", arrSelectFields);
            ctlSearchView.Command = new CommandEventHandler(Page_Command);
            ctlExportHeader.Command = new CommandEventHandler(Page_Command);
            ctlMassUpdate.Command = new CommandEventHandler(Page_Command);
            if (CRMSecurity.GetUserAccess(m_sMODULE, "delete") < 0 && CRMSecurity.GetUserAccess(m_sMODULE, "edit") < 0)
                ctlMassUpdate.Visible = false;
        }

        #endregion
    }
}