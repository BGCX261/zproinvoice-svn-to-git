using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.ProductCategories
{
    /// <summary>
    ///		Summary description for EditView.
    /// </summary>
    public partial class EditView : CRMControl
    {
        protected Guid gID;


        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Save" || e.CommandName == "SaveNew")
            {
                if (Page.IsValid)
                {
                    Guid gPARENT_ID = CommonTypeConvert.ToGuid(txtPARENT_ID.Value);
                    if (gPARENT_ID == gID)
                        gPARENT_ID = Guid.Empty;
                    CommonProcedure.spPRODUCT_CATEGORIES_Update(
                        ref gID
                        , gPARENT_ID
                        , txtNAME.Text
                        , txtDESCRIPTION.Text
                        , CommonTypeConvert.ToInteger(txtLIST_ORDER.Text)
                        );
                    Cache.Remove("vwPRODUCT_CATEGORIES_LISTBOX");
                    if (e.CommandName == "SaveNew")
                        Response.Redirect("edit.aspx");
                    else
                        Response.Redirect("Index.aspx");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle(Translation.GetTranslation.Term("ProductCategories.LBL_NAME"));
            Visible = Security.IS_ADMIN;
            Visible = Security.ADMIN_TYPE == 0 ? true : false;
            if (!Visible)
                return;

            reqNAME.DataBind();
            reqLIST_ORDER.DataBind();
            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            reqLIST_ORDER.DataBind();
            if (!IsPostBack)
            {
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
                if (!CommonTypeConvert.IsEmptyGuid(gID))
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["Administration_ProductCategories_EditView"].ToString();
                    oQuery.CommandText = innerSql;
                    oQuery.Add("@ID", SqlDbType.UniqueIdentifier, gID);


                    using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (rdr.Read())
                        {
                            txtNAME.Text = CommonTypeConvert.ToString(rdr["NAME"]);
                            ctlListHeader.Title = Translation.GetTranslation.Term("ProductCategories.LBL_NAME") + " " +
                                                  txtNAME.Text;
                            txtPARENT_ID.Value = CommonTypeConvert.ToString(rdr["PARENT_ID"]);
                            txtPARENT_NAME.Text = CommonTypeConvert.ToString(rdr["PARENT_NAME"]);
                            txtDESCRIPTION.Text = CommonTypeConvert.ToString(rdr["DESCRIPTION"]);
                            txtLIST_ORDER.Text = CommonTypeConvert.ToString(rdr["LIST_ORDER"]);
                        }
                    }
                }
                else
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["ProductCategories_EditView_111"].ToString();
                    oQuery.CommandText = innerSql;
                    txtLIST_ORDER.Text = (CommonTypeConvert.ToInteger(oQuery.ExecuteScalar()) + 1).ToString();
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
            ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "ProductCategories";
            if (IsPostBack)
            {
                ctlDynamicButtons.AppendButtons(m_sMODULE + ".EditView", Guid.Empty, null);
            }
        }

        #endregion
    }
}