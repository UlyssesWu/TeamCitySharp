using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TeamCitySharp.Connection
{
    public interface ITeamCityCaller
#if ENABLE_ASYNC
        : ITeamCityCallerAsync
#endif
    {
        void Connect(string userName, string password, bool actAsGuest);

        void ConnectWithAccessToken(string token);

        T GetFormat<T>(string urlPart, params object[] parts);

        void GetFormat(string urlPart, params object[] parts);

        T PostFormat<T>(object data, string contentType, string accept, string urlPart, params object[] parts);

        void PostFormat(object data, string contentType, string urlPart, params object[] parts);

        T PutFormat<T>(object data, string contentType, string accept, string urlPart, params object[] parts);

        void PutFormat(object data, string contentType, string urlPart, params object[] parts);

        void DeleteFormat(string urlPart, params object[] parts);

        void GetDownloadFormat(Action<string> downloadHandler, string urlPart, params object[] parts);

        void GetDownloadFormat(Action<string> downloadHandler, string urlPart, bool rest, params object[] parts);

        string StartBackup(string urlPart);

        T Get<T>(string urlPart);

        void Get(string urlPart);

        T Post<T>(object data, string contentType, string urlPart, string accept);

        T Put<T>(object data, string contentType, string urlPart, string accept);

        bool Authenticate(string urlPart, bool throwExceptionOnHttpError = true);

        HttpResponseMessage Post(object data, string contentType, string urlPart, string accept);

        HttpResponseMessage Put(object data, string contentType, string urlPart, string accept);

        void Delete(string urlPart);

        string GetRaw(string urlPart);

        string GetRaw(string urlPart, bool rest);

        bool GetBoolean(string urlPart, params object[] parts);
        T GetNextHref<T>(string nextHref);

        void DisableCache();

        void EnableCache();

        void UseVersion(string version);
    }

    public interface ITeamCityCallerAsync
    {
        Task<T> GetFormatAsync<T>(string urlPart, params object[] parts);
        Task<T> PostFormatAsync<T>(object data, string contentType, string accept, string urlPart, params object[] parts);
        Task<T> PutFormatAsync<T>(object data, string contentType, string accept, string urlPart, params object[] parts);
        Task PutFormatAsync(object data, string contentType, string urlPart, params object[] parts);
        Task DeleteFormatAsync(string urlPart, params object[] parts);
        Task GetDownloadFormatAsync(Action<string> downloadHandler, string urlPart, params object[] parts);
        Task GetDownloadFormatAsync(Action<string> downloadHandler, string urlPart, bool rest, params object[] parts);
        Task<T> GetAsync<T>(string urlPart);
        Task<T> PostAsync<T>(object data, string contentType, string urlPart, string accept);
        Task<HttpResponseMessage> PostAsync(object data, string contentType, string urlPart, string accept);
        Task<T> PutAsync<T>(object data, string contentType, string urlPart, string accept);
        Task<HttpResponseMessage> PutAsync(object data, string contentType, string urlPart, string accept);
        Task DeleteAsync(string urlPart);
        Task<string> GetRawAsync(string urlPart, bool rest = true);
        Task<bool> GetBooleanAsync(string urlPart, params object[] parts);
        Task<T> GetNextHrefAsync<T>(string nextHref);
        Task<bool> AuthenticateAsync(string urlPart, bool throwExceptionOnHttpError = true);

    }
}