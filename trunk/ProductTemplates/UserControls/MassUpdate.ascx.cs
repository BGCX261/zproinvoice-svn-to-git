using System;
using System.Web.UI.WebControls;
using CRM.Common;

namespace CRM.InvoiceManagement.ProductTemplates
{
    /// <summary>
    ///		Summary description for MassUpdate.
    /// </summary>
    public partial class MassUpdate : CRM.MassUpdate
    {
        public CommandEventHandler Command;

        public Guid ACCOUNT_ID
        {
            get { return CommonTypeConvert.ToGuid(txtACCOUNT_ID.Value); }
        }

        public string STATUS
        {
            get { return lstSTATUS.SelectedValue; }
        }

        public string TAX_CLASS
        {
            get { return lstTAX_CLASS.SelectedValue; }
        }

        public string SUPPORT_TERM
        {
            get { return lstSUPPORT_TERM.SelectedValue; }
        }

        public DateTime DATE_COST_PRICE
        {
            get { return ctlDATE_COST_PRICE.Value; }
        }

        public DateTime DATE_AVAILABLE
        {
            get { return ctlDATE_AVAILABLE.Value; }
        }

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            if (Command != null)
                Command(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int nACLACCESS_Delete = Security.GetUserAccess(m_sMODULE, "delete");
                int nACLACCESS_Edit = Security.GetUserAccess(m_sMODULE, "edit");
                btnDelete.Visible = (nACLACCESS_Delete >= 0);
                btnUpdate.Visible = (nACLACCESS_Edit >= 0);

                lstSTATUS.DataSource = CRMCache.List("product_status_dom");
                lstSTATUS.DataBind();
                lstSTATUS.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
                lstTAX_CLASS.DataSource = CRMCache.List("tax_class_dom");
                lstTAX_CLASS.DataBind();
                lstTAX_CLASS.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
                lstSUPPORT_TERM.DataSource = CRMCache.List("support_term_dom");
                lstSUPPORT_TERM.DataBind();
                lstSUPPORT_TERM.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
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
        protected override void InitializeComponent()
        {
            m_sMODULE = "ProductTemplates";
        }

        #endregion
    }
}