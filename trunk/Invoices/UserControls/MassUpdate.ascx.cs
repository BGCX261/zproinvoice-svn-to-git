using System;
using System.Web.UI.WebControls;
using CRM.Common;

namespace CRM.InvoiceManagement.Invoices
{
    /// <summary>
    ///		Summary description for MassUpdate.
    /// </summary>
    public partial class MassUpdate : CRM.MassUpdate
    {
        public CommandEventHandler Command;

        public Guid ASSIGNED_USER_ID
        {
            get { return ctlTeamAssignedMassUpdate.ASSIGNED_USER; }
        }

        public Guid TEAM_ID
        {
            get { return ctlTeamAssignedMassUpdate.TEAM; }
        }

        public string PAYMENT_TERMS
        {
            get { return lstPAYMENT_TERMS.SelectedValue; }
        }

        public string INVOICE_STAGE
        {
            get { return lstINVOICE_STAGE.SelectedValue; }
        }

        public DateTime DUE_DATE
        {
            get { return ctlDUE_DATE.Value; }
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

                lstPAYMENT_TERMS.DataSource = CRMCache.List("payment_terms_dom");
                lstPAYMENT_TERMS.DataBind();
                lstPAYMENT_TERMS.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
                lstINVOICE_STAGE.DataSource = CRMCache.List("invoice_stage_dom");
                lstINVOICE_STAGE.DataBind();
                lstINVOICE_STAGE.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
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
            m_sMODULE = "Invoices";
        }

        #endregion
    }
}