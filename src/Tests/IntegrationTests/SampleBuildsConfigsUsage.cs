using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Fields;
using TeamCitySharp.Locators;

namespace TeamCitySharp.IntegrationTests
{
  [TestFixture]
  public class when_interations_to_get_build_configuration_details
  {
    private ITeamCityClient m_client;
    private readonly string m_server;
    private readonly bool m_useSsl;
    private readonly string m_username;
    private readonly string m_password;
    private readonly string m_token;
    private readonly string m_goodBuildConfigId;
    private readonly string m_goodProjectId;
    private readonly string m_goodTemplateId;

    public when_interations_to_get_build_configuration_details()
    {
      m_server = ConfigurationManager.AppSettings["Server"];
      bool.TryParse(ConfigurationManager.AppSettings["UseSsl"], out m_useSsl);
      m_username = ConfigurationManager.AppSettings["Username"];
      m_password = ConfigurationManager.AppSettings["Password"];
      m_token = ConfigurationManager.AppSettings["Token"];
      m_goodBuildConfigId = ConfigurationManager.AppSettings["GoodBuildConfigId"];
      m_goodProjectId = ConfigurationManager.AppSettings["GoodProjectId"];
      m_goodTemplateId = ConfigurationManager.AppSettings["GoodTemplateId"];
    }

    [SetUp]
    public void SetUp()
    {
      m_client = new TeamCityClient(m_server, m_useSsl);
      //m_client.Connect(m_username, m_password);
      m_client.ConnectWithAccessToken(m_token);
    }

    [Test]
    public void it_throws_exception_when_no_url_passed()
    {
      Assert.Throws<ArgumentNullException>(() => new TeamCityClient(null));
    }

    [Test]
    public void it_throws_exception_when_host_does_not_exist()
    {
      var client = new TeamCityClient("test:81");
      client.Connect("teamcitysharpuser", "qwerty");

      Assert.Throws<HttpRequestException>(() => client.BuildConfigs.All());
    }
    [Test, Ignore("We need to configure token before run this test")]
    public void it_returns_all_build_types_with_access_token()
    {
      var client = new TeamCityClient(m_server, m_useSsl);
      client.ConnectWithAccessToken(m_token);
      var buildConfigs = client.BuildConfigs.All();
      Assert.That(buildConfigs.Any(), "No build types were found in this server");
    }

    [Test]
    public void it_throws_exception_when_no_connection_formed()
    {
      var client = new TeamCityClient(m_server, m_useSsl);

      Assert.Throws<ArgumentException>(() => client.BuildConfigs.All());

      //Assert: Exception
    }

    [Test]
    public void it_returns_all_build_types()
    {
      var buildConfigs = m_client.BuildConfigs.All();

      Assert.That(buildConfigs.Any(), "No build types were found in this server");
    }

    [Test]
    public void it_returns_build_config_details_by_configuration_id()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildConfig = m_client.BuildConfigs.ByConfigurationId(buildConfigId);

      Assert.That(buildConfig != null, "Cannot find a build type for that buildId");
    }

    [Test, Ignore("Test user doesn't have the rights to change pause status for build configs.")]
    public void it_pauses_configuration()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      m_client.BuildConfigs.SetConfigurationPauseStatus(buildLocator, true);
      var status = m_client.BuildConfigs.GetConfigurationPauseStatus(buildLocator);
      Assert.That(status == true, "Build not paused");
    }

    [Test]
    public void it_throws_exception_pauses_configuration_forbidden()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      try
      {
        var client = new TeamCityClient(m_server, m_useSsl);
        client.ConnectAsGuest();
        client.BuildConfigs.SetConfigurationPauseStatus(buildLocator, true);
      }
      catch (HttpException e)
      {
        Assert.That(e.ResponseStatusCode == HttpStatusCode.Forbidden);
      }
      catch (Exception e)
      {
        Assert.Fail($"Set configurationPauseStatus faced an unexpected exception", e);
      }
    }

    [Test, Ignore("Test user doesn't have the rights to change pause status for build configs.")]
    public void it_unpauses_configuration()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      m_client.BuildConfigs.SetConfigurationPauseStatus(buildLocator, false);
      var status = m_client.BuildConfigs.GetConfigurationPauseStatus(buildLocator);
      Assert.That(status == false, "Build not unpaused");

    }

    [Test]
    public void it_returns_build_config_details_by_configuration_name()
    {
      string buildConfigName = "Release Build";
      var buildConfig = m_client.BuildConfigs.ByConfigurationName(buildConfigName);

      Assert.That(buildConfig != null, "Cannot find a build type for that buildName");
    }

    [Test]
    public void it_returns_build_configs_by_project_id()
    {
      string projectId = m_goodProjectId;
      var buildConfigs = m_client.BuildConfigs.ByProjectId(projectId);

      Assert.That(buildConfigs.Any(), "Cannot find a build type for that projectId");
    }

    [Test]
    public void it_returns_build_configs_by_project_name()
    {
      string projectName = m_goodProjectId;
      var buildConfigs = m_client.BuildConfigs.ByProjectName(projectName);

      Assert.That(buildConfigs.Any(), "Cannot find a build type for that projectName");
    }

    [Test, Ignore("Test user doesn't have the rights to access artifact dependencies of build config.")]
    public void it_returns_artifact_dependencies_by_build_config_id()
    {
      string buildConfigId = m_goodBuildConfigId;
      var artifactDependencies = m_client.BuildConfigs.GetArtifactDependencies(buildConfigId);

      Assert.That(artifactDependencies != null, "Cannot find a Artifact dependencies for that buildConfigId");
    }


    [Test, Ignore("Test user doesn't have the rights to access artifact dependencies of build config.")]
    public void it_returns_snapshot_dependencies_by_build_config_id()
    {
      string buildConfigId = m_goodBuildConfigId;
      var snapshotDependencies = m_client.BuildConfigs.GetSnapshotDependencies(buildConfigId);
      Assert.That(snapshotDependencies != null, "Cannot find a snapshot dependencies for that buildConfigId");
    }

    [Test , Ignore("Test user doesn't have the rights to access artifact dependencies of build config.")]
    public void it_create_build_config_step()
    {
      var bt = new BuildConfig();
      try
      {
        bt = m_client.BuildConfigs.CreateConfigurationByProjectId(m_goodProjectId,
          "testNewConfig");


        var xml = "<step type=\"simpleRunner\">" +
                  "<properties>" +
                  "<property name=\"script.content\" value=\"@echo off&#xA;echo Step1&#xA;touch step1.txt\" />" +
                  "<property name=\"teamcity.step.mode\" value=\"default\" />" +
                  "<property name=\"use.custom.script\" value=\"true\" />" +
                  "</properties>" +
                  "</step>";
        m_client.BuildConfigs.PostRawBuildStep(BuildTypeLocator.WithId(bt.Id), xml);
        var newBt = m_client.BuildConfigs.ByConfigurationId(bt.Id);
        var currentStepBuild = newBt.Steps.Step[0];
        Assert.That(currentStepBuild.Type == "simpleRunner" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "script.content").Value ==
                    "@echo off\necho Step1\ntouch step1.txt" && 
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "teamcity.step.mode").Value ==
                    "default" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "use.custom.script").Value ==
                    "true");
      }
      catch (Exception e)
      {
        Assert.Fail($"{e.Message}", e);
      }
      finally
      {
        m_client.BuildConfigs.DeleteConfiguration(BuildTypeLocator.WithId(bt.Id));
      }
    }

    [Test, Ignore("Test user doesn't have the rights to access artifact dependencies of build config.")]
    public void it_create_build_config_steps()
    {
      var bt = new BuildConfig();
      try
      {
        bt = m_client.BuildConfigs.CreateConfigurationByProjectId(m_goodProjectId,
          "testNewConfig");


        const string xml = @"<steps>
                        <step name=""Test1"" type=""simpleRunner"">
                        <properties>
                          <property name=""script.content"" value=""@echo off&#xA;echo Step1&#xA;touch step1.txt"" />
                          <property name=""teamcity.step.mode"" value=""default"" />
                          <property name=""use.custom.script"" value=""true"" />
                        </properties>
                    </step>
                    <step name=""Test2"" type=""simpleRunner"">
                        <properties>
                          <property name=""script.content"" value=""@echo off&#xA;echo Step1&#xA;touch step2.txt"" />
                          <property name=""teamcity.step.mode"" value=""default"" />
                          <property name=""use.custom.script"" value=""true"" />
                        </properties>
                    </step>
                   </steps>";
        m_client.BuildConfigs.PutRawBuildStep(BuildTypeLocator.WithId(bt.Id), xml);
        var newBt = m_client.BuildConfigs.ByConfigurationId(bt.Id);
        var currentStepBuild = newBt.Steps.Step[0];
        Assert.That(currentStepBuild.Type == "simpleRunner" && currentStepBuild.Name=="Test1" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "script.content").Value ==
                    "@echo off\necho Step1\ntouch step1.txt" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "teamcity.step.mode").Value ==
                    "default" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "use.custom.script").Value ==
                    "true");
        currentStepBuild = newBt.Steps.Step[1];
        Assert.That(currentStepBuild.Type == "simpleRunner" && currentStepBuild.Name == "Test2" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "script.content").Value ==
                    "@echo off\necho Step1\ntouch step2.txt" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "teamcity.step.mode").Value ==
                    "default" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "use.custom.script").Value ==
                    "true");
      }
      catch (Exception e)
      {
        Assert.Fail($"{e.Message}", e);
      }
      finally
      {
        m_client.BuildConfigs.DeleteConfiguration(BuildTypeLocator.WithId(bt.Id));
      }
    }

    [Test, Ignore("Test user doesn't have the rights to access artifact dependencies of build config.")]
    public void it_getraw_build_config_steps()
    {
      var bt = new BuildConfig();
      try
      {
        bt = m_client.BuildConfigs.CreateConfigurationByProjectId(m_goodProjectId,
          "testNewConfig");


        const string xml = @"<steps>
                        <step name=""Test1"" type=""simpleRunner"">
                        <properties>
                          <property name=""script.content"" value=""@echo off&#xA;echo Step1&#xA;touch step1.txt"" />
                          <property name=""teamcity.step.mode"" value=""default"" />
                          <property name=""use.custom.script"" value=""true"" />
                        </properties>
                    </step>
                    <step name=""Test2"" type=""simpleRunner"">
                        <properties>
                          <property name=""script.content"" value=""@echo off&#xA;echo Step1&#xA;touch step2.txt"" />
                          <property name=""teamcity.step.mode"" value=""default"" />
                          <property name=""use.custom.script"" value=""true"" />
                        </properties>
                    </step>
                   </steps>";
        m_client.BuildConfigs.PutRawBuildStep(BuildTypeLocator.WithId(bt.Id), xml);
        var newSteps = m_client.BuildConfigs.GetRawBuildStep(BuildTypeLocator.WithId(bt.Id));
        var currentStepBuild = newSteps.Step[0];
        Assert.That(currentStepBuild.Type == "simpleRunner" && currentStepBuild.Name == "Test1" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "script.content").Value ==
                    "@echo off\necho Step1\ntouch step1.txt" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "teamcity.step.mode").Value ==
                    "default" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "use.custom.script").Value ==
                    "true");
        currentStepBuild = newSteps.Step[1];
        Assert.That(currentStepBuild.Type == "simpleRunner" && currentStepBuild.Name == "Test2" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "script.content").Value ==
                    "@echo off\necho Step1\ntouch step2.txt" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "teamcity.step.mode").Value ==
                    "default" &&
                    currentStepBuild.Properties.Property.FirstOrDefault(x => x.Name == "use.custom.script").Value ==
                    "true");
      }
      catch (Exception e)
      {
        Assert.Fail($"{e.Message}", e);
      }
      finally
      {
        m_client.BuildConfigs.DeleteConfiguration(BuildTypeLocator.WithId(bt.Id));
      }
    }


    [Test]
    public void it_throws_exception_artifact_dependencies_by_build_config_id_forbidden()
    {

      try
      {
        var client = new TeamCityClient(m_server, m_useSsl);
        client.ConnectAsGuest();
        client.BuildConfigs.GetArtifactDependencies(m_goodBuildConfigId);
      }
      catch (HttpException e)
      {
        Assert.That(e.ResponseStatusCode == HttpStatusCode.Forbidden);
      }
      catch (Exception e)
      {
        Assert.Fail($"GetArtifactDependencies faced an unexpected exception for {m_goodBuildConfigId}", e);
      }
    }


    [Test]
    public void it_throws_exception_snapshot_dependencies_by_build_config_id_forbidden()
    {
      try
      {
        var client = new TeamCityClient(m_server, m_useSsl);
        client.ConnectAsGuest();
        client.BuildConfigs.GetSnapshotDependencies(m_goodBuildConfigId);
      }
      catch (HttpException e)
      {
        Assert.That(e.ResponseStatusCode == HttpStatusCode.Forbidden);
      }
      catch (Exception e)
      {
        Assert.Fail($"GetSnapshotDependencies faced an unexpected exception for {m_goodBuildConfigId}", e);
      }
    }

    [Test]
    public void it_throws_exception_create_build_config_forbidden()
    {

      try
      {
        var client = new TeamCityClient(m_server, m_useSsl);
        client.ConnectAsGuest();
        client.BuildConfigs.CreateConfigurationByProjectId(m_goodProjectId, "testNewConfig");
      }
      catch (HttpException e)
      {
        Assert.That(e.ResponseStatusCode == HttpStatusCode.Forbidden);
      }
      catch (Exception e)
      {
        Assert.Fail($"PostRawBuildStep faced an unexpected exception for {m_goodBuildConfigId}", e);
      }
    }

    [Test, Ignore("Test user doesn't have the rights to access artifact dependencies of build config.")]
    public void it_modify_build_config()
    {
      const string depend = "TeamcityDashboardScenario_Test_TestWithCheckout";
      const string newDepend = "TeamcityDashboardScenario_Test_TestWithCheckoutWithDependencies";
      try
      {
        var buildConfig = m_client.BuildConfigs.CreateConfigurationByProjectId(m_goodProjectId, "testNewConfig");
        var buildLocator = BuildTypeLocator.WithId(buildConfig.Id);
        var bt = new BuildTrigger
        {
          Id = "ttt1", Type = "buildDependencyTrigger", Properties = new Properties
          {
            Property = new List<Property>
            {
              new Property {Name = "afterSuccessfulBuildOnly", Value = "true"},
              new Property {Name = "dependsOn", Value = depend}
            }
          }
        };

        // Configure starting trigger
        m_client.BuildConfigs.SetTrigger(buildLocator, bt);

        var actualFirst = m_client.BuildConfigs.ByConfigurationId(buildConfig.Id);
        Assert.That(actualFirst.Triggers.Trigger[0].Type == "buildDependencyTrigger" &&
                    actualFirst.Triggers.Trigger[0].Properties.Property.FirstOrDefault(x => x.Name == "dependsOn")
                      .Value == depend);

        // Modify trigger
        m_client.BuildConfigs.ModifyTrigger(buildConfig.Id, depend, newDepend);
        var actualTwo = m_client.BuildConfigs.ByConfigurationId(buildConfig.Id);
        Assert.That(actualTwo.Triggers.Trigger[0].Type == "buildDependencyTrigger" &&
                    actualTwo.Triggers.Trigger[0].Properties.Property.FirstOrDefault(x => x.Name == "dependsOn")
                      .Value == newDepend);
        var buildLocatorFinal = BuildTypeLocator.WithId(buildConfig.Id);

        //Cleanup 
        m_client.BuildConfigs.DeleteConfiguration(buildLocatorFinal);
      }
      catch (Exception e)
      {
        Assert.Fail($"{e.Message}", e);
      }
    }

    [Test]
    public void it_modify_artifact_dependencies()
    {
      const string depend = "TeamcityDashboardScenario_Test_TestWithCheckout";
      const string newDepend = "TeamcityDashboardScenario_Test_TestWithCheckoutWithDependencies";
      var buildLocatorFinal = new BuildTypeLocator();
      try
      {
        var buildConfig = m_client.BuildConfigs.CreateConfigurationByProjectId(m_goodProjectId, "testNewConfig");
        buildLocatorFinal = BuildTypeLocator.WithId(buildConfig.Id);
        var artifactDependencies = new ArtifactDependencies
        {
          ArtifactDependency = new List<ArtifactDependency>
          {
            new ArtifactDependency
            {
              Id = "TTTT_100",
              Type = "artifact_dependency",
              SourceBuildType = new BuildConfig{Id = depend},
              Properties = new Properties
              {
                Property = new List<Property>
                {
                  new Property {Name = "cleanDestinationDirectory", Value = "false"},
                  new Property {Name = "pathRules", Value = "step1.txt"},
                  new Property {Name = "revisionName", Value = "lastSuccessful"},
                  new Property {Name = "revisionValue", Value = "latest.lastSuccessful"}
                }
              }
            }
          }
        };

        m_client.BuildConfigs.SetArtifactDependency(buildLocatorFinal, artifactDependencies.ArtifactDependency[0]);

        m_client.BuildConfigs.ModifyArtifactDependencies(buildConfig.Id, depend, newDepend);

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
      finally
      {
        //Cleanup 
        m_client.BuildConfigs.DeleteConfiguration(buildLocatorFinal);
      }
      
    }

    [Test]
    public void it_modify_snapshot_dependencies()
    {
      const string depend = "TeamcityDashboardScenario_Test_TestWithCheckout";
      const string newDepend = "TeamcityDashboardScenario_Test_TestWithCheckoutWithDependencies";
      var buildLocatorFinal = new BuildTypeLocator();
      try
      {
        var buildConfig = m_client.BuildConfigs.CreateConfigurationByProjectId(m_goodProjectId, "testNewConfig");
        buildLocatorFinal = BuildTypeLocator.WithId(buildConfig.Id);
        var snapshotDependencies = new SnapshotDependencies
        {
          SnapshotDependency = new List<SnapshotDependency>
          {
            new SnapshotDependency
            {
              Id = "TTTT_100",
              Type = "snapshot_dependency",
              SourceBuildType = new BuildConfig{Id = depend},
              Properties = new Properties
              {
                Property = new List<Property>
                {
                  new Property {Name = "run-build-if-dependency-failed", Value = "RUN_ADD_PROBLEM"},
                  new Property {Name = "run-build-if-dependency-failed-to-start", Value = "MAKE_FAILED_TO_START"},
                  new Property {Name = "run-build-on-the-same-agent", Value = "false"},
                  new Property {Name = "take-started-build-with-same-revisions", Value = "true"},
                  new Property {Name = "take-successful-builds-only", Value = "true"}
                }
              }
            }
          }
        };

        m_client.BuildConfigs.SetSnapshotDependency(buildLocatorFinal, snapshotDependencies.SnapshotDependency[0]);

        m_client.BuildConfigs.ModifySnapshotDependencies(buildConfig.Id, depend, newDepend);

      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
      finally
      {
        //Cleanup 
        m_client.BuildConfigs.DeleteConfiguration(buildLocatorFinal);
      }

    }

    [Test]
    public void it_returns_first_build_types_builds_investigations_compatible_agents_no_field()

    {
      var tempBuildConfig = m_client.BuildConfigs.All().First();
      var buildConfig = m_client.BuildConfigs.ByConfigurationId(tempBuildConfig.Id);
      Assert.IsNotNull(buildConfig.Builds, "No builds ");
      Assert.IsNotNull(buildConfig.Builds.Href, "No builds href");
      Assert.IsNotNull(buildConfig.Investigations, "No Investigations ");
      Assert.IsNotNull(buildConfig.Investigations.Href, "No Investigations href");
      Assert.IsNotNull(buildConfig.CompatibleAgents, "No CompatibleAgents ");
      Assert.IsNotNull(buildConfig.CompatibleAgents.Href, "No CompatibleAgents href");
    }

    [Test]
    public void it_returns_first_build_types_builds_investigations_compatible_agents_field_null()

    {
      var tempBuildConfig = m_client.BuildConfigs.All().First();
      // Section 1
      var buildTypeField = BuildTypeField.WithFields(id:true);
      var buildConfig = m_client.BuildConfigs.GetFields(buildTypeField.ToString()).ByConfigurationId(tempBuildConfig.Id);
      Assert.IsNull(buildConfig.Builds, "No builds 1");
      Assert.IsNull(buildConfig.Investigations, "No Investigations 1");
      Assert.IsNull(buildConfig.CompatibleAgents, "No CompatibleAgents 1");

      // section 2
      var buildsField = BuildsField.WithFields(count:true);
      var investigationsField = InvestigationsField.WithFields();
      var compatibleAgentsField = CompatibleAgentsField.WithFields();
      buildTypeField = BuildTypeField.WithFields(id: true, builds: buildsField,investigations: investigationsField, compatibleAgents:compatibleAgentsField);
      buildConfig = m_client.BuildConfigs.GetFields(buildTypeField.ToString()).ByConfigurationId(tempBuildConfig.Id);
      Assert.IsNotNull(buildConfig.Builds, "No builds 2");
      Assert.IsNull(buildConfig.Builds.Href, "No builds href 2");
      Assert.IsNotNull(buildConfig.Investigations, "No Investigations 2");
      Assert.IsNotNull(buildConfig.Investigations.Href, "No Investigations href 2");
      Assert.IsNotNull(buildConfig.CompatibleAgents, "No CompatibleAgents 2");
      Assert.IsNotNull(buildConfig.CompatibleAgents.Href, "No CompatibleAgents href 2");

      // section 3
      buildsField = BuildsField.WithFields(count: true,href:true);
      investigationsField = InvestigationsField.WithFields(href:true);
      compatibleAgentsField = CompatibleAgentsField.WithFields(href:true);
      buildTypeField = BuildTypeField.WithFields(id: true, builds: buildsField, investigations: investigationsField, compatibleAgents: compatibleAgentsField);
      buildConfig = m_client.BuildConfigs.GetFields(buildTypeField.ToString()).ByConfigurationId(tempBuildConfig.Id);
      Assert.IsNotNull(buildConfig.Builds, "No builds 3");
      Assert.IsNotNull(buildConfig.Builds.Href, "No builds href 3");
      Assert.IsNotNull(buildConfig.Investigations, "No Investigations 3");
      Assert.IsNotNull(buildConfig.Investigations.Href, "No Investigations href 3");
      Assert.IsNotNull(buildConfig.CompatibleAgents, "No CompatibleAgents 3");
      Assert.IsNotNull(buildConfig.CompatibleAgents.Href, "No CompatibleAgents href 3");
    }

    [Test]
    public void it_returns_build_config_templates()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      var templates = m_client.BuildConfigs.GetTemplates(buildLocator);
      Assert.IsNotNull(templates, "No templates found, invalid templates call.");
    }

    [Test]
    public void it_attaches_templates_to_build_config()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      var buildConfig = new Template { Id = m_goodTemplateId };
      var buildConfigList = new List<Template>() { buildConfig };
      var templates = new Templates { BuildType = buildConfigList };
      m_client.BuildConfigs.AttachTemplates(buildLocator, templates);

      var templatesReceived = m_client.BuildConfigs.GetTemplates(buildLocator);
      Assert.That(templatesReceived.BuildType.Any(), "Templates not attached");
    }

    [Test]
    public void it_detaches_templates_from_build_config()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      var buildConfig = new Template { Id = m_goodTemplateId };
      var buildConfigList = new List<Template>() { buildConfig };
      var templates = new Templates { BuildType = buildConfigList };
      m_client.BuildConfigs.AttachTemplates(buildLocator, templates);
      var templatesReceived = m_client.BuildConfigs.GetTemplates(buildLocator);
      Assert.That(templatesReceived.BuildType.Any(), "Templates not attached");
      m_client.BuildConfigs.DetachTemplates(buildLocator);

      templatesReceived = m_client.BuildConfigs.GetTemplates(buildLocator);
      Assert.That(!templatesReceived.BuildType.Any(), "Templates not detached");

    }

    [Test]
    public void it_returns_build_config_templates_property()
    {
      string buildConfigId = m_goodBuildConfigId;
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      var buildConfig = new Template { Id = m_goodTemplateId };
      var buildConfigList = new List<Template>() { buildConfig };
      var templates = new Templates { BuildType = buildConfigList };
      m_client.BuildConfigs.AttachTemplates(buildLocator, templates);
      var templatesReceived = m_client.BuildConfigs.GetTemplates(buildLocator);
      Assert.That(templatesReceived.BuildType.Any(), "Templates not attached");

      var templatesField = m_client.BuildConfigs.ByConfigurationId(buildConfigId).Templates;
      Assert.IsNotNull(templatesField, "Templates property not retrieved correctly");
    }

    [Test]
    public void it_downloads_build_configuration()
    {
      string buildConfigId = m_goodBuildConfigId;
      string directory = Directory.GetCurrentDirectory();
      string destination = Path.Combine(directory, "config.txt");
      var buildLocator = BuildTypeLocator.WithId(buildConfigId);
      m_client.BuildConfigs.DownloadConfiguration(buildLocator, tempfile => System.IO.File.Move(tempfile, destination));
      Assert.IsTrue(System.IO.File.Exists(destination));
      Assert.IsTrue(new FileInfo(destination).Length > 0);
    }

    [Test]
    public void it_creates_build_configuration()
    {
      var currentBuildId = "testId";
      var buildProject = new Project() { Id = m_goodProjectId };
      var parameters = new Parameters
        { Property = new List<Property>() { new Property() { Name = "category", Value = "test"} } };
      var buildConfig = new BuildConfig() { Id = currentBuildId, Name = "testNewConfig", Project = buildProject, Parameters = parameters };

      try
      {
        buildConfig = m_client.BuildConfigs.CreateConfiguration(buildConfig);

        Assert.That(buildConfig.Id == currentBuildId);
      }
      catch (Exception e)
      {
        Assert.Fail($"{e.Message}", e);
      }
      finally
      {
        m_client.BuildConfigs.DeleteConfiguration(BuildTypeLocator.WithId(currentBuildId));
      }
    }

    [Test]
    public void it_returns_branches()
    {
      string buildConfigId = m_goodBuildConfigId;
      var tempBuild = m_client.BuildConfigs.GetBranchesByBuildConfigurationId(buildConfigId);
      Assert.IsTrue(tempBuild.Count == 2);
    }

    [Test]
    public void it_returns_branches_history()
    {
      string buildConfigId = m_goodBuildConfigId;
      var tempBuild = m_client.BuildConfigs.GetBranchesByBuildConfigurationId(buildConfigId,BranchLocator.WithDimensions(BranchPolicy.ALL_BRANCHES));
      Assert.IsTrue(tempBuild.Count == 6);
    }

    [Test]
    public void it_returns_branches_history_with_field_Default_but_active_not_fetched()
    {
      BranchField branchField = BranchField.WithFields(name:true,defaultValue:true);
      BranchesField branchesField = BranchesField.WithFields(branch: branchField);
      string buildConfigId = m_goodBuildConfigId;
      var tempBuild = m_client.BuildConfigs.GetFields(branchesField.ToString()).GetBranchesByBuildConfigurationId(buildConfigId, BranchLocator.WithDimensions(BranchPolicy.ALL_BRANCHES));
      var checkIfFieldWork = tempBuild.Branch.Single(x => x.Default);
      Assert.IsTrue(checkIfFieldWork.Active == false);

    }

    [Test]
    public void it_returns_branches_history_with_field_Default_active_fetched()
    {
      BranchField branchField = BranchField.WithFields(name: true, defaultValue: true,active:true);
      BranchesField branchesField = BranchesField.WithFields(branch: branchField);
      string buildConfigId = m_goodBuildConfigId;
      var tempBuild = m_client.BuildConfigs.GetFields(branchesField.ToString()).GetBranchesByBuildConfigurationId(buildConfigId, BranchLocator.WithDimensions(BranchPolicy.ALL_BRANCHES));
      var checkIfFieldWork = tempBuild.Branch.Single(x => x.Default);
      Assert.IsTrue(checkIfFieldWork.Active);

    }


    #region private
    private string GetXml(object data)
    {
      XmlSerializer xsSubmit = new XmlSerializer(data.GetType());
      var ns = new XmlSerializerNamespaces();
      ns.Add("", "");

      XmlWriterSettings settings = new XmlWriterSettings();
      settings.OmitXmlDeclaration = true;
      using (var sww = new StringWriter())
      {
        using (XmlWriter writer = XmlWriter.Create(sww, settings))
        {
          xsSubmit.Serialize(writer, data,ns);
          return sww.ToString();
        }
      }
    }
#endregion

  }
}