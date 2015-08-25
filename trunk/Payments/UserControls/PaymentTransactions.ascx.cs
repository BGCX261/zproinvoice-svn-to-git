using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.Payments
{
    /// <summary>
    ///		Summary description for PaymentTransactions.
    /// </summary>
    public partial class PaymentTransactions : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Refund")
            {
                Guid gPAYMENTS_TRANSACTION_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["PaymentTransactions"].ToString();
                oQuery.CommandText = innerSql;
                TypeConvert.AppendParameter(oQuery, gPAYMENTS_TRANSACTION_ID, "ID", false);
                using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (rdr.Read())
                    {
                        Guid gCURRENCY_ID = CommonTypeConvert.ToGuid(rdr["CURRENCY_ID"]);
                        Guid gACCOUNT_ID = CommonTypeConvert.ToGuid(rdr["ACCOUNT_ID"]);
                        Guid gCREDIT_CARD_ID = CommonTypeConvert.ToGuid(rdr["CREDIT_CARD_ID"]);
                        string sINVOICE_NUMBER = CommonTypeConvert.ToString(rdr["INVOICE_NUMBER"]);
                        Decimal dAMOUNT = CommonTypeConvert.ToDecimal(rdr["AMOUNT"]);
                        string sDESCRIPTION = CommonTypeConvert.ToString(rdr["DESCRIPTION"]);
                        string sTRANSACTION_NUMBER = CommonTypeConvert.ToString(rdr["TRANSACTION_NUMBER"]);
                        //CRM.Common.Charge.CC.Refund(Application, gID, gCURRENCY_ID, gACCOUNT_ID, gCREDIT_CARD_ID, Request.UserHostAddress, sINVOICE_NUMBER, sDESCRIPTION, dAMOUNT, sTRANSACTION_NUMBER);
                        BindGrid();
                    }
                }
            }
        }

        private void BindGrid()
        {
            var oQuery = new InlineQueryDBManager();
            string innerSql = "select " + CommonTypeConvert.FormatSelectFields(arrSelectFields)
                              + "  from vwPAYMENTS_TRANSACTIONS" + ControlChars.CrLf
                              + " where 1 = 1                  " + ControlChars.CrLf;
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
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".PaymentTransactions", gASSIGNED_USER_ID, gID);
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
            arrSelectFields.Add("ID");
            this.AppendGridColumns(grdMain, m_sMODULE + ".PaymentTransactions", arrSelectFields);
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".PaymentTransactions", Guid.Empty, Guid.Empty);
        }

        #endregion
    }
}