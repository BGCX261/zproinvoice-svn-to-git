using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.Controls;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Invoices
{
    /// <summary>
    ///		Summary description for EditView.
    /// </summary>
    public partial class EditView : CRMControl
    {
        protected Guid gID;
        protected HtmlTable tblSummary;

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            Guid gORDER_ID = CommonTypeConvert.ToGuid(Request["ORDER_ID"]);
            Guid gQUOTE_ID = CommonTypeConvert.ToGuid(Request["QUOTE_ID"]);
            Guid gPARENT_ID = CommonTypeConvert.ToGuid(Request["PARENT_ID"]);
            string sMODULE = String.Empty;
            string sPARENT_TYPE = String.Empty;
            string sPARENT_NAME = String.Empty;
            CommonProcedure.ParentGet(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
            if (e.CommandName == "Save")
            {
                ValidateEditViewFields(m_sMODULE + ".EditView");
                ValidateEditViewFields(m_sMODULE + ".EditAddress");
                ValidateEditViewFields(m_sMODULE + ".EditDescription");
                if (Page.IsValid)
                {
                    string sCUSTOM_MODULE = "INVOICES";
                    DataTable dtCustomFields = CRMCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
                    DataTable dtCustomLineItems =
                        CRMCache.FieldsMetaData_UnvalidatedCustomFields(sCUSTOM_MODULE + "_LINE_ITEMS");

                    DataRow rowCurrent = null;
                    var dtCurrent = new DataTable();
                    if (!CommonTypeConvert.IsEmptyGuid(gID))
                    {
                        string innerSql = ApplicationSQL.SQL["Invoices_EditView_323"].ToString();
                        var oQuery = new InlineQueryDBManager();

                        oQuery.CommandText = innerSql;
                        CRMSecurity.Filter(oQuery, m_sMODULE, "edit");
                        TypeConvert.AppendParameter(oQuery, gID, "ID", false);
                        dtCurrent = oQuery.GetTable();

                        if (dtCurrent.Rows.Count > 0)
                        {
                            rowCurrent = dtCurrent.Rows[0];
                        }
                        else
                        {
                            gID = Guid.Empty;
                        }
                    }


                    ctlEditLineItemsView.UpdateTotals();
                    CommonProcedure.InvoicesUpdate
                        (ref gID
                         , new DynamicControl(this, rowCurrent, "ASSIGNED_USER_ID").ID
                         , new DynamicControl(this, rowCurrent, "NAME").Text
                         , new DynamicControl(this, rowCurrent, "QUOTE_ID").ID
                         , new DynamicControl(this, rowCurrent, "ORDER_ID").ID
                         , new DynamicControl(this, rowCurrent, "OPPORTUNITY_ID").ID
                         , new DynamicControl(this, rowCurrent, "PAYMENT_TERMS").SelectedValue
                         , new DynamicControl(this, rowCurrent, "INVOICE_STAGE").SelectedValue
                         , new DynamicControl(this, rowCurrent, "PURCHASE_ORDER_NUM").Text
                         , new DynamicControl(this, rowCurrent, "DUE_DATE").DateValue
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "EXCHANGE_RATE").FloatValue
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "CURRENCY_ID").ID
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "TAXRATE_ID").ID
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "SHIPPER_ID").ID
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "SUBTOTAL").DecimalValue
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "DISCOUNT").DecimalValue
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "SHIPPING").DecimalValue
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "TAX").DecimalValue
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "TOTAL").DecimalValue
                         , new DynamicControl(ctlEditLineItemsView, rowCurrent, "AMOUNT_DUE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "BILLING_ACCOUNT_ID").ID
                         , new DynamicControl(this, rowCurrent, "BILLING_CONTACT_ID").ID
                         , new DynamicControl(this, rowCurrent, "BILLING_ADDRESS_STREET").Text
                         , new DynamicControl(this, rowCurrent, "BILLING_ADDRESS_CITY").Text
                         , new DynamicControl(this, rowCurrent, "BILLING_ADDRESS_STATE").Text
                         , new DynamicControl(this, rowCurrent, "BILLING_ADDRESS_POSTALCODE").Text
                         , new DynamicControl(this, rowCurrent, "BILLING_ADDRESS_COUNTRY").Text
                         , new DynamicControl(this, rowCurrent, "SHIPPING_ACCOUNT_ID").ID
                         , new DynamicControl(this, rowCurrent, "SHIPPING_CONTACT_ID").ID
                         , new DynamicControl(this, rowCurrent, "SHIPPING_ADDRESS_STREET").Text
                         , new DynamicControl(this, rowCurrent, "SHIPPING_ADDRESS_CITY").Text
                         , new DynamicControl(this, rowCurrent, "SHIPPING_ADDRESS_STATE").Text
                         , new DynamicControl(this, rowCurrent, "SHIPPING_ADDRESS_POSTALCODE").Text
                         , new DynamicControl(this, rowCurrent, "SHIPPING_ADDRESS_COUNTRY").Text
                         , new DynamicControl(this, rowCurrent, "DESCRIPTION").Text
                         , new DynamicControl(this, rowCurrent, "TEAM_ID").ID
                        );
                    CRMDynamic.UpdateCustomFields(this, gID, sCUSTOM_MODULE, dtCustomFields);

                    DataTable dtLineItems = ctlEditLineItemsView.LineItems;
                    foreach (DataRow row in dtLineItems.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted)
                        {
                            Guid gITEM_ID = CommonTypeConvert.ToGuid(row["ID", DataRowVersion.Original]);
                            if (!CommonTypeConvert.IsEmptyGuid(gITEM_ID))
                                CommonProcedure.InvoicesLINE_ITEMS_Delete(gITEM_ID);
                        }
                    }
                    int nPOSITION = 1;
                    foreach (DataRow row in dtLineItems.Rows)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            Guid gITEM_ID = CommonTypeConvert.ToGuid(row["ID"]);
                            Guid gLINE_GROUP_ID = CommonTypeConvert.ToGuid(row["LINE_GROUP_ID"]);
                            string sLINE_ITEM_TYPE = CommonTypeConvert.ToString(row["LINE_ITEM_TYPE"]);
                            string sNAME = CommonTypeConvert.ToString(row["NAME"]);
                            string sMFT_PART_NUM = CommonTypeConvert.ToString(row["MFT_PART_NUM"]);
                            string sVENDOR_PART_NUM = CommonTypeConvert.ToString(row["VENDOR_PART_NUM"]);
                            Guid gPRODUCT_TEMPLATE_ID = CommonTypeConvert.ToGuid(row["PRODUCT_TEMPLATE_ID"]);
                            string sTAX_CLASS = CommonTypeConvert.ToString(row["TAX_CLASS"]);
                            int nQUANTITY = CommonTypeConvert.ToInteger(row["QUANTITY"]);
                            Decimal dCOST_PRICE = CommonTypeConvert.ToDecimal(row["COST_PRICE"]);
                            Decimal dLIST_PRICE = CommonTypeConvert.ToDecimal(row["LIST_PRICE"]);
                            Decimal dUNIT_PRICE = CommonTypeConvert.ToDecimal(row["UNIT_PRICE"]);
                            string sDESCRIPTION = CommonTypeConvert.ToString(row["DESCRIPTION"]);
                            if (!CommonTypeConvert.IsEmptyGuid(gPRODUCT_TEMPLATE_ID) ||
                                !CommonTypeConvert.IsEmptyString(sNAME))
                            {
                                CommonProcedure.InvoicesLINE_ITEMS_Update
                                    (ref gITEM_ID
                                     , gID
                                     , gLINE_GROUP_ID
                                     , sLINE_ITEM_TYPE
                                     , nPOSITION
                                     , sNAME
                                     , sMFT_PART_NUM
                                     , sVENDOR_PART_NUM
                                     , gPRODUCT_TEMPLATE_ID
                                     , sTAX_CLASS
                                     , nQUANTITY
                                     , dCOST_PRICE
                                     , dLIST_PRICE
                                     , dUNIT_PRICE
                                     , sDESCRIPTION
                                    );
                                CRMDynamic.UpdateCustomFields(row, gITEM_ID, sCUSTOM_MODULE + "_LINE_ITEMS",
                                                              dtCustomLineItems);
                                nPOSITION++;
                            }
                        }
                    }
                    CommonProcedure.InvoicesUpdateAmountDue(gID);


                    if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID))
                        Response.Redirect("~/CRM/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID);
                    else
                        Response.Redirect("view.aspx?ID=" + gID);
                }
            }
            else if (e.CommandName == "Cancel")
            {
                if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID))
                    Response.Redirect("~/CRM/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID);
                else if (!CommonTypeConvert.IsEmptyGuid(gORDER_ID))
                    Response.Redirect("~/CRM/Orders/view.aspx?ID=" + gORDER_ID);
                else if (!CommonTypeConvert.IsEmptyGuid(gQUOTE_ID))
                    Response.Redirect("~/CRM/Quotes/view.aspx?ID=" + gQUOTE_ID);
                else if (CommonTypeConvert.IsEmptyGuid(gID))
                    Response.Redirect("Index.aspx");
                else
                    Response.Redirect("view.aspx?ID=" + gID);
            }
        }

        private void UpdateAccount(Guid gACCOUNT_ID, bool bUpdateBilling, bool bUpdateShipping)
        {
            string innerSql = ApplicationSQL.SQL["Invoices_EditView_223"].ToString();
            var oQuery = new InlineQueryDBManager();
            oQuery.CommandText = innerSql;
            oQuery.Add("@ID", SqlDbType.UniqueIdentifier, gACCOUNT_ID);
            using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (rdr.Read())
                {
                    if (bUpdateBilling)
                    {
                        new DynamicControl(this, "BILLING_ACCOUNT_ID").ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                        new DynamicControl(this, "BILLING_ACCOUNT_NAME").Text = CommonTypeConvert.ToString(rdr["NAME"]);
                        new DynamicControl(this, "BILLING_ADDRESS_STREET").Text =
                            CommonTypeConvert.ToString(rdr["BILLING_ADDRESS_STREET"]);
                        new DynamicControl(this, "BILLING_ADDRESS_CITY").Text =
                            CommonTypeConvert.ToString(rdr["BILLING_ADDRESS_CITY"]);
                        new DynamicControl(this, "BILLING_ADDRESS_STATE").Text =
                            CommonTypeConvert.ToString(rdr["BILLING_ADDRESS_STATE"]);
                        new DynamicControl(this, "BILLING_ADDRESS_POSTALCODE").Text =
                            CommonTypeConvert.ToString(rdr["BILLING_ADDRESS_POSTALCODE"]);
                        new DynamicControl(this, "BILLING_ADDRESS_COUNTRY").Text =
                            CommonTypeConvert.ToString(rdr["BILLING_ADDRESS_COUNTRY"]);
                    }
                    if (bUpdateShipping)
                    {
                        new DynamicControl(this, "SHIPPING_ACCOUNT_ID").ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                        new DynamicControl(this, "SHIPPING_ACCOUNT_NAME").Text = CommonTypeConvert.ToString(rdr["NAME"]);
                        new DynamicControl(this, "SHIPPING_ADDRESS_STREET").Text =
                            CommonTypeConvert.ToString(rdr["SHIPPING_ADDRESS_STREET"]);
                        new DynamicControl(this, "SHIPPING_ADDRESS_CITY").Text =
                            CommonTypeConvert.ToString(rdr["SHIPPING_ADDRESS_CITY"]);
                        new DynamicControl(this, "SHIPPING_ADDRESS_STATE").Text =
                            CommonTypeConvert.ToString(rdr["SHIPPING_ADDRESS_STATE"]);
                        new DynamicControl(this, "SHIPPING_ADDRESS_POSTALCODE").Text =
                            CommonTypeConvert.ToString(rdr["SHIPPING_ADDRESS_POSTALCODE"]);
                        new DynamicControl(this, "SHIPPING_ADDRESS_COUNTRY").Text =
                            CommonTypeConvert.ToString(rdr["SHIPPING_ADDRESS_COUNTRY"]);
                    }
                }
            }
        }

        private void UpdateContact(Guid gCONTACT_ID, bool bUpdateBilling, bool bUpdateShipping)
        {
            string innerSql = ApplicationSQL.SQL["Invoices_EditView_267"].ToString();
            var oQuery = new InlineQueryDBManager();
            oQuery.CommandText = innerSql;
            oQuery.Add("@ID", SqlDbType.UniqueIdentifier, gCONTACT_ID);
            using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (rdr.Read())
                {
                    if (bUpdateBilling)
                    {
                        new DynamicControl(this, "BILLING_CONTACT_ID").ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                        new DynamicControl(this, "BILLING_CONTACT_NAME").Text = CommonTypeConvert.ToString(rdr["NAME"]);
                    }
                    if (bUpdateShipping)
                    {
                        new DynamicControl(this, "SHIPPING_CONTACT_ID").ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                        new DynamicControl(this, "SHIPPING_CONTACT_NAME").Text = CommonTypeConvert.ToString(rdr["NAME"]);
                    }
                    Guid gACCOUNT_ID = CommonTypeConvert.ToGuid(rdr["ACCOUNT_ID"]);
                    if (!CommonTypeConvert.IsEmptyGuid(gACCOUNT_ID))
                    {
                        UpdateAccount(gACCOUNT_ID, bUpdateBilling, bUpdateShipping);
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
            if (!IsPostBack)
            {
                string sLOAD_MODULE = "Invoices";
                string sLOAD_MODULE_KEY = "INVOICE_ID";
                Guid gQUOTE_ID = CommonTypeConvert.ToGuid(Request["QUOTE_ID"]);
                Guid gORDER_ID = CommonTypeConvert.ToGuid(Request["ORDER_ID"]);
                Guid gDuplicateID = CommonTypeConvert.ToGuid(Request["DuplicateID"]);
                if (!CommonTypeConvert.IsEmptyGuid(gID) || !CommonTypeConvert.IsEmptyGuid(gDuplicateID) ||
                    !CommonTypeConvert.IsEmptyGuid(gQUOTE_ID) || !CommonTypeConvert.IsEmptyGuid(gORDER_ID))
                {
                    string innerSql = ApplicationSQL.SQL["Invoices_EditView_323"].ToString();
                    var oQuery = new InlineQueryDBManager();

                    oQuery.CommandText = innerSql;
                    if (!CommonTypeConvert.IsEmptyGuid(gQUOTE_ID))
                    {
                        //Load the data from the QUOTES record. 
                        sLOAD_MODULE = "Quotes";
                        sLOAD_MODULE_KEY = "QUOTE_ID";
                        innerSql = ApplicationSQL.SQL["Invoices_EditView_333"].ToString();
                        oQuery.CommandText = innerSql;
                        //Filter by the module we are loading. 
                        CRMSecurity.Filter(oQuery, sLOAD_MODULE, "edit");
                        TypeConvert.AppendParameter(oQuery, gQUOTE_ID, "ID", false);
                    }
                    else if (!CommonTypeConvert.IsEmptyGuid(gORDER_ID))
                    {
                        //  Load the data from the ORDERS record. 
                        sLOAD_MODULE = "Orders";
                        sLOAD_MODULE_KEY = "ORDER_ID";
                        innerSql = ApplicationSQL.SQL["Invoices_EditView"].ToString();
                        oQuery.CommandText = innerSql;
                        //Filter by the module we are loading. 
                        CRMSecurity.Filter(oQuery, sLOAD_MODULE, "edit");
                        TypeConvert.AppendParameter(oQuery, gORDER_ID, "ID", false);
                    }
                    else
                    {
                        //Use new CRMSecurity.Filter() function to apply Team and ACL security rules.
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
                    }

                    using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            ctlModuleHeader.Title = CommonTypeConvert.ToString(rdr["NAME"]);
                            SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE) + " - " +
                                         ctlModuleHeader.Title);
                            Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
                            ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;
                            ViewState["BILLING_ACCOUNT_ID"] = CommonTypeConvert.ToGuid(rdr["BILLING_ACCOUNT_ID"]);
                            ViewState["SHIPPING_ACCOUNT_ID"] = CommonTypeConvert.ToGuid(rdr["SHIPPING_ACCOUNT_ID"]);

                            new DynamicControl(this, "QUOTE_ID").ID = CommonTypeConvert.ToGuid(rdr["QUOTE_ID"]);
                            new DynamicControl(this, "ORDER_ID").ID = CommonTypeConvert.ToGuid(rdr["ORDER_ID"]);

                            AppendEditViewFields(m_sMODULE + ".EditView", tblMain, rdr);
                            AppendEditViewFields(m_sMODULE + ".EditAddress", tblAddress, rdr);
                            AppendEditViewFields(m_sMODULE + ".EditDescription", tblDescription, rdr);
                            //Dynamic buttons need to be recreated in order for events to fire. 
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView",
                                                            CommonTypeConvert.ToGuid(rdr["ASSIGNED_USER_ID"]), rdr);

                            if (!CommonTypeConvert.IsEmptyGuid(gQUOTE_ID))
                            {
                                new DynamicControl(this, "QUOTE_ID").ID = gQUOTE_ID;
                                new DynamicControl(this, "QUOTE_NAME").Text = CommonTypeConvert.ToString(rdr["NAME"]);
                                ctlEditLineItemsView.LoadLineItems(gQUOTE_ID, Guid.Empty, rdr, sLOAD_MODULE,
                                                                   sLOAD_MODULE_KEY);
                            }
                            else if (!CommonTypeConvert.IsEmptyGuid(gORDER_ID))
                            {
                                new DynamicControl(this, "ORDER_ID").ID = gORDER_ID;
                                new DynamicControl(this, "ORDER_NAME").Text = CommonTypeConvert.ToString(rdr["NAME"]);
                                ctlEditLineItemsView.LoadLineItems(gORDER_ID, Guid.Empty, rdr, sLOAD_MODULE,
                                                                   sLOAD_MODULE_KEY);
                            }
                            else
                            {
                                ctlEditLineItemsView.LoadLineItems(gID, gDuplicateID, rdr, sLOAD_MODULE,
                                                                   sLOAD_MODULE_KEY);
                            }
                        }
                        else
                        {
                            ctlEditLineItemsView.LoadLineItems(gID, gDuplicateID, null, String.Empty, String.Empty);

                            //If item is not visible, then don't allow save 
                            //Dynamic buttons need to be recreated in order for events to fire. 
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
                            ctlDynamicButtons.DisableAll();
                            ctlDynamicButtons.ErrorText = Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS");
                        }
                    }
                }
                else
                {
                    AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
                    AppendEditViewFields(m_sMODULE + ".EditAddress", tblAddress, null);
                    AppendEditViewFields(m_sMODULE + ".EditDescription", tblDescription, null);
                    //Dynamic buttons need to be recreated in order for events to fire. 
                    ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);

                    //Prepopulate the Account. 
                    Guid gPARENT_ID = CommonTypeConvert.ToGuid(Request["PARENT_ID"]);
                    if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID))
                    {
                        string sMODULE = String.Empty;
                        string sPARENT_TYPE = String.Empty;
                        string sPARENT_NAME = String.Empty;
                        CommonProcedure.ParentGet(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
                        if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID) && sMODULE == "Accounts")
                        {
                            UpdateAccount(gPARENT_ID, true, true);
                        }
                        if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID) && sMODULE == "Contacts")
                        {
                            UpdateContact(gPARENT_ID, true, true);
                        }
                        else if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID) && sMODULE == "Opportunities")
                        {
                            new DynamicControl(this, "OPPORTUNITY_ID").ID = gPARENT_ID;
                            new DynamicControl(this, "OPPORTUNITY_NAME").Text = sPARENT_NAME;
                        }
                    }
                    ctlEditLineItemsView.LoadLineItems(gID, gDuplicateID, new InlineQueryDBManager(), null, String.Empty,
                                                       String.Empty);
                }
            }
            else
            {
                // When validation fails, the header title does not retain its value.  Update manually. 
                ctlModuleHeader.Title = CommonTypeConvert.ToString(ViewState["ctlModuleHeader.Title"]);
                SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);

                var ctlBILLING_ACCOUNT_ID = new DynamicControl(this, "BILLING_ACCOUNT_ID");
                var ctlSHIPPING_ACCOUNT_ID = new DynamicControl(this, "SHIPPING_ACCOUNT_ID");
                if (CommonTypeConvert.ToGuid(ViewState["BILLING_ACCOUNT_ID"]) != ctlBILLING_ACCOUNT_ID.ID)
                {
                    UpdateAccount(ctlBILLING_ACCOUNT_ID.ID, true, true);
                    ViewState["BILLING_ACCOUNT_ID"] = ctlBILLING_ACCOUNT_ID.ID;
                    ViewState["SHIPPING_ACCOUNT_ID"] = ctlBILLING_ACCOUNT_ID.ID;
                }
                if (CommonTypeConvert.ToGuid(ViewState["SHIPPING_ACCOUNT_ID"]) != ctlSHIPPING_ACCOUNT_ID.ID)
                {
                    UpdateAccount(ctlSHIPPING_ACCOUNT_ID.ID, false, true);
                    ViewState["SHIPPING_ACCOUNT_ID"] = ctlSHIPPING_ACCOUNT_ID.ID;
                }
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
            SetMenu(m_sMODULE);
            if (IsPostBack)
            {
                this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
                this.AppendEditViewFields(m_sMODULE + ".EditAddress", tblAddress, null);
                this.AppendEditViewFields(m_sMODULE + ".EditDescription", tblDescription, null);
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
            }
        }

        #endregion
    }
}