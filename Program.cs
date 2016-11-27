using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Gherkin;

namespace BddTestingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //example for cucumber
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\", "Features");
            Regex rgx = new Regex(@"@UserStory\((\d+)\)");
            foreach (string file in System.IO.Directory.GetFiles(directory, "*.feature", SearchOption.AllDirectories))
            {
                Parser parser = new Parser();
                var doc = parser.Parse(file);
                if (doc.Feature != null && doc.Feature.Tags.Any())
                {
                    foreach (var tag in doc.Feature.Tags.Where(x => rgx.IsMatch(x.Name)))
                    {
                        var taskId = rgx.Match(tag.Name).Groups[1].Captures[0].Value;
                        UpdateUserStory(taskId);
                    }

                }
            }
            //
        }

        private static void UpdateUserStory(string userStoryId)
        {
            Console.WriteLine($"Changed User Story Status by using API: {userStoryId}");
        }
    }
}


/*

For not getting conflict, and branch independent testing, we simply create our test result documents under a new folder with branch name on it.
Example: If we working on feature/{taskNumber} branch, we can create a folder under doc\AcceptanceTest named as "{branchFolder}" and use this folder to put all test results there.

So we have two folders under doc\AcceptanceTest\{branchFolder}:
- doc\AcceptanceTest\{branchFolder}
-- TestResult
--- test-result.xml
-- Dhtml
--- Generated Files

If we want excel files also, there will be one more folder named as Excel and it contains excel output of tests.

For now, we execute tests manually for feeding Pickle UI by using these commands:

1- For Nunit console runner we send this command via command prompt:

nunit3-console.exe "{ProjectFilePath}.csproj" --workers=1 --agents=1 --result="{OutputFolderPath}\doc\AcceptanceTest\{branchFolder}\TestResult\test-result.xml"

This command will create an NUnit3 test result xml file to doc\AcceptanceTest folder in current repository.

2- For PickleUI to create result pages and such files like excel, we send nunit test result file to PickleUI by using command prompt again:

Here you have different options to use Pickles, but I suggest you to installing Pickles over chocolatey by using command line.

Pickle-Features -TestResultsFile "{OutputFolderPath}\doc\AcceptanceTest\{branchFolder}\TestResult\test-result.xml" -TestResultsFormat nunit3 -FeatureDirectory "{TestProjectFolderPath}\Features" -OutputDirectory "{OutputFolderPath}\doc\AcceptanceTest\{BranchFolder}\dhtml\" -DocumentationFormat dhtml

*/
