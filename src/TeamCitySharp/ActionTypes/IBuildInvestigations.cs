using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IBuildInvestigations : IBuildInvestigationsAsync
    {
        List<Investigation> All();
        BuildInvestigations GetFields(string fields);
        List<Investigation> InvestigationsByBuildTypeId(string buildTypeId);
    }

    public interface IBuildInvestigationsAsync
    {
        Task<List<Investigation>> AllAsync();
        Task<List<Investigation>> InvestigationsByBuildTypeIdAsync(string buildTypeId);
    }
}