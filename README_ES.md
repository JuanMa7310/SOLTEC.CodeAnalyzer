# SOLTEC.CodeAnalyzer

**SOLTEC.CodeAnalyzer** es una utilidad profesional de consola en C# (.NET 8, C# 12) que analiza archivos fuente C# para verificar que cumplan con los estándares de programación de SOLTEC.

## 🚀 Inicio Rápido

Puedes utilizar los scripts incluidos para ejecutar el analizador de forma interactiva:

- **Windows:** `run-analyzer.bat`
- **Linux/macOS:** `run-analyzer.sh`

Cada script te pedirá:
- Ruta del proyecto a analizar
- Ruta del informe Markdown de salida
- Si deseas imprimir también los resultados en consola

## 🔧 Uso Manual por Línea de Comandos

```bash
dotnet run --project SOLTEC.CodeAnalyzer -p <ruta_proyecto> -o <ruta_salida> [-c]
```

Usa `-c` para mostrar resultados en consola.

## 📁 Documentación

Consulta la carpeta `/Documentation` para:
- Descripción general y propósito
- Funcionalidades y normas analizadas
- Comprobaciones avanzadas opcionales

---

## ✅ Validaciones Principales

- Estructura y posición del namespace
- Documentación XML con ejemplos
- Convenciones de nombres
- Restricciones de herencia para clases públicas
