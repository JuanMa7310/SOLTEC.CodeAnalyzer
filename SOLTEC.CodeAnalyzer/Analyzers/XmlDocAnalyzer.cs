namespace SOLTEC.CodeAnalyzer.Analyzers;

using SOLTEC.CodeAnalyzer.Enums;
using System.Text.RegularExpressions;

/// <summary>
/// Validates XML documentation presence for public and protected members depending on project type.
/// </summary>
/// <example>
/// <![CDATA[
/// var results = XmlDocAnalyzer.AnalyzeDocumentation(fileContent, ProjectType.WebApi);
/// ]]>
/// </example>
public static partial class XmlDocAnalyzer
{
    /// <summary>
    /// Validates XML documentation tags in a C# file based on project type.
    /// </summary>
    /// <param name="fileContent">The content of the file.</param>
    /// <param name="projectType">The type of project (WebApi, ClassLibrary, etc.).</param>
    /// <returns>List of violations found.</returns>
    public static List<string> AnalyzeDocumentation(string fileContent, ProjectType projectType)
    {
        var _violations = new List<string>();

        if (projectType is ProjectType.RazorApp or ProjectType.Unknown)
            return _violations;

        var _lines = fileContent.Split('\n');

        for (int _i = 0; _i < _lines.Length; _i++)
        {
            string _line = _lines[_i].Trim();

            bool _isDeclaration = (_line.StartsWith("public") || _line.StartsWith("protected")) &&
                (_line.Contains("class ") || _line.Contains("interface ") || _line.Contains("enum ") ||
                 _line.Contains("struct ") || _line.Contains("record ") || _line.Contains("delegate ") ||
                 _line.Contains("void ") || _line.Contains("event ") || _line.Contains('(') || _line.Contains('{'));

            if (!_isDeclaration) continue;

            if (_i == 0 || !_lines[_i - 1].TrimStart().StartsWith("///"))
            {
                _violations.Add($"Missing XML documentation for declaration: '{_line}'");
                continue;
            }

            bool _hasSummary = false;
            int _searchBack = _i - 1;

            while (_searchBack >= 0 && _lines[_searchBack].TrimStart().StartsWith("///"))
            {
                if (SummaryTagRegex().IsMatch(_lines[_searchBack]))
                {
                    _hasSummary = true;
                    break;
                }
                _searchBack--;
            }

            if (!_hasSummary)
            {
                _violations.Add($"XML documentation found, but <summary> tag is missing near: '{_line}'");
            }

            if (projectType == ProjectType.ClassLibrary &&
                (_line.Contains("class ") || _line.Contains("void ") || _line.Contains('(')))
            {
                bool _hasExample = false;
                _searchBack = _i - 1;

                while (_searchBack >= 0 && _lines[_searchBack].TrimStart().StartsWith("///"))
                {
                    if (ExampleTagRegex().IsMatch(_lines[_searchBack]))
                    {
                        _hasExample = true;
                        break;
                    }
                    _searchBack--;
                }

                if (!_hasExample)
                {
                    _violations.Add($"Missing <example> tag in XML documentation near: '{_line}'");
                }
            }

            if (projectType == ProjectType.ConsoleApp &&
                !_line.Contains("Program") && !_line.Contains("Main"))
            {
                continue;
            }
        }

        return _violations;
    }

    [GeneratedRegex(@"///\s*<summary>", RegexOptions.Multiline)]
    private static partial Regex SummaryTagRegex();

    [GeneratedRegex(@"///\s*<example>", RegexOptions.Multiline)]
    private static partial Regex ExampleTagRegex();
}
