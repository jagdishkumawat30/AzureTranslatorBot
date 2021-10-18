using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureTranslatorBot.Utilities
{
    public class TranslatorRepository
    {
        public async Task<string> GetSupportedLanguage(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

                var response = await MakeRequestAsync(request, client);
                return PrettyJson(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception();
            }
        }
        public async Task<string> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var response = await client.SendAsync(getRequest).ConfigureAwait(false);
            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                // empty responseString
            }
            return responseString;
        }
        public string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            var jsonElement = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(unPrettyJson);
            return System.Text.Json.JsonSerializer.Serialize(jsonElement, options);
        }
    }
}