using UnityEngine;
using System.Collections.Generic;

namespace DeskXR.Core
{
    public class XRObjects : MonoBehaviour
    {
        [Header("Object Management")]
        public float autoScaleFactor = 1.0f;
        public bool validatePositions = true;
        public bool autoRescaleOnAdd = true;

        [Header("Position Guidelines")]
        public float minRecommendedDistance = 10f;
        public float maxRecommendedDistance = 60f;
        public bool showPositionWarnings = true;

        [Header("Debug")]
        public bool showDebugInfo = false;

        private List<GameObject> managedObjects = new List<GameObject>();
        private List<Vector3> originalScales = new List<Vector3>();

        public int ObjectCount => managedObjects.Count;
        public List<GameObject> ManagedObjects => new List<GameObject>(managedObjects);

        private void Start()
        {
            Debug.Log("[DeskXR] XRObjects container initialized");
        }

        public void AddObject(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("[DeskXR] Cannot add null object to XRObjects");
                return;
            }

            if (managedObjects.Contains(obj))
            {
                Debug.LogWarning($"[DeskXR] Object '{obj.name}' is already managed by XRObjects");
                return;
            }

            // Store original scale
            originalScales.Add(obj.transform.localScale);

            // Set as child of XRObjects
            obj.transform.SetParent(transform);

            // Apply automatic scaling if enabled
            if (autoRescaleOnAdd)
            {
                ApplyAutoScaling(obj);
            }

            // Validate position if enabled
            if (validatePositions)
            {
                ValidateObjectPosition(obj);
            }

            // Add to managed list
            managedObjects.Add(obj);

            Debug.Log($"[DeskXR] Object '{obj.name}' added to XRObjects (Total: {managedObjects.Count})");

            if (showDebugInfo)
            {
                Debug.Log($"[DeskXR] Object position: {obj.transform.localPosition}, Scale: {obj.transform.localScale}");
            }
        }

        public void RemoveObject(GameObject obj)
        {
            if (obj == null) return;

            int index = managedObjects.IndexOf(obj);
            if (index >= 0)
            {
                // Restore original scale if available
                if (index < originalScales.Count)
                {
                    obj.transform.localScale = originalScales[index];
                    originalScales.RemoveAt(index);
                }

                managedObjects.RemoveAt(index);

                // Remove from hierarchy
                obj.transform.SetParent(null);

                Debug.Log($"[DeskXR] Object '{obj.name}' removed from XRObjects (Remaining: {managedObjects.Count})");
            }
            else
            {
                Debug.LogWarning($"[DeskXR] Object '{obj.name}' was not managed by XRObjects");
            }
        }

        private void ApplyAutoScaling(GameObject obj)
        {
            Vector3 currentScale = obj.transform.localScale;
            Vector3 newScale = currentScale * autoScaleFactor;
            obj.transform.localScale = newScale;

            if (showDebugInfo)
            {
                Debug.Log($"[DeskXR] Applied auto-scaling to '{obj.name}': {currentScale} -> {newScale}");
            }
        }

        private void ValidateObjectPosition(GameObject obj)
        {
            Vector3 position = obj.transform.localPosition;
            float distance = Mathf.Abs(position.z);

            if (distance > maxRecommendedDistance && showPositionWarnings)
            {
                Debug.LogWarning($"[DeskXR] Object '{obj.name}' may cause ghosting effects (Z distance: {distance:F1}). " +
                                $"Recommended max distance: {maxRecommendedDistance}");
            }
            else if (distance < minRecommendedDistance && distance > 0.1f && showPositionWarnings)
            {
                Debug.LogWarning($"[DeskXR] Object '{obj.name}' may appear too close (Z distance: {distance:F1}). " +
                                $"Recommended min distance: {minRecommendedDistance}");
            }

            // Check for floating vs sunken placement
            if (position.z < 0)
            {
                if (showDebugInfo)
                    Debug.Log($"[DeskXR] Object '{obj.name}' positioned for floating effect (Z: {position.z:F1})");
            }
            else if (position.z > 0)
            {
                if (showDebugInfo)
                    Debug.Log($"[DeskXR] Object '{obj.name}' positioned for sunken effect (Z: {position.z:F1})");
            }
        }

        public void UpdateAutoScaleFactor(float newFactor)
        {
            float oldFactor = autoScaleFactor;
            autoScaleFactor = newFactor;

            // Optionally re-apply scaling to existing objects
            if (managedObjects.Count > 0)
            {
                Debug.Log($"[DeskXR] Updated auto-scale factor: {oldFactor} -> {autoScaleFactor}");
            }
        }

        public void ReapplyScalingToAllObjects()
        {
            for (int i = 0; i < managedObjects.Count; i++)
            {
                if (managedObjects[i] != null && i < originalScales.Count)
                {
                    // Reset to original scale first
                    managedObjects[i].transform.localScale = originalScales[i];

                    // Apply current auto-scaling
                    ApplyAutoScaling(managedObjects[i]);
                }
            }

            Debug.Log($"[DeskXR] Re-applied scaling to {managedObjects.Count} objects");
        }

        public void ValidateAllObjectPositions()
        {
            foreach (GameObject obj in managedObjects)
            {
                if (obj != null)
                {
                    ValidateObjectPosition(obj);
                }
            }
        }

        public void ClearAllObjects()
        {
            // Remove all objects but don't destroy them
            while (managedObjects.Count > 0)
            {
                RemoveObject(managedObjects[0]);
            }

            Debug.Log("[DeskXR] Cleared all objects from XRObjects");
        }

        // Get objects by distance range
        public List<GameObject> GetObjectsInRange(float minZ, float maxZ)
        {
            List<GameObject> objectsInRange = new List<GameObject>();

            foreach (GameObject obj in managedObjects)
            {
                if (obj != null)
                {
                    float z = obj.transform.localPosition.z;
                    if (z >= minZ && z <= maxZ)
                    {
                        objectsInRange.Add(obj);
                    }
                }
            }

            return objectsInRange;
        }

        private void OnDestroy()
        {
            // Clean up when component is destroyed
            managedObjects.Clear();
            originalScales.Clear();
        }
    }
}
