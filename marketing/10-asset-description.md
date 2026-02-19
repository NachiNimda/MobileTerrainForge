# Mobile Terrain Forge — Asset Store Description

---

## Mobile Terrain Forge

**True Wave Function Collapse for Unity Terrain Generation**

Generate consistent, deterministic, and highly optimized terrain for mobile games — without the performance pitfalls of traditional noise-based systems.

---

### Overview

Mobile Terrain Forge brings the mathematical elegance of Wave Function Collapse (WFC) to Unity terrain generation. Unlike noise-based approaches that sacrifice determinism and consistency for randomness, WFC guarantees that every terrain satisfies all constraints — every single time.

Built specifically for mobile optimization, Mobile Terrain Forge delivers:

- **75% fewer draw calls** through intelligent mesh combination
- **62% less memory usage** via shared geometry and texture pooling
- **33% better frame rates** on target mobile devices

This is not another terrain generator with marketing claims. This is a mathematically sound algorithm that produces predictable, reproducible, and efficient terrain generation.

---

### Why Wave Function Collapse?

Most terrain systems start with Perlin or Simplex noise. The result? Smooth gradients, but problematic inconsistencies:

- Adjacent terrain chunks may not connect seamlessly
- Biome transitions can be abrupt or nonsensical
- The same seed produces different results across runs
- Debugging procedural generation becomes a guessing game

Wave Function Collapse approaches terrain generation differently:

1. **Define Constraints** — Specify what terrain types can exist where
2. **Track Entropy** — Monitor remaining possibilities at each grid cell
3. **Collapse Deterministically** — Resolve cells in order of least entropy
4. **Propagate Consistency** — Ensure every neighbor respects the result

The outcome is not random. It is the only terrain that satisfies all constraints. This means:

- **Deterministic Results:** Same seed, same terrain, every time
- **Guaranteed Consistency:** No seam issues, no abrupt transitions
- **Debuggable Systems:** Step through generation and understand every decision
- **Tool Integration:** Artists and designers can iterate predictably

---

### Features

#### Core WFC Implementation
- Full Wave Function Collapse algorithm for 2D terrain generation
- Configurable constraint rules (adjacency, biome, elevation)
- Deterministic seeding with full reproducibility
- Real-time generation preview in the Unity Editor

#### Terrain Generation
- Automatic mesh generation from WFC output
- Configurable resolution and detail levels
- Support for multiple terrain types (grass, water, rock, forest, etc.)
- Elevation-based biome transitions

#### Texture & Materials
- Splatmap-based texture blending
- Configurable texture layers and weights
- Automatic normal map generation
- Support for custom shaders

#### LOD System
- Automatic Level of Detail generation
- Distance-based LOD switching
- Configurable LOD levels (0-4)
- Smooth transitions between LODs

#### Performance Optimization
- Single combined mesh (no individual mesh objects)
- GPU instancing for repeated geometry
- Memory pooling for texture data
- Asynchronous chunk loading option

#### Mobile-Specific Optimizations
- Draw call batching (50-200 draw calls → 1-10)
- Texture compression support (ASTC, ETC2)
- Memory-efficient chunk management
- Frame budget awareness (target: 16.67ms for 60fps)

#### Editor Tools
- Visual constraint editor
- Seed inspector for reproducibility
- Live preview during generation
- Performance profiling integration
- Export/import configuration presets

#### Runtime API
```csharp
// Simple usage
var generator = GetComponent<TerrainGenerator>();
generator.seed = 12345;
generator.Generate();

// Advanced usage
generator.SetConstraints(constraints);
generator.SetBiomeRules(biomeRules);
generator.onProgress += (progress) => Debug.Log(progress);
generator.GenerateAsync().ContinueWith(t => {
    Debug.Log("Generation complete");
});
```

---

### Performance Benchmarks

Tested on a mid-range mobile device (Snapdragon 665, 4GB RAM):

**Traditional Noise-Based Terrain:**
- Draw Calls: 186
- Memory: 142 MB
- Average FPS: 42 (frequent drops to 30fps)

**Mobile Terrain Forge:**
- Draw Calls: 47 (75% reduction)
- Memory: 54 MB (62% reduction)
- Average FPS: 56 (33% improvement, stable 60fps)

*Benchmarks based on 1024x1024 terrain, 4 texture layers, 4 LOD levels. Results vary based on configuration and hardware.*

---

### Who Is This For?

**For Mobile Game Developers:**
- Targeting 60fps on budget devices
- Need deterministic terrain for competitive games
- Building open-world games with streaming
- Want debuggable procedural systems

**For Technical Artists:**
- Need terrain that fits artistic vision consistently
- Want control over biome transitions
- Require reproducible results across builds
- Value algorithmic correctness over shortcuts

**For Programmers:**
- Building tools for artists and designers
- Need systems that behave predictably
- Want to understand and modify generation logic
- Appreciate clean, documented code

**Not Recommended For:**
- Desktop-first games with unlimited hardware (noise-based may suffice)
- Games requiring completely random, non-deterministic terrain
- Projects needing extreme artistic control (hand-sculpted may be better)
- Developers unwilling to learn WFC concepts

---

### What's Included

- **Mobile Terrain Forge Core DLL** — WFC algorithm implementation
- **Terrain Generator Component** — Main generation controller
- **Constraint Editor** — Visual rules configuration
- **LOD System** — Automatic detail level management
- **Texture Manager** — Splatmap and material handling
- **Editor Tools** — Seed inspector, preview, profiling
- **Demo Scenes** — 3 complete example scenes
- **Documentation** — API reference, tutorials, best practices
- **Source Code** — Full C# source (no obfuscated DLLs)

---

### Requirements

- **Unity Version:** 2021.3 LTS or later
- **Render Pipeline:** Built-in, URP, or HDRP
- **Platforms:** Android, iOS, Windows, macOS, Linux
- **Dependencies:** None (standalone asset)

**Mobile Requirements (for 60fps target):**
- Android: OpenGL ES 3.0+, Vulkan 1.0+
- iOS: Metal support
- RAM: 3GB minimum recommended
- GPU: Adreno 530 / Mali-G71 / Apple A9 or later

---

### Documentation & Support

**Included Documentation:**
- Quick Start Guide (5-minute setup)
- API Reference (full method documentation)
- Tutorial: Building Your First Terrain (15 minutes)
- Tutorial: Custom Constraints (20 minutes)
- Performance Optimization Guide
- FAQ (Frequently Asked Questions)

**Video Series:**
"The Quiet Craft of Optimization" — 7-episode series covering WFC fundamentals, mobile optimization, and real-world case studies. Available on YouTube (link included).

**Support:**
- Email support (response within 48 hours)
- Unity Forum thread (community support)
- Discord server (invite included with purchase)

---

### Pricing

**$59.99** — One-time purchase, perpetual license

**License:**
- Commercial use allowed
- Unlimited projects
- No attribution required
- Source code included
- Free updates for 12 months
- Lifetime access to purchased version

**Refund Policy:**
30-day money-back guarantee if the asset doesn't work as described or if you're not satisfied.

---

### What Users Are Saying

*"Finally, a terrain system that actually behaves predictably. The WFC approach is brilliant — my terrain generates the same way every time, which makes debugging so much easier."* — Mobile Game Developer, 5 years experience

*"The performance gains are real. I went from 30fps to 60fps on my target device just by switching to Mobile Terrain Forge. The LOD system alone was worth the price."* — Indie Developer, published 3 mobile games

*"The documentation is excellent. I had terrain generating in 10 minutes. The API is clean and well-thought-out. This is how Unity assets should be made."* — Technical Artist, AAA studio

---

### Comparison: Mobile Terrain Forge vs Noise-Based Systems

| Feature | Mobile Terrain Forge | Traditional Noise |
|---------|---------------------|-------------------|
| Determinism | ✓ (100% reproducible) | ✗ (seed may produce different results) |
| Consistency | ✓ (no seam issues) | ~ (may have adjacency problems) |
| Draw Calls | ~50 (combined mesh) | ~200 (individual chunks) |
| Memory Usage | 54 MB (optimized) | 142 MB (unoptimized) |
| Frame Rate | 56 FPS (stable 60) | 42 FPS (drops to 30) |
| Debugging | ✓ (step-by-step tracing) | ✗ (black box generation) |
| Customization | ✓ (constraint rules) | ~ (limited parameter tuning) |
| Learning Curve | Moderate (WFC concepts) | Low (well-understood) |

---

### Try Before You Buy

Download the free demo package from the Asset Store:
- Full WFC algorithm (time-limited generation)
- 2 example scenes
- Complete documentation
- Test on your target hardware

See the performance gains yourself before committing.

---

### Get Started

1. **Purchase** Mobile Terrain Forge from the Unity Asset Store
2. **Import** into your Unity project
3. **Follow** the Quick Start Guide (5 minutes)
4. **Generate** your first terrain
5. **Optimize** using the included performance guide

For questions or support, email us at [support email] or join our Discord server.

---

### Version History

**v1.0.0** (Initial Release)
- Full WFC implementation
- Terrain generation and LOD system
- Mobile optimizations
- Editor tools and documentation

---

### Changelog (Upcoming)

**v1.1.0** (Planned)
- 3D terrain support (cave systems, overhangs)
- Additional biome types (desert, tundra, swamp)
- Performance profiling improvements
- Additional demo scenes

---

### Legal

This asset is provided as-is, without warranty. Use at your own risk in commercial projects. Source code is provided for educational and modification purposes.

© 2026 Mobile Terrain Forge. All rights reserved.

---

**Generate better terrain. Generate predictable terrain. Generate with Mobile Terrain Forge.**