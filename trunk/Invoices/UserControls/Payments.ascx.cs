using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Invoices
{
    /// <summary>
    ///		Summary description for Payments.
    /// </summary>
    public partial class Payments : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Payments.Create":
                    Response.Redirect("~/CRM/Payments/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Payments.Edit":
                    {
                        Guid gPAYMENT_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        Response.Redirect("~/CRM/Payments/edit.aspx?ID=" + gPAYMENT_ID);
                        break;
                    }
                case "Payments.Remove":
                    {
                        Guid gINVOICE_PAYMENT_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        CommonProcedure.InvoicesPAYMENTS_Delete(gINVOICE_PAYMENT_ID);
                        BindGrid();
                        break;
                    }
                default:
                    break;
            }
        }

        protected void BindGrid()
        {
            string innerSql = "select " + CommonTypeConvert.FormatSelectFields(arrSelectFields)
                              + "  from vwINVOICES_PAYMENTS" + ControlChars.CrLf;
            var oQuery = new InlineQueryDBManager();
            oQuery.CommandText = innerSql;
            CRMSecurity.Filter(oQuery, m_sMODULE, "list");
            TypeConvert.AppendParameter(oQuery, gID, "INVOICE_ID");
            oQuery.CommandText += grdMain.OrderByClause("DATE_ENTERED", "desc");

            using (DataTable dt = oQuery.GetTable())
            {
                vwMain = dt.DefaultView;
                grdMain.DataSource = vwMain;
                grdMain.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            BindGrid();

            if (!IsPostBack)
            {
                Guid gASSIGNED_USER_ID = CommonTypeConvert.ToGuid(Page.Items["ASSIGNED_USER_ID"]);
                ctlDynamicButtons.AppendButtons("Invoices." + m_sMODULE, gASSIGNED_USER_ID, gID);
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
            m_sMODULE = "Payments";
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("DATE_ENTERED");
            arrSelectFields.Add("PAYMENT_ID");
            arrSelectFields.Add("ASSIGNED_USER_ID");
            arrSelectFields.Add("INVOICE_PAYMENT_ID");
            arrSelectFields.Add("INVOICE_ASSIGNED_USER_ID");
            this.AppendGridColumns(grdMain, "Invoices." + m_sMODULE, arrSelectFields);
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons("Invoices." + m_sMODULE, Guid.Empty, Guid.Empty);
        }

        #endregion
    }
}