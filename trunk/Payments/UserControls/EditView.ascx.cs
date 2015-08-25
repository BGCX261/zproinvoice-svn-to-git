using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.Controls;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Payments
{
    /// <summary>
    ///		Summary description for EditView.
    /// </summary>
    public partial class EditView : CRMControl
    {
        protected Guid gID;

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save" || e.CommandName == "Charge")
            {
                ValidateEditViewFields(m_sMODULE + ".EditView");
                bool bIsValid = Page.IsValid;

                Decimal dAMOUNT = new DynamicControl(this, "AMOUNT").DecimalValue;
                if (dAMOUNT != ctlAllocationsView.ALLOCATED_TOTAL)
                {
                    ctlDynamicButtons.ErrorText =
                        Translation.GetTranslation.Term("Payments.ERR_AMOUNT_MUST_MATCH_ALLOCATION");
                    bIsValid = false;
                }
                if (bIsValid)
                {
                    string sCUSTOM_MODULE = "PAYMENTS";
                    DataTable dtCustomFields = CRMCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
                    var oQuery = new InlineQueryDBManager();
                    DataRow rowCurrent = null;
                    if (!CommonTypeConvert.IsEmptyGuid(gID))
                    {
                        string innerSql = ApplicationSQL.SQL["Payments_EditView_259"].ToString();
                        oQuery.CommandText = innerSql;
                        CRMSecurity.Filter(oQuery, m_sMODULE, "edit");
                        TypeConvert.AppendParameter(oQuery, gID, "ID", false);
                        DataTable dtCurrent = oQuery.GetTable();
                        if (dtCurrent.Rows.Count > 0)
                        {
                            rowCurrent = dtCurrent.Rows[0];
                        }
                        else
                        {
                            gID = Guid.Empty;
                        }
                    }

                    Guid gASSIGNED_USER_ID = new DynamicControl(this, rowCurrent, "ASSIGNED_USER_ID").ID;
                    Guid gACCOUNT_ID = new DynamicControl(this, rowCurrent, "ACCOUNT_ID").ID;
                    DateTime dtPAYMENT_DATE = new DynamicControl(this, rowCurrent, "PAYMENT_DATE").DateValue;
                    string sPAYMENT_TYPE = new DynamicControl(this, rowCurrent, "PAYMENT_TYPE").SelectedValue;
                    string sCUSTOMER_REFERENCE = new DynamicControl(this, rowCurrent, "CUSTOMER_REFERENCE").Text;
                    Guid gCURRENCY_ID = new DynamicControl(this, rowCurrent, "CURRENCY_ID").ID;
                    string sDESCRIPTION = new DynamicControl(this, rowCurrent, "DESCRIPTION").Text;
                    Guid gTEAM_ID = new DynamicControl(this, rowCurrent, "TEAM_ID").ID;
                    Guid gCREDIT_CARD_ID = new DynamicControl(this, rowCurrent, "CREDIT_CARD_ID").ID;
                    if (sPAYMENT_TYPE != "Credit Card")
                        gCREDIT_CARD_ID = Guid.Empty;
                    float fEXCHANGE_RATE =
                        new DynamicControl(ctlAllocationsView, rowCurrent, "EXCHANGE_RATE").FloatValue;
                    var sbINVOICE_NUMBER = new StringBuilder();
                    if (dtPAYMENT_DATE == ((DateTime) SqlDateTime.MinValue) || e.CommandName == "Charge")
                        dtPAYMENT_DATE = DateTime.Now;

                    CommonProcedure.PatmentsUpdate
                        (ref gID
                         , gASSIGNED_USER_ID
                         , gACCOUNT_ID
                         , dtPAYMENT_DATE
                         , sPAYMENT_TYPE
                         , sCUSTOMER_REFERENCE
                         , fEXCHANGE_RATE
                         , gCURRENCY_ID
                         , dAMOUNT
                         , sDESCRIPTION
                         , gTEAM_ID
                         , gCREDIT_CARD_ID
                        );
                    CRMDynamic.UpdateCustomFields(this, gID, sCUSTOM_MODULE, dtCustomFields);

                    DataTable dtLineItems = ctlAllocationsView.LineItems;
                    foreach (DataRow row in dtLineItems.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted)
                        {
                            Guid gITEM_ID = CommonTypeConvert.ToGuid(row["ID", DataRowVersion.Original]);
                            if (!CommonTypeConvert.IsEmptyGuid(gITEM_ID))
                                CommonProcedure.InvoicesPAYMENTS_Delete(gITEM_ID);
                        }
                    }
                    foreach (DataRow row in dtLineItems.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            Guid gITEM_ID = CommonTypeConvert.ToGuid(row["ID"]);
                            Guid gINVOICE_ID = CommonTypeConvert.ToGuid(row["INVOICE_ID"]);
                            Decimal dINVOICE_AMOUNT = CommonTypeConvert.ToDecimal(row["AMOUNT"]);
                            if (!CommonTypeConvert.IsEmptyGuid(gINVOICE_ID))
                            {
                                CommonProcedure.InvoicesPAYMENTS_Update
                                    (ref gITEM_ID
                                     , gINVOICE_ID
                                     , gID
                                     , dINVOICE_AMOUNT
                                    );
                                if (sbINVOICE_NUMBER.Length > 0)
                                    sbINVOICE_NUMBER.Append(",");
                                sbINVOICE_NUMBER.Append(gINVOICE_ID.ToString());
                            }
                        }
                    }

                    ViewState["ID"] = gID;


                    if (e.CommandName == "Charge")
                    {
                        sbINVOICE_NUMBER.Append(" " + DateTime.UtcNow);
                        //CRM.Common.Charge.CC.Charge(Application, gID, gCURRENCY_ID, gACCOUNT_ID, gCREDIT_CARD_ID, Request.UserHostAddress, sbINVOICE_NUMBER.ToString(), sDESCRIPTION, dAMOUNT);
                    }

                    Response.Redirect("view.aspx?ID=" + gID);
                }
            }
            else if (e.CommandName == "Cancel")
            {
                if (CommonTypeConvert.IsEmptyGuid(gID))
                    Response.Redirect("Index.aspx");
                else
                    Response.Redirect("view.aspx?ID=" + gID);
            }
        }

        protected void CURRENCY_ID_Changed(object sender, EventArgs e)
        {
            ctlAllocationsView.CURRENCY_ID_Changed(sender, e);
        }

        protected void PAYMENT_TYPE_Changed(object sender, EventArgs e)
        {
            var PAYMENT_TYPE = tblMain.FindControl("PAYMENT_TYPE") as DropDownList;
            if (PAYMENT_TYPE != null)
            {
                bool bCreditCard = PAYMENT_TYPE.SelectedValue == "Credit Card";
                ctlDynamicButtons.ShowButton("Charge", bCreditCard);
                new DynamicControl(this, "CREDIT_CARD_ID_LABEL").Visible = bCreditCard;
                new DynamicControl(this, "CREDIT_CARD_NAME").Visible = bCreditCard;
                new DynamicControl(this, "CREDIT_CARD_ID_btnChange").Visible = bCreditCard;
                new DynamicControl(this, "CREDIT_CARD_ID_btnClear").Visible = bCreditCard;
                if (bCreditCard && CommonTypeConvert.IsEmptyGuid(new DynamicControl(this, "CREDIT_CARD_ID").ID))
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["Payments_EditView_214"].ToString();
                    oQuery.CommandText = innerSql;
                    Guid gACCOUNT_ID = new DynamicControl(this, "ACCOUNT_ID").ID;
                    TypeConvert.AppendParameter(oQuery, gACCOUNT_ID, "ACCOUNT_ID", false);
                    using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            new DynamicControl(this, "CREDIT_CARD_ID").ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                            new DynamicControl(this, "CREDIT_CARD_NAME").Text = CommonTypeConvert.ToString(rdr["NAME"]);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE));
            Visible = (Security.GetUserAccess(m_sMODULE, "edit") >= 0);
            if (!Visible)
                return;

            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            if (CommonTypeConvert.IsEmptyGuid(gID))
                gID = CommonTypeConvert.ToGuid(ViewState["ID"]);
            if (!IsPostBack)
            {
                Guid gDuplicateID = CommonTypeConvert.ToGuid(Request["DuplicateID"]);
                if (!CommonTypeConvert.IsEmptyGuid(gID) || !CommonTypeConvert.IsEmptyGuid(gDuplicateID))
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["Payments_EditView_259"].ToString();
                    oQuery.CommandText = innerSql;
                    CRMSecurity.Filter(oQuery, m_sMODULE, "edit");
                    if (!CommonTypeConvert.IsEmptyGuid(gDuplicateID))
                    {
                        TypeConvert.AppendParameter(oQuery, gDuplicateID, "ID", false);
                        gID = Guid.Empty;
                    }
                    else
                    {
                        TypeConvert.AppendParameter(oQuery, gID, "ID", false);
                    }
                    using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            ctlModuleHeader.Title = CommonTypeConvert.ToString(rdr["PAYMENT_NUM"]);
                            SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE) + " - " +
                                         ctlModuleHeader.Title);
                            Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
                            ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;

                            AppendEditViewFields(m_sMODULE + ".EditView", tblMain, rdr);
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView",
                                                            CommonTypeConvert.ToGuid(rdr["ASSIGNED_USER_ID"]), rdr);

                            var CURRENCY_ID = tblMain.FindControl("CURRENCY_ID") as DropDownList;
                            if (CURRENCY_ID != null)
                            {
                                CURRENCY_ID.AutoPostBack = true;
                                CURRENCY_ID.SelectedIndexChanged += CURRENCY_ID_Changed;
                            }
                            var PAYMENT_TYPE = tblMain.FindControl("PAYMENT_TYPE") as DropDownList;
                            if (PAYMENT_TYPE != null)
                            {
                                PAYMENT_TYPE.AutoPostBack = true;
                                PAYMENT_TYPE.SelectedIndexChanged += PAYMENT_TYPE_Changed;
                                PAYMENT_TYPE_Changed(null, null);
                            }
                            Guid gACCOUNT_ID = CommonTypeConvert.ToGuid(rdr["ACCOUNT_ID"]);
                            ctlAllocationsView.LoadLineItems(gID, gACCOUNT_ID, gDuplicateID, oQuery, rdr);
                        }
                        else
                        {
                            Guid gPARENT_ID = CommonTypeConvert.ToGuid(Request["PARENT_ID"]);
                            ctlAllocationsView.LoadLineItems(gID, gPARENT_ID, gDuplicateID, oQuery, null);
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
                            ctlDynamicButtons.DisableAll();
                            ctlDynamicButtons.ErrorText = Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS");
                        }
                    }
                }
                else
                {
                    AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
                    ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);

                    var CURRENCY_ID = tblMain.FindControl("CURRENCY_ID") as DropDownList;
                    if (CURRENCY_ID != null)
                    {
                        CURRENCY_ID.AutoPostBack = true;
                        CURRENCY_ID.SelectedIndexChanged += CURRENCY_ID_Changed;
                    }
                    Guid gACCOUNT_ID = Guid.Empty;
                    Guid gPARENT_ID = CommonTypeConvert.ToGuid(Request["PARENT_ID"]);
                    new DynamicControl(this, "PAYMENT_DATE").DateValue = DateTime.Today;
                    Guid gINVOICE_ID = gPARENT_ID;
                    if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID))
                    {
                        string sMODULE = String.Empty;
                        string sPARENT_TYPE = String.Empty;
                        string sPARENT_NAME = String.Empty;
                        CommonProcedure.ParentGet(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
                        if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID) && sMODULE == "Accounts")
                        {
                            gACCOUNT_ID = gPARENT_ID;
                            new DynamicControl(this, "ACCOUNT_ID").ID = gPARENT_ID;
                            new DynamicControl(this, "ACCOUNT_NAME").Text = sPARENT_NAME;
                            var oQuery = new InlineQueryDBManager();
                            ctlAllocationsView.LoadLineItems(gID, gACCOUNT_ID, gDuplicateID, oQuery, null);
                        }
                        else if (!CommonTypeConvert.IsEmptyGuid(gINVOICE_ID))
                        {
                            var oQuery = new InlineQueryDBManager();
                            string innerSql = ApplicationSQL.SQL["Payments_EditView"].ToString();
                            oQuery.CommandText = innerSql;
                            CRMSecurity.Filter(oQuery, "Invoices", "edit");
                            TypeConvert.AppendParameter(oQuery, gINVOICE_ID, "ID", false);
                            using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                if (rdr.Read())
                                {
                                    gACCOUNT_ID = CommonTypeConvert.ToGuid(rdr["BILLING_ACCOUNT_ID"]);
                                    new DynamicControl(this, "ACCOUNT_ID").ID =
                                        CommonTypeConvert.ToGuid(rdr["BILLING_ACCOUNT_ID"]);
                                    new DynamicControl(this, "ACCOUNT_NAME").Text =
                                        CommonTypeConvert.ToString(rdr["BILLING_ACCOUNT_NAME"]);

                                    ctlAllocationsView.LoadLineItems(gID, gACCOUNT_ID, gDuplicateID, oQuery, null);
                                    new DynamicControl(this, "AMOUNT").Text =
                                        Currency.GetCurrency.ToCurrency(
                                            CommonTypeConvert.ToDecimal(rdr["AMOUNT_DUE_USDOLLAR"])).ToString("0.00");
                                }
                            }
                        }
                    }
                    var PAYMENT_TYPE = tblMain.FindControl("PAYMENT_TYPE") as DropDownList;
                    if (PAYMENT_TYPE != null)
                    {
                        PAYMENT_TYPE.AutoPostBack = true;
                        PAYMENT_TYPE.SelectedIndexChanged += PAYMENT_TYPE_Changed;
                        PAYMENT_TYPE_Changed(null, null);
                    }
                }
            }
            else
            {
                ctlModuleHeader.Title = CommonTypeConvert.ToString(ViewState["ctlModuleHeader.Title"]);
                SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
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
            SetMenu("Invoices");
            if (IsPostBack)
            {
                this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);

                var CURRENCY_ID = tblMain.FindControl("CURRENCY_ID") as DropDownList;
                if (CURRENCY_ID != null)
                {
                    CURRENCY_ID.AutoPostBack = true;
                    CURRENCY_ID.SelectedIndexChanged += new EventHandler(CURRENCY_ID_Changed);
                }
                var PAYMENT_TYPE = tblMain.FindControl("PAYMENT_TYPE") as DropDownList;
                if (PAYMENT_TYPE != null)
                {
                    PAYMENT_TYPE.AutoPostBack = true;
                    PAYMENT_TYPE.SelectedIndexChanged += new EventHandler(PAYMENT_TYPE_Changed);
                }
            }
        }

        #endregion
    }
}