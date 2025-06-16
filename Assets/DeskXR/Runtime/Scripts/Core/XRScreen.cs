using UnityEngine;

namespace DeskXR.Core
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class XRScreen : MonoBehaviour
    {
        [Header("Screen Configuration")]
        public Vector3 screenSize = new Vector3(1.6f, 0.9f, 0.1f); // 16:9 aspect ratio
        public Material wallMaterial;
        public bool meshRendererEnabled = true;
        
        [Header("Visual Settings")]
        public Color backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private bool isInitialized = false;
        
        private void Awake()
        {
            // Ensure screen is always at origin (fixed position)
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        
        private void Start()
        {
            InitializeScreen();
        }
        
        private void InitializeScreen()
        {
            if (isInitialized) return;
            
            Debug.Log("[DeskXR] Initializing XRScreen...");
            
            // Get or create mesh components
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            
            // Create screen mesh
            CreateScreenMesh();
            
            // Setup material
            SetupMaterial();
            
            // Configure renderer
            meshRenderer.enabled = meshRendererEnabled;
            
            isInitialized = true;
            Debug.Log("[DeskXR] XRScreen initialization complete");
        }
        
        private void CreateScreenMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "XRScreen Mesh";
            
            // Define vertices for a quad
            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(-screenSize.x * 0.5f, -screenSize.y * 0.5f, 0), // Bottom left
                new Vector3(screenSize.x * 0.5f, -screenSize.y * 0.5f, 0),  // Bottom right
                new Vector3(screenSize.x * 0.5f, screenSize.y * 0.5f, 0),   // Top right
                new Vector3(-screenSize.x * 0.5f, screenSize.y * 0.5f, 0)   // Top left
            };
            
            // Define triangles (two triangles make a quad)
            int[] triangles = new int[6]
            {
                0, 2, 1, // First triangle
                0, 3, 2  // Second triangle
            };
            
            // Define UVs
            Vector2[] uvs = new Vector2[4]
            {
                new Vector2(0, 0), // Bottom left
                new Vector2(1, 0), // Bottom right
                new Vector2(1, 1), // Top right
                new Vector2(0, 1)  // Top left
            };
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            
            meshFilter.mesh = mesh;
            
            Debug.Log($"[DeskXR] Created screen mesh with size: {screenSize}");
        }
        
        private void SetupMaterial()
        {
            if (wallMaterial == null)
            {
                CreateDefaultMaterial();
            }
            else
            {
                meshRenderer.material = wallMaterial;
            }
        }
        
        private void CreateDefaultMaterial()
        {
            wallMaterial = new Material(Shader.Find("Standard"));
            wallMaterial.name = "XRScreen Default Material";
            wallMaterial.color = backgroundColor;
            
            // Configure for transparency
            wallMaterial.SetFloat("_Mode", 3); // Transparent mode
            wallMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            wallMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            wallMaterial.SetInt("_ZWrite", 0);
            wallMaterial.DisableKeyword("_ALPHATEST_ON");
            wallMaterial.EnableKeyword("_ALPHABLEND_ON");
            wallMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            wallMaterial.renderQueue = 3000;
            
            meshRenderer.material = wallMaterial;
            
            Debug.Log("[DeskXR] Created default XRScreen material");
        }
        
        public void SetMeshRendererEnabled(bool enabled)
        {
            meshRendererEnabled = enabled;
            if (meshRenderer != null)
            {
                meshRenderer.enabled = enabled;
            }
        }
        
        public void UpdateMaterial(Material newMaterial)
        {
            if (newMaterial != null)
            {
                wallMaterial = newMaterial;
                if (meshRenderer != null)
                {
                    meshRenderer.material = wallMaterial;
                }
                Debug.Log("[DeskXR] Updated XRScreen material");
            }
        }
        
        public void UpdateScreenSize(Vector3 newSize)
        {
            screenSize = newSize;
            if (isInitialized)
            {
                CreateScreenMesh();
                Debug.Log($"[DeskXR] Updated screen size to: {screenSize}");
            }
        }
        
        private void LateUpdate()
        {
            // Enforce fixed position (cannot be modified as per Holo-SDK spec)
            if (transform.localPosition != Vector3.zero)
            {
                transform.localPosition = Vector3.zero;
                Debug.LogWarning("[DeskXR] XRScreen position is fixed at (0,0,0) and cannot be modified");
            }
            
            if (transform.localRotation != Quaternion.identity)
            {
                transform.localRotation = Quaternion.identity;
                Debug.LogWarning("[DeskXR] XRScreen rotation is fixed and cannot be modified");
            }
        }
    }
}