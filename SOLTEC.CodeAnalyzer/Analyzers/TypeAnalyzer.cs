using SOLTEC.CodeAnalyzer.Models;
using SOLTEC.CodeAnalyzer.Utils;

namespace SOLTEC.CodeAnalyzer.Analyzers;

/// <summary>
/// Performs unified analysis of classes, records, and interfaces in a project,
/// applying SOLTEC programming standards.
/// </summary>
/// <example>
/// <![CDATA[
/// var results = TypeAnalyzer.AnalyzeAllTypes("C:\\MyProject");
/// ]]>
/// </example>
public static class TypeAnalyzer
{
    /// <summary>
    /// Analyzes all C# files in the given directory for SOLTEC coding compliance.
    /// </summary>
    /// <param name="projectPath">Base directory of the project.</param>
    /// <returns>List of results with violations per file.</returns>
    public static List<AnalysisResult> AnalyzeAllTypes(string projectPath)
    {
        var _results = new List<AnalysisResult>();
        var _csFiles = FileScanner.GetCsFiles(projectPath);

        foreach (var _file in _csFiles)
        {
            string _content = File.ReadAllText(_file);
            var _violations = new List<string>();

            // Verificar namespace
            var (namespaceValid, namespaceError) = NamespaceAnalyzer.AnalyzeNamespace(_file, _content, projectPath);
            if (!namespaceValid) _violations.Add(namespaceError);

            // Verificar documentación XML
            _violations.AddRange(XmlDocAnalyzer.AnalyzeDocumentation(_content));

            // Verificar nomenclatura
            _violations.AddRange(NamingRulesAnalyzer.AnalyzeNamingRules(_content));

            // Guardar resultados si hay violaciones
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