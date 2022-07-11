using System.Collections.Generic;
using System.Text;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public class ServerInformation : IServerInformation
    {
        private const string ServerUrlPrefix = "/server";
        private readonly ITeamCityCaller m_caller;

        public ServerInformation(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public Server ServerInfo()
        {
            var server = m_caller.Get<Server>(ServerUrlPrefix);
            return server;
        }

        public async Task<Server> ServerInfoAsync()
        {
            var server = await m_caller.GetAsync<Server>(ServerUrlPrefix);
            return server;
        }

        public List<Plugin> AllPlugins()
        {
            var pluginWrapper = m_caller.Get<PluginWrapper>(ServerUrlPrefix + "/plugins");

            return pluginWrapper.Plugin;
        }

        public async Task<List<Plugin>> AllPluginsAsync()
        {
            var pluginWrapper = await m_caller.GetAsync<PluginWrapper>(ServerUrlPrefix + "/plugins");

            return pluginWrapper.Plugin;
        }

        public string TriggerServerInstanceBackup(BackupOptions backupOptions)
        {
            var backupOptionsUrlPart = BuildBackupOptionsUrl(backupOptions);
            var url = string.Concat(ServerUrlPrefix, "/backup?", backupOptionsUrlPart);

            return m_caller.StartBackup(url);
        }

        public async Task<string> TriggerServerInstanceBackupAsync(BackupOptions backupOptions)
        {
            var backupOptionsUrlPart = BuildBackupOptionsUrl(backupOptions);
            var url = string.Concat(ServerUrlPrefix, "/backup?", backupOptionsUrlPart);

            return await m_caller.StartBackupAsync(url);
        }

        public string GetBackupStatus()
        {
            return m_caller.GetRaw(string.Concat(ServerUrlPrefix, "/backup"));
        }

        public async Task<string> GetBackupStatusAsync()
        {
            return await m_caller.GetRawAsync(string.Concat(ServerUrlPrefix, "/backup"));
        }

        private string BuildBackupOptionsUrl(BackupOptions backupOptions)
        {
            return new StringBuilder()
                .Append("fileName=").Append(backupOptions.FileName)
                .Append("&includeBuildLogs=").Append(backupOptions.IncludeBuildLogs)
                .Append("&includeConfigs=").Append(backupOptions.IncludeConfigurations)
                .Append("&includeDatabase=").Append(backupOptions.IncludeDatabase)
                .Append("&includePersonalChanges=").Append(backupOptions.IncludePersonalChanges)
                .ToString();
        }
    }
}