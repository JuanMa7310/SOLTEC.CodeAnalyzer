// File: ClassAnalyzer.cs

namespace SOLTEC.CodeAnalyzer.Analyzers;

using SOLTEC.CodeAnalyzer.Models;

/// <summary>
/// Coordinates the analysis of C# classes using SOLTEC standards.
/// </summary>
/// <example>
/// <![CDATA[
/// var results = ClassAnalyzer.AnalyzeAllClasses("C:\\MyProject");
/// foreach (var result in results)
/// {
///     Console.WriteLine($"{result.FileName}: {result.Violations.Count} issue(s)");
/// }
/// ]]>
/// </example>
public static class ClassAnalyzer
{
    /// <summary>
    /// Analyzes all C# files in a given directory recursively.
    /// </summary>
    /// <param name="projectPath">The root folder of the project to analyze.</param>
    /// <returns>A list of analysis results for each class/file.</returns>
    public static List<AnalysisResult> AnalyzeAllClasses(string projectPath)
    {
        var _results = new List<AnalysisResult>();
        var _csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);

        foreach (string _file in _csFiles)
        {
            string _content = File.ReadAllText(_file);
            var _violations = new List<string>();

            // Run all analyzers
            var (namespaceValid, namespaceError) = NamespaceAnalyzer.AnalyzeNamespace(_file, _content, projectPath);
            if (!namespaceValid) _violations.Add(namespaceError);

            _violations.AddRange(XmlDocAnalyzer.AnalyzeDocumentation(_content));
            _violations.AddRange(NamingRulesAnalyzer.AnalyzeNamingRules(_content));

            // If there are violations, save the result
            if (_violations.Any())
            {
                _results.Add(new AnalysisResult
                {
                    FilePath = _file,
                    Violations = _violations
                });
            }
        }

        return _results;
    }
}