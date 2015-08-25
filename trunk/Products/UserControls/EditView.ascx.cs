using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.Controls;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Products
{
    /// <summary>
    ///		Summary description for EditView.
    /// </summary>
    public partial class EditView : CRMControl
    {
        protected Guid gID;

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            Guid gPARENT_ID = CommonTypeConvert.ToGuid(Request["PARENT_ID"]);
            string sMODULE = String.Empty;
            string sPARENT_TYPE = String.Empty;
            string sPARENT_NAME = String.Empty;
            CommonProcedure.ParentGet(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
            if (e.CommandName == "Save")
            {
                var txtNAME = FindControl("NAME") as TextBox;
                var txtPRODUCT_TEMPLATE_ID = FindControl("PRODUCT_TEMPLATE_ID") as HtmlInputHidden;
                if (CommonTypeConvert.IsEmptyString(txtNAME.Text.Trim()))
                    txtPRODUCT_TEMPLATE_ID.Value = String.Empty;
                else if (CommonTypeConvert.IsEmptyString(txtPRODUCT_TEMPLATE_ID.Value))
                    txtPRODUCT_TEMPLATE_ID.Value = Guid.Empty.ToString();

                ValidateEditViewFields(m_sMODULE + ".EditView");
                ValidateEditViewFields(m_sMODULE + ".CostView");
                ValidateEditViewFields(m_sMODULE + ".MftView");
                if (Page.IsValid)
                {
                    string sCUSTOM_MODULE = "PRODUCTS";
                    DataTable dtCustomFields = CRMCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
                    var oQuery = new InlineQueryDBManager();
                    DataRow rowCurrent = null;
                    if (!CommonTypeConvert.IsEmptyGuid(gID))
                    {
                        string innerSql = ApplicationSQL.SQL["Products_EditView"].ToString();
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

                    CommonProcedure.ProductUpdate
                        (ref gID
                         , new DynamicControl(this, rowCurrent, "PRODUCT_TEMPLATE_ID").ID
                         , new DynamicControl(this, rowCurrent, "NAME").Text
                         , new DynamicControl(this, rowCurrent, "STATUS").SelectedValue
                         , new DynamicControl(this, rowCurrent, "ACCOUNT_ID").ID
                         , new DynamicControl(this, rowCurrent, "CONTACT_ID").ID
                         , new DynamicControl(this, rowCurrent, "QUANTITY").IntegerValue
                         , new DynamicControl(this, rowCurrent, "DATE_PURCHASED").DateValue
                         , new DynamicControl(this, rowCurrent, "DATE_SUPPORT_EXPIRES").DateValue
                         , new DynamicControl(this, rowCurrent, "DATE_SUPPORT_STARTS").DateValue
                         , new DynamicControl(this, rowCurrent, "MANUFACTURER_ID").ID
                         , new DynamicControl(this, rowCurrent, "CATEGORY_ID").ID
                         , new DynamicControl(this, rowCurrent, "TYPE_ID").ID
                         , new DynamicControl(this, rowCurrent, "WEBSITE").Text
                         , new DynamicControl(this, rowCurrent, "MFT_PART_NUM").Text
                         , new DynamicControl(this, rowCurrent, "VENDOR_PART_NUM").Text
                         , new DynamicControl(this, rowCurrent, "SERIAL_NUMBER").Text
                         , new DynamicControl(this, rowCurrent, "ASSET_NUMBER").Text
                         , new DynamicControl(this, rowCurrent, "TAX_CLASS").SelectedValue
                         , new DynamicControl(this, rowCurrent, "WEIGHT").FloatValue
                         , new DynamicControl(this, rowCurrent, "CURRENCY_ID").ID
                         , new DynamicControl(this, rowCurrent, "COST_PRICE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "LIST_PRICE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "BOOK_VALUE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "BOOK_VALUE_DATE").DateValue
                         , new DynamicControl(this, rowCurrent, "DISCOUNT_PRICE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "PRICING_FACTOR").IntegerValue
                         , new DynamicControl(this, rowCurrent, "PRICING_FORMULA").SelectedValue
                         , new DynamicControl(this, rowCurrent, "SUPPORT_NAME").Text
                         , new DynamicControl(this, rowCurrent, "SUPPORT_CONTACT").Text
                         , new DynamicControl(this, rowCurrent, "SUPPORT_DESCRIPTION").Text
                         , new DynamicControl(this, rowCurrent, "SUPPORT_TERM").SelectedValue
                         , new DynamicControl(this, rowCurrent, "DESCRIPTION").Text
                         , new DynamicControl(this, rowCurrent, "TEAM_ID").ID
                        );
                    CRMDynamic.UpdateCustomFields(this, gID, sCUSTOM_MODULE, dtCustomFields);

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
                else if (CommonTypeConvert.IsEmptyGuid(gID))
                    Response.Redirect("Index.aspx");
                else
                    Response.Redirect("view.aspx?ID=" + gID);
            }
        }

        protected void btnProductChanged_Clicked(object sender, EventArgs e)
        {
            var oQuery = new InlineQueryDBManager();
            string innerSql = ApplicationSQL.SQL["Products_EditView_161"].ToString();
            oQuery.CommandText = innerSql;
            Guid gPRODUCT_TEMPLATE_ID = new DynamicControl(this, "PRODUCT_TEMPLATE_ID").ID;
            TypeConvert.AppendParameter(oQuery, gPRODUCT_TEMPLATE_ID, "ID", false);
            using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (rdr.Read())
                {
                    ctlModuleHeader.Title = CommonTypeConvert.ToString(rdr["NAME"]);
                    SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE) + " - " +
                                 ctlModuleHeader.Title);
                    Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
                    ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;

                    CRMDynamic.SetEditViewFields(this, m_sMODULE + ".EditView", rdr);
                    CRMDynamic.SetEditViewFields(this, m_sMODULE + ".CostView", rdr);
                    CRMDynamic.SetEditViewFields(this, m_sMODULE + ".MftView", rdr);
                    var txtNAME = FindControl("NAME") as TextBox;
                    if (txtNAME != null)
                        txtNAME.ReadOnly = false;
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
                Guid gDuplicateID = CommonTypeConvert.ToGuid(Request["DuplicateID"]);
                if (!CommonTypeConvert.IsEmptyGuid(gID) || !CommonTypeConvert.IsEmptyGuid(gDuplicateID))
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["Products_EditView"].ToString();
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
                            ctlModuleHeader.Title = CommonTypeConvert.ToString(rdr["NAME"]);
                            SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE) + " - " +
                                         ctlModuleHeader.Title);
                            Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
                            ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;

                            AppendEditViewFields(m_sMODULE + ".EditView", tblMain, rdr);
                            AppendEditViewFields(m_sMODULE + ".CostView", tblCost, rdr);
                            AppendEditViewFields(m_sMODULE + ".MftView", tblManufacturer, rdr);
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView",
                                                            CommonTypeConvert.ToGuid(rdr["ASSIGNED_USER_ID"]), rdr);
                            var txtNAME = FindControl("NAME") as TextBox;
                            if (txtNAME != null)
                                txtNAME.ReadOnly = false;
                        }
                        else
                        {
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
                            ctlDynamicButtons.DisableAll();
                            ctlDynamicButtons.ErrorText = Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS");
                        }
                    }
                }
                else
                {
                    AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
                    AppendEditViewFields(m_sMODULE + ".CostView", tblCost, null);
                    AppendEditViewFields(m_sMODULE + ".MftView", tblManufacturer, null);
                    ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
                    var txtNAME = FindControl("NAME") as TextBox;
                    if (txtNAME != null)
                        txtNAME.ReadOnly = false;
                    Guid gPARENT_ID = CommonTypeConvert.ToGuid(Request["PARENT_ID"]);
                    if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID))
                    {
                        string sMODULE = String.Empty;
                        string sPARENT_TYPE = String.Empty;
                        string sPARENT_NAME = String.Empty;
                        CommonProcedure.ParentGet(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
                        if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID) && sMODULE == "Accounts")
                        {
                            new DynamicControl(this, "ACCOUNT_ID").ID = gPARENT_ID;
                            new DynamicControl(this, "ACCOUNT_NAME").Text = sPARENT_NAME;
                        }
                        else if (!CommonTypeConvert.IsEmptyGuid(gPARENT_ID) && sMODULE == "Contacts")
                        {
                            new DynamicControl(this, "CONTACT_ID").ID = gPARENT_ID;
                            new DynamicControl(this, "CONTACT_NAME").Text = sPARENT_NAME;
                        }
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
            m_sMODULE = "Products";
            SetMenu(m_sMODULE);
            if (IsPostBack)
            {
                this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
                this.AppendEditViewFields(m_sMODULE + ".CostView", tblCost, null);
                this.AppendEditViewFields(m_sMODULE + ".MftView", tblManufacturer, null);
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
            }
        }

        #endregion
    }
}