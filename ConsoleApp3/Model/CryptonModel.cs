using System.Collections.Generic;

namespace TheLast.Model
{
    public class CryptonModel
    {
        public List<CryptonDetailModel> Data { get; set; }
    }
    public class CryptonDetailModel
    {
        public string S { get; set; }
        public string AN { get; set; }
    }
}
