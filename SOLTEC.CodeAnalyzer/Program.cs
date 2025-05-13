// File: Program.cs

namespace SOLTEC.CodeAnalyzer;

using SOLTEC.CodeAnalyzer.Analyzers;
using SOLTEC.CodeAnalyzer.Report;
using SOLTEC.CodeAnalyzer.Utils;

/// <summary>
/// Entry point for the SOLTEC.CodeAnalyzer console application.
/// Analyzes C# files for compliance with SOLTEC coding standards
/// and generates a Markdown report listing violations.
/// </summary>
/// <example>
/// <![CDATA[
/// dotnet run -- "C:\\Projects\\MiProyecto" "C:\\Reports\\reporte.md"
/// ]]>
/// </example>
internal class Program
{
    /// <summary>
    /// Main execution method.
    /// </summary>
    /// <param name="args">Arguments: [0] = project path, [1] = output report path</param>
    private static void Main(string[] args)
    {
        if (!ParameterValidator.Validate(args, out string _projectPath, out string _reportPath, out string _error))
        {
            Console.WriteLine(_error);
            return;
        }

        try
        {
            Console.WriteLine("🔍 Starting analysis...");
            var _results = ClassAnalyzer.AnalyzeAllClasses(_projectPath);
            MarkdownReportGenerator.Generate(_results, _reportPath);
            Console.WriteLine($"✅ Analysis complete. Report saved to: {_reportPath}");
        }
        catch (Exception _ex)
        {
            Console.WriteLine($"❌ Unexpected error: {_ex.Message}");
        }
    }
}