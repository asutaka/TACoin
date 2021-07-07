namespace TheLast.Model
{
    public class BasicSettingModel
    {
        public int TimeZone { get; set; }
        public int Nen { get; set; }
        public int KhoiLuong { get; set; }
        public int MA { get; set; }
        public int EMA { get; set; }
        public MACDModel MACD { get; set; }
        public int RSI { get; set; }
        public int ADX { get; set; }
    }
    public class MACDModel
    {
        public int High { get; set; }
        public int Low { get; set; }
        public int Signal { get; set; }
    }
}
