using System;
using System.Web.UI.WebControls;

namespace CRM.InvoiceManagement.ProductTemplates
{
    /// <summary>
    ///		Summary description for ImportDefaultsView.
    /// </summary>
    public partial class ImportDefaultsView : CRMControl
    {
        protected Guid gID;

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
            m_sMODULE = "ProductTemplates";
            this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
        }

        #endregion
    }
}