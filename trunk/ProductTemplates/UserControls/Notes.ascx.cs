using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.ProductTemplates
{
    /// <summary>
    ///		Summary description for Notes.
    /// </summary>
    public partial class Notes : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
        protected Label lblError;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Notes.Create":
                    Response.Redirect("~/CRM/Notes/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Notes.Edit":
                    {
                        Guid gNOTE_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        Response.Redirect("~/CRM/Notes/edit.aspx?ID=" + gNOTE_ID);
                        break;
                    }
                case "Notes.Remove":
                    {
                        Guid gNOTE_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
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
                              + "  from vwPRODUCT_TEMPLATES_NOTES" + ControlChars.CrLf
                              + " where 1 = 1                    " + ControlChars.CrLf;
            oQuery.CommandText = innerSql;
            oQuery.Add("@PRODUCT_TEMPLATE_ID", SqlDbType.UniqueIdentifier, gID);
            TypeConvert.AppendParameter(oQuery, gID, "PRODUCT_TEMPLATE_ID");
            oQuery.CommandText += grdMain.OrderByClause("DATE_MODIFIED", "asc");
            DataTable dt = oQuery.GetTable();
            vwMain = dt.DefaultView;
            grdMain.DataSource = vwMain;
            grdMain.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            Guid gNOTE_ID = CommonTypeConvert.ToGuid(txtNOTE_ID.Value);
            if (!CommonTypeConvert.IsEmptyGuid(gNOTE_ID))
            {
                txtNOTE_ID.Value = String.Empty;
            }
            BindGrid();

            if (!IsPostBack)
            {
                ctlDynamicButtons.AppendButtons("ProductTemplates.Notes", Guid.Empty, gID);
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
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("DATE_MODIFIED");
            arrSelectFields.Add("NOTE_ID");
            this.AppendGridColumns(grdMain, "ProductTemplates.Notes", arrSelectFields);
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons("ProductTemplates.Notes", Guid.Empty, Guid.Empty);
        }

        #endregion
    }
}