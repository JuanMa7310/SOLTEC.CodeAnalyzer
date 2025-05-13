# SOLTEC.CodeAnalyzer - Funcionalidades

## ✅ Interfaz de Línea de Comandos

- Soporta parámetros con indicador:
  - `-p <ruta_proyecto>`: Directorio del proyecto a analizar.
  - `-o <ruta_salida>`: Archivo Markdown donde guardar el informe.
  - `-c`: (Opcional) Imprime las violaciones también en consola.

## 📂 Análisis de Código

- Busca de forma recursiva archivos `.cs` en el directorio especificado.
- Detecta en cada archivo:
  - Clases, registros, interfaces, enumerados, estructuras y delegados.
  - Métodos y propiedades públicas o protegidas.

## 📏 Validación de Reglas

- Verifica que los espacios de nombres comiencen con `SOLTEC.` y coincidan con la estructura de carpetas.
- Requiere documentación XML:
  - Etiquetas `<summary>` y `<example>` en tipos y métodos públicos/protegidos.
  - Etiqueta `<summary>` en propiedades públicas/protegidas.
- Reglas de nombres:
  - Variables locales: `_x`
  - Campos a nivel de clase: `gX`
  - Constantes: `gcX` (global), `_cX` (local)
  - Parámetros: inicial minúscula
- Detecta faltas de documentación o nombres inválidos.

## 📑 Generación de Informe Markdown

- Crea un informe estructurado con:
  - Fecha y hora del análisis
  - Resumen de violaciones
  - Violaciones agrupadas por archivo

## 🌐 Salida Opcional por Consola

- Con `-c`, las violaciones también se imprimen en consola para retroalimentación en tiempo real.
