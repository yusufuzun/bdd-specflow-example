using System;

namespace BddTestingExample.JiraIntegration
{
    public static class JiraBehaveProInitializer
    {
        public static void Download()
        {
            try
            {
                JiraConnector jiraConnector = new JiraConnector("acceptanceTests.jiraConnector.url");
                jiraConnector.Username = "acceptanceTests.jiraConnector.username";
                jiraConnector.Password = "acceptanceTests.jiraConnector.password";
                jiraConnector.Project = "acceptanceTests.jiraConnector.projectCode";
                jiraConnector.AddManualTests = false;
                jiraConnector.IsNUnit = true;
                jiraConnector.VerifySsl = false;
                jiraConnector.Directory = "acceptanceTests.jiraConnector.relativeImportDirectory";
                jiraConnector.ProjFile = "acceptanceTests.jiraConnector.relativeProjectPath";
                jiraConnector.JiraComponentName = "acceptanceTests.jiraConnector.jiraComponentName";

                jiraConnector.FetchBehaveProFeatures();
                jiraConnector.IncludeProject();
            }
            catch
            {
                //Jira connector gets error.
            }
        }
    }
}