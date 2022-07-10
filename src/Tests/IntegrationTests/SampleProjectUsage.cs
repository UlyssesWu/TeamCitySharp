﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Fields;
using TeamCitySharp.Locators;

namespace TeamCitySharp.IntegrationTests
{
    [TestFixture]
    public class when_interacting_to_get_project_details
    {
        private ITeamCityClient m_client;
        private readonly string m_server;
        private readonly bool m_useSsl;
        private readonly string m_username;
        private readonly string m_password;
        private readonly string m_token;
        private readonly string m_goodBuildConfigId;
        private readonly string m_goodProjectId;


        public when_interacting_to_get_project_details()
        {
            m_server = ConfigurationManager.AppSettings["Server"];
            bool.TryParse(ConfigurationManager.AppSettings["UseSsl"], out m_useSsl);
            m_username = ConfigurationManager.AppSettings["Username"];
            m_password = ConfigurationManager.AppSettings["Password"];
            m_token = ConfigurationManager.AppSettings["Token"];
            m_goodBuildConfigId = ConfigurationManager.AppSettings["GoodBuildConfigId"];
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
        public void it_throws_exception_when_not_passing_url()
        {
            Assert.Throws<ArgumentNullException>(() => new TeamCityClient(null));
        }

        [Test]
        public void it_throws_exception_when_host_does_not_exist()
        {
            var client = new TeamCityClient("test:81");
            client.Connect("admin", "qwerty");

            Assert.Throws<HttpRequestException>(() => client.Projects.All());
        }


        [Test]
        public void it_throws_exception_when_no_connection_formed()
        {
            var client = new TeamCityClient(m_server, m_useSsl);

            Assert.Throws<ArgumentException>(() => client.Projects.All());

            //Assert: Exception
        }

        [Test]
        public void it_returns_all_projects()
        {
            List<Project> projects = m_client.Projects.All();

            Assert.That(projects.Any(), "No projects were found for this server");
        }

        [Test]
        public void it_returns_project_details_when_passing_a_project_id()
        {
            string projectId = m_goodProjectId;
            Project projectDetails = m_client.Projects.ById(projectId);

            Assert.That(projectDetails != null, "No details found for that specific project");
        }

        [Test]
        public void it_returns_project_details_when_passing_a_project_name()
        {
            string projectName = m_goodProjectId;
            Project projectDetails = m_client.Projects.ByName(projectName);

            Assert.That(projectDetails != null, "No details found for that specific project");
        }

        [Test]
        public void it_returns_project_details_when_passing_project()
        {
            var project = new Project {Id = m_goodProjectId};
            Project projectDetails = m_client.Projects.Details(project);

            Assert.That(!string.IsNullOrWhiteSpace(projectDetails.Id));
        }


        [Test]
        [Ignore("Modify guid...")]
        public void it_returns_project_details_when_creating_project()
        {
            var client = new TeamCityClient("localhost:81");
            client.Connect("admin", "qwerty");
            var projectName = Guid.NewGuid().ToString("N");
            var project = client.Projects.Create(projectName);

            Assert.That(project, Is.Not.Null);
            Assert.That(project.Name, Is.EqualTo(projectName));
        }

        [Test]
        public void it_returns_projectFeatures_when_passing_a_project_id()
        {
            string projectId = "_Root";
            try
            {
                ProjectFeatures projectFeatures = m_client.Projects.GetProjectFeatures(projectId);
            }
            catch (HttpException e)
            {
                Assert.That(e.ResponseStatusCode == HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                Assert.Fail($"GetProjectFeatures for {projectId} faced an unexpected exception", e);
            }
        }

        [Test, Ignore("Current user doesn't have access to project features in tested instance.")]
        public void it_returns_projectFeatures_when_passing_a_project_id_and_feature_id()
        {
            string projectId = "_Root";
            string featureId = "PROJECT_EXT_1";
            ProjectFeature projectFeature = m_client.Projects.GetProjectFeatureByProjectFeature(projectId, featureId);

            Assert.That(projectFeature != null, "No project feature found for that specific project");
        }

        [Test, Ignore("User involved in test doesn't have permission.")]
        public void it_returns_projectFeatures_create_modify_delete()
        {
            string projectId = "_Root";
            ProjectFeature pf = new ProjectFeature
            {
                Id = "Test_TTT",
                Type = "ReportTab",
                Properties = new Properties
                {
                    Property = new List<Property>
                    {
                        new Property {Name = "startPage", Value = "javadoc.zip!index.html"},
                        new Property {Name = "title", Value = "javadoc.zip!index.html"},
                        new Property {Name = "type", Value = "BuildReportTab"},
                    }
                }
            };

            ProjectFeature projectFeature = m_client.Projects.CreateProjectFeature(projectId, pf);
            Assert.That(projectFeature != null, "No project features found for that specific project");

            m_client.Projects.DeleteProjectFeature(projectId, projectFeature.Id);
        }

        [Test]
        public void it_refuses_projectFeatures_create_modify_delete_when_unauthorized()
        {
            string projectId = "_Root";
            ProjectFeature pf = new ProjectFeature
            {
                Id = "Test_TTT",
                Type = "ReportTab",
                Properties = new Properties
                {
                    Property = new List<Property>
                    {
                        new Property {Name = "startPage", Value = "javadoc.zip!index.html"},
                        new Property {Name = "title", Value = "javadoc.zip!index.html"},
                        new Property {Name = "type", Value = "BuildReportTab"},
                    }
                }
            };


            try
            {
                ProjectFeature projectFeature = m_client.Projects.CreateProjectFeature(projectId, pf);
                m_client.Projects.DeleteProjectFeature(projectId, projectFeature.Id);
            }
            catch (HttpException e)
            {
                Assert.That(e.ResponseStatusCode == HttpStatusCode.Forbidden,
                    "Creating a project feature should fail with unauthorized http exception.");
            }
            catch (Exception e)
            {
                Assert.Fail("Create project feature raised an expected exception", e);
            }
        }

        [Test, Ignore("User involved in test doesn't have permission.")]
        public void it_returns_projectFeatures_field()
        {
            string projectId = "_Root";
            string featureId = "PROJECT_EXT_1";

            PropertyField propertyField = PropertyField.WithFields(name: true, value: true, inherited: true);
            PropertiesField propertiesField = PropertiesField.WithFields(propertyField: propertyField);
            ProjectFeatureField projectFeatureField =
                ProjectFeatureField.WithFields(type: true, properties: propertiesField);

            ProjectFeature projectFeature = m_client.Projects.GetFields(projectFeatureField.ToString())
                .GetProjectFeatureByProjectFeature(projectId, featureId);

            Assert.That(projectFeature != null, "No project feature found for that specific project");
            Assert.That(projectFeature.Type != null, "Bad Value type");
            Assert.That(projectFeature.Properties != null, "Bad Value type");
            Assert.That(projectFeature.Href == null, "Bad Value type");
            Assert.That(projectFeature.Id == null, "Bad Value type");
        }

        [Test]
        public void it_faces_exceptions_projectFeatures_field_when_unauthorized()
        {
            string projectId = "_Root";
            string featureId = "PROJECT_EXT_1";

            PropertyField propertyField = PropertyField.WithFields(name: true, value: true, inherited: true);
            PropertiesField propertiesField = PropertiesField.WithFields(propertyField: propertyField);
            ProjectFeatureField projectFeatureField =
                ProjectFeatureField.WithFields(type: true, properties: propertiesField);

            try
            {
                m_client.Projects.GetFields(projectFeatureField.ToString())
                    .GetProjectFeatureByProjectFeature(projectId, featureId);
            }
            catch (HttpException e)
            {
                Console.WriteLine(e);
                Assert.That(e.ResponseStatusCode == HttpStatusCode.Forbidden);
            }
            catch (Exception e)
            {
                Assert.Fail("GetFields faced an unexpected exception", e);
            }
        }

        [Test]
        public void it_returns_branches()
        {
            string projectId = m_goodProjectId;
            var tempBuild = m_client.Projects.GetBranchesByBuildProjectId(projectId);
            Assert.IsTrue(tempBuild.Count > 0);
        }

        [Test]
        public void it_returns_branches_history()
        {
            string projectId = m_goodProjectId;
            var tempBuild = m_client.Projects.GetBranchesByBuildProjectId(projectId,
                BranchLocator.WithDimensions(BranchPolicy.ALL_BRANCHES));
            Assert.IsTrue(tempBuild.Count == 6);
        }

        [Test]
        public void it_returns_branches_history_with_field_Default_but_active_not_fetched()
        {
            BranchField branchField = BranchField.WithFields(name: true, defaultValue: true);
            BranchesField branchesField = BranchesField.WithFields(branch: branchField);
            string projectId = m_goodProjectId;
            var tempBuild = m_client.Projects.GetFields(branchesField.ToString())
                .GetBranchesByBuildProjectId(projectId, BranchLocator.WithDimensions(BranchPolicy.ALL_BRANCHES));
            var checkIfFieldWork = tempBuild.Branch.Single(x => x.Default);
            Assert.IsTrue(checkIfFieldWork.Active == false);
        }

        [Test]
        public void it_returns_branches_history_with_field_Default_active_fetched()
        {
            BranchField branchField = BranchField.WithFields(name: true, defaultValue: true, active: true);
            BranchesField branchesField = BranchesField.WithFields(branch: branchField);
            string projectId = m_goodProjectId;
            var tempBuild = m_client.Projects.GetFields(branchesField.ToString())
                .GetBranchesByBuildProjectId(projectId, BranchLocator.WithDimensions(BranchPolicy.ALL_BRANCHES));
            var checkIfFieldWork = tempBuild.Branch.Single(x => x.Default);
            Assert.IsTrue(checkIfFieldWork.Active);
        }
    }
}