using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.Payments
{
    /// <summary>
    ///		Summary description for Invoices.
    /// </summary>
    public partial class Invoices : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Invoices.Edit":
                    {
                        Guid gINVOICE_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        Response.Redirect("~/CRM/Invoices/edit.aspx?ID=" + gINVOICE_ID);
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
                              + "  from vwPAYMENTS_INVOICES" + ControlChars.CrLf
                              + " where 1 = 1              " + ControlChars.CrLf;

            oQuery.CommandText = innerSql;
            TypeConvert.AppendParameter(oQuery, gID, "PAYMENT_ID");
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
                ctlDynamicButtons.AppendButtons("Payments." + m_sMODULE, gASSIGNED_USER_ID, gID);
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
            m_sMODULE = "Invoices";
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("DATE_ENTERED");
            arrSelectFields.Add("INVOICE_ID");
            arrSelectFields.Add("ASSIGNED_USER_ID");
            this.AppendGridColumns(grdMain, "Payments." + m_sMODULE, arrSelectFields);
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons("Payments." + m_sMODULE, Guid.Empty, Guid.Empty);
        }

        #endregion
    }
}