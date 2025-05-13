using SOLTEC.CodeAnalyzer.Enums;

namespace SOLTEC.CodeAnalyzer.Utils;

/// <summary>
/// Detects the type of .NET project based on the .csproj content.
/// </summary>
/// <example>
/// <![CDATA[
/// var projectType = ProjectTypeDetector.DetectType("C:\\MyProject");
/// Console.WriteLine($"Detected: {projectType}");
/// ]]>
/// </example>
public static class ProjectTypeDetector
{
    /// <summary>
    /// Attempts to detect the type of project from the first .csproj file found.
    /// </summary>
    /// <param name="projectDirectory">Path to the base project directory.</param>
    /// <returns>The detected project type.</returns>
    public static ProjectType DetectType(string projectDirectory)
    {
        var _csprojFile = Directory.EnumerateFiles(projectDirectory, "*.csproj", SearchOption.AllDirectories)
                                   .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(_csprojFile) || !File.Exists(_csprojFile))
            return ProjectType.Unknown;

        string _content = File.ReadAllText(_csprojFile);

        if (_content.Contains("Microsoft.NET.Sdk.Web"))
            return ProjectType.WebApi;

        if (_content.Contains("Microsoft.NET.Sdk.Razor"))
            return ProjectType.RazorApp;

        if (_content.Contains("<OutputType>Exe</OutputType>"))
            return ProjectType.ConsoleApp;

        if (_content.Contains("<OutputType>Library</OutputType>"))
            return ProjectType.ClassLibrary;

        return ProjectType.Unknown;
    }
}