using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

public static class GetProjectsByCsprojFile
{
    public static IDictionary<string, string> Projects { get; } = GetProjects();

    private static IDictionary<string, string> GetProjects()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directoryInfo != null && !directoryInfo.GetFiles("*.sln").Any())
        {
            directoryInfo = directoryInfo.Parent;
        }

        string solutionFolder = directoryInfo!.FullName.Replace("\\", "/");

        string[] csprojFiles = Directory.GetFiles(solutionFolder, "*.csproj", SearchOption.AllDirectories);

        IDictionary<string, string> projects = new Dictionary<string, string>();

        foreach (string csprojFile in csprojFiles)
        {
            string projectFolder = Path.GetDirectoryName(csprojFile)!.Replace("\\", "/");
            string projectName = Path.GetFileNameWithoutExtension(csprojFile);
            projects.Add(projectName, projectFolder);
        }

        return projects;
    }
}