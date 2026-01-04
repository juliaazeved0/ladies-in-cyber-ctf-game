using UnityEngine;

namespace HarmonyDialogueSystem
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;           // The object to follow (e.g., player)
        public Vector3 offset = new Vector3(0, 5, -10); // Camera offset from the target
        public float smoothSpeed = 0.125f; // Speed of the camera smoothing

        void LateUpdate()
        {
            if (target == null)
            {
                Debug.LogWarning("Target not set for ThirdPersonCameraFollow!");
                return;
            }

            // Calculate the desired position
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate from current position to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Set the camera's position
            transform.position = smoothedPosition;

            // Optionally, make the camera look at the target
            transform.LookAt(target);
        }
    }
}
