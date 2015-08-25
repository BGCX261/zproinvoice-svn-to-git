using System;
using System.Web.UI.WebControls;
using CRM.Common;

namespace CRM.InvoiceManagement.Products
{
    /// <summary>
    ///		Summary description for MassUpdate.
    /// </summary>
    public partial class MassUpdate : CRM.MassUpdate
    {
        public CommandEventHandler Command;

        public string STATUS
        {
            get { return lstSTATUS.SelectedValue; }
        }

        public DateTime DATE_PURCHASED
        {
            get { return ctlDATE_PURCHASED.Value; }
        }

        public DateTime DATE_SUPPORT_EXPIRES
        {
            get { return ctlDATE_SUPPORT_EXPIRES.Value; }
        }

        public DateTime DATE_SUPPORT_STARTS
        {
            get { return ctlDATE_SUPPORT_STARTS.Value; }
        }

        public DateTime BOOK_VALUE_DATE
        {
            get { return ctlBOOK_VALUE_DATE.Value; }
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

                lstSTATUS.DataSource = CRMCache.List("contract_status_dom");
                lstSTATUS.DataBind();
                lstSTATUS.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
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
            m_sMODULE = "Products";
        }

        #endregion
    }
}