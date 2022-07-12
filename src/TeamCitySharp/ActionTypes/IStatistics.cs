using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IStatistics : IStatisticsAsync
    {
        Statistics GetFields(string fields);
        Properties GetByBuildId(string buildId);
    }

    public interface IStatisticsAsync
    {
        Task<Properties> GetByBuildIdAsync(string buildId);
    }
}