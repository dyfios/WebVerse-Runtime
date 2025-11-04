# WebVerse-Runtime Examples

This directory contains example scripts and verification tools for WebVerse-Runtime features.

## Input Control Flags Verification

The `input-control-flags-verification.js` script provides automated verification of the Input Control Flags JavaScript API.

### Purpose

This script tests all input control flags including:
- **VR Control Flags**: joystick motion, grab movement, interaction, pointers, poker, turn locomotion
- **Desktop Control Flags**: gravity, WASD motion, mouse look, jump, movement speed, look speed
- **Avatar Configuration**: SetAvatarEntityByTag and SetRigOffset methods
- **Practical Use Cases**: Flight mode toggle example

### How to Run

#### Method 1: From VEML Document

Include the script in your VEML document's metadata section:

```xml
<veml>
  <metadata>
    <title>Input Control Flags Test</title>
    <script>docs/examples/input-control-flags-verification.js</script>
  </metadata>
  <!-- ... rest of VEML document ... -->
</veml>
```

#### Method 2: JavaScript Console

If WebVerse-Runtime has a JavaScript console, you can copy and paste the contents of `input-control-flags-verification.js` directly into the console.

#### Method 3: Inline Script

Add the script content directly in your VEML document:

```xml
<veml>
  <metadata>
    <title>Input Control Flags Test</title>
    <script>
      // Paste the contents of input-control-flags-verification.js here
      Logging.Log("Starting Input Control Flags Verification...");
      // ... rest of the script ...
    </script>
  </metadata>
  <!-- ... rest of VEML document ... -->
</veml>
```

### Expected Output

The script will output test results to the logging system. Look for messages like:

```
Starting Input Control Flags Verification...
Test 1: VR Control Flags
  joystickMotionEnabled: PASS
  leftGrabMoveEnabled: PASS
  rightGrabMoveEnabled: PASS
Test 1: COMPLETED
Test 2: Desktop Control Flags
  gravityEnabled: PASS
  wasdMotionEnabled: PASS
  mouseLookEnabled: PASS
  jumpEnabled: PASS
Test 2: COMPLETED
Test 3: Movement Speed and Look Speed
  movementSpeed: PASS
  lookSpeed: PASS
Test 3: COMPLETED
Test 4: Avatar Entity and Rig Offset Methods
  SetAvatarEntityByTag (non-existent): PASS (correctly returned false)
  SetRigOffset: PASS (method callable)
Test 4: COMPLETED
Test 5: Practical Use Case - Toggle Flight Mode
  Flight mode enabled
  Flight mode toggle: PASS
  Walking mode enabled
  Walking mode toggle: PASS
Test 5: COMPLETED
Input Control Flags Verification Complete!
```

### Notes

- **WARN messages** are expected if the desktop rig or VR rig are not available in the current context
- Some tests may show "no change detected" if the rig is not properly initialized
- The script automatically restores original values after each test to avoid side effects

### Interpreting Results

- **PASS**: The feature works as expected
- **WARN**: The feature may not be available in the current context (e.g., VR flags when running in desktop mode)
- **FAIL**: An unexpected error occurred; check the error message for details

## Other Examples

See `basic-usage.md` for more general WebVerse-Runtime examples and usage patterns.

## Contributing

When adding new examples:
1. Create descriptive filenames (e.g., `feature-name-example.js`)
2. Include comments explaining what the example demonstrates
3. Update this README with information about your example
4. Test the example in a real WebVerse-Runtime environment before committing
