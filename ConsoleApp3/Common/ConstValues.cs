using System.Collections.Generic;

namespace TheLast.Common
{
    public static class ConstValues
    {
        public static string COIN_LIST = "https://www.binance.com/bapi/asset/v2/public/asset-service/product/get-products?includeEtf=true";
        public static string COIN_DETAIL = "https://www.binance.com/api/v1/klines?";
        public static string COIN_SINGLE = "https://www.binance.com/vi/trade/";
        public static List<string> BLACK_LIST = new List<string> { "PSGUSDT", "ACMUSDT", "ATMUSDT", "BUSDUSDT", "ASRUSDT" }; 
    }
}
