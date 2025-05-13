using SOLTEC.CodeAnalyzer.Analyzers;
using SOLTEC.CodeAnalyzer.Report;
using SOLTEC.CodeAnalyzer.Utils;
using SOLTEC.CodeAnalyzer.Models;

/// <summary>
/// Entry point for the SOLTEC.CodeAnalyzer console application.
/// Analyzes C# classes, records and interfaces for compliance with SOLTEC coding standards.
/// </summary>
/// <example>
/// <![CDATA[
/// dotnet run -- -p "C:\\Projects\\MyProject" -o "C:\\Reports\\report.md" -c
/// ]]>
/// </example>

if (!ParameterValidator.Validate(args, out string _projectPath, out string _reportPath, out bool _printToConsole, out string _error))
{
    Console.WriteLine(_error);
    return;
}

try
{
    Console.WriteLine("🔍 Starting analysis...");
    List<AnalysisResult> _results = TypeAnalyzer.AnalyzeAllTypes(_projectPath);
    MarkdownReportGenerator.Generate(_results, _reportPath);
    Console.WriteLine($"✅ Report saved to: {_reportPath}");

    if (_printToConsole)
    {
        Console.WriteLine("\n📤 Violations Summary:");
        foreach (var _result in _results)
        {
            Console.WriteLine($"\n📄 {_result.FilePath}");
            foreach (var _violation in _result.Violations)
            {
                Console.WriteLine($"  ❌ {_violation}");
            }
        }
    }
}
catch (Exception _ex)
{
    Console.WriteLine($"❌ Unexpected error: {_ex.Message}");
}