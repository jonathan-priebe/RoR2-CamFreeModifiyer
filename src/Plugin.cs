using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace CamFreeModifiyer
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jpriebe.camfreemodifiyer";
        public const string PluginName = "CamFreeModifiyer";
        public const string PluginVersion = "1.0.0";

        // Config entries
        public static ConfigEntry<float> FieldOfView;
        public static ConfigEntry<float> CameraDistance;
        public static ConfigEntry<float> CameraPitch;
        public static ConfigEntry<float> CameraHeight;
        public static ConfigEntry<float> CameraHorizontal;
        public static ConfigEntry<KeyCode> ToggleMenuKey;

        // Instance reference
        public static Plugin Instance { get; private set; }

        // UI state
        private bool showUI = false;
        private Rect windowRect = new Rect(20, 20, 300, 320);

        private Harmony harmony;

        private void Awake()
        {
            Instance = this;

            // Initialize configuration
            InitializeConfig();

            // Apply Harmony patches
            harmony = new Harmony(PluginGUID);
            harmony.PatchAll();

            Logger.LogInfo($"{PluginName} v{PluginVersion} loaded!");
        }

        private void InitializeConfig()
        {
            FieldOfView = Config.Bind(
                "Camera Settings",
                "Field of View",
                60f,
                new ConfigDescription(
                    "Camera field of view in degrees",
                    new AcceptableValueRange<float>(30f, 120f)
                )
            );

            CameraDistance = Config.Bind(
                "Camera Settings",
                "Camera Distance",
                10f,
                new ConfigDescription(
                    "Distance of the camera from the player",
                    new AcceptableValueRange<float>(5f, 30f)
                )
            );

            CameraPitch = Config.Bind(
                "Camera Settings",
                "Camera Pitch",
                0f,
                new ConfigDescription(
                    "Vertical angle/tilt of the camera in degrees",
                    new AcceptableValueRange<float>(-45f, 45f)
                )
            );

            CameraHeight = Config.Bind(
                "Camera Settings",
                "Camera Height",
                0f,
                new ConfigDescription(
                    "Height offset of the camera",
                    new AcceptableValueRange<float>(-5f, 10f)
                )
            );

            CameraHorizontal = Config.Bind(
                "Camera Settings",
                "Camera Horizontal",
                0f,
                new ConfigDescription(
                    "Horizontal offset of the camera (negative = left, positive = right)",
                    new AcceptableValueRange<float>(-10f, 10f)
                )
            );

            ToggleMenuKey = Config.Bind(
                "Controls",
                "Toggle Menu Key",
                KeyCode.F5,
                "Key to toggle the in-game camera settings menu"
            );
        }

        private void Update()
        {
            // Toggle UI with configured key
            if (Input.GetKeyDown(ToggleMenuKey.Value))
            {
                showUI = !showUI;
            }
        }

        private void OnGUI()
        {
            if (!showUI) return;

            windowRect = GUI.Window(0, windowRect, DrawSettingsWindow, "CamFreeModifiyer Settings");
        }

        private void DrawSettingsWindow(int windowId)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10);

            // Field of View slider
            GUILayout.Label($"Field of View: {FieldOfView.Value:F1}°");
            FieldOfView.Value = GUILayout.HorizontalSlider(FieldOfView.Value, 30f, 120f);
            GUILayout.Space(10);

            // Camera Distance slider
            GUILayout.Label($"Camera Distance: {CameraDistance.Value:F1}");
            CameraDistance.Value = GUILayout.HorizontalSlider(CameraDistance.Value, 5f, 30f);
            GUILayout.Space(10);

            // Camera Pitch slider
            GUILayout.Label($"Camera Pitch: {CameraPitch.Value:F1}°");
            CameraPitch.Value = GUILayout.HorizontalSlider(CameraPitch.Value, -45f, 45f);
            GUILayout.Space(10);

            // Camera Height slider
            GUILayout.Label($"Camera Height: {CameraHeight.Value:F1}");
            CameraHeight.Value = GUILayout.HorizontalSlider(CameraHeight.Value, -5f, 10f);
            GUILayout.Space(10);

            // Camera Horizontal slider
            GUILayout.Label($"Camera Horizontal: {CameraHorizontal.Value:F1}");
            CameraHorizontal.Value = GUILayout.HorizontalSlider(CameraHorizontal.Value, -10f, 10f);
            GUILayout.Space(10);

            // Reset button
            if (GUILayout.Button("Reset to Defaults"))
            {
                FieldOfView.Value = (float)FieldOfView.DefaultValue;
                CameraDistance.Value = (float)CameraDistance.DefaultValue;
                CameraPitch.Value = (float)CameraPitch.DefaultValue;
                CameraHeight.Value = (float)CameraHeight.DefaultValue;
                CameraHorizontal.Value = (float)CameraHorizontal.DefaultValue;
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Close"))
            {
                showUI = false;
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }
}
