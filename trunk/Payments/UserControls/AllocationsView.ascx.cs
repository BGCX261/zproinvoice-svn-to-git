using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.Controls;

namespace CRM.InvoiceManagement.Payments
{
    /// <summary>
    ///		Summary description for AllocationsView.
    /// </summary>
    public partial class AllocationsView : CRMControl
    {
        protected DropDownList CURRENCY_ID;
        protected DataTable dtLineItems;
        protected Guid gACCOUNT_ID;

        public Decimal ALLOCATED_TOTAL
        {
            get { return CommonTypeConvert.ToDecimal(ALLOCATED.Text); }
        }

        public DataTable LineItems
        {
            get { return dtLineItems; }
        }

        #region Line Item Editing

        protected void grdMain_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void grdMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        protected void grdMain_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdMain.EditIndex = e.NewEditIndex;
            if (dtLineItems != null)
            {
                grdMain.DataSource = dtLineItems;
                grdMain.DataBind();
            }
        }

        protected void grdMain_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (dtLineItems != null)
            {
                DataRow[] aCurrentRows = dtLineItems.Select(String.Empty, String.Empty, DataViewRowState.CurrentRows);
                aCurrentRows[e.RowIndex].Delete();

                aCurrentRows = dtLineItems.Select(String.Empty, String.Empty, DataViewRowState.CurrentRows);
                if (aCurrentRows.Length == 0 ||
                    !CommonTypeConvert.IsEmptyGuid(aCurrentRows[aCurrentRows.Length - 1]["INVOICE_ID"]))
                {
                    DataRow rowNew = dtLineItems.NewRow();
                    dtLineItems.Rows.Add(rowNew);
                    aCurrentRows = dtLineItems.Select(String.Empty, String.Empty, DataViewRowState.CurrentRows);
                }
                UpdateTotals();

                ViewState["LineItems"] = dtLineItems;
                grdMain.DataSource = dtLineItems;
                grdMain.EditIndex = aCurrentRows.Length - 1;
                grdMain.DataBind();
                UpdateTotals();
                UpdatePaymentAmount();
            }
        }

        protected void grdMain_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (dtLineItems != null)
            {
                GridViewRow gr = grdMain.Rows[e.RowIndex];
                var txtINVOICE_NAME = gr.FindControl("INVOICE_NAME") as TextBox;
                var txtINVOICE_ID = gr.FindControl("INVOICE_ID") as HiddenField;
                var txtAMOUNT_DUE = gr.FindControl("AMOUNT_DUE") as TextBox;
                var txtAMOUNT_DUE_USDOLLAR = gr.FindControl("AMOUNT_DUE_USDOLLAR") as HiddenField;
                var txtAMOUNT = gr.FindControl("AMOUNT") as TextBox;
                var txtAMOUNT_USDOLLAR = gr.FindControl("AMOUNT_USDOLLAR") as HiddenField;

                DataRow row = dtLineItems.Rows[e.RowIndex];
                if (txtINVOICE_NAME != null) row["INVOICE_NAME"] = txtINVOICE_NAME.Text;
                if (txtINVOICE_ID != null) row["INVOICE_ID"] = CommonTypeConvert.ToGuid(txtINVOICE_ID.Value);
                if (txtAMOUNT != null) row["AMOUNT_DUE"] = CommonTypeConvert.ToDecimal(txtAMOUNT_DUE.Text);
                if (txtAMOUNT != null) row["AMOUNT"] = CommonTypeConvert.ToDecimal(txtAMOUNT.Text);

                row["AMOUNT_DUE_USDOLLAR"] =
                    Currency.GetCurrency.FromCurrency(CommonTypeConvert.ToDecimal(txtAMOUNT_DUE.Text));
                row["AMOUNT_USDOLLAR"] = Currency.GetCurrency.FromCurrency(CommonTypeConvert.ToDecimal(txtAMOUNT.Text));

                DataRow[] aCurrentRows = dtLineItems.Select(String.Empty, String.Empty, DataViewRowState.CurrentRows);
                if (aCurrentRows.Length == 0 ||
                    !CommonTypeConvert.IsEmptyString(aCurrentRows[aCurrentRows.Length - 1]["INVOICE_NAME"]))
                {
                    DataRow rowNew = dtLineItems.NewRow();
                    dtLineItems.Rows.Add(rowNew);
                    aCurrentRows = dtLineItems.Select(String.Empty, String.Empty, DataViewRowState.CurrentRows);
                }

                ViewState["LineItems"] = dtLineItems;
                grdMain.DataSource = dtLineItems;
                grdMain.EditIndex = aCurrentRows.Length - 1;
                grdMain.DataBind();
                UpdateTotals();
                UpdatePaymentAmount();
            }
        }

        protected void grdMain_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdMain.EditIndex = -1;
            if (dtLineItems != null)
            {
                DataRow[] aCurrentRows = dtLineItems.Select(String.Empty, String.Empty, DataViewRowState.CurrentRows);
                grdMain.DataSource = dtLineItems;
                grdMain.EditIndex = aCurrentRows.Length - 1;
                grdMain.DataBind();
                UpdateTotals();
                UpdatePaymentAmount();
            }
        }

        public void CURRENCY_ID_Changed(object sender, EventArgs e)
        {
            Guid gCURRENCY_ID = CommonTypeConvert.ToGuid(CURRENCY_ID.SelectedValue);
            SetCurrency(gCURRENCY_ID);
            EXCHANGE_RATE.Value = Currency.GetCurrency.CONVERSION_RATE.ToString();

            foreach (DataRow row in dtLineItems.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    row["AMOUNT"] = Currency.GetCurrency.ToCurrency(CommonTypeConvert.ToDecimal(row["AMOUNT_USDOLLAR"]));
                }
            }
            grdMain.DataBind();

            UpdateTotals();
            UpdatePaymentAmount();
        }

        private void UpdateTotals()
        {
            Double dALLOCATED = 0.0;

            foreach (DataRow row in dtLineItems.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Double dAMOUNT = CommonTypeConvert.ToDouble(row["AMOUNT"]);
                    dALLOCATED += dAMOUNT;
                }
            }
            ALLOCATED.Text = Convert.ToDecimal(dALLOCATED).ToString("c");
            ALLOCATED_USDOLLAR.Value = Currency.GetCurrency.FromCurrency(Convert.ToDecimal(dALLOCATED)).ToString("0.000");
        }

        private void UpdatePaymentAmount()
        {
            new DynamicControl(Parent as CRMControl, "AMOUNT").Text =
                Currency.GetCurrency.ToCurrency(CommonTypeConvert.ToDecimal(ALLOCATED_USDOLLAR.Value)).ToString("0.00");
        }

        #endregion

        public void LoadLineItems(Guid gID, Guid gACCOUNT_ID, Guid gDuplicateID, InlineQueryDBManager oQuery,
                                  SqlDataReader rdr)
        {
            this.gACCOUNT_ID = gACCOUNT_ID;
            if (!IsPostBack)
            {
                GetCurrencyControl();
                CURRENCY_ID.SelectedValue = Currency.GetCurrency.ID.ToString();
                EXCHANGE_RATE.Value = Currency.GetCurrency.CONVERSION_RATE.ToString();
                foreach (DataControlField col in grdMain.Columns)
                {
                    if (!CommonTypeConvert.IsEmptyString(col.HeaderText))
                    {
                        col.HeaderText = Translation.GetTranslation.Term(col.HeaderText);
                    }
                    var cf = col as CommandField;
                    if (cf != null)
                    {
                        cf.EditText = Translation.GetTranslation.Term(cf.EditText);
                        cf.DeleteText = Translation.GetTranslation.Term(cf.DeleteText);
                        cf.UpdateText = Translation.GetTranslation.Term(cf.UpdateText);
                        cf.CancelText = Translation.GetTranslation.Term(cf.CancelText);
                    }
                }

                if ((!CommonTypeConvert.IsEmptyGuid(gID) || !CommonTypeConvert.IsEmptyGuid(gDuplicateID)) &&
                    (oQuery != null) && (rdr != null))
                {
                    CURRENCY_ID.SelectedValue = CommonTypeConvert.ToString(rdr["CURRENCY_ID"]);
                    EXCHANGE_RATE.Value = Currency.GetCurrency.CONVERSION_RATE.ToString();
                    float fEXCHANGE_RATE = CommonTypeConvert.ToFloat(rdr["EXCHANGE_RATE"]);
                    if (fEXCHANGE_RATE == 0.0f)
                        fEXCHANGE_RATE = 1.0f;
                    EXCHANGE_RATE.Value = fEXCHANGE_RATE.ToString();
                    if (CURRENCY_ID.Items.Count > 0)
                    {
                        Guid gCURRENCY_ID = CommonTypeConvert.ToGuid(CURRENCY_ID.SelectedValue);
                        SetCurrency(gCURRENCY_ID, fEXCHANGE_RATE);
                        EXCHANGE_RATE.Value = Currency.GetCurrency.CONVERSION_RATE.ToString();
                    }
                    ALLOCATED.Text =
                        Currency.GetCurrency.ToCurrency(CommonTypeConvert.ToDecimal(rdr["TOTAL_ALLOCATED_USDOLLAR"])).
                            ToString("c");
                    ALLOCATED_USDOLLAR.Value =
                        CommonTypeConvert.ToDecimal(rdr["TOTAL_ALLOCATED_USDOLLAR"]).ToString("0.00");
                    rdr.Close();
                    string innerSql = ApplicationSQL.SQL["Payments_AllocationsView_233"].ToString();

                    oQuery.CommandText = innerSql;
                    TypeConvert.AppendParameter(oQuery, gID, "PAYMENT_ID", false);
                    oQuery.CommandText += " order by DATE_MODIFIED asc" + ControlChars.CrLf;
                    DataTable dtLineItems = oQuery.GetTable();
                    if (!CommonTypeConvert.IsEmptyGuid(gDuplicateID))
                    {
                        foreach (DataRow row in dtLineItems.Rows)
                        {
                            row["ID"] = Guid.NewGuid();
                        }
                    }
                    DataRow rowNew = dtLineItems.NewRow();
                    dtLineItems.Rows.Add(rowNew);

                    ViewState["LineItems"] = dtLineItems;
                    grdMain.DataSource = dtLineItems;
                    grdMain.EditIndex = dtLineItems.Rows.Count - 1;
                    grdMain.DataBind();
                }
                else
                {
                    dtLineItems = new DataTable();
                    var colID = new DataColumn("ID", Type.GetType("System.Guid"));
                    var colINVOICE_NAME = new DataColumn("INVOICE_NAME", Type.GetType("System.String"));
                    var colINVOICE_ID = new DataColumn("INVOICE_ID", Type.GetType("System.Guid"));
                    var colAMOUNT_DUE = new DataColumn("AMOUNT_DUE", Type.GetType("System.Decimal"));
                    var colAMOUNT_DUE_USDOLLAR = new DataColumn("AMOUNT_DUE_USDOLLAR", Type.GetType("System.Decimal"));
                    var colAMOUNT = new DataColumn("AMOUNT", Type.GetType("System.Decimal"));
                    var colAMOUNT_USDOLLAR = new DataColumn("AMOUNT_USDOLLAR", Type.GetType("System.Decimal"));
                    dtLineItems.Columns.Add(colID);
                    dtLineItems.Columns.Add(colINVOICE_NAME);
                    dtLineItems.Columns.Add(colINVOICE_ID);
                    dtLineItems.Columns.Add(colAMOUNT_DUE);
                    dtLineItems.Columns.Add(colAMOUNT_DUE_USDOLLAR);
                    dtLineItems.Columns.Add(colAMOUNT);
                    dtLineItems.Columns.Add(colAMOUNT_USDOLLAR);
                    DataRow rowNew = null;
                    string innerSql = ApplicationSQL.SQL["Payments_AllocationsView"].ToString();

                    oQuery.CommandText = innerSql;
                    Guid gPARENT_ID = CommonTypeConvert.ToGuid(Request["PARENT_ID"]);
                    TypeConvert.AppendParameter(oQuery, gPARENT_ID, "ID", false);
                    using (SqlDataReader rdrInvoice = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdrInvoice.Read())
                        {
                            rowNew = dtLineItems.NewRow();
                            rowNew["INVOICE_NAME"] = CommonTypeConvert.ToString(rdrInvoice["NAME"]);
                            rowNew["INVOICE_ID"] = CommonTypeConvert.ToGuid(rdrInvoice["ID"]);
                            rowNew["AMOUNT_DUE"] = CommonTypeConvert.ToDecimal(rdrInvoice["AMOUNT_DUE"]);
                            rowNew["AMOUNT_DUE_USDOLLAR"] =
                                CommonTypeConvert.ToDecimal(rdrInvoice["AMOUNT_DUE_USDOLLAR"]);
                            rowNew["AMOUNT"] = CommonTypeConvert.ToDecimal(rdrInvoice["AMOUNT_DUE"]);
                            rowNew["AMOUNT_USDOLLAR"] = CommonTypeConvert.ToDecimal(rdrInvoice["AMOUNT_DUE_USDOLLAR"]);
                            if (rdrInvoice["AMOUNT_DUE"] == DBNull.Value)
                            {
                                rowNew["AMOUNT_DUE"] = CommonTypeConvert.ToDecimal(rdrInvoice["TOTAL"]);
                                rowNew["AMOUNT_DUE_USDOLLAR"] = CommonTypeConvert.ToDecimal(rdrInvoice["TOTAL_USDOLLAR"]);
                                rowNew["AMOUNT"] = CommonTypeConvert.ToDecimal(rdrInvoice["TOTAL"]);
                                rowNew["AMOUNT_USDOLLAR"] = CommonTypeConvert.ToDecimal(rdrInvoice["TOTAL_USDOLLAR"]);
                            }
                            dtLineItems.Rows.Add(rowNew);
                        }
                    }
                    rowNew = dtLineItems.NewRow();
                    dtLineItems.Rows.Add(rowNew);

                    ViewState["LineItems"] = dtLineItems;
                    grdMain.DataSource = dtLineItems;
                    grdMain.EditIndex = dtLineItems.Rows.Count - 1;
                    grdMain.DataBind();

                    UpdateTotals();
                }
            }
        }

        protected DropDownList GetCurrencyControl()
        {
            if (CURRENCY_ID == null)
                CURRENCY_ID = Parent.FindControl("tblMain").FindControl("CURRENCY_ID") as DropDownList;

            return CURRENCY_ID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetCurrencyControl();
            if (IsPostBack)
            {
                if (CURRENCY_ID.Items.Count > 0)
                {
                    Guid gCURRENCY_ID = CommonTypeConvert.ToGuid(CURRENCY_ID.SelectedValue);
                    SetCurrency(gCURRENCY_ID, CommonTypeConvert.ToFloat(EXCHANGE_RATE.Value));
                    EXCHANGE_RATE.Value = Currency.GetCurrency.CONVERSION_RATE.ToString();
                }
                dtLineItems = ViewState["LineItems"] as DataTable;
                grdMain.DataSource = dtLineItems;
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
            ScriptManager mgrAjax = ScriptManager.GetCurrent(this.Page);
            if (mgrAjax != null)
            {
                var svc = new ServiceReference("~/CRM/Invoices/AutoComplete.asmx");
                var scr = new ScriptReference("~/CRM/Payments/AutoComplete.js");
                mgrAjax.Services.Add(svc);
                mgrAjax.Scripts.Add(scr);
            }
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}