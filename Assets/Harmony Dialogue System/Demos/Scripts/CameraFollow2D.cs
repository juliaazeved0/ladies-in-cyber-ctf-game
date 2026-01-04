using UnityEngine;

namespace HarmonyDialogueSystem
{
    public class CameraFollow2D : MonoBehaviour
    {
        public Transform target;           // The player or target to follow
        public float smoothSpeed = 0.125f; // Smoothing speed for camera movement
        public Vector3 offset = new Vector3(5, 0, -10); // Offset from the target (adjust as needed)

        void LateUpdate()
        {
            if (target == null)
            {
                Debug.LogWarning("Target not set for PlatformerCameraFollow2D!");
                return;
            }

            // Only follow the target's x position, keep y and z fixed with offset
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, offset.y, offset.z);

            // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Set the camera position
            transform.position = smoothedPosition;
        }
    }
}
