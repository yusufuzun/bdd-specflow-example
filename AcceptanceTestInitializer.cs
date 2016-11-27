using BddTestingExample.JiraIntegration;
using NUnit.Framework;

namespace BddTestingExample
{
    [SetUpFixture]
    public class AcceptanceTestInitializer
    {
        [OneTimeSetUp]
        public void SyncJiraFeatureFiles()
        {
            var dynamicDownloadEnabled = false;

            if (dynamicDownloadEnabled)
            {
                JiraBehaveProInitializer.Download();
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            //TODO: create documentation automatically here
        }
    }
}