using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Products
{
    /// <summary>
    /// Summary description for DetailView.
    /// </summary>
    public partial class DetailView : CRMControl
    {
        protected Guid gID;

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect("edit.aspx?ID=" + gID);
            }
            else if (e.CommandName == "Duplicate")
            {
                Response.Redirect("edit.aspx?DuplicateID=" + gID);
            }
            else if (e.CommandName == "Delete")
            {
                CommonProcedure.ProductDelete(gID);
                Response.Redirect("Index.aspx");
            }
            else if (e.CommandName == "Cancel")
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE));
            Visible = (Security.GetUserAccess(m_sMODULE, "view") >= 0);
            if (!Visible)
                return;

            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            if (!IsPostBack)
            {
                if (!CommonTypeConvert.IsEmptyGuid(gID))
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["Products_EditView"].ToString();
                    oQuery.CommandText = innerSql;
                    CRMSecurity.Filter(oQuery, m_sMODULE, "view");
                    TypeConvert.AppendParameter(oQuery, gID, "ID", false);

                    using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            ctlModuleHeader.Title = CommonTypeConvert.ToString(rdr["NAME"]);
                            SetPageTitle(Translation.GetTranslation.Term(".moduleList." + m_sMODULE) + " - " +
                                         ctlModuleHeader.Title);
                            Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);

                            AppendDetailViewFields(m_sMODULE + ".DetailView", tblMain, rdr, Page_Command);
                            Page.Items["ASSIGNED_USER_ID"] = CommonTypeConvert.ToGuid(rdr["ASSIGNED_USER_ID"]);
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".DetailView",
                                                            CommonTypeConvert.ToGuid(rdr["ASSIGNED_USER_ID"]), rdr);
                        }
                        else
                        {
                            plcSubPanel.Visible = false;
                            ctlDynamicButtons.AppendButtons(m_sMODULE + ".DetailView", Guid.Empty, null);
                            ctlDynamicButtons.DisableAll();
                            ctlDynamicButtons.ErrorText = Translation.GetTranslation.Term("ACL.LBL_NO_ACCESS");
                        }
                    }
                }
                else
                {
                    ctlDynamicButtons.AppendButtons(m_sMODULE + ".DetailView", Guid.Empty, null);
                    ctlDynamicButtons.DisableAll();
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "Products";
            SetMenu(m_sMODULE);
            this.AppendDetailViewRelationships(m_sMODULE + ".DetailView", plcSubPanel);
            if (IsPostBack)
            {
                this.AppendDetailViewFields(m_sMODULE + ".DetailView", tblMain, null);
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".DetailView", Guid.Empty, null);
            }
        }

        #endregion
    }
}