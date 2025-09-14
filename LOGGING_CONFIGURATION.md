# Logging Configuration System

The WebVerse Runtime now includes a flexible logging configuration system that allows you to control logging behavior at runtime.

## Features

- **Message Type Filtering**: Enable or disable specific log message types (Default, Debug, Warning, Error, ScriptDefault, ScriptDebug, ScriptWarning, ScriptError)
- **Console Output Control**: Enable or disable logging to the Unity console
- **Runtime Configuration**: Apply logging settings during runtime initialization
- **Environment-Specific Presets**: Use default or production configurations

## Usage

### Basic Configuration

```csharp
using FiveSQD.WebVerse.Utilities;

// Create a custom logging configuration
var loggingConfig = new LoggingConfiguration
{
    enableConsoleOutput = true,  // Enable/disable Unity console output
    enableDefault = true,        // Enable default log messages
    enableDebug = false,         // Disable debug messages
    enableWarning = true,        // Enable warning messages
    enableError = true,          // Enable error messages
    enableScriptDefault = true,  // Enable script default messages
    enableScriptDebug = false,   // Disable script debug messages
    enableScriptWarning = true,  // Enable script warning messages
    enableScriptError = true     // Enable script error messages
};

// Apply the configuration
Logging.SetConfiguration(loggingConfig);
```

### Preset Configurations

```csharp
// Use default configuration (all logging enabled)
var defaultConfig = LoggingConfiguration.CreateDefault();
Logging.SetConfiguration(defaultConfig);

// Use production configuration (limited logging)
var productionConfig = LoggingConfiguration.CreateProduction();
Logging.SetConfiguration(productionConfig);
```

### WebVerse Runtime Integration

#### DesktopMode
In the Unity Editor, you can configure logging through the inspector:
- `testLoggingEnableConsoleOutput`: Enable/disable console output
- `testLoggingEnableDefault`: Enable/disable default messages
- `testLoggingEnableDebug`: Enable/disable debug messages
- `testLoggingEnableWarning`: Enable/disable warning messages
- `testLoggingEnableError`: Enable/disable error messages

#### WebMode
For WebGL builds, you can configure logging through URL parameters:
- `logging_console_output=true/false`: Control console output
- `logging_enable_default=true/false`: Control default messages
- `logging_enable_debug=true/false`: Control debug messages
- `logging_enable_warning=true/false`: Control warning messages
- `logging_enable_error=true/false`: Control error messages

Example URL:
```
https://yoursite.com/webverse?world_url=example.veml&logging_console_output=false&logging_enable_debug=false
```

#### Direct Runtime Settings
```csharp
var settings = new WebVerseRuntime.RuntimeSettings
{
    storageMode = "cache",
    maxEntries = 1024,
    maxEntryLength = 8192,
    maxKeyLength = 512,
    filesDirectory = "/path/to/files",
    timeout = 120,
    loggingConfiguration = LoggingConfiguration.CreateProduction()
};

runtime.Initialize(settings);
```

## API Reference

### LoggingConfiguration

| Property | Type | Description |
|----------|------|-------------|
| `enableConsoleOutput` | bool | Enable/disable Unity console output |
| `enableDefault` | bool | Enable/disable Default type messages |
| `enableDebug` | bool | Enable/disable Debug type messages |
| `enableWarning` | bool | Enable/disable Warning type messages |
| `enableError` | bool | Enable/disable Error type messages |
| `enableScriptDefault` | bool | Enable/disable ScriptDefault type messages |
| `enableScriptDebug` | bool | Enable/disable ScriptDebug type messages |
| `enableScriptWarning` | bool | Enable/disable ScriptWarning type messages |
| `enableScriptError` | bool | Enable/disable ScriptError type messages |

### Logging Class Methods

| Method | Description |
|--------|-------------|
| `SetConfiguration(LoggingConfiguration config)` | Apply a logging configuration |
| `GetConfiguration()` | Get the current logging configuration |
| `IsLogTypeEnabled(Logging.Type type)` | Check if a log type is enabled |

## Backward Compatibility

The logging system maintains full backward compatibility. If no logging configuration is provided:
- DesktopMode uses default configuration in editor, production configuration in builds
- WebMode uses default configuration in editor, URL parameters or default in WebGL builds
- Direct runtime initialization uses default configuration if no logging config is specified