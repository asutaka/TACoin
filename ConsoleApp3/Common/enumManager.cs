using System.ComponentModel.DataAnnotations;

namespace TheLast.Common
{
    public enum enumTimeZone
    {
        [Display(Name = "15 Phút")]
        ThirteenMinute = 0,
        [Display(Name = "1 Giờ")]
        OneHour = 1,
        [Display(Name = "4 Giờ")]
        FourHour = 2,
        [Display(Name = "1 Ngày")]
        OneDay = 3,
        [Display(Name = "1 Tuần")]
        OneWeek = 4,
        [Display(Name = "1 Tháng")]
        OneMonth = 5,
    }
    public enum enumCandleStick
    {
        [Display(Name = "Open")]
        O = 0,
        [Display(Name = "High")]
        H = 1,
        [Display(Name = "Low")]
        L = 2,
        [Display(Name = "Close")]
        C = 3
    }
    public enum enumChooseData
    {
        [Display(Name = "MA")]
        MA = 0,
        [Display(Name = "EMA")]
        EMA = 1,
        [Display(Name = "Volumne")]
        Volumne = 2,
        [Display(Name = "Nến 1")]
        CandleStick_1 = 3,
        [Display(Name = "Nến 2")]
        CandleStick_2 = 4,
        [Display(Name = "MACD")]
        MACD = 5,
        [Display(Name = "RSI")]
        RSI = 6,
        [Display(Name = "ADX")]
        ADX = 7,
    }
}
