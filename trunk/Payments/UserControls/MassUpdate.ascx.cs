using System;
using System.Web.UI.WebControls;
using CRM.Common;

namespace CRM.InvoiceManagement.Payments
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

        public string PAYMENT_TYPE
        {
            get { return lstPAYMENT_TYPE.SelectedValue; }
        }

        public DateTime PAYMENT_DATE
        {
            get { return ctlPAYMENT_DATE.Value; }
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

                lstPAYMENT_TYPE.DataSource = CRMCache.List("payment_type_dom");
                lstPAYMENT_TYPE.DataBind();
                lstPAYMENT_TYPE.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
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
            m_sMODULE = "Payments";
        }

        #endregion
    }
}