using UnityEngine;

namespace DeskXR.Core
{
    public class DeskXRCore : MonoBehaviour
    {
        [Header("Core Components")]
        public XRStage xrStage;
        public SettingsManager settingsManager;
        
        [Header("System Status")]
        public bool isInitialized = false;
        public bool autoInitialize = true;
        
        private static DeskXRCore _instance;
        public static DeskXRCore Instance 
        {
            get 
            {
                if (_instance == null)
                    _instance = FindObjectOfType<DeskXRCore>();
                return _instance;
            }
        }
        
        private void Awake()
        {
            // Singleton pattern
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                
                if (autoInitialize)
                    InitializeSystem();
            }
            else if (_instance != this)
            {
                Debug.LogWarning("[DeskXR] Multiple DeskXRCore instances found. Destroying duplicate.");
                Destroy(gameObject);
            }
        }
        
        private void InitializeSystem()
        {
            Debug.Log("[DeskXR] Initializing DeskXR System...");
            
            // Create component hierarchy if not present
            CreateComponentHierarchy();
            
            // Initialize settings
            InitializeSettings();
            
            // Validate system
            ValidateSystem();
            
            isInitialized = true;
            Debug.Log("[DeskXR] System initialization complete!");
        }
        
        private void CreateComponentHierarchy()
        {
            // Create XRStage if not assigned
            if (xrStage == null)
            {
                GameObject stageObj = new GameObject("XRStage");
                stageObj.transform.SetParent(transform);
                xrStage = stageObj.AddComponent<XRStage>();
                Debug.Log("[DeskXR] Created XRStage component");
            }
            
            // Initialize XRStage
            xrStage.InitializeStage();
        }
        
        private void InitializeSettings()
        {
            if (settingsManager == null)
            {
                GameObject settingsObj = new GameObject("SettingsManager");
                settingsObj.transform.SetParent(transform);
                settingsManager = settingsObj.AddComponent<SettingsManager>();
                Debug.Log("[DeskXR] Created SettingsManager component");
            }
        }
        
        private void ValidateSystem()
        {
            bool isValid = true;
            
            if (xrStage == null)
            {
                Debug.LogError("[DeskXR] XRStage component missing!");
                isValid = false;
            }
            
            if (settingsManager == null)
            {
                Debug.LogError("[DeskXR] SettingsManager component missing!");
                isValid = false;
            }
            
            if (!isValid)
            {
                Debug.LogError("[DeskXR] System validation failed. Please check component setup.");
            }
        }
        
        public void RestartSystem()
        {
            isInitialized = false;
            InitializeSystem();
        }
        
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
