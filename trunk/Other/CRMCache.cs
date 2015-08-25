using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using CRM.BusinessLogic;
using CRM.Common;
using System.Xml;
using Config = CRM.Common.Config;
using System.Configuration;

namespace CRM
{
    public delegate DataTable CRMCacheCallback();

    public class CRMCacheReference
    {
        private readonly CRMCacheCallback m_fnDataSource;
        private readonly string m_sDataTextField;
        private readonly string m_sDataValueField;
        private readonly string m_sName;

        public CRMCacheReference(string sName, string sDataValueField, string sDataTextField,
                                 CRMCacheCallback fnDataSource)
        {
            m_sName = sName;
            m_sDataValueField = sDataValueField;
            m_sDataTextField = sDataTextField;
            m_fnDataSource = fnDataSource;
        }

        public string Name
        {
            get { return m_sName; }
        }

        public string DataValueField
        {
            get { return m_sDataValueField; }
        }

        public string DataTextField
        {
            get { return m_sDataTextField; }
        }

        public CRMCacheCallback DataSource
        {
            get { return m_fnDataSource; }
        }
    }

    /// <summary>
    /// Summary description for CRMCache.
    /// </summary>
    public class CRMCache
    {
        public static CRMCacheReference[] CustomCaches = new[]
                                                             {
                                                                 new CRMCacheReference("AssignedUser", "ID", "USER_NAME",
                                                                                       AssignedUser)
                                                                 ,
                                                                 new CRMCacheReference("Currencies", "ID", "NAME_SYMBOL",
                                                                                       Currencies)
                                                                 ,
                                                                 new CRMCacheReference("Release", "ID", "NAME", Release)
                                                                 ,
                                                                 new CRMCacheReference("Manufacturers", "ID", "NAME",
                                                                                       Manufacturers)
                                                                 ,
                                                                 new CRMCacheReference("Shippers", "ID", "NAME",
                                                                                       Shippers)
                                                                 ,
                                                                 new CRMCacheReference("ProductTypes", "ID", "NAME",
                                                                                       ProductTypes)
                                                                 ,
                                                                 new CRMCacheReference("ProductCategories", "ID", "NAME",
                                                                                       ProductCategories)
                                                                 ,
                                                                 new CRMCacheReference("ContractTypes", "ID", "NAME",
                                                                                       ContractTypes)
                                                                 ,
                                                                 new CRMCacheReference("ForumTopics", "NAME", "NAME",
                                                                                       ForumTopics)
                                                                 ,
                                                                 new CRMCacheReference("Modules", "MODULE_NAME",
                                                                                       "MODULE_NAME", Modules)
                                                                 ,
                                                                 new CRMCacheReference("EmailGroups", "ID", "NAME",
                                                                                       EmailGroups)
                                                                 ,
                                                                 new CRMCacheReference("InboundEmailBounce", "ID",
                                                                                       "NAME", InboundEmailBounce)
                                                             };

        public static DateTime DefaultCacheExpiration()
        {
            return DateTime.Now.AddDays(1);
        }

        public static DataTable List(string sListName)
        {
            DataTable dt = new DataTable();
            if (sListName == "lead_status_dom")
            {
                InlineQueryDBManager oDbManager = new InlineQueryDBManager();
                oDbManager.CommandText = "select * from LeadStatus";
                oDbManager.CommandType = CommandType.Text;
                dt = oDbManager.GetTable();
            }
            else if (sListName == "Currencies")
            {
                InlineQueryDBManager oDbManager = new InlineQueryDBManager();
                oDbManager.CommandText = "select ID as [NAME],SYMBOL as DISPLAY_NAME from Currencies";
                oDbManager.CommandType = CommandType.Text;
                dt = oDbManager.GetTable();
            }
            else if (sListName == "program_plan")
            {
                InlineQueryDBManager oQuery;
                Guid User_ID = new Guid(HttpContext.Current.Session["USER_ID"].ToString());
                oQuery = new InlineQueryDBManager();
                oQuery.CommandText = "select distinct acl_roles.Name ROLE_NAME from acl_roles inner join acl_roles_users" +
                                     " on acl_roles.ID = role_id where user_id = '" + User_ID + "'";
                oQuery.CommandType = CommandType.Text;
                DataTable oDataTable = oQuery.GetTable();
                string Roles = ConfigurationManager.AppSettings["UserRoles_Channel_Partner"];
                for (int i = 0; i < oDataTable.Rows.Count; i++)
                {
                    if (oDataTable.Rows[i]["ROLE_NAME"].ToString() == Roles)
                    {
                        string User_Name = HttpContext.Current.Session["USER_NAME"].ToString();
                        oQuery = new InlineQueryDBManager();
                        oQuery.CommandText = "select * from vwProgramPlanSelection where [USER_NAME] = '" + HttpContext.Current.Session["USER_NAME"].ToString() + "'";
                        oQuery.CommandType = CommandType.Text;
                        dt = oQuery.GetTable();
                    }
                }
            }
            else if (sListName == "reportsto_dom")
            {
                InlineQueryDBManager oQuery;
                Guid User_ID = new Guid(HttpContext.Current.Session["USER_ID"].ToString());
                oQuery = new InlineQueryDBManager();
                oQuery.CommandText = "select [USER_NAME] as [NAME],[USER_NAME] as [DISPLAY_NAME] from users" + 
                                     " where ID = (select REPORTS_TO_ID from users where ID = '" + User_ID.ToString() + "' and DELETED = 0)";
                oQuery.CommandType = CommandType.Text;
                dt = oQuery.GetTable();
            }
            else if (sListName == "AssignedUser")
            {
                InlineQueryDBManager oDbManager = new InlineQueryDBManager();
                string SQL = String.Format("select ID as NAME,  IsNull(FIRST_NAME,'')+' '+IsNull(LAST_NAME,'') as DISPLAY_NAME from Users where ID IN (SELECT ID FROM fnSubordinates('{0}')) AND DELETED = 0",Security.USER_ID);
                oDbManager.CommandText = SQL;
                //oDbManager.CommandText = "select ID as NAME,  IsNull(FIRST_NAME,'')+' '+IsNull(LAST_NAME,'') as DISPLAY_NAME from Users where DELETED = 0";
                oDbManager.CommandType = CommandType.Text;
                dt = oDbManager.GetTable();
            }
            else if (sListName == "Manufacturers")
            {
                InlineQueryDBManager oDbManager = new InlineQueryDBManager();
                oDbManager.CommandText = "SELECT ID AS NAME,NAME AS  DISPLAY_NAME from Manufacturers";
                oDbManager.CommandType = CommandType.Text;
                dt = oDbManager.GetTable();
            }
            else
            {
                string _XmlSchemaPath = HttpContext.Current.Server.MapPath("~/CRM/Resource/Schema/DropDownList.xsd");
                string FileName = HttpContext.Current.Server.MapPath(String.Format("~/CRM/Resource/DropDownList/en-US/{0}.xml", sListName));
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(_XmlSchemaPath);
                try
                {
                    ds.ReadXml(FileName);
                    dt = ds.Tables[1];
                }
                catch { }
            }
            return dt;
        }

        public static DataTable List(string sModuleName, string sListName)
        {
            Cache Cache = HttpRuntime.Cache;

            var dt = Cache.Get(Translation.GetTranslation.NAME + "." + sListName) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_177"].ToString();
                oQuery.CommandText = innerSql;

                oQuery.Add("@MODULE_NAME", SqlDbType.NVarChar, sModuleName.ToLower());
                oQuery.Add("@LIST_NAME", SqlDbType.NVarChar, sListName.ToLower());
                oQuery.Add("@LANG", SqlDbType.NVarChar, Translation.GetTranslation.NAME.ToLower());
                dt = oQuery.GetTable();
                Cache.Insert(Translation.GetTranslation.NAME + "." + sListName, dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearUsers()
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwUSERS_ASSIGNED_TO");
            Cache.Remove("vwUSERS_List");
        }


        public static string AssignedUser(Guid gID)
        {
            string sUSER_NAME = String.Empty;
            if (!CommonTypeConvert.IsEmptyGuid(gID))
            {
                var vwAssignedUser = new DataView(AssignedUser());
                vwAssignedUser.RowFilter = "ID = '" + gID + "'";
                if (vwAssignedUser.Count > 0)
                {
                    sUSER_NAME = CommonTypeConvert.ToString(vwAssignedUser[0]["USER_NAME"]);
                }
            }
            return sUSER_NAME;
        }

        public static DataTable AssignedUser()
        {
            Cache Cache = HttpRuntime.Cache;

            bool bTeamFilter = !Security.IS_ADMIN && Config.enable_team_management();
            string sCACHE_NAME = String.Empty;
            if (bTeamFilter)
                sCACHE_NAME = "vwTEAMS_ASSIGNED_TO." + Security.USER_ID;
            else
                sCACHE_NAME = "vwUSERS_ASSIGNED_TO";
            var dt = Cache.Get(sCACHE_NAME) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql;
                if (bTeamFilter)
                {
                    innerSql = ApplicationSQL.SQL["_code_CRMCache_256"].ToString();
                }
                else
                {
                    innerSql = "select ID                 " + ControlChars.CrLf
                               + "     , USER_NAME          " + ControlChars.CrLf
                               + "  from vwUSERS_ASSIGNED_TO" + ControlChars.CrLf
                               + " order by USER_NAME       " + ControlChars.CrLf;
                }
                oQuery.CommandText = innerSql;
                if (bTeamFilter)
                    oQuery.Add("@MEMBERSHIP_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                dt = oQuery.GetTable();
                Cache.Insert(sCACHE_NAME, dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable CustomEditModules()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwCUSTOM_EDIT_MODULES") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_264"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwCUSTOM_EDIT_MODULES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable ReportingModules()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwMODULES_Reporting_" + Security.USER_ID) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = "select MODULE_NAME             " + ControlChars.CrLf
                                  + "     , DISPLAY_NAME            " + ControlChars.CrLf
                                  + "  from vwMODULES_Reporting     " + ControlChars.CrLf
                                  + " where USER_ID = @USER_ID      " + ControlChars.CrLf
                                  + "    or USER_ID is null         " + ControlChars.CrLf;

                oQuery.CommandText = innerSql;
                oQuery.Add("@USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                dt = oQuery.GetTable();
                foreach (DataRow row in dt.Rows)
                {
                    row["DISPLAY_NAME"] =
                        Translation.GetTranslation.Term(CommonTypeConvert.ToString(row["DISPLAY_NAME"]));
                }
                Cache.Insert("vwMODULES_Reporting_" + Security.USER_ID, dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable ImportModules()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwMODULES_Import_" + Security.USER_ID) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_RMCache_392"].ToString();
                oQuery.CommandText = innerSql;
                oQuery.Add("@USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                dt = oQuery.GetTable();
                foreach (DataRow row in dt.Rows)
                {
                    row["DISPLAY_NAME"] =
                        Translation.GetTranslation.Term(CommonTypeConvert.ToString(row["DISPLAY_NAME"]));
                }
                Cache.Insert("vwMODULES_Import_" + Security.USER_ID, dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static string[] ReportingModulesList()
        {
            DataTable dt = ReportingModules();
            var arr = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arr[i] = CommonTypeConvert.ToString(dt.Rows[i]["MODULE_NAME"]);
            }
            return arr;
        }

        public static DataTable ReportingRelationships()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwRELATIONSHIPS_Reporting") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_448"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwRELATIONSHIPS_Reporting", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearFilterColumns(string sMODULE_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwSqlColumns_Reporting." + sMODULE_NAME);
        }

        public static DataTable ReportingFilterColumns(string sMODULE_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwSqlColumns_Reporting." + sMODULE_NAME) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_491"].ToString();
                oQuery.CommandText = innerSql;
                oQuery.Add("@OBJECTNAME", SqlDbType.VarChar, 100, "vw" + sMODULE_NAME);
                dt = oQuery.GetTable();
                Cache.Insert("vwSqlColumns_Reporting." + sMODULE_NAME, dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static string ReportingFilterColumnsListName(string sMODULE_NAME, string sDATA_FIELD)
        {
            Cache Cache = HttpRuntime.Cache;
            var sLIST_NAME = Cache.Get("vwSqlColumns_ListName." + sMODULE_NAME + "." + sDATA_FIELD) as string;
            if (sLIST_NAME == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_529"].ToString();
                oQuery.CommandText = innerSql;
                oQuery.Add("@OBJECTNAME", SqlDbType.VarChar, 100, "vw" + sMODULE_NAME);
                oQuery.Add("@DATA_FIELD", SqlDbType.VarChar, 100, sDATA_FIELD);
                sLIST_NAME = CommonTypeConvert.ToString(oQuery.ExecuteScalar());
                Cache.Insert("vwSqlColumns_ListName." + sMODULE_NAME + "." + sDATA_FIELD, sLIST_NAME, null,
                             DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return sLIST_NAME;
        }

        public static DataTable ImportColumns(string sMODULE_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwSqlColumns_Import." + sMODULE_NAME) as DataTable;
            if (dt == null)
            {
                string sTABLE_NAME =
                    CommonTypeConvert.ToString(HttpContext.Current.Application["Modules." + sMODULE_NAME + ".TableName"]);
                if (CommonTypeConvert.IsEmptyString(sTABLE_NAME))
                    sTABLE_NAME = sMODULE_NAME.ToUpper();

                var oQuery = new InlineQueryDBManager();


                dt = new DataTable();
                dt.Columns.Add("ColumnName", Type.GetType("System.String"));
                dt.Columns.Add("NAME", Type.GetType("System.String"));
                dt.Columns.Add("DISPLAY_NAME", Type.GetType("System.String"));
                dt.Columns.Add("ColumnType", Type.GetType("System.String"));
                dt.Columns.Add("Size", Type.GetType("System.Int32"));
                dt.Columns.Add("Scale", Type.GetType("System.Int32"));
                dt.Columns.Add("Precision", Type.GetType("System.Int32"));
                dt.Columns.Add("colid", Type.GetType("System.Int32"));
                dt.Columns.Add("CustomField", Type.GetType("System.Boolean"));
                {
                    oQuery = null;
                    try
                    {
                        oQuery = InlineQueryDBManager.Factory("stp_Zpro_" + sTABLE_NAME + "_Import");
                        if (oQuery == null)
                            throw new Exception();
                    }
                    catch
                    {
                        oQuery = InlineQueryDBManager.Factory("stp_Zpro_" + sTABLE_NAME + "_Update");
                    }
                    for (int i = 0; i < oQuery.Count; i++)
                    {
                        SqlParameter par = oQuery[i];
                        DataRow row = dt.NewRow();
                        dt.Rows.Add(row);
                        row["ColumnName"] = par.ParameterName;
                        row["NAME"] = CommonTypeConvert.ExtractDbName(par.ParameterName);
                        row["DISPLAY_NAME"] = row["NAME"];
                        row["ColumnType"] = par.DbType.ToString();
                        row["Size"] = par.Size;
                        row["Scale"] = par.Scale;
                        row["Precision"] = par.Precision;
                        row["colid"] = i;
                        row["CustomField"] = false;
                    }


                    string sSQL;

                    if (Config.enable_team_management())
                    {
                        bool bModuleIsTeamed =
                            CommonTypeConvert.ToBoolean(
                                HttpContext.Current.Application["Modules." + sMODULE_NAME + ".Teamed"]);
                        DataRow row = dt.NewRow();
                        row = dt.NewRow();
                        row["ColumnName"] = "@TEAM_NAME";
                        row["NAME"] = "TEAM_NAME";
                        row["DISPLAY_NAME"] = "TEAM_NAME";
                        row["ColumnType"] = "string";
                        row["Size"] = 128;
                        row["colid"] = dt.Rows.Count;
                        row["CustomField"] = false;
                        dt.Rows.Add(row);
                    }

                    sSQL = ApplicationSQL.SQL["_code_CRMCache_629"];
                    oQuery = new InlineQueryDBManager();
                    oQuery.CommandText = sSQL;
                    oQuery.Add("@OBJECTNAME", SqlDbType.NVarChar, sTABLE_NAME + "_CSTM");
                    DataTable dtCSTM = oQuery.GetTable();
                    foreach (DataRow rowCSTM in dtCSTM.Rows)
                    {
                        DataRow row = dt.NewRow();
                        row["ColumnName"] = CommonTypeConvert.ToString(rowCSTM["ColumnName"]);
                        row["NAME"] = CommonTypeConvert.ToString(rowCSTM["ColumnName"]);
                        row["DISPLAY_NAME"] = CommonTypeConvert.ToString(rowCSTM["ColumnName"]);
                        row["ColumnType"] = CommonTypeConvert.ToString(rowCSTM["CsType"]);
                        row["Size"] = CommonTypeConvert.ToInteger(rowCSTM["length"]);

                        row["colid"] = dt.Rows.Count;
                        row["CustomField"] = true;
                        dt.Rows.Add(row);
                    }

                    Cache.Insert("vwSqlColumns_Import." + sMODULE_NAME, dt, null, DefaultCacheExpiration(),
                                 Cache.NoSlidingExpiration);
                }
            }
            return dt;

            //return new DataTable();
        }

        public static DataTable Release()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwRELEASES_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_677"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwRELEASES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable ProductCategories()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwPRODUCT_CATEGORIES_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_716"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwPRODUCT_CATEGORIES_LISTBOX", dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable ProductTypes()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwPRODUCT_TYPES_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_755"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwPRODUCT_TYPES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable Manufacturers()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwMANUFACTURERS_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_794"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwMANUFACTURERS_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable Shippers()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwSHIPPERS_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_833"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwSHIPPERS_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable TaxRates()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwTAX_RATES_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_881"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwTAX_RATES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable ContractTypes()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwCONTRACT_TYPES_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_908"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwCONTRACT_TYPES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable Currencies()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwCURRENCIES_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_948"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwCURRENCIES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable Timezones()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwTIMEZONES") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1002"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwTIMEZONES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable TimezonesListbox()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwTIMEZONES_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = "select ID                 " + ControlChars.CrLf
                                  + "     , NAME               " + ControlChars.CrLf
                                  + "  from vwTIMEZONES_LISTBOX" + ControlChars.CrLf
                                  + " order by BIAS desc       " + ControlChars.CrLf;
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwTIMEZONES_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearLanguages()
        {
            HttpRuntime.Cache.Remove("vwLANGUAGES");
        }


        public static DataTable Languages()
        {
            return Languages(HttpContext.Current.Application);
        }


        public static DataTable Languages(HttpApplicationState Application)
        {
            
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwLANGUAGES") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1028"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwLANGUAGES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable Modules()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwMODULES") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1068"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwMODULES", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearTerminologyPickLists()
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwTERMINOLOGY_PickList");
        }

        public static DataTable TerminologyPickLists()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwTERMINOLOGY_PickList") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1177"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwTERMINOLOGY_PickList", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable ActiveUsers()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwUSERS_List") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1149"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwUSERS_List", dt, null, DateTime.Now.AddSeconds(15), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearTabMenu()
        {
            HttpContext.Current.Session.Remove("vwMODULES_TabMenu_ByUser." + Security.USER_ID);

            HttpContext.Current.Session.Remove("vwMODULES_MobileMenu_ByUser." + Security.USER_ID);
        }

        public static DataTable TabMenu()
        {
            HttpSessionState Session = HttpContext.Current.Session;

            var dt = Session["vwMODULES_TabMenu_ByUser." + Security.USER_ID] as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                if (Security.IsAuthenticated())
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["_code_CRMCache_1203"].ToString();
                    oQuery.CommandText = innerSql;
                    oQuery.Add("@USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                    dt = oQuery.GetTable();
                    Session["vwMODULES_TabMenu_ByUser." + Security.USER_ID] = dt;
                }
            }
            return dt;
        }
        public static DataTable TabMenuDescription()
        {
            HttpSessionState Session = HttpContext.Current.Session;

            var dt = Session["vwMODULES_DESCRIPTION_ByUser." + Security.USER_ID] as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                if (Security.IsAuthenticated())
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["_code_CRMCache_1939"].ToString();
                    oQuery.CommandText = innerSql;
                    oQuery.Add("@USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                    dt = oQuery.GetTable();
                    Session["vwMODULES_DESCRIPTION_ByUser." + Security.USER_ID] = dt;
                }
            }
            return dt;
        }

        public static DataTable MobileMenu()
        {
            HttpSessionState Session = HttpContext.Current.Session;

            var dt = Session["vwMODULES_MobileMenu_ByUser." + Security.USER_ID] as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1252"].ToString();
                oQuery.CommandText = innerSql;
                oQuery.Add("@USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                dt = oQuery.GetTable();
                Session["vwMODULES_MobileMenu_ByUser." + Security.USER_ID] = dt;
            }
            return dt;
        }

        public static void ClearShortcuts(string sMODULE_NAME)
        {
            HttpContext.Current.Session.Remove("vwSHORTCUTS_Menu_ByUser." + sMODULE_NAME + "." + Security.USER_ID);
        }

        public static DataTable Shortcuts(string sMODULE_NAME)
        {
            var dt =
                HttpContext.Current.Session["vwSHORTCUTS_Menu_ByUser." + sMODULE_NAME + "." + Security.USER_ID] as
                DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                if (Security.IsAuthenticated())
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = ApplicationSQL.SQL["_code_CRMCache_1304"].ToString();
                    oQuery.CommandText = innerSql;
                    oQuery.Add("@MODULE_NAME", SqlDbType.VarChar, 100, sMODULE_NAME);
                    oQuery.Add("@USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                    dt = oQuery.GetTable();
                    HttpContext.Current.Session["vwSHORTCUTS_Menu_ByUser." + sMODULE_NAME + "." + Security.USER_ID] = dt;
                }
            }
            return dt;
        }

        public static DataTable Themes()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(HttpContext.Current.Server.MapPath("~/App_Themes/Theme.xml"));
            DataTable dt = ds.Tables[0];
            return dt;
        }

        public static string XmlFile(string sPATH_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            var sDATA = Cache.Get("XmlFile." + sPATH_NAME) as string;
            if (sDATA == null)
            {
                using (var rd = new StreamReader(sPATH_NAME, Encoding.UTF8))
                {
                    sDATA = rd.ReadToEnd();
                }

                Cache.Insert("XmlFile." + sPATH_NAME, sDATA, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return sDATA;
        }

        public static DataTable GridViewColumns(string sGRID_NAME)
        {
            DataTable dt = new DataTable();
            string _XmlSchemaPath = HttpContext.Current.Server.MapPath("~/CRM/Resource/Schema/GridView.xsd");

            string FileName = HttpContext.Current.Server.MapPath(String.Format("~/CRM/Resource/GridView/{0}.xml", sGRID_NAME));
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(_XmlSchemaPath);
            try
            {
                ds.ReadXml(FileName);
                dt = ds.Tables[1];
            }
            catch { }
            return dt;
        }

        public static void ClearDetailView(string sDETAIL_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwDETAILVIEWS_FIELDS." + sDETAIL_NAME);
        }

        public static DataTable DetailViewFields(string sDETAIL_NAME)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string _XmlSchemaPath = HttpContext.Current.Server.MapPath("~/CRM/Resource/Schema/DetailView.xsd");
            ds.ReadXmlSchema(_XmlSchemaPath);
            string _XmlPath = HttpContext.Current.Server.MapPath(String.Format("~/CRM/Resource/DetailsView/{0}.xml", sDETAIL_NAME));
            try
            {
                ds.ReadXml(_XmlPath);
                dt = ds.Tables[1];
            }
            catch { }
            return dt;
        }

       
        public static DataTable DetailViewRelationships(string sDETAIL_NAME)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string _XmlSchemaPath = HttpContext.Current.Server.MapPath("~/CRM/Resource/Schema/Relations.xsd");
            ds.ReadXmlSchema(_XmlSchemaPath);
            string _XmlPath = HttpContext.Current.Server.MapPath(String.Format("~/CRM/Resource/Relations/{0}.xml", sDETAIL_NAME));
            try
            {
                ds.ReadXml(_XmlPath);
                dt = ds.Tables[1];
            }
            catch { }
            string sDISABLED_MODULE = !TypeConvert.ToBoolean(ConfigurationManager.AppSettings["enable_team_management"]) ? "Deal" : "Opportunities";
            DataRow dDisabledRow = null;
            foreach (DataRow dRow in dt.Rows)
            {
                if (dRow["MODULE_NAME"].ToString().Trim() == sDISABLED_MODULE.Trim())
                {
                    dDisabledRow = dRow;
                    break;
                }
            }
            if (dDisabledRow != null)
                dt.Rows.Remove(dDisabledRow);
            return dt;
        }
       
        public static DataTable EditViewFields(string sEDIT_NAME)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string _XmlSchemaPath = HttpContext.Current.Server.MapPath("~/CRM/Resource/Schema/EditView.xsd");
            ds.ReadXmlSchema(_XmlSchemaPath);
            string _XmlPath = HttpContext.Current.Server.MapPath(String.Format("~/CRM/Resource/EditView/{0}.xml", sEDIT_NAME));
            try
            {
                ds.ReadXml(_XmlPath);
                dt = ds.Tables[1];
            }
            catch { }
            return dt;
        }


        public static DataTable DynamicButtons(string sVIEW_NAME)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string _XmlSchemaPath = HttpContext.Current.Server.MapPath("~/CRM/Resource/Schema/Buttons.xsd");
            ds.ReadXmlSchema(_XmlSchemaPath);
            try
            {
                string _XmlPath = HttpContext.Current.Server.MapPath(String.Format("~/CRM/Resource/Buttons/{0}.xml", sVIEW_NAME));
                ds.ReadXml(_XmlPath);
                dt = ds.Tables[1];
            }
            catch { }
            return dt;
        }

        public static void ClearFieldsMetaData(string sMODULE_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwFIELDS_META_DATA_Validated." + sMODULE_NAME);
            ClearFilterColumns(sMODULE_NAME);
            ClearSearchColumns(sMODULE_NAME);
        }

        public static DataTable FieldsMetaData_Validated(string sMODULE)
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwFIELDS_META_DATA_Validated." + sMODULE) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1741"].ToString();
                oQuery.CommandText = innerSql;
                oQuery.Add("@CUSTOM_MODULE", SqlDbType.VarChar, sMODULE);
                dt = oQuery.GetTable();
                Cache.Insert("vwFIELDS_META_DATA_Validated." + sMODULE, dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }


        public static DataTable FieldsMetaData_UnvalidatedCustomFields(string sTABLE_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwFIELDS_META_DATA_Unvalidated." + sTABLE_NAME) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1694"].ToString();
                oQuery.CommandText = innerSql;
                oQuery.Add("@TABLE_NAME", SqlDbType.VarChar, sTABLE_NAME + "_CSTM");
                dt = oQuery.GetTable();
                Cache.Insert("vwFIELDS_META_DATA_Unvalidated." + sTABLE_NAME, dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static DataTable ForumTopics()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwFORUM_TOPICS_LISTBOX") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1739"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwFORUM_TOPICS_LISTBOX", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearSavedSearch(string sMODULE)
        {
            HttpContext.Current.Session.Remove("vwSAVED_SEARCH." + sMODULE);
        }

        public static DataTable SavedSearch(string sMODULE)
        {
            HttpSessionState Session = HttpContext.Current.Session;
            var dt = Session["vwSAVED_SEARCH." + sMODULE] as DataTable;
            if (dt == null)
            {
                dt = new DataTable();
                if (Security.IsAuthenticated())
                {
                    var oQuery = new InlineQueryDBManager();
                    string innerSql = "select ID                         " + ControlChars.CrLf
                                      + "     , NAME                       " + ControlChars.CrLf
                                      + "     , CONTENTS                   " + ControlChars.CrLf
                                      + "  from vwSAVED_SEARCH             " + ControlChars.CrLf
                                      + " where ASSIGNED_USER_ID = @USER_ID" + ControlChars.CrLf
                                      + "   and SEARCH_MODULE    = @MODULE " + ControlChars.CrLf
                                      + " order by NAME                    " + ControlChars.CrLf;
                    oQuery.CommandText = innerSql;
                    oQuery.Add("@USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                    oQuery.Add("@MODULE", SqlDbType.VarChar, sMODULE);
                    dt = oQuery.GetTable();
                    Session["vwSAVED_SEARCH." + sMODULE] = dt;
                }
            }
            return dt;
        }

        public static void ClearSearchColumns(string sVIEW_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwSqlColumns_Searching." + sVIEW_NAME);
        }

        public static DataTable SearchColumns(string sVIEW_NAME)
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwSqlColumns_Searching." + sVIEW_NAME) as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1926"].ToString();
                oQuery.CommandText = innerSql;
                oQuery.Add("@OBJECTNAME", SqlDbType.VarChar, sVIEW_NAME);
                dt = oQuery.GetTable();
                Cache.Insert("vwSqlColumns_Searching." + sVIEW_NAME, dt, null, DefaultCacheExpiration(),
                             Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearEmailGroups()
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwUSERS_Groups");
        }

        public static DataTable EmailGroups()
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwUSERS_Groups") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_1972"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwUSERS_Groups", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }

        public static void ClearInboundEmails()
        {
            Cache Cache = HttpRuntime.Cache;
            Cache.Remove("vwINBOUND_EMAILS_Bounce");
            Cache.Remove("vwINBOUND_EMAILS_Monitored");
        }


        public static DataTable InboundEmailBounce()
        {
            return InboundEmailBounce(HttpContext.Current.Application);
        }


        public static DataTable InboundEmailBounce(HttpApplicationState Application)
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwINBOUND_EMAILS_Bounce") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_2024"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwINBOUND_EMAILS_Bounce", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }


        public static DataTable InboundEmailMonitored(HttpApplicationState Application)
        {
            Cache Cache = HttpRuntime.Cache;
            var dt = Cache.Get("vwINBOUND_EMAILS_Monitored") as DataTable;
            if (dt == null)
            {
                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["_code_CRMCache_2063"].ToString();
                oQuery.CommandText = innerSql;
                dt = oQuery.GetTable();
                Cache.Insert("vwINBOUND_EMAILS_Monitored", dt, null, DefaultCacheExpiration(), Cache.NoSlidingExpiration);
            }
            return dt;
        }
    }
}