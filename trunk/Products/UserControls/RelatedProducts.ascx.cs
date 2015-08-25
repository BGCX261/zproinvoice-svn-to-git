using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Products
{
    /// <summary>
    ///		Summary description for RelatedProducts.
    /// </summary>
    public partial class RelatedProducts : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gACCOUNT_ID = Guid.Empty;
        protected Guid gID;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Products.Edit":
                    {
                        Guid gPRODUCT_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        Response.Redirect("~/CRM/Products/edit.aspx?ID=" + gPRODUCT_ID);
                        break;
                    }
                case "Products.Remove":
                    {
                        Guid gCHILD_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        CommonProcedure.ProductProductDelete(gID, gCHILD_ID);
                        BindGrid();
                        break;
                    }
                default:
                    throw (new Exception("Unknown command: " + e.CommandName));
            }
        }

        protected void BindGrid()
        {
            var oQuery = new InlineQueryDBManager();
            string innerSql = ApplicationSQL.SQL["Products_relatedProducts_58"].ToString();
            oQuery.CommandText = innerSql;
            oQuery.Add("@ID", SqlDbType.UniqueIdentifier, gID);

            gACCOUNT_ID = CommonTypeConvert.ToGuid(oQuery.ExecuteScalar());

            innerSql = "select " + CommonTypeConvert.FormatSelectFields(arrSelectFields)
                       + "  from vwPRODUCTS_RELATED_PRODUCTS" + ControlChars.CrLf
                       + " where 1 = 1                      " + ControlChars.CrLf;

            oQuery.CommandText = innerSql;
            TypeConvert.AppendParameter(oQuery, gID, "PARENT_ID");
            oQuery.CommandText += grdMain.OrderByClause("DATE_ENTERED", "desc");
            DataTable dt = oQuery.GetTable();
            vwMain = dt.DefaultView;
            grdMain.DataSource = vwMain;
            grdMain.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            Guid gCHILD_ID = CommonTypeConvert.ToGuid(txtCHILD_ID.Value);
            if (!CommonTypeConvert.IsEmptyGuid(gCHILD_ID))
            {
                CommonProcedure.ProductProductUpdate(gID, gCHILD_ID);
                txtCHILD_ID.Value = String.Empty;
            }
            BindGrid();

            if (!IsPostBack)
            {
                Guid gASSIGNED_USER_ID = CommonTypeConvert.ToGuid(Page.Items["ASSIGNED_USER_ID"]);
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".RelatedProducts", gASSIGNED_USER_ID, gID);
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
            ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "Products";
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("DATE_ENTERED");
            this.AppendGridColumns(grdMain, m_sMODULE + ".RelatedProducts", arrSelectFields);
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".RelatedProducts", Guid.Empty, Guid.Empty);
        }

        #endregion
    }
}