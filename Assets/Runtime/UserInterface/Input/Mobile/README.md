# Mobile Touch Input Support

This module provides smartphone and tablet touch input support for WebVerse-Runtime, following the established patterns used in the Desktop and SteamVR input implementations.

## Features

- **Single Touch Gestures**
  - Tap: Equivalent to mouse left-click
  - Touch and drag: Camera look movement (mouse delta)
  - Touch position tracking for UI interactions and pointer raycasting

- **Multi-Touch Gestures**
  - Pinch-to-zoom: Equivalent to mouse scroll wheel for zoom operations
  - Dual touch support for advanced gesture recognition

- **Configurable Settings**
  - Touch sensitivity adjustment
  - Drag threshold configuration
  - Tap detection timing and distance thresholds
  - Enable/disable individual touch features

## Implementation

### Core Components

1. **MobileInput.cs** - Main input handler extending `BasePlatformInput`
   - Handles touch events from Unity's Input System
   - Converts touch gestures to existing input manager events
   - Provides pointer raycasting for touch positions

2. **Mobile.inputactions** - Unity Input System action mappings
   - Primary touch (press, position, delta)
   - Secondary touch (press, position)
   - Touch count tracking

3. **MobileInputTests.cs** - Unit tests for mobile input functionality

### Integration

The mobile input system integrates seamlessly with the existing `InputManager` through the `BasePlatformInput` interface. The `InputManager` automatically handles input events from any platform implementation, including:

- Desktop (keyboard + mouse)
- SteamVR (VR controllers) 
- Mobile (touch screen) - **NEW**

### Usage

The mobile input is designed to work automatically when running on mobile devices. The `InputManager` will use the assigned `platformInput` component, which can be set to `MobileInput` for mobile builds.

#### Touch Gesture Mappings

| Touch Gesture | Input Manager Event | Equivalent Desktop Input |
|---------------|-------------------|-------------------------|
| Tap | `Left()` / `EndLeft()` | Left mouse click |
| Touch drag | `Look(Vector2)` | Mouse movement |
| Pinch out/in | `Middle()` | Mouse scroll wheel |
| Touch position | `GetPointerRaycast()` | Mouse position |

#### Configuration Options

```csharp
MobileInput mobileInput = GetComponent<MobileInput>();

// Enable/disable touch features
mobileInput.touchInputEnabled = true;
mobileInput.touchMovementEnabled = true; 
mobileInput.touchLookEnabled = true;
mobileInput.pinchZoomEnabled = true;

// Sensitivity and threshold settings
mobileInput.touchSensitivity = 1.0f;           // Look sensitivity multiplier
mobileInput.touchDragThreshold = 10.0f;        // Minimum drag distance (pixels)
mobileInput.tapTimeThreshold = 0.3f;           // Maximum tap duration (seconds)
mobileInput.tapDistanceThreshold = 50.0f;      // Maximum tap distance (pixels)
```

## Testing

Unit tests are provided in `MobileInputTests.cs` to verify:
- Component initialization and configuration
- Property getters/setters
- Inheritance from `BasePlatformInput`
- Basic input validation

Run tests through Unity Test Runner or CI/CD pipeline.

## File Structure

```
Assets/Runtime/UserInterface/Input/Mobile/
├── Scripts/
│   └── MobileInput.cs
├── Assets/
│   └── Mobile.inputactions
└── Tests/
    ├── MobileInputTests.cs
    └── FiveSQD.WebVerse.Input.Mobile.Tests.asmdef
```

## Compatibility

- Unity 2021.3.26f1+
- Unity Input System package
- Compatible with WebGL and mobile platform builds
- Follows WebVerse-Runtime architecture patterns