using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TeamCitySharp.Connection
{
    public static class HttpClientExtensions
    {
        public static HttpResponseMessage Get(this HttpClient src, string url, string accept = "")
        {
            //TODO: quick fix, need to fix soon for a big transaction
            //src.Timeout=TimeSpan.FromHours(1);
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (!string.IsNullOrEmpty(accept))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            }
            return src.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
        }

        /// <summary>
        /// GetAsync with ContentType
        /// </summary>
        /// <param name="src"></param>
        /// <param name="url"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient src, string url, string accept)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (!string.IsNullOrEmpty(accept))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            }
            return await src.SendAsync(request);
        }

        public static HttpResponseMessage Post(this HttpClient src, string url, object body, string contentType, string accept = "")
        {
            StringContent content = null;
            if (body != null)
            {
                var data = contentType == HttpContentTypes.ApplicationJson ? JsonConvert.SerializeObject(body) : body.ToString();

                content = new StringContent(data, Encoding.UTF8, contentType);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            request.Headers.Accept.Add(!string.IsNullOrEmpty(accept)
                ? new MediaTypeWithQualityHeaderValue(accept)
                : new MediaTypeWithQualityHeaderValue(contentType));

            return src.SendAsync(request).GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> PostAsync(this HttpClient src, string url, object body, string contentType, string accept = "")
        {
            StringContent content = null;
            if (body != null)
            {
                var data = contentType == HttpContentTypes.ApplicationJson ? JsonConvert.SerializeObject(body) : body.ToString();

                content = new StringContent(data, Encoding.UTF8, contentType);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            request.Headers.Accept.Add(!string.IsNullOrEmpty(accept)
                ? new MediaTypeWithQualityHeaderValue(accept)
                : new MediaTypeWithQualityHeaderValue(contentType));

            return await src.SendAsync(request);
        }

        public static HttpResponseMessage Put(this HttpClient src, string url, object body, string contentType, string accept = "")
        {
            StringContent content = null;
            if (body != null)
            {
                var data = contentType == HttpContentTypes.ApplicationJson ? JsonConvert.SerializeObject(body) : body.ToString();

                content = new StringContent(data, Encoding.UTF8, contentType);
            }

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = content;
            request.Headers.Accept.Add(!string.IsNullOrEmpty(accept)
                ? new MediaTypeWithQualityHeaderValue(accept)
                : new MediaTypeWithQualityHeaderValue(contentType));

            return src.SendAsync(request).GetAwaiter().GetResult();
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient src, string url, object body, string contentType, string accept = "")
        {
            StringContent content = null;
            if (body != null)
            {
                var data = contentType == HttpContentTypes.ApplicationJson ? JsonConvert.SerializeObject(body) : body.ToString();

                content = new StringContent(data, Encoding.UTF8, contentType);
            }

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = content;
            request.Headers.Accept.Add(!string.IsNullOrEmpty(accept)
                ? new MediaTypeWithQualityHeaderValue(accept)
                : new MediaTypeWithQualityHeaderValue(contentType));

            return await src.SendAsync(request);
        }

        public static HttpResponseMessage Delete(this HttpClient src, string url)
        {
            return src.DeleteAsync(url).GetAwaiter().GetResult();
        }

        public static HttpResponseMessage GetAsFile(this HttpClient src, string url, string tempFilename)
        {
            using (var response = src.Get(url, HttpContentTypes.ApplicationJson)) //TODO: am I right?
            {
                response.EnsureSuccessStatusCode();

                using (Stream contentStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult(),
                       fileStream = new FileStream(tempFilename, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var buffer = new byte[8192];
                    var isMoreToRead = true;

                    do
                    {
                        var read = contentStream.ReadAsync(buffer, 0, buffer.Length).GetAwaiter().GetResult();
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            fileStream.WriteAsync(buffer, 0, read).GetAwaiter().GetResult();
                        }
                    } while (isMoreToRead);

                    return response;
                }
            }
        }

        public static async Task<HttpResponseMessage> GetAsFileAsync(this HttpClient src, string url, string tempFilename)
        {
            using var response = await src.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            using Stream contentStream = await response.Content.ReadAsStreamAsync(),
                fileStream = new FileStream(tempFilename, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            await contentStream.CopyToAsync(fileStream);

            return response;
        }
    }
}