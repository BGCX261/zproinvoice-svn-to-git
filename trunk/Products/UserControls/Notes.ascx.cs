using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Products
{
    /// <summary>
    ///		Summary description for Notes.
    /// </summary>
    public partial class Notes : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
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
                case "Notes.Delete":
                    {
                        Guid gNOTE_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        CommonProcedure.NotesDelete(gNOTE_ID);
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
            string innerSql = "select " + CommonTypeConvert.FormatSelectFields(arrSelectFields)
                              + "  from vwPRODUCTS_NOTES" + ControlChars.CrLf;
            oQuery.CommandText = innerSql;
            CRMSecurity.Filter(oQuery, m_sMODULE, "list");
            TypeConvert.AppendParameter(oQuery, gID, "PRODUCT_ID");
            oQuery.CommandText += grdMain.OrderByClause("DATE_ENTERED", "desc");
            DataTable dt = oQuery.GetTable();
            vwMain = dt.DefaultView;
            grdMain.DataSource = vwMain;
            grdMain.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            BindGrid();

            if (!IsPostBack)
            {
                Guid gASSIGNED_USER_ID = CommonTypeConvert.ToGuid(Page.Items["ASSIGNED_USER_ID"]);
                ctlDynamicButtons.AppendButtons("Products." + m_sMODULE, gASSIGNED_USER_ID, gID);
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
            m_sMODULE = "Notes";
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("DATE_ENTERED");
            arrSelectFields.Add("NOTE_ID");
            arrSelectFields.Add("ASSIGNED_USER_ID");
            this.AppendGridColumns(grdMain, "Products." + m_sMODULE, arrSelectFields);
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons("Products." + m_sMODULE, Guid.Empty, Guid.Empty);
        }

        #endregion
    }
}