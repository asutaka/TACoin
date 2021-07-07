using System.Collections.Generic;
using System.Data;
using System.Linq;
using TheLast.Model;

namespace TheLast.Common
{
    public static class SeedData
    {
        public static DataTable GetDataTimeZone()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            var dr1 = dt.NewRow();
            dr1["Id"] = (int)enumTimeZone.ThirteenMinute;
            dr1["Name"] = enumTimeZone.ThirteenMinute.ToDisplayStatus();

            var dr2 = dt.NewRow();
            dr2["Id"] = (int)enumTimeZone.OneHour;
            dr2["Name"] = enumTimeZone.OneHour.ToDisplayStatus();

            var dr3 = dt.NewRow();
            dr3["Id"] = (int)enumTimeZone.FourHour;
            dr3["Name"] = enumTimeZone.FourHour.ToDisplayStatus();

            var dr4 = dt.NewRow();
            dr4["Id"] = (int)enumTimeZone.OneDay;
            dr4["Name"] = enumTimeZone.OneDay.ToDisplayStatus();

            var dr5 = dt.NewRow();
            dr5["Id"] = (int)enumTimeZone.OneWeek;
            dr5["Name"] = enumTimeZone.OneWeek.ToDisplayStatus();

            var dr6 = dt.NewRow();
            dr6["Id"] = (int)enumTimeZone.OneMonth;
            dr6["Name"] = enumTimeZone.OneMonth.ToDisplayStatus();

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            dt.Rows.Add(dr4);
            dt.Rows.Add(dr5);
            dt.Rows.Add(dr6);
            return dt;
        }

        public static DataTable GetDataCandleStick()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            var dr1 = dt.NewRow();
            dr1["Id"] = (int)enumCandleStick.O;
            dr1["Name"] = enumCandleStick.O.ToDisplayStatus();

            var dr2 = dt.NewRow();
            dr2["Id"] = (int)enumCandleStick.H;
            dr2["Name"] = enumCandleStick.H.ToDisplayStatus();

            var dr3 = dt.NewRow();
            dr3["Id"] = (int)enumCandleStick.L;
            dr3["Name"] = enumCandleStick.L.ToDisplayStatus();

            var dr4 = dt.NewRow();
            dr4["Id"] = (int)enumCandleStick.C;
            dr4["Name"] = enumCandleStick.C.ToDisplayStatus();

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            dt.Rows.Add(dr4);
            return dt;
        }

        public static DataTable GetOperator()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            var dr1 = dt.NewRow();
            dr1["Id"] = 0;
            dr1["Name"] = ">";

            var dr2 = dt.NewRow();
            dr2["Id"] = 1;
            dr2["Name"] = ">=";

            var dr3 = dt.NewRow();
            dr3["Id"] = 2;
            dr3["Name"] = "=";

            var dr4 = dt.NewRow();
            dr4["Id"] = 3;
            dr4["Name"] = "<=";

            var dr5 = dt.NewRow();
            dr5["Id"] = 4;
            dr5["Name"] = "<";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            dt.Rows.Add(dr4);
            dt.Rows.Add(dr5);
            return dt;
        }
        public static DataTable GetUnit()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            var dr1 = dt.NewRow();
            dr1["Id"] = 0;
            dr1["Name"] = "%";

            var dr2 = dt.NewRow();
            dr2["Id"] = 1;
            dr2["Name"] = "giá trị";

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            return dt;
        }
        public static DataTable GetDataChooseData()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            var dr1 = dt.NewRow();
            dr1["Id"] = (int)enumChooseData.MA;
            dr1["Name"] = enumChooseData.MA.ToDisplayStatus();

            var dr2 = dt.NewRow();
            dr2["Id"] = (int)enumChooseData.EMA;
            dr2["Name"] = enumChooseData.EMA.ToDisplayStatus();

            var dr3 = dt.NewRow();
            dr3["Id"] = (int)enumChooseData.Volumne;
            dr3["Name"] = enumChooseData.Volumne.ToDisplayStatus();

            var dr4 = dt.NewRow();
            dr4["Id"] = (int)enumChooseData.CandleStick_1;
            dr4["Name"] = enumChooseData.CandleStick_1.ToDisplayStatus();

            var dr5 = dt.NewRow();
            dr5["Id"] = (int)enumChooseData.CandleStick_2;
            dr5["Name"] = enumChooseData.CandleStick_2.ToDisplayStatus();

            var dr6 = dt.NewRow();
            dr6["Id"] = (int)enumChooseData.MACD;
            dr6["Name"] = enumChooseData.MACD.ToDisplayStatus();

            var dr7 = dt.NewRow();
            dr7["Id"] = (int)enumChooseData.RSI;
            dr7["Name"] = enumChooseData.RSI.ToDisplayStatus();

            var dr8 = dt.NewRow();
            dr8["Id"] = (int)enumChooseData.ADX;
            dr8["Name"] = enumChooseData.ADX.ToDisplayStatus();

            dt.Rows.Add(dr1);
            dt.Rows.Add(dr2);
            dt.Rows.Add(dr3);
            dt.Rows.Add(dr4);
            dt.Rows.Add(dr5);
            dt.Rows.Add(dr6);
            dt.Rows.Add(dr7);
            return dt;
        }

        public static List<CryptonDetailModel> GetCryptonList()
        {
            var cryptonModel = ExtensionMethod.GetJsonFile<CryptonModel>(ConstValues.COIN_LIST);
            return cryptonModel.Data.Where(x => !ConstValues.BLACK_LIST.Contains(x.S) 
                                            && x.S.Substring(x.S.Length - 4) == "USDT"
                                            && !x.S.Contains("UP")
                                            && !x.S.Contains("DOWN"))
                                    .OrderBy(x => x.S).ToList();
        }
    }
}
