using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLast.Common;
using TheLast.Model;

namespace TheLast
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");
            Console.ReadLine();
            //return;
            //List<CryptonDetailModel> _lstCrypton = StaticValues.lstCoin;
            //StaticValues.basicModel = 0.LoadBasicJson();
            //StaticValues.advanceModel = 0.LoadAdvanceJson();

            //Console.WriteLine($"Date Start: {DateTime.Now}");
            //var lstOutput = new List<OutputModel>();
            //var lstTask = new List<Task>();
            //foreach (var item in _lstCrypton)
            //{
            //    var task = Task.Run(() =>
            //    {
            //        lstOutput.Add(new Calculate(item.S).GetOutput());
            //    });
            //    lstTask.Add(task);
            //}
            //Task.WaitAll(lstTask.ToArray());
            //var lstOutputResult = lstOutput.OrderByDescending(x => x.Point).ThenBy(x => x.Code);
            //var count = 0;
            //Console.WriteLine($"Date End: {DateTime.Now}");
        }
    }
}
