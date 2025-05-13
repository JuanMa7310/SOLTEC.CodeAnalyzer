# SOLTEC.CodeAnalyzer

**SOLTEC.CodeAnalyzer** es una utilidad de línea de comandos desarrollada en **C# .NET 8** utilizando las características del lenguaje **C# 12**. Analiza las clases C# dentro de un directorio de proyecto especificado para comprobar que cumplan con las normas internas de programación de SOLTEC.

## 🔍 Qué Hace

- Escanea todos los archivos `.cs` en el directorio indicado.
- Analiza cada clase para verificar:
  - Declaración y estructura correcta del espacio de nombres.
  - Presencia y formato de la documentación XML (incluyendo ejemplos de uso).
  - Convenciones de nomenclatura para variables y constantes.
  - Existencia de pruebas unitarias e integración para métodos con lógica.
- Registra todas las violaciones a las normas por clase.

## 🧾 Salida

- Genera un informe detallado en **formato Markdown** que incluye:
  - Archivos y clases que incumplen las normas.
  - Normas específicas que no se han cumplido.

## 🧑‍💻 Cómo Usarlo

```bash
dotnet run -- "C:\Ruta\Al\Proyecto" "C:\Ruta\Al\informe.md"
```

- **Primer argumento**: ruta del directorio del proyecto a analizar.
- **Segundo argumento**: ruta del archivo donde se guardará el informe Markdown generado.
