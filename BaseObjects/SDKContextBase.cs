using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;

namespace BaseObjects
{
    public class SDKContextBase
    {
        public string APIEndPoint { get; set; }
        public string Token { get; set; }

        private HttpClient Client { get; set; } = null;
        private readonly string _headerPrefix = "alt-";

        public SDKContextBase(IOptions<SDKOptionsBase> options)
        {
            APIEndPoint = options.Value.APIEndPoint;
            Token = options.Value.APIToken;
        }

        public SDKContextBase(string endPoint, string token)
        {
            APIEndPoint = endPoint;
            Token = token;
        }

        public SDKContextBase(HttpClient client, string token)
        {
            APIEndPoint = "";
            Token = token;
            Client = client;
        }


        public T GetMessage<T>(string url, string parameters = "", List<KeyValuePair<string, string>> headers = null)
        {
            HttpClient client = Client ?? new HttpClient();

            using (client)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);

                if (headers != null)
                    headers.ForEach(h => client.DefaultRequestHeaders.Add(_headerPrefix + h.Key, h.Value));


                if (!string.IsNullOrEmpty(parameters))
                {
                    url += "?" + parameters;
                }

                using (var reply = client.GetAsync(url).Result)
                {
                    reply.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<T>(reply.Content.ReadAsStringAsync().Result);
                }
            }
        }        

        public T PostMessage<T>(string url, string body, List<KeyValuePair<string, string>> headers = null)
        {
            HttpClient client = Client ?? new HttpClient();

            using (client)
            {
                //Uri uri = new Uri(url);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null)
                    headers.ForEach(h => client.DefaultRequestHeaders.Add(_headerPrefix + h.Key, h.Value));

                if (body == null)
                    body = "";

                using (var reply = client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json")).Result)
                {
                    string replyData = reply.Content.ReadAsStringAsync().Result;
                    if (reply.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var bre = JsonConvert.DeserializeObject<BadRequestResponse>(replyData);

                        if (bre != null)
                            throw new BadRequestException(bre.Message);
                        else
                            throw new BadRequestException(replyData);
                    }

                    reply.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<T>(replyData);
                }
            }
        }

               
    }
}
