using System.Text.RegularExpressions;

namespace SOLTEC.CodeAnalyzer.Analyzers;

/// <summary>
/// Validates the structure and position of namespace declarations within a C# file.
/// </summary>
/// <example>
/// <![CDATA[
/// var errors = NamespaceStructureAnalyzer.AnalyzeNamespaceStructure(fileContent);
/// foreach (var e in errors) Console.WriteLine(e);
/// ]]>
/// </example>
public static class NamespaceStructureAnalyzer
{
    /// <summary>
    /// Checks that the namespace is placed at the top of the file and appears only once.
    /// </summary>
    /// <param name="fileContent">The content of the .cs file.</param>
    /// <returns>A list of violations found related to namespace structure.</returns>
    public static List<string> AnalyzeNamespaceStructure(string fileContent)
    {
        var _violations = new List<string>();
        var _namespaceRegex = new Regex(@"^\s*namespace\s+[A-Za-z0-9_.]+\s*", RegexOptions.Multiline);
        var _matches = _namespaceRegex.Matches(fileContent);

        if (_matches.Count == 0)
        {
            _violations.Add("No namespace declaration found.");
            return _violations;
        }

        if (_matches.Count > 1)
        {
            _violations.Add("Multiple namespace declarations found in the same file.");
        }

        // Confirm that the first non-comment line is the namespace declaration
        string[] _lines = fileContent.Split('\n');
        for (int _i = 0; _i < _lines.Length; _i++)
        {
            string _trimmed = _lines[_i].Trim();
            if (string.IsNullOrWhiteSpace(_trimmed) || _trimmed.StartsWith("//") || _trimmed.StartsWith("/*"))
                continue;

            if (!_trimmed.StartsWith("namespace "))
            {
                _violations.Add("Namespace declaration is not at the top of the file.");
            }
            break;
        }

        return _violations;
    }
}