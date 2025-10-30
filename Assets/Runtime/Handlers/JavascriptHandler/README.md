# WebVerse Runtime TypeScript Definitions

This directory contains TypeScript type definitions for the WebVerse Runtime JavaScript APIs.

## Overview

The `webverse-runtime.d.ts` file provides comprehensive TypeScript type definitions for all APIs exposed by the WebVerse Runtime's JavascriptHandler. These APIs are available globally when scripts are executed within the WebVerse environment.

## Usage

### In a TypeScript Project

To use these type definitions in your TypeScript project:

1. Copy the `webverse-runtime.d.ts` file to your project's type definitions directory (e.g., `types/` or `@types/`)

2. Reference the types in your TypeScript code:

```typescript
// Types are available globally, no import needed

// Example: Using Vector3
const position = new Vector3(0, 10, 0);

// Example: Using Entity API
const myEntity = Entity.Get("entity-id-here");
if (myEntity) {
    myEntity.SetPosition(position);
}

// Example: Using Logging
Logging.Log("Hello from TypeScript!", LoggingType.Default);

// Example: Using HTTPNetworking
HTTPNetworking.Fetch(
    "https://api.example.com/data",
    { method: "GET" },
    "onFetchComplete",
    "onFetchError"
);
```

3. Add a reference to the type definitions in your `tsconfig.json`:

```json
{
  "compilerOptions": {
    "typeRoots": ["./types", "./node_modules/@types"]
  },
  "include": ["src/**/*", "types/**/*"]
}
```

### In a JavaScript Project with JSDoc

You can also use these type definitions in JavaScript projects with JSDoc:

```javascript
/// <reference path="./webverse-runtime.d.ts" />

/**
 * @param {Vector3} position
 */
function moveEntity(position) {
    // Your code with type checking
}
```

## API Categories

The type definitions cover the following API categories:

### World Types
- **Vector Types**: `Vector2`, `Vector3`, `Vector4`, `Vector2D`, `Vector3D`, `Vector4D`, `Vector2Int`, `Vector3Int`, `Vector4Int`
- **Rotation Types**: `Quaternion`, `QuaternionD`
- **Other Types**: `Color`, `UUID`, `RaycastHitInfo`

### Entity Types
- **Base Entity**: `Entity`, `BaseEntity`
- **Specific Entities**: `AirplaneEntity`, `AudioEntity`, `AutomobileEntity`, `ButtonEntity`, `CanvasEntity`, `CharacterEntity`, `ContainerEntity`, `HTMLEntity`, `ImageEntity`, `InputEntity`, `LightEntity`, `MeshEntity`, `TerrainEntity`, `TextEntity`, `VoxelEntity`, `WaterEntity`, `WaterBlockerEntity`
- **Supporting Types**: `EntityMotion`, `EntityPhysicalProperties`, `LightProperties`, `LightType`, `InteractionState`, `TextAlignment`, `TextWrapping`, `UIElementAlignment`, etc.

### Networking APIs
- **HTTPNetworking**: Fetch API for HTTP requests
- **WebSocket**: WebSocket client (when USE_WEBINTERFACE is defined)
- **MQTTClient**: MQTT client (when USE_WEBINTERFACE is defined)

### Input API
- **Input**: Keyboard, mouse, and VR input handling

### Environment API
- **Environment**: World environment manipulation (sky, offset, tracking)

### Data API
- **AsyncJSON**: Asynchronous JSON parsing and stringification

### VOS Synchronization API
- **VOSSynchronization**: VOS session management (when USE_WEBINTERFACE is defined)

### World Browser Utilities
- **Camera**: Camera manipulation and control
- **Context**: Context management
- **Date**: Date and time operations
- **LocalStorage**: Local storage operations
- **Logging**: Logging operations with different log levels
- **Scripting**: Script execution
- **Time**: Time utilities and interval management
- **World**: World loading and query parameters
- **WorldStorage**: World-specific storage operations

## API Documentation

For detailed API documentation, please refer to:
- [World APIs Documentation](https://five-squared-interactive.github.io/World-APIs/)
- [VEML Documentation](https://github.com/Five-Squared-Interactive/VEML/wiki/Document-Structure)

## Source

These type definitions are generated from the C# API implementations located in:
- `Assets/Runtime/Handlers/JavascriptHandler/Scripts/JavascriptHandler.cs` (API registration)
- `Assets/Runtime/Handlers/JavascriptHandler/APIs/` (API implementations)

## Contributing

If you find any discrepancies between the type definitions and the actual API behavior, please open an issue or submit a pull request.

## License

These type definitions are provided under the same license as the WebVerse Runtime project. See the main LICENSE file for details.
