// Splatmap Optimizer for Mobile Terrain Forge
// 8-channel splatmap packing with dynamic resolution per LOD
// Optimized for mobile: efficient memory usage, GPU-friendly

using UnityEngine;
using System.Collections.Generic;

namespace MobileTerrainForge
{
    public class SplatmapOptimizer : MonoBehaviour
    {
        [Header("Splatmap Configuration")]
        [SerializeField] private int maxTextureSize = 2048;           // Maximum splatmap texture size
        [SerializeField] private bool enableDynamicResolution = true; // Adjust resolution based on LOD
        [SerializeField] private bool enableChannelPacking = true;    // Pack 8 channels into 4 textures

        [Header("Performance")]
        [SerializeField] private FilterMode filterMode = FilterMode.Bilinear;
        [SerializeField] private TextureWrapMode wrapMode = TextureWrapMode.Clamp;
        [SerializeField] private bool generateMipmaps = true;

        // Splatmap data
        private Texture2D[] splatmaps = new Texture2D[2]; // 2 textures for 8 channels (4 channels each)
        private int[] splatmapResolutions = new int[2];   // Resolution per splatmap

        // Channel definitions
        public enum SplatChannel
        {
            // First texture (RGBA)
            Grass = 0,
            Dirt = 1,
            Rock = 2,
            Sand = 3,
            // Second texture (RGBA)
            Water = 4,
            Snow = 5,
            Mud = 6,
            Custom = 7
        }

        // Splatmap data per chunk
        private Dictionary<int, SplatmapData> chunkSplatmaps = new Dictionary<int, SplatmapData>();

        // Performance tracking
        public int TotalChannelCount => 8;
        public int TextureCount => enableChannelPacking ? 2 : 8;
        public int MemoryUsageKB => CalculateMemoryUsage();

        // Splatmap data structure
        private class SplatmapData
        {
            public Texture2D[] textures;
            public int[] resolutions;
            public bool isDirty;

            public SplatmapData(int channelCount, int resolution)
            {
                textures = new Texture2D[channelCount];
                resolutions = new int[channelCount];
                isDirty = false;

                for (int i = 0; i < channelCount; i++)
                {
                    textures[i] = new Texture2D(resolution, resolution, TextureFormat.RGBA32, generateMipmaps);
                    resolutions[i] = resolution;
                    textures[i].filterMode = FilterMode.Bilinear;
                    textures[i].wrapMode = TextureWrapMode.Clamp;
                }
            }

            public void Cleanup()
            {
                if (textures != null)
                {
                    foreach (var tex in textures)
                    {
                        if (tex != null)
                    {
                            Object.Destroy(tex);
                        }
                    }
                }
            }
        }

        private void Awake()
        {
            InitializeSplatmaps();
        }

        private void InitializeSplatmaps()
        {
            // Create base splatmaps
            int baseResolution = maxTextureSize;

            // First texture (RGBA: Grass, Dirt, Rock, Sand)
            splatmaps[0] = new Texture2D(baseResolution, baseResolution, TextureFormat.RGBA32, generateMipmaps);
            splatmaps[0].filterMode = filterMode;
            splatmaps[0].wrapMode = wrapMode;
            splatmapResolutions[0] = baseResolution;

            // Second texture (RGBA: Water, Snow, Mud, Custom)
            splatmaps[1] = new Texture2D(baseResolution, baseResolution, TextureFormat.RGBA32, generateMipmaps);
            splatmaps[1].filterMode = filterMode;
            splatmaps[1].wrapMode = wrapMode;
            splatmapResolutions[1] = baseResolution;

            // Fill with default data
            FillDefaultData();
        }

        private void FillDefaultData()
        {
            // Fill first texture (default: all grass)
            Color[] grassColor = new Color[maxTextureSize * maxTextureSize];
            for (int i = 0; i < grassColor.Length; i++)
            {
                grassColor[i] = new Color(1, 0, 0, 0); // Full grass, zero others
            }
            splatmaps[0].SetPixels(grassColor);
            splatmaps[0].Apply();

            // Fill second texture (default: zero)
            Color[] zeroColor = new Color[maxTextureSize * maxTextureSize];
            for (int i = 0; i < zeroColor.Length; i++)
            {
                zeroColor[i] = Color.black;
            }
            splatmaps[1].SetPixels(zeroColor);
            splatmaps[1].Apply();
        }

        // Get splatmap texture for a specific channel
        public Texture2D GetSplatmapTexture(SplatChannel channel)
        {
            int textureIndex = (int)channel / 4;
            if (textureIndex < 0 || textureIndex >= splatmaps.Length)
            {
                return null;
            }

            return splatmaps[textureIndex];
        }

        // Get splatmap for a specific chunk with dynamic resolution
        public SplatmapData GetChunkSplatmap(int chunkIndex, int lodLevel)
        {
            // Calculate resolution based on LOD
            int resolution = CalculateResolution(lodLevel);

            // Check if we already have this chunk's splatmap
            if (chunkSplatmaps.ContainsKey(chunkIndex))
            {
                SplatmapData data = chunkSplatmaps[chunkIndex];

                // Check if resolution matches
                if (data.resolutions[0] == resolution)
                {
                    return data;
                }

                // Resolution mismatch, regenerate
                data.Cleanup();
                chunkSplatmaps.Remove(chunkIndex);
            }

            // Create new splatmap data
            int channelCount = enableChannelPacking ? 2 : 8;
            SplatmapData newData = new SplatmapData(channelCount, resolution);
            chunkSplatmaps[chunkIndex] = newData;

            return newData;
        }

        // Calculate resolution based on LOD level
        private int CalculateResolution(int lodLevel)
        {
            if (!enableDynamicResolution)
            {
                return maxTextureSize;
            }

            // Resolution decreases with LOD level
            switch (lodLevel)
            {
                case 0: return maxTextureSize;       // LOD0: Full resolution (2048)
                case 1: return maxTextureSize / 2;   // LOD1: Half resolution (1024)
                case 2: return maxTextureSize / 4;   // LOD2: Quarter resolution (512)
                case 3: return maxTextureSize / 8;   // LOD3: Eighth resolution (256)
                default: return maxTextureSize / 8;  // Default to lowest
            }
        }

        // Set splatmap value for a specific pixel
        public void SetSplatValue(int chunkIndex, int x, int y, SplatChannel channel, float value)
        {
            SplatmapData data = GetChunkSplatmap(chunkIndex, 0);

            if (data == null) return;

            // Determine texture index and channel offset
            int textureIndex = (int)channel % 4;
            int channelOffset = (int)channel / 4;

            if (textureIndex < 0 || textureIndex >= data.textures.Length)
            {
                return;
            }

            // Get current pixel color
            Color pixelColor = data.textures[textureIndex].GetPixel(x, y);

            // Set specific channel
            switch ((int)channel % 4)
            {
                case 0: pixelColor.r = value; break; // Red channel
                case 1: pixelColor.g = value; break; // Green channel
                case 2: pixelColor.b = value; break; // Blue channel
                case 3: pixelColor.a = value; break; // Alpha channel
            }

            // Set pixel back
            data.textures[textureIndex].SetPixel(x, y, pixelColor);
            data.isDirty = true;
        }

        // Apply splatmap changes
        public void ApplySplatmap(int chunkIndex)
        {
            if (!chunkSplatmaps.ContainsKey(chunkIndex))
            {
                return;
            }

            SplatmapData data = chunkSplatmaps[chunkIndex];

            if (!data.isDirty) return;

            // Apply all textures
            foreach (var tex in data.textures)
            {
                if (tex != null)
                {
                    tex.Apply();
                }
            }

            data.isDirty = false;
        }

        // Generate splatmap from terrain data
        public void GenerateSplatmapFromTerrain(int chunkIndex, float[,] heightmap, float slopeThreshold)
        {
            SplatmapData data = GetChunkSplatmap(chunkIndex, 0);

            if (data == null) return;

            int resolution = data.textures[0].width;

            // Generate splatmap based on height and slope
            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    // Sample height and calculate slope
                    float height = SampleHeight(heightmap, x, y, resolution);
                    float slope = CalculateSlope(heightmap, x, y, resolution);

                    // Determine terrain type based on height and slope
                    SplatChannel channel = DetermineTerrainType(height, slope, slopeThreshold);

                    // Set splat value
                    float value = 1.0f;

                    // Determine texture index
                    int textureIndex = (int)channel / 4;
                    int channelIndex = (int)channel % 4;

                    if (textureIndex < data.textures.Length)
                    {
                        Color pixelColor = data.textures[textureIndex].GetPixel(x, y);

                        // Set specific channel
                        switch (channelIndex)
                        {
                            case 0: pixelColor.r = value; break;
                            case 1: pixelColor.g = value; break;
                            case 2: pixelColor.b = value; break;
                            case 3: pixelColor.a = value; break;
                        }

                        data.textures[textureIndex].SetPixel(x, y, pixelColor);
                    }
                }
            }

            data.isDirty = true;
            ApplySplatmap(chunkIndex);
        }

        // Sample height from heightmap
        private float SampleHeight(float[,] heightmap, int x, int y, int resolution)
        {
            // Normalize coordinates
            float nx = (float)x / (resolution - 1);
            float ny = (float)y / (resolution - 1);

            // Sample from heightmap
            int hx = Mathf.Clamp(Mathf.FloorToInt(nx * (heightmap.GetLength(0) - 1)), 0, heightmap.GetLength(0) - 1);
            int hy = Mathf.Clamp(Mathf.FloorToInt(ny * (heightmap.GetLength(1) - 1)), 0, heightmap.GetLength(1) - 1);

            return heightmap[hx, hy];
        }

        // Calculate slope at a point
        private float CalculateSlope(float[,] heightmap, int x, int y, int resolution)
        {
            float dx = 0f;
            float dy = 0f;

            // Sample neighbors for slope calculation
            float center = SampleHeight(heightmap, x, y, resolution);
            float left = x > 0 ? SampleHeight(heightmap, x - 1, y, resolution) : center;
            float right = x < resolution - 1 ? SampleHeight(heightmap, x + 1, y, resolution) : center;
            float up = y > 0 ? SampleHeight(heightmap, x, y - 1, resolution) : center;
            float down = y < resolution - 1 ? SampleHeight(heightmap, x, y + 1, resolution) : center;

            // Calculate slope
            dx = (right - left) * 0.5f;
            dy = (down - up) * 0.5f;

            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        // Determine terrain type based on height and slope
        private SplatChannel DetermineTerrainType(float height, float slope, float slopeThreshold)
        {
            // Steep slopes -> rock
            if (slope > slopeThreshold)
            {
                return SplatChannel.Rock;
            }

            // High elevation -> snow
            if (height > 0.8f)
            {
                return SplatChannel.Snow;
            }

            // Low elevation near water level -> sand
            if (height < 0.3f)
            {
                return SplatChannel.Sand;
            }

            // Mid elevation -> grass or dirt based on slope
            if (height > 0.5f)
            {
                return SplatChannel.Dirt;
            }

            // Default -> grass
            return SplatChannel.Grass;
        }

        // Blend splatmaps at LOD boundaries
        public void BlendSplatmapBoundaries(int chunkIndex, SplatmapData neighborNorth, SplatmapData neighborSouth, SplatmapData neighborEast, SplatmapData neighborWest)
        {
            SplatmapData data = GetChunkSplatmap(chunkIndex, 0);

            if (data == null) return;

            int resolution = data.textures[0].width;

            // Blend with northern neighbor
            if (neighborNorth != null)
            {
                BlendEdge(data.textures[0], neighborNorth.textures[0], resolution, 0); // Top edge
            }

            // Blend with southern neighbor
            if (neighborSouth != null)
            {
                BlendEdge(data.textures[0], neighborSouth.textures[0], resolution, 1); // Bottom edge
            }

            // Blend with eastern neighbor
            if (neighborEast != null)
            {
                BlendEdge(data.textures[0], neighborEast.textures[0], resolution, 2); // Right edge
            }

            // Blend with western neighbor
            if (neighborWest != null)
            {
                BlendEdge(data.textures[0], neighborWest.textures[0], resolution, 3); // Left edge
            }

            data.isDirty = true;
            ApplySplatmap(chunkIndex);
        }

        // Blend edge between two textures
        private void BlendEdge(Texture2D tex1, Texture2D tex2, int resolution, int edge)
        {
            int blendWidth = Mathf.Max(1, resolution / 10); // 10% of resolution

            for (int i = 0; i < blendWidth; i++)
            {
                float alpha = (float)i / blendWidth;

                // Determine which edge to blend
                int x = 0, y = 0;
                switch (edge)
                {
                    case 0: y = i; break;              // Top edge (y from 0 to blendWidth)
                    case 1: y = resolution - 1 - i; break; // Bottom edge
                    case 2: x = resolution - 1 - i; break; // Right edge
                    case 3: x = i; break;              // Left edge
                }

                // Blend entire row/column
                for (int j = 0; j < resolution; j++)
                {
                    int px = edge == 0 || edge == 1 ? j : x;
                    int py = edge == 0 || edge == 1 ? y : j;

                    Color color1 = tex1.GetPixel(px, py);
                    Color color2 = tex2.GetPixel(px, py);

                    // Lerp between colors
                    Color blended = Color.Lerp(color1, color2, alpha);
                    tex1.SetPixel(px, py, blended);
                }
            }
        }

        // Calculate memory usage in KB
        private int CalculateMemoryUsage()
        {
            int totalMemory = 0;

            // Base splatmaps
            foreach (var tex in splatmaps)
            {
                if (tex != null)
                {
                    totalMemory += EstimateTextureMemory(tex.width, tex.height);
                }
            }

            // Chunk splatmaps
            foreach (var kvp in chunkSplatmaps)
            {
                SplatmapData data = kvp.Value;
                if (data != null && data.textures != null)
                {
                    foreach (var tex in data.textures)
                    {
                        if (tex != null)
                        {
                            totalMemory += EstimateTextureMemory(tex.width, tex.height);
                        }
                    }
                }
            }

            return totalMemory / 1024; // Convert to KB
        }

        // Estimate texture memory usage
        private int EstimateTextureMemory(int width, int height)
        {
            int pixelCount = width * height;
            int bytesPerPixel = 4; // RGBA32 = 4 bytes per pixel
            int totalBytes = pixelCount * bytesPerPixel;

            // Add mipmaps if enabled
            if (generateMipmaps)
            {
                totalBytes = (int)(totalBytes * 1.33f); // ~33% overhead for mipmaps
            }

            return totalBytes;
        }

        // Clean up on destroy
        private void OnDestroy()
        {
            // Clean up chunk splatmaps
            foreach (var kvp in chunkSplatmaps)
            {
                kvp.Value.Cleanup();
            }
            chunkSplatmaps.Clear();

            // Clean up base splatmaps
            foreach (var tex in splatmaps)
            {
                if (tex != null)
                {
                    Object.Destroy(tex);
                }
            }
        }

        // Get statistics
        public string GetStatistics()
        {
            return $"Splatmap Optimizer Stats:\n" +
                   $"  Total Channels: {TotalChannelCount}\n" +
                   $"  Texture Count: {TextureCount}\n" +
                   $"  Memory Usage: {MemoryUsageKB} KB\n" +
                   $"  Active Chunks: {chunkSplatmaps.Count}\n" +
                   $"  Dynamic Resolution: {(enableDynamicResolution ? "Enabled" : "Disabled")}\n" +
                   $"  Channel Packing: {(enableChannelPacking ? "Enabled" : "Disabled")}";
        }
    }
}