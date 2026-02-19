# Troubleshooting Guide

**Version:** 1.0
**Last Updated:** 2026-02-19

---

## Overview

This guide addresses common issues encountered when integrating and using Mobile Terrain Forge. Solutions are organized by category with step-by-step troubleshooting procedures.

---

## Quick Reference

| Issue | Likely Cause | Quick Fix |
|-------|--------------|-----------|
| Solver fails to converge | Contradictory constraints | Relax rules or increase iterations |
| Low FPS | Too many draw calls | Enable LOD and reduce resolution |
| Memory exceeded | Large textures | Enable compression (ASTC) |
| Tiles don't connect | Connector mismatch | Verify connector names match |
| Visual popping | LOD transition too abrupt | Increase hysteresis overlap |
| Build fails | Missing dependencies | Verify Unity version and APIs |
| Chunk loading slow | Large chunk size | Reduce chunkSize or increase streaming interval |

---

## 1. Solver Issues

### Issue: Solver Fails to Converge

**Symptoms:**
- Generation stops at 0% or hangs
- "Contradiction detected" error in Console
- Solver reaches max iterations without completing

**Diagnosis:**

```csharp
// Enable solver debug mode
WFCSolver solver = GetComponent<WFCSolver>();
solver.debugMode = true;
solver.logIterations = true;

// Check for contradictions
bool hasContra = solver.DetectContradictions(out List<string> contradictions);
if (hasContra)
{
    Debug.LogError("Contradictions found:");
    foreach (string c in contradictions)
        Debug.LogError($"  - {c}");
}
```

**Solutions:**

1. **Increase Max Iterations:**
```csharp
solver.maxIterations = 5000;  // Default is 1000
```

2. **Relax Constraints:**
```json
// In tile_library.json
"constraints": {
  "maxSameAdjacency": 8,      // Increase from 4
  "minDiversity": 1,          // Decrease from 3
  "waterCoverage": {"min": 0.01, "max": 0.5}  // Widen range
}
```

3. **Check Connector Rules:**
```csharp
// Verify all connectors have compatible partners
var allConnectors = solver.GetAllConnectorTypes();
foreach (var connector in allConnectors)
{
    var compatibles = solver.GetCompatibleConnectors(connector);
    if (compatibles.Count == 0)
    {
        Debug.LogWarning($"Connector '{connector}' has no compatible partners!");
    }
}
```

4. **Simplify Tile Library:**
- Remove tiles with unique connectors that have no matches
- Reduce number of connector types
- Group similar connectors

5. **Use Fallback Generation:**
```csharp
// Enable fallback to simpler algorithm if WFC fails
solver.enableFallback = true;
solver.fallbackIterations = 100;
```

---

### Issue: Generation Takes Too Long

**Symptoms:**
- Generation time >5 seconds
- Game feels unresponsive during generation
- Frame drops during generation

**Solutions:**

1. **Use Chunked Generation:**
```csharp
solver.generateMode = WFCGenerateMode.Chunked;
solver.chunkSize = 16;  // Generate in 16x16 chunks
solver.updateInterval = 0.1f;  // Update every 100ms
```

2. **Reduce Grid Size:**
```csharp
// Start with smaller grid, expand as needed
solver.gridWidth = 32;  // Instead of 64
solver.gridHeight = 32;
```

3. **Disable Rotations:**
```csharp
solver.enableRotations = false;  // Reduces solver complexity by 4x
```

4. **Use Pre-computed Solutions:**
```csharp
// Save generated terrain
string savePath = "Assets/Terrains/precomputed_32x32.json";
solver.SaveTerrain(savePath);

// Load on startup
solver.LoadTerrain(savePath);
```

---

### Issue: Generated Terrain Looks Repetitive

**Symptoms:**
- Same tile patterns repeat
- Lack of variety in terrain features
- Visible grid patterns

**Solutions:**

1. **Add Tile Variations:**
```json
{
  "id": "T001",
  "name": "Flat Ground",
  "variations": ["T001_var0", "T001_var1", "T001_var2", "T001_var3", "T001_var4"]
}
```

2. **Adjust Weights:**
```json
{
  "id": "T001",
  "weight": 30,  // Reduce from 50 to allow other tiles
  "id": "T009",
  "weight": 40   // Increase forest frequency
}
```

3. **Enable Rotations:**
```csharp
solver.enableRotations = true;
solver.supportedRotations = new[] { 0, 90, 180, 270 };
```

4. **Increase Tile Library Size:**
```csharp
// Add 3-5 tiles per terrain type
// Minimum 25 tiles total for good variety
```

5. **Use Jitter on Placement:**
```csharp
solver.positionJitter = 0.5f;  // Add small random offset to tile positions
```

---

## 2. Performance Issues

### Issue: Low Frame Rate

**Symptoms:**
- FPS drops below 30
- Stuttering during camera movement
- Frame spikes when loading chunks

**Diagnosis:**

```csharp
// Use Unity Profiler
Window > Analysis > Profiler

// Check:
// - CPU Usage (target: <16.67ms per frame for 60 FPS)
// - GPU Usage (target: <10ms per frame)
// - Rendering (count draw calls)
// - Memory (target: <15MB)
```

**Solutions:**

1. **Enable LOD System:**
```csharp
LODManager lodManager = GetComponent<LODManager>();
LODLevel[] mobileLOD = new LODLevel[]
{
    new LODLevel { level = 0, switchDistance = 12, meshResolution = 25 },
    new LODLevel { level = 1, switchDistance = 35, meshResolution = 13 },
    new LODLevel { level = 2, switchDistance = 80, meshResolution = 7 },
    new LODLevel { level = 3, switchDistance = 150, meshResolution = 5 }
};
lodManager.ConfigureLODLevels(mobileLOD);
```

2. **Reduce Draw Calls:**
```csharp
// Enable GPU instancing
Material terrainMat = Resources.Load<Material>("Materials/Terrain");
terrainMat.enableInstancing = true;

// Combine similar materials
int materialCount = terrain.terrainData.terrainLayers.Length;
if (materialCount > 8)
{
    Debug.LogWarning($"Too many materials: {materialCount}. Target: <8");
}
```

3. **Optimize Textures:**
```csharp
SplatmapOptimizer optimizer = GetComponent<SplatmapOptimizer>();
optimizer.SetCompressionFormat(TextureCompressionFormat.ASTC_6x6);
```

4. **Reduce Mesh Resolution:**
```csharp
// Lower resolution for distant LOD levels
LODLevel[] lowResLOD = new LODLevel[]
{
    new LODLevel { level = 0, switchDistance = 15, meshResolution = 17 },
    new LODLevel { level = 1, switchDistance = 40, meshResolution = 9 },
    new LODLevel { level = 2, switchDistance = 80, meshResolution = 5 }
};
```

5. **Enable Occlusion Culling:**
```csharp
TerrainChunk chunk = GetComponent<TerrainChunk>();
chunk.enableOcclusionCulling = true;
```

---

### Issue: Memory Exceeds 15MB

**Symptoms:**
- Memory usage >15MB in Profiler
- Frequent garbage collections
- Crashes on low-memory devices

**Diagnosis:**

```csharp
// Monitor memory usage
void Update()
{
    long memory = System.GC.GetTotalMemory(false);
    float memoryMB = memory / (1024f * 1024f);

    if (memoryMB > 15.0f)
    {
        Debug.LogWarning($"Memory exceeded: {memoryMB:F2}MB");
        AnalyzeMemoryUsage();
    }
}

void AnalyzeMemoryUsage()
{
    // Check texture sizes
    Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
    foreach (var tex in textures)
    {
        int texSize = tex.width * tex.height * 4;  // RGBA32
        float texSizeMB = texSize / (1024f * 1024f);
        Debug.Log($"Texture '{tex.name}': {texSizeMB:F2}MB");
    }

    // Check mesh sizes
    Mesh[] meshes = Resources.FindObjectsOfTypeAll<Mesh>();
    foreach (var mesh in meshes)
    {
        int vertexCount = mesh.vertexCount;
        int triangleCount = mesh.triangles.Length / 3;
        Debug.Log($"Mesh '{mesh.name}': {vertexCount} verts, {triangleCount} tris");
    }
}
```

**Solutions:**

1. **Enable Texture Compression:**
```csharp
// All textures should use ASTC 6x6
TextureImporter importer = AssetImporter.GetAtPath("Assets/Textures/splatmap.png") as TextureImporter;
importer.textureCompression = TextureImporterCompression.Compressed;
importer.compressionQuality = 50;  // Balance quality and size
importer.SaveAndReimport();
```

2. **Reduce Splatmap Resolution:**
```csharp
// Use 512x512 instead of 1024x1024
int splatmapResolution = 512;
Terrain.activeTerrain.terrainData.alphamapResolution = splatmapResolution;
```

3. **Unload Distant Chunks:**
```csharp
// Reduce load distance
TerrainChunk chunk = GetComponent<TerrainChunk>();
chunk.loadDistance = 150;  // Reduce from 200
chunk.unloadDistance = 180;  // Reduce from 220
```

4. **Use Object Pooling:**
```csharp
// Reuse tile objects instead of instantiating
TilePool pool = GetComponent<TilePool>();
GameObject tile = pool.GetTile();
// Use tile...
pool.ReturnTile(tile);
```

5. **Reduce Heightmap Resolution:**
```csharp
// Use lower resolution heightmaps
int heightmapResolution = 513;  // Instead of 1025
Terrain.activeTerrain.terrainData.heightmapResolution = heightmapResolution;
```

---

### Issue: Draw Calls Too High

**Symptoms:**
- Draw calls >200 in Profiler
- CPU bottleneck on rendering
- High GPU usage

**Solutions:**

1. **Enable GPU Instancing:**
```csharp
// On materials
Material terrainMat = Resources.Load<Material>("Materials/Terrain");
terrainMat.enableInstancing = true;

// On prefabs
MeshRenderer renderer = tilePrefab.GetComponent<MeshRenderer>();
renderer.enableInstancing = true;
```

2. **Use Texture Atlasing:**
```csharp
// Combine tile variations into single texture
Texture2D[] variations = Resources.LoadAll<Texture2D>("Tiles/Variations");
Texture2D atlas = CreateTextureAtlas(variations, 64);
```

3. **Batch Similar Objects:**
```csharp
// Static batching for non-moving terrain
StaticBatchingUtility.Combine(tileObjects, terrainParent);
```

4. **Reduce Material Variants:**
```csharp
// Limit to 8 materials max
if (terrain.terrainData.terrainLayers.Length > 8)
{
    Debug.LogError("Too many terrain layers! Max: 8");
}
```

5. **Use LOD to Reduce Complexity:**
```csharp
// Distant chunks use simplified materials
LODLevel[] optimizedLOD = new LODLevel[]
{
    new LODLevel { level = 0, textureResolution = 1.0f },
    new LODLevel { level = 1, textureResolution = 0.5f },
    new LODLevel { level = 2, textureResolution = 0.25f }
};
```

---

## 3. Visual Issues

### Issue: Visual Popping During LOD Transitions

**Symptoms:**
- Sudden geometry changes when camera moves
- Texture detail snaps between LOD levels
- distracting visual artifacts

**Solutions:**

1. **Increase Hysteresis:**
```csharp
// Add overlap between LOD levels to prevent rapid switching
LODLevel[] hysteresisLOD = new LODLevel[]
{
    new LODLevel { level = 0, switchDistance = 12, hysteresis = 0.15f },  // 15% overlap
    new LODLevel { level = 1, switchDistance = 35, hysteresis = 0.12f },
    new LODLevel { level = 2, switchDistance = 80, hysteresis = 0.10f }
};
```

2. **Use Crossfade Transitions:**
```csharp
// Smoothly blend between LOD levels
lodManager.enableCrossfade = true;
lodManager.crossfadeDuration = 0.3f;  // 300ms fade
```

3. **Adjust Switch Distances:**
```csharp
// Space LOD levels further apart
LODLevel[] spacedLOD = new LODLevel[]
{
    new LODLevel { level = 0, switchDistance = 10 },
    new LODLevel { level = 1, switchDistance = 40 },  // 30 units gap
    new LODLevel { level = 2, switchDistance = 90 },  // 50 units gap
    new LODLevel { level = 3, switchDistance = 180 }  // 90 units gap
};
```

4. **Use Geomorphing:**
```csharp
// Animate vertex positions during transition
lodManager.enableGeomorphing = true;
lodManager.geomorphingDuration = 0.5f;
```

---

### Issue: Texture Seams Between Tiles

**Symptoms:**
- Visible lines at tile boundaries
- Color mismatches at edges
- Normal discontinuities

**Solutions:**

1. **Use Texture Padding:**
```csharp
// Add 2-pixel border to each tile
Texture2D paddedTexture = PadTexture(sourceTexture, 2);
```

2. **Match UV Coordinates:**
```csharp
// Ensure adjacent tiles share exact UV values at edges
// Use snap-to-grid on UV coordinates
uvs[i].x = Mathf.Round(uvs[i].x * 1024) / 1024;
uvs[i].y = Mathf.Round(uvs[i].y * 1024) / 1024;
```

3. **Use Seamless Textures:**
```csharp
// Mark textures as seamless
TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
importer.wrapMode = TextureWrapMode.Clamp;
importer.mipmapFilter = TextureImporterMipFilter.BoxFilter;
```

4. **Apply Blending at Edges:**
```csharp
// Blend overlapping edge pixels
BlendTileEdges(tile1, tile2, edgeWidth: 2);
```

---

### Issue: Tiles Don't Connect Properly

**Symptoms:**
- Gaps between tiles
- Mismatched edges (e.g., cliff meets flat ground)
- Impossible geometry

**Diagnosis:**

```csharp
// Enable connector visualization
WFCSolver solver = GetComponent<WFCSolver>();
solver.debugMode = true;
solver.showConnectors = true;

// Check specific tile connections
TileData tile = solver.GetTileAt(5, 5);
Debug.Log($"Tile [{5},{5}]: {tile.id}");
Debug.Log($"  N: {tile.connectors.north}");
Debug.Log($"  E: {tile.connectors.east}");
Debug.Log($"  S: {tile.connectors.south}");
Debug.Log($"  W: {tile.connectors.west}");
```

**Solutions:**

1. **Verify Connector Names:**
```json
// Check for typos
"connectors": {
  "north": "slope_up",  // Correct
  "east": "slopr_up",   // Typo! Should be "slope_up"
}
```

2. **Update Connector Rules:**
```json
"connectorRules": {
  "slope_up": ["slope_up", "peak", "steep_down"],
  "flat": ["flat", "slope_down", "water_shore"]
}
```

3. **Check Tile Placement:**
```csharp
// Verify tiles are placed on grid correctly
solver.gridSnapEnabled = true;
solver.gridCellSize = 32.0f;
```

4. **Use Symmetry:**
```json
{
  "id": "T002",
  "rotationalSymmetry": true,  // Ensure tiles rotate correctly
  "supportedRotations": [0, 90, 180, 270]
}
```

---

## 4. Platform-Specific Issues

### Issue: Android Build Fails

**Symptoms:**
- Build errors during export
- Crashes on launch
- Graphics API errors

**Solutions:**

1. **Check Unity Version:**
```
Minimum: Unity 2022.3 LTS
Recommended: Unity 2022.3.10f1 or later
```

2. **Verify Graphics API:**
```
Player Settings > Other Settings > Graphics APIs:
- OpenGL ES 3.1+
- Vulkan (optional)
Remove: OpenGL ES 2.0, OpenGL ES 3.0
```

3. **Set Scripting Backend:**
```
Player Settings > Other Settings > Scripting Backend: IL2CPP
API Compatibility Level: .NET Standard 2.1
```

4. **Enable Multithreaded Rendering:**
```
Player Settings > Other Settings > Multithreaded Rendering: Enabled
```

5. **Check Android Manifest:**
```xml
<uses-feature android:glEsVersion="0x00030001" />
<uses-sdk android:minSdkVersion="24" android:targetSdkVersion="33" />
```

---

### Issue: iOS Build Fails

**Symptoms:**
- Xcode build errors
- Metal shader compilation errors
- App rejected by App Store

**Solutions:**

1. **Check Metal Support:**
```
Player Settings > iOS Settings > Metal API Support: Enabled
```

2. **Set Architecture:**
```
Player Settings > iOS Settings > Architecture: ARM64
```

3. **Enable Metal API Validation:**
```
Only for debug builds to catch shader errors
```

4. **Configure Texture Compression:**
```
Player Settings > iOS Settings > Texture Compression:
- ASTC (recommended)
- PVRTC (fallback)
```

5. **Update Info.plist:**
```xml
<key>UIRequiredDeviceCapabilities</key>
<array>
    <string>armv7</string>
    <string>metal</string>
</array>
```

---

### Issue: Performance Worse on Android vs iOS

**Symptoms:**
- Lower FPS on Android
- Stuttering on Android
- Different visual quality

**Solutions:**

1. **Use Platform-Specific Settings:**
```csharp
void ConfigureForPlatform()
{
    if (Application.platform == RuntimePlatform.Android)
    {
        // Android-specific optimizations
        GetComponent<LODManager>().ConfigureLODLevels(androidLOD);
        GetComponent<SplatmapOptimizer>().SetCompressionFormat(TextureCompressionFormat.ASTC_6x6);
    }
    else if (Application.platform == RuntimePlatform.IPhonePlayer)
    {
        // iOS-specific settings
        GetComponent<LODManager>().ConfigureLODLevels(iosLOD);
    }
}
```

2. **Reduce Texture Quality on Android:**
```csharp
if (Application.platform == RuntimePlatform.Android)
{
    QualitySettings.masterTextureLimit = 1;  // Half resolution
}
```

3. **Enable GPU Instancing:**
```csharp
// More critical on Android
Material terrainMat = Resources.Load<Material>("Materials/Terrain");
terrainMat.enableInstancing = true;
```

4. **Adjust LOD Distances:**
```csharp
// Android devices vary widely - use conservative settings
LODLevel[] androidLOD = new LODLevel[]
{
    new LODLevel { level = 0, switchDistance = 10 },  // Closer than iOS
    new LODLevel { level = 1, switchDistance = 30 },
    new LODLevel { level = 2, switchDistance = 60 }
};
```

---

## 5. Editor Issues

### Issue: Terrain Not Generating in Play Mode

**Symptoms:**
- Clicking "Generate" does nothing
- Inspector shows "Generating..." forever
- No terrain appears

**Solutions:**

1. **Check Tile Library Path:**
```csharp
WFCSolver solver = GetComponent<WFCSolver>();
Debug.Log($"Tile Library: {solver.tileLibraryPath}");

// Verify file exists
if (!File.Exists(solver.tileLibraryPath))
{
    Debug.LogError($"Tile library not found: {solver.tileLibraryPath}");
}
```

2. **Verify Resources Folder:**
```
tile_library.json must be in:
Assets/MobileTerrainForge/Resources/Data/tile_library.json

Then load with:
solver.Initialize("MobileTerrainForge/Data/tile_library");
```

3. **Check Console for Errors:**
```
Look for:
- JSON parsing errors
- Missing file errors
- Null reference exceptions
```

4. **Reset Solver:**
```csharp
solver.Reset();
solver.Initialize("path/to/tile_library.json");
solver.Generate();
```

---

### Issue: Editor Window Doesn't Open

**Symptoms:**
- TerrainEditorWindow not accessible
- Menu item missing
- Window opens blank

**Solutions:**

1. **Check Menu Item:**
```csharp
// In TerrainEditorWindow.cs
[MenuItem("Tools/Mobile Terrain Forge/Terrain Editor")]
public static void ShowWindow()
{
    GetWindow<TerrainEditorWindow>("Mobile Terrain Forge");
}
```

2. **Reimport Script:**
```
Right-click on TerrainEditorWindow.cs > Reimport
```

3. **Check Unity Compilation:**
```
Wait for Unity to finish compiling scripts
Check Console for compilation errors
```

4. **Force Window Open:**
```csharp
// In Console, run:
TerrainEditorWindow.ShowWindow();
```

---

## 6. Common Errors

### Error: "IndexOutOfRangeException" in WFC Solver

**Cause:** Grid coordinates out of bounds

**Solution:**
```csharp
// Check grid bounds
if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
{
    Debug.LogError($"Tile coordinates out of bounds: ({x}, {y})");
    return null;
}
```

---

### Error: "NullReferenceException" in LOD Manager

**Cause:** Missing terrain component

**Solution:**
```csharp
// Verify terrain exists
if (Terrain.activeTerrain == null)
{
    Debug.LogError("No active terrain in scene!");
    return;
}
```

---

### Error: "OutOfMemoryException"

**Cause:** Too many objects or large textures

**Solution:**
```csharp
// Reduce memory usage
1. Enable texture compression
2. Unload distant chunks
3. Reduce texture resolution
4. Use object pooling
```

---

## Getting Help

### Before Contacting Support

1. **Check This Guide:** Review all relevant sections
2. **Enable Debug Mode:** Get detailed logs
3. **Profile Performance:** Use Unity Profiler
4. **Check Unity Version:** Ensure minimum version met
5. **Review Console:** Look for error messages

### Information to Provide

When reporting issues, include:

1. **Unity Version:** e.g., 2022.3.10f1
2. **Platform:** Android/iOS, device model
3. **Asset Version:** From package manifest
4. **Error Messages:** Full stack trace from Console
5. **Reproduction Steps:** How to trigger the issue
6. **Expected Behavior:** What should happen
7. **Actual Behavior:** What actually happens
8. **Screenshots/Video:** Visual evidence of the issue

### Contact Channels

- **Documentation:** Review this guide and integration guide
- **Forum:** Post on Unity Asset Store forum
- **Email:** support@mobileterrainforge.com
- **Discord:** Join community Discord for real-time help

---

## Debug Mode

### Enable Comprehensive Debugging

```csharp
public class DebugMode : MonoBehaviour
{
    void Start()
    {
        // Enable all debug flags
        WFCSolver solver = GetComponent<WFCSolver>();
        solver.debugMode = true;
        solver.logIterations = true;
        solver.showConnectors = true;
        solver.showWeights = true;
        solver.visualizeGrid = true;

        // LOD debug
        LODManager lod = GetComponent<LODManager>();
        lod.debugMode = true;
        lod.showLODLevels = true;
        lod.showSwitchDistances = true;

        // Memory debug
        Debug.Log($"Initial Memory: {System.GC.GetTotalMemory(false) / (1024f * 1024f):F2}MB");
    }

    void Update()
    {
        // Real-time performance monitoring
        float fps = 1.0f / Time.deltaTime;
        long memory = System.GC.GetTotalMemory(false);

        if (Time.frameCount % 60 == 0)  // Every second
        {
            Debug.Log($"FPS: {fps:F1} | Memory: {memory / (1024f * 1024f):F2}MB");
        }
    }
}
```

---

**Copyright © 2026 Mobile Terrain Forge. All rights reserved.**
