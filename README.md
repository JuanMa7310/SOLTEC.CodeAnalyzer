# SOLTEC.CodeAnalyzer

**SOLTEC.CodeAnalyzer** is a command-line utility developed in **C# .NET 8** using **C# 12** language features. It analyzes C# classes within a given project directory to ensure they comply with SOLTEC's internal programming standards.

## 🔍 What It Does

- Scans all `.cs` files in the specified directory.
- Analyzes each class to verify:
  - Proper namespace declaration and structure.
  - XML documentation presence and format (including usage examples).
  - Variable and constant naming conventions.
  - Presence of unit and integration tests for methods with logic.
- Records all rule violations per class.

## 🧾 Output

- Generates a detailed **Markdown report** listing:
  - Files and classes that violate standards.
  - Specific rules that were not followed.

## 🧑‍💻 How to Use

```bash
dotnet run -- "C:\Path\To\Project" "C:\Path\To\report.md"
```

- **First argument**: directory path of the project to analyze.
- **Second argument**: file path to save the generated Markdown report.
