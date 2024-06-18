using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp6
{
    public class Nbrb 
    {
        private readonly HttpClient _httpClient;

        public Nbrb(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExchangeRate>> GetExchangeRateAsync(string curId)
        {
            var response = await _httpClient.GetAsync("/exrates/rates?periodicity=0");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                Console.WriteLine($"Response: {responseContent}");
                throw new HttpRequestException($"Error fetching exchange rates: {response.StatusCode}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var exchangeRates = JsonSerializer.Deserialize<List<ExchangeRate>>(responseContent, options);

            return exchangeRates ?? new List<ExchangeRate>(); 
        }
    }

    public class ExchangeRate
    {
        [Key]
        public int Cur_ID { get; set; }
        public DateTime Date { get; set; }
        public string Cur_Abbreviation { get; set; } = string.Empty; 
        public int Cur_Scale { get; set; }
        public string Cur_Name { get; set; } = string.Empty; 
        public decimal? Cur_OfficialRate { get; set; }
    }

    public class RateShort
    {
        public int Cur_ID { get; set; }
        [Key]
        public DateTime Date { get; set; }
        public decimal? Cur_OfficialRate { get; set; }
    }

    public class Currency
    {
        [Key]
        public int Cur_ID { get; set; }
        public int? Cur_ParentID { get; set; }
        public string Cur_Code { get; set; } = string.Empty; 
        public string Cur_Abbreviation { get; set; } = string.Empty; 
        public string Cur_Name { get; set; } = string.Empty; 
        public string Cur_Name_Bel { get; set; } = string.Empty; 
        public string Cur_Name_Eng { get; set; } = string.Empty; 
        public string Cur_QuotName { get; set; } = string.Empty; 
        public string Cur_QuotName_Bel { get; set; } = string.Empty; 
        public string Cur_QuotName_Eng { get; set; } = string.Empty;
        public string Cur_NameMulti { get; set; } = string.Empty; 
        public string Cur_Name_BelMulti { get; set; } = string.Empty; 
        public string Cur_Name_EngMulti { get; set; } = string.Empty; 
        public int Cur_Scale { get; set; }
        public int Cur_Periodicity { get; set; }
        public DateTime Cur_DateStart { get; set; }
        public DateTime Cur_DateEnd { get; set; }
    }

    public class ResultEntry
    {
        public string Result { get; set; } = string.Empty; 
        public string Currency { get; set; } = string.Empty; 
    }
}
