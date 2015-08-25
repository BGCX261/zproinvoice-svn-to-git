using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.Invoices
{
    /// <summary>
    ///		Summary description for LineItems.
    /// </summary>
    public partial class LineItems : CRMControl
    {
        protected Guid gID;
        protected DataView vwMain;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                default:
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gID = CommonTypeConvert.ToGuid(Request["ID"]);

            string innerSql = ApplicationSQL.SQL["Invoices_LineItems"].ToString();
            var oQuery = new InlineQueryDBManager();

            oQuery.CommandText = innerSql;
            TypeConvert.AppendParameter(oQuery, gID, "INVOICE_ID", false);
            oQuery.CommandText += " order by POSITION asc" + ControlChars.CrLf;


            using (DataTable dt = oQuery.GetTable())
            {
                vwMain = dt.DefaultView;
                grdMain.DataSource = vwMain;
                {
                    grdMain.DataBind();
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
            m_sMODULE = "Invoices";
            this.AppendGridColumns(grdMain, "Invoices.LineItems");
        }

        #endregion
    }
}