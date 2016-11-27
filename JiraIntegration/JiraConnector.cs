using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using Gherkin;

namespace BddTestingExample.JiraIntegration
{
    public class JiraConnector
    {
        public string JiraUrl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Project { get; set; }

        public string Directory { get; set; }

        public bool AddManualTests { get; set; }

        public bool VerifySsl { get; set; }

        public bool IsNUnit { get; set; }

        public string ProjFile { get; set; }

        public string JiraComponentName { get; set; }

        public JiraConnector(string jiraUrl)
        {
            JiraUrl = jiraUrl.TrimEnd('/');
        }

        public void FetchBehaveProFeatures()
        {
            ExtractZipStream(GetBehaveProFeatureZipStream(Project, AddManualTests, VerifySsl, IsNUnit), Directory);

            foreach (string file in System.IO.Directory.GetFiles(Directory, "*.feature", SearchOption.AllDirectories))
            {
                Parser parser = new Parser();
                var doc = parser.Parse(file);

                if (doc.Feature == null
                    || doc.Feature.Tags.All(x => x.Name != $"@Component:{JiraComponentName}"))
                {
                    File.Delete(file);
                }
            }
        }

        public void IncludeProject()
        {
            var projFile = new Microsoft.Build.Evaluation.Project(ProjFile);

            foreach (string file in System.IO.Directory.GetFiles(Directory, "*.feature", SearchOption.AllDirectories))
            {
                var fileName = Path.GetFileName(file);
                var relativeFilePath = MakeRelative(file, ProjFile).Replace("/", "\\");
                var projectItem = projFile.Items.FirstOrDefault(i => i.EvaluatedInclude == relativeFilePath);
                if (projectItem == null)
                {
                    projFile.AddItem("None", relativeFilePath, new[]
                    {
                        new KeyValuePair<string, string>("Generator", "SpecFlowSingleFileGenerator"),
                        new KeyValuePair<string, string>("LastGenOutput", $"{fileName}.cs")
                    });
                }
            }
            projFile.Save();

        }

        public static string MakeRelative(string filePath, string referencePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(referencePath);
            return referenceUri.MakeRelativeUri(fileUri).ToString();
        }

        public Stream GetBehaveProFeatureZipStream(string projectKey, bool manual, bool verify, bool isNUnit)
        {
            WebClient webClient = Username == null ? new WebClient() : CreateAuthorizedClient();
            if (!verify)
                ServicePointManager.ServerCertificateValidationCallback = (param0, param1, param2, param3) => true;
            webClient.Headers["accept"] = "application/zip";
            var address =
                string.Format("{0}/rest/cucumber/1.0/project/{1}/features?manual={2}&nunit={3}", JiraUrl, projectKey, manual, isNUnit);
            return CopyAndClose(webClient.OpenRead(address));
        }

        public void ExtractZipStream(Stream zipStream, string targetDirectory)
        {
            if (!System.IO.Directory.Exists(targetDirectory))
            {
                System.IO.Directory.CreateDirectory(targetDirectory);
            }
            using (ZipArchive zipFile = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry zipEntry in zipFile.Entries)
                {
                    zipEntry.ExtractToFile(Path.Combine(targetDirectory, zipEntry.FullName), true);
                }
            }
        }

        public WebClient CreateAuthorizedClient()
        {
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(Username, Password);
            string base64String = Convert.ToBase64String(Encoding.Default.GetBytes(Username + ":" + Password));
            webClient.Headers["Authorization"] = "Basic " + base64String;
            return webClient;
        }

        private Stream CopyAndClose(Stream inputStream)
        {
            byte[] buffer = new byte[256];
            MemoryStream memoryStream = new MemoryStream();
            for (int count = inputStream.Read(buffer, 0, 256); count > 0; count = inputStream.Read(buffer, 0, 256))
            {
                memoryStream.Write(buffer, 0, count);
            }
            memoryStream.Position = 0L;
            inputStream.Close();
            return memoryStream;
        }
    }
}