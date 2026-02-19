# QUICK START GUIDE

**Import to Unity (2 minutes):**

1. Create new Unity 3D project (URP, 2022.3+)
2. Create `Assets/Scripts/` folder
3. Copy all `.cs` files into that folder
4. Unity will auto-compile

**Generate Terrain (5 minutes):**

```csharp
// In your script
MobileTerrainForge.WFCSolver solver = new MobileTerrainForge.WFCSolver(tiles, width, height);
solver.Solve();

// Get terrain data
int[] terrain = solver.GetTerrain();
// Generate mesh from terrain data
```

**Enable LOD (3 minutes):**

1. Add `LODManager` component to empty GameObject
2. Assign player transform
3. Configure LOD levels (4-level: 1m, 2m, 4m, 8m)
4. Enable frustum culling and GPU instancing

**Full integration guide:** See `docs/integration_guide.md`

**Questions?** Check `docs/troubleshooting.md`

---

*Mobile Terrain Forge — Start generating in 10 minutes*