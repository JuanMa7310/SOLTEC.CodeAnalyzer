using System.Text.RegularExpressions;

namespace SOLTEC.CodeAnalyzer.Analyzers;

/// <summary>
/// Analyzes naming conventions of variables and constants based on SOLTEC standards.
/// </summary>
/// <example>
/// <![CDATA[
/// var violations = NamingRulesAnalyzer.AnalyzeNamingRules(fileContent);
/// ]]>
/// </example>
public static partial class NamingRulesAnalyzer
{
    /// <summary>
    /// Validates local, global, and constant variable names based on naming rules.
    /// </summary>
    /// <param name="fileContent">The content of the .cs file.</param>
    /// <returns>List of violations related to naming conventions.</returns>
    public static List<string> AnalyzeNamingRules(string fileContent)
    {
        var _violations = new List<string>();
        var _lines = fileContent.Split('\n');

        // Local variables: _x
        foreach (Match _match in LocalVariablePattern().Matches(fileContent))
        {
            int _lineNumber = fileContent.Substring(0, _match.Index).Split('\n').Length - 1;
            if (IsInMethodSignature(_lines, _lineNumber))
            {
                continue;
            }
            string _name = _match.Groups[1].Value;
            if (!_name.StartsWith('_'))
            {
                _violations.Add($"Local variable '{_name}' should start with underscore followed by lowercase.");
            }
        }

        // Global variables: gX
        foreach (Match _match in GlobalFieldPattern().Matches(fileContent))
        {
            string _name = _match.Groups[1].Value;
            if (!_name.StartsWith('g'))
            {
                _violations.Add($"Global field '{_name}' should start with lowercase 'g'.");
            }
        }

        // Global constants: gcX
        foreach (Match _match in GlobalConstantPattern().Matches(fileContent))
        {
            string _name = _match.Groups[1].Value;
            if (!_name.StartsWith("gc"))
            {
                _violations.Add($"Global constant '{_name}' should start with 'gc'.");
            }
        }

        // Local constants: _cX
        foreach (Match _match in LocalConstantPattern().Matches(fileContent))
        {
            string _name = _match.Groups[1].Value;
            if (!_name.StartsWith("_c"))
            {
                _violations.Add($"Local constant '{_name}' should start with '_c'.");
            }
        }

        return _violations;
    }
    // Comprobar si la coincidencia está dentro de una cabecera de método (línea contiene '(', ')', y no '{')
    private static bool IsInMethodSignature(string[] lines, int matchIndex)
    {
        for (int i = matchIndex; i >= 0; i--)
        {
            string _line = lines[i].Trim();
            if (_line.Contains("{")) return false;
            if (_line.Contains(")") && _line.Contains("(")) return true;
        }
        return false;
    }

    // ----------------------
    // REGEX HELPERS
    // ----------------------

    [GeneratedRegex(@"\b(?:var|int|float|double|string|bool|decimal|char)\s+(_?[a-zA-Z_][\w]*)\s*(?:[=;])", RegexOptions.Compiled)]
    private static partial Regex LocalVariablePattern();

    [GeneratedRegex(@"\b(?:public|private|protected|internal)\s+(?!const)[\w<>\[\]]+\s+(g\w+)\s*(=|;)", RegexOptions.Compiled)]
    private static partial Regex GlobalFieldPattern();

    [GeneratedRegex(@"\b(?:public|private|protected|internal)\s+const\s+[\w<>\[\]]+\s+(gc\w+)\s*=", RegexOptions.Compiled)]
    private static partial Regex GlobalConstantPattern();

    [GeneratedRegex(@"\bconst\s+[\w<>\[\]]+\s+(_c\w+)\s*=", RegexOptions.Compiled)]
    private static partial Regex LocalConstantPattern();
}