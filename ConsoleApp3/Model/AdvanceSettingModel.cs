using System.Collections.Generic;

namespace TheLast.Model
{
    public class AdvanceSettingModel
    {
        public List<IntervalModel> LstInteval { get; set; }
    }
    public class IntervalModel
    {
        public int Interval { get; set; }
        public List<IndicatorModel> LstIndicator { get; set; }
    }
    public class IndicatorModel
    {
        public string IndicatorFirst { get; set; }
        public string IndicatorFirstValue { get; set; }
        public string IndicatorLast { get; set; }
        public string IndicatorLastValue { get; set; }
        public string Operator { get; set; }
        public decimal Result { get; set; }
        public string Unit { get; set; }
        public decimal Point { get; set; }
    }
}
