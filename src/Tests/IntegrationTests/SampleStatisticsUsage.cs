using System.Configuration;
using NUnit.Framework;
using System.Linq;

namespace TeamCitySharp.IntegrationTests
{
    [TestFixture]
    public class when_interacting_to_get_build_statistics
    {
        private ITeamCityClient m_client;
        private readonly string m_server;
        private readonly bool m_useSsl;
        private readonly string m_username;
        private readonly string m_password;
        private readonly string m_token;
        private readonly string m_goodProjectId;


        public when_interacting_to_get_build_statistics()
        {
            m_server = ConfigurationManager.AppSettings["Server"];
            bool.TryParse(ConfigurationManager.AppSettings["UseSsl"], out m_useSsl);
            m_username = ConfigurationManager.AppSettings["Username"];
            m_password = ConfigurationManager.AppSettings["Password"];
            m_token = ConfigurationManager.AppSettings["Token"];
            m_goodProjectId = ConfigurationManager.AppSettings["GoodProjectId"];
        }

        [SetUp]
        public void SetUp()
        {
            m_client = new TeamCityClient(m_server, m_useSsl);
            //m_client.Connect(m_username, m_password);
            m_client.ConnectWithAccessToken(m_token);
        }

        [Test]
        public void it_returns_no_of_tests_from_last_successful_build()
        {
            var proj = m_client.Projects.ById(m_goodProjectId);
            var build = m_client.Builds.LastSuccessfulBuildByBuildConfigId(proj.BuildTypes.BuildType[0].Id);
            var stats = m_client.Statistics.GetByBuildId(build.Id);

            Assert.That(stats.Property.Any(property => property.Name.Equals("PassedTestCount")));
        }
    }
}