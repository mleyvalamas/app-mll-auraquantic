# 🚀 MLL-AQ-FileProcessor

[![.NET](https://img.shields.io/badge/.NET-10-blue)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14-purple)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

**MLL-AQ-FileProcessor** es una herramienta de consola de alto rendimiento desarrollada en .NET 10 y C# 14, diseñada como solución técnica para **AuraQuantic**. Permite el procesamiento eficiente de archivos de texto masivos, buscando y reemplazando cadenas de texto de forma segura y generando un reporte detallado de la operación según los requerimientos establecidos.

## 📑 Índice

1. [Características Principales](#-características-principales)
2. [Tecnologías Utilizadas](#-tecnologías-utilizadas)
3. [Instalación](#-instalación)
4. [Uso](#-uso)
5. [Arquitectura y Diseño](#-arquitectura-y-decisiones-de-diseño)
6. [Documentación Visual (Diagramas)](#-documentación-visual)
7. [Eficiencia y Escalabilidad](#-eficiencia-y-escalabilidad)
8. [Pruebas](#-pruebas)
9. [Notas sobre la Ejecución](#-notas-sobre-la-ejecución)
10. [Contribución](#-contribución)
11. [Licencia](#-licencia)
12. [Autor](#-autor)

## 🌟 Características Principales

- **🚀 Procesamiento por Flujo (Streams):** Optimizado para manejar archivos pesados (+50 MB, escalable a GB) con un consumo de memoria constante y mínimo.
- **🏗️ Arquitectura Modular:** Implementación basada en principios SOLID y desacoplamiento mediante interfaces para facilitar el mantenimiento.
- **🛡️ Programación Defensiva:** Manejo exhaustivo de excepciones (I/O, permisos, rutas) y validación estricta de argumentos de entrada.
- **🧪 Calidad de Ingeniería:** Suite de pruebas unitarias implementada con xUnit y NSubstitute para garantizar la fiabilidad del motor de procesamiento.

## 🛠️ Tecnologías Utilizadas

- **Lenguaje:** C# 14
- **Framework:** .NET 10
- **Pruebas:** xUnit, NSubstitute
- **Herramientas:** Visual Studio, .NET CLI

## 📦 Instalación

### Requisitos Previos

- .NET 10 SDK instalado. Puedes descargarlo desde [dotnet.microsoft.com](https://dotnet.microsoft.com/download).

### Compilación

Para restaurar las dependencias y compilar los binarios de la solución:

```bash
dotnet build
```

### Publicación para Producción

Genera un archivo ejecutable optimizado que no depende del SDK de .NET:

```bash
dotnet publish MllAqFileProcessor.App/MllAqFileProcessor.App.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

## 🚀 Uso

El programa recibe exactamente 4 parámetros en el siguiente orden: `<origen>` `<destino>` `<texto_buscar>` `<texto_reemplazo>`.

### Ejemplo en Desarrollo

```bash
dotnet run --project MllAqFileProcessor.App/MllAqFileProcessor.App.csproj "Data/origen.txt" "Data/destino.txt" "auraportal" "ap"
```

### Ejemplo en Producción (Windows)

```cmd
MllAqFileProcessor.App.exe "Data/origen.txt" "Data/destino.txt" "auraportal" "ap"
```

### Salida

El programa imprime el número total de reemplazos realizados:

```
Se han hecho 5 reemplazos
```

## 🏗️ Arquitectura y Diseño

### Arquitectura de Servicios Pragmática

Se ha implementado una arquitectura dividida por responsabilidades claras para evitar la dispersión del código y cumplir con el requisito de no centralizar la lógica en `Program.cs`:

- **Presentación (`Program.cs`):** Gestiona el ciclo de vida, la captura estricta de los 4 argumentos y la interfaz de usuario por consola.
- **Orquestación (`FileProcessorEngine`):** Coordina los servicios de infraestructura sin acoplarse a implementaciones concretas.
- **Servicios de Dominio:**
  - `FileService`: Abstrae el acceso al disco, permitiendo la manipulación de flujos de datos.
  - `TextProcessorService`: Encargado de la lógica algorítmica de búsqueda y reemplazo.

> **💡 ¿Por qué no se utilizó Clean Architecture?**  
> Como decisión de ingeniería, no aplicar Clean Architecture se basa en criterios de eficiencia técnica y operativa. Para una herramienta de consola de propósito específico, crear múltiples proyectos y capas de transporte (DTOs/Mappers) añadiría sobreingeniería innecesaria que no aporta valor al negocio en este contexto. Se priorizó el principio KISS (Keep It Simple, Stupid) y el rendimiento I/O. La solución es "Clean-Ready": gracias al uso de interfaces, puede escalarse a una arquitectura hexagonal si el dominio creciera.

## 📊 Documentación Visual

*(Nota: Los siguientes diagramas se renderizarán automáticamente al visualizar este archivo en GitHub).*

### Diagrama de Clases (Cumplimiento SOLID)

```mermaid
classDiagram
    class Program {
        +Main(args: string[]) int
    }
    class FileProcessorEngine {
        -IFileService _fileService
        -ITextProcessorService _textProcessor
        +Run(source, target, search, replace) int
    }
    class IFileService {
        <<interface>>
        +Exists(path) bool
        +OpenReader(path) StreamReader
        +OpenWriter(path) StreamWriter
    }
    class ITextProcessorService {
        <<interface>>
        +ProcessLine(line, search, replace) ProcessResult
    }
    class FileService {
        +Implementación I/O
    }
    class TextProcessorService {
        +Implementación Regex / String
    }

    Program --> FileProcessorEngine : Instancia y ejecuta
    FileProcessorEngine ..> IFileService : Inyección de Dependencia
    FileProcessorEngine ..> ITextProcessorService : Inyección de Dependencia
    FileService ..|> IFileService : Implementa
    TextProcessorService ..|> ITextProcessorService : Implementa
```

### Diagrama de Secuencia (Flujo de Memoria O(1))

```mermaid
sequenceDiagram
    participant App as FileProcessorEngine
    participant SRC as Stream Origen (Disco)
    participant PROC as TextProcessorService
    participant DST as Stream Destino (Disco)

    App->>SRC: Abre flujo de lectura (StreamReader)
    App->>DST: Abre flujo de escritura (StreamWriter)

    loop Por cada línea del archivo
        SRC->>App: Lee línea hacia el Buffer en RAM
        App->>PROC: ProcessLine(linea, buscar, reemplazar)
        PROC-->>App: Retorna (LineaProcesada, Conteo)
        App->>DST: Escribe línea procesada en disco
    end

    App->>SRC: Cierra y libera (Dispose)
    App->>DST: Cierra y libera (Dispose)
    App-->>App: Retorna total de reemplazos al Main
```

## ⚡ Eficiencia y Escalabilidad

A diferencia de enfoques básicos que utilizan `File.ReadAllText`, este programa emplea `StreamReader` y `StreamWriter` para la lectura secuencial.

**Dato técnico de alto nivel:** Esto garantiza que la complejidad espacial sea O(1) respecto al tamaño del archivo. El programa procesará un archivo de 10 GB con la misma huella de memoria RAM que uno de 1 KB, evitando de forma absoluta los errores de `OutOfMemoryException`.

## 🧪 Pruebas

El `TextProcessorService` es sometido a pruebas de reemplazo simple, múltiple, sensibilidad a mayúsculas y manejo de cadenas vacías.

Para ejecutar las pruebas unitarias:

```bash
dotnet test
```

## ⚠️ Notas sobre la Ejecución

### Interpretación de Argumentos en el Shell

La aplicación ha sido diseñada para ejecutarse desde línea de comandos utilizando exactamente el formato de parámetros especificado:

```bash
dotnet run "Data/origen.txt" "Data/destino.txt" "auraportal" "P2"
```

La aplicación recibe cuatro parámetros posicionales en el siguiente orden:

| Parámetro | Descripción          |
|-----------|----------------------|
| arg0      | Ruta del archivo origen |
| arg1      | Ruta del archivo destino |
| arg2      | Texto a buscar      |
| arg3      | Texto de reemplazo  |

Estos valores son recibidos directamente en el punto de entrada del programa:

```csharp
static int Main(string[] args)
```

### Comillas no Balanceadas (Limitación del Shell)

Si una comilla se abre pero no se cierra correctamente, el Shell bloquea la ejecución esperando el cierre. Este comportamiento pertenece al nivel del Shell y no puede ser gestionado desde el código de la aplicación.

### Alcance de Validación

La aplicación valida:
- Número de argumentos recibidos
- Existencia del archivo origen
- Accesibilidad de rutas
- Permisos de lectura y escritura
- Manejo robusto de excepciones de I/O

### 💡 Posible Variante para Reducir la Dependencia de la Sintaxis del Shell (Modo Interactivo)

En escenarios de producción donde se desee reducir la dependencia de la sintaxis del Shell, podría implementarse un modo interactivo opcional que solicite los parámetros directamente al usuario.

Ejemplo conceptual:

```csharp
Console.Write("Ruta archivo origen: ");
string source = Console.ReadLine();

Console.Write("Ruta archivo destino: ");
string target = Console.ReadLine();

Console.Write("Texto a buscar: ");
string search = Console.ReadLine();

Console.Write("Texto de reemplazo: ");
string replace = Console.ReadLine();
```

Este enfoque permitiría:
- Evitar errores de sintaxis en la línea de comandos
- Simplificar el uso para usuarios no técnicos
- Realizar validaciones progresivas antes de iniciar el procesamiento

No obstante, para este ejercicio se ha mantenido el formato CLI especificado para asegurar el cumplimiento exacto del requerimiento técnico.

## 📌 Conclusión

La aplicación sigue el modelo estándar de herramientas CLI del ecosistema de .NET utilizando C#.

La limitación relacionada con comillas sin cerrar forma parte del comportamiento normal del intérprete de comandos del sistema operativo y no puede ser gestionada desde el código de la aplicación.

A pesar de esta limitación inherente al entorno de ejecución, la solución implementa validaciones robustas y manejo exhaustivo de errores para garantizar un comportamiento seguro y predecible.

## 🤝 Contribución

¡Las contribuciones son bienvenidas! Por favor, sigue estos pasos:

1. Haz un fork del proyecto.
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`).
3. Commit tus cambios (`git commit -am 'Agrega nueva funcionalidad'`).
4. Push a la rama (`git push origin feature/nueva-funcionalidad`).
5. Abre un Pull Request.

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

## 👨‍💻 Autor

**Manuel Leyva Lamas**  
Ingeniero de Software  
[GitHub](https://github.com/mleyvalamas)
