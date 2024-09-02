using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The player's transform
    public float verticalOffset = 2f; // How far above the player the camera should be
    public float smoothTime = 0.3f; // Time for camera to catch up with player
    public float lookAheadFactor = 0.1f; // How far ahead the camera should look

    private float currentVelocity;
    private float targetVerticalPosition;
    public float MinY { get; set; } // Minimum Y position of the camera

    public float CurrentVerticalVelocity { get; private set; } // Exposed for WaterManager

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target not set!");
            return;
        }

        // Set initial minimum Y to the starting position of the camera
        MinY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target not set!");
            return;
        }

        // Calculate the desired y position
        float desiredPosition = Mathf.Max(target.position.y + verticalOffset, MinY);

        // Look ahead based on player's velocity
        float playerVelocity = target.GetComponent<Rigidbody2D>().velocity.y;
        float lookAheadOffset = playerVelocity * lookAheadFactor;
        
        targetVerticalPosition = desiredPosition + lookAheadOffset;

        // Smoothly move the camera towards the target position
        float newY = Mathf.SmoothDamp(transform.position.y, targetVerticalPosition, ref currentVelocity, smoothTime);

        // Ensure the camera doesn't go below the minimum Y position
        newY = Mathf.Max(newY, MinY);

        // Calculate and store the current vertical velocity
        CurrentVerticalVelocity = (newY - transform.position.y) / Time.deltaTime;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}