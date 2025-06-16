using UnityEngine;

namespace DeskXR.Core
{
    public class XRStage : MonoBehaviour
    {
        [Header("Stage Configuration")]
        public Vector3 stageSize = new Vector3(2f, 1.5f, 2f);
        
        [Header("Core Components")]
        public XRScreen xrScreen;
        public XRCamera xrCamera; 
        public XRObjects xrObjects;
        
        [Header("Stage Settings")]
        public bool fixedPosition = true;
        public bool autoCreateComponents = true;
        
        private bool isInitialized = false;
        
        private void Awake()
        {
            // Ensure stage is at origin if fixed
            if (fixedPosition)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
        
        public void InitializeStage()
        {
            if (isInitialized) return;
            
            Debug.Log("[DeskXR] Initializing XRStage...");
            
            if (autoCreateComponents)
            {
                CreateCoreComponents();
            }
            
            ValidateComponents();
            isInitialized = true;
            
            Debug.Log("[DeskXR] XRStage initialization complete");
        }
        
        private void CreateCoreComponents()
        {
            // Create XRScreen if not assigned
            if (xrScreen == null)
            {
                GameObject screenObj = new GameObject("XRScreen");
                screenObj.transform.SetParent(transform);
                screenObj.transform.localPosition = Vector3.zero;
                xrScreen = screenObj.AddComponent<XRScreen>();
                Debug.Log("[DeskXR] Created XRScreen component");
            }
            
            // Create XRCamera if not assigned
            if (xrCamera == null)
            {
                GameObject cameraObj = new GameObject("XRCamera");
                cameraObj.transform.SetParent(transform);
                cameraObj.transform.localPosition = new Vector3(0, 0, -0.6f); // 60cm from screen
                xrCamera = cameraObj.AddComponent<XRCamera>();
                Debug.Log("[DeskXR] Created XRCamera component");
            }
            
            // Create XRObjects if not assigned
            if (xrObjects == null)
            {
                GameObject objectsObj = new GameObject("XRObjects");
                objectsObj.transform.SetParent(transform);
                objectsObj.transform.localPosition = Vector3.zero;
                xrObjects = objectsObj.AddComponent<XRObjects>();
                Debug.Log("[DeskXR] Created XRObjects component");
            }
        }
        
        private void ValidateComponents()
        {
            bool isValid = true;
            
            if (xrScreen == null)
            {
                Debug.LogWarning("[DeskXR] XRScreen component not found in XRStage");
                isValid = false;
            }
            
            if (xrCamera == null)
            {
                Debug.LogWarning("[DeskXR] XRCamera component not found in XRStage");
                isValid = false;
            }
            
            if (xrObjects == null)
            {
                Debug.LogWarning("[DeskXR] XRObjects component not found in XRStage");
                isValid = false;
            }
            
            if (isValid)
            {
                Debug.Log("[DeskXR] XRStage component validation passed");
            }
        }
        
        private void LateUpdate()
        {
            // Enforce fixed position if enabled
            if (fixedPosition && transform.localPosition != Vector3.zero)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
        
        public void AddObjectToStage(GameObject obj)
        {
            if (xrObjects != null)
            {
                xrObjects.AddObject(obj);
            }
            else
            {
                Debug.LogWarning("[DeskXR] Cannot add object - XRObjects component not found");
            }
        }
        
        public void RemoveObjectFromStage(GameObject obj)
        {
            if (xrObjects != null)
            {
                xrObjects.RemoveObject(obj);
            }
        }
    }
}