using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using CRM.BusinessLogic;
using CRM.Common;

namespace CRM.InvoiceManagement.Products.ProductCatalog
{
    public class LineItem
    {
        public Decimal COST_PRICE;
        public Decimal COST_USDOLLAR;
        public Guid ID;
        public Decimal LIST_PRICE;
        public Decimal LIST_USDOLLAR;
        public string MFT_PART_NUM;
        public string NAME;
        public string TAX_CLASS;
        public Decimal UNIT_PRICE;
        public Decimal UNIT_USDOLLAR;
        public string VENDOR_PART_NUM;

        public LineItem()
        {
            ID = Guid.Empty;
            NAME = String.Empty;
            MFT_PART_NUM = String.Empty;
            VENDOR_PART_NUM = String.Empty;
            TAX_CLASS = String.Empty;
            COST_PRICE = Decimal.Zero;
            COST_USDOLLAR = Decimal.Zero;
            LIST_PRICE = Decimal.Zero;
            LIST_USDOLLAR = Decimal.Zero;
            UNIT_PRICE = Decimal.Zero;
            UNIT_USDOLLAR = Decimal.Zero;
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
        public LineItem GetItemDetailsByNumber(Guid gCURRENCY_ID, string sMFT_PART_NUM)
        {
            var item = new LineItem();
            {
                if (Security.USER_ID == Guid.Empty)
                    throw (new Exception("Authentication required"));

                var oQuery = new InlineQueryDBManager();
                oQuery.CommandText = ApplicationSQL.SQL["ProductCatalog_AutoComplete_63"].ToString();

                CRMSecurity.Filter(oQuery, "ProductTemplates", "list");
                TypeConvert.AppendParameter(oQuery, sMFT_PART_NUM, CommonTypeConvert.SqlFilterMode.StartsWith,
                                            "MFT_PART_NUM");
                oQuery.CommandText += " order by MFT_PART_NUM" + ControlChars.CrLf;
                using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (rdr.Read())
                    {
                        item.ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                        item.NAME = CommonTypeConvert.ToString(rdr["NAME"]);
                        item.MFT_PART_NUM = CommonTypeConvert.ToString(rdr["MFT_PART_NUM"]);
                        item.VENDOR_PART_NUM = CommonTypeConvert.ToString(rdr["VENDOR_PART_NUM"]);
                        item.TAX_CLASS = CommonTypeConvert.ToString(rdr["TAX_CLASS"]);
                        item.COST_PRICE = CommonTypeConvert.ToDecimal(rdr["COST_PRICE"]);
                        item.COST_USDOLLAR = CommonTypeConvert.ToDecimal(rdr["COST_USDOLLAR"]);
                        item.LIST_PRICE = CommonTypeConvert.ToDecimal(rdr["LIST_PRICE"]);
                        item.LIST_USDOLLAR = CommonTypeConvert.ToDecimal(rdr["LIST_USDOLLAR"]);
                        item.UNIT_PRICE = CommonTypeConvert.ToDecimal(rdr["UNIT_PRICE"]);
                        item.UNIT_USDOLLAR = CommonTypeConvert.ToDecimal(rdr["UNIT_USDOLLAR"]);
                        if (gCURRENCY_ID != CommonTypeConvert.ToGuid(rdr["CURRENCY_ID"]))
                        {
                            Currency C10n = Currency.CreateCurrency(gCURRENCY_ID);
                            item.COST_PRICE = Currency.GetCurrency.ToCurrency(item.COST_USDOLLAR);
                            item.LIST_PRICE = Currency.GetCurrency.ToCurrency(item.LIST_USDOLLAR);
                            item.UNIT_PRICE = Currency.GetCurrency.ToCurrency(item.UNIT_USDOLLAR);
                        }
                    }
                }
                if (CommonTypeConvert.IsEmptyGuid(item.ID))
                    throw (new Exception("Item not found"));
            }
            return item;
        }

        [WebMethod(EnableSession = true)]
        public LineItem GetItemDetailsByName(Guid gCURRENCY_ID, string sNAME)
        {
            var item = new LineItem();
            {
                if (Security.USER_ID == Guid.Empty)
                    throw (new Exception("Authentication required"));
                var oQuery = new InlineQueryDBManager();
                oQuery.CommandText = ApplicationSQL.SQL["ProductCatalog_AutoComplete_116"].ToString();
                CRMSecurity.Filter(oQuery, "ProductTemplates", "list");
                TypeConvert.AppendParameter(oQuery, sNAME, CommonTypeConvert.SqlFilterMode.StartsWith, "NAME");
                oQuery.CommandText += " order by NAME" + ControlChars.CrLf;
                using (SqlDataReader rdr = oQuery.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (rdr.Read())
                    {
                        item.ID = CommonTypeConvert.ToGuid(rdr["ID"]);
                        item.NAME = CommonTypeConvert.ToString(rdr["NAME"]);
                        item.MFT_PART_NUM = CommonTypeConvert.ToString(rdr["MFT_PART_NUM"]);
                        item.VENDOR_PART_NUM = CommonTypeConvert.ToString(rdr["VENDOR_PART_NUM"]);
                        item.TAX_CLASS = CommonTypeConvert.ToString(rdr["TAX_CLASS"]);
                        item.COST_PRICE = CommonTypeConvert.ToDecimal(rdr["COST_PRICE"]);
                        item.COST_USDOLLAR = CommonTypeConvert.ToDecimal(rdr["COST_USDOLLAR"]);
                        item.LIST_PRICE = CommonTypeConvert.ToDecimal(rdr["LIST_PRICE"]);
                        item.LIST_USDOLLAR = CommonTypeConvert.ToDecimal(rdr["LIST_USDOLLAR"]);
                        item.UNIT_PRICE = CommonTypeConvert.ToDecimal(rdr["UNIT_PRICE"]);
                        item.UNIT_USDOLLAR = CommonTypeConvert.ToDecimal(rdr["UNIT_USDOLLAR"]);
                        if (gCURRENCY_ID != CommonTypeConvert.ToGuid(rdr["CURRENCY_ID"]))
                        {
                            Currency C10n = Currency.CreateCurrency(gCURRENCY_ID);
                            item.COST_PRICE = Currency.GetCurrency.ToCurrency(item.COST_USDOLLAR);
                            item.LIST_PRICE = Currency.GetCurrency.ToCurrency(item.LIST_USDOLLAR);
                            item.UNIT_PRICE = Currency.GetCurrency.ToCurrency(item.UNIT_USDOLLAR);
                        }
                    }
                }
            }
            if (CommonTypeConvert.IsEmptyGuid(item.ID))
                throw (new Exception("Item not found"));

            return item;
        }

        [WebMethod(EnableSession = true)]
        public string[] ItemNumberList(string prefixText, int count)
        {
            var arrItems = new string[0];

            if (Security.USER_ID == Guid.Empty)
                throw (new Exception("Authentication required"));

            var oQuery = new InlineQueryDBManager();
            oQuery.CommandText = ApplicationSQL.SQL["ProductCatalog_AutoComplete_171"].ToString();

            CRMSecurity.Filter(oQuery, "ProductTemplates", "list");
            TypeConvert.AppendParameter(oQuery, prefixText, CommonTypeConvert.SqlFilterMode.StartsWith, "MFT_PART_NUM");
            oQuery.CommandText += " order by MFT_PART_NUM" + ControlChars.CrLf;

            using (DataTable dt = oQuery.GetTable())
            {
                arrItems = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                    arrItems[i] = CommonTypeConvert.ToString(dt.Rows[i]["MFT_PART_NUM"]);
            }


            return arrItems;
        }

        [WebMethod(EnableSession = true)]
        public string[] ItemNameList(string prefixText, int count)
        {
            var arrItems = new string[0];
            if (Security.USER_ID == Guid.Empty)
                throw (new Exception("Authentication required"));

            var oQuery = new InlineQueryDBManager();
            oQuery.CommandText = ApplicationSQL.SQL["ProductCatalog_AutoComplete_212"].ToString();
            CRMSecurity.Filter(oQuery, "ProductTemplates", "list");
            TypeConvert.AppendParameter(oQuery, prefixText, CommonTypeConvert.SqlFilterMode.StartsWith, "NAME");
            oQuery.CommandText += " order by NAME" + ControlChars.CrLf;

            using (DataTable dt = oQuery.GetTable())
            {
                arrItems = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                    arrItems[i] = CommonTypeConvert.ToString(dt.Rows[i]["NAME"]);
            }


            return arrItems;
        }
    }
}