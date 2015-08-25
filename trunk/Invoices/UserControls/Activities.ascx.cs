using System;
using System.Data;
using System.Web.UI.WebControls;
using CRM.BusinessLogic;
using CRM.Common;
using CRM.ModelLayer;

namespace CRM.InvoiceManagement.Invoices
{
    /// <summary>
    ///		Summary description for Activities.
    /// </summary>
    public partial class Activities : CRMControl
    {
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
        protected DataView vwHistory;
        protected DataView vwOpen;

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Tasks.Create":
                    Response.Redirect("~/CRM/Tasks/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Meetings.Create":
                    Response.Redirect("~/CRM/Meetings/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Calls.Create":
                    Response.Redirect("~/CRM/Calls/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Emails.Compose":
                    Response.Redirect("~/CRM/Emails/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Notes.Create":
                    Response.Redirect("~/CRM/Notes/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Emails.Archive":
                    Response.Redirect("~/CRM/Emails/edit.aspx?PARENT_ID=" + gID);
                    break;
                case "Activities.Delete":
                    {
                        Guid gACTIVITY_ID = CommonTypeConvert.ToGuid(e.CommandArgument);
                        CommonProcedure.ActivitiesDelete(gACTIVITY_ID);
                        BindGrid();
                        break;
                    }
                default:
                    break;
            }
        }

        protected void BindGrid()
        {
            string innerSql = "select " + CommonTypeConvert.FormatSelectFields(arrSelectFields)
                              + "  from vwINVOICES_ACTIVITIES" + ControlChars.CrLf;
            var oQuery = new InlineQueryDBManager();

            oQuery.CommandText = innerSql;
            CRMSecurity.Filter(oQuery, m_sMODULE, "list", "ACTIVITY_ASSIGNED_USER_ID");
            TypeConvert.AppendParameter(oQuery, gID, "INVOICE_ID");
            TypeConvert.AppendParameter(oQuery, true, "IS_OPEN", false);
            oQuery.CommandText += grdOpen.OrderByClause("DATE_DUE", "desc");


            using (DataTable dt = oQuery.GetTable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    switch (CommonTypeConvert.ToString(row["ACTIVITY_TYPE"]))
                    {
                        case "Calls":
                            row["STATUS"] = Translation.GetTranslation.Term(".call_direction_dom.", row["DIRECTION"]) +
                                            " " + Translation.GetTranslation.Term(".call_status_dom.", row["STATUS"]);
                            break;
                    }
                }
                vwOpen = new DataView(dt);
                grdOpen.DataSource = vwOpen;
                grdOpen.DataBind();
            }


            oQuery.CommandText = innerSql;
            CRMSecurity.Filter(oQuery, m_sMODULE, "list", "ACTIVITY_ASSIGNED_USER_ID");
            TypeConvert.AppendParameter(oQuery, gID, "INVOICE_ID");
            TypeConvert.AppendParameter(oQuery, false, "IS_OPEN", false);
            oQuery.CommandText += grdHistory.OrderByClause("DATE_MODIFIED", "desc");


            using (DataTable dt = oQuery.GetTable())
            {
                foreach (DataRow row in dt.Rows)
                {
                    switch (CommonTypeConvert.ToString(row["ACTIVITY_TYPE"]))
                    {
                        case "Calls":
                            row["STATUS"] = Translation.GetTranslation.Term(".call_direction_dom.", row["DIRECTION"]) +
                                            " " + Translation.GetTranslation.Term(".call_status_dom.", row["STATUS"]);
                            break;
                    }
                }
                vwHistory = new DataView(dt);
                grdHistory.DataSource = vwHistory;
                grdHistory.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gID = CommonTypeConvert.ToGuid(Request["ID"]);
            BindGrid();
            if (!IsPostBack)
            {
                Guid gASSIGNED_USER_ID = CommonTypeConvert.ToGuid(Page.Items["ASSIGNED_USER_ID"]);
                ctlDynamicButtonsOpen.AppendButtons("Invoices.Activities.Open", gASSIGNED_USER_ID, gID);
                ctlDynamicButtonsHistory.AppendButtons("Invoices.Activities.History", gASSIGNED_USER_ID, gID);
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
            ctlDynamicButtonsOpen.Command += new CommandEventHandler(Page_Command);
            ctlDynamicButtonsHistory.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "Calls";
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("DATE_MODIFIED");
            arrSelectFields.Add("ACTIVITY_ID");
            arrSelectFields.Add("ACTIVITY_TYPE");
            arrSelectFields.Add("ACTIVITY_ASSIGNED_USER_ID");
            arrSelectFields.Add("IS_OPEN");
            arrSelectFields.Add("DATE_DUE");
            arrSelectFields.Add("STATUS");
            arrSelectFields.Add("DIRECTION");
            this.AppendGridColumns(grdOpen, "Invoices.Activities.Open", arrSelectFields);
            this.AppendGridColumns(grdHistory, "Invoices.Activities.History", arrSelectFields);
            if (IsPostBack)
            {
                ctlDynamicButtonsOpen.AppendButtons("Invoices.Activities.Open", Guid.Empty, Guid.Empty);
                ctlDynamicButtonsHistory.AppendButtons("Invoices.Activities.History", Guid.Empty, Guid.Empty);
            }
        }

        #endregion
    }
}