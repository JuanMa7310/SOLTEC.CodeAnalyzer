using SOLTEC.CodeAnalyzer.Enums;
using SOLTEC.CodeAnalyzer.Models;
using SOLTEC.CodeAnalyzer.Utils;

namespace SOLTEC.CodeAnalyzer.Analyzers;

/// <summary>
/// Performs unified analysis of C# types using SOLTEC programming standards.
/// </summary>
/// <example>
/// <![CDATA[
/// var _results = TypeAnalyzer.AnalyzeAllTypes("C:\\MyProject", ProjectTypeDetector.ProjectType.WebApi);
/// ]]>
/// </example>
public static class TypeAnalyzer
{
    /// <summary>
    /// Analyzes all C# files for violations based on project type.
    /// </summary>
    /// <param name="projectPath">The root directory of the project.</param>
    /// <param name="projectType">The detected project type.</param>
    /// <returns>List of violations grouped by file.</returns>
    public static List<AnalysisResult> AnalyzeAllTypes(string projectPath, ProjectType projectType)
    {
        var _results = new List<AnalysisResult>();
        var _csFiles = FileScanner.GetCsFiles(projectPath);

        foreach (var _file in _csFiles)
        {
            string _content = File.ReadAllText(_file);
            var _violations = new List<string>();

            // Namespace validity and structure
            var (namespaceValid, namespaceError) = NamespaceAnalyzer.AnalyzeNamespace(_file, projectPath);
            if (!namespaceValid) _violations.Add(namespaceError);

            _violations.AddRange(NamespaceStructureAnalyzer.AnalyzeNamespaceStructure(_content));

            // Documentation and naming rules
            _violations.AddRange(XmlDocAnalyzer.AnalyzeDocumentation(_content, projectType));
            _violations.AddRange(NamingRulesAnalyzer.AnalyzeNamingRules(_content));

            // Inheritance control rules
            _violations.AddRange(InheritanceRulesAnalyzer.AnalyzeInheritanceControl(_content));
            // Implementation-level checks
            var (_myViolations, _alerts) = ImplementationAnalyzer.AnalyzeImplementation(_content);

            _violations.AddRange(_myViolations);

            if (_violations.Count > 0)
            {
                _results.Add(new AnalysisResult
                {
                    FilePath = _file,
                    Violations = _violations,
                    Alerts = _alerts
                });
            }
        }

        return _results;
    }
}
