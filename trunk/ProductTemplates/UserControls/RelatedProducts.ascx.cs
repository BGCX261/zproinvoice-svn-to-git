using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.ProductTemplates
{
    /// <summary>
    ///		Summary description for RelatedProductTemplates.
    /// </summary>
    public partial class RelatedProductTemplates : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
        protected Label lblError;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ProductTemplates.Edit":
                    {
                        Guid gPRODUCT_TEMPLATE_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        Response.Redirect("~/CRM/ProductTemplates/edit.aspx?ID=" + gPRODUCT_TEMPLATE_ID);
                        break;
                    }
                case "ProductTemplates.Remove":
                    {
                        Guid gPRODUCT_TEMPLATE_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        BindGrid();
                        break;
                    }
                default:
                    break;
            }
        }

        protected void BindGrid()
        {
            var oQuery = new InlineQueryDBManager();
            string innerSql = "select " + CommonTypeConvert.FormatSelectFields(arrSelectFields)
                              + "  from vwPRODUCT_TEMPLATES_PRODUCT_TEMPLATES" + ControlChars.CrLf
                              + " where 1 = 1                                " + ControlChars.CrLf;
            oQuery.CommandText = innerSql;
            TypeConvert.AppendParameter(oQuery, gID, "PRODUCT_TEMPLATE_ID");
            oQuery.CommandText += grdMain.OrderByClause("DATE_ENTERED", "desc");
            DataTable dt = oQuery.GetTable();
            vwMain = dt.DefaultView;
            grdMain.DataSource = vwMain;
            grdMain.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            Guid gPRODUCT_TEMPLATE_ID = CommonTypeConvert.ToGuid(txtPRODUCT_TEMPLATE_ID.Value);
            if (!CommonTypeConvert.IsEmptyGuid(gPRODUCT_TEMPLATE_ID))
            {
                txtPRODUCT_TEMPLATE_ID.Value = String.Empty;
            }
            BindGrid();

            if (!IsPostBack)
            {
                ctlDynamicButtons.AppendButtons("ProductTemplates.RelatedProducts", Guid.Empty, gID);
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
            m_sMODULE = "ProductTemplates";
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("DATE_ENTERED");
            arrSelectFields.Add("PRODUCT_TEMPLATE_ID");
            this.AppendGridColumns(grdMain, "ProductTemplates.RelatedProducts", arrSelectFields);
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons("ProductTemplates.RelatedProducts", Guid.Empty, Guid.Empty);
        }

        #endregion
    }
}