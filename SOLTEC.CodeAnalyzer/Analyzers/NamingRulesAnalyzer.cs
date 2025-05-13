// File: NamingRulesAnalyzer.cs

namespace SOLTEC.CodeAnalyzer.Analyzers;

using System.Text.RegularExpressions;

/// <summary>
/// Validates that variable, constant and parameter names follow SOLTEC naming conventions.
/// </summary>
/// <example>
/// <![CDATA[
/// var violations = NamingRulesAnalyzer.AnalyzeNamingRules(fileContent);
/// foreach (var v in violations)
/// {
///     Console.WriteLine(v);
/// }
/// ]]>
/// </example>
public static class NamingRulesAnalyzer
{
    /// <summary>
    /// Analyzes a C# file for naming convention violations.
    /// </summary>
    /// <param name="fileContent">The source code of the file.</param>
    /// <returns>List of violations found.</returns>
    public static List<string> AnalyzeNamingRules(string fileContent)
    {
        var _violations = new List<string>();

        // Campos globales que no empiezan con "g"
        var _fieldMatches = Regex.Matches(fileContent, @"(private|public|protected)\s+(static\s+)?[\w<>\[\]]+\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*;", RegexOptions.Multiline);
        foreach (Match _match in _fieldMatches)
        {
            string _name = _match.Groups[3].Value;
            if (!_name.StartsWith("g") && !_name.StartsWith("gc"))
            {
                _violations.Add($"Global field '{_name}' should start with 'g' or 'gc'.");
            }
        }

        // Constantes locales que no empiezan con "_c"
        var _constMatches = Regex.Matches(fileContent, @"const\s+[\w<>\[\]]+\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*=", RegexOptions.Multiline);
        foreach (Match _match in _constMatches)
        {
            string _name = _match.Groups[1].Value;
            if (!_name.StartsWith("gc") && !_name.StartsWith("_c"))
            {
                _violations.Add($"Constant '{_name}' should start with 'gc' (global) or '_c' (local).");
            }
        }

        // Variables locales (dentro de métodos) que no empiezan con "_"
        var _localVarMatches = Regex.Matches(fileContent, @"(var|[\w<>\[\]])\s+([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*.*?;", RegexOptions.Multiline);
        foreach (Match _match in _localVarMatches)
        {
            string _name = _match.Groups[2].Value;
            if (!_name.StartsWith("_") && !_name.StartsWith("gc") && !_name.StartsWith("g"))
            {
                _violations.Add($"Local variable '{_name}' should start with '_'.");
            }
        }

        // Parámetros de métodos que no comienzan en minúscula
        var _paramMatches = Regex.Matches(fileContent, @"\((.*?)\)", RegexOptions.Singleline);
        foreach (Match _match in _paramMatches)
        {
            var _paramList = _match.Groups[1].Value.Split(',');
            foreach (var _param in _paramList)
            {
                var _parts = _param.Trim().Split(' ');
                if (_parts.Length == 2)
                {
                    string _paramName = _parts[1];
                    if (!string.IsNullOrEmpty(_paramName) && char.IsUpper(_paramName[0]))
                    {
                        _violations.Add($"Parameter '{_paramName}' should start with lowercase.");
                    }
                }
            }
        }

        return _violations;
    }
}