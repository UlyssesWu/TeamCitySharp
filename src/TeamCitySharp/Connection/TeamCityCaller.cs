using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;
using File = System.IO.File;

namespace TeamCitySharp.Connection
{
    internal class TeamCityCaller : ITeamCityCaller
    {
        private readonly Credentials m_credentials;
        private bool m_useNoCache;
        private string m_version = "";
        private HttpClient m_client;

        public TeamCityCaller(string hostName, bool useSsl)
        {
            if (string.IsNullOrEmpty(hostName))
                throw new ArgumentNullException(nameof(hostName));

            m_credentials = new Credentials { UseSSL = useSsl, HostName = hostName };
        }

        public void DisableCache()
        {
            m_useNoCache = true;
        }

        public void UseVersion(string version)
        {
            m_version = version;
        }

        public void EnableCache()
        {
            m_useNoCache = false;
        }

        public void Connect(string userName, string password, bool actAsGuest)
        {
            m_credentials.Password = password;
            m_credentials.UserName = userName;
            m_credentials.ActAsGuest = actAsGuest;
        }

        public void ConnectWithAccessToken(string token)
        {
            m_credentials.Token = token;
            m_credentials.UseToken = true;
            m_credentials.ActAsGuest = false;
        }

        public T GetFormat<T>(string urlPart, params object[] parts)
        {
            return Get<T>(string.Format(urlPart, parts));
        }

        public async Task<T> GetFormatAsync<T>(string urlPart, params object[] parts)
        {
            return await GetAsync<T>(string.Format(urlPart, parts));
        }

        public void GetFormat(string urlPart, params object[] parts)
        {
            Get(string.Format(urlPart, parts));
        }

        public async Task GetFormatAsync(string urlPart, params object[] parts)
        {
            await GetAsync(string.Format(urlPart, parts));
        }

        public T PostFormat<T>(object data, string contentType, string accept, string urlPart, params object[] parts)
        {
            return Post<T>(data, contentType, string.Format(urlPart, parts), accept);
        }

        public async Task<T> PostFormatAsync<T>(object data, string contentType, string accept, string urlPart,
            params object[] parts)
        {
            return await PostAsync<T>(data, contentType, string.Format(urlPart, parts), accept);
        }

        public void PostFormat(object data, string contentType, string urlPart, params object[] parts)
        {
            Post(data, contentType, string.Format(urlPart, parts), string.Empty);
        }

        public async Task PostFormatAsync(object data, string contentType, string urlPart, params object[] parts)
        {
            await PostAsync(data, contentType, string.Format(urlPart, parts), string.Empty);
        }

        public T PutFormat<T>(object data, string contentType, string accept, string urlPart, params object[] parts)
        {
            return Put<T>(data, contentType, string.Format(urlPart, parts), accept);
        }

        public async Task<T> PutFormatAsync<T>(object data, string contentType, string accept, string urlPart,
            params object[] parts)
        {
            return await PutAsync<T>(data, contentType, string.Format(urlPart, parts), accept);
        }

        public void PutFormat(object data, string contentType, string urlPart, params object[] parts)
        {
            Put(data, contentType, string.Format(urlPart, parts), string.Empty);
        }

        public async Task PutFormatAsync(object data, string contentType, string urlPart, params object[] parts)
        {
            await PutAsync(data, contentType, string.Format(urlPart, parts), string.Empty);
        }

        public void DeleteFormat(string urlPart, params object[] parts)
        {
            Delete(string.Format(urlPart, parts));
        }

        public async Task DeleteFormatAsync(string urlPart, params object[] parts)
        {
            await DeleteAsync(string.Format(urlPart, parts));
        }

        public void GetDownloadFormat(Action<string> downloadHandler, string urlPart, params object[] parts)
        {
            GetDownloadFormat(downloadHandler, urlPart, true, parts);
        }

        public void GetDownloadFormat(Action<string> downloadHandler, string urlPart, bool rest, params object[] parts)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");
            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            if (downloadHandler == null)
                throw new ArgumentException("A download handler must be specified.");

            string tempFileName = Path.GetRandomFileName();
            var url = rest ? CreateUrl(string.Format(urlPart, parts)) : CreateUrl(string.Format(urlPart, parts), false);

            try
            {
                CreateHttpClient().GetAsFile(url, tempFileName);
                downloadHandler.Invoke(tempFileName);
            }
            finally
            {
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);
            }
        }

        public async Task GetDownloadFormatAsync(Action<string> downloadHandler, string urlPart, params object[] parts)
        {
            await GetDownloadFormatAsync(downloadHandler, urlPart, true, parts);
        }

        public async Task GetDownloadFormatAsync(Action<string> downloadHandler, string urlPart, bool rest, params object[] parts)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");
            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            if (downloadHandler == null)
                throw new ArgumentException("A download handler must be specified.");

            string tempFileName = Path.GetRandomFileName();
            var url = rest ? CreateUrl(string.Format(urlPart, parts)) : CreateUrl(string.Format(urlPart, parts), false);

            try
            {
                await CreateHttpClient().GetAsFileAsync(url, tempFileName);
                downloadHandler.Invoke(tempFileName);
            }
            finally
            {
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);
            }
        }

        public string StartBackup(string urlPart)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            var url = CreateUrl(urlPart);

            var httpClient = CreateHttpClient();
            var response = httpClient.Post(url, null, HttpContentTypes.TextPlain);
            ThrowIfHttpError(response, url);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.RawText();

            return string.Empty;
        }

        public async Task<string> StartBackupAsync(string urlPart)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            var url = CreateUrl(urlPart);

            var httpClient = CreateHttpClient();
            var response = await httpClient.PostAsync(url, null, HttpContentTypes.TextPlain);
            ThrowIfHttpError(response, url);

            if (response.StatusCode == HttpStatusCode.OK)
                return await response.RawTextAsync();

            return string.Empty;
        }

        public T Get<T>(string urlPart)
        {
            var response = GetResponse(urlPart);
            return response.StaticBody<T>();
        }

        public async Task<T> GetAsync<T>(string urlPart, bool rest = true)
        {
            var response = await GetResponseAsync(urlPart, rest);
            ThrowIfHttpError(response, urlPart);
            return await response.StaticBodyAsync<T>();
        }

        public void Get(string urlPart)
        {
            GetResponse(urlPart);
        }

        public async Task GetAsync(string urlPart)
        {
            var response = await GetResponseAsync(urlPart);
            ThrowIfHttpError(response, urlPart);
        }

        private HttpResponseMessage GetResponse(string urlPart)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            var url = CreateUrl(urlPart);

            var response =
                CreateHttpClient().Get(url, HttpContentTypes.ApplicationJson);
            ThrowIfHttpError(response, url);
            return response;
        }

        private async Task<HttpResponseMessage> GetResponseAsync(string urlPart, bool rest = true)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            var url = CreateUrl(urlPart, rest);
            var httpClient = CreateHttpClient();
            return await httpClient.GetAsync(url, HttpContentTypes.ApplicationJson);
        }

        public T Post<T>(object data, string contentType, string urlPart, string accept)
        {
            return Post(data, contentType, urlPart, accept).StaticBody<T>();
        }

        public async Task<T> PostAsync<T>(object data, string contentType, string urlPart, string accept)
        {
            return await Post(data, contentType, urlPart, accept).StaticBodyAsync<T>();
        }

        public T Put<T>(object data, string contentType, string urlPart, string accept)
        {
            return Put(data, contentType, urlPart, accept).StaticBody<T>();
        }

        public async Task<T> PutAsync<T>(object data, string contentType, string urlPart, string accept)
        {
            return await Put(data, contentType, urlPart, accept).StaticBodyAsync<T>();
        }

        public bool Authenticate(string urlPart, bool throwExceptionOnHttpError = true)
        {
            try
            {
                var httpClient = CreateHttpClient();
                var response = httpClient.Get(CreateUrl(urlPart), HttpContentTypes.TextPlain);
                if (response.StatusCode != HttpStatusCode.OK && throwExceptionOnHttpError)
                {
                    throw new AuthenticationException();
                }

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (HttpException exception)
            {
                throw new AuthenticationException(exception.StatusDescription);
            }
        }

        public async Task<bool> AuthenticateAsync(string urlPart, bool throwExceptionOnHttpError = true)
        {
            try
            {
                var httpClient = CreateHttpClient();
                var response = await httpClient.GetAsync(CreateUrl(urlPart), HttpContentTypes.TextPlain);
                if (response.StatusCode != HttpStatusCode.OK && throwExceptionOnHttpError)
                {
                    throw new AuthenticationException();
                }

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (HttpException exception)
            {
                throw new AuthenticationException(exception.StatusDescription);
            }
        }

        public HttpResponseMessage Post(object data, string contentType, string urlPart, string accept)
        {
            var response = MakePostRequest(data, contentType, urlPart, accept);

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(object data, string contentType, string urlPart, string accept)
        {
            var response = await MakePostRequestAsync(data, contentType, urlPart, accept);

            return response;
        }

        public HttpResponseMessage Put(object data, string contentType, string urlPart, string accept)
        {
            var response = MakePutRequest(data, contentType, urlPart, accept);

            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(object data, string contentType, string urlPart, string accept)
        {
            var response = await MakePutRequestAsync(data, contentType, urlPart, accept);

            return response;
        }

        public void Delete(string urlPart)
        {
            MakeDeleteRequest(urlPart);
        }

        public async Task DeleteAsync(string urlPart)
        {
            await MakeDeleteRequestAsync(urlPart);
        }

        private void MakeDeleteRequest(string urlPart)
        {
            var client = CreateHttpClient();
            var url = CreateUrl(urlPart);
            var response = client.Delete(url); //TODO: HttpContentTypes.TextPlain
            ThrowIfHttpError(response, url);
        }

        private async Task<HttpResponseMessage> MakeDeleteRequestAsync(string urlPart)
        {
            var client = CreateHttpClient();
            var url = CreateUrl(urlPart);
            var response = await client.DeleteAsync(url); //TODO: HttpContentTypes.TextPlain
            ThrowIfHttpError(response, url);
            return response;
        }

        private HttpResponseMessage MakePostRequest(object data, string contentType, string urlPart, string accept)
        {
            var client = CreateHttpClient();
            var finalAccept = string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept;
            var url = CreateUrl(urlPart);
            var response = client.Post(url, data, contentType, finalAccept);
            ThrowIfHttpError(response, url);

            return response;
        }

        private async Task<HttpResponseMessage> MakePostRequestAsync(object data, string contentType, string urlPart,
            string accept)
        {
            var client = CreateHttpClient();
            var finalAccept = string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept;
            var url = CreateUrl(urlPart);
            var response = await client.PostAsync(url, data, contentType, finalAccept);
            ThrowIfHttpError(response, url);

            return response;
        }

        private HttpResponseMessage MakePutRequest(object data, string contentType, string urlPart, string accept)
        {
            var client = CreateHttpClient();
            var url = CreateUrl(urlPart);
            var finalAccept = string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept;
            var response = client.Put(url, data, contentType, finalAccept);
            ThrowIfHttpError(response, url);

            return response;
        }

        private async Task<HttpResponseMessage> MakePutRequestAsync(object data, string contentType, string urlPart,
            string accept)
        {
            var client = CreateHttpClient();
            var url = CreateUrl(urlPart);
            var finalAccept = string.IsNullOrWhiteSpace(accept) ? GetContentType(data.ToString()) : accept;
            var response = await client.PutAsync(url, data, contentType, finalAccept);
            ThrowIfHttpError(response, url);

            return response;
        }

        private static bool IsHttpError(HttpResponseMessage response)
        {
            var num = (int)response.StatusCode / 100;

            return (num == 4 || num == 5);
        }

        /// <summary>
        /// <para>If the <paramref name="response"/> is OK (see <see cref="IsHttpError"/> for definition), does nothing.</para>
        /// <para>Otherwise, throws an exception which includes also the response raw text.
        /// This would often contain a Java exception dump from the TeamCity REST Plugin, which reveals the cause of some cryptic cases otherwise showing just "Bad Request" in the HTTP error.
        /// Also this comes in handy when TeamCity goes into maintenance, and you get back the banner in HTML instead of your data.</para> 
        /// </summary>
        private static void ThrowIfHttpError(HttpResponseMessage response, string url)
        {
            if (!IsHttpError(response))
                return;
            throw new HttpException(response.StatusCode,
                $"Error: {response.ReasonPhrase}\nHTTP: {response.StatusCode}\nURL: {url}\n{response.RawText()}");
        }

        private string CreateUrl(string urlPart, bool rest = true)
        {
            var protocol = m_credentials.UseSSL ? "https://" : "http://";
            if (m_credentials.UseSSL) ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var authType = GetAuthType();
            var restUrl = rest ? "/app/rest" : "";
            var version = m_version == "" ? "" : $"/{m_version}";
            var uri = $"{protocol}{m_credentials.HostName}{authType}{restUrl}{version}{urlPart}";
            return Uri.EscapeUriString(uri).Replace("+", "%2B");
        }

        private object GetAuthType()
        {
            if (m_credentials.ActAsGuest)
                return "/guestAuth";
            return m_credentials.UseToken ? string.Empty : "/httpAuth";
        }

        private void UpdateHttpClientCredentials()
        {
            m_client ??= new HttpClient();
            var httpClient = m_client;

            if (m_useNoCache)
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

            if (m_credentials.ActAsGuest)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "");
            }
            else if (!m_credentials.UseToken)
            {
                var credentials = Encoding.ASCII.GetBytes($"{m_credentials.UserName}:{m_credentials.Password}");
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", m_credentials.Token);
            }
        }

        private HttpClient CreateHttpClient()
        {
            if (m_client != null)
            {
                return m_client;
            }

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.ApplicationJson));
            httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.ApplicationXml));
            httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(HttpContentTypes.TextPlain));

            m_client = httpClient;
            UpdateHttpClientCredentials();

            return httpClient;
        }

        // only used by the artifact listing methods since i havent found a way to deserialize them into a domain entity
        public string GetRaw(string urlPart)
        {
            return GetRaw(urlPart, true);
        }

        public string GetRaw(string urlPart, bool rest)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            var url = rest ? CreateUrl(urlPart) : CreateUrl(urlPart, false);

            var httpClient = CreateHttpClient();
            var response = httpClient.Get(url, HttpContentTypes.TextPlain);
            if (IsHttpError(response))
            {
                throw new HttpException(response.StatusCode,
                    $"Error {response.ReasonPhrase}: Thrown with URL {url}");
            }

            return response.RawText();
        }

        public async Task<string> GetRawAsync(string urlPart, bool rest = true)
        {
            if (CheckForAuthRequest())
                throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

            if (string.IsNullOrEmpty(urlPart))
                throw new ArgumentException("Url must be specified");

            var url = rest ? CreateUrl(urlPart) : CreateUrl(urlPart, false);
            var httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(url, HttpContentTypes.TextPlain);
            if (IsHttpError(response))
            {
                throw new HttpException(response.StatusCode,
                    $"Error {response.ReasonPhrase}: Thrown with URL {url}");
            }

            return await response.RawTextAsync();
        }

        private bool CheckForAuthRequest()
        {
            if (m_credentials.UseToken)
            {
                return !m_credentials.ActAsGuest &&
                       string.IsNullOrEmpty(m_credentials.Token);
            }

            return (!m_credentials.ActAsGuest &&
                    string.IsNullOrEmpty(m_credentials.UserName) &&
                    string.IsNullOrEmpty(m_credentials.Password));
        }

        private string GetContentType(string data)
        {
            if (data.StartsWith("<"))
                return HttpContentTypes.ApplicationXml;
            if (data.StartsWith("{"))
                return HttpContentTypes.ApplicationJson;
            return HttpContentTypes.TextPlain;
        }

        public bool GetBoolean(string urlPart, params object[] parts)
        {
            var urlFull = string.Format(urlPart, parts);

            try
            {
                if (CheckForAuthRequest())
                    throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

                if (string.IsNullOrEmpty(urlFull))
                    throw new ArgumentException("Url must be specified");

                var url = CreateUrl(urlFull);

                var response =
                    CreateHttpClient().Get(url, HttpContentTypes.ApplicationJson);
                return !IsHttpError(response);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> GetBooleanAsync(string urlPart, params object[] parts)
        {
            var urlFull = string.Format(urlPart, parts);

            try
            {
                if (CheckForAuthRequest())
                    throw new ArgumentException("If you are not acting as a guest you must supply userName and password");

                if (string.IsNullOrEmpty(urlFull))
                    throw new ArgumentException("Url must be specified");

                var url = CreateUrl(urlFull);
                var response = await CreateHttpClient().GetAsync(url, HttpContentTypes.ApplicationJson);
                return !IsHttpError(response);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public T GetNextHref<T>(string nextHref)
        {
            var reg = new System.Text.RegularExpressions.Regex(@"\/(guestAuth|httpAuth)(\/app\/rest)?(\/\d+.\d+)?");
            string urlPart = nextHref.Substring(reg.Match(nextHref).Value.Length);
            return Get<T>(urlPart);
        }

        public async Task<T> GetNextHrefAsync<T>(string nextHref)
        {
            var reg = new System.Text.RegularExpressions.Regex(@"\/(guestAuth|httpAuth)(\/app\/rest)?(\/\d+.\d+)?");
            string urlPart = nextHref.Substring(reg.Match(nextHref).Value.Length);
            return await GetAsync<T>(urlPart);
        }
    }
}