using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using TheLast.Model;

namespace TheLast.Common
{
    public static class ExtensionMethod
    {
        public static IEnumerable<TResult> SelectWithPrevious<TSource, TResult>
        (this IEnumerable<TSource> source,
         Func<TSource, TSource, TResult> projection)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    yield break;
                }
                TSource previous = iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return projection(previous, iterator.Current);
                    previous = iterator.Current;
                }
            }
        }

        //public static List<CandleStickModel> ToDataModel(this DataJsonModel model)
        //{
        //    var lstCandleStick = new List<CandleStickModel>();

        //    var count = model.T.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        var time = model.T.ElementAt(i).UnixTimeStampToDateTime();
        //        lstCandleStick.Add(new CandleStickModel
        //        {
        //            Time = time,
        //            Low = model.L.ElementAt(i),
        //            High = model.H.ElementAt(i),
        //            Open = model.O.ElementAt(i),
        //            Close = model.C.ElementAt(i)
        //        });
        //    }
        //    return lstCandleStick;
        //}

        public static List<long> GetColumnInt(this JArray obj, int col)
        {
            return Enumerable.Range(0, obj.Count()).Select(x => long.Parse(obj[x][0].ToString()) / 1000).ToList();
        }

        public static List<float> GetColumnFloat(this JArray obj, int col)
        {
            return Enumerable.Range(0, obj.Count()).Select(x => float.Parse(obj[x][0].ToString())).ToList();
        }

        //public static (List<CandleStickModel>, List<BarModel>) ToDataModelWeek(this DataJsonModel model)
        //{
        //    var lstCandleStick = new List<CandleStickModel>();
        //    var lstBar = new List<BarModel>();
        //    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        //    var count = model.T.Count;
        //    var weekNo = 0;
        //    DateTime T = DateTime.Now;
        //    float L = 0;
        //    float H = 0;
        //    float O = 0;
        //    float C_Local = 0;
        //    long V_Local = 0;

        //    for (int i = 0; i < count; i++)
        //    {
        //        var time = model.T.ElementAt(i).UnixTimeStampToDateTime();
        //        Calendar cal = dfi.Calendar;
        //        var weekNo_Local = cal.GetWeekOfYear(time, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        //        if (weekNo_Local == weekNo)
        //        {
        //            if (model.L.ElementAt(i) < L)
        //                L = model.L.ElementAt(i);
        //            if (model.H.ElementAt(i) > H)
        //                H = model.H.ElementAt(i);
        //            C_Local = model.C.ElementAt(i);
        //            V_Local += model.V.ElementAt(i);
        //        }
        //        else
        //        {
        //            if (weekNo > 0)
        //            {
        //                lstCandleStick.Add(new CandleStickModel
        //                {
        //                    Time = T,
        //                    Low = L,
        //                    High = H,
        //                    Open = O,
        //                    Close = C_Local
        //                });
        //                lstBar.Add(new BarModel
        //                {
        //                    Time = T,
        //                    Value = V_Local
        //                });
        //            }

        //            weekNo = weekNo_Local;
        //            T = time;
        //            L = model.L.ElementAt(i);
        //            H = model.H.ElementAt(i);
        //            O = model.O.ElementAt(i);
        //            C_Local = model.C.ElementAt(i);
        //            V_Local = model.V.ElementAt(i);
        //        }
        //    }
        //    if (weekNo > 0)
        //    {
        //        lstCandleStick.Add(new CandleStickModel
        //        {
        //            Time = T,
        //            Low = L,
        //            High = H,
        //            Open = O,
        //            Close = C_Local
        //        });
        //        lstBar.Add(new BarModel
        //        {
        //            Time = T,
        //            Value = V_Local
        //        });
        //    }

        //    return (lstCandleStick, lstBar);
        //}
        public static DateTime UnixTimeStampToDateTime(this int unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static string To2Digit(this int val)
        {
            var strOutput = "00" + val.ToString();
            return strOutput.Substring(strOutput.Length - 2, 2);
        }
        public static void InitBasicJson(this int val)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\basic_setting.json";
            var isExist = File.Exists(path);
            if (!isExist)
            {
                var model = new BasicSettingModel
                {
                    TimeZone = (int)enumTimeZone.OneHour,
                    Nen = (int)enumCandleStick.C,
                    KhoiLuong = 20,
                    MA = 7,
                    EMA = 7,
                    MACD = new MACDModel { High = 12, Low = 26, Signal = 9 },
                    RSI = 14,
                    ADX = 14
                };
                string json = JsonConvert.SerializeObject(model);

                //write string to file
                File.WriteAllText(path, json);
            }
        }
       
        public static BasicSettingModel LoadBasicJson(this int val)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\basic_setting.json";
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                var result = JsonConvert.DeserializeObject<BasicSettingModel>(json);
                return result;
            }
        }

        public static void UpdateJson(this BasicSettingModel _model)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\basic_setting.json";
            string json = JsonConvert.SerializeObject(_model);

            //write string to file
            File.WriteAllText(path, json);
        }

        public static void InitAdvanceJson(this int val)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\advance_setting.json";
            var isExist = File.Exists(path);
            if (!isExist)
            {
                var model = new AdvanceSettingModel
                {
                    LstInteval = new List<IntervalModel>()
                };
                string json = JsonConvert.SerializeObject(model);
                
                //write string to file
                File.WriteAllText(path, json);
            }
        }

        public static AdvanceSettingModel LoadAdvanceJson(this int val)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\advance_setting.json";
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                var result = JsonConvert.DeserializeObject<AdvanceSettingModel>(json);
                return result;
            }
        }

        public static void UpdateJson(this AdvanceSettingModel _model)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\advance_setting.json";
            string json = JsonConvert.SerializeObject(_model);

            //write string to file
            File.WriteAllText(path, json);
        }

        public static List<string> ToList(this DataTable _dt, string _columnName)
        {
            var lstReturn = new List<string>();
            foreach (var item in _dt.AsEnumerable())
            {
                lstReturn.Add(item[_columnName].ToString());
            }
            return lstReturn;
        }

        public static string ToDisplayStatus(this int val)
        {
            return ((enumTimeZone)val).GetAttribute<DisplayAttribute>().Name;
        }
        public static string ToDisplayStatus(this enumTimeZone val)
        {
            return val.GetAttribute<DisplayAttribute>().Name;
        }
        public static string ToDisplayStatus(this enumCandleStick val)
        {
            return val.GetAttribute<DisplayAttribute>().Name;
        }
        public static string ToDisplayStatus(this enumChooseData val)
        {
            return val.GetAttribute<DisplayAttribute>().Name;
        }
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
           where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static T GetJsonFile<T>(string url)
        {
            if (WebRequest.Create(url) is HttpWebRequest webRequest)
            {
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Nothing";

                using (var s = webRequest.GetResponse().GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        var contributorsAsJson = sr.ReadToEnd();
                        var contributors = JsonConvert.DeserializeObject<T>(contributorsAsJson);
                        return (T)Convert.ChangeType(contributors, typeof(T));
                    }
                }
            }
            return (T)Convert.ChangeType(null, typeof(T));
        }
        public static JArray GetJsonArray(string url)
        {
            if (WebRequest.Create(url) is HttpWebRequest webRequest)
            {
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "Nothing";
                try
                {
                    using (var s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (var sr = new StreamReader(s))
                        {
                            var contributorsAsJson = sr.ReadToEnd();
                            var contributors = JArray.Parse(contributorsAsJson);
                            return contributors;
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        public static string ToIntervalValue(this int val)
        {
            if (val == (int)enumTimeZone.ThirteenMinute)
                return "15m";
            if (val == (int)enumTimeZone.OneHour)
                return "1h";
            if (val == (int)enumTimeZone.FourHour)
                return "4h";
            if (val == (int)enumTimeZone.OneDay)
                return "1d";
            if (val == (int)enumTimeZone.OneWeek)
                return "1W";
            if (val == (int)enumTimeZone.OneMonth)
                return "1M";
            return string.Empty;
        }
        public static int ToInt(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            return int.Parse(val);
        }
        public static decimal ToDecimal(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return 0;
            return decimal.Parse(val);
        }
    }
}
