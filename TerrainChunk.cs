// Terrain Chunk for Mobile Terrain Forge
// Runtime terrain chunk with LOD support and GPU instancing
// Optimized for mobile: efficient memory usage, streaming

using UnityEngine;
using System.Collections.Generic;

namespace MobileTerrainForge
{
    public class TerrainChunk : MonoBehaviour
    {
        // Chunk configuration
        private LODManager lodManager;
        private int chunkIndex;
        private Vector3 worldPosition;

        // LOD state
        private int currentLODResolution = 1;
        private Material overrideMaterial;
        private bool gpuInstancingEnabled = false;

        // Mesh data
        private Mesh chunkMesh;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        // Splatmap data
        private SplatmapOptimizer.SplatChannel[] terrainTypes;

        // Performance tracking
        private int drawCallCount = 1;

        // Initialization
        public void Initialize(LODManager manager, int index, Vector3 position)
        {
            lodManager = manager;
            chunkIndex = index;
            worldPosition = position;

            // Get or add required components
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            // Create initial mesh
            CreateInitialMesh();

            // Set default terrain types
            InitializeTerrainTypes();
        }

        private void CreateInitialMesh()
        {
            // Create simple placeholder mesh
            chunkMesh = new Mesh();
            chunkMesh.name = $"TerrainChunk_{chunkIndex}";

            // Generate basic terrain geometry (flat plane)
            GenerateBaseMesh(chunkMesh, 10, 10, 1); // 10x10 meters, 1m resolution

            meshFilter.sharedMesh = chunkMesh;

            // Create default material
            Material defaultMat = new Material(Shader.Find("Standard"));
            meshRenderer.sharedMaterial = defaultMat;
        }

        private void InitializeTerrainTypes()
        {
            // Initialize terrain type array (one per vertex, simplified)
            int vertexCount = chunkMesh != null ? chunkMesh.vertexCount : 100;
            terrainTypes = new SplatmapOptimizer.SplatChannel[vertexCount];

            // Default to grass
            for (int i = 0; i < terrainTypes.Length; i++)
            {
                terrainTypes[i] = SplatmapOptimizer.SplatChannel.Grass;
            }
        }

        // Generate base mesh geometry
        private void GenerateBaseMesh(Mesh mesh, int width, int length, int resolution)
        {
            int verticesX = width / resolution + 1;
            int verticesZ = length / resolution + 1;
            int totalVertices = verticesX * verticesZ;

            Vector3[] vertices = new Vector3[totalVertices];
            int[] triangles = new int[(verticesX - 1) * (verticesZ - 1) * 6];
            Vector2[] uv = new Vector2[totalVertices];

            // Generate vertices
            for (int z = 0; z < verticesZ; z++)
            {
                for (int x = 0; x < verticesX; x++)
                {
                    int index = z * verticesX + x;

                    vertices[index] = new Vector3(
                        (x - verticesX / 2) * resolution,
                        0, // Flat for now
                        (z - verticesZ / 2) * resolution
                    );

                    uv[index] = new Vector2((float)x / (verticesX - 1), (float)z / (verticesZ - 1));
                }
            }

            // Generate triangles
            int triangleIndex = 0;
            for (int z = 0; z < verticesZ - 1; z++)
            {
                for (int x = 0; x < verticesX - 1; x++)
                {
                    int topLeft = z * verticesX + x;
                    int topRight = topLeft + 1;
                    int bottomLeft = (z + 1) * verticesX + x;
                    int bottomRight = bottomLeft + 1;

                    // First triangle
                    triangles[triangleIndex++] = topLeft;
                    triangles[triangleIndex++] = bottomLeft;
                    triangles[triangleIndex++] = topRight;

                    // Second triangle
                    triangles[triangleIndex++] = topRight;
                    triangles[triangleIndex++] = bottomLeft;
                    triangles[triangleIndex++] = bottomRight;
                }
            }

            // Assign to mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        // Set LOD resolution
        public void SetLODResolution(int resolution)
        {
            if (resolution == currentLODResolution) return;

            currentLODResolution = resolution;

            // Regenerate mesh with new resolution
            if (chunkMesh != null)
            {
                GenerateBaseMesh(chunkMesh, 10, 10, resolution);
            }
        }

        // Set material override
        public void SetMaterial(Material material)
        {
            overrideMaterial = material;

            if (meshRenderer != null && material != null)
            {
                meshRenderer.sharedMaterial = material;
            }
        }

        // Enable/disable GPU instancing
        public void EnableGPUInstancing(bool enable)
        {
            gpuInstancingEnabled = enable;

            if (meshRenderer != null && meshRenderer.sharedMaterial != null)
            {
                meshRenderer.sharedMaterial.enableInstancing = enable;
            }
        }

        // Update terrain from heightmap data
        public void UpdateFromHeightmap(float[,] heightmap, int width, int length)
        {
            if (chunkMesh == null) return;

            Vector3[] vertices = chunkMesh.vertices;

            // Update vertex heights based on heightmap
            for (int z = 0; z < length; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    int vertexIndex = z * width + x;

                    if (vertexIndex < vertices.Length)
                    {
                        vertices[vertexIndex].y = heightmap[x, z];
                    }
                }
            }

            chunkMesh.vertices = vertices;
            chunkMesh.RecalculateNormals();
            chunkMesh.RecalculateBounds();
        }

        // Get current draw call count
        public int GetDrawCallCount()
        {
            return drawCallCount;
        }

        // Set draw call count (for performance tracking)
        public void SetDrawCallCount(int count)
        {
            drawCallCount = count;
        }

        // Get current LOD resolution
        public int GetCurrentLODResolution()
        {
            return currentLODResolution;
        }

        // Get terrain type at a specific vertex
        public SplatmapOptimizer.SplatChannel GetTerrainType(int vertexIndex)
        {
            if (vertexIndex >= 0 && vertexIndex < terrainTypes.Length)
            {
                return terrainTypes[vertexIndex];
            }
            return SplatmapOptimizer.SplatChannel.Grass;
        }

        // Set terrain type at a specific vertex
        public void SetTerrainType(int vertexIndex, SplatmapOptimizer.SplatChannel type)
        {
            if (vertexIndex >= 0 && vertexIndex < terrainTypes.Length)
            {
                terrainTypes[vertexIndex] = type;
            }
        }

        // Get chunk index
        public int GetChunkIndex()
        {
            return chunkIndex;
        }

        // Get world position
        public Vector3 GetWorldPosition()
        {
            return worldPosition;
        }

        // Cleanup when chunk is unloaded
        public void Cleanup()
        {
            if (chunkMesh != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(chunkMesh);
                }
                else
                {
                    Object.DestroyImmediate(chunkMesh);
                }
                chunkMesh = null;
            }

            if (overrideMaterial != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(overrideMaterial);
                }
                else
                {
                    Object.DestroyImmediate(overrideMaterial);
                }
                overrideMaterial = null;
            }

            terrainTypes = null;
        }

        // Get mesh for external access
        public Mesh GetMesh()
        {
            return chunkMesh;
        }

        // Get mesh renderer for external access
        public MeshRenderer GetMeshRenderer()
        {
            return meshRenderer;
        }

        // Get mesh filter for external access
        public MeshFilter GetMeshFilter()
        {
            return meshFilter;
        }

        // Calculate bounds
        public Bounds GetBounds()
        {
            if (chunkMesh != null)
            {
                return chunkMesh.bounds;
            }
            return new Bounds(transform.position, Vector3.one * 10);
        }

        // Check if point is within this chunk
        public bool ContainsPoint(Vector3 worldPoint)
        {
            Bounds bounds = GetBounds();
            return bounds.Contains(worldPoint);
        }

        // Get height at world position
        public float GetHeightAtPosition(Vector3 worldPosition)
        {
            // Convert world position to local chunk coordinates
            Vector3 localPos = transform.InverseTransformPoint(worldPosition);

            // Find closest vertex (simplified)
            if (chunkMesh != null && chunkMesh.vertices != null)
            {
                Vector3[] vertices = chunkMesh.vertices;

                // Simple linear search (optimize in production)
                float closestDistance = float.MaxValue;
                float closestHeight = 0;

                foreach (Vector3 vertex in vertices)
                {
                    float distance = Vector2.Distance(new Vector2(localPos.x, localPos.z), new Vector2(vertex.x, vertex.z));

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestHeight = vertex.y;
                    }
                }

                return transform.TransformPoint(new Vector3(0, closestHeight, 0)).y;
            }

            return transform.position.y;
        }

        // Set visibility
        public void SetVisible(bool visible)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = visible;
            }
        }

        // Is visible
        public bool IsVisible()
        {
            return meshRenderer != null && meshRenderer.enabled;
        }

        // Force mesh update
        public void ForceMeshUpdate()
        {
            if (chunkMesh != null)
            {
                chunkMesh.RecalculateNormals();
                chunkMesh.RecalculateBounds();
                chunkMesh.RecalculateTangents();
            }
        }

        // Optimization: Combine meshes if possible
        public bool CanCombineWith(TerrainChunk otherChunk)
        {
            // Check if chunks are adjacent and have same LOD
            if (otherChunk == null) return false;

            float distance = Vector3.Distance(worldPosition, otherChunk.worldPosition);

            // Chunks should be 10 meters apart
            if (Mathf.Abs(distance - 10f) < 0.1f)
            {
                return currentLODResolution == otherChunk.GetCurrentLODResolution();
            }

            return false;
        }

        // Debug visualization
        private void OnDrawGizmos()
        {
            if (!Debug.isDebugBuild) return;

            // Draw chunk bounds
            Bounds bounds = GetBounds();

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            // Draw LOD indicator
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.5f);

            // Draw label with LOD resolution
            if (Camera.current != null)
            {
                float distance = Vector3.Distance(Camera.current.transform.position, transform.position);

                if (distance < 20f)
                {
                    UnityEditor.Handles.Label(transform.position + Vector3.up * 2, $"LOD: {currentLODResolution}m\nDC: {drawCallCount}");
                }
            }
        }
    }
}