using HarmonyLib;
using RoR2;
using UnityEngine;

namespace CamFreeModifiyer
{
    /// <summary>
    /// Harmony patches for modifying RoR2 camera behavior
    /// </summary>
    [HarmonyPatch]
    public static class CameraPatches
    {
        // Store original values for reset
        private static bool initialized = false;
        private static float originalBaseFov;

        /// <summary>
        /// Patch to apply FOV modification
        /// </summary>
        [HarmonyPatch(typeof(CameraRigController), "Start")]
        [HarmonyPostfix]
        public static void CameraRigController_Start_Postfix(CameraRigController __instance)
        {
            if (__instance == null) return;

            if (!initialized)
            {
                originalBaseFov = __instance.baseFov;
                initialized = true;
            }
        }

        /// <summary>
        /// Patch to modify camera distance - intercept the currentCameraDistance setter
        /// </summary>
        [HarmonyPatch(typeof(CameraRigController), "SetCameraState")]
        [HarmonyPrefix]
        public static void CameraRigController_SetCameraState_Prefix(CameraRigController __instance)
        {
            if (__instance == null) return;

            // Apply FOV
            __instance.baseFov = Plugin.FieldOfView.Value;
        }

        /// <summary>
        /// Patch to apply camera offset modifications
        /// </summary>
        [HarmonyPatch(typeof(CameraRigController), "SetCameraState")]
        [HarmonyPostfix]
        public static void CameraRigController_SetCameraState_Postfix(CameraRigController __instance)
        {
            if (__instance == null || __instance.sceneCam == null) return;

            Camera cam = __instance.sceneCam;

            // Apply FOV directly to camera
            cam.fieldOfView = Plugin.FieldOfView.Value;

            // Apply height and pitch offset to the camera transform
            // These are additive adjustments, not replacements
            Transform camTransform = cam.transform;
            Vector3 pos = camTransform.localPosition;
            Vector3 euler = camTransform.localEulerAngles;

            // Apply distance offset (move camera back/forward along its forward axis)
            float distanceOffset = Plugin.CameraDistance.Value - 10f; // 10 is baseline
            if (distanceOffset != 0f)
            {
                pos -= camTransform.forward * distanceOffset;
            }

            // Apply height offset
            if (Plugin.CameraHeight.Value != 0f)
            {
                pos.y += Plugin.CameraHeight.Value;
            }

            // Apply pitch offset
            if (Plugin.CameraPitch.Value != 0f)
            {
                euler.x += Plugin.CameraPitch.Value;
            }

            camTransform.localPosition = pos;
            camTransform.localEulerAngles = euler;
        }
    }
}
