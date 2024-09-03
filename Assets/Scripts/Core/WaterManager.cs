using UnityEngine;

namespace Core {
    public class WaterManager : MonoBehaviour {
        public Camera mainCamera;
        [Range(0, 1)] public float waterHeightPercentage = 0.6f;
        public float buoyancyForce = 1f;
        public float linearDrag = 0.5f;
        public float angularDrag = 0.5f;
        public float smoothTime = 0.3f; // Smoothing time for water movement

        private BuoyancyEffector2D buoyancyEffector;
        private BoxCollider2D waterCollider;
        private CameraController cameraController;
        private float waterYVelocity;
        private float targetWaterY;

        private void Start() {
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

        private void SetupBuoyancyEffector() {
            buoyancyEffector.surfaceLevel = 1f;
            buoyancyEffector.density = buoyancyForce;
            buoyancyEffector.linearDrag = linearDrag;
            buoyancyEffector.angularDrag = angularDrag;
        }

        private void InitializeWaterPosition() {
            UpdateWaterPosition(true);
        }

        private void LateUpdate() {
            UpdateWaterPosition();
        }

        private void UpdateWaterPosition(bool immediate = false) {
            if (cameraController == null) return;

            float cameraHeight = 2f * mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            float waterHeight = cameraHeight * waterHeightPercentage;

            // Calculate target position
            targetWaterY = mainCamera.transform.position.y - cameraHeight / 2 + waterHeight / 2;

            // Smooth the water movement
            float newY = immediate ? targetWaterY : Mathf.SmoothDamp(transform.position.y, targetWaterY, ref waterYVelocity, smoothTime);

            // Update position
            transform.position = new Vector3(mainCamera.transform.position.x, newY, transform.position.z);

            // Scale the water to fit the camera width and desired height
            waterCollider.size = new Vector2(cameraWidth, waterHeight);
        }
    }
}