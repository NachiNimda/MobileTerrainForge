# API Usage Examples

**Version:** 1.0
**Last Updated:** 2026-02-19

---

## Overview

This document provides practical code examples for using Mobile Terrain Forge APIs. Examples range from basic setup to advanced implementations.

---

## Table of Contents

1. [Basic Setup](#basic-setup)
2. [Terrain Generation](#terrain-generation)
3. [LOD Configuration](#lod-configuration)
4. [Splatmap Optimization](#splatmap-optimization)
5. [Chunk Streaming](#chunk-streaming)
6. [Custom Tile Libraries](#custom-tile-libraries)
7. [Events and Callbacks](#events-and-callbacks)
8. [Debugging and Profiling](#debugging-and-profiling)
9. [Advanced Patterns](#advanced-patterns)

---

## Basic Setup

### Minimal Working Example

```csharp
using UnityEngine;
using MobileTerrainForge;

public class BasicTerrainSetup : MonoBehaviour
{
    void Start()
    {
        // Get or add solver component
        WFCSolver solver = gameObject.AddComponent<WFCSolver>();

        // Initialize with tile library
        solver.Initialize("MobileTerrainForge/Data/tile_library.json");

        // Configure grid
        solver.gridWidth = 32;
        solver.gridHeight = 32;

        // Generate terrain
        solver.Generate(12345);  // Use seed for reproducibility
    }
}
```

### Setup with All Components

```csharp
using UnityEngine;
using MobileTerrainForge;

public class CompleteTerrainSetup : MonoBehaviour
{
    public string tileLibraryPath = "MobileTerrainForge/Data/tile_library.json";

    private WFCSolver solver;
    private LODManager lodManager;
    private SplatmapOptimizer splatmapOptimizer;
    private TerrainChunk chunkManager;

    void Start()
    {
        // Add components
        solver = gameObject.AddComponent<WFCSolver>();
        lodManager = gameObject.AddComponent<LODManager>();
        splatmapOptimizer = gameObject.AddComponent<SplatmapOptimizer>();
        chunkManager = gameObject.AddComponent<TerrainChunk>();

        // Initialize solver
        solver.Initialize(tileLibraryPath);
        solver.gridWidth = 64;
        solver.gridHeight = 64;
        solver.enableRotations = true;
        solver.seed = 12345;

        // Configure LOD
        ConfigureLOD();

        // Configure splatmap optimization
        ConfigureSplatmap();

        // Configure chunk streaming
        ConfigureChunks();

        // Generate terrain
        solver.Generate();
    }

    void ConfigureLOD()
    {
        LODLevel[] mobileLOD = new LODLevel[]
        {
            new LODLevel { level = 0, switchDistance = 12, meshResolution = 25, textureResolution = 0.75f },
            new LODLevel { level = 1, switchDistance = 35, meshResolution = 13, textureResolution = 0.5f },
            new LODLevel { level = 2, switchDistance = 80, meshResolution = 7, textureResolution = 0.25f },
            new LODLevel { level = 3, switchDistance = 150, meshResolution = 5, textureResolution = 0.125f }
        };
        lodManager.ConfigureLODLevels(mobileLOD);
    }

    void ConfigureSplatmap()
    {
        splatmapOptimizer.SetCompressionFormat(TextureCompressionFormat.ASTC_6x6);
        splatmapOptimizer.SetQuality(50);
    }

    void ConfigureChunks()
    {
        chunkManager.chunkSize = 64;
        chunkManager.loadDistance = 200;
        chunkManager.unloadDistance = 250;
        chunkManager.updateInterval = 0.5f;
    }
}
```

---

## Terrain Generation

### Single Terrain Generation

```csharp
using UnityEngine;
using MobileTerrainForge;

public class SingleTerrainGenerator : MonoBehaviour
{
    public WFCSolver solver;

    void Start()
    {
        solver = GetComponent<WFCSolver>();
        GenerateNewTerrain();
    }

    public void GenerateNewTerrain()
    {
        // Use random seed for variety
        int randomSeed = Random.Range(0, int.MaxValue);
        solver.Generate(randomSeed);
        Debug.Log($"Generated terrain with seed: {randomSeed}");
    }

    public void RegenerateWithSpecificSeed(int seed)
    {
        solver.Generate(seed);
    }
}
```

### Chunked Terrain Generation

```csharp
using UnityEngine;
using MobileTerrainForge;

public class ChunkedTerrainGenerator : MonoBehaviour
{
    public WFCSolver solver;
    public TerrainChunk chunkManager;
    public int chunkSize = 16;

    void Start()
    {
        solver = GetComponent<WFCSolver>();
        chunkManager = GetComponent<TerrainChunk>();

        // Enable chunked generation
        solver.generateMode = WFCGenerateMode.Chunked;
        solver.chunkSize = chunkSize;
        solver.updateInterval = 0.1f;

        // Subscribe to chunk events
        chunkManager.OnChunkLoaded += OnChunkLoaded;

        // Start generation
        solver.Generate();
    }

    void OnChunkLoaded(Vector2Int gridPosition)
    {
        Debug.Log($"Chunk loaded at: {gridPosition}");
        // Add gameplay logic when chunk loads
    }
}
```

### Real-Time Terrain Updates

```csharp
using UnityEngine;
using MobileTerrainForge;

public class RealTimeTerrainUpdater : MonoBehaviour
{
    public WFCSolver solver;
    public float updateInterval = 5.0f;
    private float lastUpdateTime;

    void Update()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            UpdateTerrain();
            lastUpdateTime = Time.time;
        }
    }

    void UpdateTerrain()
    {
        // Regenerate specific area (e.g., around player)
        Vector3 playerPos = Camera.main.transform.position;
        Vector2Int centerTile = WorldToTile(playerPos);

        solver.UpdateArea(centerTile.x, centerTile.y, 10, 10);
    }

    Vector2Int WorldToTile(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / solver.tileSize),
            Mathf.FloorToInt(worldPos.z / solver.tileSize)
        );
    }
}
```

---

## LOD Configuration

### Platform-Specific LOD

```csharp
using UnityEngine;
using MobileTerrainForge;

public class PlatformSpecificLOD : MonoBehaviour
{
    public LODManager lodManager;

    void Start()
    {
        lodManager = GetComponent<LODManager>();
        ConfigureLODForPlatform();
    }

    void ConfigureLODForPlatform()
    {
        LODLevel[] levels;

        if (Application.platform == RuntimePlatform.Android)
        {
            // Conservative LOD for varied Android devices
            levels = new LODLevel[]
            {
                new LODLevel { level = 0, switchDistance = 10, meshResolution = 17, textureResolution = 0.5f },
                new LODLevel { level = 1, switchDistance = 25, meshResolution = 9, textureResolution = 0.25f },
                new LODLevel { level = 2, switchDistance = 50, meshResolution = 5, textureResolution = 0.125f }
            };
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // Higher quality for iOS
            levels = new LODLevel[]
            {
                new LODLevel { level = 0, switchDistance = 15, meshResolution = 25, textureResolution = 0.75f },
                new LODLevel { level = 1, switchDistance = 40, meshResolution = 13, textureResolution = 0.5f },
                new LODLevel { level = 2, switchDistance = 80, meshResolution = 7, textureResolution = 0.25f }
            };
        }
        else
        {
            // High quality for desktop
            levels = new LODLevel[]
            {
                new LODLevel { level = 0, switchDistance = 20, meshResolution = 33, textureResolution = 1.0f },
                new LODLevel { level = 1, switchDistance = 50, meshResolution = 17, textureResolution = 0.5f },
                new LODLevel { level = 2, switchDistance = 100, meshResolution = 9, textureResolution = 0.25f }
            };
        }

        lodManager.ConfigureLODLevels(levels);
    }
}
```

### Dynamic LOD Adjustment

```csharp
using UnityEngine;
using MobileTerrainForge;

public class DynamicLODAdjuster : MonoBehaviour
{
    public LODManager lodManager;
    public float targetFPS = 60.0f;
    public float adjustmentInterval = 2.0f;

    private float lastAdjustmentTime;

    void Update()
    {
        if (Time.time - lastAdjustmentTime > adjustmentInterval)
        {
            AdjustLODBasedOnPerformance();
            lastAdjustmentTime = Time.time;
        }
    }

    void AdjustLODBasedOnPerformance()
    {
        float currentFPS = 1.0f / Time.deltaTime;

        if (currentFPS < targetFPS * 0.8f)
        {
            // Performance is poor, reduce LOD quality
            ReduceLODQuality();
        }
        else if (currentFPS > targetFPS * 1.2f)
        {
            // Performance is good, increase LOD quality
            IncreaseLODQuality();
        }
    }

    void ReduceLODQuality()
    {
        LODLevel[] currentLevels = lodManager.GetLODLevels();
        foreach (var level in currentLevels)
        {
            level.switchDistance *= 0.9f;  // Switch earlier
            level.meshResolution = Mathf.Max(5, level.meshResolution - 2);
        }
        lodManager.ConfigureLODLevels(currentLevels);
        Debug.Log("Reduced LOD quality for performance");
    }

    void IncreaseLODQuality()
    {
        LODLevel[] currentLevels = lodManager.GetLODLevels();
        foreach (var level in currentLevels)
        {
            level.switchDistance *= 1.1f;  // Switch later
            level.meshResolution = Mathf.Min(33, level.meshResolution + 2);
        }
        lodManager.ConfigureLODLevels(currentLevels);
        Debug.Log("Increased LOD quality for better visuals");
    }
}
```

---

## Splatmap Optimization

### Basic Splatmap Optimization

```csharp
using UnityEngine;
using MobileTerrainForge;

public class SplatmapOptimization : MonoBehaviour
{
    public SplatmapOptimizer optimizer;

    void Start()
    {
        optimizer = GetComponent<SplatmapOptimizer>();

        // Load source splatmap
        Texture2D sourceSplatmap = Resources.Load<Texture2D>("Splatmaps/terrain_control");

        // Optimize
        Texture2D optimizedSplatmap = optimizer.OptimizeSplatmap(sourceSplatmap);

        // Apply to terrain
        ApplySplatmapToTerrain(optimizedSplatmap);

        // Log results
        SplatmapStats stats = optimizer.GetStats();
        Debug.Log($"Optimized splatmap: {stats.totalMemoryMB:F2}MB, {stats.compressionRatio:F1}x compression");
    }

    void ApplySplatmapToTerrain(Texture2D splatmap)
    {
        Terrain terrain = Terrain.activeTerrain;
        TerrainData terrainData = terrain.terrainData;

        // Set alphamap
        float[,,] alphamap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                Color pixel = splatmap.GetPixelBilinear(x / (float)terrainData.alphamapWidth, y / (float)terrainData.alphamapHeight);
                alphamap[x, y, 0] = pixel.r;
                alphamap[x, y, 1] = pixel.g;
                alphamap[x, y, 2] = pixel.b;
                alphamap[x, y, 3] = pixel.a;
            }
        }

        terrainData.SetAlphamaps(0, 0, alphamap);
    }
}
```

### Generate Splatmap from Heightmap

```csharp
using UnityEngine;
using MobileTerrainForge;

public class SplatmapGenerator : MonoBehaviour
{
    public SplatmapOptimizer optimizer;
    public TerrainLayer[] terrainLayers;

    public Texture2D GenerateSplatmapFromHeightmap(Texture2D heightmap)
    {
        int width = heightmap.width;
        int height = heightmap.height;
        Texture2D splatmap = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float heightValue = heightmap.GetPixel(x, y).r;
                Color splatColor = CalculateSplatColor(heightValue);
                splatmap.SetPixel(x, y, splatColor);
            }
        }

        splatmap.Apply();
        return optimizer.OptimizeSplatmap(splatmap);
    }

    Color CalculateSplatColor(float height)
    {
        // Simple height-based blending
        Color color = Color.black;

        if (height < 0.2f)
        {
            // Water
            color.r = 1.0f;  // Water layer
            color.g = 0.0f;
            color.b = 0.0f;
            color.a = 0.0f;
        }
        else if (height < 0.4f)
        {
            // Sand/Beach
            color.r = 0.0f;
            color.g = 1.0f;  // Sand layer
            color.b = 0.0f;
            color.a = 0.0f;
        }
        else if (height < 0.7f)
        {
            // Grass
            color.r = 0.0f;
            color.g = 0.0f;
            color.b = 1.0f;  // Grass layer
            color.a = 0.0f;
        }
        else
        {
            // Rock/Mountain
            color.r = 0.0f;
            color.g = 0.0f;
            color.b = 0.0f;
            color.a = 1.0f;  // Rock layer
        }

        // Smooth transitions
        color = SmoothBlend(height, color);
        return color;
    }

    Color SmoothBlend(float height, Color color)
    {
        // Add smoothing between transitions
        float blendAmount = 0.2f;

        if (height > 0.2f - blendAmount && height < 0.2f + blendAmount)
        {
            // Water to sand transition
            float t = (height - (0.2f - blendAmount)) / (2 * blendAmount);
            return Color.Lerp(new Color(1, 0, 0, 0), new Color(0, 1, 0, 0), t);
        }
        // Add other transitions...

        return color;
    }
}
```

---

## Chunk Streaming

### Basic Chunk Streaming

```csharp
using UnityEngine;
using MobileTerrainForge;

public class ChunkStreamer : MonoBehaviour
{
    public TerrainChunk chunkManager;
    public Transform player;

    void Update()
    {
        // Update chunk streaming based on player position
        chunkManager.UpdateStreaming(player);
    }
}
```

### Advanced Chunk Management

```csharp
using UnityEngine;
using MobileTerrainForge;
using System.Collections.Generic;

public class AdvancedChunkManager : MonoBehaviour
{
    public TerrainChunk chunkManager;
    public Transform player;
    public int preloadRadius = 2;

    private Dictionary<Vector2Int, TerrainChunk> loadedChunks = new Dictionary<Vector2Int, TerrainChunk>();

    void Start()
    {
        chunkManager.OnChunkLoaded += OnChunkLoaded;
        chunkManager.OnChunkUnloaded += OnChunkUnloaded;
    }

    void Update()
    {
        Vector2Int playerChunkPos = GetChunkPosition(player.position);
        ManageChunks(playerChunkPos);
    }

    void ManageChunks(Vector2Int centerChunk)
    {
        // Load chunks within view distance + preload radius
        for (int x = -preloadRadius; x <= preloadRadius; x++)
        {
            for (int y = -preloadRadius; y <= preloadRadius; y++)
            {
                Vector2Int chunkPos = new Vector2Int(centerChunk.x + x, centerChunk.y + y);
                EnsureChunkLoaded(chunkPos);
            }
        }

        // Unload distant chunks
        UnloadDistantChunks(centerChunk);
    }

    void EnsureChunkLoaded(Vector2Int chunkPos)
    {
        if (!loadedChunks.ContainsKey(chunkPos))
        {
            ChunkData chunkData = chunkManager.GetChunkData(chunkPos);
            chunkManager.LoadChunk(chunkData);
            loadedChunks[chunkPos] = chunkManager.GetChunkAt(chunkPos);
        }
    }

    void UnloadDistantChunks(Vector2Int centerChunk)
    {
        List<Vector2Int> chunksToUnload = new List<Vector2Int>();

        foreach (var kvp in loadedChunks)
        {
            Vector2Int chunkPos = kvp.Key;
            float distance = Vector2Int.Distance(centerChunk, chunkPos);

            if (distance > preloadRadius + 1)
            {
                chunksToUnload.Add(chunkPos);
            }
        }

        foreach (Vector2Int chunkPos in chunksToUnload)
        {
            chunkManager.UnloadChunk(chunkPos);
            loadedChunks.Remove(chunkPos);
        }
    }

    Vector2Int GetChunkPosition(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / (chunkManager.chunkSize * 32)),
            Mathf.FloorToInt(worldPos.z / (chunkManager.chunkSize * 32))
        );
    }

    void OnChunkLoaded(Vector2Int gridPosition)
    {
        Debug.Log($"Chunk loaded: {gridPosition}");
    }

    void OnChunkUnloaded(Vector2Int gridPosition)
    {
        Debug.Log($"Chunk unloaded: {gridPosition}");
    }
}
```

---

## Custom Tile Libraries

### Creating a Custom Tile

```csharp
using UnityEngine;
using MobileTerrainForge;

public class CustomTileCreator : MonoBehaviour
{
    public TileLibrary library;

    public TileData CreateCustomTile(string id, string name, TileType type)
    {
        TileData tile = new TileData
        {
            id = id,
            name = name,
            type = type,
            weight = 10,
            connectors = new Connectors
            {
                north = "flat",
                east = "flat",
                south = "flat",
                west = "flat"
            },
            heightmap = "custom_heightmap",
            variations = new string[0],
            tags = new string[] { "custom" }
        };

        return tile;
    }

    public void AddTileToLibrary(TileData tile)
    {
        // Add to library
        List<TileData> tiles = new List<TileData>(library.tiles);
        tiles.Add(tile);
        library.tiles = tiles.ToArray();

        // Add connector rules
        library.connectorRules[tile.connectors.north] = new string[] { tile.connectors.north, "flat", "slope_up" };

        Debug.Log($"Added tile: {tile.id}");
    }
}
```

### Programmatically Creating Tile Library

```csharp
using UnityEngine;
using MobileTerrainForge;
using System.Collections.Generic;

public class LibraryGenerator : MonoBehaviour
{
    public TileLibrary GenerateSimpleLibrary()
    {
        TileLibrary library = new TileLibrary
        {
            version = "1.0",
            format = "WFC-TILE-LIBRARY",
            gridSize = 3,
            rotationalSymmetry = true,
            supportedRotations = new int[] { 0, 90, 180, 270 },
            tiles = new TileData[0],
            connectorRules = new Dictionary<string, string[]>(),
            constraints = new Constraints
            {
                maxSameAdjacency = 4,
                minDiversity = 2,
                waterCoverage = new RangeFloat { min = 0.0f, max = 0.3f },
                terrainVariance = new RangeFloat { min = 0.1f, max = 0.5f },
                structureDensity = new RangeFloat { min = 0.0f, max = 0.1f }
            }
        };

        // Add tiles
        AddBasicTiles(library);

        return library;
    }

    void AddBasicTiles(TileLibrary library)
    {
        List<TileData> tiles = new List<TileData>();

        // Flat ground
        tiles.Add(new TileData
        {
            id = "FLAT",
            name = "Flat Ground",
            type = TileType.Ground,
            weight = 50,
            connectors = new Connectors { north = "flat", east = "flat", south = "flat", west = "flat" },
            heightmap = "flat",
            variations = new string[0],
            tags = new string[] { "ground" }
        });

        // Hill
        tiles.Add(new TileData
        {
            id = "HILL",
            name = "Gentle Hill",
            type = TileType.Terrain,
            weight = 30,
            connectors = new Connectors { north = "slope_up", east = "slope_up", south = "slope_down", west = "slope_down" },
            heightmap = "hill",
            variations = new string[0],
            tags = new string[] { "terrain" }
        });

        library.tiles = tiles.ToArray();

        // Add connector rules
        library.connectorRules["flat"] = new string[] { "flat", "slope_down" };
        library.connectorRules["slope_up"] = new string[] { "slope_up", "flat" };
        library.connectorRules["slope_down"] = new string[] { "slope_down", "flat" };
    }

    public void SaveLibrary(TileLibrary library, string path)
    {
        string json = JsonUtility.ToJson(library, true);
        System.IO.File.WriteAllText(path, json);
        Debug.Log($"Saved library to: {path}");
    }
}
```

---

## Events and Callbacks

### Terrain Generation Events

```csharp
using UnityEngine;
using MobileTerrainForge;

public class GenerationEventManager : MonoBehaviour
{
    public WFCSolver solver;

    void Start()
    {
        solver = GetComponent<WFCSolver>();

        // Subscribe to events
        solver.OnGenerationStart += OnGenerationStart;
        solver.OnGenerationComplete += OnGenerationComplete;
        solver.OnTilePlaced += OnTilePlaced;
        solver.OnProgress += OnProgress;
        solver.OnError += OnError;
    }

    void OnGenerationStart()
    {
        Debug.Log("Terrain generation started");
        // Show loading UI
        UIManager.ShowLoadingScreen();
    }

    void OnGenerationComplete()
    {
        Debug.Log("Terrain generation completed");
        // Hide loading UI
        UIManager.HideLoadingScreen();
        // Enable player movement
        PlayerController.EnableMovement();
    }

    void OnTilePlaced(int x, int y)
    {
        // Optional: Show placement visualization
        // Debug.Log($"Tile placed at: ({x}, {y})");
    }

    void OnProgress(float progress)
    {
        // Update progress bar
        UIManager.UpdateProgressBar(progress * 100);
    }

    void OnError(string errorMessage)
    {
        Debug.LogError($"Generation error: {errorMessage}");
        // Show error message to user
        UIManager.ShowError(errorMessage);
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        solver.OnGenerationStart -= OnGenerationStart;
        solver.OnGenerationComplete -= OnGenerationComplete;
        solver.OnTilePlaced -= OnTilePlaced;
        solver.OnProgress -= OnProgress;
        solver.OnError -= OnError;
    }
}
```

### LOD Transition Events

```csharp
using UnityEngine;
using MobileTerrainForge;

public class LODEventManager : MonoBehaviour
{
    public LODManager lodManager;

    void Start()
    {
        lodManager = GetComponent<LODManager>();

        lodManager.OnLODChanged += OnLODChanged;
        lodManager.OnChunkLODChanged += OnChunkLODChanged;
    }

    void OnLODChanged(int oldLOD, int newLOD)
    {
        Debug.Log($"LOD changed: {oldLOD} -> {newLOD}");

        // Update UI to show current LOD level
        UIManager.UpdateLODIndicator(newLOD);
    }

    void OnChunkLODChanged(int chunkIndex)
    {
        // Optional: Log specific chunk LOD changes
        Debug.Log($"Chunk {chunkIndex} LOD updated");
    }

    void OnDestroy()
    {
        lodManager.OnLODChanged -= OnLODChanged;
        lodManager.OnChunkLODChanged -= OnChunkLODChanged;
    }
}
```

---

## Debugging and Profiling

### Performance Profiler

```csharp
using UnityEngine;
using MobileTerrainForge;

public class TerrainProfiler : MonoBehaviour
{
    public WFCSolver solver;
    public LODManager lodManager;
    public SplatmapOptimizer optimizer;

    public float profilingInterval = 1.0f;
    private float lastProfileTime;

    void Update()
    {
        if (Time.time - lastProfileTime > profilingInterval)
        {
            ProfilePerformance();
            lastProfileTime = Time.time;
        }
    }

    void ProfilePerformance()
    {
        // Frame rate
        float fps = 1.0f / Time.deltaTime;

        // Memory
        long memory = System.GC.GetTotalMemory(false);
        float memoryMB = memory / (1024f * 1024f);

        // LOD
        int currentLOD = lodManager.GetCurrentLOD();

        // Splatmap stats
        SplatmapStats splatStats = optimizer.GetStats();

        // Log performance
        Debug.Log($"FPS: {fps:F1} | Memory: {memoryMB:F2}MB | LOD: {currentLOD}");

        // Check against targets
        if (fps < 30)
            Debug.LogWarning($"Low FPS: {fps:F1}");
        if (memoryMB > 15)
            Debug.LogWarning($"High memory: {memoryMB:F2}MB");
    }
}
```

### Debug Visualization

```csharp
using UnityEngine;
using MobileTerrainForge;

public class DebugVisualizer : MonoBehaviour
{
    public WFCSolver solver;
    public bool showConnectors = true;
    public bool showWeights = true;
    public bool showGrid = true;

    void OnDrawGizmos()
    {
        if (!showGrid) return;

        TileData[,] grid = solver.GetGrid();
        if (grid == null) return;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                TileData tile = grid[x, y];
                if (tile == null) continue;

                Vector3 position = new Vector3(x * solver.tileSize, 0, y * solver.tileSize);

                // Draw tile bounds
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(position, new Vector3(solver.tileSize, 1, solver.tileSize));

                // Draw connectors
                if (showConnectors)
                {
                    DrawConnectors(position, tile.connectors);
                }

                // Draw weight indicator
                if (showWeights)
                {
                    Gizmos.color = Color.yellow;
                    float height = tile.weight / 100.0f;
                    Gizmos.DrawLine(position, position + Vector3.up * height);
                }
            }
        }
    }

    void DrawConnectors(Vector3 center, Connectors connectors)
    {
        float halfSize = solver.tileSize / 2;

        // North connector
        Gizmos.color = Color.red;
        Gizmos.DrawLine(center + new Vector3(0, 0, -halfSize), center + new Vector3(0, 0, -halfSize - 5));

        // East connector
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(center + new Vector3(halfSize, 0, 0), center + new Vector3(halfSize + 5, 0, 0));

        // South connector
        Gizmos.color = Color.green;
        Gizmos.DrawLine(center + new Vector3(0, 0, halfSize), center + new Vector3(0, 0, halfSize + 5));

        // West connector
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(center + new Vector3(-halfSize, 0, 0), center + new Vector3(-halfSize - 5, 0, 0));
    }
}
```

---

## Advanced Patterns

### Singleton Terrain Manager

```csharp
using UnityEngine;
using MobileTerrainForge;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instance { get; private set; }

    public WFCSolver Solver { get; private set; }
    public LODManager LODManager { get; private set; }
    public SplatmapOptimizer SplatmapOptimizer { get; private set; }
    public TerrainChunk ChunkManager { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeComponents();
    }

    void InitializeComponents()
    {
        Solver = gameObject.AddComponent<WFCSolver>();
        LODManager = gameObject.AddComponent<LODManager>();
        SplatmapOptimizer = gameObject.AddComponent<SplatmapOptimizer>();
        ChunkManager = gameObject.AddComponent<TerrainChunk>();
    }

    public void GenerateTerrain(int seed)
    {
        Solver.Initialize("MobileTerrainForge/Data/tile_library.json");
        Solver.gridWidth = 64;
        Solver.gridHeight = 64;
        Solver.Generate(seed);
    }
}

// Usage elsewhere:
// TerrainManager.Instance.GenerateTerrain(12345);
```

### State Machine for Terrain Loading

```csharp
using UnityEngine;
using MobileTerrainForge;
using System.Collections;

public enum TerrainLoadingState
{
    Idle,
    Initializing,
    Generating,
    Optimizing,
    Streaming,
    Complete,
    Error
}

public class TerrainLoadingStateMachine : MonoBehaviour
{
    public WFCSolver solver;
    public TerrainLoadingState CurrentState { get; private set; }

    public void StartLoading(int seed)
    {
        StartCoroutine(LoadTerrainRoutine(seed));
    }

    IEnumerator LoadTerrainRoutine(int seed)
    {
        CurrentState = TerrainLoadingState.Initializing;
        yield return StartCoroutine(InitializeSolver());

        CurrentState = TerrainLoadingState.Generating;
        yield return StartCoroutine(GenerateTerrain(seed));

        CurrentState = TerrainLoadingState.Optimizing;
        yield return StartCoroutine(OptimizeTerrain());

        CurrentState = TerrainLoadingState.Streaming;
        yield return StartCoroutine(StartStreaming());

        CurrentState = TerrainLoadingState.Complete;
    }

    IEnumerator InitializeSolver()
    {
        solver.Initialize("MobileTerrainForge/Data/tile_library.json");
        solver.gridWidth = 64;
        solver.gridHeight = 64;
        yield return null;  // Wait one frame
    }

    IEnumerator GenerateTerrain(int seed)
    {
        solver.Generate(seed);
        while (solver.GetStatus() == SolverStatus.Generating)
        {
            yield return null;  // Wait until complete
        }

        if (solver.GetStatus() == SolverStatus.Failed)
        {
            CurrentState = TerrainLoadingState.Error;
            Debug.LogError("Terrain generation failed");
        }
    }

    IEnumerator OptimizeTerrain()
    {
        SplatmapOptimizer optimizer = GetComponent<SplatmapOptimizer>();
        optimizer.SetCompressionFormat(TextureCompressionFormat.ASTC_6x6);
        yield return null;
    }

    IEnumerator StartStreaming()
    {
        TerrainChunk chunkManager = GetComponent<TerrainChunk>();
        chunkManager.chunkSize = 64;
        yield return null;
    }
}
```

---

**Copyright © 2026 Mobile Terrain Forge. All rights reserved.**
