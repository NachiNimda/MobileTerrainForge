// LOD Manager for Mobile Terrain Forge
// Automatic LOD system for streaming terrain chunks
// Optimized for mobile: 4 levels, GPU instancing, smooth transitions

using UnityEngine;
using System.Collections.Generic;

namespace MobileTerrainForge
{
    public class LODManager : MonoBehaviour
    {
        // LOD level configuration
        [System.Serializable]
        public class LODLevel
        {
            public int resolution;        // Resolution in meters
            public float maxDistance;     // Maximum distance for this LOD
            public string name;           // LOD name for debugging
            public Material material;     // Material override (optional)

            public LODLevel(int res, float dist, string n)
            {
                resolution = res;
                maxDistance = dist;
                name = n;
                material = null;
            }
        }

        [Header("LOD Configuration")]
        [SerializeField] private Transform player;              // Player transform for distance calculation
        [SerializeField] private LODLevel[] lodLevels;          // LOD level definitions
        [SerializeField] private float transitionSmoothness = 0.5f;  // LOD transition speed (0-1)

        [Header("Performance")]
        [SerializeField] private bool enableGPUInstancing = true;
        [SerializeField] private int maxActiveChunks = 100;      // Maximum chunks loaded at once
        [SerializeField] private bool frustumCulling = true;    // Enable camera frustum culling

        // Runtime data
        private Dictionary<int, TerrainChunk> activeChunks = new Dictionary<int, TerrainChunk>();
        private Queue<int> chunkLoadQueue = new Queue<int>();
        private Camera mainCamera;

        // LOD transition state
        private Dictionary<int, int> currentLOD = new Dictionary<int, int>();
        private Dictionary<int, int> targetLOD = new Dictionary<int, int>();

        // Performance tracking
        public int ActiveChunkCount => activeChunks.Count;
        public int CurrentDrawCalls { get; private set; }
        public float MemoryUsageMB { get; private set; }

        private void Awake()
        {
            InitializeLODLevels();
            mainCamera = Camera.main;

            if (mainCamera == null)
            {
                Debug.LogError("LODManager: No main camera found!");
            }
        }

        private void Start()
        {
            // Find player if not assigned
            if (player == null)
            {
                player = FindObjectOfType<ThirdPersonController>()?.transform;
                if (player == null)
                {
                    player = Camera.main?.transform;
                }
            }

            if (player == null)
            {
                Debug.LogWarning("LODManager: No player transform found, using camera as reference");
            }
        }

        private void InitializeLODLevels()
        {
            // Default LOD levels if not configured
            if (lodLevels == null || lodLevels.Length == 0)
            {
                lodLevels = new LODLevel[]
                {
                    new LODLevel(1, 50, "LOD0"),   // 1m resolution, 50m range
                    new LODLevel(2, 150, "LOD1"),  // 2m resolution, 150m range
                    new LODLevel(4, 300, "LOD2"),  // 4m resolution, 300m range
                    new LODLevel(8, float.MaxValue, "LOD3")  // 8m resolution, infinite range
                };
            }
        }

        private void Update()
        {
            UpdateLODLevels();
            ManageChunkStreaming();
            UpdatePerformanceMetrics();
        }

        // Update LOD levels for all active chunks
        private void UpdateLODLevels()
        {
            if (player == null) return;

            Vector3 playerPosition = player.position;

            foreach (var kvp in activeChunks)
            {
                int chunkIndex = kvp.Key;
                TerrainChunk chunk = kvp.Value;

                if (chunk == null) continue;

                // Calculate distance to player
                float distance = Vector3.Distance(chunk.transform.position, playerPosition);

                // Determine target LOD based on distance
                int targetLODIndex = GetTargetLODIndex(distance);

                // Store target LOD for smooth transition
                if (!targetLOD.ContainsKey(chunkIndex))
                {
                    targetLOD[chunkIndex] = targetLODIndex;
                    currentLOD[chunkIndex] = targetLODIndex;
                }
                else
                {
                    targetLOD[chunkIndex] = targetLODIndex;
                }

                // Apply LOD transition
                ApplyLODTransition(chunk, chunkIndex, targetLODIndex);
            }
        }

        // Get the appropriate LOD level for a given distance
        private int GetTargetLODIndex(float distance)
        {
            for (int i = 0; i < lodLevels.Length; i++)
            {
                if (distance <= lodLevels[i].maxDistance)
                {
                    return i;
                }
            }
            return lodLevels.Length - 1; // Fallback to lowest LOD
        }

        // Apply smooth LOD transition
        private void ApplyLODTransition(TerrainChunk chunk, int chunkIndex, int targetLODIndex)
        {
            if (!currentLOD.ContainsKey(chunkIndex))
            {
                currentLOD[chunkIndex] = targetLODIndex;
            }

            int currentLODIndex = currentLOD[chunkIndex];

            // If already at target LOD, no transition needed
            if (currentLODIndex == targetLODIndex)
            {
                return;
            }

            // Determine direction of transition
            int direction = targetLODIndex > currentLODIndex ? 1 : -1;

            // Move toward target LOD based on smoothness
            if (ShouldTransitionLOD(currentLODIndex, targetLODIndex))
            {
                currentLODIndex = targetLODIndex;
                currentLOD[chunkIndex] = currentLODIndex;

                // Apply new LOD to chunk
                ApplyLODToChunk(chunk, currentLODIndex);
            }
        }

        // Determine if LOD transition should occur
        private bool ShouldTransitionLOD(int current, int target)
        {
            // Simple check - in production, could add hysteresis to prevent flickering
            return true;
        }

        // Apply LOD level to a specific chunk
        private void ApplyLODToChunk(TerrainChunk chunk, int lodIndex)
        {
            if (chunk == null || lodIndex < 0 || lodIndex >= lodLevels.Length)
            {
                return;
            }

            LODLevel level = lodLevels[lodIndex];

            // Apply resolution change
            chunk.SetLODResolution(level.resolution);

            // Apply material override if specified
            if (level.material != null)
            {
                chunk.SetMaterial(level.material);
            }

            // Enable/disable GPU instancing based on LOD level
            if (enableGPUInstancing)
            {
                chunk.EnableGPUInstancing(lodIndex < 2); // High LOD: no instancing, Low LOD: instancing
            }

            // Debug logging
            if (Debug.isDebugBuild)
            {
                // Optional: Log LOD transitions
                // Debug.Log($"LOD transition: Chunk {chunk.GetInstanceID()} -> {level.name}");
            }
        }

        // Manage chunk streaming (load/unload based on player position)
        private void ManageChunkStreaming()
        {
            if (player == null || !enableStreaming) return;

            Vector3 playerPosition = player.position;

            // Determine which chunks should be loaded
            HashSet<int> chunksToLoad = new HashSet<int>();

            for (int i = 0; i < maxActiveChunks; i++)
            {
                // Calculate chunk positions in a spiral around player
                Vector3 chunkPos = GetChunkPosition(playerPosition, i);
                int chunkIndex = GetChunkIndex(chunkPos);

                // Check if chunk is in frustum (if culling enabled)
                if (frustumCulling && !IsInFrustum(chunkPos))
                {
                    continue;
                }

                chunksToLoad.Add(chunkIndex);
            }

            // Load new chunks
            foreach (int chunkIndex in chunksToLoad)
            {
                if (!activeChunks.ContainsKey(chunkIndex))
                {
                    LoadChunk(chunkIndex);
                }
            }

            // Unload chunks that are no longer needed
            List<int> chunksToUnload = new List<int>();
            foreach (var kvp in activeChunks)
            {
                int chunkIndex = kvp.Key;

                if (!chunksToLoad.Contains(chunkIndex))
                {
                    chunksToUnload.Add(chunkIndex);
                }
            }

            foreach (int chunkIndex in chunksToUnload)
            {
                UnloadChunk(chunkIndex);
            }
        }

        // Enable/disable chunk streaming
        public bool enableStreaming = true;

        // Get chunk position for a given index (spiral pattern)
        private Vector3 GetChunkPosition(Vector3 center, int index)
        {
            // Simple spiral pattern around player
            int ring = Mathf.FloorToInt(Mathf.Sqrt(index));
            int ringIndex = index - ring * ring;

            float angle = (float)ringIndex / Mathf.Max(1, 2 * ring) * 2 * Mathf.PI;
            float distance = ring * 10; // 10m per chunk

            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * distance,
                0,
                Mathf.Sin(angle) * distance
            );

            return center + offset;
        }

        // Get unique chunk index from position
        private int GetChunkIndex(Vector3 position)
        {
            // Convert position to chunk coordinates
            int chunkX = Mathf.FloorToInt(position.x / 10);
            int chunkZ = Mathf.FloorToInt(position.z / 10);

            // Create unique index
            return chunkX * 10000 + chunkZ; // Simplified hashing
        }

        // Check if position is within camera frustum
        private bool IsInFrustum(Vector3 position)
        {
            if (mainCamera == null) return true;

            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

            // Create bounds for the chunk position
            Bounds bounds = new Bounds(position, Vector3.one * 10);

            return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
        }

        // Load a terrain chunk
        private void LoadChunk(int chunkIndex)
        {
            if (activeChunks.ContainsKey(chunkIndex))
            {
                return;
            }

            // Calculate chunk position
            Vector3 position = GetChunkPositionFromIndex(chunkIndex);

            // Create chunk (simplified - would use object pooling in production)
            GameObject chunkGO = new GameObject($"Chunk_{chunkIndex}");
            chunkGO.transform.position = position;

            // Add TerrainChunk component
            TerrainChunk chunk = chunkGO.AddComponent<TerrainChunk>();
            chunk.Initialize(this, chunkIndex, position);

            // Store in active chunks
            activeChunks[chunkIndex] = chunk;

            // Initialize LOD
            currentLOD[chunkIndex] = 0;
            targetLOD[chunkIndex] = 0;
        }

        // Get chunk position from index (inverse of GetChunkIndex)
        private Vector3 GetChunkPositionFromIndex(int chunkIndex)
        {
            // Reverse the hashing (simplified)
            int chunkX = chunkIndex / 10000;
            int chunkZ = chunkIndex % 10000;

            return new Vector3(chunkX * 10, 0, chunkZ * 10);
        }

        // Unload a terrain chunk
        private void UnloadChunk(int chunkIndex)
        {
            if (!activeChunks.ContainsKey(chunkIndex))
            {
                return;
            }

            TerrainChunk chunk = activeChunks[chunkIndex];

            if (chunk != null)
            {
                // Clean up chunk
                chunk.Cleanup();
                Destroy(chunk.gameObject);
            }

            // Remove from tracking
            activeChunks.Remove(chunkIndex);
            currentLOD.Remove(chunkIndex);
            targetLOD.Remove(chunkIndex);
        }

        // Update performance metrics
        private void UpdatePerformanceMetrics()
        {
            // Count draw calls (simplified - Unity Stats would be more accurate)
            CurrentDrawCalls = CalculateDrawCalls();

            // Estimate memory usage
            MemoryUsageMB = CalculateMemoryUsage();
        }

        // Calculate current draw calls
        private int CalculateDrawCalls()
        {
            int drawCalls = 0;

            foreach (var kvp in activeChunks)
            {
                TerrainChunk chunk = kvp.Value;
                if (chunk != null)
                {
                    drawCalls += chunk.GetDrawCallCount();
                }
            }

            return drawCalls;
        }

        // Estimate memory usage
        private float CalculateMemoryUsage()
        {
            // Rough estimate based on active chunks
            // In production, would use Unity's Profiler memory API
            return activeChunks.Count * 0.15f; // Approx 150KB per chunk
        }

        // Get chunk by index
        public TerrainChunk GetChunk(int chunkIndex)
        {
            return activeChunks.TryGetValue(chunkIndex, out TerrainChunk chunk) ? chunk : null;
        }

        // Get all active chunks
        public IEnumerable<TerrainChunk> GetAllChunks()
        {
            return activeChunks.Values;
        }

        // Force refresh all LODs
        public void ForceLODRefresh()
        {
            UpdateLODLevels();
        }

        // Clean up on destroy
        private void OnDestroy()
        {
            // Unload all chunks
            List<int> allChunkIndices = new List<int>(activeChunks.Keys);
            foreach (int chunkIndex in allChunkIndices)
            {
                UnloadChunk(chunkIndex);
            }
        }

        // Editor visualization
        private void OnDrawGizmos()
        {
            if (!Debug.isDebugBuild) return;

            // Visualize LOD boundaries
            if (lodLevels != null)
            {
                foreach (var level in lodLevels)
                {
                    if (level.maxDistance < float.MaxValue)
                    {
                        Gizmos.color = new Color(1, 1, 0, 0.3f);
                        Gizmos.DrawWireSphere(transform.position, level.maxDistance);
                    }
                }
            }

            // Visualize active chunks
            foreach (var kvp in activeChunks)
            {
                if (kvp.Value != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(kvp.Value.transform.position, Vector3.one * 10);
                }
            }
        }
    }
}