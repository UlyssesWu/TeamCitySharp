using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IServerInformation
    {
        Server ServerInfo();
        List<Plugin> AllPlugins();
        string TriggerServerInstanceBackup(BackupOptions backupOptions);
        string GetBackupStatus();
    }

    public interface IServerInformationAsync
    {
        Task<Server> ServerInfoAsync();
        Task<List<Plugin>> AllPluginsAsync();
        Task<string> TriggerServerInstanceBackupAsync(BackupOptions backupOptions);
        Task<string> GetBackupStatusAsync();
    }
}