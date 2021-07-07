using System;
using System.Collections.Generic;
using System.Linq;

namespace TheLast.Common
{
    public static class Calculate
    {
        public static float MA(List<float> lstInput, int amp)
        {
            var result = 0;
            var count = lstInput.Count();
            if (count < amp)
                return result;
            var skip = count - amp;
            return lstInput.Skip(skip).Take(amp).Sum() / amp;
        }
        
        public static float CrossMACD(List<float> lstInput)
        {
            var lstResult = new List<float>();
            var macd = MACD(lstInput);
            lstResult.Add(macd);
            for (int i = 1; i < 20; i++)
            {
                lstResult.Add(MACD(lstInput, i));
            }
            var signal = EMA(lstResult, 9);
            return macd - signal;
        }

        public static float RSI(List<float> lstInput, int index = 0)
        {
            float result = 0f;
            var count = lstInput.Count();
            if ((count - index) < 14)
                return result;
            if (index != 0)
            {
                lstInput.RemoveRange(count - (1 + index), index);
            }
            lstInput = lstInput.Skip(count - 14).Take(14).ToList();
            var AG = lstInput.SelectWithPrevious((prev, curr) => curr - prev).Where(x => x > 0).Sum() / 14f;
            var AL = lstInput.SelectWithPrevious((prev, curr) => curr - prev).Where(x => x < 0).Sum() / 14f;
            if (AL == 0)
                AL = 1;
            AL = Math.Abs(AL);
            var RS = AG / AL;
            var RSI = 100 - (100 / (1 + RS));
            return RSI;
        }

        private static float EMA(List<float> lstInput, int amp)
        {
            float alpha = 2f / (amp + 1);
            return lstInput.DefaultIfEmpty()
            .Aggregate((ema, nextQuote) => alpha * nextQuote + (1 - alpha) * ema);
        }
       
        private static float MACD(List<float> lstInput, int index = 0)
        {
            float result = 0;
            var count = lstInput.Count();
            if ((count - index) < 26)
                return result;
            if (index != 0)
            {
                lstInput.RemoveRange(count - (1 + index), index);
            }
            var EMA12 = EMA(lstInput, 12);
            var EMA26 = EMA(lstInput, 26);
            result = EMA12 - EMA26;
            return result;
        }
    }
}
