using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Windows_OnOff
{
    internal class domain_processing
    {
        data_processing dp;
        public domain_processing()
        {
            dp = new data_processing();
        }

        private static readonly string duckdns_token = "your bot Token";
        private static readonly string duckdns_domin = "your id Chat";
        private static string duckdns_url = $"https://www.duckdns.org/update?domains={duckdns_domin}&token={duckdns_token}&ip=";

        public async void IpDuckdns_update()
        {
            while (true)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        dp._logWrite("Send request to Duck DNS...");
                        HttpResponseMessage response = await client.GetAsync(duckdns_url);
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            dp._logWrite($"Duck DNS Answer: {result}");
                        }
                        else
                        {
                            dp._logWrite($"Error:{response.StatusCode}");
                        }

                        dp._logWrite(Translator.get_translate("duckdn_updaed", static_variables.language));
                    }
                    catch (Exception e)
                    {
                        dp._logWrite($"An error has occurred: {e.Message}");
                    }
                }
                await Task.Delay(TimeSpan.FromHours(1));
            }
        }
    }
}
