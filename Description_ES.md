# SOLTEC.CodeAnalyzer - Descripción General

**SOLTEC.CodeAnalyzer** es una herramienta de consola de nivel profesional desarrollada en C# (.NET 8, C# 12) que realiza análisis estático de archivos fuente C#. Verifica el cumplimiento de las estrictas normas internas de programación de SOLTEC.

## 🔍 Qué Hace

- Analiza todos los archivos `.cs` en un directorio de proyecto.
- Verifica que cada tipo (class, interface, record, enum, struct, delegate) cumpla con:
  - Reglas de espacio de nombres (inicia con `SOLTEC.` y coincide con la estructura de carpetas).
  - Documentación XML (`<summary>` y `<example>`) para tipos y miembros públicos/protegidos.
  - Convenciones de nombres para variables, constantes y parámetros.
- Soporta salida en archivo Markdown y opcionalmente en consola.
- Parámetros con indicadores profesionales:
  - `-p`: Ruta al proyecto a analizar.
  - `-o`: Ruta del archivo Markdown de salida.
  - `-c`: (Opcional) Muestra las violaciones también en consola.

## 🧪 Tipos Soportados

- Clases
- Registros (records)
- Interfaces
- Enumerados
- Estructuras (structs)
- Delegados (delegates)

## 📤 Salida

- Informe Markdown con:
  - Resumen de archivos analizados
  - Lista de violaciones por archivo y tipo
- Salida opcional en consola con el parámetro `-c`
