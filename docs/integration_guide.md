# Mobile Terrain Forge - Integration Guide

**Version:** 1.0
**Last Updated:** 2026-02-19
**Target:** Unity 2022.3 LTS and later

---

## Overview

Mobile Terrain Forge is a Wave Function Collapse (WFC)-based procedural terrain generation system optimized for mobile platforms. This guide covers importing, configuring, and deploying the system in your Unity project.

**Key Differentiator:** True WFC implementation with constraint-based tile placement, not noise-based generation like competitors.

---

## Quick Start

### 1. Import Source Files

Copy the following files to your Unity project:

```
Assets/MobileTerrainForge/
├── Scripts/
│   ├── WFCSolver.cs          (1,100 lines - Core WFC algorithm)
│   ├── LODManager.cs         (1,550 lines - Level of Detail system)
│   ├── SplatmapOptimizer.cs  (1,820 lines - Texture optimization)
│   ├── TerrainChunk.cs       (1,310 lines - Chunk management)
│   └── Editor/
│       └── TerrainEditorWindow.cs  (800 lines - Editor tools)
├── Data/
│   ├── tile_library.json     (Sample tile definitions)
│   ├── connector_constraints.csv
│   └── heightmaps/          (Sample heightmap data)
└── Resources/
    └── Tiles/               (Tile prefabs and textures)
```

### 2. Verify Unity Settings

**Player Settings (File > Build Settings > Player Settings):**

- **Graphics API:** OpenGL ES 3.0+ (Android), Metal (iOS)
- **Scripting Backend:** IL2CPP (recommended for mobile)
- **API Compatibility Level:** .NET Standard 2.1
- **Texture Compression:** ASTC (Android), PVRTC (iOS)

### 3. Create Initial Terrain

1. Create a new GameObject: `GameObject > Create Empty`
2. Add `WFCSolver` component
3. Load tile library: Assign `tile_library.json` to the solver
4. Configure grid size (e.g., 32x32 tiles)
5. Click "Generate" in Inspector

---

## Core Components

### WFCSolver

The constraint-based wave function collapse algorithm.

**Public API:**

```csharp
public class WFCSolver : MonoBehaviour
{
    // Initialize solver with tile library
    public void Initialize(string libraryPath);

    // Generate terrain with specified seed
    public void Generate(int seed = -1);

    // Get generated grid data
    public TileData[,] GetGrid();

    // Real-time update mode (for chunked generation)
    public void SetUpdateMode(WFCUpdateMode mode);
}
```

**Configuration Parameters:**

| Parameter | Type | Default | Range | Description |
|-----------|------|---------|-------|-------------|
| gridWidth | int | 32 | 1-256 | Number of tiles horizontally |
| gridHeight | int | 32 | 1-256 | Number of tiles vertically |
| enableRotations | bool | true | - | Allow 90° tile rotations |
| enableReflections | bool | false | - | Mirror tiles |
| maxIterations | int | 1000 | 100-5000 | Solver timeout iterations |
| seed | int | -1 | - | Random seed (-1 = random) |

### LODManager

Manages level of detail for performance optimization.

**Public API:**

```csharp
public class LODManager : MonoBehaviour
{
    // Set LOD levels configuration
    public void ConfigureLODLevels(LODLevel[] levels);

    // Update LOD based on camera position
    public void UpdateLOD(Transform camera);

    // Force specific LOD for testing
    public void ForceLOD(int level);
}
```

**LOD Configuration:**

```csharp
public class LODLevel
{
    public int level;              // 0 = highest, 3 = lowest
    public float switchDistance;   // Camera distance for switch
    public int meshResolution;     // Vertices per tile
    public float textureResolution; // Texture scale multiplier
}

// Default mobile configuration
LODLevel[] defaultLOD = new LODLevel[]
{
    new LODLevel { level = 0, switchDistance = 10, meshResolution = 33, textureResolution = 1.0f },
    new LODLevel { level = 1, switchDistance = 30, meshResolution = 17, textureResolution = 0.5f },
    new LODLevel { level = 2, switchDistance = 60, meshResolution = 9, textureResolution = 0.25f },
    new LODLevel { level = 3, switchDistance = 100, meshResolution = 5, textureResolution = 0.125f }
};
```

### SplatmapOptimizer

Compresses and optimizes terrain textures for mobile.

**Public API:**

```csharp
public class SplatmapOptimizer : MonoBehaviour
{
    // Optimize splatmap textures
    public Texture2D OptimizeSplatmap(Texture2D source);

    // Set compression format
    public void SetCompressionFormat(TextureCompressionFormat format);

    // Get memory usage stats
    public SplatmapStats GetStats();
}
```

**Compression Formats:**

- **ASTC 6x6:** Best quality/size balance (recommended)
- **ASTC 4x4:** Higher quality, larger size
- **ETC2:** Legacy format, larger size
- **PVRTC 4bpp:** iOS-specific, moderate quality

### TerrainChunk

Manages individual terrain chunks for streaming and occlusion.

**Public API:**

```csharp
public class TerrainChunk : MonoBehaviour
{
    // Load chunk data
    public void LoadChunk(ChunkData data);

    // Unload and release memory
    public void UnloadChunk();

    // Check if chunk is visible
    public bool IsVisible(Vector3 cameraPosition, float viewDistance);

    // Force rebuild chunk mesh
    public void RebuildMesh();
}
```

---

## Tile Library Format

### JSON Structure

```json
{
  "version": "1.0",
  "format": "WFC-TILE-LIBRARY",
  "gridSize": 5,
  "rotationalSymmetry": true,
  "supportedRotations": [0, 90, 180, 270],
  "tiles": [
    {
      "id": "T001",
      "name": "Flat Ground",
      "type": "ground",
      "weight": 50,
      "connectors": {
        "north": "flat",
        "east": "flat",
        "south": "flat",
        "west": "flat"
      },
      "heightmap": "flat",
      "variations": ["T001_var0", "T001_var1", "T001_var2"],
      "tags": ["base", "ground", "walkable"]
    }
  ],
  "connectorRules": {
    "flat": ["flat", "slope_down", "sand", "grass", "forest", "road", "structure"]
  },
  "constraints": {
    "maxSameAdjacency": 4,
    "minDiversity": 3,
    "waterCoverage": {"min": 0.05, "max": 0.3},
    "terrainVariance": {"min": 0.1, "max": 0.7},
    "structureDensity": {"min": 0, "max": 0.15}
  }
}
```

### Creating Custom Tiles

1. **Define Connector Types:**
   - Each tile has 4 connectors (N, E, S, W)
   - Connectors must match between adjacent tiles
   - Use descriptive names: `flat`, `slope_up`, `water_flow`, `cliff_side`

2. **Set Weight:**
   - Higher weight = more frequent placement
   - Recommended range: 1-100
   - Balance based on desired terrain distribution

3. **Add Variations:**
   - Visual variations prevent repetition
   - 3-6 variations per tile type recommended
   - Must share same connector configuration

4. **Define Tags:**
   - Used for gameplay logic (walkable, impassable, water)
   - Helpful for collision detection and navigation

---

## Performance Optimization

### 1. Enable LOD System

```csharp
// Configure LOD for mobile
LODManager lodManager = GetComponent<LODManager>();
LODLevel[] mobileLOD = new LODLevel[]
{
    new LODLevel { level = 0, switchDistance = 15, meshResolution = 25, textureResolution = 0.75f },
    new LODLevel { level = 1, switchDistance = 40, meshResolution = 13, textureResolution = 0.5f },
    new LODLevel { level = 2, switchDistance = 80, meshResolution = 7, textureResolution = 0.25f }
};
lodManager.ConfigureLODLevels(mobileLOD);
```

### 2. Optimize Splatmaps

```csharp
SplatmapOptimizer optimizer = GetComponent<SplatmapOptimizer>();
optimizer.SetCompressionFormat(TextureCompressionFormat.ASTC_6x6);
Texture2D optimized = optimizer.OptimizeSplatmap(sourceSplatmap);
```

### 3. Chunk Streaming

```csharp
// Configure chunk streaming
TerrainChunk chunk = GetComponent<TerrainChunk>();
chunk.chunkSize = 64;  // tiles per chunk
chunk.loadDistance = 200;  // units
chunk.unloadDistance = 250;  // units
```

### 4. Batch Draw Calls

- Use **GPU Instancing** for repeated tiles
- Combine similar terrain materials
- Limit unique materials per scene to <8

---

## Scripting Examples

### Basic Terrain Generation

```csharp
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public WFCSolver solver;
    public int gridWidth = 32;
    public int gridHeight = 32;

    void Start()
    {
        solver = GetComponent<WFCSolver>();
        solver.Initialize("Assets/MobileTerrainForge/Data/tile_library.json");
        solver.gridWidth = gridWidth;
        solver.gridHeight = gridHeight;
        solver.Generate(12345);  // Seed for reproducibility
    }
}
```

### Real-Time Chunk Updates

```csharp
public class ChunkManager : MonoBehaviour
{
    public WFCSolver solver;
    public TerrainChunk[] chunks;
    public Transform player;
    public float updateInterval = 1.0f;

    void Start()
    {
        InvokeRepeating(nameof(UpdateChunks), 0, updateInterval);
    }

    void UpdateChunks()
    {
        foreach (var chunk in chunks)
        {
            if (chunk.IsVisible(player.position, 100))
            {
                if (!chunk.IsLoaded)
                    chunk.LoadChunk(solver.GetChunkData(chunk.position));
            }
            else
            {
                if (chunk.IsLoaded)
                    chunk.UnloadChunk();
            }
        }
    }
}
```

### Custom Connector Rules

```csharp
public class CustomConnectorRules : MonoBehaviour
{
    public WFCSolver solver;

    void Start()
    {
        // Add custom connector compatibility
        solver.AddConnectorRule("my_connector", new[] {
            "flat", "slope_up", "water_shore", "my_connector"
        });

        // Remove default rule
        solver.RemoveConnectorRule("cliff_top", "steep_down");
    }
}
```

---

## Debugging and Profiling

### Enable Debug Visualization

```csharp
// Show connector types in scene view
WFCSolver solver = GetComponent<WFCSolver>();
solver.debugMode = true;
solver.showConnectors = true;
solver.showWeights = true;
```

### Profile Memory Usage

```csharp
// Get memory statistics
SplatmapStats stats = GetComponent<SplatmapOptimizer>().GetStats();
Debug.Log($"Splatmap Memory: {stats.totalMemoryMB} MB");
Debug.Log($"Draw Calls: {stats.drawCalls}");
Debug.Log($"LOD Level: {stats.currentLOD}");
```

---

## Common Integration Issues

### Issue: Tiles don't connect properly

**Solution:** Verify connector types match in `connectorRules`. Use debug mode to visualize connectors.

### Issue: Low framerate on mobile

**Solution:**
1. Reduce LOD switch distances
2. Increase chunk size (fewer chunks = fewer draw calls)
3. Enable texture compression (ASTC)
4. Reduce mesh resolution in LOD levels

### Issue: Memory exceeds 15MB limit

**Solution:**
1. Use ASTC 6x6 compression for textures
2. Unload distant chunks
3. Reduce splatmap resolution
4. Use texture atlasing for tile variations

### Issue: Solver fails to converge

**Solution:**
1. Increase `maxIterations`
2. Relax constraint rules
3. Add more tile variations
4. Check for contradictory connector rules

---

## Platform-Specific Notes

### Android

- **Minimum SDK:** API 24 (Android 7.0)
- **Recommended:** API 30+ (Android 11)
- **Graphics API:** OpenGL ES 3.1+, Vulkan
- **Compression:** ASTC 6x6

### iOS

- **Minimum:** iOS 12.0
- **Recommended:** iOS 15+
- **Graphics API:** Metal
- **Compression:** ASTC 6x6 (or PVRTC 4bpp fallback)

---

## Next Steps

1. Review `TECHNICAL_SPEC.md` for detailed API documentation
2. Run benchmark tests using `BENCHMARK_PLAN.md`
3. Check `TROUBLESHOOTING.md` for common issues
4. Explore sample scene in `Assets/MobileTerrainForge/Scenes/`

---

## Support

For issues, feature requests, or questions:
- Review this guide and troubleshooting documentation
- Check debug output in Unity Console
- Profile with Unity Profiler

---

**Copyright © 2026 Mobile Terrain Forge. All rights reserved.**
