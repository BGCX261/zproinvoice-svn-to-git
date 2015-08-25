using System;
using System.Data;
using CRM.Common;
using CRM.DataAccess;

namespace CRM.BusinessLogic
{
    public class ProductDBManager
    {
        public void ProductProductUpdate(Guid gPARENT_ID, Guid gCHILD_ID)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                oDm.Add("@PARENT_ID", SqlDbType.UniqueIdentifier, gPARENT_ID);
                oDm.Add("@CHILD_ID", SqlDbType.UniqueIdentifier, gCHILD_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_PRODUCT_Update");
            }
        }

        public void ProductTemplatesDelete(Guid gID)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_TEMPLATES_Delete");
            }
        }

        public void ProductTemplatesUpdate(ref Guid gID, string sNAME, string sSTATUS, Int32 nQUANTITY,
                                           DateTime dtDATE_AVAILABLE, DateTime dtDATE_COST_PRICE, Guid gACCOUNT_ID,
                                           Guid gMANUFACTURER_ID, Guid gCATEGORY_ID, Guid gTYPE_ID, string sWEBSITE,
                                           string sMFT_PART_NUM, string sVENDOR_PART_NUM, string sTAX_CLASS,
                                           float flWEIGHT, Guid gCURRENCY_ID, decimal dCOST_PRICE, decimal dLIST_PRICE,
                                           decimal dDISCOUNT_PRICE, Int32 nPRICING_FACTOR, string sPRICING_FORMULA,
                                           string sSUPPORT_NAME, string sSUPPORT_CONTACT, string sSUPPORT_DESCRIPTION,
                                           string sSUPPORT_TERM, string sDESCRIPTION)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                oDm.Add("@NAME", SqlDbType.NVarChar, 50, sNAME);
                oDm.Add("@STATUS", SqlDbType.NVarChar, 25, sSTATUS);
                oDm.Add("@QUANTITY", SqlDbType.Int, nQUANTITY);
                oDm.Add("@DATE_AVAILABLE", SqlDbType.DateTime, dtDATE_AVAILABLE); //DATE_COST_PRICE
                oDm.Add("@DATE_COST_PRICE", SqlDbType.DateTime, dtDATE_COST_PRICE);
                oDm.Add("@ACCOUNT_ID", SqlDbType.UniqueIdentifier, gACCOUNT_ID);
                oDm.Add("@MANUFACTURER_ID", SqlDbType.UniqueIdentifier, gMANUFACTURER_ID);
                oDm.Add("@CATEGORY_ID", SqlDbType.UniqueIdentifier, gCATEGORY_ID);
                oDm.Add("@TYPE_ID", SqlDbType.UniqueIdentifier, gTYPE_ID);
                oDm.Add("@WEBSITE", SqlDbType.NVarChar, 255, sWEBSITE);
                oDm.Add("@MFT_PART_NUM", SqlDbType.NVarChar, 50, sMFT_PART_NUM);
                oDm.Add("@VENDOR_PART_NUM", SqlDbType.NVarChar, 50, sVENDOR_PART_NUM);
                oDm.Add("@TAX_CLASS", SqlDbType.NVarChar, 25, sTAX_CLASS);
                oDm.Add("@WEIGHT", SqlDbType.Float, 53, flWEIGHT);
                oDm.Add("@CURRENCY_ID", SqlDbType.UniqueIdentifier, gCURRENCY_ID);
                oDm.Add("@COST_PRICE", SqlDbType.Money, dCOST_PRICE); //--
                oDm.Add("@LIST_PRICE", SqlDbType.Money, dLIST_PRICE);
                oDm.Add("@DISCOUNT_PRICE", SqlDbType.Money, dDISCOUNT_PRICE);
                oDm.Add("@PRICING_FACTOR", SqlDbType.Int, nPRICING_FACTOR);
                oDm.Add("@PRICING_FORMULA", SqlDbType.NVarChar, 25, sPRICING_FORMULA);
                oDm.Add("@SUPPORT_NAME", SqlDbType.NVarChar, 50, sSUPPORT_NAME);
                oDm.Add("@SUPPORT_CONTACT", SqlDbType.NVarChar, 50, sSUPPORT_CONTACT);
                oDm.Add("@SUPPORT_DESCRIPTION", SqlDbType.NVarChar, 255, sSUPPORT_DESCRIPTION);
                oDm.Add("@SUPPORT_TERM", SqlDbType.NVarChar, 25, sSUPPORT_TERM);
                oDm.Add("@DESCRIPTION", SqlDbType.Text, sDESCRIPTION);


                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_TEMPLATES_Update");
                gID = CommonTypeConvert.ToGuid(oDm["@ID"].Value);
            }
        }

        public void ProductTypesDelete(Guid gID)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_TYPES_Delete");
            }
        }

        public void ProductTypesUpdate(ref Guid gID, string sNAME, string sDESCRIPTION, Int32 nLIST_ORDER)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                oDm.Add("@NAME", SqlDbType.NVarChar, sNAME);
                oDm.Add("@DESCRIPTION", SqlDbType.NVarChar, 50, sDESCRIPTION);
                oDm.Add("@LIST_ORDER", SqlDbType.Int, nLIST_ORDER);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_TYPES_Update");
                gID = CommonTypeConvert.ToGuid(oDm["@ID"].Value);
            }
        }

        public void ProductDelete(Guid gID)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCTS_Delete");
            }
        }

        public void ProductMassDelete(string sID_LIST)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID_LIST", SqlDbType.VarChar, 8000, sID_LIST);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCTS_MassDelete");
            }
        }

        public void ProductUpdate(ref Guid gID, Guid gPRODUCT_TEMPLATE_ID, string sNAME, string sSTATUS,
                                  Guid gACCOUNT_ID, Guid gCONTACT_ID, Int32 nQUANTITY, DateTime dtDATE_PURCHASED,
                                  DateTime dtDATE_SUPPORT_EXPIRES, DateTime dtDATE_SUPPORT_STARTS, Guid gMANUFACTURER_ID,
                                  Guid gCATEGORY_ID, Guid gTYPE_ID, string sWEBSITE, string sMFT_PART_NUM,
                                  string sVENDOR_PART_NUM, string sSERIAL_NUMBER, string sASSET_NUMBER,
                                  string sTAX_CLASS, float flWEIGHT, Guid gCURRENCY_ID, decimal dCOST_PRICE,
                                  decimal dLIST_PRICE, decimal dBOOK_VALUE, DateTime dtBOOK_VALUE_DATE,
                                  decimal dDISCOUNT_PRICE, Int32 nPRICING_FACTOR, string sPRICING_FORMULA,
                                  string sSUPPORT_NAME, string sSUPPORT_CONTACT, string sSUPPORT_DESCRIPTION,
                                  string sSUPPORT_TERM, string sDESCRIPTION, Guid gTEAM_ID)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                oDm.Add("@PRODUCT_TEMPLATE_ID", SqlDbType.UniqueIdentifier, gPRODUCT_TEMPLATE_ID);
                oDm.Add("@NAME", SqlDbType.NVarChar, 50, sNAME);
                oDm.Add("@STATUS", SqlDbType.NVarChar, 25, sSTATUS);
                oDm.Add("@ACCOUNT_ID", SqlDbType.UniqueIdentifier, gACCOUNT_ID);
                oDm.Add("@CONTACT_ID", SqlDbType.UniqueIdentifier, gCONTACT_ID);
                oDm.Add("@QUANTITY", SqlDbType.Int, nQUANTITY);
                oDm.Add("@DATE_PURCHASED", SqlDbType.DateTime, dtDATE_PURCHASED);
                oDm.Add("@DATE_SUPPORT_EXPIRES", SqlDbType.DateTime, dtDATE_SUPPORT_EXPIRES); //DATE_COST_PRICE
                oDm.Add("@DATE_SUPPORT_STARTS", SqlDbType.DateTime, dtDATE_SUPPORT_STARTS);
                oDm.Add("@MANUFACTURER_ID", SqlDbType.UniqueIdentifier, gMANUFACTURER_ID);
                oDm.Add("@CATEGORY_ID", SqlDbType.UniqueIdentifier, gCATEGORY_ID);
                oDm.Add("@TYPE_ID", SqlDbType.UniqueIdentifier, gTYPE_ID);
                oDm.Add("@WEBSITE", SqlDbType.NVarChar, 255, sWEBSITE);
                oDm.Add("@MFT_PART_NUM", SqlDbType.NVarChar, 50, sMFT_PART_NUM);
                oDm.Add("@VENDOR_PART_NUM", SqlDbType.NVarChar, 50, sVENDOR_PART_NUM);
                oDm.Add("@SERIAL_NUMBER", SqlDbType.NVarChar, 50, sSERIAL_NUMBER);
                oDm.Add("@ASSET_NUMBER", SqlDbType.NVarChar, 50, sASSET_NUMBER);
                oDm.Add("TAX_CLASS", SqlDbType.NVarChar, 25, sTAX_CLASS);
                oDm.Add("@WEIGHT", SqlDbType.Float, 53, flWEIGHT);
                oDm.Add("@CURRENCY_ID", SqlDbType.UniqueIdentifier, gCURRENCY_ID);
                oDm.Add("@COST_PRICE", SqlDbType.Decimal, dCOST_PRICE); //--
                oDm.Add("@LIST_PRICE", SqlDbType.Decimal, dLIST_PRICE);
                oDm.Add("@BOOK_VALUE", SqlDbType.Money, dBOOK_VALUE);
                oDm.Add("@BOOK_VALUE_DATE", SqlDbType.DateTime, dtBOOK_VALUE_DATE);
                oDm.Add("@DISCOUNT_PRICE", SqlDbType.Decimal, dDISCOUNT_PRICE);
                oDm.Add("@PRICING_FACTOR", SqlDbType.Int, nPRICING_FACTOR);
                oDm.Add("@PRICING_FORMULA", SqlDbType.NVarChar, 25, sPRICING_FORMULA);
                oDm.Add("@SUPPORT_NAME", SqlDbType.NVarChar, 50, sSUPPORT_NAME);
                oDm.Add("@SUPPORT_CONTACT", SqlDbType.NVarChar, 50, sSUPPORT_CONTACT);
                oDm.Add("@SUPPORT_DESCRIPTION", SqlDbType.NVarChar, 255, sSUPPORT_DESCRIPTION);
                oDm.Add("@SUPPORT_TERM", SqlDbType.NVarChar, 25, sSUPPORT_TERM);
                oDm.Add("@DESCRIPTION", SqlDbType.Text, sDESCRIPTION);
                oDm.Add("@TeamID", SqlDbType.UniqueIdentifier, gTEAM_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCTS_Update");
                gID = CommonTypeConvert.ToGuid(oDm["@ID"].Value);
            }
        }

        public void ProductCategoriesDelete(Guid gID)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_CATEGORIES_Delete");
            }
        }

        public void ProductCategoriesUpdate(ref Guid gID, Guid gPARENT_ID, string sNAME, string sDESCRIPTION,
                                            Int32 nLIST_ORDER)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@ID", SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput, gID);
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                oDm.Add("@PARENT_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                oDm.Add("@NAME", SqlDbType.NVarChar, 50, gPARENT_ID);
                oDm.Add("@DESCRIPTION", SqlDbType.NText, sDESCRIPTION);
                oDm.Add("@LIST_ORDER", SqlDbType.Int, nLIST_ORDER);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_CATEGORIES_Update");
                gID = CommonTypeConvert.ToGuid(oDm["@ID"].Value);
            }
        }

        public void ProductProductDelete(Guid gPARENT_ID, Guid gCHILD_ID)
        {
            using (var oDm = new DataManager())
            {
                oDm.Add("@MODIFIED_USER_ID", SqlDbType.UniqueIdentifier, Security.USER_ID);
                oDm.Add("@PARENT_ID", SqlDbType.UniqueIdentifier, gPARENT_ID);
                oDm.Add("@CHILD_ID", SqlDbType.UniqueIdentifier, gCHILD_ID);

                oDm.CommandType = CommandType.StoredProcedure;
                oDm.ExecuteNonQuery("stp_Zpro_PRODUCT_PRODUCT_Delete");
            }
        }
    }
}