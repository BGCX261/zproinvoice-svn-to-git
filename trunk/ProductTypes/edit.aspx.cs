using System;

namespace CRM.InvoiceManagement.ProductTypes
{
    /// <summary>
    /// Summary description for Edit.
    /// </summary>
    public partial class Edit : CRMPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.IsAdminPage = true;
        }

        #endregion
    }
}