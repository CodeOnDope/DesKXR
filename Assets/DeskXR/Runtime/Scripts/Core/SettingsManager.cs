using UnityEngine;

namespace DeskXR.Core
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Basic Settings")]
        public bool debugMode = true;
        
        private static SettingsManager _instance;
        public static SettingsManager Instance 
        {
            get 
            {
                if (_instance == null)
                    _instance = FindObjectOfType<SettingsManager>();
                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("[DeskXR] SettingsManager initialized (basic version)");
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        // Basic functionality for Session 1
        // Will be expanded with full settings system in Session 6
        public void LoadSettings()
        {
            Debug.Log("[DeskXR] Settings loaded (placeholder)");
        }
        
        public void SaveSettings()
        {
            Debug.Log("[DeskXR] Settings saved (placeholder)");
        }
    }
}