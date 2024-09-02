using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public Camera mainCamera;
    public float waterHeightPercentage = 0.6f; // 60% of the screen height is water
    public float buoyancyForce = 1f;
    public float linearDrag = 0.5f;
    public float angularDrag = 0.5f;

    private BuoyancyEffector2D buoyancyEffector;
    private BoxCollider2D waterCollider;
    private CameraController cameraController;
    private float waterYOffset;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        cameraController = mainCamera.GetComponent<CameraController>();
        if (cameraController == null)
            Debug.LogError("CameraController not found on the main camera!");

        buoyancyEffector = GetComponent<BuoyancyEffector2D>();
        waterCollider = GetComponent<BoxCollider2D>();

        if (buoyancyEffector == null)
            buoyancyEffector = gameObject.AddComponent<BuoyancyEffector2D>();

        if (waterCollider == null)
            waterCollider = gameObject.AddComponent<BoxCollider2D>();

        waterCollider.isTrigger = true;

        SetupBuoyancyEffector();
        InitializeWaterPosition();
    }

    private void SetupBuoyancyEffector()
    {
        buoyancyEffector.surfaceLevel = 1f; // Top of the collider
        buoyancyEffector.density = buoyancyForce;
        buoyancyEffector.linearDrag = linearDrag;
        buoyancyEffector.angularDrag = angularDrag;
    }

    private void InitializeWaterPosition()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float waterHeight = cameraHeight * waterHeightPercentage;
        waterYOffset = -cameraHeight / 2 + waterHeight / 2;
        UpdateWaterPosition();
    }

    private void LateUpdate()
    {
        UpdateWaterPosition();
    }

    private void UpdateWaterPosition()
    {
        if (cameraController == null) return;

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        float waterHeight = cameraHeight * waterHeightPercentage;

        // Always position the water at the bottom of the camera view
        Vector3 newPosition = new Vector3(
            mainCamera.transform.position.x,
            mainCamera.transform.position.y - cameraHeight / 2 + waterHeight / 2,
            transform.position.z
        );

        transform.position = newPosition;

        // Scale the water to fit the camera width and desired height
        waterCollider.size = new Vector2(cameraWidth, waterHeight);
    }
}