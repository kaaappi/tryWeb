using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using tryWeb.Model;

namespace tryWeb.Clients
{
    public class Client1
    {
        HttpClient _client1;
        
        private static string _adressNBU;
        
        public Client1()
        {
            _adressNBU = Constants.adressNBU;
            
            _client1 = new HttpClient();
            
            _client1.BaseAddress = new Uri(_adressNBU);
            
        }

        public async Task<List<CourseResponse>> Getcourse(string ValueCode, string Date)
        {
            var response = await _client1.GetAsync($"NBUStatService/v1/statdirectory/exchange?valcode={ValueCode}&date={Date}&json");
            
            response.EnsureSuccessStatusCode();



            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<List<CourseResponse>>(content);

            return result;
        }
    }

    public class Client2
    {
        HttpClient _client2;
        private static string _adressCoin;

        public Client2()
        {
            
            _adressCoin = Constants.adressCoin;

            
            _client2 = new HttpClient();

            
            _client2.BaseAddress = new Uri(_adressCoin);

        }

        public async Task<CoinModel> Getprice(string TradingPlaceName)
        {
            var response = await _client2.GetAsync($"/api/v3/coins/bitcoin/tickers?exchange_ids={TradingPlaceName}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<CoinModel>(content);

            return result;
        }
    }


}
