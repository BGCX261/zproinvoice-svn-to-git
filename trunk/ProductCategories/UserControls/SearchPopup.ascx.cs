using System;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.ProductCategories
{
    /// <summary>
    ///		Summary description for SearchPopup.
    /// </summary>
    public partial class SearchPopup : SearchControl
    {
        public override void ClearForm()
        {
            txtNAME.Text = String.Empty;
        }

        public override void SqlSearchClause(InlineQueryDBManager oQuery)
        {
            TypeConvert.AppendParameter(oQuery, txtNAME.Text, 50, CommonTypeConvert.SqlFilterMode.StartsWith, "NAME");
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
        }

        #endregion
    }
}