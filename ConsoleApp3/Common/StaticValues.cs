using System.Collections.Generic;
using TheLast.Model;

namespace TheLast.Common
{
    public class StaticValues
    {
        public static List<OutputModel> lstOutput = new List<OutputModel>();
        public static BasicSettingModel basicModel = 0.LoadBasicJson();
        public static AdvanceSettingModel advanceModel = 0.LoadAdvanceJson();
        public static List<CryptonDetailModel> lstCoin = SeedData.GetCryptonList();
    }
}
