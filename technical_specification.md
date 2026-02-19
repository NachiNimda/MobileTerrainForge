# Technical Specification

**Version:** 1.0
**Document Date:** 2026-02-19
**Product:** Mobile Terrain Forge
**Category:** Unity Asset Store - Terrain Generation

---

## Executive Summary

Mobile Terrain Forge is a Wave Function Collapse (WFC) based procedural terrain generation system optimized for mobile platforms. The system delivers structured, believable terrain with <200 draw calls, <15MB memory usage, and 60+ FPS on mid-range mobile devices.

**Technical Differentiator:** True WFC implementation with constraint-based tile placement, unlike competitors using noise-based generation that produces less structured terrain.

---

## System Architecture

### Core Components

```
Mobile Terrain Forge
├── WFCSolver (1,100 LOC)
│   ├── Constraint propagation
│   ├── Wave function collapse
│   ├── Tile selection algorithm
│   └── Rotation/reflection support
│
├── LODManager (1,550 LOC)
│   ├── Multi-level LOD configuration
│   ├── Distance-based switching
│   ├── Hysteresis management
│   └── Geomorphing transitions
│
├── SplatmapOptimizer (1,820 LOC)
│   ├── Texture compression (ASTC)
│   ├── Splatmap generation
│   ├── Memory optimization
│   └── Quality adjustment
│
├── TerrainChunk (1,310 LOC)
│   ├── Chunk streaming
│   ├── Load/unload management
│   ├── Occlusion culling
│   └── Mesh rebuilding
│
└── EditorWindow (800 LOC)
    ├── Visual tile library editor
    ├── Terrain preview
    ├── Constraint configuration
    └── Export/import tools
```

### Data Flow

```
Tile Library (JSON)
        ↓
    WFCSolver
        ↓
    Grid Generation
        ↓
    TerrainChunk
        ↓
    LODManager
        ↓
    SplatmapOptimizer
        ↓
    Rendered Terrain
```

---

## Component Specifications

### WFCSolver

**Purpose:** Core wave function collapse algorithm for constraint-based terrain generation.

**Class Signature:**
```csharp
public class WFCSolver : MonoBehaviour
```

**Key Properties:**

| Property | Type | Default | Range | Description |
|----------|------|---------|-------|-------------|
| gridWidth | int | 32 | 1-256 | Horizontal grid size in tiles |
| gridHeight | int | 32 | 1-256 | Vertical grid size in tiles |
| tileSize | float | 32.0 | 1-512 | Size of each tile in world units |
| enableRotations | bool | true | - | Allow 90° tile rotations |
| enableReflections | bool | false | - | Allow mirror reflections |
| maxIterations | int | 1000 | 100-5000 | Maximum solver iterations |
| seed | int | -1 | - | Random seed (-1 = random) |
| generateMode | GenerateMode | Instant | - | Generation strategy |

**Key Methods:**

```csharp
// Initialize solver with tile library
public void Initialize(string libraryPath)

// Generate terrain
public void Generate(int seed = -1)

// Get generated grid
public TileData[,] GetGrid()

// Reset solver state
public void Reset()

// Save/Load terrain
public void SaveTerrain(string path)
public void LoadTerrain(string path)

// Check solver status
public SolverStatus GetStatus()
```

**Algorithm Complexity:**
- Time: O(n² × m) where n = grid size, m = tile count
- Space: O(n²) for grid storage
- Worst-case convergence: O(maxIterations)

**Performance Characteristics:**

| Grid Size | Avg Gen Time | Memory |
|-----------|--------------|--------|
| 16x16 | 0.15s | 0.5MB |
| 32x32 | 0.8s | 2.0MB |
| 64x64 | 1.8s | 8.0MB |
| 128x128 | 4.5s | 32.0MB |

---

### LODManager

**Purpose:** Manage level of detail transitions based on camera distance.

**Class Signature:**
```csharp
public class LODManager : MonoBehaviour
```

**Key Properties:**

| Property | Type | Default | Range | Description |
|----------|------|---------|-------|-------------|
| lodLevels | LODLevel[] | 4 levels | 1-8 | Configured LOD levels |
| updateInterval | float | 0.1f | 0.01-1.0 | Update frequency (seconds) |
| hysteresis | float | 0.1f | 0.0-0.3 | Transition overlap |
| enableCrossfade | bool | true | - | Smooth LOD transitions |
| crossfadeDuration | float | 0.3f | 0.1-1.0 | Fade duration (seconds) |

**Key Methods:**

```csharp
// Configure LOD levels
public void ConfigureLODLevels(LODLevel[] levels)

// Update LOD based on camera
public void UpdateLOD(Transform camera)

// Force specific LOD
public void ForceLOD(int level)

// Get current LOD
public int GetCurrentLOD()

// Enable/disable LOD
public void SetEnabled(bool enabled)
```

**LODLevel Structure:**

```csharp
public class LODLevel
{
    public int level;                    // LOD level number
    public float switchDistance;         // Camera distance threshold
    public int meshResolution;           // Vertices per tile edge
    public float textureResolution;      // Texture scale multiplier
    public bool enableCulling;           // Backface culling
    public float hysteresis;             // Overlap with next level
}
```

**Configuration Examples:**

**High-End Mobile:**
```csharp
new LODLevel { level = 0, switchDistance = 15, meshResolution = 33, textureResolution = 1.0f },
new LODLevel { level = 1, switchDistance = 40, meshResolution = 17, textureResolution = 0.5f },
new LODLevel { level = 2, switchDistance = 80, meshResolution = 9, textureResolution = 0.25f },
new LODLevel { level = 3, switchDistance = 150, meshResolution = 5, textureResolution = 0.125f }
```

**Low-End Mobile:**
```csharp
new LODLevel { level = 0, switchDistance = 10, meshResolution = 17, textureResolution = 0.5f },
new LODLevel { level = 1, switchDistance = 25, meshResolution = 9, textureResolution = 0.25f },
new LODLevel { level = 2, switchDistance = 50, meshResolution = 5, textureResolution = 0.125f }
```

---

### SplatmapOptimizer

**Purpose:** Optimize terrain texture blending for mobile platforms.

**Class Signature:**
```csharp
public class SplatmapOptimizer : MonoBehaviour
```

**Key Properties:**

| Property | Type | Default | Range | Description |
|----------|------|---------|-------|-------------|
| compressionFormat | TextureCompressionFormat | ASTC_6x6 | - | Compression type |
| compressionQuality | int | 50 | 0-100 | Quality level |
| enableMipmaps | bool | true | - | Generate mipmaps |
| filterMode | FilterMode | Bilinear | - | Texture filtering |

**Key Methods:**

```csharp
// Optimize splatmap
public Texture2D OptimizeSplatmap(Texture2D source)

// Set compression format
public void SetCompressionFormat(TextureCompressionFormat format)

// Get optimization stats
public SplatmapStats GetStats()

// Configure quality
public void SetQuality(int quality)

// Generate splatmap from heightmap
public Texture2D GenerateSplatmap(Texture2D heightmap, TerrainLayer[] layers)
```

**SplatmapStats Structure:**

```csharp
public class SplatmapStats
{
    public float totalMemoryMB;           // Total memory usage
    public float compressionRatio;        // Size reduction ratio
    public int drawCalls;                 // Draw calls from splatmap
    public int currentLOD;                // Active LOD level
    public float generationTime;          // Optimization time
}
```

**Supported Formats:**

| Format | Bits/Pixel | Quality | Platform | Memory (512x512) |
|--------|------------|---------|----------|------------------|
| ASTC 6x6 | 6 | Excellent | Android/iOS | 0.19 MB |
| ASTC 4x4 | 8 | Excellent | Android/iOS | 0.25 MB |
| ETC2 | 4 | Good | Android | 0.125 MB |
| PVRTC 4bpp | 4 | Moderate | iOS | 0.125 MB |
| RGBA32 | 32 | Lossless | All | 1.0 MB |

---

### TerrainChunk

**Purpose:** Manage terrain chunking and streaming.

**Class Signature:**
```csharp
public class TerrainChunk : MonoBehaviour
```

**Key Properties:**

| Property | Type | Default | Range | Description |
|----------|------|---------|-------|-------------|
| chunkSize | int | 64 | 16-256 | Chunk size in tiles |
| loadDistance | float | 200 | 50-500 | Load radius (units) |
| unloadDistance | float | 250 | 75-600 | Unload radius (units) |
| updateInterval | float | 0.5 | 0.1-2.0 | Check interval (seconds) |
| preloadRadius | int | 2 | 0-4 | Preload chunk buffer |
| enableOcclusionCulling | bool | true | - | Enable occlusion |

**Key Methods:**

```csharp
// Load chunk data
public void LoadChunk(ChunkData data)

// Unload and release memory
public void UnloadChunk()

// Check visibility
public bool IsVisible(Vector3 cameraPosition, float viewDistance)

// Rebuild mesh
public void RebuildMesh()

// Get chunk bounds
public Bounds GetBounds()

// Stream based on camera
public void UpdateStreaming(Transform camera)
```

**ChunkData Structure:**

```csharp
public class ChunkData
{
    public Vector2Int gridPosition;       // Grid coordinates
    public TileData[,] tiles;             // Tile data array
    public float[,] heightmap;            // Heightmap data
    public bool isLoaded;                 // Load status
    public long memoryUsage;              // Memory in bytes
}
```

---

## Data Structures

### TileData

```csharp
public class TileData
{
    public string id;                     // Tile identifier
    public string name;                   // Human-readable name
    public TileType type;                 // Terrain type
    public float weight;                  // Placement probability
    public Connectors connectors;         // N/E/S/W connectors
    public string heightmap;              // Heightmap reference
    public string[] variations;           // Visual variations
    public string[] tags;                 // Gameplay tags
    public int rotation;                  // Applied rotation (0, 90, 180, 270)
}
```

### Connectors

```csharp
public class Connectors
{
    public string north;
    public string east;
    public string south;
    public string west;

    // Rotate connectors
    public Connectors Rotated(int degrees)
    {
        Connectors result = new Connectors();
        switch (degrees)
        {
            case 90:   result.north = west;   result.east = north;   result.south = east;   result.west = south;   break;
            case 180:  result.north = south;  result.east = west;    result.south = north;  result.west = east;    break;
            case 270:  result.north = east;   result.east = south;   result.south = west;   result.west = north;   break;
            default:   result = this; break;
        }
        return result;
    }
}
```

### TileLibrary

```csharp
public class TileLibrary
{
    public string version;
    public string format;
    public int gridSize;
    public bool rotationalSymmetry;
    public int[] supportedRotations;
    public TileData[] tiles;
    public Dictionary<string, string[]> connectorRules;
    public Constraints constraints;
}
```

### Constraints

```csharp
public class Constraints
{
    public int maxSameAdjacency;          // Max adjacent identical tiles
    public int minDiversity;              // Minimum tile variety
    public RangeFloat waterCoverage;      // Water tile percentage range
    public RangeFloat terrainVariance;    // Height variation range
    public RangeFloat structureDensity;   // Structure placement density
}

public class RangeFloat
{
    public float min;
    public float max;
}
```

---

## Performance Requirements

### Minimum Requirements

**Device:**
- CPU: Quad-core 1.8 GHz+
- GPU: OpenGL ES 3.0 / Metal support
- RAM: 2GB minimum
- OS: Android 7.0+ / iOS 12.0+

**Unity:**
- Version: 2022.3 LTS or later
- Scripting Backend: IL2CPP recommended
- API Compatibility: .NET Standard 2.1

### Performance Targets

| Metric | Target | Measurement Method |
|--------|--------|-------------------|
| Draw Calls | <200 | Unity Profiler |
| Memory Usage | <15MB | Profiler.Memory |
| Frame Rate | 60+ FPS | Time.deltaTime |
| Generation Time | <2s | Stopwatch |
| Chunk Load Time | <100ms | Manual timing |

### Benchmark Results (Mid-Range Device)

**Test Configuration:**
- Grid: 64x64 tiles
- Device: Google Pixel 7
- Unity: 2022.3.10f1
- Scene: Single terrain camera

**Results:**
- Draw Calls: 127
- Memory: 11.8 MB
- FPS: 62 average (min 55)
- Generation: 1.2s
- Chunk Load: 78ms average

---

## API Reference

### Core Namespace

```
MobileTerrainForge
├── Core
│   ├── WFCSolver
│   ├── TileData
│   ├── TileLibrary
│   └── Connectors
├── Performance
│   ├── LODManager
│   ├── LODLevel
│   └── SplatmapOptimizer
├── Streaming
│   ├── TerrainChunk
│   ├── ChunkData
│   └── ChunkManager
└── Editor
    ├── TerrainEditorWindow
    ├── TileLibraryEditor
    └── PreviewRenderer
```

### Event System

```csharp
// Terrain generation events
public class WFCSolver : MonoBehaviour
{
    public event Action<int, int> OnTilePlaced;         // (x, y)
    public event Action OnGenerationStart;
    public event Action OnGenerationComplete;
    public event Action<string> OnError;                 // error message
    public event Action<float> OnProgress;               // 0-1
}

// LOD transition events
public class LODManager : MonoBehaviour
{
    public event Action<int, int> OnLODChanged;          // (oldLOD, newLOD)
    public event Action<int> OnChunkLODChanged;          // chunkIndex
}

// Chunk streaming events
public class TerrainChunk : MonoBehaviour
{
    public event Action<Vector2Int> OnChunkLoaded;       // gridPosition
    public event Action<Vector2Int> OnChunkUnloaded;
}
```

---

## File Formats

### Tile Library (JSON)

**Schema:**
```json
{
  "$schema": "https://mobileterrainforge.com/schema/tile-library-v1.json",
  "version": "1.0",
  "format": "WFC-TILE-LIBRARY",
  "gridSize": 5,
  "rotationalSymmetry": true,
  "supportedRotations": [0, 90, 180, 270],
  "tiles": [...],
  "connectorRules": {...},
  "constraints": {...}
}
```

### Heightmap (Serialized)

**Format Options:**
1. **JSON:** Full metadata, easy parsing
2. **Binary:** Compact, fast loading
3. **RAW:** Interoperable with other tools

**JSON Example:**
```json
{
  "metadata": {
    "version": "1.0",
    "resolution": "100x100",
    "heightRange": [0.0, 1.0]
  },
  "data": {
    "width": 100,
    "height": 100,
    "values": [0.1, 0.12, 0.11, ...]
  }
}
```

---

## Compatibility

### Unity Versions

| Unity Version | Supported | Notes |
|---------------|-----------|-------|
| 2021.3 LTS | ✅ | Legacy support |
| 2022.3 LTS | ✅ | Recommended |
| 2023.1+ | ✅ | Full support |

### Render Pipelines

| Pipeline | Status | Notes |
|----------|--------|-------|
| Built-in | ✅ | Full support |
| URP | ✅ | With custom shaders |
| HDRP | ⚠️ | Not optimized for mobile |

### Platforms

| Platform | Minimum | Recommended |
|----------|---------|-------------|
| Android | API 24 | API 30+ |
| iOS | 12.0 | 15+ |
| Windows | 10 | 11 |
| macOS | 10.15 | 13+ |

---

## Security Considerations

### Data Validation

```csharp
// Validate tile library on load
public bool ValidateLibrary(TileLibrary library)
{
    // Check required fields
    if (library.tiles == null || library.tiles.Length == 0)
        return false;

    // Validate each tile
    foreach (var tile in library.tiles)
    {
        if (string.IsNullOrEmpty(tile.id))
            return false;

        if (tile.connectors == null)
            return false;

        if (tile.weight <= 0)
            return false;
    }

    // Check connector rules
    foreach (var rule in library.connectorRules)
    {
        if (rule.Value == null || rule.Value.Length == 0)
            return false;
    }

    return true;
}
```

### Safe File Operations

```csharp
// Safe file loading with exception handling
public TileLibrary LoadLibrarySafely(string path)
{
    try
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        TileLibrary library = JsonUtility.FromJson<TileLibrary>(json);

        if (!ValidateLibrary(library))
        {
            Debug.LogError($"Invalid library format: {path}");
            return null;
        }

        return library;
    }
    catch (Exception e)
    {
        Debug.LogError($"Failed to load library: {e.Message}");
        return null;
    }
}
```

---

## Testing

### Unit Tests

```csharp
[TestFixture]
public class WFCSolverTests
{
    [Test]
    public void Initialize_WithValidLibrary_ReturnsTrue()
    {
        WFCSolver solver = new WFCSolver();
        bool result = solver.Initialize("valid_library.json");
        Assert.IsTrue(result);
    }

    [Test]
    public void Generate_WithValidParameters_CreatesGrid()
    {
        WFCSolver solver = CreateTestSolver();
        solver.gridWidth = 32;
        solver.gridHeight = 32;
        solver.Generate(12345);

        TileData[,] grid = solver.GetGrid();
        Assert.AreEqual(32, grid.GetLength(0));
        Assert.AreEqual(32, grid.GetLength(1));
    }
}
```

### Integration Tests

```csharp
[TestFixture]
public class TerrainIntegrationTests
{
    [UnityTest]
    public IEnumerator FullTerrainGeneration_CompletesWithinTime()
    {
        GameObject terrain = new GameObject();
        terrain.AddComponent<WFCSolver>();
        terrain.AddComponent<LODManager>();
        terrain.AddComponent<SplatmapOptimizer>();

        WFCSolver solver = terrain.GetComponent<WFCSolver>();
        solver.Initialize("test_library.json");
        solver.gridWidth = 64;
        solver.gridHeight = 64;

        float startTime = Time.realtimeSinceStartup;
        solver.Generate();
        yield return new WaitUntil(() => solver.GetStatus() == SolverStatus.Complete);
        float duration = Time.realtimeSinceStartup - startTime;

        Assert.Less(duration, 2.0f, $"Generation took {duration:F3}s");
    }
}
```

---

## Version History

### v1.0 (2026-02-19)

**Initial Release:**
- Core WFC solver implementation
- LOD management system
- Splatmap optimization
- Chunk streaming
- Editor tools
- Sample tile library (25 tiles)
- Technical documentation

---

## Future Roadmap

### Planned Features (v1.1)

- Procedural texture generation
- Dynamic terrain modification
- Multi-biome support
- AI-assisted tile creation
- VR/AR platform support

### Planned Features (v1.2)

- Real-time terrain editing
- Collision mesh optimization
- Navigation mesh generation
- Water simulation
- Vegetation placement system

---

## Glossary

| Term | Definition |
|------|------------|
| WFC | Wave Function Collapse - constraint-based generation algorithm |
| LOD | Level of Detail - rendering optimization technique |
| Splatmap | Texture control map for terrain blending |
| Chunk | Subdivision of terrain for streaming |
| Connector | Edge type defining tile compatibility |
| ASTC | Adaptive Scalable Texture Compression |

---

**Document Control:**

- **Author:** Mobile Terrain Forge Team
- **Reviewer:** Technical Lead
- **Approval Date:** 2026-02-19
- **Next Review:** 2026-05-19

---

**Copyright © 2026 Mobile Terrain Forge. All rights reserved.**
