using UnityEngine;

namespace DeskXR.Core
{
    public class XRCamera : MonoBehaviour
    {
        [Header("License Settings")]
        public string keyID = "";
        public string productName = "";
        public string companyName = "";
        
        [Header("Camera Configuration")]
        public Camera mainCamera;
        public float fieldOfView = 60f;
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 100f;
        
        [Header("Position Settings")]
        public Vector3 defaultPosition = new Vector3(0, 0, -0.6f); // 60cm from screen
        public bool fixedScale = true;
        
        private bool isLicenseValid = false;
        private bool isInitialized = false;
        
        private void Start()
        {
            InitializeCamera();
        }
        
        private void InitializeCamera()
        {
            if (isInitialized) return;
            
            Debug.Log("[DeskXR] Initializing XRCamera...");
            
            // Setup main camera
            SetupMainCamera();
            
            // Validate license
            ValidateLicense();
            
            // Set fixed position and scale
            if (fixedScale)
            {
                transform.localScale = Vector3.one;
            }
            
            isInitialized = true;
            Debug.Log("[DeskXR] XRCamera initialization complete");
        }
        
        private void SetupMainCamera()
        {
            // Find or create main camera
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                
                if (mainCamera == null)
                {
                    // Create new camera
                    GameObject camObj = new GameObject("Main Camera");
                    camObj.transform.SetParent(transform);
                    camObj.transform.localPosition = Vector3.zero;
                    mainCamera = camObj.AddComponent<Camera>();
                    camObj.tag = "MainCamera";
                    
                    Debug.Log("[DeskXR] Created new main camera");
                }
                else
                {
                    // Use existing main camera
                    mainCamera.transform.SetParent(transform);
                    mainCamera.transform.localPosition = Vector3.zero;
                    
                    Debug.Log("[DeskXR] Using existing main camera");
                }
            }
            
            // Configure camera settings
            mainCamera.fieldOfView = fieldOfView;
            mainCamera.nearClipPlane = nearClipPlane;
            mainCamera.farClipPlane = farClipPlane;
            
            // Set default position
            transform.localPosition = defaultPosition;
        }
        
        private void ValidateLicense()
        {
            if (string.IsNullOrEmpty(keyID) || string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(companyName))
            {
                Debug.LogWarning("[DeskXR] License information incomplete. Please configure in XRCamera Inspector.");
                Debug.LogWarning("[DeskXR] Running in evaluation mode. Some features may be limited.");
                isLicenseValid = false;
                return;
            }
            
            // Simple license validation (can be enhanced with proper validation)
            if (keyID.Length >= 8 && !string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(companyName))
            {
                isLicenseValid = true;
                Debug.Log($"[DeskXR] License validated for: {productName} by {companyName}");
            }
            else
            {
                isLicenseValid = false;
                Debug.LogError("[DeskXR] License validation failed. Please check license information.");
            }
        }
        
        public bool IsLicenseValid()
        {
            return isLicenseValid;
        }
        
        public void UpdateLicenseInfo(string newKeyID, string newProductName, string newCompanyName)
        {
            keyID = newKeyID;
            productName = newProductName;
            companyName = newCompanyName;
            
            ValidateLicense();
        }
        
        public void UpdateCameraSettings(float fov, float nearClip, float farClip)
        {
            fieldOfView = fov;
            nearClipPlane = nearClip;
            farClipPlane = farClip;
            
            if (mainCamera != null)
            {
                mainCamera.fieldOfView = fieldOfView;
                mainCamera.nearClipPlane = nearClipPlane;
                mainCamera.farClipPlane = farClipPlane;
            }
        }
        
        private void LateUpdate()
        {
            // Enforce fixed scale if enabled
            if (fixedScale && transform.localScale != Vector3.one)
            {
                transform.localScale = Vector3.one;
                Debug.LogWarning("[DeskXR] XRCamera scale is fixed and cannot be modified");
            }
        }
    }
}
