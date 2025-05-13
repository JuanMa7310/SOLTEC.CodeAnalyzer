# SOLTEC.CodeAnalyzer

**SOLTEC.CodeAnalyzer** is a professional C# (.NET 8, C# 12) console utility that analyzes source code files for compliance with SOLTEC's programming standards.

## 🚀 Quick Start

You can use the included scripts to run the analyzer interactively:

- **Windows:** `run-analyzer.bat`
- **Linux/macOS:** `run-analyzer.sh`

Each script will prompt you to:
- Enter the path to the project directory
- Enter the path to save the Markdown report
- Optionally choose to also print results to the console

## 🔧 CLI Manual Usage

```bash
dotnet run --project SOLTEC.CodeAnalyzer -p <project_path> -o <output_path> [-c]
```

Use `-c` to enable console output.

## 📁 Documentation

Check the `/Documentation` folder for:
- Description and purpose
- Features and standards
- Advanced optional checks

---

## ✅ Main Validations

- Namespace structure and position
- XML documentation with examples
- Naming conventions
- Inheritance restrictions for public classes
