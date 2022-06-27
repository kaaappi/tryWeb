using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tryWeb.Model
{
    public class CoinModel
    {
        public List<Price> tickers { get; set; }
    }
    public class Price
    {
        public string target { get; set; }
        public double last { get; set; }
    }
}
