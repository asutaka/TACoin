using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheLast.Common;
using TheLast.Model;
using TicTacTec.TA.Library;

namespace TheLast
{
    public class Calculate
    {
        private BasicSettingModel _basicModel;
        private AdvanceSettingModel _advanceModel;
        private string _code;
        public Calculate(string Code)
        {
            _code = Code;
            _basicModel = StaticValues.basicModel;
            _advanceModel = StaticValues.advanceModel;
        }
        public OutputModel GetOutput()
        {
            var dtFrom = DateTime.Now;
            var lstOutputPoint = CalculatePoint();
            var dtTo = DateTime.Now;
            var timespan = dtTo - dtFrom;
            Console.WriteLine($"AvgTime:{_code}_{timespan.TotalMilliseconds}");
            decimal point = 0;
            foreach (var itemInterval in _advanceModel.LstInteval)
            {
                foreach (var itemIndicator in itemInterval.LstIndicator)
                {
                    double? localFirstValue = lstOutputPoint.FirstOrDefault(x => x.Interval == itemInterval.Interval 
                                                                        && x.Indicator == itemIndicator.IndicatorFirst 
                                                                        && (string.IsNullOrWhiteSpace(x.Period) 
                                                                            || x.Period == itemIndicator.IndicatorFirstValue))?.Value;
                    double? localLastValue = lstOutputPoint.FirstOrDefault(x => x.Interval == itemInterval.Interval
                                                                        && x.Indicator == itemIndicator.IndicatorLast
                                                                        && (string.IsNullOrWhiteSpace(x.Period)
                                                                            || x.Period == itemIndicator.IndicatorLastValue))?.Value;
                    var local = localFirstValue.GetValueOrDefault() - localLastValue.GetValueOrDefault();
                    double curResult = 0;
                    if(itemIndicator.Unit == "%")
                    {
                        var currentValue = GetCurrentValue();
                        curResult = Math.Abs(local) / currentValue;
                    }
                    else
                    {
                        curResult = local;
                    }

                    if(itemIndicator.Operator == ">")
                    {
                        if (curResult > (double)itemIndicator.Result)
                            point += itemIndicator.Point;
                    }
                    else if (itemIndicator.Operator == ">=")
                    {
                        if (curResult >= (double)itemIndicator.Result)
                            point += itemIndicator.Point;
                    }
                    else if (itemIndicator.Operator == "=")
                    {
                        if (curResult == (double)itemIndicator.Result)
                            point += itemIndicator.Point;
                    }
                    else if (itemIndicator.Operator == "<=")
                    {
                        if (curResult <= (double)itemIndicator.Result)
                            point += itemIndicator.Point;
                    }
                    else if (itemIndicator.Operator == "<")
                    {
                        if (curResult < (double)itemIndicator.Result)
                            point += itemIndicator.Point;
                    }
                }
            }
            return new OutputModel { Code = _code, Point = point };
        }
        private double GetCurrentValue()
        {
            var url = $"{ConstValues.COIN_DETAIL}symbol={_code}&interval=15m&limit=1";
            var arrData = ExtensionMethod.GetJsonArray(url);
            var currentVal = double.Parse(arrData[0][4].ToString());
            return currentVal;
        }
        private List<OutputPointModel> CalculatePoint()
        {
            var lstOutput = new List<OutputPointModel>();
            var lstTask = new List<Task>();
            foreach (var item in _advanceModel.LstInteval)
            {
                if(item.LstIndicator.Count() > 0)
                {
                    var task = Task.Run(() =>
                    {
                        lstOutput.AddRange(CalculateFromInterval(item));
                    });
                    lstTask.Add(task);
                }
            }
            Task.WaitAll(lstTask.ToArray());
            return lstOutput;
        }
        private List<OutputPointModel> CalculateFromInterval(IntervalModel model)
        {
            var lstOutputPoint = new List<OutputPointModel>();
            var lstModel = LoadDatasource(model.Interval.ToIntervalValue());
            var lstModelClose = lstModel.Select(x => x.Close).ToList();
            var lstModelHigh = lstModel.Select(x => x.High).ToList();
            var lstModelLow = lstModel.Select(x => x.Low).ToList();
            var lstModelVolumne = lstModel.Select(x => x.Volumne).ToList();
            var count = lstModel.Count;
            if (count < 20)
                return lstOutputPoint;

            var lstIndicator = model.LstIndicator.Select(x => new AdapterModel { Indicator = x.IndicatorFirst, Value = x.IndicatorFirstValue })
                        .Union(model.LstIndicator.Where(x => !string.IsNullOrWhiteSpace(x.IndicatorLast)).Select(x => new AdapterModel { Indicator = x.IndicatorLast, Value = x.IndicatorLastValue }))
                        .Distinct().ToList();
           
            foreach (var item in lstIndicator)
            {
                var outputModel = new OutputPointModel { Interval = model.Interval, Indicator = item.Indicator, Value = 0 };
                if (item.Indicator == enumChooseData.MA.ToString())
                {
                    var period = item.Value.ToInt();
                    double[] outReal = new double[1000];
                    Core.MovingAverage(0, count - 1, lstModelClose.ToArray(), period, Core.MAType.Sma, out int outBegIdx, out int outNBElement, outReal);
                    outputModel.Period = period.ToString();
                    outputModel.Value = outReal[count - period];
                    lstOutputPoint.Add(outputModel);
                }
                else if (item.Indicator == enumChooseData.EMA.ToString())
                {
                    var period = item.Value.ToInt();
                    double[] outReal = new double[1000];
                    Core.MovingAverage(0, count - 1, lstModelClose.ToArray(), period, Core.MAType.Ema, out int outBegIdx, out int outNBElement, outReal);
                    outputModel.Period = period.ToString();
                    outputModel.Value = outReal[count - period];
                    lstOutputPoint.Add(outputModel);
                }
                else if (item.Indicator == enumChooseData.Volumne.ToString())
                {
                    double[] outReal = new double[1000];
                    Core.MovingAverage(0, count - 1, lstModelVolumne.ToArray(), _basicModel.KhoiLuong, Core.MAType.Sma, out int outBegIdx, out int outNBElement, outReal);
                    outputModel.Value = outReal[count - _basicModel.KhoiLuong];
                    lstOutputPoint.Add(outputModel);
                }
                else if (item.Indicator == enumChooseData.CandleStick_1.ToString())
                {
                    lstOutputPoint.Add(GetOutputCandleStick(model, item, lstModel, 1));
                }
                else if (item.Indicator == enumChooseData.CandleStick_2.ToString())
                {
                    lstOutputPoint.Add(GetOutputCandleStick(model, item, lstModel, 2));
                }
                else if (item.Indicator == enumChooseData.MACD.ToString())
                {
                    double[] output1 = new double[1000];
                    double[] output2 = new double[1000];
                    double[] output3 = new double[1000];
                    Core.Macd(0, count - 1, lstModelClose.ToArray(), _basicModel.MACD.Low, _basicModel.MACD.High, _basicModel.MACD.Signal, out int outBegIdx, out int outNbElement, output1, output2, output3);
                    outputModel.Value = output1[count - 1];
                    lstOutputPoint.Add(outputModel);
                }
                else if (item.Indicator == enumChooseData.RSI.ToString())
                {
                    double[] outReal = new double[1000];
                    Core.Rsi(0, count - 1, lstModelClose.ToArray(), _basicModel.RSI, out int outBegIdx, out int outNBElement, outReal);
                    outputModel.Value = outReal[count - _basicModel.RSI];
                    lstOutputPoint.Add(outputModel);
                }
                else if (item.Indicator == enumChooseData.ADX.ToString())
                {
                    double[] outReal = new double[1000];
                    Core.Adx(0, count - 1, lstModelHigh.ToArray(), lstModelLow.ToArray(), lstModelClose.ToArray(), _basicModel.ADX, out int outBegIdx, out int outNBElement, outReal);
                    outputModel.Value = outReal[count - _basicModel.ADX];
                    lstOutputPoint.Add(outputModel);
                }
            }
            return lstOutputPoint;
        }
        private OutputPointModel GetOutputCandleStick(IntervalModel intervalModel, AdapterModel adapterModel, List<CandleStickModel> lstCanleStick, int type)//type = 1, 2
        {
            var count = lstCanleStick.Count;
            var outputModel = new OutputPointModel { Interval = intervalModel.Interval, Indicator = adapterModel.Indicator, Value = 0 };

            if (adapterModel.Value == enumCandleStick.O.ToString())
            {
                outputModel.Value = (type == 1) ? lstCanleStick.ElementAt(count - 1).Open : lstCanleStick.ElementAt(count - 2).Open;
            }
            else if (adapterModel.Value == enumCandleStick.H.ToString())
            {
                outputModel.Value = (type == 1) ? lstCanleStick.ElementAt(count - 1).High : lstCanleStick.ElementAt(count - 2).High;
            }
            else if (adapterModel.Value == enumCandleStick.L.ToString())
            {
                outputModel.Value = (type == 1) ? lstCanleStick.ElementAt(count - 1).Low : lstCanleStick.ElementAt(count - 2).Low;
            }
            else if (adapterModel.Value == enumCandleStick.C.ToString())
            {
                outputModel.Value = (type == 1) ? lstCanleStick.ElementAt(count - 1).Close : lstCanleStick.ElementAt(count - 2).Close;
            }
            return outputModel;
        }
        private List<CandleStickModel> LoadDatasource(string interval)
        {
            var url = $"{ConstValues.COIN_DETAIL}symbol={_code}&interval={interval}";
            var arrData = ExtensionMethod.GetJsonArray(url);
            var lstModel = new List<CandleStickModel>();
            lstModel = arrData.Select(x => new CandleStickModel
            {
                Time = ((int)((long)x[0] / 1000)).UnixTimeStampToDateTime(),
                Open = float.Parse(x[1].ToString()),
                High = float.Parse(x[2].ToString()),
                Low = float.Parse(x[3].ToString()),
                Close = float.Parse(x[4].ToString()),
                Volumne = float.Parse(x[5].ToString())
            }).ToList();
            return lstModel;
        }
        private class AdapterModel
        {
            public string Indicator { get; set; }
            public string Value { get; set; }
            public override bool Equals(object obj)
            {
                AdapterModel obj2 = obj as AdapterModel;
                if (obj2 == null) return false;
                return (Indicator == obj2.Indicator && Value == obj2.Value);
            }
            public override int GetHashCode()
            {
                return 1;
            }
        }
    }
}
