using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.Invoices
{
    public class Invoice
    {
        public Decimal AMOUNT_DUE;
        public Decimal AMOUNT_DUE_USDOLLAR;
        public Guid ID;
        public string NAME;

        public Invoice()
        {
            ID = Guid.Empty;
            NAME = String.Empty;
            AMOUNT_DUE = Decimal.Zero;
            AMOUNT_DUE_USDOLLAR = Decimal.Zero;
        }
    }

    /// <summary>
    /// Summary description for AutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class AutoComplete : WebService
    {
        [WebMethod(EnableSession = true)]
        public Invoice GetInvoiceByName(Guid gCURRENCY_ID, string sNAME)
        {
            var item = new Invoice();

            {
                if (Security.USER_ID == Guid.Empty)
                    throw (new Exception("Authentication required"));

                var oQuery = new InlineQueryDBManager();
                string innerSql = ApplicationSQL.SQL["Invoices_AutoComplete_50"].ToString();

                oQuery.CommandText = innerSql;
                CRMSecurity.Filter(oQuery, "Invoices", "list");
                TypeConvert.AppendParameter(oQuery, sNAME, CommonTypeConvert.SqlFilterMode.StartsWith, "NAME");
                using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (rdr.Read())
                    {
                        item.ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                        item.NAME = CommonTypeConvert.ToString(rdr["NAME"]);
                        item.AMOUNT_DUE = CommonTypeConvert.ToDecimal(rdr["AMOUNT_DUE"]);
                        item.AMOUNT_DUE_USDOLLAR = CommonTypeConvert.ToDecimal(rdr["AMOUNT_DUE_USDOLLAR"]);
                        if (gCURRENCY_ID != CommonTypeConvert.ToGuid(rdr["CURRENCY_ID"]))
                        {
                            Currency C10n = Currency.CreateCurrency(gCURRENCY_ID);
                            item.AMOUNT_DUE = Currency.GetCurrency.ToCurrency(item.AMOUNT_DUE_USDOLLAR);
                        }
                    }
                }
                if (CommonTypeConvert.IsEmptyGuid(item.ID))
                    throw (new Exception("Item not found"));
            }
            return item;
        }

        [WebMethod(EnableSession = true)]
        public string[] InvoiceNameList(string prefixText, int count)
        {
            var arrItems = new string[0];
            if (Security.USER_ID == Guid.Empty)
                throw (new Exception("Authentication required"));
            var oQuery = new InlineQueryDBManager();
            string innerSql = ApplicationSQL.SQL["Invoices_AutoComplete_94"].ToString();

            oQuery.CommandText = innerSql;
            CRMSecurity.Filter(oQuery, "Invoices", "list");
            TypeConvert.AppendParameter(oQuery, prefixText, CommonTypeConvert.SqlFilterMode.StartsWith, "NAME");
            innerSql += " order by NAME" + ControlChars.CrLf;
            DataTable dt = oQuery.GetTable();
            arrItems = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                arrItems[i] = CommonTypeConvert.ToString(dt.Rows[i]["NAME"]);
            return arrItems;
        }
    }
}