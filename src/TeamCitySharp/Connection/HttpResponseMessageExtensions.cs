using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeamCitySharp.Connection
{
    public static class HttpResponseMessageExtensions
    {
        public static string RawText(this HttpResponseMessage src)
        {
            return src.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public static Task<string> RawTextAsync(this HttpResponseMessage src)
        {
            return src.Content.ReadAsStringAsync();
        }

        public static T StaticBody<T>(this HttpResponseMessage src)
        {
            var stringContent = src.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return FromJson<T>(stringContent);
        }

        public static async Task<T> StaticBodyAsync<T>(this HttpResponseMessage src)
        {
            var stringContent = await src.Content.ReadAsStringAsync();
            return FromJson<T>(stringContent);
        }

        private static JsonSerializerSettings _jsonSettings;

        public static T FromJson<T>(this string src)
        {
            _jsonSettings ??= new JsonSerializerSettings
            {
                DateFormatString = "yyyyMMdd'T'HHmmssK",
                Culture = CultureInfo.CurrentCulture
            };

            try
            {
                return JsonConvert.DeserializeObject<T>(src, _jsonSettings);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e);
                Console.WriteLine(src);
                throw;
            }
        }
    }
}