
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Diagnostics;
using System.Data.SqlTypes;
using CRM.BusinessLogic;
using System.Data.SqlClient;
using CRM.Controls;
using CRM.Common;
using CRM.Abstract;
using System.Configuration;


namespace CRM
{
    /// <summary>
    /// Summary description for CRMDynamic.
    /// </summary>
    public class CRMDynamic
    {
        public static void AppendGridColumns(string sGRID_NAME, DataGrid grd)
        {
            AppendGridColumns(sGRID_NAME, grd, null);
        }


        public static void AppendGridColumns(string sGRID_NAME, DataGrid grd, UniqueStringCollection arrSelectFields)
        {
            if (grd == null)
            {
                return;
            }
            DataTable dt = CRMCache.GridViewColumns(sGRID_NAME);
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "COLUMN_INDEX";

                bool bEnableTeamManagement = Common.Config.enable_team_management();
                foreach (DataRowView row in dv)
                {
                    int nCOLUMN_INDEX = TypeConvert.ToInteger(row["COLUMN_INDEX"]);
                    string sCOLUMN_TYPE = TypeConvert.ToString(row["COLUMN_TYPE"]);
                    string sHEADER_TEXT = TypeConvert.ToString(row["HEADER_TEXT"]);
                    string sSORT_EXPRESSION = TypeConvert.ToString(row["SORT_EXPRESSION"]);
                    string sITEMSTYLE_WIDTH = TypeConvert.ToString(row["ITEMSTYLE_WIDTH"]);
                    string sITEMSTYLE_CSSCLASS = TypeConvert.ToString(row["ITEMSTYLE_CSSCLASS"]);
                    string sITEMSTYLE_HORIZONTAL_ALIGN = TypeConvert.ToString(row["ITEMSTYLE_HORIZONTAL_ALIGN"]);
                    string sITEMSTYLE_VERTICAL_ALIGN = TypeConvert.ToString(row["ITEMSTYLE_VERTICAL_ALIGN"]);
                    bool bITEMSTYLE_WRAP = TypeConvert.ToBoolean(row["ITEMSTYLE_WRAP"]);
                    string sDATA_FIELD = TypeConvert.ToString(row["DATA_FIELD"]);
                    string sDATA_FORMAT = TypeConvert.ToString(row["DATA_FORMAT"]);
                    string sURL_FIELD = TypeConvert.ToString(row["URL_FIELD"]);
                    string sURL_FORMAT = TypeConvert.ToString(row["URL_FORMAT"]);
                    string sURL_TARGET = TypeConvert.ToString(row["URL_TARGET"]);
                    string sLIST_NAME = TypeConvert.ToString(row["LIST_NAME"]);

                    string sURL_MODULE = TypeConvert.ToString(row["URL_MODULE"]);

                    string sURL_ASSIGNED_FIELD = TypeConvert.ToString(row["URL_ASSIGNED_FIELD"]);


                    if (arrSelectFields != null)
                    {
                        arrSelectFields.Add(sDATA_FIELD);
                        if (!TypeConvert.IsEmptyString(sSORT_EXPRESSION))
                            arrSelectFields.Add(sSORT_EXPRESSION);
                        if (!TypeConvert.IsEmptyString(sURL_FIELD))
                        {
                            if (sURL_FIELD.IndexOf(' ') >= 0)
                                arrSelectFields.AddRange(sURL_FIELD.Split(' '));
                            else
                                arrSelectFields.Add(sURL_FIELD);
                            if (!TypeConvert.IsEmptyString(sURL_ASSIGNED_FIELD))
                                arrSelectFields.Add(sURL_ASSIGNED_FIELD);
                        }
                    }

                    HorizontalAlign eHorizontalAlign = HorizontalAlign.NotSet;
                    switch (sITEMSTYLE_HORIZONTAL_ALIGN.ToLower())
                    {
                        case "left": eHorizontalAlign = HorizontalAlign.Left; break;
                        case "right": eHorizontalAlign = HorizontalAlign.Right; break;
                    }
                    VerticalAlign eVerticalAlign = VerticalAlign.NotSet;
                    switch (sITEMSTYLE_VERTICAL_ALIGN.ToLower())
                    {
                        case "top": eVerticalAlign = VerticalAlign.Top; break;
                        case "middle": eVerticalAlign = VerticalAlign.Middle; break;
                        case "bottom": eVerticalAlign = VerticalAlign.Bottom; break;
                    }

                    if (row["ITEMSTYLE_WRAP"] == DBNull.Value)
                        bITEMSTYLE_WRAP = true;
                    DataGridColumn col = null;

                    if (String.Compare(sCOLUMN_TYPE, "BoundColumn", true) == 0
                      && (String.Compare(sDATA_FORMAT, "Date", true) == 0
                          || String.Compare(sDATA_FORMAT, "DateTime", true) == 0
                          || String.Compare(sDATA_FORMAT, "Currency", true) == 0
                          || String.Compare(sDATA_FORMAT, "Image", true) == 0
                         )
                       )
                    {
                        sCOLUMN_TYPE = "TemplateColumn";
                    }
                    if (String.Compare(sCOLUMN_TYPE, "BoundColumn", true) == 0)
                    {
                        if (TypeConvert.IsEmptyString(sLIST_NAME))
                        {

                            BoundColumn bnd = new BoundColumn();
                            bnd.HeaderText = sHEADER_TEXT;
                            bnd.DataField = sDATA_FIELD;
                            bnd.SortExpression = sSORT_EXPRESSION;
                            bnd.ItemStyle.Width = new Unit(sITEMSTYLE_WIDTH);
                            bnd.ItemStyle.CssClass = sITEMSTYLE_CSSCLASS;
                            bnd.ItemStyle.HorizontalAlign = eHorizontalAlign;
                            bnd.ItemStyle.VerticalAlign = eVerticalAlign;
                            bnd.ItemStyle.Wrap = bITEMSTYLE_WRAP;

                            bnd.HeaderStyle.HorizontalAlign = eHorizontalAlign;
                            col = bnd;
                        }
                        else
                        {

                            TemplateColumn tpl = new TemplateColumn();
                            tpl.HeaderText = sHEADER_TEXT;
                            tpl.SortExpression = sSORT_EXPRESSION;
                            tpl.ItemStyle.Width = new Unit(sITEMSTYLE_WIDTH);
                            tpl.ItemStyle.CssClass = sITEMSTYLE_CSSCLASS;
                            tpl.ItemStyle.HorizontalAlign = eHorizontalAlign;
                            tpl.ItemStyle.VerticalAlign = eVerticalAlign;
                            tpl.ItemStyle.Wrap = bITEMSTYLE_WRAP;

                            tpl.HeaderStyle.HorizontalAlign = eHorizontalAlign;
                            tpl.ItemTemplate = new CreateItemTemplateLiteralList(sDATA_FIELD, sLIST_NAME);
                            col = tpl;
                        }
                    }
                    else if (String.Compare(sCOLUMN_TYPE, "TemplateColumn", true) == 0)
                    {

                        TemplateColumn tpl = new TemplateColumn();
                        tpl.HeaderText = sHEADER_TEXT;
                        tpl.SortExpression = sSORT_EXPRESSION;
                        tpl.ItemStyle.Width = new Unit(sITEMSTYLE_WIDTH);
                        tpl.ItemStyle.CssClass = sITEMSTYLE_CSSCLASS;
                        tpl.ItemStyle.HorizontalAlign = eHorizontalAlign;
                        tpl.ItemStyle.VerticalAlign = eVerticalAlign;
                        tpl.ItemStyle.Wrap = bITEMSTYLE_WRAP;

                        tpl.HeaderStyle.HorizontalAlign = eHorizontalAlign;
                        if (String.Compare(sDATA_FORMAT, "HyperLink", true) == 0)
                        {

                            if (sURL_FIELD.IndexOf(' ') >= 0)
                                tpl.ItemTemplate = new CreateItemTemplateHyperLinkOnClick(sDATA_FIELD, sURL_FIELD, sURL_FORMAT, sURL_TARGET, sITEMSTYLE_CSSCLASS, sURL_MODULE, sURL_ASSIGNED_FIELD);
                            else
                                tpl.ItemTemplate = new CreateItemTemplateHyperLink(sDATA_FIELD, sURL_FIELD, sURL_FORMAT, sURL_TARGET, sITEMSTYLE_CSSCLASS, sURL_MODULE, sURL_ASSIGNED_FIELD);
                        }
                        else if (String.Compare(sDATA_FORMAT, "Image", true) == 0)
                        {
                            tpl.ItemTemplate = new CreateItemTemplateImage(sDATA_FIELD, sITEMSTYLE_CSSCLASS);
                        }
                        else
                        {
                            tpl.ItemStyle.CssClass = sITEMSTYLE_CSSCLASS;
                            tpl.ItemTemplate = new CreateItemTemplateLiteral(sDATA_FIELD, sDATA_FORMAT);
                        }
                        col = tpl;
                    }
                    else if (String.Compare(sCOLUMN_TYPE, "HyperLinkColumn", true) == 0)
                    {

                        HyperLinkColumn lnk = new HyperLinkColumn();
                        lnk.HeaderText = sHEADER_TEXT;
                        lnk.DataTextField = sDATA_FIELD;
                        lnk.SortExpression = sSORT_EXPRESSION;
                        lnk.DataNavigateUrlField = sURL_FIELD;
                        lnk.DataNavigateUrlFormatString = sURL_FORMAT;
                        lnk.Target = sURL_TARGET;
                        lnk.ItemStyle.Width = new Unit(sITEMSTYLE_WIDTH);
                        lnk.ItemStyle.CssClass = sITEMSTYLE_CSSCLASS;
                        lnk.ItemStyle.HorizontalAlign = eHorizontalAlign;
                        lnk.ItemStyle.VerticalAlign = eVerticalAlign;
                        lnk.ItemStyle.Wrap = bITEMSTYLE_WRAP;

                        lnk.HeaderStyle.HorizontalAlign = eHorizontalAlign;
                        col = lnk;
                    }
                    if (col != null)
                    {

                        if (sDATA_FIELD == "TEAM_NAME" && !bEnableTeamManagement)
                        {
                            col.Visible = false;
                        }

                        if (nCOLUMN_INDEX >= grd.Columns.Count)
                            grd.Columns.Add(col);
                        else
                            grd.Columns.AddAt(nCOLUMN_INDEX, col);
                    }
                }
            }
        }

        public static void AppendGridColumns(DataView dvFields, HtmlTable tbl, System.Data.SqlClient.SqlDataReader rdr, CommandEventHandler Page_Command)
        {
            if (tbl == null)
            {
                return;
            }

            tbl.Border = 1;

            HtmlTableRow trAction = new HtmlTableRow();
            HtmlTableRow trHeader = new HtmlTableRow();
            HtmlTableRow trField = new HtmlTableRow();
            tbl.Rows.Insert(0, trAction);
            tbl.Rows.Insert(1, trHeader);
            tbl.Rows.Insert(2, trField);
            trAction.Attributes.Add("class", "listViewThS1");
            trHeader.Attributes.Add("class", "listViewThS1");
            trField.Attributes.Add("class", "oddListRowS1");

            HttpSessionState Session = HttpContext.Current.Session;
            foreach (DataRowView row in dvFields)
            {
                int gID = TypeConvert.ToInteger(row["ID"]);
                int nCOLUMN_INDEX = TypeConvert.ToInteger(row["COLUMN_INDEX"]);
                string sCOLUMN_TYPE = TypeConvert.ToString(row["COLUMN_TYPE"]);
                string sHEADER_TEXT = TypeConvert.ToString(row["HEADER_TEXT"]);
                string sSORT_EXPRESSION = TypeConvert.ToString(row["SORT_EXPRESSION"]);
                string sITEMSTYLE_WIDTH = TypeConvert.ToString(row["ITEMSTYLE_WIDTH"]);
                string sITEMSTYLE_CSSCLASS = TypeConvert.ToString(row["ITEMSTYLE_CSSCLASS"]);
                string sITEMSTYLE_HORIZONTAL_ALIGN = TypeConvert.ToString(row["ITEMSTYLE_HORIZONTAL_ALIGN"]);
                string sITEMSTYLE_VERTICAL_ALIGN = TypeConvert.ToString(row["ITEMSTYLE_VERTICAL_ALIGN"]);
                bool bITEMSTYLE_WRAP = TypeConvert.ToBoolean(row["ITEMSTYLE_WRAP"]);
                string sDATA_FIELD = TypeConvert.ToString(row["DATA_FIELD"]);
                string sDATA_FORMAT = TypeConvert.ToString(row["DATA_FORMAT"]);
                string sURL_FIELD = TypeConvert.ToString(row["URL_FIELD"]);
                string sURL_FORMAT = TypeConvert.ToString(row["URL_FORMAT"]);
                string sURL_TARGET = TypeConvert.ToString(row["URL_TARGET"]);
                string sLIST_NAME = TypeConvert.ToString(row["LIST_NAME"]);

                HtmlTableCell tdAction = new HtmlTableCell();
                trAction.Cells.Add(tdAction);
                tdAction.NoWrap = true;

                Literal litIndex = new Literal();
                tdAction.Controls.Add(litIndex);
                litIndex.Text = " " + nCOLUMN_INDEX.ToString() + " ";

                ImageButton btnMoveUp = CreateLayoutImageButtonSkin(gID, "Layout.MoveUp", nCOLUMN_INDEX, Translation.GetTranslation.Term(".LNK_LEFT"), "leftarrow_inline", Page_Command);
                ImageButton btnMoveDown = CreateLayoutImageButtonSkin(gID, "Layout.MoveDown", nCOLUMN_INDEX, Translation.GetTranslation.Term(".LNK_RIGHT"), "rightarrow_inline", Page_Command);
                ImageButton btnInsert = CreateLayoutImageButtonSkin(gID, "Layout.Insert", nCOLUMN_INDEX, Translation.GetTranslation.Term(".LNK_INS"), "plus_inline", Page_Command);
                ImageButton btnEdit = CreateLayoutImageButtonSkin(gID, "Layout.Edit", nCOLUMN_INDEX, Translation.GetTranslation.Term(".LNK_EDIT"), "edit_inline", Page_Command);
                ImageButton btnDelete = CreateLayoutImageButtonSkin(gID, "Layout.Delete", nCOLUMN_INDEX, Translation.GetTranslation.Term(".LNK_DELETE"), "delete_inline", Page_Command);
                tdAction.Controls.Add(btnMoveUp);
                tdAction.Controls.Add(btnMoveDown);
                tdAction.Controls.Add(btnInsert);
                tdAction.Controls.Add(btnEdit);
                tdAction.Controls.Add(btnDelete);

                HtmlTableCell tdHeader = new HtmlTableCell();
                trHeader.Cells.Add(tdHeader);
                tdHeader.NoWrap = true;

                HtmlTableCell tdField = new HtmlTableCell();
                trField.Cells.Add(tdField);
                tdField.NoWrap = true;

                Literal litHeader = new Literal();
                tdHeader.Controls.Add(litHeader);
                litHeader.Text = sHEADER_TEXT;

                Literal litField = new Literal();
                tdField.Controls.Add(litField);
                litField.Text = sDATA_FIELD;
            }
        }

        public static void AppendButtons(string sVIEW_NAME, Guid gASSIGNED_USER_ID, Control ctl, bool bIsMobile, System.Data.SqlClient.SqlDataReader rdr, CommandEventHandler Page_Command)
        {
            if (ctl == null)
            {
                return;
            }


            bool bIsPostBack = ctl.Page.IsPostBack;
            DataTable dt = CRMCache.DynamicButtons(sVIEW_NAME);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int gID = TypeConvert.ToInteger(row["ID"]);
                    int nCONTROL_INDEX = TypeConvert.ToInteger(row["CONTROL_INDEX"]);
                    string sCONTROL_TYPE = TypeConvert.ToString(row["CONTROL_TYPE"]);
                    string sMODULE_NAME = TypeConvert.ToString(row["MODULE_NAME"]);
                    string sMODULE_ACCESS_TYPE = TypeConvert.ToString(row["MODULE_ACCESS_TYPE"]);
                    string sTARGET_NAME = TypeConvert.ToString(row["TARGET_NAME"]);
                    string sTARGET_ACCESS_TYPE = TypeConvert.ToString(row["TARGET_ACCESS_TYPE"]);
                    bool bMOBILE_ONLY = TypeConvert.ToBoolean(row["MOBILE_ONLY"]);
                    bool bADMIN_ONLY = TypeConvert.ToBoolean(row["ADMIN_ONLY"]);
                    string sCONTROL_TEXT = TypeConvert.ToString(row["CONTROL_TEXT"]);
                    string sCONTROL_TOOLTIP = TypeConvert.ToString(row["CONTROL_TOOLTIP"]);
                    string sCONTROL_ACCESSKEY = TypeConvert.ToString(row["CONTROL_ACCESSKEY"]);
                    string sCONTROL_CSSCLASS = TypeConvert.ToString(row["CONTROL_CSSCLASS"]);
                    string sTEXT_FIELD = TypeConvert.ToString(row["TEXT_FIELD"]);
                    string sARGUMENT_FIELD = TypeConvert.ToString(row["ARGUMENT_FIELD"]);
                    string sCOMMAND_NAME = TypeConvert.ToString(row["COMMAND_NAME"]);
                    string sURL_FORMAT = TypeConvert.ToString(row["URL_FORMAT"]);
                    string sURL_TARGET = TypeConvert.ToString(row["URL_TARGET"]);
                    string sONCLICK_SCRIPT = TypeConvert.ToString(row["ONCLICK_SCRIPT"]);


                    DataView vwSchema = null;
                    if (rdr != null)
                        vwSchema = new DataView(rdr.GetSchemaTable());

                    string[] arrTEXT_FIELD = sTEXT_FIELD.Split(' ');
                    object[] objTEXT_FIELD = new object[arrTEXT_FIELD.Length];
                    for (int i = 0; i < arrTEXT_FIELD.Length; i++)
                    {
                        if (!TypeConvert.IsEmptyString(arrTEXT_FIELD[i]))
                        {
                            objTEXT_FIELD[i] = String.Empty;
                            if (rdr != null && vwSchema != null)
                            {
                                vwSchema.RowFilter = "ColumnName = '" + TypeConvert.EscapeSQL(arrTEXT_FIELD[i]) + "'";
                                if (vwSchema.Count > 0)
                                    objTEXT_FIELD[i] = TypeConvert.ToString(rdr[arrTEXT_FIELD[i]]);
                            }
                        }
                    }
                    if (String.Compare(sCONTROL_TYPE, "Button", true) == 0)
                    {
                        Button btn = new Button();
                        ctl.Controls.Add(btn);
                        if (!TypeConvert.IsEmptyString(sARGUMENT_FIELD))
                        {
                            if (rdr != null && vwSchema != null)
                            {
                                vwSchema.RowFilter = "ColumnName = '" + TypeConvert.EscapeSQL(sARGUMENT_FIELD) + "'";
                                if (vwSchema.Count > 0)
                                    btn.CommandArgument = TypeConvert.ToString(rdr[sARGUMENT_FIELD]);
                            }
                        }

                        btn.Text = "  " + Translation.GetTranslation.Term(sCONTROL_TEXT) + "  ";
                        btn.CssClass = sCONTROL_CSSCLASS;
                        btn.Command += Page_Command;
                        btn.CommandName = sCOMMAND_NAME;
                        btn.OnClientClick = sONCLICK_SCRIPT;
                        btn.Visible = (bMOBILE_ONLY && bIsMobile || !bMOBILE_ONLY) && (bADMIN_ONLY && CRMSecurity.IS_ADMIN || !bADMIN_ONLY);
                        if (btn.Visible && !TypeConvert.IsEmptyString(sMODULE_NAME) && !TypeConvert.IsEmptyString(sMODULE_ACCESS_TYPE))
                        {
                            int nACLACCESS = CRM.CRMSecurity.GetUserAccess(sMODULE_NAME, sMODULE_ACCESS_TYPE);
                            btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            if (btn.Visible && !TypeConvert.IsEmptyString(sTARGET_NAME) && !TypeConvert.IsEmptyString(sTARGET_ACCESS_TYPE))
                            {
                                nACLACCESS = CRM.CRMSecurity.GetUserAccess(sTARGET_NAME, sTARGET_ACCESS_TYPE);
                                btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            }
                        }

                        if (!TypeConvert.IsEmptyString(sCONTROL_TOOLTIP))
                        {
                            btn.ToolTip = Translation.GetTranslation.Term(sCONTROL_TOOLTIP);
                            if (btn.ToolTip.Contains("[Alt]"))
                            {
                                if (btn.AccessKey.Length > 0)
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", "[Alt+" + btn.AccessKey + "]");
                                else
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", String.Empty);
                            }
                        }
                        btn.Attributes.Add("style", "margin-right: 3px;");
                    }
                    else if (String.Compare(sCONTROL_TYPE, "HyperLink", true) == 0)
                    {
                        HyperLink lnk = new HyperLink();
                        ctl.Controls.Add(lnk);
                        lnk.Text = Translation.GetTranslation.Term(sCONTROL_TEXT);
                        lnk.NavigateUrl = String.Format(sURL_FORMAT, objTEXT_FIELD);
                        lnk.Target = sURL_TARGET;
                        lnk.CssClass = sCONTROL_CSSCLASS;
                        lnk.Visible = (bMOBILE_ONLY && bIsMobile || !bMOBILE_ONLY) && (bADMIN_ONLY && CRMSecurity.IS_ADMIN || !bADMIN_ONLY);
                        if (lnk.Visible && !TypeConvert.IsEmptyString(sMODULE_NAME) && !TypeConvert.IsEmptyString(sMODULE_ACCESS_TYPE))
                        {
                            int nACLACCESS = CRM.CRMSecurity.GetUserAccess(sMODULE_NAME, sMODULE_ACCESS_TYPE);
                            lnk.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            if (lnk.Visible && !TypeConvert.IsEmptyString(sTARGET_NAME) && !TypeConvert.IsEmptyString(sTARGET_ACCESS_TYPE))
                            {
                                nACLACCESS = CRM.CRMSecurity.GetUserAccess(sTARGET_NAME, sTARGET_ACCESS_TYPE);
                                lnk.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            }
                        }
                        if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                        {
                            lnk.Attributes.Add("onclick", sONCLICK_SCRIPT);
                        }

                        if (!TypeConvert.IsEmptyString(sCONTROL_TOOLTIP))
                        {
                            lnk.ToolTip = Translation.GetTranslation.Term(sCONTROL_TOOLTIP);
                            if (lnk.ToolTip.Contains("[Alt]"))
                            {
                                if (lnk.AccessKey.Length > 0)
                                    lnk.ToolTip = lnk.ToolTip.Replace("[Alt]", "[Alt+" + lnk.AccessKey + "]");
                                else
                                    lnk.ToolTip = lnk.ToolTip.Replace("[Alt]", String.Empty);
                            }
                        }

                        lnk.Attributes.Add("style", "margin-right: 3px; margin-left: 3px;");
                    }
                    else if (String.Compare(sCONTROL_TYPE, "ButtonLink", true) == 0)
                    {
                        Button btn = new Button();
                        ctl.Controls.Add(btn);
                        btn.Text = "  " + Translation.GetTranslation.Term(sCONTROL_TEXT) + "  ";
                        btn.CssClass = sCONTROL_CSSCLASS;

                        btn.Command += Page_Command;
                        btn.CommandName = sCOMMAND_NAME;
                        btn.OnClientClick = "window.location.href='" + TypeConvert.EscapeJavaScript(String.Format(sURL_FORMAT, objTEXT_FIELD)) + "'; return false;";
                        btn.Visible = (bMOBILE_ONLY && bIsMobile || !bMOBILE_ONLY) && (bADMIN_ONLY && CRMSecurity.IS_ADMIN || !bADMIN_ONLY);
                        if (btn.Visible && !TypeConvert.IsEmptyString(sMODULE_NAME) && !TypeConvert.IsEmptyString(sMODULE_ACCESS_TYPE))
                        {
                            int nACLACCESS = CRM.CRMSecurity.GetUserAccess(sMODULE_NAME, sMODULE_ACCESS_TYPE);
                            btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            if (btn.Visible && !TypeConvert.IsEmptyString(sTARGET_NAME) && !TypeConvert.IsEmptyString(sTARGET_ACCESS_TYPE))
                            {
                                nACLACCESS = CRM.CRMSecurity.GetUserAccess(sTARGET_NAME, sTARGET_ACCESS_TYPE);
                                btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            }
                        }

                        if (!TypeConvert.IsEmptyString(sCONTROL_TOOLTIP))
                        {
                            btn.ToolTip = Translation.GetTranslation.Term(sCONTROL_TOOLTIP);
                            if (btn.ToolTip.Contains("[Alt]"))
                            {
                                if (btn.AccessKey.Length > 0)
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", "[Alt+" + btn.AccessKey + "]");
                                else
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", String.Empty);
                            }
                        }
                        btn.Attributes.Add("style", "margin-right: 3px;");
                    }
                }
            }
        }

        public static void AppendButtons(string sVIEW_NAME, Guid gASSIGNED_USER_ID, Control ctl, bool bIsMobile, DataTableReader rdr, CommandEventHandler Page_Command)
        {
            if (ctl == null)
            {
                return;
            }


            bool bIsPostBack = ctl.Page.IsPostBack;
            DataTable dt = CRMCache.DynamicButtons(sVIEW_NAME);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int gID = TypeConvert.ToInteger(row["ID"]);
                    int nCONTROL_INDEX = TypeConvert.ToInteger(row["CONTROL_INDEX"]);
                    string sCONTROL_TYPE = TypeConvert.ToString(row["CONTROL_TYPE"]);
                    string sMODULE_NAME = TypeConvert.ToString(row["MODULE_NAME"]);
                    string sMODULE_ACCESS_TYPE = TypeConvert.ToString(row["MODULE_ACCESS_TYPE"]);
                    string sTARGET_NAME = TypeConvert.ToString(row["TARGET_NAME"]);
                    string sTARGET_ACCESS_TYPE = TypeConvert.ToString(row["TARGET_ACCESS_TYPE"]);
                    bool bMOBILE_ONLY = TypeConvert.ToBoolean(row["MOBILE_ONLY"]);
                    bool bADMIN_ONLY = TypeConvert.ToBoolean(row["ADMIN_ONLY"]);
                    string sCONTROL_TEXT = TypeConvert.ToString(row["CONTROL_TEXT"]);
                    string sCONTROL_TOOLTIP = TypeConvert.ToString(row["CONTROL_TOOLTIP"]);
                    string sCONTROL_ACCESSKEY = TypeConvert.ToString(row["CONTROL_ACCESSKEY"]);
                    string sCONTROL_CSSCLASS = TypeConvert.ToString(row["CONTROL_CSSCLASS"]);
                    string sTEXT_FIELD = TypeConvert.ToString(row["TEXT_FIELD"]);
                    string sARGUMENT_FIELD = TypeConvert.ToString(row["ARGUMENT_FIELD"]);
                    string sCOMMAND_NAME = TypeConvert.ToString(row["COMMAND_NAME"]);
                    string sURL_FORMAT = TypeConvert.ToString(row["URL_FORMAT"]);
                    string sURL_TARGET = TypeConvert.ToString(row["URL_TARGET"]);
                    string sONCLICK_SCRIPT = TypeConvert.ToString(row["ONCLICK_SCRIPT"]);


                    DataView vwSchema = null;
                    if (rdr != null)
                        vwSchema = new DataView(rdr.GetSchemaTable());

                    string[] arrTEXT_FIELD = sTEXT_FIELD.Split(' ');
                    object[] objTEXT_FIELD = new object[arrTEXT_FIELD.Length];
                    for (int i = 0; i < arrTEXT_FIELD.Length; i++)
                    {
                        if (!TypeConvert.IsEmptyString(arrTEXT_FIELD[i]))
                        {
                            objTEXT_FIELD[i] = String.Empty;
                            if (rdr != null && vwSchema != null)
                            {
                                vwSchema.RowFilter = "ColumnName = '" + TypeConvert.EscapeSQL(arrTEXT_FIELD[i]) + "'";
                                if (vwSchema.Count > 0)
                                    objTEXT_FIELD[i] = TypeConvert.ToString(rdr[arrTEXT_FIELD[i]]);
                            }
                        }
                    }
                    if (String.Compare(sCONTROL_TYPE, "Button", true) == 0)
                    {
                        Button btn = new Button();
                        ctl.Controls.Add(btn);
                        if (!TypeConvert.IsEmptyString(sARGUMENT_FIELD))
                        {
                            if (rdr != null && vwSchema != null)
                            {
                                vwSchema.RowFilter = "ColumnName = '" + TypeConvert.EscapeSQL(sARGUMENT_FIELD) + "'";
                                if (vwSchema.Count > 0)
                                    btn.CommandArgument = TypeConvert.ToString(rdr[sARGUMENT_FIELD]);
                            }
                        }

                        btn.Text = "  " + Translation.GetTranslation.Term(sCONTROL_TEXT) + "  ";
                        btn.CssClass = sCONTROL_CSSCLASS;
                        btn.Command += Page_Command;
                        btn.CommandName = sCOMMAND_NAME;
                        btn.OnClientClick = sONCLICK_SCRIPT;
                        btn.Visible = (bMOBILE_ONLY && bIsMobile || !bMOBILE_ONLY) && (bADMIN_ONLY && CRMSecurity.IS_ADMIN || !bADMIN_ONLY);
                        if (btn.Visible && !TypeConvert.IsEmptyString(sMODULE_NAME) && !TypeConvert.IsEmptyString(sMODULE_ACCESS_TYPE))
                        {
                            int nACLACCESS = CRM.CRMSecurity.GetUserAccess(sMODULE_NAME, sMODULE_ACCESS_TYPE);
                            btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            if (btn.Visible && !TypeConvert.IsEmptyString(sTARGET_NAME) && !TypeConvert.IsEmptyString(sTARGET_ACCESS_TYPE))
                            {
                                nACLACCESS = CRM.CRMSecurity.GetUserAccess(sTARGET_NAME, sTARGET_ACCESS_TYPE);
                                btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            }
                        }

                        if (!TypeConvert.IsEmptyString(sCONTROL_TOOLTIP))
                        {
                            btn.ToolTip = Translation.GetTranslation.Term(sCONTROL_TOOLTIP);
                            if (btn.ToolTip.Contains("[Alt]"))
                            {
                                if (btn.AccessKey.Length > 0)
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", "[Alt+" + btn.AccessKey + "]");
                                else
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", String.Empty);
                            }
                        }
                        btn.Attributes.Add("style", "margin-right: 3px;");
                    }
                    else if (String.Compare(sCONTROL_TYPE, "HyperLink", true) == 0)
                    {
                        HyperLink lnk = new HyperLink();
                        ctl.Controls.Add(lnk);
                        lnk.Text = Translation.GetTranslation.Term(sCONTROL_TEXT);
                        lnk.NavigateUrl = String.Format(sURL_FORMAT, objTEXT_FIELD);
                        lnk.Target = sURL_TARGET;
                        lnk.CssClass = sCONTROL_CSSCLASS;
                        lnk.Visible = (bMOBILE_ONLY && bIsMobile || !bMOBILE_ONLY) && (bADMIN_ONLY && CRMSecurity.IS_ADMIN || !bADMIN_ONLY);
                        if (lnk.Visible && !TypeConvert.IsEmptyString(sMODULE_NAME) && !TypeConvert.IsEmptyString(sMODULE_ACCESS_TYPE))
                        {
                            int nACLACCESS = CRM.CRMSecurity.GetUserAccess(sMODULE_NAME, sMODULE_ACCESS_TYPE);
                            lnk.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            if (lnk.Visible && !TypeConvert.IsEmptyString(sTARGET_NAME) && !TypeConvert.IsEmptyString(sTARGET_ACCESS_TYPE))
                            {
                                nACLACCESS = CRM.CRMSecurity.GetUserAccess(sTARGET_NAME, sTARGET_ACCESS_TYPE);
                                lnk.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            }
                        }
                        if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                        {
                            lnk.Attributes.Add("onclick", sONCLICK_SCRIPT);
                        }

                        if (!TypeConvert.IsEmptyString(sCONTROL_TOOLTIP))
                        {
                            lnk.ToolTip = Translation.GetTranslation.Term(sCONTROL_TOOLTIP);
                            if (lnk.ToolTip.Contains("[Alt]"))
                            {
                                if (lnk.AccessKey.Length > 0)
                                    lnk.ToolTip = lnk.ToolTip.Replace("[Alt]", "[Alt+" + lnk.AccessKey + "]");
                                else
                                    lnk.ToolTip = lnk.ToolTip.Replace("[Alt]", String.Empty);
                            }
                        }

                        lnk.Attributes.Add("style", "margin-right: 3px; margin-left: 3px;");
                    }
                    else if (String.Compare(sCONTROL_TYPE, "ButtonLink", true) == 0)
                    {
                        Button btn = new Button();
                        ctl.Controls.Add(btn);
                        btn.Text = "  " + Translation.GetTranslation.Term(sCONTROL_TEXT) + "  ";
                        btn.CssClass = sCONTROL_CSSCLASS;

                        btn.Command += Page_Command;
                        btn.CommandName = sCOMMAND_NAME;
                        btn.OnClientClick = "window.location.href='" + TypeConvert.EscapeJavaScript(String.Format(sURL_FORMAT, objTEXT_FIELD)) + "'; return false;";
                        btn.Visible = (bMOBILE_ONLY && bIsMobile || !bMOBILE_ONLY) && (bADMIN_ONLY && CRMSecurity.IS_ADMIN || !bADMIN_ONLY);
                        if (btn.Visible && !TypeConvert.IsEmptyString(sMODULE_NAME) && !TypeConvert.IsEmptyString(sMODULE_ACCESS_TYPE))
                        {
                            int nACLACCESS = CRM.CRMSecurity.GetUserAccess(sMODULE_NAME, sMODULE_ACCESS_TYPE);
                            btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            if (btn.Visible && !TypeConvert.IsEmptyString(sTARGET_NAME) && !TypeConvert.IsEmptyString(sTARGET_ACCESS_TYPE))
                            {
                                nACLACCESS = CRM.CRMSecurity.GetUserAccess(sTARGET_NAME, sTARGET_ACCESS_TYPE);
                                btn.Visible = (nACLACCESS > ACL_ACCESS.OWNER) || (nACLACCESS == ACL_ACCESS.OWNER && CRMSecurity.USER_ID != gASSIGNED_USER_ID);
                            }
                        }

                        if (!TypeConvert.IsEmptyString(sCONTROL_TOOLTIP))
                        {
                            btn.ToolTip = Translation.GetTranslation.Term(sCONTROL_TOOLTIP);
                            if (btn.ToolTip.Contains("[Alt]"))
                            {
                                if (btn.AccessKey.Length > 0)
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", "[Alt+" + btn.AccessKey + "]");
                                else
                                    btn.ToolTip = btn.ToolTip.Replace("[Alt]", String.Empty);
                            }
                        }
                        btn.Attributes.Add("style", "margin-right: 3px;");
                    }
                }
            }
        }

        private static ImageButton CreateLayoutImageButtonSkin(int gID, string sCommandName, int nFIELD_INDEX, string sAlternateText, string sSkinID, CommandEventHandler Page_Command)
        {
            ImageButton btnDelete = new ImageButton();

            btnDelete.ID = sCommandName + "." + gID.ToString();
            btnDelete.CommandName = sCommandName;
            btnDelete.CommandArgument = nFIELD_INDEX.ToString();
            btnDelete.CssClass = "listViewTdToolsS1";
            btnDelete.AlternateText = sAlternateText;
            btnDelete.SkinID = sSkinID;
            if (Page_Command != null)
                btnDelete.Command += Page_Command;
            return btnDelete;
        }

        public static void AppendDetailViewFields(string sDETAIL_NAME, HtmlTable tbl, System.Data.SqlClient.SqlDataReader rdr, CommandEventHandler Page_Command)
        {
            if (tbl == null)
            {
                return;
            }
            DataTable dt = CRMCache.DetailViewFields(sDETAIL_NAME);
            if (null != dt && dt.Rows.Count > 0)
            {
                DataView dvFields = dt.DefaultView;
                dvFields.Sort = "FIELD_INDEX";
                tbl.Attributes.Add("name", sDETAIL_NAME);
                AppendDetailViewFields(dvFields, tbl, rdr, Page_Command, false);
            }
        }

        public static void AppendDetailViewFields(DataView dvFields, HtmlTable tbl, System.Data.SqlClient.SqlDataReader rdr, CommandEventHandler Page_Command, bool bLayoutMode)
        {
            bool bIsMobile = false;
            CRMPage Page = tbl.Page as CRMPage;
            if (Page != null)
                bIsMobile = Page.IsMobile;

            HtmlTableRow tr = null;

            int nRowIndex = tbl.Rows.Count - 1;
            int nColIndex = 0;

            if (bLayoutMode)
                tbl.Border = 1;

            HttpSessionState Session = HttpContext.Current.Session;

            if (dvFields.Count == 0 && tbl.Rows.Count <= 1)
                tbl.Visible = false;


            DataView vwSchema = null;
            if (rdr != null)
                vwSchema = new DataView(rdr.GetSchemaTable());


            bool bEnableTeamManagement = Common.Config.enable_team_management();
            foreach (DataRowView row in dvFields)
            {
                int gID = TypeConvert.ToInteger(row["ID"]);
                int nFIELD_INDEX = TypeConvert.ToInteger(row["FIELD_INDEX"]);
                string sFIELD_TYPE = TypeConvert.ToString(row["FIELD_TYPE"]);
                string sDATA_LABEL = TypeConvert.ToString(row["DATA_LABEL"]);
                string sDATA_FIELD = TypeConvert.ToString(row["DATA_FIELD"]);
                string sDATA_FORMAT = TypeConvert.ToString(row["DATA_FORMAT"]);
                string sLIST_NAME = TypeConvert.ToString(row["LIST_NAME"]);
                int nCOLSPAN = TypeConvert.ToInteger(row["COLSPAN"]);
                string sLABEL_WIDTH = TypeConvert.ToString(row["LABEL_WIDTH"]);
                string sFIELD_WIDTH = TypeConvert.ToString(row["FIELD_WIDTH"]);
                int nDATA_COLUMNS = TypeConvert.ToInteger(row["DATA_COLUMNS"]);

                if (nDATA_COLUMNS == 0)
                    nDATA_COLUMNS = 2;

                if (!bLayoutMode && sDATA_FIELD == "TEAM_NAME" && !bEnableTeamManagement)
                {
                    sFIELD_TYPE = "Blank";
                }

                if (nColIndex == 0 || bIsMobile)
                {

                    nRowIndex++;
                    tr = new HtmlTableRow();
                    tbl.Rows.Insert(nRowIndex, tr);
                }
                if (bLayoutMode)
                {
                    HtmlTableCell tdAction = new HtmlTableCell();
                    tr.Cells.Add(tdAction);
                    tdAction.Attributes.Add("class", "tabDetailViewDL");
                    tdAction.NoWrap = true;

                    Literal litIndex = new Literal();
                    tdAction.Controls.Add(litIndex);
                    litIndex.Text = " " + nFIELD_INDEX.ToString() + " ";


                    ImageButton btnMoveUp = CreateLayoutImageButtonSkin(gID, "Layout.MoveUp", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_UP"), "uparrow_inline", Page_Command);
                    ImageButton btnMoveDown = CreateLayoutImageButtonSkin(gID, "Layout.MoveDown", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_DOWN"), "downarrow_inline", Page_Command);
                    ImageButton btnInsert = CreateLayoutImageButtonSkin(gID, "Layout.Insert", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_INS"), "plus_inline", Page_Command);
                    ImageButton btnEdit = CreateLayoutImageButtonSkin(gID, "Layout.Edit", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_EDIT"), "edit_inline", Page_Command);
                    ImageButton btnDelete = CreateLayoutImageButtonSkin(gID, "Layout.Delete", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_DELETE"), "delete_inline", Page_Command);
                    tdAction.Controls.Add(btnMoveUp);
                    tdAction.Controls.Add(btnMoveDown);
                    tdAction.Controls.Add(btnInsert);
                    tdAction.Controls.Add(btnEdit);
                    tdAction.Controls.Add(btnDelete);
                }
                HtmlTableCell tdLabel = new HtmlTableCell();
                HtmlTableCell tdField = new HtmlTableCell();
                tr.Cells.Add(tdLabel);
                tr.Cells.Add(tdField);
                if (nCOLSPAN > 0)
                {
                    tdField.ColSpan = nCOLSPAN;
                    if (bLayoutMode)
                        tdField.ColSpan++;
                }
                tdLabel.Attributes.Add("class", "tabDetailViewDL");
                tdLabel.VAlign = "top";
                tdLabel.Width = sLABEL_WIDTH;
                tdField.Attributes.Add("class", "tabDetailViewDF");
                tdField.VAlign = "top";

                if (nCOLSPAN == 0)
                    tdField.Width = sFIELD_WIDTH;

                Literal litLabel = new Literal();
                HyperLink lnkField = null;
                tdLabel.Controls.Add(litLabel);

                if (bLayoutMode)
                    litLabel.Text = sDATA_LABEL;
                else if (sDATA_LABEL.IndexOf(".") >= 0)
                    litLabel.Text = Translation.GetTranslation.Term(sDATA_LABEL);
                else if (!TypeConvert.IsEmptyString(sDATA_LABEL) && rdr != null)
                {

                    litLabel.Text = sDATA_LABEL;
                    if (vwSchema != null)
                    {
                        vwSchema.RowFilter = "ColumnName = '" + TypeConvert.EscapeSQL(sDATA_LABEL) + "'";
                        if (vwSchema.Count > 0)
                            litLabel.Text = TypeConvert.ToString(rdr[sDATA_LABEL]) + Translation.GetTranslation.Term("Calls.LBL_COLON");
                    }
                }

                else
                    litLabel.Text = "&nbsp;";

                if (String.Compare(sFIELD_TYPE, "Blank", true) == 0)
                {
                    Literal litField = new Literal();
                    tdField.Controls.Add(litField);
                    if (bLayoutMode)
                    {
                        litLabel.Text = "*** BLANK ***";
                        litField.Text = "*** BLANK ***";
                    }
                    else
                    {

                        litLabel.Text = "&nbsp;";
                        litField.Text = "&nbsp;";
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Line", true) == 0)
                {
                    if (bLayoutMode)
                    {
                        Literal litField = new Literal();
                        tdField.Controls.Add(litField);
                        litLabel.Text = "*** LINE ***";
                        litField.Text = "*** LINE ***";
                    }
                    else
                    {
                        tr.Cells.Clear();
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "String", true) == 0)
                {
                    if (bLayoutMode)
                    {
                        Literal litField = new Literal();
                        litField.Text = sDATA_FIELD;
                        tdField.Controls.Add(litField);
                    }
                    else if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        HtmlGenericControl spnField = new HtmlGenericControl("span");
                        spnField.InnerHtml = "&nbsp;";
                        tdField.Controls.Add(spnField);
                        spnField.ID = sDATA_FIELD;

                        Literal litField = new Literal();
                        spnField.Controls.Add(litField);
                        string[] arrLIST_NAME = sLIST_NAME.Split(' ');
                        string[] arrDATA_FIELD = sDATA_FIELD.Split(' ');
                        object[] objDATA_FIELD = new object[arrDATA_FIELD.Length];
                        for (int i = 0; i < arrDATA_FIELD.Length; i++)
                        {
                            if (arrDATA_FIELD[i].IndexOf(".") >= 0)
                            {
                                objDATA_FIELD[i] = Translation.GetTranslation.Term(arrDATA_FIELD[i]);
                            }
                            else if (!TypeConvert.IsEmptyString(sLIST_NAME))
                            {
                                if (arrLIST_NAME.Length == arrDATA_FIELD.Length)
                                {
                                    if (rdr != null)
                                    {

                                        if (sLIST_NAME == "AssignedUser")
                                        {
                                            objDATA_FIELD[i] = CRMCache.AssignedUser(TypeConvert.ToGuid(rdr[arrDATA_FIELD[i]]));
                                        }

                                        else if (TypeConvert.ToString(rdr[arrDATA_FIELD[i]]).StartsWith("<?xml"))
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            XmlDocument xml = new XmlDocument();
                                            xml.LoadXml(TypeConvert.ToString(rdr[arrDATA_FIELD[i]]));
                                            XmlNodeList nlValues = xml.DocumentElement.SelectNodes("Value");
                                            foreach (XmlNode xValue in nlValues)
                                            {
                                                if (sb.Length > 0)
                                                    sb.Append(", ");
                                                sb.Append(Translation.GetTranslation.Term("." + arrLIST_NAME[i] + ".", xValue.InnerText));
                                            }
                                            objDATA_FIELD[i] = sb.ToString();
                                        }
                                        else
                                        {
                                            objDATA_FIELD[i] = Translation.GetTranslation.Term("." + arrLIST_NAME[i] + ".", rdr[arrDATA_FIELD[i]]);
                                        }
                                    }
                                    else
                                        objDATA_FIELD[i] = String.Empty;
                                }
                            }
                            else if (!TypeConvert.IsEmptyString(arrDATA_FIELD[i]))
                            {
                                if (rdr != null && rdr[arrDATA_FIELD[i]] != null && rdr[arrDATA_FIELD[i]] != DBNull.Value)
                                {

                                    if (rdr[arrDATA_FIELD[i]].GetType() == Type.GetType("System.DateTime"))
                                    {
                                        if (CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[arrDATA_FIELD[i]]) == ((DateTime)SqlDateTime.MinValue))
                                            objDATA_FIELD[i] = "";
                                        else
                                            objDATA_FIELD[i] = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[arrDATA_FIELD[i]]);
                                    }
                                    else
                                        objDATA_FIELD[i] = rdr[arrDATA_FIELD[i]];
                                }
                                else
                                    objDATA_FIELD[i] = String.Empty;
                            }
                        }
                        if (rdr != null)
                        {

                            if (sDATA_FORMAT == String.Empty)
                            {
                                for (int i = 0; i < arrDATA_FIELD.Length; i++)
                                    arrDATA_FIELD[i] = TypeConvert.ToString(objDATA_FIELD[i]);
                                litField.Text = String.Join(" ", arrDATA_FIELD);
                            }
                            else if (sDATA_FORMAT == "{0:c}" && Currency.GetCurrency != null)
                            {

                                if (!(objDATA_FIELD[0] is string))
                                {
                                    Decimal d = Currency.GetCurrency.ToCurrency(Convert.ToDecimal(objDATA_FIELD[0]));
                                    litField.Text = d.ToString("c");
                                }
                            }
                            else
                                litField.Text = String.Format(sDATA_FORMAT, objDATA_FIELD);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "CheckBox", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        CheckBox chkField = new CheckBox();
                        tdField.Controls.Add(chkField);
                        chkField.Enabled = false;
                        chkField.CssClass = "checkbox";

                        chkField.ID = sDATA_FIELD;
                        if (rdr != null)
                            chkField.Checked = TypeConvert.ToBoolean(rdr[sDATA_FIELD]);
                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Button", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        Button btnField = new Button();
                        tdField.Controls.Add(btnField);
                        btnField.CssClass = "button";

                        btnField.ID = sDATA_FIELD;
                        if (Page_Command != null)
                        {
                            btnField.Command += Page_Command;
                            btnField.CommandName = sDATA_FORMAT;
                        }
                        if (bLayoutMode)
                        {
                            btnField.Text = sDATA_FIELD;
                            btnField.Enabled = false;
                        }
                        else if (sDATA_FIELD.IndexOf(".") >= 0)
                        {
                            btnField.Text = Translation.GetTranslation.Term(sDATA_FIELD);
                        }
                        else if (!TypeConvert.IsEmptyString(sDATA_FIELD) && rdr != null)
                        {
                            btnField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                        }
                        btnField.Attributes.Add("title", btnField.Text);
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Textbox", true) == 0)
                {

                    if (bLayoutMode)
                    {
                        Literal litField = new Literal();
                        litField.Text = sDATA_FIELD;
                        tdField.Controls.Add(litField);
                    }
                    else if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        HtmlGenericControl spnField = new HtmlGenericControl("span");
                        tdField.Controls.Add(spnField);
                        spnField.ID = sDATA_FIELD;

                        Literal litField = new Literal();
                        spnField.Controls.Add(litField);
                        if (rdr != null)
                        {
                            string sDATA = TypeConvert.ToString(rdr[sDATA_FIELD]);

                            sDATA = sDATA.Replace("\r\n", "\n");
                            sDATA = sDATA.Replace("\r", "\n");
                            sDATA = sDATA.Replace("\n", "<br />\r\n");
                            litField.Text = sDATA;
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "HyperLink", true) == 0)
                {
                    string sURL_FIELD = TypeConvert.ToString(row["URL_FIELD"]);
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD) && !TypeConvert.IsEmptyString(sURL_FIELD))
                    {
                        string sURL_FORMAT = TypeConvert.ToString(row["URL_FORMAT"]);
                        string sURL_TARGET = TypeConvert.ToString(row["URL_TARGET"]);
                        lnkField = new HyperLink();
                        lnkField.Text = " &nbsp;";
                        tdField.Controls.Add(lnkField);
                        lnkField.Target = sURL_TARGET;
                        lnkField.CssClass = "tabDetailViewDFLink";

                        lnkField.ID = sDATA_FIELD;
                        if (bLayoutMode)
                        {
                            lnkField.Text = sDATA_FIELD;
                            lnkField.Enabled = false;
                        }
                        else if (rdr != null)
                        {
                            if (!TypeConvert.IsEmptyString(rdr[sDATA_FIELD]))
                            {

                                if (sDATA_FORMAT == String.Empty)
                                    lnkField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                else
                                    lnkField.Text = String.Format(sDATA_FORMAT, TypeConvert.ToString(rdr[sDATA_FIELD]));
                            }
                        }
                        if (bLayoutMode)
                        {
                            lnkField.NavigateUrl = sURL_FIELD;
                        }
                        else if (rdr != null)
                        {
                            if (!TypeConvert.IsEmptyString(rdr[sURL_FIELD]))
                            {

                                if (sDATA_FORMAT == String.Empty)
                                    lnkField.NavigateUrl = TypeConvert.ToString(rdr[sURL_FIELD]);
                                else
                                    lnkField.NavigateUrl = String.Format(sURL_FORMAT, TypeConvert.ToString(rdr[sURL_FIELD]));
                            }
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Image", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        Image imgField = new Image();

                        imgField.ID = sDATA_FIELD;
                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                        else if (rdr != null)
                        {
                            if (!TypeConvert.IsEmptyString(rdr[sDATA_FIELD]))
                            {
                                imgField.ImageUrl = "~/CRM/Images/Image.aspx?ID=" + TypeConvert.ToString(rdr[sDATA_FIELD]);

                                tdField.Controls.Add(imgField);
                            }
                        }
                    }
                }
                else
                {
                    Literal litField = new Literal();
                    tdField.Controls.Add(litField);
                    litField.Text = "Unknown field type " + sFIELD_TYPE;
                }

                if (nCOLSPAN > 0)
                    nColIndex += nCOLSPAN;
                else if (nCOLSPAN == 0)
                    nColIndex++;
                if (nColIndex >= nDATA_COLUMNS)
                    nColIndex = 0;
            }
        }

        public static void AppendEditViewFields(string sEDIT_NAME, HtmlTable tbl, System.Data.SqlClient.SqlDataReader rdr)
        {
            if (tbl == null)
            {
                return;
            }
            tbl.Attributes.Add("name", sEDIT_NAME);
            DataView dvFields = CRMCache.EditViewFields(sEDIT_NAME).DefaultView;
            try
            {
                dvFields.Sort = "FIELD_INDEX";
            }
            catch
            {
            }
            AppendEditViewFields(dvFields, tbl, rdr, null, false, sEDIT_NAME);
        }

        public static void ValidateEditViewFields(string sEDIT_NAME, Control parent)
        {

            bool bEnableTeamManagement = Common.Config.enable_team_management();
            bool bRequireTeamManagement = Common.Config.require_team_management();

            bool bRequireUserAssignment = Common.Config.require_user_assignment();
            DataTable dtFields = CRMCache.EditViewFields(sEDIT_NAME);
            DataView dvFields = new DataView(dtFields);

            dvFields.RowFilter = "UI_REQUIRED = 1 or UI_VALIDATOR = 1 or DATA_FIELD in ('TEAM_ID', 'ASSIGNED_USER_ID')";
            foreach (DataRowView row in dvFields)
            {
                string sFIELD_TYPE = TypeConvert.ToString(row["FIELD_TYPE"]);
                string sDATA_FIELD = TypeConvert.ToString(row["DATA_FIELD"]);
                bool bUI_REQUIRED = TypeConvert.ToBoolean(row["UI_REQUIRED"]);
                bool bUI_VALIDATOR = TypeConvert.ToBoolean(row["UI_VALIDATOR"]);
                if (sDATA_FIELD == "TEAM_ID")
                {

                    if (!bEnableTeamManagement)
                    {

                        bUI_REQUIRED = false;
                    }
                    else
                    {

                        if (bRequireTeamManagement)
                            bUI_REQUIRED = true;
                    }
                }
                if (sDATA_FIELD == "ASSIGNED_USER_ID")
                {

                    if (bRequireUserAssignment)
                        bUI_REQUIRED = true;
                }
                if (bUI_REQUIRED && !TypeConvert.IsEmptyString(sDATA_FIELD))
                {
                    if (String.Compare(sFIELD_TYPE, "DateRange", true) == 0)
                    {

                        DatePicker ctlDateStart = parent.FindControl(sDATA_FIELD + "_AFTER") as DatePicker;
                        if (ctlDateStart != null)
                        {
                            if (ctlDateStart.Visible)
                                ctlDateStart.Validate();
                        }
                        DatePicker ctlDateEnd = parent.FindControl(sDATA_FIELD + "_BEFORE") as DatePicker;
                        if (ctlDateEnd != null)
                        {
                            if (ctlDateEnd.Visible)
                                ctlDateEnd.Validate();
                        }
                    }
                    else if (String.Compare(sFIELD_TYPE, "DatePicker", true) == 0)
                    {
                        DatePicker ctlDate = parent.FindControl(sDATA_FIELD) as DatePicker;
                        if (ctlDate != null)
                        {

                            if (ctlDate.Visible)
                                ctlDate.Validate();
                        }
                    }
                    else if (String.Compare(sFIELD_TYPE, "DateTimePicker", true) == 0)
                    {
                        DateTimePicker ctlDate = parent.FindControl(sDATA_FIELD) as DateTimePicker;
                        if (ctlDate != null)
                        {

                            if (ctlDate.Visible)
                                ctlDate.Validate();
                        }
                    }
                    else if (String.Compare(sFIELD_TYPE, "DateTimeEdit", true) == 0)
                    {
                        DateTimeEdit ctlDate = parent.FindControl(sDATA_FIELD) as DateTimeEdit;
                        if (ctlDate != null)
                        {

                            if (ctlDate.Visible)
                                ctlDate.Validate();
                        }
                    }
                    else
                    {
                        Control ctl = parent.FindControl(sDATA_FIELD);
                        if (ctl != null)
                        {

                            if (ctl.Visible)
                            {
                                BaseValidator req = parent.FindControl(sDATA_FIELD + "_REQUIRED") as BaseValidator;
                                if (req != null)
                                {

                                    req.Enabled = true;
                                    req.Validate();
                                }
                            }
                        }
                    }
                }
                if (bUI_VALIDATOR)
                {
                    Control ctl = parent.FindControl(sDATA_FIELD);
                    if (ctl != null)
                    {

                        if (ctl.Visible)
                        {
                            BaseValidator req = parent.FindControl(sDATA_FIELD + "_VALIDATOR") as BaseValidator;
                            if (req != null)
                            {

                                req.Enabled = true;
                                req.Validate();
                            }
                        }
                    }
                }
            }
        }


        public static void ListControl_DataBound_AllowNull(object sender, EventArgs e)
        {
            ListControl lst = sender as ListControl;
            if (lst != null)
            {
                CRMPage page = lst.Page as CRMPage;
                if (page != null)
                {
                    lst.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
                }
                else
                {
                    lst.Items.Insert(0, new ListItem("", ""));
                }
            }
        }

        public static void AppendEditViewFields(DataView dvFields, HtmlTable tbl, System.Data.SqlClient.SqlDataReader rdr, CommandEventHandler Page_Command, bool bLayoutMode, string sEDIT_NAME)
        {
            bool bIsMobile = false;
            CRMPage Page = tbl.Page as CRMPage;
            if (Page != null)
                bIsMobile = Page.IsMobile;

            HtmlTableRow tr = null;

            int nRowIndex = tbl.Rows.Count - 1;
            int nColIndex = 0;
            HtmlTableCell tdLabel = null;
            HtmlTableCell tdField = null;

            if (bLayoutMode)
                tbl.Border = 1;

            if (dvFields.Count == 0 && tbl.Rows.Count <= 1)
                tbl.Visible = false;


            DataView vwSchema = null;
            if (rdr != null)
                vwSchema = new DataView(rdr.GetSchemaTable());


            bool bEnableTeamManagement = Common.Config.enable_team_management();
            bool bRequireTeamManagement = Common.Config.require_team_management();

            bool bRequireUserAssignment = Common.Config.require_user_assignment();
            HttpSessionState Session = HttpContext.Current.Session;
            foreach (DataRowView row in dvFields)
            {
                int gID = TypeConvert.ToInteger(row["ID"]);
                int nFIELD_INDEX = TypeConvert.ToInteger(row["FIELD_INDEX"]);
                string sFIELD_TYPE = TypeConvert.ToString(row["FIELD_TYPE"]);
                string sDATA_LABEL = TypeConvert.ToString(row["DATA_LABEL"]);
                string sDATA_FIELD = TypeConvert.ToString(row["DATA_FIELD"]);
                string sDISPLAY_FIELD = TypeConvert.ToString(row["DISPLAY_FIELD"]);
                string sCACHE_NAME = TypeConvert.ToString(row["CACHE_NAME"]);
                bool bDATA_REQUIRED = TypeConvert.ToBoolean(row["DATA_REQUIRED"]);
                bool bUI_REQUIRED = TypeConvert.ToBoolean(row["UI_REQUIRED"]);
                string sONCLICK_SCRIPT = TypeConvert.ToString(row["ONCLICK_SCRIPT"]);
                string sFORMAT_SCRIPT = TypeConvert.ToString(row["FORMAT_SCRIPT"]);
                short nFORMAT_TAB_INDEX = TypeConvert.ToShort(row["FORMAT_TAB_INDEX"]);
                int nFORMAT_MAX_LENGTH = TypeConvert.ToInteger(row["FORMAT_MAX_LENGTH"]);
                int nFORMAT_SIZE = TypeConvert.ToInteger(row["FORMAT_SIZE"]);
                int nFORMAT_ROWS = TypeConvert.ToInteger(row["FORMAT_ROWS"]);
                int nFORMAT_COLUMNS = TypeConvert.ToInteger(row["FORMAT_COLUMNS"]);
                int nCOLSPAN = TypeConvert.ToInteger(row["COLSPAN"]);
                int nROWSPAN = TypeConvert.ToInteger(row["ROWSPAN"]);
                string sLABEL_WIDTH = TypeConvert.ToString(row["LABEL_WIDTH"]);
                string sFIELD_WIDTH = TypeConvert.ToString(row["FIELD_WIDTH"]);
                int nDATA_COLUMNS = TypeConvert.ToInteger(row["DATA_COLUMNS"]);

                string sFIELD_VALIDATOR_MESSAGE = TypeConvert.ToString(row["FIELD_VALIDATOR_MESSAGE"]);
                string sVALIDATION_TYPE = TypeConvert.ToString(row["VALIDATION_TYPE"]);
                string sREGULAR_EXPRESSION = TypeConvert.ToString(row["REGULAR_EXPRESSION"]);
                string sDATA_TYPE = TypeConvert.ToString(row["DATA_TYPE"]);
                string sMININUM_VALUE = TypeConvert.ToString(row["MININUM_VALUE"]);
                string sMAXIMUM_VALUE = TypeConvert.ToString(row["MAXIMUM_VALUE"]);
                string sCOMPARE_OPERATOR = TypeConvert.ToString(row["COMPARE_OPERATOR"]);


                if (nDATA_COLUMNS == 0)
                    nDATA_COLUMNS = 2;

                if (!bLayoutMode && sDATA_FIELD == "TEAM_ID")
                {
                    if (!bEnableTeamManagement)
                    {
                        sFIELD_TYPE = "Blank";
                        bUI_REQUIRED = false;
                    }
                    else
                    {

                        if (bRequireTeamManagement)
                            bUI_REQUIRED = true;
                    }
                }
                if (!bLayoutMode && sDATA_FIELD == "ASSIGNED_USER_ID")
                {

                    if (bRequireUserAssignment)
                        bUI_REQUIRED = true;
                }
                if (bIsMobile && String.Compare(sFIELD_TYPE, "AddressButtons", true) == 0)
                {

                    continue;
                }

                if ((nCOLSPAN >= 0 && nColIndex == 0) || tr == null || bIsMobile)
                {

                    nRowIndex++;
                    tr = new HtmlTableRow();
                    tbl.Rows.Insert(nRowIndex, tr);
                }
                if (bLayoutMode)
                {
                    HtmlTableCell tdAction = new HtmlTableCell();
                    tr.Cells.Add(tdAction);
                    tdAction.Attributes.Add("class", "tabDetailViewDL");
                    tdAction.NoWrap = true;

                    Literal litIndex = new Literal();
                    tdAction.Controls.Add(litIndex);
                    litIndex.Text = " " + nFIELD_INDEX.ToString() + " ";


                    ImageButton btnMoveUp = CreateLayoutImageButtonSkin(gID, "Layout.MoveUp", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_UP"), "uparrow_inline", Page_Command);
                    ImageButton btnMoveDown = CreateLayoutImageButtonSkin(gID, "Layout.MoveDown", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_DOWN"), "downarrow_inline", Page_Command);
                    ImageButton btnInsert = CreateLayoutImageButtonSkin(gID, "Layout.Insert", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_INS"), "plus_inline", Page_Command);
                    ImageButton btnEdit = CreateLayoutImageButtonSkin(gID, "Layout.Edit", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_EDIT"), "edit_inline", Page_Command);
                    ImageButton btnDelete = CreateLayoutImageButtonSkin(gID, "Layout.Delete", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_DELETE"), "delete_inline", Page_Command);
                    tdAction.Controls.Add(btnMoveUp);
                    tdAction.Controls.Add(btnMoveDown);
                    tdAction.Controls.Add(btnInsert);
                    tdAction.Controls.Add(btnEdit);
                    tdAction.Controls.Add(btnDelete);
                }

                Literal litLabel = new Literal();
                if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    litLabel.ID = sDATA_FIELD + "_LABEL";
                if (nCOLSPAN >= 0 || tdLabel == null || tdField == null)
                {
                    tdLabel = new HtmlTableCell();
                    tdField = new HtmlTableCell();
                    tr.Cells.Add(tdLabel);
                    tr.Cells.Add(tdField);
                    if (nCOLSPAN > 0)
                    {
                        tdField.ColSpan = nCOLSPAN;
                        if (bLayoutMode)
                            tdField.ColSpan++;
                    }
                    tdLabel.Attributes.Add("class", "dataLabel");
                    tdLabel.VAlign = "top";
                    tdLabel.Width = sLABEL_WIDTH;
                    tdField.Attributes.Add("class", "dataField");
                    tdField.VAlign = "top";
                    if (nCOLSPAN == 0)
                        tdField.Width = sFIELD_WIDTH;

                    tdLabel.Controls.Add(litLabel);

                    if (bLayoutMode)
                        litLabel.Text = sDATA_LABEL;
                    else if (sDATA_LABEL.IndexOf(".") >= 0)
                        litLabel.Text = Translation.GetTranslation.Term(sDATA_LABEL);
                    else if (!TypeConvert.IsEmptyString(sDATA_LABEL) && rdr != null)
                    {

                        litLabel.Text = sDATA_LABEL;
                        if (vwSchema != null)
                        {
                            vwSchema.RowFilter = "ColumnName = '" + TypeConvert.EscapeSQL(sDATA_LABEL) + "'";
                            if (vwSchema.Count > 0)
                                litLabel.Text = TypeConvert.ToString(rdr[sDATA_LABEL]) + Translation.GetTranslation.Term("Calls.LBL_COLON");
                        }
                    }

                    else
                        litLabel.Text = sDATA_LABEL;  // "&nbsp;";

                    if (!bLayoutMode && bUI_REQUIRED)
                    {
                        Label lblRequired = new Label();
                        tdLabel.Controls.Add(lblRequired);
                        lblRequired.CssClass = "required";
                        lblRequired.Text = Translation.GetTranslation.Term(".LBL_REQUIRED_SYMBOL");
                    }
                }

                if (String.Compare(sFIELD_TYPE, "Blank", true) == 0)
                {
                    Literal litField = new Literal();
                    tdField.Controls.Add(litField);
                    if (bLayoutMode)
                    {
                        litLabel.Text = "*** BLANK ***";
                        litField.Text = "*** BLANK ***";
                    }
                    else
                    {

                        litLabel.Text = "&nbsp;";
                        litField.Text = "&nbsp;";
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Label", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        Literal litField = new Literal();
                        tdField.Controls.Add(litField);

                        tdField.VAlign = "middle";

                        litField.ID = sDATA_FIELD;
                        if (bLayoutMode)
                            litField.Text = sDATA_FIELD;
                        else if (sDATA_FIELD.IndexOf(".") >= 0)
                            litField.Text = Translation.GetTranslation.Term(sDATA_FIELD);
                        else if (rdr != null)
                            litField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "ListBox", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        ListControl lstField = null;
                        if (nFORMAT_ROWS > 0)
                        {
                            ListBox lb = new ListBox();
                            lb.SelectionMode = ListSelectionMode.Multiple;
                            lb.Rows = nFORMAT_ROWS;
                            lstField = lb;
                        }
                        else
                        {

                            lstField = new KeySortDropDownList();
                        }
                        tdField.Controls.Add(lstField);
                        lstField.ID = sDATA_FIELD;
                        lstField.TabIndex = nFORMAT_TAB_INDEX;

                        if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                        {

                            if (!TypeConvert.IsEmptyString(sCACHE_NAME) && (bLayoutMode || !tbl.Page.IsPostBack))
                            {
                                lstField.DataValueField = "NAME";
                                lstField.DataTextField = "DISPLAY_NAME";
                                lstField.DataSource = CRMCache.List(sCACHE_NAME);

                                if (sCACHE_NAME != "lead_status_dom")
                                {
                                    if (sCACHE_NAME != "Currencies")
                                    {
                                        lstField.Attributes.Add("class", "dropdown");
                                    }
                                }

                                if (sCACHE_NAME != "program_plan" || sCACHE_NAME != "Currencies")
                                {
                                    if (sCACHE_NAME != "Currencies")
                                    {
                                        lstField.Attributes.Add("dom", sCACHE_NAME);
                                    }
                                }
                                lstField.DataBind();

                                if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                                    lstField.Attributes.Add("onchange", sONCLICK_SCRIPT);

                                if (!bUI_REQUIRED)
                                {
                                    lstField.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));
                                    lstField.DataBound += new EventHandler(ListControl_DataBound_AllowNull);
                                }
                            }
                            if (rdr != null)
                            {
                                string sVALUE = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                if (nFORMAT_ROWS > 0 && sVALUE.StartsWith("<?xml"))
                                {
                                    XmlDocument xml = new XmlDocument();
                                    xml.LoadXml(sVALUE);
                                    XmlNodeList nlValues = xml.DocumentElement.SelectNodes("Value");
                                    foreach (XmlNode xValue in nlValues)
                                    {
                                        foreach (ListItem item in lstField.Items)
                                        {
                                            if (item.Value == xValue.InnerText)
                                                item.Selected = true;
                                        }
                                    }
                                }
                                else
                                {
                                    lstField.SelectedValue = sVALUE;
                                }
                            }

                            else if (rdr == null && !tbl.Page.IsPostBack && sCACHE_NAME == "AssignedUser")
                            {
                                if (nFORMAT_ROWS == 0)
                                    lstField.SelectedValue = CRMSecurity.USER_ID.ToString();
                            }
                        }

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "CheckBox", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        CheckBox chkField = new CheckBox();
                        tdField.Controls.Add(chkField);
                        chkField.ID = sDATA_FIELD;
                        chkField.CssClass = "checkbox";
                        chkField.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            chkField.Checked = TypeConvert.ToBoolean(rdr[sDATA_FIELD]);

                        if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                            chkField.Attributes.Add("onclick", sONCLICK_SCRIPT);
                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                            chkField.Enabled = false;
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "ChangeButton", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        if (sDATA_LABEL == "PARENT_TYPE")
                        {
                            tdLabel.Controls.Clear();

                            DropDownList lstField = new KeySortDropDownList();
                            tdLabel.Controls.Add(lstField);
                            lstField.ID = sDATA_LABEL;
                            lstField.TabIndex = nFORMAT_TAB_INDEX;
                            lstField.Attributes.Add("onChange", "ChangeParentType();");


                            if (bLayoutMode || !tbl.Page.IsPostBack)
                            {

                                lstField.DataValueField = "NAME";
                                lstField.DataTextField = "DISPLAY_NAME";

                                lstField.DataSource = CRMCache.List("record_type_display");
                                lstField.DataBind();
                                if (rdr != null)
                                {
                                    lstField.ClearSelection();
                                    lstField.SelectedValue = TypeConvert.ToString(rdr[sDATA_LABEL]);
                                }
                            }
                        }
                        TextBox txtNAME = new TextBox();
                        tdField.Controls.Add(txtNAME);
                        txtNAME.ID = sDISPLAY_FIELD;
                        txtNAME.ReadOnly = true;
                        txtNAME.TabIndex = nFORMAT_TAB_INDEX;

                        txtNAME.EnableViewState = false;

                        if (bLayoutMode)
                        {
                            txtNAME.Text = sDISPLAY_FIELD;
                            txtNAME.Enabled = false;
                        }

                        else if (tbl.Page.IsPostBack)
                        {

                            if (tbl.Page.Request[txtNAME.UniqueID] != null)
                                txtNAME.Text = TypeConvert.ToString(tbl.Page.Request[txtNAME.UniqueID]);
                        }
                        else if (!TypeConvert.IsEmptyString(sDISPLAY_FIELD) && rdr != null)
                            txtNAME.Text = TypeConvert.ToString(rdr[sDISPLAY_FIELD]);

                        else if (sDATA_FIELD == "TEAM_ID" && rdr == null && !tbl.Page.IsPostBack)
                            txtNAME.Text = CRMSecurity.TEAM_NAME;
                        else if (sDATA_FIELD == "ASSIGNED_USER_ID" && rdr == null && !tbl.Page.IsPostBack)
                            txtNAME.Text = CRMSecurity.USER_NAME;

                        HtmlInputHidden hidID = new HtmlInputHidden();
                        tdField.Controls.Add(hidID);
                        hidID.ID = sDATA_FIELD;
                        if (!bLayoutMode)
                        {
                            if (!TypeConvert.IsEmptyString(sDATA_FIELD) && rdr != null)
                            {
                                hidID.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                hidID.Value = (TypeConvert.ToGuid(hidID.Value) == Guid.Empty) ? "" : hidID.Value;
                            }

                            else if (sDATA_FIELD == "TEAM_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.TEAM_ID.ToString();

                            else if (sDATA_FIELD == "ASSIGNED_USER_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.USER_ID.ToString();
                        }
                        if (!String.IsNullOrEmpty(CRMSecurity.TEAM_NAME))
                        {
                            txtNAME.Text = CRMSecurity.TEAM_NAME;
                        }
                        else
                        {
                            InlineQueryDBManager oQuery = new InlineQueryDBManager();
                            oQuery.CommandText = "select Teams.name,Teams.id from Teams where id in (select t.team_ID from users u "
                                               + "inner join Team_Memberships t on u.id = t.USER_ID "
                                               + "and u.id ='" + Security.USER_ID.ToString() + "')";
                            using (DataTable dt = oQuery.GetTable())
                            {
                                if (dt.Rows.Count > 0 && (sDATA_FIELD != "ASSIGNED_USER_ID" && rdr == null && !tbl.Page.IsPostBack))
                                {
                                    txtNAME.Text = dt.Rows[0]["name"].ToString();
                                    hidID.Value = dt.Rows[0]["id"].ToString();
                                }
                            }
                        }
                        Literal litNBSP = new Literal();
                        tdField.Controls.Add(litNBSP);
                        litNBSP.Text = "&nbsp;";

                        HtmlInputButton btnChange = new HtmlInputButton("button");
                        tdField.Controls.Add(btnChange);

                        btnChange.ID = sDATA_FIELD + "_btnChange";
                        btnChange.Attributes.Add("class", "button");
                        if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                            btnChange.Attributes.Add("onclick", sONCLICK_SCRIPT);

                        btnChange.Attributes.Add("title", Translation.GetTranslation.Term(".LBL_SELECT_BUTTON_TITLE"));

                        btnChange.Value = Translation.GetTranslation.Term(".LBL_SELECT_BUTTON_LABEL");


                        if (sONCLICK_SCRIPT.IndexOf("Popup();") > 0)
                        {
                            litNBSP = new Literal();
                            tdField.Controls.Add(litNBSP);
                            litNBSP.Text = "&nbsp;";

                            HtmlInputButton btnClear = new HtmlInputButton("button");
                            tdField.Controls.Add(btnClear);
                            btnClear.ID = sDATA_FIELD + "_btnClear";
                            btnClear.Attributes.Add("class", "button");
                            btnClear.Attributes.Add("onclick", sONCLICK_SCRIPT.Replace("Popup();", "('', '');").Replace("return ", "return Change"));
                            btnClear.Attributes.Add("title", Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_TITLE"));
                            btnClear.Value = Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_LABEL");
                        }
                        if (!bLayoutMode && bUI_REQUIRED && !TypeConvert.IsEmptyString(sDATA_FIELD))
                        {
                            RequiredFieldValidatorForHiddenInputs reqID = new RequiredFieldValidatorForHiddenInputs();
                            reqID.ID = sDATA_FIELD + "_REQUIRED";
                            reqID.ControlToValidate = hidID.ID;
                            reqID.ErrorMessage = Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD");
                            reqID.CssClass = "required";
                            reqID.EnableViewState = false;

                            reqID.EnableClientScript = false;
                            reqID.Enabled = false;

                            reqID.Style.Add("padding-left", "4px");
                            tdField.Controls.Add(reqID);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "TextBox", true) == 0 || String.Compare(sFIELD_TYPE, "Password", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        TextBox txtField = new TextBox();
                        tdField.Controls.Add(txtField);
                        txtField.ID = sDATA_FIELD;
                        txtField.TabIndex = nFORMAT_TAB_INDEX;
                        if (nFORMAT_ROWS > 0 && nFORMAT_COLUMNS > 0)
                        {
                            txtField.Rows = nFORMAT_ROWS;
                            txtField.Columns = nFORMAT_COLUMNS;
                            txtField.TextMode = TextBoxMode.MultiLine;
                        }
                        else
                        {
                            txtField.MaxLength = nFORMAT_MAX_LENGTH;
                            txtField.Attributes.Add("size", nFORMAT_SIZE.ToString());
                            txtField.TextMode = TextBoxMode.SingleLine;
                        }
                        if (bLayoutMode)
                        {
                            txtField.Text = sDATA_FIELD;
                            txtField.ReadOnly = true;
                        }
                        else if (!TypeConvert.IsEmptyString(sDATA_FIELD) && rdr != null)
                        {
                            int nOrdinal = rdr.GetOrdinal(sDATA_FIELD);
                            string sTypeName = rdr.GetDataTypeName(nOrdinal);

                            if (sTypeName == "money" || rdr[sDATA_FIELD].GetType() == typeof(System.Decimal))
                                txtField.Text = TypeConvert.ToDecimal(rdr[sDATA_FIELD]).ToString("#,##0.00");
                            else
                                txtField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                        }

                        if (String.Compare(sFIELD_TYPE, "Password", true) == 0)
                            txtField.TextMode = TextBoxMode.Password;
                        if (!bLayoutMode && bUI_REQUIRED && !TypeConvert.IsEmptyString(sDATA_FIELD))
                        {
                            RequiredFieldValidator reqNAME = new RequiredFieldValidator();
                            reqNAME.ID = sDATA_FIELD + "_REQUIRED";
                            reqNAME.ControlToValidate = txtField.ID;
                            reqNAME.ErrorMessage = Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD");
                            reqNAME.CssClass = "required";
                            reqNAME.EnableViewState = false;

                            reqNAME.EnableClientScript = false;
                            reqNAME.Enabled = false;
                            reqNAME.Style.Add("padding-left", "4px");
                            tdField.Controls.Add(reqNAME);
                        }
                        if (!bLayoutMode && !TypeConvert.IsEmptyString(sDATA_FIELD))
                        {
                            if (sVALIDATION_TYPE == "RegularExpressionValidator" && !TypeConvert.IsEmptyString(sREGULAR_EXPRESSION) && !TypeConvert.IsEmptyString(sFIELD_VALIDATOR_MESSAGE))
                            {
                                RegularExpressionValidator reqVALIDATOR = new RegularExpressionValidator();
                                reqVALIDATOR.ID = sDATA_FIELD + "_VALIDATOR";
                                reqVALIDATOR.ControlToValidate = txtField.ID;
                                reqVALIDATOR.ErrorMessage = Translation.GetTranslation.Term(sFIELD_VALIDATOR_MESSAGE);
                                reqVALIDATOR.ValidationExpression = sREGULAR_EXPRESSION;
                                reqVALIDATOR.CssClass = "required";
                                reqVALIDATOR.EnableViewState = false;

                                reqVALIDATOR.EnableClientScript = false;
                                reqVALIDATOR.Enabled = false;
                                reqVALIDATOR.Style.Add("padding-left", "4px");
                                tdField.Controls.Add(reqVALIDATOR);
                            }
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DatePicker", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        DatePicker ctlDate = tbl.Page.LoadControl("~/CRM/UserControls/DatePicker.ascx") as DatePicker;
                        tdField.Controls.Add(ctlDate);
                        ctlDate.ID = sDATA_FIELD;

                        ctlDate.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateRange", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        Table tblDateRange = new Table();
                        tdField.Controls.Add(tblDateRange);
                        TableRow trAfter = new TableRow();
                        TableRow trBefore = new TableRow();
                        tblDateRange.Rows.Add(trAfter);
                        tblDateRange.Rows.Add(trBefore);
                        TableCell tdAfterLabel = new TableCell();
                        TableCell tdAfterData = new TableCell();
                        TableCell tdBeforeLabel = new TableCell();
                        TableCell tdBeforeData = new TableCell();
                        trAfter.Cells.Add(tdAfterLabel);
                        trAfter.Cells.Add(tdAfterData);
                        trBefore.Cells.Add(tdBeforeLabel);
                        trBefore.Cells.Add(tdBeforeData);


                        DatePicker ctlDateStart = tbl.Page.LoadControl("~/CRM/UserControls/DatePicker.ascx") as DatePicker;
                        DatePicker ctlDateEnd = tbl.Page.LoadControl("~/CRM/UserControls/DatePicker.ascx") as DatePicker;
                        Literal litAfterLabel = new Literal();
                        Literal litBeforeLabel = new Literal();
                        litAfterLabel.Text = Translation.GetTranslation.Term("SavedSearch.LBL_SEARCH_AFTER");
                        litBeforeLabel.Text = Translation.GetTranslation.Term("SavedSearch.LBL_SEARCH_BEFORE");

                        tdAfterLabel.Controls.Add(litAfterLabel);
                        tdAfterData.Controls.Add(ctlDateStart);
                        tdBeforeLabel.Controls.Add(litBeforeLabel);
                        tdBeforeData.Controls.Add(ctlDateEnd);

                        ctlDateStart.ID = sDATA_FIELD + "_AFTER";
                        ctlDateEnd.ID = sDATA_FIELD + "_BEFORE";

                        ctlDateStart.TabIndex = nFORMAT_TAB_INDEX;
                        ctlDateEnd.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                        {
                            ctlDateStart.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);
                            ctlDateEnd.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);
                        }

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateTimePicker", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        DateTimePicker ctlDate = tbl.Page.LoadControl("~/CRM/UserControls/DateTimePicker.ascx") as DateTimePicker;
                        tdField.Controls.Add(ctlDate);
                        ctlDate.ID = sDATA_FIELD;

                        ctlDate.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateTimeEdit", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        DateTimeEdit ctlDate = tbl.Page.LoadControl("~/CRM/UserControls/DateTimeEdit.ascx") as DateTimeEdit;
                        tdField.Controls.Add(ctlDate);
                        ctlDate.ID = sDATA_FIELD;

                        ctlDate.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);

                        if (!bLayoutMode && bUI_REQUIRED)
                        {
                            ctlDate.EnableNone = false;
                        }
                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "File", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        HtmlInputFile ctlField = new HtmlInputFile();
                        tdField.Controls.Add(ctlField);
                        ctlField.ID = sDATA_FIELD;
                        ctlField.MaxLength = nFORMAT_MAX_LENGTH;
                        ctlField.Size = nFORMAT_SIZE;
                        ctlField.Attributes.Add("TabIndex", nFORMAT_TAB_INDEX.ToString());
                        if (!bLayoutMode && bUI_REQUIRED)
                        {
                            RequiredFieldValidator reqNAME = new RequiredFieldValidator();
                            reqNAME.ID = sDATA_FIELD + "_REQUIRED";
                            reqNAME.ControlToValidate = ctlField.ID;
                            reqNAME.ErrorMessage = Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD");
                            reqNAME.CssClass = "required";
                            reqNAME.EnableViewState = false;

                            reqNAME.EnableClientScript = false;
                            reqNAME.Enabled = false;
                            reqNAME.Style.Add("padding-left", "4px");
                            tdField.Controls.Add(reqNAME);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Image", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        HtmlInputHidden ctlHidden = new HtmlInputHidden();
                        if (!bLayoutMode)
                        {
                            tdField.Controls.Add(ctlHidden);
                            ctlHidden.ID = sDATA_FIELD;

                            HtmlInputFile ctlField = new HtmlInputFile();
                            tdField.Controls.Add(ctlField);

                            ctlField.ID = sDATA_FIELD + "_File";

                            Literal litBR = new Literal();
                            litBR.Text = "<br />";
                            tdField.Controls.Add(litBR);
                        }

                        Image imgField = new Image();

                        imgField.ID = "img" + sDATA_FIELD;
                        try
                        {
                            if (bLayoutMode)
                            {
                                Literal litField = new Literal();
                                litField.Text = sDATA_FIELD;
                                tdField.Controls.Add(litField);
                            }
                            else if (rdr != null)
                            {
                                if (!TypeConvert.IsEmptyString(rdr[sDATA_FIELD]))
                                {
                                    ctlHidden.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                    imgField.ImageUrl = "~/CRM/Images/Image.aspx?ID=" + ctlHidden.Value;

                                    tdField.Controls.Add(imgField);


                                    Literal litClear = new Literal();
                                    litClear.Text = "<br /><input type=\"button\" class=\"button\" onclick=\"form." + ctlHidden.ClientID + ".value='';form." + imgField.ClientID + ".src='';" + "\"  value='" + "  " + Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_LABEL") + "  " + "' title='" + Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_TITLE") + "' />";
                                    tdField.Controls.Add(litClear);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Literal litField = new Literal();
                            litField.Text = ex.Message;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "AddressButtons", true) == 0)
                {
                    tr.Cells.Remove(tdField);
                    tdLabel.Width = "10%";
                    tdLabel.RowSpan = nROWSPAN;
                    tdLabel.VAlign = "middle";
                    tdLabel.Align = "center";
                    tdLabel.Attributes.Remove("class");
                    tdLabel.Attributes.Add("class", "tabFormAddDel");
                    HtmlInputButton btnCopyRight = new HtmlInputButton("button");
                    Literal litSpacer = new Literal();
                    HtmlInputButton btnCopyLeft = new HtmlInputButton("button");
                    tdLabel.Controls.Add(btnCopyRight);
                    tdLabel.Controls.Add(litSpacer);
                    tdLabel.Controls.Add(btnCopyLeft);
                    btnCopyRight.Attributes.Add("title", Translation.GetTranslation.Term("Accounts.NTC_COPY_BILLING_ADDRESS"));
                    btnCopyRight.Attributes.Add("onclick", "return copyAddressRight()");
                    btnCopyRight.Value = ">>";
                    litSpacer.Text = "<br><br>";
                    btnCopyLeft.Attributes.Add("title", Translation.GetTranslation.Term("Accounts.NTC_COPY_SHIPPING_ADDRESS"));
                    btnCopyLeft.Attributes.Add("onclick", "return copyAddressLeft()");
                    btnCopyLeft.Value = "<<";
                    nColIndex = 0;
                }
                else if (String.Compare(sFIELD_TYPE, "Hidden", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        HtmlInputHidden hidID = new HtmlInputHidden();
                        tdField.Controls.Add(hidID);
                        hidID.ID = sDATA_FIELD;
                        if (bLayoutMode)
                        {
                            TextBox txtNAME = new TextBox();
                            tdField.Controls.Add(txtNAME);
                            txtNAME.ReadOnly = true;

                            txtNAME.EnableViewState = false;
                            txtNAME.Text = sDATA_FIELD;
                            txtNAME.Enabled = false;
                        }
                        else
                        {

                            nCOLSPAN = -1;
                            tr.Cells.Remove(tdLabel);
                            tdField.Attributes.Add("style", "display:none");
                            if (!TypeConvert.IsEmptyString(sDATA_FIELD) && rdr != null)
                                hidID.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);

                            else if (sDATA_FIELD == "TEAM_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.TEAM_ID.ToString();

                            else if (sDATA_FIELD == "ASSIGNED_USER_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.USER_ID.ToString();
                        }
                    }
                }
                else
                {
                    Literal litField = new Literal();
                    tdField.Controls.Add(litField);
                    litField.Text = "Unknown field type " + sFIELD_TYPE;
                }

                if (nCOLSPAN > 0)
                    nColIndex += nCOLSPAN;
                else if (nCOLSPAN == 0)
                    nColIndex++;
                if (nColIndex >= nDATA_COLUMNS)
                    nColIndex = 0;
            }
        }

        public static void AppendEditViewFields(DataView dvFields, HtmlTable tbl, System.Data.SqlClient.SqlDataReader rdr, CommandEventHandler Page_Command, bool bLayoutMode)
        {
            bool bIsMobile = false;
            CRMPage Page = tbl.Page as CRMPage;
            if (Page != null)
                bIsMobile = Page.IsMobile;

            HtmlTableRow tr = null;

            int nRowIndex = tbl.Rows.Count - 1;
            int nColIndex = 0;
            HtmlTableCell tdLabel = null;
            HtmlTableCell tdField = null;

            if (bLayoutMode)
                tbl.Border = 1;

            if (dvFields.Count == 0 && tbl.Rows.Count <= 1)
                tbl.Visible = false;


            DataView vwSchema = null;
            if (rdr != null)
                vwSchema = new DataView(rdr.GetSchemaTable());


            bool bEnableTeamManagement = Common.Config.enable_team_management();
            bool bRequireTeamManagement = Common.Config.require_team_management();

            bool bRequireUserAssignment = Common.Config.require_user_assignment();
            HttpSessionState Session = HttpContext.Current.Session;
            foreach (DataRowView row in dvFields)
            {
                int gID = TypeConvert.ToInteger(row["ID"]);
                int nFIELD_INDEX = TypeConvert.ToInteger(row["FIELD_INDEX"]);
                string sFIELD_TYPE = TypeConvert.ToString(row["FIELD_TYPE"]);
                string sDATA_LABEL = TypeConvert.ToString(row["DATA_LABEL"]);
                string sDATA_FIELD = TypeConvert.ToString(row["DATA_FIELD"]);
                string sDISPLAY_FIELD = TypeConvert.ToString(row["DISPLAY_FIELD"]);
                string sCACHE_NAME = TypeConvert.ToString(row["CACHE_NAME"]);
                bool bDATA_REQUIRED = TypeConvert.ToBoolean(row["DATA_REQUIRED"]);
                bool bUI_REQUIRED = TypeConvert.ToBoolean(row["UI_REQUIRED"]);
                string sONCLICK_SCRIPT = TypeConvert.ToString(row["ONCLICK_SCRIPT"]);
                string sFORMAT_SCRIPT = TypeConvert.ToString(row["FORMAT_SCRIPT"]);
                short nFORMAT_TAB_INDEX = TypeConvert.ToShort(row["FORMAT_TAB_INDEX"]);
                int nFORMAT_MAX_LENGTH = TypeConvert.ToInteger(row["FORMAT_MAX_LENGTH"]);
                int nFORMAT_SIZE = TypeConvert.ToInteger(row["FORMAT_SIZE"]);
                int nFORMAT_ROWS = TypeConvert.ToInteger(row["FORMAT_ROWS"]);
                int nFORMAT_COLUMNS = TypeConvert.ToInteger(row["FORMAT_COLUMNS"]);
                int nCOLSPAN = TypeConvert.ToInteger(row["COLSPAN"]);
                int nROWSPAN = TypeConvert.ToInteger(row["ROWSPAN"]);
                string sLABEL_WIDTH = TypeConvert.ToString(row["LABEL_WIDTH"]);
                string sFIELD_WIDTH = TypeConvert.ToString(row["FIELD_WIDTH"]);
                int nDATA_COLUMNS = TypeConvert.ToInteger(row["DATA_COLUMNS"]);

                string sFIELD_VALIDATOR_MESSAGE = TypeConvert.ToString(row["FIELD_VALIDATOR_MESSAGE"]);
                string sVALIDATION_TYPE = TypeConvert.ToString(row["VALIDATION_TYPE"]);
                string sREGULAR_EXPRESSION = TypeConvert.ToString(row["REGULAR_EXPRESSION"]);
                string sDATA_TYPE = TypeConvert.ToString(row["DATA_TYPE"]);
                string sMININUM_VALUE = TypeConvert.ToString(row["MININUM_VALUE"]);
                string sMAXIMUM_VALUE = TypeConvert.ToString(row["MAXIMUM_VALUE"]);
                string sCOMPARE_OPERATOR = TypeConvert.ToString(row["COMPARE_OPERATOR"]);


                if (nDATA_COLUMNS == 0)
                    nDATA_COLUMNS = 2;

                if (!bLayoutMode && sDATA_FIELD == "TEAM_ID")
                {
                    if (!bEnableTeamManagement)
                    {
                        sFIELD_TYPE = "Blank";
                        bUI_REQUIRED = false;
                    }
                    else
                    {

                        if (bRequireTeamManagement)
                            bUI_REQUIRED = true;
                    }
                }
                if (!bLayoutMode && sDATA_FIELD == "ASSIGNED_USER_ID")
                {

                    if (bRequireUserAssignment)
                        bUI_REQUIRED = true;
                }
                if (bIsMobile && String.Compare(sFIELD_TYPE, "AddressButtons", true) == 0)
                {

                    continue;
                }

                if ((nCOLSPAN >= 0 && nColIndex == 0) || tr == null || bIsMobile)
                {

                    nRowIndex++;
                    tr = new HtmlTableRow();
                    tbl.Rows.Insert(nRowIndex, tr);
                }
                if (bLayoutMode)
                {
                    HtmlTableCell tdAction = new HtmlTableCell();
                    tr.Cells.Add(tdAction);
                    tdAction.Attributes.Add("class", "tabDetailViewDL");
                    tdAction.NoWrap = true;

                    Literal litIndex = new Literal();
                    tdAction.Controls.Add(litIndex);
                    litIndex.Text = " " + nFIELD_INDEX.ToString() + " ";


                    ImageButton btnMoveUp = CreateLayoutImageButtonSkin(gID, "Layout.MoveUp", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_UP"), "uparrow_inline", Page_Command);
                    ImageButton btnMoveDown = CreateLayoutImageButtonSkin(gID, "Layout.MoveDown", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_DOWN"), "downarrow_inline", Page_Command);
                    ImageButton btnInsert = CreateLayoutImageButtonSkin(gID, "Layout.Insert", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_INS"), "plus_inline", Page_Command);
                    ImageButton btnEdit = CreateLayoutImageButtonSkin(gID, "Layout.Edit", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_EDIT"), "edit_inline", Page_Command);
                    ImageButton btnDelete = CreateLayoutImageButtonSkin(gID, "Layout.Delete", nFIELD_INDEX, Translation.GetTranslation.Term("Dropdown.LNK_DELETE"), "delete_inline", Page_Command);
                    tdAction.Controls.Add(btnMoveUp);
                    tdAction.Controls.Add(btnMoveDown);
                    tdAction.Controls.Add(btnInsert);
                    tdAction.Controls.Add(btnEdit);
                    tdAction.Controls.Add(btnDelete);
                }

                Literal litLabel = new Literal();
                if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    litLabel.ID = sDATA_FIELD + "_LABEL";
                if (nCOLSPAN >= 0 || tdLabel == null || tdField == null)
                {
                    tdLabel = new HtmlTableCell();
                    tdField = new HtmlTableCell();
                    tr.Cells.Add(tdLabel);
                    tr.Cells.Add(tdField);
                    if (nCOLSPAN > 0)
                    {
                        tdField.ColSpan = nCOLSPAN;
                        if (bLayoutMode)
                            tdField.ColSpan++;
                    }
                    tdLabel.Attributes.Add("class", "dataLabel");
                    tdLabel.VAlign = "top";
                    tdLabel.Width = sLABEL_WIDTH;
                    tdField.Attributes.Add("class", "dataField");
                    tdField.VAlign = "top";
                    if (nCOLSPAN == 0)
                        tdField.Width = sFIELD_WIDTH;

                    tdLabel.Controls.Add(litLabel);

                    if (bLayoutMode)
                        litLabel.Text = sDATA_LABEL;
                    else if (sDATA_LABEL.IndexOf(".") >= 0)
                        litLabel.Text = Translation.GetTranslation.Term(sDATA_LABEL);
                    else if (!TypeConvert.IsEmptyString(sDATA_LABEL) && rdr != null)
                    {

                        litLabel.Text = sDATA_LABEL;
                        if (vwSchema != null)
                        {
                            vwSchema.RowFilter = "ColumnName = '" + TypeConvert.EscapeSQL(sDATA_LABEL) + "'";
                            if (vwSchema.Count > 0)
                                litLabel.Text = TypeConvert.ToString(rdr[sDATA_LABEL]) + Translation.GetTranslation.Term("Calls.LBL_COLON");
                        }
                    }

                    else
                        litLabel.Text = sDATA_LABEL;  // "&nbsp;";

                    if (!bLayoutMode && bUI_REQUIRED)
                    {
                        Label lblRequired = new Label();
                        tdLabel.Controls.Add(lblRequired);
                        lblRequired.CssClass = "required";
                        lblRequired.Text = Translation.GetTranslation.Term(".LBL_REQUIRED_SYMBOL");
                    }
                }

                if (String.Compare(sFIELD_TYPE, "Blank", true) == 0)
                {
                    Literal litField = new Literal();
                    tdField.Controls.Add(litField);
                    if (bLayoutMode)
                    {
                        litLabel.Text = "*** BLANK ***";
                        litField.Text = "*** BLANK ***";
                    }
                    else
                    {

                        litLabel.Text = "&nbsp;";
                        litField.Text = "&nbsp;";
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Label", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        Literal litField = new Literal();
                        tdField.Controls.Add(litField);

                        tdField.VAlign = "middle";

                        litField.ID = sDATA_FIELD;
                        if (bLayoutMode)
                            litField.Text = sDATA_FIELD;
                        else if (sDATA_FIELD.IndexOf(".") >= 0)
                            litField.Text = Translation.GetTranslation.Term(sDATA_FIELD);
                        else if (rdr != null)
                            litField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "ListBox", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        ListControl lstField = null;
                        if (nFORMAT_ROWS > 0)
                        {
                            ListBox lb = new ListBox();
                            lb.SelectionMode = ListSelectionMode.Multiple;
                            lb.Rows = nFORMAT_ROWS;
                            lstField = lb;
                        }
                        else
                        {

                            lstField = new KeySortDropDownList();
                        }
                        tdField.Controls.Add(lstField);
                        lstField.ID = sDATA_FIELD;
                        lstField.TabIndex = nFORMAT_TAB_INDEX;

                        if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                        {

                            if (!TypeConvert.IsEmptyString(sCACHE_NAME) && (bLayoutMode || !tbl.Page.IsPostBack))
                            {
                                if (sCACHE_NAME == "program_plan")
                                {
                                    lstField.DataValueField = "program_plan_id";
                                    lstField.DataTextField = "program_plan_id";
                                }
                                else
                                {
                                    lstField.DataValueField = "NAME";
                                    lstField.DataTextField = "DISPLAY_NAME";
                                }
                                lstField.DataSource = CRMCache.List(sCACHE_NAME);
                                if (sCACHE_NAME != "lead_status_dom")
                                {
                                    if (sCACHE_NAME != "Currencies")
                                    {
                                        lstField.Attributes.Add("class", "dropdown");
                                    }
                                }
                                if (sCACHE_NAME != "program_plan")
                                {
                                    if (sCACHE_NAME != "Currencies")
                                    {
                                        lstField.Attributes.Add("dom", sCACHE_NAME);
                                    }
                                }
                                lstField.DataBind();

                                if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                                    lstField.Attributes.Add("onchange", sONCLICK_SCRIPT);

                                if (!bUI_REQUIRED)
                                {
                                    lstField.Items.Insert(0, new ListItem(Translation.GetTranslation.Term(".LBL_NONE"), ""));

                                    lstField.DataBound += new EventHandler(ListControl_DataBound_AllowNull);
                                }
                            }
                            if (rdr != null)
                            {
                                string sVALUE = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                if (nFORMAT_ROWS > 0 && sVALUE.StartsWith("<?xml"))
                                {
                                    XmlDocument xml = new XmlDocument();
                                    xml.LoadXml(sVALUE);
                                    XmlNodeList nlValues = xml.DocumentElement.SelectNodes("Value");
                                    foreach (XmlNode xValue in nlValues)
                                    {
                                        foreach (ListItem item in lstField.Items)
                                        {
                                            if (item.Value == xValue.InnerText)
                                                item.Selected = true;
                                        }
                                    }
                                }
                                else
                                {
                                    lstField.SelectedValue = sVALUE;
                                }
                            }

                            else if (rdr == null && !tbl.Page.IsPostBack && sCACHE_NAME == "AssignedUser")
                            {
                                if (nFORMAT_ROWS == 0)
                                    lstField.SelectedValue = CRMSecurity.USER_ID.ToString();
                            }
                        }

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "CheckBox", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        CheckBox chkField = new CheckBox();
                        tdField.Controls.Add(chkField);
                        chkField.ID = sDATA_FIELD;
                        chkField.CssClass = "checkbox";
                        chkField.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            chkField.Checked = TypeConvert.ToBoolean(rdr[sDATA_FIELD]);

                        if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                            chkField.Attributes.Add("onclick", sONCLICK_SCRIPT);
                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                            chkField.Enabled = false;
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "ChangeButton", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        if (sDATA_LABEL == "PARENT_TYPE")
                        {
                            tdLabel.Controls.Clear();

                            DropDownList lstField = new KeySortDropDownList();
                            tdLabel.Controls.Add(lstField);
                            lstField.ID = sDATA_LABEL;
                            lstField.TabIndex = nFORMAT_TAB_INDEX;
                            lstField.Attributes.Add("onChange", "ChangeParentType();");


                            if (bLayoutMode || !tbl.Page.IsPostBack)
                            {
                                if (sCACHE_NAME == "program_plan")
                                {
                                    lstField.DataValueField = "program_plan_id";
                                    lstField.DataTextField = "program_plan_id";
                                }
                                else
                                {
                                    lstField.DataValueField = "NAME";
                                    lstField.DataTextField = "DISPLAY_NAME";
                                }
                                lstField.DataSource = CRMCache.List("record_type_display");
                                lstField.DataBind();
                                if (rdr != null)
                                {
                                    lstField.ClearSelection();
                                    lstField.SelectedValue = TypeConvert.ToString(rdr[sDATA_LABEL]);
                                }
                            }
                        }
                        TextBox txtNAME = new TextBox();
                        tdField.Controls.Add(txtNAME);
                        txtNAME.ID = sDISPLAY_FIELD;
                        txtNAME.ReadOnly = true;
                        txtNAME.TabIndex = nFORMAT_TAB_INDEX;

                        txtNAME.EnableViewState = false;

                        if (bLayoutMode)
                        {
                            txtNAME.Text = sDISPLAY_FIELD;
                            txtNAME.Enabled = false;
                        }

                        else if (tbl.Page.IsPostBack)
                        {

                            if (tbl.Page.Request[txtNAME.UniqueID] != null)
                                txtNAME.Text = TypeConvert.ToString(tbl.Page.Request[txtNAME.UniqueID]);
                        }
                        else if (!TypeConvert.IsEmptyString(sDISPLAY_FIELD) && rdr != null)
                            txtNAME.Text = TypeConvert.ToString(rdr[sDISPLAY_FIELD]);

                        else if (sDATA_FIELD == "TEAM_ID" && rdr == null && !tbl.Page.IsPostBack)
                            txtNAME.Text = CRMSecurity.TEAM_NAME;

                        else if (sDATA_FIELD == "ASSIGNED_USER_ID" && rdr == null && !tbl.Page.IsPostBack)
                            txtNAME.Text = CRMSecurity.USER_NAME;

                        HtmlInputHidden hidID = new HtmlInputHidden();
                        tdField.Controls.Add(hidID);
                        hidID.ID = sDATA_FIELD;
                        if (!bLayoutMode)
                        {
                            if (!TypeConvert.IsEmptyString(sDATA_FIELD) && rdr != null)
                            {
                                hidID.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                hidID.Value = (TypeConvert.ToGuid(hidID.Value) == Guid.Empty) ? "" : hidID.Value;
                            }

                            else if (sDATA_FIELD == "TEAM_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.TEAM_ID.ToString();

                            else if (sDATA_FIELD == "ASSIGNED_USER_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.USER_ID.ToString();
                        }

                        Literal litNBSP = new Literal();
                        tdField.Controls.Add(litNBSP);
                        litNBSP.Text = "&nbsp;";

                        HtmlInputButton btnChange = new HtmlInputButton("button");
                        tdField.Controls.Add(btnChange);

                        btnChange.ID = sDATA_FIELD + "_btnChange";
                        btnChange.Attributes.Add("class", "button");
                        if (!TypeConvert.IsEmptyString(sONCLICK_SCRIPT))
                            btnChange.Attributes.Add("onclick", sONCLICK_SCRIPT);

                        btnChange.Attributes.Add("title", Translation.GetTranslation.Term(".LBL_SELECT_BUTTON_TITLE"));

                        btnChange.Value = Translation.GetTranslation.Term(".LBL_SELECT_BUTTON_LABEL");


                        if (sONCLICK_SCRIPT.IndexOf("Popup();") > 0)
                        {
                            litNBSP = new Literal();
                            tdField.Controls.Add(litNBSP);
                            litNBSP.Text = "&nbsp;";

                            HtmlInputButton btnClear = new HtmlInputButton("button");
                            tdField.Controls.Add(btnClear);
                            btnClear.ID = sDATA_FIELD + "_btnClear";
                            btnClear.Attributes.Add("class", "button");
                            btnClear.Attributes.Add("onclick", sONCLICK_SCRIPT.Replace("Popup();", "('', '');").Replace("return ", "return Change"));
                            btnClear.Attributes.Add("title", Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_TITLE"));
                            btnClear.Value = Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_LABEL");
                        }
                        if (!bLayoutMode && bUI_REQUIRED && !TypeConvert.IsEmptyString(sDATA_FIELD))
                        {
                            RequiredFieldValidatorForHiddenInputs reqID = new RequiredFieldValidatorForHiddenInputs();
                            reqID.ID = sDATA_FIELD + "_REQUIRED";
                            reqID.ControlToValidate = hidID.ID;
                            reqID.ErrorMessage = Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD");
                            reqID.CssClass = "required";
                            reqID.EnableViewState = false;

                            reqID.EnableClientScript = false;
                            reqID.Enabled = false;

                            reqID.Style.Add("padding-left", "4px");
                            tdField.Controls.Add(reqID);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "TextBox", true) == 0 || String.Compare(sFIELD_TYPE, "Password", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        TextBox txtField = new TextBox();
                        tdField.Controls.Add(txtField);
                        txtField.ID = sDATA_FIELD;
                        txtField.TabIndex = nFORMAT_TAB_INDEX;
                        if (nFORMAT_ROWS > 0 && nFORMAT_COLUMNS > 0)
                        {
                            txtField.Rows = nFORMAT_ROWS;
                            txtField.Columns = nFORMAT_COLUMNS;
                            txtField.TextMode = TextBoxMode.MultiLine;
                        }
                        else
                        {
                            txtField.MaxLength = nFORMAT_MAX_LENGTH;
                            txtField.Attributes.Add("size", nFORMAT_SIZE.ToString());
                            txtField.TextMode = TextBoxMode.SingleLine;
                        }
                        if (bLayoutMode)
                        {
                            txtField.Text = sDATA_FIELD;
                            txtField.ReadOnly = true;
                        }
                        else if (!TypeConvert.IsEmptyString(sDATA_FIELD) && rdr != null)
                        {
                            int nOrdinal = rdr.GetOrdinal(sDATA_FIELD);
                            string sTypeName = rdr.GetDataTypeName(nOrdinal);

                            if (sTypeName == "money" || rdr[sDATA_FIELD].GetType() == typeof(System.Decimal))
                                txtField.Text = TypeConvert.ToDecimal(rdr[sDATA_FIELD]).ToString("#,##0.00");
                            else
                                txtField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                        }

                        if (String.Compare(sFIELD_TYPE, "Password", true) == 0)
                            txtField.TextMode = TextBoxMode.Password;
                        if (!bLayoutMode && bUI_REQUIRED && !TypeConvert.IsEmptyString(sDATA_FIELD))
                        {
                            RequiredFieldValidator reqNAME = new RequiredFieldValidator();
                            reqNAME.ID = sDATA_FIELD + "_REQUIRED";
                            reqNAME.ControlToValidate = txtField.ID;
                            reqNAME.ErrorMessage = Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD");
                            reqNAME.CssClass = "required";
                            reqNAME.EnableViewState = false;

                            reqNAME.EnableClientScript = false;
                            reqNAME.Enabled = false;
                            reqNAME.Style.Add("padding-left", "4px");
                            tdField.Controls.Add(reqNAME);
                        }
                        if (!bLayoutMode && !TypeConvert.IsEmptyString(sDATA_FIELD))
                        {
                            if (sVALIDATION_TYPE == "RegularExpressionValidator" && !TypeConvert.IsEmptyString(sREGULAR_EXPRESSION) && !TypeConvert.IsEmptyString(sFIELD_VALIDATOR_MESSAGE))
                            {
                                RegularExpressionValidator reqVALIDATOR = new RegularExpressionValidator();
                                reqVALIDATOR.ID = sDATA_FIELD + "_VALIDATOR";
                                reqVALIDATOR.ControlToValidate = txtField.ID;
                                reqVALIDATOR.ErrorMessage = Translation.GetTranslation.Term(sFIELD_VALIDATOR_MESSAGE);
                                reqVALIDATOR.ValidationExpression = sREGULAR_EXPRESSION;
                                reqVALIDATOR.CssClass = "required";
                                reqVALIDATOR.EnableViewState = false;

                                reqVALIDATOR.EnableClientScript = false;
                                reqVALIDATOR.Enabled = false;
                                reqVALIDATOR.Style.Add("padding-left", "4px");
                                tdField.Controls.Add(reqVALIDATOR);
                            }
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DatePicker", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        DatePicker ctlDate = tbl.Page.LoadControl("~/CRM/UserControls/DatePicker.ascx") as DatePicker;
                        tdField.Controls.Add(ctlDate);
                        ctlDate.ID = sDATA_FIELD;

                        ctlDate.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateRange", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        Table tblDateRange = new Table();
                        tdField.Controls.Add(tblDateRange);
                        TableRow trAfter = new TableRow();
                        TableRow trBefore = new TableRow();
                        tblDateRange.Rows.Add(trAfter);
                        tblDateRange.Rows.Add(trBefore);
                        TableCell tdAfterLabel = new TableCell();
                        TableCell tdAfterData = new TableCell();
                        TableCell tdBeforeLabel = new TableCell();
                        TableCell tdBeforeData = new TableCell();
                        trAfter.Cells.Add(tdAfterLabel);
                        trAfter.Cells.Add(tdAfterData);
                        trBefore.Cells.Add(tdBeforeLabel);
                        trBefore.Cells.Add(tdBeforeData);


                        DatePicker ctlDateStart = tbl.Page.LoadControl("~/CRM/UserControls/DatePicker.ascx") as DatePicker;
                        DatePicker ctlDateEnd = tbl.Page.LoadControl("~/CRM/UserControls/DatePicker.ascx") as DatePicker;
                        Literal litAfterLabel = new Literal();
                        Literal litBeforeLabel = new Literal();
                        litAfterLabel.Text = Translation.GetTranslation.Term("SavedSearch.LBL_SEARCH_AFTER");
                        litBeforeLabel.Text = Translation.GetTranslation.Term("SavedSearch.LBL_SEARCH_BEFORE");

                        tdAfterLabel.Controls.Add(litAfterLabel);
                        tdAfterData.Controls.Add(ctlDateStart);
                        tdBeforeLabel.Controls.Add(litBeforeLabel);
                        tdBeforeData.Controls.Add(ctlDateEnd);

                        ctlDateStart.ID = sDATA_FIELD + "_AFTER";
                        ctlDateEnd.ID = sDATA_FIELD + "_BEFORE";

                        ctlDateStart.TabIndex = nFORMAT_TAB_INDEX;
                        ctlDateEnd.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                        {
                            ctlDateStart.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);
                            ctlDateEnd.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);
                        }

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateTimePicker", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        DateTimePicker ctlDate = tbl.Page.LoadControl("~/CRM/UserControls/DateTimePicker.ascx") as DateTimePicker;
                        tdField.Controls.Add(ctlDate);
                        ctlDate.ID = sDATA_FIELD;

                        ctlDate.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);

                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateTimeEdit", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        DateTimeEdit ctlDate = tbl.Page.LoadControl("~/CRM/UserControls/DateTimeEdit.ascx") as DateTimeEdit;
                        tdField.Controls.Add(ctlDate);
                        ctlDate.ID = sDATA_FIELD;

                        ctlDate.TabIndex = nFORMAT_TAB_INDEX;
                        if (rdr != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);

                        if (!bLayoutMode && bUI_REQUIRED)
                        {
                            ctlDate.EnableNone = false;
                        }
                        if (bLayoutMode)
                        {
                            Literal litField = new Literal();
                            litField.Text = sDATA_FIELD;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "File", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        HtmlInputFile ctlField = new HtmlInputFile();
                        tdField.Controls.Add(ctlField);
                        ctlField.ID = sDATA_FIELD;
                        ctlField.MaxLength = nFORMAT_MAX_LENGTH;
                        ctlField.Size = nFORMAT_SIZE;
                        ctlField.Attributes.Add("TabIndex", nFORMAT_TAB_INDEX.ToString());
                        if (!bLayoutMode && bUI_REQUIRED)
                        {
                            RequiredFieldValidator reqNAME = new RequiredFieldValidator();
                            reqNAME.ID = sDATA_FIELD + "_REQUIRED";
                            reqNAME.ControlToValidate = ctlField.ID;
                            reqNAME.ErrorMessage = Translation.GetTranslation.Term(".ERR_REQUIRED_FIELD");
                            reqNAME.CssClass = "required";
                            reqNAME.EnableViewState = false;

                            reqNAME.EnableClientScript = false;
                            reqNAME.Enabled = false;
                            reqNAME.Style.Add("padding-left", "4px");
                            tdField.Controls.Add(reqNAME);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "Image", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        HtmlInputHidden ctlHidden = new HtmlInputHidden();
                        if (!bLayoutMode)
                        {
                            tdField.Controls.Add(ctlHidden);
                            ctlHidden.ID = sDATA_FIELD;

                            HtmlInputFile ctlField = new HtmlInputFile();
                            tdField.Controls.Add(ctlField);

                            ctlField.ID = sDATA_FIELD + "_File";

                            Literal litBR = new Literal();
                            litBR.Text = "<br />";
                            tdField.Controls.Add(litBR);
                        }

                        Image imgField = new Image();

                        imgField.ID = "img" + sDATA_FIELD;
                        try
                        {
                            if (bLayoutMode)
                            {
                                Literal litField = new Literal();
                                litField.Text = sDATA_FIELD;
                                tdField.Controls.Add(litField);
                            }
                            else if (rdr != null)
                            {
                                if (!TypeConvert.IsEmptyString(rdr[sDATA_FIELD]))
                                {
                                    ctlHidden.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                    imgField.ImageUrl = "~/CRM/Images/Image.aspx?ID=" + ctlHidden.Value;

                                    tdField.Controls.Add(imgField);


                                    Literal litClear = new Literal();
                                    litClear.Text = "<br /><input type=\"button\" class=\"button\" onclick=\"form." + ctlHidden.ClientID + ".value='';form." + imgField.ClientID + ".src='';" + "\"  value='" + "  " + Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_LABEL") + "  " + "' title='" + Translation.GetTranslation.Term(".LBL_CLEAR_BUTTON_TITLE") + "' />";
                                    tdField.Controls.Add(litClear);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Literal litField = new Literal();
                            litField.Text = ex.Message;
                            tdField.Controls.Add(litField);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "AddressButtons", true) == 0)
                {
                    tr.Cells.Remove(tdField);
                    tdLabel.Width = "10%";
                    tdLabel.RowSpan = nROWSPAN;
                    tdLabel.VAlign = "middle";
                    tdLabel.Align = "center";
                    tdLabel.Attributes.Remove("class");
                    tdLabel.Attributes.Add("class", "tabFormAddDel");
                    HtmlInputButton btnCopyRight = new HtmlInputButton("button");
                    Literal litSpacer = new Literal();
                    HtmlInputButton btnCopyLeft = new HtmlInputButton("button");
                    tdLabel.Controls.Add(btnCopyRight);
                    tdLabel.Controls.Add(litSpacer);
                    tdLabel.Controls.Add(btnCopyLeft);
                    btnCopyRight.Attributes.Add("title", Translation.GetTranslation.Term("Accounts.NTC_COPY_BILLING_ADDRESS"));
                    btnCopyRight.Attributes.Add("onclick", "return copyAddressRight()");
                    btnCopyRight.Value = ">>";
                    litSpacer.Text = "<br><br>";
                    btnCopyLeft.Attributes.Add("title", Translation.GetTranslation.Term("Accounts.NTC_COPY_SHIPPING_ADDRESS"));
                    btnCopyLeft.Attributes.Add("onclick", "return copyAddressLeft()");
                    btnCopyLeft.Value = "<<";
                    nColIndex = 0;
                }
                else if (String.Compare(sFIELD_TYPE, "Hidden", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        HtmlInputHidden hidID = new HtmlInputHidden();
                        tdField.Controls.Add(hidID);
                        hidID.ID = sDATA_FIELD;
                        if (bLayoutMode)
                        {
                            TextBox txtNAME = new TextBox();
                            tdField.Controls.Add(txtNAME);
                            txtNAME.ReadOnly = true;

                            txtNAME.EnableViewState = false;
                            txtNAME.Text = sDATA_FIELD;
                            txtNAME.Enabled = false;
                        }
                        else
                        {

                            nCOLSPAN = -1;
                            tr.Cells.Remove(tdLabel);
                            tdField.Attributes.Add("style", "display:none");
                            if (!TypeConvert.IsEmptyString(sDATA_FIELD) && rdr != null)
                                hidID.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);

                            else if (sDATA_FIELD == "TEAM_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.TEAM_ID.ToString();

                            else if (sDATA_FIELD == "ASSIGNED_USER_ID" && rdr == null && !tbl.Page.IsPostBack)
                                hidID.Value = CRMSecurity.USER_ID.ToString();
                        }
                    }
                }
                else
                {
                    Literal litField = new Literal();
                    tdField.Controls.Add(litField);
                    litField.Text = "Unknown field type " + sFIELD_TYPE;
                }

                if (nCOLSPAN > 0)
                    nColIndex += nCOLSPAN;
                else if (nCOLSPAN == 0)
                    nColIndex++;
                if (nColIndex >= nDATA_COLUMNS)
                    nColIndex = 0;
            }
        }
        public static void SetEditViewFields(System.Web.UI.UserControl Parent, string sEDIT_NAME, System.Data.SqlClient.SqlDataReader rdr)
        {

            bool bEnableTeamManagement = Common.Config.enable_team_management();
            DataTable dtFields = CRMCache.EditViewFields(sEDIT_NAME);
            DataView dvFields = dtFields.DefaultView;
            foreach (DataRowView row in dvFields)
            {
                string sFIELD_TYPE = TypeConvert.ToString(row["FIELD_TYPE"]);
                string sDATA_LABEL = TypeConvert.ToString(row["DATA_LABEL"]);
                string sDATA_FIELD = TypeConvert.ToString(row["DATA_FIELD"]);
                string sDISPLAY_FIELD = TypeConvert.ToString(row["DISPLAY_FIELD"]);
                int nFORMAT_ROWS = TypeConvert.ToInteger(row["FORMAT_ROWS"]);

                if (sDATA_FIELD == "TEAM_ID")
                {
                    if (!bEnableTeamManagement)
                    {
                        sFIELD_TYPE = "Blank";
                    }
                }
                if (String.Compare(sFIELD_TYPE, "Blank", true) == 0)
                {
                }
                else if (String.Compare(sFIELD_TYPE, "Label", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        Literal litField = Parent.FindControl(sDATA_FIELD) as Literal;
                        if (litField != null)
                        {
                            if (sDATA_FIELD.IndexOf(".") >= 0)
                                litField.Text = Translation.GetTranslation.Term(sDATA_FIELD);
                            else
                                litField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "ListBox", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        if (nFORMAT_ROWS > 0)
                        {

                            ListBox lstField = Parent.FindControl(sDATA_FIELD) as ListBox;
                            if (lstField != null)
                            {
                                lstField.SelectedValue = TypeConvert.ToString(rdr[sDATA_FIELD]);
                            }
                        }
                        else
                        {
                            DropDownList lstField = Parent.FindControl(sDATA_FIELD) as DropDownList;
                            if (lstField != null)
                            {
                                try
                                {
                                    lstField.SelectedValue = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                }
                                catch
                                {
                                    lstField.SelectedIndex = 0;
                                }
                            }
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "CheckBox", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        CheckBox chkField = Parent.FindControl(sDATA_FIELD) as CheckBox;
                        if (chkField != null)
                            chkField.Checked = TypeConvert.ToBoolean(rdr[sDATA_FIELD]);
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "ChangeButton", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {

                        if (sDATA_LABEL == "PARENT_TYPE")
                        {
                            DropDownList lstField = Parent.FindControl(sDATA_LABEL) as DropDownList;
                            if (lstField != null)
                            {
                                lstField.ClearSelection();
                                lstField.SelectedValue = TypeConvert.ToString(rdr[sDATA_LABEL]);
                            }
                        }
                        TextBox txtNAME = Parent.FindControl(sDISPLAY_FIELD) as TextBox;
                        if (txtNAME != null)
                        {
                            if (!TypeConvert.IsEmptyString(sDISPLAY_FIELD))
                                txtNAME.Text = TypeConvert.ToString(rdr[sDISPLAY_FIELD]);
                            HtmlInputHidden hidID = Parent.FindControl(sDATA_FIELD) as HtmlInputHidden;
                            if (hidID != null)
                            {
                                hidID.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);
                            }
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "TextBox", true) == 0 || String.Compare(sFIELD_TYPE, "Password", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        TextBox txtField = Parent.FindControl(sDATA_FIELD) as TextBox;
                        if (txtField != null)
                        {
                            int nOrdinal = rdr.GetOrdinal(sDATA_FIELD);
                            string sTypeName = rdr.GetDataTypeName(nOrdinal);
                            if (sTypeName == "money" || rdr[sDATA_FIELD].GetType() == typeof(System.Decimal))
                                txtField.Text = TypeConvert.ToDecimal(rdr[sDATA_FIELD]).ToString("#,##0.00");
                            else
                                txtField.Text = TypeConvert.ToString(rdr[sDATA_FIELD]);
                        }
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DatePicker", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        DatePicker ctlDate = Parent.FindControl(sDATA_FIELD) as DatePicker;
                        if (ctlDate != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateTimePicker", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        DateTimePicker ctlDate = Parent.FindControl(sDATA_FIELD) as DateTimePicker;
                        if (ctlDate != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "DateTimeEdit", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        DateTimeEdit ctlDate = Parent.FindControl(sDATA_FIELD) as DateTimeEdit;
                        if (ctlDate != null)
                            ctlDate.Value = CRM.Common.TimeZone.GetTimeZone.FromServerTime(rdr[sDATA_FIELD]);
                    }
                }
                else if (String.Compare(sFIELD_TYPE, "File", true) == 0)
                {
                }
                else if (String.Compare(sFIELD_TYPE, "Image", true) == 0)
                {
                    if (!TypeConvert.IsEmptyString(sDATA_FIELD))
                    {
                        HtmlInputHidden ctlHidden = Parent.FindControl(sDATA_FIELD) as HtmlInputHidden;
                        Image imgField = Parent.FindControl("img" + sDATA_FIELD) as Image;
                        if (ctlHidden != null && imgField != null)
                        {
                            if (!TypeConvert.IsEmptyString(rdr[sDATA_FIELD]))
                            {
                                ctlHidden.Value = TypeConvert.ToString(rdr[sDATA_FIELD]);
                                imgField.ImageUrl = "~/CRM/Images/Image.aspx?ID=" + ctlHidden.Value;
                            }
                        }
                    }
                }
            }
        }

        public static void LoadFile(Guid gID, Stream stm, IDbTransaction trn)
        {
            const int BUFFER_LENGTH = 4 * 1024;
            byte[] binFILE_POINTER = new byte[16];

            ModelLayer.CommonProcedure.ImageInitPointer(gID, ref binFILE_POINTER);
            using (BinaryReader reader = new BinaryReader(stm))
            {
                int nFILE_OFFSET = 0;
                byte[] binBYTES = reader.ReadBytes(BUFFER_LENGTH);
                while (binBYTES.Length > 0)
                {

                    ModelLayer.CommonProcedure.ImageWriteOffset(gID, binFILE_POINTER, nFILE_OFFSET, binBYTES);
                    nFILE_OFFSET += binBYTES.Length;
                    binBYTES = reader.ReadBytes(BUFFER_LENGTH);
                }
            }
        }

        public static void LoadFile(Guid gID, Stream stm)
        {
            const int BUFFER_LENGTH = 4 * 1024;
            byte[] binFILE_POINTER = new byte[16];

            ModelLayer.CommonProcedure.ImageInitPointer(gID, ref binFILE_POINTER);
            using (BinaryReader reader = new BinaryReader(stm))
            {
                int nFILE_OFFSET = 0;
                byte[] binBYTES = reader.ReadBytes(BUFFER_LENGTH);
                while (binBYTES.Length > 0)
                {

                    ModelLayer.CommonProcedure.ImageWriteOffset(gID, binFILE_POINTER, nFILE_OFFSET, binBYTES);
                    nFILE_OFFSET += binBYTES.Length;
                    binBYTES = reader.ReadBytes(BUFFER_LENGTH);
                }
            }
        }

        public static bool LoadImage(CRMControl ctlPARENT, Guid gParentID, string sFIELD_NAME, IDbTransaction trn)
        {
            bool bNewFile = false;
            HtmlInputFile fileIMAGE = ctlPARENT.FindControl(sFIELD_NAME + "_File") as HtmlInputFile;
            if (fileIMAGE != null)
            {
                HttpPostedFile pstIMAGE = fileIMAGE.PostedFile;
                if (pstIMAGE != null)
                {
                    long lFileSize = pstIMAGE.ContentLength;
                    long lUploadMaxSize = TypeConvert.ToLong(ConfigurationManager.AppSettings["upload_maxsize"]);
                    if ((lUploadMaxSize > 0) && (lFileSize > lUploadMaxSize))
                    {
                        throw (new Exception("ERROR: uploaded file was too big: max filesize: " + lUploadMaxSize.ToString()));
                    }

                    if (pstIMAGE.FileName.Length > 0)
                    {
                        string sFILENAME = Path.GetFileName(pstIMAGE.FileName);
                        string sFILE_EXT = Path.GetExtension(sFILENAME);
                        string sFILE_MIME_TYPE = pstIMAGE.ContentType;

                        Guid gImageID = Guid.Empty;
                        ModelLayer.CommonProcedure.ImagesInsert(ref gImageID, gParentID, sFILENAME, sFILE_EXT, sFILE_MIME_TYPE);
                        CRMDynamic.LoadFile(gImageID, pstIMAGE.InputStream, trn);

                        DynamicControl ctlIMAGE = new DynamicControl(ctlPARENT, sFIELD_NAME);
                        ctlIMAGE.ID = gImageID;
                        bNewFile = true;
                    }
                }
            }
            return bNewFile;
        }

        public static bool LoadImage(CRMControl ctlPARENT, Guid gParentID, string sFIELD_NAME)
        {
            bool bNewFile = false;
            HtmlInputFile fileIMAGE = ctlPARENT.FindControl(sFIELD_NAME + "_File") as HtmlInputFile;
            if (fileIMAGE != null)
            {
                HttpPostedFile pstIMAGE = fileIMAGE.PostedFile;
                if (pstIMAGE != null)
                {
                    long lFileSize = pstIMAGE.ContentLength;
                    long lUploadMaxSize = TypeConvert.ToLong(ConfigurationManager.AppSettings["upload_maxsize"]);
                    if ((lUploadMaxSize > 0) && (lFileSize > lUploadMaxSize))
                    {
                        throw (new Exception("ERROR: uploaded file was too big: max filesize: " + lUploadMaxSize.ToString()));
                    }

                    if (pstIMAGE.FileName.Length > 0)
                    {
                        string sFILENAME = Path.GetFileName(pstIMAGE.FileName);
                        string sFILE_EXT = Path.GetExtension(sFILENAME);
                        string sFILE_MIME_TYPE = pstIMAGE.ContentType;

                        Guid gImageID = Guid.Empty;
                        ModelLayer.CommonProcedure.ImagesInsert(ref gImageID, gParentID, sFILENAME, sFILE_EXT, sFILE_MIME_TYPE);
                        CRMDynamic.LoadFile(gImageID, pstIMAGE.InputStream);

                        DynamicControl ctlIMAGE = new DynamicControl(ctlPARENT, sFIELD_NAME);
                        ctlIMAGE.ID = gImageID;
                        bNewFile = true;
                    }
                }
            }
            return bNewFile;
        }

        public static void UpdateCustomFields(CRMControl ctlPARENT, IDbTransaction trn, Guid gID, string sCUSTOM_MODULE, DataTable dtCustomFields)
        {
            UpdateCustomFields(ctlPARENT, gID, sCUSTOM_MODULE, dtCustomFields);
        }


        public static void UpdateCustomFields(DataRow rowForm, IDbTransaction trn, Guid gID, string sCUSTOM_MODULE, DataTable dtCustomFields)
        {
            UpdateCustomFields(rowForm, gID, sCUSTOM_MODULE, dtCustomFields);
        }


        public static void UpdateCustomFields(CRMControl ctlPARENT, Guid gID, string sCUSTOM_MODULE, DataTable dtCustomFields)
        {
            if (dtCustomFields.Rows.Count > 0)
            {
                InlineQueryDBManager oQuery = new InlineQueryDBManager();
                oQuery.CommandText = "update " + sCUSTOM_MODULE + "_CSTM" + ControlChars.CrLf;
                int nFieldIndex = 0;
                foreach (DataRow row in dtCustomFields.Rows)
                {
                    string sNAME = TypeConvert.ToString(row["NAME"]).ToUpper();
                    string sCsType = TypeConvert.ToString(row["CsType"]);

                    int nMAX_SIZE = TypeConvert.ToInteger(row["MAX_SIZE"]);
                    DynamicControl ctlCustomField = new DynamicControl(ctlPARENT, sNAME);

                    if (ctlCustomField.Exists && ctlCustomField.Type != "Literal")
                    {
                        if (nFieldIndex == 0)
                            oQuery.CommandText += "   set ";
                        else
                            oQuery.CommandText += "     , ";

                        oQuery.CommandText += sNAME + " = @" + sNAME + ControlChars.CrLf;

                        DynamicControl ctlCustomField_File = new DynamicControl(ctlPARENT, sNAME + "_File");

                        if (sCsType == "Guid" && ctlCustomField.Type == "HtmlInputHidden" && ctlCustomField_File.Exists)
                        {
                            LoadImage(ctlPARENT, gID, sNAME);
                        }
                        SqlParameter oParam = new SqlParameter();
                        switch (sCsType)
                        {
                            case "Guid":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.UniqueIdentifier);
                                oParam.Value = ctlCustomField.ID;
                                break;
                            case "short":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Int);
                                oParam.Value = ctlCustomField.IntegerValue;
                                break;
                            case "Int32":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Int);
                                oParam.Value = ctlCustomField.IntegerValue;
                                break;
                            case "Int64":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Int);
                                oParam.Value = ctlCustomField.IntegerValue;
                                break;
                            case "float":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Float);
                                oParam.Value = ctlCustomField.FloatValue;
                                break;
                            case "decimal":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Decimal);
                                oParam.Value = ctlCustomField.DecimalValue;
                                break;
                            case "bool":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Bit);
                                oParam.Value = ctlCustomField.Checked;
                                break;
                            case "DateTime":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.DateTime);
                                oParam.Value = ctlCustomField.DateValue;
                                break;
                            default:
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.NVarChar, nMAX_SIZE);
                                oParam.Value = ctlCustomField.Text;
                                break;
                        }
                        nFieldIndex++;
                    }
                }
                if (nFieldIndex > 0)
                {
                    oQuery.CommandText += " where ID_C = @ID_C" + ControlChars.CrLf;
                    SqlParameter oParamID = new SqlParameter("@ID_C", SqlDbType.UniqueIdentifier);
                    oParamID.Value = gID;
                    oQuery.CommandType = CommandType.Text;
                    oQuery.ExecuteNonQuery(oQuery.CommandText);
                }
            }
        }


        public static void UpdateCustomFields(DataRow rowForm, Guid gID, string sCUSTOM_MODULE, DataTable dtCustomFields)
        {
            if (dtCustomFields.Rows.Count > 0)
            {
                InlineQueryDBManager oQuery = new InlineQueryDBManager();
                oQuery.CommandText = "update " + sCUSTOM_MODULE + "_CSTM" + ControlChars.CrLf;
                int nFieldIndex = 0;
                foreach (DataRow row in dtCustomFields.Rows)
                {

                    string sNAME = TypeConvert.ToString(row["NAME"]).ToUpper();
                    string sCsType = TypeConvert.ToString(row["CsType"]);

                    int nMAX_SIZE = TypeConvert.ToInteger(row["MAX_SIZE"]);
                    if (rowForm.Table.Columns.Contains(sNAME))
                    {
                        if (nFieldIndex == 0)
                            oQuery.CommandText += "   set ";
                        else
                            oQuery.CommandText += "     , ";

                        oQuery.CommandText += sNAME + " = @" + sNAME + ControlChars.CrLf;

                        SqlParameter oParam = new SqlParameter();

                        switch (sCsType)
                        {
                            case "Guid":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.UniqueIdentifier);
                                oParam.Value = TypeConvert.ToGuid(rowForm[sNAME]);
                                break;
                            case "short":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Int);
                                oParam.Value = TypeConvert.ToInteger(rowForm[sNAME]);
                                break;
                            case "Int32":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Int);
                                oParam.Value = TypeConvert.ToInteger(rowForm[sNAME]);
                                break;
                            case "Int64":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Int);
                                oParam.Value = TypeConvert.ToInteger(rowForm[sNAME]);
                                break;
                            case "float":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Float);
                                oParam.Value = TypeConvert.ToFloat(rowForm[sNAME]);
                                break;
                            case "decimal":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Decimal);
                                oParam.Value = TypeConvert.ToDecimal(rowForm[sNAME]);
                                break;
                            case "bool":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.Bit);
                                oParam.Value = TypeConvert.ToBoolean(rowForm[sNAME]);
                                break;
                            case "DateTime":
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.DateTime);
                                oParam.Value = TypeConvert.ToDateTime(rowForm[sNAME]);
                                break;
                            default:
                                oParam = new SqlParameter("@" + sNAME, SqlDbType.NVarChar, nMAX_SIZE);
                                oParam.Value = TypeConvert.ToString(rowForm[sNAME]);
                                break;
                        }
                        nFieldIndex++;
                    }
                }
                if (nFieldIndex > 0)
                {
                    oQuery.CommandText += " where ID_C = @ID_C" + ControlChars.CrLf;
                    SqlParameter oParamID = new SqlParameter("@ID_C", SqlDbType.UniqueIdentifier);
                    oParamID.Value = gID;
                    oQuery.CommandType = CommandType.Text;
                    oQuery.ExecuteNonQuery(oQuery.CommandText);
                }
            }
        }


    }
}
