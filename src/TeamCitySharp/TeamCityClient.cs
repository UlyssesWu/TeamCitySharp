using TeamCitySharp.ActionTypes;
using TeamCitySharp.Connection;

namespace TeamCitySharp
{
    public class TeamCityClient : IClientConnection, ITeamCityClient
    {
        private readonly ITeamCityCaller m_caller;
        private IBuilds m_builds;
        private IBuildQueue m_buildQueue;
        private IProjects m_projects;
        private IBuildConfigs m_buildConfigs;
        private IServerInformation m_serverInformation;
        private IUsers m_users;
        private IAgents m_agents;
        private IVcsRoots m_vcsRoots;
        private IChanges m_changes;
        private IBuildArtifacts m_artifacts;
        private IBuildInvestigations m_investigations;
        private IStatistics m_statistics;
        private ITests m_tests;

        public TeamCityClient(string hostName, bool useSsl = false)
        {
            m_caller = new TeamCityCaller(hostName, useSsl);
        }

        public void Connect(string userName, string password)
        {
            m_caller.Connect(userName, password, false);
        }

        public void ConnectWithAccessToken(string token)
        {
            m_caller.ConnectWithAccessToken(token);
        }

        public void UseVersion(string version)
        {
            m_caller.UseVersion(version);
        }

        public void EnableCache()
        {
            m_caller.EnableCache();
        }

        public void DisableCache()
        {
            m_caller.DisableCache();
        }

        public void ConnectAsGuest()
        {
            m_caller.Connect(string.Empty, string.Empty, true);
        }

        public bool Authenticate(bool throwExceptionOnHttpError = true)
        {
            return m_caller.Authenticate("", throwExceptionOnHttpError);
        }

        public IBuilds Builds => m_builds ??= new Builds(m_caller);

        public IBuildQueue BuildQueue => m_buildQueue ??= new BuildQueue(m_caller);

        public IBuildConfigs BuildConfigs => m_buildConfigs ??= new BuildConfigs(m_caller);

        public IProjects Projects => m_projects ??= new Projects(m_caller);

        public IServerInformation ServerInformation => m_serverInformation ??= new ServerInformation(m_caller);

        public IUsers Users => m_users ??= new Users(m_caller);

        public IAgents Agents => m_agents ??= new Agents(m_caller);

        public IVcsRoots VcsRoots => m_vcsRoots ??= new VcsRoots(m_caller);

        public IChanges Changes => m_changes ??= new Changes(m_caller);

        public IBuildArtifacts Artifacts => m_artifacts ??= new BuildArtifacts(m_caller);

        public IBuildInvestigations Investigations => m_investigations ??= new BuildInvestigations(m_caller);

        public IStatistics Statistics => m_statistics ??= new Statistics(m_caller);

        public ITests Tests => m_tests ??= new Tests(m_caller);
    }
}