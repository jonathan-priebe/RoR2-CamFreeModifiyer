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
        // Store the last applied offsets so we can undo them
        private static Vector3 lastAppliedPositionOffset = Vector3.zero;
        private static Vector3 lastAppliedEulerOffset = Vector3.zero;
        private static bool offsetsApplied = false;

        /// <summary>
        /// BEFORE the game calculates camera position: undo our previous offsets
        /// </summary>
        [HarmonyPatch(typeof(CameraRigController), "SetCameraState")]
        [HarmonyPrefix]
        public static void CameraRigController_SetCameraState_Prefix(CameraRigController __instance)
        {
            if (__instance == null || __instance.sceneCam == null) return;

            // Apply FOV
            __instance.baseFov = Plugin.FieldOfView.Value;

            // Undo previous offsets so the game calculates from clean position
            if (offsetsApplied)
            {
                Transform camTransform = __instance.sceneCam.transform;
                camTransform.position -= lastAppliedPositionOffset;

                Vector3 euler = camTransform.eulerAngles;
                euler.x -= lastAppliedEulerOffset.x;
                camTransform.eulerAngles = euler;

                offsetsApplied = false;
            }
        }

        /// <summary>
        /// AFTER the game calculates camera position: apply our offsets
        /// </summary>
        [HarmonyPatch(typeof(CameraRigController), "SetCameraState")]
        [HarmonyPostfix]
        public static void CameraRigController_SetCameraState_Postfix(CameraRigController __instance)
        {
            if (__instance == null || __instance.sceneCam == null) return;

            Camera cam = __instance.sceneCam;
            Transform camTransform = cam.transform;

            // Apply FOV
            cam.fieldOfView = Plugin.FieldOfView.Value;

            // Calculate offsets
            float distanceOffset = Plugin.CameraDistance.Value - 10f;
            float heightOffset = Plugin.CameraHeight.Value;
            float horizontalOffset = Plugin.CameraHorizontal.Value;
            float pitchOffset = Plugin.CameraPitch.Value;

            // Reset tracking
            lastAppliedPositionOffset = Vector3.zero;
            lastAppliedEulerOffset = Vector3.zero;

            // Apply distance offset
            if (Mathf.Abs(distanceOffset) > 0.001f)
            {
                Vector3 offset = -camTransform.forward * distanceOffset;
                camTransform.position += offset;
                lastAppliedPositionOffset += offset;
            }

            // Apply height offset
            if (Mathf.Abs(heightOffset) > 0.001f)
            {
                Vector3 offset = new Vector3(0f, heightOffset, 0f);
                camTransform.position += offset;
                lastAppliedPositionOffset += offset;
            }

            // Apply horizontal offset (left/right)
            if (Mathf.Abs(horizontalOffset) > 0.001f)
            {
                Vector3 offset = camTransform.right * horizontalOffset;
                camTransform.position += offset;
                lastAppliedPositionOffset += offset;
            }

            // Apply pitch offset
            if (Mathf.Abs(pitchOffset) > 0.001f)
            {
                Vector3 euler = camTransform.eulerAngles;
                euler.x += pitchOffset;
                camTransform.eulerAngles = euler;
                lastAppliedEulerOffset.x = pitchOffset;
            }

            offsetsApplied = true;
        }
    }
}
