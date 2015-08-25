using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.Controls;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.ProductTemplates
{
    /// <summary>
    ///		Summary description for EditView.
    /// </summary>
    public partial class EditView : CRMControl
    {
        protected Guid gID;
        protected HtmlTable tblAddress;
        protected HtmlTable tblDescription;

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                ValidateEditViewFields(m_sMODULE + ".EditView");
                if (Page.IsValid)
                {
                    string sCUSTOM_MODULE = "PRODUCT_TEMPLATES";
                    DataTable dtCustomFields = CRMCache.FieldsMetaData_Validated(sCUSTOM_MODULE);

                    var oQuery = new InlineQueryDBManager();

                    DataRow rowCurrent = null;
                    var dtCurrent = new DataTable();
                    if (!CommonTypeConvert.IsEmptyGuid(gID))
                    {
                        string innerSql = ApplicationSQL.SQL["Administration_ProductTemplates_EditView_39"].ToString();
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


                    CommonProcedure.ProductTemplatesUpdate
                        (ref gID
                         , new DynamicControl(this, rowCurrent, "NAME").Text
                         , new DynamicControl(this, rowCurrent, "STATUS").SelectedValue
                         , new DynamicControl(this, rowCurrent, "QUANTITY").IntegerValue
                         , new DynamicControl(this, rowCurrent, "DATE_AVAILABLE").DateValue
                         , new DynamicControl(this, rowCurrent, "DATE_COST_PRICE").DateValue
                         , new DynamicControl(this, rowCurrent, "ACCOUNT_ID").ID
                         , new DynamicControl(this, rowCurrent, "MANUFACTURER_ID").ID
                         , new DynamicControl(this, rowCurrent, "CATEGORY_ID").ID
                         , new DynamicControl(this, rowCurrent, "TYPE_ID").ID
                         , new DynamicControl(this, rowCurrent, "WEBSITE").Text
                         , new DynamicControl(this, rowCurrent, "MFT_PART_NUM").Text
                         , new DynamicControl(this, rowCurrent, "VENDOR_PART_NUM").Text
                         , new DynamicControl(this, rowCurrent, "TAX_CLASS").SelectedValue
                         , new DynamicControl(this, rowCurrent, "WEIGHT").FloatValue
                         , new DynamicControl(this, rowCurrent, "CURRENCY_ID").ID
                         , new DynamicControl(this, rowCurrent, "COST_PRICE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "LIST_PRICE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "DISCOUNT_PRICE").DecimalValue
                         , new DynamicControl(this, rowCurrent, "PRICING_FACTOR").IntegerValue
                         , new DynamicControl(this, rowCurrent, "PRICING_FORMULA").SelectedValue
                         , new DynamicControl(this, rowCurrent, "SUPPORT_NAME").Text
                         , new DynamicControl(this, rowCurrent, "SUPPORT_CONTACT").Text
                         , new DynamicControl(this, rowCurrent, "SUPPORT_DESCRIPTION").Text
                         , new DynamicControl(this, rowCurrent, "SUPPORT_TERM").SelectedValue
                         , new DynamicControl(this, rowCurrent, "DESCRIPTION").Text
                        );
                    CRMDynamic.UpdateCustomFields(this, gID, sCUSTOM_MODULE, dtCustomFields);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE));
            Visible = (Security.GetUserAccess(m_sMODULE, "edit") >= 0);
            if (!Visible)
                return;

            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            if (!IsPostBack)
            {
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);

                Guid gDuplicateID = CommonTypeConvert.ToGuid(Request["DuplicateID"]);
                if (!CommonTypeConvert.IsEmptyGuid(gID) || !CommonTypeConvert.IsEmptyGuid(gDuplicateID))
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["Administration_ProductTemplates_EditView"].ToString();
                    oQuery.CommandText = innerSql;
                    if (!CommonTypeConvert.IsEmptyGuid(gDuplicateID))
                    {
                        oQuery.Add("@ID", SqlDbType.UniqueIdentifier, gDuplicateID);
                        gID = Guid.Empty;
                    }
                    else
                    {
                        oQuery.Add("@ID", SqlDbType.UniqueIdentifier, gID);
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
                        }
                    }
                }
                else
                {
                    AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
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
            m_sMODULE = "ProductTemplates";
            SetMenu(m_sMODULE);
            if (IsPostBack)
            {
                this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
            }
        }

        #endregion
    }
}