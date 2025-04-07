using UnityEngine;

namespace BrickDestroyer
{
    public class CameraSize : MonoBehaviour
    {
        private float screenAspectRatio;

        void Start()
        {
            // Calculate the screen aspect ratio
            screenAspectRatio = (float)Screen.width / Screen.height;

            // Calculate the field of view adjustment based on aspect ratio
            float sizeAdjustment = 6 - (screenAspectRatio - 0.5f) * 11.66f;

            // Set the camera field of view with a minimum threshold
            Camera.main.fieldOfView = sizeAdjustment <= 5.3f ? 65f : 65f + sizeAdjustment;
        }
    }
}