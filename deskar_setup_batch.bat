@echo off
setlocal enabledelayedexpansion

echo ===============================================
echo    DeskAR Unity SDK - Automatic Setup
echo ===============================================
echo.
echo This script will organize your DeskAR files into
echo the proper Unity project structure.
echo.

REM Check if we're in a Unity project
if not exist "Assets\" (
    echo ERROR: Please run this script from your Unity project root folder!
    echo The folder should contain an "Assets" directory.
    echo.
    echo Current directory: %CD%
    echo.
    pause
    exit /b 1
)

echo ✓ Found Unity project folder: %CD%
echo.

REM Check if DeskAR files exist in current directory
set "files_found=0"
if exist "deskar_effects_system.cs" set /a files_found+=1
if exist "deskar_example_scene.cs" set /a files_found+=1
if exist "deskar_setup_guide.txt" set /a files_found+=1
if exist "deskar_stereo_composite_shader.txt" set /a files_found+=1
if exist "deskar_ui_components.cs" set /a files_found+=1
if exist "holoar_anaglyph_shader.txt" set /a files_found+=1
if exist "holoar_holographic_shader.txt" set /a files_found+=1
if exist "holoar_sdk_plugin.cs" set /a files_found+=1

if %files_found% LSS 8 (
    echo ERROR: Not all DeskAR files found in current directory!
    echo.
    echo Please ensure these files are in the same folder as this script:
    echo - deskar_effects_system.cs
    echo - deskar_example_scene.cs
    echo - deskar_setup_guide.txt
    echo - deskar_stereo_composite_shader.txt
    echo - deskar_ui_components.cs
    echo - holoar_anaglyph_shader.txt
    echo - holoar_holographic_shader.txt
    echo - holoar_sdk_plugin.cs
    echo.
    echo Found %files_found% out of 8 required files.
    echo.
    pause
    exit /b 1
)

echo ✓ Found all DeskAR files
echo.
echo Creating folder structure...

REM Create main folder structure
mkdir "Assets\DeskAR" 2>nul
mkdir "Assets\DeskAR\Scripts" 2>nul
mkdir "Assets\DeskAR\Scripts\Core" 2>nul
mkdir "Assets\DeskAR\Scripts\Effects" 2>nul
mkdir "Assets\DeskAR\Scripts\UI" 2>nul
mkdir "Assets\DeskAR\Scripts\Examples" 2>nul
mkdir "Assets\DeskAR\Shaders" 2>nul
mkdir "Assets\DeskAR\Materials" 2>nul
mkdir "Assets\DeskAR\Prefabs" 2>nul
mkdir "Assets\DeskAR\Documentation" 2>nul

echo ✓ Folders created
echo.
echo Organizing and renaming files...

REM Copy and organize the main plugin file (contains core classes)
if exist "holoar_sdk_plugin.cs" (
    copy "holoar_sdk_plugin.cs" "Assets\DeskAR\Scripts\Core\DeskARManager.cs" >nul
    echo ✓ Core system → Assets\DeskAR\Scripts\Core\DeskARManager.cs
)

REM Copy effects system
if exist "deskar_effects_system.cs" (
    copy "deskar_effects_system.cs" "Assets\DeskAR\Scripts\Effects\DeskAREffects.cs" >nul
    echo ✓ Effects system → Assets\DeskAR\Scripts\Effects\DeskAREffects.cs
)

REM Copy UI components
if exist "deskar_ui_components.cs" (
    copy "deskar_ui_components.cs" "Assets\DeskAR\Scripts\UI\DeskARUI.cs" >nul
    echo ✓ UI components → Assets\DeskAR\Scripts\UI\DeskARUI.cs
)

REM Copy example scene
if exist "deskar_example_scene.cs" (
    copy "deskar_example_scene.cs" "Assets\DeskAR\Scripts\Examples\DeskARExampleScene.cs" >nul
    echo ✓ Example scene → Assets\DeskAR\Scripts\Examples\DeskARExampleScene.cs
)

REM Copy and rename shader files
if exist "holoar_anaglyph_shader.txt" (
    copy "holoar_anaglyph_shader.txt" "Assets\DeskAR\Shaders\DeskAR_Anaglyph.shader" >nul
    echo ✓ Anaglyph shader → Assets\DeskAR\Shaders\DeskAR_Anaglyph.shader
)

if exist "holoar_holographic_shader.txt" (
    copy "holoar_holographic_shader.txt" "Assets\DeskAR\Shaders\DeskAR_Holographic.shader" >nul
    echo ✓ Holographic shader → Assets\DeskAR\Shaders\DeskAR_Holographic.shader
)

if exist "deskar_stereo_composite_shader.txt" (
    copy "deskar_stereo_composite_shader.txt" "Assets\DeskAR\Shaders\DeskAR_StereoComposite.shader" >nul
    echo ✓ Stereo composite shader → Assets\DeskAR\Shaders\DeskAR_StereoComposite.shader
)

REM Copy documentation
if exist "deskar_setup_guide.txt" (
    copy "deskar_setup_guide.txt" "Assets\DeskAR\Documentation\DeskAR_Setup_Guide.txt" >nul
    echo ✓ Setup guide → Assets\DeskAR\Documentation\DeskAR_Setup_Guide.txt
)

echo.
echo Creating additional helper scripts...

REM Create Quick Setup Script
(
echo using UnityEngine;
echo using UnityEngine.UI;
echo using DeskAR;
echo using DeskAR.Effects;
echo.
echo /// ^<summary^>
echo /// Quick setup script for DeskAR - Attach this to an empty GameObject to automatically set up a DeskAR scene
echo /// Usage: Create empty GameObject → Add this script → Press Play
echo /// ^</summary^>
echo public class DeskARQuickSetup : MonoBehaviour
echo {
echo     [Header("Auto Setup Options"^)]
echo     public bool createDeskARManager = true;
echo     public bool createUI = true;
echo     public bool createExampleObjects = true;
echo     public bool setupLighting = true;
echo.    
echo     void Start(^)
echo     {
echo         Debug.Log("DeskAR Quick Setup: Starting automatic scene setup..."^);
echo         SetupDeskARScene(^);
echo     }
echo.    
echo     void SetupDeskARScene(^)
echo     {
echo         if (createDeskARManager^)
echo             CreateDeskARManager(^);
echo.        
echo         if (createUI^)
echo             CreateUISystem(^);
echo.        
echo         if (setupLighting^)
echo             SetupLighting(^);
echo.        
echo         if (createExampleObjects^)
echo             CreateExampleObjects(^);
echo.        
echo         Debug.Log("DeskAR Quick Setup: Scene setup complete! Put on your red-cyan 3D glasses and enjoy!"^);
echo     }
echo.    
echo     void CreateDeskARManager(^)
echo     {
echo         if (FindObjectOfType^<DeskARManager^>(^) != null^)
echo         {
echo             Debug.Log("DeskAR Manager already exists in scene"^);
echo             return;
echo         }
echo.        
echo         GameObject deskARObject = new GameObject("DeskAR Manager"^);
echo         DeskARManager manager = deskARObject.AddComponent^<DeskARManager^>(^);
echo.        
echo         manager.enableHeadTracking = true;
echo         manager.useMouseSimulation = true;
echo         manager.displayMode = DeskARDisplayMode.Anaglyph;
echo         manager.anaglyphType = DeskARAnaglyphType.RedCyan;
echo         manager.debugMode = true;
echo         manager.trackingSensitivity = 1.5f;
echo         manager.trackingSmoothing = 0.15f;
echo         manager.interpupillaryDistance = 0.064f;
echo         manager.convergenceDistance = 2.5f;
echo.        
echo         Debug.Log("✓ DeskAR Manager created and configured"^);
echo     }
echo.    
echo     void CreateUISystem(^)
echo     {
echo         Canvas mainCanvas = FindObjectOfType^<Canvas^>(^);
echo         if (mainCanvas == null^)
echo         {
echo             GameObject canvasObject = new GameObject("Main Canvas"^);
echo             mainCanvas = canvasObject.AddComponent^<Canvas^>(^);
echo             mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
echo             mainCanvas.sortingOrder = 100;
echo             canvasObject.AddComponent^<CanvasScaler^>(^);
echo             canvasObject.AddComponent^<GraphicRaycaster^>(^);
echo         }
echo.        
echo         CreateOutputDisplay(mainCanvas^);
echo         Debug.Log("✓ UI System created"^);
echo     }
echo.    
echo     void CreateOutputDisplay(Canvas canvas^)
echo     {
echo         GameObject displayObject = new GameObject("DeskAR Output Display"^);
echo         displayObject.transform.SetParent(canvas.transform, false^);
echo.        
echo         RawImage displayImage = displayObject.AddComponent^<RawImage^>(^);
echo         displayImage.color = Color.white;
echo.        
echo         RectTransform displayRect = displayImage.rectTransform;
echo         displayRect.anchorMin = Vector2.zero;
echo         displayRect.anchorMax = Vector2.one;
echo         displayRect.offsetMin = Vector2.zero;
echo         displayRect.offsetMax = Vector2.zero;
echo.        
echo         DeskARManager manager = FindObjectOfType^<DeskARManager^>(^);
echo         if (manager != null^)
echo         {
echo             manager.outputDisplay = displayImage;
echo         }
echo     }
echo.    
echo     void SetupLighting(^)
echo     {
echo         Light mainLight = FindObjectOfType^<Light^>(^);
echo         if (mainLight == null^)
echo         {
echo             GameObject lightObject = new GameObject("Main Light"^);
echo             mainLight = lightObject.AddComponent^<Light^>(^);
echo             lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f^);
echo         }
echo.        
echo         mainLight.type = LightType.Directional;
echo         mainLight.color = Color.white;
echo         mainLight.intensity = 1f;
echo.        
echo         Debug.Log("✓ Lighting configured"^);
echo     }
echo.    
echo     void CreateExampleObjects(^)
echo     {
echo         GameObject container = new GameObject("Holographic Objects"^);
echo         container.transform.position = new Vector3(0, 1, 3^);
echo.        
echo         GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube^);
echo         cube.name = "Holographic Cube";
echo         cube.transform.SetParent(container.transform^);
echo         cube.transform.localPosition = new Vector3(-1.5f, 0, 0^);
echo         cube.transform.localScale = Vector3.one * 0.8f;
echo.        
echo         try {
echo             var cubeConfig = new DeskARHolographicConfig
echo             {
echo                 color = Color.cyan,
echo                 intensity = 1.8f,
echo                 enableGlow = true,
echo                 glowColor = Color.cyan,
echo                 glowIntensity = 1.2f,
echo                 enableRotation = true,
echo                 rotationSpeed = new Vector3(0, 45, 15^),
echo                 enableInteraction = true
echo             };
echo.            
echo             DeskAREffects.CreateHolographicObject(cube, cubeConfig^);
echo         } catch {
echo             Debug.LogWarning("DeskAR Effects not available - basic cube created"^);
echo         }
echo.        
echo         Debug.Log("✓ Example holographic objects created"^);
echo     }
echo }
) > "Assets\DeskAR\Scripts\Examples\DeskARQuickSetup.cs"

echo ✓ Quick setup script → Assets\DeskAR\Scripts\Examples\DeskARQuickSetup.cs

REM Create README file
(
echo # DeskAR Unity SDK
echo.
echo ## Quick Start
echo 1. Open Unity and load your project
echo 2. In the Hierarchy, create an empty GameObject
echo 3. Add the 'DeskARQuickSetup' script to it
echo 4. Press Play
echo 5. Put on red-cyan 3D glasses
echo 6. Move your mouse to simulate head tracking
echo.
echo ## Controls
echo - Tab: Toggle control panel
echo - F1: Toggle head tracking
echo - F2: Start calibration
echo - F3-F6: Switch display modes
echo - Mouse: Simulate head movement
echo - Mouse Wheel: Adjust convergence
echo.
echo ## File Structure
echo - Core/: Main DeskAR system files
echo - Effects/: Holographic effect scripts
echo - UI/: User interface components
echo - Examples/: Demo scenes and scripts
echo - Shaders/: Visual effect shaders
echo.
echo ## Troubleshooting
echo 1. Ensure all scripts compile without errors
echo 2. Check that shaders are recognized by Unity
echo 3. Verify webcam permissions if using real head tracking
echo 4. Use mouse simulation mode for testing
echo.
echo ## Hardware Requirements
echo - Webcam (for head tracking^)
echo - Red-cyan anaglyph 3D glasses
echo - Unity 2021.3 LTS or newer
echo.
echo For detailed documentation, see DeskAR_Setup_Guide.txt
) > "Assets\DeskAR\README.md"

echo ✓ README file → Assets\DeskAR\README.md

REM Create scene template instructions
(
echo // DeskAR Scene Setup Instructions
echo // ================================
echo.
echo // Method 1: Quick Setup ^(Recommended^)
echo // 1. Create new scene
echo // 2. Create empty GameObject
echo // 3. Add DeskARQuickSetup script
echo // 4. Press Play
echo.
echo // Method 2: Manual Setup
echo // 1. Create empty GameObject named "DeskAR Manager"
echo // 2. Add DeskARManager script
echo // 3. Create UI Canvas
echo // 4. Add RawImage to Canvas ^(full screen^)
echo // 5. Assign RawImage to DeskARManager.outputDisplay
echo // 6. Create holographic objects using DeskAREffects
echo.
echo // Essential Components:
echo // - DeskARManager: Core system
echo // - Canvas + RawImage: Display output
echo // - Holographic objects: Using DeskAREffects
echo.
echo // Testing:
echo // - Enable "Use Mouse Simulation" for testing
echo // - Put on red-cyan 3D glasses to see effect
echo // - Move mouse to simulate head tracking
) > "Assets\DeskAR\Scripts\SceneSetupInstructions.txt"

echo ✓ Scene setup instructions → Assets\DeskAR\Scripts\SceneSetupInstructions.txt

echo.
echo ===============================================
echo           SETUP COMPLETE!
echo ===============================================
echo.
echo ✓ All DeskAR files have been organized
echo ✓ Folder structure created
echo ✓ Scripts placed in correct locations
echo ✓ Shaders ready for Unity
echo ✓ Documentation available
echo.
echo NEXT STEPS:
echo.
echo 1. Open Unity and wait for scripts to compile
echo 2. Create a new scene
echo 3. Create an empty GameObject
echo 4. Add the "DeskARQuickSetup" script to it
echo 5. Press Play
echo 6. Put on your red-cyan 3D glasses!
echo.
echo IMPORTANT NOTES:
echo - Scripts are in: Assets/DeskAR/Scripts/
echo - Shaders are in: Assets/DeskAR/Shaders/
echo - Documentation: Assets/DeskAR/Documentation/
echo - Use mouse simulation for testing
echo.
echo For detailed instructions, see:
echo Assets/DeskAR/README.md
echo.
echo Have fun creating holographic experiences!
echo.
pause