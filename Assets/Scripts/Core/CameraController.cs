using UnityEngine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        public Transform target; // The player's transform
        public float verticalOffset = 2f; // How far above the player the camera should be
        public float smoothTime = 0.5f; // Time for camera to catch up with player
        public float lookAheadFactor = 0.05f; // How far ahead the camera should look
        public float topMargin = 2f; // Distance from the top of the screen where camera stops following player

        private float currentVelocity;
        private float targetVerticalPosition;
        public float MinY;// Minimum Y position of the camera
        public float MaxY;// Maximum Y position of the camera

        public float CurrentVerticalVelocity { get; private set; } // Exposed for WaterManager

        private void Start()
        {
            if (target == null)
            {
                Debug.LogWarning("Camera target not set!");
                return;
            }

            MinY = transform.position.y;

            MaxY = float.MaxValue;
        }

        private void LateUpdate()
        {
            if (target == null) return;
            float cameraHalfHeight = Camera.main.orthographicSize;
            float maxPlayerY = transform.position.y + cameraHalfHeight - topMargin;

            // Calculate the desired y position
            float desiredPosition = Mathf.Clamp(target.position.y, MinY, maxPlayerY) + verticalOffset;

            // Look ahead based on player's velocity
            Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                float playerVelocity = playerRb.velocity.y;

                // If the player is moving down, set the camera velocity to match the player's velocity
                if (playerVelocity < 0)
                {
                    currentVelocity = playerVelocity;
                    desiredPosition = transform.position.y + playerVelocity * Time.deltaTime;
                }
                else
                {
                    // For upward movement, use look ahead and smoothing
                    float lookAheadOffset = playerVelocity * lookAheadFactor;
                    desiredPosition += lookAheadOffset;

                    // Smoothly move the camera towards the target position for upward movement
                    desiredPosition = Mathf.SmoothDamp(transform.position.y, desiredPosition, ref currentVelocity, smoothTime);
                }
            }

            // Clamp the desired position to respect MinY and MaxY
            desiredPosition = Mathf.Clamp(desiredPosition, MinY, MaxY);

            // Update camera position
            transform.position = new Vector3(transform.position.x, desiredPosition, transform.position.z);

            // Calculate and store the current vertical velocity
            CurrentVerticalVelocity = currentVelocity;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
