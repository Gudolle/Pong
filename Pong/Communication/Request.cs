using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pong.Model;

namespace Pong.Communication
{
    public class Request
    {
        private static int Key = 262166;
        private static string BaseUri = "http://syllab.com/PTRE839/";
        public static int GetPing()
        {
            Ping Delay = new Ping();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    long t0 = GetUnixNow();
                    client.BaseAddress = new Uri(String.Format("{0}/pings?k={1}&t0={2}", BaseUri, Key, t0));
                    HttpResponseMessage reponse = client.GetAsync(client.BaseAddress).Result;
                    Delay = JsonConvert.DeserializeObject<Ping>(reponse.Content.ReadAsStringAsync().Result);
                    Delay.t3 = GetUnixNow();
                }
                catch(Exception ex) { }

            }

            return Delay.RetourneLatence();
        }
        private static long GetUnixNow()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }
    }
}
