
# Command-Line Parameter Usage - SOLTEC.CodeAnalyzer

## 🧾 Available Parameters

| Parameter      | Alias         | Description                                      | Required |
|----------------|---------------|--------------------------------------------------|----------|
| `-p`           | `--project`   | Path to the C# project or solution to analyze.   | Yes      |
| `-o`           |               | Path to save the Markdown summary output.        | Yes      |
| `-c`           |               | If present, prints the summary to the console.   | No       |

---

## 📌 Usage Example

```bash
dotnet run -- -p "C:\Projects\MyApp" -o "C:\Reports\summary.md" -c
```

This will:
- Analyze the project at `C:\Projects\MyApp`.
- Save the Markdown report to `C:\Reports\summary.md`.
- Print the result to the console.

---

## ⚠️ Common Errors

| Error                      | Cause                                                    |
|---------------------------|-----------------------------------------------------------|
| `Missing required -p`     | You didn't provide a path to the project.                 |
| `Missing required -o`     | You didn't specify where to save the report.              |
| `Unknown argument`        | You used an unsupported or misspelled parameter.          |
| `Path not found`          | The given path does not exist or is inaccessible.         |

---

## 💬 Tip
You can always use `--project` instead of `-p` for clarity.

