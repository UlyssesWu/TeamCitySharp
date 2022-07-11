using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
  public interface IChanges : IChangesAsync
  {
    List<Change> All();
    Change ByChangeId(string id);
    Change LastChangeDetailByBuildConfigId(string buildConfigId);
    List<Change> ByBuildConfigId(string buildConfigId);
  }

  public interface IChangesAsync
  {
        Task<List<Change>> AllAsync();
        Task<Change> ByChangeIdAsync(string id);
        Task<Change> LastChangeDetailByBuildConfigIdAsync(string buildConfigId);
        Task<List<Change>> ByBuildConfigIdAsync(string buildConfigId);
    }
  
}