# RoR2 CamFreeModifiyer

![Menu Camera](https://raw.githubusercontent.com/jonathan-priebe/RoR2-CamFreeModifiyer/refs/heads/main/img/MenuCam.jpg)

A Risk of Rain 2 mod that lets you customize camera settings during gameplay.

## Features

- **Field of View (FOV)**: Adjust the camera's field of view (30° - 120°)
- **Camera Distance**: Change how far the camera is from your character (5 - 30)
- **Camera Pitch**: Tilt the camera vertically (-45° to +45°)
- **Camera Height**: Adjust the vertical offset of the camera (-5 to +10)
- **Camera Horizontal**: Shift the camera left or right (-10 to +10)
- **In-Game Menu**: Press F5 (configurable) to open the settings menu
- **Persistent Settings**: All settings are saved in the BepInEx config file

## Example

![Gameplay Camera](https://raw.githubusercontent.com/jonathan-priebe/RoR2-CamFreeModifiyer/refs/heads/main/img/GameplayCam.jpg)

## Requirements

- Risk of Rain 2
- BepInEx 5.x

## Installation

### For Users
1. Download the latest release
2. Copy `CamFreeModifiyer.dll` to your `BepInEx/plugins/` folder
3. Launch the game

### For Developers (Building from Source)

1. Clone this repository
2. Copy the following DLLs from your RoR2 installation to the `libs/` folder:

   **From `Risk of Rain 2/BepInEx/core/`:**
   - `BepInEx.dll`
   - `0Harmony.dll`

   **From `Risk of Rain 2/Risk of Rain 2_Data/Managed/`:**
   - `Assembly-CSharp.dll`
   - `RoR2.dll`
   - `UnityEngine.dll`
   - `UnityEngine.CoreModule.dll`
   - `UnityEngine.InputLegacyModule.dll`
   - `UnityEngine.IMGUIModule.dll`

3. Build the project:
   ```bash
   dotnet build -c Release
   ```

4. The compiled DLL will be in `bin/Release/netstandard2.1/`

## Usage

1. Launch Risk of Rain 2
2. Press **F5** to open the camera settings menu (key is configurable)
3. Adjust the sliders to your preference
4. Close the menu with the "Close" button or press F5 again

## Configuration

Settings are saved in `BepInEx/config/com.jpriebe.camfreemodifiyer.cfg`

You can edit this file manually or use the in-game menu.

## License

MIT License
