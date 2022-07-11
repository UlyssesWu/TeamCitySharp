using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IAgents : IAgentsAsync
    {
        List<Agent> All(bool includeDisconnected = false, bool includeUnauthorized = false);
        Agents GetFields(string fields);
    }

    public interface IAgentsAsync
    {
        Task<List<Agent>> AllAsync(bool includeDisconnected = false, bool includeUnauthorized = false);
    }

}