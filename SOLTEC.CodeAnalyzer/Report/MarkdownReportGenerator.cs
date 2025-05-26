using SOLTEC.CodeAnalyzer.Models;
using System.Text;

namespace SOLTEC.CodeAnalyzer.Report;

/// <summary>
/// Generates a Markdown report from analysis results.
/// </summary>
/// <example>
/// <![CDATA[
/// var _results = ClassAnalyzer.AnalyzeAllClasses("C:\\MyProject");
/// MarkdownReportGenerator.Generate(results, "C:\\Report\\output.md");
/// ]]>
/// </example>
public static class MarkdownReportGenerator
{
    /// <summary>
    /// Generates and saves a Markdown report with the results of the class analysis.
    /// </summary>
    /// <param name="results">List of analysis results per file.</param>
    /// <param name="outputPath">Path to save the report.</param>
    public static void Generate(List<AnalysisResult> results, string outputPath)
    {
        var _sb = new StringBuilder();

        _sb.AppendLine("# 📋 SOLTEC Code Analysis Report");
        _sb.AppendLine();
        _sb.AppendLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        _sb.AppendLine();
        _sb.AppendLine("## Summary");
        _sb.AppendLine($"- Files analyzed: {results.Count}");
        _sb.AppendLine($"- Total violations: {results.Sum(r => r.Violations.Count)}");
        _sb.AppendLine();

        foreach (var _result in results)
        {
            _sb.AppendLine($"## 📄 `{Path.GetFileName(_result.FilePath)}`");
            _sb.AppendLine($"**Path**: `{_result.FilePath}`");
            _sb.AppendLine();
            _sb.AppendLine($"Violations ({_result.Violations.Count}): ");
            _sb.AppendLine();
            foreach (var _violation in _result.Violations)
            {
                _sb.AppendLine($"- ❌ {_violation}");
            }
            _sb.AppendLine();
            _sb.AppendLine();
            _sb.AppendLine($"Alertss ({_result.Alerts.Count}): ");
            _sb.AppendLine();
            foreach (var _alert in _result.Alerts)
            {
                _sb.AppendLine($"- ❌ {_alert}");
            }
            _sb.AppendLine($"-----------------------------------------");
            _sb.AppendLine();
        }

        File.WriteAllText(outputPath, _sb.ToString(), Encoding.UTF8);
    }
}