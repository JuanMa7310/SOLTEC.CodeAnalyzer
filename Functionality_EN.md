# SOLTEC.CodeAnalyzer - Features

## ✅ Command-Line Interface

- Supports CLI flags:
  - `-p <project_path>`: Specify the project directory to analyze.
  - `-o <output_path>`: Specify the Markdown file to write the report.
  - `-c`: (Optional) Print violations to the console in addition to the file.

## 📂 Code Parsing and Analysis

- Recursively searches for `.cs` files within the specified directory.
- Parses each file to detect:
  - Class, record, interface, enum, struct, and delegate declarations.
  - Public or protected methods and properties.

## 📏 Rule Enforcement

- Validates that namespaces start with `SOLTEC.` and follow the folder structure.
- Enforces XML documentation:
  - `<summary>` and `<example>` tags on public/protected types and methods.
  - `<summary>` tag on public/protected properties.
- Enforces naming conventions:
  - Local variables: `_x`
  - Class-level fields: `gX`
  - Constants: `gcX` (global), `_cX` (local)
  - Method parameters: lowercase initial
- Detects missing documentation or naming rule violations.

## 📑 Markdown Report Generation

- Generates a structured Markdown report with:
  - Timestamp of analysis
  - Summary of total violations
  - Violations grouped by file

## 🌐 Optional Console Output

- Using `-c`, violations are also printed to the console for real-time feedback.
